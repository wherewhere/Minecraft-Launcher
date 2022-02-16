using System;
using UMCLauncher.Core.Helpers;

namespace UMCLauncher.Helpers
{
    internal static class DataHelper
    {
        public static string ConvertDateTimeToReadable(DateTime time)
        {
            return ConvertUnixTimeStampToReadable(Utils.ConvertDateTimeToUnixTimeStamp(time));
        }

        public static string ConvertUnixTimeStampToReadable(double time)
        {
            return ConvertUnixTimeStampToReadable(time, DateTime.Now);
        }

        public static string ConvertUnixTimeStampToReadable(double time, DateTime baseTime)
        {
            (Utils.TimeIntervalType type, object obj) = Utils.ConvertUnixTimeStampToReadable(time, baseTime);
            return type switch
            {
                Utils.TimeIntervalType.MonthsAgo => ((DateTime)obj).ToLongDateString(),
                Utils.TimeIntervalType.DaysAgo => $"{((TimeSpan)obj).Days}天前",
                Utils.TimeIntervalType.HoursAgo => $"{((TimeSpan)obj).Hours}小时前",
                Utils.TimeIntervalType.MinutesAgo => $"{((TimeSpan)obj).Minutes}分钟前",
                Utils.TimeIntervalType.JustNow => "刚刚",
                _ => string.Empty,
            };
        }
    }
}
