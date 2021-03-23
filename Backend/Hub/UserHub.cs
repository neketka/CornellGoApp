using CommunicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendModel;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Backend.Hub
{
    public partial class CornellGoHub
    {
        public async Task<UserData> GetUserData()
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            User user = session.User;
            int index = await Database.Users.AsAsyncEnumerable()
                .Select((c, i) => new { User = c, Index = i })
                .Where(x => x.User.Id == user.Id)
                .Select(x => x.Index).SingleAsync();

            return new UserData(user.Username, user.Score, user.GroupMember.Group.Id.ToString(), user.Id.ToString(), index, Database.Users.Count());
        }

        public IAsyncEnumerable<LeaderboardData> GetTopPlayers(int index, int count)
        {
            if (count > 100) count = 100;
            return Database.Users.AsAsyncEnumerable()
                .OrderBy(User => User.Score)
                .Skip(index).Take(count)
                .Select((u, i) => new LeaderboardData(u.Id.ToString(), u.Username, i + index, u.Score));
        }

        public async IAsyncEnumerable<ChallengeHistoryEntryData> GetHistoryData()
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            User user = session.User;
            IEnumerable<PrevChallenge> query = user.PrevChallenges;
            foreach (PrevChallenge prev in query)
            {
                Challenge chal = prev.Challenge;
                yield return new ChallengeHistoryEntryData(chal.Id.ToString(), chal.ImageUrl, chal.Name, chal.Description, chal.Points, prev.Timestamp);
            }
        }

        public async Task<string> GetPrevChallengeName()
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            User user = session.User;
            return user.PrevChallenges.LastOrDefault().ToString();
        }
      
        public async Task SendMetric(FrontendMetric metric, string data)
        {

            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            User user = session.User;

            switch (metric)
            {
                case FrontendMetric.ClosedApp:
                    await Database.SessionLogEntries.AddAsync(new SessionLogEntry(SessionLogEntryType.ClosedApp, data, DateTime.UtcNow, user));
                    break;
                case FrontendMetric.AppSuspended:
                    await Database.SessionLogEntries.AddAsync(new SessionLogEntry(SessionLogEntryType.AppSuspended, data, DateTime.UtcNow, user));
                    break;
                case FrontendMetric.OpenLeaderboard:
                    await Database.SessionLogEntries.AddAsync(new SessionLogEntry(SessionLogEntryType.OpenLeaderboard, data, DateTime.UtcNow, user));
                    break;
                case FrontendMetric.OpenHistory:
                    await Database.SessionLogEntries.AddAsync(new SessionLogEntry(SessionLogEntryType.OpenHistory, data, DateTime.UtcNow, user));
                    break;
                case FrontendMetric.OpenLearnMore:
                    await Database.SessionLogEntries.AddAsync(new SessionLogEntry(SessionLogEntryType.OpenLearnMore, data, DateTime.UtcNow, user));
                    break;
                case FrontendMetric.OpenGroupMenu:
                    await Database.SessionLogEntries.AddAsync(new SessionLogEntry(SessionLogEntryType.OpenGroupMenu, data, DateTime.UtcNow, user));
                    break;
                case FrontendMetric.OpenJoinGroupMenu:
                    await Database.SessionLogEntries.AddAsync(new SessionLogEntry(SessionLogEntryType.OpenJoinGroupMenu, data, DateTime.UtcNow, user));
                    break;
                case FrontendMetric.OpenGameMenu:
                    await Database.SessionLogEntries.AddAsync(new SessionLogEntry(SessionLogEntryType.OpenGameMenu, data, DateTime.UtcNow, user));
                    break;
                case FrontendMetric.OpenSettings:
                    await Database.SessionLogEntries.AddAsync(new SessionLogEntry(SessionLogEntryType.OpenSettings, data, DateTime.UtcNow, user));
                    break;
                case FrontendMetric.AppResumed:
                    await Database.SessionLogEntries.AddAsync(new SessionLogEntry(SessionLogEntryType.AppResumed, data, DateTime.UtcNow, user));
                    break;

            }
        }
    }
}
