using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;

namespace GeoSymbology
{
    public class frmSymbology:DevComponents.DotNetBar.Office2007Form
    {
        private Dictionary<string, IRendererUI> m_RendererObject;
        private ESRI.ArcGIS.Carto.IFeatureLayer m_FeatureLayer;
        private ESRI.ArcGIS.Controls.esriSymbologyStyleClass m_StyleClass;
        private ESRI.ArcGIS.Carto.IFeatureRenderer m_FeatureRenderer;
        private List<FieldInfo> m_FieldInfo;

        #region InitializeComponent

        private DevComponents.AdvTree.AdvTree treeCatelog;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.PanelEx panelTree;
        private DevComponents.DotNetBar.LabelX labelRenderImage;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter2;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private DevComponents.DotNetBar.ButtonX buttonOK;
        private DevComponents.DotNetBar.ButtonX buttonCancel;
        private DevComponents.DotNetBar.PanelEx panelProperty;
    
        private void InitializeComponent()
        {
            this.treeCatelog = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.panelProperty = new DevComponents.DotNetBar.PanelEx();
            this.panelTree = new DevComponents.DotNetBar.PanelEx();
            this.labelRenderImage = new DevComponents.DotNetBar.LabelX();
            this.expandableSplitter2 = new DevComponents.DotNetBar.ExpandableSplitter();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this.buttonOK = new DevComponents.DotNetBar.ButtonX();
            this.buttonCancel = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.treeCatelog)).BeginInit();
            this.panelTree.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeCatelog
            // 
            this.treeCatelog.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.treeCatelog.AllowDrop = true;
            this.treeCatelog.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.treeCatelog.BackgroundStyle.Class = "TreeBorderKey";
            this.treeCatelog.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeCatelog.Location = new System.Drawing.Point(0, 0);
            this.treeCatelog.Name = "treeCatelog";
            this.treeCatelog.NodesConnector = this.nodeConnector1;
            this.treeCatelog.NodeStyle = this.elementStyle1;
            this.treeCatelog.PathSeparator = ";";
            this.treeCatelog.Size = new System.Drawing.Size(135, 260);
            this.treeCatelog.Styles.Add(this.elementStyle1);
            this.treeCatelog.TabIndex = 0;
            this.treeCatelog.AfterNodeSelect += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.treeCatelog_AfterNodeSelect);
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
            // panelProperty
            // 
            this.panelProperty.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelProperty.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProperty.Location = new System.Drawing.Point(135, 0);
            this.panelProperty.Name = "panelProperty";
            this.panelProperty.Size = new System.Drawing.Size(465, 370);
            this.panelProperty.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelProperty.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelProperty.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelProperty.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelProperty.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelProperty.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelProperty.Style.GradientAngle = 90;
            this.panelProperty.TabIndex = 2;
            // 
            // panelTree
            // 
            this.panelTree.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelTree.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelTree.Controls.Add(this.labelRenderImage);
            this.panelTree.Controls.Add(this.expandableSplitter2);
            this.panelTree.Controls.Add(this.treeCatelog);
            this.panelTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTree.Location = new System.Drawing.Point(0, 0);
            this.panelTree.Name = "panelTree";
            this.panelTree.Size = new System.Drawing.Size(135, 370);
            this.panelTree.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelTree.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelTree.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelTree.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelTree.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelTree.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelTree.Style.GradientAngle = 90;
            this.panelTree.TabIndex = 8;
            // 
            // labelRenderImage
            // 
            this.labelRenderImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRenderImage.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.labelRenderImage.Location = new System.Drawing.Point(0, 261);
            this.labelRenderImage.Name = "labelRenderImage";
            this.labelRenderImage.Size = new System.Drawing.Size(135, 109);
            this.labelRenderImage.TabIndex = 10;
            // 
            // expandableSplitter2
            // 
            this.expandableSplitter2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.expandableSplitter2.Expandable = false;
            this.expandableSplitter2.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter2.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter2.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter2.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter2.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter2.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter2.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter2.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter2.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter2.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter2.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter2.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter2.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter2.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter2.Location = new System.Drawing.Point(0, 260);
            this.expandableSplitter2.Name = "expandableSplitter2";
            this.expandableSplitter2.Size = new System.Drawing.Size(135, 1);
            this.expandableSplitter2.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter2.TabIndex = 1;
            this.expandableSplitter2.TabStop = false;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.expandableSplitter1);
            this.panelEx1.Controls.Add(this.panelProperty);
            this.panelEx1.Controls.Add(this.panelTree);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(600, 370);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 10;
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.Expandable = false;
            this.expandableSplitter1.ExpandableControl = this.panelEx1;
            this.expandableSplitter1.ExpandActionClick = false;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(135, 0);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(2, 370);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 9;
            this.expandableSplitter1.TabStop = false;
            // 
            // buttonOK
            // 
            this.buttonOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOK.Location = new System.Drawing.Point(481, 376);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(55, 23);
            this.buttonOK.TabIndex = 11;
            this.buttonOK.Text = "确 定";
            this.buttonOK.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonCancel.Location = new System.Drawing.Point(544, 376);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(55, 23);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "取 消";
            this.buttonCancel.Click += new System.EventHandler(this.Button_Click);
            // 
            // frmSymbology
            // 
            this.ClientSize = new System.Drawing.Size(600, 404);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panelEx1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSymbology";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "符号方案配置";
            ((System.ComponentModel.ISupportInitialize)(this.treeCatelog)).EndInit();
            this.panelTree.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public frmSymbology(ESRI.ArcGIS.Carto.IFeatureLayer pFeatureLayer)
        {
            InitializeComponent();
            m_FeatureLayer = pFeatureLayer;
            if (m_FeatureLayer == null) return;
            m_StyleClass = GetSymbolStyle(pFeatureLayer);
            m_RendererObject = new Dictionary<string, IRendererUI>();
            m_FeatureRenderer = GetRenderer(m_FeatureLayer);
            m_FieldInfo = null;

            InitializeUI();
            InitRendererUI(m_FieldInfo);
        }

        public frmSymbology(byte[] rendererValue,string rendererType, ESRI.ArcGIS.Controls.esriSymbologyStyleClass pStyleClass,List<ESRI.ArcGIS.Geodatabase.IField> pFields)
        {
            m_FeatureLayer = null;
            m_StyleClass = pStyleClass;
            m_RendererObject = new Dictionary<string, IRendererUI>();
            m_FeatureRenderer = GetRenderer(rendererValue, rendererType);
            m_FieldInfo = new List<FieldInfo>();
            FieldInfo noneField = new FieldInfo();
            noneField.FieldName = "<NONE>";
            noneField.FieldDesc = "<NONE>";
            noneField.FieldType = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDate;
            m_FieldInfo.Add(noneField);

            if (pFields != null && pFields.Count != 0)
            {
                for (int i = 0; i < pFields.Count; i++)
                {
                    FieldInfo field = new FieldInfo();
                    //if ((pFields[i].VarType > 1 && pFields[i].VarType < 6 && pFields[i].Type != ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID)
                    //                || pFields[i].Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString)
                    if ((int)pFields[i].Type < 5)
                    {
                        field.FieldName = pFields[i].Name;
                        field.FieldDesc = pFields[i].AliasName;
                        field.FieldType = pFields[i].Type;
                        m_FieldInfo.Add(field);
                    }
                }
            }

            InitializeUI();
            InitRendererUI(m_FieldInfo);
        }

        public ESRI.ArcGIS.Carto.IFeatureRenderer FeatureRenderer()
        {
            if (this.DialogResult != DialogResult.OK) return null;
            string typeString = treeCatelog.SelectedNode.DataKey.ToString();
            return m_RendererObject[typeString].Renderer;
        }

        public void FeatureRenderer(ref byte[] pRenderValue,ref string pRenderType)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                pRenderValue = null;
                pRenderType = string.Empty;
                return;
            }
            string typeString = treeCatelog.SelectedNode.DataKey.ToString();
            ModuleCommon.SaveRendererToByte(ref pRenderValue, ref pRenderType, m_RendererObject[typeString]);
        }

        private void InitializeUI()
        {
            treeCatelog.Nodes.Clear();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(global::GeoSymbology.Properties.Resources.RendererType);
            for (int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
            {
                string name = doc.DocumentElement.ChildNodes[i].Attributes["Name"].Value;
                string text = doc.DocumentElement.ChildNodes[i].Attributes["Text"].Value;
                string imageKey = doc.DocumentElement.ChildNodes[i].Attributes["ImageKey"].Value;

                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Name = name;
                node.Text = text;
                node.TagString = imageKey;

                treeCatelog.Nodes.Add(node);
                InitTreeNode(doc.DocumentElement.ChildNodes[i], node);
            }
        }

        private void ButtonItem_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ButtonItem button = sender as DevComponents.DotNetBar.ButtonItem;
            switch (button.Name)
            {
                case "":
                    break;
            }
        }

        private void InitTreeNode(System.Xml.XmlNode xmlNode, DevComponents.AdvTree.Node node)
        {
            for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
            {
                string name = xmlNode.ChildNodes[i].Attributes["Name"].Value;
                string text = xmlNode.ChildNodes[i].Attributes["Text"].Value;
                string imageKey = xmlNode.ChildNodes[i].Attributes["ImageKey"].Value;
                string type = xmlNode.ChildNodes[i].Attributes["Type"].Value;

                DevComponents.AdvTree.Node childNode = new DevComponents.AdvTree.Node();
                childNode.Name = name;
                childNode.Text = text;
                childNode.TagString = imageKey;
                childNode.DataKey = type;
                node.Nodes.Add(childNode);
                InitTreeNode(xmlNode.ChildNodes[i], childNode);
            }
        }

        private void treeCatelog_AfterNodeSelect(object sender, DevComponents.AdvTree.AdvTreeNodeEventArgs e)
        {
            if (e.Node.Level == 0 && e.Node.Nodes.Count > 0)
                e.Node.TreeControl.SelectedNode = e.Node.Nodes[0];
            else if (e.Node.Level == 0)
            {
                panelProperty.Controls.Clear();
                labelRenderImage.Image = GetImage(e.Node.TagString);
            }
            else
            {
                panelProperty.Controls.Clear();
                labelRenderImage.Image = GetImage(e.Node.TagString);
                string typeString = e.Node.DataKey.ToString();
                CreateRendererObject(typeString);
                if (m_RendererObject.ContainsKey(typeString) == false) return;

                panelProperty.Controls.Add(m_RendererObject[typeString] as System.Windows.Forms.UserControl);
            }
        }

        private System.Drawing.Image GetImage(string imageKey)
        {
            System.Drawing.Image image = null;
            switch (imageKey)
            {
                case "BSim":
                    image = global::GeoSymbology.Properties.Resources.BSim;
                    break;
                case "BUN":
                    image = global::GeoSymbology.Properties.Resources.BUN;
                    break;
                case "BGC":
                    image = global::GeoSymbology.Properties.Resources.BGC;
                    break;
                case "BGS":
                    image = global::GeoSymbology.Properties.Resources.BGS;
                    break;
                case "BChartD":
                    image = global::GeoSymbology.Properties.Resources.BChartD;
                    break;
                case "BChart":
                    image = global::GeoSymbology.Properties.Resources.BChart;
                    break;
                case "BChartZ":
                    image = global::GeoSymbology.Properties.Resources.BChartZ;
                    break;
                case "GDB":
                    image = global::GeoSymbology.Properties.Resources.GDB;
                    break;
            }
            return image;
        }

        private void CreateRendererObject(string typeString)
        {
            if (m_RendererObject.ContainsKey(typeString))
                return;
            try
            {
                Type type = Type.GetType(typeString);
                if (type == null) return ;
                IRendererUI rendererObject = Activator.CreateInstance(type) as IRendererUI;

                ESRI.ArcGIS.Carto.IFeatureRenderer pRenderer = null;
                switch (rendererObject.RendererType)
                {
                    case enumRendererType.SimpleRenderer:
                        pRenderer = ModuleCommon.CreateSimpleRenderer(m_StyleClass);
                        break;
                    case enumRendererType.UniqueValueRenderer:
                        pRenderer = ModuleCommon.CreateUVRenderer(m_StyleClass);
                        break;
                    case enumRendererType.BreakColorRenderer:
                        pRenderer = ModuleCommon.CreateBreakColorRenderer(m_StyleClass);
                        break;
                    case enumRendererType.BreakSizeRenderer:
                        pRenderer = ModuleCommon.CreateBreakSizeRenderer(m_StyleClass);
                        break;
                    case enumRendererType.ChartRenderer:
                        pRenderer = ModuleCommon.CreateChartRenderer(m_StyleClass);//yjl20110830 add
                        break;
                }

                if (m_FeatureLayer == null)
                    rendererObject.InitRendererObject(m_FieldInfo, pRenderer, m_StyleClass);
                else
                    rendererObject.InitRendererObject(m_FeatureLayer, pRenderer, m_StyleClass);
                m_RendererObject.Add(typeString, rendererObject);
            }
            catch (Exception ex)
            {
            }
        }

        private void InitRendererUI(List<FieldInfo> pFields)
        {
            IRendererUI pRendererUI = null;
            if (m_FeatureRenderer is ESRI.ArcGIS.Carto.ISimpleRenderer)
                pRendererUI = new frmSimpleRenderer();
            else if (m_FeatureRenderer is ESRI.ArcGIS.Carto.IUniqueValueRenderer)
                pRendererUI = new frmMFUVRenderer();
            else if (m_FeatureRenderer is ESRI.ArcGIS.Carto.IClassBreaksRenderer)
            {
                ESRI.ArcGIS.Carto.IClassBreaksUIProperties pUIProp = m_FeatureRenderer as ESRI.ArcGIS.Carto.IClassBreaksUIProperties;
                if (pUIProp.ColorRamp != "")
                    pRendererUI = new frmBreakColorRenderer();
                else
                    pRendererUI = new frmBreakSizeRenderer();
            }
            else if (m_FeatureRenderer is ESRI.ArcGIS.Carto.IChartRenderer)
            {
                IChartRenderer pChartRenderer=m_FeatureRenderer as IChartRenderer;
                IChartSymbol pChartSymbol=pChartRenderer.ChartSymbol;
                if (pChartSymbol is IPieChartSymbol)
                    pRendererUI = new frmPieChartRenderer();//yjl20110906 add
                else if (pChartSymbol is IBarChartSymbol)
                    pRendererUI = new frmBarChartRenderer();
                else
                    pRendererUI = new frmStackedChartRenderer();

            }
            if (m_FeatureLayer == null)
                pRendererUI.InitRendererObject(pFields, m_FeatureRenderer, m_StyleClass);
            else
                pRendererUI.InitRendererObject(m_FeatureLayer, m_FeatureRenderer, m_StyleClass);
            m_RendererObject.Add(pRendererUI.GetType().FullName, pRendererUI);

            treeCatelog.SelectedNode = treeCatelog.FindNodeByName(pRendererUI.GetType().FullName);
        }

        /// <summary>
        /// 从图层获取符号方案
        /// </summary>
        /// <param name="pLayer"></param>
        /// <returns></returns>
        private ESRI.ArcGIS.Carto.IFeatureRenderer GetRenderer(ESRI.ArcGIS.Carto.IFeatureLayer pLayer)
        {
            if (pLayer == null) 
                return ModuleCommon.CreateSimpleRenderer(m_StyleClass);
            ESRI.ArcGIS.Carto.IGeoFeatureLayer pGeoLayer = pLayer as ESRI.ArcGIS.Carto.IGeoFeatureLayer;
            if (pGeoLayer == null || pGeoLayer.Renderer == null)
                return ModuleCommon.CreateSimpleRenderer(m_StyleClass);

            return pGeoLayer.Renderer;
        }

        /// <summary>
        /// 从二进制获取符号方案
        /// </summary>
        /// <param name="renderValue"></param>
        /// <returns></returns>
        private ESRI.ArcGIS.Carto.IFeatureRenderer GetRenderer(byte[] renderValue,string rendererType)
        {
            ESRI.ArcGIS.Carto.IFeatureRenderer pRenderer = ModuleCommon.LoadRendererFromByte(renderValue, rendererType);
            if (pRenderer == null)
                pRenderer = ModuleCommon.CreateSimpleRenderer(m_StyleClass);
            return pRenderer;
        }

        /// <summary>
        /// 从图层获取符号类型
        /// </summary>
        /// <param name="pLayer"></param>
        /// <returns></returns>
        private ESRI.ArcGIS.Controls.esriSymbologyStyleClass GetSymbolStyle(ESRI.ArcGIS.Carto.IFeatureLayer pLayer)
        {
            ESRI.ArcGIS.Controls.esriSymbologyStyleClass styleClass = ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassMarkerSymbols;
            if (pLayer == null) return styleClass;
            switch (pLayer.FeatureClass.ShapeType)
            {
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                    styleClass = ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassMarkerSymbols;
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                    styleClass = ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassLineSymbols;
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                    styleClass = ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassFillSymbols;
                    break;
            }
            switch (pLayer.FeatureClass.FeatureType)
            {
                case ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTAnnotation:
                    styleClass = ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassTextSymbols;
                    break;
            }
            return styleClass;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ButtonX button = sender as DevComponents.DotNetBar.ButtonX;
            switch (button.Name)
            {
                case "buttonOK":
                    this.DialogResult = DialogResult.OK;
                    break;
                case "buttonCancel":
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }
        }
    }
}
