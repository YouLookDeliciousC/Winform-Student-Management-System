using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WinStudent
{
    public partial class FrmEditClass : Form
    {
        public FrmEditClass()
        {
            InitializeComponent();
        }
        private int classId = 0;
        private string oldName = "";
        private int oldGradeId = 0;
        private Action reload = null;//刷新列表页
        /// <summary>
        /// 打开页面，加载年纪列表，加载班级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEditClass_Load(object sender, EventArgs e)
        {
            InitGradeList();
            InitClassInfo();
        }

        private void InitGradeList()
        {
            string sql = "select GradeId,GradeName from GradeInfo";
            DataTable dtGradeList = SqlHelper.GetDataTable(sql);

            cboGrade.DataSource = dtGradeList;
            cboGrade.DisplayMember = "GradeName";
            cboGrade.ValueMember = "GradeId";

        }

        private void InitClassInfo()
        {
            if (this.Tag != null)
            {
                TagObject tagObject = (TagObject)this.Tag;
                classId = tagObject.EditId;
                reload = tagObject.ReLoad; //赋值
                //int.TryParse(this.Tag.ToString(), out stuId);
            }
            //查询数据
            string sql = "select ClassName,GradeId,Remark from ClassInfo" +
                " where ClassId=@ClassId";
            SqlParameter paraId = new SqlParameter("@ClassId", classId);
            SqlDataReader dr = SqlHelper.ExecuteReader(sql, paraId);
            //读取数据 只向前，不后退，读一条 丢一条
            if (dr.Read())
            {
                txtClassName.Text = dr["ClassName"].ToString();
                oldName = txtClassName.Text.Trim();
                
                txtRemark.Text = dr["Remark"].ToString();

                int gradeId = (int)dr["GradeId"];
                oldGradeId = gradeId;
                cboGrade.SelectedValue = gradeId;
            }
            dr.Close();



        }
        /// <summary>
        /// 提交修改信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //获取信息
            string className = txtClassName.Text.Trim();
            int gradeId = (int)cboGrade.SelectedValue;
            string remark = txtRemark.Text.Trim();

            //判空
            if (string.IsNullOrEmpty(className))
            {
                MessageBox.Show("Class Name cannot be void!", "EditInfo Tip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //查重

            string sql = "select count(1) from ClassInfo " +
                "where ClassName =@ClassName and GradeId = @GradeId";
            if(className==oldName && gradeId == oldGradeId)
            {
                sql += " and ClassId <> @ClassId";
            }
            SqlParameter[] paras =
            {
                new SqlParameter("@ClassName",className),
                new SqlParameter("@GradeId",gradeId),
                new SqlParameter("@ClassId",classId)
            };
            object o = SqlHelper.ExecuteScalar(sql, paras);
            if (o != null && o != DBNull.Value && (int)o != 0)
            {
                MessageBox.Show("The Class has EXIST!", "EditInof Tip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //提交

            string sqlUpdate = "update ClassInfo set " +
                "ClassName = @ClassName,GradeId = @GradeId,Remark=@Remark" +
                    " where ClassId = @ClassId";

            SqlParameter[] parasUpdate =
            {
                new SqlParameter("@ClassName",className),
                new SqlParameter("@GradeId",gradeId),
                new SqlParameter("@Remark",remark),
                new SqlParameter("@ClassId",classId)
            };

            int count = SqlHelper.ExecuteNonQuery(sqlUpdate, parasUpdate);
            if (count > 0)
            {
                MessageBox.Show($"Update Class: {className} Successful!", "EditInfo Tip",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearText(this);
                //刷新学生列表 跨页面刷新 委托
                
                reload.Invoke();

            }
            else
            {
                MessageBox.Show($"FAIL to Update Class: {classId} !", "EditInfo Tip",
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
