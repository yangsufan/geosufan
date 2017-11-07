using System;
using System.Collections.Generic;
using System.Text;
using GeoDataCenterFunLib;

//编辑命名规则
namespace GeoDBConfigFrame
{
    public class EditNameRule : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public EditNameRule()
        {
            base._Name = "GeoDBConfigFrame.EditNameRule";
            base._Caption = "编辑命名规则";
            base._Tooltip = "编辑命名规则";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "编辑命名规则";
        }
        public override void OnClick()
        {

            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("编辑命名规则");

                }
            }
            NameRuleForm frm = new NameRuleForm();
            frm.Show();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
