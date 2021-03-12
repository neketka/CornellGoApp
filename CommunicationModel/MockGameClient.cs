using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CommunicationModel
{
    public class MockGameClient : IGameClient
    {
        public event FinishChallenge ChallengeFinished;
        public event UpdateChallenge ChallengeUpdated;
        public event Func<Task> ConnectionClosed;
        public event UpdateGroupData GroupDataUpdated;
        public event LeaveGroupMember GroupMemberLeft;
        public event UpdateGroupMember GroupMemberUpdated;
        public event UpdateScorePositions ScorePositionsUpdated;
        public event UpdateUserData UserDataUpdated;

        private List<GroupMemberData> MockPlayers;
        private List<ChallengeHistoryEntryData> MockHistories;

        public MockGameClient()
        {
            MockPlayers = new()
            {
                new("You", "Your username", true, false, 5),
                new("Player1", "Player1", false, true, 4),
                new("Player2", "Player2", false, true, 3),
                new("Player3", "Player3", false, false, 2),
                new("Player4", "Player4", false, true, 1),
                new("Player5", "Player5", false, false, 0),
                new("Player6", "Player6", false, true, 0),
                new("Player7", "Player7", false, false, 0),
            };

            MockHistories = new()
            {
                new("1", "https://media-cdn.tripadvisor.com/media/photo-m/1280/13/bd/12/dd/triphammer-falls-the.jpg", 
                    "Triphammer Falls", "A place", 3, DateTime.UtcNow),
                new("2", "https://upload.wikimedia.org/wikipedia/commons/7/74/Statue_of_Ezra_Cornell%2C_founder_of_Cornell_University.jpg",
                    "Ezra Cornell Statue", "A place", 5, DateTime.UtcNow)
            };
        }

        public Task<bool> AttemptRelog(string session)
        {
            return Task.FromResult(false);
        }

        public Task<bool> ChangeEmail(string email)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ChangePassword(string password)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ChangeUsername(string username)
        {
            return Task.FromResult(true);
        }

        public Task<ChallengeProgressData> CheckProgress(double lat, double @long)
        {
            return Task.FromResult(new ChallengeProgressData("20 min", new Random().NextDouble()));
        }

        public Task<ChallengeData> GetChallengeData()
        {
            return Task.FromResult(new ChallengeData("", 
                "One of the most recognized landmarks of Cornell, this tower can found and heard above a library.", 3,
                "https://upload.wikimedia.org/wikipedia/commons/d/d2/Cornell-McGraw_Tower.jpg"));
        }

        public Task<string> GetFriendlyGroupId()
        {
            return Task.FromResult("ABCD12");
        }

        public Task<GroupMemberData> GetGroupMember(string userId)
        {
            return Task.FromResult(MockPlayers.Find(e => e.UserId == userId));
        }

        public Task<GroupMemberData[]> GetGroupMembers()
        {
            return Task.FromResult(MockPlayers.ToArray());
        }

        public async IAsyncEnumerable<ChallengeHistoryEntryData> GetHistoryData()
        {
            foreach (var his in MockHistories)
                yield return his;
        }

        public Task<string> GetPrevChallengeName()
        {
            return Task.FromResult("Previous Challenge");
        }

        public Task<string> GetSessionToken()
        {
            return Task.FromResult("");
        }

        public async IAsyncEnumerable<LeaderboardData> GetTopPlayers(int index, int count)
        {
            int len = Math.Min(count, MockPlayers.Count - index);
            for (; index < len; ++index)
            {
                var user = MockPlayers[index];
                yield return new LeaderboardData(user.UserId, user.Username, index, user.Points);
            }
        }

        public Task<UserData> GetUserData()
        {
            var you = MockPlayers[0];
            return Task.FromResult(new UserData(you.Username, you.Points, "ABCD12", you.UserId, 0, 8));
        }

        public Task<bool> JoinGroup(string groupId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> Kick(string userId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> Login(string email, string password)
        {
            return Task.FromResult(true);
        }

        public Task<bool> Logout()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Register(string username, string password, string email)
        {
            return Task.FromResult(true);
        }

        public Task Connect()
        {
            return Task.Delay(3000);
        }
        public Task<int> GetMaxPlayers()
        {
            return Task.FromResult(8);
        }

        public Task SendMetric(FrontendMetric metric, string data)
        {
            return Task.CompletedTask;
        }
    }
}
