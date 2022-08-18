using ITHome.Core.Models;
using ITHome.Views.Dialogs;
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
    public sealed partial class CommentTemplate : UserControl, INotifyPropertyChanged
    {
        public CommentTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public Comment Comment
        {
            get { return (Comment)GetValue(CommentProperty); }
            set { SetValue(CommentProperty, value); }
        }
        public static readonly DependencyProperty CommentProperty = DependencyProperty.Register("Comment", typeof(Comment), typeof(CommentTemplate), new PropertyMetadata(null));

        private async void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            await new CommentContentDialog(Comment.Id).ShowAsync();
        }
    }
}
