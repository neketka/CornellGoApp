using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private const string passwordSymbols = " !\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

        private string username;
        private string password;
        private string passwordVerification;
        private string email;
        private string emailVerification;
        private string badText;

        private bool usernameLengthValid = false;
        private bool usernameFormatValid = false;
        private bool passwordLengthValid = false;
        private bool passwordNumberValid = false;
        private bool passwordLetterValid = false;
        private bool passwordSymbolValid = false;
        private bool passwordMatches = false;
        private bool emailAddressValid = false;
        private bool emailAddressMatches = false;
        private bool formValid = false;

        public bool UsernameLengthValid { get => usernameLengthValid; set => SetProperty(ref usernameLengthValid, value); }
        public bool UsernameFormatValid { get => usernameFormatValid; set => SetProperty(ref usernameFormatValid, value); }
        public bool PasswordLengthValid { get => passwordLengthValid; set => SetProperty(ref passwordLengthValid, value); }
        public bool PasswordNumberValid { get => passwordNumberValid; set => SetProperty(ref passwordNumberValid, value); }
        public bool PasswordLetterValid { get => passwordLetterValid; set => SetProperty(ref passwordLetterValid, value); }
        public bool PasswordSymbolValid { get => passwordSymbolValid; set => SetProperty(ref passwordSymbolValid, value); }
        public bool PasswordMatches { get => passwordMatches; set => SetProperty(ref passwordMatches, value); }
        public bool EmailAddressValid { get => passwordNumberValid; set => SetProperty(ref emailAddressValid, value); }
        public bool EmailAddressMatches { get => emailAddressMatches; set => SetProperty(ref emailAddressMatches, value); }
        public bool FormValid { get => formValid; set => SetProperty(ref formValid, value); }

        public string Username { get => username; set => SetProperty(ref username, value, onChanged: ValidateUsername); }
        public string Password { get => password; set => SetProperty(ref password, value, onChanged: ValidatePassword); }
        public string PasswordVerification { get => passwordVerification; set => SetProperty(ref passwordVerification, value, onChanged: ValidatePasswordMatch); }
        public string Email { get => email; set => SetProperty(ref email, value, onChanged: ValidateEmail); }
        public string EmailVerification { get => emailVerification; set => SetProperty(ref emailVerification, value, onChanged: ValidateEmailMatch); }
        public string BadText { get => badText; set => SetProperty(ref badText, value); }
        public Command RegisterCommmand { get; }

        private void Validate()
        {
            FormValid = UsernameLengthValid && UsernameFormatValid && PasswordLengthValid && PasswordNumberValid && PasswordSymbolValid &&
                PasswordMatches && EmailAddressValid && EmailAddressMatches;
        }

        private void ValidateUsername()
        {
            UsernameLengthValid = Username.Length is >= 1 and <= 24;
            UsernameFormatValid = Username.All(c => char.IsLetterOrDigit(c) || c == '_') && !string.IsNullOrWhiteSpace(Username);
            Validate();
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

        private void ValidateEmail()
        {
            EmailAddressValid = Regex.IsMatch(Email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            EmailAddressMatches = !string.IsNullOrWhiteSpace(Email) && Email == EmailVerification;
            Validate();
        }

        private void ValidateEmailMatch()
        {
            EmailAddressMatches = !string.IsNullOrWhiteSpace(Email) && Email == EmailVerification;
            Validate();
        }

        public RegistrationViewModel()
        {
            RegisterCommmand = new Command(async () => { });
        }
    }
}
