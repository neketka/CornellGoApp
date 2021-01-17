using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using RequestModel;

namespace RequestModel
{
    public partial class CornellGoClient
    {
        public event UpdateGroupData GroupDataUpdated;
        public event UpdateGroupMember GroupMemberUpdated;
        public event LeaveGroupMember GroupMemberLeft;
        public event UpdateUserData UserDataUpdated;
        public event UpdateChallenge ChallengeUpdated;
        public event FinishChallenge ChallengeFinished;
        public event UpdateScorePositions ScorePositionsUpdated;

        private HubConnection Connection { get; }
        private ClientCalls Client { get; }

        public CornellGoClient(string url)
        {
            Connection = new HubConnectionBuilder().WithAutomaticReconnect().WithUrl(url).Build();
            Client = new ClientCalls(this);
            Connection.StartAsync().RunSynchronously();
        }

        private class ClientCalls : IClientCallback
        {
            private CornellGoClient Client { get; }
            public ClientCalls(CornellGoClient client)
            {
                Client = client;

                Client.Connection.On<string, GroupMemberData[]>("UpdateGroupData", UpdateGroupData);
                Client.Connection.On<GroupMemberData>("UpdateGroupMember", UpdateGroupMember);
                Client.Connection.On<string>("LeaveGroupMember", LeaveGroupMember);
                Client.Connection.On<string, int>("UpdateUserData", UpdateUserData);
                Client.Connection.On<ChallengeData>("UpdateChallenge", UpdateChallenge);
                Client.Connection.On("FinishChallenge", FinishChallenge);
                Client.Connection.On<string, string, int, int, int>("UpdateScorePositions", UpdateScorePositions);
            }

            public void UpdateGroupData(string friendlyId, GroupMemberData[] members) 
                => Client.GroupDataUpdated(friendlyId, members);

            public void UpdateGroupMember(GroupMemberData data) 
                => Client.GroupMemberUpdated(data);

            public void LeaveGroupMember(string userId) 
                => Client.GroupMemberLeft(userId);

            public void UpdateUserData(string username, int points) 
                => Client.UserDataUpdated(username, points);

            public void UpdateChallenge(ChallengeData data) 
                => Client.ChallengeUpdated(data);

            public void FinishChallenge() 
                => Client.ChallengeFinished();

            public void UpdateScorePositions(string userId, string username, int oldIndex, int newIndex, int score)
                => Client.ScorePositionsUpdated(userId, username, oldIndex, newIndex, score);
        }
    }
}
