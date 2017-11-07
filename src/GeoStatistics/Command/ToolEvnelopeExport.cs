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

namespace GeoStatistics.Command
{
    /// <summary>
    /// Summary description for ToolEvnelopeExport.
    /// </summary>
    [Guid("f9348c53-a9bd-47b8-a57b-435c3c3fba49")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoStatistics.Command.ToolEvnelopeExport")]
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
        private frmAreaStatistics frm=null;
        private IGeometry m_Polygon;
        private IMapControlDefault m_MapControl=null;
        private IActiveView m_pActiveView = null;
        private IActiveViewEvents_Event m_pActiveViewEvents;
        public ToolEvnelopeExport()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Statistics"; //localizable text 
            base.m_caption = "自定义范围面积统计";  //localizable text 
            base.m_message = "自定义范围面积统计";  //localizable text
            base.m_toolTip = "自定义范围面积统计";  //localizable text
            base.m_name = "ToolAreaStatistics";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
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

            m_hookHelper.Hook = hook;
            m_MapControl = hook as IMapControlDefault;
            m_pActiveView = m_hookHelper.FocusMap as IActiveView;
            m_pActiveViewEvents = m_pActiveView as IActiveViewEvents_Event;
            m_pActiveViewEvents.AfterDraw += new IActiveViewEvents_AfterDrawEventHandler(m_pActiveViewEvents_AfterDraw);
            // TODO:  Add ToolAreaStatistics.OnCreate implementation
        }

        
        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if(m_hookHelper==null) return;
            if (Button == 4||Button==2) return;
            IActiveView pAv=m_hookHelper.ActiveView;
            IMapControl2 pMapCtl=m_hookHelper.Hook as IMapControl2;

            IPolygon pPoly = new PolygonClass();
            IPointCollection polylinePointCollection = new PolygonClass();
            pPoly.SpatialReference = pMapCtl.SpatialReference;
            IEnvelope pGeo=pMapCtl.TrackRectangle();
            object missing = Type.Missing;
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(pGeo.XMin, pGeo.YMin);
            polylinePointCollection.AddPoint(pPoint, ref missing, ref missing);
            pPoint.PutCoords(pGeo.XMin, pGeo.YMax);
            polylinePointCollection.AddPoint(pPoint, ref missing, ref missing);
            pPoint.PutCoords(pGeo.XMax, pGeo.YMax);
            polylinePointCollection.AddPoint(pPoint, ref missing, ref missing);
            pPoint.PutCoords(pGeo.XMax, pGeo.YMin);
            polylinePointCollection.AddPoint(pPoint, ref missing, ref missing);
            pPoly = polylinePointCollection as IPolygon;
            pPoly.SimplifyPreserveFromTo();

            if (pPoly == null) return;
            //drawgeometryXOR(pGeo);
            frm = new frmAreaStatistics();
            pPoly.SpatialReference = pMapCtl.SpatialReference;  //added by chulili 2013-02-28 到这里为止，pPoly的空间参考变成null了，再赋值一次
            frm.CurGeometry = pPoly as IGeometry;
            frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);      
            frm.CurMap = m_hookHelper.Hook as IMapControlDefault;
            frm.InitFrm();
            frm.SetSliderValue(true);
            frm.ShowDialog();
        }
        private void drawgeometryXOR(IGeometry pPolygon)
        {
            //if (frm.IsDisposed)//如果窗体关闭或者取消 就不绘制 xisheng 2011.06.28
            //{
            //    return;
            //}

            //联动
            //this.sliderBuffer.Value = Convert.ToInt32(dblBuffLen.Text);
            //获得缓冲范围


            IScreenDisplay pScreenDisplay = m_pActiveView.ScreenDisplay;
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

                pScreenDisplay.StartDrawing(pScreenDisplay.hDC, -1);  //esriScreenCache.esriNoScreenCache -1
                pScreenDisplay.SetSymbol(pSymbol);

                //不存在已画出的多边形
                if (pPolygon != null)
                {
                    pScreenDisplay.DrawPolygon(pPolygon);
                  
                }
                pScreenDisplay.FinishDrawing();

            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制缓冲范围出错:" + ex.Message, "提示");
                pFillSymbol = null;
            }
        }
        //被擦去时画出
        private void m_pActiveViewEvents_AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            if (frm != null && !frm.IsDisposed)
            {
                drawgeometryXOR(null);
            }
        }

        //窗体关闭时 刷新前景
        private void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Polygon = null;

            m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolAreaStatistics.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolAreaStatistics.OnMouseUp implementation
        }
        #endregion
    }
}
