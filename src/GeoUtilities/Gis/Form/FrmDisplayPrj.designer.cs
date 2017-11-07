namespace GeoUtilities
{
    partial class FrmDisplayPrj
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
            this.rTextPrj = new System.Windows.Forms.RichTextBox();
            this.btnPrjPath = new DevComponents.DotNetBar.ButtonX();
            this.txtPrjPath = new System.Windows.Forms.TextBox();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelM = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rTextPrj
            // 
            this.rTextPrj.Location = new System.Drawing.Point(12, 58);
            this.rTextPrj.Name = "rTextPrj";
            this.rTextPrj.ReadOnly = true;
            this.rTextPrj.Size = new System.Drawing.Size(384, 268);
            this.rTextPrj.TabIndex = 0;
            this.rTextPrj.Text = "";
            // 
            // btnPrjPath
            // 
            this.btnPrjPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrjPath.Location = new System.Drawing.Point(341, 331);
            this.btnPrjPath.Name = "btnPrjPath";
            this.btnPrjPath.Size = new System.Drawing.Size(55, 23);
            this.btnPrjPath.TabIndex = 8;
            this.btnPrjPath.Text = "选择...";
            this.btnPrjPath.Click += new System.EventHandler(this.btnPrjPath_Click);
            // 
            // txtPrjPath
            // 
            this.txtPrjPath.Location = new System.Drawing.Point(12, 332);
            this.txtPrjPath.Name = "txtPrjPath";
            this.txtPrjPath.ReadOnly = true;
            this.txtPrjPath.Size = new System.Drawing.Size(325, 21);
            this.txtPrjPath.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(341, 363);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(279, 363);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(55, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnOK.Leave += new System.EventHandler(this.btnOK_Leave);
            // 
            // txtName
            // 
            this.txtName.Enabled = false;
            this.txtName.Location = new System.Drawing.Point(53, 12);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(335, 21);
            this.txtName.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "名称:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "详细信息:";
            // 
            // labelM
            // 
            this.labelM.AutoSize = true;
            this.labelM.Location = new System.Drawing.Point(12, 373);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(0, 12);
            this.labelM.TabIndex = 14;
            // 
            // FrmDisplayPrj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 394);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnPrjPath);
            this.Controls.Add(this.txtPrjPath);
            this.Controls.Add(this.rTextPrj);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDisplayPrj";
            this.ShowIcon = false;
            this.Text = "空间参考";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rTextPrj;
        private DevComponents.DotNetBar.ButtonX btnPrjPath;
        private System.Windows.Forms.TextBox txtPrjPath;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelM;
    }
}