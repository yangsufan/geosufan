using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data;
using System.IO;

namespace GeoDBIntegration
{
    /// <summary>
    /// chenyafei  20110311 add content:用户登录
    /// </summary>
    public class ControlsLogin: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppDBIntegraRef m_Hook;
        public ControlsLogin()
        {
            base._Name = "GeoDBIntegration.ControlsLogin";
            base._Caption = "用户登录";
            base._Tooltip = "用户登录";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "用户登录";
        }

        public override bool Enabled
        {
            get 
            {
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
            LoginForm pLoginForm = new LoginForm();
            if (pLoginForm.ShowDialog() == DialogResult.OK)
            {           
                //若用户和密码正确,则登录系统，保存用户相关信息
                ModuleData.m_User = pLoginForm.LoginUser; ;               
            }
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
        }

    }
}
