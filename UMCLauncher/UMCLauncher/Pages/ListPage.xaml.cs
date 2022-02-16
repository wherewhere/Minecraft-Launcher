using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ModuleLauncher.Re.Models.Locators.Minecraft;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMCLauncher.Core.Helpers.DataHelper;
using UMCLauncher.Helpers;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UMCLauncher.Pages
{
    /// <summary>
    /// 列表页面
    /// </summary>
    public sealed partial class ListPage : Page
    {
        internal VersionDS VersionDS = new();

        public ListPage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = Refresh();
        }

        private async Task Refresh(int p = -1)
        {
            UIHelper.ShowProgressBar();
            if (p == -2)
            {
                await VersionDS.Refresh();
            }
            else
            {
                _ = VersionDS.LoadMoreItemsAsync(20);
            }
            UIHelper.HideProgressBar();
        }

        private async void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            using Deferral RefreshCompletionDeferral = args.GetDeferral();
            await Refresh(-2);
        }
    }

    /// <summary>
    /// Provide list of Minecraft Versions. <br/>
    /// You can bind this ds to ItemSource to enable incremental loading ,
    /// or call LoadMoreItemsAsync to load more.
    /// </summary>
    internal class VersionDS : DataSourceBase<Minecraft>
    {
        private int _loaditems;

        protected override async Task<IList<Minecraft>> LoadItemsAsync(uint count)
        {
            if (_currentPage == 1)
            {
                await LaunchHelper.GetMinecrafts();
                _loaditems = 0;
            }
            if (_loaditems == LaunchHelper.Minecrafts.Count())
            {
                return null;
            }
            else if (_loaditems + count > LaunchHelper.Minecrafts.Count())
            {
                List<Minecraft> results = LaunchHelper.Minecrafts.ToList().GetRange(_loaditems, LaunchHelper.Minecrafts.Count() - _loaditems);
                _loaditems = LaunchHelper.Minecrafts.Count();
                return results;
            }
            else
            {
                List<Minecraft> results = LaunchHelper.Minecrafts.ToList().GetRange(_loaditems, (int)count);
                _loaditems += (int)count;
                return results;
            }
        }

        protected override void AddItems(IList<Minecraft> items)
        {
            if (items != null)
            {
                foreach (Minecraft news in items)
                {
                    if (!this.Any(n => n.Locality.Json == news.Locality.Json))
                    {
                        Add(news);
                    }
                }
            }
        }
    }
}
