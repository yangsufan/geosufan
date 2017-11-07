using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 数据归档
    /// </summary>
    public class ControlsSaveData:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsSaveData()
        {
            base._Name = "FileDBTool.ControlsSaveData";
            base._Caption = "数据归档";
            base._Tooltip = "数据归档";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "数据归档";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;

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
            //执行数据归档

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
    }
}
