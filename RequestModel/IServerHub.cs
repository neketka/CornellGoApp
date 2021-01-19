using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestModel
{
    public interface IServerHub
    {
        Task<bool> AttemptRelog(string session);
        Task<bool> Login(string username, string password);
        Task<bool> Logout();
        Task<string> GetSessionToken();
        Task<bool> Register(string username, string password, string email);
        Task<UserData> GetUserData();
        Task<bool> Kick(string userId);
        Task<IAsyncEnumerable<LeaderboardData>> GetTopPlayers(int index, int count);
        Task<ChallengeData> GetChallengeData();
        Task<string> GetPrevChallengeName();
        Task<GroupMemberData[]> GetGroupMembers();
        Task<GroupMemberData> GetGroupMember(string userId);
        Task<string> GetFriendlyGroupId();
        Task<bool> JoinGroup(string groupId);
        Task<ChallengeProgressData> CheckProgress(double lat, double @long);
        Task<IAsyncEnumerable<ChallengeHistoryEntry>> GetHistoryData();
    }
}
