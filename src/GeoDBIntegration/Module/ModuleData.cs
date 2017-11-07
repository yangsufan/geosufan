using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using SysCommon.Authorize;

using System.Xml;
namespace GeoDBIntegration
{
    /// <summary>
    /// 公共静态类
    /// </summary>
    public static class ModuleData
    {

        //***************************************************************************************************************************************
        //陈亚飞 添加
        //public static Role m_Role = null;
        public static User m_User = null;                                                                           //记录当前登录用户信息
        public static  bool m_BeSuperUser = true;                                                                  //标志是否超级管理员
        public static Plugin.Application.AppDBIntegra  v_AppDBIntegra;                                              //集成管理子系统接口类变量

        public static string m_SysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\SysXml.Xml";                 //所有系统界面xml
        public static string m_FeaArcSysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\ArcGIS框架要素库界面.Xml";      //框架要素库界面xml路径(ArcGIS平台)
        public static string m_FileSysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\文件库界面.Xml";         //文件库界面xml路径
        public static string m_AddreessSysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\地名数据库界面.Xml"; //地名数据库界面xml路径
        public static string m_EntiSysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\地理编码库界面.Xml";     //地理实体数据库界面xml路径
        public static string m_ImageSysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\影像数据库界面.Xml";  //影像数据库界面xml路径
        public static string m_DemSysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\高程数据库界面.Xml";      //高程数据库界面xml路径
        public static string m_SubjectSysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\专题数据库界面.Xml";  //专题数据库界面xml路径
        public static string m_MapSysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\电子地图数据库界面.Xml";  //电子地图数据库界面xml路径

        public static string m_FeaGeoSysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\GeoStar框架要素库界面.Xml";      //框架要素库界面xml路径(GeoStar平台)
        public static string m_FeaOracleSysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\Oracle框架要素库界面.Xml";      //框架要素库界面xml路径(OracleSpatial平台)
       
        public readonly static string m_PicPath = Application.StartupPath + "\\..\\Res\\Pic";                       //界面图标资源路径
        public readonly static string m_PluginFolderPath = Application.StartupPath + "\\..\\Plugins";               //插件文件路径
        public readonly static string m_SysLogPath = Application.StartupPath + "\\..\\Log\\系统初始化";             //界面加载日志
        public static Plugin.Application.AppForm v_AppForm;                                                         //系统当前窗体
        public static Dictionary<string,Plugin.Application.AppForm> v_AppFormDic = new Dictionary<string,Plugin.Application.AppForm>();  //系统所有窗体

        public static Form v_MainForm = null;                                                                       //系统主界面   
        public static Form v_SubForm = null;                                                                        //子系统当前界面  

        public readonly static string v_feaProjectXMLTemp = Application.StartupPath + "\\..\\Res\\Schema\\DatabaseProject.xml";    //框架要素库子系统工程树节点xml模板
        public readonly static string v_feaProjectXML = Application.StartupPath + "\\DatabaseProject.xml";                         //框架要素库子系统当前工程树节点xml  

        public readonly static string v_DEMProjectXmlTemp = Application.StartupPath + "\\..\\Res\\Schema\\DemDatabaseProject.xml"; //高程数据库子系统工程树节点xml模板
        public readonly static string v_DEMProjectXml= Application.StartupPath + "\\DemDatabaseProject.xml";                       //高程数据库子系统当前工程树节点xml

        public readonly static string v_ImageProjectXmlTemp = Application.StartupPath + "\\..\\Res\\Schema\\ImageDatabaseProject.xml"; //影像数据库子系统工程数节点xml模板
        public readonly static string v_ImageProjectXml= Application.StartupPath + "\\ImageDatabaseProject.xml";                  //影像数据库子系统当前工程树节点xml

        public readonly static string v_FTPDbSchameFile = Application.StartupPath + "\\..\\Template\\MetaDataBase.mdb";//////////////////////////////////////文件库模板文件

