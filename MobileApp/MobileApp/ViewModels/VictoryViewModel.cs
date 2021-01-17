using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class VictoryViewModel : BaseViewModel
    {
        private ImageSource image;
        private string name;
        private int points;

        public ImageSource Image { get => image; set => SetProperty(ref image, value); }
        public string Name { get => name; set => SetProperty(ref name, value); }
        public int Points { get => points; set => SetProperty(ref points, value); }
        public Command FindOutMore { get; } = new Command(() => { });
    }
}
