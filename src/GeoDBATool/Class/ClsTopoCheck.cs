using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.esriSystem;
using System.Data;

namespace GeoDBATool
{
    class ClsTopoCheck
    {
        private DataTable _DataTable = null;
        public DataTable DataTable
        {
            set { _DataTable = value; }
        }
        /// <summary>
        /// 为数据集创建拓扑
        /// </summary>
        /// <param name="pFeatureDataset">数据集</param>
        /// <param name="topoName">拓扑名称</param>
        /// <param name="outError">异常</param>
        /// <returns>拓扑</returns>
        public ITopology CreateTopo(IFeatureDataset pFeatureDataset, string topoName, double mTolerence, out Exception outError)
        {
            outError = null;

            //topoName =topoName + "_Topology";

            ITopology pTopo = null;
            try
            {
                ITopologyContainer2 pTopoContainer = pFeatureDataset as ITopologyContainer2;

                IWorkspace2 pWS2 = pFeatureDataset.Workspace as IWorkspace2;
                if (pWS2 == null)
                {
                    return null;
                }
                if (pWS2.get_NameExists(esriDatasetType.esriDTTopology, topoName))
                {
                    //若已经存在拓扑，则删除该拓扑
                    RemoveTopo(pFeatureDataset, topoName, out outError);

                    //ITopologyContainer pTopoCon = pFeatureDataset as ITopologyContainer;
                    //pTopo = pTopoCon.get_TopologyByName(topoName);
                }
                //else
                //{
                //try
                //{
                //    ITopology tempTopo = pTopoContainer.get_TopologyByName(topoName);
                //    if (tempTopo != null)
                //    {
                //        RemoveTopo(pFeatureDataset, topoName, out outError);
                //    }
                //}
                //catch
                //{ }
                if (mTolerence == 0)
                {
                    mTolerence = pTopoContainer.DefaultClusterTolerance;
                }
                pTopo = pTopoContainer.CreateTopology(topoName, mTolerence, -1, "");
                //}
            }
            catch (Exception ex)
            {
                ////*********************************************
                ////guozheng 2010-12-24 平安夜  added 系统异常日志
                //if (ModData.SysLog == null) ModData.SysLog = new clsWriteSystemFunctionLog();
                //ModData.SysLog.Write(ex);
                ////**********************************************
                outError = ex;
            }
            return pTopo;
        }
        /// <summary>
        /// 移除拓扑
        /// </summary>
        /// <param name="pFeaDataet">数据集</param>
        /// <param name="pTopoName">拓扑名</param>
        /// <param name="outError">异常</param>
        public void RemoveTopo(IFeatureDataset pFeaDataet, string topoName, out Exception outError)
        {
            outError = null;
            //topoName = topoName + "_Topology";
            IWorkspace pWorkspace = pFeaDataet.Workspace;
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            IFeatureWorkspaceManage pFeatureWorkspaceManage = pFeatureWorkspace as IFeatureWorkspaceManage;
            //if (!((IWorkspaceEdit)pWorkspace).IsBeingEdited())
            //{
            //    //((IWorkspaceEdit)pWorkspace).StartEditing(true);
            //    //((IWorkspaceEdit)pWorkspace).StartEditOperation();
            //}
            //    int iCount = 0;
            //    try
            //    {
            try
            {
                ITopologyContainer pTopoCon = pFeaDataet as ITopologyContainer;
                ITopology pTopo = pTopoCon.get_TopologyByName(topoName);

                //ISchemaLock pLock = pTopo as ISchemaLock;
                //pLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);

                ////删除该拓扑下所有的拓扑规则
                //RemoveTopoRule(pTopo, out outError);
                //if (outError != null) return;

                ////删除该拓扑下左右的拓扑图层
                //RemoveTopoClass(pTopo, out outError);
                //if (outError != null) return;

                //删除拓扑
                IDataset pTopoDt = pTopo as IDataset;
                IName pName = pTopoDt.FullName;
                IDatasetName ptopodatasetname = pName as IDatasetName;
                pFeatureWorkspaceManage.DeleteByName(ptopodatasetname);

                //(pTopo as IDataset).Delete();
                //pTopo.FeatureDataset.Delete();

                //Marshal.ReleaseComObject(pTopo);
                pTopo = null;
                //++iCount;

                //pLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);


            }
            catch (Exception ex)
            {
                ////*********************************************
                ////guozheng 2010-12-24 平安夜  added 系统异常日志
                //if (ModData.SysLog == null) ModData.SysLog = new clsWriteSystemFunctionLog();
                //ModData.SysLog.Write(ex);
                ////**********************************************
                outError = ex;
            }
            //}
            //finally
            //{
            //    if (((IWorkspaceEdit)pWorkspace).IsBeingEdited())
            //    {
            //        if (iCount > 0)
            //        {
            //            ((IWorkspaceEdit)pWorkspace).StopEditOperation();
            //            ((IWorkspaceEdit)pWorkspace).StopEditing(true);
            //        }
            //        {
            //            ((IWorkspaceEdit)pWorkspace).AbortEditOperation();
            //            ((IWorkspaceEdit)pWorkspace).StopEditing(true);
            //        }
            //    }
            //}

        }

