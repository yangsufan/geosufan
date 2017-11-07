using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using GeoDataCenterFunLib;
using System.Windows.Forms;
using GeoDataExport;
namespace GeoSysUpdate
{
    public class ControlsXZQExport : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsXZQExport()
        {
            base._Name = "GeoSysUpdate.ControlsXZQExport";
            base._Caption = "行政区提取";
            base._Tooltip = "行政区提取";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "行政区提取";
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
        {
            if (_hook == null) return;
            if (_hook.MainUserControl == null) return;

            //UserControlSMPD pUserControl = _hook.MainUserControl as UserControlSMPD;
            //if (pUserControl != null)
            //{
            //    //切换到标准图幅tab页
            //    pUserControl.TurnToXZQTab();
            //} //ygc 2013-01-28修改行政区选择方式
            //更新图库树
            FrmGetXZQLocation newFrom = new FrmGetXZQLocation();
            newFrom.m_DefaultMap = _hook.MapControl;
            newFrom.m_IsClose = true;
            if(newFrom .ShowDialog ()!=DialogResult .OK) return ;
            newFrom.drawgeometryXOR(newFrom .m_pGeometry);
            frmExport pfrmExport = new GeoDataExport.frmExport(_hook.MapControl.Map,newFrom .m_pGeometry);
            pfrmExport.WriteLog = this.WriteLog;//ygc 2012-9-11 新增是否写日志
            pfrmExport.XZQCode = newFrom.m_XZQCode;
            pfrmExport.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
    }
}
