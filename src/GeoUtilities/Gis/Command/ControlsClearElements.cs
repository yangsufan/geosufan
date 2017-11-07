using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GeoUtilities
{
    public class ControlsClearElements : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private Plugin.Application.AppGidUpdate _hook;
        //private Plugin.Application.IAppGISRef m_Hook;
        public ControlsClearElements()
        {
            base._Name = "GeoUtilities.ControlsClearElements";
            base._Caption = "清除元素";
            base._Tooltip = "清除元素";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "清除元素";
            //base._Image = "";
            //base._Category = "";
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
            if (_AppHk.MapControl == null) return;
            if (_hook != null)
            {
                _hook.pGeometry = null;
            }
            _AppHk.MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
            //_AppHk.MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            _AppHk.MapControl.ActiveView.GraphicsContainer.DeleteAllElements();
            SysCommon.ScreenDraw.list.Clear();
            _AppHk.MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, _AppHk.MapControl.ActiveView.Extent);
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(base._Tooltip);//xisheng 2011.07.08 增加日志
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
            if (_AppHk.MapControl == null) return;
        }
    }
}
