using System;
using System.Collections.Generic;
using System.Text;

//用户自定义发布
namespace GeoDataManagerFrame
{
    public class DataOutByUse : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public DataOutByUse()
        {
            base._Name = "GeoDataManagerFrame.DataOutByUse";
            base._Caption = "用户自定义发布";
            base._Tooltip = "用户自定义发布";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "用户自定义发布";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;


        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}
