namespace GeoDBATool
{
    partial class frmTYDataUpload
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textSource = new System.Windows.Forms.TextBox();
            this.btnInverse = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnNewLayer = new System.Windows.Forms.Button();
            this.text_prj = new System.Windows.Forms.TextBox();
            this.btn_SelectPRJ = new DevComponents.DotNetBar.ButtonX();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_CheckExist = new DevComponents.DotNetBar.ButtonX();
            this.rbMDB = new System.Windows.Forms.RadioButton();
            this.rbGDB = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_MutiRepace = new System.Windows.Forms.Button();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpload.Location = new System.Drawing.Point(87, 332);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(78, 28);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "入库";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(794, 368);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(71, 28);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "退  出";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // listView
            // 
            this.listView.CheckBoxes = true;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader3,
            this.columnHeader6});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.Location = new System.Drawing.Point(3, 17);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(708, 342);
            this.listView.TabIndex = 4;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.Click += new System.EventHandler(this.listView_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "源文件名";
            this.columnHeader1.Width = 199;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "目标文件名";
            this.columnHeader2.Width = 75;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "图层描述";
            this.columnHeader4.Width = 71;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "是否存在同名";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 160;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "入库状态";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 90;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.listView);
            this.groupBox.Location = new System.Drawing.Point(182, 1);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(714, 362);
            this.groupBox.TabIndex = 6;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "文件列表";
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btnServer.Location = new System.Drawing.Point(142, 20);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(33, 21);
            this.btnServer.TabIndex = 32;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(88, 233);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(78, 28);
            this.btnDel.TabIndex = 10;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(3, 332);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(78, 28);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(4, 232);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(78, 28);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(10, 80);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(135, 21);
            this.textBox.TabIndex = 35;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textSource);
            this.groupBox1.Controls.Add(this.btnServer);
            this.groupBox1.Location = new System.Drawing.Point(4, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 49);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "目标数据源";
            // 
            // textSource
            // 
            this.textSource.Location = new System.Drawing.Point(6, 20);
            this.textSource.Name = "textSource";
            this.textSource.ReadOnly = true;
            this.textSource.Size = new System.Drawing.Size(135, 21);
            this.textSource.TabIndex = 33;
            // 
            // btnInverse
            // 
            this.btnInverse.Location = new System.Drawing.Point(87, 280);
            this.btnInverse.Name = "btnInverse";
            this.btnInverse.Size = new System.Drawing.Size(78, 28);
            this.btnInverse.TabIndex = 41;
            this.btnInverse.Text = "反选";
            this.btnInverse.UseVisualStyleBackColor = true;
            this.btnInverse.Click += new System.EventHandler(this.btnInverse_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(4, 280);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(78, 28);
            this.btnAll.TabIndex = 40;
            this.btnAll.Text = "全选";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnNewLayer
            // 
            this.btnNewLayer.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNewLayer.Location = new System.Drawing.Point(463, 359);
            this.btnNewLayer.Name = "btnNewLayer";
            this.btnNewLayer.Size = new System.Drawing.Size(71, 28);
            this.btnNewLayer.TabIndex = 1;
            this.btnNewLayer.Text = "新  增";
            this.btnNewLayer.UseVisualStyleBackColor = true;
            this.btnNewLayer.Visible = false;
            this.btnNewLayer.Click += new System.EventHandler(this.btnNewLayer_Click);
            // 
            // text_prj
            // 
            this.text_prj.Enabled = false;
            this.text_prj.Location = new System.Drawing.Point(12, 131);
            this.text_prj.Name = "text_prj";
            this.text_prj.ReadOnly = true;
            this.text_prj.Size = new System.Drawing.Size(130, 21);
            this.text_prj.TabIndex = 35;
            // 
            // btn_SelectPRJ
            // 
            this.btn_SelectPRJ.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btn_SelectPRJ.Location = new System.Drawing.Point(145, 131);
            this.btn_SelectPRJ.Name = "btn_SelectPRJ";
            this.btn_SelectPRJ.Size = new System.Drawing.Size(33, 21);
            this.btn_SelectPRJ.TabIndex = 39;
            this.btn_SelectPRJ.Text = "...";
            this.btn_SelectPRJ.Click += new System.EventHandler(this.btn_SelectPRJ_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 42;
            this.label1.Text = "设置空间参考(非必填)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 42;
            this.label2.Text = "设置数据集名称(必填)";
            // 
            // btn_CheckExist
            // 
            this.btn_CheckExist.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btn_CheckExist.Location = new System.Drawing.Point(146, 80);
            this.btn_CheckExist.Name = "btn_CheckExist";
            this.btn_CheckExist.Size = new System.Drawing.Size(33, 21);
            this.btn_CheckExist.TabIndex = 39;
            this.btn_CheckExist.Text = "检查";
            this.btn_CheckExist.Click += new System.EventHandler(this.btn_CheckExist_Click);
            // 
            // rbMDB
            // 
            this.rbMDB.AutoSize = true;
            this.rbMDB.Checked = true;
            this.rbMDB.Location = new System.Drawing.Point(8, 20);
            this.rbMDB.Name = "rbMDB";
            this.rbMDB.Size = new System.Drawing.Size(41, 16);
            this.rbMDB.TabIndex = 43;
            this.rbMDB.TabStop = true;
            this.rbMDB.Text = "mdb";
            this.rbMDB.UseVisualStyleBackColor = true;
            // 
            // rbGDB
            // 
            this.rbGDB.AutoSize = true;
            this.rbGDB.Location = new System.Drawing.Point(91, 20);
            this.rbGDB.Name = "rbGDB";
            this.rbGDB.Size = new System.Drawing.Size(41, 16);
            this.rbGDB.TabIndex = 43;
            this.rbGDB.Text = "gdb";
            this.rbGDB.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbGDB);
            this.groupBox2.Controls.Add(this.rbMDB);
            this.groupBox2.Location = new System.Drawing.Point(12, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(167, 38);
            this.groupBox2.TabIndex = 44;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "源数据类型";
            // 
            // btn_MutiRepace
            // 
            this.btn_MutiRepace.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_MutiRepace.Location = new System.Drawing.Point(606, 369);
            this.btn_MutiRepace.Name = "btn_MutiRepace";
            this.btn_MutiRepace.Size = new System.Drawing.Size(71, 28);
            this.btn_MutiRepace.TabIndex = 1;
            this.btn_MutiRepace.Text = "批量操作";
            this.btn_MutiRepace.UseVisualStyleBackColor = true;
            this.btn_MutiRepace.Click += new System.EventHandler(this.btn_MutiRepace_Click);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "详细日志";
            this.columnHeader6.Width = 110;
            // 
            // frmTYDataUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 399);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnInverse);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btn_CheckExist);
            this.Controls.Add(this.btn_SelectPRJ);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.text_prj);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.btnNewLayer);
            this.Controls.Add(this.btn_MutiRepace);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTYDataUpload";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "通用数据入库";
            this.groupBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private DevComponents.DotNetBar.ButtonX btnServer;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnInverse;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btnNewLayer;
        private System.Windows.Forms.TextBox text_prj;
        private DevComponents.DotNetBar.ButtonX btn_SelectPRJ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textSource;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.ButtonX btn_CheckExist;
        private System.Windows.Forms.RadioButton rbMDB;
        private System.Windows.Forms.RadioButton rbGDB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btn_MutiRepace;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }
}