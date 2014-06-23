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
using System.IO.IsolatedStorage;
using System.Windows.Threading;
using System.ComponentModel;
using Microsoft.Devices;
using Microsoft.Phone.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;
using System.Web;
using System.Threading;
using System.Diagnostics;

namespace Xupt_lib
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            N1.Tap += N1_Tap;
            N2.Tap += N1_Tap;
            N3.Tap += N1_Tap;
            N4.Tap += N1_Tap;
            N5.Tap += N1_Tap;
            N6.Tap += N1_Tap;
            N7.Tap += N1_Tap;
            N8.Tap += N1_Tap;
            N9.Tap += N1_Tap;
            N10.Tap += N1_Tap;
            N11.Tap += N1_Tap;
            N12.Tap += N1_Tap;
            N13.Tap += N1_Tap;
            N14.Tap += N1_Tap;
            N15.Tap += N1_Tap;
            N16.Tap += N1_Tap;
            N17.Tap += N1_Tap;
            N18.Tap += N1_Tap;
            N19.Tap += N1_Tap;
            N20.Tap += N1_Tap;
            N21.Tap += N1_Tap;
            N22.Tap += N1_Tap;
            N23.Tap += N1_Tap;

        }

        void N1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var s = (Grid)sender;
            string uri = "/NewBookResult.xaml?type=" + s.Name.ToString();
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
        
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.NavigationService.BackStack.ElementAt(0).Source.ToString().Contains("Guide.xaml"))
                {
                    this.NavigationService.RemoveBackEntry();
                }
            }
            catch { }
            if (DeviceNetworkInformation.IsNetworkAvailable == false)
            {
                new App.ToastTips("网络不可用");
                return;
            }
            LoadUserInfo();
        }

        private void login_Click(object sender, EventArgs e)
        {
            doLogin();
        }
        private void doLogin()
        {
            string passwd = passwd1.Password;
            string username = textBox1.Text;
            App.ID = username;
            App.Password = passwd;
            if (passwd == "" || username == "")
            {
                new App.ToastTips("用户名或密码不能为空");
                return;
            }
            ProcUserInfo();
            load.IsIndeterminate = true;
            load.IsVisible = true;
            load.Text = "加载中...";
            if (!DeviceNetworkInformation.IsNetworkAvailable)
            {
                new App.ToastTips("网络不可用");
                load.IsVisible = false;
                return;
            }
            GetWebData(username, passwd, result =>
            {
                load.IsVisible = false;
                if (result == "shibai")
                {
                    new App.ToastTips("用户名或密码错误");
                    return;
                }
                else if (result == "null")
                {
                    new App.ToastTips("信息不完整");
                    return;
                }
                else if (result == "error")
                {
                    new App.ToastTips("服务器错误");
                    return;
                }
                else if (result == "")
                {
                    return;
                }
                else
                {
                    (App.Current as App).info = result;
                    GetUserData(username, passwd, aresult =>
                    {
                        if (aresult == "true")
                        {
                            this.NavigationService.Navigate(new Uri("/certify.xaml?user=true", UriKind.Relative));
                        }
                        else
                        {
                            if ((App.Current as App).info != null)
                                this.NavigationService.Navigate(new Uri("/certify.xaml?user=false", UriKind.Relative));
                            else
                            {
                                new App.ToastTips("网络错误");
                                return;
                            }
                        }
                    });
                }
            });
        }
        private void ProcUserInfo()
        {
            if (remember.IsChecked == true)
            {
                IsolatedStorageSettings.ApplicationSettings["UserName"] = textBox1.Text;
                IsolatedStorageSettings.ApplicationSettings["Password"] = passwd1.Password;
                IsolatedStorageSettings.ApplicationSettings["Remember"] = true;
            }
            else
            {
                try { IsolatedStorageSettings.ApplicationSettings.Remove("UserName"); }
                catch { }
                try { IsolatedStorageSettings.ApplicationSettings.Remove("Password"); }
                catch { }
                try { IsolatedStorageSettings.ApplicationSettings.Remove("Remember"); }
                catch { }
            }
        }

        private void LoadUserInfo()
        {
            if (!(IsolatedStorageSettings.ApplicationSettings.Contains("UserName") ||
                IsolatedStorageSettings.ApplicationSettings.Contains("Password") ||
                IsolatedStorageSettings.ApplicationSettings.Contains("Remember")))
            {
                textBox1.Text = "";
                passwd1.Password = "";
                remember.IsChecked = false;
            }
            else
            {
                textBox1.Text = IsolatedStorageSettings.ApplicationSettings["UserName"].ToString();
                passwd1.Password = IsolatedStorageSettings.ApplicationSettings["Password"].ToString();
                remember.IsChecked = true;
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            passwd1.Password = "";
            remember.IsChecked = false;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (book.SelectedIndex)
            {
                case 0:
                    ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["submit"];
                    break;
                case 1:
                    ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["search"];
                    break;
                case 2:
                    ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["rank"];
                    //GetRanksInfo();
                    break;
                default:
                    ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["submit"];
                    break;
            }
        }
#region
        /*void GetRanksInfo()
        {
            Dispatcher.BeginInvoke(() =>
                {
                    string Rank1, Rank2, Rank3, Rank4;
                    if (IsolatedStorageSettings.ApplicationSettings.Contains("Rank1"))
                    {
                        if (!string.IsNullOrEmpty(IsolatedStorageSettings.ApplicationSettings["Rank1"].ToString()))
                        {
                            Rank1 = IsolatedStorageSettings.ApplicationSettings["Rank1"].ToString();
                            JObject obj = JObject.Parse(Rank1);
                            Dispatcher.BeginInvoke(() =>
                            {
                                R1.Message = "共计：" + (string)obj["Amount"] + " 条";
                            });
                        }
                        else
                        {
                            RanksHttpRequset("01", result =>
                            {
                                Rank1 = result;
                                IsolatedStorageSettings.ApplicationSettings["Rank1"] = Rank1;
                                JObject obj = JObject.Parse(Rank1);
                                Dispatcher.BeginInvoke(() =>
                                {
                                    R1.Message = "共计：" + (string)obj["Amount"] + " 条";
                                });
                            });
                        }
                    }
                    else
                    {
                        RanksHttpRequset("01", result =>
                        {
                            Rank1 = result;
                            IsolatedStorageSettings.ApplicationSettings["Rank1"] = Rank1;
                            JObject obj = JObject.Parse(Rank1);
                            Dispatcher.BeginInvoke(() =>
                                {
                                    R1.Message = "共计：" + (string)obj["Amount"] + " 条";
                                });
                        });
                    }
                    if (IsolatedStorageSettings.ApplicationSettings.Contains("Rank2"))
                    {
                        if (!string.IsNullOrEmpty(IsolatedStorageSettings.ApplicationSettings["Rank2"].ToString()))
                        {
                            Rank2 = IsolatedStorageSettings.ApplicationSettings["Rank2"].ToString();
                            JObject obj = JObject.Parse(Rank2);
                            Dispatcher.BeginInvoke(() =>
                            {
                                R1.Message = "共计：" + (string)obj["Amount"] + " 条";
                            });
                        }
                        else
                        {
                            RanksHttpRequset("02", result =>
                            {
                                Rank2 = result;
                                IsolatedStorageSettings.ApplicationSettings["Rank2"] = Rank2;
                                JObject obj = JObject.Parse(Rank2);
                                Dispatcher.BeginInvoke(() =>
                                {
                                    R2.Message = "共计：" + (string)obj["Amount"] + " 条";
                                });
                            });
                        }
                    }
                    else
                    {
                        RanksHttpRequset("02", result =>
                        {
                            Rank2 = result;
                            IsolatedStorageSettings.ApplicationSettings["Rank2"] = Rank2;
                            JObject obj = JObject.Parse(Rank2);
                            Dispatcher.BeginInvoke(() =>
                                {
                                    R2.Message = "共计：" + (string)obj["Amount"] + " 条";
                                });
                        });
                    }
                    if (IsolatedStorageSettings.ApplicationSettings.Contains("Rank3"))
                    {
                        if (!string.IsNullOrEmpty(IsolatedStorageSettings.ApplicationSettings["Rank3"].ToString()))
                        {
                            Rank3 = IsolatedStorageSettings.ApplicationSettings["Rank3"].ToString();
                            JObject obj = JObject.Parse(Rank3);
                            Dispatcher.BeginInvoke(() =>
                            {
                                R1.Message = "共计：" + (string)obj["Amount"] + " 条";
                            });
                        }
                        else
                        {
                            RanksHttpRequset("03", result =>
                            {
                                Rank3 = result;
                                IsolatedStorageSettings.ApplicationSettings["Rank3"] = Rank3;
                                JObject obj = JObject.Parse(Rank3);
                                Dispatcher.BeginInvoke(() =>
                                {
                                    R3.Message = "共计：" + (string)obj["Amount"] + " 条";
                                });
                            });
                        }
                    }
                    else
                    {
                        RanksHttpRequset("03", result =>
                        {
                            Rank3 = result;
                            IsolatedStorageSettings.ApplicationSettings["Rank3"] = Rank3;
                            JObject obj = JObject.Parse(Rank3);
                            Dispatcher.BeginInvoke(() =>
                                {
                                    R3.Message = "共计：" + (string)obj["Amount"] + " 条";
                                });
                        });
                    }
                    if (IsolatedStorageSettings.ApplicationSettings.Contains("Rank4"))
                    {
                        if (!string.IsNullOrEmpty(IsolatedStorageSettings.ApplicationSettings["Rank4"].ToString()))
                        {
                            Rank4 = IsolatedStorageSettings.ApplicationSettings["Rank4"].ToString();
                            JObject obj = JObject.Parse(Rank4);
                            Dispatcher.BeginInvoke(() =>
                            {
                                R1.Message = "共计：" + (string)obj["Amount"] + " 条";
                            });
                        }
                        else
                        {
                            RanksHttpRequset("05", result =>
                            {
                                Rank4 = result;
                                IsolatedStorageSettings.ApplicationSettings["Rank4"] = Rank4;
                                JObject obj = JObject.Parse(Rank4);
                                Dispatcher.BeginInvoke(() =>
                                {
                                    R4.Message = "共计：" + (string)obj["Amount"] + " 条";
                                });
                            });
                        }
                    }
                    else
                    {
                        RanksHttpRequset("05", result =>
                        {
                            Rank4 = result;
                            IsolatedStorageSettings.ApplicationSettings["Rank4"] = Rank4;
                            JObject obj = JObject.Parse(Rank4);
                            Dispatcher.BeginInvoke(() =>
                                {
                                    R4.Message = "共计：" + (string)obj["Amount"] + " 条";
                                });
                        });
                    }
                });
        }*/
        void RanksHttpRequset(string type,HandleResult handle)
        {
            this.handle = handle;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://222.24.63.109/lib/rank.aspx?type=" + type);
            request.UserAgent = "XiyouLibrary_Windows_Phone_Client";
            request.BeginGetResponse(new AsyncCallback(RanksRes), request);
        }
        void RanksRes(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            handle(reader.ReadToEnd());
        }
#endregion
        private void about_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/about.xaml", UriKind.Relative));
        }

        private void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text == "")
                return;
            textBox1.Text = textBox1.Text.ToUpper();
            //if (textBox1.Text[0].ToString().ToUpper() != "S")
            //{
            //    Regex rex = new Regex("[0-9]+");
            //    Match ma = rex.Match(textBox1.Text);
            //    if (ma.Success)
            //    {
            //        textBox1.Text = "S" + textBox1.Text;
            //    }
            //}
        }
