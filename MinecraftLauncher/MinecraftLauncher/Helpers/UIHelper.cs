using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using MinecraftLauncher.Pages;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace MinecraftLauncher.Helpers
{
    internal static class UIHelper
    {
        public const string Error = "";
        public const string Seccess = "";
        public const string Message = "";
        public const string Warnning = "";

        public static float DpiX, DpiY;
        public static MainPage MainPage;
        public static MainWindow MainWindow;
        public static bool IsShowingProgressRing, IsShowingProgressBar, IsShowingMessage;
        private static readonly ObservableCollection<(string message, string info, MainPage.MessageColor color)> MessageList = new();

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
            _ = Type switch
            {
                NavigationThemeTransition.DrillIn => MainPage?.Frame.Navigate(pageType, e, new DrillInNavigationTransitionInfo()),
                NavigationThemeTransition.Entrance => MainPage?.Frame.Navigate(pageType, e, new EntranceNavigationTransitionInfo()),
                NavigationThemeTransition.Suppress => MainPage?.Frame.Navigate(pageType, e, new SuppressNavigationTransitionInfo()),
                NavigationThemeTransition.Default => MainPage?.Frame.Navigate(pageType, e, new DrillInNavigationTransitionInfo()),
                _ => MainPage?.Frame.Navigate(pageType, e, new DrillInNavigationTransitionInfo()),
            };
        }

        public static void ShowProgressRing()
        {
            IsShowingProgressRing = true;
            MainPage.ShowProgressRing();
        }

        public static void HideProgressRing()
        {
            IsShowingProgressRing = false;
            MainPage.HideProgressRing();
        }

        public static void ShowProgressBar()
        {
            IsShowingProgressBar = true;
            MainPage.ShowProgressBar();
        }

        public static void PausedProgressBar()
        {
            IsShowingProgressBar = true;
            MainPage.PausedProgressBar();
        }

        public static void ErrorProgressBar()
        {
            IsShowingProgressBar = true;
            MainPage.ErrorProgressBar();
        }

        public static void HideProgressBar()
        {
            IsShowingProgressBar = false;
            MainPage.HideProgressBar();
        }

        public static async void ShowMessage(string message, string info = Message, MainPage.MessageColor color = MainPage.MessageColor.Blue)
        {
            MessageList.Add((message, info, color));
            if (!IsShowingMessage)
            {
                IsShowingMessage = true;
                while (MessageList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(MessageList[0].message))
                    {
                        string messages = $"{MessageList[0].message.Replace("\n", " ")}";
                        MainPage.ShowMessage(messages, MessageList[0].info, MessageList[0].color);
                        await Task.Delay(3000);
                    }
                    MessageList.RemoveAt(0);
                    if (MessageList.Count == 0)
                    {
                        MainPage.RectanglePointerExited();
                    }
                }
                IsShowingMessage = false;
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
                (sender as Page).RequestedTheme = SettingsHelper.Theme;
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
