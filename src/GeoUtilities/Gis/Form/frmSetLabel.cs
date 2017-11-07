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


namespace GeoUtilities
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
            if (this.rdbPerName.Checked)
            {
                pBasic.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerName;  //每个名称标注一次
            }
            else if (this.rdbPerPart.Checked)
            {
                pBasic.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerPart;  //每个部分标注一次
            }
            else
            {
                pBasic.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerShape;  //每个地物标注一次
            }
            //pBasic.NumLabelsOption = esriBasicNumLabelsOption.esriNoLabelRestrictions ;
            //pBasic.PlaceOnlyInsidePolygon = true;  //仅在地物内部显示标注  deleted by chulili s20111018 界面上并没有这项设置，这句话应注释掉，否则像是错误

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
            //最大\最小比例尺
            try
            {
                if (this.txtMaxLabelScale.Text.Trim() != "")
                {
                    pAnnoLayerProps.AnnotationMaximumScale = Convert.ToDouble(this.txtMaxLabelScale.Text);
                }
                if (this.txtMinLabelScale.Text.Trim() != "")
                {
                    pAnnoLayerProps.AnnotationMinimumScale = Convert.ToDouble(this.txtMinLabelScale.Text);
                }
            }
            catch(Exception err)
            {}
            pAnnoProps.Clear();

            pAnnoProps.Add(pAnnoLayerProps);
            pGeoFeatLayer.DisplayAnnotation = this.chkIsLabel.Checked ;
            (pMapControl.Map as IActiveView).Refresh();
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

        private bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg1
                = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
            return reg1.IsMatch(str);
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
            for (int i = 0; i < 11; i++)
            {
                CmbFontSize.Items.Add(i + 4);
            }
            for (int i = 11; i < 21; i++)
            {
                CmbFontSize.Items.Add(i + 3 * i);
            }

            // shduan 20110724 add
            try
            {
                getLayerLabel_Maplex(pGeoFeatLayer);
            }
            catch (Exception err)
            {}
        }
        private void getLayerLabel(IGeoFeatureLayer pGeoFeatLayer)
        {
            Boolean bIsLabel = pGeoFeatLayer.DisplayAnnotation;

            if (bIsLabel)
            {
                this.chkIsLabel.Checked = true;
            }
            else
            {
                this.chkIsLabel.Checked = false;//changed by chulili 20110712修改bug true->false
            }

            //added by chulili 20110804按照图层的标注设置初始化界面
            //this.chkIsLabel.Checked = pGeoFeatureLayer.DisplayAnnotation;
            IAnnotateLayerPropertiesCollection pAnnotateLayerPropertiesCollection = pGeoFeatLayer.AnnotationProperties;
            if (pAnnotateLayerPropertiesCollection == null) return;
            //定义IAnnotateLayerPropertiesCollection.QueryItem方法中调用的对象
            IAnnotateLayerProperties pAnnoLayerProperties = null;
            IElementCollection pElementCollection = null;
            IElementCollection pElementCollection1 = null;
            //获取标注渲染对象
            pAnnotateLayerPropertiesCollection.QueryItem(0, out  pAnnoLayerProperties, out pElementCollection, out pElementCollection1);
            if (pAnnoLayerProperties == null) return;
            ILabelEngineLayerProperties pLabelEngineLayerPro = pAnnoLayerProperties as ILabelEngineLayerProperties;
            if (pLabelEngineLayerPro == null) return;
            ITextSymbol pTextSymbol = pLabelEngineLayerPro.Symbol;
            IBasicOverposterLayerProperties pBasicOverposterLayerProp = pLabelEngineLayerPro.BasicOverposterLayerProperties;
            if (pBasicOverposterLayerProp != null)
            {
                switch (pBasicOverposterLayerProp.NumLabelsOption)           
                {
                    case esriBasicNumLabelsOption.esriOneLabelPerName:
                        this.rdbPerName.Checked = true;
                        break;
                    case esriBasicNumLabelsOption.esriOneLabelPerPart:
                        this.rdbPerPart.Checked = true;
                        break;
                    case esriBasicNumLabelsOption.esriOneLabelPerShape:
                        this.rdbPerShape.Checked = true;
                        break;
                    default:
                        this.rdbPerName.Checked = true;
                        break;
                }
            }
            /////////////////////////////
            string strMaxScale = pAnnoLayerProperties.AnnotationMaximumScale.ToString();
            string strMinScale = pAnnoLayerProperties.AnnotationMinimumScale.ToString();
            ////////////////////////////
            this.txtMaxLabelScale.Text = strMaxScale;
            this.txtMinLabelScale.Text = strMinScale;

            this.btnBold.Checked = pTextSymbol.Font.Bold;
            this.btnItalic.Checked = pTextSymbol.Font.Italic;
            this.btnUnderLine.Checked = pTextSymbol.Font.Underline;
            this.FontColorPicker.SelectedColor = ColorTranslator.FromOle(pTextSymbol.Color.RGB);

            newSize = (float)Convert.ToDouble(pTextSymbol.Font.Size);
            newFamily = new FontFamily(pTextSymbol.Font.Name);
            newFont = FontStyle.Regular;//CmbFields
            string strField = pLabelEngineLayerPro.Expression;
            //获取字段名称
            if (strField.StartsWith("["))
            {
                strField = strField.Substring(1);
            }
            if (strField.EndsWith("]"))
            {
                strField = strField.Substring(0, strField.Length - 1);
            }
            //设置界面上字段名称
            for (int i = 0; i < CmbFields.Items.Count; i++)
            {
                if (CmbFields.Items[i].ToString() == strField)
                {
                    CmbFields.SelectedIndex = i;
                    break;
                }

            }
            //设置界面上字体名称
            for (int i = 0; i < CmbFontName.Items.Count; i++)
            {
                if (CmbFontName.Items[i].ToString() == pTextSymbol.Font.Name)
                {
                    CmbFontName.SelectedIndex = i;
                    break;
                }

            }
            //设置界面上字体大小
            decimal dSize = decimal.Round(pTextSymbol.Font.Size, 0, MidpointRounding.AwayFromZero);
            string strLabelSize = Convert.ToInt32(dSize).ToString();
            for (int i = 0; i < CmbFontSize.Items.Count; i++)
            {
                if (CmbFontSize.Items[i].ToString() == strLabelSize)
                {
                    CmbFontSize.SelectedIndex = i;
                    break;
                }

            }

            if (pTextSymbol.Font.Underline)
                newFont = newFont ^ FontStyle.Underline;
            if (pTextSymbol.Font.Bold)
                newFont = newFont ^ FontStyle.Bold;
            if (pTextSymbol.Font.Italic)
                newFont = newFont ^ FontStyle.Italic;
            //设置界面上字体格式
            setFont();
            //设置界面上字体颜色
            LabelText.ForeColor = ColorTranslator.FromOle(pTextSymbol.Color.RGB);
        }
        private void getLayerLabel_Maplex(IGeoFeatureLayer pGeoFeatLayer)
        {
            Boolean  bIsLabel = pGeoFeatLayer.DisplayAnnotation;

            if (bIsLabel)
            {
                this.chkIsLabel.Checked = true;
            }
            else
            {
                this.chkIsLabel.Checked = false;//changed by chulili 20110712修改bug true->false
            }
            
            //added by chulili 20110804按照图层的标注设置初始化界面
            //this.chkIsLabel.Checked = pGeoFeatureLayer.DisplayAnnotation;
            IAnnotateLayerPropertiesCollection pAnnotateLayerPropertiesCollection = pGeoFeatLayer.AnnotationProperties;
            if (pAnnotateLayerPropertiesCollection == null) return;
            //定义IAnnotateLayerPropertiesCollection.QueryItem方法中调用的对象
            IAnnotateLayerProperties pAnnoLayerProperties = null;
            IElementCollection pElementCollection = null;
            IElementCollection pElementCollection1 = null;
            //获取标注渲染对象
            pAnnotateLayerPropertiesCollection.QueryItem(0, out  pAnnoLayerProperties, out pElementCollection, out pElementCollection1);
            if (pAnnoLayerProperties == null) return;
            ILabelEngineLayerProperties2 pLabelEngineLayerPro = pAnnoLayerProperties as ILabelEngineLayerProperties2;
            if (pLabelEngineLayerPro == null) return;
            ITextSymbol pTextSymbol = pLabelEngineLayerPro.Symbol;


            IMaplexOverposterLayerProperties pmaplexoverposterlayerprop = pLabelEngineLayerPro.OverposterLayerProperties as IMaplexOverposterLayerProperties;
            IBasicOverposterLayerProperties pBasicOverposterLayerProp = pLabelEngineLayerPro.OverposterLayerProperties as IBasicOverposterLayerProperties;
            if (pBasicOverposterLayerProp != null)
            {
                switch (pBasicOverposterLayerProp.NumLabelsOption )
                {
                    case esriBasicNumLabelsOption.esriOneLabelPerName:
                        this.rdbPerName.Checked = true;
                        break;
                    case esriBasicNumLabelsOption.esriOneLabelPerPart:
                        this.rdbPerPart.Checked = true;
                        break;
                    case esriBasicNumLabelsOption.esriOneLabelPerShape:
                        this.rdbPerShape.Checked = true;
                        break;
                    default:
                        this.rdbPerName.Checked = true;
                        break;
                }
            }
            /////////////////////////////
            string strMaxScale = pAnnoLayerProperties.AnnotationMaximumScale.ToString();
            string strMinScale = pAnnoLayerProperties.AnnotationMinimumScale.ToString();
            ////////////////////////////
            this.txtMaxLabelScale.Text = strMaxScale;
            this.txtMinLabelScale.Text = strMinScale;

            this.btnBold.Checked = pTextSymbol.Font.Bold;
            this.btnItalic.Checked = pTextSymbol.Font.Italic;
            this.btnUnderLine.Checked = pTextSymbol.Font.Underline;
            this.FontColorPicker.SelectedColor = ColorTranslator.FromOle(pTextSymbol.Color.RGB);

            newSize = (float)Convert.ToDouble(pTextSymbol.Font.Size);
            newFamily = new FontFamily(pTextSymbol.Font.Name);
            newFont = FontStyle.Regular;//CmbFields
            string strField = pLabelEngineLayerPro.Expression;
            //获取字段名称
            if (strField.StartsWith("["))
            {
                strField = strField.Substring(1);
            }
            if (strField.EndsWith("]"))
            {
                strField = strField.Substring(0, strField.Length - 1);
            }
            //设置界面上字段名称
            for (int i = 0; i < CmbFields.Items.Count; i++)
            {
                if (CmbFields.Items[i].ToString() == strField)
                {
                    CmbFields.SelectedIndex = i;
                    break;
                }

            }
            //设置界面上字体名称
            for (int i = 0; i < CmbFontName.Items.Count; i++)
            {
                if (CmbFontName.Items[i].ToString() == pTextSymbol.Font.Name)
                {
                    CmbFontName.SelectedIndex = i;
                    break;
                }

            }
            //设置界面上字体大小
            decimal dSize = decimal.Round(pTextSymbol.Font.Size, 0, MidpointRounding.AwayFromZero);
            string strLabelSize = Convert.ToInt32(dSize).ToString();
            for (int i = 0; i < CmbFontSize.Items.Count; i++)
            {
                if (CmbFontSize.Items[i].ToString() == strLabelSize)
                {
                    CmbFontSize.SelectedIndex = i;
                    break;
                }

            }

            if (pTextSymbol.Font.Underline)
                newFont = newFont ^ FontStyle.Underline;
            if (pTextSymbol.Font.Bold)
                newFont = newFont ^ FontStyle.Bold;
            if (pTextSymbol.Font.Italic)
                newFont = newFont ^ FontStyle.Italic;
            //设置界面上字体格式
            setFont();
            //设置界面上字体颜色
            LabelText.ForeColor = ColorTranslator.FromOle(pTextSymbol.Color.RGB);
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

        private void CmbFontSize_ComboBoxTextChanged(object sender, EventArgs e)
        {
            if (CmbFontSize.ControlText.Trim() == string.Empty || !IsNumeric(CmbFontSize.ControlText))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "字体大小请输入数字!");
                return;
            }

            newSize = (float)Convert.ToDouble(CmbFontSize.ControlText);
            setFont();

        }
        public System.Drawing.Font GetFontFromIFontDisp(IFontDisp obj)
        {
            string name = obj.Name;
            float size = (float)obj.Size;
            FontStyle pFontStyle = FontStyle.Regular;

            if (obj.Bold)
            {
                pFontStyle = FontStyle.Bold;
            }
            if (obj.Italic)
            {
                pFontStyle = pFontStyle | FontStyle.Italic;
            }

            if (obj.Strikethrough)
            {
                pFontStyle = pFontStyle | FontStyle.Strikeout;
            }

            if (obj.Underline)
            {
                pFontStyle = pFontStyle | FontStyle.Underline;
            }

            System.Drawing.Font pFont = new System.Drawing.Font(name, size, pFontStyle);

            return pFont;
        }

        private void txtMinLabelScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

        private void txtMaxLabelScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }


    }
}