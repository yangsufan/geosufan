using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
namespace GeoDBConfigFrame
{
    public class UserManager : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public UserManager()
        {
            base._Name = "GeoDBConfigFrame.UserManager";
            base._Caption = "用户管理";
            base._Tooltip = "用户管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "用户管理";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;

            //GeoUserRole.FrmUserManager pFrm = new GeoUserRole.FrmUserManager(m_Hook);
            //pFrm.ShowDialog();

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}