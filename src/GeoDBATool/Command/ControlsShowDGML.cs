using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Runtime;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;

namespace GeoDBATool
{
    /// <summary>
    /// 陈亚飞添加
    /// </summary>
    public class ControlsShowDGML : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        Plugin.Application.IAppFormRef v_AppForm;
        private Dictionary<string, List<string>> DicRequiedFIeld = new Dictionary<string, List<string>>();//记录注记的用户自定义的字段

        public ControlsShowDGML()
        {
            base._Name = "GeoDBATool.ControlsShowDGML";
            base._Caption = "查看DGML数据";
            base._Tooltip = "查看DGML数据";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "查看DGML数据";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
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
            string[] pDocPath = null;   //保存DGML文档路径
            ///选择DGML文档
            pDocPath = SelectDocument("xml");
            if (pDocPath == null || pDocPath.Length == 0) return;
            //读取DGML文档，将文档中的要素内容，写入临时工作空间中
            if (!ImportContnent(pDocPath))
            {
                return;
            }
            //更新对比列表显示
            ShowUpdateGrid(pDocPath);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppGISRef;
            v_AppForm = m_Hook as Plugin.Application.IAppFormRef;
        }

        #region 函数 将DGML文件数据在ArcMap上显示出来

        /// <summary>
        /// 根据DGML的内容导入要素要临时空间中    陈亚飞编写    20091225 修改
        /// </summary>
        /// <param name="pDocPath">文档路径</param>
        /// <returns>是否导入成功</returns>
        private bool ImportContnent(string[] pDocPath)
        {
            try
            {
                //读取xml文档
                XmlDocument tempDGMLxml = new XmlDocument();
                tempDGMLxml.Load(pDocPath[0]);

                //显示进度条
                ShowProgressBar(true);

                #region 创建Workspace
                Exception eError = null;
                //获得pdb的路径
                int index = pDocPath[0].LastIndexOf("\\");
                string dbPurePath = pDocPath[0].Substring(0, index);
                string dbPureName = GetPureName(pDocPath[0]);
                string dbPath = dbPurePath + "\\" + dbPureName + ".mdb";//获得与xml目录相同的pdb的路径
                //string prjFile = dbPurePath + "\\" + dbPureName + ".prj";//获得同目录下的空间参考文件

                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                }
                //创建Workspace
                IWorkspace pWorkSpace = CreateWorkspace(dbPath, "PDB", out eError);
                //IWorkspace pWorkSpace = CreateTempWorkspace(out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建工作空间出错！");
                    return false;
                }
                IFeatureWorkspace pFeatureWorkSpace = pWorkSpace as IFeatureWorkspace;
                #endregion
                //用来记录创建的图层名
                List<string> DatasetsName = new List<string>();
                for (int i = 0; i < pDocPath.Length; i++)
                {
                    XmlDocument DGMLxml = new XmlDocument();
                    DGMLxml.Load(pDocPath[i]);
                    //XmlNodeList pLogicFeaNodeList = DGMLxml.SelectNodes(".//DGML//Data//LogicFeature");//地理实体节点 

                    #region 创建库体结构
                    //根据元信息获取比例尺
                    XmlElement ProNode = DGMLxml.SelectSingleNode(".//DGML//MetaInfo//ProjectInfo") as XmlElement;
                    if (ProNode == null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "该文档与DGML结构文档不一致，请选择正确的DGML文档！");
                        return false;
                    }
                    int pScale = int.Parse(ProNode.GetAttribute("Scale").Trim());//从xml中获取比例尺  
                    //获得数据结构DataStruct节点
                    XmlNode dataStructNode = DGMLxml.SelectSingleNode(".//DGML//DataStruct");
                    //创建库体结构
                    if (!CreateDataBase(dataStructNode, pFeatureWorkSpace, pScale, DatasetsName, out eError))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        return false;
                    }
                    #endregion
                    # region 将数据导入到库体里面去
                    //获得库体下所有的要素类
                    //List<IDataset> ListFeaCls = new List<IDataset>();
                    //ListFeaCls = GetAllFeatureClass(pWorkSpace);
                    //获得数据Data节点
                    XmlNode dataNode = DGMLxml.SelectSingleNode(".//DGML//Data");
                    if (!ImportAllData(dataNode, pWorkSpace, out eError))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        return false;
                    }
                    #endregion
                }
                ShowErrForm("提示", "操作成功!");
                ShowStatusTips("入库完成");
                //隐藏进度条
                ShowProgressBar(false);
                #region 将要素类图层分组加载到arcMap里面
                //定义oldData的IGroupLayer
                IGroupLayer pOldGroupLayer = new GroupLayerClass();
                pOldGroupLayer.Name = "OldData";
                //定义newData的IGroupLayer
                IGroupLayer pNewGroupLayer = new GroupLayerClass();
                pNewGroupLayer.Name = "NewData";
                //获得库体下所有的要素类
                List<IDataset> ListFeaCls = new List<IDataset>();
                ListFeaCls = GetAllFeatureClass(pWorkSpace);
                foreach (IDataset pdatast in ListFeaCls)
                {
                    try
                    {
                        IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                        //获取层名
                        string feaName = pdatast.Name;
                        IFeatureClass pFeatureClass = pdatast as IFeatureClass;
                        if (pFeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        {
                            pFeatureLayer = new FDOGraphicsLayerClass();
                        }
                        pFeatureLayer.FeatureClass = pFeatureClass;
                        ILayer mLayer = pFeatureLayer as ILayer;
                        mLayer.Name = feaName;
                        if (feaName.Substring(feaName.Length - 2) == "_t")
                        {
                            pNewGroupLayer.Add(mLayer);
                        }
                        else
                        {
                            pOldGroupLayer.Add(mLayer);
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
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "加载图层失败！");
                        return false;
                    }
                }

                m_Hook.MapControl.Map.AddLayer(pOldGroupLayer as ILayer);
                m_Hook.MapControl.Map.AddLayer(pNewGroupLayer as ILayer);
                //对图层进行排序
                SysCommon.Gis.ModGisPub.LayersCompose(m_Hook.MapControl);
                m_Hook.TOCControl.Update();
                m_Hook.MapControl.ActiveView.Refresh();
                #endregion
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
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                //隐藏进度条
                ShowProgressBar(false);
                return false;
            }
        }

        /// <summary>
        /// 选择文档
        /// </summary>
        /// <param name="pDocPath">文档路径</param>
        /// <param name="DocType">文档类型</param>
        /// <returns>是否已经选择</returns>
        private string[] SelectDocument(string DocType)
        {
            string[] pDocPath = null;
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Title = "选择文件";
            fileDlg.Filter = "文件(*." + DocType + ")|*." + DocType;
            fileDlg.Multiselect = true;
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                pDocPath = fileDlg.FileNames;
            }
            return pDocPath;
        }

        /// <summary>
        /// 设置PDB、GDB工作区  陈亚飞添加
        /// </summary>
        /// <param name="sFilePath">文件路径</param>
        /// <param name="wstype">工作区类型</param>
        /// <returns>输出错误Exception</returns>
        private IWorkspace CreateWorkspace(string sFilePath, string strType, out Exception eError)
        {
            try
            {
                eError = null;
                IWorkspace TempWorkSpace = null;
                IWorkspaceFactory pWorkspaceFactory = null;
                if (strType.Trim().ToUpper() == "PDB")
                {
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                }
                else if (strType.Trim().ToUpper() == "GDB")
                {
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                }

                if (!File.Exists(sFilePath))
                {
                    FileInfo finfo = new FileInfo(sFilePath);
                    string outputDBPath = finfo.DirectoryName;
                    string outputDBName = finfo.Name;
                    outputDBName = outputDBName.Substring(0, outputDBName.Length - 4);
                    IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(outputDBPath, outputDBName, null, 0);
                    IName pName = (IName)pWorkspaceName;
                    TempWorkSpace = (IWorkspace)pName.Open();
                }
                else
                {
                    TempWorkSpace = pWorkspaceFactory.OpenFromFile(sFilePath, 0);
                }
                return TempWorkSpace;
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
                return null;
            }
        }
        /// <summary>
        /// 创建内存工作空间
        /// </summary>
        /// <param name="eError"></param>
        /// <returns></returns>
        private IWorkspace CreateTempWorkspace(out Exception eError)
        {
            try
            {
                eError = null;
                IWorkspace TempWorkSpace = null;
                IWorkspaceFactory pWorkspaceFactory = new InMemoryWorkspaceFactoryClass();

                IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create("", "tempWorkSpace", null, 0);
                IName pName = (IName)pWorkspaceName;
                TempWorkSpace = (IWorkspace)pName.Open();

                return TempWorkSpace;
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
                return null;
            }
        }
        #region new Function Design 20091225 陈亚飞添加
        /// <summary>
        /// 将所有的数据导入到创建的库体之中   陈亚飞  20091225  添加
        /// </summary>
        /// <param name="DataNode"></param>
        /// <param name="ListFeas"></param>
        /// <returns></returns>
        private bool ImportAllData(XmlNode DataNode, IWorkspace pWorkSpace, out Exception eError)
        {
            eError = null;
            Exception ex = null;
            XmlNodeList logicFeaNodeList = DataNode.SelectNodes(".//LogicFeature");
            foreach (XmlNode logicFeaNode in logicFeaNodeList)
            {
                XmlNodeList recordNodeList = logicFeaNode.SelectNodes(".//Record");

                //设置状态栏的值
                ShowStatusTips("地理实体" + (logicFeaNode as XmlElement).GetAttribute("Name").Trim() + "中的数据开始入库");
                //设置初始进度条
                int tempValue = 0;
                ChangeProgressBar(v_AppForm.ProgressBar, 0, recordNodeList.Count, tempValue);

                foreach (XmlNode RecordNode in recordNodeList)
                {
                    string newFCName = RecordNode.SelectSingleNode(".//NEWFCNAME").InnerText.Trim();//新图层名
                    string oldFCName = RecordNode.SelectSingleNode(".//OLDFCNAME").InnerText.Trim();//原始图层名
                    int pGOFID = int.Parse(RecordNode.SelectSingleNode(".//GOFID").InnerText.Trim());//GOFID
                    string state = RecordNode.SelectSingleNode(".//STATE").InnerText.Trim();//状态
                    if (state != "删除")
                    {
                        XmlNode newFeaNode = RecordNode.SelectSingleNode(".//NEWFEATURE");
                        if (newFCName == "" || newFeaNode == null)
                        {
                            eError = new Exception("新建或修改的要素的图层名和NEWFEATURE节点不能为空！");
                            return false;
                        }
                        if (!ImprotData(newFCName, pWorkSpace, newFeaNode, out ex))
                        {
                            eError = ex;
                            return false;
                        }
                    }
                    if (state != "新建")//非新建的数据
                    {
                        XmlNode oldFeaNode = RecordNode.SelectSingleNode(".//OLDFEATURE");
                        if (oldFCName == "" || oldFeaNode == null)
                        {
                            eError = new Exception("删除或修改的要素的图层名和OLDFEATURE节点不能为空！");
                            return false;
                        }
                        if (!ImprotData(oldFCName, pWorkSpace, oldFeaNode, out ex))
                        {
                            eError = ex;
                            return false;
                        }
                    }
                    tempValue += 1;
                    ChangeProgressBar(v_AppForm.ProgressBar, -1, -1, tempValue);
                }
            }
            return true;
        }
        /// <summary>
        /// 根据xml将数据导入到创建的库体之中  陈亚飞  2009 12 25   添加 
        /// </summary>
        /// <param name="pFeatureClsName"></param>
        /// <param name="ListFeas"></param>
        /// <param name="featureNode"></param>
        /// <returns></returns>
        private bool ImprotData(string pFeatureClsName, IWorkspace pWorkSpace, XmlNode featureNode, out Exception eError)
        {
            //bool bb = false;
            //foreach (IDataset pDataset in ListFeas)
            //{
            //    if (pFeatureClsName == pDataset.Name.Trim())
            //    {
            //bb = true;
            eError = null;

            try
            {
                IFeatureClass pfeatureClass = (pWorkSpace as IFeatureWorkspace).OpenFeatureClass(pFeatureClsName);
                //IEnumDataset pEnumDataST = pWorkSpace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                //IDataset pDT= pEnumDataST.Next();
                //while  (pDT != null)
                //{
                //    string s = pDT.Name;
                //    pDT = pEnumDataST.Next();
                //}

                // 创建要素
                IFeature pFeature = pfeatureClass.CreateFeature();
                XmlNodeList valueNodeList = featureNode.SelectNodes(".//Value");
                #region 遍历Feature的每一个字段节点并复赋值
                int pAOID = -1; ;
                foreach (XmlNode valueNode in valueNodeList)
                {
                    int fieldIndex = -1;
                    string fieldName = valueNode.SelectSingleNode(".//FieldName").InnerText.Trim();//字段名称
                    string fieldvalue = valueNode.SelectSingleNode(".//FieldValue").InnerText.Trim();//字段值
                    if (fieldvalue == "") continue;
                    fieldIndex = pFeature.Fields.FindField(fieldName);//字段索引
                    if (fieldIndex == -1) continue;

                    if (fieldName == "OBJECTID")
                    {
                        pAOID = int.Parse(fieldvalue);
                    }
                    IField pField = pFeature.Fields.get_Field(fieldIndex);
                    if (pField.Editable == false) continue;
                    # region 判断字段的类型并进行类型转换,并赋值
                    //普通的字段直接赋值
                    if (pField.Type != esriFieldType.esriFieldTypeGeometry && pField.Type != esriFieldType.esriFieldTypeBlob)
                    {

                        pFeature.set_Value(fieldIndex, fieldvalue as object);
                    }
                    #region 对特殊字段进行解析
                    if (pField.Type == esriFieldType.esriFieldTypeGeometry || pField.Type == esriFieldType.esriFieldTypeBlob)
                    {
                        try
                        {
                            //将字符串还原为字节
                            //char[] c = new char[] { ',' };
                            //string[] xmlstring = fieldvalue.Split(c);
                            byte[] xmlByte = Convert.FromBase64String(fieldvalue);
                            //for (int i = 0; i < xmlstring.Length; i++)
                            //{
                            //    byte b = Convert.ToByte(Convert.ToInt32(xmlstring[i]));
                            //    xmlByte[i] = b;
                            //}
                            object fieldShape = null;//用来保存解析后的值
                            //解析后的对象需根据不同的要素类类型，用不同的类来进行初始化
                            if (pfeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                            {
                                //注记有两个特殊字段需要解析
                                if (pField.Type == esriFieldType.esriFieldTypeGeometry)
                                {
                                    fieldShape = new PolygonElementClass();
                                }
                                else if (pField.Type == esriFieldType.esriFieldTypeBlob)
                                {
                                    fieldShape = new TextElementClass();
                                }
                            }
                            else if (pfeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                            {
                                fieldShape = new PointClass();
                            }
                            else if (pfeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                            {
                                fieldShape = new PolylineClass();
                            }
                            else if (pfeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                            {
                                fieldShape = new PolygonClass();
                            }

                            //解析对象，并给字段赋值
                            if (XmlDeSerializer(xmlByte, fieldShape) == true)
                            {
                                if (pField.Type == esriFieldType.esriFieldTypeGeometry)
                                {
                                    pFeature.set_Value(fieldIndex, fieldShape);
                                    //pFeature.Shape = fieldShape as IGeometry;
                                }
                                else if (pField.Type == esriFieldType.esriFieldTypeBlob)
                                {
                                    IAnnotationFeature pAnnoFeature = pFeature as IAnnotationFeature;
                                    pAnnoFeature.Annotation = fieldShape as IElement;
                                }
                            }
                            else
                            {
                                break;
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
                            eError = new Exception("特殊字段解析出错！");
                            return false;
                        }
                    }
                    #endregion
                    #endregion
                }
                //将OID的值写到数据库里面去，用一个字段保存起来
                if (pAOID == -1)
                {
                    eError = new Exception("OBJECTID字段值不正确！");
                    return false;
                }
                int fIndex = pFeature.Fields.FindField("AOID");
                if (fIndex == -1)
                {
                    eError = new Exception("字段AOID不存在！");
                    return false;
                }
                pFeature.set_Value(fIndex, pAOID as object);
                pFeature.Store();
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
                eError = ex;
                return false;
            }
            //break;
            //    }
            //}
            //if (bb == false)
            //{
            //找不到名称相同的要素类
            //return false;
            //}
            return true;
        }

        /// <summary>
        /// 根据XML创建库体结构  陈亚飞  20091225 添加
        /// </summary>
        /// <param name="dataStructNode"></param>
        /// <param name="feaworkspace"></param>
        /// <param name="intScale"></param>
        /// <param name="prjFile"></param>
        /// <returns></returns>
        private bool CreateDataBase(XmlNode dataStructNode, IFeatureWorkspace feaworkspace, int intScale, List<string> DatasetsName, out Exception eError)
        {
            //获取工作空间下的所有的图层名
            //List<string> DatasetsName = new List<string>();
            //IEnumDatasetName pEnumDatasetName = pWorkSpace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            //IDatasetName pDatasetName = pEnumDatasetName.Next();
            //while (pDatasetName != null)
            //{
            //    DatasetsName.Add(pDatasetName.Name);
            //    DatasetsName = pEnumDatasetName.Next();
            //}
            //return DatasetsName;
            eError = null;
            Exception ex = null;
            string feaClsName = "";
            XmlNodeList oldFeaClsNodeList = dataStructNode.SelectNodes(".//oldFeatureClass//FeatureClass");
            foreach (XmlNode FeaClsNode in oldFeaClsNodeList)
            {
                feaClsName = (FeaClsNode as XmlElement).GetAttribute("Name").Trim();//要素类名称
                if (!DatasetsName.Contains(feaClsName))
                {
                    DatasetsName.Add(feaClsName);
                    if (!CreateFeatureClass(FeaClsNode, feaworkspace, intScale, out ex))
                    {
                        eError = ex;
                        return false;
                    }
                }
                else
                {
                    continue;
                }
            }

            XmlNodeList newFeaClsNodeList = dataStructNode.SelectNodes(".//newFeatureClass//FeatureClass");
            foreach (XmlNode FeaClsNode in newFeaClsNodeList)
            {
                feaClsName = (FeaClsNode as XmlElement).GetAttribute("Name").Trim();//要素类名称
                if (!DatasetsName.Contains(feaClsName))
                {
                    DatasetsName.Add(feaClsName);
                    if (!CreateFeatureClass(FeaClsNode, feaworkspace, intScale, out ex))
                    {
                        eError = ex;
                        return false;
                    }
                }
                else
                {
                    continue;
                }
            }
            return true;
        }
        /// <summary>
        /// 创建图层结构，包括普通要素层和注记层  陈亚飞  20091225  添加
        /// </summary>
        /// <param name="FeaClsNode">图层节点</param>
        /// <param name="feaworkspace"></param>
        /// <param name="intScale"></param>
        /// <param name="prjFile"></param>
        /// <returns></returns>
        private bool CreateFeatureClass(XmlNode FeaClsNode, IFeatureWorkspace feaworkspace, int intScale, out Exception eError)
        {
            eError = null;
            try
            {
                string feaClsName = (FeaClsNode as XmlElement).GetAttribute("Name").Trim();//要素类名称
                string shapeType = (FeaClsNode as XmlElement).GetAttribute("ShapeType").Trim();//要素类类型：点，线，面
                XmlNodeList fieldNodeList = FeaClsNode.SelectNodes(".//Field");
                XmlNode geoNode = null;
                #region 创建字段并设置字段的属性
                IFields fields = new FieldsClass();
                IFieldsEdit fsEdit = fields as IFieldsEdit;
                #region 创建用户自定义的字段
                foreach (XmlNode fieldNode in fieldNodeList)
                {
                    //bool annofield = false;//在判断注记的必要字段时，作为标志跳出循环
                    if (fieldNode["Name"].InnerText.ToLower() == "shape")
                    {
                        geoNode = fieldNode.SelectSingleNode(".//GeometryDef");
                        if (geoNode == null)
                        {
                            eError = new Exception("GeometryDef节点不存在！");
                            return false;
                        }
                    }
                    if (shapeType == "点")
                    {
                        if (fieldNode["Name"].InnerText == "OBJECTID" || fieldNode["Name"].InnerText.ToLower() == "shape")
                        {
                            continue;
                        }
                    }
                    if (shapeType == "线")
                    {
                        if (fieldNode["Name"].InnerText == "OBJECTID" || fieldNode["Name"].InnerText.ToLower() == "shape" || fieldNode["Name"].InnerText.ToLower() == "shape_length")
                        {
                            continue;
                        }
                    }
                    if (shapeType == "面")
                    {
                        if (fieldNode["Name"].InnerText == "OBJECTID" || fieldNode["Name"].InnerText.ToLower() == "shape" || fieldNode["Name"].InnerText.ToLower() == "shape_length" || fieldNode["Name"].InnerText.ToLower() == "shape_area")
                        {
                            continue;
                        }
                    }
                    if (shapeType == "注记")//&& fieldNode["Required"].InnerText.ToLower()=="true")
                    {
                        //continue;
                        bool annofield = false;//在判断注记的必要字段时，作为标志跳出循环
                        //if (fieldNode["Required"].InnerText.ToLower() == "false")
                        //{

                        ////判断如果该字段是注记的必要字段，不增加
                        IObjectClassDescription pOCDesc = new AnnotationFeatureClassDescription();
                        IFields pfields = pOCDesc.RequiredFields;
                        for (int k = 0; k < pfields.FieldCount; k++)
                        {

                            if (pfields.get_Field(k).Name.ToLower() == fieldNode["Name"].InnerText.ToLower())
                            {
                                annofield = true;
                                break;
                            }
                        }
                        if (annofield == true)
                        {
                            continue;
                        }
                        //if (annofield == true) continue;
                        //else
                        //{
                        //    if (!DicRequiedFIeld.ContainsKey(feaClsName))
                        //    {
                        //        List<string> temp = new List<string>();
                        //        temp.Add(fieldNode["Name"].InnerText);
                        //        DicRequiedFIeld.Add(feaClsName, temp);//保存注记的要素类名称和非必须字段名称
                        //    }
                        //    else
                        //    {
                        //        DicRequiedFIeld[feaClsName].Add(fieldNode["Name"].InnerText);
                        //    }
                        //}
                        //}
                    }
                    //以下变量用来定义字段的属性
                    string fieldName = "";//记录字段名称
                    string fieldType = "";//记录字段类型
                    int fieldLen;//记录字段长度
                    bool isNullable = true;//记录字段是否允许空值
                    int precision = 0;//精度
                    int scale = 0;
                    bool required = false;
                    bool editable = true;
                    bool domainfixed = false;
                    //获得字段的属性
                    fieldName = fieldNode["Name"].InnerText;
                    fieldType = fieldNode["Type"].InnerText;
                    isNullable = bool.Parse(fieldNode["IsNullable"].InnerText);
                    fieldLen = Convert.ToInt32(fieldNode["Length"].InnerText);
                    precision = Convert.ToInt32(fieldNode["Precision"].InnerText);
                    scale = Convert.ToInt32(fieldNode["Scale"].InnerText);
                    required = false;

                    editable = bool.Parse(fieldNode["Editable"].InnerText);
                    domainfixed = bool.Parse(fieldNode["DomainFixed"].InnerText.ToLower());

                    //创建用户自定义的字段
                    IField newfield = new FieldClass();
                    IFieldEdit fieldEdit = newfield as IFieldEdit;
                    fieldEdit.Name_2 = fieldName;
                    fieldEdit.AliasName_2 = fieldName;
                    //字段类型要装化为枚举类型
                    fieldEdit.Type_2 = (esriFieldType)Enum.Parse(typeof(esriFieldType), fieldType, true);
                    fieldEdit.IsNullable_2 = isNullable;
                    fieldEdit.Length_2 = fieldLen;
                    fieldEdit.Precision_2 = precision;
                    fieldEdit.Scale_2 = scale;
                    fieldEdit.Required_2 = required;
                    fieldEdit.Editable_2 = editable;
                    fieldEdit.DomainFixed_2 = domainfixed;
                    newfield = fieldEdit as IField;
                    fsEdit.AddField(newfield);
                }
                #endregion
                //增加一个附加OID字段，以便用来定位
                IField ppField = new FieldClass();
                IFieldEdit ppFieldEdit = ppField as IFieldEdit;
                ppFieldEdit.Name_2 = "AOID";//字段名称
                ppFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                ppFieldEdit.Length_2 = 4;
                ppFieldEdit.Editable_2 = true;
                ppField = ppFieldEdit as IField;
                fsEdit.AddField(ppField);

                if (shapeType == "注记")
                {
                    //创建注记层的特殊字段
                    if (!createAnnoFeatureClass(feaClsName, feaworkspace, fsEdit, intScale, shapeType, geoNode))
                    {
                        eError = new Exception("创建注记层失败！");
                        return false;
                    }
                }
                else
                {
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

                    //添加Geometry字段
                    IField newfield1 = new FieldClass();
                    newfield1 = GetGeometryField(newfield1, shapeType, geoNode);
                    if (newfield1 == null)
                    {
                        eError = new Exception("几何字段解析出错！");
                        return false;
                    }
                    fsEdit.AddField(newfield1);
                    fields = fsEdit as IFields;

                    #endregion
                    feaworkspace.CreateFeatureClass(feaClsName, fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                }
                #endregion
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
                eError = new Exception("创建图层出错！");
                return false;
            }
        }

        //根据规则XML中库体结构节点创建库体(注记层)   陈亚飞  编写
        private bool createAnnoFeatureClass(string feaName, IFeatureWorkspace feaworkspace, IFieldsEdit fsEditAnno, int intScale, string shapeType, XmlNode geoNode)
        {
            //创建注记的特殊字段
            try
            {
                //注记的workSpace
                IFeatureWorkspaceAnno pFWSAnno = feaworkspace as IFeatureWorkspaceAnno;
                if (pFWSAnno == null)
                {
                    return false;
                }
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
                //根据xml文件，Geometry字段可能带有空间参考，因此单独添加Geometry字段
                //添加Geometry字段
                IField newfield1 = new FieldClass();
                newfield1 = GetGeometryField(newfield1, shapeType, geoNode);
                if (newfield1 == null) return false;
                fsEditAnno.AddField(newfield1);

                fields = fsEditAnno as IFields;
                pFWSAnno.CreateAnnotationClass(feaName, fields, pOCDesc.InstanceCLSID, pOCDesc.ClassExtensionCLSID, pFDesc.ShapeFieldName, "", null, null, pAnnoPropsColl, pGLS, pSymbolColl, true);
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
            return true;
        }

        #endregion


        /// <summary>
        /// 单独创建几何字段   陈亚飞 编写
        /// </summary>
        /// <param name="newfield1"></param>
        /// <param name="shapeType"></param>
        /// <param name="geoNode"></param>
        /// <param name="prjFile"></param>
        /// <returns></returns>
        private IField GetGeometryField(IField newfield1, string shapeType, XmlNode geoNode)
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
            XmlNode spatialNode = geoNode.SelectSingleNode(".//SpatialReference");
            string spatialType = (spatialNode as XmlElement).GetAttribute("Type");
            string spaialDes = (spatialNode as XmlElement).GetAttribute("SpatialDes").Trim();
            int spaialByteRead = int.Parse((spatialNode as XmlElement).GetAttribute("ByteRead").Trim());
            //从xml中读取节点的值
            double pXYResolution = Double.Parse(spatialNode["XYResolution"].InnerText);
            bool isHighPrecision = bool.Parse(spatialNode["HighPrecision"].InnerText);
            double xMin = Double.Parse(spatialNode["xMin"].InnerText);
            double xMax = Double.Parse(spatialNode["xMax"].InnerText);
            double yMin = Double.Parse(spatialNode["yMin"].InnerText);
            double yMax = Double.Parse(spatialNode["yMax"].InnerText);
            try
            {
                ISpatialReference pSR = null;
                //ISpatialReferenceFactory pSpatialRefFac = new SpatialReferenceEnvironmentClass();
                IESRISpatialReferenceGEN pESRISpatialGEN = null;
                if (spatialType == "UnknownCoordinateSystem")
                {
                    pSR = new UnknownCoordinateSystemClass();
                }
                else if (spatialType == "ProjectedCoordinateSystem")
                {
                    //if (!File.Exists(prjFile))
                    //{
                    //    //若空间参考文件已被删除则无法导入xml
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "空间参考文件已被删除!");
                    //    return null;
                    //}
                    //pSR = pSpatialRefFac.CreateESRISpatialReferenceFromPRJFile(prjFile);
                    pSR = new ProjectedCoordinateSystemClass();
                    pESRISpatialGEN = pSR as IESRISpatialReferenceGEN;
                    pESRISpatialGEN.ImportFromESRISpatialReference(spaialDes, out spaialByteRead);
                    pSR = pESRISpatialGEN as ISpatialReference;
                }
                else if (spatialType == "GeographicCoordinateSystem")
                {
                    //if (!File.Exists(prjFile))
                    //{
                    //    //若空间参考文件已被删除则无法导入xml
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "空间参考文件已被删除!");
                    //    return null;
                    //}
                    //pSR = pSpatialRefFac.CreateESRISpatialReferenceFromPRJFile(prjFile);
                    pSR = new GeographicCoordinateSystemClass();
                    pESRISpatialGEN = pSR as IESRISpatialReferenceGEN;
                    pESRISpatialGEN.ImportFromESRISpatialReference(spaialDes, out spaialByteRead);
                    pSR = pESRISpatialGEN as ISpatialReference;
                }
                ISpatialReferenceResolution pSRR = pSR as ISpatialReferenceResolution;
                ISpatialReferenceTolerance pSRT = (ISpatialReferenceTolerance)pSR;
                IControlPrecision2 pSpatialPrecision = (IControlPrecision2)pSR;

                pSRR.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon
                pSRR.set_XYResolution(true, pXYResolution);
                pSRT.SetDefaultXYTolerance();
                pSpatialPrecision.IsHighPrecision = isHighPrecision;

                geoDefEdit.SpatialReference_2 = pSR;
                fieldEdit1.GeometryDef_2 = geoDefEdit as GeometryDef;
                newfield1 = fieldEdit1 as IField;
                return newfield1;
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
        /// <summary>
        /// 获取工作空间下指定类型数据集  陈亚飞 添加
        /// </summary>
        /// <param name="pWorkspace">工作空间</param>
        /// <param name="aDatasetTyp">数据集类型</param>
        /// <returns></returns>
        private List<IDataset> GetDatasets(IWorkspace pWorkspace, esriDatasetType aDatasetTyp)
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
        /// 获取某一要素集合下FC  陈亚飞添加
        /// </summary>
        /// <param name="pFeaDsName">要素集IFeatureDataset</param>
        /// <returns></returns>
        private List<IDataset> GetFeatureClass(IFeatureDataset pFeaDs)
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
        /// 获取数据库下全部的FC  陈亚飞添加
        /// </summary>
        /// <returns></returns>
        private List<IDataset> GetAllFeatureClass(IWorkspace pWorkspace)
        {
            List<IDataset> listFC = new List<IDataset>();

            //得到全部游离的FC名称
            List<IDataset> LsFC = GetDatasets(pWorkspace, esriDatasetType.esriDTFeatureClass);
            if (LsFC != null)
            {
                if (LsFC.Count > 0)
                {
                    listFC.AddRange(LsFC);
                }
            }

            //得到要素集合下全部FC名称
            IEnumDataset pEnumDs = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
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
        /// 关闭工作区
        /// </summary>
        /// <returns>操作结果</returns>
        public bool CloseWorkspace(IWorkspace mWorkSpace)
        {
            if (mWorkSpace == null) return true;
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(mWorkSpace.WorkspaceFactory);
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(mWorkSpace);
            mWorkSpace = null;
            return true;
        }
        /// <summary>
        /// 获得导出的xml的纯的文件名，不带后缀
        /// </summary>
        /// <param name="xmlPath">导出的xml的完整的文件名，包括路径和后缀</param>
        /// <returns></returns>
        private string GetPureName(string xmlPath)
        {
            //获得xml路径下的文件名称
            FileInfo fileInfo = new FileInfo(xmlPath);
            string pureName = "";
            string xmlName = fileInfo.Name;
            int index2 = xmlName.LastIndexOf(".");
            pureName = xmlName.Substring(0, index2);
            return pureName;
        }

        /// <summary>
        /// 将xmlByte解析为obj
        /// </summary>
        /// <param name="xmlByte"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool XmlDeSerializer(byte[] xmlByte, object obj)
        {
            try
            {
                //判断字符串是否为空
                if (xmlByte != null)
                {
                    ESRI.ArcGIS.esriSystem.IPersistStream pStream = obj as ESRI.ArcGIS.esriSystem.IPersistStream;

                    ESRI.ArcGIS.esriSystem.IXMLStream xmlStream = new ESRI.ArcGIS.esriSystem.XMLStreamClass();

                    xmlStream.LoadFromBytes(ref xmlByte);
                    pStream.Load(xmlStream as ESRI.ArcGIS.esriSystem.IStream);

                    return true;
                }
                return false;
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
        #endregion

        #region 显示更新对比列表
        //创建管理工程下更新对比列表表格
        private DataTable CreateUpdateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("GOFID", System.Type.GetType("System.String"));
            dt.Columns.Add("工作库图层名", System.Type.GetType("System.String"));
            dt.Columns.Add("工作库OID", System.Type.GetType("System.String"));
            dt.Columns.Add("外部库图层名", System.Type.GetType("System.String"));
            dt.Columns.Add("外部库OID", System.Type.GetType("System.String"));
            dt.Columns.Add("更新状态", System.Type.GetType("System.String"));
            dt.Columns.Add("最后修改时间", System.Type.GetType("System.String"));
            return dt;
        }
        //根据DGML文档将更新对比列表显示出来，以方便定位查看
        private void ShowUpdateGrid(string[] fileNames)
        {
            DataTable tempDT = new DataTable();
            tempDT = CreateUpdateTable();
            # region 向表格里面填充值
            foreach (string fileName in fileNames)
            {
                //加载xml文档
                XmlDocument DGMLDoc = new XmlDocument();
                DGMLDoc.Load(fileName);
                XmlNodeList recordList = DGMLDoc.SelectNodes(".//Data//Record");
                foreach (XmlNode recordNode in recordList)
                {
                    DataRow newRow = tempDT.NewRow();

                    string pGOFID = recordNode.SelectSingleNode(".//GOFID").InnerText.Trim();
                    string newFCName = recordNode.SelectSingleNode(".//NEWFCNAME").InnerText.Trim();//工作库图层名
                    string OLDFCName = recordNode.SelectSingleNode(".//OLDFCNAME").InnerText.Trim();//外部库图层名
                    string pState = recordNode.SelectSingleNode(".//STATE").InnerText.Trim();//更新状态
                    string pUpdateTime = recordNode.SelectSingleNode(".//UPDATETIME").InnerText.Trim();//最后更新时间
                    string pNewOID = "";//工作库OID
                    string pOldOID = "";//外部库OID
                    XmlNodeList valueNodeList = recordNode.SelectNodes(".//NEWFEATURE//Value");
                    foreach (XmlNode valueNode in valueNodeList)
                    {
                        string pfieldName = "";
                        pfieldName = valueNode.SelectSingleNode(".//FieldName").InnerText.Trim();//字段名
                        if (pfieldName == "") return;
                        if (pfieldName == "OBJECTID")
                        {
                            pNewOID = valueNode.SelectSingleNode(".//FieldValue").InnerText.Trim();//OBJECTID值
                        }
                    }
                    XmlNodeList pValueNodeList = recordNode.SelectNodes(".//OLDFEATURE//Value");
                    foreach (XmlNode valueNode in pValueNodeList)
                    {
                        string pfieldName = "";
                        pfieldName = valueNode.SelectSingleNode(".//FieldName").InnerText.Trim();//字段名
                        if (pfieldName == "") return;
                        if (pfieldName == "OBJECTID")
                        {
                            pOldOID = valueNode.SelectSingleNode(".//FieldValue").InnerText.Trim();//OBJECTID值
                        }
                    }
                    if (pOldOID == "" && pNewOID == "") return;

                    //给行赋值
                    newRow["GOFID"] = pGOFID;
                    newRow["工作库图层名"] = newFCName;
                    newRow["工作库OID"] = pNewOID;
                    newRow["外部库图层名"] = OLDFCName;
                    newRow["外部库OID"] = pOldOID;
                    newRow["更新状态"] = pState;
                    newRow["最后修改时间"] = pUpdateTime;
                    tempDT.Rows.Add(newRow);
                }
            }
            #endregion

            //分页显示更新对比列表
            ModData.TotalTable = tempDT;

            ModDBOperator.LoadPage(m_Hook, tempDT, ModData.CurrentPage, ModData.recNum);

            ////清空更新对比列表
            //if (m_Hook.UpdateGrid.DataSource!= null)
            //{
            //    m_Hook.UpdateGrid.DataSource = null;
            //}
            ////将表格绑定到DataGrid上
            //m_Hook.UpdateGrid.DataSource = DisTable;
            //m_Hook.UpdateGrid.Visible = true;
            //m_Hook.UpdateGrid.ReadOnly = true;
            //for (int j = 0; j < m_Hook.UpdateGrid.Columns.Count; j++)
            //{
            //    m_Hook.UpdateGrid.Columns[j].Width = (m_Hook.UpdateGrid.Width - 20) / 7;
            //}
            //m_Hook.UpdateGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //m_Hook.UpdateGrid.RowHeadersWidth = 20;
            //m_Hook.UpdateGrid.Refresh();
            ////分页文本框信息显示
            //ModDBOperator.DisplayPageInfo(m_Hook.TxtDisplayPage, ModData.CurrentPage, pageCount);
        }
        # endregion

        #region 进度条显示
        //控制进度条显示
        private void ShowProgressBar(bool bVisible)
        {
            if (bVisible == true)
            {
                v_AppForm.ProgressBar.Visible = true;
            }
            else
            {
                v_AppForm.ProgressBar.Visible = false;
            }
        }
        //修改进度条
        private void ChangeProgressBar(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value)
        {
            if (min != -1)
            {
                pProgressBar.Minimum = min;
            }
            if (max != -1)
            {
                pProgressBar.Maximum = max;
            }
            pProgressBar.Value = value;
            pProgressBar.Refresh();
        }


        //改变状态栏提示内容
        private void ShowStatusTips(string strText)
        {
            v_AppForm.OperatorTips = strText;
        }
        #endregion

        #region 提示对话框
        private void ShowErrForm(string strCaption, string strText)
        {
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle(strCaption, strText);
        }
        #endregion
    }
}
