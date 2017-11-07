using System;
using System.Collections.Generic;
using System.Text; 
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GeoEdit
{
    /// <summary>
    /// 冲突版本协调  陈亚飞20101104添加
    /// </summary>
    public class ClsVersionReconcile
    {
        private IFeatureWorkspace featureWorkspace = null;  //

        private IWorkspaceEdit v_WSEdit = null;//当前编辑工作空间

        public ClsVersionReconcile(IWorkspaceEdit pWSEdit)//(IVersion version)
        {
            if (pWSEdit == null) return;
            v_WSEdit = pWSEdit;
            //// Save the version as a member variable.
            //featureWorkspace = (IFeatureWorkspace)version;
            //// Subscribe to the OnReconcile event.
            //IVersionEvents_Event versionEvent = (IVersionEvents_Event)version;
            //versionEvent.OnConflictsDetected += new
            //  IVersionEvents_OnConflictsDetectedEventHandler(OnConflictsDetected);
        }

        /// <summary>
        /// 获取工作空间的默认版本
        /// </summary>
        /// <param name="eError"></param>
        /// <returns></returns>
        public string GetDefautVersionName(out Exception eError)
        {
            string verName = "";//默认版本名称
            eError = null;
            IVersionedWorkspace pVersionedWS = v_WSEdit as IVersionedWorkspace;
            if (pVersionedWS == null)
            {
                eError=new Exception("该数据还没有注册版本!");
                return "";
            }

            IVersion pDefVersion = pVersionedWS.DefaultVersion;  //默认版本
            if (pDefVersion == null)
            {
                eError = new Exception("获取默认版本出错！");
                return "";
            }
            if (pDefVersion.VersionInfo == null)
            {
                eError = new Exception("获取默认版本出错！");
                return "";
            }
            verName = pDefVersion.VersionInfo.VersionName;  

            if (verName == "")
            {
                eError=new Exception("获取默认版本名称出错!");
                return "";
            }
            return verName;
        }

        //******************************************************************************************************************************************* 
        //若使用数据库版本，并且没有选中融合，说明冲突要素没有发生变化，则需要将冲突要素保存起来，在写入更新日志时，要排除这些记录
        //同理，若使用数据库版本，选中融合，但是融合不成功，说明要素还是没有发生变化，也需要将这一类要素存储起来，在写入更新日志时，排除这些记录

        //若使用当前版本覆盖数据库版本，不用清除更新日志记录表里面冲突要素对应的更新日志信息，但是要修改历史库相应的要素，将以前的更改数据置为无效
        //*******************************************************************************************************************************************
        /// <summary>
        /// 保存需要排除的冲突要素记录
        /// </summary>
        /// <param name="defVerName">默认版本名称</param>
        /// <param name="bChildWin">true表示用当前版本替换前一版本，false表示使用数据库版本</param>
        /// <param name="beMerge">true表示融合冲突要素几何形状，false表示不融合</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<int, List<int>>> ReconcileVersion(string defVerName, bool bChildWin, bool beMerge, out Exception eError)
        {
            Dictionary<string, Dictionary<int, List<int>>> conflictFeaClsDic = null;  //产生冲突的要素类信息

            bool bLock = true;                      //true获得锁
            bool bebortIfConflict = false;           //检测到冲突时，版本协调是否停止，true停止，false不停止
            //bool bChildWin = false;                 //true替换上一个版本（冲突版本）,false用上一个版本（冲突版本）
            bool battributeCheck = false;          //false若为true则只有修改同一个属性时才检测出冲突
            //bool beMerge = true;                   //对于产生冲突的要素是否融合
            eError = null;

            IVersionEdit4 pVersionEdit4 =v_WSEdit as IVersionEdit4; //版本编辑
            if (pVersionEdit4 == null)
            {
                eError = new Exception("获取当前版本编辑出错！");
                return null;
            }
            try
            {
                //协调版本
                if (pVersionEdit4.Reconcile4(defVerName, bLock, bebortIfConflict, bChildWin, battributeCheck))
                {
                    //存在冲突,冲突处理方式分为3种： 使用当前版本、使用数据库版本、使用自定义处理方式(未实现)
                    IFeatureWorkspace pOrgFWS = null;           //原始数据工作空间
                    IFeatureWorkspace pReconWS = null;          //第一个编辑冲突版本工作空间
                    IFeatureWorkspace pPreReconWS = null;       //第二个编辑冲突版本工作空间（正在编辑的）

                    pOrgFWS = pVersionEdit4.CommonAncestorVersion as IFeatureWorkspace;
                    if (pOrgFWS == null)
                    {
                        eError = new Exception("原始数据库工作空间为空！");
                        return null ;
                    }
                    pReconWS = pVersionEdit4.ReconcileVersion as IFeatureWorkspace;
                    if (pReconWS == null)
                    {
                        eError = new Exception("第一个编辑冲突版本工作空间为空！");
                        return null;
                    }
                    pPreReconWS = pVersionEdit4.PreReconcileVersion as IFeatureWorkspace;
                    if (pPreReconWS == null)
                    {
                        eError = new Exception("第二个编辑冲突版本工作空间为空！");
                        return null;
                    }

                    conflictFeaClsDic = new Dictionary<string, Dictionary<int, List<int>>>();
                    #region 对冲突要素类进行处理
                    //获取产生冲突的要素
                    IEnumConflictClass pEnumConflictCls = pVersionEdit4.ConflictClasses;
                    if (pEnumConflictCls == null) return null ;
                    pEnumConflictCls.Reset();
                    IConflictClass pConflictCls = pEnumConflictCls.Next();
                    //遍历冲突要素类，对冲突要素进行处理
                    while (pConflictCls != null)
                    {
                        if (pConflictCls.HasConflicts)
                        {
                            //若该要素类存在冲突要素
                            IDataset pdt = pConflictCls as IDataset;
                            string feaClsName = ""; //冲突要素类名称
                            feaClsName = pdt.Name;  
                            if (feaClsName.Contains("."))
                            {
                                feaClsName = feaClsName.Substring(feaClsName.IndexOf('.') + 1);
                            }

                            IFeatureClass pFeaCls = null;               //需要编辑的featureclass
                            IFeatureClass pOrgFeaCls = null;                     //冲突原始要素类
                            IFeatureClass pReconFeaCls = null;                   //第一个编辑冲突版本要素类
                            IFeatureClass pPreReconFeaCls = null;                //第二个编辑版本冲突要素类
                            IFeature pOrgFea = null;                             //原始要素类中产生冲突对应的要素
                            IFeature pReconFea = null;                           //第一个编辑版本中产生冲突的要素
                            IFeature pPreReconFea = null;                        //第二个编辑版本中产生冲突的要素
                            
                            Dictionary<int, List<int>> feaOIDDic = new Dictionary<int, List<int>>();//用来保存产生冲突的要素类信息
                            List<int> OidLst = null;                                                //用来保存产生冲突的要素oid集合

                            try
                            {
                                pFeaCls = (v_WSEdit as IFeatureWorkspace).OpenFeatureClass(feaClsName);
                                //获取不同版本对应的冲突要素类
                                pOrgFeaCls = pOrgFWS.OpenFeatureClass(feaClsName);
                                pReconFeaCls = pReconWS.OpenFeatureClass(feaClsName);
                                pPreReconFeaCls = pPreReconWS.OpenFeatureClass(feaClsName);
                            }
                            catch(Exception ex)
                            {
                                //******************************************
                                //guozheng added System Exception log
                                if (SysCommon.Log.Module.SysLog == null)
                                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                SysCommon.Log.Module.SysLog.Write(ex);
                                //******************************************
                                eError = new Exception("获取冲突要素类出错！");
                                //******************************************
                                //guozheng added System Exception log
                                if (SysCommon.Log.Module.SysLog == null)
                                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                SysCommon.Log.Module.SysLog.Write(eError);
                                //******************************************
                                //if (!bChildWin &&beMerge)
                                //{
                                //    //使用数据库版本，未进行融合，保要素信息存冲突
                                //    return;
                                //}
                                pConflictCls = pEnumConflictCls.Next();
                                continue;
                            }

                            #region 处理冲突（进行融合）,并保存冲突要素信息
                            //在上一个版本（目标冲突版本）被修改,修改用2表示，但是在当前版本被删除的选择集
                            ISelectionSet deUpSelSet = pConflictCls.DeleteUpdates;
                            if (deUpSelSet != null)
                            {
                                #region 进行冲突处理，并保存产生冲突的要素信息，这种情况不能融合
                                IEnumIDs enumIDs = deUpSelSet.IDs;
                                int ppoid = -1;
                                ppoid = enumIDs.Next();

                                OidLst = new List<int>();
                                //遍历冲突要素类中存在的修改删除冲突要素
                                while (ppoid != -1)
                                {
                                    if (beMerge)
                                    {
                                        // 融合
                                        //eError = new Exception("编辑区域重叠,不能融合\n要素OID为:" + ppoid + ",要素类名称为：" + feaClsName + "。\n处理为用当前编辑覆盖原有编辑!");
                                    }
                                    if (bChildWin == false)
                                    {
                                        //使用数据库版本,保存产生冲突的要素信息
                                        if (!OidLst.Contains(ppoid))
                                        {
                                            OidLst.Add(ppoid);
                                        }
                                        if (!feaOIDDic.ContainsKey(2))
                                        {
                                            feaOIDDic.Add(2, OidLst);
                                        }
                                        else
                                        {
                                            if (!feaOIDDic[2].Contains(ppoid))
                                            {
                                                feaOIDDic[2].Add(ppoid);
                                            }
                                        }
                                    }
                                    ppoid = enumIDs.Next();
                                }
                                #endregion
                            }

                            //在上一个版本（目标冲突版本）被删除,删除用3表示，但是在当前版本被修改的选择集
                            ISelectionSet upDeSelSet = pConflictCls.UpdateDeletes;
                            if (upDeSelSet != null)
                            {
                                #region 进行冲突处理，并保存产生冲突的要素信息
                                IEnumIDs enumIDs = upDeSelSet.IDs;
                                int ppoid = -1;
                                ppoid = enumIDs.Next();
                                OidLst = new List<int>();
                                //遍历要素类中存在的删除修改冲突
                                while (ppoid != -1)
                                {
                                    if (beMerge)
                                    {
                                        // 融合
                                        //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "编辑区域重叠,不能融合\n要素OID为:" + ppoid + ",要素类名称为：" + feaClsName + "。");
                                    }
                                    if (bChildWin == false)
                                    {
                                        //使用数据库版本，保存产生冲突的融合不了的要素信息
                                        if (!OidLst.Contains(ppoid))
                                        {
                                            OidLst.Add(ppoid);
                                        }
                                        if (!feaOIDDic.ContainsKey(3))
                                        {
                                            feaOIDDic.Add(3, OidLst);
                                        }
                                        else
                                        {
                                            if (!feaOIDDic[3].Contains(ppoid))
                                            {
                                                feaOIDDic[3].Add(ppoid);
                                            }
                                        }
                                    }
                                    ppoid = enumIDs.Next();
                                }
                                #endregion
                            }

                            //在上一个版本（目标冲突版本）被修改,修改用2表示，但是在当前版本被修改的选择集
                            ISelectionSet upUpSelSet = pConflictCls.UpdateUpdates;
                            if (upUpSelSet != null)
                            {
                                #region 进行冲突处理，并保存产生冲突的要素信息
                                IEnumIDs enumIDs = upUpSelSet.IDs;
                                int ppoid = -1;
                                ppoid = enumIDs.Next();
                                OidLst = new List<int>();
                                //遍历要素类中存在的修改修改冲突要素
                                while (ppoid != -1)
                                {
                                    if (pPreReconFeaCls == null || pReconFeaCls == null || pOrgFeaCls == null) break;
                                    pPreReconFea = pPreReconFeaCls.GetFeature(ppoid);
                                    pReconFea = pReconFeaCls.GetFeature(ppoid);
                                    pOrgFea = pOrgFeaCls.GetFeature(ppoid);
                                    if (pPreReconFea == null || pReconFea == null || pOrgFea == null) break;
                                    IConstructMerge constructMerge = new GeometryEnvironmentClass();  //融合变量
                                    IGeometry newGeometry = null;                                     //融合后的几何形状
                                    if (beMerge)
                                    {
                                        #region 融合
                                        try
                                        {
                                            newGeometry = constructMerge.MergeGeometries(pOrgFea.ShapeCopy, pReconFea.ShapeCopy, pPreReconFea.ShapeCopy);
                                        }
                                        catch(Exception ex)
                                        {
                                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "编辑区域重叠,不能融合");
                                            //******************************************
                                            //guozheng added System Exception log
                                            if (SysCommon.Log.Module.SysLog == null)
                                                SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                            SysCommon.Log.Module.SysLog.Write(ex);
                                            //******************************************
                                            if (bChildWin == false)
                                            {
                                                //若用原来的版本替换现在的版本并且不能融合，说明该要素没有发生变化，应该在日志记录表里面排除掉
                                                //保存产生冲突的要素信息
                                                if (!OidLst.Contains(ppoid))
                                                {
                                                    OidLst.Add(ppoid);
                                                }
                                                if (!feaOIDDic.ContainsKey(2))
                                                {
                                                    feaOIDDic.Add(2, OidLst);
                                                }
                                                else
                                                {
                                                    if (!feaOIDDic[2].Contains(ppoid))
                                                    {
                                                        feaOIDDic[2].Add(ppoid);
                                                    }
                                                }
                                            }

                                            ppoid = enumIDs.Next();
                                            continue;
                                        }
                                        IFeature ppfea = pFeaCls.GetFeature(ppoid);
                                        ppfea.Shape = newGeometry;
                                        ppfea.Store();
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 不融合
                                        if (bChildWin == false)
                                        {
                                            //若用原来的版本替换现在的版本并且不能融合，说明该要素没有发生变化，应该在日志记录表里面排除掉
                                            //保存产生冲突的要素信息
                                            if (!OidLst.Contains(ppoid))
                                            {
                                                OidLst.Add(ppoid);
                                            }
                                            if (!feaOIDDic.ContainsKey(2))
                                            {
                                                feaOIDDic.Add(2, OidLst);
                                            }
                                            else
                                            {
                                                if (!feaOIDDic[2].Contains(ppoid))
                                                {
                                                    feaOIDDic[2].Add(ppoid);
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    ppoid = enumIDs.Next();
                                }
                                #endregion
                            }

                            if (!conflictFeaClsDic.ContainsKey(feaClsName))
                            {
                                if (feaOIDDic != null)
                                {
                                    if (feaOIDDic.Count > 0)
                                    {
                                        conflictFeaClsDic.Add(feaClsName, feaOIDDic);
                                    }
                                }
                            }
                            #endregion
                        }
                        pConflictCls = pEnumConflictCls.Next();
                    }
                    #endregion
                }
                return conflictFeaClsDic;
            }
            catch(Exception ex)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(ex);
                //******************************************
                eError=new Exception("版本协调出错!");
                return null;
            }
        }

        /// <summary>
        /// 保存发生更新变化的所有的要素信息
        /// </summary>
        /// <param name="eError"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<int, List<IRow>>> GetModifyClsInfo(out Exception eError)
        {
            eError = null; 
            Dictionary<string, Dictionary<int, List<IRow>>> feaChangeDic = null;

            //获得命名空间下发生变化的信息
            IWorkspaceEdit2 pWSEdit2 =v_WSEdit as IWorkspaceEdit2;
            if (pWSEdit2 == null)
            {
                eError = new Exception("编辑工作空间为空！");
                return null;
            }

            //获取在一个编辑会话中发生更新变化数据
            IDataChangesEx pDataChangeEx = pWSEdit2.get_EditDataChanges(esriEditDataChangesType.esriEditDataChangesWithinSession);
            if (pDataChangeEx == null)
            {
                eError = new Exception("未发现更新变化数据!");
                return null;
            }

            feaChangeDic = new Dictionary<string, Dictionary<int, List<IRow>>>();

            //获取发生更新变化的要素类集合
            IEnumBSTR pEnumString = pDataChangeEx.ModifiedClasses;  //发生变化的要素类
            pEnumString.Reset();
            string pModifyClsName = "";//发生更新变化的要素类名称
            pModifyClsName = pEnumString.Next();
            //遍历发生更新变化的要素类，并将相关信息保存下来
            while (pModifyClsName != null)
            {
                IDifferenceCursorEx pDifCusorEx = null;  //游标
                int pFeaOid = -1;                       //要素OID
                IRow pSourceRow = null;                 //正在编辑的冲突要素行
                IRow pDifRow = null;                    //上一编辑版本的冲突要素行
                ILongArray pDifIndices = null;          //字段索引

                if (pModifyClsName.Contains("."))
                {
                    pModifyClsName = pModifyClsName.Substring(pModifyClsName.IndexOf('.') + 1);
                }
                Dictionary<int, List<IRow>> stateRowDic = new Dictionary<int, List<IRow>>();
                List<IRow> rowLst = null;

                #region 保存新增加的要素信息
                rowLst = new List<IRow>();
                //新增数据游标
                pDifCusorEx = pDataChangeEx.ExtractEx(pModifyClsName, esriDifferenceType.esriDifferenceTypeInsert);
                if (pDifCusorEx == null)
                {
                    eError = new Exception("获取新增数据的游标发生错误！");
                    return null;
                }
                pDifCusorEx.Next(out pFeaOid, out pSourceRow, out pDifRow, out pDifIndices);//pDifRow=null
                //遍历新增加的要素oid,并将其保存起来
                while (pFeaOid != -1)
                {
                    //将新增数据保存起来
                    if (!rowLst.Contains(pSourceRow))
                    {
                        rowLst.Add(pSourceRow);
                    }
                    pDifCusorEx.Next(out pFeaOid, out pSourceRow, out pDifRow, out pDifIndices);
                }
                if (rowLst != null)
                {
                    if (rowLst.Count > 0)
                    {
                        stateRowDic.Add(1, rowLst);
                    }
                }
                #endregion

                #region 保存修改后的要素信息
                rowLst = new List<IRow>();
                //修改数据游标
                pDifCusorEx = pDataChangeEx.ExtractEx(pModifyClsName, esriDifferenceType.esriDifferenceTypeUpdateNoChange);
                if (pDifCusorEx == null)
                {
                    eError = new Exception("获取修改数据的游标发生错误！");
                    return null;
                }
                pDifCusorEx.Next(out pFeaOid, out pSourceRow, out pDifRow, out pDifIndices);
                //遍历修改后的要素OID，并将其保存起来
                while (pFeaOid != -1)
                {
                    //将修改后的数据保存起来
                    if (!rowLst.Contains(pSourceRow))
                    {
                        rowLst.Add(pSourceRow);
                    }

                    pDifCusorEx.Next(out pFeaOid, out pSourceRow, out pDifRow, out pDifIndices);
                }
                if (rowLst != null)
                {
                    if (rowLst.Count > 0)
                    {
                        stateRowDic.Add(2, rowLst);
                    }
                }
                #endregion

                #region 保存删除的要素信息
                rowLst = new List<IRow>();
                //删除数据游标
                pDifCusorEx = pDataChangeEx.ExtractEx(pModifyClsName, esriDifferenceType.esriDifferenceTypeDeleteNoChange);
                if (pDifCusorEx == null)
                {
                    eError = new Exception("获取删除数据的游标发生错误！");
                    return null;
                }
                pDifCusorEx.Next(out pFeaOid, out pSourceRow, out pDifRow, out pDifIndices);//pSourceRow
                //遍历删除的要素OID，并将其保存起来
                while (pFeaOid != -1)
                {
                    //将删除的数据保存起来（数据库版本数据）
                    if (!rowLst.Contains(pDifRow))
                    {
                        rowLst.Add(pDifRow);
                    }
                    pDifCusorEx.Next(out pFeaOid, out pSourceRow, out pDifRow, out pDifIndices);
                }
                if (rowLst != null)
                {
                    if (rowLst.Count > 0)
                    {
                        stateRowDic.Add(3, rowLst);
                    }
                }
                #endregion

                if (!feaChangeDic.ContainsKey(pModifyClsName))
                {
                    feaChangeDic.Add(pModifyClsName, stateRowDic);
                }
                pModifyClsName = pEnumString.Next();
            }
            
            return feaChangeDic;
        }


        /// <summary>
        /// 保存更新后的更新变化的要素信息，排除掉冲突要素
        /// </summary>
        /// <param name="feaChangeDic">更新变化要素，冲突要素</param>
        /// <param name="conflictFeaClsDic"></param>
        /// <param name="eError"></param>
        public Dictionary<string, Dictionary<int, List<IRow>>> GetPureModifySaveInfo(Dictionary<string, Dictionary<int, List<IRow>>> feaChangeDic, Dictionary<string, Dictionary<int, List<int>>> conflictFeaClsDic, out Exception eError)
        {
            eError = null;
            if (conflictFeaClsDic == null) return feaChangeDic;
            if (conflictFeaClsDic.Count == 0) return feaChangeDic;
            //遍历发生冲突的要素类
            foreach (KeyValuePair<string, Dictionary<int, List<int>>> feaConflicItem in conflictFeaClsDic)
            {
                string pFeaCLsName= "";                                                         //更新变化的要素类名称
                Dictionary<int, List<IRow>> feaSafeDic = new Dictionary<int, List<IRow>>();    //保存编辑后的状态和要素集合
                pFeaCLsName = feaConflicItem.Key;
                if (feaChangeDic.ContainsKey(pFeaCLsName))
                {
                    feaSafeDic = feaChangeDic[pFeaCLsName];
                }
                else
                {
                    continue;
                }

                //遍历更变化状态：1,2,3
                foreach (KeyValuePair<int, List<int>> stateOIdItem in feaConflicItem.Value)
                {
                    int pState;                          //更新变化状态
                    List<IRow> rowLst = new List<IRow>();  //发生冲突的要素集合
                    pState = stateOIdItem.Key;

                    if (feaSafeDic.ContainsKey(pState))
                    {
                        rowLst = feaSafeDic[pState];
                    }
                    else
                    {
                        continue;
                    }
                    //遍历某一更新状态下的更新行集合
                    foreach (int oidItem in stateOIdItem.Value)
                    {
                        foreach(IRow rowItem in rowLst)
                        {
                            int pOID = rowItem.OID;
                            if (pOID == oidItem)
                            {
                                //删除该条记录
                                if (!rowLst.Remove(rowItem))
                                {
                                    eError = new Exception("删除冲突项出错！");
                                    return null;
                                }
                            }
                        }
                    }
                }
            }

            return feaChangeDic;
        }






        /// <summary>
        /// Pseudocode:
        /// - Loop through all conflicts classes after the reconcile.
        /// - Loop through every UpdateUpdate conflict on the class.
        /// - Determine if geometry is in conflict on the feature.
        /// - If so, merge geometries together (handling errors) and store the feature.
        /// </summary>
        public void OnConflictsDetected(ref bool conflictsRemoved, ref bool errorOccurred, ref string errorString)
        {
            try
            {
                IVersionEdit4 versionEdit4 = (IVersionEdit4)featureWorkspace;
                // Get the various versions on which to output information.
                IFeatureWorkspace commonAncestorFWorkspace = (IFeatureWorkspace)
                  versionEdit4.CommonAncestorVersion;
                IFeatureWorkspace preReconcileFWorkspace = (IFeatureWorkspace)
                  versionEdit4.PreReconcileVersion;
                IFeatureWorkspace reconcileFWorkspace = (IFeatureWorkspace)
                  versionEdit4.ReconcileVersion;
                IEnumConflictClass enumConflictClass = versionEdit4.ConflictClasses;
                IConflictClass conflictClass = null;
                while ((conflictClass = enumConflictClass.Next()) != null)
                {
                    IDataset dataset = (IDataset)conflictClass;
                    // Make sure class is a feature class.
                    if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        String datasetName = dataset.Name;
                        IFeatureClass featureClass = featureWorkspace.OpenFeatureClass
                          (datasetName);
                        Console.WriteLine("Conflicts on feature class {0}", datasetName);
                        // Get all UpdateUpdate conflicts.
                        ISelectionSet updateUpdates = conflictClass.UpdateUpdates;
                        if (updateUpdates.Count > 0)
                        {
                            // Get conflict feature classes on the three reconcile versions.
                            IFeatureClass featureClassPreReconcile =
                              preReconcileFWorkspace.OpenFeatureClass(datasetName);
                            IFeatureClass featureClassReconcile =
                              reconcileFWorkspace.OpenFeatureClass(datasetName);
                            IFeatureClass featureClassCommonAncestor =
                              commonAncestorFWorkspace.OpenFeatureClass(datasetName);
                            // Iterate through each OID, outputting information.
                            IEnumIDs enumIDs = updateUpdates.IDs;
                            int oid = -1;
                            while ((oid = enumIDs.Next()) != -1)
                            //loop through all conflicting features 
                            {
                                Console.WriteLine("UpdateUpdate conflicts on feature {0}", oid);
                                // Get conflict feature on the three reconcile versions.
                                IFeature featurePreReconcile =
                                  featureClassPreReconcile.GetFeature(oid);
                                IFeature featureReconcile = featureClassReconcile.GetFeature(oid);
                                IFeature featureCommonAncestor =
                                  featureClassCommonAncestor.GetFeature(oid);
                                // Check to make sure each shape is different than the common ancestor (conflict is on shape field).
                                if (IsShapeInConflict(featureCommonAncestor, featurePreReconcile,
                                  featureReconcile))
                                {
                                    Console.WriteLine(
                                      " Shape attribute has changed on both versions...");
                                    // Geometries are in conflict ... merge geometries.
                                    try
                                    {
                                        IConstructMerge constructMerge = new GeometryEnvironmentClass
                                          ();
                                        IGeometry newGeometry = constructMerge.MergeGeometries
                                          (featureCommonAncestor.ShapeCopy,
                                          featureReconcile.ShapeCopy, featurePreReconcile.ShapeCopy);
                                        // Setting new geometry as a merge between the two versions.
                                        IFeature feature = featureClass.GetFeature(oid);
                                        feature.Shape = newGeometry;
                                        feature.Store();
                                        updateUpdates.RemoveList(1, ref oid);
                                        conflictsRemoved = true;
                                    }
                                    catch(Exception eError) //COMException comExc
                                    {
                                        //******************************************
                                        //guozheng added System Exception log
                                        if (SysCommon.Log.Module.SysLog == null)
                                            SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                        SysCommon.Log.Module.SysLog.Write(eError);
                                        //******************************************
                                        //// Check if the error is from overlapping edits.
                                        //if (comExc.ErrorCode == (int)
                                        //  fdoError.FDO_E_WORKSPACE_EXTENSION_DATASET_CREATE_FAILED ||
                                        //  comExc.ErrorCode == (int)
                                        //  fdoError.FDO_E_WORKSPACE_EXTENSION_DATASET_DELETE_FAILED)
                                        //{
                                        //    // Edited areas overlap.
                                        //    Console.WriteLine(
                                        //      "Error from overlapping edits on feature {0}", oid);
                                        //    Console.WriteLine(" Error Message: {0}", comExc.Message);
                                        //    Console.WriteLine(
                                        //      " Can't merge overlapping edits to same feature.");
                                        //}
                                        //else
                                        //{
                                        //    // Unexpected COM exception throw this to exception handler.
                                        //    throw comExc;
                                        //}
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(
                                      " Shape field not in conflict: merge not necessary ... ");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eError)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
                //Console.WriteLine("Error Message: {0}, Error Code: {1}", comExc.Message,
                //  comExc.ErrorCode);
            }
            //catch (Exception exc)
            //{
            //    Console.WriteLine("Unhandled Exception: {0}", exc.Message);
            //}
        }


        //判断原始版本要素，冲突要素和当前版本要素是否存在几何冲突
        // Method to determine if shape field is in conflict.
        private bool IsShapeInConflict(IFeature commonAncestorFeature, IFeature preReconcileFeature, IFeature reconcileFeature)
        {
            // 1st check: Common Ancestor with PreReconcile.
            // 2nd check: Common Ancestor with Reconcile. 
            // 3rd check: Reconcile with PreReconcile (case of same change on both versions)
            if (IsGeometryEqual(commonAncestorFeature.ShapeCopy,preReconcileFeature.ShapeCopy) || 
                IsGeometryEqual(commonAncestorFeature.ShapeCopy, reconcileFeature.ShapeCopy) ||
              IsGeometryEqual(reconcileFeature.ShapeCopy, preReconcileFeature.ShapeCopy)
              )
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        //判断两个几何形状是否相等
        // Method returning if two shapes are equal to one another.
        private bool IsGeometryEqual(IGeometry shape1, IGeometry shape2)
        {
            if (shape1 == null && shape2 == null)
            {
                return true;
            }
            else if (shape1 == null|| shape2 == null)
            {
                return false;
            }
            else
            {
                IClone clone1 = (IClone)shape1;
                IClone clone2 = (IClone)shape2;
                return clone1.IsEqual(clone2);
            }
        }
    }
}
