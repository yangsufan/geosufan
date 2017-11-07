using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;

using System.Windows.Forms;
//通视分析      张琪    20110618
namespace GeoArcScene3DAnalyse
{
    /// <summary>
    /// Summary description for Tool3DLineOfSight.
    /// </summary>
    [Guid("8f371c5a-bc68-41cf-aa3d-c851b718cf14")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoArcScene3DAnalyse.Tool.Tool3DLineOfSight")]
    public sealed class Tool3DLineOfSight : BaseTool
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
            SxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            SxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private ISceneHookHelper m_sceneHookHelper = null;
        //当前的surface
        public ISurface m_SurFace;
        //当前图层
        public ILayer m_Layer;
        private frm3DLineOfSight m_frm3DLineOfSight;
        public IActiveView m_pActiveView;
        public IDisplayTransformation m_pDispTrans;
        private INewLineFeedback m_pNewLineFeedback = null;
        private IPointCollection m_pScenePoints = null;
      

        public Tool3DLineOfSight()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "Vertical Line Of Sight";  //localizable text 
            base.m_message = "查询两点间的通视情况";  //localizable text
            base.m_toolTip = "3D Developer Samples";  //localizable text
            base.m_name = "通视分析";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
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
                m_sceneHookHelper = new SceneHookHelperClass();
                m_sceneHookHelper.Hook = hook;
                if (m_sceneHookHelper.ActiveViewer == null)
                {
                    m_sceneHookHelper = null;
                }
            }
            catch
            {
                m_sceneHookHelper = null;
            }

