namespace GeoSysSetting
{
    partial class FormQueryConfig
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
            this.listQueryType = new System.Windows.Forms.ListBox();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.buttonOK = new DevComponents.DotNetBar.ButtonX();
            this.buttonCancel = new DevComponents.DotNetBar.ButtonX();
            this.groupPanelDM = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelQueryName = new DevComponents.DotNetBar.LabelX();
            this.comboBoxDMLayer1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboBoxDMLayer2 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelXDMLayer2 = new DevComponents.DotNetBar.LabelX();
            this.labelXDMLayer1 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxDMField = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelXDMField = new DevComponents.DotNetBar.LabelX();
            this.groupPanelRoad = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelXRoad = new DevComponents.DotNetBar.LabelX();
            this.comboBoxRoadLayer = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboBoxRoadCode = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelXRoadCode = new DevComponents.DotNetBar.LabelX();
            this.labelXRoadLayer = new DevComponents.DotNetBar.LabelX();
            this.comboBoxRoadName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelXRoadName = new DevComponents.DotNetBar.LabelX();
            this.panelExTreeLayer = new DevComponents.DotNetBar.PanelEx();
            this.advTreeLayer = new DevComponents.AdvTree.AdvTree();
            this.node1 = new DevComponents.AdvTree.Node();
            this.node2 = new DevComponents.AdvTree.Node();
            this.node3 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.groupPanel3.SuspendLayout();
            this.groupPanelDM.SuspendLayout();
            this.groupPanelRoad.SuspendLayout();
            this.panelExTreeLayer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayer)).BeginInit();
            this.SuspendLayout();
            // 
            // listQueryType
            // 
            this.listQueryType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listQueryType.FormattingEnabled = true;
            this.listQueryType.ItemHeight = 12;
            this.listQueryType.Items.AddRange(new object[] {
            "地名查询",
            "道路查询"});
            this.listQueryType.Location = new System.Drawing.Point(0, 0);
            this.listQueryType.Name = "listQueryType";
            this.listQueryType.Size = new System.Drawing.Size(118, 196);
            this.listQueryType.TabIndex = 5;
            this.listQueryType.SelectedIndexChanged += new System.EventHandler(this.listQueryType_SelectedIndexChanged);
            // 
            // groupPanel3
            // 
            this.groupPanel3.BackColor = System.Drawing.Color.White;
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.listQueryType);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(3, 8);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(120, 198);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel3.TabIndex = 7;
            // 
            // buttonOK
            // 
            this.buttonOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOK.Location = new System.Drawing.Point(179, 176);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(65, 23);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "确定";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonCancel.Location = new System.Drawing.Point(279, 176);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(65, 23);
            this.buttonCancel.TabIndex = 11;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupPanelDM
            // 
            this.groupPanelDM.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanelDM.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanelDM.Controls.Add(this.labelQueryName);
            this.groupPanelDM.Controls.Add(this.comboBoxDMLayer1);
            this.groupPanelDM.Controls.Add(this.comboBoxDMLayer2);
            this.groupPanelDM.Controls.Add(this.labelXDMLayer2);
            this.groupPanelDM.Controls.Add(this.labelXDMLayer1);
            this.groupPanelDM.Controls.Add(this.comboBoxDMField);
            this.groupPanelDM.Controls.Add(this.labelX1);
            this.groupPanelDM.Controls.Add(this.labelXDMField);
            this.groupPanelDM.Location = new System.Drawing.Point(131, 7);
            this.groupPanelDM.Name = "groupPanelDM";
            this.groupPanelDM.Size = new System.Drawing.Size(272, 160);
            // 
            // 
            // 
            this.groupPanelDM.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanelDM.Style.BackColorGradientAngle = 90;
            this.groupPanelDM.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanelDM.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelDM.Style.BorderBottomWidth = 1;
            this.groupPanelDM.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanelDM.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelDM.Style.BorderLeftWidth = 1;
            this.groupPanelDM.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelDM.Style.BorderRightWidth = 1;
            this.groupPanelDM.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelDM.Style.BorderTopWidth = 1;
            this.groupPanelDM.Style.CornerDiameter = 4;
            this.groupPanelDM.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanelDM.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanelDM.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanelDM.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanelDM.TabIndex = 13;
            // 
            // labelQueryName
            // 
            this.labelQueryName.BackColor = System.Drawing.Color.Transparent;
            this.labelQueryName.Location = new System.Drawing.Point(68, 8);
            this.labelQueryName.Name = "labelQueryName";
            this.labelQueryName.Size = new System.Drawing.Size(189, 21);
            this.labelQueryName.TabIndex = 22;
            this.labelQueryName.Text = "地名查询";
            // 
            // comboBoxDMLayer1
            // 
            this.comboBoxDMLayer1.DisplayMember = "Text";
            this.comboBoxDMLayer1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxDMLayer1.FormattingEnabled = true;
            this.comboBoxDMLayer1.ItemHeight = 15;
            this.comboBoxDMLayer1.Location = new System.Drawing.Point(68, 38);
            this.comboBoxDMLayer1.Name = "comboBoxDMLayer1";
            this.comboBoxDMLayer1.Size = new System.Drawing.Size(193, 21);
            this.comboBoxDMLayer1.TabIndex = 19;
            this.comboBoxDMLayer1.DropDown += new System.EventHandler(this.comboBoxDMLayer1_DropDown);
            // 
            // comboBoxDMLayer2
            // 
            this.comboBoxDMLayer2.DisplayMember = "Text";
            this.comboBoxDMLayer2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxDMLayer2.FormattingEnabled = true;
            this.comboBoxDMLayer2.ItemHeight = 15;
            this.comboBoxDMLayer2.Location = new System.Drawing.Point(68, 68);
            this.comboBoxDMLayer2.Name = "comboBoxDMLayer2";
            this.comboBoxDMLayer2.Size = new System.Drawing.Size(193, 21);
            this.comboBoxDMLayer2.TabIndex = 20;
            this.comboBoxDMLayer2.DropDown += new System.EventHandler(this.comboBoxDMLayer2_DropDown);
            // 
            // labelXDMLayer2
            // 
            this.labelXDMLayer2.BackColor = System.Drawing.Color.Transparent;
            this.labelXDMLayer2.Location = new System.Drawing.Point(5, 68);
            this.labelXDMLayer2.Name = "labelXDMLayer2";
            this.labelXDMLayer2.Size = new System.Drawing.Size(73, 22);
            this.labelXDMLayer2.TabIndex = 15;
            this.labelXDMLayer2.Text = "地名图层2：";
            // 
            // labelXDMLayer1
            // 
            this.labelXDMLayer1.BackColor = System.Drawing.Color.Transparent;
            this.labelXDMLayer1.Location = new System.Drawing.Point(5, 38);
            this.labelXDMLayer1.Name = "labelXDMLayer1";
            this.labelXDMLayer1.Size = new System.Drawing.Size(73, 22);
            this.labelXDMLayer1.TabIndex = 14;
            this.labelXDMLayer1.Text = "地名图层1：";
            // 
            // comboBoxDMField
            // 
            this.comboBoxDMField.DisplayMember = "Text";
            this.comboBoxDMField.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxDMField.FormattingEnabled = true;
            this.comboBoxDMField.ItemHeight = 15;
            this.comboBoxDMField.Location = new System.Drawing.Point(68, 99);
            this.comboBoxDMField.Name = "comboBoxDMField";
            this.comboBoxDMField.Size = new System.Drawing.Size(193, 21);
            this.comboBoxDMField.TabIndex = 18;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(5, 6);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(72, 24);
            this.labelX1.TabIndex = 13;
            this.labelX1.Text = "查询名称：";
            // 
            // labelXDMField
            // 
            this.labelXDMField.BackColor = System.Drawing.Color.Transparent;
            this.labelXDMField.Location = new System.Drawing.Point(5, 98);
            this.labelXDMField.Name = "labelXDMField";
            this.labelXDMField.Size = new System.Drawing.Size(74, 22);
            this.labelXDMField.TabIndex = 16;
            this.labelXDMField.Text = "地名字段：";
            // 
            // groupPanelRoad
            // 
            this.groupPanelRoad.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanelRoad.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanelRoad.Controls.Add(this.labelXRoad);
            this.groupPanelRoad.Controls.Add(this.comboBoxRoadLayer);
            this.groupPanelRoad.Controls.Add(this.comboBoxRoadCode);
            this.groupPanelRoad.Controls.Add(this.labelXRoadCode);
            this.groupPanelRoad.Controls.Add(this.labelXRoadLayer);
            this.groupPanelRoad.Controls.Add(this.comboBoxRoadName);
            this.groupPanelRoad.Controls.Add(this.labelX5);
            this.groupPanelRoad.Controls.Add(this.labelXRoadName);
            this.groupPanelRoad.Location = new System.Drawing.Point(131, 7);
            this.groupPanelRoad.Name = "groupPanelRoad";
            this.groupPanelRoad.Size = new System.Drawing.Size(272, 160);
            // 
            // 
            // 
            this.groupPanelRoad.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanelRoad.Style.BackColorGradientAngle = 90;
            this.groupPanelRoad.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanelRoad.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelRoad.Style.BorderBottomWidth = 1;
            this.groupPanelRoad.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanelRoad.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelRoad.Style.BorderLeftWidth = 1;
            this.groupPanelRoad.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelRoad.Style.BorderRightWidth = 1;
            this.groupPanelRoad.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelRoad.Style.BorderTopWidth = 1;
            this.groupPanelRoad.Style.CornerDiameter = 4;
            this.groupPanelRoad.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanelRoad.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanelRoad.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanelRoad.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanelRoad.TabIndex = 23;
            // 
            // labelXRoad
            // 
            this.labelXRoad.BackColor = System.Drawing.Color.Transparent;
            this.labelXRoad.Location = new System.Drawing.Point(68, 8);
            this.labelXRoad.Name = "labelXRoad";
            this.labelXRoad.Size = new System.Drawing.Size(189, 21);
            this.labelXRoad.TabIndex = 22;
            this.labelXRoad.Text = "道路查询";
            // 
            // comboBoxRoadLayer
            // 
            this.comboBoxRoadLayer.DisplayMember = "Text";
            this.comboBoxRoadLayer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxRoadLayer.FormattingEnabled = true;
            this.comboBoxRoadLayer.ItemHeight = 15;
            this.comboBoxRoadLayer.Location = new System.Drawing.Point(68, 38);
            this.comboBoxRoadLayer.Name = "comboBoxRoadLayer";
            this.comboBoxRoadLayer.Size = new System.Drawing.Size(193, 21);
            this.comboBoxRoadLayer.TabIndex = 19;
            this.comboBoxRoadLayer.DropDown += new System.EventHandler(this.comboBoxRoadLayer_DropDown);
            // 
            // comboBoxRoadCode
            // 
            this.comboBoxRoadCode.DisplayMember = "Text";
            this.comboBoxRoadCode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxRoadCode.FormattingEnabled = true;
            this.comboBoxRoadCode.ItemHeight = 15;
            this.comboBoxRoadCode.Location = new System.Drawing.Point(68, 68);
            this.comboBoxRoadCode.Name = "comboBoxRoadCode";
            this.comboBoxRoadCode.Size = new System.Drawing.Size(193, 21);
            this.comboBoxRoadCode.TabIndex = 20;
            // 
            // labelXRoadCode
            // 
            this.labelXRoadCode.BackColor = System.Drawing.Color.Transparent;
            this.labelXRoadCode.Location = new System.Drawing.Point(5, 68);
            this.labelXRoadCode.Name = "labelXRoadCode";
            this.labelXRoadCode.Size = new System.Drawing.Size(73, 22);
            this.labelXRoadCode.TabIndex = 15;
            this.labelXRoadCode.Text = "编码字段：";
            // 
            // labelXRoadLayer
            // 
            this.labelXRoadLayer.BackColor = System.Drawing.Color.Transparent;
            this.labelXRoadLayer.Location = new System.Drawing.Point(5, 38);
            this.labelXRoadLayer.Name = "labelXRoadLayer";
            this.labelXRoadLayer.Size = new System.Drawing.Size(73, 22);
            this.labelXRoadLayer.TabIndex = 14;
            this.labelXRoadLayer.Text = "道路图层：";
            // 
            // comboBoxRoadName
            // 
            this.comboBoxRoadName.DisplayMember = "Text";
            this.comboBoxRoadName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxRoadName.FormattingEnabled = true;
            this.comboBoxRoadName.ItemHeight = 15;
            this.comboBoxRoadName.Location = new System.Drawing.Point(68, 99);
            this.comboBoxRoadName.Name = "comboBoxRoadName";
            this.comboBoxRoadName.Size = new System.Drawing.Size(193, 21);
            this.comboBoxRoadName.TabIndex = 18;
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            this.labelX5.Location = new System.Drawing.Point(5, 6);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(72, 24);
            this.labelX5.TabIndex = 13;
            this.labelX5.Text = "查询名称：";
            // 
            // labelXRoadName
            // 
            this.labelXRoadName.BackColor = System.Drawing.Color.Transparent;
            this.labelXRoadName.Location = new System.Drawing.Point(5, 98);
            this.labelXRoadName.Name = "labelXRoadName";
            this.labelXRoadName.Size = new System.Drawing.Size(74, 22);
            this.labelXRoadName.TabIndex = 16;
            this.labelXRoadName.Text = "名称字段：";
            // 
            // panelExTreeLayer
            // 
            this.panelExTreeLayer.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExTreeLayer.Controls.Add(this.advTreeLayer);
            this.panelExTreeLayer.Location = new System.Drawing.Point(130, 170);
            this.panelExTreeLayer.Name = "panelExTreeLayer";
            this.panelExTreeLayer.Size = new System.Drawing.Size(272, 35);
            this.panelExTreeLayer.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExTreeLayer.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelExTreeLayer.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelExTreeLayer.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExTreeLayer.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelExTreeLayer.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExTreeLayer.Style.GradientAngle = 90;
            this.panelExTreeLayer.TabIndex = 24;
            this.panelExTreeLayer.Text = "panelEx1";
            this.panelExTreeLayer.Visible = false;
            this.panelExTreeLayer.Leave += new System.EventHandler(this.panelExTreeLayer_Leave);
            // 
            // advTreeLayer
            // 
            this.advTreeLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeLayer.AllowDrop = true;
            this.advTreeLayer.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeLayer.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTreeLayer.Location = new System.Drawing.Point(0, 0);
            this.advTreeLayer.Name = "advTreeLayer";
            this.advTreeLayer.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1,
            this.node2});
            this.advTreeLayer.NodesConnector = this.nodeConnector1;
            this.advTreeLayer.NodeStyle = this.elementStyle1;
            this.advTreeLayer.PathSeparator = ";";
            this.advTreeLayer.Size = new System.Drawing.Size(272, 35);
            this.advTreeLayer.Styles.Add(this.elementStyle1);
            this.advTreeLayer.TabIndex = 12;
            this.advTreeLayer.Text = "advTree1";
            this.advTreeLayer.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeLayer_NodeDoubleClick);
            this.advTreeLayer.Leave += new System.EventHandler(this.advTreeLayer_Leave);
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
            // 
            // node2
            // 
            this.node2.Expanded = true;
            this.node2.Name = "node2";
            this.node2.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node3});
            this.node2.Text = "aa";
            // 
            // node3
            // 
            this.node3.Expanded = true;
            this.node3.Name = "node3";
            this.node3.Text = "cc";
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
            // FormQueryConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 210);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panelExTreeLayer);
            this.Controls.Add(this.groupPanelDM);
            this.Controls.Add(this.groupPanelRoad);
            this.Controls.Add(this.groupPanel3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormQueryConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询功能配置";
            this.Load += new System.EventHandler(this.FormQueryConfig_Load);
            this.groupPanel3.ResumeLayout(false);
            this.groupPanelDM.ResumeLayout(false);
            this.groupPanelRoad.ResumeLayout(false);
            this.panelExTreeLayer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listQueryType;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.ButtonX buttonOK;
        private DevComponents.DotNetBar.ButtonX buttonCancel;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanelDM;
        private DevComponents.DotNetBar.LabelX labelQueryName;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxDMLayer1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxDMLayer2;
        private DevComponents.DotNetBar.LabelX labelXDMLayer2;
        private DevComponents.DotNetBar.LabelX labelXDMLayer1;
        private DevComponents.DotNetBar.LabelX labelXDMField;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxDMField;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanelRoad;
        private DevComponents.DotNetBar.LabelX labelXRoad;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxRoadLayer;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxRoadCode;
        private DevComponents.DotNetBar.LabelX labelXRoadCode;
        private DevComponents.DotNetBar.LabelX labelXRoadLayer;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxRoadName;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelXRoadName;
        private DevComponents.DotNetBar.PanelEx panelExTreeLayer;
        private DevComponents.AdvTree.AdvTree advTreeLayer;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.Node node2;
        private DevComponents.AdvTree.Node node3;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
    }
}