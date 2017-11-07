using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoEdit
{
    public class ControlsSplit : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        public ControlsSplit()
        {
            base._Name = "GeoEdit.ControlsSplit";
            base._Caption = "ÏßÇÐ¸î";
            base._Tooltip = "ÏßÇÐ¸î";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "ÏßÇÐ¸î";

        }

        public override bool Enabled
        {
            get
            {
                if (myHook == null) return false;
                if (myHook.MapControl == null) return false;
                if (MoData.v_CurWorkspaceEdit == null) return false;
                if (myHook.MapControl.Map.SelectionCount != 1) return false;

                IEnumFeature features = myHook.MapControl.Map.FeatureSelection as IEnumFeature;
                features.Reset();
                IFeature feature = features.Next();
                if (feature.Shape.GeometryType == esriGeometryType.esriGeometryLine || feature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline) return true;
                return false;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = myHook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = myHook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
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
