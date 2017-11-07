using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace GeoUserManager
{
    public class CommandSaveSymbol : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef _AppHk;

        public CommandSaveSymbol()
        {
            base._Name = "GeoUserManager.CommandSaveSymbol";
            base._Caption = "保存符号";
            base._Tooltip = "保存符号";
            base._Checked = false;
            base._Visible = true;
            base._Enabled =false;
            base._Message = "保存符号";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
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
            IFeatureLayer pLayer = (IFeatureLayer)_AppHk.MapControl.CustomProperty;
            if (pLayer == null) return;

            //if(pLayer is IFeatureLayer
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;

        }
    }
}
