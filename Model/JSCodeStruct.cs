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

        private int No;
        /// <summary>
        /// 编号
        /// </summary>
        public int NO
        {
            get { return No; }
            set
            {
                No = value;
                RaiseChange("No");
            }
        }

        private int id;
        /// <summary>
        /// 编号
        /// </summary>
        public int ID
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

        private int status;
        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get { return status; }
            set 
            {
                status = value;
                RaiseChange("Status");
            }
        }

        private string jSCountCode;
        /// <summary>
        /// 模式
        /// </summary>
        public string JSCountCode
        {
            get { return jSCountCode; }
            set 
            {
                jSCountCode = value;
                RaiseChange("JSCountCode");
            }
        }

        private string associatedWebAddress;
        /// <summary>
        /// 关联网址
        /// </summary>
        public string AssociatedWebAddress
        {
            get { return associatedWebAddress; }
            set 
            {
                associatedWebAddress = value;
                RaiseChange("AssociatedWebAddress");
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
