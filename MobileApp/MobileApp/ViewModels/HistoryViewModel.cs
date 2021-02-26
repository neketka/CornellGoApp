using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public ObservableCollection<HistoryEntry> HistoryEntries { get; }
        public Command<string> ShowMoreCommand { get; }
        public HistoryViewModel()
        {
            HistoryEntries = new ObservableCollection<HistoryEntry>();
            ShowMoreCommand = new(async (id) => { await NavigationService.ShowServerError(); });
            Load().Wait();
        }

        private async Task Load()
        {
            await foreach (var entry in GameService.Client.GetHistoryData())
            {
                ImageSource img = new UriImageSource
                {
                    Uri = new Uri(entry.ImageUrl),
                    CachingEnabled = true
                };
                HistoryEntries.Add(new(entry.ChallengeId, img, entry.UtcDateTime.ToLocalTime(), entry.Name, entry.Points));
            }
        }
    }
}
