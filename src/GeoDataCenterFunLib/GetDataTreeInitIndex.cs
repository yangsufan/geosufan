using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
//========================================================
//功能：针对Res\\Xml\\DataTreeInitIndex.xml进行读取操作（只读）
//作者：wgf
//时间：2011-01-25
//========================================================
namespace GeoDataCenterFunLib
{
    public class GetDataTreeInitIndex
    {
        //加载xml文件
        public    string m_strInitXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\DataTreeInitIndex.xml";
  

        //根据itemName获取tblName属性
        public string GetTblNameByItemName(string strItemName)
        {
            string strTblName = "";
            if (strItemName.Equals(""))
                return strTblName;
         
            XmlDocument xmldoc = new XmlDocument();
            if (xmldoc != null)
            {
                if (File.Exists(m_strInitXmlPath))
                {
                    xmldoc.Load(m_strInitXmlPath);
                    string strSearch = "//Layer[@ItemName=" + "'" + strItemName + "'" + "]";
                 //   string strSearch = "//Layer[@ItemName=" + strItemName;
                    XmlNode xmlNode = xmldoc.SelectSingleNode(strSearch);
                    if (xmlNode != null)
                    {
                        XmlElement xmlElt = (XmlElement)xmlNode;
                        strTblName = xmlElt.GetAttribute("tblName");
                    }
                }
            }
            return strTblName;
        }

        //从配置文件中获取信息
        //strItemTag       标签
        //strItemName      元素名称
        //返回             元素值
        public string GetXmlElementValue(string strItemTag,string strItemName)
        {
            string strVaule = "";
            if (strItemName.Equals(""))
                return strVaule;
            XmlDocument xmldoc = new XmlDocument();
            if (xmldoc != null)
            {
                if (File.Exists(m_strInitXmlPath))
                {
                    xmldoc.Load(m_strInitXmlPath);
                    string strSearch = "//" + strItemTag;
                    XmlNode xmlNode = xmldoc.SelectSingleNode(strSearch);
                    if (xmlNode != null)
                    {
                        XmlElement xmlElt = (XmlElement)xmlNode;
                        strVaule = xmlElt.GetAttribute(strItemName);
                    }
                }
            }
            return strVaule;
        }

        //获取数据库记录信息
        public string GetDbInfo()
        {
            string strVaule = "";
            string strBuffer = "";
            XmlDocument xmldoc = new XmlDocument();
            if (xmldoc != null)
            {
                if (File.Exists(m_strInitXmlPath))
                {
                    xmldoc.Load(m_strInitXmlPath);
                    string strSearch = "//DbInfo";
                    XmlNode xmlNode = xmldoc.SelectSingleNode(strSearch);
                    if (xmlNode != null)
                    {
                        XmlElement xmlElt = (XmlElement)xmlNode;
                        strBuffer = xmlElt.GetAttribute("dbInfoPath");
                    }
                }
                strVaule = Application.StartupPath + strBuffer;
            }
            return strVaule;
        }

        //获取当前数据库连接信息
        //strItemName  对应dbType、dbServerPath、dbServerName、dbUser、dbPassword
        public string GetDbValue(string strItemName)
        {
            string strVaule = "";
            if (strItemName.Equals(""))
                return strVaule;
            XmlDocument xmldoc = new XmlDocument();
            if (xmldoc != null)
            {
                if (File.Exists(m_strInitXmlPath))
                {
                    xmldoc.Load(m_strInitXmlPath);
                    string strSearch = "//CurDbSet";
                    XmlNode xmlNode = xmldoc.SelectSingleNode(strSearch);
                    if (xmlNode != null)
                    {
                        XmlElement xmlElt = (XmlElement)xmlNode;
                        strVaule = xmlElt.GetAttribute(strItemName);
                    }
                }
            }
            return strVaule;
        }

        //修改当前的数据库连接信息
        //strItemName  对应dbType、dbServerPath、dbServerName、dbUser、dbPassword
        public void SetDbValue(string strItemName,string strVaule)
        {
            if (strItemName.Equals(""))
                return ;
            XmlDocument xmldoc = new XmlDocument();
            if (xmldoc != null)
            {
                if (File.Exists(m_strInitXmlPath))
                {
                    xmldoc.Load(m_strInitXmlPath);
                    string strSearch = "//CurDbSet";
                    XmlNode xmlNode = xmldoc.SelectSingleNode(strSearch);
                    if (xmlNode != null)
                    {
                        XmlElement xmlElt = (XmlElement)xmlNode;
                        xmlElt.SetAttribute(strItemName, strVaule);
                        xmldoc.Save(m_strInitXmlPath);
                    }
                }
            }
            return ;
        }
    }

   
}
