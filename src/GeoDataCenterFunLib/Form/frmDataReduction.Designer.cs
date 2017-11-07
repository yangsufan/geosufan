namespace GeoDataCenterFunLib
{
    partial class frmDataReduction
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.datagwSource = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.btn_Detail = new System.Windows.Forms.Button();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.listBoxDetail = new System.Windows.Forms.ListBox();
            this.btn_Edit = new System.Windows.Forms.Button();
            this.btn_Del = new System.Windows.Forms.Button();
            this.labelDetail = new System.Windows.Forms.Label();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.comboBoxSource = new System.Windows.Forms.ComboBox();
            this.comboBoxSub = new System.Windows.Forms.ComboBox();
            this.comboBoxBig = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxYear = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxArea = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxScale = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_ExportNDB = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnInverse = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBoxGrid = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.datagwSource)).BeginInit();
            this.panelDetail.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.groupBoxGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // datagwSource
            // 
            this.datagwSource.AllowUserToAddRows = false;
            this.datagwSource.AllowUserToResizeColumns = false;
            this.datagwSource.AllowUserToResizeRows = false;
            this.datagwSource.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagwSource.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.datagwSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagwSource.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column1,
            this.Column2});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.datagwSource.DefaultCellStyle = dataGridViewCellStyle4;
            this.datagwSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagwSource.Location = new System.Drawing.Point(3, 17);
            this.datagwSource.Name = "datagwSource";
            this.datagwSource.RowHeadersVisible = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.datagwSource.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.datagwSource.RowTemplate.Height = 23;
            this.datagwSource.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.datagwSource.Size = new System.Drawing.Size(357, 432);
            this.datagwSource.TabIndex = 0;
            this.datagwSource.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.datagwSource_CellBeginEdit);
            this.datagwSource.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.datagwSource_CellValueChanged);
            // 
            // Column3
            // 
            this.Column3.HeaderText = "";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column3.Width = 20;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "数据名称";
            this.Column1.Name = "Column1";
            this.Column1.Width = 200;
            // 
            // Column2
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "图层名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 130;
            // 
            // btn_Clear
            // 
            this.btn_Clear.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Clear.Location = new System.Drawing.Point(538, 503);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(73, 29);
            this.btn_Clear.TabIndex = 2;
            this.btn_Clear.Text = "清    空";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Visible = false;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // btn_Detail
            // 
            this.btn_Detail.Location = new System.Drawing.Point(550, 497);
            this.btn_Detail.Name = "btn_Detail";
            this.btn_Detail.Size = new System.Drawing.Size(75, 36);
            this.btn_Detail.TabIndex = 4;
            this.btn_Detail.Text = "详细信息";
            this.btn_Detail.UseVisualStyleBackColor = true;
            this.btn_Detail.Visible = false;
            this.btn_Detail.Click += new System.EventHandler(this.btn_Detail_Click);
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.listBoxDetail);
            this.panelDetail.Location = new System.Drawing.Point(495, 569);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(196, 10);
            this.panelDetail.TabIndex = 5;
            this.panelDetail.Visible = false;
            // 
            // listBoxDetail
            // 
            this.listBoxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDetail.FormattingEnabled = true;
            this.listBoxDetail.ItemHeight = 12;
            this.listBoxDetail.Location = new System.Drawing.Point(0, 0);
            this.listBoxDetail.MultiColumn = true;
            this.listBoxDetail.Name = "listBoxDetail";
            this.listBoxDetail.Size = new System.Drawing.Size(196, 4);
            this.listBoxDetail.TabIndex = 0;
            this.listBoxDetail.UseWaitCursor = true;
            this.listBoxDetail.Visible = false;
            // 
            // btn_Edit
            // 
            this.btn_Edit.Location = new System.Drawing.Point(18, 360);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(78, 28);
            this.btn_Edit.TabIndex = 6;
            this.btn_Edit.Text = "修改";
            this.btn_Edit.UseVisualStyleBackColor = true;
            this.btn_Edit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // btn_Del
            // 
            this.btn_Del.Location = new System.Drawing.Point(112, 360);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(78, 28);
            this.btn_Del.TabIndex = 6;
            this.btn_Del.Text = "删除";
            this.btn_Del.UseVisualStyleBackColor = true;
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // labelDetail
            // 
            this.labelDetail.AutoSize = true;
            this.labelDetail.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDetail.Location = new System.Drawing.Point(513, 535);
            this.labelDetail.Name = "labelDetail";
            this.labelDetail.Size = new System.Drawing.Size(98, 14);
            this.labelDetail.TabIndex = 2;
            this.labelDetail.Text = "操作详细信息:";
            this.labelDetail.Visible = false;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.comboBoxSource);
            this.groupBox.Controls.Add(this.comboBoxSub);
            this.groupBox.Controls.Add(this.comboBoxBig);
            this.groupBox.Controls.Add(this.label5);
            this.groupBox.Controls.Add(this.comboBoxYear);
            this.groupBox.Controls.Add(this.label3);
            this.groupBox.Controls.Add(this.label4);
            this.groupBox.Controls.Add(this.label6);
            this.groupBox.Controls.Add(this.comboBoxArea);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.comboBoxScale);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Location = new System.Drawing.Point(9, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(195, 300);
            this.groupBox.TabIndex = 10;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "检索选项";
            // 
            // comboBoxSource
            // 
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Location = new System.Drawing.Point(33, 41);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new System.Drawing.Size(148, 20);
            this.comboBoxSource.TabIndex = 8;
            this.comboBoxSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxSource_SelectedIndexChanged);
            this.comboBoxSource.Click += new System.EventHandler(this.comboBoxYear_Click);
            // 
            // comboBoxSub
            // 
            this.comboBoxSub.FormattingEnabled = true;
            this.comboBoxSub.Location = new System.Drawing.Point(33, 259);
            this.comboBoxSub.Name = "comboBoxSub";
            this.comboBoxSub.Size = new System.Drawing.Size(148, 20);
            this.comboBoxSub.TabIndex = 8;
            this.comboBoxSub.Text = "所有小类业务";
            this.comboBoxSub.SelectedIndexChanged += new System.EventHandler(this.comboBoxSub_SelectedIndexChanged);
            this.comboBoxSub.Click += new System.EventHandler(this.comboBoxYear_Click);
            // 
            // comboBoxBig
            // 
            this.comboBoxBig.FormattingEnabled = true;
            this.comboBoxBig.Location = new System.Drawing.Point(33, 215);
            this.comboBoxBig.Name = "comboBoxBig";
            this.comboBoxBig.Size = new System.Drawing.Size(148, 20);
            this.comboBoxBig.TabIndex = 8;
            this.comboBoxBig.Text = "所有大类业务";
            this.comboBoxBig.SelectedIndexChanged += new System.EventHandler(this.comboBoxBig_SelectedIndexChanged);
            this.comboBoxBig.Click += new System.EventHandler(this.comboBoxYear_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "选择行政区:";
            // 
            // comboBoxYear
            // 
            this.comboBoxYear.FormattingEnabled = true;
            this.comboBoxYear.Location = new System.Drawing.Point(33, 82);
            this.comboBoxYear.Name = "comboBoxYear";
            this.comboBoxYear.Size = new System.Drawing.Size(148, 20);
            this.comboBoxYear.TabIndex = 8;
            this.comboBoxYear.Text = "所有年度";
            this.comboBoxYear.SelectedIndexChanged += new System.EventHandler(this.comboBoxYear_SelectedIndexChanged);
            this.comboBoxYear.Click += new System.EventHandler(this.comboBoxYear_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "选择比例尺:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "选择数据源:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 244);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "选择小类业务:";
            // 
            // comboBoxArea
            // 
            this.comboBoxArea.FormattingEnabled = true;
            this.comboBoxArea.Location = new System.Drawing.Point(33, 126);
            this.comboBoxArea.Name = "comboBoxArea";
            this.comboBoxArea.Size = new System.Drawing.Size(148, 20);
            this.comboBoxArea.TabIndex = 8;
            this.comboBoxArea.Text = "所有行政区";
            this.comboBoxArea.TextChanged += new System.EventHandler(this.comboBoxArea_TextChanged);
            this.comboBoxArea.Click += new System.EventHandler(this.comboBoxArea_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "选择大类业务:";
            // 
            // comboBoxScale
            // 
            this.comboBoxScale.FormattingEnabled = true;
            this.comboBoxScale.Location = new System.Drawing.Point(33, 170);
            this.comboBoxScale.Name = "comboBoxScale";
            this.comboBoxScale.Size = new System.Drawing.Size(148, 20);
            this.comboBoxScale.TabIndex = 8;
            this.comboBoxScale.Text = "所有比例尺";
            this.comboBoxScale.SelectedIndexChanged += new System.EventHandler(this.comboBoxScale_SelectedIndexChanged);
            this.comboBoxScale.Click += new System.EventHandler(this.comboBoxYear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "选择年度:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(112, 408);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 28);
            this.button1.TabIndex = 7;
            this.button1.Text = "退出";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_ExportNDB
            // 
            this.btn_ExportNDB.Location = new System.Drawing.Point(18, 408);
            this.btn_ExportNDB.Name = "btn_ExportNDB";
            this.btn_ExportNDB.Size = new System.Drawing.Size(78, 28);
            this.btn_ExportNDB.TabIndex = 7;
            this.btn_ExportNDB.Text = "下载";
            this.btn_ExportNDB.UseVisualStyleBackColor = true;
            this.btn_ExportNDB.Click += new System.EventHandler(this.btn_ExportNDB_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(18, 316);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(78, 28);
            this.btnAll.TabIndex = 6;
            this.btnAll.Text = "全选";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnInverse
            // 
            this.btnInverse.Location = new System.Drawing.Point(112, 316);
            this.btnInverse.Name = "btnInverse";
            this.btnInverse.Size = new System.Drawing.Size(78, 28);
            this.btnInverse.TabIndex = 6;
            this.btnInverse.Text = "反选";
            this.btnInverse.UseVisualStyleBackColor = true;
            this.btnInverse.Click += new System.EventHandler(this.btnInverse_Click);
            // 
            // groupBoxGrid
            // 
            this.groupBoxGrid.Controls.Add(this.datagwSource);
            this.groupBoxGrid.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBoxGrid.Location = new System.Drawing.Point(220, 0);
            this.groupBoxGrid.Name = "groupBoxGrid";
            this.groupBoxGrid.Size = new System.Drawing.Size(363, 452);
            this.groupBoxGrid.TabIndex = 11;
            this.groupBoxGrid.TabStop = false;
            this.groupBoxGrid.Text = "当前入库所有矢量数据:";
            // 
            // frmDataReduction
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(583, 452);
            this.Controls.Add(this.groupBoxGrid);
            this.Controls.Add(this.labelDetail);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.panelDetail);
            this.Controls.Add(this.btn_Detail);
            this.Controls.Add(this.btnInverse);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_Edit);
            this.Controls.Add(this.btn_ExportNDB);
            this.Controls.Add(this.btn_Del);
            this.Controls.Add(this.btnAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDataReduction";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "矢量数据整理";
            this.Load += new System.EventHandler(this.frmDataReduction_Load);
            ((System.ComponentModel.ISupportInitialize)(this.datagwSource)).EndInit();
            this.panelDetail.ResumeLayout(false);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.groupBoxGrid.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView datagwSource;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Button btn_Detail;
        private System.Windows.Forms.Panel panelDetail;
        private System.Windows.Forms.ListBox listBoxDetail;
        private System.Windows.Forms.Button btn_Edit;
        private System.Windows.Forms.Button btn_Del;
        private System.Windows.Forms.Label labelDetail;
        private System.Windows.Forms.Button btn_ExportNDB;
        private System.Windows.Forms.ComboBox comboBoxBig;
        private System.Windows.Forms.ComboBox comboBoxYear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxScale;
        private System.Windows.Forms.ComboBox comboBoxArea;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnInverse;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBoxGrid;
        private System.Windows.Forms.ComboBox comboBoxSource;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxSub;
        private System.Windows.Forms.Label label6;
    }
}