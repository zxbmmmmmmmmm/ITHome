using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using ITHome.Core.Models;
using ITHome.Core.Services;
using ITHome.Helpers;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace ITHome.Views
{
    public sealed partial class HomePage : Page, INotifyPropertyChanged, IBackNavigationHandler
    {
        private News _selected;
        private WinUI.TwoPaneViewPriority _twoPanePriority;

        public event EventHandler<bool> OnPageCanGoBackChanged;

        public News Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public WinUI.TwoPaneViewPriority TwoPanePriority
        {
            get { return _twoPanePriority; }
            set { Set(ref _twoPanePriority, value); }
        }

        public ObservableCollection<News> NewsList { get; private set; } = new ObservableCollection<News>();

        public HomePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NewsList.Clear();
            GetNewsList();
        }

        public bool TryCloseDetail()
        {
            if (TwoPanePriority == WinUI.TwoPaneViewPriority.Pane2)
            {
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1;
                return true;
            }

            return false;
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane)
            {
                OnPageCanGoBackChanged?.Invoke(this, true);
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2;
            }
        }

        private void OnModeChanged(WinUI.TwoPaneView sender, object args)
        {
            if (twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane)
            {
                OnPageCanGoBackChanged?.Invoke(this, true);
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2;
            }
            else
            {
                OnPageCanGoBackChanged?.Invoke(this, false);
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1;
            }
        }

        public void GoBack()
        {
            if (TwoPanePriority == WinUI.TwoPaneViewPriority.Pane2)
            {
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1;
                OnPageCanGoBackChanged?.Invoke(this, false);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }
        private async void GetNewsList()
        {
            var timeStamp = (long)(DateTime.Now.ToLocalTime() - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
            var data = await ITHomeProxy.GetNewsList(timeStamp.ToString() + "000");
            foreach (var news in data.News)
                NewsList.Add(news);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void LoadMoreBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var timeStamp = (long)(NewsList[NewsList.Count - 1].PostDate - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
            var data = await ITHomeProxy.GetNewsList(timeStamp.ToString() + "000");
            foreach (var news in data.News)
                NewsList.Add(news);
        }
    }
}
