using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace WinStudent
{
    class SqlHelper
    {

        private static readonly string connString =ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public static object ExecuteScalar(string sql, params SqlParameter[] paras)
        {
            object o = null;
            //建立数据库连接
            //连接字符串 钥匙
            
            //datasource            initial catalog     userid         password
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);//如果是个存储过程要加
                                                       //cmd.CommandType = CommandType.StoredProcedure; 
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(paras);
                //打开连接
                conn.Open(); //最晚打开，最早关闭
                o = cmd.ExecuteScalar();//执行查询，返回结果第一行第一列的值，忽略其它行或列
                                               //关闭连接
                conn.Close();
            }
                //创建command对象
                
            return o;
        }
    }
}
