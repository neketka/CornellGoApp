using Backend.Hub;
using BackendModel;
using CommunicationModel;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Admin
{
    public class AdminHub : Hub<IAdminClientCallback>, IAdminHub
    {
        private CornellGoDb Database { get; }
        public AdminHub(CornellGoDb context) => Database = context;

        public async IAsyncEnumerable<PlaceData> GetPlaces()
        {
            if (!await CheckAuthorization())
            {
                await foreach (var place in new List<PlaceData>().ToAsyncEnumerable())
                    yield return place;
            }
            else
            {
                var places = Database.Challenges.AsAsyncEnumerable().Select(chal => new PlaceData(chal.Id.ToString(), chal.ImageUrl, chal.Name,
                    chal.Description, chal.Points, chal.LongLat.Y, chal.LongLat.X, chal.Radius, chal.CitationUrl, chal.LinkUrl, chal.LongDescription));

                await foreach (var place in places)
                {
                    yield return place;
                }
            }
        }

        public async Task<bool> ModifyPlace(PlaceDataModifiedState state, PlaceData data)
        {
            if (!await CheckAuthorization())
                return false;

            Challenge chal = state == PlaceDataModifiedState.Created 
                ? new(data.Name, data.Description, data.Points, new(data.Long, data.Lat), data.Radius, data.ImageUrl, data.LongDescription, data.CitationUrl, data.LinkUrl)
                : await Database.Challenges.SingleAsync(c => c.Id.ToString() == data.Id);

            switch (state)
            {
                case PlaceDataModifiedState.Created:
                    data = data with { Id = (await Database.Challenges.AddAsync(chal)).Entity.Id.ToString() };
                    break;
                case PlaceDataModifiedState.Destroyed:
                    Database.Challenges.Remove(chal);
                    break;
                case PlaceDataModifiedState.Modified:
                    chal.Name = data.Name;
                    chal.Description = data.Description;
                    chal.Points = data.Points;
                    chal.LongLat = new(data.Long, data.Lat);
                    chal.Radius = data.Radius;
                    chal.CitationUrl = data.CitationUrl;
                    chal.ImageUrl = data.ImageUrl;
                    chal.LongDescription = data.LongDescription;
                    chal.LinkUrl = data.LinkUrl;
                    break;
            }

            await Database.SaveChangesAsync();
            await Clients.Group("Authorized").PlaceModified(state, data);

            return true;
        }

        public async Task<bool> RequestAdmin(string email, string password)
        {
            if (await Database.Admins.AnyAsync(e => e.Email == email))
                return false;

            var hashed = CornellGoHub.CreatePasswordHash(password);

            BackendModel.Admin admin = new BackendModel.Admin(email, hashed,
                AdminAccountStatus.Awaiting, Context.ConnectionId);

            await Database.Admins.AddAsync(admin);
            await Database.SaveChangesAsync();

            await Clients.Group("Authorized").AdminApprovalUpdate(email, false);
            return true;
        }

        public async Task<AdminLoginResult> Login(string email, string password)
        {
            BackendModel.Admin a = await Database.Admins.FirstOrDefaultAsync(a => a.Email == email);
            if (a == null)
                return AdminLoginResult.NoAccount;
            
            if (!CornellGoHub.VerifyPasswordHash(password, a.PasswordHash))
                return AdminLoginResult.WrongPassword;

            await Groups.AddToGroupAsync(Context.ConnectionId, "Authorized");

            switch (a.Status)
            {
                case AdminAccountStatus.Approved:
                    a.SignalRId = Context.ConnectionId;
                    await Database.SaveChangesAsync();
                    return AdminLoginResult.Success;
                case AdminAccountStatus.Awaiting:
                    return AdminLoginResult.NoAdminApproval;
                case AdminAccountStatus.Rejected:
                    return AdminLoginResult.AdminRejected;
            }

            return AdminLoginResult.NoAccount;
        }

        public async Task<string[]> GetUnapprovedAdmins()
        {
            if (!await CheckAuthorization())
                return new string[0];

            return await Database.Admins
                .AsAsyncEnumerable()
                .Where(a => a.Status == AdminAccountStatus.Awaiting)
                .Select(a => a.Email)
                .ToArrayAsync();
        }

        public async Task<bool> UpdateAdminStatus(string email, bool approve)
        {
            if (!await CheckAuthorization())
                return false;

            BackendModel.Admin admin = Database.Admins.SingleOrDefault(a => a.Status == AdminAccountStatus.Awaiting && a.Email == email);
            if (admin == null)
                return true;

            admin.Status = approve ? AdminAccountStatus.Approved : AdminAccountStatus.Rejected;
            await Database.SaveChangesAsync();

            await Clients.Group("Authorized").AdminApprovalUpdate(email, true);
            return true;
        }

        private async Task<bool> CheckAuthorization()
        {
            return await Database.Admins.AnyAsync(a => a.SignalRId == Context.ConnectionId && a.Status == AdminAccountStatus.Approved);
        }
    }
}
