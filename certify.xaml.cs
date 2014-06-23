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
using System.Windows.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using Microsoft.Phone.Net.NetworkInformation;

namespace Xupt_lib
{
    public partial class certify : PhoneApplicationPage
    {
        ObservableCollection<BookInfo> BookData = new ObservableCollection<BookInfo>();
        bool isloaded = false;
        public certify()
        {
            InitializeComponent();
            books.ItemsSource = BookData;
            FavData.ItemsSource = FavInfo;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DeviceNetworkInformation.IsNetworkAvailable == false)
            {
                new App.ToastTips("网络不可用");
                return;
            }
            if (isloaded == true)
                return;
            string user = NavigationContext.QueryString["user"];
            if (user == "false")
            {
                textBlock3.Text = "同学你好！";
            }
            else
            {
                textBlock3.Text = string.Format("欢迎 {0} {1} 同学", App.User.Cls, App.User.Name);
            }
            string resultStr = (App.Current as App).info;

            JArray data = JArray.Parse(resultStr);
            isloaded = true;
            if (data.Count == 0)
            {
                new App.ToastTips("暂时没有任何借阅信息");
                //toptips.Begin();
                textBlock3.Visibility = Visibility.Visible;
                textBlock4.Text = "你暂时没有任何图书借阅信息";
                textBlock4.Visibility = Visibility.Visible;
                return;
            }
            for (int i = 0; i < data.Count - 1; i++)
            {
                BookData.Add(new BookInfo
                {
                    id = ((string)data[i]["id"]).Replace(" ",""),
                    name = ((string)data[i]["name"]).Replace("\\u003d", "="),
                    barcode = (string)data[i]["barcode"],
                    library_id = (string)data[i]["library_id"],
                    department_id = (string)data[i]["department_id"],
                    isRenew = (string)data[i]["isRenew"],
                    state = (string)data[i]["state"],
                    date = (string)data[i]["date"],
                    detailvis=Visibility.Collapsed
                });
            }
            show.Begin();
            textBlock3.Visibility = Visibility.Visible;
            //textBlock4.Visibility = Visibility.Visible;
        }
        
