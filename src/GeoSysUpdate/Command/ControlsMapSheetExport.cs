using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace GeoSysUpdate
{
    public class ControlsMapSheetExport : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsMapSheetExport()
        {
            base._Name = "GeoSysUpdate.ControlsMapSheetExport";
            base._Caption = "标准图幅输出";
            base._Tooltip = "标准图幅输出";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "标准图幅输出";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
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
            if (_hook == null) return;
            if (_hook.MainUserControl == null) return;

            UserControlSMPD pUserControl = _hook.MainUserControl as UserControlSMPD;
            if (pUserControl != null)
            {
                //切换到标准图幅tab页
                pUserControl.TurnToSheetTab();
            }

            //更新图库树

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
    }
}
