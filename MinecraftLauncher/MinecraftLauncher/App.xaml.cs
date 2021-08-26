using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MinecraftLauncher.Core.Exceptions;
using MinecraftLauncher.Helpers;
using MinecraftLauncher.Pages;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            UnhandledException += Application_UnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        private MainWindow m_window;

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            RegisterExceptionHandlingSynchronizationContext();

            Frame rootFrame = new();
            m_window = new MainWindow
            {
                ExtendsContentIntoTitleBar = true,
                Content = rootFrame
            };
            //m_window.SetTitleBar(m_window.CustomTitleBar);
            _ = rootFrame.Navigate(typeof(MainPage));

            m_window.Activate();
        }
        
        private void Application_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            if (SettingsHelper.Get<bool>(SettingsHelper.ShowOtherException))
            {
                UIHelper.ShowMessage($"程序出现了错误……\n{e.Exception.Message}\n(0x{Convert.ToString(e.Exception.HResult, 16)})"
#if DEBUG
                    + $"\n{e.Exception.StackTrace}"
#endif
                , "", MainPage.MessageColor.Yellow);
            }
            SettingsHelper.LogManager.GetLogger("UnhandledException").Error($"\n{e.Exception.Message}\n{e.Exception.HResult}\n{e.Exception.StackTrace}\nHelperLink: {e.Exception.HelpLink}",e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            if (SettingsHelper.Get<bool>(SettingsHelper.ShowOtherException))
            {
                UIHelper.ShowMessage(e.ExceptionObject.ToString(), "", MainPage.MessageColor.Red);
            }
            SettingsHelper.LogManager.GetLogger("UnhandledException").Error(e.ExceptionObject.ToString());
        }
        /// <summary>
        /// Should be called from OnActivated and OnLaunched
        /// </summary>
        private void RegisterExceptionHandlingSynchronizationContext()
        {
            ExceptionHandlingSynchronizationContext
                .Register()
                .UnhandledException += SynchronizationContext_UnhandledException;
        }

        private void SynchronizationContext_UnhandledException(object sender, Core.Exceptions.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            if (SettingsHelper.Get<bool>(SettingsHelper.ShowOtherException))
            {
                UIHelper.ShowMessage($"程序出现了错误……\n{e.Exception.Message}\n(0x{Convert.ToString(e.Exception.HResult, 16)})"
#if DEBUG
                    + $"\n{e.Exception.StackTrace}"
#endif
                , "", MainPage.MessageColor.Yellow);
            }
            SettingsHelper.LogManager.GetLogger("UnhandledException").Error($"\n{e.Exception.Message}\n{e.Exception.HResult}(0x{Convert.ToString(e.Exception.HResult, 16)})\n{e.Exception.StackTrace}\nHelperLink: {e.Exception.HelpLink}", e.Exception);
        }
    }
}
