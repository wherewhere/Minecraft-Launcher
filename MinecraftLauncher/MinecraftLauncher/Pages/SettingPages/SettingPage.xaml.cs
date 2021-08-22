using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MinecraftLauncher.Helpers;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher.Pages.SettingPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingPage : Page
    {
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
                    UIHelper.CheckTheme();
                    break;
                case "Light":
                    SettingsHelper.Set("IsBackgroundColorFollowSystem", false);
                    SettingsHelper.Set("IsDarkTheme", false);
                    UIHelper.CheckTheme();
                    break;
                case "Default":
                    SettingsHelper.Set("IsBackgroundColorFollowSystem", true);
                    UIHelper.CheckTheme();
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
