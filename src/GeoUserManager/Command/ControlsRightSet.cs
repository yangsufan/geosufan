using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Fan.Common.Authorize;
using Fan.Common.Error;

namespace GeoUserManager
{
    public class ControlsRightSet : Fan.Plugin.Interface.CommandRefBase
    {
        private Fan.Plugin.Application.IAppPrivilegesRef m_Hook;
        public ControlsRightSet()
        {
            base._Name = "GeoUserManager.ControlsRightSet";
            base._Caption = "权限设置";
            base._Tooltip = "权限设置";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "权限设置";
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
            if (m_Hook.RoleTree.SelectedNode != null)
            {
                Exception eError;
                Role role = m_Hook.RoleTree.SelectedNode.Tag as Role;
                if (role == null) return;
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "选择权限文件";
                ofd.Filter = "Xml文件|*xml";
                ofd.RestoreDirectory = false;
                if (ofd.ShowDialog()==DialogResult.OK)
                {
                    XmlDocument doc = new XmlDocument();
                    string fileName = ofd.FileName;
                    doc.Load(fileName);
                    //给用户组添加权限
                    if (ModuleOperator.AddPrivilege(role, doc, ref ModData.gisDb, out eError))
                    {
                        //将权限显示在权限树上
                        ModuleOperator.DisplayInLstView(doc, m_Hook.PrivilegeTree);
                    }
                    else
                    {
                        if (eError != null)
                        {
                            ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            return;
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