#region
        public delegate void HandleResult(string result);
        private HandleResult handle;
        private void GetWebData(string User, string Password, HandleResult handle)
        {
            this.handle = handle;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(string.Format("http://222.24.63.101/library/login?userNumber={0}&password={1}", User, Password));
            req.UserAgent = "XiyouLibrary_Windows_Phone_Client";
            req.BeginGetResponse(new AsyncCallback(GetResponse), req);
        }
        private void GetResponse(IAsyncResult result)
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
                Deployment.Current.Dispatcher.BeginInvoke(() => { handle("error"); });
            }
            Deployment.Current.Dispatcher.BeginInvoke(() => { handle(responsestring); });
        }
        byte[] postBin;
        private void GetUserData(string User, string Password,HandleResult handle)
        {
            this.handle = handle;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://222.24.63.109/libuser/");
            req.UserAgent = "XiyouLibrary_Windows_Phone_Client";
            string param = string.Format("id={0}&password={1}", User,Password);
            postBin = Encoding.UTF8.GetBytes(param);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            req.BeginGetRequestStream(new AsyncCallback(GetRequestSt), req);
        }
        private void GetRequestSt(IAsyncResult result)
        {
            HttpWebRequest Request = (HttpWebRequest)result.AsyncState;
            using (Stream st = Request.EndGetRequestStream(result))
            {
                st.Write(postBin, 0, postBin.Length);
            }
            Request.BeginGetResponse(new AsyncCallback(UserDataResponse), Request);
            //st.Close();
        }
        private void UserDataResponse(IAsyncResult result)
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
                Deployment.Current.Dispatcher.BeginInvoke(() => { handle("error"); });
            }
            if (responsestring.Contains("Login_Failed"))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { handle("failed"); });
            }
            JObject data = JObject.Parse(responsestring);
            App.User = new UserInfo
            {
                ID = (string)data["ID"],
                Name = (string)data["Name"],
                Cls = (string)data["Cls"]
            };
            Deployment.Current.Dispatcher.BeginInvoke(() => { handle("true"); });
        }
