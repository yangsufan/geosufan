using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using SysCommon.Authorize;

namespace GeoDataCenterFunLib
{
    public static class Mod
    {
        public readonly static string m_SysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\GeoDBConfigSys.Xml";
        public readonly static string m_ResPath = Application.StartupPath + "\\..\\Res\\Pic";
        public readonly static string m_PluginFolderPath = Application.StartupPath + "\\..\\Bin";
        public readonly static string m_LogPath = Application.StartupPath + "\\..\\Log\\系统初始化";

        //连接信息
        public static string v_ConfigPath = Application.StartupPath + "\\conn.dat";
        public static string Server = "";
        public static string Instance = "";
        public static string Database="";
        public static string User = "";
        public static string Password = "";
        public static string Version = "";
        public static string dbType = "";

        public static User v_AppUser;
        public static XmlDocument v_SystemXml;
        public static Plugin.Application.AppForm v_AppForm;
    }
}
