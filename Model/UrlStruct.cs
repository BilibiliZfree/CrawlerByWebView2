using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Model
{
    /// <summary>
    /// 属性类
    /// </summary>
    public class UrlStruct : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                RaiseChange("Id");
            }
        }

        private string url;
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                RaiseChange("Url");
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaiseChange("Title");
            }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                RaiseChange("Status");
            }
        }

        /// <summary>
        /// 获取属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性改变时需要做出的反应
        /// </summary>
        /// <param name="property"></param>
        public void RaiseChange(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
