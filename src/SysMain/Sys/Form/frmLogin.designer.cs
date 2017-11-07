namespace GeoDatabaseManager
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
            this.textBoxXPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX3 = new DevComponents.DotNetBar.ButtonX();
            this.checkBoxNotPassWord = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.comboBoxUser = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // textBoxXPassword
            // 
            // 
            // 
            // 
            this.textBoxXPassword.Border.Class = "TextBoxBorder";
            this.textBoxXPassword.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxXPassword.Location = new System.Drawing.Point(337, 256);
            this.textBoxXPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxXPassword.Name = "textBoxXPassword";
            this.textBoxXPassword.PasswordChar = '*';
            this.textBoxXPassword.Size = new System.Drawing.Size(313, 31);
            this.textBoxXPassword.TabIndex = 3;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.BackColor = System.Drawing.Color.SeaGreen;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.buttonX1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonX1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonX1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonX1.Location = new System.Drawing.Point(456, 304);
            this.buttonX1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(87, 32);
            this.buttonX1.TabIndex = 4;
            this.buttonX1.Text = "登  录";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.BackColor = System.Drawing.Color.SeaGreen;
            this.buttonX3.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.buttonX3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonX3.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.buttonX3.Location = new System.Drawing.Point(560, 304);
            this.buttonX3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(87, 32);
            this.buttonX3.TabIndex = 6;
            this.buttonX3.Text = "退  出";
            this.buttonX3.Click += new System.EventHandler(this.buttonX3_Click);
            // 
            // checkBoxNotPassWord
            // 
            this.checkBoxNotPassWord.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxNotPassWord.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxNotPassWord.Location = new System.Drawing.Point(331, 302);
            this.checkBoxNotPassWord.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxNotPassWord.Name = "checkBoxNotPassWord";
            this.checkBoxNotPassWord.Size = new System.Drawing.Size(101, 31);
            this.checkBoxNotPassWord.TabIndex = 7;
            this.checkBoxNotPassWord.Text = "记住密码";
            this.checkBoxNotPassWord.TextColor = System.Drawing.SystemColors.Desktop;
            // 
            // comboBoxUser
            // 
            this.comboBoxUser.DisplayMember = "Text";
            this.comboBoxUser.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxUser.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxUser.FormattingEnabled = true;
            this.comboBoxUser.ItemHeight = 20;
            this.comboBoxUser.Location = new System.Drawing.Point(337, 209);
            this.comboBoxUser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxUser.Name = "comboBoxUser";
            this.comboBoxUser.Size = new System.Drawing.Size(312, 26);
            this.comboBoxUser.TabIndex = 8;
            this.comboBoxUser.SelectedIndexChanged += new System.EventHandler(this.comboBoxUser_SelectedIndexChanged);
            // 
            // frmLogin
            // 
            this.AcceptButton = this.buttonX1;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GeoDatabaseManager.Properties.Resources.登录2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(901, 595);
            this.Controls.Add(this.comboBoxUser);
            this.Controls.Add(this.checkBoxNotPassWord);
            this.Controls.Add(this.buttonX3);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.textBoxXPassword);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.Activated += new System.EventHandler(this.frmLogin_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLogin_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXPassword;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX buttonX3;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxNotPassWord;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxUser;
    }
}