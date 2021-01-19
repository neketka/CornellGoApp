using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestModel
{
    public record UserData(string Username, int Points, string GroupId, string UserId);
    public record LeaderboardData(string UserId, string Username, int Index, int Score);
    public record ChallengeData(string ChallengeId, string Description, int Points, string ImageUrl);
    public record ChallengeHistoryEntryData(string ChallengeId, string ImageUrl, string Name, string Description, string Points, DateTime UtcDateTime);
    public record GroupMemberData(string UserId, string Username, bool IsHost, bool IsDone, int Points);
    public record ChallengeProgressData(string WalkDistance, double Progress);
}
