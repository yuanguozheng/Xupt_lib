using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Text;
using WeiboSdk.PageViews;
using WeiboSdk;
using SinaBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.IsolatedStorage;

namespace Xupt_lib
{
    public partial class WeiboPublisher : PhoneApplicationPage
    {
        public WeiboPublisher()
        {
            InitializeComponent();
        }
        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            /*SdkNetEngine WeiboSDK = new SdkNetEngine();
            
            cmdUploadMessage Param = new cmdUploadMessage();
            //string text = string.Format("我刚通过西邮图书管WP客户端分享了图书《{0}》({1})，方便、快捷，还有这本书的图片{2}", "C#权威指南", "TP123", "http://img3.douban.com/lpic/s5889267.jpg");
            Param.status = content.Text;
            Param.acessToken = App.WeiboToken;
            
            WeiboSDK.RequestCmd(SdkRequestType.UPLOAD_MESSAGE, Param, (e1, e2) =>
                {
                    if (e2.errCode != 0)
                    {
                        Dispatcher.BeginInvoke(() => { new App.ToastTips("分享失败"); });
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(() => { new App.ToastTips("分享成功"); });
                    }
                });
            //WeiboSDK.RequestCmd(SdkRequestType.UPLOAD_MESSAGE_PIC*/
            PubWeibo();
            this.NavigationService.GoBack();
        }
        void GetInfo()
        {
            string nickname;
            string url = "https://api.weibo.com/2/account/get_uid.json?access_token=" + App.WeiboToken;
            HttpReq(url, result =>
                {
                    if (result.Contains("uid"))
                    {
                        JObject data = JObject.Parse(result);
                        App.WeiboUid = (string)data["uid"];
                        string url1=string.Format("https://api.weibo.com/2/users/show.json?access_token={0}&uid={1}",App.WeiboToken,App.WeiboUid);
                        HttpReq(url1, result1 =>
                            {
                                JObject data1 = JObject.Parse(result1);
                                nickname = (string)data1["screen_name"];
                                Dispatcher.BeginInvoke(() =>
                                    {
                                        name.Text = nickname;
                                    });
                            });
                    }
                });
        }
        public delegate void HandleResult(string result);
        private HandleResult handle;
        void HttpReq(string url,HandleResult handle)
        {
            this.handle = handle;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.BeginGetResponse(new AsyncCallback(HttpResp), request);
        }
        void HttpResp(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
            Stream st = response.GetResponseStream();
            StreamReader reader = new StreamReader(st);
            string str = reader.ReadToEnd();
            Dispatcher.BeginInvoke(() => { handle(str); });
        }
        void PubWeibo()
        {
            SdkNetEngine WeiboSDK = new SdkNetEngine();
            cmdUploadMessage Param = new cmdUploadMessage();
            SdkCmdBase data = new SdkCmdBase
            {
                acessToken = App.WeiboToken
            };
            Param.status = content.Text;
            Param.acessToken = App.WeiboToken;
            Hammock.RestRequest req = new Hammock.RestRequest();
            req.Method = Hammock.Web.WebMethod.Post;
            req.Path = "/statuses/upload_url_text.json";
            req.AddParameter("status", content.Text);
            req.AddParameter("url", App.ImgUrl);
            if (!string.IsNullOrEmpty(App.ImgUrl))
            {
                WeiboSDK.SendRequest(req, data, callback =>
                {
                    if (callback.errCode == SdkErrCode.SUCCESS)
                    {
                        Dispatcher.BeginInvoke(() => { new App.ToastTips("分享成功"); });
                    }
                    else if (callback.specificCode == "21315" || callback.specificCode == "21314" || callback.specificCode == "21316" || callback.specificCode == "21317" || callback.specificCode == "21319")
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            new App.ToastTips("授权错误");
                            App.WeiboToken = null;
                            if (IsolatedStorageSettings.ApplicationSettings.Contains("WeiboToken"))
                                IsolatedStorageSettings.ApplicationSettings.Remove("WeiboToken");
                            this.NavigationService.GoBack();
                        });
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(() => { new App.ToastTips("分享失败"); });
                    }
                });
            }
            else
            {
                WeiboSDK.RequestCmd(SdkRequestType.UPLOAD_MESSAGE, Param, (e1, e2) =>
                {

                    if (e2.errCode != 0)
                    {
                        Dispatcher.BeginInvoke(() => { new App.ToastTips("分享失败"); });
                    }
                    else if (e2.specificCode == "21315" || e2.specificCode == "21314" || e2.specificCode == "21316" || e2.specificCode == "21317" || e2.specificCode == "21319")
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            new App.ToastTips("授权错误");
                            App.WeiboToken = null;
                            if (IsolatedStorageSettings.ApplicationSettings.Contains("WeiboToken"))
                                IsolatedStorageSettings.ApplicationSettings.Remove("WeiboToken");
                            this.NavigationService.GoBack();
                        });
                    }

                    else
                    {
                        Dispatcher.BeginInvoke(() => { new App.ToastTips("分享成功"); });
                    }
                });
            }
        }

        

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            content.Text = App.WeiboShareContent;
            GetInfo();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确实要注销吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            App.WeiboToken = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("WeiboToken"))
                IsolatedStorageSettings.ApplicationSettings.Remove("WeiboToken");
            this.NavigationService.GoBack();
        }
    }
}