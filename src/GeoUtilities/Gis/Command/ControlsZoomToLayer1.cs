using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace GeoUtilities
{
    public class ControlsZoomToLayer1 : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        public ControlsZoomToLayer1()
        {
            base._Name = "GeoUtilities.ControlsZoomToLayer1";
            base._Caption = "缩放到图层";
            base._Tooltip = "缩放到图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "缩放到图层";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                ILayer mLayer = _AppHk.MapControl.CustomProperty as ILayer;
                if (mLayer == null) return false;
                if (mLayer is IDynamicLayer) return false;
                return true;
            }
        }
        public override bool Visible
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    if (pAppFormRef.LayerAdvTree != null)
                    {
                        DevComponents.AdvTree.AdvTree pTree = pAppFormRef.LayerAdvTree as DevComponents.AdvTree.AdvTree;
                        if (pTree.SelectedNode != null)
                        {
                            DevComponents.AdvTree.Node pNode = pTree.SelectedNode;
                            if (pNode != null)
                            {
                                if (pNode.Tag != null)
                                {
                                    string strtag = pNode.Tag.ToString();
                                    if (strtag.ToLower().Contains("layer"))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
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
            pLayer = (ILayer)_AppHk.MapControl.CustomProperty;
            if (pLayer == null) return;
            IActiveView pActiveView = _AppHk.MapControl.Map as IActiveView;
            pActiveView.Extent = pLayer.AreaOfInterest;
            pActiveView.Refresh();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
        }
    }
}
