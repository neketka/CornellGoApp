using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationModel
{
    public record PlaceData(string Id, string ImageUrl, string Name, string Description, int Points, double Lat, double Long, double Radius, string citationURL, string linkURL, string longDescription);
    public enum PlaceDataModifiedState
    {
        Created = 0, Destroyed = 1, Modified = 2
    }

    public enum AdminLoginResult
    {
        NoAdminApproval = 0, AdminRejected = 1, NoAccount = 2, WrongPassword = 3, Success = 4
    }
}
