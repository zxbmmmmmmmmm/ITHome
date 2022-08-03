using ITHome.Core.Models;
using ITHome.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
        public News Item
        {
            get { return (News)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(News), typeof(HomeListItemControl), new PropertyMetadata(null, OnItemPropertyChanged));
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
        private NewsSearch _newsSearch;

        private static void OnItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as NewsDetailPage;
            page.GetNewsSearch();
        }
        public async void GetNewsSearch()
        {
            var split = Item.Url.Split("/");
            var length = split.Length;
            string url;
            if(Item.Url.Contains("la"))//广告
            {
                url = split[length - 1].Replace(".htm", "");
            }
            else
            {
                url = split[length - 2] + split[length - 1].Replace(".htm", "");
            }
            NewsSearch = await ITHomeProxy.GetNewsSearch(url);
        }
    }
}
