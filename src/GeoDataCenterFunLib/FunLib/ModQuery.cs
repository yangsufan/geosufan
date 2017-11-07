using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using SysCommon.Authorize;

using ESRI.ArcGIS.Carto;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
namespace GeoDataCenterFunLib
{
    public static class ModQuery
    {

        private static string _layerTreePath = Application.StartupPath + "\\..\\res\\xml\\展示图层树.xml";//added by chulili 20110802 褚丽丽添加变量,展示图层树路径,专门用来查找道路,河流,地名地物类
        //deleted by chulili 20110818 统一使用属性值自动映射字典表进行属性值映射
        //public static IDictionary<string, string> _DicGBname = new Dictionary<string, string>();    //国标对照表
        //public static IDictionary<string, string> _DicClassname = new Dictionary<string, string>(); //地名分类对照表
        
        //public static IDictionary<string, string> _DicXZQname = new Dictionary<string, string>(); //行政区编码表
        //行政区编码表，表格内容按照特定结构存储，表结构（ID,CODE,NAME）
        //CODE:行政区编码 NAME：行政区名称 
        //CODE的位数，省级2位 市级4位 县级6位
        public readonly static string m_QueryPath = Application.StartupPath + "\\..\\Template\\QueryConfig.Xml";
        public readonly static string m_StatisticsPath = Application.StartupPath + "\\..\\Template\\StatisticsConfig.Xml";

        
        //获取数据库的数据类型（ORACLE MDB GDB）
        public static string GetDescriptionOfWorkspace(IWorkspace pWorkspace)
        {
            if (pWorkspace == null)
            {
                return "";
            }
            IWorkspaceFactory pWorkSpaceFac = pWorkspace.WorkspaceFactory;
            if (pWorkSpaceFac == null)
            {
                return "";
            }
            string strDescrip=pWorkSpaceFac.get_WorkspaceDescription(false);
            return strDescrip;
        }
       
        //added by chulili 20110802 褚丽丽添加函数,获取道路地物类,不从map中查找,直接从xml中查找信息后,获取地物类
        public static void GetQueryConfig(string strNodeName,out IFeatureClass pFeatureClass,out string strLayerName, out string strFieldname,out string strFieldCode)
        {
            pFeatureClass = null;
            strFieldname = "";//yjl0730 return前 out参数必须赋值
            strFieldCode = "";
            strLayerName = "";
            if (!File.Exists(m_QueryPath))
            {
                return;
            }
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(m_QueryPath);
            string strSearch = "//QueryConfig/QueryItem[@ItemText=" + "'" + strNodeName + "'" + "]";
            XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
            if (pNode == null)
            {
                return;
            }
            XmlNode pLayerNode = pNode.SelectSingleNode(".//LayerItem");
            string strNodeKey = "";
            if (pLayerNode != null)
            {
                strNodeKey = pLayerNode.Attributes["NodeKey"].Value;
            }
            XmlNode pFieldCodeNode = pNode.SelectSingleNode(".//FieldItem[@ItemText='编码']");
            string strField = "";
            string strCode = "";
            if (pFieldCodeNode != null)
            {
                strCode = pFieldCodeNode.Attributes["FieldName"].Value;
            }
            
            XmlNode pFieldNameNode = pNode.SelectSingleNode(".//FieldItem[@ItemText='名称']");
            if (pFieldNameNode != null)
            {
                strField = pFieldNameNode.Attributes["FieldName"].Value;
            }
            IFeatureClass pFeaClass = GetFeatureClassByNodeKey(strNodeKey, out strLayerName);

            pFeatureClass = pFeaClass;
            strFieldname = strField;
            strFieldCode = strCode;
        }
        public static IFeatureClass GetFeatureClassByNodeKey( string strNodeKey, out string strLayerName)
        {
            IFeatureClass  pFeatureClass = GetFeatureClassByNodeKey(_layerTreePath, strNodeKey, out strLayerName);
            return pFeatureClass;
        }

        //added by chulili 20110802褚丽丽添加函数,根据nodeKey获取地物类,直接读取数据源连接信息,读取地物类
        public static IFeatureClass GetFeatureClassByNodeKey(string LayerTreeXmlPath,string strNodeKey,out string strLayerName)
        {
            strLayerName = "";
            if (strNodeKey == "")
            {
                return null;
            }
            //目录树路径变量:_layerTreePath
            strLayerName = "";
            XmlDocument pXmldoc = new XmlDocument();
            if (!File.Exists(LayerTreeXmlPath))
            {
                return null;
            }
            //打开展示图层树,获取图层节点
            pXmldoc.Load(LayerTreeXmlPath);
            string strSearch = "//Layer[@NodeKey=" + "'"+strNodeKey+"'" + "]";
            XmlNode pNode = pXmldoc.SelectSingleNode(strSearch );
            if (pNode == null)
            {
                return null;
            }
            //获取图层名,数据源id
            string strFeaClassName = ""; 
            string strDBSourceID = "";
            try
            {
                strLayerName=pNode.Attributes["NodeText"].Value;
                strFeaClassName=pNode.Attributes["Code"].Value;
                strDBSourceID=pNode.Attributes["ConnectKey"].Value;
            }
            catch
            { }
            //根据数据源id,获取数据源信息
            SysGisTable sysTable = new SysGisTable(Plugin.ModuleCommon.TmpWorkSpace );
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
            IFeatureClass pFeaClass = null;
            try
            {
                pFeaClass = pFeaWorkSpace.OpenFeatureClass(strFeaClassName);
            }
            catch 
            { }
            return pFeaClass;

        }


