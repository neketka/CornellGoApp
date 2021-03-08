using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommunicationModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.Services
{
    public interface IGameService
    {
        IGameClient Client { get; }
        string UserId { get; }

        event Action LoggedIn;
        event Action<ChallengeProgressData> ProgressUpdated;

        Task<bool> LoginWithSession(string username, string password);
        Task<bool> LogoutWithSession();
    }

    public class GameService : IGameService
    {
        public IGameClient Client { get; }
        public string UserId { get; private set; }
        public event Action LoggedIn = delegate { };
        public event Action<ChallengeProgressData> ProgressUpdated = delegate { };

        private bool runTimer;
        public GameService()
        {
            Client = new CornellGoClient("https://10.0.2.2:44367/hub");
        }

        public GameService(IGameClient client)
        {
            Client = client;
        }

        public async Task<bool> LoginWithSession(string username, string password)
        {
            bool loggedIn = await Client.Login(username, password);
            if (loggedIn)
            {
                UserId = (await Client.GetUserData()).UserId;
                await SecureStorage.SetAsync("session", await Client.GetSessionToken());
                LoggedIn();

                BeginPollingLocation();
            }
            return loggedIn;
        }

        public async Task<bool> LogoutWithSession()
        {
            bool loggedOut = await Client.Logout();
            if (loggedOut)
            {
                StopPollingLocation();
                SecureStorage.Remove("session");
            }
            return loggedOut;
        }

        private void BeginPollingLocation()
        {
            runTimer = true;

            Device.StartTimer(TimeSpan.FromMilliseconds(2), () =>
            {
                PollLocation();
                return runTimer;
            });
        }

        private void StopPollingLocation()
        {
            runTimer = false;
        }

        private async Task PollLocation()
        {
            var location = await Geolocation.GetLocationAsync(new(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1)));
            var progress = await Client.CheckProgress(location.Latitude, location.Longitude);
            await Device.InvokeOnMainThreadAsync(() => ProgressUpdated(progress));
        }
    }
}
