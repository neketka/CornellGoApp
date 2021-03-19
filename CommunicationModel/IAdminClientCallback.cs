using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationModel
{
    public interface IAdminClientCallback
    {
        Task PlaceModified(PlaceDataModifiedState state, PlaceData data);
    }
}
