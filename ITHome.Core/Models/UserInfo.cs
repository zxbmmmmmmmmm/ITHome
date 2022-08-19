using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITHome.Core.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Mobile { get; set; }
        public int Coin { get; set; }
        public int Conldays { get; set; }
        public static UserInfo CreateFromJson(JToken token)
        {
            var userInfo = new UserInfo
            {
                Id = token.Value<int>("userid"),
                UserName = token.Value<string>("username"),
                NickName = token.Value<string>("nickname"),
                Mobile = token.Value<string>("mobile"),
                Coin = token.Value<int>("coin"),
                Conldays = token.Value<int>("conldays"),
            };
            return userInfo;
        }
        public static UserInfo NotLoggedUser()
        {
            var user = new UserInfo
            {
                Id = -1,
                UserName = "未登录",
                NickName = "未登录",
            };
            return user;
        }
    }
}
