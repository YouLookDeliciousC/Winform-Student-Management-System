using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WinStudent
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //获取用户输入
            string uName = txtUserName.Text.Trim();
            string uPwd = txtUserPwd.Text.Trim();
            //判断是否为空
            if(string.IsNullOrEmpty(uName))
            {
                MessageBox.Show("Please type in USERID!", "Login Tips", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(uPwd))
            {
                MessageBox.Show("Please type in PASSWORD!", "Login Tips",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserPwd.Focus();
                return;
            }

            //与数据库通信 检查输入与数据库中是否一致
            {
                /*//建立数据库连接
                //连接字符串 钥匙
                *//*string connString = "server=DESKTOP-AVP4QM5;database=StudentDB;." +
                    "Integrated Security=true";*//* //Windows身份验证
                //sql server身份验证
                string connString = "server=DESKTOP-AVP4QM5;database=StudentDB;uid=LocalWin;pwd=123456";
                                    //datasource            initial catalog     userid         password
                SqlConnection conn = new SqlConnection(connString);
                //写查询语句
                //string sql = "select count(1) from UserInfo where UserName='" + uName + "' and UserPwd='" + uPwd + "'";
                //添加参数
                //SqlParameter paraUName = new SqlParameter("@UserName", uName);
                //SqlParameter paraUPwd = new SqlParameter("@UserPwd", uPwd);*/
                string sql = "select count(1) from UserInfo where UserName=@UserName and UserPwd=@UserPwd";
                SqlParameter[] paras =
                {
                    new SqlParameter("@UserName",uName),
                    new SqlParameter("@UserPwd",uPwd)
                };
                /*//创建command对象
                SqlCommand cmd = new SqlCommand(sql, conn);//如果是个存储过程要加
                                                           //cmd.CommandType = CommandType.StoredProcedure; 
                cmd.Parameters.Clear();
                //cmd.Parameters.Add(paraUName);
                //cmd.Parameters.Add(paraUPwd); 用参数数组会简洁些
                cmd.Parameters.AddRange(paras);
                //打开连接
                conn.Open(); //最晚打开，最早关闭

                //执行命令  要求在连接状态下
                object o = cmd.ExecuteScalar();//执行查询，返回结果第一行第一列的值，忽略其它行或列
                //关闭连接
                conn.Close();*/

                //调用
                //SqlHelper helper = new SqlHelper(); 写为静态方法，无需实例化
                object o = SqlHelper.ExecuteScalar(sql, paras);
                //处理结果  
                if (o == null|| (o==DBNull.Value)||((int)o)==0)
                {
                    MessageBox.Show("UserID or Password is wrong,please check!", "Loggin Error",
                        MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Successful!", "Tips",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 转到主页面
                    FrmMain fmain = new FrmMain();
                    fmain.Show();
                    this.Hide();
                }
            }



            //返回的结果进行的不同的提示
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close(); //如果不是主页面，且要退出系统，要用Application.Exit();
        }
    }
}
