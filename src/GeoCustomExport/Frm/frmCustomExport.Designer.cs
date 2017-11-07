namespace GeoCustomExport
{
    partial class frmCustomExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomExport));
            this.panelEx3 = new DevComponents.DotNetBar.PanelEx();
            this.btnCheck = new DevComponents.DotNetBar.ButtonX();
            this.labelDetail = new DevComponents.DotNetBar.LabelX();
            this.labelprogress = new DevComponents.DotNetBar.LabelX();
            this.progressBarField = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.progressBarLayer = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.btn_ImportXml = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnOutPath = new DevComponents.DotNetBar.ButtonX();
            this.txtPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btn_Exit = new DevComponents.DotNetBar.ButtonX();
            this.btn_ImportData = new DevComponents.DotNetBar.ButtonX();
            this.btn_SaveXml = new DevComponents.DotNetBar.ButtonX();
            this.groupPanelRange = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btn_LoadData = new DevComponents.DotNetBar.ButtonX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txt_Range = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btn_SelectRange = new DevComponents.DotNetBar.ButtonX();
            this.cb_Range = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.btn_Save = new DevComponents.DotNetBar.ButtonX();
            this.groupPanelField = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtSQL = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btn_AddCondition = new DevComponents.DotNetBar.ButtonX();
            this.btn_DelCondition = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btn_DelField = new DevComponents.DotNetBar.ButtonX();
            this.listViewField = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.btn_ModifyField = new DevComponents.DotNetBar.ButtonX();
            this.btn_AddField = new DevComponents.DotNetBar.ButtonX();
            this.btn_AllcheckField = new DevComponents.DotNetBar.ButtonX();
            this.btn_RecheckField = new DevComponents.DotNetBar.ButtonX();
            this.groupPanelLayer = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.checkBox_Cut = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btn_DelLayer = new DevComponents.DotNetBar.ButtonX();
            this.btn_ModifyLayer = new DevComponents.DotNetBar.ButtonX();
            this.btn_AddLayer = new DevComponents.DotNetBar.ButtonX();
            this.btn_RecheckLayer = new DevComponents.DotNetBar.ButtonX();
            this.btn_AllcheckLayer = new DevComponents.DotNetBar.ButtonX();
            this.listViewFeaCls = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.colText = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuLayer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPast = new System.Windows.Forms.ToolStripMenuItem();
            this.panelEx3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.groupPanelRange.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.groupPanelField.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupPanelLayer.SuspendLayout();
            this.contextMenuLayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx3
            // 
            this.panelEx3.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx3.Controls.Add(this.btnCheck);
            this.panelEx3.Controls.Add(this.labelDetail);
            this.panelEx3.Controls.Add(this.labelprogress);
            this.panelEx3.Controls.Add(this.progressBarField);
            this.panelEx3.Controls.Add(this.progressBarLayer);
            this.panelEx3.Controls.Add(this.axLicenseControl1);
            this.panelEx3.Controls.Add(this.btn_ImportXml);
            this.panelEx3.Controls.Add(this.labelX1);
            this.panelEx3.Controls.Add(this.btnOutPath);
            this.panelEx3.Controls.Add(this.txtPath);
            this.panelEx3.Controls.Add(this.btn_Exit);
            this.panelEx3.Controls.Add(this.btn_ImportData);
            this.panelEx3.Controls.Add(this.btn_SaveXml);
            this.panelEx3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx3.Location = new System.Drawing.Point(0, 545);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(646, 124);
            this.panelEx3.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx3.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx3.Style.GradientAngle = 90;
            this.panelEx3.TabIndex = 1;
            // 
            // btnCheck
            // 
            this.btnCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCheck.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCheck.Location = new System.Drawing.Point(162, 84);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(85, 23);
            this.btnCheck.TabIndex = 19;
            this.btnCheck.Text = "配置校核";
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // labelDetail
            // 
            this.labelDetail.AutoSize = true;
            this.labelDetail.Location = new System.Drawing.Point(76, 66);
            this.labelDetail.Name = "labelDetail";
            this.labelDetail.Size = new System.Drawing.Size(68, 18);
            this.labelDetail.TabIndex = 18;
            this.labelDetail.Text = "进度条提示";
            // 
            // labelprogress
            // 
            this.labelprogress.AutoSize = true;
            this.labelprogress.Location = new System.Drawing.Point(9, 32);
            this.labelprogress.Name = "labelprogress";
            this.labelprogress.Size = new System.Drawing.Size(62, 18);
            this.labelprogress.TabIndex = 18;
            this.labelprogress.Text = "当前进度:";
            // 
            // progressBarField
            // 
            this.progressBarField.Location = new System.Drawing.Point(76, 48);
            this.progressBarField.Name = "progressBarField";
            this.progressBarField.Size = new System.Drawing.Size(557, 12);
            this.progressBarField.TabIndex = 17;
            this.progressBarField.Text = "progressBarX1";
            // 
            // progressBarLayer
            // 
            this.progressBarLayer.ChunkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.progressBarLayer.ChunkColor2 = System.Drawing.Color.Teal;
            this.progressBarLayer.Location = new System.Drawing.Point(76, 33);
            this.progressBarLayer.Name = "progressBarLayer";
            this.progressBarLayer.Size = new System.Drawing.Size(557, 12);
            this.progressBarLayer.TabIndex = 17;
            this.progressBarLayer.Text = "progressBarX1";
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(0, 84);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 16;
            // 
            // btn_ImportXml
            // 
            this.btn_ImportXml.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_ImportXml.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_ImportXml.Location = new System.Drawing.Point(282, 84);
            this.btn_ImportXml.Name = "btn_ImportXml";
            this.btn_ImportXml.Size = new System.Drawing.Size(85, 23);
            this.btn_ImportXml.TabIndex = 15;
            this.btn_ImportXml.Text = "导入配置";
            this.btn_ImportXml.Click += new System.EventHandler(this.btn_ImportXml_Click);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(9, 6);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(62, 18);
            this.labelX1.TabIndex = 14;
            this.labelX1.Text = "输出设置:";
            // 
            // btnOutPath
            // 
            this.btnOutPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOutPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOutPath.Location = new System.Drawing.Point(604, 6);
            this.btnOutPath.Name = "btnOutPath";
            this.btnOutPath.Size = new System.Drawing.Size(37, 21);
            this.btnOutPath.TabIndex = 13;
            this.btnOutPath.Text = "...";
            this.btnOutPath.Click += new System.EventHandler(this.btnOutPath_Click);
            // 
            // txtPath
            // 
            this.txtPath.BackColor = System.Drawing.SystemColors.InactiveBorder;
            // 
            // 
            // 
            this.txtPath.Border.Class = "TextBoxBorder";
            this.txtPath.Enabled = false;
            this.txtPath.Location = new System.Drawing.Point(70, 5);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(530, 21);
            this.txtPath.TabIndex = 12;
            // 
            // btn_Exit
            // 
            this.btn_Exit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_Exit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_Exit.Location = new System.Drawing.Point(579, 84);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(53, 23);
            this.btn_Exit.TabIndex = 11;
            this.btn_Exit.Text = "退出";
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // btn_ImportData
            // 
            this.btn_ImportData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_ImportData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_ImportData.Location = new System.Drawing.Point(516, 84);
            this.btn_ImportData.Name = "btn_ImportData";
            this.btn_ImportData.Size = new System.Drawing.Size(53, 23);
            this.btn_ImportData.TabIndex = 11;
            this.btn_ImportData.Text = "输出";
            this.btn_ImportData.Click += new System.EventHandler(this.btn_ImportData_Click);
            // 
            // btn_SaveXml
            // 
            this.btn_SaveXml.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_SaveXml.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_SaveXml.Location = new System.Drawing.Point(373, 84);
            this.btn_SaveXml.Name = "btn_SaveXml";
            this.btn_SaveXml.Size = new System.Drawing.Size(85, 23);
            this.btn_SaveXml.TabIndex = 11;
            this.btn_SaveXml.Text = "保存配置";
            this.btn_SaveXml.Click += new System.EventHandler(this.btn_SaveXml_Click);
            // 
            // groupPanelRange
            // 
            this.groupPanelRange.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanelRange.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanelRange.Controls.Add(this.btn_LoadData);
            this.groupPanelRange.Controls.Add(this.labelX4);
            this.groupPanelRange.Controls.Add(this.labelX3);
            this.groupPanelRange.Controls.Add(this.txt_Range);
            this.groupPanelRange.Controls.Add(this.btn_SelectRange);
            this.groupPanelRange.Controls.Add(this.cb_Range);
            this.groupPanelRange.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanelRange.Location = new System.Drawing.Point(0, 0);
            this.groupPanelRange.Name = "groupPanelRange";
            this.groupPanelRange.Size = new System.Drawing.Size(646, 75);
            // 
            // 
            // 
            this.groupPanelRange.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanelRange.Style.BackColorGradientAngle = 90;
            this.groupPanelRange.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanelRange.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelRange.Style.BorderBottomWidth = 1;
            this.groupPanelRange.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanelRange.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelRange.Style.BorderLeftWidth = 1;
            this.groupPanelRange.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelRange.Style.BorderRightWidth = 1;
            this.groupPanelRange.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelRange.Style.BorderTopWidth = 1;
            this.groupPanelRange.Style.CornerDiameter = 4;
            this.groupPanelRange.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanelRange.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanelRange.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanelRange.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanelRange.TabIndex = 2;
            this.groupPanelRange.Text = "范围设置";
            // 
            // btn_LoadData
            // 
            this.btn_LoadData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_LoadData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_LoadData.Location = new System.Drawing.Point(205, 7);
            this.btn_LoadData.Name = "btn_LoadData";
            this.btn_LoadData.Size = new System.Drawing.Size(55, 23);
            this.btn_LoadData.TabIndex = 20;
            this.btn_LoadData.Text = "连接数据";
            this.btn_LoadData.Click += new System.EventHandler(this.btn_LoadData_Click);
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.Location = new System.Drawing.Point(10, 10);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(62, 18);
            this.labelX4.TabIndex = 19;
            this.labelX4.Text = "选择范围:";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(273, 9);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(87, 18);
            this.labelX3.TabIndex = 19;
            this.labelX3.Text = "导入范围路径:";
            // 
            // txt_Range
            // 
            // 
            // 
            // 
            this.txt_Range.Border.Class = "TextBoxBorder";
            this.txt_Range.Location = new System.Drawing.Point(359, 7);
            this.txt_Range.Name = "txt_Range";
            this.txt_Range.ReadOnly = true;
            this.txt_Range.Size = new System.Drawing.Size(241, 21);
            this.txt_Range.TabIndex = 3;
            // 
            // btn_SelectRange
            // 
            this.btn_SelectRange.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_SelectRange.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_SelectRange.Location = new System.Drawing.Point(603, 6);
            this.btn_SelectRange.Name = "btn_SelectRange";
            this.btn_SelectRange.Size = new System.Drawing.Size(31, 23);
            this.btn_SelectRange.TabIndex = 2;
            this.btn_SelectRange.Text = "...";
            this.btn_SelectRange.Click += new System.EventHandler(this.btn_SelectRange_Click);
            // 
            // cb_Range
            // 
            this.cb_Range.DisplayMember = "Text";
            this.cb_Range.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_Range.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Range.FormattingEnabled = true;
            this.cb_Range.ItemHeight = 15;
            this.cb_Range.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3});
            this.cb_Range.Location = new System.Drawing.Point(78, 7);
            this.cb_Range.Name = "cb_Range";
            this.cb_Range.Size = new System.Drawing.Size(128, 21);
            this.cb_Range.TabIndex = 1;
            this.cb_Range.SelectedIndexChanged += new System.EventHandler(this.cb_Range_SelectedIndexChanged);
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "全库范围";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "导入范围";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "任意范围";
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx2.Controls.Add(this.btn_Save);
            this.panelEx2.Controls.Add(this.groupPanelField);
            this.panelEx2.Controls.Add(this.groupPanelLayer);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx2.Location = new System.Drawing.Point(0, 75);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(646, 464);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 11;
            // 
            // btn_Save
            // 
            this.btn_Save.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Save.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_Save.Location = new System.Drawing.Point(578, 428);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(53, 23);
            this.btn_Save.TabIndex = 16;
            this.btn_Save.Text = "保存";
            this.btn_Save.Tooltip = "保存属性结构和过滤条件";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // groupPanelField
            // 
            this.groupPanelField.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanelField.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanelField.Controls.Add(this.pictureBox1);
            this.groupPanelField.Controls.Add(this.txtSQL);
            this.groupPanelField.Controls.Add(this.btn_AddCondition);
            this.groupPanelField.Controls.Add(this.btn_DelCondition);
            this.groupPanelField.Controls.Add(this.labelX2);
            this.groupPanelField.Controls.Add(this.btn_DelField);
            this.groupPanelField.Controls.Add(this.listViewField);
            this.groupPanelField.Controls.Add(this.btn_ModifyField);
            this.groupPanelField.Controls.Add(this.btn_AddField);
            this.groupPanelField.Controls.Add(this.btn_AllcheckField);
            this.groupPanelField.Controls.Add(this.btn_RecheckField);
            this.groupPanelField.Location = new System.Drawing.Point(355, 2);
            this.groupPanelField.Name = "groupPanelField";
            this.groupPanelField.Size = new System.Drawing.Size(298, 415);
            // 
            // 
            // 
            this.groupPanelField.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanelField.Style.BackColorGradientAngle = 90;
            this.groupPanelField.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanelField.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelField.Style.BorderBottomWidth = 1;
            this.groupPanelField.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanelField.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelField.Style.BorderLeftWidth = 1;
            this.groupPanelField.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelField.Style.BorderRightWidth = 1;
            this.groupPanelField.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelField.Style.BorderTopWidth = 1;
            this.groupPanelField.Style.CornerDiameter = 4;
            this.groupPanelField.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanelField.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanelField.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanelField.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanelField.TabIndex = 14;
            this.groupPanelField.Text = "属性匹配与过滤条件";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(1, 314);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(286, 1);
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // txtSQL
            // 
            // 
            // 
            // 
            this.txtSQL.Border.Class = "TextBoxBorder";
            this.txtSQL.Location = new System.Drawing.Point(70, 330);
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.ReadOnly = true;
            this.txtSQL.Size = new System.Drawing.Size(207, 21);
            this.txtSQL.TabIndex = 24;
            // 
            // btn_AddCondition
            // 
            this.btn_AddCondition.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_AddCondition.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_AddCondition.Location = new System.Drawing.Point(161, 366);
            this.btn_AddCondition.Name = "btn_AddCondition";
            this.btn_AddCondition.Size = new System.Drawing.Size(53, 23);
            this.btn_AddCondition.TabIndex = 23;
            this.btn_AddCondition.Text = "添加";
            this.btn_AddCondition.Click += new System.EventHandler(this.btn_AddCondition_Click);
            // 
            // btn_DelCondition
            // 
            this.btn_DelCondition.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_DelCondition.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_DelCondition.Location = new System.Drawing.Point(220, 366);
            this.btn_DelCondition.Name = "btn_DelCondition";
            this.btn_DelCondition.Size = new System.Drawing.Size(53, 23);
            this.btn_DelCondition.TabIndex = 21;
            this.btn_DelCondition.Text = "清除";
            this.btn_DelCondition.Click += new System.EventHandler(this.btn_DelCondition_Click);
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(15, 331);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(62, 18);
            this.labelX2.TabIndex = 22;
            this.labelX2.Text = "过滤条件:";
            // 
            // btn_DelField
            // 
            this.btn_DelField.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_DelField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_DelField.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_DelField.Location = new System.Drawing.Point(124, 280);
            this.btn_DelField.Name = "btn_DelField";
            this.btn_DelField.Size = new System.Drawing.Size(53, 23);
            this.btn_DelField.TabIndex = 19;
            this.btn_DelField.Text = "删除";
            this.btn_DelField.Visible = false;
            // 
            // listViewField
            // 
            // 
            // 
            // 
            this.listViewField.Border.Class = "ListViewBorder";
            this.listViewField.CheckBoxes = true;
            this.listViewField.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.listViewField.Dock = System.Windows.Forms.DockStyle.Top;
            this.listViewField.FullRowSelect = true;
            this.listViewField.GridLines = true;
            this.listViewField.Location = new System.Drawing.Point(0, 0);
            this.listViewField.Name = "listViewField";
            this.listViewField.Size = new System.Drawing.Size(292, 275);
            this.listViewField.TabIndex = 9;
            this.listViewField.UseCompatibleStateImageBehavior = false;
            this.listViewField.View = System.Windows.Forms.View.Details;
            this.listViewField.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewField_MouseDoubleClick);
            this.listViewField.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewField_ItemChecked);
            this.listViewField.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewField_ColumnClick);
            this.listViewField.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewField_MouseDown);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "源字段";
            this.columnHeader2.Width = 151;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "目标字段";
            this.columnHeader3.Width = 178;
            // 
            // btn_ModifyField
            // 
            this.btn_ModifyField.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_ModifyField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ModifyField.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_ModifyField.Location = new System.Drawing.Point(232, 281);
            this.btn_ModifyField.Name = "btn_ModifyField";
            this.btn_ModifyField.Size = new System.Drawing.Size(53, 23);
            this.btn_ModifyField.TabIndex = 16;
            this.btn_ModifyField.Text = "修改";
            this.btn_ModifyField.Click += new System.EventHandler(this.btn_ModifyField_Click);
            // 
            // btn_AddField
            // 
            this.btn_AddField.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_AddField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_AddField.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_AddField.Location = new System.Drawing.Point(177, 281);
            this.btn_AddField.Name = "btn_AddField";
            this.btn_AddField.Size = new System.Drawing.Size(53, 23);
            this.btn_AddField.TabIndex = 20;
            this.btn_AddField.Text = "增加";
            this.btn_AddField.Click += new System.EventHandler(this.btn_AddField_Click);
            // 
            // btn_AllcheckField
            // 
            this.btn_AllcheckField.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_AllcheckField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_AllcheckField.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_AllcheckField.Location = new System.Drawing.Point(13, 281);
            this.btn_AllcheckField.Name = "btn_AllcheckField";
            this.btn_AllcheckField.Size = new System.Drawing.Size(53, 23);
            this.btn_AllcheckField.TabIndex = 18;
            this.btn_AllcheckField.Text = "全选";
            this.btn_AllcheckField.Click += new System.EventHandler(this.btn_AllcheckField_Click);
            // 
            // btn_RecheckField
            // 
            this.btn_RecheckField.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_RecheckField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_RecheckField.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_RecheckField.Location = new System.Drawing.Point(68, 281);
            this.btn_RecheckField.Name = "btn_RecheckField";
            this.btn_RecheckField.Size = new System.Drawing.Size(53, 23);
            this.btn_RecheckField.TabIndex = 17;
            this.btn_RecheckField.Text = "反选";
            this.btn_RecheckField.Click += new System.EventHandler(this.btn_RecheckField_Click);
            // 
            // groupPanelLayer
            // 
            this.groupPanelLayer.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanelLayer.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanelLayer.Controls.Add(this.checkBox_Cut);
            this.groupPanelLayer.Controls.Add(this.btn_DelLayer);
            this.groupPanelLayer.Controls.Add(this.btn_ModifyLayer);
            this.groupPanelLayer.Controls.Add(this.btn_AddLayer);
            this.groupPanelLayer.Controls.Add(this.btn_RecheckLayer);
            this.groupPanelLayer.Controls.Add(this.btn_AllcheckLayer);
            this.groupPanelLayer.Controls.Add(this.listViewFeaCls);
            this.groupPanelLayer.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupPanelLayer.Location = new System.Drawing.Point(0, 0);
            this.groupPanelLayer.Name = "groupPanelLayer";
            this.groupPanelLayer.Size = new System.Drawing.Size(352, 464);
            // 
            // 
            // 
            this.groupPanelLayer.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanelLayer.Style.BackColorGradientAngle = 90;
            this.groupPanelLayer.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanelLayer.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelLayer.Style.BorderBottomWidth = 1;
            this.groupPanelLayer.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanelLayer.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelLayer.Style.BorderLeftWidth = 1;
            this.groupPanelLayer.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelLayer.Style.BorderRightWidth = 1;
            this.groupPanelLayer.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelLayer.Style.BorderTopWidth = 1;
            this.groupPanelLayer.Style.CornerDiameter = 4;
            this.groupPanelLayer.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanelLayer.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanelLayer.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanelLayer.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanelLayer.TabIndex = 12;
            this.groupPanelLayer.Text = "图层匹配";
            // 
            // checkBox_Cut
            // 
            this.checkBox_Cut.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_Cut.Location = new System.Drawing.Point(2, 409);
            this.checkBox_Cut.Name = "checkBox_Cut";
            this.checkBox_Cut.Size = new System.Drawing.Size(79, 23);
            this.checkBox_Cut.TabIndex = 17;
            this.checkBox_Cut.Text = "是否裁剪";
            // 
            // btn_DelLayer
            // 
            this.btn_DelLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_DelLayer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_DelLayer.Location = new System.Drawing.Point(291, 407);
            this.btn_DelLayer.Name = "btn_DelLayer";
            this.btn_DelLayer.Size = new System.Drawing.Size(39, 23);
            this.btn_DelLayer.TabIndex = 10;
            this.btn_DelLayer.Text = "删除";
            this.btn_DelLayer.Click += new System.EventHandler(this.btn_DelLayer_Click);
            // 
            // btn_ModifyLayer
            // 
            this.btn_ModifyLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_ModifyLayer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_ModifyLayer.Location = new System.Drawing.Point(248, 407);
            this.btn_ModifyLayer.Name = "btn_ModifyLayer";
            this.btn_ModifyLayer.Size = new System.Drawing.Size(39, 23);
            this.btn_ModifyLayer.TabIndex = 10;
            this.btn_ModifyLayer.Text = "修改";
            this.btn_ModifyLayer.Click += new System.EventHandler(this.btn_ModifyLayer_Click);
            // 
            // btn_AddLayer
            // 
            this.btn_AddLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_AddLayer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_AddLayer.Location = new System.Drawing.Point(205, 407);
            this.btn_AddLayer.Name = "btn_AddLayer";
            this.btn_AddLayer.Size = new System.Drawing.Size(39, 23);
            this.btn_AddLayer.TabIndex = 10;
            this.btn_AddLayer.Text = "增加";
            this.btn_AddLayer.Click += new System.EventHandler(this.btn_AddLayer_Click);
            // 
            // btn_RecheckLayer
            // 
            this.btn_RecheckLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_RecheckLayer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_RecheckLayer.Location = new System.Drawing.Point(129, 407);
            this.btn_RecheckLayer.Name = "btn_RecheckLayer";
            this.btn_RecheckLayer.Size = new System.Drawing.Size(39, 23);
            this.btn_RecheckLayer.TabIndex = 10;
            this.btn_RecheckLayer.Text = "反选";
            this.btn_RecheckLayer.Click += new System.EventHandler(this.btn_RecheckLayer_Click);
            // 
            // btn_AllcheckLayer
            // 
            this.btn_AllcheckLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_AllcheckLayer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_AllcheckLayer.Location = new System.Drawing.Point(87, 407);
            this.btn_AllcheckLayer.Name = "btn_AllcheckLayer";
            this.btn_AllcheckLayer.Size = new System.Drawing.Size(39, 23);
            this.btn_AllcheckLayer.TabIndex = 10;
            this.btn_AllcheckLayer.Text = "全选";
            this.btn_AllcheckLayer.Click += new System.EventHandler(this.btn_AllcheckLayer_Click);
            // 
            // listViewFeaCls
            // 
            // 
            // 
            // 
            this.listViewFeaCls.Border.Class = "ListViewBorder";
            this.listViewFeaCls.CheckBoxes = true;
            this.listViewFeaCls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colText,
            this.columnHeader1});
            this.listViewFeaCls.ContextMenuStrip = this.contextMenuLayer;
            this.listViewFeaCls.Dock = System.Windows.Forms.DockStyle.Top;
            this.listViewFeaCls.FullRowSelect = true;
            this.listViewFeaCls.GridLines = true;
            this.listViewFeaCls.Location = new System.Drawing.Point(0, 0);
            this.listViewFeaCls.Name = "listViewFeaCls";
            this.listViewFeaCls.Size = new System.Drawing.Size(346, 400);
            this.listViewFeaCls.TabIndex = 9;
            this.listViewFeaCls.UseCompatibleStateImageBehavior = false;
            this.listViewFeaCls.View = System.Windows.Forms.View.Details;
            this.listViewFeaCls.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewFeaCls_MouseDoubleClick);
            this.listViewFeaCls.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewFeaCls_ItemChecked);
            this.listViewFeaCls.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewFeaCls_MouseClick);
            this.listViewFeaCls.SelectedIndexChanged += new System.EventHandler(this.listViewFeaCls_SelectedIndexChanged);
            this.listViewFeaCls.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewFeaCls_ColumnClick);
            this.listViewFeaCls.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewFeaCls_MouseDown);
            this.listViewFeaCls.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewFeaCls_KeyDown);
            // 
            // colText
            // 
            this.colText.Text = "源图层";
            this.colText.Width = 178;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "目标图层";
            this.columnHeader1.Width = 173;
            // 
            // contextMenuLayer
            // 
            this.contextMenuLayer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemCopy,
            this.ToolStripMenuItemPast});
            this.contextMenuLayer.Name = "contextMenuLayer";
            this.contextMenuLayer.Size = new System.Drawing.Size(165, 48);
            // 
            // ToolStripMenuItemCopy
            // 
            this.ToolStripMenuItemCopy.Name = "ToolStripMenuItemCopy";
            this.ToolStripMenuItemCopy.Size = new System.Drawing.Size(164, 22);
            this.ToolStripMenuItemCopy.Text = "复制属性结构(&C)";
            this.ToolStripMenuItemCopy.Click += new System.EventHandler(this.ToolStripMenuItemCopy_Click);
            // 
            // ToolStripMenuItemPast
            // 
            this.ToolStripMenuItemPast.Name = "ToolStripMenuItemPast";
            this.ToolStripMenuItemPast.Size = new System.Drawing.Size(164, 22);
            this.ToolStripMenuItemPast.Text = "粘贴属性结构(&V)";
            this.ToolStripMenuItemPast.Click += new System.EventHandler(this.ToolStripMenuItemPast_Click);
            // 
            // frmCustomExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 669);
            this.Controls.Add(this.panelEx2);
            this.Controls.Add(this.groupPanelRange);
            this.Controls.Add(this.panelEx3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmCustomExport";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自定义导出";
            this.Load += new System.EventHandler(this.frmCustomExport_Load);
            this.panelEx3.ResumeLayout(false);
            this.panelEx3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.groupPanelRange.ResumeLayout(false);
            this.groupPanelRange.PerformLayout();
            this.panelEx2.ResumeLayout(false);
            this.groupPanelField.ResumeLayout(false);
            this.groupPanelField.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupPanelLayer.ResumeLayout(false);
            this.contextMenuLayer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx3;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanelRange;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.ButtonX btn_SaveXml;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanelField;
        private DevComponents.DotNetBar.Controls.ListViewEx listViewField;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanelLayer;
        private DevComponents.DotNetBar.ButtonX btn_DelLayer;
        private DevComponents.DotNetBar.ButtonX btn_AddLayer;
        private DevComponents.DotNetBar.ButtonX btn_RecheckLayer;
        private DevComponents.DotNetBar.ButtonX btn_AllcheckLayer;
        private DevComponents.DotNetBar.Controls.ListViewEx listViewFeaCls;
        private DevComponents.DotNetBar.ButtonX btn_Exit;
        private DevComponents.DotNetBar.ButtonX btn_ImportData;
        private DevComponents.DotNetBar.ButtonX btn_ImportXml;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnOutPath;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPath;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        public System.Windows.Forms.ColumnHeader columnHeader1;
        public System.Windows.Forms.ColumnHeader colText;
        private DevComponents.DotNetBar.ButtonX btn_ModifyLayer;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarLayer;
        private DevComponents.DotNetBar.LabelX labelprogress;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarField;
        private DevComponents.DotNetBar.LabelX labelDetail;
        private System.Windows.Forms.ContextMenuStrip contextMenuLayer;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPast;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_Range;
        private DevComponents.DotNetBar.ButtonX btn_SelectRange;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_Range;
        private DevComponents.DotNetBar.ButtonX btn_LoadData;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.DotNetBar.ButtonX btn_DelField;
        private DevComponents.DotNetBar.ButtonX btn_ModifyField;
        private DevComponents.DotNetBar.ButtonX btn_AddField;
        private DevComponents.DotNetBar.ButtonX btn_AllcheckField;
        private DevComponents.DotNetBar.ButtonX btn_RecheckField;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSQL;
        private DevComponents.DotNetBar.ButtonX btn_AddCondition;
        private DevComponents.DotNetBar.ButtonX btn_DelCondition;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btn_Save;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBox_Cut;
        private DevComponents.DotNetBar.ButtonX btnCheck;




    }
}

