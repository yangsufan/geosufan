using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    /// <summary>
    /// 联动更新  chenyafei add
    /// </summary>
    public static class ClsUpdate
    {
       /// <summary>
       /// 获取选择的要素
       /// </summary>
       /// <param name="pMap">地图</param>
       /// <param name="pFeaType">要素类型：参照要素、目标要素</param>
       /// <param name="pError">异常</param>
       /// <returns>返回选中的要素</returns>
        public static IFeature getFea(IMap pMap,EnumFeatureType pFeaType,out Exception pError)
        {
            pError = null;      //异常
            IFeature pFea=null; //用来保存返回的要素
            //必须要选择要素
            if (pMap.SelectionCount == 0)
            {
                pError = new Exception("请选择一个需要更新的要素");
                return null;
            }
            if (pMap.SelectionCount> 1)
            {
                pError = new Exception("请只选择一个参照要素！");
                return null;
            }
            //遍历控件上的所有图层
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                //ILayer pLayer = pMap.get_Layer(i);
                //if (pLayer is IGroupLayer)
                //{ 
                //}
                //获得图层
                IFeatureLayer pFeaLayer = pMap.get_Layer(i) as IFeatureLayer; 
                if (pFeaLayer == null) return null;
                //获得图层的选择集
                IFeatureSelection pFeaSelection = pFeaLayer as IFeatureSelection; 
                if (pFeaSelection == null) continue;
                ISelectionSet pSelectionSet = pFeaSelection.SelectionSet;
                if (pSelectionSet == null) continue;
                if (pSelectionSet.Count == 0) continue;
                IEnumIDs pEnumIDs = pSelectionSet.IDs;
                pEnumIDs.Reset();
                int pOID = pEnumIDs.Next();
                
                if (pOID != -1)
                {
                    //若存在选择集

                    //图层对应的要素类
                    IFeatureClass pFeaCls = pFeaLayer.FeatureClass;
                    if (pFeaCls == null) return null;
                    //获得选择的要素
                    pFea = pFeaCls.GetFeature(pOID);
                    if (pFea == null)
                    {
                        if (pFeaType == EnumFeatureType.参照要素)
                        {
                            pError = new Exception("获取参照要素失败！");
                            return null ;
                        }
                        else if (pFeaType == EnumFeatureType.更新要素)
                        {
                            pError = new Exception("获取更新要素失败！");
                            return null;
                        }
                    }
                }
                break;
            }
            return pFea;
        }


        /// <summary>
        /// 联动更新函数
        /// </summary>
        /// <param name="orgFea">参照要素</param>
        /// <param name="objFea">目标更新要素</param>
        /// <param name="pObjFeaClss">目标更新要素类</param>
        /// <param name="pObjWS">目标工作空间</param>
        /// <param name="fromDate">目标工作空间</param>
        /// <param name="pUpdateType">更新类型，新增、修改、删除</param>
        /// <param name="pError">异常</param>
        public static void UpdateFea(IMap pMap,IFeature orgFea, IFeature objFea,IFeatureClass pObjFeaClss,IWorkspace pObjWS,EnumUpdateType pUpdateType, out Exception pError)
        {
            pError = null;        //异常
            string pFeaClsName = "";//更新要素类 名称
            int pVersion=-1;          //版本信息
             pFeaClsName=(pObjFeaClss as IDataset).Name;//目标图层名

             IWorkspaceEdit pWsEdit = pObjWS as IWorkspaceEdit;//目标编辑工作空间
             if (pObjWS.WorkspaceFactory.WorkspaceType == esriWorkspaceType.esriRemoteDatabaseWorkspace)
             {
                 //工作空间是SDE类型

                 //开启编辑
                 if (!pWsEdit.IsBeingEdited())
                 {
                     try
                     {
                         pWsEdit.StartEditing(true);  //开启编辑
                         pWsEdit.EnableUndoRedo();
                         pWsEdit.StartEditOperation();//开启会话
                     }
                     catch(Exception e)
                     {
                         //*******************************************************************
                         //Exception Log
                         if (ModData.SysLog == null)
                         {
                             ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                         }
                         ModData.SysLog.Write(e, null, DateTime.Now);
                         //********************************************************************
                         pError = new Exception("不能编辑该工作空间,请检查是否为只读或被其它用户占用！");
                         return;
                     }
                 }

                 //IFeatureDataset pFeaDataset = pObjFeaClss.FeatureDataset;
                 //if (pFeaDataset != null)
                 //{
                 //    IVersionedObject pVerObj = pFeaDataset as IVersionedObject;
                 //    if (pVerObj.IsRegisteredAsVersioned)
                 //    {
                 //        //反注册
                 //        pVerObj.RegisterAsVersioned(false);
                 //    }
                 //}

                 //获得版本信息
                 pVersion = GetVersion(pObjWS, out pError);
                 if (pError != null)
                 {
                     return;
                 }
             }
            DateTime fromDate = DateTime.Now;  //更新时间
            if (pUpdateType == EnumUpdateType.新增)
            {
                #region 新增操作
                if (orgFea == null)
                {
                    pError = new Exception("请选择参照要素");
                    return;
                }
                //在目标图层中创建一个新的要素
                IFeature newFea = pObjFeaClss.CreateFeature();  

                //给创建的要素的几何字段赋值
                newFea.Shape = orgFea.ShapeCopy;
                //给创建的要素的属性字段赋值

                try
                {
                    //保存要素
                    newFea.Store();
                }
                catch (Exception pEx)
                {
                    //*******************************************************************
                    //Exception Log
                    if (ModData.SysLog == null)
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    }
                    ModData.SysLog.Write(pEx, null, DateTime.Now);                    
                    //********************************************************************
                    pError = new Exception("新增要素出错，\n原因:" + pEx.Message);
                    return;
                }
                if (pObjWS.WorkspaceFactory.WorkspaceType == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    //停止编辑
                    pWsEdit.StartEditOperation();
                    pWsEdit.StopEditing(true);

                    //更新历史、日志记录表
                    RecordLOG(pObjWS, newFea as IRow, 1, fromDate, pVersion, pFeaClsName, out pError);
                    if (pError != null) return;
                }
                #endregion
            }
            else if (pUpdateType == EnumUpdateType.修改)
            {
                #region 修改操作
                if (orgFea == null)
                {
                    pError = new Exception("请选择参照要素");
                    return;
                }
                if (objFea == null)
                {
                    pError = new Exception("请选择更新要素");
                    return;
                }
                //***********************************************************************
                //guozheng added 
                if (objFea.Shape.GeometryType != orgFea.Shape.GeometryType)
                {
                    pError = new Exception("源要素与目标要素几何不一致，未能修改。");
                    return;
                }
                //***********************************************************************
                //给几何字段赋值
                objFea.Shape = orgFea.ShapeCopy;
                //给属性字段赋值,暂时未能实现

                try
                {
                    //对修改的要素进行存储
                    objFea.Store();
                  
                }
                catch (Exception pEx)
                {
                    //*******************************************************************
                    //Exception Log
                    if (ModData.SysLog == null)
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    }
                    ModData.SysLog.Write(pEx, null, DateTime.Now);
                    //********************************************************************
                    pError = new Exception("修改要素出错，\n原因:" + pEx.Message);
                    return;
                }


                if (pObjWS.WorkspaceFactory.WorkspaceType == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    pWsEdit.StopEditOperation();
                    pWsEdit.StopEditing(true);


                    //更新历史、日志记录表
                    RecordLOG(pObjWS, objFea as IRow, 2, fromDate, pVersion, pFeaClsName, out pError);
                    if (pError != null) return;
                }
                #endregion
            }
            else
            {
                #region 删除操作
                if (objFea == null)
                {
                    pError = new Exception("请选择要删除的要素");
                    return;
                }
                //删除要素

                //查找要删除的要素
                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = "OBJECTID=" + objFea.OID;
                ITable pTable = pObjFeaClss as ITable;
                try
                {
                    //删除要素
                    pTable.DeleteSearchedRows(pFilter);

                }
                catch (Exception pEx)
                {
                    //*******************************************************************
                    //Exception Log
                    if (ModData.SysLog == null)
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    }
                    ModData.SysLog.Write(pEx, null, DateTime.Now);
                    //********************************************************************
                    pError = new Exception("删除要素出错，\n原因:" + pEx.Message);
                    return;
                }

                if (pObjWS.WorkspaceFactory.WorkspaceType == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    pWsEdit.StopEditOperation();
                    pWsEdit.StopEditing(true);


                    //更新历史库和日志记录表
                    RecordLOG(pObjWS, objFea.OID, fromDate, pVersion, pFeaClsName, out pError);
                    if (pError != null) return;
                }
                #endregion
            }


            //更新版本信息

            if (pObjWS.WorkspaceFactory.WorkspaceType == esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                //更新版本信息
                WriteDBVersion(pObjWS, pVersion, fromDate, out pError);
            }

            if (pError != null) return;
           
        }


        /// <summary>
        /// 记录一个要素的编辑信息  guozheng added
        /// </summary>
        /// <param name="in_Row">编辑要素的IRow</param>
        /// <param name="in_iState">编辑状态：1、新增，2、修改，3、删除</param>
        /// <param name="in_DateTime">编辑的时间</param>
        /// <param name="in_iVersion">版本</param>
        /// <param name="in_sLayerName">要素所在的图层名</param>
        /// <param name="ex">异常</param>
        private static void RecordLOG(IWorkspace pSDEWS, IRow in_Row, int in_iState, DateTime in_DateTime, int in_iVersion, string in_sLayerName, out Exception ex)
        {
            ex = null;
            if (in_Row == null) { ex = new Exception("输入要素为空"); return; }
            if (in_DateTime == null) { ex = new Exception("输入的编辑时间为空"); return; }
            IFeature getFea = in_Row as IFeature;
            if (getFea == null) { ex = new Exception("获取要素失败"); return; }
            ///////////////获取必要信息/////////////////
            //////去掉sde图层带的用户名
            if (in_sLayerName.Contains("."))
            {
                in_sLayerName = in_sLayerName.Substring(in_sLayerName.LastIndexOf('.') + 1);
            }
            int iOID = -1;/////////////////////////////要素OID
            string sLayerName = string.Empty;//////////要素图层名
            int iVersion = -1;/////////////////////////版本信息 
            if (getFea.HasOID) iOID = getFea.OID;
            sLayerName = in_sLayerName;
            iVersion = in_iVersion;/////////////获取版本
            if (ex != null) return;
            //////////////////写入日志////////////////////////////
            try
            {
                ////////写入更新日志表///////
                WriteLog(pSDEWS, iOID, sLayerName, iVersion, in_DateTime, in_iState, getFea.Shape.Envelope, out ex);
                if (ex != null) { return; }
                ////////写入数据库版本表/////
                //WriteDBVersion(iVersion, in_DateTime, this.m_Con, out ex);
                //if (ex != null) { myTrans.Rollback(); }
                ///////修改过程库////////////
                WriteHisDB(sLayerName, pSDEWS, getFea, in_DateTime, in_iState, iVersion, out ex);
                if (ex != null) { return; }
            }
            catch (Exception eError)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog == null)
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                }
                ModData.SysLog.Write(eError, null, DateTime.Now);
                //********************************************************************
                ex = new Exception("编辑要素记录失败。\n原因：" + eError.Message);
                return;
            }
        }


        /// <summary>
        /// 记录一个要素的编辑信息(针对删除)   guozheng added
        /// </summary>
        /// <param name="in_OID">删除要素的OID</param>
        /// <param name="in_DateTime">编辑的时间</param>
        /// <param name="in_iVersion">版本</param>
        /// <param name="in_sLayerName">要素所在的图层名</param>
        /// <param name="ex"></param>
        private static void RecordLOG(IWorkspace pSDEWS, int in_OID, DateTime in_DateTime, int in_iVersion, string in_sLayerName, out Exception ex)
        {
            ex = null;

            if (in_DateTime == null) { ex = new Exception("输入的编辑时间为空"); return; }

            ///////////////获取必要信息/////////////////
            //////去掉sde图层带的用户名
            if (in_sLayerName.Contains("."))
            {
                in_sLayerName = in_sLayerName.Substring(in_sLayerName.LastIndexOf('.') + 1);
            }
            int iOID = in_OID;/////////////////////////////要素OID
            string sLayerName = string.Empty;//////////要素图层名
            int iVersion = -1;/////////////////////////版本信息 
            sLayerName = in_sLayerName;
            iVersion = in_iVersion;/////////////获取版本
            if (ex != null) return;
            //////////////////写入日志////////////////////////////
            try
            {
                ITransactions LOGTran = pSDEWS as ITransactions;
                LOGTran.StartTransaction();
                ////////写入更新日志表///////
                WriteLog(pSDEWS, iOID, sLayerName, iVersion, in_DateTime, 3, null, out ex);
                if (ex != null) { LOGTran.AbortTransaction(); return; }
                ////////写入数据库版本表/////
                //WriteDBVersion(iVersion, in_DateTime, this.m_Con, out ex);
                //if (ex != null) { myTrans.Rollback(); }
                ///////修改过程库////////////
                //WriteHisDB(sLayerName, this.m_HisWS, null, in_DateTime, 3, iVersion, out ex);
                WriteHisDB(sLayerName, pSDEWS, in_OID, in_DateTime, in_iVersion, out ex);
                if (ex != null) { LOGTran.AbortTransaction(); return; }
                LOGTran.CommitTransaction();
            }
            catch (Exception eError)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog == null)
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                }
                ModData.SysLog.Write(eError, null, DateTime.Now);
                //********************************************************************
                ex = new Exception("编辑要素记录失败。\n原因：" + eError.Message);
                return;
            }
        }


        /// <summary>
        /// 写入远程更新日志    guozheng added
        /// </summary>
        /// <param name="in_iOID">更新要素OID</param>
        /// <param name="in_sLayerName">更新要素所在图层名</param>
        /// <param name="in_iVersion">版本</param>
        /// <param name="in_DateTime">更新时间</param>
        /// <param name="in_iState">更新状态：1、新增，2、修改，3、删除</param>
        /// <param name="in_Envelope">更新要素的最小外包矩形</param>
        /// <param name="ex"></param>
        private static void WriteLog(IWorkspace pSDEWS, int in_iOID, string in_sLayerName, int in_iVersion, DateTime in_DateTime, int in_iState, IEnvelope in_Envelope, out Exception ex)
        {
            ex = null;
            //////去掉sde图层带的用户名
            if (in_sLayerName.Contains("."))
            {
                in_sLayerName = in_sLayerName.Substring(in_sLayerName.LastIndexOf('.') + 1);
            }
            if (pSDEWS == null) { ex = new Exception("更新环境库连接信息未初始化"); return; };
            string sql = "INSERT INTO " +ModData.m_sUpDataLOGTable + "(OID,STATE,LAYERNAME,LASTUPDATE,VERSION,XMIN,XMAX,YMIN,YMAX) values(";
            sql += in_iOID.ToString() + "," + in_iState.ToString() + ",'" + in_sLayerName + "'," + "to_date('" + in_DateTime.ToString("G") + "','yyyy-mm-dd hh24:mi:ss')" + "," + in_iVersion.ToString() + ",";
            if (in_Envelope != null)
                sql += in_Envelope.XMin.ToString() + "," + in_Envelope.XMax.ToString() + "," + in_Envelope.YMin.ToString() + "," + in_Envelope.YMax.ToString() + ")";
            else
                sql += "NULL,NULL,NULL,NULL)";
            try
            {
                pSDEWS.ExecuteSQL(sql);
            }
            catch (Exception eError)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog == null)
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                }
                ModData.SysLog.Write(eError, null, DateTime.Now);
                //********************************************************************
                ex = new Exception("写入日志表失败。\n原因：" + eError.Message);
                return;
            }
        }


        /// <summary>
        /// 修改过程库（历史库，针对删除）  guozheng added
        /// </summary>
        /// <param name="in_sLayerName">图层名(要素集名)</param>
        /// <param name="in_WS">过程库的Workspace</param>
        /// <param name="in_fea">更新的Feature</param>
        /// <param name="in_FromDateTime">编辑时间</param>
        /// <param name="in_iState">编辑状态：1、新增，2、修改，3、删除</param>
        /// <param name="in_iVersion">版本</param>
        /// <param name="ex"></param>
        private static void WriteHisDB(string in_sLayerName, IWorkspace in_WS, int OID, DateTime in_FromDateTime, int in_iVersion, out Exception ex)
        {
            ex = null;
            //////去掉sde图层带的用户名
            if (in_sLayerName.Contains("."))
            {
                in_sLayerName = in_sLayerName.Substring(in_sLayerName.LastIndexOf('.') + 1);
            }
            IFeatureClass getFeaCls = (in_WS as IFeatureWorkspace).OpenFeatureClass(in_sLayerName + "_GOH");
            if (getFeaCls == null) { ex = new Exception("找不到名为“" + in_sLayerName + "”的图层"); return; }
            //删除要素要在要素集中找到此要素的有效版本，使其变为失效状态
            try
            {
                #region 删除的要素
                IQueryFilter queryFilter = new QueryFilterClass();
                string sValue = DateTime.MaxValue.ToString("u");
                //queryFilter.WhereClause = "ToDate='" + sValue + "' AND SourceOID=" + in_fea.OID.ToString();
                queryFilter.WhereClause = "ToDate='" + sValue + "' AND SourceOID=" + OID.ToString();
                IFeatureCursor FesCur = getFeaCls.Search(queryFilter, false);
                IFeature CurFea = FesCur.NextFeature();
                while (CurFea != null)
                {
                    //**************************************************************************************
                    //GUOZHENG ADDED 应增加判断，对于删除的历史记录，若为生效的版本比当前编辑生成版本减一要小
                    //               则需要将前一版本的历史记录设为失效，再建立一个失效的版本状态为删除
                    //               否则查看历史数据时，低版本的状态都为删除，这不合逻辑
                    bool IsLowVersion = false;////////当前生效的历史记录是否比当前编辑生成版本减一要小
                    int index = CurFea.Fields.FindField("VERSION");
                    if (index > -1)
                    {
                        // CurFea.set_Value(index, in_iVersion.ToString());
                        int igetVersion = -1;
                        try
                        {
                            igetVersion = Convert.ToInt32(CurFea.get_Value(index).ToString());
                            if (igetVersion < in_iVersion - 1)
                            {
                                IsLowVersion = true;
                            }
                            else
                            {
                                IsLowVersion = false;
                            }
                        }
                        catch (Exception e)
                        {
                            //******************************************
                            //Exception Log
                            if (SysCommon.Log.Module.SysLog == null)
                                SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                            SysCommon.Log.Module.SysLog.Write(e);
                            //******************************************
                        }
                    }
                    //***************************************************************************************
                    if (IsLowVersion)
                    {
                        string sLastVersionTime = Convert.ToDateTime(GetVersionEstablishTime(in_WS, in_iVersion - 1, out ex)).ToString("u"); ;
                        //index = CurFea.Fields.FindField("VERSION");
                        //if (index > -1)
                        //{
                        //    CurFea.set_Value(index, in_iVersion - 1);
                        //}
                        index = CurFea.Fields.FindField("LASTUPDATE");
                        if (index > -1)
                        {
                            CurFea.set_Value(index, sLastVersionTime);
                        }
                        index = CurFea.Fields.FindField("ToDate");
                        if (index > -1)
                        {
                            CurFea.set_Value(index, sLastVersionTime);

                        }
                        ////////////建立一个新的版本，状态为删除////////
                        #region 建立一个新的版本，状态为删除/
                        IFeature newFea = getFeaCls.CreateFeature();
                        newFea.Shape = CurFea.Shape;
                        if (SetFieldsValue(ref newFea, ref CurFea))
                        {
                            index = -1;
                            index = newFea.Fields.FindField("FromDate");
                            if (index > -1)
                            {
                                newFea.set_Value(index, sLastVersionTime);
                            }
                            index = newFea.Fields.FindField("ToDate");
                            if (index > -1)
                            {
                                newFea.set_Value(index, in_FromDateTime.ToString("u"));
                            }
                            index = newFea.Fields.FindField("SourceOID");
                            if (index > -1)
                            {
                                newFea.set_Value(index, OID);
                            }
                            index = newFea.Fields.FindField("State");
                            if (index > -1)
                            {
                                newFea.set_Value(index, 3);
                            }
                            index = newFea.Fields.FindField("VERSION");
                            if (index > -1)
                            {
                                newFea.set_Value(index, in_iVersion.ToString());
                            }
                            newFea.Store();
                        }
                        #endregion
                    }
                    else
                    {
                        index = CurFea.Fields.FindField("State");
                        if (index > -1)
                        {
                            CurFea.set_Value(index, 3);
                        }
                        index = CurFea.Fields.FindField("VERSION");
                        if (index > -1)
                        {
                            CurFea.set_Value(index, in_iVersion);
                        }
                        index = CurFea.Fields.FindField("ToDate");
                        if (index > -1)
                        {
                            CurFea.set_Value(index, in_FromDateTime.ToString("u"));

                        }
                    }
                    CurFea.Store();
                    CurFea = FesCur.NextFeature();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(FesCur);
                #endregion
            }
            catch (Exception eError)
            {
                //******************************************
                //Exception Log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
            }
        }

        /// <summary>
        /// 修改过程库（历史库）   guozheng added
        /// </summary>
        /// <param name="in_sLayerName">图层名(要素集名)</param>
        /// <param name="in_WS">过程库的Workspace</param>
        /// <param name="in_fea">更新的Feature</param>
        /// <param name="in_FromDateTime">编辑时间</param>
        /// <param name="in_iState">编辑状态：1、新增，2、修改，3、删除</param>
        /// <param name="in_iVersion">版本</param>
        /// <param name="ex"></param>
        private static void WriteHisDB(string in_sLayerName, IWorkspace in_WS, IFeature in_fea, DateTime in_FromDateTime, int in_iState, int in_iVersion, out Exception ex)
        {
            ex = null;
            //////去掉sde图层带的用户名
            if (in_sLayerName.Contains("."))
            {
                in_sLayerName = in_sLayerName.Substring(in_sLayerName.LastIndexOf('.') + 1);
            }
            IFeatureClass getFeaCls = (in_WS as IFeatureWorkspace).OpenFeatureClass(in_sLayerName + "_GOH");
            if (getFeaCls == null) { ex = new Exception("找不到名为“" + in_sLayerName + "”的图层"); return; }
            try
            {
                if (in_iState == 1)/////新增
                {
                    //新增加的要素在要素集中CreateFeature，生效日期为当前日期，失效日期为maxvalue
                    #region 新增加的要素
                    if (in_fea == null) { ex = new Exception("新增加的要素不能为空"); return; };
                    IFeature newFea = getFeaCls.CreateFeature();
                    newFea.Shape = in_fea.Shape;
                    if (SetFieldsValue(ref newFea, ref in_fea))
                    {
                        int index = -1;
                        index = newFea.Fields.FindField("FromDate");
                        if (index > -1)
                        {
                            newFea.set_Value(index, in_FromDateTime.ToString("u"));
                        }
                        index = newFea.Fields.FindField("ToDate");
                        if (index > -1)
                        {
                            newFea.set_Value(index, DateTime.MaxValue.ToString("u"));
                        }
                        index = newFea.Fields.FindField("SourceOID");
                        if (index > -1)
                        {
                            newFea.set_Value(index, in_fea.OID.ToString());
                        }
                        index = newFea.Fields.FindField("State");
                        if (index > -1)
                        {
                            newFea.set_Value(index, in_iState.ToString());
                        }
                        index = newFea.Fields.FindField("VERSION");
                        if (index > -1)
                        {
                            newFea.set_Value(index, in_iVersion.ToString());
                        }
                        newFea.Store();
                    }
                    #endregion
                }
                else if (in_iState == 2)/////修改
                {
                    if (in_fea == null) { ex = new Exception("修改后的要素不能为空"); return; };
                    //修改要素要在要素集中找到此要素的上一个有效版本，将其设置为失效状态，再建立一个新的有效版本
                    #region 修改要素
                    //////////使原版本失效/////////////////////
                    IQueryFilter queryFilter = new QueryFilterClass();
                    string sValue = DateTime.MaxValue.ToString("u");
                    queryFilter.WhereClause = "ToDate='" + sValue + "' AND SourceOID=" + in_fea.OID.ToString();
                    IFeatureCursor FesCur = getFeaCls.Search(queryFilter, false);
                    IFeature CurFea = FesCur.NextFeature();
                    while (CurFea != null)
                    {
                        int index = -1;
                        index = CurFea.Fields.FindField("ToDate");
                        if (index > -1)
                        {
                            CurFea.set_Value(index, in_FromDateTime.ToString("u"));
                            CurFea.Store();
                        }
                        CurFea = FesCur.NextFeature();
                    }
                    ///////////建立新的版本////////
                    IFeature newFea = getFeaCls.CreateFeature();
                    newFea.Shape = in_fea.Shape;
                    if (SetFieldsValue(ref newFea, ref in_fea))
                    {
                        int index = -1;
                        index = newFea.Fields.FindField("FromDate");
                        if (index > -1)
                        {
                            newFea.set_Value(index, in_FromDateTime.ToString("u"));
                        }
                        index = newFea.Fields.FindField("ToDate");
                        if (index > -1)
                        {
                            newFea.set_Value(index, DateTime.MaxValue.ToString("u"));
                        }
                        index = newFea.Fields.FindField("SourceOID");
                        if (index > -1)
                        {
                            newFea.set_Value(index, in_fea.OID.ToString());
                        }
                        index = newFea.Fields.FindField("State");
                        if (index > -1)
                        {
                            newFea.set_Value(index, in_iState.ToString());
                        }
                        index = newFea.Fields.FindField("VERSION");
                        if (index > -1)
                        {
                            newFea.set_Value(index, in_iVersion.ToString());
                        }
                        newFea.Store();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(FesCur);
                    #endregion
                }
                else if (in_iState == 3)/////删除
                {
                    //删除要素要在要素集中找到此要素的有效版本，使其变为失效状态
                    #region 删除的要素
                    IQueryFilter queryFilter = new QueryFilterClass();
                    string sValue = DateTime.MaxValue.ToString("u");
                    //queryFilter.WhereClause = "ToDate='" + sValue + "' AND SourceOID=" + in_fea.OID.ToString();
                    queryFilter.WhereClause = "ToDate='" + sValue + "' AND SourceOID=" + in_fea.OID.ToString();
                    IFeatureCursor FesCur = getFeaCls.Search(queryFilter, false);
                    IFeature CurFea = FesCur.NextFeature();
                    while (CurFea != null)
                    {
                        //**************************************************************************************
                        //GUOZHENG ADDED 应增加判断，对于删除的历史记录，若为生效的版本比当前编辑生成版本减一要小
                        //               则需要将前一版本的历史记录设为失效，再建立一个失效的版本状态为删除
                        //               否则查看历史数据时，低版本的状态都为删除，这不合逻辑
                        bool IsLowVersion = false;////////当前生效的历史记录是否比当前编辑生成版本减一要小
                        int index = CurFea.Fields.FindField("VERSION");
                        if (index > -1)
                        {
                            // CurFea.set_Value(index, in_iVersion.ToString());
                            int igetVersion = -1;
                            try
                            {
                                igetVersion = Convert.ToInt32(CurFea.get_Value(index).ToString());
                                if (igetVersion < in_iVersion - 1)
                                {
                                    IsLowVersion = true;
                                }
                                else
                                {
                                    IsLowVersion = false;
                                }
                            }
                            catch (Exception e)
                            {
                                //******************************************
                                //Exception Log
                                if (SysCommon.Log.Module.SysLog == null)
                                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                SysCommon.Log.Module.SysLog.Write(e);
                                //******************************************
                            }
                        }
                        //***************************************************************************************
                        if (IsLowVersion)
                        {
                            string sLastVersionTime = Convert.ToDateTime(GetVersionEstablishTime(in_WS, in_iVersion - 1, out ex)).ToString("u");
                            //index = CurFea.Fields.FindField("VERSION");
                            //if (index > -1)
                            //{
                            //    CurFea.set_Value(index, in_iVersion - 1);
                            //}
                            index = CurFea.Fields.FindField("LASTUPDATE");
                            if (index > -1)
                            {
                                CurFea.set_Value(index, sLastVersionTime);
                            }
                            index = CurFea.Fields.FindField("ToDate");
                            if (index > -1)
                            {
                                CurFea.set_Value(index, sLastVersionTime);

                            }
                            ////////////建立一个新的版本，状态为删除////////
                            #region 建立一个新的版本，状态为删除/
                            IFeature newFea = getFeaCls.CreateFeature();
                            newFea.Shape = in_fea.Shape;
                            if (SetFieldsValue(ref newFea, ref in_fea))
                            {
                                index = -1;
                                index = newFea.Fields.FindField("FromDate");
                                if (index > -1)
                                {
                                    newFea.set_Value(index, sLastVersionTime);
                                }
                                index = newFea.Fields.FindField("ToDate");
                                if (index > -1)
                                {
                                    newFea.set_Value(index, in_FromDateTime.ToString("u"));
                                }
                                index = newFea.Fields.FindField("SourceOID");
                                if (index > -1)
                                {
                                    newFea.set_Value(index, in_fea.OID.ToString());
                                }
                                index = newFea.Fields.FindField("State");
                                if (index > -1)
                                {
                                    newFea.set_Value(index, in_iState.ToString());
                                }
                                index = newFea.Fields.FindField("VERSION");
                                if (index > -1)
                                {
                                    newFea.set_Value(index, in_iVersion.ToString());
                                }
                                newFea.Store();
                            }
                            #endregion
                        }
                        else
                        {
                            index = CurFea.Fields.FindField("State");
                            if (index > -1)
                            {
                                CurFea.set_Value(index, in_iState.ToString());
                            }
                            index = CurFea.Fields.FindField("VERSION");
                            if (index > -1)
                            {
                                CurFea.set_Value(index, in_iVersion);
                            }
                            index = CurFea.Fields.FindField("ToDate");
                            if (index > -1)
                            {
                                CurFea.set_Value(index, in_FromDateTime.ToString("u"));

                            }
                        }
                        CurFea.Store();
                        CurFea = FesCur.NextFeature();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(FesCur);
                    #endregion
                }
                else
                {
                    ex = new Exception("不支持的状态");
                    return;
                }
            }
            catch (Exception eError)
            {
                //******************************************
                //Exception Log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
            }
        }

        /// <summary>
        /// 将pDesFeat的字段赋予pOriFeat   guozheng added
        /// </summary>
        /// <param name="pOriFeat">属性接受要素</param>
        /// <param name="pDesFeat">属性来源要素</param>
        /// <returns></returns>
        private static  bool SetFieldsValue(ref IFeature pOriFeat, ref IFeature pDesFeat)/////将pDesFeat的字段赋予pOriFeat
        {

            int IdesFiledIndex = -1;
            string sFieldName = string.Empty;

            for (int i = 0; i < pOriFeat.Fields.FieldCount; i++)
            {
                if (pOriFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeOID || pOriFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    continue;
                if (pOriFeat.Fields.get_Field(i).Editable)
                {
                    sFieldName = pOriFeat.Fields.get_Field(i).Name;
                    IdesFiledIndex = pDesFeat.Fields.FindField(sFieldName);
                    if (IdesFiledIndex > -1)
                    {
                        if (pDesFeat.get_Value(IdesFiledIndex) != null)
                        {
                            pOriFeat.set_Value(i, pDesFeat.get_Value((int)IdesFiledIndex));

                        }
                        else
                        {
                            if (pDesFeat.Fields.get_Field(IdesFiledIndex).IsNullable)
                            {
                                pOriFeat.set_Value(i, null);
                            }
                            else
                            {
                                if (pDesFeat.Fields.get_Field(IdesFiledIndex).Type == esriFieldType.esriFieldTypeString)
                                {
                                    pOriFeat.set_Value(i, string.Empty);
                                }
                                else if (pDesFeat.Fields.get_Field(IdesFiledIndex).Type == esriFieldType.esriFieldTypeDouble || pDesFeat.Fields.get_Field(IdesFiledIndex).Type == esriFieldType.esriFieldTypeInteger || pDesFeat.Fields.get_Field(IdesFiledIndex).Type == esriFieldType.esriFieldTypeSingle)
                                {
                                    pOriFeat.set_Value(i, 0);
                                }
                            }

                        }
                    }
                }
            }
            return true;
        }
 
        
        /// <summary>
        ///  写入数据库版本表    guozheng added
        /// </summary>
        /// <param name="in_iVersion">要写入的版本号</param>
        /// <param name="in_DateTime">版本建立时间</param>
        /// <param name="ex"></param>
        private static void WriteDBVersion(IWorkspace pSDEWS, int in_iVersion, DateTime in_DateTime, out Exception ex)
        {
            ex = null;
            if (null == pSDEWS) { ex = new Exception("更新环境库连接信息未初始化。"); return; }
            if (null == in_DateTime) { ex = new Exception("输入时间不能为空"); return; }
            string sql = "INSERT INTO " +ModData.m_sDBVersionTable + "(VERSION,USERNAME,VERSIONTIME,DES) values(";
            sql += in_iVersion.ToString() + "," + "null," + "to_date('" + in_DateTime.ToString("G") + "','yyyy-mm-dd hh24:mi:ss')" + ",null)";
            try
            {
                pSDEWS.ExecuteSQL(sql);
            }
            catch (Exception eError)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog == null)
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                }
                ModData.SysLog.Write(eError, null, DateTime.Now);
                //********************************************************************
                ex = new Exception("写入数据库版本表失败。\n原因：" + eError.Message);
                return;
            }

        }
        /// <summary>
        /// 获取当前版本信息（获取环境中数据库版本表中VERSION的最大值加1做为当前编辑生成版本）   guozheng added
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static int GetVersion(IWorkspace pSDEWS, out Exception ex)
        {
            ex = null;
            int iVersion = -1;
            if (pSDEWS == null) { ex = new Exception("更新环境库尚未初始化"); return -1; }
            try
            {
                ITable getTable = (pSDEWS as IFeatureWorkspace).OpenTable(ModData.m_sDBVersionTable);
                if (getTable.RowCount(null) == 0) return 1;
                else
                {
                    int index = getTable.FindField("VERSION");
                    if (index < 0) { ex = new Exception("数据库版本表中未能找到VERSION字段"); return -1; }
                    ICursor TableCursor = getTable.Search(null, false);
                    IDataStatistics dataStatistics = new DataStatisticsClass();
                    dataStatistics.Field = "VERSION";
                    dataStatistics.Cursor = TableCursor;
                    ESRI.ArcGIS.esriSystem.IStatisticsResults statisticsResults = dataStatistics.Statistics;
                    double getMaxVersion = statisticsResults.Maximum;
                    iVersion = Convert.ToInt32(getMaxVersion);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(TableCursor);
                }
                return iVersion + 1;
            }
            catch (Exception eError)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog == null)
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                }
                ModData.SysLog.Write(eError, null, DateTime.Now);
                //********************************************************************
                ex = new Exception("获取新数据库版本信息失败。\n原因：" + eError.Message);
                return -1;
            }
        }

        //***********************************************************************************************************
        //guozheng added  获取一个版本建立的时间
        private static string GetVersionEstablishTime(IWorkspace in_WS, int in_iVersion, out Exception ex)
        {
            ex = null;
            if (in_WS == null) { ex = new Exception("更新环境库尚未初始化"); return null; }
            try
            {
                ITable getTable = (in_WS as IFeatureWorkspace).OpenTable("GO_DATABASE_VERSION");
                IQueryFilter Filter = new QueryFilter();
                Filter.WhereClause = "VERSION=" + in_iVersion.ToString();
                ICursor TableCur = getTable.Search(Filter, false);
                IRow getRow = TableCur.NextRow();
                while (getRow != null)
                {
                    int index = getRow.Fields.FindField("VERSIONTIME");
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(TableCur);
                    return getRow.get_Value(index).ToString();
                }
                ex = new Exception("数据库版本记录表中找不到版本为：" + in_iVersion.ToString() + ",的建立时间。");
                System.Runtime.InteropServices.Marshal.ReleaseComObject(TableCur);
                return null;
            }
            catch 
            {

                return null;
            }
        }
        //***********************************************************************************************************
    }
}
