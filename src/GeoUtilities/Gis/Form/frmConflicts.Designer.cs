namespace GeoUtilities
{
    partial class frmConflicts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConflicts));
            this.itemPanelDataList = new DevComponents.DotNetBar.ItemPanel();
            this.dataGridViewX = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.advTree = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.expandablePanel = new DevComponents.DotNetBar.ExpandablePanel();
            this.labelXCommonVersion = new DevComponents.DotNetBar.LabelX();
            this.labelXPreVersion = new DevComponents.DotNetBar.LabelX();
            this.labelXCurrent = new DevComponents.DotNetBar.LabelX();
            this.axToolbarControlCommon = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axMapControlCommon = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControlPre = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axMapControlPre = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControlCurrent = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axMapControlCurrent = new ESRI.ArcGIS.Controls.AxMapControl();
            this.contextMenuBar = new DevComponents.DotNetBar.ContextMenuBar();
            this.btnTree = new DevComponents.DotNetBar.ButtonItem();
            this.btnCurrent = new DevComponents.DotNetBar.ButtonItem();
            this.btnPre = new DevComponents.DotNetBar.ButtonItem();
            this.btnZoomToCurrent = new DevComponents.DotNetBar.ButtonItem();
            this.btnZoomToPre = new DevComponents.DotNetBar.ButtonItem();
            this.btnZoomToCommon = new DevComponents.DotNetBar.ButtonItem();
            this.btnShow = new DevComponents.DotNetBar.ButtonItem();
            this.btnShowCurrent = new DevComponents.DotNetBar.ButtonItem();
            this.btnShowPre = new DevComponents.DotNetBar.ButtonItem();
            this.btnShowCommon = new DevComponents.DotNetBar.ButtonItem();
            this.node1 = new DevComponents.AdvTree.Node();
            this.itemPanelDataList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX)).BeginInit();
            this.itemPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTree)).BeginInit();
            this.expandablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControlCommon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControlCommon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControlPre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControlPre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControlCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControlCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar)).BeginInit();
            this.SuspendLayout();
            // 
            // itemPanelDataList
            // 
            // 
            // 
            // 
            this.itemPanelDataList.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.itemPanelDataList.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelDataList.BackgroundStyle.BorderBottomWidth = 1;
            this.itemPanelDataList.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.itemPanelDataList.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelDataList.BackgroundStyle.BorderLeftWidth = 1;
            this.itemPanelDataList.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelDataList.BackgroundStyle.BorderRightWidth = 1;
            this.itemPanelDataList.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelDataList.BackgroundStyle.BorderTopWidth = 1;
            this.itemPanelDataList.BackgroundStyle.PaddingBottom = 1;
            this.itemPanelDataList.BackgroundStyle.PaddingLeft = 1;
            this.itemPanelDataList.BackgroundStyle.PaddingRight = 1;
            this.itemPanelDataList.BackgroundStyle.PaddingTop = 1;
            this.itemPanelDataList.ContainerControlProcessDialogKey = true;
            this.itemPanelDataList.Controls.Add(this.dataGridViewX);
            this.itemPanelDataList.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanelDataList.Location = new System.Drawing.Point(203, 5);
            this.itemPanelDataList.Name = "itemPanelDataList";
            this.itemPanelDataList.Size = new System.Drawing.Size(480, 445);
            this.itemPanelDataList.TabIndex = 1;
            this.itemPanelDataList.Text = "itemPanel2";
            // 
            // dataGridViewX
            // 
            this.dataGridViewX.AllowUserToAddRows = false;
            this.dataGridViewX.AllowUserToDeleteRows = false;
            this.dataGridViewX.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewX.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewX.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewX.Name = "dataGridViewX";
            this.dataGridViewX.ReadOnly = true;
            this.dataGridViewX.RowHeadersWidth = 18;
            this.dataGridViewX.RowTemplate.Height = 23;
            this.dataGridViewX.Size = new System.Drawing.Size(480, 445);
            this.dataGridViewX.TabIndex = 0;
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
            this.itemPanel1.Controls.Add(this.advTree);
            this.itemPanel1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanel1.Location = new System.Drawing.Point(0, 5);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(199, 443);
            this.itemPanel1.TabIndex = 3;
            this.itemPanel1.Text = "itemPanel1";
            // 
            // advTree
            // 
            this.advTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTree.AllowDrop = true;
            this.advTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTree.BackgroundStyle.Class = "TreeBorderKey";
            this.advTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTree.Location = new System.Drawing.Point(0, 0);
            this.advTree.Name = "advTree";
            this.advTree.NodesConnector = this.nodeConnector1;
            this.advTree.NodeStyle = this.elementStyle1;
            this.advTree.PathSeparator = ";";
            this.advTree.Size = new System.Drawing.Size(199, 443);
            this.advTree.Styles.Add(this.elementStyle1);
            this.advTree.TabIndex = 6;
            this.advTree.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTree_NodeDoubleClick);
            this.advTree.NodeMouseUp += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTree_NodeMouseUp);
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
            // expandablePanel
            // 
            this.expandablePanel.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.expandablePanel.Controls.Add(this.labelXCommonVersion);
            this.expandablePanel.Controls.Add(this.labelXPreVersion);
            this.expandablePanel.Controls.Add(this.labelXCurrent);
            this.expandablePanel.Controls.Add(this.axToolbarControlCommon);
            this.expandablePanel.Controls.Add(this.axMapControlCommon);
            this.expandablePanel.Controls.Add(this.axToolbarControlPre);
            this.expandablePanel.Controls.Add(this.axMapControlPre);
            this.expandablePanel.Controls.Add(this.axToolbarControlCurrent);
            this.expandablePanel.Controls.Add(this.axMapControlCurrent);
            this.expandablePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.expandablePanel.Expanded = false;
            this.expandablePanel.ExpandedBounds = new System.Drawing.Rectangle(0, 217, 681, 265);
            this.expandablePanel.Location = new System.Drawing.Point(0, 456);
            this.expandablePanel.Name = "expandablePanel";
            this.expandablePanel.Size = new System.Drawing.Size(681, 26);
            this.expandablePanel.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.expandablePanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.expandablePanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandablePanel.Style.GradientAngle = 90;
            this.expandablePanel.TabIndex = 4;
            this.expandablePanel.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.expandablePanel.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel.TitleStyle.GradientAngle = 90;
            this.expandablePanel.TitleText = "要素图形对比";
            this.expandablePanel.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.expandablePanel_ExpandedChanged);
            // 
            // labelXCommonVersion
            // 
            this.labelXCommonVersion.AutoSize = true;
            this.labelXCommonVersion.Location = new System.Drawing.Point(532, 238);
            this.labelXCommonVersion.Name = "labelXCommonVersion";
            this.labelXCommonVersion.Size = new System.Drawing.Size(74, 18);
            this.labelXCommonVersion.TabIndex = 15;
            this.labelXCommonVersion.Text = "最 初 版 本";
            // 
            // labelXPreVersion
            // 
            this.labelXPreVersion.AutoSize = true;
            this.labelXPreVersion.Location = new System.Drawing.Point(304, 238);
            this.labelXPreVersion.Name = "labelXPreVersion";
            this.labelXPreVersion.Size = new System.Drawing.Size(74, 18);
            this.labelXPreVersion.TabIndex = 14;
            this.labelXPreVersion.Text = "前 一 版 本";
            // 
            // labelXCurrent
            // 
            this.labelXCurrent.AutoSize = true;
            this.labelXCurrent.Location = new System.Drawing.Point(75, 238);
            this.labelXCurrent.Name = "labelXCurrent";
            this.labelXCurrent.Size = new System.Drawing.Size(74, 18);
            this.labelXCurrent.TabIndex = 13;
            this.labelXCurrent.Text = "当 前 版 本";
            // 
            // axToolbarControlCommon
            // 
            this.axToolbarControlCommon.Location = new System.Drawing.Point(461, 39);
            this.axToolbarControlCommon.Name = "axToolbarControlCommon";
            this.axToolbarControlCommon.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControlCommon.OcxState")));
            this.axToolbarControlCommon.Size = new System.Drawing.Size(208, 28);
            this.axToolbarControlCommon.TabIndex = 12;
            // 
            // axMapControlCommon
            // 
            this.axMapControlCommon.Location = new System.Drawing.Point(461, 66);
            this.axMapControlCommon.Name = "axMapControlCommon";
            this.axMapControlCommon.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControlCommon.OcxState")));
            this.axMapControlCommon.Size = new System.Drawing.Size(208, 166);
            this.axMapControlCommon.TabIndex = 11;
            // 
            // axToolbarControlPre
            // 
            this.axToolbarControlPre.Location = new System.Drawing.Point(237, 39);
            this.axToolbarControlPre.Name = "axToolbarControlPre";
            this.axToolbarControlPre.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControlPre.OcxState")));
            this.axToolbarControlPre.Size = new System.Drawing.Size(208, 28);
            this.axToolbarControlPre.TabIndex = 10;
            // 
            // axMapControlPre
            // 
            this.axMapControlPre.Location = new System.Drawing.Point(237, 66);
            this.axMapControlPre.Name = "axMapControlPre";
            this.axMapControlPre.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControlPre.OcxState")));
            this.axMapControlPre.Size = new System.Drawing.Size(208, 166);
            this.axMapControlPre.TabIndex = 9;
            // 
            // axToolbarControlCurrent
            // 
            this.axToolbarControlCurrent.Location = new System.Drawing.Point(12, 39);
            this.axToolbarControlCurrent.Name = "axToolbarControlCurrent";
            this.axToolbarControlCurrent.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControlCurrent.OcxState")));
            this.axToolbarControlCurrent.Size = new System.Drawing.Size(208, 28);
            this.axToolbarControlCurrent.TabIndex = 8;
            // 
            // axMapControlCurrent
            // 
            this.axMapControlCurrent.Location = new System.Drawing.Point(12, 66);
            this.axMapControlCurrent.Name = "axMapControlCurrent";
            this.axMapControlCurrent.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControlCurrent.OcxState")));
            this.axMapControlCurrent.Size = new System.Drawing.Size(208, 166);
            this.axMapControlCurrent.TabIndex = 7;
            // 
            // contextMenuBar
            // 
            this.contextMenuBar.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            this.contextMenuBar.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnTree});
            this.contextMenuBar.Location = new System.Drawing.Point(192, 140);
            this.contextMenuBar.Name = "contextMenuBar";
            this.contextMenuBar.Size = new System.Drawing.Size(75, 25);
            this.contextMenuBar.Stretch = true;
            this.contextMenuBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.contextMenuBar.TabIndex = 5;
            this.contextMenuBar.TabStop = false;
            this.contextMenuBar.Text = "树图右键菜单";
            // 
            // btnTree
            // 
            this.btnTree.AutoExpandOnClick = true;
            this.btnTree.Name = "btnTree";
            this.btnTree.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnCurrent,
            this.btnPre,
            this.btnZoomToCurrent,
            this.btnZoomToPre,
            this.btnZoomToCommon,
            this.btnShow});
            this.btnTree.Text = "树图右键菜单";
            // 
            // btnCurrent
            // 
            this.btnCurrent.Name = "btnCurrent";
            this.btnCurrent.Text = "以当前版本进行替换";
            // 
            // btnPre
            // 
            this.btnPre.Name = "btnPre";
            this.btnPre.Text = "以前一版本进行替换";
            // 
            // btnZoomToCurrent
            // 
            this.btnZoomToCurrent.BeginGroup = true;
            this.btnZoomToCurrent.Name = "btnZoomToCurrent";
            this.btnZoomToCurrent.Text = "缩放到当前版本";
            // 
            // btnZoomToPre
            // 
            this.btnZoomToPre.Name = "btnZoomToPre";
            this.btnZoomToPre.Text = "缩放到前一版本";
            // 
            // btnZoomToCommon
            // 
            this.btnZoomToCommon.Name = "btnZoomToCommon";
            this.btnZoomToCommon.Text = "缩放到最初版本";
            // 
            // btnShow
            // 
            this.btnShow.BeginGroup = true;
            this.btnShow.Name = "btnShow";
            this.btnShow.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnShowCurrent,
            this.btnShowPre,
            this.btnShowCommon});
            this.btnShow.Text = "控制显示版本";
            // 
            // btnShowCurrent
            // 
            this.btnShowCurrent.Name = "btnShowCurrent";
            this.btnShowCurrent.Text = "当前版本";
            // 
            // btnShowPre
            // 
            this.btnShowPre.Name = "btnShowPre";
            this.btnShowPre.Text = "前一版本";
            // 
            // btnShowCommon
            // 
            this.btnShowCommon.Name = "btnShowCommon";
            this.btnShowCommon.Text = "最初版本";
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.HostedControl = this.contextMenuBar;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
            // 
            // frmConflicts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 482);
            this.Controls.Add(this.expandablePanel);
            this.Controls.Add(this.itemPanel1);
            this.Controls.Add(this.itemPanelDataList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmConflicts";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "要素冲突协调";
            this.itemPanelDataList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX)).EndInit();
            this.itemPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTree)).EndInit();
            this.expandablePanel.ResumeLayout(false);
            this.expandablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControlCommon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControlCommon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControlPre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControlPre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControlCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControlCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ItemPanel itemPanelDataList;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX;
        private DevComponents.DotNetBar.ItemPanel itemPanel1;
        private DevComponents.AdvTree.AdvTree advTree;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.ExpandablePanel expandablePanel;
        private DevComponents.DotNetBar.LabelX labelXCommonVersion;
        private DevComponents.DotNetBar.LabelX labelXPreVersion;
        private DevComponents.DotNetBar.LabelX labelXCurrent;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControlCommon;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControlCommon;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControlPre;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControlPre;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControlCurrent;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControlCurrent;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar;
        private DevComponents.DotNetBar.ButtonItem btnTree;
        private DevComponents.DotNetBar.ButtonItem btnCurrent;
        private DevComponents.DotNetBar.ButtonItem btnPre;
        private DevComponents.DotNetBar.ButtonItem btnZoomToCurrent;
        private DevComponents.DotNetBar.ButtonItem btnZoomToPre;
        private DevComponents.DotNetBar.ButtonItem btnZoomToCommon;
        private DevComponents.DotNetBar.ButtonItem btnShow;
        private DevComponents.DotNetBar.ButtonItem btnShowCurrent;
        private DevComponents.DotNetBar.ButtonItem btnShowPre;
        private DevComponents.DotNetBar.ButtonItem btnShowCommon;
        private DevComponents.AdvTree.Node node1;


    }
}