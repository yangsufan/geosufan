using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 删除指定的产品及相关信息
    /// </summary>
    public class ControlsDelProduct: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsDelProduct()
        {
            base._Name = "FileDBTool.ControlsDelProduct";
            base._Caption = "删除产品";
            base._Tooltip = "删除产品及所有产品相关的信息";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除产品及所有产品相关的信息";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //若没有选中产品节点，则不可用
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
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
            SysCommon.Error.frmInformation eerorFrm = new SysCommon.Error.frmInformation("是", "否", "删除产品将删除产品下的所有数据文件，确定吗？");
            eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
            if (eerorFrm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            //执行删除产品操作
            Exception ex = null;
            ModDBOperator.DelProduct(m_Hook.ProjectTree.SelectedNode, out ex);
            if (null != ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示！", ex.Message);
                return;
            }
            else
            {
                m_Hook.ProjectTree.SelectedNode.Remove();
                m_Hook.DataInfoGrid.DataSource = null;
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
    }
}

