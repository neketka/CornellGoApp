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
    [QueryProperty("ImageUrl", "image")]
    [QueryProperty("Name", "name")]
    [QueryProperty("Points", "points")]
    public partial class VictoryPage : ContentPage
    {
        public VictoryPage()
        {
            InitializeComponent();
        }

        public string Name
        {
            set => ((VictoryViewModel)BindingContext).Name = Uri.UnescapeDataString(value);
        }

        public string ImageUrl
        {
            set => ((VictoryViewModel)BindingContext).Image = new UriImageSource
            {
                Uri = new Uri(Uri.UnescapeDataString(value)),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(5, 0, 0, 0)
            };
        }

        public string Points
        {
            set => ((VictoryViewModel)BindingContext).Points = int.Parse(value);
        }

        private void Continue_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("..");
        }
    }
}