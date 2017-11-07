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


namespace GeoPageLayout
{
    /// <summary>
    /// Summary description for CommandSelOutmap.
    /// </summary>
    [Guid("6AC8466F-294E-438F-9770-33D889A1C713")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoPageLayout.CommandRectangleOutmap")]
    public sealed class CommandRectangleOutmap : BaseTool
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
        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
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
        private ESRI.ArcGIS.Geometry.IGeometry pGeometry;
        private IPageLayoutControl2 pLCCtl;
        private IEnvelope pExtent;//mapframe初始大小
        //为绘制导入的范围
        private IScreenDisplay m_pScreenDisplay;
        private IActiveViewEvents_Event m_pActiveViewEvents;
        private IActiveView m_pActiveView = null;
        private IEnvelope m_Polygon;
        //为绘制导入的范围
        FrmPageLayout frm = null;
        IMapControl2 pMapCtl;
        public CommandRectangleOutmap()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "GeoPageLayout_CommandRectangleOutmap";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            //pExtent = pEnv;
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                System.IO.Stream strm = GetType().Assembly.GetManifestResourceStream("GeoPageLayout.Command.CommandPolygonOutmap.cur");
                base.m_cursor = new System.Windows.Forms.Cursor(strm);//(GetType(), GetType().Name + ".cur");
                strm.Close();
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
            pMapCtl = m_hookHelper.Hook as IMapControl2;
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
            // TODO:  Add ToolEvnelopeExport.OnCreate implementation
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ToolEvnelopeExport.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (m_hookHelper.Hook == null) return;
            IMapControl2 pMapCtl = m_hookHelper.Hook as IMapControl2;

            ESRI.ArcGIS.Geometry.IGeometry pGeometry = pMapCtl.TrackRectangle();
            if (pGeometry == null) 
                return;
            if (pGeometry.GeometryType != esriGeometryType.esriGeometryEnvelope) 
                return;
            if (pGeometry.Envelope.Width < 0)
                return;
            //pLCCtl.Page.FormID = esriPageFormID.esriPageFormA4;
            //IMapFrame pMapFrame = (IMapFrame)pLCCtl.GraphicsContainer.FindFrame(pLCCtl.ActiveView.FocusMap);

            //IElement pMapEle = pMapFrame as IElement;
            //pMapEle.Geometry = pExtent as IGeometry;


            //pLCCtl.ZoomToWholePage();
            //FrmScale pfrmScale = new FrmScale();
            //if (pfrmScale.ShowDialog() != DialogResult.OK)
            //    return;

            //createPageLayout_Rectangle(pfrmScale.ScaleSet);
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("矩形范围制图");//xisheng 日志记录07.08
            }
            drawgeometryXOR(pGeometry as IEnvelope, m_pScreenDisplay);
            ESRI.ArcGIS.Carto.IMap pMap = m_hookHelper.FocusMap;
            SysCommon.CProgress pgss = new SysCommon.CProgress("正在加载制图界面，请稍候...");
            pgss.EnableCancel = false;
            pgss.ShowDescription = false;
            pgss.FakeProgress = true;
            pgss.TopMost = true;
            pgss.ShowProgress();
            Application.DoEvents();
            frm = new FrmPageLayout(m_hookHelper.FocusMap, pGeometry);
            frm.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
            frm.typeZHT = 2;
            frm.Show();
            pgss.Close();
            pMapCtl.CurrentTool = null;
            Application.DoEvents();
        }
        //窗体关闭时 刷新前景
        private void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Polygon = null;
            m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
        }
        //矩形范围制图
        //void createPageLayout_Rectangle()
        //{


        //    double width = 0, height = 0;
           
        //    IEnvelope pEnv = pGeometry.Envelope;

        //    //pLCCtl.PageLayout = cMD.PageLayout;

        //    width = pEnv.Width * 100 / cScale;
        //    height = pEnv.Height * 100 / cScale;
        //    pLCCtl.Page.PutCustomSize(pEnv.Width * 100 / cScale + 15, pEnv.Height * 100 / cScale + 15);//centimeter





        //    IMapFrame pMapFrame = (IMapFrame)pLCCtl.GraphicsContainer.FindFrame(pLCCtl.ActiveView.FocusMap);

        //    IElement pMapEle = pMapFrame as IElement;
        //    (pMapEle as IElementProperties).Name = "地图";
        //    IEnvelope mapframebound = new EnvelopeClass();
        //    mapframebound.PutCoords(5, 5, 5 + width, 5 + height);

        //    //IPointCollection pPc = new PolygonClass();
        //    //object missing = Type.Missing;
        //    //pPc.AddPoint(XYtoP(5, 5), ref missing, ref missing);
        //    //pPc.AddPoint(XYtoP(5 + width , 5), ref missing, ref missing);
        //    //pPc.AddPoint(XYtoP(5 + height, 5 + pEnv.Height / 100), ref missing, ref missing);
        //    //pPc.AddPoint(XYtoP(5, 5 + pEnv.Height / 100), ref missing, ref missing);
        //    pMapEle.Geometry = mapframebound as IGeometry;
        //    //delete preadded element
        //    this.pLCCtl.GraphicsContainer.Reset();
        //    IElement plele = this.pLCCtl.GraphicsContainer.Next();

        //    while (plele != null)
        //    {
        //        if ((plele as IElementProperties3).Name != "地图") //i++;
        //        {
        //            this.pLCCtl.GraphicsContainer.DeleteElement(plele);
        //            this.pLCCtl.GraphicsContainer.Reset();
        //        }
        //        plele = this.pLCCtl.GraphicsContainer.Next();

        //    }
        //    //pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentScale;//fixed scale
        //    //pMapFrame.MapScale = cScale;
        //    //pEnva.Width = pEnv.Width / 500000;
        //    //pEnva.Height = pEnv.Height / 500000;

        //    //frameProperties.Geometry = pEnv as IPolygon;


        //    //pLCCtl.ActiveView.Extent = pLCCtl.FullExtent;


        //    //CreateMeasuredGrid();
        //    ////addCornerCoor();
        //    //addText();


        //    //AddLegend(pLCCtl.PageLayout, pLCCtl.ActiveView.FocusMap, 5 + width + 1, 5 + height, 4);
        //    //AddNorthArrow(pLCCtl.PageLayout, pLCCtl.ActiveView.FocusMap);
        //    //AddScalebar(pLCCtl.PageLayout, pLCCtl.ActiveView.FocusMap);

        //    pLCCtl.ZoomToWholePage();

        //    //axPageLayoutControl1.Refresh();


        //    //axPageLayoutControl1.PageLayout.ReplaceMaps(pMaps);
        //    //axPageLayoutControl1.ActiveView.Activate(axPageLayoutControl1.hWnd);
        //    //axPageLayoutControl1.ActiveView.FocusMap = pMaps.get_Item(0);
        //    //axPageLayoutControl1.ActiveView.Refresh();
        //    //cMD.Save(true, false);


        //}


        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolEvnelopeExport.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolEvnelopeExport.OnMouseUp implementation
        }
        #endregion

        //擦去重绘
        private void m_pActiveViewEvents_AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            if (frm != null && !frm.IsDisposed)
            {
                drawgeometryXOR(null, m_pScreenDisplay);
            }

        }
        //绘制导入的范围
        private void drawgeometryXOR(IEnvelope pPolygon, IScreenDisplay pScreenDisplay)
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
                    pScreenDisplay.DrawRectangle(pPolygon);
                    m_Polygon = pPolygon;
                }
                //存在已画出的多边形
                else
                {
                    if (m_Polygon != null)
                    {
                        pScreenDisplay.DrawRectangle(m_Polygon);
                    }
                }

                pScreenDisplay.FinishDrawing();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示");
                pFillSymbol = null;
            }
        }


        
    }
}
