using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SysCommon.Authorize;
using SysCommon.Error;

namespace GeoDBIntegration
{
    /// <summary>
    /// chenyafei 20110311 add content： 修改用户，用户只能修改用户自己的信息
    /// </summary>
    public class ControlsModifyUser : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppDBIntegraRef m_Hook;

        public ControlsModifyUser()
        {
            base._Name = "GeoDBIntegration.ControlsModifyUser";
            base._Caption = "修改用户";
            base._Tooltip = "修改用户";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "修改用户信息";
        }

        public override bool Enabled
        {
            get
            {
                //有普通用户登录时，该按钮才可用
                if (ModuleData.m_User == null) return false;
                //if (ModuleData.m_User.RoleTypeID == EnumRoleType.系统管理员.GetHashCode()) return false;
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
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
            AddUser frmUser = new AddUser(ModuleData.m_User);
            if (frmUser.BeSuccedd)
            {
                frmUser.ShowDialog();
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
        }

    }
}
