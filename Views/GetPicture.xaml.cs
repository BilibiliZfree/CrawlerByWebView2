using Crawler.Model;
using Crawler.Util;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace Crawler.Views
{
    /// <summary>
    /// GetPicture.xaml 的交互逻辑
    /// </summary>
    public partial class GetPicture : Page
    {
        const string settingFileName = "Setting.init";
        const string downloadPath = "downloadPath";
        const string Url = "Url";
        const string DefaultUrl = "https://www.bilibili.com/";
        const string defaultDownloadDirectory = "Pictures";

        //用于在Setting.init文件中分隔名称和值的字符。
        const char SettingDivider = '$';

        WebView2 webView = new WebView2();

        //锁对象
        object obj = new object();
        //图片集合
        ObservableCollection<UrlStruct> imageCollection = new ObservableCollection<UrlStruct>();
        public GetPicture()
        {
            InitializeComponent();
            webView.Show();
            ReadAttributes();

            //控件资源绑定
            ImageMessage_ListView.ItemsSource = imageCollection;
        }

        #region Click

        /// <summary>
        /// 加载网页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            webView.webView.Source = new Uri(_UrlTextBox.Text);

        }

        /// <summary>
        /// 获取图片链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetImage_Click(object sender, RoutedEventArgs e)
        {
            imageCollection.Clear();
            foreach (var item in webView.GetWebView2Struct().Result)
            {
                AddToCollection(new UrlStruct() { Id = item.Key, IamgeName = "", Link = item.Value, Status = "已获取" });
            }
        }


        private void DownloadOne_Click(object sender, RoutedEventArgs e)
        {
            if (imageCollection.Count<=0)
            {
                MessageBox.Show("下载失败！\n原因：列表中没有图片");
                return;
            }
            int index = ImageMessage_ListView.SelectedIndex;
            UrlStruct urlStruct = imageCollection[index];
            DownloadOneImage(urlStruct);
        }

        private void DownloadAll_Click(object sender, RoutedEventArgs e)
        {
            if (imageCollection.Count <= 0)
            {
                MessageBox.Show("下载失败！\n原因：列表中没有图片");
                return;
            }
            DownloadAllImage(imageCollection);
        }

        /// <summary>
        /// 设定页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JavascriptSetting_Click(object sender, RoutedEventArgs e)
        {
            JavascriptSettings otherSettings = new JavascriptSettings();
            otherSettings.ShowDialog();
        }


        #endregion


        /// <summary>
        /// 将链接数据添加到列表源里面
        /// </summary>
        /// <param name="urlStruct"></param>
        public void AddToCollection(UrlStruct urlStruct)
        {
            //线程锁，防止多线程同时操作List数据导致数据异常
            lock (obj)
            {
                //查询imageCollection中有没有和urlStruct地址相同的数据
                var result = imageCollection.Where(x => x.Link == urlStruct.Link).FirstOrDefault();
                //不为空就返回
                if (result != null)
                    return;
                //不是图片链接便返回
                if (RegexUtil.IsInvalidImgUrl(urlStruct.Link) == false)
                    return;
                urlStruct.IamgeName = GetImageName(urlStruct.Link);
                //如果是图片则调度imageCollection动态数据集合添加方法
                Dispatcher.Invoke(() => {
                    imageCollection.Add(urlStruct);
                });
            }
        }

        private string GetImageName(string link)
        {
            int index = link.LastIndexOf('/');
            if (index == -1)
            {
                return link;
            }
            else
            {
                return link.Substring(index + 1);
            }
        }
        /// <summary>
        /// 点击列表中图片链接显示图片及其信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageMessage_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ImageMessage_ListView.SelectedIndex;
            if (index != -1)
            {
                try
                {
                    _ShowImage.Source = new BitmapImage(new Uri(imageCollection[index].Link));
                    PictrueNameLabel.Content = imageCollection[index].IamgeName;
                    PictrueIDLabel.Content = imageCollection[index].Id;
                    PictrueLinkLabel.Content = imageCollection[index].Link;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        
        /// <summary>
        /// 下载单个图片
        /// </summary>
        /// <param name="urlStruct"></param>
        private void DownloadOneImage(UrlStruct urlStruct)
        {
            try
            {
                Network network = new Network();
                string destinationFileName = $"{ PathTextBox.Text}{Path.DirectorySeparatorChar}{urlStruct.IamgeName}";
                if (RegexUtil.IsInvalidImgUrl(urlStruct.Link))
                {
                    if (CoverExistaFile.IsChecked == true)
                        if (File.Exists(destinationFileName))
                            File.Delete(destinationFileName);
                    network.DownloadFile(urlStruct.Link, destinationFileName);
                }
            }
            catch (Exception ex) 
            {
                ShowStatusText(ex.Message);
            }
            finally
            {
                MessageBox.Show("下载完成！");
            }
        }

        /// <summary>
        /// 下载所有图片
        /// </summary>
        /// <param name="urls"></param>
        private void DownloadAllImage(ObservableCollection<UrlStruct> urls)
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
                        string destinationFileName = $"{ PathTextBox.Text}{Path.DirectorySeparatorChar}{url.IamgeName}";
                        if (RegexUtil.IsInvalidImgUrl(url.Link))
                        {
                            if (File.Exists(destinationFileName))
                                File.Delete(destinationFileName);
                            network.DownloadFile(url.Link, destinationFileName);
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
                MessageBox.Show("下载完成！");
            }

        }

        private void DefaultDownloadPath_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (DefaultDownloadPath_CheckBox.IsChecked == false)
            {
                DefaultDownloadPath_CheckBox.IsChecked = true;
                ShowStatusText("更改目录请点击[选择下载目录]按钮.");
            }
            else
            {
                DefaultDownloadPath_CheckBox.IsChecked = true;
                var downloadCurrentPath = $"{downloadPath}{SettingDivider}{PathTextBox.Text}";
                //File.WriteAllText(settingFileName, downloadCurrentPath);
                if (ChangeDefaultDownloadPath(downloadCurrentPath))
                {
                    ShowStatusText("已将下载目录更改为当前地址");
                }

            }
        }

        /// <summary>
        /// 选择图片保存的路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectPathButton_Click(object sender, RoutedEventArgs e)
        {
            ShowStatusText("正在选择新目录...");
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = PathTextBox.Text;
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                PathTextBox.Text = folderBrowserDialog.SelectedPath;
                DefaultDownloadPath_CheckBox.IsChecked = false;
                ShowStatusText("新目录已获取.");
            }
        }

        #region 设置文件读取
        /// <summary>
        /// 追加下载路径属性
        /// </summary>
        private void AppendDownloadPath()
        {
            if(File.Exists(settingFileName))
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                string[] downloadCurrentPath = { $"{downloadPath}{SettingDivider}{currentDirectory}{Path.DirectorySeparatorChar}{defaultDownloadDirectory}" };
                File.AppendAllLines(settingFileName, downloadCurrentPath);
                ShowStatusText("下载路径属性已追加.");
            }
            else
            {
                File.Create(settingFileName);
                AppendDownloadPath();
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
                    if (line.StartsWith("downloadPath$"))
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
                PathTextBox.Text = ReadAttribute("downloadPath");
                noPath = false;
            }
            return !noPath;
        }


        /// <summary>
        /// 追加下载路径属性
        /// </summary>
        private void AppendUrl()
        {
            if (File.Exists(settingFileName))
            {
                var Link = _UrlTextBox.Text;
                string[] Links = { $"{Url}{SettingDivider}{DefaultUrl}" };
                if (RegexUtil.IsUrl(Link)) 
                    Links[0] = $"{Url}{SettingDivider}{Link}";
                File.AppendAllLines(settingFileName, Links);
                ShowStatusText("链接属性已追加.");
            }
            else
            {
                File.Create(settingFileName);
                AppendDownloadPath();
            }

        }

        /// <summary>
        /// 读取Setting.init文件中的属性
        /// </summary>
        private string ReadAttribute(string attribute)
        {
            string line = null;
            string[] part;
            bool noResult = true;
            string result = null;
            if (string.IsNullOrEmpty(attribute)) { ShowStatusText("输入属性不能为空");  return null; }
            if (File.Exists(settingFileName)) 
            {
                //获取图片保存目录
                if (attribute == "downloadPath")
                {
                    using (var sr = new StreamReader(settingFileName))
                    {
                        //获取图片下载地址
                        while ((line = sr.ReadLine()) != null)
                        {
                            //将行拆分为分隔符(名称)之前的部分和分隔符之后的部分(值)并组合成字符数组。
                            part = line.Split(SettingDivider);
                            //获取图片下载地址
                            switch (part[0])
                            {
                                case downloadPath:
                                    result = part[1];
                                    noResult = false;
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                    //如果在设置文件里找不到路径属性
                    if (noResult)
                    {
                        AppendDownloadPath();
                        result = ReadAttribute(attribute);
                    }
                }

                //获取链接地址
                else if (attribute == "DefaultUrl")
                {
                    using (var sr = new StreamReader(settingFileName))
                    {
                        //获取图片下载地址
                        while ((line = sr.ReadLine()) != null)
                        {
                            //将行拆分为分隔符(名称)之前的部分和分隔符之后的部分(值)并组合成字符数组。
                            part = line.Split(SettingDivider);
                            //获取图片下载地址
                            switch (part[0])
                            {
                                case Url:
                                    result = part[1];
                                    noResult = false;
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                    //如果在设置文件里找不到路径属性
                    if (noResult)
                    {
                        AppendUrl();
                        result = ReadAttribute(attribute);
                    }
                }
            }
            else
            {
                File.Create(settingFileName);
                result = ReadAttribute(attribute);
            }
            return result;
        }

        public void ReadAttributes()
        {
            PathTextBox.Text = ReadAttribute("downloadPath");
            _UrlTextBox.Text = ReadAttribute("DefaultUrl");
        }


        #endregion


        #region 公共
        public void ShowStatusText(string content)
        {
            this.Dispatcher.Invoke(() => {
                this.MessageLabel.Content = content;
            });
        }

        #endregion

        
    }
}