#endregion

        private void PhoneApplicationPage_BackKeyPress_1(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("确定退出吗？", "西邮图书借阅查询系统", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

        private void passwd1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                doLogin();
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text != "")
            {
                SearchBox.Tag = "";
            }
            else
            {
                SearchBox.Tag = "轻触输入关键词";
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text != "")
            {
                SearchBox.Tag = "";
            }
            else
            {
                SearchBox.Tag = "轻触输入关键词";
            }
            SearchBox.SelectAll();
        }

        private void DoSearch()
        {
            if (SearchBox.Text == "")
            {
                new App.ToastTips("关键词不能为空");
                return;
            }
            if (SearchBox.Text.Length <= 1)
            {
                new App.ToastTips("关键词长度过短");
                return;
            }
            App.SearchParam = SearchBox.Text;
            string uri = "/SearchResult.xaml";
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void search_Click(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DoSearch(); 
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoSearch();
            }
        }

        private void MoreSearch_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SpecificSearch.xaml", UriKind.Relative));
        }

        private void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox1.SelectAll();
        }

        private void passwd1_GotFocus_1(object sender, RoutedEventArgs e)
        {
            passwd1.SelectAll();
        }

        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                passwd1.Focus();
        }

        private void reset_search_Click(object sender, EventArgs e)
        {
            SearchBox.Text = "";
        }


        private void scanbarcode_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/ScanPage.xaml", UriKind.Relative));

        }

        private void refresh_rank_Click(object sender, EventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings.Remove("Rank1");
            IsolatedStorageSettings.ApplicationSettings.Remove("Rank2");
            IsolatedStorageSettings.ApplicationSettings.Remove("Rank3");
            IsolatedStorageSettings.ApplicationSettings.Remove("Rank4");
        }

        private void R1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/RankDetail.xaml?kind=1",UriKind.Relative));
        }

        private void R2_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/RankDetail.xaml?kind=2", UriKind.Relative));
        }

        private void R3_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/RankDetail.xaml?kind=3", UriKind.Relative));
        }

        private void R4_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/RankDetail.xaml?kind=4", UriKind.Relative));
        }

        private void mPass_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/ModifyPassword.xaml", UriKind.Relative));
        }

        private void clearUserInfo_Click(object sender, EventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings.Remove("UserName");
            IsolatedStorageSettings.ApplicationSettings.Remove("Password");
            IsolatedStorageSettings.ApplicationSettings.Remove("Remember");
            textBox1.Text = "";
            passwd1.Password = "";
            remember.IsChecked = false;
        }

       

    }
}