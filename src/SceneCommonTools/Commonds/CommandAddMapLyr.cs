using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;

namespace SceneCommonTools
{
    public class CommandAddMapLyr : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private ICommand _cmd = null;

        public CommandAddMapLyr()
        {
            base._Name = "SceneCommonTools.CommandAddMapLyr";
            base._Caption = "加载二维图层";
            base._Tooltip = "加载二维图层";
            base._Checked = false;
            base._Visible = true;
            //base._Enabled = true;
            base._Message = "加载二维图层";
            //base._Image = "";
            //base._Category = "";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;
                if (_AppHk.CurrentControl == null) return false;
                if (_AppHk.CurrentControl is ISceneControl) return true;
                return false;
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
            if (_AppHk.CurrentControl == null) return;
           
            FrmAddLayerFromMap vfrm = new SceneCommonTools.FrmAddLayerFromMap();
            vfrm.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            vfrm.CurMap = _AppHk.MapControl.Map;
            vfrm.CurScene = _AppHk.SceneControl.Scene;
            vfrm.ShowDialog();

            //_AppHk.SceneControl = _AppHk.MapControl.Extent;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}
