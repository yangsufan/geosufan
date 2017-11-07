using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.OracleClient;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;

namespace GeoDBATool
{
    /// <summary>
    /// 初始化历史库：包括三个步骤：根据现势库体创建历史库、将现势库的数据导入到历史库、创建远程日志记录表、将历史库连接信息写入XML
    /// </summary>
    public class ControlsInitialDBHistory : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsInitialDBHistory()
        {
            base._Name = "GeoDBATool.ControlsInitialDBHistory";
            base._Caption = "初始化历史库";
            base._Tooltip = "初始化历史库";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "初始化历史库";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //cyf 20110625 modify:
                //if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "project") return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "FD") return false;
                if (m_Hook.ProjectTree.SelectedNode.Parent.Text != "现势库") return false;
                //end
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            //frmInitialDBHistory newFrm = new frmInitialDBHistory(m_Hook.ProjectTree.SelectedNode.Tag as XmlElement);
            //newFrm.ShowDialog();

            Exception err = null;
            XmlElement pProElem = null;                         //数据库工程节点xml
            XmlElement pDbProjectElement = null;              //数据库节点
            string pDBType = "";                              //数据库类型
            string pServer = "";                              //服务器
            string pInstance = "";                            //实例名
            string pDataBase = "";                            //数据库
            string pUser = "";                                //用户
            string pPassword = "";                            //密码
            string pVersion = "";                             //版本
            string pFeaDatasetName = "";                      //数据集名称
            string sTodatestr = DateTime.Now.ToString("u");
            IWorkspace pTagetWorkspace = null;                  //目标数据库工作空间：历史库
            //cyf 20110625 modify 
            pDbProjectElement = m_Hook.ProjectTree.SelectedNode.Parent.Tag as XmlElement;    //数据库节点的xml信息
            pProElem = m_Hook.ProjectTree.SelectedNode.Parent.Parent.Tag as XmlElement;       //数据库工程节点的xml信息
            //数据库连接信息
            XmlElement aElement = pDbProjectElement.SelectSingleNode(".//连接信息") as XmlElement;
            //end
            pDBType = aElement.GetAttribute("类型");
            pServer = aElement.GetAttribute("服务器");
            pInstance = aElement.GetAttribute("服务名");
            pDataBase = aElement.GetAttribute("数据库");
            pVersion = aElement.GetAttribute("版本");
            pUser = aElement.GetAttribute("用户");
            pPassword = aElement.GetAttribute("密码");
            //数据集名称

            //cyf 20110625 modify：获取数据集名称
            //XmlElement tElem = pDbProjectElement.SelectSingleNode(".//内容//现势库//连接信息//库体") as XmlElement;
            //pFeaDatasetName = tElem.GetAttribute("名称");
            pFeaDatasetName = m_Hook.ProjectTree.SelectedNode.Text;
            //end
            //================================================================================================
            //将历史库连接信息写入XML
            XmlElement aHisElement = pProElem.SelectSingleNode(".//内容//历史库//连接信息") as XmlElement;
            aHisElement.SetAttribute("类型", pDBType);
            aHisElement.SetAttribute("服务器", pServer);
            aHisElement.SetAttribute("服务名", pInstance);
            aHisElement.SetAttribute("数据库", pDataBase);
            aHisElement.SetAttribute("版本", pVersion);
            aHisElement.SetAttribute("用户", pUser);
            aHisElement.SetAttribute("密码", pPassword);

            //设置数据库连接

