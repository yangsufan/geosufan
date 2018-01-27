using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fan.SysSetting
{
    class InitializeSet : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppFormRef m_Hook;
        public InitializeSet()
        {
            _Name = "SysSetting.InitializeSet";
            _Caption = "数据库初始化";
            _Visible = true;
            _Enabled = true;
            _Message = "初始化数据库操作";
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppFormRef;
        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                return true;
            }
        }
        public override void OnClick()
        {
            FromInitializeSet fromInitalSet = new FromInitializeSet(m_Hook.MainForm);
            fromInitalSet.Text = _Caption;
            fromInitalSet.Show();
        }
    }
}
