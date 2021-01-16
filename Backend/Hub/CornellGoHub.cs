using BackendModel;
using Microsoft.AspNetCore.SignalR;
using RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Hub
{
    public partial class CornellGoHub : Hub<IClientCallback>, IServerHub
    {
        private CornellGoDb Database { get; }
        public CornellGoHub(CornellGoDb context) => Database = context;
    }
}
