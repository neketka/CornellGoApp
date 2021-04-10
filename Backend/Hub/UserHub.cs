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
            IDictionary<FrontendMetric, SessionLogEntryType> entryDic = new Dictionary<FrontendMetric, SessionLogEntryType>();
            entryDic.Add(FrontendMetric.AppResumed, SessionLogEntryType.AppResumed);
            entryDic.Add(FrontendMetric.AppSuspended, SessionLogEntryType.AppSuspended);
            entryDic.Add(FrontendMetric.OpenSettings, SessionLogEntryType.OpenSettings);
            entryDic.Add(FrontendMetric.OpenGameMenu, SessionLogEntryType.OpenGameMenu);
            entryDic.Add(FrontendMetric.OpenJoinGroupMenu, SessionLogEntryType.OpenJoinGroupMenu);
            entryDic.Add(FrontendMetric.OpenGroupMenu, SessionLogEntryType.OpenGroupMenu);
            entryDic.Add(FrontendMetric.OpenLearnMore, SessionLogEntryType.OpenLearnMore);
            entryDic.Add(FrontendMetric.AppSuspended, SessionLogEntryType.AppSuspended);
            entryDic.Add(FrontendMetric.OpenHistory, SessionLogEntryType.OpenHistory);
            entryDic.Add(FrontendMetric.OpenLeaderboard, SessionLogEntryType.OpenLeaderboard);
            entryDic.Add(FrontendMetric.ClosedApp, SessionLogEntryType.ClosedApp);

            await Database.SessionLogEntries.AddAsync(new SessionLogEntry(entryDic[metric], data, DateTime.UtcNow, user));
            await Database.SaveChangesAsync();

        }

        public async Task<LearnMoreData> GetLearnMoreData(string placeId)
        {
            Challenge place = Database.Challenges.Single(b => b.Id == long.Parse(placeId));
            return new LearnMoreData(place.Id, place.Name, place.LongLat.X, place.LongLat.Y, place.Description, place.LongDescription, place.CitationUrl, place.LinkUrl, place.ImageUrl, DateTime.UtcNow);

        }
    }
}
