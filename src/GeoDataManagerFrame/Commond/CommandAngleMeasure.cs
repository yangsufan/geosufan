using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataManagerFrame
{
   public class CommandAngleMeasure: Plugin.Interface.CommandRefBase
    {
       private Plugin.Application.IAppGisUpdateRef _AppHk;
       public CommandAngleMeasure()
        {
            base._Name = "GeoDataManagerFrame.CommandAngleMeasure";
            base._Caption = "方位角量算";
            base._Tooltip = "方位角量算";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = false;
            base._Message = "方位角量算";

        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;
                if (_AppHk.CurrentControl == null) return false;
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
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
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;
            if (_AppHk.CurrentControl == null) return;
        }
    }
}
