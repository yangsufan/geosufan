using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 修改指定产品属性
    /// </summary>
    class ControlsEditProduct: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsEditProduct()
        {
            base._Name = "FileDBTool.ControlsEditProduct";
            base._Caption = "产品属性";
            base._Tooltip = "查看修改产品属性";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "查看修改产品属性";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
                //若没有选中产品节点，则不可用
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() != EnumTreeNodeType.PRODUCT.ToString()) return false;

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
            //执行修改产品属性操作
            string text = "修改产品";
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

