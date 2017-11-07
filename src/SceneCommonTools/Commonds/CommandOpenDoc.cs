using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;

namespace SceneCommonTools
{
    public class CommandOpenDoc : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private ICommand _cmd = null;

        public CommandOpenDoc()
        {
            base._Name = "SceneCommonTools.CommandOpenDoc";
            base._Caption = "打开三维文档";
            base._Tooltip = "打开三维文档";
            base._Checked = false;
            base._Visible = true;
            //base._Enabled = true;
            base._Message = "打开三维文档";
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
            if (_cmd == null || _AppHk == null) return;
            if (_AppHk.CurrentControl == null) return;
           // Plugin.LogTable.Writelog(Caption);//xisheng 日志记录07.08
            _cmd.OnCreate(_AppHk.CurrentControl);
            _cmd.OnClick();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;

            _cmd = new CommandOpenDocClass();
            _cmd.OnCreate(_AppHk.MapControl);
        }
    }
}
