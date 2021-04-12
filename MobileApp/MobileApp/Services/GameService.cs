using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommunicationModel;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
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
        Task<bool> AttemptRelog();
        Task PollLocation();
    }

    public class GameService : IGameService
    {
        public IGameClient Client { get; }
        public string UserId { get; private set; }
        public event Action LoggedIn = delegate { };
        public event Action<ChallengeProgressData> ProgressUpdated = delegate { };

        public GameService()
        {
            //https://10.0.2.2:5001/hub
            //https://cornellgo.herokuapp.com/hub
            Client = new CornellGoClient("https://cornellgo.herokuapp.com/hub");
            CrossGeolocator.Current.PositionChanged += (s, e) => PollLocation(e.Position);
            CrossGeolocator.Current.DesiredAccuracy = 1;
        }

        public GameService(IGameClient client)
        {
            Client = client;
            CrossGeolocator.Current.PositionChanged += (s, e) => PollLocation(e.Position);
            CrossGeolocator.Current.DesiredAccuracy = 1;
        }

        public async Task<bool> LoginWithSession(string username, string password)
        {
            bool loggedIn = await Client.Login(username, password);
            if (loggedIn)
            {
                UserId = (await Client.GetUserData()).UserId;
                await SecureStorage.SetAsync("session", await Client.GetSessionToken());
                LoggedIn();

                if (!await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(2), 1))
                {
                    await Client.Logout();
                    SecureStorage.Remove("session");
                    return false;
                }
            }
            return loggedIn;
        }

        public async Task<bool> LogoutWithSession()
        {
            bool loggedOut = await Client.Logout();
            if (loggedOut)
            {
                await CrossGeolocator.Current.StopListeningAsync();
                SecureStorage.Remove("session");
            }
            return loggedOut;
        }

        public async Task<bool> AttemptRelog()
        {
            string token = await SecureStorage.GetAsync("session");
            if (token != null && await Client.AttemptRelog(token))
            {
                UserId = (await Client.GetUserData()).UserId;
                LoggedIn();

                if (!await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(2), 1))
                {
                    await Client.Logout();
                    SecureStorage.Remove("session");
                    return false;
                }

                return true;
            }
            return false;
        }

        public async Task PollLocation()
        {
            Location pos = await Geolocation.GetLocationAsync(new()
            {
                DesiredAccuracy = GeolocationAccuracy.Best
            });
            var progress = await Client.CheckProgress(pos.Latitude, pos.Longitude);
            await Device.InvokeOnMainThreadAsync(() => ProgressUpdated(progress));
        }

        public async Task PollLocation(Position pos)
        {
            var progress = await Client.CheckProgress(pos.Latitude, pos.Longitude);
            await Device.InvokeOnMainThreadAsync(() => ProgressUpdated(progress));
        }
    }
}
