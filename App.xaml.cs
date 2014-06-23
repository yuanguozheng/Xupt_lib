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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO.IsolatedStorage;

namespace Xupt_lib
{
    public partial class App : Application
    {
        public static RenrenSDKLibrary.RenrenAPI api = new RenrenSDKLibrary.RenrenAPI("233813", "d70ec16672ac47f6afa7cfee771d3cff", "fadb482122b14f309dbcbfd1ca6ee880");
        public static string RenrenShareContent;
        public static string WeiboToken;
        public static string WeiboUid;
        public static string WeiboShareContent;
        public static string ImgUrl;
        public static string DoubanLink;
        public static string SearchParam;
        public static string ID;
        public static string Password;
        /// <summary>
        /// 提供对电话应用程序的根框架的轻松访问。
        /// </summary>
        /// <returns>电话应用程序的根框架。</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }
        public static UserInfo User = new UserInfo();
        /// <summary>
        /// Application 对象的构造函数。
        /// </summary>
        public App()
        {
            // 未捕获的异常的全局处理程序。 
            UnhandledException += Application_UnhandledException;

            // 标准 Silverlight 初始化
            InitializeComponent();

            // 特定于电话的初始化
            InitializePhoneApplication();

            // 调试时显示图形分析信息。
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 显示当前帧速率计数器
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // 显示在每个帧中重绘的应用程序区域。
                //Application.Current.Host.Settings.EnableRedrawRegions = true；

                // 启用非生产分析可视化模式， 
                // 该模式显示递交给 GPU 的包含彩色重叠区的页面区域。
                //Application.Current.Host.Settings.EnableCacheVisualization = true；

                // 通过将应用程序的 PhoneApplicationService 对象的 UserIdleDetectionMode 属性
                // 设置为 Disabled 来禁用应用程序空闲检测。
                //  注意: 仅在调试模式下使用此设置。禁用用户空闲检测的应用程序在用户不使用电话时将继续运行
                // 并且消耗电池电量。
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }
        public static BookInfo[] bookList { get; set; }
        public string info { get; set; }
        // 应用程序启动(例如，从“开始”菜单启动)时执行的代码
        // 此代码在重新激活应用程序时不执行
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            if(IsolatedStorageSettings.ApplicationSettings.Contains("WeiboToken"))
                WeiboToken = Convert.ToString(IsolatedStorageSettings.ApplicationSettings["WeiboToken"]);
        }

        // 激活应用程序(置于前台)时执行的代码
        // 此代码在首次启动应用程序时不执行
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // 确保正确恢复应用程序状态
            if (!DeviceNetworkInformation.IsNetworkAvailable)
            {
                new App.ToastTips("网络不可用");
                return;
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains("WeiboToken"))
                WeiboToken = Convert.ToString(IsolatedStorageSettings.ApplicationSettings["WeiboToken"]);
        }

        // 停用应用程序(发送到后台)时执行的代码
        // 此代码在应用程序关闭时不执行
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // 应用程序关闭(例如，用户点击“后退”)时执行的代码
        // 此代码在停用应用程序时不执行
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // 确保所需的应用程序状态在此处保持不变。
        }

        // 导航失败时执行的代码
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 导航已失败；强行进入调试器
                System.Diagnostics.Debugger.Break();
            }
        }

        // 出现未处理的异常时执行的代码
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 出现未处理的异常；强行进入调试器
                System.Diagnostics.Debugger.Break();
            }
        }

        #region 电话应用程序初始化

        // 避免双重初始化
        private bool phoneApplicationInitialized = false;

        // 请勿向此方法中添加任何其他代码
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // 创建框架但先不将它设置为 RootVisual；这允许初始
            // 屏幕保持活动状态，直到准备呈现应用程序时。
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // 处理导航故障
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;
            RootFrame.Navigating += RootFrame_Navigating;
            // 确保我们未再次初始化
            phoneApplicationInitialized = true;
        }

        void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri.ToString().Contains("MainPage.xaml") && isFirstUse())
            {
                e.Cancel = true;
                this.RootFrame.Dispatcher.BeginInvoke(delegate
                {
                    this.RootFrame.Navigate(new Uri("/Guide.xaml", UriKind.Relative));
                });
            }
        }
        bool isFirstUse()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("First"))
            {
                if ((bool)IsolatedStorageSettings.ApplicationSettings["First"] == false)
                {
                    return false;
                }
                else
                    return true;
            }
            else
                return true;
        }
        // 请勿向此方法中添加任何其他代码
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // 设置根视觉效果以允许应用程序呈现
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // 删除此处理程序，因为不再需要它
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

        /// <summary>
        /// 用于提示
        /// </summary>
        public class ToastTips : ToastPrompt
        {
            /// <summary>
            /// 显示顶部的Toast通知
            /// </summary>
            /// <param name="message">通知的内容</param>
            public ToastTips(string message)
            {
                Thickness a = new Thickness(0, 25, 0, 0);
                this.Title = message;
                this.Margin = a;
                this.Background = new SolidColorBrush(Color.FromArgb(255, 0, 133, 226));
                this.Foreground = new SolidColorBrush(Colors.White);
                this.Message = " ";
                this.FontSize = 28;
                this.Height = 72;
                this.Show();
            }
        }
    }
    public class UserInfo
    {
        public string ID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Cls
        {
            get;
            set;
        }
    }
}