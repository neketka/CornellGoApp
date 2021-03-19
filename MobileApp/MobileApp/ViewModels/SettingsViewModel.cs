using MobileApp.Services;
using MobileApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Linq;
using System.Threading.Tasks;

namespace MobileApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private ImageSource avatar;
        private string username;
        private IGameService gameService;

        public ImageSource Avatar { get => avatar; set => SetProperty(ref avatar, value); }
        public string Username { get => username; set => SetProperty(ref username, value); }

        public Command ChangeAvatarCommand { get; }
        public Command ChangeUsernameCommand { get; }
        public Command ChangePasswordCommand { get; }
        public Command ChangeEmailCommand { get; }
        public Command CloseAccountCommand { get; }
        public Command LogoutCommand { get; }

        public SettingsViewModel(IGameService gameService, INavigationService navigationService, IDialogService dialogService)
        {
            this.gameService = gameService;

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
                    newName = await dialogService.ShowChangeUsername(newName, !isValid);

                    bool lenValid = !string.IsNullOrWhiteSpace(newName) && newName.Length is >= 1 and <= 16;
                    bool formatValid = !string.IsNullOrWhiteSpace(newName) && newName.All(c => char.IsLetterOrDigit(c) || c == '_');

                    isValid = lenValid && formatValid;
                }
                while (!isValid && newName != null);

                if (newName == null)
                    return;

                if (await gameService.Client.ChangeUsername(newName))
                    Username = newName;
                else
                    await dialogService.ShowServerError();
            });
            ChangePasswordCommand = new Command(async () => await navigationService.NavigateTo<ChangePasswordViewModel>());
            ChangeEmailCommand = new Command(async () => await navigationService.NavigateTo<ChangeEmailViewModel>());
            CloseAccountCommand = new Command(async () => await navigationService.NavigateTo<CloseAccountViewModel>());
            LogoutCommand = new Command(async () => 
            {
                if (await gameService.LogoutWithSession())
                    await navigationService.NavigateBackTo<LandingViewModel>();
            });
        }

        public override Task OnEntering(object parameter)
        {
            gameService.Client.SendMetric(CommunicationModel.FrontendMetric.OpenSettings, "");
            return Task.CompletedTask;
        }
    }
}
