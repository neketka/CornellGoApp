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
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
            Routing.RegisterRoute(nameof(ChangeEmailPage), typeof(ChangeEmailPage));
            Routing.RegisterRoute(nameof(CloseAccountPage), typeof(CloseAccountPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
            Routing.RegisterRoute(nameof(LeaderPage), typeof(LeaderPage));
            Routing.RegisterRoute(nameof(LandingPage), typeof(LandingPage));

            InitializeComponent();
        }

        private async void Suggest_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://docs.google.com/forms/d/e/1FAIpQLSeUQFU8LI2LnHHSg7I4r-i10BNYr4zDiwp_la0v0nBT-fP4GA/viewform", BrowserLaunchMode.SystemPreferred);
            Current.FlyoutIsPresented = false;
        }

        private async void Feedback_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://docs.google.com/forms/d/e/1FAIpQLSfwl50y3ebjjsQrtoAxkXoYYoQntodpQhp9RFkYURv_vg94Ug/viewform", BrowserLaunchMode.SystemPreferred);
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
