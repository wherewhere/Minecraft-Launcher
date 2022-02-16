using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ModuleLauncher.Re.Launcher;
using ModuleLauncher.Re.Models.Locators.Minecraft;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UMCLauncher.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UMCLauncher.Pages
{
    /// <summary>
    /// 启动页面
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            GetDPI.Text = $"X:{UIHelper.DpiX} Y:{UIHelper.DpiY}";
            SettingsHelper.GetCapacity();
            SettingsHelper.GetAvailable();
            _ = LaunchHelper.GetMinecrafts();
            GetMemory.Text = $"{SettingsHelper.Available.GetSizeString()}/{SettingsHelper.Capacity.GetSizeString()}";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = Refresh();
        }

        private async Task Refresh()
        {
            UIHelper.ShowProgressBar();
            await LaunchHelper.GetMinecrafts();
            ObservableCollection<string> List = new();
            foreach (Minecraft item in LaunchHelper.Minecrafts)
            {
                if (item.Locality.Jar.Exists)
                {
                    List.Add(item.Locality.Version.Name);
                }
            }
            ChooseMC.ItemsSource = List;
            ChooseMC.SelectedValue = SettingsHelper.Get<string>(SettingsHelper.ChooseVersion);
            UIHelper.HideProgressBar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            switch (element.Name)
            {
                case "Start":
                    StartLaunch();
                    break;
                default:
                    break;
            }
        }

        private async void StartLaunch()
        {
            UIHelper.ShowProgressBar();
            Launcher launcher = LaunchHelper.Launch(false);
            //await DownloadHelper.DownloadDependencies(ChooseMC.SelectedValue.ToString(), true);
            UIHelper.MainPage.AppTitle.Text = "正在加载...";
            System.Diagnostics.Process Process = await launcher.Launch(ChooseMC.SelectedValue.ToString());
            SettingsHelper.Set(SettingsHelper.ChooseVersion, ChooseMC.SelectedValue.ToString());
            UIHelper.MainPage.AppTitle.Text = "正在启动...";
            string Title = await Process.StandardOutput.ReadLineAsync();
            UIHelper.HideProgressBar();
            while (!string.IsNullOrEmpty(Title))
            {
                UIHelper.MainPage.AppTitle.Text = Title;
                Title = await Process.StandardOutput.ReadLineAsync();
            }
            UIHelper.MainPage.AppTitle.Text = "已退出";
            await Task.Delay(3000);
            UIHelper.MainPage.AppTitle.Text = UIHelper.AppTitle;
        }

        private void ComboBox_TextSubmitted(ComboBox sender, ComboBoxTextSubmittedEventArgs args)
        {

        }
    }
}
