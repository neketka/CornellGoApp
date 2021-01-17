using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private double progress = 0.0;
        private string progressString = "10 min";
        private int points = 1;
        private int membersReady = 0;
        private string groupCode = "ABCDEF12";
        private bool isHost = true;
        private int maxMembers = 8;
        private ImageSource challengeImage;

        public double Progress { get => progress; set => SetProperty(ref progress, value); }
        public string ProgressString { get => progressString; set => SetProperty(ref progressString, value); }
        public int Points { get => points; set => SetProperty(ref points, value); }
        public int MembersReady { get => membersReady; set => SetProperty(ref membersReady, value); }
        public string GroupCode { get => groupCode; set => SetProperty(ref groupCode, value); }
        public bool IsHost { get => isHost; set => SetProperty(ref isHost, value); }
        public ObservableCollection<GroupMember> GroupMembers { get; private set; }
        public int MaxMembers { get => maxMembers; set => SetProperty(ref maxMembers, value); }
        public ImageSource ChallengeImage { get => challengeImage; set => SetProperty(ref challengeImage, value); }

        public Command<string> LeaveCommand { get; private set; }
        public Command JoinCommand { get; private set; }

        public GameViewModel()
        {
            GroupMembers = new ObservableCollection<GroupMember>
            {
                new GroupMember("a", ImageSource.FromResource("MobileApp.Assets.Images.bsquare.jpg"), true, true, true, "Your username", 12),
                new GroupMember("b", ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, false, "Not ready person", 0),
                new GroupMember("c", ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, false, "Not ready person", 0),
                new GroupMember("d", ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, true, "Ready person", 0),
                new GroupMember("e", ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, true, "Ready person", 0),
                new GroupMember("f", ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, true, "Ready person", 0),
                new GroupMember("g", ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, false, "Not ready person", 0),
                new GroupMember("h", ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, false, "Not ready person", 0)
            };

            ChallengeImage = ImageSource.FromResource("MobileApp.Assets.Images.grid.png");
            LeaveCommand = new Command<string>(s => { });
            JoinCommand = new Command(() => { });
        }
    }
}
