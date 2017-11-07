using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Xml;
using System.Collections;
using System.Windows.Forms;

namespace GeoDataChecker
{
    public static class ModCommonFunction
    {

        //为数据集创建拓扑
        public static ITopology CreateTopology(IFeatureDataset featureDataset, out Exception outErr)
        {
            outErr = null;
            ITopology topology = null;
            try
            {
                ITopologyContainer2 topologyContainer = (ITopologyContainer2)featureDataset;
                try
                {
                    ITopology tempTopo = topologyContainer.get_TopologyByName(featureDataset.Name );
                    if (tempTopo != null)
                    {
                        RemoveTopology(featureDataset, out outErr);
                    }
                }
                catch
                { }
                topology = topologyContainer.CreateTopology(featureDataset.Name,
                                                            topologyContainer.DefaultClusterTolerance, -1, "");

            }
            catch (Exception err)
            {
                outErr = err;
            }

            return topology;
        }

        //移出拓扑
        public static void RemoveTopology(IFeatureDataset featureDataset, out Exception outErr)
        {
            outErr = null;
            try
            {
                ITopologyContainer topologyContainer = (ITopologyContainer)featureDataset;
                ITopology top = topologyContainer.get_TopologyByName(featureDataset.Name);

                //删除该拓扑下的所有拓扑规则
                RemoveTopologyRule(top, out outErr);
                if (outErr != null) return;

                //删除该拓扑下的所有拓扑图层
                RemoveTopologyClass(top, out outErr);
                if (outErr != null) return;

                //删除拓扑
                (top as IDataset).Delete();
            }
            catch (Exception err)
            {
                outErr = err;
            }

        }

        //移出拓扑类
        public static void RemoveTopologyClass(ITopology topology, out Exception outErr)
        {
            outErr = null;
            try
            {
                //删除该拓扑下的所有拓扑图层
                IFeatureClassContainer topFcList = topology as IFeatureClassContainer;
                for (int d = topFcList.ClassCount - 1; d >= 0; d--)
                {
                    topology.RemoveClass(topFcList.get_Class(d) as IClass);
                }
            }
            catch (Exception err)
            {
                outErr = err;
            }
        }

        //移出拓扑规则
        public static void RemoveTopologyRule(ITopology topology, out Exception outErr)
        {
            outErr = null;
            try
            {
                //删除该拓扑下的所有拓扑规则
                ITopologyRuleContainer topruleList = topology as ITopologyRuleContainer;
                IEnumRule er = topruleList.Rules;
                er.Reset();
                IRule r = er.Next();
                while (r != null && r is ITopologyRule)
                {
                    topruleList.DeleteRule(r as ITopologyRule);
                    r = er.Next();
                }
            }
            catch (Exception err)
            {
                outErr = err;
            }
        }

        //将要素类添加到拓扑中并创建拓扑规则
        public static void AddClassToTopology(ITopology topology, IFeatureDataset featureDataset, esriGeometryType geometryType, out Exception errEx)
        {
            errEx = null;
            try
            {
                ITopologyRuleContainer topologyRuleContainer = topology as ITopologyRuleContainer;
                List<IDataset> featureClasses = GetFeatureClass(featureDataset);
                foreach (IDataset dataset in featureClasses)
                {
                    IFeatureClass featureClass = dataset as IFeatureClass;
                    if (featureClass == null) continue;
                    if (featureClass.FeatureType != esriFeatureType.esriFTSimple) continue;
                    if (geometryType != esriGeometryType.esriGeometryNull)
                    {
                        if (featureClass.ShapeType != geometryType) continue;
                    }

                    topology.AddClass(featureClass, 5, 1, 1, false);
                }
            }
            catch (Exception err)
            {
                errEx = err;
            }
        }

        //将要素类添加到拓扑中并创建拓扑规则
        public static void AddClassAndRuleToTopology(ITopology topology, IFeatureDataset featureDataset, esriGeometryType geometryType, esriTopologyRuleType topologyRuleType, out Exception errEx)
        {
            errEx = null;
            try
            {
                ITopologyRuleContainer topologyRuleContainer = topology as ITopologyRuleContainer;
                List<IDataset> featureClasses = GetFeatureClass(featureDataset);
                foreach (IDataset dataset in featureClasses)
                {
                    IFeatureClass featureClass = dataset as IFeatureClass;
                    if (featureClass == null) continue;
                    if (featureClass.ShapeType != geometryType || featureClass.FeatureType != esriFeatureType.esriFTSimple) continue;

                    topology.AddClass(featureClass, 5, 1, 1, false);

                    //为要素类创建拓扑规则并添加到拓扑中
                    ITopologyRule topologyRule = new TopologyRuleClass();
                    topologyRule.TopologyRuleType = topologyRuleType;
                    topologyRule.Name = (featureClass as IDataset).Name;
                    topologyRule.OriginClassID = featureClass.FeatureClassID;
                    topologyRule.AllOriginSubtypes = true;
                    topologyRuleContainer.AddRule(topologyRule);
                }
            }
            catch (Exception err)
            {
                errEx = err;
            }
        }

        //为要素类创建拓扑规则添加到拓扑中
        public static void AddRuleToTopology(ITopology topology, esriGeometryType geometryType, esriTopologyRuleType topologyRuleType, out Exception errEx)
        {
            errEx = null;
            try
            {
                ITopologyRuleContainer topologyRuleContainer = topology as ITopologyRuleContainer;
                IFeatureClassContainer featureClassContainer = topology as IFeatureClassContainer;
                IEnumFeatureClass enumFeatureClass = featureClassContainer.Classes;
                enumFeatureClass.Reset();
                IFeatureClass featureClass = enumFeatureClass.Next();
                while (featureClass != null)
                {
                    if (featureClass.ShapeType == geometryType)
                    {
                        //为要素类创建拓扑规则并添加到拓扑中
                        ITopologyRule topologyRule = new TopologyRuleClass();
                        topologyRule.TopologyRuleType = topologyRuleType;
                        topologyRule.Name = (featureClass as IDataset).Name;
                        topologyRule.OriginClassID = featureClass.FeatureClassID;
                        topologyRule.AllOriginSubtypes = true;
                        topologyRuleContainer.AddRule(topologyRule);
                    }

                    featureClass = enumFeatureClass.Next();
                }
            }
            catch (Exception err)
            {
                errEx = err;
            }
        }

