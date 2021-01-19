﻿using MobileApp.ViewModels;
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
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

            Routing.RegisterRoute(nameof(VictoryPage), typeof(VictoryPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute($"{nameof(LoginPage)}/{nameof(RegistrationPage)}", typeof(RegistrationPage));
        }

        private async void Suggest_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("");
            Current.FlyoutIsPresented = false;
        }

        private async void Feedback_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("");
            Current.FlyoutIsPresented = false;
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            Current.FlyoutIsPresented = false;
            await Current.GoToAsync(nameof(SettingsPage));
        }
    }
}
