using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoEdit
{
    public class ControlsCopy : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        public ControlsCopy()
        {
            base._Name = "GeoEdit.ControlsCopy";
            base._Caption = "¸´ÖÆ";
            base._Tooltip = "¸´ÖÆ";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "¸´ÖÆ";

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

            Dictionary<IGeometry, IElement> ges = new Dictionary<IGeometry, IElement>();
            IEnumFeature features = myHook.MapControl.Map.FeatureSelection as IEnumFeature;
            features.Reset();
            IFeature feature = features.Next();
            while (feature != null)
            {
                IElement ele = null;
                if (feature is IAnnotationFeature)
                {
                    IAnnotationFeature af = feature as IAnnotationFeature;
                    ele = af.Annotation;
                }

                ges.Add(feature.ShapeCopy, ele);
                feature = features.Next();
            }

            ControlsPaste.GeometriesToBePasted = ges;
        }

        public override bool Enabled
        {
            get
            {
                if (myHook == null) return false;
                if (myHook.MapControl == null) return false;
                if (MoData.v_CurWorkspaceEdit == null) return false;
                if (myHook.MapControl.Map.SelectionCount == 0) return false;
                return true;
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppGISRef;
            myHook.ArcGisMapControl.OnKeyDown+=new IMapControlEvents2_Ax_OnKeyDownEventHandler(ArcGisMapControl_OnKeyDown);
        }

        private void ArcGisMapControl_OnKeyDown(object sender, IMapControlEvents2_OnKeyDownEvent e)
        {
            if (this.Enabled && e.shift == 2 && e.keyCode == 67)
            {
                this.OnClick();
            }
        }

    }
}
