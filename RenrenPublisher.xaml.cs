using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RenrenSDKLibrary;

namespace Xupt_lib
{
    public partial class RenrenPublisher : PhoneApplicationPage
    {
        RenrenAPI api = App.api;
        public RenrenPublisher()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            content.Text = App.RenrenShareContent;
            GetUser();
        }
        void GetUser()
        {
            api.GetCurUserInfo(renren_GetCurUserInfoCompletedHandler);
        }
        void renren_GetCurUserInfoCompletedHandler(object sender, GetUserUidCompletedEventArgs e)
        {
            name.Text = e.Result.name;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<APIParameter> param = new List<APIParameter>();
            param.Add(new APIParameter("method", "status.set"));
            param.Add(new APIParameter("status", content.Text));
            //param.Add(new APIParameter("name", "图书分享"));
            //param.Add(new APIParameter("description", content.Text));
            //param.Add(new APIParameter("url", App.DoubanLink));
            //param.Add(new APIParameter("message", "我分享了一本书"));
            //param.Add(new APIParameter("action_name", "来自西邮图书馆-WP"));
            //param.Add(new APIParameter("action_link", "http://www.windowsphone.com/s?appid=becf5496-8127-4c56-8917-bc855f70d797"));
            //if (App.ImgUrl != "")
            //    param.Add(new APIParameter("image", App.ImgUrl));
            api.RequestAPIInterface(PublishBlogCompletedCallBack, param);
            this.NavigationService.GoBack();
            new App.ToastTips("正在发布请稍后");
            
        }
        void PublishBlogCompletedCallBack(object sender, APIRequestCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                new App.ToastTips("分享成功");
            }
            else
            {
                new App.ToastTips("分享失败");
            }
        }
        private void logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确实要注销吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            api.LogOut();
            this.NavigationService.GoBack();
        }
    }
}