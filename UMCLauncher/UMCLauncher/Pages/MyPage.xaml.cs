using LiteDB;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ModuleLauncher.Re.Models.Authenticators;
using UMCLauncher.Control;
using UMCLauncher.Helpers;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UMCLauncher.Pages
{
    /// <summary>
    /// 账户页面
    /// </summary>
    public sealed partial class MyPage : Page
    {
        public MyPage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Refresh();
        }

        private void Refresh(int p = -1)
        {
            UIHelper.ShowProgressBar();
            if (p == -2)
            {
                _ = ScrollViewer.ChangeView(null, 0, null);
            }
            using (LiteDatabase db = new(SettingsHelper.AccountPath))
            {
                ILiteCollection<AuthenticateResult> AuthenticateResults = db.GetCollection<AuthenticateResult>();
                ListView.ItemsSource = AuthenticateResults.FindAll();
            }
            UIHelper.HideProgressBar();
        }

        private void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            using Deferral RefreshCompletionDeferral = args.GetDeferral();
            Refresh(-2);
        }

        private void ListViewItem_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element.Tag.ToString() == "登录")
            {
                LoginDialog dialog = new()
                {
                    XamlRoot = XamlRoot
                };
                _ = dialog.ShowAsync();
            }
            else
            {
                using LiteDatabase db = new(SettingsHelper.AccountPath);
                ILiteCollection<AuthenticateResult> AuthenticateResults = db.GetCollection<AuthenticateResult>();
                SettingsHelper.Authentication = AuthenticateResults.FindById(element.Tag.ToString());
                SettingsHelper.Set(SettingsHelper.UserUID, SettingsHelper.Authentication.Uuid);
                UIHelper.ShowMessage("切换成功", UIHelper.Seccess, MainPage.MessageColor.Blue);
                UIHelper.MainPage.HelloWorld();
            }
        }
    }
}
