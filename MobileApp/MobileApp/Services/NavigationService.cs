using MobileApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.Services
{
    public class NavigationService
    {
        public async Task ToGamePage()
        {
            await Shell.Current.GoToAsync($"//{nameof(GamePage)}");
        }

        public async Task PushLoginPage()
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }

        public async Task PushRegistrationPage()
        {
            await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
        }

        public async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task ToLoadingPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoadingPage)}");
        }

        public async Task PushLandingPage()
        {
            await Shell.Current.GoToAsync($"{nameof(LandingPage)}");
        }
    }
}
