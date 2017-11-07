using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using SCHEMEMANAGERCLASSESLib;

namespace GeoDataChecker
{
    public class GeoStructChecker : IDataCheckRealize
    {

        #region ICheckEvent 成员

        public event DataErrTreatHandle DataErrTreat;
        public event ProgressChangeHandle ProgressShow;
        /// <summary>
        /// 显示进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataCheck_ProgressShow(object sender, ProgressChangeEvent e)
        { }

        #endregion

        #region IDataCheck 成员

        private IArcgisDataCheckHook Hook;

        public void OnCreate(IDataCheckHook hook)
        {
            Hook = hook as IArcgisDataCheckHook;
            
        }

        public void OnDataCheck()
        {

            if (Hook == null) return;
            IArcgisDataCheckParaSet dataCheckParaSet = Hook.DataCheckParaSet as IArcgisDataCheckParaSet;
            if (dataCheckParaSet == null) return;
            if (dataCheckParaSet.Workspace == null) return;

            //获取所有数据集
            SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet(dataCheckParaSet.Workspace);
            List<IDataset> featureDatasets = sysGisDataSet.GetAllFeatureClass();

            ISchemeProject m_pProject = null;                                  //数据库结构文件对象
            int m_DBScale = 0;                                                          //默认比例尺
            int m_DSScale = 0;                                                          //数据集比例尺

            Dictionary<string, IFeatureClass> pLayerDic = new Dictionary<string, IFeatureClass>();            //记录加载到map上的图层对象                

            foreach (IDataset var in featureDatasets)
            {
                string FCName=var.Name;
                if(FCName.Contains("."))
                {
                    FCName = FCName.Substring(FCName.IndexOf('.') + 1);
                }
                IFeatureClass FC = var as IFeatureClass;
                if (FC != null && !pLayerDic.ContainsKey(FCName))
                {
                    pLayerDic.Add(FCName, FC);
                }

            }

            ///读取配置方案到对象
            ///
            m_pProject = new SchemeProjectClass();     //创建实例
            m_pProject.Load(GeoDataChecker.DBSchemaPath, e_FileType.GO_SCHEMEFILETYPE_MDB);    //加载schema文件

            if (m_pProject != null)
            {
                #region 获得比例尺信息
                string DBScale = m_pProject.get_MetaDataValue("Scale") as string;   //获取比例尺信息（总工程中的默认比例尺）
                string[] DBPScaleArayy = DBScale.Split(':');
                m_DBScale = Convert.ToInt32(DBPScaleArayy[1]);
                #endregion

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

                    ////遍历属性表
                    int TabNum = pDBs.GetCount();
                    for (int j = 0; j < TabNum; j++)
                    {

                        //获取属性表信息
                        ISchemeItem pTable = pDBs.get_ItemByIndex(j);  //获取属性表对象
                        string pFeatureClassName = pTable.Name;     //要素类名称

                        ///检查图层是否存在
                        if (!pLayerDic.ContainsKey(pFeatureClassName))
                        {
                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.目标要素类缺失.GetHashCode(),"要素类在标准中存在，却在目标数据集中未找到", 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString()}));
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                            continue;
                        }

                        #region 检查要素类类型是否与配置方案一致
                        string pFeatureClassType = pTable.Value as string;   //要素类类型
                        ///检查图层类型是否一致
                        ///
                        IFeatureClass pfeatureclass;

                        bool GetlyrSeccess = pLayerDic.TryGetValue(pFeatureClassName, out pfeatureclass);

