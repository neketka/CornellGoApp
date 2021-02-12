using MobileApp.Extensions;
using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                EntryCollection.ItemsSource = ((HistoryViewModel)BindingContext).HistoryEntries;
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var entries = ((HistoryViewModel)BindingContext).HistoryEntries;
            var query = ((SearchBar)sender).Text;

            EntryCollection.ItemsSource = string.IsNullOrWhiteSpace(query) ? entries :
                entries.Select(e => (new F23.StringSimilarity.JaroWinkler().Distance(query, e.Name), e))
                       .Where(p => p.Item1 != 0).OrderBy(p => p.Item1).Select(p => p.e);
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            this.CleanupPage();
        }
    }
}