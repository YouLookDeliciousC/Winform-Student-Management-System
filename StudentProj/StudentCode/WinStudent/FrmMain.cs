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
        /// <summary>
        /// 新增班级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subAddClass_Click(object sender, EventArgs e)
        {
            FrmAddClass fAddClass = new FrmAddClass();
            fAddClass.MdiParent = this;
            fAddClass.Show();
        }
        /// <summary>
        /// 班级列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subClassList_Click(object sender, EventArgs e)
        {
            bool bl = CheckForm(typeof(FrmClassList).Name) ; //type 获取指定类的类对象并使用对象信息
            if (!bl)
            {
                FrmClassList fClassList = new FrmClassList();
                fClassList.MdiParent = this;
                fClassList.Show();
            }
        }
        /// <summary>
        /// 年纪列表 也只能打开一个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subGradeList_Click(object sender, EventArgs e)
        {
            bool bl = CheckForm(typeof(FrmGradeList).Name); //type 获取指定类的类对象并使用对象信息
            if (!bl)
            {
                FrmGradeList fGradeList = new FrmGradeList();
                fGradeList.MdiParent = this;
                fGradeList.Show();
            }

        }
        /// <summary>
        /// 窗体关闭，退出应用程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to exit the application?", "Logout Tip", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Application.ExitThread(); //退出消息循环，关闭窗口
            }
            else
            {
                e.Cancel = true; //手动取消
            }
            
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
