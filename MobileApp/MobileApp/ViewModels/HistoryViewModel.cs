using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MobileApp.Services;

namespace MobileApp.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public ObservableCollection<HistoryEntry> HistoryEntries { get; }
        public Command<string> ShowMoreCommand { get; }

        private IGameService gameService;
        public HistoryViewModel(IGameService gameService, IDialogService dialogService)
        {
            this.gameService = gameService;

            HistoryEntries = new ObservableCollection<HistoryEntry>();
            ShowMoreCommand = new(async id => { await dialogService.ShowServerError(); });
            IsBusy = true;
        }

        public override Task OnEntering(object parameter)
        {
            gameService.Client.SendMetric(CommunicationModel.FrontendMetric.OpenHistory, "");
            return Task.Run(Load);
        }

        private async Task Load()
        {
            List<HistoryEntry> entries = new List<HistoryEntry>();
            await foreach (var entry in gameService.Client.GetHistoryData())
            {
                ImageSource img = new UriImageSource
                {
                    Uri = new Uri(entry.ImageUrl),
                    CachingEnabled = true
                };
                entries.Add(new(entry.ChallengeId, img, entry.UtcDateTime.ToLocalTime(), entry.Name, entry.Points));
            }
            await Device.InvokeOnMainThreadAsync(() =>
            {
                entries.ForEach(HistoryEntries.Add);
                IsBusy = false;
            });
        }
    }
}
