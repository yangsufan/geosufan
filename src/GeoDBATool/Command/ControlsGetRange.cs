using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBATool
{
    /// <summary>
    /// 陈亚飞添加
    /// </summary>
    public class ControlsGetRange: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsGetRange()
        {
            base._Name = "GeoDBATool.ControlsGetRange";
            base._Caption = "获取图幅数据范围";
            base._Tooltip = "获取图幅数据范围";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "获取图幅数据范围";

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
            FrmGetRange pFrmGetRange = new FrmGetRange(m_Hook);
            pFrmGetRange.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
