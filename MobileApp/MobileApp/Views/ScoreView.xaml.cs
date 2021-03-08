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
    public partial class ScoreView : ContentView
    {
        public static readonly BindableProperty ImageProperty = BindableProperty.Create(
            propertyName: "Image",
            returnType: typeof(ImageSource),
            declaringType: typeof(AvatarView),
            defaultValue: null);

        public static readonly BindableProperty UsernameProperty = BindableProperty.Create(
            propertyName: "Username",
            returnType: typeof(string),
            declaringType: typeof(AvatarView),
            defaultValue: "Avatar name");

        public static readonly BindableProperty ScoreProperty = BindableProperty.Create(
            propertyName: "Score",
            returnType: typeof(int),
            declaringType: typeof(AvatarView),
            defaultValue: 0);

        public static readonly BindableProperty IsHighlightedProperty = BindableProperty.Create(
            propertyName: "IsHighlighted",
            returnType: typeof(bool),
            declaringType: typeof(AvatarView),
            defaultValue: false);

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public string Username
        {
            get => (string)GetValue(UsernameProperty);
            set => SetValue(UsernameProperty, value);
        }

        public int Score
        {
            get => (int)GetValue(ScoreProperty);
            set => SetValue(ScoreProperty, value);
        }

        public bool IsHighlighted
        {
            get => (bool)GetValue(IsHighlightedProperty);
            set => SetValue(IsHighlightedProperty, value);
        }

        public ScoreView()
        {
            InitializeComponent();
        }
    }
}