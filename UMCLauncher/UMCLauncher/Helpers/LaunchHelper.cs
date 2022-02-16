using ModuleLauncher.Re.Launcher;
using ModuleLauncher.Re.Locators;
using ModuleLauncher.Re.Locators.Concretes;
using ModuleLauncher.Re.Models.Locators.Minecraft;
using System.Collections.Generic;
using System.Threading.Tasks;
using UMCLauncher.Core.Helpers;
using UMCLauncher.Models;

namespace UMCLauncher.Helpers
{
    internal class LaunchHelper
    {
        /// <summary>
        /// Java 环境列表
        /// </summary>
        public static List<JavaVersion> Javas;

        /// <summary>
        /// Minecraft 版本列表
        /// </summary>
        public static IEnumerable<Minecraft> Minecrafts;

        /// <summary>
        /// 生成启动参数
        /// </summary>
        /// <param name="IsOld">是否为1.17以下</param>
        /// <param name="Fullscreen">是否全屏</param>
        /// <returns>启动参数</returns>
        public static Launcher Launch(bool IsOld = false, bool Fullscreen = false)
        {
            SettingsHelper.GetCapacity();
            Launcher launcher = new(SettingsHelper.Get<string>(SettingsHelper.MinecraftRoot))
            {
                Java = IsOld ? SettingsHelper.Get<string>(SettingsHelper.Java8Root) : SettingsHelper.Get<string>(SettingsHelper.Java16Root),
                Authentication = string.IsNullOrEmpty(SettingsHelper.Authentication.Name) ? "Steve" : SettingsHelper.Authentication,
                LauncherName = "UMCL", //optianal
                MaximumMemorySize = (int)(SettingsHelper.Available * 0.9 / 1048576), //optional
                MinimumMemorySize = null, //optional
                WindowHeight = (int?)(480 * UIHelper.DpiY), //optional
                WindowWidth = (int?)(854 * UIHelper.DpiX), //optional
                Fullscreen = Fullscreen //optional
            };
            return launcher;
        }

        /// <summary>
        /// 刷新已有 Minecraft 版本
        /// </summary>
        /// <returns></returns>
        public static async Task GetMinecrafts()
        {
            MinecraftLocator Locator = new(new LocalityLocator(SettingsHelper.Get<string>(SettingsHelper.MinecraftRoot)));
            Minecrafts = await Locator.GetLocalMinecrafts();
        }

        /// <summary>
        /// 刷新已有 Java 环境
        /// </summary>
        public static void GetJavas()
        {
            Javas = Utils.GetJavaInstallationPath();
        }
    }
}
