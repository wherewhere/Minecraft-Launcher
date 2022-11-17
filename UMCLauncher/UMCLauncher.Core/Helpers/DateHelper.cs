using System;

namespace UMCLauncher.Core.Helpers
{
    public static class DateHelper
    {
        public enum TimeIntervalType
        {
            MonthsAgo,
            DaysAgo,
            HoursAgo,
            MinutesAgo,
            JustNow,
        }

        private static readonly DateTime UnixDateBase = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string ConvertUnixTimeStampToReadable(double time, DateTime baseTime)
        {
            TimeIntervalType type;
            object obj;

            TimeSpan ttime = new((long)time * 1000_0000);
            DateTime tdate = UnixDateBase.Add(ttime);
            TimeSpan temp = baseTime.ToUniversalTime()
                                    .Subtract(tdate);

            if (temp.TotalDays > 30)
            {
                type = TimeIntervalType.MonthsAgo;
                obj = tdate;
            }
            else
            {
                type = temp.Days > 0
                    ? TimeIntervalType.DaysAgo
                    : temp.Hours > 0
                        ? TimeIntervalType.HoursAgo
                        : temp.Minutes > 0
                            ? TimeIntervalType.MinutesAgo
                            : TimeIntervalType.JustNow;
                obj = temp;
            }

            return type switch
            {
                TimeIntervalType.MonthsAgo => ((DateTime)obj).ToLongDateString(),
                TimeIntervalType.DaysAgo => $"{((TimeSpan)obj).Days}天前",
                TimeIntervalType.HoursAgo => $"{((TimeSpan)obj).Hours}小时前",
                TimeIntervalType.MinutesAgo => $"{((TimeSpan)obj).Minutes}分钟前",
                TimeIntervalType.JustNow => "刚刚",
                _ => string.Empty,
            };
        }

        public static double ConvertDateTimeToUnixTimeStamp(DateTime time)
        {
            return Math.Round(
                time.ToUniversalTime()
                    .Subtract(UnixDateBase)
                    .TotalSeconds);
        }

        public static string ConvertDateTimeToReadable(DateTime time)
        {
            return ConvertUnixTimeStampToReadable(ConvertDateTimeToUnixTimeStamp(time));
        }

        public static string ConvertUnixTimeStampToReadable(double time)
        {
            return ConvertUnixTimeStampToReadable(time, DateTime.Now);
        }
    }
}
