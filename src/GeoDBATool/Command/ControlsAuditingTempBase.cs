using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBATool
{
    public class ControlsAuditingTempBase : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsAuditingTempBase()
        {
            base._Name = "GeoDBATool.ControlsAuditingTempBase";
            base._Caption = "临时库审核";
            base._Tooltip = "临时库审核";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "临时库审核";

        }

        public override bool Enabled
        {
            get
            {
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
            Plugin.Application.AppGIS pApp = m_Hook as Plugin.Application.AppGIS;
            Plugin.Application.IAppFormRef pAppFrm = m_Hook as Plugin.Application.IAppFormRef;
            if (pApp == null)
            {
                return;
            }
            FrmTmpDataCheck pFrm = new FrmTmpDataCheck(pApp.ProjectTree);
            pFrm.Show(pAppFrm.MainForm);
            pFrm = null;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
