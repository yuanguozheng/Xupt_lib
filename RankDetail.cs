using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace Xupt_lib
{
    public class RankItemBase : INotifyPropertyChanged
    {
        private string _Rank;
        public string Rank
        {
            get
            {
                return "排名：" + _Rank;
            }
            set
            {
                if (_Rank != value)
                    _Rank = value;
                NotifyPropertyChanged("Rank");
            }
        }

        private string _Times;
        public string Times
        {
            get
            {
                return "次数：" + _Times;
            }
            set
            {
                if (_Times != value)
                    _Times = value;
                NotifyPropertyChanged("Times");
            }
        }

        private Visibility _isSearch;
        public Visibility isSearch
        {
            get
            {
                return _isSearch;
            }
            set
            {
                if (_isSearch != value)
                    _isSearch = value;
                NotifyPropertyChanged("isSearch");
            }
        }

        public Visibility isOther
        {
            get
            {
                if (_isSearch == Visibility.Visible)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        private string _Message;
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                if (_Message != value)
                    _Message = value;
                NotifyPropertyChanged("Message");
            }
        } 

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class RankSec : RankItemBase
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
                if (_ID != value)
                {
                    _ID = value;
                    Message = _ID;
                }
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
                if (_Title != value)
                    _Title = value;
                NotifyPropertyChanged("Title");
            }
        }
    }
    public class RentRank : RankSec
    {
        private string _Sort;
        public string Sort
        {
            get
            {
                return "分类号：" + _Sort;
            }
            set
            {
                if (_Sort != value)
                    _Sort = value;
                NotifyPropertyChanged("Sort");
            }
        }
    }
    public class SearchRank : RankItemBase
    {
        private string _Keyword;
        public string Keyword
        {
            get
            {
                return _Keyword;
            }
            set
            {
                if (_Keyword != value)
                {
                    _Keyword = value;
                    Message = _Keyword;
                }
                NotifyPropertyChanged("Keyword");
            }
        }
    }
    public class FavRank : RankSec { }
    public class LookRank : RankSec { }
    public class RankItem : INotifyPropertyChanged
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
                if (_ID != value)
                    _ID = value;
                NotifyPropertyChanged("ID");
            }
        }

        private string _Keyword;
        public string Keyword
        {
            get
            {
                return _Keyword;
            }
            set
            {
                if (_Keyword != value)
                    _Keyword = value;
                NotifyPropertyChanged("Keyword");
            }
        }

        private string _Sort;
        public string Sort
        {
            get
            {
                return "分类号：" + _Sort;
            }
            set
            {
                if (_Sort != value)
                    _Sort = value;
                NotifyPropertyChanged("Sort");
            }
        }
        private string _Rank;
        public string Rank
        {
            get
            {
                return "排名：" + _Rank;
            }
            set
            {
                if (_Rank != value)
                    _Rank = value;
                NotifyPropertyChanged("Rank");
            }
        }

        private string _Times;
        public string Times
        {
            get
            {
                return "次数：" + _Times;
            }
            set
            {
                if (_Times != value)
                    _Times = value;
                NotifyPropertyChanged("Times");
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
                if (_Title != value)
                    _Title = value;
                NotifyPropertyChanged("Title");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class RankInfo
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public int Size { get; set; }

        public ObservableCollection<object> Items { get; set; }
    }
}
