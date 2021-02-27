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

        public static async Task NewGroup(this User user, DbSet<Challenge> chals)
        {
            Group grp = new(user.GroupMember.Group.Challenge); //unsure here

            grp.SyncPlacesWithUsers();
            await grp.GetNewChallenge(chals);
            //make async
            //add user to a new group, sync the group with users, and generate a new challenge.

        }

        public static async Task<Challenge> GetNewChallenge(this Group group, DbSet<Challenge> chals)
        {
            //Group Extension method to generate new random challenge (not in prev challenge), add current challenge if one exists to prev challenge list of group + all members

            if(group.PrevChallenges.Count() > 0)
            {
                return chals.OrderBy(c => c.LongLat.Distance(group.PrevChallenges.Last().LongLat)).FirstOrDefault();

            }

            var query = chals.Where(p => group.PrevChallenges.All(p2 => p2.Id != p.Id));
            var rnd = new Random();

            Challenge Selected = await query.OrderBy(x => rnd.Next()).FirstOrDefaultAsync();

            return Selected;
        }

        public static string GetFriendlyId(this Group group)
        {
            long id = group.Id;
            return new string(new[]
            {
                mapString[(byte)(id & 31L)], mapString[(byte)((id >> 5) & 31L)], mapString[(byte)((id >> 10) & 31L)],
                mapString[(byte)((id >> 15) & 31L)], mapString[(byte)((id >> 20) & 31L)], mapString[(byte)((id >> 25) & 31L)],
                mapString[(byte)((id >> 30) & 31L)], mapString[(byte)((id >> 35) & 31L)], mapString[(byte)((id >> 40) & 31L)]
            }); 
        }

        public static ValueTask<Group> FromFriendlyId(this DbSet<Group> groups, string friendlyId)
        {
            long id = 
                mapToNums[friendlyId[0]]       | mapToNums[friendlyId[1]] << 5  | mapToNums[friendlyId[1]] << 10 |
                mapToNums[friendlyId[2]] << 15 | mapToNums[friendlyId[3]] << 20 | mapToNums[friendlyId[4]] << 25 |
                mapToNums[friendlyId[5]] << 30 | mapToNums[friendlyId[6]] << 35 | mapToNums[friendlyId[7]] << 40;

            return groups.FindAsync(id);
        }
    }
}
