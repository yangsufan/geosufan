using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace GeoUtilities
{
    public class ControlsAddGroupLayer1 : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFormRef _AppHk;
        public ControlsAddGroupLayer1()
        {
            base._Name = "GeoUtilities.ControlsAddGroupLayer1";
            base._Caption = "添加图层组";
            base._Tooltip = "添加图层组";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "添加图层组";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false; 
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
            IGroupLayer pGroupFLayer = new GroupLayer();

            pGroupFLayer.Name = "新建图层组";
            pGroupFLayer.Visible = true;
            _AppHk.MapControl.AddLayer(pGroupFLayer, 0);

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppFormRef ;
        }
    }
}
