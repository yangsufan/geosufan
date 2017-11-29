namespace SysCommon
{
    partial class FrmResult
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtPageCount = new DevExpress.XtraEditors.TextEdit();
            this.btnNextPage = new DevExpress.XtraEditors.SimpleButton();
            this.btnLastPage = new DevExpress.XtraEditors.SimpleButton();
            this.btnExportExecl = new DevExpress.XtraEditors.SimpleButton();
            this.btnExportChart = new DevExpress.XtraEditors.SimpleButton();
            this.cbStyle = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPageCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbStyle.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtPageCount);
            this.splitContainer1.Panel2.Controls.Add(this.btnNextPage);
            this.splitContainer1.Panel2.Controls.Add(this.btnLastPage);
            this.splitContainer1.Panel2.Controls.Add(this.btnExportExecl);
            this.splitContainer1.Panel2.Controls.Add(this.btnExportChart);
            this.splitContainer1.Panel2.Controls.Add(this.cbStyle);
            this.splitContainer1.Panel2.Controls.Add(this.labelX1);
            this.splitContainer1.Size = new System.Drawing.Size(813, 705);
            this.splitContainer1.SplitterDistance = 608;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // txtPageCount
            // 
            this.txtPageCount.Location = new System.Drawing.Point(121, 29);
            this.txtPageCount.Name = "txtPageCount";
            this.txtPageCount.Size = new System.Drawing.Size(63, 20);
            this.txtPageCount.TabIndex = 6;
            // 
            // btnNextPage
            // 
            this.btnNextPage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNextPage.Location = new System.Drawing.Point(204, 29);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(87, 27);
            this.btnNextPage.TabIndex = 5;
            this.btnNextPage.Text = "下页";
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnLastPage
            // 
            this.btnLastPage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLastPage.Location = new System.Drawing.Point(14, 29);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(87, 27);
            this.btnLastPage.TabIndex = 4;
            this.btnLastPage.Text = "上页";
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnExportExecl
            // 
            this.btnExportExecl.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportExecl.Location = new System.Drawing.Point(693, 29);
            this.btnExportExecl.Name = "btnExportExecl";
            this.btnExportExecl.Size = new System.Drawing.Size(87, 27);
            this.btnExportExecl.TabIndex = 3;
            this.btnExportExecl.Text = "导出统计表";
            this.btnExportExecl.Click += new System.EventHandler(this.btnExportExecl_Click);
            // 
            // btnExportChart
            // 
            this.btnExportChart.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportChart.Location = new System.Drawing.Point(579, 29);
            this.btnExportChart.Name = "btnExportChart";
            this.btnExportChart.Size = new System.Drawing.Size(107, 27);
            this.btnExportChart.TabIndex = 2;
            this.btnExportChart.Text = "导出统计图";
            this.btnExportChart.Click += new System.EventHandler(this.btnExportChart_Click);
            // 
            // cbStyle
            // 
            this.cbStyle.Location = new System.Drawing.Point(393, 31);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new System.Drawing.Size(178, 20);
            this.cbStyle.TabIndex = 1;
            this.cbStyle.SelectedIndexChanged += new System.EventHandler(this.cbStyle_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(299, 31);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 14);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "外观样式：";
            // 
            // FrmResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 705);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmResult";
            this.Text = "查询结果统计";
            this.Load += new System.EventHandler(this.FrmResult_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPageCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbStyle.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraEditors.SimpleButton btnExportExecl;
        private DevExpress.XtraEditors.SimpleButton btnExportChart;
        private DevExpress.XtraEditors.ComboBoxEdit cbStyle;
        private DevExpress.XtraEditors.LabelControl labelX1;
        private DevExpress.XtraEditors.TextEdit txtPageCount;
        private DevExpress.XtraEditors.SimpleButton btnNextPage;
        private DevExpress.XtraEditors.SimpleButton btnLastPage;
      
    }
}