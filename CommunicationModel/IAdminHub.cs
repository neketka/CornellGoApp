using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationModel
{
    public interface IAdminHub
    {
        Task<bool> ModifyPlace(PlaceDataModifiedState state, PlaceData data);
        IAsyncEnumerable<PlaceData> GetPlaces();
        Task<string[]> GetUnapprovedAdmins();
        Task<bool> ApproveAdmin(string email);
        Task<bool> RequestAdmin(string email, string password);
        Task<AdminLoginResult> Login(string email, string password);
    }
}
