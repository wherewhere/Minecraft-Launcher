using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PInvoke;
using System;
using System.Runtime.InteropServices;
using UMCLauncher.Helpers;
using UMCLauncher.Models;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UMCLauncher.Pages.SettingPages
{
    [ComImport, Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitializeWithWindow
    {
        void Initialize([In] IntPtr hwnd);
    }
    /// <summary>
    /// 设置页面
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        internal int SelectedTheme
        {
            get => 2 - (int)ThemeHelper.RootTheme;
            set
            {
                if (SelectedTheme != value)
                {
                    ThemeHelper.RootTheme = (ElementTheme)(2 - value);
                }
            }
        }

        internal static string VersionTextBlockText
        {
            get
            {
                string ver = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";
                string name = UIHelper.AppTitle;
                return $"{name} v{ver}";
            }
        }

        public SettingPage()
        {
            InitializeComponent();
            LaunchHelper.GetJavas();
            SetValue();
        }

        private void SetValue()
        {
            Java8Root.ItemsSource = Java16Root.ItemsSource = LaunchHelper.Javas;
            Java8Root.Text = SettingsHelper.Get<string>(SettingsHelper.Java8Root);
            MCRoot.Text = SettingsHelper.Get<string>(SettingsHelper.MinecraftRoot);
            Java16Root.Text = SettingsHelper.Get<string>(SettingsHelper.Java16Root);
        }

        private static void SaveValue(string title, string value, bool isbackground = false)
        {
            SettingsHelper.Set(title, value);
            if (SettingsHelper.Get<string>(title) == value)
            {
                if (!isbackground)
                {
                    UIHelper.ShowMessage("保存成功", UIHelper.Seccess, MainPage.MessageColor.Green);
                }
            }
            else
            {
                UIHelper.ShowMessage("保存失败", UIHelper.Error, MainPage.MessageColor.Red);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            switch (element.Name)
            {
                case "TestPage":
                    _ = Frame.Navigate(typeof(TestPage));
                    break;
                case "SaveMCRoot":
                    SaveValue(SettingsHelper.MinecraftRoot, MCRoot.Text);
                    break;
                case "SaveJava8Root":
                    SaveValue(SettingsHelper.Java8Root, Java8Root.Text);
                    break;
                case "SaveJava16Root":
                    SaveValue(SettingsHelper.Java16Root, Java16Root.Text);
                    break;
                case "LogFolder":
                    _ = await Windows.System.Launcher.LaunchFolderAsync(await ApplicationData.Current.LocalFolder.CreateFolderAsync("MetroLogs", CreationCollisionOption.OpenIfExists));
                    break;
                default:
                    break;
            }
        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;

            FileOpenPicker FileOpen = new();
            FolderPicker Folder = new();
            if (element.Name == "ChooseMCRoot")
            {
                Folder.FileTypeFilter.Add("*");
                Folder.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            }
            else if (element.Name == "ChooseJava8Root" || element.Name == "ChooseJava16Root")
            {
                FileOpen.FileTypeFilter.Add(".exe");
                FileOpen.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            }

            // When running on win32, FileOpenPicker needs to know the top-level hwnd via IInitializeWithWindow::Initialize.
            if (Window.Current == null)
            {
                IInitializeWithWindow initializeWithWindowWrapper = null;
                if (element.Name == "ChooseMCRoot")
                {
                    initializeWithWindowWrapper = Folder.As<IInitializeWithWindow>();
                }
                else if (element.Name == "ChooseJava8Root" || element.Name == "ChooseJava16Root")
                {
                    initializeWithWindowWrapper = FileOpen.As<IInitializeWithWindow>();
                }
                IntPtr hwnd = User32.GetActiveWindow();
                initializeWithWindowWrapper.Initialize(hwnd);
            }

            if (element.Name == "ChooseMCRoot")
            {
                StorageFolder folder = await Folder.PickSingleFolderAsync();
                if (folder != null)
                {
                    switch (element.Name)
                    {
                        case "ChooseMCRoot":
                            MCRoot.Text = folder.Path;
                            SaveValue(SettingsHelper.MinecraftRoot, MCRoot.Text, true);
                            break;
                    }
                }
            }
            else if (element.Name == "ChooseJava8Root" || element.Name == "ChooseJava16Root")
            {
                StorageFile file = await FileOpen.PickSingleFileAsync();
                if (file != null)
                {
                    switch (element.Name)
                    {
                        case "ChooseJava8Root":
                            Java8Root.Text = file.Path;
                            SaveValue(SettingsHelper.Java8Root, Java8Root.Text, true);
                            break;
                        case "ChooseJava16Root":
                            Java16Root.Text = file.Path;
                            SaveValue(SettingsHelper.Java16Root, Java16Root.Text, true);
                            break;
                        default:
                            break;
                    }
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
                        SaveValue(SettingsHelper.MinecraftRoot, MCRoot.Text);
                        break;
                    case "Java8Root":
                        SaveValue(SettingsHelper.Java8Root, Java8Root.Text);
                        break;
                    case "Java16Root":
                        SaveValue(SettingsHelper.Java16Root, Java16Root.Text);
                        break;
                    default:
                        break;
                }
            }
        }

        private async void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = (args.SelectedItem as JavaVersion).JavaPath;
            switch (sender.Name)
            {
                case "Java8Root":
                    if ((args.SelectedItem as JavaVersion).Version.ProductMajorPart > 9 || (args.SelectedItem as JavaVersion).Version.ProductMajorPart < 8)
                    {
                        ContentDialog dialog = new()
                        {
                            Title = "确认",
                            Content = $"{(args.SelectedItem as JavaVersion).Version.ProductVersion}并不是 JAVA 8，是否仍然使用？",
                            PrimaryButtonText = "继续",
                            CloseButtonText = "算了",
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = XamlRoot
                        };
                        ContentDialogResult result = await dialog.ShowAsync();
                        if (result == ContentDialogResult.Primary)
                        {
                            SaveValue(SettingsHelper.Java8Root, Java8Root.Text);
                        }
                        else
                        {
                            sender.Text = SettingsHelper.Get<string>(SettingsHelper.Java8Root);
                        }
                    }
                    else
                    {
                        SaveValue(SettingsHelper.Java8Root, Java8Root.Text, true);
                    }
                    break;
                case "Java16Root":
                    if ((args.SelectedItem as JavaVersion).Version.ProductMajorPart < 16)
                    {
                        ContentDialog dialog = new()
                        {
                            Title = "确认",
                            Content = $"{(args.SelectedItem as JavaVersion).Version.ProductVersion}并不是 JAVA 16，是否仍然使用？",
                            PrimaryButtonText = "继续",
                            CloseButtonText = "算了",
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = XamlRoot
                        };
                        ContentDialogResult result = await dialog.ShowAsync();
                        if (result == ContentDialogResult.Primary)
                        {
                            SaveValue(SettingsHelper.Java16Root, Java16Root.Text);
                        }
                        else
                        {
                            sender.Text = SettingsHelper.Get<string>(SettingsHelper.Java16Root);
                        }
                    }
                    else
                    {
                        SaveValue(SettingsHelper.Java16Root, Java16Root.Text, true);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
