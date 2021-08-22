using Microsoft.UI.Xaml.Controls;
using MinecraftLauncher.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            UIHelper.CheckTheme();
            NavigationView.SelectedItem = NavigationView.MenuItems[0];
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                _ = NavigationViewFrame.Navigate(typeof(SettingPages.SettingPage), args.RecommendedNavigationTransitionInfo);
            }
            else if (args.SelectedItemContainer != null)
            {
                string NavItemTag = args.SelectedItemContainer.Tag.ToString();
                switch (NavItemTag)
                {
                    case "Home":
                        _ = NavigationViewFrame.Navigate(typeof(HomePage), args.RecommendedNavigationTransitionInfo);
                        break;
                    default:
                        break;
                }
            }
            try
            {
                if (args.SelectedItemContainer.Content != null)
                {
                    NavigationView.Header = args.SelectedItemContainer.Content;
                    NavigationView.PaneTitle = args.SelectedItemContainer.Content.ToString();
                }
                else
                {
                    NavigationView.Header = " ";
                    NavigationView.PaneTitle = "麦块启动器";
                }
            }
            catch
            {
                NavigationView.Header = " ";
                NavigationView.PaneTitle = "麦块启动器";
            }
        }
    }
}
