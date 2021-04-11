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
        private IGameService GameService { get; }
        public static BrowserLaunchOptions CustomTabsOptions { get; } = new BrowserLaunchOptions
        {
            LaunchMode = BrowserLaunchMode.SystemPreferred,
            TitleMode = BrowserTitleMode.Show,
            PreferredToolbarColor = Color.FromHex("2C2F33"),
            PreferredControlColor = Color.FromHex("CB2424")
        };

        public ViewModelContainer Container { get; }

        public AppShell()
        {
            Container = new ViewModelContainer();

            Container.RegisterService<IGameService, GameService>();
            Container.RegisterService<IDialogService, DialogService>();

            GameService = Container.GetService<IGameService>();

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

        public static async Task OpenFeedbackForm()
        {
            string iosLink = "https://docs.google.com/forms/d/e/1FAIpQLSfwl50y3ebjjsQrtoAxkXoYYoQntodpQhp9RFkYURv_vg94Ug/viewform?usp=sf_link";
            string andLink = "https://docs.google.com/forms/d/e/1FAIpQLSfbEp9ytE9JtKy-hM2YF-X7nPIaKEuUTuA3eyuE9IbQJZI1dg/viewform?usp=sf_link";
            
            await Browser.OpenAsync(Device.RuntimePlatform == Device.Android ? andLink : iosLink, CustomTabsOptions);
        }

        private async void Suggest_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://docs.google.com/forms/d/e/1FAIpQLSeUQFU8LI2LnHHSg7I4r-i10BNYr4zDiwp_la0v0nBT-fP4GA/viewform?usp=sf_link", CustomTabsOptions);
            Current.FlyoutIsPresented = false;
        }

        private async void Feedback_Clicked(object sender, EventArgs e)
        {
            await OpenFeedbackForm();
            Current.FlyoutIsPresented = false;
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            bool wasPresented = Current.FlyoutIsPresented;
            Current.FlyoutIsPresented = false;
            if (wasPresented)
                await Container.NavigationService.NavigateTo<SettingsViewModel>();
        }

        private async void Leaderboard_Clicked(object sender, EventArgs e)
        {
            bool wasPresented = Current.FlyoutIsPresented;
            Current.FlyoutIsPresented = false;
            if (wasPresented)
                await Container.NavigationService.NavigateTo<LeaderViewModel>();
        }

        private async void History_Clicked(object sender, EventArgs e)
        {
            bool wasPresented = Current.FlyoutIsPresented;
            Current.FlyoutIsPresented = false;
            if (wasPresented)
                await Container.NavigationService.NavigateTo<HistoryViewModel>();
        }
    }
}
