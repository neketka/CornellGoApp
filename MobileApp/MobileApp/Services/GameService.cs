using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommunicationModel;
using Xamarin.Essentials;

namespace MobileApp.Services
{
    public class GameService
    {
        public CornellGoClient Client { get; }
        public GameService()
        {

        }

        public async Task<bool> LoginWithSession(string username, string password)
        {
            bool loggedIn = await Client.Login(username, password);
            if (loggedIn)
                await SecureStorage.SetAsync("session", await Client.GetSessionToken());

        }
    }
}
