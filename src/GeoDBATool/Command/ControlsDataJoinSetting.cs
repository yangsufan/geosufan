using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    class ControlsDataJoinSetting : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsDataJoinSetting()
        {
            base._Name = "GeoDBATool.ControlsDataJoinSetting";
            base._Caption = "接边参数设置";
            base._Tooltip = "";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "接边参数设置";

        }

        public override bool Enabled
        {
            get
            {
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            frmJoinSetting Fjoinsetting = new frmJoinSetting();
            Fjoinsetting.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
