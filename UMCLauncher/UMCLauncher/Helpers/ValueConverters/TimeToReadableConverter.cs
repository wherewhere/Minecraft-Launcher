using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace UMCLauncher.Helpers.ValueConverters
{
    public class TimeToReadableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (string)parameter switch
            {
                "double" => DataHelper.ConvertUnixTimeStampToReadable((double)value),
                "DataTime" => DataHelper.ConvertDateTimeToReadable((DateTime)value),
                _ => value is DateTime time ? DataHelper.ConvertDateTimeToReadable(time) : value is double @double ? DataHelper.ConvertUnixTimeStampToReadable(@double) : value,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => (Visibility)value == Visibility.Visible;
    }
}
