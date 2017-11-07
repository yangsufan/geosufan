using System;
using System.Collections.Generic;
using System.Text;
using GeoDataCenterFunLib;

namespace GeoDBConfigFrame
{
    public class NewDOCDirectory : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public NewDOCDirectory()
        {
            base._Name = "GeoDBConfigFrame.NewDOCDirectory";
            base._Caption = "配置文档数据源";
            base._Tooltip = "配置文档数据源";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "配置文档数据源";
        }
        public override void OnClick()
        {

            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("配置文档数据源");

                }
            }
            frmNewDOCDirectory frm = new frmNewDOCDirectory();
            frm.Show();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
