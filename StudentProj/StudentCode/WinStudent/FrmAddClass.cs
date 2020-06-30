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

namespace WinStudent
{
    public partial class FrmAddClass : Form
    {
        public FrmAddClass()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化添加班级页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAddClass_Load(object sender, EventArgs e)
        {
            InitGradeList();

        }

        private void InitGradeList()
        {
            string sql = "select GradeId,GradeName from GradeInfo";
            DataTable dtGradeList = SqlHelper.GetDataTable(sql);
            
            cboGrade.DataSource = dtGradeList;
            cboGrade.DisplayMember = "GradeName";
            cboGrade.ValueMember = "GradeId";

            cboGrade.SelectedIndex = 0;
        }
        /// <summary>
        /// 添加班级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //信息获取
            string className = txtClassName.Text.Trim();
            int gradeId = (int)cboGrade.SelectedValue;
            string remark = txtRemark.Text.Trim();


            //判断是否为空
            if (string.IsNullOrEmpty(className))
            {
                MessageBox.Show("Class Name Cannot be void!", "Tip", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }


            //判断是否已存在 检查数据库
            {
                string sqlExit = "select count(1) from classInfo where ClassName=@ClassName and GradeId=@GradeId";
                SqlParameter[] paras =
                {
                    new SqlParameter("@ClassName",className),
                    new SqlParameter("@GradeId",gradeId)
                };

                object oCount = SqlHelper.ExecuteScalar(sqlExit, paras);
                if (oCount == null || oCount == DBNull.Value || ((int)oCount == 0)){
                    //添加操作
                    string sqlAdd = "insert into ClassInfo(ClassName,GradeId,Remark) values (@ClassName,@GradeId,@Remark)";
                    SqlParameter[] parasAdd =
                    {
                        new SqlParameter("@ClassName",className),
                        new SqlParameter("@GradeId",gradeId),
                        new SqlParameter("@Remark",remark)
                    };
                    //执行并返回值
                    int count = SqlHelper.ExecuteNonQuery(sqlAdd, parasAdd);
                    if(count > 0)
                    {
                        MessageBox.Show($"Insert Class:{className} Successful!", "AddClassTip", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearText(this);
                    }
                    else
                    {
                        MessageBox.Show("Fail to Add Class!", "AddClassTip", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                }
                else
                {
                    MessageBox.Show("Class has exist!", "AddClass Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
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
