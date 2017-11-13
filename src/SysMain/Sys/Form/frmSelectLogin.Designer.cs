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
            this.btnDataManage = new DevExpress.XtraEditors.SimpleButton();
            this.btnDBIntegration = new DevExpress.XtraEditors.SimpleButton();
            this.btnUpdata = new DevExpress.XtraEditors.SimpleButton();
            this.buttonQuit = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnDataManage
            // 
            this.btnDataManage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDataManage.Appearance.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDataManage.Appearance.ForeColor = System.Drawing.Color.Green;
            this.btnDataManage.Appearance.Options.UseFont = true;
            this.btnDataManage.Appearance.Options.UseForeColor = true;
            this.btnDataManage.ImageOptions.Image = global::GeoDatabaseManager.Properties.Resources.btn1;
            this.btnDataManage.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDataManage.Location = new System.Drawing.Point(251, 229);
            this.btnDataManage.Name = "btnDataManage";
            this.btnDataManage.Size = new System.Drawing.Size(352, 60);
            this.btnDataManage.TabIndex = 0;
            this.btnDataManage.Click += new System.EventHandler(this.btnDataManage_Click);
            this.btnDataManage.MouseEnter += new System.EventHandler(this.btnDataManage_MouseEnter);
            this.btnDataManage.MouseLeave += new System.EventHandler(this.btnDataManage_MouseLeave);
            // 
            // btnDBIntegration
            // 
            this.btnDBIntegration.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDBIntegration.Appearance.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDBIntegration.Appearance.ForeColor = System.Drawing.Color.Green;
            this.btnDBIntegration.Appearance.Options.UseFont = true;
            this.btnDBIntegration.Appearance.Options.UseForeColor = true;
            this.btnDBIntegration.ImageOptions.Image = global::GeoDatabaseManager.Properties.Resources.btn2;
            this.btnDBIntegration.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDBIntegration.Location = new System.Drawing.Point(195, 329);
            this.btnDBIntegration.Name = "btnDBIntegration";
            this.btnDBIntegration.Size = new System.Drawing.Size(189, 49);
            this.btnDBIntegration.TabIndex = 0;
            this.btnDBIntegration.Click += new System.EventHandler(this.btnDBIntegration_Click);
            this.btnDBIntegration.MouseEnter += new System.EventHandler(this.btnDBIntegration_MouseEnter);
            this.btnDBIntegration.MouseLeave += new System.EventHandler(this.btnDBIntegration_MouseLeave);
            // 
            // btnUpdata
            // 
            this.btnUpdata.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUpdata.Appearance.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdata.Appearance.ForeColor = System.Drawing.Color.Green;
            this.btnUpdata.Appearance.Options.UseFont = true;
            this.btnUpdata.Appearance.Options.UseForeColor = true;
            this.btnUpdata.ImageOptions.Image = global::GeoDatabaseManager.Properties.Resources.btn3;
            this.btnUpdata.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUpdata.Location = new System.Drawing.Point(450, 329);
            this.btnUpdata.Name = "btnUpdata";
            this.btnUpdata.Size = new System.Drawing.Size(188, 47);
            this.btnUpdata.TabIndex = 0;
            this.btnUpdata.Click += new System.EventHandler(this.btnUpdata_Click);
            this.btnUpdata.MouseEnter += new System.EventHandler(this.btnUpdata_MouseEnter);
            this.btnUpdata.MouseLeave += new System.EventHandler(this.btnUpdata_MouseLeave);
            // 
            // buttonQuit
            // 
            this.buttonQuit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonQuit.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonQuit.Appearance.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.buttonQuit.Appearance.Options.UseFont = true;
            this.buttonQuit.Appearance.Options.UseForeColor = true;
            this.buttonQuit.Location = new System.Drawing.Point(672, 511);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(76, 30);
            this.buttonQuit.TabIndex = 7;
            this.buttonQuit.Text = "退  出";
            this.buttonQuit.Visible = false;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // frmSelectLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Stretch;
            this.BackgroundImageStore = global::GeoDatabaseManager.Properties.Resources.bg;
            this.ClientSize = new System.Drawing.Size(789, 555);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.btnUpdata);
            this.Controls.Add(this.btnDBIntegration);
            this.Controls.Add(this.btnDataManage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmSelectLogin_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnDataManage;
        private DevExpress.XtraEditors.SimpleButton btnDBIntegration;
        private DevExpress.XtraEditors.SimpleButton btnUpdata;
        private DevExpress.XtraEditors.SimpleButton buttonQuit;
    }
}