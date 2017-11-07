using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

namespace GeoDataExport.Commands
{
    /// <summary>
    /// Summary description for ToolEvnelopeExport.
    /// </summary>
    [Guid("f9348c53-a9bd-47b8-a57b-435c3c3fba49")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoDataExport.Commands.ToolEvnelopeExport")]
    public sealed class ToolEvnelopeExport : BaseTool
    {
        Plugin.Application.IAppFormRef m_pAppForm = null;
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

        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;
        //为绘制导入的范围
        //private IScreenDisplay m_pScreenDisplay;
        ////private IActiveViewEvents_Event m_pActiveViewEvents;
        //private IActiveView m_pActiveView = null;
        private IGeometry m_Polygon;
        //为绘制导入的范围
        GeoDataExport.frmExport frm = null;
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

        public ToolEvnelopeExport()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "数据提取"; //localizable text 
            base.m_caption = "矩形范围提取";  //localizable text 
            base.m_message = "矩形范围提取";  //localizable text
            base.m_toolTip = "矩形范围提取";  //localizable text
            base.m_name = "ToolEvnelopeExport";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();
            m_pAppForm = hook as Plugin.Application.IAppFormRef;
            Plugin.Application.IAppGisUpdateRef appHK=hook as Plugin.Application.IAppGisUpdateRef;
            m_hookHelper.Hook = appHK.MapControl.Object;
            m_MapControl = appHK.MapControl;
            //m_pActiveView = m_hookHelper.FocusMap as IActiveView;
           
            //m_pActiveViewEvents = m_pActiveView as IActiveViewEvents_Event;
            //m_pScreenDisplay = m_pActiveView.ScreenDisplay;
            
            //try
            //{
            //    m_pActiveViewEvents.AfterDraw += new IActiveViewEvents_AfterDrawEventHandler(m_pActiveViewEvents_AfterDraw);

            //}
            //catch
            //{
            //}
            // TODO:  Add ToolEvnelopeExport.OnCreate implementation
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ToolEvnelopeExport.OnClick implementation
        }

        private double GetArea(IGeometry pGeometry, IMap pMap)
        {
            IClone pClone = pGeometry as IClone;
            IGeometry pNewGeo = pClone.Clone() as IGeometry;
            IArea pArea = pNewGeo as IArea;
            double area = pArea.Area;
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
                    ////转换如果是经纬度的地图 xisheng 20110731
                    //UnitConverter punitConverter = new UnitConverterClass();
                    //area = punitConverter.ConvertUnits(Math.Abs(area), esriUnits.esriMeters, esriUnits.esriDecimalDegrees);
                    //上面转换方法适合长度不适合面积 修改 xisheng 20111123

                    ISpatialReference pOldSpatial = pMap.SpatialReference;
                    //做投影变换
                    if (!(pOldSpatial is IProjectedCoordinateSystem))
                    {
                        pNewGeo.SpatialReference = pOldSpatial;
                        //获得坐标系
                        ISpatialReference pNewSpatial = SysCommon.Gis.ModGisPub.GetSpatialByX((pNewGeo.Envelope.XMin + pNewGeo.Envelope.XMax) / 2);
                        if (pNewSpatial != null) pNewGeo.Project(pNewSpatial);
                        IArea parea = pNewGeo as IArea;
                        area = parea.Area;
                    }

                    break;
                default:
                    area = 0;
                    break;
            }
            return area;

        }


        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (m_hookHelper.Hook == null) return;
            IMapControl2 pMapCtl = m_hookHelper.Hook as IMapControl2;

            ESRI.ArcGIS.Geometry.IEnvelope pGeometry = pMapCtl.TrackRectangle();
            if (pGeometry == null) return;
            ESRI.ArcGIS.Carto.IMap pMap = m_hookHelper.FocusMap;
            
          //更改获取面积的方法，20111123
            double area = GetArea(pGeometry, pMap);
            double dArea = SysCommon.ModSysSetting.GetExportAreaOfUser(Plugin.ModuleCommon.TmpWorkSpace, m_pAppForm.ConnUser);
           
            if (dArea >= 0 && area > dArea)
            {
                MessageBox.Show("超过提取最大面积", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           
            drawgeometryXOR(pGeometry as IGeometry);
            
            frm = new frmExport(pMap, pGeometry);
            frm.WriteLog = WriteLog; //ygc 2012-9-12 是否写日志
            frm.m_area = area;
            //ZQ 2011 1126 modify
            SysCommon.ScreenDraw.list.Add(ToolEvnelopeExportAfterDraw);
            frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
            frm.ShowDialog();
           
        }
        //窗体关闭时 刷新前景
        private void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Polygon = null;
            SysCommon.ScreenDraw.list.Remove(ToolEvnelopeExportAfterDraw);
            m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolEvnelopeExport.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolEvnelopeExport.OnMouseUp implementation
        }
        #endregion
        /// ZQ 2011 1128 modify
        //擦去重绘
        internal void ToolEvnelopeExportAfterDraw(IDisplay Display, esriViewDrawPhase phase)
        //private void m_pActiveViewEvents_AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            if (frm != null && !frm.IsDisposed)
            {
                drawgeometryXOR(null);
            }

        }
        /// <summary>
        /// ZQ 2011 1129  modify
        /// </summary>
        /// <param name="pPolygon"></param>
        private void drawgeometryXOR(IGeometry pPolygon)
        {

            //联动
            //this.sliderBuffer.Value = Convert.ToInt32(dblBuffLen.Text);
            //获得缓冲范围

            IScreenDisplay pScreenDisplay = m_MapControl.ActiveView.ScreenDisplay;
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

                pLineSymbol.Width = 1.0;
                pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = pLineSymbol;

                pFillSymbol.Color = pRGBColor;
                pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;

                pScreenDisplay.StartDrawing(pScreenDisplay.hDC, -1);  //esriScreenCache.esriNoScreenCache -1
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
                MessageBox.Show("绘制缓冲范围出错:" + ex.Message, "提示");
                pFillSymbol = null;
            }
        }
    }
}
