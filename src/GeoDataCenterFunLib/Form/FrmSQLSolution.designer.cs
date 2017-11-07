namespace GeoDataCenterFunLib
{
    partial class FrmSQLSolution
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSQLSolution));
            this.cmblayersel = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cmboxSqlSolution = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelDescription = new DevComponents.DotNetBar.LabelX();
            this.richTextExpression = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.advTreeLayers = new DevComponents.AdvTree.AdvTree();
            this.node2 = new DevComponents.AdvTree.Node();
            this.nodeConnector2 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayers)).BeginInit();
            this.SuspendLayout();
            // 
            // cmblayersel
            // 
            this.cmblayersel.DisplayMember = "Text";
            this.cmblayersel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmblayersel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cmblayersel.FormattingEnabled = true;
            this.cmblayersel.ItemHeight = 15;
            this.cmblayersel.Location = new System.Drawing.Point(69, 13);
            this.cmblayersel.Name = "cmblayersel";
            this.cmblayersel.Size = new System.Drawing.Size(249, 21);
            this.cmblayersel.TabIndex = 2;
            this.cmblayersel.TextChanged += new System.EventHandler(this.cmblayersel_TextChanged);
            this.cmblayersel.Click += new System.EventHandler(this.cmblayersel_Click);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(10, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "查询图层：";
            this.labelX1.Click += new System.EventHandler(this.labelX1_Click);
            // 
            // cmboxSqlSolution
            // 
            this.cmboxSqlSolution.DisplayMember = "Text";
            this.cmboxSqlSolution.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmboxSqlSolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboxSqlSolution.FormattingEnabled = true;
            this.cmboxSqlSolution.ItemHeight = 15;
            this.cmboxSqlSolution.Location = new System.Drawing.Point(69, 48);
            this.cmboxSqlSolution.Name = "cmboxSqlSolution";
            this.cmboxSqlSolution.Size = new System.Drawing.Size(249, 21);
            this.cmboxSqlSolution.TabIndex = 33;
            this.cmboxSqlSolution.SelectedIndexChanged += new System.EventHandler(this.cmboxType_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(140, 150);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(77, 23);
            this.btnOK.TabIndex = 29;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(240, 150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 23);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(10, 48);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 0;
            this.labelX2.Text = "查询方案：";
            this.labelX2.Click += new System.EventHandler(this.labelX2_Click);
            // 
            // labelDescription
            // 
            this.labelDescription.BackColor = System.Drawing.Color.Transparent;
            this.labelDescription.Location = new System.Drawing.Point(70, 85);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(249, 52);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.TextLineAlignment = System.Drawing.StringAlignment.Near;
            this.labelDescription.WordWrap = true;
            this.labelDescription.Click += new System.EventHandler(this.labelDescription_Click);
            // 
            // richTextExpression
            // 
            this.richTextExpression.DisplayMember = "Text";
            this.richTextExpression.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.richTextExpression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.richTextExpression.FormattingEnabled = true;
            this.richTextExpression.ItemHeight = 15;
            this.richTextExpression.Location = new System.Drawing.Point(68, 147);
            this.richTextExpression.Name = "richTextExpression";
            this.richTextExpression.Size = new System.Drawing.Size(25, 21);
            this.richTextExpression.TabIndex = 2;
            this.richTextExpression.Visible = false;
            this.richTextExpression.Click += new System.EventHandler(this.cmblayersel_Click);
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(10, 84);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "方案说明：";
            this.labelX3.Click += new System.EventHandler(this.labelX3_Click);
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
            this.advTreeLayers.Location = new System.Drawing.Point(69, 13);
            this.advTreeLayers.Name = "advTreeLayers";
            this.advTreeLayers.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node2});
            this.advTreeLayers.NodesConnector = this.nodeConnector2;
            this.advTreeLayers.NodeStyle = this.elementStyle2;
            this.advTreeLayers.PathSeparator = ";";
            this.advTreeLayers.Size = new System.Drawing.Size(24, 128);
            this.advTreeLayers.Styles.Add(this.elementStyle2);
            this.advTreeLayers.TabIndex = 34;
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
            // FrmSQLSolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 179);
            this.Controls.Add(this.advTreeLayers);
            this.Controls.Add(this.richTextExpression);
            this.Controls.Add(this.cmblayersel);
            this.Controls.Add(this.cmboxSqlSolution);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSQLSolution";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询方案";
            this.Load += new System.EventHandler(this.FrmSQLQuery_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSQLQuery_FormClosed);
            this.Click += new System.EventHandler(this.FrmSQLSolution_Click);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSQLQuery_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx cmblayersel;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmboxSqlSolution;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelDescription;
        private DevComponents.DotNetBar.Controls.ComboBoxEx richTextExpression;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.AdvTree.AdvTree advTreeLayers;
        private DevComponents.AdvTree.Node node2;
        private DevComponents.AdvTree.NodeConnector nodeConnector2;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        public System.Windows.Forms.ImageList ImageList;
    }
}