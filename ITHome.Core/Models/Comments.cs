using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ITHome.Core.Models
{
    public class CommentsList
    {
        public ObservableCollection<Comment> Comments { get; set; }
        public ObservableCollection<Comment> HotComments { get; set; }

        public static CommentsList CreateFromJson(JToken token)
        {
            var commentsList = new CommentsList();
            commentsList.Comments = new ObservableCollection<Comment>();
            commentsList.HotComments = new ObservableCollection<Comment>();
            foreach (var comment in token["content"]["hotComments"])
                commentsList.HotComments.Add(Comment.CreateFromJson(comment));
            foreach (var comment in token["content"]["comments"])
                commentsList.Comments.Add(Comment.CreateFromJson(comment));
            return commentsList;
        }
    }
    
    public class Comment
    {
        public int Id { get; set; }
        public int Support { get; set; }
        public int Against { get; set; }
        public int ExpandCount { get; set; }

        public string City { get; set; }
        public DateTime PostTime { get; set; }
        public string Tail { get; set; }
        public ObservableCollection<Comment> Children { get; set; }
        public CommentElements CommentElements { get; set; }
        public User User { get; set; }
        public int NewsId { get; set; }
        public string Floor { get; set; }

        public static Comment CreateFromJson(JToken token)
        {
            var comment = new Comment
            {
                Id = token.Value<int>("id"),
                Support = token.Value<int>("support"),
                Against = token.Value<int>("against"),
                ExpandCount = token.Value<int>("expandCount"),
                City = token.Value<string>("city"),
                Tail = token.Value<string>("tail"),
                Floor = token.Value<string>("floorStr"),
                PostTime = token.Value<DateTime>("postTime"),
                CommentElements = CommentElements.CreateFromJson(token.Value<JToken>("elements")),
                User = User.CreateFromJson(token.Value<JToken>("userInfo")),
            };
            if (token["newsId"] != null)
                comment.NewsId = token.Value<int>("newsId");
            if (token["children"] != null)
            {
                comment.Children = new ObservableCollection<Comment>();
                foreach (var child in token["children"])
                {
                    comment.Children.Add(CreateFromJson(child));
                }
            } 
            return comment;
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string Nick { get; set; }
        public string Avatar { get; set;}
        public static User CreateFromJson(JToken token)
        {
            var user = new User
            {
                Id = token.Value<int>("id"),
                Nick = token.Value<string>("userNick"),
                Avatar = token.Value<string>("userAvatar"),
            };
            //user.Avatar.Replace("https://avatar.ithome.com/avatars/001/92/91/39_60.jpg", "ms-Appx:///Assets/App/guest.png");
            return user;
        }
    }

    public class CommentElements
    {
        public string Content { get; set; }
        public static CommentElements CreateFromJson(JToken token)
        {
            var first = token[0];
            var elements = new CommentElements
            {
                Content = first.Value<string>("content"),
            };
            return elements;
        }
    }

}
