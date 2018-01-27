namespace Fan.Common
{
    partial class BaseRibbonForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainRibbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ItemLoginUser = new DevExpress.XtraBars.BarStaticItem();
            ((System.ComponentModel.ISupportInitialize)(this.mainRibbon)).BeginInit();
            this.SuspendLayout();
            // 
            // mainRibbon
            // 
            this.mainRibbon.ExpandCollapseItem.Id = 0;
            this.mainRibbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.mainRibbon.ExpandCollapseItem,
            this.ItemLoginUser});
            this.mainRibbon.Location = new System.Drawing.Point(0, 0);
            this.mainRibbon.MaxItemId = 2;
            this.mainRibbon.Name = "mainRibbon";
            this.mainRibbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.mainRibbon.Size = new System.Drawing.Size(1281, 143);
            this.mainRibbon.StatusBar = this.ribbonStatusBar;
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.ItemLoginUser);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 710);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.mainRibbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1281, 31);
            // 
            // ItemLoginUser
            // 
            this.ItemLoginUser.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.ItemLoginUser.Caption = "当前登录用户：";
            this.ItemLoginUser.Id = 1;
            this.ItemLoginUser.Name = "ItemLoginUser";
            // 
            // BaseRibbonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 741);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.mainRibbon);
            this.IsMdiContainer = true;
            this.Name = "BaseRibbonForm";
            this.Ribbon = this.mainRibbon;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "BaseRibbonForm";
            ((System.ComponentModel.ISupportInitialize)(this.mainRibbon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraBars.Ribbon.RibbonControl mainRibbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        public DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarStaticItem ItemLoginUser;
    }
}