using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using stdole;


namespace GeoDataCenterFunLib
{
    public partial class frmSetLabel : DevComponents.DotNetBar.Office2007Form
    {
        public frmSetLabel()
        {
            InitializeComponent();
        }

        private IGeoFeatureLayer pGeoFeatLayer = null;
        public IGeoFeatureLayer GeoFeatLayer
        {
            set { pGeoFeatLayer = value; }
        }
        private IMapControlDefault pMapControl = null;
        public IMapControlDefault MapControl
        {
            set { pMapControl = value; }
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            FontDialog pFontDialog = new FontDialog();
            pFontDialog.ShowHelp = true;
            if (pFontDialog.ShowDialog() == DialogResult.OK)
            {
                LabelText.Font = pFontDialog.Font;
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            ColorDialog pColorDialog = new ColorDialog();
            pColorDialog.AllowFullOpen = true;
            pColorDialog.AnyColor = true;
            if (pColorDialog.ShowDialog() == DialogResult.OK)
            {
                LabelText.ForeColor = pColorDialog.Color;
            }

        }
        private void AddLabel(string StrDisplayField)
        {
            pGeoFeatLayer.DisplayAnnotation = false;
            pMapControl.ActiveView.Refresh();

            pGeoFeatLayer.DisplayField = StrDisplayField;

            IAnnotateLayerPropertiesCollection pAnnoProps = null;
            pAnnoProps = pGeoFeatLayer.AnnotationProperties;

            ILineLabelPosition pPosition = null;
            pPosition = new LineLabelPositionClass();
            pPosition.Parallel = true;
            pPosition.Above = true;

            ILineLabelPlacementPriorities pPlacement = new LineLabelPlacementPrioritiesClass();
            IBasicOverposterLayerProperties4 pBasic = new BasicOverposterLayerPropertiesClass();
            pBasic.FeatureType = esriBasicOverposterFeatureType.esriOverposterPolyline;
            pBasic.LineLabelPlacementPriorities = pPlacement;
            pBasic.LineLabelPosition = pPosition;
            pBasic.BufferRatio = 0;
            pBasic.FeatureWeight = esriBasicOverposterWeight.esriHighWeight;
            pBasic.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerPart;
            //pBasic.PlaceOnlyInsidePolygon = true;//仅在地物内部显示标注  deleted by chulili s20111018 界面上并没有这项设置，这句话应注释掉，否则像是错误

            ILabelEngineLayerProperties pLabelEngine = null;
            pLabelEngine = new LabelEngineLayerPropertiesClass();
            pLabelEngine.BasicOverposterLayerProperties = pBasic as IBasicOverposterLayerProperties;
            pLabelEngine.Expression = "[" + StrDisplayField + "]";
            ITextSymbol pTextSymbol = null;
            pTextSymbol = pLabelEngine.Symbol;
            System.Drawing.Font pFont = null;
            pFont = LabelText.Font;
            IFontDisp pFontDisp = ESRI.ArcGIS.ADF.Converter.ToStdFont(pFont);
            pTextSymbol.Font = pFontDisp;

            IRgbColor pColor = new RgbColorClass();
            pColor.Red = Convert.ToInt32(LabelText.ForeColor.R);
            pColor.Green = Convert.ToInt32(LabelText.ForeColor.G);
            pColor.Blue = Convert.ToInt32(LabelText.ForeColor.B);
            pTextSymbol.Color = pColor as IColor;
            pLabelEngine.Symbol = pTextSymbol;

            IAnnotateLayerProperties pAnnoLayerProps = null;
            pAnnoLayerProps = pLabelEngine as IAnnotateLayerProperties;
            pAnnoLayerProps.LabelWhichFeatures = esriLabelWhichFeatures.esriAllFeatures;
            pAnnoProps.Clear();

            pAnnoProps.Add(pAnnoLayerProps);
            pGeoFeatLayer.DisplayAnnotation = true;
            pMapControl.ActiveView.Refresh();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string DisplayField = "";
            DisplayField = CmbFields.Text.Trim();
            if (DisplayField == "")
            {
                DisplayField = "OBJECTID";
            }
            AddLabel(DisplayField);
            this.Close();
        }

        private void btnConcel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSetLabel_Load(object sender, EventArgs e)
        {
            //获取字段
            IFields pFields = null;
            string FieldName = "";
            if (pGeoFeatLayer != null)
            {
                IDisplayRelationshipClass pDisResCls = pGeoFeatLayer as IDisplayRelationshipClass;
                if (pDisResCls.RelationshipClass != null)
                {
                    //如果进行了联表查询，则图层的字段名称发生了变化，通过将“层名”和“原字段名称”组合起来形成新的字段名称
                    //加载到CmbFields中
                    string pLayerName = pGeoFeatLayer.Name;//获得图层名
                    pFields = pGeoFeatLayer.FeatureClass.Fields;
                    for (int i = 0; i < pFields.FieldCount; i++)
                    {
                        //遍历图层的字段，将“图层名”和“字段名”组合起来
                        FieldName = pFields.get_Field(i).Name;
                        if (FieldName.ToLower() == "shape") continue;//如果是几何字段，则过滤掉
                        if (FieldName.ToLower() == "objectid") continue;//如果是ID过滤
                        FieldName = pLayerName + "." + FieldName;
                        CmbFields.Items.Add(FieldName);
                    }
                }
                else
                {
                    //没有进行联表查询，则直接将字段的名称加载到CmbFields中
                    pFields = pGeoFeatLayer.FeatureClass.Fields;
                    for (int i = 0; i < pFields.FieldCount; i++)
                    {
                        FieldName = pFields.get_Field(i).Name;
                        if (FieldName.ToLower() == "shape") continue;
                        if (FieldName.ToLower() == "objectid")  
                            continue;//如果是ID过滤
                        CmbFields.Items.Add(FieldName);
                    }
                }
                CmbFields.SelectedIndex = 0;
            }
            //获取字体名称
            System.Drawing.Text.InstalledFontCollection FontsCol = new System.Drawing.Text.InstalledFontCollection();
            foreach (System.Drawing.FontFamily family in FontsCol.Families)
            {
                CmbFontName.Items.Add(family.Name);
            }
            //获取字体大小
            for (int i = 1; i < 30; i++)
            {
                CmbFontSize.Items.Add(i);
            }
        }

        private FontStyle newFont;
        private FontFamily newFamily = new FontFamily("宋体");
        private float newSize = 8;

        private void FontColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {
            LabelText.ForeColor = FontColorPicker.SelectedColor;
        }

        private void CmbFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            newFamily = new FontFamily(CmbFontName.SelectedItem.ToString());
            setFont();
        }

        private void CmbFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            newSize = (float)Convert.ToDouble(CmbFontSize.SelectedItem.ToString());
            setFont();
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            if (LabelText.Font.Bold == true)
            {
                btnBold.Checked = false;
            }
            else
            {
                btnBold.Checked = true;
            }
            newFont = newFont ^ FontStyle.Bold;
            setFont();
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            if (LabelText.Font.Italic == true)
            {
                btnItalic.Checked = false;
            }
            else
            {
                btnItalic.Checked = true;
            }
            newFont = newFont ^ FontStyle.Italic;
            setFont();
        }

        private void btnUnderLine_Click(object sender, EventArgs e)
        {
            if (LabelText.Font.Underline == true)
            {
                btnUnderLine.Checked = false;
            }
            else
            {
                btnUnderLine.Checked = true;
            }
            newFont = newFont ^ FontStyle.Underline;
            setFont();
        }
        private void setFont()
        {
            this.LabelText.Font = new System.Drawing.Font(newFamily, newSize, newFont);
        }

    }
}