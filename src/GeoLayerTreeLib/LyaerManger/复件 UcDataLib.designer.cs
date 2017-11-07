namespace GeoLayerTreeLib.LayerManager
{
    partial class UcDataLib
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcDataLib));
            this.TreeDataLib = new DevComponents.AdvTree.AdvTree();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem3 = new DevComponents.DotNetBar.ButtonItem();
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuLayerTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemSetDbsource = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSetRender = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAddGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAddLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemAddDataset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemModify = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemZoomToLayer = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.TreeDataLib)).BeginInit();
            this.TreeDataLib.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.contextMenuLayerTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeDataLib
            // 
            this.TreeDataLib.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.TreeDataLib.AllowDrop = true;
            this.TreeDataLib.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.TreeDataLib.BackgroundStyle.Class = "TreeBorderKey";
            this.TreeDataLib.Controls.Add(this.contextMenuBar1);
            this.TreeDataLib.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeDataLib.DragDropEnabled = false;
            this.TreeDataLib.DragDropNodeCopyEnabled = false;
            this.TreeDataLib.Location = new System.Drawing.Point(0, 0);
            this.TreeDataLib.Name = "TreeDataLib";
            this.TreeDataLib.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.TreeDataLib.NodesConnector = this.nodeConnector1;
            this.TreeDataLib.NodeStyle = this.elementStyle1;
            this.TreeDataLib.PathSeparator = ";";
            this.TreeDataLib.Size = new System.Drawing.Size(382, 379);
            this.TreeDataLib.Styles.Add(this.elementStyle1);
            this.TreeDataLib.TabIndex = 0;
            this.TreeDataLib.Text = "advTree1";
            this.TreeDataLib.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeDataLib_KeyDown);
            this.TreeDataLib.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TreeDataLib_KeyPress);
            this.TreeDataLib.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.TreeDataLib_NodeClick);
            this.TreeDataLib.AfterCheck += new DevComponents.AdvTree.AdvTreeCellEventHandler(this.TreeDataLib_AfterCheck);
            this.TreeDataLib.NodeMouseDown += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.TreeDataLib_NodeMouseDown);
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1,
            this.buttonItem2,
            this.buttonItem3});
            this.contextMenuBar1.Location = new System.Drawing.Point(133, 270);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(75, 75);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.contextMenuBar1.TabIndex = 0;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // buttonItem1
            // 
            this.buttonItem1.AutoExpandOnClick = true;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "buttonItem1";
            // 
            // buttonItem2
            // 
            this.buttonItem2.AutoExpandOnClick = true;
            this.buttonItem2.Name = "buttonItem2";
            this.buttonItem2.Text = "buttonItem2";
            // 
            // buttonItem3
            // 
            this.buttonItem3.AutoExpandOnClick = true;
            this.buttonItem3.Name = "buttonItem3";
            this.buttonItem3.Text = "buttonItem3";
            // 
            // node1
            // 
            this.node1.Name = "node1";
            this.node1.Text = "node1";
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
            // contextMenuLayerTree
            // 
            this.contextMenuLayerTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemSetDbsource,
            this.MenuItemSetRender,
            this.toolStripSeparator3,
            this.MenuItemAddFolder,
            this.MenuItemAddGroup,
            this.MenuItemAddLayer,
            this.toolStripSeparator4,
            this.MenuItemAddDataset,
            this.toolStripSeparator1,
            this.MenuItemModify,
            this.MenuDelete,
            this.toolStripSeparator2,
            this.MenuItemZoomToLayer});
            this.contextMenuLayerTree.Name = "contextMenuLayerTree";
            this.contextMenuLayerTree.Size = new System.Drawing.Size(149, 226);
            // 
            // MenuItemSetDbsource
            // 
            this.MenuItemSetDbsource.Name = "MenuItemSetDbsource";
            this.MenuItemSetDbsource.Size = new System.Drawing.Size(148, 22);
            this.MenuItemSetDbsource.Text = "设置数据源";
            this.MenuItemSetDbsource.Click += new System.EventHandler(this.MenuItemSetDbsource_Click);
            // 
            // MenuItemSetRender
            // 
            this.MenuItemSetRender.Name = "MenuItemSetRender";
            this.MenuItemSetRender.Size = new System.Drawing.Size(148, 22);
            this.MenuItemSetRender.Text = "自动匹配符号";
            this.MenuItemSetRender.Click += new System.EventHandler(this.MenuItemSetRender_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(145, 6);
            // 
            // MenuItemAddFolder
            // 
            this.MenuItemAddFolder.Name = "MenuItemAddFolder";
            this.MenuItemAddFolder.Size = new System.Drawing.Size(148, 22);
            this.MenuItemAddFolder.Text = "添加专题";
            this.MenuItemAddFolder.Click += new System.EventHandler(this.MenuItemAddFolder_Click);
            // 
            // MenuItemAddGroup
            // 
            this.MenuItemAddGroup.Name = "MenuItemAddGroup";
            this.MenuItemAddGroup.Size = new System.Drawing.Size(148, 22);
            this.MenuItemAddGroup.Text = "添加分组";
            this.MenuItemAddGroup.Click += new System.EventHandler(this.MenuItemAddGroup_Click);
            // 
            // MenuItemAddLayer
            // 
            this.MenuItemAddLayer.Name = "MenuItemAddLayer";
            this.MenuItemAddLayer.Size = new System.Drawing.Size(148, 22);
            this.MenuItemAddLayer.Text = "添加图层";
            this.MenuItemAddLayer.Click += new System.EventHandler(this.MenuItemAddLayer_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(145, 6);
            // 
            // MenuItemAddDataset
            // 
            this.MenuItemAddDataset.Name = "MenuItemAddDataset";
            this.MenuItemAddDataset.Size = new System.Drawing.Size(148, 22);
            this.MenuItemAddDataset.Text = "添加数据集";
            this.MenuItemAddDataset.Click += new System.EventHandler(this.MenuItemAddDataset_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // MenuItemModify
            // 
            this.MenuItemModify.Name = "MenuItemModify";
            this.MenuItemModify.Size = new System.Drawing.Size(148, 22);
            this.MenuItemModify.Text = "修改节点";
            this.MenuItemModify.Click += new System.EventHandler(this.MenuItemModify_Click);
            // 
            // MenuDelete
            // 
            this.MenuDelete.Name = "MenuDelete";
            this.MenuDelete.Size = new System.Drawing.Size(148, 22);
            this.MenuDelete.Text = "删除节点";
            this.MenuDelete.Click += new System.EventHandler(this.MenuDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // MenuItemZoomToLayer
            // 
            this.MenuItemZoomToLayer.Name = "MenuItemZoomToLayer";
            this.MenuItemZoomToLayer.Size = new System.Drawing.Size(148, 22);
            this.MenuItemZoomToLayer.Text = "缩放到图层";
            this.MenuItemZoomToLayer.Click += new System.EventHandler(this.MenuItemZoomToLayer_Click);
            // 
            // UcDataLib
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TreeDataLib);
            this.Name = "UcDataLib";
            this.Size = new System.Drawing.Size(382, 379);
            this.Load += new System.EventHandler(this.UcDataLib_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TreeDataLib)).EndInit();
            this.TreeDataLib.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.contextMenuLayerTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.AdvTree.AdvTree TreeDataLib;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        public System.Windows.Forms.ImageList ImageList;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.ButtonItem buttonItem3;
        private System.Windows.Forms.ContextMenuStrip contextMenuLayerTree;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAddFolder;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAddDataset;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAddLayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemModify;
        private System.Windows.Forms.ToolStripMenuItem MenuDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem MenuItemZoomToLayer;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSetDbsource;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSetRender;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAddGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}
