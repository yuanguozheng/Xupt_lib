using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Xupt_lib
{
    public class SearchDetail : INotifyPropertyChanged
    {
        private string _CtrlID;
        public string CtrlID
        {
            get
            {
                return _CtrlID;
            }
            set
            {
                if (value != _CtrlID)
                {
                    _CtrlID = value;
                }
                NotifyPropertyChanged("CtrlID");
            }
        }

        private string _ID;
        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value != _ID)
                    _ID = value;
                NotifyPropertyChanged("ID");
            }
        }

        private string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (value != _Title)
                    _Title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private string _Author;
        public string Author
        {
            get
            {
                return _Author;
            }
            set
            {
                if (value != _Author)
                    _Author = value;
                NotifyPropertyChanged("Author");
            }
        }

        private string _Pub;
        public string Pub
        {
            get
            {
                return _Pub;
            }
            set
            {
                if (value != _Pub)
                    _Pub = value;
                NotifyPropertyChanged("Pub");
            }
        }

        private string _ISBN;
        public string ISBN
        {
            get
            {
                return _ISBN;
            }
            set
            {
                if (value != _ISBN)
                    _ISBN = value;
                NotifyPropertyChanged("ISBN");
            }
        }

        private string _image_l;
        public string image_l
        {
            get
            {
                return _image_l;
            }
            set
            {
                if (value != _image_l)
                    _image_l = value;
                NotifyPropertyChanged("image_l");
            }
        }

        private string _image_m;
        public string image_m
        {
            get
            {
                return _image_m;
            }
            set
            {
                if (value != _image_m)
                    _image_m = value;
                NotifyPropertyChanged("image_m");
            }
        }

        private string _image_s;
        public string image_s
        {
            get
            {
                return _image_s;
            }
            set
            {
                if (value != _image_s)
                    _image_s = value;
                NotifyPropertyChanged("image_s");
            }
        }

        private string _Summary;
        public string Summary
        {
            get
            {
                return _Summary;
            }
            set
            {
                if (value != _Summary)
                    _Summary = value;
                NotifyPropertyChanged("Summary");
            }
        }

        private string _Pages;
        public string Pages
        {
            get
            {
                return _Pages;
            }
            set
            {
                if (value != _Pages)
                    _Pages = value;
                NotifyPropertyChanged("Pages");
            }
        }

        private string _Price;
        public string Price
        {
            get
            {
                return _Price;
            }
            set
            {
                if (value != _Price)
                    _Price = value;
                NotifyPropertyChanged("Price");
            }
        }

        public string _Link;
        public string Link
        {
            get
            {
                return _Link;
            }
            set
            {
                if (value != _Link)
                    _Link = value;
                NotifyPropertyChanged("Link");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class BookCirculation : INotifyPropertyChanged
    {
        private string _Barcode;
        public string Barcode
        {
            get
            {
                return _Barcode;
            }
            set
            {
                if (value != _Barcode)
                    _Barcode = value;
                NotifyPropertyChanged("Barcode");
            }
        }
        private string _Department;
        public string Department
        {
            get
            {
                return _Department;
            }
            set
            {
                if (value != _Department)
                    _Department = value;
                NotifyPropertyChanged("Department");
            }
        }
        private string _State;
        public string State
        {
            get
            {
                return _State;
            }
            set
            {
                if (value != _State)
                    _State = value;
                NotifyPropertyChanged("State");
            }
        }
        private string _Date;
        public string Date
        {
            get
            {
                if (_Date != "")
                    return "应还日期：" + _Date;
                else
                    return "";
            }
            set
            {
                if (value != _Date)
                    _Date = value;
                NotifyPropertyChanged("Date");
            }
        }

        public string Color
        {
            get
            {
                if (_State == "在架可借")
                {
                    return "#FF007EC6";
                }
                else
                {
                    return "#FFC69000";
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
