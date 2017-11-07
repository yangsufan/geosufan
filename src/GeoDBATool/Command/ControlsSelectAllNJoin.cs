using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    class ControlsSelectAllNJoin : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        private string DataGridNo = string.Empty;
        public ControlsSelectAllNJoin()
        {
            base._Name = "GeoDBATool.ControlsSelectAllNJoin";
            base._Caption = "全部选择未接边";
            base._Tooltip = "全部选择";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "全部选择";

        }

        public override bool Enabled
        {
            get
            {
                //处理界面的时候不显示 20110829 xisheng
                if (m_Hook.JoinMergeResultGrid.Name != null)
                {
                    if (m_Hook.JoinMergeResultGrid.Name.Trim() == "ActiveMenu")
                        return false;
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
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
                //DevComponents.DotNetBar.Controls.CheckBoxX getCheck = null;
                //if (GetDataGrid.Rows[i].Cells[0].Value == null)
                //    continue;
                //getCheck= GetDataGrid.Rows[i].Cells[0].Value as DevComponents.DotNetBar.Controls.CheckBoxX;
                if (GetDataGrid.Rows[i].Cells[GetDataGrid.ColumnCount - 1].Value.ToString().Trim() == "未接边")//判断如果是未接边则选中 20110829 xisheng
                {
                    GetDataGrid.Rows[i].Cells[0].Value = true;
                }
            }
         }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
            
        }
    }
}
