using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.ConversionTools;
using ESRI.ArcGIS.Geoprocessor;


namespace GeoDBATool
{
    /// <summary>
    /// cyf 20110620 add:栅格数据更新
    /// 若不是服务器上的数据，首先将数据上传到FTP服务器上；第二步，将ftp服务器上的数据录入到RasterCatalog里面
    /// </summary>
    public class ControlsUpdateRasterCatalog: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;        //主功能应用app        
        Plugin.Application.IAppFormRef pAppFormRef;          //应用程序主窗体
        //private System.Windows.Forms.Timer _timer;           //计时器

        //初始化按钮信息
        public ControlsUpdateRasterCatalog()
        {
            base._Name = "GeoDBATool.ControlsUpdateRasterCatalog";
            base._Caption = "栅格数据更新(RC)";
            base._Tooltip = "栅格数据更新(RC)";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "栅格数据更新(RC)";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //cyf 20110626 modify 
                if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "RC" ) return false;//&& m_Hook.ProjectTree.SelectedNode.DataKeyString != "RD"
                //end
                return true;
            }
        }

        public override string Message
        {
            get
            {
                pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
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
            #region 获取系统维护库连接信息，并将工作空间保存起来
            if (ModData.TempWks == null)
            {
                bool blnCanConnect = false;
                SysCommon.Gis.SysGisDB vgisDb = new SysCommon.Gis.SysGisDB();
                if (File.Exists(ModData.v_ConfigPath))
                {
                    //获得系统维护库连接信息
                    SysCommon.Authorize.AuthorizeClass.GetConnectInfo(ModData.v_ConfigPath, out ModData.Server, out ModData.Instance, out ModData.Database, out ModData.User, out ModData.Password, out ModData.Version, out ModData.dbType);
                    //连接系统维护库
                    blnCanConnect =ModDBOperator.CanOpenConnect(vgisDb, ModData.dbType, ModData.Server, ModData.Instance, ModData.Database, ModData.User, ModData.Password, ModData.Version);
                }
                else
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "缺失系统维护库连接信息文件：" + ModData.v_ConfigPath + "/n请重新配置");
                    return;
                }
                if (!blnCanConnect)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统能够维护库连接失败，请检查!");
                    return;
                }
                ModData.TempWks = vgisDb.WorkSpace;
            }
            if (ModData.TempWks == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库工作空间失败，请检查!");
                return;
            }
            #endregion
            //获得数据库工程ID
            //long proID = 0;            //工程项目ID
            //string proIDStr = m_Hook.ProjectTree.SelectedNode.Name;  //工程项目ID字符串
            //try { proID = Convert.ToInt64(proIDStr); }
            //catch
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据源ID失败!");
            //    return;
            //}

            //进行初始化设置
            FrmInitiaRasterCatalog pFrmInitiaRasterCatalog = new FrmInitiaRasterCatalog(ModData.TempWks, m_Hook,EnumRasterOperateType.Update);
            if(pFrmInitiaRasterCatalog.m_pRootPathfst!="")
            pFrmInitiaRasterCatalog.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
        }
    }
}
