using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinStudent
{
    public partial class FrmAddStudent : Form
    {
        public FrmAddStudent()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 加载班级列表和性别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAddStudent_Load(object sender, EventArgs e)
        {
            InitClass();
            rbM.Checked = true;
        }

        private void InitClass()
        {
            //获取数据
            string sql = "select ClassId,ClassName,GradeName from ClassInfo c " +
                "inner join GradeInfo g on c.GradeId=g.GradeId";
            DataTable dtClasses = SqlHelper.GetDataTable(sql);
            //组合班级列表显示项
            if (dtClasses.Rows.Count > 0)
            {
                foreach (DataRow dr in dtClasses.Rows)
                {
                    string className = dr["ClassName"].ToString();
                    string gradeName = dr["GradeName"].ToString();
                    dr["ClassName"] = className + "-" + gradeName;
                }
            }
            
            //指定数据源
            cboClass.DataSource = dtClasses;
            cboClass.DisplayMember = "ClassName";
            cboClass.ValueMember = "ClassId";
            if (cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("No Class Now,Please Add Class First");
                
            }
            else
            {
                cboClass.SelectedIndex = 0;
            }
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //获取信息
            string stuName = txtName.Text.Trim();
            int classId = (int)cboClass.SelectedValue;
            string sex = rbM.Checked ? rbM.Text : rbF.Text;
            string phone = txtPhone.Text.Trim();
            //判空
            if (string.IsNullOrEmpty(stuName))
            {
                MessageBox.Show("Student name Cannot be void!", "AddStudent Tip", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Phone number Cannot be void!", "AddStudent Tip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //判断是否存在
            string sql = "select count(1) from StudentInfo " +
                "where StudentName =@StudentName and Phone = @Phone";
            SqlParameter[] paras =
            {
                new SqlParameter("@StudentName",stuName),
                new SqlParameter("@Phone",phone)
            };
            object o = SqlHelper.ExecuteScalar(sql,paras);
            if (o == null || o == DBNull.Value || ((int)o) == 0)
            {
                //入库
                string sqlAdd = "insert into StudentInfo(StudentName,ClassId,Sex,Phone)" +
                    " values (@StudentName,@ClassId,@Sex,@Phone)";

                SqlParameter[] parasAdd =
                {
                    new SqlParameter("StudentName",stuName),
                    new SqlParameter("@ClassId",classId),
                    new SqlParameter("@Sex",sex),
                    new SqlParameter("@Phone",phone)
                };

                int addCheck = SqlHelper.ExecuteNonQuery(sqlAdd, parasAdd);
                if (addCheck > 0)
                {
                    MessageBox.Show($"Add Student: {stuName} Successful!", "AddStudent Tip", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText(this);
                }
                else
                {
                    MessageBox.Show($"FAIL to Add Student: {stuName} !", "AddStudent Tip",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("This student has Exist! Cannot be added!", "AddStudent Tip", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }
        /// <summary>
        /// 清除当前文本框内的内容
        /// </summary>
        /// <param name="ctrlTop"></param>
        private void ClearText(Control ctrlTop)
        {
            if (ctrlTop.GetType() == typeof(TextBox))
                ctrlTop.Text = "";
            else
            {
                foreach (Control ctrl in ctrlTop.Controls)
                {
                    ClearText(ctrl); //循环调用
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
