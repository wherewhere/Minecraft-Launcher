using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using UMCLauncher.Core.Helpers;

namespace UMCLauncher.Helpers.ValueConverters
{
    public class TimeToReadableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is DateTime time
                ? DateHelper.ConvertDateTimeToReadable(time)
                : value is double @double
                    ? DateHelper.ConvertUnixTimeStampToReadable(@double)
                    : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
