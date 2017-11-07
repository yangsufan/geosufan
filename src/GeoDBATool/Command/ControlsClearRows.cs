using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    class ControlsClearRows: Plugin.Interface.CommandRefBase
    {
          private Plugin.Application.IAppGISRef m_Hook;

        public ControlsClearRows()
        {
            base._Name = "GeoDBATool.ControlsClearRows";
            base._Caption = "清除行";
            base._Tooltip = "清除行";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "清除行";

        }

        public override bool Enabled
        {
            get
            {
                return true;
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
            DevComponents.DotNetBar.Controls.DataGridViewX GetDataGrid = null;

            if (m_Hook.PolylineSearchGrid.Name != null)
            {
                if (m_Hook.PolylineSearchGrid.Name.Trim() == "ActiveMenu")
                    GetDataGrid = m_Hook.PolylineSearchGrid;
            }
            if (m_Hook.PolygonSearchGrid.Name != null)
            {
                if (m_Hook.PolygonSearchGrid.Name.Trim() == "ActiveMenu")
                    GetDataGrid = m_Hook.PolygonSearchGrid;
            }
            if (m_Hook.JoinMergeResultGrid.Name != null)
            {
                if (m_Hook.JoinMergeResultGrid.Name.Trim() == "ActiveMenu")
                    GetDataGrid = m_Hook.JoinMergeResultGrid;
            }
            if (null == GetDataGrid)
                return;
            GetDataGrid.DataSource = null;
         }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
            
        }
    }
}
