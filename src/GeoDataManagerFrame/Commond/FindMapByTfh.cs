using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataManagerFrame
{
  public class FindMapByTfh : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public FindMapByTfh()
        {
            base._Name = "GeoDataManagerFrame.FindMapByTfh";
            base._Caption = "按图幅号定位";
            base._Tooltip = "按图幅号定位";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "按图幅号定位";
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
