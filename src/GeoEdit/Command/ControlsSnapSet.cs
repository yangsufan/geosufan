using System;
using System.Collections.Generic;
using System.Text;

namespace GeoEdit
{
    public class ControlsSnapSet : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        public ControlsSnapSet()
        {
            base._Name = "GeoEdit.ControlsSnapSet";
            base._Caption = "≤∂◊Ω…Ë÷√";
            base._Tooltip = "≤∂◊Ω…Ë÷√";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "≤∂◊Ω…Ë÷√";

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
            frmSnapSet pfrmSnapSet = new frmSnapSet(myHook.MapControl.Map);
            Plugin.Application.IAppFormRef pAppFormRef=myHook as Plugin.Application.IAppFormRef;
            pfrmSnapSet.Show(pAppFormRef.MainForm);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
