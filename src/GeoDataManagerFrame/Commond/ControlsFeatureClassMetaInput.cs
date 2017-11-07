using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataManagerFrame
{
    public class ControlsFeatureClassMetaInput : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public ControlsFeatureClassMetaInput()
        {
            base._Name = "GeoDataManagerFrame.ControlsFeatureClassMetaInput";
            base._Caption = "数据库属性信息录入";
            base._Tooltip = "数据库属性信息录入";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "数据库属性信息录入";
        }
        public override void OnClick()
        {
            //if (m_Hook == null) return;
            frmFeatureClassMetaInput pfrmFeatureClassMetaInput = new frmFeatureClassMetaInput(Plugin.ModuleCommon.TmpWorkSpace);
            pfrmFeatureClassMetaInput.ShowDialog();
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
