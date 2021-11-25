using Crawler.Model;
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
            test();
        }

        ///<summary>
        ///链接本地数据库
        ///</summary>
        public void test()
        {
            SqlConnection sqlConnection = new SqlConnection(databaseLink);
            using (sqlConnection)
            {
                sqlConnection.Open();
                Console.WriteLine("Open Database Connect Success!");
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM JavaScriptCommand";
                    command.ExecuteNonQuery();
                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    try
                    {
                        jSCodeStructs.Clear();
                        if (sqlDataReader.HasRows)
                        {
                            //sqlDataReader.Read();
                            while(sqlDataReader.Read()){
                                JSCodeStruct jSCodeStruct = new JSCodeStruct()
                                {
                                    ID = sqlDataReader["Id"].ToString(),
                                    JSCODE = sqlDataReader["JSCode"].ToString(),
                                    Status = sqlDataReader["status"].ToString()
                                };
                                jSCodeStructs.Add(jSCodeStruct);
                            }
                            
                            
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }
    }
}
