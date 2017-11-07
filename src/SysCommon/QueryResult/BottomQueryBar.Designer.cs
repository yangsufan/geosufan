namespace SysCommon
{
    partial class BottomQueryBar
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BottomQueryBar));
            this.barTop = new DevComponents.DotNetBar.Bar();
            this.comboLayers = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.controlContainerItem2 = new DevComponents.DotNetBar.ControlContainerItem();
            this.btnQuerSecond = new DevComponents.DotNetBar.ButtonItem();
            this.btnSecondQuery = new DevComponents.DotNetBar.ButtonItem();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.cbStatisticsField = new DevComponents.DotNetBar.ComboBoxItem();
            this.labelItem3 = new DevComponents.DotNetBar.LabelItem();
            this.cbClassifyField = new DevComponents.DotNetBar.ComboBoxItem();
            this.btnStatistics = new DevComponents.DotNetBar.ButtonItem();
            this.btnExportToExcel = new DevComponents.DotNetBar.ButtonItem();
            this.lblQueryCount = new DevComponents.DotNetBar.LabelItem();
            this.barBottom = new DevComponents.DotNetBar.Bar();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.controlContainerItem1 = new DevComponents.DotNetBar.ControlContainerItem();
            this.advTreeLayers = new DevComponents.AdvTree.AdvTree();
            this.node2 = new DevComponents.AdvTree.Node();
            this.nodeConnector2 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barTop)).BeginInit();
            this.barTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayers)).BeginInit();
            this.SuspendLayout();
            // 
            // barTop
            // 
            this.barTop.Controls.Add(this.comboLayers);
            this.barTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barTop.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1,
            this.controlContainerItem2,
            this.btnQuerSecond,
            this.btnSecondQuery,
            this.labelItem2,
            this.cbStatisticsField,
            this.labelItem3,
            this.cbClassifyField,
            this.btnStatistics,
            this.btnExportToExcel,
            this.lblQueryCount});
            this.barTop.Location = new System.Drawing.Point(0, 0);
            this.barTop.Name = "barTop";
            this.barTop.Size = new System.Drawing.Size(1186, 28);
            this.barTop.Stretch = true;
            this.barTop.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barTop.TabIndex = 0;
            this.barTop.TabStop = false;
            this.barTop.Text = "bar1";
            // 
            // comboLayers
            // 
            this.comboLayers.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboLayers.DisplayMember = "Text";
            this.comboLayers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.comboLayers.FormattingEnabled = true;
            this.comboLayers.ItemHeight = 17;
            this.comboLayers.Location = new System.Drawing.Point(47, 3);
            this.comboLayers.Name = "comboLayers";
            this.comboLayers.Size = new System.Drawing.Size(180, 21);
            this.comboLayers.TabIndex = 3;
            this.comboLayers.Click += new System.EventHandler(this.txtLayerName_Click);
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "图层：";
            this.labelItem1.Click += new System.EventHandler(this.labelItem1_Click);
            // 
            // controlContainerItem2
            // 
            this.controlContainerItem2.AllowItemResize = false;
            this.controlContainerItem2.Control = this.comboLayers;
            this.controlContainerItem2.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.controlContainerItem2.Name = "controlContainerItem2";
            // 
            // btnQuerSecond
            // 
            this.btnQuerSecond.Name = "btnQuerSecond";
            this.btnQuerSecond.Text = "二次查询";
            this.btnQuerSecond.Click += new System.EventHandler(this.btnQuerSecond_Click);
            // 
            // btnSecondQuery
            // 
            this.btnSecondQuery.BeginGroup = true;
            this.btnSecondQuery.Name = "btnSecondQuery";
            this.btnSecondQuery.Text = "分类查询";
            this.btnSecondQuery.Visible = false;
            this.btnSecondQuery.Click += new System.EventHandler(this.btnSecondQuery_Click);
            // 
            // labelItem2
            // 
            this.labelItem2.BeginGroup = true;
            this.labelItem2.Name = "labelItem2";
            this.labelItem2.Text = "统计字段：";
            this.labelItem2.Click += new System.EventHandler(this.labelItem2_Click);
            // 
            // cbStatisticsField
            // 
            this.cbStatisticsField.ComboWidth = 200;
            this.cbStatisticsField.DropDownHeight = 106;
            this.cbStatisticsField.ItemHeight = 17;
            this.cbStatisticsField.Name = "cbStatisticsField";
            // 
            // labelItem3
            // 
            this.labelItem3.Name = "labelItem3";
            this.labelItem3.Text = "分类字段：";
            // 
            // cbClassifyField
            // 
            this.cbClassifyField.ComboWidth = 200;
            this.cbClassifyField.DropDownHeight = 106;
            this.cbClassifyField.ItemHeight = 17;
            this.cbClassifyField.Name = "cbClassifyField";
            // 
            // btnStatistics
            // 
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Text = "统计";
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.BeginGroup = true;
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Text = "导出成EXCEL";
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // lblQueryCount
            // 
            this.lblQueryCount.Name = "lblQueryCount";
            // 
            // barBottom
            // 
            this.barBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barBottom.Location = new System.Drawing.Point(0, 316);
            this.barBottom.Name = "barBottom";
            this.barBottom.Size = new System.Drawing.Size(1186, 25);
            this.barBottom.Stretch = true;
            this.barBottom.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barBottom.TabIndex = 1;
            this.barBottom.TabStop = false;
            this.barBottom.Text = "bar2";
            this.barBottom.Click += new System.EventHandler(this.barBottom_Click);
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(0, 28);
            this.dataGridViewX1.MultiSelect = false;
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.ReadOnly = true;
            this.dataGridViewX1.RowHeadersVisible = false;
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX1.Size = new System.Drawing.Size(1186, 288);
            this.dataGridViewX1.TabIndex = 2;
            this.dataGridViewX1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellDoubleClick);
            this.dataGridViewX1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellClick);
            // 
            // controlContainerItem1
            // 
            this.controlContainerItem1.AllowItemResize = true;
            this.controlContainerItem1.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.controlContainerItem1.Name = "controlContainerItem1";
            // 
            // advTreeLayers
            // 
            this.advTreeLayers.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeLayers.AllowDrop = true;
            this.advTreeLayers.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeLayers.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeLayers.Location = new System.Drawing.Point(47, 3);
            this.advTreeLayers.Name = "advTreeLayers";
            this.advTreeLayers.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node2});
            this.advTreeLayers.NodesConnector = this.nodeConnector2;
            this.advTreeLayers.NodeStyle = this.elementStyle2;
            this.advTreeLayers.PathSeparator = ";";
            this.advTreeLayers.Size = new System.Drawing.Size(24, 128);
            this.advTreeLayers.Styles.Add(this.elementStyle2);
            this.advTreeLayers.TabIndex = 35;
            this.advTreeLayers.Text = "advTree1";
            this.advTreeLayers.Visible = false;
            this.advTreeLayers.Leave += new System.EventHandler(this.advTreeLayers_Leave);
            this.advTreeLayers.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeLayers_NodeClick);
            // 
            // node2
            // 
            this.node2.Expanded = true;
            this.node2.Name = "node2";
            this.node2.Text = "node2";
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
            // BottomQueryBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.advTreeLayers);
            this.Controls.Add(this.dataGridViewX1);
            this.Controls.Add(this.barBottom);
            this.Controls.Add(this.barTop);
            this.Name = "BottomQueryBar";
            this.Size = new System.Drawing.Size(1186, 341);
            this.Load += new System.EventHandler(this.BottomQueryBar_Load);
            this.Click += new System.EventHandler(this.BottomQueryBar_Click);
            ((System.ComponentModel.ISupportInitialize)(this.barTop)).EndInit();
            this.barTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Bar barTop;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.Bar barBottom;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.ButtonItem btnSecondQuery;
        private DevComponents.DotNetBar.ButtonItem btnExportToExcel;
        private DevComponents.DotNetBar.ButtonItem btnStatistics;
        private DevComponents.DotNetBar.LabelItem labelItem2;
        private DevComponents.DotNetBar.LabelItem labelItem3;
        private DevComponents.DotNetBar.ComboBoxItem cbStatisticsField;
        private DevComponents.DotNetBar.ComboBoxItem cbClassifyField;
        private DevComponents.DotNetBar.ButtonItem btnQuerSecond;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboLayers;
        private DevComponents.DotNetBar.ControlContainerItem controlContainerItem1;
        private DevComponents.DotNetBar.ControlContainerItem controlContainerItem2;
        private DevComponents.AdvTree.AdvTree advTreeLayers;
        private DevComponents.AdvTree.Node node2;
        private DevComponents.AdvTree.NodeConnector nodeConnector2;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        public System.Windows.Forms.ImageList ImageList;
        private DevComponents.DotNetBar.LabelItem lblQueryCount;
    }
}
