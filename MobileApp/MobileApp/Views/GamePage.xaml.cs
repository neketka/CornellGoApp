using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.PlatformConfiguration;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        private double[] positions;
        private double lastChange = 0;
        public GamePage()
        {
            InitializeComponent();

            ((GameViewModel)BindingContext).PropertyChanged += GamePage_PropertyChanged;
            BottomSheet.PropertyChanged += BottomSheet_PropertyChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ContentPage_SizeChanged(null, null);
        }

        private void AdjustTemp(double value)
        {
            TempBar.TranslateTo((Width - 12.0) * value + 12.0, 0);
        }

        private async void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GameViewModel.Progress)) 
                AdjustTemp(((GameViewModel)BindingContext).Progress);
            else if (e.PropertyName == nameof(GameViewModel.VictoryMode))
            {
                bool state = ((GameViewModel)BindingContext).VictoryMode;
                Shell.Current.FlyoutBehavior = state ? FlyoutBehavior.Disabled : FlyoutBehavior.Flyout;
                if (state)
                {
                    OldImage.Opacity = 1;
                    HamburgerButton.FadeTo(0, 800u, Easing.CubicOut);
                    await BottomSheet.TranslateTo(0, Height, 800u, Easing.CubicOut);
                    Darkener.FadeTo(0.4);
                    VictoryView.FadeTo(1);
                }
                else
                {
                    Darkener.FadeTo(0);
                    await VictoryView.FadeTo(0);
                    await OldImage.FadeTo(0, easing: Easing.CubicIn);
                    await Task.Delay(250);
                    HamburgerButton.FadeTo(1);
                    await BottomSheet.TranslateTo(0, positions[1], 350u, Easing.CubicOut); 
                }
            }
        }

        private void ContentPage_SizeChanged(object sender, EventArgs e)
        {
            //Other: 65, X: 95
            double bias = Device.RuntimePlatform == Device.iOS ? On<iOS>().SafeAreaInsets().Bottom > 10 ? 95 : 65 : 0;

            double desc = Height - Divider.Bounds.Y - bias;
            double max = desc + Description.Height;
            double groups = desc - 250;

            positions = new[] { groups, desc, max };

            AdjustTemp(((GameViewModel)BindingContext).Progress);
            BottomSheet.TranslationY = positions[1];
        }

        private void UpdateLayout()
        {
            if (positions != null)
            {
                double middleDist = positions[1] - BottomSheet.TranslationY;
                double normalizedMiddleDist = Math.Min(Math.Max(middleDist / (positions[1] - positions[0]), 0), 1);

                Therm.TranslationY = Math.Min(Math.Max(middleDist, -Description.Height), 0);
                Description.Opacity = 1 - Math.Min(Math.Max(-middleDist / (positions[2] - positions[1]), 0), 0.7) / 0.7;
                Darkener.Opacity = 0.75 * normalizedMiddleDist;

                double bottomAlpha = BottomSheet.TranslationY < positions[1] ? 0.8 + 0.2 * normalizedMiddleDist : 0.8;
                Color bgColor = BottomSheet.BackgroundColor;
                BottomSheet.BackgroundColor = new Color(bgColor.R, bgColor.G, bgColor.B, bottomAlpha);
            }
        }

        private void SnapToNearest(double vel)
        {
            int indexRight = 1;
            while (indexRight < positions.Length && BottomSheet.TranslationY > positions[indexRight]) ++indexRight;
            int indexLeft = indexRight - 1;

            double newTrans;
            if (Math.Abs(vel) < 3) newTrans = (positions[indexRight] - BottomSheet.TranslationY <= BottomSheet.TranslationY - positions[indexLeft]) ?
                    positions[indexRight] : positions[indexLeft];
            else newTrans = vel < 0 ? positions[indexLeft] : positions[indexRight];

            BottomSheet.TranslateTo(0, newTrans, 350u, Easing.CubicOut);
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            ViewExtensions.CancelAnimations(BottomSheet);
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    lastChange = e.TotalY;
                    BottomSheet.TranslationY = Math.Min(Math.Max(BottomSheet.TranslationY + e.TotalY, positions[0]), positions[positions.Length - 1]);
                    break;
                case GestureStatus.Completed:
                    SnapToNearest(lastChange);
                    break;
            }
        }

        private void PlaceImage_SizeChanged(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        private void BottomSheet_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateLayout();
        }

        private void Hamburger_Tapped(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = true;
            //((GameViewModel)BindingContext).DoVictoryCommand.Execute(null);
        }
    }
}