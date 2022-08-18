﻿using ITHome.Core.Helpers;
using ITHome.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ITHome.Helpers
{
    public class ITHomeProxy
    {
        public async static Task<NewsList> GetNewsList(string ot)
        {
            var jObj = await NetworkHelper.GetAsync($"https://m.ithome.com/api/news/newslistpageget?page=0&ot={ot}", null);
            var newsList = new NewsList { News = new ObservableCollection<News>()};
            foreach(var news in jObj["Result"])
            {
                newsList.News.Add(News.CreateFromJson(news));
            }
            return newsList;
        }
        public async static Task<NewsSlideList> GetNewsSlideList()
        {
            var jObj = await NetworkHelper.GetArrayAsync($"https://api.ithome.com/json/slide/index", null);
            var newsSlideList = NewsSlideList.CreateFromJson(jObj);
            return newsSlideList;
        }
        /*public async static Task<NewsList> GetNewsList(string ot)
        {
            var jObj = await NetworkHelper.GetAsync("https://api.ithome.com/json/newslist/news", null);
            var newsList = new NewsList { News = new ObservableCollection<News>() };
            foreach (var news in jObj["newslist"])
            {
                newsList.News.Add(News.CreateFromJson(news));
            }
            return newsList;
        }*/
        public async static Task<NewsSearch> GetNewsSearch(string id)
        {
            var jObj = await NetworkHelper.GetAsync($"https://api.ithome.com/json/newssearch/{id}", null);
            var newsSearch = NewsSearch.CreateFromJson(jObj);

            return newsSearch;
        }
        public async static Task<CommentsList> GetCommentsList(string newsId)
        {
            var sn = GetCommentSn(newsId);
            var jObj = await NetworkHelper.GetAsync($"https://cmt.ithome.com/apiv2/comment/getnewscomment?sn={sn}", null);
            var commentsList = CommentsList.CreateFromJson(jObj);
            return commentsList;
        }
        public async static Task<Comment> GetCommentContent(string commentId)
        {
            var id = GetCommentContentId(commentId);
            var jObj = await NetworkHelper.GetAsync($"https://cmt.ithome.com/apiv2/comment/getcommentcontent?commentid={id}", null);
            var content = Comment.CreateFromJson(jObj["content"]["comment"]);
            return content;
        }
        public static string GetCommentContentId(string id)
        {
            string sn = string.Empty;
            var length = id.Length;
            var times = 8 - length;
            if (length >= 8)
            {
                length %= 8;
                if (length == 0)
                    times = 0;
            }
            var encrypted = Encrypt(id, "(#i@x*l%");
            var hex = BitConverter.ToString(encrypted, 0).Replace("-", string.Empty).ToLower();

            return hex;
        }
        public static string GetCommentSn(string id)
        {
            string sn = string.Empty;
            var length = id.Length;
            var times = 8 - length;
            if (length >= 8)
            {
                length %= 8;
                if (length == 0)
                    times = 0;
            }
            string sb = id;
            for (int i = 0; i < times; i++)
                sb = sb + Convert.ToChar(0);

            byte[] data = Encoding.Unicode.GetBytes(sb);
            byte[] key = Encoding.Unicode.GetBytes("(#i@x*l%");

            //sn = Encrypt(sb, "(#i@x*l%");
            //var cnm = Decrypt_DES16(data, key);  
            var encrypted = Encrypt(sb, "(#i@x*l%");
            var hex = BitConverter.ToString(encrypted, 0).Replace("-", string.Empty).ToLower();

            return hex;
        }
        //DES加密 ECB 
        public static byte[] Encrypt(string encryptString, string sKey)
        {
            try
            {
                string key;
                //密钥为8位
                if (sKey.Length <= 8)
                {
                    key = sKey.PadRight(8, '0');
                }
                else
                {
                    key = sKey.Substring(0, 8);
                }

                byte[] keyBytes = Encoding.Default.GetBytes(key);
                byte[] keyIV = keyBytes;
                byte[] encryptBytes = Encoding.Default.GetBytes(encryptString);

                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                // 使用ECB方式
                desProvider.Mode = CipherMode.ECB;
                // PKCS7padding
                desProvider.Padding = PaddingMode.None;
                MemoryStream memStream = new MemoryStream();
                //CreateEncryptor(keyBytes, keyIV)类似于OpenSSL中的密钥置换
                CryptoStream crypStream = new CryptoStream(memStream, desProvider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                crypStream.Write(encryptBytes, 0, encryptBytes.Length);
                crypStream.FlushFinalBlock();
                byte[] cipherBytes = memStream.ToArray();

                return cipherBytes;
            }
            catch
            {
                return Encoding.Default.GetBytes(encryptString);
            }
        }
    }
}
