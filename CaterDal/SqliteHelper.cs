using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SQLite;
using System.Data;

namespace CaterDal
{
    public class SqliteHelper
    {
        //从配置文件中读取连接字符串
        private static string connStr = ConfigurationManager.ConnectionStrings["CaterConn"].ConnectionString;

        //执行命令的方法：insert delete update
        /// <summary>
        /// 执行非查询SQL命令 例如：insert update delete drop
        /// </summary>
        /// <param name="sql">要执行的参数化SQL语句</param>
        /// <param name="ps">附加的参数</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(string sql, params SQLiteParameter[] ps)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(ps);

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        //获取首行首列值的方法
        /// <summary>
        /// 执行返回首行首列的SQL查询命令
        /// </summary>
        /// <param name="sql">要执行的参数化SQL语句</param>
        /// <param name="ps">附加的参数</param>
        /// <returns>返回首行首列的查询结果</returns>
        public static object ExecuteScalar(string sql, params SQLiteParameter[] ps)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(ps);

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        //获取结果集
        /// <summary>
        /// 执行返回查询结果集 DataTable
        /// </summary>
        /// <param name="sql">要执行的参数化SQL语句</param>
        /// <param name="ps">附件的参数</param>
        /// <returns>返回DataTable结果集</returns>
        public static DataTable GetDataTable(string sql, params SQLiteParameter[] ps)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.SelectCommand.Parameters.AddRange(ps);
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
