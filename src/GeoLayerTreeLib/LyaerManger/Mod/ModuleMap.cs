using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using SysCommon.Gis;
using SysCommon.Error;
using System.Windows.Forms;
using ESRI.ArcGIS.GISClient;
namespace GeoLayerTreeLib.LayerManager
{
    public static class ModuleMap
    {
        private static string _MatchConfigPath = Application.StartupPath + "\\..\\res\\xml\\MatchConfig.xml";
	 	//判断文件格式是否符合图层目录格式 added by chulili 20110729
        private static string _HideFieldConfigPath = Application.StartupPath + "\\..\\res\\xml\\MatchFieldConfig.xml";
        public static List<string> _ListHideFields =null;//隐藏字段名称的链表，设置主显字段时，避开隐藏字段
        public static void ChangeLableEngine2MapLex(IMapControlDefault pMapControl)
        {
            pMapControl.Map.AnnotationEngine =new ESRI.ArcGIS.Maplex.MaplexAnnotateMapClass() ;
        }
        public static void SetDataKey(DevComponents.AdvTree.Node pNode,XmlNode pXmlnode)
        {
            while (pNode != null && pXmlnode!=null)
            {
                pNode.DataKey = pXmlnode as object;
                pNode = pNode.Parent;
                pXmlnode = pXmlnode.ParentNode;
            }
        }
        public static void GetHideFields()
        {
            if (_ListHideFields == null)
            {
                _ListHideFields = new List<string>();
            }
            if (!File.Exists(_HideFieldConfigPath))
                return;
            if (_ListHideFields == null)
            {
                _ListHideFields = new List<string>();
            }
            //打开配置文件
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_HideFieldConfigPath);
            //查找隐藏字段配置的主节点
            string strSearch = "//HideFieldConfig";
            XmlNode pXmlnode = pXmldoc.SelectSingleNode(strSearch );
            if (pXmlnode == null)
                return;
            if (!pXmlnode.HasChildNodes)
            {
                return;
            }
            //读取隐藏的各个字段名称
            for (int i = 0; i < pXmlnode.ChildNodes.Count; i++)
            {
                XmlNode pChildNode=pXmlnode.ChildNodes[i];
                XmlElement pChildEle = pChildNode as XmlElement;
                if (pChildEle != null)
                {
                    if (pChildEle.HasAttribute("FieldName"))
                    {
                        _ListHideFields.Add(pChildEle.GetAttribute("FieldName"));
                    }
                }
            }
        }
        public static bool IsLayerTreeXmlRight(string xmlpath)
        {
            if (xmlpath.Equals(string.Empty))
                return false;
            if (!File.Exists(xmlpath))
                return false;
            //打开xml文件
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(xmlpath );
            //判断所有子节点
            if (pXmldoc.ChildNodes.Count > 0)
            {
                for (int i = 0; i < pXmldoc.ChildNodes.Count; i++)
                {
                    XmlNode pNode = pXmldoc.ChildNodes[i];
                    if (!IsLayerTreeXmlRight(pNode))//有任何一个子节点不满足条件，则返回假
                    {
                        return false;
                    }
                }
            }
            else//没有任何子节点，也不满足条件
            {
                return false;
            }
            return true;
        }
        //根据节点名称判断 该节点是否符合图层目录规范 递归调用函数added by chulili 20110729
        public static bool IsLayerTreeXmlRight(XmlNode pNode)
        {
            if (pNode == null) return false;
            if (pNode.NodeType != XmlNodeType.Element)
            {
                return true;
            }
            switch (pNode.Name)
            {
                case "Root":
                case "DIR":
                case "DataDIR":
                    break;
                case "ConfigInfo":
                case "Layer":
                    return true;//判断到Layer节点为止，不再判断子节点
                    break;
                default :
                    return false;
                    break;
            }
            //判断所有子节点，只要有一个不符合要求，则返回false
            if (pNode.ChildNodes.Count > 0)
            {
                for (int i = 0; i < pNode.ChildNodes.Count; i++)
                {
                    XmlNode pTmpnode = pNode.ChildNodes[i];
                    if (!IsLayerTreeXmlRight(pTmpnode))
                        return false;
                }
            }
            return true;
        }
        //本地向数据库保存图层目录
        public static bool SaveLayerTree(IWorkspace pWorkspace,string xmlpath)
        {
            //判断各个参数是否有效
            if (pWorkspace == null)
            {
                return false ;
            }
            if (!System.IO.File.Exists(xmlpath)) return false ;
            Exception exError = null;
            ITransactions pTransactions = null;
            //保存图层树（由本地向数据库保存）
            try
            {
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.Load(xmlpath );
                IMemoryBlobStream pBlobStream = new MemoryBlobStreamClass();
                byte[] bytes = Encoding.Default.GetBytes(pXmlDoc.OuterXml);                
                pBlobStream.ImportFromMemory(ref bytes[0], (uint)bytes.GetLength(0));

                //启动事务
                pTransactions = (ITransactions)pWorkspace;
                if (!pTransactions.InTransaction) pTransactions.StartTransaction();
                SysGisTable sysTable = new SysGisTable(pWorkspace);
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                dicData.Add("LAYERTREE", pBlobStream);
                dicData.Add("NAME", "LAYERTREE");
                //判断是更新还是添加
                //不存在则添加，已存在则更新
                if (!sysTable.ExistData("LAYERTREE_XML", "NAME='LAYERTREE'"))
                {
                    if (!sysTable.NewRow("LAYERTREE_XML", dicData, out exError))
                    {
                        ErrorHandle.ShowFrmErrorHandle("提示", "添加失败！");
                        return false ;
                    }
                }
                else
                {
                    if (!sysTable.UpdateRow("LAYERTREE_XML", "NAME='LAYERTREE'", dicData, out exError))
                    {
                        ErrorHandle.ShowFrmErrorHandle("提示", "更新失败！");
                        return false ;
                    }
                }
                //提交事务
                if (pTransactions.InTransaction) pTransactions.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                //出错则放弃提交
                if (pTransactions.InTransaction) pTransactions.AbortTransaction();
                ErrorHandle.ShowFrmErrorHandle("提示", "更新失败！");
                return false;
            }
        }

