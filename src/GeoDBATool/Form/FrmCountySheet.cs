using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using stdole;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    public partial class FrmCountySheet : DevComponents.DotNetBar.Office2007Form
    {
        //比例尺
        //private string v_Scale = "";
        //选择的总范围
        //private IGeometry m_CountyGeometry = null;
        //public IGeometry CountyGEOMETRY
        //{
        //    get 
        //    {
        //        return m_CountyGeometry;
        //    }
        //    set 
        //    {
        //        m_CountyGeometry=value;
        //    }
        //}

        //选择的范围的要素信息集合
        private Dictionary<string, IGeometry> m_rangeFeaDic = null;
        public Dictionary<string, IGeometry> RANGEFEADIC
        {
            get 
            {
                return m_rangeFeaDic;
            }
            set 
            {
                m_rangeFeaDic = value;
            }
        }

        public FrmCountySheet(string pScale,string textStr)
        {
            InitializeComponent();
            axToolbarControl1.SetBuddyControl(countySheetControl.Object);

            Exception eError = null;

            this.Text = textStr;
            //v_Scale = pScale;
            //添加图层
            if (this.Text == "行政区范围选择")
            {
                AddRangeLayer(ModData.countyPath, pScale, "NAME",out eError);
            }
            else if (this.Text == "图幅选择")
            {
                AddRangeLayer(ModData.MapPath, pScale, "MAP_NEWNO", out eError);
            }
            if(eError!=null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                return;
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            if (countySheetControl.Map.LayerCount == 0) return;
            IFeatureLayer pFeaLayer = countySheetControl.Map.get_Layer(0) as IFeatureLayer;
            IFeatureSelection pFeaSel = pFeaLayer as IFeatureSelection;
            if (pFeaSel.SelectionSet == null || pFeaSel.SelectionSet.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请在图上选择范围！");
                return;
            }
            if (pFeaSel.SelectionSet != null && pFeaSel.SelectionSet.Count != 0)
            {
                //m_CountyGeometry = GetFeaGeometry(pFeaSel, pFeaLayer,"NAME", out m_rangeFeaDic);
                if (this.Text == "行政区范围选择")
                {
                    m_rangeFeaDic = GetFeaGeometry(pFeaSel, pFeaLayer, "NAME", out eError);

                }
                else if (this.Text == "图幅选择")
                {
                    m_rangeFeaDic = GetFeaGeometry(pFeaSel, pFeaLayer, "MAP_NEWNO", out eError);
                }
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        //获得选择的图层的范围
        private Dictionary<string,IGeometry> GetFeaGeometry(IFeatureSelection pFeatureSel, IFeatureLayer pMapLayer,string fieldName, out Exception eError)
        {
            eError = null;
            Dictionary<string, IGeometry> feaDic = new Dictionary<string, IGeometry>();
            IEnumIDs pEnumIDs = pFeatureSel.SelectionSet.IDs;
            int id = pEnumIDs.Next();
            //IGeometry pGeo = null;
            //int rangNOIndex = pMapLayer.FeatureClass.Fields.FindField("RANGE_NO");  //范围号索引
            int rangeNameIndex = pMapLayer.FeatureClass.Fields.FindField(fieldName);   //范围名称索引
            if (rangeNameIndex == -1) //||rangNOIndex==-1
            {
                eError=new Exception("范围号或范围名称字段不存在！");
                return null;
            }
            while (id != -1)
            {
                IFeature pFeat = pMapLayer.FeatureClass.GetFeature(id);
                //string rangeNO = pFeat.get_Value(rangNOIndex).ToString();   //范围号
                string rangeName = pFeat.get_Value(rangeNameIndex).ToString();//范围名称
                if(!feaDic.ContainsKey(rangeName))
                {
                    feaDic.Add(rangeName, pFeat.Shape);
                }
                //if (pGeo == null)
                //{
                //    pGeo = pFeat.Shape;
                //}
                //else
                //{
                //    ITopologicalOperator pTop = pGeo as ITopologicalOperator;
                //    pGeo = pTop.Union(pFeat.Shape);
                //}
                id = pEnumIDs.Next();
            }
            return feaDic;
        }

        //添加图层
        private void AddRangeLayer(string wsPath, string pScale, string fieldName, out Exception eError)
        {
            eError = null;
            try
            {
                IWorkspaceFactory pWorkSpaceFactory = new AccessWorkspaceFactoryClass();
                IWorkspace pWorkSpace = null;
                IFeatureClass pFeatureClass = null;
                if (ModData.countyPath != "")
                {
                    pWorkSpace = pWorkSpaceFactory.OpenFromFile(wsPath, 0);
                    IFeatureWorkspace pFeatureWorkSpace = pWorkSpace as IFeatureWorkspace;
                    string rangeLayerName = GetMapFrameName(wsPath, pScale, out eError);
                    if (rangeLayerName == "" || eError != null)
                    {
                        return;
                    }
                    pFeatureClass = pFeatureWorkSpace.OpenFeatureClass(rangeLayerName);
                }
                if (pFeatureClass != null)
                {
                    IDataset pDataSet = pFeatureClass as IDataset;
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    pFeatureLayer.FeatureClass = pFeatureClass;
                    ILayer pLayer = pFeatureLayer as ILayer;
                    pLayer.Name = pDataSet.Name;
                    countySheetControl.AddLayer(pLayer);

                    //图层显示标注
                    SetLableToGeoFeatureLayer(pFeatureLayer as IGeoFeatureLayer, fieldName, Convert.ToInt32(pScale), countySheetControl.ReferenceScale);
                }
            }
            catch (Exception ex)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                //********************************************************************

                eError = ex;
                return;
            }
        }

        /// <summary>
        /// 设置属性标注
        /// </summary>
        /// <param name="pGeoFeatureLayer">图层</param>
        /// <param name="vLabelField">属性字段</param>
        /// <param name="vMapFrameScale">图层比例尺</param>
        /// <param name="vMapRefrenceScale">参考比例尺</param>
        private void SetLableToGeoFeatureLayer(IGeoFeatureLayer pGeoFeatureLayer, string vLabelField, int vMapFrameScale, double vMapRefrenceScale)
        {
            IAnnotateLayerPropertiesCollection pAnnoLayerProperCol = pGeoFeatureLayer.AnnotationProperties;
            IAnnotateLayerProperties pAnnoLayerProper;
            IElementCollection placedElements;
            IElementCollection unplacedElements;
            //得到当前层的当前标注属性
            pAnnoLayerProperCol.QueryItem(0, out pAnnoLayerProper, out placedElements, out unplacedElements);
            ILabelEngineLayerProperties pLabelEngineLayerProper = (ILabelEngineLayerProperties)pAnnoLayerProper;
            IBasicOverposterLayerProperties4 pBasicOverposterLayerProper = (IBasicOverposterLayerProperties4)pLabelEngineLayerProper.BasicOverposterLayerProperties;
            //标注的字体
            ITextSymbol pTextSymbol = pLabelEngineLayerProper.Symbol;
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.Red = 0;
            pRGBColor.Blue = 255;
            pRGBColor.Green = 0;
            pTextSymbol.Color = pRGBColor;
            stdole.StdFont pStdFont = new stdole.StdFontClass();
            IFontDisp pFont = (IFontDisp)pStdFont;
            pFont.Name = "宋体";
            if (vMapRefrenceScale != 0)
            {
                double size = (vMapFrameScale / 3) * vMapFrameScale / vMapRefrenceScale;
                pFont.Size = (decimal)size;
            }
            pTextSymbol.Font = pFont;
            //标注内容
            pLabelEngineLayerProper.Expression = "[" + vLabelField + "]";
            pBasicOverposterLayerProper.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerPart;
            //标注的方向信息
            pBasicOverposterLayerProper.PolygonPlacementMethod = esriOverposterPolygonPlacementMethod.esriAlwaysHorizontal;
            //标注的与几何图形的大小关系
            pBasicOverposterLayerProper.PlaceOnlyInsidePolygon = false;
            //开启标注
            pGeoFeatureLayer.DisplayAnnotation = true;
        }

        /// <summary>
        /// 判断是否存在着相应比利尺的图副
        /// </summary>
        /// <param name="MapPath">图副路径</param>
        /// <param name="StrScale">比例尺</param>
        /// <returns></returns>
        private string GetMapFrameName(string MapPath, string StrScale, out Exception eError)
        {
            eError = null;
            IWorkspaceFactory pWorkSpaceFactory = new AccessWorkspaceFactoryClass();
            IWorkspace pWorkSpace = null;
            IEnumDataset pEnumDataSet = null;
            IDataset pDataSet = null;
            IFeatureDataset pFeatureDataSet = null;
            IEnumDataset pEnumFC = null;
            IDataset pFC = null;
            string LayerName = "";
            string MapFrameName = "";
            if (MapPath != "")
            {
                pWorkSpace = pWorkSpaceFactory.OpenFromFile(MapPath, 0);
                pEnumDataSet = pWorkSpace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                pEnumDataSet.Reset();
                pDataSet = pEnumDataSet.Next();
                while (pDataSet != null)
                {
                    if (pDataSet is IFeatureClass)
                    {
                        LayerName = pDataSet.Name;
                        if (LayerName.Contains(StrScale))
                        {
                            MapFrameName = LayerName;
                        }
                    }
                    else if (pDataSet is IFeatureDataset)
                    {
                        pFeatureDataSet = pDataSet as IFeatureDataset;
                        pEnumFC = pFeatureDataSet.Subsets;
                        pEnumFC.Reset();
                        pFC = pEnumFC.Next();
                        while (pFC != null)
                        {
                            LayerName = pFC.Name;
                            if (LayerName.Contains(StrScale))
                            {
                                MapFrameName = LayerName;
                            }
                        }
                    }
                    else
                    {
                        MapFrameName = "";
                    }
                    pDataSet = pEnumDataSet.Next();
                }
                if (MapFrameName == "")
                {
                    eError = new Exception("未找到比例尺为'" + StrScale + "'的范围数据，请检查！");
                    return "";
                }
            }
            return MapFrameName;
        }
    }
}