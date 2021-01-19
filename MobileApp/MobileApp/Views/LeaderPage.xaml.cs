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

        private void UserLine_Tapped(object sender, EventArgs e)
        {
        }

        private void LeaderboardLine_Tapped(object sender, EventArgs e)
        {
            Leaderboard.ScrollTo(0);
        }
    }
}