using System.Windows.Forms;
using System;
namespace GeoPageLayout
{
    partial class FrmPageLayout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPageLayout));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SSLblPageX = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SSLabelPageY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SSLabelMapX = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel10 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SSLabelMapY = new System.Windows.Forms.ToolStripStatusLabel();
            this.cMSElePro = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itmElePro = new System.Windows.Forms.ToolStripMenuItem();
            this.itmConvertGra = new System.Windows.Forms.ToolStripMenuItem();
            this.itmUngroup = new System.Windows.Forms.ToolStripMenuItem();
            this.itmGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.cMSPageLayout = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.itmClearSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.cMSTOC = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ItemSymbolSet = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemLyrPro = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.axPageLayoutControl1 = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ribbonBar3 = new DevComponents.DotNetBar.RibbonBar();
            this.btnRotate = new DevComponents.DotNetBar.ButtonItem();
            this.btnExportAV = new DevComponents.DotNetBar.ButtonItem();
            this.btnSelect = new DevComponents.DotNetBar.ButtonItem();
            this.btnPrint = new DevComponents.DotNetBar.ButtonItem();
            this.btnPrintPreview = new DevComponents.DotNetBar.ButtonItem();
            this.btnLayoutPageSet = new DevComponents.DotNetBar.ButtonItem();
            this.btnElementEdit = new DevComponents.DotNetBar.ButtonItem();
            this.btnDeleteEle = new DevComponents.DotNetBar.ButtonItem();
            this.btnAnnotation = new DevComponents.DotNetBar.ButtonItem();
            this.btnTextEle = new DevComponents.DotNetBar.ButtonItem();
            this.btnMTextEle = new DevComponents.DotNetBar.ButtonItem();
            this.btnGraphic = new DevComponents.DotNetBar.ButtonItem();
            this.btnCircle = new DevComponents.DotNetBar.ButtonItem();
            this.btnRectangle = new DevComponents.DotNetBar.ButtonItem();
            this.btnPolygon = new DevComponents.DotNetBar.ButtonItem();
            this.btnEllipse = new DevComponents.DotNetBar.ButtonItem();
            this.btnZline = new DevComponents.DotNetBar.ButtonItem();
            this.btnRline = new DevComponents.DotNetBar.ButtonItem();
            this.btnPoint = new DevComponents.DotNetBar.ButtonItem();
            this.btnMapSurround = new DevComponents.DotNetBar.ButtonItem();
            this.btnLegend = new DevComponents.DotNetBar.ButtonItem();
            this.btnNorthArrow = new DevComponents.DotNetBar.ButtonItem();
            this.btnScaleBar = new DevComponents.DotNetBar.ButtonItem();
            this.btnScalebarText = new DevComponents.DotNetBar.ButtonItem();
            this.btnPicture = new DevComponents.DotNetBar.ButtonItem();
            this.btnYWZT = new DevComponents.DotNetBar.ButtonItem();
            this.btnTDLYXZ = new DevComponents.DotNetBar.ButtonItem();
            this.btnTDLYGH = new DevComponents.DotNetBar.ButtonItem();
            this.btnBZTF = new DevComponents.DotNetBar.ButtonItem();
            this.btn500 = new DevComponents.DotNetBar.ButtonItem();
            this.btn1000 = new DevComponents.DotNetBar.ButtonItem();
            this.btn2000 = new DevComponents.DotNetBar.ButtonItem();
            this.btn5K = new DevComponents.DotNetBar.ButtonItem();
            this.btn1W = new DevComponents.DotNetBar.ButtonItem();
            this.btn2W5 = new DevComponents.DotNetBar.ButtonItem();
            this.btn5W = new DevComponents.DotNetBar.ButtonItem();
            this.btn25W = new DevComponents.DotNetBar.ButtonItem();
            this.btnZDYFW = new DevComponents.DotNetBar.ButtonItem();
            this.btnSelectaFeatureExtent = new DevComponents.DotNetBar.ButtonItem();
            this.btnRectangleExtent = new DevComponents.DotNetBar.ButtonItem();
            this.btnPolygonExtent = new DevComponents.DotNetBar.ButtonItem();
            this.btnImportExtent = new DevComponents.DotNetBar.ButtonItem();
            this.btnPageLayerout = new DevComponents.DotNetBar.ButtonItem();
            this.btnManagerPageLayerout = new DevComponents.DotNetBar.ButtonItem();
            this.statusStrip1.SuspendLayout();
            this.cMSElePro.SuspendLayout();
            this.cMSPageLayout.SuspendLayout();
            this.cMSTOC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.SSLblPageX,
            this.toolStripStatusLabel1,
            this.SSLabelPageY,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel8,
            this.SSLabelMapX,
            this.toolStripStatusLabel10,
            this.SSLabelMapY});
            this.statusStrip1.Location = new System.Drawing.Point(0, 494);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1172, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(120, 17);
            this.toolStripStatusLabel3.Text = "                            ";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel4.Text = "页面坐标：";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(25, 17);
            this.toolStripStatusLabel5.Text = "X=";
            // 
            // SSLblPageX
            // 
            this.SSLblPageX.Name = "SSLblPageX";
            this.SSLblPageX.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(24, 17);
            this.toolStripStatusLabel1.Text = "Y=";
            // 
            // SSLabelPageY
            // 
            this.SSLabelPageY.Name = "SSLabelPageY";
            this.SSLabelPageY.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel7.Text = "厘米";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(25, 17);
            this.toolStripStatusLabel8.Text = "X=";
            this.toolStripStatusLabel8.Visible = false;
            // 
            // SSLabelMapX
            // 
            this.SSLabelMapX.Name = "SSLabelMapX";
            this.SSLabelMapX.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel10
            // 
            this.toolStripStatusLabel10.Name = "toolStripStatusLabel10";
            this.toolStripStatusLabel10.Size = new System.Drawing.Size(24, 17);
            this.toolStripStatusLabel10.Text = "Y=";
            this.toolStripStatusLabel10.Visible = false;
            // 
            // SSLabelMapY
            // 
            this.SSLabelMapY.Name = "SSLabelMapY";
            this.SSLabelMapY.Size = new System.Drawing.Size(0, 17);
            // 
            // cMSElePro
            // 
            this.cMSElePro.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmElePro,
            this.itmConvertGra,
            this.itmUngroup});
            this.cMSElePro.Name = "cMSElePro";
            this.cMSElePro.ShowImageMargin = false;
            this.cMSElePro.Size = new System.Drawing.Size(100, 70);
            // 
            // itmElePro
            // 
            this.itmElePro.Name = "itmElePro";
            this.itmElePro.Size = new System.Drawing.Size(99, 22);
            this.itmElePro.Text = "属性";
            this.itmElePro.Click += new System.EventHandler(this.属性ToolStripMenuItem_Click);
            // 
            // itmConvertGra
            // 
            this.itmConvertGra.Name = "itmConvertGra";
            this.itmConvertGra.Size = new System.Drawing.Size(99, 22);
            this.itmConvertGra.Text = "转为图形";
            this.itmConvertGra.Click += new System.EventHandler(this.转为图形ToolStripMenuItem_Click);
            // 
            // itmUngroup
            // 
            this.itmUngroup.Name = "itmUngroup";
            this.itmUngroup.Size = new System.Drawing.Size(99, 22);
            this.itmUngroup.Text = "打散";
            this.itmUngroup.Click += new System.EventHandler(this.取消组合ToolStripMenuItem_Click);
            // 
            // itmGroup
            // 
            this.itmGroup.Name = "itmGroup";
            this.itmGroup.Size = new System.Drawing.Size(99, 22);
            this.itmGroup.Text = "组合";
            this.itmGroup.Click += new System.EventHandler(this.组合ToolStripMenuItem_Click);
            // 
            // cMSPageLayout
            // 
            this.cMSPageLayout.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmGroup,
            this.itmSelectAll,
            this.itmClearSelect});
            this.cMSPageLayout.Name = "cMSElePro";
            this.cMSPageLayout.ShowImageMargin = false;
            this.cMSPageLayout.Size = new System.Drawing.Size(100, 70);
            // 
            // itmSelectAll
            // 
            this.itmSelectAll.Name = "itmSelectAll";
            this.itmSelectAll.Size = new System.Drawing.Size(99, 22);
            this.itmSelectAll.Text = "全选元素";
            this.itmSelectAll.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // itmClearSelect
            // 
            this.itmClearSelect.Name = "itmClearSelect";
            this.itmClearSelect.Size = new System.Drawing.Size(99, 22);
            this.itmClearSelect.Text = "清除选择";
            this.itmClearSelect.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // cMSTOC
            // 
            this.cMSTOC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ItemSymbolSet,
            this.ItemLyrPro});
            this.cMSTOC.Name = "cMSTOC";
            this.cMSTOC.Size = new System.Drawing.Size(125, 48);
            // 
            // ItemSymbolSet
            // 
            this.ItemSymbolSet.Name = "ItemSymbolSet";
            this.ItemSymbolSet.Size = new System.Drawing.Size(124, 22);
            this.ItemSymbolSet.Text = "符号设置";
            this.ItemSymbolSet.Click += new System.EventHandler(this.ItemSymbolSet_Click);
            // 
            // ItemLyrPro
            // 
            this.ItemLyrPro.Name = "ItemLyrPro";
            this.ItemLyrPro.Size = new System.Drawing.Size(124, 22);
            this.ItemLyrPro.Text = "图层属性";
            this.ItemLyrPro.Click += new System.EventHandler(this.ItemLyrPro_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 5000;
            // 
            // axPageLayoutControl1
            // 
            this.axPageLayoutControl1.ContextMenuStrip = this.cMSElePro;
            this.axPageLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axPageLayoutControl1.Location = new System.Drawing.Point(28, 0);
            this.axPageLayoutControl1.Name = "axPageLayoutControl1";
            this.axPageLayoutControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPageLayoutControl1.OcxState")));
            this.axPageLayoutControl1.Size = new System.Drawing.Size(892, 494);
            this.axPageLayoutControl1.TabIndex = 0;
            this.axPageLayoutControl1.OnPageLayoutReplaced += new ESRI.ArcGIS.Controls.IPageLayoutControlEvents_Ax_OnPageLayoutReplacedEventHandler(this.axPageLayoutControl1_OnPageLayoutReplaced);
            this.axPageLayoutControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IPageLayoutControlEvents_Ax_OnMouseDownEventHandler(this.axPageLayoutControl1_OnMouseDown);
            this.axPageLayoutControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IPageLayoutControlEvents_Ax_OnMouseMoveEventHandler(this.axPageLayoutControl1_OnMouseMove);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 0);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(28, 494);
            this.axToolbarControl1.TabIndex = 1;
            this.axToolbarControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IToolbarControlEvents_Ax_OnMouseMoveEventHandler(this.axToolbarControl1_OnMouseMove);
            this.axToolbarControl1.OnItemClick += new ESRI.ArcGIS.Controls.IToolbarControlEvents_Ax_OnItemClickEventHandler(this.axToolbarControl1_OnItemClick);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(134, 87);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 2;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOCControl1.Location = new System.Drawing.Point(0, 0);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(248, 494);
            this.axTOCControl1.TabIndex = 2;
            this.axTOCControl1.OnMouseUp += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseUpEventHandler(this.axTOCControl1_OnMouseUp);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.axTOCControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.axLicenseControl1);
            this.splitContainer1.Panel2.Controls.Add(this.axPageLayoutControl1);
            this.splitContainer1.Panel2.Controls.Add(this.axToolbarControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1172, 494);
            this.splitContainer1.SplitterDistance = 248;
            this.splitContainer1.TabIndex = 2;
            // 
            // ribbonBar3
            // 
            this.ribbonBar3.AutoOverflowEnabled = true;
            this.ribbonBar3.ContainerControlProcessDialogKey = true;
            this.ribbonBar3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ribbonBar3.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnRotate,
            this.btnExportAV,
            this.btnSelect,
            this.btnPrint,
            this.btnPrintPreview,
            this.btnLayoutPageSet,
            this.btnElementEdit,
            this.btnDeleteEle,
            this.btnAnnotation,
            this.btnGraphic,
            this.btnMapSurround,
            this.btnYWZT,
            this.btnBZTF,
            this.btnZDYFW,
            this.btnPageLayerout});
            this.ribbonBar3.Location = new System.Drawing.Point(0, 0);
            this.ribbonBar3.Name = "ribbonBar3";
            this.ribbonBar3.Size = new System.Drawing.Size(1172, 43);
            this.ribbonBar3.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.ribbonBar3.TabIndex = 4;
            // 
            // btnRotate
            // 
            this.btnRotate.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ControlsRotateElementTool;
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.SubItemsExpandWidth = 14;
            this.btnRotate.Text = "buttonItem2";
            this.btnRotate.Tooltip = "旋转元素";
            this.btnRotate.Click += new System.EventHandler(this.btnRotate_Click);
            // 
            // btnExportAV
            // 
            this.btnExportAV.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ControlsPageZoomPageToLastExtentForwardCommand;
            this.btnExportAV.Name = "btnExportAV";
            this.btnExportAV.SubItemsExpandWidth = 14;
            this.btnExportAV.Text = "输出";
            this.btnExportAV.Tooltip = "地图输出为图片";
            this.btnExportAV.Click += new System.EventHandler(this.btnExportAV_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ControlsDefaultTool;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.SubItemsExpandWidth = 14;
            this.btnSelect.Text = "btnSelect";
            this.btnSelect.Tooltip = "选择元素";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_Print;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.SubItemsExpandWidth = 14;
            this.btnPrint.Text = "buttonItem3";
            this.btnPrint.Tooltip = "打印";
            this.btnPrint.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_PrintPreview;
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.SubItemsExpandWidth = 14;
            this.btnPrintPreview.Text = "buttonItem2";
            this.btnPrintPreview.Tooltip = "打印预览";
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnLayoutPageSet
            // 
            this.btnLayoutPageSet.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ControlsPageZoomWholePageCommand;
            this.btnLayoutPageSet.Name = "btnLayoutPageSet";
            this.btnLayoutPageSet.SubItemsExpandWidth = 14;
            this.btnLayoutPageSet.Tooltip = "出图页面设置";
            this.btnLayoutPageSet.Click += new System.EventHandler(this.btnLayoutPageSet_Click);
            // 
            // btnElementEdit
            // 
            this.btnElementEdit.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ElementEditCommand;
            this.btnElementEdit.Name = "btnElementEdit";
            this.btnElementEdit.SubItemsExpandWidth = 14;
            this.btnElementEdit.Text = "buttonItem1";
            this.btnElementEdit.Tooltip = "修改元素";
            this.btnElementEdit.Click += new System.EventHandler(this.btnElementEdit_Click);
            // 
            // btnDeleteEle
            // 
            this.btnDeleteEle.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_CommandExtentView;
            this.btnDeleteEle.Name = "btnDeleteEle";
            this.btnDeleteEle.SubItemsExpandWidth = 14;
            this.btnDeleteEle.Text = "buttonItem1";
            this.btnDeleteEle.Tooltip = "删除选择的元素";
            this.btnDeleteEle.Click += new System.EventHandler(this.btnDeleteEle_Click);
            // 
            // btnAnnotation
            // 
            this.btnAnnotation.Name = "btnAnnotation";
            this.btnAnnotation.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnTextEle,
            this.btnMTextEle});
            this.btnAnnotation.SubItemsExpandWidth = 14;
            this.btnAnnotation.Text = "注记";
            this.btnAnnotation.Tooltip = "添加文字元素";
            // 
            // btnTextEle
            // 
            this.btnTextEle.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_TbTextNormal;
            this.btnTextEle.Name = "btnTextEle";
            this.btnTextEle.Text = "文本";
            this.btnTextEle.Click += new System.EventHandler(this.btnTextEle_Click);
            // 
            // btnMTextEle
            // 
            this.btnMTextEle.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_TbTextLabel;
            this.btnMTextEle.Name = "btnMTextEle";
            this.btnMTextEle.Text = "标注";
            this.btnMTextEle.Click += new System.EventHandler(this.btnMTextEle_Click);
            // 
            // btnGraphic
            // 
            this.btnGraphic.Name = "btnGraphic";
            this.btnGraphic.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnCircle,
            this.btnRectangle,
            this.btnPolygon,
            this.btnEllipse,
            this.btnZline,
            this.btnRline,
            this.btnPoint});
            this.btnGraphic.SubItemsExpandWidth = 14;
            this.btnGraphic.Text = "图形";
            this.btnGraphic.Tooltip = "添加图形元素";
            // 
            // btnCircle
            // 
            this.btnCircle.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ControlsNewCircleTool;
            this.btnCircle.Name = "btnCircle";
            this.btnCircle.Text = "圆";
            this.btnCircle.Click += new System.EventHandler(this.btnCircle_Click);
            // 
            // btnRectangle
            // 
            this.btnRectangle.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ControlsNewRectangleTool;
            this.btnRectangle.Name = "btnRectangle";
            this.btnRectangle.Text = "矩形";
            this.btnRectangle.Click += new System.EventHandler(this.btnRectangle_Click);
            // 
            // btnPolygon
            // 
            this.btnPolygon.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ControlsNewPolygonTool;
            this.btnPolygon.Name = "btnPolygon";
            this.btnPolygon.Text = "多边形";
            this.btnPolygon.Click += new System.EventHandler(this.btnPolygon_Click);
            // 
            // btnEllipse
            // 
            this.btnEllipse.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ControlsNewEllipseTool;
            this.btnEllipse.Name = "btnEllipse";
            this.btnEllipse.Text = "椭圆";
            this.btnEllipse.Click += new System.EventHandler(this.btnEllipse_Click);
            // 
            // btnZline
            // 
            this.btnZline.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ControlsNewLineTool;
            this.btnZline.Name = "btnZline";
            this.btnZline.Text = "折线";
            this.btnZline.Click += new System.EventHandler(this.btnZline_Click);
            // 
            // btnRline
            // 
            this.btnRline.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ControlsNewFreeHandTool;
            this.btnRline.Name = "btnRline";
            this.btnRline.Text = "任意线";
            this.btnRline.Click += new System.EventHandler(this.btnRline_Click);
            // 
            // btnPoint
            // 
            this.btnPoint.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_ControlsNewMarkerTool;
            this.btnPoint.Name = "btnPoint";
            this.btnPoint.Text = "点";
            this.btnPoint.Click += new System.EventHandler(this.btnPoint_Click);
            // 
            // btnMapSurround
            // 
            this.btnMapSurround.Name = "btnMapSurround";
            this.btnMapSurround.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnLegend,
            this.btnNorthArrow,
            this.btnScaleBar,
            this.btnScalebarText,
            this.btnPicture});
            this.btnMapSurround.SubItemsExpandWidth = 14;
            this.btnMapSurround.Text = "制图元素";
            this.btnMapSurround.Tooltip = "添加制图元素";
            // 
            // btnLegend
            // 
            this.btnLegend.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_AddLegendCommand;
            this.btnLegend.Name = "btnLegend";
            this.btnLegend.Text = "添加图例";
            this.btnLegend.Click += new System.EventHandler(this.btnLegend_Click);
            // 
            // btnNorthArrow
            // 
            this.btnNorthArrow.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_AddNorthArrowTool;
            this.btnNorthArrow.Name = "btnNorthArrow";
            this.btnNorthArrow.Text = "指北针";
            this.btnNorthArrow.Click += new System.EventHandler(this.btnNorthArrow_Click);
            // 
            // btnScaleBar
            // 
            this.btnScaleBar.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_AddScaleBarTool;
            this.btnScaleBar.Name = "btnScaleBar";
            this.btnScaleBar.Text = "比例尺";
            this.btnScaleBar.Click += new System.EventHandler(this.btnScaleBar_Click);
            // 
            // btnScalebarText
            // 
            this.btnScalebarText.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_AddScaleTextTool;
            this.btnScalebarText.Name = "btnScalebarText";
            this.btnScalebarText.Text = "比例尺说明";
            this.btnScalebarText.Click += new System.EventHandler(this.btnScalebarText_Click);
            // 
            // btnPicture
            // 
            this.btnPicture.Image = global::GeoPageLayout.Properties.Resources.GeoDataCenterFunLib_AddPictureCommand;
            this.btnPicture.Name = "btnPicture";
            this.btnPicture.Text = "插入图片";
            this.btnPicture.Click += new System.EventHandler(this.btnPicture_Click);
            // 
            // btnYWZT
            // 
            this.btnYWZT.Name = "btnYWZT";
            this.btnYWZT.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnTDLYXZ,
            this.btnTDLYGH});
            this.btnYWZT.SubItemsExpandWidth = 14;
            this.btnYWZT.Text = "业务制图";
            this.btnYWZT.Visible = false;
            // 
            // btnTDLYXZ
            // 
            this.btnTDLYXZ.Name = "btnTDLYXZ";
            this.btnTDLYXZ.Text = "森林资源现状图";
            // 
            // btnTDLYGH
            // 
            this.btnTDLYGH.Name = "btnTDLYGH";
            this.btnTDLYGH.Text = "森林资源规划图";
            // 
            // btnBZTF
            // 
            this.btnBZTF.Name = "btnBZTF";
            this.btnBZTF.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btn500,
            this.btn1000,
            this.btn2000,
            this.btn5K,
            this.btn1W,
            this.btn2W5,
            this.btn5W,
            this.btn25W});
            this.btnBZTF.SubItemsExpandWidth = 14;
            this.btnBZTF.Text = "标准图幅制图";
            this.btnBZTF.Visible = false;
            // 
            // btn500
            // 
            this.btn500.Name = "btn500";
            this.btn500.Text = "1:5百";
            // 
            // btn1000
            // 
            this.btn1000.Name = "btn1000";
            this.btn1000.Text = "1:1千";
            // 
            // btn2000
            // 
            this.btn2000.Name = "btn2000";
            this.btn2000.Text = "1:2千";
            // 
            // btn5K
            // 
            this.btn5K.Name = "btn5K";
            this.btn5K.Text = "1:5千";
            // 
            // btn1W
            // 
            this.btn1W.Name = "btn1W";
            this.btn1W.Text = "1:1万";
            // 
            // btn2W5
            // 
            this.btn2W5.Name = "btn2W5";
            this.btn2W5.Text = "1:2.5万";
            // 
            // btn5W
            // 
            this.btn5W.Name = "btn5W";
            this.btn5W.Text = "1:5万";
            // 
            // btn25W
            // 
            this.btn25W.Name = "btn25W";
            this.btn25W.Text = "1:25万";
            // 
            // btnZDYFW
            // 
            this.btnZDYFW.Name = "btnZDYFW";
            this.btnZDYFW.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnSelectaFeatureExtent,
            this.btnRectangleExtent,
            this.btnPolygonExtent,
            this.btnImportExtent});
            this.btnZDYFW.SubItemsExpandWidth = 14;
            this.btnZDYFW.Text = "自定义范围制图";
            this.btnZDYFW.Visible = false;
            // 
            // btnSelectaFeatureExtent
            // 
            this.btnSelectaFeatureExtent.Name = "btnSelectaFeatureExtent";
            this.btnSelectaFeatureExtent.Text = "选择要素范围";
            this.btnSelectaFeatureExtent.Visible = false;
            // 
            // btnRectangleExtent
            // 
            this.btnRectangleExtent.Name = "btnRectangleExtent";
            this.btnRectangleExtent.Text = "矩形范围";
            // 
            // btnPolygonExtent
            // 
            this.btnPolygonExtent.Name = "btnPolygonExtent";
            this.btnPolygonExtent.Text = "多边形范围";
            this.btnPolygonExtent.Visible = false;
            // 
            // btnImportExtent
            // 
            this.btnImportExtent.Name = "btnImportExtent";
            this.btnImportExtent.Text = "导入范围";
            // 
            // btnPageLayerout
            // 
            this.btnPageLayerout.Name = "btnPageLayerout";
            this.btnPageLayerout.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnManagerPageLayerout});
            this.btnPageLayerout.SubItemsExpandWidth = 14;
            this.btnPageLayerout.Text = "林业专题制图";
            // 
            // btnManagerPageLayerout
            // 
            this.btnManagerPageLayerout.Name = "btnManagerPageLayerout";
            this.btnManagerPageLayerout.Text = "管理制图方案";
            this.btnManagerPageLayerout.Click += new System.EventHandler(this.btnManagerPageLayerout_Click);
            // 
            // FrmPageLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1172, 516);
            this.Controls.Add(this.ribbonBar3);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.DoubleBuffered = true;
            this.Name = "FrmPageLayout";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "制图";
            this.Load += new System.EventHandler(this.FrmPageLayout_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmPageLayout_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPageLayout_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cMSElePro.ResumeLayout(false);
            this.cMSPageLayout.ResumeLayout(false);
            this.cMSTOC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel SSLblPageX;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel SSLabelPageY;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.ToolStripStatusLabel SSLabelMapX;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel10;
        private System.Windows.Forms.ToolStripStatusLabel SSLabelMapY;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip cMSElePro;
        private System.Windows.Forms.ToolStripMenuItem itmElePro;
        private System.Windows.Forms.ToolStripMenuItem itmGroup;
        private System.Windows.Forms.ToolStripMenuItem itmUngroup;
        private System.Windows.Forms.ContextMenuStrip cMSPageLayout;
        private System.Windows.Forms.ToolStripMenuItem itmSelectAll;
        private System.Windows.Forms.ToolStripMenuItem itmClearSelect;
        private System.Windows.Forms.ContextMenuStrip cMSTOC;
        private System.Windows.Forms.ToolStripMenuItem ItemSymbolSet;
        private System.Windows.Forms.ToolStripMenuItem ItemLyrPro;
        private System.Windows.Forms.ToolStripMenuItem itmConvertGra;
        private System.Windows.Forms.ToolTip toolTip1;
        private ESRI.ArcGIS.Controls.AxPageLayoutControl axPageLayoutControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private SplitContainer splitContainer1;
        private DevComponents.DotNetBar.RibbonBar ribbonBar3;
        private DevComponents.DotNetBar.ButtonItem btnRotate;
        private DevComponents.DotNetBar.ButtonItem btnExportAV;
        private DevComponents.DotNetBar.ButtonItem btnSelect;
        private DevComponents.DotNetBar.ButtonItem btnPrint;
        private DevComponents.DotNetBar.ButtonItem btnPrintPreview;
        private DevComponents.DotNetBar.ButtonItem btnLayoutPageSet;
        private DevComponents.DotNetBar.ButtonItem btnElementEdit;
        private DevComponents.DotNetBar.ButtonItem btnDeleteEle;
        private DevComponents.DotNetBar.ButtonItem btnAnnotation;
        private DevComponents.DotNetBar.ButtonItem btnTextEle;
        private DevComponents.DotNetBar.ButtonItem btnMTextEle;
        private DevComponents.DotNetBar.ButtonItem btnGraphic;
        private DevComponents.DotNetBar.ButtonItem btnCircle;
        private DevComponents.DotNetBar.ButtonItem btnRectangle;
        private DevComponents.DotNetBar.ButtonItem btnPolygon;
        private DevComponents.DotNetBar.ButtonItem btnEllipse;
        private DevComponents.DotNetBar.ButtonItem btnZline;
        private DevComponents.DotNetBar.ButtonItem btnRline;
        private DevComponents.DotNetBar.ButtonItem btnPoint;
        private DevComponents.DotNetBar.ButtonItem btnMapSurround;
        private DevComponents.DotNetBar.ButtonItem btnLegend;
        private DevComponents.DotNetBar.ButtonItem btnNorthArrow;
        private DevComponents.DotNetBar.ButtonItem btnScaleBar;
        private DevComponents.DotNetBar.ButtonItem btnScalebarText;
        private DevComponents.DotNetBar.ButtonItem btnPicture;
        private DevComponents.DotNetBar.ButtonItem btnYWZT;
        private DevComponents.DotNetBar.ButtonItem btnTDLYXZ;
        private DevComponents.DotNetBar.ButtonItem btnTDLYGH;
        private DevComponents.DotNetBar.ButtonItem btnBZTF;
        private DevComponents.DotNetBar.ButtonItem btn500;
        private DevComponents.DotNetBar.ButtonItem btn1000;
        private DevComponents.DotNetBar.ButtonItem btn2000;
        private DevComponents.DotNetBar.ButtonItem btn5K;
        private DevComponents.DotNetBar.ButtonItem btn1W;
        private DevComponents.DotNetBar.ButtonItem btn2W5;
        private DevComponents.DotNetBar.ButtonItem btn5W;
        private DevComponents.DotNetBar.ButtonItem btn25W;
        private DevComponents.DotNetBar.ButtonItem btnZDYFW;
        private DevComponents.DotNetBar.ButtonItem btnSelectaFeatureExtent;
        private DevComponents.DotNetBar.ButtonItem btnRectangleExtent;
        private DevComponents.DotNetBar.ButtonItem btnPolygonExtent;
        private DevComponents.DotNetBar.ButtonItem btnImportExtent;
        private DevComponents.DotNetBar.ButtonItem btnPageLayerout;
        private DevComponents.DotNetBar.ButtonItem btnManagerPageLayerout;

    }
}