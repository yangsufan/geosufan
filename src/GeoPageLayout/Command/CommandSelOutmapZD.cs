using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using SysCommon.Error;
using ESRI.ArcGIS.esriSystem;

namespace GeoPageLayout
{
    /// <summary>
    /// Summary description for CommandSelOutmap.
    /// </summary>
    [Guid("2F38AC78-A458-4a28-A729-19AA286DF90E")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoPageLayout.CommandSelOutmapZD")]
    public sealed class CommandSelOutmapZD : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion
        private bool _Writelog = true;  //added by chulili 2012-09-10 山西支持“是否写日志”属性
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }

        private IHookHelper m_hookHelper;
        //为绘制导入的范围
        private IScreenDisplay m_pScreenDisplay;
        private IActiveViewEvents_Event m_pActiveViewEvents;
        private IActiveView m_pActiveView = null;
        private IGeometry m_Polygon;
        //为绘制导入的范围
        FrmPageLayout frm = null;
        private IFeature pZD = null;
        public CommandSelOutmapZD()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "GeoPageLayout_CommandSelOutmap";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
            IMapControl2 pMapCtl = m_hookHelper.Hook as IMapControl2;
            pMapCtl.CurrentTool = null;
            m_pActiveView = m_hookHelper.FocusMap as IActiveView;

