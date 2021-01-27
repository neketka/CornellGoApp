using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class LandingViewModel : BaseViewModel
    {
        public Command SigninCommand { get; }
        public Command SignupCommand { get; }

        public LandingViewModel()
        {
            SigninCommand = new Command(async () => await NavigationService.PushLoginPage());
            SignupCommand = new Command(async () => await NavigationService.PushRegistrationPage());
        }
    }
}
