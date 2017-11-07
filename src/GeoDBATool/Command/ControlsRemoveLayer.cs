using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;

namespace GeoDBATool
{
    public class ControlsRemoveLayer : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;
        public ControlsRemoveLayer()
        {
            base._Name = "GeoDBATool.ControlsRemoveLayer";
            base._Caption = "ÒÆ³ýÍ¼²ã";
            base._Tooltip = "ÒÆ³ýµ±Ç°Í¼²ã";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "ÒÆ³ýµ±Ç°Í¼²ã";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                ILayer pLayer = (ILayer)_AppHk.MapControl.CustomProperty;
                if (pLayer == null) return false;
                if (pLayer.Name == "·¶Î§" ||pLayer.Name=="ÈÎÎñÇø·¶Î§" || pLayer.Name.StartsWith("Í¼·ù·¶Î§")) return false;
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

            ILayer mLayer = _AppHk.MapControl.CustomProperty as ILayer;
            if (mLayer == null) return;
            _AppHk.MapControl.Map.DeleteLayer(mLayer);
                        
            _AppHk.MapControl.ActiveView.Refresh();
            _AppHk.TOCControl.Update();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
        }
    }
}
