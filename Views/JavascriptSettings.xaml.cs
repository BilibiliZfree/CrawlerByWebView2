using Crawler.Model;
using Crawler.DataAccessObject;
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
    /// JavascriptSettings.xaml 的交互逻辑
    /// </summary>
    public partial class JavascriptSettings : Window
    {

        const string databaseLink = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\98546\\Documents\\LocalSql\\Databases\\Crawler.mdf;Integrated Security=True;Connect Timeout=30";

        /// <summary>
        /// 存Javascript命令列表
        /// </summary>
        ObservableCollection<JSCodeStruct> jSCodeStructs = new ObservableCollection<JSCodeStruct>();

        public JavascriptSettings()
        {
            InitializeComponent();
            Javascript_ListView.ItemsSource = jSCodeStructs;
            SqlRead();
        }

        ///<summary>
        ///链接本地数据库：读取JavaScriptCommand表
        ///</summary>
        public void SqlRead()
        {
            jSCodeStructs.Clear();
            foreach (var item in new SqlServerCURD().QueryAllData(new ObservableCollection<JSCodeStruct>(), databaseLink))
            {
                jSCodeStructs.Add(item);
            }
        }

        #region ButtonClick

        private async void GetAssociatedWebAddress_Button_Click(object sender, RoutedEventArgs e)
        {
            //获取运行中的WebView2窗口
            WebView2 webView2 = Application.Current.MainWindow as WebView2;
            string vs =await webView2.GetWebSiteAsync();
            if (vs != null)
                AssociatedWebAddress_TextBox.Text = vs.Substring(1,vs.Length-2);
        }

        /// <summary>
        /// 双击ListView列表中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Javascript_ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int cat = Javascript_ListView.SelectedIndex;
            if (cat != -1)
            {
                JSCodeStruct @struct = jSCodeStructs.ToList().Find(x => x.NO == (cat + 1));
                JSCode_TextBox.Text = @struct.JSCODE;
                JSCount_TextBox.Text = @struct.JSCountCode;
                AssociatedWebAddress_TextBox.Text = @struct.AssociatedWebAddress;
            }
            else
            {
                MessageBox.Show("列表中没有数据.");
            }
        }

        /// <summary>
        /// 添加数据按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertCommand_Button_Click(object sender, RoutedEventArgs e)
        {
            JSCodeStruct @Struct = new JSCodeStruct() 
            { 
                JSCODE = JSCode_TextBox.Text,
                Status = 1,
                JSCountCode = JSCount_TextBox.Text,
                AssociatedWebAddress = AssociatedWebAddress_TextBox.Text
            };
            bool result = new SqlServerCURD().InsertData(@Struct, databaseLink);
            if (result)
            {
                MessageBox.Show("添加数据成功.");
            }
            else
            {
                MessageBox.Show("添加数据失败.");
            }
            SqlRead();
        }
        

        



        private void UseOrNot_Button_Click(object sender, RoutedEventArgs e)
        {
            int cat = Javascript_ListView.SelectedIndex;
            if (cat == -1)
            {
                MessageBox.Show("请选择正确的数据.");
                return;
            }
            else
            {
                JSCodeStruct jSCodestruct = jSCodeStructs.ToList().Find(x => x.NO == (cat + 1));
                /* 状态
                 * 0：不使用
                 * 1：只使用JSCode
                 * 2：使用JSCode和JSCount
                 */
                if (jSCodestruct.Status==0)
                {
                    jSCodestruct.Status = 1;
                }
                else if(jSCodestruct.Status == 1)
                {
                    jSCodestruct.Status = 2;
                }
                else
                {
                    jSCodestruct.Status = 0;
                }
                bool result = new SqlServerCURD().UpdateOne(jSCodestruct,databaseLink);
                if (result)
                {
                    MessageBox.Show("状态已更新.");
                }
                else
                {
                    MessageBox.Show("状态更新失败.");
                }
                SqlRead();
            }
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            int cat = Javascript_ListView.SelectedIndex;
            JSCodeStruct jSCodeStruct = jSCodeStructs.ToList().Find(x => x.NO == (cat + 1));
            jSCodeStruct.JSCODE = JSCode_TextBox.Text;
            jSCodeStruct.JSCountCode = JSCount_TextBox.Text;
            jSCodeStruct.AssociatedWebAddress = AssociatedWebAddress_TextBox.Text;
            bool result = new SqlServerCURD().UpdateOne(jSCodeStruct, databaseLink);
            if (result)
            {
                MessageBox.Show("数据已更新.");
            }
            else
            {
                MessageBox.Show("数据更新失败.");
            }
            SqlRead();
        }

        /// <summary>
        /// 删除数据按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteCommand_Button_Click(object sender, RoutedEventArgs e)
        {
            int cat = Javascript_ListView.SelectedIndex;
            if (cat == -1)
            {
                MessageBox.Show("请选择正确的数据.");
                return;
            }
            else
            {
                JSCodeStruct jSCodeStruct = jSCodeStructs.ToList().Find(x => x.NO == (cat + 1));
                bool result = new SqlServerCURD().DeleteOne(jSCodeStruct, databaseLink);
                if (result)
                {
                    MessageBox.Show("数据已删除.");
                }
                else
                {
                    MessageBox.Show("数据删除失败.");
                }
            }
            SqlRead();
        }

        #endregion
    }
}