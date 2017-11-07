using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    class AnnotationDigitalDuplicateCheck:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;

        public AnnotationDigitalDuplicateCheck()
        {
            base._Name = "GeoDataChecker.AnnotationDigitalDuplicateCheck";
            base._Caption = "注记重复数字化检查";
            base._Tooltip = "检查图面中的注记要素是否有与之重复的注记要素（位置相同、属性相同）";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "注记重复数字化检查";
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
            ExcuteAnnotationDigitalDuplicateCheck(_AppHk);
        }



        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;
        }

        /// <summary>
        /// 注记重复数字化检查主函数
        /// </summary>
        /// <param name="_AppHk"></param>
        private void ExcuteAnnotationDigitalDuplicateCheck(Plugin.Application.IAppArcGISRef _AppHk)
        {
            
        }
    }
}
