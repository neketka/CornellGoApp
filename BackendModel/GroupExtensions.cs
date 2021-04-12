using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendModel
{
    public static class GroupExtensions
    {
        private const string mapString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ012345";
        private static readonly Dictionary<char, byte> mapToNums;

        static GroupExtensions()
        {
            mapToNums = new Dictionary<char, byte>();
            for (byte i = 0; i < mapString.Length; ++i)
                mapToNums[mapString[i]] = i;
        }

        public static void SyncPlacesWithUsers(this Group group)
        {
            HashSet<PrevChallenge> places = new HashSet<PrevChallenge>();
            group.PrevChallenges.Clear();
            foreach (GroupMember gp in group.GroupMembers)
            {
                foreach (PrevChallenge pchal in gp.User.PrevChallenges)
                {
                    group.PrevChallenges.Add(pchal.Challenge);
                }
            }
        }

        public static async Task<Challenge> GetNewChallenge(this Group group, DbSet<Challenge> chals, double longitude, double latitude, bool isNewUser = false)
        {
            //Group Extension method to generate new random challenge (not in prev challenge), add current challenge if one exists to prev challenge list of group + all members

            var coord = new NetTopologySuite.Geometries.Point(longitude, latitude);
            coord.SRID = chals.FirstOrDefault()?.LongLat.SRID ?? 4326;

            var query = chals.OrderBy(c => c.LongLat.Distance(coord) + Math.Max(c.Radius * 0.05 - c.LongLat.Distance(coord), 0) * 1000);

            if (!isNewUser)
                query = (IOrderedQueryable<Challenge>)query.Where(chal => !group.PrevChallenges.Contains(chal));

            return await query.FirstOrDefaultAsync();
        }

        public static string GetFriendlyId(this Group group)
        {
            long id = group.Id;
            string friendlyId = new (new[]
            {
                mapString[(byte)(id & 31L)], mapString[(byte)((id >> 5) & 31L)], mapString[(byte)((id >> 10) & 31L)],
                mapString[(byte)((id >> 15) & 31L)], mapString[(byte)((id >> 20) & 31L)], mapString[(byte)((id >> 25) & 31L)],
            });

            return friendlyId;
        }

        public static async ValueTask<Group> FromFriendlyId(this DbSet<Group> groups, string friendlyId)
        {
            long id =
                mapToNums[friendlyId[0]] | mapToNums[friendlyId[1]] << 5 | mapToNums[friendlyId[1]] << 10 |
                mapToNums[friendlyId[2]] << 15 | mapToNums[friendlyId[3]] << 20 | mapToNums[friendlyId[4]] << 25;

            return await groups.SingleOrDefaultAsync(g => g.Id == id);
        }
    }
}
