using MobileApp.Services;
using MobileApp.ViewModels;
using MobileApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(VictoryPage), typeof(VictoryPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute($"{nameof(LoginPage)}/{nameof(RegistrationPage)}", typeof(RegistrationPage));
            Routing.RegisterRoute($"{nameof(GamePage)}/{nameof(HistoryPage)}", typeof(HistoryPage));
            Routing.RegisterRoute($"{nameof(GamePage)}/{nameof(LeaderPage)}", typeof(LeaderPage));

            InitializeComponent();
        }

        private async void Suggest_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://forms.gle/wSPq9z2ymjxcob2w6");
            Current.FlyoutIsPresented = false;
        }

        private async void Feedback_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://forms.gle/ZsCg6PFQshd53GPm7");
            Current.FlyoutIsPresented = false;
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            Current.FlyoutIsPresented = false;
            await Current.GoToAsync(nameof(SettingsPage));
        }

        private async void Leaderboard_Clicked(object sender, EventArgs e)
        {
            Current.FlyoutIsPresented = false;
            await Current.GoToAsync($"{nameof(GamePage)}/{nameof(LeaderPage)}");
        }

        private async void History_Clicked(object sender, EventArgs e)
        {
            Current.FlyoutIsPresented = false;
            await Current.GoToAsync($"{nameof(GamePage)}/{nameof(HistoryPage)}");
        }
    }
}
