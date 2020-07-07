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
    public partial class FrmGradeList : Form
    {
        private int flag = 0; // 0 添加 1 修改
        private int editGradeId = 0;//修改
        private string oldName = "";
        public FrmGradeList()
        {
            InitializeComponent();
        }

        private void FrmGradeList_Load(object sender, EventArgs e)
        {
            txtGradeName.Text = "";
            flag = 0;//initial as addition
            btnSubmit.Text = "Add";
            LoadGradeList();
        }
        /// <summary>
        /// 修改或删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGradeList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                //获取选择的单元格，判断
                DataGridViewCell cell = dgvGradeList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                DataRow dr = (dgvGradeList.Rows[e.RowIndex].DataBoundItem as DataRowView).Row;
                int gradeId = (int)dr["GradeId"];
                editGradeId = gradeId;
                if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "Update")
                {
                    //修改 年纪名称加载到上面的文本框
                    txtGradeName.Text = dr["GradeName"].ToString();
                    oldName = dr["GradeName"].ToString();
                    flag = 1;//修改状态
                    btnSubmit.Text = "Update";
                    
                    
                }
                else if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "Delete")
                {
                    if (MessageBox.Show("Are you sure you want to delete this Grade and its related Classes and Students?", "DeleteTip",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //单条删除
                        //班级删除过程 班级--学生 先删除学生 后删除班级 删除年级
                        //删除学生
                        string sqlDelStu = "delete from StudentInfo where ClassId in " +
                            "(select ClassId from ClassInfo where GradeId = @GradeId)";
                        //班级信息
                        string sqlDelClass = "delete from ClassInfo where GradeId = @GradeId";

                        string sqlDelGrade = "delete from GradeInfo where GradeId=@GradeId";

                        SqlParameter[] paras = { new SqlParameter("@GradeId", editGradeId) };
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

                        CommandInfo comGrade = new CommandInfo()
                        {
                            CommandText = sqlDelGrade,
                            IsProc = false,
                            Parameters = paras
                        };
                        listComs.Add(comGrade);
                        //执行事务

                        bool bl = SqlHelper.ExecuteTrans(listComs);
                        if (bl)
                        {
                            MessageBox.Show("The information deleted successful!", "DeleteTip",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                            DataTable dtGradeList = (DataTable)dgvGradeList.DataSource;

                            dtGradeList.Rows.Remove(dr);
                            dgvGradeList.DataSource = dtGradeList;
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
        private void LoadGradeList()
        {
            string sql = "select GradeId, GradeName from GradeInfo";
            DataTable dtGradeList = SqlHelper.GetDataTable(sql);
            dgvGradeList.DataSource = dtGradeList;
        }
        /// <summary>
        /// 添加状态设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (flag != 0)
            {
                flag = 0;
                btnSubmit.Text = "Add";
                txtGradeName.Text = "";
            }

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //接收输入
            string gradeName = txtGradeName.Text.Trim();
            
            //判断是否为空
            if (string.IsNullOrEmpty(gradeName))
            {
                MessageBox.Show("GradeName cannot be void!", "AddGradeTip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //查重
            //添加 查找整个年级
            //修改下 查找除本身的整个年级
            if(flag == 0) // add
            {
                string sqlExist = "select count(1) from GradeInfo where GradeName=@GradeName";
                SqlParameter paraName = new SqlParameter("@GradeName", gradeName);
                object o = SqlHelper.ExecuteScalar(sqlExist, paraName);
                if(o!= null && (int)o > 0)
                {
                    MessageBox.Show("GradeName has EXIST","AddGradeTip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //入库,返回年级编号
                string sqlAdd = "insert into GradeInfo(GradeName) values (@GradeName); select @@Identity";
                SqlParameter paraNamenew = new SqlParameter("@GradeName", gradeName);

                object oGradeId = SqlHelper.ExecuteScalar(sqlAdd, paraNamenew);
                if(oGradeId!=null && int.Parse(oGradeId.ToString()) > 0)
                {
                    MessageBox.Show($"Add Grade: {gradeName} Successful", "AddGradeTip",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //刷新
                    DataTable dt = (DataTable)dgvGradeList.DataSource;
                    DataRow dr = dt.NewRow();
                    dr["GradeId"] = oGradeId;
                    dr["GradeName"] = gradeName;
                    dt.Rows.Add(dr);
                    dgvGradeList.DataSource = dt;

                }
                else
                {
                    MessageBox.Show($"Fail to Add Grade: {gradeName}", "AddGradeTip",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



            }
            else if(flag == 1) // update
            {
                if(gradeName == oldName)
                {
                    MessageBox.Show($"GradeName has not Edit.", "UpdateGrade Tip", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //判断是否已存在
                string sqlExist = "select count(1) from GradeInfo " +
                    "where GradeName=@GradeName and GradeId<>@GradeId";
                SqlParameter[] paras =
                {
                    new SqlParameter("@GradeName",gradeName),
                    new SqlParameter("@GradeId",editGradeId)
                };
                object oCount = SqlHelper.ExecuteScalar(sqlExist, paras);
                if (oCount != null && (int)oCount > 0)
                {
                    MessageBox.Show("GradeName has Exist!", "UpdateGrade Tip",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //修改入库
                string sqlUpdate = "update GradeInfo set GradeName=@GradeName where GradeId = @GradeId";
                //SqlParameter paraNamenew = new SqlParameter("@GradeName", gradeName);
                SqlParameter[] parasNew =
                {
                    new SqlParameter("@GradeName",gradeName),
                    new SqlParameter("@GradeId",editGradeId)
                };
                object oGradeId = SqlHelper.ExecuteNonQuery(sqlUpdate, parasNew);
                if(oGradeId!=null && int.Parse(oGradeId.ToString()) > 0)
                {
                    MessageBox.Show($"Update Grade: {gradeName} Successful", "UpdateGradeTip",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //刷新
                    DataTable dt = (DataTable)dgvGradeList.DataSource;
                    DataRow[] drs = dt.Select("GradeId=" + editGradeId);
                    DataRow dr = drs[0];
                    dr["GradeName"] = gradeName;
                    dgvGradeList.DataSource = dt;
                    
                    dgvGradeList.DataSource = dt;

                }
                else
                {
                    MessageBox.Show($"Fail to Update Grade: {gradeName}", "UpdateGradeTip",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
        }
        /// <summary>
        /// 多条删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //获取要删除的数据ClasstId
            //判断删除的个数 =0 没有选择，提示选择

            //大于0继续

            //删除操作  事务  代码里启动事务

            List<int> listIds = new List<int>();
            for (int i = 0; i < dgvGradeList.Rows.Count; ++i)
            {
                DataGridViewCheckBoxCell cell = dgvGradeList.Rows[i].Cells["colCheck"] as DataGridViewCheckBoxCell;
                bool chk = Convert.ToBoolean(cell.Value);
                if (chk)
                {
                    DataRow dr = (dgvGradeList.Rows[i].DataBoundItem as DataRowView).Row;
                    int gradeId = (int)dr["GradeId"];
                    listIds.Add(gradeId);
                }

            }
            //真删除
            if (listIds.Count == 0)
            {
                MessageBox.Show("Please choose Grade to be deleted!", "DeleteTip",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (MessageBox.Show("You absolutely want to delete the Grade and related Classes and students?", "DeleteTip",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string sqlDelStu = "delete from StudentInfo where ClassId in " +
                            "(select ClassId from ClassInfo where GradeId = @GradeId)";
                    //班级信息
                    string sqlDelClass = "delete from ClassInfo where GradeId = @GradeId";

                    string sqlDelGrade = "delete from GradeInfo where GradeId=@GradeId";

                    //SqlParameter[] paras = { new SqlParameter("@GradeId", editGradeId) };
                    List<CommandInfo> listComs = new List<CommandInfo>();
                    foreach(int id in listIds)
                    {
                        SqlParameter[] para =
                        {
                            new SqlParameter("@GradeId",id)
                        };
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
                        CommandInfo comGrade = new CommandInfo()
                        {
                            CommandText = sqlDelGrade,
                            IsProc = false,
                            Parameters = para
                        };
                        listComs.Add(comGrade);

                    }


                    bool bl = SqlHelper.ExecuteTrans(listComs);
                    if (bl)
                    {
                        MessageBox.Show("These information of Grades and related classes and students has been deleted!", "DeleteTip",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DataTable dtGrade = (DataTable)dgvGradeList.DataSource;
                        string idStr = string.Join(",", listIds);
                        DataRow[] rows = dtGrade.Select("GradeId in (" + idStr + ")");
                        foreach (DataRow dr in rows)
                        {
                            dtGrade.Rows.Remove(dr);
                        }
                        dgvGradeList.DataSource = dtGrade;
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
