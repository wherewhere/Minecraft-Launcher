using System;
using System.Text;

namespace MinecraftLauncher.Core.Helpers
{
    public static partial class Utils
    {
        public static string GetMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] r1 = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                string r2 = BitConverter.ToString(r1).ToLowerInvariant();
                return r2.Replace("-", "");
            }
        }

        public static string GetBase64(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        public enum TimeIntervalType
        {
            MonthsAgo,
            DaysAgo,
            HoursAgo,
            MinutesAgo,
            JustNow,
        }

        private static readonly DateTime UnixDateBase = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static (TimeIntervalType type, object time) ConvertUnixTimeStampToReadable(double time, DateTime baseTime)
        {
            TimeSpan ttime = new((long)time * 1000_0000);
            DateTime tdate = UnixDateBase.Add(ttime);
            TimeSpan temp = baseTime.ToUniversalTime()
                                    .Subtract(tdate);

            if (temp.TotalDays > 30)
            {
                return (TimeIntervalType.MonthsAgo, tdate);
            }
            else
            {
                TimeIntervalType type = temp.Days > 0
                    ? TimeIntervalType.DaysAgo
                    : temp.Hours > 0 ? TimeIntervalType.HoursAgo : temp.Minutes > 0 ? TimeIntervalType.MinutesAgo : TimeIntervalType.JustNow;
                return (type, temp);
            }
        }

        public static double ConvertDateTimeToUnixTimeStamp(DateTime time)
        {
            return Math.Round(
                time.ToUniversalTime()
                    .Subtract(UnixDateBase)
                    .TotalSeconds);
        }
    }
}
