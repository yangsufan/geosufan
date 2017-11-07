using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;

namespace GeoArcScene3DAnalyse
{
    public class GeoCommand3DSection : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;
        private ESRI.ArcGIS.SystemUI.ICommand m_pCommand;


        public GeoCommand3DSection()
        {
            base._Name = "GeoArcScene3DAnalyse.GeoCommand3DSection";
            base._Caption = "剖面分析";
            base._Tooltip = "剖面分析";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = false;
            base._Message = "剖面分析";

            m_pCommand = new GeoArcScene3DAnalyse.Command3DSection();
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;
                if (_AppHk.CurrentControl == null) return false;
                if (_AppHk.CurrentControl is ISceneControl)
                {
                    ISceneControl ptmpControl = _AppHk.CurrentControl as ISceneControl;
                    if (ptmpControl.Scene.LayerCount > 0)
                    {
                        return true;
                    }
                }
                return false;
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

            (m_pCommand as GeoArcScene3DAnalyse.Command3DSection).WriteLog = this.WriteLog;
            m_pCommand.OnCreate(_AppHk.CurrentControl);
            m_pCommand.OnClick();

            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.CurrentControl == null) return;
        }
    }
}
