namespace GeoDataManagerFrame
{
    partial class FrmResultDataUploading
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmResultDataUploading));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dataGrid = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.CmnSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CmnPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CmnState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOtherSelected = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnAllSelected = new DevComponents.DotNetBar.ButtonX();
            this.bttAllRemove = new DevComponents.DotNetBar.ButtonX();
            this.bttRemove = new DevComponents.DotNetBar.ButtonX();
            this.bttOpenFolder = new DevComponents.DotNetBar.ButtonX();
            this.bttOpenFile = new DevComponents.DotNetBar.ButtonX();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuDel = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupPanel1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(205, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78.05486F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(456, 466);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.dataGrid);
            this.groupPanel1.Controls.Add(this.btnCancel);
            this.groupPanel1.Controls.Add(this.btnOtherSelected);
            this.groupPanel1.Controls.Add(this.btnOK);
            this.groupPanel1.Controls.Add(this.btnAllSelected);
            this.groupPanel1.Controls.Add(this.bttAllRemove);
            this.groupPanel1.Controls.Add(this.bttRemove);
            this.groupPanel1.Controls.Add(this.bttOpenFolder);
            this.groupPanel1.Controls.Add(this.bttOpenFile);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(3, 3);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(450, 460);
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
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 1;
            this.groupPanel1.TitleImagePosition = DevComponents.DotNetBar.eTitleImagePosition.Center;
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CmnSelect,
            this.CmnPath,
            this.CmnState});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGrid.Location = new System.Drawing.Point(3, 3);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RowTemplate.Height = 23;
            this.dataGrid.Size = new System.Drawing.Size(363, 448);
            this.dataGrid.TabIndex = 30;
            // 
            // CmnSelect
            // 
            this.CmnSelect.HeaderText = "选择";
            this.CmnSelect.Name = "CmnSelect";
            this.CmnSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CmnSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CmnSelect.Width = 60;
            // 
            // CmnPath
            // 
            this.CmnPath.HeaderText = "名称";
            this.CmnPath.Name = "CmnPath";
            this.CmnPath.ReadOnly = true;
            this.CmnPath.Width = 170;
            // 
            // CmnState
            // 
            this.CmnState.HeaderText = "状态";
            this.CmnState.Name = "CmnState";
            this.CmnState.ReadOnly = true;
            this.CmnState.Width = 90;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(372, 306);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOtherSelected
            // 
            this.btnOtherSelected.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOtherSelected.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOtherSelected.Location = new System.Drawing.Point(372, 231);
            this.btnOtherSelected.Name = "btnOtherSelected";
            this.btnOtherSelected.Size = new System.Drawing.Size(65, 23);
            this.btnOtherSelected.TabIndex = 26;
            this.btnOtherSelected.Text = "反选";
            this.btnOtherSelected.Click += new System.EventHandler(this.btnOtherSelected_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(372, 278);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 23);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "上传";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAllSelected
            // 
            this.btnAllSelected.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAllSelected.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAllSelected.Location = new System.Drawing.Point(372, 202);
            this.btnAllSelected.Name = "btnAllSelected";
            this.btnAllSelected.Size = new System.Drawing.Size(65, 23);
            this.btnAllSelected.TabIndex = 25;
            this.btnAllSelected.Text = "全选";
            this.btnAllSelected.Click += new System.EventHandler(this.btnAllSelected_Click);
            // 
            // bttAllRemove
            // 
            this.bttAllRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttAllRemove.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttAllRemove.Location = new System.Drawing.Point(372, 152);
            this.bttAllRemove.Name = "bttAllRemove";
            this.bttAllRemove.Size = new System.Drawing.Size(65, 23);
            this.bttAllRemove.TabIndex = 24;
            this.bttAllRemove.Text = "全部移除";
            this.bttAllRemove.Click += new System.EventHandler(this.bttAllRemove_Click);
            // 
            // bttRemove
            // 
            this.bttRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttRemove.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttRemove.Location = new System.Drawing.Point(372, 123);
            this.bttRemove.Name = "bttRemove";
            this.bttRemove.Size = new System.Drawing.Size(65, 23);
            this.bttRemove.TabIndex = 23;
            this.bttRemove.Text = "移除";
            this.bttRemove.Click += new System.EventHandler(this.bttRemove_Click);
            // 
            // bttOpenFolder
            // 
            this.bttOpenFolder.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttOpenFolder.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttOpenFolder.Location = new System.Drawing.Point(372, 75);
            this.bttOpenFolder.Name = "bttOpenFolder";
            this.bttOpenFolder.Size = new System.Drawing.Size(65, 23);
            this.bttOpenFolder.TabIndex = 22;
            this.bttOpenFolder.Text = "文件夹";
            this.bttOpenFolder.Click += new System.EventHandler(this.bttOpenFolder_Click);
            // 
            // bttOpenFile
            // 
            this.bttOpenFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttOpenFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttOpenFile.Location = new System.Drawing.Point(372, 46);
            this.bttOpenFile.Name = "bttOpenFile";
            this.bttOpenFile.Size = new System.Drawing.Size(65, 23);
            this.bttOpenFile.TabIndex = 21;
            this.bttOpenFile.Text = "文件";
            this.bttOpenFile.Click += new System.EventHandler(this.bttOpenFile_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "CopyFolderHS.png");
            this.imageList1.Images.SetKeyName(1, "folder_link.png");
            this.imageList1.Images.SetKeyName(2, "folder_database.png");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuAdd,
            this.toolStripMenuOpen,
            this.toolStripMenuDel});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(131, 70);
            // 
            // toolStripMenuAdd
            // 
            this.toolStripMenuAdd.Name = "toolStripMenuAdd";
            this.toolStripMenuAdd.Size = new System.Drawing.Size(130, 22);
            this.toolStripMenuAdd.Text = "新增文件夹";
            this.toolStripMenuAdd.Click += new System.EventHandler(this.toolStripMenuAdd_Click);
            // 
            // toolStripMenuOpen
            // 
            this.toolStripMenuOpen.Name = "toolStripMenuOpen";
            this.toolStripMenuOpen.Size = new System.Drawing.Size(130, 22);
            this.toolStripMenuOpen.Text = "打开文件夹";
            this.toolStripMenuOpen.Visible = false;
            this.toolStripMenuOpen.Click += new System.EventHandler(this.toolStripMenuOpen_Click);
            // 
            // toolStripMenuDel
            // 
            this.toolStripMenuDel.Name = "toolStripMenuDel";
            this.toolStripMenuDel.Size = new System.Drawing.Size(130, 22);
            this.toolStripMenuDel.Text = "删除文件夹";
            this.toolStripMenuDel.Click += new System.EventHandler(this.toolStripMenuDel_Click);
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(187, 463);
            this.treeView1.TabIndex = 18;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // FrmResultDataUploading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 490);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmResultDataUploading";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "成果数据入库";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX bttOpenFile;
        private DevComponents.DotNetBar.ButtonX btnOtherSelected;
        private DevComponents.DotNetBar.ButtonX btnAllSelected;
        private DevComponents.DotNetBar.ButtonX bttAllRemove;
        private DevComponents.DotNetBar.ButtonX bttRemove;
        private DevComponents.DotNetBar.ButtonX bttOpenFolder;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CmnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn CmnPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn CmnState;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuAdd;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuOpen;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuDel;
        private System.Windows.Forms.TreeView treeView1;
    }
}