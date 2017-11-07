using System;
using System.Collections.Generic;
using System.Text;

namespace GeoEdit
{
    public class LabelLayer : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        public LabelLayer()
        {
            base._Name = "GeoEdit.LabelLayer";
            base._Caption = "当前操作图层";
            base._Tooltip = "当前操作图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "当前操作图层";

        }

        public override void OnClick()
        {

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppGISRef;
            if (myHook.MapControl == null) return;
        }
    }
}
