namespace GeoDataChecker
{
    partial class frmLineContourCheckSet
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
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbOrient = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.txtLog = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(347, 63);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(42, 23);
            this.buttonX1.TabIndex = 0;
            this.buttonX1.Text = "浏览";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 26);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(97, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "高  程 方 向:";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(12, 65);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(97, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "错误日志路径:";
            // 
            // cmbOrient
            // 
            this.cmbOrient.DisplayMember = "Text";
            this.cmbOrient.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbOrient.FormattingEnabled = true;
            this.cmbOrient.ItemHeight = 15;
            this.cmbOrient.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2});
            this.cmbOrient.Location = new System.Drawing.Point(115, 28);
            this.cmbOrient.Name = "cmbOrient";
            this.cmbOrient.Size = new System.Drawing.Size(274, 21);
            this.cmbOrient.TabIndex = 3;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "递增";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "递减";
            // 
            // txtLog
            // 
            // 
            // 
            // 
            this.txtLog.Border.Class = "TextBoxBorder";
            this.txtLog.Location = new System.Drawing.Point(115, 65);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(230, 21);
            this.txtLog.TabIndex = 4;
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(332, 108);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(57, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "确 定";
            this.btnOk.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // frmLineContourCheckSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 143);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.cmbOrient);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.buttonX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLineContourCheckSet";
            this.ShowIcon = false;
            this.Text = "等高线高程检查参数设置";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbOrient;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLog;
        private DevComponents.DotNetBar.ButtonX btnOk;
    }
}