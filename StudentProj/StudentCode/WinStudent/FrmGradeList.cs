using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinStudent
{
    public partial class FrmGradeList : Form
    {
        public FrmGradeList()
        {
            InitializeComponent();
        }

        private void FrmGradeList_Load(object sender, EventArgs e)
        {
            string sql = "select GradeId, GradeName from GradeInfo";
            DataTable dtGradeList = SqlHelper.GetDataTable(sql);
            dgvGradeList.DataSource = dtGradeList;
        }

        private void dgvGradeList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
