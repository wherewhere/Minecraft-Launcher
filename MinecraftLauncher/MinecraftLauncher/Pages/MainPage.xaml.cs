using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncher.Helpers;
using MinecraftLauncher.Pages.SettingPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string useravatar;
        public string UserAvatar
        {
            get => useravatar;
            set
            {
                useravatar = value;
                RaisePropertyChangedEvent();
            }
        }

        private string usernames;
        public string UserNames
        {
            get => usernames;
            set
            {
                if (value == null) { value = "登录"; }
                usernames = value;
                RaisePropertyChangedEvent();
            }
        }

        internal void RaisePropertyChangedEvent([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            if (name != null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
        }

        private readonly List<(string Tag, Type Page)> _pages = new()
        {
            ("Home", typeof(HomePage)),
        };

        [Obsolete]
        public MainPage()
        {
            InitializeComponent();
            UIHelper.CheckTheme();
            SettingsHelper.CheckLogin();
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            // Add handler for ContentFrame navigation.
            NavigationViewFrame.Navigated += On_Navigated;
            NavigationView.SelectedItem = NavigationView.MenuItems[0];
        }

        private void NavigationView_Navigate(string NavItemTag, NavigationTransitionInfo TransitionInfo)
        {
            Type _page = null;
            if (NavItemTag == "settings")
            {
                _page = typeof(SettingPage);
            }
            else
            {
                (string Tag, Type Page) item = _pages.FirstOrDefault(p => p.Tag.Equals(NavItemTag));
                _page = item.Page;
            }
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            Type PreNavPageType = NavigationViewFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) && !Equals(PreNavPageType, _page))
            {
                _ = NavigationViewFrame.Navigate(_page, null, TransitionInfo);
            }
        }

        private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                _ = NavigationViewFrame.Navigate(typeof(SettingPage), null, args.RecommendedNavigationTransitionInfo);
            }
            else if (args.SelectedItemContainer != null)
            {
                string NavItemTag = args.SelectedItemContainer.Tag.ToString();
                NavigationView_Navigate(NavItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private bool TryGoBack()
        {
            if (!NavigationViewFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (NavigationView.IsPaneOpen &&
                (NavigationView.DisplayMode == NavigationViewDisplayMode.Compact ||
                 NavigationView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            NavigationViewFrame.GoBack();
            return true;
        }

        private void On_Navigated(object _, NavigationEventArgs e)
        {
            NavigationView.IsBackEnabled = NavigationViewFrame.CanGoBack;
            if (NavigationViewFrame.SourcePageType == typeof(SettingPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                NavigationView.SelectedItem = (NavigationViewItem)NavigationView.SettingsItem;
                NavigationView.Header = "设置";
            }
            else if (NavigationViewFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);

                NavigationView.SelectedItem = NavigationView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));

                NavigationView.Header =
                    ((NavigationViewItem)NavigationView.SelectedItem)?.Content?.ToString();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (XamlRoot != null)
            {
                UIHelper.ChangeTheme(XamlRoot.Content);
            }
        }
    }
}
