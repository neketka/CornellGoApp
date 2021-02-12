using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.Extensions
{
    public static class PageExtensions
    {
        public static void CleanupPage(this ContentPage page)
        {
            (page.BindingContext as BaseViewModel)?.CleanupEvents();
        }
    }
}
