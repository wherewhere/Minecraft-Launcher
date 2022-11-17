using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ModuleLauncher.Re.Downloaders.Concrete;
using ModuleLauncher.Re.Models.Downloaders;
using ModuleLauncher.Re.Models.Downloaders.Minecraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMCLauncher.Helpers;
using UMCLauncher.Helpers.DataHelper;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UMCLauncher.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DownloadPage : Page
    {
        internal MinecraftDS MinecraftRS = new(MinecraftDownloadType.Release);
        internal MinecraftDS MinecraftSS = new(MinecraftDownloadType.Snapshot);
        internal MinecraftDS MinecraftOB = new(MinecraftDownloadType.OldBeta);
        internal MinecraftDS MinecraftOA = new(MinecraftDownloadType.OldAlpha);

        public DownloadPage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = Refresh(-2);
        }

        private async Task Refresh(int p = -1)
        {
            UIHelper.ShowProgressBar();
            if (p == -2)
            {
                await MinecraftRS.Refresh();
                await MinecraftSS.Refresh();
                await MinecraftOB.Refresh();
                await MinecraftOA.Refresh();
            }
            else
            {
                switch (p)
                {
                    case 1: await MinecraftRS.Refresh(); break;
                    case 2: await MinecraftSS.Refresh(); break;
                    case 3: await MinecraftOB.Refresh(); break;
                    case 4: await MinecraftOA.Refresh(); break;
                    default: _ = Refresh(-2); break;
                }
            }
            UIHelper.HideProgressBar();
        }

        private async void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            using Deferral RefreshCompletionDeferral = args.GetDeferral();
            switch (sender.Name)
            {
                case "RefreshRelease": await Refresh(1); break;
                case "RefreshSnapshot": await Refresh(2); break;
                case "RefreshBeta": await Refresh(3); break;
                case "RefreshAlpha": await Refresh(4); break;
                default: await Refresh(-2); break;
            }
        }

        private async void ListViewItem_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ContentDialog dialog = new()
            {
                Title = "下载",
                Content = $"确认下载 Minecraft {element.Tag} ？",
                PrimaryButtonText = "开始下载",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = XamlRoot
            };
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                UIHelper.ShowProgressBar();
                UIHelper.MainPage.AppTitle.Text = "正在下载 Jar 文件...";
                await MinecraftRS.Downloader.Download(element.Tag.ToString(), true);
                await DownloadHelper.DownloadDependencies(element.Tag.ToString(), true);
                UIHelper.HideProgressBar();
            }
        }
    }

    /// <summary>
    /// Provide list of Minecraft Download Versions. <br/>
    /// You can bind this ds to ItemSource to enable incremental loading ,
    /// or call LoadMoreItemsAsync to load more.
    /// </summary>
    internal class MinecraftDS : DataSourceBase<MinecraftDownloadItem>
    {
        private IEnumerable<MinecraftDownloadItem> Minecrafts;
        internal MinecraftDownloader Downloader;
        private readonly MinecraftDownloadType _type;
        private DownloaderSource _source;
        private int _loaditems;

        internal MinecraftDS(MinecraftDownloadType Type = MinecraftDownloadType.Release, DownloaderSource Source = DownloaderSource.Official)
        {
            _type = Type;
            _source = Source;
        }

        protected override async Task<IList<MinecraftDownloadItem>> LoadItemsAsync(uint count)
        {
            Downloader = DownloadHelper.MinecraftDownload(_source);
            if (_currentPage == 1)
            {
                Minecrafts = (await Downloader.GetRemoteMinecrafts()).Where(x => x.Type == _type);
                _loaditems = 0;
            }
            if (_loaditems == Minecrafts.Count())
            {
                return null;
            }
            else if (_loaditems + count > Minecrafts.Count())
            {
                List<MinecraftDownloadItem> results = Minecrafts.ToList().GetRange(_loaditems, Minecrafts.Count() - _loaditems);
                _loaditems = Minecrafts.Count();
                return results;
            }
            else
            {
                List<MinecraftDownloadItem> results = Minecrafts.ToList().GetRange(_loaditems, (int)count);
                _loaditems += (int)count;
                return results;
            }
        }

        protected override void AddItems(IList<MinecraftDownloadItem> items)
        {
            if (items != null)
            {
                foreach (MinecraftDownloadItem news in items)
                {
                    if (!this.Any(n => n.Id == news.Id))
                    {
                        Add(news);
                    }
                }
            }
        }

        public void ChangeChannel(DownloaderSource Source)
        {
            _source = Source;
            _ = Refresh();
        }
    }
}
