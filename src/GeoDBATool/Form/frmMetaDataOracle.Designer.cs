namespace GeoDBATool
{
    partial class frmMetaDataOracle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMetaDataOracle));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.checkedMDData = new System.Windows.Forms.CheckedListBox();
            this.bttAllRemove = new System.Windows.Forms.Button();
            this.bttRemove = new System.Windows.Forms.Button();
            this.bttOpenFolder = new System.Windows.Forms.Button();
            this.bttOpenFile = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOtherSelected = new System.Windows.Forms.Button();
            this.btnAllSelected = new System.Windows.Forms.Button();
            this.panelEx1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.Controls.Add(this.groupBox1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(400, 409);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.StyleMouseDown.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.StyleMouseDown.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.panelEx1.StyleMouseDown.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.panelEx1.StyleMouseDown.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBorder;
            this.panelEx1.StyleMouseDown.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedText;
            this.panelEx1.StyleMouseOver.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.StyleMouseOver.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground;
            this.panelEx1.StyleMouseOver.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground2;
            this.panelEx1.StyleMouseOver.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBorder;
            this.panelEx1.StyleMouseOver.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotText;
            this.panelEx1.TabIndex = 16;
            this.panelEx1.Text = "panelEx1";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.labelX1);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.checkedMDData);
            this.groupBox1.Controls.Add(this.bttAllRemove);
            this.groupBox1.Controls.Add(this.bttRemove);
            this.groupBox1.Controls.Add(this.bttOpenFolder);
            this.groupBox1.Controls.Add(this.bttOpenFile);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnOtherSelected);
            this.groupBox1.Controls.Add(this.btnAllSelected);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 385);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(9, 11);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(111, 23);
            this.labelX1.TabIndex = 39;
            this.labelX1.Text = "输入元数据库信息";
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(3, 367);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(370, 15);
            this.progressBar1.TabIndex = 38;
            // 
            // checkedMDData
            // 
            this.checkedMDData.FormattingEnabled = true;
            this.checkedMDData.HorizontalScrollbar = true;
            this.checkedMDData.Location = new System.Drawing.Point(9, 38);
            this.checkedMDData.Name = "checkedMDData";
            this.checkedMDData.ScrollAlwaysVisible = true;
            this.checkedMDData.Size = new System.Drawing.Size(274, 308);
            this.checkedMDData.TabIndex = 20;
            // 
            // bttAllRemove
            // 
            this.bttAllRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bttAllRemove.Enabled = false;
            this.bttAllRemove.Location = new System.Drawing.Point(297, 185);
            this.bttAllRemove.Name = "bttAllRemove";
            this.bttAllRemove.Size = new System.Drawing.Size(69, 25);
            this.bttAllRemove.TabIndex = 18;
            this.bttAllRemove.Text = "全部移除";
            this.bttAllRemove.UseVisualStyleBackColor = true;
            this.bttAllRemove.Click += new System.EventHandler(this.bttAllRemove_Click);
            // 
            // bttRemove
            // 
            this.bttRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bttRemove.Enabled = false;
            this.bttRemove.Location = new System.Drawing.Point(297, 155);
            this.bttRemove.Name = "bttRemove";
            this.bttRemove.Size = new System.Drawing.Size(70, 25);
            this.bttRemove.TabIndex = 17;
            this.bttRemove.Text = "移除";
            this.bttRemove.UseVisualStyleBackColor = true;
            this.bttRemove.Click += new System.EventHandler(this.bttRemove_Click);
            // 
            // bttOpenFolder
            // 
            this.bttOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("bttOpenFolder.Image")));
            this.bttOpenFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bttOpenFolder.Location = new System.Drawing.Point(297, 67);
            this.bttOpenFolder.Name = "bttOpenFolder";
            this.bttOpenFolder.Size = new System.Drawing.Size(70, 25);
            this.bttOpenFolder.TabIndex = 16;
            this.bttOpenFolder.Text = "   文件夹";
            this.bttOpenFolder.UseVisualStyleBackColor = true;
            this.bttOpenFolder.Click += new System.EventHandler(this.bttOpenFolder_Click);
            // 
            // bttOpenFile
            // 
            this.bttOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("bttOpenFile.Image")));
            this.bttOpenFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bttOpenFile.Location = new System.Drawing.Point(297, 36);
            this.bttOpenFile.Name = "bttOpenFile";
            this.bttOpenFile.Size = new System.Drawing.Size(70, 25);
            this.bttOpenFile.TabIndex = 15;
            this.bttOpenFile.Text = "   文件";
            this.bttOpenFile.UseVisualStyleBackColor = true;
            this.bttOpenFile.Click += new System.EventHandler(this.bttOpenFile_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(299, 326);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(64, 30);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "确 定";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(299, 290);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 30);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取 消";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOtherSelected
            // 
            this.btnOtherSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOtherSelected.Location = new System.Drawing.Point(297, 126);
            this.btnOtherSelected.Name = "btnOtherSelected";
            this.btnOtherSelected.Size = new System.Drawing.Size(70, 25);
            this.btnOtherSelected.TabIndex = 3;
            this.btnOtherSelected.Text = "反选";
            this.btnOtherSelected.UseVisualStyleBackColor = true;
            this.btnOtherSelected.Click += new System.EventHandler(this.btnOtherSelected_Click);
            // 
            // btnAllSelected
            // 
            this.btnAllSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAllSelected.Location = new System.Drawing.Point(297, 96);
            this.btnAllSelected.Name = "btnAllSelected";
            this.btnAllSelected.Size = new System.Drawing.Size(70, 25);
            this.btnAllSelected.TabIndex = 2;
            this.btnAllSelected.Text = "全选";
            this.btnAllSelected.UseVisualStyleBackColor = true;
            this.btnAllSelected.Click += new System.EventHandler(this.btnAllSelected_Click);
            // 
            // frmMetaDataOracle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 409);
            this.Controls.Add(this.panelEx1);
            this.MaximizeBox = false;
            this.Name = "frmMetaDataOracle";
            this.ShowIcon = false;
            this.Text = "元数据入库";
            this.Load += new System.EventHandler(this.frmMetaDataOracle_Load);
            this.panelEx1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckedListBox checkedMDData;
        private System.Windows.Forms.Button bttAllRemove;
        private System.Windows.Forms.Button bttRemove;
        private System.Windows.Forms.Button bttOpenFolder;
        private System.Windows.Forms.Button bttOpenFile;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOtherSelected;
        private System.Windows.Forms.Button btnAllSelected;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}