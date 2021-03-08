using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace CommunicationModel
{
    public partial class CornellGoClient
    {
        public async Task<bool> AttemptRelog(string session)
            => await Connection.InvokeAsync<bool>("AttemptRelog", session);

        public async Task<ChallengeProgressData> CheckProgress(double lat, double @long) 
            => await Connection.InvokeAsync<ChallengeProgressData>("CheckProgress", lat, @long);

        public async Task<ChallengeData> GetChallengeData()
            => await Connection.InvokeAsync<ChallengeData>("GetChallengeData");

        public async Task<string> GetFriendlyGroupId()
            => await Connection.InvokeAsync<string>("GetFriendlyGroupId");

        public async Task<GroupMemberData> GetGroupMember(string userId)
            => await Connection.InvokeAsync<GroupMemberData>("GetGroupMemeber", userId);

        public async Task<GroupMemberData[]> GetGroupMembers() 
            => await Connection.InvokeAsync<GroupMemberData[]>("GetGroupMembers");

        public async Task<string> GetPrevChallengeName() 
            => await Connection.InvokeAsync<string>("GetPrevChallengeName");

        public async Task<string> GetSessionToken() 
            => await Connection.InvokeAsync<string>("GetSessionToken");

        public IAsyncEnumerable<LeaderboardData> GetTopPlayers(int index, int count)
            => Connection.StreamAsync<LeaderboardData>("GetTopPlayers", index, count);

        public IAsyncEnumerable<ChallengeHistoryEntryData> GetHistoryData()
            => Connection.StreamAsync<ChallengeHistoryEntryData>("GetHistoryData");

        public async Task<UserData> GetUserData()
            => await Connection.InvokeAsync<UserData>("GetUserData");

        public async Task<bool> JoinGroup(string groupId)
            => await Connection.InvokeAsync<bool>("JoinGroup", groupId);

        public async Task<bool> Kick(string userId)
            => await Connection.InvokeAsync<bool>("Kick", userId);

        public async Task<bool> Login(string username, string password)
            => await Connection.InvokeAsync<bool>("Login", username, password);

        public async Task<bool> Logout()
            => await Connection.InvokeAsync<bool>("Logout");

        public async Task<bool> Register(string username, string password, string email)
            => await Connection.InvokeAsync<bool>("Register", username, password, email);

        public async Task<bool> ChangeUsername(string username)
            => await Connection.InvokeAsync<bool>("ChangeUsername", username);

        public async Task<bool> ChangePassword(string password)
            => await Connection.InvokeAsync<bool>("ChangePassword", password);

        public async Task<bool> ChangeEmail(string email)
            => await Connection.InvokeAsync<bool>("ChangeEmail", email);
    }
}
