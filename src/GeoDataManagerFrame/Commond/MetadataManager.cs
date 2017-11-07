using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataManagerFrame
{
  public class MetadataManager : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
       public MetadataManager()
        {
            base._Name = "GeoDataManagerFrame.MetadataManager";
            base._Caption = "元数据管理";
            base._Tooltip = "元数据管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "元数据管理";
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
        }
    }
}
