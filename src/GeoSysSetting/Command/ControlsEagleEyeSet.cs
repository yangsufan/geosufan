using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSysSetting
{
    public class ControlsEagleEyeSet : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFormRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;
        private Plugin.Application.IAppArcGISRef _AppHk;

        public ControlsEagleEyeSet()
        {
            base._Name = "GeoSysSetting.ControlsEagleEyeSet";
            base._Caption = "鹰眼图设置导入";
            base._Tooltip = "鹰眼图设置导入";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "鹰眼图设置导入";
        }
        public override bool Enabled
        {
            get
            {
                if (_hook == null) return false;
                if (_hook.MainForm.Controls[0] is SubControl.UCDataSourceManger)
                {
                    return true;
                }
                return false;
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
            frmEagleEyeSet pfrmEagleEyeSet = new frmEagleEyeSet(_AppHk.ArcGisMapControl,Plugin.ModuleCommon.TmpWorkSpace);
            pfrmEagleEyeSet.ShowDialog();

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            m_Hook = hook as Plugin.Application.IAppFormRef;
            _hook = hook as Plugin.Application.IAppFormRef;


        }
    }
}
