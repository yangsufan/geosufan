using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.SystemUI;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Controls;

namespace GeoUtilities
{
    public class ControlsDefaultTool : Plugin.Interface.ToolRefBase
    {
       private Plugin.Application.IAppArcGISRef _AppHk;
        private ITool _pTool;
        private ICommand _pCommand;
       public ControlsDefaultTool()
        {
            base._Name = "GeoUtilities.ControlsDefaultTool";
            base._Caption = "默认工具";
            base._Tooltip = "默认工具";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Deactivate = false;
            base._Message = "恢复默认工具";
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
      
        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;

                if (_AppHk.CurrentControl is ISceneControl) return false;  //为了只有效于2维控件
                return true;
            }
        }

        public override void OnClick()
        {
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            _AppHk.CurrentTool = "";
            _AppHk.MapControl.CurrentTool = _pTool;
        }

       public override void OnCreate(Plugin.Application.IApplicationRef hook)
       {
           if (hook == null) return;
           _AppHk = hook as Plugin.Application.IAppArcGISRef;
           if (_AppHk.MapControl == null) return;
           _pTool = new DefaultTool();
           _pCommand = _pTool as ICommand;
           _pCommand.OnCreate(_AppHk.MapControl);
       }
    }
    public sealed class DefaultTool : BaseTool
    {
        private IHookHelper m_hookHelper;
        public DefaultTool()
        {
            
            base.m_category = ""; 
            base.m_caption = " ";  
            base.m_message = "默认工具"; 
            base.m_toolTip = "默认工具";
            base.m_name = "";
            base.m_cursor = Cursors.Default;
          
        }
        public override void OnCreate(object hook)
        {
            // TODO:  Add MeasureTool.OnCreate implementation
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
        }
    }
}
