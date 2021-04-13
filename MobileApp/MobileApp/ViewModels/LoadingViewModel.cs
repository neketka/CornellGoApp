using MobileApp.Services;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class LoadingViewModel : BaseViewModel
    {
        private readonly IGameService gameService;
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;

        public LoadingViewModel(IGameService gameService, INavigationService navigationService, IDialogService dialogService)
        {
            this.gameService = gameService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;

            gameService.Client.ConnectionClosed += Client_ConnectionClosed;
            gameService.Client.Reconnecting += Client_Reconnecting;
            gameService.Client.Reconnected += Client_Reconnected;
            Load();
        }

        private async Task Client_Reconnecting()
        {
            await CrossGeolocator.Current.StopListeningAsync();
            await navigationService.NavigateToRoot();
        }

        private async Task Client_Reconnected()
        {
            await Login();
        }

        private async Task Client_ConnectionClosed()
        {
            await CrossGeolocator.Current.StopListeningAsync();
            await navigationService.NavigateToRoot();
            await Load();
        }

        private async Task Load()
        {
            while (!gameService.Client.Connected)
            {
                try
                {
                    await gameService.Client.Connect();
                    break;
                }
                catch (Exception e)
                {
                    await dialogService.ShowConnectionError(e.Message);
                }
            }
            await Login();
        }

        private async Task Login()
        {
            Console.WriteLine("Logging in");
            if (await gameService.AttemptRelog())
            {
                Console.WriteLine("To landing");
                await navigationService.NavigateTo<LandingViewModel>(animate: false);
                await navigationService.NavigateTo<GameViewModel>();
            }
            else await navigationService.NavigateTo<LandingViewModel>();
        }
    }
}