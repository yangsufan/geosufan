using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace FileDBTool
{
    public static class ModData
    {
        public static Plugin.Application.AppFileDB v_AppFileDB;
        public static string v_CoonectionInfoXML = Application.StartupPath + "\\FTPConnectionInfo.xml";
        public static string v_ProjectInfoXML = Application.StartupPath + "\\Project.xml";
        public static string v_ProductInfoXML = Application.StartupPath + "\\Product.xml";//产品目录信息
        public static string v_TempControlDB = Application.StartupPath + "\\..\\CacheDB\\控制点数据元信息模板.mdb";//控制点元信息文件
        public static DevComponents.DotNetBar.ComboBoxItem v_ComboxTime = null;

        public static string v_tempErrLog = Application.StartupPath + "\\..\\Template\\FileErrLog.mdb";

        public static int recNum = 0;//上一个页面的最后一条记录
        public static int CurrentPage = 1;//当前页数
        public static int TotalPageCount;//总页数
        public static int pageSize = 50;//每页包含的记录数
        public static DataTable TotalTable = null;//更新对比列表显示源表
    }

    //项目树图节点类型变量
    public enum EnumTreeNodeType
    {
        DATACONNECT=0, //数据连接节点
        DATABASE=1,    //数据库节点
        PROJECT=2,     //项目节点
        DATAFORMAT=3,    //数据类型节点（DLG、DRG、DEM、DOM）
        PRODUCT=4,     //产品节点
        PRODUCTPYPE=5, //产品类型节点 （标准图幅、非标准图幅、属性表）   
        DATAITEM=6,    //数据项节点
    }

    //数据格式枚举型变量
    public enum EnumDataFormat
    {
        DLG=0,
        DEM=1,
        DOM=2,
        DRG=3
    }

    //数据类型枚举变量
    public enum EnumDataType
    {
        标准图幅 = 0,   //标准图幅
        非标准图幅 = 1, //非标准图幅
        控制点数据 = 2  //控制点数据
    }

    //控件的位置枚举变量
    public enum enumLayType
    {
        LEFT = 1,
        RIGHT = 2,
        TOP = 3,
        BOTTOM = 4,
        FILL = 5,
    }
}
