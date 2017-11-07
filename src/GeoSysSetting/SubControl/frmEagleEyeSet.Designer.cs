namespace GeoSysSetting
{
    partial class frmEagleEyeSet
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.bttOpen = new DevComponents.DotNetBar.ButtonX();
            this.bttImport = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(11, 15);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(81, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "鹰眼图位置：";
            // 
            // txtPath
            // 
            // 
            // 
            // 
            this.txtPath.Border.Class = "TextBoxBorder";
            this.txtPath.Location = new System.Drawing.Point(88, 15);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(249, 21);
            this.txtPath.TabIndex = 1;
            // 
            // bttOpen
            // 
            this.bttOpen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttOpen.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttOpen.Location = new System.Drawing.Point(351, 15);
            this.bttOpen.Name = "bttOpen";
            this.bttOpen.Size = new System.Drawing.Size(75, 23);
            this.bttOpen.TabIndex = 2;
            this.bttOpen.Text = "浏览";
            this.bttOpen.Click += new System.EventHandler(this.bttOpen_Click);
            // 
            // bttImport
            // 
            this.bttImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttImport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttImport.Location = new System.Drawing.Point(351, 55);
            this.bttImport.Name = "bttImport";
            this.bttImport.Size = new System.Drawing.Size(75, 23);
            this.bttImport.TabIndex = 3;
            this.bttImport.Text = "导入";
            this.bttImport.Click += new System.EventHandler(this.bttImport_Click);
            // 
            // frmEagleEyeSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 89);
            this.Controls.Add(this.bttImport);
            this.Controls.Add(this.bttOpen);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.labelX1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEagleEyeSet";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "鹰眼图导入";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPath;
        private DevComponents.DotNetBar.ButtonX bttOpen;
        private DevComponents.DotNetBar.ButtonX bttImport;
    }
}