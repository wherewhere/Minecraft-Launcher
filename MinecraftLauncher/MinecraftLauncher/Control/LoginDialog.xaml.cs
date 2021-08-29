using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using MinecraftLauncher.Helpers;
using MinecraftLauncher.Pages;
using ModuleLauncher.Re.Authenticators;
using ModuleLauncher.Re.Utils.Extensions;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher.Control
{
    public sealed partial class LoginDialog : ContentDialog
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && !string.IsNullOrEmpty(Username.Text) && ((bool)IsOffline.IsChecked || !string.IsNullOrEmpty(Password.Password)))
            {
                ContentDialog_PrimaryButtonClick(sender as ContentDialog, null);
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            UIHelper.Navigate(typeof(BrowserPage), new object[] { true });
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            CheckLogin();
        }

        private async void CheckLogin()
        {
            UIHelper.MainPage.AppTitle.Text = "正在登录...";
            if ((bool)IsOffline.IsChecked ? await SettingsHelper.CheckLogin(AuthenticatorType.OfflineAuthenticator, new object[] { Username.Text }) : await SettingsHelper.CheckLogin(AuthenticatorType.MojangAuthenticator, new object[] { Username.Text, Password.Password }))
            {
                UIHelper.ShowMessage("登录成功", UIHelper.Seccess, MainPage.MessageColor.Blue);
                UIHelper.MainPage.HelloWorld();
                UIHelper.HideProgressBar();
            }
            else
            {
                UIHelper.ShowMessage("登录失败", UIHelper.Warnning, MainPage.MessageColor.Yellow);
                UIHelper.ErrorProgressBar();
                UIHelper.MainPage.AppTitle.Text = UIHelper.AppTitle;
            }
        }
    }
}
