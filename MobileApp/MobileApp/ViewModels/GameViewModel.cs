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
        public double Progress { get => progress; set => SetProperty(ref progress, value); }

        private string progressString = "10 min";
        public string ProgressString { get => progressString; set => SetProperty(ref progressString, value); }

        private string pointsString = "1 Point";
        public string PointsString { get => pointsString; set => SetProperty(ref pointsString, value); }

        private int membersReady = 0;
        public int MembersReady { get => membersReady; set => SetProperty(ref membersReady, value); }

        private string groupCode = "ABCDEFGH12";
        public string GroupCode { get => groupCode; set => SetProperty(ref groupCode, value); }

        private bool isHost = true;
        public bool IsHost { get => isHost; set => SetProperty(ref isHost, value); }

        public ObservableCollection<GroupMember> GroupMembers { get; private set; }
        private int maxMembers = 8;
        public int MaxMembers { get => maxMembers; set => SetProperty(ref maxMembers, value); }

        public GameViewModel()
        {
            GroupMembers = new ObservableCollection<GroupMember>
            {
                new GroupMember(ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), true, true, true, "Your username", 12),
                new GroupMember(ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, false, "Not ready person", 0),
                new GroupMember(ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, false, "Not ready person", 0),
                new GroupMember(ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, true, "Ready person", 0),
                new GroupMember(ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, true, "Ready person", 0),
                new GroupMember(ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, true, "Ready person", 0),
                new GroupMember(ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, false, "Not ready person", 0),
                new GroupMember(ImageSource.FromResource("MobileApp.Assets.Images.logo.png"), false, false, false, "Not ready person", 0)
            };
        }
    }
}
