using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    public class LabelScaleSel: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef myHook;
        public LabelScaleSel()
        {
            base._Name = "FileDBTool.LabelScaleSel";
            base._Caption = "比例尺：";
            base._Tooltip = "比例尺：";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "比例尺：";

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
