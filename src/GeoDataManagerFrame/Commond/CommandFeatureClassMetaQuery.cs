using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataManagerFrame
{
   public  class CommandFeatureClassMetaQuery :Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public CommandFeatureClassMetaQuery()
        {
            base._Name = "GeoDataManagerFrame.CommandFeatureClassMetaQuery";
            base._Caption = "数据库属性信息查询";
            base._Tooltip = "数据库属性信息查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "数据库属性信息查询";
        }
        public override void OnClick()
        {
            //if (m_Hook == null) return;
            frmFeatureClassMetaQuery pfrmFeatureClassMetaQuery = new frmFeatureClassMetaQuery(Plugin.ModuleCommon.TmpWorkSpace);
            pfrmFeatureClassMetaQuery.ShowDialog();
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
