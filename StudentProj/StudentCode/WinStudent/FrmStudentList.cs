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
        private void FrmStudentList_Load(object sender, EventArgs e)
        {

        }
    }
}
