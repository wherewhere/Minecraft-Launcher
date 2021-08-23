using Microsoft.UI.Xaml;
using System;
using System.Management;
using Windows.Storage;
using Windows.System.Profile;

namespace MinecraftLauncher.Helpers
{
    internal class SettingsHelper
    {
        public static double Capacity, Available;
        public static ulong version = ulong.Parse(AnalyticsInfo.VersionInfo.DeviceFamilyVersion);
        private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public static bool GetBoolen(string key) => (bool)LocalSettings.Values[key];
        public static string GetString(string key) => LocalSettings.Values[key] as string;

        public static void Set(string key, object value) => LocalSettings.Values[key] = value;
        public static double WindowsVersion = double.Parse($"{(ushort)((version & 0x00000000FFFF0000L) >> 16)}.{(ushort)(version & 0x000000000000FFFFL)}");
        public static ElementTheme Theme => GetBoolen("IsBackgroundColorFollowSystem") ? ElementTheme.Default : (GetBoolen("IsDarkTheme") ? ElementTheme.Dark : ElementTheme.Light);

        static SettingsHelper()
        {
            if (!LocalSettings.Values.ContainsKey("Java8Root"))
            { LocalSettings.Values.Add("Java8Root", "C:/Program Files (x86)/Minecraft Launcher/runtime/jre-x64/bin/javaw.exe"); }
            if (!LocalSettings.Values.ContainsKey("Java16Root"))
            { LocalSettings.Values.Add("Java16Root", "C:/Program Files (x86)/Minecraft Launcher/runtime/java-runtime-alpha/windows-x64/java-runtime-alpha/bin/javaw.exe"); }
            if (!LocalSettings.Values.ContainsKey("IsDarkTheme"))
            { LocalSettings.Values.Add("IsDarkTheme", true); }
            if (!LocalSettings.Values.ContainsKey("MinecraftRoot"))
            { LocalSettings.Values.Add("MinecraftRoot", $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("\\","/")}/.minecraft"); }
            if (!LocalSettings.Values.ContainsKey("IsBackgroundColorFollowSystem"))
            { LocalSettings.Values.Add("IsBackgroundColorFollowSystem", true); }
        }

        public static void GetCapacity()
        {
            ManagementClass cimobject1 = new("Win32_ComputerSystem");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            Capacity = 0;
            foreach (ManagementObject mo1 in moc1)
            {
                Capacity += long.Parse(mo1.Properties["TotalPhysicalMemory"].Value.ToString());
            }
            moc1.Dispose();
            cimobject1.Dispose();
        }

        public static void GetAvailable()
        {
            ManagementClass cimobject2 = new("Win32_OperatingSystem");
            ManagementObjectCollection moc2 = cimobject2.GetInstances();
            Available = 0;
            foreach (ManagementObject mo2 in moc2)
            {
                Available += long.Parse(mo2.Properties["FreePhysicalMemory"].Value.ToString());
            }
            Available *= 1024;
            moc2.Dispose();
            cimobject2.Dispose();
        }
    }
}
