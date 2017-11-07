using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    /// <summary>
    /// 简单线检查
    /// </summary>
    public class ControlsLineNoMutiPart:Plugin.Interface.CommandRefBase
    {

       private Plugin.Application.IAppGISRef _AppHk;

        public ControlsLineNoMutiPart()
        {
            base._Name = "GeoDataChecker.ControlsLineNoMutiPart";
            base._Caption = "简单线检查";
            base._Tooltip = "检查线层中的线是否为简单线";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "简单线检查";
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
            Exception eError = null;

            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;

            //执行简单线检查
            FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.简单线检查);
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
