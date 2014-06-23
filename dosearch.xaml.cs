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

namespace Xupt_lib
{
    public partial class dosearch : PhoneApplicationPage
    {
        string word, type, match, record, lib;
        public dosearch()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            load.IsIndeterminate = true;
            load.IsVisible = true;
            load.Text = "加载中...";
            word = NavigationContext.QueryString["word"];
            type = NavigationContext.QueryString["type"];
            match = NavigationContext.QueryString["match"];
            record = NavigationContext.QueryString["record"];
            lib = NavigationContext.QueryString["lib"];
            string uri = "kind=simple&type=" + type + "&word=" + word + "&match=" + match + "&recordtype=" + record + "&library_id=" + lib + "&x=0&y=0";
            webBrowser1.Navigate(new Uri("http://222.24.3.7:8080/book/queryOut.jsp?" + uri, UriKind.Absolute));
            webBrowser1.LoadCompleted += new System.Windows.Navigation.LoadCompletedEventHandler(webBrowser1_LoadCompleted);
            get_time();
        }
        public void get_time()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(20);
            timer.Tick += timerTick;
            timer.Start();
        }

        void timerTick(object sender, EventArgs e)
        {
            if (grid1.Opacity == 1)
            {
                lost.Begin();
                MessageBox.Show("您当前的网络环境无法连接至西安邮电大学图书馆，请尝试使用校内网络！", "网络错误", MessageBoxButton.OK);
                webBrowser1.IsEnabled = false;
                load.IsVisible = false;
            }
        }
        void webBrowser1_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            load.IsVisible = false;
            lost.Begin();
            String returned;
            returned = webBrowser1.SaveToString();
            show.Begin();
            books.Text = returned;
        }
    }
}
