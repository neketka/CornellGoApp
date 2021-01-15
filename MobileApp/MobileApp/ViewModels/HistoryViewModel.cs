using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public ObservableCollection<HistoryEntry> HistoryEntries { get; private set; }
        public HistoryViewModel()
        {
            HistoryEntries = new ObservableCollection<HistoryEntry>()
            {
                new HistoryEntry("a", ImageSource.FromResource("MobileApp.Assets.Images.grid.png"), DateTime.UtcNow, "Testing unit 1", 3),
                new HistoryEntry("b", ImageSource.FromResource("MobileApp.Assets.Images.grid.png"), DateTime.UtcNow, "Seconding unit for tests", 5),
            };
        }
    }
}
