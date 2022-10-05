using LiteDB;
using MetroLog;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using ModuleLauncher.Re.Authenticators;
using ModuleLauncher.Re.Models.Authenticators;
using System;
using System.IO;
using System.Management;
using System.Threading.Tasks;
using Windows.Storage;
using System.Text.Json;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using JsonSerializer = System.Text.Json.JsonSerializer;
using CommunityToolkit.WinUI.Helpers;

namespace UMCLauncher.Helpers
{
    internal static partial class SettingsHelper
    {
        public const string UserUID = "UserUID";
        public const string Java8Root = "Java8Root";
        public const string Java16Root = "Java16Root";
        public const string ChooseVersion = "ChooseVersion";
        public const string MinecraftRoot = "MinecraftRoot";
        public const string SelectedAppTheme = "SelectedAppTheme";
        public const string SelectedBackdrop = "SelectedBackdrop";
        public const string ShowOtherException = "ShowOtherException";

        public static Type Get<Type>(string key) => LocalObject.Read<Type>(key);
        public static void Set<Type>(string key, Type value) => LocalObject.Save(key, value);
        public static void SetFile<Type>(string key, Type value) => LocalObject.CreateFileAsync(key, value);
        public static async Task<Type> GetFile<Type>(string key) => await LocalObject.ReadFileAsync<Type>(key);

        public static void SetDefaultSettings()
        {
            if (!LocalObject.KeyExists(UserUID))
            { LocalObject.Save(UserUID, string.Empty); }
            if (!LocalObject.KeyExists(Java8Root))
            { LocalObject.Save(Java8Root, @"C:\Program Files (x86)\Minecraft Launcher\runtime\jre-x64\bin\javaw.exe"); }
            if (!LocalObject.KeyExists(Java16Root))
            { LocalObject.Save(Java16Root, @"C:\Program Files (x86)\Minecraft Launcher\runtime\java-runtime-alpha\windows-x64\java-runtime-alpha\bin\javaw.exe"); }
            if (!LocalObject.KeyExists(ChooseVersion))
            { LocalObject.Save(ChooseVersion, string.Empty); }
            if (!LocalObject.KeyExists(MinecraftRoot))
            { LocalObject.Save(MinecraftRoot, @$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\.minecraft"); }
            if (!LocalObject.KeyExists(SelectedAppTheme))
            { LocalObject.Save(SelectedAppTheme, ElementTheme.Default); }
            if (!LocalObject.KeyExists(SelectedBackdrop))
            { LocalObject.Save(SelectedBackdrop, BackdropType.Mica); }
            if (!LocalObject.KeyExists(ShowOtherException))
            { LocalObject.Save(ShowOtherException, true); }
        }
    }

    internal enum AuthenticatorType
    {
        MicrosoftAuthenticator,
        OfflineAuthenticator,
        MojangAuthenticator,
        AuthenticateResult
    }

    internal static partial class SettingsHelper
    {
        public static double Capacity, Available;
        public static readonly ILogManager LogManager = LogManagerFactory.CreateLogManager();
        public static ulong version = ulong.Parse(AnalyticsInfo.VersionInfo.DeviceFamilyVersion);
        public static string AccountPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Accounts.db");
        private static readonly ApplicationDataStorageHelper LocalObject = ApplicationDataStorageHelper.GetCurrent(new SystemTextJsonObjectSerializer());
        public static double WindowsVersion = double.Parse($"{(ushort)((version & 0x00000000FFFF0000L) >> 16)}.{(ushort)(version & 0x000000000000FFFFL)}");

        private static AuthenticateResult authentication;
        public static AuthenticateResult Authentication
        {
            get => authentication;
            set
            {
                UIHelper.MainPage.UserNames = value.Name;
                UIHelper.MainPage.UserAvatar = "https://crafatar.com/renders/head/" + value.Uuid;
                authentication = value;
            }
        }

        static SettingsHelper()
        {
            SetDefaultSettings();
        }

        /// <summary>
        /// 检查账号信息
        /// </summary>
        /// <param name="Type">登录方式</param>
        /// <param name="vs">登录参数</param>
        /// <returns>是否登录成功</returns>
        public static async Task<bool> CheckLogin(AuthenticatorType Type = AuthenticatorType.AuthenticateResult, object[] vs = null)
        {
            switch (Type)
            {
                case AuthenticatorType.MojangAuthenticator:
                    if (vs != null && vs[0] is string && vs[1] is string)
                    {
                        MojangAuthenticator Authenticator = new(vs[0] as string, vs[1] as string);
                        Authentication = await Authenticator.Authenticate();
                        if (!string.IsNullOrEmpty(Authentication.Uuid))
                        {
                            using (LiteDatabase db = new(AccountPath))
                            {
                                ILiteCollection<AuthenticateResult> AuthenticateResults = db.GetCollection<AuthenticateResult>();
                                _ = AuthenticateResults.Upsert(Authentication.Uuid, Authentication);
                                Set(UserUID, Authentication.Uuid);
                            }
                            return true;
                        }
                    }
                    break;

                case AuthenticatorType.OfflineAuthenticator:
                    if (vs != null && vs[0] is string)
                    {
                        OfflineAuthenticator Authenticator = new(vs[0] as string);
                        Authentication = await Authenticator.Authenticate();
                        if (!string.IsNullOrEmpty(Authentication.Uuid))
                        {
                            using (LiteDatabase db = new(AccountPath))
                            {
                                ILiteCollection<AuthenticateResult> AuthenticateResults = db.GetCollection<AuthenticateResult>();
                                _ = AuthenticateResults.Upsert(Authentication.Uuid, Authentication);
                                Set(UserUID, Authentication.Uuid);
                            }
                            return true;
                        }
                    }
                    break;

                case AuthenticatorType.MicrosoftAuthenticator:
                    if (vs != null && vs[0] is string)
                    {
                        MicrosoftAuthenticator Authenticator = new(vs[0] as string);
                        Authentication = await Authenticator.Authenticate();
                        if (!string.IsNullOrEmpty(Authentication.Uuid))
                        {
                            using (LiteDatabase db = new(AccountPath))
                            {
                                ILiteCollection<AuthenticateResult> AuthenticateResults = db.GetCollection<AuthenticateResult>();
                                _ = AuthenticateResults.Upsert(Authentication.Uuid, Authentication);
                                Set(UserUID, Authentication.Uuid);
                            }
                            return true;
                        }
                    }
                    break;

                default:
                    if (!string.IsNullOrEmpty(Get<string>(UserUID)))
                    {
                        using LiteDatabase db = new(AccountPath);
                        ILiteCollection<AuthenticateResult> AuthenticateResults = db.GetCollection<AuthenticateResult>();
                        Authentication = AuthenticateResults.FindById(Get<string>(UserUID));
                        return true;
                    }
                    break;
            }
            UIHelper.MainPage.UserNames = "登录";
            return false;
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

    public class SystemTextJsonObjectSerializer : CommunityToolkit.Common.Helpers.IObjectSerializer
    {
        string CommunityToolkit.Common.Helpers.IObjectSerializer.Serialize<T>(T value) => JsonSerializer.Serialize(value);

        public T Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value);
    }
}
