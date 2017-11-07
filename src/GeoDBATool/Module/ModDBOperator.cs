using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Data.OracleClient;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;

namespace GeoDBATool
{
    public static class ModDBOperator
    {
        private static string _LogFilePath = Application.StartupPath + "\\..\\Log\\GeoDBATool.txt";
        //added by chulili 20111108 
        public static void WriteLog(string strLog)
        {
            //判断文件是否存在  不存在就创建添加写日志的函数，为了测试加载历史数据的效率
            if (!File.Exists(_LogFilePath))
            {
                System.IO.FileStream pFileStream = File.Create(_LogFilePath);
                pFileStream.Close();
            }
            //FileStream fs = File.Open(_LogFilePath,FileMode.Append);

            //StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));

            string strTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string strread = strLog + "     " + strTime + "\r\n";
            StreamWriter sw = new StreamWriter(_LogFilePath, true, Encoding.GetEncoding("gb2312"));
            sw.Write(strread);
            sw.Close();
            //fs.Close();
            sw = null;
            //fs = null;
        }
        //添加树图节点
        public static DevComponents.AdvTree.Node CreateAdvTreeNode(DevComponents.AdvTree.NodeCollection nodeCol, string strText, string strName, Image pImage, bool bExpand)
        {

            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
            node.Text = strText;
            node.Image = pImage;
            if (strName != null)
            {
                node.Name = strName;
            }

            if (bExpand == true)
            {
                node.Expand();
            }

            nodeCol.Add(node);
            return node;
        }


        //添加树图节点列

        public static DevComponents.AdvTree.Cell CreateAdvTreeCell(DevComponents.AdvTree.Node aNode, string strText, Image pImage)
        {
            DevComponents.AdvTree.Cell aCell = new DevComponents.AdvTree.Cell(strText);
            aCell.Images.Image = pImage;
            aNode.Cells.Add(aCell);

            return aCell;
        }

