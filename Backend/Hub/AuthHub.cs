using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RequestModel;
using BackendModel;
using Microsoft.EntityFrameworkCore;

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
                return true;
            }
            return false;
        }

        public async Task<bool> Login(string username, string password)
        {
            Authenticator authenticator = await Database.Authenticators.FirstOrDefaultAsync(e => e.Username == username && e.Password == password);
            if (authenticator == null)
                return false;

            UserSession session = authenticator.User.UserSession ?? new UserSession(DateTime.UtcNow, authenticator.User);
            authenticator.User.UserSession = session;

            session.SignalRId = Context.UserIdentifier;

            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Logout()
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            if (session != null)
                return false;

            Database.Remove(session);
            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetSessionToken()
        {
            return (await Database.UserSessions.FromSignalRId(Context.UserIdentifier))?.ToToken();
        }

        public async Task<bool> Register(string username, string password)
        {
            if (await Database.Authenticators.AnyAsync(e => e.Username == username))
                return false;

            Authenticator authenticator = new Authenticator(username, password, DateTime.UtcNow, new User(0, username));

            await Database.Authenticators.AddAsync(authenticator);
            return true;
        }
    }
}
