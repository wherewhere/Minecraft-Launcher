using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MinecraftLauncher.Helpers;
using System;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher.Pages.SettingPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs _)
        {
            FrameworkElement element = sender as FrameworkElement;
            switch (element.Name)
            {
                case "ShowError":
                    throw new Exception(NotifyMessage.Text);
                case "ShowMessage":
                    UIHelper.ShowMessage(NotifyMessage.Text);
                    break;
                case "ShowAsyncError":
                    Thread thread = new(() => throw new Exception(NotifyMessage.Text));
                    thread.Start();
                    break;
                case "ShowProgressBar":
                    UIHelper.ShowProgressBar();
                    break;
                case "HideProgressBar":
                    UIHelper.HideProgressBar();
                    break;
                case "ErrorProgressBar":
                    UIHelper.ErrorProgressBar();
                    break;
                case "PausedProgressBar":
                    UIHelper.PausedProgressBar();
                    break;
                case "PrograssRingState":
                    if (UIHelper.IsShowingProgressRing)
                    {
                        UIHelper.HideProgressRing();
                    }
                    else
                    {
                        UIHelper.ShowProgressRing();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
