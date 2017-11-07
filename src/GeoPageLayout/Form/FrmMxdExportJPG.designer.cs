namespace GeoPageLayout
{
    partial class FrmMxdExportJPG
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
            this.checkedMDData = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.UpDownResolution = new System.Windows.Forms.NumericUpDown();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnOtherSelected = new DevComponents.DotNetBar.ButtonX();
            this.btnAllSelected = new DevComponents.DotNetBar.ButtonX();
            this.bttAllRemove = new DevComponents.DotNetBar.ButtonX();
            this.bttRemove = new DevComponents.DotNetBar.ButtonX();
            this.bttOpenFolder = new DevComponents.DotNetBar.ButtonX();
            this.bttOpenFile = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSaveDl = new DevComponents.DotNetBar.ButtonX();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownResolution)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedMDData
            // 
            this.checkedMDData.FormattingEnabled = true;
            this.checkedMDData.HorizontalScrollbar = true;
            this.checkedMDData.Location = new System.Drawing.Point(3, 3);
            this.checkedMDData.Name = "checkedMDData";
            this.checkedMDData.ScrollAlwaysVisible = true;
            this.checkedMDData.Size = new System.Drawing.Size(316, 276);
            this.checkedMDData.TabIndex = 20;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 77.03704F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.96296F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(414, 405);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.btnSaveDl);
            this.groupPanel2.Controls.Add(this.txtPath);
            this.groupPanel2.Controls.Add(this.labelX2);
            this.groupPanel2.Controls.Add(this.UpDownResolution);
            this.groupPanel2.Controls.Add(this.labelX1);
            this.groupPanel2.Controls.Add(this.btnCancel);
            this.groupPanel2.Controls.Add(this.btnOK);
            this.groupPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(3, 315);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(408, 87);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 3;
            this.groupPanel2.Text = "输出设置";
            // 
            // UpDownResolution
            // 
            this.UpDownResolution.Location = new System.Drawing.Point(80, 33);
            this.UpDownResolution.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.UpDownResolution.Name = "UpDownResolution";
            this.UpDownResolution.Size = new System.Drawing.Size(59, 21);
            this.UpDownResolution.TabIndex = 29;
            this.UpDownResolution.Value = new decimal(new int[] {
            96,
            0,
            0,
            0});
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(23, 38);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 23);
            this.labelX1.TabIndex = 28;
            this.labelX1.Text = "分辨率：";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(325, 35);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(254, 33);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 23);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btnOtherSelected);
            this.groupPanel1.Controls.Add(this.btnAllSelected);
            this.groupPanel1.Controls.Add(this.bttAllRemove);
            this.groupPanel1.Controls.Add(this.bttRemove);
            this.groupPanel1.Controls.Add(this.bttOpenFolder);
            this.groupPanel1.Controls.Add(this.bttOpenFile);
            this.groupPanel1.Controls.Add(this.checkedMDData);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(3, 3);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(408, 306);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 1;
            this.groupPanel1.Text = "数据源设置";
            // 
            // btnOtherSelected
            // 
            this.btnOtherSelected.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOtherSelected.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOtherSelected.Location = new System.Drawing.Point(325, 172);
            this.btnOtherSelected.Name = "btnOtherSelected";
            this.btnOtherSelected.Size = new System.Drawing.Size(65, 23);
            this.btnOtherSelected.TabIndex = 26;
            this.btnOtherSelected.Text = "反选";
            this.btnOtherSelected.Click += new System.EventHandler(this.btnOtherSelected_Click);
            // 
            // btnAllSelected
            // 
            this.btnAllSelected.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAllSelected.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAllSelected.Location = new System.Drawing.Point(325, 143);
            this.btnAllSelected.Name = "btnAllSelected";
            this.btnAllSelected.Size = new System.Drawing.Size(65, 23);
            this.btnAllSelected.TabIndex = 25;
            this.btnAllSelected.Text = "全选";
            // 
            // bttAllRemove
            // 
            this.bttAllRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttAllRemove.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttAllRemove.Location = new System.Drawing.Point(325, 102);
            this.bttAllRemove.Name = "bttAllRemove";
            this.bttAllRemove.Size = new System.Drawing.Size(65, 23);
            this.bttAllRemove.TabIndex = 24;
            this.bttAllRemove.Text = "全部移除";
            this.bttAllRemove.Click += new System.EventHandler(this.bttAllRemove_Click);
            // 
            // bttRemove
            // 
            this.bttRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttRemove.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttRemove.Location = new System.Drawing.Point(325, 73);
            this.bttRemove.Name = "bttRemove";
            this.bttRemove.Size = new System.Drawing.Size(65, 23);
            this.bttRemove.TabIndex = 23;
            this.bttRemove.Text = "移除";
            this.bttRemove.Click += new System.EventHandler(this.bttRemove_Click);
            // 
            // bttOpenFolder
            // 
            this.bttOpenFolder.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttOpenFolder.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttOpenFolder.Location = new System.Drawing.Point(325, 32);
            this.bttOpenFolder.Name = "bttOpenFolder";
            this.bttOpenFolder.Size = new System.Drawing.Size(65, 23);
            this.bttOpenFolder.TabIndex = 22;
            this.bttOpenFolder.Text = "文件夹";
            this.bttOpenFolder.Click += new System.EventHandler(this.bttOpenFolder_Click);
            // 
            // bttOpenFile
            // 
            this.bttOpenFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttOpenFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttOpenFile.Location = new System.Drawing.Point(325, 3);
            this.bttOpenFile.Name = "bttOpenFile";
            this.bttOpenFile.Size = new System.Drawing.Size(65, 23);
            this.bttOpenFile.TabIndex = 21;
            this.bttOpenFile.Text = "文件";
            this.bttOpenFile.Click += new System.EventHandler(this.bttOpenFile_Click);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(10, 8);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(73, 23);
            this.labelX2.TabIndex = 30;
            this.labelX2.Text = "输出路径：";
            // 
            // txtPath
            // 
            // 
            // 
            // 
            this.txtPath.Border.Class = "TextBoxBorder";
            this.txtPath.Location = new System.Drawing.Point(80, 4);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(239, 21);
            this.txtPath.TabIndex = 31;
            // 
            // btnSaveDl
            // 
            this.btnSaveDl.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveDl.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveDl.Location = new System.Drawing.Point(325, 3);
            this.btnSaveDl.Name = "btnSaveDl";
            this.btnSaveDl.Size = new System.Drawing.Size(40, 23);
            this.btnSaveDl.TabIndex = 32;
            this.btnSaveDl.Text = "...";
            this.btnSaveDl.Click += new System.EventHandler(this.btnSaveDl_Click);
            // 
            // FrmMxdExportJPG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 405);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "FrmMxdExportJPG";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mxd导出JPG";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UpDownResolution)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedMDData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX bttOpenFile;
        private DevComponents.DotNetBar.ButtonX btnOtherSelected;
        private DevComponents.DotNetBar.ButtonX btnAllSelected;
        private DevComponents.DotNetBar.ButtonX bttAllRemove;
        private DevComponents.DotNetBar.ButtonX bttRemove;
        private DevComponents.DotNetBar.ButtonX bttOpenFolder;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.NumericUpDown UpDownResolution;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPath;
        private DevComponents.DotNetBar.ButtonX btnSaveDl;
    }
}