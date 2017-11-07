using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoUserManager
{
    public class ControlRoleManager : Plugin.Interface.CommandRefBase
    {
        private UserControl ucCtl = null;
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;

        public ControlRoleManager()
        {
            base._Name = "GeoUserManager.ControlRoleManager";
            base._Caption = "角色管理";
            base._Tooltip = "角色管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "角色管理";
        }

        public override bool Enabled
        {
            get
            {
                if (_hook == null) return true;
                //根据时间刷新一下该控件是否显示
                if (_hook.Visible == false && this.ucCtl != null)
                {
                    this.ucCtl.Visible = false;
                }
                if (_hook.MainForm.Controls[0] is UCRole)
                {
                    return true;
                }
                return false;
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
            if (ucCtl == null)
            {
                ucCtl = ModData.v_AppPrivileges.MainUserControl;
            }
            UCRole pRole = ucCtl as UCRole;
            pRole.ChangeUC("Role");
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
            _hook = hook as Plugin.Application.IAppFormRef;

        }

    }
}
