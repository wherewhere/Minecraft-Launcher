using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MinecraftLauncher.Helpers;
using System;
using System.Runtime.InteropServices;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher.Pages.SettingPages
{
    [ComImport, Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitializeWithWindow
    {
        void Initialize([In] IntPtr hwnd);
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto, PreserveSig = true, SetLastError = false)]
        public static extern IntPtr GetActiveWindow();

        public SettingPage()
        {
            InitializeComponent();
            SetValue();
        }

        private void SetValue()
        {
            Java8Root.Text = SettingsHelper.GetString("Java8Root");
            MCRoot.Text = SettingsHelper.GetString("MinecraftRoot");
            Java16Root.Text = SettingsHelper.GetString("Java16Root");
            if (SettingsHelper.GetBoolen("IsBackgroundColorFollowSystem"))
            {
                Default.IsChecked = true;
            }
            else if (SettingsHelper.GetBoolen("IsDarkTheme"))
            {
                Dark.IsChecked = true;
            }
            else
            {
                Light.IsChecked = true;
            }
        }

        private void Button_Checked(object sender, RoutedEventArgs _)
        {
            FrameworkElement element = sender as FrameworkElement;
            switch(element.Name)
            {
                case "Dark":
                    SettingsHelper.Set("IsBackgroundColorFollowSystem", false);
                    SettingsHelper.Set("IsDarkTheme", true);
                    if (XamlRoot != null)
                    {
                        UIHelper.ChangeTheme(XamlRoot.Content);
                    }
                    break;
                case "Light":
                    SettingsHelper.Set("IsBackgroundColorFollowSystem", false);
                    SettingsHelper.Set("IsDarkTheme", false);
                    if (XamlRoot != null)
                    {
                        UIHelper.ChangeTheme(XamlRoot.Content);
                    }
                    break;
                case "Default":
                    SettingsHelper.Set("IsBackgroundColorFollowSystem", true);
                    if (XamlRoot != null)
                    {
                        UIHelper.ChangeTheme(XamlRoot.Content);
                    }
                    break;
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            switch(element.Name)
            {
                case "TestPage":
                    _ = Frame.Navigate(typeof(TestPage));
                    break;
                case "SaveMCRoot":
                    SettingsHelper.Set("MinecraftRoot", MCRoot.Text);
                    break;
                case "SaveJava8Root":
                    SettingsHelper.Set("Java8Root", Java8Root.Text);
                    break;
                case "SaveJava16Root":
                    SettingsHelper.Set("Java16Root", Java16Root.Text);
                    break;
                default:
                    break;
            }
        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a text file.
            FileOpenPicker open = new();
            open.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            open.FileTypeFilter.Add(".exe");

            // When running on win32, FileOpenPicker needs to know the top-level hwnd via IInitializeWithWindow::Initialize.
            if (Window.Current == null)
            {
                IInitializeWithWindow initializeWithWindowWrapper = open.As<IInitializeWithWindow>();
                IntPtr hwnd = GetActiveWindow();
                initializeWithWindowWrapper.Initialize(hwnd);
            }

            StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                FrameworkElement element = sender as FrameworkElement;
                switch (element.Name)
                {
                    case "ChooseMCRoot":
                        MCRoot.Text = file.Path.Replace("\\","/");
                        break;
                    case "ChooseJava8Root":
                        Java8Root.Text = file.Path.Replace("\\", "/");
                        break;
                    case "ChooseJava16Root":
                        Java16Root.Text = file.Path.Replace("\\", "/");
                        break;
                    default:
                        break;
                }
            }
        }

        private void TextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                FrameworkElement element = sender as FrameworkElement;
                switch (element.Name)
                {
                    case "MCRoot":
                        SettingsHelper.Set("MinecraftRoot", MCRoot.Text);
                        break;
                    case "Java8Root":
                        SettingsHelper.Set("Java8Root", Java8Root.Text);
                        break;
                    case "Java16Root":
                        SettingsHelper.Set("Java16Root", Java8Root.Text);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
