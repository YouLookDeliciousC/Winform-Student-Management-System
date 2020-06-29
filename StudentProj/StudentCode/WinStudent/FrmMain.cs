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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void studentManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 新增学生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAddStudent fAddSudent = new FrmAddStudent();
            fAddSudent.MdiParent = this;
            fAddSudent.Show();
        }
        /// <summary>
        /// 学生列表 只能开启一个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subStudentList_Click(object sender, EventArgs e)
        {
            //方法一，使用单例
            /*FrmStudentList fStudentList = FrmStudentList.CreateInstance();
            fStudentList.MdiParent = this;
            fStudentList.Show();*/
            //方法二：遍历所有存在的窗体
            bool bl = CheckForm("FrmStudentList");
            
            if (!bl)
            {
                FrmStudentList fStudentList = new FrmStudentList();
                fStudentList.MdiParent = this;
                fStudentList.Show();
            }
        }
        /// <summary>
        /// 检查窗体是否存在
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        private bool CheckForm(string formName)
        {
            bool bl = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == formName)
                {
                    bl = true;
                    f.Activate();
                    break;
                }
            }
            return bl;
        }
    }
}
