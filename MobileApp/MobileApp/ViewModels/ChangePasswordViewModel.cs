using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private bool passwordLengthValid = false;
        private bool passwordNumberValid = false;
        private bool passwordLetterValid = false;
        private bool passwordSymbolValid = false;
        private bool passwordMatches = false;
        private bool formValid = false;

        private string password;
        private string passwordVerification;
        private const string passwordSymbols = " !\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
        private string badText = "";

        public string Password { get => password; set => SetProperty(ref password, value, onChanged: ValidatePassword); }
        public string PasswordVerification { get => passwordVerification; set => SetProperty(ref passwordVerification, value, onChanged: ValidatePasswordMatch); }
        public string BadText { get => badText; set => SetProperty(ref badText, value); }

        public bool PasswordLengthValid { get => passwordLengthValid; set => SetProperty(ref passwordLengthValid, value); }
        public bool PasswordNumberValid { get => passwordNumberValid; set => SetProperty(ref passwordNumberValid, value); }
        public bool PasswordLetterValid { get => passwordLetterValid; set => SetProperty(ref passwordLetterValid, value); }
        public bool PasswordSymbolValid { get => passwordSymbolValid; set => SetProperty(ref passwordSymbolValid, value); }
        public bool PasswordMatches { get => passwordMatches; set => SetProperty(ref passwordMatches, value); }
        public bool FormValid { get => formValid; set => SetProperty(ref formValid, value); }

        public Command ChangePasswordCommand { get; }

        private void Validate()
        {
            FormValid = PasswordLengthValid && PasswordNumberValid && PasswordSymbolValid && PasswordMatches;
            ChangePasswordCommand.ChangeCanExecute();
        }

        private void ValidatePassword()
        {
            PasswordLengthValid = Password.All(c => char.IsLetter(c) || char.IsNumber(c) || passwordSymbols.Contains(c))
                && Password.Length is >= 8 and <= 128;
            PasswordLetterValid = Password.Any(char.IsLetter);
            PasswordNumberValid = Password.Any(char.IsNumber);
            PasswordSymbolValid = Password.Any(c => passwordSymbols.Contains(c));
            PasswordMatches = !string.IsNullOrWhiteSpace(Password) && Password == PasswordVerification;
            Validate();
        }

        private void ValidatePasswordMatch()
        {
            PasswordMatches = !string.IsNullOrWhiteSpace(Password) && Password == PasswordVerification;
            Validate();
        }

        public ChangePasswordViewModel()
        {
            ChangePasswordCommand = new Command(() => { }, () => !IsBusy && FormValid);
        }
    }
}
