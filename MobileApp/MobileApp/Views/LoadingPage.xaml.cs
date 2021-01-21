using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
            DependencyService.Get<GameService>().Client.ConnectionClosed += Client_ConnectionClosed;
        }

        private async Task Client_ConnectionClosed()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoadingPage)}");
        }

        protected override async void OnAppearing()
        {
            await Task.Delay(1500);
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }
    }
}