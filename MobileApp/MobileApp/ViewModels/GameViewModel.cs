using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private double progress = 0.0;
        public double Progress
        {
            get => progress;
            set => SetProperty(ref progress, value);
        }
    }
}
