using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace GeoSysUpdate
{
    [Guid("1f398028-96f6-4d96-b1a5-3faa32ac161d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoSysUpdate.SettingManager.CurDBConInfoSetting")]
    public class CurDBConInfoSetting : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public CurDBConInfoSetting()
        {
            base._Name = "GeoSysUpdate.CurDBConInfoSetting";
            base._Caption = "用地库密码管理";
            base._Tooltip = "用地库密码管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "用地库密码管理";
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
                Plugin.Application.IAppPrivilegesRef pAppFormRef = m_Hook as Plugin.Application.IAppPrivilegesRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppPrivilegesRef pAppFormRef = m_Hook as Plugin.Application.IAppPrivilegesRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            //Control.frmEditCurDbInfo vCurDBInfo = new GeoSysUpdate.Control.frmEditCurDbInfo();
            //vCurDBInfo.GisDB = ModData.v_SysDataSet;
            //vCurDBInfo.ShowDialog();
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
