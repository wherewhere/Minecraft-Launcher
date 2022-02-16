using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading;
using UMCLauncher.Helpers;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UMCLauncher.Pages.SettingPages
{
    /// <summary>
    /// 测试页面
    /// </summary>
    public sealed partial class TestPage : Page
    {
        public TestPage() => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            switch (element.Name)
            {
                case "OpenEdge":
                    _ = Launcher.LaunchUriAsync(new Uri(WebUrl.Text));
                    break;
                case "ShowError":
                    throw new Exception(NotifyMessage.Text);
                case "ShowMessage":
                    UIHelper.ShowMessage(NotifyMessage.Text);
                    break;
                case "OpenBrowser":
                    _ = Frame.Navigate(typeof(BrowserPage), new object[] { WebUrl.Text });
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
