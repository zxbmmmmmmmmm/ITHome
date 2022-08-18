using ITHome.Helpers;
using System;
using System.Collections.Generic;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace ITHome.Views.Dialogs
{
    public sealed partial class CommentContentDialog : ContentDialog
    {
        public CommentContentDialog(int commentId)
        {
            this.InitializeComponent();
            GetComment(commentId);
        }

        private async void GetComment(int id)
        {
            var comment = await ITHomeProxy.GetCommentContent(id.ToString());
            MainComment.Comment = comment;
            CommentsListView.ItemsSource = comment.Children;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
