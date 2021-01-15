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
            InitializeComponent();
            //Routing.RegisterRoute(nameof(SuggestPage), typeof(SuggestPage));
            //Routing.RegisterRoute(nameof(FeedbackPage), typeof(FeedbackPage));
            //await Current.GoToAsync(nameof(FeedbackPage));
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Current.FlyoutIsPresented = false;
        }
    }
}
