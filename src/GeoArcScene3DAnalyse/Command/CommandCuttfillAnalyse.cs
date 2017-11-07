using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
//填挖      张琪   20110613
namespace GeoArcScene3DAnalyse
{
    /// <summary>
    /// Command that works in ArcScene or SceneControl
    /// </summary>
    [Guid("de86e6d8-4a72-44d2-bffe-82d6e59af064")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoArcScene3DAnalyse.Command.CommandCuttofillAnalyse")]
    public sealed class CommandCuttfillAnalyse : BaseCommand
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

        public CommandCuttfillAnalyse()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "填挖";  //localizable text 
            base.m_message = "填挖";  //localizable text
            base.m_toolTip = "填挖";  //localizable text
            base.m_name = "CommandCuttofillAnalyse";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
           
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

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;
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
           

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_sceneHookHelper == null) return;
            ISceneControl pSceneControl = m_sceneHookHelper.Hook as ISceneControl;
            frmCuttfillAnalyse pfrmCuttofillAnalyse = new frmCuttfillAnalyse();
            pfrmCuttofillAnalyse.WriteLog = this.WriteLog;
            pfrmCuttofillAnalyse.CurrentSceneControl = m_sceneHookHelper.Hook as ISceneControl;
            pfrmCuttofillAnalyse.initialization();
            pfrmCuttofillAnalyse.ShowDialog(); 
        }

        #endregion
    }
}
