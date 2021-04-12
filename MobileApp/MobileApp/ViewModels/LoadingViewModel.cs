using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            Load();
        }

        public override void OnDestroying()
        {
            gameService.Client.ConnectionClosed -= Client_ConnectionClosed;
        }

        public override Task OnReturning(object parameter)
        {
            return Client_ConnectionClosed();
        }

        private async Task Client_ConnectionClosed()
        {
            await navigationService.NavigateBackTo<LoadingViewModel>();
            await Load();
        }

        private async Task Load()
        {
            while (true)
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
            if (await gameService.AttemptRelog())
            {
                await navigationService.NavigateTo<LandingViewModel>(animate: false);
                await navigationService.NavigateTo<GameViewModel>();
            }
            else await navigationService.NavigateTo<LandingViewModel>();
        }
    }
}
