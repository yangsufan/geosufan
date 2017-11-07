using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    /// <summary>
    /// 线端点被点覆盖检查
    /// </summary>
    public class ControlsLineEndpointCoveredByPoint: Plugin.Interface.CommandRefBase
    {
       private Plugin.Application.IAppGISRef _AppHk;

        public ControlsLineEndpointCoveredByPoint()
        {
            base._Name = "GeoDataChecker.ControlsLineEndpointCoveredByPoint";
            base._Caption = "线端点被点覆盖检查";
            base._Tooltip = "检查线层中线要素的端点是否被点所覆盖";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "线端点被点覆盖检查";
        }

        public override bool Enabled
        {
            get
            {
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
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;

            //执行线端点被点覆盖检查
            FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.线端点被点覆盖检查);
            mFrmMathematicsCheck.ShowDialog();
         
        }


       public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}