        //选中XML某节点若不存在则创建
        public static XmlNode SelectNode(XmlNode parentNode, string strName)
        {
            XmlNode aNode = parentNode.SelectSingleNode(".//" + strName);
            if (aNode == null)
            {
                aNode = parentNode.OwnerDocument.CreateElement(strName) as XmlNode;
            }

            parentNode.AppendChild(aNode);
            return aNode;
        }
        //得到一个工作空间中所有要素类的名称列表
        public static List<string> GetFeatureClassListOfWorkspace(IWorkspace pWorkspace)
        {
            if (pWorkspace == null)
            {
                return null;
            }
            List<string> pList = new List<string>();
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            pEnumDataset.Reset();
            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {
                pList.Add(pDataset.Name);
                pDataset = pEnumDataset.Next();
            }
            pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            pEnumDataset.Reset();
            IFeatureDataset pFeaDataset = pEnumDataset.Next() as IFeatureDataset ;
            while (pFeaDataset != null)
            {
                IFeatureClassContainer pFeatureClassContainer = pFeaDataset as IFeatureClassContainer;
                IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
                IFeatureClass pFeatureClass = pEnumFeatureClass.Next();
                while (pFeatureClass != null)
                {
                    IDataset pTmpdt = pFeatureClass as IDataset;
                    pList.Add(pTmpdt.Name);
                    pFeatureClass = pEnumFeatureClass.Next();
                }
                pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
            }
            return pList;
                     
        }
        //得到一个工作空间中所有要素类的名称列表
        public static Dictionary <string, List<string>> GetFieldsListOfWorkSpace(IWorkspace pWorkspace)
        {
            if (pWorkspace == null)
            {
                return null;
            }
            Dictionary <string, List<string>> pDic = new Dictionary <string, List<string>>();
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            pEnumDataset.Reset();
            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {
                IFeatureClass pFeatureClass = pDataset as IFeatureClass;
                IFields pFields = pFeatureClass.Fields;
                List<string> pFieldList = new List<string>();
                for (int f = 0; f < pFields.FieldCount; f++)
                {
                    IField pField = pFields.get_Field(f);
                    if (pField.Type != esriFieldType.esriFieldTypeOID && pField.Type != esriFieldType.esriFieldTypeGeometry)
                    {
                        pFieldList.Add(pField.Name);
                    }
                }
                pDic.Add(pDataset.Name, pFieldList);
                pDataset = pEnumDataset.Next();
            }
            pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            pEnumDataset.Reset();
            IFeatureDataset pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
            while (pFeaDataset != null)
            {
                IFeatureClassContainer pFeatureClassContainer = pFeaDataset as IFeatureClassContainer;
                IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
                IFeatureClass pFeatureClass = pEnumFeatureClass.Next();
                while (pFeatureClass != null)
                {
                    IDataset pTmpDataset = pFeatureClass as IDataset;
                    IFields pTmpFields = pFeatureClass.Fields;
                    List<string> pTmpFieldList = new List<string>();
                    for (int f = 0; f < pTmpFields.FieldCount; f++)
                    {
                        IField pTmpField = pTmpFields.get_Field(f);
                        if (pTmpField.Type != esriFieldType.esriFieldTypeOID && pTmpField.Type != esriFieldType.esriFieldTypeGeometry)
                        {
                            pTmpFieldList.Add(pTmpField.Name);
                        }
                    }
                    pDic.Add(pTmpDataset.Name, pTmpFieldList);
                    pFeatureClass = pEnumFeatureClass.Next();
                }
                pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
            }
            return pDic;

        }
        public static void createAnnoFeatureClass(string feaName, IFeatureDataset featureDataset, IFeatureWorkspace feaworkspace, IFieldsEdit fsEditAnno, int intScale)
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
                    fsEditAnno.AddField(pOCDesc.RequiredFields.get_Field(j));
                }
                fields = fsEditAnno as IFields;
                pFWSAnno.CreateAnnotationClass(feaName, fields, pOCDesc.InstanceCLSID, pOCDesc.ClassExtensionCLSID, pFDesc.ShapeFieldName, "", featureDataset, null, pAnnoPropsColl, pGLS, pSymbolColl, true);
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

            }
        }
        public static bool createFeatureClass(XmlDocument ruleXML, IFeatureWorkspace feaworkspace, int intScale)
        {
            try
            {
                //获取“目标库体结构节点”

                XmlNodeList tempNodeList = ruleXML.GetElementsByTagName("目标库体结构");

                //创建空间参考，通过读取xml文件来获得空间参考的路径
                XmlElement spatialElement = tempNodeList.Item(0).SelectSingleNode(".//空间参考") as XmlElement;
                string spatialPath = spatialElement.GetAttribute("路径");
                spatialPath = Application.StartupPath + "\\" + spatialPath;
                ISpatialReferenceFactory pSpaReferenceFac = new SpatialReferenceEnvironmentClass();//空间参考工厂

                ISpatialReference pSpatialReference = null;//用来获得空间参考

                if (File.Exists(spatialPath))
                {
                    pSpatialReference = pSpaReferenceFac.CreateESRISpatialReferenceFromPRJFile(spatialPath);
                }

                if (pSpatialReference == null)
                {
                    pSpatialReference = new UnknownCoordinateSystemClass();
                }

                //设置默认的Resolution
                ISpatialReferenceResolution pSpatiaprefRes = pSpatialReference as ISpatialReferenceResolution;
                pSpatiaprefRes.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon 
                pSpatiaprefRes.SetDefaultXYResolution();
                pSpatiaprefRes.SetDefaultZResolution();
                pSpatiaprefRes.SetDefaultMResolution();
                //设置默认的Tolerence
                ISpatialReferenceTolerance pSpatialrefTole = pSpatiaprefRes as ISpatialReferenceTolerance;
                pSpatialrefTole.SetDefaultXYTolerance();
                pSpatialrefTole.SetDefaultZTolerance();
                pSpatialrefTole.SetDefaultMTolerance();

                //创建数据集

                IFeatureDataset pFeatureDataset = null;//定义数据集用来装载要素类
                pFeatureDataset = feaworkspace.CreateFeatureDataset("DLG", pSpatialReference);

                #region 通过读取xml文件来获得要素类信息
                tempNodeList = tempNodeList.Item(0).SelectNodes(".//目标数据");
                XmlNode newFeatureNode = null;
                string shapestr = "Shape";

                for (int i = 0; i < tempNodeList.Count; i++)
                {
                    newFeatureNode = tempNodeList.Item(i);
                    string feaName = newFeatureNode["名称"].InnerText;
                    string feaType = newFeatureNode["类型"].InnerText;
                    XmlNodeList fieldNodeList = newFeatureNode.SelectNodes(".//字段");
                    IFields fields = new FieldsClass();
                    IFieldsEdit fsEdit = fields as IFieldsEdit;

                    for (int j = 0; j < fieldNodeList.Count; j++)
                    {
                        //根据xml文件创建用户自定义的字段
                        IField newfield = new FieldClass();
                        IFieldEdit fieldEdit = newfield as IFieldEdit;
                        XmlNode fieldNode = fieldNodeList.Item(j);
                        fieldEdit.Name_2 = fieldNode["名称"].InnerText;
                        fieldEdit.AliasName_2 = fieldNode["别名"].InnerText;
                        fieldEdit.Type_2 = (esriFieldType)Enum.Parse(typeof(esriFieldType), fieldNode["类型"].InnerText, true);
                        if (fieldNode["长度"].InnerText.Trim() != "")
                        {
                            fieldEdit.Length_2 = int.Parse(fieldNode["长度"].InnerText);
                        }
                        if (fieldNode["默认值"].InnerText.Trim() != "")
                        {
                            //======================================================
                            //chenyafei  20110104  modify :有几种特殊的字段类型的默认值要进行控制和保护,时间blob字段等
                            try
                            {
                                fieldEdit.DefaultValue_2 = fieldNode["默认值"].InnerText; //默认值
                            }
                            catch
                            {
                            }

                            //=============================================================
                        }
                        fieldEdit.IsNullable_2 = !bool.Parse(fieldNode["是否必填"].InnerText);
                        newfield = fieldEdit as IField;
                        fsEdit.AddField(newfield);
                    }

                    if (feaType == "注记")
                    {
                        //注记
                        createAnnoFeatureClass(feaName, pFeatureDataset, feaworkspace, fsEdit, intScale);
                    }
                    else
                    {
                        //featureClass的特殊字段

                        //添加Object字段
                        IField newfield2 = new FieldClass();
                        IFieldEdit fieldEdit2 = newfield2 as IFieldEdit;
                        fieldEdit2.Name_2 = "OBJECTID";
                        fieldEdit2.Type_2 = esriFieldType.esriFieldTypeOID;
                        fieldEdit2.AliasName_2 = "OBJECTID";
                        newfield2 = fieldEdit2 as IField;
                        fsEdit.AddField(newfield2);

                        //添加Geometry字段
                        IField newfield1 = new FieldClass();
                        IFieldEdit fieldEdit1 = newfield1 as IFieldEdit;
                        fieldEdit1.Name_2 = shapestr;
                        fieldEdit1.Type_2 = esriFieldType.esriFieldTypeGeometry;
                        IGeometryDef geoDef = new GeometryDefClass();
                        IGeometryDefEdit geoDefEdit = geoDef as IGeometryDefEdit;
                        if (feaType == "点")
                        {
                            geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
                        }
                        else if (feaType == "线")
                        {
                            geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;
                        }
                        else if (feaType == "面")
                        {
                            geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                        }
                        ISpatialReference pSR = pSpatialReference;
                        //ISpatialReferenceResolution pSRR = pSR as ISpatialReferenceResolution;
                        //pSRR.SetDefaultXYResolution();
                        //ISpatialReferenceTolerance pSRT = (ISpatialReferenceTolerance)pSR;
                        //pSRT.SetDefaultXYTolerance();
                        geoDefEdit.SpatialReference_2 = pSR;
                        fieldEdit1.GeometryDef_2 = geoDefEdit as GeometryDef;
                        newfield1 = fieldEdit1 as IField;
                        fsEdit.AddField(newfield1);
                        fields = fsEdit as IFields;

                        pFeatureDataset.CreateFeatureClass(feaName, fields, null, null, esriFeatureType.esriFTSimple, shapestr, "");
                    }

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
                return false;
            }
        }

        //根据XML节点获取连接信息
        public static object GetDBInfoByXMLNode(XmlElement dbElement, string strPath)
        {
            try
            {
                string strType = dbElement.GetAttribute("类型");
                string strServer = dbElement.GetAttribute("服务器");
                string strInstance = dbElement.GetAttribute("服务名");
                string strDB = dbElement.GetAttribute("数据库");
                if (strPath != "")
                {
                    strDB = strPath + strDB;
                }
                string strUser = dbElement.GetAttribute("用户");
                string strPassword = dbElement.GetAttribute("密码");
                string strVersion = dbElement.GetAttribute("版本");

                IPropertySet pPropSet = null;
                switch (strType.Trim().ToLower())
                {
                    case "pdb":
                        pPropSet = new PropertySetClass();
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        if (!File.Exists(strDB))
                        {
                            FileInfo filePDB = new FileInfo(strDB);
                            pAccessFact.Create(filePDB.DirectoryName, filePDB.Name, null, 0);
                        }
                        pPropSet.SetProperty("DATABASE", strDB);
                        IWorkspace pdbWorkspace = pAccessFact.Open(pPropSet, 0);
                        pAccessFact = null;
                        return pdbWorkspace;

                    case "gdb":
                        pPropSet = new PropertySetClass();
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        if (!Directory.Exists(strDB))
                        {
                            DirectoryInfo dirGDB = new DirectoryInfo(strDB);
                            pFileGDBFact.Create(dirGDB.Parent.FullName, dirGDB.Name, null, 0);
                        }
                        pPropSet.SetProperty("DATABASE", strDB);
                        IWorkspace gdbWorkspace = pFileGDBFact.Open(pPropSet, 0);
                        pFileGDBFact = null;
                        return gdbWorkspace;

                    case "sde":
                        pPropSet = new PropertySetClass();
                        IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
                        pPropSet.SetProperty("SERVER", strServer);
                        pPropSet.SetProperty("INSTANCE", strInstance);
                        pPropSet.SetProperty("DATABASE", strDB);
                        pPropSet.SetProperty("USER", strUser);
                        pPropSet.SetProperty("PASSWORD", strPassword);
                        pPropSet.SetProperty("VERSION", strVersion);
                        IWorkspace sdeWorkspace = pSdeFact.Open(pPropSet, 0);
                        pSdeFact = null;
                        return sdeWorkspace;

                    case "access":
                        System.Data.Common.DbConnection dbCon = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strDB);
                        dbCon.Open();
                        return dbCon;

                    case "oracle":
                        string strOracle = "Data Source=" + strDB + ";Persist Security Info=True;User ID=" + strUser + ";Password=" + strPassword + ";Unicode=True";
                        System.Data.Common.DbConnection dbConoracle = new OracleConnection(strOracle);
                        dbConoracle.Open();
                        return dbConoracle;

                    case "sql":
                        string strSql = "Data Source=" + strDB + ";Initial Catalog=" + strInstance + ";User ID=" + strUser + ";Password=" + strPassword;
                        System.Data.Common.DbConnection dbConsql = new SqlConnection(strSql);
                        dbConsql.Open();
                        return dbConsql;

                    default:
                        break;
                }

                return null;
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
        /// 从坐标字符串得到范围Polygon
        /// </summary>
        /// <param name="strCoor">坐标字符串,格式为X@Y,以逗号分割</param>
        /// <returns></returns>
        public static IPolygon GetPolygonByCol(string strCoor)
        {
            try
            {
                object after = Type.Missing;
                object before = Type.Missing;
                IPolygon polygon = new PolygonClass();
                IPointCollection pPointCol = (IPointCollection)polygon;
                string[] strTemp = strCoor.Split(',');
                for (int index = 0; index < strTemp.Length; index++)
                {
                    string CoorLine = strTemp[index];
                    string[] coors = CoorLine.Split('@');

                    double X = Convert.ToDouble(coors[0]);
                    double Y = Convert.ToDouble(coors[1]);

                    IPoint pPoint = new PointClass();
                    pPoint.PutCoords(X, Y);
                    pPointCol.AddPoint(pPoint, ref before, ref after);
                }

                polygon = (IPolygon)pPointCol;
                polygon.Close();

                ITopologicalOperator pTopo = (ITopologicalOperator)polygon;
                pTopo.Simplify();

                return polygon;
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


        public static bool NewFeatures(IFeatureClass ObjFeatureCls, IFeatureCursor pfeacursor, Dictionary<string, string> dicFieldsPair, Dictionary<string, object> values, bool useOrgFdVal, bool bIngore, DevComponents.DotNetBar.Controls.ProgressBarX progressBar, out Exception eError)
        {
            eError = null;
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
        /// 根据图层组名获取组图层

        /// </summary>
        /// <param name="pMapcontrol"></param>
        /// <param name="strName">图层组名称</param>
        /// <returns></returns>
        public static IGroupLayer GetGroupLayer(IMapControlDefault pMapcontrol, string strName)
        {
            IGroupLayer pGroupLayer = null;
            for (int i = 0; i < pMapcontrol.Map.LayerCount; i++)
            {
                ILayer pLayer = pMapcontrol.Map.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    if (pLayer.Name == strName)
                    {
                        pGroupLayer = pLayer as IGroupLayer;
                        break;
                    }
                }
            }
            return pGroupLayer;
        }
        //cyf 20110625 add:获取面板上指定图层名的图层
        public static ILayer GetLayer(IMapControlDefault pMapcontrol, string strName)
        {
            ILayer pLayer = null;
            for (int i = 0; i < pMapcontrol.Map.LayerCount; i++)
            {
                ILayer mLayer = pMapcontrol.Map.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    ICompositeLayer pComLayer = mLayer as ICompositeLayer;
                    for (int j = 0; j < pComLayer.Count; j++)
                    {
                        if (pComLayer.get_Layer(j).Name== strName)
                        {
                            pLayer = pComLayer.get_Layer(j);
                            break;
                        }
                    }
                }
                else
                {
                    if (mLayer.Name == strName)
                    {
                        pLayer = mLayer;
                        break;
                    }
                }
                if (pLayer != null)
                {
                    break;
                }
            }
            return pLayer;
        }


        /// <summary>
        /// 刷新 框架要素库子系统 xml文件   陈亚飞添加 20101009  陈亚飞修改20101011  cyf 20110612 modify
        /// <param name="curXml">数据库子系统工程xml</param>
        /// <param name="xmlTemp">数据库子系统工程xml模板</param>
        /// <param name="proID">数据库工程ID</param>
        /// <param name="proName">数据库工程名称</param>
        /// <param name="pScale">数据库工程比例尺</param>
        /// <param name="dbFormtID">数据库连接类型:PDB、GDB、SDE,针对ArcGIS平台</param>
        /// <param name="strConn">数据库连接信息</param>
        /// <param name="eError"></param>
        public static void RefeshFeaXml(string curXml, string xmlTemp, string proID, string proName, string pScale, string dbFormtID, string strConn, string strParaDB, out Exception eError)
        {
            eError = null;
            string pServer = "";       //服务器
            string pInstance = "";     //服务名
            string pDB = "";           //数据库
            string pUser = "";         //用户
            string pPassword = "";     //密码
            string pVersion = "";      //版本
            string pDtName = "";       //数据集名称

            //获得数据库连接参数
            string[] strArr = strConn.Split(new char[] { '|' });
            if (strArr.Length != 7)
            {
                eError = new Exception("连接字符串设置有误！");
                return;
            }
            pServer = strArr[0];
            pInstance = strArr[1];
            pDB = strArr[2];
            pUser = strArr[3];
            pPassword = strArr[4];
            pVersion = strArr[5];
            pDtName = strArr[6];

            //加载xml文件
            if (!File.Exists(xmlTemp))
            {
                eError = new Exception("缺失模板文件,请检查!");
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(curXml))
            {
                //若工程xml文件不存在，则创建xml文件
                xmlDoc.LoadXml("<工程管理></工程管理>");
                xmlDoc.Save(curXml);
            }
            xmlDoc.Load(curXml);

            //创建工程管理节点下面的子节点 “工程”节点
            XmlElement proElement = xmlDoc.CreateElement("工程");//SelectNode(xmlDoc.DocumentElement, "工程") as XmlElement;//
            xmlDoc.DocumentElement.AppendChild(proElement as XmlNode);

            //加载工程模板xml文件
            XmlDocument xmlDocTemple = new XmlDocument();
            xmlDocTemple.Load(xmlTemp);
            //获得xml模板文件中的“工程”节点
            XmlNode nodeTemple = xmlDocTemple.SelectSingleNode(".//工程管理//工程");
            //将模板文件中的“工程”节点引入到新创建的xml文件中
            XmlNode DBXmlNodeNew = xmlDoc.ImportNode(nodeTemple, true);
            //设置工程节点的属性信息
            (DBXmlNodeNew as XmlElement).SetAttribute("编号", proID);
            (DBXmlNodeNew as XmlElement).SetAttribute("名称", proName);
            (DBXmlNodeNew as XmlElement).SetAttribute("比例尺", pScale);
            //用设置好的节点替换原有的节点
            xmlDoc.DocumentElement.ReplaceChild(DBXmlNodeNew, proElement as XmlNode);
            //cyf 20110608 modify :将栅格数据库的属性设置为FALSE
            XmlNode RasterNode = DBXmlNodeNew.SelectSingleNode(".//内容//栅格数据库");
            if (RasterNode != null)
            {
                XmlElement RasterElem = RasterNode as XmlElement;
                if (RasterElem != null)
                {
                    RasterElem.SetAttribute("是否显示", "false");
                }
            }
            //end

            //内容节点
            XmlNode contextNode = DBXmlNodeNew.FirstChild;
            #region 遍历数据库子节点集合，设置数据库的连接属性
            foreach (XmlNode subNode in contextNode.ChildNodes)
            {
                string sVisible = (subNode as XmlElement).GetAttribute("是否显示");
                if (sVisible == bool.FalseString.ToLower()) continue;

                //连接信息节点
                XmlElement subElement = subNode.FirstChild as XmlElement;

                //设置连接信息属性
                if (dbFormtID == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
                {
                    subElement.SetAttribute("类型", "PDB");
                    subElement.SetAttribute("数据库", pDB);

                }
                else if (dbFormtID == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
                {
                    subElement.SetAttribute("类型", "GDB");
                    subElement.SetAttribute("数据库", pDB);
                }
                else if (dbFormtID == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                {
                    subElement.SetAttribute("类型", "SDE");
                    subElement.SetAttribute("服务器", pServer);
                    subElement.SetAttribute("服务名", pInstance);
                    subElement.SetAttribute("数据库", pDB);
                    subElement.SetAttribute("用户", pUser);
                    subElement.SetAttribute("密码", pPassword);
                    subElement.SetAttribute("版本", pVersion);
                }
                else if (dbFormtID == enumInterDBFormat.GEOSTARACCESS.GetHashCode().ToString())
                {
                    subElement.SetAttribute("类型", "Access");
                    subElement.SetAttribute("数据库", pDB);
                }
                else if (dbFormtID == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString())
                {
                    subElement.SetAttribute("类型", "Oracle");
                    //subElement.SetAttribute("服务器", pPassword);
                    subElement.SetAttribute("数据库", pDB);
                    subElement.SetAttribute("用户", pUser);
                    subElement.SetAttribute("密码", pPassword);
                }
                else if (dbFormtID == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString())
                {
                    subElement.SetAttribute("类型", "SQL Server");
                    subElement.SetAttribute("服务器", pServer);
                    subElement.SetAttribute("数据库", pDB);
                    subElement.SetAttribute("用户", pUser);
                    subElement.SetAttribute("密码", pPassword);
                }
                else if (dbFormtID == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString())
                {
                    subElement.SetAttribute("服务器", pServer);
                    subElement.SetAttribute("用户", pUser);
                    subElement.SetAttribute("密码", pPassword);
                }


                //库体子节点
                XmlElement dbElem = subElement.FirstChild as XmlElement;
                //设置数据集名称
                dbElem.SetAttribute("名称", pDtName);

            }

            #endregion

            //设置图幅工作库  范围信息节点
            XmlElement rangeNode = contextNode.SelectSingleNode(".//图幅工作库//范围信息") as XmlElement;
            rangeNode.SetAttribute("范围", strParaDB);

            //保存设置
            xmlDoc.Save(curXml);
        }

        /// <summary>
        /// 刷新 高程数据库子系统和影像数据库子系统 xml文件   陈亚飞添加 20101012  cyf 20110612 modify
        /// <param name="curXml">数据库子系统工程xml</param>
        /// <param name="xmlTemp">数据库子系统工程xml模板</param>
        /// <param name="proID">数据库工程ID</param>
        /// <param name="proName">数据库工程名称</param>
        /// <param name="pScale">数据库工程比例尺</param>
        /// <param name="dbFormtID">数据库连接类型:PDB、GDB、SDE,针对ArcGIS平台</param>
        /// <param name="strConn">数据库连接信息</param>
        /// <param name="strParaDB">数据库参数信息</param>
        /// <param name="eError"></param>
        public static void RefeshRasterXml(string curXml, string xmlTemp, string proID, string proName, string pScale, string dbFormtID, string strConn, string strParaDB, out Exception eError)
        {
            eError = null;
            string pServer = "";       //服务器
            string pInstance = "";     //服务名
            string pDB = "";           //数据库
            string pUser = "";         //用户
            string pPassword = "";     //密码
            string pVersion = "";      //版本
            string pDtName = "";       //数据集名称

            string dbtypeStr = "";        //存储类型
            string pResampleStr = "";       //重采样方法
            string pCompressionStr = "";  //压缩类型
            string pPyramidStr = "";      //金字塔
            string pTileHStr = "";        //瓦片高度
            string pTileWStr = "";        //瓦片宽度
            string pBandStr = "";         //波段


            #region 获得数据库连接参数

            string[] strArr = strConn.Split(new char[] { '|' });
            if (strArr.Length != 7)
            {
                eError = new Exception("连接字符串设置有误！");
                return;
            }
            pServer = strArr[0];
            pInstance = strArr[1];
            pDB = strArr[2];
            pUser = strArr[3];
            pPassword = strArr[4];
            pVersion = strArr[5];
            pDtName = strArr[6];

            #endregion

            #region 获得栅格数据库参数
            if (strParaDB != "")
            {
                string[] paraArr = strParaDB.Split(new char[] { '|' });
                if (paraArr.Length != 7)
                {
                    eError = new Exception("栅格数据库参数设置有误！");
                    return;
                }
                dbtypeStr = paraArr[0];
                pResampleStr = paraArr[1];
                pCompressionStr = paraArr[2];
                pPyramidStr = paraArr[3];
                pTileHStr = paraArr[4];
                pTileWStr = paraArr[5];
                pBandStr = paraArr[6];
            }
            #endregion

            //加载xml文件
            if (!File.Exists(xmlTemp))
            {
                eError = new Exception("缺失模板文件,请检查!");
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(curXml))
            {
                //若工程xml文件不存在，则创建xml文件
                xmlDoc.LoadXml("<工程管理></工程管理>");
                xmlDoc.Save(curXml);
            }
            xmlDoc.Load(curXml);

            //创建工程管理节点下面的子节点 “工程”节点
            XmlElement proElement = xmlDoc.CreateElement("工程");//SelectNode(xmlDoc.DocumentElement, "工程") as XmlElement;//
            xmlDoc.DocumentElement.AppendChild(proElement as XmlNode);

            //加载工程模板xml文件
            XmlDocument xmlDocTemple = new XmlDocument();
            xmlDocTemple.Load(xmlTemp);
            //获得xml模板文件中的“工程”节点
            XmlNode nodeTemple = xmlDocTemple.SelectSingleNode(".//工程管理//工程");
            //将模板文件中的“工程”节点引入到新创建的xml文件中
            XmlNode DBXmlNodeNew = xmlDoc.ImportNode(nodeTemple, true);
            //设置工程节点的属性信息
            (DBXmlNodeNew as XmlElement).SetAttribute("编号", proID);
            (DBXmlNodeNew as XmlElement).SetAttribute("名称", proName);
            (DBXmlNodeNew as XmlElement).SetAttribute("比例尺", pScale);
            //用设置好的节点替换原有的节点
            xmlDoc.DocumentElement.ReplaceChild(DBXmlNodeNew, proElement as XmlNode);
            //cyf 20110608 modify :将现势库、历史库、工作库的属性设置为FALSE
            XmlNode tempNode = DBXmlNodeNew.SelectSingleNode(".//内容//现势库");
            XmlElement tempElem = null;
            if (tempNode != null)
            {
                tempElem = tempNode as XmlElement;
                if (tempElem != null)
                {
                    tempElem.SetAttribute("是否显示", "false");
                }
            }
            //历史库设置为FALSE
            tempNode = DBXmlNodeNew.SelectSingleNode(".//内容//历史库");
            if (tempNode != null)
            {
                tempElem = tempNode as XmlElement;
                if (tempElem != null)
                {
                    tempElem.SetAttribute("是否显示", "false");
                }
            }
            //工作库设置为false
            tempNode = DBXmlNodeNew.SelectSingleNode(".//内容//工作库");
            if (tempNode != null)
            {
                tempElem = tempNode as XmlElement;
                if (tempElem != null)
                {
                    tempElem.SetAttribute("是否显示", "false");
                }
            }
            //end

            XmlNode contextNode = DBXmlNodeNew.FirstChild;
            //遍历数据库子节点集合，设置数据库的连接属性
            foreach (XmlNode subNode in contextNode.ChildNodes)
            {
                string sVisible = (subNode as XmlElement).GetAttribute("是否显示");
                if (sVisible == bool.FalseString.ToLower()) continue;

                //设置数据库节点的“存储类型”属性
                (subNode as XmlElement).SetAttribute("存储类型", dbtypeStr);

                //连接信息节点
                XmlElement subElement = subNode.FirstChild as XmlElement;
                //设置连接信息属性
                if (dbFormtID == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
                {
                    subElement.SetAttribute("类型", "PDB");
                    subElement.SetAttribute("数据库", pDB);

                }
                else if (dbFormtID == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
                {
                    subElement.SetAttribute("类型", "GDB");
                    subElement.SetAttribute("数据库", pDB);
                }
                else if (dbFormtID == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                {
                    subElement.SetAttribute("类型", "SDE");
                    subElement.SetAttribute("服务器", pServer);
                    subElement.SetAttribute("服务名", pInstance);
                    subElement.SetAttribute("数据库", pDB);
                    subElement.SetAttribute("用户", pUser);
                    subElement.SetAttribute("密码", pPassword);
                    subElement.SetAttribute("版本", pVersion);
                }

                //库体子节点
                XmlElement dbElem = subElement.FirstChild as XmlElement;
                //设置数据集名称
                dbElem.SetAttribute("名称", pDtName);

                //栅格数据参数节点
                XmlElement rasterParaEle = subNode.SelectSingleNode(".//栅格参数设置") as XmlElement;
                //设置栅格数据参数
                rasterParaEle.SetAttribute("重采样类型", pResampleStr);
                rasterParaEle.SetAttribute("压缩类型", pCompressionStr);
                rasterParaEle.SetAttribute("金字塔", pPyramidStr);
                rasterParaEle.SetAttribute("瓦片高度", pTileHStr);
                rasterParaEle.SetAttribute("瓦片宽度", pTileWStr);
                rasterParaEle.SetAttribute("波段", pBandStr);

                break;
            }

            //保存设置
            xmlDoc.Save(curXml);
        }

        //测试链接信息是否可用
        public static bool CanOpenConnect(SysCommon.Gis.SysGisDB vgisDb, string strType, string strServer, string strService, string strDatabase, string strUser, string strPassword, string strVersion)
        {
            bool blnOpen = false;

            Exception Err;

            if (strType.ToUpper() == "ORACLE" || strType.ToUpper() == "SQLSERVER")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, strService, strDatabase, strUser, strPassword, strVersion, out Err);
            }
            else if (strType.ToUpper() == "ACCESS")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.PDB, out Err);
            }
            else if (strType.ToUpper() == "FILE")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.GDB, out Err);
            }

            return blnOpen;

        }

        // *-------------------------------------------------------------------------------------------------------
        // *功能函数：获取自动编号的下一个值
        // *开 发 者：陈亚飞
        // *开发日期：20110617
        // *参    数：表格所在工作空间，表格名称，自动编号的字段名，异常
        // *返    回：返回该字段的下一个值
        public static long GetMaxID(IFeatureWorkspace pFeaWs, string pTableName, string pFiledName, out Exception outError)
        {
            outError = null;
            long ReturnMaxID = -1;
            //获取表格
            ITable pTable = pFeaWs.OpenTable(pTableName);
            try
            {
                //表格行数
                long count = Convert.ToInt64(pTable.RowCount(null).ToString());
                if (count == 0)
                {
                    ReturnMaxID = 1;
                }
                else
                {
                    //若表格行数部位0行，则统计表格中该字段的最大值
                    IDataStatistics pDataSta = new DataStatisticsClass();
                    pDataSta.Field = pFiledName;
                    pDataSta.Cursor = pTable.Search(null, false);
                    IStatisticsResults pStaRes = null;
                    pStaRes = pDataSta.Statistics;
                    count = (long)pStaRes.Maximum;
                    //下一个值为最大值+1
                    ReturnMaxID = count + 1;
                }
                return ReturnMaxID;
            }
            catch (Exception eError)
            {
                outError = new Exception("获取自动编号的下一个值失败！");
                return -1;
            }
        }

        // *---------------------------------------------------------------------------------------
        // *开 发 者：chenyafei
        // *开发时间：20110622
        // *功能函数：根据执行的条件查询数据库中的信息
        // *参    数：表名称（支持多表，以逗号隔开）、字段名称(支持多只段，以逗号隔开)、where字句，异常(输出)
        // *返 回 值：返回查询表格的游标
        public static ICursor GetCursor(IFeatureWorkspace pFeaWS, string tableName, string fieldName, string whereStr, out Exception outError)
        {
            outError = null;
            ICursor pCursor = null;
            IQueryDef pQueryDef = pFeaWS.CreateQueryDef();
            pQueryDef.Tables = tableName;
            pQueryDef.SubFields = fieldName;
            pQueryDef.WhereClause = whereStr;
            try
            {
                pCursor = pQueryDef.Evaluate();
                if (pCursor == null)
                {
                    outError = new Exception("查询数据库失败！");
                    return null; ;
                }
            }
            catch
            {
                outError = new Exception("查询数据库失败！");
                return null;
            }
            return pCursor;
        }

        // *----------------------------------------------------------------------------------------
        // *功能函数：读取xml文件，将节点挂接在树上，刷新树图
        // *开 发 者：陈亚飞
        // *开发日期：2011-06-25
        // *参    数：工程树图xml,工程署
        public static  void RefreshProjectTree(XmlDocument xmlDoc, DevComponents.AdvTree.AdvTree projectTree, out Exception outError)
        {
            outError = null;
            //若存在工程，则遍历工程节点，获得属性信息并挂在树节点上
            foreach (XmlNode aXmlNode in xmlDoc.DocumentElement.ChildNodes)
            {
                DevComponents.AdvTree.Node newNode = new DevComponents.AdvTree.Node();
                XmlElement aElement = aXmlNode as XmlElement;
                newNode.Name = aElement.GetAttribute("编号");
                newNode.Text = aElement.GetAttribute("名称");
                newNode.Tag = aElement;
                newNode.Image = projectTree.ImageList.Images["数据库"];
                newNode.DataKeyString = "project";
                newNode.Expanded = true;
                //newNode.CheckBoxVisible = true;
                //newNode.Checked = true;
                projectTree.Nodes.Add(newNode);

                XmlNode contextNode = aXmlNode.FirstChild;
                if (contextNode == null) continue;

                //遍历数据库子节点
                foreach (XmlNode subNode in contextNode.ChildNodes)
                {
                    XmlElement subElement = subNode as XmlElement;
                    string sVisible = subElement.GetAttribute("是否显示");
                    if (sVisible == bool.FalseString.ToLower()) continue;

                    DevComponents.AdvTree.Node newNodeTemp = new DevComponents.AdvTree.Node();
                    newNodeTemp.Name = subElement.GetAttribute("名称");
                    newNodeTemp.Text = subElement.GetAttribute("名称");
                    newNodeTemp.Tag = subElement;
                    newNodeTemp.DataKeyString = "DB";
                    newNodeTemp.Expanded = true;
                    //newNodeTemp.CheckBoxVisible = true;
                    //newNodeTemp.Checked = true;
                    if (newNodeTemp.Name == "现势库" || newNodeTemp.Name == "历史库" || newNodeTemp.Name == "临时库")
                    {
                        newNodeTemp.Image = projectTree.ImageList.Images["数据库子节点"];
                    }
                    else
                    {
                        newNodeTemp.Image = projectTree.ImageList.Images["栅格数据库"];
                    }
                    //若为现势库、历史库或者栅格数据库  cyf 20110705 将历史库下面的图层节点也挂接在树节点上
                    if (newNodeTemp.Name == "现势库" || newNodeTemp.Name == "历史库" || newNodeTemp.Name == "临时库") //cyf 20110705 modify:若没有历史库体不存在，则不显示树节点。
                    {
                        bool beExist = true;   //表示库体是否存在
                        #region 添加数据集节点
                        XmlNodeList dtNodeLst = null;           //数据集xml节点
                        try { dtNodeLst = subNode.SelectNodes(".//数据集"); } catch { }
                        if (dtNodeLst != null)
                        {
                            //遍历数据集xml节点，添加数据集树节点
                            foreach (XmlNode dtNode in dtNodeLst)
                            {
                                XmlElement dtElem = dtNode as XmlElement;
                                if (newNodeTemp.Name == "历史库" || newNodeTemp.Name == "临时库")
                                {
                                    //判断历史库是否存在
                                    beExist = BeExist(subElement, dtElem.GetAttribute("名称").Trim(), out outError);
                                }
                                if (beExist)
                                {
                                    DevComponents.AdvTree.Node subTreeNode = new DevComponents.AdvTree.Node();
                                    subTreeNode.Name = dtElem.GetAttribute("名称").Trim();
                                    subTreeNode.Text = dtElem.GetAttribute("名称");
                                    subTreeNode.Tag = dtElem;
                                    subTreeNode.DataKeyString = "FD";
                                    subTreeNode.Image = projectTree.ImageList.Images["数据集"];
                                    subTreeNode.Expanded = true;
                                    //subTreeNode.CheckBoxVisible = true;
                                    //subTreeNode.Checked = true;
                                    #region 添加图层名节点
                                    XmlNode subFCXmlNode = null;          //图层名节点
                                    try { subFCXmlNode = dtNode.SelectSingleNode(".//图层名"); } catch { }
                                    if (subFCXmlNode != null)
                                    {
                                        XmlElement FcElem = subFCXmlNode as XmlElement;
                                        string fcNames = FcElem.GetAttribute("名称").Trim();
                                        string[] FcArr = fcNames.Split(',');
                                        //遍历图层名数组
                                        for (int i = 0; i < FcArr.Length; i++)
                                        {
                                            DevComponents.AdvTree.Node subFCTreeNode = new DevComponents.AdvTree.Node();
                                            subFCTreeNode.Name = FcArr[i];
                                            subFCTreeNode.Text = FcArr[i];
                                            subFCTreeNode.Tag = FcElem;
                                            subFCTreeNode.DataKeyString = "FC";
                                            subFCTreeNode.Image = projectTree.ImageList.Images["要素类"];
                                            //subFCTreeNode.CheckBoxVisible = true;
                                            //subFCTreeNode.Checked = true;
                                            subTreeNode.Nodes.Add(subFCTreeNode);
                                        }
                                    }
                                    #endregion
                                    newNodeTemp.Nodes.Add(subTreeNode);
                                }
                            }
                        }
                        #endregion
                    }

                    else if (newNodeTemp.Name == "栅格数据库")
                    {
                        # region 添加图层子节点
                        XmlNodeList RCNodeLst = null;           //图层xml节点
                        try { RCNodeLst = subNode.SelectNodes(".//图层"); } catch { }
                        if (RCNodeLst != null)
                        {
                            //遍历图层xml节点，添加图层树节点
                            foreach (XmlNode rcNode in RCNodeLst)
                            {
                                XmlElement rcElem = rcNode as XmlElement;
                                DevComponents.AdvTree.Node subRCTreeNode = new DevComponents.AdvTree.Node();
                                subRCTreeNode.Name = rcElem.GetAttribute("存储类型").Trim();
                                subRCTreeNode.Text = rcElem.GetAttribute("名称");
                                subRCTreeNode.Tag = rcElem;
                                if (subRCTreeNode.Name == "栅格编目")
                                {
                                    subRCTreeNode.DataKeyString = "RC";
                                }
                                else if (subRCTreeNode.Name == "栅格数据集")
                                {
                                    subRCTreeNode.DataKeyString = "RD";
                                }
                                subRCTreeNode.Image = projectTree.ImageList.Images["栅格图层"];
                                //subRCTreeNode.CheckBoxVisible = true;
                                //subRCTreeNode.Checked = true;
                                newNodeTemp.Nodes.Add(subRCTreeNode);
                            }
                        }
                        #endregion
                    }
                    newNode.Nodes.Add(newNodeTemp);
                }
            }
            //刷新
            projectTree.Update();
        }

        // *----------------------------------------------------------------------------------------
        // *功能函数：判断数据库是否创建
        // *开 发 者：陈亚飞
        // *开发日期：2011-07-05
        // *参    数：数据库xml节点，数据集名称， 异常（输出）
        // *返    回：数据库中是否存在指定名称的数据集。true：存在；false：不存在
        private static bool BeExist(XmlElement dbElement, string pFeaDatasetName, out Exception outError)
        {
            outError = null;
            XmlElement pConElem = null;  //连接信息xml节点
            try { pConElem = dbElement.SelectSingleNode(".//连接信息") as XmlElement; } catch { }
            if (pConElem == null)
            {
                outError = new Exception("获取连接信息节点失败！");
                return false;
            }
            string pDBType = "";  //数据库类型
            string pServer = "";  //服务器
            string pInstance = "";//实例名
            string pDbName = "";  //数据库
            string pUser = "";      //用户
            string pPassword = "";  //密码
            string pVersion = ""; //版本
            pDBType = pConElem.GetAttribute("类型").Trim();
            pServer = pConElem.GetAttribute("服务器").Trim();
            pInstance = pConElem.GetAttribute("服务名").Trim();
            pDbName = pConElem.GetAttribute("数据库").Trim();
            pUser = pConElem.GetAttribute("用户").Trim();
            pPassword = pConElem.GetAttribute("密码").Trim();
            pVersion = pConElem.GetAttribute("版本").Trim();
            //连接数据库
            SysCommon.Gis.SysGisDataSet pSysDT = new SysCommon.Gis.SysGisDataSet();
            if (pDBType == "PDB")
            {
                pSysDT.SetWorkspace(pDbName, SysCommon.enumWSType.PDB, out outError);
            }
            else if (pDBType == "GDB")
            {
                pSysDT.SetWorkspace(pDbName, SysCommon.enumWSType.GDB, out outError);
            }
            else if (pDBType == "SDE")
            {
                pSysDT.SetWorkspace(pServer, pInstance, pDbName, pUser, pPassword, pVersion, out outError);
            }
            if (outError != null)
            {
                outError = new Exception("连接空间数据库失败！");
                return false;
            }
            IWorkspace2 pWs2 = pSysDT.WorkSpace as IWorkspace2;
            if (pWs2 == null) return false;
            //存在，则返回true，不存在，则返回 false
            return (pWs2.get_NameExists(esriDatasetType.esriDTFeatureDataset, pFeaDatasetName));
        }

        # region 更新对比列表分页显示

        /// <summary>
        /// 更新对比列表分业务显示，加载页面记录  陈亚飞编写

        /// </summary>
        /// <param name="DT">数据源</param>
        /// <param name="currenPage">当前页</param>
        /// <param name="recNum">上一个页面的最后一条记录</param>
        /// <returns></returns>
        public static void LoadPage(Plugin.Application.IAppGISRef m_Hook, DataTable DT, int currenPage, int recNum)
        {
            if (DT == null) return;
            int totalRec = DT.Rows.Count;//总记录数
            int pageSize = ModData.pageSize;//每页包含的记录数
            int pageCount = totalRec / pageSize;//总页数

            if (totalRec % pageSize > 0)
            {
                pageCount += 1;
            }
            if (pageCount == 0)
            {
                m_Hook.TxtDisplayPage.Enabled = false;
                m_Hook.BtnLast.Enabled = false;
                m_Hook.BtnNext.Enabled = false;
                m_Hook.BtnFirst.Enabled = false;
                m_Hook.BtnPre.Enabled = false;
            }
            ModData.TotalPageCount = pageCount;

            int starRec;
            int endRec;
            DataTable tempDT = DT.Clone();
            if (currenPage == pageCount)
            {
                endRec = totalRec;
            }
            else
            {
                endRec = pageSize * currenPage;
            }
            starRec = recNum;
            for (int i = starRec; i < endRec; i++)
            {
                tempDT.ImportRow(DT.Rows[i]);
                recNum += 1;
            }

            //清空更新对比列表
            if (m_Hook.UpdateGrid.DataSource != null)
            {
                m_Hook.UpdateGrid.DataSource = null;
            }
            //将表格绑定到DataGrid上

            m_Hook.UpdateGrid.DataSource = tempDT;
            m_Hook.UpdateGrid.Visible = true;
            m_Hook.UpdateGrid.ReadOnly = true;
            for (int j = 0; j < m_Hook.UpdateGrid.Columns.Count; j++)
            {
                m_Hook.UpdateGrid.Columns[j].Width = (m_Hook.UpdateGrid.Width - 20) / 7;
            }
            m_Hook.UpdateGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            m_Hook.UpdateGrid.RowHeadersWidth = 20;
            m_Hook.UpdateGrid.Refresh();
            //分页文本框信息显示

            m_Hook.TxtDisplayPage.ControlText = currenPage.ToString() + "/" + pageCount.ToString();

            //设置按是否可用

            if (currenPage == pageCount)
            {
                //若为最后一页，则"最后一页"按钮和“下一页”按钮都不可用

                m_Hook.BtnLast.Enabled = false;
                m_Hook.BtnNext.Enabled = false;
            }
            else
            {
                m_Hook.BtnLast.Enabled = true;
                m_Hook.BtnNext.Enabled = true;
            }
            if (currenPage == 1)
            {
                //若为第一页，则该按钮不可用

                m_Hook.BtnFirst.Enabled = false;
                m_Hook.BtnPre.Enabled = false;
            }
            else
            {
                m_Hook.BtnFirst.Enabled = true;
                m_Hook.BtnPre.Enabled = true;
            }
        }
        #endregion

        /// <summary>
        /// 缩放到Feature
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="pFeature"></param>
        public static void ZoomToFeature(IMap pMap, IGeometry pFeature)
        {
            if (pFeature == null) return;
            IEnvelope pEnvelope = null;
            IActiveView pActiveView = pMap as IActiveView;
            if (pFeature.GeometryType == esriGeometryType.esriGeometryPoint)
            {

                pEnvelope = GetPointEnvelope(pActiveView.Extent, (pFeature as IPoint).X, (pFeature as IPoint).Y, 1);
            }
            else
            {
                pEnvelope = pFeature.Envelope;
                if (pEnvelope == null) return;
                if (pEnvelope.XMax == pEnvelope.XMin && pEnvelope.YMax == pEnvelope.YMin)
                {
                    pEnvelope = GetPointEnvelope(pActiveView.Extent, pEnvelope.XMax, pEnvelope.YMax, 1);
                }
                else
                {
                    pEnvelope.Expand(2, 2, true);
                }
            }
            pActiveView.Extent = pEnvelope;
            pActiveView.Refresh();
        }
        /// <summary>
        /// 获得数据集下所有的要素类
        /// </summary>
        /// <param name="pFeatureDataset">数据集</param>
        /// <returns></returns>
        public static List<IDataset> GetAllFeaCls(IFeatureDataset pFeatureDataset)
        {
            List<IDataset> LstDT = new List<IDataset>();

            IEnumDataset pEnumDt = pFeatureDataset.Subsets;
            pEnumDt.Reset();
            IDataset pDataset = pEnumDt.Next();
            while (pDataset != null)
            {
                LstDT.Add(pDataset);
                pDataset = pEnumDt.Next();
            }
            return LstDT;
        }
           /// <summary>
        /// 调整矩形框的大小，中心点不变，矩形框放大或缩小，仅作二维矩形
        /// </summary>
        /// <param name="pEnve"></param>
        /// <param name="dSize">j矩形框的大大小</param>
        public static IEnvelope GetPointEnvelope(IEnvelope pEnve, double douX, double douY, double dSize)
        {
            IEnvelope pEnvelope = pEnve;
            //排错
            if (pEnve == null)
                return pEnvelope;
            if (dSize <= 0)
                return pEnvelope;

            //求偏移量
            double dblXoff = douX - (pEnve.XMax + pEnve.XMin) / 2;
            double dblYoff = douY - (pEnve.YMax + pEnve.YMin) / 2;

            //平移
            pEnvelope.PutCoords(pEnvelope.XMin + dblXoff, pEnvelope.YMin + dblYoff, pEnvelope.XMax + dblXoff, pEnvelope.YMax + dblYoff);
            pEnvelope.Expand(dSize, dSize, true);

            //缩放
            pEnve.Expand(dSize, dSize, true);

            //取矩形框的高度、宽度
            //double dHight = pEnve.Height;
            //double dWidth = pEnve.Width;
            ////取矩形框的最小最大X Y
            //double dxmin = douX - (dWidth / 2);
            //double dxmax = douX + (dWidth / 2);
            //double dymin = douY - (dHight / 2);
            //double dymax = douY + (dHight / 2);
            ////对矩形框进行缩放
            //pEnvelope.XMin = dxmin - ((dSize - 1) / 2) * dWidth;
            //pEnvelope.XMax = dxmax + ((dSize - 1) / 2) * dWidth;
            //pEnvelope.YMin = dymin - ((dSize - 1) / 2) * dHight;
            //pEnvelope.YMax = dymax + ((dSize - 1) / 2) * dHight;
            return pEnvelope;
        }
    
    }
}
