using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MinecraftLauncher.Core.Helpers;
using MinecraftLauncher.Helpers;
using ModuleLauncher.Re.Launcher;
using System;
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

        [Obsolete]
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            switch (element.Name)
            {
                case "Start":
                    StartLaunch();
                    break;
                default:
                    break;
            }
        }

        [Obsolete]
        private async void StartLaunch()
        {
            Launcher launcher = LaunchHelper.Launch(false);
            IsInfo.Visibility = Visibility.Visible;
            System.Diagnostics.Process Process = await launcher.Launch("1.16.5");
            while (!string.IsNullOrEmpty(await Process.StandardOutput.ReadLineAsync()))
            {
                StartInfo.Text = await Process.StandardOutput.ReadLineAsync();
            }
            StartInfo.Text = "已退出";
            await Task.Delay(1000);
            IsInfo.Visibility = Visibility.Collapsed;
            StartInfo.Text = string.Empty;
        }
    }
}
