using System;
using System.Collections.Generic;
using System.Text;
using SysCommon.Error;

using System.IO;
// zhangqi    add   
using SysCommon.Authorize;
using SysCommon.Gis;

namespace GeoUtilities
{
    class ControlsOrderTaskQuery : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public ControlsOrderTaskQuery()
        {
            base._Name = "GeoUtilities.ControlsOrderTaskQuery";
            base._Caption = "订单管理";
            base._Tooltip = "订单管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "订单管理";
        }
        //public override bool Enabled
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (m_Hook.MapControl.Map.LayerCount == 0)
        //            {
        //                base._Enabled = false;
        //                return false;
        //            }

        //            base._Enabled = true;
        //            return true;
        //        }
        //        catch
        //        {
        //            base._Enabled = false;
        //            return false;
        //        }
        //    }
        //}
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

      
        public override void OnClick()
        {
            if (m_Hook == null)  return;
            try
            {
                frmOrderTaskQuery vFrm = new frmOrderTaskQuery(m_Hook.MapControl,Plugin.ModuleCommon.TmpWorkSpace);
                vFrm.ShowDialog();

            }
            catch (Exception exError)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", exError.Message);
            }

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}
