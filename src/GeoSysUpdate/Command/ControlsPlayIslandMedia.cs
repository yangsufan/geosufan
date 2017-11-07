using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace GeoSysUpdate
{
    public class ControlsPlayIslandMedia : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsPlayIslandMedia()
        {
            base._Name = "GeoSysUpdate.ControlsPlayIslandMedia";
            base._Caption = "查看多媒体文件";
            base._Tooltip = "查看多媒体文件";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "查看多媒体文件";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_hook.XZQTree.SelectedNode == null) return false;
                if (_hook.XZQTree.SelectedNode.Tag != "Island")
                {
                    base._Visible = false;
                    return false;
                }
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

           //查看多媒体文件
            string path = _hook.XZQTree.SelectedNode.DataKey.ToString();
            //frmWindowsPlayer frm = new frmWindowsPlayer(path);
            //frm.Show();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
    }
}
