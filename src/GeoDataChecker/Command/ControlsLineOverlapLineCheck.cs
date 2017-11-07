using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    /// <summary>
    /// 异层线重叠检查
    /// </summary>
    public class ControlsLineOverlapLineCheck:Plugin.Interface.CommandRefBase
    {
       private Plugin.Application.IAppGISRef _AppHk;

        public ControlsLineOverlapLineCheck()
        {
            base._Name = "GeoDataChecker.ControlsLineOverlapLineCheck";
            base._Caption = "异层线重叠检查";
            base._Tooltip = "检查在不同的要素类中，线与线是否重叠";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "异层线重叠检查";
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

            //执行异层线重叠检查
            FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.异层线重叠检查);
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


