namespace GeoDataExport
{
    partial class frmExportEx
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExportEx));
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelErr = new DevComponents.DotNetBar.LabelX();
            this.btn_TbCatalog = new DevComponents.DotNetBar.ButtonX();
            this.advTreeLayerList = new DevComponents.AdvTree.AdvTree();
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
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
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayerList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLayers)).BeginInit();
            this.groupPanel2.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.labelErr);
            this.groupPanel1.Controls.Add(this.btn_TbCatalog);
            this.groupPanel1.Controls.Add(this.advTreeLayerList);
            this.groupPanel1.Controls.Add(this.dataGridLayers);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(12, 19);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(370, 381);
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
            // labelErr
            // 
            this.labelErr.Location = new System.Drawing.Point(9, 320);
            this.labelErr.Name = "labelErr";
            this.labelErr.Size = new System.Drawing.Size(1, 1);
            this.labelErr.TabIndex = 16;
            this.labelErr.Text = "labelX3";
            // 
            // btn_TbCatalog
            // 
            this.btn_TbCatalog.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_TbCatalog.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_TbCatalog.Location = new System.Drawing.Point(16, 321);
            this.btn_TbCatalog.Name = "btn_TbCatalog";
            this.btn_TbCatalog.Size = new System.Drawing.Size(90, 23);
            this.btn_TbCatalog.TabIndex = 15;
            this.btn_TbCatalog.Text = "同步展示目录";
            this.btn_TbCatalog.Visible = false;
            this.btn_TbCatalog.Click += new System.EventHandler(this.btn_TbCatalog_Click);
            // 
            // advTreeLayerList
            // 
            this.advTreeLayerList.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeLayerList.AllowDrop = true;
            this.advTreeLayerList.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeLayerList.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeLayerList.DragDropEnabled = false;
            this.advTreeLayerList.Location = new System.Drawing.Point(15, -4);
            this.advTreeLayerList.MultiSelect = true;
            this.advTreeLayerList.Name = "advTreeLayerList";
            this.advTreeLayerList.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.advTreeLayerList.NodesConnector = this.nodeConnector1;
            this.advTreeLayerList.NodeStyle = this.elementStyle1;
            this.advTreeLayerList.PathSeparator = ";";
            this.advTreeLayerList.Size = new System.Drawing.Size(330, 319);
            this.advTreeLayerList.Styles.Add(this.elementStyle1);
            this.advTreeLayerList.TabIndex = 14;
            this.advTreeLayerList.Text = "advTree1";
            this.advTreeLayerList.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeLayerList_NodeDoubleClick);
            this.advTreeLayerList.AfterCheck += new DevComponents.AdvTree.AdvTreeCellEventHandler(this.advTreeLayerList_AfterCheck);
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
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
            this.dataGridLayers.Location = new System.Drawing.Point(351, 3);
            this.dataGridLayers.Name = "dataGridLayers";
            this.dataGridLayers.RowTemplate.Height = 23;
            this.dataGridLayers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridLayers.Size = new System.Drawing.Size(14, 332);
            this.dataGridLayers.TabIndex = 0;
            this.dataGridLayers.Visible = false;
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
            this.progressStep.Location = new System.Drawing.Point(-1, 530);
            this.progressStep.Name = "progressStep";
            this.progressStep.Size = new System.Drawing.Size(390, 13);
            this.progressStep.TabIndex = 1;
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
            this.groupPanel2.Size = new System.Drawing.Size(370, 86);
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
            this.chkTrans.Location = new System.Drawing.Point(15, 41);
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
            this.cmdCoorSet.Location = new System.Drawing.Point(135, 37);
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
            this.btnOutPath.Location = new System.Drawing.Point(324, 12);
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
            this.txtPath.Location = new System.Drawing.Point(19, 12);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(298, 21);
            this.txtPath.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(247, 503);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(317, 503);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTips
            // 
            this.lblTips.Location = new System.Drawing.Point(12, 502);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(232, 24);
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
            this.groupPanel3.Size = new System.Drawing.Size(370, 12);
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
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "earth");
            this.ImageList.Images.SetKeyName(1, "Root");
            this.ImageList.Images.SetKeyName(2, "DIR");
            this.ImageList.Images.SetKeyName(3, "DataDIRHalfOpen");
            this.ImageList.Images.SetKeyName(4, "DataDIRClosed");
            this.ImageList.Images.SetKeyName(5, "DataDIROpen");
            this.ImageList.Images.SetKeyName(6, "Layer");
            this.ImageList.Images.SetKeyName(7, "_annotation");
            this.ImageList.Images.SetKeyName(8, "_Dimension");
            this.ImageList.Images.SetKeyName(9, "_line");
            this.ImageList.Images.SetKeyName(10, "_MultiPatch");
            this.ImageList.Images.SetKeyName(11, "_point");
            this.ImageList.Images.SetKeyName(12, "_polygon");
            this.ImageList.Images.SetKeyName(13, "annotation");
            this.ImageList.Images.SetKeyName(14, "Dimension");
            this.ImageList.Images.SetKeyName(15, "line");
            this.ImageList.Images.SetKeyName(16, "MultiPatch");
            this.ImageList.Images.SetKeyName(17, "point");
            this.ImageList.Images.SetKeyName(18, "polygon");
            this.ImageList.Images.SetKeyName(19, "PublicVersion");
            this.ImageList.Images.SetKeyName(20, "PersonalVersion");
            this.ImageList.Images.SetKeyName(21, "INVISIBLE");
            this.ImageList.Images.SetKeyName(22, "VISIBLE");
            // 
            // frmExportEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 543);
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.lblTips);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.progressStep);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExportEx";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据输出";
            this.Load += new System.EventHandler(this.frmExportEx_Load);
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayerList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLayers)).EndInit();
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressStep;
        public DevComponents.DotNetBar.Controls.DataGridViewX dataGridLayers;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.LabelX lblTips;
        private DevComponents.DotNetBar.ButtonX btnOutPath;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPath;
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
        private DevComponents.DotNetBar.ButtonX btn_TbCatalog;
        private DevComponents.AdvTree.AdvTree advTreeLayerList;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        public System.Windows.Forms.ImageList ImageList;
        private DevComponents.DotNetBar.LabelX labelErr;
    }
}