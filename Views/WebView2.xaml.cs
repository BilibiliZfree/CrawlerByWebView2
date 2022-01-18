using Crawler.DataAccessObject;
using Crawler.Model;
using Crawler.Util;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace Crawler.Views
{
    /// <summary>
    /// WebView2.xaml 的交互逻辑
    /// </summary>
    public partial class WebView2 : Window
    {
        public WebView2Struct _webView2Struct = new WebView2Struct() { Result= new Dictionary<int, string>() };

        /// <summary>
        /// 存Javascript命令列表
        /// </summary>
        ObservableCollection<JSCodeStruct> jSCodeStructs = new ObservableCollection<JSCodeStruct>();

        const string databaseLink = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\98546\\Documents\\LocalSql\\Databases\\Crawler.mdf;Integrated Security=True;Connect Timeout=30";


        public WebView2()
        {
            InitializeComponent();
            webView.Source = _webView2Struct.Source;
            //_webView2Struct.IsWork = true;
            
        }

        private async void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            //_webView2Struct.Result.Clear();
            //HtmlSource.Text = "";
            //普通的img标签
            //object i = await webView.CoreWebView2.ExecuteScriptAsync("document.getElementsByTagName('img').length");
            //int total = int.Parse(i.ToString());
            //for (int num = 1; num < total; num++)
            //{
            //    //获取图片链接
            //    object o = await webView.CoreWebView2.ExecuteScriptAsync($"document.getElementsByTagName(\'img\').item({num}).src");
            //    HtmlSource.Text += o.ToString().Replace("\"","") + "\n";
            //    _webView2Struct.Result.Add(num, o.ToString().Replace("\"", ""));
            //}
            //MessageBox.Show(await GetWebSiteAsync());
            UpdateJSCodeStructs();
            await GetLinkByJavascript();
        }


        private async void WebView_GetLinkByDiv(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            //_webView2Struct.Result.Clear();
            //HtmlSource.Text = "";
            ////普通的img标签
            //object i = await webView.CoreWebView2.ExecuteScriptAsync("document.getElementsByTagName('div').length");
            //int total = int.Parse(i.ToString());
            //for (int num = 1; num < total; num++)
            //{
            //    //获取图片链接
            //    object o = await webView.CoreWebView2.ExecuteScriptAsync($"document.getElementsByTagName(\'img\').item({num}).src");
            //    HtmlSource.Text += o.ToString().Replace("\"", "") + "\n";
            //    _webView2Struct.Result.Add(num, o.ToString().Replace("\"", ""));
            //}
            UpdateJSCodeStructs();
            await GetLinkByJavascript();

        }

        /// <summary>
        /// 更新JSCodeStructs列表
        /// </summary>
        private void UpdateJSCodeStructs()
        {
            jSCodeStructs.Clear();
            foreach (var item in new SqlServerCURD().QueryAllData(new ObservableCollection<JSCodeStruct>(), databaseLink))
            {
                jSCodeStructs.Add(item);
            }
        }


        private async Task GetLinkByJavascript()
        {
            _webView2Struct.Result.Clear();
            HtmlSource.Text = "";
            int length = jSCodeStructs.Count;
            for (int i = 0; i < length; i++)
            {
                string website = await webView.CoreWebView2.ExecuteScriptAsync("document.location.href");
                bool flag = website.Substring(1, website.Length - 2) == jSCodeStructs[i].AssociatedWebAddress;
                if (flag)
                {
                    Console.WriteLine(website.Substring(1, website.Length - 2));
                    Console.WriteLine(jSCodeStructs[i].AssociatedWebAddress);
                    Console.WriteLine(flag);
                    object o1 = await webView.CoreWebView2.ExecuteScriptAsync(jSCodeStructs[i].JSCountCode);
                    Console.WriteLine(jSCodeStructs[i].JSCountCode);
                    int total = int.Parse(o1.ToString());
                    for (int num = 1; num < total; num++)
                    {
                        //获取图片链接
                        object o2 = await webView.CoreWebView2.ExecuteScriptAsync(jSCodeStructs[i].JSCODE.Replace("{num}", num.ToString()));
                        //Console.WriteLine(jSCodeStructs[i].JSCODE);
                        if (RegexUtil.IsUrl(o2.ToString().Replace("\"", "")))
                        {
                            HtmlSource.Text += o2.ToString().Replace("\"", "") + "\n";
                            _webView2Struct.Result.Add(num, o2.ToString().Replace("\"", ""));
                        }
                    }
                }
                
            }
            
        }


        /// <summary>
        /// 灵机一动：用方法来取数据
        /// </summary>
        /// <returns></returns>
        public WebView2Struct GetWebView2Struct() { return _webView2Struct; }


        /// <summary>
        /// 获取当前网页地址
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWebSiteAsync() => await webView.CoreWebView2.ExecuteScriptAsync("document.location.href");
    }

}
