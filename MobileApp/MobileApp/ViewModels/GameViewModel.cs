using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using MobileApp.Services;

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
            set => SetProperty(ref victoryMode, value, onChanged: () => { 
                if (value) (OldChallengeImage, OldChallengePoints) = (ChallengeImage, Points); 
            });
        }
        public Command FindOutMoreCommand { get; }
        public Command NextChallengeCommand { get; }
        public Command DoVictoryCommand { get; }

        public Command<string> LeaveCommand { get; }
        public Command JoinCommand { get; }

        private ImageSource pfp;

        private IGameService gameService;

        public GameViewModel(IGameService gameService, INavigationService navigationService, IDialogService dialogService)
        {
            this.gameService = gameService;

            pfp = ImageSource.FromResource("MobileApp.Assets.Images.profile.png");
            GroupMembers = new ObservableCollection<GroupMember>();

            LeaveCommand = new Command<string>(async (s) =>
            {
                GroupMember gm = GroupMembers.First(e => e.Id == s);
                if (gm.IsYou ? (gm.IsHost ? await dialogService.ConfirmDisband(false)
                                          : await dialogService.ConfirmLeave(false))
                             : await dialogService.ConfirmKick(gm.Username))
                {
                    await gameService.Client.Kick(gm.Id);
                }
            });
            JoinCommand = new Command(async () =>
            {
                if (await dialogService.ConfirmDisband(true))
                {
                    string id = await dialogService.ShowJoinGroup(false);
                    if (id != null)
                        await gameService.Client.JoinGroup(id);
                }
            });
            FindOutMoreCommand = new Command(async () => { await dialogService.ShowServerError(); });
            NextChallengeCommand = new Command(() => VictoryMode = false );

            DoVictoryCommand = new Command(() =>
            {
                OldChallengeName = "Test Challenge";
                ImageSource source = ImageSource.FromUri(new Uri("https://media-cdn.tripadvisor.com/media/photo-m/1280/13/bd/12/dd/triphammer-falls-the.jpg"));
                VictoryMode = true;
                ChallengeImage = source;
                Points = 100;
                ChallengeDescription = "New challenge description";
            });

            gameService.Client.ChallengeFinished += Client_ChallengeFinished;
            gameService.Client.ChallengeUpdated += Client_ChallengeUpdated;
            gameService.Client.GroupDataUpdated += Client_GroupDataUpdated;
            gameService.Client.GroupMemberLeft += Client_GroupMemberLeft;
            gameService.Client.GroupMemberUpdated += Client_GroupMemberUpdated;
            gameService.ProgressUpdated += GameService_ProgressUpdated;

            LoadInitialData();
        }

        private async Task LoadInitialData()
        {
            var challenge = await gameService.Client.GetChallengeData();
            ChallengeImage = new UriImageSource
            {
                Uri = new(challenge.ImageUrl),
                CachingEnabled = true
            };
            ChallengeDescription = challenge.Description;
            Points = challenge.Points;

            GroupCode = await gameService.Client.GetFriendlyGroupId();

            var members = await gameService.Client.GetGroupMembers();
            AddMembers(members);
            UpdateGroupDataFromList();
        }

        private async Task Client_GroupMemberUpdated(CommunicationModel.GroupMemberData data)
        {
            int index = GetGroupMemberIndex(data.UserId);
            var newMember = GroupMembers[index] with {
                IsHost = data.IsHost, IsReady = data.IsDone, Username = data.Username, Score = data.Points
            };

            await Device.InvokeOnMainThreadAsync(() => GroupMembers[index] = newMember);
        }

        private async Task Client_GroupMemberLeft(string userId)
        {
            int index = GetGroupMemberIndex(userId);
            await Device.InvokeOnMainThreadAsync(() => GroupMembers.RemoveAt(index));
        }

        private async Task Client_GroupDataUpdated(string friendlyId, CommunicationModel.GroupMemberData[] members)
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                AddMembers(members);
                UpdateGroupDataFromList();
            });
        }

        private async Task Client_ChallengeUpdated(CommunicationModel.ChallengeData data)
        {
            ImageSource img = new UriImageSource
            {
                Uri = new(data.ImageUrl),
                CachingEnabled = true
            };

            string oldName = await gameService.Client.GetPrevChallengeName();

            await Device.InvokeOnMainThreadAsync(() =>
            {
                OldChallengeName = oldName;
                VictoryMode = true;
                ChallengeImage = img;
                Points = data.Points;
            });
        }

        private async Task Client_ChallengeFinished()
        {
            await Device.InvokeOnMainThreadAsync(() => IsDone = true);
        }

        private int GetGroupMemberIndex(string id)
        {
            int index = 0;
            for (; index < GroupMembers.Count; ++index)
            {
                if (GroupMembers[index].Id == id)
                    return index;
            }
            return -1;
        }

        private void AddMembers(CommunicationModel.GroupMemberData[] members)
        {
            GroupMembers.Clear();
            foreach (var data in members.OrderByDescending(d => d.UserId == gameService.UserId ? 2 : d.IsHost ? 1 : 0))
            {
                GroupMembers.Add(new(data.UserId, pfp, data.UserId == gameService.UserId, data.IsHost,
                    data.IsDone, data.Username, data.Points));
            }
        }

        private void GameService_ProgressUpdated(CommunicationModel.ChallengeProgressData obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Progress = obj.Progress;
                ProgressString = obj.WalkDistance;
            });
        }

        private void UpdateGroupDataFromList()
        {
            int membersDone = 0;
            foreach (var member in GroupMembers)
            {
                if (member.IsReady)
                    ++membersDone;

                if (member.IsYou)
                {
                    IsHost = member.IsHost;
                    IsDone = member.IsReady;
                }
            }
            MembersReady = membersDone;
            MaxMembers = 8;
        }

        public override void OnDestroying()
        {
            gameService.Client.ChallengeFinished -= Client_ChallengeFinished;
            gameService.Client.ChallengeUpdated -= Client_ChallengeUpdated;
            gameService.Client.GroupDataUpdated -= Client_GroupDataUpdated;
            gameService.Client.GroupMemberLeft -= Client_GroupMemberLeft;
            gameService.Client.GroupMemberUpdated -= Client_GroupMemberUpdated;
            gameService.ProgressUpdated -= GameService_ProgressUpdated;
        }
    }
}
