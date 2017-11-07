using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    class BatchDataCheck : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef _AppHk;

        public BatchDataCheck()
        {
            base._Name = "GeoDataChecker.BatchDataCheck";
            base._Caption = "批量数据检查";
            base._Tooltip = "批量数据检查";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "批量数据检查";
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
            frmBatchDataCheck frm = new frmBatchDataCheck();
            frm.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
        }
    }
}
