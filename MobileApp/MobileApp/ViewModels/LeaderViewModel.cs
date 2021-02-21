﻿using MobileApp.Models;
using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private ImageSource pfp;

        public ObservableCollection<LeaderboardPlayer> Players { get; private set; }
        public Command<int> LoadMoreCommand { get; private set; }

        public ImageSource ProfilePicture { get => profilePicture; set => SetProperty(ref profilePicture, value); }
        public int RankedPlayers { get => rankedPlayers; set => SetProperty(ref rankedPlayers, value); }
        public int YourRank { get => yourRank; set => SetProperty(ref yourRank, value); }
        public int YourPoints { get => yourPoints; set => SetProperty(ref yourPoints, value); }
        public string YourUsername { get => yourUsername; set => SetProperty(ref yourUsername, value); }
        public LeaderViewModel()
        {
            pfp = ImageSource.FromResource("MobileApp.Assets.Images.profile.png");

            Players = new ObservableCollection<LeaderboardPlayer>();
            LoadMoreCommand = new Command<int>((i) => LoadLeaderData(i));

            ProfilePicture = pfp;
            GameService.Client.ScorePositionsUpdated += RerankPlayer;
            LoadData();
        }

        private async Task RerankPlayer(string userId, string username, int oldIndex, int newIndex, int score)
        {
            if (YourRank > oldIndex)
                await LoadUserData();
            
            await Device.InvokeOnMainThreadAsync(() =>
            {
                int minModifyIndex = Math.Min(oldIndex, newIndex);

                Players.Move(oldIndex, newIndex);
                var transform = Players
                    .Skip(minModifyIndex)
                    .Select<LeaderboardPlayer, LeaderboardPlayer>((p, i) =>
                        userId == p.Id ? new(p.Id, i + minModifyIndex + 1, pfp, username, score, p.IsYou)
                                       : new(p.Id, i + minModifyIndex + 1, pfp, p.Username, p.Points, p.IsYou));

                foreach (var newPlayer in transform)
                {
                    Players[minModifyIndex] = newPlayer;
                    ++minModifyIndex;
                }
            });
        }

        public override void CleanupEvents()
        {
            GameService.Client.ScorePositionsUpdated -= RerankPlayer;
        }

        private async Task LoadData()
        {
            IsBusy = true;

            try
            {
                await LoadUserData();
                await LoadLeaderData(20);
            }
            catch
            {
                await NavigationService.ShowServerError();
                await NavigationService.GoBack();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadUserData()
        {
            var data = await GameService.Client.GetUserData();

            await Device.InvokeOnMainThreadAsync(() =>
            {
                RankedPlayers = data.TotalUserCount;
                YourRank = data.RankIndex + 1;
                YourPoints = data.Points;
                YourUsername = data.Username;
            });
        }

        private async Task LoadLeaderData(int count)
        {
            var data = GameService.Client.GetTopPlayers(Players.Count, count);
            List<LeaderboardPlayer> tempPlayers = new List<LeaderboardPlayer>();
            await foreach (var entry in data)
            {
                tempPlayers.Add(new(entry.UserId, entry.Index + 1, pfp, entry.Username, entry.Score, entry.UserId == GameService.UserId));
            }
            await Device.InvokeOnMainThreadAsync(() => tempPlayers.ForEach(p => Players.Add(p)));
        }
    }
}
