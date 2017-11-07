using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SysCommon.Authorize;
using SysCommon.Error;

namespace GeoUserManager
{
    public class ControlsModifyDepartment : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;

        public ControlsModifyDepartment()
        {
            base._Name = "GeoUserManager.ControlsModifyDepartment";
            base._Caption = "修改科室";
            base._Tooltip = "修改科室";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "修改科室信息";
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
                                if (m_Hook.UserTree.SelectedNode.Level == 1)
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
                string DeID= m_Hook.UserTree.SelectedNode.TagString ;
                if (DeID == null||DeID =="") return;
                if (m_Hook.UserTree.SelectedNode.Text == "遥感室")
                {
                    MessageBox.Show("该科室为管理员科室，不可修改！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Stop);
                    return;
                }
                FrmAddDepartment frmDepartment= new FrmAddDepartment();
                frmDepartment.m_id = DeID;
                frmDepartment.m_FrmText = "更新科室";
                frmDepartment.m_BtnText = "更新";
                frmDepartment.m_IsUpdate = true;
                if (frmDepartment.ShowDialog() == DialogResult.OK)
                {
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
                    }
                    ModuleOperator.DisplayUserTree("", m_Hook.UserTree, ref ModData.gisDb, out eError);
                    if (eError != null)
                    {
                        ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        return;
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
