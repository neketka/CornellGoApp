﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.Extensions
{
    public class StringLengthNotZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? false : value.ToString().Length > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
