using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ITHome.Core.Models
{
    public class NewsList
    {
        [JsonProperty("newslist")]
        public  ObservableCollection<News> News { get; set; }
        public static NewsList CreateFromJson(JToken token)
        {
            var newsList = new NewsList();
            return newsList;
        }
    }
    public class News
    {
        [JsonProperty("newsid")]
        public int Id { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("commentcount")]
        public int CommentCount { get; set; }
        [JsonProperty("postdate")]
        public DateTime PostDate { get; set; }
        public List<string> ImageList { get; set; }
        public static News CreateFromJson(JToken token)
        {
            var news = new News
            {
                Id = token.Value<int>("newsid"),
                Url = token.Value<string>("url"),
                Title = token.Value<string>("title"),
                Image = token.Value<string>("image"),
                CommentCount = token.Value<int>("commentcount"),
                PostDate = token.Value<DateTime>("postdate"),
            };
            if (token["imagelist"]!= null)
            {
                news.ImageList = new List<string>();
                foreach (var image in token["imagelist"])
                    news.ImageList.Add(image.ToString());
            }
            return news;
        }
    }
    public class NewsSearch
    {
        public string Detail { get; set; }
        public string Author { get; set; }
        public string Editer { get; set; }
        public List<NewsTag> Tags { get; set; }
        public static NewsSearch CreateFromJson(JToken token)
        {
            var newssearch = new NewsSearch
            {
                Detail = token.Value<string>("detail"),
                Author = token.Value<string>("newsauthor"),
                Editer = token.Value<string>("z"),
            };
            if (token["newstags"] != null)
            {
                newssearch.Tags = new List<NewsTag>();
                foreach (var tag in token["newstags"])
                    newssearch.Tags.Add(NewsTag.CreateFromJson(tag));
            }
            return newssearch;
        }
    }
    public class NewsTag
    { 
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Link { get; set; }
        public static NewsTag CreateFromJson(JToken token)
        {
            var newstag = new NewsTag
            {
                Id = token.Value<int>("id"),
                Keyword = token.Value<string>("keyword"),
                Link = token.Value<string>("link"),
            };
            return newstag;
        }
    }
    
}
