using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return ((BaseViewModel)BindingContext).IsBusy ? true : base.OnBackButtonPressed();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://cornellgo.herokuapp.com/privacypolicy", AppShell.CustomTabsOptions);
        }
    }
}