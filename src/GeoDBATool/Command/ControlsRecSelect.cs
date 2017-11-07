using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    class ControlsRecSelect : Plugin.Interface.CommandRefBase
    {
         private Plugin.Application.IAppGISRef m_Hook;

         public ControlsRecSelect()
        {
            base._Name = "GeoDBATool.ControlsRecSelect";
            base._Caption = "反选";
            base._Tooltip = "反选";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "反选";

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
            for (int i = 0; i < GetDataGrid.Rows.Count; i++)
            {
                if (GetDataGrid.Rows[i].Cells[0].Value == null)
                    GetDataGrid.Rows[i].Cells[0].Value = false;
                else if ((bool)GetDataGrid.Rows[i].Cells[0].Value == false)
                    GetDataGrid.Rows[i].Cells[0].Value = true;
                else
                    GetDataGrid.Rows[i].Cells[0].Value = false;
            }
         }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
            
        }
    }
}
