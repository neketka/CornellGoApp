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
        Task<bool> UpdateAdminStatus(string email, bool approve);
        Task<bool> RequestAdmin(string email, string password);
        Task<AdminLoginResult> Login(string email, string password);
    }
}
