using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UMCLauncher.Pages;

namespace UMCLauncher.Helpers
{
    internal static class UIHelper
    {
        public const string AppTitle = "Universal-like Minecraft Launcher";

        public static bool HasTitleBar = !AppWindowTitleBar.IsCustomizationSupported();
        public static bool TitleBarExtended => HasTitleBar ? MainWindow.ExtendsContentIntoTitleBar : WindowHelper.GetAppWindowForCurrentWindow().TitleBar.ExtendsContentIntoTitleBar;

        public static MainPage MainPage;
        public static MainWindow MainWindow;
        public static bool IsShowingProgressRing, IsShowingProgressBar, IsShowingMessage;
        private static readonly ObservableCollection<(string Message, InfoBarSeverity Severity)> MessageList = new();

        public enum NavigationThemeTransition
        {
            Default,
            Entrance,
            DrillIn,
            Suppress
        }

        public static void Navigate(Type pageType, object e = null, NavigationThemeTransition Type = NavigationThemeTransition.Default)
        {
            _ = Type switch
            {
                NavigationThemeTransition.DrillIn => MainPage?.NavigationViewFrame.Navigate(pageType, e, new DrillInNavigationTransitionInfo()),
                NavigationThemeTransition.Entrance => MainPage?.NavigationViewFrame.Navigate(pageType, e, new EntranceNavigationTransitionInfo()),
                NavigationThemeTransition.Suppress => MainPage?.NavigationViewFrame.Navigate(pageType, e, new SuppressNavigationTransitionInfo()),
                NavigationThemeTransition.Default => MainPage?.NavigationViewFrame.Navigate(pageType, e),
                _ => MainPage?.NavigationViewFrame.Navigate(pageType, e),
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

        public static async void ShowMessage(string message, InfoBarSeverity severity = InfoBarSeverity.Warning)
        {
            MessageList.Add((message, severity));
            if (!IsShowingMessage)
            {
                IsShowingMessage = true;
                while (MessageList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(MessageList[0].Message))
                    {
                        string messages = $"{MessageList[0].Message.Replace("\n", " ")}";
                        MainPage?.ShowMessage(messages, MessageList.Count, MessageList[0].Severity);
                        await Task.Delay(3000);
                    }
                    MessageList.RemoveAt(0);
                    if (MessageList.Count == 0)
                    {
                        MainPage?.PageHeader?.RectanglePointerExited();
                    }
                }
                IsShowingMessage = false;
            }
        }

        public static string GetGreetings()
        {
            string str = "";
            DateTime now = DateTime.Now;
            int times = now.Hour;
            if (times is >= 0 and < 6) { str = "熬夜对身体不好哦"; }
            if (times is >= 6 and < 9) { str = "早安"; }
            if (times is >= 9 and < 11) { str = "上午好"; }
            if (times is >= 11 and < 13) { str = "中午好"; }
            if (times is >= 13 and < 17) { str = "下午好"; }
            if (times is >= 17 and < 19) { str = "吃过晚饭了吗"; }
            if (times is >= 19 and < 24) { str = "晚安"; }
            return str;
        }
    }
}
