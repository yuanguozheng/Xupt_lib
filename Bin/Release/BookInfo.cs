using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Xupt_lib
{
    public class BookInfo : INotifyPropertyChanged
    {
        private string _id;
        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                    _id = value;
                NotifyPropertyChanged("id");
            }
        }
        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                    _name = value;
                NotifyPropertyChanged("name");
            }
        }
        private string _barcode;
        public string barcode {
            get
            {
                return "书号："+_barcode;
            }
            set
            {
                if (value != _barcode)
                    _barcode = value;
                NotifyPropertyChanged("barcode");
            }
        }
        private string _state;
        public string state
        {
            get
            {
                return _state;
            }
            set
            {
                if (value != _state)
                    _state = value;
                NotifyPropertyChanged("state");
            }
        }
        public string _date;
        public string date
        {
            get
            {
                return "到期："+_date;
            }
            set
            {
                if (value != _date)
                    _date = value;
                NotifyPropertyChanged("date");
            }
        }
        public string _isRenew;
        public string isRenew
        {
            get
            {
                return _isRenew;
            }
            set
            {
                if (value != _isRenew)
                    _isRenew = value;
                NotifyPropertyChanged("isRenew");
            }
        }
        private string _department_id;
        public string department_id
        {
            get
            {
                return _department_id;
            }
            set
            {
                if (value != _department_id)
                    _department_id = value;
                NotifyPropertyChanged("department_id");
            }
        }
        private string _library_id;
        public string library_id
        {
            get
            {
                return _library_id;
            }
            set
            {
                if (value != _library_id)
                    _library_id = value;
                NotifyPropertyChanged("library_id");
            }
        }
        private double _height;
        public double height
        {
            get
            {
                return _height;
            }
            set
            {
                if (value != _height)
                    _height = value;
                NotifyPropertyChanged("height");
            }
        }

        public string state_s
        {
            get
            {
                string[] tmp = Regex.Split(_date, "/");
                DateTime LastDate = new DateTime(Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]));
                
                if (_isRenew == "True")
                {
                    if (LastDate < DateTime.Now)
                    {
                        return "已超期";
                    }
                    return "可续借";
                }
                else if (LastDate.Date == DateTime.Now.Date)
                {
                    return "今天到期";
                }
                else if (LastDate < DateTime.Now)
                {
                    return "已超期";
                }
                else
                {
                    return "不可续借";
                }
            }
        }
        public string bookdetail
        {
            get
            {
                string[] tmp = Regex.Split(_date, "/");
                DateTime LastDate = new DateTime(Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]));
                if (LastDate < DateTime.Now)
                {
                    return "完整书名：" + _name + "\n" + "此书已超期：" + Math.Abs((LastDate.Date - DateTime.Now.Date).Days) + " 天" + "\n" + "欠费金额：￥" + Math.Abs((LastDate.Date - DateTime.Now.Date).Days) * 0.5;
                }
                if (du <= 3)
                {
                    return "完整书名：" + _name+"\n"+"温馨提示：此书即将到期！请尽快续借！";
                }
                return "完整书名：" + _name;
            }
        }
        private Visibility _detailvis;
        public Visibility detailvis
        {
            get
            {
                return _detailvis;
            }
            set
            {
                if (value != _detailvis)
                    _detailvis = value;
                NotifyPropertyChanged("detailvis");
            }
        }
        int du = 0;
        public string Color
        {
            get
            {
                string[] tmp = Regex.Split(_date, "/");
                DateTime LastDate = new DateTime(Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]));
                du = (LastDate.Date - DateTime.Now.Date).Days;
                if (du <= 3 && du > 1)
                {
                    return "#FFFFAE00";
                }
                else if (du <= 1)
                {
                    return "Red";
                }
                else
                {
                    return "White";
                }
            }
        }
        public int Duration
        {
            get
            {
                string[] tmp = Regex.Split(_date, "/");
                DateTime LastDate = new DateTime(Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]));
                return (LastDate - DateTime.Now).Days;
            }
        }
        public RenewInfoClass RenewInfo
        {
            get
            {
                return new RenewInfoClass
                {
                    Duration = Duration,
                    Enable = _isRenew,
                    depart=_department_id,
                    lib=_library_id,
                    barcode=_barcode,
                    id=_id
                };
            }
        }
        public Visibility RenewButton
        {
            get
            {
                switch (_isRenew)
                {
                    case "True": return Visibility.Visible;
                    case "False": return Visibility.Collapsed;
                    default: return Visibility.Collapsed;
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
    public class RenewInfoClass
    {
        public string id
        {
            get;
            set;
        }
        public string Enable
        {
            get;
            set;
        }
        public int Duration
        {
            get;
            set;
        }
        public string barcode
        {
            get;
            set;
        }
        public string depart
        {
            get;
            set;
        }
        public string lib
        {
            get;
            set;
        }
    }
}
