using Crawler.Model;
using Crawler.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Crawler.DataAccessObject
{
    public class SqlServerCURD
    {
        //const string databaseLink = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\98546\\Documents\\LocalSql\\Databases\\Crawler.mdf;Integrated Security=True;Connect Timeout=30";

        ///<summary>插入新数据</summary>
        ///<param name="jSCodeStruct">输入数据</param>
        ///<param name="databaseLink">数据库链接字符串</param>
        ///<returns>返回true：插入数据成功</returns>
        public bool InsertData(JSCodeStruct jSCodeStruct,string databaseLink)
        {
            bool flag = false;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(databaseLink);
                using (sqlConnection)
                {
                    sqlConnection?.Open();
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = $"INSERT  INTO [JavaScriptCommand] ([JSCode],[status],[JSCount],[AssociatedWebAddress]) VALUES (N'{jSCodeStruct.JSCODE}', {jSCodeStruct.Status}, N'{jSCodeStruct.JSCountCode}', N'{jSCodeStruct.AssociatedWebAddress}'); ";
                        int cat = sqlCommand.ExecuteNonQuery();
                        if (cat > 0)
                            flag = true;
                    }
                    sqlConnection?.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// 删除单项数据
        /// </summary>
        /// <param name="jSCodeStruct"></param>
        /// <param name="databaseLink"></param>
        /// <returns></returns>
        public bool DeleteOne(JSCodeStruct jSCodeStruct, string databaseLink)
        {
            bool flag = false;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(databaseLink);
                using (sqlConnection)
                {
                    sqlConnection?.Open();
                    using(SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = $"DELETE FROM [JavaScriptCommand] WHERE [Id] = {jSCodeStruct.ID}";
                        var cat = sqlCommand.ExecuteNonQuery();
                        if(cat > 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            throw new Exception("删除数据失败.");
                        }
                    }
                    sqlConnection?.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return flag;
        }
        
        /// <summary>
        /// 更新单项数据
        /// </summary>
        /// <param name="jSCodeStruct">最新数据</param>
        /// <param name="databaseLink">数据库链接</param>
        /// <returns></returns>
        public bool UpdateOne(JSCodeStruct jSCodeStruct, string databaseLink)
        {
            bool flag = false;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(databaseLink);
                using (sqlConnection)
                {
                    sqlConnection?.Open();
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = $"UPDATE [JavaScriptCommand] SET [JSCode] = N'{jSCodeStruct.JSCODE}',[status] = {jSCodeStruct.Status}, [JSCount] = N'{jSCodeStruct.JSCountCode}',[AssociatedWebAddress] = N'{jSCodeStruct.AssociatedWebAddress}' WHERE [Id] = {jSCodeStruct.ID};";
                        var cat = sqlCommand.ExecuteNonQuery();
                        if (cat > 0) 
                        { 
                            flag = true;
                        }else
                        {
                            throw new Exception("修改数据失败.");
                        }
                    }
                    sqlConnection?.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return flag;
        }

        ///<summary>根据ID查询数据是否存在</summary>
        ///<param name="IdValue">ID</param>
        ///<param name="databaseLink">数据库链接</param>
        ///<returns>存在返回ture，不存在返回false</returns>
        public bool QueryIsExist(int IdValue, string databaseLink) 
        {
            bool flag = false;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(databaseLink);
                using (sqlConnection)
                {
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = $"SELECT * FROM [JavaScriptCommand] WHERE [Id] = {IdValue};";
                        var cat = sqlCommand.ExecuteNonQuery();
                        if (cat > 0)
                            flag=true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return flag; 
        }

        /// <summary>
        /// 查找单项数据
        /// </summary>
        /// <param name="IdValue">Id</param>
        /// <param name="databaseLink">数据库链接</param>
        /// <returns>查询结果</returns>
        public JSCodeStruct QueryOne(int IdValue, string databaseLink)
        {
            JSCodeStruct jSCodeStruct = new JSCodeStruct();
            try
            {
                SqlConnection sqlConnection = new SqlConnection(databaseLink);
                using(sqlConnection)
                {
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = $"SELECT * FROM [JavaScriptCommand] WHERE [Id] = {IdValue};";
                        var cat = sqlCommand.ExecuteReader();
                        if (cat.HasRows)
                        {
                            jSCodeStruct.ID = cat.GetInt32(0);
                            jSCodeStruct.JSCODE = cat["JSCode"].ToString();
                            jSCodeStruct.Status = cat.GetInt32(2);
                            jSCodeStruct.JSCountCode = cat["JSCount"].ToString();
                            jSCodeStruct.AssociatedWebAddress = cat["AssociatedWebAddress"].ToString();
                        }
                        else
                        {
                            throw new Exception("在数据库中找不到所需数据.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return jSCodeStruct;
        }

        ///<summary>查找所有数据</summary>
        ///<param name="jSCodeStructs">传入ObservableCollection<JSCodeStruct>类值</param>
        ///<param name="databaseLink">数据库链接</param>
        public ObservableCollection<JSCodeStruct> QueryAllData(ObservableCollection<JSCodeStruct> jSCodeStructs, string databaseLink)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(databaseLink);
                using (sqlConnection)
                {
                    sqlConnection.Open();
                    Console.WriteLine("Open Database Connect Success!");
                    using (SqlCommand SqlCommand = sqlConnection.CreateCommand())
                    {
                        SqlCommand.CommandText = $"SELECT * FROM JavaScriptCommand";
                        //SqlCommand.CommandText = $"SELECT * FROM {tableName}";
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
                                        Status = sqlDataReader.GetInt32(2),
                                        JSCountCode = sqlDataReader["JSCount"].ToString(),
                                        AssociatedWebAddress = sqlDataReader["AssociatedWebAddress"].ToString()
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
                    sqlConnection.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return jSCodeStructs;
        }

        ///<summary>
        ///查询数据库是否存在
        /// </summary>
        /// <returns>
        /// true:链接成功，输出数据库名;
        /// false:链接失败
        /// </returns>
        public bool DatabaseLinkStatus(string databaseLink) 
        {
            bool status = false;
            string databaseName = null;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(databaseLink);
                sqlConnection.Open();
                databaseName = sqlConnection.Database;
                sqlConnection.Close();
                status = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (status)
                {
                    Console.WriteLine($"已连接到数据库{databaseName}.");
                }
                else
                {
                    Console.WriteLine("链接数据库失败.");
                }
            }
            return status;
        }
     
    }
}
