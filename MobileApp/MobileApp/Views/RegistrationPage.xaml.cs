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
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"..");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RegistrationViewModel viewModel = (RegistrationViewModel)BindingContext;
            viewModel.Username = "";
            viewModel.Password = "";
            viewModel.PasswordVerification = "";
            viewModel.Email = "";
            viewModel.EmailVerification = "";
        }
    }
}