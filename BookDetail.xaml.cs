using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Net.NetworkInformation;
using SinaBase;
using WeiboSdk;
using WeiboSdk.PageViews;
using System.IO.IsolatedStorage;
using Hammock;
using Hammock.Web;
using RenrenSDKLibrary;

namespace Xupt_lib
{
    public partial class BookDetail : PhoneApplicationPage
    {
        ObservableCollection<BookCirculation> CirculationData = new ObservableCollection<BookCirculation>();
        public BookDetail()
        {
            InitializeComponent();
        }
        public delegate void HandleResult(string result);
        private HandleResult handle;
        string id = null, barcode = null;
        private void GetHttp(HandleResult handle)
        {
            this.handle = handle;
            HttpWebRequest request;            
            if (barcode != null && id == null)
                request = (HttpWebRequest)HttpWebRequest.Create("http://222.24.63.109/lib/detail.aspx?detail=true&barcode=" + barcode);
            else
                request = (HttpWebRequest)HttpWebRequest.Create("http://222.24.63.109/lib/detail.aspx?detail=true&id=" + id);
            request.UserAgent = "XiyouLibrary_Windows_Phone_Client";
            request.BeginGetResponse(new AsyncCallback(GetResponse), request);
        }
        private void GetResponse(IAsyncResult result)
        {
            string responstring = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                responstring = reader.ReadToEnd();
            }
            catch
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { handle("error"); });
            }
            Deployment.Current.Dispatcher.BeginInvoke(() => { handle(responstring); });
        }
        SearchDetail detail;
        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (DeviceNetworkInformation.IsNetworkAvailable == false)
            {
                new App.ToastTips("网络不可用");
                return;
            }
            try
            {
                this.id = NavigationContext.QueryString["id"];

            }
            catch
            {
                try
                {
                    this.barcode = NavigationContext.QueryString["barcode"];
                }
                catch
                {
                    if (MessageBox.Show("参数丢失！", "错误", MessageBoxButton.OK) == MessageBoxResult.OK)
                        this.NavigationService.GoBack();
                    else
                        return;
                }
            }
            load.IsVisible = true;
            GetHttp(result => {
                load.IsVisible = false;
                if (result == "error")
                {
                    new App.ToastTips("服务器错误");
                    return;
                }
                if (result == "null")
                {
                    new App.ToastTips("图书信息不存在");
                    return;
                }
                try
                {
                    JObject data = JObject.Parse(result);
                    detail = new SearchDetail
                    {
                        CtrlID = (string)data["CtrlID"],
                        ID = (string)data["ID"],
                        Title = (string)data["Title"],
                        Author = (string)data["Author"],
                        ISBN = (string)data["ISBN"],
                        Pub = (string)data["Pub"],
                        image_l = (string)data["image_l"],
                        image_m = (string)data["image_m"],
                        image_s = (string)data["image_s"],
                        Pages = (string)data["Pages"],
                        Price = (string)data["Price"],
                        Summary = (string)data["Summary"],
                        Link = (string)data["Link"]
                    };
                    CirculationData.Clear();
                    foreach (var s in data["Info"])
                    {
                        CirculationData.Add(new BookCirculation
                        {
                            Barcode = (string)s["Barcode"],
                            Department = (string)s["Department"],
                            State = (string)s["State"],
                            Date = (string)s["Date"]
                        });
                    }
                }
                catch
                {
                    new App.ToastTips("数据解析错误");
                    return;
                }
                Circulation.ItemsSource = CirculationData;
                BookName.Text = detail.Title;
                BookAuthor.Text = detail.Author;
                BookPub.Text = detail.Pub;
                if (detail.Price != "" && detail.Price != null)
                    BookPrice.Text = detail.Price;
                else
                    BookPrice.Text = "暂无";
                if (detail.Pages != "" && detail.Pages != null)
                    BookPages.Text = detail.Pages;
                else
                    BookPages.Text = "暂无";
                if (detail.ISBN != "" && detail.ISBN != null)
                    BookISBN.Text = detail.ISBN;
                else
                    BookISBN.Text = "暂无";
                loadingImg.Visibility = Visibility.Visible;
               
                if (detail.image_m == "http://img3.douban.com/pics/book-default-medium.gif")
                {
                    Icon_m.Source = new BitmapImage(new Uri("/book.png", UriKind.Relative));
                    loadingImg.Visibility = Visibility.Collapsed;
                }
                else if (detail.image_m != "" && detail.image_m!=null)
                {
                    Icon_m.Source = new BitmapImage(new Uri(detail.image_m, UriKind.Absolute));
                    Icon_m.ImageOpened += Icon_m_ImageOpened;
                }
                else
                {
                    Icon_m.Source = new BitmapImage(new Uri("/book.png", UriKind.Relative));
                    loadingImg.Visibility = Visibility.Collapsed;
                }
                BookId.Text = detail.ID;
                BookSummary.Text = detail.Summary;
                if (detail.Summary == "" || detail.Summary == null || detail.Summary.Replace("\n","").Trim()=="")
                {
                    BookSummary.Text = "暂无此图书摘要";
                }
                LoadLarImg.Visibility = Visibility.Visible;
                LarImg.ImageOpened += LarImg_ImageOpened;
                if (detail.image_l != "http://img3.douban.com/pics/book-default-large.gif" && (detail.image_l != "" && detail.image_l != null))
                    LarImg.Source = new BitmapImage(new Uri(detail.image_l, UriKind.Absolute));
                else 
                {
                    LoadLarImg.Visibility = Visibility.Collapsed;
                    LarImg.Source = Icon_m.Source = new BitmapImage(new Uri("/book.png", UriKind.Relative));
                }
            });
        }

        void LarImg_ImageOpened(object sender, RoutedEventArgs e)
        {
            LoadLarImg.Visibility = Visibility.Collapsed;
        }

        void Icon_m_ImageOpened(object sender, RoutedEventArgs e)
        {
            loadingImg.Visibility = Visibility.Collapsed;
        }

        private void Weibo_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            SdkData.AppKey = "1107543272";
            SdkData.AppSecret = "e0d0b7747bb6433afbeebdd40e633a88";
            SdkData.RedirectUri = "https://api.weibo.com/oauth2/default.html";
            if (IsolatedStorageSettings.ApplicationSettings.Contains("WeiboToken"))
                App.WeiboToken = Convert.ToString(IsolatedStorageSettings.ApplicationSettings["WeiboToken"]);

            if (App.WeiboToken == "" || App.WeiboToken == null)
            {
                AuthenticationView.OAuth2VerifyCompleted = (e1, e2, e3) => VerifyBack(e1, e2, e3);
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/WeiboSdk;component/PageViews/AuthenticationView.xaml", UriKind.Relative));
                });
            }
            else
            {
                ShareWeibo();
            }
            /*else
            {
                /*ClientOAuth.RefleshAccessToken(App.WeiboToken, (e1, e2, e3) =>
                {
                    if (true == e1)
                    {
                        //Debug.WriteLine("accessToken:" + e3.accesssToken);

                        //Debug.WriteLine("refleshToken:" + e3.refleshToken);

                        //Debug.WriteLine("expriesIn:" + e3.expriesIn);
                    }
                    else
                    {
                        if (e2.errCode == SdkErrCode.NET_UNUSUAL)
                        {

                        }
                        else if (e2.errCode == SdkErrCode.SERVER_ERR)
                        {
                        }
                    }
                });
                GetTokenHttp(result =>
                    {
                        if (result == "Server_Err")
                        {
                            AuthenticationView.OAuth2VerifyCompleted = (e1, e2, e3) => VerifyBack(e1, e2, e3);
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                new App.ToastTips("登陆已过期");
                                NavigationService.Navigate(new Uri("/WeiboSdk;component/PageViews/AuthenticationView.xaml", UriKind.Relative));
                            });
                            return;
                        }
                        if(result.Contains("expires_in"))
                        {
                            JObject data = JObject.Parse(result);
                            if ((int)data["expires_in"] > 90)
                            {
                                ShareWeibo();
                            }
                            else
                            {
                                AuthenticationView.OAuth2VerifyCompleted = (e1, e2, e3) => VerifyBack(e1, e2, e3);
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    new App.ToastTips("登陆已过期");
                                    NavigationService.Navigate(new Uri("/WeiboSdk;component/PageViews/AuthenticationView.xaml", UriKind.Relative));
                                });
                                return;
                            }
                        }
                    });
                 
            }*/
        }
        private void VerifyBack(bool isSucess, SdkAuthError errCode, SdkAuth2Res response)
        {
            if (errCode.errCode == SdkErrCode.SUCCESS)
            {
                if (null != response)
                {
                    App.WeiboToken = response.accesssToken;
                    IsolatedStorageSettings.ApplicationSettings["WeiboToken"] = App.WeiboToken;
                }
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //NavigationService.Navigate(new Uri("/WeiboPublisher.xaml", UriKind.Relative));
                    NavigationService.GoBack();
                    ShareWeibo();
                });
            }
            #region
            /*else if (errCode.errCode == SdkErrCode.NET_UNUSUAL)
            {

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    MessageBox.Show("检查网络");

                });

            }

            else if (errCode.errCode == SdkErrCode.SERVER_ERR)
            {

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    MessageBox.Show("服务器返回错误，错误代码:" + errCode.specificCode);

                });

            }*/
            #endregion
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    new App.ToastTips("授权失败");
                    return;
                });
            }
        }
        void ShareWeibo()
        {
            if (detail == null)
            {
                return;
            }
            string text = "";
            if (detail.image_l != "http://img3.douban.com/pics/book-default-large.gif" && (detail.image_l != "" && detail.image_l != null))
            {
                text = string.Format("我刚刚通过#西邮图书馆WP客户端#分享了图书《{0}》（索书号：{1}），还有它的图片{2}，可续借、查询图书，方便、快捷，非常不错！[good]", BookName.Text, BookId.Text, detail.image_l);
                App.ImgUrl = detail.image_l;
            }
            else
                text = string.Format("我刚刚通过#西邮图书馆WP客户端#分享了图书《{0}》（索书号：{1}），可续借、查询图书，方便、快捷，非常不错！[good]", BookName.Text, BookId.Text);
            App.WeiboShareContent = text;
            this.NavigationService.Navigate(new Uri("/WeiboPublisher.xaml", UriKind.Relative));
        }
        
        //public delegate void HandleResult(string result);
        //private HandleResult handle;
         
        void GetTokenHttp(HandleResult handle)
        {
            this.handle = handle;
            string uri = "https://api.weibo.com/oauth2/get_token_info";
            WebRequest request = WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream(new AsyncCallback(UploadParam), request);
        }
        void UploadParam(IAsyncResult result)
        {
            string param = "access_token=" + App.WeiboToken;
            byte[] postBin = System.Text.Encoding.UTF8.GetBytes(param);
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            Stream st = request.EndGetRequestStream(result);
            st.Write(postBin, 0, postBin.Length);
            request.BeginGetResponse(new AsyncCallback(GetInfo), request);
            //st.Close();
        }
        void GetInfo(IAsyncResult result)
        {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st);
                string str = reader.ReadToEnd();
                reader.Close();
                st.Close();
                handle(str);
           
                //handle("Server_Err");
            
        }
        RenrenAPI api = App.api;
        void RenrenLogin()
        {
            List<string> scope = new List<string> { "status_update" };
            api.Login(this, scope, renren_LoginCompletedHandler);
        }
        public void renren_LoginCompletedHandler(object sender, LoginCompletedEventArgs e)
        {
            if (e.Error == null)
                NavigationService.Navigate(new Uri("/RenrenPublisher.xaml", UriKind.Relative));
            else
                MessageBox.Show(e.Error.Message);
        }

        private void Renren_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (detail == null)
            {
                return;
            }
            string text = "";
            if (detail.image_l != "http://img3.douban.com/pics/book-default-large.gif" && (detail.image_l != "" && detail.image_l != null))
            {
                text = string.Format("我刚刚通过“西邮图书馆WP客户端”分享了图书《{0}》（索书号：{1}），还有它的图片{2}，可续借、查询图书，方便、快捷，非常不错！(good)", BookName.Text, BookId.Text, detail.image_l);
                App.ImgUrl = detail.image_l;
            }
            else
            {
                text = string.Format("我刚刚通过“西邮图书馆WP客户端”分享了图书《{0}》（索书号：{1}），可续借、查询图书，方便、快捷，非常不错！(good)", BookName.Text, BookId.Text);
                App.ImgUrl = "";
            }
            App.RenrenShareContent = text;
            App.DoubanLink = detail.Link;
            if(!api.IsAccessTokenValid())
                RenrenLogin();
            else
                this.NavigationService.Navigate(new Uri("/RenrenPublisher.xaml", UriKind.Relative));
        }

        private void AddFav_Click(object sender, EventArgs e)
        {
            bool isLogined = false;
            try
            {
                isLogined = (bool)IsolatedStorageSettings.ApplicationSettings["Remember"];
            }
            catch
            {
                string uri = string.Format("/LoginPage.xaml?book={0}", detail.CtrlID);
                this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }
            if (isLogined == true)
            {
                string id = null, password = null;
                try
                {
                    id = IsolatedStorageSettings.ApplicationSettings["UserName"].ToString();
                    password = IsolatedStorageSettings.ApplicationSettings["Password"].ToString();
                }
                catch
                {
                    string uri = string.Format("/LoginPage.xaml?book={0}", NavigationContext.QueryString["id"]);
                    this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
                }
                if (!(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password)))
                {
                    if (MessageBox.Show("确实要收藏到账号为 " + id + " 的书架中吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        load.IsVisible = true;
                        UniRequest api = new UniRequest("http://xiyoumobile.com/lib/AddFav.aspx", "POST");
                        api.AddParams("id", id);
                        api.AddParams("password", password);
                        api.AddParams("book", detail.CtrlID);
                        api.StartRequest(result =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                    {
                                        load.IsVisible = false;
                                        if (result.Contains("Successed"))
                                        {
                                            new App.ToastTips("收藏成功");
                                            return;
                                        }
                                        else if (result.Contains("Failed"))
                                        {
                                            new App.ToastTips("收藏失败");
                                            return;
                                        }
                                        else if (result.Contains("Already"))
                                        {
                                            new App.ToastTips("此书已收藏");
                                            return;
                                        }
                                        else
                                        {
                                            new App.ToastTips("未知错误");
                                            return;
                                        }
                                    });
                            });
                    }
                }
            }
        }
        
    }
}