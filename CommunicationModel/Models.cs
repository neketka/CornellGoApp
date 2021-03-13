using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}

namespace CommunicationModel
{

    public record UserData(string Username, int Points, string GroupId, string UserId, int RankIndex, int TotalUserCount);
    public record LeaderboardData(string UserId, string Username, int Index, int Score);
    public record ChallengeData(string ChallengeId, string Description, int Points, string ImageUrl);
    public record ChallengeHistoryEntryData(string ChallengeId, string ImageUrl, string Name, string Description, int Points, DateTime UtcDateTime);
    public record GroupMemberData(string UserId, string Username, bool IsHost, bool IsDone, int Points);
    public record ChallengeProgressData(string WalkDistance, double Progress);
    public enum FrontendMetric
    {
        ClosedApp = 0, AppSuspended = 1, OpenLeaderboard = 2, OpenHistory = 3, OpenLearnMore = 4,
        OpenGroupMenu = 5, OpenJoinGroupMenu = 6, OpenGameMenu = 7, OpenSettings = 8, AppResumed = 9
    }
}