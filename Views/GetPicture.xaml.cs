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
        WebView2 webView = new WebView2();

        //锁对象
        object obj = new object();
        //图片集合
        ObservableCollection<UrlStruct> imageCollection = new ObservableCollection<UrlStruct>();
        public GetPicture()
        {
            InitializeComponent();
            webView.Show();

            //控件资源绑定
            ImageMessage_ListView.ItemsSource = imageCollection;
        }

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
                    string imgURL = imageCollection[index].Link;
                    _ShowImage.Source = new BitmapImage(new Uri(imgURL));
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void DownloadOne_Click(object sender, RoutedEventArgs e)
        {
            int index = ImageMessage_ListView.SelectedIndex;
            UrlStruct urlStruct = imageCollection[index];
            DownloadOneImage(urlStruct);
        }

        private void DownloadAll_Click(object sender, RoutedEventArgs e)
        {
            DownloadAllImage(imageCollection);
        }

        private void DownloadOneImage(UrlStruct urlStruct)
        {
            try
            {
                Network network = new Network();
                if (RegexUtil.IsInvalidImgUrl(urlStruct.Link))
                {
                    network.DownloadFile(urlStruct.Link, $"{ PathTextBox.Text}{Path.DirectorySeparatorChar}{urlStruct.IamgeName}");
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
                        if (RegexUtil.IsInvalidImgUrl(url.Link))
                        {
                            network.DownloadFile(url.Link, $"{ PathTextBox.Text}{Path.DirectorySeparatorChar}{url.IamgeName}");
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
                DownloadPathDefault_CheckBox.IsChecked = false;
                ShowStatusText("新目录已获取.");
            }
        }

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
