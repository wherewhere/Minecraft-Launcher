using ModuleLauncher.Re.Launcher;
using ModuleLauncher.Re.Locators;
using ModuleLauncher.Re.Locators.Concretes;
using ModuleLauncher.Re.Models.Locators.Minecraft;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinecraftLauncher.Helpers
{
    internal class LaunchHelper
    {
        public static IEnumerable<Minecraft> Minecrafts;

        [System.Obsolete]
        public static Launcher Launch(bool IsOld = false, bool Fullscreen = false)
        {
            SettingsHelper.CheckLogin();
            SettingsHelper.GetCapacity();
            Launcher launcher = new(SettingsHelper.Get<string>(SettingsHelper.MinecraftRoot))
            {
                Java = IsOld ? SettingsHelper.Get<string>(SettingsHelper.Java8Root) : SettingsHelper.Get<string>(SettingsHelper.Java16Root),
                Authentication = SettingsHelper.Authentication,
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
            MinecraftLocator Locator = new(new LocalityLocator(SettingsHelper.Get<string>(SettingsHelper.MinecraftRoot)));
            Minecrafts = await Locator.GetLocalMinecrafts();
        }
    }
}
