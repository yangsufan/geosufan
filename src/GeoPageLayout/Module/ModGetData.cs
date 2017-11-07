using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using System.IO;
using System.Xml;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;

namespace GeoPageLayout
{
    public static class ModGetData
    {
        public static string LayerXMLpath = Application.StartupPath + "\\..\\res\\xml\\展示图层树.xml";
        static string xzqXml = Application.StartupPath + "\\..\\res\\xml\\xzq.xml";
        static string pageLayoutXml = Application.StartupPath + "\\..\\res\\xml\\PageLayout.xml";
        //获取地图来源方式
        public static bool IsMapSpecial()
        {
            bool res=true;
            if (!File.Exists(pageLayoutXml))
            {
                return true;
            }
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(pageLayoutXml);
                XmlElement xe = cXmlDoc.SelectSingleNode("//MapSource") as XmlElement;
                if (xe.HasAttribute("Special"))
                {
                    string value = xe.GetAttribute("Special");
                    if (value == "1")
                        res = true;
                    else
                        res = false;
                }
                cXmlDoc = null;
            }
            return res;
 
        }
        //获取制图机构所在市
        public static string  OrgName()
        {
            string res = "";
            if (!File.Exists(pageLayoutXml))
            {
                return "";
            }
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(pageLayoutXml);
                XmlElement xe = cXmlDoc.SelectSingleNode("//OrgName") as XmlElement;
                if (xe.HasAttribute("ValueSet"))
                {
                    res = xe.GetAttribute("ValueSet");
                    
                }
                cXmlDoc = null;
            }
            return res;

        }
        //获取制图配置信息，元素，属性
        public static string AttrValue(string inEleName,string inAttrName)
        {
            string res = "";
            if (!File.Exists(pageLayoutXml))
            {
                return "";
            }
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(pageLayoutXml);
                XmlElement xe = cXmlDoc.SelectSingleNode("//"+inEleName) as XmlElement;
                if (xe.HasAttribute(inAttrName))
                {
                    res = xe.GetAttribute(inAttrName);

                }
                cXmlDoc = null;
            }
            return res;

        }
        //获取制图配置信息，元素，属性
        public static string AttrValueByXpath(string xPath, string inAttrName)
        {
            string res = "";
            if (!File.Exists(pageLayoutXml))
            {
                return "";
            }
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(pageLayoutXml);
                XmlElement xe = cXmlDoc.SelectSingleNode(xPath) as XmlElement;
                if (xe != null)
                {
                    if (xe.HasAttribute(inAttrName))
                    {
                        res = xe.GetAttribute(inAttrName);

                    }
                }
                cXmlDoc = null;
            }
            return res;

        }
        //获取行政区代码字段名
        public static string  GetXZQFd(string xpath)
        {
            if (!File.Exists(xzqXml))
            {
                return null;
            }
            XmlDocument cXmlDoc = new XmlDocument();
            cXmlDoc.Load(xzqXml);
            if (cXmlDoc == null)
                return null;
            XmlNode xn = cXmlDoc.SelectSingleNode(xpath);
            if (xn == null)
                return null;
            XmlElement xe = xn as XmlElement;
            if (!xe.HasAttribute("XZBMField"))
                return null;
            string fdname = xn.Attributes["XZBMField"].Value;
            cXmlDoc = null;
            return fdname;

        }
        //获取行政区要素
        public static IFeatureClass GetXZQFC(string xpath)
        {
            if (!File.Exists(xzqXml))
            {
                return null;
            }
            XmlDocument cXmlDoc = new XmlDocument();
            cXmlDoc.Load(xzqXml);
            if (cXmlDoc == null)
                return null;
            XmlNode xn = cXmlDoc.SelectSingleNode(xpath);
            if (xn == null)
                return null;
            string nk = xn.Attributes["NodeKey"].Value;
            cXmlDoc = null;
            return GetFeatureClassByNodeKey(nk);
 
        }
        //由行政区代码获得行政区名称-县级
        public static string getXZQMC(string xzqCode)
        {
            string schemaPath = Application.StartupPath + "\\..\\Res\\Xml\\XZQ.xml";
            if (!File.Exists(schemaPath))
            {
                return null;
            }
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(schemaPath);
                XmlNode xn = cXmlDoc.SelectSingleNode("//County[@XzqCode='" + xzqCode + "']");
                if (xn != null)
                {
                    string name = xn.Attributes["ItemName"].Value;
                    cXmlDoc = null;
                    return name;
                }

            }
            return null;

        }
        //由专题类型获得专题名称
        public static List<string> GetZT(string inType)
        {
            
            if (!File.Exists(LayerXMLpath))
            {
                return null;
            }
            XmlDocument cXmlDoc = new XmlDocument();
            cXmlDoc.Load(LayerXMLpath);
            if (cXmlDoc == null)
                return null;
            XmlNodeList xnl = cXmlDoc.SelectNodes("//DIR[@DIRType='" + inType + "']");
            if (xnl == null)
                return null;
            List<string> res = new List<string>();
            foreach (XmlNode pxn in xnl)
            {
                string zt = pxn.Attributes["NodeText"].Value;
                res.Add(zt);
            }
            
            cXmlDoc = null;
            return res;
        }
        //由专题类型获得专题名称---一般性---影像
        public static List<string> GetZTFromXPath(string xpath)
        {

            if (!File.Exists(LayerXMLpath))
            {
                return null;
            }
            XmlDocument cXmlDoc = new XmlDocument();
            cXmlDoc.Load(LayerXMLpath);
            if (cXmlDoc == null)
                return null;
            XmlNodeList xnl = cXmlDoc.SelectNodes(xpath);
            if (xnl == null)
                return null;
            List<string> res = new List<string>();
            foreach (XmlNode pxn in xnl)
            {
                string zt = pxn.Attributes["NodeText"].Value;
                res.Add(zt);
            }

            cXmlDoc = null;
            return res;
        }
        //由专题类型构造制图页面的地图图层
        public static void AddMapOfNoneXZQ(IMap inMap,string inType,IMap inSourceMap)
        {
            IMapLayers inMapLayers = inMap as IMapLayers;
            if (!File.Exists(LayerXMLpath))
            {
                return;
            }
            XmlDocument cXmlDoc = new XmlDocument();
            cXmlDoc.Load(LayerXMLpath);
            if (cXmlDoc == null)
                return;
            XmlNode xn = cXmlDoc.SelectSingleNode("//DIR[@DIRType='" + inType + "']");
            if(xn==null)
                return;
            inMap.Name=xn.Attributes["NodeText"].Value;//设置地图名称
            IGroupLayer gLayer=new GroupLayerClass();
            gLayer.Name=xn.Attributes["NodeText"].Value;
            XmlNodeList xnl = xn.SelectNodes(".//Layer");
            foreach (XmlNode pxn in xnl)
            {
                string nodeKey=pxn.Attributes["NodeKey"].Value;
                ILayer pLayer=GetLayerByNodeKey(inSourceMap,nodeKey);
                if(pLayer!=null)
                    gLayer.Add(pLayer);
            }
            inMapLayers.InsertLayer(gLayer as ILayer, false, inMapLayers.LayerCount);
            cXmlDoc = null;
        }
        //由专题类型构造制图页面的地图图层-----多了个专题名称针对DOM
        public static void AddMapOfNoneXZQ(IMap inMap, string inType,string LyrNT, IMap inSourceMap)
        {
            IMapLayers inMapLayers = inMap as IMapLayers;
            if (!File.Exists(LayerXMLpath))
            {
                return;
            }
            XmlDocument cXmlDoc = new XmlDocument();
            cXmlDoc.Load(LayerXMLpath);
            if (cXmlDoc == null)
                return;
            XmlNode xn = cXmlDoc.SelectSingleNode("//DIR[@DIRType='" + inType + "']");
            if (xn == null)
                return;
            inMap.Name = xn.Attributes["NodeText"].Value;//设置地图名称
            IGroupLayer gLayer = new GroupLayerClass();
            gLayer.Name = xn.Attributes["NodeText"].Value;
            XmlNodeList xnl = xn.SelectNodes(".//Layer[@NodeText='" + LyrNT + "']");
            foreach (XmlNode pxn in xnl)
            {
                string nodeKey = pxn.Attributes["NodeKey"].Value;
                ILayer pLayer = GetLayerByNodeKey(inSourceMap, nodeKey);
                if (pLayer != null)
                    gLayer.Add(pLayer);
            }
            inMapLayers.InsertLayer(gLayer as ILayer, false, inMapLayers.LayerCount);
            cXmlDoc = null;
        }
        //区分行政区
        public static void AddMapOfByXZQ(IMap inMap, string inType, string ZTMC,IMap inSourceMap,string inXZQMC)
        {

            IMapLayers inMapLayers = inMap as IMapLayers;
            if (!File.Exists(LayerXMLpath))
            {
                return;
            }
            XmlDocument cXmlDoc = new XmlDocument();
            cXmlDoc.Load(LayerXMLpath);
            if (cXmlDoc == null)
                return;
            string xpath = "";
            if (ZTMC == "")//规划不分年度
                xpath = "//DIR[@DIRType='" + inType + "']";
            else//现状分年度专题
                xpath = "//DIR[@DIRType='" + inType + "' and @NodeText='" + ZTMC + "']";
            XmlNode xn = cXmlDoc.SelectSingleNode(xpath);
            if (xn == null)
                return;
            inMap.Name = xn.Attributes["NodeText"].Value;//设置地图名称
            string xzqmc = getXIAN(inXZQMC);//从不同级别的行政区获得县名称，以获得数据
            if (xzqmc == null)
                return;
            if (ZTMC != "")//区分现状和规划
            {
                string nxzq = ModGetData.AttrValueByXpath("//SpecialXZQ[@ZTMC='" + ZTMC + "']", "New");
                if (nxzq != "")
                {
                    if (nxzq.Contains(xzqmc))
                        xzqmc = ModGetData.AttrValueByXpath("//SpecialXZQ[@ZTMC='" + ZTMC + "']", "Old");

                    if (xzqmc == "")
                        return;

                }
            }
            XmlNode xnXZQ = xn.SelectSingleNode(".//DataDIR[contains(@NodeText,'" + xzqmc + "')]");
            if (xnXZQ == null)
                return;
            IGroupLayer gLayer = new GroupLayerClass();
            gLayer.Name = xnXZQ.Attributes["NodeText"].Value;
            XmlNodeList xnl = xnXZQ.SelectNodes(".//Layer");
            foreach (XmlNode pxn in xnl)
            {
                string nodeKey = pxn.Attributes["NodeKey"].Value;
                ILayer pLayer = GetLayerByNodeKey(inSourceMap, nodeKey);
                if(pLayer!=null)
                   gLayer.Add(pLayer);
            }
            if ((gLayer as ICompositeLayer).Count != 0)
            {
                if (!isExistGpLyr(inMap, gLayer.Name))
                   inMapLayers.InsertLayer(gLayer as ILayer, false, inMapLayers.LayerCount);
            }
            cXmlDoc = null;
        }
        private static bool isExistGpLyr(IMap inMap,string inGpLyrName)
        {
            bool res = false;
            for (int i = 0; i < inMap.LayerCount; i++)
            {
                if (inMap.get_Layer(i).Name == inGpLyrName)
                {
                    res = true;
                    break;
                }
 
            }
            return res;
        }
        //根据行政区名称获取对应县级数据节点
        private static string  getXIAN(string inXZQMC)
        {
            if (!File.Exists(xzqXml))
            {
                return null;
            }
            XmlDocument cXmlDoc = new XmlDocument();
            cXmlDoc.Load(xzqXml);
            if (cXmlDoc == null)
                return null;
            XmlNode xn = cXmlDoc.SelectSingleNode("//*[@ItemName='"+inXZQMC+"']");
            if (xn == null)
                return null;
            if (xn.Name == "County")
                return inXZQMC;
            else if (xn.Name == "Town")
                return xn.ParentNode.Attributes["ItemName"].Value;
            else if (xn.Name == "Village")
                return xn.ParentNode.ParentNode.Attributes["ItemName"].Value;
            return null;
 
        }
        //added by chulili 20110802褚丽丽添加函数,根据nodeKey获取地物类,直接读取数据源连接信息,读取地物类
        public static IFeatureClass GetFeatureClassByNodeKey(string strNodeKey)
        {
            if (strNodeKey.Equals(""))
            {
                return null;
            }
            //目录树路径变量:_layerTreePath
            XmlDocument pXmldoc = new XmlDocument();
            if (!File.Exists(LayerXMLpath))
            {
                return null;
            }
            //打开展示图层树,获取图层节点
            pXmldoc.Load(LayerXMLpath);
            string strSearch = "//Layer[@NodeKey=" + "'" + strNodeKey + "'" + "]";
            XmlNode pNode = pXmldoc.SelectSingleNode(strSearch);
            if (pNode == null)
            {
                return null;
            }
			IFeatureClass pFeaClass = null;
            //获取图层名,数据源id
            string strFeaClassName = "";
            string strDBSourceID = "";
            try
            {
                strFeaClassName = pNode.Attributes["Code"].Value;
                strDBSourceID = pNode.Attributes["ConnectKey"].Value;
            }
            catch
            { }
            //根据数据源id,获取数据源信息
            SysGisTable sysTable = new SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
            Exception eError = null;
            object objConnstr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "ID=" + strDBSourceID, out eError);
            string conninfostr = "";
            if (objConnstr != null)
            {
                conninfostr = objConnstr.ToString();
            }
            object objType = sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "ID=" + strDBSourceID, out eError);
            int type = -1;
            if (objType != null)
            {
                type = int.Parse(objType.ToString());
            }
            //根据数据源连接信息,获取数据源连接
            IWorkspace pWorkspace = GetWorkSpacefromConninfo(conninfostr, type);
            if (pWorkspace == null)
            {
                return null;
            }
            //打开地物类
            IFeatureWorkspace pFeaWorkSpace = pWorkspace as IFeatureWorkspace;
            //IFeatureClass pFeaClass = null;
            try
            {
                pFeaClass = pFeaWorkSpace.OpenFeatureClass(strFeaClassName);
            }
            catch
            { }
            return pFeaClass;

        }
        //added by chulili 20110730 根据nodeKey查找图层,
        //yjl20110930 modify 返回一个图层拷贝
        public static ILayer GetLayerByNodeKey(IMap pMap, string strNodeKey)
        {
            if (pMap == null) return null;
            if (strNodeKey.Equals(string.Empty)) return null;

            ILayer pSearchLayer = null;
            //循环子节点，比对NodeKey
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                ILayer pLayer = pMap.get_Layer(i);
                if (pLayer != null)
                {   //调用查找图层的函数
                    pSearchLayer = GetLayerByNodeKey(pMap, pLayer, strNodeKey);
                    if (pSearchLayer != null)
                    {
                        IObjectCopy pOC = new ObjectCopyClass();
                        ILayer tmpLayer = pOC.Copy(pSearchLayer) as ILayer;
                        return tmpLayer;
                    }
                }

            }
            return null;
        }
        //added by chulili 20110730根据NodeKey查找图层 递归调用
        private static ILayer GetLayerByNodeKey(IMap pMap, ILayer pLayer, string strNodeKey)
        {
            if (pMap == null)
                return null;
            if (pLayer == null)
                return null;
            if (strNodeKey.Equals(string.Empty)) return null;
            ILayer pSearchLayer = null;
            //遍历子节点
            IGroupLayer pGrouplayer = pLayer as IGroupLayer;
            if (pGrouplayer != null)
            {
                ICompositeLayer pComLayer = pGrouplayer as ICompositeLayer;
                for (int i = 0; i < pComLayer.Count; i++)
                {
                    ILayer pTmpLayer = pComLayer.get_Layer(i);
                    pSearchLayer = GetLayerByNodeKey(pMap, pTmpLayer, strNodeKey);
                    if (pSearchLayer != null)
                    {
                        return pSearchLayer;
                    }
                }
            }
            else
            {
                ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
                //读取图层的描述
                string strNodeXml = pLayerGenPro.LayerDescription;
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.LoadXml(strNodeXml);
                //构成xml节点，根据NodeKey在节点里查询
                string strSearch = "//Layer[@NodeKey=" + "'" + strNodeKey + "'" + "]";
                XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
                if (pNode != null)
                {
                    pXmlDoc = null;
                    return pLayer;
                }
            }
            return null;
        }
        
        //获取比例尺代码
        public static string GetDMofScale(string inScale)
        {
            string res = "";
            switch (inScale)
            {
                case"1:500000":
                    res = "B";
                    break;
                case "1:250000":
                    res = "C";
                    break;
                case "1:100000":
                    res = "D";
                    break;
                case "1:50000":
                    res = "E";
                    break;
                case "1:25000":
                    res = "F";
                    break;
                case "1:10000":
                    res = "G";
                    break;
                case "1:5000":
                    res = "H";
                    break;
                case "1:2000":
                    res = "I";
                    break;
                case "1:1000":
                    res = "J";
                    break;
                case "1:500":
                    res = "K";
                    break;
            }
            return res;
        }
        public static string getWhereByXZQ(DevComponents.AdvTree.Node vSelectNode)
        {
            if (vSelectNode == null)
            {
                return "";
            }
            string strCode = vSelectNode.Name;
            string strWhere = "";
            switch (strCode.Length)
            {
                case 2:
                    strWhere = "SHENG='"+strCode+"'";
                    break;
                case 4:
                    strWhere = "SHI='" + strCode + "'";
                    break;
                case 6:
                    strWhere = "XIAN='" + strCode + "'";
                    break;
                case 8:
                    strWhere = "XIANG='0" + strCode.Substring(strCode.Length - 3, 2) + "' AND XIAN='"+strCode.Substring(0,strCode.Length-2)+"'";
                    break;

            }
            return strWhere;
        }
        //由行政区树获得行政区范围
        public static IGeometry getExtentByXZQ(DevComponents.AdvTree.Node vSelectNode)
        {
            string XZQpath = Application.StartupPath + "\\..\\Res\\Xml\\XZQ.xml";
            string strTag = vSelectNode.Tag.ToString();
            if (File.Exists(XZQpath))
            {
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.Load(XZQpath);

                XmlNode pNode = pXmldoc.SelectSingleNode("//LayerConfig/" + strTag);
                XmlElement pEle = pNode as XmlElement;
                if (pEle != null)
                {
                    string strNodeKey = "";
                    string strXZBM = "";
                    if (pEle.HasAttribute("NodeKey"))
                    {
                        strNodeKey = pEle.GetAttribute("NodeKey");
                    }
                    if (pEle.HasAttribute("XZBMField"))
                    {
                        strXZBM = pEle.GetAttribute("XZBMField");
                    }
                    pXmldoc = null;
                    IFeatureClass pFeatureClass = GetFeatureClassByNodeKey(strNodeKey);
                    return getXZQExtentFromFL(pFeatureClass, vSelectNode.Name, strXZBM);
                }
                pXmldoc = null;

            }
            //if (vSelectNode.Parent != null)
            //{
            //    DevComponents.AdvTree.Node vParentNode = vSelectNode.Parent;
            //    if (vParentNode.DataKey != null)
            //    {
            //        XmlNode pNode = vParentNode.DataKey as XmlNode;
            //        if ((pNode as XmlElement) != null)
            //        {
            //            XmlElement pNodeEle = pNode as XmlElement;
            //            if (pNodeEle.HasAttribute("NodeKey") && pNodeEle.HasAttribute("FieldXZBM"))
            //            {
            //                string strNodeKey = pNodeEle.GetAttribute("NodeKey");
            //                string strField = pNodeEle.GetAttribute("FieldXZBM");
            //                IFeatureClass pFeatureClass = GetFeatureClassByNodeKey(strNodeKey);
            //                return getXZQExtentFromFL(pFeatureClass, vSelectNode.Name, strField);
            //            }
            //        }
            //    }
            //}
            return null;
        }
        //由行政区树获取行政区单位名称
        public static string GetUnitName(DevComponents.AdvTree.Node vSelectNode)
        {
            string XZQpath = Application.StartupPath + "\\..\\Res\\Xml\\XZQ.xml";
            string strTag = vSelectNode.Tag.ToString();
            string strXZBM = "";
            if (File.Exists(XZQpath))
            {
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.Load(XZQpath);

                XmlNode pNode = pXmldoc.SelectSingleNode("//LayerConfig/" + strTag);
                XmlElement pEle = pNode as XmlElement;
                if (pEle != null)
                {
                    
                    if (pEle.HasAttribute("XZBMField"))
                    {
                        strXZBM = pEle.GetAttribute("XZBMField");
                    }
                    pXmldoc = null;
                }
                pXmldoc = null;

            }
            return strXZBM;
        }
        //由行政区树获得行政区要素类和行政区编码字段
        public static IFeatureClass getFCByXZQ(DevComponents.AdvTree.Node vSelectNode, ref string outXZBMfd)
        {
            string XZQpath = Application.StartupPath + "\\..\\Res\\Xml\\XZQ.xml";
            string strTag = vSelectNode.Tag.ToString();
            outXZBMfd = "";
            if (File.Exists(XZQpath))
            {
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.Load(XZQpath);

                XmlNode pNode = pXmldoc.SelectSingleNode("//LayerConfig/" + strTag);
                XmlElement pEle = pNode as XmlElement;
                if (pEle != null)
                {
                    string strNodeKey = "";
                    string strXZBM = "";
                    if (pEle.HasAttribute("NodeKey"))
                    {
                        strNodeKey = pEle.GetAttribute("NodeKey");
                    }
                    if (pEle.HasAttribute("XZBMField"))
                    {
                        strXZBM = pEle.GetAttribute("XZBMField");
                    }
                    pXmldoc = null;
                    outXZBMfd = strXZBM;
                    IFeatureClass pFeatureClass = GetFeatureClassByNodeKey(strNodeKey);
                    return pFeatureClass;
                }
                pXmldoc = null;

            }
            //if (vSelectNode.Parent != null)
            //{
            //    DevComponents.AdvTree.Node vParentNode = vSelectNode.Parent;
            //    if (vParentNode.DataKey != null)
            //    {
            //        XmlNode pNode = vParentNode.DataKey as XmlNode;
            //        if ((pNode as XmlElement) != null)
            //        {
            //            XmlElement pNodeEle = pNode as XmlElement;
            //            if (pNodeEle.HasAttribute("NodeKey") && pNodeEle.HasAttribute("FieldXZBM"))
            //            {
            //                string strNodeKey = pNodeEle.GetAttribute("NodeKey");
            //                string strField = pNodeEle.GetAttribute("FieldXZBM");
            //                IFeatureClass pFeatureClass = GetFeatureClassByNodeKey(strNodeKey);
            //                return getXZQExtentFromFL(pFeatureClass, vSelectNode.Name, strField);
            //            }
            //        }
            //    }
            //}
            return null;
        }
        //根据行政区图层，行政编码字段名称和字段值，获取对应的地物几何数据
        private static  IGeometry getXZQExtentFromFL(IFeatureClass pXZQFeaCls, string strXZQBM, string strFieldXZQBM)
        {

            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                if (pXZQFeaCls != null)
                {//查找行政编码属性列
                    int iIndex = pXZQFeaCls.Fields.FindField(strFieldXZQBM);
                    IField pField = pXZQFeaCls.Fields.get_Field(iIndex);
                    //构造过滤条件
                    if (pField.Type.ToString() == "esriFieldTypeString")
                    {

                        pQueryFilter.WhereClause = strFieldXZQBM + "='" + strXZQBM + "'";
                    }
                    else if (pField.Type.ToString() == "esriFieldTypeDouble")
                    {
                        pQueryFilter.WhereClause = strFieldXZQBM + "=" + strXZQBM;
                    }
                    //end
                    //查找
                    IFeatureCursor pFCursor = pXZQFeaCls.Search(pQueryFilter, false);
                    IFeature pFeature = pFCursor.NextFeature();
                    //只获取找到的第一个地物
                    if (pFeature != null)
                    {
                        pFCursor = null;
                        pQueryFilter = null;
                        return pFeature.ShapeCopy;
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFCursor);
                    pFCursor = null;
                    pQueryFilter = null;
                }

            }
            catch
            {

            }

            return null;
        }
        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        private static IWorkspace GetWorkSpacefromConninfo(string conninfostr, int type)
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
                    pWSFact = new AccessWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 2://gdb
                    pWSFact = new FileGDBWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 3://sde
                    pWSFact = new SdeWorkspaceFactoryClass();
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
    }
}
