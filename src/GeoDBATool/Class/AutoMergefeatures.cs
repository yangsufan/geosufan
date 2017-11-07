using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Collections;

namespace GeoDBATool
{
    /// <summary>
    /// 融合提交事件委托
    /// </summary>
    /// <param name="FeatureClassName">进行融合的要素类名称</param>
    /// <param name="目标要素ID">融合后的目标要素OID</param>
    /// <param name="SourOID">被融合的要素OID（该OID将消失）</param>
    public delegate void AutoMergefeaturesCommitEventHander(string FeatureClassName, int DesOID,int SourOID);

    /// <summary>
    /// 融合错误事件委托
    /// </summary>
    /// <param name="ErrorString">错误消息文本</param>
    public delegate void AutoMergefeaturesReturnErr(string ErrorString);

    /// <summary>
    /// 自动融合类（目前只针对GDB数据）
    /// </summary>
    public class AutoMergefeatures
    {

        private IFeatureWorkspace _DesWorkspace=null;
        private FeatureSearcheProperties[] _SearchInfo=null;
        /// <summary>
        /// 是否处理更新日志表
        /// </summary>
        private bool _TreatUpdateLog = false;

        /// <summary>
        /// 初始化融合参数
        /// </summary>
        /// <param name="SearchInfo">要素搜索信息（图层，容差，字段等等）</param>
        /// <param name="DesWorkspace">需要进行融合的目标库体</param>
        public AutoMergefeatures(FeatureSearcheProperties[] SearchInfo,IFeatureWorkspace DesWorkspace)
        {
            ///如果融合目标不为空
            if (DesWorkspace != null && SearchInfo != null)
            {
                this._DesWorkspace=DesWorkspace;
                this._SearchInfo=SearchInfo;
            }
        }

        public AutoMergefeatures(FeatureSearcheProperties[] SearchInfo, IFeatureWorkspace DesWorkspace,bool TreatUpdateLog)
        {
            ///如果融合目标不为空
            if (DesWorkspace != null && SearchInfo != null)
            {
                this._DesWorkspace = DesWorkspace;
                this._SearchInfo = SearchInfo;
                this._TreatUpdateLog = TreatUpdateLog;
            }
        }