            List<string> Pra = new List<string>();
            SysCommon.Gis.SysGisDataSet sourceSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
            switch (pDBType)
            {
                case "SDE":
                    sourceSysGisDataSet.SetWorkspace(pServer, pInstance, pDataBase, pUser, pPassword, pVersion, out err);
                    Pra.Add("SDE");
                    Pra.Add(pServer);
                    Pra.Add(pInstance);
                    Pra.Add(pDataBase);
                    Pra.Add(pUser);
                    Pra.Add(pVersion);

                    break;
                case "GDB":
                    sourceSysGisDataSet.SetWorkspace(pDataBase, SysCommon.enumWSType.GDB, out err);
                    Pra.Add("GDB");
                    Pra.Add(pDataBase);
                    break;
                case "PDB":
                    sourceSysGisDataSet.SetWorkspace(pDataBase, SysCommon.enumWSType.PDB, out err);
                    Pra.Add("PDB");
                    Pra.Add(pDataBase);
                    break;
                default:
                    break;
            }

            if (err != null || sourceSysGisDataSet.WorkSpace == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未设置数据库连接或连接失败,请检查!");
                return;
            }
            pTagetWorkspace = sourceSysGisDataSet.WorkSpace;

            if (SysCommon.Error.ErrorHandle.ShowFrmInformation("确认", "取消", "是否确认开始初始化历史库?"))
            {
                //*********************************************************
                //guozheng added 初始化历史库 Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write("初始化历史库", Pra, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write("初始化历史库", Pra, DateTime.Now);
                }

