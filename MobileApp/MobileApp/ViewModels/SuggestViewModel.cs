using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class SuggestViewModel : BaseViewModel
    {
        public Command CancelCommand { get; private set; }
        
        public SuggestViewModel()
        {
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync("//play"));
        }
    }
}
