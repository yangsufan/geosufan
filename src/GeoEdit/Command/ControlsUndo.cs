using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;

namespace GeoEdit
{
    /// <summary>
    /// 撤消
    /// </summary>
    public class ControlsUndo : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        public ControlsUndo()
        {
            base._Name = "GeoEdit.ControlsUndo";
            base._Caption = "撤消";
            base._Tooltip = "撤消";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = false;
            base._Message = "撤消";

        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (m_Hook == null) return false;
                    if (m_Hook.MapControl == null) return false;
                    if (MoData.v_CurWorkspaceEdit == null) return false;

                    bool bHasRedos = false;
                    MoData.v_CurWorkspaceEdit.HasUndos(ref bHasRedos);
                    return bHasRedos;
                }
                catch
                {
                    return false;
                }
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
            MoData.v_CurWorkspaceEdit.UndoEditOperation();
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
            if (this.Enabled && e.shift == 2 && e.keyCode == 90)
            {
                this.OnClick();
            }
        }
    }
}
