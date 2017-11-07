using ESRI.ArcGIS.Geodatabase;
using System;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
namespace GeoDBATool
{
    public static class ModHistory
    {
        /// <summary>
        /// 创建历史库要素类，以数据集为单位


        /// </summary>
        /// <param name="featureDataset"></param>
        /// <param name="sourceFeatCls"></param>
        /// <param name="strTagetName"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static bool  CreateHistoryFeaClsStructure(IFeatureDataset featureDataset, IFeatureClass sourceFeatCls, string strTagetName, out Exception err)
        {
            try
            {
                err = null;
                IFeatureWorkspace featureWorkspace = featureDataset.Workspace as IFeatureWorkspace;

                //取源图层字段,并添加字段FromDate(生效日期),ToDate(失效日期),SourceOID(现势库对应数据OID),State(更新变化状态)
                IFields pFields = (sourceFeatCls.Fields as IClone).Clone() as IFields;
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                IField newfield = new FieldClass();                //字段对象
                IFieldEdit fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "USERNAME";
                fieldEdit.AliasName_2 = "用户名";
                fieldEdit.IsNullable_2 = true;
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "RANGEID";
                fieldEdit.AliasName_2 = "任务范围号";
                fieldEdit.IsNullable_2 = true;
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "FromDate";
                fieldEdit.AliasName_2 = "生效日期";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "ToDate";
                fieldEdit.AliasName_2 = "失效日期";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "SourceOID";
                fieldEdit.AliasName_2 = "现势库对应数据OID";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "State";
                fieldEdit.AliasName_2 = "更新变化状态";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "VERSION";
                fieldEdit.AliasName_2 = "更新版本号";
                fieldEdit.DefaultValue_2 = 0;
                fieldEdit.IsNullable_2 = false;
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                pFieldsEdit.AddField(newfield);


                if (sourceFeatCls.FeatureType == esriFeatureType.esriFTSimple)
                {
                    featureDataset.CreateFeatureClass(strTagetName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                }
                else if (sourceFeatCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    IObjectClassDescription pObjectClassDesc = new AnnotationFeatureClassDescriptionClass();
                    UID pCLSID = pObjectClassDesc.InstanceCLSID;
                    UID pExtCLSID = pObjectClassDesc.ClassExtensionCLSID;
                    IFeatureWorkspaceAnno pFeatWorkspaceAnno = (IFeatureWorkspaceAnno)featureWorkspace;
                    IAnnoClass pAnnoCls = (IAnnoClass)sourceFeatCls.Extension;
                    IGraphicsLayerScale pRefScale = new GraphicsLayerScaleClass();

                    // 设置参考比例尺的相关参数


                    pRefScale.ReferenceScale = pAnnoCls.ReferenceScale;
                    pRefScale.Units = pAnnoCls.ReferenceScaleUnits;

                    pFeatWorkspaceAnno.CreateAnnotationClass(strTagetName, pFieldsEdit,
                                                                        pCLSID, pExtCLSID, sourceFeatCls.ShapeFieldName,
                                                                        "", featureDataset, null, pAnnoCls.AnnoProperties,
                                                                        pRefScale, pAnnoCls.SymbolCollection, false);


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
                err = e;
                return false;
            }
        }
        /// <summary>
        /// 创建临时库要素类，以数据集为单位


        /// </summary>
        /// <param name="featureDataset"></param>
        /// <param name="sourceFeatCls"></param>
        /// <param name="strTagetName"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static bool CreateTmpFeaClsStructure(IFeatureDataset  featureDataset, IFeatureClass sourceFeatCls, string strTagetName, out Exception err)
        {
            try
            {
                err = null;
                IFeatureWorkspace featureWorkspace = featureDataset.Workspace as IFeatureWorkspace;

                //取源图层字段,并添加字段FromDate(生效日期),ToDate( ),SourceOID(现势库对应数据OID),State(更新变化状态)
                IFields pFields = (sourceFeatCls.Fields as IClone).Clone() as IFields;
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                IField newfield = new FieldClass();                //字段对象
                IFieldEdit fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "USERNAME";
                fieldEdit.AliasName_2 = "入库用户名";
                fieldEdit.IsNullable_2 = true;
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                pFieldsEdit.AddField(newfield);

                //newfield = new FieldClass();                //字段对象
                //fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                //fieldEdit.Name_2 = "RANGEID";
                //fieldEdit.AliasName_2 = "任务范围号";
                //fieldEdit.IsNullable_2 = true;
                ////字段类型要装化为枚举类型
                //fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                //pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "PutInDate";
                fieldEdit.AliasName_2 = "入库日期";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                pFieldsEdit.AddField(newfield);

                //newfield = new FieldClass();                //字段对象
                //fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                //fieldEdit.Name_2 = "ToDate";
                //fieldEdit.AliasName_2 = "失效日期";
                ////字段类型要装化为枚举类型
                //fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                //pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "CheckState";
                fieldEdit.AliasName_2 = "审核状态";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                pFieldsEdit.AddField(newfield);

                //newfield = new FieldClass();                //字段对象
                //fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                //fieldEdit.Name_2 = "State";
                //fieldEdit.AliasName_2 = "更新变化状态";
                ////字段类型要装化为枚举类型
                //fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                //pFieldsEdit.AddField(newfield);
                //ygc 2013-01-19 屏蔽部分字段，山西临时库只要三个字段

                if (sourceFeatCls.FeatureType == esriFeatureType.esriFTSimple)
                {
                 IFeatureClass pfeatureClass=featureDataset.CreateFeatureClass(strTagetName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                }
                else if (sourceFeatCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    IObjectClassDescription pObjectClassDesc = new AnnotationFeatureClassDescriptionClass();
                    UID pCLSID = pObjectClassDesc.InstanceCLSID;
                    UID pExtCLSID = pObjectClassDesc.ClassExtensionCLSID;
                    IFeatureWorkspaceAnno pFeatWorkspaceAnno = (IFeatureWorkspaceAnno)featureWorkspace;
                    IAnnoClass pAnnoCls = (IAnnoClass)sourceFeatCls.Extension;
                    IGraphicsLayerScale pRefScale = new GraphicsLayerScaleClass();

                    // 设置参考比例尺的相关参数

                    pRefScale.ReferenceScale = pAnnoCls.ReferenceScale;
                    pRefScale.Units = pAnnoCls.ReferenceScaleUnits;

                    pFeatWorkspaceAnno.CreateAnnotationClass(strTagetName, pFieldsEdit,
                                                                        pCLSID, pExtCLSID, sourceFeatCls.ShapeFieldName,
                                                                        "", featureDataset, null, pAnnoCls.AnnoProperties,
                                                                        pRefScale, pAnnoCls.SymbolCollection, false);


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
                err = e;
                return false;
            }
        }
    }
}
