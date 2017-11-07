using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    public class ControlsCreateTempDB4Update : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsCreateTempDB4Update()
        {
            base._Name = "GeoDBATool.ControlsCreateTempDB4Update";
            base._Caption = "初始化更新环境库";
            base._Tooltip = "初始化更新环境库";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "初始化更新环境库";

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
            frmInitUpdateEnviroment InitUpdateEnv = new frmInitUpdateEnviroment(m_Hook);
            InitUpdateEnv.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
