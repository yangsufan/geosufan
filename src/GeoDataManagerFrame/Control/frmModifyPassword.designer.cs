namespace GeoDataManagerFrame
{
    partial class frmModifyPassword
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtOldPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtNewPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.bttCommit = new DevComponents.DotNetBar.ButtonX();
            this.bttCancel = new DevComponents.DotNetBar.ButtonX();
            this.errModifyPassword = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errModifyPassword)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(32, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "旧密码：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(32, 49);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 23);
            this.labelX2.TabIndex = 0;
            this.labelX2.Text = "新密码：";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(9, 85);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(83, 23);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "确认新密码：";
            // 
            // txtOldPassword
            // 
            // 
            // 
            // 
            this.txtOldPassword.Border.Class = "TextBoxBorder";
            this.txtOldPassword.Location = new System.Drawing.Point(88, 14);
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.PasswordChar = '*';
            this.txtOldPassword.Size = new System.Drawing.Size(157, 21);
            this.txtOldPassword.TabIndex = 1;
            this.txtOldPassword.TextChanged += new System.EventHandler(this.txtOldPassword_TextChanged);
            this.txtOldPassword.Leave += new System.EventHandler(this.txtOldPassword_Leave);
            // 
            // txtPassword
            // 
            // 
            // 
            // 
            this.txtPassword.Border.Class = "TextBoxBorder";
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(88, 85);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(157, 21);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            // 
            // txtNewPassword
            // 
            // 
            // 
            // 
            this.txtNewPassword.Border.Class = "TextBoxBorder";
            this.txtNewPassword.Enabled = false;
            this.txtNewPassword.Location = new System.Drawing.Point(88, 49);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '*';
            this.txtNewPassword.Size = new System.Drawing.Size(157, 21);
            this.txtNewPassword.TabIndex = 1;
            this.txtNewPassword.TextChanged += new System.EventHandler(this.txtNewPassword_TextChanged);
            this.txtNewPassword.Leave += new System.EventHandler(this.txtNewPassword_Leave);
            // 
            // bttCommit
            // 
            this.bttCommit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttCommit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttCommit.Enabled = false;
            this.bttCommit.Location = new System.Drawing.Point(84, 120);
            this.bttCommit.Name = "bttCommit";
            this.bttCommit.Size = new System.Drawing.Size(75, 23);
            this.bttCommit.TabIndex = 2;
            this.bttCommit.Text = "提交";
            this.bttCommit.Click += new System.EventHandler(this.bttCommit_Click);
            // 
            // bttCancel
            // 
            this.bttCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttCancel.Location = new System.Drawing.Point(169, 120);
            this.bttCancel.Name = "bttCancel";
            this.bttCancel.Size = new System.Drawing.Size(75, 23);
            this.bttCancel.TabIndex = 2;
            this.bttCancel.Text = "取消";
            this.bttCancel.Click += new System.EventHandler(this.bttCancel_Click);
            // 
            // errModifyPassword
            // 
            this.errModifyPassword.ContainerControl = this;
            // 
            // frmModifyPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 155);
            this.Controls.Add(this.bttCancel);
            this.Controls.Add(this.bttCommit);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtOldPassword);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmModifyPassword";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改密码";
            ((System.ComponentModel.ISupportInitialize)(this.errModifyPassword)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtOldPassword;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassword;
        private DevComponents.DotNetBar.Controls.TextBoxX txtNewPassword;
        private DevComponents.DotNetBar.ButtonX bttCommit;
        private DevComponents.DotNetBar.ButtonX bttCancel;
        private System.Windows.Forms.ErrorProvider errModifyPassword;
    }
}