using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    class PolygonStadardize:Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        public PolygonStadardize()
        {
            base._Name = "GeoDataChecker.PolygonStadardize";
            base._Caption = "面要素标准化";
            base._Tooltip = "面拓扑检查，面重复性检查，三点房屋检查";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "面要素标准化";
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

            //执行面要素标准化
            ExcutePolygonStadardize(_AppHk);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;
        }

        /// <summary>
        /// 面要素标准化主函数

        /// </summary>
        /// <param name="_AppHk"></param>
        private void ExcutePolygonStadardize(Plugin.Application.IAppArcGISRef _AppHk)
        {

        }
    }
}
