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
using MobileApp.Extensions;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        private double[] positions;
        private double lastChange = 0;
        private bool groupMenuOpen = false;
        public GamePage()
        {
            InitializeComponent();

            BindingContextChanged += GamePage_BindingContextChanged;
            BottomSheet.PropertyChanged += BottomSheet_PropertyChanged;
            Shell.Current.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Shell.Current.FlyoutIsPresented))
                    SnapToNearest(0);
            };
        }

        private void GamePage_BindingContextChanged(object sender, EventArgs e)
        {
            ((BaseViewModel)BindingContext).PropertyChanged += GamePage_PropertyChanged;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
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
                    HamburgerButton.FadeTo(0.85);
                    await BottomSheet.TranslateTo(0, positions[1], 350u, Easing.CubicOut); 
                }
            }
            else if (e.PropertyName == nameof(GameViewModel.IsDone))
            {
                if (((GameViewModel)BindingContext).IsDone)
                    TempFinisher.FadeTo(1);
                else
                    TempFinisher.FadeTo(0);
            }
        }

        private void ContentPage_SizeChanged(object sender, EventArgs e)
        {
            double desc = Height - Divider.Bounds.Y;
            double max = desc + Description.Height;
            double groups = desc - 250;

            positions = new[] { groups, desc, max };

            if (BindingContext != null)
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

                double bottomAlpha = BottomSheet.TranslationY < positions[1] ? 0.75 + 0.25 * normalizedMiddleDist : 0.75;
                Color bgColor = BottomSheet.BackgroundColor;
                BottomSheet.BackgroundColor = new Color(bgColor.R, bgColor.G, bgColor.B, bottomAlpha);

                if (normalizedMiddleDist >= 0.999)
                {
                    if (!groupMenuOpen)
                        ((GameViewModel)BindingContext).GroupMenuVisibilityCommand.Execute(true);
                    groupMenuOpen = true;
                }
                else if (normalizedMiddleDist <= 0.001)
                {
                    if (groupMenuOpen)
                        ((GameViewModel)BindingContext).GroupMenuVisibilityCommand.Execute(false);
                    groupMenuOpen = false;
                }

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
                case GestureStatus.Canceled:
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
        }
    }
}