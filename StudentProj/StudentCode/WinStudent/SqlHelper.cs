using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WinStudent
{
    class SqlHelper
    {

        public static readonly string connString =ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
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

                cmd.Parameters.Clear(); 
                conn.Close();
            }
                //创建command对象
                
            return o;
        }
        /// <summary>
        /// 执行查询，返回SqlDataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] paras)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql,conn);
            try
            {
                if (paras.Length > 0)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(paras);
                }
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch(Exception ex)
            {
                conn.Close();
                throw new Exception("Query Excption",ex);
            }
        }

        /// <summary>
        /// 返回dataTable  （dataset用于一次性返回多个结果）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, params SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);//如果是个存储过程要加
                                                           //cmd.CommandType = CommandType.StoredProcedure; 
                //SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                //可以直接实例桥接器，不用sqlcommand， 有参数时不能这也使用。
                if(paras!= null)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(paras);
                }

                //打开连接
                //conn.Open(); //如果手动打开，也要手动关闭
                //o = cmd.ExecuteScalar();//执行查询，返回结果第一行第一列的值，忽略其它行或列
                //关闭连接
                //conn.Close();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);


            }
            return dt;


        }
        /// <summary>
        /// 返回受影响的行数 增删改都通用
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql,params SqlParameter[] paras)
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);//如果是个存储过程要加
                                                           //cmd.CommandType = CommandType.StoredProcedure; 
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(paras);
                //打开连接
                conn.Open(); //最晚打开，最早关闭
                count = cmd.ExecuteNonQuery();//执行T-sql语句，返回受影响行数
                                              //关闭连接

                cmd.Parameters.Clear();
                conn.Close();
            }
            return count;
        }

        public static bool ExecuteTrans(List<CommandInfo> comList)
        {
            using(SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = trans;
                try
                {
                    int count = 0;
                    for(int i = 0; i < comList.Count; i++)
                    {
                        cmd.CommandText = comList[i].CommandText;
                        if (comList[i].IsProc)
                            cmd.CommandType = CommandType.StoredProcedure;
                        else
                            cmd.CommandType = CommandType.Text;
                        if (comList[i].Parameters.Length > 0)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(comList[i].Parameters);
                        }
                        count += cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                    }
                    trans.Commit();
                    return true;
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    throw new Exception("Execute transction error!", e);
                }
            }
        }

    }
}
