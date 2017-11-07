using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data.Common;
using System.Threading;

using ESRI.ArcGIS.Geodatabase;

namespace GeoDataChecker
{
    //单例模式
    public class GeoDataChecker
    {
        //连接配置参数信息
        //public static string GeoCheckParaPath = Application.StartupPath + "\\..\\Res\\checker\\GeoCheckPara.mdb";
        public static string DBSchemaPath = Application.StartupPath + "\\..\\Template\\DBSchema.mdb";
        //日志模板
        public static string LogTemplePath = Application.StartupPath + "\\..\\Template\\Log.mdb";
        //功能配置xml
        public static string GeoCheckXmlPath = Application.StartupPath + "\\..\\Res\\checker\\GeoDataCheck.Xml";

        //进度条
        public static DevComponents.DotNetBar.Controls.ProgressBarX _PrgressBarOut;
        public static DevComponents.DotNetBar.Controls.ProgressBarX _ProgressBarInner;
        public static Form _CheckForm;
        public static DevComponents.DotNetBar.LabelX _Label;

        private static long eErrorCount=0;
        public static long EERrorCount
        {
            set
            {
                eErrorCount = value;
            }
            get
            {
                return eErrorCount;
            }
        }


        private static GeoDataChecker instance;

        //数据检查接口
        private static Dictionary<string, IDataCheck> DicCheck;
        public static Dictionary<string, IDataCheck> DicDataCheck
        {
            get
            {
                return DicDataCheck;
            }
        }
        //数据拓扑检查
        private static Dictionary<string,string> m_DicTopoDataCheck;
        public static Dictionary<string, string> DicTopoDataCheck
        {
            get 
            {
                return m_DicTopoDataCheck;
            }
            set 
            {
                m_DicTopoDataCheck = value;
            }
        }

        //数据检查公共参数
        private static IDataCheckHook Hook;
        public static IDataCheckHook DataCheckHook
        {
            get
            {
                return Hook;
            }
        }

        public static GeoDataChecker GetInstance()
        {
            if (instance == null)
            {
                instance = new GeoDataChecker();
            }

            return instance;
        }

         public static void DoDispose()
         {
             if (instance != null)
             {
                 instance=null;
             }
         }

        private GeoDataChecker()
        {
            //获取数据检查实现类
            DataCheckPluginHandle dataCheckPluginHandle = new DataCheckPluginHandle(Application.StartupPath + "\\..\\Plugins", "GeoDataChecker.dll");
            DataCheckPluginCollection dataCheckPluginCollection = dataCheckPluginHandle.GetPluginFromDLL();

            DicCheck = new Dictionary<string, IDataCheck>();
            foreach (IDataCheck plugin in dataCheckPluginCollection)
            {
                DicCheck.Add(plugin.ToString(), plugin);
            }
        }

        //初始化检查相关参数
        public void InitialCheckPara(IWorkspace workspace, DbConnection dbConnPara, string dbConnParaGeoOne, DbConnection dbConnRes, string errResTableName, string xmlPath)
        {
            IArcgisDataCheckParaSet dataCheckParaSet = new ArcgisDataCheckParaSet();
            //检查数据来接
            dataCheckParaSet.Workspace = workspace;

            //参数数据连接
            dataCheckParaSet.DbConnPara = dbConnPara;
            dataCheckParaSet.DbConnParaGeoOne = dbConnParaGeoOne;

            //检查结果日志输出连接
            dataCheckParaSet.DbConnRes = dbConnRes;

            //检查结果日志表名
            dataCheckParaSet.ErrResTableName = errResTableName;

            //检查功能组合结构XML
            dataCheckParaSet.XmlDocDataCheckSet = new XmlDocument();
            try
            {
                dataCheckParaSet.XmlDocDataCheckSet.Load(xmlPath);
            }
            catch
            {

            }

            ArcgisDataCheck dataCheck = new ArcgisDataCheck(dataCheckParaSet);
            Hook = dataCheck as IDataCheckHook;
        }

