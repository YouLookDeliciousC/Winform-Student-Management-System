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

        private void dgvGradeList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
                }



            }
            else if(flag == 1) // update
            {
                string sqlExist = "select count(1) from GradeInfo " +
                    "where GradeName=@GradeName and GradeId<>@GradeId";
            }
        }
    }
}
