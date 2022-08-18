using ITHome.Core.Models;
using ITHome.Helpers;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ITHome.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewsDetailPage : Page, INotifyPropertyChanged
    {

        public NewsDetailPage()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string NewsUrl
        {
            get { return (string)GetValue(NewsUrlProperty); }
            set { SetValue(NewsUrlProperty, value); }
        }
        public static readonly DependencyProperty NewsUrlProperty = DependencyProperty.Register(nameof(NewsUrl), typeof(string), typeof(HomeListItemControl), new PropertyMetadata(null, OnItemPropertyChanged));
        public News Item
        {
            get { return (News)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(News), typeof(HomeListItemControl), new PropertyMetadata(null, null));
        public NewsSearch NewsSearch
        {
            get => _newsSearch;
            set
            {
                if (_newsSearch != value)
                {
                    _newsSearch = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewsSearch)));
                }
            }
        }
        private NewsSearch _newsSearch = new NewsSearch();

        private static void OnItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as NewsDetailPage;
            page.GetNewsSearch();
        }
        public async void GetNewsSearch()
        {
            if (Item != null)
            {
                NewsSearch.Title = Item.Title;
                NewsSearch.PostDate = Item.PostDate;

            }

            var split = NewsUrl.Split("/");
            var length = split.Length;
            string id;
            if(NewsUrl.Contains("la"))//广告
            {
                id = split[length - 1].Replace(".htm", "");
            }
            else
            {
                id = split[length - 2] + split[length - 1].Replace(".htm", "");
            }
            NewsSearch = await ITHomeProxy.GetNewsSearch(id);
            GetComments();
            
        }
        public async void GetComments()
        {
            var comments = await ITHomeProxy.GetCommentsList(NewsSearch.Id.ToString());
            if (comments.Comments.Count > 0)
                CommentsHeader.Visibility = Visibility.Visible;
            else            
                CommentsHeader.Visibility = Visibility.Collapsed;

            if (comments.HotComments.Count > 0)
                HotCommentsHeader.Visibility = Visibility.Visible;
            else
                HotCommentsHeader.Visibility = Visibility.Collapsed;
            HotCommentsListView.ItemsSource = comments.HotComments;
            CommentsListView.ItemsSource = comments.Comments;
        }
    }
}
