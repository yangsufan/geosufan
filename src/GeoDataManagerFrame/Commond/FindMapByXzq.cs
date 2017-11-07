using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataManagerFrame
{
    public class FindMapByXzq : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public FindMapByXzq()
        {
            base._Name = "GeoDataManagerFrame.FindMapByXzq";
            base._Caption = "按行政区定位";
            base._Tooltip = "按行政区定位";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "按行政区定位";
        }

        public override void OnClick()
        {
          
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
