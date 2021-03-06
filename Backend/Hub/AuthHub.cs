﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using CommunicationModel;
using BackendModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Backend.Hub
{
    public partial class CornellGoHub
    {
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);

            if (session != null)
            {
                //Add to sessionlog
                var entry = new SessionLogEntry(SessionLogEntryType.LostConnection, session.User.Id.ToString(), DateTime.UtcNow, session.User);
                await Database.SessionLogEntries.AddAsync(entry);
                await Database.SaveChangesAsync();

                session.SignalRId = null;
                await Database.SaveChangesAsync();
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<bool> AttemptRelog(string session)
        {
            UserSession usession = await Database.UserSessions.FromToken(session);
            if (usession != null)
            {
                usession.SignalRId = Context.ConnectionId;
                await Groups.AddToGroupAsync(Context.ConnectionId, usession.User.GroupMember.Group.SignalRId);

                //Add to sessionlog
                var entry = new SessionLogEntry(SessionLogEntryType.Relog, usession.User.Id.ToString(), DateTime.UtcNow, usession.User);
                await Database.SessionLogEntries.AddAsync(entry);

                await Database.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Login(string email, string password)
        {
            Authenticator authenticator = await Database.Authenticators.AsAsyncEnumerable().FirstOrDefaultAsync(e => e.Email == email);
            if (authenticator == null || !VerifyPasswordHash(password, authenticator.Password))
                return false;

            UserSession session = authenticator.User.UserSession ?? new(DateTime.UtcNow, authenticator.User);
            User user = session.User;
            authenticator.User.UserSession = session;

            session.SignalRId = Context.ConnectionId;

            await Groups.AddToGroupAsync(Context.ConnectionId, user.GroupMember.Group.SignalRId);
            await Database.SaveChangesAsync();

            //Add to sessionlog
            var entry = new SessionLogEntry(SessionLogEntryType.Login, session.User.Id.ToString(), DateTime.UtcNow, user);
            await Database.SessionLogEntries.AddAsync(entry);

            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Logout()
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);
            if (session == null)
                return false;

            Database.Remove(session);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, session.User.GroupMember.Group.Id.ToString());

            //Add to sessionlog
            var entry = new SessionLogEntry(SessionLogEntryType.Logout, session.User.Id.ToString(), DateTime.UtcNow, session.User);
            await Database.SessionLogEntries.AddAsync(entry);

            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetSessionToken()
        {
            return (await Database.UserSessions.FromSignalRId(Context.ConnectionId))?.ToToken();
        }

        public async Task<bool> Register(string username, string password, string email, double curLat, double curLong)
        {
            if (await Database.Authenticators.AsAsyncEnumerable().AnyAsync(e => e.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase)))
                return false;

            username = Censor(username);

            User user = new(0, username, email);
            user = (await Database.AddAsync(user)).Entity;

            Authenticator auth = new(email, CreatePasswordHash(password), DateTime.UtcNow, user);

            var newChal = await GroupExtensions.GetNewChallenge(null, Database.Challenges, curLong, curLat, true);

            Group g = new Group(newChal);
            g = (await Database.AddAsync(g)).Entity;
            await Database.SaveChangesAsync();

            user.GroupMember = new GroupMember(true, false, g);
            g.SignalRId = g.Id.ToString();

            await Groups.AddToGroupAsync(Context.ConnectionId, auth.User.GroupMember.Group.Id.ToString());

            //Add to sessionlog
            var entry = new SessionLogEntry(SessionLogEntryType.UserCreated, user.Id.ToString(), DateTime.UtcNow, user);

            await Database.AddAsync(entry);

            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeUsername(string username)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);
            if (session == null)
                return false;

            username = Censor(username);

            User user = session.User;
            string oldUsername = user.Username;
            user.Username = username;

            await Database.SaveChangesAsync();
            await Clients.Group(user.GroupMember.Group.SignalRId).UpdateGroupMember(new(user.Id.ToString(), username, user.GroupMember.IsHost, user.GroupMember.IsDone, user.Score));
            await Clients.Caller.UpdateUserData(username, user.Score);

            //Add to sessionlog
            var entry = new SessionLogEntry(SessionLogEntryType.ChangeUsername, user.Id + ";" + oldUsername + ";" + username, DateTime.UtcNow, user);
            await Database.SessionLogEntries.AddAsync(entry);
            return true;
        }

        public async Task<bool> ChangePassword(string password)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);
            if (session == null)
                return false;

            long sid = session.User.Id;
            await Database.SaveChangesAsync();

            Authenticator auth = session.User.Authenticator;
            if (auth == null)
                return false;

            auth.Password = CreatePasswordHash(password);
            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeEmail(string email)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);
            if (session == null)
                return false;

            long sid = session.User.Id;
            await Database.SaveChangesAsync();

            Authenticator auth = session.User.Authenticator;
            if (auth == null)
                return false;

            User user = session.User;

            auth.Email = email;
            user.Email = email;
            await Database.SaveChangesAsync();
            return true;
        }

        // With insight from: http://csharptest.net/470/another-example-of-how-to-store-a-salted-password-hash/
        public static string CreatePasswordHash(string password)
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);

            Rfc2898DeriveBytes pbkdf2 = new(password, salt, 65536);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] storedBytes = new byte[36];

            Array.Copy(salt, 0, storedBytes, 0, 16);
            Array.Copy(hash, 0, storedBytes, 16, 20);

            return Convert.ToBase64String(storedBytes);
        }

        public static bool VerifyPasswordHash(string password, string passwordHash)
        {
            byte[] storedBytes = Convert.FromBase64String(passwordHash);
            byte[] salt = new byte[16];
            Array.Copy(storedBytes, 0, salt, 0, 16);

            Rfc2898DeriveBytes pbkdf2 = new(password, salt, 65536);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; ++i)
            {
                if (storedBytes[i + 16] != hash[i])
                    return false;
            }
            return true;
        }

        private static string Censor(string value)
        {
            var filter = new ProfanityFilter.ProfanityFilter();
            string censored = filter.CensorString(value).Replace('*', '_');
            if (filter.ContainsProfanity(string.Concat(censored.Where(c => c != '_'))))
                return new string('_', value.Length);
            return censored;
        }
    }
}