using MetroLog;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using ModuleLauncher.Re.Authenticators;
using ModuleLauncher.Re.Models.Authenticators;
using ModuleLauncher.Re.Utils.Extensions;
using System;
using System.Management;
using Windows.Storage;
using Windows.System.Profile;
using Windows.UI.ViewManagement;

namespace MinecraftLauncher.Helpers
{
    internal static partial class SettingsHelper
    {
        public const string Java8Root = "Java8Root";
        public const string Java16Root = "Java16Root";
        public const string IsDarkTheme = "IsDarkTheme";
        public const string AccessToken = "AccessToken";
        public const string ClientToken = "ClientToken";
        public const string MinecraftRoot = "MinecraftRoot";
        public const string ShowOtherException = "ShowOtherException";
        public const string IsBackgroundColorFollowSystem = "IsBackgroundColorFollowSystem";

        public static void SetDefaultSettings()
        {
            if (!LocalSettings.Values.ContainsKey(Java8Root))
            { LocalSettings.Values.Add(Java8Root, @"C:\Program Files (x86)\Minecraft Launcher\runtime\jre-x64\bin\javaw.exe"); }
            if (!LocalSettings.Values.ContainsKey(Java16Root))
            { LocalSettings.Values.Add(Java16Root, @"C:\Program Files (x86)\Minecraft Launcher\runtime\java-runtime-alpha\windows-x64\java-runtime-alpha\bin\javaw.exe"); }
            if (!LocalSettings.Values.ContainsKey(IsDarkTheme))
            { LocalSettings.Values.Add(IsDarkTheme, true); }
            if (!LocalSettings.Values.ContainsKey(AccessToken))
            { LocalSettings.Values.Add(AccessToken, null); }
            if (!LocalSettings.Values.ContainsKey(ClientToken))
            { LocalSettings.Values.Add(ClientToken, null); }
            if (!LocalSettings.Values.ContainsKey(MinecraftRoot))
            { LocalSettings.Values.Add(MinecraftRoot, @$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\.minecraft"); }
            if (!LocalSettings.Values.ContainsKey(ShowOtherException))
            { LocalSettings.Values.Add(ShowOtherException, true); }
            if (!LocalSettings.Values.ContainsKey(IsBackgroundColorFollowSystem))
            { LocalSettings.Values.Add(IsBackgroundColorFollowSystem, true); }
        }
    }

    internal enum UISettingChangedType
    {
        LightMode,
        DarkMode,
        NoPicChanged
    }

    internal static partial class SettingsHelper
    {
        public static double Capacity, Available;
        public static AuthenticateResult Authentication;
        public static readonly UISettings UISettings = new();
        public static readonly ILogManager LogManager = LogManagerFactory.CreateLogManager();
        public static ulong version = ulong.Parse(AnalyticsInfo.VersionInfo.DeviceFamilyVersion);
        private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        public static Core.WeakEvent<UISettingChangedType> UISettingChanged { get; } = new Core.WeakEvent<UISettingChangedType>();

        public static Type Get<Type>(string key) => (Type)LocalSettings.Values[key];

        public static void Set(string key, object value) => LocalSettings.Values[key] = value;
        public static double WindowsVersion = double.Parse($"{(ushort)((version & 0x00000000FFFF0000L) >> 16)}.{(ushort)(version & 0x000000000000FFFFL)}");
        public static ElementTheme Theme => Get<bool>(IsBackgroundColorFollowSystem) ? ElementTheme.Default : (Get<bool>(IsDarkTheme) ? ElementTheme.Dark : ElementTheme.Light);

        static SettingsHelper()
        {
            SetDefaultSettings();
            SetBackgroundTheme(UISettings, null);
            UISettings.ColorValuesChanged += SetBackgroundTheme;
            UIHelper.CheckTheme();
        }

        private static void SetBackgroundTheme(UISettings o, object _)
        {
            if (Get<bool>(IsBackgroundColorFollowSystem))
            {
                bool value = o.GetColorValue(UIColorType.Background) == Colors.Black;
                Set(IsDarkTheme, value);
                UISettingChanged.Invoke(value ? UISettingChangedType.DarkMode : UISettingChangedType.LightMode);
            }
        }

        [Obsolete]
        public static async void CheckLogin()
        {
            if (!string.IsNullOrEmpty(Get<string>(AccessToken)) && !string.IsNullOrEmpty(Get<string>(ClientToken)))
            {
                MojangAuthenticator Mojang = new();
                AuthenticateResult Authentication = await Mojang.Refresh(Get<string>(AccessToken), Get<string>(ClientToken));
                if (await Authentication.Validate())
                {
                    if (UIHelper.MainPage != null)
                    {
                        UIHelper.MainPage.UserNames = Authentication.Name;
                    }
                }
            }
            else
            {
                if (UIHelper.MainPage != null)
                {
                    UIHelper.MainPage.UserNames = "登录";
                }
            }

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
