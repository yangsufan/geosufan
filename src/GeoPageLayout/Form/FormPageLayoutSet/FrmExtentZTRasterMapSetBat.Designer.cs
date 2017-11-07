namespace GeoPageLayout
{
    partial class FrmExtentZTRasterMapSetBat
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
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnSetLabel = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cBoxZT = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cBoxHasBootLine = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(89, 57);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(170, 57);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSetLabel
            // 
            this.btnSetLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSetLabel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSetLabel.Location = new System.Drawing.Point(3, 57);
            this.btnSetLabel.Name = "btnSetLabel";
            this.btnSetLabel.Size = new System.Drawing.Size(75, 23);
            this.btnSetLabel.TabIndex = 46;
            this.btnSetLabel.Text = "设置标注";
            this.btnSetLabel.Click += new System.EventHandler(this.btnSetLabel_Click);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(3, 10);
            this.labelX1.Name = "labelX1";
            this.labelX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelX1.Size = new System.Drawing.Size(42, 23);
            this.labelX1.TabIndex = 48;
            this.labelX1.Text = "专  题";
            // 
            // cBoxZT
            // 
            this.cBoxZT.DisplayMember = "Text";
            this.cBoxZT.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBoxZT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxZT.FormattingEnabled = true;
            this.cBoxZT.ItemHeight = 15;
            this.cBoxZT.Location = new System.Drawing.Point(51, 12);
            this.cBoxZT.Name = "cBoxZT";
            this.cBoxZT.Size = new System.Drawing.Size(194, 21);
            this.cBoxZT.TabIndex = 47;
            // 
            // cBoxHasBootLine
            // 
            this.cBoxHasBootLine.AutoSize = true;
            this.cBoxHasBootLine.BackColor = System.Drawing.Color.Transparent;
            this.cBoxHasBootLine.Checked = true;
            this.cBoxHasBootLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cBoxHasBootLine.Location = new System.Drawing.Point(3, 39);
            this.cBoxHasBootLine.Name = "cBoxHasBootLine";
            this.cBoxHasBootLine.Size = new System.Drawing.Size(60, 16);
            this.cBoxHasBootLine.TabIndex = 51;
            this.cBoxHasBootLine.Text = "加引线";
            this.cBoxHasBootLine.UseVisualStyleBackColor = false;
            // 
            // FrmExtentZTRasterMapSetBat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 97);
            this.Controls.Add(this.cBoxHasBootLine);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cBoxZT);
            this.Controls.Add(this.btnSetLabel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmExtentZTRasterMapSetBat";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "范围项目专题图栅格";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnSetLabel;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxZT;
        private System.Windows.Forms.CheckBox cBoxHasBootLine;
    }
}