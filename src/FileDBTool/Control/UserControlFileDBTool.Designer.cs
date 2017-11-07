namespace FileDBTool
{
    partial class UserControlFileDBTool
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlFileDBTool));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DataInfoGridView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.IconContainer = new System.Windows.Forms.ImageList(this.components);
            this.advTreeProject = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.tabControl = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabItemDBInfo = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.axTOCControl = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.tabItemDBLayer = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlInfo = new DevComponents.DotNetBar.TabControl();
            this.tabControlDataList = new DevComponents.DotNetBar.TabControlPanel();
            this.tabItem4 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlMetaData = new DevComponents.DotNetBar.TabControlPanel();
            this.MetaDataGridView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.tabItem5 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanelMapView = new DevComponents.DotNetBar.TabControlPanel();
            this.tabItem3 = new DevComponents.DotNetBar.TabItem(this.components);
            this.axMapControl = new ESRI.ArcGIS.Controls.AxMapControl();
            this.DevBar = new DevComponents.DotNetBar.Bar();
            this.txtDisplayPage = new DevComponents.DotNetBar.TextBoxItem();
            this.btnFirst = new DevComponents.DotNetBar.ButtonItem();
            this.btnPreview = new DevComponents.DotNetBar.ButtonItem();
            this.btnNext = new DevComponents.DotNetBar.ButtonItem();
            this.btnLast = new DevComponents.DotNetBar.ButtonItem();
            this.dataGridViewSysSetting = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.advTreeSysSetting = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector2 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            ((System.ComponentModel.ISupportInitialize)(this.DataInfoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabControlPanel2.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlInfo)).BeginInit();
            this.tabControlInfo.SuspendLayout();
            this.tabControlDataList.SuspendLayout();
            this.tabControlMetaData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MetaDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DevBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSysSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeSysSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // DataInfoGridView
            // 
            this.DataInfoGridView.AllowUserToAddRows = false;
            this.DataInfoGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataInfoGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DataInfoGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataInfoGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.DataInfoGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataInfoGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.DataInfoGridView.Location = new System.Drawing.Point(1, 1);
            this.DataInfoGridView.Name = "DataInfoGridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataInfoGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DataInfoGridView.RowTemplate.Height = 23;
            this.DataInfoGridView.Size = new System.Drawing.Size(378, 353);
            this.DataInfoGridView.TabIndex = 2;
            // 
            // IconContainer
            // 
            this.IconContainer.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IconContainer.ImageStream")));
            this.IconContainer.TransparentColor = System.Drawing.Color.Transparent;
            this.IconContainer.Images.SetKeyName(0, "GeoDBATool.ControlsInitialDBHistory.PNG");
            this.IconContainer.Images.SetKeyName(1, "GeoSMPDMain.ControlsAddAllWorkDataBase.png");
            this.IconContainer.Images.SetKeyName(2, "GeoSMPDMain.ControlsAddOutDataBase.png");
            this.IconContainer.Images.SetKeyName(3, "GeoSMPDMain.ControlsOpenSubareaProject.png");
            this.IconContainer.Images.SetKeyName(4, "GeoSMPDMain.ControlsAddUpdateData.PNG");
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
            this.advTreeProject.DragDropEnabled = false;
            this.advTreeProject.ImageList = this.IconContainer;
            this.advTreeProject.Location = new System.Drawing.Point(1, 1);
            this.advTreeProject.Name = "advTreeProject";
            this.advTreeProject.NodesConnector = this.nodeConnector1;
            this.advTreeProject.NodeStyle = this.elementStyle1;
            this.advTreeProject.PathSeparator = ";";
            this.advTreeProject.Size = new System.Drawing.Size(266, 381);
            this.advTreeProject.SuspendPaint = false;
            this.advTreeProject.TabIndex = 1;
            this.advTreeProject.Text = "advTree1";
            this.advTreeProject.NodeMouseDown += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeProject_NodeMouseDown);
            this.advTreeProject.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeProject_NodeClick);
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
            // tabControl
            // 
            this.tabControl.CanReorderTabs = true;
            this.tabControl.Controls.Add(this.tabControlPanel2);
            this.tabControl.Controls.Add(this.tabControlPanel1);
            this.tabControl.Location = new System.Drawing.Point(46, 20);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl.SelectedTabIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(268, 409);
            this.tabControl.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Bottom;
            this.tabControl.TabIndex = 1;
            this.tabControl.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl.Tabs.Add(this.tabItemDBLayer);
            this.tabControl.Tabs.Add(this.tabItemDBInfo);
            this.tabControl.Text = "tabControl1";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this.advTreeProject);
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(268, 383);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlPanel2.Style.GradientAngle = -90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabItemDBInfo;
            // 
            // tabItemDBInfo
            // 
            this.tabItemDBInfo.AttachedControl = this.tabControlPanel2;
            this.tabItemDBInfo.Name = "tabItemDBInfo";
            this.tabItemDBInfo.Text = "数据库";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.axTOCControl);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(268, 383);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlPanel1.Style.GradientAngle = -90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItemDBLayer;
            // 
            // axTOCControl
            // 
            this.axTOCControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOCControl.Location = new System.Drawing.Point(1, 1);
            this.axTOCControl.Name = "axTOCControl";
            this.axTOCControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl.OcxState")));
            this.axTOCControl.Size = new System.Drawing.Size(266, 381);
            this.axTOCControl.TabIndex = 3;
            this.axTOCControl.OnMouseUp += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseUpEventHandler(this.axTOCControl_OnMouseUp);
            this.axTOCControl.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl_OnMouseDown);
            // 
            // tabItemDBLayer
            // 
            this.tabItemDBLayer.AttachedControl = this.tabControlPanel1;
            this.tabItemDBLayer.Name = "tabItemDBLayer";
            this.tabItemDBLayer.Text = "图层";
            // 
            // tabControlInfo
            // 
            this.tabControlInfo.CanReorderTabs = true;
            this.tabControlInfo.Controls.Add(this.tabControlDataList);
            this.tabControlInfo.Controls.Add(this.tabControlMetaData);
            this.tabControlInfo.Controls.Add(this.tabControlPanelMapView);
            this.tabControlInfo.Location = new System.Drawing.Point(332, 21);
            this.tabControlInfo.Name = "tabControlInfo";
            this.tabControlInfo.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControlInfo.SelectedTabIndex = 1;
            this.tabControlInfo.Size = new System.Drawing.Size(380, 381);
            this.tabControlInfo.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Bottom;
            this.tabControlInfo.TabIndex = 5;
            this.tabControlInfo.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControlInfo.Tabs.Add(this.tabItem4);
            this.tabControlInfo.Tabs.Add(this.tabItem3);
            this.tabControlInfo.Tabs.Add(this.tabItem5);
            this.tabControlInfo.Text = "tabControl2";
            // 
            // tabControlDataList
            // 
            this.tabControlDataList.Controls.Add(this.DataInfoGridView);
            this.tabControlDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlDataList.Location = new System.Drawing.Point(0, 0);
            this.tabControlDataList.Name = "tabControlDataList";
            this.tabControlDataList.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlDataList.Size = new System.Drawing.Size(380, 355);
            this.tabControlDataList.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlDataList.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlDataList.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlDataList.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlDataList.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlDataList.Style.GradientAngle = -90;
            this.tabControlDataList.TabIndex = 1;
            this.tabControlDataList.TabItem = this.tabItem4;
            // 
            // tabItem4
            // 
            this.tabItem4.AttachedControl = this.tabControlDataList;
            this.tabItem4.Name = "tabItem4";
            this.tabItem4.Text = "数据列表";
            // 
            // tabControlMetaData
            // 
            this.tabControlMetaData.Controls.Add(this.MetaDataGridView);
            this.tabControlMetaData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMetaData.Location = new System.Drawing.Point(0, 0);
            this.tabControlMetaData.Name = "tabControlMetaData";
            this.tabControlMetaData.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlMetaData.Size = new System.Drawing.Size(380, 355);
            this.tabControlMetaData.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlMetaData.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlMetaData.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlMetaData.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlMetaData.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlMetaData.Style.GradientAngle = -90;
            this.tabControlMetaData.TabIndex = 3;
            this.tabControlMetaData.TabItem = this.tabItem5;
            // 
            // MetaDataGridView
            // 
            this.MetaDataGridView.AllowUserToAddRows = false;
            this.MetaDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MetaDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.MetaDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.MetaDataGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.MetaDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetaDataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.MetaDataGridView.Location = new System.Drawing.Point(1, 1);
            this.MetaDataGridView.Name = "MetaDataGridView";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MetaDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.MetaDataGridView.RowTemplate.Height = 23;
            this.MetaDataGridView.Size = new System.Drawing.Size(378, 353);
            this.MetaDataGridView.TabIndex = 6;
            // 
            // tabItem5
            // 
            this.tabItem5.AttachedControl = this.tabControlMetaData;
            this.tabItem5.Name = "tabItem5";
            this.tabItem5.Text = "元信息";
            // 
            // tabControlPanelMapView
            // 
            this.tabControlPanelMapView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanelMapView.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanelMapView.Name = "tabControlPanelMapView";
            this.tabControlPanelMapView.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanelMapView.Size = new System.Drawing.Size(380, 355);
            this.tabControlPanelMapView.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanelMapView.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanelMapView.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanelMapView.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanelMapView.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlPanelMapView.Style.GradientAngle = -90;
            this.tabControlPanelMapView.TabIndex = 2;
            this.tabControlPanelMapView.TabItem = this.tabItem3;
            // 
            // tabItem3
            // 
            this.tabItem3.AttachedControl = this.tabControlPanelMapView;
            this.tabItem3.Name = "tabItem3";
            this.tabItem3.Text = "图形";
            // 
            // axMapControl
            // 
            this.axMapControl.Location = new System.Drawing.Point(1, 1);
            this.axMapControl.Name = "axMapControl";
            this.axMapControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl.OcxState")));
            this.axMapControl.Size = new System.Drawing.Size(378, 353);
            this.axMapControl.TabIndex = 0;
            this.axMapControl.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl_OnMouseMove);
            this.axMapControl.OnAfterDraw += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterDrawEventHandler(this.axMapControl_OnAfterDraw);
            // 
            // DevBar
            // 
            this.DevBar.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.txtDisplayPage,
            this.btnFirst,
            this.btnPreview,
            this.btnNext,
            this.btnLast});
            this.DevBar.Location = new System.Drawing.Point(347, 458);
            this.DevBar.Name = "DevBar";
            this.DevBar.Size = new System.Drawing.Size(380, 25);
            this.DevBar.Stretch = true;
            this.DevBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.DevBar.TabIndex = 6;
            this.DevBar.TabStop = false;
            this.DevBar.Text = "bar1";
            // 
            // txtDisplayPage
            // 
            this.txtDisplayPage.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.txtDisplayPage.Name = "txtDisplayPage";
            this.txtDisplayPage.WatermarkColor = System.Drawing.SystemColors.GrayText;
            this.txtDisplayPage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDisplayPage_KeyDown);
            // 
            // btnFirst
            // 
            this.btnFirst.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnFirst.ImagePaddingHorizontal = 8;
            this.btnFirst.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Text = "第一页";
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnPreview.ImagePaddingHorizontal = 8;
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Text = "前一页";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnNext
            // 
            this.btnNext.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnNext.ImagePaddingHorizontal = 8;
            this.btnNext.Name = "btnNext";
            this.btnNext.Text = "下一页";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnLast
            // 
            this.btnLast.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnLast.ImagePaddingHorizontal = 8;
            this.btnLast.Name = "btnLast";
            this.btnLast.Text = "最后页";
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // dataGridViewSysSetting
            // 
            this.dataGridViewSysSetting.AllowUserToAddRows = false;
            this.dataGridViewSysSetting.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewSysSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSysSetting.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewSysSetting.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewSysSetting.Location = new System.Drawing.Point(856, 31);
            this.dataGridViewSysSetting.Name = "dataGridViewSysSetting";
            this.dataGridViewSysSetting.RowTemplate.Height = 23;
            this.dataGridViewSysSetting.Size = new System.Drawing.Size(240, 344);
            this.dataGridViewSysSetting.TabIndex = 7;
            // 
            // advTreeSysSetting
            // 
            this.advTreeSysSetting.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeSysSetting.AllowDrop = true;
            this.advTreeSysSetting.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeSysSetting.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeSysSetting.Location = new System.Drawing.Point(750, 31);
            this.advTreeSysSetting.Name = "advTreeSysSetting";
            this.advTreeSysSetting.NodesConnector = this.nodeConnector2;
            this.advTreeSysSetting.NodeStyle = this.elementStyle2;
            this.advTreeSysSetting.PathSeparator = ";";
            this.advTreeSysSetting.Size = new System.Drawing.Size(100, 344);
            this.advTreeSysSetting.Styles.Add(this.elementStyle2);
            this.advTreeSysSetting.SuspendPaint = false;
            this.advTreeSysSetting.TabIndex = 8;
            this.advTreeSysSetting.Text = "advTree1";
            // 
            // nodeConnector2
            // 
            this.nodeConnector2.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle2
            // 
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // UserControlFileDBTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.advTreeSysSetting);
            this.Controls.Add(this.dataGridViewSysSetting);
            this.Controls.Add(this.DevBar);
            this.Controls.Add(this.tabControlInfo);
            this.Controls.Add(this.tabControl);
            this.Name = "UserControlFileDBTool";
            this.Size = new System.Drawing.Size(1319, 575);
            ((System.ComponentModel.ISupportInitialize)(this.DataInfoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabControlPanel2.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlInfo)).EndInit();
            this.tabControlInfo.ResumeLayout(false);
            this.tabControlDataList.ResumeLayout(false);
            this.tabControlMetaData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MetaDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DevBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSysSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeSysSetting)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl;
        private DevComponents.DotNetBar.Controls.DataGridViewX DataInfoGridView;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl;
        private System.Windows.Forms.ImageList IconContainer;
        private DevComponents.AdvTree.AdvTree advTreeProject;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.TabControl tabControl;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabItemDBLayer;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.TabItem tabItemDBInfo;
        private DevComponents.DotNetBar.TabControl tabControlInfo;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanelMapView;
        private DevComponents.DotNetBar.TabItem tabItem3;
        private DevComponents.DotNetBar.TabControlPanel tabControlDataList;
        private DevComponents.DotNetBar.TabItem tabItem4;
        private DevComponents.DotNetBar.TabControlPanel tabControlMetaData;
        private DevComponents.DotNetBar.TabItem tabItem5;
        private DevComponents.DotNetBar.Controls.DataGridViewX MetaDataGridView;
        private DevComponents.DotNetBar.Bar DevBar;
        private DevComponents.DotNetBar.TextBoxItem txtDisplayPage;
        private DevComponents.DotNetBar.ButtonItem btnFirst;
        private DevComponents.DotNetBar.ButtonItem btnPreview;
        private DevComponents.DotNetBar.ButtonItem btnNext;
        private DevComponents.DotNetBar.ButtonItem btnLast;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewSysSetting;
        private DevComponents.AdvTree.AdvTree advTreeSysSetting;
        private DevComponents.AdvTree.NodeConnector nodeConnector2;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
    }
}