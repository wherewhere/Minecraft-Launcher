using ModuleLauncher.Re.Authenticators;
using ModuleLauncher.Re.Launcher;
using ModuleLauncher.Re.Locators;
using ModuleLauncher.Re.Locators.Concretes;
using ModuleLauncher.Re.Models.Authenticators;
using ModuleLauncher.Re.Models.Locators.Minecraft;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinecraftLauncher.Helpers
{
    internal class LaunchHelper
    {
        public static IEnumerable<Minecraft> Minecrafts;

        [System.Obsolete]
        public static async Task<Launcher> Launch(bool IsOld = false,bool Fullscreen = false)
        {
            SettingsHelper.GetCapacity();
            Launcher launcher = new(SettingsHelper.GetString("MinecraftRoot"))
            {
                Java = IsOld ? SettingsHelper.GetString("Java8Root") : SettingsHelper.GetString("Java16Root"),
                Authentication = await Login(),
                LauncherName = "UWP", //optianal
                MaximumMemorySize = (int)(SettingsHelper.Available * 0.9 / 1048576), //optional
                MinimumMemorySize = null, //optional
                WindowHeight = (int?)(480 * UIHelper.DpiY), //optional
                WindowWidth = (int?)(854 * UIHelper.DpiX), //optional
                Fullscreen = Fullscreen //optional
            };
            return launcher;
        }

        public static async Task GetMinecrafts()
        {
            MinecraftLocator Locator = new MinecraftLocator(new LocalityLocator(SettingsHelper.GetString("MinecraftRoot")));
            Minecrafts = await Locator.GetLocalMinecrafts();
        }

        [System.Obsolete]
        public static async Task<AuthenticateResult> Login()
        {
            MojangAuthenticator Mojang = new(SettingsHelper.GetString("Username"), SettingsHelper.GetString("Password"));
            AuthenticateResult Result = await Mojang.Authenticate();
            return Result;
        }
    }
}
