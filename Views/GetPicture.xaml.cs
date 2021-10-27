using Crawler.Model;
using Crawler.Util;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Crawler.Views
{
    /// <summary>
    /// GetPictrue.xaml 的交互逻辑
    /// </summary>
    public partial class GetPicture : Page
    {
        //readonly GlobalDataUtil globalDataUtil = new GlobalDataUtil();
        readonly GlobalDataUtil globalDataUtil = GlobalDataUtil.GetInstance();
        
        const string downloadPath = "downloadPath";
        const string settingFileName = "Setting.init";
        const string defaultDownloadDirectory = "Pictures";

        //锁对象
        readonly object obj = new object();

        //图片集合
        private readonly ObservableCollection<UrlStruct> imageCollection = new ObservableCollection<UrlStruct>();
        public string BaseUrl { get; set; }

        //用于在Setting.init文件中分隔名称和值的字符。
        const char SettingDivider = '@';
        public GetPicture()
        {
            InitializeComponent();
            ReadTheSetting();
            //控件资源绑定
            downloadImageMessage_ListView.ItemsSource = imageCollection;
        }

        /// <summary>
        /// 单选框逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CLick(object sender, RoutedEventArgs e)
        {
            //获取当前CheckBox对象
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                //获取checkBox所属DockPanel对象
                var dockPanel = VisualTreeHelper.GetParent(checkBox) as DockPanel;
                //获取当前CheckBox所在位置
                var index = dockPanel.Children.IndexOf(checkBox);
                //遍历所有选项把其他选项置为不勾选
                for (int i = 0; i < dockPanel.Children.Count; i++)
                {
                    //跳过本选项操作
                    if (i == index)
                        continue;

                    //其他置为不选择
                    (dockPanel.Children[i] as CheckBox).IsChecked = false;
                }
            }
            else
            {
                //保证当前选项为选择状态
                checkBox.IsChecked = true;
            }
        }

        #region 自定义设置文件读取

        /// <summary>
        /// 选为默认路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefaultDownloadPath_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (DownloadPathDefault_CheckBox.IsChecked == false)
            {
                DownloadPathDefault_CheckBox.IsChecked = true;
                ShowStatusText("更改目录请点击[选择下载目录]按钮.");
            }
            else
            {
                DownloadPathDefault_CheckBox.IsChecked = true;
                var downloadCurrentPath = $"{downloadPath}{SettingDivider}{PathTextBox.Text}";
                //File.WriteAllText(settingFileName, downloadCurrentPath);
                if (ChangeDefaultDownloadPath(downloadCurrentPath))
                {
                    ShowStatusText("已将下载目录更改为当前地址.");
                }

            }
        }

        private bool ChangeDefaultDownloadPath(string CurrentPath)
        {
            string line = null;
            List<string> part = new List<string>();
            bool noPath = true;
            var sr = new StreamReader(settingFileName);
            using (sr)
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("downloadPath@"))
                    {
                        line = $"{CurrentPath}";
                        noPath = false;
                    }
                    part.Add(line);
                }

            }
            sr.Close();
            File.WriteAllLines(settingFileName, part);
            if (noPath)
            {
                AppendDownloadPath();
                ReadDownLoadPath();
                noPath = false;
            }
            return !noPath;
        }

        /// <summary>
        /// 选择图片保存的路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectPathButton_Click(object sender, RoutedEventArgs e)
        {
            ShowStatusText("正在选择新目录...");
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog
            {
                SelectedPath = PathTextBox.Text
            };
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                PathTextBox.Text = folderBrowserDialog.SelectedPath;
                DownloadPathDefault_CheckBox.IsChecked = false;
                ShowStatusText("新目录已获取.");
            }
        }

        /// <summary>
        /// 读取设置文件
        /// </summary>
        private void ReadTheSetting()
        {

            //判断是否存在Setting.init文件
            if (File.Exists(settingFileName))
            {
                ReadDownLoadPath();
                if (!Directory.Exists(defaultDownloadDirectory))
                {
                    Directory.CreateDirectory(defaultDownloadDirectory);
                }
            }
            else
            {
                AppendDownloadPath();
                ReadDownLoadPath();
                if (!Directory.Exists(defaultDownloadDirectory))
                {
                    Directory.CreateDirectory(defaultDownloadDirectory);
                }
            }
        }

        /// <summary>
        /// 读取Setting.init文件中图片下载路径
        /// </summary>
        private void ReadDownLoadPath()
        {
            string line = null;
            string[] part;
            bool noPath = true;
            using (var sr = new StreamReader(settingFileName))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    //将行拆分为分隔符(名称)之前的部分和分隔符之后的部分(值)并组合成字符数组。
                    part = line.Split(SettingDivider);
                    //获取图片下载地址
                    switch (part[0])
                    {
                        case downloadPath:
                            PathTextBox.Text = part[1];
                            noPath = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            //如果在设置文件里找不到路径属性
            if (noPath)
            {
                AppendDownloadPath();
                ReadDownLoadPath();
            }
            DownloadPathDefault_CheckBox.IsChecked = true;
        }

        /// <summary>
        /// 追加下载路径属性
        /// </summary>
        private void AppendDownloadPath()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            string[] downloadCurrentPath = { $"{downloadPath}{SettingDivider}{currentDirectory}{Path.DirectorySeparatorChar}{defaultDownloadDirectory}" };
            File.AppendAllLines(settingFileName, downloadCurrentPath);
            ShowStatusText("下载路径属性已追加.");
        }

        #endregion

        #region 图片爬取逻辑

        /// <summary>
        /// 解析网页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HtmlParser_Button_Click(object sender, RoutedEventArgs e)
        {
            //获取url
            string url = urlTextBox.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                ShowStatusText("请输入URL");
                return;
            }
            //如果使用HttpWebRequest模式需要进行url格式判定
            if (HttpWebRequest_CheckBox.IsChecked == true)
            {
                //不是网址
                if (RegexUtil.IsUrl(url) == false)
                {
                    ShowStatusText("网址输入有误.");
                    return;
                }
            }
            //移除imageCollection控件中元素
            imageCollection.Clear();
            BaseUrl = UrlUtil.FixUrl(url);
            HtmlParser(url);
        }

        public void HtmlParser(string url)
        {
            ShowStatusText($"正在从 {url} 抓取图像.");
            if (CEF_CheckBox.IsChecked == true)
            {
                ParserByCEF(url);
            }
            else
            {
                ParseByHttpWebRequest(url);
            }
        }
        /// <summary>
        /// 使用CEF方法
        /// </summary>
        /// <param name="url">网页链接</param>
        private void ParserByCEF(string url)
        {
            
            if (HtmlAgilityPack_CheckBox.IsChecked == true)
            {
                globalDataUtil.Browser.GetHtmlSourceDynamic(url, ExtractImageWithHtmlAgilityPack);
                //new GlobalDataUtil().Browser.GetHtmlSourceDynamic(url, ExtractImageWithHtmlAgilityPack);
            }
            else
            {
                globalDataUtil.Browser.GetHtmlSourceDynamic(url, ExtractImageWithRegex);
                //new GlobalDataUtil().Browser.GetHtmlSourceDynamic(url, ExtractImageWithRegex);
            }

        }

        private async void ParseByHttpWebRequest(string url)
        {
            try
            {
                string html = await WebUtil.GetHtmlSource(url);
                if (HtmlAgilityPack_CheckBox.IsChecked == true)
                {
                    ExtractImageWithHtmlAgilityPack(html);
                }
                else
                {
                    ExtractImageWithRegex(html);
                }
            }
            catch (Exception ex)
            {
                ShowStatusText(ex.Message);
            }
        }

        private void ExtractImageWithRegex(string html)
        {
            try
            {
                string urlValue = "";
                string titleValue = "";
                MatchCollection matchCollection = RegexUtil.Matches(html, RegexPattern.TagImgPattern);
                for (int i = 0; i < matchCollection.Count; i++)
                {
                    urlValue = matchCollection[i].Groups["images"].Value;
                    if (urlValue.Contains("//") == false)
                    {
                        urlValue = BaseUrl + urlValue;
                    }
                    titleValue = UrlUtil.GetImageFromUrl(urlValue);
                    AddToCollection(new UrlStruct() { Id = i + 1, Status = "", Title = titleValue, Url = urlValue });
                }
                ShowStatusText($"已抓取到{imageCollection.Count}个图像.");
            }
            catch (Exception ex)
            {
                ShowStatusText(ex.Message);
            }
        }

        private async void ExtractImageWithHtmlAgilityPack(string html)
        {
            try
            {
                string urlValue = "";
                string titleValue = "";
                var imageList = await HtmlAgilityPackUtil.GetImgFromHtmlAsync(html);
                for (int i = 0; i < imageList.Count; i++)
                {
                    urlValue = imageList[i];
                    //修正value地址
                    if (urlValue.StartsWith("//"))
                    {
                        urlValue = "http:" + urlValue;
                    }
                    if (urlValue.Contains(":") == false)
                    {
                        urlValue = BaseUrl + urlValue;
                    }
                    titleValue = UrlUtil.GetImageFromUrl(urlValue);
                    AddToCollection(new UrlStruct() { Id = i + 1, Status = "", Title = titleValue, Url = urlValue });
                }
                ShowStatusText($"已抓取到{imageCollection.Count}个图像.");
            }
            catch (Exception ex)
            {
                ShowStatusText(ex.Message);
            }
        }

        public void AddToCollection(UrlStruct urlStruct)
        {
            //线程锁，防止多线程同时操作List数据导致数据异常
            lock (obj)
            {
                //查询imageCollection中有没有和urlStruct地址相同的数据
                var result = imageCollection.Where(x => x.Url == urlStruct.Url).FirstOrDefault();
                //不为空就返回
                if (result != null)
                    return;
                //不是图片链接便返回
                if (RegexUtil.IsInvalidImgUrl(urlStruct.Url) == false)
                    return;

                //如果是图片则调度imageCollection动态数据集合添加方法
                Dispatcher.Invoke(() => {
                    imageCollection.Add(urlStruct);
                });
            }
        }
        /// <summary>
        /// 点击列表中图片链接显示图片及其信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadImageMessage_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = downloadImageMessage_ListView.SelectedIndex;
            if (index != -1)
            {
                try
                {
                    string imgURL = imageCollection[index].Url;
                    //加载图片
                    ImageShow.Source = new BitmapImage(new Uri(imgURL));
                    ImageId.Content = imageCollection[index].Title;
                    ImageUrl.Content = imageCollection[index].Url;
                }
                catch (Exception ex)
                {
                    ShowStatusText(ex.Message);
                }
            }
        }

        private void DownloadImage_Button_Click(object sender, RoutedEventArgs e)
        {
            DownloadImage(imageCollection);
        }

        private void DownloadImage(ObservableCollection<UrlStruct> urls)
        {
            try
            {
                if (urls.Count == 0)
                {
                    ShowStatusText("找不到可下载的图片，请重新解析网页。");
                }
                else
                {
                    Network network = new Network();
                    foreach (var url in urls)
                    {
                        if (RegexUtil.IsInvalidImgUrl(url.Url))
                        {
                            network.DownloadFile(url.Url, $"{ PathTextBox.Text}{Path.DirectorySeparatorChar}{url.Title}");
                            ShowStatusText($"图片下载进度：{url.Id}/{urls.Count}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowStatusText(ex.Message);
            }
            finally
            {
                ShowStatusText("下载完成！");
            }

        }

        #endregion

        #region 公共
        public void ShowStatusText(string content)
        {
            this.Dispatcher.Invoke(() => {
                this.MessageLabel.Content = content;
            });
        }

        public void GlobalDataClose()
        {
            if (globalDataUtil != null)
                globalDataUtil.Browser.Close();
        }
        #endregion
    }
}
