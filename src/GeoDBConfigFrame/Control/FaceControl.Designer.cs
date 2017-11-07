namespace GeoDBConfigFrame
{
    partial class FaceControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FaceControl));
            this.DataIndexTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.gridControl = new System.Windows.Forms.DataGridView();
            this.tipRichBox = new System.Windows.Forms.RichTextBox();
            this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemAddRcd = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemModifyRcd = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDelRcd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemDelAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabControlIndex = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabItemIndex = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlData = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabItemData = new DevComponents.DotNetBar.TabItem(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            this.contextMenuLog.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlIndex)).BeginInit();
            this.tabControlIndex.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlData)).BeginInit();
            this.tabControlData.SuspendLayout();
            this.tabControlPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataIndexTree
            // 
            this.DataIndexTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataIndexTree.ImageIndex = 0;
            this.DataIndexTree.ImageList = this.imageList;
            this.DataIndexTree.Location = new System.Drawing.Point(1, 1);
            this.DataIndexTree.Name = "DataIndexTree";
            this.DataIndexTree.SelectedImageIndex = 2;
            this.DataIndexTree.ShowNodeToolTips = true;
            this.DataIndexTree.Size = new System.Drawing.Size(254, 124);
            this.DataIndexTree.TabIndex = 0;
            this.DataIndexTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.DataIndexTree_NodeMouseClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Web DIsk.png");
            this.imageList.Images.SetKeyName(1, "053753149.gif");
            this.imageList.Images.SetKeyName(2, "053753150.gif");
            // 
            // gridControl
            // 
            this.gridControl.AllowUserToAddRows = false;
            this.gridControl.AllowUserToDeleteRows = false;
            this.gridControl.AllowUserToOrderColumns = true;
            this.gridControl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridControl.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            this.gridControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridControl.GridColor = System.Drawing.SystemColors.InactiveBorder;
            this.gridControl.Location = new System.Drawing.Point(1, 1);
            this.gridControl.Name = "gridControl";
            this.gridControl.RowTemplate.Height = 23;
            this.gridControl.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridControl.Size = new System.Drawing.Size(254, 124);
            this.gridControl.TabIndex = 0;
            this.gridControl.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridControl_CellMouseClick);
            this.gridControl.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridControl_CellDoubleClick);
            // 
            // tipRichBox
            // 
            this.tipRichBox.Location = new System.Drawing.Point(297, 367);
            this.tipRichBox.Name = "tipRichBox";
            this.tipRichBox.Size = new System.Drawing.Size(100, 96);
            this.tipRichBox.TabIndex = 3;
            this.tipRichBox.Text = "";
            this.tipRichBox.Visible = false;
            // 
            // ToolStripMenuItem
            // 
            this.ToolStripMenuItem.Name = "ToolStripMenuItem";
            this.ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.ToolStripMenuItem.Text = "查看详细日志";
            this.ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // contextMenuLog
            // 
            this.contextMenuLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem});
            this.contextMenuLog.Name = "contextMenuStrip";
            this.contextMenuLog.Size = new System.Drawing.Size(143, 26);
            // 
            // MenuItemAddRcd
            // 
            this.MenuItemAddRcd.Name = "MenuItemAddRcd";
            this.MenuItemAddRcd.Size = new System.Drawing.Size(118, 22);
            this.MenuItemAddRcd.Text = "添加记录";
            this.MenuItemAddRcd.Click += new System.EventHandler(this.MenuItemAddRcd_Click);
            // 
            // MenuItemModifyRcd
            // 
            this.MenuItemModifyRcd.Name = "MenuItemModifyRcd";
            this.MenuItemModifyRcd.Size = new System.Drawing.Size(118, 22);
            this.MenuItemModifyRcd.Text = "修改记录";
            this.MenuItemModifyRcd.Click += new System.EventHandler(this.MenuItemModifyRcd_Click);
            // 
            // MenuItemDelRcd
            // 
            this.MenuItemDelRcd.Name = "MenuItemDelRcd";
            this.MenuItemDelRcd.Size = new System.Drawing.Size(118, 22);
            this.MenuItemDelRcd.Text = "删除记录";
            this.MenuItemDelRcd.Click += new System.EventHandler(this.MenuItemDelRcd_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(115, 6);
            // 
            // MenuItemDelAll
            // 
            this.MenuItemDelAll.Name = "MenuItemDelAll";
            this.MenuItemDelAll.Size = new System.Drawing.Size(118, 22);
            this.MenuItemDelAll.Text = "全部删除";
            this.MenuItemDelAll.Click += new System.EventHandler(this.MenuItemDelAll_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemAddRcd,
            this.MenuItemModifyRcd,
            this.MenuItemDelRcd,
            this.toolStripSeparator1,
            this.MenuItemDelAll});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(119, 98);
            // 
            // tabControlIndex
            // 
            this.tabControlIndex.CanReorderTabs = true;
            this.tabControlIndex.Controls.Add(this.tabControlPanel1);
            this.tabControlIndex.Location = new System.Drawing.Point(21, 62);
            this.tabControlIndex.Name = "tabControlIndex";
            this.tabControlIndex.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControlIndex.SelectedTabIndex = 0;
            this.tabControlIndex.Size = new System.Drawing.Size(256, 152);
            this.tabControlIndex.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Bottom;
            this.tabControlIndex.TabIndex = 7;
            this.tabControlIndex.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControlIndex.Tabs.Add(this.tabItemIndex);
            this.tabControlIndex.Text = "索引信息";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.DataIndexTree);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(256, 126);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlPanel1.Style.GradientAngle = -90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItemIndex;
            // 
            // tabItemIndex
            // 
            this.tabItemIndex.AttachedControl = this.tabControlPanel1;
            this.tabItemIndex.Name = "tabItemIndex";
            this.tabItemIndex.Text = "数据字典索引";
            // 
            // tabControlData
            // 
            this.tabControlData.CanReorderTabs = true;
            this.tabControlData.Controls.Add(this.tabControlPanel2);
            this.tabControlData.Location = new System.Drawing.Point(297, 72);
            this.tabControlData.Name = "tabControlData";
            this.tabControlData.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControlData.SelectedTabIndex = 0;
            this.tabControlData.Size = new System.Drawing.Size(256, 152);
            this.tabControlData.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Bottom;
            this.tabControlData.TabIndex = 8;
            this.tabControlData.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControlData.Tabs.Add(this.tabItemData);
            this.tabControlData.Text = "数据信息";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this.gridControl);
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(256, 126);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Top)));
            this.tabControlPanel2.Style.GradientAngle = -90;
            this.tabControlPanel2.TabIndex = 1;
            this.tabControlPanel2.TabItem = this.tabItemData;
            // 
            // tabItemData
            // 
            this.tabItemData.AttachedControl = this.tabControlPanel2;
            this.tabItemData.Name = "tabItemData";
            this.tabItemData.Text = "数据详细信息";
            // 
            // FaceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tipRichBox);
            this.Controls.Add(this.tabControlIndex);
            this.Controls.Add(this.tabControlData);
            this.Name = "FaceControl";
            this.Size = new System.Drawing.Size(592, 476);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            this.contextMenuLog.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlIndex)).EndInit();
            this.tabControlIndex.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlData)).EndInit();
            this.tabControlData.ResumeLayout(false);
            this.tabControlPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox tipRichBox;
        private System.Windows.Forms.DataGridView gridControl;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuLog;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAddRcd;
        private System.Windows.Forms.ToolStripMenuItem MenuItemModifyRcd;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDelRcd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDelAll;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private DevComponents.DotNetBar.TabControl tabControlIndex;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabItemIndex;
        private DevComponents.DotNetBar.TabControl tabControlData;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.TabItem tabItemData;
        public System.Windows.Forms.TreeView DataIndexTree;
    }
}