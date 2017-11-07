using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    /// <summary>
    /// 面边界面边界重合检查
    /// </summary>
    public class ControlsAreaBoundaryCoveredByAreaBoundary: Plugin.Interface.CommandRefBase
    {
       private Plugin.Application.IAppGISRef _AppHk;

        public ControlsAreaBoundaryCoveredByAreaBoundary()
        {
            base._Name = "GeoDataChecker.ControlsAreaBoundaryCoveredByAreaBoundary";
            base._Caption = "面边界面边界重合检查";
            base._Tooltip = "一个多边形要素类的边界线是否在另一个多边形要素类的边界线上";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "面边界面边界重合检查";
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

            //执行面边界面边界重合检查
            FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.面边界面边界重合检查);
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
