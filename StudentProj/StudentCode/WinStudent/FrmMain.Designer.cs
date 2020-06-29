namespace WinStudent
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.miStudent = new System.Windows.Forms.ToolStripMenuItem();
            this.subAddStudent = new System.Windows.Forms.ToolStripMenuItem();
            this.subStudentList = new System.Windows.Forms.ToolStripMenuItem();
            this.miClass = new System.Windows.Forms.ToolStripMenuItem();
            this.subAddClass = new System.Windows.Forms.ToolStripMenuItem();
            this.subClassList = new System.Windows.Forms.ToolStripMenuItem();
            this.miGrade = new System.Windows.Forms.ToolStripMenuItem();
            this.subGradeList = new System.Windows.Forms.ToolStripMenuItem();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miStudent,
            this.miClass,
            this.miGrade,
            this.miExit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(871, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // miStudent
            // 
            this.miStudent.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subAddStudent,
            this.subStudentList});
            this.miStudent.Name = "miStudent";
            this.miStudent.Size = new System.Drawing.Size(182, 24);
            this.miStudent.Text = "Student Management";
            this.miStudent.Click += new System.EventHandler(this.studentManagementToolStripMenuItem_Click);
            // 
            // subAddStudent
            // 
            this.subAddStudent.Name = "subAddStudent";
            this.subAddStudent.Size = new System.Drawing.Size(224, 26);
            this.subAddStudent.Text = "Add Student";
            this.subAddStudent.Click += new System.EventHandler(this.addStudentToolStripMenuItem_Click);
            // 
            // subStudentList
            // 
            this.subStudentList.Name = "subStudentList";
            this.subStudentList.Size = new System.Drawing.Size(224, 26);
            this.subStudentList.Text = "Student List";
            this.subStudentList.Click += new System.EventHandler(this.subStudentList_Click);
            // 
            // miClass
            // 
            this.miClass.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subAddClass,
            this.subClassList});
            this.miClass.Name = "miClass";
            this.miClass.Size = new System.Drawing.Size(160, 26);
            this.miClass.Text = "Class Management";
            // 
            // subAddClass
            // 
            this.subAddClass.Name = "subAddClass";
            this.subAddClass.Size = new System.Drawing.Size(224, 26);
            this.subAddClass.Text = "Add Class";
            // 
            // subClassList
            // 
            this.subClassList.Name = "subClassList";
            this.subClassList.Size = new System.Drawing.Size(224, 26);
            this.subClassList.Text = "Class List";
            // 
            // miGrade
            // 
            this.miGrade.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subGradeList});
            this.miGrade.Name = "miGrade";
            this.miGrade.Size = new System.Drawing.Size(168, 26);
            this.miGrade.Text = "Grade Management";
            // 
            // subGradeList
            // 
            this.subGradeList.Name = "subGradeList";
            this.subGradeList.Size = new System.Drawing.Size(224, 26);
            this.subGradeList.Text = "Grade List";
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(106, 26);
            this.miExit.Text = "Exit System";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 487);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "Student Management System Main Page";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miStudent;
        private System.Windows.Forms.ToolStripMenuItem subAddStudent;
        private System.Windows.Forms.ToolStripMenuItem subStudentList;
        private System.Windows.Forms.ToolStripMenuItem miClass;
        private System.Windows.Forms.ToolStripMenuItem subAddClass;
        private System.Windows.Forms.ToolStripMenuItem subClassList;
        private System.Windows.Forms.ToolStripMenuItem miGrade;
        private System.Windows.Forms.ToolStripMenuItem subGradeList;
        private System.Windows.Forms.ToolStripMenuItem miExit;
    }
}