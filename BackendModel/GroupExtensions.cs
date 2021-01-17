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
            // TODO: implement
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