        //加载执行检查功能
        public static void LoadDataCheckLogicFunction(string logicFunName, IDataCheckHook hook, IDataCheckLogic dataCheckLogic)
        {
            IArcgisDataCheckHook dataCheckHook = hook as IArcgisDataCheckHook;
            if (dataCheckHook == null) return;
            IArcgisDataCheckParaSet checkParaSet = dataCheckHook.DataCheckParaSet as IArcgisDataCheckParaSet;
            if (checkParaSet == null) return;
            if (checkParaSet.XmlDocDataCheckSet == null) return;
            XmlNode aNode = checkParaSet.XmlDocDataCheckSet.DocumentElement.SelectSingleNode(".//LogicCheckFunction[@Name='" + logicFunName + "']");
            if (aNode == null) return;
            if (aNode.ChildNodes.Count == 0) return;
            if (DicCheck == null) return;
            //设置进度条
            //int pMax = aNode.ChildNodes.Count;
            //_CheckForm.Invoke(new IntiProgressBarOut(intiaProgressout), new object[] { pMax });

            m_DicTopoDataCheck = new Dictionary<string, string>();
            int pValue = 0;
            ProgressChangeEvent eInfo = new ProgressChangeEvent();
            eInfo.Max = aNode.ChildNodes.Count;

            foreach (XmlNode childNode in aNode.ChildNodes)
            {
                XmlElement childElement = childNode as XmlElement;
                if (!childElement.HasAttribute("Name")) continue;
                string name = childElement.GetAttribute("Name");
                string pText = childElement.GetAttribute("text").Trim();
                string pType = childElement.GetAttribute("type").Trim();
                if (pType == "拓扑检查")
                {
                    if(!m_DicTopoDataCheck.ContainsKey(name))
                    {
                        m_DicTopoDataCheck.Add(name, pText);
                    }
                    continue;
                }

                //if (DicCheck.ContainsKey(name))
                //{
                //    _CheckForm.Invoke(new ShowErrorInfo(showEInfo), new object[] { pText });

                //    DicCheck[name].OnCreate(hook);
                //    (DicCheck[name] as ICheckEvent).DataErrTreat += new DataErrTreatHandle(dataCheckLogic.DataCheckLogic_DataErrTreat);
                //    (DicCheck[name] as ICheckEvent).ProgressShow += new ProgressChangeHandle(GeoDataChecker_ProgressShow);
                //    DicCheck[name].OnDataCheck();
                //    //进度条加1
                //    pValue++;
                //    eInfo.Value = pValue;
                //    //进度条控制
                //    GeoDataChecker_ProgressShow((object)_PrgressBarOut, eInfo);
                //}
            }

            if(m_DicTopoDataCheck!=null&&m_DicTopoDataCheck.Count!=0)
            {
                //进行拓扑检查
                if (DicCheck.ContainsKey("GeoDataChecker.GeoTopologyCheck"))
                {
                    _CheckForm.Invoke(new ShowErrorInfo(showEInfo), new object[] { "进行拓扑检查" });

                    DicCheck["GeoDataChecker.GeoTopologyCheck"].OnCreate(hook);
                    (DicCheck["GeoDataChecker.GeoTopologyCheck"] as ICheckEvent).DataErrTreat += new DataErrTreatHandle(dataCheckLogic.DataCheckLogic_DataErrTreat);
                    (DicCheck["GeoDataChecker.GeoTopologyCheck"] as ICheckEvent).ProgressShow += new ProgressChangeHandle(GeoDataChecker_ProgressShow);
                    DicCheck["GeoDataChecker.GeoTopologyCheck"].OnDataCheck();

                    pValue++;
                    eInfo.Value = pValue;
                    //进度条控制
                    GeoDataChecker_ProgressShow((object)_PrgressBarOut, eInfo);
                }
            }
            foreach (XmlNode childNode in aNode.ChildNodes)
            {
                XmlElement childElement = childNode as XmlElement;
                if (!childElement.HasAttribute("Name")) continue;
                string name = childElement.GetAttribute("Name");
                string pText = childElement.GetAttribute("text").Trim();
                string pType = childElement.GetAttribute("type").Trim();
                if (pType == "拓扑检查")
                {
                    continue;
                }
                if (DicCheck.ContainsKey(name))
                {
                    _CheckForm.Invoke(new ShowErrorInfo(showEInfo), new object[] { pText });

                    DicCheck[name].OnCreate(hook);
                    (DicCheck[name] as ICheckEvent).DataErrTreat += new DataErrTreatHandle(dataCheckLogic.DataCheckLogic_DataErrTreat);
                    (DicCheck[name] as ICheckEvent).ProgressShow += new ProgressChangeHandle(GeoDataChecker_ProgressShow);
                    DicCheck[name].OnDataCheck();
                    //进度条加1
                    pValue++;
                    eInfo.Value = pValue;
                    //进度条控制
                    GeoDataChecker_ProgressShow((object)_PrgressBarOut, eInfo);
                }
            }
            
            
            /*
            int pValue = 0;
            ProgressChangeEvent eInfo = new ProgressChangeEvent();
            eInfo.Max=aNode.ChildNodes.Count;

            foreach (XmlNode childNode in aNode.ChildNodes)
            {
                XmlElement childElement = childNode as XmlElement;
                if (!childElement.HasAttribute("Name")) continue;
                string name = childElement.GetAttribute("Name");
                string pText = childElement.GetAttribute("text").Trim();

                //如果不是库体结构检查，就用多线程
                //Thread pThread = new Thread(new ThreadStart(delegate { DataCheckRelizeFunction(name, pValue, eInfo, hook, dataCheckLogic); }));
                //pThread.Start();
                if (DicCheck.ContainsKey(name))
                {
                    _CheckForm.Invoke(new ShowErrorInfo(showEInfo), new object[] { pText });

                    DicCheck[name].OnCreate(hook);
                    (DicCheck[name] as ICheckEvent).DataErrTreat += new DataErrTreatHandle(dataCheckLogic.DataCheckLogic_DataErrTreat);
                    (DicCheck[name] as ICheckEvent).ProgressShow += new ProgressChangeHandle(GeoDataChecker_ProgressShow);
                    DicCheck[name].OnDataCheck();
                    //进度条加1
                    pValue++;
                    eInfo.Value = pValue;
                    //_CheckForm.Invoke(new ChangeProgressBarOut(changeProgressOut), new object[] { pValue });
                    //进度条控制
                    GeoDataChecker_ProgressShow((object)_PrgressBarOut, eInfo);
                }
            }
             * */
        }


