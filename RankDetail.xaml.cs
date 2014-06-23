using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.ObjectModel;

namespace Xupt_lib
{
    public partial class RankDetail : PhoneApplicationPage
    {
        int RequsetSize = 10;
        public delegate void HandleResult(string result);
        private HandleResult handle;
        string kind = null;
        RankInfo info = new RankInfo();

        public RankDetail()
        {
            InitializeComponent();
            this.Loaded += RankDetail_Loaded;
        }

        void RankDetail_Loaded(object sender, RoutedEventArgs e)
        {
            load.IsVisible = true;
            try
            {
                kind = this.NavigationContext.QueryString["kind"];
            }
            catch
            {
                this.NavigationService.GoBack();
            }
            GetRanksInfo();
        }
        #region
        void GetRanksInfo()
        {
            Dispatcher.BeginInvoke(() =>
            {
                switch (kind)
                {
                    case "1":
                        {
                            string InnerID = "Rank1";
                            string RequestID = "01";
                            ProcInfo(InnerID, RequestID);
                        }
                        break;
                    case "2":
                        {
                            string InnerID = "Rank2";
                            string RequestID = "02";
                            ProcInfo(InnerID, RequestID);
                        }
                        break;
                    case "3":
                        {
                            string InnerID = "Rank3";
                            string RequestID = "03";
                            ProcInfo(InnerID, RequestID);
                        }
                        break;
                    case "4":
                        {
                            string InnerID = "Rank4";
                            string RequestID = "05";
                            ProcInfo(InnerID, RequestID);
                        }
                        break;
                    default: break;
                }
            });
        }
        void RanksHttpRequset(string type, HandleResult handle)
        {
            this.handle = handle;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://222.24.63.109/lib/rank.aspx?type={0}&size={1}", type, RequsetSize));
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
        void ProcInfo(string InnerID, string RequestID)
        {
            string Rank1 = "";
            if (IsolatedStorageSettings.ApplicationSettings.Contains(InnerID))
            {
                if (!string.IsNullOrEmpty(IsolatedStorageSettings.ApplicationSettings[InnerID].ToString()))
                {
                    Rank1 = IsolatedStorageSettings.ApplicationSettings[InnerID].ToString();
                    JObject obj = JObject.Parse(Rank1);
                    ProcJSON(obj);
                }
                else
                {
                    RanksHttpRequset(RequestID, result =>
                    {
                        Rank1 = result;
                        IsolatedStorageSettings.ApplicationSettings[InnerID] = Rank1;
                        JObject obj = JObject.Parse(Rank1);
                        ProcJSON(obj);
                    });
                }
            }
            else
            {
                RanksHttpRequset(RequestID, result =>
                {
                    Rank1 = result;
                    IsolatedStorageSettings.ApplicationSettings[InnerID] = Rank1;
                    JObject obj = JObject.Parse(Rank1);
                    ProcJSON(obj);
                });
            }
        }
        void GetMore()
        {
            RequsetSize += 10;
            GetRanksInfo();
        }
        void ProcJSON(JObject obj)
        {
            Dispatcher.BeginInvoke(() =>
            {
                ObservableCollection<object> item = new ObservableCollection<object>();
                info.Amount = (int)obj["Amount"];
                info.Size = (int)obj["Size"];
                info.Type = (string)obj["Type"];
                foreach (var i in obj["Item"])
                {
                    object temp;
                    switch (kind)
                    {
                        case "1":
                            {
                                temp = new RentRank
                                {
                                    ID = (string)i["ID"],
                                    Times = (string)i["Times"],
                                    Title = (string)i["Title"],
                                    Rank = (string)i["Rank"],
                                    Sort = (string)i["Sort"],
                                    isSearch = Visibility.Collapsed
                                };
                            }
                            break;
                        case "2":
                            {
                                temp = new SearchRank
                                {
                                    Times = (string)i["Times"],
                                    Rank = (string)i["Rank"],
                                    Keyword = (string)i["Keyword"],
                                    isSearch = Visibility.Visible
                                };
                            }
                            break;
                        case "3":
                        case "4":
                            {
                                temp = new FavRank
                                {
                                    ID = (string)i["ID"],
                                    Times = (string)i["Times"],
                                    Title = (string)i["Title"],
                                    Rank = (string)i["Rank"],
                                    isSearch = Visibility.Collapsed
                                };
                            }
                            break;
                        default:
                            temp = null;
                            break;
                    }
                    item.Add(temp);
                }
                info.Items = item;
                BindData();
            });
        }
        void BindData()
        {
            Keyword.Text = info.Type;
            Counter.Text = string.Format("共计：{0} 条", info.Amount);
            ResultList.ItemsSource = info.Items;
            load.IsVisible = false;
        }

        private void BookItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selected=(StackPanel)sender;
            if (selected.Tag.ToString() == "")
            {
                return;
            }
            switch (kind)
            {
                case "2":
                    {
                        App.SearchParam = selected.Tag.ToString();
                        this.NavigationService.Navigate(new Uri("/SearchResult.xaml", UriKind.Relative));
                    }
                    break;
                default:
                    {
                        string uri = "/BookDetail.xaml?id=" + selected.Tag.ToString();
                        this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
                    }
                    break;
            }
        }
    }
    
}