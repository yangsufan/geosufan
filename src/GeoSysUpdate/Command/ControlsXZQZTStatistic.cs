using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using GeoPageLayout;

namespace GeoSysUpdate
{
    /// <summary>
    /// 作者：ZQ
    /// 日期：20120721
    /// 说明：行政区专题统计
    /// </summary>
    public class ControlsXZQZTStatistic : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsXZQZTStatistic()
        {
            base._Name = "GeoSysUpdate.ControlsXZQZTStatistic";
            base._Caption = "行政区专题统计";
            base._Tooltip = "行政区专题统计";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "行政区专题统计";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
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
        {//XZQLocation
            if (_hook == null) return;
            if (_hook.MainUserControl == null) return;
            try
            {
                UserControlSMPD pUserControl = _hook.MainUserControl as UserControlSMPD;
                pUserControl.LocationByXZQNode();
                DevComponents.AdvTree.AdvTree xzqTree = _hook.XZQTree;
                IGeometry xzqGeo = ModGetData.getExtentByXZQ(xzqTree.SelectedNode);
                if (xzqGeo == null)
                {
                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到相应的行政区范围！"); 屏蔽二次警告窗体 ygc 2012-8-29
                    return;
                }
                frmXZQZTStatistical pfrmXZQZTStatistical = new frmXZQZTStatistical(xzqTree.SelectedNode);
                pfrmXZQZTStatistical.ShowDialog();
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("行政区专题统计"); //ygc 2012-9-14 写日志
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
        #region 自定义方法
       
        #endregion
    }
}
