using Microsoft.Win32;
using MinecraftLauncher.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MinecraftLauncher.Core.Helpers
{
    public static partial class Utils
    {
        /// <summary>
        /// 获取 MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns>不含 '-' 的 MD5 结果</returns>
        public static string GetMD5(string input)
        {
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] r1 = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            string r2 = BitConverter.ToString(r1).ToLowerInvariant();
            return r2.Replace("-", "");
        }

        /// <summary>
        /// 获取 Base64
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Base64 结果</returns>
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

        /// <summary>
        /// 将 Unix 时间戳转换为正常的内容
        /// </summary>
        /// <param name="time">Unix 时间戳</param>
        /// <param name="baseTime">相对时间</param>
        /// <returns></returns>
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

        /// <summary>
        /// 将 DataTime 转换为 Unix 时间戳
        /// </summary>
        /// <param name="time">DataTime 时间</param>
        /// <returns>Unix 时间戳</returns>
        public static double ConvertDateTimeToUnixTimeStamp(DateTime time)
        {
            return Math.Round(
                time.ToUniversalTime()
                    .Subtract(UnixDateBase)
                    .TotalSeconds);
        }

        /// <summary>
        /// 向 IEnumerable 添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }

        /// <summary>
        /// 取Java路径(新)
        /// </summary>
        /// <returns>列表(Java 环境信息)</returns>
        public static List<JavaVersion> GetJavaInstallationPath()
        {
            List<JavaVersion> vs = new();

            try
            {
                string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");

                if (!string.IsNullOrEmpty(environmentPath) && File.Exists(environmentPath + @"\bin\javaw.exe"))
                {
                    vs.Add(new JavaVersion() { RootPath = $@"{environmentPath}\"});
                }
                
                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\JavaSoft\JDK\"))
                {
                    if (rk != null)
                    {
                        string currentVersion = rk.GetValue("CurrentVersion").ToString();

                        using RegistryKey key = rk.OpenSubKey(currentVersion);
                        string path = key.GetValue("JavaHome").ToString();

                        if (File.Exists(path + @"\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { RootPath = $@"{path}\"});
                        }
                    }
                }

                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\JavaSoft\Java Runtime Environment\"))
                {
                    if (rk != null)
                    {
                        string currentVersion = rk.GetValue("CurrentVersion").ToString();

                        using RegistryKey key = rk.OpenSubKey(currentVersion);
                        string path = key.GetValue("JavaHome").ToString();

                        if (File.Exists(path + @"\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { RootPath = $@"{path}\"});
                        }
                    }
                }

                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\JDK\"))
                {
                    if (rk != null)
                    {
                        foreach (string currentVersion in rk.GetSubKeyNames())
                        {
                            using RegistryKey key = rk.OpenSubKey(currentVersion + @"\hotspot\MSI");
                            string path = key.GetValue("Path").ToString();

                            if (File.Exists(path + @"bin\javaw.exe"))
                            {
                                vs.Add(new JavaVersion() { RootPath = path});
                            }
                        }
                    }
                }

                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Mojang\InstalledProducts\Minecraft Launcher\"))
                {
                    if (rk != null)
                    {
                        string path = rk.GetValue("InstallLocation").ToString();

                        if (File.Exists(path + @"runtime\java-runtime-alpha\windows-x64\java-runtime-alpha\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { RootPath = @$"{path}runtime\java-runtime-alpha\windows-x64\java-runtime-alpha\"});
                        }

                        if (File.Exists(path + @"runtime\jre-legacy\windows-x64\jre-legacy\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { RootPath = @$"{path}runtime\jre-legacy\windows-x64\jre-legacy\"});
                        }

                        if (File.Exists(path + @"runtime\jre-x64\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { RootPath = @$"{path}runtime\jre-x64\"});
                        }
                    }
                }

                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\JavaSoft\JDK\"))
                {
                    if (rk != null)
                    {
                        string currentVersion = rk.GetValue("CurrentVersion").ToString();

                        using RegistryKey key = rk.OpenSubKey(currentVersion);
                        string path = key.GetValue("JavaHome").ToString();

                        if (File.Exists(path + @"\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { RootPath = $@"{path}\", ISWOW6432 = true });
                        }
                    }
                }

                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\JavaSoft\Java Runtime Environment\"))
                {
                    if (rk != null)
                    {
                        string currentVersion = rk.GetValue("CurrentVersion").ToString();

                        using RegistryKey key = rk.OpenSubKey(currentVersion);
                        string path = key.GetValue("JavaHome").ToString();

                        if (File.Exists(path + @"\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { RootPath = $@"{path}\", ISWOW6432 = true });
                        }
                    }
                }

                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\JDK\"))
                {
                    if (rk != null)
                    {
                        foreach (string currentVersion in rk.GetSubKeyNames())
                        {
                            using RegistryKey key = rk.OpenSubKey(currentVersion + @"\hotspot\MSI");
                            string path = key.GetValue("Path").ToString();

                            if (File.Exists(path + @"bin\javaw.exe"))
                            {
                                vs.Add(new JavaVersion() { RootPath = path, ISWOW6432 = true });
                            }
                        }
                    }
                }

                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Mojang\InstalledProducts\Minecraft Launcher\"))
                {
                    if (rk != null)
                    {
                        string path = rk.GetValue("InstallLocation").ToString();

                        if (File.Exists(path + @"runtime\java-runtime-alpha\windows-x64\java-runtime-alpha\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { RootPath = @$"{path}runtime\java-runtime-alpha\windows-x64\java-runtime-alpha\" });
                        }

                        if (File.Exists(path + @"runtime\jre-legacy\windows-x64\jre-legacy\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { RootPath = @$"{path}runtime\jre-legacy\windows-x64\jre-legacy\" });
                        }

                        if (File.Exists(path + @"runtime\jre-x64\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { RootPath = @$"{path}runtime\jre-x64\" });
                        }
                    }
                }

                return vs;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
