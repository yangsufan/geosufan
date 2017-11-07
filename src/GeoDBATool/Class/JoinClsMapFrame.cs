using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;


using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

using System.Xml;
namespace GeoDBATool
{
    /*
     * ClsMapFrame(标准图幅)类实现了IMapframe （接边图幅）接口
     * 获取的源接边要素缓冲区（图幅内）和目标要素缓冲区（图幅外）是一标准图幅的左、下边界的缓冲区分别与图幅范围求交、求差的范围
     * 设置当前接边图幅可使用GetOriFrame方法通过图幅号字段名与值进行设置，也可通过OriMapFrame属性直接赋值
     * Getborderline（获取接边边界）的方法实现的是获取标准图幅的左边界、下边界（不适用于不规则范围接边） 
     * 
    */
    class ClsMapFrame : IMapframe
    {
        public ClsMapFrame()////构造函数
        {
            this._mapframefea = null;
            this._OriMapFrame = null;
        }
        private IFeatureClass _mapframefea;
        public IFeatureClass MapFrameFea
        {
            get { return _mapframefea; }
            set { this._mapframefea = value; }
        }
        private IFeature _OriMapFrame;
        public IFeature OriMapFrame
        {
            get { return this._OriMapFrame; }
            set { this._OriMapFrame = value; }
        }
        public IFeature GetOriFrame(string OriFrameNoValue, string OriFrameNoField)
        {
            IFeature Fea = this.GetFrame(OriFrameNoValue, OriFrameNoField);
            _OriMapFrame = Fea;
            return Fea;
        }
        public void GetBufferArea(double dBufferRadius, out IGeometry OriBufferArea, out IGeometry DesBufferArea)
        {
            OriBufferArea = null;
            DesBufferArea = null;
            if (null == this._OriMapFrame)
                return;
            ////获取源图幅的左边界、下边界的缓冲区域
            try
            {
                IGeometry LBufferArea = GetBufferGeometryByMapFrame(_OriMapFrame.Shape, dBufferRadius, 1);
                IGeometry DBufferArea = GetBufferGeometryByMapFrame(_OriMapFrame.Shape, dBufferRadius, 4);
                ITopologicalOperator topo = LBufferArea as ITopologicalOperator;
                IGeometry UnionArea = topo.Union(DBufferArea);
                ////获取源图幅、目标图幅的接边缓冲区
                topo = UnionArea as ITopologicalOperator;
                OriBufferArea = topo.Intersect((IGeometry)_OriMapFrame.Shape, esriGeometryDimension.esriGeometry2Dimension);
                DesBufferArea = topo.Difference((IGeometry)_OriMapFrame.Shape);
            }
            catch (Exception Error)
            {
                throw Error;
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(Error, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(Error, null, DateTime.Now);
                }
                //********************************************************************
            }
            //OriBufferArea = UnionArea;
            //DesBufferArea = UnionArea;

        }
        private IFeature GetFrame(string OriFrameNoValue, string OriFrameNoField)
        {
            if (null == this._mapframefea)
                return null;
            else
            {
                try
                {
                    IFeatureCursor FeatureCursor = _mapframefea.Search(null, false);
                    IFeature fea = FeatureCursor.NextFeature();
                    while (null != fea)
                    {
                        int index = -1;
                        index = fea.Fields.FindField(OriFrameNoField);
                        if (index > 0)
                        {
                            string MapFrameno = fea.get_Value(index).ToString();
                            if (OriFrameNoValue == MapFrameno)
                                return fea;
                        }
                        fea = FeatureCursor.NextFeature();
                    }

                    //释放cursor
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(FeatureCursor);
                }
                catch (Exception e)
                {
                    //*******************************************************************
                    //guozheng added
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
                    return null;
                }
            }
            return null;
        }
        private IGeometry GetBufferGeometryByMapFrame(IGeometry pGeometry, double distance, int state)
        {
            IEnvelope pEnvelope = pGeometry.Envelope;
            IPolyline polyline = new PolylineClass();

            switch (state)
            {
                case 1:      //左边
                    polyline.FromPoint = pEnvelope.UpperLeft;
                    polyline.ToPoint = pEnvelope.LowerLeft;
                    break;
                case 3:     //右边
                    polyline.FromPoint = pEnvelope.UpperRight;
                    polyline.ToPoint = pEnvelope.LowerRight;
                    break;
                case 2:     //上边
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
        public List<IPolyline> Getborderline()
        {
            if (null == this._OriMapFrame)
                return null;
            List<IPolyline> ResList = new List<IPolyline>();
            IEnvelope pEnvelope = this._OriMapFrame.Shape.Envelope;
            IPolyline Lpolyline = new PolylineClass();
            IPolyline Dpolyline = new PolylineClass();
            ////左边
            Lpolyline.FromPoint = pEnvelope.UpperLeft;
            Lpolyline.ToPoint = pEnvelope.LowerLeft;
            ResList.Add(Lpolyline);
            ////下边
            Dpolyline.FromPoint = pEnvelope.LowerLeft;
            Dpolyline.ToPoint = pEnvelope.LowerRight;
            ResList.Add(Dpolyline);
            return ResList;
        }
    }
    /*
     * ClsTaskFrame(非标准图幅)类实现了IMapframe （接边图幅）接口
     * 获取的源接边要素缓冲区（图幅内）和目标要素缓冲区（图幅外）是图幅边界的缓冲区分别与图幅范围求交、求差的范围
     * 设置当前接边图幅可使用GetOriFrame方法通过图幅号字段名与值进行设置，也可通过OriMapFrame属性直接赋值
     * Getborderline（获取接边边界）的方法实现的获取图幅的所有边界（同时适用于规则范围和不规则范围接边，但是使用与规则范围接边时效率较低）
     */
    class ClsTaskFrame : IMapframe
    {
        public ClsTaskFrame()////构造函数
        {
            this._mapframefea = null;
            this._OriMapFrame = null;
        }
        private IFeatureClass _mapframefea;
        public IFeatureClass MapFrameFea
        {
            get { return _mapframefea; }
            set { this._mapframefea = value; }
        }
        private IFeature _OriMapFrame;
        public IFeature OriMapFrame
        {
            get { return this._OriMapFrame; }
            set { this._OriMapFrame = value; }
        }
        public IFeature GetOriFrame(string OriFrameNoValue, string OriFrameNoField)
        {
            IFeature Fea = this.GetFrame(OriFrameNoValue, OriFrameNoField);
            _OriMapFrame = Fea;
            return Fea;
        }
        public void GetBufferArea(double dBufferRadius, out IGeometry OriBufferArea, out IGeometry DesBufferArea)
        {
            OriBufferArea = null;
            DesBufferArea = null;
            if (null == this._OriMapFrame)
                return;
            IGeometry BufferArea = this.GetBufferGeometryByMapFrame(dBufferRadius);
            if (null == BufferArea)
                return;
            ////获取源图幅、目标图幅的接边缓冲区
            try
            {
                ITopologicalOperator topo = BufferArea as ITopologicalOperator;
                OriBufferArea = topo.Intersect(_OriMapFrame.Shape, esriGeometryDimension.esriGeometry2Dimension);
                DesBufferArea = topo.Difference(_OriMapFrame.Shape);
            }
            catch (Exception Error)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(Error, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(Error, null, DateTime.Now);
                }
                //********************************************************************
                throw Error;
            }
        }
        private IFeature GetFrame(string OriFrameNoValue, string OriFrameNoField)
        {
            if (null == this._mapframefea)
                return null;
            else
            {
                try
                {
                    IFeatureCursor FeatureCursor = _mapframefea.Search(null, false);
                    IFeature fea = FeatureCursor.NextFeature();
                    while (null != fea)
                    {
                        int index = -1;
                        index = fea.Fields.FindField(OriFrameNoField);
                        if (index > 0)
                        {
                            string MapFrameno = fea.get_Value(index).ToString();
                            if (OriFrameNoValue == MapFrameno)
                                return fea;
                        }
                        fea = FeatureCursor.NextFeature();
                    }

                    //释放cursor
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(FeatureCursor);
                }
                catch (Exception e)
                {
                    //*******************************************************************
                    //guozheng added
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
                    return null;
                }
            }
            return null;
        }
        private IGeometry GetBufferGeometryByMapFrame(double distance)
        {
            IGeometry geo = null;
            List<IPolyline> BorList = this.Getborderline();
            if (null == BorList)
                return null;
            List<IGeometry> AreaList = new List<IGeometry>();
            for (int i = 0; i < BorList.Count; i++)
            {
                IPolyline line = new PolylineClass();
                line.FromPoint = BorList[i].FromPoint;
                line.ToPoint = BorList[i].ToPoint;
                line.SpatialReference = BorList[i].SpatialReference;
                ITopologicalOperator topo = line as ITopologicalOperator;
                IGeometry buffer = topo.Buffer(distance);
                AreaList.Add(buffer);
            }
            if (AreaList != null)
            {
                for (int i = 0; i < AreaList.Count; i++)
                {
                    IGeometry getGeo = AreaList[i];
                    if (geo == null)
                        geo = getGeo;
                    else
                    {
                        ITopologicalOperator UniTop = geo as ITopologicalOperator;
                        geo = UniTop.Union(getGeo);
                    }
                }
            }
            return geo;

        }
        public List<IPolyline> Getborderline()
        {
            if (null == this._OriMapFrame)
                return null;
            List<IPolyline> ResList = new List<IPolyline>();
            if (this._OriMapFrame.Shape.GeometryType != esriGeometryType.esriGeometryPolygon)
                return null;
            ////////获取任务区的所有边界
            IPolygon MapFramePolygon = this._OriMapFrame.ShapeCopy as IPolygon;
            IPointCollection PoCol = new Polyline();
            PoCol.AddPointCollection(MapFramePolygon as IPointCollection);
            ISegmentCollection newSeCol = PoCol as ISegmentCollection;
            int count = newSeCol.SegmentCount;
            ISegment Seg = null;
            ILine getLine = null;
            for (int j = 0; j < count; j++)
            {
                Seg = newSeCol.get_Segment(j);
                if (Seg.GeometryType == esriGeometryType.esriGeometryLine)
                {
                    getLine = Seg as ILine;
                    getLine.SpatialReference = _OriMapFrame.Shape.SpatialReference;
                    IPolyline GetPolyline = new PolylineClass();
                    GetPolyline.FromPoint = getLine.FromPoint;
                    GetPolyline.ToPoint = getLine.ToPoint;
                    GetPolyline.SpatialReference = getLine.SpatialReference;
                    ResList.Add(GetPolyline);
                }
            }
            return ResList;
        }
    }
    /*
     * ClsDestinatDataset类实现了IDestinatDataset（接边目标层集）接口
     * GetFeaturesByGeometry（获取待接边要素信息）方法实现了通过缓冲区获取待接边要素信息的方法，
     *   GetFeaturesByGeometry（获取待接边要素信息）方法中维护一个记录已搜索出的接边要素记录集，
     *   为防止出现重复接边情况，在使用循环赋予缓冲区获取接边要素时，声明应当在循环外部
     *   Dictionary中string key 是接边图层名，long是接边要素oid
     */

    class ClsDestinatDataset : IDestinatDataset
    {
        public ClsDestinatDataset()////构造函数
        {
            this._IsStandardMapFrame = false;
            this._JoinFeatureClass = null;
            this._IsGeometrySimplify = false;
            this._IsRemoveRedundantPnt = false;
            this._Angle_to = 1;
            this.m_lremve = new List<long>();
            this.m_lsimplify = new List<long>();
        }
        public ClsDestinatDataset(bool IsStandard)////构造函数
        {
            this._IsStandardMapFrame = IsStandard;
            this._JoinFeatureClass = null;
            this._IsGeometrySimplify = false;
            this._IsRemoveRedundantPnt = false;
            this._Angle_to = 1;
            this.m_lremve = new List<long>();
            this.m_lsimplify = new List<long>();
        }
        private bool _IsRemoveRedundantPnt;/////是否删除多边形上多余点
        public bool IsRemoveRedundantPnt
        {
            get { return this._IsRemoveRedundantPnt; }
            set { this._IsRemoveRedundantPnt = value; }
        }
        private bool _IsGeometrySimplify;//////是否进行要素的简单化（针对具有多个Geometry的要素）
        public bool IsGeometrySimplify
        {
            get { return this._IsGeometrySimplify; }
            set { this._IsGeometrySimplify = value; }
        }
        private double _Angle_to;/////角度容差
        public double Angle_to
        {
            get { return this._Angle_to; }
            set { this._Angle_to = value; }
        }

        private bool _IsStandardMapFrame;
        public bool IsStandardMapFrame
        {
            get { return this._IsStandardMapFrame; }
            set { this._IsStandardMapFrame = value; }
        }////是否为标准图幅接

        private List<long> m_lremve;
        private List<long> m_lsimplify;///////维护一个列表记录删除多余点、简单化要素的oid避免重复操作

