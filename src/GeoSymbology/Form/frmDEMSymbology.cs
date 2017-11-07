using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

namespace GeoSymbology
{
    public class frmDEMSymbology:DevComponents.DotNetBar.Office2007Form
    {
        private Dictionary<string, IRasterRendererUI > m_RendererObject;
        private ESRI.ArcGIS.Carto.ILayer _Layer;
        private ESRI.ArcGIS.Controls.esriSymbologyStyleClass m_StyleClass;
        private ESRI.ArcGIS.Carto.IRasterRenderer _RasterRenderer;
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
            this.panelProperty.Size = new System.Drawing.Size(466, 371);
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
            this.panelTree.Size = new System.Drawing.Size(135, 371);
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
            this.labelRenderImage.Size = new System.Drawing.Size(135, 110);
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
            this.panelEx1.Size = new System.Drawing.Size(601, 371);
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
            this.expandableSplitter1.Size = new System.Drawing.Size(2, 371);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 9;
            this.expandableSplitter1.TabStop = false;
            // 
            // buttonOK
            // 
            this.buttonOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(480, 377);
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
            this.buttonCancel.Location = new System.Drawing.Point(543, 377);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(55, 23);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "取 消";
            this.buttonCancel.Click += new System.EventHandler(this.Button_Click);
            // 
            // frmDEMSymbology
            // 
            this.ClientSize = new System.Drawing.Size(601, 405);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panelEx1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDEMSymbology";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "符号方案配置";
            ((System.ComponentModel.ISupportInitialize)(this.treeCatelog)).EndInit();
            this.panelTree.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public frmDEMSymbology(ILayer pLayer)
        {
            
            InitializeComponent();
            _Layer = pLayer;
            if (_Layer == null) return;
            m_RendererObject = new Dictionary<string, IRasterRendererUI >();
            _RasterRenderer = GetRasterRenderer(pLayer);
            m_FieldInfo = null;

            InitializeUI();
            InitRendererUI();
        }

        public frmDEMSymbology(byte[] rendererValue,string rendererType, ESRI.ArcGIS.Controls.esriSymbologyStyleClass pStyleClass,List<ESRI.ArcGIS.Geodatabase.IField> pFields)
        {
            _Layer  = null;
            m_StyleClass = pStyleClass;
            m_RendererObject = new Dictionary<string, IRasterRendererUI >();
            _RasterRenderer  = GetRasterRenderer(rendererValue, rendererType);
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
            InitRendererUI();
        }

        public ESRI.ArcGIS.Carto.IRasterRenderer  RasterRenderer()
        {
            if (this.DialogResult != DialogResult.OK) return null;
            string typeString = treeCatelog.SelectedNode.DataKey.ToString();
            return m_RendererObject[typeString].RasterRenderer;
        }

