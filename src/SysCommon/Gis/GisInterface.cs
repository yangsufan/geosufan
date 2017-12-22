using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace Fan.Common.Gis
{
    public interface IGisDB
    {
        #region 获取工作空间
        IWorkspace WorkSpace { get;set;}
        #endregion

        #region 设置工作空间

            #region 异构 设置连接工作空间
            //SDE连接
            bool SetWorkspace(String sServer, String sService, String sDatabase, String sUser, String sPassword,string strVersion,out Exception eError);
            //FDB、PDB连接
            bool SetWorkspace(string sFilePath, Fan.Common.enumWSType wstype, out Exception eError);
            #endregion
        #endregion

        #region 开启、关闭编辑
        bool StartWorkspaceEdit(bool withunodredo, out Exception eError);
        bool EndWorkspaceEdit(bool saveedits, out Exception eError);
        #endregion
    }

    public interface IGisDataSet
    {
        #region 创建
        //创建DEM数据集
        bool CreateDEMDataSet(string name, int vCompressionType, int CompressionQuality, int vResamplingTypes, int vTileHeight, int vTileWidth, out Exception eError);
        //创建DOM数据集
        bool CreateDOMDataSet(string name, out Exception eError);
        #endregion

        #region 读操作
        //获取要素集FeatureClass
        IFeatureClass GetFeatureClass(string feaclassname, out Exception eError);
        //获取要素集FeatureDataset
        IFeatureDataset GetFeatureDataset(string featuredsname, out Exception eError);
        //获取工作空间下指定类型数据集名称
        List<string> GetDatasetNames(IWorkspace pWorkspace, esriDatasetType aDatasetTyp);
        //获取数据库下全部的RD名称
        List<string> GetAllRDNames();
        //获取数据库下全部的RC名称
        List<string> GetAllRCNames();
        //获取数据库下全部的FD名称
        List<string> GetAllFeatureDatasetNames();
        //获取数据库下全部的FC名称
        List<string> GetAllFeatureClassNames(bool bFullName);

        #region 异构 获取FC名称
        //获取数据库下全部的离散FC名称
        List<string> GetFeatureClassNames();
        //获取某一要素集合下FC名称
        List<string> GetFeatureClassNames(string fdname, bool bFullName, out Exception eError);
        List<string> GetFeatureClassNames(IFeatureDatasetName pFeaDsName, bool bFullName);
        #endregion

        #region 异构 获取要素集合FeatureCursor
        //根据条件获取
        IFeatureCursor GetFeatureCursor(string featureclassname, string condition, IGeometry pGeometry, esriSpatialRelEnum pSpatialRel, out Exception eError);
        //根据条件获取,输出获取要素数量
        IFeatureCursor GetFeatureCursor(string featureclassname, string condition, IGeometry pGeometry, esriSpatialRelEnum pSpatialRel, out Exception eError, out int count);
        //根据条件获取,输出获取要素数量及FC中要素总量
        IFeatureCursor GetFeatureCursor(string featureclassname, string condition, IGeometry pGeometry, esriSpatialRelEnum pSpatialRel, out Exception eError, out int count, out int total);
        IFeatureCursor GetFeatureCursor(string featureclassname, string condition, IGeometry pGeometry, string strSpatialRel, out Exception eError, out int count, out int total);

        #endregion

        #region 异构 获取指定要素

        IFeature GetFeature(string featureclassname, string condition, IGeometry pGeometry, esriSpatialRelEnum pSpatialRel, out Exception eError);
        IFeature GetFeature(string featureclassname, string condition, IGeometry pGeometry, string strSpatialRel, out Exception eError);
        IFeature GetFeature(IFeatureClass pFeatCls, string condition, IGeometry pGeometry, esriSpatialRelEnum pSpatialRel, out Exception eError);
        IFeature GetFeature(IFeatureClass pFeatCls, string condition, IGeometry pGeometry, string strSpatialRel, out Exception eError);

        #endregion

        //检查数据集FeatureClass是否存在
        bool CheckFeatureClassExist(string feaclassname, out string FeatureType, out Exception eError);
        //检查数据集Dataset是否存在
        bool CheckDatasetExist(string featuredsname, esriDatasetType aDatasetTyp);
        #endregion

        #region 写操作
        #region 异构 导入DOM数据(RC文件)集合
        bool InputDOMData(string RCDatasetName, List<string> filepaths, out Exception eError);
        bool InputDOMData(string RCDatasetName, string filepaths, out Exception eError);
        #endregion

        #region 异构 导入DEM数据(RD文件)集合
        bool InputDEMData(string RDDatasetName, List<string> filepaths, out Exception eError);
        bool InputDEMData(string RDDatasetName, string filepaths, out Exception eError);
        #endregion

        #region 异构 新建Feature
        bool NewFeature(string objfcname, Dictionary<string, object> values, IGeometry geomtry, bool Edit, out Exception eError);
        bool NewFeature(string objfcname, Dictionary<string, object> values, Dictionary<int, string> dicCoor, bool Edit, out Exception eError);
        bool NewFeature(IFeatureClass objfc, Dictionary<string, object> values, IGeometry geomtry, bool Edit, out Exception eError);
        bool NewFeature(IFeatureClass objfc, Dictionary<string, object> values, Dictionary<int, string> dicCoor, bool Edit, out Exception eError);
        bool NewFeatures(IFeatureClass objfc, IFeatureCursor pfeacursor, Dictionary<string, object> values, bool Edit, bool bIngore, out Exception eError);
        bool NewFeatures(IFeatureClass objfc, IFeatureCursor pfeacursor, List<string> FieldNames, Dictionary<string, object> values, bool Edit, bool bIngore, out Exception eError);
        bool NewFeatures(IFeatureClass objfc, IFeatureCursor pfeacursor, Dictionary<string, string> dicFieldsPair, Dictionary<string, object> values, bool Edit, bool bIngore, out Exception eError);
        bool NewFeatures(string objfcname, IFeatureCursor pfeacursor, Dictionary<string, object> values, bool useOrgFdVal, bool Edit, bool bIngore, out Exception eError);
        bool NewFeatures(string objfcname, IFeatureCursor pfeacursor, List<string> FieldNames, Dictionary<string, object> values, bool Edit, bool bIngore, out Exception eError);
        bool NewFeatures(string objfcname, IFeatureCursor pfeacursor, Dictionary<string, string> dicFieldsPair, Dictionary<string, object> values, bool Edit, bool bIngore, out Exception eError);
        #endregion

        #region 异构 编辑Feature
        bool EditFeature(string objfcname, string condition, Dictionary<string, object> values, Dictionary<int, string> dicCoor, bool Edit, out Exception eError);
        bool EditFeature(string objfcname, string condition, Dictionary<string, object> values, IGeometry geomtry, bool Edit, out Exception eError);
        bool EditFeatures(string objfcname, string condition, Dictionary<string, object> dicValues, bool bEdit, out Exception eError);
        #endregion
        #endregion
    }

    public interface IGisLayer
    {
    }

    public interface IGisTable
    {
        #region 创建
        bool CreateTable(string sTableName, IFields pFields, out Exception eError);
        #endregion

        #region 读操作
        //打开表
        ITable OpenTable(string tablename, out Exception eError);
        //返回表中某个字段的唯一值
        ArrayList GetUniqueValue(string tablename, string fieldname, string condition, out Exception eError);
        //得到某个表的字段类型集合
        Dictionary<string, Type> GetFieldsType(string tablename, out Exception eError);
        //得到数据表的某一条记录
        Dictionary<string, object> GetRow(string tablename, string condition, out Exception eError);
        //得到数据表的某一条记录某一字段值
        object GetFieldValue(string tablename, string keyfieldname, string condition, out Exception eError);
        //得到数据表中符合某一条件的记录数量
        long GetRowCount(string tablename, string condition, out Exception eError);

            #region 异构 得到某个表中符合条件的多条记录
            //获取某字段值,返回Dictionary的KEY为OID
        Dictionary<int, object> GetRows(string tablename, string field, string condition, out Exception eError);
            //获取指定多个字段值,返回Dictionary的KEY为OID
        Dictionary<int, ArrayList> GetRows(string tablename, List<string> fields, string condition, out Exception eError);
            //获取时根据某字段进行排序,返回Dictionary的KEY为OID
        Dictionary<int, ArrayList> GetRows(string tablename, List<string> fields, string condition, string postfixClause, out Exception eError);
            //获取某字段值,返回Dictionary的KEY为指定字段值
        Dictionary<object, object> GetRows(string tablename, string keyField, string field, string condition, out Exception eError);
            //获取指定多个字段值,返回Dictionary的KEY为指定字段值
        Dictionary<object, ArrayList> GetRows(string tablename, string keyField, List<string> fields, string condition, out Exception eError);
            //获取时根据某字段进行排序,返回Dictionary的KEY为指定字段值
        Dictionary<object, ArrayList> GetRows(string tablename, string keyField, List<string> fields, string condition, string postfixClause, out Exception eError);
            #endregion

        //表字段值类型检查
        bool CheckFieldValue(string tablename, string fieldname, object value, out Exception eError);
        #endregion

        #region 写操作
            #region 异构 新建行
        bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, out Exception eError);
        bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, out int objectid, out Exception eError);
        bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, string strOIDField, out Exception eError);
        bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, string strOIDField, out int objectid, out Exception eError);
        bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, string strField, esriFieldType FieldType, out Exception eError);
        bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, string strField, esriFieldType FieldType, out int objectid, out Exception eError);
            #endregion

            #region 异构 编辑行
        bool EditRows(string tablename, string condition, Dictionary<string, object> dicValues, bool bEdit, out Exception eError);
        bool EditRows(string tablename, string condition, Dictionary<string, object> dicValues, bool bEdit, string fieldName, esriFieldType FieldType, out Exception eError);
            #endregion

        //删除行
        bool DeleteRows(string tablename, string condition, out Exception eError);
        #endregion
    }

    public interface ICreateGeoDatabase
    {
        /// <summary>
        /// 加载数据库结构文档
        /// </summary>
        /// <param name="LoadPath">数据库结构文档路径</param>
        /// <returns></returns>
        bool LoadDBShecmaDocument(string LoadPath);

        /// <summary>
        /// 加载空间参考文件
        /// </summary>
        /// <param name="LoadPath">空间参考文件路径</param>
        /// <returns></returns>
        bool LoadSpatialReference(string LoadPath);

        /// <summary>
        /// 设置目标数据库访问参数
        /// </summary>
        /// <param name="Type">数据库类型，PDB，GDB，SDE</param>
        /// <param name="IPoPath">如果是PDB或GDB则填写文件路径，如果是SDE则填写服务器IP</param>
        /// <param name="Intance">如果是SDE则填写sde实例名</param>
        /// <param name="User">如果是SDE则填写用户名</param>
        /// <param name="PassWord">如果是SDE则填写密码</param>
        /// <param name="Version">如果是SDE则填写版本</param>
        /// <returns></returns>
        bool SetDestinationProp(string Type, string IPoPath, string Intance, string User, string PassWord, string Version);

        /// <summary>
        /// 创建库体
        /// </summary>
        /// 
        /// <returns></returns>
        bool CreateDBStruct();

        bool CreateDBStruct(List<string> DSName);

        bool CreateDBStruct(List<string> DSName,out int iScale, out string sDBName);

        //cyf 20110707 modify
        bool CreateDBStruct(out string iScale,out List<string> DSName,System.Windows.Forms.ProgressBar in_ProcBar);
        /// <summary>
        /// 创建远程日志表
        /// </summary>
        /// <param name="pDBType"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        bool CreateSQLTable(string pDBType, out Exception eError);
    }
        
    public interface IGisCommon
    {
        //设置工作空间
        bool SetWorkSpace(XmlElement aElemnet, Fan.Common.enumWSType wstype);

        #region 异构 创建数据表
        bool CreateTable(XmlElement aTableElement);
        bool CreateTable(XmlElement aOIDNode, XmlNodeList aNodeList, string sFeaClassName);
        #endregion

        //创建数据集
        bool CreateFeatureClass(XmlElement aOIDNode, XmlNodeList aNodeList, string sFeaClassName, string SpatialReference);
    }
}
