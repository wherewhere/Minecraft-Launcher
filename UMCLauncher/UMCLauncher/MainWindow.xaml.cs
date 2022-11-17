using Microsoft.UI.Xaml;
using PInvoke;
using System;
using UMCLauncher.Helpers;
using UMCLauncher.Pages;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UMCLauncher
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static float ScalingFactor;
        public BackdropHelper Backdrop;

        public MainWindow()
        {
            InitializeComponent();
            Backdrop = new BackdropHelper(this);
            UIHelper.MainWindow = this;
            MainPage MainPage = new();
            Content = MainPage;
            SetBackdrop();
            GetDPI();
        }

        private void SetBackdrop()
        {
            BackdropType type = SettingsHelper.Get<BackdropType>(SettingsHelper.SelectedBackdrop);
            Backdrop.SetBackdrop(type);
        }

        private void GetDPI()
        {
            IntPtr hwnd = WindowNative.GetWindowHandle(this);
            int dpi = User32.GetDpiForWindow(hwnd);
            ScalingFactor = (float)dpi / 96;
        }
    }
}
