using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace CommunicationModel
{
    public partial class CornellGoClient : IGameClient
    {
        public event UpdateGroupData GroupDataUpdated;

        public event UpdateGroupMember GroupMemberUpdated;

        public event LeaveGroupMember GroupMemberLeft;

        public event UpdateUserData UserDataUpdated;

        public event UpdateChallenge ChallengeUpdated;

        public event FinishChallenge ChallengeFinished;

        public event UpdateScorePositions ScorePositionsUpdated;

        public event Func<Task> ConnectionClosed;

        public event Func<Task> Reconnecting;

        public event Func<Task> Reconnected;

        public bool Connected => Connection.State != HubConnectionState.Disconnected;

        private HubConnection Connection { get; }
        private ClientCalls Client { get; }

        public CornellGoClient(string url)
        {
            Connection = new HubConnectionBuilder()
                .WithUrl(url, opts =>
                {
                    opts.HttpMessageHandlerFactory = (message) =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            // bypass SSL certificate
                            clientHandler.ServerCertificateCustomValidationCallback += (_, __, ___, ____) => true;
                        return message;
                    };
                }).Build();

            Client = new ClientCalls(this);
            Connection.Closed += async e => await ConnectionClosed();
            Connection.Reconnected += async e => await Reconnected();
            Connection.Reconnecting += async e => await Reconnecting();
        }

        public async Task Connect()
        {
            await Connection.StartAsync();
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

            public async Task UpdateGroupData(string friendlyId, GroupMemberData[] members)
                => await Client.GroupDataUpdated(friendlyId, members);

            public async Task UpdateGroupMember(GroupMemberData data)
                => await Client.GroupMemberUpdated(data);

            public async Task LeaveGroupMember(string userId)
                => await Client.GroupMemberLeft(userId);

            public async Task UpdateUserData(string username, int points)
                => await Client.UserDataUpdated(username, points);

            public async Task UpdateChallenge(ChallengeData data)
                => await Client.ChallengeUpdated(data);

            public async Task FinishChallenge()
                => await Client.ChallengeFinished();

            public async Task UpdateScorePositions(string userId, string username, int oldIndex, int newIndex, int score)
                => await Client.ScorePositionsUpdated(userId, username, oldIndex, newIndex, score);
        }
    }
}