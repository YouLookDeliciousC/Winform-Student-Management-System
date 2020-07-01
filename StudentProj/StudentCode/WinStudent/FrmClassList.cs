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
    public partial class FrmClassList : Form
    {
        public FrmClassList()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 初始化年纪列表和所有班级列表信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmClassList_Load(object sender, EventArgs e)
        {
            InitGrades();//初始化选择框
            InitAllClasses(); //加载所有班级信息
        }

        private void InitAllClasses()
        {
            string sql = "select ClassId,ClassName,GradeName,Remark from ClassInfo c " +
                "inner join GradeInfo d on c.GradeId = d.GradeId order by GradeName";
            DataTable dtClasses = SqlHelper.GetDataTable(sql);
            dgvClassList.DataSource = dtClasses;
        }

        private void InitGrades()
        {
            string sql = "select GradeId,GradeName from GradeInfo";
            DataTable dtGradeList = SqlHelper.GetDataTable(sql);
            DataRow row = dtGradeList.NewRow();
            row["GradeId"] = 0;
            row["GradeName"] = "Please Choose ...";
            dtGradeList.Rows.InsertAt(row, 0);
            cboGrade.DataSource = dtGradeList;
            cboGrade.DisplayMember = "GradeName";
            cboGrade.ValueMember = "GradeId";
            
        }
        /// <summary>
        /// 查询班级按钮信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            //查询条件
            int gradeId = (int)cboGrade.SelectedValue;
            string className = txtClassName.Text.Trim();

            string sql = "select ClassId,ClassName,GradeName,Remark from ClassInfo c " +
                "inner join GradeInfo g on c.GradeId = g.GradeId";
            sql += " where 1=1";
            
            if (gradeId > 0)
            {
                sql += " and c.GradeId = @GradeId";
            }
            if (!string.IsNullOrEmpty(className))
            {
                sql += " and ClassName like @ClassName";
            }
            SqlParameter[] paras =
            {
                new SqlParameter("@GradeId",gradeId),
                new SqlParameter("@ClassName","%"+className+"%")
            };

            DataTable dtClasses = SqlHelper.GetDataTable(sql,paras);
            dgvClassList.DataSource = dtClasses;

        }
    }
}
