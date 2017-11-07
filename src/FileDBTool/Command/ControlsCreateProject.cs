using System;
using System.Collections.Generic;
using System.Text;


namespace FileDBTool
{
    /// <summary>
    /// 在指定的库中新建项目
    /// </summary>
    public class ControlsCreateProject: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsCreateProject()
        {
            base._Name = "FileDBTool.ControlsCreateProject";
            base._Caption = "新建项目";
            base._Tooltip = "新建项目";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "新建项目";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //若没有选中库体节点，则不可用

                if ("DATABASE" != m_Hook.ProjectTree.SelectedNode.DataKey.ToString())
                    return false;
                if ("文件连接" != m_Hook.ProjectTree.SelectedNode.Name)
                    return false;
                if(m_Hook.ProjectTree.SelectedNode.ImageIndex==1) return false;

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
            //执行新建项目操作
            string text = "新建项目";
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
