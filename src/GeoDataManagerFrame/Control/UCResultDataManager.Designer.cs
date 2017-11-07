namespace GeoDataManagerFrame
{
    partial class UCResultDataManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCResultDataManager));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.MapControlLayer = new ESRI.ArcGIS.Controls.AxMapControl();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.buttonItemZoomIn = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemZoonOut = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemFullExtent = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemPan = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemZoonInfixed = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemZoomOutfixed = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemRefresh = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemBack = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemForward = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemMapScan = new DevComponents.DotNetBar.ButtonItem();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.buttonXDel = new DevComponents.DotNetBar.ButtonX();
            this.bar2 = new DevComponents.DotNetBar.Bar();
            this.bttOpen = new DevComponents.DotNetBar.ButtonItem();
            this.bttModify = new DevComponents.DotNetBar.ButtonItem();
            this.bttDelete = new DevComponents.DotNetBar.ButtonItem();
            this.buttonXMODIFY = new DevComponents.DotNetBar.ButtonX();
            this.buttonXOpen = new DevComponents.DotNetBar.ButtonX();
            this.bttQuery = new DevComponents.DotNetBar.ButtonX();
            this.txtKeys = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labQuery = new DevComponents.DotNetBar.LabelX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dataGridVRe = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.CumFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CmnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CumFilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MapControlLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar2)).BeginInit();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVRe)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.MapControlLayer);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(534, 381);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            // 
            // MapControlLayer
            // 
            this.MapControlLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapControlLayer.Location = new System.Drawing.Point(1, 1);
            this.MapControlLayer.Name = "MapControlLayer";
            this.MapControlLayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MapControlLayer.OcxState")));
            this.MapControlLayer.Size = new System.Drawing.Size(532, 379);
            this.MapControlLayer.TabIndex = 0;
            // 
            // bar1
            // 
            this.bar1.AccessibleDescription = "DotNetBar Bar (bar1)";
            this.bar1.AccessibleName = "DotNetBar Bar";
            this.bar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.bar1.DockOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.bar1.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            this.bar1.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(36, 0);
            this.bar1.Stretch = true;
            this.bar1.TabIndex = 0;
            this.bar1.TabStop = false;
            // 
            // buttonItemZoomIn
            // 
            this.buttonItemZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemZoomIn.Image")));
            this.buttonItemZoomIn.Name = "buttonItemZoomIn";
            this.buttonItemZoomIn.Tag = "  ";
            this.buttonItemZoomIn.Text = "1";
            this.buttonItemZoomIn.Tooltip = "放大";
            // 
            // buttonItemZoonOut
            // 
            this.buttonItemZoonOut.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemZoonOut.Image")));
            this.buttonItemZoonOut.Name = "buttonItemZoonOut";
            this.buttonItemZoonOut.Text = "2";
            this.buttonItemZoonOut.Tooltip = "缩小";
            // 
            // buttonItemFullExtent
            // 
            this.buttonItemFullExtent.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemFullExtent.Image")));
            this.buttonItemFullExtent.Name = "buttonItemFullExtent";
            this.buttonItemFullExtent.Text = "3";
            this.buttonItemFullExtent.Tooltip = "全图";
            // 
            // buttonItemPan
            // 
            this.buttonItemPan.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemPan.Image")));
            this.buttonItemPan.Name = "buttonItemPan";
            this.buttonItemPan.Text = "4";
            this.buttonItemPan.Tooltip = "漫游";
            // 
            // buttonItemZoonInfixed
            // 
            this.buttonItemZoonInfixed.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemZoonInfixed.Image")));
            this.buttonItemZoonInfixed.Name = "buttonItemZoonInfixed";
            this.buttonItemZoonInfixed.Text = "5";
            this.buttonItemZoonInfixed.Tooltip = "中心放大";
            // 
            // buttonItemZoomOutfixed
            // 
            this.buttonItemZoomOutfixed.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemZoomOutfixed.Image")));
            this.buttonItemZoomOutfixed.Name = "buttonItemZoomOutfixed";
            this.buttonItemZoomOutfixed.Text = "6";
            this.buttonItemZoomOutfixed.Tooltip = "中心缩小";
            // 
            // buttonItemRefresh
            // 
            this.buttonItemRefresh.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemRefresh.Image")));
            this.buttonItemRefresh.Name = "buttonItemRefresh";
            this.buttonItemRefresh.Text = "7";
            this.buttonItemRefresh.Tooltip = "刷新";
            // 
            // buttonItemBack
            // 
            this.buttonItemBack.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemBack.Image")));
            this.buttonItemBack.Name = "buttonItemBack";
            this.buttonItemBack.Text = "8";
            this.buttonItemBack.Tooltip = "前一视图";
            // 
            // buttonItemForward
            // 
            this.buttonItemForward.Image = ((System.Drawing.Image)(resources.GetObject("buttonItemForward.Image")));
            this.buttonItemForward.Name = "buttonItemForward";
            this.buttonItemForward.Text = "9";
            this.buttonItemForward.Tooltip = "后一视图";
            // 
            // buttonItemMapScan
            // 
            this.buttonItemMapScan.Name = "buttonItemMapScan";
            this.buttonItemMapScan.Text = "浏览";
            this.buttonItemMapScan.Tooltip = "图层浏览";
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "视图浏览";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.buttonXDel);
            this.groupPanel1.Controls.Add(this.bar2);
            this.groupPanel1.Controls.Add(this.buttonXMODIFY);
            this.groupPanel1.Controls.Add(this.buttonXOpen);
            this.groupPanel1.Controls.Add(this.bttQuery);
            this.groupPanel1.Controls.Add(this.txtKeys);
            this.groupPanel1.Controls.Add(this.labQuery);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(613, 49);
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
            this.groupPanel1.TabIndex = 2;
            // 
            // buttonXDel
            // 
            this.buttonXDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXDel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXDel.Location = new System.Drawing.Point(526, 12);
            this.buttonXDel.Name = "buttonXDel";
            this.buttonXDel.Size = new System.Drawing.Size(70, 24);
            this.buttonXDel.TabIndex = 9;
            this.buttonXDel.Text = "删除";
            this.buttonXDel.Click += new System.EventHandler(this.bttDelete_Click);
            // 
            // bar2
            // 
            this.bar2.AccessibleDescription = "DotNetBar Bar (bar2)";
            this.bar2.AccessibleName = "DotNetBar Bar";
            this.bar2.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.bar2.DockOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.bar2.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            this.bar2.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
            this.bar2.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.bttOpen,
            this.bttModify,
            this.bttDelete});
            this.bar2.Location = new System.Drawing.Point(184, 12);
            this.bar2.Name = "bar2";
            this.bar2.Size = new System.Drawing.Size(26, 187);
            this.bar2.Stretch = true;
            this.bar2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.bar2.TabIndex = 3;
            this.bar2.TabStop = false;
            this.bar2.Text = "bar2";
            this.bar2.Visible = false;
            // 
            // bttOpen
            // 
            this.bttOpen.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bttOpen.Name = "bttOpen";
            this.bttOpen.Tag = "  ";
            this.bttOpen.Text = "打开";
            this.bttOpen.Tooltip = "打开";
            this.bttOpen.Click += new System.EventHandler(this.bttOpen_Click);
            // 
            // bttModify
            // 
            this.bttModify.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bttModify.Name = "bttModify";
            this.bttModify.Text = "修改";
            this.bttModify.Tooltip = "修改数据";
            this.bttModify.Click += new System.EventHandler(this.bttModify_Click);
            // 
            // bttDelete
            // 
            this.bttDelete.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bttDelete.Name = "bttDelete";
            this.bttDelete.Text = "删除";
            this.bttDelete.Tooltip = "删除数据";
            this.bttDelete.Click += new System.EventHandler(this.bttDelete_Click);
            // 
            // buttonXMODIFY
            // 
            this.buttonXMODIFY.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXMODIFY.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXMODIFY.Location = new System.Drawing.Point(450, 12);
            this.buttonXMODIFY.Name = "buttonXMODIFY";
            this.buttonXMODIFY.Size = new System.Drawing.Size(70, 24);
            this.buttonXMODIFY.TabIndex = 9;
            this.buttonXMODIFY.Text = "修改";
            this.buttonXMODIFY.Click += new System.EventHandler(this.bttModify_Click);
            // 
            // buttonXOpen
            // 
            this.buttonXOpen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOpen.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXOpen.Location = new System.Drawing.Point(374, 12);
            this.buttonXOpen.Name = "buttonXOpen";
            this.buttonXOpen.Size = new System.Drawing.Size(70, 24);
            this.buttonXOpen.TabIndex = 9;
            this.buttonXOpen.Text = "打开";
            this.buttonXOpen.Click += new System.EventHandler(this.bttOpen_Click);
            // 
            // bttQuery
            // 
            this.bttQuery.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttQuery.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttQuery.Location = new System.Drawing.Point(298, 12);
            this.bttQuery.Name = "bttQuery";
            this.bttQuery.Size = new System.Drawing.Size(70, 24);
            this.bttQuery.TabIndex = 9;
            this.bttQuery.Text = "检索";
            this.bttQuery.Click += new System.EventHandler(this.bttQuery_Click);
            // 
            // txtKeys
            // 
            // 
            // 
            // 
            this.txtKeys.Border.Class = "TextBoxBorder";
            this.txtKeys.Location = new System.Drawing.Point(50, 12);
            this.txtKeys.Name = "txtKeys";
            this.txtKeys.Size = new System.Drawing.Size(242, 21);
            this.txtKeys.TabIndex = 8;
            this.txtKeys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeys_KeyDown);
            // 
            // labQuery
            // 
            this.labQuery.BackColor = System.Drawing.Color.Transparent;
            this.labQuery.Location = new System.Drawing.Point(3, 10);
            this.labQuery.Name = "labQuery";
            this.labQuery.Size = new System.Drawing.Size(57, 23);
            this.labQuery.TabIndex = 7;
            this.labQuery.Text = "关键字：";
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.dataGridVRe);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(0, 49);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(610, 436);
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
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 3;
            this.groupPanel2.Text = "成果展示";
            // 
            // dataGridVRe
            // 
            this.dataGridVRe.AllowUserToAddRows = false;
            this.dataGridVRe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridVRe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CumFileName,
            this.CmnType,
            this.CumFilePath});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridVRe.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridVRe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridVRe.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridVRe.Location = new System.Drawing.Point(0, 0);
            this.dataGridVRe.Name = "dataGridVRe";
            this.dataGridVRe.ReadOnly = true;
            this.dataGridVRe.RowTemplate.Height = 23;
            this.dataGridVRe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridVRe.Size = new System.Drawing.Size(604, 412);
            this.dataGridVRe.TabIndex = 3;
            // 
            // CumFileName
            // 
            this.CumFileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CumFileName.HeaderText = "成果数据名称";
            this.CumFileName.Name = "CumFileName";
            this.CumFileName.ReadOnly = true;
            // 
            // CmnType
            // 
            this.CmnType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CmnType.HeaderText = "成果数据类型";
            this.CmnType.Name = "CmnType";
            this.CmnType.ReadOnly = true;
            // 
            // CumFilePath
            // 
            this.CumFilePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CumFilePath.HeaderText = "存储位置";
            this.CumFilePath.Name = "CumFilePath";
            this.CumFilePath.ReadOnly = true;
            // 
            // UCResultDataManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.groupPanel1);
            this.Name = "UCResultDataManager";
            this.Size = new System.Drawing.Size(613, 485);
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MapControlLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bar2)).EndInit();
            this.groupPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVRe)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private ESRI.ArcGIS.Controls.AxMapControl MapControlLayer;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem buttonItemZoomIn;
        private DevComponents.DotNetBar.ButtonItem buttonItemZoonOut;
        private DevComponents.DotNetBar.ButtonItem buttonItemFullExtent;
        private DevComponents.DotNetBar.ButtonItem buttonItemPan;
        private DevComponents.DotNetBar.ButtonItem buttonItemZoonInfixed;
        private DevComponents.DotNetBar.ButtonItem buttonItemZoomOutfixed;
        private DevComponents.DotNetBar.ButtonItem buttonItemRefresh;
        private DevComponents.DotNetBar.ButtonItem buttonItemBack;
        private DevComponents.DotNetBar.ButtonItem buttonItemForward;
        private DevComponents.DotNetBar.ButtonItem buttonItemMapScan;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridVRe;
        private DevComponents.DotNetBar.Bar bar2;
        private DevComponents.DotNetBar.ButtonItem bttOpen;
        private DevComponents.DotNetBar.ButtonItem bttModify;
        private DevComponents.DotNetBar.ButtonItem bttDelete;
        private DevComponents.DotNetBar.ButtonX bttQuery;
        private DevComponents.DotNetBar.Controls.TextBoxX txtKeys;
        private DevComponents.DotNetBar.LabelX labQuery;
        private DevComponents.DotNetBar.ButtonX buttonXDel;
        private DevComponents.DotNetBar.ButtonX buttonXMODIFY;
        private DevComponents.DotNetBar.ButtonX buttonXOpen;
        private System.Windows.Forms.DataGridViewTextBoxColumn CumFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CmnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CumFilePath;

    }
}
