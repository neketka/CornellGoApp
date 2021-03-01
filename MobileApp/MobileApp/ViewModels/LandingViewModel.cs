using MobileApp.Services;
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

        public LandingViewModel(INavigationService navigationService)
        {
            SigninCommand = new Command(async () => await navigationService.NavigateTo<LoginViewModel>());
            SignupCommand = new Command(async () => await navigationService.NavigateTo<RegistrationViewModel>());
        }
    }
}
