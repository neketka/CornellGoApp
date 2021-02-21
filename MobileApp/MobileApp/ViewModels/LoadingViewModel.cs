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

        public override void CleanupEvents()
        {
            base.CleanupEvents();
            GameService.Client.ConnectionClosed -= Client_ConnectionClosed;
        }

        private async Task Client_ConnectionClosed()
        {
            await NavigationService.ToLoadingPage();
            await Load();
        }

        private async Task Load()
        {
            await Task.Delay(3000);
            await NavigationService.ToLandingPage();
        }
    }
}
