using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace SysCommon
{
    public static class ModField
    {
        private static string _HideFieldConfigPath = Application.StartupPath + "\\..\\res\\xml\\MatchFieldConfig.xml";
        public static string _XZQFieldName = "";//行政区字段名
        public static IDictionary<string, string> _DicFieldName = new Dictionary<string, string>();   //属性名称对照表
        public static string _MatchFieldValuepath = Application.StartupPath + "\\..\\res\\xml\\MatchFieldConfig.xml";//added by chulili 20110818 属性值对照配置文件路径
        public static IDictionary<string, IDictionary<string, string>> _DicMatchFieldValue = new Dictionary<string, IDictionary<string, string>>();//属性值自动映射字典表，此表中包括所有属性值对照字典表
        public static IDictionary<string, string> _DicLayerName = new Dictionary<string, string>();//标准图层代码表，数据字典    ZQ 20111020  add
        public static List<string> _ListHideFields = null;//隐藏字段名称的链表，设置主显字段时，避开隐藏字段

        public static void GetHideFields()
        {
            if (_ListHideFields == null)
            {
                _ListHideFields = new List<string>();
            }
            if (!File.Exists(_HideFieldConfigPath))
                return;

            //打开配置文件
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_HideFieldConfigPath);
            //查找隐藏字段配置的主节点
            string strSearch = "//HideFieldConfig";
            XmlNode pXmlnode = pXmldoc.SelectSingleNode(strSearch);
            if (pXmlnode == null)
                return;
            if (!pXmlnode.HasChildNodes)
            {
                return;
            }
            //读取隐藏的各个字段名称
            for (int i = 0; i < pXmlnode.ChildNodes.Count; i++)
            {
                XmlNode pChildNode = pXmlnode.ChildNodes[i];
                XmlElement pChildEle = pChildNode as XmlElement;
                if (pChildEle != null)
                {
                    if (pChildEle.HasAttribute("FieldName"))
                    {
                        try
                        {
                            _ListHideFields.Add(pChildEle.GetAttribute("FieldName").ToUpper());
                        }
                        catch
                        { }
                    }
                }
            }
        }

        public static void InitMatchFieldValueDic(IWorkspace pWorkSpace)
        {
            if (!File.Exists(_MatchFieldValuepath))
                return;
            //读取配置文件
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_MatchFieldValuepath);
            string strSearch = "//MatchFieldConfig";
            XmlNode pNode = pXmldoc.SelectSingleNode(strSearch);
            if (pNode == null)
                return;
            if (!pNode.HasChildNodes)
                return;
            for (int i = 0; i < pNode.ChildNodes.Count; i++)
            {
                XmlNode pChildNode = pNode.ChildNodes[i];
                XmlElement pChildEle = pChildNode as XmlElement;
                if (pChildEle != null)
                {
                    //单独记录下行政区的属性字段名
                    if (pChildEle.Name.Equals("XZQField"))
                    {
                        if (pChildEle.HasAttribute("FieldName"))
                        {
                            _XZQFieldName = pChildEle.GetAttribute("FieldName");
                        }
                    }
                    //获取字段名、字典表名称
                    string strFieldName = "";
                    string strTableName = "";
                    if (pChildEle.HasAttribute("FieldName"))
                    {
                        strFieldName = pChildEle.GetAttribute("FieldName");
                    }
                    if (pChildEle.HasAttribute("TableName"))
                    {
                        strTableName = pChildEle.GetAttribute("TableName");
                    }
                    //将字典表内容添加到字典中
                    if (strTableName != "" && strFieldName != "")
                    {
                        IDictionary<string, string> DicFieldValue = new Dictionary<string, string>();
                        InitNameDic(pWorkSpace, DicFieldValue, strTableName);
                        if (DicFieldValue.Count > 0)
                        {
                            //将字典添加到总的字段值匹配字典中
                            _DicMatchFieldValue.Add(strFieldName, DicFieldValue);
                        }
                        else
                        {
                            DicFieldValue = null;
                        }
                    }

                }
            }
        }
        //added by chulili 20110818 获得字段的中文名
        public static string GetChineseOfFieldValue(string strFieldName, string strFieldValue)
        {
            string strChineseName = strFieldValue;
            if (strFieldName == "")
                return strChineseName;
            if (!_DicMatchFieldValue.Keys.Contains(strFieldName))
                return strChineseName;
            if (strFieldName == _XZQFieldName)
                return GetChineseNameOfXZQ(strFieldValue);
            IDictionary<string, string> DicFieldValue = _DicMatchFieldValue[strFieldName];
            if (DicFieldValue == null)
                return strChineseName;
            if (DicFieldValue.Keys.Contains(strFieldValue))
            {
                strChineseName = DicFieldValue[strFieldValue];
                return strChineseName + "【" + strFieldValue + "】";
            }
            return strChineseName;
        }

        public static string GetDomainValueOfFieldValue(IFeatureClass pFeatureClass, string strFieldName, string strFieldValue)
        {
            string strValue = strFieldValue;
            try
            {
               // IDataset dataset = pFeatureClass as IDataset;
               //IWorkspace workspace = dataset.Workspace;
               // IWorkspaceDomains workspaceDomains = (IWorkspaceDomains)workspace;
                int indexfield = pFeatureClass.Fields.FindField(strFieldName);
                IField pField = pFeatureClass.Fields.get_Field(indexfield);
                IDomain requestDomain = pField.Domain;
                ICodedValueDomain codedValueDomain = (ICodedValueDomain)requestDomain;
                if(codedValueDomain!=null)
                {
                    for (int i = 0; i < codedValueDomain.CodeCount; i++)
                    {
                        string strTmpFieldValue = codedValueDomain.get_Value(i).ToString();
                        if (strTmpFieldValue.Equals(strFieldValue))
                        {
                            strValue = codedValueDomain.get_Name(i);
                            break;
                        }

                    }
                }
            }
            catch
            { }
            return strValue;
        }
        //added by chulili 20110810 获得行政区的中文名称
        public static string GetChineseNameOfXZQ(string strXZQCode)
        {
            string strChineseName = strXZQCode;
            if (_XZQFieldName == "")
                return strChineseName;
            if (!_DicMatchFieldValue.Keys.Contains(_XZQFieldName))
            {
                return strChineseName;
            }
            IDictionary<string, string> DicXZQName = _DicMatchFieldValue[_XZQFieldName];
            if (DicXZQName == null)
                return strChineseName;
            string strProvinceCode = strXZQCode.Substring(0, 2);//取前两位，省名
            if (DicXZQName.Keys.Contains(strProvinceCode))
            {
                strChineseName = DicXZQName[strProvinceCode];
            }
            string strCityCode = strXZQCode.Substring(0, 4);//取前四位，市名
            if (DicXZQName.Keys.Contains(strCityCode))
            {
                strChineseName = strChineseName + DicXZQName[strCityCode];
            }
            if (DicXZQName.Keys.Contains(strXZQCode))//所有位数，6位，县名
            {
                strChineseName = strChineseName + DicXZQName[strXZQCode];
            }
            return strChineseName + "【" + strXZQCode + "】";
        }

        //added by chulili 20110731 获得字段的中文名
        public static string GetChineseNameOfField(string strFieldName)
        {
            string strChineseName = strFieldName;
            if (_DicFieldName == null)
                return strChineseName;
            if (_DicFieldName.Keys.Contains(strFieldName))
            {
                strChineseName = _DicFieldName[strFieldName];
            }
            return strChineseName;
        }
        //added by chulili 20110730初始化属性名称数据字典（英文名映射中文名）key:英文  value:中文
        public static void InitNameDic(IWorkspace pConfigWks, IDictionary<string, string> DicFieldname, string DicTableName)
        {

            Exception exError = null;
            if (DicFieldname == null)
            {
                DicFieldname = new Dictionary<string, string>();//changed by chulili 20110731
            }
            DicFieldname.Clear();
            SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(pConfigWks);
            List<Dictionary<string, object>> lstDicData = sysTable.GetRows(DicTableName, "", out exError);
            if (lstDicData == null)
                return;
            try
            {
                if (lstDicData.Count > 0)
                {
                    for (int i = 0; i < lstDicData.Count; i++)
                    {
                        string strName = "";
                        string strAliasName = "";
                        if (lstDicData[i]["CODE"] != null)
                            strName = lstDicData[i]["CODE"].ToString();
                        if (lstDicData[i]["NAME"] != null)
                            strAliasName = lstDicData[i]["NAME"].ToString();
                        //将属性名及别名添加到字典中
                        if ((!strName.Equals("")) && (!strAliasName.Equals("")))
                        {
                            if (!DicFieldname.Keys.Contains(strName))
                            {
                                DicFieldname.Add(strName, strAliasName);
                            }
                        }
                    }
                }
            }
            catch
            { }
        }
        //初始化图层名称数据字典（英文名映射中文名）key:英文  value:中文    ZQ  20111020   add
        public static void InitLayerNameDic(IWorkspace pConfigWks, IDictionary<string, string> DicLayername)
        {
            Exception exError = null;
            DicLayername.Clear();
            SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(pConfigWks);
            List<Dictionary<string, object>> lstDicData = sysTable.GetRows("标准图层代码表", "", out exError);
            if (lstDicData == null)
                return;
            try
            {
                if (lstDicData.Count > 0)
                {
                    for (int i = 0; i < lstDicData.Count; i++)
                    {
                        string strName = "";
                        string strAliasName = "";
                        if (lstDicData[i]["CODE"] != null)
                            strName = lstDicData[i]["CODE"].ToString();
                        if (lstDicData[i]["NAME"] != null)
                            strAliasName = lstDicData[i]["NAME"].ToString();
                        //将图层名及别名添加到字典中
                        if ((!strName.Equals("")) && (!strAliasName.Equals("")))
                        {
                            if (!DicLayername.Keys.Contains(strName))
                            {
                                DicLayername.Add(strName, strAliasName);
                            }
                        }
                    }
                }
            }
            catch
            { }
        }
    }
}
