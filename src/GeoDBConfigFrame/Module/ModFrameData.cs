using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using SysCommon.Gis;

namespace GeoDBConfigFrame
{
    public static class ModFrameData
    {
        public static Plugin.Application.AppPrivileges v_AppPrivileges;
        public static string v_ConfigPath = Application.StartupPath + "\\conn.dat";

        //记录数据操作时间
        public static DateTime v_OperatorTime;
        //数据库操作类
        public static SysGisDB gisDb;

        //======================================================
        //系统运行日志  cyf 20110518
        public static SysCommon.Log.clsWriteSystemFunctionLog v_SysLog;
        public static string m_SysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\SysXml.Xml";                 //所有系统界面xml
        //系统维护库连接信息 cyf 20110520
        public readonly static string v_AppDBConectXml = Application.StartupPath + "\\AppDBConectInfo.xml";////////////系统维护库连接字符串 
        //end=======================================================
    }

    public enum enumLayType
    {
        LEFT = 1,
        RIGHT = 2,
        TOP = 3,
        BOTTOM = 4,
        FILL = 5,
    }

    public static class SdeConfig
    {
        public static string dbType = "";
        public static string Server="";
        public static string Instance="";
        public static string Database="";
        public static string User="";
        public static string Password="";
        public static string Version="SDE.DEFAULT";
    }    
}
