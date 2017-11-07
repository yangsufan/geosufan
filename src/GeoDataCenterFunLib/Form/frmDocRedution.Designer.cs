namespace GeoDataCenterFunLib
{
    partial class frmDocRedution
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.comboBoxCatalog = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxSub = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxYear = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxArea = new System.Windows.Forms.ComboBox();
            this.comboBoxScale = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxGrid = new System.Windows.Forms.GroupBox();
            this.datagwSource = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_Export = new System.Windows.Forms.Button();
            this.btn_Del = new System.Windows.Forms.Button();
            this.btnInverse = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.groupBox.SuspendLayout();
            this.groupBoxGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagwSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.comboBoxCatalog);
            this.groupBox.Controls.Add(this.label4);
            this.groupBox.Controls.Add(this.comboBoxSub);
            this.groupBox.Controls.Add(this.label5);
            this.groupBox.Controls.Add(this.comboBoxYear);
            this.groupBox.Controls.Add(this.label3);
            this.groupBox.Controls.Add(this.label6);
            this.groupBox.Controls.Add(this.comboBoxArea);
            this.groupBox.Controls.Add(this.comboBoxScale);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Location = new System.Drawing.Point(12, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(195, 246);
            this.groupBox.TabIndex = 11;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "检索选项";
            // 
            // comboBoxCatalog
            // 
            this.comboBoxCatalog.FormattingEnabled = true;
            this.comboBoxCatalog.Location = new System.Drawing.Point(31, 32);
            this.comboBoxCatalog.Name = "comboBoxCatalog";
            this.comboBoxCatalog.Size = new System.Drawing.Size(148, 20);
            this.comboBoxCatalog.TabIndex = 11;
            this.comboBoxCatalog.SelectedIndexChanged += new System.EventHandler(this.comboBoxCatalog_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "选择目录:";
            // 
            // comboBoxSub
            // 
            this.comboBoxSub.FormattingEnabled = true;
            this.comboBoxSub.Location = new System.Drawing.Point(31, 199);
            this.comboBoxSub.Name = "comboBoxSub";
            this.comboBoxSub.Size = new System.Drawing.Size(148, 20);
            this.comboBoxSub.TabIndex = 8;
            this.comboBoxSub.Text = "所有专题类型";
            this.comboBoxSub.SelectedIndexChanged += new System.EventHandler(this.comboBoxSub_SelectedIndexChanged);
            this.comboBoxSub.Click += new System.EventHandler(this.comboBoxYear_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "选择行政区:";
            // 
            // comboBoxYear
            // 
            this.comboBoxYear.FormattingEnabled = true;
            this.comboBoxYear.Location = new System.Drawing.Point(31, 73);
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
            this.label3.Location = new System.Drawing.Point(8, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "选择比例尺:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 184);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "选择专题类型:";
            // 
            // comboBoxArea
            // 
            this.comboBoxArea.FormattingEnabled = true;
            this.comboBoxArea.Location = new System.Drawing.Point(31, 117);
            this.comboBoxArea.Name = "comboBoxArea";
            this.comboBoxArea.Size = new System.Drawing.Size(148, 20);
            this.comboBoxArea.TabIndex = 8;
            this.comboBoxArea.Text = "所有行政区";
            this.comboBoxArea.TextChanged += new System.EventHandler(this.comboBoxArea_TextChanged);
            this.comboBoxArea.Click += new System.EventHandler(this.comboBoxArea_Click);
            // 
            // comboBoxScale
            // 
            this.comboBoxScale.FormattingEnabled = true;
            this.comboBoxScale.Location = new System.Drawing.Point(31, 161);
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
            this.label1.Location = new System.Drawing.Point(8, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "选择年度:";
            // 
            // groupBoxGrid
            // 
            this.groupBoxGrid.Controls.Add(this.datagwSource);
            this.groupBoxGrid.Location = new System.Drawing.Point(213, 0);
            this.groupBoxGrid.Name = "groupBoxGrid";
            this.groupBoxGrid.Size = new System.Drawing.Size(238, 347);
            this.groupBoxGrid.TabIndex = 12;
            this.groupBoxGrid.TabStop = false;
            this.groupBoxGrid.Text = "当前入库所有文档:";
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
            this.Column1});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.datagwSource.DefaultCellStyle = dataGridViewCellStyle3;
            this.datagwSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagwSource.Location = new System.Drawing.Point(3, 17);
            this.datagwSource.Name = "datagwSource";
            this.datagwSource.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.datagwSource.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.datagwSource.RowTemplate.Height = 23;
            this.datagwSource.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.datagwSource.Size = new System.Drawing.Size(232, 327);
            this.datagwSource.TabIndex = 0;
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
            this.Column1.HeaderText = "文档名称";
            this.Column1.Name = "Column1";
            this.Column1.Width = 200;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(361, 353);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 28);
            this.button1.TabIndex = 16;
            this.button1.Text = "退出";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_Export
            // 
            this.btn_Export.Location = new System.Drawing.Point(19, 319);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(78, 28);
            this.btn_Export.TabIndex = 17;
            this.btn_Export.Text = "下载";
            this.btn_Export.UseVisualStyleBackColor = true;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // btn_Del
            // 
            this.btn_Del.Location = new System.Drawing.Point(113, 319);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(78, 28);
            this.btn_Del.TabIndex = 15;
            this.btn_Del.Text = "删除";
            this.btn_Del.UseVisualStyleBackColor = true;
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // btnInverse
            // 
            this.btnInverse.Location = new System.Drawing.Point(113, 275);
            this.btnInverse.Name = "btnInverse";
            this.btnInverse.Size = new System.Drawing.Size(78, 28);
            this.btnInverse.TabIndex = 13;
            this.btnInverse.Text = "反选";
            this.btnInverse.UseVisualStyleBackColor = true;
            this.btnInverse.Click += new System.EventHandler(this.btnInverse_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(20, 275);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(78, 28);
            this.btnAll.TabIndex = 14;
            this.btnAll.Text = "全选";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // frmDocRedution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 385);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_Export);
            this.Controls.Add(this.btn_Del);
            this.Controls.Add(this.btnInverse);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.groupBoxGrid);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDocRedution";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文档数据整理";
            this.Load += new System.EventHandler(this.frmDocRedution_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.groupBoxGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagwSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.ComboBox comboBoxSub;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxYear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxArea;
        private System.Windows.Forms.ComboBox comboBoxScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxGrid;
        private System.Windows.Forms.DataGridView datagwSource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_Export;
        private System.Windows.Forms.Button btn_Del;
        private System.Windows.Forms.Button btnInverse;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.ComboBox comboBoxCatalog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Label label6;
    }
}