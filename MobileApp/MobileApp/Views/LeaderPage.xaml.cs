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
    public partial class LeaderPage : ContentPage
    {
        public LeaderPage()
        {
            InitializeComponent();
        }

        private async void UserLine_Tapped(object sender, EventArgs e)
        {
            LeaderViewModel vm = (LeaderViewModel)BindingContext;
            int index = vm.YourRank - 1;
            if (index >= vm.Players.Count)
            {

            }
        }

        private async void LeaderboardLine_Tapped(object sender, EventArgs e)
        {
            Leaderboard.ScrollTo(0);
        }
    }
}