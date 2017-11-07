namespace GeoUtilities
{
    partial class frmGeoTrans
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGeoTrans));
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnSelAll = new DevComponents.DotNetBar.ButtonX();
            this.btnSelReverse = new DevComponents.DotNetBar.ButtonX();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoMDB = new System.Windows.Forms.RadioButton();
            this.rdoGDB = new System.Windows.Forms.RadioButton();
            this.rdoSHP = new System.Windows.Forms.RadioButton();
            this.lstLyrFile = new System.Windows.Forms.ListView();
            this.btnSource = new DevComponents.DotNetBar.ButtonX();
            this.txtSource = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dtTns = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.tnsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tnsValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmboTnsMethod = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
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
            this.tabControl1 = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.tabItem2 = new DevComponents.DotNetBar.TabItem(this.components);
            this.uC_CoorTran1 = new GeoUtilities.UC_CoorTran();
            this.groupPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtTns)).BeginInit();
            this.groupPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            this.tabControlPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btnSelAll);
            this.groupPanel1.Controls.Add(this.btnSelReverse);
            this.groupPanel1.Controls.Add(this.groupBox1);
            this.groupPanel1.Controls.Add(this.lstLyrFile);
            this.groupPanel1.Controls.Add(this.btnSource);
            this.groupPanel1.Controls.Add(this.txtSource);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(4, 4);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(487, 230);
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
            // btnSelAll
            // 
            this.btnSelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelAll.Location = new System.Drawing.Point(430, 114);
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
            this.btnSelReverse.Location = new System.Drawing.Point(430, 149);
            this.btnSelReverse.Name = "btnSelReverse";
            this.btnSelReverse.Size = new System.Drawing.Size(31, 21);
            this.btnSelReverse.TabIndex = 58;
            this.btnSelReverse.Text = "反选";
            this.btnSelReverse.Click += new System.EventHandler(this.btnSelReverse_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.rdoMDB);
            this.groupBox1.Controls.Add(this.rdoGDB);
            this.groupBox1.Controls.Add(this.rdoSHP);
            this.groupBox1.Location = new System.Drawing.Point(24, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 48);
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
            this.lstLyrFile.Location = new System.Drawing.Point(23, 87);
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
            this.btnSource.Location = new System.Drawing.Point(419, 60);
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
            this.txtSource.Location = new System.Drawing.Point(23, 60);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(395, 21);
            this.txtSource.TabIndex = 0;
            // 
            // groupPanel2
            // 
            this.groupPanel2.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.dtTns);
            this.groupPanel2.Controls.Add(this.cmboTnsMethod);
            this.groupPanel2.Controls.Add(this.labelX1);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(5, 233);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(487, 177);
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
            this.groupPanel2.TabIndex = 1;
            this.groupPanel2.Text = "转换参数（可选）";
            // 
            // dtTns
            // 
            this.dtTns.AllowUserToAddRows = false;
            this.dtTns.AllowUserToDeleteRows = false;
            this.dtTns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtTns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tnsName,
            this.tnsValue});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtTns.DefaultCellStyle = dataGridViewCellStyle1;
            this.dtTns.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dtTns.Location = new System.Drawing.Point(23, 29);
            this.dtTns.MultiSelect = false;
            this.dtTns.Name = "dtTns";
            this.dtTns.RowTemplate.Height = 23;
            this.dtTns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtTns.ShowCellErrors = false;
            this.dtTns.ShowRowErrors = false;
            this.dtTns.Size = new System.Drawing.Size(437, 118);
            this.dtTns.TabIndex = 2;
            this.dtTns.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtTns_CellMouseDown);
            this.dtTns.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dtTns_DataError);
            // 
            // tnsName
            // 
            this.tnsName.DataPropertyName = "tnsName";
            this.tnsName.HeaderText = "参数名称";
            this.tnsName.Name = "tnsName";
            this.tnsName.Width = 150;
            // 
            // tnsValue
            // 
            this.tnsValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tnsValue.DataPropertyName = "tnsValue";
            this.tnsValue.HeaderText = "参数值";
            this.tnsValue.Name = "tnsValue";
            // 
            // cmboTnsMethod
            // 
            this.cmboTnsMethod.DisplayMember = "Text";
            this.cmboTnsMethod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmboTnsMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboTnsMethod.FormattingEnabled = true;
            this.cmboTnsMethod.ItemHeight = 15;
            this.cmboTnsMethod.Location = new System.Drawing.Point(82, 2);
            this.cmboTnsMethod.Name = "cmboTnsMethod";
            this.cmboTnsMethod.Size = new System.Drawing.Size(378, 21);
            this.cmboTnsMethod.TabIndex = 1;
            this.cmboTnsMethod.SelectedIndexChanged += new System.EventHandler(this.cmboTnsMethod_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(20, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(67, 20);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "变换方法：";
            // 
            // groupPanel3
            // 
            this.groupPanel3.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.labelX3);
            this.groupPanel3.Controls.Add(this.labelX2);
            this.groupPanel3.Controls.Add(this.btnPrjFile);
            this.groupPanel3.Controls.Add(this.txtPrjFile);
            this.groupPanel3.Controls.Add(this.btnGdbFile);
            this.groupPanel3.Controls.Add(this.txtGdbFile);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(4, 416);
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
            this.cmdOk.Location = new System.Drawing.Point(297, 504);
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
            this.cmdCancel.Location = new System.Drawing.Point(397, 504);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(94, 27);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "取消";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // lblTips
            // 
            this.lblTips.BackColor = System.Drawing.Color.Transparent;
            this.lblTips.Location = new System.Drawing.Point(7, 509);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(268, 22);
            this.lblTips.TabIndex = 5;
            // 
            // progressBarX1
            // 
            this.progressBarX1.Location = new System.Drawing.Point(7, 537);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.Size = new System.Drawing.Size(488, 8);
            this.progressBarX1.TabIndex = 6;
            this.progressBarX1.Text = "proBar";
            this.progressBarX1.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.tabControl1.CanReorderTabs = true;
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Controls.Add(this.tabControlPanel2);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl1.SelectedTabIndex = 2;
            this.tabControl1.Size = new System.Drawing.Size(496, 574);
            this.tabControl1.TabIndex = 8;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl1.Tabs.Add(this.tabItem1);
            this.tabControl1.Tabs.Add(this.tabItem2);
            this.tabControl1.Text = "tabControl1";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.cmdCancel);
            this.tabControlPanel1.Controls.Add(this.groupPanel1);
            this.tabControlPanel1.Controls.Add(this.progressBarX1);
            this.tabControlPanel1.Controls.Add(this.cmdOk);
            this.tabControlPanel1.Controls.Add(this.groupPanel3);
            this.tabControlPanel1.Controls.Add(this.lblTips);
            this.tabControlPanel1.Controls.Add(this.groupPanel2);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(496, 548);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItem1;
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "根据参数转换";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this.uC_CoorTran1);
            this.tabControlPanel2.Controls.Add(this.axLicenseControl1);
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(496, 548);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.GradientAngle = 90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabItem2;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(249, 209);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 35;
            // 
            // tabItem2
            // 
            this.tabItem2.AttachedControl = this.tabControlPanel2;
            this.tabItem2.Name = "tabItem2";
            this.tabItem2.Text = "根据控制点文件转换";
            // 
            // uC_CoorTran1
            // 
            this.uC_CoorTran1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.uC_CoorTran1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_CoorTran1.Location = new System.Drawing.Point(1, 1);
            this.uC_CoorTran1.Name = "uC_CoorTran1";
            this.uC_CoorTran1.Size = new System.Drawing.Size(494, 546);
            this.uC_CoorTran1.TabIndex = 36;
            // 
            // frmGeoTrans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 576);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGeoTrans";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坐标变换";
            this.Load += new System.EventHandler(this.frmGeoTrans_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtTns)).EndInit();
            this.groupPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            this.tabControlPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.ButtonX btnSource;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSource;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX cmdOk;
        private DevComponents.DotNetBar.ButtonX cmdCancel;
        private DevComponents.DotNetBar.ButtonX btnGdbFile;
        private DevComponents.DotNetBar.Controls.TextBoxX txtGdbFile;
        private DevComponents.DotNetBar.ButtonX btnPrjFile;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPrjFile;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmboTnsMethod;
        private DevComponents.DotNetBar.Controls.DataGridViewX dtTns;
        private System.Windows.Forms.DataGridViewTextBoxColumn tnsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn tnsValue;
        private System.Windows.Forms.ListView lstLyrFile;
        private DevComponents.DotNetBar.LabelX lblTips;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.TabControl tabControl1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.TabItem tabItem2;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private UC_CoorTran uC_CoorTran1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoMDB;
        private System.Windows.Forms.RadioButton rdoGDB;
        private System.Windows.Forms.RadioButton rdoSHP;
        private DevComponents.DotNetBar.ButtonX btnSelAll;
        private DevComponents.DotNetBar.ButtonX btnSelReverse;

    }
}