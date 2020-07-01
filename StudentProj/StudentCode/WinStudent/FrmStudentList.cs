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
    public partial class FrmStudentList : Form
    {
        public FrmStudentList()
        {
            InitializeComponent();
        }

        //实现单例
        private static FrmStudentList frmStudentList = null;

        public static FrmStudentList CreateInstance()
        {
            if(frmStudentList == null || frmStudentList.IsDisposed)
            {
                frmStudentList = new FrmStudentList();

            }
            return frmStudentList;
        }
        //加载班级列表和所有学生信息
        private void FrmStudentList_Load(object sender, EventArgs e)
        {
            LoadClasses(); //加载班级列表
            LoadAllStudentList();//加载所有学生信息

        }

        private void LoadAllStudentList()
        {
            string sql = "select StudentId,StudentName,ClassName,GradeName,Sex,Phone from StudentInfo s" +
                " inner join ClassInfo c on s.ClassId = c.ClassId" +
                " inner join GradeInfo g on c.GradeId = g.GradeId" +
                " order by GradeName";
            //加载数据
            DataTable dtStudents = SqlHelper.GetDataTable(sql);
            //组装数据
            if (dtStudents.Rows.Count > 0)
            {
                foreach(DataRow dr in dtStudents.Rows)
                {
                    string className = dr["ClassName"].ToString();
                    string gradeName = dr["GradeName"].ToString();

                    dr["ClassName"] = className + "-" + gradeName;

                }
            }
            //绑定数据
            dtStudents.Columns.Remove("GradeName"); //删除使用后的多余列
            //dgvStudentList.AutoGenerateColumns = false; //或者使用
            dgvStudentList.DataSource = dtStudents;
            
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
                foreach(DataRow dr in dtClasses.Rows)
                {
                    string className = dr["ClassName"].ToString();
                    string gradeName = dr["GradeName"].ToString();
                    dr["ClassName"] = className +"-"+ gradeName;
                }
            }
            //添加默认项
            DataRow drnew = dtClasses.NewRow();
            drnew["ClassId"] = 0;
            drnew["ClassName"] = "Please Choose";

            dtClasses.Rows.InsertAt(drnew, 0);
            //指定数据源
            cboClasses.DataSource = dtClasses;
            cboClasses.DisplayMember = "ClassName";
            cboClasses.ValueMember = "ClassId";
        }
        /// <summary>
        /// 查询学生信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            int classId = (int)cboClasses.SelectedValue;
            string StuName = txtName.Text.Trim();

            string sql = "select StudentId,StudentName,ClassName,GradeName,Sex,Phone from StudentInfo s" +
                " inner join ClassInfo c on s.ClassId = c.ClassId" +
                " inner join GradeInfo g on c.GradeId = g.GradeId" +
                " where 1=1 ";
            if(classId > 0)
            {
                sql += " and s.ClassId = @ClassId ";
            }
            if (!string.IsNullOrEmpty(StuName))
            {
                sql += " and StudentName like @StudentName ";
            }

            SqlParameter[] paras =
            {
                new SqlParameter("@ClassId",classId),
                new SqlParameter("@StudentName","%"+StuName+"%")
            };
            DataTable dtStudentList = SqlHelper.GetDataTable(sql, paras);

            if (dtStudentList.Rows.Count > 0)
            {
                foreach(DataRow dr in dtStudentList.Rows)
                {
                    string className = dr["ClassName"].ToString();
                    string gradeName = dr["GradeName"].ToString();
                    dr["ClassName"] = className + "-" + gradeName;
                }
            }
            dtStudentList.Columns.RemoveAt(3);
            dgvStudentList.DataSource = dtStudentList;



        }

        private void dgvStudentList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                //获取点击的单元格
                DataGridViewCell cell = dgvStudentList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "Update")
                {
                    //修改操作 打开修改页，传studentId
                }
                else if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "Delete")
                {
                    //删除操作
                    
                }
            }
            


        }
    }
}
