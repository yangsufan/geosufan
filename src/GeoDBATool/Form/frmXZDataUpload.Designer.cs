namespace GeoDBATool
{
    partial class frmXZDataUpload
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
            this.btnUpload = new DevComponents.DotNetBar.ButtonX();
            this.btnClose = new DevComponents.DotNetBar.ButtonX();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
            this.btnDel = new DevComponents.DotNetBar.ButtonX();
            this.btnClear = new DevComponents.DotNetBar.ButtonX();
            this.btnAdd = new DevComponents.DotNetBar.ButtonX();
            this.textBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textSource = new System.Windows.Forms.TextBox();
            this.btnSelect = new DevComponents.DotNetBar.ButtonX();
            this.btnInverse = new DevComponents.DotNetBar.ButtonX();
            this.btnAll = new DevComponents.DotNetBar.ButtonX();
            this.btnNewLayer = new DevComponents.DotNetBar.ButtonX();
            this.text_prj = new System.Windows.Forms.TextBox();
            this.btn_SelectPRJ = new DevComponents.DotNetBar.ButtonX();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpload
            // 
            this.btnUpload.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUpload.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpload.Location = new System.Drawing.Point(131, 388);
            this.btnUpload.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(104, 35);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "入库";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClose.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(988, 438);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "退  出";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // listView
            // 
            this.listView.CheckBoxes = true;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader3,
            this.columnHeader5});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.Location = new System.Drawing.Point(4, 22);
            this.listView.Margin = new System.Windows.Forms.Padding(4);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(849, 403);
            this.listView.TabIndex = 4;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "源文件名";
            this.columnHeader1.Width = 199;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "目标文件名";
            this.columnHeader2.Width = 155;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "图层描述";
            this.columnHeader4.Width = 118;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "入库状态";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "详细日志";
            this.columnHeader5.Width = 110;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.listView);
            this.groupBox.Location = new System.Drawing.Point(243, 1);
            this.groupBox.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox.Name = "groupBox";
            this.groupBox.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox.Size = new System.Drawing.Size(857, 429);
            this.groupBox.TabIndex = 6;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "文件列表";
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btnServer.Location = new System.Drawing.Point(189, 25);
            this.btnServer.Margin = new System.Windows.Forms.Padding(4);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(44, 26);
            this.btnServer.TabIndex = 32;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // btnDel
            // 
            this.btnDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDel.Location = new System.Drawing.Point(131, 276);
            this.btnDel.Margin = new System.Windows.Forms.Padding(4);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(104, 35);
            this.btnDel.TabIndex = 10;
            this.btnDel.Text = "删除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnClear
            // 
            this.btnClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClear.Location = new System.Drawing.Point(13, 388);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(104, 35);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "清空";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAdd.Location = new System.Drawing.Point(15, 276);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 35);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(13, 116);
            this.textBox.Margin = new System.Windows.Forms.Padding(4);
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(179, 25);
            this.textBox.TabIndex = 35;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textSource);
            this.groupBox1.Controls.Add(this.btnServer);
            this.groupBox1.Location = new System.Drawing.Point(5, 1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(237, 71);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "目标数据源";
            // 
            // textSource
            // 
            this.textSource.Location = new System.Drawing.Point(8, 25);
            this.textSource.Margin = new System.Windows.Forms.Padding(4);
            this.textSource.Name = "textSource";
            this.textSource.ReadOnly = true;
            this.textSource.Size = new System.Drawing.Size(179, 25);
            this.textSource.TabIndex = 33;
            // 
            // btnSelect
            // 
            this.btnSelect.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btnSelect.Location = new System.Drawing.Point(195, 116);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(44, 26);
            this.btnSelect.TabIndex = 39;
            this.btnSelect.Text = "选项";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnInverse
            // 
            this.btnInverse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnInverse.Location = new System.Drawing.Point(131, 331);
            this.btnInverse.Margin = new System.Windows.Forms.Padding(4);
            this.btnInverse.Name = "btnInverse";
            this.btnInverse.Size = new System.Drawing.Size(104, 35);
            this.btnInverse.TabIndex = 41;
            this.btnInverse.Text = "反选";
            this.btnInverse.Click += new System.EventHandler(this.btnInverse_Click);
            // 
            // btnAll
            // 
            this.btnAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAll.Location = new System.Drawing.Point(13, 331);
            this.btnAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(104, 35);
            this.btnAll.TabIndex = 40;
            this.btnAll.Text = "全选";
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnNewLayer
            // 
            this.btnNewLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNewLayer.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNewLayer.Location = new System.Drawing.Point(727, 438);
            this.btnNewLayer.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewLayer.Name = "btnNewLayer";
            this.btnNewLayer.Size = new System.Drawing.Size(95, 35);
            this.btnNewLayer.TabIndex = 1;
            this.btnNewLayer.Text = "新  增";
            this.btnNewLayer.Visible = false;
            this.btnNewLayer.Click += new System.EventHandler(this.btnNewLayer_Click);
            // 
            // text_prj
            // 
            this.text_prj.Enabled = false;
            this.text_prj.Location = new System.Drawing.Point(12, 201);
            this.text_prj.Margin = new System.Windows.Forms.Padding(4);
            this.text_prj.Name = "text_prj";
            this.text_prj.ReadOnly = true;
            this.text_prj.Size = new System.Drawing.Size(179, 25);
            this.text_prj.TabIndex = 35;
            this.text_prj.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // btn_SelectPRJ
            // 
            this.btn_SelectPRJ.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btn_SelectPRJ.Location = new System.Drawing.Point(193, 201);
            this.btn_SelectPRJ.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SelectPRJ.Name = "btn_SelectPRJ";
            this.btn_SelectPRJ.Size = new System.Drawing.Size(44, 26);
            this.btn_SelectPRJ.TabIndex = 39;
            this.btn_SelectPRJ.Text = "...";
            this.btn_SelectPRJ.Click += new System.EventHandler(this.btn_SelectPRJ_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 171);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 15);
            this.label1.TabIndex = 42;
            this.label1.Text = "设置空间参考(可选)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 90);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 15);
            this.label2.TabIndex = 42;
            this.label2.Text = "文件名自动加前缀(必填)";
            // 
            // frmXZDataUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 488);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnInverse);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btn_SelectPRJ);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.text_prj);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.btnNewLayer);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpload);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmXZDataUpload";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "森林资源数据入库";
            this.groupBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnUpload;
        private DevComponents.DotNetBar.ButtonX btnClose;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private DevComponents.DotNetBar.ButtonX btnServer;
        private DevComponents.DotNetBar.ButtonX btnDel;
        private DevComponents.DotNetBar.ButtonX btnClear;
        private DevComponents.DotNetBar.ButtonX btnAdd;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.ButtonX btnSelect;
        private DevComponents.DotNetBar.ButtonX btnInverse;
        private DevComponents.DotNetBar.ButtonX btnAll;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private DevComponents.DotNetBar.ButtonX btnNewLayer;
        private System.Windows.Forms.TextBox text_prj;
        private DevComponents.DotNetBar.ButtonX btn_SelectPRJ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}