        //导入mxd符号模板，存储到工作库里面，存储的表名为Render
        public static bool ReadmxdToDataBase(string mxdpath, string password, IWorkspace pWKS,bool iscover)
        {
            //打开mxd文件
            IMapDocument pMdoc = new MapDocumentClass();
            pMdoc.Open(mxdpath, password);
            if (pMdoc == null) return false;
            int mapcnt = pMdoc.MapCount;
            //定义SysGisTable变量，用来向Render表写内容
            SysGisTable sysTable = new SysGisTable(pWKS);

            for (int i = 0; i < mapcnt; i++)
            {
                IMap pMap = pMdoc.get_Map(i);
                int layercnt = pMap.LayerCount;
                for (int j = 0; j < layercnt; j++)
                {
                    //读取mxd文件中图层的符号化信息
                    IFeatureLayer pFeaLayer = pMap.get_Layer(j) as IFeatureLayer;
                    IGeoFeatureLayer pGeoLayer = pMap.get_Layer(j) as IGeoFeatureLayer;
                    ILayer pLayer = pMap.get_Layer(j) as ILayer;
                    IFeatureLayerDefinition pLayerDefine = pMap.get_Layer(j) as IFeatureLayerDefinition;
                    IFeatureRenderer pRender = pGeoLayer.Renderer;
                    IDataset pDataset = pFeaLayer.FeatureClass as IDataset;

                    byte[] rendererValue = null;
                    string RendererType = "";
                    
                    //读取render信息，写成二进制流
                    WriteRendererTobyte(ref rendererValue, ref RendererType, pRender, "", pFeaLayer as ILayer, "");
                    
                    Dictionary<string, object> dicData = new Dictionary<string, object>();
                    IMemoryBlobStream pBlobStream = new MemoryBlobStreamClass();
                    pBlobStream.ImportFromMemory(ref rendererValue[0], (uint)rendererValue.GetLength(0));
                    //render表内容写在字典里面，稍后存储到表里面
                    dicData.Add("ID", System.Guid.NewGuid().ToString());
                    dicData.Add("Render", pBlobStream);
                    dicData.Add("Layer", pFeaLayer.Name);
                    dicData.Add("RenderType", RendererType);
                    //最大最小比例尺
                    //xisheng 20110812
                  //  dicData.Add("MaxScale", pLayer.MaximumScale.ToString());
                 //   dicData.Add("MinScale", pLayer.MinimumScale.ToString());
                    //定义显示
                    dicData.Add("DefineExpression", pLayerDefine.DefinitionExpression);
                    if (pDataset != null)
                    {
                        dicData.Add("FeatureClass", pDataset.Name);
                    }
                    Exception exError = null;

                    ITransactions pTransactions = (ITransactions)pWKS;
                    if (!pTransactions.InTransaction) pTransactions.StartTransaction();
                    //向render表添加一条新纪录，记录当前图层的render
                    bool result=false;
                    if(sysTable.ExistData("Render","Layer='"+pFeaLayer.Name +"'") && iscover)
                    {
                        dicData.Remove("ID");
                        result = sysTable.UpdateRow("Render", "Layer='" + pFeaLayer.Name + "'", dicData, out exError);
                    }
                    else
                    {
                     result= sysTable.NewRow("Render", dicData, out exError);
                    }
                    if (pTransactions.InTransaction) pTransactions.CommitTransaction();
                    if (!result) return result;
                }
            }
            return true;

        }
        public static void AutoMatchLayerConfig(IWorkspace pWks,DevComponents.AdvTree.Node pLayerNode,XmlDocument pXmldoc,bool MatchScale,bool MatchRender,bool MatchLabel,bool MatchFilter)
        {
            string DirectoryFieldname = "";
            string RenderFieldname = "";
            GetMatchConfig(out DirectoryFieldname, out RenderFieldname);//获取匹配设置
            if (pWks == null)
                return;
            if (pLayerNode == null)
                return;
            if (pXmldoc == null)
                return;

            string strTag = pLayerNode.Tag.ToString();
            string strNodename = strTag;
            if (strTag.Contains("DataDIR"))
            {
                strNodename = "DataDIR";
            }

            string strSearch = "//" + strNodename + "[@NodeKey='" + pLayerNode.Name + "']";
            XmlNode pXmlnode = pXmldoc.SelectSingleNode(strSearch);
            if (pXmlnode == null)
                return;
            if (!(pXmlnode is XmlElement))
                return;
            //获取xml节点地物类名称
            XmlElement pNodeEle = pXmlnode as XmlElement;
            string strLayerName = pNodeEle.GetAttribute("NodeText");
            string strFeaClsName = pNodeEle.GetAttribute ("Code");
            string strTrueFeaClsName = strFeaClsName;
            if (strFeaClsName.Contains("."))
            {
                strTrueFeaClsName = strFeaClsName.Substring(strFeaClsName.IndexOf(".") + 1);
            }
            //获取数据库中符号表对应内容
            Exception eError = null;
            SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(pWks );
            string strMatchName = "";
            if (DirectoryFieldname == "Layer")
            {
                strMatchName = strLayerName;
            }
            else
            {
                strMatchName = strTrueFeaClsName;
            }
            //wgf 增加判断名称是否带前缀:***_DLTB 解析成DLTB或者 **_地类图斑解析成 地类图斑  20110917
            //if (strMatchName.Contains("_"))
            //{
            //    string strNewMatchName = "";
            //    string strBufferName = "";
            //    strBufferName = strMatchName;
            //    strNewMatchName = strBufferName.Substring(strBufferName.LastIndexOf("_") + 1);
            //    strMatchName = strNewMatchName;
            //}
            //end
            object objRender = sysTable.GetFieldValue("Render", "ID", RenderFieldname+"='" + strMatchName + "'", out eError);
            object objLayer = ModuleRenderer.GetLayerConfigFromBlob(RenderFieldname+"='" + strMatchName + "'", pWks);

            if (objLayer == null && objRender==null)
                return;
            ILayer pLayer = objLayer as ILayer;
            //获取render所在子节点
            XmlNode nodeLabel = pXmlnode["AttrLabel"];
            XmlNode nodeDefine = pXmlnode["ShowDefine"];
            XmlNode nodeShow = pXmlnode["AboutShow"];
            XmlNode nodeCAD = pXmlnode["AboutCADShow"];

            string strDataType="";
            if(pNodeEle.Attributes["DataType"].Value !=null)
            {
                strDataType=pNodeEle.Attributes["DataType"].Value.ToString();
            }
            #region 设置最大、最小比例尺
            //为AboutShow节点写属性

            XmlElement eleShow = null;
            if (nodeShow == null)
            {
                eleShow = pXmldoc.CreateElement("AboutShow");
                nodeShow = pXmlnode.AppendChild(eleShow as XmlNode);
            }
            eleShow = nodeShow as XmlElement;
            if (objRender != null && MatchRender)//shduan 20110615 add判断
            {
                eleShow.SetAttribute("Renderer", objRender.ToString());

            }
            if (objLayer == null)
                return;
            if (pLayer != null && MatchScale )
            {
                //xisheng 20110812
                eleShow.SetAttribute("MaxScale", pLayer.MaximumScale.ToString());
                eleShow.SetAttribute("MinScale", pLayer.MinimumScale.ToString());

            }
            IFeatureLayerDefinition pLayerDefine = pLayer as IFeatureLayerDefinition;
            if (pLayerDefine != null && MatchFilter && strDataType == "FC")
            {
                XmlElement eleDefine = null;
                if (nodeLabel == null)
                {
                    eleDefine = pXmldoc.CreateElement("ShowDefine");
                    nodeDefine = pXmlnode.AppendChild(eleDefine as XmlNode);
                }
                eleDefine = nodeDefine as XmlElement;
                eleDefine.SetAttribute("Express", pLayerDefine.DefinitionExpression);
                if (!pLayerDefine.DefinitionExpression.Equals(string.Empty))
                {
                    eleDefine.SetAttribute("IsDefineDisplay", "Ture");
                }
                else
                {
                    eleDefine.SetAttribute("IsDefineDisplay", "False");
                }
            }

            #endregion
            if (strDataType == "FC")
            {

                //为AttrLabel节点写属性
                XmlElement eleLabel = null;
                if (nodeLabel == null)
                {
                    eleLabel = pXmldoc.CreateElement("AttrLabel");
                    nodeLabel = pXmlnode.AppendChild(eleLabel as XmlNode);
                }
                eleLabel = nodeLabel as XmlElement;
                #region 写标注相关属性

                IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
                if (pGeoFeatureLayer != null && MatchLabel )
                {
                    IAnnotateLayerPropertiesCollection pAnnotateLayerPropertiesCollection = pGeoFeatureLayer.AnnotationProperties;
                    //定义IAnnotateLayerPropertiesCollection.QueryItem方法中调用的对象
                    IAnnotateLayerProperties pAnnoLayerProperties = null;
                    IElementCollection pElementCollection = null;
                    IElementCollection pElementCollection1 = null;
                    //获取标注渲染对象
                    pAnnotateLayerPropertiesCollection.QueryItem(0, out  pAnnoLayerProperties, out pElementCollection, out pElementCollection1);
                    ILabelEngineLayerProperties pLabelEngineLayerPro = pAnnoLayerProperties as ILabelEngineLayerProperties;
                    IBasicOverposterLayerProperties4 pBasicOverposterLayerProperties = pAnnoLayerProperties as IBasicOverposterLayerProperties4;
                    ITextSymbol pTextSymbol = pLabelEngineLayerPro.Symbol;
                    if (pAnnoLayerProperties != null)
                    {
                        string strExpression = pLabelEngineLayerPro.Expression.ToString();
                        if (strExpression != "")
                        {
                            strExpression = strExpression.Substring(1, strExpression.Length - 2);
                        }

                        eleLabel.SetAttribute("IsLabel", pGeoFeatureLayer.DisplayAnnotation.ToString());
                        //shduan 20110623 修改XML节点属性名称，并区分点、线、面*****************************************
                        eleLabel.SetAttribute("Expression", strExpression);

                        eleLabel.SetAttribute("FontName", pTextSymbol.Font.Name);
                        eleLabel.SetAttribute("FontSize", pTextSymbol.Font.Size.ToString());
                        eleLabel.SetAttribute("FontUnderLine", pTextSymbol.Font.Underline.ToString());
                        eleLabel.SetAttribute("FontBold", pTextSymbol.Font.Bold.ToString());
                        eleLabel.SetAttribute("FontItalic", pTextSymbol.Font.Italic.ToString());
                        eleLabel.SetAttribute("FontBoldColor", pTextSymbol.Color.RGB.ToString());

                       //将mxd中的比例尺匹配成xml中的比例尺，后续修改 20110812 xisheng 
                        //eleLabel.SetAttribute("MaxScale", pAnnoLayerProperties.AnnotationMaximumScale.ToString());
                        //eleLabel.SetAttribute("MinScale", pAnnoLayerProperties.AnnotationMinimumScale.ToString());
                        if (pBasicOverposterLayerProperties != null)
                        {
                            switch (pBasicOverposterLayerProperties.NumLabelsOption)
                            {
                                case esriBasicNumLabelsOption.esriOneLabelPerName:
                                    eleLabel.SetAttribute("NumLabelsOption", "esriOneLabelPerName");
                                    break;
                                case esriBasicNumLabelsOption.esriOneLabelPerPart:
                                    eleLabel.SetAttribute("NumLabelsOption", "esriOneLabelPerPart");
                                    break;
                                case esriBasicNumLabelsOption.esriOneLabelPerShape:
                                    eleLabel.SetAttribute("NumLabelsOption", "esriOneLabelPerShape");
                                    break;
                                default:
                                    eleLabel.SetAttribute("NumLabelsOption", "esriOneLabelPerName");
                                    break;
                            }
                        }
                        eleLabel.SetAttribute("RotationField", "");
                        eleLabel.SetAttribute("RotationType", "");
                        string strFeatureType = "";
                        if (pNodeEle.Attributes["FeatureType"].Value != null)
                        {
                            strDataType = pNodeEle.Attributes["FeatureType"].Value.ToString();
                        }
                        if (strFeatureType == "esriGeometryPoint")
                        {
                            eleLabel.SetAttribute("PointPlacementAngles", "");
                            eleLabel.SetAttribute("PointPlacementOnTop", "true");
                            eleLabel.SetAttribute("PointPlacementMethod", "esriOnTopPoint");
                            eleLabel.SetAttribute("PointPlacementPriorities", "22122333");
                        }
                        else if (strFeatureType == "esriGeometryPolyline")
                        {
                            eleLabel.SetAttribute("PointPlacementAngles", "");
                            eleLabel.SetAttribute("PointPlacementOnTop", "true");
                            eleLabel.SetAttribute("PointPlacementMethod", "esriOnTopPoint");
                        }
                        else if (strFeatureType == "esriGeometryPolygon")
                        {
                            eleLabel.SetAttribute("PolygonPlacementMethod", "esriAlwaysHorizontal");
                            eleLabel.SetAttribute("PlaceOnlyInsidePolygon", "false");
                        }
                    }
                }

                #endregion

                //pLayerNode.DataKey = pXmlnode as object;
                ModuleMap.SetDataKey(pLayerNode, pXmlnode);
            }
        }
        //获取匹配字段信息，放到全局变量里面
        public static void GetMatchConfig(out string DirectoryField,out string RenderField)
        {
            DirectoryField = "";
            RenderField = "";
            if (System.IO.File.Exists(_MatchConfigPath))
            {
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.Load(_MatchConfigPath);
                string strSearch = "//MatchConfig";
                XmlNode pXmlnode = pXmldoc.SelectSingleNode(strSearch);
                if (pXmlnode != null)
                {
                    XmlNode pChildnode = pXmlnode.ChildNodes[0];
                    if (pChildnode != null)
                    {
                        XmlElement pChildEle = pChildnode as XmlElement;
                        if (pChildEle != null)
                        {
                            if (pChildEle.HasAttribute("DirectoryField"))
                            {//与目录中图层名还是地物类名称匹配 Layer:与图层名匹配 FeatureClass:与地物类名称匹配
                                DirectoryField = pChildEle.GetAttribute("DirectoryField");
                            }
                            if (pChildEle.HasAttribute("RenderField"))
                            {//与Render表（符号库）中哪个字段匹配Layer 或FeatureClass
                                RenderField = pChildEle.GetAttribute("RenderField");
                            }
                        }
                    }
                }
                pXmldoc = null;
            }

            if (DirectoryField.Equals("")) DirectoryField = "FeatureClass";
            if (RenderField.Equals("")) RenderField = "Layer";

        }
        public static void ReadLayerToDataBaseEx(ILayer pLayer, IWorkspace pWKS, bool iscover,SysCommon.CProgress vProg)
        {
            if (pLayer == null)
                return;
            if(pWKS==null)
                return;
            //读取render信息
            byte[] rendererValue = null;
            string RendererType = "";
            //读取mxd文件中图层的符号化信息 shduan 20110720 增加DEM分层设色符号
            string strLayerName = "";
            string strFeaClsName = "";
            //定义SysGisTable变量，用来向Render表写内容
            SysGisTable sysTable = new SysGisTable(pWKS);
            strLayerName = pLayer.Name;
            if (vProg != null)
            {
                vProg.SetProgress("导入图层‘"+strLayerName+"’");
            }
            if (pLayer is IFeatureLayer)
            {
                IFeatureLayer pFeaLayer = pLayer as IFeatureLayer;
                
                IGeoFeatureLayer pGeoLayer = pLayer as IGeoFeatureLayer;
                IFeatureRenderer pFRender = null;
                if (pGeoLayer != null)
                {
                    pFRender = pGeoLayer.Renderer;
                }

                IDataset pDataset = pFeaLayer.FeatureClass as IDataset;
                if (pDataset != null) strFeaClsName = pDataset.Name;
                //写成二进制流
                WriteRendererTobyte(ref rendererValue, ref RendererType, pFRender, "", pLayer, "");
            }
            else if (pLayer is IRasterLayer)
            {
                IRasterLayer pRL = pLayer as IRasterLayer;
                strLayerName = pRL.Name;
                IRasterRenderer pRasterRenderer = pRL.Renderer;

                //写成二进制流
                WriteRasterRendererTobyte(ref rendererValue, ref RendererType, pRasterRenderer, "", pLayer, "");
            }
            else 
            {
                return;
            }
            //读取图层设置
            IFeatureLayerDefinition pLayerDefine = pLayer as IFeatureLayerDefinition;
            byte[] LayerValue = null;
            string LayerType = "";
            //写成二进制流
            WriteLayerConfigTobyte(ref LayerValue, ref LayerType, pLayer);
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            IMemoryBlobStream pBlobStream = new MemoryBlobStreamClass();
            pBlobStream.ImportFromMemory(ref LayerValue[0], (uint)LayerValue.GetLength(0));

            IMemoryBlobStream pBlobStreamRender = null;
            if (rendererValue != null)
            {
                pBlobStreamRender = new MemoryBlobStreamClass();
                pBlobStreamRender.ImportFromMemory(ref rendererValue[0], (uint)rendererValue.GetLength(0));
            }

            //Layer内容写在字典里面，稍后存储到表里面
            dicData.Add("ID", System.Guid.NewGuid().ToString());
            dicData.Add("LayerConfig", pBlobStream);
            dicData.Add("Layer", strLayerName);
            dicData.Add("LayerType", LayerType);
            if (pBlobStreamRender != null)
            {
                dicData.Add("Render", pBlobStreamRender);
            }
            dicData.Add("RenderType", RendererType);
            //最大最小比例尺 shduan 20110717add
            //xisheng 20110812
            // dicData.Add("MaxScale", pLayer.MaximumScale.ToString());
            // dicData.Add("MinScale", pLayer.MinimumScale.ToString());
            dicData.Add("FeatureClass", strFeaClsName);
            //定义显示 shduan 20110717add
            if (pLayerDefine != null)
            {
                dicData.Add("DefineExpression", pLayerDefine.DefinitionExpression);
            }
            else
            {
                dicData.Add("DefineExpression", "");
            }
            Exception exError = null;

            ITransactions pTransactions = (ITransactions)pWKS;
            if (!pTransactions.InTransaction) pTransactions.StartTransaction();
            //向render表添加一条新纪录，记录当前图层的render
            bool result = false;
            if (sysTable.ExistData("Render", "Layer='" + strLayerName + "'") && iscover)
            {
                dicData.Remove("ID");
                result = sysTable.UpdateRow("Render", "Layer='" + strLayerName + "'", dicData, out exError);
            }
            else
             {
                result = sysTable.NewRow("Render", dicData, out exError);
            }
            if (pTransactions.InTransaction) pTransactions.CommitTransaction();
            return;
        }
        public static void ReadGroupLayerToDataBaseEx(IGroupLayer pGroupLayer,IWorkspace pWKS, bool iscover,SysCommon.CProgress vProg)
        {
            ICompositeLayer pComLayer = pGroupLayer as ICompositeLayer;
            if (pComLayer != null)
            {
                for (int i = 0; i < pComLayer.Count; i++)
                {
                    ILayer pLayer = pComLayer.get_Layer(i);
                    if (pLayer is IGroupLayer)
                    {
                        ReadGroupLayerToDataBaseEx(pLayer as IGroupLayer, pWKS, iscover, vProg);
                    }
                    else
                    {
                        ReadLayerToDataBaseEx(pLayer, pWKS, iscover, vProg);
                    }
                }
            }
        }
        //added by chulili 20110711 改为直接把layer保存为字段值，导入mxd符号模板，存储到工作库里面，存储的表名为Render
        public static bool ReadmxdToDataBaseEx(string mxdpath, string password, IWorkspace pWKS, bool iscover,SysCommon.CProgress vProgress)
        {
            try
            {
                //打开mxd文件
                IMapDocument pMdoc = new MapDocumentClass();
                pMdoc.Open(mxdpath, password);
                if (pMdoc == null) return false;
                int mapcnt = pMdoc.MapCount;
                

                for (int i = 0; i < mapcnt; i++)
                {
                    IMap pMap = pMdoc.get_Map(i);
                    int layercnt = pMap.LayerCount;
                    for (int j = 0; j < layercnt; j++)
                    {
                        //读取render信息
                        //byte[] rendererValue = null;
                        //string RendererType = "";
                        //;
                        ////读取mxd文件中图层的符号化信息 shduan 20110720 增加DEM分层设色符号
                        //string strLayerName = "";
                        //string strFeaClsName = "";
                        ILayer pLayer = pMap.get_Layer(j) as ILayer;
                        if (pLayer is IGroupLayer)
                        {
                            ReadGroupLayerToDataBaseEx(pLayer as IGroupLayer, pWKS, iscover, vProgress);
                        }
                        else
                        {
                            ReadLayerToDataBaseEx(pLayer, pWKS, iscover, vProgress);
                        }

                    }
                }
                return true;
            }
            catch 
            {
                return false;
            }
            return true;
        }
        //把图层序列化
        private static void WriteLayerConfigTobyte(ref byte[] LayerByte, ref string LayerType, ILayer pLayer)
        {
            if (pLayer is IFDOGraphicsLayer)
            {
                LayerType="FDOGraphicsLayer";
            }
            else if(pLayer is IDimensionLayer )
            {
                LayerType = "DimensionLayer";
            }
            else if (pLayer is IGdbRasterCatalogLayer)
            {
                LayerType = "GdbRasterCatalogLayer";
            }
            else if (pLayer is IRasterLayer)
            {
                LayerType = "RasterLayer";
            }
            else
            {
                LayerType = "FeatureLayer";
            }
            IPersistStream pPersistStream = pLayer as IPersistStream;


            IStream pStream = new XMLStreamClass();
            pPersistStream.Save(pStream, 0);

            IXMLStream pXMLStream = pStream as IXMLStream;
            LayerByte = pXMLStream.SaveToBytes();
        }
        // 将配置的Renderer序列化写入数据库       
        private static void WriteRendererTobyte(ref byte[] RenderByte, ref string RendererType, IFeatureRenderer pFeaRenderer, string sRgbColor, ILayer pLayer, string sKeyInfo)
        {
            if (pFeaRenderer == null) return;
            if (pLayer is IFDOGraphicsLayer && sKeyInfo != "")
            {
                RendererType = "AnnoColor";
                RenderByte = (byte[])Encoding.Default.GetBytes(sRgbColor);
            }
            else
            {
                //向redererinfo表中写入相关的renderer信息
                RendererType = "";
                if (pFeaRenderer is ISimpleRenderer)
                {
                    RendererType = "SimpleRenderer";
                }
                else if (pFeaRenderer is IUniqueValueRenderer)
                {
                    RendererType = "UniqueValueRenderer";
                }
                else if (pFeaRenderer is IClassBreaksRenderer)
                {
                    RendererType = "ClassBreaksRenderer";
                }
                else if (pFeaRenderer is IProportionalSymbolRenderer)
                {
                    RendererType = "ProportionalSymbolRenderer";
                }
                else if (pFeaRenderer is IChartRenderer)
                {
                    RendererType = "ChartRenderer";
                }

                IPersistStream pPersistStream = pFeaRenderer as IPersistStream;

                //IMemoryBlobStream pMemoryStream = new MemoryBlobStream();
                IStream pStream = new XMLStreamClass();
                pPersistStream.Save(pStream, 0);

                IXMLStream pXMLStream = pStream as IXMLStream;
                RenderByte = pXMLStream.SaveToBytes();
            }
        }
        // 将配置RasterDataset的Renderer序列化写入数据库     shduan 20110721    
        private static void WriteRasterRendererTobyte(ref byte[] RenderByte, ref string RendererType, IRasterRenderer  pRasRenderer, string sRgbColor, ILayer pLayer, string sKeyInfo)
        {
            
                //向redererinfo表中写入相关的renderer信息
                RendererType = "";
                if (pRasRenderer is IRasterClassifyColorRampRenderer)
                {
                    RendererType = "RasterClassifyColorRampRenderer";
                }
                else if (pRasRenderer is IRasterDiscreteColorRenderer)
                {
                    RendererType = "RasterDiscreteColorRenderer";
                }
                else if (pRasRenderer is IRasterRGBRenderer)
                {
                    RendererType = "RasterRGBRenderer";
                }
                else if (pRasRenderer is IRasterStretchColorRampRenderer)
                {
                    RendererType = "RasterStretchColorRampRenderer";
                }
                else if (pRasRenderer is IRasterUniqueValueRenderer)
                {
                    RendererType = "RasterUniqueValueRenderer";
                }
                //else if (pRasRenderer is IRasterColormapRenderer)
                //{
                //    RendererType = "RasterColormapRenderer";
                //}

                IPersistStream pPersistStream = pRasRenderer as IPersistStream;

                //IMemoryBlobStream pMemoryStream = new MemoryBlobStream();
                IStream pStream = new XMLStreamClass();
                pPersistStream.Save(pStream, 0);

                IXMLStream pXMLStream = pStream as IXMLStream;
                RenderByte = pXMLStream.SaveToBytes();
        }
        //初始化数据源工作空间字典
        public static void InitDBSourceDic(IWorkspace pConfigWks, IDictionary<string, IWorkspace> DicDataLibWks,IDictionary<string,List<IDataset>> DicDataset)
        {
            Exception exError;
            DicDataLibWks.Clear();
            SysCommon.Gis.SysGisTable  sysTable = new SysCommon.Gis.SysGisTable(pConfigWks);
            List<Dictionary<string, object>> lstDicData = sysTable.GetRows("DATABASEMD", "", out exError);
            if (lstDicData == null)
                return;
            if (lstDicData.Count > 0)
            {
                for (int i = 0; i < lstDicData.Count; i++)
                {
                    string connstr = "";
                    string dbsourceid="";
                    int itype=-1;
                    if (lstDicData[i]["CONNECTIONINFO"]!=null) 
                        connstr = lstDicData[i]["CONNECTIONINFO"].ToString();                    
                    if(lstDicData[i]["ID"]!=null)
                        dbsourceid= lstDicData[i]["ID"].ToString();
                    if (lstDicData[i]["DATAFORMATID"]!=null) 
                        itype = int.Parse(lstDicData[i]["DATAFORMATID"].ToString());
                    if ((!connstr.Equals("")) && (!dbsourceid.Equals("")))
                    {
                        IWorkspace pWorkspace = GetWorkSpacefromConninfo(connstr, itype);
                        DicDataLibWks.Add(dbsourceid, pWorkspace);
                        if (DicDataset != null)
                        {
                            int index6 = connstr.LastIndexOf("|");
                            string strDatasets = connstr.Substring(index6 + 1);
                            string[] strTemp = strDatasets.Split(new char[] { ',' });
                            IFeatureWorkspace pFeaWorkSpace = pWorkspace as IFeatureWorkspace;
                            if (pFeaWorkSpace != null)
                            {
                                List<IDataset> pTmpListdataset = new List<IDataset>();
                                for (int k = 0; k < strTemp.Length; k++)
                                {
                                    IDataset pTmpdataset = pFeaWorkSpace.OpenFeatureDataset(strTemp[k]) as IDataset;
                                    if (pTmpdataset != null)
                                    {
                                        pTmpListdataset.Add(pTmpdataset);
                                    }
                                }
                                DicDataset.Add(dbsourceid, pTmpListdataset);
                            }
                        }
                    }
                }
            }

        }
        //初始化数据源工作空间字典
        public static void InitDBSourceDic(IWorkspace pConfigWks, IDictionary<string, IWorkspace> DicDataLibWks)
        {
            InitDBSourceDic(pConfigWks, DicDataLibWks, null);
        }