        bool buttonc = false;
        int tmpid = -1;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            buttonc = true;
            var s = (Button)sender;
            if (((RenewInfoClass)s.Tag).Enable == "False" || ((RenewInfoClass)s.Tag).Duration < 0)
            {
                new App.ToastTips("此书无法续借");
                return;
            }
            if (((RenewInfoClass)s.Tag).Duration > 5)
            {
                if (MessageBox.Show("距到期超过5天，确实需要续借吗？", "续借提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
            }
            else
            {
                if (MessageBox.Show("确实需要续借吗？", "续借提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
            }
            tmpid = Convert.ToInt32(((RenewInfoClass)s.Tag).id.TrimEnd());
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://222.24.63.101/library/renew?barcode={0}&department_id={1}&library_id={2}", ((RenewInfoClass)s.Tag).barcode, ((RenewInfoClass)s.Tag).depart, ((RenewInfoClass)s.Tag).lib));
            req.UserAgent = "XiyouLibrary_Windows_Phone_Client";
            req.BeginGetResponse(new AsyncCallback(GetRenewResponse), req);
        }
        private void GetRenewResponse(IAsyncResult result)
        {
            string responsestring = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                using (StreamReader streamread = new StreamReader(streamResponse))
                {
                    responsestring = streamread.ReadToEnd();
                }
                streamResponse.Close();
            }
            catch
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { new App.ToastTips("网络错误"); });
                return;
            }
            if (responsestring == "ok")
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    new App.ToastTips("续借成功");
                    if (tmpid != -1)
                    {
                        string[] tmp = Regex.Split(BookData[tmpid - 1].date, "/");
                        DateTime t = DateTime.Now.Date;
                        t = t + TimeSpan.FromDays(15);
                        BookData[tmpid - 1].date = t.ToString("yyyy/MM/dd");
                        BookData[tmpid - 1].isRenew = "False";
                        BookInfo tm = BookData[tmpid - 1];
                        BookData.RemoveAt(tmpid - 1);
                        BookData.Insert(tmpid - 1, tm);
                    }
                });
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    new App.ToastTips("续借失败");
                    return;
                });
            }
        }
        private void grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (buttonc == true)
            {
                buttonc = false;
                return; 
            }
            var s = (Grid)sender;
            int i = Convert.ToInt32(s.Tag.ToString().TrimEnd());
            if (BookData[i - 1].detailvis == Visibility.Collapsed)
            {
                BookData[i - 1].detailvis = Visibility.Visible;
            }
            else
            {
                BookData[i - 1].detailvis = Visibility.Collapsed;
            }
        }
        ObservableCollection<FavList> FavInfo = new ObservableCollection<FavList>();
        bool isFavloaded = false;
        private void Panorama1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Panorama1.SelectedIndex)
            {
                case 1:
                    if (isFavloaded == true)
                        return;
                    FavInfo.Clear();
                    FavLoadingTip.Visibility = Visibility.Visible;
                    UniRequest request = new UniRequest("http://xiyoumobile.com/lib/favorite.aspx", "POST");
                    request.AddParams("ID", App.ID);
                    request.AddParams("Password", App.Password);
                    isFavloaded = true;
                    request.StartRequest(result =>
                        {
                            FavLoadingTip.Visibility = Visibility.Collapsed;
                            JArray data = null;
                            try
                            {
                                data = JArray.Parse(result);
                            }
                            catch
                            {
                                NullFavResult.Visibility = Visibility.Visible;
                                isFavloaded = true;
                                return;
                            }
                            if (data.Count == 0)
                            {
                                NullFavResult.Visibility = Visibility.Visible;
                                isFavloaded = true;
                                return;
                            }
                            foreach (var s in data)
                            {
                                FavInfo.Add(new FavList
                                {
                                    ID = (string)s["ID"],
                                    Title = (string)s["Title"],
                                    Img = (string)s["Img"],
                                    Sort = (string)s["Sort"]
                                });
                            }
                            isFavloaded = true;
                        });
                    break;
                default: break;
            }
        }

        private void DelFav_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确实要将这本书移出你的收藏夹吗？", "温馨提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            var s = (MenuItem)sender;
            string id = s.Tag.ToString();
            load.IsVisible = true;
            UniRequest request = new UniRequest("http://xiyoumobile.com/lib/DelFav.aspx", "POST");
            request.AddParams("ID", App.ID);
            request.AddParams("Password", App.Password);
            request.AddParams("Book", id);
            request.StartRequest(result =>
                {
                    load.IsVisible = false;
                    if (!result.Contains("Deleted_Successed"))
                    {
                        new App.ToastTips("移出失败");
                        return;
                    }
                    else
                    {
                        new App.ToastTips("移出成功");
                        int i;
                        for (i = 0; i < FavInfo.Count; i++)
                        {
                            if (FavInfo[i].ID == id)
                                break;
                        }
                        FavInfo.RemoveAt(i);
                        if (FavInfo.Count == 0)
                            NullFavResult.Visibility = Visibility.Visible;
                        return;
                    }
                });
        }

        private void StackPanel_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var s = (StackPanel)sender;
            string uri = "/BookDetail.xaml?id=" + s.Tag.ToString();
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            switch (Panorama1.SelectedIndex)
            {
                case 0:
                    load.IsVisible = true;
                    UniRequest request = new UniRequest("http://old.xiyoumobile.com/library/login?stmp=" + DateTime.Now.ToString("yyyyMMddHHmmssfff"), "POST");
                    request.AddParams("userNumber", App.ID);
                    request.AddParams("password", App.Password);
                    request.StartRequest(result =>
                        {
                            load.IsVisible = false;
                            JArray data = JArray.Parse(result);
                            if (data.Count == 0)
                            {
                                new App.ToastTips("暂时没有任何借阅信息");
                                //toptips.Begin();
                                textBlock3.Visibility = Visibility.Visible;
                                textBlock4.Text = "你暂时没有任何图书借阅信息";
                                textBlock4.Visibility = Visibility.Visible;
                                return;
                            }
                            BookData.Clear();
                            for (int i = 0; i < data.Count - 1; i++)
                            {
                                BookData.Add(new BookInfo
                                {
                                    id = ((string)data[i]["id"]).Replace(" ", ""),
                                    name = ((string)data[i]["name"]).Replace("\\u003d", "="),
                                    barcode = (string)data[i]["barcode"],
                                    library_id = (string)data[i]["library_id"],
                                    department_id = (string)data[i]["department_id"],
                                    isRenew = (string)data[i]["isRenew"],
                                    state = (string)data[i]["state"],
                                    date = (string)data[i]["date"],
                                    detailvis = Visibility.Collapsed
                                });
                            }
                            show.Begin();
                            textBlock3.Visibility = Visibility.Visible;
                        });
                    break;
                case 1:
                    NullFavResult.Visibility = Visibility.Collapsed;
                    load.IsVisible = true;
                    FavLoadingTip.Visibility = Visibility.Visible;
                    UniRequest request1 = new UniRequest("http://xiyoumobile.com/lib/favorite.aspx", "POST");
                    request1.AddParams("ID", App.ID);
                    request1.AddParams("Password", App.Password);
                    isFavloaded = true;
                    request1.StartRequest(aresult =>
                        {
                            load.IsVisible = false;
                            FavLoadingTip.Visibility = Visibility.Collapsed;
                            JArray data = null;
                            try
                            {
                                data = JArray.Parse(aresult);
                            }
                            catch
                            {
                                NullFavResult.Visibility = Visibility.Visible;
                                isFavloaded = true;
                                return;
                            }
                            if (data.Count == 0)
                            {
                                NullFavResult.Visibility = Visibility.Visible;
                                isFavloaded = true;
                                return;
                            }
                            FavInfo.Clear();
                            foreach (var s in data)
                            {
                                FavInfo.Add(new FavList
                                {
                                    ID = (string)s["ID"],
                                    Title = (string)s["Title"],
                                    Img = (string)s["Img"],
                                    Sort = (string)s["Sort"]
                                });
                            }
                            isFavloaded = true;
                        });
                    break;
                default: break;
            }
        }

        private void GetBookDetail_Click(object sender, RoutedEventArgs e)
        {
            var s = (MenuItem)sender;
            string uri = "/BookDetail.xaml?barcode=" + s.Tag.ToString().Replace("书号：","");
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        
    }
}
