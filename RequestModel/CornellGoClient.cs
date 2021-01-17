using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using RequestModel;

namespace RequestModel
{
    public partial class CornellGoClient
    {
        private HubConnection Connection { get; }


        public CornellGoClient(string url)
        {
            Connection = new HubConnectionBuilder().WithAutomaticReconnect().WithUrl(url).Build();
            Connection.StartAsync().RunSynchronously();
        }

        private class ClientMethods : IClientCallback
        {
            public void FinishChallenge()
            {
            }

            public void LeaveGroupMember(string userId)
            {
                throw new NotImplementedException();
            }

            public void UpdateChallenge(ChallengeData data)
            {
                throw new NotImplementedException();
            }

            public void UpdateGroupData(string friendlyId, GroupMemberData[] members)
            {
                throw new NotImplementedException();
            }

            public void UpdateGroupMember(GroupMemberData data)
            {
                throw new NotImplementedException();
            }

            public void UpdateScorePositions(string userId, string username, int oldIndex, int newIndex, int score)
            {
                throw new NotImplementedException();
            }

            public void UpdateUserData(string username, int points)
            {
                throw new NotImplementedException();
            }
        }
    }
}
