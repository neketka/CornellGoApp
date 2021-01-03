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
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
            ((GameViewModel)BindingContext).PropertyChanged += GamePage_PropertyChanged;
            AdjustTemp(((GameViewModel)BindingContext).Progress);
        }

        private void AdjustTemp(double value)
        {
            TempBar.TranslateTo((Width - 16.0) * value + 16.0, TranslationY);
        }

        private void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            DisplayAlert("Changed", ((GameViewModel)BindingContext).Progress.ToString(), "Cancel");
            if (e.PropertyName == nameof(GameViewModel.Progress)) AdjustTemp(((GameViewModel)BindingContext).Progress);
        }
    }
}