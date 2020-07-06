using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinStudent
{
    public partial class FrmEditStudent : Form
    {
        public FrmEditStudent()
        {
            InitializeComponent();
        }
        private int stuId = 0;
        private Action reLoad = null;
        /*public FrmEditStudent(int _stuId)
        {
            InitializeComponent();
            stuId = _stuId;
        }*/

        private void FrmEditStudent_Load(object sender, EventArgs e)
        {
            LoadClasses(); //加载班级列表
            //加载学生信息
            InitStuInfo();
        }

        private void InitStuInfo()
        {
            //获取StuId
            if(this.Tag != null)
            {
                TagObject tagObject = (TagObject)this.Tag;
                stuId = tagObject.EditId;
                reLoad = tagObject.ReLoad; //赋值
                //int.TryParse(this.Tag.ToString(), out stuId);
            }
            //查询数据
            string sql = "select StudentName,Sex,ClassId,Phone from StudentInfo" +
                " where Studentid = @StudentId";
            SqlParameter paraId = new SqlParameter("@StudentId", stuId);
            SqlDataReader dr = SqlHelper.ExecuteReader(sql, paraId);
            //读取数据 只向前，不后退，读一条 丢一条
            if (dr.Read())
            {
                txtName.Text = dr["StudentName"].ToString();
                txtPhone.Text = dr["Phone"].ToString();
                string sex = dr["Sex"].ToString();
                if (sex == "Male")
                    rbM.Checked = true;
                else
                    rbF.Checked = true;
                int classId = (int)dr["ClassId"];
                cboClass.SelectedValue = classId;
            }
            dr.Close();
        }

        private void LoadClasses()
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
            //添加默认项
            DataRow drnew = dtClasses.NewRow();
            drnew["ClassId"] = 0;
            drnew["ClassName"] = "Please Choose";

            dtClasses.Rows.InsertAt(drnew, 0);
            //指定数据源
            cboClass.DataSource = dtClasses;
            cboClass.DisplayMember = "ClassName";
            cboClass.ValueMember = "ClassId";
        }
        /// <summary>
        /// 修改学生信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string stuName = txtName.Text.Trim();
            int classId = (int)cboClass.SelectedValue;
            string sex = rbM.Checked ? rbM.Text.Trim() : rbF.Text.Trim();
            string phone = txtPhone.Text.Trim();
            //判空处理
            if (string.IsNullOrEmpty(stuName))
            {
                MessageBox.Show("Name cannot be void!", "EditInfo Tip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Phone cannot be void!", "EditInfo Tip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //判断是否存在
            string sql = "select count(1) from StudentInfo " +
                "where StudentName =@StudentName and Phone = @Phone and StudentId <> @StudentId";
            SqlParameter[] paras =
            {
                new SqlParameter("@StudentName",stuName),
                new SqlParameter("@Phone",phone),
                new SqlParameter("@StudentId",stuId)
            };
            object o = SqlHelper.ExecuteScalar(sql, paras);
            if(o != null && o != DBNull.Value && (int)o != 0)
            {
                MessageBox.Show("The Student has EXIST!", "EditInof Tip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //修改
            string sqlUpdate = "update StudentInfo set " +
                "StudentName = @StudentName,Sex = @Sex,ClassId=@ClassId,Phone=@Phone" +
                    " where StudentId = @StudentId";

            SqlParameter[] parasUpdate =
            {
                new SqlParameter("StudentName",stuName),
                new SqlParameter("@ClassId",classId),
                new SqlParameter("@Sex",sex),
                new SqlParameter("@Phone",phone),
                new SqlParameter("@StudentId",stuId)
            };

            int count = SqlHelper.ExecuteNonQuery(sqlUpdate, parasUpdate);
            if (count > 0)
            {
                MessageBox.Show($"Update Student: {stuName} Successful!", "EditInfo Tip",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearText(this);
                //刷新学生列表 跨页面刷新 委托
                reLoad.Invoke();

            }
            else
            {
                MessageBox.Show($"FAIL to Update Student: {stuName} !", "EditInfo Tip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
