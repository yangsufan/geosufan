namespace GeoPageLayout
{
    partial class FrmExportActiveViewSet
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
            this.txtFileName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtResolution = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cBoxRatio = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.btnSaveDl = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelRatio = new DevComponents.DotNetBar.LabelX();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            // 
            // 
            // 
            this.txtFileName.Border.Class = "TextBoxBorder";
            this.txtFileName.Location = new System.Drawing.Point(42, 12);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(235, 21);
            this.txtFileName.TabIndex = 0;
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            // 
            // txtResolution
            // 
            // 
            // 
            // 
            this.txtResolution.Border.Class = "TextBoxBorder";
            this.txtResolution.Location = new System.Drawing.Point(42, 39);
            this.txtResolution.Name = "txtResolution";
            this.txtResolution.Size = new System.Drawing.Size(76, 21);
            this.txtResolution.TabIndex = 1;
            this.txtResolution.Text = "300";
            // 
            // cBoxRatio
            // 
            this.cBoxRatio.DisplayMember = "Text";
            this.cBoxRatio.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBoxRatio.FormattingEnabled = true;
            this.cBoxRatio.ItemHeight = 15;
            this.cBoxRatio.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3,
            this.comboItem4,
            this.comboItem5});
            this.cBoxRatio.Location = new System.Drawing.Point(42, 66);
            this.cBoxRatio.Name = "cBoxRatio";
            this.cBoxRatio.Size = new System.Drawing.Size(76, 21);
            this.cBoxRatio.TabIndex = 2;
            this.cBoxRatio.Text = "1";
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "1";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "2";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "3";
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "4";
            // 
            // comboItem5
            // 
            this.comboItem5.Text = "5";
            // 
            // btnSaveDl
            // 
            this.btnSaveDl.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveDl.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveDl.Location = new System.Drawing.Point(283, 12);
            this.btnSaveDl.Name = "btnSaveDl";
            this.btnSaveDl.Size = new System.Drawing.Size(50, 23);
            this.btnSaveDl.TabIndex = 3;
            this.btnSaveDl.Text = "...";
            this.btnSaveDl.Click += new System.EventHandler(this.btnSaveDl_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(227, 66);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(50, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(283, 66);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(3, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelX1.Size = new System.Drawing.Size(42, 23);
            this.labelX1.TabIndex = 6;
            this.labelX1.Text = "保存为";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(3, 39);
            this.labelX2.Name = "labelX2";
            this.labelX2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelX2.Size = new System.Drawing.Size(42, 23);
            this.labelX2.TabIndex = 7;
            this.labelX2.Text = "分辨率";
            // 
            // labelRatio
            // 
            this.labelRatio.Location = new System.Drawing.Point(3, 64);
            this.labelRatio.Name = "labelRatio";
            this.labelRatio.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelRatio.Size = new System.Drawing.Size(42, 23);
            this.labelRatio.TabIndex = 8;
            this.labelRatio.Text = "比率";
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(115, 37);
            this.labelX4.Name = "labelX4";
            this.labelX4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 9;
            this.labelX4.Text = "dpi";
            // 
            // FrmExportActiveViewSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 108);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelRatio);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnSaveDl);
            this.Controls.Add(this.cBoxRatio);
            this.Controls.Add(this.txtResolution);
            this.Controls.Add(this.txtFileName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmExportActiveViewSet";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地图输出设置对话框";
            this.Load += new System.EventHandler(this.FrmExportActiveViewSet_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtFileName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtResolution;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxRatio;
        private DevComponents.DotNetBar.ButtonX btnSaveDl;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelRatio;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem5;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private DevComponents.DotNetBar.LabelX labelX4;

    }
}