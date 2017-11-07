namespace GeoDataChecker
{
    partial class SetJoinCheck
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
            this.ribbonClientPanel1 = new DevComponents.DotNetBar.Ribbon.RibbonClientPanel();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btn_cancel = new DevComponents.DotNetBar.ButtonX();
            this.btn_enter = new DevComponents.DotNetBar.ButtonX();
            this.btn_check = new DevComponents.DotNetBar.ButtonX();
            this.txt_path = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.ribbonClientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbonClientPanel1
            // 
            this.ribbonClientPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.ribbonClientPanel1.Controls.Add(this.labelX2);
            this.ribbonClientPanel1.Controls.Add(this.btn_cancel);
            this.ribbonClientPanel1.Controls.Add(this.btn_enter);
            this.ribbonClientPanel1.Controls.Add(this.btn_check);
            this.ribbonClientPanel1.Controls.Add(this.txt_path);
            this.ribbonClientPanel1.Controls.Add(this.labelX1);
            this.ribbonClientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ribbonClientPanel1.Location = new System.Drawing.Point(4, 1);
            this.ribbonClientPanel1.Name = "ribbonClientPanel1";
            this.ribbonClientPanel1.Size = new System.Drawing.Size(351, 167);
            // 
            // 
            // 
            this.ribbonClientPanel1.Style.Class = "RibbonClientPanel";
            this.ribbonClientPanel1.TabIndex = 0;
            this.ribbonClientPanel1.Text = "ribbonClientPanel1";
            this.ribbonClientPanel1.Click += new System.EventHandler(this.ribbonClientPanel1_Click);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX2.Location = new System.Drawing.Point(27, 108);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(226, 23);
            this.labelX2.TabIndex = 6;
            this.labelX2.Text = "说明：请选择已预处理的数据集合！";
            // 
            // btn_cancel
            // 
            this.btn_cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_cancel.Location = new System.Drawing.Point(178, 67);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 4;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_enter
            // 
            this.btn_enter.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_enter.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_enter.Location = new System.Drawing.Point(89, 67);
            this.btn_enter.Name = "btn_enter";
            this.btn_enter.Size = new System.Drawing.Size(75, 23);
            this.btn_enter.TabIndex = 3;
            this.btn_enter.Text = "确定";
            this.btn_enter.Click += new System.EventHandler(this.btn_enter_Click);
            // 
            // btn_check
            // 
            this.btn_check.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_check.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_check.Location = new System.Drawing.Point(260, 22);
            this.btn_check.Name = "btn_check";
            this.btn_check.Size = new System.Drawing.Size(75, 23);
            this.btn_check.TabIndex = 1;
            this.btn_check.Text = "浏览...";
            this.btn_check.Click += new System.EventHandler(this.btn_check_Click);
            // 
            // txt_path
            // 
            this.txt_path.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txt_path.Border.Class = "TextBoxBorder";
            this.txt_path.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txt_path.Location = new System.Drawing.Point(88, 23);
            this.txt_path.Name = "txt_path";
            this.txt_path.ReadOnly = true;
            this.txt_path.Size = new System.Drawing.Size(165, 21);
            this.txt_path.TabIndex = 2;
            this.txt_path.Text = "本地数据集合";
            this.txt_path.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(20, 25);
            this.labelX1.Name = "labelX1";
            this.labelX1.SingleLineColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelX1.Size = new System.Drawing.Size(83, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "本地数据库";
            // 
            // SetJoinCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 170);
            this.Controls.Add(this.ribbonClientPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SetJoinCheck";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置检查数据";
            this.ribbonClientPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Ribbon.RibbonClientPanel ribbonClientPanel1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btn_enter;
        private DevComponents.DotNetBar.ButtonX btn_check;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_path;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btn_cancel;
    }
}