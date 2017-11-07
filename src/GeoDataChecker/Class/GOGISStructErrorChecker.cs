using System;
using System.Collections.Generic;
using System.Text;
using SCHEMEMANAGERCLASSESLib;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;


namespace GeoDataChecker
{
    /// <summary>
    /// 检查目标的图层与属性结构是否与标准中的一致
    /// </summary>
    public class GOGISStructErrorChecker : GOGISErrorChecker
    {

        public GOGISStructErrorChecker()
        {
            _StructErrorEventArgs = new ErrorEventArgs();
        }

        /// <summary>
        /// 数据库标准文件访问路径
        /// </summary>
        private string _DBSchemaDocPath;
        private ErrorEventArgs _StructErrorEventArgs;

        public string DBSchemaDocPath
        {
            get { return _DBSchemaDocPath; }
            set { _DBSchemaDocPath = value; }
        }

        /// <summary>
        /// 执行检查
        /// </summary>
        /// <param name="AppHk"></param>
        public void ExcuteCheck(Plugin.Application.IAppGISRef AppHk)
        {
            Plugin.Application.IAppFormRef pAppForm = AppHk as Plugin.Application.IAppFormRef;

            ISchemeProject m_pProject = null;                                  //数据库结构文件对象
            int m_DBScale = 0;                                                          //默认比例尺
            int m_DSScale = 0;                                                          //数据集比例尺

            ///从图层上获取所有的要素类
            ///
            IMap CurMap = AppHk.MapControl.Map as IMap;      //获得当前地图对象
            
            Dictionary<string, ILayer> pLayerDic = new Dictionary<string, ILayer>();            //记录加载到map上的图层对象                                    
            for (int i = 0; i < CurMap.LayerCount; i++)
            {
                
                ILayer player=CurMap.get_Layer(i);

                //如果是图层组，则退出不做检查
                if (player is IGroupLayer)
                {
                    ICompositeLayer pComLayer = player as ICompositeLayer;
                    for(int k=0;k<pComLayer.Count;k++)
                    {
                        ILayer mmLayer = pComLayer.get_Layer(k);
                        if(mmLayer==null) continue;
                        IFeatureLayer mmFeaLyer = mmLayer as IFeatureLayer;
                        if(mmFeaLyer==null) continue;
                        IDataset mDT=mmFeaLyer.FeatureClass as IDataset;
                        string tempName = mDT.Name;
                        if(tempName.Contains("."))
                        {
                            tempName = tempName.Substring(tempName.IndexOf('.') + 1);
                        }
                        if (!pLayerDic.ContainsKey(tempName))
                        {
                            pLayerDic.Add(tempName, mmLayer);
                        }
                    }
                    continue;
                }
                IFeatureLayer pFeatlayer=player as IFeatureLayer;
                if (pFeatlayer == null) continue;
                IDataset pdataset=pFeatlayer.FeatureClass as IDataset;

                string tempNm = pdataset.Name;
                if (tempNm.Contains("."))
                {
                    tempNm = tempNm.Substring(tempNm.IndexOf('.') + 1);
                }
                if (player != null && !pLayerDic.ContainsKey(tempNm))
                {
                    pLayerDic.Add(tempNm, player);
                }
            }


            ///读取配置方案到对象
            ///
            m_pProject = new SchemeProjectClass();     //创建实例
            m_pProject.Load(this._DBSchemaDocPath, e_FileType.GO_SCHEMEFILETYPE_MDB);    //加载schema文件

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
                            ///触发错误事件
                            this._StructErrorEventArgs.ErrorName = "要素类缺失";
                            this._StructErrorEventArgs.ErrDescription = "该要素类在标准中存在，却在当前数据中不存在";
                            this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                            this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                            this.OnProgressStep(AppHk, j + 1, TabNum);
                            continue;
                        }

                        #region 检查要素类类型是否与配置方案一致
                        string pFeatureClassType = pTable.Value as string;   //要素类类型
                        ///检查图层类型是否一致
                        ///
                        ILayer player;

