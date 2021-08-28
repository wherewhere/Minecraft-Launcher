using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncher.Helpers;
using System.Threading.Tasks;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// 列表页面
    /// </summary>
    public sealed partial class ListPage : Page
    {
        public ListPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await LaunchHelper.GetMinecrafts();
            ListView.ItemsSource = LaunchHelper.Minecrafts;
        }

        private async Task Refresh(int p = -1)
        {
            if (p == -2)
            {
                _ = ScrollViewer.ChangeView(null, 0, null);
            }
            await LaunchHelper.GetMinecrafts();
            ListView.ItemsSource = LaunchHelper.Minecrafts;
        }

        private async void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            using Deferral RefreshCompletionDeferral = args.GetDeferral();
            await Refresh(-2);
        }
    }
}
