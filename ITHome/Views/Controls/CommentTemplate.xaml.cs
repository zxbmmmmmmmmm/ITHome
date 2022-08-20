using ITHome.Core.Models;
using ITHome.Core.Models.Post;
using ITHome.Helpers;
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
        public bool IsExpanded = false;
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
        public static readonly DependencyProperty CommentProperty = DependencyProperty.Register("Comment", typeof(Comment), typeof(CommentTemplate), new PropertyMetadata(null,OnItemPropertyChanged));

        private async void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            await new CommentContentDialog(Comment.Id).ShowAsync();
        }
        private static void OnItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CommentTemplate;
            control.ReplyGrid.Visibility = Visibility.Collapsed;
            control.IsExpanded = false;
            control.DownvoteBtn.IsChecked = false;
            control.UpvoteBtn.IsChecked = false;
        }
        private void ReplyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsExpanded)
            {
                ReplyGrid.Visibility = Visibility.Collapsed;
                IsExpanded = false;
            }
            else
            {
                ReplyGrid.Visibility = Visibility.Visible;
                IsExpanded = true;
            }
        }

        private async void SubmitReplyBtn_Click(object sender, RoutedEventArgs e)
        {
            ReplyBtn.IsEnabled = false;
            var comment = new CommentClient
            {
                device = "",
                newsId = Comment.NewsId,
                client = Common.Settings.Client,
                elements = new List<CommentClientElement>(),
                replyCommentId = Comment.Id
            };
            var element = new CommentClientElement
            {
                content = CommentEdit.Text.ToString(),

            };
            comment.elements.Add(element);
            var jObj = await ITHomeProxy.SubmitCommentAsync(comment);
            if ((bool)jObj["success"] == true)
            {
                CommentEdit.Text = "";
            }
            new Toast(jObj["message"].ToString(), TimeSpan.FromSeconds(3)).Show();
            ReplyBtn.IsEnabled = true;
        }

        private async void UpvoteBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)UpvoteBtn.IsChecked)
            {
                var message = await ITHomeProxy.Vote(Comment.Id, 1);

                if ((bool)message["success"])
                {
                    Comment.Support += 1;
                    UpvoteTextBlock.Text = Comment.Support.ToString();
                }
                else
                {
                    if (message["message"].ToString().Contains("反对"))//已经反对过
                    {
                        await ITHomeProxy.CancelVote(Comment.Id, 2);
                        message = await ITHomeProxy.Vote(Comment.Id, 1);
                        if ((bool)message["success"])
                        {
                            Comment.Support += 1;
                            Comment.Against -= 1;
                            DownvoteTextBlock.Text = Comment.Against.ToString();
                            UpvoteTextBlock.Text = Comment.Support.ToString();
                        }
                    }
                    new Toast(message["message"].ToString()).Show();
                }
            }
            else
            {
                var message = await ITHomeProxy.CancelVote(Comment.Id, 1);
                if ((bool)message["success"])
                {
                    Comment.Support -= 1;
                    UpvoteTextBlock.Text = Comment.Support.ToString();
                }
            }
            DownvoteBtn.IsChecked = false;
        }

        private async void DownvoteBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)DownvoteBtn.IsChecked)
            {
                var message = await ITHomeProxy.Vote(Comment.Id, 1);

                if ((bool)message["success"])
                {
                    Comment.Against += 1;
                    DownvoteTextBlock.Text = Comment.Against.ToString();
                }
                else
                {
                    if (message["message"].ToString().Contains("支持"))//已经支持过
                    {
                        await ITHomeProxy.CancelVote(Comment.Id, 1);
                        message = await ITHomeProxy.Vote(Comment.Id, 2);
                        if ((bool)message["success"])
                        {
                            Comment.Against += 1;
                            Comment.Support -= 1;
                            DownvoteTextBlock.Text = Comment.Against.ToString();
                            UpvoteTextBlock.Text = Comment.Support.ToString();
                        }
                    }
                    new Toast(message["message"].ToString()).Show();
                }
            }
            else
            {
                var message = await ITHomeProxy.CancelVote(Comment.Id, 1);
                if ((bool)message["success"])
                {
                    Comment.Against -= 1;
                    DownvoteTextBlock.Text = Comment.Against.ToString();
                }
            }
            UpvoteBtn.IsChecked = false;
        }
    }
}