                        if (GetlyrSeccess)
                        {
                            //IFeatureLayer pFeatureLayer = player as IFeatureLayer;

                            switch (pFeatureClassType)
                            {
                                case "ANNO":
                                    if (pfeatureclass.FeatureType != esriFeatureType.esriFTAnnotation)
                                    {
                                        //传递错误日志
                                        IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.要素类类型不一致.GetHashCode(), "目标要素类应为 注记 类型", 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                                        DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                                    }
                                    break;
                                case "POINT":
                                    if (pfeatureclass.ShapeType != ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
                                    {
                                        //传递错误日志
                                        IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.要素类类型不一致.GetHashCode(), "目标要素类应为 点 类型", 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                                        DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                                    }
                                    break;
                                case "LINE":
                                    if (pfeatureclass.ShapeType != ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
                                    {
                                        //传递错误日志
                                        IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.要素类类型不一致.GetHashCode(), "目标要素类应为 线 类型", 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                                        DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                                    }
                                    break;
                                case "AREA":
                                    if (pfeatureclass.ShapeType != ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
                                    {
                                        //传递错误日志
                                        IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.要素类类型不一致.GetHashCode(), "目标要素类应为 面 类型", 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                                        DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        #endregion

                        //遍历字段
                        IAttributeList pAttrs = pTable.AttributeList;
                        long FNum = pAttrs.GetCount();

                        //检查非GIS平台定义的字段
                        IFields fields = pfeatureclass.Fields;

                        //循环属性表中的字段，添加到arcgis的字段对象中
                        for (int k = 0; k < FNum; k++)
                        {
                            //添加自定义属性字段
                            CheckFields(pAttrs, k, fields, pFeatureClassName, dataCheckParaSet);
                        }
                    }
                }

            }
            else
            {
                return;
            }
        }

                   /// <summary>
        /// 根据输入的字段属性对象，检查属性字段是否与标准中定义的一致
        /// </summary>
        /// <param name="pAttrs"></param>
        /// <param name="k"></param>
        /// <param name="fields"></param>
        private void CheckFields(IAttributeList pAttrs, int k, IFields fields, string pFeatureClassName, IArcgisDataCheckParaSet dataCheckParaSet)
        {
            //获取基本属性信息
            IAttribute pAttr = pAttrs.get_AttributeByIndex(k);

            //获取扩展属性信息
            IAttributeDes pAttrDes = pAttr.Description;

            ///检查是否存在该名称的字段
            ///
            int i = fields.FindField(pAttr.Name);

            if (i < 0)
            {
                //传递错误日志
                IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.属性字段缺失.GetHashCode(), "目标要素类的 " + pAttr.Name + " 字段缺失", 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                return;
            }

            IField pField = fields.get_Field(i);

            #region 字段类型检查
            ///检查字段类型是否一致
            ///
            string pfieldType = pAttr.Type.ToString();
            switch (pfieldType)
            {
                case "GO_VALUETYPE_STRING":
                    if (pField.Type != esriFieldType.esriFieldTypeString)
                    {
                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.属性字段类型不一致.GetHashCode(), "目标要素类的 " + pField.Name + " 字段应为字符串类型", 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                    }
                    break;
                case "GO_VALUETYPE_LONG":
                    if (pField.Type != esriFieldType.esriFieldTypeInteger)
                    {
                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.属性字段类型不一致.GetHashCode(), "目标要素类的 " + pField.Name + " 字段应为长整类型", 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                    }
                    break;
                case "GO_VALUETYPE_BOOL":
                    break;
                case "GO_VALUETYPE_DATE":
                    if (pField.Type != esriFieldType.esriFieldTypeDate)
                    {
                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.属性字段类型不一致.GetHashCode(), "目标要素类的 " + pField.Name + " 字段应为日期类型", 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                    }
                    break;
                case "GO_VALUETYPE_FLOAT":
                    if (pField.Type != esriFieldType.esriFieldTypeSingle)
                    {
                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.属性字段类型不一致.GetHashCode(), "目标要素类的 " + pField.Name + " 字段应为浮点类型", 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                    }
                    break;
                case "GO_VALUETYPE_DOUBLE":
                    if (pField.Type != esriFieldType.esriFieldTypeDouble)
                    {
                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.属性字段类型不一致.GetHashCode(), "目标要素类的 " + pField.Name + " 字段应为双精度类型", 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                    }
                    break;

                default:
                    break;
            }
            #endregion

            #region 字段长度检查
            if (pfieldType != "GO_VALUETYPE_DOUBLE")
            {
                if (pField.Length != Convert.ToInt32(pAttrDes.InputWidth))
                {
                    //传递错误日志
                    IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.字段扩展属性不一致.GetHashCode(), "目标要素类的 " + pField.Name + " 字段长度应" + Convert.ToInt32(pAttrDes.InputWidth).ToString(), 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                    DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                    DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                }
            }
            #endregion

            #region 字段可否为空检查
            if (pField.IsNullable != pAttrDes.AllowNull)
            {
                //传递错误日志
                IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.字段扩展属性不一致.GetHashCode(), "目标要素类的 " + pField.Name + " 字段是否为空属性应为" + pAttrDes.AllowNull.ToString(), 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
            }
            #endregion

            #region 字段是否必须存在检查
            if (pField.Required != bool.Parse(pAttrDes.Necessary.ToString()))
            {
                //传递错误日志
                IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.字段扩展属性不一致.GetHashCode(), "目标要素类的 " + pField.Name + " 字段是否必须属性应为" + pAttrDes.Necessary.ToString(), 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
            }
            #endregion

            #region 字段是否可编辑检查

            #endregion

            #region 字段值域是否可变检查

            #endregion

            #region 字段精度检查
            //双精度类型不可设置精度（在PDB和GDB中不会出现错误，但是在SDE中会抛出“无效的列”错误）
            if (pfieldType != "GO_VALUETYPE_DOUBLE")
            {
                if (pField.Precision != Convert.ToInt32(pAttrDes.PrecisionEx))
                {
                    //传递错误日志
                    IDataErrInfo dataErrInfo = new DataErrInfo(new List<object>(new object[] { "批量检查", "数据结构检查", dataCheckParaSet.Workspace.PathName, enumErrorType.字段扩展属性不一致.GetHashCode(), "目标要素类的 " + pField.Name + " 字段精度应为" + pAttrDes.PrecisionEx.ToString(), 0, 0, pFeatureClassName, 0, "", 0, false, System.DateTime.Now.ToString() }));
                    DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                    DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                }
            }
            #endregion
        }

        #endregion

        public void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e)
        {
        }
    }
}
