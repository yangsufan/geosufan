using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ConversionTools;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;

using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.DataManagementTools;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using SCHEMEMANAGERCLASSESLib;
using System.Data.OleDb;
using ESRI.ArcGIS.DataSourcesOleDB;
using ADODB;
using System.Data;

namespace SysCommon.Gis
{

    public class SysGisDB : IGisDB, IDisposable
    {
        private IWorkspace _Workspace;
        public IWorkspace WorkSpace
        {
            get { return _Workspace; }
            set { _Workspace = value; }
        }

        private string _WorkspaceServer;
        public string WorkspaceServer
        {
            get
            {
                return _WorkspaceServer;
            }

        }

        /// <summary>
        /// 设置SDE工作区
        /// </summary>
        /// <param name="sServer">服务器名</param>
        /// <param name="sService">服务名</param>
        /// <param name="sDatabase">数据库名(SQLServer)</param>
        /// <param name="sUser">用户名</param>
        /// <param name="sPassword">密码</param>
        /// <param name="strVersion">SDE版本</param>
        /// <returns>输出错误Exception</returns>
        public bool SetWorkspace(string sServer, string sService, string sDatabase, string sUser, string sPassword, string strVersion, out Exception eError)
        {
            eError = null;
            IPropertySet pPropSet = new PropertySetClass();
            IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
            pPropSet.SetProperty("SERVER", sServer);
            pPropSet.SetProperty("INSTANCE", sService);
            pPropSet.SetProperty("DATABASE", sDatabase);
            pPropSet.SetProperty("USER", sUser);
            pPropSet.SetProperty("PASSWORD", sPassword);
            pPropSet.SetProperty("VERSION", strVersion);

            try
            {
                _Workspace = pSdeFact.Open(pPropSet, 0);
                _WorkspaceServer = sServer;
                pPropSet = null;
                pSdeFact = null;
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 设置PDB、GDB工作区
        /// </summary>
        /// <param name="sFilePath">文件路径</param>
        /// <param name="wstype">工作区类型</param>
        /// <returns>输出错误Exception</returns>
        public bool SetWorkspace(string sFilePath, enumWSType wstype, out Exception eError)
        {
            eError = null;

            try
            {
                IPropertySet pPropSet = new PropertySetClass();
                switch (wstype)
                {
                    case enumWSType.PDB:
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        _Workspace = pAccessFact.Open(pPropSet, 0);
                        pAccessFact = null;
                        break;
                    case enumWSType.GDB:
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        _Workspace = pFileGDBFact.Open(pPropSet, 0);
                        pFileGDBFact = null;
                        break;
                    case enumWSType.SHP:
                        ShapefileWorkspaceFactory pFileSHPFact = new ShapefileWorkspaceFactory();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        _Workspace = pFileSHPFact.Open(pPropSet, 0);
                        pFileSHPFact = null;
                        break;
                }
                _WorkspaceServer = sFilePath;
                pPropSet = null;
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 开启WS的编辑操作
        /// </summary>
        /// <param name="withunodredo">是否支持UndoRedo操作</param>
        /// <returns>输出错误Exception</returns>
        public bool StartWorkspaceEdit(bool withunodredo, out Exception eError)
        {
            IWorkspaceEdit pFeaWS = (IWorkspaceEdit)_Workspace;
            eError = null;
            try
            {
                if (pFeaWS.IsBeingEdited()) return true;
                pFeaWS.StartEditing(withunodredo);
                pFeaWS.StartEditOperation();
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 关闭WS的编辑操作
        /// </summary>
        /// <param name="saveedits">是否保存编辑操作</param>
        /// <returns>输出错误Exception</returns>
        public bool EndWorkspaceEdit(bool saveedits, out Exception eError)
        {
            eError = null;
            try
            {
                IWorkspaceEdit pFeaWS = (IWorkspaceEdit)_Workspace;
                if (!pFeaWS.IsBeingEdited()) return true;

                pFeaWS.StopEditOperation();
                pFeaWS.StopEditing(saveedits);
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 启动事务
        /// </summary>
        /// <param name="eError">输出错误Exception</param>
        /// <returns></returns>
        public bool StartTransaction(out Exception eError)
        {
            eError = null;
            try
            {
                ITransactions pTransactions = (ITransactions)_Workspace;
                if (!pTransactions.InTransaction) pTransactions.StartTransaction();
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveedits">是否提交事务</param>
        /// <param name="eError">输出错误Exception</param>
        /// <returns></returns>
        public bool EndTransaction(bool saveedits, out Exception eError)
        {
            eError = null;
            try
            {
                ITransactions pTransactions = (ITransactions)_Workspace;
                if (saveedits)
                {
                    if (pTransactions.InTransaction) pTransactions.CommitTransaction();
                }
                else
                {
                    if (pTransactions.InTransaction) pTransactions.AbortTransaction();
                }
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 关闭工作区
        /// </summary>
        /// /// <returns>操作结果</returns>
        public bool CloseWorkspace()
        {
            if (_Workspace == null) return true;
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(_Workspace.WorkspaceFactory);
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(_Workspace);
            _Workspace = null;
            return true;
        }

        /// <summary>
        /// 关闭工作区
        /// </summary>
        /// <returns>操作结果</returns>
        public bool CloseWorkspace(bool bRemove)
        {
            if (_Workspace == null) return true;
            if (bRemove == true)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_Workspace.WorkspaceFactory);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_Workspace);
            }
            _Workspace = null;
            return true;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(_Workspace);
            Marshal.ReleaseComObject(_Workspace);
            _Workspace = null;
        }

        #endregion
    }

    public class SysGisDataSet : SysGisDB, IGisDataSet
    {
        public SysGisDataSet()
        {

        }

        public SysGisDataSet(IWorkspace pWorkspace)
        {
            this.WorkSpace = pWorkspace;
        }

        /// <summary>
        /// 创建DEM数据集
        /// </summary>
        /// <param name="name">数据集名称</param>
        /// <param name="vCompressionType">压缩方式</param>
        /// <param name="CompressionQuality">压缩比，仅JPEG方式有效</param>
        /// <param name="vPyramidLevel">金字塔层数</param>
        /// <param name="vResamplingTypes">金字塔采样方式</param>
        /// <param name="vTileHeight">格网高度</param>
        /// <param name="vTileWidth">格网宽度</param>
        /// <returns></returns>
        public bool CreateDEMDataSet(string name, int vCompressionType, int CompressionQuality, int vResamplingTypes, int vTileHeight, int vTileWidth, out Exception eError)
        {
            eError = null;
            IRasterWorkspaceEx pRasterWSEx = (IRasterWorkspaceEx)this.WorkSpace;
            ISpatialReference pSpatialRef = new UnknownCoordinateSystemClass();

            // 创建IRasterStorageDef
            IRasterStorageDef pRasterStorageDef = new RasterStorageDefClass();
            pRasterStorageDef.CompressionType = (esriRasterCompressionType)vCompressionType;
            pRasterStorageDef.CompressionQuality = CompressionQuality;
            pRasterStorageDef.PyramidLevel = 7;
            pRasterStorageDef.PyramidResampleType = (rstResamplingTypes)vResamplingTypes;
            pRasterStorageDef.TileHeight = vTileHeight;
            pRasterStorageDef.TileWidth = vTileWidth;

            // 创建IRasterDef
            IRasterDef pRasterDef = new RasterDefClass();
            pRasterDef.Description = "RasterDataSet";
            pRasterDef.SpatialReference = pSpatialRef;

            // 创建IGeometryDef
            IGeometryDef pGeometryDef = new GeometryDefClass();
            IGeometryDefEdit pGeometryDefEdit = (IGeometryDefEdit)pGeometryDef;

            pGeometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            pGeometryDefEdit.AvgNumPoints_2 = 4;
            pGeometryDefEdit.GridCount_2 = 1;
            pGeometryDefEdit.set_GridSize(0, 1000);
            pGeometryDefEdit.SpatialReference_2 = new UnknownCoordinateSystemClass();
            try
            {
                pRasterWSEx.CreateRasterDataset(name, 3, rstPixelType.PT_LONG, pRasterStorageDef, "DEFAULTS", pRasterDef, pGeometryDef);
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 创建DOM数据集
        /// </summary>
        /// <param name="name">数据集名称</param>
        /// <returns></returns>
        public bool CreateDOMDataSet(string name, out Exception eError)
        {
            eError = null;
            IRasterWorkspaceEx pRasterWSEx = (IRasterWorkspaceEx)this.WorkSpace;
            ISpatialReference pSpatialRef = new UnknownCoordinateSystemClass();

            //创建对应的Fields
            IField pField;
            IFieldEdit pFieldEdit;

            IFields pFields = new FieldsClass();
            IFieldsEdit pFieldsEdit = (IFieldsEdit)pFields;

            //创建OID字段
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "ObjectID";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;

            pFieldsEdit.AddField(pField);

            //创建Name字段
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "Name";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;

            pFieldsEdit.AddField(pField);

            //创建Shape字段
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "Shape";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            // 创建IGeometryDef
            IGeometryDef pGeometryDef = new GeometryDefClass();
            IGeometryDefEdit pGeometryDefEdit = (IGeometryDefEdit)pGeometryDef;
            pGeometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            pGeometryDefEdit.AvgNumPoints_2 = 4;
            pGeometryDefEdit.GridCount_2 = 1;
            pGeometryDefEdit.set_GridSize(0, 1000);
            pGeometryDefEdit.SpatialReference_2 = pSpatialRef;

            pFieldEdit.GeometryDef_2 = pGeometryDef;
            pFieldsEdit.AddField(pField);

            //创建Raster字段
            IField2 pField2 = new FieldClass();
            IFieldEdit2 pFieldEdit2 = (IFieldEdit2)pField2;
            pFieldEdit2.Name_2 = "Raster";
            pFieldEdit2.Type_2 = esriFieldType.esriFieldTypeRaster;

            // 创建IRasterDef
            IRasterDef pRasterDef = new RasterDefClass();
            pRasterDef.Description = "it is a raster catalog";
            pRasterDef.SpatialReference = pSpatialRef;

            pFieldEdit2.RasterDef = pRasterDef;
            pFieldsEdit.AddField(pField2);

            try
            {
                pRasterWSEx.CreateRasterCatalog(name, pFields, "Shape", "Raster", "defaults");
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 获取要素集FeatureClass
        /// </summary>
        /// <param name="feaclassname">要素集名</param>
        /// <returns></returns>
        public IFeatureClass GetFeatureClass(string feaclassname, out Exception eError)
        {
            eError = null;
            //得到FeatrueWS
            IFeatureWorkspace pFeaWS = (IFeatureWorkspace)this.WorkSpace;
            //打开FeaClass
            try
            {   //要素集可能不存在，做一次保护
                return pFeaWS.OpenFeatureClass(feaclassname);
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 获取要素集FeatureDataset
        /// </summary>
        /// <param name="featuredsname">要素集名</param>
        /// <returns></returns>
        public IFeatureDataset GetFeatureDataset(string featuredsname, out Exception eError)
        {
            eError = null;
            //得到FeatrueWS
            IFeatureWorkspace pFeaWS = (IFeatureWorkspace)this.WorkSpace;
            //打开FeatureDataset
            try
            {   //要素集可能不存在，做一次保护
                return pFeaWS.OpenFeatureDataset(featuredsname);
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }

        public IRasterDataset GetRasterDataset(string name, out Exception eError)
        {
            eError = null;

            IRasterWorkspaceEx pRasterWSEx = (IRasterWorkspaceEx)this.WorkSpace;
            try
            {   //要素集可能不存在，做一次保护
                return pRasterWSEx.OpenRasterDataset(name);
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 获取工作空间下指定类型数据集名称
        /// </summary>
        /// <param name="pWorkspace">工作空间</param>
        /// <param name="aDatasetTyp">数据集类型</param>
        /// <returns></returns>
        public List<string> GetDatasetNames(IWorkspace pWorkspace, esriDatasetType aDatasetTyp)
        {
            try
            {
                List<string> DatasetNames = new List<string>();
                IEnumDatasetName pEnumDatasetName = pWorkspace.get_DatasetNames(aDatasetTyp);
                IDatasetName pDatasetName = pEnumDatasetName.Next();
                while (pDatasetName != null)
                {
                    //deleted by chulili 20110915 暂时注释掉  “不在本用户的数据集不添加”，后续修改成根据配置文件判断
                    //if (pWorkspace.WorkspaceFactory is SdeWorkspaceFactoryClass)//add by xisheng 20110906
                    //{
                    //    object name = pWorkspace.ConnectionProperties.GetProperty("USER");

                    //    string[] strTemp = pDatasetName.Name.Split('.');
                    //    if (strTemp[0].Trim().ToUpper() != name.ToString().ToUpper())
                    //    {
                    //        pDatasetName = pEnumDatasetName.Next();
                    //        continue;
                    //    }
                    //}
                    //end deleted by chulili 20110915
                    DatasetNames.Add(pDatasetName.Name);
                    pDatasetName = pEnumDatasetName.Next();

                }
                return DatasetNames;
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取工作空间下指定类型数据集名称
        /// </summary>
        /// <param name="pWorkspace">工作空间</param>
        /// <param name="aDatasetTyp">数据集类型</param>
        /// <param name="bHasUserName">如果为SDE,是否包含用户名</param>
        /// <returns></returns>
        public List<string> GetDatasetNames(IWorkspace pWorkspace, esriDatasetType aDatasetTyp, bool bHasUserName)
        {
            List<string> DatasetNames = new List<string>();
            IEnumDatasetName pEnumDatasetName = pWorkspace.get_DatasetNames(aDatasetTyp);
            IDatasetName pDatasetName = pEnumDatasetName.Next();
            object name = pWorkspace.ConnectionProperties.GetProperty("USER");
            
            while (pDatasetName != null)
            {
                string[] strTemp = pDatasetName.Name.Split('.');
                if (strTemp[0].Trim().ToUpper() != name.ToString().ToUpper())
                {
                    pDatasetName = pEnumDatasetName.Next();
                    continue;
                }
                if (bHasUserName == true)
                {
                    DatasetNames.Add(pDatasetName.Name);
                }
                else
                {
                   
                    DatasetNames.Add(strTemp[1]);
                }
                pDatasetName = pEnumDatasetName.Next();
            }
            return DatasetNames;
        }

        /// <summary>
        /// 获取数据库下全部的RD名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllRDNames()
        {
            return GetDatasetNames(this.WorkSpace, esriDatasetType.esriDTRasterDataset);

        }

        /// <summary>
        /// 获取数据库下全部的RC名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllRCNames()
        {
            return GetDatasetNames(this.WorkSpace, esriDatasetType.esriDTRasterCatalog);
        }

        /// <summary>
        /// 获取数据库下全部的FD名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFeatureDatasetNames()
        {
            return GetDatasetNames(this.WorkSpace, esriDatasetType.esriDTFeatureDataset);
        }

        /// <summary>
        /// 获取数据库下全部的FC名称
        /// </summary>
        /// <param name="bFullName">FC名称是否为FD.Name + "\" +FC.Name</param>
        /// <returns></returns>
        public List<string> GetAllFeatureClassNames(bool bFullName)
        {
            List<string> FCNames = new List<string>();

            //得到全部游离的FC名称
            List<string> LsFCNames = GetFeatureClassNames();
            if (LsFCNames != null)
            {
                if (LsFCNames.Count > 0)
                {
                    FCNames.AddRange(LsFCNames);
                }
            }

            //得到要素集合下全部FC名称
            IEnumDatasetName pEnumDsName = this.WorkSpace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName pDsName = pEnumDsName.Next();
            while (pDsName != null)
            {
                IFeatureDatasetName pFeatureDsName = (IFeatureDatasetName)pDsName;
                List<string> FdFCNames = GetFeatureClassNames(pFeatureDsName, bFullName);
                if (FdFCNames != null)
                {
                    if (FdFCNames.Count > 0)
                    {
                        FCNames.AddRange(FdFCNames);
                    }
                }
                pDsName = pEnumDsName.Next();
            }

            return FCNames;
        }

        /// <summary>
        /// 获取数据库下全部的FC名称
        /// </summary>
        /// <param name="bFullName">FC名称是否为FD.Name + "\" +FC.Name</param>
        /// <param name="bHasUserName">如果为SDE,则是否加用户名</param>
        /// <returns></returns>
        public List<string> GetAllFeatureClassNames(bool bFullName, bool bHasUserName)
        {
            List<string> FCNames = new List<string>();

            //得到全部游离的FC名称
            List<string> LsFCNames = GetFeatureClassNames(bHasUserName);
            if (LsFCNames != null)
            {
                if (LsFCNames.Count > 0)
                {
                    FCNames.AddRange(LsFCNames);
                }
            }

            //得到要素集合下全部FC名称
            IEnumDatasetName pEnumDsName = this.WorkSpace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName pDsName = pEnumDsName.Next();
            object name = this.WorkSpace.ConnectionProperties.GetProperty("USER");
            while (pDsName != null)
            {
                string[] strTemp = pDsName.Name.Split('.');
                if (strTemp[0].Trim().ToUpper() != name.ToString().ToUpper())
                {
                    pDsName = pEnumDsName.Next();
                    continue;
                }
                IFeatureDatasetName pFeatureDsName = (IFeatureDatasetName)pDsName;
                List<string> FdFCNames = GetFeatureClassNames(pFeatureDsName, bFullName, bHasUserName);
                if (FdFCNames != null)
                {
                    if (FdFCNames.Count > 0)
                    {
                        FCNames.AddRange(FdFCNames);
                    }
                }
                pDsName = pEnumDsName.Next();
            }

            return FCNames;
        }

        /// <summary>
        /// 获取工作空间下指定类型数据集
        /// </summary>
        /// <param name="pWorkspace">工作空间</param>
        /// <param name="aDatasetTyp">数据集类型</param>
        /// <returns></returns>
        public List<IDataset> GetDatasets(IWorkspace pWorkspace, esriDatasetType aDatasetTyp)
        {
            List<IDataset> Datasets = new List<IDataset>();
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(aDatasetTyp);
            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {
                Datasets.Add(pDataset);
                pDataset = pEnumDataset.Next();
            }
            return Datasets;
        }

        /// <summary>
        /// 获取工作空间下指定类型数据集
        /// </summary>
        /// <param name="pWorkspace">工作空间</param>
        /// <param name="aDatasetTyp">数据集类型</param>
        /// <returns></returns>
        public List<IDataset> GetDatasets1(IWorkspace pWorkspace, esriDatasetType aDatasetTyp)
        {
            List<IDataset> Datasets = new List<IDataset>();
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(aDatasetTyp);
            if (pWorkspace.WorkspaceFactory is SdeWorkspaceFactoryClass)//add by xisheng 20110906
            {
                object name = pWorkspace.ConnectionProperties.GetProperty("USER");

              
                IDataset pDataset = pEnumDataset.Next();
                while (pDataset != null)
                {
                    string[] strTemp = pDataset.Name.Split('.');
                    if (strTemp[0].Trim().ToUpper() != name.ToString().ToUpper())
                    {
                        pDataset = pEnumDataset.Next();
                        continue;
                    }
                    Datasets.Add(pDataset);
                    pDataset = pEnumDataset.Next();
                }
            }
            else
            {
                IDataset pDataset = pEnumDataset.Next();
                while (pDataset != null)
                {
                    Datasets.Add(pDataset);
                    pDataset = pEnumDataset.Next();
                }
            }
            return Datasets;
        }

        /// <summary>
        /// 获取数据库下全部的FC
        /// </summary>
        /// <returns></returns>
        public List<IDataset> GetAllFeatureClass()
        {
            List<IDataset> listFC = new List<IDataset>();

            //得到全部游离的FC名称
            List<IDataset> LsFC = GetDatasets(this.WorkSpace, esriDatasetType.esriDTFeatureClass);
            if (LsFC != null)
            {
                if (LsFC.Count > 0)
                {
                    listFC.AddRange(LsFC);
                }
            }

            //得到要素集合下全部FC名称
            IEnumDataset pEnumDs = this.WorkSpace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            IDataset pDs = pEnumDs.Next();
            while (pDs != null)
            {
                IFeatureDataset pFeatureDs = (IFeatureDataset)pDs;
                List<IDataset> FdFCs = GetFeatureClass(pFeatureDs);
                if (FdFCs != null)
                {
                    if (FdFCs.Count > 0)
                    {
                        listFC.AddRange(FdFCs);
                    }
                }
                pDs = pEnumDs.Next();
            }

            return listFC;
        }
        /// <summary>
        /// 获取某一要素集合下FC
        /// </summary>
        /// <param name="pFeaDsName">要素集IFeatureDataset</param>
        /// <returns></returns>
        public List<IDataset> GetFeatureClass(IFeatureDataset pFeaDs)
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

        /// <summary>
        /// 获取数据库下全部的离散FC名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetFeatureClassNames()
        {
            return GetDatasetNames(this.WorkSpace, esriDatasetType.esriDTFeatureClass);
        }

        /// <summary>
        /// 获取数据库下全部的离散FC名称
        /// </summary>
        /// <param name="bHasUserName">如果为SDE,是否包含用户名</param>
        /// <returns></returns>
        public List<string> GetFeatureClassNames(bool bHasUserName)
        {
            return GetDatasetNames(this.WorkSpace, esriDatasetType.esriDTFeatureClass, bHasUserName);
        }

        /// <summary>
        /// 获取某一要素集合下FC名称
        /// </summary>
        /// <param name="fdname">要素集FeatureDataset名称</param>
        /// <param name="bFullName">FC名称是否为FD.Name + "\" +FC.Name</param>
        /// <returns></returns>
        public List<string> GetFeatureClassNames(string fdname, bool bFullName, out Exception eError)
        {
            eError = null;
            List<string> FCNames = new List<string>();

            // 得到对应的FeatureDs
            IFeatureDataset pFeatureDs = GetFeatureDataset(fdname, out eError);
            if (pFeatureDs == null) return null;

            IFeatureDatasetName pFeaDsName = (IFeatureDatasetName)(pFeatureDs.FullName);
            IEnumDatasetName pEnumDsName = pFeaDsName.FeatureClassNames;
            IDatasetName pDsName = pEnumDsName.Next();
            while (pDsName != null)
            {
                string strDSName = pDsName.Name;
                //shduan 20110718 delete**************************************************
                //if (strDSName.Contains("."))
                //{
                //    strDSName = strDSName.Substring(strDSName.IndexOf(".") + 1);
                //}
                //shduan 20110718 delete**************************************************
                if (bFullName == true)
                {
                    FCNames.Add(pFeatureDs.Name + "\\" + strDSName);
                }
                else
                {
                    FCNames.Add(strDSName);
                }
                pDsName = pEnumDsName.Next();
            }
            return FCNames;
        }

        /// <summary>
        /// 获取某一要素集合下FC名称
        /// </summary>
        /// <param name="pFeaDsName">要素集IFeatureDatasetName</param>
        /// <param name="bFullName">FC名称是否为FD.Name + "\" +FC.Name</param>
        /// <returns></returns>
        public List<string> GetFeatureClassNames(IFeatureDatasetName pFeaDsName, bool bFullName)
        {
            List<string> FCNames = new List<string>();

            IEnumDatasetName pEnumDsName = pFeaDsName.FeatureClassNames;
            IDatasetName pDsName = pEnumDsName.Next();
            while (pDsName != null)
            {
                if (bFullName == true)
                {
                    IDatasetName pName = (IDatasetName)pFeaDsName;
                    FCNames.Add(pName.Name + "\\" + pDsName.Name);
                }
                else
                {
                    FCNames.Add(pDsName.Name);
                }
                pDsName = pEnumDsName.Next();
            }
            return FCNames;
        }

        /// <summary>
        /// 获取某一要素集合下FC名称
        /// </summary>
        /// <param name="pFeaDsName">要素集IFeatureDatasetName</param>
        /// <param name="bFullName">FC名称是否为FD.Name + "\" +FC.Name</param>
        /// <param name="bHasUserName">如果为SDE,是否包含用户名</param>
        /// <returns></returns>
        public List<string> GetFeatureClassNames(IFeatureDatasetName pFeaDsName, bool bFullName, bool bHasUserName)
        {
            List<string> FCNames = new List<string>();
            IEnumDatasetName pEnumDsName = null;
            try
            {
                pEnumDsName = pFeaDsName.FeatureClassNames;
            }
            catch { return FCNames; }
            IDatasetName pDsName = pEnumDsName.Next();
            object name = this.WorkSpace.ConnectionProperties.GetProperty("USER");
            while (pDsName != null)
            {
                string[] strTemp = pDsName.Name.Split('.');
                if (strTemp[0].Trim().ToUpper() != name.ToString().ToUpper())
                {
                    pDsName = pEnumDsName.Next();
                    continue;
                }
                if (bFullName == true)
                {
                    IDatasetName pName = (IDatasetName)pFeaDsName;
                    if (bHasUserName == true)
                    {
                        FCNames.Add(pName.Name + "\\" + pDsName.Name);
                    }
                    else
                    {
                        string[] strTemp1 = pName.Name.Split('.');
                        FCNames.Add(strTemp1[1] + "\\" + strTemp[1]);
                    }
                }
                else
                {
                    if (bHasUserName == true)
                    {
                        FCNames.Add(pDsName.Name);
                    }
                    else
                    {
                        FCNames.Add(strTemp[1]);
                    }
                }
                pDsName = pEnumDsName.Next();
            }
            return FCNames;
        }

        /// <summary>
        /// 根据条件获取要素集合FeatureCursor
        /// </summary>
        /// <param name="featureclassname">FC名称</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public IFeatureCursor GetFeatureCursor(string featureclassname, string condition, out Exception eError)
        {
            eError = null;
            IFeatureClass pFeaClass = GetFeatureClass(featureclassname, out eError);
            if (pFeaClass == null) return null;

            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = condition;

            try
            {
                IFeatureCursor pFeaCursor = pFeaClass.Search(pQueryFilter, false);
                return pFeaCursor;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }
        /// <summary>
        /// 根据条件获取要素集合FeatureCursor
        /// </summary>
        /// <param name="featureclassname">FC名称</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public IFeatureCursor GetFeatureCursor(string featureclassname, string condition, IGeometry pGeometry, esriSpatialRelEnum pSpatialRel, out Exception eError)
        {
            eError = null;
            IFeatureClass pFeaClass = GetFeatureClass(featureclassname, out eError);
            if (pFeaClass == null) return null;

            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.WhereClause = condition;
            if (pGeometry != null)
            {
                pSpatialFilter.Geometry = pGeometry;
                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel = pSpatialRel;
            }

            try
            {
                IFeatureCursor pFeaCursor = pFeaClass.Search(pSpatialFilter, false);
                return pFeaCursor;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 根据条件获取,输出获取要素数量
        /// </summary>
        /// <param name="featureclassname">FC名称</param>
        /// <param name="condition">条件</param>
        /// <param name="count">获取要素数量</param>
        /// <returns></returns>
        public IFeatureCursor GetFeatureCursor(string featureclassname, string condition, IGeometry pGeometry, esriSpatialRelEnum pSpatialRel, out Exception eError, out int count)
        {
            count = -1;
            eError = null;
            IFeatureClass pFeaClass = GetFeatureClass(featureclassname, out eError);
            if (pFeaClass == null) return null;
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.WhereClause = condition;
            if (pGeometry != null)
            {
                pSpatialFilter.Geometry = pGeometry;
                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel = pSpatialRel;
            }
            try
            {
                count = pFeaClass.FeatureCount(pSpatialFilter);
                IFeatureCursor pFeaCursor = pFeaClass.Search(pSpatialFilter, false);
                return pFeaCursor;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                count = -1;
                return null;
            }
        }

        /// <summary>
        /// 根据条件获取,输出获取要素数量及FC中要素总量
        /// </summary>
        /// <param name="featureclassname">FC名称</param>
        /// <param name="condition">条件</param>
        /// <param name="count">获取要素数量</param>
        /// <param name="total">FC中要素总量</param>
        /// <returns></returns>
        public IFeatureCursor GetFeatureCursor(string featureclassname, string condition, IGeometry pGeometry, esriSpatialRelEnum pSpatialRel, out Exception eError, out int count, out int total)
        {
            count = -1;
            total = -1;
            eError = null;

            IFeatureClass pFeaClass = GetFeatureClass(featureclassname, out eError);
            if (pFeaClass == null) return null;
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.WhereClause = condition;
            if (pGeometry != null)
            {
                pSpatialFilter.Geometry = pGeometry;
                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel = pSpatialRel;
            }

            try
            {
                count = pFeaClass.FeatureCount(pSpatialFilter);
                total = ModGisPub.GetFeatureCount(pFeaClass,null);
                IFeatureCursor pFeaCursor = pFeaClass.Search(pSpatialFilter, false);
                return pFeaCursor;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                count = -1;
                total = -1;
                return null;
            }
        }

        /// <summary>
        /// 根据条件获取,输出获取要素数量及FC中要素总量
        /// </summary>
        /// <param name="featureclassname">FC名称</param>
        /// <param name="condition">条件</param>
        /// <param name="count">获取要素数量</param>
        /// <param name="total">FC中要素总量</param>
        /// <returns></returns>
        public IFeatureCursor GetFeatureCursor(string featureclassname, string condition, IGeometry pGeometry, string strSpatialRel, out Exception eError, out int count, out int total)
        {
            count = -1;
            total = -1;
            eError = null;

            IFeatureClass pFeaClass = GetFeatureClass(featureclassname, out eError);
            if (pFeaClass == null) return null;
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.WhereClause = condition;
            if (pGeometry != null)
            {
                pSpatialFilter.Geometry = pGeometry;
                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpatialFilter.SpatialRelDescription = strSpatialRel;
            }

            try
            {
                count = pFeaClass.FeatureCount(pSpatialFilter);
                total = ModGisPub.GetFeatureCount(pFeaClass, null);
                IFeatureCursor pFeaCursor = pFeaClass.Search(pSpatialFilter, false);
                return pFeaCursor;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                count = -1;
                total = -1;
                return null;
            }
        }

        /// <summary>
        /// 根据条件获取指定要素
        /// </summary>
        /// <param name="featureclassname">FC名称</param>
        /// <param name="condition">属性条件</param>
        /// <param name="pGeometry">空间图形条件</param>
        /// <param name="pSpatialRel">空间关系</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public IFeature GetFeature(string featureclassname, string condition, IGeometry pGeometry, esriSpatialRelEnum pSpatialRel, out Exception eError)
        {
            eError = null;
            IFeatureClass pFeaClass = GetFeatureClass(featureclassname, out eError);
            if (pFeaClass == null) return null;
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.WhereClause = condition;
            if (pGeometry != null)
            {
                pSpatialFilter.Geometry = pGeometry;
                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel = pSpatialRel;
            }
            try
            {
                IFeatureCursor pFeaCursor = pFeaClass.Search(pSpatialFilter, false);
                IFeature pFeat = pFeaCursor.NextFeature();
                Marshal.ReleaseComObject(pFeaCursor);
                return pFeat;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 根据条件获取指定要素
        /// </summary>
        /// <param name="featureclassname">FC名称</param>
        /// <param name="condition">属性条件</param>
        /// <param name="pGeometry">空间图形条件</param>
        /// <param name="strSpatialRel">空间关系</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public IFeature GetFeature(string featureclassname, string condition, IGeometry pGeometry, string strSpatialRel, out Exception eError)
        {
            eError = null;
            IFeatureClass pFeaClass = GetFeatureClass(featureclassname, out eError);
            if (pFeaClass == null) return null;
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.WhereClause = condition;
            if (pGeometry != null)
            {
                pSpatialFilter.Geometry = pGeometry;
                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpatialFilter.SpatialRelDescription = strSpatialRel;
            }
            try
            {
                IFeatureCursor pFeaCursor = pFeaClass.Search(pSpatialFilter, false);
                IFeature pFeat = pFeaCursor.NextFeature();
                Marshal.ReleaseComObject(pFeaCursor);
                return pFeat;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 根据条件获取指定要素
        /// </summary>
        /// <param name="pFeatCls">FC</param>
        /// <param name="condition">属性条件</param>
        /// <param name="pGeometry">空间图形条件</param>
        /// <param name="pSpatialRel">空间关系</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public IFeature GetFeature(IFeatureClass pFeaClass, string condition, IGeometry pGeometry, esriSpatialRelEnum pSpatialRel, out Exception eError)
        {
            eError = null;
            if (pFeaClass == null) return null;
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.WhereClause = condition;
            if (pGeometry != null)
            {
                pSpatialFilter.Geometry = pGeometry;
                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel = pSpatialRel;
            }
            try
            {
                IFeatureCursor pFeaCursor = pFeaClass.Search(pSpatialFilter, false);
                IFeature pFeat = pFeaCursor.NextFeature();
                Marshal.ReleaseComObject(pFeaCursor);
                return pFeat;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 根据条件获取指定要素
        /// </summary>
        /// <param name="pFeatCls">FC</param>
        /// <param name="condition">属性条件</param>
        /// <param name="pGeometry">空间图形条件</param>
        /// <param name="strSpatialRel">空间关系</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public IFeature GetFeature(IFeatureClass pFeaClass, string condition, IGeometry pGeometry, string strSpatialRel, out Exception eError)
        {
            eError = null;
            if (pFeaClass == null) return null;
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.WhereClause = condition;
            if (pGeometry != null)
            {
                pSpatialFilter.Geometry = pGeometry;
                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpatialFilter.SpatialRelDescription = strSpatialRel;
            }
            try
            {
                IFeatureCursor pFeaCursor = pFeaClass.Search(pSpatialFilter, false);
                IFeature pFeat = pFeaCursor.NextFeature();
                Marshal.ReleaseComObject(pFeaCursor);
                return pFeat;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 检查数据集FeatureClass是否存在
        /// </summary>
        /// <param name="feaclassname">FC名称</param>
        /// <param name="FeatureType">FC类型</param>
        /// <returns></returns>
        public bool CheckFeatureClassExist(string feaclassname, out string FeatureType, out Exception eError)
        {
            FeatureType = string.Empty;
            eError = null;
            IFeatureClass pFeatureClass = GetFeatureClass(feaclassname, out eError);
            if (pFeatureClass == null) return false;

            switch (pFeatureClass.FeatureType)
            {
                case esriFeatureType.esriFTSimple:
                    switch (pFeatureClass.ShapeType)
                    {
                        case esriGeometryType.esriGeometryPoint:
                            FeatureType = "点";
                            break;
                        case esriGeometryType.esriGeometryPolyline:
                            FeatureType = "线"; ;
                            break;
                        case esriGeometryType.esriGeometryPolygon:
                            FeatureType = "面";
                            break;
                    }

                    break;
                case esriFeatureType.esriFTAnnotation:
                case esriFeatureType.esriFTDimension:
                    FeatureType = "注记";
                    break;
            }

            return true;
        }

        /// <summary>
        /// 检查数据集Dataset是否存在
        /// </summary>
        /// <param name="featuredsname">Dataset名称</param>
        /// <param name="aDatasetTyp">Dataset类型</param>
        /// <returns></returns>
        public bool CheckDatasetExist(string Datasetname, esriDatasetType aDatasetTyp)
        {
            IWorkspace2 pWorkspace2 = (IWorkspace2)this.WorkSpace;
            if (pWorkspace2 == null) return false;
            bool bRes = pWorkspace2.get_NameExists(aDatasetTyp, Datasetname);
            if (bRes == false) return false;
            return true;
        }

        /// <summary>
        /// 导入DOM数据(RC文件)集合
        /// </summary>
        /// <param name="RCDatasetName">DOM数据集(RC)</param>
        /// <param name="filepaths">DOM数据(RC文件)路径集合</param>
        /// <returns></returns>
        public bool InputDOMData(string RCDatasetName, List<string> filepaths, out Exception eError)
        {
            eError = null;
            string NameList = String.Empty;
            foreach (string filepath in filepaths)
            {
                if (NameList.Length == 0)
                {
                    NameList = filepath;
                }
                else
                {
                    NameList = NameList + " ; " + filepath;
                }
            }
            return InputDOMData(RCDatasetName, NameList, out eError);
        }

        /// <summary>
        /// 导入DOM数据(RC文件)
        /// </summary>
        /// <param name="RCDatasetName">DOM数据集(RC)</param>
        /// <param name="filepaths">DOM数据(RC文件)路径</param>
        /// <returns></returns>
        public bool InputDOMData(string RCDatasetName, string filepaths, out Exception eError)
        {
            eError = null;

            try
            {
                IRasterCatalogLoader pRCLoader = new RasterCatalogLoaderClass();
                pRCLoader.Workspace = this.WorkSpace;

                pRCLoader.LoadDatasets(RCDatasetName, filepaths, null);
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 导入DEM数据(RD文件)集合
        /// </summary>
        /// <param name="RDDatasetName">DEM数据集(RD)</param>
        /// <param name="filepaths">DEM数据(RD文件)路径集合</param>
        /// <returns></returns>
        public bool InputDEMData(string RDDatasetName, List<string> filepaths, out Exception eError)
        {
            eError = null;
            RasterToGeodatabase pRasterToGDB = new RasterToGeodatabase();

            string NameList = String.Empty;
            foreach (string filepath in filepaths)
            {
                if (NameList.Length == 0)
                {
                    NameList = filepath;
                }
                else
                {
                    NameList = NameList + " ; " + filepath;
                }
            }
            return InputDEMData(RDDatasetName, NameList, out eError);
        }

        /// <summary>
        /// 导入DEM数据(RD文件)
        /// </summary>
        /// <param name="RDDatasetName">DEM数据集(RD)<</param>
        /// <param name="filepaths">DEM数据(RD文件)路径</param>
        /// <returns></returns>
        public bool InputDEMData(string RDDatasetName, string filepaths, out Exception eError)
        {
            eError = null;
            IRasterDataset pRasterDataset = GetRasterDataset(RDDatasetName, out eError);
            if (pRasterDataset == null) return false;

            RasterToGeodatabase pRasterToGDB = new RasterToGeodatabase();
            pRasterToGDB.Input_Rasters = filepaths;
            pRasterToGDB.Output_Geodatabase = pRasterDataset;

            Geoprocessor pGeoProcessor = new Geoprocessor();
            try
            {
                pGeoProcessor.Execute(pRasterToGDB, null);
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 新建Feature,属性字段为Dictionary,几何属性为IGeometry
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="values">属性字段名值</param>
        /// <param name="geomtry">几何属性</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public bool NewFeature(string objfcname, Dictionary<string, object> values, IGeometry geomtry, bool Edit, out Exception eError)
        {
            eError = null;
            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return false;
                }
            }

            IFeatureClass ObjFeatureCls = GetFeatureClass(objfcname, out eError);
            if (ObjFeatureCls == null) return false;

            try
            {
                IFeature pFeature = ObjFeatureCls.CreateFeature();
                foreach (KeyValuePair<string, object> keyvalue in values)
                {
                    int index = pFeature.Fields.FindField(keyvalue.Key);
                    if (index == -1) continue;

                    pFeature.set_Value(index, keyvalue.Value);
                }

                if (geomtry != null) pFeature.Shape = geomtry;
                pFeature.Store();

                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(false, out eError) == false)
                    {
                        return false;
                    }
                }
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 新建Feature,属性字段为Dictionary,几何属性为IGeometry,返回新建要素的OID
        /// </summary>
        /// <param name="objfcname">要素类名称</param>
        /// <param name="values">属性字段</param>
        /// <param name="geomtry">图形字段</param>
        /// <param name="NewOID">新要素OID</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <param name="eError">返回错误信息</param>
        /// <returns></returns>
        public bool NewFeature(string objfcname, Dictionary<string, object> values, IGeometry geomtry, out int NewOID, bool Edit, out Exception eError)
        {
            eError = null;
            NewOID = -1;
            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return false;
                }
            }

            IFeatureClass ObjFeatureCls = GetFeatureClass(objfcname, out eError);
            if (ObjFeatureCls == null) return false;

            try
            {
                IFeature pFeature = ObjFeatureCls.CreateFeature();
                foreach (KeyValuePair<string, object> keyvalue in values)
                {
                    int index = pFeature.Fields.FindField(keyvalue.Key);
                    if (index == -1) continue;

                    pFeature.set_Value(index, keyvalue.Value);
                }

                if (geomtry != null) pFeature.Shape = geomtry;
                pFeature.Store();

                NewOID = pFeature.OID;

                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(false, out eError) == false)
                    {
                        return false;
                    }
                }
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 新建Feature,属性字段为Dictionary,几何属性为Dictionary
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="values">属性字段名值</param>
        /// <param name="dicCoor">几何属性</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public bool NewFeature(string objfcname, Dictionary<string, object> values, Dictionary<int, string> dicCoor, bool Edit, out Exception eError)
        {
            eError = null;
            IGeometry geomtry = null;
            IPolygon pPolygon;
            // 从坐标字典得到范围geomtry
            if (ModGisPub.GetPolygonByCol(dicCoor, out pPolygon, out eError))
            {
                geomtry = (IGeometry)pPolygon;
            }
            else
                return false;

            return NewFeature(objfcname, values, geomtry, Edit, out eError);
        }

        /// <summary>
        /// 新建Feature,属性字段为Dictionary,几何属性为IGeometry
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="values">属性字段名值</param>
        /// <param name="geomtry">几何属性</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public bool NewFeature(IFeatureClass ObjFeatureCls, Dictionary<string, object> values, IGeometry geomtry, bool Edit, out Exception eError)
        {
            eError = null;
            if (ObjFeatureCls == null) return false;

            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return false;
                }
            }

            try
            {
                IFeature pFeature = ObjFeatureCls.CreateFeature();
                foreach (KeyValuePair<string, object> keyvalue in values)
                {
                    int index = pFeature.Fields.FindField(keyvalue.Key);
                    if (index == -1) continue;

                    pFeature.set_Value(index, keyvalue.Value);
                }

                if (geomtry != null) pFeature.Shape = geomtry;
                pFeature.Store();

                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(false, out eError) == false)
                    {
                        return false;
                    }
                }
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 新建Feature,属性字段为Dictionary,几何属性为IGeometry,返回新建要素的OID
        /// </summary>
        /// <param name="objfcname">要素类名称</param>
        /// <param name="values">属性字段</param>
        /// <param name="geomtry">图形字段</param>
        /// <param name="NewOID">新要素OID</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <param name="eError">返回错误信息</param>
        /// <returns></returns>
        public bool NewFeature(IFeatureClass ObjFeatureCls, Dictionary<string, object> values, IGeometry geomtry, out int NewOID, bool Edit, out Exception eError)
        {
            eError = null;
            NewOID = -1;
            if (ObjFeatureCls == null) return false;

            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return false;
                }
            }

            try
            {
                IFeature pFeature = ObjFeatureCls.CreateFeature();
                foreach (KeyValuePair<string, object> keyvalue in values)
                {
                    int index = pFeature.Fields.FindField(keyvalue.Key);
                    if (index == -1) continue;

                    pFeature.set_Value(index, keyvalue.Value);
                }

                if (geomtry != null) pFeature.Shape = geomtry;
                pFeature.Store();

                NewOID = pFeature.OID;

                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(false, out eError) == false)
                    {
                        return false;
                    }
                }
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 新建Feature,属性字段为Dictionary,几何属性为Dictionary
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="values">属性字段名值</param>
        /// <param name="dicCoor">几何属性</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public bool NewFeature(IFeatureClass ObjFeatureCls, Dictionary<string, object> values, Dictionary<int, string> dicCoor, bool Edit, out Exception eError)
        {
            eError = null;
            IGeometry geomtry = null;
            IPolygon pPolygon;
            // 从坐标字典得到范围geomtry
            if (ModGisPub.GetPolygonByCol(dicCoor, out pPolygon, out eError))
            {
                geomtry = (IGeometry)pPolygon;
            }
            else
                return false;

            return NewFeature(ObjFeatureCls, values, geomtry, Edit, out eError);
        }

        /// <summary>
        /// 新建Features,属性字段为Dictionary,几何属性为源数据的Geometry
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="pfeacursor">源数据</param>
        /// <param name="values">字段集合</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <param name="bIngore">是否忽略错误继续下一循环</param>
        /// <returns></returns>
        public bool NewFeatures(IFeatureClass ObjFeatureCls, IFeatureCursor pfeacursor, Dictionary<string, object> values, bool Edit, bool bIngore, out Exception eError)
        {
            eError = null;
            if (ObjFeatureCls == null) return false;

            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return false;
                }
            }

            IFeatureBuffer pFeatureBuffer = ObjFeatureCls.CreateFeatureBuffer();
            IFeatureCursor pObjFeaCursor = ObjFeatureCls.Insert(true);

            IFeature pFeature = pfeacursor.NextFeature();
            while (pFeature != null)
            {
                try
                {
                    if (values != null)
                    {
                        foreach (KeyValuePair<string, object> keyvalue in values)
                        {
                            int index = pFeatureBuffer.Fields.FindField(keyvalue.Key);
                            if (index == -1) continue;

                            pFeatureBuffer.set_Value(index, keyvalue.Value);
                        }
                    }

                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pObjFeaCursor.InsertFeature(pFeatureBuffer);

                    pFeature = pfeacursor.NextFeature();
                }
                catch (Exception eX)
                {
                    //********************************
                    //guozheng added  system exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eX);
                    //********************************
                    eError = eX;
                    if (bIngore == false)
                        break;
                    else
                        pFeature = pfeacursor.NextFeature();
                    continue;
                }
            }

            pObjFeaCursor.Flush();
            Marshal.ReleaseComObject(pObjFeaCursor);

            if (Edit == true)
            {
                if (eError != null)
                {
                    if (this.EndWorkspaceEdit(bIngore, out eError) == false)
                    {
                        return false;
                    }
                    return false;
                }
                else
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 新建Features,属性字段部分采用Dictionary部分来自源数据(如取值发生冲突以源数据为准),几何属性为源数据的Geometry
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="pfeacursor">源数据</param>
        /// <param name="FieldNames">采用源数据字段值的字段名列表</param>
        /// <param name="values">字段集合</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public bool NewFeatures(IFeatureClass ObjFeatureCls, IFeatureCursor pfeacursor, List<string> FieldNames, Dictionary<string, object> values, bool Edit, bool bIngore, out Exception eError)
        {
            eError = null;
            if (ObjFeatureCls == null) return false;

            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return false;
                }
            }

            IFeatureBuffer pFeatureBuffer = ObjFeatureCls.CreateFeatureBuffer();
            IFeatureCursor pObjFeaCursor = ObjFeatureCls.Insert(true);

            IFeature pFeature = pfeacursor.NextFeature();
            while (pFeature != null)
            {
                try
                {
                    if (values != null)
                    {
                        foreach (KeyValuePair<string, object> keyvalue in values)
                        {
                            int index = pFeatureBuffer.Fields.FindField(keyvalue.Key);
                            if (index == -1) continue;

                            pFeatureBuffer.set_Value(index, keyvalue.Value);
                        }
                    }

                    if (FieldNames != null)
                    {
                        foreach (string fieldname in FieldNames)
                        {
                            int index = pFeature.Fields.FindField(fieldname);
                            int ObjIndex = pFeatureBuffer.Fields.FindField(fieldname);
                            if (index == -1 || ObjIndex == -1) continue;

                            pFeatureBuffer.set_Value(ObjIndex, pFeature.get_Value(index));
                        }
                    }

                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pObjFeaCursor.InsertFeature(pFeatureBuffer);

                    pFeature = pfeacursor.NextFeature();
                }
                catch (Exception eX)
                {
                    //********************************
                    //guozheng added  system exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eX);
                    //********************************
                    eError = eX;
                    if (bIngore == false)
                        break;
                    else
                        pFeature = pfeacursor.NextFeature();
                    continue;
                }
            }

            pObjFeaCursor.Flush();
            Marshal.ReleaseComObject(pObjFeaCursor);

            if (Edit == true)
            {
                if (eError != null)
                {
                    if (this.EndWorkspaceEdit(bIngore, out eError) == false)
                    {
                        return false;
                    }
                    return false;
                }
                else
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 新建Features,属性字段部分采用Dictionary部分来自源数据(如取值发生冲突以源数据为准),几何属性为源数据的Geometry
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="pfeacursor">源数据</param>
        /// <param name="dicFieldsPair">采用源数据字段值且与目标字段对应的Dictionary,key为源数据字段名,value为目标字段名</param>
        /// <param name="values">字段集合</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public bool NewFeatures(IFeatureClass ObjFeatureCls, IFeatureCursor pfeacursor, Dictionary<string, string> dicFieldsPair, Dictionary<string, object> values, bool Edit, bool bIngore, out Exception eError)
        {
            eError = null;
            if (ObjFeatureCls == null) return false;

            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return false;
                }
            }

            IFeatureBuffer pFeatureBuffer = ObjFeatureCls.CreateFeatureBuffer();
            IFeatureCursor pObjFeaCursor = ObjFeatureCls.Insert(true);

            IFeature pFeature = pfeacursor.NextFeature();
            while (pFeature != null)
            {
                try
                {
                    if (values != null)
                    {
                        foreach (KeyValuePair<string, object> keyvalue in values)
                        {
                            int index = pFeatureBuffer.Fields.FindField(keyvalue.Key);
                            if (index == -1) continue;

                            pFeatureBuffer.set_Value(index, keyvalue.Value);
                        }
                    }

                    if (dicFieldsPair != null)
                    {
                        foreach (KeyValuePair<string, string> keyvalue in dicFieldsPair)
                        {
                            int index = pFeature.Fields.FindField(keyvalue.Key);
                            int ObjIndex = pFeatureBuffer.Fields.FindField(keyvalue.Value);
                            if (index == -1 || ObjIndex == -1) continue;

                            pFeatureBuffer.set_Value(ObjIndex, pFeature.get_Value(index));
                        }
                    }

                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pObjFeaCursor.InsertFeature(pFeatureBuffer);

                    pFeature = pfeacursor.NextFeature();
                }
                catch (Exception eX)
                {
                    //********************************
                    //guozheng added  system exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eX);
                    //********************************
                    eError = eX;
                    if (bIngore == false)
                        break;
                    else
                        pFeature = pfeacursor.NextFeature();
                    continue;
                }
            }

            pObjFeaCursor.Flush();
            Marshal.ReleaseComObject(pObjFeaCursor);

            if (Edit == true)
            {
                if (eError != null)
                {
                    if (this.EndWorkspaceEdit(bIngore, out eError) == false)
                    {
                        return false;
                    }
                    return false;
                }
                else
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// 新建Features,属性字段为Dictionary,几何属性为源数据的Geometry
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="pfeacursor">源数据</param>
        /// <param name="values">字段集合</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <param name="bIngore">是否忽略错误继续下一循环</param>
        /// <returns></returns>
        public bool NewFeatures(string objfcname, IFeatureCursor pfeacursor, Dictionary<string, object> values, bool useOrgFdVal, bool Edit, bool bIngore, out Exception eError)
        {
            eError = null;

            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return false;
                }
            }

            IFeatureClass ObjFeatureCls = GetFeatureClass(objfcname, out eError);
            if (ObjFeatureCls == null) return false;
            IFeatureBuffer pFeatureBuffer = ObjFeatureCls.CreateFeatureBuffer();
            IFeatureCursor pObjFeaCursor = ObjFeatureCls.Insert(true);

            IFeature pFeature = pfeacursor.NextFeature();
            while (pFeature != null)
            {
                try
                {
                    if (values != null)
                    {
                        foreach (KeyValuePair<string, object> keyvalue in values)
                        {
                            int index = pFeatureBuffer.Fields.FindField(keyvalue.Key);
                            if (index == -1) continue;

                            pFeatureBuffer.set_Value(index, keyvalue.Value);
                        }
                    }

                    if (useOrgFdVal)
                    {
                        for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                        {
                            IField aField = pFeature.Fields.get_Field(i);
                            if (aField.Type != esriFieldType.esriFieldTypeGeometry && aField.Type != esriFieldType.esriFieldTypeOID && aField.Editable)
                            {
                                int ObjIndex = pFeatureBuffer.Fields.FindField(aField.Name);
                                if (ObjIndex == -1) continue;

                                pFeatureBuffer.set_Value(ObjIndex, pFeature.get_Value(i));
                            }
                        }
                    }

                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pObjFeaCursor.InsertFeature(pFeatureBuffer);

                    pFeature = pfeacursor.NextFeature();
                }
                catch (Exception eX)
                {
                    //********************************
                    //guozheng added  system exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eX);
                    //********************************
                    eError = eX;
                    if (bIngore == false)
                        break;
                    else
                        pFeature = pfeacursor.NextFeature();
                    continue;
                }
            }

            pObjFeaCursor.Flush();
            Marshal.ReleaseComObject(pObjFeaCursor);

            if (Edit == true)
            {
                if (eError != null)
                {
                    if (this.EndWorkspaceEdit(bIngore, out eError) == false)
                    {
                        return false;
                    }
                    return false;
                }
                else
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 新建Features,属性字段部分采用Dictionary部分来自源数据(如取值发生冲突以源数据为准),几何属性为源数据的Geometry
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="pfeacursor">源数据</param>
        /// <param name="FieldNames">采用源数据字段值的字段名列表</param>
        /// <param name="values">字段集合</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public bool NewFeatures(string objfcname, IFeatureCursor pfeacursor, List<string> FieldNames, Dictionary<string, object> values, bool Edit, bool bIngore, out Exception eError)
        {
            eError = null;

            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return false;
                }
            }

            IFeatureClass ObjFeatureCls = GetFeatureClass(objfcname, out eError);
            if (ObjFeatureCls == null) return false;
            IFeatureBuffer pFeatureBuffer = ObjFeatureCls.CreateFeatureBuffer();
            IFeatureCursor pObjFeaCursor = ObjFeatureCls.Insert(true);

            IFeature pFeature = pfeacursor.NextFeature();
            while (pFeature != null)
            {
                try
                {
                    if (values != null)
                    {
                        foreach (KeyValuePair<string, object> keyvalue in values)
                        {
                            int index = pFeatureBuffer.Fields.FindField(keyvalue.Key);
                            if (index == -1) continue;

                            pFeatureBuffer.set_Value(index, keyvalue.Value);
                        }
                    }

                    if (FieldNames != null)
                    {
                        foreach (string fieldname in FieldNames)
                        {
                            int index = pFeature.Fields.FindField(fieldname);
                            int ObjIndex = pFeatureBuffer.Fields.FindField(fieldname);
                            if (index == -1 || ObjIndex == -1) continue;

                            pFeatureBuffer.set_Value(ObjIndex, pFeature.get_Value(index));
                        }
                    }

                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pObjFeaCursor.InsertFeature(pFeatureBuffer);

                    pFeature = pfeacursor.NextFeature();
                }
                catch (Exception eX)
                {
                    //********************************
                    //guozheng added  system exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eX);
                    //********************************
                    eError = eX;
                    if (bIngore == false)
                        break;
                    else
                        pFeature = pfeacursor.NextFeature();
                    continue;
                }
            }

            pObjFeaCursor.Flush();
            Marshal.ReleaseComObject(pObjFeaCursor);

            if (Edit == true)
            {
                if (eError != null)
                {
                    if (this.EndWorkspaceEdit(bIngore, out eError) == false)
                    {
                        return false;
                    }
                    return false;
                }
                else
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 新建Features,属性字段部分采用Dictionary部分来自源数据(如取值发生冲突以源数据为准),几何属性为源数据的Geometry
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="pfeacursor">源数据</param>
        /// <param name="dicFieldsPair">采用源数据字段值且与目标字段对应的Dictionary,key为源数据字段名,value为目标字段名</param>
        /// <param name="values">字段集合</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public bool NewFeatures(string objfcname, IFeatureCursor pfeacursor, Dictionary<string, string> dicFieldsPair, Dictionary<string, object> values, bool Edit, bool bIngore, out Exception eError)
        {
            eError = null;

            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return false;
                }
            }

            IFeatureClass ObjFeatureCls = GetFeatureClass(objfcname, out eError);
            if (ObjFeatureCls == null) return false;
            IFeatureBuffer pFeatureBuffer = ObjFeatureCls.CreateFeatureBuffer();
            IFeatureCursor pObjFeaCursor = ObjFeatureCls.Insert(true);

            IFeature pFeature = pfeacursor.NextFeature();
            while (pFeature != null)
            {
                try
                {
                    if (values != null)
                    {
                        foreach (KeyValuePair<string, object> keyvalue in values)
                        {
                            int index = pFeatureBuffer.Fields.FindField(keyvalue.Key);
                            if (index == -1) continue;

                            pFeatureBuffer.set_Value(index, keyvalue.Value);
                        }
                    }

                    if (dicFieldsPair != null)
                    {
                        foreach (KeyValuePair<string, string> keyvalue in dicFieldsPair)
                        {
                            int index = pFeature.Fields.FindField(keyvalue.Key);
                            int ObjIndex = pFeatureBuffer.Fields.FindField(keyvalue.Value);
                            if (index == -1 || ObjIndex == -1) continue;

                            pFeatureBuffer.set_Value(ObjIndex, pFeature.get_Value(index));
                        }
                    }

                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pObjFeaCursor.InsertFeature(pFeatureBuffer);

                    pFeature = pfeacursor.NextFeature();
                }
                catch (Exception eX)
                {
                    //********************************
                    //guozheng added  system exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eX);
                    //********************************
                    eError = eX;
                    if (bIngore == false)
                        break;
                    else
                        pFeature = pfeacursor.NextFeature();
                    continue;
                }
            }

            pObjFeaCursor.Flush();
            Marshal.ReleaseComObject(pObjFeaCursor);

            if (Edit == true)
            {
                if (eError != null)
                {
                    if (this.EndWorkspaceEdit(bIngore, out eError) == false)
                    {
                        return false;
                    }
                    return false;
                }
                else
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 编辑Feature
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="condition">条件</param>
        /// <param name="values">字段集合</param>
        /// <param name="dicCoor">几何属性</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public bool EditFeature(string objfcname, string condition, Dictionary<string, object> values, Dictionary<int, string> dicCoor, bool Edit, out Exception eError)
        {
            IGeometry geomtry = null;
            IPolygon pPolygon;
            // 从坐标字典得到范围geomtry
            if (ModGisPub.GetPolygonByCol(dicCoor, out pPolygon, out eError))
            {
                geomtry = (IGeometry)pPolygon;
            }
            else
                return false;

            return EditFeature(objfcname, condition, values, geomtry, Edit, out eError);
        }

        /// <summary>
        /// 编辑Feature
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="condition">条件</param>
        /// <param name="values">字段集合</param>
        /// <param name="geomtry">几何属性</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public bool EditFeature(string objfcname, string condition, Dictionary<string, object> values, IGeometry geomtry, bool Edit, out Exception eError)
        {
            eError = null;

            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return false;
                }
            }

            IFeatureCursor pFeatureCursor = GetFeatureCursor(objfcname, condition, out eError);
            if (pFeatureCursor == null) return false;
            IFeature pFeature = pFeatureCursor.NextFeature();
            if (pFeature == null)
            {
                eError = new Exception("要素不存在");
                return false;
            }

            try
            {
                foreach (KeyValuePair<string, object> keyvalue in values)
                {
                    int index = pFeature.Fields.FindField(keyvalue.Key);
                    if (index == -1) continue;

                    pFeature.set_Value(index, keyvalue.Value);
                }

                if (geomtry != null) pFeature.Shape = geomtry;
                pFeature.Store();

                Marshal.ReleaseComObject(pFeatureCursor);
                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(false, out eError) == false)
                    {
                        return false;
                    }
                }
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 编辑Feature
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="condition">条件</param>
        /// <param name="values">字段集合</param>
        /// <param name="geomtry">几何属性</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public IFeature GetEditFeature(string objfcname, string condition, Dictionary<string, object> values, IGeometry geomtry, bool Edit, out Exception eError)
        {
            eError = null;

            if (Edit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false)
                {
                    return null;
                }
            }

            IFeatureCursor pFeatureCursor = GetFeatureCursor(objfcname, condition, out eError);
            if (pFeatureCursor == null) return null;
            IFeature pFeature = pFeatureCursor.NextFeature();
            if (pFeature == null)
            {
                eError = new Exception("要素不存在");
                return null;
            }

            try
            {
                foreach (KeyValuePair<string, object> keyvalue in values)
                {
                    int index = pFeature.Fields.FindField(keyvalue.Key);
                    if (index == -1) continue;

                    pFeature.set_Value(index, keyvalue.Value);
                }

                if (geomtry != null) pFeature.Shape = geomtry;
                pFeature.Store();

                Marshal.ReleaseComObject(pFeatureCursor);
                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false)
                    {
                        return null;
                    }
                }
                return pFeature;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                if (Edit == true)
                {
                    if (this.EndWorkspaceEdit(false, out eError) == false)
                    {
                        return null;
                    }
                }
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 编辑Features
        /// </summary>
        /// <param name="objfcname">FC名称</param>
        /// <param name="condition">条件</param>
        /// <param name="values">字段集合</param>
        /// <param name="Edit">是否开启编辑</param>
        /// <returns></returns>
        public bool EditFeatures(string objfcname, string condition, Dictionary<string, object> values, bool bEdit, out Exception eError)
        {
            eError = null;

            if (bEdit == true)
            {
                if (this.StartWorkspaceEdit(false, out eError) == false) return false;
            }

            IFeatureClass pFeaClass = GetFeatureClass(objfcname, out eError);
            if (pFeaClass == null) return false;
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = condition;
            IFeatureCursor pFeatureCursor = pFeaClass.Update(pQueryFilter, false);
            if (pFeatureCursor == null) return false;
            IFeature pFeature = pFeatureCursor.NextFeature();

            try
            {
                while (pFeature != null)
                {
                    foreach (KeyValuePair<string, object> keyvalue in values)
                    {
                        int index = pFeature.Fields.FindField(keyvalue.Key);
                        if (index == -1) continue;
                        pFeature.set_Value(index, keyvalue.Value);
                    }

                    pFeatureCursor.UpdateFeature(pFeature);
                    pFeature = pFeatureCursor.NextFeature();
                }

                Marshal.ReleaseComObject(pFeatureCursor);
                if (bEdit == true)
                {
                    if (this.EndWorkspaceEdit(true, out eError) == false) return false;
                }

                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                if (bEdit == true)
                {
                    if (this.EndWorkspaceEdit(false, out eError) == false)
                    {
                        return false;
                    }
                }
                eError = eX;
                return false;
            }
        }

        /// <summary>
        /// 得到要素类的满足条件的字段名和索引的配对字典
        /// </summary>
        /// <param name="fcname"> 要素类名称</param>
        /// <param name="bEdit"> 是否添加不可编辑字段</param>
        /// <param name="wstype"> 工作区类型</param>
        /// <returns> 字段名和索引的配对字典</returns>
        public Dictionary<string, int> GetFieldIndexs(IFeatureClass pFeaClass, bool bEdit)
        {
            if (pFeaClass == null) return null;
            Dictionary<string, int> FieldIndexs = new Dictionary<string, int>();
            // 获取要素类型字段集合并循环
            IFields pFields = pFeaClass.Fields;
            for (int index = 0; index < pFields.FieldCount; index++)
            {
                IField pField = pFields.get_Field(index);

                if (!bEdit)
                {
                    if (!pField.Editable) continue;

                }

                if (pField.Type == esriFieldType.esriFieldTypeGeometry || pField.Type == esriFieldType.esriFieldTypeOID) continue;
                FieldIndexs.Add(pField.Name.ToUpper(), index);
            }
            return FieldIndexs;
        }

        /// <summary>
        /// 得到要素类的满足条件的字段名和索引的配对字典
        /// </summary>
        /// <param name="fcname"> 要素类名称</param>
        /// <param name="bEdit"> 是否添加不可编辑字段</param>
        /// <param name="wstype"> 工作区类型</param>
        /// <returns> 字段名和索引的配对字典</returns>
        public Dictionary<string, int> GetFieldIndexs(string fcname, bool bEdit)
        {
            // 打开要素类
            int pos = fcname.IndexOf("\\");
            if (pos >= 1) fcname = fcname.Substring(pos + 1);

            Exception errEx = null;
            IFeatureClass pFeaClass = GetFeatureClass(fcname, out errEx);
            if (pFeaClass == null) return null;

            Dictionary<string, int> FieldIndexs = new Dictionary<string, int>();
            // 获取要素类型字段集合并循环
            IFields pFields = pFeaClass.Fields;
            for (int index = 0; index < pFields.FieldCount; index++)
            {
                IField pField = pFields.get_Field(index);

                if (!bEdit)
                {
                    if (!pField.Editable) continue;

                }
                if (pField.Type == esriFieldType.esriFieldTypeGeometry || pField.Type == esriFieldType.esriFieldTypeOID) continue;
                FieldIndexs.Add(pField.Name.ToUpper(), index);
            }
            return FieldIndexs;
        }

        public bool DeleteRows(string tablename, string condition, out Exception eError)
        {
            eError = null;
            //得到FeatrueWS
            IFeatureWorkspace pFeaWS = (IFeatureWorkspace)this.WorkSpace;

            try
            {
                ITable pTable = pFeaWS.OpenTable(tablename);
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = condition;
                pTable.DeleteSearchedRows(pQueryFilter);

                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        public bool DeleteRows(string tablename, IQueryFilter pQueryFilter, out Exception eError)
        {
            eError = null;
            //得到FeatrueWS
            IFeatureWorkspace pFeaWS = (IFeatureWorkspace)this.WorkSpace;

            try
            {
                ITable pTable = pFeaWS.OpenTable(tablename);
                pTable.DeleteSearchedRows(pQueryFilter);

                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }
    }

    public class SysGisTable : SysGisDB, IGisTable
    {
        public SysGisTable()
        {

        }

        public SysGisTable(IWorkspace pWorkspace)
        {
            this.WorkSpace = pWorkspace;
        }

        public SysGisTable(SysGisDB gisDb)
        {
            this.WorkSpace = gisDb.WorkSpace;
        }

        #region IGisTable 成员

        public bool CreateTable(string sTableName, IFields pFields, out Exception eError)
        {
            eError = null;
            //得到FeatrueWS
            IFeatureWorkspace pFeaWS = (IFeatureWorkspace)this.WorkSpace;
            IObjectClassDescription pOCD=new ObjectClassDescriptionClass();
            try
            {
               ITable pTable=pFeaWS.CreateTable(sTableName, pFields,pOCD.InstanceCLSID,null,"");
               if (pTable == null)
                   return false;
               return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        public ITable OpenTable(string tablename, out Exception eError)
        {
            eError = null;
     
            try
            {
                //得到FeatrueWS
                IFeatureWorkspace pFeaWS = (IFeatureWorkspace)this.WorkSpace;
                return pFeaWS.OpenTable(tablename);
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }

        public System.Collections.ArrayList GetUniqueValue(string tablename, string fieldname, string condition, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dictionary<string, Type> GetFieldsType(string tablename, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dictionary<string, object> GetRow(string tablename, string condition, out Exception eError)
        {
            eError = null;
            //得到FeatrueWS
            try
            {
                Dictionary<string, object> dicValue = new Dictionary<string, object>();
                ITable pTable = OpenTable(tablename, out eError);
                if (eError != null) return null;
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = condition;
                ICursor pCursor = pTable.Search(pQueryFilter, true);
                IRow pRow = pCursor.NextRow();
                if (pRow == null)
                {
                    Marshal.ReleaseComObject(pCursor);
                    return null;
                }

                for (int i = 0; i < pRow.Fields.FieldCount; i++)
                {
                    dicValue.Add(pRow.Fields.get_Field(i).Name, pRow.get_Value(i));
                }

                Marshal.ReleaseComObject(pCursor);
                return dicValue;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 获取属性表中多字段多条记录
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="condition"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetRows(string tablename, string condition, out Exception eError)
        {
            try
            {
                ITable table = OpenTable(tablename, out eError);
                List<Dictionary<string, object>> lstDicData = null;

                if (table != null)
                {
                    lstDicData = new List<Dictionary<string, object>>();
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = condition;

                    ICursor pCursor = table.Search(pQueryFilter, true);
                    if (pCursor != null)
                    {
                        IRow row = pCursor.NextRow();
                        while (row != null)
                        {
                            Dictionary<string, object> dicData = new Dictionary<string, object>();
                            for (int i = 0; i < table.Fields.FieldCount; i++)
                            {
                                object obj = row.get_Value(i);
                                //blob类型转换(不转换会出现值替换的怪问题)
                                if (obj is IMemoryBlobStreamVariant)
                                {
                                    try
                                    {
                                        IMemoryBlobStreamVariant var = obj as IMemoryBlobStreamVariant;
                                        object tempObj = null;
                                        if (var == null) continue;
                                        var.ExportToVariant(out tempObj);
                                        XmlDocument doc = new XmlDocument();
                                        byte[] btyes = (byte[])tempObj;
                                        string xml = Encoding.Default.GetString(btyes);
                                        doc.LoadXml(xml);
                                        dicData.Add(table.Fields.get_Field(i).Name, doc);
                                        continue;
                                    }
                                    catch
                                    {}
                                }
                                dicData.Add(table.Fields.get_Field(i).Name, obj);
                            }
                            lstDicData.Add(dicData);
                            row = pCursor.NextRow();
                        }
                    }
                }
                return lstDicData;
            }
            catch (Exception ex)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(ex);
                //********************************
                eError = ex;
                return null;
            }
        }
        public List<object> GetFieldValues(string tablename, string keyfieldname, string condition, out Exception eError)
        {
            try
            {
                ITable table = OpenTable(tablename, out eError);
                if (table != null)
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = condition;

                    ICursor pCursor = table.Search(pQueryFilter, true);
                    int index = table.Fields.FindField(keyfieldname);
                    if (pCursor != null)
                    {
                        List<object> zonesetids = new List<object>();
                        IRow row = pCursor.NextRow();
                        while (row != null)
                        {
                            if (index != -1)
                            {
                                if (row != null)
                                {
                                    object obj = row.get_Value(index);
                                    if (obj is IMemoryBlobStreamVariant)
                                    {
                                        IMemoryBlobStreamVariant var = obj as IMemoryBlobStreamVariant;
                                        object tempObj = null;
                                        if (var == null) continue;
                                        var.ExportToVariant(out tempObj);
                                        XmlDocument doc = new XmlDocument();
                                        byte[] btyes = (byte[])tempObj;
                                        string xml = Encoding.Default.GetString(btyes);
                                        doc.LoadXml(xml);
                                        obj = doc;
                                    }
                                    if (!zonesetids.Contains(obj))
                                    {
                                        zonesetids.Add(obj);
                                    }
                                }
                            }
                            row = pCursor.NextRow();
                        }
                        return zonesetids;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                eError = ex;
                return null;
            }
        }
        public object GetFieldValue(string tablename, string keyfieldname, string condition, out Exception eError)
        {
            eError = null;
            //得到FeatrueWS
            try
            {
                Dictionary<string, object> dicValue = new Dictionary<string, object>();
                ITable pTable = OpenTable(tablename, out eError);
                if (eError != null) return null;
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = condition;
                ICursor pCursor = pTable.Search(pQueryFilter, true);
                IRow pRow = pCursor.NextRow();
                if (pRow == null)
                {
                    Marshal.ReleaseComObject(pCursor);
                    return null;
                }

                int indexF = pRow.Fields.FindField(keyfieldname);
                if (indexF == -1)
                {
                    Marshal.ReleaseComObject(pCursor);
                    return null;
                }

                object val = pRow.get_Value(indexF);
                //Marshal.ReleaseComObject(pCursor);
                if (val is IMemoryBlobStreamVariant)
                {
                    IMemoryBlobStreamVariant var = val as IMemoryBlobStreamVariant;
                    object tempObj = null;
                    if (var == null) return null;
                    var.ExportToVariant(out tempObj);
                    XmlDocument doc = new XmlDocument();
                    byte[] btyes = (byte[])tempObj;
                    try
                    {
                        string xml = Encoding.Default.GetString(btyes);
                        doc.LoadXml(xml);
                        Marshal.ReleaseComObject(pCursor);
                        return doc;
                    }
                    catch
                    {
                        Marshal.ReleaseComObject(pCursor);
                        return btyes;
                    }
                }
                Marshal.ReleaseComObject(pCursor);
                return val;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }

        public long GetRowCount(string tablename, string condition, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dictionary<int, object> GetRows(string tablename, string field, string condition, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dictionary<int, System.Collections.ArrayList> GetRows(string tablename, List<string> fields, string condition, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dictionary<int, System.Collections.ArrayList> GetRows(string tablename, List<string> fields, string condition, string postfixClause, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dictionary<object, object> GetRows(string tablename, string keyField, string field, string condition, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dictionary<object, System.Collections.ArrayList> GetRows(string tablename, string keyField, List<string> fields, string condition, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dictionary<object, System.Collections.ArrayList> GetRows(string tablename, string keyField, List<string> fields, string condition, string postfixClause, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool CheckFieldValue(string tablename, string fieldname, object value, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, out int objectid, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, string strOIDField, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, string strOIDField, out int objectid, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, string strField, esriFieldType FieldType, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool NewRow(string tablename, Dictionary<string, object> dicvalues, bool bEdit, string strField, esriFieldType FieldType, out int objectid, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public bool NewRowByAliasName(string tablename, Dictionary<string, object> dicValues, out Exception eError)
        {
            try
            {
                ITable table = OpenTable(tablename, out eError);
                if (table != null)
                {
                    return ModGisPub.NewRowByAliasName(table, dicValues, out eError);
                }
                return false;
            }
            catch (Exception ex)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(ex);
                //********************************
                eError = ex;
                return false;
            }
        }
        public bool NewRow(string tablename, Dictionary<string, object> dicvalues, out Exception eError)
        {
            try
            {
                ITable table = OpenTable(tablename, out eError);
                if (table != null)
                {
                    return ModGisPub.NewRow(table, dicvalues, out eError);
                }
                return false;
            }
            catch (Exception ex)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(ex);
                //********************************
                eError = ex;
                return false;
            }
        }

        public bool ExistData(string tablename, string condition)
        {
            Exception eError = null;
            try
            {
                ITable table = OpenTable(tablename, out eError);
                return ExistData(table, condition);
            }
            catch (Exception ex)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(ex);
                //********************************
                eError = ex;
                return false;
            }
        }

        private bool ExistData(ITable table, string condition)
        {
            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = condition;
                ICursor pCursor = table.Search(pQueryFilter, false);
                if (pCursor != null)
                {
                    IRow pRow = pCursor.NextRow();
                    if (pRow != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(ex);
                //********************************
                return false;
            }
        }

        public bool EditRows(string tablename, string condition, Dictionary<string, object> dicValues, bool bEdit, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool EditRows(string tablename, string condition, Dictionary<string, object> dicValues, bool bEdit, string fieldName, esriFieldType FieldType, out Exception eError)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public bool UpdateRowByAliasName(string tablename, string condition, Dictionary<string, object> dicValues, out Exception eError)
        {
            try
            {
                ITable table = OpenTable(tablename, out eError);
                if (table != null)
                {
                    return ModGisPub.UpdateRowByAliasName(table, condition, dicValues, out eError);
                }
                return false;
            }
            catch (Exception ex)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(ex);
                //********************************
                eError = ex;
                return false;
            }
        }

        public bool UpdateRow(string tablename, string condition, Dictionary<string, object> dicValues, out Exception eError)
        {
            try
            {
                ITable table = OpenTable(tablename, out eError);
                if (table != null)
                {
                    return ModGisPub.UpdateRow(table, condition, dicValues, out eError);
                }
                return false;
            }
            catch (Exception ex)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(ex);
                //********************************
                eError = ex;
                return false;
            }
        }

        public bool DeleteRows(string tablename, string condition, out Exception eError)
        {
            eError = null;
            try
            {
                ITable pTable = OpenTable(tablename, out eError);
                if (pTable == null) return false;

                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = condition;
                pTable.DeleteSearchedRows(pQueryFilter);

                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }

        #endregion
    }

    public class CreateArcGISGeoDatabase : ICreateGeoDatabase
    {

        private ISchemeProject m_pProject = null;                                           //数据库结构文件对象
        private int m_DBScale = 0;                                                          //默认比例尺
        private int m_DSScale = 0;                                                          //数据集比例尺
        private ISpatialReference m_SpatialReference = null;                                //空间参考对象

        private IWorkspace m_WorkSpace = null;                                              //设置属性后获得的工作空间

        string netLogPath = Application.StartupPath + @"\..\Template\Network_Log.mdb";       //远程日志模板路径

        #region ICreateGeoDatabase 成员
        /// <summary>
        /// 加载数据库结构文件方法
        /// </summary>
        /// <param name="LoadPath">文件路径</param>
        /// <returns></returns>
        public bool LoadDBShecmaDocument(string LoadPath)
        {
            try
            {
                m_pProject = new SchemeProjectClass();     //创建实例
                int index = LoadPath.LastIndexOf('.');
                if (index == -1) return false;
                string lastName = LoadPath.Substring(index + 1);
                if (lastName == "mdb")
                {
                    m_pProject.Load(LoadPath, e_FileType.GO_SCHEMEFILETYPE_MDB);    //加载schema文件
                }
                else if (lastName == "gosch")
                {
                    m_pProject.Load(LoadPath, e_FileType.GO_SCHEMEFILETYPE_GOSCH);    //加载schema文件
                }


                ///如果加载成功则获取比例尺返回true，否则返回false
                if (m_pProject != null)
                {
                    string DBScale = m_pProject.get_MetaDataValue("Scale") as string;   //获取比例尺信息（总工程中的默认比例尺）
                    string[] DBScaleArayy = DBScale.Split(':');
                    m_DBScale = Convert.ToInt32(DBScaleArayy[1]);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
            }
        }

        /// <summary>
        /// 加载空间参考文件获取空间参考
        /// </summary>
        /// <param name="LoadPath">空间参考文件路径</param>
        /// <returns></returns>
        public bool LoadSpatialReference(string LoadPath)
        {
            try
            {
                ISpatialReference pSR = null;
                ISpatialReferenceFactory pSpatialRefFac = new SpatialReferenceEnvironmentClass();

                if (!File.Exists(LoadPath))
                {
                    m_SpatialReference = null;
                    return false;
                }
                pSR = pSpatialRefFac.CreateESRISpatialReferenceFromPRJFile(LoadPath);

                ISpatialReferenceResolution pSRR = pSR as ISpatialReferenceResolution;
                ISpatialReferenceTolerance pSRT = (ISpatialReferenceTolerance)pSR;
                IControlPrecision2 pSpatialPrecision = (IControlPrecision2)pSR;

                pSRR.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon
                pSRR.SetDefaultXYResolution();
                pSRT.SetDefaultXYTolerance();

                m_SpatialReference = pSR;                //赋值空间参考

                return true;
            }
            catch
            {
                m_SpatialReference = null;
                return false;
            }
        }

        /// <summary>
        /// 设置连接属性
        /// </summary>
        /// <param name="要素类型">数据库类型</param>
        /// <param name="IPoPath">数据库访问路径或服务器IP</param>
        /// <param name="Intance">sde服务实例</param>
        /// <param name="User">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="Version">sde版本</param>
        /// <returns></returns>
        public bool SetDestinationProp(string Type, string IPoPath, string Intance, string User, string PassWord, string Version)
        {
            IWorkspace TempWorkSpace = null;                                 //工作空间
            IWorkspaceFactory pWorkspaceFactory = null;                      //工作空间工厂

            try
            {
                //初始化工作空间工厂
                if (Type == "PDB")
                {
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                }
                else if (Type == "GDB")
                {
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                }
                else if (Type == "SDE")
                {
                    pWorkspaceFactory = new SdeWorkspaceFactoryClass();
                }
                //cyf  20110622 delete:不删除原来的数据库，在原有的基础进行追加
                ///如果创建的是本地库体，则首先判断库体是否存在
                ///如果库体存在，则先删除原有库体
                //if (File.Exists(IPoPath))
                //{
                    //if (!SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "是否库体中创建数据集"))
                    //{
                    //    File.Delete(IPoPath);
                    //}
                //}
                //end

                if (Type == "SDE")  //如果是SDE则设置sde工作空间连接信息
                {
                    IPropertySet propertySet = new PropertySetClass();
                    propertySet.SetProperty("SERVER", IPoPath);
                    propertySet.SetProperty("INSTANCE", Intance);
                    //propertySet.SetProperty("DATABASE", ""); 
                    propertySet.SetProperty("USER", User);
                    propertySet.SetProperty("PASSWORD", PassWord);
                    propertySet.SetProperty("VERSION", Version);
                    TempWorkSpace = pWorkspaceFactory.Open(propertySet, 0);
                }
                else  //如果不是sde则创建工作空间
                {
                    FileInfo finfo = new FileInfo(IPoPath);
                    string outputDBPath = finfo.DirectoryName;
                    string outputDBName = finfo.Name;
                    if (outputDBName.EndsWith(".gdb"))
                    {
                        outputDBName = outputDBName.Substring(0, outputDBName.Length - 4);
                    }
                    //cyf 20110622 add:打开已有的工作空间
                    try { TempWorkSpace = pWorkspaceFactory.OpenFromFile(IPoPath, 0); }
                    catch { }
                    //end
                    if (TempWorkSpace == null)
                    {
                        IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(outputDBPath, outputDBName, null, 0);
                        ESRI.ArcGIS.esriSystem.IName pName = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;
                        TempWorkSpace = (IWorkspace)pName.Open();
                    }
                }

                //判断获取工作空间是否成功
                if (TempWorkSpace != null)
                {
                    m_WorkSpace = TempWorkSpace;                //工作空间赋值
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //***********************
                //guozheng 2010-12-17 added
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败！\n原因：" + ex.Message);
                //***********************
                return false;
            }

        }

        /// <summary>
        /// 创建库体主函数
        /// </summary>
        /// <returns></returns>
        public bool CreateDBStruct()
        {
            IFeatureWorkspace pFeatureWorkSpace = null;

            try
            {
                //如果工作空间获取成功则赋值要素工作空间
                if (m_WorkSpace != null)
                {
                    pFeatureWorkSpace = m_WorkSpace as IFeatureWorkspace;
                }
                else
                {
                    return false;
                }

                //如果获取库体schema成功则开始创建过程
                if (m_pProject != null)
                {
                    IChildItemList pProjects = m_pProject as IChildItemList;
                    //获取属性库集合信息
                    ISchemeItem pDBList = pProjects.get_ItemByName("ATTRDB");
                    IChildItemList pDBLists = pDBList as IChildItemList;
                    //遍历属性库集合
                    long DBNum = pDBLists.GetCount();
                    for (int i = 0; i < DBNum; i++)
                    {
                        m_DSScale = 0;    //比例尺信息

                        //取得属性库信息
                        ISchemeItem pDB = pDBLists.get_ItemByIndex(i);
                        ///获取数据集的比例尺信息，如果获取失败则，取默认比例尺信息
                        IAttribute pa = pDB.AttributeList.get_AttributeByName("Scale") as IAttribute;
                        if (pa == null)
                        {
                            m_DSScale = m_DBScale;
                        }
                        else
                        {
                            string[] DBScaleArayy = pa.Value.ToString().Split(':');
                            m_DSScale = Convert.ToInt32(DBScaleArayy[1]);
                        }

                        IChildItemList pDBs = pDB as IChildItemList;
                        string pDatasetName = pDB.Name;

                        #region 在工作空间中创建数据集，返回数据集对象
                        //创建数据集信息，并输出数据集对象
                        IFeatureDataset pFeatureDataset = null;  //定义数据集用来装载要素类
                        if (createFeatureDataset(pFeatureWorkSpace, pDatasetName, out pFeatureDataset, m_SpatialReference) == false)
                        {
                            return false;
                        }
                        #endregion

                        //遍历属性表
                        long TabNum = pDBs.GetCount();
                        for (int j = 0; j < TabNum; j++)
                        {
                            //获取属性表信息
                            ISchemeItem pTable = pDBs.get_ItemByIndex(j);  //获取属性表对象

                            string pFeatureClassName = pTable.Name;     //要素类名称
                            string pFeatureClassType = pTable.Value as string;   //要素类类型

                            //遍历字段
                            IAttributeList pAttrs = pTable.AttributeList;
                            long FNum = pAttrs.GetCount();

                            //创建用户自定义的字段

                            IFields fields = new FieldsClass();
                            IFieldsEdit fsEdit = fields as IFieldsEdit;

                            //循环属性表中的字段，添加到arcgis的字段对象中
                            for (int k = 0; k < FNum; k++)
                            {
                                //添加自定义属性字段
                                AddCustomusFields(pAttrs, k, fsEdit);
                            }

                            if (pFeatureClassType == "NONE")
                            {
                                //创建非空间表
                            }
                            //创建要素类或者注记
                            else if (pFeatureClassType == "ANNO")  //如果是注记图层
                            {
                                //创建注记层
                                createAnnoFeatureClass(pFeatureClassName, pFeatureWorkSpace, fsEdit, m_DSScale, "注记", pFeatureDataset);
                            }
                            else  //如果是普通要素类图层
                            {
                                //创建普通要素类
                                createCommomFeatureClass(pFeatureClassName, pFeatureClassType, fsEdit, fields, pFeatureDataset);
                            }
                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                    return true;
                }
                else
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                return false;
            }

        }


        /// <summary>
        /// 创建库体主函数
        /// </summary>
        /// <returns></returns>
        public bool CreateDBStruct(List<string> DSName)
        {
            IFeatureWorkspace pFeatureWorkSpace = null;

            try
            {
                //如果工作空间获取成功则赋值要素工作空间
                if (m_WorkSpace != null)
                {
                    pFeatureWorkSpace = m_WorkSpace as IFeatureWorkspace;
                }
                else
                {
                    return false;
                }

                //如果获取库体schema成功则开始创建过程
                if (m_pProject != null)
                {
                    IChildItemList pProjects = m_pProject as IChildItemList;
                    //获取属性库集合信息
                    ISchemeItem pDBList = pProjects.get_ItemByName("ATTRDB");
                    IChildItemList pDBLists = pDBList as IChildItemList;
                    //遍历属性库集合
                    long DBNum = pDBLists.GetCount();
                    for (int i = 0; i < DBNum; i++)
                    {
                        m_DSScale = 0;    //比例尺信息

                        //取得属性库信息
                        ISchemeItem pDB = pDBLists.get_ItemByIndex(i);
                        ///获取数据集的比例尺信息，如果获取失败则，取默认比例尺信息
                        IAttribute pa = pDB.AttributeList.get_AttributeByName("Scale") as IAttribute;
                        if (pa == null)
                        {
                            m_DSScale = m_DBScale;
                        }
                        else
                        {
                            string[] DBScaleArayy = pa.Value.ToString().Split(':');
                            m_DSScale = Convert.ToInt32(DBScaleArayy[1]);
                        }

                        IChildItemList pDBs = pDB as IChildItemList;
                        string pDatasetName = pDB.Name;
                        DSName.Add(pDatasetName);

                        #region 在工作空间中创建数据集，返回数据集对象
                        //创建数据集信息，并输出数据集对象
                        IFeatureDataset pFeatureDataset = null;  //定义数据集用来装载要素类
                        if (createFeatureDataset(pFeatureWorkSpace, pDatasetName, out pFeatureDataset, m_SpatialReference) == false)
                        {
                            return false;
                        }
                        #endregion

                        //遍历属性表
                        long TabNum = pDBs.GetCount();
                        for (int j = 0; j < TabNum; j++)
                        {
                            //获取属性表信息
                            ISchemeItem pTable = pDBs.get_ItemByIndex(j);  //获取属性表对象

                            string pFeatureClassName = pTable.Name;     //要素类名称
                            string pFeatureClassType = pTable.Value as string;   //要素类类型

                            //遍历字段
                            IAttributeList pAttrs = pTable.AttributeList;
                            long FNum = pAttrs.GetCount();

                            //创建用户自定义的字段

                            IFields fields = new FieldsClass();
                            IFieldsEdit fsEdit = fields as IFieldsEdit;

                            //循环属性表中的字段，添加到arcgis的字段对象中
                            for (int k = 0; k < FNum; k++)
                            {
                                //添加自定义属性字段
                                AddCustomusFields(pAttrs, k, fsEdit);
                            }

                            /////添加版本字段，供协同更新版本库使用，陈胜鹏  2010.3.26添加
                            //AddVersionField(fsEdit);

                            if (pFeatureClassType == "NONE")
                            {
                                //创建非空间表
                                //createNonSpatialTable(pFeatureClassName, m_WorkSpace, fsEdit, pFeatureDataset);
                            }
                            //创建要素类或者注记
                            else if (pFeatureClassType == "ANNO")  //如果是注记图层
                            {
                                //创建注记层
                                createAnnoFeatureClass(pFeatureClassName, pFeatureWorkSpace, fsEdit, m_DSScale, "注记", pFeatureDataset);
                            }
                            else  //如果是普通要素类图层
                            {
                                //创建普通要素类
                                createCommomFeatureClass(pFeatureClassName, pFeatureClassType, fsEdit, fields, pFeatureDataset);
                            }
                        }
                    }
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                    return true;
                }
                else
                {
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                //***********************
                //guozheng 2010-12-17 added
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建数据库失败！\n原因：" + e.Message);
                //***********************
                return false;
            }

        }

        /// <summary>
        /// 创建库体主函数
        /// </summary>
        /// <returns></returns>
        public bool CreateDBStruct(List<string> DSName, out int iScale, out string sDBName)
        {
            iScale = -1;
            sDBName = string.Empty;
            IFeatureWorkspace pFeatureWorkSpace = null;

            try
            {
                //如果工作空间获取成功则赋值要素工作空间
                if (m_WorkSpace != null)
                {
                    pFeatureWorkSpace = m_WorkSpace as IFeatureWorkspace;
                }
                else
                {
                    return false;
                }

                //如果获取库体schema成功则开始创建过程
                if (m_pProject != null)
                {
                    IChildItemList pProjects = m_pProject as IChildItemList;
                    //获取属性库集合信息
                    ISchemeItem pDBList = pProjects.get_ItemByName("ATTRDB");
                    IChildItemList pDBLists = pDBList as IChildItemList;
                    //遍历属性库集合
                    long DBNum = pDBLists.GetCount();
                    for (int i = 0; i < DBNum; i++)
                    {
                        m_DSScale = 0;    //比例尺信息

                        //取得属性库信息
                        ISchemeItem pDB = pDBLists.get_ItemByIndex(i);
                        ///获取数据集的比例尺信息，如果获取失败则，取默认比例尺信息
                        IAttribute pa = pDB.AttributeList.get_AttributeByName("Scale") as IAttribute;
                        if (pa == null)
                        {
                            iScale = m_DBScale;
                        }
                        else
                        {
                            string[] DBScaleArayy = pa.Value.ToString().Split(':');
                            m_DSScale = Convert.ToInt32(DBScaleArayy[1]);
                            iScale = m_DSScale;
                        }

                        IChildItemList pDBs = pDB as IChildItemList;
                        string pDatasetName = pDB.Name;
                        sDBName = pDatasetName;
                        DSName.Add(pDatasetName);

                        #region 在工作空间中创建数据集，返回数据集对象
                        //创建数据集信息，并输出数据集对象
                        IFeatureDataset pFeatureDataset = null;  //定义数据集用来装载要素类
                        if (createFeatureDataset(pFeatureWorkSpace, pDatasetName, out pFeatureDataset, m_SpatialReference) == false)
                        {
                            return false;
                        }
                        #endregion

                        //遍历属性表
                        long TabNum = pDBs.GetCount();
                        for (int j = 0; j < TabNum; j++)
                        {
                            //获取属性表信息
                            ISchemeItem pTable = pDBs.get_ItemByIndex(j);  //获取属性表对象

                            string pFeatureClassName = pTable.Name;     //要素类名称
                            string pFeatureClassType = pTable.Value as string;   //要素类类型

                            //遍历字段
                            IAttributeList pAttrs = pTable.AttributeList;
                            long FNum = pAttrs.GetCount();

                            //创建用户自定义的字段

                            IFields fields = new FieldsClass();
                            IFieldsEdit fsEdit = fields as IFieldsEdit;

                            //循环属性表中的字段，添加到arcgis的字段对象中
                            for (int k = 0; k < FNum; k++)
                            {
                                //添加自定义属性字段
                                AddCustomusFields(pAttrs, k, fsEdit);
                            }

                            /////添加版本字段，供协同更新版本库使用，陈胜鹏  2010.3.26添加
                            //AddVersionField(fsEdit);

                            if (pFeatureClassType == "NONE")
                            {
                                //创建非空间表
                            }
                            //创建要素类或者注记
                            else if (pFeatureClassType == "ANNO")  //如果是注记图层
                            {
                                //创建注记层
                                createAnnoFeatureClass(pFeatureClassName, pFeatureWorkSpace, fsEdit, m_DSScale, "注记", pFeatureDataset);
                            }
                            else  //如果是普通要素类图层
                            {
                                //创建普通要素类
                                createCommomFeatureClass(pFeatureClassName, pFeatureClassType, fsEdit, fields, pFeatureDataset);
                            }
                        }
                    }
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                    return true;
                }
                else
                {
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                //***********************
                //guozheng 2010-12-17 added
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建数据库失败！\n原因：" + e.Message);
                //***********************
                return false;
            }

        }

        /// <summary>
        /// 创建库体主函数(进度条重载)  cyf 20110707 modify
        /// </summary>
        /// <returns></returns>
        public bool CreateDBStruct(out string iScale, out List<string> DSName, System.Windows.Forms.ProgressBar in_ProcBar)
        {
            //cyf 20110622 modify
            //iScale = -1;
            iScale = "";
            //end
            //sDBName = string.Empty;  //cyf 20110707 modify
            IFeatureWorkspace pFeatureWorkSpace = null;
            //cyf 20110707 modify
                DSName = new List<string>();
            //end
            try
            {
                //如果工作空间获取成功则赋值要素工作空间
                if (m_WorkSpace != null)
                {
                    pFeatureWorkSpace = m_WorkSpace as IFeatureWorkspace;
                }
                else
                {
                    return false;
                }

                //如果获取库体schema成功则开始创建过程
                if (m_pProject != null)
                {
                    IChildItemList pProjects = m_pProject as IChildItemList;
                    //获取属性库集合信息
                    ISchemeItem pDBList = pProjects.get_ItemByName("ATTRDB");
                    IChildItemList pDBLists = pDBList as IChildItemList;
                    //遍历属性库集合
                    long DBNum = pDBLists.GetCount();
                   
                    for (int i = 0; i < DBNum; i++)
                    {
                        m_DSScale = 0;    //比例尺信息
                        //取得属性库信息
                        ISchemeItem pDB = pDBLists.get_ItemByIndex(i);
                        ///获取数据集的比例尺信息，如果获取失败则，取默认比例尺信息
                        IAttribute pa = pDB.AttributeList.get_AttributeByName("Scale") as IAttribute;
                        if (pa == null)
                        {
                            //cyf 20110622 modify:将比例尺类型改为字符串型
                            iScale = m_DBScale.ToString();
                            //end
                        }
                        else
                        {
                            string[] DBScaleArayy = pa.Value.ToString().Split(':');
                            m_DSScale = Convert.ToInt32(DBScaleArayy[1]);
                            //cyf 20110622 modify:将比例尺类型改为字符串型
                            iScale = m_DSScale.ToString();
                            //end
                        }

                        IChildItemList pDBs = pDB as IChildItemList;
                        string pDatasetName = pDB.Name;
                        //cyf 20110706 modify
                        //sDBName = pDatasetName;
                        //DSName.Add(pDatasetName);
                        //end

                        #region 在工作空间中创建数据集，返回数据集对象
                        //创建数据集信息，并输出数据集对象
                        IFeatureDataset pFeatureDataset = null;  //定义数据集用来装载要素类
                        if (createFeatureDataset(pFeatureWorkSpace, pDatasetName, out pFeatureDataset, m_SpatialReference) == false)
                        {
                            return false;
                        }
                        #endregion
                        //cyf 20110706 modify
                        pDatasetName = (pFeatureDataset as IDataset).Name;
                        //sDBName = pDatasetName;  //cyf 20110707 modify
                        DSName.Add(pDatasetName);
                        //end

                        //遍历属性表
                        long TabNum = pDBs.GetCount();
                        /////////////////////////////////////////进度条//////////////////////
                        if (in_ProcBar != null)
                        {
                            in_ProcBar.Maximum = (int)TabNum;
                            in_ProcBar.Value = 0;
                        }
                        for (int j = 0; j < TabNum; j++)
                        {
                            //获取属性表信息
                            if (in_ProcBar != null)
                            {
                                in_ProcBar.Value = j;
                                Application.DoEvents();
                            }
                            ISchemeItem pTable = pDBs.get_ItemByIndex(j);  //获取属性表对象

                            string pFeatureClassName = pTable.Name;     //要素类名称
                            string pFeatureClassType = pTable.Value as string;   //要素类类型

                            //遍历字段
                            IAttributeList pAttrs = pTable.AttributeList;
                            long FNum = pAttrs.GetCount();

                            //创建用户自定义的字段

                            IFields fields = new FieldsClass();
                            IFieldsEdit fsEdit = fields as IFieldsEdit;

                            //循环属性表中的字段，添加到arcgis的字段对象中
                            for (int k = 0; k < FNum; k++)
                            {
                                //添加自定义属性字段
                                AddCustomusFields(pAttrs, k, fsEdit);
                            }

                            /////添加版本字段，供协同更新版本库使用，陈胜鹏  2010.3.26添加
                            //AddVersionField(fsEdit);

                            if (pFeatureClassType == "NONE")
                            {
                                //创建非空间表
                            }
                            //创建要素类或者注记
                            else if (pFeatureClassType == "ANNO")  //如果是注记图层
                            {
                                //创建注记层
                                createAnnoFeatureClass(pFeatureClassName, pFeatureWorkSpace, fsEdit, m_DSScale, "注记", pFeatureDataset);
                            }
                            else  //如果是普通要素类图层
                            {
                                //创建普通要素类
                                createCommomFeatureClass(pFeatureClassName, pFeatureClassType, fsEdit, fields, pFeatureDataset);
                            }
                        }
                    }
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                    return true;
                }
                else
                {
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureWorkSpace);
                //***********************
                //guozheng 2010-12-17 added
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建数据库失败！\n原因：" + e.Message);
                //***********************
                return false;
            }

        }
        /// <summary>
        /// 创建远程日志表 陈亚飞添加
        /// </summary>
        /// <param name="pDBType">工作空间类型：PDB、GDB、SDE</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public bool CreateSQLTable(string pDBType, out Exception eError)
        {
            //创建远程日志表
            eError = null;

            //检查远程日志表是否存在，只要有一张表格存在，就返回
            ITable pTable = null;
            ITable mTable = null;
            IFeatureWorkspace pFeaWS = m_WorkSpace as IFeatureWorkspace;
            if (pFeaWS == null) return false;
            //cyf 20110622
            try
            {
                pTable = pFeaWS.OpenTable("GO_DATABASE_UPDATELOG");
            }
            catch
            {
            }
            if (pTable == null)
            {
                //若表格为空，则创建表格
                //创建表格
                try
                {
                    if (pDBType.ToUpper() == "PDB")
                    {
                        m_WorkSpace.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  integer,STATE integer,LAYERNAME varchar(50),USERNAME varchar(255),LASTUPDATE date,VERSION integer,XMIN float,XMAX float,YMIN float,YMAX float)");
                        //m_WorkSpace.ExecuteSQL("create table go_database_version (VERSION  integer,USERNAME varchar(255),VERSIONTIME date,DES varchar(255))");

                    }
                    else if (pDBType.ToUpper() == "SDE")
                    {
                        m_WorkSpace.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  INTEGER,STATE INTEGER,LAYERNAME NVARCHAR2(50),USERNAME NVARCHAR2(255),LASTUPDATE DATE,VERSION INTEGER,XMIN FLOAT,XMAX FLOAT,YMIN FLOAT,YMAX FLOAT)");
                        //m_WorkSpace.ExecuteSQL("create table go_database_version (VERSION  INTEGER,USERNAME NVARCHAR2(255),VERSIONTIME DATE,DES NVARCHAR2(255))");
                    }
                    else if (pDBType.ToUpper() == "GDB")
                    {
                        string tempFile = netLogPath;
                        FileInfo pFI = new FileInfo(tempFile);
                        string fName = pFI.Name;  //远程日志表名

                        //日志存储路径
                        string dbPath = m_WorkSpace.PathName;
                        int index = dbPath.LastIndexOf('\\');
                        if (index == -1) return false;
                        string FileDic = dbPath.Substring(0, index);
                        string FileName = FileDic + "\\" + fName;

                        if (File.Exists(FileName))
                        {
                            File.Delete(FileName);
                        }
                        File.Copy(tempFile, FileName);
                    }
                } catch (System.Exception ex)
                {
                    eError = ex;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaWS);
                    return false;
                }
            }
            try
            {
                mTable = pFeaWS.OpenTable("go_database_version");
            } catch
            {
            }
            if (mTable == null)
            {
                //若表格为空，则创建表格
                try
                {
                    if (pDBType.ToUpper() == "PDB")
                    {
                        //m_WorkSpace.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  integer,STATE integer,LAYERNAME varchar(50),USERNAME varchar(255),LASTUPDATE date,VERSION integer,XMIN float,XMAX float,YMIN float,YMAX float)");
                        m_WorkSpace.ExecuteSQL("create table go_database_version (VERSION  integer,USERNAME varchar(255),VERSIONTIME date,DES varchar(255))");

                    }
                    else if (pDBType.ToUpper() == "SDE")
                    {
                        //m_WorkSpace.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  INTEGER,STATE INTEGER,LAYERNAME NVARCHAR2(50),USERNAME NVARCHAR2(255),LASTUPDATE DATE,VERSION INTEGER,XMIN FLOAT,XMAX FLOAT,YMIN FLOAT,YMAX FLOAT)");
                        m_WorkSpace.ExecuteSQL("create table go_database_version (VERSION  INTEGER,USERNAME NVARCHAR2(255),VERSIONTIME DATE,DES NVARCHAR2(255))");
                    }
                    else if (pDBType.ToUpper() == "GDB")
                    {
                        string tempFile = netLogPath;
                        FileInfo pFI = new FileInfo(tempFile);
                        string fName = pFI.Name;  //远程日志表名

                        //日志存储路径
                        string dbPath = m_WorkSpace.PathName;
                        int index = dbPath.LastIndexOf('\\');
                        if (index == -1) return false;
                        string FileDic = dbPath.Substring(0, index);
                        string FileName = FileDic + "\\" + fName;

                        if (File.Exists(FileName))
                        {
                            File.Delete(FileName);
                        }
                        File.Copy(tempFile, FileName);
                    }
                } catch (System.Exception ex)
                {
                    eError = ex;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaWS);
                    return false;
                }
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaWS);
            return true;
            //创建表格
            //try
            //{
            //    if (pDBType.ToUpper() == "PDB")
            //    {
            //        m_WorkSpace.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  integer,STATE integer,LAYERNAME varchar(50),USERNAME varchar(255),LASTUPDATE date,VERSION integer,XMIN float,XMAX float,YMIN float,YMAX float)");
            //        m_WorkSpace.ExecuteSQL("create table go_database_version (VERSION  integer,USERNAME varchar(255),VERSIONTIME date,DES varchar(255))");

            //    }
            //    else if (pDBType.ToUpper() == "SDE")
            //    {
            //        m_WorkSpace.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  INTEGER,STATE INTEGER,LAYERNAME NVARCHAR2(50),USERNAME NVARCHAR2(255),LASTUPDATE DATE,VERSION INTEGER,XMIN FLOAT,XMAX FLOAT,YMIN FLOAT,YMAX FLOAT)");
            //        m_WorkSpace.ExecuteSQL("create table go_database_version (VERSION  INTEGER,USERNAME NVARCHAR2(255),VERSIONTIME DATE,DES NVARCHAR2(255))");
            //    }
            //    else if (pDBType.ToUpper() == "GDB")
            //    {
            //        string tempFile = netLogPath;
            //        FileInfo pFI = new FileInfo(tempFile);
            //        string fName = pFI.Name;  //远程日志表名

            //        //日志存储路径
            //        string dbPath = m_WorkSpace.PathName;
            //        int index = dbPath.LastIndexOf('\\');
            //        if (index == -1) return false;
            //        string FileDic = dbPath.Substring(0, index);
            //        string FileName = FileDic + "\\" + fName;

            //        if (File.Exists(FileName))
            //        {
            //            if (!SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "日志文件'" + fName + "'已存在,\n是否替换？"))
            //            {
            //                return true;
            //            }
            //            else
            //            {
            //                File.Delete(FileName);
            //            }
            //        }
            //        File.Copy(tempFile, FileName);
            //    }
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaWS);
            //    return true;
            //}
            //catch (System.Exception ex)
            //{
            //    eError = ex;
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaWS);
            //    return false;
            //}
            //end
        }

        /// <summary>
        /// 添加版本字段，陈胜鹏  2010.3.25添加
        /// </summary>
        /// <param name="fsEdit"></param>
        private void AddVersionField(IFieldsEdit fsEdit)
        {
            try
            {
                IField newfield = new FieldClass();                //字段对象
                IFieldEdit fieldEdit = newfield as IFieldEdit;     //字段编辑对象

                //以下变量用来定义字段的属性
                string fieldName = "VERSION";//记录字段名称
                string fieldType = "esriFieldTypeInteger";//记录字段类型
                int fieldLen;//记录字段长度
                bool isNullable = false;//记录字段是否允许空值
                int precision = 0;//精度

                bool required = false;
                bool editable = true;
                bool domainfixed = false;   //值域是否可以改变

                fieldEdit.Name_2 = fieldName;
                fieldEdit.AliasName_2 = fieldName;
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = (esriFieldType)Enum.Parse(typeof(esriFieldType), fieldType, true);
                fieldEdit.IsNullable_2 = isNullable;
                fieldEdit.DefaultValue_2 = 0;  //默认值

                fieldEdit.Required_2 = required;
                fieldEdit.Editable_2 = editable;
                fieldEdit.DomainFixed_2 = domainfixed;
                newfield = fieldEdit as IField;
                fsEdit.AddField(newfield);
                return;
            }
            catch
            {
                fsEdit = null;
                return;
            }
        }

        /// <summary>
        /// 在指定数据集下创建普通要素类
        /// </summary>
        /// <param name="pFeatureClassName">要素类名称</param>
        /// <param name="pFeatureClassType">要素类类型</param>
        /// <param name="fsEdit">编辑字段对象</param>
        /// <param name="fields">字段集合对象</param>
        /// <param name="pFeatureDataset">数据集对象</param>
        private void createCommomFeatureClass(string pFeatureClassName, string pFeatureClassType, IFieldsEdit fsEdit, IFields fields, IFeatureDataset pFeatureDataset)
        {
            try
            {
                string pCommonFeatureType = null;

                switch (pFeatureClassType)
                {
                    case "POINT":
                        pCommonFeatureType = "点";
                        break;
                    case "3DPOINT":
                        pCommonFeatureType = "3D点";
                        break;
                    case "LINE":
                        pCommonFeatureType = "线";
                        break;
                    case "3DLINE":
                        pCommonFeatureType = "3D线";
                        break;
                    case "AREA":
                        pCommonFeatureType = "面";
                        break;
                    case "3DAREA":
                        pCommonFeatureType = "3D面";
                        break;
                    default:
                        break;
                }
                //创建普通图层
                # region 创建普通featureClass的特殊字段
                //添加Object字段
                IField newfield2 = new FieldClass();
                IFieldEdit fieldEdit2 = newfield2 as IFieldEdit;
                fieldEdit2.Name_2 = "OBJECTID";
                fieldEdit2.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldEdit2.AliasName_2 = "OBJECTID";
                fieldEdit2.IsNullable_2 = false;
                fieldEdit2.Required_2 = true;
                fieldEdit2.Editable_2 = false;
                newfield2 = fieldEdit2 as IField;
                fsEdit.AddField(newfield2);

                ISpatialReference pSR = null;
                IGeoDataset GeoDataset = pFeatureDataset as IGeoDataset;
                pSR = GeoDataset.SpatialReference;

                //添加Geometry字段
                IField newfield1 = new FieldClass();
                newfield1 = GetGeometryField(newfield1, pCommonFeatureType, pSR);
                if (newfield1 == null) return;
                fsEdit.AddField(newfield1);
                fields = fsEdit as IFields;

                #endregion
                pFeatureDataset.CreateFeatureClass(pFeatureClassName, fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// 根据自定义的字段，在指定工作空间的指定数据集下创建注记图层
        /// 并设置参考比例尺
        /// </summary>
        /// <param name="feaName">创建的注记名称</param>
        /// <param name="feaworkspace">作业工作空间</param>
        /// <param name="fsEditAnno">注记字段对象</param>
        /// <param name="intScale">注记参考比例尺</param>
        /// <param name="shapeType">要素类型，这里输入“注记”以获取注记的Geometry字段</param>
        /// <param name="pFeatureDataset">将注记层生成到该数据集下</param>
        private void createAnnoFeatureClass(string feaName, IFeatureWorkspace feaworkspace, IFieldsEdit fsEditAnno, int intScale, string shapeType, IFeatureDataset pFeatureDataset)
        {
            //创建注记的特殊字段
            try
            {
                //注记的workSpace
                IFeatureWorkspaceAnno pFWSAnno = feaworkspace as IFeatureWorkspaceAnno;

                IGraphicsLayerScale pGLS = new GraphicsLayerScaleClass();
                pGLS.Units = esriUnits.esriMeters;
                pGLS.ReferenceScale = Convert.ToDouble(intScale);//创建注记必须要设置比例尺

                IFormattedTextSymbol myTextSymbol = new TextSymbolClass();
                ISymbol pSymbol = (ISymbol)myTextSymbol;
                //Anno要素类必须有的缺省符号
                ISymbolCollection2 pSymbolColl = new SymbolCollectionClass();
                ISymbolIdentifier2 pSymID = new SymbolIdentifierClass();
                pSymbolColl.AddSymbol(pSymbol, "Default", out pSymID);

                //Anno要素类的必要属性
                IAnnotateLayerProperties pAnnoProps = new LabelEngineLayerPropertiesClass();
                pAnnoProps.CreateUnplacedElements = true;
                pAnnoProps.CreateUnplacedElements = true;
                pAnnoProps.DisplayAnnotation = true;
                pAnnoProps.UseOutput = true;

                ILabelEngineLayerProperties pLELayerProps = (ILabelEngineLayerProperties)pAnnoProps;
                pLELayerProps.Symbol = pSymbol as ITextSymbol;
                pLELayerProps.SymbolID = 0;
                pLELayerProps.IsExpressionSimple = true;
                pLELayerProps.Offset = 0;
                pLELayerProps.SymbolID = 0;

                IAnnotationExpressionEngine aAnnoVBScriptEngine = new AnnotationVBScriptEngineClass();
                pLELayerProps.ExpressionParser = aAnnoVBScriptEngine;
                pLELayerProps.Expression = "[DESCRIPTION]";
                IAnnotateLayerTransformationProperties pATP = (IAnnotateLayerTransformationProperties)pAnnoProps;
                pATP.ReferenceScale = pGLS.ReferenceScale;
                pATP.ScaleRatio = 1;

                IAnnotateLayerPropertiesCollection pAnnoPropsColl = new AnnotateLayerPropertiesCollectionClass();
                pAnnoPropsColl.Add(pAnnoProps);

                IObjectClassDescription pOCDesc = new AnnotationFeatureClassDescription();
                IFields fields = pOCDesc.RequiredFields;
                IFeatureClassDescription pFDesc = pOCDesc as IFeatureClassDescription;

                for (int j = 0; j < pOCDesc.RequiredFields.FieldCount; j++)
                {
                    IField tempField = pOCDesc.RequiredFields.get_Field(j);
                    if (tempField.Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        continue;
                    }
                    fsEditAnno.AddField(tempField);
                }
                ISpatialReference pSR = null;
                IGeoDataset GeoDataset = pFeatureDataset as IGeoDataset;
                pSR = GeoDataset.SpatialReference;


                //根据xml文件，Geometry字段可能带有空间参考，因此单独添加Geometry字段
                //添加Geometry字段
                IField newfield1 = new FieldClass();
                newfield1 = GetGeometryField(newfield1, shapeType, pSR);
                if (newfield1 == null) return;
                fsEditAnno.AddField(newfield1);

                fields = fsEditAnno as IFields;
                pFWSAnno.CreateAnnotationClass(feaName, fields, pOCDesc.InstanceCLSID, pOCDesc.ClassExtensionCLSID, pFDesc.ShapeFieldName, "", pFeatureDataset, null, pAnnoPropsColl, pGLS, pSymbolColl, true);
            }
            catch
            {
                return;
            }
        }

        private void createNonSpatialTable(string feaName, IWorkspace pWS, IFieldEdit fsEdit, IFeatureDataset pFeaDataset)
        {
            //string strSql = "create table " + feaName + " (";
            //if (pDBType.ToUpper() == "PDB")
            //{

            //    //pWS.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  integer,STATE integer,LAYERNAME varchar(50),LASTUPDATE date,VERSION integer,XMIN float,XMAX float,YMIN float,YMAX float)");

            //}
            //else if (pDBType.ToUpper() == "SDE")
            //{
            //    //pWS.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  INTEGER,STATE INTEGER,LAYERNAME NVARCHAR2(50),LASTUPDATE DATE,VERSION INTEGER,XMIN FLOAT,XMAX FLOAT,YMIN FLOAT,YMAX FLOAT)");
            //}
            //else if (pDBType.ToUpper() == "GDB")
            //{
            //    //GDB存储不了非空间数据
            //}
        }

        /// <summary>
        /// 获得图形字段
        /// </summary>
        /// <param name="newfield1">字段对象</param>
        /// <param name="shapeType">图形类型</param>
        /// <param name="pSR">空间参考对象</param>
        /// <returns>返回图形字段</returns>
        private IField GetGeometryField(IField newfield1, string shapeType, ISpatialReference pSR)
        {
            IFieldEdit fieldEdit1 = newfield1 as IFieldEdit;
            fieldEdit1.Name_2 = "SHAPE";
            fieldEdit1.Type_2 = esriFieldType.esriFieldTypeGeometry;
            IGeometryDef geoDef = new GeometryDefClass();
            IGeometryDefEdit geoDefEdit = geoDef as IGeometryDefEdit;
            if (shapeType == "点")
            {
                geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
            }
            else if (shapeType == "线")
            {
                geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;
            }
            else if (shapeType == "面")
            {
                geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            }
            else if (shapeType == "注记")
            {
                geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            }
            else if (shapeType == "3D点")
            {
                geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
                geoDefEdit.HasM_2 = false;
                geoDefEdit.HasZ_2 = true;
            }
            else if (shapeType == "3D线")
            {
                geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;
                geoDefEdit.HasM_2 = false;
                geoDefEdit.HasZ_2 = true;
            }
            else if (shapeType == "3D面")
            {
                geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                geoDefEdit.HasM_2 = false;
                geoDefEdit.HasZ_2 = true;
            }


            try
            {
                geoDefEdit.SpatialReference_2 = pSR;
                fieldEdit1.GeometryDef_2 = geoDefEdit as GeometryDef;
                newfield1 = fieldEdit1 as IField;
                return newfield1;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 添加自定义字段函数
        /// </summary>
        /// <param name="pAttrs">库体结构的属性表集合</param>
        /// <param name="k">属性表索引号</param>
        /// <param name="fsEdit">输出的自定义字段对象</param>
        private void AddCustomusFields(IAttributeList pAttrs, int k, IFieldsEdit fsEdit)
        {
            try
            {
                IField newfield = new FieldClass();                //字段对象
                IFieldEdit fieldEdit = newfield as IFieldEdit;     //字段编辑对象

                //获取基本属性信息
                IAttribute pAttr = pAttrs.get_AttributeByIndex(k);

                //获取扩展属性信息
                IAttributeDes pAttrDes = pAttr.Description;

                //以下变量用来定义字段的属性
                string fieldName = "";//记录字段名称
                esriFieldType fieldType = esriFieldType.esriFieldTypeString;//记录字段类型
                //string fieldType = "";
                int fieldLen;//记录字段长度
                bool isNullable = true;//记录字段是否允许空值
                int precision = 0;//精度

                bool required = false;
                bool editable = true;
                bool domainfixed = false;   //值域是否可以改变

                //获得字段的属性
                fieldName = pAttr.Name;

                //======chenayfei modify 增加BLOB字段的定义============================================
                //fieldType = pAttr.Type.ToString();
                //根据字段类型记录arcgis中对应的字段类型
                switch (pAttr.Type)
                {
                    case GeoOneDataCheck.VALUETYPE.GO_VALUETYPE_STRING:
                        fieldType = esriFieldType.esriFieldTypeString;// "esriFieldTypeString";
                        try
                        {
                            fieldEdit.DefaultValue_2 = pAttr.Value.ToString();  //默认值
                        }
                        catch
                        {
                        }
                        break;
                    case GeoOneDataCheck.VALUETYPE.GO_VALUETYPE_LONG:// "GO_VALUETYPE_LONG":
                        fieldType = esriFieldType.esriFieldTypeInteger;// "esriFieldTypeInteger";
                        try
                        {
                            fieldEdit.DefaultValue_2 = Convert.ToInt32(pAttr.Value.ToString());  //默认值
                        }
                        catch
                        {
                        }
                        break;
                    case GeoOneDataCheck.VALUETYPE.GO_VALUETYPE_BOOL:// "GO_VALUETYPE_BOOL":
                        fieldType = esriFieldType.esriFieldTypeSmallInteger;// "esriFieldTypeSmallInteger";
                        try
                        {
                            fieldEdit.DefaultValue_2 = Convert.ToBoolean(pAttr.Value.ToString()); //默认值
                        }
                        catch
                        {
                        }
                        break;
                    case GeoOneDataCheck.VALUETYPE.GO_VALUETYPE_DATE:// "GO_VALUETYPE_DATE":
                        fieldType = esriFieldType.esriFieldTypeDate;// "esriFieldTypeDate";
                        try
                        {
                            fieldEdit.DefaultValue_2 = Convert.ToDateTime(pAttr.Value.ToString());  //默认值
                        }
                        catch
                        {
                            fieldEdit.DefaultValue_2 = "";
                        }

                        break;
                    case GeoOneDataCheck.VALUETYPE.GO_VALUETYPE_DATETIME:// "GO_VALUETYPE_DATE":
                        fieldType = esriFieldType.esriFieldTypeDate;// "esriFieldTypeDate";
                        try
                        {
                            fieldEdit.DefaultValue_2 = Convert.ToDateTime(pAttr.Value.ToString());  //默认值
                        }
                        catch
                        {
                            fieldEdit.DefaultValue_2 = "";
                        }
                        break;
                    case GeoOneDataCheck.VALUETYPE.GO_VALUETYPE_FLOAT:// "GO_VALUETYPE_FLOAT":
                        fieldType = esriFieldType.esriFieldTypeSingle;// "esriFieldTypeSingle";
                        try
                        {
                            fieldEdit.DefaultValue_2 = Convert.ToSingle(pAttr.Value.ToString());//默认值
                        }
                        catch
                        {
                        }
                        break;
                    case GeoOneDataCheck.VALUETYPE.GO_VALUETYPE_DOUBLE:// "GO_VALUETYPE_DOUBLE":
                        fieldType = esriFieldType.esriFieldTypeDouble;// "esriFieldTypeDouble";
                        try
                        {
                            fieldEdit.DefaultValue_2 = Convert.ToDouble(pAttr.Value.ToString());  //默认值
                        }
                        catch
                        {
                        }
                        break;
                    case GeoOneDataCheck.VALUETYPE.GO_VALUETYPE_BYTE:// "GO_VALUETYPE_DOUBLE":
                        fieldType = esriFieldType.esriFieldTypeBlob;// "esriFieldTypeDouble";
                        try
                        {
                            fieldEdit.DefaultValue_2 = pAttr.Value.ToString();  //默认值
                        }
                        catch
                        {
                            fieldEdit.DefaultValue_2 = null;
                        }
                        break;

                    default:
                        break;
                }

                isNullable = pAttrDes.AllowNull;
                fieldLen = Convert.ToInt32(pAttrDes.InputWidth);
                precision = Convert.ToInt32(pAttrDes.PrecisionEx);

                required = bool.Parse(pAttrDes.Necessary.ToString());

                fieldEdit.Name_2 = fieldName;
                fieldEdit.AliasName_2 = fieldName;
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = fieldType;// (esriFieldType)Enum.Parse(typeof(esriFieldType), fieldType, true);
                fieldEdit.IsNullable_2 = isNullable;
                fieldEdit.Length_2 = fieldLen;
                //fieldEdit.DefaultValue_2 =  pAttr.Value;  //默认值

                //双精度类型不可设置精度（在PDB和GDB中不会出现错误，但是在SDE中会抛出“无效的列”错误）
                if (fieldType != esriFieldType.esriFieldTypeDouble)// "esriFieldTypeDouble")
                {
                    fieldEdit.Precision_2 = precision;
                }

                //========================================================================

                fieldEdit.Required_2 = required;
                fieldEdit.Editable_2 = editable;
                fieldEdit.DomainFixed_2 = domainfixed;
                newfield = fieldEdit as IField;
                fsEdit.AddField(newfield);
                return;
            }
            catch
            {
                fsEdit = null;
                return;
            }
        }
        /// <summary>
        /// 创建数据集
        /// </summary>
        /// <param name="pFeatureWorkSpace">工作空间对象</param>
        /// <param name="pDatasetName">数据集名称</param>
        /// <param name="pFeatureDataset">输出的数据集对象</param>
        /// <param name="ProjectFilePath">空间参考文件路径</param>
        /// <returns></returns>
        private bool createFeatureDataset(IFeatureWorkspace pFeatureWorkSpace, string pDatasetName, out IFeatureDataset pFeatureDataset, ISpatialReference pSR)
        {
            try
            {
                //ISpatialReference pSR = null;
                //ISpatialReferenceFactory pSpatialRefFac = new SpatialReferenceEnvironmentClass();

                //if (!File.Exists(ProjectFilePath))
                //{
                //    pFeatureDataset = null;
                //    return false;
                //}
                //pSR = pSpatialRefFac.CreateESRISpatialReferenceFromPRJFile(ProjectFilePath);

                //ISpatialReferenceResolution pSRR = pSR as ISpatialReferenceResolution;
                //ISpatialReferenceTolerance pSRT = (ISpatialReferenceTolerance)pSR;
                //IControlPrecision2 pSpatialPrecision = (IControlPrecision2)pSR;

                //pSRR.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon
                //pSRR.SetDefaultXYResolution();
                //pSRT.SetDefaultXYTolerance();

                pFeatureDataset = pFeatureWorkSpace.CreateFeatureDataset(pDatasetName, pSR);
                if (pFeatureDataset == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                pFeatureDataset = null;
                return false;
            }
        }

        #endregion
    }

    public static class ModGisPub
    {
        /// <summary>
        /// 从坐标字典得到范围Polygon
        /// </summary>
        /// <param name="diccoor">坐标字典value为X@Y</param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool GetPolygonByCol(Dictionary<int, string> diccoor, out IPolygon polygon, out Exception eError)
        {
            eError = null;
            object after = Type.Missing;
            object before = Type.Missing;
            polygon = new PolygonClass();
            IPointCollection pPointCol = (IPointCollection)polygon;

            try
            {
                for (int index = 0; index < diccoor.Count; index++)
                {
                    string CoorLine = diccoor[index];
                    string[] coors = CoorLine.Split('@');

                    double X = Convert.ToDouble(coors[0]);
                    double Y = Convert.ToDouble(coors[1]);

                    IPoint pPoint = new PointClass();
                    pPoint.PutCoords(X, Y);
                    pPointCol.AddPoint(pPoint, ref before, ref after);
                }

                polygon = (IPolygon)pPointCol;
                polygon.Close();

                if (!IsValidateGeometry(polygon))
                {
                    eError = new Exception("几何不符合要求");
                    return false;
                }
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }

            return true;
        }
        /// <summary>
        ///  返回指向单一字段的ICursor xisheng 20120612
        /// </summary>
        /// <param name="pWorkspace"></param>
        /// <param name="sTableName"></param>
        /// <param name="sQueryString"></param>
        /// <param name="sFieldName"></param>
        /// <returns></returns>
        public static ICursor GetQueryCursor(IWorkspace workspace, string tableName, string queryString, string[] fieldNames)
        {
            if (workspace == null) return null;

            try
            {
                IFeatureWorkspace pFeaWorkspace = workspace as IFeatureWorkspace;
                ITable pTable = pFeaWorkspace.OpenTable(tableName);

                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = queryString;
                pQueryFilter.SubFields = fieldNames == null ? "*" : string.Join(",", fieldNames);

                return pTable.Search(pQueryFilter, false);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 检测一个几何体是否非法
        /// </summary>
        /// <param name="pgeometry">几何体</param>
        /// <returns></returns>
        public static bool IsValidateGeometry(IGeometry pgeometry)
        {
            // 获取此Geometry的原始点数
            IPointCollection pOrgPointCol = (IPointCollection)pgeometry;

            // 获取此Geometry的原始Part数
            IGeometryCollection pOrgGeometryCol = (IGeometryCollection)pgeometry;

            // 对目标进行克隆和对应的处理
            ESRI.ArcGIS.esriSystem.IClone pClone = (ESRI.ArcGIS.esriSystem.IClone)pgeometry;
            IGeometry pGeometryTemp = (IPolygon)pClone.Clone();
            ITopologicalOperator pTopo = (ITopologicalOperator)pGeometryTemp;
            pTopo.Simplify();

            // 得到新的Geometry
            pGeometryTemp = (IPolygon)pTopo;

            // 获取新的Geometry的点数
            IPointCollection pObjPointCol = (IPointCollection)pGeometryTemp;

            // 获取新的Geometry的Part数
            IGeometryCollection pObjGeometryCol = (IGeometryCollection)pGeometryTemp;

            // 进行比较
            if (pOrgPointCol.PointCount != pObjPointCol.PointCount) return false;

            if (pOrgGeometryCol.GeometryCount != pObjGeometryCol.GeometryCount) return false;

            return true;
        }

        /// <summary>
        /// 缩放到Feature
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="pFeature"></param>
        public static void ZoomToFeature(IMapControlDefault pMapControl, IFeature pFeature)
        {
            if (pFeature == null) return;
            if (pFeature.Shape == null) return;
            IEnvelope pEnvelope = null;
			/*xisheng 20110802 changed*/
            if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                //ITopologicalOperator pTop = pFeature.Shape as ITopologicalOperator;
                //IGeometry pGeometry = pTop.Buffer(50);
                //pEnvelope = pGeometry.Envelope;

                IActiveView pActiveView = pMapControl.Map as IActiveView;
                //if (!GetPointInEnvelope(pActiveView.Extent, pFeature.Shape as IPoint))
                //{//pActiveView.Extent = pEnvelope; 
                    pMapControl.CenterAt(pFeature.Shape as IPoint);
                //}
                pActiveView.Refresh();
            }
            else
            {
                pEnvelope = pFeature.Extent;
            

	            if (pEnvelope == null) return;
	            pEnvelope.Expand(1.5, 1.5, true);
	            IActiveView pActiveView = pMapControl.Map as IActiveView;
	            pActiveView.Extent = pEnvelope;
	            pActiveView.Refresh();
			}
            /*xisheng 20110802 changed end*/
        }

        /// <summary>
        /// 查看点是否包含在范围里面
        /// </summary>
        /// <param name="pEnvelop">矩形范围</param>
        /// <param name="point">点</param>
        /// <returns></returns>
        private static bool GetPointInEnvelope(IEnvelope pEnvelop, IPoint point)
        {
            if (point.X > pEnvelop.XMax)
            {
                return false;
            }
            if(point.X < pEnvelop.XMin)
            {
                return false;
            }
            if(point.Y < pEnvelop.YMin)
            {
                return false;
            }
            if(point.Y > pEnvelop.YMax)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 缩放到Feature  
        /// chenyafei  20110328  modify   :修改定位
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="pFeature"></param>
        public static void ZoomToFeature(IMapControlDefault pMapControl, IFeature pFeature,ISpatialReference pSpatialRef)
        {
            if (pFeature == null) return;
            if (pFeature.Shape == null) return;
            IEnvelope pEnvelope = null;
            double pDis = 50;
            IGeographicCoordinateSystem pGeoCoorSys = null;
            if (pSpatialRef != null)
            {
                pGeoCoorSys = pSpatialRef as IGeographicCoordinateSystem;
            }
            if (pGeoCoorSys != null)
            {
                //地理坐标系，进行坐标转换
                IUnitConverter pUnitConverter = new UnitConverterClass();
                pDis = pUnitConverter.ConvertUnits(pDis, esriUnits.esriMeters, esriUnits.esriDecimalDegrees);
            }
            
            if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                ITopologicalOperator pTop = pFeature.Shape as ITopologicalOperator;
                IGeometry pGeometry = pTop.Buffer(pDis);
                pEnvelope = pGeometry.Envelope;
            }
            else
            {
                pEnvelope = pFeature.Extent;
            }

            if (pEnvelope == null) return;
            pEnvelope.Expand(1.5, 1.5, true);
            IActiveView pActiveView = pMapControl.Map as IActiveView;
            pActiveView.Extent = pEnvelope;
            pActiveView.Refresh();
        }

        /// <summary>
        /// 设置图层渲染UniqueValueRenderer
        /// </summary>
        /// <param name="pFeatLay">渲染图层</param>
        /// <param name="strFieldName">渲染字段</param>
        /// <param name="dicFieldValue">渲染值对(字段值,渲染名称)</param>
        /// <param name="dicFieldSymbol">渲染Symbol对(字段值,Symbol)</param>
        public static void SetLayerUniqueValueRenderer(IFeatureLayer pFeatLay, string strFieldName, Dictionary<string, string> dicFieldValue, Dictionary<string, ISymbol> dicFieldSymbol, bool bUseDefaultSymbol)
        {
            if (pFeatLay == null || strFieldName == string.Empty || dicFieldValue == null || dicFieldSymbol == null) return;
            IFeatureClass pFeatCls = pFeatLay.FeatureClass;
            IUniqueValueRenderer pUniqueValueRenderer = new UniqueValueRendererClass();
            pUniqueValueRenderer.FieldCount = 1;
            pUniqueValueRenderer.set_Field(0, strFieldName);
            if (bUseDefaultSymbol == true)
            {
                pUniqueValueRenderer.UseDefaultSymbol = true;
            }
            else
            {
                pUniqueValueRenderer.UseDefaultSymbol = false;
            }
            foreach (KeyValuePair<string, string> keyValue in dicFieldValue)
            {
                if (dicFieldSymbol.ContainsKey(keyValue.Key))
                {
                    pUniqueValueRenderer.AddValue(keyValue.Key, "", dicFieldSymbol[keyValue.Key]);
                    pUniqueValueRenderer.set_Label(keyValue.Key, keyValue.Value);
                }
            }

            IGeoFeatureLayer pGeoFeatLay = pFeatLay as IGeoFeatureLayer;
            if (pGeoFeatLay != null) pGeoFeatLay.Renderer = pUniqueValueRenderer as IFeatureRenderer;
        }

        /// <summary>
        /// 获取RGB
        /// </summary>
        /// <param name="lngR"></param>
        /// <param name="lngG"></param>
        /// <param name="lngB"></param>
        /// <returns></returns>
        public static IRgbColor GetRGBColor(int lngR, int lngG, int lngB)
        {
            IRgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = lngR;
            rgbColor.Green = lngG;
            rgbColor.Blue = lngB;
            rgbColor.UseWindowsDithering = false;

            return rgbColor;
        }

        /// <summary>
        /// 高亮显示地物
        /// </summary>
        /// <param name="pGeometry">地物图形</param>
        /// <param name="pActiveView">显示控件</param>
        public static void FlashGeometry(IGeometry pGeometry, IActiveView pActiveView)
        {
            if (pGeometry == null || pActiveView == null) return;
            //如果当前图层的空间参考与当前地图的空间参考不一样,则出现不了闪烁的效果
            pGeometry.Project(pActiveView.FocusMap.SpatialReference);
            IRgbColor pRgbColor = GetRGBColor(255, 150, 150);
            ISymbol pSymbol = null;

            pActiveView.ScreenDisplay.StartDrawing(0, -1);
            switch (pGeometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
                    pSimpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    pSimpleMarkerSymbol.Color = pRgbColor;
                    pSymbol = pSimpleMarkerSymbol as ISymbol;
                    pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
                    pActiveView.ScreenDisplay.SetSymbol(pSymbol);
                    pActiveView.ScreenDisplay.DrawPoint(pGeometry);
                    TimeSpan pPointTimeSpan = new TimeSpan(150);
                    pActiveView.ScreenDisplay.DrawPoint(pGeometry);
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
                    pSimpleLineSymbol.Color = pRgbColor;
                    pSimpleLineSymbol.Width = 4;
                    pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                    pSymbol = pSimpleLineSymbol as ISymbol;
                    pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
                    pActiveView.ScreenDisplay.SetSymbol(pSymbol);
                    pActiveView.ScreenDisplay.DrawPolyline(pGeometry);
                    TimeSpan pPolylineTimeSpan = new TimeSpan(150);
                    pActiveView.ScreenDisplay.DrawPolyline(pGeometry);
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbolClass();
                    pSimpleFillSymbol.Outline = null;
                    pSimpleFillSymbol.Color = pRgbColor;
                    pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                    pSymbol = pSimpleFillSymbol as ISymbol;
                    pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
                    pActiveView.ScreenDisplay.SetSymbol(pSymbol);
                    pActiveView.ScreenDisplay.DrawPolygon(pGeometry);
                    TimeSpan pPolygonTimeSpan = new TimeSpan(150);
                    pActiveView.ScreenDisplay.DrawPolygon(pGeometry);
                    break;
            }
            pActiveView.ScreenDisplay.FinishDrawing();
        }

        // 根据几何体画出其对应的范围
        public static bool DoDrawRange(IMapControlDefault pMapcontrol, IGeometry pgeometry, int intRed, int intGreen, int intBlue, bool bDel)
        {
            // 创建该要素的Element
            IGraphicsContainer pMapGraphics = (IGraphicsContainer)pMapcontrol.Map;

            IElement pElement = null;
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.Red = intRed;
            pRGBColor.Green = intGreen;
            pRGBColor.Blue = intBlue;
            switch (pgeometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPolygon:
                    IPolygonElement pPolElemnt = new PolygonElementClass();
                    IFillShapeElement pFillShapeElement = (IFillShapeElement)pPolElemnt;
                    pFillShapeElement.Symbol = GetDrawSymbol(intRed, intGreen, intBlue);

                    pElement = pFillShapeElement as IElement;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    ILineElement pLineElement = new LineElementClass();
                    ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
                    pLineSymbol.Color = pRGBColor;
                    pLineElement.Symbol = pLineSymbol;

                    pElement = pLineElement as IElement;
                    break;
                case esriGeometryType.esriGeometryPoint:
                    IMarkerElement pMarkerElemnt = new MarkerElementClass();
                    ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass();
                    pMarkerSymbol.Color = pRGBColor;
                    pMarkerSymbol.Size = 2;
                    pMarkerElemnt.Symbol = pMarkerSymbol;

                    pElement = pMarkerElemnt as IElement;
                    break;
                default:
                    return false;
            }

            // 画出Element
            pElement.Geometry = pgeometry;

            if (bDel == true)
            {
                pMapGraphics.DeleteAllElements();
            }

            // 将新画的Element添加到图形界面
            pMapGraphics.AddElement(pElement, 0);
            pMapcontrol.Refresh(esriViewDrawPhase.esriViewForeground, null, null);

            // 刷新当前界面
            IActiveView pActiveView = (IActiveView)pMapcontrol.Map;
            pActiveView.Refresh();
            return true;
        }

        // 根据几何体画出其对应的范围
        public static IElement DoDrawGeometry(IMapControlDefault pMapcontrol, IGeometry pgeometry, int intRed, int intGreen, int intBlue, bool bDel)
        {
            // 创建该要素的Element
            IGraphicsContainer pMapGraphics = (IGraphicsContainer)pMapcontrol.Map;

            IElement pElement = null;
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.Red = intRed;
            pRGBColor.Green = intGreen;
            pRGBColor.Blue = intBlue;
            switch (pgeometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPolygon:
                    IPolygonElement pPolElemnt = new PolygonElementClass();
                    IFillShapeElement pFillShapeElement = (IFillShapeElement)pPolElemnt;
                    pFillShapeElement.Symbol = GetDrawSymbol(intRed, intGreen, intBlue);

                    pElement = pFillShapeElement as IElement;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    ILineElement pLineElement = new LineElementClass();
                    ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
                    pLineSymbol.Color = pRGBColor;
                    pLineElement.Symbol = pLineSymbol;

                    pElement = pLineElement as IElement;
                    break;
                case esriGeometryType.esriGeometryPoint:
                    IMarkerElement pMarkerElemnt = new MarkerElementClass();
                    ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass();
                    pMarkerSymbol.Color = pRGBColor;
                    pMarkerSymbol.Size = 2;
                    pMarkerElemnt.Symbol = pMarkerSymbol;

                    pElement = pMarkerElemnt as IElement;
                    break;
                default:
                    return null;
            }

            // 画出Element
            pElement.Geometry = pgeometry;

            //if (bDel == true)
            //{
            //    pMapGraphics.DeleteAllElements();
            //}
            // 将新画的Element添加到图形界面
            pMapGraphics.AddElement(pElement, 0);
            pMapcontrol.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);

            // 刷新当前界面
            IActiveView pActiveView = (IActiveView)pMapcontrol.Map;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
            return pElement;
        }

        // 得到画范围的填充符合
        public static ISimpleFillSymbol GetDrawSymbol(int intRed, int intGreen, int intBlue)
        {
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.UseWindowsDithering = false;

            ISymbol pSymbol = (ISymbol)pFillSymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            pRGBColor.Red = intRed;
            pRGBColor.Green = intGreen;
            pRGBColor.Blue = intBlue;
            pLineSymbol.Color = pRGBColor;

            pLineSymbol.Width = 0.8;
            pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            pFillSymbol.Outline = pLineSymbol;

            pFillSymbol.Color = pRGBColor;
            pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;

            return pFillSymbol;
        }

        #region 实现将同一个数据库中不同的表或不同数据库中的表Join起来
        /// <summary>
        /// 获取关系类
        /// </summary>
        /// <param name="Name">标示名称</param>
        /// <param name="originPrimaryClass">类1</param>
        /// <param name="originPrimaryKeyField">类1主键字段</param>
        /// <param name="originForeignClass">类2</param>
        /// <param name="originForeignKeyField">类2外键字段</param>
        /// <param name="ForwardPathLabel">标注字段1</param>
        /// <param name="BackwardPathLabel">标注字段2</param>
        /// <param name="Cardinality">关联对应关系(OneToOne,OneToMany,ManyToMany)</param>
        /// <param name="eError">错误描述</param>
        /// <returns></returns>
        public static IRelationshipClass GetRelationShipClass(string Name, IObjectClass originPrimaryClass, string originPrimaryKeyField, IObjectClass originForeignClass, string originForeignKeyField, string ForwardPathLabel, string BackwardPathLabel, esriRelCardinality Cardinality, out Exception eError)
        {
            eError = null;
            try
            {
                IMemoryRelationshipClassFactory memoryRelateFactory = new MemoryRelationshipClassFactoryClass();
                IRelationshipClass relationShipClass = memoryRelateFactory.Open(Name, originPrimaryClass, originPrimaryKeyField, originForeignClass, originForeignKeyField, ForwardPathLabel, BackwardPathLabel, Cardinality);
                return relationShipClass;
            }
            catch (Exception ex)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(ex);
                //********************************
                eError = ex;
                return null;
            }
        }

        /// <summary>
        /// 建立关系表
        /// </summary>
        /// <param name="relClass">关系类</param>
        /// <param name="joinForward">?</param>
        /// <param name="queryFilter">过滤条件</param>
        /// <param name="target_Columns">保留字段组合字符串如"a,b"</param>
        /// <param name="DoNotPushJoinToDB">?</param>
        /// <param name="openAsLeftOuterJoin">数据记录个数以哪个为准</param>
        /// <param name="eError">错误描述</param>
        /// <returns></returns>
        public static IRelQueryTable GetRelQueryTable(IRelationshipClass relClass, bool joinForward, IQueryFilter queryFilter, string target_Columns, bool DoNotPushJoinToDB, bool openAsLeftOuterJoin, out Exception eError)
        {
            eError = null;
            try
            {
                IRelQueryTableFactory relQueryTableFactory = new RelQueryTableFactoryClass();
                IRelQueryTable relQueryTable = relQueryTableFactory.Open(relClass, joinForward, queryFilter, null, target_Columns, DoNotPushJoinToDB, openAsLeftOuterJoin);
                return relQueryTable;
            }
            catch (Exception ex)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(ex);
                //********************************
                eError = ex;
                return null;
            }
        }
        #endregion

        #region 图层排序
        /// <summary>
        /// 对mapcontrol上的图层进行排序
        /// </summary>
        /// <param name="vAxMapControl"></param>
        public static void LayersCompose(IMapControlDefault pMapcontrol)
        {
            IMap pMap = pMapcontrol.Map;
            int[] iLayerIndex = new int[2] { 0, 0 };
            int[] iFeatureLayerIndex = new int[3] { 0, 0, 0 };

            int iCount = pMapcontrol.LayerCount;
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = pMap.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = pMap.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }

                if (pFeatureLayer != null)
                {
                    switch (pFeatureLayer.FeatureClass.FeatureType)
                    {
                        case esriFeatureType.esriFTDimension:
                            pMap.MoveLayer(pFeatureLayer, iLayerIndex[0]);
                            iLayerIndex[0] = iLayerIndex[0] + 1;
                            break;
                        case esriFeatureType.esriFTAnnotation:

                            pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1]);
                            iLayerIndex[1] = iLayerIndex[1] + 1;
                            break;
                        case esriFeatureType.esriFTSimple:

                            switch (pFeatureLayer.FeatureClass.ShapeType)
                            {
                                case esriGeometryType.esriGeometryPoint:
                                    pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1] + iFeatureLayerIndex[0]);
                                    iFeatureLayerIndex[0] = iFeatureLayerIndex[0] + 1;
                                    break;
                                case esriGeometryType.esriGeometryLine:
                                case esriGeometryType.esriGeometryPolyline:
                                    pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1] + iFeatureLayerIndex[0] + iFeatureLayerIndex[1]);
                                    iFeatureLayerIndex[1] = iFeatureLayerIndex[1] + 1;
                                    break;
                                case esriGeometryType.esriGeometryPolygon:
                                    pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1] + iFeatureLayerIndex[0] + iFeatureLayerIndex[1] + iFeatureLayerIndex[2]);
                                    iFeatureLayerIndex[2] = iFeatureLayerIndex[2] + 1;
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 对mapcontrol上groupLayer内的图层进行排序
        /// </summary>
        /// <param name="groupLayer"></param>
        public static void LayersCompose(IGroupLayer groupLayer)
        {
            ICompositeLayer comLayer = groupLayer as ICompositeLayer;
            int iCount = comLayer.Count;

            List<ILayer> listLays = new List<ILayer>();
            //对Dimension层进行排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = comLayer.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = comLayer.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }

                if (pFeatureLayer == null) break;
                if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTDimension)
                {
                    listLays.Add(pFeatureLayer as ILayer);
                }
            }
            foreach (ILayer pTempLay in listLays)
            {
                groupLayer.Delete(pTempLay);
                groupLayer.Add(pTempLay);
            }

            listLays = new List<ILayer>();
            //对Annotation层进行排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = comLayer.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = comLayer.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }
                if (pFeatureLayer == null) break;
                if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    listLays.Add(pFeatureLayer as ILayer);
                }
            }
            foreach (ILayer pTempLay in listLays)
            {
                groupLayer.Delete(pTempLay);
                groupLayer.Add(pTempLay);
            }

            listLays = new List<ILayer>();
            //对点层进行排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = comLayer.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = comLayer.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }
                if (pFeatureLayer == null) break;
                if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTSimple)
                {
                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        listLays.Add(pFeatureLayer as ILayer);
                    }
                }
            }
            foreach (ILayer pTempLay in listLays)
            {
                groupLayer.Delete(pTempLay);
                groupLayer.Add(pTempLay);
            }

            listLays = new List<ILayer>();
            //对线层进行排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = comLayer.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = comLayer.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }
                if (pFeatureLayer == null) break;
                if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTSimple)
                {
                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryLine || pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        listLays.Add(pFeatureLayer as ILayer);
                    }
                }
            }
            foreach (ILayer pTempLay in listLays)
            {
                groupLayer.Delete(pTempLay);
                groupLayer.Add(pTempLay);
            }

            listLays = new List<ILayer>();
            //对面层排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = comLayer.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = comLayer.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }
                if (pFeatureLayer == null) break;
                if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTSimple)
                {
                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        listLays.Add(pFeatureLayer as ILayer);
                    }
                }
            }
            foreach (ILayer pTempLay in listLays)
            {
                groupLayer.Delete(pTempLay);
                groupLayer.Add(pTempLay);
            }

            listLays = null;
        }
        #endregion

        #region Table相关操作实现
        /// <summary>
        /// 新建行
        /// </summary>
        /// <param name="pTable">表名</param>
        /// <param name="dicvalues">字段,值对应集合</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static bool NewRow(ITable pTable, Dictionary<string, object> dicvalues, out Exception eError)
        {
            eError = null;

            ICursor pCursor = pTable.Insert(false);
            IRowBuffer pRowBuffer = pTable.CreateRowBuffer();
            foreach (KeyValuePair<string, object> keyValue in dicvalues)
            {
                int index = pRowBuffer.Fields.FindField(keyValue.Key);
                if (index == -1)
                {
                    eError = new Exception("字段" + keyValue.Key + "不存在");
                    return false;
                }

                try
                {
                    if (pRowBuffer.Fields.get_Field(index).Editable)
                    {
                        pRowBuffer.set_Value(index, keyValue.Value);
                    }
                }
                catch (Exception eX)
                {
                    eError = new Exception("字段" + keyValue.Key + "类型与值不匹配");
                    //********************************
                    //guozheng added  system exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eX);
                    SysCommon.Log.Module.SysLog.Write(eError);
                    //********************************
                    return false;
                }
            }

            try
            {
                pCursor.InsertRow(pRowBuffer);
            }
            catch (Exception eR)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eR);
                //********************************
                eError = eR;
                return false;
            }

            return true;
        }
        /// <summary>
        /// 新建行
        /// </summary>
        /// <param name="pTable">表名</param>
        /// <param name="dicvalues">字段,值对应集合</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static bool NewRowByAliasName(ITable pTable, Dictionary<string, object> dicvalues, out Exception eError)
        {
            eError = null;

            ICursor pCursor = pTable.Insert(false);
            IRowBuffer pRowBuffer = pTable.CreateRowBuffer();
            foreach (KeyValuePair<string, object> keyValue in dicvalues)
            {
                int index = pRowBuffer.Fields.FindFieldByAliasName(keyValue.Key);
                if (index == -1)
                {
                    eError = new Exception("字段" + keyValue.Key + "不存在");
                    return false;
                }

                try
                {
                    if (pRowBuffer.Fields.get_Field(index).Editable)
                    {
                        pRowBuffer.set_Value(index, keyValue.Value);
                    }
                }
                catch (Exception eX)
                {
                    eError = new Exception("字段" + keyValue.Key + "类型与值不匹配");
                    //********************************
                    //guozheng added  system exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eX);
                    SysCommon.Log.Module.SysLog.Write(eError);
                    //********************************
                    return false;
                }
            }

            try
            {
                pCursor.InsertRow(pRowBuffer);
            }
            catch (Exception eR)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eR);
                //********************************
                eError = eR;
                return false;
            }

            return true;
        }
        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="pTable">表名</param>
        /// <param name="strDelCon">删除条件</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static bool DelRow(ITable pTable, string strDelCon, out Exception eError)
        {
            eError = null;
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = strDelCon;
            try
            {
                pTable.DeleteSearchedRows(pQueryFilter);
            }
            catch (Exception eR)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eR);
                //********************************
                eError = eR;
                return false;
            }

            return true;
        }
        /// <summary>
        /// 修改行
        /// </summary>
        /// <param name="pTable">表名</param>
        /// <param name="strDelCon">修改条件</param>
        /// <param name="dicvalues">字段,值对应集合</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static bool UpdateRowByAliasName(ITable pTable, string strDelCon, Dictionary<string, object> dicvalues, out Exception eError)
        {
            eError = null;
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = strDelCon;

            ICursor pCursor = null;
            try
            {
                pCursor = pTable.Update(pQueryFilter, false);
            }
            catch (Exception eR)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eR);
                //********************************
                eError = new Exception("过滤查询条件设置错误");
                return false;
            }

            IRow pRow = pCursor.NextRow();
            if (pRow == null) return false;

            foreach (KeyValuePair<string, object> keyValue in dicvalues)
            {
                int index = pRow.Fields.FindFieldByAliasName(keyValue.Key);//changed by xisheng 06.17
                if (index == -1)
                {
                    eError = new Exception("字段" + keyValue.Key + "不存在");
                    return false;
                }

                try
                {
                    if (keyValue.Value != pRow.get_Value(index))//如果不等才赋值
                        pRow.set_Value(index, keyValue.Value);
                }
                catch (Exception eR)
                {
                    eError = new Exception("字段" + keyValue.Key + "类型与值不匹配");
                    //********************************
                    //guozheng added  system exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eR);
                    SysCommon.Log.Module.SysLog.Write(eError);
                    //********************************
                    return false;
                }
            }

            try
            {
                pCursor.UpdateRow(pRow);
            }
            catch (Exception eR)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eR);
                //********************************
                eError = eR;
                return false;
            }

            return true;
        }
        /// <summary>
        /// 修改行
        /// </summary>
        /// <param name="pTable">表名</param>
        /// <param name="strDelCon">修改条件</param>
        /// <param name="dicvalues">字段,值对应集合</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static bool UpdateRow(ITable pTable, string strDelCon, Dictionary<string, object> dicvalues, out Exception eError)
        {
            eError = null;
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = strDelCon;

            ICursor pCursor = null;
            try
            {
                pCursor = pTable.Update(pQueryFilter, false);
            }
            catch (Exception eR)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eR);
                //********************************
                eError = new Exception("过滤查询条件设置错误");
                return false;
            }

            IRow pRow = pCursor.NextRow();
            if (pRow == null) return false;

            foreach (KeyValuePair<string, object> keyValue in dicvalues)
            {
                int index = pRow.Fields.FindField(keyValue.Key);//changed by xisheng 06.17
                if (index == -1)
                {
                    eError = new Exception("字段" + keyValue.Key + "不存在");
                    return false;
                }

                try
                {
                    if(keyValue.Value!=pRow.get_Value(index))//如果不等才赋值
                        pRow.set_Value(index, keyValue.Value);
                }
                catch (Exception eR)
                {
                    eError = new Exception("字段" + keyValue.Key + "类型与值不匹配");
                    //********************************
                    //guozheng added  system exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eR);
                    SysCommon.Log.Module.SysLog.Write(eError);
                    //********************************
                    return false;
                }
            }

            try
            {
                pCursor.UpdateRow(pRow);
            }
            catch (Exception eR)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eR);
                //********************************
                eError = eR;
                return false;
            }

            return true;
        }
        #endregion

        #region Featureclass相关操作实现
        /// <summary>
        /// 删除指定要素
        /// </summary>
        /// <param name="pFeatCls">要素类</param>
        /// <param name="strCon">指定条件</param>
        /// <param name="eError"></param>
        public static void DelFeature(IFeatureClass pFeatCls, string strCon, out Exception eError)
        {
            eError = null;
            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = strCon;
                IFeatureCursor pFcursor = pFeatCls.Update(pQueryFilter, false);
                IFeature pFeatTemp = pFcursor.NextFeature();
                if (pFeatTemp != null)
                {
                    pFeatTemp.Delete();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFcursor);
            }
            catch (Exception exError)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(exError);
                //********************************
                eError = exError;
            }
        }
        #endregion

        //根据MXD文档为图层配置符号
        public static void RenderLayerByMxd(String MxdPath, IMapControlDefault pMapcontrol, out Exception errOut)
        {
            errOut = null;

            try
            {
                //加载MXD文档
                IMapDocument pMapDocument = new MapDocumentClass();
                pMapDocument.Open(MxdPath, "");
                IMap pMap = pMapDocument.get_Map(0);
                if (pMap == null) return;

                //符号方案字典
                Dictionary<string, IFeatureRenderer> dicValue = new Dictionary<string, IFeatureRenderer>();
                //最大最小可见比例尺字典
                Dictionary<string, ILayer> dicScaleRange = new Dictionary<string, ILayer>();
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    ILayer pLayer = pMap.get_Layer(i);
                    IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;

                    if (pGeoFeatureLayer == null) continue;

                    string name = string.Empty;
                    IDataset pDataset = (IDataset)pGeoFeatureLayer.FeatureClass;
                    if (pDataset != null)
                    {
                        name = pDataset.Name;
                    }
                    else
                    {
                        name = pLayer.Name;
                    }
                    if (name.Contains(".")) name = name.Substring(name.LastIndexOf('.') + 1);

                    if (!dicScaleRange.ContainsKey(name))
                    {
                        dicScaleRange.Add(name, pLayer);
                    }


                    IFeatureRenderer pFeatureRender = pGeoFeatureLayer.Renderer;
                    if (!dicValue.ContainsKey(name))
                    {
                        dicValue.Add(name, pFeatureRender);
                    }
                }

                if (dicValue.Count == 0) return;

                //设置参考比例尺
                pMapcontrol.ReferenceScale = pMap.ReferenceScale;

                //设置符号和现实比例尺
                for (int i = 0; i < pMapcontrol.LayerCount; i++)
                {
                    ILayer pLayer = pMapcontrol.get_Layer(i);
                    if (pLayer is IGroupLayer)
                    {
                        ScaleVisibleGroupLayer(pLayer as IGroupLayer, dicScaleRange);
                        RenderGroupLayer(pLayer as IGroupLayer, dicValue);
                    }
                    else if (pLayer is IGeoFeatureLayer)
                    {
                        string name = ((pLayer as IFeatureLayer).FeatureClass as IDataset).Name;
                        if (name.Contains(".")) name = name.Substring(name.LastIndexOf('.') + 1);
                        if (dicValue.ContainsKey(name))
                        {
                            (pLayer as IGeoFeatureLayer).Renderer = dicValue[name];

                            pLayer.MaximumScale = dicScaleRange[name].MaximumScale;
                            pLayer.MinimumScale = dicScaleRange[name].MinimumScale;

                        }
                    }
                }

            }
            catch (Exception err)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(err);
                //********************************
                errOut = err;
            }
        }

        private static void ScaleVisibleGroupLayer(IGroupLayer pLayer, Dictionary<string, ILayer> dicScaleRange)
        {
            ICompositeLayer pCompositeLayer = pLayer as ICompositeLayer;
            for (int i = 0; i < pCompositeLayer.Count; i++)
            {
                ILayer mLayer = pCompositeLayer.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    ScaleVisibleGroupLayer(mLayer as IGroupLayer, dicScaleRange);
                }
                else if (mLayer is IGeoFeatureLayer)
                {
                    string name = ((mLayer as IFeatureLayer).FeatureClass as IDataset).Name;
                    if (name.Contains(".")) name = name.Substring(name.LastIndexOf('.') + 1);
                    if (dicScaleRange.ContainsKey(name))
                    {
                        mLayer.MaximumScale = dicScaleRange[name].MaximumScale;
                        mLayer.MinimumScale = dicScaleRange[name].MinimumScale;

                    }
                }
            }
        }

        private static void RenderGroupLayer(IGroupLayer pLayer, Dictionary<string, IFeatureRenderer> dicValue)
        {
            ICompositeLayer pCompositeLayer = pLayer as ICompositeLayer;
            for (int i = 0; i < pCompositeLayer.Count; i++)
            {
                ILayer mLayer = pCompositeLayer.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    RenderGroupLayer(mLayer as IGroupLayer, dicValue);
                }
                else if (mLayer is IGeoFeatureLayer)
                {
                    string name = ((mLayer as IFeatureLayer).FeatureClass as IDataset).Name;
                    if (name.Contains(".")) name = name.Substring(name.LastIndexOf('.') + 1);
                    if (dicValue.ContainsKey(name))
                    {
                        (mLayer as IGeoFeatureLayer).Renderer = dicValue[name];
                    }
                }
            }
        }

        //chenxinwei Add 20110730
        /// <summary>
        /// 国家测绘局 根据x获得投影文件
        /// </summary>
        /// <param name="dblX"></param>
        /// <returns></returns>
        public static ISpatialReference GetSpatialByX(double dblX)
        {
            string strPrjFileName = "";
            if (dblX < 112.5)
            {
                strPrjFileName = "Xian 1980 3 Degree GK CM 111E.prj";
            }
            else if (dblX < 115.5)
            {
                strPrjFileName = "Xian 1980 3 Degree GK CM 114E.prj";
            }
            else if (dblX < 118.5)
            {
                strPrjFileName = "Xian 1980 3 Degree GK CM 117E.prj";
            }
            else
            {
                strPrjFileName = "WGS 1984 PDC Mercator.prj";
            }
            

            //
            strPrjFileName = Application.StartupPath + "\\..\\Prj\\" + strPrjFileName;
            if (!System.IO.File.Exists(strPrjFileName)) return null;

            ISpatialReferenceFactory pSpaFac = new SpatialReferenceEnvironmentClass();
            return pSpaFac.CreateESRISpatialReferenceFromPRJFile(strPrjFileName);

        }

        /// <summary>
        /// 获得当前map中的所有图层
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="lstLyrs"></param>
        public static void GetLayersByMap(ESRI.ArcGIS.Carto.IMap pMap, ref List<ILayer> lstLyrs)
        {
            lstLyrs = new List<ILayer>();
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                GetLyrsByLyr(pMap.get_Layer(i), ref lstLyrs);
            }
        }

        public static void GetLyrsByLyr(ILayer pLyr, ref List<ILayer> lstLyrs)
        {
            if (pLyr != null)
            {
                if (pLyr is IGroupLayer)
                {
                    ICompositeLayer pComLyr = pLyr as ICompositeLayer;
                    for (int i = 0; i < pComLyr.Count; i++)
                    {
                        GetLyrsByLyr(pComLyr.get_Layer(i), ref lstLyrs);
                    }
                }
                else
                {
                    lstLyrs.Add(pLyr);
                }
            }
        }
        /// <summary>
        /// ZQ 2011 1203 获取feature的个数
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <param name="WhereClause"></param>
        /// <returns></returns>
        public static int  GetFeatureCount(IFeatureClass pFeatureClass,string WhereClause)
        {
            int iFeatureCount = 0;
            try
            {
                IDataset pDataset = pFeatureClass as IDataset;
                IFeatureWorkspace pFeatureWorkspace = pDataset.Workspace as IFeatureWorkspace;
                IQueryDef pQueryDef = pFeatureWorkspace.CreateQueryDef();
                pQueryDef.Tables = pDataset.Name.ToString();
                pQueryDef.SubFields = "count(*)";
                pQueryDef.WhereClause = WhereClause;
                ICursor pCursor = pQueryDef.Evaluate();
                IRow pRow = pCursor.NextRow();
                iFeatureCount =Convert.ToInt32(pRow.get_Value(0).ToString());
                return iFeatureCount;
            }
            catch { return iFeatureCount =0; }
        }
        /// <summary>
        /// 将ITable转换为DataTable
        /// </summary>
        /// <param name="pTable"></param>
        /// <param name="sTableName"></param>
        /// <returns></returns>
        public static DataTable ITableToDataTable(ITable pTable, string sTableName)
        {
            DataTable pDataTable = new DataTable(sTableName);
            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                ICursor pCursor = pTable.Search(pQueryFilter, false);
                IRow pRow = pCursor.NextRow();
                if (pRow != null)
                {
                    for (int i = 0; i < pRow.Fields.FieldCount; i++)
                    {
                        pDataTable.Columns.Add(pRow.Fields.get_Field(i).Name);
                    }
                    while (pRow != null)
                    {
                        DataRow pDataRow = pDataTable.NewRow();
                        for (int j = 0; j < pCursor.Fields.FieldCount; j++)
                            pDataRow[j] = pRow.get_Value(j);
                        pDataTable.Rows.Add(pDataRow);
                        pRow = pCursor.NextRow();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return pDataTable;
        }



        /// <summary>
        /// IFDOToADOConnection获取属性表
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <returns></returns>
        public static DataTable GetFeaturClassTable(IFeatureClass pFeatureClass)
        {
            IDataset pDataset = pFeatureClass as IDataset;
            IFeatureWorkspace pFeawks = pDataset.Workspace as IFeatureWorkspace;
            ITable pTable = null;
            try
            {
                pTable = pFeawks.OpenTable(pDataset.Name);
            }
            catch
            { }
            DataTable dtTerritories = null;
            try
            {
                dtTerritories = ITableToDataTable(pTable, pDataset.Name);
            }
            catch
            { }
            return dtTerritories;
            //IDataset pDataset = pFeatureClass as IDataset;
            //IFDOToADOConnection pFDOToADOConnection = new FdoAdoConnectionClass();
            
            //ADODB.Connection pADODBConnection = new ADODB.Connection();
            //ADODB.Recordset pADODBRecordSet = new ADODB.Recordset();
            //DataTable dtTerritories = new DataTable();
            //try
            //{
            //    string strSQL = GetFiledSQL(pFeatureClass);
            //    pFDOToADOConnection.Connect(pDataset.Workspace, pADODBConnection);                
            //    pADODBRecordSet.Open("Select " + strSQL + " from " + pDataset.Name, pADODBConnection, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, 0);
            //    OleDbDataAdapter custDA = new OleDbDataAdapter();
            //    custDA.Fill(dtTerritories, pADODBRecordSet);
            //    return dtTerritories;
            //}
            //catch(Exception err)
            //{
            //    return dtTerritories = null;
            //}
            //finally
            //{

            //    pADODBConnection.Close();
            //    pADODBConnection = null;
            //    pADODBRecordSet = null;
            //}

        }
        public static string GetFiledSQL(IFeatureClass pFeatureClass)
        {
            IFields pFields = pFeatureClass.Fields;
            string strSQL="";
            for (int i = 0; i < pFields.FieldCount;i++ )
            {
                string strField = pFields.get_Field(i).Name.ToString();
                if (strField == "SHAPE" || strField == "Shape" || pFields.get_Field(i).Type == esriFieldType.esriFieldTypeBlob)
                {
                    continue;
                }
                //if (!strField.Contains("SHAPE.") || !strField.Contains("Shape_"))
                //{
                //    strSQL = strSQL + strField + ",";
                //}
                if (i == pFields.FieldCount - 1)
                {
                    strSQL = strSQL + strField;
                }
                else { strSQL = strSQL + strField + ","; }
            }
            return strSQL;
        }
        public enum eumDataType
        {
            Oracle =0,
            PDB = 1,
            GDB= 2,
            Shp =3,
        }
        //获取数据库的数据类型（ORACLE MDB GDB）
        public static eumDataType GetDescriptionOfWorkspace(IWorkspace pWorkspace)
        {
            
            if (pWorkspace == null)
            {
                return eumDataType.PDB;
            }
            IWorkspaceFactory pWorkSpaceFac = pWorkspace.WorkspaceFactory;
            if (pWorkSpaceFac == null)
            {
                return eumDataType.PDB;
            }
            string strDescrip = pWorkSpaceFac.get_WorkspaceDescription(false);
            switch (strDescrip)
            {
                case "Personal Geodatabase"://mdb数据库 使用*作匹配符
                    return eumDataType.PDB;
                    break;
                case "File Geodatabase"://gdb数据库 使用%作匹配符
                    return eumDataType.GDB;
                    break;
                case "Spatial Database Connection"://sde(oracle数据库) 使用%作匹配符(sql server数据库，现在暂未考虑)
                    return eumDataType.Oracle;
                    break;
                case"Shapefiles":
                    return eumDataType.Shp;
                    break;
                default:
                    return eumDataType.PDB;
                    break;
            }
           return eumDataType.PDB;
        }

    }

}