            m_pActiveViewEvents = m_pActiveView as IActiveViewEvents_Event;
            m_pScreenDisplay = m_pActiveView.ScreenDisplay;
            try
            {
                m_pActiveViewEvents.AfterDraw += new IActiveViewEvents_AfterDrawEventHandler(m_pActiveViewEvents_AfterDraw);

            }
            catch
            {
            }
            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_hookHelper.Hook == null) return;
            //IFeatureLayer tmpFeatureLayer = layerCurSeleted();
            //if (tmpFeatureLayer == null)
            //{
            //    MessageBox.Show("请在地图目录设置当前选择图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            try
            {
                IMap pMap = null;
                bool isSpecial = ModGetData.IsMapSpecial();

                if (isSpecial)//如果找特定专题
                {
                    pMap = new MapClass();
                    pMap.SpatialReference = m_hookHelper.FocusMap.SpatialReference;
                    ModGetData.AddMapOfNoneXZQ(pMap, "CZDJ", m_hookHelper.FocusMap);
                    ModGetData.AddMapOfNoneXZQ(pMap, "DLGK", m_hookHelper.FocusMap);
                    if (pMap.LayerCount == 0)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到图层。");
                        return;
                    }
                    ModuleMap.LayersComposeEx(pMap);//图层排序
                }
                else
                {
                    IObjectCopy pOC = new ObjectCopyClass();
                    pMap = pOC.Copy(m_hookHelper.FocusMap) as IMap;//复制地图
                }
                pMap.Name = "宗地图";
                ILayer ZD = getLayer(pMap, "CZDJ_ZD");
                ILayer JZD = getLayer(pMap, "CZDJ_JZD");
                ILayer JZX = getLayer(pMap, "CZDJ_JZX");
              
                //IMap JZDXmap = new MapClass();//界址点线地图
                //pMap.DeleteLayer(JZD);
                //pMap.DeleteLayer(JZX);
                //JZDXmap.AddLayer(JZX);
                //JZDXmap.AddLayer(JZD);
                //JZDXmap.Name ="界址数据";
                //IMaps newMaps = new Maps();
                //newMaps.Add(pMap);
                //newMaps.Add(JZDXmap);
                IFeatureLayer pFL=ZD as IFeatureLayer;
                if(pFL==null || pFL.FeatureClass==null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到图层！请加载宗地图层。");
                    return;
                }
                //获得选择要素的图形
                List<IGeometry> vTemp = GetDataGeometry(ZD as IFeatureLayer);
                if (vTemp == null||vTemp.Count==0) return;
            
                IFeatureClass zdFC=pFL.FeatureClass;
                string OID=zdFC.OIDFieldName;
                IFeatureLayerDefinition pFLD = pFL as IFeatureLayerDefinition;
                pFLD.DefinitionExpression = OID+" = "+pZD.OID;
                delSelOfLyr(ZD);
                pMap.ClearSelection();//清楚选择集

                try//空间过滤显示
                {
                    filterLyrBySpatial(JZD, pZD.ShapeCopy);
                    filterLyrBySpatial(JZX, pZD.ShapeCopy);

                }
                catch
                { }
                //if (m_hookHelper.FocusMap.SelectionCount > 100)
                //int ooo = m_hookHelper.FocusMap.SelectionCount;
               
                SysCommon.CProgress pgss = new SysCommon.CProgress("正在加载制图界面，请稍候...");
                pgss.EnableCancel = false;
                pgss.ShowDescription = false;
                pgss.FakeProgress = true;
                pgss.TopMost = true;
                pgss.ShowProgress();
                
                //ITopologicalOperator pTO = GetUnion(vTemp) as ITopologicalOperator;

                ESRI.ArcGIS.Geometry.IGeometry pGeometry = GetUnion(vTemp);
                IEnvelope extent = pGeometry.Envelope;
                //if (extent.Width > extent.Height)
                //extent.Expand((2 *extent.Width*0.8)/extent.Height, 2, false);
                extent.Expand(2, 2, true);//出图范围暂设为宗地的2倍
                //m_hookHelper.ActiveView.Extent=pGeometry.Envelope;
                //m_hookHelper.ActiveView.Refresh();
                pgss.Close();
                Application.DoEvents();
                drawgeometryXOR(extent, m_pScreenDisplay);
                
                frm = new FrmPageLayout(pMap, extent, pZD);
                frm.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
                frm.typeZHT = 2;
                frm.Show();
                
                Application.DoEvents();
            }
            catch (Exception exError)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", exError.Message);
            }
        }
        //空间过滤图层的要素显示
        private void filterLyrBySpatial(ILayer inLayer,IGeometry inGeometry)
        {
            IFeatureLayer pFL = inLayer as IFeatureLayer;
            if (pFL == null)
                return;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = (inGeometry as ITopologicalOperator).Boundary;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            //pSF.SpatialRelDescription = "****T****";
            IFeatureCursor pFC = pFL.Search(pSF, false);
            if (pFC == null)
                return;
            string lyrDef = pFL.FeatureClass.OIDFieldName + " IN(";
            IFeature pFeature = pFC.NextFeature();
            while (pFeature != null)
            {

                lyrDef += pFeature.OID.ToString() + ",";

                pFeature = pFC.NextFeature();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFC);
            pFC = null;
            if (lyrDef.EndsWith("IN("))
                return;
            lyrDef = lyrDef.Substring(0, lyrDef.Length - 1);
            lyrDef += ")";
            
            IFeatureLayerDefinition pFLD = pFL as IFeatureLayerDefinition;
            pFLD.DefinitionExpression = lyrDef;
        }
        //设置选择集的符号
        private void setLyrSelSym(ILayer inLyr)
        {
            IFeatureLayer pFL = inLyr as IFeatureLayer;
            if (pFL == null)
                return;
            IFeatureSelection pFS = pFL as IFeatureSelection;
            pFS.SetSelectionSymbol = false;
            ISymbol pSymbol = pFS.SelectionSymbol;
            IFillSymbol pFillSym = pSymbol as IFillSymbol;
            if (pFillSym == null)
                return;
            IRgbColor pColor=new RgbColorClass();
            pColor.Red=255;
            pColor.Blue=0;
            pColor.Green=0;
            pFillSym.Color = pColor as IColor;

        }
        //去除选择集
        private void delSelOfLyr(ILayer inLyr)
        {
            IFeatureLayer pFL = inLyr as IFeatureLayer;
            if (pFL == null)
                return;
            IFeatureSelection pFS = pFL as IFeatureSelection;
            pFS.Clear();

        }
        //窗体关闭时 刷新前景
        private void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Polygon = null;
            m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
        }
        #endregion


        private List<ESRI.ArcGIS.Geometry.IGeometry> GetDataGeometry(IFeatureLayer pFL)
        {
            IFeatureSelection pFeatureSelection = pFL as IFeatureSelection;
            List<IGeometry> lstGeometrys = new List<IGeometry>();
            
            //IGeometry pTempGeo = null;
            if (pFeatureSelection.SelectionSet.Count< 1)
            {
                System.Windows.Forms.MessageBox.Show("请使用地图工具条中的【选择要素】工具在当前地图中选择要素！然后方可该功能。","系统提示",MessageBoxButtons.OK ,MessageBoxIcon.Information );
                return null;
            }
            if (pFeatureSelection.SelectionSet.Count > 1)
            {
                System.Windows.Forms.MessageBox.Show("当前地图选择数量大于1！请在宗地图层选择1个宗地！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            IFeature pFea = pFL.FeatureClass.GetFeature(pFeatureSelection.SelectionSet.IDs.Next());
            if (pFea != null)
            {
                if (pFea.ShapeCopy != null)
                {
                    pZD = pFea;//选择的宗地
                    lstGeometrys.Add(pFea.ShapeCopy);
                }
                
            }
            return lstGeometrys;
        }

        /// <summary>
        /// union几个面要素
        /// </summary>
        /// <param name="lstGeometry">需要操作的面要素集合</param>
        /// <returns>返回union后的图形</returns>
        public static IGeometry GetUnion(List<IGeometry> lstGeometry)
        {
            IGeometryBag pGeoBag = new GeometryBagClass();
            IGeometryCollection pGeoCol = pGeoBag as IGeometryCollection;
           
            if (lstGeometry.Count < 1) return null;
            if (lstGeometry[0].SpatialReference != null)
            {
                pGeoBag.SpatialReference = lstGeometry[0].SpatialReference;
            }

            object obj = System.Type.Missing;
            for (int i = 0; i < lstGeometry.Count; i++)
            {
                IGeometry pTempGeo = lstGeometry[i];
                pGeoCol.AddGeometry(pTempGeo, ref obj, ref obj);
            }
            ISpatialIndex pSI = pGeoBag as ISpatialIndex;
            pSI.AllowIndexing = true;
            pSI.Invalidate();

            ITopologicalOperator pTopo = new PolygonClass();
            pTopo.ConstructUnion(pGeoBag as IEnumGeometry);
            IGeometry pGeo = pTopo as IGeometry;

            return pGeo;
        }
        private ILayer getLayer(ILayer inLayer,string fcName)
        {
             ILayer res=null;
             if (inLayer is IGroupLayer)//如果是组图层，递归
             {
                 ICompositeLayer pCL = inLayer as ICompositeLayer;
                 for (int i = 0; i < pCL.Count; i++)
                 {
                     res = getLayer(pCL.get_Layer(i), fcName);
                     if (res != null)
                         break;

                 }
             }
             else
             {
                 IFeatureLayer pFeaLyr = inLayer as IFeatureLayer;
                 if (pFeaLyr == null)
                     return null;
                 IDataset pDS = pFeaLyr.FeatureClass as IDataset;
                 if (pDS == null)
                     return null;
                 if (pDS.Name.Contains(fcName))
                     res = inLayer;
             }
             return res;

        }
        //获得指定要素类名字的图层
        private ILayer getLayer(IMap pMap, string fcName)
        {
            ILayer res = null;
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                res = getLayer(pMap.get_Layer(i), fcName);
                if (res != null)
                    break;
            }
            return res;
        }
        //擦去重绘
        private void m_pActiveViewEvents_AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            if (frm != null && !frm.IsDisposed)
            {
                drawgeometryXOR(null, m_pScreenDisplay);
            }

        }
        //绘制导入的范围
        private void drawgeometryXOR(IGeometry pPolygon, IScreenDisplay pScreenDisplay)
        {
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

            try
            {
                //颜色对象
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                ISymbol pSymbol = (ISymbol)pFillSymbol;
                pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                pRGBColor.Red = 255;
                pRGBColor.Green = 170;
                pRGBColor.Blue = 0;
                pLineSymbol.Color = pRGBColor;

                pLineSymbol.Width = 0.8;
                pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = pLineSymbol;

                pFillSymbol.Color = pRGBColor;
                pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;

                pScreenDisplay.StartDrawing(m_pScreenDisplay.hDC, -1);  //esriScreenCache.esriNoScreenCache -1
                pScreenDisplay.SetSymbol(pSymbol);

                //不存在已画出的多边形
                if (pPolygon != null)
                {
                    pScreenDisplay.DrawPolygon(pPolygon);
                    m_Polygon = pPolygon;
                }
                //存在已画出的多边形
                else
                {
                    if (m_Polygon != null)
                    {
                        pScreenDisplay.DrawPolygon(m_Polygon);
                    }
                }

                pScreenDisplay.FinishDrawing();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pFillSymbol = null;
            }
        }
    }
}
