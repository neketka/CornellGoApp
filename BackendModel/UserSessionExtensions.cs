using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendModel
{
    public static class UserSessionsExtensions
    {
        public static Task<UserSession> FromToken(this DbSet<UserSession> userSessions, string session)
        {
            return userSessions.FirstAsync(s => s.Id.ToString() == session);
        }
    }

    public static class UserSessionExtensions
    {
        public static string ToToken(this UserSession userSession)
        {
            return userSession.ToString();
        }
    }
}
