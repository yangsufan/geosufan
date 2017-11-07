using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoDataChecker
{
    /// <summary>
    /// 综合拓扑检查
    /// </summary>
    public class GeoTopologyCheck : IDataCheckRealize
    {
        public event DataErrTreatHandle DataErrTreat;
        public event ProgressChangeHandle ProgressShow;

        private IArcgisDataCheckHook Hook;

        #region IDataCheck 成员

        public void OnCreate(IDataCheckHook hook)
        {
            Hook = hook as IArcgisDataCheckHook;
        }

        public void OnDataCheck()
        {
            Exception eError = null;
            if (Hook == null) return;
            IArcgisDataCheckParaSet dataCheckParaSet = Hook.DataCheckParaSet as IArcgisDataCheckParaSet;
            if (dataCheckParaSet == null) return;
            if (dataCheckParaSet.Workspace == null) return;

            //实现代码
            SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
            pSysTable.DbConn = dataCheckParaSet.DbConnPara;
            pSysTable.DBConType = SysCommon.enumDBConType.OLEDB;
            pSysTable.DBType = SysCommon.enumDBType.ACCESS;



            //获取所有数据集
            SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet(dataCheckParaSet.Workspace);
            List<string> featureDatasetNames = sysGisDataSet.GetAllFeatureDatasetNames();
            if (featureDatasetNames.Count == 0) return;

            //设置进度条
            ProgressChangeEvent eInfo = new ProgressChangeEvent();
            eInfo.Max = featureDatasetNames.Count;
            int pValue = 0;

            foreach (string name in featureDatasetNames)
            {
                IFeatureDataset featureDataset = sysGisDataSet.GetFeatureDataset(name, out eError);
                if (eError != null) continue;

                //....................
                //执行综合拓扑检查
                if (GeoDataChecker.DicTopoDataCheck.Count != 0)
                {
                    //创建拓扑
                    ITopology pTopo = TopologyCheckClass.CreateTopo(featureDataset, out eError);
                    if (eError != null)
                    {
                        TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                        continue;
                    }

                    //在拓扑中添加要素类
                    TopologyCheckClass.AddFeaClasstoTopo(pTopo, featureDataset, out eError);
                    if (eError != null)
                    {
                        TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                        continue;
                    }

                    //将规则添加进去
                    foreach (KeyValuePair<string, string> topoCheck in GeoDataChecker.DicTopoDataCheck)
                    {
                        List<string> LstOrgClsName = new List<string>();
                        List<string> DicClsName = new List<string>();
                        esriTopologyRuleType pTopoRuleType;
                        DataTable mTable = null;
                        #region 将拓扑检查项的参数组织起来,并添加拓扑规则
                        switch (topoCheck.Key)
                        {
                            //=============================================================================
                            case "GeoDataChecker.GeoPolylineDanglesCheck":
                                //线悬挂点检查(通用，分几种情况,有待完善(最好单独做拓扑检查))
                                pTopoRuleType = esriTopologyRuleType.esriTRTLineNoDangles;
                                //添加拓扑规则
                                TopologyCheckClass.AddRuletoTopology(pTopo, esriGeometryType.esriGeometryPolyline, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoPolylinePseudosCheck":
                                //线伪节点检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTLineNoPseudos;
                                //添加拓扑规则
                                TopologyCheckClass.AddRuletoTopology(pTopo, esriGeometryType.esriGeometryPolyline, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoPolylineSelfIntersect":
                                //线自相交检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTLineNoSelfIntersect;
                                //添加拓扑规则
                                TopologyCheckClass.AddRuletoTopology(pTopo, esriGeometryType.esriGeometryPolyline, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoPolylineSelfOverlapCheck":
                                //线自重叠检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTLineNoSelfOverlap;
                                //添加拓扑规则
                                TopologyCheckClass.AddRuletoTopology(pTopo, esriGeometryType.esriGeometryPolyline, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoPolygonOverlapCheck":
                                //同层面重叠检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTAreaNoOverlap;
                                //添加拓扑规则
                                TopologyCheckClass.AddRuletoTopology(pTopo, esriGeometryType.esriGeometryPolygon, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoCheckMustBeSinglePart":
                                //简单线检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTLineNoMultipart;
                                //添加拓扑规则
                                TopologyCheckClass.AddRuletoTopology(pTopo, esriGeometryType.esriGeometryPolyline, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            //===========================================================================
                            case "GeoDataChecker.GeoPolylineOverlapCheck":
                                //同层线重叠检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTLineNoOverlap;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 14, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                LstOrgClsName = GetLstFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddFeaRuletoTopo(pTopo, LstOrgClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoPolylineIntersectCheck":
                                //同层线相交检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTLineNoIntersection;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 15, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                LstOrgClsName = GetLstFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddFeaRuletoTopo(pTopo, LstOrgClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoPolygonGapsCheck":
                                //面缝隙检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTAreaNoGaps;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 8, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                LstOrgClsName = GetLstFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddFeaRuletoTopo(pTopo, LstOrgClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            //===========================================================================
                            case "GeoDataChecker.GeoPolygonOverlapPolygonCheck":
                                //异层面重叠检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTAreaNoOverlapArea;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 7, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            //break;
                            case "GeoDataChecker.GeoPointProperlyInsideAreaCheck":
                                //点位于面内检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTPointProperlyInsideArea;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 13, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoPointCoveredByLineEndPointCheck":
                                //点位于线端点检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTPointCoveredByLineEndpoint;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 12, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoPointCoveredByLineCheck":
                                //点搭线检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTPointCoveredByLine;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 11, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoPointCoveredByBoundaryCheck":
                                //点搭面检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTPointCoveredByAreaBoundary;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 10, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoLineCoveredByAreaBoundaryCheck":
                                //线面边界重合检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTLineCoveredByAreaBoundary;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 16, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoAreaCoveredByAreaCheck":
                                //面含面检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTAreaCoveredByArea;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 9, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoCheckAreaBoundaryCoveredByAreaBoundary":
                                //面边界面边界重合检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTAreaBoundaryCoveredByAreaBoundary;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 41, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoCheckAreaBoundaryCoveredByLine":
                                //面边界线重合检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTAreaBoundaryCoveredByLine;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 44, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoCheckAreaContainsPoint":
                                //面含点检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTAreaContainPoint;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 43, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoCheckAreaCoverEachOther":
                                //面面相互覆盖检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTAreaAreaCoverEachOther;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 42, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoCheckLineCoveredByLine":
                                //线线重合检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTLineCoveredByLineClass;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 45, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoCheckLineEndpointCoveredByPoint":
                                //线端点点重合检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTLineEndpointCoveredByPoint;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 46, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            case "GeoDataChecker.GeoCheckLineNoOverlapWithLine":
                                //异层线重叠检查
                                pTopoRuleType = esriTopologyRuleType.esriTRTLineNoOverlapLine;
                                mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 40, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                if (mTable.Rows.Count == 0)
                                {
                                    //return;
                                    continue;
                                }
                                //配置表里面需要检查的要素类集合
                                DicClsName = GetDicFeaClsName(mTable, out eError);
                                if (eError != null)
                                {
                                    return;
                                }
                                //添加拓扑规则
                                TopologyCheckClass.AddDicRuletoTopology(pTopo, DicClsName, pTopoRuleType, out eError);
                                if (eError != null)
                                {
                                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                                    continue;
                                }
                                break;
                            default:
                                break;
                        }
                        #endregion
                    }

                    //验证拓扑
                    TopologyCheckClass.ValidateTopology(pTopo, (featureDataset as IGeoDataset).Extent, out eError);
                    if (eError != null)
                    {
                        TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                        continue;
                    }


                    //获得错误列表
                    GetErrorList(Hook, featureDataset, pTopo,"", "", out eError);
                    if (eError != null)
                    {
                        TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                        continue;
                    }

                    TopologyCheckClass.RemoveTopo(featureDataset, out eError);
                    if (eError != null) return;

                }
                //进度条加1
                pValue++;
                eInfo.Value = pValue;
                GeoDataChecker.GeoDataChecker_ProgressShow((object)GeoDataChecker._ProgressBarInner, eInfo);
            }
        }


        public void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            e.ErrInfo.FunctionCategoryName = "批量检查";
            e.ErrInfo.FunctionName = "拓扑检查";
            e.ErrInfo.ErrID = enumErrorType.面含面检查.GetHashCode();
            DataErrTreat(sender, e);
        }

        /// <summary>
        /// 查询参数配置表里面需要检查的要素类(只包含源要素)
        /// </summary>
        /// <param name="pTable"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private List<string> GetLstFeaClsName(DataTable pTable, out Exception eError)
        {
            eError = null;
            List<string> feaclsNameList = new List<string>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                if (FeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return null;
                }
                if (!feaclsNameList.Contains(FeaClsName))
                {
                    feaclsNameList.Add(FeaClsName);
                }
            }
            return feaclsNameList;
        }

        /// <summary>
        /// 查询参数配置表里面需要检查的要素类(包含源要素和目标要素类)
        /// </summary>
        /// <param name="pTable"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private List<string> GetDicFeaClsName(DataTable pTable, out Exception eError)
        {
            eError = null;

            List<string> FeaClsNameDic = new List<string>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                if ((FeaClsName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("图层名为空或配置不正确!");
                    return null;
                }

                string[] feaNameArr = FeaClsName.Split(new char[] { ';' });
                string oriFeaClsName = feaNameArr[0].Trim();  //源要素类名
                string desFeaClsName = feaNameArr[1].Trim();  //目标要素名
                if (!FeaClsNameDic.Contains(oriFeaClsName + ";" + desFeaClsName))
                {
                    FeaClsNameDic.Add(oriFeaClsName + ";" + desFeaClsName);
                }
            }
            return FeaClsNameDic;
        }


        /// <summary>
        /// 获得错误列表，一般的拓扑检查
        /// </summary>
        /// <param name="pFeaDatset"></param>
        /// <param name="pTopo"></param>
        /// <param name="outError"></param>
        private void GetErrorList(IArcgisDataCheckHook hook, IFeatureDataset pFeaDatset, ITopology pTopo, string pOrgFeaClaName, string desFeaClsName, out Exception outError)
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
                    esriTopologyRuleType pTopoRuleType = pTopoRule.TopologyRuleType;
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
                            if (desID > 0 && desFeaClsID > 0)
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

        #endregion

        /// <summary>
        /// 显示进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataCheck_ProgressShow(object sender, ProgressChangeEvent e)
        { }
    }
}