        private static void DataCheckRelizeFunction(string name, int pValue, ProgressChangeEvent eInfo, IDataCheckHook hook, IDataCheckLogic dataCheckLogic)
        {
            object objLock = new object();
            lock (objLock)
            {
                if (DicCheck.ContainsKey(name))
                {
                    DicCheck[name].OnCreate(hook);
                    (DicCheck[name] as ICheckEvent).DataErrTreat += new DataErrTreatHandle(dataCheckLogic.DataCheckLogic_DataErrTreat);
                    (DicCheck[name] as ICheckEvent).ProgressShow += new ProgressChangeHandle(GeoDataChecker_ProgressShow);
                    DicCheck[name].OnDataCheck();
                    //进度条加1
                    pValue++;
                    eInfo.Value = pValue;
                    //_CheckForm.Invoke(new ChangeProgressBarOut(changeProgressOut), new object[] { pValue });
                    //进度条控制
                    GeoDataChecker_ProgressShow((object)_PrgressBarOut, eInfo);
                }
            }
        }
        //处理检查结果
        public static void GeoDataChecker_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            IArcgisDataCheckParaSet dataCheckParaSet = sender as IArcgisDataCheckParaSet;
            if (dataCheckParaSet == null) return;

            if (dataCheckParaSet.DbConnRes != null)
            {
                eErrorCount += 1;
                _CheckForm.Invoke(new ShowErrorCount(showECount), new object[] { eErrorCount });
                InsertRowToAccess(dataCheckParaSet.DbConnRes, dataCheckParaSet.ErrResTableName, e);
            }
        }
        //将数据检查结果存入ACCESS中 
        private static void InsertRowToAccess(DbConnection dbCon, string tableName, DataErrTreatEvent e)
        {
            SysCommon.DataBase.SysTable sysTable = new SysCommon.DataBase.SysTable();
            sysTable.DbConn = dbCon;
            sysTable.DBConType = SysCommon.enumDBConType.OLEDB;
            sysTable.DBType = SysCommon.enumDBType.ACCESS;
            string sql = "insert into " + tableName +
                "(数据文件路径,检查功能名,错误类型,错误描述,数据图层1,数据OID1,数据图层2,数据OID2,定位点X,定位点Y,检查时间) values(" +
                "'" + e.ErrInfo.DataFileName + "','" + e.ErrInfo.FunctionName + "','" + GeoDataChecker.GetErrorTypeString(Enum.Parse(typeof(enumErrorType), e.ErrInfo.ErrID.ToString()).ToString()) + "','" + e.ErrInfo.ErrDescription + "','" + e.ErrInfo.OriginClassName + "','" + e.ErrInfo.OriginFeatOID.ToString() + "','" +
                e.ErrInfo.DestinationClassName + "','" + e.ErrInfo.DestinationFeatOID.ToString() + "'," + e.ErrInfo.MapX + "," + e.ErrInfo.MapY + ",'" + e.ErrInfo.OperatorTime + "')";

            Exception errEx = null;
            sysTable.UpdateTable(sql, out errEx);
        }


