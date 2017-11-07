using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataEdit
{
    /// <summary>
    /// 当前操作类型
    /// </summary>
    public enum DrawTypeConstant
    {
        /// <summary>
        /// 普通点
        /// </summary>
        CommonPoint ,

        /// <summary>
        /// 普通线
        /// </summary>
        CommonLine ,

        /// <summary>
        /// 普通面
        /// </summary>
        CommonPolygon ,

        /// <summary>
        /// 平行线
        /// </summary>
        ParallelLine ,

        /// <summary>
        /// 添加节点
        /// </summary>
        AddVertex ,

        /// <summary>
        /// 删除节点
        /// </summary>
        DeleteVertex ,

        /// <summary>
        /// 移动节点
        /// </summary>
        MoveVertex ,

        /// <summary>
        /// 删除要素
        /// </summary>
        DeleteSketch ,

        /// <summary>
        /// 移动要素
        /// </summary>
        MoveSketch ,

        /// <summary>
        /// 已知线的平行线(固定平行线)
        /// </summary>
        AnchorParallelLine ,
        /// <summary>
        /// 延伸线
        /// </summary>
        ExtendLine ,
        /// <summary>
        /// 打断线
        /// </summary>
        BreakLine ,
        /// <summary>
        /// 分解线、面
        /// </summary>
        ExplodeLine ,
        /// <summary>
        /// 改变线的方向
        /// </summary>
        ChangeLineDirection ,

        /// <summary>
        /// 过点作垂线
        /// </summary>
        VerticalLineFromPoint ,

        /// <summary>
        /// 比例缩放
        /// </summary>
        ScaleZoom ,

        /// <summary>
        /// 旋转要素
        /// </summary>
        RotateFeature ,

        /// <summary>
        /// 镜像要素
        /// </summary>
        MirrorFeature ,

        /// <summary>
        /// 修改要素属性
        /// </summary>
        ChangeFeatureEttri ,

        /// <summary>
        /// 选择要素
        /// </summary>
        SelectFeature ,

        /// <summary>
        /// 合并线
        /// </summary>
        UnionLine ,

        /// <summary>
        /// 合并面
        /// </summary>
        UnionPolygon ,

        /// <summary>
        /// 分割线
        /// </summary>
        SplitLine ,

        /// <summary>
        /// 分割面
        /// </summary>
        SplitPolygon ,

        /// <summary>
        /// 倒圆角
        /// </summary>
        InRoundAngle
    }

    public static class OperationParam
    {
        //键盘输入参数
        /// <summary>
        /// 退出
        /// </summary>
        public const string KeyEsc = "ESC";
        /// <summary>
        /// 完成
        /// </summary>
        public const string KeyEnter = "ENT";
        /// <summary>
        /// 回退
        /// </summary>
        public const string KeyUndo = "U";
        /// <summary>
        /// 下一个路径
        /// </summary>
        public const string KeyNextPart = "N";
        /// <summary>
        /// 长度
        /// </summary>
        public const string KeyLength = "L";
        /// <summary>
        /// 封闭
        /// </summary>
        public const string KeyClose = "C";
        /// <summary>
        /// 打断
        /// </summary>
        public const string KeyBreak = "B";
        /// <summary>
        /// 确认
        /// </summary>
        public const string KeyOK = "Y";
        /// <summary>
        /// 是
        /// </summary>
        public const string KeyYes = "Y";
        /// <summary>
        /// 否
        /// </summary>
        public const string KeyNo = "N";

        //放大缩放常数
        public const string KeyZoomOut = "ZOOMOUT";
        public const string KeyZoomIn = "ZOOMIN";
        public const string KeyPan = "PAN";

        //命令输入类型

        /// <summary>
        /// 命令
        /// </summary>
        public const string CheckCommand = "Command";                     //
        /// <summary>
        /// 多级命令输入
        /// </summary>
        public const string CheckPara = "Param";                          //
        /// <summary>
        /// 关键字
        /// </summary>
        public const string CheckChar = "Char";                           //
        /// <summary>
        /// 点坐标
        /// </summary>
        public const string CheckPoint = "Point";                         //
        /// <summary>
        /// 长度
        /// </summary>
        public const string CheckLength = "Length";                       //
        /// <summary>
        /// 整数
        /// </summary>
        public const string CheckLong = "Long";                           //
        /// <summary>
        /// 分数
        /// </summary>
        public const string CheckDouble = "Double";                       //
        /// <summary>
        /// 不接受输入
        /// </summary>
        public const string CheckNothing = "Nothing";                     // 
        /// <summary>
        /// 字符串
        /// </summary>
        public const string CheckString = "String";                       //
        /// <summary>
        /// 角度
        /// </summary>
        public const string CheckAngle = "Angle";                         //
        /// <summary>
        /// 点坐标或者命令参数
        /// </summary>
        public const string CheckPointChar = "PC";                        //
        /// <summary>
        /// 点坐标或者长度
        /// </summary>
        public const string CheckPointLength = "PL";                      //
        /// <summary>
        /// 点坐标或者角度
        /// </summary>
        public const string CheckPointAngle = "PA";                      //
        /// <summary>
        /// 长度或者命令参数
        /// </summary>
        public const string CheckLengthChar = "LC";                       //
        /// <summary>
        /// 退出选择
        /// </summary>
        public const string CheckEscToSelect = "EscToSelect";             //
        /// <summary>
        /// 选择开始
        /// </summary>
        public const string CheckSelectBegin = "SelectBegin";              //
        /// <summary>
        /// 选择结束
        /// </summary>
        public const string CheckSelectEnd = "SelectEnd";                 //
        /// <summary>
        /// 选择正确
        /// </summary>
        public const string CheckEndSelectOK = "EndSelectOK";             //
        /// <summary>
        /// 回退等的更新当前点坐标
        /// </summary>
        public const string CheckUpdate = "Update";                       // 
        /// <summary>
        /// mousedown等的更新当前点坐标
        /// </summary>
        public const string CheckUpOut = "UpOut";                         //
        /// <summary>
        /// 结束等的更新当前点坐标
        /// </summary>
        public const string CheckEmpty = "Empty";                         //
        /// <summary>
        /// 对象初始化
        /// </summary>
        public const string CheckInit = "Init";                           //
        /// <summary>
        /// 退出当前命令
        /// </summary>
        public const string CheckESC = "ESC";                             //
        /// <summary>
        /// 按键信息转发
        /// </summary>
        public const string CheckKEYDOWN = "KEYDOWN";                     //
        /// <summary>
        /// 退出当前命令
        /// </summary>
        public const string CheckSubBegin = "SubBegin";                   //
        /// <summary>
        /// 按键信息转发
        /// </summary>
        public const string CheckSubEnd = "SubEnd";                       //
        /// <summary>
        /// 子命令开始
        /// </summary>
        public const string CheckTempCommandBegin = "TempCommandBegin";   //
        /// <summary>
        /// 退出子命令
        /// </summary>
        public const string CheckTempCommandEnd = "TempCommandEnd";       //
        public const string CheckSelectTool = "SELECTFEATURETOOL";         //

        //选择类型
        public const string SelectPoint = "SD";
        public const string SelectPolyline = "SX";
        public const string SelectPolygon = "SM";
        public const string SelectAll = "SA";
        public const string SelectCur = "SC";

        //右键菜单常数
        public const string MenuCompelte = "完成";
        public const string MenuUndo = "回退";
        public const string MenuCancel = "放弃";
        public const string MenuPan = "平移";
        public const string MenuZoomIn = "放大";
        public const string MenuZoomOut = "缩小";


        //命令提示常数
        public const string TipUndoInvalid = "#回退无效！";
        public const string TipUndoOK = "#回退成功！";


        public const string TipPolylineEnd = "#多边线要素绘制完成！";
        public const string TipPolylineFail = "#多边线要素绘制失败！";




    }

    //1为普通点，2普通线，3为普通面，4为平行线，5添加节点，6删除节点，7移动节点，8删除要素,9移动要素,
    //10画已知线的平行线,11改变线的方向,12过点作垂线,13比例缩放,14要素转动,15要素属性修改,16合并线,
    //17合并面,18分割线,19分割面,20画注记,21修改注记,22选择
    //23倒圆角，24倒直角，25延伸线，26修剪线，27镜象，28投影,29分解线,30打断线

    /// <summary>
    /// 当前命令的执行结果
    /// </summary>
    /// <param name="strMsg"></param>
    public delegate void RefreshCommandHandler ( string strMsg );

    ///// <summary>
    ///// 下步操作的提示反馈
    ///// </summary>
    ///// <param name="sCommandType"></param>
    ///// <param name="sTipInfo"></param>
    ///// <param name="sKeyword"></param>
    //public delegate void GetCommandTipEventHandler(string sCommandType, string sTipInfo, string sKeyword);

    ///// <summary>
    ///// 使命令行控件获得焦点
    ///// </summary>
    //public delegate void SetFocusEventHandler();

    ///// <summary>
    ///// 弹出当前Tool的右键菜单
    ///// </summary>
    ///// <param name="ToolRightMenu"></param>
    //public delegate void PopupMenuEventHandler(Dictionary<string, bool> ToolRightMenu);

    /// <summary>
    /// 画笔迹时主动刷新Toolbar
    /// </summary>
    public delegate void RefreshTBEventHandler ();
}
