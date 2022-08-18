using ITHome.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITHome.Core.Helpers
{
    public class NetworkHelper
    {
        public static async Task<JObject> GetAsync(string link, string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:80.0) Gecko/20100101 Firefox/80.0");
            client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            client.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");
            //client.DefaultRequestHeaders.Add("Authorization", "Token " + token);
            var response = await client.GetAsync(new Uri(link));
            var result = await response.Content.ReadAsStringAsync();
            result = TextEncoding.GetUtf8RemovedBomString(result);
            return JObject.Parse(result);
        }
        public static async Task<JArray> GetArrayAsync(string link, string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:80.0) Gecko/20100101 Firefox/80.0");
            client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            client.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");
            //client.DefaultRequestHeaders.Add("Authorization", "Token " + token);
            var response = await client.GetAsync(new Uri(link));
            var result = await response.Content.ReadAsStringAsync();
            result = TextEncoding.GetUtf8RemovedBomString(result);
            return JArray.Parse(result);
        }
    }
}
