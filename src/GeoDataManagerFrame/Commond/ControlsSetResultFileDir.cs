using System;
using System.Collections.Generic;
using System.Text;
///ZQ 20111010   add  成果数据目录配置
namespace GeoDataManagerFrame
{
   public class ControlsSetResultFileDir:Plugin.Interface.CommandRefBase
    {
         private Plugin.Application.IAppPrivilegesRef m_Hook;
         public ControlsSetResultFileDir()
        {
            base._Name = "GeoDataManagerFrame.ControlsSetResultFileDir";
            base._Caption = "成果数据目录配置";
            base._Tooltip = "成果数据目录配置";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "成果数据目录配置";
        }
        public override void OnClick()
        {
            //if (m_Hook == null) return;
            FrmSetResultFileDir pFrmSetResultFileDir = new FrmSetResultFileDir(Plugin.ModuleCommon.TmpWorkSpace);
            pFrmSetResultFileDir.ShowDialog();
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
