using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using System.Collections.Generic;
using ESRI.ArcGIS.Carto;

namespace GeoDataEdit.Tools
{
    /// <summary>
    /// Summary description for ToolDrawPolygons.
    /// </summary>
    [Guid("543d2c17-f0af-423e-97f0-c1c54e2ced97")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoDataEdit.Tools.ToolDrawPolygons")]
    public sealed class ToolDrawPolygons : BaseTool
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
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        public static List<IGeometry> ListGeometrys = new List<IGeometry>();
        private IMapControlDefault m_MapControl;
        IActiveView m_pActiveView = null;
        IActiveViewEvents_Event m_pActiveViewEvents = null;

        public static string strType = "Polygon";

        public ToolDrawPolygons()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "范围描绘"; //localizable text 
            base.m_caption = "范围描绘";  //localizable text 
            base.m_message = "范围描绘";  //localizable text
            base.m_toolTip = "范围描绘";  //localizable text
            base.m_name = "ToolDrawPolygons";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
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

            //m_pAppForm = hook as Plugin.Application.IAppFormRef;
            Plugin.Application.IAppGisUpdateRef appHK = hook as Plugin.Application.IAppGisUpdateRef;
            m_hookHelper.Hook = appHK.MapControl.Object;
            m_MapControl = appHK.MapControl;

            m_pActiveView = m_hookHelper.FocusMap as IActiveView; 
            m_pActiveViewEvents = m_pActiveView as IActiveViewEvents_Event;
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
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ToolDrawPolygons.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (m_hookHelper.Hook == null) return;
            IMapControl2 pMapCtl = m_hookHelper.Hook as IMapControl2;
            ESRI.ArcGIS.Geometry.IGeometry pGeometry = null;
            if (strType.Trim() == "Polygon")
            {
                pGeometry = pMapCtl.TrackPolygon();
            }
            else
            {
                pGeometry = pMapCtl.TrackLine();
            }
            if (pGeometry == null) return;
            ListGeometrys.Add(pGeometry);
            drawgeometryXOR(pGeometry);
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
           
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolDrawPolygons.OnMouseUp implementation
        }
        #endregion

        private void m_pActiveViewEvents_AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            if (phase == esriViewDrawPhase.esriViewForeground)
            {
                drawgeometryXOR(null);
            }
        }
        
        /// <summary>
        /// ZQ 2011 1129  modify
        /// </summary>
        /// <param name="pPolygon"></param>
        public  void drawgeometryXOR(IGeometry geo)
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
                pFillSymbol.Style = esriSimpleFillStyle.esriSFSNull;

                pScreenDisplay.StartDrawing(pScreenDisplay.hDC, -1);  //esriScreenCache.esriNoScreenCache -1
                pScreenDisplay.SetSymbol(pSymbol);

                ////不存在已画出的多边形
                //if (pPolygon != null)
                //{
                //    pScreenDisplay.DrawPolygon(pPolygon);
                //    m_Polygon = pPolygon;
                //}
                ////存在已画出的多边形
                //else
                //{
                //    if (m_Polygon != null)
                //    {
                //        pScreenDisplay.DrawPolygon(m_Polygon);
                //    }
                //}
                //foreach (IGeometry geo in ListGeometrys)
                //{
                //    pScreenDisplay.DrawPolygon(geo);
                //}

                if (geo == null)
                {
                    foreach (IGeometry geo2 in ListGeometrys)
                    {
                        pScreenDisplay.DrawPolygon(geo2);
                    }
                }
                else
                {
                    pScreenDisplay.DrawPolygon(geo);
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
