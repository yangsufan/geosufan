namespace GeoCustomExport
{
    partial class frmDBPropertySet
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
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.labelX15 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.labelX7 = new System.Windows.Forms.Label();
            this.labelX6 = new System.Windows.Forms.Label();
            this.txtDB = new System.Windows.Forms.TextBox();
            this.labelX5 = new System.Windows.Forms.Label();
            this.txtInstance = new System.Windows.Forms.TextBox();
            this.labelX4 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.labelX3 = new System.Windows.Forms.Label();
            this.labelX2 = new System.Windows.Forms.Label();
            this.btnDB = new System.Windows.Forms.Button();
            this.comBoxType = new System.Windows.Forms.ComboBox();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnTest = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(88, 171);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(157, 21);
            this.txtVersion.TabIndex = 8;
            this.txtVersion.Text = "SDE.DEFAULT";
            // 
            // labelX15
            // 
            this.labelX15.AutoSize = true;
            this.labelX15.Location = new System.Drawing.Point(11, 174);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(71, 12);
            this.labelX15.TabIndex = 25;
            this.labelX15.Text = "  版   本 :";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(88, 144);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(157, 21);
            this.txtPassword.TabIndex = 7;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(88, 117);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(157, 21);
            this.txtUser.TabIndex = 6;
            // 
            // labelX7
            // 
            this.labelX7.AutoSize = true;
            this.labelX7.Location = new System.Drawing.Point(12, 147);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(71, 12);
            this.labelX7.TabIndex = 24;
            this.labelX7.Text = "  密   码 :";
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            this.labelX6.Location = new System.Drawing.Point(12, 120);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(71, 12);
            this.labelX6.TabIndex = 22;
            this.labelX6.Text = " 用 户 名 :";
            // 
            // txtDB
            // 
            this.txtDB.Location = new System.Drawing.Point(88, 90);
            this.txtDB.Name = "txtDB";
            this.txtDB.Size = new System.Drawing.Size(132, 21);
            this.txtDB.TabIndex = 4;
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            this.labelX5.Location = new System.Drawing.Point(12, 93);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(71, 12);
            this.labelX5.TabIndex = 20;
            this.labelX5.Text = " 数 据 库 :";
            // 
            // txtInstance
            // 
            this.txtInstance.Location = new System.Drawing.Point(88, 63);
            this.txtInstance.Name = "txtInstance";
            this.txtInstance.Size = new System.Drawing.Size(157, 21);
            this.txtInstance.TabIndex = 3;
            this.txtInstance.Text = "5152";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.Location = new System.Drawing.Point(12, 66);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(65, 12);
            this.labelX4.TabIndex = 23;
            this.labelX4.Text = " 服务端口:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(88, 36);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(157, 21);
            this.txtServer.TabIndex = 2;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.Location = new System.Drawing.Point(12, 39);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(71, 12);
            this.labelX3.TabIndex = 21;
            this.labelX3.Text = " 服 务 器 :";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(12, 12);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(71, 12);
            this.labelX2.TabIndex = 30;
            this.labelX2.Text = " 类    型 :";
            // 
            // btnDB
            // 
            this.btnDB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDB.Location = new System.Drawing.Point(220, 90);
            this.btnDB.Name = "btnDB";
            this.btnDB.Size = new System.Drawing.Size(26, 21);
            this.btnDB.TabIndex = 5;
            this.btnDB.Text = "...";
            this.btnDB.Click += new System.EventHandler(this.btnDB_Click);
            // 
            // comBoxType
            // 
            this.comBoxType.FormattingEnabled = true;
            this.comBoxType.Items.AddRange(new object[] {
            "ArcSDE(For Oracle)",
            "ESRI文件数据库(*.gdb)",
            "ESRI个人数据库(*.mdb)"});
            this.comBoxType.Location = new System.Drawing.Point(88, 9);
            this.comBoxType.Name = "comBoxType";
            this.comBoxType.Size = new System.Drawing.Size(157, 20);
            this.comBoxType.TabIndex = 31;
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(196, 202);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(53, 23);
            this.btnOK.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.btnOK.TabIndex = 32;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnTest
            // 
            this.btnTest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTest.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTest.Location = new System.Drawing.Point(129, 202);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(53, 23);
            this.btnTest.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.btnTest.TabIndex = 32;
            this.btnTest.Text = "测试";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // frmDBPropertySet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 230);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.comBoxType);
            this.Controls.Add(this.btnDB);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.labelX15);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.txtDB);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.txtInstance);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.labelX3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDBPropertySet";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置数据源连接";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label labelX15;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label labelX7;
        private System.Windows.Forms.Label labelX6;
        private System.Windows.Forms.TextBox txtDB;
        private System.Windows.Forms.Label labelX5;
        private System.Windows.Forms.TextBox txtInstance;
        private System.Windows.Forms.Label labelX4;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label labelX3;
        private System.Windows.Forms.Label labelX2;
        private System.Windows.Forms.Button btnDB;
        private System.Windows.Forms.ComboBox comBoxType;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnTest;
    }
}