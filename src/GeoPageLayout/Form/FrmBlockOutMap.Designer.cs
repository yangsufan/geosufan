namespace GeoPageLayout
{
    partial class FrmBlockOutMap
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
            this.txtPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.bttOK = new DevComponents.DotNetBar.ButtonX();
            this.bttImport = new DevComponents.DotNetBar.ButtonX();
            this.btnOutPath = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtOutPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cboPapers = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            // 
            // 
            // 
            this.txtPath.Border.Class = "TextBoxBorder";
            this.txtPath.Location = new System.Drawing.Point(101, 15);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(191, 21);
            this.txtPath.TabIndex = 0;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(7, 15);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(94, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "导入地块范围：";
            // 
            // bttOK
            // 
            this.bttOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttOK.Location = new System.Drawing.Point(219, 118);
            this.bttOK.Name = "bttOK";
            this.bttOK.Size = new System.Drawing.Size(75, 23);
            this.bttOK.TabIndex = 2;
            this.bttOK.Text = "确定";
            this.bttOK.Click += new System.EventHandler(this.bttOK_Click);
            // 
            // bttImport
            // 
            this.bttImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttImport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttImport.Location = new System.Drawing.Point(300, 14);
            this.bttImport.Name = "bttImport";
            this.bttImport.Size = new System.Drawing.Size(75, 23);
            this.bttImport.TabIndex = 3;
            this.bttImport.Text = "浏览";
            this.bttImport.Click += new System.EventHandler(this.bttImport_Click);
            // 
            // btnOutPath
            // 
            this.btnOutPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOutPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOutPath.Location = new System.Drawing.Point(300, 43);
            this.btnOutPath.Name = "btnOutPath";
            this.btnOutPath.Size = new System.Drawing.Size(75, 23);
            this.btnOutPath.TabIndex = 6;
            this.btnOutPath.Text = "浏览";
            this.btnOutPath.Click += new System.EventHandler(this.btnOutPath_Click);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(7, 44);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(94, 23);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "输出到文件夹：";
            // 
            // txtOutPath
            // 
            // 
            // 
            // 
            this.txtOutPath.Border.Class = "TextBoxBorder";
            this.txtOutPath.Location = new System.Drawing.Point(101, 44);
            this.txtOutPath.Name = "txtOutPath";
            this.txtOutPath.ReadOnly = true;
            this.txtOutPath.Size = new System.Drawing.Size(191, 21);
            this.txtOutPath.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(300, 118);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(7, 73);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(94, 23);
            this.labelX3.TabIndex = 8;
            this.labelX3.Text = "纸张大小：";
            // 
            // cboPapers
            // 
            this.cboPapers.DisplayMember = "Text";
            this.cboPapers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboPapers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPapers.FormattingEnabled = true;
            this.cboPapers.ItemHeight = 15;
            this.cboPapers.Location = new System.Drawing.Point(101, 73);
            this.cboPapers.Name = "cboPapers";
            this.cboPapers.Size = new System.Drawing.Size(191, 21);
            this.cboPapers.TabIndex = 9;
            // 
            // FrmBlockOutMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 153);
            this.Controls.Add(this.cboPapers);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOutPath);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.txtOutPath);
            this.Controls.Add(this.bttImport);
            this.Controls.Add(this.bttOK);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.txtPath);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBlockOutMap";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量范围专题图";
            this.Load += new System.EventHandler(this.FrmBlockOutMap_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtPath;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX bttOK;
        private DevComponents.DotNetBar.ButtonX bttImport;
        private DevComponents.DotNetBar.ButtonX btnOutPath;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtOutPath;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboPapers;
    }
}