using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Model
{
    public class JSCodeStruct : INotifyPropertyChanged
    {
        private string id;
        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get { return id; }
            set 
            { 
                id = value;
                RaiseChange("ID");
            }
        }

        private string JSCode;
        /// <summary>
        /// JavaScript代码
        /// </summary>
        public string JSCODE
        {
            get { return JSCode; }
            set 
            { 
                JSCode = value;
                RaiseChange("JSCODE");
            }
        }

        private string status;
        /// <summary>
        /// 状态
        /// </summary>
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
