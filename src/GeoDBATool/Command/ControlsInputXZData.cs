using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBATool
{
    public class ControlsInputXZData : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsInputXZData()
        {
            base._Name = "GeoDBATool.ControlsInputXZData";
            base._Caption = "现状数据入库更新";
            base._Tooltip = "现状数据入库更新";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "现状数据入库更新";

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
            frmXZDataUpload newFrm = new frmXZDataUpload(Plugin.ModuleCommon.TmpWorkSpace);
            newFrm.WriteLog = this.WriteLog;//ygc 2012-9-12 是否写日志
            newFrm.Show();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
