using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 在指定的项目下新建产品
    /// </summary>
    public class ControlsCreateProduct: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsCreateProduct()
        {
            base._Name = "FileDBTool.ControlsCreateProduct";
            base._Caption = "新建产品";
            base._Tooltip = "新建产品";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "新建产品";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //若没有选中项目节点，则不可用
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
                if (EnumTreeNodeType.DATAFORMAT.ToString() != m_Hook.ProjectTree.SelectedNode.DataKey.ToString()) return false;
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
            //执行新建产品操作（选择产品总范围、成果数据类型节点）
            string text = "新建产品";
            FrmProjectSetting frmProSetting = new FrmProjectSetting(m_Hook.ProjectTree.SelectedNode, text);
            frmProSetting.ShowDialog();


            //在产品节点上添加三个子节点（成果数据类型节点）：标准图幅、非标准图幅、控制点数据（控制点测量数据）

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
    }
}
