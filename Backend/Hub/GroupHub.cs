using CommunicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendModel;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Backend.Hub
{
    public partial class CornellGoHub
    {
        public async Task<bool> Kick(string userId)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            if (session == null) return false;
            
            User user = await Database.Users.AsAsyncEnumerable().SingleAsync(b => b.Id == long.Parse(userId));
            if (user.GroupMember.Group == null) return false; //Is default null?

            GroupMember gmem = user.GroupMember;
            Group grp = user.GroupMember.Group;
            grp.GroupMembers.Remove(gmem);
            Database.GroupMembers.Remove(gmem);
            grp.SyncPlacesWithUsers();
            await Database.SaveChangesAsync();

            await Clients.Group(user.GroupMember.Group.SignalRId).UpdateGroupData(grp.GetFriendlyId(), await GetGroupMembers());
            

            //edge case if kicked get points and update challeng
            if (user.GroupMember.IsDone)
            {
                user.Score += grp.Challenge.Points;
            }
            //edge case if kicked and last one then move group on
            if (grp.GroupMembers.All(b => b.IsDone))
            {
                await grp.GetNewChallenge(Database.Challenges);
            }
            
            await user.NewGroup(Database.Challenges, grp.Challenge.LongLat.Coordinate.Y, grp.Challenge.LongLat.Coordinate.X);

            await Clients.Group(user.GroupMember.Group.SignalRId).UpdateGroupData(grp.GetFriendlyId(), await GetGroupMembers());

            //Add to sessionlog
            if (user.Id == long.Parse(userId) && !user.GroupMember.IsHost)
            { 
                var entry = new SessionLogEntry(SessionLogEntryType.LeaveGroup, user + ";" + grp, DateTime.UtcNow, user);
                await Database.SessionLogEntries.AddAsync(entry);
            }
            else if (user.Id == long.Parse(userId) && user.GroupMember.IsHost)
            {
                var entry = new SessionLogEntry(SessionLogEntryType.LeaveGroup, user + ";" + grp, DateTime.UtcNow, user);
                await Database.SessionLogEntries.AddAsync(entry);

                var entry2 = new SessionLogEntry(SessionLogEntryType.DisbandedGroup, user + ";" + grp, DateTime.UtcNow, user);
                await Database.SessionLogEntries.AddAsync(entry2);
            }
            else
            {
                var entry = new SessionLogEntry(SessionLogEntryType.KickedMember,  session.User + ";" + user + ";" + grp, DateTime.UtcNow, session.User);
                await Database.SessionLogEntries.AddAsync(entry);

                var entry2 = new SessionLogEntry(SessionLogEntryType.KickedByHost, user + ";" + session.User + ";" + grp, DateTime.UtcNow, user);
                await Database.SessionLogEntries.AddAsync(entry2);
            }

            if (!user.GroupMember.IsHost)
            {
                foreach (GroupMember gmem2 in grp.GroupMembers)
                {
                    //Add to sessionlog of group members
                    if (gmem2.Id != long.Parse(userId))
                    {
                        var entry2 = new SessionLogEntry(SessionLogEntryType.UserFromGroupLeft, user + ";" + grp, DateTime.UtcNow, gmem2.User);
                        await Database.SessionLogEntries.AddAsync(entry2);
                    }

                }
            }
            await Database.SaveChangesAsync();
            return true;
            
        }

        public async Task<bool> JoinGroup(string groupId)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            User user = session.User;
            Group grp = Database.Groups.Single(b => b.Id == long.Parse(groupId));

            //Make sure group isnt null
            if (grp == null)
                return false;

            //Remove user from old group
            await Kick(user.Id.ToString());

            //Add user to new group
            grp.GroupMembers.Add(user.GroupMember);
            Database.GroupMembers.Add(user.GroupMember);

            //Add to sessionlog
            var entry = new SessionLogEntry(SessionLogEntryType.UserFromGroupJoined, user.Id + ";" + groupId, DateTime.UtcNow, user);
            await Database.SessionLogEntries.AddAsync(entry);


            foreach (GroupMember gmem2 in user.GroupMember.Group.GroupMembers)
            {
                //Add to sessionlog of group members
                if (gmem2.Id != user.Id)
                {
                    var entry2 = new SessionLogEntry(SessionLogEntryType.UserFromGroupJoined, user + ";" + grp, DateTime.UtcNow, user);
                    await Database.SessionLogEntries.AddAsync(entry2);
                }

            }

            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<ChallengeData> GetChallengeData()
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            User user = session.User;
            Challenge chal = user.GroupMember.Group.Challenge;
            return new ChallengeData(chal.Id.ToString(), chal.Description, chal.Points, chal.ImageUrl);
        }

        public async Task<GroupMemberData[]> GetGroupMembers()
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            User user = session.User;

            var gmems = Database.GroupMembers.AsAsyncEnumerable().Where(b => b.Group.Id == user.GroupMember.Group.Id);
            List<GroupMemberData> list = new List<GroupMemberData>();

            await foreach (GroupMember gmem in gmems)
            {
                GroupMemberData res = await GetGroupMember(gmem.User.Id.ToString());
                list.Add(res);
            }
            // CHANGE TO LINQ
            return list.ToArray();
   
        }

        public async Task<GroupMemberData> GetGroupMember(string userId)
        {

            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            User user = Database.Users.Single(b => b.Id == long.Parse(userId));
            return new GroupMemberData(user.Id.ToString(), user.Username, user.GroupMember.IsHost, user.GroupMember.IsDone, user.Score);
        }

        public async Task<string> GetFriendlyGroupId()
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            User user = session.User;
            return user.GroupMember.Group.GetFriendlyId();
        }



        public async Task<ChallengeProgressData> CheckProgress(double lat, double @long)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.UserIdentifier);
            User user = session.User;
            Challenge chal = user.GroupMember.Group.Challenge;
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory();

            var nPoint = geometryFactory.CreatePoint(new Coordinate(lat, @long));
            var fPoint = geometryFactory.CreatePoint(chal.LongLat.Coordinates.FirstOrDefault());

            double dist = nPoint.Distance(fPoint);
            double scaled = dist / chal.Radius;
            if(dist <= 10)
            {
                user.GroupMember.IsDone = true;
                await Clients.Caller.FinishChallenge();

                //Add to sessionlog
                var entry = new SessionLogEntry(SessionLogEntryType.FoundPlace, user.GroupMember.Group.Id.ToString(), DateTime.UtcNow, user);
                await Database.SessionLogEntries.AddAsync(entry);

                if (user.GroupMember.Group.GroupMembers.All(b => b.IsDone))
                {
                    Challenge newChal = await user.GroupMember.Group.GetNewChallenge(Database.Challenges);
                    ChallengeData cData = new ChallengeData(newChal.Id.ToString(), newChal.Description, newChal.Points, newChal.ImageUrl);
                    await Clients.Group(user.GroupMember.Group.SignalRId).UpdateChallenge(cData);

                }
                await Database.SaveChangesAsync();

            }

            return new ChallengeProgressData(nPoint.Distance(fPoint).ToString(), scaled);
        }

        public async Task<int> GetMaxPlayers()
        {
            int current_max = 8;
            return current_max;
        }
    }
}
