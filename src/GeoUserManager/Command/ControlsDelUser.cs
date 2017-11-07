using System;
using System.Collections.Generic;
using System.Text;
using SysCommon.Authorize;
using SysCommon.Error;

namespace GeoUserManager
{
    public class ControlsDelUser : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;                //传入的窗体对象
        private User m_AppUser;                                             //当前登陆的用户

        public ControlsDelUser()
        {
            base._Name = "GeoUserManager.ControlsDelUser";
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
                if (m_Hook != null)
                {
                    if (m_Hook.MainForm.Controls[0] is UCRole)
                    {
                        UCRole pRole = m_Hook.MainForm.Controls[0] as UCRole;
                        if (pRole.UCtag == "User" && m_Hook.UserTree.SelectedNode != null)
                        {
                            if (ModuleOperator.GroupByName == "")
                            {
                                if (m_Hook.UserTree.SelectedNode.Level != 0)
                                    return true;
                            }
                            else
                            {
                                if (m_Hook.UserTree.SelectedNode.Level == 2)
                                    return true;
                            }
                        }
                    }
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
            if (m_Hook.UserTree.SelectedNode != null)
            {
                Exception eError;
                User user = m_Hook.UserTree.SelectedNode.Tag as User;
                if (user == null) return;
                if (m_AppUser == null) return;
                if (user.Name.ToLower() == "admin")
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "管理员不能删除！");
                    return;
                }
                if (user.IDStr  == m_AppUser.IDStr )
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "用户不能删除自己！");
                    return;
                }
                if (ErrorHandle.ShowFrmInformation("确认", "取消", "确认删除？"))
                {
                    //删除提取范围
                    ModuleOperator.DeleteData("USER_EXPORT", "userid", user.IDStr, ref ModData.gisDb, out eError);
                    if (ModuleOperator.DeleteData("user_info", "userid", user.IDStr, ref ModData.gisDb, out eError))
                    {
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
                        }
                        m_Hook.UserTree.SelectedNode.Remove();
                    }
                    else
                    {
                        if (eError != null)
                        {
                            ErrorHandle.ShowInform("提示", eError.Message);
                            return;
                        }
                    }
                }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
            m_AppUser = (m_Hook as Plugin.Application.IAppFormRef).ConnUser;
        }

    }
}
