using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestModel
{
    public interface IClientCallback
    {
        void UpdateGroupData(string friendlyId, GroupMemberData[] members);
        void UpdateGroupMember(GroupMemberData data);
        void LeaveGroupMember(string userId);
        void UpdateUserData(string username, int points);
        void UpdateChallenge(ChallengeData data);
        void FinishChallenge();
        void UpdateScorePositions(string userId, string username, int oldIndex, int newIndex, int score);
    }
}
