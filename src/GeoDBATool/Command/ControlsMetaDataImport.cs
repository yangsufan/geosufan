using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
// zhangqi    add   
using SysCommon.Authorize;
using SysCommon.Gis;

namespace GeoDBATool
{
    /// <summary>
    /// cyf 20110615 ：元数据 导入类
    /// </summary>
   public class ControlsMetaDataImport : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        public ControlsMetaDataImport()
        {
            base._Name = "GeoDBATool.ControlsMetaDataImport";
            base._Caption = "元数据入库";
            base._Tooltip = "元数据入库";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "元数据入库";
        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.MapControl == null) return false;
                return true;
                
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

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
        //测试链接信息是否可用
        private bool CanOpenConnect(SysCommon.Gis.SysGisDB vgisDb, string strType, string strServer, string strService, string strDatabase, string strUser, string strPassword, string strVersion)
        {
            bool blnOpen = false;

            Exception Err;

            if (strType.ToUpper() == "ORACLE" || strType.ToUpper() == "SQLSERVER")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, strService, strDatabase, strUser, strPassword, strVersion, out Err);
            }
            else if (strType.ToUpper() == "ACCESS")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.PDB, out Err);
            }
            else if (strType.ToUpper() == "FILE")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.GDB, out Err);
            }

            return blnOpen;

        }
        public override void OnClick()
        {
           frmMetaConversionXML FrmMetaConv = new frmMetaConversionXML(Plugin.ModuleCommon.TmpWorkSpace);
           if (this.WriteLog)
           {
               Plugin.LogTable.Writelog(Caption); //ygc 2012-9-14写日志
           }
            FrmMetaConv.ShowDialog();    
        }
    }
}
