namespace GeoSysSetting.SubControl
{
    partial class UCDataSourceManger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDataSourceManger));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanelSource = new DevComponents.DotNetBar.TabControlPanel();
            this.itemPanel2 = new DevComponents.DotNetBar.ItemPanel();
            this.layerTree = new GeoLayerTreeLib.LayerManager.UcDataLib();
            this.tabItemDBSource = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanelDBLayer = new DevComponents.DotNetBar.TabControlPanel();
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.axTOCControl = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.tabItemDBLayer = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControl1 = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.buttonItemZoomIn = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemZoonOut = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemFullExtent = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemPan = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemZoonInfixed = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemZoomOutfixed = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemRefresh = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemBack = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemForward = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemMapScan = new DevComponents.DotNetBar.ButtonItem();
            this.MapControlLayer = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axMapControlcontextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MapZoomInMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZoomOutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MapFullExtentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MapPanMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MapZoomInFixedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MapZoomOutFixedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MapRefreshMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BackCommandMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ForwardCommandMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabItem2 = new DevComponents.DotNetBar.TabItem(this.components);
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabControlPanelSource.SuspendLayout();
            this.itemPanel2.SuspendLayout();
            this.tabControlPanelDBLayer.SuspendLayout();
            this.itemPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapControlLayer)).BeginInit();
            this.axMapControlcontextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(729, 419);
            this.splitContainer1.SplitterDistance = 191;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.CanReorderTabs = true;
            this.tabControl.Controls.Add(this.tabControlPanelSource);
            this.tabControl.Controls.Add(this.tabControlPanelDBLayer);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl.SelectedTabIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(191, 419);
            this.tabControl.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Dock;
            this.tabControl.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Bottom;
            this.tabControl.TabIndex = 2;
            this.tabControl.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl.Tabs.Add(this.tabItemDBSource);
            this.tabControl.Tabs.Add(this.tabItemDBLayer);
            // 
            // tabControlPanelSource
            // 
            this.tabControlPanelSource.Controls.Add(this.itemPanel2);
            this.tabControlPanelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanelSource.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanelSource.Name = "tabControlPanelSource";
            this.tabControlPanelSource.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanelSource.Size = new System.Drawing.Size(191, 394);
            this.tabControlPanelSource.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.tabControlPanelSource.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.tabControlPanelSource.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanelSource.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanelSource.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlPanelSource.Style.GradientAngle = -90;
            this.tabControlPanelSource.TabIndex = 1;
            this.tabControlPanelSource.TabItem = this.tabItemDBSource;
            // 
            // itemPanel2
            // 
            // 
            // 
            // 
            this.itemPanel2.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.itemPanel2.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel2.BackgroundStyle.BorderBottomWidth = 1;
            this.itemPanel2.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.itemPanel2.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel2.BackgroundStyle.BorderLeftWidth = 1;
            this.itemPanel2.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel2.BackgroundStyle.BorderRightWidth = 1;
            this.itemPanel2.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel2.BackgroundStyle.BorderTopWidth = 1;
            this.itemPanel2.BackgroundStyle.PaddingBottom = 1;
            this.itemPanel2.BackgroundStyle.PaddingLeft = 1;
            this.itemPanel2.BackgroundStyle.PaddingRight = 1;
            this.itemPanel2.BackgroundStyle.PaddingTop = 1;
            this.itemPanel2.ContainerControlProcessDialogKey = true;
            this.itemPanel2.Controls.Add(this.layerTree);
            this.itemPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemPanel2.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanel2.Location = new System.Drawing.Point(1, 1);
            this.itemPanel2.Name = "itemPanel2";
            this.itemPanel2.Size = new System.Drawing.Size(189, 392);
            this.itemPanel2.TabIndex = 1;
            this.itemPanel2.Text = "itemPanel2";
            // 
            // layerTree
            // 
            this.layerTree.DicMenu = null;
            this.layerTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layerTree.Location = new System.Drawing.Point(0, 0);
            this.layerTree.Name = "layerTree";
            this.layerTree.Size = new System.Drawing.Size(189, 392);
            this.layerTree.TabIndex = 1;
            // 
            // tabItemDBSource
            // 
            this.tabItemDBSource.AttachedControl = this.tabControlPanelSource;
            this.tabItemDBSource.Name = "tabItemDBSource";
            this.tabItemDBSource.Text = "目录管理";
            // 
            // tabControlPanelDBLayer
            // 
            this.tabControlPanelDBLayer.Controls.Add(this.itemPanel1);
            this.tabControlPanelDBLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanelDBLayer.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanelDBLayer.Name = "tabControlPanelDBLayer";
            this.tabControlPanelDBLayer.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanelDBLayer.Size = new System.Drawing.Size(191, 394);
            this.tabControlPanelDBLayer.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.tabControlPanelDBLayer.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.tabControlPanelDBLayer.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanelDBLayer.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanelDBLayer.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlPanelDBLayer.Style.GradientAngle = -90;
            this.tabControlPanelDBLayer.TabIndex = 2;
            this.tabControlPanelDBLayer.TabItem = this.tabItemDBLayer;
            // 
            // itemPanel1
            // 
            // 
            // 
            // 
            this.itemPanel1.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.itemPanel1.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderBottomWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.itemPanel1.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderLeftWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderRightWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderTopWidth = 1;
            this.itemPanel1.BackgroundStyle.PaddingBottom = 1;
            this.itemPanel1.BackgroundStyle.PaddingLeft = 1;
            this.itemPanel1.BackgroundStyle.PaddingRight = 1;
            this.itemPanel1.BackgroundStyle.PaddingTop = 1;
            this.itemPanel1.ContainerControlProcessDialogKey = true;
            this.itemPanel1.Controls.Add(this.axTOCControl);
            this.itemPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemPanel1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanel1.Location = new System.Drawing.Point(1, 1);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(189, 392);
            this.itemPanel1.TabIndex = 1;
            this.itemPanel1.Text = "itemPanel1";
            // 
            // axTOCControl
            // 
            this.axTOCControl.AllowDrop = true;
            this.axTOCControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOCControl.Location = new System.Drawing.Point(0, 0);
            this.axTOCControl.Name = "axTOCControl";
            this.axTOCControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl.OcxState")));
            this.axTOCControl.Size = new System.Drawing.Size(189, 392);
            this.axTOCControl.TabIndex = 2;
            this.axTOCControl.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl_OnMouseDown);
            this.axTOCControl.OnMouseUp += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseUpEventHandler(this.axTOCControl_OnMouseUp);
            this.axTOCControl.OnMouseMove += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseMoveEventHandler(this.axTOCControl_OnMouseMove);
            // 
            // tabItemDBLayer
            // 
            this.tabItemDBLayer.AttachedControl = this.tabControlPanelDBLayer;
            this.tabItemDBLayer.Name = "tabItemDBLayer";
            this.tabItemDBLayer.Text = "图层目录";
            // 
            // tabControl1
            // 
            this.tabControl1.CanReorderTabs = true;
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl1.SelectedTabIndex = 1;
            this.tabControl1.Size = new System.Drawing.Size(534, 419);
            this.tabControl1.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Bottom;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl1.Tabs.Add(this.tabItem1);
            this.tabControl1.Text = "tabControl1";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.bar1);
            this.tabControlPanel1.Controls.Add(this.MapControlLayer);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(534, 393);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlPanel1.Style.GradientAngle = -90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItem1;
            // 
            // bar1
            // 
            this.bar1.AccessibleDescription = "DotNetBar Bar (bar1)";
            this.bar1.AccessibleName = "DotNetBar Bar";
            this.bar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.bar1.DockOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.bar1.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.bar1.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItemZoomIn,
            this.buttonItemZoonOut,
            this.buttonItemFullExtent,
            this.buttonItemPan,
            this.buttonItemZoonInfixed,
            this.buttonItemZoomOutfixed,
            this.buttonItemRefresh,
            this.buttonItemBack,
            this.buttonItemForward,
            this.buttonItemMapScan});
            this.bar1.Location = new System.Drawing.Point(1, 1);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(28, 391);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.bar1.TabIndex = 2;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // buttonItemZoomIn
            // 
            this.buttonItemZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemZoomIn.Image")));
            this.buttonItemZoomIn.Name = "buttonItemZoomIn";
            this.buttonItemZoomIn.Tag = "  ";
            this.buttonItemZoomIn.Text = "1";
            this.buttonItemZoomIn.Tooltip = "放大";
            this.buttonItemZoomIn.Click += new System.EventHandler(this.buttonItemZoomIn_Click);
            // 
            // buttonItemZoonOut
            // 
            this.buttonItemZoonOut.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemZoonOut.Image")));
            this.buttonItemZoonOut.Name = "buttonItemZoonOut";
            this.buttonItemZoonOut.Text = "2";
            this.buttonItemZoonOut.Tooltip = "缩小";
            this.buttonItemZoonOut.Click += new System.EventHandler(this.buttonItemZoonOut_Click);
            // 
            // buttonItemFullExtent
            // 
            this.buttonItemFullExtent.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemFullExtent.Image")));
            this.buttonItemFullExtent.Name = "buttonItemFullExtent";
            this.buttonItemFullExtent.Text = "3";
            this.buttonItemFullExtent.Tooltip = "全图";
            this.buttonItemFullExtent.Click += new System.EventHandler(this.buttonItemFullExtent_Click);
            // 
            // buttonItemPan
            // 
            this.buttonItemPan.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemPan.Image")));
            this.buttonItemPan.Name = "buttonItemPan";
            this.buttonItemPan.Text = "4";
            this.buttonItemPan.Tooltip = "漫游";
            this.buttonItemPan.Click += new System.EventHandler(this.buttonItemPan_Click);
            // 
            // buttonItemZoonInfixed
            // 
            this.buttonItemZoonInfixed.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemZoonInfixed.Image")));
            this.buttonItemZoonInfixed.Name = "buttonItemZoonInfixed";
            this.buttonItemZoonInfixed.Text = "5";
            this.buttonItemZoonInfixed.Tooltip = "中心放大";
            this.buttonItemZoonInfixed.Click += new System.EventHandler(this.buttonItemZoonInfixed_Click);
            // 
            // buttonItemZoomOutfixed
            // 
            this.buttonItemZoomOutfixed.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemZoomOutfixed.Image")));
            this.buttonItemZoomOutfixed.Name = "buttonItemZoomOutfixed";
            this.buttonItemZoomOutfixed.Text = "6";
            this.buttonItemZoomOutfixed.Tooltip = "中心缩小";
            this.buttonItemZoomOutfixed.Click += new System.EventHandler(this.buttonItemZoomOutfixed_Click);
            // 
            // buttonItemRefresh
            // 
            this.buttonItemRefresh.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemRefresh.Image")));
            this.buttonItemRefresh.Name = "buttonItemRefresh";
            this.buttonItemRefresh.Text = "7";
            this.buttonItemRefresh.Tooltip = "刷新";
            this.buttonItemRefresh.Click += new System.EventHandler(this.buttonItemRefresh_Click);
            // 
            // buttonItemBack
            // 
            this.buttonItemBack.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemBack.Image")));
            this.buttonItemBack.Name = "buttonItemBack";
            this.buttonItemBack.Text = "8";
            this.buttonItemBack.Tooltip = "前一视图";
            this.buttonItemBack.Click += new System.EventHandler(this.buttonItemBack_Click);
            // 
            // buttonItemForward
            // 
            this.buttonItemForward.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemForward.Image")));
            this.buttonItemForward.Name = "buttonItemForward";
            this.buttonItemForward.Text = "9";
            this.buttonItemForward.Tooltip = "后一视图";
            this.buttonItemForward.Click += new System.EventHandler(this.buttonItemForward_Click);
            // 
            // buttonItemMapScan
            // 
            this.buttonItemMapScan.Image = global::GeoSysSetting.Properties.Resources.MapScan;
            this.buttonItemMapScan.Name = "buttonItemMapScan";
            this.buttonItemMapScan.Text = "浏览";
            this.buttonItemMapScan.Tooltip = "图层浏览";
            this.buttonItemMapScan.Click += new System.EventHandler(this.buttonItemMapScan_Click);
            // 
            // MapControlLayer
            // 
            this.MapControlLayer.ContextMenuStrip = this.axMapControlcontextMenu;
            this.MapControlLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapControlLayer.Location = new System.Drawing.Point(1, 1);
            this.MapControlLayer.Name = "MapControlLayer";
            this.MapControlLayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MapControlLayer.OcxState")));
            this.MapControlLayer.Size = new System.Drawing.Size(532, 391);
            this.MapControlLayer.TabIndex = 0;
            this.MapControlLayer.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.MapControlLayer_OnMouseDown);
            this.MapControlLayer.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.MapControlLayer_OnMouseMove);
            this.MapControlLayer.OnAfterDraw += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterDrawEventHandler(this.MapControlLayer_OnAfterDraw);
            // 
            // axMapControlcontextMenu
            // 
            this.axMapControlcontextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MapZoomInMenuItem,
            this.ZoomOutMenuItem,
            this.MapFullExtentMenuItem,
            this.MapPanMenuItem,
            this.MapZoomInFixedMenuItem,
            this.MapZoomOutFixedMenuItem,
            this.MapRefreshMenuItem,
            this.BackCommandMenuItem,
            this.ForwardCommandMenuItem});
            this.axMapControlcontextMenu.Name = "axMapControlcontextMenu";
            this.axMapControlcontextMenu.Size = new System.Drawing.Size(125, 202);
            // 
            // MapZoomInMenuItem
            // 
            this.MapZoomInMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("MapZoomInMenuItem.Image")));
            this.MapZoomInMenuItem.Name = "MapZoomInMenuItem";
            this.MapZoomInMenuItem.Size = new System.Drawing.Size(124, 22);
            this.MapZoomInMenuItem.Text = "放大";
            this.MapZoomInMenuItem.Click += new System.EventHandler(this.MapZoomInMenuItem_Click);
            // 
            // ZoomOutMenuItem
            // 
            this.ZoomOutMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ZoomOutMenuItem.Image")));
            this.ZoomOutMenuItem.Name = "ZoomOutMenuItem";
            this.ZoomOutMenuItem.Size = new System.Drawing.Size(124, 22);
            this.ZoomOutMenuItem.Text = "缩小";
            this.ZoomOutMenuItem.Click += new System.EventHandler(this.ZoomOutMenuItem_Click);
            // 
            // MapFullExtentMenuItem
            // 
            this.MapFullExtentMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("MapFullExtentMenuItem.Image")));
            this.MapFullExtentMenuItem.Name = "MapFullExtentMenuItem";
            this.MapFullExtentMenuItem.Size = new System.Drawing.Size(124, 22);
            this.MapFullExtentMenuItem.Text = "全屏";
            this.MapFullExtentMenuItem.Click += new System.EventHandler(this.MapFullExtentMenuItem_Click);
            // 
            // MapPanMenuItem
            // 
            this.MapPanMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("MapPanMenuItem.Image")));
            this.MapPanMenuItem.Name = "MapPanMenuItem";
            this.MapPanMenuItem.Size = new System.Drawing.Size(124, 22);
            this.MapPanMenuItem.Text = "漫游";
            this.MapPanMenuItem.Click += new System.EventHandler(this.MapPanMenuItem_Click);
            // 
            // MapZoomInFixedMenuItem
            // 
            this.MapZoomInFixedMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("MapZoomInFixedMenuItem.Image")));
            this.MapZoomInFixedMenuItem.Name = "MapZoomInFixedMenuItem";
            this.MapZoomInFixedMenuItem.Size = new System.Drawing.Size(124, 22);
            this.MapZoomInFixedMenuItem.Text = "中心放大";
            this.MapZoomInFixedMenuItem.Click += new System.EventHandler(this.MapZoomInFixedMenuItem_Click);
            // 
            // MapZoomOutFixedMenuItem
            // 
            this.MapZoomOutFixedMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("MapZoomOutFixedMenuItem.Image")));
            this.MapZoomOutFixedMenuItem.Name = "MapZoomOutFixedMenuItem";
            this.MapZoomOutFixedMenuItem.Size = new System.Drawing.Size(124, 22);
            this.MapZoomOutFixedMenuItem.Text = "中心缩小";
            this.MapZoomOutFixedMenuItem.Click += new System.EventHandler(this.MapZoomOutFixedMenuItem_Click);
            // 
            // MapRefreshMenuItem
            // 
            this.MapRefreshMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("MapRefreshMenuItem.Image")));
            this.MapRefreshMenuItem.Name = "MapRefreshMenuItem";
            this.MapRefreshMenuItem.Size = new System.Drawing.Size(124, 22);
            this.MapRefreshMenuItem.Text = "刷新";
            this.MapRefreshMenuItem.Click += new System.EventHandler(this.MapRefreshMenuItem_Click);
            // 
            // BackCommandMenuItem
            // 
            this.BackCommandMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("BackCommandMenuItem.Image")));
            this.BackCommandMenuItem.Name = "BackCommandMenuItem";
            this.BackCommandMenuItem.Size = new System.Drawing.Size(124, 22);
            this.BackCommandMenuItem.Text = "后景";
            this.BackCommandMenuItem.Click += new System.EventHandler(this.BackCommandMenuItem_Click);
            // 
            // ForwardCommandMenuItem
            // 
            this.ForwardCommandMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ForwardCommandMenuItem.Image")));
            this.ForwardCommandMenuItem.Name = "ForwardCommandMenuItem";
            this.ForwardCommandMenuItem.Size = new System.Drawing.Size(124, 22);
            this.ForwardCommandMenuItem.Text = "前景";
            this.ForwardCommandMenuItem.Click += new System.EventHandler(this.ForwardCommandMenuItem_Click);
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "视图浏览";
            // 
            // tabItem2
            // 
            this.tabItem2.Name = "tabItem2";
            this.tabItem2.Text = "数据源管理";
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
            // UCDataSourceManger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UCDataSourceManger";
            this.Size = new System.Drawing.Size(729, 419);
            this.Load += new System.EventHandler(this.UCDataSourceManger_Load);
            this.EnabledChanged += new System.EventHandler(this.UCDataSourceManger_EnabledChanged);
            this.VisibleChanged += new System.EventHandler(this.UCDataSourceManger_VisibleChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabControlPanelSource.ResumeLayout(false);
            this.itemPanel2.ResumeLayout(false);
            this.tabControlPanelDBLayer.ResumeLayout(false);
            this.itemPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapControlLayer)).EndInit();
            this.axMapControlcontextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.TabControl tabControl1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private DevComponents.DotNetBar.TabItem tabItem2;
        private ESRI.ArcGIS.Controls.AxMapControl MapControlLayer;
        public System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.ContextMenuStrip axMapControlcontextMenu;
        private System.Windows.Forms.ToolStripMenuItem MapZoomInMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ZoomOutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MapFullExtentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MapPanMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MapZoomInFixedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MapZoomOutFixedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MapRefreshMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BackCommandMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ForwardCommandMenuItem;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem buttonItemZoomIn;
        private DevComponents.DotNetBar.ButtonItem buttonItemZoonOut;
        private DevComponents.DotNetBar.ButtonItem buttonItemFullExtent;
        private DevComponents.DotNetBar.ButtonItem buttonItemPan;
        private DevComponents.DotNetBar.ButtonItem buttonItemZoonInfixed;
        private DevComponents.DotNetBar.ButtonItem buttonItemZoomOutfixed;
        private DevComponents.DotNetBar.ButtonItem buttonItemRefresh;
        private DevComponents.DotNetBar.ButtonItem buttonItemBack;
        private DevComponents.DotNetBar.ButtonItem buttonItemForward;
        private DevComponents.DotNetBar.TabControl tabControl;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanelSource;
        private DevComponents.DotNetBar.ItemPanel itemPanel2;
        private DevComponents.DotNetBar.TabItem tabItemDBSource;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanelDBLayer;
        private DevComponents.DotNetBar.ItemPanel itemPanel1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl;
        private DevComponents.DotNetBar.TabItem tabItemDBLayer;
        private GeoLayerTreeLib.LayerManager.UcDataLib layerTree;
        private DevComponents.DotNetBar.ButtonItem buttonItemMapScan;
    }
}
