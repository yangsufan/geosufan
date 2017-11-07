namespace GeoDBIntegration
{
    partial class frmCreateDB
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
            this.superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtservername = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.comBoxType = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtVersion = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
            this.txtDataBase = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.txtPassWord = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtUser = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtServer = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            this.labelX16 = new DevComponents.DotNetBar.LabelX();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelXErr = new DevComponents.DotNetBar.LabelX();
            this.btnRuleFile = new DevComponents.DotNetBar.ButtonX();
            this.textRuleFilePath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnProj = new DevComponents.DotNetBar.ButtonX();
            this.txtProjFilePath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btn_cancle = new DevComponents.DotNetBar.ButtonX();
            this.btn_OK = new DevComponents.DotNetBar.ButtonX();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.txtservername);
            this.groupPanel3.Controls.Add(this.labelX5);
            this.groupPanel3.Controls.Add(this.comBoxType);
            this.groupPanel3.Controls.Add(this.txtVersion);
            this.groupPanel3.Controls.Add(this.labelX3);
            this.groupPanel3.Controls.Add(this.labelX4);
            this.groupPanel3.Controls.Add(this.btnServer);
            this.groupPanel3.Controls.Add(this.txtDataBase);
            this.groupPanel3.Controls.Add(this.labelX13);
            this.groupPanel3.Controls.Add(this.txtPassWord);
            this.groupPanel3.Controls.Add(this.txtUser);
            this.groupPanel3.Controls.Add(this.txtServer);
            this.groupPanel3.Controls.Add(this.labelX15);
            this.groupPanel3.Controls.Add(this.labelX16);
            this.groupPanel3.Controls.Add(this.labelX18);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(9, 12);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(305, 218);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel3.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel3.TabIndex = 28;
            this.groupPanel3.Text = "数据库连接设置";
            // 
            // txtservername
            // 
            // 
            // 
            // 
            this.txtservername.Border.Class = "TextBoxBorder";
            this.txtservername.Location = new System.Drawing.Point(56, 32);
            this.txtservername.Name = "txtservername";
            this.txtservername.Size = new System.Drawing.Size(240, 21);
            this.txtservername.TabIndex = 48;
            this.txtservername.WatermarkText = "数据库实例名称";
            this.txtservername.WordWrap = false;
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            this.labelX5.Location = new System.Drawing.Point(3, 33);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(56, 18);
            this.labelX5.TabIndex = 47;
            this.labelX5.Text = "服务名 :";
            // 
            // comBoxType
            // 
            // 
            // 
            // 
            this.comBoxType.Border.Class = "TextBoxBorder";
            this.comBoxType.Location = new System.Drawing.Point(56, 6);
            this.comBoxType.Name = "comBoxType";
            this.comBoxType.Size = new System.Drawing.Size(240, 21);
            this.comBoxType.TabIndex = 46;
            // 
            // txtVersion
            // 
            // 
            // 
            // 
            this.txtVersion.Border.Class = "TextBoxBorder";
            this.txtVersion.Location = new System.Drawing.Point(56, 162);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(240, 21);
            this.txtVersion.TabIndex = 45;
            this.txtVersion.WatermarkText = "版本信息";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.Location = new System.Drawing.Point(3, 168);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(56, 18);
            this.labelX3.TabIndex = 44;
            this.labelX3.Text = "版  本 :";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.Location = new System.Drawing.Point(3, 6);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(56, 18);
            this.labelX4.TabIndex = 43;
            this.labelX4.Text = "类  型 :";
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnServer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnServer.Location = new System.Drawing.Point(270, 84);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(26, 21);
            this.btnServer.TabIndex = 20;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // txtDataBase
            // 
            // 
            // 
            // 
            this.txtDataBase.Border.Class = "TextBoxBorder";
            this.txtDataBase.Location = new System.Drawing.Point(56, 84);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Size = new System.Drawing.Size(212, 21);
            this.txtDataBase.TabIndex = 3;
            this.txtDataBase.WatermarkText = "数据库名或本地库路径";
            // 
            // labelX13
            // 
            this.labelX13.AutoSize = true;
            this.labelX13.Location = new System.Drawing.Point(3, 87);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(56, 18);
            this.labelX13.TabIndex = 15;
            this.labelX13.Text = "数据库 :";
            // 
            // txtPassWord
            // 
            // 
            // 
            // 
            this.txtPassWord.Border.Class = "TextBoxBorder";
            this.txtPassWord.Location = new System.Drawing.Point(56, 136);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(240, 21);
            this.txtPassWord.TabIndex = 5;
            this.txtPassWord.WatermarkText = "访问密码";
            // 
            // txtUser
            // 
            // 
            // 
            // 
            this.txtUser.Border.Class = "TextBoxBorder";
            this.txtUser.Location = new System.Drawing.Point(56, 110);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(240, 21);
            this.txtUser.TabIndex = 4;
            this.txtUser.WatermarkText = "访问用户名";
            // 
            // txtServer
            // 
            // 
            // 
            // 
            this.txtServer.Border.Class = "TextBoxBorder";
            this.txtServer.Location = new System.Drawing.Point(56, 58);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(240, 21);
            this.txtServer.TabIndex = 2;
            this.txtServer.WatermarkText = "服务器ip地址或服务名";
            this.txtServer.WordWrap = false;
            // 
            // labelX15
            // 
            this.labelX15.AutoSize = true;
            this.labelX15.Location = new System.Drawing.Point(3, 141);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(56, 18);
            this.labelX15.TabIndex = 4;
            this.labelX15.Text = "密  码 :";
            // 
            // labelX16
            // 
            this.labelX16.AutoSize = true;
            this.labelX16.Location = new System.Drawing.Point(3, 114);
            this.labelX16.Name = "labelX16";
            this.labelX16.Size = new System.Drawing.Size(56, 18);
            this.labelX16.TabIndex = 3;
            this.labelX16.Text = "用  户 :";
            // 
            // labelX18
            // 
            this.labelX18.AutoSize = true;
            this.labelX18.Location = new System.Drawing.Point(3, 60);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(56, 18);
            this.labelX18.TabIndex = 1;
            this.labelX18.Text = "服务器 :";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(9, 265);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(62, 18);
            this.labelX2.TabIndex = 51;
            this.labelX2.Text = "配置文件:";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(9, 239);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(62, 18);
            this.labelX1.TabIndex = 50;
            this.labelX1.Text = "空间参考:";
            // 
            // labelXErr
            // 
            this.labelXErr.Location = new System.Drawing.Point(9, 289);
            this.labelXErr.Name = "labelXErr";
            this.labelXErr.Size = new System.Drawing.Size(272, 19);
            this.labelXErr.TabIndex = 49;
            // 
            // btnRuleFile
            // 
            this.btnRuleFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRuleFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRuleFile.Location = new System.Drawing.Point(282, 263);
            this.btnRuleFile.Name = "btnRuleFile";
            this.btnRuleFile.Size = new System.Drawing.Size(26, 21);
            this.btnRuleFile.TabIndex = 48;
            this.btnRuleFile.Text = "...";
            this.btnRuleFile.Click += new System.EventHandler(this.btnRuleFile_Click);
            // 
            // textRuleFilePath
            // 
            // 
            // 
            // 
            this.textRuleFilePath.Border.Class = "TextBoxBorder";
            this.textRuleFilePath.Location = new System.Drawing.Point(74, 263);
            this.textRuleFilePath.Name = "textRuleFilePath";
            this.textRuleFilePath.Size = new System.Drawing.Size(206, 21);
            this.textRuleFilePath.TabIndex = 46;
            this.textRuleFilePath.WatermarkText = "库体配置文件路径";
            // 
            // btnProj
            // 
            this.btnProj.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnProj.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnProj.Location = new System.Drawing.Point(282, 235);
            this.btnProj.Name = "btnProj";
            this.btnProj.Size = new System.Drawing.Size(26, 21);
            this.btnProj.TabIndex = 47;
            this.btnProj.Text = "...";
            this.btnProj.Click += new System.EventHandler(this.btnProj_Click);
            // 
            // txtProjFilePath
            // 
            // 
            // 
            // 
            this.txtProjFilePath.Border.Class = "TextBoxBorder";
            this.txtProjFilePath.Location = new System.Drawing.Point(74, 236);
            this.txtProjFilePath.Name = "txtProjFilePath";
            this.txtProjFilePath.Size = new System.Drawing.Size(207, 21);
            this.txtProjFilePath.TabIndex = 45;
            this.txtProjFilePath.WatermarkText = "空间参考投影文件路径";
            // 
            // btn_cancle
            // 
            this.btn_cancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_cancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_cancle.Location = new System.Drawing.Point(244, 314);
            this.btn_cancle.Name = "btn_cancle";
            this.btn_cancle.Size = new System.Drawing.Size(70, 23);
            this.btn_cancle.TabIndex = 52;
            this.btn_cancle.Text = "取消";
            this.btn_cancle.Click += new System.EventHandler(this.btn_cancle_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_OK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_OK.Location = new System.Drawing.Point(163, 314);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(70, 23);
            this.btn_OK.TabIndex = 53;
            this.btn_OK.Text = " 确定";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(9, 296);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(305, 10);
            this.progressBar1.TabIndex = 54;
            this.progressBar1.Visible = false;
            // 
            // frmCreateDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 344);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_cancle);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.labelXErr);
            this.Controls.Add(this.btnRuleFile);
            this.Controls.Add(this.textRuleFilePath);
            this.Controls.Add(this.btnProj);
            this.Controls.Add(this.txtProjFilePath);
            this.Controls.Add(this.groupPanel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCreateDB";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "初始化库体：";
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.SuperTooltip superTooltip;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.LabelX labelX4;
        public DevComponents.DotNetBar.ButtonX btnServer;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDataBase;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassWord;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUser;
        private DevComponents.DotNetBar.Controls.TextBoxX txtServer;
        private DevComponents.DotNetBar.LabelX labelX15;
        private DevComponents.DotNetBar.LabelX labelX16;
        private DevComponents.DotNetBar.LabelX labelX18;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelXErr;
        public DevComponents.DotNetBar.ButtonX btnRuleFile;
        public DevComponents.DotNetBar.Controls.TextBoxX textRuleFilePath;
        public DevComponents.DotNetBar.ButtonX btnProj;
        public DevComponents.DotNetBar.Controls.TextBoxX txtProjFilePath;
        private DevComponents.DotNetBar.ButtonX btn_cancle;
        private DevComponents.DotNetBar.ButtonX btn_OK;
        private DevComponents.DotNetBar.Controls.TextBoxX txtVersion;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX comBoxType;
        private DevComponents.DotNetBar.Controls.TextBoxX txtservername;
        private DevComponents.DotNetBar.LabelX labelX5;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}