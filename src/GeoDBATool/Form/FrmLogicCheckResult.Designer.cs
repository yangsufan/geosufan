namespace GeoDBATool
{
    partial class FrmLogicCheckResult
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
            this.dgvCheckResultData = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dgvCheckItem = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.btnExportDetialTable = new DevComponents.DotNetBar.ButtonX();
            this.btnExportDataTable = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckResultData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckItem)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvCheckResultData
            // 
            this.dgvCheckResultData.AllowUserToAddRows = false;
            this.dgvCheckResultData.AllowUserToDeleteRows = false;
            this.dgvCheckResultData.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvCheckResultData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCheckResultData.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCheckResultData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCheckResultData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvCheckResultData.Location = new System.Drawing.Point(2, 16);
            this.dgvCheckResultData.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvCheckResultData.Name = "dgvCheckResultData";
            this.dgvCheckResultData.ReadOnly = true;
            this.dgvCheckResultData.RowHeadersVisible = false;
            this.dgvCheckResultData.RowTemplate.Height = 27;
            this.dgvCheckResultData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCheckResultData.Size = new System.Drawing.Size(383, 534);
            this.dgvCheckResultData.TabIndex = 1;
            // 
            // dgvCheckItem
            // 
            this.dgvCheckItem.AllowUserToAddRows = false;
            this.dgvCheckItem.AllowUserToDeleteRows = false;
            this.dgvCheckItem.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvCheckItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCheckItem.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCheckItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCheckItem.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvCheckItem.Location = new System.Drawing.Point(2, 16);
            this.dgvCheckItem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvCheckItem.Name = "dgvCheckItem";
            this.dgvCheckItem.ReadOnly = true;
            this.dgvCheckItem.RowHeadersVisible = false;
            this.dgvCheckItem.RowTemplate.Height = 27;
            this.dgvCheckItem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCheckItem.Size = new System.Drawing.Size(442, 534);
            this.dgvCheckItem.TabIndex = 2;
            this.dgvCheckItem.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCheckItem_CellClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(836, 552);
            this.splitContainer1.SplitterDistance = 446;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.dgvCheckItem);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(446, 552);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "逻辑检查信息列表";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.dgvCheckResultData);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(387, 552);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "详细列表";
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(729, 567);
            this.btnExit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(105, 30);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnExportDetialTable
            // 
            this.btnExportDetialTable.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportDetialTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportDetialTable.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportDetialTable.Location = new System.Drawing.Point(513, 567);
            this.btnExportDetialTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExportDetialTable.Name = "btnExportDetialTable";
            this.btnExportDetialTable.Size = new System.Drawing.Size(103, 30);
            this.btnExportDetialTable.TabIndex = 5;
            this.btnExportDetialTable.Text = "导出详细列表";
            this.btnExportDetialTable.Click += new System.EventHandler(this.btnExportDetialTable_Click);
            // 
            // btnExportDataTable
            // 
            this.btnExportDataTable.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportDataTable.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportDataTable.Location = new System.Drawing.Point(620, 567);
            this.btnExportDataTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExportDataTable.Name = "btnExportDataTable";
            this.btnExportDataTable.Size = new System.Drawing.Size(105, 30);
            this.btnExportDataTable.TabIndex = 6;
            this.btnExportDataTable.Text = "导出信息列表";
            this.btnExportDataTable.Click += new System.EventHandler(this.btnExportDataTable_Click);
            // 
            // FrmLogicCheckResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 608);
            this.Controls.Add(this.btnExportDataTable);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnExportDetialTable);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FrmLogicCheckResult";
            this.ShowIcon = false;
            this.Text = "逻辑检查结果";
            this.Load += new System.EventHandler(this.FrmLogicCheckResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckResultData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckItem)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvCheckResultData;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvCheckItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnExportDetialTable;
        private DevComponents.DotNetBar.ButtonX btnExportDataTable;
    }
}