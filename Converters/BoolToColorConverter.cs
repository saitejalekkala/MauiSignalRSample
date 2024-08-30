using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace MauiSignalRSample.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSent)
            {
                return isSent ? Color.FromArgb("#10331a") : Color.FromArgb("#3d403d");
            }
            return Color.FromArgb("#3d403d");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
