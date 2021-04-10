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

        public IAsyncEnumerable<PlaceData> GetPlaces()
        {
            return Database.Challenges.AsAsyncEnumerable().Select(chal => new PlaceData(chal.Id.ToString(), chal.ImageUrl, chal.Name,
                chal.Description, chal.Points, chal.LongLat.Y, chal.LongLat.X, chal.Radius, chal.CitationUrl, chal.LinkUrl, chal.LongDescription));
        }

        public async Task<bool> ModifyPlace(PlaceDataModifiedState state, PlaceData data)
        {
            if (!await CheckAuthorization())
                return false;

            Challenge chal = state == PlaceDataModifiedState.Created 
                ? new(data.Name, data.Description, data.Points, new(data.Long, data.Lat), data.Radius, data.ImageUrl, data.longDescription, data.citationURL, data.linkURL)
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
                    break;
            }

            await Database.SaveChangesAsync();
            await Clients.All.PlaceModified(state, data);

            return true;
        }

        public async Task<bool> RequestAdmin(string email, string password)
        {
            if (await Database.Admins.AnyAsync(e => e.Email == email))
                return false;

            BackendModel.Admin admin = new BackendModel.Admin(email, CornellGoHub.CreatePasswordHash(password),
                AdminAccountStatus.Awaiting, "");

            await Database.Admins.AddAsync(admin);
            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<AdminLoginResult> Login(string email, string password)
        {
            BackendModel.Admin a = await Database.Admins.FirstOrDefaultAsync(a => a.Email == email);
            if (a == null)
                return AdminLoginResult.NoAccount;

            if (!CornellGoHub.VerifyPasswordHash(password, a.PasswordHash))
                return AdminLoginResult.WrongPassword;

            switch (a.Status)
            {
                case AdminAccountStatus.Approved:
                    a.SignalRId = Context.UserIdentifier;
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

        public async Task<bool> ApproveAdmin(string email)
        {
            if (!await CheckAuthorization())
                return false;

            BackendModel.Admin admin = Database.Admins.SingleOrDefault(a => a.Status == AdminAccountStatus.Awaiting && a.Email == email);
            if (admin == null)
                return false;
            admin.Status = AdminAccountStatus.Approved;
            await Database.SaveChangesAsync();
            return true;
        }

        private async Task<bool> CheckAuthorization()
        {
            return await Database.Admins.AnyAsync(a => a.SignalRId == Context.ConnectionId && a.Status == AdminAccountStatus.Approved);
        }
    }
}