                        bool GetlyrSeccess = pLayerDic.TryGetValue(pFeatureClassName, out player);

                        if (GetlyrSeccess)
                        {
                            IFeatureLayer pFeatureLayer = player as IFeatureLayer;

                            switch (pFeatureClassType)
                            {
                                case "ANNO":
                                    if (pFeatureLayer.FeatureClass.FeatureType != esriFeatureType.esriFTAnnotation)
                                    {
                                        ///触发错误事件
                                        this._StructErrorEventArgs.ErrorName = "要素类类型不一致";
                                        this._StructErrorEventArgs.ErrDescription = "该要素类在标准中被定义为注记类型";
                                        this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                                        this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                                    }
                                    break;
                                case "POINT":
                                    if (pFeatureLayer.FeatureClass.ShapeType != ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
                                    {
                                        ///触发错误事件
                                        this._StructErrorEventArgs.ErrorName = "要素类类型不一致";
                                        this._StructErrorEventArgs.ErrDescription = "该要素类在标准中被定义为点要素类型";
                                        this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                                        this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                                    }
                                    break;
                                case "LINE":
                                    if (pFeatureLayer.FeatureClass.ShapeType != ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
                                    {
                                        ///触发错误事件
                                        this._StructErrorEventArgs.ErrorName = "要素类类型不一致";
                                        this._StructErrorEventArgs.ErrDescription = "该要素类在标准中被定义为线要素类型";
                                        this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                                        this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                                    }
                                    break;
                                case "AREA":
                                    if (pFeatureLayer.FeatureClass.ShapeType != ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
                                    {
                                        ///触发错误事件
                                        this._StructErrorEventArgs.ErrorName = "要素类类型不一致";
                                        this._StructErrorEventArgs.ErrDescription = "该要素类在标准中被定义为面要素类型";
                                        this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                                        this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            ///触发错误事件
                            this._StructErrorEventArgs.ErrorName = "获取要素类失败";
                            this._StructErrorEventArgs.ErrDescription = "获取图层失败";
                            this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                            this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                            this.OnProgressStep(AppHk, j + 1, TabNum);
                            continue;
                        }
                        #endregion

                        //遍历字段
                        IAttributeList pAttrs = pTable.AttributeList;
                        long FNum = pAttrs.GetCount();

                        //检查非GIS平台定义的字段
                        IFeatureLayer pChecklyr =player as IFeatureLayer;
                        IFeatureClass pFeatureClass=pChecklyr.FeatureClass;
                        IFields fields = pFeatureClass.Fields;

                        //循环属性表中的字段，添加到arcgis的字段对象中
                        for (int k = 0; k < FNum; k++)
                        {
                            //添加自定义属性字段
                            CheckFields(pAttrs, k, fields, pFeatureClassName, AppHk);
                        }

                        this.OnProgressStep(AppHk, j+1, TabNum);
                    }
                }

            }
            else
            {
                SetCheckState.Message(pAppForm, "提示", "加载配置方案失败！");
            }


        }

        /// <summary>
        /// 根据输入的字段属性对象，检查属性字段是否与标准中定义的一致
        /// </summary>
        /// <param name="pAttrs"></param>
        /// <param name="k"></param>
        /// <param name="fields"></param>
        private void CheckFields(IAttributeList pAttrs, int k, IFields fields, string pFeatureClassName, Plugin.Application.IAppGISRef AppHk)
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
                ///触发错误事件
                this._StructErrorEventArgs.ErrorName = "属性字段缺失";
                this._StructErrorEventArgs.ErrDescription = "在目标要素类中不存在 " + pAttr.Name + " 字段，而该字段已经在标准中定义";
                this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                this.OnErrorFind(AppHk, this._StructErrorEventArgs);
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
                        ///触发错误事件
                        this._StructErrorEventArgs.ErrorName = "属性字段类型不一致";
                        this._StructErrorEventArgs.ErrDescription = "在目标要素类中 " + pAttr.Name + " 字段类型被定义为字符串型";
                        this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                        this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                    }
                    break;
                case "GO_VALUETYPE_LONG":
                    if (pField.Type != esriFieldType.esriFieldTypeInteger)
                    {
                        ///触发错误事件
                        this._StructErrorEventArgs.ErrorName = "属性字段类型不一致";
                        this._StructErrorEventArgs.ErrDescription = "在目标要素类中 " + pAttr.Name + " 字段类型被定义为长整形型";
                        this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                        this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                    }
                    break;
                case "GO_VALUETYPE_BOOL":
                    break;
                case "GO_VALUETYPE_DATE":
                    if (pField.Type != esriFieldType.esriFieldTypeDate)
                    {
                        ///触发错误事件
                        this._StructErrorEventArgs.ErrorName = "属性字段类型不一致";
                        this._StructErrorEventArgs.ErrDescription = "在目标要素类中 " + pAttr.Name + " 字段类型被定义为日期型";
                        this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                        this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                    }
                    break;
                case "GO_VALUETYPE_FLOAT":
                    if (pField.Type != esriFieldType.esriFieldTypeSingle)
                    {
                        ///触发错误事件
                        this._StructErrorEventArgs.ErrorName = "属性字段类型不一致";
                        this._StructErrorEventArgs.ErrDescription = "在目标要素类中 " + pAttr.Name + " 字段类型被定义为浮点型";
                        this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                        this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                    }
                    break;
                case "GO_VALUETYPE_DOUBLE":
                    if (pField.Type != esriFieldType.esriFieldTypeDouble)
                    {
                        ///触发错误事件
                        this._StructErrorEventArgs.ErrorName = "属性字段类型不一致";
                        this._StructErrorEventArgs.ErrDescription = "在目标要素类中 " + pAttr.Name + " 字段类型被定义为双精度型";
                        this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                        this.OnErrorFind(AppHk, this._StructErrorEventArgs);
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
                    ///触发错误事件
                    this._StructErrorEventArgs.ErrorName = "字段扩展属性不一致";
                    this._StructErrorEventArgs.ErrDescription = "在目标要素类中 " + pAttr.Name + " 字段的长度属性在标准中的定义为" + pAttrDes.InputWidth.ToString();
                    this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                    this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                }
            }
            #endregion

