using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestModel
{
    public delegate void UpdateGroupData(string friendlyId, GroupMemberData[] members);
    public delegate void UpdateGroupMember(GroupMemberData data);
    public delegate void LeaveGroupMember(string userId);
    public delegate void UpdateUserData(string username, int points);
    public delegate void UpdateChallenge(ChallengeData data);
    public delegate void FinishChallenge();
    public delegate void UpdateScorePositions(string userId, string username, int oldIndex, int newIndex, int score);
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
