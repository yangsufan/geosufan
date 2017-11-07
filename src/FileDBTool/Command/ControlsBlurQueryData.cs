using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 模糊查询
    /// </summary>
    public class ControlsBlurQueryData:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;
        DevComponents.AdvTree.Node v_DBNode = null;  //数据库数节点
        public ControlsBlurQueryData()
        {
            base._Name = "FileDBTool.ControlsBlurQueryData";
            base._Caption = "模糊查询";
            base._Tooltip = "模糊查询";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "模糊查询";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.ImageIndex == 1) return false;//若未连接上，则不能查询
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() == EnumTreeNodeType.DATACONNECT.ToString()) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() == EnumTreeNodeType.DATAITEM.ToString()) return false;

                v_DBNode = m_Hook.ProjectTree.SelectedNode;
                while (v_DBNode.Parent != null)
                {
                    v_DBNode = v_DBNode.Parent;
                }
                if (v_DBNode.Name != "文件连接") return false;
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
            FrmBlurQuery pFrmQueryAll = new FrmBlurQuery(m_Hook,v_DBNode);
           pFrmQueryAll.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }


    
    }
}
