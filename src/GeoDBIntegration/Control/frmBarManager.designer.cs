namespace GeoDBIntegration
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
            this.barFilldockSite = new DevComponents.DotNetBar.DockSite();
            this.barLeftDockSite = new DevComponents.DotNetBar.DockSite();
            this.barRightDockSite = new DevComponents.DotNetBar.DockSite();
            this.toolbarBottomDockSite = new DevComponents.DotNetBar.DockSite();
            this.toolbarLeftDockSite = new DevComponents.DotNetBar.DockSite();
            this.toolbarRightDockSite = new DevComponents.DotNetBar.DockSite();
            this.toolbarTopDockSite = new DevComponents.DotNetBar.DockSite();
            this.barTopDockSite = new DevComponents.DotNetBar.DockSite();
            this.dockContainerItem1 = new DevComponents.DotNetBar.DockContainerItem();
            this.barBottomDockSite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barTip)).BeginInit();
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
            this.dotNetBarManager.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
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
            this.barTip.CloseSingleTab = true;
            this.barTip.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Caption;
            this.barTip.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            this.barTip.Location = new System.Drawing.Point(0, 3);
            this.barTip.Name = "barTip";
            this.barTip.Size = new System.Drawing.Size(720, 126);
            this.barTip.Stretch = true;
            this.barTip.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barTip.TabIndex = 0;
            this.barTip.TabStop = false;
            this.barTip.Text = "提示窗口";
            // 
            // barFilldockSite
            // 
            this.barFilldockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barFilldockSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barFilldockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.barFilldockSite.Location = new System.Drawing.Point(0, 0);
            this.barFilldockSite.Name = "barFilldockSite";
            this.barFilldockSite.Size = new System.Drawing.Size(720, 301);
            this.barFilldockSite.TabIndex = 4;
            this.barFilldockSite.TabStop = false;
            // 
            // barLeftDockSite
            // 
            this.barLeftDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barLeftDockSite.Dock = System.Windows.Forms.DockStyle.Left;
            this.barLeftDockSite.Location = new System.Drawing.Point(0, 0);
            this.barLeftDockSite.Name = "barLeftDockSite";
            this.barLeftDockSite.Size = new System.Drawing.Size(0, 301);
            this.barLeftDockSite.TabIndex = 0;
            this.barLeftDockSite.TabStop = false;
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

    }
}