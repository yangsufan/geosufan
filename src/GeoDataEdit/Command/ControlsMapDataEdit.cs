using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataEdit
{
    public class ControlsMapDataEdit : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        public ControlsMapDataEdit()
        {
            base._Name = "GeoDataEdit.ControlsMapDataEdit";
            base._Caption = "数据编辑";
            base._Tooltip = "数据编辑";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "数据编辑";
        }
        //public override bool Enabled
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (_AppHk.MapControl.Map.LayerCount == 0)
        //            {
        //                base._Enabled = false;
        //                return false;
        //            }

        //            base._Enabled = true;
        //            return true;
        //        }
        //        catch
        //        {
        //            base._Enabled = false;
        //            return false;
        //        }
        //    }
        //}

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
            if (_AppHk.MapControl == null) return;
            MapDataEdit pMapDataEdit = new MapDataEdit();
            pMapDataEdit.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}
