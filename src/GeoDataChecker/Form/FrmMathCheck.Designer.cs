namespace GeoDataChecker
{
    partial class FrmMathCheck
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
            this.btnPrj = new DevComponents.DotNetBar.ButtonX();
            this.txtPrj = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // btnPrj
            // 
            this.btnPrj.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrj.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrj.Location = new System.Drawing.Point(286, 12);
            this.btnPrj.Name = "btnPrj";
            this.btnPrj.Size = new System.Drawing.Size(32, 21);
            this.btnPrj.TabIndex = 54;
            this.btnPrj.Text = "...";
            this.btnPrj.Click += new System.EventHandler(this.btnPrj_Click);
            // 
            // txtPrj
            // 
            // 
            // 
            // 
            this.txtPrj.Border.Class = "TextBoxBorder";
            this.txtPrj.Location = new System.Drawing.Point(92, 12);
            this.txtPrj.Name = "txtPrj";
            this.txtPrj.Size = new System.Drawing.Size(188, 21);
            this.txtPrj.TabIndex = 53;
            // 
            // labelX10
            // 
            this.labelX10.Location = new System.Drawing.Point(1, 12);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(85, 23);
            this.labelX10.TabIndex = 52;
            this.labelX10.Text = "空间参考文件:";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(261, 39);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(56, 23);
            this.buttonX1.TabIndex = 56;
            this.buttonX1.Text = "取  消";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(192, 39);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(52, 23);
            this.btnOK.TabIndex = 55;
            this.btnOK.Text = "确  定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FrmMathCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 68);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnPrj);
            this.Controls.Add(this.txtPrj);
            this.Controls.Add(this.labelX10);
            this.MaximizeBox = false;
            this.Name = "FrmMathCheck";
            this.ShowIcon = false;
            this.Text = "标准空间参考设置";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnPrj;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPrj;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX btnOK;
    }
}