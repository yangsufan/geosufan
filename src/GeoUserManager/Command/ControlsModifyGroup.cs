using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SysCommon.Authorize;
using SysCommon.Error;

namespace GeoUserManager
{
    public class ControlsModifyGroup : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public ControlsModifyGroup()
        {
            base._Name = "GeoUserManager.ControlsModifyGroup";
            base._Caption = "修改角色";
            base._Tooltip = "修改角色";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "修改角色";
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
                        if (pRole.UCtag == "Role" && m_Hook.RoleTree.SelectedNode != null && m_Hook.RoleTree.SelectedNode.Level != 0)
                        {
                            return true;
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
            if (m_Hook.RoleTree.SelectedNode != null)
            {
                Exception eError;
                Role role = m_Hook.RoleTree.SelectedNode.Tag as Role;
                if (role == null) return;
                AddGroup frmGroup = new AddGroup(role);
                if (frmGroup.ShowDialog() == DialogResult.OK)
                {
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
                    }
                    ModuleOperator.DisplayRoleTree("", m_Hook.RoleTree,ref ModData.gisDb,out eError);
                    if (eError != null)
                    {
                        ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    }
                }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }

    }
}
