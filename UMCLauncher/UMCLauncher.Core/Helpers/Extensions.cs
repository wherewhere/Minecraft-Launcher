using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using UMCLauncher.Models;

namespace UMCLauncher.Core.Helpers
{
    public static class Extensions
    {
        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (T cur in e)
            {
                yield return cur;
            }
            yield return value;
        }

        [SupportedOSPlatform("windows")]
        public static List<JavaVersion> GetJavaInstallationPath()
        {
            List<JavaVersion> list = new();

            string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");

            if (!string.IsNullOrEmpty(environmentPath) && File.Exists(Path.Combine(environmentPath, @"bin\javaw.exe")))
            {
                list.Add(new JavaVersion(environmentPath));
            }

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\JavaSoft\JDK\"))
            {
                if (rk != null)
                {
                    string currentVersion = rk.GetValue("CurrentVersion").ToString();

                    using RegistryKey key = rk.OpenSubKey(currentVersion);
                    string path = key.GetValue("JavaHome").ToString();

                    if (File.Exists(Path.Combine(path, @"bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(path));
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

                    if (File.Exists(Path.Combine(path, @"bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(path));
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

                        if (File.Exists(Path.Combine(path, @"bin\javaw.exe")))
                        {
                            list.Add(new JavaVersion(path));
                        }
                    }
                }
            }

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Mojang\InstalledProducts\Minecraft Launcher\"))
            {
                if (rk != null)
                {
                    string path = rk.GetValue("InstallLocation").ToString();

                    if (File.Exists(Path.Combine(path, @"runtime\java-runtime-alpha\windows-x64\java-runtime-alpha\bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(Path.Combine(path, @"runtime\java-runtime-alpha\windows-x64\java-runtime-alpha")));
                    }

                    if (File.Exists(Path.Combine(path, @"runtime\java-runtime-beta\windows-x64\java-runtime-beta\bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(Path.Combine(path, @"runtime\java-runtime-beta\windows-x64\java-runtime-beta")));
                    }

                    if (File.Exists(Path.Combine(path, @"runtime\jre-legacy\windows-x64\jre-legacy\bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(Path.Combine(path, @"runtime\jre-legacy\windows-x64\jre-legacy")));
                    }

                    if (File.Exists(Path.Combine(path, @"runtime\jre-x64\bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(Path.Combine(path, @"runtime\jre-x64")));
                    }
                }
            }

            if (OSVersionHelper.IsWindows8OrGreater)
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Packages\Microsoft.4297127D64EC6_8wekyb3d8bbwe\LocalCache\Local");

                if (File.Exists(Path.Combine(path, @"runtime\java-runtime-alpha\windows-x64\java-runtime-alpha\bin\javaw.exe")))
                {
                    list.Add(new JavaVersion(Path.Combine(path, @"runtime\java-runtime-alpha\windows-x64\java-runtime-alpha")));
                }

                if (File.Exists(Path.Combine(path, @"runtime\java-runtime-beta\windows-x64\java-runtime-beta\bin\javaw.exe")))
                {
                    list.Add(new JavaVersion(Path.Combine(path, @"runtime\java-runtime-beta\windows-x64\java-runtime-beta")));
                }

                if (File.Exists(Path.Combine(path, @"runtime\jre-legacy\windows-x64\jre-legacy\bin\javaw.exe")))
                {
                    list.Add(new JavaVersion(Path.Combine(path, @"runtime\jre-legacy\windows-x64\jre-legacy")));
                }

                if (File.Exists(Path.Combine(path, @"runtime\jre-x64\bin\javaw.exe")))
                {
                    list.Add(new JavaVersion(Path.Combine(path, @"runtime\jre-x64")));
                }
            }

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\JavaSoft\JDK\"))
            {
                if (rk != null)
                {
                    string currentVersion = rk.GetValue("CurrentVersion").ToString();

                    using RegistryKey key = rk.OpenSubKey(currentVersion);
                    string path = key.GetValue("JavaHome").ToString();

                    if (File.Exists(Path.Combine(path, @"bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(path, true));
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

                    if (File.Exists(Path.Combine(path, @"bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(path, true));
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

                        if (File.Exists(Path.Combine(path, @"bin\javaw.exe")))
                        {
                            list.Add(new JavaVersion(path, true));
                        }
                    }
                }
            }

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Mojang\InstalledProducts\Minecraft Launcher\"))
            {
                if (rk != null)
                {
                    string path = rk.GetValue("InstallLocation").ToString();

                    if (File.Exists(Path.Combine(path, @"runtime\java-runtime-alpha\windows-x86\java-runtime-alpha\bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(Path.Combine(path, @"runtime\java-runtime-alpha\windows-x86\java-runtime-alpha"), true));
                    }

                    if (File.Exists(Path.Combine(path, @"runtime\java-runtime-beta\windows-x86\java-runtime-beta\bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(Path.Combine(path, @"runtime\java-runtime-beta\windows-x86\java-runtime-beta"), true));
                    }

                    if (File.Exists(Path.Combine(path, @"runtime\jre-legacy\windows-x86\jre-legacy\bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(Path.Combine(path, @"runtime\jre-legacy\windows-x86\jre-legacy"), true));
                    }

                    if (File.Exists(Path.Combine(path, @"runtime\jre-x86\bin\javaw.exe")))
                    {
                        list.Add(new JavaVersion(Path.Combine(path, @"runtime\jre-x86"), true));
                    }
                }
            }

            list = list.GroupBy(x => x.JavaPath).Select(y => y.First()).ToList();
            list.Sort();
            return list;
        }
    }
}
