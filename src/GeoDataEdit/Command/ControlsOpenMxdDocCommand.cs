using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using System.IO;

namespace GeoDataEdit.Command
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("9c1157bc-fa34-484e-9de6-38173d419850")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoDataEdit.Command.ControlsOpenMxdDocCommand")]
    public sealed class ControlsOpenMxdDocCommand : BaseCommand
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
        public ControlsOpenMxdDocCommand(AxMapControl pAxMapControl)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "打开地图文档";  //localizable text 
            base.m_message = "打开地图文档";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "OpenMxdDoc";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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
            m_AxMapControl = pAxMapControl;
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
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;
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
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            if (!ModMxd._MxdPath.Equals(""))
            {
                DialogResult pResult = MessageBox.Show("是否保存当前的地图文档?", "询问", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (pResult)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        {
                            IMxdContents pMxdC;

                            pMxdC = m_hookHelper.FocusMap as IMxdContents;

                            IMapDocument pMapDocument = new MapDocumentClass();
                            //打开地图文档
                            if (File.Exists(ModMxd._MxdPath))
                            {
                                pMapDocument.Open(ModMxd._MxdPath, "");
                            }
                            else
                            {
                                pMapDocument.New(ModMxd._MxdPath);
                            }
                            //保存信息
                            IActiveView pActiveView = m_hookHelper.ActiveView;

                            pMapDocument.ReplaceContents(pMxdC);

                            pMapDocument.Save(true, true);

                            break;
                        }
                    case DialogResult.No:
                        break;
                }
            }
            OpenFileDialog pOpendlg = new OpenFileDialog();
            pOpendlg.Title = "打开地图文档";

            pOpendlg.Filter = "(*.mxd)|*.mxd";
            if (pOpendlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string strMxdName = pOpendlg.FileName;
            if (m_AxMapControl.CheckMxFile(strMxdName))
            {
                m_AxMapControl.LoadMxFile(strMxdName, "", "");
            }
             
            ModMxd._MxdPath = strMxdName;
        }

        #endregion
    }
}
