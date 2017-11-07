using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

namespace GeoDBATool
{
    public interface IMapframe////图幅结合表
    {
        #region 设置图幅结合表图层
        IFeatureClass MapFrameFea { get; set; }
        #endregion

        #region 设置待接边源图幅
        IFeature OriMapFrame { get; set; }
        #endregion

        #region 获取源图幅
        IFeature GetOriFrame(string OriFrameNoValue, string OriFrameNoField);
        #endregion

        #region 获取源图幅、目标图幅的待接边缓冲范围
        void GetBufferArea(double dBufferRadius, out IGeometry OriBufferArea, out IGeometry DesBufferArea);
        #endregion

        #region 获取源图幅参与接边的边界
        List<IPolyline> Getborderline();
        #endregion
    }


    public interface IDestinatDataset/////要进行接边的图层对象
    {
        bool IsStandardMapFrame { get; set; }////是否是标准图幅接边       

        bool IsRemoveRedundantPnt { get; set; }/////是否删除多边形上多余点

        bool IsGeometrySimplify { get; set; }//////是否进行要素的简单化（针对具有多个Geometry的要素）

        double Angle_to { get; set; }/////角度容差

        #region 设置参与接边的图层
        List<IFeatureClass> JoinFeatureClass { get; set; }
        #endregion

        #region 遍历获取图层
        IFeatureClass TargetFeatureClass(string FeatureClassName);////遍历获取图层
        #endregion

        #region 获取缓冲范围内的接边要素
        Dictionary<string, List<long>> GetFeaturesByGeometry(IGeometry BufferArea, bool isOriArea);
        #endregion


    }

    public interface ICheckOperation/////接边操作
    {
        #region 设置需要接边的图层
        IFeatureClass DestinatFeaCls { get; set; }
        #endregion

        bool CreatLog { get; set; }

        List<IPolyline> borderline { get; set; }/////接边边界

        List<long> OriFeaturesOID { get; set; }////待接边源要素集

        List<long> DesFeaturesOID { get; set; }////待接边目标要素集

        List<string> FieldsControlList { get; set; }////////控制属性字段

        double Dis_Tolerance { get; set; }////距离容差

        double Angel_Tolerrance { get; set; }////角度容差

        double Length_Tolerrance { get; set; }////长度容差

        double Search_Tolerrance { get; set; }////搜索容差

        IGeometry OriBufferArea { get; set; }////源图幅的缓冲范围

        IGeometry DesBufferArea { get; set; }////目标图幅的缓冲范围

        esriGeometryType GetDatasetGeometryType();////获取需要接边的图层的几何类型

        DataTable GetPolylineDesFeatureOIDByOriFeature();////获取待接边目标要素集中要与源要素接边的要素

        DataTable GetPolygonDesFeatureOIDByOriFeature();



        //bool PolylineFilter(IPolyline polyline, bool IsOri);////判断一条线是否满足接边条件（例如端点不在接边缓冲区，起点和终点相连等）




    }

    public interface IJoinOperation
    {
        bool CreatLog { get; set; }
        List<IFeatureClass> JoinFeaClss { get; set; }
        List<IPolyline> borderline { get; set; }/////接边边界
        DataTable MovePolylinePnt(DataTable OIDInfo);/////移动线上的点进行接边
        DataTable MovePolygonPnt(DataTable OIDInfo);//////移动多边形上的点进行接边
    }

    public interface IMergeOperation/////融合操作
    {
        bool CreatLog { get; set; }
        #region 设置需要融合的图层
        List<IFeatureClass> JoinFeaClss { get; set; }
        #endregion

        #region 是否将待接边目标要素的属性信息覆盖源要素的属性信息
        bool SetDesValueToOri { get; set; }
        #endregion

        bool MergePolyline(string DataSetName, long OriOID, long DesOID);
        bool MergePolygon(string DataSetName, long OriOID, long DesOID);
    }



    public interface IJoinLOG/////事件
    {

        void InitialLog(out Exception ex);

        void onDataJoin_JoinDataSet(int state, List<IFeatureClass> DataSetList, out Exception ex);

        void onDataJoin_Start(int state, out Exception ex);

        void onDataJoin_Terminate(int state, out Exception ex);

        void OnDataJoin_OnCheck(DataRow in_DataRow, double x, double y, out Exception ex);

        void OnDataJoin_OnJoin(DataRow in_DataRow, double x, double y, out Exception ex);

        void OnDataJoin_OnMerge(DataRow in_DataRow, double x, double y, out Exception ex);

    }


}
