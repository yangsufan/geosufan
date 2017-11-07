using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    public class LabelTimeShow: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef myHook;
        public LabelTimeShow()
        {
            base._Name = "FileDBTool.LabelTimeShow";
            base._Caption = "显示时间：";
            base._Tooltip = "显示时间：";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "显示时间：";

        }

        public override void OnClick()
        {

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppFileRef;
            if (myHook.MapControl == null) return;
        }
    }
}
