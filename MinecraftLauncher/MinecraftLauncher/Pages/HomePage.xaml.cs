﻿using Microsoft.UI.Xaml.Controls;
using MinecraftLauncher.Helpers;
using ModuleLauncher.Re.Launcher;
using ModuleLauncher.Re.Locators;
using ModuleLauncher.Re.Locators.Concretes;
using ModuleLauncher.Re.Models.Locators.Minecraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            GetDPI.Text = $"X:{UIHelper.DpiX} Y:{UIHelper.DpiY}";
            SettingsHelper.GetCapacity();
            SettingsHelper.GetAvailable();
            _ = LaunchHelper.GetMinecrafts();
            GetMemory.Text = $"{SettingsHelper.Available.GetSizeString()}/{SettingsHelper.Capacity.GetSizeString()}";
        }

        private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            StartLaunch();
        }

        private async void StartLaunch()
        {
            Launcher launcher = LaunchHelper.Launch(false);
            IsInfo.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            System.Diagnostics.Process Process = await launcher.Launch("1.16.5");
            while (!string.IsNullOrEmpty(await Process.StandardOutput.ReadLineAsync()))
            {
                StartInfo.Text = await Process.StandardOutput.ReadLineAsync();
            }
            StartInfo.Text = "已退出";
            await Task.Delay(1000);
            IsInfo.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            StartInfo.Text = string.Empty;
        }

        private async void UpdateMemory()
        {
            SettingsHelper.GetCapacity();
            GetMemory.Text = $"{SettingsHelper.Capacity}G";

            while (true)
            {
            }
        }
    }
}
