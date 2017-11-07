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
    /// yjl 20110727 ：元数据 统计类
    /// </summary>
   public class ControlsMetaDataStat : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        public ControlsMetaDataStat()
        {
            base._Name = "GeoDBATool.ControlsMetaDataStat";
            base._Caption = "元数据统计";
            base._Tooltip = "元数据统计";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "元数据统计";
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
            //   zhangqi Add  
            //判断配置文件是否存在
            bool blnCanConnect = false;
            SysCommon.Gis.SysGisDB vgisDb = new SysGisDB();
            if (File.Exists(GeoDBIntegration.ModuleData.v_ConfigPath))
            {
                //获得系统维护库连接信息
                SysCommon.Authorize.AuthorizeClass.GetConnectInfo(GeoDBIntegration.ModuleData.v_ConfigPath, out GeoDBIntegration.ModuleData.Server, out GeoDBIntegration.ModuleData.Instance, out GeoDBIntegration.ModuleData.Database, out GeoDBIntegration.ModuleData.User, out GeoDBIntegration.ModuleData.Password, out GeoDBIntegration.ModuleData.Version, out GeoDBIntegration.ModuleData.dbType);
                //连接系统维护库
                blnCanConnect = CanOpenConnect(vgisDb, GeoDBIntegration.ModuleData.dbType, GeoDBIntegration.ModuleData.Server, GeoDBIntegration.ModuleData.Instance, GeoDBIntegration.ModuleData.Database, GeoDBIntegration.ModuleData.User, GeoDBIntegration.ModuleData.Password, GeoDBIntegration.ModuleData.Version);
            }
            else
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "缺失系统维护库连接信息文件：" + GeoDBIntegration.ModuleData.v_ConfigPath + "/n请重新配置");
                return;
            }
            if (!blnCanConnect)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统能够维护库连接失败，请检查!");
                return;
            }
            
           GeoDBIntegration.ModuleData.TempWks = vgisDb.WorkSpace;
            //zhangqi   add  end
           frmMetaStat FrmMetaConv = new frmMetaStat();
            FrmMetaConv.ShowDialog();    
        }
    }
}
