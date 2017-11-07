using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
namespace GeoUtilities
{
    public class ControlsSetLimitScale1 : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFormRef _AppHk;
        public ControlsSetLimitScale1()
        {
            base._Name = "GeoUtilities.ControlsSetLimitScale1";
            base._Caption = "设置限制比例尺";
            base._Tooltip = "设置限制比例尺";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "设置限制比例尺";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.LayerAdvTree==null) return false;
                if (_AppHk.MapControl == null) return false;
                DevComponents.AdvTree.AdvTree pTree = _AppHk.LayerAdvTree as DevComponents.AdvTree.AdvTree;
                if (pTree == null)
                {
                    return false;
                }
                if (pTree.SelectedNode == null)
                {
                    return false;
                }
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
            if (_AppHk == null)
            {
                return;
            }
            if (_AppHk.LayerAdvTree == null)
            {
                return;
            }
            if (_AppHk.MapControl == null)
            {
                return;
            }
            DevComponents.AdvTree.AdvTree pTree = _AppHk.LayerAdvTree as DevComponents.AdvTree.AdvTree;
            if (pTree == null)
            {
                return;
            }
            if (pTree.SelectedNode == null)
            {
                return;
            }
            DevComponents.AdvTree.Node pNode = pTree.SelectedNode;
            IMapControlDefault pMapControl = _AppHk.MapControl as IMapControlDefault;
            FormSetLimitScale pFrm = new FormSetLimitScale(pNode, pMapControl,"");
            DialogResult pRes= pFrm.ShowDialog();
            if (pRes == DialogResult.OK)
            {
                (pMapControl.Map as IActiveView).Refresh();
            }

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppFormRef ;
        }
    }
}

