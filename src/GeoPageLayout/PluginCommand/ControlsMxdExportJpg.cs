using System;
using System.Collections.Generic;
using System.Text;

namespace GeoPageLayout
{
    public class ControlsMxdExportJpg:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public ControlsMxdExportJpg()
        {
            base._Name = "GeoPageLayout.ControlsMxdExportJpg";
            base._Caption = "Mxd导出JPG";
            base._Tooltip = "Mxd导出JPG";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "Mxd导出JPG";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            try
            {
                FrmMxdExportJPG pFrmMxdExportJPG = new FrmMxdExportJPG();
                pFrmMxdExportJPG.WriteLog = this.WriteLog;//ygc 2012-9-12 是否写日志
                pFrmMxdExportJPG.ShowDialog();
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
            }
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
