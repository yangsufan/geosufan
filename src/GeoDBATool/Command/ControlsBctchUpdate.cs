using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBATool
{
    public class ControlsBctchUpdate : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsBctchUpdate()
        {
            base._Name = "GeoDBATool.ControlsBctchUpdate";
            base._Caption = "批量更新";
            base._Tooltip = "批量更新";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "批量更新";

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
            if (m_Hook == null)
            {
                return;
            }
            FrmBatchUpdate pFrm = new FrmBatchUpdate(m_Hook);
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//ygc 2012-9-14 写日志
            }
            pFrm.ShowDialog();
            pFrm = null;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
