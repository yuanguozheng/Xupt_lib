﻿#pragma checksum "D:\C#\Windows Phone\XUPT_LIB\Xupt_lib\LoginPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BF992272262D8C9EEC64A7875138B034"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18051
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Xupt_lib {
    
    
    public partial class LoginPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton Confirm;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton Reset;
        
        internal Microsoft.Phone.Shell.ProgressIndicator load;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox ID;
        
        internal System.Windows.Controls.PasswordBox Password;
        
        internal System.Windows.Controls.CheckBox Remember;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Xupt_lib;component/LoginPage.xaml", System.UriKind.Relative));
            this.Confirm = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("Confirm")));
            this.Reset = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("Reset")));
            this.load = ((Microsoft.Phone.Shell.ProgressIndicator)(this.FindName("load")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ID = ((System.Windows.Controls.TextBox)(this.FindName("ID")));
            this.Password = ((System.Windows.Controls.PasswordBox)(this.FindName("Password")));
            this.Remember = ((System.Windows.Controls.CheckBox)(this.FindName("Remember")));
        }
    }
}

