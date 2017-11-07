namespace GeoDataCenterFunLib
{
    partial class frmDocUpload
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
            this.labelArea = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.labelOpen = new System.Windows.Forms.Label();
            this.labelScale = new System.Windows.Forms.Label();
            this.listViewDoc = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.comboBoxArea = new System.Windows.Forms.ComboBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.comboBoxScale = new System.Windows.Forms.ComboBox();
            this.comboBoxOpen = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxYear = new System.Windows.Forms.ComboBox();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxSource = new System.Windows.Forms.ComboBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnInverse = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Location = new System.Drawing.Point(8, 138);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(59, 12);
            this.labelArea.TabIndex = 1;
            this.labelArea.Text = "行政区划:";
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(8, 96);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(59, 12);
            this.labelType.TabIndex = 1;
            this.labelType.Text = "专题类型:";
            // 
            // labelOpen
            // 
            this.labelOpen.AutoSize = true;
            this.labelOpen.Location = new System.Drawing.Point(8, 260);
            this.labelOpen.Name = "labelOpen";
            this.labelOpen.Size = new System.Drawing.Size(59, 12);
            this.labelOpen.TabIndex = 1;
            this.labelOpen.Text = "打开方式:";
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(8, 179);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(47, 12);
            this.labelScale.TabIndex = 1;
            this.labelScale.Text = "比例尺:";
            // 
            // listViewDoc
            // 
            this.listViewDoc.CheckBoxes = true;
            this.listViewDoc.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewDoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDoc.GridLines = true;
            this.listViewDoc.Location = new System.Drawing.Point(3, 17);
            this.listViewDoc.Name = "listViewDoc";
            this.listViewDoc.Size = new System.Drawing.Size(371, 405);
            this.listViewDoc.TabIndex = 2;
            this.listViewDoc.UseCompatibleStateImageBehavior = false;
            this.listViewDoc.View = System.Windows.Forms.View.Details;
            this.listViewDoc.SelectedIndexChanged += new System.EventHandler(this.listViewDoc_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "              文档名";
            this.columnHeader1.Width = 268;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "入库状态";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 95;
            // 
            // comboBoxArea
            // 
            this.comboBoxArea.FormattingEnabled = true;
            this.comboBoxArea.Location = new System.Drawing.Point(33, 155);
            this.comboBoxArea.Name = "comboBoxArea";
            this.comboBoxArea.Size = new System.Drawing.Size(150, 20);
            this.comboBoxArea.TabIndex = 3;
            this.comboBoxArea.Click += new System.EventHandler(this.comboBoxArea_Click);
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(33, 111);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(150, 20);
            this.comboBoxType.TabIndex = 3;
            // 
            // comboBoxScale
            // 
            this.comboBoxScale.FormattingEnabled = true;
            this.comboBoxScale.Location = new System.Drawing.Point(33, 200);
            this.comboBoxScale.Name = "comboBoxScale";
            this.comboBoxScale.Size = new System.Drawing.Size(150, 20);
            this.comboBoxScale.TabIndex = 3;
            // 
            // comboBoxOpen
            // 
            this.comboBoxOpen.FormattingEnabled = true;
            this.comboBoxOpen.Items.AddRange(new object[] {
            "内部打开",
            "外部打开"});
            this.comboBoxOpen.Location = new System.Drawing.Point(33, 275);
            this.comboBoxOpen.Name = "comboBoxOpen";
            this.comboBoxOpen.Size = new System.Drawing.Size(150, 20);
            this.comboBoxOpen.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(10, 397);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 27);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "文档入库";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(489, 443);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "退   出";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 223);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "年度:";
            // 
            // comboBoxYear
            // 
            this.comboBoxYear.FormattingEnabled = true;
            this.comboBoxYear.Location = new System.Drawing.Point(33, 237);
            this.comboBoxYear.Name = "comboBoxYear";
            this.comboBoxYear.Size = new System.Drawing.Size(150, 20);
            this.comboBoxYear.TabIndex = 3;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.listViewDoc);
            this.groupBox.Location = new System.Drawing.Point(196, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(377, 425);
            this.groupBox.TabIndex = 5;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "文档列表";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxSource);
            this.groupBox1.Location = new System.Drawing.Point(2, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 57);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "入库目录";
            // 
            // comboBoxSource
            // 
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Location = new System.Drawing.Point(10, 20);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new System.Drawing.Size(170, 20);
            this.comboBoxSource.TabIndex = 38;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(10, 358);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 27);
            this.btnUpload.TabIndex = 4;
            this.btnUpload.Text = "文档添加";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(107, 363);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 27);
            this.btnDel.TabIndex = 4;
            this.btnDel.Text = "文档删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(107, 397);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 27);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "清空列表";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnInverse
            // 
            this.btnInverse.Location = new System.Drawing.Point(107, 324);
            this.btnInverse.Name = "btnInverse";
            this.btnInverse.Size = new System.Drawing.Size(75, 27);
            this.btnInverse.TabIndex = 43;
            this.btnInverse.Text = "反选";
            this.btnInverse.UseVisualStyleBackColor = true;
            this.btnInverse.Click += new System.EventHandler(this.btnInverse_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(9, 324);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(76, 27);
            this.btnAll.TabIndex = 42;
            this.btnAll.Text = "全选";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // frmDocUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 466);
            this.Controls.Add(this.btnInverse);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.comboBoxOpen);
            this.Controls.Add(this.comboBoxYear);
            this.Controls.Add(this.comboBoxScale);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.comboBoxArea);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.labelOpen);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.labelArea);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDocUpload";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文档数据入库";
            this.Load += new System.EventHandler(this.frmDocUpload_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelOpen;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.ListView listViewDoc;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ComboBox comboBoxArea;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.ComboBox comboBoxScale;
        private System.Windows.Forms.ComboBox comboBoxOpen;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxYear;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxSource;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnInverse;
        private System.Windows.Forms.Button btnAll;
    }
}