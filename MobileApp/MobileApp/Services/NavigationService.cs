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

        public async Task<string> ShowJoinGroup()
        {
            return await Shell.Current.DisplayPromptAsync("Join Group", "Enter the code between the brackets from a group member's screen.");
        }

        public async Task<bool> ConfirmKick(string user)
        {
        }

        public async Task<bool> ConfirmDisband(bool isJoining)
        {
        }

        public async Task<bool> ConfirmLeave(bool isJoining)
        {
        }

        public async Task<string> ShowChangeUsername(string oldName)
        {

        }

        public async Task<(string pass, string confirmPass)> ShowChangePassword(bool invalid)
        {

        }

        public async Task<(string pass, string email)> ShowChangeEmail(bool invalidPass, bool invalidEmail)
        {

        }

        public async Task<string> ShowCloseAccount(bool invalid)
        {
        }
    }
}
