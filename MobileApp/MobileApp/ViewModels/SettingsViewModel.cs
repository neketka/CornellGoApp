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
        public Command CloseAccountCommand { get; }
        public Command LogoutCommand { get; }

        public SettingsViewModel()
        {
            Avatar = ImageSource.FromResource("MobileApp.Assets.Images.bsquare.jpg");
            ChangeAvatarCommand = new Command(async () => { });
            ChangeUsernameCommand = new Command(async () => { });
            ChangePasswordCommand = new Command(async () => { });
            CloseAccountCommand = new Command(async () => { });
            LogoutCommand = new Command(async () => 
            {
                if (await GameService.LogoutWithSession())
                    await NavigationService.PushLandingPage();
            });
        }
    }
}
