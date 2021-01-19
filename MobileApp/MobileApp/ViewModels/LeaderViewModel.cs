using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class LeaderViewModel : BaseViewModel
    {
        private ImageSource profilePicture;
        private int rankedPlayers;
        private int yourRank;
        private int yourPoints;
        private string yourUsername;

        public ObservableCollection<LeaderboardPlayer> Players { get; private set; }
        public Command LoadMore { get; private set; }

        public ImageSource ProfilePicture { get => profilePicture; set => SetProperty(ref profilePicture, value); }
        public int RankedPlayers { get => rankedPlayers; set => SetProperty(ref rankedPlayers, value); }
        public int YourRank { get => yourRank; set => SetProperty(ref yourRank, value); }
        public int YourPoints { get => yourPoints; set => SetProperty(ref yourPoints, value); }
        public string YourUsername { get => yourUsername; set => SetProperty(ref yourUsername, value); }
        public LeaderViewModel()
        {
            Console.WriteLine("LeaderViewModel");

            Players = new ObservableCollection<LeaderboardPlayer>(Enumerable.Range(1, 20)
                .Select(i => new LeaderboardPlayer(i.ToString(), i, ImageSource.FromResource(i == 3 ? "MobileApp.Assets.Images.bsquare.jpg" :
                    "MobileApp.Assets.Images.logo.png"), "User" + i, 1000 - i, i == 3)));
            LoadMore = new Command(() => IsBusy = false);
            ProfilePicture = ImageSource.FromResource("MobileApp.Assets.Images.bsquare.jpg");
            RankedPlayers = 20;
            YourRank = 4;
            YourPoints = 997;
            YourUsername = "User3";
        }
    }
}
