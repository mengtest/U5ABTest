using UnityEngine;
using System.Collections;
using ResetCore.Util;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;
using System.Collections.Generic;
using System;

namespace ResetCore.MySQL
{
    public class MySQLManager
    {

        public static MySqlConnection current { get; private set; }

        /// <summary>
        /// 打开连接
        /// </summary>
        /// <param name="host">主机IP</param>
        /// <param name="database">数据库名</param>
        /// <param name="id">账号</param>
        /// <param name="pwd">密码</param>
        /// <param name="port">端口（默认为3306）</param>
        public static void OpenSql(string host, string database, string id, string pwd, string port = "3306")
        {
            if (current != null)
            {
                current.Close();
            }
            try
            {
                string connectionString =
                    string.Format("Server = {0};port={4};Database = {1}; User ID = {2}; Password = {3};", host, database, id, pwd, port);
                current = new MySqlConnection(connectionString);
                current.Open();
            }
            catch (Exception e)
            {
                throw new Exception("服务器连接失败，请重新检查是否打开MySql服务。" + e.Message.ToString());

            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public static void Close()
        {

            if (current != null)
            {
                current.Close();
                current.Dispose();
                current = null;
            }

        }

        /// <summary>
        /// 执行SQL操作
        /// </summary>
        /// <param name="sqlString">需要执行的字符串</param>
        /// <returns>返回相应的DataSet</returns>
        public static DataSet ExecuteQuery(string sqlString)
        {
            if (current.State == ConnectionState.Open)
            {
                DataSet ds = new DataSet();
                try
                {

                    MySqlDataAdapter da = new MySqlDataAdapter(sqlString, current);
                    da.Fill(ds);

                }
                catch (Exception ee)
                {
                    throw new Exception("SQL:" + sqlString + "/n" + ee.Message.ToString());
                }
                finally
                {
                }
                return ds;
            }
            return null;
        }
    }
}

