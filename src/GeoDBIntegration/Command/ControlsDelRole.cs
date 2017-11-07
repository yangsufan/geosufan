using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBIntegration
{
    /// <summary>
    /// chenyafei  20110314 add content: 删除角色
    /// </summary>
    public class ControlsDelRole: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppDBIntegraRef m_Hook;
        public ControlsDelRole()
        {
            base._Name = "GeoDBIntegration.ControlsDelRole";
            base._Caption = "删除角色";
            base._Tooltip = "删除角色";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除角色";
        }

        public override bool Enabled
        {
            get 
            {
                //超级管理员才具备删除角色的权限
                if (ModuleData.m_User == null) return false;
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
            //删除角色
            DelGroup pDelGroup = new DelGroup(true);
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
        }

    }
}
