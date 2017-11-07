using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SysCommon.Error;
using System.IO;
// zhangqi    add   
using SysCommon.Authorize;
using SysCommon.Gis;

namespace GeoUtilities
{
    public class ControlsOrderTask : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public ControlsOrderTask()
        {
            base._Name = "GeoUtilities.ControlsOrderTask";
            base._Caption = "订单管理";
            base._Tooltip = "订单管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "订单管理";
        }
        public override bool Enabled
        {
            get
            {
                try
                {
                    if (m_Hook.MapControl.Map.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    return true;
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

        ////测试链接信息是否可用
        //private bool CanOpenConnect(SysCommon.Gis.SysGisDB vgisDb, string strType, string strServer, string strService, string strDatabase, string strUser, string strPassword, string strVersion)
        //{
        //    bool blnOpen = false;

        //    Exception Err;

        //    if (strType.ToUpper() == "ORACLE" || strType.ToUpper() == "SQLSERVER")
        //    {
        //        blnOpen = vgisDb.SetWorkspace(strServer, strService, strDatabase, strUser, strPassword, strVersion, out Err);
        //    }
        //    else if (strType.ToUpper() == "ACCESS")
        //    {
        //        blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.PDB, out Err);
        //    }
        //    else if (strType.ToUpper() == "FILE")
        //    {
        //        blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.GDB, out Err);
        //    }

        //    return blnOpen;

        //}
        public override void OnClick()
        {
            if (m_Hook == null)
                return;

            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            try
            {
                frmOrderTask vFrm = new frmOrderTask(m_Hook.MapControl,Plugin.ModuleCommon.TmpWorkSpace);
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
