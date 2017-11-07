namespace GeoDBConfigFrame
{
    partial class DataBaseLinkControl
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
            this.txtDataBase = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.txtVersion = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtPassWord = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.buttonXTest = new DevComponents.DotNetBar.ButtonX();
            this.txtUser = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtService = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtServer = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
            this.cboDataType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // txtDataBase
            // 
            // 
            // 
            // 
            this.txtDataBase.Border.Class = "TextBoxBorder";
            this.txtDataBase.Location = new System.Drawing.Point(78, 112);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Size = new System.Drawing.Size(173, 21);
            this.txtDataBase.TabIndex = 33;
            // 
            // labelX10
            // 
            this.labelX10.Location = new System.Drawing.Point(17, 113);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(54, 19);
            this.labelX10.TabIndex = 43;
            this.labelX10.Text = "数据库：";
            // 
            // txtVersion
            // 
            // 
            // 
            // 
            this.txtVersion.Border.Class = "TextBoxBorder";
            this.txtVersion.Location = new System.Drawing.Point(78, 194);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(173, 21);
            this.txtVersion.TabIndex = 38;
            this.txtVersion.Text = "SDE.DEFAULT";
            // 
            // txtPassWord
            // 
            // 
            // 
            // 
            this.txtPassWord.Border.Class = "TextBoxBorder";
            this.txtPassWord.Location = new System.Drawing.Point(78, 167);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(173, 21);
            this.txtPassWord.TabIndex = 35;
            // 
            // buttonXTest
            // 
            this.buttonXTest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXTest.Location = new System.Drawing.Point(195, 228);
            this.buttonXTest.Name = "buttonXTest";
            this.buttonXTest.Size = new System.Drawing.Size(56, 23);
            this.buttonXTest.TabIndex = 40;
            this.buttonXTest.Text = "测 试";
            this.buttonXTest.Click += new System.EventHandler(this.buttonXTest_Click);
            // 
            // txtUser
            // 
            // 
            // 
            // 
            this.txtUser.Border.Class = "TextBoxBorder";
            this.txtUser.Location = new System.Drawing.Point(78, 139);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(173, 21);
            this.txtUser.TabIndex = 34;
            // 
            // txtService
            // 
            // 
            // 
            // 
            this.txtService.Border.Class = "TextBoxBorder";
            this.txtService.Location = new System.Drawing.Point(78, 85);
            this.txtService.Name = "txtService";
            this.txtService.Size = new System.Drawing.Size(173, 21);
            this.txtService.TabIndex = 32;
            this.txtService.Text = "5151";
            // 
            // txtServer
            // 
            // 
            // 
            // 
            this.txtServer.Border.Class = "TextBoxBorder";
            this.txtServer.Location = new System.Drawing.Point(78, 56);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(134, 21);
            this.txtServer.TabIndex = 30;
            this.txtServer.WordWrap = false;
            // 
            // labelX9
            // 
            this.labelX9.Location = new System.Drawing.Point(17, 195);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(54, 19);
            this.labelX9.TabIndex = 42;
            this.labelX9.Text = "版  本：";
            // 
            // labelX8
            // 
            this.labelX8.Location = new System.Drawing.Point(17, 168);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(54, 19);
            this.labelX8.TabIndex = 41;
            this.labelX8.Text = "密  码：";
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(17, 141);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(54, 19);
            this.labelX7.TabIndex = 39;
            this.labelX7.Text = "用  户：";
            // 
            // labelX6
            // 
            this.labelX6.Location = new System.Drawing.Point(17, 86);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(54, 19);
            this.labelX6.TabIndex = 37;
            this.labelX6.Text = "服务名：";
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(17, 58);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(54, 19);
            this.labelX5.TabIndex = 36;
            this.labelX5.Text = "服务器：";
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btnServer.Location = new System.Drawing.Point(218, 56);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(33, 21);
            this.btnServer.TabIndex = 31;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // cboDataType
            // 
            this.cboDataType.DisplayMember = "Text";
            this.cboDataType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboDataType.FormattingEnabled = true;
            this.cboDataType.Location = new System.Drawing.Point(78, 28);
            this.cboDataType.Name = "cboDataType";
            this.cboDataType.Size = new System.Drawing.Size(173, 22);
            this.cboDataType.TabIndex = 29;
            this.cboDataType.SelectedIndexChanged += new System.EventHandler(this.cboDataType_SelectedIndexChanged);
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.labelX3.Location = new System.Drawing.Point(17, 32);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(56, 18);
            this.labelX3.TabIndex = 28;
            this.labelX3.Text = "类  型：";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(17, 228);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(56, 23);
            this.btnOK.TabIndex = 40;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(94, 228);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 23);
            this.btnCancel.TabIndex = 40;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DataBaseLinkControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 259);
            this.Controls.Add(this.txtDataBase);
            this.Controls.Add(this.labelX10);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.txtPassWord);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.buttonXTest);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtService);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.labelX9);
            this.Controls.Add(this.labelX8);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.cboDataType);
            this.Controls.Add(this.labelX3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataBaseLinkControl";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库连接信息";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtDataBase;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.Controls.TextBoxX txtVersion;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassWord;
        private DevComponents.DotNetBar.ButtonX buttonXTest;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUser;
        private DevComponents.DotNetBar.Controls.TextBoxX txtService;
        private DevComponents.DotNetBar.Controls.TextBoxX txtServer;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.ButtonX btnServer;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboDataType;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
    }
}