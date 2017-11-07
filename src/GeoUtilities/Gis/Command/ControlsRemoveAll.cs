using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;

namespace GeoUtilities
{
    public class ControlsRemoveAll : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        public ControlsRemoveAll()
        {
            base._Name = "GeoUtilities.ControlsRemoveAll";
            base._Caption = "移除";
            base._Tooltip = "移除所有图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "移除视图上所有图层数据";
            //base._Image = "";
            //base._Category = "";
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
            ILayer pLayTemp = null;
            while(_AppHk.MapControl.LayerCount>0)
            {
                ILayer mLayer = _AppHk.MapControl.get_Layer(0);
                if (mLayer is IGroupLayer && mLayer.Name == "范围")
                {
                    pLayTemp = mLayer;
                }
                _AppHk.MapControl.Map.DeleteLayer(mLayer);
            }

            if (pLayTemp != null)
            {
                _AppHk.MapControl.Map.AddLayer(pLayTemp);
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(base._Tooltip);//xisheng 2011.07.08 增加日志
            }
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
