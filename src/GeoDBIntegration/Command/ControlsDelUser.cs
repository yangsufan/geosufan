using System;
using System.Collections.Generic;
using System.Text;
using SysCommon.Authorize;
using SysCommon.Error;
using System.Windows.Forms;

namespace GeoDBIntegration
{
    /// <summary>
    /// chenyafei  20110311 add content： 删除用户，超级管理员具备删除用户的资格
    /// </summary>
    public class ControlsDelUser : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppDBIntegraRef m_Hook;                //传入的窗体对象
        private User m_AppUser;                                             //当前登陆的用户

        public ControlsDelUser()
        {
            base._Name = "GeoDBIntegration.ControlsDelUser";
            base._Caption = "删除用户";
            base._Tooltip = "删除用户";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除用户信息";
        }

        public override bool Enabled
        {
            get
            {
                //有普通用户登录，该按钮不可用
                if (ModuleData.m_User == null) return false;
                if (ModuleData.m_User.RoleTypeID == EnumRoleType.普通用户.GetHashCode() ) return false;
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
            //删除用户
            DelGroup pDelGroup = new DelGroup(false);
            if (pDelGroup.BeSuccedd)
            {
                pDelGroup.ShowDialog();
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            m_AppUser = (m_Hook as Plugin.Application.IAppFormRef).ConnUser;
        }

    }
}
