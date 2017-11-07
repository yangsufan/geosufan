using System;
using System.Collections.Generic;
using System.Text;
using GeoDataCenterFunLib;

//文档数据整理
namespace GeoDBConfigFrame
{
    public class DocDataArrange : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public DocDataArrange()
        {
            base._Name = "GeoDBConfigFrame.DocDataArrange";
            base._Caption = "文档数据整理";
            base._Tooltip = "文档数据整理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "文档数据整理";
        }
        public override void OnClick()
        {

            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("文档数据整理");

                }
            }
            frmDocRedution frm = new frmDocRedution();
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
