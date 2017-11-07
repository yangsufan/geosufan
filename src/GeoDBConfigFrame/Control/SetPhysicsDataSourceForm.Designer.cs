namespace GeoDBConfigFrame
{
    partial class SetPhysicsDataSourceForm
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
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxDsName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnDel = new DevComponents.DotNetBar.ButtonX();
            this.buttonXTest = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // txtDataBase
            // 
            // 
            // 
            // 
            this.txtDataBase.Border.Class = "TextBoxBorder";
            this.txtDataBase.Location = new System.Drawing.Point(104, 117);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Size = new System.Drawing.Size(173, 21);
            this.txtDataBase.TabIndex = 49;
            // 
            // labelX10
            // 
            this.labelX10.Location = new System.Drawing.Point(12, 118);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(74, 18);
            this.labelX10.TabIndex = 61;
            this.labelX10.Text = "数据库:";
            // 
            // txtVersion
            // 
            // 
            // 
            // 
            this.txtVersion.Border.Class = "TextBoxBorder";
            this.txtVersion.Location = new System.Drawing.Point(104, 199);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(173, 21);
            this.txtVersion.TabIndex = 54;
            this.txtVersion.Text = "SDE.DEFAULT";
            // 
            // txtPassWord
            // 
            // 
            // 
            // 
            this.txtPassWord.Border.Class = "TextBoxBorder";
            this.txtPassWord.Location = new System.Drawing.Point(104, 172);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(173, 21);
            this.txtPassWord.TabIndex = 51;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(215, 233);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 28);
            this.btnCancel.TabIndex = 56;
            this.btnCancel.Text = "退 出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(75, 233);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 28);
            this.btnOK.TabIndex = 58;
            this.btnOK.Text = "新 建";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtUser
            // 
            // 
            // 
            // 
            this.txtUser.Border.Class = "TextBoxBorder";
            this.txtUser.Location = new System.Drawing.Point(104, 144);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(173, 21);
            this.txtUser.TabIndex = 50;
            // 
            // txtService
            // 
            // 
            // 
            // 
            this.txtService.Border.Class = "TextBoxBorder";
            this.txtService.Location = new System.Drawing.Point(104, 90);
            this.txtService.Name = "txtService";
            this.txtService.Size = new System.Drawing.Size(173, 21);
            this.txtService.TabIndex = 48;
            this.txtService.Text = "esri_sde";
            // 
            // txtServer
            // 
            // 
            // 
            // 
            this.txtServer.Border.Class = "TextBoxBorder";
            this.txtServer.Location = new System.Drawing.Point(104, 61);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(134, 21);
            this.txtServer.TabIndex = 46;
            this.txtServer.WordWrap = false;
            // 
            // labelX9
            // 
            this.labelX9.Location = new System.Drawing.Point(12, 200);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(74, 18);
            this.labelX9.TabIndex = 60;
            this.labelX9.Text = "版  本:";
            // 
            // labelX8
            // 
            this.labelX8.Location = new System.Drawing.Point(12, 173);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(74, 18);
            this.labelX8.TabIndex = 59;
            this.labelX8.Text = "密  码:";
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(12, 146);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(74, 18);
            this.labelX7.TabIndex = 55;
            this.labelX7.Text = "用户名:";
            // 
            // labelX6
            // 
            this.labelX6.Location = new System.Drawing.Point(12, 91);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(74, 18);
            this.labelX6.TabIndex = 53;
            this.labelX6.Text = "服  务:";
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(12, 63);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(74, 18);
            this.labelX5.TabIndex = 52;
            this.labelX5.Text = "服务器名称:";
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btnServer.Location = new System.Drawing.Point(244, 61);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(33, 21);
            this.btnServer.TabIndex = 47;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // cboDataType
            // 
            this.cboDataType.DisplayMember = "Text";
            this.cboDataType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboDataType.FormattingEnabled = true;
            this.cboDataType.Location = new System.Drawing.Point(104, 33);
            this.cboDataType.Name = "cboDataType";
            this.cboDataType.Size = new System.Drawing.Size(173, 22);
            this.cboDataType.TabIndex = 45;
            this.cboDataType.SelectedIndexChanged += new System.EventHandler(this.cboDataType_SelectedIndexChanged);
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.labelX3.Location = new System.Drawing.Point(12, 36);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(74, 18);
            this.labelX3.TabIndex = 44;
            this.labelX3.Text = "数据源类型:";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.labelX1.Location = new System.Drawing.Point(12, 9);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 18);
            this.labelX1.TabIndex = 44;
            this.labelX1.Text = "数据源名称:";
            // 
            // comboBoxDsName
            // 
            this.comboBoxDsName.DisplayMember = "Text";
            this.comboBoxDsName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxDsName.FormattingEnabled = true;
            this.comboBoxDsName.Location = new System.Drawing.Point(104, 5);
            this.comboBoxDsName.Name = "comboBoxDsName";
            this.comboBoxDsName.Size = new System.Drawing.Size(173, 22);
            this.comboBoxDsName.TabIndex = 45;
            this.comboBoxDsName.SelectedIndexChanged += new System.EventHandler(this.comboBoxDsName_SelectedIndexChanged);
            // 
            // btnDel
            // 
            this.btnDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDel.Location = new System.Drawing.Point(144, 233);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(65, 28);
            this.btnDel.TabIndex = 58;
            this.btnDel.Text = "删 除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // buttonXTest
            // 
            this.buttonXTest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXTest.Location = new System.Drawing.Point(4, 233);
            this.buttonXTest.Name = "buttonXTest";
            this.buttonXTest.Size = new System.Drawing.Size(65, 28);
            this.buttonXTest.TabIndex = 62;
            this.buttonXTest.Text = "测 试";
            this.buttonXTest.Click += new System.EventHandler(this.buttonXTest_Click);
            // 
            // SetPhysicsDataSourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 273);
            this.Controls.Add(this.buttonXTest);
            this.Controls.Add(this.txtDataBase);
            this.Controls.Add(this.labelX10);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.txtPassWord);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtService);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.labelX9);
            this.Controls.Add(this.labelX8);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.comboBoxDsName);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cboDataType);
            this.Controls.Add(this.labelX3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetPhysicsDataSourceForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据源配置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtDataBase;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.Controls.TextBoxX txtVersion;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassWord;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
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
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxDsName;
        private DevComponents.DotNetBar.ButtonX btnDel;
        private DevComponents.DotNetBar.ButtonX buttonXTest;
    }
}