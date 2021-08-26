using MinecraftLauncher.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Helpers
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
            switch (type)
            {
                case Utils.TimeIntervalType.MonthsAgo:
                    return ((DateTime)obj).ToLongDateString();

                case Utils.TimeIntervalType.DaysAgo:
                    return $"{((TimeSpan)obj).Days}天前";

                case Utils.TimeIntervalType.HoursAgo:
                    return $"{((TimeSpan)obj).Hours}小时前";

                case Utils.TimeIntervalType.MinutesAgo:
                    return $"{((TimeSpan)obj).Minutes}分钟前";

                case Utils.TimeIntervalType.JustNow:
                    return "刚刚";

                default:
                    return string.Empty;
            }
        }
    }
}
