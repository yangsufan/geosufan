namespace GeoDataExport
{
    partial class frmExport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btn_Clear = new DevComponents.DotNetBar.ButtonX();
            this.labelCut = new DevComponents.DotNetBar.LabelX();
            this.labelSelect = new DevComponents.DotNetBar.LabelX();
            this.btnCutNotSel = new DevComponents.DotNetBar.ButtonX();
            this.btnCutSelAll = new DevComponents.DotNetBar.ButtonX();
            this.btnjiancai = new DevComponents.DotNetBar.ButtonX();
            this.btnNotSel = new DevComponents.DotNetBar.ButtonX();
            this.btnSelAll = new DevComponents.DotNetBar.ButtonX();
            this.dataGridLayers = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colChecked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colchecked1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDatasetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLyrName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnScale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWhere = new System.Windows.Forms.DataGridViewButtonColumn();
            this.progressStep = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.chkTrans = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cmdCoorSet = new DevComponents.DotNetBar.ButtonX();
            this.btnOutPath = new DevComponents.DotNetBar.ButtonX();
            this.txtPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.lblTips = new DevComponents.DotNetBar.LabelX();
            this.chckBoxDOM = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chckBoxDEM = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chckBoxDLG = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.chckBoxEDOM = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chckBoxEDEM = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chckBoxEDLG = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btn_Select = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLayers)).BeginInit();
            this.groupPanel2.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btn_Clear);
            this.groupPanel1.Controls.Add(this.labelCut);
            this.groupPanel1.Controls.Add(this.labelSelect);
            this.groupPanel1.Controls.Add(this.btnCutNotSel);
            this.groupPanel1.Controls.Add(this.btnCutSelAll);
            this.groupPanel1.Controls.Add(this.btnjiancai);
            this.groupPanel1.Controls.Add(this.btnNotSel);
            this.groupPanel1.Controls.Add(this.btnSelAll);
            this.groupPanel1.Controls.Add(this.dataGridLayers);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(12, 38);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(704, 362);
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
            this.groupPanel1.Text = "图层设置";
            // 
            // btn_Clear
            // 
            this.btn_Clear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_Clear.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_Clear.Location = new System.Drawing.Point(609, 315);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(80, 21);
            this.btn_Clear.TabIndex = 10;
            this.btn_Clear.Text = "清空列表";
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // labelCut
            // 
            this.labelCut.AutoSize = true;
            this.labelCut.BackColor = System.Drawing.Color.Transparent;
            this.labelCut.Location = new System.Drawing.Point(120, 315);
            this.labelCut.Name = "labelCut";
            this.labelCut.Size = new System.Drawing.Size(37, 18);
            this.labelCut.TabIndex = 9;
            this.labelCut.Text = "剪裁:";
            // 
            // labelSelect
            // 
            this.labelSelect.AutoSize = true;
            this.labelSelect.BackColor = System.Drawing.Color.Transparent;
            this.labelSelect.Location = new System.Drawing.Point(0, 316);
            this.labelSelect.Name = "labelSelect";
            this.labelSelect.Size = new System.Drawing.Size(37, 18);
            this.labelSelect.TabIndex = 9;
            this.labelSelect.Text = "选择:";
            // 
            // btnCutNotSel
            // 
            this.btnCutNotSel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCutNotSel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCutNotSel.Location = new System.Drawing.Point(197, 313);
            this.btnCutNotSel.Name = "btnCutNotSel";
            this.btnCutNotSel.Size = new System.Drawing.Size(37, 22);
            this.btnCutNotSel.TabIndex = 8;
            this.btnCutNotSel.Text = "反选";
            this.btnCutNotSel.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // btnCutSelAll
            // 
            this.btnCutSelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCutSelAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCutSelAll.Location = new System.Drawing.Point(157, 313);
            this.btnCutSelAll.Name = "btnCutSelAll";
            this.btnCutSelAll.Size = new System.Drawing.Size(37, 22);
            this.btnCutSelAll.TabIndex = 7;
            this.btnCutSelAll.Text = "全选";
            this.btnCutSelAll.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // btnjiancai
            // 
            this.btnjiancai.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnjiancai.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnjiancai.Location = new System.Drawing.Point(514, 316);
            this.btnjiancai.Name = "btnjiancai";
            this.btnjiancai.Size = new System.Drawing.Size(80, 21);
            this.btnjiancai.TabIndex = 6;
            this.btnjiancai.Text = "选择剪裁范围";
            this.btnjiancai.Visible = false;
            this.btnjiancai.Click += new System.EventHandler(this.btnjiancai_Click);
            // 
            // btnNotSel
            // 
            this.btnNotSel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNotSel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnNotSel.Location = new System.Drawing.Point(76, 313);
            this.btnNotSel.Name = "btnNotSel";
            this.btnNotSel.Size = new System.Drawing.Size(37, 22);
            this.btnNotSel.TabIndex = 2;
            this.btnNotSel.Text = "反选";
            this.btnNotSel.Click += new System.EventHandler(this.btnNotSel_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelAll.Location = new System.Drawing.Point(37, 313);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(37, 22);
            this.btnSelAll.TabIndex = 1;
            this.btnSelAll.Text = "全选";
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // dataGridLayers
            // 
            this.dataGridLayers.AllowUserToAddRows = false;
            this.dataGridLayers.AllowUserToOrderColumns = true;
            this.dataGridLayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridLayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChecked,
            this.colchecked1,
            this.colDatasetName,
            this.colLyrName,
            this.ColumnScale,
            this.ColumnDataType,
            this.colWhere});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridLayers.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridLayers.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridLayers.Location = new System.Drawing.Point(3, 3);
            this.dataGridLayers.Name = "dataGridLayers";
            this.dataGridLayers.RowTemplate.Height = 23;
            this.dataGridLayers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridLayers.Size = new System.Drawing.Size(686, 308);
            this.dataGridLayers.TabIndex = 0;
            this.dataGridLayers.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridLayers_CellMouseClick);
            this.dataGridLayers.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridLayers_CellMouseDown);
            // 
            // colChecked
            // 
            this.colChecked.HeaderText = "选择";
            this.colChecked.Name = "colChecked";
            this.colChecked.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colChecked.Width = 55;
            // 
            // colchecked1
            // 
            this.colchecked1.HeaderText = "剪裁";
            this.colchecked1.Name = "colchecked1";
            this.colchecked1.Width = 50;
            // 
            // colDatasetName
            // 
            this.colDatasetName.HeaderText = "要素类名称";
            this.colDatasetName.Name = "colDatasetName";
            this.colDatasetName.Width = 200;
            // 
            // colLyrName
            // 
            this.colLyrName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLyrName.HeaderText = "图层名称";
            this.colLyrName.Name = "colLyrName";
            // 
            // ColumnScale
            // 
            this.ColumnScale.HeaderText = "比例尺";
            this.ColumnScale.Name = "ColumnScale";
            this.ColumnScale.Visible = false;
            // 
            // ColumnDataType
            // 
            this.ColumnDataType.HeaderText = "数据类型";
            this.ColumnDataType.Name = "ColumnDataType";
            this.ColumnDataType.Visible = false;
            // 
            // colWhere
            // 
            this.colWhere.HeaderText = "过滤条件";
            this.colWhere.Name = "colWhere";
            this.colWhere.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colWhere.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colWhere.Width = 130;
            // 
            // progressStep
            // 
            this.progressStep.BackColor = System.Drawing.Color.White;
            this.progressStep.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.progressStep.Location = new System.Drawing.Point(3, 530);
            this.progressStep.Name = "progressStep";
            this.progressStep.Size = new System.Drawing.Size(719, 12);
            this.progressStep.TabIndex = 1;
            this.progressStep.Visible = false;
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.chkTrans);
            this.groupPanel2.Controls.Add(this.cmdCoorSet);
            this.groupPanel2.Controls.Add(this.btnOutPath);
            this.groupPanel2.Controls.Add(this.txtPath);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(12, 405);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(704, 86);
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
            this.groupPanel2.TabIndex = 2;
            this.groupPanel2.Text = "输出设置";
            // 
            // chkTrans
            // 
            this.chkTrans.BackColor = System.Drawing.Color.Transparent;
            this.chkTrans.Location = new System.Drawing.Point(15, 45);
            this.chkTrans.Name = "chkTrans";
            this.chkTrans.Size = new System.Drawing.Size(115, 18);
            this.chkTrans.TabIndex = 7;
            this.chkTrans.Text = "进行坐标变换";
            this.chkTrans.Visible = false;
            this.chkTrans.CheckedChanged += new System.EventHandler(this.chkTrans_CheckedChanged);
            // 
            // cmdCoorSet
            // 
            this.cmdCoorSet.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cmdCoorSet.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cmdCoorSet.Location = new System.Drawing.Point(135, 41);
            this.cmdCoorSet.Name = "cmdCoorSet";
            this.cmdCoorSet.Size = new System.Drawing.Size(68, 23);
            this.cmdCoorSet.TabIndex = 6;
            this.cmdCoorSet.Text = "设置";
            this.cmdCoorSet.Visible = false;
            this.cmdCoorSet.Click += new System.EventHandler(this.cmdCoorSet_Click);
            // 
            // btnOutPath
            // 
            this.btnOutPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOutPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOutPath.Location = new System.Drawing.Point(638, 20);
            this.btnOutPath.Name = "btnOutPath";
            this.btnOutPath.Size = new System.Drawing.Size(37, 21);
            this.btnOutPath.TabIndex = 4;
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
            this.txtPath.Location = new System.Drawing.Point(19, 20);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(613, 21);
            this.txtPath.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(570, 503);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 24);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(646, 503);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTips
            // 
            this.lblTips.Location = new System.Drawing.Point(12, 502);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(377, 24);
            this.lblTips.TabIndex = 5;
            this.lblTips.Text = "进行提取设置";
            // 
            // chckBoxDOM
            // 
            this.chckBoxDOM.BackColor = System.Drawing.Color.Transparent;
            this.chckBoxDOM.Location = new System.Drawing.Point(231, 5);
            this.chckBoxDOM.Name = "chckBoxDOM";
            this.chckBoxDOM.Size = new System.Drawing.Size(46, 23);
            this.chckBoxDOM.TabIndex = 3;
            this.chckBoxDOM.Text = "DOM";
            this.chckBoxDOM.CheckedChanged += new System.EventHandler(this.chckBoxDOM_CheckedChanged);
            // 
            // chckBoxDEM
            // 
            this.chckBoxDEM.BackColor = System.Drawing.Color.Transparent;
            this.chckBoxDEM.Location = new System.Drawing.Point(172, 5);
            this.chckBoxDEM.Name = "chckBoxDEM";
            this.chckBoxDEM.Size = new System.Drawing.Size(46, 23);
            this.chckBoxDEM.TabIndex = 2;
            this.chckBoxDEM.Text = "DEM";
            this.chckBoxDEM.CheckedChanged += new System.EventHandler(this.chckBoxDEM_CheckedChanged);
            // 
            // chckBoxDLG
            // 
            this.chckBoxDLG.BackColor = System.Drawing.Color.Transparent;
            this.chckBoxDLG.Location = new System.Drawing.Point(114, 6);
            this.chckBoxDLG.Name = "chckBoxDLG";
            this.chckBoxDLG.Size = new System.Drawing.Size(46, 23);
            this.chckBoxDLG.TabIndex = 1;
            this.chckBoxDLG.Text = "DLG";
            this.chckBoxDLG.CheckedChanged += new System.EventHandler(this.chckBoxDLG_CheckedChanged);
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.labelX2);
            this.groupPanel3.Controls.Add(this.labelX1);
            this.groupPanel3.Controls.Add(this.chckBoxEDOM);
            this.groupPanel3.Controls.Add(this.chckBoxEDEM);
            this.groupPanel3.Controls.Add(this.chckBoxEDLG);
            this.groupPanel3.Controls.Add(this.chckBoxDOM);
            this.groupPanel3.Controls.Add(this.chckBoxDEM);
            this.groupPanel3.Controls.Add(this.chckBoxDLG);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(12, 1);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(704, 14);
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
            this.groupPanel3.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel3.TabIndex = 8;
            this.groupPanel3.Text = "提取条件设置";
            this.groupPanel3.Visible = false;
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(19, 7);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(87, 23);
            this.labelX2.TabIndex = 8;
            this.labelX2.Text = "1:5万比例尺：";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(419, 9);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(96, 23);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "1:25万比例尺：";
            // 
            // chckBoxEDOM
            // 
            this.chckBoxEDOM.BackColor = System.Drawing.Color.Transparent;
            this.chckBoxEDOM.Location = new System.Drawing.Point(632, 7);
            this.chckBoxEDOM.Name = "chckBoxEDOM";
            this.chckBoxEDOM.Size = new System.Drawing.Size(46, 23);
            this.chckBoxEDOM.TabIndex = 6;
            this.chckBoxEDOM.Text = "DOM";
            this.chckBoxEDOM.CheckedChanged += new System.EventHandler(this.chckBoxEDOM_CheckedChanged);
            // 
            // chckBoxEDEM
            // 
            this.chckBoxEDEM.BackColor = System.Drawing.Color.Transparent;
            this.chckBoxEDEM.Location = new System.Drawing.Point(570, 7);
            this.chckBoxEDEM.Name = "chckBoxEDEM";
            this.chckBoxEDEM.Size = new System.Drawing.Size(46, 23);
            this.chckBoxEDEM.TabIndex = 5;
            this.chckBoxEDEM.Text = "DEM";
            this.chckBoxEDEM.CheckedChanged += new System.EventHandler(this.chckBoxEDEM_CheckedChanged);
            // 
            // chckBoxEDLG
            // 
            this.chckBoxEDLG.BackColor = System.Drawing.Color.Transparent;
            this.chckBoxEDLG.Location = new System.Drawing.Point(514, 7);
            this.chckBoxEDLG.Name = "chckBoxEDLG";
            this.chckBoxEDLG.Size = new System.Drawing.Size(46, 23);
            this.chckBoxEDLG.TabIndex = 4;
            this.chckBoxEDLG.Text = "DLG";
            this.chckBoxEDLG.CheckedChanged += new System.EventHandler(this.chckBoxEDLG_CheckedChanged);
            // 
            // btn_Select
            // 
            this.btn_Select.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_Select.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_Select.Location = new System.Drawing.Point(15, 5);
            this.btn_Select.Name = "btn_Select";
            this.btn_Select.Size = new System.Drawing.Size(113, 27);
            this.btn_Select.TabIndex = 9;
            this.btn_Select.Text = "选择提取图层";
            this.btn_Select.Tooltip = "弹出目录树选择提取图层";
            this.btn_Select.Click += new System.EventHandler(this.btn_Select_Click);
            // 
            // frmExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 543);
            this.Controls.Add(this.btn_Select);
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.lblTips);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.progressStep);
            this.Controls.Add(this.groupPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExport";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据提取";
            this.Load += new System.EventHandler(this.frmExport_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLayers)).EndInit();
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressStep;
        private DevComponents.DotNetBar.ButtonX btnNotSel;
        private DevComponents.DotNetBar.ButtonX btnSelAll;
        public DevComponents.DotNetBar.Controls.DataGridViewX dataGridLayers;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.LabelX lblTips;
        private DevComponents.DotNetBar.ButtonX btnOutPath;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPath;
        private DevComponents.DotNetBar.ButtonX btnjiancai;
        private DevComponents.DotNetBar.ButtonX btnCutNotSel;
        private DevComponents.DotNetBar.ButtonX btnCutSelAll;
        private DevComponents.DotNetBar.ButtonX cmdCoorSet;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkTrans;
        private DevComponents.DotNetBar.Controls.CheckBoxX chckBoxDOM;
        private DevComponents.DotNetBar.Controls.CheckBoxX chckBoxDEM;
        private DevComponents.DotNetBar.Controls.CheckBoxX chckBoxDLG;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChecked;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colchecked1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDatasetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLyrName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnScale;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataType;
        private System.Windows.Forms.DataGridViewButtonColumn colWhere;
        private DevComponents.DotNetBar.Controls.CheckBoxX chckBoxEDOM;
        private DevComponents.DotNetBar.Controls.CheckBoxX chckBoxEDEM;
        private DevComponents.DotNetBar.Controls.CheckBoxX chckBoxEDLG;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelCut;
        private DevComponents.DotNetBar.LabelX labelSelect;
        private DevComponents.DotNetBar.ButtonX btn_Select;
        private DevComponents.DotNetBar.ButtonX btn_Clear;
    }
}