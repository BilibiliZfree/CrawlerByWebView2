using Crawler.Model;
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
    /// GetPicture.xaml 的交互逻辑
    /// </summary>
    public partial class GetPicture : Page
    {
        WebView2 webView = new WebView2();

        private WebView2Struct _WebViewStruct = new WebView2Struct() { Result = new Dictionary<int, string>() };
        public GetPicture()
        {
            InitializeComponent();
            webView.Show();
        }

        /// <summary>
        /// 加载网页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            webView.webView.Source = new Uri(_UrlTextBox.Text);
            _WebViewStruct = webView._webView2Struct;
        }
    }
}
