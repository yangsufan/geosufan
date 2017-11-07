using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;

//保存地图文档
namespace GeoDataManagerFrame
{
    public class SaveMapDoc : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public SaveMapDoc()
        {
            base._Name = "GeoDataManagerFrame.SaveMapDoc";
            base._Caption = "保存地图文档";
            base._Tooltip = "保存地图文档";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "保存地图文档";
        }

        public override void OnClick()
        {
            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);

                if (log != null)
                {
                    log.Writelog("保存地图文档");
                }
            }
            string strWorkFile = Application.StartupPath + "\\..\\Temp\\CurPrj.xml";
            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            xmldoc.Load(strWorkFile);
            SetControl  pSetControl=(SetControl)m_Hook.MainUserControl;
            string savefilename = pSetControl.m_SaveXmlFileName ;
            xmldoc.Save(savefilename);
           
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
