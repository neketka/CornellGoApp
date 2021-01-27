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
        private string challengeDescription;
        private ImageSource oldChallengeImage;
        private string oldChallengeName;
        private int oldChallengePoints;
        private bool victoryMode;
        private bool isDone = false;

        public double Progress { get => progress; set => SetProperty(ref progress, value); }
        public string ProgressString { get => progressString; set => SetProperty(ref progressString, value); }
        public int Points { get => points; set => SetProperty(ref points, value); }
        public ImageSource ChallengeImage { get => challengeImage; set => SetProperty(ref challengeImage, value); }
        public string ChallengeDescription { get => challengeDescription; set => SetProperty(ref challengeDescription, value); }

        public int MembersReady { get => membersReady; set => SetProperty(ref membersReady, value); }
        public string GroupCode { get => groupCode; set => SetProperty(ref groupCode, value); }
        public bool IsHost { get => isHost; set => SetProperty(ref isHost, value); }
        public ObservableCollection<GroupMember> GroupMembers { get; private set; }
        public int MaxMembers { get => maxMembers; set => SetProperty(ref maxMembers, value); }
        public bool IsDone { get => isDone; set => SetProperty(ref isDone, value); }

        public ImageSource OldChallengeImage { get => oldChallengeImage; set => SetProperty(ref oldChallengeImage, value); }
        public string OldChallengeName { get => oldChallengeName; set => SetProperty(ref oldChallengeName, value); }
        public int OldChallengePoints { get => oldChallengePoints; set => SetProperty(ref oldChallengePoints, value); }
        public bool VictoryMode 
        { 
            get => victoryMode;
            set => SetProperty(ref victoryMode, value, onChanged: () => { if (value) (OldChallengeImage, OldChallengePoints) = (ChallengeImage, Points); });
        }
        public Command FindOutMoreCommand { get; }
        public Command NextChallengeCommand { get; }
        public Command DoVictoryCommand { get; }

        public Command<string> LeaveCommand { get; }
        public Command JoinCommand { get; }

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
            ChallengeDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt";

            LeaveCommand = new Command<string>(s => { });
            JoinCommand = new Command(async () => 
            {
                await NavigationService.ShowJoinGroup();
            });
            FindOutMoreCommand = new Command(() => { });
            NextChallengeCommand = new Command(() => { VictoryMode = false; });

            DoVictoryCommand = new Command(() =>
            {
                OldChallengeName = "Test Challenge";
                ImageSource source = ImageSource.FromUri(new Uri("https://media-cdn.tripadvisor.com/media/photo-m/1280/13/bd/12/dd/triphammer-falls-the.jpg"));
                VictoryMode = true;
                ChallengeImage = source;
                Points = 100;
                ChallengeDescription = "New challenge description";
            });
        }
    }
}
