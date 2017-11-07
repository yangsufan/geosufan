using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 关闭连接
    /// </summary>
    public class ControlsDisConnection: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsDisConnection()
        {
            base._Name = "FileDBTool.ControlsDisConnection";
            base._Caption = "关闭连接";
            base._Tooltip = "关闭连接";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "关闭连接";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() != EnumTreeNodeType.DATABASE.ToString()) return false;
                if (m_Hook.ProjectTree.SelectedNode.ImageIndex != 2) return false;
              

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
            DevComponents.AdvTree.Node SelNode = m_Hook.ProjectTree.SelectedNode;
            SelNode.Nodes.Clear();
            SelNode.ImageIndex=1;
            m_Hook.DataInfoGrid.DataSource = null;
            m_Hook.MapControl.Map.ClearLayers();
            m_Hook.MapControl.ActiveView.Refresh();
            m_Hook.MetaDataGrid.DataSource = null;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
    }
}
