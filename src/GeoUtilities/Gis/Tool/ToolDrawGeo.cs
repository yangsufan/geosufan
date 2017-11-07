using System;
using System.Drawing;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;

namespace GeoUtilities
{
    public delegate void myEventHandler(bool BeginDraw);//委托事件
    /// <summary>
    /// 在地图上绘制点、线、面要素
    /// </summary>
    [Guid("8dfd5318-dccf-4f6a-800a-d231bcbc5b26")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoUtilities.Gis.Tool.ToolDrawGeo")]
    public sealed class ToolDrawGeo : BaseTool
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
        //public event myEventHandler BeginDrawed;//定义事件
        public event myEventHandler EndDtrawd;//定义结束事件
        //绘制对象点集合
        private ESRI.ArcGIS.Geometry.IPointCollection m_pPointColl;
        //最终生成的集合对象
        public IGeometry m_Geometry;
        private const string sPolyOutlineName = "_POLYOUTLINE_";
        //绘制对象的类型 目前只是提供点、线、面的绘制
        ESRI.ArcGIS.Geometry.esriGeometryType m_DrawType;

        public ESRI.ArcGIS.Geometry.esriGeometryType GeometryType
        {
            set
            {
                m_DrawType = value;
            }
        }
        //绘制三维要素所在的基准面
        ISurface m_psurface = null;
        public ISurface pSurface
        {
            set
            {
                m_psurface = value;
            }
        }

        public ToolDrawGeo()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
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
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;  
            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ToolDrawGeo.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            try
            {
                ESRI.ArcGIS.Geometry.IPoint pMapPoint = new ESRI.ArcGIS.Geometry.PointClass(); ;
                object before = Type.Missing;
                object after = Type.Missing;
                object StepSize = Type.Missing;
                m_hookHelper.FocusMap.SpatialReference = m_psurface.Domain.SpatialReference;
                pMapPoint = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X,Y);
                if (pMapPoint == null)
                {
                    return;
                }
                pMapPoint.Project(m_psurface.Domain.SpatialReference);
                pMapPoint.Z = m_psurface.GetElevation(pMapPoint);
            
                //pMapPoint.SpatialReference = m_hookHelper.FocusMap.SpatialReference;
                //pDisplay = m_sceneHookHelper.SceneGraph as IDisplay3D;
                //pDisplay.FlashLocation(pMapPoint);//闪烁显示被点击的位置
                
                IGeometry pGeom = null;
                ClsMarkDraw.DeleteAllElementsWithName(m_hookHelper.FocusMap, sPolyOutlineName);
                //根据绘制对象类型的不同定义不同的类型
                switch (m_DrawType.ToString())
                {
                    case "esriGeometryPoint":
                        m_Geometry = pMapPoint;
                        break;
                    case "esriGeometryLine":
                        if (m_pPointColl == null)
                        {
                            m_pPointColl = new PolylineClass();
                            pGeom = new PolylineClass();
                        }
                        m_pPointColl.AddPoint(pMapPoint, ref before, ref after);
                        break;
                    case "esriGeometryPolygon":
                        if (m_pPointColl == null)
                        {
                            m_pPointColl = new PolygonClass();
                            pGeom = new PolygonClass();
                        }
                        m_pPointColl.AddPoint(pMapPoint, ref before, ref after);
                        break;
                }

                //BeginDrawed(true);

                IGroupElement pGroup = null;
                if (m_pPointColl.PointCount == 1)
                {
                    //当为一个点时绘制点
                    ClsMarkDraw.AddSimpleGraphic(pMapPoint, ClsMarkDraw.getRGB(71, 61, 255), 3, sPolyOutlineName, m_hookHelper.FocusMap, pGroup);

                }
                else if (m_DrawType.ToString() == "esriGeometryLine")
                {

                    pGeom = m_pPointColl as IGeometry;
                    pGeom.SpatialReference = pMapPoint.SpatialReference;
                    m_psurface.InterpolateShape(pGeom, out  pGeom, ref StepSize);
                    ClsMarkDraw.AddSimpleGraphic(pGeom, ClsMarkDraw.getRGB(71, 61, 255), 2, sPolyOutlineName, m_hookHelper.FocusMap, pGroup);
                    m_pPointColl = pGeom as IPointCollection;
                }
                else
                {
                    ITopologicalOperator pTopo = m_pPointColl as ITopologicalOperator;
                    pGeom = pTopo.Boundary;
                    pGeom.SpatialReference = pMapPoint.SpatialReference;
                    m_psurface.InterpolateShape(pGeom, out  pGeom, ref StepSize);
                    ClsMarkDraw.AddSimpleGraphic(pGeom, ClsMarkDraw.getRGB(71, 61, 255), 2, sPolyOutlineName, m_hookHelper.FocusMap, pGroup);
                }

                m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            }
            catch
            {
                return;
            }
        }
        public override void OnDblClick()
        {
            ITopologicalOperator pTopoOp;
            if (m_pPointColl != null)
            {
                switch (m_DrawType.ToString())
                {

                    case "esriGeometryLine":
                        IPointCollection pPolyLine = new PolylineClass();
                        pPolyLine.AddPointCollection(m_pPointColl);
                        pTopoOp = pPolyLine as ITopologicalOperator;
                        pTopoOp.Simplify();
                        m_Geometry = pPolyLine as IGeometry;
                        //ZQ   20110809
                        m_Geometry.SpatialReference = m_psurface.Domain.SpatialReference;
                        //m_Geometry.SpatialReference = m_hookHelper.FocusMap.SpatialReference;
                        ClsMarkDraw.DeleteAllElementsWithName(m_hookHelper.FocusMap, sPolyOutlineName);
                        EndDtrawd(true);//触发结束事件
                        break;
                    case "esriGeometryPolygon":
                        if (m_pPointColl.PointCount < 3)
                        {
                            return;
                        }
                        IPointCollection pPolygon = new PolygonClass();
                        pPolygon.AddPointCollection(m_pPointColl);
                        pTopoOp = pPolygon as ITopologicalOperator;
                        pTopoOp.Simplify();
                        m_Geometry = pPolygon as IGeometry;
                        //ZQ   20110809
                        m_Geometry.SpatialReference = m_psurface.Domain.SpatialReference;
                        //m_Geometry.SpatialReference = m_hookHelper.FocusMap.SpatialReference;
                        ClsMarkDraw.DeleteAllElementsWithName(m_hookHelper.FocusMap, sPolyOutlineName);
                        EndDtrawd(true);//触发结束事件
                        break;
                }
                m_pPointColl = null;

            }
            //Cls3DMarkDraw.DeleteAllElementsWithName(m_sceneHookHelper.Scene, sPolyOutlineName);

            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolDrawGeo.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolDrawGeo.OnMouseUp implementation
        }
        #endregion
    }
}
