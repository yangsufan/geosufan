using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBATool
{
    /// <summary>
    /// 陈亚飞添加,导入xml文件用以更新数据
    /// </summary>
    public class ControlsInputUpdateData : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsInputUpdateData()
        {
            base._Name = "GeoDBATool.ControlsInputUpdateData";
            base._Caption = "导入更新数据";
            base._Tooltip = "导入更新数据以更新数据库";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "导入更新数据以更新数据库";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "project") return false;
                if (m_Hook.ProjectTree.SelectedNode.Tag == null) return false;
                XmlNode ProNode = m_Hook.ProjectTree.SelectedNode.Tag as XmlNode;
                if (ProNode == null) return false;
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
            //清空处理树图
            if (m_Hook.DataTree.Nodes != null)
            {
                m_Hook.DataTree.Nodes.Clear();
            }
            Plugin.Application.IAppFormRef pAppForm = m_Hook as Plugin.Application.IAppFormRef;
          
            frmInputUpdateData frmSubmitData = new frmInputUpdateData(m_Hook);
            frmSubmitData.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