        public static string GetErrorTypeString(string typename)
        {
            string errType;
            switch (typename.ToLower())
            {
                case ("其他错误"):
                    errType = "其他错误";
                    break;
                case ("目标要素类缺失"):
                    errType = "目标要素类缺失";
                    break;
                case ("要素类类型不一致"):
                    errType = "要素类类型不一致";
                    break;
                case ("属性字段缺失"):
                    errType = "属性字段缺失";
                    break;
                case ("属性字段类型不一致"):
                    errType = "属性字段类型不一致";
                    break;
                case ("字段扩展属性不一致"):
                    errType = "字段扩展属性不一致";
                    break;
                case ("分类代码不存在"):
                    errType = "分类代码不存在";
                    break;
                case ("分类代码与图层名对应关系不正确"):
                    errType = "分类代码与图层名对应关系不正确";
                    break;
                case ("线存在伪节点"):
                    errType = "线存在伪节点";
                    break;
                case ("线存在悬挂点"):
                    errType = "线存在悬挂点";
                    break;
                case ("线自重叠检查"):
                    errType = "线自重叠错误";
                    break;
                case ("线自相交检查"):
                    errType = "线自相交错误";
                    break;
                case ("同层面重叠检查"):
                    errType = "同层面重叠错误";
                    break;
                case ("要素重复性检查"):
                    errType = "要素重复性错误";
                    break;
                case ("数学基础正确性检查"):
                    errType = "数学基础正确性检查,空间参考不一致";
                    break;
                case ("空值检查"):
                    errType = "空值检查错误";
                    break;
                case ("线长度逻辑性检查"):
                    errType = "线长度逻辑性错误";
                    break;
                case ("面面积逻辑性检查"):
                    errType = "面面积逻辑性错误";
                    break;
                case ("高程值检查"):
                    errType = "高程值不在给定的高程值范围内";
                    break;
                case ("等高线高程值检查"):
                    errType = "等高线高程值间距异常";
                    break;
                case ("等高线注记一致性检查"):
                    errType = "等高线注记高程值不一致";
                    break;
                case ("控制点注记一致性检查"):
                    errType = "控制点注记高程值不一致";
                    break;
                case ("高程点注记一致性检查"):
                    errType = "高程点注记高程值不一致";
                    break;
                case ("等高线点线矛盾检查"):
                    errType = "等高线点线高程值相互矛盾";
                    break;
                case ("同层线重叠检查"):
                    errType = "同层线重叠错误";
                    break;
                case ("同层线相交检查"):
                    errType = "同层线相交错误";
                    break;
                case ("异层面重叠检查"):
                    errType = "异层面重叠错误";
                    break;
                case ("面缝隙检查"):
                    errType = "面缝隙检查";
                    break;
                case ("面含面检查"):
                    errType = "面含面错误";
                    break;
                case ("点搭面检查"):
                    errType = "点搭面错误";
                    break;
                case ("点搭线检查"):
                    errType = "点搭线错误";
                    break;
                case ("点位于线端点检查"):
                    errType = "点位于线端点错误";
                    break;
                case ("点位于面内检查"):
                    errType = "点位于面内检查错误";
                    break;
                case ("线面边界重合检查"):
                    errType = "线面边界重合检查错误";
                    break;
                case ("线穿面检查"):
                    errType = "线穿面检查错误";
                    break;
                case ("接边检查"):
                    errType = "接边检查错误";
                    break;
                case ("同层点重复检查"):
                    errType = "同层点重复检查错误";
                    break;
                case ("异层线重叠检查"):
                    errType = "异层线重叠检查错误";
                    break;
                case ("线端点被点覆盖检查"):
                    errType = "线端点被点覆盖检查错误";
                    break;
                case ("线线重合检查"):
                    errType = "线线重合检查错误";
                    break;
                case ("简单线检查"):
                    errType = "简单线检查错误";
                    break;
                case ("面含点检查"):
                    errType = "面含点检查错误";
                    break;
                case ("面边界线重合检查"):
                    errType = "面边界线重合检查错误";
                    break;
                case ("面面相互覆盖检查"):
                    errType = "面面相互覆盖检查错误";
                    break;
                case ("面边界面边界重合检查"):
                    errType = "面边界面边界重合检查错误";
                    break;
                case ("点不在面内检查"):
                    errType = "点不在面内检查错误";
                    break;
                case ("线打折检查"):
                    errType = "线打折检查错误";
                    break;
                case (" 线不自相交且不重叠检查"):
                    errType = " 线不自相交且不重叠检查错误";
                    break;
                default:
                    return string.Empty;
            }

            return errType;
        }

