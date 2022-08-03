using ITHome.Core.Helpers;
using ITHome.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHome.Helpers
{
    public class ITHomeProxy
    {
        public async static Task<NewsList> GetNewsList()
        {
            var jObj = await NetworkHelper.GetAsync("https://api.ithome.com/json/newslist/news", null);
            var newsList = new NewsList { News = new ObservableCollection<News>()};
            foreach(var news in jObj["newslist"])
            {
                newsList.News.Add(News.CreateFromJson(news));
            }
            return newsList;
        }
        public async static Task<NewsSearch> GetNewsSearch(string url)
        {
            var jObj = await NetworkHelper.GetAsync($"https://api.ithome.com/json/newssearch/{url}", null);
            var newsSearch = NewsSearch.CreateFromJson(jObj);

            return newsSearch;
        }
    }
}
