using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SysCommon.Authorize;
using SysCommon.Error;

namespace GeoUserManager
{
    public class ControlsAddDepartment : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;

        public ControlsAddDepartment()
        {
            base._Name = "GeoUserManager.ControlsAddDepartment";
            base._Caption = "添加科室";
            base._Tooltip = "添加科室";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "添加科室";
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
                            if (ModuleOperator.GroupByName == "科室")
                            {
                                if (m_Hook.UserTree.SelectedNode.Level == 0)
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
             Exception eError;
             FrmAddDepartment frmDepartment = new FrmAddDepartment();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
            if (frmDepartment.ShowDialog() == DialogResult.OK)
            {
                ModuleOperator.DisplayUserTree("", m_Hook.UserTree, ref ModData.gisDb, out eError);
                if (eError != null)
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
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
