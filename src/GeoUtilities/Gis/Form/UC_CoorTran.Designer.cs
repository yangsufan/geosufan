namespace GeoUtilities
{
    partial class UC_CoorTran
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_CoorTran));
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoMDB = new System.Windows.Forms.RadioButton();
            this.rdoGDB = new System.Windows.Forms.RadioButton();
            this.rdoSHP = new System.Windows.Forms.RadioButton();
            this.btnExport = new DevComponents.DotNetBar.ButtonX();
            this.btnSrcPath = new DevComponents.DotNetBar.ButtonX();
            this.txtSrcPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnToPath = new DevComponents.DotNetBar.ButtonX();
            this.txtToPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.dgvCtrlPt = new System.Windows.Forms.DataGridView();
            this.ColID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSrcX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSrcY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSrcZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColToX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColToY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColToZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColResidualError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCtrlPtPath = new DevComponents.DotNetBar.ButtonX();
            this.txtCtrlPtPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblRMS = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lstFC = new System.Windows.Forms.ListView();
            this.btnSelAll = new DevComponents.DotNetBar.ButtonX();
            this.btnSelReverse = new DevComponents.DotNetBar.ButtonX();
            this.pbCoorTran = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.btnSaveCtrPts = new DevComponents.DotNetBar.ButtonX();
            this.btnEditCtrlPts = new DevComponents.DotNetBar.ButtonX();
            this.btnPrjFile = new DevComponents.DotNetBar.ButtonX();
            this.txtPrjFile = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCtrlPt)).BeginInit();
            this.SuspendLayout();
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(254, 358);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 49;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.rdoMDB);
            this.groupBox1.Controls.Add(this.rdoGDB);
            this.groupBox1.Controls.Add(this.rdoSHP);
            this.groupBox1.Location = new System.Drawing.Point(9, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 48);
            this.groupBox1.TabIndex = 48;
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
            this.rdoMDB.CheckedChanged += new System.EventHandler(this.rdoMDB_CheckedChanged);
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
            this.rdoGDB.CheckedChanged += new System.EventHandler(this.rdoGDB_CheckedChanged);
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
            this.rdoSHP.CheckedChanged += new System.EventHandler(this.rdoSHP_CheckedChanged);
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(309, 486);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 47;
            this.btnExport.Text = "查看日志";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSrcPath
            // 
            this.btnSrcPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSrcPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSrcPath.Location = new System.Drawing.Point(455, 57);
            this.btnSrcPath.Name = "btnSrcPath";
            this.btnSrcPath.Size = new System.Drawing.Size(33, 23);
            this.btnSrcPath.TabIndex = 44;
            this.btnSrcPath.Text = "...";
            this.btnSrcPath.Click += new System.EventHandler(this.btnSrcPath_Click);
            // 
            // txtSrcPath
            // 
            // 
            // 
            // 
            this.txtSrcPath.Border.Class = "TextBoxBorder";
            this.txtSrcPath.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSrcPath.Location = new System.Drawing.Point(84, 59);
            this.txtSrcPath.Name = "txtSrcPath";
            this.txtSrcPath.Size = new System.Drawing.Size(365, 21);
            this.txtSrcPath.TabIndex = 43;
            // 
            // btnToPath
            // 
            this.btnToPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnToPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnToPath.Location = new System.Drawing.Point(453, 420);
            this.btnToPath.Name = "btnToPath";
            this.btnToPath.Size = new System.Drawing.Size(33, 21);
            this.btnToPath.TabIndex = 42;
            this.btnToPath.Text = "...";
            this.btnToPath.Click += new System.EventHandler(this.btnToPath_Click);
            // 
            // txtToPath
            // 
            // 
            // 
            // 
            this.txtToPath.Border.Class = "TextBoxBorder";
            this.txtToPath.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtToPath.Location = new System.Drawing.Point(82, 420);
            this.txtToPath.Name = "txtToPath";
            this.txtToPath.Size = new System.Drawing.Size(367, 21);
            this.txtToPath.TabIndex = 41;
            // 
            // dgvCtrlPt
            // 
            this.dgvCtrlPt.AllowUserToAddRows = false;
            this.dgvCtrlPt.ColumnHeadersHeight = 20;
            this.dgvCtrlPt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCtrlPt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColID,
            this.ColSrcX,
            this.ColSrcY,
            this.ColSrcZ,
            this.ColToX,
            this.ColToY,
            this.ColToZ,
            this.ColResidualError});
            this.dgvCtrlPt.Location = new System.Drawing.Point(9, 273);
            this.dgvCtrlPt.Name = "dgvCtrlPt";
            this.dgvCtrlPt.ReadOnly = true;
            this.dgvCtrlPt.RowHeadersWidth = 20;
            this.dgvCtrlPt.RowTemplate.Height = 23;
            this.dgvCtrlPt.Size = new System.Drawing.Size(440, 141);
            this.dgvCtrlPt.TabIndex = 40;
            // 
            // ColID
            // 
            this.ColID.HeaderText = "编号";
            this.ColID.Name = "ColID";
            this.ColID.ReadOnly = true;
            this.ColID.Width = 40;
            // 
            // ColSrcX
            // 
            this.ColSrcX.HeaderText = "源X";
            this.ColSrcX.Name = "ColSrcX";
            this.ColSrcX.ReadOnly = true;
            // 
            // ColSrcY
            // 
            this.ColSrcY.HeaderText = "源Y";
            this.ColSrcY.Name = "ColSrcY";
            this.ColSrcY.ReadOnly = true;
            // 
            // ColSrcZ
            // 
            this.ColSrcZ.HeaderText = "源Z";
            this.ColSrcZ.Name = "ColSrcZ";
            this.ColSrcZ.ReadOnly = true;
            // 
            // ColToX
            // 
            this.ColToX.HeaderText = "目标X";
            this.ColToX.Name = "ColToX";
            this.ColToX.ReadOnly = true;
            // 
            // ColToY
            // 
            this.ColToY.HeaderText = "目标Y";
            this.ColToY.Name = "ColToY";
            this.ColToY.ReadOnly = true;
            // 
            // ColToZ
            // 
            this.ColToZ.HeaderText = "目标Z";
            this.ColToZ.Name = "ColToZ";
            this.ColToZ.ReadOnly = true;
            // 
            // ColResidualError
            // 
            this.ColResidualError.HeaderText = "残差";
            this.ColResidualError.Name = "ColResidualError";
            this.ColResidualError.ReadOnly = true;
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(413, 486);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 37;
            this.btnOK.Text = "开始转换";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCtrlPtPath
            // 
            this.btnCtrlPtPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCtrlPtPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCtrlPtPath.Location = new System.Drawing.Point(453, 246);
            this.btnCtrlPtPath.Name = "btnCtrlPtPath";
            this.btnCtrlPtPath.Size = new System.Drawing.Size(33, 23);
            this.btnCtrlPtPath.TabIndex = 39;
            this.btnCtrlPtPath.Text = "...";
            this.btnCtrlPtPath.Click += new System.EventHandler(this.buttonX1_Click);
            this.btnCtrlPtPath.Leave += new System.EventHandler(this.btnCtrlPtPath_Leave);
            // 
            // txtCtrlPtPath
            // 
            // 
            // 
            // 
            this.txtCtrlPtPath.Border.Class = "TextBoxBorder";
            this.txtCtrlPtPath.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCtrlPtPath.Location = new System.Drawing.Point(84, 248);
            this.txtCtrlPtPath.Name = "txtCtrlPtPath";
            this.txtCtrlPtPath.Size = new System.Drawing.Size(363, 21);
            this.txtCtrlPtPath.TabIndex = 38;
            // 
            // lblRMS
            // 
            this.lblRMS.AutoSize = true;
            this.lblRMS.BackColor = System.Drawing.Color.Transparent;
            this.lblRMS.Location = new System.Drawing.Point(7, 507);
            this.lblRMS.Name = "lblRMS";
            this.lblRMS.Size = new System.Drawing.Size(0, 12);
            this.lblRMS.TabIndex = 50;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 255);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 51;
            this.label1.Text = "控制点文件:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 52;
            this.label2.Text = "源数据:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 427);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 53;
            this.label3.Text = "目标数据:";
            // 
            // lstFC
            // 
            this.lstFC.CheckBoxes = true;
            this.lstFC.Location = new System.Drawing.Point(9, 86);
            this.lstFC.Name = "lstFC";
            this.lstFC.Size = new System.Drawing.Size(440, 156);
            this.lstFC.TabIndex = 54;
            this.lstFC.UseCompatibleStateImageBehavior = false;
            this.lstFC.View = System.Windows.Forms.View.List;
            // 
            // btnSelAll
            // 
            this.btnSelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelAll.Location = new System.Drawing.Point(455, 131);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(31, 21);
            this.btnSelAll.TabIndex = 55;
            this.btnSelAll.Text = "全选";
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnSelReverse
            // 
            this.btnSelReverse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelReverse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelReverse.Location = new System.Drawing.Point(455, 166);
            this.btnSelReverse.Name = "btnSelReverse";
            this.btnSelReverse.Size = new System.Drawing.Size(31, 21);
            this.btnSelReverse.TabIndex = 56;
            this.btnSelReverse.Text = "反选";
            this.btnSelReverse.Click += new System.EventHandler(this.btnSelReverse_Click);
            // 
            // pbCoorTran
            // 
            this.pbCoorTran.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbCoorTran.Location = new System.Drawing.Point(0, 522);
            this.pbCoorTran.Name = "pbCoorTran";
            this.pbCoorTran.Size = new System.Drawing.Size(496, 14);
            this.pbCoorTran.TabIndex = 57;
            this.pbCoorTran.Text = "pbCopyRows";
            this.pbCoorTran.Visible = false;
            // 
            // btnSaveCtrPts
            // 
            this.btnSaveCtrPts.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveCtrPts.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveCtrPts.Location = new System.Drawing.Point(453, 358);
            this.btnSaveCtrPts.Name = "btnSaveCtrPts";
            this.btnSaveCtrPts.Size = new System.Drawing.Size(33, 23);
            this.btnSaveCtrPts.TabIndex = 58;
            this.btnSaveCtrPts.Text = "导出";
            this.btnSaveCtrPts.Click += new System.EventHandler(this.btnSaveCtrPts_Click);
            // 
            // btnEditCtrlPts
            // 
            this.btnEditCtrlPts.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnEditCtrlPts.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnEditCtrlPts.Location = new System.Drawing.Point(453, 310);
            this.btnEditCtrlPts.Name = "btnEditCtrlPts";
            this.btnEditCtrlPts.Size = new System.Drawing.Size(33, 23);
            this.btnEditCtrlPts.TabIndex = 59;
            this.btnEditCtrlPts.Text = "编辑";
            this.btnEditCtrlPts.Click += new System.EventHandler(this.btnEditCtrlPts_Click);
            // 
            // btnPrjFile
            // 
            this.btnPrjFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrjFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrjFile.Location = new System.Drawing.Point(453, 447);
            this.btnPrjFile.Name = "btnPrjFile";
            this.btnPrjFile.Size = new System.Drawing.Size(33, 21);
            this.btnPrjFile.TabIndex = 61;
            this.btnPrjFile.Text = "...";
            this.btnPrjFile.Click += new System.EventHandler(this.btnPrjFile_Click);
            // 
            // txtPrjFile
            // 
            this.txtPrjFile.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtPrjFile.Border.Class = "TextBoxBorder";
            this.txtPrjFile.Location = new System.Drawing.Point(82, 445);
            this.txtPrjFile.Name = "txtPrjFile";
            this.txtPrjFile.ReadOnly = true;
            this.txtPrjFile.Size = new System.Drawing.Size(367, 21);
            this.txtPrjFile.TabIndex = 60;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 447);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 63;
            this.label4.Text = "空间参考:";
            // 
            // UC_CoorTran
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnPrjFile);
            this.Controls.Add(this.txtPrjFile);
            this.Controls.Add(this.btnEditCtrlPts);
            this.Controls.Add(this.btnSaveCtrPts);
            this.Controls.Add(this.pbCoorTran);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.btnSelReverse);
            this.Controls.Add(this.lstFC);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblRMS);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnSrcPath);
            this.Controls.Add(this.txtSrcPath);
            this.Controls.Add(this.btnToPath);
            this.Controls.Add(this.txtToPath);
            this.Controls.Add(this.dgvCtrlPt);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCtrlPtPath);
            this.Controls.Add(this.txtCtrlPtPath);
            this.Name = "UC_CoorTran";
            this.Size = new System.Drawing.Size(496, 536);
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCtrlPt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoMDB;
        private System.Windows.Forms.RadioButton rdoGDB;
        private System.Windows.Forms.RadioButton rdoSHP;
        private DevComponents.DotNetBar.ButtonX btnExport;
        private DevComponents.DotNetBar.ButtonX btnSrcPath;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSrcPath;
        private DevComponents.DotNetBar.ButtonX btnToPath;
        private DevComponents.DotNetBar.Controls.TextBoxX txtToPath;
        private System.Windows.Forms.DataGridView dgvCtrlPt;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCtrlPtPath;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCtrlPtPath;
        private System.Windows.Forms.Label lblRMS;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView lstFC;
        private DevComponents.DotNetBar.ButtonX btnSelAll;
        private DevComponents.DotNetBar.ButtonX btnSelReverse;
        private DevComponents.DotNetBar.Controls.ProgressBarX pbCoorTran;
        private DevComponents.DotNetBar.ButtonX btnSaveCtrPts;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSrcX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSrcY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSrcZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColToX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColToY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColToZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColResidualError;
        private DevComponents.DotNetBar.ButtonX btnEditCtrlPts;
        private DevComponents.DotNetBar.ButtonX btnPrjFile;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPrjFile;
        private System.Windows.Forms.Label label4;
    }
}