        //added by chulili 20110802褚丽丽添加函数,根据nodeKey获取地物类,直接读取数据源连接信息,读取地物类
        public static IFeatureClass GetFeatureClassByNodeKey(string strNodeKey)
        {
            string strName = "";
            IFeatureClass pFeatureClass = GetFeatureClassByNodeKey(strNodeKey, out strName);
            return pFeatureClass;

        }

        //added by chulili 20110730 根据nodeKey查找图层
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
                        return pSearchLayer;
                    }
                }

            }
            return null;
        }
        //added by chulili 20110730根据NodeKey查找图层 递归调用
        private static ILayer GetLayerByNodeKey(IMap pMap, ILayer pLayer, string strNodeKey)
        {
            if (pMap==null)
                return null;
            if (pLayer==null)
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
                pXmlDoc.LoadXml(strNodeXml );
                //构成xml节点，根据NodeKey在节点里查询
                string strSearch = "//Layer[@NodeKey=" + "'"+strNodeKey +"'" + "]";
                XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
                if (pNode != null)
                {
                    pXmlDoc = null;
                    return pLayer;
                }
            }
            return null;
        }
        //changed by chulili 20110731 修改获取地名查询参数的函数
        public static void GetPlaceNameQueryConfig(IMap pMap, out ILayer pXZNameLayer, out ILayer pZRNameLayer, out string strDMname)
        {
            if (!File.Exists(m_QueryPath))
            {
                pXZNameLayer = null;
                pZRNameLayer = null;
                //strDMFLName = "";//changed by chulili 20110825地名查询现在用不到地名分类字段
                strDMname = "";//yjl0730 return前 out参数必须赋值
                return;
            }
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(m_QueryPath);
            string strSearch = "//QueryItem[@Name=" + "'地名查询'" + "]";
            XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
            if (pNode == null)
            {
                pXZNameLayer = null;
                pZRNameLayer = null;
                //strDMFLName = "";
                strDMname = "";//yjl0730 return前 out参数必须赋值
                return;
            }
            string strNodeKey_XZ = pNode.Attributes["XZFeatureClassKey"].Value;//行政地名图层名
            string strNodeKey_ZR = pNode.Attributes["ZRFeatureClassKey"].Value;//自然地名图层名
            string strField = pNode.Attributes["FieldDM"].Value;
            //string strFieldFL = pNode.Attributes["FieldDMFL"].Value;
            ILayer pLayer_XZ = GetLayerByNodeKey(pMap, strNodeKey_XZ);
            ILayer pLayer_ZR = GetLayerByNodeKey(pMap ,strNodeKey_ZR);
            pXZNameLayer = pLayer_XZ;
            pZRNameLayer = pLayer_ZR;
            strDMname = strField;
            //strDMFLName = strFieldFL;
        }
       /// <summary>
       /// 根据配置文件获取图层和字段名
       /// </summary>
       /// <param name="pNameClass">要素</param>
       /// <param name="strLayerName">名称</param>
       /// <param name="strFieldname">字段名</param>
       /// <param name="strTypeName">统计类型</param>
        public static void GetPlaceNameStatisticsConfig(out IFeatureClass pNameClass, out string strLayerName, out string strFieldname, string strTypeName)
        {
            strLayerName = "";
            pNameClass = null;
            strFieldname = "";//yjl0730 return前 out参数必须赋值
            if (!File.Exists(m_StatisticsPath))
            {
                return;
            }
            try
            {
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.Load(m_StatisticsPath);
                string strSearch = "//StatisticsConfig/StatisticsItem[@ItemText= '" + strTypeName + "']";
                XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
                if (pNode == null)
                {
                    return;
                }
                XmlNodeList pNodeList = pNode.SelectNodes(".//LayerItem");
                string strNodeKey = "";
                if (pNodeList.Count > 0)
                {
                    XmlNode pXZnode = pNodeList[0];
                    strNodeKey = pXZnode.Attributes["NodeKey"].Value;//行政地名图层名
                }
                XmlNode pFieldNode = pNode.SelectSingleNode(".//FieldItem");
                string strField = "";
                if (pFieldNode != null)
                {
                    strField = pFieldNode.Attributes["FieldName"].Value;
                }
                //string strFieldFL = pNode.Attributes["FieldDMFL"].Value;
                IFeatureClass pFeaClass = ModGetData.GetFeatureClassByNodeKey(strNodeKey);
                strLayerName = (pFeaClass as IDataset).Name;
                pNameClass = pFeaClass;
                strFieldname = strField;
            }
            catch { }
        }
        //changed by chulili 20110802 添加获取地名地物类的函数
        public static void GetPlaceNameQueryConfig(IMap pMap,out List<string> pListNodeKeys, out List<ILayer> pListLayers,out List<IFeatureClass> pListFeatureClasses,out List<string> pListLayerNames,out string strDMname)
        {
            pListLayers = null;
            pListFeatureClasses = null;
            pListLayerNames = null;
            pListNodeKeys = null;
            strDMname = "";
            if (!File.Exists(m_QueryPath))
            {
                return;
            }

            try
            {
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.Load(m_QueryPath);
                string strSearch = "//QueryConfig/QueryItem[@ItemText=" + "'地名查询'" + "]";
                XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
                if (pNode == null)
                {
                    return;
                }
                XmlNodeList pNodeList = pNode.SelectNodes(".//LayerItem");
                for (int i = 0; i < pNodeList.Count; i++)
                {
                    if (pListLayers == null)
                    {
                        pListLayers = new List<ILayer>();
                    }
                    if (pListFeatureClasses == null)
                    {
                        pListFeatureClasses = new List<IFeatureClass>();
                    }
                    if (pListLayerNames == null)
                    {
                        pListLayerNames = new List<string>();
                    }
                    if (pListNodeKeys == null)
                    {
                        pListNodeKeys = new List<string>();
                    }
                    XmlNode pTmpNode = pNodeList[i];
                    string strNodeKey = pTmpNode.Attributes["NodeKey"].Value;
                    ILayer pLayer = GetLayerByNodeKey(pMap, strNodeKey);
                    IFeatureClass pFeatureClass = null;
                    string strName = "";
                    if (pLayer != null)
                    {
                        strName = pLayer.Name;
                        try
                        {
                            pFeatureClass = (pLayer as IFeatureLayer).FeatureClass;
                        }
                        catch
                        { }
                    }
                    else
                    {
                        pFeatureClass = GetFeatureClassByNodeKey(strNodeKey, out strName);
                    }
                    if (pFeatureClass != null)
                    {
                        pListLayers.Add(pLayer);
                        pListFeatureClasses.Add(pFeatureClass);
                        pListLayerNames.Add(strName);
                        pListNodeKeys.Add(strNodeKey);
                    }
                }

                XmlNode pFieldNode = pNode.SelectSingleNode(".//FieldItem");
                string strField = "";
                if (pFieldNode != null)
                {
                    strField = pFieldNode.Attributes["FieldName"].Value;
                }
                strDMname = strField;
            }
            catch
            { }
        }
        //changed by chulili 20110802 添加获取地名地物类的函数
        public static void GetPlaceNameQueryConfig( out IFeatureClass  pXZNameClass, out IFeatureClass  pZRNameClass, out string XZLayerName,out string ZRLayerName, out string strDMname)
        {
            XZLayerName = "";
            ZRLayerName = "";
            pXZNameClass = null;
            pZRNameClass = null;
            //strDMFLName = ""; //changed by chulili 20110825地名查询现在用不到地名分类字段
            strDMname = "";//yjl0730 return前 out参数必须赋值
            if (!File.Exists(m_QueryPath))
            {         
                return;
            }
            try
            {
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.Load(m_QueryPath);
                string strSearch = "//QueryConfig/QueryItem[@ItemText=" + "'地名查询'" + "]";
                XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
                if (pNode == null)
                {
                    return;
                }
                XmlNodeList  pNodeList = pNode.SelectNodes(".//LayerItem");
                string strNodeKey_XZ = "";
                string strNodeKey_ZR = "";
                if (pNodeList.Count > 0)
                {
                    XmlNode pXZnode = pNodeList[0];
                    strNodeKey_XZ = pXZnode.Attributes["NodeKey"].Value;//行政地名图层名
                    
                }
                if (pNodeList.Count > 1)
                {
                    XmlNode pXZnode = pNodeList[1];
                    strNodeKey_ZR = pXZnode.Attributes["NodeKey"].Value;//自然地名图层名
                }
                XmlNode pFieldNode = pNode.SelectSingleNode(".//FieldItem");
                string strField = "";
                if (pFieldNode != null)
                {
                    strField = pFieldNode.Attributes["FieldName"].Value;
                }
                //string strFieldFL = pNode.Attributes["FieldDMFL"].Value;
                IFeatureClass pFeaClass_XZ = GetFeatureClassByNodeKey(strNodeKey_XZ, out XZLayerName);
                IFeatureClass pFeaClass_ZR = GetFeatureClassByNodeKey(strNodeKey_ZR, out ZRLayerName);
                pXZNameClass = pFeaClass_XZ;
                pZRNameClass = pFeaClass_ZR;
                //XZLayerName = strXZlayer;
                //ZRLayerName = strZRlayer;
                strDMname = strField;
                //strDMFLName = strFieldFL;
            }
            catch
            { }
        }
        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        private static IWorkspace GetWorkSpacefromConninfo(string conninfostr, int type)
        {
            if (conninfostr == "")
            {
                return null;
            }
            if (type < 0)
            {
                return null;
            }
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