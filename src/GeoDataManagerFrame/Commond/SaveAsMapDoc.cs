using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;

//另存地图文档
namespace GeoDataManagerFrame
{
    public class SaveAsMapDoc : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public SaveAsMapDoc()
        {
            base._Name = "GeoDataManagerFrame.SaveAsMapDoc";
            base._Caption = "另存地图文档";
            base._Tooltip = "另存地图文档";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "另存地图文档";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
             
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "xml文件|*.xml";
            dlg.ShowDialog();
            string xmlpath = dlg.FileName;
            if (xmlpath.Equals ("") )
                return;

            LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
            string strLog = "另存地图文档,目标文件路径:" + xmlpath;
            if (log != null)
            {
                log.Writelog(strLog);
            }

            string strWorkFile = Application.StartupPath + "\\..\\Temp\\CurPrj.xml";
            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            xmldoc.Load(strWorkFile);
            xmldoc.Save(xmlpath);

            /*       Exception eError;
                   AddGroup frmGroup = new AddGroup();
                   if (frmGroup.ShowDialog() == DialogResult.OK)
                   {
                       ModuleOperator.DisplayRoleTree("", m_Hook.RoleTree, ref ModData.gisDb, out eError);
                       if (eError != null)
                       {
                           ErrorHandle.ShowFrmError("提示", eError.Message);
                           return;
                       }
                   }
             */
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}
