namespace GeoDataCenterFunLib
{
    partial class frmDataUpload
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
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.checkBoxNew = new System.Windows.Forms.CheckBox();
            this.checkboxLegal = new System.Windows.Forms.CheckBox();
            this.checkboxPf = new System.Windows.Forms.CheckBox();
            this.textBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxSource = new System.Windows.Forms.ComboBox();
            this.btnSelect = new DevComponents.DotNetBar.ButtonX();
            this.btnInverse = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.btnNewLayer = new System.Windows.Forms.Button();
            this.groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpload.Location = new System.Drawing.Point(14, 292);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(78, 28);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "文件入库";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(645, 325);
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
            this.columnHeader3});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.Location = new System.Drawing.Point(3, 17);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(534, 301);
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
            // columnHeader3
            // 
            this.columnHeader3.Text = "入库状态";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 100;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.listView);
            this.groupBox.Location = new System.Drawing.Point(182, 1);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(540, 321);
            this.groupBox.TabIndex = 6;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "文件列表";
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btnServer.Location = new System.Drawing.Point(128, 20);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(33, 21);
            this.btnServer.TabIndex = 32;
            this.btnServer.Text = "...";
            this.btnServer.Visible = false;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(98, 247);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(78, 28);
            this.btnDel.TabIndex = 10;
            this.btnDel.Text = "文件删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(98, 292);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(78, 28);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "清空列表";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(14, 246);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(78, 28);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "文件添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // checkBoxNew
            // 
            this.checkBoxNew.AutoSize = true;
            this.checkBoxNew.Location = new System.Drawing.Point(13, 139);
            this.checkBoxNew.Name = "checkBoxNew";
            this.checkBoxNew.Size = new System.Drawing.Size(144, 16);
            this.checkBoxNew.TabIndex = 36;
            this.checkBoxNew.Text = "覆盖文件名相同的数据";
            this.checkBoxNew.UseVisualStyleBackColor = true;
            this.checkBoxNew.CheckedChanged += new System.EventHandler(this.checkBoxNew_CheckedChanged);
            // 
            // checkboxLegal
            // 
            this.checkboxLegal.AutoSize = true;
            this.checkboxLegal.Location = new System.Drawing.Point(13, 170);
            this.checkboxLegal.Name = "checkboxLegal";
            this.checkboxLegal.Size = new System.Drawing.Size(132, 16);
            this.checkboxLegal.TabIndex = 34;
            this.checkboxLegal.Text = "进行文件名合法检查";
            this.checkboxLegal.UseVisualStyleBackColor = true;
            this.checkboxLegal.CheckedChanged += new System.EventHandler(this.checkboxLegal_CheckedChanged);
            // 
            // checkboxPf
            // 
            this.checkboxPf.AutoSize = true;
            this.checkboxPf.Location = new System.Drawing.Point(14, 80);
            this.checkboxPf.Name = "checkboxPf";
            this.checkboxPf.Size = new System.Drawing.Size(120, 16);
            this.checkboxPf.TabIndex = 33;
            this.checkboxPf.Text = "文件名自动加前缀";
            this.checkboxPf.UseVisualStyleBackColor = true;
            this.checkboxPf.CheckedChanged += new System.EventHandler(this.checkboxPf_CheckedChanged);
            // 
            // textBox
            // 
            this.textBox.Enabled = false;
            this.textBox.Location = new System.Drawing.Point(15, 102);
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(130, 21);
            this.textBox.TabIndex = 35;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxSource);
            this.groupBox1.Controls.Add(this.btnServer);
            this.groupBox1.Location = new System.Drawing.Point(4, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(167, 57);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "目标数据源";
            // 
            // comboBoxSource
            // 
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Location = new System.Drawing.Point(10, 20);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new System.Drawing.Size(151, 20);
            this.comboBoxSource.TabIndex = 38;
            // 
            // btnSelect
            // 
            this.btnSelect.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point(143, 102);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(33, 21);
            this.btnSelect.TabIndex = 39;
            this.btnSelect.Text = "选项";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnInverse
            // 
            this.btnInverse.Location = new System.Drawing.Point(98, 204);
            this.btnInverse.Name = "btnInverse";
            this.btnInverse.Size = new System.Drawing.Size(78, 28);
            this.btnInverse.TabIndex = 41;
            this.btnInverse.Text = "反选";
            this.btnInverse.UseVisualStyleBackColor = true;
            this.btnInverse.Click += new System.EventHandler(this.btnInverse_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(15, 204);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(78, 28);
            this.btnAll.TabIndex = 40;
            this.btnAll.Text = "全选";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "图层描述";
            this.columnHeader4.Width = 71;
            // 
            // btnNewLayer
            // 
            this.btnNewLayer.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNewLayer.Location = new System.Drawing.Point(542, 325);
            this.btnNewLayer.Name = "btnNewLayer";
            this.btnNewLayer.Size = new System.Drawing.Size(71, 28);
            this.btnNewLayer.TabIndex = 1;
            this.btnNewLayer.Text = "新  增";
            this.btnNewLayer.UseVisualStyleBackColor = true;
            this.btnNewLayer.Click += new System.EventHandler(this.btnNewLayer_Click);
            // 
            // frmDataUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 354);
            this.Controls.Add(this.btnInverse);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.checkBoxNew);
            this.Controls.Add(this.checkboxLegal);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.checkboxPf);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.btnNewLayer);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDataUpload";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "矢量数据入库";
            this.Load += new System.EventHandler(this.frmDataUpload_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
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
        private System.Windows.Forms.CheckBox checkBoxNew;
        private System.Windows.Forms.CheckBox checkboxLegal;
        private System.Windows.Forms.CheckBox checkboxPf;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxSource;
        private DevComponents.DotNetBar.ButtonX btnSelect;
        private System.Windows.Forms.Button btnInverse;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btnNewLayer;
    }
}