using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationModel
{
    public interface IServerHub
    {
        Task<bool> AttemptRelog(string session);
        Task<bool> Login(string email, string password);
        Task<bool> Logout();
        Task<string> GetSessionToken();
        Task<bool> Register(string username, string password, string email);
        Task<UserData> GetUserData();
        Task<bool> Kick(string userId);
        IAsyncEnumerable<LeaderboardData> GetTopPlayers(int index, int count);
        IAsyncEnumerable<ChallengeHistoryEntryData> GetHistoryData();
        Task<ChallengeData> GetChallengeData();
        Task<string> GetPrevChallengeName();
        Task<GroupMemberData[]> GetGroupMembers();
        Task<GroupMemberData> GetGroupMember(string userId);
        Task<string> GetFriendlyGroupId();
        Task<bool> JoinGroup(string groupId);
        Task<ChallengeProgressData> CheckProgress(double lat, double @long);
    }
}
