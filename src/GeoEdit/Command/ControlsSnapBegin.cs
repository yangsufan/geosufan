using System;
using System.Collections.Generic;
using System.Text;

namespace GeoEdit
{
    public class ControlsSnapBegin : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        private bool m_Checked;
        public ControlsSnapBegin()
        {
            base._Name = "GeoEdit.ControlsSnapBegin";
            base._Caption = "²¶×½¿ªÆô";
            base._Tooltip = "²¶×½¿ªÆô";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "²¶×½¿ªÆô";

        }

        public override bool Enabled
        {
            get
            {
                if (myHook == null) return false;
                if (myHook.MapControl == null) return false;
                if (MoData.v_CurWorkspaceEdit == null) return false;
                return true;
            }
        }

        public override bool Checked
        {
            get
            {
                return m_Checked;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = myHook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = myHook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            m_Checked = !m_Checked;
            if (this.Checked == true)
            {
                MoData.v_bSnapStart = true;
            }
            else
            {
                MoData.v_bSnapStart = false;
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