        public static Plugin.Parse.PluginCollection m_PluginCol = null;                  //插件口袋

        /////guozheng 2011-2-14 added 系统维护库库体定义模板文件
        public static  string v_SystemFunctionDBSchema=Application.StartupPath + "\\..\\Res\\Schema\\SystemFunctionDBConfiguration.sql";

        //****************************************************************************************************************************************

        //guozheng 2010-9-29
        public readonly static string v_AppDBConectXml = Application.StartupPath + "\\AppDBConectInfo.xml";////////////系统维护库连接字符串  
        public readonly static string v_FTPCoonectionInfoXML = Application.StartupPath + "\\FTPConnectionInfo.xml";///////文件库工程信息
        public static string v_AppConnStr = string.Empty;/////////////////////////////////////////////////////////////////系统维护库连接字符串
        ////////////OracleSpatial相关//////////
        public static string s_Packname = "GETSDOORDINATEPACK";///////程序包体名
        public static string s_PackFunNameG = "GETCOORS";//////设置坐标函数名
        public static string s_PackFunNameS = "SETCOORS";//////获取坐标函数名
        public static string s_GeometryFieldName = "SHAPE";//////几何字段名
        public static string s_KeyFieldName = "OBJECTID";//////主码字段名
        public static string s_GetGeometry = "GETGEOMETRY";/////用于存储几何对象的包体  

        ///////////////////////////////////////系统运行日志
        public static SysCommon.Log.clsWriteSystemFunctionLog v_SysLog;

        /////////////////////////////////////数据库面板类//
        public static clsDatabasePanel v_DataBaseProPanel = null;


        //连接信息  added by chulili 20110531
        public static User v_AppUser;
        public static XmlDocument v_SystemXml;

        //public static Plugin.Parse.PluginCollection m_PluginCol;      //插件口袋

        ///////////////////////////////////////系统运行日志
        //public static SysCommon.Log.clsWriteSystemFunctionLog v_SysLog;


        //临时库连接信息
        public static ESRI.ArcGIS.Geodatabase.IWorkspace TempWks = null;
        public static string v_ConfigPath = Application.StartupPath + "\\conn.dat";//系统维护库连接配置文件
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
        //end   added by chulili 20110531


        //cyf  20110626 add:记录历史连接信息
        public readonly static string v_HistoryConnXML = Application.StartupPath + "\\HistoryConnectionInfo.xml";
        //end
    }

    /// <summary>
    /// 定义图层的位置

    /// </summary>
    public enum enumLayType
    {
        LEFT = 1,
        RIGHT = 2,
        TOP = 3,
        BOTTOM = 4,
        FILL = 5,
    }

    //cyf 20110602 modify:定义角色类型
    public enum EnumRoleType
    {
        管理员=1,    //管理员
        作业员=2,    
        普通用户=3, 
        质检员=4,
    }
    #region 定义的枚举型
    public enum enumInterDBType///////数据库类型
    {
        成果文件数据库 = 1,
        框架要素数据库 = 2,
        高程数据库 = 3,
        影像数据库 = 4,
        地名数据库 = 5,
        地理编码数据库 = 6,
        电子地图数据库 = 7,
        专题要素数据库 = 8
    }
    public enum enumInterDBFormat///////数据库平台类型
    {
        ARCGISPDB = 1,
        ARCGISGDB = 2,
        ARCGISSDE = 3,
        ORACLESPATIAL = 4,
        GEOSTARACCESS = 5,
        GEOSTARORACLE = 6,
        GEOSTARORSQLSERVER = 7,
        FTP = 8
    }

    public enum EnumUpdateType    //添加数据库工程、连接已有的数据库
    {
        New = 1,
        Update = 2
    }

    public enum EnumDBState
    {
        库体未初始化=1,
        库体已初始化=2,
        已连接=3,
        未连接=4,
    }

    #endregion
}
