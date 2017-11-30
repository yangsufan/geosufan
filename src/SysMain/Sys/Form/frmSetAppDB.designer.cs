namespace GDBM
{
    partial class frmSetAppDB
    {
        /// <summary>
        /// 必需的设计器变量。        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。        /// </summary>
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
        /// 使用代码编辑器修改此方法的内容。        /// </summary>
        private void InitializeComponent()
        {
            this.groupPanel1 = new DevExpress.XtraEditors.GroupControl();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.buttonX3 = new DevExpress.XtraEditors.SimpleButton();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.txtUser = new DevExpress.XtraEditors.TextEdit();
            this.txtServer = new DevExpress.XtraEditors.TextEdit();
            this.textBoxX1 = new DevExpress.XtraEditors.TextEdit();
            this.labelX4 = new DevExpress.XtraEditors.LabelControl();
            this.labelX2 = new DevExpress.XtraEditors.LabelControl();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            this.labelX3 = new DevExpress.XtraEditors.LabelControl();
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            this.buttonX2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxX1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.checkBox1);
            this.groupPanel1.Controls.Add(this.buttonX3);
            this.groupPanel1.Controls.Add(this.txtPassword);
            this.groupPanel1.Controls.Add(this.txtUser);
            this.groupPanel1.Controls.Add(this.txtServer);
            this.groupPanel1.Controls.Add(this.textBoxX1);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Location = new System.Drawing.Point(14, 14);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(280, 187);
            this.groupPanel1.TabIndex = 0;
            this.groupPanel1.Text = "系统维护库";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(14, 120);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(146, 18);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "初始化维护库库体结构";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.Location = new System.Drawing.Point(224, 85);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(38, 26);
            this.buttonX3.TabIndex = 12;
            this.buttonX3.Text = "测试";
            this.buttonX3.Click += new System.EventHandler(this.buttonX3_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(100, 86);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(126, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(100, 59);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(162, 20);
            this.txtUser.TabIndex = 2;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(100, 33);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(162, 20);
            this.txtServer.TabIndex = 1;
            // 
            // textBoxX1
            // 
            this.textBoxX1.EditValue = " Oracle";
            this.textBoxX1.Enabled = false;
            this.textBoxX1.Location = new System.Drawing.Point(100, 6);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Size = new System.Drawing.Size(162, 20);
            this.textBoxX1.TabIndex = 0;
            this.textBoxX1.UseWaitCursor = true;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(14, 33);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(48, 14);
            this.labelX4.TabIndex = 11;
            this.labelX4.Text = "服务名：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(14, 86);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(44, 14);
            this.labelX2.TabIndex = 10;
            this.labelX2.Text = "密  码：";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(14, 59);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(44, 14);
            this.labelX1.TabIndex = 9;
            this.labelX1.Text = "用  户：";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(14, 6);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(44, 14);
            this.labelX3.TabIndex = 8;
            this.labelX3.Text = "类  型：";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Location = new System.Drawing.Point(157, 209);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(101, 26);
            this.buttonX1.TabIndex = 1;
            this.buttonX1.Text = "取消";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Enabled = false;
            this.buttonX2.Location = new System.Drawing.Point(49, 209);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(101, 26);
            this.buttonX2.TabIndex = 2;
            this.buttonX2.Text = "确定";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // frmSetAppDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 246);
            this.Controls.Add(this.buttonX2);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.groupPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetAppDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置系统维护库连接信息";
            this.Load += new System.EventHandler(this.frmSetAppDB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxX1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupPanel1;
        private DevExpress.XtraEditors.LabelControl labelX4;
        private DevExpress.XtraEditors.LabelControl labelX2;
        private DevExpress.XtraEditors.LabelControl labelX1;
        private DevExpress.XtraEditors.LabelControl labelX3;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.TextEdit txtUser;
        private DevExpress.XtraEditors.TextEdit txtServer;
        private DevExpress.XtraEditors.TextEdit textBoxX1;
        private DevExpress.XtraEditors.SimpleButton buttonX1;
        private DevExpress.XtraEditors.SimpleButton buttonX2;
        private System.Windows.Forms.CheckBox checkBox1;
        private DevExpress.XtraEditors.SimpleButton buttonX3;
    }
}