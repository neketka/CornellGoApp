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
        public string UserId { get; private set; }
        public GameService()
        {
            Client = new CornellGoClient("localhost:5000/hub");
        }

        public async Task<bool> LoginWithSession(string username, string password)
        {
            bool loggedIn = await Client.Login(username, password);
            if (loggedIn)
            {
                UserId = (await Client.GetUserData()).UserId;
                await SecureStorage.SetAsync("session", await Client.GetSessionToken());
            }
            return loggedIn;
        }

        public async Task<bool> LogoutWithSession()
        {
            bool loggedOut = true;//await Client.Logout();
            if (loggedOut)
                SecureStorage.Remove("session");
            return loggedOut;
        }
    }
}
