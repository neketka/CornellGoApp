using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.Services
{
    public interface IDialogService
    {
        Task<bool> ConfirmDisband(bool isJoining);
        Task<bool> ConfirmKick(string user);
        Task<bool> ConfirmLeave(bool isJoining);
        Task<string> ShowChangeUsername(string oldName, bool invalid);
        Task ShowConnectionError();
        Task<string> ShowJoinGroup(bool invalid);
        Task ShowServerError();
    }

    public class DialogService : IDialogService
    {
        public async Task<string> ShowJoinGroup(bool invalid)
        {
            return await Shell.Current.DisplayPromptAsync("Join Group", invalid ? "Could not join this group! Make sure you are entering the correct code."
                : "Enter the code between the brackets from a group member's screen.");
        }

        public async Task<bool> ConfirmKick(string user)
        {
            return await Shell.Current.DisplayAlert("Kick member", $"Are you sure you want to kick \"{user}\"?", "Kick", "Cancel");
        }

        public async Task<bool> ConfirmDisband(bool isJoining)
        {
            if (isJoining)
                return await Shell.Current.DisplayAlert("Disband group", $"Joining another group will disband this one. Do you wish to proceed?", "Continue", "Cancel");
            else
                return await Shell.Current.DisplayAlert("Disband group", $"Are you sure you want to disband this group?", "Disband", "Cancel");
        }

        public async Task<bool> ConfirmLeave(bool isJoining)
        {
            if (isJoining)
                return await Shell.Current.DisplayAlert("Leave group", $"Joining another group will leave this one. Do you wish to proceed?", "Continue", "Cancel");
            else
                return await Shell.Current.DisplayAlert("Leave group", $"Are you sure you want to leave this group?", "Leave", "Cancel");
        }

        public async Task<string> ShowChangeUsername(string oldName, bool invalid)
        {
            return await Shell.Current.DisplayPromptAsync("Change username", invalid ? "Invalid username! Must have letters, numbers, underscores, and 1-24 characters." :
                "Enter your desired username (letters, numbers, underscores, 1-16 characters).", "Change", "Cancel", "Username", 16, null, oldName);
        }

        public async Task ShowServerError()
        {
            await Shell.Current.DisplayAlert("Error!", "Server returned an error while processing your request!", "OK");
        }

        public async Task ShowConnectionError(string message)
        {
            await Shell.Current.DisplayAlert("Could not connect to server!", $"Please check your wireless connection and hit retry. Error: {message}", "Retry");
        }
    }
}
