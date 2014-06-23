using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Xupt_lib
{
    public partial class NewBookResult : PhoneApplicationPage
    {
        ObservableCollection<SearchList> Info = new ObservableCollection<SearchList>();

        bool isLoaded = false;

        string currenttype = "";
        int currentPage = 1;

        public NewBookResult()
        {
            InitializeComponent();
            this.Loaded += NewBookResult_Loaded;
            ResultList.ItemsSource = Info;
        }

        void NewBookResult_Loaded(object sender, RoutedEventArgs e)
        {
            if (isLoaded == true)
                return;
            try
            {
                string type = this.NavigationContext.QueryString["type"];
            }
            catch
            {
                new App.ToastTips("参数丢失");
                return;
            }
            switch (this.NavigationContext.QueryString["type"])
            {
                case "N1": Keyword.Text = "电工技术"; break;
                case "N2": Keyword.Text = "航空、航天"; break;
                case "N3": Keyword.Text = "化学工业"; break;
                case "N4": Keyword.Text = "环境科学、安全科学"; break;
                case "N5": Keyword.Text = "机械、仪表工业"; break;
                case "N6": Keyword.Text = "交通运输"; break;
                case "N7": Keyword.Text = "金属学与金属工艺"; break;
                case "N8": Keyword.Text = "经济"; break;
                case "N9": Keyword.Text = "军事"; break;
                case "N10": Keyword.Text = "历史、地理"; break;
                case "N11": Keyword.Text = "社会科学总论"; break;
                case "N12": Keyword.Text = "数理科学和化学"; break;
                case "N13": Keyword.Text = "天文学、地球科学"; break;
                case "N14": Keyword.Text = "文化、科学、教育、体育"; break;
                case "N15": Keyword.Text = "文学"; break;
                case "N16": Keyword.Text = "无线电电子学、电信技术"; break;
                case "N17": Keyword.Text = "一般工业技术"; break;
                case "N18": Keyword.Text = "医药、卫生"; break;
                case "N19": Keyword.Text = "艺术"; break;
                case "N20": Keyword.Text = "语言、文字"; break;
                case "N21": Keyword.Text = "哲学、宗教"; break;
                case "N22": Keyword.Text = "政治、法律"; break;
                case "N23": Keyword.Text = "自动化技术、计算机技术"; break;
                default: break;
            }
            Keyword.Text = Keyword.Text + " 相关新书";
            int n = Convert.ToInt32(NavigationContext.QueryString["type"].Replace("N", ""));
            string param="";
            if (n < 10)
                param = "0" + n.ToString();
            else
                param = n.ToString();
            load.IsVisible = true;
            UniRequest request = new UniRequest("http://www.xiyoumobile.com/lib/newbook.aspx?type=" + param);
            request.StartRequest(result =>
                {
                    isLoaded = true;
                    currenttype = param;
                    load.IsVisible = false;
                    if (result == "Server_Error")
                    {
                        new App.ToastTips("服务器错误");
                        return;
                    }
                    if (result == "Request Error!")
                    {
                        new App.ToastTips("服务器错误");
                        return;
                    }
                    if (result == "null")
                    {
                        new App.ToastTips("该类别暂无新书");
                        NoBooks.Visibility = Visibility.Visible;
                        return;
                    }
                    JObject data = JObject.Parse(result);
                    currentPage = (int)data["CurrentPage"];
                    int amount = (int)data["Amount"];
                    Amount.Text = "共计 " + amount.ToString() + " 本新书";
                    int pages = (int)data["Pages"];
                    Info.Clear();
                    foreach (var s in data["Item"])
                    {
                        Info.Add(new SearchList
                        {
                            ID = (string)s["ID"],
                            Name = (string)s["Title"],
                            BookItem = Visibility.Visible,
                            LoadingMore = Visibility.Collapsed
                        });
                    }
                    if (pages > 1)
                    {
                        Info.Add(new SearchList
                        {
                            BookItem = Visibility.Collapsed,
                            LoadingMore = Visibility.Visible,
                            ShowContent = "加载更多...",
                            IsEnable = "true"
                        });
                    }
                    else
                    {
                        Info.Add(new SearchList
                        {
                            BookItem = Visibility.Collapsed,
                            LoadingMore = Visibility.Visible,
                            ShowContent = "无更多信息",
                            IsEnable = "false"
                        });
                    }
                });
        }

        private void BookItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var s = (StackPanel)sender;
            string uri = "/Bookdetail.xaml?id=" + s.Tag.ToString();
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void LoadMoreTag_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Info[Info.Count - 1].ShowContent = "加载中...";
            Info[Info.Count - 1].IsEnable = "false";
            load.IsVisible = true;
            UniRequest request = new UniRequest("http://www.xiyoumobile.com/lib/newbook.aspx?page=" + (currentPage + 1).ToString() + "&type=" + currenttype);
            request.StartRequest(result =>
                {
                    load.IsVisible = false;
                    if (result == "Server_Error")
                    {
                        new App.ToastTips("服务器错误");
                        return;
                    }
                    if (result == "Request Error!")
                    {
                        new App.ToastTips("服务器错误");
                        return;
                    }
                    if (result == "Out_Of_Range")
                    {
                        new App.ToastTips("超出范围");
                        NoBooks.Visibility = Visibility.Visible;
                        return;
                    }
                    JObject data = JObject.Parse(result);
                    currentPage = (int)data["CurrentPage"];
                    int pages=(int)data["Pages"];
                    foreach (var s in data["Item"])
                    {
                        Info.Insert(Info.Count - 1, new SearchList
                        {
                            ID = (string)s["ID"],
                            Name = (string)s["Title"],
                            BookItem = Visibility.Visible,
                            LoadingMore = Visibility.Collapsed
                        });
                    }

                    if (currentPage < pages)
                    {
                        Info[Info.Count - 1].ShowContent = "加载更多...";
                        Info[Info.Count - 1].IsEnable = "True";
                    }
                    else
                    {
                        Info[Info.Count - 1].ShowContent = "无更多内容";
                        Info[Info.Count - 1].IsEnable = "false";
                    }
                    this.ResultList.SelectedIndex = this.ResultList.Items.Count - 1;
                    this.ResultList.ScrollIntoView(this.ResultList.SelectedItem);
                });
        }
    }
}