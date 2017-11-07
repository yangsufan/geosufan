using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using SysCommon.Gis;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace GeoSysUpdate
{
    public enum enumOperatorType
    {
        Null = -1,
        Stract = 1,
        Output = 2,
        Input = 3,
        Update = 4,
        Validate = 5,
        Submit = 6
    }

    public enum enumLayType
    {
        LEFT = 1,
        RIGHT = 2,
        TOP = 3,
        BOTTOM = 4,
        FILL = 5,
    }

    public enum enumTaskType
    {
        MANAGER=1,
        ZONE=2,
        INTERZONE=3,
        CHECK=4,
        SPOTCHECK=5
    }

    public enum enumProjectState
    {
        未完成=0,
        完成=1
    }

    public enum enumFeatureUpdate
    {
        增加 = 1,
        删除= 2,
        修改=3
    }

    public enum enumZoneState
    {
        无 = 0,
        任务已分配 = 1,
        数据已下载 = 2,
        已提交到工作库 = 3,
        检查未通过 = 4,
        检查通过 = 5,
        已经提交到现势库 = 6
    }

    public enum enumInterZoneState
    {
        无 = 0,
        任务已分配 = 1,
        正在修编 = 2,
        检查未通过 = 3,
        完成修编 = 4
    }

    public enum enumMapState
    {
        无 = 0,
        正在修编 = 1,
        检查未通过 = 2,
        完成修编 = 3
    }

    public enum enumMetaType
    {
        ProjectInfo,
        Zone,
        InterZone,
        Map,
        FeatureUpdate
    }

    public static class ModData
    {
        public static string _XZQpath = Application.StartupPath + "\\..\\Res\\Xml\\XZQ.xml";
        public readonly static string v_CachedDBPath = Application.StartupPath + "\\..\\CacheDB\\map.mdb";
        public readonly static string v_LogDBPath = Application.StartupPath + "\\..\\CacheDB\\Log.mdb";
        public readonly static string v_projectDetalXML = Application.StartupPath + "\\DatabaseDetalProject.xml";

        public static Plugin.Application.AppGidUpdate v_AppGisUpdate;
        public static IFeatureClass v_CurMapSheetFeatureClass;

        //鼠标点击位置
        public static ESRI.ArcGIS.Geometry.IPoint v_CurPoint;
        //数据操作类(配置)
        public static SysGisDataSet v_SysDataSet;

        //数据操作类(库)
        public static SysGisDataSet v_SysSet;

        //数据操作类(图幅元数据)
        public static SysGisTable v_SysMapTable;

        //记录数据操作时间
        public static DateTime v_OperatorTime;

        //是否需要重新刷新更新对比列表
        public static bool v_RefreshUpdateDataInfo;

        //当前图幅比例尺
        public static string Scale;

        //当前任务类型
        public static enumTaskType v_CurrentTaskType;


        //图库控制树图
        public static System.Xml.XmlDocument v_DataViewXml;

        //*****************************************************************************************
        // System Function Log
        public static SysCommon.Log.clsWriteSystemFunctionLog SysLog;
        //*****************************************************************************************
        //连接信息
        public static string v_ConfigPath = Application.StartupPath + "\\conn.dat";
        public static string dbType = "";
        public static string Server = "";
        public static string Instance = "";
        public static string Database="";
        public static string User = "";
        public static string Password = "";
        public static string Version = "";
     }
    public static class ModFrameData
    {
        public static Plugin.Application.AppGidUpdate v_AppGisUpdate;
        public static IFeatureWorkspace m_pFeatureWorkspace;
        //?  public static IFeatureClass v_CurMapSheetFeatureClass;
        //?  public static frmQueryOperationRecords v_QueryResult;

        //鼠标点击位置
        public static ESRI.ArcGIS.Geometry.IPoint v_CurPoint;

        //连接信息
        public static string v_ConfigPath = Application.StartupPath + "\\conn.dat";
        public static string dbType = "";
        public static string Server = "";
        public static string Instance = "";
        public static string Database = "";
        public static string User = "";
        public static string Password = "";
        public static string Version = "DATACENTER.DEFAULT";

        //======================================================
        //系统运行日志  cyf 20110518
        public static SysCommon.Log.clsWriteSystemFunctionLog v_SysLog;
        public static string m_SysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\SysXml.Xml";                 //所有系统界面xml
        //系统维护库连接信息 cyf 20110520
        public readonly static string v_AppDBConectXml = Application.StartupPath + "\\AppDBConectInfo.xml";////////////系统维护库连接字符串 
        //end=======================================================
    }

}
