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
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Net.NetworkInformation;
using System.Reflection;
using System.Diagnostics;

namespace Xupt_lib
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask web = new WebBrowserTask();
            web.Uri=new Uri("http://page.renren.com/601470193",UriKind.Absolute);
            web.Show();
            this.NavigationService.GoBack();
        }

        private void mark_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask mp = new MarketplaceReviewTask();
            mp.Show();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask em = new EmailComposeTask();
            em.Subject = "反馈西邮图书信息查询系统-WP7版";
            em.To = "xiyouwp@126.com";
            em.Show();
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (DeviceNetworkInformation.IsNetworkAvailable == false)
            {
                new App.ToastTips("网络不可用");
                return;
            }
        }

        private void weibo_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask web = new WebBrowserTask();
            web.Uri = new Uri("http://weibo.com/u/2794801680", UriKind.Absolute);
            web.Show();   
        }

        private void renren_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask web = new WebBrowserTask();
            web.Uri = new Uri("http://3g.renren.com/profile.do?id=601470193", UriKind.Absolute);
            web.Show();
        }

        private void Mark_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MarketplaceReviewTask mp = new MarketplaceReviewTask();
            mp.Show();
        }

        private void Feedback_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string s = GetVersionNumber();
            EmailComposeTask em = new EmailComposeTask();
            em.Subject = "反馈《西邮图书馆-WP客户端》 V" + s;
            em.To = "feedback@xiyoumobile.com";
            em.Show();
        }
        private static string GetVersionNumber()
        {
            var asm = Assembly.GetExecutingAssembly();
            var parts = asm.FullName.Split(',');
            return parts[1].Split('=')[1];
        }
    }
}