using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoSysSetting
{
    public class CommandQueryConfig : Plugin.Interface.CommandRefBase
    {
        private UserControl ucCtl = null;
        private Plugin.Application.IAppFormRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;

        public CommandQueryConfig()
        {
            base._Name = "GeoSysSetting.CommandQueryConfig";
            base._Caption = "查询配置";
            base._Tooltip = "查询功能配置";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "查询功能配置";
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
            Exception eError;
            FrmQueryConfig pFrm = new FrmQueryConfig(Plugin.ModuleCommon.TmpWorkSpace,"查询配置");
            pFrm.ShowDialog();

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppFormRef;
            _hook = hook as Plugin.Application.IAppFormRef;


        }
    }
}
