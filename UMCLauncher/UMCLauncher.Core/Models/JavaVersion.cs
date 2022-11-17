using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace UMCLauncher.Models
{
    public class JavaVersion : IComparable
    {
        /// <summary>
        /// 是否为 x86
        /// </summary>
        public bool ISWOW6432 { get; set; }

        /// <summary>
        /// 环境根目录
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// java.exe 目录
        /// </summary>
        public string JavaPath => Path.Combine(RootPath, @"bin\javaw.exe");

        /// <summary>
        /// Java 环境版本信息
        /// </summary>
        public FileVersionInfo Version => FileVersionInfo.GetVersionInfo(JavaPath);

        public JavaVersion(string path, bool x86 = false)
        {
            RootPath = path;
            ISWOW6432 = x86;
        }

        public int CompareTo(object obj)
        {
            if (obj is JavaVersion another)
            {
                int result = another.ISWOW6432.CompareTo(ISWOW6432);
                if (result == 0)
                {
                    result = GetAsVersionInfo(Version.ProductVersion).CompareTo(GetAsVersionInfo(another.Version.ProductVersion));
                    if (result == 0)
                    {
                        JavaPath.CompareTo(another.JavaPath);
                    }
                }
                return result;
            }
            throw new ArgumentException(null, nameof(obj));
        }

        public static bool operator <(JavaVersion left, JavaVersion right) => left.CompareTo(right) < 0;

        public static bool operator <=(JavaVersion left, JavaVersion right) => left.CompareTo(right) <= 0;

        public static bool operator >(JavaVersion left, JavaVersion right) => left.CompareTo(right) > 0;

        public static bool operator >=(JavaVersion left, JavaVersion right) => left.CompareTo(right) >= 0;

        private static Version GetAsVersionInfo(string version)
        {
            List<int> nums = GetVersionNumbers(version).Split('.').Select(int.Parse).ToList();

            return nums.Count <= 1
                ? new Version(nums[0], 0, 0, 0)
                : nums.Count <= 2
                    ? new Version(nums[0], nums[1], 0, 0)
                    : nums.Count <= 3
                        ? new Version(nums[0], nums[1], nums[2], 0)
                        : new Version(nums[0], nums[1], nums[2], nums[3]);
        }

        private static string GetVersionNumbers(string version)
        {
            string allowedChars = "01234567890.";
            return new string(version.Where(allowedChars.Contains).ToArray());
        }
    }
}
