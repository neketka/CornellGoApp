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
    public partial class AvatarView : ContentView
    {
        public static readonly BindableProperty ImageProperty = BindableProperty.Create(
            propertyName: "Image", 
            returnType: typeof(ImageSource),
            declaringType: typeof(AvatarView),
            defaultValue: null);

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public AvatarView()
        {
            InitializeComponent();
        }
    }
}