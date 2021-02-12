using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string username;
        private string password;
        private string badText;

        public string Username { get => username; set => SetProperty(ref username, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string BadText { get => badText; set => SetProperty(ref badText, value); }
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () =>
            {
                IsBusy = true;
                LoginCommand.ChangeCanExecute();

                try
                {
                    await Task.Delay(3000);
                    await NavigationService.PushGamePage();
                }
                catch
                {

                }
                finally
                {
                    IsBusy = false;
                    LoginCommand.ChangeCanExecute();
                }
            }, () => !IsBusy);
        }
    }
}
