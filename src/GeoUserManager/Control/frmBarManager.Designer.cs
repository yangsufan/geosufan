namespace GeoUserManager
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dotNetBarManager = new DevComponents.DotNetBar.DotNetBarManager(this.components);
            this.dockSiteBottom = new DevComponents.DotNetBar.DockSite();
            this.tipBar = new DevComponents.DotNetBar.Bar();
            this.dockSiteFill = new DevComponents.DotNetBar.DockSite();
            this.dockSiteLeft = new DevComponents.DotNetBar.DockSite();
            this.dockSiteRight = new DevComponents.DotNetBar.DockSite();
            this.toolDockSiteBottom = new DevComponents.DotNetBar.DockSite();
            this.toolDockSiteLeft = new DevComponents.DotNetBar.DockSite();
            this.toolDockSiteRight = new DevComponents.DotNetBar.DockSite();
            this.toolDockSiteTop = new DevComponents.DotNetBar.DockSite();
            this.dockSiteTop = new DevComponents.DotNetBar.DockSite();
            this.dockContainerItem1 = new DevComponents.DotNetBar.DockContainerItem();
            //changed by chulili 20110622
            //this.dockContainerItem2 = new DevComponents.DotNetBar.DockContainerItem();
            this.panelDockContainer1 = new DevComponents.DotNetBar.PanelDockContainer();
            this.dockSiteBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tipBar)).BeginInit();
            this.tipBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // dotNetBarManager
            // 
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Ins);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Del);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.F1);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlA);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlY);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ);
            this.dotNetBarManager.BottomDockSite = this.dockSiteBottom;
            this.dotNetBarManager.EnableFullSizeDock = false;
            this.dotNetBarManager.FillDockSite = this.dockSiteFill;
            this.dotNetBarManager.LeftDockSite = this.dockSiteLeft;
            this.dotNetBarManager.ParentForm = this;
            this.dotNetBarManager.ParentUserControl = this;
            this.dotNetBarManager.RightDockSite = this.dockSiteRight;
            this.dotNetBarManager.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.dotNetBarManager.ToolbarBottomDockSite = this.toolDockSiteBottom;
            this.dotNetBarManager.ToolbarLeftDockSite = this.toolDockSiteLeft;
            this.dotNetBarManager.ToolbarRightDockSite = this.toolDockSiteRight;
            this.dotNetBarManager.ToolbarTopDockSite = this.toolDockSiteTop;
            this.dotNetBarManager.TopDockSite = this.dockSiteTop;
            // 
            // dockSiteBottom
            // 
            this.dockSiteBottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSiteBottom.Controls.Add(this.tipBar);
            this.dockSiteBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dockSiteBottom.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer(new DevComponents.DotNetBar.DocumentBaseContainer[] {
            ((DevComponents.DotNetBar.DocumentBaseContainer)(new DevComponents.DotNetBar.DocumentBarContainer(this.tipBar, 731, 108)))}, DevComponents.DotNetBar.eOrientation.Vertical);
            this.dockSiteBottom.Location = new System.Drawing.Point(0, 277);
            this.dockSiteBottom.Name = "dockSiteBottom";
            this.dockSiteBottom.Size = new System.Drawing.Size(731, 111);
            this.dockSiteBottom.TabIndex = 3;
            this.dockSiteBottom.TabStop = false;
            // 
            // tipBar
            // 
            this.tipBar.AccessibleDescription = "DotNetBar Bar (tipBar)";
            this.tipBar.AccessibleName = "DotNetBar Bar";
            this.tipBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.tipBar.AutoHide = true;
            this.tipBar.AutoSyncBarCaption = true;
            this.tipBar.CloseSingleTab = true;
            this.tipBar.Controls.Add(this.panelDockContainer1);
            this.tipBar.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Caption;
            //changed by chulili 20110622
            //this.tipBar.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            //this.dockContainerItem2});
            this.tipBar.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            this.tipBar.Location = new System.Drawing.Point(0, 3);
            this.tipBar.Name = "tipBar";
            this.tipBar.Size = new System.Drawing.Size(731, 108);
            this.tipBar.Stretch = true;
            this.tipBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.tipBar.TabIndex = 8;
            this.tipBar.TabStop = false;
            this.tipBar.Text = "提示";
            // 
            // dockSiteFill
            // 
            this.dockSiteFill.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSiteFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockSiteFill.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.dockSiteFill.Location = new System.Drawing.Point(0, 0);
            this.dockSiteFill.Name = "dockSiteFill";
            this.dockSiteFill.Size = new System.Drawing.Size(731, 277);
            this.dockSiteFill.TabIndex = 8;
            this.dockSiteFill.TabStop = false;
            // 
            // dockSiteLeft
            // 
            this.dockSiteLeft.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSiteLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.dockSiteLeft.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.dockSiteLeft.Location = new System.Drawing.Point(0, 0);
            this.dockSiteLeft.Name = "dockSiteLeft";
            this.dockSiteLeft.Size = new System.Drawing.Size(0, 277);
            this.dockSiteLeft.TabIndex = 0;
            this.dockSiteLeft.TabStop = false;
            // 
            // dockSiteRight
            // 
            this.dockSiteRight.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSiteRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.dockSiteRight.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.dockSiteRight.Location = new System.Drawing.Point(731, 0);
            this.dockSiteRight.Name = "dockSiteRight";
            this.dockSiteRight.Size = new System.Drawing.Size(0, 277);
            this.dockSiteRight.TabIndex = 1;
            this.dockSiteRight.TabStop = false;
            // 
            // toolDockSiteBottom
            // 
            this.toolDockSiteBottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.toolDockSiteBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolDockSiteBottom.Location = new System.Drawing.Point(0, 388);
            this.toolDockSiteBottom.Name = "toolDockSiteBottom";
            this.toolDockSiteBottom.Size = new System.Drawing.Size(731, 0);
            this.toolDockSiteBottom.TabIndex = 7;
            this.toolDockSiteBottom.TabStop = false;
            // 
            // toolDockSiteLeft
            // 
            this.toolDockSiteLeft.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.toolDockSiteLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolDockSiteLeft.Location = new System.Drawing.Point(0, 0);
            this.toolDockSiteLeft.Name = "toolDockSiteLeft";
            this.toolDockSiteLeft.Size = new System.Drawing.Size(0, 388);
            this.toolDockSiteLeft.TabIndex = 4;
            this.toolDockSiteLeft.TabStop = false;
            // 
            // toolDockSiteRight
            // 
            this.toolDockSiteRight.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.toolDockSiteRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolDockSiteRight.Location = new System.Drawing.Point(731, 0);
            this.toolDockSiteRight.Name = "toolDockSiteRight";
            this.toolDockSiteRight.Size = new System.Drawing.Size(0, 388);
            this.toolDockSiteRight.TabIndex = 5;
            this.toolDockSiteRight.TabStop = false;
            // 
            // toolDockSiteTop
            // 
            this.toolDockSiteTop.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.toolDockSiteTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolDockSiteTop.Location = new System.Drawing.Point(0, 0);
            this.toolDockSiteTop.Name = "toolDockSiteTop";
            this.toolDockSiteTop.Size = new System.Drawing.Size(731, 0);
            this.toolDockSiteTop.TabIndex = 6;
            this.toolDockSiteTop.TabStop = false;
            // 
            // dockSiteTop
            // 
            this.dockSiteTop.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSiteTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.dockSiteTop.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.dockSiteTop.Location = new System.Drawing.Point(0, 0);
            this.dockSiteTop.Name = "dockSiteTop";
            this.dockSiteTop.Size = new System.Drawing.Size(731, 0);
            this.dockSiteTop.TabIndex = 2;
            this.dockSiteTop.TabStop = false;
            // 
            // dockContainerItem1
            // 
            this.dockContainerItem1.Name = "dockContainerItem1";
            this.dockContainerItem1.Text = "dockContainerItem1";
            // 
            // dockContainerItem2
            // //changed by chulili 20110622
            //this.dockContainerItem2.Control = this.panelDockContainer1;
            //this.dockContainerItem2.Name = "dockContainerItem2";
            //this.dockContainerItem2.Text = "dockContainerItem2";
            // 
            // panelDockContainer1
            // 
            this.panelDockContainer1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelDockContainer1.Location = new System.Drawing.Point(3, 23);
            this.panelDockContainer1.Name = "panelDockContainer1";
            this.panelDockContainer1.Size = new System.Drawing.Size(725, 82);
            this.panelDockContainer1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelDockContainer1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelDockContainer1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelDockContainer1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelDockContainer1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelDockContainer1.Style.GradientAngle = 90;
            this.panelDockContainer1.TabIndex = 0;
            // 
            // frmBarManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 388);
            this.Controls.Add(this.dockSiteFill);
            this.Controls.Add(this.dockSiteRight);
            this.Controls.Add(this.dockSiteLeft);
            this.Controls.Add(this.dockSiteTop);
            this.Controls.Add(this.dockSiteBottom);
            this.Controls.Add(this.toolDockSiteLeft);
            this.Controls.Add(this.toolDockSiteRight);
            this.Controls.Add(this.toolDockSiteTop);
            this.Controls.Add(this.toolDockSiteBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmBarManager";
            this.dockSiteBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tipBar)).EndInit();
            this.tipBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.DotNetBarManager dotNetBarManager;
        private DevComponents.DotNetBar.DockSite dockSiteBottom;
        private DevComponents.DotNetBar.DockSite dockSiteLeft;
        private DevComponents.DotNetBar.DockSite dockSiteRight;
        private DevComponents.DotNetBar.DockSite dockSiteTop;
        private DevComponents.DotNetBar.DockSite toolDockSiteLeft;
        private DevComponents.DotNetBar.DockSite toolDockSiteRight;
        private DevComponents.DotNetBar.DockSite toolDockSiteTop;
        private DevComponents.DotNetBar.DockSite toolDockSiteBottom;
        private DevComponents.DotNetBar.Bar tipBar;
        private DevComponents.DotNetBar.DockContainerItem dockContainerItem1;
        private DevComponents.DotNetBar.DockSite dockSiteFill;
        private DevComponents.DotNetBar.PanelDockContainer panelDockContainer1;
        //changed by chulili 20110622
        //private DevComponents.DotNetBar.DockContainerItem dockContainerItem2;
    }
}
