using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBATool
{
    public class ControlsSubmitMapFramData: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsSubmitMapFramData()
        {
            base._Name = "GeoDBATool.ControlsSubmitMapFramData";
            base._Caption = "提交图幅数据";
            base._Tooltip = "提交图幅数据";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "提交图幅数据";

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
            frmDataSubmit frmDataSubmit = new frmDataSubmit(EnumOperateType.Submit, m_Hook);
            frmDataSubmit.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}