using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;

namespace GeoArcScene3DAnalyse
{
    public class GeoTool3DLineOfSight : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;
        private ESRI.ArcGIS.SystemUI.ICommand m_pCommand;
        private ITool _tool = null;

        public GeoTool3DLineOfSight()
        {
            base._Name = "GeoArcScene3DAnalyse.GeoTool3DLineOfSight";
            base._Caption = "两点通视";
            base._Tooltip = "两点通视";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = false;
            base._Message = "两点通视";

            //m_pCommand = new GeoArcScene3DAnalyse.Tool3DLineOfSight();
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
            //if ( _AppHk == null) return;
            //if (_AppHk.CurrentControl == null) return;

            //m_pCommand.OnCreate(_AppHk.CurrentControl);
            //m_pCommand.OnClick();

            //_AppHk.CurrentTool = this.Name; 
            if (_tool == null || m_pCommand == null || _AppHk == null)
                return;
            if (_AppHk.CurrentControl == null)
                return;

            m_pCommand.OnCreate(_AppHk.CurrentControl);
            //m_pCommand.OnClick();

            if (_AppHk.CurrentControl is ISceneControl)
            {
                _AppHk.SceneControl.CurrentTool = _tool;
            }
            else
            {
                return;
            }
            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;

            if (_AppHk.MapControl == null) return;
            _tool = new Tool3DLineOfSight();
            m_pCommand = _tool as ICommand;

            //m_pCommand.OnCreate(_AppHk.CurrentControl);
        }
    }
}
