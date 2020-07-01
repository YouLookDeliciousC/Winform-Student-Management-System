namespace WinStudent
{
    partial class FrmStudentList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cboClasses = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvStudentList = new System.Windows.Forms.DataGridView();
            this.colCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StudentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StuName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colDel = new System.Windows.Forms.DataGridViewLinkColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnFind);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.cboClasses);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 88);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Query condition";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(605, 36);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 26);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "Query";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(345, 36);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(152, 25);
            this.txtName.TabIndex = 3;
            // 
            // cboClasses
            // 
            this.cboClasses.FormattingEnabled = true;
            this.cboClasses.Location = new System.Drawing.Point(113, 39);
            this.cboClasses.Name = "cboClasses";
            this.cboClasses.Size = new System.Drawing.Size(144, 23);
            this.cboClasses.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(284, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Class:";
            // 
            // dgvStudentList
            // 
            this.dgvStudentList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStudentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudentList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheck,
            this.StudentId,
            this.StuName,
            this.ClassName,
            this.Sex,
            this.Phone,
            this.colEdit,
            this.colDel});
            this.dgvStudentList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvStudentList.Location = new System.Drawing.Point(0, 106);
            this.dgvStudentList.Name = "dgvStudentList";
            this.dgvStudentList.RowHeadersWidth = 51;
            this.dgvStudentList.RowTemplate.Height = 27;
            this.dgvStudentList.Size = new System.Drawing.Size(800, 344);
            this.dgvStudentList.TabIndex = 1;
            this.dgvStudentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStudentList_CellContentClick);
            // 
            // colCheck
            // 
            this.colCheck.HeaderText = "Select";
            this.colCheck.MinimumWidth = 6;
            this.colCheck.Name = "colCheck";
            // 
            // StudentId
            // 
            this.StudentId.DataPropertyName = "StudentId";
            this.StudentId.HeaderText = "ID";
            this.StudentId.MinimumWidth = 6;
            this.StudentId.Name = "StudentId";
            this.StudentId.ReadOnly = true;
            // 
            // StuName
            // 
            this.StuName.DataPropertyName = "StudentName";
            this.StuName.HeaderText = "Name";
            this.StuName.MinimumWidth = 6;
            this.StuName.Name = "StuName";
            this.StuName.ReadOnly = true;
            // 
            // ClassName
            // 
            this.ClassName.DataPropertyName = "ClassName";
            this.ClassName.HeaderText = "Class";
            this.ClassName.MinimumWidth = 6;
            this.ClassName.Name = "ClassName";
            this.ClassName.ReadOnly = true;
            // 
            // Sex
            // 
            this.Sex.DataPropertyName = "Sex";
            this.Sex.HeaderText = "Sex";
            this.Sex.MinimumWidth = 6;
            this.Sex.Name = "Sex";
            this.Sex.ReadOnly = true;
            // 
            // Phone
            // 
            this.Phone.DataPropertyName = "Phone";
            this.Phone.HeaderText = "Phone";
            this.Phone.MinimumWidth = 6;
            this.Phone.Name = "Phone";
            this.Phone.ReadOnly = true;
            // 
            // colEdit
            // 
            dataGridViewCellStyle1.NullValue = "Update";
            this.colEdit.DefaultCellStyle = dataGridViewCellStyle1;
            this.colEdit.HeaderText = "Update";
            this.colEdit.MinimumWidth = 6;
            this.colEdit.Name = "colEdit";
            // 
            // colDel
            // 
            dataGridViewCellStyle2.NullValue = "Delete";
            this.colDel.DefaultCellStyle = dataGridViewCellStyle2;
            this.colDel.HeaderText = "Delete";
            this.colDel.MinimumWidth = 6;
            this.colDel.Name = "colDel";
            // 
            // FrmStudentList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvStudentList);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmStudentList";
            this.Text = "Student List";
            this.Load += new System.EventHandler(this.FrmStudentList_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cboClasses;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvStudentList;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn StuName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sex;
        private System.Windows.Forms.DataGridViewTextBoxColumn Phone;
        private System.Windows.Forms.DataGridViewLinkColumn colEdit;
        private System.Windows.Forms.DataGridViewLinkColumn colDel;
    }
}