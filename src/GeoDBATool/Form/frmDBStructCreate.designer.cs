namespace GeoDBATool
{
    partial class frmDBStructCreate
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
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.comBoxType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
            this.txtDataBase = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.txtVersion = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtPassWord = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtUser = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtInstance = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtServer = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            this.labelX16 = new DevComponents.DotNetBar.LabelX();
            this.labelX17 = new DevComponents.DotNetBar.LabelX();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.txtProjFilePath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnProj = new DevComponents.DotNetBar.ButtonX();
            this.btnRuleFile = new DevComponents.DotNetBar.ButtonX();
            this.textRuleFilePath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.labelXErr = new DevComponents.DotNetBar.LabelX();
            this.groupPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.comBoxType);
            this.groupPanel3.Controls.Add(this.labelX4);
            this.groupPanel3.Controls.Add(this.btnServer);
            this.groupPanel3.Controls.Add(this.txtDataBase);
            this.groupPanel3.Controls.Add(this.labelX13);
            this.groupPanel3.Controls.Add(this.txtVersion);
            this.groupPanel3.Controls.Add(this.txtPassWord);
            this.groupPanel3.Controls.Add(this.txtUser);
            this.groupPanel3.Controls.Add(this.txtInstance);
            this.groupPanel3.Controls.Add(this.txtServer);
            this.groupPanel3.Controls.Add(this.labelX14);
            this.groupPanel3.Controls.Add(this.labelX15);
            this.groupPanel3.Controls.Add(this.labelX16);
            this.groupPanel3.Controls.Add(this.labelX17);
            this.groupPanel3.Controls.Add(this.labelX18);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(12, 22);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(272, 223);
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
            this.groupPanel3.TabIndex = 27;
            this.groupPanel3.Text = "数据库连接设置";
            // 
            // comBoxType
            // 
            this.comBoxType.DisplayMember = "Text";
            this.comBoxType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoxType.FormattingEnabled = true;
            this.comBoxType.ItemHeight = 15;
            this.comBoxType.Location = new System.Drawing.Point(56, 3);
            this.comBoxType.Name = "comBoxType";
            this.comBoxType.Size = new System.Drawing.Size(207, 21);
            this.comBoxType.TabIndex = 42;
            this.comBoxType.SelectedIndexChanged += new System.EventHandler(this.comBoxType_SelectedIndexChanged);
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
            this.btnServer.Location = new System.Drawing.Point(237, 84);
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
            this.txtDataBase.Size = new System.Drawing.Size(184, 21);
            this.txtDataBase.TabIndex = 16;
            this.txtDataBase.WatermarkText = "本地库路径";
            this.txtDataBase.TextChanged += new System.EventHandler(this.txtDataBase_TextChanged);
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
            // txtVersion
            // 
            // 
            // 
            // 
            this.txtVersion.Border.Class = "TextBoxBorder";
            this.txtVersion.Location = new System.Drawing.Point(56, 165);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(207, 21);
            this.txtVersion.TabIndex = 10;
            this.txtVersion.Text = "SDE.DEFAULT";
            this.txtVersion.TextChanged += new System.EventHandler(this.txtVersion_TextChanged);
            // 
            // txtPassWord
            // 
            // 
            // 
            // 
            this.txtPassWord.Border.Class = "TextBoxBorder";
            this.txtPassWord.Location = new System.Drawing.Point(56, 138);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(207, 21);
            this.txtPassWord.TabIndex = 9;
            this.txtPassWord.WatermarkText = "SDE访问密码";
            this.txtPassWord.TextChanged += new System.EventHandler(this.txtPassWord_TextChanged);
            // 
            // txtUser
            // 
            // 
            // 
            // 
            this.txtUser.Border.Class = "TextBoxBorder";
            this.txtUser.Location = new System.Drawing.Point(56, 111);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(207, 21);
            this.txtUser.TabIndex = 8;
            this.txtUser.WatermarkText = "SDE访问用户名";
            this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            // 
            // txtInstance
            // 
            // 
            // 
            // 
            this.txtInstance.Border.Class = "TextBoxBorder";
            this.txtInstance.Location = new System.Drawing.Point(56, 57);
            this.txtInstance.Name = "txtInstance";
            this.txtInstance.Size = new System.Drawing.Size(207, 21);
            this.txtInstance.TabIndex = 7;
            this.txtInstance.WatermarkText = "数据库实例名称";
            this.txtInstance.TextChanged += new System.EventHandler(this.txtInstance_TextChanged);
            // 
            // txtServer
            // 
            // 
            // 
            // 
            this.txtServer.Border.Class = "TextBoxBorder";
            this.txtServer.Location = new System.Drawing.Point(56, 30);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(207, 21);
            this.txtServer.TabIndex = 6;
            this.txtServer.WatermarkText = "服务器IP地址或机器名";
            this.txtServer.WordWrap = false;
            this.txtServer.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // labelX14
            // 
            this.labelX14.AutoSize = true;
            this.labelX14.Location = new System.Drawing.Point(3, 168);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(56, 18);
            this.labelX14.TabIndex = 5;
            this.labelX14.Text = "版  本 :";
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
            // labelX17
            // 
            this.labelX17.AutoSize = true;
            this.labelX17.Location = new System.Drawing.Point(3, 60);
            this.labelX17.Name = "labelX17";
            this.labelX17.Size = new System.Drawing.Size(56, 18);
            this.labelX17.TabIndex = 2;
            this.labelX17.Text = "实例名 :";
            // 
            // labelX18
            // 
            this.labelX18.AutoSize = true;
            this.labelX18.Location = new System.Drawing.Point(3, 33);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(56, 18);
            this.labelX18.TabIndex = 1;
            this.labelX18.Text = "服务器 :";
            // 
            // txtProjFilePath
            // 
            // 
            // 
            // 
            this.txtProjFilePath.Border.Class = "TextBoxBorder";
            this.txtProjFilePath.Location = new System.Drawing.Point(12, 251);
            this.txtProjFilePath.Name = "txtProjFilePath";
            this.txtProjFilePath.Size = new System.Drawing.Size(243, 21);
            this.txtProjFilePath.TabIndex = 34;
            this.txtProjFilePath.WatermarkText = "空间参考投影文件路径";
            this.txtProjFilePath.TextChanged += new System.EventHandler(this.txtProjFilePath_TextChanged);
            // 
            // btnProj
            // 
            this.btnProj.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnProj.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnProj.Location = new System.Drawing.Point(252, 251);
            this.btnProj.Name = "btnProj";
            this.btnProj.Size = new System.Drawing.Size(26, 21);
            this.btnProj.TabIndex = 35;
            this.btnProj.Text = "...";
            this.btnProj.Click += new System.EventHandler(this.btnProj_Click);
            // 
            // btnRuleFile
            // 
            this.btnRuleFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRuleFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRuleFile.Location = new System.Drawing.Point(252, 278);
            this.btnRuleFile.Name = "btnRuleFile";
            this.btnRuleFile.Size = new System.Drawing.Size(26, 21);
            this.btnRuleFile.TabIndex = 37;
            this.btnRuleFile.Text = "...";
            this.btnRuleFile.Click += new System.EventHandler(this.btnRuleFile_Click);
            // 
            // textRuleFilePath
            // 
            // 
            // 
            // 
            this.textRuleFilePath.Border.Class = "TextBoxBorder";
            this.textRuleFilePath.Location = new System.Drawing.Point(12, 278);
            this.textRuleFilePath.Name = "textRuleFilePath";
            this.textRuleFilePath.Size = new System.Drawing.Size(243, 21);
            this.textRuleFilePath.TabIndex = 36;
            this.textRuleFilePath.WatermarkText = "库体配置文件路径";
            this.textRuleFilePath.TextChanged += new System.EventHandler(this.textRuleFilePath_TextChanged);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(218, 330);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(70, 25);
            this.btnCancle.TabIndex = 41;
            this.btnCancle.Text = "取 消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(142, 330);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 25);
            this.btnOk.TabIndex = 40;
            this.btnOk.Text = "确 定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // labelXErr
            // 
            this.labelXErr.Location = new System.Drawing.Point(12, 305);
            this.labelXErr.Name = "labelXErr";
            this.labelXErr.Size = new System.Drawing.Size(272, 19);
            this.labelXErr.TabIndex = 42;
            this.labelXErr.Click += new System.EventHandler(this.labelXErr_Click);
            // 
            // frmDBStructCreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 363);
            this.Controls.Add(this.labelXErr);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnRuleFile);
            this.Controls.Add(this.textRuleFilePath);
            this.Controls.Add(this.btnProj);
            this.Controls.Add(this.txtProjFilePath);
            this.Controls.Add(this.groupPanel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDBStructCreate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "创建空间数据库体结构";
            this.Load += new System.EventHandler(this.frmDBStructCreate_Load);
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        public DevComponents.DotNetBar.ButtonX btnServer;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDataBase;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.Controls.TextBoxX txtVersion;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassWord;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUser;
        private DevComponents.DotNetBar.Controls.TextBoxX txtInstance;
        private DevComponents.DotNetBar.Controls.TextBoxX txtServer;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.DotNetBar.LabelX labelX15;
        private DevComponents.DotNetBar.LabelX labelX16;
        private DevComponents.DotNetBar.LabelX labelX17;
        private DevComponents.DotNetBar.LabelX labelX18;
        public DevComponents.DotNetBar.Controls.TextBoxX txtProjFilePath;
        public DevComponents.DotNetBar.ButtonX btnProj;
        public DevComponents.DotNetBar.ButtonX btnRuleFile;
        public DevComponents.DotNetBar.Controls.TextBoxX textRuleFilePath;
        public DevComponents.DotNetBar.ButtonX btnCancle;
        public DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.DotNetBar.LabelX labelXErr;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comBoxType;
        private DevComponents.DotNetBar.LabelX labelX4;


    }
}