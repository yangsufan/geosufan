using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataChecker
{
    /// <summary>
    /// 拉线查等高线
    /// </summary>
    public class ContourLinesElevCheck :Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;

        public ContourLinesElevCheck()
        {
            base._Name = "GeoDataChecker.ContourLinesElevCheck";
            base._Caption = "拉线查等高线";
            base._Tooltip = "检查图面中所有等高线的高程是否标准，即是否是零高程或异常高程值（高程线的步进值是否是0.5的倍数）";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "拉线查等高线";
        }

       
        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false ;
                if (_AppHk.MapControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
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
            if (_AppHk == null||_tool==null||_cmd==null) return;
            if (_AppHk.MapControl == null) return;

            //执行等高线高程检查
            //FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.等高线高程值);
            //mFrmMathematicsCheck.ShowDialog();

            _AppHk.MapControl.CurrentTool = _tool;
            _AppHk.CurrentTool = this.Name;

        }


        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;

            _tool = new LineElevToolCls(_AppHk);
            _cmd = _tool as ICommand;
            _cmd.OnCreate(_AppHk.MapControl);
        }
    }
}
