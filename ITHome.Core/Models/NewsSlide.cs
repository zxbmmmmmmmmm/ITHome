using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ITHome.Core.Models
{
    public class NewsSlideList
    {
        public ObservableCollection<NewsSlide> News { get; set; }
        public static NewsSlideList CreateFromJson(JToken token)
        {
            var newsSlideList = new NewsSlideList();
            newsSlideList.News = new ObservableCollection<NewsSlide>();
            foreach (var news in token)
                newsSlideList.News.Add(NewsSlide.CreateFromJson(news));
            return newsSlideList;
        }
    }

    public class NewsSlide
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public bool? IsAd { get; set; }
        public static NewsSlide CreateFromJson(JToken token)
        {
            var newsSlide = new NewsSlide
            {
                Title = token.Value<string>("title"),
                Link = token.Value<string>("link"),
                Image = token.Value<string>("image"),
                IsAd = token.Value<bool?>("isad"),

            };
            return newsSlide;
        }
    }
}
