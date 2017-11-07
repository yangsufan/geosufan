using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace GeoSysUpdate
{
    public class ControlsExitMxdDoc : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsExitMxdDoc()
        {
            base._Name = "GeoSysUpdate.ControlsExitMxdDoc";
            base._Caption = "退出地图文档";
            base._Tooltip = "退出地图文档";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "退出地图文档";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                if (ModMxd._MxdPath.Equals("")) return false;
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
            ModMxd._MxdPath = "";

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
    }
}
