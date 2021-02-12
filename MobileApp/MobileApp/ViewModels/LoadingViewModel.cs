using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.ViewModels
{
    public class LoadingViewModel : BaseViewModel
    {
        public LoadingViewModel()
        {
            GameService.Client.ConnectionClosed += Client_ConnectionClosed;
            Load();
        }

        private async Task Client_ConnectionClosed()
        {
            await NavigationService.ToLoadingPage();
        }

        private async Task Load()
        {
            await Task.Delay(3000);
            await NavigationService.ToLandingPage();
        }
    }
}
