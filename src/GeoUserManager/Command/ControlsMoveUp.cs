using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace GeoUserManager
{
    public class ControlsMoveUp : Fan.Plugin.Interface.CommandRefBase
    {
        private Fan.Plugin.Application.IAppPrivilegesRef m_Hook;
        public ControlsMoveUp()
        {
            base._Name = "GeoUserManager.ControlsMoveUp";
            base._Caption = "обрф";
            base._Tooltip = "обрф";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "обрф";
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
                            if (m_Hook.RoleTree.SelectedNode != null && m_Hook.RoleTree.SelectedNode.Index != 0)//changed by xisheng 2011.06.27
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (m_Hook.UserTree.SelectedNode != null && m_Hook.UserTree.SelectedNode.Index != 0)//changed by xisheng 2011.06.27
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
                }
                return false;
            }
        }

        public override string Message
        {
            get
            {
                Fan.Plugin.Application.IAppPrivilegesRef pAppFormRef = m_Hook as Fan.Plugin.Application.IAppPrivilegesRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Fan.Plugin.Application.IAppPrivilegesRef pAppFormRef = m_Hook as Fan.Plugin.Application.IAppPrivilegesRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            if (m_Hook.MainForm.Controls[0] is UCRole)
            {
                UCRole pRole = m_Hook.MainForm.Controls[0] as UCRole;
                if (pRole.UCtag == "Role")
                {
                    if (m_Hook.RoleTree.SelectedNode != null)
                    {
                        ModuleOperator.MovUp(m_Hook.RoleTree, m_Hook.RoleTree.SelectedNode);
                    }
                }
                else
                {
                    if (m_Hook.UserTree.SelectedNode != null)
                    {
                        if (m_Hook.UserTree.SelectedNode != null)
                        {
                            ModuleOperator.MovUp(m_Hook.UserTree, m_Hook.UserTree.SelectedNode);
                        }
                    }
                }
            }
        }

        public override void OnCreate(Fan.Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Fan.Plugin.Application.IAppPrivilegesRef;
        }

    }
}
