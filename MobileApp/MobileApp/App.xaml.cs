using MobileApp.Services;
using MobileApp.ViewModels;
using MobileApp.Views;
using System;
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

        protected override void OnResume()
        {
            ViewModelContainer container = ((AppShell)MainPage).Container;
            IGameService gameService = container.GetService<IGameService>();
            if (!gameService.Client.Connected)
                container.NavigationService.NavigateToRoot();
            else if (gameService.IsLoggedIn)
            {
                container.NavigationService.NavigateTo<LandingViewModel>(animate: false);
                container.NavigationService.NavigateTo<GameViewModel>();
            }
            else container.NavigationService.NavigateTo<LandingViewModel>();
        }
    }
}