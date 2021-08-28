using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MinecraftLauncher.Control;
using MinecraftLauncher.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// 账户页面
    /// </summary>
    public sealed partial class MyPage : Page
    {
        public MyPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            switch (element.Name)
            {
                case "Login":
                    LoginDialog dialog = new()
                    {
                        RequestedTheme = SettingsHelper.Theme,
                        XamlRoot = XamlRoot
                    };
                    _ = dialog.ShowAsync();
                    break;
                default:
                    break;
            }
        }
    }
}
