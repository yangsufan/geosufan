using System;
using System.Collections.Generic;
using System.Text;

//变化图斑研判
namespace GeoDataCenterFrame
{
    public class FindAlterTb : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public FindAlterTb()
        {
            base._Name = "GeoDataCenterFrame.FindAlterTb";
            base._Caption = "变化图斑研判";
            base._Tooltip = "变化图斑研判";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "变化图斑研判";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;


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
