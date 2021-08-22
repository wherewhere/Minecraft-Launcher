using KMCCC.Tools;
using Microsoft.UI.Xaml.Controls;
using ModuleLauncher.Re.Launcher;
using System;
using System.Drawing;
using System.Linq;

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
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            float dpiX = graphics.DpiX;
            float dpiY = graphics.DpiY;
            GetDPI.Text = $"X:{dpiX} Y:{dpiY}";
        }

        private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var launcher = new Launcher("C:/Users/qq251/AppData/Roaming/.minecraft")
            {
                Java = "C:/Program Files/Java/jre1.8.0_301/bin/javaw.exe",
                Authentication = "wherewhere",
                LauncherName = "UWP", //optianal
                MaximumMemorySize = 1024, //optional
                MinimumMemorySize = null, //optional
                WindowHeight = null, //optional
                WindowWidth = null, //optional
                Fullscreen = null //optional
            };
            StartInfo.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            _ = await launcher.Launch("1.16.5");
            StartInfo.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
        }
    }
}
