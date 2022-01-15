using Crawler.Model;
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
            _webView2Struct.Result.Clear();
            HtmlSource.Text = "";
            //普通的img标签
            object i = await webView.CoreWebView2.ExecuteScriptAsync("document.getElementsByTagName('img').length");
            int total = int.Parse(i.ToString());
            for (int num = 1; num < total; num++)
            {
                //获取图片链接
                object o = await webView.CoreWebView2.ExecuteScriptAsync($"document.getElementsByTagName(\'img\').item({num}).src");
                HtmlSource.Text += o.ToString().Replace("\"","") + "\n";
                _webView2Struct.Result.Add(num, o.ToString().Replace("\"", ""));
            }
            //MessageBox.Show(await GetWebSiteAsync());
        }


        ///<summary>
        ///链接本地数据库：读取JavaScriptCommand表
        ///</summary>
        public void SqlRead()
        {
            SqlConnection sqlConnection = new SqlConnection(databaseLink);

            try
            {
                using (sqlConnection)
                {
                    sqlConnection.Open();
                    Console.WriteLine("Open Database Connect Success!");
                    using (SqlCommand SqlCommand = sqlConnection.CreateCommand())
                    {
                        SqlCommand.CommandText = "SELECT * FROM JavaScriptCommand";
                        SqlCommand.ExecuteNonQuery();
                        SqlDataReader sqlDataReader = SqlCommand.ExecuteReader();
                        try
                        {
                            jSCodeStructs.Clear();
                            if (sqlDataReader.HasRows)
                            {
                                int cat = 1;
                                while (sqlDataReader.Read())
                                {
                                    JSCodeStruct jSCodeStruct = new JSCodeStruct()
                                    {
                                        NO = cat++,
                                        ID = sqlDataReader.GetInt32(0),
                                        JSCODE = sqlDataReader["JSCode"].ToString(),
                                        Status = sqlDataReader.GetInt32(2)
                                    };
                                    jSCodeStructs.Add(jSCodeStruct);
                                }


                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                MessageBox.Show(exc.Message);
            }
            finally
            {
                sqlConnection.Close();

            }
        }


        private async void WebView_GetLinkByDiv(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            _webView2Struct.Result.Clear();
            HtmlSource.Text = "";
            //普通的img标签
            object i = await webView.CoreWebView2.ExecuteScriptAsync("document.getElementsByTagName('div').length");
            int total = int.Parse(i.ToString());
            for (int num = 1; num < total; num++)
            {
                //获取图片链接
                object o = await webView.CoreWebView2.ExecuteScriptAsync($"document.getElementsByTagName(\'img\').item({num}).src");
                HtmlSource.Text += o.ToString().Replace("\"", "") + "\n";
                _webView2Struct.Result.Add(num, o.ToString().Replace("\"", ""));
            }

        }

        /// <summary>
        /// 灵机一动：用方法来取数据
        /// </summary>
        /// <returns></returns>
        public WebView2Struct GetWebView2Struct() { return _webView2Struct; }



        public async Task<string> GetWebSiteAsync() => await webView.CoreWebView2.ExecuteScriptAsync("document.location.href");
    }

}
