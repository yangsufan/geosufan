using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBATool
{
    public class ControlsInputData : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsInputData()
        {
            base._Name = "GeoDBATool.ControlsInputData";
            base._Caption = "导入数据";
            base._Tooltip = "导入数据以完成数据入库";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "导入数据以完成数据入库";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //cyf 20110625 modify
                if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "DB") return false;
                if (m_Hook.ProjectTree.SelectedNode.Text != "现势库") return false;  //cyf 20110708
                //if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "FD") return false;
                //end
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
            FrmImportData newFrm = new FrmImportData("数据入库", EnumOperateType.UserDBInput, m_Hook, m_Hook.ProjectTree.SelectedNode.Tag as XmlElement);
            newFrm.Show();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
