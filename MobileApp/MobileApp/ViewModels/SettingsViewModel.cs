using MobileApp.Services;
using MobileApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private ImageSource avatar;
        private string username;

        public ImageSource Avatar { get => avatar; set => SetProperty(ref avatar, value); }
        public string Username { get => username; set => SetProperty(ref username, value); }

        public Command ChangeAvatarCommand { get; }
        public Command ChangeUsernameCommand { get; }
        public Command ChangePasswordCommand { get; }
        public Command ChangeEmailCommand { get; }
        public Command CloseAccountCommand { get; }
        public Command LogoutCommand { get; }

        public SettingsViewModel()
        {
            Avatar = ImageSource.FromResource("MobileApp.Assets.Images.bsquare.jpg");
            Username = "Username";
            ChangeAvatarCommand = new Command(async () => { });
            ChangeUsernameCommand = new Command(async () => await NavigationService.ShowChangeUsername(Username, false));
            ChangePasswordCommand = new Command(async () => await NavigationService.PushChangePasswordPage());
            ChangeEmailCommand = new Command(async () => await NavigationService.PushChangeEmailPage());
            CloseAccountCommand = new Command(async () => await NavigationService.PushCloseAccountPage());
            LogoutCommand = new Command(async () => 
            {
                if (await GameService.LogoutWithSession())
                    await NavigationService.PushLandingPage();
            });
        }
    }
}
