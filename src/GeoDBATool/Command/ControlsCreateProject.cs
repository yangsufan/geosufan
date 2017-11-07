using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    public class ControlsCreateProject : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsCreateProject()
        {
            base._Name = "GeoDBATool.ControlsCreateProject";
            base._Caption = "新建";
            base._Tooltip = "新建数据库工程";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "新建数据库工程";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
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
            frmProject newFrm = new frmProject(m_Hook.ProjectTree,true);
            newFrm.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
