using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    /// <summary>
    /// 元数据提取
    /// </summary>
    public class ControlsMetaDataStract : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        public ControlsMetaDataStract()
        {
            base._Name = "GeoDBATool.ControlsMetaDataStract";
            base._Caption = "元数据提取";
            base._Tooltip = "元数据提取";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "元数据提取";
        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.MapControl == null) return false;
                return true;
                
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
        public override void OnClick()
        {
            frmMetaConversion FrmMetaConv = new frmMetaConversion();
            FrmMetaConv.ShowDialog();           
        }
    }
}
