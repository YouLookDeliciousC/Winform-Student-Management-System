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
            LoadClasses();
        }

        private void LoadClasses()
        {
            //获取数据
            string sql = "select ClassId,ClassName,GradeName from ClassInfo c " +
                "inner join GradeInfo g on c.GradeId=g.GradeId";
            DataTable dtClasses = SqlHelper.GetDataTable(sql);
            if (dtClasses.Rows.Count > 0)
            {
                foreach(DataRow dr in dtClasses.Rows)
                {
                    string className = dr["ClassName"].ToString();
                    string gradeName = dr["GradeName"].ToString();
                    dr["ClassName"] = className + gradeName;
                }
            }
            DataRow drnew = dtClasses.NewRow();
            drnew["ClassId"] = 0;
            drnew["ClassName"] = "Please Choose";

            dtClasses.Rows.InsertAt(drnew, 0);

            cboClasses.DataSource = dtClasses;
            cboClasses.DisplayMember = "ClassName";
            cboClasses.ValueMember = "ClassId";
        }
    }
}
