using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBIntegration
{
    /// <summary>
    /// chenyafei  20110314  add  content： 添加角色
    /// </summary>
    public class ControlsAddRole : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppDBIntegraRef m_Hook;
        public ControlsAddRole()
        {
            base._Name = "GeoDBIntegration.ControlsAddRole";
            base._Caption = "添加角色";
            base._Tooltip = "添加角色";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "添加角色信息";
        }

        public override bool Enabled
        {
            get 
            {
                //超级管理员具备添加角色的功能，普通用户不具备添加用户的权限
                if (ModuleData.m_User==null) return false;
                //cyf 20110602 modify
                //if (ModuleData.m_User.RoleTypeID != EnumRoleType.系统管理员.GetHashCode()) return false;
                //end
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
            //添加角色
            AddGroup frmGroup = new AddGroup(false);
            if (frmGroup.beSucceed)
            {
                frmGroup.ShowDialog();
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
        }

    }
}
