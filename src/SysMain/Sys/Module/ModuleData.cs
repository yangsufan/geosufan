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
        public readonly static string m_ResPath = Application.StartupPath + "\\..\\Res\\Pic";
        public readonly static string m_PluginFolderPath = Application.StartupPath + "\\..\\Plugins";
        public readonly static string m_LogPath = Application.StartupPath + "\\..\\Log\\系统初始化";
        /// <summary>
        /// 业务操作连接
        /// </summary>
        public static IDBOperate m_SysDbOperate = null;
        /// <summary>
        /// 系统登录用户
        /// </summary>
        public static User m_LoginUser = null;
        /// <summary>
        /// 系统配置
        /// </summary>
        public static SysConfig m_sysConfig = null;

        public static bool LoginState = false;

        public static Fan.Plugin.Application.AppForm v_AppForm;

        public readonly static string v_AppDBConectXml = Application.StartupPath + "\\AppDBConectInfo.xml";

        public static string v_SystemFunctionDBSchema = Application.StartupPath + "\\..\\Res\\Schema\\SystemFunctionDBConfiguration.sql";

        public static XmlDocument v_SystemXml;

        public static Fan.Common.Log.clsWriteSystemFunctionLog v_SysLog;

        public static string v_UserInfoPath = Application.StartupPath + "\\user.dat";
        public static string v_ConfigPath = Application.StartupPath + "\\conn.dat";
    }
}
