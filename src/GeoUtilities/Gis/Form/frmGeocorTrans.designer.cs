namespace GeoUtilities
{
    partial class frmGeocorTrans
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
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
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoMDB = new System.Windows.Forms.RadioButton();
            this.rdoGDB = new System.Windows.Forms.RadioButton();
            this.rdoSHP = new System.Windows.Forms.RadioButton();
            this.lstLyrFile = new System.Windows.Forms.ListView();
            this.btnSource = new DevComponents.DotNetBar.ButtonX();
            this.txtSource = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btnPrjFile = new DevComponents.DotNetBar.ButtonX();
            this.txtPrjFile = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnGdbFile = new DevComponents.DotNetBar.ButtonX();
            this.txtGdbFile = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cmdOk = new DevComponents.DotNetBar.ButtonX();
            this.cmdCancel = new DevComponents.DotNetBar.ButtonX();
            this.lblTips = new DevComponents.DotNetBar.LabelX();
            this.progressBarX1 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.btnSelAll = new DevComponents.DotNetBar.ButtonX();
            this.btnSelReverse = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btnSelAll);
            this.groupPanel1.Controls.Add(this.btnSelReverse);
            this.groupPanel1.Controls.Add(this.groupBox1);
            this.groupPanel1.Controls.Add(this.lstLyrFile);
            this.groupPanel1.Controls.Add(this.btnSource);
            this.groupPanel1.Controls.Add(this.txtSource);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(12, 4);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(487, 224);
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
            this.groupPanel1.TabIndex = 0;
            this.groupPanel1.Text = "源数据";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.rdoMDB);
            this.groupBox1.Controls.Add(this.rdoGDB);
            this.groupBox1.Controls.Add(this.rdoSHP);
            this.groupBox1.Location = new System.Drawing.Point(21, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 48);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据类型";
            // 
            // rdoMDB
            // 
            this.rdoMDB.AutoSize = true;
            this.rdoMDB.Location = new System.Drawing.Point(192, 20);
            this.rdoMDB.Name = "rdoMDB";
            this.rdoMDB.Size = new System.Drawing.Size(41, 16);
            this.rdoMDB.TabIndex = 18;
            this.rdoMDB.Text = "mdb";
            this.rdoMDB.UseVisualStyleBackColor = true;
            // 
            // rdoGDB
            // 
            this.rdoGDB.AutoSize = true;
            this.rdoGDB.Location = new System.Drawing.Point(334, 20);
            this.rdoGDB.Name = "rdoGDB";
            this.rdoGDB.Size = new System.Drawing.Size(41, 16);
            this.rdoGDB.TabIndex = 19;
            this.rdoGDB.Text = "gdb";
            this.rdoGDB.UseVisualStyleBackColor = true;
            // 
            // rdoSHP
            // 
            this.rdoSHP.AutoSize = true;
            this.rdoSHP.Checked = true;
            this.rdoSHP.Location = new System.Drawing.Point(50, 20);
            this.rdoSHP.Name = "rdoSHP";
            this.rdoSHP.Size = new System.Drawing.Size(41, 16);
            this.rdoSHP.TabIndex = 17;
            this.rdoSHP.TabStop = true;
            this.rdoSHP.Text = "shp";
            this.rdoSHP.UseVisualStyleBackColor = true;
            // 
            // lstLyrFile
            // 
            this.lstLyrFile.CheckBoxes = true;
            this.lstLyrFile.Location = new System.Drawing.Point(21, 82);
            this.lstLyrFile.Name = "lstLyrFile";
            this.lstLyrFile.Size = new System.Drawing.Size(395, 114);
            this.lstLyrFile.TabIndex = 12;
            this.lstLyrFile.UseCompatibleStateImageBehavior = false;
            this.lstLyrFile.View = System.Windows.Forms.View.List;
            this.lstLyrFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstLyrFile_MouseDown);
            this.lstLyrFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstLyrFile_KeyDown);
            // 
            // btnSource
            // 
            this.btnSource.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSource.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSource.Location = new System.Drawing.Point(417, 55);
            this.btnSource.Name = "btnSource";
            this.btnSource.Size = new System.Drawing.Size(42, 21);
            this.btnSource.TabIndex = 1;
            this.btnSource.Text = "...";
            this.btnSource.Click += new System.EventHandler(this.btnSource_Click);
            // 
            // txtSource
            // 
            // 
            // 
            // 
            this.txtSource.Border.Class = "TextBoxBorder";
            this.txtSource.Location = new System.Drawing.Point(21, 55);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(395, 21);
            this.txtSource.TabIndex = 0;
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.labelX3);
            this.groupPanel3.Controls.Add(this.labelX2);
            this.groupPanel3.Controls.Add(this.btnPrjFile);
            this.groupPanel3.Controls.Add(this.txtPrjFile);
            this.groupPanel3.Controls.Add(this.btnGdbFile);
            this.groupPanel3.Controls.Add(this.txtGdbFile);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(15, 234);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(487, 82);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel3.TabIndex = 2;
            this.groupPanel3.Text = "目标数据";
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(9, 27);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(67, 20);
            this.labelX3.TabIndex = 7;
            this.labelX3.Text = "空间参考：";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(9, 4);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(67, 20);
            this.labelX2.TabIndex = 6;
            this.labelX2.Text = "输出文件：";
            // 
            // btnPrjFile
            // 
            this.btnPrjFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrjFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrjFile.Location = new System.Drawing.Point(419, 30);
            this.btnPrjFile.Name = "btnPrjFile";
            this.btnPrjFile.Size = new System.Drawing.Size(42, 21);
            this.btnPrjFile.TabIndex = 5;
            this.btnPrjFile.Text = "...";
            this.btnPrjFile.Click += new System.EventHandler(this.btnPrjFile_Click);
            // 
            // txtPrjFile
            // 
            // 
            // 
            // 
            this.txtPrjFile.Border.Class = "TextBoxBorder";
            this.txtPrjFile.Location = new System.Drawing.Point(82, 30);
            this.txtPrjFile.Name = "txtPrjFile";
            this.txtPrjFile.ReadOnly = true;
            this.txtPrjFile.Size = new System.Drawing.Size(335, 21);
            this.txtPrjFile.TabIndex = 4;
            // 
            // btnGdbFile
            // 
            this.btnGdbFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGdbFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnGdbFile.Location = new System.Drawing.Point(419, 3);
            this.btnGdbFile.Name = "btnGdbFile";
            this.btnGdbFile.Size = new System.Drawing.Size(42, 21);
            this.btnGdbFile.TabIndex = 3;
            this.btnGdbFile.Text = "...";
            this.btnGdbFile.Click += new System.EventHandler(this.btnGdbFile_Click);
            // 
            // txtGdbFile
            // 
            // 
            // 
            // 
            this.txtGdbFile.Border.Class = "TextBoxBorder";
            this.txtGdbFile.Location = new System.Drawing.Point(82, 3);
            this.txtGdbFile.Name = "txtGdbFile";
            this.txtGdbFile.ReadOnly = true;
            this.txtGdbFile.Size = new System.Drawing.Size(335, 21);
            this.txtGdbFile.TabIndex = 2;
            // 
            // cmdOk
            // 
            this.cmdOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cmdOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cmdOk.Location = new System.Drawing.Point(305, 342);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(94, 27);
            this.cmdOk.TabIndex = 3;
            this.cmdOk.Text = "确定";
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cmdCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cmdCancel.Location = new System.Drawing.Point(405, 342);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(94, 27);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "取消";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // lblTips
            // 
            this.lblTips.Location = new System.Drawing.Point(15, 347);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(268, 22);
            this.lblTips.TabIndex = 5;
            // 
            // progressBarX1
            // 
            this.progressBarX1.Location = new System.Drawing.Point(15, 322);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.Size = new System.Drawing.Size(498, 8);
            this.progressBarX1.TabIndex = 6;
            this.progressBarX1.Text = "proBar";
            this.progressBarX1.Visible = false;
            // 
            // btnSelAll
            // 
            this.btnSelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelAll.Location = new System.Drawing.Point(428, 105);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(31, 21);
            this.btnSelAll.TabIndex = 57;
            this.btnSelAll.Text = "全选";
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnSelReverse
            // 
            this.btnSelReverse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelReverse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelReverse.Location = new System.Drawing.Point(428, 140);
            this.btnSelReverse.Name = "btnSelReverse";
            this.btnSelReverse.Size = new System.Drawing.Size(31, 21);
            this.btnSelReverse.TabIndex = 58;
            this.btnSelReverse.Text = "反选";
            this.btnSelReverse.Click += new System.EventHandler(this.btnSelReverse_Click);
            // 
            // frmGeocorTrans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 381);
            this.Controls.Add(this.progressBarX1);
            this.Controls.Add(this.lblTips);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.groupPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGeocorTrans";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "投影变换";
            this.Load += new System.EventHandler(this.frmGeoTrans_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.ButtonX btnSource;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSource;
        private DevComponents.DotNetBar.ButtonX cmdOk;
        private DevComponents.DotNetBar.ButtonX cmdCancel;
        private DevComponents.DotNetBar.ButtonX btnGdbFile;
        private DevComponents.DotNetBar.Controls.TextBoxX txtGdbFile;
        private DevComponents.DotNetBar.ButtonX btnPrjFile;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPrjFile;
        private System.Windows.Forms.ListView lstLyrFile;
        private DevComponents.DotNetBar.LabelX lblTips;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoMDB;
        private System.Windows.Forms.RadioButton rdoGDB;
        private System.Windows.Forms.RadioButton rdoSHP;
        private DevComponents.DotNetBar.ButtonX btnSelAll;
        private DevComponents.DotNetBar.ButtonX btnSelReverse;

    }
}