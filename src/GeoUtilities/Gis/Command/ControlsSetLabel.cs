using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;


namespace GeoUtilities
{
    public class ControlsSetLabel : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        public ControlsSetLabel()
        {
            base._Name = "GeoUtilities.ControlsSetLabel";
            base._Caption = "设置标注";
            base._Tooltip = "设置图层标注";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "设置图层标注";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                ILayer pLayer = null;
                pLayer = _AppHk.MapControl.CustomProperty as ILayer;
                if (pLayer is IGeoFeatureLayer)
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
            frmSetLabel pFrmSetLabel = new frmSetLabel();
            ILayer pLayer = null;
            pLayer = _AppHk.MapControl.CustomProperty as ILayer;
            if (pLayer == null) return;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(base.Caption);//xisheng 2011.07.08 增加日志
            }
            if (pLayer is IGeoFeatureLayer)
            {
                pFrmSetLabel.GeoFeatLayer = pLayer as IGeoFeatureLayer;
                pFrmSetLabel.MapControl = _AppHk.MapControl;
                pFrmSetLabel.ShowDialog();

            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
        }
    }
}
