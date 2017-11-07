namespace GeoDataExport
{
    partial class frmAddData
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddData));
            this.lblFind = new DevComponents.DotNetBar.LabelX();
            this.lblName = new DevComponents.DotNetBar.LabelX();
            this.lblType = new DevComponents.DotNetBar.LabelX();
            this.cboFind = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnUpOn = new DevComponents.DotNetBar.ButtonX();
            this.cboType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.txtName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnAdd = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.treData = new DevComponents.AdvTree.AdvTree();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.lvwData = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.treData)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFind
            // 
            this.lblFind.Location = new System.Drawing.Point(12, 8);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(48, 23);
            this.lblFind.TabIndex = 0;
            this.lblFind.Text = "查找：";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(10, 336);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(50, 23);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "名称：";
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(10, 365);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(50, 23);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "类型：";
            // 
            // cboFind
            // 
            this.cboFind.DisplayMember = "Text";
            this.cboFind.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboFind.FormattingEnabled = true;
            this.cboFind.ItemHeight = 15;
            this.cboFind.Location = new System.Drawing.Point(66, 8);
            this.cboFind.Name = "cboFind";
            this.cboFind.Size = new System.Drawing.Size(344, 21);
            this.cboFind.TabIndex = 3;
            // 
            // btnUpOn
            // 
            this.btnUpOn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUpOn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUpOn.Image = ((System.Drawing.Image)(resources.GetObject("btnUpOn.Image")));
            this.btnUpOn.Location = new System.Drawing.Point(428, 8);
            this.btnUpOn.Name = "btnUpOn";
            this.btnUpOn.Size = new System.Drawing.Size(20, 20);
            this.btnUpOn.TabIndex = 4;
            this.btnUpOn.Click += new System.EventHandler(this.UpOn_Click);
            // 
            // cboType
            // 
            this.cboType.DisplayMember = "Text";
            this.cboType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboType.FormattingEnabled = true;
            this.cboType.ItemHeight = 15;
            this.cboType.Items.AddRange(new object[] {
            this.comboItem1});
            this.cboType.Location = new System.Drawing.Point(66, 365);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(399, 21);
            this.cboType.TabIndex = 6;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "Datasets and Layers(*.lyr)";
            // 
            // txtName
            // 
            // 
            // 
            // 
            this.txtName.Border.Class = "TextBoxBorder";
            this.txtName.Location = new System.Drawing.Point(66, 336);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(399, 21);
            this.txtName.TabIndex = 7;
            // 
            // btnAdd
            // 
            this.btnAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAdd.Location = new System.Drawing.Point(487, 336);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.Add_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(487, 365);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // treData
            // 
            this.treData.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.treData.AllowDrop = true;
            this.treData.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.treData.BackgroundStyle.Class = "TreeBorderKey";
            this.treData.ImageIndex = 5;
            this.treData.ImageList = this.imageList1;
            this.treData.Location = new System.Drawing.Point(1, 35);
            this.treData.Name = "treData";
            this.treData.NodesConnector = this.nodeConnector1;
            this.treData.NodeStyle = this.elementStyle1;
            this.treData.PathSeparator = ";";
            this.treData.Size = new System.Drawing.Size(175, 296);
            this.treData.Styles.Add(this.elementStyle1);
            this.treData.TabIndex = 10;
            this.treData.Text = "advTree1";
            this.treData.AfterNodeSelect += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.advTreeView_AfterNodeSelect);
            this.treData.BeforeExpand += new DevComponents.AdvTree.AdvTreeNodeCancelEventHandler(this.advTreeView_BeforeExpand);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            this.imageList1.Images.SetKeyName(11, "");
            this.imageList1.Images.SetKeyName(12, "");
            this.imageList1.Images.SetKeyName(13, "");
            this.imageList1.Images.SetKeyName(14, "");
            this.imageList1.Images.SetKeyName(15, "sg.bmp");
            this.imageList1.Images.SetKeyName(16, "rb.bmp");
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
            // lvwData
            // 
            // 
            // 
            // 
            this.lvwData.Border.Class = "ListViewBorder";
            this.lvwData.Location = new System.Drawing.Point(179, 35);
            this.lvwData.Name = "lvwData";
            this.lvwData.Size = new System.Drawing.Size(391, 296);
            this.lvwData.SmallImageList = this.imageList2;
            this.lvwData.TabIndex = 11;
            this.lvwData.UseCompatibleStateImageBehavior = false;
            this.lvwData.View = System.Windows.Forms.View.List;
            this.lvwData.DoubleClick += new System.EventHandler(this.ExListView_DoubleClick);
            this.lvwData.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ExListView_ItemSelectionChanged);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "");
            this.imageList2.Images.SetKeyName(1, "");
            this.imageList2.Images.SetKeyName(2, "");
            this.imageList2.Images.SetKeyName(3, "");
            this.imageList2.Images.SetKeyName(4, "");
            this.imageList2.Images.SetKeyName(5, "09965.bmp");
            this.imageList2.Images.SetKeyName(6, "");
            this.imageList2.Images.SetKeyName(7, "");
            this.imageList2.Images.SetKeyName(8, "");
            this.imageList2.Images.SetKeyName(9, "");
            this.imageList2.Images.SetKeyName(10, "");
            this.imageList2.Images.SetKeyName(11, "00218.ico");
            this.imageList2.Images.SetKeyName(12, "MultiPatch.bmp");
            this.imageList2.Images.SetKeyName(13, "sg.bmp");
            this.imageList2.Images.SetKeyName(14, "rb.bmp");
            // 
            // frmAddData
            // 
            this.ClientSize = new System.Drawing.Size(571, 393);
            this.Controls.Add(this.lvwData);
            this.Controls.Add(this.treData);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.btnUpOn);
            this.Controls.Add(this.cboFind);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblFind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddData";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加数据";
            this.Load += new System.EventHandler(this.AddData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.treData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX lblFind;
        private DevComponents.DotNetBar.LabelX lblName;
        private DevComponents.DotNetBar.LabelX lblType;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboFind;
        private DevComponents.DotNetBar.ButtonX btnUpOn;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboType;
        private DevComponents.DotNetBar.Controls.TextBoxX txtName;
        private DevComponents.DotNetBar.ButtonX btnAdd;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.AdvTree.AdvTree treData;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.Controls.ListViewEx lvwData;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;
        private DevComponents.Editors.ComboItem comboItem1;
    }
}