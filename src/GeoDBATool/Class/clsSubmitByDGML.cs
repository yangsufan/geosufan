using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Data;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    /// <summary>
    /// 根据DGML文档将数据提交到原始库  陈亚飞添加
    /// </summary>
    public class clsSubmitByDGML
    {
        string[] v_DGMLFiles = null;//DGML文档路径和名称
        //FID库连接信息
        SysCommon.enumDBConType v_dbConType;
        SysCommon.enumDBType v_dbType;
        string v_FIDConStr = "";
        //目标库连接信息
        string v_objDBType = "";
        string v_Sever = "";
        string v_Instance = "";
        string v_DataBase = "";
        string v_User = "";
        string v_Password = "";
        string v_Version = "";
        //历史库连接
        string v_histoDBType = "";
        string v_histoSever = "";
        string v_histoInstance = "";
        string v_histoDataBase = "";
        string v_histoUser = "";
        string v_histoPassword = "";
        string v_histoVersion = "";
        //SysCommon.Gis.SysGisDataSet v_Gisdt;//目标库体连接
        //SysCommon.DataBase.SysTable v_Table;//FID记录表库体连接

        Plugin.Application.IAppGISRef v_AppGIS;//主功能应用APP
        Plugin.Application.IAppFormRef v_AppForm;//主窗体APP
        //当前处理数据的线程
        private Thread _CurrentThread;
        public Thread CurrentThread
        {
            get
            {
                return _CurrentThread;
            }
            set
            {
                _CurrentThread = value;
            }
        }

        private bool m_Res;
        public bool Res
        {
            get
            {
                return m_Res;
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileNames">DGML路径文件名集合</param>
        /// /// <param name="pAppGIS"></param>
        public clsSubmitByDGML(string[] fileNames, string pFIDConstr, SysCommon.enumDBConType pConType, SysCommon.enumDBType pDbType, string pObjType, string pServer, string pInstance, string pDatabse, string pUser, string pPassword, string pVersion, string pHistoType, string pHistoServer, string pHistoInstance, string pHistoDB, string pHistoUser, string pHistoPassword, string pHistoVersion, Plugin.Application.IAppGISRef pAppGIS)
        {
            //v_Gisdt = pGisDataset;
            //v_Table = pSysTable;
            v_FIDConStr = pFIDConstr;
            v_dbConType = pConType;
            v_dbType = pDbType;

            v_objDBType = pObjType;
            v_Sever = pServer;
            v_Instance = pInstance;
            v_DataBase = pDatabse;
            v_User = pUser;
            v_Password = pPassword;
            v_Version = pVersion;

            v_histoDBType = pHistoType;
            v_histoSever = pHistoServer;
            v_histoInstance = pHistoInstance;
            v_histoDataBase = pHistoDB;
            v_histoUser = pHistoUser;
            v_histoPassword = pHistoPassword;
            v_histoVersion = pHistoVersion;

            v_DGMLFiles = fileNames;
            v_AppGIS = pAppGIS;
            v_AppForm = pAppGIS as Plugin.Application.IAppFormRef;
        }

        public void SubmitThread()
        {
            bool isConnect = true;//标识历史库是否能够连接上
            DateTime pDate = DateTime.Now;
            Exception eError = null;
            m_Res = true;
            //创建树图
            if (v_AppGIS == null)
            {
                m_Res = false;
                return;
            }
            if (v_AppGIS.DataTree == null)
            {
                m_Res = false;
                return;
            }
            //创建处理树图
            v_AppForm.MainForm.Invoke(new InitTree(IntialTree), new object[] { v_AppGIS.DataTree });
            //设置树节点颜色
            v_AppForm.MainForm.Invoke(new SetSelectNodeColor(setNodeColor), new object[] { v_AppGIS.DataTree });
            v_AppGIS.DataTree.Tag = false;
            //显示进度条
            v_AppForm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { true });

            #region 设置库体连接
            //设置目标库连接
            SysCommon.Gis.SysGisDataSet v_Gisdt = new SysCommon.Gis.SysGisDataSet();
            if (v_objDBType == "SDE")
            {
                v_Gisdt.SetWorkspace(v_Sever, v_Instance, v_DataBase, v_User, v_Password, v_Version, out eError);
            }
            else if (v_objDBType == "PDB")
            {
                v_Gisdt.SetWorkspace(v_DataBase, SysCommon.enumWSType.PDB, out eError);
            }
            else if (v_objDBType == "GDB")
            {
                v_Gisdt.SetWorkspace(v_DataBase, SysCommon.enumWSType.GDB, out eError);
            }
            if (eError != null)
            {
                v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "目标数据库连接出错" });
                m_Res = false;
                //终止进程
                v_AppGIS.CurrentThread = null;
                if (_CurrentThread.ThreadState != ThreadState.Stopped)
                {
                    _CurrentThread.Abort();
                }
                return;
            }

            //设置历史库连接
            SysCommon.Gis.SysGisDataSet v_HistoGisdt = new SysCommon.Gis.SysGisDataSet();
            if (v_histoDBType == "SDE")
            {
                v_HistoGisdt.SetWorkspace(v_histoSever, v_histoInstance, v_histoDataBase, v_histoUser, v_histoPassword, v_histoVersion, out eError);
            }
            else if (v_histoDBType == "PDB")
            {
                v_HistoGisdt.SetWorkspace(v_histoDataBase, SysCommon.enumWSType.PDB, out eError);
            }
            else if (v_histoDBType == "GDB")
            {
                v_HistoGisdt.SetWorkspace(v_histoDataBase, SysCommon.enumWSType.GDB, out eError);
            }
            if (eError != null)
            {
                //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "历史库数据库连接出错" });
                isConnect = false;
                //m_Res = false;
                ////终止进程
                //v_AppGIS.CurrentThread = null;
                //_CurrentThread.Abort();
                //return;
            }
            //设置FID记录表连接
            SysCommon.DataBase.SysTable v_Table = new SysCommon.DataBase.SysTable();
            v_Table.SetDbConnection(v_FIDConStr, v_dbConType, v_dbType, out eError);
            if (eError != null)
            {
                v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "连接FID记录表数据库出错" });
                m_Res = false;
                // 终止进程
                v_AppGIS.CurrentThread = null;
                if (_CurrentThread.ThreadState != ThreadState.Stopped)
                {
                    _CurrentThread.Abort();
                }
                return;
            }
            #endregion
            try
            {
                //遍历多个文件
                foreach (string fileName in v_DGMLFiles)
                {
                    //创建树图父节点(以文件名作为根结点)
                    DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                    string tempFile = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                    pNode = (DevComponents.AdvTree.Node)v_AppForm.MainForm.Invoke(new CreateTreeNode(CreateAdvTreeNode), new object[] { v_AppGIS.DataTree.Nodes, tempFile, tempFile, v_AppGIS.DataTree.ImageList.Images[5], true });//文件名节点
                    //显示状态栏
                    v_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { "文件" + tempFile + "中的数据开始入库" });

                    //记录删除要素的 GOFID
                    StringBuilder FIDsb = new StringBuilder();

                    FIDsb = GetFeaClsByDGML(fileName, "删除", out eError);
                    if (FIDsb == null)
                    {
                        v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", eError.Message });
                        m_Res = false;
                        //// 终止进程
                        //v_AppGIS.CurrentThread = null;
                        //if (_CurrentThread.ThreadState != ThreadState.Stopped)
                        //{
                        //    _CurrentThread.Suspend();
                        //    _CurrentThread.Abort();
                        //}
                        break;
                    }

                    //开启事物，开启编辑
                    v_Table.StartTransaction();
                    v_Gisdt.StartWorkspaceEdit(false, out eError);
                    if (eError != null)
                    {
                        v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "开启编辑失败！" });
                        m_Res = false;
                        //关闭连接 终止进程
                        v_Table.CloseDbConnection();
                        //隐藏进度条
                        v_AppForm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { false });
                        //将状态栏信息置为空
                        v_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { "" });
                        //v_AppGIS.CurrentThread = null;
                        //if (_CurrentThread.ThreadState != ThreadState.Stopped)
                        //{
                        //    _CurrentThread.Suspend();
                        //    _CurrentThread.Abort();
                        //}
                        break;
                    }
                    //如果有历史库，需要产生历史，首先连接历史库
                    if (isConnect == true)
                    {
                        v_HistoGisdt.StartWorkspaceEdit(false, out eError);
                        if (eError != null)
                        {
                            v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "开启编辑失败！" });
                            m_Res = false;
                            //关闭连接 终止进程
                            v_Table.CloseDbConnection();
                            //隐藏进度条
                            v_AppForm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { false });
                            //将状态栏信息置为空
                            v_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { "" });
                            //v_AppGIS.CurrentThread = null;
                            //if (_CurrentThread.ThreadState != ThreadState.Stopped)
                            //{
                            //    _CurrentThread.Suspend();
                            //    _CurrentThread.Abort();
                            //}
                            break;
                        }
                    }
                    #region 将历史库"修改和删除"的数据置为历史
                    if (isConnect == true)
                    {
                        if (!UpdateHistoData(fileName, v_histoDBType, v_HistoGisdt, v_Table, pDate, out eError))
                        {
                            m_Res = false;
                            //回滚事物,关闭连接
                            v_Table.EndTransaction(false);
                            v_Table.CloseDbConnection();
                            v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", eError.Message });
                            //结束编辑
                            v_Gisdt.EndWorkspaceEdit(false, out eError);
                            if (isConnect)
                            {
                                v_HistoGisdt.EndWorkspaceEdit(false, out eError);
                            }
                            if (eError != null)
                            {
                                v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "结束编辑出错！" });
                            }
                            //隐藏进度条
                            v_AppForm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { false });
                            //将状态栏信息置为空
                            v_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { "" });

                            //v_AppGIS.CurrentThread = null;
                            //if (_CurrentThread.ThreadState != ThreadState.Stopped)
                            //{
                            //    _CurrentThread.Suspend();
                            //    _CurrentThread.Abort();
                            //}
                            break;
                        }
                    }
                    #endregion

                    # region 删除现势库里面 删除 的数据，同时删除FID对应的记录
                    if (!UpdateData(FIDsb, v_Gisdt, v_Table, out eError))
                    {
                        m_Res = false;
                        //回滚事物,关闭连接
                        v_Table.EndTransaction(false);
                        v_Table.CloseDbConnection();
                        v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", eError.Message });
                        //结束编辑
                        v_Gisdt.EndWorkspaceEdit(false, out eError);
                        if (isConnect)
                        {
                            v_HistoGisdt.EndWorkspaceEdit(false, out eError);
                        }
                        if (eError != null)
                        {
                            v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "结束编辑出错！" });
                        }
                        //隐藏进度条
                        v_AppForm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { false });
                        //将状态栏信息置为空
                        v_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { "" });
                        //v_AppGIS.CurrentThread = null;
                        //if (_CurrentThread.ThreadState != ThreadState.Stopped)
                        //{
                        //    _CurrentThread.Suspend();
                        //    _CurrentThread.Abort();
                        //}
                        break;
                    }
                    #endregion

                    #region 将 新建和修改 的数据导入目标数据库和历史库中，并修改FID记录表，
                    if (!ImportData(fileName, pNode, v_Gisdt, v_HistoGisdt, v_Table, pDate, isConnect, out eError))
                    {
                        m_Res = false;
                        //回滚事物,关闭连接
                        v_Table.EndTransaction(false);
                        v_Table.CloseDbConnection();
                        v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", eError.Message });
                        //结束编辑
                        v_Gisdt.EndWorkspaceEdit(false, out eError);
                        if (isConnect)
                        {
                            v_HistoGisdt.EndWorkspaceEdit(false, out eError);
                        }
                        if (eError != null)
                        {
                            v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "结束编辑出错！" });
                        }
                        //隐藏进度条
                        v_AppForm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { false });
                        //将状态栏信息置为空
                        v_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { "" });
                        //// 终止进程
                        //v_AppGIS.CurrentThread = null;
                        //if (_CurrentThread.ThreadState != ThreadState.Stopped)
                        //{
                        //    _CurrentThread.Suspend();
                        //    _CurrentThread.Abort();
                        //}
                        break;
                    }
                    #endregion

                    //结束事物，结束编辑
                    v_Table.EndTransaction(true);
                    v_Gisdt.EndWorkspaceEdit(true, out eError);
                    if (isConnect)
                    {
                        v_HistoGisdt.EndWorkspaceEdit(true, out eError);
                    }
                    if (eError != null)
                    {
                        m_Res = false;
                        v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "结束编辑失败！" });

                        //关闭连接 终止进程
                        v_Table.CloseDbConnection();
                        //隐藏进度条
                        v_AppForm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { false });
                        //将状态栏信息置为空
                        v_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { "" });
                        //v_AppGIS.CurrentThread = null;
                        //if (_CurrentThread.ThreadState != ThreadState.Stopped)
                        //{
                        //    _CurrentThread.Suspend();
                        //    _CurrentThread.Abort();
                        //}
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                //********************************************************************
                m_Res = false;
                v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", ex.Message });
                ////关闭连接 终止进程
                //v_Table.CloseDbConnection();
                ////隐藏进度条
                //v_AppForm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { false });
                ////将状态栏信息置为空
                //v_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { "" });
                ////停止线程
                //v_AppGIS.CurrentThread = null;
                //if (_CurrentThread.ThreadState != ThreadState.Stopped)
                //{
                //    _CurrentThread.Suspend();
                //    _CurrentThread.Abort();
                //}
            }

            //关闭连接
            v_Table.CloseDbConnection();
            //隐藏进度条
            v_AppForm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { false });
            //将状态栏信息置为空
            v_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { "" });

            if (m_Res == true)
            {
                v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "提示", "操作成功!" });
            }
            //停止线程
            v_AppGIS.CurrentThread = null;
            if (_CurrentThread.ThreadState != ThreadState.Stopped)
            {
                _CurrentThread.Abort();
            }
        }

        #region 函数

        /// <summary>
        /// 读DGML，根据状态获取要素信息
        /// </summary>
        /// <param name="pfileName">DGML文档路径</param>
        /// <param name="state">状态（新增、修改和删除）</param>
        /// <returns></returns>
        private StringBuilder GetFeaClsByDGML(string pfileName, string state, out Exception eError)
        {
            try
            {
                eError = null;
                StringBuilder FIDsb = new StringBuilder();
                if (pfileName == "")
                {
                    eError = new Exception("文件名为空!");
                    return null;
                }

                ////该变量用来记录原始库图层名称和OID
                //Dictionary<string, StringBuilder> FeaClsDic = new Dictionary<string, StringBuilder>();

                //加载DGML文档
                XmlDocument DGMLDoc = new XmlDocument();
                DGMLDoc.Load(pfileName);

                XmlNodeList RecordList = DGMLDoc.SelectNodes(".//DGML//Data//Record");
                if (RecordList == null)
                {
                    eError = new Exception("xml文档结构不正确，请检查该文档是否为更新导出的文档!");
                    return null;
                }
                foreach (XmlNode recordNode in RecordList)
                {
                    XmlNode sNode = recordNode.SelectSingleNode(".//STATE");//状态节点
                    if (sNode == null)
                    {
                        eError = new Exception("xml文档结构不正确，请检查该文档是否为更新导出的文档!");
                        return null;
                    }
                    XmlNode fNode = recordNode.SelectSingleNode(".//GOFID");//GOFID节点
                    if (fNode == null)
                    {
                        eError = new Exception("xml文档结构不正确，请检查该文档是否为更新导出的文档!");
                        return null;
                    }
                    string pState = sNode.InnerText.Trim();//状态：新建、修改、删除
                    if (pState == state)
                    {
                        if (fNode.InnerText.Trim() == "")
                        {
                            eError = new Exception("从xml文件中读取修改或删除的数据出错");
                            return null;
                        }
                        string pFID = fNode.InnerText.Trim();//GOFID
                        if (FIDsb.Length != 0)
                        {
                            FIDsb.Append(',');
                        }
                        FIDsb.Append(pFID);//记录GOFID
                    }
                }
                return FIDsb;
            }
            catch (Exception eex)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(eex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(eex, null, DateTime.Now);
                }
                //********************************************************************
                eError = eex;
                return null;
            }
        }
        /// <summary>
        /// 记录图层名和对应的OID字符串
        /// </summary>
        /// <param name="v_Table">FID记录表连接</param>
        /// <param name="FIDinfo">GOFID字段信息</param>
        /// <returns></returns>
        private Dictionary<string, StringBuilder> GetDicFeaOIDInfo(SysCommon.DataBase.SysTable v_Table, StringBuilder FIDinfo, out Exception eError)
        {
            eError = null;
            Exception ex = null;
            //记录图层名和OID值对
            Dictionary<string, StringBuilder> FCInfo = new Dictionary<string, StringBuilder>();
            if (FIDinfo.Length == 0) return FCInfo;
            DataTable tempTable = v_Table.GetSQLTable("select * from FID记录表 where GOFID in (" + FIDinfo.ToString() + ")", out ex);
            if (ex != null)
            {
                eError = new Exception("根据条件查找FID记录表出错！");
                //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", eError.Message });
                return null;
            }

            for (int i = 0; i < tempTable.Rows.Count; i++)
            {
                string PName = tempTable.Rows[i]["FCNAME"].ToString().Trim();
                string pOID = tempTable.Rows[i]["OID"].ToString().Trim();
                if (!FCInfo.ContainsKey(PName))
                {
                    StringBuilder sb = new StringBuilder();
                    if (sb.Length != 0)
                    {
                        sb.Append(',');
                    }
                    sb.Append(pOID);
                    FCInfo.Add(PName, sb);
                }
                else
                {
                    if (FCInfo[PName].Length != 0)
                    {
                        FCInfo[PName].Append(',');
                    }
                    FCInfo[PName].Append(pOID);
                }
            }
            return FCInfo;
        }
        /// <summary>
        /// 更新历史库里面的历史数据
        /// </summary>
        /// <param name="pfileName">DGML文档</param>
        /// <param name="v_HistoDBType">历史库连接类型</param>
        /// <param name="v_HistoGIsdt">历史库连接</param>
        /// <param name="v_Table">FID记录表</param>
        /// <param name="invalidDate">失效日期</param>
        /// <returns></returns>
        private bool UpdateHistoData(string pfileName, string v_HistoDBType, SysCommon.Gis.SysGisDataSet v_HistoGIsdt, SysCommon.DataBase.SysTable v_Table, DateTime invalidDate, out Exception eError)
        {
            eError = null;
            Exception ex = null;
            StringBuilder updateFID = new StringBuilder();//修改的FID集合
            StringBuilder deleteFID = new StringBuilder();//修改的FID集合
            Dictionary<string, StringBuilder> FeaClsUpdateInfo = new Dictionary<string, StringBuilder>();//记录"修改"要素图层名和OID
            Dictionary<string, StringBuilder> FeaClsDeleteInfo = new Dictionary<string, StringBuilder>();//记录"删除"要素图层名和OID
            updateFID = GetFeaClsByDGML(pfileName, "修改", out ex);
            if (updateFID == null)
            {
                eError = ex;
                return false;
            }
            deleteFID = GetFeaClsByDGML(pfileName, "删除", out ex);
            if (deleteFID == null)
            {
                eError = ex;
                return false;
            }

            if (updateFID.Length == 0 && deleteFID.Length == 0) return true;


            FeaClsUpdateInfo = GetDicFeaOIDInfo(v_Table, updateFID, out ex);
            if (ex != null)
            {
                eError = ex;
            }
            FeaClsDeleteInfo = GetDicFeaOIDInfo(v_Table, deleteFID, out ex);
            if (ex != null)
            {
                eError = ex;
            }
            string[] strArr = deleteFID.ToString().Split(new char[] { ',' });
            int t = 0;
            foreach (KeyValuePair<string, StringBuilder> feaInfo in FeaClsDeleteInfo)
            {
                string[] aa = feaInfo.Value.ToString().Split(new char[] { ',' });
                t += aa.Length;
            }

            if (t != strArr.Length)
            {
                //若DGML文档中导出的删除的要素记录与FID记录表中的记录不一致，说明该DGML文档可能不是最新的文档
                eError = new Exception("该DGML文档中的数据与现势库数据不一致，请检查！");
                return false;
            }

            #region 对修改的要素进行相应的修改
            foreach (KeyValuePair<string, StringBuilder> updateItem in FeaClsUpdateInfo)
            {
                string pFCName = updateItem.Key + "_GOH";//图层名
                IFeatureClass pFeaCls = v_HistoGIsdt.GetFeatureClass(pFCName, out ex);
                if (ex != null)
                {
                    eError = new Exception("获得图层" + pFCName + "失败！");
                    //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "获得图层" + pFCName + "失败！" });
                    return false;
                }
                int dateIndex = -1;//"ToDate"索引值
                //int stateIndex = -1;//"State"索引值
                dateIndex = pFeaCls.Fields.FindField("ToDate");
                //stateIndex = pFeaCls.Fields.FindField("State");
                if (dateIndex == -1)
                {
                    eError = new Exception("在历史库中找不到字段ToDate！");
                    return false;
                }
                //if (stateIndex == -1) return false;
                //设置过滤条件
                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = "ToDate='" + DateTime.MaxValue.ToString("u") + "' and SourceOID in (" + updateItem.Value + ")";
                IFeatureCursor pFeaCursor = pFeaCls.Update(pFilter, false);
                if (pFeaCursor == null)
                {
                    //eError = new Exception("IFeatureCursor变量为空！");
                    return false;
                }
                IFeature pFeature = pFeaCursor.NextFeature();
                while (pFeature != null)
                {
                    pFeature.set_Value(dateIndex, invalidDate.ToString("u") as object);
                    pFeaCursor.UpdateFeature(pFeature);
                    pFeaCursor.Flush();
                    pFeature = pFeaCursor.NextFeature();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
                //if (v_HistoDBType == "SDE")
                //{
                //    pFilter.WhereClause = "ToDate=TO_DATE('" + DateTime.MaxValue+ "','YYYY-MM-DD HH24:MI:SS') and SourceOID in (" + updateItem.Value + ")";
                //}
                //else if (v_HistoDBType == "GDB")
                //{
                //    pFilter.WhereClause = "ToDate=date '"+DateTime.MaxValue+"' and SourceOID in (" + updateItem.Value + ")";
                //}
                //else if (v_HistoDBType == "PDB")
                //{
                //    pFilter.WhereClause = "ToDate=#"+DateTime.MaxValue+"# and SourceOID in (" + updateItem.Value + ")";
                //}
                //pFilter.SubFields = "ToDate,State";
                //ITable pTable = pFeaCls as ITable;
                //IRowBuffer pRowBuf = pTable.CreateRowBuffer();
                //pRowBuf.set_Value(dateIndex, invalidDate.ToString("u") as object);
                //pRowBuf.set_Value(stateIndex, 2);
                //修改要素
                //pTable.UpdateSearchedRows(pFilter, pRowBuf);
                //还需要测试注记，若不对则采用另外的方法来
                //......
            }
            #endregion

            # region 对删除的要素进行相应的修改
            foreach (KeyValuePair<string, StringBuilder> deleteItem in FeaClsDeleteInfo)
            {
                string pFCName = deleteItem.Key + "_GOH";//图层名
                IFeatureClass pFeaCls = v_HistoGIsdt.GetFeatureClass(pFCName, out ex);
                if (ex != null)
                {
                    eError = new Exception("获得图层" + pFCName + "失败！");
                    //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "获得图层" + pFCName + "失败！" });
                    return false;
                }
                int dateIndex = -1;//"ToDate"索引值
                //int stateIndex = -1;//"State"索引值
                dateIndex = pFeaCls.Fields.FindField("ToDate");
                //stateIndex = pFeaCls.Fields.FindField("State");
                if (dateIndex == -1)
                {
                    eError = new Exception("在历史库中找不到字段ToDate！");
                    return false;
                }
                //if (stateIndex == -1) return false;
                //设置过滤条件
                IQueryFilter pDelFilter = new QueryFilterClass();
                pDelFilter.WhereClause = "ToDate='" + DateTime.MaxValue.ToString("u") + "' and SourceOID in (" + deleteItem.Value + ")";
                IFeatureCursor pDelFeaCursor = pFeaCls.Update(pDelFilter, false);
                if (pDelFeaCursor == null) return false;
                IFeature pDelFeature = pDelFeaCursor.NextFeature();
                while (pDelFeature != null)
                {
                    pDelFeature.set_Value(dateIndex, invalidDate.ToString("u") as object);
                    pDelFeaCursor.UpdateFeature(pDelFeature);
                    pDelFeaCursor.Flush();
                    pDelFeature = pDelFeaCursor.NextFeature();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pDelFeaCursor);
                //if (v_HistoDBType == "SDE")
                //{
                //    pFilter.WhereClause = "ToDate=TO_DATE('"+DateTime.MaxValue+"','YYYY-MM-DD HH24:MI:SS') and SourceOID in (" + deleteItem.Value + ")";
                //}
                //else if (v_HistoDBType == "GDB")
                //{
                //    pFilter.WhereClause = "ToDate=date '"+DateTime.MaxValue+"' and SourceOID in (" + deleteItem.Value + ")";
                //}
                //else if (v_HistoDBType == "PDB")
                //{
                //    pFilter.WhereClause = "ToDate=#"+DateTime.MaxValue+"# and SourceOID in (" + deleteItem.Value + ")";
                //}
                //pFilter.SubFields = "ToDate,State";
                //ITable pTable = pFeaCls as ITable;
                //IRowBuffer pRowBuf = pTable.CreateRowBuffer();
                //pRowBuf.set_Value(dateIndex, invalidDate);
                //pRowBuf.set_Value(stateIndex, 3);
                //修改要素
                //pTable.UpdateSearchedRows(pFilter, pRowBuf);
                //还需要测试注记，若不对则采用另外的方法来
                //......
            }
            # endregion
            return true;
        }
        /// <summary>
        /// 删除原始库里面更新变化(修改和删除)的数据，删除FID记录表里面对应的记录
        /// </summary>
        /// <param name="FCInfo">更新变化数据的图层名和OID（修改和删除的数据）</param>
        /// <param name="FIDinfo">FID记录表里面更新变化的数据对应的记录(修改和删除的数据)</param>
        /// <param name="pGisDataset">原始库连接</param>
        /// <param name="pSysTable">FID记录表连接</param>
        /// <returns></returns>
        private bool UpdateData(StringBuilder FIDinfo, SysCommon.Gis.SysGisDataSet v_Gisdt, SysCommon.DataBase.SysTable v_Table, out Exception eError)
        {
            eError = null;
            Exception ex = null;
            if (FIDinfo == null) return false;
            if (FIDinfo.Length == 0) return true;
            Dictionary<string, StringBuilder> FCInfo = new Dictionary<string, StringBuilder>();
            FCInfo = GetDicFeaOIDInfo(v_Table, FIDinfo, out ex);
            if (FCInfo == null)
            {
                eError = ex;
                return false;
            }
            if (FCInfo.Count == 0) return true;


            string[] strArr = FIDinfo.ToString().Split(new char[] { ',' });
            int t = 0;
            foreach (KeyValuePair<string, StringBuilder> feaInfo in FCInfo)
            {
                string[] aa = feaInfo.Value.ToString().Split(new char[] { ',' });
                t += aa.Length;
            }

            if (t != strArr.Length)
            {
                //若DGML文档中导出的删除的要素记录与FID记录表中的记录不一致，说明该DGML文档可能不是最新的文档
                eError = new Exception("该DGML文档中的数据与现势库数据不一致，请检查！");
                return false;
            }

            //删除数据
            foreach (KeyValuePair<string, StringBuilder> FcItem in FCInfo)
            {
                string conndiStr = "OBJECTID in (" + FcItem.Value.ToString() + ")";
                v_Gisdt.DeleteRows(FcItem.Key, conndiStr, out ex);
                if (ex != null)
                {
                    eError = new Exception("现势库中‘删除’的数据更新失败！");
                    //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "删除数据失败！" });
                    return false;
                }
            }
            //删除日志记录表里面修改和删除的记录
            v_Table.UpdateTable("delete from FID记录表 where GOFID in (" + FIDinfo.ToString() + ")", out ex);
            if (ex != null)
            {
                eError = new Exception("删除FID记录表记录失败！");
                //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "删除FID记录表记录失败！" });
                return false;
            }
            return true;
        }
        /// <summary>
        /// 查找现势库中修改要素对应的OBJECTID
        /// </summary>
        /// <param name="pFID">修改要素对应的GOFID</param>
        /// <param name="v_Table">FID记录表连接</param>
        /// <returns></returns>
        private int GetUserDBOID(int pFID, SysCommon.DataBase.SysTable v_Table)
        {
            Exception eError = null;
            int pOid = -1;
            DataTable dt = v_Table.GetSQLTable("select * from FID记录表 where GOFID=" + pFID, out eError);
            if (eError != null)
            {
                return pOid;
            }
            if (dt.Rows.Count != 1) return pOid;
            pOid = int.Parse(dt.Rows[0]["OID"].ToString().Trim());
            return pOid;
        }

        /// <summary>
        /// 将 新建和修改的要素 导入到目标库和历史库中
        /// </summary>
        /// <param name="pFileName">DGML文档</param>
        /// <param name="pNode">处理树图根结点（文件名）</param>
        /// <param name="v_Gisdt">目标库连接</param>
        /// <param name="v_histoGisdt">历史库连接</param>
        /// <param name="v_Table">FID记录表连接</param>
        /// <returns></returns>
        private bool ImportData(string pFileName, DevComponents.AdvTree.Node pNode, SysCommon.Gis.SysGisDataSet v_Gisdt, SysCommon.Gis.SysGisDataSet v_histoGisdt, SysCommon.DataBase.SysTable v_Table, DateTime pDate, bool isConne, out Exception eError)
        {
            try
            {
                eError = null;
                Exception ex = null;
                //记录"新建和修改"的数据的图层名、更新后的Feature和原始的OID
                Dictionary<string, Dictionary<XmlNode, int>> pFeaInfo = new Dictionary<string, Dictionary<XmlNode, int>>();
                //加载DGML文档
                XmlDocument DGMLDoc = new XmlDocument();
                DGMLDoc.Load(pFileName);

                #region 将更新要素（新建和修改的要素）对应的图层名和要素节点保存起来
                XmlNodeList RecordList = DGMLDoc.SelectNodes(".//DGML//Data//Record");
                //if (RecordList == null)
                //{
                //    eError = new Exception("xml文档结构不正确，请检查该文档是否为更新导出的文档!");
                //}
                foreach (XmlNode recordNode in RecordList)
                {
                    string pState = recordNode.SelectSingleNode(".//STATE").InnerText.Trim();//更新状态
                    string FCName = recordNode.SelectSingleNode(".//OLDFCNAME").InnerText.Trim();//图层名
                    if (FCName == "")
                    {
                        eError = new Exception("新建或修改的要素图层名为空！");
                        //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "新建或修改的要素图层名为空！");
                        return false;
                    }
                    XmlNode newFeature = recordNode.SelectSingleNode(".//NEWFEATURE");
                    int pOID = -1;
                    #region 根据XML文档读取“新建和修改”的要素信息
                    if (pState == "修改")
                    {
                        if (recordNode.SelectSingleNode(".//GOFID").InnerText.Trim() == "")
                        {
                            eError = new Exception("在" + FCName + "中，修改要素的GOFID为空！");
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "在" + FCName + "中，修改要素的GOFID为空！");
                            return false;
                        }
                        int pFid = int.Parse(recordNode.SelectSingleNode(".//GOFID").InnerText.Trim());//图层名
                        if (pFid == -1)
                        {
                            eError = new Exception("在" + FCName + "中，修改要素的GOFID为-1！");
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "在" + FCName + "中，修改要素的GOFID为-1！");
                            return false;
                        }
                        pOID = GetUserDBOID(pFid, v_Table);
                        if (pOID == -1)
                        {
                            eError = new Exception("获取现势库修改要素源OID失败!");
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取现势库修改要素源OID失败!");
                            return false;
                        }

                        if (!pFeaInfo.ContainsKey(FCName))
                        {
                            Dictionary<XmlNode, int> tempDic = new Dictionary<XmlNode, int>();
                            tempDic.Add(newFeature, pOID);
                            pFeaInfo.Add(FCName, tempDic);
                        }
                        else
                        {
                            if (!pFeaInfo[FCName].ContainsKey(newFeature))
                            {
                                pFeaInfo[FCName].Add(newFeature, pOID);
                            }
                        }
                    }
                    if (pState == "新建")
                    {
                        if (!pFeaInfo.ContainsKey(FCName))
                        {
                            Dictionary<XmlNode, int> tempDic = new Dictionary<XmlNode, int>();
                            tempDic.Add(newFeature, pOID);
                            pFeaInfo.Add(FCName, tempDic);
                        }
                        else
                        {
                            if (!pFeaInfo[FCName].ContainsKey(newFeature))
                            {
                                pFeaInfo[FCName].Add(newFeature, pOID);
                            }
                        }
                    }
                    #endregion
                }
                if (pFeaInfo.Count == 0) return true;
                #endregion
                ////获得目标库Workspace
                //IFeatureWorkspace pFeaWS = v_Gisdt.WorkSpace as IFeatureWorkspace;
                //if (pFeaWS == null) return false;
                //
                //将新的要素导入到相应的图层中
                foreach (KeyValuePair<string, Dictionary<XmlNode, int>> item in pFeaInfo)
                {

                    //添加树图 图层名子节点
                    DevComponents.AdvTree.Node FcNode = new DevComponents.AdvTree.Node();
                    FcNode = (DevComponents.AdvTree.Node)v_AppForm.MainForm.Invoke(new CreateTreeNode(CreateAdvTreeNode), new object[] { pNode.Nodes, item.Key, item.Key, v_AppGIS.DataTree.ImageList.Images[6], true });//图层名子节点
                    //添加树图节点列
                    v_AppForm.MainForm.Invoke(new CreateTreeCell(CreateAdvTreeCell), new object[] { FcNode, "运行", null });
                    v_AppForm.MainForm.Invoke(new CreateTreeCell(CreateAdvTreeCell), new object[] { FcNode, "", null });

                    //设置状态栏的值
                    v_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { "图层" + item.Key + "数据开始入库" });
                    //设置初始进度条
                    int tempValue = 0;
                    v_AppForm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { v_AppForm.ProgressBar, 0, item.Value.Count, tempValue });

                    try
                    {
                        //获得目标库图层
                        IFeatureClass pFeaCLs = v_Gisdt.GetFeatureClass(item.Key.Trim(), out ex);
                        if (ex != null)
                        {
                            eError = new Exception("找不到图层名为" + item.Key.Trim() + "的图层,请检查！");
                            //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误!", "找不到图层名为" + item.Key.Trim() + "的图层,请检查！" });
                            //改变树图运行状态
                            v_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { FcNode, "失败", "", false });
                            return false;
                        }
                        IFeatureClass pHistoFeaCLs = null;
                        if (isConne == true)
                        {
                            //获得历史库图层
                            pHistoFeaCLs = v_histoGisdt.GetFeatureClass(item.Key.Trim() + "_GOH", out ex);
                            if (ex != null)
                            {
                                eError = new Exception("找不到图层名为" + item.Key.Trim() + "_goh 的图层,请检查！");
                                //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误!", "找不到图层名为" + item.Key.Trim() + "_goh 的图层,请检查！" });
                                //改变树图运行状态
                                v_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { FcNode, "失败", "", false });
                                return false;
                            }
                        }
                        # region 将所有的新建的要素导入目标库中，修改的要素进行修改(现势库和历史库)
                        //对于新建的要素,将更新的要素导入到目标库中
                        //创建一个FeatureBuffer，
                        IFeatureBuffer pFeatureBuffer = pFeaCLs.CreateFeatureBuffer();
                        IFeatureCursor pFeaCusor = null;
                        pFeaCusor = pFeaCLs.Insert(true);
                        //遍历要素字典，并导入要素
                        foreach (KeyValuePair<XmlNode, int> FeaStateItem in item.Value)
                        {
                            #region 给"修改"的要素置历史
                            IFeature pHistoFeature = null;
                            int stateIndex = -1;//"状态"字段索引值
                            int sourceIDIndex = -1;//"源OID"索引值
                            if (isConne == true)
                            {
                                //定义插入历史库的要素（新建和修改的雅俗掺入历史库）
                                pHistoFeature = pHistoFeaCLs.CreateFeature();
                                //声明历史库中特殊增加的字段
                                int fromdateIndex = -1;//"生效日期" 字段索引值 
                                int todateIndex = -1;//"失效日期"字段索引值
                                fromdateIndex = pHistoFeature.Fields.FindField("FromDate");
                                todateIndex = pHistoFeature.Fields.FindField("ToDate");
                                stateIndex = pHistoFeature.Fields.FindField("State");
                                sourceIDIndex = pHistoFeature.Fields.FindField("SourceOID");
                                if (fromdateIndex == -1 || todateIndex == -1 || stateIndex == -1 || sourceIDIndex == -1)
                                {
                                    eError = new Exception("找不到历史库中相应的字段！");
                                    //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "提示!", "找不到历史库中相应的字段！" });
                                    //改变树图运行状态
                                    v_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { FcNode, "失败", "", false });
                                }
                                //给历史库中特殊增加的字段赋值
                                pHistoFeature.set_Value(fromdateIndex, pDate.ToString("u") as object);
                                pHistoFeature.set_Value(todateIndex, DateTime.MaxValue.ToString("u") as object);
                            }
                            #endregion

                            //对于修改的要素，首先在现势库里查找到对应的要素，然后修改字段的值
                            IFeatureCursor pUpdateCurosr = null; //“修改”的要素游标
                            IFeature pUpdateFeature = null;//"修改"的要素

                            XmlNode FeatureNode = FeaStateItem.Key;//要素节点
                            int ppOID = FeaStateItem.Value;//旧的要素的OID
                            if (ppOID != -1)
                            {
                                //修改的要素
                                IQueryFilter ppFilter = new QueryFilterClass();
                                ppFilter.WhereClause = "OBJECTID=" + ppOID;
                                pUpdateCurosr = pFeaCLs.Update(ppFilter, false);
                                if (pUpdateCurosr == null) return false;
                                pUpdateFeature = pUpdateCurosr.NextFeature();
                                if (pUpdateFeature == null)
                                {
                                    eError = new Exception("在现势库中找不到OBJECTID为" + ppOID + "修改的要素！");
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "在现势库中找不到OBJECTID为"+ppOID+"修改的要素！");
                                    return false;
                                }
                            }
                            XmlNodeList valueNodeList = FeatureNode.SelectNodes(".//Value");
                            //给要素赋值。遍历value节点，获得要素每个字段的值
                            foreach (XmlNode valueNode in valueNodeList)
                            {
                                string fielName = ""; //字段名称
                                string fieldVaue = "";//字段值
                                int fieldIndex = -1; //字段索引
                                fielName = valueNode.SelectSingleNode(".//FieldName").InnerText.Trim();
                                if (fielName == "GOFID") continue;
                                fieldVaue = valueNode.SelectSingleNode(".//FieldValue").InnerText.Trim();
                                if (fieldVaue == "") continue;
                                fieldIndex = pFeaCLs.Fields.FindField(fielName);
                                if (fieldIndex == -1)
                                {
                                    eError = new Exception("找不到字段名为'" + fielName.Trim() + "'的字段,请检查！");
                                    //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误!", "找不到字段名为'" + fielName.Trim() + "'的字段,请检查！" });
                                    //改变树图运行状态
                                    v_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { FcNode, "失败", "", false });
                                    return false;
                                }
                                IField pField = pFeaCLs.Fields.get_Field(fieldIndex);
                                if (pField.Editable == false) continue;
                                //给普通字段赋值
                                #region “新建”的要素入库，“修改”的要素对库中的数据进行修改
                                if (pField.Type != esriFieldType.esriFieldTypeBlob && pField.Type != esriFieldType.esriFieldTypeGeometry)
                                {
                                    if (ppOID == -1)
                                    {
                                        //新建的要素
                                        pFeatureBuffer.set_Value(fieldIndex, fieldVaue as object);
                                    }
                                    else
                                    {
                                        //修改的要素
                                        pUpdateFeature.set_Value(fieldIndex, fieldVaue as object);
                                    }
                                    if (isConne == true)
                                    {
                                        //对于历史库来说，修改和新建的要素都要导入到历史库中
                                        pHistoFeature.set_Value(fieldIndex, fieldVaue as object);
                                    }
                                }
                                else
                                {
                                    # region 特殊字段经过解析后赋值
                                    //将xml字符串转化为字节
                                    byte[] xmlByte = Convert.FromBase64String(fieldVaue);
                                    //用来记录解析后的字段值
                                    object fieldShape = null;

                                    //根据不同的情况对fieldShape进行初始化
                                    if (pFeaCLs.FeatureType == esriFeatureType.esriFTAnnotation)
                                    {
                                        //注记
                                        if (pField.Type == esriFieldType.esriFieldTypeGeometry)
                                        {
                                            fieldShape = new PolygonElementClass();//几何字段
                                        }
                                        if (pField.Type == esriFieldType.esriFieldTypeBlob)
                                        {
                                            fieldShape = new TextElementClass();//BLOB字段
                                        }
                                    }
                                    else
                                    {
                                        //普通要素类
                                        if (pFeaCLs.ShapeType == esriGeometryType.esriGeometryPoint)
                                        {
                                            fieldShape = new PointClass();
                                        }
                                        else if (pFeaCLs.ShapeType == esriGeometryType.esriGeometryPolyline)
                                        {
                                            fieldShape = new PolylineClass();
                                        }
                                        else if (pFeaCLs.ShapeType == esriGeometryType.esriGeometryPolygon)
                                        {
                                            fieldShape = new PolygonClass();
                                        }
                                    }

                                    //对字段进行解析并赋值
                                    if (XmlDeSerializer(xmlByte, fieldShape) == true)
                                    {
                                        if (pField.Type == esriFieldType.esriFieldTypeGeometry)
                                        {
                                            if (ppOID == -1)
                                            {
                                                //新建的要素
                                                pFeatureBuffer.Shape = fieldShape as IGeometry;
                                                //pFeatureBuffer.set_Value(fieldIndex, fieldShape);
                                            }
                                            else
                                            {
                                                //修改的要素
                                                pUpdateFeature.Shape = fieldShape as IGeometry;
                                            }
                                            if (isConne == true)
                                            {
                                                //给历史库的Geometry字段赋值
                                                pHistoFeature.Shape = fieldShape as IGeometry;
                                            }
                                        }
                                        else if (pField.Type == esriFieldType.esriFieldTypeBlob)
                                        {
                                            if (ppOID == -1)
                                            {
                                                //新建的要素
                                                IAnnotationFeature pAnnoFeature = pFeatureBuffer as IAnnotationFeature;
                                                pAnnoFeature.Annotation = fieldShape as IElement;
                                            }
                                            else
                                            {
                                                //修改的要素
                                                IAnnotationFeature pUpdateAnnoFeature = pUpdateFeature as IAnnotationFeature;
                                                pUpdateAnnoFeature.Annotation = fieldShape as IElement;
                                            }
                                            if (isConne == true)
                                            {
                                                //给历史库中注记的特殊字段赋值
                                                IAnnotationFeature pHistoAnnoFeature = pHistoFeature as IAnnotationFeature;
                                                pHistoAnnoFeature.Annotation = fieldShape as IElement;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        eError = new Exception("解析字段值出错！");
                                        //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "解析字段值出错！" });
                                        //改变树图运行状态
                                        v_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { FcNode, "失败", "", false });
                                        return false;
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            //存储要素，并修改FID记录表
                            if (ppOID == -1)
                            {
                                //新建的要素
                                int newOID = (int)pFeaCusor.InsertFeature(pFeatureBuffer);
                                //将新建的要素信息写入FID记录表
                                if (UpdateFIDTable(item.Key, newOID, v_Table, out ex) == false)
                                {
                                    eError = ex;
                                    //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误!", "修改FID记录表失败！" });
                                    //改变树图运行状态
                                    v_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { FcNode, "失败", "", false });
                                    return false;
                                }
                                if (isConne == true)
                                {
                                    //给历史库中特殊增加的字段赋值（新建的要素的状态值）
                                    pHistoFeature.set_Value(stateIndex, 1 as object);
                                    pHistoFeature.set_Value(sourceIDIndex, newOID as object);
                                }
                            }
                            else
                            {
                                //修改目标库中中要素
                                pUpdateCurosr.UpdateFeature(pUpdateFeature);
                                pUpdateCurosr.Flush();

                                if (isConne == true)
                                {
                                    //给历史库中特殊增加的字段赋值（修改的要素）
                                    pHistoFeature.set_Value(stateIndex, 2 as object);
                                    pHistoFeature.set_Value(sourceIDIndex, ppOID as object);
                                }
                            }
                            if (isConne == true)
                            {
                                //存储历史库中的要素
                                pHistoFeature.Store();
                            }
                            //释放修改游标
                            if (pUpdateCurosr != null)
                            {
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(pUpdateCurosr);
                            }
                            tempValue += 1;//进度条的值加1
                            v_AppForm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { v_AppForm.ProgressBar, -1, -1, tempValue });
                        }
                        if (pFeaCusor != null)
                        {
                            pFeaCusor.Flush();
                        }
                        //释放插入游标
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCusor);
                        #endregion


                        //改变树图运行状态
                        v_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { FcNode, "完成", item.Value.Count + "个要素完成入库", false });
                    }
                    catch (Exception eex)
                    {
                        //*******************************************************************
                        //Exception Log
                        if (ModData.SysLog != null)
                        {
                            ModData.SysLog.Write(eex, null, DateTime.Now);
                        }
                        else
                        {
                            ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                            ModData.SysLog.Write(eex, null, DateTime.Now);
                        }
                        //********************************************************************
                        eError = eex;
                        //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误!", eex.Message });
                        //改变树图运行状态
                        v_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { FcNode, "失败", "", false });
                        return false;
                    }
                }
                return true;
            }
            catch (Exception eeex)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(eeex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(eeex, null, DateTime.Now);
                }
                //********************************************************************
                eError = eeex;
                return false;
            }

        }

        /// <summary>
        /// 根据DGML提交后修改FID记录表
        /// </summary>
        /// <param name="FCName">图层名</param>
        /// <param name="newOID">更新后的OID</param>
        /// <param name="pSysTable">FID记录表连接</param>
        /// <returns></returns>
        private bool UpdateFIDTable(string FCName, int newOID, SysCommon.DataBase.SysTable v_Table, out Exception eError)
        {
            eError = null;
            Exception ex = null;
            string str = "insert into FID记录表(FCNAME,OID) values('" + FCName + "'," + newOID.ToString() + ")";
            v_Table.UpdateTable(str, out ex);
            if (ex != null)
            {
                eError = new Exception("修改FID记录表失败！");
                //v_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "插入FID记录表出错！" });
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将xmlByte解析为obj
        /// </summary>
        /// <param name="xmlByte"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool XmlDeSerializer(byte[] xmlByte, object obj)
        {
            try
            {
                //判断字符串是否为空
                if (xmlByte != null)
                {
                    ESRI.ArcGIS.esriSystem.IPersistStream pStream = obj as ESRI.ArcGIS.esriSystem.IPersistStream;

                    ESRI.ArcGIS.esriSystem.IXMLStream xmlStream = new ESRI.ArcGIS.esriSystem.XMLStreamClass();

                    xmlStream.LoadFromBytes(ref xmlByte);
                    pStream.Load(xmlStream as ESRI.ArcGIS.esriSystem.IStream);

                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                return false;
            }
        }
        #endregion

        #region 处理树图相关函数
        //创建处理树图
        private delegate void InitTree(DevComponents.AdvTree.AdvTree aTree);
        private void IntialTree(DevComponents.AdvTree.AdvTree aTree)
        {
            DevComponents.AdvTree.ColumnHeader aColumnHeader;
            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "FCName";
            aColumnHeader.Text = "图层名";
            aColumnHeader.Width.Relative = 50;
            aTree.Columns.Add(aColumnHeader);

            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "NodeState";
            aColumnHeader.Text = "状态";
            aColumnHeader.Width.Relative = 25;
            aTree.Columns.Add(aColumnHeader);

            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "NodeRes";
            aColumnHeader.Text = "结果";
            aColumnHeader.Width.Relative = 25;
            aTree.Columns.Add(aColumnHeader);
        }
        //设置选中树节点的颜色
        private delegate void SetSelectNodeColor(DevComponents.AdvTree.AdvTree aTree);
        private void setNodeColor(DevComponents.AdvTree.AdvTree aTree)
        {
            DevComponents.DotNetBar.ElementStyle elementStyle = new DevComponents.DotNetBar.ElementStyle();
            elementStyle.BackColor = Color.FromArgb(255, 244, 213);
            elementStyle.BackColor2 = Color.FromArgb(255, 216, 105);
            elementStyle.BackColorGradientAngle = 90;
            elementStyle.Border = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderBottomWidth = 1;
            elementStyle.BorderColor = Color.DarkGray;
            elementStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderLeftWidth = 1;
            elementStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderRightWidth = 1;
            elementStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderTopWidth = 1;
            elementStyle.BorderWidth = 1;
            elementStyle.CornerDiameter = 4;
            elementStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            aTree.NodeStyleSelected = elementStyle;
            aTree.DragDropEnabled = false;
        }

        //创建树图节点
        private delegate DevComponents.AdvTree.Node CreateTreeNode(DevComponents.AdvTree.NodeCollection nodeCol, string strText, string strName, Image pImage, bool bExpand);
        private DevComponents.AdvTree.Node CreateAdvTreeNode(DevComponents.AdvTree.NodeCollection nodeCol, string strText, string strName, Image pImage, bool bExpand)
        {

            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
            node.Text = strText;
            node.Image = pImage;
            if (strName != null)
            {
                node.Name = strName;
            }

            if (bExpand == true)
            {
                node.Expand();
            }

            nodeCol.Add(node);
            return node;
        }

        //添加树图节点列
        private delegate DevComponents.AdvTree.Cell CreateTreeCell(DevComponents.AdvTree.Node aNode, string strText, Image pImage);
        private DevComponents.AdvTree.Cell CreateAdvTreeCell(DevComponents.AdvTree.Node aNode, string strText, Image pImage)
        {
            DevComponents.AdvTree.Cell aCell = new DevComponents.AdvTree.Cell(strText);
            aCell.Images.Image = pImage;
            aNode.Cells.Add(aCell);

            return aCell;
        }

        //为数据处理树图节点添加处理结果状态
        private delegate void ChangeSelectNode(DevComponents.AdvTree.Node aNode, string strMemo, string strRes, bool bClear);
        private void ChangeTreeSelectNode(DevComponents.AdvTree.Node aNode, string strMemo, string strRes, bool bClear)
        {
            if (aNode == null)
            {
                v_AppGIS.DataTree.SelectedNode = null;
                v_AppGIS.DataTree.Refresh();
                return;
            }

            v_AppGIS.DataTree.SelectedNode = aNode;
            if (bClear)
            {
                v_AppGIS.DataTree.SelectedNode.Nodes.Clear();
            }
            v_AppGIS.DataTree.SelectedNode.Cells[1].Text = strMemo;
            v_AppGIS.DataTree.SelectedNode.Cells[2].Text = strRes;
            v_AppGIS.DataTree.Refresh();
        }
        #endregion

        #region 进度条显示
        //控制进度条显示
        private delegate void ShowProgress(bool bVisible);
        private void ShowProgressBar(bool bVisible)
        {
            if (bVisible == true)
            {
                v_AppForm.ProgressBar.Visible = true;
            }
            else
            {
                v_AppForm.ProgressBar.Visible = false;
            }
        }
        //修改进度条
        private delegate void ChangeProgress(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value);
        private void ChangeProgressBar(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value)
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


        //改变状态栏提示内容
        private delegate void ShowTips(string strText);
        private void ShowStatusTips(string strText)
        {
            v_AppForm.OperatorTips = strText;
        }
        #endregion

        #region 提示对话框
        private delegate void ShowForm(string strCaption, string strText);
        private void ShowErrForm(string strCaption, string strText)
        {
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle(strCaption, strText);
        }
        private delegate bool ShowInfoForm();
        private bool ShowInfoDial(string StrOK, string StrCancel, string StrDesc)
        {
            bool isHisto = true;
            SysCommon.Error.frmInformation InfoDial = new SysCommon.Error.frmInformation(StrOK, StrCancel, StrDesc);
            if (InfoDial.ShowDialog() == DialogResult.OK)
            {
                isHisto = true;
            }
            else
            {
                isHisto = false;
            }
            return isHisto;
        }
        #endregion
    }
}
