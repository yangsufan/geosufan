namespace GeoDBIntegration
{
    partial class UserControlDBIntegra
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlDBIntegra));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabTreeControl = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.advTreeProject = new DevComponents.AdvTree.AdvTree();
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.tabProtree = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.tabLayertree = new DevComponents.DotNetBar.TabItem(this.components);
            this.IconContainer = new System.Windows.Forms.ImageList(this.components);
            this.node2 = new DevComponents.AdvTree.Node();
            this.node3 = new DevComponents.AdvTree.Node();
            this.node4 = new DevComponents.AdvTree.Node();
            this.node5 = new DevComponents.AdvTree.Node();
            this.node6 = new DevComponents.AdvTree.Node();
            this.node7 = new DevComponents.AdvTree.Node();
            this.node8 = new DevComponents.AdvTree.Node();
            this.node9 = new DevComponents.AdvTree.Node();
            this.tabMapItem = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabMapControl = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel3 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabDB = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel7 = new DevComponents.DotNetBar.TabControlPanel();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.tabMap = new DevComponents.DotNetBar.TabItem(this.components);
            this.dgConnecInfo = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dgParaInfo = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dgQueryRes = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnMapDB = new DevComponents.DotNetBar.ButtonX();
            this.btnDemDB = new DevComponents.DotNetBar.ButtonX();
            this.btnEntiDB = new DevComponents.DotNetBar.ButtonX();
            this.btnImageDB = new DevComponents.DotNetBar.ButtonX();
            this.bnAddressDB = new DevComponents.DotNetBar.ButtonX();
            this.btnSubjectDB = new DevComponents.DotNetBar.ButtonX();
            this.btnFileDB = new DevComponents.DotNetBar.ButtonX();
            this.btnFeaDB = new DevComponents.DotNetBar.ButtonX();
            this.cmbMapDB = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbDemDB = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbEntiDB = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbImageDB = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbAdressDB = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbFeatureDB = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbSubjectDB = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbFileDB = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.contextDataSource = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemAttr = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.tabTreeControl)).BeginInit();
            this.tabTreeControl.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeProject)).BeginInit();
            this.tabControlPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMapControl)).BeginInit();
            this.tabMapControl.SuspendLayout();
            this.tabControlPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgConnecInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgParaInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgQueryRes)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.contextDataSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabTreeControl
            // 
            this.tabTreeControl.CanReorderTabs = true;
            this.tabTreeControl.Controls.Add(this.tabControlPanel1);
            this.tabTreeControl.Controls.Add(this.tabControlPanel2);
            this.tabTreeControl.Location = new System.Drawing.Point(82, 24);
            this.tabTreeControl.Name = "tabTreeControl";
            this.tabTreeControl.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabTreeControl.SelectedTabIndex = 1;
            this.tabTreeControl.Size = new System.Drawing.Size(221, 364);
            this.tabTreeControl.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Bottom;
            this.tabTreeControl.TabIndex = 0;
            this.tabTreeControl.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabTreeControl.Tabs.Add(this.tabProtree);
            this.tabTreeControl.Tabs.Add(this.tabLayertree);
            this.tabTreeControl.Text = "tabControl1";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.advTreeProject);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(221, 338);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlPanel1.Style.GradientAngle = -90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabProtree;
            // 
            // advTreeProject
            // 
            this.advTreeProject.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeProject.AllowDrop = true;
            this.advTreeProject.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeProject.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTreeProject.Location = new System.Drawing.Point(1, 1);
            this.advTreeProject.Name = "advTreeProject";
            this.advTreeProject.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.advTreeProject.NodesConnector = this.nodeConnector1;
            this.advTreeProject.NodeStyle = this.elementStyle1;
            this.advTreeProject.PathSeparator = ";";
            this.advTreeProject.Size = new System.Drawing.Size(219, 336);
            this.advTreeProject.Styles.Add(this.elementStyle1);
            this.advTreeProject.TabIndex = 0;
            this.advTreeProject.Text = "advTree1";
            this.advTreeProject.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeProject_NodeClick);
            this.advTreeProject.NodeMouseDown += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeProject_NodeMouseDown);
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "数据库管理工具";
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
            // tabProtree
            // 
            this.tabProtree.AttachedControl = this.tabControlPanel1;
            this.tabProtree.Name = "tabProtree";
            this.tabProtree.Text = "数据管理";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this.axTOCControl1);
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(221, 338);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlPanel2.Style.GradientAngle = -90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabLayertree;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOCControl1.Location = new System.Drawing.Point(1, 1);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(219, 336);
            this.axTOCControl1.TabIndex = 0;
            this.axTOCControl1.OnMouseUp += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseUpEventHandler(this.axTOCControl1_OnMouseUp);
            this.axTOCControl1.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl1_OnMouseDown);
            // 
            // tabLayertree
            // 
            this.tabLayertree.AttachedControl = this.tabControlPanel2;
            this.tabLayertree.Name = "tabLayertree";
            this.tabLayertree.Text = "图层显示";
            // 
            // IconContainer
            // 
            this.IconContainer.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IconContainer.ImageStream")));
            this.IconContainer.TransparentColor = System.Drawing.Color.Transparent;
            this.IconContainer.Images.SetKeyName(0, "buttonSystems.png");
            this.IconContainer.Images.SetKeyName(1, "GeoSMPDMain.ControlsAddOutDataBase.png");
            this.IconContainer.Images.SetKeyName(2, "GeoDBATool.ControlsShowDGML.png");
            // 
            // node2
            // 
            this.node2.Name = "node2";
            // 
            // node3
            // 
            this.node3.Name = "node3";
            // 
            // node4
            // 
            this.node4.Name = "node4";
            // 
            // node5
            // 
            this.node5.Name = "node5";
            // 
            // node6
            // 
            this.node6.Expanded = true;
            this.node6.Name = "node6";
            this.node6.TagString = "Database";
            this.node6.Text = "专题要素数据库";
            // 
            // node7
            // 
            this.node7.Expanded = true;
            this.node7.Name = "node7";
            this.node7.TagString = "Database";
            this.node7.Text = "地名数据库";
            // 
            // node8
            // 
            this.node8.Expanded = true;
            this.node8.Name = "node8";
            this.node8.TagString = "Database";
            this.node8.Text = "地理编码数据库";
            // 
            // node9
            // 
            this.node9.Expanded = true;
            this.node9.Name = "node9";
            this.node9.TagString = "Database";
            this.node9.Text = "电子地图数据库";
            // 
            // tabMapItem
            // 
            this.tabMapItem.Name = "tabMapItem";
            this.tabMapItem.Text = "数据视图";
            // 
            // tabMapControl
            // 
            this.tabMapControl.CanReorderTabs = true;
            this.tabMapControl.Controls.Add(this.tabControlPanel3);
            this.tabMapControl.Controls.Add(this.tabControlPanel7);
            this.tabMapControl.Location = new System.Drawing.Point(309, 24);
            this.tabMapControl.Name = "tabMapControl";
            this.tabMapControl.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabMapControl.SelectedTabIndex = 1;
            this.tabMapControl.Size = new System.Drawing.Size(256, 360);
            this.tabMapControl.TabIndex = 3;
            this.tabMapControl.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabMapControl.Tabs.Add(this.tabDB);
            this.tabMapControl.Tabs.Add(this.tabMap);
            this.tabMapControl.Text = "tabControl1";
            // 
            // tabControlPanel3
            // 
            this.tabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel3.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel3.Name = "tabControlPanel3";
            this.tabControlPanel3.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel3.Size = new System.Drawing.Size(256, 334);
            this.tabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel3.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel3.Style.GradientAngle = 90;
            this.tabControlPanel3.TabIndex = 1;
            this.tabControlPanel3.TabItem = this.tabDB;
            // 
            // tabDB
            // 
            this.tabDB.AttachedControl = this.tabControlPanel3;
            this.tabDB.Name = "tabDB";
            this.tabDB.Text = "数据库面板";
            // 
            // tabControlPanel7
            // 
            this.tabControlPanel7.Controls.Add(this.axMapControl1);
            this.tabControlPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel7.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel7.Name = "tabControlPanel7";
            this.tabControlPanel7.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel7.Size = new System.Drawing.Size(256, 334);
            this.tabControlPanel7.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel7.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel7.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel7.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel7.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel7.Style.GradientAngle = 90;
            this.tabControlPanel7.TabIndex = 2;
            this.tabControlPanel7.TabItem = this.tabMap;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(1, 1);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(254, 332);
            this.axMapControl1.TabIndex = 0;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl_OnMouseMove);
            this.axMapControl1.OnAfterDraw += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterDrawEventHandler(this.axMapControl1_OnAfterDraw);
            // 
            // tabMap
            // 
            this.tabMap.AttachedControl = this.tabControlPanel7;
            this.tabMap.Name = "tabMap";
            this.tabMap.Text = "数据视图";
            // 
            // dgConnecInfo
            // 
            this.dgConnecInfo.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgConnecInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgConnecInfo.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgConnecInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgConnecInfo.Location = new System.Drawing.Point(309, 454);
            this.dgConnecInfo.Name = "dgConnecInfo";
            this.dgConnecInfo.RowTemplate.Height = 23;
            this.dgConnecInfo.Size = new System.Drawing.Size(179, 150);
            this.dgConnecInfo.TabIndex = 4;
            // 
            // dgParaInfo
            // 
            this.dgParaInfo.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgParaInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgParaInfo.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgParaInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgParaInfo.Location = new System.Drawing.Point(494, 454);
            this.dgParaInfo.Name = "dgParaInfo";
            this.dgParaInfo.RowTemplate.Height = 23;
            this.dgParaInfo.Size = new System.Drawing.Size(148, 150);
            this.dgParaInfo.TabIndex = 5;
            // 
            // dgQueryRes
            // 
            this.dgQueryRes.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgQueryRes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgQueryRes.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgQueryRes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgQueryRes.Location = new System.Drawing.Point(648, 454);
            this.dgQueryRes.Name = "dgQueryRes";
            this.dgQueryRes.RowTemplate.Height = 23;
            this.dgQueryRes.Size = new System.Drawing.Size(171, 150);
            this.dgQueryRes.TabIndex = 6;
            // 
            // groupPanel1
            // 
            this.groupPanel1.AutoScroll = true;
            this.groupPanel1.BackColor = System.Drawing.SystemColors.Window;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btnMapDB);
            this.groupPanel1.Controls.Add(this.btnDemDB);
            this.groupPanel1.Controls.Add(this.btnEntiDB);
            this.groupPanel1.Controls.Add(this.btnImageDB);
            this.groupPanel1.Controls.Add(this.bnAddressDB);
            this.groupPanel1.Controls.Add(this.btnSubjectDB);
            this.groupPanel1.Controls.Add(this.btnFileDB);
            this.groupPanel1.Controls.Add(this.btnFeaDB);
            this.groupPanel1.Controls.Add(this.cmbMapDB);
            this.groupPanel1.Controls.Add(this.cmbDemDB);
            this.groupPanel1.Controls.Add(this.cmbEntiDB);
            this.groupPanel1.Controls.Add(this.cmbImageDB);
            this.groupPanel1.Controls.Add(this.cmbAdressDB);
            this.groupPanel1.Controls.Add(this.cmbFeatureDB);
            this.groupPanel1.Controls.Add(this.cmbSubjectDB);
            this.groupPanel1.Controls.Add(this.cmbFileDB);
            this.groupPanel1.Location = new System.Drawing.Point(68, 82);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(935, 605);
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
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 0;
            // 
            // btnMapDB
            // 
            this.btnMapDB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnMapDB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnMapDB.Image = ((System.Drawing.Image)(resources.GetObject("btnMapDB.Image")));
            this.btnMapDB.Location = new System.Drawing.Point(391, 388);
            this.btnMapDB.Name = "btnMapDB";
            this.btnMapDB.Size = new System.Drawing.Size(158, 150);
            this.btnMapDB.TabIndex = 24;
            this.btnMapDB.Tooltip = "电子地图数据库";
            this.btnMapDB.Visible = false;
            this.btnMapDB.Click += new System.EventHandler(this.btnMapDB_Click);
            // 
            // btnDemDB
            // 
            this.btnDemDB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDemDB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDemDB.Image = ((System.Drawing.Image)(resources.GetObject("btnDemDB.Image")));
            this.btnDemDB.Location = new System.Drawing.Point(67, 194);
            this.btnDemDB.Name = "btnDemDB";
            this.btnDemDB.Size = new System.Drawing.Size(163, 153);
            this.btnDemDB.TabIndex = 23;
            this.btnDemDB.Tooltip = "高程数据库";
            this.btnDemDB.Visible = false;
            this.btnDemDB.Click += new System.EventHandler(this.btnDemDB_Click);
            // 
            // btnEntiDB
            // 
            this.btnEntiDB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnEntiDB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnEntiDB.Image = ((System.Drawing.Image)(resources.GetObject("btnEntiDB.Image")));
            this.btnEntiDB.Location = new System.Drawing.Point(67, 388);
            this.btnEntiDB.Name = "btnEntiDB";
            this.btnEntiDB.Size = new System.Drawing.Size(141, 150);
            this.btnEntiDB.TabIndex = 22;
            this.btnEntiDB.Tooltip = "地理编码数据库";
            this.btnEntiDB.Visible = false;
            this.btnEntiDB.Click += new System.EventHandler(this.btnEntiDB_Click);
            // 
            // btnImageDB
            // 
            this.btnImageDB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnImageDB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnImageDB.Image = ((System.Drawing.Image)(resources.GetObject("btnImageDB.Image")));
            this.btnImageDB.Location = new System.Drawing.Point(726, 4);
            this.btnImageDB.Name = "btnImageDB";
            this.btnImageDB.Size = new System.Drawing.Size(163, 153);
            this.btnImageDB.TabIndex = 21;
            this.btnImageDB.Tooltip = "影像数据库";
            this.btnImageDB.Visible = false;
            this.btnImageDB.Click += new System.EventHandler(this.btnImageDB_Click);
            // 
            // bnAddressDB
            // 
            this.bnAddressDB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bnAddressDB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bnAddressDB.Image = ((System.Drawing.Image)(resources.GetObject("bnAddressDB.Image")));
            this.bnAddressDB.Location = new System.Drawing.Point(726, 194);
            this.bnAddressDB.Name = "bnAddressDB";
            this.bnAddressDB.Size = new System.Drawing.Size(151, 142);
            this.bnAddressDB.TabIndex = 20;
            this.bnAddressDB.Tooltip = "地名数据库";
            this.bnAddressDB.Visible = false;
            this.bnAddressDB.Click += new System.EventHandler(this.bnAddressDB_Click);
            // 
            // btnSubjectDB
            // 
            this.btnSubjectDB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSubjectDB.BackColor = System.Drawing.Color.Transparent;
            this.btnSubjectDB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSubjectDB.Image = ((System.Drawing.Image)(resources.GetObject("btnSubjectDB.Image")));
            this.btnSubjectDB.Location = new System.Drawing.Point(391, 194);
            this.btnSubjectDB.Name = "btnSubjectDB";
            this.btnSubjectDB.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(17, 2, 2, 2);
            this.btnSubjectDB.Size = new System.Drawing.Size(163, 153);
            this.btnSubjectDB.TabIndex = 19;
            this.btnSubjectDB.Tooltip = "专题要素数据库";
            this.btnSubjectDB.Visible = false;
            this.btnSubjectDB.Click += new System.EventHandler(this.btnSubjectDB_Click);
            // 
            // btnFileDB
            // 
            this.btnFileDB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnFileDB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFileDB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnFileDB.Image = global::GeoDBIntegration.Properties.Resources.email_server;
            this.btnFileDB.Location = new System.Drawing.Point(67, 4);
            this.btnFileDB.Name = "btnFileDB";
            this.btnFileDB.Size = new System.Drawing.Size(163, 153);
            this.btnFileDB.TabIndex = 18;
            this.btnFileDB.Tooltip = "文件数据库";
            this.btnFileDB.Visible = false;
            this.btnFileDB.Click += new System.EventHandler(this.btnFileDB_Click);
            // 
            // btnFeaDB
            // 
            this.btnFeaDB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnFeaDB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnFeaDB.Image = global::GeoDBIntegration.Properties.Resources.database;
            this.btnFeaDB.Location = new System.Drawing.Point(391, 4);
            this.btnFeaDB.Name = "btnFeaDB";
            this.btnFeaDB.Size = new System.Drawing.Size(163, 153);
            this.btnFeaDB.TabIndex = 17;
            this.btnFeaDB.Tooltip = "框架要素库";
            this.btnFeaDB.Visible = false;
            this.btnFeaDB.Click += new System.EventHandler(this.btnFeaDB_Click);
            // 
            // cmbMapDB
            // 
            this.cmbMapDB.DisplayMember = "Text";
            this.cmbMapDB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbMapDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapDB.FormattingEnabled = true;
            this.cmbMapDB.ItemHeight = 15;
            this.cmbMapDB.Location = new System.Drawing.Point(391, 544);
            this.cmbMapDB.Name = "cmbMapDB";
            this.cmbMapDB.Size = new System.Drawing.Size(158, 21);
            this.cmbMapDB.TabIndex = 15;
            this.cmbMapDB.Visible = false;
            this.cmbMapDB.SelectedIndexChanged += new System.EventHandler(this.cmbMapDB_SelectedIndexChanged);
            // 
            // cmbDemDB
            // 
            this.cmbDemDB.DisplayMember = "Text";
            this.cmbDemDB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDemDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDemDB.FormattingEnabled = true;
            this.cmbDemDB.ItemHeight = 15;
            this.cmbDemDB.Location = new System.Drawing.Point(67, 355);
            this.cmbDemDB.Name = "cmbDemDB";
            this.cmbDemDB.Size = new System.Drawing.Size(163, 21);
            this.cmbDemDB.TabIndex = 14;
            this.cmbDemDB.Visible = false;
            this.cmbDemDB.SelectedIndexChanged += new System.EventHandler(this.cmbDemDB_SelectedIndexChanged);
            // 
            // cmbEntiDB
            // 
            this.cmbEntiDB.DisplayMember = "Text";
            this.cmbEntiDB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbEntiDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEntiDB.FormattingEnabled = true;
            this.cmbEntiDB.ItemHeight = 15;
            this.cmbEntiDB.Location = new System.Drawing.Point(67, 544);
            this.cmbEntiDB.Name = "cmbEntiDB";
            this.cmbEntiDB.Size = new System.Drawing.Size(141, 21);
            this.cmbEntiDB.TabIndex = 13;
            this.cmbEntiDB.Visible = false;
            this.cmbEntiDB.SelectedIndexChanged += new System.EventHandler(this.cmbEntiDB_SelectedIndexChanged);
            // 
            // cmbImageDB
            // 
            this.cmbImageDB.DisplayMember = "Text";
            this.cmbImageDB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbImageDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImageDB.FormattingEnabled = true;
            this.cmbImageDB.ItemHeight = 15;
            this.cmbImageDB.Location = new System.Drawing.Point(726, 163);
            this.cmbImageDB.Name = "cmbImageDB";
            this.cmbImageDB.Size = new System.Drawing.Size(163, 21);
            this.cmbImageDB.TabIndex = 12;
            this.cmbImageDB.Visible = false;
            this.cmbImageDB.SelectedIndexChanged += new System.EventHandler(this.cmbImageDB_SelectedIndexChanged);
            // 
            // cmbAdressDB
            // 
            this.cmbAdressDB.DisplayMember = "Text";
            this.cmbAdressDB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAdressDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAdressDB.FormattingEnabled = true;
            this.cmbAdressDB.ItemHeight = 15;
            this.cmbAdressDB.Location = new System.Drawing.Point(726, 342);
            this.cmbAdressDB.Name = "cmbAdressDB";
            this.cmbAdressDB.Size = new System.Drawing.Size(151, 21);
            this.cmbAdressDB.TabIndex = 11;
            this.cmbAdressDB.Visible = false;
            this.cmbAdressDB.SelectedIndexChanged += new System.EventHandler(this.cmbAdressDB_SelectedIndexChanged);
            // 
            // cmbFeatureDB
            // 
            this.cmbFeatureDB.DisplayMember = "Text";
            this.cmbFeatureDB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFeatureDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFeatureDB.FormattingEnabled = true;
            this.cmbFeatureDB.ItemHeight = 15;
            this.cmbFeatureDB.Location = new System.Drawing.Point(391, 163);
            this.cmbFeatureDB.Name = "cmbFeatureDB";
            this.cmbFeatureDB.Size = new System.Drawing.Size(163, 21);
            this.cmbFeatureDB.TabIndex = 10;
            this.cmbFeatureDB.Visible = false;
            this.cmbFeatureDB.SelectedIndexChanged += new System.EventHandler(this.cmbFeatureDB_SelectedIndexChanged);
            // 
            // cmbSubjectDB
            // 
            this.cmbSubjectDB.DisplayMember = "Text";
            this.cmbSubjectDB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSubjectDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubjectDB.FormattingEnabled = true;
            this.cmbSubjectDB.ItemHeight = 15;
            this.cmbSubjectDB.Location = new System.Drawing.Point(391, 355);
            this.cmbSubjectDB.Name = "cmbSubjectDB";
            this.cmbSubjectDB.Size = new System.Drawing.Size(163, 21);
            this.cmbSubjectDB.TabIndex = 9;
            this.cmbSubjectDB.Visible = false;
            this.cmbSubjectDB.SelectedIndexChanged += new System.EventHandler(this.cmbSubjectDB_SelectedIndexChanged);
            // 
            // cmbFileDB
            // 
            this.cmbFileDB.DisplayMember = "Text";
            this.cmbFileDB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFileDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileDB.FormattingEnabled = true;
            this.cmbFileDB.ItemHeight = 15;
            this.cmbFileDB.Location = new System.Drawing.Point(67, 163);
            this.cmbFileDB.Name = "cmbFileDB";
            this.cmbFileDB.Size = new System.Drawing.Size(163, 21);
            this.cmbFileDB.TabIndex = 8;
            this.cmbFileDB.Visible = false;
            this.cmbFileDB.SelectedIndexChanged += new System.EventHandler(this.cmbFileDB_SelectedIndexChanged);
            // 
            // contextDataSource
            // 
            this.contextDataSource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemAttr});
            this.contextDataSource.Name = "contextDataSource";
            this.contextDataSource.Size = new System.Drawing.Size(101, 26);
            // 
            // MenuItemAttr
            // 
            this.MenuItemAttr.Name = "MenuItemAttr";
            this.MenuItemAttr.Size = new System.Drawing.Size(100, 22);
            this.MenuItemAttr.Text = "属性";
            this.MenuItemAttr.Click += new System.EventHandler(this.MenuItemAttr_Click);
            // 
            // UserControlDBIntegra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupPanel1);
            this.Name = "UserControlDBIntegra";
            this.Size = new System.Drawing.Size(1021, 742);
            ((System.ComponentModel.ISupportInitialize)(this.tabTreeControl)).EndInit();
            this.tabTreeControl.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeProject)).EndInit();
            this.tabControlPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMapControl)).EndInit();
            this.tabMapControl.ResumeLayout(false);
            this.tabControlPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgConnecInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgParaInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgQueryRes)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.contextDataSource.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.TabControl tabTreeControl;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.TabItem tabLayertree;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabProtree;
        private DevComponents.AdvTree.AdvTree advTreeProject;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.Node node2;
        private DevComponents.AdvTree.Node node3;
        private DevComponents.AdvTree.Node node4;
        private DevComponents.AdvTree.Node node5;
        private DevComponents.AdvTree.Node node6;
        private DevComponents.AdvTree.Node node7;
        private DevComponents.AdvTree.Node node8;
        private DevComponents.AdvTree.Node node9;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private System.Windows.Forms.ImageList IconContainer;
        private DevComponents.DotNetBar.TabItem tabMapItem;
        private DevComponents.DotNetBar.TabControl tabMapControl;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel3;
        private DevComponents.DotNetBar.TabItem tabDB;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel7;
        private DevComponents.DotNetBar.TabItem tabMap;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgConnecInfo;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgParaInfo;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgQueryRes;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSubjectDB;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbFileDB;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbMapDB;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbDemDB;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbEntiDB;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbImageDB;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbAdressDB;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbFeatureDB;
        private DevComponents.DotNetBar.ButtonX btnFeaDB;
        private DevComponents.DotNetBar.ButtonX btnEntiDB;
        private DevComponents.DotNetBar.ButtonX btnImageDB;
        private DevComponents.DotNetBar.ButtonX bnAddressDB;
        private DevComponents.DotNetBar.ButtonX btnSubjectDB;
        private DevComponents.DotNetBar.ButtonX btnFileDB;
        private DevComponents.DotNetBar.ButtonX btnMapDB;
        private DevComponents.DotNetBar.ButtonX btnDemDB;
        private System.Windows.Forms.ContextMenuStrip contextDataSource;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAttr;

    }
}
