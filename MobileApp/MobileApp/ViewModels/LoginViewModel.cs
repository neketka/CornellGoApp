using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
