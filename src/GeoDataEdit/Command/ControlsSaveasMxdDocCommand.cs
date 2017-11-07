﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;

namespace GeoDataEdit.Command
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("4d2cdff1-7e4a-4a88-899e-7a3fc970dde0")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoDataEdit.Command.ControlsSaveasMxdDocCommand")]
    public sealed class ControlsSaveasMxdDocCommand : BaseCommand
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
        public ControlsSaveasMxdDocCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "另存为地图文档";  //localizable text 
            base.m_message = "另存为地图文档";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "SaveasMxdDoc";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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
            //保存路径

            SaveFileDialog Sfd = new SaveFileDialog();

            Sfd.Title = "另存为地图文档";

            Sfd.Filter = "(*.mxd)|*.mxd";

            if (Sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            ModMxd._MxdPath = Sfd.FileName;

            IMxdContents pMxdC;

            pMxdC = m_hookHelper.FocusMap as IMxdContents;

            IMapDocument pMapDocument = new MapDocumentClass();
            //创建地图文档
            pMapDocument.New(ModMxd._MxdPath);

            //保存信息
            IActiveView pActiveView = m_hookHelper.ActiveView;

            pMapDocument.ReplaceContents(pMxdC);
            try//yjl20110817 防止保存时，其他程序也在打开这个文档而导致共享冲突从而使系统报错
            {
                pMapDocument.Save(true, true);
                MessageBox.Show("保存成功", "提示！");
            }
            catch
            {
                MessageBox.Show("保存失败，请关掉其他正在打开该文档的程序，重新试一次。", "提示！");
            }
            pMapDocument.Close();//yjl20110817

        }

        #endregion
    }
}
