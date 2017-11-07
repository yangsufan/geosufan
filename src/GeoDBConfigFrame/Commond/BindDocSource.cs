using System;
using System.Collections.Generic;
using System.Text;
using GeoDataCenterFunLib;

namespace GeoDBConfigFrame
{
    class BindDocSource : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
         public BindDocSource()
        {
            base._Name = "GeoDBConfigFrame.BindDocSource";
            base._Caption = "文档数据挂接";
            base._Tooltip = "文档数据挂接";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "文档数据挂接";
        }

        public override void OnClick()
        {

            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("文档库挂接");

                }
            }
            frmBindSource frm = new frmBindSource(1);
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
