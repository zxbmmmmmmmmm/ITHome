using ITHome.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace ITHome.Views.Controls
{
    public sealed partial class NewsSliderTemplate : UserControl
    {
        public NewsDetailPage DetailPage = new NewsDetailPage();
        public NewsSliderTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
            DetailPage = (((Window.Current.Content as ShellPage).shellFrame.Content as HomePage).RightFrame.Content as NewsDetailPage);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public NewsSlide NewsSlide
        {
            get { return (NewsSlide)GetValue(NewsSlideProperty); }
            set { SetValue(NewsSlideProperty, value); }
        }
        public static readonly DependencyProperty NewsSlideProperty = DependencyProperty.Register("NewsSlide", typeof(NewsSlide), typeof(NewsSliderTemplate), new PropertyMetadata(null));

        private void ImageEx_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DetailPage.Item = new News { Url = NewsSlide.Link, Title = NewsSlide.Title};
            DetailPage.NewsUrl = NewsSlide.Link;
        }
    }
}
