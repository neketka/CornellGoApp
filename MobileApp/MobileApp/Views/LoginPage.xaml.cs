using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private bool canNavigate = true;
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            canNavigate = true;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void Register_Clicked(object sender, EventArgs e)
        {
            if (canNavigate)
            {
                canNavigate = false;
                await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
            }
        }
    }
}