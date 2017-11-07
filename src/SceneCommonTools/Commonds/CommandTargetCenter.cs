using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;

namespace SceneCommonTools
{
    public class CommandTargetCenter : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;

        public CommandTargetCenter()
        {
            base._Name = "SceneCommonTools.CommandTargetCenter";
            base._Caption = "移动至目标";
            base._Tooltip = "移动至目标";
            base._Checked = false;
            base._Visible = true;
            //base._Enabled = true;
            base._Deactivate = false;
            base._Message = "移动至目标";
            //base._Cursor = (int)esriControlsMousePointer.esriPointerPan;
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
            if (_tool == null || _cmd == null || _AppHk == null) return;
            if (_AppHk.CurrentControl == null) return;

            _cmd.OnCreate(_AppHk.CurrentControl);
            _cmd.OnClick();
            if (_AppHk.CurrentControl is ISceneControl)
            {
                _AppHk.SceneControl.CurrentTool = _tool;
            }
            else
            {
                return;
            }

            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;

            _tool = new ControlsSceneTargetCenterToolClass();
            _cmd = _tool as ICommand;

        }
    }
}
