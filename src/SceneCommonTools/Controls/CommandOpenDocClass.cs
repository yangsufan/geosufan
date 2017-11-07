using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
//打开ArcScene场景文档     张琪     20110707
namespace SceneCommonTools
{
    /// <summary>
    /// Command that works in ArcScene or SceneControl
    /// </summary>
    [Guid("3f57da8b-d4c0-48a0-82ad-20e9983f0c3b")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SceneCommonTools.Controls.CommandOpenDocClass")]
    public sealed class CommandOpenDocClass : BaseCommand
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

        public CommandOpenDocClass()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "打开三维场景文档"; //localizable text
            base.m_caption = "打开三维场景文档";  //localizable text 
            base.m_message = "打开三维场景文档";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "SceneCommonTools.CommandOpenDocClass";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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

        /// <summary>
        /// 打开ArcScene场景文档    张琪   
        /// </summary>
        public override void OnClick()
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "打开一个场景文档";
                openFileDialog1.Filter = "ArcScene场景(*.sxd)|*.sxd";
                openFileDialog1.Multiselect = false;
                DialogResult pDialogResult = openFileDialog1.ShowDialog();
                if (pDialogResult != DialogResult.OK)
                {
                    return;
                }
                
                string pFileName = openFileDialog1.FileName;
                
                Plugin.LogTable.Writelog(Caption+pFileName);//xisheng 日志记录07.08
                ISceneControl pSceneControl = m_sceneHookHelper.Hook as ISceneControl;
                vProgress.EnableCancel = false;//设置进度条
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("正在加载三维场景数据");
                pSceneControl.LoadSxFile(pFileName);//加载三维文档
                pSceneControl.SceneGraph.RefreshViewers();
                vProgress.Close();
            }
            catch
            {
                vProgress.Close();
            }
        }

        #endregion
    }
}
