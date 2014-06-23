using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Net.NetworkInformation;

namespace Xupt_lib
{
    public partial class SpecificSearch : PhoneApplicationPage
    {
        List<ListPickerItemClass> Type, Match, Record, Lib;
        public SpecificSearch()
        {
            InitializeComponent();
            Type = new List<ListPickerItemClass>();
            Type.Add(new ListPickerItemClass { Title = "所有题名", Key = "title" });
            Type.Add(new ListPickerItemClass { Title = "著/作者", Key = "author" });
            Type.Add(new ListPickerItemClass { Title = "标准号(ISBN或ISSN)", Key = "number" });
            Type.Add(new ListPickerItemClass { Title = "主题词", Key = "subject_term" });
            Type.Add(new ListPickerItemClass { Title = "分类号", Key = "call_no" });
            TypeList.ItemsSource = Type;

            Match = new List<ListPickerItemClass>();
            Match.Add(new ListPickerItemClass { Title = "模糊匹配", Key = "mh" });
            Match.Add(new ListPickerItemClass { Title = "前向匹配", Key = "qx" });
            Match.Add(new ListPickerItemClass { Title = "精确匹配", Key = "jq" });
            MatchList.ItemsSource = Match;

            Record = new List<ListPickerItemClass>();
            Record.Add(new ListPickerItemClass { Title = "全部", Key = "all" });
            Record.Add(new ListPickerItemClass { Title = "中文图书", Key = "01" });
            Record.Add(new ListPickerItemClass { Title = "西文图书", Key = "02" });
            Record.Add(new ListPickerItemClass { Title = "日文图书", Key = "03" });
            Record.Add(new ListPickerItemClass { Title = "俄文图书", Key = "04" });
            Record.Add(new ListPickerItemClass { Title = "中文期刊", Key = "11" });
            Record.Add(new ListPickerItemClass { Title = "西文期刊", Key = "12" });
            Record.Add(new ListPickerItemClass { Title = "日文期刊", Key = "13" });
            Record.Add(new ListPickerItemClass { Title = "俄文期刊", Key = "14" });
            Record.Add(new ListPickerItemClass { Title = "中文报纸", Key = "c1" });
            Record.Add(new ListPickerItemClass { Title = "西文报纸", Key = "e1" });
            Record.Add(new ListPickerItemClass { Title = "数据库", Key = "s1" });
            Record.Add(new ListPickerItemClass { Title = "年鉴", Key = "z1" });
            RecordList.ItemsSource = Record;

            Lib = new List<ListPickerItemClass>();
            Lib.Add(new ListPickerItemClass { Title = "西安邮电学院图书馆", Key = "A" });
            Lib.Add(new ListPickerItemClass { Title = "虚拟馆", Key = "B" });
            LibList.ItemsSource = Lib;
        }

        private void TypeList_Loaded(object sender, RoutedEventArgs e)
        {
            if (DeviceNetworkInformation.IsNetworkAvailable == false)
            {
                new App.ToastTips("网络不可用");
                return;
            }
        }

        private void search_Click(object sender, EventArgs e)
        {
            DoSearch();
        }
        private void DoSearch()
        {
            if (keyword.Text == "")
            {
                new App.ToastTips("关键词不能为空");
                return;
            }
            if (keyword.Text.Length <= 1)
            {
                new App.ToastTips("关键词长度过短");
                return;
            }
            string type = ((ListPickerItemClass)TypeList.SelectedItem).Key;
            string match = ((ListPickerItemClass)MatchList.SelectedItem).Key;
            string record = ((ListPickerItemClass)RecordList.SelectedItem).Key;
            string lib = ((ListPickerItemClass)LibList.SelectedItem).Key;
            App.SearchParam = keyword.Text;
            string uri = string.Format("/SearchResult.xaml?type={0}&match={1}&record={2}&lib={3}", type, match, record, lib);
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void keyword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                DoSearch();
        }

        private void scanbarcode_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/ScanPage.xaml", UriKind.Relative));
        }
    }
    public class ListPickerItemClass
    {
        public string Title
        {
            get;
            set;
        }
        public string Key
        {
            get;
            set;
        }
    }
}