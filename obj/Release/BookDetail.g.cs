﻿#pragma checksum "D:\C#\Windows Phone\XUPT_LIB\Xupt_lib\BookDetail.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "723147683F6B94A96E98469659410636"
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
    
    
    public partial class BookDetail : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton AddFav;
        
        internal Microsoft.Phone.Shell.ProgressIndicator load;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.PivotItem p1;
        
        internal System.Windows.Controls.ListBox BasicInfo;
        
        internal System.Windows.Controls.Image Icon_m;
        
        internal System.Windows.Controls.ProgressBar loadingImg;
        
        internal System.Windows.Controls.TextBlock BookName;
        
        internal System.Windows.Controls.TextBlock BookId;
        
        internal System.Windows.Controls.TextBlock BookAuthor;
        
        internal System.Windows.Controls.TextBlock BookPages;
        
        internal System.Windows.Controls.TextBlock BookPrice;
        
        internal System.Windows.Controls.TextBlock BookPub;
        
        internal System.Windows.Controls.TextBlock BookISBN;
        
        internal System.Windows.Controls.TextBlock BookSummary;
        
        internal System.Windows.Controls.Image LarImg;
        
        internal System.Windows.Controls.ProgressBar LoadLarImg;
        
        internal System.Windows.Controls.ListBox Circulation;
        
        internal System.Windows.Controls.StackPanel Weibo;
        
        internal System.Windows.Controls.StackPanel Renren;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Xupt_lib;component/BookDetail.xaml", System.UriKind.Relative));
            this.AddFav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("AddFav")));
            this.load = ((Microsoft.Phone.Shell.ProgressIndicator)(this.FindName("load")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.p1 = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("p1")));
            this.BasicInfo = ((System.Windows.Controls.ListBox)(this.FindName("BasicInfo")));
            this.Icon_m = ((System.Windows.Controls.Image)(this.FindName("Icon_m")));
            this.loadingImg = ((System.Windows.Controls.ProgressBar)(this.FindName("loadingImg")));
            this.BookName = ((System.Windows.Controls.TextBlock)(this.FindName("BookName")));
            this.BookId = ((System.Windows.Controls.TextBlock)(this.FindName("BookId")));
            this.BookAuthor = ((System.Windows.Controls.TextBlock)(this.FindName("BookAuthor")));
            this.BookPages = ((System.Windows.Controls.TextBlock)(this.FindName("BookPages")));
            this.BookPrice = ((System.Windows.Controls.TextBlock)(this.FindName("BookPrice")));
            this.BookPub = ((System.Windows.Controls.TextBlock)(this.FindName("BookPub")));
            this.BookISBN = ((System.Windows.Controls.TextBlock)(this.FindName("BookISBN")));
            this.BookSummary = ((System.Windows.Controls.TextBlock)(this.FindName("BookSummary")));
            this.LarImg = ((System.Windows.Controls.Image)(this.FindName("LarImg")));
            this.LoadLarImg = ((System.Windows.Controls.ProgressBar)(this.FindName("LoadLarImg")));
            this.Circulation = ((System.Windows.Controls.ListBox)(this.FindName("Circulation")));
            this.Weibo = ((System.Windows.Controls.StackPanel)(this.FindName("Weibo")));
            this.Renren = ((System.Windows.Controls.StackPanel)(this.FindName("Renren")));
        }
    }
}

