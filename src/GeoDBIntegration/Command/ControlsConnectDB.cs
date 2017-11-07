using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SysCommon.Gis;
using System.IO;
using SysCommon.Authorize;

namespace GeoDBIntegration
{
    /// <summary>
    /// chenyafei  20100315  add content:连接已经存在的数据库，并初始化日志记录表格
    /// </summary>
   public class ControlsConnectDB: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppDBIntegraRef m_Hook;
        //added by chulili 20110624 为了判断可用状态添加变量
        private Plugin.Application.IAppFormRef _hook;
        public ControlsConnectDB()
        {
            base._Name = "GeoDBIntegration.ControlsConnectDB";
            base._Caption = "连接数据库";
            base._Tooltip = "连接数据库";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "连接数据库";
        }

        public override bool Enabled
        {
            get 
            {
                ////若没有登录系统，则按钮不可用
                //if (ModuleData.m_User== null) return false;
                ////若用户不是管理员，则按钮不可用
                //if (ModuleData.m_User.RoleTypeID != EnumRoleType.管理员.GetHashCode()) return false;
              
                if (m_Hook == null) return false;
                if (m_Hook.ProjectTree == null || m_Hook.MapControl == null || m_Hook.CurrentThread != null) return false;
                
                
                //cyf 20110603 modify
                //若没有登录系统，则按钮不可用
                if ((m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo == null) return false;
                //若用户不是管理员，则按钮不可用
                bool beAdmin = false;
                //added by chulili 20110624 若不处于数据源管理界面  菜单不可用
                if (!(_hook.MainForm.Controls[0] is UserControlDBIntegra))
                {
                    return false;
                }
                if (!_hook.MainForm.Controls[0].Visible)
                {
                    return false;
                }
                //end add
                foreach (Role pRole in (m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo)
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
            //连接数据库
            //if (ModuleData.v_AppConnStr.Trim() == "")
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库连接字符串失败！");
            //    return;
            //}

            //frmAddNewDB AddNewDB = new frmAddNewDB(SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, ModuleData.v_AppConnStr, EnumUpdateType.Update.ToString());
            //AddNewDB.ShowDialog(); 
            #endregion
            //判断配置文件是否存在
            if (ModuleData.TempWks == null)
            {
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
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统维护库连接失败，请检查!");
                    return;
                }
                ModuleData.TempWks = vgisDb.WorkSpace;
            }
            //cyf 20110615 add:
            if (ModuleData.TempWks == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库工作空间失败，请检查!");
                return;
            }
            //end
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
            frmAddNewDB AddNewDB = new frmAddNewDB(ModuleData.TempWks, EnumUpdateType.Update.ToString());
            AddNewDB.ShowDialog();
            // *end
            // ***********************************************************
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            //added by chulili 20110624
            _hook = hook as Plugin.Application.IAppFormRef;
            //end add
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

