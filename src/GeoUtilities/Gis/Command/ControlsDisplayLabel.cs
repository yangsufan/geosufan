using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace GeoUtilities
{
    public class ControlsDisplayLabel : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        public ControlsDisplayLabel()
        {
            base._Name = "GeoUtilities.ControlsDisplayLabel";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Caption = "ÒÆ³ý±ê×¢";
            base._Tooltip = "ÒÆ³ýÍ¼²ã±ê×¢";
            base._Message = "ÒÆ³ýÍ¼²ã±ê×¢";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                ILayer mLayer = _AppHk.MapControl.CustomProperty as ILayer;
                if (mLayer is IGeoFeatureLayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }   
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
            ILayer pLayer = null;
            pLayer = _AppHk.MapControl.CustomProperty as ILayer;
            if (pLayer == null) return;
            IGeoFeatureLayer pGeoFeatureLayer = null;
            if (pLayer is IGeoFeatureLayer)
            {
                pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
                if (pGeoFeatureLayer.DisplayAnnotation == true)
                {
                    pGeoFeatureLayer.DisplayAnnotation = false;   
                }
                _AppHk.MapControl.ActiveView.Refresh();
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Message);//xisheng 2011.07.08 Ôö¼ÓÈÕÖ¾
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
        }
    }
}
