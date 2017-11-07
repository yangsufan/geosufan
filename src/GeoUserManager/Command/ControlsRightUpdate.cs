using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using SysCommon.Authorize;
using SysCommon.Error;

namespace GeoUserManager
{
    public class ControlsRightUpdate : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public ControlsRightUpdate()
        {
            base._Name = "GeoUserManager.ControlsRightUpdate";
            base._Caption = "更新权限";
            base._Tooltip = "更新权限";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "更新权限";
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
                        if (pRole.UCtag == "Role")
                        {
                            if (m_Hook.RoleTree.SelectedNode != null)
                            {
                                if (m_Hook.PrivilegeTree.Nodes.Count > 0)
                                {
                                    return true;
                                }
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
            if (m_Hook.RoleTree.SelectedNode != null&&m_Hook.PrivilegeTree.Tag != null)
            {
                Exception eError;
                Role role = m_Hook.RoleTree.SelectedNode.Tag as Role;
                if (role == null) return;
                XmlDocument doc = m_Hook.PrivilegeTree.Tag as XmlDocument;
                if (doc == null) return;
                if (!ModuleOperator.AddPrivilege(role, doc, ref ModData.gisDb, out eError))
                {
                    if (eError != null)
                    {
                        ErrorHandle.ShowInform("提示", eError.Message);
                        return;
                    }
                }
                else
                {
                    ErrorHandle.ShowInform("提示", "更新成功！");
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
