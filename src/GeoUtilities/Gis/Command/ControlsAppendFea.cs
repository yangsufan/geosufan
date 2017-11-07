using System;
using System.Collections.Generic;
using System.Text;

namespace GeoUtilities
{
    public class ControlsAppendFea : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        public ControlsAppendFea()
        {
            base._Name = "GeoUtilities.ControlsAppendFea";
            base._Caption = "数据备份";
            base._Tooltip = "数据备份";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "数据备份";
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
            //
            frmAppendFea vAppendFea = new frmAppendFea();
            vAppendFea.ShowDialog();
            vAppendFea = null;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(base._Tooltip);//xisheng 2011.07.08 增加日志
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}
