using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Text;

namespace Xupt_lib
{
    public partial class ModifyPassword : PhoneApplicationPage
    {
        public ModifyPassword()
        {
            InitializeComponent();
        }

        private void ID_LostFocus(object sender, RoutedEventArgs e)
        {
            ID.Text = ID.Text.ToUpper();
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            ID.Text = "";
            Old.Password = "";
            New1.Password = "";
            New2.Password = "";
        }

        private void ID_GotFocus(object sender, RoutedEventArgs e)
        {
            ID.SelectAll();
        }

        private void Old_GotFocus(object sender, RoutedEventArgs e)
        {
            Old.SelectAll();
        }

        private void New1_GotFocus(object sender, RoutedEventArgs e)
        {
            New1.SelectAll();
        }

        private void New2_GotFocus(object sender, RoutedEventArgs e)
        {
            New2.SelectAll();
        }
        string Param = "";
        private void Confirm_Click(object sender, EventArgs e)
        {
            if (!(ID.Text != "" && Old.Password != "" && New1.Password != "" && New2.Password != ""))
            {
                new App.ToastTips("信息不完整");
                return;
            }
            if (New1.Password != New2.Password)
            {
                new App.ToastTips("两次密码输入不一致");
                return;
            }
            if (MessageBox.Show("确认修改吗？","提示",MessageBoxButton.OKCancel)==MessageBoxResult.Cancel)
            {
                return;
            }
            Param = string.Format("ID={0}&Password={1}&NewPassword={2}&Verify={3}", ID.Text, Old.Password, New1.Password, New2.Password);
            load.IsVisible = true;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.xiyoumobile.com/lib/modify.aspx");
            request.Method = "POST";
            request.UserAgent = "XiyouLibrary_Windows_Phone_Client";
            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream(new AsyncCallback(GiveParam), request);
        }
        void GiveParam(IAsyncResult result)
        {
            byte[] PostBin = Encoding.UTF8.GetBytes(Param);
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            Stream st = request.EndGetRequestStream(result);
            st.Write(PostBin, 0, PostBin.Length);
            st.Close();
            request.BeginGetResponse(new AsyncCallback(GetResponse), request);
        }
        void GetResponse(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string restr = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            Dispatcher.BeginInvoke(() =>
            {
                load.IsVisible = false;
                if (restr != "Modify_Successed")
                {
                    new App.ToastTips("修改失败");
                    return;
                }
                else
                {
                    var s = new App.ToastTips("修改成功");
                    s.Completed += s_Completed;
                }
            });
        }

        void s_Completed(object sender, Coding4Fun.Toolkit.Controls.PopUpEventArgs<string, Coding4Fun.Toolkit.Controls.PopUpResult> e)
        {
            this.NavigationService.GoBack();
        }
    }
}