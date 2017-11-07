using System;
using System.Collections.Generic;
using System.Text;

namespace GeoUtilities
{

    public class ControlsGeoCorTrans : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        public ControlsGeoCorTrans()
        {
            base._Name = "=GeoUtilities.ControlsGeoCorTrans";
            base._Caption = "投影变换";
            base._Tooltip = "投影变换";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "投影变换";
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
            //
            frmGeocorTrans vTrans = new frmGeocorTrans();
            vTrans.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            vTrans.ShowDialog();
            vTrans = null;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}