        public void RasterRenderer(ref byte[] pRenderValue,ref string pRenderType)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                pRenderValue = null;
                pRenderType = string.Empty;
                return;
            }
            string typeString = treeCatelog.SelectedNode.DataKey.ToString();
            ModuleCommon.SaveRasterRendererToByte(ref pRenderValue, ref pRenderType, m_RendererObject[typeString]);
        }

        private void InitializeUI()
        {
            treeCatelog.Nodes.Clear();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(global::GeoSymbology.Properties.Resources.RasterRendererType );
            for (int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
            {
                string name = doc.DocumentElement.ChildNodes[i].Attributes["Name"].Value;
                string text = doc.DocumentElement.ChildNodes[i].Attributes["Text"].Value;
                string imageKey = doc.DocumentElement.ChildNodes[i].Attributes["ImageKey"].Value;
                string type = doc.DocumentElement.ChildNodes[i].Attributes["Type"].Value;
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Name = name;
                node.Text = text;
                node.TagString = imageKey;
                node.DataKey = type;
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
            //if (e.Node.Level == 0 && e.Node.Nodes.Count > 0)
            //    e.Node.TreeControl.SelectedNode = e.Node.Nodes[0];
            //else if (e.Node.Level == 0)
            //{
            //    panelProperty.Controls.Clear();
            //    labelRenderImage.Image = GetImage(e.Node.TagString);
            //}
            //else
            //{
                panelProperty.Controls.Clear();
                labelRenderImage.Image = GetImage(e.Node.TagString);
                if (e.Node.DataKey == null)
                {
                    return;
                }
                string typeString = e.Node.DataKey.ToString();
                CreateRendererObject(typeString);
                if (m_RendererObject.ContainsKey(typeString) == false) return;

                panelProperty.Controls.Add(m_RendererObject[typeString] as System.Windows.Forms.UserControl);
            //}
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
                IRasterRendererUI rendererObject = Activator.CreateInstance(type) as IRasterRendererUI;

                ESRI.ArcGIS.Carto.IRasterRenderer  pRasterRenderer = null;
                switch (rendererObject.RasterRendererType )
                {
                    case enumRasterRendererType.StretchColorRampRenderer:
                        pRasterRenderer = ModuleCommon.CreateStretchColorRampRenderer();
                        break;
                    case enumRasterRendererType.ClassifyColorRampRenderer:
                        pRasterRenderer = ModuleCommon.CreateClassifyColorRampRenderer();
                        break;
                    case enumRasterRendererType.UniqueValueRenderer:
                        pRasterRenderer = ModuleCommon.CreateUniqueValueRasterRenderer();
                        break;
                    case enumRasterRendererType.RGBRenderer:
                        pRasterRenderer = ModuleCommon.CreateRGBRenderer();
                        break;
                }

                if (_Layer  == null)
                    rendererObject.InitRasterRendererObject(m_FieldInfo, pRasterRenderer);
                else
                    rendererObject.InitRasterRendererObject(_Layer, pRasterRenderer);
                m_RendererObject.Add(typeString, rendererObject);
            }
            catch (Exception ex)
            {
            }
        }

        private void InitRendererUI()
        {
            IRasterRendererUI pRasterRendererUI = null;

            if (_RasterRenderer is IRasterRGBRenderer)
            {
                return; 
            }
            else if (_RasterRenderer is IRasterUniqueValueRenderer)
            {
                return; 
            }
            else if (_RasterRenderer is IRasterStretchColorRampRenderer)
            {
                pRasterRendererUI = new frmStretchColorRampRenderer();
            }
            else if (_RasterRenderer is IRasterClassifyColorRampRenderer)
            {
                return; 
            }
            else
            { 
                return; 
            }
            if (_Layer != null)
            {
                pRasterRendererUI.InitRasterRendererObject(_Layer,_RasterRenderer);
            }
            m_RendererObject.Add(pRasterRendererUI.GetType().FullName, pRasterRendererUI);

            treeCatelog.SelectedNode = treeCatelog.FindNodeByName(pRasterRendererUI.GetType().FullName);
        }

        /// <summary>
        /// 从图层获取符号方案
        /// </summary>
        /// <param name="pLayer"></param>
        /// <returns></returns>
        private ESRI.ArcGIS.Carto.IRasterRenderer  GetRasterRenderer(ESRI.ArcGIS.Carto.ILayer  pLayer)
        {
            IRasterRenderer pRasterRender = null;
            if (pLayer is IRasterLayer)
            {
                IRasterLayer pRasterLayer = pLayer as IRasterLayer;
                pRasterRender = pRasterLayer.Renderer;

            }
            else if (pLayer is IRasterCatalogLayer)
            {
                IRasterCatalogLayer pRasterCatalogLayer = pLayer as IRasterCatalogLayer;
                pRasterRender = pRasterCatalogLayer.Renderer;
            }
            return pRasterRender;

        }

        /// <summary>
        /// 从二进制获取符号方案
        /// </summary>
        /// <param name="renderValue"></param>
        /// <returns></returns>
        private ESRI.ArcGIS.Carto.IRasterRenderer  GetRasterRenderer(byte[] renderValue,string rendererType)
        {
            ESRI.ArcGIS.Carto.IRasterRenderer  pRenderer = ModuleCommon.LoadRasterRendererFromByte(renderValue, rendererType);
            if (pRenderer == null)
                pRenderer = ModuleCommon.CreateStretchColorRampRenderer();
            return pRenderer;
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
