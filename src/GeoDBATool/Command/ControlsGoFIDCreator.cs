using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    public class ControlsGoFIDCreator : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsGoFIDCreator()
        {
            base._Name = "GeoDBATool.ControlsGoFIDCreator";
            base._Caption = "初始化业务维护信息";
            base._Tooltip = "初始化业务维护信息";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "初始化业务维护信息";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if(m_Hook.ProjectTree.SelectedNode == null) return false;
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
            frmInitCaseEnviroment InitUpdateEnv = new frmInitCaseEnviroment(m_Hook);
            InitUpdateEnv.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
