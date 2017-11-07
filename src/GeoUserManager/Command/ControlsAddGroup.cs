using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SysCommon.Error;

namespace GeoUserManager
{
    public class ControlsAddGroup : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public ControlsAddGroup()
        {
            base._Name = "GeoUserManager.ControlsAddGroup";
            base._Caption = "添加角色";
            base._Tooltip = "添加角色";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "添加角色";
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

                        if (pRole.UCtag == "Role" && m_Hook.RoleTree.SelectedNode != null && m_Hook.RoleTree.SelectedNode.Level == 0)
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
            Exception eError;
            AddGroup frmGroup = new AddGroup();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
            if (frmGroup.ShowDialog() == DialogResult.OK)
            {
                ModuleOperator.DisplayRoleTree("", m_Hook.RoleTree, ref ModData.gisDb,out eError);
                if (eError != null)
                {
                    ErrorHandle.ShowInform("提示", eError.Message);
                    return;
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
