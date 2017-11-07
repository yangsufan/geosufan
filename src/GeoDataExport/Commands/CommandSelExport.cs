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
using ESRI.ArcGIS.Display;
using System.Windows.Forms;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;

namespace GeoDataExport.Commands
{
    /// <summary>
    /// Summary description for CommandSelExport.
    /// </summary>
    [Guid("8990ccb1-7015-4d9e-9534-491b6ec7f7df")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoDataExport.Commands.CommandSelExport")]
    public sealed class CommandSelExport : BaseCommand
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
        Plugin.Application.IAppFormRef m_pAppForm = null;
        private IHookHelper m_hookHelper;
        //为绘制导入的范围
        private IScreenDisplay m_pScreenDisplay;
        private IActiveViewEvents_Event m_pActiveViewEvents;
        private IActiveView m_pActiveView = null;
        private IPolygon m_Polygon;
        //为绘制导入的范围
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

        GeoDataExport.frmExport frm = null;
        public CommandSelExport()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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

        private void GetArea(ref double area, IMap pMap)
        {
            switch (pMap.MapUnits)
            {
                case esriUnits.esriKilometers:
                    area = (Math.Abs(area)) * 1000000;
                    break;
                case esriUnits.esriMeters:
                case esriUnits.esriUnknownUnits:
                    area = Math.Abs(area);
                    break;
                case esriUnits.esriDecimalDegrees:
                    //转换如果是经纬度的地图 xisheng 20110731
                    UnitConverter punitConverter = new UnitConverterClass();
                    area = punitConverter.ConvertUnits(Math.Abs(area), esriUnits.esriMeters, esriUnits.esriDecimalDegrees);
                    break;
                default:
                    area = 0;
                    break;
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

            m_pAppForm = hook as Plugin.Application.IAppFormRef;
            Plugin.Application.IAppGisUpdateRef appHK = hook as Plugin.Application.IAppGisUpdateRef;
            m_hookHelper.Hook = appHK.MapControl.Object;
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

            List<IGeometry> vTemp = GetDataGeometry(m_hookHelper.FocusMap);
            IMap pMap = m_hookHelper.FocusMap;
            if (vTemp == null) return;
            ESRI.ArcGIS.Geometry.IPolygon pGeometry = GetUnion(vTemp) as IPolygon;

            if (pGeometry == null) return;
            IArea pArea = pGeometry as IArea;
            double area = pArea.Area;
            GetArea(ref area, pMap);
            //ygc 2012-10-15 屏蔽面积判断
            //double dArea = SysCommon.ModSysSetting.GetExportAreaOfUser(Plugin.ModuleCommon.TmpWorkSpace, m_pAppForm.ConnUser);
            //if (dArea >= 0 && area > dArea)
            //{
            //    MessageBox.Show("超过提取最大面积", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //end
            //ICommand pCmd = new ControlsClearSelectionCommandClass();
            //pCmd.OnCreate(m_hookHelper.Hook);
            //pCmd.OnClick();
            pMap.ClearSelection();
            m_pActiveView.Refresh();
            Application.DoEvents();
            drawgeometryXOR(pGeometry, m_pScreenDisplay);
            
           
            frm = new frmExport(pMap, pGeometry);
            frm.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            frm.m_area = area;
            frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
            frm.ShowDialog();
            Application.DoEvents();
        }
        //窗体关闭时 刷新前景
        private void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Polygon = null;
            m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
        }

        #endregion


        private List<ESRI.ArcGIS.Geometry.IGeometry> GetDataGeometry(IMap pMap)
        {
            List<IGeometry> lstGeometrys = new List<IGeometry>();

            //IGeometry pTempGeo = null;
            if (pMap.SelectionCount < 1)
            {
                System.Windows.Forms.MessageBox.Show("请使用地图工具条中的【选择要素】工具在当前地图中选择要素！然后方可该功能。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            IActiveView pAv = pMap as IActiveView;
            ISelection pSelection = pMap.FeatureSelection;
            if (pSelection == null) return null;

            IEnumFeature pEnumFea = pSelection as IEnumFeature;
            IFeature pFea = pEnumFea.Next();
            while (pFea != null)
            {
                if (pFea.ShapeCopy != null)
                {
                    if (pFea.Shape.GeometryType != esriGeometryType.esriGeometryPolygon)
                    {
                        pFea = pEnumFea.Next();//while循环，continue的时候千万记得 后移！
                        continue;
                    }
                    lstGeometrys.Add(pFea.ShapeCopy);
                }
                pFea = pEnumFea.Next();
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
            pSI.Invalidate();//建立索引
            ITopologicalOperator pTopo = new PolygonClass();
            pTopo.ConstructUnion(pGeoBag as IEnumGeometry);
            IGeometry pGeo = pTopo as IGeometry;

            return pGeo;
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
        private void drawgeometryXOR(IPolygon pPolygon, IScreenDisplay pScreenDisplay)
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
