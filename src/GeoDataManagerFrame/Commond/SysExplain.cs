using System;
using System.Collections.Generic;
using System.Text;
using GeoDataCenterFunLib;

//关于
namespace GeoDataManagerFrame
{
    public class SysExplain : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public SysExplain()
        {
            base._Name = "GeoDataManagerFrame.SysExplain";
            base._Caption = "关于";
            base._Tooltip = "关于";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "关于";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            frmExplain frm = new frmExplain(0);
            frm.ShowDialog();

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
