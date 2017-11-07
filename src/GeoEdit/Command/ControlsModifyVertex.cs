using System;
using System.Collections.Generic;
using System.Text;

namespace GeoEdit
{
    public class ControlsModifyVertex : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        public ControlsModifyVertex()
        {
            base._Name = "GeoEdit.ControlsModifyVertex";
            base._Caption = "节点编辑";
            base._Tooltip = "节点编辑";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "节点编辑";

        }

        public override bool Enabled
        {
            get
            {
                if (myHook == null) return false;
                if (myHook.MapControl == null) return false;
                if (MoData.v_CurWorkspaceEdit == null) return false;
                if (myHook.MapControl.Map.SelectionCount != 1) return false;
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

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppGISRef;
            if (myHook.MapControl == null) return;
        }
    }
}
