using MobileApp.Services;
using MobileApp.ViewModels;
using MobileApp.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Device.SetFlags(new string[] { "Brush_Experimental" });
            MainPage = new AppShell();
            ((AppShell)MainPage).Container.NavigationService.InitializeFirst<LoadingViewModel>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            ((AppShell)MainPage).Container.GetService<IGameService>().Client.SendMetric(CommunicationModel.FrontendMetric.AppSuspended, "");
        }

        protected override async void OnResume()
        {
            Console.WriteLine("Resuming");
            await Task.Delay(1000);
            ViewModelContainer container = ((AppShell)MainPage).Container;
            IGameService gameService = container.GetService<IGameService>();
            if (!gameService.Client.Connected)
                await container.NavigationService.NavigateToRoot();
            else if (container.NavigationService.CurrentViewModel is not LandingViewModel and not GameViewModel)
            {
                if (gameService.IsLoggedIn)
                {
                    await container.NavigationService.NavigateTo<LandingViewModel>(animate: false);
                    await container.NavigationService.NavigateTo<GameViewModel>();
                }
                else await container.NavigationService.NavigateTo<LandingViewModel>();
            }
        }
    }
}