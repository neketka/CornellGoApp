using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestModel
{
    public partial class CornellGoClient : IServerHub
    {
        public async Task<bool> AttemptRelog(string session)
        {
            throw new NotImplementedException();
        }

        public async Task<ChallengeProgressData> CheckProgress(double lat, double @long)
        {
            throw new NotImplementedException();
        }

        public async Task<ChallengeData> GetChallengeData()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetFriendlyGroupId()
        {
            throw new NotImplementedException();
        }

        public async Task<GroupMemberData> GetGroupMember(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<GroupMemberData[]> GetGroupMembers()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetPrevChallengeName()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetSessionToken()
        {
            throw new NotImplementedException();
        }

        public async Task<LeaderboardData[]> GetTopPlayers(int index, int count)
        {
            throw new NotImplementedException();
        }

        public async Task<UserData> GetUserData()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> JoinGroup(string groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Kick(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
