using Crawler.Views;
using Crawler.DataAccessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crawler
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //获取屏幕高度和宽度
        static readonly double screenHeight = SystemParameters.WorkArea.Height;

        //static readonly double screenWidth = SystemParameters.PrimaryScreenWidth;

        GetPicture getPicture = new GetPicture();


        // ToggleButton定义
        ToggleButton toggleButton = null;
        public MainWindow()
        {
            InitializeComponent();
            //限制窗口高度
            MaxHeight = screenHeight+12;
            //MaxWidth = screenWidth;
            _Version.Content = $"当前版本:{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
        }

        /// <summary>
        /// 窗体移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// 窗体最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiniButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Minimized)
            {
                WindowState = WindowState.Minimized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        /// <summary>
        /// 窗体最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            //判断窗口状态
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                
                _Maximize.Content = "\xe73a;";
            }
            else
            {
                WindowState = WindowState.Normal;
                _Maximize.Content = "\xe659;";
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            
            Close();
        }

        /// <summary>
        /// ToggleButton Checked状态更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (toggleButton != null)
                toggleButton.IsChecked = false;

            toggleButton = sender as ToggleButton;
        }

        private void GetPictures_Click(object sender, RoutedEventArgs e)
        {
            _Frame.Content = getPicture;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            //string databaseLink = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\98546\\Documents\\LocalSql\\Databases\\Crawler.mdf;Integrated Security=True;Connect Timeout=30";
            
        }
    }
}
