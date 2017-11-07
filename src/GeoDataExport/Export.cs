using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
namespace GeoDataExport
{
    /// <summary>
    /// Summary description for Export.
    /// </summary>
    [Guid("2c0ac223-921e-459b-b6af-d1271389989e")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoDataExport.Export")]
    public sealed class Export : BaseTool
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
            GMxCommands.Register(regKey);
            MxCommands.Register(regKey);
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
            GMxCommands.Unregister(regKey);
            MxCommands.Unregister(regKey);
            SxCommands.Unregister(regKey);
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

        private IHookHelper m_hookHelper = null;
        private IGlobeHookHelper m_globeHookHelper = null;
        private ISceneHookHelper m_sceneHookHelper = null;
        public IGeometry pGeometry = null;
        public Export()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "dataex"; //localizable text 
            base.m_caption = "ex";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl, " +
                             "ArcScene/SceneControl, ArcGlobe/GlobeControl";   //localizable text 
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
            // Test the hook that calls this command and disable if nothing valid
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
            {
                try
                {
                    //Can be a scene or globe
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
                {
                    //Can be globe
                    try
                    {
                        m_globeHookHelper = new GlobeHookHelperClass();
                        m_globeHookHelper.Hook = hook;
                        if (m_globeHookHelper.ActiveViewer == null)
                        {
                            //Nothing valid!
                            m_globeHookHelper = null;
                        }
                    }
                    catch
                    {
                        m_globeHookHelper = null;
                    }
                }
            }

            if (m_globeHookHelper == null && m_sceneHookHelper == null && m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            
            if (m_hookHelper != null)
            {
                //frmExport frm = new frmExport(m_hookHelper, pGeometry);
               // frm.Show();
                //TODO: Add Map/PageLayout related logic
            }
            else if (m_sceneHookHelper != null)
            {
                //TODO: Add Scene related logic
            }
            else if (m_globeHookHelper != null)
            {
                //TODO: Add Globe related logic
            }
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
           
            if (m_hookHelper != null)
            {
                IMapControl2 pMapcontrol = (IMapControl2)((IToolbarControl)m_hookHelper.Hook).Buddy;
                pMapcontrol.ActiveView.Refresh();
                pGeometry = pMapcontrol.TrackPolygon();
               
                ISimpleFillSymbol pSimplefillSymbol = new SimpleFillSymbolClass();
                IRgbColor pRgbColor = new RgbColorClass();
                pRgbColor.Red = 155;
                pRgbColor.Green = 100;
                pRgbColor.Blue = 200;
                pSimplefillSymbol.Color = (IColor)pRgbColor;
                ILineSymbol pLineSymbol = new SimpleLineSymbolClass();
                pLineSymbol.Color = (IColor)pRgbColor;
                pLineSymbol.Width = 1;
                pSimplefillSymbol.Outline = pLineSymbol;
                pSimplefillSymbol.Style = esriSimpleFillStyle.esriSFSHollow;
                object symbol = pSimplefillSymbol;
                pMapcontrol.DrawShape(pGeometry, ref symbol);
                IMap pMap=m_hookHelper.FocusMap;
                frmExport frm = new frmExport(pMap, pGeometry);
                frm.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                frm.Show();
                //TODO: Add Map/PageLayout related logic
            }
            else if (m_sceneHookHelper != null)
            {
                //TODO: Add Scene related logic
            }
            else if (m_globeHookHelper != null)
            {
                //TODO: Add Globe related logic
            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            if (m_hookHelper != null)
            {
                //TODO: Add Map/PageLayout related logic
            }
            else if (m_sceneHookHelper != null)
            {
                //TODO: Add Scene related logic
            }
            else if (m_globeHookHelper != null)
            {
                //TODO: Add Globe related logic
            }
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (m_hookHelper != null)
            {

                //TODO: Add Map/PageLayout related logic
            }
            else if (m_sceneHookHelper != null)
            {
                //TODO: Add Scene related logic
            }
            else if (m_globeHookHelper != null)
            {
                //TODO: Add Globe related logic
            }
        }
        #endregion
    }
}
