using System;
using System.Collections.Generic;
using System.Text;
using GeoDataCenterFunLib;

//文档数据入库
namespace GeoDBConfigFrame
{
     public class DocUpload : Plugin.Interface.CommandRefBase
     {
         private Plugin.Application.IAppPrivilegesRef m_Hook;
         public DocUpload()
         {
            base._Name = "GeoDBConfigFrame.DocUpload";
            base._Caption = "文档数据入库";
            base._Tooltip = "文档数据入库";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "文档数据入库";
        }
        public override void OnClick()
        {

            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("文档数据入库");

                }
            }
            frmDocUpload frm = new frmDocUpload();
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
