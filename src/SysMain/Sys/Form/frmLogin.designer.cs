namespace GDBM
{
    partial class frmLogin
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
            this.textBoxXPassword = new DevExpress.XtraEditors.TextEdit();
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            this.buttonX3 = new DevExpress.XtraEditors.SimpleButton();
            this.checkBoxNotPassWord = new DevExpress.XtraEditors.CheckEdit();
            this.txtUser = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxXPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxNotPassWord.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxXPassword
            // 
            this.textBoxXPassword.Location = new System.Drawing.Point(252, 201);
            this.textBoxXPassword.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxXPassword.Name = "textBoxXPassword";
            this.textBoxXPassword.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxXPassword.Properties.Appearance.Options.UseFont = true;
            this.textBoxXPassword.Properties.PasswordChar = '*';
            this.textBoxXPassword.Size = new System.Drawing.Size(234, 26);
            this.textBoxXPassword.TabIndex = 3;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonX1.Appearance.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonX1.Appearance.Options.UseFont = true;
            this.buttonX1.Appearance.Options.UseForeColor = true;
            this.buttonX1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonX1.Location = new System.Drawing.Point(410, 239);
            this.buttonX1.Margin = new System.Windows.Forms.Padding(4);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(76, 30);
            this.buttonX1.TabIndex = 4;
            this.buttonX1.Text = "登  录";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonX3.Appearance.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.buttonX3.Appearance.Options.UseFont = true;
            this.buttonX3.Appearance.Options.UseForeColor = true;
            this.buttonX3.Location = new System.Drawing.Point(326, 239);
            this.buttonX3.Margin = new System.Windows.Forms.Padding(4);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(76, 30);
            this.buttonX3.TabIndex = 6;
            this.buttonX3.Text = "退  出";
            this.buttonX3.Click += new System.EventHandler(this.buttonX3_Click);
            // 
            // checkBoxNotPassWord
            // 
            this.checkBoxNotPassWord.Location = new System.Drawing.Point(188, 247);
            this.checkBoxNotPassWord.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxNotPassWord.Name = "checkBoxNotPassWord";
            this.checkBoxNotPassWord.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxNotPassWord.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxNotPassWord.Properties.Appearance.Options.UseBackColor = true;
            this.checkBoxNotPassWord.Properties.Appearance.Options.UseFont = true;
            this.checkBoxNotPassWord.Properties.Caption = "记住密码";
            this.checkBoxNotPassWord.Size = new System.Drawing.Size(88, 19);
            this.checkBoxNotPassWord.TabIndex = 7;
            this.checkBoxNotPassWord.Visible = false;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(252, 167);
            this.txtUser.Margin = new System.Windows.Forms.Padding(4);
            this.txtUser.Name = "txtUser";
            this.txtUser.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUser.Properties.Appearance.Options.UseFont = true;
            this.txtUser.Size = new System.Drawing.Size(234, 26);
            this.txtUser.TabIndex = 8;
            // 
            // frmLogin
            // 
            this.AcceptButton = this.buttonX1;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Tile;
            this.BackgroundImageStore = global::GDBM.Properties.Resources.bglogin;
            this.ClientSize = new System.Drawing.Size(675, 476);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.checkBoxNotPassWord);
            this.Controls.Add(this.buttonX3);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.textBoxXPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.frmLogin_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.textBoxXPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxNotPassWord.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textBoxXPassword;
        private DevExpress.XtraEditors.SimpleButton buttonX1;
        private DevExpress.XtraEditors.SimpleButton buttonX3;
        private DevExpress.XtraEditors.CheckEdit checkBoxNotPassWord;
        private DevExpress.XtraEditors.TextEdit txtUser;
    }
}