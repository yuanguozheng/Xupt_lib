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

namespace Xupt_lib
{
    public partial class LoginPage : PhoneApplicationPage
    {
        string book = "";

        public LoginPage()
        {
            InitializeComponent();
            this.Loaded += LoginPage_Loaded;
        }

        void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                book = this.NavigationContext.QueryString["book"];
            }
            catch { }
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            string passwd = Password.Password;
            string username = ID.Text;
            if (passwd == "" || username == "")
            {
                new App.ToastTips("用户名或密码不能为空");
                return;
            }
            load.IsIndeterminate = true;
            load.IsVisible = true;
            load.Text = "加载中...";
            if (Remember.IsChecked == true)
            {
                IsolatedStorageSettings.ApplicationSettings["UserName"] = ID.Text;
                IsolatedStorageSettings.ApplicationSettings["Password"] = Password.Password;
                IsolatedStorageSettings.ApplicationSettings["Remember"] = true;
            }
            UniRequest Login = new UniRequest("http://xiyoumobile.com/lib/AddFav.aspx","POST");
            Login.AddParams("id", username);
            Login.AddParams("password", passwd);
            Login.AddParams("book", book);
            Login.StartRequest(result =>
                {
                    load.IsVisible=false;
                    Dispatcher.BeginInvoke(() =>
                        {
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
                            else if (result.Contains("Account"))
                            {
                                new App.ToastTips("用户名或密码错误");
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

        private void Reset_Click(object sender, EventArgs e)
        {
            ID.Text = "";
            Password.Password = "";
            Remember.IsChecked = false;
        }

        private void ID_GotFocus(object sender, RoutedEventArgs e)
        {
            ID.SelectAll();
        }

        private void ID_LostFocus(object sender, RoutedEventArgs e)
        {
            ID.Text = ID.Text.ToUpper();
        }

        private void Old_GotFocus(object sender, RoutedEventArgs e)
        {
            Password.SelectAll();
        }
    }
}