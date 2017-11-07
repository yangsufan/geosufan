using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoEdit
{
    public class ControlsDelete : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        public ControlsDelete()
        {
            base._Name = "GeoEdit.ControlsDelete";
            base._Caption = "É¾³ý";
            base._Tooltip = "É¾³ý";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "É¾³ý";

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
            MoData.v_CurWorkspaceEdit.StartEditOperation();
            IEnumFeature features = myHook.MapControl.Map.FeatureSelection as IEnumFeature;
            features.Reset();
            IFeature feature = features.Next();
            while (feature != null)
            {
                IDataset pDataset = feature.Class as IDataset;
                IWorkspaceEdit pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                if (pWorkspaceEdit.IsBeingEdited())
                {
                    feature.Delete();
                }
                feature = features.Next();
            }
            MoData.v_CurWorkspaceEdit.StopEditOperation();
            myHook.MapControl.ActiveView.Refresh();
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppGISRef;
            myHook.ArcGisMapControl.OnKeyDown += new IMapControlEvents2_Ax_OnKeyDownEventHandler(ArcGisMapControl_OnKeyDown);
        }

        private void ArcGisMapControl_OnKeyDown(object sender, IMapControlEvents2_OnKeyDownEvent e)
        {
            if (this.Enabled && e.keyCode == 46)
            {
                this.OnClick();
            }
        }

    }
}
