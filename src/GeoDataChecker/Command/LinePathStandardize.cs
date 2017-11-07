using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    class LinePathStandardize:Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        public LinePathStandardize()
        {
            base._Name = "GeoDataChecker.LinePathStandardize";
            base._Caption = "线方准化处理";
            base._Tooltip = "将加载的线要素的方向标准化并清除伪节点";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "线标准化处理";
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

            //执行线方向标准化和伪节点清除处理
            ExcuteLinePathStandardize(_AppHk);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;
        }

        /// <summary>
        /// 线方向标准化主函数

        /// </summary>
        /// <param name="_AppHk"></param>
        private void ExcuteLinePathStandardize(Plugin.Application.IAppArcGISRef _AppHk)
        {
            
        }
    }
}
