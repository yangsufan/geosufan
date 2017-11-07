using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110915
    /// 说明：行政区出图
    /// </summary>
    public class ControlsXZQOutMap : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsXZQOutMap()
        {
            base._Name = "GeoPageLayout.ControlsXZQOutMap";
            base._Caption = "行政区出图";
            base._Tooltip = "行政区出图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "行政区出图";
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
                DevComponents.AdvTree.AdvTree xzqTree = _hook.XZQTree;
                IGeometry xzqGeo = ModGetData.getExtentByXZQ(xzqTree.SelectedNode);
                if (xzqGeo == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到相应的行政区范围！");
                    return;
                }
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog(this._Caption);
                }
                FrmPageLayout frm = new FrmPageLayout(_hook.MapControl.Map, xzqGeo, true);
                frm.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                frm.Show();
                //pgss.Close();
                _hook.MapControl.CurrentTool = null;
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
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
