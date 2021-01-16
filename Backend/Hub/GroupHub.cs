using RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Hub
{
    public partial class CornellGoHub
    {
        public async Task<bool> Kick(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ChallengeData> GetChallengeData()
        {
            throw new NotImplementedException();
        }

        public async Task<GroupMemberData[]> GetGroupMembers()
        {
            throw new NotImplementedException();
        }

        public async Task<GroupMemberData> GetGroupMember(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetFriendlyGroupId()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> JoinGroup(string groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<ChallengeProgressData> CheckProgress(double lat, double @long)
        {
            throw new NotImplementedException();
        }
    }
}
