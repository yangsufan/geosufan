using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Xml;
using Fan.DataBase;
using Fan.Common;

namespace GDBM
{
    public static class Mod
    {
        public static string m_SysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\SysXml.Xml";
        public static SystemTypeEnum m_SystemType = SystemTypeEnum.ManagerSystem;
        public readonly static string m_ResPath = Application.StartupPath + "\\..\\Res\\Pic";
        public readonly static string m_PluginFolderPath = Application.StartupPath + "\\..\\Plugins";
        public readonly static string m_LogPath = Application.StartupPath + "\\..\\Log\\系统初始化";
        public static string filestr = Application.StartupPath + "\\dbSet.txt";
        /// <summary>
        /// 业务操作连接
        /// </summary>
        public static IDBOperate m_SysDbOperate = null;
        public static User m_LoginUser = null;
        public static bool LoginState = false;//控件登陆后，最终是否显示主界面

        public static Fan.Plugin.Application.AppForm v_AppForm;

        public readonly static string v_AppDBConectXml = Application.StartupPath + "\\AppDBConectInfo.xml";////////////系统维护库连接字符串 
        /////guozheng 2011-2-14 added 系统维护库库体定义模板文件
        public static string v_SystemFunctionDBSchema = Application.StartupPath + "\\..\\Res\\Schema\\SystemFunctionDBConfiguration.sql";
        //连接信息
        //cyf 20110602 add:连接用户对应的角色信息列表
        //end
        public static XmlDocument v_SystemXml;

        public static Fan.Plugin.Parse.PluginCollection m_PluginCol;      //插件口袋

        ///////////////////////////////////////系统运行日志
        public static Fan.Common.Log.clsWriteSystemFunctionLog v_SysLog;

        public static string v_UserInfoPath = Application.StartupPath + "\\user.dat";//added by chulili 20110707 记住密码文件

        //临时库连接信息
        public static ESRI.ArcGIS.Geodatabase.IWorkspace TempWks = null;
        public static string v_ConfigPath = Application.StartupPath + "\\conn.dat";//南京项目系统维护库连接配置文件
        public static string Server = "";
        public static string Instance = "";
        public static string Database = "";
        public static string User = "";
        public static string Password = "";
        public static string Version = "";
        public static string dbType = "";

        //正式库连接信息
        public static ESRI.ArcGIS.Geodatabase.IWorkspace CurWks = null;
        public static string CurServer = "";
        public static string CurInstance = "";
        public static string CurDatabase = "";
        public static string CurUser = "";
        public static string CurPassword = "";
        public static string CurVersion = "";
        public static string CurdbType = "";

        public static List<string> v_ListUserPrivilegeID;
        public static List<string> v_ListUserdataPriID;
    }

    // *=======================================================
    // *开发者：陈亚飞
    // *时  间：20110520
    // *功  能：申明用户枚举类型
    public enum UserTypeEnum
    {
        SuperAdmin=0,          //超级管理员，用来设置用户权限，分配用户等
        Admin=1,               //管理员，首先进入集成管理系统
        CommonUser=2           //普通用户
    }
    //===================================================
    // *=======================================================
    // *开发者：褚丽丽
    // *时  间：20120809
    // *功  能：申明子系统枚举类型
    public enum SystemTypeEnum
    {
        ManagerSystem = 0,          //数据展示子系统
        ConfigSystem = 1,               //配置子系统
        UpdateSystem = 2           //入库更新子系统
    }
    //===================================================
}
