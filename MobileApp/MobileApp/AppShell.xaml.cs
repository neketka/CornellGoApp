using MobileApp.ViewModels;
using MobileApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SuggestPage), typeof(SuggestPage));
            Routing.RegisterRoute(nameof(FeedbackPage), typeof(FeedbackPage));
        }

        private void Suggest_Clicked(object sender, EventArgs e)
        {
            Current.GoToAsync(nameof(SuggestPage));
            Current.FlyoutIsPresented = false;
        }

        private void Feedback_Clicked(object sender, EventArgs e)
        {
            Current.GoToAsync(nameof(FeedbackPage));
            Current.FlyoutIsPresented = false;
        }
    }
}
