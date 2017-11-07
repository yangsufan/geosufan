namespace GeoUserManager
{
    partial class UCRole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCRole));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.roleTree = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.tabControlRole = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.priTree = new DevComponents.AdvTree.AdvTree();
            this.columnHeader1 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader2 = new DevComponents.AdvTree.ColumnHeader();
            this.MenupriTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuUnselectall = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeConnector5 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle5 = new DevComponents.DotNetBar.ElementStyle();
            this.tabMenu = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel3 = new DevComponents.DotNetBar.TabControlPanel();
            this.userLstTree = new DevComponents.AdvTree.AdvTree();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.nodeConnector6 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle6 = new DevComponents.DotNetBar.ElementStyle();
            this.tabUser = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.DataTree = new DevComponents.AdvTree.AdvTree();
            this.columnHeader3 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader4 = new DevComponents.AdvTree.ColumnHeader();
            this.MenudataTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenudataSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDataunSelectall = new System.Windows.Forms.ToolStripMenuItem();
            this.tabData = new DevComponents.DotNetBar.TabItem(this.components);
            this.dbSourceTree = new DevComponents.AdvTree.AdvTree();
            this.columnHeader5 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader6 = new DevComponents.AdvTree.ColumnHeader();
            this.nodeConnector7 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle7 = new DevComponents.DotNetBar.ElementStyle();
            this.IconContainer = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.userTree = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector2 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lstUserPrivilege = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.btnPrivilegesRemove = new DevComponents.DotNetBar.ButtonX();
            this.btnPrivilegesAdd = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lstAllPrivilege = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.colCaption = new DevComponents.AdvTree.ColumnHeader();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.roleTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlRole)).BeginInit();
            this.tabControlRole.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.priTree)).BeginInit();
            this.MenupriTree.SuspendLayout();
            this.tabControlPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userLstTree)).BeginInit();
            this.tabControlPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataTree)).BeginInit();
            this.MenudataTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbSourceTree)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userTree)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.roleTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlRole);
            this.splitContainer1.Size = new System.Drawing.Size(729, 419);
            this.splitContainer1.SplitterDistance = 243;
            this.splitContainer1.TabIndex = 0;
            // 
            // roleTree
            // 
            this.roleTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.roleTree.AllowDrop = true;
            this.roleTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.roleTree.BackgroundStyle.Class = "TreeBorderKey";
            this.roleTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roleTree.DragDropEnabled = false;
            this.roleTree.Location = new System.Drawing.Point(0, 0);
            this.roleTree.Name = "roleTree";
            this.roleTree.NodesConnector = this.nodeConnector1;
            this.roleTree.NodeStyle = this.elementStyle1;
            this.roleTree.PathSeparator = ";";
            this.roleTree.Size = new System.Drawing.Size(243, 419);
            this.roleTree.Styles.Add(this.elementStyle1);
            this.roleTree.TabIndex = 1;
            this.roleTree.Text = "advTree1";
            this.roleTree.AfterNodeSelect += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.roleTree_AfterNodeSelect);
            this.roleTree.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.roleTree_NodeClick);
            this.roleTree.NodeMouseDown += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.roleTree_NodeMouseDown);
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
            // tabControlRole
            // 
            this.tabControlRole.BackColor = System.Drawing.Color.Transparent;
            this.tabControlRole.CanReorderTabs = true;
            this.tabControlRole.Controls.Add(this.tabControlPanel1);
            this.tabControlRole.Controls.Add(this.tabControlPanel3);
            this.tabControlRole.Controls.Add(this.tabControlPanel2);
            this.tabControlRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlRole.Location = new System.Drawing.Point(0, 0);
            this.tabControlRole.Name = "tabControlRole";
            this.tabControlRole.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControlRole.SelectedTabIndex = 1;
            this.tabControlRole.Size = new System.Drawing.Size(482, 419);
            this.tabControlRole.TabIndex = 8;
            this.tabControlRole.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControlRole.Tabs.Add(this.tabMenu);
            this.tabControlRole.Tabs.Add(this.tabData);
            this.tabControlRole.Tabs.Add(this.tabUser);
            this.tabControlRole.Text = "tabControl1";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.priTree);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(482, 393);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabMenu;
            // 
            // priTree
            // 
            this.priTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.priTree.AllowDrop = true;
            this.priTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.priTree.BackgroundStyle.Class = "TreeBorderKey";
            this.priTree.Columns.Add(this.columnHeader1);
            this.priTree.Columns.Add(this.columnHeader2);
            this.priTree.ContextMenuStrip = this.MenupriTree;
            this.priTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.priTree.DragDropEnabled = false;
            this.priTree.Location = new System.Drawing.Point(1, 1);
            this.priTree.Name = "priTree";
            this.priTree.NodesConnector = this.nodeConnector5;
            this.priTree.NodeStyle = this.elementStyle5;
            this.priTree.PathSeparator = ";";
            this.priTree.Size = new System.Drawing.Size(480, 391);
            this.priTree.Styles.Add(this.elementStyle5);
            this.priTree.TabIndex = 1;
            this.priTree.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.priTree_NodeClick);
            this.priTree.AfterCheck += new DevComponents.AdvTree.AdvTreeCellEventHandler(this.priTree_AfterCheck);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Name = "columnHeader1";
            this.columnHeader1.Text = "系统功能";
            this.columnHeader1.Width.Absolute = 300;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Name = "columnHeader2";
            this.columnHeader2.Text = "权限";
            this.columnHeader2.Width.Absolute = 200;
            // 
            // MenupriTree
            // 
            this.MenupriTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuSelectAll,
            this.MenuUnselectall});
            this.MenupriTree.Name = "MenupriTree";
            this.MenupriTree.Size = new System.Drawing.Size(113, 48);
            // 
            // MenuSelectAll
            // 
            this.MenuSelectAll.Name = "MenuSelectAll";
            this.MenuSelectAll.Size = new System.Drawing.Size(112, 22);
            this.MenuSelectAll.Text = "全选";
            this.MenuSelectAll.Click += new System.EventHandler(this.MenuSelectAll_Click);
            // 
            // MenuUnselectall
            // 
            this.MenuUnselectall.Name = "MenuUnselectall";
            this.MenuUnselectall.Size = new System.Drawing.Size(112, 22);
            this.MenuUnselectall.Text = "全不选";
            this.MenuUnselectall.Click += new System.EventHandler(this.MenuUnselectall_Click);
            // 
            // nodeConnector5
            // 
            this.nodeConnector5.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle5
            // 
            this.elementStyle5.Name = "elementStyle5";
            this.elementStyle5.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // tabMenu
            // 
            this.tabMenu.AttachedControl = this.tabControlPanel1;
            this.tabMenu.Name = "tabMenu";
            this.tabMenu.Text = "功能权限";
            // 
            // tabControlPanel3
            // 
            this.tabControlPanel3.Controls.Add(this.userLstTree);
            this.tabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel3.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel3.Name = "tabControlPanel3";
            this.tabControlPanel3.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel3.Size = new System.Drawing.Size(482, 393);
            this.tabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel3.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel3.Style.GradientAngle = 90;
            this.tabControlPanel3.TabIndex = 3;
            this.tabControlPanel3.TabItem = this.tabUser;
            // 
            // userLstTree
            // 
            this.userLstTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.userLstTree.AllowDrop = true;
            this.userLstTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.userLstTree.BackgroundStyle.Class = "TreeBorderKey";
            this.userLstTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userLstTree.DragDropEnabled = false;
            this.userLstTree.ImageList = this.ImageList;
            this.userLstTree.Location = new System.Drawing.Point(1, 1);
            this.userLstTree.Name = "userLstTree";
            this.userLstTree.NodesConnector = this.nodeConnector6;
            this.userLstTree.NodeStyle = this.elementStyle6;
            this.userLstTree.PathSeparator = ";";
            this.userLstTree.Size = new System.Drawing.Size(480, 391);
            this.userLstTree.TabIndex = 2;
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
            // nodeConnector6
            // 
            this.nodeConnector6.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle6
            // 
            this.elementStyle6.Name = "elementStyle6";
            this.elementStyle6.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // tabUser
            // 
            this.tabUser.AttachedControl = this.tabControlPanel3;
            this.tabUser.Name = "tabUser";
            this.tabUser.Text = "用户列表";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this.DataTree);
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(482, 393);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.GradientAngle = 90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabData;
            // 
            // DataTree
            // 
            this.DataTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.DataTree.AllowDrop = true;
            this.DataTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.DataTree.BackgroundStyle.Class = "TreeBorderKey";
            this.DataTree.Columns.Add(this.columnHeader3);
            this.DataTree.Columns.Add(this.columnHeader4);
            this.DataTree.ContextMenuStrip = this.MenudataTree;
            this.DataTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataTree.DragDropEnabled = false;
            this.DataTree.ImageList = this.ImageList;
            this.DataTree.Location = new System.Drawing.Point(1, 1);
            this.DataTree.Name = "DataTree";
            this.DataTree.NodesConnector = this.nodeConnector6;
            this.DataTree.NodeStyle = this.elementStyle6;
            this.DataTree.PathSeparator = ";";
            this.DataTree.Size = new System.Drawing.Size(480, 391);
            this.DataTree.Styles.Add(this.elementStyle6);
            this.DataTree.TabIndex = 2;
            this.DataTree.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.DataTree_NodeClick);
            this.DataTree.AfterCheck += new DevComponents.AdvTree.AdvTreeCellEventHandler(this.DataTree_AfterCheck);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Name = "columnHeader3";
            this.columnHeader3.Text = "系统数据";
            this.columnHeader3.Width.Absolute = 300;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Name = "columnHeader4";
            this.columnHeader4.Text = "权限";
            this.columnHeader4.Width.Absolute = 200;
            // 
            // MenudataTree
            // 
            this.MenudataTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenudataSelectAll,
            this.MenuDataunSelectall});
            this.MenudataTree.Name = "MenudataTree";
            this.MenudataTree.Size = new System.Drawing.Size(113, 48);
            // 
            // MenudataSelectAll
            // 
            this.MenudataSelectAll.Name = "MenudataSelectAll";
            this.MenudataSelectAll.Size = new System.Drawing.Size(112, 22);
            this.MenudataSelectAll.Text = "全选";
            this.MenudataSelectAll.Click += new System.EventHandler(this.MenudataSelectAll_Click);
            // 
            // MenuDataunSelectall
            // 
            this.MenuDataunSelectall.Name = "MenuDataunSelectall";
            this.MenuDataunSelectall.Size = new System.Drawing.Size(112, 22);
            this.MenuDataunSelectall.Text = "全不选";
            this.MenuDataunSelectall.Click += new System.EventHandler(this.MenuDataunSelectall_Click);
            // 
            // tabData
            // 
            this.tabData.AttachedControl = this.tabControlPanel2;
            this.tabData.Name = "tabData";
            this.tabData.Text = "数据权限";
            // 
            // dbSourceTree
            // 
            this.dbSourceTree.AllowDrop = true;
            this.dbSourceTree.Location = new System.Drawing.Point(0, 0);
            this.dbSourceTree.Name = "dbSourceTree";
            this.dbSourceTree.PathSeparator = ";";
            this.dbSourceTree.Size = new System.Drawing.Size(0, 0);
            this.dbSourceTree.TabIndex = 0;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Name = "columnHeader5";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Name = "columnHeader6";
            // 
            // nodeConnector7
            // 
            this.nodeConnector7.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle7
            // 
            this.elementStyle7.Name = "elementStyle7";
            this.elementStyle7.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // IconContainer
            // 
            this.IconContainer.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IconContainer.ImageStream")));
            this.IconContainer.TransparentColor = System.Drawing.Color.Transparent;
            this.IconContainer.Images.SetKeyName(0, "");
            this.IconContainer.Images.SetKeyName(1, "");
            this.IconContainer.Images.SetKeyName(2, "");
            this.IconContainer.Images.SetKeyName(3, "");
            this.IconContainer.Images.SetKeyName(4, "");
            this.IconContainer.Images.SetKeyName(5, "");
            this.IconContainer.Images.SetKeyName(6, "");
            this.IconContainer.Images.SetKeyName(7, "");
            this.IconContainer.Images.SetKeyName(8, "");
            this.IconContainer.Images.SetKeyName(9, "");
            this.IconContainer.Images.SetKeyName(10, "");
            this.IconContainer.Images.SetKeyName(11, "");
            this.IconContainer.Images.SetKeyName(12, "trash.ico");
            this.IconContainer.Images.SetKeyName(13, "flag.ico");
            this.IconContainer.Images.SetKeyName(14, "pictures.ico");
            this.IconContainer.Images.SetKeyName(15, "refresh.ico");
            this.IconContainer.Images.SetKeyName(16, "export.ico");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.userTree);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panelEx1);
            this.splitContainer2.Size = new System.Drawing.Size(729, 419);
            this.splitContainer2.SplitterDistance = 174;
            this.splitContainer2.TabIndex = 1;
            this.splitContainer2.Visible = false;
            // 
            // userTree
            // 
            this.userTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.userTree.AllowDrop = true;
            this.userTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.userTree.BackgroundStyle.Class = "TreeBorderKey";
            this.userTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userTree.DragDropEnabled = false;
            this.userTree.Location = new System.Drawing.Point(0, 0);
            this.userTree.Name = "userTree";
            this.userTree.NodesConnector = this.nodeConnector2;
            this.userTree.NodeStyle = this.elementStyle2;
            this.userTree.PathSeparator = ";";
            this.userTree.Size = new System.Drawing.Size(174, 419);
            this.userTree.Styles.Add(this.elementStyle2);
            this.userTree.TabIndex = 1;
            this.userTree.Text = "advTree1";
            this.userTree.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.userTree_NodeClick);
            this.userTree.NodeMouseDown += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.userTree_NodeMouseDown);
            // 
            // nodeConnector2
            // 
            this.nodeConnector2.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle2
            // 
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Controls.Add(this.btnOk);
            this.panelEx1.Controls.Add(this.groupPanel2);
            this.panelEx1.Controls.Add(this.btnPrivilegesRemove);
            this.panelEx1.Controls.Add(this.btnPrivilegesAdd);
            this.panelEx1.Controls.Add(this.groupPanel1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(551, 419);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            this.panelEx1.Resize += new System.EventHandler(this.panelEx1_Resize);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(461, 370);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(395, 370);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 23);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupPanel2
            // 
            this.groupPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.lstUserPrivilege);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(321, 30);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(200, 330);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 10;
            this.groupPanel2.Text = "已授予权限";
            // 
            // lstUserPrivilege
            // 
            // 
            // 
            // 
            this.lstUserPrivilege.Border.Class = "ListViewBorder";
            this.lstUserPrivilege.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstUserPrivilege.Location = new System.Drawing.Point(0, 0);
            this.lstUserPrivilege.Name = "lstUserPrivilege";
            this.lstUserPrivilege.Size = new System.Drawing.Size(194, 306);
            this.lstUserPrivilege.TabIndex = 0;
            this.lstUserPrivilege.UseCompatibleStateImageBehavior = false;
            this.lstUserPrivilege.View = System.Windows.Forms.View.List;
            this.lstUserPrivilege.DoubleClick += new System.EventHandler(this.lstUserPrivilege_DoubleClick);
            // 
            // btnPrivilegesRemove
            // 
            this.btnPrivilegesRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrivilegesRemove.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPrivilegesRemove.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrivilegesRemove.Location = new System.Drawing.Point(252, 185);
            this.btnPrivilegesRemove.Name = "btnPrivilegesRemove";
            this.btnPrivilegesRemove.Size = new System.Drawing.Size(37, 23);
            this.btnPrivilegesRemove.TabIndex = 9;
            this.btnPrivilegesRemove.Text = "<<";
            this.btnPrivilegesRemove.Click += new System.EventHandler(this.btnPrivilegesRemove_Click);
            // 
            // btnPrivilegesAdd
            // 
            this.btnPrivilegesAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrivilegesAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPrivilegesAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrivilegesAdd.Location = new System.Drawing.Point(252, 125);
            this.btnPrivilegesAdd.Name = "btnPrivilegesAdd";
            this.btnPrivilegesAdd.Size = new System.Drawing.Size(37, 23);
            this.btnPrivilegesAdd.TabIndex = 8;
            this.btnPrivilegesAdd.Text = ">>";
            this.btnPrivilegesAdd.Click += new System.EventHandler(this.btnPrivilegesAdd_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.lstAllPrivilege);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(26, 30);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(200, 330);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 7;
            this.groupPanel1.Text = "可用权限";
            // 
            // lstAllPrivilege
            // 
            // 
            // 
            // 
            this.lstAllPrivilege.Border.Class = "ListViewBorder";
            this.lstAllPrivilege.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAllPrivilege.Location = new System.Drawing.Point(0, 0);
            this.lstAllPrivilege.Name = "lstAllPrivilege";
            this.lstAllPrivilege.Size = new System.Drawing.Size(194, 306);
            this.lstAllPrivilege.TabIndex = 0;
            this.lstAllPrivilege.UseCompatibleStateImageBehavior = false;
            this.lstAllPrivilege.View = System.Windows.Forms.View.List;
            this.lstAllPrivilege.DoubleClick += new System.EventHandler(this.lstAllPrivilege_DoubleClick);
            // 
            // colCaption
            // 
            this.colCaption.Name = "colCaption";
            this.colCaption.Text = "系统功能";
            this.colCaption.Width.Absolute = 300;
            // 
            // UCRole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.splitContainer1);
            this.Name = "UCRole";
            this.Size = new System.Drawing.Size(729, 419);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.roleTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlRole)).EndInit();
            this.tabControlRole.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.priTree)).EndInit();
            this.MenupriTree.ResumeLayout(false);
            this.tabControlPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.userLstTree)).EndInit();
            this.tabControlPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataTree)).EndInit();
            this.MenudataTree.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dbSourceTree)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.userTree)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.AdvTree.AdvTree roleTree;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.TabControl tabControlRole;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.AdvTree.AdvTree priTree;
        private DevComponents.AdvTree.ColumnHeader columnHeader1;
        private DevComponents.AdvTree.ColumnHeader columnHeader2;
        private DevComponents.AdvTree.NodeConnector nodeConnector5;
        private DevComponents.DotNetBar.ElementStyle elementStyle5;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.AdvTree.AdvTree DataTree;
        private DevComponents.AdvTree.ColumnHeader columnHeader3;
        private DevComponents.AdvTree.ColumnHeader columnHeader4;
        private DevComponents.AdvTree.NodeConnector nodeConnector6;
        private DevComponents.DotNetBar.ElementStyle elementStyle6;
        //private DevComponents.DotNetBar.TabControlPanel tabControlPanel3;
        private DevComponents.AdvTree.AdvTree dbSourceTree;
        private DevComponents.AdvTree.ColumnHeader columnHeader5;
        private DevComponents.AdvTree.ColumnHeader columnHeader6;
        private DevComponents.AdvTree.NodeConnector nodeConnector7;
        private DevComponents.DotNetBar.ElementStyle elementStyle7;
        private System.Windows.Forms.ImageList IconContainer;
        private DevComponents.DotNetBar.TabItem tabMenu;
        private DevComponents.DotNetBar.TabItem tabData;
        //private DevComponents.DotNetBar.TabItem tabDbsource;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DevComponents.AdvTree.AdvTree userTree;
        private DevComponents.AdvTree.NodeConnector nodeConnector2;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.ListViewEx lstUserPrivilege;
        private DevComponents.DotNetBar.ButtonX btnPrivilegesRemove;
        private DevComponents.DotNetBar.ButtonX btnPrivilegesAdd;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.ListViewEx lstAllPrivilege;
        private System.Windows.Forms.ContextMenuStrip MenudataTree;
        private System.Windows.Forms.ToolStripMenuItem MenudataSelectAll;
        private System.Windows.Forms.ToolStripMenuItem MenuDataunSelectall;
        private System.Windows.Forms.ContextMenuStrip MenupriTree;
        private System.Windows.Forms.ToolStripMenuItem MenuSelectAll;
        private System.Windows.Forms.ToolStripMenuItem MenuUnselectall;
        private DevComponents.AdvTree.ColumnHeader colCaption;
        public System.Windows.Forms.ImageList ImageList;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel3;
        private DevComponents.DotNetBar.TabItem tabUser;
        private DevComponents.AdvTree.AdvTree userLstTree;
    }
}
