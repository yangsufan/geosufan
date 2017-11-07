using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using System.Collections;
using System.Xml;
using System.Drawing;
using System.Data;

using ESRI.ArcGIS.Controls;


namespace GeoDBATool
{
    public static class ModData
    {
        public static Plugin.Application.AppGIS v_AppGIS;
        public static int recNum = 0;//上一个页面的最后一条记录
        public static int CurrentPage = 1;//当前页数
        public static int TotalPageCount;//总页数
        public static int pageSize = 50;//每页包含的记录数
        public static DataTable TotalTable = null;//更新对比列表显示源表

        public readonly static string v_projectXMLTemp = Application.StartupPath + "\\..\\Res\\Schema\\DatabaseProject.xml";
        public readonly static string v_projectXML = Application.StartupPath + "\\DatabaseProject.xml";
        //cyf 20110624 20110624 add:初始化工程树图额xml
        public readonly static string v_projectDetalXMLTemp = Application.StartupPath + "\\..\\Res\\Schema\\DatabaseDetalProject.xml";
        public readonly static string v_projectDetalXML = Application.StartupPath + "\\DatabaseDetalProject.xml";
        //end
        public readonly static string v_DbInfopath = Application.StartupPath + "\\..\\Template\\DbInfoTemplate.mdb";

        public readonly static string v_JoinSettingXML = Application.StartupPath + "\\MapFrameJoinSetting.xml";
        public readonly static string v_JoinLOGXML = Application.StartupPath + "\\MapFrameJoinLOGTemplet.xml";

        public static string MapPath = Application.StartupPath + @"\..\CacheDB\Map.mdb";
        public static string countyPath = Application.StartupPath + @"\..\CacheDB\Range.mdb";

        //协同更新远程日志表
        public static string netLogFile = Application.StartupPath + @"\..\Template\Network_Log.mdb";

        public static string DBImportPath = Application.StartupPath + @"\..\Res\rule\数据移植\批量数据入库.xml";  //数据批量入库规则

        //图幅批量数据处理的xml映射关系
        public static string DBTufuInputXml = Application.StartupPath + @"\..\Res\rule\数据移植\图幅数据入工作库.xml";  //图幅数据入工作库规则
        public static string DBTufuStractXml = Application.StartupPath + @"\..\Res\rule\数据移植\图幅接边数据提取.xml";   //获取图幅接边数据
        public static string DBTufuSubmitXml = Application.StartupPath + @"\..\Res\rule\数据移植\图幅数据更新入库.xml";   //获取图幅接边数据

        public static string temporaryDBPath = Application.StartupPath + @"\..\CacheDB\tempRasterDtDB.gdb";            //临时栅格数据数据库存储路径
        //cyf 20110609 add
        public static string RasterInDBTemp = Application.StartupPath + @"\..\Template\RasterInDBTemp.mdb";            //存储入库的栅格数据的模板
        public static string RasterInDBLog = Application.StartupPath + "\\RasterInDBLog.mdb";                        //存储入库的栅格数据路径
        //end
        public static string v_DbInterConn = Application.StartupPath + "\\AppDBConectInfo.xml";    //系统维护库连接字符串

        public static Dictionary<IFeatureLayer, IFeatureRenderer> m_DicFeaLayerRender = new Dictionary<IFeatureLayer, IFeatureRenderer>();  //保存渲染的符号

        //更新日志表和版本信息表  一致性更新
        public static string m_sUpDataLOGTable = "GO_DATABASE_UPDATELOG";///////远程更新日志表
        public static string m_sDBVersionTable = "GO_DATABASE_VERSION";/////////数据库版本表
        public static int DBVersion = 0;          //版本

        public static IWorkspace m_ObjWS;                           //目标工作空间
        public static IFeature m_OrgFeature;                        //参照要素
        public static IMap m_orgMap;                                //参照图层

        //public static ComboBox m_ComboBox;                                        //图层列表框

        public static ILayer m_CurLayer;                                          //当前图层

        public static EnumUpdateType m_CurOperType;                            //当前操作类型

        public static AxMapControl m_Mapcontrol;                                //子窗口控件

        //*****************************************************************************************
        // System Function Log
        public static SysCommon.Log.clsWriteSystemFunctionLog SysLog;
        //*****************************************************************************************



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


        ////////一致性更新对比窗口
        public static frmCompareData UpDataCompareFrm;

        //guozheng 2011-5-6 更新发布相关
        //发布更新变量 远程发布
        public static CSendUpdateMsg v_RemoteMsg = null;       //本地通讯
        public static GeoMsgCenterCOMLib.MessageSession v_RemoteSesion = null; //session
        public static CSendUpdateMsg v_Msg = null;       //本地通讯
        public static GeoMsgCenterCOMLib.MessageSession v_Sesion = null; //session


        //临时库连接信息
        //public static ESRI.ArcGIS.Geodatabase.IWorkspace TempWks = null;
        //public static string v_ConfigPath = Application.StartupPath + "\\conn.dat";//系统维护库连接配置文件
        //public static string Server = "";
        //public static string Instance = "";
        //public static string Database = "";
        //public static string User = "";
        //public static string Password = "";
        //public static string Version = "";
        //public static string dbType = "";
  
    }
    public enum EnumOperateType
    {
        NULL=0,
        Input = 1,
        Stract = 2,
        Submit=3,
        OutputUpdateData=4,
        UserDBInput=5
    }
    public enum enumLayType
    {
        LEFT = 1,
        RIGHT = 2,
        TOP = 3,
        BOTTOM = 4,
        FILL = 5,
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

    public enum EnumFeatureType
    {
        参照要素 = 1,  //参照要素
        更新要素 = 2  //需要更新要素
    }

    public enum EnumUpdateType
    {
        新增 = 1,           //新增
        修改 = 2,           //修改
        删除 = 3            //删除
    }

    //cyf 20110602 modify:定义角色类型
    public enum EnumRoleType
    {
        管理员 = 1,    //管理员
        作业员 = 2,
        普通用户 = 3,
        质检员 = 4,
    }

    public enum EnumRasterOperateType
    {
        Input=1,            //入库
        Update=2             //更新
    }
       
}
