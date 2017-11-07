using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using SysCommon.Gis;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Windows.Forms;

namespace GeoUtilities
{
    /// <summary>
    /// 作者：yjl整理收集
    /// 日期：20110825
    /// 说明：复制要素到新要素类
    /// 来源:数据提取窗体
    /// </summary>
    public static class TransFeatures
    {
        private static IEnumFieldError pEnumFieldError = null;//字段检查错误集，为了给出错的字段赋值yjl20110804 add
        private static IFields pFixedField = null;//字段检查类修正后的字段集，根据错误集寻找修正后的字段名
        //创建featureclass
        public static IFeatureClass CreateFeatureClass(string name, IFeatureClass srcFC, IWorkspace pToWorkspace, UID uidCLSID, UID uidCLSEXT, esriGeometryType GeometryType)
        {
            try
            {
                if (uidCLSID == null)
                {
                    uidCLSID = new UIDClass();
                    switch (srcFC.FeatureType)
                    {
                        case (esriFeatureType.esriFTSimple):
                            uidCLSID.Value = "{52353152-891A-11D0-BEC6-00805F7C4268}";
                            break;
                        case (esriFeatureType.esriFTSimpleJunction):
                            GeometryType = esriGeometryType.esriGeometryPoint;
                            uidCLSID.Value = "{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexJunction):
                            uidCLSID.Value = "{DF9D71F4-DA32-11D1-AEBA-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTSimpleEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{E7031C90-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTAnnotation):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{E3676993-C682-11D2-8A2A-006097AFF44E}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{496764FC-E0C9-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                // 设置 uidCLSEXT (if Null)
                if (uidCLSEXT == null)
                {
                    switch (srcFC.FeatureType)
                    {
                        case (esriFeatureType.esriFTAnnotation):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{24429589-D711-11D2-9F41-00C04F6BC6A5}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{48F935E2-DA66-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pToWorkspace;
                IFieldChecker fdCheker = new FieldCheckerClass();//yjl20110804 add
                pEnumFieldError = null;
                pFixedField = null;
                fdCheker.ValidateWorkspace = pToWorkspace;
                fdCheker.Validate(srcFC.Fields, out pEnumFieldError, out pFixedField);

                string strShapeFieldName = srcFC.ShapeFieldName;//geometry字段名
                string[] strShapeNames = strShapeFieldName.Split('.');
                strShapeFieldName = strShapeNames[strShapeNames.GetLength(0) - 1];


                IFeatureClass targetFeatureclass = pFeatureWorkspace.CreateFeatureClass("" + name + "", pFixedField, uidCLSID, uidCLSEXT, srcFC.FeatureType, strShapeFieldName, "");

                return targetFeatureclass;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Cannot create a low precision dataset in a high precision database.")
                {
                    MessageBox.Show("数据必须是ArcGis9.3的数据，请将数据处理成ArcGis9.2的数据！");
                }
            }
            IFeatureClass featureclass = null;
            return featureclass;
        }

        //复制要素
        public static void CopyFeatureAndTran(IFeatureCursor pCursor, IFeatureClass pToFeatureClass, ITransformation inTransformation)
        {
            IFeature pFeature = pCursor.NextFeature();
            IFeatureCursor pFeatureCursor = pToFeatureClass.Insert(true);
            int iCount = 0;
            while (pFeature != null)
            {
                IFeatureBuffer pFeatureBuffer = pToFeatureClass.CreateFeatureBuffer();

                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    string sFieldName = pFeature.Fields.get_Field(i).Name;

                    int iIndex = pFeatureBuffer.Fields.FindField(sFieldName);

                    if ((iIndex > -1) && (pFeatureBuffer.Fields.get_Field(iIndex).Editable == true))
                    {
                        pFeatureBuffer.set_Value(iIndex, pFeature.get_Value(i));
                    }
                }
                if (pEnumFieldError != null)//yjl20110804 add
                {
                    pEnumFieldError.Reset();
                    IFieldError pFieldError = pEnumFieldError.Next();
                    while (pFieldError != null)
                    {
                        int srcIx = pFieldError.FieldIndex;//错误字段的源索引
                        int desIx = pFeatureBuffer.Fields.FindField(pFixedField.get_Field(srcIx).Name);//错误字段的新索引
                        if (desIx == -1)
                        {
                            pFieldError = pEnumFieldError.Next();
                            continue;
                        }
                        IField pFld = pFeatureBuffer.Fields.get_Field(desIx);
                        if ((desIx > -1) && (pFld.Editable == true) && pFld.Type != esriFieldType.esriFieldTypeGeometry)
                        {
                            pFeatureBuffer.set_Value(desIx, pFeature.get_Value(srcIx));
                        }
                        pFieldError = pEnumFieldError.Next();
                    }
                }
                IGeometry shpTransformed = pFeature.ShapeCopy;
                ITransform2D pTransform2D = shpTransformed as ITransform2D;
                pTransform2D.Transform(esriTransformDirection.esriTransformForward, inTransformation);
                pFeatureBuffer.Shape = shpTransformed;
                pFeatureCursor.InsertFeature(pFeatureBuffer);
                if (iCount == 500)
                {
                    pFeatureCursor.Flush();
                    iCount = 0;
                }
                iCount++;
                pFeature = pCursor.NextFeature();
            }
            if (iCount > 0) pFeatureCursor.Flush();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
        }
        //复制GDB数据
        public static void CopyPasteGeodatabaseData(IWorkspace sourceWorkspace, IWorkspace targetWorkspace, string objectName, esriDatasetType esriDataType)
        {
            // Validate input

            if ((sourceWorkspace.Type == esriWorkspaceType.esriFileSystemWorkspace) || (targetWorkspace.Type == esriWorkspaceType.esriFileSystemWorkspace))
            {
                return; // Should be a throw
            }
            //create source workspace name

            IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;

            IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;

            //create target workspace name

            IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;

            IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;

            //Create Name Object Based on data type

            IDatasetName datasetName;

            switch (esriDataType)
            {
                case esriDatasetType.esriDTFeatureDataset:

                    IFeatureDatasetName inFeatureDatasetName = new FeatureDatasetNameClass();

                    datasetName = (IDatasetName)inFeatureDatasetName;
                    break;
                case esriDatasetType.esriDTFeatureClass:

                    IFeatureClassName inFeatureClassName = new FeatureClassNameClass();

                    datasetName = (IDatasetName)inFeatureClassName;

                    break;
                case esriDatasetType.esriDTTable:

                    ITableName inTableName = new TableNameClass();

                    datasetName = (IDatasetName)inTableName;

                    break;

                case esriDatasetType.esriDTGeometricNetwork:

                    IGeometricNetworkName inGeometricNetworkName = new GeometricNetworkNameClass();

                    datasetName = (IDatasetName)inGeometricNetworkName;

                    break;

                case esriDatasetType.esriDTRelationshipClass:

                    IRelationshipClassName inRelationshipClassName = new RelationshipClassNameClass();

                    datasetName = (IDatasetName)inRelationshipClassName;
                    break;

                case esriDatasetType.esriDTNetworkDataset:

                    INetworkDatasetName inNetworkDatasetName = new NetworkDatasetNameClass();

                    datasetName = (IDatasetName)inNetworkDatasetName;

                    break;

                case esriDatasetType.esriDTTopology:

                    ITopologyName inTopologyName = new TopologyNameClass();

                    datasetName = (IDatasetName)inTopologyName;

                    break;

                default:

                    return; // Should be a throw
            }

            // Set the name of the object to be copied

            datasetName.WorkspaceName = (IWorkspaceName)sourceWorkspaceName;

            datasetName.Name = objectName;

            //Setup mapping for copy/paste

            IEnumNameMapping fromNameMapping;

            ESRI.ArcGIS.esriSystem.IEnumNameEdit editFromName;

            ESRI.ArcGIS.esriSystem.IEnumName fromName = new NamesEnumerator();

            editFromName = (ESRI.ArcGIS.esriSystem.IEnumNameEdit)fromName;

            editFromName.Add((ESRI.ArcGIS.esriSystem.IName)datasetName);

            ESRI.ArcGIS.esriSystem.IName toName = (ESRI.ArcGIS.esriSystem.IName)targetWorkspaceName;

            // Generate name mapping


            ESRI.ArcGIS.Geodatabase.IGeoDBDataTransfer geoDBDataTransfer = new GeoDBDataTransferClass();

            bool targetWorkspaceExists;

            targetWorkspaceExists = geoDBDataTransfer.GenerateNameMapping(fromName, toName, out fromNameMapping);

            // Copy/Pate the data

            geoDBDataTransfer.Transfer(fromNameMapping, toName);


        }
        
    }
    
}
