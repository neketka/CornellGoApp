using MobileApp.Services;
using MobileApp.ViewModels;
using MobileApp.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private GameService GameService { get; }
        public static BrowserLaunchOptions CustomTabsOptions { get; } = new BrowserLaunchOptions
        {
            LaunchMode = BrowserLaunchMode.SystemPreferred,
            TitleMode = BrowserTitleMode.Show,
            PreferredToolbarColor = Color.FromHex("2C2F33"),
            PreferredControlColor = Color.FromHex("CB2424")
        };

        public AppShell()
        {
            Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
            Routing.RegisterRoute(nameof(ChangeEmailPage), typeof(ChangeEmailPage));
            Routing.RegisterRoute(nameof(CloseAccountPage), typeof(CloseAccountPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
            Routing.RegisterRoute(nameof(LeaderPage), typeof(LeaderPage));
            Routing.RegisterRoute(nameof(LandingPage), typeof(LandingPage));

            GameService = DependencyService.Get<GameService>();

            GameService.LoggedIn += GameService_LoggedIn;
            GameService.Client.UserDataUpdated += Client_UserDataUpdated;

            InitializeComponent();
        }

        private async void GameService_LoggedIn()
        {
            var data = await GameService.Client.GetUserData();
            await Device.InvokeOnMainThreadAsync(() =>
            {
                ProfileView.Username = data.Username;
                ProfileView.Score = data.Points;
            });
        }

        private async Task Client_UserDataUpdated(string username, int points)
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                ProfileView.Username = username;
                ProfileView.Score = points;
            });
        }

        private async void Suggest_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://docs.google.com/forms/d/e/1FAIpQLSeUQFU8LI2LnHHSg7I4r-i10BNYr4zDiwp_la0v0nBT-fP4GA/viewform", CustomTabsOptions);
            Current.FlyoutIsPresented = false;
        }

        private async void Feedback_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://docs.google.com/forms/d/e/1FAIpQLSfwl50y3ebjjsQrtoAxkXoYYoQntodpQhp9RFkYURv_vg94Ug/viewform", CustomTabsOptions);
            Current.FlyoutIsPresented = false;
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            bool wasPresented = Current.FlyoutIsPresented;
            Current.FlyoutIsPresented = false;
            if (wasPresented)
                await Current.GoToAsync(nameof(SettingsPage));
        }

        private async void Leaderboard_Clicked(object sender, EventArgs e)
        {
            bool wasPresented = Current.FlyoutIsPresented;
            Current.FlyoutIsPresented = false;
            if (wasPresented)
                await Current.GoToAsync(nameof(LeaderPage));
        }

        private async void History_Clicked(object sender, EventArgs e)
        {
            bool wasPresented = Current.FlyoutIsPresented;
            Current.FlyoutIsPresented = false;
            if (wasPresented)
                await Current.GoToAsync(nameof(HistoryPage));
        }
    }
}