            #region 字段可否为空检查
            if (pField.IsNullable != pAttrDes.AllowNull)
            {
                ///触发错误事件
                this._StructErrorEventArgs.ErrorName = "字段扩展属性不一致";
                this._StructErrorEventArgs.ErrDescription = "在目标要素类中 " + pAttr.Name + " 字段的是否可为空属性在标准中的定义为" + pAttrDes.AllowNull.ToString();
                this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                this.OnErrorFind(AppHk, this._StructErrorEventArgs);
            }
            #endregion

            #region 字段是否必须存在检查
            if (pField.Required != bool.Parse(pAttrDes.Necessary.ToString()))
            {
                ///触发错误事件
                this._StructErrorEventArgs.ErrorName = "字段扩展属性不一致";
                this._StructErrorEventArgs.ErrDescription = "在目标要素类中 " + pAttr.Name + " 字段的是否为必须属性在标准中的定义为" + pAttrDes.Necessary.ToString();
                this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                this.OnErrorFind(AppHk, this._StructErrorEventArgs);
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
                    ///触发错误事件
                    this._StructErrorEventArgs.ErrorName = "字段扩展属性不一致";
                    this._StructErrorEventArgs.ErrDescription = "在目标要素类中 " + pAttr.Name + " 字段的精度属性在标准中的定义为" + pAttrDes.PrecisionEx.ToString();
                    this._StructErrorEventArgs.FeatureClassName = pFeatureClassName;
                    this.OnErrorFind(AppHk, this._StructErrorEventArgs);
                }
            }
            #endregion
        }
    }
}
