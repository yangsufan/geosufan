using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;

namespace GeoArcScene3DAnalyse
{
    public class GeoCommandViewshedAnalyse : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;
        private ESRI.ArcGIS.SystemUI.ICommand m_pCommand;


        public GeoCommandViewshedAnalyse()
        {
            base._Name = "GeoArcScene3DAnalyse.GeoCommandViewshedAnalyse";
            base._Caption = "通视分析";
            base._Tooltip = "通视分析";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = false;
            base._Message = "通视分析";

            m_pCommand = new GeoArcScene3DAnalyse.CommandViewshedAnalyse();
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
            GeoArcScene3DAnalyse.CommandViewshedAnalyse TempCommand = m_pCommand as CommandViewshedAnalyse;
            TempCommand.WriteLog = this.WriteLog;
            TempCommand.OnCreate(_AppHk.CurrentControl);
            TempCommand.OnClick();

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
