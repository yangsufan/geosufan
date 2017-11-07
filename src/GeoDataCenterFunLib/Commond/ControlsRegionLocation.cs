using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using SysCommon.Error;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataCenterFunLib
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110730
    /// 说明：河流查询
    /// </summary>
    public class ControlsRegionLocation : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;

        public ControlsRegionLocation()
        {
            base._Name = "GeoDataCenterFunLib.ControlsRegionLocation";
            base._Caption = "行政区定位";
            base._Tooltip = "行政区定位";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "行政区定位";
            //base._Image = "";
            //base._Category = "";
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
          /*  get
            {
                //不存在图幅结合表图层、数据操作进程正在进行时不可用
                if (_AppHk.MapControl == null) return false;
                if (_AppHk.MapControl.Map.LayerCount == 0) return false;
                return true;
            }*/
            get
            {
                try
                {
                    if (_AppHk.CurrentControl is ISceneControl) return false;
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }
        public override void OnClick()
        {
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            IMap pMap = _AppHk.MapControl.Map;
            IMapControlDefault pMapControl = _AppHk.MapControl;
            FrmGetXZQLocation newfrom = new FrmGetXZQLocation();
            newfrom.m_DefaultMap = pMapControl;
            newfrom.ShowDialog();
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}
