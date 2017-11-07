using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace GeoDBATool
{
    public class ControlsDelProject : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsDelProject()
        {
            base._Name = "GeoDBATool.ControlsDelProject";
            base._Caption = "删除";
            base._Tooltip = "删除数据库工程";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除数据库工程";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "project") return false;
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
             SysCommon.Error.frmInformation frmInfo = new SysCommon.Error.frmInformation("是", "否", "是否删除数据库工程节点？");
             if (frmInfo.ShowDialog() == DialogResult.OK)
             {
                 XmlElement aElement = m_Hook.ProjectTree.SelectedNode.Tag as XmlElement;
                 ProjectXml.DelTreeNode(aElement.OwnerDocument, m_Hook.ProjectTree.SelectedNode);
             }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
