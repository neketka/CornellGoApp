using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class CloseAccountViewModel : BaseViewModel
    {
        private string password;
        public string Password { get => password; set => SetProperty(ref password, value, onChanged: CloseAccountCommand.ChangeCanExecute); }
        public Command CloseAccountCommand { get; }

        public CloseAccountViewModel(IDialogService dialogService)
        {
            CloseAccountCommand = new Command(async () => 
            {
                await dialogService.ShowServerError();
            }, () => !(IsBusy || string.IsNullOrWhiteSpace(Password)));
        }
    }
}
