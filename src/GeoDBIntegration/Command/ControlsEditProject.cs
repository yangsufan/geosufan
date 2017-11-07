using System;
using System.Collections.Generic;
using System.Text;
using SysCommon.Authorize;

namespace GeoDBIntegration
{
    public class ControlsEditProject : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppDBIntegraRef m_Hook;
        //added by chulili 20110624 为了判断可用状态添加变量
        private Plugin.Application.IAppFormRef _hook;
        public ControlsEditProject()
        {
            base._Name = "GeoDBIntegration.ControlsEditProject";
            base._Caption = "修改数据库工程";
            base._Tooltip = "修改数据库工程名称";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "修改数据库工程名称";
        }

        public override bool Enabled
        {
            get
            {
                //若没有选中数据库工程节点，则按钮不可用
                if (m_Hook == null) return false;
                if (m_Hook.ProjectTree == null || m_Hook.MapControl == null || m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.Tag == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;

                //cyf 20110602 modify
                //若没有登录系统，则按钮不可用
                if ((m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo == null) return false;
                //若用户不是管理员，则按钮不可用
                bool beAdmin = false;
                //added by chulili 20110624 若不处于数据源管理界面  菜单不可用
                if (!(_hook.MainForm.Controls[0] is UserControlDBIntegra))
                {
                    return false;
                }

                if (_hook.MainForm.Controls[0].Visible == false)
                {
                    return false;
                }


                //end add
                foreach (Role pRole in (m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo)
                {
                    if (pRole.TYPEID == EnumRoleType.管理员.GetHashCode().ToString())
                    {
                        beAdmin = true;
                        break;
                    }
                }
                return true;
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
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
            frmProject newFrm = new frmProject(m_Hook.ProjectTree);
            newFrm.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            //added by chulili 20110624
            _hook = hook as Plugin.Application.IAppFormRef;
            //end add
            if (m_Hook == null) return;
        }
    }
}
