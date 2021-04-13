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
            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);
            if (session == null) return false;

            User user = await Database.Users.AsAsyncEnumerable().SingleAsync(b => b.Id == long.Parse(userId));
            if (user.GroupMember.Group == null) return false; //Is default null?

            GroupMember gmem = user.GroupMember;
            Group grp = user.GroupMember.Group;

            if (gmem.IsHost)
            {
                foreach (GroupMember member in grp.GroupMembers)
                {
                    Group gp = new Group(grp.Challenge);
                    gp.GroupMembers.Add(member);
                    member.Group = gp;
                    member.IsHost = true;
                    gp.SyncPlacesWithUsers();

                    await Database.SaveChangesAsync();
                    gp.SignalRId = gp.Id.ToString();

                    if (member.User?.UserSession?.SignalRId != null)
                    {
                        await Groups.RemoveFromGroupAsync(member.User.UserSession.SignalRId, grp.SignalRId);
                        await Groups.AddToGroupAsync(gmem.User.UserSession.SignalRId, gp.SignalRId);
                    }

                    await Clients.Group(gp.SignalRId).UpdateGroupData(gp.GetFriendlyId(), new GroupMemberData[] {
                        new GroupMemberData(member.User.Id.ToString(), member.User.Username, true, member.IsDone, member.User.Score)
                    });
                }
            }
            else
            {
                Group gp = new Group(grp.Challenge);
                gp.GroupMembers.Add(gmem);
                gmem.Group = gp;
                gmem.IsHost = true;
                gp.SyncPlacesWithUsers();

                await Database.SaveChangesAsync();
                gp.SignalRId = gp.Id.ToString();

                await Groups.RemoveFromGroupAsync(gmem.User.UserSession.SignalRId, grp.SignalRId);
                await Groups.AddToGroupAsync(gmem.User.UserSession.SignalRId, gp.SignalRId);

                grp.GroupMembers.Remove(gmem);

                await Clients.Group(grp.SignalRId).UpdateGroupData(grp.GetFriendlyId(), grp.GroupMembers.Select(gmem =>
                    new GroupMemberData(gmem.User.Id.ToString(), gmem.User.Username, true, gmem.IsDone, gmem.User.Score)).ToArray());

                await Clients.Group(gp.SignalRId).UpdateGroupData(gp.GetFriendlyId(), new[] {
                    new GroupMemberData(gmem.User.Id.ToString(), gmem.User.Username, true, gmem.IsDone, gmem.User.Score)
                });
            }
            await Database.SaveChangesAsync();

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
                var entry = new SessionLogEntry(SessionLogEntryType.KickedMember, session.User + ";" + user + ";" + grp, DateTime.UtcNow, session.User);
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
            groupId = groupId.ToUpper();

            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);
            User user = session.User;
            Group grp;
            try
            {
                grp = await Database.Groups.FromFriendlyId(groupId);
            }
            catch
            {
                return false;
            }

            //Make sure group isnt null
            if (grp == null)
                return false;

            if (grp.Id == user.GroupMember.Group.Id || grp.MaxMembers == grp.GroupMembers.Count || grp.GroupMembers.Count == 0)
                return false;

            //Remove user from old group
            await Kick(user.Id.ToString());

            //Add user to new group
            grp.GroupMembers.Add(user.GroupMember);
            user.GroupMember.Group = grp;
            user.GroupMember.IsHost = false;

            user.GroupMember.IsDone = user.PrevChallenges.Any(e => e.Challenge.Id == grp.Challenge.Id);

            //Add to sessionlog
            var entry = new SessionLogEntry(SessionLogEntryType.UserFromGroupJoined, user.Id + ";" + groupId, DateTime.UtcNow, user);
            await Database.SessionLogEntries.AddAsync(entry);

            await Groups.AddToGroupAsync(user.UserSession.SignalRId, grp.Id.ToString());

            await Clients.Group(grp.SignalRId).UpdateGroupData(grp.GetFriendlyId(), grp.GroupMembers.Select(gmem =>
                    new GroupMemberData(gmem.User.Id.ToString(), gmem.User.Username, true, gmem.IsDone, gmem.User.Score)).ToArray());

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
            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);
            User user = session.User;
            Challenge chal = user.GroupMember.Group.Challenge;
            return new ChallengeData(chal.Id.ToString(), chal.Description, chal.Points, chal.ImageUrl);
        }

        public async Task<GroupMemberData[]> GetGroupMembers()
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);
            User user = session.User;

            return user.GroupMember.Group.GroupMembers.Select(gmem =>
                new GroupMemberData(gmem.User.Id.ToString(), gmem.User.Username, gmem.IsHost, gmem.IsDone, gmem.User.Score)).ToArray();
        }

        public async Task<GroupMemberData> GetGroupMember(string userId)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);
            User user = Database.Users.Single(b => b.Id == long.Parse(userId));
            return new GroupMemberData(user.Id.ToString(), user.Username, user.GroupMember.IsHost, user.GroupMember.IsDone, user.Score);
        }

        public async Task<string> GetFriendlyGroupId()
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);
            User user = session.User;
            return user.GroupMember.Group.GetFriendlyId();
        }

        public async Task<ChallengeProgressData> CheckProgress(double lat, double @long)
        {
            UserSession session = await Database.UserSessions.FromSignalRId(Context.ConnectionId);
            User user = session.User;
            Challenge chal = user.GroupMember.Group.Challenge;

            var fPoint = chal.LongLat;
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(fPoint.SRID);
            var nPoint = geometryFactory.CreatePoint(new Coordinate(@long, lat));

            double dist = nPoint.ProjectTo(2830).Distance(fPoint.ProjectTo(2830));

            double d = chal.Radius * 10.0;
            double scaled = ((dist + d) * d - d * d) / (d * (dist + d));

            if (dist < chal.Radius)
            {
                user.GroupMember.IsDone = true;
                await Clients.Caller.FinishChallenge();

                await Clients.Group(user.GroupMember.Group.SignalRId).UpdateGroupMember(
                    new(user.Id.ToString(), user.Username, user.GroupMember.IsHost, true, user.Score));

                if (!user.GroupMember.IsDone)
                {
                    //Add to sessionlog
                    var entry = new SessionLogEntry(SessionLogEntryType.FoundPlace, user.GroupMember.Group.Id.ToString(), DateTime.UtcNow, user);
                    await Database.SessionLogEntries.AddAsync(entry);
                }

                await Database.SaveChangesAsync();
            }

            if (user.GroupMember.Group.GroupMembers.All(b => b.IsDone))
            {
                foreach (GroupMember member in user.GroupMember.Group.GroupMembers)
                {
                    if (!user.PrevChallenges.Any(e => e.Challenge.Id == chal.Id))
                    {
                        member.User.Score += chal.Points;
                        member.User.PrevChallenges.Add(new PrevChallenge(DateTime.UtcNow, user.GroupMember.Group.Challenge, member.User));
                    }
                    member.User.GroupMember.IsDone = false;
                    await Clients.Group(user.GroupMember.Group.SignalRId).UpdateGroupMember(new GroupMemberData(
                        member.User.Id.ToString(), member.User.Username, member.IsHost, member.IsDone, member.User.Score
                    ));

                    string uId = member.User.UserSession?.SignalRId;
                    if (uId != null)
                        await Clients.Client(uId).UpdateUserData(member.User.Username, member.User.Score);
                }
                user.GroupMember.Group.PrevChallenges.Add(user.GroupMember.Group.Challenge);

                await Database.SaveChangesAsync();

                Challenge newChal = await user.GroupMember.Group.GetNewChallenge(Database.Challenges, @long, lat);
                user.GroupMember.Group.Challenge = newChal;

                ChallengeData cData = new ChallengeData(newChal.Id.ToString(), newChal.Description, newChal.Points, newChal.ImageUrl);
                await Database.SaveChangesAsync();

                await Clients.Group(user.GroupMember.Group.SignalRId).UpdateChallenge(cData);
            }

            string progressString = ((int)Math.Ceiling(dist / 64.32)).ToString() + " min";
            double progressScale = 1.0 - Math.Max(Math.Min(scaled, 1.0), 0.0);

            return new ChallengeProgressData(progressString, progressScale);
        }

        public async Task<int> GetMaxPlayers()
        {
            int current_max = 8;
            return current_max;
        }
    }
}