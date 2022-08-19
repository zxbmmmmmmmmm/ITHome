using ITHome.Helpers;
using ITHome.Views.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace ITHome.Views.Dialogs
{
    public sealed partial class LoginDialog : ContentDialog
    {
        public LoginDialog()
        {
            this.InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void LoginWebview_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
        
        }

        private async void LoginWebview2_NavigationCompleted(Microsoft.UI.Xaml.Controls.WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {

            IReadOnlyList<CoreWebView2Cookie> cookieList = await sender.CoreWebView2.CookieManager.GetCookiesAsync("https://my.ruanmei.com/Default.aspx/LoginUser");
            StringBuilder cookieResult = new StringBuilder(cookieList.Count + " cookie(s) received from https://www.bing.com\n");
            List<CoreWebView2Cookie> result = new List<CoreWebView2Cookie>();
            for (int i = 0; i < cookieList.Count; ++i)
            {
                result.Add(cookieList[i]);
                var name = cookieList[i].Name;
                Debug.WriteLine(name + ":" + cookieList[i].Value);
                if (name == "user")
                {
                    Common.Settings.UserHash = cookieList[i].Value.Replace("hash=","");
                    new Toast("已登录").Show();
                    Hide();
                    return;
                }
            }
        }
    }
}
