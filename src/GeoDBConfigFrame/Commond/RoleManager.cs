using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
namespace GeoDBConfigFrame
{
    public class RoleManager : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public RoleManager()
        {
            base._Name = "GeoDBConfigFrame.RoleManager";
            base._Caption = "角色管理";
            base._Tooltip = "角色管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "角色管理";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;


            //GeoUserRole.FrmRoleManager pFrm = new GeoUserRole.FrmRoleManager(m_Hook);
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