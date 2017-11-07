namespace GeoDBConfigFrame
{
    partial class SubIndexScript
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubIndexScript));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listViewControl = new System.Windows.Forms.ListView();
            this.专题类型 = new System.Windows.Forms.ColumnHeader();
            this.专题描述 = new System.Windows.Forms.ColumnHeader();
            this.脚本文件 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuSub = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemAddSub = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemModifySub = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDelSub = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.treeViewControl = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.butnCancel = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.contextMenuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemTreeAddGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAddLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemEditLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemMainLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemCanceMainLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemTreeDelLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.contextMenuSub.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listViewControl);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 462);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "专题列表";
            // 
            // listViewControl
            // 
            this.listViewControl.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewControl.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.专题类型,
            this.专题描述,
            this.脚本文件});
            this.listViewControl.ContextMenuStrip = this.contextMenuSub;
            this.listViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewControl.FullRowSelect = true;
            this.listViewControl.GridLines = true;
            this.listViewControl.HotTracking = true;
            this.listViewControl.HoverSelection = true;
            this.listViewControl.Location = new System.Drawing.Point(3, 17);
            this.listViewControl.Name = "listViewControl";
            this.listViewControl.Size = new System.Drawing.Size(365, 442);
            this.listViewControl.TabIndex = 0;
            this.listViewControl.UseCompatibleStateImageBehavior = false;
            this.listViewControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewControl_MouseClick);
            // 
            // 专题类型
            // 
            this.专题类型.Text = "专题类型";
            this.专题类型.Width = 70;
            // 
            // 专题描述
            // 
            this.专题描述.Text = "专题描述";
            this.专题描述.Width = 150;
            // 
            // 脚本文件
            // 
            this.脚本文件.Text = "脚本文件";
            this.脚本文件.Width = 80;
            // 
            // contextMenuSub
            // 
            this.contextMenuSub.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemAddSub,
            this.MenuItemModifySub,
            this.MenuItemDelSub});
            this.contextMenuSub.Name = "contextMenuSub";
            this.contextMenuSub.Size = new System.Drawing.Size(119, 70);
            // 
            // MenuItemAddSub
            // 
            this.MenuItemAddSub.Name = "MenuItemAddSub";
            this.MenuItemAddSub.Size = new System.Drawing.Size(118, 22);
            this.MenuItemAddSub.Text = "添加专题";
            this.MenuItemAddSub.Click += new System.EventHandler(this.MenuItemAddSub_Click);
            // 
            // MenuItemModifySub
            // 
            this.MenuItemModifySub.Name = "MenuItemModifySub";
            this.MenuItemModifySub.Size = new System.Drawing.Size(118, 22);
            this.MenuItemModifySub.Text = "修改专题";
            this.MenuItemModifySub.Click += new System.EventHandler(this.MenuItemModifySub_Click);
            // 
            // MenuItemDelSub
            // 
            this.MenuItemDelSub.Name = "MenuItemDelSub";
            this.MenuItemDelSub.Size = new System.Drawing.Size(118, 22);
            this.MenuItemDelSub.Text = "删除专题";
            this.MenuItemDelSub.Click += new System.EventHandler(this.MenuItemDelSub_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.treeViewControl);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 462);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "专题脚本";
            // 
            // treeViewControl
            // 
            this.treeViewControl.AllowDrop = true;
            this.treeViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewControl.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeViewControl.ImageIndex = 3;
            this.treeViewControl.ImageList = this.imageList;
            this.treeViewControl.Location = new System.Drawing.Point(3, 17);
            this.treeViewControl.Name = "treeViewControl";
            this.treeViewControl.SelectedImageIndex = 0;
            this.treeViewControl.Size = new System.Drawing.Size(238, 442);
            this.treeViewControl.TabIndex = 0;
            this.treeViewControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewControl_DragDrop);
            this.treeViewControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewControl_DragEnter);
            this.treeViewControl.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewControl_NodeMouseClick);
            this.treeViewControl.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewControl_ItemDrag);
            this.treeViewControl.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewControl_DragOver);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            // 
            // butnCancel
            // 
            this.butnCancel.Location = new System.Drawing.Point(523, 475);
            this.butnCancel.Name = "butnCancel";
            this.butnCancel.Size = new System.Drawing.Size(75, 23);
            this.butnCancel.TabIndex = 3;
            this.butnCancel.Text = "退出";
            this.butnCancel.UseVisualStyleBackColor = true;
            this.butnCancel.Click += new System.EventHandler(this.butnCancel_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(2, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(619, 462);
            this.splitContainer1.SplitterDistance = 371;
            this.splitContainer1.TabIndex = 4;
            // 
            // contextMenuTree
            // 
            this.contextMenuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemTreeAddGroup,
            this.MenuItemAddLayer,
            this.MenuItemEditLayer,
            this.toolStripSeparator2,
            this.MenuItemMainLayer,
            this.MenuItemCanceMainLayer,
            this.toolStripSeparator1,
            this.MenuItemTreeDelLayer});
            this.contextMenuTree.Name = "contextMenuTree";
            this.contextMenuTree.Size = new System.Drawing.Size(143, 148);
            // 
            // MenuItemTreeAddGroup
            // 
            this.MenuItemTreeAddGroup.Name = "MenuItemTreeAddGroup";
            this.MenuItemTreeAddGroup.Size = new System.Drawing.Size(142, 22);
            this.MenuItemTreeAddGroup.Text = "添加组";
            this.MenuItemTreeAddGroup.Click += new System.EventHandler(this.MenuItemTreeAddGroup_Click);
            // 
            // MenuItemAddLayer
            // 
            this.MenuItemAddLayer.Name = "MenuItemAddLayer";
            this.MenuItemAddLayer.Size = new System.Drawing.Size(142, 22);
            this.MenuItemAddLayer.Text = "添加图层";
            this.MenuItemAddLayer.Click += new System.EventHandler(this.MenuItemAddLayer_Click);
            // 
            // MenuItemEditLayer
            // 
            this.MenuItemEditLayer.Name = "MenuItemEditLayer";
            this.MenuItemEditLayer.Size = new System.Drawing.Size(142, 22);
            this.MenuItemEditLayer.Text = "编辑图层";
            this.MenuItemEditLayer.Click += new System.EventHandler(this.MenuItemEditLayer_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(139, 6);
            // 
            // MenuItemMainLayer
            // 
            this.MenuItemMainLayer.Name = "MenuItemMainLayer";
            this.MenuItemMainLayer.Size = new System.Drawing.Size(142, 22);
            this.MenuItemMainLayer.Text = "设为关键图层";
            this.MenuItemMainLayer.Click += new System.EventHandler(this.MenuItemMainLayer_Click);
            // 
            // MenuItemCanceMainLayer
            // 
            this.MenuItemCanceMainLayer.Name = "MenuItemCanceMainLayer";
            this.MenuItemCanceMainLayer.Size = new System.Drawing.Size(142, 22);
            this.MenuItemCanceMainLayer.Text = "取消关键图层";
            this.MenuItemCanceMainLayer.Click += new System.EventHandler(this.MenuItemCanceMainLayer_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // MenuItemTreeDelLayer
            // 
            this.MenuItemTreeDelLayer.Name = "MenuItemTreeDelLayer";
            this.MenuItemTreeDelLayer.Size = new System.Drawing.Size(142, 22);
            this.MenuItemTreeDelLayer.Text = "删除节点";
            this.MenuItemTreeDelLayer.Click += new System.EventHandler(this.MenuItemTreeDelLayer_Click);
            // 
            // SubIndexScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 502);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.butnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubIndexScript";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据专题脚本";
            this.groupBox1.ResumeLayout(false);
            this.contextMenuSub.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView treeViewControl;
        private System.Windows.Forms.Button butnCancel;
        private System.Windows.Forms.ListView listViewControl;
        private System.Windows.Forms.ColumnHeader 专题类型;
        private System.Windows.Forms.ColumnHeader 专题描述;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ColumnHeader 脚本文件;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuSub;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAddSub;
        private System.Windows.Forms.ToolStripMenuItem MenuItemModifySub;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDelSub;
        private System.Windows.Forms.ContextMenuStrip contextMenuTree;
        private System.Windows.Forms.ToolStripMenuItem MenuItemTreeAddGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAddLayer;
        private System.Windows.Forms.ToolStripMenuItem MenuItemTreeDelLayer;
        private System.Windows.Forms.ToolStripMenuItem MenuItemEditLayer;
        private System.Windows.Forms.ToolStripMenuItem MenuItemMainLayer;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCanceMainLayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}