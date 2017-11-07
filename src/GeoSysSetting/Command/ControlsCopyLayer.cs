using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoSysSetting
{
    public class ControlsCopyLayer : Plugin.Interface.CommandRefBase
    {
        private UserControl ucCtl = null;
        private Plugin.Application.IAppFormRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;

        public ControlsCopyLayer()
        {
            base._Name = "GeoSysSetting.ControlsCopyLayer";
            base._Caption = "复制图层";
            base._Tooltip = "复制图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "复制图层";
        }

        public override bool Enabled
        {
            get
            {
                if (_hook == null) return false;
                if (_hook.MainForm.Controls[0] is SubControl.UCDataSourceManger)
                {
                    SubControl.UCDataSourceManger pUCdatasource = _hook.MainForm.Controls[0] as SubControl.UCDataSourceManger;
                    if (pUCdatasource != null)
                    {
                        string strtag = pUCdatasource.GetTagOfSeletedNode();
                        if (strtag.Equals("Layer"))
                            return true;
                    }
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
            SubControl.UCDataSourceManger pUCdatasource = _hook.MainForm.Controls[0] as SubControl.UCDataSourceManger;
            if (pUCdatasource == null)
            {
                return;
            }
            pUCdatasource.CopyLayer();

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppFormRef;
            _hook = hook as Plugin.Application.IAppFormRef;


        }
    }
}
