using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ITHome.Core.Helpers;
using ITHome.Core.Models;
using ITHome.Core.Services;
using ITHome.Helpers;
using ITHome.Services;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace ITHome.Views
{
    public sealed partial class HomePage : Page, INotifyPropertyChanged, IBackNavigationHandler
    {
        private News _selected;
        private WinUI.TwoPaneViewPriority _twoPanePriority;
        DispatcherTimer _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(10),
        };

        public ObservableCollection<NewsSlide> NewsSlides = new ObservableCollection<NewsSlide>();
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
            NavigationCacheMode = NavigationCacheMode.Enabled;
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            _timer.Tick += ChangeImage;
            _timer.Start();
        }

        private void ChangeImage(object sender, object e)
        {
            SlideFlipView.SelectedIndex += 1;
            Singleton<LiveTileService>.Instance.SampleUpdate(NewsList[FlipViewPipsPager.SelectedPageIndex]);//更新动态磁贴(假的)
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            base.OnNavigatedTo(e);
            if(NewsList.Count == 0)
            {
                NewsList.Clear();
                GetNewsList();
            }

        }

        private async void GetNewsSlide()
        {
            var slides = await ITHomeProxy.GetNewsSlideList();
            NewsSlides = slides.News;
            NewsSlides.Insert(0, slides.News.Last());
            NewsSlides.Add(slides.News.First());

            SlideFlipView.ItemsSource = NewsSlides;
            FlipViewPipsPager.NumberOfPages = NewsSlides.Count() - 2;
            FlipViewPipsPager.MaxVisiblePips = NewsSlides.Count() - 2;

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
            GetNewsSlide();

        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void LoadMoreBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var timeStamp = (long)(NewsList[NewsList.Count - 1].PostDate - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
            var data = await ITHomeProxy.GetNewsList(timeStamp.ToString() + "000");
            foreach (var news in data.News)
                NewsList.Add(news);
        }

        private void SlideListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flip = sender as FlipView;
            if (flip.SelectedIndex == NewsSlides.Count() - 1)//最后一个
                flip.SelectedIndex = 1;
            if (flip.SelectedIndex == 0)//第一个
                flip.SelectedIndex = NewsSlides.Count() - 2;
            FlipViewPipsPager.SelectedPageIndex = flip.SelectedIndex - 1;
        }

        private void FlipViewPipsPager_SelectedIndexChanged(WinUI.PipsPager sender, WinUI.PipsPagerSelectedIndexChangedEventArgs args)
        {
            SlideFlipView.SelectedIndex = sender.SelectedPageIndex + 1;
        }

        private void SlideListView_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void SlideListView_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _timer.Start();
        }

        private void SlideImage_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            
            var image = sender as Control;
            NewsDetail.NewsUrl = image.Tag.ToString();
            NewsListView.SelectedItem = null;
            if (twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane)
            {
                OnPageCanGoBackChanged?.Invoke(this, true);
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2;
            }
        }
    }
}
