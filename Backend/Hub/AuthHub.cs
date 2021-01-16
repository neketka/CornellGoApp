using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RequestModel;

namespace Backend.Hub
{
    public partial class CornellGoHub
    {
        public async Task<bool> AttemptRelog(string session)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetSessionToken()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
