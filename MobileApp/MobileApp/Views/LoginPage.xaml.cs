using MobileApp.ViewModels;
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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            ((LoginViewModel)BindingContext).PropertyChanged += LoginPage_PropertyChanged;
        }

        private async void LoginPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ShouldAppear" && !((LoginViewModel)BindingContext).ShouldAppear)
                await Shell.Current.GoToAsync($"//{nameof(GamePage)}");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((LoginViewModel)BindingContext).ShouldAppear = true;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void Register_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}/{nameof(RegistrationPage)}");
        }
    }
}