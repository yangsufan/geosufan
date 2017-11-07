using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBATool
{
    /// <summary>
    /// 陈亚飞添加
    /// </summary>
    public class ControlsStractData: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsStractData()
        {
            base._Name = "GeoDBATool.ControlsStractData";
            base._Caption = "获取相交要素";
            base._Tooltip = "获取相交要素";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "获取相交要素";

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
                XmlElement workDBElem = ProNode.SelectSingleNode(".//内容//图幅工作库//范围信息") as XmlElement;
                if (workDBElem == null) return false;
                if (!workDBElem.HasAttribute("范围")) return false;
                string rangeStr = workDBElem.GetAttribute("范围").Trim();
                if (rangeStr == "") return false;
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
            frmDataTransplant newFrm = new frmDataTransplant("获取相交要素", EnumOperateType.Stract, m_Hook, m_Hook.ProjectTree.SelectedNode.Tag as XmlElement);
            newFrm.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
