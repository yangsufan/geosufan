namespace SysCommon.Gis
{
    partial class UIDataConnect
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupPanel = new DevExpress.XtraEditors.GroupControl();
            this.btnNew = new DevExpress.XtraEditors.SimpleButton();
            this.buttonXTest = new DevExpress.XtraEditors.SimpleButton();
            this.buttonXOK = new DevExpress.XtraEditors.SimpleButton();
            this.cboVersion = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtDataBase = new DevExpress.XtraEditors.TextEdit();
            this.labelX10 = new DevExpress.XtraEditors.LabelControl();
            this.txtPassWord = new DevExpress.XtraEditors.TextEdit();
            this.txtUser = new DevExpress.XtraEditors.TextEdit();
            this.txtService = new DevExpress.XtraEditors.TextEdit();
            this.txtServer = new DevExpress.XtraEditors.TextEdit();
            this.labelX9 = new DevExpress.XtraEditors.LabelControl();
            this.labelX8 = new DevExpress.XtraEditors.LabelControl();
            this.labelX7 = new DevExpress.XtraEditors.LabelControl();
            this.labelX6 = new DevExpress.XtraEditors.LabelControl();
            this.labelX5 = new DevExpress.XtraEditors.LabelControl();
            this.btnServer = new DevExpress.XtraEditors.SimpleButton();
            this.cboDataType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.errorServer = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorService = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorUser = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorPassWord = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorVersion = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorDataBase = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel)).BeginInit();
            this.groupPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboVersion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataBase.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassWord.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtService.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDataType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorPassWord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorDataBase)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel
            // 
            this.groupPanel.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.groupPanel.Controls.Add(this.btnNew);
            this.groupPanel.Controls.Add(this.buttonXTest);
            this.groupPanel.Controls.Add(this.buttonXOK);
            this.groupPanel.Controls.Add(this.cboVersion);
            this.groupPanel.Controls.Add(this.txtDataBase);
            this.groupPanel.Controls.Add(this.labelX10);
            this.groupPanel.Controls.Add(this.txtPassWord);
            this.groupPanel.Controls.Add(this.txtUser);
            this.groupPanel.Controls.Add(this.txtService);
            this.groupPanel.Controls.Add(this.txtServer);
            this.groupPanel.Controls.Add(this.labelX9);
            this.groupPanel.Controls.Add(this.labelX8);
            this.groupPanel.Controls.Add(this.labelX7);
            this.groupPanel.Controls.Add(this.labelX6);
            this.groupPanel.Controls.Add(this.labelX5);
            this.groupPanel.Controls.Add(this.btnServer);
            this.groupPanel.Controls.Add(this.cboDataType);
            this.groupPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel.Location = new System.Drawing.Point(0, 0);
            this.groupPanel.Name = "groupPanel";
            this.groupPanel.Size = new System.Drawing.Size(336, 290);
            this.groupPanel.TabIndex = 66;
            this.groupPanel.Text = "数据连接";
            // 
            // btnNew
            // 
            this.btnNew.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(274, 33);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(38, 24);
            this.btnNew.TabIndex = 31;
            this.btnNew.Text = "新建";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // buttonXTest
            // 
            this.buttonXTest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonXTest.Location = new System.Drawing.Point(225, 232);
            this.buttonXTest.Name = "buttonXTest";
            this.buttonXTest.Size = new System.Drawing.Size(87, 27);
            this.buttonXTest.TabIndex = 29;
            this.buttonXTest.Text = "测 试";
            this.buttonXTest.Click += new System.EventHandler(this.buttonXTest_Click);
            // 
            // buttonXOK
            // 
            this.buttonXOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonXOK.Location = new System.Drawing.Point(225, 232);
            this.buttonXOK.Name = "buttonXOK";
            this.buttonXOK.Size = new System.Drawing.Size(87, 27);
            this.buttonXOK.TabIndex = 30;
            this.buttonXOK.Text = "确 定";
            this.buttonXOK.Visible = false;
            this.buttonXOK.Click += new System.EventHandler(this.buttonXOK_Click);
            // 
            // cboVersion
            // 
            this.cboVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboVersion.EditValue = "SDE.DEFAULT";
            this.cboVersion.Location = new System.Drawing.Point(73, 195);
            this.cboVersion.Name = "cboVersion";
            this.cboVersion.Size = new System.Drawing.Size(239, 20);
            this.cboVersion.TabIndex = 28;
            // 
            // txtDataBase
            // 
            this.txtDataBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataBase.Location = new System.Drawing.Point(75, 98);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Size = new System.Drawing.Size(238, 20);
            this.txtDataBase.TabIndex = 12;
            // 
            // labelX10
            // 
            this.labelX10.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelX10.Appearance.Options.UseBackColor = true;
            this.labelX10.Location = new System.Drawing.Point(3, 99);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(48, 14);
            this.labelX10.TabIndex = 27;
            this.labelX10.Text = "数据库：";
            // 
            // txtPassWord
            // 
            this.txtPassWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassWord.Location = new System.Drawing.Point(75, 162);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.Properties.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(238, 20);
            this.txtPassWord.TabIndex = 16;
            // 
            // txtUser
            // 
            this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUser.Location = new System.Drawing.Point(75, 129);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(238, 20);
            this.txtUser.TabIndex = 14;
            // 
            // txtService
            // 
            this.txtService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtService.EditValue = "5151";
            this.txtService.Location = new System.Drawing.Point(75, 66);
            this.txtService.Name = "txtService";
            this.txtService.Size = new System.Drawing.Size(238, 20);
            this.txtService.TabIndex = 10;
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.EditValue = "192.168.100.2";
            this.txtServer.Location = new System.Drawing.Point(75, 33);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(148, 20);
            this.txtServer.TabIndex = 8;
            // 
            // labelX9
            // 
            this.labelX9.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelX9.Appearance.Options.UseBackColor = true;
            this.labelX9.Location = new System.Drawing.Point(3, 197);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(44, 14);
            this.labelX9.TabIndex = 21;
            this.labelX9.Text = "版  本：";
            // 
            // labelX8
            // 
            this.labelX8.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelX8.Appearance.Options.UseBackColor = true;
            this.labelX8.Location = new System.Drawing.Point(3, 163);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(44, 14);
            this.labelX8.TabIndex = 20;
            this.labelX8.Text = "密  码：";
            // 
            // labelX7
            // 
            this.labelX7.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelX7.Appearance.Options.UseBackColor = true;
            this.labelX7.Location = new System.Drawing.Point(3, 132);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(44, 14);
            this.labelX7.TabIndex = 19;
            this.labelX7.Text = "用  户：";
            // 
            // labelX6
            // 
            this.labelX6.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelX6.Appearance.Options.UseBackColor = true;
            this.labelX6.Location = new System.Drawing.Point(3, 68);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(48, 14);
            this.labelX6.TabIndex = 18;
            this.labelX6.Text = "服务名：";
            // 
            // labelX5
            // 
            this.labelX5.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelX5.Appearance.Options.UseBackColor = true;
            this.labelX5.Location = new System.Drawing.Point(3, 35);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(48, 14);
            this.labelX5.TabIndex = 17;
            this.labelX5.Text = "服务器：";
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btnServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnServer.Location = new System.Drawing.Point(230, 33);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(38, 24);
            this.btnServer.TabIndex = 9;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // cboDataType
            // 
            this.cboDataType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDataType.Location = new System.Drawing.Point(75, 0);
            this.cboDataType.Name = "cboDataType";
            this.cboDataType.Size = new System.Drawing.Size(238, 20);
            this.cboDataType.TabIndex = 6;
            this.cboDataType.SelectedIndexChanged += new System.EventHandler(this.cboDataType_SelectedIndexChanged);
            // 
            // errorServer
            // 
            this.errorServer.ContainerControl = this;
            // 
            // errorService
            // 
            this.errorService.ContainerControl = this;
            // 
            // errorUser
            // 
            this.errorUser.ContainerControl = this;
            // 
            // errorPassWord
            // 
            this.errorPassWord.ContainerControl = this;
            // 
            // errorVersion
            // 
            this.errorVersion.ContainerControl = this;
            // 
            // errorDataBase
            // 
            this.errorDataBase.ContainerControl = this;
            // 
            // UIDataConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupPanel);
            this.Name = "UIDataConnect";
            this.Size = new System.Drawing.Size(336, 290);
            this.Load += new System.EventHandler(this.UIDataConnect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel)).EndInit();
            this.groupPanel.ResumeLayout(false);
            this.groupPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboVersion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataBase.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassWord.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtService.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDataType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorPassWord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorDataBase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupPanel;
        private DevExpress.XtraEditors.TextEdit txtDataBase;
        private DevExpress.XtraEditors.LabelControl labelX10;
        private DevExpress.XtraEditors.TextEdit txtPassWord;
        private DevExpress.XtraEditors.TextEdit txtUser;
        private DevExpress.XtraEditors.TextEdit txtService;
        private DevExpress.XtraEditors.TextEdit txtServer;
        private DevExpress.XtraEditors.LabelControl labelX9;
        private DevExpress.XtraEditors.LabelControl labelX8;
        private DevExpress.XtraEditors.LabelControl labelX7;
        private DevExpress.XtraEditors.LabelControl labelX6;
        private DevExpress.XtraEditors.LabelControl labelX5;
        private DevExpress.XtraEditors.SimpleButton btnServer;
        private DevExpress.XtraEditors.ComboBoxEdit cboDataType;
        private DevExpress.XtraEditors.ComboBoxEdit cboVersion;
        private DevExpress.XtraEditors.SimpleButton buttonXTest;
        private DevExpress.XtraEditors.SimpleButton buttonXOK;
        private DevExpress.XtraEditors.SimpleButton btnNew;
        private System.Windows.Forms.ErrorProvider errorServer;
        private System.Windows.Forms.ErrorProvider errorService;
        private System.Windows.Forms.ErrorProvider errorUser;
        private System.Windows.Forms.ErrorProvider errorPassWord;
        private System.Windows.Forms.ErrorProvider errorVersion;
        private System.Windows.Forms.ErrorProvider errorDataBase;
    }
}