        //验证拓扑
        public static void ValidateTopology(ITopology topology, IEnvelope envelope, out Exception errEx)
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
                errEx = err;
            }
        }

        /// <summary>
        /// 获取某一要素集合下FC
        /// </summary>
        /// <param name="pFeaDsName">要素集IFeatureDataset</param>
        /// <returns></returns>
        public static List<IDataset> GetFeatureClass(IFeatureDataset pFeaDs)
        {
            List<IDataset> FCs = new List<IDataset>();

            IEnumDataset pEnumDs = pFeaDs.Subsets;
            IDataset pDs = pEnumDs.Next();
            while (pDs != null)
            {
                FCs.Add(pDs);
                pDs = pEnumDs.Next();
            }
            return FCs;
        }

        //获取错误要素的定位点(暂时为要素第一个点)
        public static IPoint GetPntOfFeature(IFeature feature)
        {
            IPointCollection pntCol = feature.Shape as IPointCollection;
            if (pntCol == null) return null;
            return pntCol.get_Point(0);
        }
    }

    public static class ModDataCheck
    {
        //一般拓扑检查
        public static void CommonTopologyCheck(IArcgisDataCheckHook hook, IDataCheckRealize dataCheckRealize, esriGeometryType geometryType, esriTopologyRuleType topologyRuleType)
        {
            IArcgisDataCheckParaSet dataCheckParaSet = hook.DataCheckParaSet as IArcgisDataCheckParaSet;
            if (dataCheckParaSet == null) return;
            if (dataCheckParaSet.Workspace == null) return;

            //获取所有数据集
            SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet(dataCheckParaSet.Workspace);
            List<string> featureDatasetNames = sysGisDataSet.GetAllFeatureDatasetNames();
            if (featureDatasetNames.Count == 0) return;

            //设置进度条
            ProgressChangeEvent eInfo = new ProgressChangeEvent();
            eInfo.Max = featureDatasetNames.Count;
            int pValue = 0;

            //设置进度条
            //int pMax = featureDatasetNames.Count;
            //int pValue = 0;
            //GeoDataChecker._CheckForm.Invoke(new GeoDataChecker.IntiProgressBar(GeoDataChecker.intiaProgress), new object[] { pMax });

            Exception errEx = null;
            foreach (string name in featureDatasetNames)
            {
                IFeatureDataset featureDataset = sysGisDataSet.GetFeatureDataset(name, out errEx);
                if (errEx != null) continue;

                CommonTopologyCheckClass commonTopologyCheckClass = new CommonTopologyCheckClass();
                commonTopologyCheckClass.DataErrTreat += new DataErrTreatHandle(dataCheckRealize.DataCheckRealize_DataErrTreat);
                commonTopologyCheckClass.TopologyCheck(hook, featureDataset, geometryType, topologyRuleType, out errEx);
                if (errEx != null)
                {
                }

                //进度条加1
                pValue++;
                eInfo.Value = pValue;
                //GeoDataChecker._CheckForm.Invoke(new GeoDataChecker.GEODataCheckerProgressShow(GeoDataChecker.GeoDataChecker_ProgressShow), new object[] { (object)GeoDataChecker._ProgressBarInner, eInfo });
                GeoDataChecker.GeoDataChecker_ProgressShow((object)GeoDataChecker._ProgressBarInner, eInfo);
              
                //进度条加1
                //pValue++;
                //GeoDataChecker._CheckForm.Invoke(new GeoDataChecker.ChangeProgressBar(GeoDataChecker.changeProgress), new object[] {pValue });
           
            }
        }

        //要素重复性检查(依赖拓扑)
        public static void DuplicateCheck(IArcgisDataCheckHook hook, IDataCheckRealize dataCheckRealize, esriGeometryType geometryType)
        {
            IArcgisDataCheckParaSet dataCheckParaSet = hook.DataCheckParaSet as IArcgisDataCheckParaSet;
            if (dataCheckParaSet == null) return;
            if (dataCheckParaSet.Workspace == null) return;

            //获取所有数据集
            SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet(dataCheckParaSet.Workspace);
            List<string> featureDatasetNames = sysGisDataSet.GetAllFeatureDatasetNames();
            if (featureDatasetNames.Count == 0) return;
            Exception errEx = null;

            //设置进度条
            ProgressChangeEvent eInfo = new ProgressChangeEvent();
            eInfo.Max = featureDatasetNames.Count;
            int pValue = 0;

            //设置进度条
            //int pMax = featureDatasetNames.Count;
            //int pValue = 0;
            //GeoDataChecker._CheckForm.Invoke(new GeoDataChecker.IntiProgressBar(GeoDataChecker.intiaProgress), new object[] { pMax });

            foreach (string name in featureDatasetNames)
            {
                IFeatureDataset featureDataset = sysGisDataSet.GetFeatureDataset(name, out errEx);
                if (errEx != null) continue;

                DuplicateCheckClass duplicateCheckClass = new DuplicateCheckClass();
                duplicateCheckClass.DataErrTreat += new DataErrTreatHandle(dataCheckRealize.DataCheckRealize_DataErrTreat);
                duplicateCheckClass.DuplicateCheck(hook, featureDataset, geometryType, out errEx);
                if (errEx != null)
                {
                }

                //进度条加1
                pValue++;
                eInfo.Value = pValue;
                //GeoDataChecker._CheckForm.Invoke(new GeoDataChecker.GEODataCheckerProgressShow(GeoDataChecker.GeoDataChecker_ProgressShow), new object[] { (object)GeoDataChecker._ProgressBarInner, eInfo });
                GeoDataChecker.GeoDataChecker_ProgressShow((object)GeoDataChecker._ProgressBarInner, eInfo);
              
                ////进度条加1
                //pValue++;
                //GeoDataChecker._CheckForm.Invoke(new GeoDataChecker.ChangeProgressBar(GeoDataChecker.changeProgress), new object[] {pValue});
           
            }
        }
    }

    //一般拓扑检查
    public class CommonTopologyCheckClass
    {
        public event DataErrTreatHandle DataErrTreat;

        //进行拓扑检查
        public void TopologyCheck(IArcgisDataCheckHook hook, IFeatureDataset featureDataset, esriGeometryType geometryType, esriTopologyRuleType topologyRuleType, out Exception errEx)
        {
            errEx = null;
            ITopology topology = ModCommonFunction.CreateTopology(featureDataset, out errEx);
            if (topology == null)
            {
                ModCommonFunction.RemoveTopology(featureDataset, out errEx);
                return;
            }

            //将线要素类添加到拓扑中并创建拓扑规则
            ModCommonFunction.AddClassAndRuleToTopology(topology, featureDataset, geometryType, topologyRuleType, out errEx);
            if (errEx != null)
            {
                ModCommonFunction.RemoveTopology(featureDataset, out errEx);
                return;
            }

            //验证拓扑
            ModCommonFunction.ValidateTopology(topology, (featureDataset as IGeoDataset).Extent, out errEx);
            if (errEx != null)
            {
                ModCommonFunction.RemoveTopology(featureDataset, out errEx);
                return;
            }

            //依照拓扑规则获取错误要素
            IErrorFeatureContainer pErrFeatCon = topology as IErrorFeatureContainer;
            ITopologyRuleContainer topologyRuleContainer = topology as ITopologyRuleContainer;
            IEnumRule pEnumRule = topologyRuleContainer.Rules;
            pEnumRule.Reset();
            ITopologyRule rule = pEnumRule.Next() as ITopologyRule;
            while (rule != null)
            {
                IEnumTopologyErrorFeature pEnumTopoErrFeat=null;
                try
                {
                    pEnumTopoErrFeat = pErrFeatCon.get_ErrorFeatures((featureDataset as IGeoDataset).SpatialReference, rule as ITopologyRule, (featureDataset as IGeoDataset).Extent, true, false);
                }
                catch
                {

                }

                if (pEnumTopoErrFeat == null)
                {
                    rule = pEnumRule.Next() as ITopologyRule;
                    continue;
                }

                ITopologyErrorFeature topoErrFeat = pEnumTopoErrFeat.Next();
                while (topoErrFeat != null)
                {
                    //求错误要素的定位点

                    double localPntX = 0;
                    double localPntY = 0;
                    IFeature originFeat = (topology as IFeatureClassContainer).get_ClassByID(topoErrFeat.OriginClassID).GetFeature(topoErrFeat.OriginOID);
                    IPoint pnt = ModCommonFunction.GetPntOfFeature(originFeat);
                    if (pnt != null)
                    {
                        localPntX = pnt.X;
                        localPntY = pnt.Y;
                    }
                    # region 错误描述信息分类保存
                    int errID = 0;              //错误ID，
                    string errDes = "";         //错误描述
                    switch (topologyRuleType)
                    {
                        case esriTopologyRuleType.esriTRTLineNoSelfIntersect:
                            //线自相交
                            errID = enumErrorType.线自相交检查.GetHashCode();
                            errDes = "线不能和自己相交";
                            break;
                        case esriTopologyRuleType.esriTRTLineNoSelfOverlap:
                            //线自重叠
                            errID = enumErrorType.线自重叠检查.GetHashCode();
                            errDes = "线不能和自己重叠";
                            break;
                        case esriTopologyRuleType.esriTRTLineNoPseudos:
                            //线伪节点
                            errID = enumErrorType.线存在伪节点.GetHashCode();
                            errDes = "不能有伪节点，即线段的端点不能仅仅是两个端点的接触点（自身首尾接触的除外）";
                            break;
                        case esriTopologyRuleType.esriTRTAreaNoOverlap:
                            //同层面重叠
                            errID = enumErrorType.同层面重叠检查.GetHashCode();
                            errDes = "同一面要素类中的多边形要素不能互相重叠";
                            break;
                        case esriTopologyRuleType.esriTRTLineNoDangles:
                            //线存在悬挂点
                            errID = enumErrorType.线存在悬挂点.GetHashCode();
                            errDes = "不允许线要素有悬结点，即每一条线段的端点都不能孤立";
                            break;
                        case esriTopologyRuleType.esriTRTLineNoMultipart:
                            //简单线检查
                            errID = enumErrorType.简单线检查.GetHashCode();
                            errDes = "线必须为简单线，不能相互接触、重叠";
                            break;
                        default:
                            break;
                    }
                    #endregion

                    //用来保存错误结果
                    List<object> errorLst = new List<object>();
                    errorLst.Add("一般拓扑检查");
                    errorLst.Add("一般拓扑检查");
                    errorLst.Add((featureDataset as IDataset).Workspace.PathName);
                    errorLst.Add(errID);
                    errorLst.Add(errDes);
                    errorLst.Add(localPntX);
                    errorLst.Add(localPntY);
                    errorLst.Add(((topology as IFeatureClassContainer).get_ClassByID(topoErrFeat.OriginClassID) as IDataset).Name);
                    errorLst.Add(topoErrFeat.OriginOID);
                    errorLst.Add("");
                    errorLst.Add(-1);
                    errorLst.Add(false);
                    errorLst.Add(System.DateTime.Now.ToString());
                    //传递错误日志
                    IDataErrInfo dataErrInfo = new DataErrInfo(errorLst);
                    DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                    DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);

                    topoErrFeat = pEnumTopoErrFeat.Next();
                }

                rule = pEnumRule.Next() as ITopologyRule;
            }

            //删除拓扑
            ModCommonFunction.RemoveTopology(featureDataset, out errEx);
            if (errEx != null) return;
        }

        //一般的拓扑检查，针对异层检查，包括源和目标层
        public void OrdinaryTopoCheck(IArcgisDataCheckHook hook, IFeatureDataset pFeaDatset, string oriFeaClsName, string desFeaClsName, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;
            Exception exError = null;
            //创建拓扑
            ITopology pTopo = TopologyCheckClass.CreateTopo(pFeaDatset, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            //将要素类添加到拓扑中并创建拓扑规则
            TopologyCheckClass.AddRuleandClasstoTopology(pTopo, pFeaDatset, oriFeaClsName, desFeaClsName, pTopoRuleType, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            //验证拓扑
            TopologyCheckClass.ValidateTopology(pTopo, (pFeaDatset as IGeoDataset).Extent, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            //获得错误列表
            GetErrorList(hook,pFeaDatset, pTopo, pTopoRuleType,oriFeaClsName,desFeaClsName, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
            if (outError != null) return;
        }

        //一般的拓扑检查，针对异层检查，包括源和目标层(列表)
        public void OrdinaryDicTopoCheck(IArcgisDataCheckHook hook, IFeatureDataset pFeaDatset, List<string> feaClsNameDic, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;
            Exception exError = null;
            //创建拓扑
            ITopology pTopo = TopologyCheckClass.CreateTopo(pFeaDatset, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            //将要素类添加到拓扑中并创建拓扑规则
            TopologyCheckClass.AddDicRuleandClasstoTopology(pTopo, pFeaDatset, feaClsNameDic, pTopoRuleType, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                //outError = exError;
                return;
            }

            //验证拓扑
            TopologyCheckClass.ValidateTopology(pTopo, (pFeaDatset as IGeoDataset).Extent, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            //获得错误列表
            GetErrorList(hook, pFeaDatset, pTopo, pTopoRuleType, "", "", out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
            if (outError != null) return;
        }

        //一般的拓扑检查，针对特殊层的检查,包括源
        public void OrdinaryTopoCheck(IArcgisDataCheckHook hook,IFeatureDataset pFeaDatset, string pFeaClsName, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;
            Exception exError = null;
            //创建拓扑
            ITopology pTopo = TopologyCheckClass.CreateTopo(pFeaDatset, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            //将要素类添加到拓扑中并创建拓扑规则
            TopologyCheckClass.AddRuleandClasstoTopology(pTopo, pFeaDatset, pFeaClsName, pTopoRuleType, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            //验证拓扑
            TopologyCheckClass.ValidateTopology(pTopo, (pFeaDatset as IGeoDataset).Extent, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            //获得错误列表
            GetErrorList(hook,pFeaDatset, pTopo, pTopoRuleType, pFeaClsName,"",out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
            if (outError != null) return;
        }

        //一般的拓扑检查，针对特殊层的检查,包括源,利用列表来保存
        public void OrdinaryTopoCheck(IArcgisDataCheckHook hook, IFeatureDataset pFeaDatset, List<string> pFeaClsNameList, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;
            Exception exError = null;
            //创建拓扑
            ITopology pTopo = TopologyCheckClass.CreateTopo(pFeaDatset, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            //将要素类添加到拓扑中并创建拓扑规则
            TopologyCheckClass.AddRuleandClasstoTopology(pTopo, pFeaDatset, pFeaClsNameList, pTopoRuleType, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                //outError = exError;
                return;
            }

            //验证拓扑
            TopologyCheckClass.ValidateTopology(pTopo, (pFeaDatset as IGeoDataset).Extent, out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            //获得错误列表
            GetErrorList(hook,pFeaDatset, pTopo, pTopoRuleType, "", "", out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }

            TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
            if (outError != null) return;
        }


        /// <summary>
        /// 获得错误列表，一般的拓扑检查
        /// </summary>
        /// <param name="pFeaDatset"></param>
        /// <param name="pTopo"></param>
        /// <param name="outError"></param>
        private void GetErrorList(IArcgisDataCheckHook hook,IFeatureDataset pFeaDatset, ITopology pTopo, esriTopologyRuleType pTopoRuleType,string pOrgFeaClaName,string desFeaClsName,out Exception outError)
        {
            outError = null;
            try
            {
                //依照拓扑规则获取错误要素
                IErrorFeatureContainer pErrorFeaCon = pTopo as IErrorFeatureContainer;
                ITopologyRuleContainer pTopoRuleCon = pTopo as ITopologyRuleContainer;
                IEnumRule pEnumRule = pTopoRuleCon.Rules;
                pEnumRule.Reset();
                ITopologyRule pTopoRule = pEnumRule.Next() as ITopologyRule;
                while (pTopoRule != null)
                {
                    IEnumTopologyErrorFeature pEnumErrorFea = null;
                    try
                    {
                        pEnumErrorFea = pErrorFeaCon.get_ErrorFeatures(
                            (pFeaDatset as IGeoDataset).SpatialReference, pTopoRule, (pFeaDatset as IGeoDataset).Extent, true, false);
                    }
                    catch (Exception ex)
                    {
                        TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                        outError = ex;
                        return;
                    }
                    if (pEnumErrorFea == null)
                    {
                        pTopoRule = pEnumRule.Next() as ITopologyRule;
                        continue;
                    }
                    ITopologyErrorFeature pErrorFea = pEnumErrorFea.Next();
                    while (pErrorFea != null)
                    {
                        //将错误结果保存起来
                        double pMapx = 0.0;
                        double pMapy = 0.0;
                        int oriFeaClsID = pErrorFea.OriginClassID;
                        int desFeaClsID = pErrorFea.DestinationClassID;
                        //string desFeaClsName = "";
                        int desID = pErrorFea.DestinationOID;
                        int oriID = pErrorFea.OriginOID;

                        # region 同层线相交，排除掉同层线重叠的情况
                        if (pTopoRuleType == esriTopologyRuleType.esriTRTLineNoIntersection)
                        {
                            if (pErrorFea.ShapeType == esriGeometryType.esriGeometryPolyline)
                            {
                                //同层线重叠，作为例外拓扑处理
                                pTopoRuleCon.PromoteToRuleException(pErrorFea);
                                pErrorFea = pEnumErrorFea.Next();
                                continue;
                            }
                        }
                        #endregion

                        if (desFeaClsID > 0)
                        {
                            //目标要素类名称
                            desFeaClsName = ((pTopo as IFeatureClassContainer).get_ClassByID(pErrorFea.DestinationClassID) as IDataset).Name;
                        }
                        if (desID <= 0)
                        {
                            //目标要素OID
                            desID = -1;
                        }

                        //if (oriFeaClsID <= 0)
                        //{
                        //    outError = new Exception("拓扑检查结果显示不正确，找不到源要素类ID！");
                        //    return;
                        //}
                        IFeatureClass oriFeaCls = null;
                        if (oriFeaClsID > 0)
                        {
                            oriFeaCls = (pTopo as IFeatureClassContainer).get_ClassByID(oriFeaClsID);
                        }
                        else
                        {
                            if (pTopoRuleType == esriTopologyRuleType.esriTRTAreaAreaCoverEachOther)
                            {
                            }
                            else
                            {
                                if (oriFeaCls == null)
                                {
                                    outError = new Exception("获取源要素类信息出错！");
                                    return;
                                }
                            }
                        }
                        
                       

                        if (oriID > 0)
                        {
                            IPoint pPoint = null;
                            IFeature oriFeature = oriFeaCls.GetFeature(oriID);
                            if (oriFeature.Shape.GeometryType != esriGeometryType.esriGeometryPoint)
                            {
                                pPoint = TopologyCheckClass.GetPointofFeature(oriFeature);
                            }
                            else
                            {
                                //说明该要素是点要素
                                pPoint = oriFeature.Shape as IPoint;
                            }
                            pMapx = pPoint.X;
                            pMapy = pPoint.Y;
                        }
                        else
                        {
                            if (pTopoRuleType == esriTopologyRuleType.esriTRTAreaNoGaps)
                            {
                                # region 面缝隙检查的oriID为0，通过特殊的处理查找面缝隙检查的源要素
                                IFeature oriFea = null;

                                //声明点集合
                                IPointCollection mPointCol = new PolygonClass();
                                IFeature pFeature = pErrorFea as IFeature;
                                mPointCol.AddPointCollection(pFeature.ShapeCopy as IPointCollection);

                                //闭合点集合成面状
                                IPolygon mPolygon = mPointCol as IPolygon;
                                mPolygon.Close();

                                //根据闭合的面状进行查找
                                mPointCol = mPolygon as IPointCollection;
                                if (mPointCol is IArea)
                                {
                                    //若点集合是面状，进行处理
                                    ITopologicalOperator2 mTopoOper = mPointCol as ITopologicalOperator2;
                                    mTopoOper.IsKnownSimple_2 = false;
                                    mTopoOper.Simplify();
                                    mPointCol = mTopoOper as IPointCollection;
                                    //如果拓扑错误包含要素，作为例外处理
                                    ISpatialFilter pFilter = new SpatialFilterClass();
                                    pFilter.Geometry = mPointCol as IGeometry;
                                    pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                                    IFeatureCursor pFeaCursor = oriFeaCls.Search(pFilter, false);
                                    if (pFeaCursor == null) return;
                                    oriFea = pFeaCursor.NextFeature();
                                    if (oriFea != null)
                                    {
                                        pTopoRuleCon.PromoteToRuleException(pErrorFea);
                                        pErrorFea = pEnumErrorFea.Next();
                                        continue;
                                    }
                                    //若面积为0.则作为例外处理
                                    IArea mArea = mPointCol as IArea;
                                    if (mArea.Area == 0)
                                    {
                                        pTopoRuleCon.PromoteToRuleException(pErrorFea);
                                        pErrorFea = pEnumErrorFea.Next();
                                        continue;
                                    }
                                    else
                                    {
                                        //获得源要素
                                        pFilter = new SpatialFilterClass();
                                        pFilter.Geometry = mTopoOper as IGeometry;
                                        pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;//共界
                                        pFeaCursor = oriFeaCls.Search(pFilter, false);
                                        if (pFeaCursor == null) return;
                                        oriFea = pFeaCursor.NextFeature();
                                        if (oriFea == null)
                                        {
                                            pErrorFea = pEnumErrorFea.Next();
                                            continue;
                                        }
                                        oriID = oriFea.OID;
                                        IPoint mPoint = TopologyCheckClass.GetPointofFeature(oriFea);
                                        pMapx = mPoint.X;
                                        pMapy = mPoint.Y;
                                    }

                                    //释放cursor
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
                                }
                                else
                                {
                                    pErrorFea = pEnumErrorFea.Next();
                                    continue;
                                }
                                #endregion
                            }
                            if (desID >0 && desFeaClsID>0)
                            {
                                IFeatureClass desFeaCls = (pTopo as IFeatureClassContainer).get_ClassByID(desFeaClsID);
                                IPoint pPoint = null;
                                IFeature desFeature = desFeaCls.GetFeature(desID);
                                if (desFeature.Shape.GeometryType != esriGeometryType.esriGeometryPoint)
                                {
                                    pPoint = TopologyCheckClass.GetPointofFeature(desFeature);
                                }
                                else
                                {
                                    //说明该要素是点要素
                                    pPoint = desFeature.Shape as IPoint;
                                }
                                pMapx = pPoint.X;
                                pMapy = pPoint.Y;
                            }
                        }

                      
                        # region 错误描述信息分类保存
                        int errID = 0;              //错误ID，
                        string errDes = "";         //错误描述
                        switch (pTopoRuleType)
                        {
                            case esriTopologyRuleType.esriTRTAreaNoOverlapArea:
                                //异层面重叠
                                errID = enumErrorType.异层面重叠检查.GetHashCode();
                                errDes = "源要素类中的多边形不能与目标要素类中的多边形重叠";
                                break;
                            case esriTopologyRuleType.esriTRTAreaNoGaps:
                                //面缝隙
                                errID = enumErrorType.面缝隙检查.GetHashCode();
                                errDes = "多边形不能有缝隙";
                                break;
                            case esriTopologyRuleType.esriTRTAreaCoveredByArea:
                                //面含面
                                errID = enumErrorType.面含面检查.GetHashCode();
                                errDes = "源要素类中的多边形必须被目标要素类中的多边形所覆盖";
                                break;
                            case esriTopologyRuleType.esriTRTPointCoveredByAreaBoundary:
                                //点搭面
                                errID = enumErrorType.点搭面检查.GetHashCode();
                                errDes = "源要素类中的点必须在目标要素类的多边形边界上";
                                break;
                            case esriTopologyRuleType.esriTRTPointCoveredByLine:
                                //点搭线
                                errID = enumErrorType.点搭线检查.GetHashCode();
                                errDes = "源要素类中的点必须在目标要素类的线之上";
                                break;
                            case esriTopologyRuleType.esriTRTPointCoveredByLineEndpoint:
                                //点位于线端点检查
                                errID = enumErrorType.点位于线端点检查.GetHashCode();
                                errDes = "源要素类中的点必须位于目标要素类中的线的端点上";
                                break;
                            case esriTopologyRuleType.esriTRTPointProperlyInsideArea:
                                //点位于面内检查
                                errID = enumErrorType.点位于面内检查.GetHashCode();
                                errDes = "源要素类中的点必须位于目标要素类的多边形内";
                                break;
                            case esriTopologyRuleType.esriTRTLineNoOverlap:
                                //同层线重叠
                                errID = enumErrorType.同层线重叠检查.GetHashCode();
                                errDes = "同一要素类中，两个线要素不能重叠";
                                break;
                            case esriTopologyRuleType.esriTRTLineNoIntersection:
                                //同层线相交
                                errID = enumErrorType.同层线相交检查.GetHashCode();
                                errDes = "同一要素类中，两个线要素不能相交";
                                break;
                            case esriTopologyRuleType.esriTRTLineCoveredByAreaBoundary:
                                //线面边界重合检查
                                errID = enumErrorType.线面边界重合检查.GetHashCode();
                                errDes = "源要素类中的线必须被目标要素类中的多边形的边界覆盖";
                                break;
                            case esriTopologyRuleType.esriTRTLineNoOverlapLine:
                                //异层线重叠检查
                                errID = enumErrorType.异层线重叠检查.GetHashCode();
                                errDes = "源要素类中的线与目标要素类中的线不能重叠";
                                break;
                            case esriTopologyRuleType.esriTRTAreaBoundaryCoveredByAreaBoundary:
                                //面边界面边界重合检查
                                errID = enumErrorType.面边界面边界重合检查.GetHashCode();
                                errDes = "源要素类中的多边形边界必须被目标要素类中的多边形边界覆盖";
                                break;
                            case esriTopologyRuleType.esriTRTAreaAreaCoverEachOther:
                                //面面相互覆盖检查
                                errID = enumErrorType.面面相互覆盖检查.GetHashCode();
                                errDes = "源要素类与目标要素类中的多边形必须相互覆盖";
                                break;
                            case esriTopologyRuleType.esriTRTAreaContainPoint:
                                //面含点检查
                                errID = enumErrorType.面含点检查.GetHashCode();
                                errDes = "源要素类中的多边形必须包含点";
                                break;
                            case esriTopologyRuleType.esriTRTAreaBoundaryCoveredByLine:
                                //面边界线重合检查
                                errID = enumErrorType.面边界线重合检查.GetHashCode();
                                errDes = "源要素类中多边形的边界必须和线要素重合";
                                break;
                            case esriTopologyRuleType.esriTRTLineCoveredByLineClass:
                                //线线重合检查
                                errID = enumErrorType.线线重合检查.GetHashCode();
                                errDes = "源要素类中的线要素必须被目标要素类中的线要素覆盖";
                                break;
                            case esriTopologyRuleType.esriTRTLineEndpointCoveredByPoint:
                                //线端点被点覆盖检查
                                errID = enumErrorType.线端点被点覆盖检查.GetHashCode();
                                errDes = "源要素类中线的端点必须被目标要素类中的点覆盖";
                                break;
                            default:
                                break;
                        }
                        #endregion

                        //用来保存错误结果
                        List<object> ErrorLst = new List<object>();
                        ErrorLst.Add("一般拓扑检查");//功能组名称
                        ErrorLst.Add("一般拓扑检查");//功能名称
                        ErrorLst.Add((pFeaDatset as IDataset).Workspace.PathName);  //数据文件名
                        ErrorLst.Add(errID);//错误id;
                        ErrorLst.Add(errDes);//错误描述
                        ErrorLst.Add(pMapx);    //...
                        ErrorLst.Add(pMapy);    //...
                        if (oriFeaCls != null)
                        {
                            ErrorLst.Add((oriFeaCls as IDataset).Name);
                            ErrorLst.Add(oriID);
                        }
                        else
                        {
                            ErrorLst.Add(pOrgFeaClaName);
                            ErrorLst.Add(-1);
                        }
                        ErrorLst.Add(desFeaClsName);
                        ErrorLst.Add(desID);
                        ErrorLst.Add(false);
                        ErrorLst.Add(System.DateTime.Now.ToString());

                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);
                        pErrorFea = pEnumErrorFea.Next();
                    }

                    pTopoRule = pEnumRule.Next() as ITopologyRule;
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 线穿面检查
        /// </summary>
        /// <param name="pFeaDatset"></param>
        /// <param name="oriFeaClsName">线要素类名称</param>
        /// <param name="desFeaClsName">面要素类名称</param>
        /// <param name="outError"></param>
        public void CrossTopoCheck(IArcgisDataCheckHook hook, IFeatureDataset pFeaDatset, string oriFeaClsName, string desFeaClsName, out Exception outError)
        {
            outError = null;
            try
            {
                //ErrorEventArgs pErrorArgs = new ErrorEventArgs();

                //线要素类和面要素类
                IFeatureClass mLineFeaCls = (pFeaDatset.Workspace as IFeatureWorkspace).OpenFeatureClass(oriFeaClsName);
                IFeatureClass mAreaFeaCls = (pFeaDatset.Workspace as IFeatureWorkspace).OpenFeatureClass(desFeaClsName);
                IFeatureCursor mAreaCursor = mAreaFeaCls.Search(null, false);
                if (mAreaCursor == null) return;
                IFeature mAreaFea = mAreaCursor.NextFeature();

                //设置进度条
                ProgressChangeEvent eInfo = new ProgressChangeEvent();
                eInfo.Max = mAreaFeaCls.FeatureCount(null);
                int pValue = 0;

                //遍历面要素
                while (mAreaFea != null)
                {
                    //查找线穿面的要素
                    ISpatialFilter mFilter = new SpatialFilterClass();
                    mFilter.Geometry = mAreaFea.Shape;
                    mFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                    IFeatureCursor pCursor = mLineFeaCls.Search(mFilter, false);
                    if (pCursor == null) return;
                    IFeature mLineErrFeature = pCursor.NextFeature();
                    //遍历出错的线要素
                    while (mLineErrFeature != null)
                    {
                        //保存错误结果
                        IPoint pPoint = TopologyCheckClass.GetPointofFeature(mLineErrFeature);
                        double pMapx = pPoint.X;
                        double pMapy = pPoint.Y;

                        List<object> ErrorLst = new List<object>();
                        ErrorLst.Add("线面拓扑检查");//功能组名称
                        ErrorLst.Add("线穿面检查");//功能名称
                        ErrorLst.Add((pFeaDatset as IDataset).Workspace.PathName);  //数据文件名
                        ErrorLst.Add(enumErrorType.线穿面检查.GetHashCode());//错误id;
                        ErrorLst.Add("等高线不能穿过面");//错误描述
                        ErrorLst.Add(pMapx);    //...
                        ErrorLst.Add(pMapy);    //...
                        ErrorLst.Add(oriFeaClsName);
                        ErrorLst.Add(mLineErrFeature.OID);
                        ErrorLst.Add(desFeaClsName);
                        ErrorLst.Add(mAreaFea.OID);
                        ErrorLst.Add(false);
                        ErrorLst.Add(System.DateTime.Now.ToString());

                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);
                       
                        mLineErrFeature = pCursor.NextFeature();
                    }

                    //释放cursor
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    mAreaFea = mAreaCursor.NextFeature();

                    //进度条加1
                    pValue++;
                    eInfo.Value = pValue;
                    GeoDataChecker.GeoDataChecker_ProgressShow((object)GeoDataChecker._ProgressBarInner, eInfo);
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mAreaCursor);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }
    }

       

    //要素重复性检查(依赖拓扑)
    public class DuplicateCheckClass
    {
        public event DataErrTreatHandle DataErrTreat;

        //要素重复性检查
        public void DuplicateCheck(IArcgisDataCheckHook hook, IFeatureDataset featureDataset, esriGeometryType geometryType, out Exception errEx)
        {
            errEx = null;
            ITopology topology = ModCommonFunction.CreateTopology(featureDataset, out errEx);
            if (topology == null) return;

            //将线要素类添加到拓扑中并创建拓扑规则
            ModCommonFunction.AddClassToTopology(topology, featureDataset, geometryType, out errEx);
            if (errEx != null) return;

            switch (geometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    PointDuplicateCheck(hook, featureDataset, out errEx);
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    PolylineAndPolygonDuplicateCheck(hook, topology, featureDataset, esriGeometryType.esriGeometryPolyline, esriTopologyRuleType.esriTRTLineNoOverlap, out errEx);
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    PolylineAndPolygonDuplicateCheck(hook, topology, featureDataset, esriGeometryType.esriGeometryPolygon, esriTopologyRuleType.esriTRTAreaNoOverlap, out errEx);
                    break;
                case esriGeometryType.esriGeometryNull:
                    PointDuplicateCheck(hook, featureDataset, out errEx);
                    if (errEx != null) return;
                    PolylineAndPolygonDuplicateCheck(hook, topology, featureDataset, esriGeometryType.esriGeometryPolyline, esriTopologyRuleType.esriTRTLineNoOverlap, out errEx);
                    if (errEx != null) return;
                    PolylineAndPolygonDuplicateCheck(hook, topology, featureDataset, esriGeometryType.esriGeometryPolygon, esriTopologyRuleType.esriTRTAreaNoOverlap, out errEx);
                    if (errEx != null) return;
                    break;
                default:
                    break;
            }

            if (errEx != null) return;

            //删除拓扑
            ModCommonFunction.RemoveTopology(featureDataset, out errEx);
        }

        //点重复性检查
        public void PointDuplicateCheck(IArcgisDataCheckHook hook, IFeatureDataset featureDataset, out Exception errEx)
        {
            errEx = null;
            Exception eex=null;
            #region point
            try
            {
                // 读XML内容
                if (featureDataset == null) return;
                IEnumDataset GetFeatureClass = featureDataset.Subsets;//得到集合
                GetFeatureClass.Reset();
                IDataset FeatureDatasetClass = GetFeatureClass.Next();
                //开始遍历集合 检查点的重复
                while (FeatureDatasetClass != null)
                {
                    if (FeatureDatasetClass is IFeatureClass)
                    {
                        Hashtable Foreach_hs = new Hashtable();//用一个哈希来遍历重复
                        IFeatureClass pFeatureClass = FeatureDatasetClass as IFeatureClass;//要素类
                        if (pFeatureClass == null || pFeatureClass.ShapeType != esriGeometryType.esriGeometryPoint)
                        {
                            FeatureDatasetClass = GetFeatureClass.Next();
                            continue;//如果为空或是非点层就返回
                        }
                        //开始遍历
                        #region 遍历要素类
                        IFeatureCursor cour = pFeatureClass.Search(null, false);
                        IFeature fu = cour.NextFeature();
                        ArrayList FeatureList = new ArrayList();//存要素动态数组
                        //将当前要素类里的所有要素存入动态数组
                        while (fu != null)
                        {
                            FeatureList.Add(fu);
                            fu = cour.NextFeature();
                        }
                        //如果为空，就返回
                        int FeatureCount = FeatureList.Count;
                        if (FeatureCount == 0)
                        {
                            FeatureDatasetClass = GetFeatureClass.Next();
                            continue;
                        }
                        #region 遍历要素类里的重复点
                        for (int F = 0; F < FeatureCount; F++)
                        {
                            fu = FeatureList[F] as IFeature;
                            int i = F + 1;//方便计算
                            #region 总体上得到一个比较值，然后加入到LIST当中
                            IPoint point = fu.Shape as IPoint;
                            double x = point.X;
                            double y = point.Y;
                            string temp = x.ToString() + "," + y.ToString();
                            #endregion
                            string soruce = fu.OID.ToString();//源OID
                            if (Foreach_hs == null)
                            {
                                Foreach_hs.Add(temp, soruce);
                            }
                            else
                            {
                                if (!Foreach_hs.Contains(temp))
                                {
                                    Foreach_hs.Add(temp, soruce);
                                }
                                else
                                {
                                    //判断属性值是否相同
                                    int oriOID =int.Parse( Foreach_hs[temp].ToString());
                                    IFeature oriFea = pFeatureClass.GetFeature(oriOID);//源要素
                                    IFeature DesFea =  fu;//目标要素
                                    //要素属性重复检查
                                    bool res = isSamePropety(hook, oriFea, DesFea, out eex);

                                    if (res)
                                    {
                                        //bool 
                                        #region 出错LOG
                                        //以下是出错的记录
                                        string Org = Foreach_hs[temp].ToString();//取得源重复的OID
                                        string Des = fu.OID.ToString();//des

                                        double localPntX = 0;
                                        double localPntY = 0;
                                        IPoint pnt = oriFea.Shape as IPoint;
                                        if (pnt != null)
                                        {
                                            localPntX = pnt.X;
                                            localPntY = pnt.Y;
                                        }

                                        //用来保存错误结果
                                        List<object> errorLst = new List<object>();
                                        errorLst.Add("");
                                        errorLst.Add("");
                                        errorLst.Add((featureDataset as IDataset).Workspace.PathName);
                                        errorLst.Add(0);
                                        errorLst.Add("");
                                        errorLst.Add(localPntX);
                                        errorLst.Add(localPntY);
                                        errorLst.Add(FeatureDatasetClass.Name);
                                        errorLst.Add(Org);//源OID
                                        errorLst.Add(FeatureDatasetClass.Name);
                                        errorLst.Add(Des);//目标重复OID
                                        errorLst.Add(false);
                                        errorLst.Add(System.DateTime.Now.ToString());
                                        //传递错误日志
                                        IDataErrInfo dataErrInfo = new DataErrInfo(errorLst);
                                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                                        DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);
                                        #endregion
                                    }
                                }
                            }
                        }
                        #endregion
                        #endregion

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(cour);//释放游标
                    }
                    FeatureDatasetClass = GetFeatureClass.Next();
                }
            }
            catch (Exception ex)
            {
                errEx = ex;
                return;
            }
            #endregion
        }


        //线、面重复性检查
        public void PolylineAndPolygonDuplicateCheck(IArcgisDataCheckHook hook, ITopology topology, IFeatureDataset featureDataset, esriGeometryType geometryType, esriTopologyRuleType topologyRuleType, out Exception errEx)
        {
            errEx = null;

            //添加拓扑规则
            ModCommonFunction.AddRuleToTopology(topology, geometryType, topologyRuleType, out errEx);
            if (errEx != null) return;

            //验证拓扑
            ModCommonFunction.ValidateTopology(topology, (featureDataset as IGeoDataset).Extent, out errEx);
            if (errEx != null) return;

            //依照拓扑规则获取错误要素
            IErrorFeatureContainer pErrFeatCon = topology as IErrorFeatureContainer;
            ITopologyRuleContainer topologyRuleContainer = topology as ITopologyRuleContainer;
            IEnumRule pEnumRule = topologyRuleContainer.Rules;
            pEnumRule.Reset();
            ITopologyRule rule = pEnumRule.Next() as ITopologyRule;
            while (rule != null)
            {
                try
                {

                    IEnumTopologyErrorFeature pEnumTopoErrFeat = pErrFeatCon.get_ErrorFeatures((featureDataset as IGeoDataset).SpatialReference, rule as ITopologyRule, (featureDataset as IGeoDataset).Extent, true, false);

                    ITopologyErrorFeature topoErrFeat = pEnumTopoErrFeat.Next();
                    while (topoErrFeat != null)
                    {
                        IFeatureClass featureClass = (topology as IFeatureClassContainer).get_ClassByID(topoErrFeat.OriginClassID);

                        //判断两线要素是否完全重叠
                        bool res = IsSameFeature(hook, featureClass.GetFeature(topoErrFeat.OriginOID), featureClass.GetFeature(topoErrFeat.DestinationOID), out errEx);
                        if (errEx != null)
                        {
                            topoErrFeat = pEnumTopoErrFeat.Next();
                            continue;
                        }

                        if (res == false)
                        {
                            topoErrFeat = pEnumTopoErrFeat.Next();
                            continue;
                        }

                        //求错误要素的定位点
                        double localPntX = 0;
                        double localPntY = 0;
                        IFeature originFeat = featureClass.GetFeature(topoErrFeat.OriginOID);
                        IPoint pnt = ModCommonFunction.GetPntOfFeature(originFeat);
                        if (pnt != null)
                        {
                            localPntX = pnt.X;
                            localPntY = pnt.Y;
                        }
                        //求错误要素的定位点

                        //用来保存错误结果
                        List<object> errorLst = new List<object>();
                        errorLst.Add("");
                        errorLst.Add("");
                        errorLst.Add((featureDataset as IDataset).Workspace.PathName);
                        errorLst.Add(0);
                        errorLst.Add("");
                        errorLst.Add(localPntX);
                        errorLst.Add(localPntY);
                        errorLst.Add((featureClass as IDataset).Name);
                        errorLst.Add(topoErrFeat.OriginOID);
                        errorLst.Add((featureClass as IDataset).Name);
                        errorLst.Add(topoErrFeat.DestinationOID);
                        errorLst.Add(false);
                        errorLst.Add(System.DateTime.Now.ToString());
                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(errorLst);
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);

                        topoErrFeat = pEnumTopoErrFeat.Next();
                    }
                }
                catch (Exception ex)
                {

                }
                rule = pEnumRule.Next() as ITopologyRule;

                //移出规则
                ModCommonFunction.RemoveTopologyRule(topology, out errEx);
                if (errEx != null) return;
            }
        }
        //判断两线(面)要素是否完全重叠
        private bool IsSameFeature(IArcgisDataCheckHook hook, IFeature originFeat, IFeature destinationFeat, out Exception outErr)
        {
            outErr = null;
            bool res = false;

            //几何属性判断
            res = IsSameGeometry(originFeat.Shape, destinationFeat.Shape, out outErr);
            if (outErr != null || res == false)
            {
                //几何属性不相同
                return res;
            }
            try
            {
                //几何属性相同，字段属性判断
                bool propetyRes = isSamePropety(hook, originFeat, destinationFeat, out outErr);
                return propetyRes;
            }
            catch (Exception err)
            {
                outErr = err;
                return false;
            }
        }
        //判断两个要素属性是否相同(陈亚飞添加)
        private bool isSamePropety(IArcgisDataCheckHook hook, IFeature originFea, IFeature DestinFea, out Exception eError)
        {
            eError = null;
            Exception ex=null;
            //获得模板规则库的连接
            //SysCommon.DataBase.SysTable pSysTable= new SysCommon.DataBase.SysTable();
            //pSysTable.DbConn = (hook.DataCheckParaSet as IArcgisDataCheckParaSet).DbConnPara;
            //pSysTable.DBConType = SysCommon.enumDBConType.OLEDB;
            //pSysTable.DBType = SysCommon.enumDBType.ACCESS;

            SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
            pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +TopologyCheckClass.GeoDataCheckParaPath, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out ex);
            if (ex != null)
            {
                eError = new Exception("连接库体出错！路径为：" + TopologyCheckClass.GeoDataCheckParaPath);
                pSysTable.CloseDbConnection();
                return false;
            }

            //要素所在的图层名
            IDataset pDataset = originFea.Class as IDataset;
            string pFeaClsName = pDataset.Name;
            if(pFeaClsName.Contains("."))
            {
                pFeaClsName = pFeaClsName.Substring(pFeaClsName.IndexOf('.') + 1);
            }

            //查找图层用于检查的字段集合
            string str = "select * from GeoCheckerDataDuplicate where FEATURECLASSNAME='" + pFeaClsName + "'";
            DataTable tempDt = pSysTable.GetSQLTable(str, out ex);
            if (ex != null)
            {
                eError = new Exception("查询表格GeoCheckerDataDuplicate出错！");
                pSysTable.CloseDbConnection();
                return false;
            }
            if (tempDt.Rows.Count == 0)
            {
                eError = new Exception("库中缺少图层，请检查图层"+pFeaClsName+"在模板库中是否存在！");
                pSysTable.CloseDbConnection();
                return false;
            }
            pSysTable.CloseDbConnection();

            string fields = tempDt.Rows[0]["FIELDS"].ToString();
            string[] fieldArr = fields.Split(new char[] { ',' });

            //进行字段匹配检查
            bool fieldCheckRes = true;
            for (int i = 0; i < fieldArr.Length; i++)
            {
                int oriIndex = -1;
                int DesIndes = -1;
                oriIndex = originFea.Fields.FindField(fieldArr[i]);
                DesIndes = DestinFea.Fields.FindField(fieldArr[i]);
                if (oriIndex == -1 || DesIndes == -1)
                {
                    eError = new Exception("要素不包含字段"+fieldArr[i]+",请检查!");
                    return false;
                }
                if (originFea.get_Value(oriIndex).ToString() != DestinFea.get_Value(DesIndes).ToString())
                {
                    //有一个字段值不相同，就说明属性不相同
                    fieldCheckRes = false;
                    break;
                }
            }
            return fieldCheckRes;
        }
        //判断两图形是否相同
        private bool IsSameGeometry(IGeometry originGeometry, IGeometry destinationGeometry, out Exception outErr)
        {
            outErr = null;
            bool res = false;
            try
            {
                //求几何空间差值，必须要两种情况考虑：a->b  b->a，如果同时都为空，说明两个重复
                ITopologicalOperator topologicalOperator = originGeometry as ITopologicalOperator;//空间几何运算
                IGeometry geometryX = topologicalOperator.Difference(destinationGeometry as IGeometry);//求两个面的差值

                topologicalOperator = destinationGeometry as ITopologicalOperator;//空间几何运算
                IGeometry geometryY = topologicalOperator.Difference(originGeometry as IGeometry);//求两个面的差值

                if (geometryX.IsEmpty && geometryY.IsEmpty)                        //空间位置相同
                {
                    res = true;
                }
            }
            catch (Exception err)
            {
                outErr = err;
            }

            return res;
        }
    }
}
