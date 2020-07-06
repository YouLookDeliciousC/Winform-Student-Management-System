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
        //内置委托Action 无返回值，Func 有一个返回值

        private Action reLoad = null;

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
                " where s.IsDeleted = 0 " + 
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
            sql += " and s.IsDeleted = 1 order by StudentId";
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
                //获取行数据
                DataRow dr = (dgvStudentList.Rows[e.RowIndex].DataBoundItem as DataRowView).Row;
                //获取点击的单元格
                DataGridViewCell cell = dgvStudentList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "Update")
                {
                    //修改操作 打开修改页，传studentId 1.构造函数  2.tag  3.开放变量
                    reLoad = LoadAllStudentList;// 赋值委托
                    int stuId = (int)dr["StudentId"];
                    FrmEditStudent frmEdit = new FrmEditStudent();
                    frmEdit.Tag = new TagObject()
                    {
                        EditId = stuId,
                        ReLoad = reLoad
                    };
                    frmEdit.MdiParent = this.MdiParent;
                    frmEdit.Show();//顶级窗体
                    
                    
                }
                else if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "Delete")
                {
                    if(MessageBox.Show("You absolutely want to DELETE this student?","DeleteTip",
                        MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        
                        int stuId = (int)dr["StudentId"];
                        //删除操作
                        //加删除
                        string sqlDel0 = "update StudentInfo set IsDeleted = 1 where StudentId = @StudentId ";
                        SqlParameter paraDel0 = new SqlParameter("@StudentId", stuId);
                        int count = SqlHelper.ExecuteNonQuery(sqlDel0, paraDel0);
                        if (count > 0)
                        {
                            MessageBox.Show("Delete student SUCCESSFUL!", "DeleteTip",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //DataGrideView还未刷新
                            DataTable dtStudents = (DataTable)dgvStudentList.DataSource;
                            //dgvStudentList.DataSource = null;
                            dtStudents.Rows.Remove(dr);
                            dgvStudentList.DataSource = dtStudents;
                        }
                        else
                        {
                            MessageBox.Show("Fail to Delete!", "DeleteTip",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    


                    //真删除
                    
                }
            }
            


        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            //
            //获取要删除的数据StudentId
            //判断删除的个数 =0 没有选择，提示选择

            //大于0继续

            //删除操作  事务  代码里启动事务

            List<int> listIds = new List<int>();
            for (int i = 0; i < dgvStudentList.Rows.Count; ++i)
            {
                DataGridViewCheckBoxCell cell = dgvStudentList.Rows[i].Cells["colCheck"] as DataGridViewCheckBoxCell;
                bool chk = Convert.ToBoolean(cell.Value);
                if (chk)
                {
                    DataRow dr = (dgvStudentList.Rows[i].DataBoundItem as DataRowView).Row;
                    int stuId = (int)dr["StudentId"];
                    listIds.Add(stuId);
                }
                
            }
            //真删除
            if (listIds.Count == 0)
            {
                MessageBox.Show("Please choose Student to be deleted!", "DeleteTip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if(MessageBox.Show("You absolutely want to delete?", "DeleteTip", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int count = 0;
                    //启动事物
                    using(SqlConnection conn = new SqlConnection(SqlHelper.connString))
                    {
                        //事务通过conn开启
                        conn.Open();
                        SqlTransaction trans = conn.BeginTransaction();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = trans;

                        try
                        {
                            foreach(int id in listIds)
                            {
                                cmd.CommandText = "delete from StudentInfo where StudentId = @StudentId";
                                SqlParameter para = new SqlParameter("@StudentId", id);
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(para);

                                count += cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                        }
                        catch(SqlException ex)
                        {
                            trans.Rollback();
                            MessageBox.Show("Error:Fail to delete students!", "DeleteTip",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }
                    if(count == listIds.Count)
                    {
                        MessageBox.Show("The information deleted successful!", "DeleteTip",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DataTable dtStudent = (DataTable)dgvStudentList.DataSource;
                        string idStr = string.Join(",", listIds);
                        DataRow[] rows = dtStudent.Select("StudentId in (" + idStr + ")");
                        foreach(DataRow dr in rows)
                        {
                            dtStudent.Rows.Remove(dr);
                        }
                        dgvStudentList.DataSource = dtStudent;



                    }
                }

            }

        }
    }
}