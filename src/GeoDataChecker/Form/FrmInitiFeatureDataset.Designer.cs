namespace GeoDataChecker
{
    partial class FrmInitiFeatureDataset
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
            this.btn_cancel = new DevComponents.DotNetBar.ButtonX();
            this.btn_enter = new DevComponents.DotNetBar.ButtonX();
            this.btn_prj = new DevComponents.DotNetBar.ButtonX();
            this.txt_prj = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btn_org = new DevComponents.DotNetBar.ButtonX();
            this.txt_org = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lab_org = new DevComponents.DotNetBar.LabelX();
            this.pic_processbar = new System.Windows.Forms.PictureBox();
            this.lab_show = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.pic_processbar)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_cancel
            // 
            this.btn_cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(227, 129);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 23;
            this.btn_cancel.Text = "取消";
            // 
            // btn_enter
            // 
            this.btn_enter.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_enter.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_enter.Location = new System.Drawing.Point(120, 129);
            this.btn_enter.Name = "btn_enter";
            this.btn_enter.Size = new System.Drawing.Size(75, 23);
            this.btn_enter.TabIndex = 22;
            this.btn_enter.Text = "开始处理";
            this.btn_enter.Click += new System.EventHandler(this.btn_enter_Click);
            // 
            // btn_prj
            // 
            this.btn_prj.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_prj.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_prj.Location = new System.Drawing.Point(313, 76);
            this.btn_prj.Name = "btn_prj";
            this.btn_prj.Size = new System.Drawing.Size(75, 23);
            this.btn_prj.TabIndex = 21;
            this.btn_prj.Text = "浏览...";
            this.btn_prj.Click += new System.EventHandler(this.btn_prj_Click);
            // 
            // txt_prj
            // 
            this.txt_prj.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txt_prj.Border.Class = "TextBoxBorder";
            this.txt_prj.Enabled = false;
            this.txt_prj.FocusHighlightColor = System.Drawing.Color.White;
            this.txt_prj.Location = new System.Drawing.Point(115, 77);
            this.txt_prj.Name = "txt_prj";
            this.txt_prj.ReadOnly = true;
            this.txt_prj.Size = new System.Drawing.Size(190, 21);
            this.txt_prj.TabIndex = 20;
            this.txt_prj.WatermarkColor = System.Drawing.SystemColors.Window;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(44, 78);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 23);
            this.labelX1.TabIndex = 19;
            this.labelX1.Text = "PRJ文件：";
            // 
            // btn_org
            // 
            this.btn_org.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_org.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_org.Location = new System.Drawing.Point(312, 32);
            this.btn_org.Name = "btn_org";
            this.btn_org.Size = new System.Drawing.Size(75, 23);
            this.btn_org.TabIndex = 18;
            this.btn_org.Text = "浏览...";
            this.btn_org.Click += new System.EventHandler(this.btn_org_Click);
            // 
            // txt_org
            // 
            this.txt_org.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txt_org.Border.Class = "TextBoxBorder";
            this.txt_org.Enabled = false;
            this.txt_org.FocusHighlightColor = System.Drawing.Color.White;
            this.txt_org.Location = new System.Drawing.Point(114, 33);
            this.txt_org.Name = "txt_org";
            this.txt_org.ReadOnly = true;
            this.txt_org.Size = new System.Drawing.Size(190, 21);
            this.txt_org.TabIndex = 17;
            // 
            // lab_org
            // 
            this.lab_org.Location = new System.Drawing.Point(28, 34);
            this.lab_org.Name = "lab_org";
            this.lab_org.Size = new System.Drawing.Size(83, 23);
            this.lab_org.TabIndex = 16;
            this.lab_org.Text = "源数据路径：";
            // 
            // pic_processbar
            // 
            this.pic_processbar.Image = global::GeoDataChecker.Properties.Resources._2;
            this.pic_processbar.Location = new System.Drawing.Point(230, 35);
            this.pic_processbar.Name = "pic_processbar";
            this.pic_processbar.Size = new System.Drawing.Size(104, 94);
            this.pic_processbar.TabIndex = 24;
            this.pic_processbar.TabStop = false;
            this.pic_processbar.Visible = false;
            // 
            // lab_show
            // 
            this.lab_show.Location = new System.Drawing.Point(68, 73);
            this.lab_show.Name = "lab_show";
            this.lab_show.Size = new System.Drawing.Size(156, 23);
            this.lab_show.TabIndex = 25;
            this.lab_show.Text = "正在处理数据，请稍后....";
            this.lab_show.Visible = false;
            // 
            // FrmInitiFeatureDataset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 200);
            this.Controls.Add(this.lab_show);
            this.Controls.Add(this.pic_processbar);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_enter);
            this.Controls.Add(this.btn_prj);
            this.Controls.Add(this.txt_prj);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btn_org);
            this.Controls.Add(this.txt_org);
            this.Controls.Add(this.lab_org);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmInitiFeatureDataset";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置数据路径";
            this.Load += new System.EventHandler(this.FrmInitiFeatureDataset_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_processbar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btn_cancel;
        public DevComponents.DotNetBar.ButtonX btn_enter;
        private DevComponents.DotNetBar.ButtonX btn_prj;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_prj;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btn_org;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_org;
        private DevComponents.DotNetBar.LabelX lab_org;
        private System.Windows.Forms.PictureBox pic_processbar;
        private DevComponents.DotNetBar.LabelX lab_show;



    }
}