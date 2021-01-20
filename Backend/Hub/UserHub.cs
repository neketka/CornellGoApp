using CommunicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Hub
{
    public partial class CornellGoHub
    {
        public async Task<UserData> GetUserData()
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<LeaderboardData> GetTopPlayers(int index, int count)
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<ChallengeHistoryEntryData> GetHistoryData()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetPrevChallengeName()
        {
            throw new NotImplementedException();
        }

    }
}