        /// <summary>
        /// 验证拓扑
        /// </summary>
        /// <param name="topology">拓扑</param>
        /// <param name="envelope">验证拓扑范围</param>
        /// <param name="errEx">异常</param>
        public void ValidateTopology(ITopology topology, IEnvelope envelope, out Exception errEx)
        {
            errEx = null;
            try
            {
                ISegmentCollection pLocation = new PolygonClass();
                pLocation.SetRectangle(envelope);
                IPolygon pPoly = topology.get_DirtyArea(pLocation as IPolygon);
                IEnvelope pAreaValidated = topology.ValidateTopology(pPoly.Envelope);
            }
            catch (Exception err)
            {
                ////*********************************************
                ////guozheng 2010-12-24 平安夜  added 系统异常日志
                //if (ModData.SysLog == null) ModData.SysLog = new clsWriteSystemFunctionLog();
                //ModData.SysLog.Write(err);
                ////**********************************************
                errEx = err;
            }
        }
        /// <summary>
        /// 获得错误定位信息  chenayfei add 20101222
        /// </summary>
        /// <param name="pErrorFea">错误要素</param>
        /// <param name="pMapX">定位点x</param>
        /// <param name="pMapY">定位点y</param>
        /// <param name="errSeriaFea">错误几何形状解析后的字符串</param>
        public void GetZoomErrFea(IFeature pFea, out double pMapX, out double pMapY, out string errSeriaFea, out string errCoor)
        {
            pMapX = 0;
            pMapY = 0;
            errSeriaFea = "";
            errCoor = "";
            IPoint pPoint = null; //定位点

            //IFeature pFea = pErrorFea as IFeature;  //将错误要素转化为要素
            if (pFea != null)
            {
                if (pFea.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                {
                    //错误要素为点，则记录定位点
                    pPoint = pFea.Shape as IPoint;
                    errCoor = pPoint.X + "," + pPoint.Y;

                }
                else
                {
                    if (pFea.Shape.GeometryType == esriGeometryType.esriGeometryPolyline || pFea.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        //为线或者面形状，则取错误形状的其中一个点
                        pPoint = GetPointofFeature(pFea);

                        IGeometry pGeo = null;  //用来保存错误的形状
                        pGeo = pFea.Shape as IGeometry;

                        byte[] xmlByte = xmlSerializer(pGeo as object);
                        errSeriaFea = Convert.ToBase64String(xmlByte);

                        errCoor = GetCoor(pFea.Shape);
                    }
                }
            }
            if (pPoint != null)
            {
                pMapX = pPoint.X;
                pMapY = pPoint.Y;
            }
        }
        /// <summary>
        /// 获取错误要素的定位点(暂时为要素第一个点)  chenyafei 20101202
        /// </summary>
        /// <param name="feature">错误要素</param>
        /// <returns>错误要素形状的第一个点</returns>
        public IPoint GetPointofFeature(IFeature feature)
        {
            IPointCollection pntCol = feature.Shape as IPointCollection;
            if (pntCol == null) return null;
            return pntCol.get_Point(0);
        }

        /// <summary>
        /// 序列化(将对象序列化成字符串)
        /// </summary>
        /// <param name="xmlByte">序列化字节</param>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static byte[] xmlSerializer(object obj)
        {
            try
            {
                byte[] xmlByte = null;//保存序列化后的字节
                //判断是否支持IPersistStream接口,只有支持该接口的对象才能进行序列化
                if (obj is ESRI.ArcGIS.esriSystem.IPersistStream)
                {
                    ESRI.ArcGIS.esriSystem.IPersistStream pStream = obj as ESRI.ArcGIS.esriSystem.IPersistStream;

                    ESRI.ArcGIS.esriSystem.IXMLStream xmlStream = new ESRI.ArcGIS.esriSystem.XMLStreamClass();

                    pStream.Save(xmlStream as ESRI.ArcGIS.esriSystem.IStream, 0);

                    xmlByte = xmlStream.SaveToBytes();
                }
                return xmlByte;
            }
            catch (Exception ex)
            {
                ////*********************************************
                ////guozheng 2010-12-24 平安夜  added 系统异常日志
                //if (ModData.SysLog == null) ModData.SysLog = new clsWriteSystemFunctionLog();
                //ModData.SysLog.Write(ex);
                ////**********************************************
                return null;
            }
        }

        /// <summary>
        /// chenyafei  20110420  add :获得要素的坐标串
        /// </summary>
        /// <param name="pFea"></param>
        /// <returns></returns>
        private string GetCoor(IGeometry pInGeo)
        {
            if (pInGeo.IsEmpty) return "";
            string CoorStr = "";
            if (pInGeo.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                IPoint mPnt = pInGeo as IPoint;
                CoorStr = mPnt.X + "," + mPnt.Y;
            }
            else if (pInGeo.GeometryType == esriGeometryType.esriGeometryPolyline)
            {
                IPointCollection pPntColl = pInGeo as IPointCollection;
                for (int i = 0; i < pPntColl.PointCount; i++)
                {
                    IPoint pPnt = pPntColl.get_Point(i);
                    CoorStr += pPnt.X + "," + pPnt.Y + ";";
                }
                if (CoorStr != "")
                {
                    CoorStr = CoorStr.Substring(0, CoorStr.Length - 1);
                }
            }
            else if (pInGeo.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                IPolygon4 pPolygon4 = pInGeo as IPolygon4;
                int pRingCount = pPolygon4.ExteriorRingCount;
                if (pRingCount > 0)
                {
                    IGeometryBag pGeoBag = pPolygon4.ExteriorRingBag;
                    IEnumGeometry pEnumGeo = pGeoBag as IEnumGeometry;
                    pEnumGeo.Reset();
                    IGeometry mGeo = pEnumGeo.Next();
                    IPointCollection pPntColl = mGeo as IPointCollection;
                    for (int i = 0; i < pPntColl.PointCount; i++)
                    {
                        IPoint pPnt = pPntColl.get_Point(i);
                        CoorStr += pPnt.X + "," + pPnt.Y + ";";
                    }
                    if (CoorStr != "")
                    {
                        CoorStr = CoorStr.Substring(0, CoorStr.Length - 1);
                    }
                }
            }
            return CoorStr;
        }
        /// <summary>
        /// 将要素类添加到拓扑中，并为要素类添加拓扑规则，针对针对特殊的要素类检查
        /// </summary>
        /// <param name="pTopo">拓扑</param>
        /// <param name="pFeatureDataset">数据集</param>
        /// <param name="pFeaClsName">图层名</param>
        /// <param name="pTopoRuleType">拓扑规则</param>
        /// <param name="outError">异常</param>
        public void AddRuleandClasstoTopology(ITopology pTopo, IFeatureDataset pFeatureDataset, string pFeaClsName, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;
            AddClasstoTopology(pTopo, pFeatureDataset, pFeaClsName, out outError);
            if (outError != null) return;

            AddRuletoTopology(pTopo, pTopoRuleType, out outError);
            if (outError != null) return;
        }
        /// <summary>
        /// 将要素类添加到拓扑中，针对针对特殊的要素类检查
        /// </summary>
        /// <param name="pTopo">拓扑</param>
        /// <param name="pFeatureDataset">数据集</param>
        /// <param name="pFeaClsName">图层名</param>
        /// <param name="outError">异常</param>
        public void AddClasstoTopology(ITopology pTopo, IFeatureDataset pFeatureDataset, string pFeaClsName, out Exception outError)
        {
            outError = null;

            try
            {
                ITopologyRuleContainer pTopoRulrContainer = pTopo as ITopologyRuleContainer;
                List<IDataset> LstDataSet = ModDBOperator.GetAllFeaCls(pFeatureDataset);
                bool b = false;

                //遍历要素类，将符合条件的要素类添加到拓扑中
                foreach (IDataset pDt in LstDataSet)
                {
                    IFeatureClass pFeaCls = pDt as IFeatureClass;
                    if (pFeaCls == null) continue;
                    if (pFeaCls.FeatureType != esriFeatureType.esriFTSimple) continue;
                    string pFeaName = pDt.Name.Trim();
                    if (pFeaName.Contains("."))
                    {
                        pFeaName = pFeaName.Substring(pFeaName.IndexOf('.') + 1);
                    }
                    if (pFeaName == pFeaClsName)
                    {
                        pTopo.AddClass(pFeaCls as IClass, 5, 1, 1, false);
                        b = true;
                        break;
                    }
                }
                if (b == false)
                {
                    outError = new Exception("要进行检查的要素类不存在！");
                    ////*********************************************
                    ////guozheng 2010-12-24 平安夜  added 系统异常日志
                    //if (ModData.SysLog == null) ModData.SysLog = new clsWriteSystemFunctionLog();
                    //ModData.SysLog.Write(outError);
                    ////**********************************************
                    return;
                }
            }
            catch (Exception ex)
            {
                ////*********************************************
                ////guozheng 2010-12-24 平安夜  added 系统异常日志
                //if (ModData.SysLog == null) ModData.SysLog = new clsWriteSystemFunctionLog();
                //ModData.SysLog.Write(ex);
                ////**********************************************
                outError = ex;
            }
        }
        /// <summary>
        /// 为要素类添加拓扑规则，针对针对特殊的要素类检查
        /// </summary>
        /// <param name="pTopo">拓扑</param>
        /// <param name="pTopoRuleType">拓扑规则</param>
        /// <param name="outError">异常</param>
        public void AddRuletoTopology(ITopology pTopo, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;

            try
            {
                ITopologyRuleContainer pRuleContainer = pTopo as ITopologyRuleContainer;
                IFeatureClassContainer pFeaClsContainer = pTopo as IFeatureClassContainer;
                IEnumFeatureClass pEnumFeaCls = pFeaClsContainer.Classes;
                pEnumFeaCls.Reset();
                IFeatureClass pFeaCls = pEnumFeaCls.Next();

                //设置拓扑规则
                while (pFeaCls != null)
                {
                    ITopologyRule pTopoRule = new TopologyRuleClass();
                    pTopoRule.TopologyRuleType = pTopoRuleType;
                    pTopoRule.Name = (pFeaCls as IDataset).Name;
                    pTopoRule.OriginClassID = pFeaCls.FeatureClassID;
                    pTopoRule.AllOriginSubtypes = true;
                    pRuleContainer.AddRule(pTopoRule);
                    pFeaCls = pEnumFeaCls.Next();
                }
            }
            catch (Exception ex)
            {
                ////*********************************************
                ////guozheng 2010-12-24 平安夜  added 系统异常日志
                //if (ModData.SysLog == null) ModData.SysLog = new clsWriteSystemFunctionLog();
                //ModData.SysLog.Write(ex);
                ////**********************************************
                outError = ex;
            }
        }

        /// <summary>
        /// 面重叠检查错误列表  同层面检查  chenyafei  添加20101208
        /// </summary>
        /// <param name="pFeaDatset">数据集</param>
        /// <param name="pTopo">拓扑</param>
        /// <param name="pTopoRuleType">拓扑规则</param>
        /// <param name="pOrgFeaClsNameDic">源图层名</param>
        /// <param name="pEnvelop">拓扑验证范围</param>
        /// <param name="pDataGrid">检查结果列表显示</param>
        /// <param name="outError"></param>
        public void GetAreaNoOverlopErrorList(IFeatureDataset pFeaDatset, ITopology pTopo, IEnvelope pEnvelop, DataGridViewX pDataGrid, out Exception outError)
        {
            outError = null;
            try
            {
                IFeatureClass oriFeaCls = null;     //源要素类
                IFeatureClass desFeaCls = null;     //目标要素类

                //依照拓扑规则获取错误要素
                IErrorFeatureContainer pErrorFeaCon = pTopo as IErrorFeatureContainer;
                ITopologyRuleContainer pTopoRuleCon = pTopo as ITopologyRuleContainer;
                IEnumRule pEnumRule = pTopoRuleCon.Rules;
                pEnumRule.Reset();
                ITopologyRule pTopoRule = pEnumRule.Next() as ITopologyRule;
                //遍历拓扑规则
                while (pTopoRule != null)
                {
                    //if (pTopoRule.TopologyRuleType != pTopoRuleType)
                    //{
                    //    pTopoRule = pEnumRule.Next() as ITopologyRule;
                    //    continue;
                    //}
                    IEnumTopologyErrorFeature pEnumErrorFea = null;
                    try
                    {

                        pEnumErrorFea = pErrorFeaCon.get_ErrorFeatures(
                            (pFeaDatset as IGeoDataset).SpatialReference, pTopoRule, pEnvelop, true, false);
                    }
                    catch (Exception ex)
                    {
                        ////*********************************************
                        ////guozheng 2010-12-24 平安夜  added 系统异常日志
                        //if (ModData.SysLog == null) ModData.SysLog = new clsWriteSystemFunctionLog();
                        //ModData.SysLog.Write(ex);
                        ////**********************************************
                        RemoveTopo(pFeaDatset, pFeaDatset.Name, out outError);
                        outError = ex;
                        return;
                    }
                    if (pEnumErrorFea == null)
                    {
                        pTopoRule = pEnumRule.Next() as ITopologyRule;
                        continue;
                    }

                    //进度条初始值
                    //ModData.m_ProgressBarDetail.Minimum = 0;
                    //ModData.m_ProgressBarDetail.Value = 0;
                    //ModData.m_ProgressBarDetail.Maximum = ModOperator.GetErrCount(pEnumErrorFea);
                    int pValue = 0;
                    //Application.DoEvents();

                    pEnumErrorFea = pErrorFeaCon.get_ErrorFeatures(
                           (pFeaDatset as IGeoDataset).SpatialReference, pTopoRule, pEnvelop, true, false);

                    ITopologyErrorFeature pErrorFea = pEnumErrorFea.Next();
                    //遍历错误要素，进行错误输出
                    while (pErrorFea != null)
                    {
                        double pMapx = 0;                                  //错误定位点x
                        double pMapy = 0;                                  //错误定位点y
                        string errFeaGeoStr = "";                          //错误几何信息
                        string errCoorStr = "";
                        string pFeaClsName = "";                           //要素类名称
                        int desID = pErrorFea.DestinationOID;               //目标要素ID
                        int oriID = pErrorFea.OriginOID;                    //源要素类ID
                        IFeature oriFeature = null;                         //源要素
                        IFeature desFeature = null;                         //目标要素
                        int oriFeaClsID = pErrorFea.OriginClassID;          //源要素类ID
                        int desFeaClsID = pErrorFea.DestinationClassID;     //目标要素类ID
                        string errDes = "";                                 //错误描述

                        try
                        {
                            //若要素类为空，则说明是对数据集中的所有的同层面进行重叠检查
                            oriFeaCls = (pTopo as IFeatureClassContainer).get_ClassByID(oriFeaClsID);
                            desFeaCls = oriFeaCls;
                        }
                        catch
                        { }

                        if (oriFeaCls != null && oriID > 0)
                        {
                            oriFeature = oriFeaCls.GetFeature(oriID);
                        }
                        else
                        {
                            //源要素OID
                            oriID = -1;
                        }
                        if (desFeaCls != null && desID > 0)
                        {
                            desFeature = desFeaCls.GetFeature(desID);
                        }
                        else
                        {
                            //目标要素OID
                            desID = -1;
                        }
                        double pMinArea = 0;       //面积最小值
                        double pMaxArea = 0;       //重叠面积最大值
                        pFeaClsName = (oriFeaCls as IDataset).Name;
                        if (pFeaClsName.Contains("."))
                        {
                            pFeaClsName = pFeaClsName.Substring(pFeaClsName.IndexOf('.') + 1);
                        }

                        
                        //获得错误几何信息
                        GetZoomErrFea(pErrorFea as IFeature, out pMapx, out pMapy, out errFeaGeoStr, out errCoorStr);

                        //目标数据库对应到元数据库中的OID
                        int dicDesOID = -1;
                        if (desFeature != null)
                        {
                            int index = desFeature.Fields.FindField(desFeaCls.OIDFieldName);
                            if (index != -1)
                            {

                                if (desFeature.get_Value(index).ToString().Trim() != "")
                                {
                                    dicDesOID = Convert.ToInt32(desFeature.get_Value(index).ToString().Trim());
                                }
                            }
                        }
                        string errType = "";
                        if (pTopoRule.TopologyRuleType == esriTopologyRuleType.esriTRTAreaNoOverlap)
                        {
                            errDes = "OID为" + oriID + "的小班与OID为" + dicDesOID + "的小班之间有重叠";
                            errType = "面重叠检查";
                        }
                        else if (pTopoRule.TopologyRuleType == esriTopologyRuleType.esriTRTAreaNoGaps)
                        {
                            errDes = "小班之间有缝隙";
                            errType = "面缝隙检查";
                        }

                        //处理错误结果
                        ProcErrorList(errType,pFeaDatset, pFeaClsName, oriID, pFeaClsName, dicDesOID, pMapx, pMapy, errFeaGeoStr, pDataGrid, errDes, errCoorStr);
                        //ProcErrorList(pFeaDatset, pFeaClsName, oriFeature, pFeaClsName, desID, pDataGrid, EnumCheckType.面重叠检查, pErrType, errDes);

                        //错误数加1
                        //ModData.m_ErrCount++;
                        //m_ErrCount++;
                        //m_LabelRes.Text = "同层面重叠检查错误:" + m_ErrCount + "个";

                        //进度条
                        pValue++;
                        //ModData.m_ProgressBarDetail.Value = pValue;
                        //Application.DoEvents();   //实时处理windows消息

                        pErrorFea = pEnumErrorFea.Next();
                    }

                    pTopoRule = pEnumRule.Next() as ITopologyRule;
                }
            }
            catch (Exception ex)
            {
                ////*********************************************
                ////guozheng 2010-12-24 平安夜  added 系统异常日志
                //if (ModData.SysLog == null) ModData.SysLog = new clsWriteSystemFunctionLog();
                //ModData.SysLog.Write(ex);
                ////**********************************************
                outError = ex;
            }
        }
        private void ProcErrorList(string errType,IFeatureDataset pFeaDatset, string oriFeaClsName, int iOrgFeaOID, string desFeaClsName, int iDesFeaOID, double pMapx, double pMapy, string errFeaGeoStr, DataGridViewX pDataGrid, string pErrDes, string pErrCoor)
        {
            DataRow newRow = _DataTable.NewRow();
            ///检查功能名、检查时间、检查人屏蔽掉  ZQ 20111020 modify
            newRow["错误类型"] = errType;
            newRow["错误描述"] = pErrDes;
            newRow["数据图层1"] = oriFeaClsName;
            newRow["要素OID1"] = iOrgFeaOID;
            newRow["数据图层2"] = desFeaClsName;
            newRow["要素OID2"] = iDesFeaOID;
            newRow["检查时间"] = System.DateTime.Now.ToString();
            newRow["定位点X"] = pMapx;
            newRow["定位点Y"] = pMapy;
            newRow["数据文件名"] = "";
            newRow["检查人"] = "";
            newRow["错误几何形状"] = errFeaGeoStr;
            newRow["错误坐标串"] = pErrCoor;

            _DataTable.Rows.Add(newRow);
        }
    }
}
