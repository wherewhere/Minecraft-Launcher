using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using MinecraftLauncher.Pages;
using System;
using System.Drawing;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace MinecraftLauncher.Helpers
{
    internal static class UIHelper
    {
        public static float DpiX, DpiY;
        public static MainPage MainPage = null;

        public enum NavigationThemeTransition
        {
            Default,
            Entrance,
            DrillIn,
            Suppress
        }

        static UIHelper()
        {
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            DpiX = graphics.DpiX / 96;
            DpiY = graphics.DpiY / 96;
        }

        public static void Navigate(Type pageType, object e = null, NavigationThemeTransition Type = NavigationThemeTransition.Default)
        {
            switch (Type)
            {
                case NavigationThemeTransition.DrillIn:
                    _ = (MainPage?.Frame.Navigate(pageType, e, new DrillInNavigationTransitionInfo()));
                    break;
                case NavigationThemeTransition.Entrance:
                    _ = (MainPage?.Frame.Navigate(pageType, e, new EntranceNavigationTransitionInfo()));
                    break;
                case NavigationThemeTransition.Suppress:
                    _ = (MainPage?.Frame.Navigate(pageType, e, new SuppressNavigationTransitionInfo()));
                    break;
                case NavigationThemeTransition.Default:
                default:
                    _ = (MainPage?.Frame.Navigate(pageType, e, new DrillInNavigationTransitionInfo()));
                    break;
            }
        }

        public static async void CheckTheme()
        {
            while (Window.Current?.Content is null)
            {
                await Task.Delay(100);
            }

            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                foreach (CoreApplicationView item in CoreApplication.Views)
                {
                    await item.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        (Window.Current.Content as FrameworkElement).RequestedTheme = SettingsHelper.Theme;
                    });
                }

                bool IsDark = IsDarkTheme(SettingsHelper.Theme);

                if (IsDark)
                {
                    ApplicationViewTitleBar view = ApplicationView.GetForCurrentView().TitleBar;
                    view.ButtonBackgroundColor = view.InactiveBackgroundColor = view.ButtonInactiveBackgroundColor = Colors.Transparent;
                    view.ButtonForegroundColor = Colors.White;
                }
                else
                {
                    ApplicationViewTitleBar view = ApplicationView.GetForCurrentView().TitleBar;
                    view.ButtonBackgroundColor = view.InactiveBackgroundColor = view.ButtonInactiveBackgroundColor = Colors.Transparent;
                    view.ButtonForegroundColor = Colors.Black;
                }
            }
        }

        public static void ChangeTheme(object sender)
        {
            if (sender != null)
            {
                (sender as Frame).RequestedTheme = SettingsHelper.Theme;
            }
        }

        public static bool IsDarkTheme(ElementTheme Theme)
        {
            return Theme == ElementTheme.Default ? Application.Current.RequestedTheme == ApplicationTheme.Dark : Theme == ElementTheme.Dark;
        }

        public static string GetSizeString(this double size)
        {
            int index = 0;
            while (true)
            {
                index++;
                size /= 1024;
                if (size is > 0.7 and < 716.8) { break; }
                else if (size >= 716.8) { continue; }
                else if (size <= 0.7)
                {
                    size *= 1024;
                    index--;
                    break;
                }
            }
            string str = string.Empty;
            switch (index)
            {
                case 0: str = "B"; break;
                case 1: str = "KB"; break;
                case 2: str = "MB"; break;
                case 3: str = "GB"; break;
                case 4: str = "TB"; break;
                default:
                    break;
            }
            return $"{size:N2}{str}";
        }
    }
}
