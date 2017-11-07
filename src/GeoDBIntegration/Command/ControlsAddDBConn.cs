using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using SysCommon.Authorize;
using SysCommon.Gis;

namespace GeoDBIntegration
{
    public class ControlsAddDBConn: Plugin.Interface.CommandRefBase
    {
      
        private Plugin.Application.IAppDBIntegraRef m_Hook;
        //added by chulili 20110624 为了判断可用状态添加变量
        private Plugin.Application.IAppFormRef _hook;
        public ControlsAddDBConn()
        {
            base._Name = "GeoDBIntegration.ControlsAddDBConn";
            base._Caption = "配置数据库连接信息";
            base._Tooltip = "配置数据库连接信息";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "配置数据库连接信息";

        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.ProjectTree == null || m_Hook.MapControl == null || m_Hook.CurrentThread != null) return false;

                //cyf 20110602 modify
                //若没有登录系统，则按钮不可用
                if ((m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo == null) return false;
                //若用户不是管理员，则按钮不可用
                bool beAdmin = false;
                //added by chulili 20110624 若不处于数据源管理界面  菜单不可用
                if (!(_hook.MainForm.Controls[0] is UserControlDBIntegra))
                {
                    return false;
                }

                if (_hook.MainForm.Controls[0].Visible == false)
                {
                    return false;
                }

                //end add
                foreach(Role pRole in (m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo)
                {
                    if (pRole.TYPEID == EnumRoleType.管理员.GetHashCode().ToString())
                    {
                        beAdmin = true;
                        break;
                    }
                }
                return true;
                //end
               
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

        public override void OnClick()
        {
            // *********************************************
            // *cyf
            // *modify
            // *读取系统维护库连接信息
            // *20110602
            #region 原有代码 
            ////执行添加数据库操作
            //XmlDocument XmlDoc = new XmlDocument();
            //string sConnect = string.Empty;
            //if (File.Exists(ModuleData.v_AppDBConectXml))
            //{
            //    XmlDoc.Load(ModuleData.v_AppDBConectXml);
            //    XmlElement ele = XmlDoc.SelectSingleNode(".//系统维护库连接信息") as XmlElement;
            //    if (ele != null)
            //    {
            //        try
            //        {
            //            sConnect = ele.GetAttribute("连接字符串");
            //        }
            //        catch
            //        {
            //            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库连接信息失败");
            //            return;
            //        }
            //    }
            //    else { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库连接信息失败"); return; }
            //}
            //else
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "缺失系统维护库连接信息文件："+ModuleData.v_AppDBConectXml);
            //    return;
            //}
            //frmAddNewDB AddNewDB = new frmAddNewDB(SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE,sConnect,EnumUpdateType.New.ToString());
            //AddNewDB.ShowDialog();
            #endregion

            //判断配置文件是否存在
            bool blnCanConnect = false;
            SysCommon.Gis.SysGisDB vgisDb = new SysGisDB();
            if (File.Exists(ModuleData.v_ConfigPath))
            {
                //获得系统维护库连接信息
                SysCommon.Authorize.AuthorizeClass.GetConnectInfo(ModuleData.v_ConfigPath, out ModuleData.Server, out ModuleData.Instance, out ModuleData.Database, out ModuleData.User, out ModuleData.Password, out ModuleData.Version, out ModuleData.dbType);
                //连接系统维护库
                blnCanConnect = CanOpenConnect(vgisDb, ModuleData.dbType, ModuleData.Server, ModuleData.Instance, ModuleData.Database, ModuleData.User, ModuleData.Password, ModuleData.Version);
            }
            else
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "缺失系统维护库连接信息文件：" + ModuleData.v_ConfigPath + "/n请重新配置");
                return;
            }
            if (!blnCanConnect)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统能够维护库连接失败，请检查!");
                return;
            }
            ModuleData.TempWks = vgisDb.WorkSpace;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
            frmAddNewDB AddNewDB = new frmAddNewDB(vgisDb.WorkSpace, EnumUpdateType.New.ToString());
            AddNewDB.ShowDialog();
            // *end
            // ***********************************************************
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            //added by chulili 20110624
            _hook = hook as Plugin.Application.IAppFormRef;
            //end add
            if (m_Hook == null) return;
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

    }
}