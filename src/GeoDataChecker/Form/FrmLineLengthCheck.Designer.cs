namespace GeoDataChecker
{
    partial class FrmLineLengthCheck
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
            this.txtMin = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtMax = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // txtMin
            // 
            // 
            // 
            // 
            this.txtMin.Border.Class = "TextBoxBorder";
            this.txtMin.Location = new System.Drawing.Point(99, 41);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(201, 21);
            this.txtMin.TabIndex = 54;
            // 
            // txtMax
            // 
            // 
            // 
            // 
            this.txtMax.Border.Class = "TextBoxBorder";
            this.txtMax.Location = new System.Drawing.Point(99, 10);
            this.txtMax.Name = "txtMax";
            this.txtMax.Size = new System.Drawing.Size(201, 21);
            this.txtMax.TabIndex = 53;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(86, 23);
            this.labelX1.TabIndex = 51;
            this.labelX1.Text = "最   大   值:";
            // 
            // labelX8
            // 
            this.labelX8.Location = new System.Drawing.Point(12, 41);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(95, 23);
            this.labelX8.TabIndex = 52;
            this.labelX8.Text = "最   小   值:";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(247, 74);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(56, 23);
            this.buttonX1.TabIndex = 58;
            this.buttonX1.Text = "取  消";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(178, 74);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(52, 23);
            this.btnOK.TabIndex = 57;
            this.btnOK.Text = "确  定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FrmLineLengthCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 101);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtMin);
            this.Controls.Add(this.txtMax);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.labelX8);
            this.MaximizeBox = false;
            this.Name = "FrmLineLengthCheck";
            this.ShowIcon = false;
            this.Text = "最大最小值检查设置";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtMin;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMax;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX btnOK;
    }
}