        /// <summary>
        /// 执行融合操作
        /// </summary>
        /// <param name="_TreatUpdateLog">是否进行更新日志表的联动操作</param>
        public void ExcuteMerge(bool _TreatUpdateLog)
        {
            if (!_TreatUpdateLog)  ///不操作更新日志表（批量更新中的处理）
            {
                ///如果资源有效，则进行融合操作
                if (this._DesWorkspace != null && this._SearchInfo != null)
                {
                    try
                    {
                        foreach (FeatureSearcheProperties FeatureSearcheProperty in this._SearchInfo)
                        {
                            ///获得需要操作的要素类
                            string pFeatureClassName = FeatureSearcheProperty.FeatureClass;
                            IFeatureClass pFeatureClass = this._DesWorkspace.OpenFeatureClass(pFeatureClassName);

                            if (pFeatureClass != null)
                            {
                                IFields pFields = FeatureSearcheProperty.CompareFields;
                                if (FeatureSearcheProperty.SpatialRange != null)
                                {
                                    ///获取参与融合的要素游标
                                    ///
                                    IFeatureCursor pFeatureCursor = GetFeatureCursor4Merge(pFeatureClass, FeatureSearcheProperty.SpatialRange, FeatureSearcheProperty.SearchBuffer);
                                    Dictionary<int, List<int>> TouchedGroup = GetTouchedGroup(pFeatureClass,pFeatureCursor, FeatureSearcheProperty.CompareFields);

                                    //释放cursor
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                                }
                                else
                                {
                                    this.ErrOcur("融合范围获取失败!");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //*******************************************************************
                        //异常日志
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

                        this.ErrOcur(e.Message);
                        
                    }
                }
            }
            else  ///操作更新日志表中的相关记录
            {
                ///如果资源有效，则进行融合操作
                if (this._DesWorkspace != null && this._SearchInfo != null)
                {

                }
            }
        }

        /// <summary>
        /// 遍历游标中的要素获取空间关系为Touch，字段相同的要素组成一组
        /// </summary>
        /// <param name="pFeatureCursor">要素游标</param>
        /// <param name="iFields">字段组</param>
        ///  <param name="iFields">目标要素类</param>
        /// <returns>返回需要融合的要素组合，key为融合后要素oid，value为被融合要素oid</returns>
        private Dictionary<int, List<int>> GetTouchedGroup(IFeatureClass DesFLC , IFeatureCursor pFeatureCursor, IFields CompareFields)
        {

            Dictionary<int, List<int>> pTouchedgroup = new Dictionary<int, List<int>>();
            IFeature pSourceFeature = pFeatureCursor.NextFeature();

            while (pSourceFeature != null)
            {
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                try
                {
                    ///空间关系设置
                    ///
                    pSpatialFilter.GeometryField = "SHAPE";
                    pSpatialFilter.Geometry = pSourceFeature.Shape;
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;

                    ///属性条件设置
                    ///
                    string pWhereClause = "";

                    if (CompareFields != null) ///有字段比对条件
                    {
                        ///遍历字段形成查询语句
                        for (int i = 0; i < CompareFields.FieldCount; i++)
                        {
                            IField pField = CompareFields.get_Field(i);

                            if (pWhereClause == "")///如果是第一个字段
                            {
                                switch (pField.Type)
                                {
                                    case esriFieldType.esriFieldTypeString:

                                        pWhereClause = pField.Name + " = '" + pSourceFeature.get_Value(pSourceFeature.Fields.FindField(pField.Name)) + "'";
                                        break;
                                    default:
                                        pWhereClause = pField.Name + " = " + pSourceFeature.get_Value(pSourceFeature.Fields.FindField(pField.Name));
                                        break;
                                }

                            }
                            else///如果不是第一个字段
                            {
                                switch (pField.Type)
                                {
                                    case esriFieldType.esriFieldTypeString:

                                        pWhereClause = pWhereClause + " and " + pField.Name + " = '" + pSourceFeature.get_Value(pSourceFeature.Fields.FindField(pField.Name)) + "'";
                                        break;
                                    default:
                                        pWhereClause = pWhereClause + " and " + pField.Name + " = " + pSourceFeature.get_Value(pSourceFeature.Fields.FindField(pField.Name));
                                        break;
                                }
                            }
                        }
                    }

                    ///填充pTouchedgroup变量
                    ///
                    IFeatureCursor pMatchedFeatureCursor = DesFLC.Search(pSpatialFilter, false);
                    if (pMatchedFeatureCursor!=null)
                    {
                        IFeature pMatchedFeature = pMatchedFeatureCursor.NextFeature();
                        List<int> MatchedFeatures = new List<int>();

                        while (pMatchedFeature != null)
                        {
                            if (!pTouchedgroup.ContainsKey(pSourceFeature.OID))
                            {
                                MatchedFeatures.Add(pMatchedFeature.OID);
                            }
                            else
                            {
                                break;
                            }
                            pMatchedFeature = pMatchedFeatureCursor.NextFeature();
                        }

                        if (!pTouchedgroup.ContainsKey(pSourceFeature.OID))
                        {
                            pTouchedgroup.Add(pSourceFeature.OID, MatchedFeatures);
                        }
                    }

                    //释放cursor
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pMatchedFeatureCursor);

                    pTouchedgroup = Regroup(pTouchedgroup);
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

                    this.ErrOcur(e.Message);
                    return null;
                    
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pSpatialFilter);
                }

                pSourceFeature = pFeatureCursor.NextFeature();
            }

            return pTouchedgroup;
        }

        /// <summary>
        /// 重组匹配的要素列表，以配置融合操作
        /// </summary>
        /// <param name="pTouchedgroup">原始要素列表</param>
        /// <returns></returns>
        private Dictionary<int, List<int>> Regroup(Dictionary<int, List<int>> pTouchedgroup)
        {
            Dictionary<int, List<int>> AfterRegroup = new Dictionary<int, List<int>>();
            

            foreach (KeyValuePair<int,List<int>> Item in pTouchedgroup)
            {
                List<int> pList = Item.Value as List<int>;
                List<int> pHash = new List<int>();

                //if (!pHash.Contains(Item.Key))
                //{
                //    pHash.Add(Item.Key);
                //}

                Recursive(pTouchedgroup, pHash, pList);

                if (pHash.Contains(Item.Key))
                {
                    pHash.Remove(Item.Key);
                }

                AfterRegroup.Add(Item.Key, pHash);
            }
            return AfterRegroup;
        }

        /// <summary>
        /// 递归读取列表中的值
        /// </summary>
        /// <param name="pTouchedgroup"></param>
        /// <param name="pHash"></param>
        /// <param name="pList"></param>
        private void Recursive(Dictionary<int, List<int>> pTouchedgroup, List<int> pHash, List<int> pList)
        {
            foreach (int var in pList)
            {
                ///如果Item的值存在于集合的Keys中
                if (pTouchedgroup.ContainsKey(var))
                {
                    Recursive(pTouchedgroup, pHash, pTouchedgroup[var]);
                    pTouchedgroup.Remove(var);
                }

                if (!pHash.Contains(var))
                {
                    pHash.Add(var);
                }

            }
        }


        /// <summary>
        /// 获取参与融合的要素游标
        /// </summary>
        /// <param name="pFeatureClass">进行融合的要素类</param>
        /// <param name="pRangePolygon">融合搜索的范围</param>
        ///  <param name="pSearchBuffer">融合搜索缓冲半径</param>
        /// <returns></returns>
        private IFeatureCursor GetFeatureCursor4Merge(IFeatureClass pFeatureClass, IGeometry pRangePolygon, double pSearchBuffer)
        {

            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            try
            {
                ITopologicalOperator pTopologicalOperator = pRangePolygon as ITopologicalOperator;
                IGeometry Boundery = pTopologicalOperator.Boundary;
                pTopologicalOperator = Boundery as ITopologicalOperator;

                IGeometry Buffered = pTopologicalOperator.Buffer(pSearchBuffer);

                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.Geometry = Buffered;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                IFeatureCursor pFeatureCursor = pFeatureClass.Search(pSpatialFilter, false);
                if (pFeatureCursor != null)
                {
                    return pFeatureCursor;
                }
                else
                {
                    return null;
                }
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
                
                this.ErrOcur(e.Message);
                return null;
            }
            finally
            {
                
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pSpatialFilter);
            }
           
        }