                //*********************************************************
                //==============================================================================================
                //创建库体
                List<string> lstData = new List<string>();
                try
                {
                    #region 根据用户数据库结构创建历史库
                    if (pFeaDatasetName != "")
                    {
                        //有数据集
                        IFeatureDataset tagetFeatureDataset = null;
                        IFeatureDataset pFeatureDataset = sourceSysGisDataSet.GetFeatureDataset(pFeaDatasetName, out err);
                        if (!(pTagetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureDataset, pFeaDatasetName + "_GOH"))
                        {
                            tagetFeatureDataset = (pTagetWorkspace as IFeatureWorkspace).CreateFeatureDataset(pFeaDatasetName + "_GOH", (pFeatureDataset as IGeoDataset).SpatialReference);
                        }
                        else
                        {
                            //cyf 20110706 modify :若存在历史库，说明已经初始化过
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "该历史库已经进行了初始化!");
                            return;
                            //tagetFeatureDataset = (pTagetWorkspace as IFeatureWorkspace).OpenFeatureDataset(pFeaDatasetName + "_GOH");
                            //end
                        }

                        IEnumDataset pEnumDs = pFeatureDataset.Subsets;
                        pEnumDs.Reset();
                        IDataset pDs = pEnumDs.Next();
                        while (pDs != null)
                        {
                            IFeatureClass pFeatureClass = pDs as IFeatureClass;
                            if (pFeatureClass != null)
                            {
                                if (!(pTagetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, pDs.Name + "_GOH"))
                                {
                                    CreateFeatCls(tagetFeatureDataset, pFeatureClass, pDs.Name + "_GOH", out err);
                                    if (err != null)
                                    {
                                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建历史库库体结构失败!\n" + err.Message);
                                        return;
                                    }
                                }
                                lstData.Add(pDs.Name);
                            }
                            pDs = pEnumDs.Next();
                        }
                    }
                    else
                    {
                        if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "数据集名称为空，是否创建离散的要素类"))
                        {
                            //创建离散的要素类
                            List<IDataset> LstFeaCls = new List<IDataset>();          //要素类集合
                            LstFeaCls = sourceSysGisDataSet.GetAllFeatureClass();
                            foreach (IDataset pDt in LstFeaCls)
                            {
                                string pFeaClsName = pDt.Name;
                                if (pFeaClsName.Contains("_GOH")) continue;

                                IFeatureClass pFeatureClass = pDt as IFeatureClass;
                                if (!(pTagetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, pFeaClsName + "_GOH"))
                                {
                                    CreateFeatCls(pTagetWorkspace as IFeatureWorkspace, pFeatureClass, pFeaClsName + "_GOH", out err);
                                    if (err != null)
                                    {
                                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建历史库库体结构失败!\n" + err.Message);
                                        return;
                                    }
                                }
                                if (!lstData.Contains(pFeaClsName))
                                {
                                    lstData.Add(pFeaClsName);
                                }
                            }
                        }
                        else
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "设置要数集的名称！");
                            return;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    //*******************************************************************
                    //guozheng added
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(ex, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(ex, null, DateTime.Now);
                    }
                    //********************************************************************
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建历史库库体结构失败!\n" + ex.Message);
                    return;
                }

                //================================================================================================
                //数据入库
                //遍历现势库数据FC进行数据移植
                Dictionary<string, string> dicFieldsPair = new Dictionary<string, string>();
                dicFieldsPair.Add("OBJECTID", "SourceOID");
                Dictionary<string, object> dicValue = new Dictionary<string, object>();
                dicValue.Add("FromDate", DateTime.Now.ToString("u"));
                dicValue.Add("ToDate", DateTime.MaxValue.ToString("u"));//.ToString("YYYY-MM-DD HH:MI:SS"));
                dicValue.Add("State", 0);
                (pTagetWorkspace as IWorkspaceEdit).StartEditing(false);
                bool res = true;

                foreach (string aFeatClsName in lstData)
                {
                    #region 数据入库
                    //状态栏
                    (m_Hook as Plugin.Application.IAppFormRef).OperatorTips = "正在进行图层" + aFeatClsName + "...";
                    //进度条显示


                    (m_Hook as Plugin.Application.IAppFormRef).ProgressBar.Visible = true;

                    Application.DoEvents();
                    int cnt = 0;
                    int allcnt = 0;
                    IFeatureCursor featureCursor = null;
                    IFeatureClass tagetFeatCls = null;
                    try
                    {
                        featureCursor = sourceSysGisDataSet.GetFeatureCursor(aFeatClsName, "", null, "", out err, out cnt, out allcnt);
                        tagetFeatCls = (pTagetWorkspace as IFeatureWorkspace).OpenFeatureClass(aFeatClsName + "_GOH");
                        //***********************************************************
                        //guozheng 2010-10-18 added  使已有的历史数据版本从生效状态改为失效状态

                        Exception ex = null;
                        SetHisDataInvalid(tagetFeatCls, sTodatestr, out ex);
                        //***********************************************************
                    }
                    catch (Exception ex)
                    {
                        //*******************************************************************
                        //guozheng added
                        if (ModData.SysLog != null)
                        {
                            ModData.SysLog.Write(ex, null, DateTime.Now);
                        }
                        else
                        {
                            ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                            ModData.SysLog.Write(ex, null, DateTime.Now);
                        }
                        //********************************************************************
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                        return;
                    }

                    //进度条


                    (m_Hook as Plugin.Application.IAppFormRef).ProgressBar.Maximum = cnt;
                    (m_Hook as Plugin.Application.IAppFormRef).ProgressBar.Minimum = 0;
                    (m_Hook as Plugin.Application.IAppFormRef).ProgressBar.Value = 0;

                    NewFeatures(tagetFeatCls, featureCursor, dicFieldsPair, dicValue, true, false, (m_Hook as Plugin.Application.IAppFormRef).ProgressBar, out err);
                    Marshal.ReleaseComObject(featureCursor);
                    Application.DoEvents();
                    if (err != null)
                    {
                        res = false;
                        break;
                    }

                    (m_Hook as Plugin.Application.IAppFormRef).OperatorTips = "";
                    #endregion
                }

                if (!res)
                {
                    (pTagetWorkspace as IWorkspaceEdit).StopEditing(false);
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "生成历史库失败!");
                    return;
                }
                //cyf 20110705 创建完历史库后，刷新树图
                //先清空树图
                m_Hook.ProjectTree.Nodes.Clear();
                //添加节点
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ModData.v_projectDetalXML);
                //刷新树图
                ModDBOperator.RefreshProjectTree(xmlDoc, m_Hook.ProjectTree, out err);
                if (err != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "刷新树图失败!");
                    return;
                }
                //end

                //================================================================================================
                //将历史库信息写入XML
                //cyf 20110625
                //(aHisElement.FirstChild as XmlElement).SetAttribute("名称", pFeaDatasetName + "_GOH");
                //aHisElement.OwnerDocument.Save(ModData.v_projectXML);
                //end
                //=================================================================================================
                //创建远程日志记录表

                //if (!CreateSQLTable(pTagetWorkspace, pDBType, out err))//!CreateTable(pTagetWorkspace,out err)
                //{
                //    res = false;
                //    (pTagetWorkspace as IWorkspaceEdit).StopEditing(false);
                //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建远程日志表失败！\n" + err.Message);
                //    return;
                //}
                CreateSQLTable(pTagetWorkspace, pDBType, out err);//20110828 xisheng
                try
                {
                    (pTagetWorkspace as IWorkspaceEdit).StopEditing(res);//数据量过大，可能会报错；xisheng 2011.06.30
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "操作完成！");
                }
                catch (Exception error)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", error.Message);
                }
                (m_Hook as Plugin.Application.IAppFormRef).ProgressBar.Visible = false;
            }
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }

        /// <summary>
        /// 创建要素类，离散的要素类
        /// </summary>
        /// <param name="featureWorkspace"></param>
        /// <param name="sourceFeatCls"></param>
        /// <param name="strTagetName"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        private bool CreateFeatCls(IFeatureWorkspace featureWorkspace, IFeatureClass sourceFeatCls, string strTagetName, out Exception err)
        {
            try
            {
                err = null;
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
                    featureWorkspace.CreateFeatureClass(strTagetName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
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
                                                                        "", null, null, pAnnoCls.AnnoProperties,
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
        /// 创建要素类，以数据集为单位


        /// </summary>
        /// <param name="featureDataset"></param>
        /// <param name="sourceFeatCls"></param>
        /// <param name="strTagetName"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        private bool CreateFeatCls(IFeatureDataset featureDataset, IFeatureClass sourceFeatCls, string strTagetName, out Exception err)
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
                fieldEdit.IsNullable_2= true;
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

        private bool NewFeatures(IFeatureClass ObjFeatureCls, IFeatureCursor pfeacursor, Dictionary<string, string> dicFieldsPair, Dictionary<string, object> values, bool useOrgFdVal, bool bIngore, DevComponents.DotNetBar.ProgressBarItem progressBar, out Exception eError)
        {
            eError = null;
            if (ObjFeatureCls == null) return false;
            IFeatureBuffer pFeatureBuffer = ObjFeatureCls.CreateFeatureBuffer();
            IFeatureCursor pObjFeaCursor = ObjFeatureCls.Insert(true);
            IFeature pFeature = null;
            if (pObjFeaCursor != null)
            {
                pFeature = pfeacursor.NextFeature();
            }
            else
            {
                return false;
            }
            while (pFeature != null)
            {
                
                try
                {
                    //***************************************
                    //guozheng 2011-4-11 added 对空要素的保护
                    //空要素不需要
                    if (pFeature.Shape != null)  //wgf 20111109 死机
                    {
                        if (pFeature.Shape.IsEmpty)
                        {
                            pFeature = pfeacursor.NextFeature();
                            continue;
                        }
                    }
                    else
                    {
                        pFeature = pfeacursor.NextFeature();
                        continue;
                    }
                    //****************************************
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
                    progressBar.Value++;
                    Application.DoEvents();
                }
                catch (Exception eX)
                {
                    //*******************************************************************
                    //guozheng added
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(eX, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(eX, null, DateTime.Now);
                    }
                    //********************************************************************
                    eError = eX;
                    if (bIngore == false)
                        break;
                    else
                        pFeature = pfeacursor.NextFeature();
                    progressBar.Value++;
                    Application.DoEvents();
                    continue;
                }
            }

            pObjFeaCursor.Flush();
            Marshal.ReleaseComObject(pObjFeaCursor);
            return true;
        }

        /// <summary>
        /// 创建远程日志记录表


        /// </summary>
        /// <param name="pWorkSpace"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private bool CreateTable(IWorkspace pWorkSpace, out Exception eError)
        {
            eError = null;
            try
            {
                IFeatureWorkspace pFeaWorkSpa = pWorkSpace as IFeatureWorkspace;
                if (!(pWorkSpace as IWorkspace2).get_NameExists(esriDatasetType.esriDTTable, "GO_DATABASE_UPDATELOG"))
                {
                    #region 创建表格"GO_DATABASE_UPDATELOG"
                    IFields pFields = new FieldsClass();
                    IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                    IField pNewField = new FieldClass();
                    IFieldEdit pFieldEdit = pNewField as IFieldEdit;
                    pFieldEdit.Name_2 = "OID";
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                    pFieldEdit.AliasName_2 = "OID";
                    pFieldsEdit.AddField(pNewField);

                    pNewField = new FieldClass();
                    pFieldEdit = pNewField as IFieldEdit;
                    pFieldEdit.Name_2 = "STATE";
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                    pFieldEdit.AliasName_2 = "STATE";
                    pFieldsEdit.AddField(pNewField);

                    pNewField = new FieldClass();
                    pFieldEdit = pNewField as IFieldEdit;
                    pFieldEdit.Name_2 = "LAYERNAME";
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    pFieldEdit.AliasName_2 = "LAYERNAME";
                    pFieldEdit.Length_2 = 50;
                    pFieldsEdit.AddField(pNewField);

                    pNewField = new FieldClass();
                    pFieldEdit = pNewField as IFieldEdit;
                    pFieldEdit.Name_2 = "LASTUPDATE";
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDate;
                    pFieldEdit.AliasName_2 = "LASTUPDATE";
                    pFieldsEdit.AddField(pNewField);

                    pNewField = new FieldClass();
                    pFieldEdit = pNewField as IFieldEdit;
                    pFieldEdit.Name_2 = "VERSION";
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                    pFieldEdit.AliasName_2 = "VERSION";
                    pFieldsEdit.AddField(pNewField);

                    pNewField = new FieldClass();
                    pFieldEdit = pNewField as IFieldEdit;
                    pFieldEdit.Name_2 = "XMIN";
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    pFieldEdit.AliasName_2 = "XMIN";
                    pFieldsEdit.AddField(pNewField);

                    pNewField = new FieldClass();
                    pFieldEdit = pNewField as IFieldEdit;
                    pFieldEdit.Name_2 = "XMAX";
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    pFieldEdit.AliasName_2 = "XMAX";
                    pFieldsEdit.AddField(pNewField);

                    pNewField = new FieldClass();
                    pFieldEdit = pNewField as IFieldEdit;
                    pFieldEdit.Name_2 = "YMIN";
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    pFieldEdit.AliasName_2 = "YMIN";
                    pFieldsEdit.AddField(pNewField);

                    pNewField = new FieldClass();
                    pFieldEdit = pNewField as IFieldEdit;
                    pFieldEdit.Name_2 = "YMAX";
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    pFieldEdit.AliasName_2 = "YMAX";
                    pFieldsEdit.AddField(pNewField);

                    pNewField = new FieldClass();
                    pFieldEdit = pNewField as IFieldEdit;
                    pFieldEdit.Name_2 = "OBJECTID";
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                    pFieldEdit.AliasName_2 = "OBJECTID";
                    pFieldsEdit.AddField(pNewField);

                    IObjectClassDescription pObjClassDes = new ObjectClassDescriptionClass();
                    UID pUID = pObjClassDes.InstanceCLSID;
                    UID pEXTUID = pObjClassDes.ClassExtensionCLSID;


                    //IWorkspaceConfiguration pWSConfig = pWorkSpace as IWorkspaceConfiguration;
                    //string pKeyWord=pWSConfig.ConfigurationKeywords;

                    pFeaWorkSpa.CreateTable("GO_DATABASE_UPDATELOG", pFields, pUID, pEXTUID, "");

                    #endregion
                }

                if (!(pWorkSpace as IWorkspace2).get_NameExists(esriDatasetType.esriDTTable, "go_database_version"))
                {
                    #region 创建表格"go_database_version"
                    IFields pFields2 = new FieldsClass();
                    IFieldsEdit pFieldsEdit2 = pFields2 as IFieldsEdit;

                    IField pNewField2 = new FieldClass();
                    IFieldEdit pFieldEdit2 = pNewField2 as IFieldEdit;
                    pFieldEdit2.Name_2 = "VERSION";
                    pFieldEdit2.Type_2 = esriFieldType.esriFieldTypeInteger;
                    pFieldEdit2.AliasName_2 = "远程数据库的版本";
                    pFieldsEdit2.AddField(pNewField2);

                    pNewField2 = new FieldClass();
                    pFieldEdit2 = pNewField2 as IFieldEdit;
                    pFieldEdit2.Name_2 = "USERNAME";
                    pFieldEdit2.Type_2 = esriFieldType.esriFieldTypeString;
                    pFieldEdit2.AliasName_2 = "建立版本用户名";
                    pFieldEdit2.Length_2 = 255;
                    pFieldsEdit2.AddField(pNewField2);

                    pNewField2 = new FieldClass();
                    pFieldEdit2 = pNewField2 as IFieldEdit;
                    pFieldEdit2.Name_2 = "VERSIONTIME";
                    pFieldEdit2.Type_2 = esriFieldType.esriFieldTypeDate;
                    pFieldEdit2.AliasName_2 = "建立版本的时间";
                    pFieldsEdit2.AddField(pNewField2);

                    pNewField2 = new FieldClass();
                    pFieldEdit2 = pNewField2 as IFieldEdit;
                    pFieldEdit2.Name_2 = "DESCRIBE";
                    pFieldEdit2.Type_2 = esriFieldType.esriFieldTypeString;
                    pFieldEdit2.AliasName_2 = "建立版本的相关描述";
                    pFieldEdit2.Length_2 = 255;
                    pFieldsEdit2.AddField(pNewField2);

                    pNewField2 = new FieldClass();
                    pFieldEdit2 = pNewField2 as IFieldEdit;
                    pFieldEdit2.Name_2 = "OBJECTID";
                    pFieldEdit2.Type_2 = esriFieldType.esriFieldTypeOID;
                    pFieldEdit2.AliasName_2 = "OBJECTID";
                    pFieldsEdit2.AddField(pNewField2);

                    IObjectClassDescription pObjClassDes2 = new ObjectClassDescriptionClass();
                    UID pUID2 = pObjClassDes2.InstanceCLSID;
                    UID pEXTUID2 = pObjClassDes2.ClassExtensionCLSID;

                    pFeaWorkSpa.CreateTable("go_database_version", pFields2, pUID2, pEXTUID2, "");
                    #endregion
                }

                return true;
            }
            catch (Exception ex)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                //********************************************************************
                eError = ex;
                return false;
            }
        }

        /// <summary>
        /// 创建远程日志表

        /// </summary>
        /// <param name="pWorkSpace">工作空间</param>
        /// <param name="pDBType">工作空间类型：PDB、GDB、SDE</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private bool CreateSQLTable(IWorkspace pWorkSpace, string pDBType, out Exception eError)
        {
            //创建远程日志表

            eError = null;

            //检查远程日志表是否存在，只要有一张表格存在，就返回

            ITable pTable = null;
            ITable mTable = null;
            IFeatureWorkspace pFeaWS = pWorkSpace as IFeatureWorkspace;
            if (pFeaWS == null) return false;
            try
            {
                pTable = pFeaWS.OpenTable("GO_DATABASE_UPDATELOG");
                if (pTable != null)
                {
                    eError = new Exception("远程日志表'GO_DATABASE_UPDATELOG'已经存在，请检查！");
                    return false;
                }
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
                try
                {
                    mTable = pFeaWS.OpenTable("go_database_version");
                    if (mTable != null)
                    {
                        eError = new Exception("远程日志表'go_database_version'已经存在，请检查！");
                        return false;
                    }
                }
                catch (Exception er)
                {
                    //*******************************************************************
                    //guozheng added
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(er, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(er, null, DateTime.Now);
                    }
                    //********************************************************************
                }
            }

            //创建表格
            try
            {
                if (pDBType.ToUpper() == "PDB")
                {
                    pWorkSpace.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  integer,STATE integer,LAYERNAME varchar(50),LASTUPDATE date,VERSION integer,XMIN float,XMAX float,YMIN float,YMAX float)");
                    pWorkSpace.ExecuteSQL("create table go_database_version (VERSION  integer,USERNAME varchar(255),VERSIONTIME date,DES varchar(255))");

                }
                else if (pDBType.ToUpper() == "SDE")
                {
                    pWorkSpace.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  INTEGER,STATE INTEGER,LAYERNAME NVARCHAR2(50),LASTUPDATE DATE,VERSION INTEGER,XMIN FLOAT,XMAX FLOAT,YMIN FLOAT,YMAX FLOAT)");
                    pWorkSpace.ExecuteSQL("create table go_database_version (VERSION  INTEGER,USERNAME NVARCHAR2(255),VERSIONTIME DATE,DES NVARCHAR2(255))");
                }
                else if (pDBType.ToUpper() == "GDB")
                {
                    string tempFile = ModData.netLogFile;
                    FileInfo pFI = new FileInfo(tempFile);
                    string fName = pFI.Name;  //远程日志表名

                    //日志存储路径
                    string dbPath = pWorkSpace.PathName;
                    int index = dbPath.LastIndexOf('\\');
                    if (index == -1) return false;
                    string FileDic = dbPath.Substring(0, index);
                    string FileName = FileDic + "\\" + fName;

                    if (File.Exists(FileName))
                    {
                        if (!SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "日志文件'" + fName + "'已存在,\n是否替换？"))
                        {
                            return true;
                        }
                        else
                        {
                            File.Delete(FileName);
                        }
                    }
                    File.Copy(tempFile, FileName);
                }
                return true;
            }
            catch (System.Exception ex)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                //********************************************************************
                eError = ex;
                return false;
            }

        }

        /// <summary>
        /// 2010-10-18 guozheng added
        /// 初始化历史库体前，将原版本设置为失效状态(Todate设置为：DateTime.Now.ToLongDateString())
        /// </summary>
        /// <param name="DesHisFeature">历史库FeatureClass名称</param>
        /// <param name="ex">输出：错误信息</param>
        private void SetHisDataInvalid(IFeatureClass DesHisFeature, string sToDatastr, out Exception ex)
        {
            ex = null;
            if (DesHisFeature == null) { ex = new Exception("输入的要素集为空!"); return; }
            try
            {
                IQueryFilter queryFilter = new QueryFilterClass();
                string sValue = DateTime.MaxValue.ToString("u");
                string sNow = sToDatastr;
                queryFilter.WhereClause = "ToDate='" + sValue + "'";
                IFeatureCursor getFeaCur = DesHisFeature.Search(queryFilter, false);
                if (getFeaCur == null) { ex = new Exception("查询要素错误"); return; }
                IFeature getFeature = getFeaCur.NextFeature();
                while (getFeature != null)
                {
                    int index = -1;
                    index = getFeaCur.Fields.FindField("ToDate");
                    if (index < 0) { getFeature = getFeaCur.NextFeature(); continue; }
                    getFeature.set_Value(index, sNow);
                    getFeature.Store();
                    getFeature = getFeaCur.NextFeature();
                }
                Marshal.ReleaseComObject(getFeaCur);
            }
            catch (Exception eError)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(eError, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(eError, null, DateTime.Now);
                }
                //********************************************************************
                ex = eError;
                return;
            }
        }
    }
}
