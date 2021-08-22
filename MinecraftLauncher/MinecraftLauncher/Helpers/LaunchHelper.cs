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

        public static Launcher Launch(bool IsOld = false,bool Fullscreen = false)
        {
            SettingsHelper.GetCapacity();
            Launcher launcher = new(SettingsHelper.GetString("MinecraftRoot"))
            {
                Java = IsOld ? SettingsHelper.GetString("Java8Root") : SettingsHelper.GetString("Java16Root"),
                Authentication = "wherewhere",
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
    }
}
