namespace GeoDBATool
{
    partial class FrmTopoCheckResult
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
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.btnExportDetialTable = new DevComponents.DotNetBar.ButtonX();
            this.btnExportDataTable = new DevComponents.DotNetBar.ButtonX();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DataGridErrs = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnExport = new DevComponents.DotNetBar.ButtonX();
            this.progressStep = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.lblTips = new DevComponents.DotNetBar.LabelX();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridErrs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(500, 347);
            this.btnExit.Margin = new System.Windows.Forms.Padding(2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(76, 30);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "退出";
            // 
            // btnExportDetialTable
            // 
            this.btnExportDetialTable.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportDetialTable.Location = new System.Drawing.Point(0, 0);
            this.btnExportDetialTable.Name = "btnExportDetialTable";
            this.btnExportDetialTable.Size = new System.Drawing.Size(0, 0);
            this.btnExportDetialTable.TabIndex = 9;
            // 
            // btnExportDataTable
            // 
            this.btnExportDataTable.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportDataTable.Location = new System.Drawing.Point(0, 0);
            this.btnExportDataTable.Name = "btnExportDataTable";
            this.btnExportDataTable.Size = new System.Drawing.Size(0, 0);
            this.btnExportDataTable.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.DataGridErrs);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(580, 313);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "拓扑检查信息列表";
            // 
            // DataGridErrs
            // 
            this.DataGridErrs.AllowUserToAddRows = false;
            this.DataGridErrs.AllowUserToDeleteRows = false;
            this.DataGridErrs.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DataGridErrs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridErrs.DefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridErrs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridErrs.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.DataGridErrs.Location = new System.Drawing.Point(2, 16);
            this.DataGridErrs.Margin = new System.Windows.Forms.Padding(2);
            this.DataGridErrs.Name = "DataGridErrs";
            this.DataGridErrs.ReadOnly = true;
            this.DataGridErrs.RowHeadersVisible = false;
            this.DataGridErrs.RowTemplate.Height = 27;
            this.DataGridErrs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridErrs.Size = new System.Drawing.Size(576, 295);
            this.DataGridErrs.TabIndex = 2;
            this.DataGridErrs.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridErrs_CellDoubleClick);
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(484, 324);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(95, 21);
            this.btnExport.TabIndex = 10;
            this.btnExport.Text = "导出检查结果";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // progressStep
            // 
            this.progressStep.BackColor = System.Drawing.Color.White;
            this.progressStep.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.progressStep.Location = new System.Drawing.Point(-1, 338);
            this.progressStep.Name = "progressStep";
            this.progressStep.Size = new System.Drawing.Size(484, 10);
            this.progressStep.TabIndex = 12;
            this.progressStep.Visible = false;
            // 
            // lblTips
            // 
            this.lblTips.Location = new System.Drawing.Point(0, 323);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(478, 14);
            this.lblTips.TabIndex = 11;
            // 
            // FrmTopoCheckResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 347);
            this.Controls.Add(this.progressStep);
            this.Controls.Add(this.lblTips);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExportDataTable);
            this.Controls.Add(this.btnExportDetialTable);
            this.Controls.Add(this.btnExit);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmTopoCheckResult";
            this.ShowIcon = false;
            this.Text = "拓扑检查结果";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridErrs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnExportDetialTable;
        private DevComponents.DotNetBar.ButtonX btnExportDataTable;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.Controls.DataGridViewX DataGridErrs;
        private DevComponents.DotNetBar.ButtonX btnExport;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressStep;
        private DevComponents.DotNetBar.LabelX lblTips;
    }
}