using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;

namespace GeoSysUpdate
{
    /// <summary>
    /// 右键菜单折叠图层   chenyafei
    /// </summary>
    public class ControlsCollapseAllNode: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsCollapseAllNode()
        {

            base._Name = "GeoSysUpdate.ControlsCollapseAllNode";
            base._Caption = "折叠所有节点";
            base._Tooltip = "折叠所有节点";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "折叠所有节点";
        }
        public override bool Enabled
        {
            get
            {
                if (_hook.MapControl == null || _hook.TOCControl == null) return false;
                if (_hook.MainUserControl==null) return false;
                return true;
            }
        }
        public override void OnClick()
        {
            if (_hook == null)
                return;
            if (_hook.MainUserControl == null)
                return;
            UserControlSMPD pUserControl = _hook.MainUserControl as UserControlSMPD;
            if (pUserControl != null)
            {
                pUserControl.CollapseAllNode();
            }
        }
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
    }
}
