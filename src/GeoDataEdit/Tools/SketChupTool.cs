using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using DevComponents.DotNetBar;

namespace GeoDataEdit.Tools
{
    /// <summary>
    /// Summary description for SketChupTool.
    /// </summary>
    [Guid("0eeb1539-21c6-4fa3-bef0-e4d501ae5f0f")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("MapDataEdit.Tools.SketChupTool")]
    public sealed class SketChupTool : BaseTool
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
        private AxMapControl m_AxMapControl;
        private static IFeature m_pEditFeature; 
        private static IPoint m_PointStop;
        private static INewLineFeedback m_pNewLineFeedback;
        private static INewPolygonFeedback m_pPlygonFeadback;
        private static bool m_bInUse;
        public SketChupTool(AxMapControl pAxMapControl)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "绘制图形";  //localizable text 
            base.m_message = "绘制图形";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "SketChup";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
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
            m_AxMapControl = pAxMapControl;
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
               m_bInUse = false;
            // TODO:  Add other initialization code
        }
        private  void EndSketch(AxMapControl m_MapControl)                                            //结束一次画图操作
        {
            IGeometry m_Geometry = null;
            IPointCollection m_PointCollection = null;
            if (m_pNewLineFeedback != null)                            //线对象有效
            {
                //INewLineFeedback m_LineFeed = (INewLineFeedback)m_Feedback;
                m_pNewLineFeedback.AddPoint(m_PointStop);
                IPolyline m_PolyLine = m_pNewLineFeedback.Stop();
                m_PointCollection = (IPointCollection)m_PolyLine;
                if (m_PointCollection.PointCount < 2)
                {
                    MessageBoxEx.Show("需要两个点才能生成一条线！", "未能生成线", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    m_Geometry = (IGeometry)m_PointCollection;
                }
            }
            else if (m_pPlygonFeadback != null)                    //多边形对象有效
            {

                m_pPlygonFeadback.AddPoint(m_PointStop);
                IPolygon m_Polygon = m_pPlygonFeadback.Stop();
                if (m_Polygon != null)
                    m_PointCollection = (IPointCollection)m_Polygon;
                if (m_PointCollection.PointCount < 3)
                {
                    MessageBoxEx.Show("需要三个点才能生成一个面！", "未能生成面", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    m_Geometry = (IGeometry)m_PointCollection;
                }
            }
            CreateFeature(m_Geometry);
            m_pPlygonFeadback = null;
            m_pNewLineFeedback = null;
            m_bInUse = false;
        }

        private  void CreateFeature(IGeometry m_Geometry)                       //创建要素
        {
            if (m_Geometry == null) return;
            if (CreateShape.m_CurrentLayer == null) return;

            IWorkspaceEdit m_WorkspaceEdit = GetWorkspaceEdit();
            IFeatureLayer m_FeatureLayer = (IFeatureLayer)CreateShape.m_CurrentLayer;
            IFeatureClass m_FeatureClass = m_FeatureLayer.FeatureClass;

            m_WorkspaceEdit.StartEditOperation();                            //使用WorkspaceEdit接口新建要素
            IFeature m_Feature = m_FeatureClass.CreateFeature();
            m_Feature.Shape = m_Geometry;
            m_Feature.Store();
            m_WorkspaceEdit.StopEditOperation();

            // 以一定缓冲范围刷新视图
            IActiveView m_ActiveView = m_AxMapControl.ActiveView;
            if (m_Geometry.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                double Length;
                Length = ConvertPixelsToMapUnits(m_ActiveView, 30);
                ITopologicalOperator m_Topo = (ITopologicalOperator)m_Geometry;
                IGeometry m_Buffer = m_Topo.Buffer(Length);
                m_ActiveView.PartialRefresh((esriViewDrawPhase)(esriDrawPhase.esriDPGeography |
                    esriDrawPhase.esriDPSelection), CreateShape.m_CurrentLayer, m_Buffer.Envelope);
            }
            else
                m_ActiveView.PartialRefresh((esriViewDrawPhase)(esriDrawPhase.esriDPGeography |
                    esriDrawPhase.esriDPSelection), CreateShape.m_CurrentLayer, m_Geometry.Envelope);
        }
        private  IWorkspaceEdit GetWorkspaceEdit()                            //获取当前编辑空间
        {
            if (CreateShape.m_CurrentLayer == null) return null;

            IFeatureLayer m_FeatureLayer = (IFeatureLayer)CreateShape.m_CurrentLayer;
            IFeatureClass m_FeatureClass = m_FeatureLayer.FeatureClass;
            IDataset m_Dataset = (IDataset)m_FeatureClass;
            if (m_Dataset == null) return null;
            return (IWorkspaceEdit)m_Dataset.Workspace;
        }

        private  double ConvertPixelsToMapUnits(IActiveView pActiveView, double pixelUnits)
        {
            // 依据当前视图，将屏幕像素转换成地图单位
            IPoint Point1 = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperLeft;
            IPoint Point2 = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperRight;
            int x1, x2, y1, y2;
            pActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(Point1, out x1, out y1);
            pActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(Point2, out x2, out y2);
            double pixelExtent = x2 - x1;
            double realWorldDisplayExtent = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            double sizeOfOnePixel = realWorldDisplayExtent / pixelExtent;
            return pixelUnits * sizeOfOnePixel;
        }
        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add SketChupTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            object pObj =Type.Missing;
            if (Button == 1)
            {
                //IFeatureLayer m_FeatureLayer = (IFeatureLayer)CreateShape.m_CurrentLayer;
                //if (m_FeatureLayer.FeatureClass == null) return;
                IActiveView m_ActiveView = m_AxMapControl.ActiveView;
                IPoint m_PointMousedown = m_ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

                m_PointStop = m_PointMousedown;

                if (!m_bInUse)
                {
                    //switch (m_FeatureLayer.FeatureClass.ShapeType)
                    //{
                    //    case esriGeometryType.esriGeometryPoint:         //点类型
                    //        CreateFeature(m_PointMousedown);
                    //        m_AxMapControl.ActiveView.Refresh();
                    //        break;
                    //    case esriGeometryType.esriGeometryPolyline:      //线类型
                    //        m_bInUse = true;
                    //        m_pNewLineFeedback = new NewLineFeedbackClass();
                    //        m_pNewLineFeedback.Display = m_ActiveView.ScreenDisplay;
                    //        m_pNewLineFeedback.Start(m_PointMousedown);
                    //        break;
                    //    case esriGeometryType.esriGeometryPolygon:       //多边形类型
                    //        m_bInUse = true;
                    //        m_pPlygonFeadback = new NewPolygonFeedbackClass();
                    //        m_pPlygonFeadback.Display = m_ActiveView.ScreenDisplay;
                    //        m_pPlygonFeadback.Start(m_PointMousedown);
                    //        break;
                    //}
                    m_bInUse = true;
                    m_pNewLineFeedback = new NewLineFeedbackClass();
                    m_pNewLineFeedback.Display = m_ActiveView.ScreenDisplay;
                    m_pNewLineFeedback.Start(m_PointMousedown);
                }
                else
                {
                    if (m_pNewLineFeedback != null)
                    {
                        m_pNewLineFeedback.AddPoint(m_PointMousedown);

                    }
                    else if (m_pPlygonFeadback != null)
                    {
                        m_pPlygonFeadback.AddPoint(m_PointMousedown);
                    }
                }
            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            IFeatureLayer m_FeatureLayer = (IFeatureLayer)CreateShape.m_CurrentLayer; ;
             if (m_FeatureLayer.FeatureClass == null) return;
          
            // 移动鼠标形成线、面的节点
            IActiveView m_ActiveView =m_AxMapControl.ActiveView;
            IPoint pMovePoint = m_ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
          
                if (m_pNewLineFeedback != null)
                {
                    m_pNewLineFeedback.MoveTo(pMovePoint);
                }
                else if (m_pPlygonFeadback != null)
                {
                    m_pPlygonFeadback.MoveTo(pMovePoint);
                }
        }
        public override void OnDblClick()
        {
            EndSketch(m_AxMapControl);
        }
        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add SketChupTool.OnMouseUp implementation
        }
        #endregion
    }
}
