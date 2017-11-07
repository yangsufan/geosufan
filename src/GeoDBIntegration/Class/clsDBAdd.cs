using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SysCommon;
using System.Xml;
using SCHEMEMANAGERCLASSESLib;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using System.IO;

namespace GeoDBIntegration
{
    /* guozheng  2010.9.28
     * 该类实现在系统维护库中添加、删除库体、

     * 框架要素库的库体初始化

     * 需要提供数据库类型，数据库平台类型，数据库名称
     */
    class clsDBAdd
    {
        private SysCommon.DataBase.SysTable m_DataBaseOper;
        public SysCommon.DataBase.SysTable DataBaseOper
        {
            get { return this.m_DataBaseOper; }
            set { this.m_DataBaseOper = value; }
        }

        private System.Windows.Forms.ProgressBar m_ProcBar = null;///////////进度条对象
        public System.Windows.Forms.ProgressBar ProcBar
        {
            get { return this.m_ProcBar; }
            set { this.m_ProcBar = value; }
        }
        #region 构造函数

        /// <summary>
        /// 构造函数（系统维护库连接对象初始化）

        /// </summary>
        /// <param name="DBConType">系统维护库连接类型</param>
        /// <param name="DBType">系统维护库类型</param>
        /// <param name="sConInfo">连接字符串</param>
        public clsDBAdd(SysCommon.enumDBConType DBConType, SysCommon.enumDBType DBType, string sConInfo)
        {
            Exception ex = null;
            this.m_DataBaseOper = new SysCommon.DataBase.SysTable();
            this.m_DataBaseOper.SetDbConnection(sConInfo, DBConType, DBType, out ex);
            if (ex != null) this.m_DataBaseOper = null;
        }
        public clsDBAdd()
        {
            this.m_DataBaseOper = null;
        }
        #endregion
        /// <summary>cyf 20110629 modify cyf 20110627 modify:将类型中通过名称进行判断改为通过ID来进行判断//enumInterDBType enDBType, enumInterDBFormat enDBFormat,
        /// 系统维护库中新增一个数据库记录，初始化为“库体未初始化状态” cyf 20110615 add:添加将栅格目录的ftp信息和根目录信息下入系统维护库中  
        /// cyf 20110609 modify:添加写入栅格数据的DBpara信息（栅格数据类型：栅格编目、栅格数据集）
        /// </summary>
        /// <param name="sDBName">数据库名称</param>
        /// <param name="enDBType">数据库类型</param>
        /// <param name="enDBFormat">数据库平台类型</param>
        /// <param name="lDBID">输出：建立的数据库ID，错误返回-1</param>
        /// <param name="ex">输出:错误信息</param>
        public void AddNewDB(string sDBName, long lDBTypeID,long lDBFormatID, string sConnectIfo,string ftpConnInfo,string pDBPara, out long lDBID,string pUpdateType,int pScale, out Exception ex)
        {
            ex = null;
            lDBID = -1;
            //long lDBTypeID = -1;
            //long lDBFormatID = -1;
            long lDBStateID = -1;
            #region 参数检测

            if (sDBName == null) { ex = new Exception("数据库名称为空"); return; }
            //cyf 201100602 modify :修改系统维护库连接方式
            //if (this.m_DataBaseOper == null) { ex = new Exception("系统维护库连接信息未初始化"); return; }
            if (ModuleData.TempWks == null) { ex = new Exception("系统维护库连接失败"); return; }
            //end
            #endregion
            #region cyf 20110627 modify 注释
            ////***********************************************************************************
            ////cyf 20110602 
            //////////获取数据库类型、
            //IQueryDef pQuerDef = null;
            //IFeatureWorkspace pFeaWs = ModuleData.TempWks as IFeatureWorkspace;
            //if (pFeaWs == null) { ex = new Exception("系统维护库连接失败"); return; }
            //pQuerDef = pFeaWs.CreateQueryDef();
            //pQuerDef.Tables = "DATABASETYPEMD";
            //pQuerDef.SubFields="ID";
            //pQuerDef.WhereClause = "DATABASETYPE = '" + enDBType.ToString() + "'";
            //ICursor pCursor = null; 
            //try
            //{
            //    //查询系统维护库
            //    pCursor = pQuerDef.Evaluate();
            //    if (pCursor == null) { ex = new Exception("查询系统维护库失败！"); return; }
            //    IRow pRow = pCursor.NextRow();
            //    while (pRow != null)
            //    {
            //        lDBTypeID = Convert.ToInt64(pRow.get_Value(0).ToString().Trim());
            //        break;
            //        //pRow = pCursor.NextRow();
            //    }
            //} catch (Exception eError)
            //{
            //    //****************************************************
            //    if (ModuleData.v_SysLog != null)
            //        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
            //    //****************************************************
            //    ex = new Exception("不支持的数据库类型");
            //    return;
            //}
            ////////////////平台类型ID/////////
            //pQuerDef = pFeaWs.CreateQueryDef();
            //pQuerDef.Tables = "DATABASEFORMATMD";
            //pQuerDef.SubFields = "ID";
            //pQuerDef.WhereClause = "DATABASEFORMAT = '" + enDBFormat.ToString() + "'";
            //pCursor = null;
            //try
            //{
            //    //查询系统维护库
            //    pCursor = pQuerDef.Evaluate();
            //    if (pCursor == null) { ex = new Exception("查询系统维护库失败！"); return; }
            //    IRow pRow = pCursor.NextRow();
            //    while (pRow != null)
            //    {
            //        lDBFormatID = Convert.ToInt64(pRow.get_Value(0).ToString().Trim());
            //        break;
            //        //pRow = pCursor.NextRow();
            //    }
              
            //} catch (Exception eError)
            //{
            //    //****************************************************
            //    if (ModuleData.v_SysLog != null)
            //        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
            //    //****************************************************
            //    ex = new Exception("不支持的数据库平台类型");
            //    return;
            //}

            /////////////新增加的数据库设置为未初始化状态///////////
            //pQuerDef = pFeaWs.CreateQueryDef();
            //pQuerDef.Tables = "DATABASESTATEMD";
            //pQuerDef.SubFields = "ID";
            //pCursor = null;
            //if (pUpdateType == EnumUpdateType.New.ToString())
            //{
            //    pQuerDef.WhereClause = "DATABASESTATE ='库体未初始化'";
            //}
            //else if (pUpdateType == EnumUpdateType.Update.ToString())
            //{
            //    pQuerDef.WhereClause = "DATABASESTATE ='库体已初始化'";
            //}
            //try
            //{
            //    //查询系统维护库
            //    pCursor = pQuerDef.Evaluate();
            //    if (pCursor == null) { ex = new Exception("查询系统维护库失败！"); return; }
            //    IRow pRow = pCursor.NextRow();
            //    if (pRow != null)
            //    {
            //        lDBStateID = Convert.ToInt64(pRow.get_Value(0).ToString().Trim());
            //    }
            //}
            //catch (Exception eError)
            //{
            //    //****************************************************
            //    if (ModuleData.v_SysLog != null)
            //        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
            //    //****************************************************
            //    ex = new Exception("数据库状态获取失败");
            //    return;
            //}
            #endregion
            /////////////新增加的数据库设置为未初始化状态///////////
            //cyf 20110627
            if (pUpdateType == EnumUpdateType.New.ToString())
            {
                lDBStateID = (long)EnumDBState.库体未初始化.GetHashCode();
            }
            else if (pUpdateType == EnumUpdateType.Update.ToString())
            {
                lDBStateID = (long)EnumDBState.库体已初始化.GetHashCode();
            }
            /////////////判断数据库名称是否存在重名/////////////////
            IQueryDef pQuerDef = null;
            IFeatureWorkspace pFeaWs = ModuleData.TempWks as IFeatureWorkspace;
            if (pFeaWs == null) { ex = new Exception("系统维护库连接失败"); return; }
            pQuerDef = pFeaWs.CreateQueryDef();
            pQuerDef.Tables = "DATABASEMD";
            pQuerDef.SubFields = "ID";
            pQuerDef.WhereClause = " DATABASENAME='" + sDBName + "' AND DATABASETYPEID=" + lDBTypeID.ToString();
            ICursor pCursor = null;
            try
            {
                //查询系统维护库
                pCursor = pQuerDef.Evaluate();
                if (pCursor == null) { ex = new Exception("查询系统维护库失败！"); return; }
                IRow pRow = pCursor.NextRow();
               if (pRow != null)
                {
                    ex = new Exception("已存在同名的数据库");
                    return;
                }

            } catch (Exception eError)
            {
                //****************************************************
                if (ModuleData.v_SysLog != null)
                    ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                //****************************************************
                ex = new Exception("读取数据库名称失败");
                return;
            }
          
            /////////////////插入数据库记录///////////////
            ////获取一个可用的主码

            ITable pTable = pFeaWs.OpenTable("DATABASEMD");
            try
            {
                long count = Convert.ToInt64(pTable.RowCount(null).ToString());
                if (count == 0)
                {
                    lDBID = 1;
                }
                else
                {
                    IDataStatistics pDataSta = new DataStatisticsClass();
                    pDataSta.Field = "ID";
                    pDataSta.Cursor = pTable.Search(null, false);
                    IStatisticsResults pStaRes = null;
                    pStaRes = pDataSta.Statistics;
                    count = (long)pStaRes.Maximum;
                    lDBID = count + 1;
                }
            } catch (Exception eError)
            {
                //****************************************************
                if (ModuleData.v_SysLog != null)
                    ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                //****************************************************
                ex = new Exception("获取数据库ID失败");
                lDBID = -1;
                return;
            }

            //释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

            /////插入数据库记录
            string sql="";
            if (pUpdateType == EnumUpdateType.New.ToString())
            {
                sql = "INSERT INTO DATABASEMD(OBJECTID,ID,DATABASENAME,DATABASETYPEID,DATABASSTATEID,DATAFORMATID,CONNECTIONINFO,ftpConn)";
                sql += " VALUES(" + lDBID.ToString() +","+ lDBID.ToString() + ",'" + sDBName + "'," + lDBTypeID.ToString() + "," + lDBStateID.ToString() + "," + lDBFormatID.ToString() + ",'" + sConnectIfo + "','"+ftpConnInfo+"')";
            }
            else if (pUpdateType == EnumUpdateType.Update.ToString())
            {
                if (lDBFormatID.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString() || lDBFormatID.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString() || lDBFormatID.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                {
                    //cyf 20110609 modify
                    //ArcGIS
                    sql = "INSERT INTO DATABASEMD(OBJECTID,ID,DATABASENAME,DATABASETYPEID,DATABASSTATEID,DATAFORMATID,CONNECTIONINFO,SCALE,DBPARA,ftpConn)";
                    sql += " VALUES(" + lDBID.ToString() + "," + lDBID.ToString() + ",'" + sDBName + "'," + lDBTypeID.ToString() + "," + lDBStateID.ToString() + "," + lDBFormatID.ToString() + ",'" + sConnectIfo + "','" + pScale + "','" + pDBPara + "','" + ftpConnInfo + "')";
                    //end
                }
            }
            try { ModuleData.TempWks.ExecuteSQL(sql); } catch(Exception err) { lDBID = -1; return; }
            //end
            //****************************************************************************************************

            #region 原有代码  注释掉了
            //string sql = "SELECT ID FROM DATABASETYPEMD WHERE DATABASETYPE ='" + enDBType.ToString() + "'";
            //try
            //{
            //    DataTable gettable = this.m_DataBaseOper.GetSQLTable(sql, out ex);
            //    if (ex != null) return;
            //    lDBTypeID = Convert.ToInt64(gettable.Rows[0][0].ToString());
            //}
            //catch (Exception eError)
            //{
            //    //****************************************************
            //    if (ModuleData.v_SysLog != null)
            //        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
            //    //****************************************************
            //    ex = new Exception("不支持的数据库类型");
            //    return;
            //}
            //////////
         
            
            //sql = "SELECT ID FROM DATABASEFORMATMD WHERE DATABASEFORMAT ='" + enDBFormat.ToString() + "'";
            //try
            //{
            //    DataTable gettable = this.m_DataBaseOper.GetSQLTable(sql, out ex);
            //    if (ex != null) return;
            //    lDBFormatID = Convert.ToInt64(gettable.Rows[0][0].ToString());
            //}
            //catch (Exception eError)
            //{
            //    //****************************************************
            //    if (ModuleData.v_SysLog != null)
            //        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
            //    //****************************************************
            //    ex = new Exception("不支持的数据库平台类型");
            //    return;
            //}
            ///////////////判断数据库名称是否存在重名/////////////////
            //sql = "SELECT ID FROM DATABASEMD WHERE DATABASENAME='" + sDBName + "' AND DATABASETYPEID=" + lDBTypeID.ToString();
            //try
            //{
            //    DataTable gettable = this.m_DataBaseOper.GetSQLTable(sql, out ex);
            //    if (ex != null) return;
            //    if (gettable.Rows.Count > 0)
            //    {
            //        ex = new Exception("已存在同名的数据库");
            //        return;
            //    }
            //}
            //catch (Exception eError)
            //{
            //    //****************************************************
            //    if (ModuleData.v_SysLog != null)
            //        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
            //    //****************************************************
            //    ex = new Exception("读取数据库名称失败");
            //    return;
            //}
            ///////////////新增加的数据库设置为未初始化状态///////////
            //if (pUpdateType == EnumUpdateType.New.ToString())
            //{
            //    sql = "SELECT ID FROM DATABASESTATEMD WHERE DATABASESTATE ='库体未初始化'";
            //}
            //else if(pUpdateType==EnumUpdateType.Update.ToString())
            //{
            //    sql = "SELECT ID FROM DATABASESTATEMD WHERE DATABASESTATE ='库体已初始化'";
            //}
            //try
            //{
            //    DataTable gettable = this.m_DataBaseOper.GetSQLTable(sql, out ex);
            //    if (ex != null) return;
            //    lDBStateID = Convert.ToInt64(gettable.Rows[0][0].ToString());
            //}
            //catch (Exception eError)
            //{
            //    //****************************************************
            //    if (ModuleData.v_SysLog != null)
            //        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
            //    //****************************************************
            //    ex = new Exception("数据库状态获取失败");
            //    return;
            //}
            ///////////////////插入数据库记录///////////////
            //////获取一个可用的主码
            //sql = "SELECT  COUNT(*)  FROM  DATABASEMD";
            //try
            //{
            //    DataTable gettable = this.m_DataBaseOper.GetSQLTable(sql, out ex);
            //    if (ex != null) return;
            //    long count = Convert.ToInt64(gettable.Rows[0][0].ToString());
            //    if (count == 0) lDBID = 1;
            //    else
            //    {
            //        sql = "SELECT MAX(ID) FROM DATABASEMD";
            //        gettable = this.m_DataBaseOper.GetSQLTable(sql, out ex);
            //        if (ex != null) return;
            //        count = Convert.ToInt64(gettable.Rows[0][0].ToString());
            //        lDBID = count + 1;
            //    }
            //}
            //catch (Exception eError)
            //{
            //    //****************************************************
            //    if (ModuleData.v_SysLog != null)
            //        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
            //    //****************************************************
            //    ex = new Exception("获取数据库ID失败");
            //    lDBID = -1;
            //    return;
            //}
            ///////插入数据库记录
            //if (pUpdateType == EnumUpdateType.New.ToString())
            //{
            //    sql = "INSERT INTO DATABASEMD(ID,DATABASENAME,DATABASETYPEID,DATABASSTATEID,DATAFORMATID,CONNECTIONINFO)";
            //    sql += " VALUES(" + lDBID.ToString() + ",'" + sDBName + "'," + lDBTypeID.ToString() + "," + lDBStateID.ToString() + "," + lDBFormatID.ToString() + ",'" + sConnectIfo + "')";
            //}
            //else if (pUpdateType == EnumUpdateType.Update.ToString())
            //{
            //    if (enDBFormat == enumInterDBFormat.ARCGISGDB || enDBFormat == enumInterDBFormat.ARCGISPDB|| enDBFormat == enumInterDBFormat.ARCGISSDE)
            //    {
            //        //ArcGIS
            //        sql = "INSERT INTO DATABASEMD(ID,DATABASENAME,DATABASETYPEID,DATABASSTATEID,DATAFORMATID,CONNECTIONINFO,SCALE)";
            //        sql += " VALUES(" + lDBID.ToString() + ",'" + sDBName + "'," + lDBTypeID.ToString() + "," + lDBStateID.ToString() + "," + lDBFormatID.ToString() + ",'" + sConnectIfo + "',"+pScale+")";
      
            //    }
            //}
            //    this.m_DataBaseOper.UpdateTable(sql, out ex);
            //if (ex != null) { lDBID = -1; return; }
            #endregion
        }
       
        /// <summary>
        /// 框架要素库体初始化函数  
        /// </summary>
        /// <param name="enumDBFormat">数据库平台</param>
        /// <param name="enumDBType">数据库类型</param>
        /// <param name="sServer">服务器</param>
        /// <param name="sServerName">服务名</param>
        /// <param name="sDataBase">数据库</param>
        /// <param name="sUser">用户</param>
        /// <param name="sPassWord">密码</param>
        /// <param name="sVersion">版本</param>
        /// <param name="sProj">空间参考</param>
        /// <param name="sDBShcame">配置方案文件</param>
        /// <param name="iScale">比例尺（输出）</param>
        /// <param name="sDBFeatureName">数据集名称（输出）</param>
        /// <param name="ex"></param>cyf 20110707 modify
        public void DBCreate(string pDBFormatID, string pDBTypeID, string sServer, string sServerName, string sDataBase, string sUser, string sPassWord, string sVersion, string sProj, string sDBShcame, out string iScale, out List<string> sDBFeatureName, out Exception ex)
        {
            ex = null;
            //cyf 20110622 add;
            //iScale = -1;
            iScale = "";
            //end
            sDBFeatureName = new List<string>();//cyf 20110707 add string.Empty;
            //string sDBName = string.Empty;///////要素集(GB500)
            //cyf 20110628 modify
            if (pDBTypeID == enumInterDBType.框架要素数据库.GetHashCode().ToString())
            {
                #region 创建框架要素库
                //cyf 20110628 modify
                if (pDBFormatID == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString() || pDBFormatID == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString() || pDBFormatID == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                {
                    this.m_ProcBar.Visible = true;

                    //yjl 重新实现 针对pdb 20110813
                    //
                    //
                    //
                    //
                    CProgress pgss = new CProgress("正在初始化数据库，请稍候...");
                    pgss.EnableCancel = false;
                    pgss.ShowDescription = false;
                    pgss.FakeProgress = true;
                    pgss.TopMost = true;
                    pgss.ShowProgress();
                    ArcGisDBCreateNew(pDBFormatID, sServer, sDataBase, sUser, sPassWord, sProj, sDBShcame, sServerName, sVersion, out iScale, out sDBFeatureName, out ex);
                    pgss.Close();
                  //  ArcGisDBCreate(pDBFormatID, sServer, sDataBase, sUser, sPassWord, sProj, sDBShcame, sServerName, sVersion, out iScale, out sDBFeatureName, out ex);
                    if (ex != null) { this.m_ProcBar.Visible = false; return; }
                    this.m_ProcBar.Visible = false;
                }
                else if (pDBFormatID == enumInterDBFormat.GEOSTARACCESS.GetHashCode().ToString() || pDBFormatID == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString() || pDBFormatID == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString())
                {
                    //GeostarDBCreate(enumDBFormat, sServer, sDataBase, sUser, sPassWord, sProj, sDBShcame, out iScale, out sDBName, out ex);
                }
                else if (pDBFormatID == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString())
                {
                    //cyf 20110622 modify
                    //OracleSpatialDBCreate(sServer, sUser, sPassWord, sProj, sDBShcame, out iScale, out sDBName, out ex);
                    //if (ex != null) { return; }
                    //sDBFeatureName = sDBName;
                    //end
                }
                //end
                //sDBFeatureName = sDBName;
                #endregion
                //cyf 20110623 add:添加更新数据库状态信息
                #region 更新系统维护库信息。将比例尺信息、连接字符串信息（数据集信息）、数据库状态信息写入数据库  chenyafei 添加20101013
                /*
                    DevComponents.AdvTree.Node rootNode = null;//根节点

                    rootNode = pCurNode.Parent.Parent;
                    if (rootNode == null)
                    {
                        ex = new Exception("根节点为空！");
                        return;
                    }
                    if (rootNode.Tag == null)
                    {
                        ex = new Exception("获取系统维护库连接失败！");
                        return;
                    }
                    //cyf 20110603 modify
                    //更新比例尺信息
                    IFeatureWorkspace pFeaWS = ModuleData.TempWks as IFeatureWorkspace;
                    //cyf 20110622 modify:存在多数据集时，将数据集名称已逗号隔开进行保存，比例尺以逗号隔开进行保存
                    string pScaleStr = iScale;  //所有的比例尺信息（以逗号隔开）
                    string GetDtNames = "";//所有的数据集信息（以逗号隔开）
                    if (pFeaWS != null)
                    {
                          #region 更新比例尺信息和数据集信息
                        //首先读取数据库中比例尺的信息
                        //string pScaleStr = iScale.ToString();  //当前比例尺信息
                        string pConnDtStr = "";              //当前数据库连接信息字段
                        string pTableNames = "DATABASEMD";
                        string pFieldNames = "SCALE,CONNECTIONINFO";
                        string pWhereStr = "ID=" + Convert.ToInt32(pDBID);
                        ICursor pCursor = null;
                        pCursor = GetCursor(pFeaWS, pTableNames, pFieldNames, pWhereStr, out ex);
                        if (pCursor == null || ex != null)
                        {
                            ex = new Exception("查询系统维护库中比例尺信息失败！");
                            return;
                        }
                        IRow pRow = pCursor.NextRow();   //查询到的行
                        string pConnInfoFromDB = "";//查询到的连接信息
                        string getScale = "";       //查询到的比例尺信息
                        if (pRow != null)
                        {
                            getScale = pRow.get_Value(0).ToString();             
                            pConnInfoFromDB = pRow.get_Value(1).ToString();
                        }
                        if (getScale.Trim() != "")
                        {
                            pScaleStr = getScale + "," + pScaleStr;
                        }
                        if (pConnInfoFromDB.Trim() != "")
                        {
                            string pConnStr = pConnInfoFromDB.Substring(0, (pConnInfoFromDB.LastIndexOf('|') + 1));//连接信息不带数据集的
                            string getDtName = pConnInfoFromDB.Substring(pConnInfoFromDB.LastIndexOf('|') + 1).Trim();//已经存储的数据集名称
                            if (getDtName.Length == 0)
                            {
                                pConnDtStr = pConnStr + sDBName;
                                GetDtNames = sDBName;
                            }
                            else
                            {
                                pConnDtStr = pConnInfoFromDB + "," + sDBName;
                                GetDtNames = getDtName + "," + sDBName;
                            }
                        }
                        //其次，将读取出来的比例尺的信息+“，”+“当前比例尺信息”
                        //最后更新数据库
                        string updateStr = "update DATABASEMD set SCALE='" + pScaleStr + "'and  CONNECTIONINFO='" + pConnDtStr+"'  where ID=" + Convert.ToInt32(pDBID);
                        try
                        {
                            ModuleData.TempWks.ExecuteSQL(updateStr);
                        } catch
                        {
                            ex = new Exception("更新比例尺信息失败或者连接信息和数据集名称信息！");
                            return;
                        }
                        //updateStr = "update DATABASEMD set CONNECTIONINFO='" + pConnDtStr + "' where ID=" + Convert.ToInt32(pDBID);
                        //try
                        //{
                        //    ModuleData.TempWks.ExecuteSQL(updateStr);
                        //} catch
                        //{
                        //    ex = new Exception("更新连接信息和数据集名称信息失败！");
                        //    return;
                        //}
                        #endregion
                        sDBFeatureName = GetDtNames;
                        iScale = pScaleStr;
                        //释放游标
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

                        //end
                    }
                    */
                #region 原有代码
                //string strConn = rootNode.Tag.ToString(); //系统维护库连接字符串

                //SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
                //pSysTable.SetDbConnection(strConn, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out ex);
                //if (ex != null)
                //{
                //    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库失败！");
                //    return;
                //}

                //插入比例尺信息

                //string updateStr = "update DATABASEMD set SCALE='" + iScale.ToString() + "' where ID=" + Convert.ToInt32(pDBID);
                //pSysTable.UpdateTable(updateStr, out ex);
                //if (ex != null)
                //{
                //    ex = new Exception("更新系统维护库比例尺信息失败！");
                //    return;
                //}
                ////插入数据集名称

                //string selStr = "select * from DATABASEMD where ID=" + Convert.ToInt32(pDBID);
                //DataTable pDT = pSysTable.GetSQLTable(selStr, out ex);
                //if (ex != null)
                //{
                //    ex = new Exception("查询系统维护库失败！");
                //    return;
                //}
                //if (pDT == null || pDT.Rows.Count == 0) return;
                //string pConnInfoFromDB = pDT.Rows[0]["CONNECTIONINFO"].ToString();
                //pConnInfoFromDB = pConnInfoFromDB.Substring(0, (pConnInfoFromDB.LastIndexOf('|') + 1));
                //pConnInfoFromDB = pConnInfoFromDB + sDBName;
                //updateStr = "update DATABASEMD set CONNECTIONINFO='" + pConnInfoFromDB + "' where ID=" + Convert.ToInt32(pDBID);
                //pSysTable.UpdateTable(updateStr, out ex);
                //if (ex != null)
                //{
                //    ex = new Exception("更新系统维护库数据集信息失败！");
                //    return;
                //}
                #endregion
                #endregion
                //end

                #region 将相关信息写入XML中  陈亚飞添加20100930
                /*------------------在后面统一刷新界面
                    //加载xml文件
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ModuleData.v_feaProjectXML);
                    XmlElement pElem = xmlDoc.SelectSingleNode(".//工程管理//工程[@编号='" + pDBID + "']") as XmlElement;
                    //保存比例尺属性
                    //cyf 20110622 modify
                    //pElem.SetAttribute("比例尺", iScale.ToString());
                    pElem.SetAttribute("比例尺", pScaleStr.ToString());
                    //end
                    //遍历库体节点，给库体数据集命名

                    foreach (XmlNode pNode in pElem.FirstChild.ChildNodes)
                    {

                        string isVisible = (pNode as XmlElement).GetAttribute("是否显示");
                        if (isVisible.ToLower() == "false") continue;
                        //获得库体数据集名称节点

                        XmlElement pDatasetNameElem = pNode.FirstChild.FirstChild as XmlElement;
                        //保存数据集名称属性

                        string pDbName = (pNode as XmlElement).GetAttribute("名称");
                        //cyf 20110622 modify
                        if (pDbName == "现势库")
                        {
                            pDatasetNameElem.SetAttribute("名称", GetDtNames);
                        }
                        else if (pDbName == "历史库")
                        {
                            //pDatasetNameElem.SetAttribute("名称", sDBName + "_GOH");
                        }
                        else if (pDbName == "工作库")
                        {
                            //pDatasetNameElem.SetAttribute("名称", sDBName + "_got");
                        }
                        //end
                    }
                    xmlDoc.Save(ModuleData.v_feaProjectXML);
                    */
                #endregion
            }
            else if (pDBTypeID == enumInterDBType.成果文件数据库.GetHashCode().ToString())
            { }
            else if (pDBTypeID == enumInterDBType.地理编码数据库.GetHashCode().ToString())
            { }
            else if (pDBTypeID == enumInterDBType.地名数据库.GetHashCode().ToString())
            { }
            else if (pDBTypeID == enumInterDBType.高程数据库.GetHashCode().ToString())
            { }
            else if (pDBTypeID == enumInterDBType.影像数据库.GetHashCode().ToString())
            {
                #region 创建影像栅格库


                #endregion
            }
        }
       
        // *---------------------------------------------------------------------------------------
        // *开 发 者：chenyafei  cyf 20110707 modify :修改保存多个数据集的功能
        // *开发时间：20110623
        // *功能函数：更新系统维护库信息.将比例尺信息、连接字符串信息（数据集信息）、数据库状态信息写入数据库
        // *参    数：当前树节点,当前数据集比例尺，当前数据集名称，所有数据集比例尺信息（比例尺之间以"，"隔开）（输出），所有数据名名称信息（名称之间以"，"隔开）（输出），异常（输出）
        // *返 回 值：无返回值
        public void UpdateDB(DevComponents.AdvTree.Node pCurNode, string iScale, List<string> sDBNameLst, out string pScaleStr, out string GetDtNames, out Exception ex)
        {
            ex = null;
            pScaleStr ="";  //所有的比例尺信息（以逗号隔开）
            GetDtNames = "";//所有的数据集信息（以逗号隔开）
            string pConnDtStr = "";              //当前数据库连接信息字段(包含数据集)
            string pDBID = "";           //当前数据源ID
           //cyf 20110707 add:遍历数据集名称，将数据及名称保存成字符串
            string sDBName = "";          //数据集名称字符串  
            foreach (string pName in sDBNameLst)
            {
                sDBName += pName + ",";
                pScaleStr += iScale + ",";
            }
            if (sDBName.Trim().Length>0)
            {
                sDBName = sDBName.Substring(0, sDBName.Length - 1);
            }
            if (pScaleStr.Trim().Length > 0)
            {
                pScaleStr = pScaleStr.Substring(0, pScaleStr.Length - 1);
            }
            //end
            
            //获取当前数据源ID
            if (pCurNode.DataKey == null)
            {
                ex = new Exception("获取数据库工程ID失败！");
                return;
            }
            pDBID = pCurNode.DataKey.ToString();  //当前数据库工程ID
            IFeatureWorkspace pFeaWS = ModuleData.TempWks as IFeatureWorkspace;
            if (pFeaWS == null)
            {
                ex = new Exception("获取系统维护库工作空间失败！");
                return;
            }
            //cyf 20110622 modify:存在多数据集时，将数据集名称已逗号隔开进行保存，比例尺以逗号隔开进行保存
            #region 更新比例尺信息和数据集信息
            //首先读取数据库中比例尺的信息

            string pTableNames = "DATABASEMD";
            string pFieldNames = "SCALE,CONNECTIONINFO";
            string pWhereStr = "ID=" + Convert.ToInt32(pDBID);
            ICursor pCursor = null;
            pCursor = ModDBOperate.GetCursor(pFeaWS, pTableNames, pFieldNames, pWhereStr, out ex);
            if (pCursor == null || ex != null)
            {
                ex = new Exception("查询系统维护库中比例尺信息失败！");
                return;
            }
            IRow pRow = pCursor.NextRow();   //查询到的行
            string pConnInfoFromDB = "";//查询到的连接信息
            string getScale = "";       //查询到的比例尺信息
            if (pRow != null)
            {
                getScale = pRow.get_Value(0).ToString();
                pConnInfoFromDB = pRow.get_Value(1).ToString();
            }
            if (getScale.Trim() != ""&&getScale.Trim()!="0")
            {
                pScaleStr = getScale + "," + pScaleStr;
            }
            if (pConnInfoFromDB.Trim() != "")
            {
                string pConnStr = pConnInfoFromDB.Substring(0, (pConnInfoFromDB.LastIndexOf('|') + 1));//连接信息不带数据集的
                string getDtName = pConnInfoFromDB.Substring(pConnInfoFromDB.LastIndexOf('|') + 1).Trim();//已经存储的数据集名称
                if (getDtName.Length == 0)
                {
                    pConnDtStr = pConnStr + sDBName;
                    GetDtNames = sDBName;
                }
                else
                {
                    pConnDtStr = pConnInfoFromDB + "," + sDBName;
                    GetDtNames = getDtName + "," + sDBName;
                }
            }
            //其次，将读取出来的比例尺的信息+“，”+“当前比例尺信息”
            //最后更新数据库
            string updateStr = "update DATABASEMD set SCALE='" + pScaleStr + "',CONNECTIONINFO='" + pConnDtStr + "'  where ID=" + Convert.ToInt32(pDBID);
            try
            {
                ModuleData.TempWks.ExecuteSQL(updateStr);
            }
            catch
            {
                ex = new Exception("更新比例尺信息失败或者连接信息和数据集名称信息！");
                return;
            }
            #endregion

            #region 更新数据库状态信息
            pTableNames = "DATABASEMD";
            pFieldNames = "*";
            pWhereStr = "DATAFORMATID=" + EnumDBState.库体未初始化.GetHashCode() + " and ID=" + Convert.ToInt32(pDBID);
            pCursor =ModDBOperate.GetCursor(pFeaWS, pTableNames, pFieldNames, pWhereStr, out ex);
            if (pCursor == null || ex != null)
            {
                ex = new Exception("查询系统维护库中数据库状态信息失败！");
                return;
            }
            IRow pRow2 = pCursor.NextRow();   //查询到的行
            if (pRow2 != null)
            {
                //更新数据库状态信息
                updateStr = "update DATABASEMD set DATABASSTATEID=" + EnumDBState.库体已初始化.GetHashCode() + " where ID=" + Convert.ToInt32(pDBID);
                try
                {
                    ModuleData.TempWks.ExecuteSQL(updateStr);
                }
                catch
                {
                    ex = new Exception("更新数据库状态信息失败！");
                    return;
                }
            }
            #endregion
            //释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        }
        /// <summary>
        /// 加载空间参考文件获取空间参考
        /// </summary>
        /// <param name="LoadPath">空间参考文件路径</param>
        /// <returns></returns>
        public ISpatialReference LoadSpatialReference(string LoadPath)
        {
            try
            {
                ISpatialReference pSR = null;
                ISpatialReferenceFactory pSpatialRefFac = new SpatialReferenceEnvironmentClass();

                if (!File.Exists(LoadPath))
                {
                    //m_SpatialReference = null;
                    return null;
                }
                pSR = pSpatialRefFac.CreateESRISpatialReferenceFromPRJFile(LoadPath);

                ISpatialReferenceResolution pSRR = pSR as ISpatialReferenceResolution;
                ISpatialReferenceTolerance pSRT = (ISpatialReferenceTolerance)pSR;
                IControlPrecision2 pSpatialPrecision = (IControlPrecision2)pSR;

                pSRR.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon
                pSRR.SetDefaultXYResolution();
                pSRT.SetDefaultXYTolerance();

                              //赋值空间参考

                return pSR;
            }
            catch
            {
                //m_SpatialReference = null;
                return null;
            }
        }

        /// <summary>
        /// ArcGis创建库体
        /// </summary>
        /// <param name="enumDBFormat">ArcGis数据库类型 cyf 20110628</param>
        /// <param name="sServer">服务器</param>
        /// <param name="sDataBase">数据库</param>
        /// <param name="sUser">用户</param>
        /// <param name="sPassWord">密码</param>
        /// <param name="sProj">投影文件路径</param>
        /// <param name="sDBShcame">数据库配置文件</param>
        /// <param name="sInstance">SDE实例号</param>
        /// <param name="sVersion">版本号</param>
        /// <param name="ex">错误输出</param>
        public void ArcGisDBCreateNew(string pDBDtypeID, string sServer, string sDataBase, string sUser, string sPassWord, string sProj, string sDBShcame, string sInstance, string sVersion, out string iScale, out List<string> DSName, out Exception ex)
        {
            ex = null;
            //cyf 20110622 modify
            //iScale = -1;
            iScale = "";
            //end
            //m_DBName = string.Empty;  //cyf 20110707 modify
            DSName = new List<string>();//cyf 20110707 modify
            ///////////////////////////初始化数据库类，读取配置方案//////////////
           SysCommon.Gis.ICreateGeoDatabase pCreateGeoDatabase = new SysCommon.Gis.CreateArcGISGeoDatabase();
          //if (!pCreateGeoDatabase.LoadDBShecmaDocument(sDBShcame))
          //      {
          //          ex = new Exception("读取库体配置文件失败,请检查配置文件是否正确！");
          //          pCreateGeoDatabase = null;
          //          return;
          //     }

            //1、判断是否是pdb  ？？？？
           IWorkspace pSchemaW = null;
           try
           {
              pSchemaW = new AccessWorkspaceFactoryClass().OpenFromFile(sDBShcame, 0);
           }
           catch
           {
               ex = new Exception("PDB数据库无效！"); return;
           }
           if (pSchemaW == null)
           {
               ex = new Exception("PDB数据库无效！"); return;
 
           }
            ISpatialReference pSR=LoadSpatialReference(sProj);//yjl20110813
            //判断空间参考文件
            if (pSR==null)
            {
                ex = new Exception("获取空间参考失败");
                pCreateGeoDatabase = null;
                return;
            }
            string sDBtype = string.Empty;
            IWorkspace pToW = null;//yjl20110813
            #region 判断类型，设置数据库连接
            //////////判断类型，设置数据库连接/////////////
            if (pDBDtypeID == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())  //enumInterDBFormat.ARCGISGDB:
            {
                if (string.IsNullOrEmpty(sDataBase)) { ex = new Exception("GDB数据库路径未指定"); return; }
                pToW=SetDestinationProp("GDB", sDataBase, "", "", "", "");
                //pCreateGeoDatabase.SetDestinationProp("GDB", sDataBase, "", "", "", "");
                sDBtype = "GDB";
            }
            else if (pDBDtypeID == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(sDataBase)) { ex = new Exception("PDB数据库路径未指定"); return; }
                pToW = SetDestinationProp("PDB", sDataBase, "", "", "", "");
                //pCreateGeoDatabase.SetDestinationProp("PDB", sDataBase, "", "", "", "");
                sDBtype = "PDB";
            }
            else if (pDBDtypeID == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
            {
                //全部屏蔽
                try
                {
                    pToW = SetDestinationProp("SDE", sServer, sInstance, sUser, sPassWord, sVersion);
                    //pCreateGeoDatabase.SetDestinationProp("SDE", sServer, sInstance, sUser, sPassWord, sVersion);
                }
                catch (Exception eError)
                {
                    //****************************************************
                    if (ModuleData.v_SysLog != null)
                        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                    //****************************************************
                    ex = new Exception("SDE数据库连接信息设置失败");
                    return;
                }
                sDBtype = "SDE";
            }

            #endregion
            /////////////////////////////创建库体////////////////////////////////
            //List<string> DSName = new List<string>();  //cyf 20110707 modify
       /*     if (!pCreateGeoDatabase.CreateDBStruct(out iScale, out DSName, this.m_ProcBar))  //cyf 20110707 modify
            {
                ex = new Exception("初始化失败或者库体已初始化");
                pCreateGeoDatabase = null;
                return;
            }
        * */
            if (pToW == null)
            {
                ex = new Exception("获取空间参考失败");
                pCreateGeoDatabase = null;
                return;
 
            }
            IWorkspace2 pw2 = pToW as IWorkspace2;
            //2、pdb属性结构组织方式到目标库中？？？？ 目标库工作空间  yjl20110813
            IEnumDataset srcED = pSchemaW.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            IDataset srcD = srcED.Next();
            while (srcD != null)
            {
                if (pw2.get_NameExists(esriDatasetType.esriDTFeatureDataset, srcD.Name))
                {
                    ex = new Exception("该数据库已经初始化!");
                    return;
                }
                DSName.Add(srcD.Name);//20110820 修改 xisheng 
                IGeoDataset srcGD = srcD as IGeoDataset;
                IGeoDatasetSchemaEdit srcGS = srcGD as IGeoDatasetSchemaEdit;
                if (srcGS.CanAlterSpatialReference)
                    srcGS.AlterSpatialReference(pSR);
                CopyPasteGDBData.CopyPasteGeodatabaseData(pSchemaW, pToW, srcD.Name, esriDatasetType.esriDTFeatureDataset);
                srcD = srcED.Next();
 
            }
           
            



            //  string pDSName = DSName[0];
            /////////////////////////////创建远程日志表//////////////////////////
            //if (!pCreateGeoDatabase.CreateSQLTable(sDBtype, out ex))
            //{

            //    ex = new Exception("远程日志表创建失败");
            //    pCreateGeoDatabase = null;
            //    return;
            //}
            //pCreateGeoDatabase = null;
        }

        /// <summary>
        /// 设置连接属性
        /// </summary>
        /// <param name="Type">数据库类型</param>
        /// <param name="IPoPath">数据库访问路径或服务器IP</param>
        /// <param name="Intance">sde服务实例</param>
        /// <param name="User">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="Version">sde版本</param>
        /// <returns></returns>
        public IWorkspace SetDestinationProp(string Type, string IPoPath, string Intance, string User, string PassWord, string Version)
        {
            IWorkspace TempWorkSpace = null;                                 //工作空间
            IWorkspaceFactory pWorkspaceFactory = null;                      //工作空间工厂

            try
            {
                //初始化工作空间工厂
                if (Type == "PDB")
                {
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                }
                else if (Type == "GDB")
                {
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                }
                else if (Type == "SDE")
                {
                    pWorkspaceFactory = new SdeWorkspaceFactoryClass();
                }
                //cyf  20110622 delete:不删除原来的数据库，在原有的基础进行追加
                ///如果创建的是本地库体，则首先判断库体是否存在
                ///如果库体存在，则先删除原有库体
                //if (File.Exists(IPoPath))
                //{
                //if (!SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "是否库体中创建数据集"))
                //{
                //    File.Delete(IPoPath);
                //}
                //}
                //end

                if (Type == "SDE")  //如果是SDE则设置sde工作空间连接信息
                {
                    IPropertySet propertySet = new PropertySetClass();
                    propertySet.SetProperty("SERVER", IPoPath);
                    propertySet.SetProperty("INSTANCE", Intance);
                    //propertySet.SetProperty("DATABASE", ""); 
                    propertySet.SetProperty("USER", User);
                    propertySet.SetProperty("PASSWORD", PassWord);
                    propertySet.SetProperty("VERSION", Version);
                    TempWorkSpace = pWorkspaceFactory.Open(propertySet, 0);
                }
                else  //如果不是sde则创建工作空间
                {
                    FileInfo finfo = new FileInfo(IPoPath);
                    string outputDBPath = finfo.DirectoryName;
                    string outputDBName = finfo.Name;
                    if (outputDBName.EndsWith(".gdb"))
                    {
                        outputDBName = outputDBName.Substring(0, outputDBName.Length - 4);
                    }
                    //cyf 20110622 add:打开已有的工作空间
                    try { TempWorkSpace = pWorkspaceFactory.OpenFromFile(IPoPath, 0); }
                    catch { }
                    //end
                    if (TempWorkSpace == null)
                    {
                        IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(outputDBPath, outputDBName, null, 0);
                        ESRI.ArcGIS.esriSystem.IName pName = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;
                        TempWorkSpace = (IWorkspace)pName.Open();
                    }
                }

                //判断获取工作空间是否成功
                if (TempWorkSpace != null)
                {
                                   //工作空间赋值
                    return TempWorkSpace;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //***********************
                //guozheng 2010-12-17 added
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败！\n原因：" + ex.Message);
                //***********************
                return null;
            }

        }

        /// <summary>
        /// ArcGis创建库体
        /// </summary>
        /// <param name="enumDBFormat">ArcGis数据库类型 cyf 20110628</param>
        /// <param name="sServer">服务器</param>
        /// <param name="sDataBase">数据库</param>
        /// <param name="sUser">用户</param>
        /// <param name="sPassWord">密码</param>
        /// <param name="sProj">投影文件路径</param>
        /// <param name="sDBShcame">数据库配置文件</param>
        /// <param name="sInstance">SDE实例号</param>
        /// <param name="sVersion">版本号</param>
        /// <param name="ex">错误输出</param>
        public void ArcGisDBCreate(string pDBDtypeID, string sServer, string sDataBase, string sUser, string sPassWord, string sProj, string sDBShcame, string sInstance, string sVersion, out string iScale,out List<string> DSName, out Exception ex)
        {
            ex = null;
            //cyf 20110622 modify
            //iScale = -1;
            iScale = "";
            //end
            //m_DBName = string.Empty;  //cyf 20110707 modify
            DSName = new List<string>();//cyf 20110707 modify
            ///////////////////////////初始化数据库类，读取配置方案//////////////
            SysCommon.Gis.ICreateGeoDatabase pCreateGeoDatabase = new SysCommon.Gis.CreateArcGISGeoDatabase();
            if (!pCreateGeoDatabase.LoadDBShecmaDocument(sDBShcame))
            {
                ex = new Exception("读取库体配置文件失败,请检查配置文件是否正确！");
                pCreateGeoDatabase = null;
                return;
            }
            if (!pCreateGeoDatabase.LoadSpatialReference(sProj))
            {
                ex = new Exception("获取空间参考失败");
                pCreateGeoDatabase = null;
                return;
            }
            string sDBtype = string.Empty;
            #region 判断类型，设置数据库连接
            //////////判断类型，设置数据库连接/////////////
            if (pDBDtypeID == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())  //enumInterDBFormat.ARCGISGDB:
            {
                if (string.IsNullOrEmpty(sDataBase)) { ex = new Exception("GDB数据库路径未指定"); return; }
                pCreateGeoDatabase.SetDestinationProp("GDB", sDataBase, "", "", "", "");
                sDBtype = "GDB";
            }
            else if (pDBDtypeID == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(sDataBase)) { ex = new Exception("PDB数据库路径未指定"); return; }
                pCreateGeoDatabase.SetDestinationProp("PDB", sDataBase, "", "", "", "");
                sDBtype = "PDB";
            }
            else if (pDBDtypeID == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
            {
                try
                {
                    pCreateGeoDatabase.SetDestinationProp("SDE", sServer, sInstance, sUser, sPassWord, sVersion);
                }
                catch (Exception eError)
                {
                    //****************************************************
                    if (ModuleData.v_SysLog != null)
                        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                    //****************************************************
                    ex = new Exception("SDE数据库连接信息设置失败");
                    return;
                }
                sDBtype = "SDE";
            }

            #endregion
            /////////////////////////////创建库体////////////////////////////////
            //List<string> DSName = new List<string>();  //cyf 20110707 modify
            if (!pCreateGeoDatabase.CreateDBStruct(out iScale,out DSName, this.m_ProcBar))  //cyf 20110707 modify
            {
                ex = new Exception("初始化失败或者库体已初始化");
                pCreateGeoDatabase = null;
                return;
            }
            //  string pDSName = DSName[0];
            /////////////////////////////创建远程日志表//////////////////////////
            if (!pCreateGeoDatabase.CreateSQLTable(sDBtype, out ex))
            {

                ex = new Exception("远程日志表创建失败");
                pCreateGeoDatabase = null;
                return;
            }
            pCreateGeoDatabase = null;
        }
        /// <summary>
        /// OracleSpatial数据库库体创建

        /// </summary>
        /// <param name="Sserver">服务名</param>
        /// <param name="sUser">用户</param>
        /// <param name="sPassword">密码</param>
        /// <param name="sProj">投影文件</param>
        /// <param name="sDBSchame">数据库配置文件路径</param>
        /// <param name="ex">错误返回</param>
        public void OracleSpatialDBCreate(string Sserver, string sUser, string sPassword, string sProj, string sDBSchame, out int iScale, out string m_DBName, out Exception ex)
        {
            ex = null;
            iScale = -1;
            m_DBName = string.Empty;
            ClsOracleSpatialDBCreater OraclSpatialDBCreater = new ClsOracleSpatialDBCreater(Sserver, sUser, sPassword);
            OraclSpatialDBCreater.CreateDataBase(sDBSchame, false, out ex);
            if (ex != null) { ex = new Exception("库体初始化失败，\n原因:" + ex.Message); return; }
            //OraclSpatialDBCreater.CreateDataBase(sDBSchame, true, out ex);
            //if (ex != null) { ex = new Exception("现势库初始化成功，历史库库体初始化失败，\n原因:" + ex.Message); return; }
            iScale = GetDBSchameScale(sDBSchame, out m_DBName, out ex);
            if (ex != null) { ex = new Exception("获取比例尺信息失败！"); return; }

        }
        /// <summary>
        /// GeoStar数据库库体创建

        /// </summary>
        /// <param name="enumDBFormat">数据库平台</param>
        /// <param name="sServer">服务名</param>
        /// <param name="sDataBase">数据库</param>
        /// <param name="sUser">用户</param>
        /// <param name="sPassWord">密码</param>
        /// <param name="sProj">投影文件</param>
        /// <param name="sDBSchame">数据库配置文件</param>
        /// <param name="ex">错误返回</param>
        //public void GeostarDBCreate(enumInterDBFormat enumDBFormat, string sServer, string sDataBase, string sUser, string sPassWord, string sProj, string sDBSchame, out int iScale,out string m_DBName, out Exception ex)
        //{
        //ex = null;
        //iScale = -1;
        //m_DBName=string.Empty;
        //ClsCreateGeostarStruct createGeostarStruct = null;
        //GeoStarCore.IConnectPropertiesEx pConnex = null;
        /////////////加载投影文件和数据库配置方案文件/////////////
        //#region 加载投影文件和数据库配置方案文件
        //try
        //{
        //    createGeostarStruct = new ClsCreateGeostarStruct();
        //    pConnex = new GeoStarCore.ConnectProperties() as GeoStarCore.IConnectPropertiesEx;
        //    createGeostarStruct.put_ESRISpatialRefenceString(sProj);
        //}
        //catch
        //{
        //    ex = new Exception("读取投影文件失败");
        //    return;
        //}
        //try
        //{
        //    createGeostarStruct.put_GeoOneSchemaPath(sDBSchame);
        //}
        //catch
        //{
        //    ex = new Exception("读取数据库配置方案失败");
        //    return;
        //}
        //#endregion
        //string mDbType = string.Empty;
        //#region 根据数据库平台设置连接信息

        //switch (enumDBFormat)
        //{
        //    case enumInterDBFormat.GEOSTARACCESS:
        //        pConnex.SourceInfo = GeoStarCore.geoDataSource.GEO_ACCESS;
        //        pConnex.Database = sDataBase;
        //        pConnex.User = sUser;
        //        pConnex.Password = sPassWord;
        //        createGeostarStruct.put_ConnectDBInfo(pConnex);
        //        mDbType = "ACCESS";
        //        break;
        //    case enumInterDBFormat.GEOSTARORACLE:
        //        pConnex.SourceInfo = GeoStarCore.geoDataSource.GEO_ORACLE_SPATIAL;
        //        pConnex.Database = sDataBase;
        //        pConnex.User = sUser;
        //        pConnex.Password = sPassWord;
        //        createGeostarStruct.put_ConnectDBInfo(pConnex);
        //        mDbType = "ORACLE";
        //        break;
        //    case enumInterDBFormat.GEOSTARORSQLSERVER:
        //        pConnex.SourceInfo = GeoStarCore.geoDataSource.GEO_SQLSERVER;
        //        pConnex.Database = sDataBase;
        //        pConnex.Server = sServer;
        //        pConnex.User = sUser;
        //        pConnex.Password = sPassWord;
        //        createGeostarStruct.put_ConnectDBInfo(pConnex);
        //        mDbType = "SQL SERVER";
        //        break;
        //    default:
        //        ex = new Exception("不支持的GeoStar数据库平台类型");
        //        break;
        //}
        //#endregion
        ///////创建库体/////////////
        //List<string> DSName = new List<string>();
        //int hr = createGeostarStruct.createDBStruct(DSName);
        //if (hr == -1) { ex = new Exception("库体创建失败！"); return; }
        ///////创建远程日志表///////
        //if (!createGeostarStruct.CreateSQLTable(mDbType, out ex))
        //{
        //    ex = new Exception("创建远程日志表失败！原因：" + ex.Message);
        //    return;
        //}
        //iScale = GetDBSchameScale(sDBSchame,out m_DBName, out ex);
        //if (ex != null) { ex = new Exception("获取比例尺信息失败！"); return; }
        //}
        /// <summary>
        /// 根据配置方案获取数据库比例尺
        /// </summary>
        /// <param name="sDBSchame">数据库配置方案路径</param>
        /// <param name="ex">错误返回</param>
        /// <returns></returns>
        public int GetDBSchameScale(string sDBSchame, out string m_DBName, out Exception ex)
        {
            ex = null;
            int m_DBScale = -1;/////比例尺信息

            m_DBName = string.Empty;
            try
            {
                ISchemeProject m_pProject = new SchemeProjectClass();     //创建实例
                int index = sDBSchame.LastIndexOf('.');
                if (index == -1) { ex = new Exception("数据库配置方案文件格式不正确"); return -1; }
                string lastName = sDBSchame.Substring(index + 1);
                if (lastName == "mdb")
                {
                    m_pProject.Load(sDBSchame, e_FileType.GO_SCHEMEFILETYPE_MDB);    //加载schema文件
                }
                else if (lastName == "gosch")
                {
                    m_pProject.Load(sDBSchame, e_FileType.GO_SCHEMEFILETYPE_GOSCH);    //加载schema文件
                }
                if (m_pProject != null)
                {
                    string DBScale = m_pProject.get_MetaDataValue("Scale") as string;   //获取比例尺信息（总工程中的默认比例尺）

                    string[] DBScaleArayy = DBScale.Split(':');
                    m_DBScale = Convert.ToInt32(DBScaleArayy[1]);
                    IChildItemList pProjects = m_pProject as IChildItemList;/////获取属性库信息
                    ISchemeItem pDBList = pProjects.get_ItemByName("ATTRDB");
                    IChildItemList pDBLists = pDBList as IChildItemList;
                    long DBNum = pDBLists.GetCount();
                    for (int i = 0; i < DBNum; i++)
                    {
                        ISchemeItem pDB = pDBLists.get_ItemByIndex(i);
                        IChildItemList pDBs = pDB as IChildItemList;
                        m_DBName = pDB.Name;
                    }
                }
                else
                {
                    ex = new Exception("加载数据库配置方案文件：" + sDBSchame + "失败！");
                    return -1;
                }
                return m_DBScale;
            }
            catch (Exception eError)
            {
                //****************************************************
                if (ModuleData.v_SysLog != null)
                    ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                //****************************************************
                ex = new Exception("加载数据库配置方案文件：" + sDBSchame + "失败！");
                return -1;
            }

        }
        /// <summary>
        /// cyf 20110602 modify:系统维护库中卸载一个数据库
        /// </summary>
        /// <param name="lDBid">数据库ID</param>
        /// <param name="ex">返回：错误信息</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool DelDB(long lDBid, out Exception ex)
        {
            ex = null;
            #region 原有代码
            //if (this.m_DataBaseOper == null)
            //{
            //    ex = new Exception("系统维护库连接信息未初始化");
            //    return false;
            //}
           
            //===============================================
            //chenyafei  20110316 add content: 开启事物
            //this.m_DataBaseOper.StartTransaction();
            ////chenyafei  20110316  add  content :删除状态表中对应饿改工程的记录
            //string sql = "delete from updateinfo where PROJECTID=" + lDBid.ToString();
            //this.m_DataBaseOper.UpdateTable(sql, out ex);
            //if (ex != null)
            //{
            //    this.m_DataBaseOper.EndTransaction(false);
            //    this.m_DataBaseOper.CloseDbConnection();
            //    return false;
            //}
            ////===============================================
            //sql = "DELETE FROM DATABASEMD WHERE ID=" + lDBid.ToString();
            //this.m_DataBaseOper.UpdateTable(sql, out ex);
            //if (ex != null)
            //{
            //    this.m_DataBaseOper.EndTransaction(false);
            //    this.m_DataBaseOper.CloseDbConnection();
            //    return false;
            //}
            //this.m_DataBaseOper.EndTransaction(true);
            //this.m_DataBaseOper.CloseDbConnection();
            ////=============================================
            //return true;
            #endregion 
            //cyf 20110602 modify
            if (ModuleData.TempWks == null)
            {
                ex = new Exception("系统维护库连接信息未初始化");
                return false;
            }
          
            //删除状态表中对应工程的记录
            string sql = "delete from updateinfo where PROJECTID=" + lDBid.ToString();
            try
            {
                ModuleData.TempWks.ExecuteSQL(sql);
            } catch
            {
                ex = new Exception("删除信息失败！");
                return false;
            }
            //===============================================
            sql = "DELETE FROM DATABASEMD WHERE ID=" + lDBid.ToString();
            try
            {
                ModuleData.TempWks.ExecuteSQL(sql);
            } catch
            {
                ex = new Exception("删除信息失败！");
                return false;
            }
            //=============================================
            //cyf 20110620 add:删除栅格图幅索引日志表
            sql = "DELETE FROM RasterIndexTable WHERE ProID=" + lDBid.ToString();
            try
            {
                ModuleData.TempWks.ExecuteSQL(sql);
            }
            catch
            {
                ex = new Exception("删除栅格图幅索引日志表失败！");
                return false;
            }
            //===============================================
            //cyf 20110629 add:删除栅格FTP目录信息表
            sql = "DELETE FROM RasterCatalogLayerInfo WHERE ProjectID=" + lDBid.ToString();
            try
            {
                ModuleData.TempWks.ExecuteSQL(sql);
            }
            catch
            {
                ex = new Exception("删除栅格编目FTP目录表失败！");
                return false;
            }
            //end
            return true;
            //end
        }

        /// <summary>
        /// 更新相应的数据库记录的信息
        /// </summary>
        /// <param name="lDBid">数据库ID</param>
        /// <param name="in_sFieldName">字段名</param>
        /// <param name="in_sValue">字段值（需要进行SQL标准化，例如字符使用‘号包括）</param>
        public void UpDataDBInfo(long lDBid, string in_sFieldName, string in_sValue,out Exception ex)
        {
            ex = null;
            if (this.m_DataBaseOper == null)
            {
                ex = new Exception("系统维护库连接信息未初始化");
                return;
            }
            string sql = "UPDATE DATABASEMD SET " + in_sFieldName + " =" + in_sValue + " WHERE ID=" + lDBid.ToString() ;
            //this.m_DataBaseOper.StartTransaction();
            this.m_DataBaseOper.UpdateTable(sql, out ex);
            if (ex != null)
            {
              //  this.m_DataBaseOper.EndTransaction(false);
               // this.m_DataBaseOper.CloseDbConnection();
                return ;
            }
           
          //  this.m_DataBaseOper.EndTransaction(true);
           // this.m_DataBaseOper.CloseDbConnection();
        }
    }

  
}
