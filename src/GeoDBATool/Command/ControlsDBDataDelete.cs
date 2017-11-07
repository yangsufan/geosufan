using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    /// <summary>
    /// 删除  陈亚飞添加20101124
    /// </summary>
   public class ControlsDBDataDelete: Plugin.Interface.CommandRefBase
    {
         private Plugin.Application.IAppGISRef m_Hook;

         public ControlsDBDataDelete()
        {
            base._Name = "GeoDBATool.ControlsDBDataDelete";
            base._Caption = "删除";
            base._Tooltip = "删除";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
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

        public override void OnClick()
        {
            //删除操作
            ModData.m_CurOperType = EnumUpdateType.删除;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
