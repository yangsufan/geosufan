using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Data.Common;
using ESRI.ArcGIS.Geodatabase ;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;

using System.Collections;
using ESRI.ArcGIS.Carto;

/// <summary>
/// 陈亚飞添加
/// </summary>
namespace GeoDataChecker
{
   //拓扑创建类
    public static class TopologyCheckClass
    {

        //public static string DataCheckPath = @"C:\Documents and Settings\chenyafei\桌面\数据检查测试数据.mdb";
        //public static string DataCheckPath = @"C:\Documents and Settings\chenyafei\桌面\面拓扑检查测试数据.mdb";
        public static string GeoDataCheckParaPath = Application.StartupPath + @"\..\Res\checker\GIS数据检查配置表.mdb";
        public static string GeoLogPath = Application.StartupPath + @"\..\Log\";
      
        /// <summary>
        /// 为数据集创建拓扑
        /// </summary>
        /// <param name="pFeatureDataset"></param>
        /// <param name="outError"></param>
        /// <returns></returns>
        public static ITopology CreateTopo(IFeatureDataset pFeatureDataset, out Exception outError)
        {
            outError = null;

            ITopology pTopo = null;
            try
            {
                ITopologyContainer2 pTopoContainer = pFeatureDataset as ITopologyContainer2;

                try
                {
                    ITopology tempTopo = pTopoContainer.get_TopologyByName(pFeatureDataset.Name);
                    if (tempTopo != null)
                    {
                        RemoveTopo(pFeatureDataset, out outError);
                    }
                }
                catch
                { }
                pTopo = pTopoContainer.CreateTopology(pFeatureDataset.Name,
                                                          pTopoContainer.DefaultClusterTolerance, -1, "");
            }
            catch (Exception ex)
            {
                outError = ex;
            }
            return pTopo;
        }

        /// <summary>
        /// 向拓扑中添加要素类 
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="outError"></param>
        public static void AddFeaClasstoTopo(ITopology pTopo,IFeatureDataset pFeaDataset,out Exception outError)
         {
             outError = null;
             try
             {
                 bool b = false;
                 ITopologyRuleContainer pTopoRulrContainer = pTopo as ITopologyRuleContainer;
                 List<IDataset> LstDataSet = GetAllFeaCls(pFeaDataset);
                 foreach (IDataset pDt in LstDataSet)
                 {
                     IFeatureClass pFeaCls = pDt as IFeatureClass;
                     if (pFeaCls == null) continue;
                     if (pFeaCls.FeatureType != esriFeatureType.esriFTSimple) continue;
                     pTopo.AddClass(pFeaCls as IClass, 5, 1, 1, false);
                     b = true;
                 }
                 if (b == false)
                 {
                     outError = new Exception("要进行检查的要素类不存在！");
                     return;
                 }
             }
             catch (Exception ex)
             {
                 outError = ex;
             }
         }

