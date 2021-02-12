using MobileApp.Services;
using MobileApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Linq;

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
            Avatar = ImageSource.FromResource("MobileApp.Assets.Images.profile.png");
            Username = "Username";
            ChangeAvatarCommand = new Command(async () => 
            {
                await MediaPicker.PickPhotoAsync();
            });
            ChangeUsernameCommand = new Command(async () =>
            {
                bool isValid = true;

                string newName = Username;
                do
                {
                    newName = await NavigationService.ShowChangeUsername(newName, isValid);

                    bool lenValid = newName.Length is >= 1 and <= 16;
                    bool formatValid = newName.All(c => char.IsLetterOrDigit(c) || c == '_') && !string.IsNullOrWhiteSpace(newName);

                    isValid = lenValid && formatValid;
                }
                while (!isValid && newName != null);

                if (newName == null)
                    return;

                if (await GameService.Client.ChangeUsername(newName))
                    Username = newName;
                else
                    await NavigationService.ShowServerError();
            });
            ChangePasswordCommand = new Command(async () => await NavigationService.PushChangePasswordPage());
            ChangeEmailCommand = new Command(async () => await NavigationService.PushChangeEmailPage());
            CloseAccountCommand = new Command(async () => await NavigationService.PushCloseAccountPage());
            LogoutCommand = new Command(async () => 
            {
                if (await GameService.LogoutWithSession())
                    await NavigationService.ToLandingPage();
            });
        }
    }
}
