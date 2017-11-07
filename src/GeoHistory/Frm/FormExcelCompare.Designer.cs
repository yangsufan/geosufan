namespace GeoHistory
{
    partial class FormExcelCompare
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
            this.cmbType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtBoxOldExcel = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSelectOld = new DevComponents.DotNetBar.ButtonX();
            this.txtBoxNewExcel = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSelectNew = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.txtBoxResPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnRes = new DevComponents.DotNetBar.ButtonX();
            this.lblTips = new DevComponents.DotNetBar.LabelX();
            this.progressBarX1 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(63, 20);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "报表类型:";
            // 
            // cmbType
            // 
            this.cmbType.DisplayMember = "Text";
            this.cmbType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.ItemHeight = 15;
            this.cmbType.Location = new System.Drawing.Point(72, 11);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(191, 21);
            this.cmbType.TabIndex = 1;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(12, 41);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(63, 20);
            this.labelX2.TabIndex = 0;
            this.labelX2.Text = "报表(旧):";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(12, 68);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(63, 20);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "报表(新):";
            // 
            // txtBoxOldExcel
            // 
            // 
            // 
            // 
            this.txtBoxOldExcel.Border.Class = "TextBoxBorder";
            this.txtBoxOldExcel.Enabled = false;
            this.txtBoxOldExcel.Location = new System.Drawing.Point(72, 41);
            this.txtBoxOldExcel.Name = "txtBoxOldExcel";
            this.txtBoxOldExcel.Size = new System.Drawing.Size(155, 21);
            this.txtBoxOldExcel.TabIndex = 2;
            // 
            // btnSelectOld
            // 
            this.btnSelectOld.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectOld.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelectOld.Location = new System.Drawing.Point(233, 41);
            this.btnSelectOld.Name = "btnSelectOld";
            this.btnSelectOld.Size = new System.Drawing.Size(30, 20);
            this.btnSelectOld.TabIndex = 3;
            this.btnSelectOld.Text = "...";
            this.btnSelectOld.Click += new System.EventHandler(this.btnSelectOld_Click);
            // 
            // txtBoxNewExcel
            // 
            // 
            // 
            // 
            this.txtBoxNewExcel.Border.Class = "TextBoxBorder";
            this.txtBoxNewExcel.Enabled = false;
            this.txtBoxNewExcel.Location = new System.Drawing.Point(72, 68);
            this.txtBoxNewExcel.Name = "txtBoxNewExcel";
            this.txtBoxNewExcel.Size = new System.Drawing.Size(155, 21);
            this.txtBoxNewExcel.TabIndex = 2;
            // 
            // btnSelectNew
            // 
            this.btnSelectNew.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectNew.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelectNew.Location = new System.Drawing.Point(233, 68);
            this.btnSelectNew.Name = "btnSelectNew";
            this.btnSelectNew.Size = new System.Drawing.Size(30, 20);
            this.btnSelectNew.TabIndex = 3;
            this.btnSelectNew.Text = "...";
            this.btnSelectNew.Click += new System.EventHandler(this.btnSelectNew_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(61, 156);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(62, 22);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(149, 156);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 22);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(12, 96);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(63, 20);
            this.labelX4.TabIndex = 0;
            this.labelX4.Text = "对比结果:";
            // 
            // txtBoxResPath
            // 
            // 
            // 
            // 
            this.txtBoxResPath.Border.Class = "TextBoxBorder";
            this.txtBoxResPath.Enabled = false;
            this.txtBoxResPath.Location = new System.Drawing.Point(72, 95);
            this.txtBoxResPath.Name = "txtBoxResPath";
            this.txtBoxResPath.Size = new System.Drawing.Size(155, 21);
            this.txtBoxResPath.TabIndex = 2;
            // 
            // btnRes
            // 
            this.btnRes.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRes.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRes.Location = new System.Drawing.Point(233, 95);
            this.btnRes.Name = "btnRes";
            this.btnRes.Size = new System.Drawing.Size(30, 20);
            this.btnRes.TabIndex = 3;
            this.btnRes.Text = "...";
            this.btnRes.Click += new System.EventHandler(this.btnRes_Click);
            // 
            // lblTips
            // 
            this.lblTips.Location = new System.Drawing.Point(3, 130);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(246, 22);
            this.lblTips.TabIndex = 68;
            // 
            // progressBarX1
            // 
            this.progressBarX1.Location = new System.Drawing.Point(3, 121);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.Size = new System.Drawing.Size(268, 10);
            this.progressBarX1.TabIndex = 67;
            this.progressBarX1.Text = "proBar";
            this.progressBarX1.Visible = false;
            // 
            // FormExcelCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 180);
            this.Controls.Add(this.lblTips);
            this.Controls.Add(this.progressBarX1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRes);
            this.Controls.Add(this.btnSelectNew);
            this.Controls.Add(this.btnSelectOld);
            this.Controls.Add(this.txtBoxResPath);
            this.Controls.Add(this.txtBoxNewExcel);
            this.Controls.Add(this.txtBoxOldExcel);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormExcelCompare";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "报表对比";
            this.Load += new System.EventHandler(this.FormExcelCompare_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbType;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxOldExcel;
        private DevComponents.DotNetBar.ButtonX btnSelectOld;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxNewExcel;
        private DevComponents.DotNetBar.ButtonX btnSelectNew;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxResPath;
        private DevComponents.DotNetBar.ButtonX btnRes;
        private DevComponents.DotNetBar.LabelX lblTips;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
    }
}