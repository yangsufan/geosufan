using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    class ContourLinesSelfintersectCheck:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;

        public ContourLinesSelfintersectCheck()
        {
            base._Name = "GeoDataChecker.ContourLinesSelfintersectCheck";
            base._Caption = "等高线自相交检查";
            base._Tooltip = "检查图面中是否有相交的等高线";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "等高线自相交检查";
            //base._Image = "";
            //base._Category = "";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    else
                    {
                        if (SetCheckState.CheckState)
                        {
                            base._Enabled = true;
                            return true;
                        }
                        else
                        {
                            base._Enabled = false;
                            return false;
                        }
                    }
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
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

            //执行等高线自相交检查

            ExcuteContourLinesSelfintersectCheck(_AppHk);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;
        }

        /// <summary>
        /// 等高线自相交检查主函数
        /// </summary>
        /// <param name="_AppHk"></param>
        private void ExcuteContourLinesSelfintersectCheck(Plugin.Application.IAppGISRef _AppHk)
        {
            
        }

    }
}
