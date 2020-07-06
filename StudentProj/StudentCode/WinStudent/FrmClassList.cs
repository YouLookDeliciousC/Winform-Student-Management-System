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
        private Action reLoad = null;
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

        private void dgvClassList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                //获取选择的单元格，判断
                DataGridViewCell cell = dgvClassList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                DataRow dr = (dgvClassList.Rows[e.RowIndex].DataBoundItem as DataRowView).Row;
                int classId = (int)dr["ClassId"];

                if(cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "Update")
                {
                    //修改 打开修改页，把编号传过去
                    reLoad = InitAllClasses;//赋值
                    FrmEditClass frmEdit = new FrmEditClass();
                    frmEdit.Tag = new TagObject()
                    {
                        EditId = classId,
                        ReLoad = reLoad
                    };
                    frmEdit.MdiParent = this.MdiParent;
                    frmEdit.Show();


                }
                else if(cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "Delete")
                {
                    if(MessageBox.Show("Are you sure you want to delete this class and its related students?","DeleteTip",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //单条删除
                        //班级删除过程 班级--学生 先删除学生 后删除班级
                        string sqlDelStu = "delete from StudentInfo where ClassId = @ClassId";
                        //班级信息
                        string sqlDelClass = "delete from ClassInfo where ClassId = @ClassId";

                        SqlParameter[] paras = { new SqlParameter("@ClassId", classId) };
                        List<CommandInfo> listComs = new List<CommandInfo>();
                        CommandInfo comStudent = new CommandInfo()
                        {
                            CommandText = sqlDelStu,
                            IsProc = false,
                            Parameters = paras
                        };
                        listComs.Add(comStudent);

                        CommandInfo comClass = new CommandInfo()
                        {
                            CommandText = sqlDelClass,
                            IsProc = false,
                            Parameters = paras
                        };
                        listComs.Add(comClass);
                        //执行事务

                        bool bl = SqlHelper.ExecuteTrans(listComs);
                        if (bl)
                        {
                            MessageBox.Show("The information deleted successful!", "DeleteTip",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                            DataTable dtClass = (DataTable)dgvClassList.DataSource;

                            dtClass.Rows.Remove(dr);
                            dgvClassList.DataSource = dtClass;
                        }
                        else
                        {
                            MessageBox.Show("Error:Delete error!", "DeleteTip",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    

                }
            }
        }
        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            //获取要删除的数据ClasstId
            //判断删除的个数 =0 没有选择，提示选择

            //大于0继续

            //删除操作  事务  代码里启动事务

            List<int> listIds = new List<int>();
            for (int i = 0; i < dgvClassList.Rows.Count; ++i)
            {
                DataGridViewCheckBoxCell cell = dgvClassList.Rows[i].Cells["colCheck"] as DataGridViewCheckBoxCell;
                bool chk = Convert.ToBoolean(cell.Value);
                if (chk)
                {
                    DataRow dr = (dgvClassList.Rows[i].DataBoundItem as DataRowView).Row;
                    int classId = (int)dr["ClassId"];
                    listIds.Add(classId);
                }

            }
            //真删除
            if (listIds.Count == 0)
            {
                MessageBox.Show("Please choose Class to be deleted!", "DeleteTip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (MessageBox.Show("You absolutely want to delete the classes and related students?", "DeleteTip",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string sqlDelStu = "delete from StudentInfo where ClassId = @ClassId";
                    //班级信息
                    string sqlDelClass = "delete from ClassInfo where ClassId = @ClassId";
                    List<CommandInfo> listComs = new List<CommandInfo>();
                    foreach (int  id in listIds)
                    {
                        SqlParameter[] para = { new SqlParameter("@ClassId", id) };
                        CommandInfo comStudent = new CommandInfo()
                        {
                            CommandText = sqlDelStu,
                            IsProc = false,
                            Parameters = para
                        };
                        listComs.Add(comStudent);

                        CommandInfo comClass = new CommandInfo()
                        {
                            CommandText = sqlDelClass,
                            IsProc = false,
                            Parameters = para
                        };
                        listComs.Add(comClass);
                    }


                    bool bl = SqlHelper.ExecuteTrans(listComs);
                    if (bl)
                    {
                        MessageBox.Show("These information of Classes and related students has been deleted!", "DeleteTip",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DataTable dtClass = (DataTable)dgvClassList.DataSource;
                        string idStr = string.Join(",", listIds);
                        DataRow[] rows = dtClass.Select("ClassId in (" + idStr + ")");
                        foreach (DataRow dr in rows)
                        {
                            dtClass.Rows.Remove(dr);
                        }
                        dgvClassList.DataSource = dtClass;
                    }
                    else
                    {
                        MessageBox.Show("Fail to delete the information", "DeleteTip",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    
                }

            }
        }
    }
}
