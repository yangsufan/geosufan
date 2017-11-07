namespace GeoStatistics
{
    partial class UcArea
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcArea));
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxExJsFld = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.dblBuffLen = new System.Windows.Forms.TextBox();
            this.sliderBuffer = new DevComponents.DotNetBar.Controls.Slider();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnGetSQL = new DevComponents.DotNetBar.ButtonX();
            this.txtSQL = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnStatistics = new DevComponents.DotNetBar.ButtonX();
            this.comboLayers = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.gridRes = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.comboType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnExport = new DevComponents.DotNetBar.ButtonX();
            this.btnChart = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.comboTypeY = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.advTreeLayers = new DevComponents.AdvTree.AdvTree();
            this.node2 = new DevComponents.AdvTree.Node();
            this.nodeConnector2 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayers)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.labelX6);
            this.groupPanel1.Controls.Add(this.comboBoxExJsFld);
            this.groupPanel1.Controls.Add(this.dblBuffLen);
            this.groupPanel1.Controls.Add(this.sliderBuffer);
            this.groupPanel1.Controls.Add(this.labelX7);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.btnGetSQL);
            this.groupPanel1.Controls.Add(this.txtSQL);
            this.groupPanel1.Controls.Add(this.btnStatistics);
            this.groupPanel1.Controls.Add(this.comboLayers);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(756, 117);
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
            this.groupPanel1.TabIndex = 2;
            this.groupPanel1.Text = "统计设置";
            this.groupPanel1.Click += new System.EventHandler(this.groupPanel1_Click);
            // 
            // labelX6
            // 
            this.labelX6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelX6.Location = new System.Drawing.Point(554, 64);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(69, 23);
            this.labelX6.TabIndex = 15;
            this.labelX6.Text = "计算字段：";
            this.labelX6.Visible = false;
            // 
            // comboBoxExJsFld
            // 
            this.comboBoxExJsFld.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxExJsFld.DisplayMember = "Text";
            this.comboBoxExJsFld.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxExJsFld.FormattingEnabled = true;
            this.comboBoxExJsFld.ItemHeight = 15;
            this.comboBoxExJsFld.Location = new System.Drawing.Point(623, 64);
            this.comboBoxExJsFld.Name = "comboBoxExJsFld";
            this.comboBoxExJsFld.Size = new System.Drawing.Size(96, 21);
            this.comboBoxExJsFld.TabIndex = 14;
            this.comboBoxExJsFld.Visible = false;
            // 
            // dblBuffLen
            // 
            this.dblBuffLen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.dblBuffLen.Location = new System.Drawing.Point(87, 64);
            this.dblBuffLen.Name = "dblBuffLen";
            this.dblBuffLen.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dblBuffLen.Size = new System.Drawing.Size(65, 21);
            this.dblBuffLen.TabIndex = 13;
            this.dblBuffLen.Text = "0";
            this.dblBuffLen.TextChanged += new System.EventHandler(this.dblBuffLen_TextChanged);
            // 
            // sliderBuffer
            // 
            this.sliderBuffer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderBuffer.LabelWidth = 32;
            this.sliderBuffer.Location = new System.Drawing.Point(178, 64);
            this.sliderBuffer.Maximum = 1000;
            this.sliderBuffer.Name = "sliderBuffer";
            this.sliderBuffer.Size = new System.Drawing.Size(363, 23);
            this.sliderBuffer.TabIndex = 10;
            this.sliderBuffer.Text = "缓冲";
            this.sliderBuffer.Value = 0;
            this.sliderBuffer.ValueChanged += new System.EventHandler(this.sliderBuffer_ValueChanged);
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            this.labelX7.Location = new System.Drawing.Point(152, 64);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(29, 23);
            this.labelX7.TabIndex = 8;
            this.labelX7.Text = "米";
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.Location = new System.Drawing.Point(14, 64);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(67, 23);
            this.labelX4.TabIndex = 8;
            this.labelX4.Text = "空间设置：";
            this.labelX4.Click += new System.EventHandler(this.labelX4_Click);
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(15, 37);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(67, 23);
            this.labelX3.TabIndex = 6;
            this.labelX3.Text = "属性条件：";
            this.labelX3.Click += new System.EventHandler(this.labelX3_Click);
            // 
            // btnGetSQL
            // 
            this.btnGetSQL.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGetSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetSQL.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnGetSQL.Location = new System.Drawing.Point(614, 37);
            this.btnGetSQL.Name = "btnGetSQL";
            this.btnGetSQL.Size = new System.Drawing.Size(42, 21);
            this.btnGetSQL.TabIndex = 5;
            this.btnGetSQL.Text = "...";
            this.btnGetSQL.Click += new System.EventHandler(this.btnGetSQL_Click);
            // 
            // txtSQL
            // 
            this.txtSQL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtSQL.Border.Class = "TextBoxBorder";
            this.txtSQL.Location = new System.Drawing.Point(88, 37);
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.ReadOnly = true;
            this.txtSQL.Size = new System.Drawing.Size(525, 21);
            this.txtSQL.TabIndex = 4;
            // 
            // btnStatistics
            // 
            this.btnStatistics.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStatistics.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStatistics.Location = new System.Drawing.Point(662, 11);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(57, 20);
            this.btnStatistics.TabIndex = 2;
            this.btnStatistics.Text = "统计";
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // comboLayers
            // 
            this.comboLayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboLayers.DisplayMember = "Text";
            this.comboLayers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.comboLayers.FormattingEnabled = true;
            this.comboLayers.ItemHeight = 15;
            this.comboLayers.Location = new System.Drawing.Point(88, 10);
            this.comboLayers.Name = "comboLayers";
            this.comboLayers.Size = new System.Drawing.Size(568, 21);
            this.comboLayers.TabIndex = 1;
            this.comboLayers.SelectedIndexChanged += new System.EventHandler(this.comboLayers_SelectedIndexChanged_1);
            this.comboLayers.Click += new System.EventHandler(this.comboLayers_Click);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(15, 8);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(67, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "统计图层：";
            this.labelX1.Click += new System.EventHandler(this.labelX1_Click);
            // 
            // gridRes
            // 
            this.gridRes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridRes.DefaultCellStyle = dataGridViewCellStyle7;
            this.gridRes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.gridRes.Location = new System.Drawing.Point(0, 114);
            this.gridRes.Name = "gridRes";
            this.gridRes.RowTemplate.Height = 23;
            this.gridRes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRes.Size = new System.Drawing.Size(756, 360);
            this.gridRes.TabIndex = 3;
            // 
            // comboType
            // 
            this.comboType.DisplayMember = "Text";
            this.comboType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.FormattingEnabled = true;
            this.comboType.ItemHeight = 15;
            this.comboType.Location = new System.Drawing.Point(492, 480);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(167, 21);
            this.comboType.TabIndex = 4;
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(17, 483);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(99, 18);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "自定义结果数据";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnChart
            // 
            this.btnChart.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnChart.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnChart.Location = new System.Drawing.Point(663, 480);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(87, 21);
            this.btnChart.TabIndex = 6;
            this.btnChart.Text = "统计结果";
            this.btnChart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(450, 481);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(44, 20);
            this.labelX2.TabIndex = 7;
            this.labelX2.Text = "分类：";
            // 
            // comboTypeY
            // 
            this.comboTypeY.DisplayMember = "Text";
            this.comboTypeY.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboTypeY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTypeY.FormattingEnabled = true;
            this.comboTypeY.ItemHeight = 15;
            this.comboTypeY.Location = new System.Drawing.Point(277, 481);
            this.comboTypeY.Name = "comboTypeY";
            this.comboTypeY.Size = new System.Drawing.Size(167, 21);
            this.comboTypeY.TabIndex = 8;
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            this.labelX5.Location = new System.Drawing.Point(230, 482);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(44, 20);
            this.labelX5.TabIndex = 9;
            this.labelX5.Text = "统计：";
            this.labelX5.UseWaitCursor = true;
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
            this.advTreeLayers.Location = new System.Drawing.Point(91, 31);
            this.advTreeLayers.Name = "advTreeLayers";
            this.advTreeLayers.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node2});
            this.advTreeLayers.NodesConnector = this.nodeConnector2;
            this.advTreeLayers.NodeStyle = this.elementStyle2;
            this.advTreeLayers.PathSeparator = ";";
            this.advTreeLayers.Size = new System.Drawing.Size(24, 128);
            this.advTreeLayers.Styles.Add(this.elementStyle2);
            this.advTreeLayers.TabIndex = 36;
            this.advTreeLayers.Text = "advTree1";
            this.advTreeLayers.Visible = false;
            this.advTreeLayers.Leave += new System.EventHandler(this.advTreeLayers_Leave);
            this.advTreeLayers.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeLayers_NodeClick);
            // 
            // node2
            // 
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
            // UcArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.advTreeLayers);
            this.Controls.Add(this.comboTypeY);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.comboType);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.btnChart);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.gridRes);
            this.Name = "UcArea";
            this.Size = new System.Drawing.Size(756, 508);
            this.Load += new System.EventHandler(this.UcArea_Load);
            this.Click += new System.EventHandler(this.UcArea_Click);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX btnStatistics;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboLayers;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.DataGridViewX gridRes;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboType;
        private DevComponents.DotNetBar.ButtonX btnExport;
        private DevComponents.DotNetBar.ButtonX btnChart;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX btnGetSQL;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSQL;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.Slider sliderBuffer;
        private System.Windows.Forms.TextBox dblBuffLen;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboTypeY;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxExJsFld;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.AdvTree.AdvTree advTreeLayers;
        private DevComponents.AdvTree.Node node2;
        private DevComponents.AdvTree.NodeConnector nodeConnector2;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        public System.Windows.Forms.ImageList ImageList;
    }
}
