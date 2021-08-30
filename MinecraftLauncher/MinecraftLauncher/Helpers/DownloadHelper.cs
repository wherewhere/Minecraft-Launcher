using ModuleLauncher.Re.Downloaders;
using ModuleLauncher.Re.Downloaders.Concrete;
using ModuleLauncher.Re.Locators.Dependencies;
using ModuleLauncher.Re.Models.Downloaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Helpers
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
    }
}
