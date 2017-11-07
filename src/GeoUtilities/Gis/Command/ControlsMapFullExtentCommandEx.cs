using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GeoUtilities
{   //added by chulili 20110916 根据系统参数，执行全图操作
    public class ControlsMapFullExtentCommandEx : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;


        public ControlsMapFullExtentCommandEx()
        {
            base._Name = "GeoUtilities.ControlsMapFullExtentCommandEx";
            base._Caption = "全图";
            base._Tooltip = "全图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "全图";
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
            if ( _AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            if (_AppHk.MapControl.Map == null) return;
            string strMinX = SysCommon.ModSysSetting.GetSysSettingValue(Plugin.ModuleCommon.TmpWorkSpace, "X最小值");//MinX yjl20120813 modify 
            string strMinY = SysCommon.ModSysSetting.GetSysSettingValue(Plugin.ModuleCommon.TmpWorkSpace, "Y最小值");//MinY
            string strMaxX = SysCommon.ModSysSetting.GetSysSettingValue(Plugin.ModuleCommon.TmpWorkSpace, "X最大值");//MaxX
            string strMaxY = SysCommon.ModSysSetting.GetSysSettingValue(Plugin.ModuleCommon.TmpWorkSpace, "Y最大值");//MaxY
            try
            {
                double dMinX = Convert.ToDouble(strMinX);
                double dMinY = Convert.ToDouble(strMinY);
                double dMaxX = Convert.ToDouble(strMaxX);
                double dMaxY = Convert.ToDouble(strMaxY);
                Envelope  pEnvelope = new Envelope();
                pEnvelope.XMin = dMinX;
                pEnvelope.XMax = dMaxX;
                pEnvelope.YMin = dMinY;
                pEnvelope.YMax = dMaxY;
                _AppHk.MapControl.Extent = pEnvelope as IEnvelope ;
            }
            catch(Exception err)
            {
 
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("全图:" + Message);//xisheng 2011.07.08 增加日志
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;

        }
    }
}