        /// <summary>
        /// 向拓扑中添加规则
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeaClsNameList"></param>
        /// <param name="pTopoRuleType"></param>
        /// <param name="outError"></param>
        public static void AddFeaRuletoTopo(ITopology pTopo, List<string> pFeaClsNameList,esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;

            try
            {
                ITopologyRuleContainer pRuleContainer = pTopo as ITopologyRuleContainer;
                IFeatureClassContainer pFeaClsContainer = pTopo as IFeatureClassContainer;
                IEnumFeatureClass pEnumFeaCls = pFeaClsContainer.Classes;
                pEnumFeaCls.Reset();
                IFeatureClass pFeaCls = pEnumFeaCls.Next();
                while (pFeaCls != null)
                {
                    string pFeaName =(pFeaCls as IDataset).Name.Trim();
                    if (pFeaName.Contains("."))
                    {
                        pFeaName = pFeaName.Substring(pFeaName.IndexOf('.') + 1);
                    }
                    if (pFeaClsNameList.Contains(pFeaName))
                    {
                   
                        ITopologyRule pTopoRule = new TopologyRuleClass();
                        pTopoRule.TopologyRuleType = pTopoRuleType;
                        pTopoRule.Name = pTopoRule.ToString();
                        pTopoRule.OriginClassID = pFeaCls.FeatureClassID;
                        pTopoRule.AllOriginSubtypes = true;
                        pRuleContainer.AddRule(pTopoRule);
                    }
                    pFeaCls = pEnumFeaCls.Next();
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 将要素类添加到拓扑中
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="pGeometryType"></param>
        /// <param name="outError"></param>
        private static void AddClasstoTopology(ITopology pTopo,IFeatureDataset pFeatureDataset,esriGeometryType pGeometryType,out Exception outError)
        {
            outError = null;
            try
            {
                bool b = false;
                ITopologyRuleContainer pTopoRulrContainer = pTopo as ITopologyRuleContainer;
                List<IDataset> LstDataSet = GetAllFeaCls(pFeatureDataset);
                foreach (IDataset pDt in LstDataSet)
                {
                    IFeatureClass pFeaCls = pDt as IFeatureClass;
                    if (pFeaCls == null) continue;
                    if (pFeaCls.FeatureType != esriFeatureType.esriFTSimple) continue;
                    if (pGeometryType != esriGeometryType.esriGeometryNull)
                    {
                        if (pFeaCls.ShapeType != pGeometryType) continue;
                    }
                    pTopo.AddClass(pFeaCls as IClass, 5, 1, 1, false);
                    b = true;
                }
                if (b == false)
                {
                    outError = new Exception("要进行检查的要素类不存在！");
                    return;
                }
            }
            catch(Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 为要素类创建拓扑规则并添加到拓扑中
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pGeometryTyoe"></param>
        /// <param name="pTopoRuleType"></param>
        /// <param name="outError"></param>
        public static void AddRuletoTopology(ITopology pTopo, esriGeometryType pGeometryType, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError=null;

            try
            {
                ITopologyRuleContainer pRuleContainer = pTopo as ITopologyRuleContainer;
                IFeatureClassContainer pFeaClsContainer = pTopo as IFeatureClassContainer;
                IEnumFeatureClass pEnumFeaCls = pFeaClsContainer.Classes;
                pEnumFeaCls.Reset();
                IFeatureClass pFeaCls = pEnumFeaCls.Next();
                while (pFeaCls != null)
                {
                    if (pFeaCls.ShapeType == pGeometryType)
                    {
                        ITopologyRule pTopoRule = new TopologyRuleClass();
                        pTopoRule.TopologyRuleType = pTopoRuleType;
                        pTopoRule.Name = (pFeaCls as IDataset).Name;
                        pTopoRule.OriginClassID = pFeaCls.FeatureClassID;
                        pTopoRule.AllOriginSubtypes = true;
                        pRuleContainer.AddRule(pTopoRule);
                    }
                    pFeaCls = pEnumFeaCls.Next();
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 将要素类添加到拓扑中并创建拓扑规则
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="pGeometryType"></param>
        /// <param name="pTopoRuleType"></param>
        /// <param name="outError"></param>
        public static void AddRuleandClasstoTopology(ITopology pTopo, IFeatureDataset pFeatureDataset, esriGeometryType pGeometryType, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;
            AddClasstoTopology(pTopo, pFeatureDataset, pGeometryType, out outError);
            if (outError != null) return;

            AddRuletoTopology(pTopo, pTopoRuleType, out outError);
            if (outError != null) return;
        }




        /// <summary>
        /// 将要素类添加到拓扑中，针对异层检查，包括源和目标层
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="feaClsNameDic"></param>
        /// <param name="outError"></param>
        private static void AddClasstoTopology(ITopology pTopo, IFeatureDataset pFeatureDataset, string oriFeaClsName,string desFeaClsName, out Exception outError)
        {
            outError = null;

            try
            {
                ITopologyRuleContainer pTopoRulrContainer = pTopo as ITopologyRuleContainer;
                List<IDataset> LstDataSet = GetAllFeaCls(pFeatureDataset);
                bool b = false;
                foreach (IDataset pDt in LstDataSet)
                {
                    IFeatureClass pFeaCls = pDt as IFeatureClass;
                    if (pFeaCls == null) continue;
                    if (pFeaCls.FeatureType != esriFeatureType.esriFTSimple) continue;
                    string pFeaName = pDt.Name;
                    if(pFeaName.Contains("."))
                    {
                        pFeaName = pFeaName.Substring(pFeaName.IndexOf('.') + 1);
                    }
                    if (pFeaName != oriFeaClsName && pFeaName != desFeaClsName) continue;
                    pTopo.AddClass(pFeaCls as IClass, 5, 1, 1, false);
                    b = true;
                }
                if (b == false)
                {
                    outError = new Exception("要进行检查的要素类不存在！");
                    return;
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 将要素类添加到拓扑中，针对异层检查，包括源和目标层(列表)
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="feaClsNameDic"></param>
        /// <param name="outError"></param>
        private static void AddDicClasstoTopology(ITopology pTopo, IFeatureDataset pFeatureDataset, List<string> feaclsNameDic, out Exception outError)
        {
            outError = null;
            List<string> feaClsNameLst = new List<string>();
            for (int i = 0; i < feaclsNameDic.Count; i++)
            {
                string feaclsDic = feaclsNameDic[i];
                string[] tStrArr = new string[2];
                tStrArr = feaclsDic.Split(new char[] { ';' });
                for (int j = 0; j < tStrArr.Length; j++)
                    if (!feaClsNameLst.Contains(tStrArr[j]))
                    {
                        feaClsNameLst.Add(tStrArr[j]);
                    }
            }

            try
            {
                ITopologyRuleContainer pTopoRulrContainer = pTopo as ITopologyRuleContainer;
                List<IDataset> LstDataSet = GetAllFeaCls(pFeatureDataset);
                bool b = false;
                foreach (IDataset pDt in LstDataSet)
                {
                    IFeatureClass pFeaCls = pDt as IFeatureClass;
                    if (pFeaCls == null) continue;
                    if (pFeaCls.FeatureType != esriFeatureType.esriFTSimple) continue;
                    string pFeaName = pDt.Name;
                    if (pFeaName.Contains("."))
                    {
                        pFeaName = pFeaName.Substring(pFeaName.IndexOf('.') + 1);
                    }
                    if (feaClsNameLst.Contains(pFeaName))
                    {
                        pTopo.AddClass(pFeaCls as IClass, 5, 1, 1, false);
                        b = true;
                    }
                }
                if (b == false)
                {
                    outError = new Exception("要进行检查的要素类不存在！");
                    return;
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 为要素类添加拓扑规则，针对异层检查，包括源和目标层
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="feaclsNameDic"></param>
        /// <param name="pTopoRuleType"></param>
        /// <param name="outError"></param>
        private static void AddRuletoTopology(ITopology pTopo, string oriFeaClsName,string desFeaClsName, esriTopologyRuleType pTopoRuleType, out Exception outError)
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
                ITopologyRule pTopoRule = new TopologyRuleClass();
                pTopoRule.TopologyRuleType = pTopoRuleType;
                while (pFeaCls != null)
                {
                    string pFeaClsName = (pFeaCls as IDataset).Name;
                    if(pFeaClsName.Contains("."))
                    {
                        pFeaClsName = pFeaClsName.Substring(pFeaClsName.IndexOf('.') + 1);
                    }
                    if (pFeaClsName == oriFeaClsName)
                    {
                        pTopoRule.Name = (pFeaCls as IDataset).Name;
                        pTopoRule.OriginClassID = pFeaCls.FeatureClassID;
                        pTopoRule.AllOriginSubtypes = true;
                    }
                    if (pFeaClsName == desFeaClsName)
                    {
                        pTopoRule.DestinationClassID = pFeaCls.FeatureClassID;
                        pTopoRule.AllDestinationSubtypes = true;
                    }
                    pFeaCls = pEnumFeaCls.Next();
                }
                pRuleContainer.AddRule(pTopoRule);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 为要素类添加拓扑规则，针对异层检查，包括源和目标层(列表)
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="feaclsNameDic"></param>
        /// <param name="pTopoRuleType"></param>
        /// <param name="outError"></param>
        public static void AddDicRuletoTopology(ITopology pTopo, List<string> feaclsNameDic, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;

            try
            {
                //设置拓扑规则
                ITopologyRuleContainer pRuleContainer = pTopo as ITopologyRuleContainer;
               
                for (int i = 0; i < feaclsNameDic.Count; i++)
                {
                    string feaclsDic = feaclsNameDic[i];
                    string[] tStrArr = new string[2];
                    tStrArr = feaclsDic.Split(new char[] { ';' });
                    string oriFeaClsName = tStrArr[0];
                    string desFeaClsName = tStrArr[1];
                    
                    IFeatureClassContainer pFeaClsContainer = pTopo as IFeatureClassContainer;
                    IEnumFeatureClass pEnumFeaCls = pFeaClsContainer.Classes;
                    pEnumFeaCls.Reset();
                    IFeatureClass pFeaCls = pEnumFeaCls.Next();
                    ITopologyRule pTopoRule = new TopologyRuleClass();
                    pTopoRule.TopologyRuleType = pTopoRuleType;
                    while (pFeaCls != null)
                    {
                        string pFeaClsName = (pFeaCls as IDataset).Name;
                        if (pFeaClsName.Contains("."))
                        {
                            pFeaClsName = pFeaClsName.Substring(pFeaClsName.IndexOf('.') + 1);
                        }
                        if (pFeaClsName == oriFeaClsName)
                        {
                            pTopoRule.Name = (pFeaCls as IDataset).Name;
                            pTopoRule.OriginClassID = pFeaCls.FeatureClassID;
                            pTopoRule.AllOriginSubtypes = true;
                        }
                        if (pFeaClsName == desFeaClsName)
                        {
                            pTopoRule.DestinationClassID = pFeaCls.FeatureClassID;
                            pTopoRule.AllDestinationSubtypes = true;
                        }
                        pFeaCls = pEnumFeaCls.Next();
                    }
                    pRuleContainer.AddRule(pTopoRule);
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 将要素类添加到拓扑中，并为要素类添加拓扑规则，针对异层检查，包括源和目标层
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="feaclsNameDic"></param>
        /// <param name="pTopoRuleType"></param>
        /// <param name="outError"></param>
        public static void AddRuleandClasstoTopology(ITopology pTopo, IFeatureDataset pFeatureDataset, string oriFeaClsName, string desFeaClsName, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;
            AddClasstoTopology(pTopo, pFeatureDataset,oriFeaClsName,desFeaClsName, out outError);
            if (outError != null) return;

            AddRuletoTopology(pTopo, oriFeaClsName,desFeaClsName, pTopoRuleType, out outError);
            if (outError != null) return;
        }

        /// <summary>
        /// 将要素类添加到拓扑中，并为要素类添加拓扑规则，针对异层检查，包括源和目标层(列表)
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="feaclsNameDic"></param>
        /// <param name="pTopoRuleType"></param>
        /// <param name="outError"></param>
        public static void AddDicRuleandClasstoTopology(ITopology pTopo, IFeatureDataset pFeatureDataset,List<string> feaClsNameDic, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;
            AddDicClasstoTopology(pTopo, pFeatureDataset,feaClsNameDic, out outError);
            if (outError != null) return;

            AddDicRuletoTopology(pTopo, feaClsNameDic, pTopoRuleType, out outError);
            if (outError != null) return;
        }


        /// <summary>
        /// 将要素类添加到拓扑中，针对针对特殊的要素类检查
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="feaClsNameLst"></param>
        /// <param name="outError"></param>
        private static void AddClasstoTopology(ITopology pTopo, IFeatureDataset pFeatureDataset, string pFeaClsName, out Exception outError)
        {
            outError = null;

            try
            {
                ITopologyRuleContainer pTopoRulrContainer = pTopo as ITopologyRuleContainer;
                List<IDataset> LstDataSet = GetAllFeaCls(pFeatureDataset);
                bool b = false;
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
                    return;
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 将要素类添加到拓扑中，针对针对特殊的要素类检查(列表)
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="feaClsNameLst"></param>
        /// <param name="outError"></param>
        private static void AddClasstoTopology(ITopology pTopo, IFeatureDataset pFeatureDataset, List<string> pFeaClsNameList, out Exception outError)
        {
            outError = null;

            try
            {
                ITopologyRuleContainer pTopoRulrContainer = pTopo as ITopologyRuleContainer;
                List<IDataset> LstDataSet = GetAllFeaCls(pFeatureDataset);
                bool b = false;
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
                    if (pFeaClsNameList.Contains(pFeaName))
                    {
                        pTopo.AddClass(pFeaCls as IClass, 5, 1, 1, false);
                        b = true;
                    }
                }
                if (b == false)
                {
                    outError = new Exception("要进行检查的要素类不存在！");
                    return;
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }


        /// <summary>
        /// 为要素类添加拓扑规则，针对针对特殊的要素类检查
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pTopoRuleType"></param>
        /// <param name="outError"></param>
        public static void AddRuletoTopology(ITopology pTopo, esriTopologyRuleType pTopoRuleType, out Exception outError)
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
                outError = ex;
            }
        }

        /// <summary>
        /// 将要素类添加到拓扑中，并为要素类添加拓扑规则，针对针对特殊的要素类检查
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="feaClsNameLst"></param>
        /// <param name="pTopoRuleType"></param>
        /// <param name="outError"></param>
        public static void AddRuleandClasstoTopology(ITopology pTopo, IFeatureDataset pFeatureDataset, string pFeaClsName, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;
            AddClasstoTopology(pTopo, pFeatureDataset,pFeaClsName, out outError);
            if (outError != null) return;

            AddRuletoTopology(pTopo, pTopoRuleType, out outError);
            if (outError != null) return;
        }

        /// <summary>
        /// 将要素类列表添加到拓扑中，并为要素类添加拓扑规则，针对针对特殊的要素类检查 
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="feaClsNameLst"></param>
        /// <param name="pTopoRuleType"></param>
        /// <param name="outError"></param>
        public static void AddRuleandClasstoTopology(ITopology pTopo, IFeatureDataset pFeatureDataset, List<string> pFeaClsNameList, esriTopologyRuleType pTopoRuleType, out Exception outError)
        {
            outError = null;
            AddClasstoTopology(pTopo, pFeatureDataset, pFeaClsNameList, out outError);
            if (outError != null) return;

            AddRuletoTopology(pTopo, pTopoRuleType, out outError);
            if (outError != null) return;
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
        /// 移除拓扑规则
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="outError"></param>
       private static void RemoveTopoRule(ITopology pTopo, out Exception outError)
        {
            outError = null;
            try
            {
                ITopologyRuleContainer pTopoRuleCon = pTopo as ITopologyRuleContainer;
                IEnumRule pEnumRule = pTopoRuleCon.Rules;
                pEnumRule.Reset();
                IRule pRule = pEnumRule.Next();
                while (pRule != null && pRule is ITopologyRule)
                {
                    pTopoRuleCon.DeleteRule(pRule as ITopologyRule);
                    pRule = pEnumRule.Next();
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 移除拓扑类
        /// </summary>
        /// <param name="pTopo"></param>
        /// <param name="outError"></param>
        private static void RemoveTopoClass(ITopology pTopo, out Exception outError)
        {
            outError = null;
            try
            {
                IFeatureClassContainer pFeaClsCon = pTopo as IFeatureClassContainer;
                for (int j = pFeaClsCon.ClassCount - 1; j >= 0; j--)
                {
                    pTopo.RemoveClass(pFeaClsCon.get_Class(j) as IClass );
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 移除拓扑
        /// </summary>
        /// <param name="pFeaDataet"></param>
        /// <param name="pTopoName"></param>
        /// <param name="outError"></param>
        public static void RemoveTopo(IFeatureDataset pFeaDataet,out Exception outError)
        {
            outError = null;
            try
            {
                ITopologyContainer pTopoCon = pFeaDataet as ITopologyContainer;
                ITopology pTopo = pTopoCon.get_TopologyByName(pFeaDataet.Name);

                //删除该拓扑下所有的拓扑规则
                RemoveTopoRule(pTopo, out outError);
                if (outError != null) return;

                //删除该拓扑下左右的拓扑图层
                RemoveTopoClass(pTopo,out outError);
                if (outError != null) return;

                //删除拓扑
                (pTopo as IDataset).Delete();
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 获得数据集下所有的要素类
        /// </summary>
        /// <param name="pFeatureDataset"></param>
        /// <returns></returns>
        private  static List<IDataset> GetAllFeaCls(IFeatureDataset pFeatureDataset)
        {
            List<IDataset> LstDT = new List<IDataset>();

            IEnumDataset pEnumDt = pFeatureDataset.Subsets;
            pEnumDt.Reset();
            IDataset pDataset = pEnumDt.Next();
            while (pDataset != null)
            {
                LstDT.Add(pDataset);
                pDataset = pEnumDt.Next();
            }
            return LstDT;
        }

        //获取错误要素的定位点(暂时为要素第一个点)
        public static IPoint GetPointofFeature(IFeature feature)
        {
            IPointCollection pntCol = feature.Shape as IPointCollection;
            if (pntCol == null) return null;
            return pntCol.get_Point(0);
        }


        /// <summary>
        /// 查找参数值表格
        /// </summary>
        /// <param name="pSysTable"></param>
        /// <param name="checkParaID">参数ID，唯一标识检查类型</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static DataTable GetParaValueTable(SysCommon.DataBase.SysTable pSysTable, int checkParaID, out Exception eError)
        {
            eError = null;
            DataTable mTable = null;

            string selStr = "select * from GeoCheckPara where 参数ID=" + checkParaID;
            DataTable pTable = pSysTable.GetSQLTable(selStr, out eError);
            if (eError != null)
            {
                eError = new Exception("查询表格错误，表名为：GeoCheckPara，参数ID为:" + checkParaID);
                return null;
            }

            if (pTable == null || pTable.Rows.Count == 0)
            {
                eError = new Exception("找不到记录，参数ID为:" + checkParaID);
                return null;
            }
            string ParaType = pTable.Rows[0]["参数类型"].ToString().Trim();            //参数类型
            if (ParaType == "GeoCheckParaValue")
            {
                int ParaValue = int.Parse(pTable.Rows[0]["参数值"].ToString().Trim());   //参数值，用来标识检查类型
                string str = "select * from GeoCheckParaValue where 检查类型=" + ParaValue;
                mTable = pSysTable.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    eError = new Exception("查询表格错误，表名为：GeoCheckParaValue，检查类型为:" + ParaValue);
                    return null;
                }
            }
            return mTable;
        }

        /// <summary>
        /// 查找参数值表格
        /// </summary>
        /// <param name="pFeaDataset"></param>
        /// <param name="pSysTable"></param>
        /// <param name="checkParaID">参数ID，唯一标识检查类型</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static DataTable GetParaValueTable(IFeatureDataset pFeaDataset, SysCommon.DataBase.SysTable pSysTable, int checkParaID, out Exception eError)
        {
            eError = null;
            DataTable mTable = null;

            string selStr = "select * from GeoCheckPara where 参数ID=" + checkParaID;
            DataTable pTable = pSysTable.GetSQLTable(selStr, out eError);
            if (eError != null)
            {
                eError = new Exception("查询表格错误，表名为：GeoCheckPara，参数ID为:" + checkParaID);
                return null;
            }

            if (pTable == null || pTable.Rows.Count == 0)
            {
                eError = new Exception("找不到记录，参数ID为:" + checkParaID);
                return null;
            }
            string ParaType = pTable.Rows[0]["参数类型"].ToString().Trim();            //参数类型
            if (ParaType == "GeoCheckParaValue")
            {
                int ParaValue = int.Parse(pTable.Rows[0]["参数值"].ToString().Trim());   //参数值，用来标识检查类型
                string feaDTName = pFeaDataset.Name;                                   //数据集名称 
                if(feaDTName.Contains("."))
                {
                    feaDTName = feaDTName.Substring(feaDTName.IndexOf('.') + 1);
                }
                string str = "select * from GeoCheckParaValue where 检查类型=" + ParaValue + " and 数据集='" + feaDTName + "'";
                mTable = pSysTable.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    eError = new Exception("查询表格错误，表名为：GeoCheckParaValue，检查类型为:" + ParaValue);
                    return null;
                }
            }
            return mTable;
        }

        /// <summary>
        /// 查找分类代码字段名称
        /// </summary>
        /// <param name="pSysTable"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static string GetCodeName(SysCommon.DataBase.SysTable pSysTable, out Exception eError)
        {
            eError = null;
            string codeName = "";    //分类代码字段名称
            string selStr = "select * from GeoCheckPara where 参数ID=1";
            DataTable pTable = pSysTable.GetSQLTable(selStr, out eError);
            if (eError != null)
            {
                eError = new Exception("查找分类代码字段名称失败！");
            }

            if (pTable == null || pTable.Rows.Count == 0)
            {
                eError = new Exception("找不到分类代码字段名称记录!");
            }
            codeName = pTable.Rows[0]["参数值"].ToString().Trim();
            if (codeName == "")
            {
                eError = new Exception("配置表中未配置分类代码字段名，请检查！");
            }
            return codeName;
        }


        /// <summary>
        /// 根据参数ID获得参数值
        /// </summary>
        /// <param name="pSysTable"></param>
        /// <param name="checkParaID"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static string GetParaValue(SysCommon.DataBase.SysTable pSysTable, int checkParaID, out Exception eError)
        {
            eError = null;
            string paraValue = "";

            string selStr = "select * from GeoCheckPara where 参数ID=" + checkParaID;
            DataTable pTable = pSysTable.GetSQLTable(selStr, out eError);
            if (eError != null)
            {
                eError = new Exception("查询表格错误，表名为：GeoCheckPara，参数ID为:" + checkParaID);
                return "";
            }

            if (pTable == null || pTable.Rows.Count == 0)
            {
                eError = new Exception("找不到记录，参数ID为:" + checkParaID);
                return "";
            }
            paraValue = pTable.Rows[0]["参数值"].ToString().Trim();            //参数类型
            return paraValue;
        }

       
    }

    //数据检查类
    public class DataCheckClass
    {
        private DbConnection _ErrDbCon = null;                       //错误日志表连接
        public DbConnection ErrDbCon
        {
            get
            {
                return _ErrDbCon;
            }
            set
            {
                _ErrDbCon = value;
            }
        }
        private string _ErrTableName = "";                            //错误日志表名
        public string ErrTableName
        {
            get
            {
                return _ErrTableName;
            }
            set
            {
                _ErrTableName = value;
            }
        }


        public event DataErrTreatHandle DataErrTreat;

        private DataTable _ResultTable =new DataTable();
        private Plugin.Application.IAppGISRef hook = null;

        private void CreateTable()
        {
            _ResultTable.Columns.Add("检查功能名", typeof(string));
            _ResultTable.Columns.Add("错误类型", typeof(string));
            _ResultTable.Columns.Add("错误描述", typeof(string));
            _ResultTable.Columns.Add("数据图层1", typeof(string));
            _ResultTable.Columns.Add("要素OID1", typeof(string));
            _ResultTable.Columns.Add("数据图层2", typeof(string));
            _ResultTable.Columns.Add("要素OID2", typeof(string));
            _ResultTable.Columns.Add("检查时间", typeof(string));
            _ResultTable.Columns.Add("定位点X", typeof(string));
            _ResultTable.Columns.Add("定位点Y", typeof(string));
            _ResultTable.Columns.Add("数据文件名", typeof(string));
        }

        //构造函数
        public DataCheckClass(Plugin.Application.IAppGISRef phook)
        {
            CreateTable();
            hook = phook;
            if (hook.DataCheckGrid.RowCount > 0)
            {
                hook.DataCheckGrid.DataSource = null;
            }
            hook.DataCheckGrid.DataSource = _ResultTable;
            hook.DataCheckGrid.SelectionMode=DataGridViewSelectionMode.FullRowSelect;
            DataErrTreat += new DataErrTreatHandle(GeoDataChecker_DataErrTreat);
        }

        #region 拓扑检查
        //一般的拓扑检查，针对同层检查
        public void OrdinaryTopoCheck(IFeatureDataset pFeaDatset, esriGeometryType pGeometryType, esriTopologyRuleType pTopoRuleType, out Exception outError)
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
            TopologyCheckClass.AddRuleandClasstoTopology(pTopo, pFeaDatset, pGeometryType, pTopoRuleType, out exError);
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
            GetErrorList(pFeaDatset, pTopo,pTopoRuleType,"","", out exError);
            if (exError != null)
            {
                TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
                outError = exError;
                return;
            }
            TopologyCheckClass.RemoveTopo(pFeaDatset, out outError);
            if (outError != null) return;
        }
 
        //一般的拓扑检查，针对异层检查，包括源和目标层
        public void OrdinaryTopoCheck(IFeatureDataset pFeaDatset, string oriFeaClsName, string desFeaClsName, esriTopologyRuleType pTopoRuleType, out Exception outError)
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
            TopologyCheckClass.AddRuleandClasstoTopology(pTopo, pFeaDatset, oriFeaClsName,desFeaClsName, pTopoRuleType, out exError);
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
            GetErrorList(pFeaDatset, pTopo, pTopoRuleType, oriFeaClsName, desFeaClsName, out exError);
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
        public void OrdinaryDicTopoCheck(IFeatureDataset pFeaDatset, List<string> feaClsNameDic, esriTopologyRuleType pTopoRuleType, out Exception outError)
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
            GetErrorList(pFeaDatset, pTopo, pTopoRuleType, "", "", out exError);
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
        public void OrdinaryTopoCheck(IFeatureDataset pFeaDatset, string pFeaClsName, esriTopologyRuleType pTopoRuleType, out Exception outError)
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
            TopologyCheckClass.AddRuleandClasstoTopology(pTopo, pFeaDatset,pFeaClsName, pTopoRuleType, out exError);
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
            GetErrorList(pFeaDatset, pTopo, pTopoRuleType,pFeaClsName,"", out exError);
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
        public void OrdinaryTopoCheck(IFeatureDataset pFeaDatset, List<string> pFeaClsNameList, esriTopologyRuleType pTopoRuleType, out Exception outError)
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
            GetErrorList(pFeaDatset, pTopo, pTopoRuleType, "", "", out exError);
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
        /// 线穿面检查
        /// </summary>
        /// <param name="pFeaDatset"></param>
        /// <param name="oriFeaClsName">线要素类名称</param>
        /// <param name="desFeaClsName">面要素类名称</param>
        /// <param name="outError"></param>
        public void CrossTopoCheck(IFeatureDataset pFeaDatset, string oriFeaClsName, string desFeaClsName, out Exception outError)
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
                        DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
                        mLineErrFeature = pCursor.NextFeature();
                    }

                    //释放cursor
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

                    mAreaFea = mAreaCursor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mAreaCursor);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 线悬挂点检查
        /// </summary>
        /// <param name="pFeaDatset"></param>
        /// <param name="oriFeaClsName">要检查的线要素类名称</param>
        /// <param name="oriWhereStr">线要素过滤条件</param>
        /// <param name="desFeaClsName">目标要素类名称</param>
        /// <param name="desWhereStr">目标要素过滤条件</param>
        /// <param name="tolenrence">点缓冲容差</param>
        /// <param name="outError"></param>
        public void LineDangleCheck(IFeatureDataset pFeaDatset, string oriFeaClsName, string oriWhereStr, string desFeaClsName, string desWhereStr, double tolenrence, out Exception outError)
        {
            outError = null;
            try
            {
                //ErrorEventArgs pErrorArgs = new ErrorEventArgs();

                //根据要素类名称获得线要素类和其他要素类
                IFeatureClass pLineFeCls = (pFeaDatset.Workspace as IFeatureWorkspace).OpenFeatureClass(oriFeaClsName);
                IFeatureClass pDesFeaCls = (pFeaDatset.Workspace as IFeatureWorkspace).OpenFeatureClass(desFeaClsName);
                
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = oriWhereStr;
                IFeatureCursor pOriCursor = pLineFeCls.Search(pQueryFilter, false);
                if (pOriCursor == null) return;
                IFeature lineFea = pOriCursor.NextFeature();
                while (lineFea != null)
                {
                    string eerStr = "获取线要素端点出错，线要素OID为：";
                    //对符合条件的线要素进行线悬挂点检查

                    //查找线要素的两个端点
                    IPointCollection mPointCol = new PolylineClass();
                    mPointCol.AddPointCollection(lineFea.ShapeCopy as IPointCollection);
                    //线要素两个端点
                    IPoint firstPoint = new PointClass();
                    IPoint lastPoint = new PointClass();
                    mPointCol.QueryPoint(0, firstPoint);
                    if (firstPoint == null)
                    {
                        eerStr += lineFea.OID + ",";
                        outError = new Exception(eerStr.Substring(0, eerStr.Length - 1));
                        lineFea = pOriCursor.NextFeature();
                        continue;
                    }
                    mPointCol.QueryPoint(mPointCol.PointCount - 1, lastPoint);
                    if (lastPoint == null)
                    {
                        eerStr += lineFea.OID + ",";
                        outError = new Exception(eerStr.Substring(0, eerStr.Length - 1));
                        lineFea = pOriCursor.NextFeature();
                        continue;
                    }

                    //根据给定的容差，分别对线要素的两个端点进行缓冲
                    //声明缓冲范围
                    IGeometry firstGeo = null;
                    IGeometry lastGeo = null;
                    ITopologicalOperator pTopoOper = firstPoint as ITopologicalOperator;
                    firstGeo = pTopoOper.Buffer(tolenrence);
                    pTopoOper = lastPoint as ITopologicalOperator;
                    lastGeo = pTopoOper.Buffer(tolenrence);
                    if (firstGeo == null || lastGeo == null)
                    {
                        lineFea = pOriCursor.NextFeature();
                        continue;
                    }

                    # region 根据缓冲范围在目标要素类中进行查找，若找不到要素，则该线要素存在悬挂点错误
                    //先根据第一个端点的缓冲范围进行查找
                    ISpatialFilter spatialFilter = new SpatialFilterClass();
                    spatialFilter.WhereClause = desWhereStr;
                    spatialFilter.Geometry = firstGeo;
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                    spatialFilter.SpatialRelDescription = "***TT****";
                    IFeatureCursor desFeaCursor = pDesFeaCls.Search(spatialFilter, false);
                    if (desFeaCursor == null) return;
                    IFeature desFea = desFeaCursor.NextFeature();
                    if (desFea == null)
                    {
                        //没有找到与线首端点相交的要素，该要素存在悬挂点错误

                        //将错误结果显示出来
                        GetErrorList(pFeaDatset , oriFeaClsName, lineFea);
                    }
                    else
                    {
                        //根据尾端点的缓冲范围进行查找
                        spatialFilter = new SpatialFilterClass();
                        spatialFilter.WhereClause = desWhereStr;
                        spatialFilter.Geometry = lastGeo;
                        spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                        spatialFilter.SpatialRelDescription = "***TT****";
                        IFeatureCursor desFeaCursor2 = pDesFeaCls.Search(spatialFilter, false);
                        if (desFeaCursor2 == null) return;
                        IFeature desFea2 = desFeaCursor2.NextFeature();
                        if (desFea2 == null)
                        {
                            //没有找到与线首端点相交的要素，该要素存在悬挂点错误

                            //将错误结果显示出来
                            GetErrorList(pFeaDatset, oriFeaClsName, lineFea);
                        }

                        //释放cursor
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(desFeaCursor2);
                    }

                    //释放cursor
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(desFeaCursor);
                    #endregion
                    lineFea = pOriCursor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pOriCursor);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }
        
        /// <summary>
        /// 获得错误列表，一般的拓扑检查
        /// </summary>
        /// <param name="pFeaDatset"></param>
        /// <param name="pTopo"></param>
        /// <param name="outError"></param>
        private void GetErrorList(IFeatureDataset pFeaDatset, ITopology pTopo, esriTopologyRuleType pTopoRuleType, string pOrgFeaClsName, string desFeaClsName, out Exception outError)
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
                //ErrorEventArgs pErrorArgs = new ErrorEventArgs();
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

                        IFeatureClass oriFeaCls = null;
                        //if (oriFeaClsID <= 0)
                        //{
                        //    outError = new Exception("拓扑检查结果显示不正确，找不到源要素类ID！");
                        //    return;
                        //    //pErrorFea = pEnumErrorFea.Next();
                        //    //continue;
                        //}
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
                            if (desID >0 && desFeaClsID >0)
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
                        ErrorLst.Add(errID );//错误id;
                        ErrorLst.Add(errDes);//错误描述
                        ErrorLst.Add(pMapx);    //...
                        ErrorLst.Add(pMapy);    //...

                        
                        if(oriFeaCls!=null)
                        {
                            ErrorLst.Add((oriFeaCls as IDataset).Name);
                            ErrorLst.Add(oriID);
                        }
                        else 
                        {
                            ErrorLst.Add(pOrgFeaClsName);
                            ErrorLst.Add(-1);
                        }
                       
                        ErrorLst.Add(desFeaClsName);
                        ErrorLst.Add(desID);
                        ErrorLst.Add(false);
                        ErrorLst.Add(System.DateTime.Now.ToString());

                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
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
        /// 获得错误列表 ,线悬挂点检查
        /// </summary>
        /// <param name="pFeaDatset"></param>
        /// <param name="oriFeaclsName"></param>
        /// <param name="oriFeature"></param>
        /// <param name="desFeaclsName"></param>
        /// <param name="desFeature"></param>
        private void GetErrorList(IFeatureDataset pFeaDatset, string oriFeaclsName, IFeature oriFeature)
        {
            //保存错误结果
            IPoint pPoint = TopologyCheckClass.GetPointofFeature(oriFeature);
            double pMapx = pPoint.X;
            double pMapy = pPoint.Y;

            List<object> ErrorLst = new List<object>();
            ErrorLst.Add("线拓扑检查");//功能组名称
            ErrorLst.Add("线悬挂点检查");//功能名称
            ErrorLst.Add((pFeaDatset as IDataset).Workspace.PathName);  //数据文件名
            ErrorLst.Add(enumErrorType.线存在悬挂点.GetHashCode());//错误id;
            ErrorLst.Add("不允许线要素有悬结点，即每一条线段的端点都不能孤立");//错误描述
            ErrorLst.Add(pMapx);    //...
            ErrorLst.Add(pMapy);    //...
            ErrorLst.Add(oriFeaclsName);
            ErrorLst.Add(oriFeature.OID );
            ErrorLst.Add("");
            ErrorLst.Add(-1);
            ErrorLst.Add(false);
            ErrorLst.Add(System.DateTime.Now.ToString());

            //传递错误日志
            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
        }

        #endregion


        /// <summary>
        /// 数学基础正确性检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="coorStr">空间参考字符串</param>
        /// <param name="outError"></param>
        public void MathematicsCheck(IFeatureDataset pFeatureDataset, string coorStr, out Exception outError)
        {
            outError = null;

            //获得数据集的空间参考
            IGeoDataset pGeoDataset = pFeatureDataset as IGeoDataset;
            ISpatialReference pSpatialRef = pGeoDataset.SpatialReference;

            string strSpatialDes = "";//空间参考字符串

            #region 导出 空间参考字符串
            int byteRead = -1;
            try
            {
                IESRISpatialReferenceGEN pSpatialRefGEN = null;
                IGeographicCoordinateSystem pGeoCoor = new GeographicCoordinateSystemClass();
                IProjectedCoordinateSystem pProCoor = new ProjectedCoordinateSystemClass();
                IUnknownCoordinateSystem pUnknownCoor = new UnknownCoordinateSystemClass();
                pGeoCoor = pSpatialRef as IGeographicCoordinateSystem;
                pProCoor = pSpatialRef as IProjectedCoordinateSystem;
                pUnknownCoor = pSpatialRef as IUnknownCoordinateSystem;

                if (pGeoCoor != null)
                {
                    pSpatialRefGEN = pGeoCoor as IESRISpatialReferenceGEN;
                }
                if (pProCoor != null)
                {
                    pSpatialRefGEN = pProCoor as IESRISpatialReferenceGEN;
                }
                if (pSpatialRefGEN != null)
                {
                    pSpatialRefGEN.ExportToESRISpatialReference(out strSpatialDes, out byteRead);
                    if (strSpatialDes == "")
                    {
                        outError = new Exception("导出空间参考出错！");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                outError = ex;
                return;
            }
            #endregion

            //与标准的空间参考字符串对比检查
            if (strSpatialDes != coorStr)
            {
                //用来保存错误结果
                List<object> ErrorLst = new List<object>();
                ErrorLst.Add("数学基础正确性检查");//功能组名称
                ErrorLst.Add("数学基础正确性检查");//功能名称
                ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                ErrorLst.Add(enumErrorType.数学基础正确性检查.GetHashCode());//错误id;
                ErrorLst.Add("数据集" + pFeatureDataset.Name + "的空间参考与标准的空间参考不一致");//错误描述
                ErrorLst.Add(0);    //...
                ErrorLst.Add(0);    //...
                ErrorLst.Add("");
                ErrorLst.Add(-1);
                ErrorLst.Add("");
                ErrorLst.Add(-1);
                ErrorLst.Add(false);
                ErrorLst.Add(System.DateTime.Now.ToString());

                //传递错误日志
                IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
            }
        }

        /// <summary>
        /// 数学基础正确性检查
        /// </summary>
        /// <param name="pFeatureDataset"></param>
        /// <param name="standardRef"></param>
        /// <param name="outError">标准的空间参考</param>
        public void MathematicsCheck(IFeatureDataset pFeatureDataset,ISpatialReference standardRef, out Exception outError)
        {
            outError = null;

            //标准空间参考字符串
            string sptialStandStr = "";
            int bytesWrote1;
            IESRISpatialReferenceGEN pSpatialStandGEN = standardRef as IESRISpatialReferenceGEN;
            pSpatialStandGEN.ExportToESRISpatialReference(out sptialStandStr, out bytesWrote1);

            //获得数据集的空间参考
            IGeoDataset pGeoDataset = pFeatureDataset as IGeoDataset;
            ISpatialReference pSpatialRef = pGeoDataset.SpatialReference;
            IESRISpatialReferenceGEN pSpatialGEN = pSpatialRef as IESRISpatialReferenceGEN;
            string spatialStr = "";
            int bytesWrote;
            pSpatialGEN.ExportToESRISpatialReference(out spatialStr, out bytesWrote);
           
            //与标准的空间参考对比检查
            //if (pSpatialRef!=standardRef)
            if (sptialStandStr != spatialStr)
            {
                //用来保存错误结果
                List<object> ErrorLst = new List<object>();
                ErrorLst.Add("数学基础正确性检查");//功能组名称
                ErrorLst.Add("数学基础正确性检查");//功能名称
                ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                ErrorLst.Add(enumErrorType.数学基础正确性检查.GetHashCode());//错误id;
                ErrorLst.Add("数据集" + pFeatureDataset.Name + "的空间参考与标准的空间参考不一致");//错误描述
                ErrorLst.Add(0);    //...
                ErrorLst.Add(0);    //...
                ErrorLst.Add("");
                ErrorLst.Add(-1);
                ErrorLst.Add("");
                ErrorLst.Add(-1);
                ErrorLst.Add(false);
                ErrorLst.Add(System.DateTime.Now.ToString());

                //传递错误日志
                IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
            }
        }

        /// <summary>
        /// 数学基础正确性检查
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <param name="standardRef"></param>
        /// <param name="outError"></param>
        public void MathematicsCheck(IFeatureClass pFeatureClass, ISpatialReference standardRef, out Exception outError)
        {
            outError = null;
            
            //标准空间参考字符串
            string sptialStandStr = "";
            int bytesWrote1;
            IESRISpatialReferenceGEN pSpatialStandGEN = standardRef as IESRISpatialReferenceGEN;
            pSpatialStandGEN.ExportToESRISpatialReference(out sptialStandStr, out bytesWrote1);

            //获得数据集的空间参考
            IGeoDataset pGeoDataset = pFeatureClass as IGeoDataset;
            ISpatialReference pSpatialRef = pGeoDataset.SpatialReference;
            IESRISpatialReferenceGEN pSpatialGEN = pSpatialRef as IESRISpatialReferenceGEN;
            string spatialStr = "";
            int bytesWrote;
            pSpatialGEN.ExportToESRISpatialReference(out spatialStr, out bytesWrote);
            


            //与标准的空间参考对比检查
            //if (pSpatialRef != standardRef)
            if (sptialStandStr != spatialStr)
            {
                //用来保存错误结果
                List<object> ErrorLst = new List<object>();
                ErrorLst.Add("数学基础正确性检查");//功能组名称
                ErrorLst.Add("数学基础正确性检查");//功能名称
                ErrorLst.Add((pFeatureClass as IDataset).Workspace.PathName);  //数据文件名
                ErrorLst.Add(enumErrorType.数学基础正确性检查.GetHashCode());//错误id;
                ErrorLst.Add("数据集" + (pFeatureClass as IDataset).Name + "的空间参考与标准的空间参考不一致");//错误描述
                ErrorLst.Add(0);    //...
                ErrorLst.Add(0);    //...
                ErrorLst.Add("");
                ErrorLst.Add(-1);
                ErrorLst.Add("");
                ErrorLst.Add(-1);
                ErrorLst.Add(false);
                ErrorLst.Add(System.DateTime.Now.ToString());

                //传递错误日志
                IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
            }
        }


        #region 要素属性检查

        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="FeaClsNameDic">图层名和属性非空的字段名对</param>
        /// <param name="outError"></param>
        public void IsNullableCheck( IFeatureDataset pFeatureDataset, Dictionary<string, List<string>> FeaClsNameDic, out Exception outError)
        {
            outError = null;
            try
            {
                foreach (KeyValuePair<string, List<string>> item in FeaClsNameDic)
                {
                    IFeatureClass pFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(item.Key.Trim());
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        string fieldName = item.Value[i].Trim();
                        int index = pFeaCls.Fields.FindField(fieldName);
                        if (index == -1)
                        {
                            outError = new Exception("找不到字段名为" + fieldName + "的字段");
                            return;
                        }
                        if (pFeaCls.Fields.get_Field(index).IsNullable == true)
                        {
                            //字段属性为空，与标准不一致，将结果保存起来
                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("空值检查");//功能名称
                            ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                            ErrorLst.Add(enumErrorType.空值检查.GetHashCode());//错误id;
                            ErrorLst.Add("图层" + item.Key + "的字段" + fieldName + "属性值不允许为空");//错误描述
                            ErrorLst.Add(0);    //...
                            ErrorLst.Add(0);    //...
                            ErrorLst.Add(item.Key);
                            ErrorLst.Add(-1);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
                        }
                        else
                        {
                            IFeatureCursor pFeaCusor = pFeaCls.Search(null, false);
                            if(pFeaCusor==null) return;
                            IFeature pFeature = pFeaCusor.NextFeature();
                            if (pFeature == null) continue;
                            while (pFeature != null)
                            {
                                object fieldValue = pFeature.get_Value(index);
                                if (fieldValue == null || fieldValue.ToString() == "")
                                {
                                    //字段值不能为空，将检查结果保存起来
                                    double pMapx = 0.0;
                                    double pMapy = 0.0;
                                    IPoint pPoint = new PointClass();
                                    if (pFeaCls.ShapeType != esriGeometryType.esriGeometryPoint)
                                    {
                                        pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                                    }
                                    else
                                    {
                                        pPoint = pFeature.Shape as IPoint;
                                    }
                                    pMapx = pPoint.X;
                                    pMapy = pPoint.Y;
                                    List<object> ErrorLst = new List<object>();
                                    ErrorLst.Add("要素属性检查");//功能组名称
                                    ErrorLst.Add("空值检查");//功能名称
                                    ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                                    ErrorLst.Add(enumErrorType.空值检查.GetHashCode());//错误id;
                                    ErrorLst.Add("图层" + item.Key + "的字段" + fieldName + "的值不能为空");//错误描述
                                    ErrorLst.Add(pMapx);    //...
                                    ErrorLst.Add(pMapy);    //...
                                    ErrorLst.Add(item.Key);
                                    ErrorLst.Add(pFeature.OID);
                                    ErrorLst.Add("");
                                    ErrorLst.Add(-1);
                                    ErrorLst.Add(false);
                                    ErrorLst.Add(System.DateTime.Now.ToString());

                                    //传递错误日志
                                    IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                                    DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                                    DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
                                }
                                pFeature = pFeaCusor.NextFeature();
                            }

                            //释放cursor
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCusor);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="pFeatureClassLst"></param>
        /// <param name="FeaClsNameDic"></param>
        /// <param name="outError"></param>
        public void IsNullableCheck(List<IFeatureClass> pFeatureClassLst, Dictionary<string, List<string>> FeaClsNameDic, out Exception outError)
        {
            outError = null;
            try
            {

                if (hook.DataTree == null) return;
                hook.DataTree.Nodes.Clear();
                //创建处理树图
                IntialTree(hook.DataTree);
                //设置树节点颜色
                setNodeColor(hook.DataTree);
                hook.DataTree.Tag = false;

                foreach (KeyValuePair<string, List<string>> item in FeaClsNameDic)
                {
                    IFeatureClass pFeaCls = null;
                    foreach(IFeatureClass pFeaClsItem in pFeatureClassLst)
                    {
                        string pFeaClsNm= (pFeaClsItem as IDataset).Name;
                        if(pFeaClsNm.Contains("."))
                        {
                            pFeaClsNm = pFeaClsNm.Substring(pFeaClsNm.IndexOf('.') + 1);
                        }
                        if (pFeaClsNm == item.Key)
                        {
                            pFeaCls = pFeaClsItem;
                            break;
                        }
                    }
                    if(pFeaCls==null) continue;
                    //IFeatureClass pFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(item.Key.Trim());


                    //创建树图节点(以图层名作为根结点)
                    DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                    pNode = (DevComponents.AdvTree.Node)CreateAdvTreeNode(hook.DataTree.Nodes, item.Key, item.Key, hook.DataTree.ImageList.Images[6], true);//图层名节点
                    //显示进度条
                    ShowProgressBar(true);

                    int tempValue = 0;
                    ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, 0, item.Value.Count, tempValue);

                   
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        string fieldName = item.Value[i].Trim();
                        int index = pFeaCls.Fields.FindField(fieldName);
                        if (index == -1)
                        {
                            //outError = new Exception("找不到字段名为" + fieldName + "的字段");
                            //return;
                            continue;
                        }
                        if (pFeaCls.Fields.get_Field(index).IsNullable == true)
                        {
                            //字段属性为空，与标准不一致，将结果保存起来
                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("空值检查");//功能名称
                            ErrorLst.Add("");  //数据文件名
                            ErrorLst.Add(enumErrorType.空值检查.GetHashCode());//错误id;
                            ErrorLst.Add("图层" + item.Key + "的字段" + fieldName + "属性值不允许为空");//错误描述
                            ErrorLst.Add(0);    //...
                            ErrorLst.Add(0);    //...
                            ErrorLst.Add(item.Key);
                            ErrorLst.Add(-1);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
                        }
                        else
                        {
                            IFeatureCursor pFeaCusor = pFeaCls.Search(null, false);
                            if (pFeaCusor == null) return;
                            IFeature pFeature = pFeaCusor.NextFeature();
                            if (pFeature == null) continue;
                            while (pFeature != null)
                            {
                                object fieldValue = pFeature.get_Value(index);
                                if (fieldValue == null || fieldValue.ToString() == "")
                                {
                                    //字段值不能为空，将检查结果保存起来
                                    double pMapx = 0.0;
                                    double pMapy = 0.0;
                                    IPoint pPoint = new PointClass();
                                    if (pFeaCls.ShapeType != esriGeometryType.esriGeometryPoint)
                                    {
                                        pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                                    }
                                    else
                                    {
                                        pPoint = pFeature.Shape as IPoint;
                                    }
                                    pMapx = pPoint.X;
                                    pMapy = pPoint.Y;
                                    List<object> ErrorLst = new List<object>();
                                    ErrorLst.Add("要素属性检查");//功能组名称
                                    ErrorLst.Add("空值检查");//功能名称
                                    ErrorLst.Add("");  //数据文件名
                                    ErrorLst.Add(enumErrorType.空值检查.GetHashCode());//错误id;
                                    ErrorLst.Add("图层" + item.Key + "的字段" + fieldName + "的值不能为空");//错误描述
                                    ErrorLst.Add(pMapx);    //...
                                    ErrorLst.Add(pMapy);    //...
                                    ErrorLst.Add(item.Key);
                                    ErrorLst.Add(pFeature.OID);
                                    ErrorLst.Add("");
                                    ErrorLst.Add(-1);
                                    ErrorLst.Add(false);
                                    ErrorLst.Add(System.DateTime.Now.ToString());

                                    //传递错误日志
                                    IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                                    DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                                    DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
                                }
                                pFeature = pFeaCusor.NextFeature();
                            }

                            //释放cursor
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCusor);
                        }

                        tempValue += 1;//进度条的值加1
                        ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);
                    }
                    //改变树图运行状态
                    ChangeTreeSelectNode(pNode, "完成图层" + item.Key + "的空值检查！", false);
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

       
        /// <summary>
        /// 线长度逻辑性检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="fieldName">分类代码字段名</param>
        /// <param name="feaCodeDic">要素类和分类代码字典对</param>
        /// <param name="minLen">线最小长度</param>
        /// <param name="maxLen">线最大长度</param>
        /// <param name="outError"></param>
        public void LineLengthLogicCheck(IFeatureDataset pFeatureDataset,string fieldName, Dictionary<string, string> feaCodeDic, string  minLen, string maxLen, out Exception outError)
        {
            outError = null;
            try
            {
                //遍历图层
                foreach (KeyValuePair<string, string> item in feaCodeDic)
                {
                    string feaClsName = item.Key;
                    IFeatureClass feaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(feaClsName);
                    if (feaCls.ShapeType != esriGeometryType.esriGeometryPolyline) continue;
                    int index = feaCls.Fields.FindField("SHAPE_Length");
                    if (index == -1)
                    {
                        index = feaCls.Fields.FindField("SHAPE_Leng");//shape文件
                        if(index==-1)
                        {
                            outError = new Exception("字段SHAPE_Leng不存在，请检查！");
                            return;
                        }
                    }

                    string code = item.Value;
                    string filterStr = "";
                    if (code != "")
                    {
                        //分类代码限制条件
                        filterStr = fieldName + " in (" + code + ")";
                    }
                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = filterStr;
                    IFeatureCursor pCursor = feaCls.Search(pFilter, false);
                    if (pCursor == null) return;
                    IFeature pFeature = pCursor.NextFeature();
                    if (pFeature == null) continue;
                    while (pFeature != null)
                    {
                        # region 执行检查
                        double lineLen = Convert.ToDouble(pFeature.get_Value(index).ToString());
                        double dminLen = 0;
                        double dmaxLen = 999999999999;
                        if (minLen != "")
                        {
                            dminLen = Convert.ToDouble(minLen);
                        }
                        if (maxLen != "")
                        {
                            dmaxLen =Convert.ToDouble(maxLen);
                        }
                        if (lineLen < dminLen)
                        {
                            //线长度不在指定的长度范围内，将出错结果显示出来
                            IPoint pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                            double pMapx = pPoint.X;
                            double pMapy = pPoint.Y;

                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("线长度逻辑性检查");//功能名称
                            ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                            ErrorLst.Add(enumErrorType.线长度逻辑性检查.GetHashCode());//错误id;
                            ErrorLst.Add("线长度不在给定的范围内");//错误描述
                            ErrorLst.Add(pMapx);    //...
                            ErrorLst.Add(pMapy);    //...
                            ErrorLst.Add(feaClsName);
                            ErrorLst.Add(pFeature.OID);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);

                            pFeature = pCursor.NextFeature();
                            continue;
                        }
                        else if (lineLen > dmaxLen)
                        {
                            //线长度不在指定的长度范围内，将出错结果显示出来
                            IPoint pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                            double pMapx = pPoint.X;
                            double pMapy = pPoint.Y;

                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("线长度逻辑性检查");//功能名称
                            ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                            ErrorLst.Add(enumErrorType.线长度逻辑性检查.GetHashCode());//错误id;
                            ErrorLst.Add("线长度不在给定的范围内");//错误描述
                            ErrorLst.Add(pMapx);    //...
                            ErrorLst.Add(pMapy);    //...
                            ErrorLst.Add(feaClsName);
                            ErrorLst.Add(pFeature.OID);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
                        }
                        #endregion
                        pFeature = pCursor.NextFeature();
                    }

                    //释放cursor
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 线长度逻辑性检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="feaClsLst"></param>
        /// <param name="fieldName">分类代码字段名</param>
        /// <param name="feaCodeDic">要素类和分类代码字典对</param>
        /// <param name="minLen">线最小长度</param>
        /// <param name="maxLen">线最大长度</param>
        /// <param name="outError"></param>
        public void LineLengthLogicCheck(List<IFeatureClass> feaClsLst, string fieldName, Dictionary<string, string> feaCodeDic, string minLen, string maxLen, out Exception outError)
        {
            outError = null;
            try
            {
                if (hook.DataTree == null) return;
                hook.DataTree.Nodes.Clear();
                //创建处理树图
                IntialTree(hook.DataTree);
                //设置树节点颜色
                setNodeColor(hook.DataTree);
                hook.DataTree.Tag = false;

                //遍历图层
                foreach (KeyValuePair<string, string> item in feaCodeDic)
                {
                    string feaClsName = item.Key;
                    IFeatureClass feaCls = null;// (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(feaClsName);
                    foreach(IFeatureClass mFeaCls in feaClsLst)
                    {
                        string tempFeaClsNm = (mFeaCls as IDataset).Name;
                        if(tempFeaClsNm.Contains("."))
                        {
                            tempFeaClsNm = tempFeaClsNm.Substring(tempFeaClsNm.IndexOf('.') + 1);
                        }
                        if(tempFeaClsNm==feaClsName)
                        {
                            feaCls = mFeaCls;
                            break;
                        }
                    }
                    if (feaCls == null) continue;

                    //创建树图节点(以图层名作为根结点)
                    DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                    pNode = (DevComponents.AdvTree.Node)CreateAdvTreeNode(hook.DataTree.Nodes, item.Key, item.Key, hook.DataTree.ImageList.Images[6], true);//图层名节点
                   

                    if (feaCls.ShapeType != esriGeometryType.esriGeometryPolyline) continue;
                    int index = feaCls.Fields.FindField("SHAPE_Length");
                    if (index == -1)
                    {
                        index = feaCls.Fields.FindField("SHAPE_Leng");
                        if (index == -1)
                        {
                            index = feaCls.Fields.FindField("SHAPE.LEN");
                            if (index == -1)
                            {
                                index = feaCls.Fields.FindField("shape.len");
                                outError = new Exception("字段SHAPE_Leng不存在，请检查！");
                                return;
                            }
                        }
                    }

                    string code = item.Value;
                    string filterStr = "";
                    if (code != "")
                    {
                        //分类代码限制条件
                        filterStr = fieldName + " in (" + code + ")";
                    }
                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = filterStr;
                    IFeatureCursor pCursor = feaCls.Search(pFilter, false);
                    if (pCursor == null) return;
                    IFeature pFeature = pCursor.NextFeature();
                    if (pFeature == null) continue;
                    //显示进度条
                    ShowProgressBar(true);

                    int tempValue = 0;
                    ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, 0, feaCls.FeatureCount(null), tempValue);

                    while (pFeature != null)
                    {
                        # region 执行检查
                        double lineLen = Convert.ToDouble(pFeature.get_Value(index).ToString());
                        double dminLen = 0;
                        double dmaxLen = 999999999999;
                        if (minLen != "")
                        {
                            dminLen = Convert.ToDouble(minLen);
                        }
                        if (maxLen != "")
                        {
                            dmaxLen = Convert.ToDouble(maxLen);
                        }
                        if (lineLen < dminLen)
                        {
                            //线长度不在指定的长度范围内，将出错结果显示出来
                            IPoint pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                            double pMapx = pPoint.X;
                            double pMapy = pPoint.Y;

                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("线长度逻辑性检查");//功能名称
                            ErrorLst.Add("");  //数据文件名
                            ErrorLst.Add(enumErrorType.线长度逻辑性检查.GetHashCode());//错误id;
                            ErrorLst.Add("线长度不在给定的范围内");//错误描述
                            ErrorLst.Add(pMapx);    //...
                            ErrorLst.Add(pMapy);    //...
                            ErrorLst.Add(feaClsName);
                            ErrorLst.Add(pFeature.OID);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);

                            pFeature = pCursor.NextFeature();
                            tempValue += 1;//进度条的值加1
                            ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);
                            continue;
                        }
                        else if (lineLen > dmaxLen)
                        {
                            //线长度不在指定的长度范围内，将出错结果显示出来
                            IPoint pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                            double pMapx = pPoint.X;
                            double pMapy = pPoint.Y;

                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("线长度逻辑性检查");//功能名称
                            ErrorLst.Add("");  //数据文件名
                            ErrorLst.Add(enumErrorType.线长度逻辑性检查.GetHashCode());//错误id;
                            ErrorLst.Add("线长度不在给定的范围内");//错误描述
                            ErrorLst.Add(pMapx);    //...
                            ErrorLst.Add(pMapy);    //...
                            ErrorLst.Add(feaClsName);
                            ErrorLst.Add(pFeature.OID);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
                        }
                        #endregion
                        pFeature = pCursor.NextFeature();

                        tempValue += 1;//进度条的值加1
                        ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);
                    }
                    //释放cursor
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

                    //改变树图运行状态
                    ChangeTreeSelectNode(pNode, "完成图层" + item.Key + "线长度逻辑检查！", false);
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }


        /// <summary>
        /// 面面积逻辑性检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="fieldName">分类代码字段</param>
        /// <param name="feaCodeDic">要素类和分类代码字典对</param>
        /// <param name="minArea">最小面积</param>
        /// <param name="maxArea">最大面积</param>
        /// <param name="outError"></param>
        public void AreaLogicCheck( IFeatureDataset pFeatureDataset, string fieldName, Dictionary<string, string> feaCodeDic, string minArea, string maxArea, out Exception outError)
        {
            outError = null;
            try
            {
                //遍历图层
                foreach (KeyValuePair<string, string> item in feaCodeDic)
                {
                    string feaClsName = item.Key;
                    IFeatureClass feaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(feaClsName);
                    if (feaCls.ShapeType != esriGeometryType.esriGeometryPolygon) continue;
                    int index = feaCls.Fields.FindField("SHAPE_Area");
                    if (index == -1)
                    {
                        index = feaCls.Fields.FindField("SHAPE.AREA");
                        if (index == -1)
                        {
                            outError = new Exception("字段SHAPE.AREA不存在，请检查！");
                            return;
                        }
                    }

                    string code = item.Value;
                    string filterStr = "";
                    if (code != "")
                    {
                        //分类代码限制条件
                        filterStr = fieldName + " in (" + code + ")";
                    }
                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = filterStr;
                    IFeatureCursor pCursor = feaCls.Search(pFilter, false);
                    if (pCursor == null) return;
                    IFeature pFeature = pCursor.NextFeature();
                    if (pFeature == null) continue;
                    while (pFeature != null)
                    {
                        #region 执行检查
                        double Area = Convert.ToDouble(pFeature.get_Value(index).ToString());
                        double dminArea = 0;                   //最小面积默认值
                        double dmaxArea = 999999999999;        //最大面积默认值
                        if (minArea != "")
                        {
                            dminArea = Convert.ToDouble(minArea);
                        }
                        if (maxArea != "")
                        {
                            dmaxArea = Convert.ToDouble(maxArea);
                        }
                        if (Area < dminArea)
                        {
                            //面面积不在指定的面积范围内，将出错结果显示出来
                            IPoint pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                            double pMapx = pPoint.X;
                            double pMapy = pPoint.Y;

                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("面面积逻辑性检查");//功能名称
                            ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                            ErrorLst.Add(enumErrorType.面面积逻辑性检查.GetHashCode());//错误id;
                            ErrorLst.Add("面面积不在给定的范围内");//错误描述
                            ErrorLst.Add(pMapx);    //...
                            ErrorLst.Add(pMapy);    //...
                            ErrorLst.Add(feaClsName);
                            ErrorLst.Add(pFeature.OID);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);

                            pFeature = pCursor.NextFeature();
                            continue;
                        }
                        else if (Area > dmaxArea)
                        {
                            //面面积不在指定的面积范围内，将出错结果显示出来
                            IPoint pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                            double pMapx = pPoint.X;
                            double pMapy = pPoint.Y;

                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("面面积逻辑性检查");//功能名称
                            ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                            ErrorLst.Add(enumErrorType.面面积逻辑性检查.GetHashCode());//错误id;
                            ErrorLst.Add("面面积不在给定的范围内");//错误描述
                            ErrorLst.Add(pMapx);    //...
                            ErrorLst.Add(pMapy);    //...
                            ErrorLst.Add(feaClsName);
                            ErrorLst.Add(pFeature.OID);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
                        }

                        pFeature = pCursor.NextFeature();
                        #endregion
                    }

                    //释放cursor
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }


        /// <summary>
        /// 面面积逻辑性检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="feaClsLst"></param>
        /// <param name="fieldName">分类代码字段</param>
        /// <param name="feaCodeDic">要素类和分类代码字典对</param>
        /// <param name="minArea">最小面积</param>
        /// <param name="maxArea">最大面积</param>
        /// <param name="outError"></param>
        public void AreaLogicCheck(List<IFeatureClass> feaClsLst, string fieldName, Dictionary<string, string> feaCodeDic, string minArea, string maxArea, out Exception outError)
        {
            outError = null;
            try
            {
                if (hook.DataTree == null) return;
                hook.DataTree.Nodes.Clear();
                //创建处理树图
                IntialTree(hook.DataTree);
                //设置树节点颜色
                setNodeColor(hook.DataTree);
                hook.DataTree.Tag = false;


                //遍历图层
                foreach (KeyValuePair<string, string> item in feaCodeDic)
                {
                    string feaClsName = item.Key;
                    IFeatureClass feaCls=null;// = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(feaClsName);
                    foreach (IFeatureClass mFeaCls in feaClsLst)
                    {
                        string tempName = (mFeaCls as IDataset).Name;
                        if(tempName.Contains("."))
                        {
                            tempName = tempName.Substring(tempName.IndexOf('.') + 1);
                        }
                        if (tempName == feaClsName)
                        {
                            feaCls = mFeaCls;
                            break;
                        }
                    }
                    if (feaCls == null) continue;

                    if (feaCls.ShapeType != esriGeometryType.esriGeometryPolygon) continue;
                    int index = feaCls.Fields.FindField("SHAPE_Area");
                    if (index == -1)
                    {
                        index = feaCls.Fields.FindField("SHAPE.AREA");
                        if (index == -1)
                        {
                            outError = new Exception("字段SHAPE.AREA不存在，请检查！");
                            return;
                        }
                    }

                    string code = item.Value;
                    string filterStr = "";
                    if (code != "")
                    {
                        //分类代码限制条件
                        filterStr = fieldName + " in (" + code + ")";
                    }
                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = filterStr;
                    IFeatureCursor pCursor = feaCls.Search(pFilter, false);
                    if (pCursor == null) return;
                    IFeature pFeature = pCursor.NextFeature();
                    if (pFeature == null) continue;

                    //显示进度条
                    ShowProgressBar(true);

                    int tempValue = 0;
                    ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, 0, feaCls.FeatureCount(null), tempValue);


                    while (pFeature != null)
                    {
                        #region 执行检查
                        double Area = Convert.ToDouble(pFeature.get_Value(index).ToString());
                        double dminArea = 0;                   //最小面积默认值
                        double dmaxArea = 999999999999;        //最大面积默认值
                        if (minArea != "")
                        {
                            dminArea = Convert.ToDouble(minArea);
                        }
                        if (maxArea != "")
                        {
                            dmaxArea = Convert.ToDouble(maxArea);
                        }
                        if (Area < dminArea)
                        {
                            //面面积不在指定的面积范围内，将出错结果显示出来
                            IPoint pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                            double pMapx = pPoint.X;
                            double pMapy = pPoint.Y;

                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("面面积逻辑性检查");//功能名称
                            ErrorLst.Add("");  //数据文件名
                            ErrorLst.Add(enumErrorType.面面积逻辑性检查.GetHashCode());//错误id;
                            ErrorLst.Add("面面积不在给定的范围内");//错误描述
                            ErrorLst.Add(pMapx);    //...
                            ErrorLst.Add(pMapy);    //...
                            ErrorLst.Add(feaClsName);
                            ErrorLst.Add(pFeature.OID);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);

                            pFeature = pCursor.NextFeature();

                            tempValue += 1;//进度条的值加1
                            ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);
                            continue;
                        }
                        else if (Area > dmaxArea)
                        {
                            //面面积不在指定的面积范围内，将出错结果显示出来
                            IPoint pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                            double pMapx = pPoint.X;
                            double pMapy = pPoint.Y;

                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("面面积逻辑性检查");//功能名称
                            ErrorLst.Add("");  //数据文件名
                            ErrorLst.Add(enumErrorType.面面积逻辑性检查.GetHashCode());//错误id;
                            ErrorLst.Add("面面积不在给定的范围内");//错误描述
                            ErrorLst.Add(pMapx);    //...
                            ErrorLst.Add(pMapy);    //...
                            ErrorLst.Add(feaClsName);
                            ErrorLst.Add(pFeature.OID);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
                        }

                        pFeature = pCursor.NextFeature();
                        #endregion

                        tempValue += 1;//进度条的值加1
                        ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);
                    }

                    //释放cursor
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }



        /// <summary>
        /// 等高线高程值检查，还需完善
        /// </summary>
        /// <param name="pFeatureDataset">数据集</param>
        /// <param name="feaClsName">图层名</param>
        /// <param name="contourFiledName">高程字段名</param>
        /// <param name="elevMin">高程最小值</param>
        /// <param name="elevMax">高程最大值</param>
        /// <param name="intevalValue">高程间距值</param>
        /// <param name="outError"></param>
        public void contourIntevalCheck(IFeatureDataset pFeatureDataset, string feaClsName, string contourFiledName,string elevMin,string elevMax, double intevalValue, out Exception outError)
        {
            outError = null;
            try
            {
                IFeatureClass pFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(feaClsName);
                if (pFeaCls.ShapeType != esriGeometryType.esriGeometryPolyline)
                {
                    outError = new Exception("该图层不是线图层，请检查。图层名为:" + feaClsName);
                    return;
                }

                //高程字段的索引值
                int index = pFeaCls.Fields.FindField(contourFiledName);
                if (index == -1)
                {
                    outError = new Exception("高程字段名" + contourFiledName + "不存在！");
                    return;
                }
                //检查要素的高程值是否在制定的高程范围内
                ElevationValueCheck(pFeaCls, index, pFeatureDataset, feaClsName, elevMin, elevMax, out outError);
                if (outError != null) return;

                //检查等高线的高程间距是否一致
                //条件查询和排序
                //IQueryFilter pFilter = new QueryFilterClass();
                //pFilter.WhereClause = "";
                //IQueryFilterDefinition pFilterDef = pFilter as IQueryFilterDefinition;
                //pFilterDef.PostfixClause = "ORDER BY " + contourFiledName;
                //IFeatureCursor pCursor = pFeaCls.Search(pFilter, true);
                ITableSort pTableSort = new TableSortClass();
                pTableSort.Fields = contourFiledName;
                pTableSort.set_Ascending(contourFiledName, true);
                pTableSort.set_CaseSensitive(contourFiledName, true);
                pTableSort.QueryFilter = null;
                pTableSort.Table = pFeaCls as ITable;

                //升序排列
                pTableSort.Sort(null);
                ICursor pCursor = pTableSort.Rows;
                if (pCursor == null) return;
                IFeatureCursor pFeaCursor = pCursor as IFeatureCursor;
                IFeature pFeature = pFeaCursor.NextFeature();
                double contValue = -1;
                IFeature oriFeature = null;
                while (pFeature != null)
                {
                    if (contValue == -1)
                    {
                        contValue = Convert.ToDouble(pFeature.get_Value(index).ToString());
                        oriFeature = pFeature;
                    }
                    else
                    {
                        double tempValue = contValue;
                        IFeature tempFeature = oriFeature;
                        contValue = Convert.ToDouble(pFeature.get_Value(index).ToString());
                        oriFeature = pFeature;
                        if (contValue - tempValue != intevalValue)
                        {
                            double errInterval = contValue - tempValue;
                            //等高线高程值间距不等，将错误结果保存出来
                            IPoint pPoint = TopologyCheckClass.GetPointofFeature(tempFeature);
                            double pMapx = pPoint.X;
                            double pMapy = pPoint.Y;

                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("等高线高程间距检查");//功能名称
                            ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                            ErrorLst.Add(enumErrorType.等高线高程值检查.GetHashCode());//错误id;
                            ErrorLst.Add("图层" + feaClsName + "高程间距值矛盾,间距值为:"+errInterval.ToString());//错误描述
                            ErrorLst.Add(pMapx);    //...
                            ErrorLst.Add(pMapy);    //...
                            ErrorLst.Add(feaClsName);
                            ErrorLst.Add(tempFeature.OID);
                            ErrorLst.Add(feaClsName);
                            ErrorLst.Add(pFeature.OID);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
                        }
                    }
                    pFeature = pFeaCursor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 拉线查等高线
        /// </summary>
        /// <param name="pFeatureCls">等高线要素类</param>
        /// <param name="contourFieldName">高程字段名</param>
        /// <param name="pPolyLine">画线</param>
        /// <param name="intervalValue">高程间距</param>
        /// <param name="orient">等高线方向：递增、递减</param>
        /// <param name="eError"></param>
        public void LineIntervalCheck(IFeatureClass pFeatureCls, string contourFieldName, IPolyline pPolyLine, double intervalValue, string orient, out Exception eError)
        {
            eError = null;
            try
            {
                //获得线pPolyLine的起点和终点
                IPointCollection pointCol = pPolyLine as IPointCollection;
                IPoint firstPoint = pointCol.get_Point(0);     //起点
                IPoint lastPoint = pointCol.get_Point(pointCol.PointCount - 1);      //终点
                double firstX = firstPoint.X;                    //起点X坐标
                double firstY = firstPoint.Y;                    //起点Y坐标
                double lastX = lastPoint.X;                      //终点X坐标
                double lastY = lastPoint.Y;                      //终点Y坐标

                #region 遍历要素类，将与线pPolyLine相交的点和要素保存起来
                Dictionary<double, IFeature> feaDic = new Dictionary<double, IFeature>();
                List<double> pointXLst = new List<double>();         //保存相交点的X坐标
                IFeatureCursor pCusor = pFeatureCls.Search(null, false);
                if (pCusor == null) return;
                IFeature pFeature = pCusor.NextFeature();
                while (pFeature != null)
                {
                    IRelationalOperator pRelOper = pPolyLine as IRelationalOperator;
                    if (!pRelOper.Disjoint(pFeature.Shape))
                    {
                        //相交
                        ITopologicalOperator pTopoOper = pFeature.Shape as ITopologicalOperator;
                        IGeometry pGeo = pTopoOper.Intersect(pPolyLine as IGeometry, esriGeometryDimension.esriGeometry0Dimension);
                        IPoint pTemp = null;      //划线与等高线图层要素的交点
                        if(pGeo.GeometryType==esriGeometryType.esriGeometryMultipoint)
                        {
                            //多点，取其中一个点
                            IPointCollection mPpointCol = pGeo as IPointCollection;
                            if (mPpointCol != null)
                            {
                                pTemp = mPpointCol.get_Point(0);
                            }
                        }
                        else if (pGeo.GeometryType == esriGeometryType.esriGeometryPoint)
                        {
                            //单点
                            pTemp = pGeo as IPoint;
                        }
                        if (pTemp == null) return;
                        double tempX = pTemp.X;
                        if (!feaDic.ContainsKey(tempX))
                        {
                            feaDic.Add(tempX, pFeature);
                        }
                        if (!pointXLst.Contains(tempX))
                        {
                            pointXLst.Add(tempX);
                        }
                    }
                    pFeature = pCusor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCusor);

                #endregion
                if (feaDic.Count == 0)
                {
                    eError = new Exception("画线与等高线并不相交！");
                    return;
                }

                //获得要素类的高程字段索引
                int index = pFeatureCls.Fields.FindField(contourFieldName);
                if (index == -1)
                {
                    eError = new Exception("找不到高程字段，字段名为：" + contourFieldName);
                    return;
                }

                #region 按照起点到终点的方向，来存储相交点
                //根据起点到终点的方向将各个要素的点坐标进行排序,并保存起来
                if (firstX < lastX)
                {
                    //点从小到大
                    //对相交点的X坐标进行从小到大的排序
                    SortList(pointXLst);
                }
                else if (firstX > lastX)
                {
                    //点从大到小
                    //对相交点的X坐标进行从大到小的排序
                    SortList2(pointXLst);
                }
                else
                {
                    if (firstY < lastY)
                    {
                        //点从小到大
                        //对相交点的Y坐标进行从小到大的排序
                        SortList(pointXLst);
                    }
                    else if (firstY > lastY)
                    {
                        //点从大到小
                        //对相交点的Y坐标进行从大到小的排序
                        SortList2(pointXLst);
                    }
                    else
                    {
                        eError = new Exception("请划直线检查是否存在等高线高程值异常！");
                        return;
                    }
                }

                #endregion

                //对高程值进行检查
                //double tempValue = 0.0;
                double firstValue=0.0;
                //IFeature tempFea = null;
                for (int i = 0; i < pointXLst.Count; i++)
                {
                    double pointX = pointXLst[i];
                    IFeature mFea = feaDic[pointX];
                    //要素的高程值 
                    string pValueStr = mFea.get_Value(index).ToString().Trim();
                    if (pValueStr == "")
                    {
                        eError = new Exception("高程值为空！");
                        return;
                    }
                    double pValue = Convert.ToDouble(pValueStr);
                    if (i == 0)
                    {
                        //第一个点的高程
                        firstValue = pValue;
                        continue;
                    }
                    //if (tempValue != 0.0 || tempFea != null)
                    //{
                    if (orient == "递增")
                    {
                        if (pValue != firstValue + intervalValue * i)
                        {
                            //错误,tempFea,mFea
                            GetErrorList(pFeatureCls, mFea, null, null, out eError);
                            if (eError != null)
                            {
                                return;
                            }
                        }
                    }
                    else if (orient == "递减")
                    {
                        if (pValue != firstValue - intervalValue * i)
                        {
                            //错误,tempFea,mFea
                            GetErrorList(pFeatureCls, mFea, null, null, out eError);
                            if (eError != null)
                            {
                                return;
                            }
                        }
                    }
                    //}
                    //tempValue = pValue;
                    //tempFea = mFea;
                }
            }
            catch (System.Exception ex)
            {
                eError = ex;
            }
        }

        //对列表进行从小到大排序
        private void SortList(List<double> pointLst)
        {
            for(int i=0;i<pointLst.Count-1 ;i++)
            {
                for(int j=i+1;j<pointLst.Count;j++)
                {
                    if(pointLst[i]>pointLst[j])
                    {
                        //ChangeValue(pointLst[i], pointLst[j]);
                        double tempV = pointLst[i];
                        pointLst[i] = pointLst[j];
                        pointLst[j] = tempV;
                    }
                }
            }
        }
        //对列表进行从大到小排序
        private void SortList2(List<double> pointLst)
        {
            for (int i = 0; i < pointLst.Count - 1; i++)
            {
                for (int j =i+ 1; j < pointLst.Count; j++)
                {
                    if (pointLst[i] < pointLst[j])
                    {
                        //ChangeValue(pointLst[i],pointLst[j]);
                        double tempV = pointLst[i];
                        pointLst[i] = pointLst[j];
                        pointLst[j]=tempV;
                    }
                }
            }
        }
        //交换数据
       private void ChangeValue(double v1,double v2)
       {
           double temp = 0.0;
           temp = v1;
           v1=v2;
           v2=temp;
       }
        //等高线高程值检查错误列表
        private void GetErrorList(IFeatureClass pOriFeatureCls, IFeature pOriFeature, IFeatureClass pDesFeatureCls, IFeature pDesFeature, out Exception eError)
        {
            eError = null;
            try
            {
                int eErrorID = enumErrorType.等高线高程值检查.GetHashCode();
                string eErrorDes = "等高线高程值不正确";
                double pMapx = 0.0;
                double pMapy = 0.0;
                IPoint pPoint = null;
                //高程值不在给定的高程范围内,将错误结果保存下来
                pPoint = TopologyCheckClass.GetPointofFeature(pOriFeature);

                if(pPoint!=null)
                {
                    pMapx = pPoint.X;
                    pMapy = pPoint.Y;
                }

                List<object> ErrorLst = new List<object>();
                ErrorLst.Add("要素属性检查");//功能组名称
                ErrorLst.Add("等高线高程值检查");//功能名称
                ErrorLst.Add("");  //数据文件名
                ErrorLst.Add(eErrorID);//错误id;
                ErrorLst.Add(eErrorDes);//错误描述
                ErrorLst.Add(pMapx);    //...
                ErrorLst.Add(pMapy);    //...
                ErrorLst.Add((pOriFeatureCls as IDataset).Name);
                ErrorLst.Add(pOriFeature.OID);
                if (pDesFeatureCls != null)
                {
                    ErrorLst.Add((pDesFeatureCls as IDataset).Name);
                }
                else
                {
                    ErrorLst.Add("");
                }
                if (pDesFeature != null)
                {
                    ErrorLst.Add(pDesFeature.OID);
                }
                else
                {
                    ErrorLst.Add(-1);
                }
               
                ErrorLst.Add(false);
                ErrorLst.Add(System.DateTime.Now.ToString());

                //传递错误日志
                IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
            }
            catch (Exception ex)
            {
                eError = ex;
            }
        }

        /// <summary>
        /// 高程值检查
        /// </summary>
        /// <param name="pFeatureDataset"></param>
        /// <param name="feaClsName"></param>
        /// <param name="elevMin">最小高程</param>
        /// <param name="elevMax">最大高程</param>
        /// <param name="outError"></param>
        public void CoutourValueCheck(IFeatureDataset pFeatureDataset, string feaClsName,string contourFiledName, string elevMin, string elevMax, out Exception outError)
        {
            outError = null;
            try
            {
                IFeatureClass pFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(feaClsName);
                //if (pFeaCls.ShapeType != esriGeometryType.esriGeometryPolyline)
                //{
                //    outError = new Exception("该图层不是线图层，请检查。图层名为:" + feaClsName);
                //    return;
                //}

                //高程字段的索引值
                int index = pFeaCls.Fields.FindField(contourFiledName);
                if (index == -1)
                {
                    outError = new Exception("高程字段名" + contourFiledName + "不存在！");
                    return;
                }
                //检查要素的高程值是否在指定的高程范围内
                ElevationValueCheck(pFeaCls, index, pFeatureDataset, feaClsName, elevMin, elevMax, out outError);
                if (outError != null) return;
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 高程值检查
        /// </summary>
        /// <param name="pFeatureDataset"></param>
        /// <param name="feaClsName"></param>
        /// <param name="elevMin">最小高程</param>
        /// <param name="elevMax">最大高程</param>
        /// <param name="outError"></param>
        public void CoutourValueCheck(List<IFeatureClass> feaClsLst, string feaClsName, string contourFiledName, string elevMin, string elevMax, out Exception outError)
        {
            outError = null;
            try
            {
                IFeatureClass pFeaCls = null;// = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(feaClsName);
                foreach (IFeatureClass mFeaCls in feaClsLst)
                {
                    string tempName = (mFeaCls as IDataset).Name;
                    if(tempName.Contains("."))
                    {
                        tempName = tempName.Substring(tempName.IndexOf('.') + 1);
                    }
                    if (tempName == feaClsName)
                    {
                        pFeaCls = mFeaCls;
                        break;
                    }
                }
                if (pFeaCls == null) return;
                //if (pFeaCls.ShapeType != esriGeometryType.esriGeometryPolyline)
                //{
                //    outError = new Exception("该图层不是线图层，请检查。图层名为:" + feaClsName);
                //    return;
                //}

                //高程字段的索引值
                int index = pFeaCls.Fields.FindField(contourFiledName);
                if (index == -1)
                {
                    outError = new Exception("高程字段名" + contourFiledName + "不存在！");
                    return;
                }
                //检查要素的高程值是否在指定的高程范围内
                ElevationValueCheck(pFeaCls, index, null, feaClsName, elevMin, elevMax, out outError);
                if (outError != null) return;
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 异常高程值检查
        /// </summary>
        /// <param name="pFeaCls">图层</param>
        /// <param name="index">高程字段索引</param>
        /// <param name="pFeatureDataset">数据集</param>
        /// <param name="feaClsName">图层名</param>
        /// <param name="elevMin">最小高程值</param>
        /// <param name="elevMax">最大高程值</param>
        /// <param name="outError"></param>
        private void ElevationValueCheck(IFeatureClass pFeaCls, int index, IFeatureDataset pFeatureDataset, string feaClsName, string elevMin, string elevMax, out Exception outError)
        {
            outError = null;
            try
            {
                IFeatureCursor pCursor = pFeaCls.Search(null, false);
                if (pCursor == null) return;
                IFeature pFeature = pCursor.NextFeature();
                if (pFeature == null) return;

                //显示进度条
                ShowProgressBar(true);

                int tempValue = 0;
                ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, 0, pFeaCls.FeatureCount(null), tempValue);

                while (pFeature != null)
                {
                    double fieldValue = Convert.ToDouble(pFeature.get_Value(index).ToString());
                    double dminElev = 0;                   //最小高程值
                    double dmaxElev = 999999999999;        //最大高程值
                    if (elevMin != "")
                    {
                        dminElev = Convert.ToDouble(elevMin);
                    }
                    if (elevMax != "")
                    {
                        dmaxElev = Convert.ToDouble(elevMax);
                    }
                    //首先检查所有高程值是否在制定的范围内
                    if (fieldValue < dminElev)
                    {
                        ElevCheckErrShow(pFeatureDataset, feaClsName, pFeature, out outError);
                        if (outError != null)
                        {
                            pFeature = pCursor.NextFeature();
                            continue;
                        }
                        pFeature = pCursor.NextFeature();
                        continue;
                    }
                    else if (fieldValue > dmaxElev)
                    {
                        ElevCheckErrShow(pFeatureDataset, feaClsName, pFeature, out outError);
                        if (outError != null)
                        {
                            pFeature = pCursor.NextFeature();
                            continue;
                        }
                    }
                    pFeature = pCursor.NextFeature();

                    tempValue += 1;//进度条的值加1
                    ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 等高线高程检查错误结果信息
        /// </summary>
        /// <param name="pFeatureDataset"></param>
        /// <param name="feaClsname"></param>
        /// <param name="pFeature"></param>
        /// <param name="eError"></param>
        private void ElevCheckErrShow(IFeatureDataset pFeatureDataset, string feaClsname, IFeature pFeature,out Exception eError)
        {
            eError = null;
            try
            {
                //高程值不在给定的高程范围内,将错误结果保存下来
                IPoint pPoint = null;
                double pMapx = 0.0;
                double pMapy = 0.0;
                if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                {
                    pPoint = pFeature.Shape as IPoint;
                }
                else
                {
                    pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                }
                if (pPoint != null)
                {
                    pMapx = pPoint.X;
                    pMapy = pPoint.Y;
                }

                List<object> ErrorLst = new List<object>();
                ErrorLst.Add("要素属性检查");//功能组名称
                ErrorLst.Add("异常高程值检查");//功能名称
                if (pFeatureDataset == null)
                {
                    ErrorLst.Add("");  //数据文件名
                }
                else
                {
                    ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                }

                ErrorLst.Add(enumErrorType.高程值检查.GetHashCode());//错误id;
                ErrorLst.Add("图层" + feaClsname + "高程值不在给定的范围内");//错误描述
                ErrorLst.Add(pMapx);    //...
                ErrorLst.Add(pMapy);    //...
                ErrorLst.Add(feaClsname);
                ErrorLst.Add(pFeature.OID);
                ErrorLst.Add("");
                ErrorLst.Add(-1);
                ErrorLst.Add(false);
                ErrorLst.Add(System.DateTime.Now.ToString());

                //传递错误日志
                IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
            }
            catch (Exception ex)
            {
                eError = ex;
            }
        }


        /// <summary>
        /// 注记高程值一致性检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="codeName">分类代码字段名</param>
        /// <param name="oriFeaClsName">源要素类名称</param>
        /// <param name="oriCodeValue">源要素类分类代码值</param>
        /// <param name="oriElevFieldName">源高程值字段名</param>
        /// <param name="desFeaClsName">目标要素类名称</param>
        /// <param name="desCodeValue">目标要素类分类代码值</param>
        /// <param name="labelFieldName">目标要素类高程值</param>
        /// <param name="radius">搜索半径</param>
        /// <param name="precision">精度控制</param>
        /// <param name="outError"></param>
        public void ElevAccordanceCheck(IFeatureDataset pFeatureDataset, string codeName, string oriFeaClsName, string oriCodeValue, string oriElevFieldName, string desFeaClsName, string desCodeValue, string labelFieldName, double radius, long precision, out Exception outError)
        {
            outError = null;
            try
            {
                //源要素类
                IFeatureClass pOriFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(oriFeaClsName);
                //源要素类高程字段索引值
                int oriEleIndex = pOriFeaCls.Fields.FindField(oriElevFieldName);
                if (oriEleIndex == -1)
                {
                    outError = new Exception("要素类" + oriFeaClsName + "字段" + oriElevFieldName + "不存在！");
                    return;
                }

                //目标要素类
                IFeatureClass pDesFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(desFeaClsName);
                //目标要素类高程字段索引值
                int desElevIndex = pDesFeaCls.Fields.FindField(labelFieldName);
                if (desElevIndex == -1)
                {
                    outError = new Exception("要素类" + desFeaClsName + "字段" + labelFieldName + "不存在！");
                    return;
                }

                //查找源要素类中符合分类代码限制条件的要素
                string whereStr = "";
                if (oriCodeValue != "")
                {
                    whereStr = codeName + " ='" + oriCodeValue + "'";
                }
                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = whereStr;
                IFeatureCursor pCursor = pOriFeaCls.Search(pFilter, false);
                if (pCursor == null) return;
                IFeature pOriFea = pCursor.NextFeature();

                //遍历源要素，进行比较
                while (pOriFea != null)
                {
                    #region 进行检查
                    string oriElevValue = pOriFea.get_Value(oriEleIndex).ToString();
                    //根据原高程值求出精度允许的高程值用于比较
                    if (oriElevValue.Contains("."))
                    {
                        //原高程值包含小数点
                        int oriDotIndex = oriElevValue.IndexOf('.');
                        if (precision == 0)
                        {
                            oriElevValue = oriElevValue.Substring(0, oriDotIndex);
                        }
                        else if (oriElevValue.Substring(oriDotIndex + 1).Length > precision && precision > 0)
                        {
                            //原高程值的小数点位数大于精度控制
                            int oriLen = oriDotIndex + 1 +Convert.ToInt32(precision);
                            oriElevValue = oriElevValue.Substring(0, oriLen);
                        }
                    }

                    IFeature desFeature = GetNearestFeature(pDesFeaCls, codeName, desCodeValue, pOriFea, radius, out outError);
                    if (outError != null) return;
                    if (desFeature == null)
                    {
                        pOriFea = pCursor.NextFeature();
                        continue;
                    }
                    string desElevValue = desFeature.get_Value(desElevIndex).ToString();
                    if (desElevValue.Contains("."))
                    {
                        //目标高程值包含小数点
                        int desDotIndex = desElevValue.IndexOf('.');
                        if (precision == 0)
                        {
                            desElevValue = desElevValue.Substring(0, desDotIndex);
                        }
                        else if (desElevValue.Substring(desDotIndex + 1).Length > precision && precision > 0)
                        {
                            //目标高程值的小数点位数大于精度
                            int desLen = desDotIndex + 1 +Convert.ToInt32(precision);
                            desElevValue = desElevValue.Substring(0, desLen);
                        }
                    }

                    //根据精度进行比较，在容许的范围内不相同不算错误
                    if (Convert.ToDouble(oriElevValue) !=Convert.ToDouble(desElevValue))
                    {
                        //说明点，或线与相应注记的高程值不一致。将错误结果显示出来
                        double pMapx = 0.0;
                        double pMapy = 0.0;
                        IPoint pPoint = new PointClass();
                        if (pOriFeaCls.ShapeType != esriGeometryType.esriGeometryPoint)
                        {
                            pPoint = TopologyCheckClass.GetPointofFeature(pOriFea);
                        }
                        else  
                        {
                            //点要素类
                            pPoint = pOriFea.Shape as IPoint;
                        }
                        pMapx = pPoint.X;
                        pMapy = pPoint.Y;

                        List<object> ErrorLst = new List<object>();
                        ErrorLst.Add("要素属性检查");//功能组名称
                        ErrorLst.Add("点线注记高程值一致性检查");//功能名称
                        ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                        ErrorLst.Add(0);//错误id;
                        ErrorLst.Add("图层" + oriFeaClsName + "与图层" + desFeaClsName + "高程值不一致");//错误描述
                        ErrorLst.Add(pMapx);    //...
                        ErrorLst.Add(pMapy);    //...
                        ErrorLst.Add(oriFeaClsName);
                        ErrorLst.Add(pOriFea.OID);
                        ErrorLst.Add(desFeaClsName);
                        ErrorLst.Add(desFeature.OID);
                        ErrorLst.Add(false);
                        ErrorLst.Add(System.DateTime.Now.ToString());

                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);

                    }
                    pOriFea = pCursor.NextFeature();
                    #endregion
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }

        /// <summary>
        /// 注记高程值一致性检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="feaClsLst"></param>
        /// <param name="codeName">分类代码字段名</param>
        /// <param name="oriFeaClsName">源要素类名称</param>
        /// <param name="oriCodeValue">源要素类分类代码值</param>
        /// <param name="oriElevFieldName">源高程值字段名</param>
        /// <param name="desFeaClsName">目标要素类名称</param>
        /// <param name="desCodeValue">目标要素类分类代码值</param>
        /// <param name="labelFieldName">目标要素类高程值</param>
        /// <param name="radius">搜索半径</param>
        /// <param name="precision">精度控制</param>
        /// <param name="outError"></param>
        public void ElevAccordanceCheck(List<IFeatureClass> feaClsLst, string codeName, string oriFeaClsName, string oriCodeValue, string oriElevFieldName, string desFeaClsName, string desCodeValue, string labelFieldName, double radius, long precision, out Exception outError)
        {
            outError = null;
            try
            {
                //源要素类
                IFeatureClass pOriFeaCls = null;// (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(oriFeaClsName);
                foreach (IFeatureClass mFeaCls in feaClsLst)
                {
                    string tempNm = (mFeaCls as IDataset).Name;
                    if(tempNm.Contains("."))
                    {
                        tempNm = tempNm.Substring(tempNm.IndexOf('.') + 1);
                    }
                    if (tempNm == oriFeaClsName)
                    {
                        
                        pOriFeaCls = mFeaCls;
                        break;
                    }
                }
                if (pOriFeaCls == null) return;
                //源要素类高程字段索引值
                int oriEleIndex = pOriFeaCls.Fields.FindField(oriElevFieldName);
                if (oriEleIndex == -1)
                {
                    outError = new Exception("要素类" + oriFeaClsName + "字段" + oriElevFieldName + "不存在！");
                    return;
                }

                //目标要素类
                IFeatureClass pDesFeaCls = null;// (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(desFeaClsName);
                foreach (IFeatureClass mFeaCls in feaClsLst)
                {
                    string tempNm = (mFeaCls as IDataset).Name;
                    if (tempNm.Contains("."))
                    {
                        tempNm = tempNm.Substring(tempNm.IndexOf('.') + 1);
                    }
                    if (tempNm== desFeaClsName)
                    {
                        pDesFeaCls = mFeaCls;
                        break;
                    }
                }
                if (pDesFeaCls == null) return;
                //目标要素类高程字段索引值
                int desElevIndex = pDesFeaCls.Fields.FindField(labelFieldName);
                if (desElevIndex == -1)
                {
                    outError = new Exception("要素类" + desFeaClsName + "字段" + labelFieldName + "不存在！");
                    return;
                }

                //查找源要素类中符合分类代码限制条件的要素
                string whereStr = "";
                if (oriCodeValue != "")
                {
                    whereStr = codeName + " ='" + oriCodeValue + "'";
                }
                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = whereStr;
                IFeatureCursor pCursor = pOriFeaCls.Search(pFilter, false);
                if (pCursor == null) return;
                IFeature pOriFea = pCursor.NextFeature();

                //显示进度条
                ShowProgressBar(true);

                int tempValue = 0;
                ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, 0, pOriFeaCls.FeatureCount(null), tempValue);

                //遍历源要素，进行比较
                while (pOriFea != null)
                {
                    #region 进行检查
                    string oriElevValue = pOriFea.get_Value(oriEleIndex).ToString();
                    //根据原高程值求出精度允许的高程值用于比较
                    if (oriElevValue.Contains("."))
                    {
                        //原高程值包含小数点
                        int oriDotIndex = oriElevValue.IndexOf('.');
                        if (precision == 0)
                        {
                            oriElevValue = oriElevValue.Substring(0, oriDotIndex);
                        }
                        else if (oriElevValue.Substring(oriDotIndex + 1).Length > precision && precision > 0)
                        {
                            //原高程值的小数点位数大于精度控制
                            int oriLen = oriDotIndex + 1 + Convert.ToInt32(precision);
                            oriElevValue = oriElevValue.Substring(0, oriLen);
                        }
                    }

                    IFeature desFeature = GetNearestFeature(pDesFeaCls, codeName, desCodeValue, pOriFea, radius, out outError);
                    if (outError != null) return;
                    if (desFeature == null)
                    {
                        pOriFea = pCursor.NextFeature();
                        continue;
                    }
                    string desElevValue = desFeature.get_Value(desElevIndex).ToString();
                    if (desElevValue.Contains("."))
                    {
                        //目标高程值包含小数点
                        int desDotIndex = desElevValue.IndexOf('.');
                        if (precision == 0)
                        {
                            desElevValue = desElevValue.Substring(0, desDotIndex);
                        }
                        else if (desElevValue.Substring(desDotIndex + 1).Length > precision && precision > 0)
                        {
                            //目标高程值的小数点位数大于精度
                            int desLen = desDotIndex + 1 + Convert.ToInt32(precision);
                            desElevValue = desElevValue.Substring(0, desLen);
                        }
                    }

                    //根据精度进行比较，在容许的范围内不相同不算错误
                    if (Convert.ToDouble(oriElevValue) != Convert.ToDouble(desElevValue))
                    {
                        //说明点，或线与相应注记的高程值不一致。将错误结果显示出来
                        double pMapx = 0.0;
                        double pMapy = 0.0;
                        IPoint pPoint = new PointClass();
                        if (pOriFeaCls.ShapeType != esriGeometryType.esriGeometryPoint)
                        {
                            pPoint = TopologyCheckClass.GetPointofFeature(pOriFea);
                        }
                        else
                        {
                            //点要素类
                            pPoint = pOriFea.Shape as IPoint;
                        }
                        pMapx = pPoint.X;
                        pMapy = pPoint.Y;

                        List<object> ErrorLst = new List<object>();
                        ErrorLst.Add("要素属性检查");//功能组名称
                        ErrorLst.Add("点线注记高程值一致性检查");//功能名称
                        ErrorLst.Add("");  //数据文件名
                        ErrorLst.Add(0);//错误id;
                        ErrorLst.Add("图层" + oriFeaClsName + "与图层" + desFeaClsName + "高程值不一致");//错误描述
                        ErrorLst.Add(pMapx);    //...
                        ErrorLst.Add(pMapy);    //...
                        ErrorLst.Add(oriFeaClsName);
                        ErrorLst.Add(pOriFea.OID);
                        ErrorLst.Add(desFeaClsName);
                        ErrorLst.Add(desFeature.OID);
                        ErrorLst.Add(false);
                        ErrorLst.Add(System.DateTime.Now.ToString());

                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);

                    }
                    pOriFea = pCursor.NextFeature();
                    #endregion


                    tempValue += 1;//进度条的值加1
                    ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }


        /// <summary>
        /// 等高线点线矛盾一致性检查
        /// </summary>
        /// <param name="pFeatureDataset"></param>
        /// <param name="lineFeaClsName">等高线要素类名称</param>
        /// <param name="lineFieldName">等高线高程字段名</param>
        /// <param name="pointFeaClsName">高程点要素类名称</param>
        /// <param name="pointFieldName">高程点字段名</param>
        /// <param name="radiu">高程点搜索半径</param>
        /// <param name="intervalValue">等高线间距值</param>
        /// <param name="eError"></param>
        public void PointLineElevCheck(IFeatureDataset pFeatureDataset, string lineFeaClsName, string lineFieldName, string pointFeaClsName, string pointFieldName,double intervalValue, out Exception eError)
        {
            eError = null;
            try
            {
                //等高线要素类
                IFeatureClass lineFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(lineFeaClsName);
                //等高线高程字段索引值
                int lineIndex = lineFeaCls.Fields.FindField(lineFieldName);
                if (lineIndex == -1)
                {
                    eError = new Exception("等高线图层的高程字段不存在,字段名为：" + lineFieldName);
                    return;
                }
                //高程点要素类
                IFeatureClass pointFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(pointFeaClsName );
                int pointIndex = pointFeaCls.Fields.FindField(pointFieldName);
                if (lineIndex == -1)
                {
                    eError = new Exception("高程点图层的高程字段不存在,字段名为：" + pointFieldName);
                    return;
                }

                //遍历高程点要素
                IFeatureCursor pCusor = pointFeaCls.Search(null, false);
                if (pCusor == null) return;
                IFeature pointFeature = pCusor.NextFeature();
                while (pointFeature != null)
                {
                    //高程点要素的高程值
                    double pointElevValue =Convert.ToDouble(pointFeature.get_Value(pointIndex).ToString());
                    //查找高程点相邻的两条高程线要素

                    //与高程点最近的等高线要素以及最短距离
                    Dictionary<double, IFeature> nearestFeaDic = GetShortestDis(lineFeaCls, pointFeature, out eError);
                    if (eError != null || nearestFeaDic == null)
                    {
                        eError = new Exception("在搜索范围内的未找到要素!");
                        return;
                    }
                    double pShortestDis = -1;
                    IFeature nearestFea = null;
                    foreach (KeyValuePair<double, IFeature> item in nearestFeaDic)
                    {
                        pShortestDis = item.Key;
                        nearestFea = item.Value;
                        break;
                    }
                    if(eError!=null||pShortestDis==-1) return;
                    //获得等高线上离高程点最近的点
                    IPoint nearestPoint = new PointClass();//等高线上的最近点
                    IProximityOperator mProxiOpe = nearestFea.Shape as IProximityOperator;
                    if (mProxiOpe == null) return;
                    nearestPoint = mProxiOpe.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                    //将高程点和等高线上的点连成线段并进行两端延长

                    PointLineAccordanceCheck2(pFeatureDataset, lineFeaCls, pointFeaCls, pointFeature, lineFieldName,lineIndex, pointIndex,nearestFea,pShortestDis, intervalValue, out eError);
                    //PointLineAccordanceCheck(pFeatureDataset, lineFeaCls, pointFeaCls, pointFeature, lineIndex, pointIndex, pShortestDis, nearestFea, intervalValue, out eError);
                    if (eError != null) return;
                    pointFeature = pCusor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCusor);
            }
            catch (Exception ex)
            {
                eError = ex;
            }
        }

        /// <summary>
        /// 等高线点线矛盾一致性检查
        /// </summary>
        /// <param name="feaClsLst"></param>
        /// <param name="lineFeaClsName">等高线要素类名称</param>
        /// <param name="lineFieldName">等高线高程字段名</param>
        /// <param name="pointFeaClsName">高程点要素类名称</param>
        /// <param name="pointFieldName">高程点字段名</param>
        /// <param name="radiu">高程点搜索半径</param>
        /// <param name="intervalValue">等高线间距值</param>
        /// <param name="eError"></param>
        public void PointLineElevCheck(List<IFeatureClass> feaClsLst,string lineFeaClsName, string lineFieldName, string pointFeaClsName, string pointFieldName, double intervalValue, out Exception eError)
        {
            eError = null;
            try
            {
                //等高线要素类
                IFeatureClass lineFeaCls = null;// (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(lineFeaClsName);
                foreach (IFeatureClass mFeaCls in feaClsLst)
                {
                    string pName = (mFeaCls as IDataset).Name;
                    if(pName.Contains("."))
                    {
                        pName = pName.Substring(pName.IndexOf('.') + 1);
                    }
                    if (pName == lineFeaClsName)
                    {
                        lineFeaCls = mFeaCls;
                        break;
                    }
                }
                if (lineFeaCls == null) return;
                //等高线高程字段索引值
                int lineIndex = lineFeaCls.Fields.FindField(lineFieldName);
                if (lineIndex == -1)
                {
                    eError = new Exception("等高线图层的高程字段不存在,字段名为：" + lineFieldName);
                    return;
                }
                //高程点要素类
                IFeatureClass pointFeaCls = null;// (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(pointFeaClsName);
                foreach (IFeatureClass mFeaCls in feaClsLst)
                {
                    string pName = (mFeaCls as IDataset).Name;
                    if (pName.Contains("."))
                    {
                        pName = pName.Substring(pName.IndexOf('.') + 1);
                    }
                    if (pName == pointFeaClsName)
                    {
                        pointFeaCls = mFeaCls;
                        break;
                    }
                }
                if (pointFeaCls == null) return;
                int pointIndex = pointFeaCls.Fields.FindField(pointFieldName);
                if (lineIndex == -1)
                {
                    eError = new Exception("高程点图层的高程字段不存在,字段名为：" + pointFieldName);
                    return;
                }

                //遍历高程点要素
                IFeatureCursor pCusor = pointFeaCls.Search(null, false);
                if (pCusor == null) return;
                IFeature pointFeature = pCusor.NextFeature();

                //显示进度条
                ShowProgressBar(true);

                int tempValue = 0;
                ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, 0, pointFeaCls.FeatureCount(null), tempValue);

                while (pointFeature != null)
                {
                    //高程点要素的高程值
                    double pointElevValue = Convert.ToDouble(pointFeature.get_Value(pointIndex).ToString());
                    //查找高程点相邻的两条高程线要素

                    //与高程点最近的等高线要素以及最短距离
                    Dictionary<double, IFeature> nearestFeaDic = GetShortestDis(lineFeaCls, pointFeature, out eError);
                    if (eError != null || nearestFeaDic == null)
                    {
                        eError = new Exception("在搜索范围内的未找到要素!");
                        return;
                    }
                    double pShortestDis = -1;
                    IFeature nearestFea = null;
                    foreach (KeyValuePair<double, IFeature> item in nearestFeaDic)
                    {
                        pShortestDis = item.Key;
                        nearestFea = item.Value;
                        break;
                    }
                    if (eError != null || pShortestDis == -1) return;
                    //获得等高线上离高程点最近的点
                    IPoint nearestPoint = new PointClass();//等高线上的最近点
                    IProximityOperator mProxiOpe = nearestFea.Shape as IProximityOperator;
                    if (mProxiOpe == null) return;
                    nearestPoint = mProxiOpe.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                    //将高程点和等高线上的点连成线段并进行两端延长

                    PointLineAccordanceCheck2(null, lineFeaCls, pointFeaCls, pointFeature, lineFieldName, lineIndex, pointIndex, nearestFea, pShortestDis, intervalValue, out eError);
                    //PointLineAccordanceCheck(pFeatureDataset, lineFeaCls, pointFeaCls, pointFeature, lineIndex, pointIndex, pShortestDis, nearestFea, intervalValue, out eError);
                    if (eError != null) return;
                    pointFeature = pCusor.NextFeature();

                    tempValue += 1;//进度条的值加1
                    ChangeProgressBar1((hook as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCusor);
            }
            catch (Exception ex)
            {
                eError = ex;
            }
        }


        /// <summary>
        /// 在要素类上查找距离最近的要素
        /// </summary>
        /// <param name="desFeaCls">目标要素类</param>
        /// <param name="codeName">分类代码字段名</param>
        /// <param name="codeValue">分类代码值</param>
        /// <param name="oriFeature">源要素</param>
        /// <param name="radiu">缓冲半径</param>
        /// <param name="outError"></param>
        /// <returns></returns>
        private IFeature GetNearestFeature(IFeatureClass desFeaCls, string codeName, string codeValue, IFeature oriFeature, double radiu, out Exception outError)
        {
            outError = null;
            IFeature returnFeature = null;
            double pDistance = -1;
            try
            {
                //根据源要素的缓冲半径得到缓冲范围
                ITopologicalOperator pTopoOper = oriFeature.Shape as ITopologicalOperator;
                IGeometry pGeo = pTopoOper.Buffer(radiu);

                //查找目标要素类中给定分类代码的缓冲范围内的要素
                string str = "";
                if (codeValue != "")
                {
                    str = codeName + " = '" + codeValue + "'";
                }
                ISpatialFilter pFilter = new SpatialFilterClass();
                pFilter.WhereClause = str;
                pFilter.GeometryField = "SHAPE";
                pFilter.Geometry = pGeo;
                pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;//.esriSpatialRelWithin;
                IFeatureCursor pCursor = desFeaCls.Search(pFilter, false);
                if (pCursor == null) return null;
                IFeature pFeature = pCursor.NextFeature();

                //遍历要素，查找距离最近的要素
                while (pFeature != null)
                {
                    IProximityOperator pProxiOper = oriFeature.Shape as IProximityOperator;
                    double tempDis = pProxiOper.ReturnDistance(pFeature.Shape);
                    if (pDistance == -1 || pDistance > tempDis)
                    {
                        pDistance = tempDis;
                        returnFeature = pFeature;
                    }
                    pFeature = pCursor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
            return returnFeature;
        }

        /// <summary>
        /// 点要素与目标要素的最短距离
        /// </summary>
        /// <param name="desFeaCls"></param>
        /// <param name="pointFeature">点要素</param>
        /// <param name="radiu">点要素搜索半径</param>
        /// <param name="outError"></param>
        /// <returns></returns>
        private Dictionary<double, IFeature> GetShortestDis(IFeatureClass desFeaCls, IFeature pointFeature, out Exception outError)
        {
            outError = null;
            Dictionary<double, IFeature> ShortestFeature = new Dictionary<double, IFeature>();
            double pShortestDistance = -1;
            IFeature pFeature = null;
            try
            {
                //List<IFeature> tempFeaLst = new List<IFeature>();
                //tempFeaLst = GetFeatureByDis(desFeaCls, pointFeature,esriSpatialRelEnum.esriSpatialRelIntersects, radiu);
                IFeatureCursor pCusor = desFeaCls.Search(null, false);
                if (pCusor == null) return null ;
                IFeature mFea = pCusor.NextFeature();
                //遍历要素，查找距离最近的要素,
                while(mFea!=null)
                {
                    IProximityOperator pProxiOper = pointFeature.Shape as IProximityOperator;
                    double tempDis = pProxiOper.ReturnDistance(mFea.Shape);
                    if (pShortestDistance == -1 || pShortestDistance > tempDis)
                    {
                        pShortestDistance = tempDis;
                        pFeature = mFea;
                    }
                    mFea = pCusor.NextFeature();
                }
                if (pShortestDistance == -1||pFeature==null)
                {
                    outError = new Exception("未找到任何要素!");
                    return null;
                }
                ShortestFeature.Add(pShortestDistance, pFeature);
            }
            catch (Exception ex )
            {
                outError = ex;
            }
            return ShortestFeature;
        }

        private void PointLineAccordanceCheck(IFeatureDataset pFeaDataset, IFeatureClass desFeaCls, IFeatureClass oriFeaCls, IFeature pointFeature, int lineIndex, int pointIndex, double pShortestDistance,IFeature nearestFea, double intervalElev, out Exception eError)
        {
            eError = null;
            try
            {
                //离点最近和次近的线要素
                IFeature pLineFeature1 = null;
                IFeature pLineFeature2 = null;
                //两个线要素的高程值
                string pLineElev1 = "";
                string pLineElev2 = "";
                //点要素的高程值
                string pPointElev = "";
                //错误ID
                int eErrorID = enumErrorType.等高线点线矛盾检查.GetHashCode();
                //错误描述
                string eErrorDes = "";

                //点要素的高程值
                pPointElev = pointFeature.get_Value(pointIndex).ToString().Trim();
                if (pPointElev == "")
                {
                    eError = new Exception("高程点的高程值为空，OID为：" + pointFeature.OID);
                    return;
                }
                if (pShortestDistance == 0)
                {
                    #region 高程点位于等高线上
                    //高程点位于等高线上
                    
                    pLineElev1 =nearestFea.get_Value(lineIndex).ToString().Trim();
                    if (pLineElev1 == "")
                    {
                        eError = new Exception("等高线的高程值为空，OID为：" + nearestFea.OID);
                        return;
                    }
                    //比较点要素和线要素的高程值，若不相等，则认为不正确
                    if (Convert.ToDouble(pLineElev1) != Convert.ToDouble(pPointElev))
                    {
                        //将错误结果保存起来
                        eErrorDes = "高程点与高程线的高程值矛盾";
                        GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                        if (eError != null) return;
                    }
                    #endregion 
                }
                else if (pShortestDistance >= intervalElev)
                {
                    #region  高程点位于等高线的最里面或最外面
                    //根据该距离进行查找
                    List<IFeature> lstFea = GetFeatureByDis(desFeaCls, pointFeature, esriSpatialRelEnum.esriSpatialRelTouches, pShortestDistance);
                    if (lstFea==null|| lstFea.Count == 0) return;
                    //离点最近的一条线
                    pLineFeature1 = lstFea[0];
                    pLineElev1 = pLineFeature1.get_Value(lineIndex).ToString().Trim();
                    if (pLineElev1 == "")
                    {
                        eError = new Exception("等高线的高程值为空，OID为：" + pLineFeature1.OID);
                        return;
                    }
                    lstFea = GetFeatureByDis(desFeaCls, pointFeature, esriSpatialRelEnum.esriSpatialRelTouches, (intervalElev+pShortestDistance));
                    if (lstFea == null || lstFea.Count == 0) return;
                    //离点次近的一条线
                    pLineFeature2 = lstFea[0];
                    pLineElev2 = pLineFeature2.get_Value(lineIndex).ToString().Trim();
                    if (pLineElev2 == "")
                    {
                        eError = new Exception("等高线的高程值为空，OID为：" + pLineFeature2.OID);
                        return;
                    }
                    eErrorDes = "高程点与最近的等高线高程值矛盾";
                    if ((Convert.ToDouble(pLineElev1) < Convert.ToDouble(pLineElev2)) && (Convert.ToDouble(pPointElev) > Convert.ToDouble(pLineElev1)))
                    {
                        //高程点错误的高程值，保存错误结果
                        GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, pLineFeature1, eErrorID, eErrorDes, out eError);
                    }
                    if ((Convert.ToDouble(pLineElev1) > Convert.ToDouble(pLineElev2)) && (Convert.ToDouble(pPointElev) < Convert.ToDouble(pLineElev1)))
                    {
                        //高程点错误的高程值，保存错误结果
                        GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, pLineFeature1, eErrorID, eErrorDes, out eError);
                    }
                    #endregion
                }
                else if(pShortestDistance<intervalElev)
                {
                    #region 包含两种情况，高程点位于两条等高线之间、高程点位于等高线最外面或最里面
                    List<IFeature> lstFeature2 = new List<IFeature>();
                    //根据距离进行查找
                    lstFeature2 = GetFeatureByDis(desFeaCls, pointFeature, esriSpatialRelEnum.esriSpatialRelIntersects, (intervalElev - pShortestDistance));
                    if (lstFeature2 == null || lstFeature2.Count == 0) return;
                    if (lstFeature2.Count == 1)
                    {
                        //高程点位于等高线的最外面或最里面
                        //离高程点最近的要素
                        pLineFeature1 = lstFeature2[0];
                        pLineElev1 = pLineFeature1.get_Value(lineIndex).ToString().Trim();
                        if (pLineElev1 == "")
                        {
                            eError = new Exception("等高线的高程值为空，OID为：" + pLineFeature1.OID);
                            return;
                        }
                        List<IFeature> mFeaLst = GetFeatureByDis(desFeaCls, pointFeature, esriSpatialRelEnum.esriSpatialRelTouches, (pShortestDistance+intervalElev));
                        if (mFeaLst == null || mFeaLst.Count == 0) return;
                        //离高程点次近的要素
                        pLineFeature2 = mFeaLst[0];
                        pLineElev2 = pLineFeature2.get_Value(lineIndex).ToString().Trim();
                        if (pLineElev2 == "")
                        {
                            eError = new Exception("等高线的高程值为空，OID为：" + pLineFeature2.OID);
                            return;
                        }
                        eErrorDes = "高程点与最近的等高线高程值矛盾";
                        if ((Convert.ToDouble(pLineElev1) < Convert.ToDouble(pLineElev2)) && (Convert.ToDouble(pLineElev1) < Convert.ToDouble(pPointElev)))
                        {
                            //高程点与等高线高程值矛盾，保存错误结果
                            GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, pLineFeature1, eErrorID, eErrorDes,out eError);
                        }
                        if ((Convert.ToDouble(pLineElev1) > Convert.ToDouble(pLineElev2)) && (Convert.ToDouble(pLineElev1) > Convert.ToDouble(pPointElev)))
                        {
                            //高程点与等高线高程值矛盾，保存错误结果
                            GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, pLineFeature1, eErrorID, eErrorDes, out eError);
                        }
                    }
                    else if (lstFeature2.Count == 2)
                    {
                        //高程点位于等高线之间
                        //离高程点最近的线要素
                        List<IFeature> pFeaLst1 = GetFeatureByDis(desFeaCls, pointFeature, esriSpatialRelEnum.esriSpatialRelTouches, pShortestDistance);
                        if (pFeaLst1 == null || pFeaLst1.Count == 0) return;
                        pLineFeature1 = pFeaLst1[0];
                        pLineElev1 = pLineFeature1.get_Value(lineIndex).ToString().Trim();
                        if (pLineElev1 == "")
                        {
                            eError = new Exception("等高线的高程值为空，OID为：" + pLineFeature1.OID);
                            return;
                        }
                        //离高程点次近的线要素
                        List<IFeature> pFeaLst2 = GetFeatureByDis(desFeaCls, pointFeature, esriSpatialRelEnum.esriSpatialRelTouches, (intervalElev - pShortestDistance));
                        if (pFeaLst2 == null || pFeaLst2.Count == 0) return;
                        pLineFeature2 = pFeaLst2[0];
                        pLineElev2 = pLineFeature2.get_Value(lineIndex).ToString().Trim();
                        if (pLineElev2 == "")
                        {
                            eError = new Exception("等高线的高程值为空，OID为：" + pLineFeature2.OID);
                            return;
                        }
                        eErrorDes = "高程点与邻近的两条等高线高程值矛盾";
                        if (Convert.ToDouble(pLineElev1) > Convert.ToDouble(pLineElev2))
                        {
                            if((Convert.ToDouble(pPointElev)>=Convert.ToDouble(pLineElev1))||(Convert.ToDouble(pPointElev)<=Convert.ToDouble(pLineElev2)))
                            {
                                GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, pLineFeature1, eErrorID, eErrorDes, out eError);
                                if (eError != null) return;
                            }
                        }
                        if (Convert.ToDouble(pLineElev1) < Convert.ToDouble(pLineElev2))
                        {
                            if ((Convert.ToDouble(pPointElev) <= Convert.ToDouble(pLineElev1)) || (Convert.ToDouble(pPointElev) >= Convert.ToDouble(pLineElev2)))
                            {
                                GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, pLineFeature1, eErrorID, eErrorDes, out eError);
                                if (eError != null) return;
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                eError = ex;
                return;
            }
        }

        /// <summary>
        /// 等高线高程点点线矛盾检查,还需要完善
        /// </summary>
        /// <param name="pFeaDataset"></param>
        /// <param name="desFeaCls">等高线要素类</param>
        /// <param name="oriFeaCls">高程点要素类</param>
        /// <param name="pointFeature">高程点要素</param>
        /// <param name="lineFieldName">等高线高程字段名</param>
        /// <param name="lineIndex">等高线高程字段索引</param>
        /// <param name="pointIndex">高程点高程字段民</param>
        /// <param name="nearestFea">离高程点要素最近的等高线要素</param>
        /// <param name="intervalElev">等高线间距值</param>
        /// <param name="eError"></param>
        private void PointLineAccordanceCheck2(IFeatureDataset pFeaDataset, IFeatureClass desFeaCls, IFeatureClass oriFeaCls, IFeature pointFeature,string lineFieldName,int lineIndex, int pointIndex, IFeature nearestFea,double pShortestDis, double intervalElev, out Exception eError)
        {
            eError = null;

            try
            {
                double pValue = 0.0;                                       //点要素的高程值
                double fValue = 0.0;                                       //最近线要素高程值
                double lValue = 0.0;                                        //最近线要素相邻的第一条要素
                double sValue = 0.0;                                       //最近线要素相邻的第二条要素
                int eErrorID = enumErrorType.等高线点线矛盾检查.GetHashCode(); //错误ID
                string eErrorDes = "等高线与高程点高程值点线矛盾！";       //错误描述

                //点要素的高程值
                string pValueStr = pointFeature.get_Value(pointIndex).ToString().Trim();
                if (pValueStr == "")
                {
                    eError = new Exception("高程点的高程值为空，OID为：" + pointFeature.OID);
                    return;
                }
                pValue = Convert.ToDouble(pValueStr);
                //最近线要素高程值
                string fValueStr = nearestFea.get_Value(lineIndex).ToString().Trim();
                if (fValueStr == "")
                {
                    eError = new Exception("线要素的高程值为空。OID为：" + nearestFea.OID);
                    return;
                }
                fValue = Convert.ToDouble(fValueStr);

                if (pShortestDis == 0)
                {
                    //说明点在线上
                    if (pValue != fValue)
                    {
                        //点线高程值矛盾
                        GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                        if (eError != null) return;
                    }
                }
                else
                {
                    if (fValue < intervalElev)
                    {
                        #region 说明是最小高程,说明该要素是最里面或最外面的要素
                        //最近线要素相邻的第一条要素的高程值
                        lValue = fValue + intervalElev;
                        string whereStr = lineFieldName + "=" + lValue;
                        List<IFeature> lstFefa = GetFeatureByStr(desFeaCls, whereStr, out eError);
                        if (eError != null || lstFefa == null || lstFefa.Count == 0)
                        {
                            return;
                        }
                        //bool isIntersact = false;
                        
                        //for (int k = 0; k < lstFefa.Count; k++)
                        //{
                        if (lstFefa.Count == 1)
                        {
                            IFeature secondFea = lstFefa[0];
                            //返回secondFea上与高程点最近的点
                            IProximityOperator pProxiOper = secondFea.Shape as IProximityOperator;
                            IPoint p = new PointClass();
                            p = pProxiOper.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                            //    if (IsIntersect(pointFeature.Shape as IPoint, p, nearestFea))
                            //    {
                            //        isIntersact = true;
                            //        secondFea = lstFefa[k];
                            //        break;
                            //    }
                            //}

                            #region 最近线要素相邻的第一条要素上点与高程点连线与最近线要素的拓扑关系包含两种情况：相交、不相交。
                            if (IsIntersect(pointFeature.Shape as IPoint, p, nearestFea))
                            {
                                //若两点连线与nearestFea相交，说明高程点在线要素的最里面或者最外面
                                //由于fValue<lValue
                                if (pValue > fValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                            }
                            else
                            {
                                //若两点连线与nearestFea不相交，高程点在两条线要素之间
                                if (pValue < fValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, secondFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                                if (pValue > lValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                            }
                            #endregion
                        }

                        #endregion
                    }
                    else if (fValue >= intervalElev)
                    {
                        #region 分为两种情况：或者是最里面或最外面的线要素、或者是中间线要素
                        //与nearestFea相邻的两线要素的高程值
                        lValue = fValue + intervalElev;
                        sValue = fValue - intervalElev;
                        //与最近要素要素的两个要素
                        IFeature secondFea = null;
                        IFeature thirdFea = null;

                        string whereStr = lineFieldName + "=" + lValue;
                        
                        List<IFeature> lstFea2=GetFeatureByStr(desFeaCls, whereStr, out eError);
                        if (eError != null||lstFea2==null)
                        {
                            return;
                        }
                        whereStr = lineFieldName + "=" + sValue;
                        List<IFeature> lstFea3=GetFeatureByStr(desFeaCls, whereStr, out eError);
                        if (eError != null || lstFea3 == null)
                        {
                            return;
                        }
                      
                        if (lstFea2.Count==0&&lstFea3.Count==0)
                        {
                            //只有一条等高线
                            eError = new Exception("只有一条等高线，不能进行检查！");
                            return;
                        }
                        else if (lstFea2.Count==1 && lstFea3.Count==1)
                        {
                            secondFea = lstFea2[0];
                            thirdFea = lstFea3[0];
                            #region nearestFea不是最里面或最外面的要素
                            IPoint p = new PointClass();
                            IProximityOperator pProxiOper = secondFea.Shape as IProximityOperator;
                            p = pProxiOper.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                            if (IsIntersect(pointFeature.Shape as IPoint, p, nearestFea))
                            {
                                //相交。sValue<fValue<lValue ,pValue应位于sValue与fValue之间
                                if (pValue < sValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, thirdFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                                if (pValue > fValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                            }
                            else
                            {
                                //不相交。sValue<fValue<lValue ,pValue应位于fValue与lValue之间
                                if (pValue < fValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                                if (pValue > lValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, secondFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                            }
                            #endregion
                        }
                        else if(lstFea2.Count==1 || lstFea3.Count==1)
                        {
                            if (lstFea2.Count == 1&&lstFea3.Count==0)
                            {
                                secondFea = lstFea2[0];
                            }
                            else if (lstFea3.Count == 1 && lstFea2.Count == 0)
                            {
                                thirdFea = lstFea3[0];
                            }
                            if (secondFea == null && thirdFea == null) return;

                            #region  nearestFea是最里面或最外面的要素
                            IPoint p = new PointClass();
                            if (secondFea != null)
                            {
                                IProximityOperator pProxiOper = secondFea.Shape as IProximityOperator;
                                p = pProxiOper.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                            }
                            if (thirdFea != null)
                            {
                                IProximityOperator pProxiOper = thirdFea.Shape as IProximityOperator;
                                p = pProxiOper.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                            }
                            if (IsIntersect(pointFeature.Shape as IPoint, p, nearestFea))
                            {
                                //相交。高程点在等高线的最外面或最里面
                                if (secondFea != null)
                                {
                                    //由于fValue<lValue
                                    if (pValue > fValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                }
                                if (thirdFea != null)
                                {
                                    //由于fValue>sValue
                                    if (pValue < fValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                }
                            }
                            else
                            {
                                //不相交。高程点在等高线的最靠边的两条等高线之间
                                if (secondFea != null)
                                {
                                    //由于fValue<lValue
                                    if (pValue < fValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                    if (pValue > lValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, secondFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                }
                                if (thirdFea != null)
                                {
                                    //由于fValue>sValue
                                    if (pValue > fValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                    if (pValue < sValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(pFeaDataset, oriFeaCls, pointFeature, desFeaCls, thirdFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                eError = ex;
            }
        }
        /// <summary>
        /// 两点之间的连线是否与线相交
        /// </summary>
        /// <param name="mFPoint"></param>
        /// <param name="mLPoint"></param>
        /// <param name="lineFeature"></param>
        /// <returns>true:相交;false:不相交</returns>
        private bool IsIntersect(IPoint mFPoint,IPoint mLPoint,IFeature lineFeature)
        {
            //声明线要素
            //IPolyline mPLine = new PolylineClass();
            //mPLine.FromPoint = mFPoint;
            //mPLine.ToPoint = mLPoint;
            //声明点集
            //IPointCollection mPointCol = new PolylineClass();
            //object obj = System.Reflection.Missing.Value;
            //mPointCol.AddPoint(mFPoint, ref obj, ref obj);
            //mPointCol.AddPoint(mLPoint, ref obj, ref obj);
            //mPLine = mPointCol as IPolyline;

            ILine pLine=new LineClass();
            pLine.PutCoords(mFPoint,mLPoint);
            ISegmentCollection pSegCol=new PolylineClass();
            object obj=Type.Missing;
            pSegCol.AddSegment(pLine as ISegment,ref obj,ref obj);
            IRelationalOperator pRelOpera=pSegCol as IRelationalOperator;
            if (pRelOpera.Disjoint(lineFeature.Shape))
            {
                //不相交
                return false;
            }
            else
            {
                //相交
                return true;
            }
        }

        /// <summary>
        /// 根据指定的条件获得要素
        /// </summary>
        /// <param name="desFeaCls"></param>
        /// <param name="whereStr"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private List<IFeature> GetFeatureByStr(IFeatureClass desFeaCls,string whereStr,out Exception eError)
        {
            eError=null;
            List<IFeature> LstFeature = new List<IFeature>();
            try
            {
                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = whereStr;

                IFeatureCursor pCusor = desFeaCls.Search(pFilter, false);
                if (pCusor == null) return null;
                IFeature pFea = pCusor.NextFeature();
                while (pFea != null)
                {
                    LstFeature.Add(pFea);
                    pFea = pCusor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCusor);
                return LstFeature;
            }
            catch (Exception ex)
            {
                eError = ex;
                return null;
            }
        }

        /// <summary>
        /// 根据要素的缓冲距离查找要素
        /// </summary>
        /// <param name="desFeaCls"></param>
        /// <param name="pointFeature"></param>
        /// <param name="dis"></param>
        /// <returns></returns>
        private List<IFeature> GetFeatureByDis(IFeatureClass desFeaCls, IFeature pointFeature,esriSpatialRelEnum spatialRelEnum, double dis)
        {
            List<IFeature> LstFeature = new List<IFeature>();
            //根据源要素的缓冲半径得到缓冲范围
            ITopologicalOperator pTopoOper = pointFeature.Shape as ITopologicalOperator;
            IGeometry pGeo = pTopoOper.Buffer(dis);

            //查找缓冲范围内的要素
            ISpatialFilter pFilter = new SpatialFilterClass();
            pFilter.GeometryField = "SHAPE";
            pFilter.Geometry = pGeo;
            pFilter.SpatialRel = spatialRelEnum;// esriSpatialRelEnum.esriSpatialRelIntersects;//.esriSpatialRelWithin;
            IFeatureCursor pCursor = desFeaCls.Search(pFilter, false);
            if (pCursor == null) return null;
            IFeature pFeature = pCursor.NextFeature();
            while (pFeature != null)
            { 
                LstFeature.Add(pFeature);
                pFeature=pCursor.NextFeature();
            }
            return LstFeature;
        }

        /// <summary>
        /// 获得错误列表，等高线点线矛盾
        /// </summary>
        /// <param name="pFeatureDataset"></param>
        /// <param name="pFeatureCls"></param>
        /// <param name="feaClsname"></param>
        /// <param name="pFeature"></param>
        /// <param name="eErrorID"></param>
        /// <param name="eError"></param>
        private void GetErrorList(IFeatureDataset pFeatureDataset,IFeatureClass pOriFeatureCls,IFeature pOriFeature,IFeatureClass pDesFeatureCls, IFeature pDesFeature, int eErrorID,string eErrorDes, out Exception eError)
        {
            eError = null;
            try
            {
                double pMapx = 0.0;
                double pMapy = 0.0;
                IPoint pPoint = null;
                //高程值不在给定的高程范围内,将错误结果保存下来
                if (pOriFeatureCls.ShapeType != esriGeometryType.esriGeometryPoint)
                {
                    pPoint = TopologyCheckClass.GetPointofFeature(pOriFeature);
                }
                else
                {
                    pPoint = pOriFeature.Shape as IPoint;
                }
                pMapx = pPoint.X;
                pMapy = pPoint.Y;

                List<object> ErrorLst = new List<object>();
                ErrorLst.Add("要素属性检查");//功能组名称
                ErrorLst.Add("等高线点线矛盾检查");//功能名称
                if (pFeatureDataset == null)
                {
                    ErrorLst.Add("");  //数据文件名
                }
                else
                {
                    ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                }
                
                ErrorLst.Add(eErrorID );//错误id;
                ErrorLst.Add(eErrorDes);//错误描述
                ErrorLst.Add(pMapx);    //...
                ErrorLst.Add(pMapy);    //...
                ErrorLst.Add((pOriFeatureCls as IDataset).Name);
                ErrorLst.Add(pOriFeature.OID);
                ErrorLst.Add((pDesFeatureCls as IDataset).Name);
                ErrorLst.Add(pDesFeature.OID);
                ErrorLst.Add(false);
                ErrorLst.Add(System.DateTime.Now.ToString());

                //传递错误日志
                IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
            }
            catch (Exception ex)
            {
                eError = ex;
            }
        }


        #endregion

        # region 接边检查
        double SearchValue = 0;//搜索缓冲的值
        double AreaValue = 0;//范围缓冲的值
        public double SEARCHValue
        {
            get 
            {
                return SearchValue;
            }
            set 
            {
                SearchValue=value;
            }
        }
        public double AREAValue
        {
            get 
            {
                return AreaValue;
            }
            set 
            {
                AreaValue=value;
            }
        }
        private Plugin.Application.IAppGISRef _AppHk;
        ArrayList FilterList = new ArrayList();//用来过滤重复出现的行
      
        /// <summary>
        /// 清空出错的行
        /// </summary>
        /// <param name="AppHk"></param>
        private delegate void ClearCheck(Plugin.Application.IAppGISRef AppHk);
        private void Clear(Plugin.Application.IAppGISRef AppHk)
        {
            if (AppHk.DataCheckGrid.Rows.Count > 0)
            {
                AppHk.DataCheckGrid.DataSource = null;
            }
            Plugin.Application.IAppFormRef pAppFormRef = AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
            AppHk.DataTree.Nodes.Clear();

        }

        /// <summary>
        /// 双击事件具体的实现委托的方法 并将我们组织的数据邦定到列表当中 定义这个委托
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="AppHk"></param>
        private delegate void DeleteBindCilck(DataTable tb, Plugin.Application.IAppGISRef AppHk);
        /// <summary>
        /// 双击事件具体的实现委托的方法 并将我们组织的数据邦定到列表当中
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="AppHk"></param>
        private void BindClick(DataTable tb, Plugin.Application.IAppGISRef AppHk)
        {
            AppHk.DataCheckGrid.DataSource = tb;
            AppHk.DataCheckGrid.Columns[0].Width = AppHk.DataCheckGrid.Width/2-5;
            AppHk.DataCheckGrid.Columns[1].Width = AppHk.DataCheckGrid.Width / 2 - 5;
            AppHk.DataCheckGrid.Columns[2].Visible = false;//隐藏第三列
            if (AppHk.DataCheckGrid.DataSource != null)
            {
                AppHk.DataCheckGrid.MouseDoubleClick += new MouseEventHandler(Overridfunction.DataCheckGridDoubleClick);//加入双击事件
            }
        }

        /// <summary>
        /// 初始化树
        /// </summary>
        public bool Initialize_Tree(SysCommon.Gis.SysGisDataSet pRangeGisDB, string LayerName, string FiledName, DevComponents.AdvTree.AdvTree pDataTree, DevComponents.AdvTree.AdvTree Trees, Plugin.Application.IAppGISRef AppHk,out Exception err)
        {
            err = null;

            ArrayList StrParlist = new ArrayList();//用来存放节点上面我们所选择的层属性字段名
            bool CheckTree = false;//确定是否选择过层
            pDataTree.Nodes.Clear();//清除所有的节点
            //获取要接边检查的数据图层 从设置界面上树上选择的层取
            List<ILayer> listCheckLays = new List<ILayer>();
            if (Trees.Nodes.Count == 0) return false;
            foreach (DevComponents.AdvTree.Node tempnode in Trees.Nodes)
            {
                if (tempnode.Checked)
                {
                    CheckTree = true;
                    StrParlist.Add(tempnode.Name.Trim());//将对应的层字段属性加入到参数列表当中
                    listCheckLays.Add(tempnode.Tag as ILayer);//将设置界面上我们选择要进行接边检查的层存入到动态数组当中
                }
            }
            if (!CheckTree)
            {
                SetCheckState.Message(AppHk as Plugin.Application.IAppFormRef, "提示", "请选择图层进行检查！");
                return false;
            }
            bool ReState = BindTree(pRangeGisDB,LayerName, FiledName, pDataTree, listCheckLays, StrParlist, AppHk,out err);
            if (!ReState)
            {
                return false;
            }
            
            return CheckTree;//返回状态，确定是否选择过要检查接边的层
        }
        /// <summary>
        /// 在进行接边检查时，可以选择利用图幅为参照还是以任务为参照，然后邦定显示的树的数据
        /// </summary>
        /// <param name="LayerName"></param>
        /// <param name="FiledName"></param>
        /// <param name="pDataTree"></param>
        private bool BindTree(SysCommon.Gis.SysGisDataSet pRangeGisDB, string LayerName, string FiledName, DevComponents.AdvTree.AdvTree pDataTree, List<ILayer> listCheckLays, ArrayList StrParlist, Plugin.Application.IAppGISRef AppHk,out Exception err)
        {
            bool State = true;
            err = null;
            try
            {
                //获取图幅
                IFeatureClass pMapFeaCls=(pRangeGisDB.WorkSpace as IFeatureWorkspace).OpenFeatureClass(LayerName);
                 IFeatureLayer pFeatMapLay=new FeatureLayerClass();
                pFeatMapLay.FeatureClass=pMapFeaCls;
                if (pFeatMapLay == null) return false;
                //遍历图幅结合表
                if (pMapFeaCls.Fields.FindField(FiledName) == -1) return false;
                IFeatureCursor pFeatCur = pMapFeaCls.Search(null, false);
                IFeature pFeature = pFeatCur.NextFeature();
                string tempStr = "接边范围";
                //if (LayerName.IndexOf("图幅") != -1) tempStr = "图幅：";
                while (pFeature != null)
                {
                    string TableName = pFeature.get_Value(pFeature.Fields.FindField(FiledName)).ToString();//图幅结合表编号
                    DevComponents.AdvTree.Node aNode = new DevComponents.AdvTree.Node();
                    aNode.Text = tempStr + TableName;
                    aNode.Name = TableName;
                    aNode.Tag = pFeature;//将图幅存放在父节点TAG上
                    aNode.Image = pDataTree.ImageList.Images[4];//设置节点上显示的图片

                    //将所有的线和面层作为子节点组织形式如：图幅为父节点，线和面层为子节点
                    for (int n = 0; n < listCheckLays.Count; n++)
                    {
                        DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                        node.Text = ((listCheckLays[n] as IFeatureLayer).FeatureClass as IDataset).Name;//listCheckLays[n].Name;//将层名作为子节点
                        node.Name = TableName + "@" + ((listCheckLays[n] as IFeatureLayer).FeatureClass as IDataset).Name;// listCheckLays[n].Name;
                        node.Tag = listCheckLays[n];//将对应的层放到对应的以层名为子节点的节点TAG上，以备后面使用
                        node.Image = pDataTree.ImageList.Images[8];//设置子节点的图片
                        node.DataKey = StrParlist[n];//属性值
                        aNode.Nodes.Add(node);

                    }

                    aNode.CollapseAll();
                    pDataTree.Nodes.Add(aNode);//将组织好的节点加入树中
                    pFeature = pFeatCur.NextFeature();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatCur);
                return State;
            }
            catch (System.Exception ex)
            {
                err = ex;
                return false;
            }
           
        }
        /// <summary>
        /// 接边检查主函数 , int pur
        /// </summary>
        /// <param name="pAppHk"></param>
        public void DoJoinCheck(object Para,int pur)
        {
            //Application.DoEvents();
            Plugin.Application.IAppGISRef pAppHk = Para as Plugin.Application.IAppGISRef;
            char[] splite = new char[] { ' ' };//分割的参照
            Plugin.Application.IAppFormRef pAppFrm = pAppHk as Plugin.Application.IAppFormRef;
            if (pAppHk.DataTree.Nodes.Count == 0) return;

            //创建接边检查结果列表
            //DataTable table = new DataTable();
            //table.Columns.Add("源数据", typeof(string));
            //table.Columns.Add("目标数据", typeof(string));
            //table.Columns.Add("点", typeof(object));

            pAppFrm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { pAppFrm, true });

            int intAllCnt = pAppHk.DataTree.Nodes.Count;
            int intCnt = 0;
            foreach (DevComponents.AdvTree.Node aNode in pAppHk.DataTree.Nodes)
            {
                intCnt++;
                //选中该图幅节点
                pAppFrm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { pAppHk.DataTree, aNode });
                string strMemo = "进行" + aNode.Text + "(" + intCnt.ToString() + "/" + intAllCnt.ToString() + ")接边检查...";
                pAppFrm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { pAppFrm, strMemo });

                IFeature pFeature = aNode.Tag as IFeature;
                if (pFeature == null) continue;
                double distance = SearchValue;
                //将整个的区作为对象，取边界进行缓冲
                //首先将线做缓冲
                try
                {
                    if (pur == 1)
                    {
                        #region 以任务区作为接边参照
                        
                        ITopologicalOperator topo = pFeature.Shape as ITopologicalOperator;
                        IGeometry geo = topo.Boundary;//得到区的边界
                        topo.Simplify();

                        IGeometry NewGeo = null;
                        ITopologicalOperator NewTopo = null;
                        NewTopo = geo as ITopologicalOperator;
                        NewGeo = NewTopo.Buffer(distance);//得到边界后，再次的进行缓冲
                        NewTopo = NewGeo as ITopologicalOperator;
                        NewTopo.Simplify();

                        ////缓冲范围与任务区求交
                        NewGeo = NewTopo.Intersect(pFeature.Shape, esriGeometryDimension.esriGeometry2Dimension);
                        NewTopo = NewGeo as ITopologicalOperator;
                        NewTopo.Simplify(); //提高查询效率 

                        int intLayCnt = 0;
                        int intLays = aNode.Nodes.Count;
                        foreach (DevComponents.AdvTree.Node nodeChild in aNode.Nodes)
                        {
                            intLayCnt++;
                            //选中该图幅节点
                            pAppFrm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { pAppHk.DataTree, nodeChild });
                            strMemo = "进行" + aNode.Text + "(" + intCnt.ToString() + "/" + intAllCnt.ToString() + ")中的" + nodeChild.Text + "(" + intLayCnt.ToString() + "/" + intLays.ToString() + ")接边检查...";
                            pAppFrm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { pAppFrm, strMemo });

                            ILayer pLay = nodeChild.Tag as ILayer;
                            if (pLay == null) continue;

                            //遍历图层获取范围内的要素

                            List<IFeature> listLeftFeatures = GetFeaturesByGeometry(pLay, NewGeo);

                            if (listLeftFeatures == null) continue;
                            int allFeatsCnt = listLeftFeatures.Count;
                            if (allFeatsCnt == 0) continue;

                            pAppFrm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { pAppFrm.ProgressBar, 0, allFeatsCnt, 0 });
                            pAppFrm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { pAppFrm, strMemo });

                            //取节点上面挂着的图层用来检查的属性名
                            List<string> tempAttribute = new List<string>();//形成属性数组
                            string[] Attribute = nodeChild.DataKey.ToString().Split(splite);
                            for (int A = 0; A < Attribute.Length; A++)
                            {
                                tempAttribute.Add(Attribute[A]);
                            }
                            //以任务为参照进行接边检查
                            DoJoinCheckByEdge(pFeature, NewGeo, pLay, listLeftFeatures, pAppFrm, AreaValue * 2, tempAttribute);
                            
                        }
                        
                        #endregion
                    }
                    else
                    {
                        //获取图幅左边缓冲范围
                        #region 以图幅作为参照
                        IGeometry pLeftGeometry = GetBufferGeometryByMapFrame(pFeature.Shape, distance, 1);
                        //缓冲范围与图幅求交
                        ITopologicalOperator pTop = pLeftGeometry as ITopologicalOperator;
                        pLeftGeometry = pTop.Intersect(pFeature.Shape, esriGeometryDimension.esriGeometry2Dimension);
                        pTop = pLeftGeometry as ITopologicalOperator;
                        pTop.Simplify();  //提高查询效率

                        //获取图幅下边缓冲范围
                        IGeometry pDownGeometry = GetBufferGeometryByMapFrame(pFeature.Shape, distance, 4);
                        //缓冲范围与图幅求交
                        pTop = pDownGeometry as ITopologicalOperator;
                        pDownGeometry = pTop.Intersect(pFeature.Shape, esriGeometryDimension.esriGeometry2Dimension);
                        pTop = pDownGeometry as ITopologicalOperator;
                        pTop.Simplify(); //提高查询效率 



                        int intLayCnt = 0;
                        int intLays = aNode.Nodes.Count;
                        foreach (DevComponents.AdvTree.Node nodeChild in aNode.Nodes)
                        {
                            intLayCnt++;
                            //选中该图幅节点
                            pAppFrm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { pAppHk.DataTree, nodeChild });
                            strMemo = aNode.Text + "(" + intCnt.ToString() + "/" + intAllCnt.ToString() + ")中的" + nodeChild.Text + "(" + intLayCnt.ToString() + "/" + intLays.ToString() + ")接边检查...";
                            pAppFrm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { pAppFrm, strMemo });

                            ILayer pLay = nodeChild.Tag as ILayer;
                            if (pLay == null) continue;

                            //遍历图层获取左边范围内的要素

                            List<IFeature> listLeftFeatures = GetFeaturesByGeometry(pLay, pLeftGeometry);
                            //遍历图层获取下边范围内的要素
                            List<IFeature> listDownFeatures = GetFeaturesByGeometry(pLay, pDownGeometry);

                            if (listLeftFeatures == null && listDownFeatures == null) continue;
                            int allFeatsCnt = listLeftFeatures.Count + listDownFeatures.Count;
                            if (allFeatsCnt == 0) continue;

                            pAppFrm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { pAppFrm.ProgressBar, 0, allFeatsCnt, 0 });
                            pAppFrm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { pAppFrm, strMemo });

                            //取节点上面挂着的图层用来检查的属性名
                            List<string> tempAttribute = new List<string>();//形成属性数组
                            string[] Attribute = nodeChild.DataKey.ToString().Split(splite);
                            for (int A = 0; A < Attribute.Length; A++)
                            {
                                tempAttribute.Add(Attribute[A]);
                            }
                            //做图幅左边接边图层检查
                            DoJoinCheckByEdge(pFeature, pLeftGeometry, pLay, listLeftFeatures,  pAppFrm, distance * 2, tempAttribute);
                            //做图幅下边接边图层检查
                            DoJoinCheckByEdge(pFeature, pDownGeometry, pLay, listDownFeatures, pAppFrm, distance * 2, tempAttribute);
                            
                        }

                        #endregion
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeature);
                }
                catch (Exception ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("容差可能设置有问题", ex.Message);
                    FilterList.Clear();
                    pAppFrm.MainForm.Invoke(new ClearCheck(Clear), new object[] { pAppFrm });
                    pAppFrm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { pAppFrm, false });
                    return;
                }
            }
            FilterList.Clear();
            pAppFrm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { pAppFrm, false });

            //将接边检查结果列表内容邦定到界面控件上
            //pAppFrm.MainForm.Invoke(new DeleteBindCilck(BindClick), new object[] { table, pAppHk });

            pAppHk.CurrentThread = null;
            //选中检查结果项
            pAppFrm.MainForm.Invoke(new SelectDataCheckGrid(SelectCheckGrid), pAppHk);
            pAppFrm.OperatorTips = "检查完成！";
            pAppFrm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "提示", "检查完毕!" });
        }

        /// <summary>
        ///  做图幅某条边上的接边检查
        /// </summary>
        /// <param name="pFeature">图幅要素</param>
        /// <param name="pEdgeGeometry">图幅边缓冲与图幅求交范围</param>
        /// <param name="listFeatures">接边检查图层</param>
        /// <param name="listFeatures">接边检查图层要素</param>
        /// <param name="table">接边检查结果列表</param>
        /// <param name="pAppFrm">主应用程序用以获取进度条显示进度</param>
        /// <param name="distance">点缓冲半径</param>
        private void DoJoinCheckByEdge(IFeature pFeature, IGeometry pEdgeGeometry, ILayer pCheckLay, List<IFeature> listFeatures,  Plugin.Application.IAppFormRef pAppFrm, double distance, List<string> tempAttribute)
        {
            int intcnt = pAppFrm.ProgressBar.Value;
            foreach (IFeature pFeat in listFeatures)
            {
                intcnt++;
                pAppFrm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { pAppFrm.ProgressBar, -1, -1, intcnt });

                //获取要素位于范围内的节点（含点在范围边界上）
                List<IPoint> listPoints = GetFeatPointsBufferByGeometry(pFeat, pEdgeGeometry);
                if (listPoints == null) continue;


                foreach (IPoint pTempPnt in listPoints)
                {
                    IGeometry pTempGeo = null;
                    try
                    {
                        //以点为中心得到缓冲范围,并与图幅求差
                        ITopologicalOperator pTop = pTempPnt as ITopologicalOperator;
                        pTempGeo = pTop.Buffer(distance);
                        pTop = pTempGeo as ITopologicalOperator;
                        pTempGeo = pTop.Difference(pFeature.Shape);
                        pTop = pTempGeo as ITopologicalOperator;
                        pTop.Simplify(); //提高查询效率
                    }
                    catch (Exception ex)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("容差可能设置有问题", ex.Message);
                        return;
                    }

                    //获取接边属性限制条件
                    string strCon = GetJoinCheckCon(pFeat, tempAttribute);
                    //获取同层范围内的相同属性条件的要素
                    List<IFeature> pObjFeats = GetFeaturesByGeometry(pCheckLay, pTempGeo, strCon);
                    if (pObjFeats == null)
                    {
                        //System.Windows.Forms.MessageBox.Show("");
                    }

                    if (pObjFeats == null) continue;

                    IFeature pObjFeature = pObjFeats[0];
                    //符合与要素接边的要素可能为多条应取距离该点最近的要素
                    if (pObjFeats.Count > 1)
                    {
                        //...........................
                        int cntFeat = 0;
                        double minDistance = 0;
                        foreach (IFeature pTempFeat in pObjFeats)
                        {
                            cntFeat++;
                            IProximityOperator pProximityOperator = pTempPnt as IProximityOperator;
                            double pTemp = pProximityOperator.ReturnDistance(pTempFeat.Shape);
                            if (cntFeat == 1)
                            {
                                minDistance = pTemp;
                                pObjFeature = pTempFeat;
                            }
                            else if (minDistance > pTemp)
                            {
                                minDistance = pTemp;
                                pObjFeature = pTempFeat;
                            }
                        }
                    }

                    //检查两要素在节点pTempPnt上是否接边上
                    if (IsJoinedOn(pFeat, pTempPnt, pObjFeature) == false)
                    {
                        //为未接边要素输出
                        //.............................
                        //string temp = pCheckLay.Name + "：" + pFeat.OID.ToString() + pObjFeature.OID.ToString();
                        //string temp_1 = pCheckLay.Name + "：" + pObjFeature.OID.ToString() + pFeat.OID.ToString();
                        //DataRow row = table.NewRow();
                        //row[0] = pCheckLay.Name + "：" + pFeat.OID.ToString();
                        //row[1] = pObjFeature.OID.ToString();
                        //row[2] = pTempPnt;
                        //if (!FilterList.Contains(temp) && !FilterList.Contains(temp_1))
                        //{
                        //FilterList.Add(temp);
                        //table.Rows.Add(row);
                        //}
                        string mFeaClsName = (((pCheckLay as IFeatureLayer).FeatureClass) as IDataset).Name;
                        #region 将错误结果保存起来
                        //保存错误结果
                        double pMapx = pTempPnt.X;
                        double pMapy = pTempPnt.Y;

                        List<object> ErrorLst = new List<object>();
                        ErrorLst.Add("接边检查");//功能组名称
                        ErrorLst.Add("接边检查");//功能名称
                        ErrorLst.Add("");  //数据文件名
                        ErrorLst.Add(enumErrorType.接边检查.GetHashCode());//错误id;
                        ErrorLst.Add("接边检查错误");//错误描述
                        ErrorLst.Add(pMapx);    //...
                        ErrorLst.Add(pMapy);    //...
                        ErrorLst.Add(mFeaClsName);
                        ErrorLst.Add(pFeat.OID);
                        ErrorLst.Add(mFeaClsName);
                        ErrorLst.Add(pObjFeature.OID);
                        ErrorLst.Add(false);
                        ErrorLst.Add(System.DateTime.Now.ToString());

                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(hook.DataCheckGrid as object, dataErrTreatEvent);
                        #endregion

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pObjFeature);
                    }
                }
                
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeat);
            }
        }

        #region 进程与界面控件响应实现


        //弹出提示对话框
        private delegate void ShowForm(string strCaption, string strText);
        private void ShowErrForm(string strCaption, string strText)
        {
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle(strCaption, strText);
        }

        //选中树图节点
        private delegate void ChangeSelectNode(DevComponents.AdvTree.AdvTree aTree, DevComponents.AdvTree.Node aNode);
        private void ChangeTreeSelectNode(DevComponents.AdvTree.AdvTree aTree, DevComponents.AdvTree.Node aNode)
        {

            aTree.SelectedNode = aNode;
            if (aNode.PrevNode != null)
            {
                aNode.PrevNode.CollapseAll();
            }
            aNode.Expand();
            aTree.Refresh();
        }

        //选中检查结果列表
        private delegate void SelectDataCheckGrid(Plugin.Application.IAppGISRef pApp);
        private void SelectCheckGrid(Plugin.Application.IAppGISRef pApp)
        {
            DevComponents.DotNetBar.PanelDockContainer PanelTip = pApp.DataCheckGrid.Parent as DevComponents.DotNetBar.PanelDockContainer;
            if (PanelTip != null)
            {
                PanelTip.DockContainerItem.Selected = true;
            }
        }

        //改变进度条
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

        //控制进度条显示
        private delegate void ShowProgress(Plugin.Application.IAppFormRef pAppFrm, bool bVisible);
        private void ShowProgressBar(Plugin.Application.IAppFormRef pAppFrm, bool bVisible)
        {
            if (bVisible == true)
            {
                pAppFrm.ProgressBar.Visible = true;
            }
            else
            {
                pAppFrm.ProgressBar.Visible = false;
            }
        }

        //改变状态栏提示内容
        private delegate void ShowTips(Plugin.Application.IAppFormRef pAppFrm, string strText);
        private void ShowStatusTips(Plugin.Application.IAppFormRef pAppFrm, string strText)
        {
            pAppFrm.OperatorTips = strText;
        }
        #endregion

        #region  接边检查公共函数


        /// <summary>
        /// 从mapcontrol上获取图幅结合表
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="strName"></param>
        /// <returns></returns>
        public ILayer GetMapFrameLayer(IMap pMap, string strName, string C_name)
        {

            for (int i = 0; i < pMap.LayerCount; i++)
            {
                ILayer pLayer = pMap.get_Layer(i);
                IGroupLayer pGroupLayer = pLayer as IGroupLayer;
                if (pGroupLayer == null) continue;
                if (pGroupLayer.Name != strName) continue;
                ICompositeLayer pCompositeLayer = pGroupLayer as ICompositeLayer;
                if (pCompositeLayer.Count == 0) return null;
                ILayer layer = null;
                for (int n = 0; n < pCompositeLayer.Count; n++)
                {
                    layer = pCompositeLayer.get_Layer(n);
                    if (layer.Name == C_name) break;

                }
                return layer;
            }
            return null;
        }

        /// <summary>
        /// 获取图幅的某边缓冲后范围
        /// </summary>
        /// <param name="pGeometry">一个图幅图形</param>
        /// <param name="distance">缓冲半径</param>
        /// <param name="state">方向：1-左，2-右，3-上，4-下</param>
        /// <returns></returns>
        public IGeometry GetBufferGeometryByMapFrame(IGeometry pGeometry, double distance, int state)
        {
            IEnvelope pEnvelope = pGeometry.Envelope;
            IPolyline polyline = new PolylineClass();

            switch (state)
            {
                case 1:      //左边
                    polyline.FromPoint = pEnvelope.UpperLeft;
                    polyline.ToPoint = pEnvelope.LowerLeft;
                    break;
                case 2:     //右边
                    polyline.FromPoint = pEnvelope.UpperRight;
                    polyline.ToPoint = pEnvelope.LowerRight;
                    break;
                case 3:     //上边
                    polyline.FromPoint = pEnvelope.UpperLeft;
                    polyline.ToPoint = pEnvelope.UpperRight;
                    break;
                case 4:     //下边
                    polyline.FromPoint = pEnvelope.LowerLeft;
                    polyline.ToPoint = pEnvelope.LowerRight;
                    break;
                default:
                    return null;
            }

            ITopologicalOperator topo = polyline as ITopologicalOperator;
            IGeometry geo = topo.Buffer(distance);
            topo = geo as ITopologicalOperator;
            topo.Simplify();
            return geo;

        }

        /// <summary>
        /// 获取图层位于范围内的要素(包含相交不包含接触)
        /// </summary>
        /// <param name="pLay">图层</param>
        /// <param name="pGeometry">范围条件</param>
        /// <returns></returns>
        public List<IFeature> GetFeaturesByGeometry(ILayer pLay, IGeometry pGeometry)
        {
            IFeatureLayer pFeatLay = pLay as IFeatureLayer;
            if (pFeatLay == null) return null;

            IFeatureClass pFeatCls = pFeatLay.FeatureClass;

            //范围内缩,以去除关系为接触的要素 
            //double xdbl = pGeometry.Envelope.XMax - pGeometry.Envelope.XMin;
            //double ydbl = pGeometry.Envelope.YMax - pGeometry.Envelope.YMin;
            //double distance = xdbl;
            //if (xdbl > ydbl)
            //{
            //    distance = ydbl;
            //}
            //ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            ////IGeometry pTempGeo = pTop.Buffer(0 - distance / 10);
            //IGeometry pTempGeo = pTop.Buffer(-0.5);
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = pGeometry;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSpatialFilter.GeometryField = "SHAPE";

            List<IFeature> listFeats = new List<IFeature>();
            IFeatureCursor pFeatCur = pFeatCls.Search(pSpatialFilter, false);
            IFeature pFeat = pFeatCur.NextFeature();
            while (pFeat != null)
            {
                listFeats.Add(pFeat);
                pFeat = pFeatCur.NextFeature();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatCur);
            return listFeats;
        }

        /// <summary>
        /// 获取图层范围内的相同属性条件的要素(包含相交不包含接触)
        /// </summary>
        /// <param name="listLays">图层</param>
        /// <param name="pGeometry">范围条件</param>
        /// <param name="pGeometry">属性条件</param>
        /// <returns></returns>
        public List<IFeature> GetFeaturesByGeometry(ILayer pLay, IGeometry pGeometry, string strCon)
        {
            try
            {
                IFeatureLayer pFeatLay = pLay as IFeatureLayer;
                if (pFeatLay == null) return null;
                IFeatureClass pFeatCls = pFeatLay.FeatureClass;

                //范围内缩,以去除关系为接触的要素 
                double xdbl = pGeometry.Envelope.XMax - pGeometry.Envelope.XMin;
                double ydbl = pGeometry.Envelope.YMax - pGeometry.Envelope.YMin;
                double distance = xdbl;
                if (xdbl > ydbl)
                {
                    distance = ydbl;
                }
                ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
                IGeometry pTempGeo = pTop.Buffer(0 - distance / 10);

                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pTempGeo;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                pSpatialFilter.GeometryField = "SHAPE";

                IQueryFilter pQueryFilter = pSpatialFilter as IQueryFilter;
                pQueryFilter.WhereClause = strCon;

                List<IFeature> listValue = new List<IFeature>();
                IFeatureCursor pFeatCur = pFeatCls.Search(pSpatialFilter, false);
                IFeature pFeat = pFeatCur.NextFeature();
                while (pFeat != null)
                {
                    listValue.Add(pFeat);
                    pFeat = pFeatCur.NextFeature();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatCur);

                if (listValue.Count == 0) return null;
                return listValue;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取要素位于范围内的节点（含点在范围边界上）
        /// </summary>
        /// <param name="pFeat">要素</param>
        /// <param name="pGeometry">范围</param>
        /// <returns>返回要素上的节点（位于范围内或在范围边界上）</returns>
        public List<IPoint> GetFeatPointsBufferByGeometry(IFeature pFeat, IGeometry pGeometry)
        {
            List<IPoint> listPoints = new List<IPoint>();
            IPointCollection pPointCollection = pFeat.Shape as IPointCollection;
            //范围外扩,以获取关系为在范围边界上的点
            double xdbl = pGeometry.Envelope.XMax - pGeometry.Envelope.XMin;
            double ydbl = pGeometry.Envelope.YMax - pGeometry.Envelope.YMin;
            double distance = xdbl;
            if (xdbl > ydbl)
            {
                distance = ydbl;
            }
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            //IGeometry pTempGeo = pTop.Buffer(distance / 100);
            IGeometry pTempGeo = pTop.Buffer(1);//初始给定一个默认值1
            IRelationalOperator pRelation = pTempGeo as IRelationalOperator;
            if (pPointCollection == null || pRelation == null) return null;

            int cnt = pPointCollection.PointCount;
            //如果是面则应去除尾点
            if (pFeat.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                cnt = pPointCollection.PointCount - 1;
            }

            for (int i = 0; i < cnt; i++)
            {
                IGeometry geo = pPointCollection.get_Point(i) as IGeometry;
                if (pRelation.Contains(geo))
                {
                    listPoints.Add(pPointCollection.get_Point(i));
                }
            }

            if (listPoints.Count == 0) return null;
            return listPoints;
        }

        /// <summary>
        /// 获取接边属性条件
        /// </summary>
        /// <param name="pFeat">接边参照要素</param>
        /// <param name="listFields">接边参照字段</param>
        /// <returns></returns>
        public string GetJoinCheckCon(IFeature pFeat, List<string> listFields)
        {
            if (pFeat == null || listFields == null) return "";
            if (listFields.Count == 0) return "";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < pFeat.Fields.FieldCount; i++)
            {
                IField pField = pFeat.Fields.get_Field(i);
                if (!listFields.Contains(pField.Name)) continue;

                if (sb.Length != 0)
                {
                    sb.Append(" and ");
                }

                switch (pField.Type)
                {
                    case esriFieldType.esriFieldTypeDate:
                        sb.Append(pField.Name + "=#" + pFeat.get_Value(i).ToString() + "#");
                        break;
                    case esriFieldType.esriFieldTypeString:
                        sb.Append(pField.Name + "='" + pFeat.get_Value(i).ToString() + "'");
                        break;
                    default:
                        sb.Append(pField.Name + "=" + pFeat.get_Value(i).ToString());
                        break;
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// 检查两要素在节点pPnt上是否接边上
        /// </summary>
        /// <param name="pPnt"> 源要素</param>
        /// <param name="pPnt"> 源要素上节点</param>
        /// <param name="pFeature">目标要素</param>
        /// <returns></returns>
        private bool IsJoinedOn(IFeature pOrgFeature, IPoint pPnt, IFeature pObjFeature)
        {
            //目标要素距离源要素节点最近的点的坐标与要素节点坐标一致
            IProximityOperator pProximity = pObjFeature.Shape as IProximityOperator;
            IPoint pNearestPnt = pProximity.ReturnNearestPoint(pPnt, esriSegmentExtension.esriNoExtension);
            if (pPnt.X == pNearestPnt.X && pPnt.Y == pNearestPnt.Y)
            {
                return true;
            }
            else
            {
                //如果是面则还应判断目标要素距离源要素节点最近的点是否在源要素上
                if (pOrgFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    pProximity = pOrgFeature.Shape as IProximityOperator;
                    IPoint pNearestPntTemp = pProximity.ReturnNearestPoint(pNearestPnt, esriSegmentExtension.esriNoExtension);
                    if (pNearestPntTemp.X == pNearestPnt.X && pNearestPntTemp.Y == pNearestPnt.Y) return true;
                }

                return false;
            }
        }

        #endregion

        #endregion

        //处理检查结果
        public void GeoDataChecker_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            //用户界面上表现出错误信息
            DevComponents.DotNetBar.Controls.DataGridViewX pDataGrid = sender as DevComponents.DotNetBar.Controls.DataGridViewX;
            if (_ResultTable == null) return;
            DataRow newRow = _ResultTable.NewRow();
            newRow["检查功能名"] = e.ErrInfo.FunctionName;
            newRow["错误类型"] = GeoDataChecker.GetErrorTypeString(Enum.Parse(typeof(enumErrorType), e.ErrInfo.ErrID.ToString()).ToString());
            newRow["错误描述"] = e.ErrInfo.ErrDescription;
            newRow["数据图层1"] = e.ErrInfo.OriginClassName;
            newRow["要素OID1"] = e.ErrInfo.OriginFeatOID;
            newRow["数据图层2"] = e.ErrInfo.DestinationClassName;
            newRow["要素OID2"] = e.ErrInfo.DestinationFeatOID;
            newRow["检查时间"] = e.ErrInfo.OperatorTime;
            newRow["定位点X"] = e.ErrInfo.MapX;
            newRow["定位点Y"] = e.ErrInfo.MapY;
            newRow["数据文件名"] = e.ErrInfo.DataFileName;
            _ResultTable.Rows.Add(newRow);

            pDataGrid.Update();

            //将结果输出excle

            InsertRowToExcel(e);
        }

        //将数据检查结果存入Excel中 
        private void InsertRowToExcel(DataErrTreatEvent e)
        {
            if (_ErrDbCon != null && _ErrTableName != "")
            {
                SysCommon.DataBase.SysTable sysTable = new SysCommon.DataBase.SysTable();
                sysTable.DbConn = _ErrDbCon;
                sysTable.DBConType = SysCommon.enumDBConType.OLEDB;
                sysTable.DBType = SysCommon.enumDBType.ACCESS;
                string sql = "insert into " +_ErrTableName +
                    "(数据文件路径,检查功能名,错误类型,错误描述,数据图层1,数据OID1,数据图层2,数据OID2,定位点X,定位点Y,检查时间) values(" +
                    "'" + e.ErrInfo.DataFileName + "','" + e.ErrInfo.FunctionName + "','" + GeoDataChecker.GetErrorTypeString(Enum.Parse(typeof(enumErrorType), e.ErrInfo.ErrID.ToString()).ToString()) + "','" + e.ErrInfo.ErrDescription + "','" + e.ErrInfo.OriginClassName + "','" + e.ErrInfo.OriginFeatOID.ToString() + "','" +
                    e.ErrInfo.DestinationClassName + "','" + e.ErrInfo.DestinationFeatOID.ToString() + "'," + e.ErrInfo.MapX + "," + e.ErrInfo.MapY + ",'" + e.ErrInfo.OperatorTime + "')";

                Exception errEx = null;
                sysTable.UpdateTable(sql, out errEx);
                if (errEx != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "写入Excel文件出错！");
                    return;
                }
            }
        }



        #region 处理树图相关函数
        //创建处理树图
        public void IntialTree(DevComponents.AdvTree.AdvTree aTree)
        {
            DevComponents.AdvTree.ColumnHeader aColumnHeader;
            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "FCName";
            aColumnHeader.Text = "图层名";
            aColumnHeader.Width.Relative = 50;
            aTree.Columns.Add(aColumnHeader);

            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "NodeRes";
            aColumnHeader.Text = "结果";
            aColumnHeader.Width.Relative = 45;
            aTree.Columns.Add(aColumnHeader);
        }
        //设置选中树节点颜色
       public void setNodeColor(DevComponents.AdvTree.AdvTree aTree)
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
        public  DevComponents.AdvTree.Node CreateAdvTreeNode(DevComponents.AdvTree.NodeCollection nodeCol, string strText, string strName, Image pImage, bool bExpand)
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
            //添加树图列节点
            DevComponents.AdvTree.Cell aCell = new DevComponents.AdvTree.Cell();
            aCell.Images.Image = null;
            node.Cells.Add(aCell);
            nodeCol.Add(node);
            return node;
        }

        //添加树图节点列
        public DevComponents.AdvTree.Cell CreateAdvTreeCell(DevComponents.AdvTree.Node aNode, string strText, Image pImage)
        {
            DevComponents.AdvTree.Cell aCell = new DevComponents.AdvTree.Cell(strText);
            aCell.Images.Image = pImage;
            aNode.Cells.Add(aCell);

            return aCell;
        }

        //为数据处理树图节点添加处理结果状态
        public void ChangeTreeSelectNode(DevComponents.AdvTree.Node aNode, string strRes, bool bClear)
        {
            if (aNode == null)
            {
                hook.DataTree.SelectedNode = null;
                hook.DataTree.Refresh();
                return;
            }

            hook.DataTree.SelectedNode = aNode;
            if (bClear)
            {
                hook.DataTree.SelectedNode.Nodes.Clear();
            }
            hook.DataTree.SelectedNode.Cells[1].Text = strRes;
            hook.DataTree.Refresh();
        }
        #endregion

        #region 进度条显示
        //控制进度条显示
        public void ShowProgressBar(bool bVisible)
        {
            if (bVisible == true)
            {
                (hook as Plugin.Application.IAppFormRef).ProgressBar.Visible = true;
            }
            else
            {
                (hook as Plugin.Application.IAppFormRef).ProgressBar.Visible = false;
            }
        }
        //修改进度条
        private void ChangeProgressBar1(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value)
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
        private void ShowStatusTips(string strText)
        {
            (hook as Plugin.Application.IAppFormRef).OperatorTips = strText;
        }
        #endregion
    }
}
