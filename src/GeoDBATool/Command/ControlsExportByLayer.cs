﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    public class ControlsExportByLayer : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsExportByLayer()
        {
            base._Name = "GeoDBATool.ControlsExportByLayer";
            base._Caption = "分层抽取";
            base._Tooltip = "分层抽取";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "分层抽取";

        }

        public override bool Enabled
        {
            get
            {
              
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

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