        private Dictionary<string, List<long>> JoinedFeaOIDDic = new Dictionary<string, List<long>>();////维护一个已接边的要素记录结合，防止重复接边
        private List<IFeatureClass> _JoinFeatureClass;
        public List<IFeatureClass> JoinFeatureClass
        {
            get { return this._JoinFeatureClass; }
            set { this._JoinFeatureClass = value; }
        }
        public IFeatureClass TargetFeatureClass(string FeatureClassName)////遍历获取图层
        {
            if (null == this._JoinFeatureClass)
                return null;
            foreach (IFeatureClass Feas in this._JoinFeatureClass)
            {
                if ((Feas as IDataset).Name == FeatureClassName)
                    return Feas;
            }
            return null;
        }
        public Dictionary<string, List<long>> GetFeaturesByGeometry(IGeometry BufferArea, bool isOriArea)
        {
            Dictionary<string, List<long>> ResDic = new Dictionary<string, List<long>>();
            try
            {
                if (null != this._JoinFeatureClass)
                {
                    //////数据操作类
                    ClsDataOperationer DataOper = new ClsDataOperationer();
                    DataOper.Angle_to = this._Angle_to;
                    for (int i = 0; i < this._JoinFeatureClass.Count; i++)
                    {
                        IFeatureClass ipFtClass = _JoinFeatureClass[i];
                        DataOper.OpeFeaClss = ipFtClass;
                        ISpatialFilter SpFilter = new SpatialFilterClass();
                        SpFilter.Geometry = BufferArea;
                        IFeatureClass FromFeaCls = ipFtClass;
                        string FeaName = (FromFeaCls as IDataset).Name;
                        List<IFeature> GetFea = new List<IFeature>();

                        if (ipFtClass.ShapeType == esriGeometryType.esriGeometryPolyline)//////线型要素获取
                        {
                            #region 线要素穿越获取
                            SpFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                            IFeatureCursor GetFeaCur = FromFeaCls.Search(SpFilter, false);
                            if (null != GetFeaCur)
                            {
                                IFeature Fea = null;
                                if (this._IsGeometrySimplify)//////要素简单化
                                {
                                    Fea = GetFeaCur.NextFeature();
                                    while (null != Fea)
                                    {
                                        if (!this.m_lsimplify.Contains((long)Fea.OID))
                                        {
                                            this.m_lsimplify.Add((long)Fea.OID);
                                            DataOper.GeometrySimplify((long)Fea.OID);
                                            Fea = GetFeaCur.NextFeature();
                                        }
                                        Fea = GetFeaCur.NextFeature();
                                    }
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(GetFeaCur);
                                    GetFeaCur = FromFeaCls.Search(SpFilter, false);
                                }

                                Fea = GetFeaCur.NextFeature();
                                while (null != Fea)
                                {
                                    GetFea.Add(Fea);
                                    Fea = GetFeaCur.NextFeature();
                                }

                            }
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(GetFeaCur);/////释放游标否则多次循环会报错
                            #endregion

                        }
                        else if (ipFtClass.ShapeType == esriGeometryType.esriGeometryPolygon)//////多变形要素获取
                        {
                            #region 多边形交叠获取
                            SpFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelOverlaps;
                            IFeatureCursor GetFeaCur = FromFeaCls.Search(SpFilter, false);
                            if (null != GetFeaCur)
                            {
                                IFeature Fea = null;
                                if (this._IsGeometrySimplify)//////要素简单化
                                {
                                    Fea = GetFeaCur.NextFeature();
                                    while (null != Fea)
                                    {
                                        if (!this.m_lsimplify.Contains((long)Fea.OID))
                                        {
                                            this.m_lsimplify.Add((long)Fea.OID);
                                            DataOper.GeometrySimplify((long)Fea.OID);
                                            Fea = GetFeaCur.NextFeature();
                                        }
                                        Fea = GetFeaCur.NextFeature();
                                    }
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(GetFeaCur);
                                    GetFeaCur = FromFeaCls.Search(SpFilter, false);
                                }

                                Fea = GetFeaCur.NextFeature();
                                while (null != Fea)
                                {

                                    if (this._IsRemoveRedundantPnt)//////去除多边形上多余点
                                    {
                                        if (!this.m_lremve.Contains((long)Fea.OID))
                                        {
                                            DataOper.RemoveRedundantPntFromPolygon((long)Fea.OID);
                                            this.m_lremve.Add((long)Fea.OID);
                                        }
                                    }

                                    GetFea.Add(Fea);
                                    Fea = GetFeaCur.NextFeature();
                                }
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(GetFeaCur);
                            }
                            #endregion
                        }
                        SpFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;//////线型要素与多边形要素(包含)获取
                        IFeatureCursor GetFeaCur2 = FromFeaCls.Search(SpFilter, false);
                        if (null != GetFeaCur2)
                        {
                            #region 线要素、面要素包含获取
                            IFeature Fea = null;
                            if (this._IsGeometrySimplify)//////要素简单化
                            {
                                Fea = GetFeaCur2.NextFeature();
                                while (null != Fea)
                                {
                                    if (!this.m_lsimplify.Contains((long)Fea.OID))
                                    {
                                        this.m_lsimplify.Add((long)Fea.OID);
                                        DataOper.GeometrySimplify((long)Fea.OID);
                                        Fea = GetFeaCur2.NextFeature();

                                    }
                                    Fea = GetFeaCur2.NextFeature();
                                }
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(GetFeaCur2);
                                GetFeaCur2 = FromFeaCls.Search(SpFilter, false);
                            }

                            Fea = GetFeaCur2.NextFeature();
                            while (null != Fea)
                            {
                                if (Fea.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                                {
                                    if (this._IsRemoveRedundantPnt)//////去除多边形上多余点
                                    {
                                        if (!this.m_lremve.Contains((long)Fea.OID))
                                        {
                                            DataOper.RemoveRedundantPntFromPolygon((long)Fea.OID);
                                            this.m_lremve.Add((long)Fea.OID);
                                        }
                                    }
                                }
                                GetFea.Add(Fea);
                                Fea = GetFeaCur2.NextFeature();
                            }
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(GetFeaCur2);/////释放游标否则多次循环会报错
                            #endregion
                        }

                        if (null != GetFea)
                        {

                            for (int n = 0; n < GetFea.Count; n++)
                            {

                                IFeature Fea = GetFea[n];
                                long OID = (long)Fea.OID;
                                /////////////////////判断此要素是否已经在搜索记录结果中(只在非标准图幅接边时判断)
                                if (this._IsStandardMapFrame == false)
                                {
                                    if (isOriArea == false)
                                    {
                                        if (JoinedFeaOIDDic.ContainsKey(FeaName))
                                        {
                                            if (JoinedFeaOIDDic[FeaName].Contains(OID))//////此要素已经存在记录
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                }
                                /////////////////////
                                if (ResDic.ContainsKey(FeaName))
                                {
                                    ResDic[FeaName].Add(OID);
                                }
                                else
                                {
                                    List<long> OidList = new List<long>();
                                    OidList.Add(OID);
                                    ResDic.Add(FeaName, OidList);
                                }
                                //////////////////记录已搜索出的要素记录
                                if (this._IsStandardMapFrame == false)
                                {
                                    if (isOriArea == true)
                                    {
                                        if (JoinedFeaOIDDic.ContainsKey(FeaName))
                                        {
                                            JoinedFeaOIDDic[FeaName].Add(OID);
                                        }
                                        else
                                        {
                                            List<long> OidList = new List<long>();
                                            OidList.Add(OID);
                                            JoinedFeaOIDDic.Add(FeaName, OidList);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return ResDic;
            }
            catch (Exception eError)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog == null)
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write(eError, null, DateTime.Now);
                //********************************************************************
                return null;
            }
        }
    }
    /*
     * ClsCheckOperationer类实现了ICheckOperation（接边检查接口）
     * 
     */
    class ClsCheckOperationer : ICheckOperation
    {
        private bool _CreatLog;
        public bool CreatLog
        {
            get { return this._CreatLog; }
            set { this._CreatLog = value; }
        }
        public ClsCheckOperationer()////构造函数
        {
            this._Angel_Tolerrance = 1;
            this._borderline = null;
            this._DesBufferArea = null;
            this._DestinatFeaCls = null;
            this._Dis_Tolerance = 3;
            this._Length_Tolerrance = 1;
            this._OriBufferArea = null;
            this._OriFeaturesOID = null;
            this._Search_Tolerrance = 3;
            this._DesFeaturesOID = null;
            this._CreatLog = false;
            this.FieldsControlList = null;
        }
        private IFeatureClass _DestinatFeaCls;////待接边图层
        public IFeatureClass DestinatFeaCls
        {
            get { return this._DestinatFeaCls; }
            set { this._DestinatFeaCls = value; }
        }

        private List<IPolyline> _borderline;/////接边边界
        public List<IPolyline> borderline
        {
            get { return this._borderline; }
            set { this._borderline = value; }
        }

        private List<long> _OriFeaturesOID;/////待接边源要素OID
        public List<long> OriFeaturesOID
        {
            get { return this._OriFeaturesOID; }
            set { this._OriFeaturesOID = value; }
        }

        private List<long> _DesFeaturesOID;/////待接边目标要素OID
        public List<long> DesFeaturesOID
        {
            get { return this._DesFeaturesOID; }
            set { this._DesFeaturesOID = value; }
        }

        private List<string> _FieldsControlList;///////控制属性字段列表
        public List<string> FieldsControlList
        {
            get { return this._FieldsControlList; }
            set { this._FieldsControlList = value; }
        }

        private double _Dis_Tolerance;/////距离容差
        public double Dis_Tolerance
        {
            get { return this._Dis_Tolerance; }
            set { this._Dis_Tolerance = value; }
        }
        private double _Search_Tolerrance;/////搜索容差
        public double Search_Tolerrance
        {
            get { return this._Search_Tolerrance; }
            set { this._Search_Tolerrance = value; }
        }
        private double _Angel_Tolerrance;/////角度容差
        public double Angel_Tolerrance
        {
            get { return this._Angel_Tolerrance; }
            set { this._Angel_Tolerrance = value; }
        }
        private double _Length_Tolerrance;
        public double Length_Tolerrance ////长度容差
        {
            get { return this._Length_Tolerrance; }
            set { this._Length_Tolerrance = value; }
        }
        private IGeometry _OriBufferArea;////源图幅接边缓冲区域
        public IGeometry OriBufferArea
        {
            get { return this._OriBufferArea; }
            set { this._OriBufferArea = value; }
        }
        private IGeometry _DesBufferArea;/////目标图幅接边缓冲区域
        public IGeometry DesBufferArea
        {
            get { return this._DesBufferArea; }
            set { this._DesBufferArea = value; }
        }
        public esriGeometryType GetDatasetGeometryType()
        {
            if (null == this._DestinatFeaCls)
                return esriGeometryType.esriGeometryAny;
            return this._DestinatFeaCls.ShapeType;
        }
        public DataTable GetPolylineDesFeatureOIDByOriFeature()////获取线型待接边目标要素集中要与源要素接边的要素
        {
                Exception ex = null;
                if (this._OriFeaturesOID == null)
                    return null;
                IJoinLOG JoinLog = new ClsJoinLog();

                DataTable ResTable = new DataTable();
                DataColumn dc1 = new DataColumn("数据集", Type.GetType("System.String"));
                DataColumn dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
                DataColumn dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
                dc3.DefaultValue = -1;
                DataColumn dc4 = new DataColumn("OriPtn", Type.GetType("System.String"));
                DataColumn dc5 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
                dc5.DefaultValue = -1;
                DataColumn dc6 = new DataColumn("DesPtn", Type.GetType("System.String"));
                DataColumn dc7 = new DataColumn("接边状态", Type.GetType("System.String"));
                ResTable.Columns.Add(dc1);
                ResTable.Columns.Add(dc2);
                ResTable.Columns.Add(dc3);
                ResTable.Columns.Add(dc4);
                ResTable.Columns.Add(dc5);
                ResTable.Columns.Add(dc6);
                ResTable.Columns.Add(dc7);
                FrmProcessBar ProcBar = new FrmProcessBar((long)this._OriFeaturesOID.Count);
                ProcBar.SetChild();
                ProcBar.Show();
                for (int i = 0; i < this._OriFeaturesOID.Count; i++)
                {
                    long OriOID = this._OriFeaturesOID[i];
                    ////进度条
                    ProcBar.SetFrmProcessBarValue(i);
                    ProcBar.SetFrmProcessBarText("正在处理线要素：" + OriOID);
                    System.Windows.Forms.Application.DoEvents();
                    IFeatureClass Feacls = this._DestinatFeaCls;
                    IFeature Fea = Feacls.GetFeature((int)OriOID);
                    IPolyline OriPolyline = Fea.ShapeCopy as IPolyline;

                    IPoint FrPoint = OriPolyline.FromPoint;
                    IPoint ToPOint = OriPolyline.ToPoint;
                    DataRow newRow = ResTable.NewRow();
                    newRow["数据集"] = Feacls.AliasName;
                    newRow["要素类型"] = "Polyline";
                    newRow["源要素ID"] = OriOID;
                    newRow["OriPtn"] = "ToPoint";


                    DataRow newRow2 = ResTable.NewRow();
                    newRow2["数据集"] = Feacls.AliasName; ;
                    newRow2["要素类型"] = "Polyline";
                    newRow2["源要素ID"] = OriOID;
                    newRow2["OriPtn"] = "FromPoint";
                    bool rec = false;
                    bool judge = false;
                    DataRow AddRowT = null;
                    DataRow AddRowF = null;
                    ProcBar.SetFrmProcessBarText("正在处理线要素：" + OriOID + "，接边目标要素搜索");
                    System.Windows.Forms.Application.DoEvents();
                    judge = JudgePolygonIsContainPT(ToPOint, this._OriBufferArea);

                    if (judge)
                    {
                        AddRowT = GetPointJionDes(ToPOint, newRow, out rec);//////终点搜索
                        if (rec)
                        {
                            ResTable.Rows.Add(AddRowT);
                            if (this._CreatLog)
                            {
                                JoinLog.OnDataJoin_OnCheck(AddRowT, ToPOint.X, ToPOint.Y, out ex);
                            }
                        }
                    }
                    else if (JudgePolygonIsContainPT(ToPOint, this._DesBufferArea))
                    {
                        AddRowT = GetPointJionDes(ToPOint, newRow, out rec);//////终点搜索
                        if (rec)
                        {
                            ResTable.Rows.Add(AddRowT);
                            if (this._CreatLog)
                            {
                                JoinLog.OnDataJoin_OnCheck(AddRowT, ToPOint.X, ToPOint.Y, out ex);
                            }
                        }
                    }
                    judge = JudgePolygonIsContainPT(FrPoint, this._OriBufferArea);
                    if (judge)
                    {
                        AddRowF = GetPointJionDes(FrPoint, newRow2, out rec);//////起点搜索
                        if (rec)
                        {
                            ResTable.Rows.Add(AddRowF);
                            if (this._CreatLog)
                            {
                                JoinLog.OnDataJoin_OnCheck(AddRowF, FrPoint.X, FrPoint.Y, out ex);
                            }
                        }
                    }
                    else if (JudgePolygonIsContainPT(FrPoint, this._DesBufferArea))
                    {
                        AddRowF = GetPointJionDes(FrPoint, newRow2, out rec);//////起点搜索
                        if (rec)
                        {
                            ResTable.Rows.Add(AddRowF);
                            if (this._CreatLog)
                            {
                                JoinLog.OnDataJoin_OnCheck(AddRowF, FrPoint.X, FrPoint.Y, out ex);
                            }
                        }
                    }

                }
                ProcBar.Close();
                JoinLog.onDataJoin_Terminate(0, out ex);
                return ResTable;
            

 
        }
       
        public DataTable GetPolygonDesFeatureOIDByOriFeature()////获取多边形待接边目标要素集中要与源要素接边的要素
        {
            DataTable ResTable = new DataTable();
            IJoinLOG JoinLog = new ClsJoinLog();
            DataColumn dc1 = new DataColumn("数据集", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
            DataColumn dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
            dc3.DefaultValue = -1;
            DataColumn dc4 = new DataColumn("OriLineIndex", Type.GetType("System.Int64"));
            dc4.DefaultValue = -1;
            DataColumn dc5 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
            dc5.DefaultValue = -1;
            DataColumn dc6 = new DataColumn("DesLineIndex", Type.GetType("System.Int64"));
            dc6.DefaultValue = -1;
            DataColumn dc7 = new DataColumn("接边状态", Type.GetType("System.String"));
            ResTable.Columns.Add(dc1);
            ResTable.Columns.Add(dc2);
            ResTable.Columns.Add(dc3);
            ResTable.Columns.Add(dc4);
            ResTable.Columns.Add(dc5);
            ResTable.Columns.Add(dc6);
            ResTable.Columns.Add(dc7);
            //////////////////////////////////////////////////////////
            if (this._OriFeaturesOID == null)
                return null;
            FrmProcessBar ProcBar = new FrmProcessBar((long)this._OriFeaturesOID.Count);
            ProcBar.SetChild();
            ProcBar.Show();
            for (int i = 0; i < this._OriFeaturesOID.Count; i++)
            {
                /////////////////////////获取待接边源要素及接边的边的Index
                /////进度条
                long OriOID = this._OriFeaturesOID[i];
                ProcBar.SetFrmProcessBarValue(i);
                ProcBar.SetFrmProcessBarText("正在处理多边形要素：" + OriOID);
                System.Windows.Forms.Application.DoEvents();

                IFeatureClass Feacls = this._DestinatFeaCls;
                IFeature Fea = Feacls.GetFeature((int)OriOID);
                IPolygon OriPolygon = Fea.Shape as IPolygon;
                if (null == OriPolygon)
                    continue;
                List<long> Index = new List<long>();
                List<ILine> lineList = GetPolygonJoinLine(Fea, this._OriBufferArea, out Index);
                ///////////////////////获取待接边目标要素及接边的边的Index
                List<long> oid = new List<long>();
                List<long> oidindex = new List<long>();
                if (null != lineList)
                {
                    for (int j = 0; j < lineList.Count; j++)
                    {
                        ILine getline = lineList[j];
                        long OIDIndex = -1;
                        long lOID = GetDesOIDByLine(getline, out OIDIndex);
                        oid.Add(lOID);
                        oidindex.Add(OIDIndex);
                    }
                }
                if (null != oid && null != lineList)
                {
                    if (lineList.Count == oid.Count)
                    {
                        for (int index = 0; index < oid.Count; index++)
                        {
                            if (OriOID == oid[index])
                                continue;
                            if (oid[index] == -1)
                                continue;
                            /////进度条
                            ProcBar.SetFrmProcessBarText("正在处理多边形要素：" + OriOID + ",接边目标要素：" + oid[index]);
                            System.Windows.Forms.Application.DoEvents();
                            DataRow newRow = ResTable.NewRow();
                            newRow["数据集"] = this._DestinatFeaCls.AliasName;
                            newRow["要素类型"] = "Polygon";
                            newRow["源要素ID"] = OriOID;
                            newRow["OriLineIndex"] = Index[index];
                            newRow["目标要素ID"] = oid[index];
                            newRow["DesLineIndex"] = oidindex[index];
                            ////////////////属性判断/////////////////////
                            string sFieldState = string.Empty;
                            if (this.FieldsControlList != null)
                            {
                                if (judgeTwoFeatureField(OriOID, oid[index]))
                                    sFieldState = "，属性一致";
                                else
                                    sFieldState = "，属性不一致";
                            }
                            ///////////////////////////////////////////////
                            IFeature DesFeature = Feacls.GetFeature((int)oid[index]);
                            IRelationalOperator RelOper = Fea.ShapeCopy as IRelationalOperator;
                            if (RelOper.Touches(DesFeature.ShapeCopy))
                            {
                                newRow["接边状态"] = "已接边" + sFieldState;
                            }
                            else
                            {
                                newRow["接边状态"] = "未接边" + sFieldState;
                            }

                            ResTable.Rows.Add(newRow);
                            if (this._CreatLog)
                            {
                                Exception ex = null;
                                JoinLog.OnDataJoin_OnJoin(newRow, OriPolygon.ToPoint.X, OriPolygon.ToPoint.Y, out ex);
                            }
                        }
                    }
                }
            }
            ProcBar.Close();
            return ResTable;
        }
        public static bool JudgePolygonIsContainPT(IPoint po, IGeometry Area)////判断点是否在多边形内
        {
            IRelationalOperator RelOp = Area as IRelationalOperator;
            bool Sate = RelOp.Contains(po);
            if (Sate == false)
            {
                RelOp = Area as IRelationalOperator;
                Sate = RelOp.Touches(po);
            }
            return Sate;

        }
        public static bool JudgePolygonIsContainLine(ILine line, IGeometry Area)////判断线是否在多边形内
        {
            IRelationalOperator RelOp = Area as IRelationalOperator;
            IPointCollection pPntColLine = null;
            IGeometry pGeometry = null;
            pPntColLine = new Polyline();
            object missing = Type.Missing;
            pPntColLine.AddPoint(line.FromPoint, ref missing, ref missing);
            pPntColLine.AddPoint(line.ToPoint, ref missing, ref missing);
            pGeometry = pPntColLine as IGeometry;
            bool Sate = RelOp.Contains(pGeometry);
            bool touch = false;
            touch = RelOp.Touches(pGeometry);
            if (Sate || touch)
                return true;
            else
                return false;
        }
        private IFeatureCursor GetFeaturesByGeometry(IGeometry Area, esriSpatialRelEnum Rel)/////通过Geometry及空间关系获取与要素集
        {
            if (this._DestinatFeaCls == null)
                return null;
            ISpatialFilter SpFilter = new SpatialFilterClass();
            SpFilter.Geometry = Area;
            SpFilter.SpatialRel = Rel;
            //IFeatureClass FromFeaCls = this._DestinatFeaCls;
            IFeatureCursor GetFeaCur = this._DestinatFeaCls.Search(SpFilter, false);
            return GetFeaCur;
        }
        private DataRow GetPointJionDes(IPoint point, DataRow in_Row, out bool rec)////获取待接边点的目标OID
        {
            rec = false;
            if (null == in_Row)
                return null;
            long OriOid = -1;
            try
            {
                OriOid = Convert.ToInt64(in_Row["源要素ID"].ToString());
            }
            catch
            {
                rec = false;
                return null;
            }
            ITopologicalOperator Topoper = point as ITopologicalOperator;
            IGeometry Pointbuffer = Topoper.Buffer(this._Search_Tolerrance);
            List<IFeature> FeaList = new List<IFeature>();
            IFeatureCursor DesFeaCursor = this.GetFeaturesByGeometry(Pointbuffer, esriSpatialRelEnum.esriSpatialRelIntersects);
            if (null != DesFeaCursor)
            {
                IFeature Fea = DesFeaCursor.NextFeature();
                while (null != Fea)
                {
                    FeaList.Add(Fea);
                    Fea = DesFeaCursor.NextFeature();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(DesFeaCursor);/////释放游标否则多次循环会出错
            }
            if (FeaList.Count == 0)
                return null;
            string TopointDesPt = string.Empty;
            string FrmpointDesPt = string.Empty;
            double MinDis = this._Dis_Tolerance;
            string DesPt = string.Empty;
            string joinpt = string.Empty;
            IPolyline DesPolyline = null;
            IPolyline ResPolyline = null;
            long ResFeatureOID = -1;
            #region 遍历FeaList获取满足接边条件要素的OID

            for (int i = 0; i < FeaList.Count; i++)
            {
                IFeature DesFea = FeaList[i];
                if (OriOid == (long)DesFea.OID)
                    continue;

                if (this._DesFeaturesOID.Contains((long)DesFea.OID) && DesFea.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                {
                    DesPolyline = DesFea.Shape as IPolyline;
                    IPoint DesFrpoint = DesPolyline.FromPoint;
                    IPoint DesTopoint = DesPolyline.ToPoint;
                    ////对搜索到的feature计算其起点、终点与待接边源polyline端点的距离，取距离最小的，
                    // IRelationalOperator RelOper = point as IRelationalOperator;
                    if (JudgePolygonIsContainPT(DesFrpoint, Pointbuffer))
                    {
                        #region 判断DesFromPoint
                        double distance = ClsCheckOperationer.CalculateDistance(point, DesFrpoint);
                        if (distance < MinDis)
                        {
                            MinDis = distance;
                            joinpt = "FromPoint";
                            //ResPoint = DesFrpoint;
                            ResFeatureOID = (long)DesFea.OID;
                            ResPolyline = DesPolyline;
                            //in_Row["目标要素ID"] = DesFea.OID; ;
                            //in_Row["DesPtn"] = "FromPoint";
                            //if (RelOper.Equals(DesFrpoint))
                            //{
                            //    in_Row["接边状态"] = "已接边";
                            //}
                            //else
                            //{
                            //    in_Row["接边状态"] = "未接边";
                            //}
                            rec = true;
                        }
                        #endregion
                    }

                    if (JudgePolygonIsContainPT(DesTopoint, Pointbuffer))
                    {
                        #region 判断DesToPoint
                        double distance = ClsCheckOperationer.CalculateDistance(point, DesTopoint);
                        if (distance < MinDis)
                        {
                            MinDis = distance;
                            joinpt = "ToPoint";
                            //ResPoint = DesTopoint;
                            ResFeatureOID = (long)DesFea.OID;
                            ResPolyline = DesPolyline;
                            //in_Row["目标要素ID"] = DesFea.OID;
                            //in_Row["DesPtn"] = "ToPoint";
                            //if (RelOper.Equals(DesTopoint))
                            //{
                            //    in_Row["接边状态"] = "已接边";
                            //}
                            //else
                            //{
                            //    in_Row["接边状态"] = "未接边";
                            //}
                            rec = true;
                        }

                        #endregion
                    }
                }
                else
                {
                    continue;
                }
            }

            #endregion
            /////////////////////判断属性情况///////////////////////////
            string sFieldState = string.Empty;
            if (this.FieldsControlList != null)
            {

                if (judgeTwoFeatureField(OriOid, ResFeatureOID))
                    sFieldState = "，属性一致";
                else
                    sFieldState = "，属性不一致";
            }
            ///////////////////////////////////////////////////////////

            if (string.IsNullOrEmpty(joinpt))
            {
                rec = false;
                return null;
            }
            IRelationalOperator RelOper = point as IRelationalOperator;
            if (joinpt == "FromPoint")
            {
                in_Row["目标要素ID"] = ResFeatureOID;
                in_Row["DesPtn"] = "FromPoint";
                if (RelOper.Equals(ResPolyline.FromPoint))
                {
                    in_Row["接边状态"] = "已接边" + sFieldState;
                }
                else
                {
                    in_Row["接边状态"] = "未接边" + sFieldState;
                }
                rec = true;
            }
            else if (joinpt == "ToPoint")
            {
                in_Row["目标要素ID"] = ResFeatureOID;
                in_Row["DesPtn"] = "ToPoint";
                if (RelOper.Equals(ResPolyline.ToPoint))
                {
                    in_Row["接边状态"] = "已接边" + sFieldState;
                }
                else
                {
                    in_Row["接边状态"] = "未接边" + sFieldState;
                }
                rec = true;
            }


            DesFeaCursor = null;
            return in_Row;

        }
        private List<ILine> GetPolygonJoinLine(IFeature Pol, IGeometry JoinArea, out List<long> Index)//// 获取待接边多多边形需要参与接边的边
        {
            Index = new List<long>();
            IGeometryCollection GeoCol = Pol.ShapeCopy as IGeometryCollection;
            if (GeoCol.GeometryCount != 1)
                return null;
            List<ILine> linelist = new List<ILine>();
            IPointCollection PoCol = new Polyline();
            IPolygon newpolygon = Pol.ShapeCopy as IPolygon;
            PoCol.AddPointCollection(newpolygon as IPointCollection);
            ISegmentCollection newSeCol = PoCol as ISegmentCollection;
            int count = newSeCol.SegmentCount;

            ISegment Seg = null;
            ILine getLine = null;
            for (int j = 0; j < count; j++)
            {
                Seg = newSeCol.get_Segment(j);
                if (Seg.GeometryType == esriGeometryType.esriGeometryLine)
                {
                    getLine = Seg as ILine;
                    getLine.SpatialReference = Pol.Shape.SpatialReference;
                    if (JudgePolygonIsContainLine(getLine, JoinArea))
                    {
                        bool ISParallel = false;
                        if (this._borderline != null)//////若设置了接边边界则使用接边边界平行条件来筛选边
                        {
                            double minDistanxe = this._Dis_Tolerance;
                            ILine newline = null;
                            for (int i = 0; i < this._borderline.Count; i++)
                            {
                                IPolyline BorPolLine = this._borderline[i];

                                double distance = -1;
                                double curve = -1;
                                bool Isright = true;
                                BorPolLine.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, getLine.ToPoint, false, null, ref curve, ref distance, ref Isright);
                                if (distance < minDistanxe)
                                {
                                    BorPolLine.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, getLine.FromPoint, false, null, ref curve, ref distance, ref Isright);
                                    if (distance < minDistanxe)
                                    {
                                        minDistanxe = distance;
                                        newline = new LineClass();
                                        newline.ToPoint = BorPolLine.ToPoint;
                                        newline.FromPoint = BorPolLine.FromPoint;
                                        newline.SpatialReference = BorPolLine.SpatialReference;

                                        ISParallel = JudgeParallel(newline, getLine, this.Angel_Tolerrance);
                                        if (ISParallel)
                                        {

                                            linelist.Add(getLine);
                                            Index.Add(j);
                                        }
                                    }
                                }

                            }
                            //ISParallel = JudgeParallel(newline, getLine, this.Angel_Tolerrance);
                            //if (ISParallel)
                            //{

                            //    linelist.Add(getLine);
                            //    Index.Add(j);
                            //}
                        }
                        else
                        {
                            linelist.Add(getLine);
                            Index.Add(j);
                        }
                    }
                }

            }
            return linelist;
        }
        public static bool JudgeParallel(ILine Oline, ILine Dline, double AngeleTo)////根据角度容差判断两条线是否平行
        {
            double dOriAngle;
            double dDesAngle;
            double PI = 3.1415927;
            if (null == Oline || null == Dline)
                return false;
            dOriAngle = Oline.Angle * 180 / PI;
            dDesAngle = Dline.Angle * 180 / PI;

            ///由于不考虑线的方向,所以只考虑线与x轴的夹角的绝对值
            if (dOriAngle < 0)
                dOriAngle = 180 + dOriAngle;
            if (dDesAngle < 0)
                dDesAngle = 180 + dDesAngle;
            if (Math.Abs(dOriAngle - dDesAngle) <= AngeleTo || Math.Abs(dOriAngle - dDesAngle) >= (180 - AngeleTo))
                return true;
            else
                return false;
        }
        private long GetDesOIDByLine(ILine joinline, out long jionIndex)////通过多边形的一条边获取待接边目标多边形OID
        {
            jionIndex = -1;
            if (this._DesFeaturesOID == null)
                return -1;
            if (joinline == null)
                return -1;

            IPolyline ppolyline = new PolylineClass();
            ppolyline.ToPoint = joinline.ToPoint;
            ppolyline.FromPoint = joinline.FromPoint;
            ppolyline.SpatialReference = joinline.SpatialReference;
            ITopologicalOperator Topoper = ppolyline as ITopologicalOperator;
            IGeometry Linebuffer = Topoper.Buffer(this._Search_Tolerrance);
            List<IFeature> Fealist = new List<IFeature>();
            IFeatureCursor DesFeaCursor = this.GetFeaturesByGeometry(Linebuffer, esriSpatialRelEnum.esriSpatialRelIntersects);
            if (DesFeaCursor != null)
            {
                IFeature getfea = DesFeaCursor.NextFeature();
                while (null != getfea)
                {
                    Fealist.Add(getfea);
                    getfea = DesFeaCursor.NextFeature();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(DesFeaCursor);
            }
            DesFeaCursor = null;
            if (Fealist.Count == 0)
                return -1;
            long OID = -1;
            double distance = this._Dis_Tolerance;
            double pdistance = this._Dis_Tolerance;
            for (int j = 0; j < Fealist.Count; j++)
            {
                IFeature DesFea = Fealist[j];
                if (!this._DesFeaturesOID.Contains((long)DesFea.OID))
                    continue;

                List<long> Index = new List<long>();
                List<ILine> DesLine = this.GetPolygonJoinLine(DesFea, this._DesBufferArea, out Index);
                if (null != DesLine)
                {

                    for (int i = 0; i < DesLine.Count; i++)
                    {
                        ILine line = DesLine[i];
                        ///////////还要加判断：如果两个多边形待接边的两条边两个端点共点，另两个端点不共点则不作为接边
                        if (!judgeTwoLineIsVerOrOnline(joinline, line))
                            continue;
                        double curve = -1;
                        bool Isright = true;
                        IPolyline newpolyline = new PolylineClass();
                        newpolyline.ToPoint = line.ToPoint;
                        newpolyline.FromPoint = line.FromPoint;
                        newpolyline.SpatialReference = line.SpatialReference;
                        newpolyline.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, joinline.ToPoint, false, null, ref curve, ref distance, ref Isright);
                        double tem = -1;
                        newpolyline.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, joinline.FromPoint, false, null, ref curve, ref tem, ref Isright);
                        if (tem < distance)
                            distance = tem;
                        if (distance < this._Dis_Tolerance)
                        {
                            if (!this.JudgeTwoLineIsinLength_to(joinline, line)) //////如果两条线不在长度容差范围内继续检索下一条线
                                continue;


                            tem = CalculateDistance(joinline.ToPoint, line.ToPoint);//////找出距离最近的线
                            if (tem < pdistance)
                            {
                                pdistance = tem;
                                OID = (long)DesFea.OID;
                                jionIndex = Index[i];
                            }
                            tem = CalculateDistance(joinline.ToPoint, line.FromPoint);
                            if (tem < pdistance)
                            {
                                pdistance = tem;
                                OID = (long)DesFea.OID;
                                jionIndex = Index[i];
                            }
                            tem = CalculateDistance(joinline.FromPoint, line.ToPoint);
                            if (tem < pdistance)
                            {
                                pdistance = tem;
                                OID = (long)DesFea.OID;
                                jionIndex = Index[i];
                            }
                            tem = CalculateDistance(joinline.FromPoint, line.FromPoint);
                            if (tem < pdistance)
                            {
                                pdistance = tem;
                                OID = (long)DesFea.OID;
                                jionIndex = Index[i];
                            }
                        }
                    }
                }

            }
            return OID;
        }
        public static double CalculateDistance(IPoint FromPoint, IPoint ToPoint)//////计算两点距离
        {
            if (null == ToPoint || null == FromPoint)
                return -1;
            double x1 = FromPoint.X;
            double y1 = FromPoint.Y;
            double x2 = ToPoint.X;
            double y2 = ToPoint.Y;
            double _x = x2 - x1;
            double _y = y2 - y1;
            double Distance = Math.Sqrt(Math.Pow(_x, 2) + Math.Pow(_y, 2));
            return Distance;
        }
        private bool JudgeTwoLineIsinLength_to(ILine line1, ILine line2)
        {
            double length = Math.Abs(line1.Length - line2.Length);
            if (length < this._Length_Tolerrance)
                return true;
            else
                return false;
        }
        private bool judgeTwoLineIsVerOrOnline(ILine line1, ILine line2)
        {
            IPoint Topoint1 = line1.ToPoint;
            IPoint Frompoint1 = line1.FromPoint;
            IPoint Topoint2 = line2.ToPoint;
            IPoint Frompoint2 = line2.FromPoint;
            IRelationalOperator SpaRel = Topoint1 as IRelationalOperator;
            if (SpaRel.Equals(Topoint2))
            {
                SpaRel = Frompoint1 as IRelationalOperator;
                if (!SpaRel.Equals(Frompoint2))
                {
                    double dis = CalculateDistance(Frompoint1, Frompoint2);
                    if (dis > this.Search_Tolerrance)
                        return false;
                    else
                        return true;
                }
                else
                {
                    return true;
                }
            }
            else if (SpaRel.Equals(Frompoint2))
            {
                SpaRel = Frompoint1 as IRelationalOperator;
                if (!SpaRel.Equals(Topoint2))
                {
                    double dis = CalculateDistance(Frompoint1, Topoint2);
                    if (dis > this.Search_Tolerrance)
                        return false;
                    else
                        return true;
                }
                else
                {
                    return true;
                }
            }
            //////////////////////////////////////////////
            SpaRel = Frompoint1 as IRelationalOperator;
            if (SpaRel.Equals(Topoint2))
            {
                SpaRel = Topoint1 as IRelationalOperator;
                if (!SpaRel.Equals(Frompoint2))
                {
                    double dis = CalculateDistance(Topoint1, Frompoint2);
                    if (dis > this.Search_Tolerrance)
                        return false;
                    else
                        return true;
                }
                else
                {
                    return true;
                }
            }
            else if (SpaRel.Equals(Frompoint2))
            {
                SpaRel = Topoint1 as IRelationalOperator;
                if (!SpaRel.Equals(Topoint2))
                {
                    double dis = CalculateDistance(Topoint1, Topoint2);
                    if (dis > this.Search_Tolerrance)
                        return false;
                    else
                        return true;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        private bool judgeTwoFeatureField(long OriOId, long DesOid)/////判断两个要素的属性字段是否匹配
        {
            if (this.FieldsControlList == null) return true;
            if (this._DestinatFeaCls == null) return false;
            try
            {
                IFeature OriFeature = this._DestinatFeaCls.GetFeature((int)OriOId);
                IFeature DesFeature = this._DestinatFeaCls.GetFeature((int)DesOid);
                for (int i = 0; i < this.FieldsControlList.Count; i++)
                {
                    string FieldName = this.FieldsControlList[i];
                    int index1 = OriFeature.Fields.FindField(FieldName);
                    if (index1 < 0) return false;
                    int index2 = DesFeature.Fields.FindField(FieldName);
                    if (index2 < 0) return false;
                    if (OriFeature.get_Value(index1) == null && DesFeature.get_Value(index2) == null) continue;
                    if (OriFeature.get_Value(index1) == null && DesFeature.get_Value(index2) != null) return false;
                    if (OriFeature.get_Value(index1) != null && DesFeature.get_Value(index2) == null) return false;
                    if (OriFeature.get_Value(index1).ToString().Trim() != DesFeature.get_Value(index2).ToString().Trim()) return false;
                }
                return true;
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
    }
    class ClsMergeOperationer : IMergeOperation
    {
        private bool _CreatLog;
        public bool CreatLog
        {
            get { return this._CreatLog; }
            set { this._CreatLog = value; }
        }
        public ClsMergeOperationer()/////构造函数
        {
            this._JoinFeaClss = null;
            this._SetDesValueToOri = false;
            this._CreatLog = false;
        }

        private List<IFeatureClass> _JoinFeaClss;
        public List<IFeatureClass> JoinFeaClss
        {
            get { return this._JoinFeaClss; }
            set { this._JoinFeaClss = value; }
        }
        private bool _SetDesValueToOri;
        public bool SetDesValueToOri
        {
            get { return this._SetDesValueToOri; }
            set { this._SetDesValueToOri = value; }
        }
        public bool MergePolyline(string DataSetName, long OriOID, long DesOID)
        {
            IFeatureClass FeaCls = GetFeatureClassByName(DataSetName);
            if (null == FeaCls)
                return false;
            IFeature OriFea = null;
            IFeature DesFea = null;
            try
            {
                OriFea = FeaCls.GetFeature((int)OriOID);
                DesFea = FeaCls.GetFeature((int)DesOID);
            }
            catch
            {
                return false;
            }
            if (null == OriFea || null == DesFea)
                return false;
            if (OriFea.Shape.GeometryType != esriGeometryType.esriGeometryPolyline || DesFea.Shape.GeometryType != esriGeometryType.esriGeometryPolyline)
                return false;
            UnionTwoFeat(DataSetName, OriFea, DesFea, this._SetDesValueToOri);
            return true;
        }
        public bool MergePolygon(string DataSetName, long OriOID, long DesOID)
        {
            IFeatureClass FeaCls = GetFeatureClassByName(DataSetName);
            if (null == FeaCls)
                return false;
            IFeature OriFea = null;
            IFeature DesFea = null;
            try
            {
                OriFea = FeaCls.GetFeature((int)OriOID);
                DesFea = FeaCls.GetFeature((int)DesOID);
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
            if (null == OriFea || null == DesFea)
                return false;
            if (OriFea.Shape.GeometryType != esriGeometryType.esriGeometryPolygon || DesFea.Shape.GeometryType != esriGeometryType.esriGeometryPolygon)
                return false;
            UnionTwoFeat(DataSetName, OriFea, DesFea, this._SetDesValueToOri);
            return true;
        }
        private bool UnionTwoFeat(string DataSetName, IFeature OriFea, IFeature DesFea, bool SetDesValueToOri)
        {
            string UniLogstr = string.Empty;        
            try
            {
                IWorkspaceEdit workspaceEdit = ((IFeatureClass)OriFea.Class as IDataset).Workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                
                IRelationalOperator RelOper = OriFea.ShapeCopy as IRelationalOperator;
                if (!RelOper.Touches(DesFea.ShapeCopy))
                    return false;
                ITopologicalOperator pTopoOperator = OriFea.ShapeCopy as ITopologicalOperator;
                IGeometry pGeometry = pTopoOperator.Union(DesFea.ShapeCopy);
                pTopoOperator = pGeometry as ITopologicalOperator;
                pTopoOperator.Simplify();
                OriFea.Shape = pGeometry;

                IFields pFields = OriFea.Fields;  
                if (SetDesValueToOri)////把目标要素的属性赋予源要素(OID,shape字段除外)
                {
                    SetFieldsValue(ref OriFea, ref DesFea);
                }
                else/////////////////////把目标要素的属性值加到源要素(OID,shape字段除外),字符型字段用逗号分隔，数字型相加
                {
                    AddFieldsValue(ref OriFea, ref DesFea);
                }
                workspaceEdit.StartEditOperation();
                OriFea.Store();
                DesFea.Delete();
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                UniLogstr = "已融合";
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", "要素：" + OriFea.OID+"无法保存，\n请确认数据文件是否被占用。");
                UniLogstr = "未融合";
            }

            ///////日志
            if (this._CreatLog)
            {
                DataTable Table = new DataTable();
                DataColumn dc1 = new DataColumn("数据集", Type.GetType("System.String"));
                DataColumn dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
                DataColumn dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
                dc3.DefaultValue = -1;
                DataColumn dc4 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
                dc4.DefaultValue = -1;
                DataColumn dc5 = new DataColumn("处理结果", Type.GetType("System.String"));

                Table.Columns.Add(dc1);
                Table.Columns.Add(dc2);
                Table.Columns.Add(dc3);
                Table.Columns.Add(dc4);
                Table.Columns.Add(dc5);
                double x = -1;
                double y = -2;
                ///////////////////////////////////////////////////////////////////////////
                DataRow LogRow = Table.NewRow();
                LogRow["数据集"] = DataSetName;
                if (OriFea.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                {
                    LogRow["要素类型"] = "Polyline";
                    x = (OriFea.ShapeCopy as IPolyline).FromPoint.X;
                    y = (OriFea.ShapeCopy as IPolyline).FromPoint.Y;
                }
                else
                {
                    LogRow["要素类型"] = "Polygone";
                    x = (OriFea.ShapeCopy as IPolygon).ToPoint.X;
                    y = (OriFea.ShapeCopy as IPolygon).ToPoint.Y;
                }
                LogRow["源要素ID"] = OriFea.OID;
                LogRow["目标要素ID"] = DesFea.OID;
                LogRow["处理结果"] = UniLogstr;
                Exception ex = null;
                IJoinLOG log = new ClsJoinLog();
                log.OnDataJoin_OnMerge(LogRow, x, y, out ex);

            }

            return true;
        }
        private bool SetFieldsValue(ref IFeature pOriFeat, ref IFeature pDesFeat)/////将pDesFeat的字段赋予pOriFeat
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
        private bool AddFieldsValue(ref IFeature pOriFeat, ref IFeature pDesFeat)
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
                        if (!string.IsNullOrEmpty(pDesFeat.get_Value(IdesFiledIndex).ToString()))
                        {
                            if (pOriFeat.get_Value(i) == null)
                            {
                                if (pOriFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeString)
                                {
                                    pOriFeat.set_Value(i, pDesFeat.get_Value(IdesFiledIndex).ToString());
                                }
                                else
                                {
                                    pOriFeat.set_Value(i, pDesFeat.get_Value(IdesFiledIndex));
                                }
                            }
                            else
                            {
                                if (pOriFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeString)
                                {
                                    if (pOriFeat.get_Value(i).ToString() != pDesFeat.get_Value(IdesFiledIndex).ToString())
                                        pOriFeat.set_Value(i, pOriFeat.get_Value(i).ToString() + "," + pDesFeat.get_Value(IdesFiledIndex).ToString());
                                }
                                else if (pOriFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeDouble)
                                {
                                    try
                                    {
                                        pOriFeat.set_Value(i, Convert.ToDouble(pOriFeat.get_Value(i).ToString()) + Convert.ToDouble(pDesFeat.get_Value(IdesFiledIndex).ToString()));
                                    }
                                    catch
                                    {
                                        pOriFeat.set_Value(i, Convert.ToDouble(pDesFeat.get_Value(IdesFiledIndex).ToString()));
                                    }
                                }
                                else if (pOriFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeSingle)
                                {
                                    try
                                    {
                                        pOriFeat.set_Value(i, Convert.ToSingle(pOriFeat.get_Value(i).ToString()) + Convert.ToSingle(pDesFeat.get_Value(IdesFiledIndex).ToString()));
                                    }
                                    catch
                                    {
                                        pOriFeat.set_Value(i, Convert.ToSingle(pDesFeat.get_Value(IdesFiledIndex).ToString()));
                                    }
                                }
                                else if (pOriFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeInteger)
                                {
                                    try
                                    {
                                        pOriFeat.set_Value(i, Convert.ToInt32(pOriFeat.get_Value(i).ToString()) + Convert.ToInt32(pDesFeat.get_Value(IdesFiledIndex).ToString()));
                                    }
                                    catch
                                    {
                                        pOriFeat.set_Value(i, Convert.ToInt32(pDesFeat.get_Value(IdesFiledIndex).ToString()));
                                    }
                                }
                                else if (pOriFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeSmallInteger)
                                {
                                    try
                                    {
                                        pOriFeat.set_Value(i, Convert.ToInt16(pOriFeat.get_Value(i).ToString()) + Convert.ToInt16(pDesFeat.get_Value(IdesFiledIndex).ToString()));
                                    }
                                    catch
                                    {
                                        pOriFeat.set_Value(i, Convert.ToInt16(pDesFeat.get_Value(IdesFiledIndex).ToString()));
                                    }
                                }
                            }

                        }
                        else
                        {

                        }
                    }
                }
            }
            return true;
        }
        private IFeatureClass GetFeatureClassByName(string FeatureClassName)
        {
            if (this._JoinFeaClss == null)
                return null;
            for (int i = 0; i < this._JoinFeaClss.Count; i++)
            {
                IFeatureClass GetFeaClss = this._JoinFeaClss[i];
                if ((GetFeaClss as IDataset).Name == FeatureClassName)
                    return GetFeaClss;
            }
            return null;
        }
    }
    class ClsJoinOperationer : IJoinOperation
    {
        public ClsJoinOperationer()
        {
            this._CreatLog = false;
            this._JoinFeaClss = null;
        }
        private List<IPolyline> _borderline;/////接边边界
        public List<IPolyline> borderline
        {
            get { return this._borderline; }
            set { this._borderline = value; }
        }
        private bool _CreatLog;
        public bool CreatLog
        {
            get { return this._CreatLog; }
            set { this._CreatLog = value; }
        }
        private List<IFeatureClass> _JoinFeaClss;
        public List<IFeatureClass> JoinFeaClss
        {
            get { return this._JoinFeaClss; }
            set { this._JoinFeaClss = value; }
        }
        public DataTable MovePolylinePnt(DataTable OIDInfo)/////移动线上的点进行接边
        {

            DataTable ResTable = new DataTable();
            DataColumn dc1 = new DataColumn("数据集", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
            DataColumn dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
            dc3.DefaultValue = -1;
            DataColumn dc4 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
            dc4.DefaultValue = -1;
            DataColumn dc5 = new DataColumn("处理结果", Type.GetType("System.String"));

            ResTable.Columns.Add(dc1);
            ResTable.Columns.Add(dc2);
            ResTable.Columns.Add(dc3);
            ResTable.Columns.Add(dc4);
            ResTable.Columns.Add(dc5);
            ///////////////////////////////////////////////////////////////////////////

            if (OIDInfo == null)
                return null;

            for (int i = 0; i < OIDInfo.Rows.Count; i++)
            {
                string DatasetName = OIDInfo.Rows[i]["数据集"].ToString();
                IDataset Destinataset = GetFeatureClassByName(DatasetName) as IDataset;
                if (null == Destinataset)
                    return null;

                long OriOID = -1;
                long DesOID = -1;
                string OriPtn = string.Empty;
                string DesPtn = string.Empty;
                string JoinState = string.Empty;
                try
                {
                    OriOID = Convert.ToInt64(OIDInfo.Rows[i]["源要素ID"]);
                    DesOID = Convert.ToInt64(OIDInfo.Rows[i]["目标要素ID"]);
                    OriPtn = OIDInfo.Rows[i]["OriPtn"].ToString();
                    DesPtn = OIDInfo.Rows[i]["DesPtn"].ToString();
                    JoinState = OIDInfo.Rows[i]["接边状态"].ToString();
                }
                catch
                {
                    continue;
                }
                ////////获取定位坐标
                double x = -1;
                double y = -1;
                if (this._CreatLog)
                {
                    IPolyline getPolyline = ((IFeatureClass)Destinataset).GetFeature((int)OriOID).Shape as IPolyline;
                    if (OriPtn == "ToPoint")
                    {
                        x = getPolyline.ToPoint.X;
                        y = getPolyline.ToPoint.Y;
                    }
                    else
                    {
                        x = getPolyline.FromPoint.X;
                        y = getPolyline.FromPoint.Y;
                    }
                }
                if (JoinState.Substring(0, 3) == "已接边")
                {
                    DataRow newRow = ResTable.NewRow();
                    newRow["数据集"] = DatasetName;
                    newRow["要素类型"] = "Polyline";
                    newRow["源要素ID"] = OriOID;
                    newRow["目标要素ID"] = DesOID;
                    newRow["处理结果"] = "已接边";
                    ResTable.Rows.Add(newRow);
                    if (this._CreatLog)
                    {
                        Exception ex = null;
                        IJoinLOG joinLog = new ClsJoinLog();
                        joinLog.OnDataJoin_OnJoin(newRow, x, y, out ex);
                    }
                }
                if (OriOID == -1)
                    continue;
                bool state = false;
                if (JoinState.Substring(0, 3) == "未接边")
                {
                    state = PolylineDoJoin(Destinataset, OriOID, OriPtn, DesOID, DesPtn);

                    DataRow newRow = ResTable.NewRow();
                    newRow["数据集"] = DatasetName;
                    newRow["要素类型"] = "Polyline";
                    newRow["源要素ID"] = OriOID;
                    newRow["目标要素ID"] = DesOID;
                    if (state)
                        newRow["处理结果"] = "已接边";
                    else
                        newRow["处理结果"] = "未接边";
                    ResTable.Rows.Add(newRow);
                }
            }
            return ResTable;
        }
        public DataTable MovePolygonPnt(DataTable OIDInfo)//////移动多边形上的点进行接边
        {
            DataTable ResTable = new DataTable();
            DataColumn dc1 = new DataColumn("数据集", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
            DataColumn dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
            dc3.DefaultValue = -1;
            DataColumn dc4 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
            dc4.DefaultValue = -1;
            DataColumn dc5 = new DataColumn("处理结果", Type.GetType("System.String"));

            ResTable.Columns.Add(dc1);
            ResTable.Columns.Add(dc2);
            ResTable.Columns.Add(dc3);
            ResTable.Columns.Add(dc4);
            ResTable.Columns.Add(dc5);
            ///////////////////////////////////////////////////////////////////////////
            if (null == OIDInfo)
                return null;
            for (int i = 0; i < OIDInfo.Rows.Count; i++)
            {
                DataRow getrow = OIDInfo.Rows[i];
                string FeaClsname = getrow["数据集"].ToString();
                string FeaType = getrow["要素类型"].ToString();
                long OriOID = -1;
                long OriLineIndex = -1;
                long DesOID = -1;
                long DesLineIndex = -1;
                string JoinState = string.Empty;
                try
                {
                    OriOID = Convert.ToInt64(getrow["源要素ID"].ToString());
                    OriLineIndex = Convert.ToInt64(getrow["OriLineIndex"].ToString());
                    DesOID = Convert.ToInt64(getrow["目标要素ID"].ToString());
                    DesLineIndex = Convert.ToInt64(getrow["DesLineIndex"].ToString());
                    JoinState = getrow["接边状态"].ToString();
                }
                catch (Exception e)
                {
                    //*******************************************************************
                    //guozheng added
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
                    continue;
                }
                ///////获取定位坐标
                double x = -1;
                double y = -1;
                if (this._CreatLog)
                {
                    IPolygon GetPolygon = GetFeatureClassByName(FeaClsname).GetFeature((int)OriOID).ShapeCopy as IPolygon;
                    x = GetPolygon.ToPoint.X;
                    y = GetPolygon.ToPoint.Y;
                }
                if (JoinState == "已接边")
                {
                    DataRow addrow = ResTable.NewRow();
                    addrow["数据集"] = FeaClsname;
                    addrow["要素类型"] = "Polygon";
                    addrow["源要素ID"] = OriOID;
                    addrow["目标要素ID"] = DesOID;
                    addrow["处理结果"] = "已接边";
                    ResTable.Rows.Add(addrow);
                    if (this._CreatLog)
                    {
                        Exception ex = null;
                        IJoinLOG joinLog = new ClsJoinLog();
                        joinLog.OnDataJoin_OnJoin(addrow, x, y, out ex);
                    }
                }
                else
                {
                    if (OriOID == -1 || DesOID == -1)
                        continue;
                    bool bRes = PolygonDoJoin(FeaClsname, OriOID, OriLineIndex, DesOID, DesLineIndex);
                    DataRow addrow = ResTable.NewRow();
                    addrow["数据集"] = FeaClsname;
                    addrow["要素类型"] = "Polygon";
                    addrow["源要素ID"] = OriOID;
                    addrow["目标要素ID"] = DesOID;
                    if (bRes)
                        addrow["处理结果"] = "已接边";
                    else
                        addrow["处理结果"] = "未接边";
                    ResTable.Rows.Add(addrow);
                }
            }
            return ResTable;

        }
        private bool PolygonDoJoin(string DataSetName, long OriFeaOID, long OriLineIndex, long DesFeaOID, long DesLineIndex)
        {

            IFeatureClass FeaCls = GetFeatureClassByName(DataSetName);

            IFeature OriFeature = null;
            IFeature DesFeature = null;
            OriFeature = FeaCls.GetFeature((int)OriFeaOID);
            DesFeature = FeaCls.GetFeature((int)DesFeaOID);
            if (null == OriFeature || null == DesFeature)
                return false;

            IPointCollection OriPoCol = OriFeature.Shape as IPointCollection;
            IPointCollection DesPoCol = DesFeature.Shape as IPointCollection;
            IPolygon Oripolygon = OriFeature.ShapeCopy as IPolygon;
            IPolygon Despolygon = DesFeature.ShapeCopy as IPolygon;

            ISegmentCollection OriSeCol = OriPoCol as ISegmentCollection;
            ISegmentCollection DesSeCol = DesPoCol as ISegmentCollection;
            ISegment Seg = null;
            ILine OriLine = null;
            ILine DesLine = null;
            Seg = OriSeCol.get_Segment((int)OriLineIndex);
            OriLine = Seg as ILine;
            Seg = DesSeCol.get_Segment((int)DesLineIndex);
            DesLine = Seg as ILine;
            if (null == OriLine || null == DesLine)
                return false;
            IPoint OriTopoint = OriLine.ToPoint;
            IPoint DesTopoint = DesLine.ToPoint;
            IPoint DesFrompoint = DesLine.FromPoint;
            //////判断接边情况
            double disto = -1;
            double disfrom = -1;
            disto = this.CalculateDistance(OriTopoint, DesTopoint);
            disfrom = this.CalculateDistance(OriTopoint, DesFrompoint);
            IPoint newpoint = null; ;
            ////////////////////////////开始接边，接边可以实现多种算法，，这里是选择中点,(guozheng 2011-2-22 修改为 选择与接边边界的交点)
            //double x = -1;
            //double y = -1;
            try
            {
                IWorkspaceEdit workspaceEdit = (FeaCls as IDataset).Workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                if (disto >= disfrom)/////Ori的ToPoint接Des的FromPoint
                {
                    IPoint GetPoint = GetInsertPoint(OriLine.ToPoint, DesLine.FromPoint);
                    if (GetPoint == null) return false;
                    //x = (OriLine.ToPoint.X + DesLine.FromPoint.X) / 2;
                    //y = (OriLine.ToPoint.Y + DesLine.FromPoint.Y) / 2;
                    newpoint = new PointClass();
                    //newpoint.PutCoords(x, y);
                    newpoint.PutCoords(GetPoint.X, GetPoint.Y);
                    ReplaceOnePntOfPolygon(OriPoCol, OriLineIndex + 1, newpoint, OriSeCol.SegmentCount);
                    ReplaceOnePntOfPolygon(DesPoCol, DesLineIndex, newpoint, DesSeCol.SegmentCount);
                    ///////////////
                    //x = (OriLine.FromPoint.X + DesLine.ToPoint.X) / 2;
                    //y = (OriLine.FromPoint.Y + DesLine.ToPoint.Y) / 2;
                    newpoint = new PointClass();
                    GetPoint = GetInsertPoint(OriLine.FromPoint, DesLine.ToPoint);
                    if (GetPoint == null) return false;
                    newpoint.PutCoords(GetPoint.X, GetPoint.Y);
                    ReplaceOnePntOfPolygon(OriPoCol, OriLineIndex, newpoint, OriSeCol.SegmentCount);
                    ReplaceOnePntOfPolygon(DesPoCol, DesLineIndex + 1, newpoint, DesSeCol.SegmentCount);
                    OriFeature.Shape = OriPoCol as IPolygon;
                    DesFeature.Shape = DesPoCol as IPolygon;
                    OriFeature.Store();
                    DesFeature.Store();
                }
                else/////Ori的ToPoint接Des的ToPoint
                {
                    //x = (OriLine.ToPoint.X + DesLine.ToPoint.X) / 2;
                    //y = (OriLine.ToPoint.Y + DesLine.ToPoint.Y) / 2;
                    IPoint GetPoint = GetInsertPoint(OriLine.ToPoint, DesLine.ToPoint);
                    if (GetPoint == null) return false;
                    newpoint = new PointClass();
                    newpoint.PutCoords(GetPoint.X, GetPoint.Y);
                    ReplaceOnePntOfPolygon(OriPoCol, OriLineIndex + 1, newpoint, OriSeCol.SegmentCount);
                    ReplaceOnePntOfPolygon(DesPoCol, DesLineIndex + 1, newpoint, DesSeCol.SegmentCount);
                    ///////////////
                    //x = (OriLine.FromPoint.X + DesLine.FromPoint.X) / 2;
                    //y = (OriLine.FromPoint.Y + DesLine.FromPoint.Y) / 2;
                    GetPoint = GetInsertPoint(OriLine.FromPoint, DesLine.FromPoint);
                    if (GetPoint == null) return false;
                    newpoint = new PointClass();
                    newpoint.PutCoords(GetPoint.X, GetPoint.Y);
                    ReplaceOnePntOfPolygon(OriPoCol, OriLineIndex, newpoint, OriSeCol.SegmentCount);
                    ReplaceOnePntOfPolygon(DesPoCol, DesLineIndex, newpoint, DesSeCol.SegmentCount);
                    OriFeature.Shape = OriPoCol as IPolygon;
                    DesFeature.Shape = DesPoCol as IPolygon;
                    OriFeature.Store();
                    DesFeature.Store();

                }
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                return true;
            }
            catch (Exception eError)
            {
                if (ModData.SysLog == null) ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write(eError, null, DateTime.Now);
                return false;
            }
        }
        private bool PolylineDoJoin(IDataset Dataset, long OriFeaOID, string OriPT, long DesFeaOID, string DesPT)////执行线的接边
        {

            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)Dataset.Workspace;
            try////////////////////////////////////////////////////接边算法可以实现多种(最终都是生成NewPoint新的点位)这里是取中点
            {
                IFeatureClass FeaCls = Dataset as IFeatureClass;
                IFeature OriFea = FeaCls.GetFeature((int)OriFeaOID);
                IFeature DesFea = FeaCls.GetFeature((int)DesFeaOID);
                IPolyline OriPolyline = OriFea.Shape as IPolyline;
                IPolyline DesPolyline = DesFea.Shape as IPolyline;
                IPoint OriPoint = null;
                IPoint DesPoint = null;
                IPoint NewPoint = new PointClass();
                if ("FromPoint" == OriPT)
                {
                    OriPoint = OriPolyline.FromPoint;
                }
                else if ("ToPoint" == OriPT)
                {
                    OriPoint = OriPolyline.ToPoint;
                }

                if ("FromPoint" == DesPT)
                {
                    DesPoint = DesPolyline.FromPoint;
                }
                else if ("ToPoint" == DesPT)
                {
                    DesPoint = DesPolyline.ToPoint;
                }

                double x = (OriPoint.X + DesPoint.X) / 2;
                double y = (OriPoint.Y + DesPoint.Y) / 2;
                NewPoint.X = x;
                NewPoint.Y = y;
                /////////////开始接边    ///////这里实现的是取中点算法     
                try
                {
                    workspaceEdit.StartEditing(true);
                    workspaceEdit.StartEditOperation();
                    //IFeatureWorkspace Fw = (IFeatureWorkspace)this._Destinataset.Workspace;
                    //IFeatureClass DesFeaCls = Fw.OpenFeatureClass(Dataset.Name);
                    //IFeature Orif = DesFeaCls.GetFeature((int)OriFeaOID);
                    //IFeature Desf = DesFeaCls.GetFeature((int)DesFeaOID);

                    if ("FromPoint" == OriPT)
                    {
                        OriPolyline.FromPoint = NewPoint;
                    }
                    else if ("ToPoint" == OriPT)
                    {
                        OriPolyline.ToPoint = NewPoint;
                    }

                    if ("FromPoint" == DesPT)
                    {
                        DesPolyline.FromPoint = NewPoint;
                    }
                    else if ("ToPoint" == DesPT)
                    {
                        DesPolyline.ToPoint = NewPoint;
                    }
                    OriFea.Shape = OriPolyline;
                    DesFea.Shape = DesPolyline;
                    OriFea.Store();
                    DesFea.Store();
                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(true);
                }
                catch(Exception eError)
                {
                    if (ModData.SysLog == null) ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(eError, null, DateTime.Now);
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                if (workspaceEdit.IsBeingEdited())
                    workspaceEdit.StopEditing(false);
                return false;
            }
        }
        private double CalculateDistance(IPoint FromPoint, IPoint ToPoint)//////计算两点距离
        {
            if (null == ToPoint || null == FromPoint)
                return -1;
            double x1 = FromPoint.X;
            double y1 = FromPoint.Y;
            double x2 = ToPoint.X;
            double y2 = ToPoint.Y;
            double _x = x2 - x1;
            double _y = y2 - y1;
            double Distance = Math.Sqrt(Math.Pow(_x, 2) + Math.Pow(_y, 2));
            return Distance;
        }
        private bool ReplaceOnePntOfPolygon(IPointCollection pPntCol, long lIndex, IPoint pPnt, long lSegmentCount)////修改多边形中的点进行接边 
        {
            pPntCol.ReplacePoints((int)lIndex, 1, 1, ref pPnt);
            //如果改变的是第一个点,则需要修改最后一个点的坐标
            if (lIndex == 0)
                pPntCol.ReplacePoints((int)lSegmentCount, 1, 1, ref pPnt);

            //如果改变的是最后一个点,则需要修改第一个点的坐标
            if (lIndex == lSegmentCount)
                pPntCol.ReplacePoints(0, 1, 1, ref pPnt);
            return true;
        }
        private IFeatureClass GetFeatureClassByName(string FeatureClassName)
        {
            if (this._JoinFeaClss == null)
                return null;
            for (int i = 0; i < this._JoinFeaClss.Count; i++)
            {
                IFeatureClass GetFeaClss = this._JoinFeaClss[i];
                if ((GetFeaClss as IDataset).Name == FeatureClassName)
                    return GetFeaClss;
            }
            return null;
        }
        //*************************************************
        //测试代码   通过两个接边点与接边边界求交，返回交点作为接边点
        private IPoint GetInsertPoint(IPoint in_FromPt, IPoint in_ToPt)
        {
            if (null == in_FromPt || null == in_ToPt) return null;
            if (this._borderline == null) return null;
            /////////取交点/////////////////////
            IPolyline pLine = new PolylineClass();
            pLine.ToPoint = in_ToPt;
            pLine.FromPoint = in_FromPt;
            foreach (IPolyline GetBorline in this._borderline)
            {
                if (GetBorline == null) continue;
                try
                {
                    pLine.SpatialReference = GetBorline.SpatialReference;
                    ITopologicalOperator pTopologicalOperator1 = pLine as ITopologicalOperator;
                    pTopologicalOperator1.Simplify();
                    IRelationalOperator pRelationalOperator = GetBorline as IRelationalOperator;

                    ITopologicalOperator pTopologicalOperator = GetBorline as ITopologicalOperator;
                    pTopologicalOperator.Simplify();
                    IGeometry GetPoints = pTopologicalOperator.Intersect(pLine, esriGeometryDimension.esriGeometry0Dimension) as IGeometry;
                    if (GetPoints != null)
                    {
                        if (!GetPoints.IsEmpty)
                        {
                            IGeometryCollection GeoCollection = GetPoints as IGeometryCollection;
                            IPoint ResPoint = null;
                            IGeometry Shape = GeoCollection.get_Geometry(0);
                            ResPoint = Shape as IPoint;
                            return ResPoint;
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            return null;
        }
        //*************************************************
    }
    class ClsDataOperationer/////数据处理类（进行多边形多余点的删除、要素Geometry简单化处理）
    {
        public ClsDataOperationer()
        {
            this._Angle_to = 1;
            this._OpeFeaClss = null;
        }
        private double _Angle_to;
        public double Angle_to
        {
            get { return this._Angle_to; }
            set { this._Angle_to = value; }
        }
        private IFeatureClass _OpeFeaClss;
        public IFeatureClass OpeFeaClss
        {
            get { return this._OpeFeaClss; }
            set { this._OpeFeaClss = value; }
        }
        public void RemoveRedundantPntFromPolygon(long OID)
        {
            if (null == this._OpeFeaClss)
                return;

            IFeatureClass FeaCls = this._OpeFeaClss;
            IFeature pFeat = null;
            IPointCollection pOriPntCol = null;
            IPointCollection pNewPntCol = null;
            IZAware pZAware = null;
            bool IsPolygon = false;

            /////如果是复杂面或线,不做处理,因为处理时会出错
            IGeometryCollection pGeometryCol = null;
            if (FeaCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                IsPolygon = true;
            else
                IsPolygon = false;

            pFeat = FeaCls.GetFeature((int)OID);
            pGeometryCol = pFeat.ShapeCopy as IGeometryCollection;
            #region 开始处理
            if (pGeometryCol.GeometryCount == 1)
            {
                pZAware = pFeat.Shape as IZAware;
                if (pFeat.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                {
                    pNewPntCol = new Polyline();
                    pOriPntCol = pFeat.ShapeCopy as IPointCollection;
                }
                else if (pFeat.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    pNewPntCol = new Polygon();
                    pOriPntCol = new Polyline();
                    object missing = Type.Missing;
                    pOriPntCol.AddPointCollection(pFeat.ShapeCopy as IPointCollection);
                }
                if (pZAware.ZAware)
                {
                    pZAware = pNewPntCol as IZAware;
                    pZAware.ZAware = true;
                }
                pNewPntCol = RemoveRedundantPnt(pOriPntCol, IsPolygon);

                if (pNewPntCol != null)
                {
                    try
                    {
                        IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(this._OpeFeaClss as IDataset).Workspace;
                        workspaceEdit.StartEditing(true);
                        workspaceEdit.EnableUndoRedo();
                        IPolygon newpolygon = pNewPntCol as IPolygon;
                        pFeat.Shape = newpolygon;
                        pFeat.Store();
                        workspaceEdit.StopEditing(true);
                    }
                    catch (Exception eError)
                    {
                        if (null == ModData.SysLog) ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(eError);
                    }
                }


            }
            #endregion
        }
        public void GeometrySimplify(long OID)
        {
            if (null == this._OpeFeaClss)
                return;
            IFeatureClass ipFtClass = this._OpeFeaClss;
            IFeature Fea = ipFtClass.GetFeature((int)OID);
            IGeometryCollection GeoCol = Fea.Shape as IGeometryCollection;
            if (GeoCol.GeometryCount > 1)
            {
                ITopologicalOperator Topo = Fea.Shape as ITopologicalOperator;
                Topo.Simplify();
            }
            ipFtClass = null;
            Fea = null;
            GeoCol = null;
        }
        private IPointCollection RemoveRedundantPnt(IPointCollection pOriPntCol, bool IsPolygon)
        {
            IPointCollection pNewPntCol = null;
            if (IsPolygon)
                pNewPntCol = new Polygon();
            else
                pNewPntCol = new Polyline();
            long i = -1;
            ILine pFirstLine = null;
            ILine pSecontLine = null;
            long lSegmentCount = -1;
            ISegmentCollection pSegmentCol = null;
            ISegment pSegment = null;
            bool bIsParallel = false;

            if (pOriPntCol == null)
                return null;


            pSegmentCol = pOriPntCol as ISegmentCollection;
            lSegmentCount = pSegmentCol.SegmentCount;
            object missing = Type.Missing;
            if (lSegmentCount > 1)
            {
                for (i = 0; i <= lSegmentCount - 2; i++)
                {
                    pSegment = pSegmentCol.get_Segment((int)i);
                    if (pSegment.GeometryType == esriGeometryType.esriGeometryLine)
                        pFirstLine = pSegment as ILine;
                    else
                        return null;
                    pSegment = pSegmentCol.get_Segment((int)i + 1);
                    if (pSegment.GeometryType == esriGeometryType.esriGeometryLine)
                        pSecontLine = pSegment as ILine;
                    else
                        return null;

                    bIsParallel = ClsCheckOperationer.JudgeParallel(pFirstLine, pSecontLine, this._Angle_to);

                    if (bIsParallel == false)
                    {
                        if (i == 0)
                        {
                            pNewPntCol.AddPoint(pOriPntCol.get_Point((int)i), ref missing, ref missing);
                            pNewPntCol.AddPoint(pOriPntCol.get_Point((int)i + 1), ref missing, ref missing);
                        }
                        else
                        {
                            pNewPntCol.AddPoint(pOriPntCol.get_Point((int)i + 1), ref missing, ref missing);
                        }

                    }
                    else
                    {
                        if (i == 0)
                            pNewPntCol.AddPoint(pOriPntCol.get_Point((int)i), ref missing, ref missing);


                    }
                }
                //添加最后一个点

                pNewPntCol.AddPoint(pOriPntCol.get_Point((int)i + 1), ref missing, ref missing);

                //如果是多边形,则需要判断第一条边和最后一条边是否平行,
                //如果平行则移除第一个顶点,同时修改最后一个顶点
                if (IsPolygon && pNewPntCol.PointCount > 4)
                {
                    //还需要判断最后一条边和第一条边是否平行,如果平行,需要移除第一个点
                    pSegment = pSegmentCol.get_Segment(0);
                    if (pSegment.GeometryType == esriGeometryType.esriGeometryLine)
                        pFirstLine = pSegment as ILine;

                    pSegment = pSegmentCol.get_Segment((int)lSegmentCount - 1);
                    if (pSegment.GeometryType == esriGeometryType.esriGeometryLine)
                        pSecontLine = pSegment as ILine;

                    bIsParallel = ClsCheckOperationer.JudgeParallel(pFirstLine, pSecontLine, this._Angle_to);
                    if (bIsParallel)
                    {
                        pNewPntCol.RemovePoints(0, 1);
                        IPoint newpoint = pNewPntCol.get_Point(0);
                        pNewPntCol.ReplacePoints(pNewPntCol.PointCount - 1, 1, 1, ref newpoint);
                        //pNewPntCol.SetPoints(0,ref newpoint);
                    }
                }

            }
            pSegmentCol = null;
            return pNewPntCol;
        }
    }

    class ClsJoinLog : IJoinLOG
    {

        private string _Logpath;
        public string LogPath
        {
            get { return this._Logpath; }
            set { this._Logpath = value; }
        }
        /// <summary>
        /// 初始化日志文件
        /// </summary>
        /// <param name="ex"></param>
        public void InitialLog(out Exception ex)
        {
            ex = null;
            if (string.IsNullOrEmpty(ModData.v_JoinSettingXML))
            {
                ex = new Exception("获取接边参数设置文件失败!");
                return;
            }
            if (!File.Exists(ModData.v_JoinSettingXML))
            {
                ex = new Exception("获取接边参数设置文件丢失！请检查：" + ModData.v_JoinSettingXML);
                return;
            }
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(ModData.v_JoinSettingXML);
            if (XmlDoc == null)
            {
                ex = new Exception("获取接边参数设置文件失败!");
                return;
            }
            XmlElement ele = XmlDoc.SelectSingleNode(".//接边设置") as XmlElement;
            ////// 初始化参数
            #region  初始化参数
            string sDisto = ele.GetAttribute("距离容差");
            string sAngelto = ele.GetAttribute("角度容差");
            string slegthto = ele.GetAttribute("长度容差");
            string sSearchto = ele.GetAttribute("搜索容差");
            string sjointype = ele.GetAttribute("接边类型");
            string sIsDelPnt = ele.GetAttribute("删除多边形多余点");
            string sIsSimplify = ele.GetAttribute("简单化要素");

            ele = XmlDoc.SelectSingleNode(".//日志设置") as XmlElement;
            string Logpath = ele.GetAttribute("日志路径");
            if (string.IsNullOrEmpty(Logpath))
            {
                ex = new Exception("日志路径获取失败！");
                return;
            }
            if (File.Exists(Logpath))
            {
                File.Delete(Logpath);
            }
            try
            {
                File.Copy(ModData.v_JoinLOGXML, Logpath);
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                ex = new Exception("初始化接边日志失败！请确认日志模板文件是否存在。");
                return;
            }
            XmlDoc.Load(Logpath);
            ele = XmlDoc.SelectSingleNode(".//检查操作/参数") as XmlElement;
            ele.SetAttribute("距离容差", sDisto);
            ele.SetAttribute("角度容差", sAngelto);
            ele.SetAttribute("长度容差", slegthto);
            ele.SetAttribute("搜索容差", sSearchto);
            ele.SetAttribute("接边类型", sjointype);
            ele.SetAttribute("是否删除多边形多余点", sIsDelPnt);
            ele.SetAttribute("是否要素简单化", sIsSimplify);
            try
            {
                XmlDoc.Save(Logpath);
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                ex = new Exception("接边参数写入接边日志失败！请确日志文件是否存在或为只读状态");
                return;
            }
            #endregion

        }

        public void onDataJoin_JoinDataSet(int state, List<IFeatureClass> DataSetList, out Exception ex)
        {
            ex = null;
            if (DataSetList == null)
            {
                ex = new Exception("没有接边图层");
                return;
            }
            if (string.IsNullOrEmpty(this._Logpath))
            {
                ex = new Exception("日志路径未初始化！");
                return;
            }
            XmlDocument Doc = new XmlDocument();
            Doc.Load(_Logpath);
            if (Doc == null)
            {
                ex = new Exception("日志加载失败!");
                return;
            }
            XmlElement getele = null;
            switch (state)
            {
                case 0://///检查操作
                    getele = Doc.SelectSingleNode(".//检查操作/检查图层") as XmlElement;
                    break;
                case 1://///接边操作
                    getele = Doc.SelectSingleNode(".//接边操作/接边图层") as XmlElement;
                    break;
                case 2:////融合操作
                    getele = Doc.SelectSingleNode(".//融合操作/融合图层") as XmlElement;
                    break;
            }
            if (null == getele)
            {
                ex = new Exception("操作节点获取失败");
                return;
            }
            for (int i = 0; i < DataSetList.Count; i++)
            {
                string sDataSetName = (DataSetList[i] as IDataset).Name;
                string sPath = (DataSetList[i] as IDataset).Workspace.ConnectionProperties.ToString();
                XmlNode addNode = Doc.CreateNode(XmlNodeType.Element, string.Empty, string.Empty);

                XmlAttribute addAttr = null;
                ///////
                addAttr = Doc.CreateAttribute("接边图层");
                addAttr.Value = sDataSetName;
                addNode.Attributes.SetNamedItem(addAttr);
                ///////
                addAttr = Doc.CreateAttribute("数据路径");
                addAttr.Value = sPath;
                addNode.Attributes.SetNamedItem(addAttr);
                getele.AppendChild(addNode);
            }
            try
            {
                Doc.Save(_Logpath);
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                ex = new Exception("接边检查日志写入失败！");
                return;
            }

        }

        public void onDataJoin_Start(int state, out Exception ex)
        {
            ex = null;
            XmlDocument Doc = new XmlDocument();
            if (string.IsNullOrEmpty(this._Logpath))
            {
                //ex = new Exception("日志路径未初始化！");
                //return;

                Doc.Load(ModData.v_JoinSettingXML);
                if (null == Doc)
                {
                    ex = new Exception("读取接边参数配置文件失败");
                    return;
                }
                XmlElement ele = Doc.SelectSingleNode(".//日志设置") as XmlElement;
                this._Logpath = ele.GetAttribute("日志路径");
                if (string.IsNullOrEmpty(this._Logpath))
                {
                    ex = new Exception("日志路径未初始化！");
                    return;
                }
            }
            ////////日志文件不存在则初始化这一日志文件
            if (!File.Exists(_Logpath))
            {
                IJoinLOG JoinLog = new ClsJoinLog();
                JoinLog.InitialLog(out ex);
                if (ex != null)
                {
                    return;
                }
            }
            Doc.Load(_Logpath);
            if (Doc == null)
            {
                ex = new Exception("日志加载失败!");
                return;
            }
            XmlElement getele = null;
            switch (state)
            {
                case 0://///检查操作
                    getele = Doc.SelectSingleNode(".//检查操作") as XmlElement;
                    break;
                case 1://///接边操作
                    getele = Doc.SelectSingleNode(".//接边操作") as XmlElement;
                    break;
                case 2:////融合操作
                    getele = Doc.SelectSingleNode(".//融合操作") as XmlElement;
                    break;
            }
            if (null == getele)
            {
                ex = new Exception("操作节点获取失败");
                return;
            }
            getele.SetAttribute("开始时间", DateTime.Now.ToLongTimeString());
            try
            {
                Doc.Save(_Logpath);
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                ex = new Exception("接边检查日志写入失败！");
                return;
            }
        }

        public void onDataJoin_Terminate(int state, out Exception ex)
        {
            ex = null;
            XmlDocument Doc = new XmlDocument();
            if (string.IsNullOrEmpty(this._Logpath))
            {
                //ex = new Exception("日志路径未初始化！");
                //return;

                Doc.Load(ModData.v_JoinSettingXML);
                if (null == Doc)
                {
                    ex = new Exception("读取接边参数配置文件失败");
                    return;
                }
                XmlElement ele = Doc.SelectSingleNode(".//日志设置") as XmlElement;
                this._Logpath = ele.GetAttribute("日志路径");
                if (string.IsNullOrEmpty(this._Logpath))
                {
                    ex = new Exception("日志路径未初始化！");
                    return;
                }
            }

            Doc.Load(_Logpath);
            if (Doc == null)
            {
                ex = new Exception("日志加载失败!");
                return;
            }
            XmlElement getele = null;
            switch (state)
            {
                case 0://///检查操作
                    getele = Doc.SelectSingleNode(".//检查操作") as XmlElement;
                    break;
                case 1://///接边操作
                    getele = Doc.SelectSingleNode(".//接边操作") as XmlElement;
                    break;
                case 2:////融合操作
                    getele = Doc.SelectSingleNode(".//融合操作") as XmlElement;
                    break;
            }
            if (null == getele)
            {
                ex = new Exception("操作节点获取失败");
                return;
            }
            getele.SetAttribute("结束时间", DateTime.Now.ToLongTimeString());
            try
            {
                Doc.Save(_Logpath);
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                ex = new Exception("接边检查日志写入失败！");
                return;
            }


        }

        public void OnDataJoin_OnCheck(DataRow in_DataRow, double x, double y, out Exception ex)
        {
            ex = null;
            XmlDocument Doc = new XmlDocument();
            if (string.IsNullOrEmpty(this._Logpath))
            {
                //ex = new Exception("日志路径未初始化！");
                //return;

                Doc.Load(ModData.v_JoinSettingXML);
                if (null == Doc)
                {
                    ex = new Exception("读取接边参数配置文件失败");
                    return;
                }
                XmlElement ele = Doc.SelectSingleNode(".//日志设置") as XmlElement;
                this._Logpath = ele.GetAttribute("日志路径");
                if (string.IsNullOrEmpty(this._Logpath))
                {
                    ex = new Exception("日志路径未初始化！");
                    return;
                }
            }

            Doc.Load(_Logpath);
            if (Doc == null)
            {
                ex = new Exception("日志加载失败!");
                return;
            }
            string DataSetName = string.Empty;
            string Geometrytype = string.Empty;
            string Result = string.Empty;
            long OriOID = -1;
            long DesOId = -1;
            try
            {
                DataSetName = in_DataRow["数据集"].ToString().Trim();
                Geometrytype = in_DataRow["要素类型"].ToString().Trim();
                Result = in_DataRow["接边状态"].ToString().Trim();
                OriOID = Convert.ToInt64(in_DataRow["源要素ID"].ToString());
                DesOId = Convert.ToInt64(in_DataRow["目标要素ID"].ToString());
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                ex = new Exception("获取接边信息失败");
                return;
            }

            XmlNode resultNode = null;
            if (Geometrytype == "Polyline")
                resultNode = Doc.SelectSingleNode("//检查操作/检查结果/线要素");
            else if (Geometrytype == "Polygon")
                resultNode = Doc.SelectSingleNode("//检查操作/检查结果/面要素");


            XmlNode addNode = Doc.CreateNode(XmlNodeType.Element, "要素", null);

            XmlAttribute addAttr = null;
            ///////
            addAttr = Doc.CreateAttribute("接边图层");
            addAttr.Value = DataSetName;
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("要素类型");
            addAttr.Value = Geometrytype;
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("源要素OID");
            addAttr.Value = OriOID.ToString();
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("目标要素OID");
            addAttr.Value = DesOId.ToString();
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("定位坐标X");
            addAttr.Value = x.ToString();
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("定位坐标Y");
            addAttr.Value = y.ToString();
            addNode.Attributes.SetNamedItem(addAttr);


            addAttr = Doc.CreateAttribute("检查结果");
            addAttr.Value = Result;
            addNode.Attributes.SetNamedItem(addAttr);
            ///////
            resultNode.AppendChild(addNode);
            try
            {
                Doc.Save(_Logpath);
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                ex = new Exception("接边检查日志写入失败！");
                return;
            }
        }

        public void OnDataJoin_OnJoin(DataRow in_DataRow, double x, double y, out Exception ex)
        {
            ex = null;
            XmlDocument Doc = new XmlDocument();
            if (string.IsNullOrEmpty(this._Logpath))
            {
                //ex = new Exception("日志路径未初始化！");
                //return;

                Doc.Load(ModData.v_JoinSettingXML);
                if (null == Doc)
                {
                    ex = new Exception("读取接边参数配置文件失败");
                    return;
                }
                XmlElement ele = Doc.SelectSingleNode(".//日志设置") as XmlElement;
                this._Logpath = ele.GetAttribute("日志路径");
                if (string.IsNullOrEmpty(this._Logpath))
                {
                    ex = new Exception("日志路径未初始化！");
                    return;
                }
            }
            // XmlDocument Doc = new XmlDocument();
            Doc.Load(_Logpath);
            if (Doc == null)
            {
                ex = new Exception("日志加载失败!");
                return;
            }
            string DataSetName = string.Empty;
            string Geometrytype = string.Empty;
            string Result = string.Empty;
            long OriOID = -1;
            long DesOId = -1;
            try
            {
                DataSetName = in_DataRow["数据集"].ToString().Trim();
                Geometrytype = in_DataRow["要素类型"].ToString().Trim();
                Result = in_DataRow["处理结果"].ToString().Trim();
                OriOID = Convert.ToInt64(in_DataRow["源要素ID"].ToString());
                DesOId = Convert.ToInt64(in_DataRow["目标要素ID"].ToString());
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                ex = new Exception("获取接边信息失败");
                return;
            }

            XmlNode resultNode = null;
            if (Geometrytype == "Polyline")
                resultNode = Doc.SelectSingleNode("//接边操作/接边结果/线要素");
            else if (Geometrytype == "Polygon")
                resultNode = Doc.SelectSingleNode("//接边操作/接边结果/面要素");
            XmlNode addNode = Doc.CreateNode(XmlNodeType.Element, "要素", null);

            XmlAttribute addAttr = null;
            ///////
            addAttr = Doc.CreateAttribute("接边图层");
            addAttr.Value = DataSetName;
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("要素类型");
            addAttr.Value = Geometrytype;
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("源要素OID");
            addAttr.Value = OriOID.ToString();
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("目标要素OID");
            addAttr.Value = DesOId.ToString();
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("定位坐标X");
            addAttr.Value = x.ToString();
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("定位坐标Y");
            addAttr.Value = y.ToString();
            addNode.Attributes.SetNamedItem(addAttr);


            addAttr = Doc.CreateAttribute("接边结果");
            addAttr.Value = Result;
            addNode.Attributes.SetNamedItem(addAttr);
            ///////
            resultNode.AppendChild(addNode);
            try
            {
                Doc.Save(_Logpath);
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                ex = new Exception("接边检查日志写入失败！");
                return;
            }
        }

        public void OnDataJoin_OnMerge(DataRow in_DataRow, double x, double y, out Exception ex)
        {
            ex = null;
            XmlDocument Doc = new XmlDocument();
            if (string.IsNullOrEmpty(this._Logpath))
            {
                //ex = new Exception("日志路径未初始化！");
                //return;

                Doc.Load(ModData.v_JoinSettingXML);
                if (null == Doc)
                {
                    ex = new Exception("读取接边参数配置文件失败");
                    return;
                }
                XmlElement ele = Doc.SelectSingleNode(".//日志设置") as XmlElement;
                this._Logpath = ele.GetAttribute("日志路径");
                if (string.IsNullOrEmpty(this._Logpath))
                {
                    ex = new Exception("日志路径未初始化！");
                    return;
                }
            }
            // XmlDocument Doc = new XmlDocument();
            Doc.Load(_Logpath);
            if (Doc == null)
            {
                ex = new Exception("日志加载失败!");
                return;
            }
            string DataSetName = string.Empty;
            string Geometrytype = string.Empty;
            string Result = string.Empty;
            long OriOID = -1;
            long DesOId = -1;
            try
            {
                DataSetName = in_DataRow["数据集"].ToString().Trim();
                Geometrytype = in_DataRow["要素类型"].ToString().Trim();
                Result = in_DataRow["处理结果"].ToString().Trim();
                OriOID = Convert.ToInt64(in_DataRow["源要素ID"].ToString());
                DesOId = Convert.ToInt64(in_DataRow["目标要素ID"].ToString());
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
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
                ex = new Exception("获取接边信息失败");
                return;
            }

            XmlNode resultNode = null;
            if (Geometrytype == "Polyline")
                resultNode = Doc.SelectSingleNode("//融合操作/融合结果/线要素");
            else if (Geometrytype == "Polygone")
                resultNode = Doc.SelectSingleNode("//融合操作/融合结果/面要素");
            XmlNode addNode = Doc.CreateNode(XmlNodeType.Element, "要素", null);

            XmlAttribute addAttr = null;
            ///////
            addAttr = Doc.CreateAttribute("融合图层");
            addAttr.Value = DataSetName;
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("要素类型");
            addAttr.Value = Geometrytype;
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("源要素OID");
            addAttr.Value = OriOID.ToString();
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("目标要素OID");
            addAttr.Value = DesOId.ToString();
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("定位坐标X");
            addAttr.Value = x.ToString();
            addNode.Attributes.SetNamedItem(addAttr);

            addAttr = Doc.CreateAttribute("定位坐标Y");
            addAttr.Value = y.ToString();
            addNode.Attributes.SetNamedItem(addAttr);


            addAttr = Doc.CreateAttribute("融合结果");
            addAttr.Value = Result;
            addNode.Attributes.SetNamedItem(addAttr);
            ///////
            if (resultNode != null)
            {
                resultNode.AppendChild(addNode);
                try
                {
                    Doc.Save(_Logpath);
                }
                catch (Exception e)
                {
                    //*******************************************************************
                    //guozheng added
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
                    ex = new Exception("接边检查日志写入失败！");
                    return;
                }
            }
        }
    }
}
