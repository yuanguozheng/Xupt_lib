using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Xupt_lib
{
    public class FavList : INotifyPropertyChanged
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

        private string _Sort;
        public string Sort
        {
            get
            {
                return "索书号：" + _Sort;
            }
            set
            {
                if (value != _Sort)
                    _Sort = value;
                NotifyPropertyChanged("Sort");
            }
        }

        private string _Img;
        public string Img
        {
            get
            {
                if (_Img.Contains("book-default"))
                    return "/book.png";
                return _Img;
            }
            set
            {
                if (value != _Img)
                    _Img = value;
                NotifyPropertyChanged("Img");
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
}
