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
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Phone.Net.NetworkInformation;

namespace Xupt_lib
{
    public partial class SearchResult : PhoneApplicationPage
    {
        string type = "title";
        string match = "mh";
        string record = "01";
        string lib = "all";

        static bool isLoaded = false;
        string keyword = "";
        int amount, pages, current;
        ObservableCollection<SearchList> SearchData = new ObservableCollection<SearchList>();
        public SearchResult()
        {
            InitializeComponent();
            isLoaded = false;
            ResultList.ItemsSource = SearchData;
        }

        private void GetParam()
        {
            try
            {
                type = this.NavigationContext.QueryString["type"];
            }
            catch
            {
                type = "title";
            }
            try
            {
                match = this.NavigationContext.QueryString["match"];
            }
            catch
            {
                match = "mh";
            }
            try
            {
                record = this.NavigationContext.QueryString["record"];
            }
            catch
            {
                record = "01";
            }
            try
            {
                lib = this.NavigationContext.QueryString["lib"];
            }
            catch
            {
                lib = "all";
            }
            try
            {
                string from = this.NavigationContext.QueryString["from"];
                this.NavigationService.RemoveBackEntry();
            }
            catch{}
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (DeviceNetworkInformation.IsNetworkAvailable == false)
            {
                new App.ToastTips("网络不可用");
                return;
            }
            if (isLoaded == true)
                return;
            GetParam();
            keyword = App.SearchParam;
            Keyword.Text = string.Format("搜索关键词：{0}", keyword);
            load.IsVisible = true;
            GetHttp(result =>
            {
                JObject data = JObject.Parse(result);
                amount = Convert.ToInt32((string)data["Amount"]);
                pages = Convert.ToInt32((string)data["Pages"]);
                current = Convert.ToInt32((string)data["CurrentPage"]);
                Amount.Text = string.Format("共检索到 {0} 条记录", amount);
                SearchData.Clear();
                foreach (var s in data["BookData"])
                {
                    SearchData.Add(new SearchList
                    {
                        ID = (string)s["ID"],
                        Name = (string)s["Name"],
                        Author = (string)s["Author"],
                        ISBN = (string)s["ISBN"],
                        Pub = (string)s["Pub"],
                        Year = (string)s["Year"],
                        BookItem = Visibility.Visible,
                        LoadingMore = Visibility.Collapsed
                    });
                }
                if (current + 1 > pages)
                {
                    SearchData.Add(new SearchList { LoadingMore = Visibility.Visible, BookItem = Visibility.Collapsed, ShowContent = "无更多内容", IsEnable = "False" });
                }
                else
                {
                    SearchData.Add(new SearchList { LoadingMore = Visibility.Visible, BookItem = Visibility.Collapsed, ShowContent = "加载更多...", IsEnable = "True" });
                }
                isLoaded = true;
            }, "1", "simple", type, match, record, lib, "20", "title", "asc");
        }

        public delegate void HandleResult(string result);
        private HandleResult handle;
        string realkey, param, responsestring;
        private void GetHttp(
            HandleResult handle,
            string cpage = "1",
            string kind = "simple",
            string type = "title",
            string match = "mh",
            string record = "01",
            string lib = "all",
            string size = "20",
            string orderby = "title",
            string ordersc = "asc"
            )
        {
            this.handle = handle;
            realkey = keyword;
            realkey = realkey.Replace("%", "%25");
            realkey = realkey.Replace("+", "%2b");
            realkey = realkey.Replace("/", "%2f");
            realkey = realkey.Replace("?", "%3f");
            realkey = realkey.Replace(" ", "%20");
            realkey = realkey.Replace("&", "%26");
            realkey = realkey.Replace("=", "%3d");
            realkey = realkey.Replace("#", "%23");

            param = string.Format(
                "kind=simple&type={0}&word={1}&match={2}&record={3}&lib={4}&x=0&y=0&page={5}&size={6}&orderby={7}&ordersc={8}",
                type, realkey, match, record, lib, cpage, size, orderby, ordersc);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://222.24.63.109/lib/");
            req.UserAgent = "XiyouLibrary_Windows_Phone_Client";
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.BeginGetRequestStream(new AsyncCallback(GetRequestStr), req);
        }
        private void GetRequestStr(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            byte[] postbin = Encoding.UTF8.GetBytes(param);
            Stream str = request.EndGetRequestStream(result);
            str.Write(postbin, 0, postbin.Length);
            str.Close();
            request.BeginGetResponse(new AsyncCallback(GetResponse), request);
        }
        private void GetResponse(IAsyncResult result)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse rp = (HttpWebResponse)request.EndGetResponse(result);
                Stream st = rp.GetResponseStream();
                StreamReader reader = new StreamReader(st);
                responsestring = reader.ReadToEnd();
                st.Close();
                reader.Close();
            }
            catch
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    new App.ToastTips("服务器错误");
                    load.IsVisible = false;
                    return;
                });
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (responsestring == "Server_Error")
                    {
                        new App.ToastTips("服务器错误");
                        Amount.Text = "共检索到 0 条记录";
                        load.IsVisible = false;
                        return;
                    }
                    if (responsestring == "null")
                    {
                        new App.ToastTips("无相关信息");
                        Amount.Text = "共检索到 0 条记录";
                        load.IsVisible = false;
                        return;
                    }
                    if (responsestring == null)
                    {
                        new App.ToastTips("服务器错误");
                        Amount.Text = "共检索到 0 条记录";
                        load.IsVisible = false;
                        return;
                    }
                    load.IsVisible = false;
                    handle(responsestring);
                });
        }

        private void LoadMoreTag_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (current + 1 > pages)
            {
                new App.ToastTips("无更多信息");
                return;
            }
            load.IsVisible = true;
            SearchData[SearchData.Count - 1].ShowContent = "正在加载...";
            SearchData[SearchData.Count - 1].IsEnable = "False";
            GetHttp(result =>
            {
                load.IsVisible = false;
                int cou = SearchData.Count;
                JObject data = JObject.Parse(result);
                pages = Convert.ToInt32((string)data["Pages"]);
                current = Convert.ToInt32((string)data["CurrentPage"]);
                for (int i = 0; i < data["BookData"].Count(); i++)
                {
                    SearchData.Insert(SearchData.Count - 1, new SearchList
                    {
                        ID = (string)data["BookData"][i]["ID"],
                        Name = (string)data["BookData"][i]["Name"],
                        Author = (string)data["BookData"][i]["Author"],
                        ISBN = (string)data["BookData"][i]["ISBN"],
                        Pub = (string)data["BookData"][i]["Pub"],
                        Year = (string)data["BookData"][i]["Year"],
                        BookItem = Visibility.Visible,
                        LoadingMore = Visibility.Collapsed
                    });
                }
                if (current + 1 > pages)
                {
                    SearchData[SearchData.Count - 1].ShowContent = "无更多内容";
                    SearchData[SearchData.Count - 1].IsEnable = "False";
                }
                else
                {
                    SearchData[SearchData.Count - 1].ShowContent = "加载更多...";
                    SearchData[SearchData.Count - 1].IsEnable = "True";
                }
                this.ResultList.SelectedIndex = this.ResultList.Items.Count - 1;
                this.ResultList.ScrollIntoView(this.ResultList.SelectedItem);
            }, (++current).ToString(), "simple", type, match, record, lib, "20", "title", "asc");
        }

        private void BookItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!DeviceNetworkInformation.IsNetworkAvailable)
            {
                new App.ToastTips("网络不可用");
                return;
            }
            var s = (StackPanel)sender;
            string uri = "/BookDetail.xaml?id=" + s.Tag.ToString();
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}