        /// <summary>
        /// 定义融合要素事件
        /// </summary>
        //public event AutoMergefeaturesCommitEventHander CommitMerge;

        public event AutoMergefeaturesReturnErr ErrOcur;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this._DesWorkspace = null;
            this._SearchInfo = null;
        }

        /// <summary>
        /// 要素融合 陈亚飞添加
        /// </summary>
        /// <param name="pFeatureClass">需要融合的图层</param>
        /// <param name="newOID">融合后保存的要素OID</param>
        /// <param name="oldOIDLst">需要融合的要素OID</param>
        private  void MergeFeatures(IFeatureClass pFeatureClass, int newOID, List<int> oldOIDLst)
        {
            IGeometry tempGeo = null;
            for (int i = 0; i < oldOIDLst.Count; i++)
            {
                int oldOID = oldOIDLst[i];
                IFeature pFeature = pFeatureClass.GetFeature(oldOID);
                if (tempGeo != null)
                {
                    ITopologicalOperator pTop = tempGeo as ITopologicalOperator;
                    tempGeo = pTop.Union(pFeature.Shape);
                    //融合后将图形简单化
                    pTop = tempGeo as ITopologicalOperator;
                    pTop.Simplify();
                }
                else
                {
                    tempGeo = pFeature.Shape;
                }
            }

            IFeature newFea = pFeatureClass.GetFeature(newOID);
            //将融合后的图形赋值给新的要素
            newFea.Shape = tempGeo;

            //将新生成的要素存储
            newFea.Store();

            //融合后删除被融合的要素
            for (int j = 0; j < oldOIDLst.Count; j++)
            {
                if (oldOIDLst[j] != newOID)
                {
                    IFeature delFeature = pFeatureClass.GetFeature(oldOIDLst[j]);
                    delFeature.Delete();
                }
            }
        }
    }

    /// <summary>
    /// 设置要素搜索参数
    /// </summary>
    public class FeatureSearcheProperties 
    {

        private IGeometry _SpatialRange = null;
        private string _FeatureClass = null;
        private IFields _CompareFields = null;
        private double  _SearchBuffer = 0;

        /// <summary>
        /// 初始化类成员
        /// </summary>
        public FeatureSearcheProperties()
        { 

        }

        /// <summary>
        /// 初始化类成员(带参数)
        /// </summary>
        public FeatureSearcheProperties(IGeometry SpatialRange, string FeatureClass, IFields CompareFields)
        {
            this._SpatialRange = SpatialRange;
            this._FeatureClass = FeatureClass;
            this._CompareFields = CompareFields;
                
        }
        /// <summary>
        /// 获取融合搜索的边界范围
        /// </summary>
        public IGeometry SpatialRange
        {
            get
            {
                return this._SpatialRange;
            }
            set
            {
                this._SpatialRange = value;
            }
        }

        /// <summary>
        /// 获取需要进行融合的要素类
        /// </summary>
        public string FeatureClass
        {
            get
            {
                return this._FeatureClass;
            }
            set
            {
                this._FeatureClass = value;
            }
        }

        /// <summary>
        /// 参与融合比较的属性字段
        /// </summary>
        public IFields CompareFields
        {
            get
            {
                return this._CompareFields;
            }
            set
            {
                this._CompareFields = value;
            }
        }

        /// <summary>
        /// 搜索容差
        /// </summary>
        public double  SearchBuffer
        {
            get
            {
                return this._SearchBuffer;
            }
            set
            {
                this._SearchBuffer = value;
            }
        }
    }


}