            if (m_sceneHookHelper == null)
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
            m_pScenePoints = null;
            m_frm3DLineOfSight = new frm3DLineOfSight();
            m_frm3DLineOfSight.CurrentSceneControl = m_sceneHookHelper.Hook as ISceneControl;
            m_frm3DLineOfSight.Init(1,0,this);//初始化设置界面
            m_frm3DLineOfSight.initialization();
            m_frm3DLineOfSight.Show();
            m_frm3DLineOfSight.Focus();
            m_frm3DLineOfSight.TopMost = false;
        }
        /// <summary>
        /// 通过鼠标点击获取观察点与被观察点同时进行两点间通视分析
        /// </summary>
        /// <param name="Button"></param>
        /// <param name="Shift"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if(m_frm3DLineOfSight.m_Layer ==null||m_frm3DLineOfSight.m_Surface==null)
            {
                MessageBox.Show("请设置有效的表面数据", "提示！");
                return;
            }
            if(m_frm3DLineOfSight.txtObsOffset.Text==""||m_frm3DLineOfSight.txtTarOffset.Text=="")
            {
                MessageBox.Show("观察点高度和被观察点高度不能为空","提示！");
                return;
            }
            m_frm3DLineOfSight.TopMost = true;
            ISceneGraph pSceneGraph = m_sceneHookHelper.SceneGraph;
            m_pNewLineFeedback = new NewLineFeedbackClass();
            IPolyline pPolyline = m_pNewLineFeedback.Stop();//用于判断是否已经获取两点
            ISceneControl pSceneControl = m_sceneHookHelper.Hook as ISceneControl;
            Cls3DModulsefun pCls3DModulsefun = new Cls3DModulsefun();//用于绘制通视分析结果的方法
            object pOwner;
            object pObject;
            ESRI.ArcGIS.Geometry.IPoint pPoint = new ESRI.ArcGIS.Geometry.PointClass();
            pSceneGraph.Locate(pSceneGraph.ActiveViewer, X, Y, esriScenePickMode.esriScenePickGeography, true,out pPoint,out  pOwner,out pObject);//获取鼠标点击的位置并转化为地理坐标
            if(pPoint == null)
            {
                return;
            }
            ESRI.ArcGIS.Geometry.IPoint pFlashPoint = new ESRI.ArcGIS.Geometry.PointClass();
            IClone pClone = pPoint as IClone;
            pFlashPoint = pClone.Clone() as ESRI.ArcGIS.Geometry.IPoint;
            pFlashPoint.Z = pFlashPoint.Z / pSceneGraph.VerticalExaggeration;
            pFlashPoint.SpatialReference = pSceneGraph.Scene.SpatialReference;
            IDisplay3D pDisplay = pSceneGraph as IDisplay3D;
            pDisplay.FlashLocation(pFlashPoint);//闪烁显示被点击的位置
            IGeometry pGeometry = null;
            if(m_pScenePoints==null)
            {
                m_pScenePoints = new PolylineClass();
                pGeometry = m_pScenePoints as IGeometry;
                pGeometry.SpatialReference = pSceneGraph.Scene.SpatialReference;
            }
            object before = Type.Missing; 
            object after = Type.Missing; 
            m_pScenePoints.AddPoint(pPoint, ref before,ref after);//添加获取的点到点集合中
            if (m_pScenePoints.PointCount == 2)
            {
                pClone = m_pScenePoints as IClone;
                pPolyline = pClone.Clone() as  ESRI.ArcGIS.Geometry.IPolyline;//当点集合中点数达到两个时生成一条线用于判断观察点与被观察点是否确定
                m_pScenePoints = null;
            }
            if(pPolyline !=null)
            {
                m_pScenePoints = null;
                ISurface pSurface = m_SurFace;
                ESRI.ArcGIS.Geometry.IPoint fPoint = pPolyline.FromPoint;//获取观察点
                fPoint.Z = pSurface.GetElevation(fPoint);//获取观察点的高程
                ESRI.ArcGIS.Geometry.IPoint tPoint = pPolyline.ToPoint;
                tPoint.Z = pSurface.GetElevation(tPoint);
                if (pSurface.IsVoidZ(fPoint.Z) || pSurface.IsVoidZ(tPoint.Z))
                {
                    return;
                }
                fPoint.Z = fPoint.Z + Convert.ToDouble(m_frm3DLineOfSight.txtObsOffset.Text);//观察者的高度加上观察者所在的高程才是观察点实际的高程
                tPoint.Z = tPoint.Z + Convert.ToDouble(m_frm3DLineOfSight.txtTarOffset.Text);
                ESRI.ArcGIS.Geometry.IPoint pObstruct;
                IPolyline pVisPolyline;
                IPolyline pInVisPolyline;
                bool bIsVis;
                object pRefractionFactor = Type.Missing;
                //进行两点间的通视分析
                pSurface.GetLineOfSight(fPoint, tPoint,out pObstruct, out pVisPolyline, out pInVisPolyline, out bIsVis, m_frm3DLineOfSight.checkBoxCurv.Checked, m_frm3DLineOfSight.checkBoxCurv.Checked,ref pRefractionFactor);

                ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
                pSimpleLineSymbol.Width = 2;
                pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                //绘制可视与不可视的沿地表线要素
                if(pVisPolyline!=null)
                {
                    pSimpleLineSymbol.Color=Cls3DMarkDraw.getRGB(0,255,0);
                    pCls3DModulsefun.AddGraphic(pSceneControl, pVisPolyline as IGeometry, pSimpleLineSymbol as ISymbol);

                }
                if(pInVisPolyline !=null)
                {
                    pSimpleLineSymbol.Color=Cls3DMarkDraw.getRGB(255,0,0);
                    pCls3DModulsefun.AddGraphic(pSceneControl, pInVisPolyline as IGeometry, pSimpleLineSymbol as ISymbol);

                }
                IGeometryCollection pVisPatch = new MultiPatchClass();//用于存储可视域的要素
                pGeometry = pVisPatch as IGeometry;
                pGeometry.SpatialReference = pSceneGraph.Scene.SpatialReference;
                double dTargetHeightForVis = 0;
                ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbolClass();
                IGeometryCollection pInVisPatch = new MultiPatchClass();//存储不可视域的要素
                pGeometry = pInVisPatch as IGeometry;
                pGeometry.SpatialReference = pSceneGraph.Scene.SpatialReference;
                IGeometryCollection pPathGeo = pInVisPolyline as IGeometryCollection;
                if (pPathGeo != null)
                {
                    //下面的作用是将不可视域线每段path生成线要素进行绘制       张琪  20110623
                    for (int i = 0; i < pPathGeo.GeometryCount; i++)
                    { 
                        IGeometryCollection pInPolyline = new PolylineClass();
                        IPath path = pPathGeo.get_Geometry(i) as IPath;
                        pInPolyline.AddGeometry(path as IGeometry, ref before, ref after);
                       pCls3DModulsefun.CreateVerticalLOSPatches(bIsVis, fPoint, tPoint, pVisPolyline, pInPolyline as IPolyline, pVisPatch, pInVisPatch, dTargetHeightForVis);
                    }
                }
                else//当不可视域为空时，直接分析生成可视域与不可视域
                {
                    pCls3DModulsefun.CreateVerticalLOSPatches(bIsVis, fPoint, tPoint, pVisPolyline, pInVisPolyline, pVisPatch, pInVisPatch, dTargetHeightForVis);
                }
                //
                // 对可视域与不可视域要素在场景中绘制出来
                if (pInVisPatch != null)
                {
                    pSimpleFillSymbol.Color = Cls3DMarkDraw.getRGB(255, 0, 0);
                    pCls3DModulsefun.AddGraphic(pSceneControl, pInVisPatch as IGeometry, pSimpleFillSymbol as ISymbol);
                }
                if (pVisPatch != null)
                {
                    pSimpleFillSymbol.Color = Cls3DMarkDraw.getRGB(0, 255, 0);
                    pCls3DModulsefun.AddGraphic(pSceneControl, pVisPatch as IGeometry, pSimpleFillSymbol as ISymbol);
                }
            }

                
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool3DLineOfSight.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool3DLineOfSight.OnMouseUp implementation
        }
        #endregion
    }
}
