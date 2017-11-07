using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;

namespace GeoEdit
{
    /// <summary>
    /// 重做
    /// </summary>

    class ControlsRedo : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        public ControlsRedo()
        {
            base._Name = "GeoEdit.ControlsRedo";
            base._Caption = "重做";
            base._Tooltip = "重做";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = false;
            base._Message = "重做";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.MapControl == null) return false;
                if (MoData.v_CurWorkspaceEdit == null) return false;

                bool bHasUndos = false;
                MoData.v_CurWorkspaceEdit.HasRedos(ref bHasUndos);
                return bHasUndos;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            MoData.v_CurWorkspaceEdit.RedoEditOperation();
            m_Hook.MapControl.ActiveView.Refresh();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppGISRef;
            m_Hook.ArcGisMapControl.OnKeyDown += new IMapControlEvents2_Ax_OnKeyDownEventHandler(ArcGisMapControl_OnKeyDown);
        }

        private void ArcGisMapControl_OnKeyDown(object sender, IMapControlEvents2_OnKeyDownEvent e)
        {
            if (this.Enabled && e.shift == 2 && e.keyCode == 82)
            {
                this.OnClick();
            }
        }
    }
}
