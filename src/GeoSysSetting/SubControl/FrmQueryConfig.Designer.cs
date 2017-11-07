namespace GeoSysSetting
{
    partial class FrmQueryConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmQueryConfig));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.advTreeLayerList = new DevComponents.AdvTree.AdvTree();
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelField1 = new DevComponents.DotNetBar.LabelX();
            this.labelField2 = new DevComponents.DotNetBar.LabelX();
            this.listViewQueryLayer = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.buttonAddLayer = new DevComponents.DotNetBar.ButtonX();
            this.buttonDelLayer = new DevComponents.DotNetBar.ButtonX();
            this.comboBoxField1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboBoxField2 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.buttonDelAll = new DevComponents.DotNetBar.ButtonX();
            this.comboBoxQueryType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonCancel = new DevComponents.DotNetBar.ButtonX();
            this.buttonOK = new DevComponents.DotNetBar.ButtonX();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayerList)).BeginInit();
            this.panelEx2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.advTreeLayerList);
            this.panelEx1.Location = new System.Drawing.Point(6, 9);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(180, 304);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            this.panelEx1.Text = "panelEx1";
            // 
            // advTreeLayerList
            // 
            this.advTreeLayerList.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeLayerList.AllowDrop = true;
            this.advTreeLayerList.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeLayerList.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeLayerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTreeLayerList.Location = new System.Drawing.Point(0, 0);
            this.advTreeLayerList.Name = "advTreeLayerList";
            this.advTreeLayerList.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.advTreeLayerList.NodesConnector = this.nodeConnector1;
            this.advTreeLayerList.NodeStyle = this.elementStyle1;
            this.advTreeLayerList.PathSeparator = ";";
            this.advTreeLayerList.Size = new System.Drawing.Size(180, 304);
            this.advTreeLayerList.Styles.Add(this.elementStyle1);
            this.advTreeLayerList.TabIndex = 0;
            this.advTreeLayerList.Text = "advTree1";
            // 
            // node1
            // 
            this.node1.Expanded = true;
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
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(10, 43);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(67, 26);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "目标图层：";
            // 
            // labelField1
            // 
            this.labelField1.Location = new System.Drawing.Point(10, 203);
            this.labelField1.Name = "labelField1";
            this.labelField1.Size = new System.Drawing.Size(66, 23);
            this.labelField1.TabIndex = 3;
            this.labelField1.Text = "目标字段：";
            // 
            // labelField2
            // 
            this.labelField2.Location = new System.Drawing.Point(10, 235);
            this.labelField2.Name = "labelField2";
            this.labelField2.Size = new System.Drawing.Size(74, 23);
            this.labelField2.TabIndex = 3;
            this.labelField2.Text = "目标字段：";
            // 
            // listViewQueryLayer
            // 
            // 
            // 
            // 
            this.listViewQueryLayer.Border.Class = "ListViewBorder";
            this.listViewQueryLayer.Location = new System.Drawing.Point(71, 45);
            this.listViewQueryLayer.Name = "listViewQueryLayer";
            this.listViewQueryLayer.Size = new System.Drawing.Size(215, 145);
            this.listViewQueryLayer.TabIndex = 6;
            this.listViewQueryLayer.UseCompatibleStateImageBehavior = false;
            this.listViewQueryLayer.View = System.Windows.Forms.View.List;
            this.listViewQueryLayer.SelectedIndexChanged += new System.EventHandler(this.listViewQueryLayer_SelectedIndexChanged);
            // 
            // buttonAddLayer
            // 
            this.buttonAddLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonAddLayer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonAddLayer.Location = new System.Drawing.Point(20, 78);
            this.buttonAddLayer.Name = "buttonAddLayer";
            this.buttonAddLayer.Size = new System.Drawing.Size(32, 18);
            this.buttonAddLayer.TabIndex = 7;
            this.buttonAddLayer.Text = ">";
            this.buttonAddLayer.Tooltip = "添加图层";
            this.buttonAddLayer.Click += new System.EventHandler(this.buttonAddLayer_Click);
            // 
            // buttonDelLayer
            // 
            this.buttonDelLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonDelLayer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonDelLayer.Location = new System.Drawing.Point(20, 102);
            this.buttonDelLayer.Name = "buttonDelLayer";
            this.buttonDelLayer.Size = new System.Drawing.Size(32, 18);
            this.buttonDelLayer.TabIndex = 8;
            this.buttonDelLayer.Text = "<";
            this.buttonDelLayer.Tooltip = "移除图层";
            this.buttonDelLayer.Click += new System.EventHandler(this.buttonDelLayer_Click);
            // 
            // comboBoxField1
            // 
            this.comboBoxField1.DisplayMember = "Text";
            this.comboBoxField1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxField1.FormattingEnabled = true;
            this.comboBoxField1.ItemHeight = 15;
            this.comboBoxField1.Location = new System.Drawing.Point(71, 203);
            this.comboBoxField1.Name = "comboBoxField1";
            this.comboBoxField1.Size = new System.Drawing.Size(215, 21);
            this.comboBoxField1.TabIndex = 9;
            this.comboBoxField1.SelectedIndexChanged += new System.EventHandler(this.comboBoxField1_SelectedIndexChanged);
            // 
            // comboBoxField2
            // 
            this.comboBoxField2.DisplayMember = "Text";
            this.comboBoxField2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxField2.FormattingEnabled = true;
            this.comboBoxField2.ItemHeight = 15;
            this.comboBoxField2.Location = new System.Drawing.Point(71, 236);
            this.comboBoxField2.Name = "comboBoxField2";
            this.comboBoxField2.Size = new System.Drawing.Size(215, 21);
            this.comboBoxField2.TabIndex = 9;
            this.comboBoxField2.SelectedIndexChanged += new System.EventHandler(this.comboBoxField2_SelectedIndexChanged);
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx2.Controls.Add(this.comboBoxField2);
            this.panelEx2.Controls.Add(this.comboBoxField1);
            this.panelEx2.Controls.Add(this.buttonDelAll);
            this.panelEx2.Controls.Add(this.buttonDelLayer);
            this.panelEx2.Controls.Add(this.buttonAddLayer);
            this.panelEx2.Controls.Add(this.listViewQueryLayer);
            this.panelEx2.Controls.Add(this.comboBoxQueryType);
            this.panelEx2.Controls.Add(this.labelField2);
            this.panelEx2.Controls.Add(this.labelField1);
            this.panelEx2.Controls.Add(this.labelX2);
            this.panelEx2.Controls.Add(this.labelX1);
            this.panelEx2.Location = new System.Drawing.Point(199, 10);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(298, 271);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 10;
            this.panelEx2.Text = "panelEx2";
            // 
            // buttonDelAll
            // 
            this.buttonDelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonDelAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonDelAll.Location = new System.Drawing.Point(20, 135);
            this.buttonDelAll.Name = "buttonDelAll";
            this.buttonDelAll.Size = new System.Drawing.Size(32, 18);
            this.buttonDelAll.TabIndex = 8;
            this.buttonDelAll.Text = "<<";
            this.buttonDelAll.Tooltip = "全部移除";
            this.buttonDelAll.Click += new System.EventHandler(this.buttonDelAll_Click);
            // 
            // comboBoxQueryType
            // 
            this.comboBoxQueryType.DisplayMember = "Text";
            this.comboBoxQueryType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxQueryType.FormattingEnabled = true;
            this.comboBoxQueryType.ItemHeight = 15;
            this.comboBoxQueryType.Location = new System.Drawing.Point(71, 12);
            this.comboBoxQueryType.Name = "comboBoxQueryType";
            this.comboBoxQueryType.Size = new System.Drawing.Size(215, 21);
            this.comboBoxQueryType.TabIndex = 5;
            this.comboBoxQueryType.SelectedIndexChanged += new System.EventHandler(this.comboBoxQueryType_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(10, 11);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 24);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "查询类别：";
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonCancel.Location = new System.Drawing.Point(423, 290);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(70, 23);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOK.Location = new System.Drawing.Point(337, 290);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(70, 23);
            this.buttonOK.TabIndex = 11;
            this.buttonOK.Text = "确定";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
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
            // FrmQueryConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 317);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panelEx2);
            this.Controls.Add(this.panelEx1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmQueryConfig";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询配置";
            this.Load += new System.EventHandler(this.FrmQueryConfig_Load);
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayerList)).EndInit();
            this.panelEx2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.AdvTree.AdvTree advTreeLayerList;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelField1;
        private DevComponents.DotNetBar.LabelX labelField2;
        private DevComponents.DotNetBar.Controls.ListViewEx listViewQueryLayer;
        private DevComponents.DotNetBar.ButtonX buttonAddLayer;
        private DevComponents.DotNetBar.ButtonX buttonDelLayer;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxField1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxField2;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.ButtonX buttonCancel;
        private DevComponents.DotNetBar.ButtonX buttonOK;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxQueryType;
        private DevComponents.DotNetBar.LabelX labelX1;
        public System.Windows.Forms.ImageList ImageList;
        private DevComponents.DotNetBar.ButtonX buttonDelAll;
    }
}