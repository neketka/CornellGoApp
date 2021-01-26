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

        public async Task ToLoginPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        public async Task ToLoadingPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoadingPage)}");
        }
    }
}
