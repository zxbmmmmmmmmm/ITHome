using System;
using System.Collections.Generic;
using System.Text;

namespace ITHome.Core.Models.Post
{
    public class CommentClientElement
    {
        public object alt { get; set; }
        public bool animation { get; set; }
        public int atUserId { get; set; }
        public string content { get; set; }
        public int height { get; set; }
        public int length { get; set; }
        public object link { get; set; }
        public object originSrc { get; set; }
        public object src { get; set; }
        public object thumbSrc { get; set; }
        public int topicId { get; set; }
        public int type { get; set; }
        public object videoHeader { get; set; }
        public int width { get; set; }
    }

    public class CommentClient
    {
        public object city { get; set; }
        public int client { get; set; }
        public object color { get; set; }
        public object commentContent { get; set; }
        public object commentNick { get; set; }
        public string device { get; set; }
        public IList<CommentClientElement> elements { get; set; }
        public object ip { get; set; }
        public int newsId { get; set; }
        public object noCity { get; set; }
        public object noTail { get; set; }
        public object paragraphId { get; set; }
        public int parentCommentId { get; set; }
        public int ppCId { get; set; }
        public object referText { get; set; }
        public int replyCommentId { get; set; }
        public object userHash { get; set; }
        public int ver { get; set; }
    }
}
