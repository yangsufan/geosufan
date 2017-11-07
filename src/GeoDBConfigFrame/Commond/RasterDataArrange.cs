using System;
using System.Collections.Generic;
using System.Text;
using GeoDataCenterFunLib;

//栅格数据整理
namespace GeoDBConfigFrame
{
    public class RasterDataArrange : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public RasterDataArrange()
        {
            base._Name = "GeoDBConfigFrame.RasterDataArrange";
            base._Caption = "栅格数据整理";
            base._Tooltip = "栅格数据整理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "栅格数据整理";
        }

        public override void OnClick()
        {

            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("栅格数据整理");

                }
            }
            frmRasterDataReduction frm = new frmRasterDataReduction();
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
