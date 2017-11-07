using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoEdit
{
    /// <summary>
    /// 编辑时的状态 及编辑空间的定义
    /// </summary>
    public static class MoData
    {
        public static IWorkspaceEdit v_CurWorkspaceEdit;//定义一个全局的编辑空间

        public static SysCommon.DataBase.SysTable v_LogTable;//日志记录表

        public static ILayer v_CurLayer;               //当前操作图层

        public static bool v_bVertexSelectionTracker;  //判断节点编辑中的选择节点，用于刷新画节点

        public static bool v_bSnapStart;               //是否开启捕捉
        public static double v_SearchDist;             //捕捉半径
        public static double v_CacheRadius;            //缓冲半径
        public static Dictionary<ILayer, ArrayList> v_dicSnapLayers; //存储捕捉的图层及设置(节点捕捉、端点捕捉、相交点捕捉、中点捕捉、最近点捕捉)

        public static Hashtable GetOnlyReadAtt = new Hashtable();//不可修改属性
        public static int DBVersion = 0;
    }


    /// <summary>
    /// 用来控制属性编辑显示时的状态
    /// </summary>
    public static class AttributeShow_state
    {
        public static Hashtable hs_Feature;//将我们的源要素属性存起来
        public static string OID = "";//将源OID存起来
        public static int feature_count = 0;//要素字段个数，确定一个源，后面的个数都是一样的。
        public static bool state_brush = false;//定义一个状态，属性刷使用
        public static bool show_state = false;//一开始是不显示的
        public static bool state_value = false;//确定地图上是否有选中的值
        public static bool doubleclick = false;//地图选择集的状态，是否有改变
        public static FrmAttribute Temp_frm;//一个临时用来存放属性的窗体
        public static FrmAttribute4Merge Temp_frm4Merge;//一个临时用来存放属性的窗体

    }
}
