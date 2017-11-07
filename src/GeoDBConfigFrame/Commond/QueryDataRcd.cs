using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBConfigFrame
{
    public class QueryDataRcd : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public QueryDataRcd()
        {
            base._Name = "GeoDBConfigFrame.QueryDataRcd";
            base._Caption = "检索记录";
            base._Tooltip = "检索记录";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "检索记录";
        }
        //添加记录菜单响应
        public override void OnClick()
        {
            if (m_Hook.DataTabIndexTree == null)
                return;
            frmQueryDataRcd pfrmQueryDataRcd = new frmQueryDataRcd(m_Hook.DataTabIndexTree,Plugin.ModuleCommon.TmpWorkSpace);
            pfrmQueryDataRcd.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
