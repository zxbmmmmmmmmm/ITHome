using ITHome.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ITHome.Core.Helpers
{
    public class NetworkHelper
    {
        public static async Task<JObject> GetAsync(string link, string token, string model = null)
        {
            var client = new HttpClient();
            if (model != null)
                client.DefaultRequestHeaders.Add("x-model", model);
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:80.0) Gecko/20100101 Firefox/80.0");
            client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            client.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");
            client.DefaultRequestHeaders.Add("userhash",  token);
            var response = await client.GetAsync(new Uri(link));
            var result = await response.Content.ReadAsStringAsync();
            //result = TextEncoding.GetUtf8RemovedBomString(result);
            return JObject.Parse(result);
        }
        public static async Task<JArray> GetArrayAsync(string link, string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:80.0) Gecko/20100101 Firefox/80.0");
            client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            client.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");
            client.DefaultRequestHeaders.Add("userhash", token);
            var response = await client.GetAsync(new Uri(link));
            var result = await response.Content.ReadAsStringAsync();
            result = TextEncoding.GetUtf8RemovedBomString(result);
            return JArray.Parse(result);
        }
        public static async Task<JObject> PostWithJsonAsync(string link, string json, string token, string model = null)
        {
            var client = new HttpClient();
            if (model != null)
                client.DefaultRequestHeaders.Add("x-model", model);

            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:80.0) Gecko/20100101 Firefox/80.0");//模拟浏览器
            client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            client.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");
            client.DefaultRequestHeaders.Add("userhash", token);
            var response = await client.PostAsync(link, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            return JObject.Parse(result);
        }
    }
}
