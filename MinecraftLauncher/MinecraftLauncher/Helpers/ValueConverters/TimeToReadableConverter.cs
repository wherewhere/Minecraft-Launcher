using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace MinecraftLauncher.Helpers.ValueConverters
{
    public class TimeToReadableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((string)parameter)
            {
                case "double": return DataHelper.ConvertUnixTimeStampToReadable((double)value);
                case "DataTime": return DataHelper.ConvertDateTimeToReadable((DateTime)value);
                default: return value is DateTime time ? DataHelper.ConvertDateTimeToReadable(time) : value is double @double ? DataHelper.ConvertUnixTimeStampToReadable(@double) : value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => (Visibility)value == Visibility.Visible;
    }
}
