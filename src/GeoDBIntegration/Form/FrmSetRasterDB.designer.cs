namespace GeoDBIntegration
{
    partial class FrmSetRasterDB
    {
        /// <summary>
        /// 必需的设计器变量。        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。        /// </summary>
        private void InitializeComponent()
        {
            this.cmbRasterType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.pListViewDT = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnAddList = new DevComponents.DotNetBar.ButtonX();
            this.rbcatalog = new System.Windows.Forms.RadioButton();
            this.rbdataset = new System.Windows.Forms.RadioButton();
            this.cmbRasterSpaRef = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cmbGeoSpaRef = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelXErr = new DevComponents.DotNetBar.LabelX();
            this.labelX19 = new DevComponents.DotNetBar.LabelX();
            this.btnRuleFile = new DevComponents.DotNetBar.ButtonX();
            this.textRuleFilePath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelX20 = new DevComponents.DotNetBar.LabelX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.cmbRasterPixeType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.tileW = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.txtBand = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.tileH = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cmbResampleType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.cmbCompression = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.txtPyramid = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txt_RasterName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbRasterType
            // 
            this.cmbRasterType.DisplayMember = "Text";
            this.cmbRasterType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRasterType.FormattingEnabled = true;
            this.cmbRasterType.ItemHeight = 15;
            this.cmbRasterType.Location = new System.Drawing.Point(131, 474);
            this.cmbRasterType.Name = "cmbRasterType";
            this.cmbRasterType.Size = new System.Drawing.Size(220, 21);
            this.cmbRasterType.TabIndex = 0;
            this.cmbRasterType.SelectedIndexChanged += new System.EventHandler(this.cmbRasterType_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(34, 478);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(101, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "数 据 类 型：";
            // 
            // pListViewDT
            // 
            // 
            // 
            // 
            this.pListViewDT.Border.Class = "ListViewBorder";
            this.pListViewDT.Location = new System.Drawing.Point(9, 526);
            this.pListViewDT.Name = "pListViewDT";
            this.pListViewDT.Size = new System.Drawing.Size(328, 62);
            this.pListViewDT.TabIndex = 2;
            this.pListViewDT.UseCompatibleStateImageBehavior = false;
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(278, 324);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(53, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "完 成";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAddList
            // 
            this.btnAddList.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddList.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddList.Location = new System.Drawing.Point(292, 501);
            this.btnAddList.Name = "btnAddList";
            this.btnAddList.Size = new System.Drawing.Size(53, 23);
            this.btnAddList.TabIndex = 5;
            this.btnAddList.Text = "添 加";
            // 
            // rbcatalog
            // 
            this.rbcatalog.AutoSize = true;
            this.rbcatalog.Location = new System.Drawing.Point(119, 24);
            this.rbcatalog.Name = "rbcatalog";
            this.rbcatalog.Size = new System.Drawing.Size(71, 16);
            this.rbcatalog.TabIndex = 6;
            this.rbcatalog.TabStop = true;
            this.rbcatalog.Text = "栅格编目";
            this.rbcatalog.UseVisualStyleBackColor = true;
            this.rbcatalog.CheckedChanged += new System.EventHandler(this.rbcatalog_CheckedChanged);
            // 
            // rbdataset
            // 
            this.rbdataset.AutoSize = true;
            this.rbdataset.Location = new System.Drawing.Point(208, 24);
            this.rbdataset.Name = "rbdataset";
            this.rbdataset.Size = new System.Drawing.Size(83, 16);
            this.rbdataset.TabIndex = 7;
            this.rbdataset.TabStop = true;
            this.rbdataset.Text = "栅格数据集";
            this.rbdataset.UseVisualStyleBackColor = true;
            this.rbdataset.CheckedChanged += new System.EventHandler(this.rbdataset_CheckedChanged);
            // 
            // cmbRasterSpaRef
            // 
            this.cmbRasterSpaRef.DisplayMember = "Text";
            this.cmbRasterSpaRef.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRasterSpaRef.FormattingEnabled = true;
            this.cmbRasterSpaRef.ItemHeight = 15;
            this.cmbRasterSpaRef.Location = new System.Drawing.Point(355, 519);
            this.cmbRasterSpaRef.Name = "cmbRasterSpaRef";
            this.cmbRasterSpaRef.Size = new System.Drawing.Size(210, 21);
            this.cmbRasterSpaRef.TabIndex = 8;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(17, 20);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(90, 23);
            this.labelX3.TabIndex = 10;
            this.labelX3.Text = "存 储 类 型：";
            // 
            // cmbGeoSpaRef
            // 
            this.cmbGeoSpaRef.DisplayMember = "Text";
            this.cmbGeoSpaRef.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbGeoSpaRef.FormattingEnabled = true;
            this.cmbGeoSpaRef.ItemHeight = 15;
            this.cmbGeoSpaRef.Location = new System.Drawing.Point(354, 546);
            this.cmbGeoSpaRef.Name = "cmbGeoSpaRef";
            this.cmbGeoSpaRef.Size = new System.Drawing.Size(210, 21);
            this.cmbGeoSpaRef.TabIndex = 13;
            // 
            // labelXErr
            // 
            this.labelXErr.Location = new System.Drawing.Point(10, 353);
            this.labelXErr.Name = "labelXErr";
            this.labelXErr.Size = new System.Drawing.Size(321, 56);
            this.labelXErr.TabIndex = 16;
            // 
            // labelX19
            // 
            this.labelX19.Location = new System.Drawing.Point(368, 481);
            this.labelX19.Name = "labelX19";
            this.labelX19.Size = new System.Drawing.Size(94, 23);
            this.labelX19.TabIndex = 40;
            this.labelX19.Text = "库体配置文件：";
            // 
            // btnRuleFile
            // 
            this.btnRuleFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRuleFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRuleFile.Location = new System.Drawing.Point(650, 481);
            this.btnRuleFile.Name = "btnRuleFile";
            this.btnRuleFile.Size = new System.Drawing.Size(26, 21);
            this.btnRuleFile.TabIndex = 39;
            this.btnRuleFile.Text = "...";
            // 
            // textRuleFilePath
            // 
            // 
            // 
            // 
            this.textRuleFilePath.Border.Class = "TextBoxBorder";
            this.textRuleFilePath.Location = new System.Drawing.Point(465, 481);
            this.textRuleFilePath.Name = "textRuleFilePath";
            this.textRuleFilePath.Size = new System.Drawing.Size(185, 21);
            this.textRuleFilePath.TabIndex = 38;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelX20);
            this.groupBox2.Controls.Add(this.labelX12);
            this.groupBox2.Controls.Add(this.cmbRasterPixeType);
            this.groupBox2.Controls.Add(this.tileW);
            this.groupBox2.Controls.Add(this.labelX11);
            this.groupBox2.Controls.Add(this.txtBand);
            this.groupBox2.Controls.Add(this.labelX10);
            this.groupBox2.Controls.Add(this.tileH);
            this.groupBox2.Controls.Add(this.cmbResampleType);
            this.groupBox2.Controls.Add(this.labelX7);
            this.groupBox2.Controls.Add(this.cmbCompression);
            this.groupBox2.Controls.Add(this.labelX9);
            this.groupBox2.Controls.Add(this.txtPyramid);
            this.groupBox2.Controls.Add(this.labelX8);
            this.groupBox2.Location = new System.Drawing.Point(10, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(327, 226);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "栅格数据集参数设置";
            // 
            // labelX20
            // 
            this.labelX20.Location = new System.Drawing.Point(12, 198);
            this.labelX20.Name = "labelX20";
            this.labelX20.Size = new System.Drawing.Size(94, 23);
            this.labelX20.TabIndex = 45;
            this.labelX20.Text = "栅格像素类型：";
            // 
            // labelX12
            // 
            this.labelX12.Location = new System.Drawing.Point(11, 20);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(94, 23);
            this.labelX12.TabIndex = 0;
            this.labelX12.Text = "重采样类型：";
            // 
            // cmbRasterPixeType
            // 
            this.cmbRasterPixeType.DisplayMember = "Text";
            this.cmbRasterPixeType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRasterPixeType.FormattingEnabled = true;
            this.cmbRasterPixeType.ItemHeight = 15;
            this.cmbRasterPixeType.Location = new System.Drawing.Point(109, 198);
            this.cmbRasterPixeType.Name = "cmbRasterPixeType";
            this.cmbRasterPixeType.Size = new System.Drawing.Size(210, 21);
            this.cmbRasterPixeType.TabIndex = 46;
            // 
            // tileW
            // 
            // 
            // 
            // 
            this.tileW.Border.Class = "TextBoxBorder";
            this.tileW.Location = new System.Drawing.Point(109, 137);
            this.tileW.Name = "tileW";
            this.tileW.Size = new System.Drawing.Size(210, 21);
            this.tileW.TabIndex = 9;
            // 
            // labelX11
            // 
            this.labelX11.Location = new System.Drawing.Point(12, 50);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(75, 23);
            this.labelX11.TabIndex = 1;
            this.labelX11.Text = "压缩类型：";
            // 
            // txtBand
            // 
            // 
            // 
            // 
            this.txtBand.Border.Class = "TextBoxBorder";
            this.txtBand.Location = new System.Drawing.Point(109, 167);
            this.txtBand.Name = "txtBand";
            this.txtBand.Size = new System.Drawing.Size(210, 21);
            this.txtBand.TabIndex = 21;
            // 
            // labelX10
            // 
            this.labelX10.Location = new System.Drawing.Point(12, 79);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(75, 23);
            this.labelX10.TabIndex = 2;
            this.labelX10.Text = "金字塔：";
            // 
            // tileH
            // 
            // 
            // 
            // 
            this.tileH.Border.Class = "TextBoxBorder";
            this.tileH.Location = new System.Drawing.Point(110, 108);
            this.tileH.Name = "tileH";
            this.tileH.Size = new System.Drawing.Size(209, 21);
            this.tileH.TabIndex = 8;
            // 
            // cmbResampleType
            // 
            this.cmbResampleType.DisplayMember = "Text";
            this.cmbResampleType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbResampleType.FormattingEnabled = true;
            this.cmbResampleType.ItemHeight = 15;
            this.cmbResampleType.Location = new System.Drawing.Point(109, 20);
            this.cmbResampleType.Name = "cmbResampleType";
            this.cmbResampleType.Size = new System.Drawing.Size(210, 21);
            this.cmbResampleType.TabIndex = 5;
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(12, 169);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(93, 23);
            this.labelX7.TabIndex = 20;
            this.labelX7.Text = "波   段   数：";
            // 
            // cmbCompression
            // 
            this.cmbCompression.DisplayMember = "Text";
            this.cmbCompression.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCompression.FormattingEnabled = true;
            this.cmbCompression.ItemHeight = 15;
            this.cmbCompression.Location = new System.Drawing.Point(109, 50);
            this.cmbCompression.Name = "cmbCompression";
            this.cmbCompression.Size = new System.Drawing.Size(210, 21);
            this.cmbCompression.TabIndex = 6;
            // 
            // labelX9
            // 
            this.labelX9.Location = new System.Drawing.Point(12, 137);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(75, 23);
            this.labelX9.TabIndex = 4;
            this.labelX9.Text = "瓦片宽度：";
            // 
            // txtPyramid
            // 
            // 
            // 
            // 
            this.txtPyramid.Border.Class = "TextBoxBorder";
            this.txtPyramid.Location = new System.Drawing.Point(110, 77);
            this.txtPyramid.Name = "txtPyramid";
            this.txtPyramid.Size = new System.Drawing.Size(209, 21);
            this.txtPyramid.TabIndex = 7;
            // 
            // labelX8
            // 
            this.labelX8.Location = new System.Drawing.Point(12, 108);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(75, 23);
            this.labelX8.TabIndex = 3;
            this.labelX8.Text = "瓦片高度：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(17, 49);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(94, 23);
            this.labelX2.TabIndex = 41;
            this.labelX2.Text = "数  据   集：";
            // 
            // txt_RasterName
            // 
            // 
            // 
            // 
            this.txt_RasterName.Border.Class = "TextBoxBorder";
            this.txt_RasterName.Location = new System.Drawing.Point(120, 53);
            this.txt_RasterName.Name = "txt_RasterName";
            this.txt_RasterName.Size = new System.Drawing.Size(209, 21);
            this.txt_RasterName.TabIndex = 42;
            // 
            // FrmSetRasterDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 360);
            this.Controls.Add(this.txt_RasterName);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.labelXErr);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX19);
            this.Controls.Add(this.btnRuleFile);
            this.Controls.Add(this.rbdataset);
            this.Controls.Add(this.textRuleFilePath);
            this.Controls.Add(this.rbcatalog);
            this.Controls.Add(this.cmbRasterSpaRef);
            this.Controls.Add(this.cmbGeoSpaRef);
            this.Controls.Add(this.btnAddList);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbRasterType);
            this.Controls.Add(this.pListViewDT);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSetRasterDB";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "栅格数据库管理";
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbRasterType;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ListViewEx pListViewDT;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnAddList;
        private System.Windows.Forms.RadioButton rbcatalog;
        private System.Windows.Forms.RadioButton rbdataset;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbRasterSpaRef;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbGeoSpaRef;
        private DevComponents.DotNetBar.LabelX labelXErr;
        private DevComponents.DotNetBar.LabelX labelX19;
        public DevComponents.DotNetBar.ButtonX btnRuleFile;
        public DevComponents.DotNetBar.Controls.TextBoxX textRuleFilePath;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevComponents.DotNetBar.LabelX labelX20;
        private DevComponents.DotNetBar.LabelX labelX12;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbRasterPixeType;
        private DevComponents.DotNetBar.Controls.TextBoxX tileW;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBand;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.Controls.TextBoxX tileH;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbResampleType;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCompression;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPyramid;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_RasterName;
    }
}

