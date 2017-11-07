using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSysSetting
{
    class ControlsUserManager: Plugin.Interface.CommandRefBase
    {
        private GeoSysSetting.SubControl.UCDataSourceManger ucCtl = null;
        //changed by chulili 20110722启动于数据源界面 IAppPrivilegesRef->IAppDBIntegraRef
        private Plugin.Application.IAppDBIntegraRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;

        public ControlsUserManager()
        {
            base._Name = "GeoSysSetting.ControlsUserManager";
            base._Caption = "用户管理";
            base._Tooltip = "用户管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "用户管理";
        }

        public override bool Enabled
        {
            get
            {
                if (_hook == null) return true;
                //根据时间刷新一下改空间是否显示
                if (_hook.Visible == false && this.ucCtl != null)
                {
                    this.ucCtl.Visible = false;
                }
                return true;
            }
        }

        public override string Message
        {
            get
            {//changed by chulili 20110722启动于数据源界面 IAppPrivilegesRef->IAppFormRef
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
            //changed by chulili 20110722启动于数据源界面 IAppPrivilegesRef->IAppFormRef
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            //Exception eError;

            //if (ucCtl == null)
            //{
            //    ucCtl = new SubControl.UCDataSourceManger();
            //    ucCtl.Dock = DockStyle.Fill;
            //    ucCtl.m_TmpWorkSpace = _hook.TempWksInfo.Wks;
                
                
            //    _hook.MainForm.Controls.Add(ucCtl);
            //}

            //ucCtl.Visible = true;
            //_hook.MainForm.Controls.SetChildIndex(ucCtl, 0);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            _hook = hook as Plugin.Application.IAppFormRef;


        }

    }
}
