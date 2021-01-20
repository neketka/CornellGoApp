using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationModel
{
    public delegate Task UpdateGroupData(string friendlyId, GroupMemberData[] members);
    public delegate Task UpdateGroupMember(GroupMemberData data);
    public delegate Task LeaveGroupMember(string userId);
    public delegate Task UpdateUserData(string username, int points);
    public delegate Task UpdateChallenge(ChallengeData data);
    public delegate Task FinishChallenge();
    public delegate Task UpdateScorePositions(string userId, string username, int oldIndex, int newIndex, int score);
    public interface IClientCallback
    {
        Task UpdateGroupData(string friendlyId, GroupMemberData[] members);
        Task UpdateGroupMember(GroupMemberData data);
        Task LeaveGroupMember(string userId);
        Task UpdateUserData(string username, int points);
        Task UpdateChallenge(ChallengeData data);
        Task FinishChallenge();
        Task UpdateScorePositions(string userId, string username, int oldIndex, int newIndex, int score);
    }
}
