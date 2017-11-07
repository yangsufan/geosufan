namespace GeoDatabaseManager
{
    partial class frmSelectLogin
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
            this.btnDataManage = new DevComponents.DotNetBar.ButtonX();
            this.btnDBIntegration = new DevComponents.DotNetBar.ButtonX();
            this.btnUpdata = new DevComponents.DotNetBar.ButtonX();
            this.buttonQuit = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // btnDataManage
            // 
            this.btnDataManage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDataManage.BackColor = System.Drawing.Color.Transparent;
            this.btnDataManage.ColorTable = DevComponents.DotNetBar.eButtonColor.Magenta;
            this.btnDataManage.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDataManage.ForeColor = System.Drawing.Color.Green;
            this.btnDataManage.Image = global::GeoDatabaseManager.Properties.Resources.btn1;
            this.btnDataManage.Location = new System.Drawing.Point(215, 196);
            this.btnDataManage.Name = "btnDataManage";
            this.btnDataManage.Size = new System.Drawing.Size(260, 47);
            this.btnDataManage.TabIndex = 0;
            this.btnDataManage.MouseLeave += new System.EventHandler(this.btnDataManage_MouseLeave);
            this.btnDataManage.Click += new System.EventHandler(this.btnDataManage_Click);
            this.btnDataManage.MouseEnter += new System.EventHandler(this.btnDataManage_MouseEnter);
            // 
            // btnDBIntegration
            // 
            this.btnDBIntegration.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDBIntegration.BackColor = System.Drawing.Color.Transparent;
            this.btnDBIntegration.ColorTable = DevComponents.DotNetBar.eButtonColor.Magenta;
            this.btnDBIntegration.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDBIntegration.ForeColor = System.Drawing.Color.Green;
            this.btnDBIntegration.Image = global::GeoDatabaseManager.Properties.Resources.btn2;
            this.btnDBIntegration.Location = new System.Drawing.Point(167, 282);
            this.btnDBIntegration.Name = "btnDBIntegration";
            this.btnDBIntegration.Size = new System.Drawing.Size(139, 42);
            this.btnDBIntegration.TabIndex = 0;
            this.btnDBIntegration.MouseLeave += new System.EventHandler(this.btnDBIntegration_MouseLeave);
            this.btnDBIntegration.Click += new System.EventHandler(this.btnDBIntegration_Click);
            this.btnDBIntegration.MouseEnter += new System.EventHandler(this.btnDBIntegration_MouseEnter);
            // 
            // btnUpdata
            // 
            this.btnUpdata.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUpdata.BackColor = System.Drawing.Color.Transparent;
            this.btnUpdata.ColorTable = DevComponents.DotNetBar.eButtonColor.Magenta;
            this.btnUpdata.FocusCuesEnabled = false;
            this.btnUpdata.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdata.ForeColor = System.Drawing.Color.Green;
            this.btnUpdata.Image = global::GeoDatabaseManager.Properties.Resources.btn3;
            this.btnUpdata.Location = new System.Drawing.Point(388, 284);
            this.btnUpdata.Name = "btnUpdata";
            this.btnUpdata.Size = new System.Drawing.Size(139, 39);
            this.btnUpdata.TabIndex = 0;
            this.btnUpdata.MouseLeave += new System.EventHandler(this.btnUpdata_MouseLeave);
            this.btnUpdata.Click += new System.EventHandler(this.btnUpdata_Click);
            this.btnUpdata.MouseEnter += new System.EventHandler(this.btnUpdata_MouseEnter);
            // 
            // buttonQuit
            // 
            this.buttonQuit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonQuit.BackColor = System.Drawing.Color.SeaGreen;
            this.buttonQuit.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.buttonQuit.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonQuit.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.buttonQuit.Location = new System.Drawing.Point(576, 438);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(65, 26);
            this.buttonQuit.TabIndex = 7;
            this.buttonQuit.Text = "退  出";
            this.buttonQuit.Visible = false;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // frmSelectLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GeoDatabaseManager.Properties.Resources.bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(676, 476);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.btnUpdata);
            this.Controls.Add(this.btnDBIntegration);
            this.Controls.Add(this.btnDataManage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectLogin";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmSelectLogin_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnDataManage;
        private DevComponents.DotNetBar.ButtonX btnDBIntegration;
        private DevComponents.DotNetBar.ButtonX btnUpdata;
        private DevComponents.DotNetBar.ButtonX buttonQuit;
    }
}