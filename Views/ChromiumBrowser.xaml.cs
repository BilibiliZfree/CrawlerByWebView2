using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crawler.Views
{
    /// <summary>
    /// ChromiumBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class ChromiumBrowser : Window
    {
        /// <summary>
        /// 泛型，封装一个只有一个参数的的void方法，参数类型为string
        /// 例如：void A(string B);
        /// 使用时需要传入string参数
        /// loadEndCallBack(S)
        /// </summary>
        private Action<string> loadEndCallBack;

        public ChromiumBrowser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载网页事件方法
        /// </summary>
        /// <param name="url">链接地址</param>
        /// <param name="act">触发事件</param>
        public void GetHtmlSourceDynamic(string url, Action<string> act)
        {
            //如果不清除Address，网页不会再次加载，就不会触发browser_FrameLoadEnd事件
            if (browser.Address == url)
                browser.Address = "";
            browser.Address = url;
            loadEndCallBack = act;
        }

        /// <summary>
        /// 浏览器加载函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Browser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            string source = await browser.GetSourceAsync();

            /// if (loadEndCallBack != null)
            ///     loadEndCallBack(source);
            loadEndCallBack?.Invoke(source);
        }
    }
}
