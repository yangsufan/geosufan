using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 修改数据项及相关元信息
    /// </summary>
    public class ControlsEditData:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsEditData()
        {
            base._Name = "FileDBTool.ControlsEditData";
            base._Caption = "数据元信息修改";
            base._Tooltip = "修改成果数据项及其相关的元数据信息";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "修改成果数据项及其相关的元数据信息";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() != EnumTreeNodeType.DATAITEM.ToString()) return false;
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
            //执行成果数据修改操作（在列表中选中要修改的数据项，进行修改）
            string text = "修改数据元信息";
            FrmProjectSetting frmProSetting = new FrmProjectSetting(m_Hook.ProjectTree.SelectedNode, text);
            frmProSetting.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
    }
}
