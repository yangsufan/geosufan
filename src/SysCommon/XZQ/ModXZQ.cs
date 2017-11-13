using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;

using ESRI.ArcGIS.Geodatabase;


namespace SysCommon
{
    public static class ModXZQ
    {
        private static string m_DicConfigpat = Application.StartupPath + "\\..\\res\\xml\\Dictionary.xml";
        /// <summary>
        /// 获取系统参数值，传入业务库工作空间和参数名称
        /// </summary>
        /// <param name="TmpWorkspace"></param>
        /// <param name="SettingName"></param>
        /// <returns>strSettingValue</returns>
        public static string GetXzqName(IWorkspace TmpWorkspace, string XZQcode)
        {
            string strXZQname = XZQcode;
            if (TmpWorkspace == null)
                return strXZQname;
            //使用SysGisTable类获取系统表中的记录
            Gis.SysGisTable sysTable = new Gis.SysGisTable(TmpWorkspace);

            Exception eError = null;
            //根据参数名获取记录
            Dictionary<string, object> dicData = sysTable.GetRow("行政区字典表", "CODE='" + XZQcode + "'", out eError);
            //获取参数值
            if (dicData != null)
            {
                if (dicData.Count > 0)
                {
                    if (dicData.ContainsKey("NAME"))
                    {
                        if (dicData["NAME"] != null)
                        {
                            strXZQname = dicData["NAME"].ToString();
                        }
                    }
                }
            }
            dicData = null;
            sysTable = null;
            return strXZQname;

        }
        public static string GetXzqCode(IWorkspace TmpWorkspace, string XZQName)
        {
            string strXZQcode = "";
            if (TmpWorkspace == null)
                return strXZQcode;
            //使用SysGisTable类获取系统表中的记录
            Gis.SysGisTable sysTable = new Gis.SysGisTable(TmpWorkspace);

            Exception eError = null;
            //根据参数名获取记录
            Dictionary<string, object> dicData = sysTable.GetRow("行政区字典表", "NAME='" + XZQName + "'", out eError);
            //获取参数值
            if (dicData != null)
            {
                if (dicData.Count > 0)
                {
                    if (dicData.ContainsKey("CODE"))
                    {
                        if (dicData["CODE"] != null)
                        {
                            strXZQcode = dicData["CODE"].ToString();
                        }
                    }
                }
            }
            dicData = null;
            sysTable = null;
            return strXZQcode;

        }
        //ygc 20130318 添加字典翻译
        public static string GetChineseName(IWorkspace TmpWorkspace,string FieldName,string FieldValue)
        {
            string ChineseValue = "";
            if (TmpWorkspace == null) return FieldValue;
            if (FieldValue == "" || FieldValue == string.Empty) return FieldValue;
            //根据xml文件读入字典表
            if (!File.Exists(m_DicConfigpat)) return FieldValue;
            XmlDocument dicXml = new XmlDocument();
            dicXml.Load(m_DicConfigpat);
            //获取字典表名称
            XmlNode xmlNode = dicXml.SelectSingleNode("Root/Item[@FieldName='" + FieldName + "']");
            if (xmlNode == null) return FieldValue;
            string TableName = xmlNode.Attributes["TableName"].Value.ToString();

            //使用SysGisTable类获取系统表中的记录
            Gis.SysGisTable sysTable = new Gis.SysGisTable(TmpWorkspace);
            Exception eError = null;
            //对行政区属性进行调整
            if (FieldName == "SHENG")
            {
                FieldValue = FieldValue.Substring(0, 2);
            }
            else if(FieldName =="SHI")
            {
                if (FieldValue.Length > 4)
                {
                    FieldValue = FieldValue.Substring(0, 4);
                }
            }
            else if (FieldName == "XIANG")
            {
                if (FieldValue.Length > 8)
                {
                    FieldValue = FieldValue.Substring(0, 8);
                }
            }
            //根据参数名获取记录
            Dictionary<string, object> dicData = sysTable.GetRow(TableName, "CODE='" + FieldValue + "'", out eError);
            //获取参数值
            if (dicData == null)
            {
                return FieldValue;
            }
            else
            {
                if (dicData.Count > 0)
                {
                    if (dicData.ContainsKey("NAME"))
                    {
                        if (dicData["NAME"] != null)
                        {
                            ChineseValue = dicData["NAME"].ToString();
                        }
                    }
                }
            }
            dicData = null;
            sysTable = null;
            return ChineseValue;
        }
        //ygc 20130326 获取指定字段字典
        public static List<string> GetListChineseName(IWorkspace TmpWorkspace, string FieldName)
        {
            List<string> NewList = new List<string>();
            if (TmpWorkspace == null) return NewList;
            //根据xml文件读入字典表
            if (!File.Exists(m_DicConfigpat)) return NewList;

            XmlDocument dicXml = new XmlDocument();
            dicXml.Load(m_DicConfigpat);
            //获取字典表名称
            XmlNode xmlNode = dicXml.SelectSingleNode("Root/Item[@FieldName='" + FieldName + "']");
            if (xmlNode == null) return NewList;
            string TableName = xmlNode.Attributes["TableName"].Value.ToString();

            //使用SysGisTable类获取系统表中的记录
            Gis.SysGisTable sysTable = new Gis.SysGisTable(TmpWorkspace);
            Exception eError = null;
            List<Dictionary<string, object>> listDic = null;
            if (FieldName == "SHENG" || FieldName=="sheng")
            {
            listDic = sysTable.GetRows(TableName, "xzjb='1'", out eError);
            }
            else if (FieldName == "SHI" || FieldName == "shi")
            {
                listDic = sysTable.GetRows(TableName, "xzjb='2'", out eError);
            }
            else if (FieldName == "xian" || FieldName == "XIAN")
            {
                listDic = sysTable.GetRows(TableName, "xzjb='3'", out eError);
            }
            else if (FieldName == "xiang" || FieldName == "XIANG")
            {
                listDic = sysTable.GetRows(TableName, "xzjb='4'", out eError);
            }
            else if (FieldName == "cun" || FieldName == "CUN")
            {
                listDic = sysTable.GetRows(TableName, "xzjb='5'", out eError);
            }
            else
            {
                listDic = sysTable.GetRows(TableName, "", out eError);
            }
            if (listDic != null || listDic.Count != 0)
            {
                for (int i = 0; i < listDic.Count; i++)
                {
                    Dictionary<string, object> newdic = listDic[i];
                    NewList.Add(newdic["NAME"].ToString ());
                }
            }
            return NewList;
        }
        public static string GetCode(IWorkspace TmpWorkspace,string FieldName,string FieldChineseName )
        {
            string FCode = "";
            if (TmpWorkspace == null) return FieldChineseName;
            if (FieldChineseName == "" || FieldChineseName == string.Empty) return FieldChineseName;
            if (!File.Exists(m_DicConfigpat)) return FieldChineseName;

            XmlDocument dicXml = new XmlDocument();
            dicXml.Load(m_DicConfigpat);
            //获取字典表名称
            XmlNode xmlNode = dicXml.SelectSingleNode("Root/Item[@FieldName='" + FieldName + "']");
            if (xmlNode == null) return FieldChineseName;
            string TableName = xmlNode.Attributes["TableName"].Value.ToString();

            //使用SysGisTable类获取系统表中的记录
            Gis.SysGisTable sysTable = new Gis.SysGisTable(TmpWorkspace);
            Exception eError = null;
            //根据参数名获取记录
            Dictionary<string, object> dicData = sysTable.GetRow(TableName, "NAME='" + FieldChineseName + "'", out eError);
            //获取参数值
            if (dicData == null)
            {
                return FieldChineseName;
            }
            else
            {
                if (dicData.Count > 0)
                {
                    if (dicData.ContainsKey("CODE"))
                    {
                        if (dicData["CODE"] != null)
                        {
                            FCode = dicData["CODE"].ToString();
                        }
                    }
                }
            }
            dicData = null;
            sysTable = null;
            //行政区代码转换
            switch (FieldName)
            {
                case "SHENG":
                    FCode = FCode + "0000";
                    break;
                case "SHI":
                    FCode = FCode + "00";
                    break; 
                case "XIANG":
                    FCode = FCode + "00";
                    break;
                default :
                    break;
            }
            return FCode;
        }
        //ygc 20130407 通过字段名称获取字典表名称
        public static string GetDicTableName( string FieldName)
        {
            string tableName = "";
            if (FieldName == "" || FieldName == null) return tableName;
            if (!File.Exists(m_DicConfigpat)) return tableName;
            XmlDocument dicXml = new XmlDocument();
            dicXml.Load(m_DicConfigpat);
            //获取字典表名称
            XmlNode xmlNode = dicXml.SelectSingleNode("Root/Item[@FieldName='" + FieldName + "']");
            if (xmlNode == null) return tableName;
            tableName = xmlNode.Attributes["TableName"].Value.ToString();
            return tableName;
        }
        //ygc 20130416 初始话数据字典，放到内存中
        public static Dictionary<string, Dictionary<string, string>> InitialDic(IWorkspace pWorkspace)
        {
            Dictionary<string, Dictionary<string, string>> newdic = new Dictionary<string, Dictionary<string, string>>();
            if (pWorkspace == null) return newdic;
            if (!File.Exists(m_DicConfigpat)) return newdic;
            XmlDocument dicXml = new XmlDocument();
            dicXml.Load(m_DicConfigpat);
            XmlNodeList ItemList = dicXml.SelectNodes("Root/Item");
            if (ItemList.Count == 0) return newdic;

            //使用SysGisTable类获取系统表中的记录
            Gis.SysGisTable sysTable = new Gis.SysGisTable(pWorkspace);
            Exception eError = null;
            try
            {
                for (int i = 0; i < ItemList.Count; i++)
                {
                    List<Dictionary<string, object>> ListdicValue = new List<Dictionary<string, object>>();
                    Dictionary<string, string> dicValue = new Dictionary<string, string>();
                    string tableName = ItemList[i].Attributes["TableName"].Value.ToString();
                    ListdicValue = sysTable.GetRows(tableName, "", out eError);
                    for (int t = 0; t < ListdicValue.Count; t++)
                    {
                        Dictionary<string, object> tempDic = new Dictionary<string, object>();
                        tempDic = ListdicValue[t];
                        dicValue.Add(tempDic["CODE"].ToString(), tempDic["NAME"].ToString());
                    }
                    newdic.Add(ItemList[i].Attributes["FieldName"].Value.ToString(), dicValue);
                }
            }
            catch (Exception ex)
            {
 
            }
                return newdic;
        }
    }

}