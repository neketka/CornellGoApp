using MobileApp.Models;
using MobileApp.Services;
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
        public Command<int> LoadMoreCommand { get; private set; }

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
            LoadMoreCommand = new Command<int>(async (i) =>
            {
                var service = DependencyService.Get<GameService>();
                var data = service.Client.GetTopPlayers(Players.Count, i);
                await foreach (var entry in data)
                {
                    Players.Add(new LeaderboardPlayer(entry.UserId, entry.Index + 1, ImageSource.FromResource("MobileApp.Assets.Images.bsquare.jpg"), 
                        entry.Username, entry.Score, entry.UserId == service.UserId));
                }
            });
            ProfilePicture = ImageSource.FromResource("MobileApp.Assets.Images.bsquare.jpg");
            RankedPlayers = 20;
            YourRank = 4;
            YourPoints = 997;
            YourUsername = "User3";
        }
    }
}