        private delegate void  ShowErrorCount(long eCount);
        private static void showECount(long ErrorCount)
        {
            _Label.Text = "检查错误条数：" + ErrorCount + "条";
        }
        private delegate void ShowErrorInfo(string pText);
        private static void showEInfo(string pText)
        {
            _Label.Text = "正在进行：" +pText + ".......";
        }

        #region 进度条处理函数
        //进度条事件处理函数
        public delegate void GEODataCheckerProgressShow(object sender, ProgressChangeEvent e);
        public static void GeoDataChecker_ProgressShow(object sender, ProgressChangeEvent e)
        {
            DevComponents.DotNetBar.Controls.ProgressBarX pProgress = sender as DevComponents.DotNetBar.Controls.ProgressBarX;
            //pProgress.Minimum = 0;
            //pProgress.Maximum = e.Max;
            //pProgress.Value = e.Value;
            if (_CheckForm != null)
            {
                _CheckForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { pProgress, 0, e.Max, e.Value });
            }
        }

        //修改进度条
        private delegate void ChangeProgress(DevComponents.DotNetBar.Controls.ProgressBarX pProgressBar, int min, int max, int value);
        private static void ChangeProgressBar(DevComponents.DotNetBar.Controls.ProgressBarX pProgressBar, int min, int max, int value)
        {
            if (min != -1)
            {
                pProgressBar.Minimum = min;
            }
            if (max != -1)
            {
                pProgressBar.Maximum = max;
            }
            pProgressBar.Value = value;
            pProgressBar.Refresh();
        }

        //初始化进度条
        //public delegate void IntiProgressBar(int pMax);
        //public static void intiaProgress(int max)
        //{
        //    _ProgressBarInner.Minimum = 0;
        //    _ProgressBarInner.Maximum = max;
        //    _ProgressBarInner.Value = 0;
        //}
        //public delegate void IntiProgressBarOut(int pMax);
        //public static void intiaProgressout(int max)
        //{
        //    _PrgressBarOut.Minimum = 0;
        //    _PrgressBarOut.Maximum = max;
        //    _PrgressBarOut.Value = 0;
        //}
        ////改变进度条的值
        //public delegate void ChangeProgressBar(int pValue);
        //public static void changeProgress(int value)
        //{
        //    _ProgressBarInner.Value = value;
        //}

        //public delegate void ChangeProgressBarOut(int pValue);
        //public static void changeProgressOut(int value)
        //{
        //    _PrgressBarOut.Value = value;
        //}
        #endregion
    }


    public enum enumErrorType
    {
        其他错误 = 0,
        目标要素类缺失 = 1,
        要素类类型不一致 = 2,
        属性字段缺失 = 3,
        属性字段类型不一致 = 4,
        字段扩展属性不一致 = 5,
        分类代码不存在 = 6,
        分类代码与图层名对应关系不正确 = 7,
        数学基础正确性检查=8,
        空值检查=9,
        线长度逻辑性检查=10,
        面面积逻辑性检查=11,
        高程值检查=12,
        等高线高程值检查=13,
        等高线注记一致性检查=14,
        控制点注记一致性检查=15,
        高程点注记一致性检查=16,
        等高线点线矛盾检查=17,
        要素重复性检查 = 18,
        同层点重复检查 = 19,
        点搭面检查 = 20,
        点搭线检查 = 21,
        点位于线端点检查 = 22,
        点位于面内检查 = 23,
        点不在面内检查=24,             //没有
        线存在伪节点 = 25,
        线存在悬挂点 = 26,
        线自重叠检查 = 27,
        线自相交检查 = 28,
        同层线重叠检查 = 29,
        同层线相交检查 = 30,
        异层线重叠检查 = 31,
        线面边界重合检查 = 32,
        线穿面检查 = 33,
        线端点被点覆盖检查 = 34,
        线线重合检查 = 35,
        简单线检查=36,
        线不自相交且不重叠检查=37,   //没有
        线打折检查=38,               //没有
        面含点检查 = 39,
        面含面检查 = 40,
        同层面重叠检查=41,
        异层面重叠检查=42,
        面缝隙检查= 43,
        面边界线重合检查=44,
        面边界面边界重合检查=45,
        面面相互覆盖检查=46,
        接边检查=47,
    }
}
