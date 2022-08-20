using ITHome.Core.Helpers;
using ITHome.Core.Models;
using Newtonsoft.Json;
using ReverseMarkdown;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;

namespace ITHome.Helpers
{
    internal static class Common
    {
        public static Settings Settings = new Settings();
    }

    public class Settings : INotifyPropertyChanged
    {
        /// <summary>
        /// 用于身份验证
        /// </summary>
        public string UserHash
        {
            get => GetSettings("UserHash", "");
            set
            {
                ApplicationData.Current.LocalSettings.Values["UserHash"] = value;
                OnPropertyChanged();
            }
        }
        public string Platform
        {
            get => GetSettings("Platform", "uwp");
            set
            {
                ApplicationData.Current.LocalSettings.Values["Platform"] = value;
                OnPropertyChanged();
            }
        }
        public int Client
        {
            get => GetSettings("Client", 7);
            set
            {
                ApplicationData.Current.LocalSettings.Values["Client"] = value;
                OnPropertyChanged();
            }
        }
        public string Model
        {
            get => GetSettings("Model", "");
            set
            {
                ApplicationData.Current.LocalSettings.Values["Model"] = value;
                OnPropertyChanged();
            }
        }
        public Product Product
        {
            get => GetSettingsWithClass("Product", Product.UnknownProduct());
            set
            {
                ApplicationData.Current.LocalSettings.Values["Product"] = JsonHelper.GetJsonByObject(value);
                OnPropertyChanged();
            }
        }
        public UserInfo LoggedUser
        {
            get => GetSettingsWithClass("LoggedUser", UserInfo.NotLoggedUser());
            set
            {
                ApplicationData.Current.LocalSettings.Values["LoggedUser"] = JsonHelper.GetJsonByObject(value);
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public async void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); });
        }
        /// <summary>
        /// 获取设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetSettings<T>(string propertyName, T defaultValue)
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(propertyName) &&
                    ApplicationData.Current.LocalSettings.Values[propertyName] != null &&
                    !string.IsNullOrEmpty(ApplicationData.Current.LocalSettings.Values[propertyName].ToString()))
                {
                    if (typeof(T).ToString() == "System.Boolean")
                        return (T)(object)bool.Parse(ApplicationData.Current.LocalSettings.Values[propertyName]
                            .ToString());

                    return (T)ApplicationData.Current.LocalSettings.Values[propertyName];
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values[propertyName] = defaultValue;
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// 将设置转换为Json后保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetSettingsWithClass<T>(string propertyName, T defaultValue)//使用default value中的T
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(propertyName) &&
                    ApplicationData.Current.LocalSettings.Values[propertyName] != null &&
                    !string.IsNullOrEmpty(ApplicationData.Current.LocalSettings.Values[propertyName].ToString()))
                {
                    if (typeof(T).ToString() == "System.Boolean")
                        return (T)(object)bool.Parse(ApplicationData.Current.LocalSettings.Values[propertyName]
                            .ToString());
                    var str = (string)ApplicationData.Current.LocalSettings.Values[propertyName];//获取字符串
                    return (T)JsonHelper.GetObjectByJson<T>(str);
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values[propertyName] = JsonHelper.GetJsonByObject(defaultValue);
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
   
}
