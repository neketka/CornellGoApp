using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace RequestModel
{
    public partial class CornellGoClient
    {
        private HubConnection Connection { get; }
        public CornellGoClient(string url)
        {
            Connection = new HubConnectionBuilder().WithAutomaticReconnect().WithUrl(url).Build();
        }
    }
}
