using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationModel
{
    public interface IGameClient : IServerHub
    {
        event FinishChallenge ChallengeFinished;
        event UpdateChallenge ChallengeUpdated;
        event Func<Task> ConnectionClosed;
        event UpdateGroupData GroupDataUpdated;
        event LeaveGroupMember GroupMemberLeft;
        event UpdateGroupMember GroupMemberUpdated;
        event UpdateScorePositions ScorePositionsUpdated;
        event UpdateUserData UserDataUpdated;

        Task Connect();
    }
}