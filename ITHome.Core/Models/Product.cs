using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITHome.Core.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public int Score { get; set; }
        public string ScoreStr { get; set; }
        public int HotCount { get; set; }

        public static Product CreateFromJson(JToken token)
        {
            var product = new Product
            {
                Name = token.Value<string>("name"),
                Picture = token.Value<string>("picture"),
                Score = token.Value<int>("score"),
                ScoreStr = token.Value<string>("scoreStr"),
                HotCount = token.Value<int>("hotCount")
            };
            return product;
        }
        public static Product UnknownProduct()
        {
            var product = new Product
            {
                Name = "未知设备",
            };
            return product;
        }
    }
}
