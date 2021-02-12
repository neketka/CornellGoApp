using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class ChangeEmailViewModel : BaseViewModel
    {
        private string email;
        private string emailVerification;
        private string badText;

        private bool emailAddressValid = false;
        private bool emailAddressMatches = false;
        private bool formValid = false;

        public string Email { get => email; set => SetProperty(ref email, value, onChanged: ValidateEmail); }
        public string EmailVerification { get => emailVerification; set => SetProperty(ref emailVerification, value, onChanged: ValidateEmailMatch); }
        public string BadText { get => badText; set => SetProperty(ref badText, value); }

        public bool EmailAddressValid { get => emailAddressValid; set => SetProperty(ref emailAddressValid, value); }
        public bool EmailAddressMatches { get => emailAddressMatches; set => SetProperty(ref emailAddressMatches, value); }
        public bool FormValid { get => formValid; set => SetProperty(ref formValid, value); }

        public Command ChangeEmailCommand { get; }

        private void Validate()
        {
            FormValid = EmailAddressValid && EmailAddressMatches;
            ChangeEmailCommand.ChangeCanExecute();
        }

        private void ValidateEmail()
        {
            EmailAddressValid = IsValidEmail(email);
            EmailAddressMatches = !string.IsNullOrWhiteSpace(Email) && Email == EmailVerification;
            Validate();
        }

        private void ValidateEmailMatch()
        {
            EmailAddressMatches = !string.IsNullOrWhiteSpace(Email) && Email == EmailVerification;
            Validate();
        }

        private bool IsValidEmail(string emailaddress)
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailaddress);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ChangeEmailViewModel()
        {
            ChangeEmailCommand = new Command(async () => 
            {
                if (await GameService.Client.ChangeEmail(Email))
                    await NavigationService.GoBack();
                else
                    BadText = "Either this email is in use, or it is not a valid email.";
            }, () => !IsBusy && FormValid);
        }
    }
}
