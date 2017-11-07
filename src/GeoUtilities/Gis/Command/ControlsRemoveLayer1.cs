using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace GeoUtilities
{
    public class ControlsRemoveLayer1 : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFormRef _AppHk;
        public ControlsRemoveLayer1()
        {
            base._Name = "GeoUtilities.ControlsRemoveLayer1";
            base._Caption = "移除图层";
            base._Tooltip = "移除当前图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "移除当前图层";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                ILayer pLayer = (ILayer)_AppHk.MapControl.CustomProperty;
                if (pLayer == null) return false;
                if (pLayer.Name == "示意图" ||pLayer.Name=="Default" || pLayer.Name.StartsWith("MapFrame_")) return false;
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
            Plugin.Application.IAppFormRef pAppFrm = _AppHk as Plugin.Application.IAppFormRef;
            object pLayerTree = pAppFrm.LayerTree;
            _AppHk.MapControl.Map.DeleteLayer(mLayer);
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption + mLayer.Name);//xisheng 2011.07.08 增加日志
            }
            _AppHk.MapControl.ActiveView.Refresh();
            _AppHk.TOCControl.Update();

            //更新图库树

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppFormRef ;
        }
    }
}
