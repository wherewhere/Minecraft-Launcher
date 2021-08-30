using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using MinecraftLauncher.Helpers;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrowserPage : Page
    {
        private bool IsLoginPage;
        private readonly string LoginUrl = "https://login.live.com/oauth20_authorize.srf?client_id=00000000402b5328&response_type=code&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL &redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf";
        public BrowserPage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object[] vs = e.Parameter as object[];
            if (vs[0] is string && !string.IsNullOrEmpty(vs[0] as string))
            {
                string uri = vs[0] as string;
                if (!uri.Contains("://"))
                {
                    uri = $"http://{uri}";
                }
                WebView.Source = new Uri(uri);
            }
            else if (vs[0] is bool boolean && boolean)
            {
                IsLoginPage = boolean;
                WebView.Source = new Uri(LoginUrl);
            }
        }

        private void WebView_NavigationStarting(WebView2 sender, CoreWebView2NavigationStartingEventArgs args)
        {
            UIHelper.ShowProgressBar();
            if (IsLoginPage && args.Uri.StartsWith("https://login.live.com/oauth20_desktop.srf?code="))
            {
                CheckLogin(args.Uri.Substring(args.Uri.IndexOf("=") + 1));
            }
            else if (args.Uri == LoginUrl)
            {
                IsLoginPage = true;
            }
        }

        private void WebView_NavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
        {
            if (!IsLoginPage)
            {
                UIHelper.HideProgressBar();
            }
        }

        private async void CheckLogin(string code)
        {
            UIHelper.MainPage.AppTitle.Text = "正在登录...";
            if (!string.IsNullOrEmpty(code) && await SettingsHelper.CheckLogin(AuthenticatorType.MicrosoftAuthenticator, new object[] { code }))
            {
                if (Frame.CanGoBack) { Frame.GoBack(); }
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
