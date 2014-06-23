using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace Xupt_lib
{
    public class SearchList : INotifyPropertyChanged
    {
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
                {
                    _ID = value;
                }
                NotifyPropertyChanged("ID");
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                if (_Name == "" || _Name == null)
                    return "<未知书名>";
                return _Name;
            }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                }
                NotifyPropertyChanged("Name");
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
                {
                    _Author = value;
                }
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
                {
                    _Pub = value;
                }
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
                {
                    _ISBN = value;
                }
                NotifyPropertyChanged("ISBN");
            }
        }

        private string _Year;
        public string Year
        {
            get
            {
                return _Year;
            }
            set
            {
                if (value != _Year)
                {
                    _Year = value;
                }
                NotifyPropertyChanged("Year");
            }
        }

        private string _ShowContent;
        public string ShowContent
        {
            get
            {
                return _ShowContent;
            }
            set
            {
                if (value != _ShowContent)
                {
                    _ShowContent = value;
                }
                NotifyPropertyChanged("ShowContent");
            }
        }

        private string _IsEnable;
        public string IsEnable
        {
            get 
            {
                return _IsEnable;
            }
            set
            {
                if (value != _IsEnable)
                    _IsEnable = value;
                NotifyPropertyChanged("IsEnable");
            }
        }

        private Visibility _BookItem;
        public Visibility BookItem
        {
            get
            {
                return _BookItem;
            }
            set
            {
                if (value != _BookItem)
                    _BookItem = value;
                NotifyPropertyChanged("BookItem");
            }
        }

        private Visibility _LoadingMore;
        public Visibility LoadingMore
        {
            get
            {
                return _LoadingMore;
            }
            set
            {
                if (value != _LoadingMore)
                    _LoadingMore = value;
                NotifyPropertyChanged("LoadingMore");
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
