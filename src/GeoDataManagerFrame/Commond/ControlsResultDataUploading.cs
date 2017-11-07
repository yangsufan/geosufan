using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataManagerFrame
{
   public class ControlsResultDataUploading:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public ControlsResultDataUploading()
        {
            base._Name = "GeoDataManagerFrame.ControlsResultDataUploading";
            base._Caption = "成果数据入库";
            base._Tooltip = "成果数据入库";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "成果数据入库";
        }
        public override void OnClick()
        {
            //if (m_Hook == null) return;
            FrmResultDataUploading pFrmResultDataUploading = new FrmResultDataUploading(Plugin.ModuleCommon.TmpWorkSpace);
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption); //ygc 2012-9-14 写日志
            }
            pFrmResultDataUploading.ShowDialog();
            

        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
