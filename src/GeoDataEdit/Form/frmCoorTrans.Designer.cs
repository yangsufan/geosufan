namespace GeoDataEdit
{
    partial class frmCoorTrans
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCoorTrans));
            this.txtCtrlPtPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnCtrlPtPath = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dgvCtrlPt = new System.Windows.Forms.DataGridView();
            this.ColSrcX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSrcY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSrcZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColToX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColToY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColToZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColResidualError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnToPath = new DevComponents.DotNetBar.ButtonX();
            this.txtToPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSrcPath = new DevComponents.DotNetBar.ButtonX();
            this.txtSrcPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cboxSrcType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.lblRMS = new System.Windows.Forms.Label();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.btnOpenxml = new DevComponents.DotNetBar.ButtonX();
            this.btnSaveXml = new DevComponents.DotNetBar.ButtonX();
            this.lstViewResult = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnReturn = new DevComponents.DotNetBar.ButtonX();
            this.btnExport = new DevComponents.DotNetBar.ButtonX();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoMDB = new System.Windows.Forms.RadioButton();
            this.rdoGDB = new System.Windows.Forms.RadioButton();
            this.rdoSHP = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCtrlPt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCtrlPtPath
            // 
            // 
            // 
            // 
            this.txtCtrlPtPath.Border.Class = "TextBoxBorder";
            this.txtCtrlPtPath.ForeColor = System.Drawing.Color.Gray;
            this.txtCtrlPtPath.Location = new System.Drawing.Point(12, 58);
            this.txtCtrlPtPath.Name = "txtCtrlPtPath";
            this.txtCtrlPtPath.Size = new System.Drawing.Size(403, 21);
            this.txtCtrlPtPath.TabIndex = 0;
            this.txtCtrlPtPath.Text = "打开一个控制点文件（txt）";
            this.txtCtrlPtPath.Enter += new System.EventHandler(this.txtCtrlPtPath_Enter);
            this.txtCtrlPtPath.Leave += new System.EventHandler(this.txtCtrlPtPath_Leave);
            // 
            // btnCtrlPtPath
            // 
            this.btnCtrlPtPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCtrlPtPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCtrlPtPath.Location = new System.Drawing.Point(421, 56);
            this.btnCtrlPtPath.Name = "btnCtrlPtPath";
            this.btnCtrlPtPath.Size = new System.Drawing.Size(33, 23);
            this.btnCtrlPtPath.TabIndex = 1;
            this.btnCtrlPtPath.Text = "...";
            this.btnCtrlPtPath.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(286, 287);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "开始转换";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(379, 287);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dgvCtrlPt
            // 
            this.dgvCtrlPt.AllowUserToAddRows = false;
            this.dgvCtrlPt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCtrlPt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColSrcX,
            this.ColSrcY,
            this.ColSrcZ,
            this.ColToX,
            this.ColToY,
            this.ColToZ,
            this.ColResidualError});
            this.dgvCtrlPt.Location = new System.Drawing.Point(12, 139);
            this.dgvCtrlPt.Name = "dgvCtrlPt";
            this.dgvCtrlPt.RowTemplate.Height = 23;
            this.dgvCtrlPt.Size = new System.Drawing.Size(442, 142);
            this.dgvCtrlPt.TabIndex = 2;
            this.dgvCtrlPt.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCtrlPt_CellContentClick);
            // 
            // ColSrcX
            // 
            this.ColSrcX.HeaderText = "源X";
            this.ColSrcX.Name = "ColSrcX";
            // 
            // ColSrcY
            // 
            this.ColSrcY.HeaderText = "源Y";
            this.ColSrcY.Name = "ColSrcY";
            // 
            // ColSrcZ
            // 
            this.ColSrcZ.HeaderText = "源Z";
            this.ColSrcZ.Name = "ColSrcZ";
            // 
            // ColToX
            // 
            this.ColToX.HeaderText = "目标X";
            this.ColToX.Name = "ColToX";
            // 
            // ColToY
            // 
            this.ColToY.HeaderText = "目标Y";
            this.ColToY.Name = "ColToY";
            // 
            // ColToZ
            // 
            this.ColToZ.HeaderText = "目标Z";
            this.ColToZ.Name = "ColToZ";
            // 
            // ColResidualError
            // 
            this.ColResidualError.HeaderText = "残差";
            this.ColResidualError.Name = "ColResidualError";
            // 
            // btnToPath
            // 
            this.btnToPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnToPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnToPath.Location = new System.Drawing.Point(421, 112);
            this.btnToPath.Name = "btnToPath";
            this.btnToPath.Size = new System.Drawing.Size(33, 23);
            this.btnToPath.TabIndex = 4;
            this.btnToPath.Text = "...";
            this.btnToPath.Click += new System.EventHandler(this.btnToPath_Click);
            // 
            // txtToPath
            // 
            // 
            // 
            // 
            this.txtToPath.Border.Class = "TextBoxBorder";
            this.txtToPath.ForeColor = System.Drawing.Color.Gray;
            this.txtToPath.Location = new System.Drawing.Point(12, 112);
            this.txtToPath.Name = "txtToPath";
            this.txtToPath.Size = new System.Drawing.Size(403, 21);
            this.txtToPath.TabIndex = 3;
            this.txtToPath.Text = "转换后的数据的工作空间路径";
            this.txtToPath.Enter += new System.EventHandler(this.txtCtrlPtPath_Enter);
            this.txtToPath.Leave += new System.EventHandler(this.txtCtrlPtPath_Leave);
            // 
            // btnSrcPath
            // 
            this.btnSrcPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSrcPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSrcPath.Location = new System.Drawing.Point(421, 85);
            this.btnSrcPath.Name = "btnSrcPath";
            this.btnSrcPath.Size = new System.Drawing.Size(33, 23);
            this.btnSrcPath.TabIndex = 6;
            this.btnSrcPath.Text = "...";
            this.btnSrcPath.Click += new System.EventHandler(this.btnSrcPath_Click);
            // 
            // txtSrcPath
            // 
            // 
            // 
            // 
            this.txtSrcPath.Border.Class = "TextBoxBorder";
            this.txtSrcPath.ForeColor = System.Drawing.Color.Gray;
            this.txtSrcPath.Location = new System.Drawing.Point(12, 85);
            this.txtSrcPath.Name = "txtSrcPath";
            this.txtSrcPath.Size = new System.Drawing.Size(403, 21);
            this.txtSrcPath.TabIndex = 5;
            this.txtSrcPath.Text = "源数据工作空间路径";
            this.txtSrcPath.Enter += new System.EventHandler(this.txtCtrlPtPath_Enter);
            this.txtSrcPath.Leave += new System.EventHandler(this.txtCtrlPtPath_Leave);
            // 
            // cboxSrcType
            // 
            this.cboxSrcType.DisplayMember = "Text";
            this.cboxSrcType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboxSrcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSrcType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboxSrcType.FormattingEnabled = true;
            this.cboxSrcType.ItemHeight = 15;
            this.cboxSrcType.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3});
            this.cboxSrcType.Location = new System.Drawing.Point(26, 179);
            this.cboxSrcType.Name = "cboxSrcType";
            this.cboxSrcType.Size = new System.Drawing.Size(84, 21);
            this.cboxSrcType.TabIndex = 0;
            this.cboxSrcType.Visible = false;
            this.cboxSrcType.SelectedValueChanged += new System.EventHandler(this.cboxSrcType_SelectedValueChanged);
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "shp";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "mdb";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "gdb";
            // 
            // lblRMS
            // 
            this.lblRMS.AutoSize = true;
            this.lblRMS.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRMS.Location = new System.Drawing.Point(0, 491);
            this.lblRMS.Name = "lblRMS";
            this.lblRMS.Size = new System.Drawing.Size(0, 12);
            this.lblRMS.TabIndex = 8;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(116, 290);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 9;
            // 
            // btnOpenxml
            // 
            this.btnOpenxml.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpenxml.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOpenxml.Location = new System.Drawing.Point(12, 290);
            this.btnOpenxml.Name = "btnOpenxml";
            this.btnOpenxml.Size = new System.Drawing.Size(98, 23);
            this.btnOpenxml.TabIndex = 10;
            this.btnOpenxml.Text = "打开参数文件";
            this.btnOpenxml.Visible = false;
            this.btnOpenxml.Click += new System.EventHandler(this.btnOpenxml_Click);
            // 
            // btnSaveXml
            // 
            this.btnSaveXml.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveXml.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveXml.Location = new System.Drawing.Point(116, 290);
            this.btnSaveXml.Name = "btnSaveXml";
            this.btnSaveXml.Size = new System.Drawing.Size(98, 23);
            this.btnSaveXml.TabIndex = 11;
            this.btnSaveXml.Text = "保存参数文件";
            this.btnSaveXml.Visible = false;
            this.btnSaveXml.Click += new System.EventHandler(this.btnSaveXml_Click);
            // 
            // lstViewResult
            // 
            // 
            // 
            // 
            this.lstViewResult.Border.Class = "ListViewBorder";
            this.lstViewResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstViewResult.FullRowSelect = true;
            this.lstViewResult.GridLines = true;
            this.lstViewResult.Location = new System.Drawing.Point(12, 328);
            this.lstViewResult.Name = "lstViewResult";
            this.lstViewResult.Size = new System.Drawing.Size(403, 151);
            this.lstViewResult.TabIndex = 12;
            this.lstViewResult.UseCompatibleStateImageBehavior = false;
            this.lstViewResult.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "要素类名称";
            this.columnHeader1.Width = 183;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "转换结果";
            this.columnHeader2.Width = 205;
            // 
            // btnReturn
            // 
            this.btnReturn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReturn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReturn.Location = new System.Drawing.Point(421, 415);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(33, 23);
            this.btnReturn.TabIndex = 13;
            this.btnReturn.Text = "返回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(421, 456);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(33, 23);
            this.btnExport.TabIndex = 14;
            this.btnExport.Text = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoMDB);
            this.groupBox1.Controls.Add(this.rdoGDB);
            this.groupBox1.Controls.Add(this.rdoSHP);
            this.groupBox1.Location = new System.Drawing.Point(12, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 48);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据类型";
            // 
            // rdoMDB
            // 
            this.rdoMDB.AutoSize = true;
            this.rdoMDB.Location = new System.Drawing.Point(180, 20);
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
            this.rdoGDB.Location = new System.Drawing.Point(310, 20);
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
            // frmCoorTrans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 503);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.lstViewResult);
            this.Controls.Add(this.btnSaveXml);
            this.Controls.Add(this.btnOpenxml);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.lblRMS);
            this.Controls.Add(this.cboxSrcType);
            this.Controls.Add(this.btnSrcPath);
            this.Controls.Add(this.txtSrcPath);
            this.Controls.Add(this.btnToPath);
            this.Controls.Add(this.txtToPath);
            this.Controls.Add(this.dgvCtrlPt);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCtrlPtPath);
            this.Controls.Add(this.txtCtrlPtPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCoorTrans";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坐标转换";
            this.Load += new System.EventHandler(this.frmRdCtrlPt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCtrlPt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtCtrlPtPath;
        private DevComponents.DotNetBar.ButtonX btnCtrlPtPath;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView dgvCtrlPt;
        private DevComponents.DotNetBar.ButtonX btnToPath;
        private DevComponents.DotNetBar.Controls.TextBoxX txtToPath;
        private DevComponents.DotNetBar.ButtonX btnSrcPath;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSrcPath;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboxSrcType;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private System.Windows.Forms.Label lblRMS;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSrcX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSrcY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSrcZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColToX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColToY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColToZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColResidualError;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private DevComponents.DotNetBar.ButtonX btnOpenxml;
        private DevComponents.DotNetBar.ButtonX btnSaveXml;
        private DevComponents.DotNetBar.Controls.ListViewEx lstViewResult;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private DevComponents.DotNetBar.ButtonX btnReturn;
        private DevComponents.DotNetBar.ButtonX btnExport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoMDB;
        private System.Windows.Forms.RadioButton rdoGDB;
        private System.Windows.Forms.RadioButton rdoSHP;
    }
}