        ////初始化图层名称数据字典（英文名映射中文名）key:英文  value:中文   ZQ 2011020  modify 移植到sysCommon
        //public static void InitLayerNameDic(IWorkspace pConfigWks, IDictionary<string, string > DicLayername)
        //{
        //    Exception exError=null;
        //    DicLayername.Clear();
        //    SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(pConfigWks);
        //    List<Dictionary<string, object>> lstDicData = sysTable.GetRows("标准图层代码表", "", out exError);
        //    if (lstDicData == null)
        //        return;
        //    try
        //    {
        //        if (lstDicData.Count > 0)
        //        {
        //            for (int i = 0; i < lstDicData.Count; i++)
        //            {
        //                string strName = "";
        //                string strAliasName = "";
        //                if (lstDicData[i]["CODE"] != null)
        //                    strName = lstDicData[i]["CODE"].ToString();
        //                if (lstDicData[i]["NAME"] != null)
        //                    strAliasName = lstDicData[i]["NAME"].ToString();
        //                //将图层名及别名添加到字典中
        //                if ((!strName.Equals("")) && (!strAliasName.Equals("")))
        //                {
        //                    if (!DicLayername.Keys.Contains(strName))
        //                    {
        //                        DicLayername.Add(strName, strAliasName);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    { }
        //}
        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        public static IWorkspace GetWorkSpacefromConninfo(string conninfostr, int type)
        {
            //added by chulili 20111109 添加保护
            if (conninfostr == "")
            {
                return null;
            }
            if (type < 0)
            {
                return null;
            }
            //end added by chulili 20111109
            int index1 = conninfostr.IndexOf("|");
            int index2 = conninfostr.IndexOf("|", index1 + 1);
            int index3 = conninfostr.IndexOf("|", index2 + 1);
            int index4 = conninfostr.IndexOf("|", index3 + 1);
            int index5 = conninfostr.IndexOf("|", index4 + 1);
            int index6 = conninfostr.IndexOf("|", index5 + 1);
            IPropertySet pPropSet = new PropertySetClass();
            IWorkspaceFactory pWSFact = null;
            string sServer = ""; string sService = ""; string sDatabase = "";
            string sUser = ""; string sPassword = ""; string strVersion = "";
            switch (type)
            {
                case 1://mdb
                    pWSFact = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 2://gdb
                    pWSFact = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 3://sde
                    pWSFact = new ESRI.ArcGIS.DataSourcesGDB.SdeWorkspaceFactoryClass();
                    sServer = conninfostr.Substring(0, index1);
                    sService = conninfostr.Substring(index1 + 1, index2 - index1 - 1);
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    sUser = conninfostr.Substring(index3 + 1, index4 - index3 - 1);
                    sPassword = conninfostr.Substring(index4 + 1, index5 - index4 - 1);
                    strVersion = conninfostr.Substring(index5 + 1, index6 - index5 - 1);
                    break;
            }

            pPropSet.SetProperty("SERVER", sServer);
            pPropSet.SetProperty("INSTANCE", sService);
            pPropSet.SetProperty("DATABASE", sDatabase);
            pPropSet.SetProperty("USER", sUser);
            pPropSet.SetProperty("PASSWORD", sPassword);
            pPropSet.SetProperty("VERSION", strVersion);
            try
            {

                IWorkspace pWorkspace = pWSFact.Open(pPropSet, 0);
                return pWorkspace;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 判断服务是否开启并找到对应的服务
        /// </summary>
        /// <param name="pHostOrUrl">服务地址</param>
        /// <param name="pServiceName">名称</param>
        /// <param name="pIsLAN">类型</param>
        /// <returns></returns>
        public static bool OpenConn(string pHostOrUrl, string pServiceName, bool bLAN)
        {
            bool bIsConn = false;
            try
            {

                IAGSServerConnectionFactory pConnF = new AGSServerConnectionFactory();
                IAGSServerConnection pAGSServerConnection = new AGSServerConnectionClass();
                IPropertySet pProSet = new PropertySet();
                if (bLAN)
                    pProSet.SetProperty("machine", pHostOrUrl);
                else
                    pProSet.SetProperty("url", pHostOrUrl);
                ///连接服务
                pAGSServerConnection = pConnF.Open(pProSet, 0);
                if(pAGSServerConnection != null)
                {
                    ///获取所有的服务名称
                    pAGSServerConnection.ServerObjectNames.Reset();
                    IAGSEnumServerObjectName pEnumServerObjectNames = pAGSServerConnection.ServerObjectNames;
                    pEnumServerObjectNames.Reset();
                    IAGSServerObjectName pServerObjectName = pEnumServerObjectNames.Next();
                    while (pServerObjectName != null)
                    {
                        ///找对应的服务
                        if (pServerObjectName.Name.ToLower() == pServiceName.ToLower())
                        {
                            return bIsConn = true;
                        }
                        pServerObjectName = pEnumServerObjectNames.Next();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pEnumServerObjectNames);
                }
                else
                {
                    return bIsConn =false;
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pAGSServerConnection);
                return bIsConn = false;
            }

            catch(Exception err)
            {
                MessageBox.Show("连接失败！请检查服务是否开启","提示！");
                return bIsConn = false; ;
            }
        }
        /// <summary>
        /// 判断服务是否开启并找到对应的服务
        /// </summary>
        /// <param name="pHostOrUrl">服务地址</param>
        /// <param name="pServiceName">名称</param>
        /// <param name="pIsLAN">类型</param>
        /// <returns></returns>
        public static bool OpenWMSConn(string pHostOrUrl, string pServiceName, bool bLAN)
        {
            bool bIsConn = false;
            try
            {

                IWMSConnectionFactory pWmsFac = new WMSConnectionFactory();
                IWMSConnection pWmsConn = null;
                IPropertySet pProSet = new PropertySetClass();
                if (bLAN)
                    pProSet.SetProperty("machine", pHostOrUrl);
                else
                    pProSet.SetProperty("url", pHostOrUrl);
                ///连接服务                
                try
                {
                    pWmsConn = pWmsFac.Open(pProSet, 0, null);
                }
                catch
                { }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pWmsFac);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pProSet);
                if (pWmsConn != null)
                {
                    ///获取所有的服务名称

                    IWMSConnectionName pWmsConnectionName = pWmsConn.FullName as IWMSConnectionName;
                    ILayerFactory pLayerFactory = new EngineWMSMapLayerFactoryClass();


                    if (pLayerFactory.get_CanCreate(pWmsConnectionName))
                    {
                        IEnumLayer pEnumLayer = pLayerFactory.Create(pWmsConnectionName);
                        pEnumLayer.Reset();
                        ILayer pLayer = pEnumLayer.Next();
                        while (pLayer != null)
                        {
                            if (pLayer is IWMSMapLayer)
                            {
                                if (pLayer.Name.ToLower() == pServiceName.ToLower())//yjl20120814 add 针对顶级服务
                                {
                                    return true;
                                }
                                IWMSMapLayer pWmsMapLayer = pLayer as IWMSMapLayer;
                                IWMSGroupLayer pWmsGroupLayer = pWmsMapLayer as IWMSGroupLayer;
                                for (int j = 0; j < pWmsGroupLayer.Count; j++)
                                {
                                    ILayer pTmpLayer= pWmsGroupLayer.get_Layer(j);
                                    if (pTmpLayer.Name.ToLower() == pServiceName.ToLower())
                                    {
                                        return bIsConn = true;
                                    }
                                    IWMSGroupLayer pTmpWmsGroupLayer = pTmpLayer as IWMSGroupLayer;
                                    if (pTmpWmsGroupLayer != null)
                                    {
                                        for (int k = 0; k < pTmpWmsGroupLayer.Count; k++)
                                        {
                                            ILayer pTmpTmplayer = pTmpWmsGroupLayer.get_Layer(k);
                                            if (pTmpTmplayer.Name.ToLower() == pServiceName.ToLower())
                                            {
                                                return bIsConn = true;
                                            }
                                        }
                                    }
                                }
                            }
                            pLayer = pEnumLayer.Next();
                        }
                    }
                }
                else
                {
                    return bIsConn = false;
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pWmsFac);
                return bIsConn = false;
            }

            catch (Exception err)
            {
                MessageBox.Show("连接失败！请检查服务是否开启", "提示！");
                return bIsConn = false; ;
            }
        }
        //pHostOrUrl   服务地址
        //pServiceName 服务名称
        public static IAGSServerObjectName GetMapServer(string pHostOrUrl, string pServiceName, bool pIsLAN)
        {


            //设置连接属性
            IPropertySet pPropertySet = new PropertySetClass();
            if (pIsLAN)
                pPropertySet.SetProperty("machine", pHostOrUrl);
            else
                pPropertySet.SetProperty("url", pHostOrUrl);

            //打开连接

            IAGSServerConnectionFactory pFactory = new AGSServerConnectionFactory();
            //Type factoryType = Type.GetTypeFromProgID(
            //    "esriGISClient.AGSServerConnectionFactory");
            //IAGSServerConnectionFactory agsFactory = (IAGSServerConnectionFactory)
            //    Activator.CreateInstance(factoryType);
            IAGSServerConnection pConnection = pFactory.Open(pPropertySet, 0);

            //Get the image server.
            IAGSEnumServerObjectName pServerObjectNames = pConnection.ServerObjectNames;
            pServerObjectNames.Reset();
            IAGSServerObjectName ServerObjectName = pServerObjectNames.Next();
            while (ServerObjectName != null)
            {
                if ((ServerObjectName.Name.ToLower() == pServiceName.ToLower()) &&
                    (ServerObjectName.Type == "MapServer"))
                {

                    break;
                }
                ServerObjectName = pServerObjectNames.Next();
            }

            //返回对象
            return ServerObjectName;
        }
        //pHostOrUrl   服务地址
        //pServiceName 服务名称
        public static ILayer GetWMSServerLayer(string pHostOrUrl, string pServiceName, bool pIsLAN, string LayerName)
        {


            IWMSConnectionFactory pWmsFac = new WMSConnectionFactory();
            IWMSConnection pWmsConn = null;
            IPropertySet pProSet = new PropertySet();
            if (pIsLAN)
                pProSet.SetProperty("machine", pHostOrUrl);
            else
                pProSet.SetProperty("url", pHostOrUrl);
            ///连接服务                

            pWmsConn = pWmsFac.Open(pProSet, 0, null);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pWmsFac);
            pWmsFac = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pProSet);
            pProSet = null;
            if (pWmsConn != null)
            {
                ///获取所有的服务名称

                IWMSConnectionName pWmsConnectionName = pWmsConn.FullName as IWMSConnectionName;
                ILayerFactory pLayerFactory = new EngineWMSMapLayerFactoryClass();


                if (pLayerFactory.get_CanCreate(pWmsConnectionName))
                {
                    IEnumLayer pEnumLayer = pLayerFactory.Create(pWmsConnectionName);
                    pEnumLayer.Reset();
                    ILayer pLayer = pEnumLayer.Next();
                    while (pLayer != null)
                    {
                        ILayer pServiceLayer = null;//yjl20120814 add 针对globe服务此两级不可访问，故构造组图层
                        ILayer pMapLayer = null;//yjl20120814 add
                        if (pLayer is IWMSMapLayer)
                        {
                            if (pLayer.Name.ToLower() == pServiceName.ToLower())//yjl20120814 add 针对顶级服务
                            {
                                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayerFactory);
                                //pLayerFactory = null;
                                pServiceLayer = new GroupLayerClass();
                                pServiceLayer.Name = LayerName;
                                //return pLayer;
                            }
                            IWMSMapLayer pWmsMapLayer = pLayer as IWMSMapLayer;
                            IWMSGroupLayer pWmsGroupLayer = pWmsMapLayer as IWMSGroupLayer;

                            for (int j = 0; j < pWmsGroupLayer.Count; j++)
                            {
                                ILayer pTmpLayer = pWmsGroupLayer.get_Layer(j);
                                if (pTmpLayer.Name.ToLower() == pServiceName.ToLower())
                                {
                                    pMapLayer = new GroupLayerClass();
                                    pMapLayer.Name = LayerName;
                                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayerFactory);
                                    //pLayerFactory = null;
                                    //return pTmpLayer;
                                }
                                ILayer pTmpMapLayer = null;
                                if (pServiceLayer != null)
                                {
                                    pTmpMapLayer = new GroupLayerClass();
                                    pTmpMapLayer.Name = pTmpLayer.Name;
                                }
                                IWMSGroupLayer pTmpWmsGroupLayer = pTmpLayer as IWMSGroupLayer;
                                if (pTmpWmsGroupLayer != null)
                                {
                                    for (int k = 0; k < pTmpWmsGroupLayer.Count; k++)
                                    {
                                        ILayer pTmpTmplayer = pTmpWmsGroupLayer.get_Layer(k);
                                        if (pTmpTmplayer.Name.ToLower() == pServiceName.ToLower())
                                        {
                                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayerFactory);
                                            pLayerFactory = null;
                                            pWmsGroupLayer.Clear();
                                            pTmpTmplayer.Visible = true;
                                            pWmsGroupLayer.InsertLayer(pTmpTmplayer, 0);
                                            //IGroupLayer pGroupLayer = new GroupLayerClass();
                                            //pGroupLayer.Name = LayerName;
                                            //pGroupLayer.Add(pTmpTmplayer);
                                            ILayer pReturnLayer = pWmsGroupLayer as ILayer;
                                            pReturnLayer.Name = LayerName;
                                            return pReturnLayer as ILayer;
                                        }
                                        if (pTmpMapLayer != null)
                                        {
                                            IObjectCopy pOC = new ObjectCopyClass();
                                            IWMSGroupLayer pWGL = pOC.Copy(pLayer) as IWMSGroupLayer;
                                            pWGL.Clear();
                                            (pWGL as ILayer).Name = pTmpTmplayer.Name;
                                            pWGL.Add(pTmpTmplayer);
                                            (pTmpMapLayer as IGroupLayer).Add(pWGL as ILayer);
                                        }
                                        if (pMapLayer != null)
                                        {
                                            IObjectCopy pOC = new ObjectCopyClass();
                                            IWMSGroupLayer pWGL = pOC.Copy(pLayer) as IWMSGroupLayer;
                                            pWGL.Clear();
                                            (pWGL as ILayer).Name = pTmpTmplayer.Name;
                                            pWGL.Add(pTmpTmplayer);
                                            (pMapLayer as IGroupLayer).Add(pWGL as ILayer);
                                        }
                                    }
                                }//3rd 循环
                                if (pMapLayer != null)
                                {
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayerFactory);
                                    pLayerFactory = null;
                                    return pMapLayer;
                                }
                                if (pServiceLayer != null)
                                {
                                    (pServiceLayer as IGroupLayer).Add(pTmpMapLayer);
                                }
                            }// 2nd 循环

                            if (pServiceLayer != null)
                            {
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayerFactory);
                                pLayerFactory = null;
                                return pServiceLayer;
                            }
                        }
                        pLayer = pEnumLayer.Next();
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayerFactory);
                pLayerFactory = null;
            }
            else
            {
                return null;
            }

            return null;
        }
        //added by chulili 20111119 获取图层是否真正的可见
        public static bool GetVisibleOfLayer(double dCurScale,ILayer pLayer)
        {
            bool bRes = false;
            if (pLayer == null) //排除NULL值
            {
                return bRes;
            }
            if (!pLayer.Visible)    //图层本来不可见
            {
                return bRes;
            }
            //获取图层的限制比例尺
            double dMaxScale = pLayer.MaximumScale;
            double dMinScale = pLayer.MinimumScale;
            if (dMaxScale > 0)  //若最大比例尺存在
            {
                if (dCurScale <= dMaxScale) //不满足最大比例尺限制
                {
                    return bRes;
                }
            }
            if (dMinScale > 0)  //若最小比例尺存在
            {
                if (dCurScale >= dMinScale) //不满足最小比例尺限制
                {
                    return bRes;
                }
            }
            return true;    //最大最小比例尺不存在  或者  满足最大最小比例尺
        }
        public static ILayer AddLayerFromXml(IDictionary<string, IWorkspace> DicDataLibWks,XmlNode nodeLayer, IWorkspace pConfigWks, string strConfigTable, ILayer oldLayer,out Exception error)
        {
            error = null;
            try
            {
                #region 服务类型 临时为韶关添加的代码 chulili 20111104
                if (nodeLayer.Name == "Layer")
                {
                    string tmpDataType = nodeLayer.Attributes["DataType"].Value;
                    if (tmpDataType == "SERVICE")
                    {
                        try
                        {
                            string strServiceLocation = nodeLayer.Attributes["ConnectKey"].Value; ;
                            string strServiceName = nodeLayer.Attributes["FeatureDatasetName"].Value;
                            if (OpenConn(strServiceLocation, strServiceName, false))
                            {
                                IAGSServerObjectName pServerObjectName = GetMapServer(strServiceLocation, strServiceName, false);
                                IName pName = (IName)pServerObjectName;
                                IAGSServerObject pServerObject = (IAGSServerObject)pName.Open();
                                IMapServer pMapServer = (IMapServer)pServerObject;

                                IMapServerLayer pMapServerLayer = new MapServerLayerClass();

                                pMapServerLayer.ServerConnect(pServerObjectName, pMapServer.DefaultMapName);
                                ILayerGeneralProperties layerProerties = (ILayerGeneralProperties)pMapServerLayer;
                                if (layerProerties != null)
                                {
                                    layerProerties.LayerDescription = nodeLayer.OuterXml;
                                }
                                return pMapServerLayer as ILayer;
                            }
                            return null;
                        }
                        catch(Exception err)
                        {
                            return null;
                        }

                    }
                    if (tmpDataType == "WMSSERVICE")
                    {
                        try
                        {
                            string strServiceLocation = nodeLayer.Attributes["ConnectKey"].Value;
                            if (!strServiceLocation.EndsWith("?"))
                            {
                                strServiceLocation = strServiceLocation + "?";
                            }
                            string strServiceName = nodeLayer.Attributes["FeatureDatasetName"].Value;
                            string strLayerName = nodeLayer.Attributes["NodeText"].Value;
                            if (OpenWMSConn(strServiceLocation, strServiceName, false))
                            {
                                ILayer pWMSlayer = GetWMSServerLayer(strServiceLocation, strServiceName, false, strLayerName);
                                ILayerGeneralProperties layerProerties = (ILayerGeneralProperties)pWMSlayer;
                                if (layerProerties != null)
                                {
                                    layerProerties.LayerDescription = nodeLayer.OuterXml;
                                }
                                return pWMSlayer;
                            }
                            return null;
                        }
                        catch (Exception err)
                        {
                            return null;
                        }
                    }
                }
                #endregion
                ILayer returnLayer = null;
                string dataType = "";
                string layerCode = nodeLayer.Attributes["Code"].Value;
                string layerText = nodeLayer.Attributes["NodeText"].Value;
                string FeatureDataset = nodeLayer.Attributes["FeatureDatasetName"].Value;
                string layerView = "1";
                bool LayerVisible = false;
                if (nodeLayer.Attributes["View"] != null)
                    layerView = nodeLayer.Attributes["View"].Value;

                if (layerView.ToLower() == "true" || layerView == "1")
                {
                    LayerVisible = true;
                }
                IFeatureWorkspace featureWorkspace = null;
                if (nodeLayer.Name == "Layer")
                {
                    dataType = nodeLayer.Attributes["DataType"].Value;
                    featureWorkspace = GetWorkspaceFromLayerXmlNode(DicDataLibWks, nodeLayer, pConfigWks, strConfigTable) as IFeatureWorkspace;
                }
                else
                {
                    dataType = nodeLayer.Name;
                    featureWorkspace = GetWorkspaceFromLayerXmlNode(DicDataLibWks, nodeLayer, pConfigWks, strConfigTable) as IFeatureWorkspace;
                }

                if (featureWorkspace == null)
                {
                    if (error == null)
                    {
                        error = new Exception("未找到数据源");
                    }
                    return null;
                }
                IWorkspace2 pWks2 = featureWorkspace as IWorkspace2;

                switch (dataType)
                {
                    case "SubType":
                        #region SubType
                        {
                            IFeatureLayer subTypeLayer = null;
                            if (oldLayer == null)
                            {
                                IFeatureClass subTypeClass = featureWorkspace.OpenFeatureClass(layerCode);
                                subTypeLayer = GetNewFeatureLayer(subTypeClass);
                                subTypeLayer.FeatureClass = subTypeClass;
                            }
                            else
                                subTypeLayer = oldLayer as IFeatureLayer;

                            SetXmlAttrToLayer(subTypeLayer, nodeLayer, pConfigWks);

                            string[] textInfo = layerText.Split('@');
                            if (textInfo.Length > 1)
                                subTypeLayer.Name = layerCode + "@" + textInfo[1];
                            else
                                subTypeLayer.Name = layerCode;
                            subTypeLayer.Visible = true;
                            returnLayer = subTypeLayer as ILayer;
                        }
                        #endregion
                        break;
                    case "FC":
                        #region FC
                        {
                            IFeatureLayer featureLayer = null;
                            if (oldLayer == null)
                            {
                                if (!pWks2.get_NameExists(esriDatasetType.esriDTFeatureClass, layerCode)) return null;

                                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(layerCode);
                                SetXmlAttrToFeaLayer(out featureLayer, featureClass, nodeLayer, pConfigWks);
                                if (featureLayer == null)
                                {
                                    featureLayer = GetNewFeatureLayer(featureClass);
                                    featureLayer.FeatureClass = featureClass;
                                    SetXmlAttrToLayer(featureLayer, nodeLayer, pConfigWks);
                                }
                            }
                            else
                            {
                                featureLayer = oldLayer as IFeatureLayer;

                                SetXmlAttrToLayer(featureLayer, nodeLayer, pConfigWks);
                            }

                            featureLayer.Name = layerText;
                            featureLayer.Visible = true;
                            returnLayer = featureLayer as ILayer;
                        }
                        #endregion
                        break;
                    case "RC":
                        #region RC
                        {
                            try
                            {
                                IRasterWorkspaceEx rasterWorkspace = (IRasterWorkspaceEx)featureWorkspace;
                                IRasterCatalog pRCatalog = rasterWorkspace.OpenRasterCatalog(FeatureDataset);//changed by chulili 直接取数据集
                                IGdbRasterCatalogLayer pGRCatalogLayer = new GdbRasterCatalogLayerClass();
                                if (!pGRCatalogLayer.Setup((ITable)pRCatalog))
                                {

                                }
                                ILayer pLayer = pGRCatalogLayer as ILayer;
                                pLayer.Name = layerText;
                                pLayer.Visible = true;
                                SetXmlAttrToLayer(pLayer, nodeLayer, pConfigWks);
                                returnLayer = pLayer;
                            }
                            catch
                            { }
                        }
                        #endregion
                        break;
                    case "RD":
                        #region RD
                        {
                            IRasterWorkspaceEx rasterWorkspace = (IRasterWorkspaceEx)featureWorkspace;
                            IRasterDataset pRDataset = rasterWorkspace.OpenRasterDataset(FeatureDataset);//changed by chulili 直接取数据集
                            IRasterLayer pRLayer = new RasterLayer();
                            pRLayer.CreateFromDataset(pRDataset);
                            pRLayer.Name = layerText;
                            pRLayer.Visible = true;

                            SetXmlAttrToLayer(pRLayer, nodeLayer, pConfigWks);
                            returnLayer = pRLayer;
                        }
                        #endregion
                        break;
                    case "SHP":
                        #region SHP
                        {
                            IFeatureClass shpClass = featureWorkspace.OpenFeatureClass(layerCode);
                            IFeatureLayer shpLayer = new FeatureLayer();
                            shpLayer.FeatureClass = shpClass;
                            shpLayer.Name = layerText;
                            shpLayer.Visible = true;

                            SetXmlAttrToLayer(shpLayer, nodeLayer, pConfigWks);

                            returnLayer = shpLayer;
                        }
                        #endregion
                        break;
                    case "CAD":
                        #region CAD
                        {
                            ICadDrawingWorkspace cadWorkspace = (ICadDrawingWorkspace)featureWorkspace;
                            ICadDrawingDataset cadDataset = cadWorkspace.OpenCadDrawingDataset(layerCode);
                            ICadLayer cadLayer = new CadLayerClass();
                            cadLayer.CadDrawingDataset = cadDataset;
                            cadLayer.Name = layerText;
                            cadLayer.Visible = true;

                            SetXmlAttrToLayer(cadLayer, nodeLayer, pConfigWks);
                            returnLayer = cadLayer;
                        }
                        #endregion
                        break;
                    case "TIF":
                    case "BMP":
                    case "SID":
                        #region TIF,BMP,SID
                        {
                            IRasterWorkspace rasterWorkspace = (IRasterWorkspace)featureWorkspace;
                            IRasterDataset pRDataset = rasterWorkspace.OpenRasterDataset(layerCode);
                            IRasterLayer pRLayer = new RasterLayer();
                            pRLayer.CreateFromDataset(pRDataset);
                            pRLayer.Name = layerText;
                            pRLayer.Visible = true;

                            SetXmlAttrToLayer(pRLayer, nodeLayer, pConfigWks);
                            returnLayer = pRLayer;
                        }
                        #endregion
                        break;
                    case "TILE":
                        {
                            //ILayer tileLayer = ClassTile.ConnectAddMapSeverLayer(nodeLayer, (layerView == "1") ? true : false, clsMain);
                            //returnLayer = tileLayer;
                        }
                        break;
                    default:
                        break;
                }
                returnLayer.Visible = LayerVisible;
                return returnLayer;
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public static void SetFieldVisibleOfLayer(ILayer pLayer, XmlNode pLayerNode)
        {
            if (pLayer == null)
            {
                return;
            }
            if (pLayerNode == null)
            {
                return;
            }
            ILayerFields pLayerFields = (ILayerFields)pLayer;
            try
            {
                if (pLayerFields != null)
                {
                    for (int i = 0; i < pLayerFields.FieldCount; i++)
                    {
                        IFieldInfo pFieldInfo = pLayerFields.get_FieldInfo(i);

                        XmlNode pNode = pLayerNode.SelectSingleNode("./Field[@AliasName='" + pFieldInfo.Alias + "']");
                        if (pNode != null)
                        {
                            string strNodeKey = (pNode as XmlElement).GetAttribute("NodeKey");
                            if (!Plugin.ModuleCommon.ListUserdataPriID.Contains(strNodeKey))
                            {
                                pFieldInfo.Visible = false;
                            }
                        }
                    }
                }
            }
            catch
            { }
        }
        public static IFeatureWorkspace GetWorkspaceFromLayerXmlNode(IDictionary<string, IWorkspace> DicDataLibWks,XmlNode layerNode, IWorkspace pConfigWks, string strConnectTable)
        {
            //
            //return pConfigWks as IFeatureWorkspace;

            if (layerNode.Attributes["ConnectKey"] == null) return null;

            string ConnectKey = layerNode.Attributes["ConnectKey"].Value;
            if (!DicDataLibWks.Keys.Contains(ConnectKey))
            {
                IWorkspace pTempWks = GetWorkspaceByConnectKey(ConnectKey, pConfigWks, strConnectTable);
                if (pTempWks == null) return null;
                DicDataLibWks.Add(ConnectKey, pTempWks);
            }
            if (DicDataLibWks[ConnectKey]==null)            
            {
                return pConfigWks as IFeatureWorkspace;
            }
            else
            {
                return DicDataLibWks[ConnectKey] as IFeatureWorkspace;
            }
        }

        public static IWorkspace GetWorkspaceByConnectKey(string strConnectKey, IWorkspace pWks, string strConfigName)
        {
            Exception exError = null;
            IWorkspace pWorkspace = null;
            SysCommon.Gis.SysGisTable  sysTable = new SysCommon.Gis.SysGisTable(pWks);
            Dictionary<string, object> DicData = sysTable.GetRow("DATABASEMD", "ID="+strConnectKey+"", out exError);
            if (DicData == null)
                return pWorkspace;
            if (DicData.Count == 0)
                return pWorkspace;
            string connstr = "";
            string dbsourceid="";
            int itype=-1;
            if (DicData["CONNECTIONINFO"] != null)
                connstr = DicData["CONNECTIONINFO"].ToString();
            if (DicData["ID"] != null)
                dbsourceid = DicData["ID"].ToString();
            if (DicData["DATAFORMATID"] != null)
                itype = int.Parse(DicData["DATAFORMATID"].ToString());
            if ((!connstr.Equals("")) && (!dbsourceid.Equals("")))
            {
                pWorkspace = GetWorkSpacefromConninfo(connstr, itype);
                return pWorkspace;
            }
            return pWorkspace;
        }
        public static void SetXmlAttrToFeaLayer(out IFeatureLayer  pFeatureLayer,IFeatureClass pFeaClass, XmlNode layerNode, IWorkspace pConfigWks)
        {
            ////记录数据是DLG、DEM还是DOM
            //string strDataType = XmlOperation.GetString(layerNode, "", "FeatureType", false);
            

            //比例尺设置
            XmlNode vNodeAboutShow = layerNode["AboutShow"];
            string strRenderKey = "";
            if (vNodeAboutShow != null)
            {
                strRenderKey = XmlOperation.GetString(vNodeAboutShow, "", "Renderer", false);
            }
            try
            {
                object objLayer = ModuleRenderer.GetLayerConfigFromBlob("ID='" + strRenderKey + "'", pConfigWks);
                if (objLayer != null)
                {
                    pFeatureLayer = objLayer as IFeatureLayer;
                    pFeatureLayer.FeatureClass = pFeaClass;
                    //将整个节点作为xml传递给layer层的description
                    ILayerGeneralProperties layerProerties = (ILayerGeneralProperties)pFeatureLayer;
                    layerProerties.LayerDescription = layerNode.OuterXml;

                    //比例尺设置
                    if (vNodeAboutShow != null)
                    {
                        double dMaxScale = XmlOperation.GetDouble(vNodeAboutShow, 0, "MaxScale", true);
                        double dMinScale = XmlOperation.GetDouble(vNodeAboutShow, 0, "MinScale", true);

                        pFeatureLayer.MaximumScale = dMaxScale;
                        pFeatureLayer.MinimumScale = dMinScale;

                    }
                    SetFieldVisibleOfLayer(pFeatureLayer as ILayer, layerNode);
                    return;
                }
            }
            catch
            { }
            pFeatureLayer = null;
        }
        public static void SetXmlAttrToLayer(ILayer pLayer, XmlNode layerNode, IWorkspace pConfigWks)
        {
            //记录数据是DLG、DEM还是DOM
            string strDataType = XmlOperation.GetString(layerNode, "", "FeatureType", false);
            //将整个节点作为xml传递给layer层的description
            ILayerGeneralProperties layerProerties = (ILayerGeneralProperties)pLayer;
            layerProerties.LayerDescription = layerNode.OuterXml;

            //比例尺设置
            XmlNode vNodeAboutShow = layerNode["AboutShow"];
            string strRenderKey = "";
            if (vNodeAboutShow != null)
            {
                double dMaxScale = XmlOperation.GetDouble(vNodeAboutShow, 0, "MaxScale", true);
                double dMinScale = XmlOperation.GetDouble(vNodeAboutShow, 0, "MinScale", true);

                pLayer.MaximumScale = dMaxScale;
                pLayer.MinimumScale = dMinScale;

                strRenderKey = XmlOperation.GetString(vNodeAboutShow, "", "Renderer", false);
            }

            if (pLayer is  IRasterLayer)
            {
                // shduan 20110720 去数据库中读取符号，如果没有则给出默认显示符号
                IRasterLayer pRLayer = pLayer as IRasterLayer;
                IRasterRenderer pRasterRenderer = ModuleRenderer.DoWithRasterRenderer(pConfigWks, strRenderKey, "", pRLayer);
                if (pRasterRenderer != null)
                {
                    pRLayer.Renderer = pRasterRenderer;
                }
                else
                {
                    //默认设置颜色 如果是l拉伸符号为透明色 否则为白色
                    ModuleRenderer.SetRasterDefaultNoDataColor(pLayer);
                    //if (strDataType == "DOM")
                    //{
                    //    IRasterRGBRenderer pRasterRGBRenderer = new RasterRGBRendererClass();
                    //    IRasterStretch2 pRasterStretch2 = pRasterRGBRenderer as IRasterStretch2;
                    //    //pRasterStretch2.Background = true;
                    //    //IColor pColor = new RgbColorClass();
                    //    //pColor.RGB = 0xFFFFFF;
                    //    //pRasterStretch2.BackgroundColor = pColor;

                    //    pRasterStretch2.StretchType = esriRasterStretchTypesEnum.esriRasterStretch_NONE;
                    //    pRasterStretch2.StretchStatsType = esriRasterStretchStatsTypeEnum.esriRasterStretchStats_Dataset;
                    //    IArray pArrayNew = new ArrayClass();
                    //    pArrayNew.Add(pRasterRGBRenderer);

                    //    pRLayer.Renderer = pRasterRGBRenderer as IRasterRenderer;
                    //}
                }
            }
            else if (pLayer is  IRasterCatalogLayer)
            {
                //默认设置颜色 如果是l拉伸符号为透明色 否则为白色
                ModuleRenderer.SetRasterDefaultNoDataColor(pLayer);
            }
            else if (pLayer is IFeatureLayer)
            {
                //设置符号
                
                IFeatureLayer pFeatureLayer = (IFeatureLayer)pLayer;
                
                if (!(pFeatureLayer is IGeoFeatureLayer)) return;
                IGeoFeatureLayer pGeoFeatLyr = pFeatureLayer as IGeoFeatureLayer;

                IFeatureRenderer pFRenderer = ModuleRenderer.DoWithRenderer(pConfigWks, strRenderKey, "", pFeatureLayer);
                if (pFRenderer != null)
                {
                    pGeoFeatLyr.Renderer = pFRenderer;
                }
                //当时栅格目录时，返回
                if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTRasterCatalogItem) return;

                if (vNodeAboutShow != null)
                {
                    //是否随比例尺缩放
                    pFeatureLayer.ScaleSymbols = XmlOperation.GetBoolean(vNodeAboutShow, false, "IsReferScale");
                    //图层透明度
                    ILayerEffects pLayerEffects = pFeatureLayer as ILayerEffects;
                    pLayerEffects.Transparency = XmlOperation.GetShort(vNodeAboutShow, 0, "LayerTransparency", true);
                }
                //added by chulili 20110907  处理图层隐藏要素功能(韶关特殊需求，显示标注时，隐藏要素符号)
                if (layerNode["AttrLabel"] != null)
                {
                    bool HideSymbol = XmlOperation.GetBoolean(layerNode["AttrLabel"], false, "HideSymbol");
                    if (HideSymbol)
                    {
                        IFeatureRenderer pTmpRenderer = ModuleRenderer.HideSymbolOfLayer(pGeoFeatLyr);
                        pGeoFeatLyr.Renderer = pTmpRenderer;
                    }
                }
                //end added by chulili 
                //定义表达式
                if (layerNode["ShowDefine"] != null)
                {
                    if (XmlOperation.GetBoolean(layerNode["ShowDefine"], false, "IsDefineDisplay"))
                    {
                        IFeatureLayerDefinition featureLayerDefine = pFeatureLayer as IFeatureLayerDefinition;
                        string express = XmlOperation.GetString(layerNode["ShowDefine"], "", "Express", true);
                        featureLayerDefine.DefinitionExpression = express;
                    }
                }

                //设置标注信息
                if (layerNode["AttrLabel"] != null)
                {
                    bool isLabel = XmlOperation.GetBoolean(layerNode["AttrLabel"], false, "IsLabel");
                    if (isLabel)
                    {
                        pGeoFeatLyr.DisplayAnnotation = true;
                        ModuleLabel.SetLayerInfoFromXml(pGeoFeatLyr);

                        //ModuleLabel.SetLayerInfoFromXml(pConfigWks, strRenderKey, pGeoFeatLyr);

                    }
                }
                try
                {
                    //统一设置填充，边框及注记颜色
                    string fillColor = XmlOperation.GetString(vNodeAboutShow, "", "FillColor", true);
                    string borderColor = XmlOperation.GetString(vNodeAboutShow, "", "BorderColor", true);
                    if (string.IsNullOrEmpty(fillColor) == false && string.IsNullOrEmpty(borderColor) == false)
                    {
                        IRgbColor fillRGBColor = ModuleRenderer.GetRgbColorFromRgbString(fillColor);
                        IRgbColor borderRGBColor = ModuleRenderer.GetRgbColorFromRgbString(borderColor);
                        pGeoFeatLyr.Renderer = ModuleRenderer.SetColorOfRenderer(pGeoFeatLyr, fillRGBColor, borderRGBColor, -1, -1);
                    }
                    //设置注记颜色
                    if (pFeatureLayer is IFDOGraphicsLayer)
                    {
                        string annoColor = XmlOperation.GetString(vNodeAboutShow, "", "AnnoColor", true);
                        if (string.IsNullOrEmpty(annoColor) == false)
                        {
                            ISymbolSubstitution annoSymbolSubstitution = pFeatureLayer as ISymbolSubstitution;
                            annoSymbolSubstitution.SubstituteType = esriSymbolSubstituteType.esriSymbolSubstituteColor;
                            annoSymbolSubstitution.MassColor = ModuleRenderer.GetRgbColorFromRgbString(annoColor);
                        }
                    }
                    SetFieldVisibleOfLayer(pFeatureLayer as ILayer, layerNode);
                }
                catch
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 返回一个DimensionLayer、FDOGraphicsLayer、FeatureLayer的图层，都支持IFeatureLayer接口
        /// </summary>
        /// <returns></returns>
        public static IFeatureLayer GetNewFeatureLayer(IFeatureClass pFeatureClass)
        {
            IFeatureLayer pFeatureLayer;
            switch (pFeatureClass.FeatureType)
            {
                case esriFeatureType.esriFTAnnotation:          //注记层
                    pFeatureLayer = (IFeatureLayer)(new FDOGraphicsLayer());
                    break;
                case esriFeatureType.esriFTDimension:           //标注层
                    pFeatureLayer = (IFeatureLayer)(new DimensionLayer());
                    break;
                case esriFeatureType.esriFTRasterCatalogItem:   //影像层
                    pFeatureLayer = (IFeatureLayer)(new GdbRasterCatalogLayer());
                    break;
                default:
                    pFeatureLayer = new FeatureLayer();
                    break;
            }
            pFeatureLayer.FeatureClass = pFeatureClass;
            return pFeatureLayer;
        }
        public static void SetLayersVisibleAttri(ESRI.ArcGIS.Controls.IMapControlDefault pMapcontrol,bool isVisible)
        {
            IMap pMap = pMapcontrol.Map;

            int iCount = pMapcontrol.LayerCount;
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                ILayer pLayer = pMap.get_Layer(iIndex) as ILayer;
                IFeatureLayer pFeatureLayer = pMap.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = pMap.get_Layer(iIndex) as IGroupLayer;
                
                if (groupTempLayer != null)
                {
                    SetLayersVisibleAttri(groupTempLayer, isVisible);
                    groupTempLayer.Visible = isVisible;
                }

                if (pLayer == null) continue;
                bool bNodeVisible = getVisibleOfLayerNode(pMap.get_Layer(iIndex));
                if (!isVisible)
                {
                    bNodeVisible = false;
                }
                pLayer.Visible = bNodeVisible;
            }
        }
        public static void SetLayersVisibleAttri(IMapDocument pMapdoc, bool isVisible)
        {
            if (pMapdoc.MapCount == 0)
            {
                return;
            }
            IMap pMap = pMapdoc.get_Map(0);

            int iCount = pMap.LayerCount;
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = pMap.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = pMap.get_Layer(iIndex) as IGroupLayer;

                if (groupTempLayer != null)
                {
                    SetLayersVisibleAttri(groupTempLayer, isVisible);
                    groupTempLayer.Visible = isVisible;
                }

                if (pFeatureLayer == null) continue;
                bool bNodeVisible = getVisibleOfLayerNode(pMap.get_Layer(iIndex));
                if (!isVisible)
                {
                    bNodeVisible = false;
                }
                pFeatureLayer.Visible = bNodeVisible;
            }
        }
        public static bool getVisibleOfLayerNode(ILayer pLayer)
        {
            bool bVisible = false;
            ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
            //读取图层的描述，转成xml节点
            string strNodeXml = pLayerGenPro.LayerDescription;

            if (!strNodeXml.Equals(""))
            {
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.LoadXml(strNodeXml);
                //获取节点的NodeKey信息
                XmlNode pxmlnode = pXmldoc.SelectSingleNode("//Layer");
                XmlElement pxmlEle = pxmlnode as XmlElement;

                if (pxmlEle != null)
                {
                    if (pxmlEle.HasAttribute("View"))
                    {
                        string strView = pxmlEle.GetAttribute("View");
                        if (strView.ToLower() == "true" || strView == "1")
                        {
                            bVisible = true;
                        }
                        else
                        {
                            bVisible = false;
                        }
                    }
                }
                pXmldoc = null;
            }
            return bVisible;
            

        }
        public static void SetLayersVisibleAttri(IGroupLayer groupLayer, bool isVisible)
        {
            ICompositeLayer comLayer = groupLayer as ICompositeLayer;
            int iCount = comLayer.Count;

            //对Dimension层进行排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = comLayer.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = comLayer.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    SetLayersVisibleAttri(groupTempLayer, isVisible);
                }

                if (pFeatureLayer == null) break;
                pFeatureLayer.Visible = isVisible;
            }
        }
        #region 图层排序
        /// <summary>
        /// 对mapcontrol上的图层进行排序
        /// </summary>
        /// <param name="vAxMapControl"></param>
        public static void LayersComposeEx(ESRI.ArcGIS.Controls.IMapControlDefault pMapcontrol)
        {
            IMap pMap = pMapcontrol.Map;
            int[] iLayerIndex = new int[2] { 0, 0 };
            int[] iFeatureLayerIndex = new int[3] { 0, 0, 0 };

            int iCount = pMapcontrol.LayerCount;
            //组内排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IGroupLayer groupTempLayeri = pMap.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayeri != null)
                {
                    LayersComposeEx(pMap, groupTempLayeri);
                }
            }
            //图层组排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                ILayer TempLayeri = pMap.get_Layer(iIndex) as ILayer;
                //LayersComposeEx(groupTempLayeri);
                for (int jindex = iIndex+1; jindex < iCount; jindex++)
                {

                    ILayer TempLayerj = pMap.get_Layer(jindex) as ILayer;
                    int iOrderi = -1;
                    int iOrderj = -1;
                    if (TempLayeri != null && TempLayerj != null)
                    {

                        string strOrderid_i = GetOrderIDofLayer(TempLayeri);
                        string strOrderid_j = GetOrderIDofLayer(TempLayerj);
                        if (!strOrderid_i.Equals("") && !strOrderid_j.Equals(""))
                        {
                            try
                            {
                                iOrderi = int.Parse(strOrderid_i);
                                iOrderj = int.Parse(strOrderid_j);

                            }
                            catch
                            { }
                        }
                        if (iOrderi >0 && iOrderj>0)
                        {
                            if (iOrderi > iOrderj)
                            {
                                pMap.MoveLayer(TempLayerj, iIndex);
                                TempLayeri = pMap.get_Layer(iIndex) as ILayer;
                            }
                        }
                        else
                        {
                            int intDataTypeID_i = GetDataTypeIDofLayer(TempLayeri);
                            int intDataTypeID_j = GetDataTypeIDofLayer(TempLayerj);
                            if (intDataTypeID_i > intDataTypeID_j)
                            {
                                pMap.MoveLayer(TempLayerj, iIndex);
                                TempLayeri = pMap.get_Layer(iIndex) as ILayer;
                            }
                        }
                    }
                }                
            }
        }
        public static int GetDataTypeIDofLayer(ILayer pLayer)
        {
            //类型顺序是：注记：0 点：1 线：2  面：3  栅格：4
            if (pLayer is IGroupLayer)
                return 9;
            if (pLayer is IFeatureLayer)
            {
                IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                if(pFeatureLayer.FeatureClass!=null)
                {
                    switch (pFeatureLayer.FeatureClass.FeatureType)
                    {
                        case esriFeatureType.esriFTAnnotation:
                            return 0;
                            break;
                        case esriFeatureType.esriFTSimple:
                            switch (pFeatureLayer.FeatureClass.ShapeType)
                            {
                                case esriGeometryType.esriGeometryPoint:
                                case esriGeometryType.esriGeometryMultipoint:
                                    return 1;
                                    break;
                                case esriGeometryType.esriGeometryLine:
                                case esriGeometryType.esriGeometryPolyline:
                                    return 2;
                                    break;
                                case esriGeometryType.esriGeometryPolygon:
                                    return 3;
                                    break;
                            }
                            break;
                    }
                }

            }
            else if(pLayer is IRasterLayer)
            {
                return 4;
            }
            else if(pLayer is IRasterCatalogLayer)
            {
                return 4;
            }
            return 999;//无效数据

        }
        //想要新添加一个图层，先获取其在Map中应加载在什么位置
        //参数含义：Mapcontrol控件，图层
        public static int GetIndexOfNewLayer(ESRI.ArcGIS.Controls.IMapControlDefault pMapcontrol,ILayer pLayer)
        {
            int intOrderID = -1;
            int intDataTypeID = -1;//为不同的数据类型分配不同的整数，用来排序
            string strOrderID = GetOrderIDofLayer(pLayer);
            intDataTypeID = GetDataTypeIDofLayer(pLayer);
            if (!strOrderID.Equals(""))
            {
                intOrderID = int.Parse(strOrderID);
            }

            if (pLayer is IGroupLayer)
            {
                LayersComposeEx(pMapcontrol.Map, pLayer as IGroupLayer);
            }
            int IndexOfNewLayer = pMapcontrol.LayerCount;   //预定排在最下面
            for (int i = 0; i < pMapcontrol.Map.LayerCount; i++)    //从上往下找
            {
                ILayer pCurLayer = pMapcontrol.Map.get_Layer(i);
                string strCurOrderID = GetOrderIDofLayer(pCurLayer);
                int intCurOrderID = -1;
                if (!strCurOrderID.Equals(""))
                    intCurOrderID = int.Parse(strCurOrderID);
                if (intOrderID > 0 && intCurOrderID > 0)    //如果有顺序号，按照顺序号，顺序号越小的图层排在上面
                {
                    if (intOrderID < intCurOrderID)//顺序号比当前图层小，排在当前图层上面，退出循环（因为是从上往下找，找到第一个就可以退出）
                    {
                        if (IndexOfNewLayer > i)
                        {
                            IndexOfNewLayer = i;
                            break;
                        }
                    }
                    else if (intOrderID > intCurOrderID)    //顺序号比当前图层大，排在当前图层的下面
                    {
                        if (IndexOfNewLayer < i)    //这种情况不会遇到
                        {
                            IndexOfNewLayer = i+1;
                        }
                    }
                }
                else    //如果没有顺序号，那么按照类型，注记层排在最上面，下面依次是点、线、面，类型编号小的排在上面
                {
                    int intCurDataTypeID = GetDataTypeIDofLayer(pCurLayer);
                    if (intDataTypeID < intCurDataTypeID)  //类型号比当前图层小，排在当前图层的上面 
                    {
                        if (IndexOfNewLayer > i)
                        {
                            IndexOfNewLayer = i;
                            break;
                        }
                    }
                    else if (intDataTypeID > intCurDataTypeID)  //类型号比当前图层大，排在当前图层的下面
                    {
                        if (IndexOfNewLayer < i)    //这种情况不会遇到
                        {
                            IndexOfNewLayer = i+1;
                        }
                    }

                }

            }
            return IndexOfNewLayer;
        }
        public static void DealOrderOfNewLayer(ESRI.ArcGIS.Controls.IMapControlDefault pMapcontrol,ILayer pLayer)
        {
            int intOrderID = -1;
            int intDataTypeID = -1;//为不同的数据类型分配不同的整数，用来排序
            string strOrderID = GetOrderIDofLayer(pLayer);
            intDataTypeID = GetDataTypeIDofLayer(pLayer);
            if (!strOrderID.Equals(""))
            {
                intOrderID = int.Parse(strOrderID);
            }
            
            if (pLayer is IGroupLayer)
            {
                LayersComposeEx(pMapcontrol.Map ,pLayer as IGroupLayer);
            }
            int IndexOfNewLayer = -1;
            for (int i = 0; i < pMapcontrol.Map.LayerCount; i++)
            {
                ILayer pTmpLayer = pMapcontrol.Map.get_Layer(i);
                if (pTmpLayer.Equals(pLayer))
                {
                    if ((pTmpLayer as ILayerGeneralProperties).LayerDescription == (pLayer as ILayerGeneralProperties).LayerDescription)
                    {
                        IndexOfNewLayer = i;
                        break;
                    }
                }
            }
            for (int i = 0; i < pMapcontrol.Map.LayerCount; i++)
            {
                ILayer pCurLayer = pMapcontrol.Map.get_Layer(i);
                string strCurOrderID = GetOrderIDofLayer(pCurLayer);
                int intCurOrderID = -1;
                if (!strCurOrderID.Equals(""))
                    intCurOrderID = int.Parse(strCurOrderID);
                if (intOrderID > 0 && intCurOrderID > 0)
                {
                    if (intOrderID < intCurOrderID)
                    {
                        if (IndexOfNewLayer > i)
                        {
                            pMapcontrol.Map.MoveLayer(pLayer, i);
                            IndexOfNewLayer = i;
                            break;
                        }
                    }
                    else if (intOrderID > intCurOrderID)
                    {
                        if (IndexOfNewLayer < i)
                        {
                            pMapcontrol.Map.MoveLayer(pLayer, i);
                            IndexOfNewLayer = i;
                        }
                    }
                }
                else
                {
                    int intCurDataTypeID = GetDataTypeIDofLayer(pCurLayer);
                    if (intDataTypeID < intCurDataTypeID)
                    {
                        if (IndexOfNewLayer > i)
                        {
                            pMapcontrol.Map.MoveLayer(pLayer, i);
                            IndexOfNewLayer = i;
                            break;
                        }
                    }
                    else if (intDataTypeID > intCurDataTypeID)
                    {
                        if (IndexOfNewLayer < i)
                        {
                            pMapcontrol.Map.MoveLayer(pLayer, i);
                            IndexOfNewLayer = i;
                        }
                    }

                }

            }
        }
        //added by chulili 20110731 获取图层顺序号
        public static string  GetOrderIDofLayer(ILayer pLayer)
        {
            try
            {
                ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
                //读取图层的描述
                string strNodeXml = pLayerGenPro.LayerDescription;
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.LoadXml(strNodeXml);
                //构成xml节点，根据NodeKey在节点里查询
                XmlNode pNode = pXmlDoc.ChildNodes[0];
                string strOrder = "";
                try
                {
                    strOrder = pNode.Attributes["OrderID"].Value.ToString();
                }
                catch
                { }
                return strOrder;
            }
            catch
            { 
            }
            return "";

        }
        /// <summary>
        /// 对mapcontrol上groupLayer内的图层进行排序
        /// </summary>
        /// <param name="groupLayer"></param>
        public static void LayersComposeEx(IMap pMap, IGroupLayer groupLayer)
        {
            //判断参数有效性
            if (pMap == null) return;
            if (groupLayer == null) return;
            ICompositeLayer comLayer = groupLayer as ICompositeLayer;
            int iCount = comLayer.Count;
            IMapLayers pMapLayers = pMap as IMapLayers;
            //对Dimension层进行排序 冒泡排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                ILayer TempLayeri = comLayer.get_Layer(iIndex) as ILayer;
                for (int jindex = iIndex + 1; jindex < iCount; jindex++)
                {

                    ILayer TempLayerj = comLayer.get_Layer(jindex) as ILayer;
                    if (TempLayeri != null && TempLayerj != null)
                    {
                        //获取图层顺序号
                        string strOrderid_i = GetOrderIDofLayer(TempLayeri);
                        string strOrderid_j = GetOrderIDofLayer(TempLayerj);
                        int iOrderi = -1;
                        int iOrderj = -1;

                        if (!strOrderid_i.Equals("") && !strOrderid_j.Equals(""))
                        {
                            try
                            {
                                iOrderi = int.Parse(strOrderid_i);
                                iOrderj = int.Parse(strOrderid_j);
                            }
                            catch
                            { }
                        }
                        if (iOrderi>0 &&  iOrderj>0)
                        {
                            if (iOrderi > iOrderj)
                            {
                                groupLayer.Delete(TempLayerj);
                                pMapLayers.InsertLayerInGroup(groupLayer, TempLayerj, false, iIndex);
                                TempLayeri = comLayer.get_Layer(iIndex) as ILayer;
                            }

                        }
                        else
                        {
                            int intDataTypeID_i = GetDataTypeIDofLayer(TempLayeri);
                            int intDataTypeID_j = GetDataTypeIDofLayer(TempLayerj);
                            if (intDataTypeID_i > intDataTypeID_j)
                            {
                                groupLayer.Delete(TempLayerj);
                                pMapLayers.InsertLayerInGroup(groupLayer, TempLayerj, false, iIndex);
                                TempLayeri = comLayer.get_Layer(iIndex) as ILayer;
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region 图层排序
        /// <summary>
        /// 对mapcontrol上的图层进行排序
        /// </summary>
        /// <param name="vAxMapControl"></param>
        public static void LayersCompose(ESRI.ArcGIS.Controls.IMapControlDefault pMapcontrol)
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

                if (pFeatureLayer == null) continue ;
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

                if (pFeatureLayer == null) continue ;
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

        /// <summary>
        /// 从Blob字段中获取Imap对象
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="querySql"></param>
        /// <returns></returns>
        public static void GetMapFromBlob(IWorkspace workspace, string tableName, string fieldName, string querySql, IMap map)
        {
            //try
            //{
            //    byte[] mapByte = GeoGISBase.ModuleDatabase.ReadByteFromBlob(workspace, tableName, querySql, fieldName);

            //    //是否得到了序列化的blob，则返回NULL
            //    if (mapByte == null || mapByte.Length == 0) return;
            //    IMemoryBlobStreamVariant pMemoryBlobStreamVariant = new MemoryBlobStreamClass();
            //    pMemoryBlobStreamVariant.ImportFromVariant((object)mapByte);
            //    IStream pStream = pMemoryBlobStreamVariant as IStream;
            //    IPersistStream pPersistStream = map as IPersistStream;
            //    pPersistStream.Load(pStream);
            //    map = pPersistStream as IMap;
            //}
            //catch
            //{
            //    return;
            //}
        }

        //公共变量 用户存放数据视图的工作空间
        public static IDictionary<string, IWorkspace> _DicDataLibWks = new Dictionary<string, IWorkspace>();
        public static IDictionary<string, List<IDataset>> _DicDataset = new Dictionary<string, List<IDataset>>();
        //ZQ  2011020  modify 移植到sysCommon
        //public static IDictionary<string, string> _DicLayerName = new Dictionary<string, string>();
        ////当前Map中已经添加的图层
        //public static IDictionary<string, ILayer> DicAddLyrs = new Dictionary<string, ILayer>();
        ////当前Map中添加后又删除的图层
        //public static IDictionary<string, ILayer> DicDelLyrs = new Dictionary<string, ILayer>();
        public static string _DefaultDBsource = "";

        //changed by chulili 20110922 作为全局变量放到SysCommon工程里面
        //private  static bool _IsLayerTreeChanged = false;//added by chulili 20110701 表示图层目录配置是否有所修改
        //public static bool IsLayerTreeChanged
        //{
        //    set { _IsLayerTreeChanged = value; }
        //    get { return _IsLayerTreeChanged; }
        //}
        //end changed by chulili 20110922
    }
}
