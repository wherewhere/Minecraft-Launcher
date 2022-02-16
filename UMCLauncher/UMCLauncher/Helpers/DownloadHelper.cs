using ModuleLauncher.Re.Downloaders.Concrete;
using ModuleLauncher.Re.Models.Downloaders;
using System.Threading.Tasks;

namespace UMCLauncher.Helpers
{
    internal class DownloadHelper
    {
        public static MinecraftDownloader MinecraftDownload(DownloaderSource source)
        {
            MinecraftDownloader Downloader = new(SettingsHelper.Get<string>(SettingsHelper.MinecraftRoot))
            {
                Source = source
            };
            return Downloader;
        }

        public static async Task DownloadDependencies(string Version, bool ignoreExist = false, int maxParallel = 5)
        {
            UIHelper.MainPage.AppTitle.Text = "正在下载 Assets 文件...";
            await new AssetsDownloader(SettingsHelper.Get<string>(SettingsHelper.MinecraftRoot)).DownloadParallel(Version, ignoreExist, maxParallel);
            UIHelper.MainPage.AppTitle.Text = "正在下载 Libraries 文件...";
            await new LibrariesDownloader(SettingsHelper.Get<string>(SettingsHelper.MinecraftRoot)).DownloadParallel(Version, ignoreExist, maxParallel);
        }
    }
}
