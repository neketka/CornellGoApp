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

        private void AdjustTemp(double value)
        {
            TempBar.TranslateTo((Width - 16.0) * value + 16.0, 0);
        }

        private void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GameViewModel.Progress)) AdjustTemp(((GameViewModel)BindingContext).Progress);
        }

        private void ContentPage_SizeChanged(object sender, EventArgs e)
        {
            double max = Height - 100;
            positions = new[] { max - 300, max - 75, max };

            AdjustTemp(((GameViewModel)BindingContext).Progress);
            BottomSheet.TranslationY = positions[1];
        }

        private void UpdateLayout()
        {
            Therm.TranslationY = Math.Min(Math.Max(Height - BottomSheet.TranslationY - 100, 0), 75) - 75;

            double imageHeight = BottomSheet.TranslationY;
            double allocAspect = Width / imageHeight;
            double aspect = 1.0 / PlaceImage.Height;

            PlaceImage.BatchBegin();
            PlaceImage.TranslationY = imageHeight / 2.0;
            PlaceImage.Scale = aspect > allocAspect ? aspect * imageHeight : Width;
            PlaceImage.BatchCommit();
        }

        private void SnapToNearest(double vel)
        {
            int indexRight = 1;
            while (indexRight < positions.Length && BottomSheet.TranslationY > positions[indexRight]) ++indexRight;
            int indexLeft = indexRight - 1;

            double newTrans = 0;
            if (Math.Abs(vel) == 0) newTrans = (positions[indexRight] - BottomSheet.TranslationY <= BottomSheet.TranslationY - positions[indexLeft]) ?
                    positions[indexRight] : positions[indexLeft];
            else newTrans = vel < 0 ? positions[indexLeft] : positions[indexRight];

            BottomSheet.TranslateTo(0, newTrans, 300u, Easing.CubicOut);
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
    }
}