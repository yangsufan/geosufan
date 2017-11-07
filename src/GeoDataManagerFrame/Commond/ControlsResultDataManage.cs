using System;
using System.Collections.Generic;
using System.Text;
////ZQ   20111012  add 成果数据管理 
namespace GeoDataManagerFrame
{
    public  class ControlsResultDataManage:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public ControlsResultDataManage()
        {
            base._Name = "GeoDataManagerFrame.ControlsResultDataManage";
            base._Caption = "成果数据管理";
            base._Tooltip = "成果数据管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "成果数据管理";
        }
        public override void OnClick()
        {
            
            //if (m_Hook == null) return;
            FrmResultDataManage pFrmResultDataManage = new FrmResultDataManage(Plugin.ModuleCommon.TmpWorkSpace);
            pFrmResultDataManage.ShowDialog();
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }

    }
}
