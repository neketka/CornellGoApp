using System;
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
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            if (session != null)
            {
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
                usession.SignalRId = Context.UserIdentifier;
                await Database.SaveChangesAsync();
                await Groups.AddToGroupAsync(Context.UserIdentifier, usession.User.GroupMember.Group.Id.ToString());
                return true;
            }
            return false;
        }

        public async Task<bool> Login(string email, string password)
        {
            Authenticator authenticator = await Database.Authenticators.FirstOrDefaultAsync(e => e.Email == email);
            if (authenticator == null || !VerifyPasswordHash(password, authenticator.Password))
                return false;

            UserSession session = authenticator.User.UserSession ?? new(DateTime.UtcNow, authenticator.User);
            authenticator.User.UserSession = session;

            session.SignalRId = Context.UserIdentifier;

            await Groups.AddToGroupAsync(Context.UserIdentifier, session.User.GroupMember.Group.Id.ToString());
            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Logout()
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            if (session != null)
                return false;

            Database.Remove(session);
            await Groups.RemoveFromGroupAsync(Context.UserIdentifier, session.User.GroupMember.Group.Id.ToString());
            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetSessionToken()
        {
            return (await Database.UserSessions.FromSignalRId(Context.UserIdentifier))?.ToToken();
        }

        public async Task<bool> Register(string username, string password, string email)
        {
            if (await Database.Authenticators.AnyAsync(e => e.Email == email))
                return false;

            Authenticator auth = new(email, CreatePasswordHash(password), DateTime.UtcNow, new(0, username, email));

            // TODO: add user to a new group, sync the group with users, and generate a new challenge. There should be an extenstion method for each one.

            await Groups.AddToGroupAsync(Context.UserIdentifier, auth.User.GroupMember.Group.Id.ToString());

            await Database.Authenticators.AddAsync(auth);
            return true;
        }

        public async Task<bool> ChangeUsername(string username)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            if (session == null)
                return false;

            User user = session.User;
            user.Username = username;

            await Database.SaveChangesAsync();
            await Clients.Group(user.GroupMember.Group.SignalRId).UpdateGroupMember(new(user.Id.ToString(), username, user.GroupMember.IsHost, user.GroupMember.IsDone, user.Score));
            await Clients.Caller.UpdateUserData(username, user.Score);
            return true;
        }

        public async Task<bool> ChangePassword(string password)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            if (session == null)
                return false;

            Authenticator auth = await Database.Authenticators.FirstOrDefaultAsync(a => a.User.Id == session.User.Id);
            if (auth == null)
                return false;

            auth.Password = CreatePasswordHash(password);
            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeEmail(string email, string password)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            if (session == null)
                return false;

            Authenticator auth = await Database.Authenticators.FirstOrDefaultAsync(a => a.User.Id == session.User.Id);
            if (auth == null || !VerifyPasswordHash(password, auth.Password))
                return false;

            auth.Email = email;
            await Database.SaveChangesAsync();
            return true;
        }

        // With insight from: http://csharptest.net/470/another-example-of-how-to-store-a-salted-password-hash/
        private static string CreatePasswordHash(string password)
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

        private static bool VerifyPasswordHash(string password, string passwordHash)
        {
            byte[] storedBytes = Convert.FromBase64String(passwordHash); 
            byte[] salt = new byte[16];
            Array.Copy(storedBytes, 0, salt, 0, 16);

            Rfc2898DeriveBytes pbkdf2 = new(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; ++i)
            {
                if (storedBytes[i + 16] != hash[i])
                    return false;
            }
            return true;
        }
    }
}
