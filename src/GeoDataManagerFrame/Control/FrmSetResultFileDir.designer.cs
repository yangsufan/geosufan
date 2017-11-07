namespace GeoDataManagerFrame
{
    partial class FrmSetResultFileDir
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
            this.textBox_XLS = new System.Windows.Forms.TextBox();
            this.textBox_Jpg = new System.Windows.Forms.TextBox();
            this.buttonXQuit = new DevComponents.DotNetBar.ButtonX();
            this.buttonXOK = new DevComponents.DotNetBar.ButtonX();
            this.btn_openXlsDir = new DevComponents.DotNetBar.ButtonX();
            this.btn_openJpgDir = new DevComponents.DotNetBar.ButtonX();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_XLS
            // 
            this.textBox_XLS.Location = new System.Drawing.Point(53, 42);
            this.textBox_XLS.Name = "textBox_XLS";
            this.textBox_XLS.ReadOnly = true;
            this.textBox_XLS.Size = new System.Drawing.Size(304, 21);
            this.textBox_XLS.TabIndex = 4;
            // 
            // textBox_Jpg
            // 
            this.textBox_Jpg.Location = new System.Drawing.Point(53, 97);
            this.textBox_Jpg.Name = "textBox_Jpg";
            this.textBox_Jpg.ReadOnly = true;
            this.textBox_Jpg.Size = new System.Drawing.Size(304, 21);
            this.textBox_Jpg.TabIndex = 5;
            // 
            // buttonXQuit
            // 
            this.buttonXQuit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXQuit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXQuit.Location = new System.Drawing.Point(364, 138);
            this.buttonXQuit.Name = "buttonXQuit";
            this.buttonXQuit.Size = new System.Drawing.Size(70, 23);
            this.buttonXQuit.TabIndex = 9;
            this.buttonXQuit.Text = "退出";
            this.buttonXQuit.Click += new System.EventHandler(this.buttonXQuit_Click);
            // 
            // buttonXOK
            // 
            this.buttonXOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXOK.Location = new System.Drawing.Point(271, 138);
            this.buttonXOK.Name = "buttonXOK";
            this.buttonXOK.Size = new System.Drawing.Size(70, 23);
            this.buttonXOK.TabIndex = 8;
            this.buttonXOK.Text = "确定";
            this.buttonXOK.Click += new System.EventHandler(this.buttonXOK_Click);
            // 
            // btn_openXlsDir
            // 
            this.btn_openXlsDir.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_openXlsDir.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_openXlsDir.Location = new System.Drawing.Point(363, 40);
            this.btn_openXlsDir.Name = "btn_openXlsDir";
            this.btn_openXlsDir.Size = new System.Drawing.Size(70, 23);
            this.btn_openXlsDir.TabIndex = 8;
            this.btn_openXlsDir.Text = "打开";
            this.btn_openXlsDir.Click += new System.EventHandler(this.btn_openXlsDir_Click);
            // 
            // btn_openJpgDir
            // 
            this.btn_openJpgDir.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_openJpgDir.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_openJpgDir.Location = new System.Drawing.Point(363, 95);
            this.btn_openJpgDir.Name = "btn_openJpgDir";
            this.btn_openJpgDir.Size = new System.Drawing.Size(70, 23);
            this.btn_openJpgDir.TabIndex = 9;
            this.btn_openJpgDir.Text = "打开";
            this.btn_openJpgDir.Click += new System.EventHandler(this.btn_openJpgDir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "文档成果路径:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "图件成果路径:";
            // 
            // FrmSetResultFileDir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 173);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_openJpgDir);
            this.Controls.Add(this.buttonXQuit);
            this.Controls.Add(this.btn_openXlsDir);
            this.Controls.Add(this.buttonXOK);
            this.Controls.Add(this.textBox_Jpg);
            this.Controls.Add(this.textBox_XLS);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSetResultFileDir";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置成果文件路径";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_XLS;
        private System.Windows.Forms.TextBox textBox_Jpg;
        private DevComponents.DotNetBar.ButtonX buttonXQuit;
        private DevComponents.DotNetBar.ButtonX buttonXOK;
        private DevComponents.DotNetBar.ButtonX btn_openXlsDir;
        private DevComponents.DotNetBar.ButtonX btn_openJpgDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}