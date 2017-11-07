namespace FileDBTool
{
    partial class frmConSet
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.IP_tex = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ID_tex = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.Password_tex = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnDBSel = new DevComponents.DotNetBar.ButtonX();
            this.txtDB = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.Cannel_btn = new DevComponents.DotNetBar.ButtonX();
            this.Ok_btn = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(3, 16);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(87, 26);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "FTP服务器IP:";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(3, 48);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(87, 26);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "用户登录 ID:";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(3, 80);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(87, 26);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "用户登录密码:";
            // 
            // IP_tex
            // 
            // 
            // 
            // 
            this.IP_tex.Border.Class = "TextBoxBorder";
            this.IP_tex.Location = new System.Drawing.Point(96, 16);
            this.IP_tex.Name = "IP_tex";
            this.IP_tex.Size = new System.Drawing.Size(241, 21);
            this.IP_tex.TabIndex = 3;
            // 
            // ID_tex
            // 
            // 
            // 
            // 
            this.ID_tex.Border.Class = "TextBoxBorder";
            this.ID_tex.Location = new System.Drawing.Point(96, 48);
            this.ID_tex.Name = "ID_tex";
            this.ID_tex.Size = new System.Drawing.Size(241, 21);
            this.ID_tex.TabIndex = 4;
            // 
            // Password_tex
            // 
            // 
            // 
            // 
            this.Password_tex.Border.Class = "TextBoxBorder";
            this.Password_tex.Location = new System.Drawing.Point(96, 80);
            this.Password_tex.Name = "Password_tex";
            this.Password_tex.Size = new System.Drawing.Size(241, 21);
            this.Password_tex.TabIndex = 5;
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btnDBSel);
            this.groupPanel1.Controls.Add(this.txtDB);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Controls.Add(this.Password_tex);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.ID_tex);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.IP_tex);
            this.groupPanel1.Location = new System.Drawing.Point(12, 12);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(358, 178);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 6;
            this.groupPanel1.Text = "FTP连接设置：";
            // 
            // btnDBSel
            // 
            this.btnDBSel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDBSel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDBSel.Location = new System.Drawing.Point(308, 112);
            this.btnDBSel.Name = "btnDBSel";
            this.btnDBSel.Size = new System.Drawing.Size(31, 23);
            this.btnDBSel.TabIndex = 8;
            this.btnDBSel.Text = "...";
            this.btnDBSel.Click += new System.EventHandler(this.btnDBSel_Click);
            // 
            // txtDB
            // 
            // 
            // 
            // 
            this.txtDB.Border.Class = "TextBoxBorder";
            this.txtDB.Location = new System.Drawing.Point(96, 112);
            this.txtDB.Name = "txtDB";
            this.txtDB.Size = new System.Drawing.Size(212, 21);
            this.txtDB.TabIndex = 7;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(3, 112);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(87, 23);
            this.labelX4.TabIndex = 6;
            this.labelX4.Text = "元 数 据 库:";
            // 
            // Cannel_btn
            // 
            this.Cannel_btn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Cannel_btn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Cannel_btn.Location = new System.Drawing.Point(300, 196);
            this.Cannel_btn.Name = "Cannel_btn";
            this.Cannel_btn.Size = new System.Drawing.Size(69, 29);
            this.Cannel_btn.TabIndex = 7;
            this.Cannel_btn.Text = "取  消";
            this.Cannel_btn.Click += new System.EventHandler(this.Cannel_btn_Click);
            // 
            // Ok_btn
            // 
            this.Ok_btn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Ok_btn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Ok_btn.Location = new System.Drawing.Point(221, 196);
            this.Ok_btn.Name = "Ok_btn";
            this.Ok_btn.Size = new System.Drawing.Size(73, 29);
            this.Ok_btn.TabIndex = 8;
            this.Ok_btn.Text = "确  定";
            this.Ok_btn.Click += new System.EventHandler(this.Ok_btn_Click);
            // 
            // frmConSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 234);
            this.Controls.Add(this.Ok_btn);
            this.Controls.Add(this.Cannel_btn);
            this.Controls.Add(this.groupPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConSet";
            this.ShowIcon = false;
            this.Text = "增加FTP连接";
            this.Load += new System.EventHandler(this.frmConSet_Load);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX IP_tex;
        private DevComponents.DotNetBar.Controls.TextBoxX ID_tex;
        private DevComponents.DotNetBar.Controls.TextBoxX Password_tex;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX Cannel_btn;
        private DevComponents.DotNetBar.ButtonX Ok_btn;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.ButtonX btnDBSel;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDB;
    }
}