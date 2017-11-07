namespace GeoStatistics
{
    partial class frmStatisticsPic
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStatisticsPic));
            this.axMSChart1 = new AxMSChart20Lib.AxMSChart();
            this.btnExportPic = new DevComponents.DotNetBar.ButtonX();
            this.comboType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.axMSChart1)).BeginInit();
            this.SuspendLayout();
            // 
            // axMSChart1
            // 
            this.axMSChart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.axMSChart1.DataSource = null;
            this.axMSChart1.Location = new System.Drawing.Point(12, 21);
            this.axMSChart1.Name = "axMSChart1";
            this.axMSChart1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMSChart1.OcxState")));
            this.axMSChart1.Size = new System.Drawing.Size(482, 273);
            this.axMSChart1.TabIndex = 0;
            // 
            // btnExportPic
            // 
            this.btnExportPic.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportPic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportPic.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportPic.Location = new System.Drawing.Point(373, 300);
            this.btnExportPic.Name = "btnExportPic";
            this.btnExportPic.Size = new System.Drawing.Size(121, 24);
            this.btnExportPic.TabIndex = 1;
            this.btnExportPic.Text = "复制到剪贴板";
            this.btnExportPic.Click += new System.EventHandler(this.btnExportPic_Click);
            // 
            // comboType
            // 
            this.comboType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboType.DisplayMember = "Text";
            this.comboType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.FormattingEnabled = true;
            this.comboType.ItemHeight = 15;
            this.comboType.Location = new System.Drawing.Point(13, 300);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(167, 21);
            this.comboType.TabIndex = 5;
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.comboType_SelectedIndexChanged);
            // 
            // frmStatisticsPic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 332);
            this.Controls.Add(this.comboType);
            this.Controls.Add(this.btnExportPic);
            this.Controls.Add(this.axMSChart1);
            this.Name = "frmStatisticsPic";
            this.ShowIcon = false;
            this.Text = "统计图";
            this.Load += new System.EventHandler(this.frmStatisticsPic_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMSChart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxMSChart20Lib.AxMSChart axMSChart1;
        private DevComponents.DotNetBar.ButtonX btnExportPic;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboType;

    }
}