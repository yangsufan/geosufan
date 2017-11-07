namespace GeoDBATool
{
    partial class frmBrowseHistorySelFeatures
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBrowseHistorySelFeatures));
            this.axMapControl = new ESRI.ArcGIS.Controls.AxMapControl();
            this.barBrowse = new DevComponents.DotNetBar.Bar();
            this.sliderItem = new DevComponents.DotNetBar.SliderItem();
            this.comboBoxItem = new DevComponents.DotNetBar.ComboBoxItem();
            this.btnCompare = new DevComponents.DotNetBar.ButtonItem();
            this.btnStract = new DevComponents.DotNetBar.ButtonItem();
            this.barMap = new DevComponents.DotNetBar.Bar();
            this.btnDefault = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapZoomIn = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapZoomOut = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapPan = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapZoomInFixed = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapZoomOutFixed = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapRefreshView = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapFullExtent = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapZoomToLastExtentBack = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapZoomToLastExtentForward = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapIdentify = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapMeasure = new DevComponents.DotNetBar.ButtonItem();
            this.barHistoryDataCompare = new DevComponents.DotNetBar.Bar();
            this.dotNetBarManager = new DevComponents.DotNetBar.DotNetBarManager(this.components);
            this.dockSite4 = new DevComponents.DotNetBar.DockSite();
            this.barFilldockSite = new DevComponents.DotNetBar.DockSite();
            this.dockSite1 = new DevComponents.DotNetBar.DockSite();
            this.dockSite2 = new DevComponents.DotNetBar.DockSite();
            this.dockSite8 = new DevComponents.DotNetBar.DockSite();
            this.dockSite5 = new DevComponents.DotNetBar.DockSite();
            this.dockSite6 = new DevComponents.DotNetBar.DockSite();
            this.dockSite7 = new DevComponents.DotNetBar.DockSite();
            this.dockSite3 = new DevComponents.DotNetBar.DockSite();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barBrowse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barHistoryDataCompare)).BeginInit();
            this.SuspendLayout();
            // 
            // axMapControl
            // 
            this.axMapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl.Location = new System.Drawing.Point(0, 0);
            this.axMapControl.Name = "axMapControl";
            this.axMapControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl.OcxState")));
            this.axMapControl.Size = new System.Drawing.Size(707, 465);
            this.axMapControl.TabIndex = 0;
            // 
            // barBrowse
            // 
            this.barBrowse.Dock = System.Windows.Forms.DockStyle.Top;
            this.barBrowse.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            this.barBrowse.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.sliderItem,
            this.comboBoxItem,
            this.btnCompare,
            this.btnStract});
            this.barBrowse.Location = new System.Drawing.Point(0, 0);
            this.barBrowse.Name = "barBrowse";
            this.barBrowse.Size = new System.Drawing.Size(707, 26);
            this.barBrowse.Stretch = true;
            this.barBrowse.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barBrowse.TabIndex = 1;
            this.barBrowse.TabStop = false;
            // 
            // sliderItem
            // 
            this.sliderItem.Name = "sliderItem";
            this.sliderItem.Text = "浏览";
            this.sliderItem.Value = 0;
            this.sliderItem.Width = 300;
            this.sliderItem.ValueChanged += new System.EventHandler(this.sliderItem_ValueChanged);
            // 
            // comboBoxItem
            // 
            this.comboBoxItem.ComboWidth = 180;
            this.comboBoxItem.DropDownHeight = 106;
            this.comboBoxItem.Name = "comboBoxItem";
            this.comboBoxItem.SelectedIndexChanged += new System.EventHandler(this.comboBoxItem_SelectedIndexChanged);
            // 
            // btnCompare
            // 
            this.btnCompare.Image = global::GeoDBATool.Properties.Resources.CompareHistoryData;
            this.btnCompare.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnCompare.ImagePaddingHorizontal = 8;
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Text = "btnCompare";
            this.btnCompare.Tooltip = "对比浏览";
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // btnStract
            // 
            this.btnStract.Image = global::GeoDBATool.Properties.Resources.StractHistoryData;
            this.btnStract.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnStract.ImagePaddingHorizontal = 8;
            this.btnStract.Name = "btnStract";
            this.btnStract.Text = "btnStract";
            this.btnStract.Tooltip = "数据提取";
            this.btnStract.Click += new System.EventHandler(this.btnStract_Click);
            // 
            // barMap
            // 
            this.barMap.Dock = System.Windows.Forms.DockStyle.Left;
            this.barMap.DockOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.barMap.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            this.barMap.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnDefault,
            this.btnMapZoomIn,
            this.btnMapZoomOut,
            this.btnMapPan,
            this.btnMapZoomInFixed,
            this.btnMapZoomOutFixed,
            this.btnMapRefreshView,
            this.btnMapFullExtent,
            this.btnMapZoomToLastExtentBack,
            this.btnMapZoomToLastExtentForward,
            this.btnMapIdentify,
            this.btnMapMeasure});
            this.barMap.Location = new System.Drawing.Point(0, 26);
            this.barMap.Name = "barMap";
            this.barMap.Size = new System.Drawing.Size(28, 439);
            this.barMap.Stretch = true;
            this.barMap.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barMap.TabIndex = 2;
            this.barMap.TabStop = false;
            this.barMap.ItemClick += new System.EventHandler(this.barMap_ItemClick);
            // 
            // btnDefault
            // 
            this.btnDefault.Image = global::GeoDBATool.Properties.Resources.Default;
            this.btnDefault.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnDefault.ImagePaddingHorizontal = 8;
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Text = "btnDefault";
            this.btnDefault.Tooltip = "默认工具";
            // 
            // btnMapZoomIn
            // 
            this.btnMapZoomIn.BeginGroup = true;
            this.btnMapZoomIn.Image = global::GeoDBATool.Properties.Resources.MapZoomIn;
            this.btnMapZoomIn.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnMapZoomIn.ImagePaddingHorizontal = 8;
            this.btnMapZoomIn.Name = "btnMapZoomIn";
            this.btnMapZoomIn.Text = "btnMapZoomIn";
            this.btnMapZoomIn.Tooltip = "放大";
            // 
            // btnMapZoomOut
            // 
            this.btnMapZoomOut.Image = global::GeoDBATool.Properties.Resources.MapZoomOut;
            this.btnMapZoomOut.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnMapZoomOut.ImagePaddingHorizontal = 8;
            this.btnMapZoomOut.Name = "btnMapZoomOut";
            this.btnMapZoomOut.Text = "btnMapZoomOut";
            this.btnMapZoomOut.Tooltip = "缩小";
            // 
            // btnMapPan
            // 
            this.btnMapPan.Image = global::GeoDBATool.Properties.Resources.MapPan;
            this.btnMapPan.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnMapPan.ImagePaddingHorizontal = 8;
            this.btnMapPan.Name = "btnMapPan";
            this.btnMapPan.Text = "btnMapPan";
            this.btnMapPan.Tooltip = "漫游";
            // 
            // btnMapZoomInFixed
            // 
            this.btnMapZoomInFixed.Image = global::GeoDBATool.Properties.Resources.MapZoomInFixed;
            this.btnMapZoomInFixed.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnMapZoomInFixed.ImagePaddingHorizontal = 8;
            this.btnMapZoomInFixed.Name = "btnMapZoomInFixed";
            this.btnMapZoomInFixed.Text = "btnMapZoomInFixed";
            this.btnMapZoomInFixed.Tooltip = "中心放大";
            // 
            // btnMapZoomOutFixed
            // 
            this.btnMapZoomOutFixed.Image = global::GeoDBATool.Properties.Resources.MapZoomOutFixed;
            this.btnMapZoomOutFixed.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnMapZoomOutFixed.ImagePaddingHorizontal = 8;
            this.btnMapZoomOutFixed.Name = "btnMapZoomOutFixed";
            this.btnMapZoomOutFixed.Text = "btnMapZoomOutFixed";
            this.btnMapZoomOutFixed.Tooltip = "中心缩小";
            // 
            // btnMapRefreshView
            // 
            this.btnMapRefreshView.Image = global::GeoDBATool.Properties.Resources.MapRefreshView;
            this.btnMapRefreshView.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnMapRefreshView.ImagePaddingHorizontal = 8;
            this.btnMapRefreshView.Name = "btnMapRefreshView";
            this.btnMapRefreshView.Text = "btnMapRefreshView";
            this.btnMapRefreshView.Tooltip = "刷新";
            // 
            // btnMapFullExtent
            // 
            this.btnMapFullExtent.Image = global::GeoDBATool.Properties.Resources.MapFullExtent;
            this.btnMapFullExtent.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnMapFullExtent.ImagePaddingHorizontal = 8;
            this.btnMapFullExtent.Name = "btnMapFullExtent";
            this.btnMapFullExtent.Text = "btnMapFullExtent";
            this.btnMapFullExtent.Tooltip = "全屏";
            // 
            // btnMapZoomToLastExtentBack
            // 
            this.btnMapZoomToLastExtentBack.Image = global::GeoDBATool.Properties.Resources.MapZoomToLastExtentBack;
            this.btnMapZoomToLastExtentBack.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnMapZoomToLastExtentBack.ImagePaddingHorizontal = 8;
            this.btnMapZoomToLastExtentBack.Name = "btnMapZoomToLastExtentBack";
            this.btnMapZoomToLastExtentBack.Text = "btnMapZoomToLastExtentBack";
            this.btnMapZoomToLastExtentBack.Tooltip = "后景";
            // 
            // btnMapZoomToLastExtentForward
            // 
            this.btnMapZoomToLastExtentForward.Image = global::GeoDBATool.Properties.Resources.MapZoomToLastExtentForward;
            this.btnMapZoomToLastExtentForward.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnMapZoomToLastExtentForward.ImagePaddingHorizontal = 8;
            this.btnMapZoomToLastExtentForward.Name = "btnMapZoomToLastExtentForward";
            this.btnMapZoomToLastExtentForward.Text = "btnMapZoomToLastExtentForward";
            this.btnMapZoomToLastExtentForward.Tooltip = "前景";
            // 
            // btnMapIdentify
            // 
            this.btnMapIdentify.BeginGroup = true;
            this.btnMapIdentify.Image = global::GeoDBATool.Properties.Resources.MapIdentify;
            this.btnMapIdentify.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnMapIdentify.ImagePaddingHorizontal = 8;
            this.btnMapIdentify.Name = "btnMapIdentify";
            this.btnMapIdentify.Text = "btnMapIdentify";
            this.btnMapIdentify.Tooltip = "查询";
            // 
            // btnMapMeasure
            // 
            this.btnMapMeasure.Image = global::GeoDBATool.Properties.Resources.MapMeasure;
            this.btnMapMeasure.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnMapMeasure.ImagePaddingHorizontal = 8;
            this.btnMapMeasure.Name = "btnMapMeasure";
            this.btnMapMeasure.Text = "btnMapMeasure";
            this.btnMapMeasure.Tooltip = "量算工具";
            // 
            // barHistoryDataCompare
            // 
            this.barHistoryDataCompare.AccessibleDescription = "DotNetBar Bar (barHistoryDataCompare)";
            this.barHistoryDataCompare.AccessibleName = "DotNetBar Bar";
            this.barHistoryDataCompare.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.barHistoryDataCompare.AutoSyncBarCaption = true;
            this.barHistoryDataCompare.CanHide = true;
            this.barHistoryDataCompare.CloseSingleTab = true;
            this.barHistoryDataCompare.Dock = System.Windows.Forms.DockStyle.Right;
            this.barHistoryDataCompare.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            this.barHistoryDataCompare.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Caption;
            this.barHistoryDataCompare.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            this.barHistoryDataCompare.Location = new System.Drawing.Point(632, 0);
            this.barHistoryDataCompare.Name = "barHistoryDataCompare";
            this.barHistoryDataCompare.Size = new System.Drawing.Size(75, 465);
            this.barHistoryDataCompare.Stretch = true;
            this.barHistoryDataCompare.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barHistoryDataCompare.TabIndex = 3;
            this.barHistoryDataCompare.TabStop = false;
            // 
            // dotNetBarManager
            // 
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.F1);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlA);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlY);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Del);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Ins);
            this.dotNetBarManager.BottomDockSite = this.dockSite4;
            this.dotNetBarManager.DefinitionName = "";
            this.dotNetBarManager.EnableFullSizeDock = false;
            this.dotNetBarManager.FillDockSite = this.barFilldockSite;
            this.dotNetBarManager.LeftDockSite = this.dockSite1;
            this.dotNetBarManager.ParentForm = this;
            this.dotNetBarManager.RightDockSite = this.dockSite2;
            this.dotNetBarManager.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.dotNetBarManager.ToolbarBottomDockSite = this.dockSite8;
            this.dotNetBarManager.ToolbarLeftDockSite = this.dockSite5;
            this.dotNetBarManager.ToolbarRightDockSite = this.dockSite6;
            this.dotNetBarManager.ToolbarTopDockSite = this.dockSite7;
            this.dotNetBarManager.TopDockSite = this.dockSite3;
            // 
            // dockSite4
            // 
            this.dockSite4.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dockSite4.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.dockSite4.Location = new System.Drawing.Point(0, 465);
            this.dockSite4.Name = "dockSite4";
            this.dockSite4.Size = new System.Drawing.Size(707, 0);
            this.dockSite4.TabIndex = 3;
            this.dockSite4.TabStop = false;
            // 
            // barFilldockSite
            // 
            this.barFilldockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barFilldockSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barFilldockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.barFilldockSite.Location = new System.Drawing.Point(0, 0);
            this.barFilldockSite.Name = "barFilldockSite";
            this.barFilldockSite.Size = new System.Drawing.Size(707, 465);
            this.barFilldockSite.TabIndex = 4;
            this.barFilldockSite.TabStop = false;
            // 
            // dockSite1
            // 
            this.dockSite1.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dockSite1.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.dockSite1.Location = new System.Drawing.Point(0, 0);
            this.dockSite1.Name = "dockSite1";
            this.dockSite1.Size = new System.Drawing.Size(0, 465);
            this.dockSite1.TabIndex = 0;
            this.dockSite1.TabStop = false;
            // 
            // dockSite2
            // 
            this.dockSite2.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite2.Dock = System.Windows.Forms.DockStyle.Right;
            this.dockSite2.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.dockSite2.Location = new System.Drawing.Point(707, 0);
            this.dockSite2.Name = "dockSite2";
            this.dockSite2.Size = new System.Drawing.Size(0, 465);
            this.dockSite2.TabIndex = 1;
            this.dockSite2.TabStop = false;
            // 
            // dockSite8
            // 
            this.dockSite8.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dockSite8.Location = new System.Drawing.Point(0, 465);
            this.dockSite8.Name = "dockSite8";
            this.dockSite8.Size = new System.Drawing.Size(707, 0);
            this.dockSite8.TabIndex = 7;
            this.dockSite8.TabStop = false;
            // 
            // dockSite5
            // 
            this.dockSite5.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite5.Dock = System.Windows.Forms.DockStyle.Left;
            this.dockSite5.Location = new System.Drawing.Point(0, 0);
            this.dockSite5.Name = "dockSite5";
            this.dockSite5.Size = new System.Drawing.Size(0, 465);
            this.dockSite5.TabIndex = 4;
            this.dockSite5.TabStop = false;
            // 
            // dockSite6
            // 
            this.dockSite6.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite6.Dock = System.Windows.Forms.DockStyle.Right;
            this.dockSite6.Location = new System.Drawing.Point(707, 0);
            this.dockSite6.Name = "dockSite6";
            this.dockSite6.Size = new System.Drawing.Size(0, 465);
            this.dockSite6.TabIndex = 5;
            this.dockSite6.TabStop = false;
            // 
            // dockSite7
            // 
            this.dockSite7.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite7.Dock = System.Windows.Forms.DockStyle.Top;
            this.dockSite7.Location = new System.Drawing.Point(0, 0);
            this.dockSite7.Name = "dockSite7";
            this.dockSite7.Size = new System.Drawing.Size(707, 0);
            this.dockSite7.TabIndex = 6;
            this.dockSite7.TabStop = false;
            // 
            // dockSite3
            // 
            this.dockSite3.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite3.Dock = System.Windows.Forms.DockStyle.Top;
            this.dockSite3.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.dockSite3.Location = new System.Drawing.Point(0, 0);
            this.dockSite3.Name = "dockSite3";
            this.dockSite3.Size = new System.Drawing.Size(707, 0);
            this.dockSite3.TabIndex = 2;
            this.dockSite3.TabStop = false;
            // 
            // frmBrowseHistorySelFeatures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 465);
            this.Controls.Add(this.axMapControl);
            this.Controls.Add(this.barFilldockSite);
            this.Controls.Add(this.dockSite2);
            this.Controls.Add(this.dockSite1);
            this.Controls.Add(this.dockSite3);
            this.Controls.Add(this.dockSite4);
            this.Controls.Add(this.dockSite5);
            this.Controls.Add(this.dockSite6);
            this.Controls.Add(this.dockSite7);
            this.Controls.Add(this.dockSite8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmBrowseHistorySelFeatures";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "要素历史浏览";
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barBrowse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barHistoryDataCompare)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl;
        private DevComponents.DotNetBar.Bar barBrowse;
        private DevComponents.DotNetBar.SliderItem sliderItem;
        private DevComponents.DotNetBar.ComboBoxItem comboBoxItem;
        private DevComponents.DotNetBar.Bar barMap;
        private DevComponents.DotNetBar.ButtonItem btnDefault;
        private DevComponents.DotNetBar.ButtonItem btnMapZoomIn;
        private DevComponents.DotNetBar.ButtonItem btnMapZoomOut;
        private DevComponents.DotNetBar.ButtonItem btnMapPan;
        private DevComponents.DotNetBar.ButtonItem btnMapZoomInFixed;
        private DevComponents.DotNetBar.ButtonItem btnMapZoomOutFixed;
        private DevComponents.DotNetBar.ButtonItem btnMapRefreshView;
        private DevComponents.DotNetBar.ButtonItem btnMapFullExtent;
        private DevComponents.DotNetBar.ButtonItem btnMapZoomToLastExtentBack;
        private DevComponents.DotNetBar.ButtonItem btnMapZoomToLastExtentForward;
        private DevComponents.DotNetBar.ButtonItem btnMapIdentify;
        private DevComponents.DotNetBar.ButtonItem btnMapMeasure;
        private DevComponents.DotNetBar.Bar barHistoryDataCompare;
        private DevComponents.DotNetBar.ButtonItem btnCompare;
        private DevComponents.DotNetBar.ButtonItem btnStract;
        private DevComponents.DotNetBar.DotNetBarManager dotNetBarManager;
        private DevComponents.DotNetBar.DockSite dockSite4;
        private DevComponents.DotNetBar.DockSite dockSite1;
        private DevComponents.DotNetBar.DockSite dockSite2;
        private DevComponents.DotNetBar.DockSite dockSite3;
        private DevComponents.DotNetBar.DockSite dockSite5;
        private DevComponents.DotNetBar.DockSite dockSite6;
        private DevComponents.DotNetBar.DockSite dockSite7;
        private DevComponents.DotNetBar.DockSite dockSite8;
        private DevComponents.DotNetBar.DockSite barFilldockSite;
    }
}