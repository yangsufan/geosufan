﻿using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
namespace GeoDataManagerFrame
{
    public class CommandAreaMeasure: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef _AppHk;
        //private Plugin.Application.IAppArcGISRef _AppHk;

        private Plugin.Application.IAppFormRef m_pAppForm;
        private ESRI.ArcGIS.SystemUI.ITool m_pTool = null;
        private ESRI.ArcGIS.SystemUI.ICommand m_pCommand = null;

       public CommandAreaMeasure()
        {
            base._Name = "GeoDataManagerFrame.CommandAreaMeasure";
            base._Caption = "面积量算";
            base._Tooltip = "面积量算";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = false;
            base._Message = "面积量算";

            m_pTool = new GeoDataCenterFunLib.ControlsMapMeasureTool() as ESRI.ArcGIS.SystemUI.ITool;
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;
                if (_AppHk.CurrentControl == null) return false;
                return true ;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            if (_AppHk == null) return;
            if (_AppHk.CurrentControl == null) return;

            _AppHk.MapControl.CurrentTool = m_pTool;
            
            _AppHk.CurrentTool = this.Name;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;
            if (_AppHk.CurrentControl == null) return;

            m_pTool = new GeoDataCenterFunLib.ControlsMapMeasureToolDefClass() as ESRI.ArcGIS.SystemUI.ITool;
            m_pCommand = m_pTool as ESRI.ArcGIS.SystemUI.ICommand;
            m_pCommand.OnCreate(_AppHk);
        }
    }
}
