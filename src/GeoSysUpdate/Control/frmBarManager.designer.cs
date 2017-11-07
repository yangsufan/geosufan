namespace GeoSysUpdate
{
    partial class frmBarManager
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
            this.dotNetBarManager = new DevComponents.DotNetBar.DotNetBarManager(this.components);
            this.barBottomDockSite = new DevComponents.DotNetBar.DockSite();
            this.barTip = new DevComponents.DotNetBar.Bar();
            this.panelDockContainer1 = new DevComponents.DotNetBar.PanelDockContainer();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStripExport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dockContainerItem4 = new DevComponents.DotNetBar.DockContainerItem();
            this.barFilldockSite = new DevComponents.DotNetBar.DockSite();
            this.barLeftDockSite = new DevComponents.DotNetBar.DockSite();
            this.barLeftTree = new DevComponents.DotNetBar.Bar();
            this.panelDockContainerLeftTree = new DevComponents.DotNetBar.PanelDockContainer();
            this.dockContainerItem3 = new DevComponents.DotNetBar.DockContainerItem();
            this.barRightDockSite = new DevComponents.DotNetBar.DockSite();
            this.toolbarBottomDockSite = new DevComponents.DotNetBar.DockSite();
            this.toolbarLeftDockSite = new DevComponents.DotNetBar.DockSite();
            this.toolbarRightDockSite = new DevComponents.DotNetBar.DockSite();
            this.toolbarTopDockSite = new DevComponents.DotNetBar.DockSite();
            this.barTopDockSite = new DevComponents.DotNetBar.DockSite();
            this.dockContainerItem1 = new DevComponents.DotNetBar.DockContainerItem();
            this.dockContainerItem2 = new DevComponents.DotNetBar.DockContainerItem();
            this.btnClose = new DevComponents.DotNetBar.ButtonX();
            this.barBottomDockSite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barTip)).BeginInit();
            this.barTip.SuspendLayout();
            this.panelDockContainer1.SuspendLayout();
            this.contextMenuStripExport.SuspendLayout();
            this.barLeftDockSite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barLeftTree)).BeginInit();
            this.barLeftTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // dotNetBarManager
            // 
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.F1);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlA);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Del);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Ins);
            this.dotNetBarManager.BottomDockSite = this.barBottomDockSite;
            this.dotNetBarManager.FillDockSite = this.barFilldockSite;
            this.dotNetBarManager.LeftDockSite = this.barLeftDockSite;
            this.dotNetBarManager.ParentForm = this;
            this.dotNetBarManager.RightDockSite = this.barRightDockSite;
            this.dotNetBarManager.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.dotNetBarManager.ToolbarBottomDockSite = this.toolbarBottomDockSite;
            this.dotNetBarManager.ToolbarLeftDockSite = this.toolbarLeftDockSite;
            this.dotNetBarManager.ToolbarRightDockSite = this.toolbarRightDockSite;
            this.dotNetBarManager.ToolbarTopDockSite = this.toolbarTopDockSite;
            this.dotNetBarManager.TopDockSite = this.barTopDockSite;
            // 
            // barBottomDockSite
            // 
            this.barBottomDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barBottomDockSite.Controls.Add(this.barTip);
            this.barBottomDockSite.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barBottomDockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer(new DevComponents.DotNetBar.DocumentBaseContainer[] {
            ((DevComponents.DotNetBar.DocumentBaseContainer)(new DevComponents.DotNetBar.DocumentBarContainer(this.barTip, 720, 126)))}, DevComponents.DotNetBar.eOrientation.Vertical);
            this.barBottomDockSite.Location = new System.Drawing.Point(0, 301);
            this.barBottomDockSite.Name = "barBottomDockSite";
            this.barBottomDockSite.Size = new System.Drawing.Size(720, 129);
            this.barBottomDockSite.TabIndex = 3;
            this.barBottomDockSite.TabStop = false;
            // 
            // barTip
            // 
            this.barTip.AccessibleDescription = "DotNetBar Bar (barTip)";
            this.barTip.AccessibleName = "DotNetBar Bar";
            this.barTip.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.barTip.AutoHide = true;
            this.barTip.AutoSyncBarCaption = true;
            this.barTip.CanReorderTabs = false;
            this.barTip.CloseSingleTab = true;
            this.barTip.Controls.Add(this.btnClose);
            this.barTip.Controls.Add(this.panelDockContainer1);
            this.barTip.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Caption;
            this.barTip.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.dockContainerItem4});
            this.barTip.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            this.barTip.Location = new System.Drawing.Point(0, 3);
            this.barTip.Name = "barTip";
            this.barTip.Size = new System.Drawing.Size(720, 126);
            this.barTip.Stretch = true;
            this.barTip.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.barTip.TabIndex = 0;
            this.barTip.TabStop = false;
            this.barTip.Text = "提示信息";
            // 
            // panelDockContainer1
            // 
            this.panelDockContainer1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelDockContainer1.Controls.Add(this.richTextBox1);
            this.panelDockContainer1.Location = new System.Drawing.Point(3, 23);
            this.panelDockContainer1.Name = "panelDockContainer1";
            this.panelDockContainer1.Size = new System.Drawing.Size(714, 100);
            this.panelDockContainer1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelDockContainer1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelDockContainer1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelDockContainer1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelDockContainer1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelDockContainer1.Style.GradientAngle = 90;
            this.panelDockContainer1.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.ContextMenuStrip = this.contextMenuStripExport;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(714, 100);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // contextMenuStripExport
            // 
            this.contextMenuStripExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出日志ToolStripMenuItem});
            this.contextMenuStripExport.Name = "contextMenuStripExport";
            this.contextMenuStripExport.Size = new System.Drawing.Size(123, 26);
            // 
            // 导出日志ToolStripMenuItem
            // 
            this.导出日志ToolStripMenuItem.Name = "导出日志ToolStripMenuItem";
            this.导出日志ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.导出日志ToolStripMenuItem.Text = "导出日志";
            this.导出日志ToolStripMenuItem.Click += new System.EventHandler(this.导出日志ToolStripMenuItem_Click);
            // 
            // dockContainerItem4
            // 
            this.dockContainerItem4.Control = this.panelDockContainer1;
            this.dockContainerItem4.Name = "dockContainerItem4";
            this.dockContainerItem4.Text = "提示信息";
            // 
            // barFilldockSite
            // 
            this.barFilldockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barFilldockSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barFilldockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.barFilldockSite.Location = new System.Drawing.Point(233, 0);
            this.barFilldockSite.Name = "barFilldockSite";
            this.barFilldockSite.Size = new System.Drawing.Size(487, 301);
            this.barFilldockSite.TabIndex = 4;
            this.barFilldockSite.TabStop = false;
            // 
            // barLeftDockSite
            // 
            this.barLeftDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barLeftDockSite.Controls.Add(this.barLeftTree);
            this.barLeftDockSite.Dock = System.Windows.Forms.DockStyle.Left;
            this.barLeftDockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer(new DevComponents.DotNetBar.DocumentBaseContainer[] {
            ((DevComponents.DotNetBar.DocumentBaseContainer)(new DevComponents.DotNetBar.DocumentBarContainer(this.barLeftTree, 230, 301)))}, DevComponents.DotNetBar.eOrientation.Horizontal);
            this.barLeftDockSite.Location = new System.Drawing.Point(0, 0);
            this.barLeftDockSite.Name = "barLeftDockSite";
            this.barLeftDockSite.Size = new System.Drawing.Size(233, 301);
            this.barLeftDockSite.TabIndex = 0;
            this.barLeftDockSite.TabStop = false;
            // 
            // barLeftTree
            // 
            this.barLeftTree.AccessibleDescription = "DotNetBar Bar (barLeftTree)";
            this.barLeftTree.AccessibleName = "DotNetBar Bar";
            this.barLeftTree.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.barLeftTree.AutoSyncBarCaption = true;
            this.barLeftTree.CloseSingleTab = true;
            this.barLeftTree.Controls.Add(this.panelDockContainerLeftTree);
            this.barLeftTree.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Caption;
            this.barLeftTree.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.dockContainerItem3});
            this.barLeftTree.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            this.barLeftTree.Location = new System.Drawing.Point(0, 0);
            this.barLeftTree.Name = "barLeftTree";
            this.barLeftTree.Size = new System.Drawing.Size(230, 301);
            this.barLeftTree.Stretch = true;
            this.barLeftTree.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.barLeftTree.TabIndex = 0;
            this.barLeftTree.TabStop = false;
            this.barLeftTree.Text = "目录管理";
            // 
            // panelDockContainerLeftTree
            // 
            this.panelDockContainerLeftTree.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelDockContainerLeftTree.Location = new System.Drawing.Point(3, 23);
            this.panelDockContainerLeftTree.Name = "panelDockContainerLeftTree";
            this.panelDockContainerLeftTree.Size = new System.Drawing.Size(224, 275);
            this.panelDockContainerLeftTree.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelDockContainerLeftTree.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelDockContainerLeftTree.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelDockContainerLeftTree.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelDockContainerLeftTree.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelDockContainerLeftTree.Style.GradientAngle = 90;
            this.panelDockContainerLeftTree.TabIndex = 0;
            this.panelDockContainerLeftTree.Text = "目录管理";
            // 
            // dockContainerItem3
            // 
            this.dockContainerItem3.Control = this.panelDockContainerLeftTree;
            this.dockContainerItem3.Name = "dockContainerItem3";
            this.dockContainerItem3.Text = "目录管理";
            // 
            // barRightDockSite
            // 
            this.barRightDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barRightDockSite.Dock = System.Windows.Forms.DockStyle.Right;
            this.barRightDockSite.Location = new System.Drawing.Point(720, 0);
            this.barRightDockSite.Name = "barRightDockSite";
            this.barRightDockSite.Size = new System.Drawing.Size(0, 301);
            this.barRightDockSite.TabIndex = 1;
            this.barRightDockSite.TabStop = false;
            // 
            // toolbarBottomDockSite
            // 
            this.toolbarBottomDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.toolbarBottomDockSite.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolbarBottomDockSite.Location = new System.Drawing.Point(0, 430);
            this.toolbarBottomDockSite.Name = "toolbarBottomDockSite";
            this.toolbarBottomDockSite.Size = new System.Drawing.Size(720, 0);
            this.toolbarBottomDockSite.TabIndex = 8;
            this.toolbarBottomDockSite.TabStop = false;
            // 
            // toolbarLeftDockSite
            // 
            this.toolbarLeftDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.toolbarLeftDockSite.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolbarLeftDockSite.Location = new System.Drawing.Point(0, 0);
            this.toolbarLeftDockSite.Name = "toolbarLeftDockSite";
            this.toolbarLeftDockSite.Size = new System.Drawing.Size(0, 430);
            this.toolbarLeftDockSite.TabIndex = 5;
            this.toolbarLeftDockSite.TabStop = false;
            // 
            // toolbarRightDockSite
            // 
            this.toolbarRightDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.toolbarRightDockSite.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolbarRightDockSite.Location = new System.Drawing.Point(720, 0);
            this.toolbarRightDockSite.Name = "toolbarRightDockSite";
            this.toolbarRightDockSite.Size = new System.Drawing.Size(0, 430);
            this.toolbarRightDockSite.TabIndex = 6;
            this.toolbarRightDockSite.TabStop = false;
            // 
            // toolbarTopDockSite
            // 
            this.toolbarTopDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.toolbarTopDockSite.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolbarTopDockSite.Location = new System.Drawing.Point(0, 0);
            this.toolbarTopDockSite.Name = "toolbarTopDockSite";
            this.toolbarTopDockSite.Size = new System.Drawing.Size(720, 0);
            this.toolbarTopDockSite.TabIndex = 7;
            this.toolbarTopDockSite.TabStop = false;
            // 
            // barTopDockSite
            // 
            this.barTopDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barTopDockSite.Dock = System.Windows.Forms.DockStyle.Top;
            this.barTopDockSite.Location = new System.Drawing.Point(0, 0);
            this.barTopDockSite.Name = "barTopDockSite";
            this.barTopDockSite.Size = new System.Drawing.Size(720, 0);
            this.barTopDockSite.TabIndex = 2;
            this.barTopDockSite.TabStop = false;
            // 
            // dockContainerItem1
            // 
            this.dockContainerItem1.Name = "dockContainerItem1";
            this.dockContainerItem1.Text = "dockContainerItem1";
            // 
            // dockContainerItem2
            // 
            this.dockContainerItem2.Name = "dockContainerItem2";
            this.dockContainerItem2.Text = "dockContainerItem2";
            // 
            // btnClose
            // 
            this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClose.Location = new System.Drawing.Point(653, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 20);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmBarManager
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(720, 430);
            this.Controls.Add(this.barFilldockSite);
            this.Controls.Add(this.barLeftDockSite);
            this.Controls.Add(this.barRightDockSite);
            this.Controls.Add(this.barTopDockSite);
            this.Controls.Add(this.barBottomDockSite);
            this.Controls.Add(this.toolbarLeftDockSite);
            this.Controls.Add(this.toolbarRightDockSite);
            this.Controls.Add(this.toolbarTopDockSite);
            this.Controls.Add(this.toolbarBottomDockSite);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmBarManager";
            this.Text = "Show Case of Document Docking";
            this.barBottomDockSite.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barTip)).EndInit();
            this.barTip.ResumeLayout(false);
            this.panelDockContainer1.ResumeLayout(false);
            this.contextMenuStripExport.ResumeLayout(false);
            this.barLeftDockSite.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barLeftTree)).EndInit();
            this.barLeftTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.DotNetBarManager dotNetBarManager;
        private DevComponents.DotNetBar.DockSite barLeftDockSite;
        private DevComponents.DotNetBar.DockSite barRightDockSite;
        private DevComponents.DotNetBar.DockSite barTopDockSite;
        private DevComponents.DotNetBar.DockSite barBottomDockSite;
        private DevComponents.DotNetBar.DockSite barFilldockSite;
        private DevComponents.DotNetBar.DockSite toolbarLeftDockSite;
        private DevComponents.DotNetBar.DockSite toolbarRightDockSite;
        private DevComponents.DotNetBar.DockSite toolbarTopDockSite;
        private DevComponents.DotNetBar.DockSite toolbarBottomDockSite;
        private DevComponents.DotNetBar.Bar barTip;
        private DevComponents.DotNetBar.DockContainerItem dockContainerItem1;
        private DevComponents.DotNetBar.DockContainerItem dockContainerItem2;
        private DevComponents.DotNetBar.Bar barLeftTree;
        private DevComponents.DotNetBar.PanelDockContainer panelDockContainerLeftTree;
        private DevComponents.DotNetBar.DockContainerItem dockContainerItem3;
        private DevComponents.DotNetBar.PanelDockContainer panelDockContainer1;
        private DevComponents.DotNetBar.DockContainerItem dockContainerItem4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripExport;
        private System.Windows.Forms.ToolStripMenuItem 导出日志ToolStripMenuItem;
        private DevComponents.DotNetBar.ButtonX btnClose;

    }
}