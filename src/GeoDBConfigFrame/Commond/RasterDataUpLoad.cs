using System;
using System.Collections.Generic;
using System.Text;
using GeoDataCenterFunLib;

//栅格数据入库
namespace GeoDBConfigFrame
{
    public class RasterDataUpLoad : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public RasterDataUpLoad()
        {
            base._Name = "GeoDBConfigFrame.RasterDataUpLoad";
            base._Caption = "栅格数据入库";
            base._Tooltip = "栅格数据入库";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "栅格数据入库";
        }

        public override void OnClick()
        {

            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("栅格数据入库");

                }
            }
            //调用数据上载
            frmRasterDataUpload frm = new frmRasterDataUpload();
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
