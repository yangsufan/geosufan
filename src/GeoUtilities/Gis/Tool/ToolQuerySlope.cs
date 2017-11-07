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
    public delegate void myEventHandlerSlope (bool BeginDraw);//委托事件
    /// <summary>
    /// Summary description for ToolQuerySlope.
    /// </summary>
    [Guid("a6912b16-d78c-4919-a62d-13cab64b4760")]
    [ClassInterface(ClassInterfaceType.None)] 
    [ProgId("GeoUtilities.Gis.Tool.ToolQuerySlope")]
    public sealed class ToolQuerySlope : BaseTool
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
        public event myEventHandlerSlope EndDtrawd;//定义结束事件
        //最终生成的集合对象
        public ESRI.ArcGIS.Geometry.IPoint m_Geometry = new PointClass();
        private const string sPolyOutlineName = "_POLYOUTLINE_";      
        //绘制三维要素所在的基准面
        ISurface m_psurface = null;
        public ISurface pSurface
        {
            set
            {
                m_psurface = value;
            }
        }

        public ToolQuerySlope()
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
            // TODO: Add ToolQuerySlope.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            try
            {
                ClsMarkDraw.DeleteAllElementsWithName(m_hookHelper.FocusMap, sPolyOutlineName);
                ESRI.ArcGIS.Geometry.IPoint pMapPoint = new ESRI.ArcGIS.Geometry.PointClass();
                m_hookHelper.FocusMap.SpatialReference = m_psurface.Domain.SpatialReference;
                pMapPoint = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                if (pMapPoint == null)
                {
                    return;
                }
                pMapPoint.Project(m_psurface.Domain.SpatialReference);
                pMapPoint.Z = m_psurface.GetElevation(pMapPoint);
                IGroupElement pGroup = null;
                ClsMarkDraw.AddSimpleGraphic(pMapPoint, ClsMarkDraw.getRGB(71, 61, 255), 3, sPolyOutlineName, m_hookHelper.FocusMap, pGroup);
                m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
               m_Geometry = pMapPoint;
                EndDtrawd(true);

            }
            catch
            {
                return;
            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolQuerySlope.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolQuerySlope.OnMouseUp implementation
        }
        #endregion
    }
}
