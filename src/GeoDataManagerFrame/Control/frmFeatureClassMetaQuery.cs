using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Gis;
using System.Xml;
using ESRI.ArcGIS.esriSystem;
using System.IO;

namespace GeoDataManagerFrame
{
    public partial class frmFeatureClassMetaQuery :DevComponents.DotNetBar.Office2007Form
    {
        IWorkspace m_Workspace = null;
        private int m_iFileIDItem = 0;//文件标识符ID
        private int m_iRootItem = 0;//根节点ID
        private String m_MetadataItemPath = Application.StartupPath + "\\..\\MDxmlData\\Res\\MetadataItem.xml";
        private String m_ResTypePath = Application.StartupPath + "\\..\\MDxmlData\\Res\\restype.xml";
        //ZQ 20111020  add 值为0代表不可元数据编辑，其他值代表可编辑
        private string m_IsEdite = "0";
        private string m_IsEditePath = Application.StartupPath + "\\..\\MDxmlData\\Res\\元数据编辑设置.xml";
        private string m_strMetaDataValue = Application.StartupPath + "\\..\\MDxmlData\\Res\\元数特殊表字段及值对应关系表.xml";
        public frmFeatureClassMetaQuery(IWorkspace pWorkspace)
        {
            InitializeComponent();
            m_Workspace = pWorkspace;
        }
        #region  公共函数
        //获取数据库的数据类型（ORACLE MDB GDB）
        public static string GetDescriptionOfWorkspace(IWorkspace pWorkspace)
        {
            string strLike = "%";
            if (pWorkspace == null)
            {
                return strLike = "%";
            }
            IWorkspaceFactory pWorkSpaceFac = pWorkspace.WorkspaceFactory;
            if (pWorkSpaceFac == null)
            {
                return strLike = "%";
            }
            string strDescrip = pWorkSpaceFac.get_WorkspaceDescription(false);
            switch (strDescrip)
            {
                case "Personal Geodatabase"://mdb数据库 使用*作匹配符
                    strLike = "*";
                    break;
                case "File Geodatabase"://gdb数据库 使用%作匹配符
                    strLike = "%";
                    break;
                case "Spatial Database Connection"://sde(oracle数据库) 使用%作匹配符(sql server数据库，现在暂未考虑)
                    strLike = "%";
                    break;
                default:
                    strLike = "%";
                    break;
            }
            return strLike;
        }
        /// <summary>
        /// 查询与关键字相关的所有数据库名称
        /// </summary>
        /// <param name="strKeys"></param>
        /// <returns></returns>
        private List<object> GetFeatureCalssName(string strKeys)
        {
            List<object> LstFeatureClassName = new List<object>(); 
            Exception exError = null;
                SysGisTable sysTable = new SysGisTable(m_Workspace);
            try
            {

                string condition = "数据库名称 Like '" + GetDescriptionOfWorkspace(m_Workspace) + strKeys + GetDescriptionOfWorkspace(m_Workspace) + "'";
                LstFeatureClassName = sysTable.GetFieldValues("METADATA_LIB", "数据库名称", condition,out exError);
                return LstFeatureClassName;
            }
            catch { return LstFeatureClassName; }
            finally
            {
                sysTable = null;
            }
        }
        private void Query()
        {
            //if (txtKeys.Text == "")
            //{
            //    MessageBox.Show("请输入查询关键字", "提示！");
            //    return;
            //}
            nodeName.Nodes.Clear();
            nodeName.Nodes.Add("查询结果", "查询结果", 1, 1);
            nodeName.ExpandAll();
            try
            {
                List<object> LstFeatureClassName = GetFeatureCalssName(txtKeys.Text);
                if (LstFeatureClassName.Count == 0) { return; }
                TreeNode vNode = nodeName.Nodes[0];
                for (int i = 0; i < LstFeatureClassName.Count; i++)
                {
                    vNode.Nodes.Add(LstFeatureClassName[i].ToString(), LstFeatureClassName[i].ToString(), 2, 2);
                }
                vNode.ExpandAll();
            }
            catch { }

        }
        #endregion
        private void btnQuery_Click(object sender, EventArgs e)
        {
            Query();
            exontrol.EXGRIDLib.Columns vColumns = this.exgridXML.Columns;
            vColumns.Clear();
        }

        private void txtKeys_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Query();
            }
        }

        private void nodeName_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 1)
            {
                if (e.Node.Text == null) { return; }
                string strFeatureClassName = e.Node.Text;
                try
                {
                    IFeatureWorkspace pFeatureWorkspace = m_Workspace as IFeatureWorkspace;
                    ITable pTable = pFeatureWorkspace.OpenTable("METADATA_LIB");
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    int pIndex = pTable.FindField("数据库名称");
                    IField pField = pTable.Fields.get_Field(pIndex);
                    switch (pField.Type)
                    {
                        case esriFieldType.esriFieldTypeString:
                            pQueryFilter.WhereClause = pField.Name + " = '" + strFeatureClassName + "'";
                            break;
                        default:
                            pQueryFilter.WhereClause = pField.Name + " = " + strFeatureClassName;
                            break;
                    }
                    ICursor pCursor = pTable.Search(pQueryFilter, false);
                    IRow pRow = pCursor.NextRow();
                    if (pRow != null) {  }
                    while (pRow != null)
                    {
                      conversionMetadata(pRow);
                      pRow = pCursor.NextRow();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    pCursor = null;
                }
                catch { }
            }


        }
        #region 根据Table生成xml并展示成中文结构
        private void conversionMetadata(IRow pRow)
        {
            try
            {
                //读取对照关系
                XmlDocument vXmlDoc = new XmlDocument();
                string strXMLFile = Application.StartupPath + "\\..\\MDxmlData\\Res\\metachange.xml"; ;
                vXmlDoc.Load(strXMLFile);
                XmlNode vCurNode = vXmlDoc.SelectSingleNode("/zjMetadata/table[@name='FeatureDataset']");
                //***模板文件****
                XmlNode vModelXMLNode = vCurNode.FirstChild;
                string strClassField = vModelXMLNode.Attributes["name"].Value;
                //*******遍历子节点为模板文件******
                foreach (XmlNode vSubItemNode in vModelXMLNode.ChildNodes)//都取metachange.xml中item项中的DLG、DOM、DEM等值来选取模板
                {

                    string strFieldValue = vSubItemNode.Attributes["value"].Value;//DLG、DOM、DEM等
                    string strModelXML = vSubItemNode.Attributes["modelxml"].Value;//选取的模板ＸＭＬ位置\Bin\model
                    string strfuzzyQuery = "";
                    if (vSubItemNode.Attributes["fuzzyQuery"] != null)
                    {
                        strfuzzyQuery = vSubItemNode.Attributes["fuzzyQuery"].Value;
                    }
                    string strModelXMLPath = Application.StartupPath + "\\..\\MDxmlData\\model\\" + strModelXML;

                    XmlNamespaceManager vXMLNSM = new XmlNamespaceManager(vXmlDoc.NameTable);
                    vXMLNSM.AddNamespace("smmd", "http://data.sbsm.gov.cn/smmd/2007");
                    changeRow2XML(vSubItemNode, strModelXMLPath,pRow);
                }
            }
            catch
            {}
        }
        private void changeRow2XML( XmlNode vCurNode, string strModelXMLPath,IRow pRow)
        {
            try
            {
               XmlNode vTableNode = vCurNode.ParentNode.ParentNode;
                //读模板XML
                XmlDocument xmlDocModel = new XmlDocument();
                xmlDocModel.Load(strModelXMLPath);//读取选择的模板XML（例如：基础测绘_片中DLG再匹对模板）
                //清除模板中的提示信息，即将中括号[]内的信息清空
                clearNodeValue(xmlDocModel);
                XmlNamespaceManager vXMLNSM = new XmlNamespaceManager(xmlDocModel.NameTable);
                vXMLNSM.AddNamespace("smmd", "http://data.sbsm.gov.cn/smmd/2007");
                //  ZQ 20110802   add记录  行列号
                string[] strRowC = new string[2];
                XmlNode vXmlNodeComparison = vTableNode.LastChild;
                foreach (XmlNode vSubNode in vXmlNodeComparison.ChildNodes)
                {
                    try
                    {
                        string strXMLNode = vSubNode.Attributes["name"].Value;
                        string strFieldName = vSubNode.Attributes["fieldname"].Value;
                        string strDefault = vSubNode.Attributes["default"].Value;
                        string strValue = "";
                        if (strFieldName != "")
                        {
                            int iFieldNameIndex = pRow.Fields.FindField(strFieldName);
                            if (iFieldNameIndex != -1)
                            {
                                string strFileValue = pRow.get_Value(iFieldNameIndex).ToString();
                                strValue = strFileValue;
                            }
                        }
                        else if (strDefault != "")
                        {
                            //一些默认值从对照关系表中读取，其余值从Oracle数据中读取
                            strValue = strDefault;
                        }
                        XmlNode vModelSubNode = xmlDocModel.SelectSingleNode(strXMLNode, vXMLNSM);
                        if (vModelSubNode == null)
                        {
                            continue;
                        }
                        else if (strValue == "")
                        {
                            continue;
                        }
                        else
                        {
                            if (vModelSubNode.InnerText != "")
                            {
                                switch (vModelSubNode.Name)
                                {
                                    case "smmd:rpOrgName":
                                    case "smmd:role":
                                    case "smmd:refDate":
                                    case "smmd:refDateType":
                                        XmlNode pOneXmlNode = vModelSubNode.ParentNode.NextSibling;
                                        XmlNodeList pOneXmlNodeList = pOneXmlNode.ChildNodes;
                                        for (int i = 0; i < pOneXmlNodeList.Count; i++)
                                        {
                                            XmlNode pXmlNode = pOneXmlNodeList.Item(i);
                                            if (pXmlNode.Name == vModelSubNode.Name && pXmlNode.InnerXml == "")
                                            {
                                                pXmlNode.InnerXml = strValue;
                                            }
                                            else if (pXmlNode.Name == vModelSubNode.Name && pXmlNode.InnerXml != "")
                                            {
                                                SamePathVale(pXmlNode, strValue);
                                            }
                                        }
                                        break;
                                    case "smmd:keyword":
                                        vModelSubNode.InnerXml = vModelSubNode.InnerText + "," + strValue;
                                        break;
                                    case "smmd:idAbs":
                                    case "smmd:dqStatement":
                                    case "smmd:mdFileID":
                                        vModelSubNode.InnerXml = vModelSubNode.InnerText + strValue;
                                        break;
                                    case "smmd:linStatement":
                                        vModelSubNode.InnerXml = GetNodeValue("//Smmd[@Name='", vModelSubNode.Name, strValue);
                                        if (vModelSubNode.InnerXml == "")
                                        {
                                            vModelSubNode.InnerXml = strValue;
                                        }
                                        break;
                                    case "smmd:mdDateSt":
                                    case "smmd:measDateTm":
                                        if (strDefault != "")
                                        {
                                            vModelSubNode.InnerXml = strValue;
                                        }
                                        else
                                        {
                                            vModelSubNode.InnerXml = getDateString(strValue);
                                        }
                                        break;
                                    default:
                                        vModelSubNode.InnerXml = vModelSubNode.InnerText + "," + strValue;
                                        break;

                                }
                            }
                            else
                            {
                                switch (vModelSubNode.Name)
                                {
                                    case "smmd:idAbs":
                                    case "smmd:dqStatement":
                                    case "smmd:mdFileID":
                                        vModelSubNode.InnerXml = vModelSubNode.InnerText + strValue;
                                        break;
                                    case "smmd:linStatement":
                                        vModelSubNode.InnerXml = GetNodeValue("//Smmd[@Name='", vModelSubNode.Name, strValue);
                                        if (vModelSubNode.InnerXml == "")
                                        {
                                            vModelSubNode.InnerXml = strValue;
                                        }
                                        break;
                                    case "smmd:refDate":
                                    case "smmd:mdDateSt":
                                    case "smmd:measDateTm":
                                        if (strDefault != "")
                                        {
                                            vModelSubNode.InnerXml = strValue;
                                        }
                                        else
                                        {
                                            vModelSubNode.InnerXml = getDateString(strValue);
                                        }
                                        break;
                                    case "smmd:denFlatRat":
                                        if (strValue.Contains("/"))
                                        {
                                            string[] strfen = strValue.Split(new char[] { '/' });
                                            vModelSubNode.InnerXml = strfen[1];
                                        }
                                        else
                                        {
                                            vModelSubNode.InnerXml = strValue;
                                        }
                                        break;
                                    case "smmd:gridRows":
                                        if (strValue.Contains("*"))
                                        {
                                            strRowC = strValue.Split(new char[] { '*' });
                                            vModelSubNode.InnerXml = strRowC[0];
                                        }
                                        else if (strValue.Contains("X"))
                                        {
                                            strRowC = strValue.Split(new char[] { 'X' });
                                            vModelSubNode.InnerXml = strRowC[0];
                                        }
                                        else
                                        {
                                            vModelSubNode.InnerXml = strValue;
                                        }
                                        break;
                                    case "smmd:gridColumns":

                                        try
                                        {
                                            if (strRowC[0] == "" && strRowC[1] == "")
                                            {
                                                vModelSubNode.InnerXml = strValue;
                                            }
                                            else
                                            {
                                                vModelSubNode.InnerXml = strRowC[1];
                                            }
                                        }
                                        catch { vModelSubNode.InnerXml = strValue; }
                                        break;
                                    case "smmd:westBL":
                                    case "smmd:southBL":
                                        //ZQ  20110805  Add 处理经纬度得问题
                                        GetLongLatMin(vModelSubNode, strValue);
                                        break;
                                    case "smmd:eastBL":
                                    case "smmd:northBL":
                                        //ZQ  20110805  Add 处理经纬度得问题
                                        GetLongLatMax(vModelSubNode, strValue);
                                        break;
                                    default:
                                        vModelSubNode.InnerXml = strValue;
                                        break;
                                }

                            }
                        }
                      } catch{}
                    }
                DeleteFolder(Application.StartupPath + "\\..\\MDxmlData\\XMLData\\");
                string strXMLPath = Application.StartupPath + "\\..\\MDxmlData\\XMLData\\" + Guid.NewGuid().ToString() + ".xml";
                xmlDocModel.Save(strXMLPath);
                //加载元数据文件
                addXMLFile2GRID(strXMLPath, false);
            }
            catch
            { }
      
        }
        /// <summary>
        /// 清空指定的文件夹，但不删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(string dir)
        {
            try
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                        File.Delete(d);//直接删除其中的文件  
                    }
                    else
                    {
                        DirectoryInfo d1 = new DirectoryInfo(d);
                        if (d1.GetFiles().Length != 0)
                        {
                            DeleteFolder(d1.FullName);////递归删除子文件夹
                        }
                        Directory.Delete(d);
                    }
                }
            }
            catch { }
        }
        /// <summary>
        /// 通过搜索具体某个节点获取其一级节点下的某个属性字段值
        /// </summary>
        /// <param name="strSearch">xmlpath</param>
        /// <param name="strFieldName">属性字段</param>
        /// <param name="strValue">父节点值</param>
        /// <returns></returns>
        private string GetNodeValue(string strSearch, string strSmmdName, string strValue)
        {
            string strNodeName = "";
            XmlDocument pXmlDocument = new XmlDocument();
            try
            {
                pXmlDocument.Load(m_strMetaDataValue);
            }
            catch { return strNodeName = ""; }
            XmlNode pXmlNode = pXmlDocument.SelectSingleNode(strSearch + strSmmdName + "']");
            if (pXmlNode == null)
            {
                return strNodeName = "";
            }
            XmlNodeList pXmlNodeList = pXmlNode.ChildNodes;
            for (int i = 0; i < pXmlNodeList.Count; i++)
            {
                if (pXmlNodeList.Item(i).Attributes["Name"].Value == strValue)
                {
                    strNodeName = pXmlNodeList.Item(i).Attributes["Value"].Value;
                    break;
                }
            }
            return strNodeName;
        }
        //ZQ 20110805 Add
        private void GetLongLatMin(XmlNode vModelSubNode, string strValue)
        {
            double DouLongLat;
            if (strValue.Contains("-"))
            {
                string[] strLongLat = strValue.Split(new char[] { '-' });
                DouLongLat = Convert.ToDouble(strLongLat[0].Substring(3, 2)) / 60;
                DouLongLat = Convert.ToDouble(strLongLat[0].Substring(0, 3)) + DouLongLat + Convert.ToDouble(strLongLat[0].Substring(5, strLongLat[0].Length - 5)) / 360;
                vModelSubNode.InnerXml = DouLongLat.ToString();
            }
            else
            {
                vModelSubNode.InnerXml = strValue;
            }
        }
        private void GetLongLatMax(XmlNode vModelSubNode, string strValue)
        {
            double DouLongLat;
            if (strValue.Contains("-"))
            {
                string[] strLongLat = strValue.Split(new char[] { '-' });
                DouLongLat = Convert.ToDouble(strLongLat[1].Substring(3, 2)) / 60;
                DouLongLat = Convert.ToDouble(strLongLat[1].Substring(0, 3)) + DouLongLat + Convert.ToDouble(strLongLat[1].Substring(5, strLongLat[1].Length - 5)) / 360;
                vModelSubNode.InnerXml = DouLongLat.ToString();
            }
            else
            {
                vModelSubNode.InnerXml = strValue;
            }
        }

        //ZQ  20110802  Add
        private void SamePathVale(XmlNode vModelSubNode, string strValue)
        {
            XmlNode pOneXmlNode = vModelSubNode.ParentNode.NextSibling;
            XmlNodeList pOneXmlNodeList = pOneXmlNode.ChildNodes;
            for (int i = 0; i < pOneXmlNodeList.Count; i++)
            {
                XmlNode pXmlNode = pOneXmlNodeList.Item(i);
                if (pXmlNode.Name == vModelSubNode.Name && pXmlNode.InnerXml == "")
                {
                    pXmlNode.InnerXml = strValue;
                }
                else if (pXmlNode.Name == vModelSubNode.Name && pXmlNode.InnerXml != "")
                {
                    SamePathVale(pXmlNode, strValue);
                }
            }
        }
        //end
        //处理时间字符串，标准化为形如“1900-01-01”
        private string getDateString(string strValue)
        {
            string strNewDate = "";
            //处理时间元数据项  
            if (strValue == "0")
            {
                strNewDate = "1899-01-01";
            }
            else if (strValue.Contains("/") || strValue.Contains("."))
            {
                if (strValue.Contains("/"))
                {
                    strNewDate = strValue.Replace("/", "-");
                }
                else if (strValue.Contains("."))
                {
                    strNewDate = strValue.Replace(".", "-");
                }
            }
            else if (strValue.Contains("年") & strValue.Contains("月") & strValue.Contains("日"))
            {
                strNewDate = strValue.Replace("年", "-");
                strNewDate = strNewDate.Replace("月", "-");
                strNewDate = strNewDate.Substring(0, strNewDate.Length - 1);
            }
            else if (strValue.Contains("年") & strValue.Contains("月"))
            {
                strNewDate = strValue.Replace("年", "-");
                strNewDate = strNewDate.Replace("月", "-");
                strNewDate = strNewDate + "01";
            }
            else
            {
                if (strValue.Length == 4)
                {
                    strNewDate = strValue + "-01-01";
                }
                else if (strValue.Length == 5)
                {
                    if (strValue.Substring(4, 1) == "年")
                    {
                        strNewDate = strValue.Substring(0, 4) + "-01-01";
                    }
                    else
                    {
                        strNewDate = strValue.Substring(0, 4) + "-0" + strValue.Substring(4, 1) + "-01";
                    }
                }
                else if (strValue.Length == 6 & !(strValue.Contains("/") || strValue.Contains(".")))
                {
                    strNewDate = strValue.Substring(0, 4) + "-" + strValue.Substring(4, 2) + "-01";
                }
                else if (strValue.Length == 8)
                {
                    strNewDate = strValue.Substring(0, 4) + "-" + strValue.Substring(4, 2) + "-" + strValue.Substring(6, 2);
                }
                else
                {
                    strNewDate = strValue;
                }
            }

            if (strNewDate.Contains("-") & strNewDate.Length != 10)
            {
                string[] strTemp = strNewDate.Split(new char[1] { '-' });
                if (strTemp[1].Length == 1)
                {
                    strTemp[1] = "0" + strTemp[1];
                }

                if (strTemp.Length == 2)
                {
                    strNewDate = strTemp[0] + "-" + strTemp[1] + "-01";
                }
                else if (strTemp.Length == 3)
                {
                    if (strTemp[2].Length == 1)
                    {
                        strTemp[2] = "0" + strTemp[2];
                    }
                    strNewDate = strTemp[0] + "-" + strTemp[1] + "-" + strTemp[2];
                }
            }
            return strNewDate;
        }
        private void clearNodeValue(XmlDocument xmlDocModel)
        {
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocModel.NameTable);
            namespaceManager.AddNamespace("smmd", "http://data.sbsm.gov.cn/smmd/2007");

            XmlNode vRootNode = xmlDocModel.SelectSingleNode("/smmd:Metadata", namespaceManager);
            ReadxmlAllNode(vRootNode.ChildNodes);
            //return xmlDocModel;
        }
        //遍历xml
        private void ReadxmlAllNode(XmlNodeList vChildNodes)
        {
            foreach (XmlNode vCurNode in vChildNodes)
            {
                if (vCurNode.ChildNodes.Count > 1 || vCurNode.ChildNodes[0].HasChildNodes)//
                {
                    XmlNodeList vCurNodes = vCurNode.ChildNodes;
                    ReadxmlAllNode(vCurNodes);
                }
                else
                {
                    //if (vCurNode.InnerText.Contains("["))
                    //{
                    vCurNode.InnerText = "";
                    //}
                }
            }
        }

        private string getAllNode(string strXMLNode)
        {
            string[] strTemp = strXMLNode.Split(new char[1] { '/' });
            string strAllNode = "";
            for (int i = 1; i < strTemp.Length; i++)
            {
                if (strAllNode == "")
                {
                    strAllNode = "/smmd:" + strTemp[i];
                }
                else
                {
                    strAllNode = strAllNode + "/smmd:" + strTemp[i];
                }
            }
            return strAllNode;
        }
        private void addXMLFile2GRID(string strXMLFile, Boolean bIsNew)
        {
            this.exgridXML.BeginUpdate();
            exontrol.EXGRIDLib.Appearance vAppearance = this.exgridXML.VisualAppearance;
            //创建列标题
            exontrol.EXGRIDLib.Columns vColumns = this.exgridXML.Columns;
            vColumns.Clear();
            exontrol.EXGRIDLib.Column vColumn1 = vColumns.Add("元数据项");
            vColumn1.Width = 45;
            vColumn1.Editor.EditType = exontrol.EXGRIDLib.EditTypeEnum.ReadOnly;
            vColumn1.DisplayFilterButton = true;
            vColumn1.Alignment = exontrol.EXGRIDLib.AlignmentEnum.LeftAlignment;
            vColumn1.FireFormatColumn = true;
            vColumn1.LevelKey = "123";

            vColumns.Add("元数据值").Editor.EditType = exontrol.EXGRIDLib.EditTypeEnum.SpinType;
            exontrol.EXGRIDLib.Column vColumn2 = vColumns.Add("备注");
            vColumn2.Editor.EditType = exontrol.EXGRIDLib.EditTypeEnum.ReadOnly;
            vColumn2.Width = 5;
            exgridXML.LoadXML(strXMLFile);
            try
            {
                //读模板XML
                //ZQ   20110802   modify
                if (!File.Exists(strXMLFile))
                {
                    MessageBox.Show("没有查找到元数据详细信息,请确认数据.", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //end
                ///ZQ 20111020 add 获取元数据编辑状态
                m_IsEdite = GetMetaIsEdite();
                XmlDocument vXMLDoc = new XmlDocument();
                vXMLDoc.Load(strXMLFile);

                XmlNamespaceManager vXMLNM = new XmlNamespaceManager(vXMLDoc.NameTable);
                vXMLNM.AddNamespace("smmd", "http://data.sbsm.gov.cn/smmd/2007");

                XmlNode rootNode = vXMLDoc.SelectSingleNode("/smmd:Metadata", vXMLNM);
                //if (bIsNew)
                //{
                //    XmlNode vNodeFileID = vXMLDoc.SelectSingleNode("/smmd:Metadata/smmd:mdFileID", vXMLNM);
                //    if (vNodeFileID != null)
                //    {
                //        vNodeFileID.InnerText   = "";
                //    }
                //}

                exgridXML.Images(imageList1);

                //增加Grid行中的项
                exontrol.EXGRIDLib.Items var_Items = exgridXML.Items;
                string strRootName = getItemName(rootNode.Name);
                int iRootIndex = var_Items.InsertItem(0, 0, getCNameFromText(strRootName));
                m_iRootItem = iRootIndex;
                var_Items.set_CellEditorVisible(iRootIndex, 1, false);
                var_Items.set_CellBackColor(iRootIndex, 0, Color.BurlyWood);
                var_Items.set_CellBackColor(iRootIndex, 1, Color.BurlyWood);

                XmlNodeList vChildNodes = rootNode.ChildNodes;

                xml2GridItems(vChildNodes, iRootIndex, bIsNew);


                //展开节点
                var_Items.set_ExpandItem(iRootIndex, true);
                this.exgridXML.EndUpdate();
            }
            catch
            {
                MessageBox.Show("读取元数据详细信息时，元数据不符合规范！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 获取配置文件中元数据编辑设置
        /// </summary>
        /// <returns></returns>
        private string GetMetaIsEdite()
        {
            string MetaIsEdit = "0";
            try
            {
                if (!System.IO.File.Exists(m_IsEditePath)) { return MetaIsEdit = "0"; }
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.Load(m_IsEditePath);
                XmlNodeList pNodelist = pXmlDoc.SelectNodes("//Root//SmmdEdit");
                foreach (XmlNode vnode in pNodelist)
                {
                    MetaIsEdit = vnode.Attributes["value"].Value.ToString().Trim();
                }
                return MetaIsEdit;
            }
            catch
            { return MetaIsEdit; }
        }

        private string getItemName(string strEName)
        {
            string[] strTemp = strEName.Split(new char[1] { ':' });
            return strTemp[1];
        }

        //从元数据项的英文名称得到中文名称 
        private string getCNameFromText(string strEName)
        {
            string strCName = "";
            XmlDocument xmlDoc = new XmlDocument();
            string strXMLFile = m_MetadataItemPath;
            if (File.Exists(strXMLFile))
            {
                xmlDoc.Load(strXMLFile);
                XmlNode vCurNode = xmlDoc.SelectSingleNode("/ZJMetaDataItems/item[@name='" + strEName + "']");
                if (vCurNode != null)
                {
                    strCName = vCurNode.Attributes["value"].Value;
                }
            }
            return strCName;
        }

        //从元数据项的中文名称,iPramIndex=1得到英文名称;iPramIndex=2得到选择条件;iPramIndex=3得到提示信息;
        private string getValueFromCName(string strCName, int iPramIndex)
        {
            string strEName = "";
            XmlDocument xmlDoc = new XmlDocument();
            string strXMLFile = m_MetadataItemPath;
            if (File.Exists(strXMLFile))
            {
                xmlDoc.Load(strXMLFile);
                XmlNode vCurNode = xmlDoc.SelectSingleNode("/ZJMetaDataItems/item[@value='" + strCName + "']");
                if (vCurNode != null)
                {
                    if (iPramIndex == 1)
                    {
                        strEName = vCurNode.Attributes["name"].Value;
                    }
                    else if (iPramIndex == 2)
                    {
                        strEName = vCurNode.Attributes["optional"].Value;
                    }
                    else if (iPramIndex == 3)
                    {
                        strEName = vCurNode.Attributes["remark"].Value;
                    }
                }
            }
            return strEName;
        }
        //从字典文件中读取字典项，放到下来框中
        private int GetDictFromFile(string strItem, int iRow, string strCodeText)
        {
            int iOutIndex = 0;
            exontrol.EXGRIDLib.Items var_Items = exgridXML.Items;
            exontrol.EXGRIDLib.Editor editor = var_Items.get_CellEditor(iRow, 1);
            editor.EditType = exontrol.EXGRIDLib.EditTypeEnum.DropDownListType;
            ///ZQ 20111019 add 设置元数据是否可以编辑
            switch (m_IsEdite)
            {
                case "0":
                    editor.DropDownVisible = false;
                    break;
                default:
                    editor.DropDownVisible = true;
                    break;
            }
            ///end
            editor.DropDownAutoWidth = exontrol.EXGRIDLib.DropDownWidthType.exDropDownEditorWidth;
            XmlDocument xmlDoc = new XmlDocument();
            string strXMLFile = m_ResTypePath;
            xmlDoc.Load(strXMLFile);
            XmlNode vCurNode = xmlDoc.SelectSingleNode("/ZJMetaDataDictionery/ClassType[@xmlnode='" + strItem + "']");
            int iIndex = 0;
            editor.AddItem(iIndex, " ");
            foreach (XmlNode vSubXMLNode in vCurNode.ChildNodes)
            {
                iIndex++;
                string strName = vSubXMLNode.Attributes["value"].Value;
                string strID = vSubXMLNode.Attributes["id"].Value;
                if (strItem == "resType")
                {
                    editor.AddItem(iIndex, strName);
                }
                else if (strItem == "tpCat")
                {
                    if (strID.Substring(2, 2) == "00")
                    {
                        editor.AddItem(iIndex, strName);
                    }
                    else if (strID.Substring(3, 1) == "0")
                    {
                        editor.AddItem(iIndex, "     " + strName);
                    }
                    else
                    {
                        editor.AddItem(iIndex, "          " + strName);
                    }

                }
                else if (strItem == "GACateId")
                {
                    if (strID.Length == 2)
                    {
                        editor.AddItem(iIndex, strName);
                    }
                    else if (strID.Length == 5)
                    {
                        editor.AddItem(iIndex, "     " + strName);
                    }
                }
                else
                {
                    editor.AddItem(iIndex, strName);
                }
                if (strName == strCodeText)
                {
                    iOutIndex = iIndex;
                }
            }
            return iOutIndex;
        }
        //遍历xml,写到GRID中
        private void xml2GridItems(XmlNodeList vChildNodes, int h, Boolean bIsNew)
        {
            Dictionary<int, string> vDictItems = new Dictionary<int, string>();
            int iIndex = 0;
            string strXMLFile = m_ResTypePath;
            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(strXMLFile))
            {
                xmlDoc.Load(strXMLFile);
                XmlNode vRootNode = xmlDoc.SelectSingleNode("/ZJMetaDataDictionery");
                if (vRootNode != null)
                {
                    foreach (XmlNode vSubNode in vRootNode.ChildNodes)
                    {
                        if (vSubNode.Attributes["xmlnode"].Value != "")
                        {
                            vDictItems.Add(iIndex, vSubNode.Attributes["xmlnode"].Value);
                            iIndex++;
                        }
                    }
                }
            }

            exontrol.EXGRIDLib.Items var_Items = exgridXML.Items;
            foreach (XmlNode vCurNode in vChildNodes)
            {
                string strCurName = getItemName(vCurNode.Name);
                //测试代码
                if (strCurName == "resType")
                {

                }
                string strCName = getCNameFromText(strCurName);
                if (strCurName == "FeatureCollection")
                {
                    continue;
                }


                if (strCName == "")
                {
                    strCName = vCurNode.Name;
                }

                int iRow = var_Items.InsertItem(h, 0, strCName);

                //shduan 20101101 将节点对应的路径记录在提示中
                string strNodePath = "";
                strNodePath = "/" + vCurNode.ParentNode.Name + "/" + vCurNode.Name;
                var_Items.set_CellToolTip(iRow, 0, strNodePath);
                if (strCurName == "mdFileID")
                {
                    m_iFileIDItem = iRow;
                    var_Items.set_CellEditorVisible(iRow, 1, false);
                }

                if (vCurNode.InnerText != "")
                {
                    if (vCurNode.ChildNodes.Count > 1 || vCurNode.ChildNodes[0].HasChildNodes)
                    {
                        var_Items.set_CellEditorVisible(iRow, 1, false);
                        if (getValueFromCName(strCName, 2) == "m")
                        {
                            var_Items.set_CellImages(iRow, 0, "6,5");
                            //var_Items.set_CellValue(iRow, 2, "必填");
                        }
                        else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
                        {
                            var_Items.set_CellImages(iRow, 0, "6,8");
                        }
                        var_Items.set_CellBackColor(iRow, 0, Color.BurlyWood);//Cornsilk
                        var_Items.set_CellBackColor(iRow, 1, Color.BurlyWood);//Bisque

                        XmlNodeList vCurNodes = vCurNode.ChildNodes;
                        xml2GridItems(vCurNodes, iRow, bIsNew);

                        var_Items.set_ExpandItem(h, true);
                    }
                    else
                    {
                        string strCurNodeName = getItemName(vCurNode.Name);
                        //日期
                        if (strCurNodeName == "mdDateSt" || strCurNodeName == "refDate" || strCurNodeName == "measDateTm" || strCurNodeName == "appDate" || strCurNodeName == "beginning" || strCurNodeName == "ending" || strCurNodeName == "DataNext")
                        {
                            exontrol.EXGRIDLib.Editor vEditor = var_Items.get_CellEditor(iRow, 1);
                            //vEditor.EditType = exontrol.EXGRIDLib.EditTypeEnum.DateType;
                            ///ZQ 20111019 add 设置元数据是否可以编辑
                            switch (m_IsEdite)
                            {
                                case "0":
                                    vEditor.EditType = exontrol.EXGRIDLib.EditTypeEnum.ReadOnly;
                                    break;
                                default:
                                    vEditor.EditType = exontrol.EXGRIDLib.EditTypeEnum.DateType;
                                    break;
                            }
                            ///end
                            if (bIsNew)
                            {
                                var_Items.set_CellValue(iRow, 1, "");
                            }
                            else
                            {
                                var_Items.set_CellValue(iRow, 1, vCurNode.InnerText);
                            }
                            if (getValueFromCName(strCName, 2) == "m")
                            {
                                var_Items.set_CellImages(iRow, 0, "7,5");
                                var_Items.set_CellValue(iRow, 2, "必填");
                            }
                            else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
                            {
                                var_Items.set_CellImages(iRow, 0, "7,8");
                            }
                            var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
                            var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);
                        }
                        else
                        {
                            if (getValueFromCName(strCName, 2) == "m")
                            {
                                var_Items.set_CellImage(iRow, 0, 5);
                                var_Items.set_CellValue(iRow, 2, "必填");
                            }
                            else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
                            {
                                var_Items.set_CellImage(iRow, 0, 8);
                            }
                            if (vDictItems.ContainsValue(strCurNodeName))
                            {
                                int iTextIndex = GetDictFromFile(strCurNodeName, iRow, vCurNode.InnerText);
                                var_Items.set_CellValue(iRow, 1, iTextIndex);
                                var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
                                var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);
                            }
                            else if (strCurNodeName == "mdFileID")
                            {
                                var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
                                var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);//BurlyWood
                                if (bIsNew)
                                {
                                    var_Items.set_CellValue(iRow, 1, "");
                                }
                                else
                                {
                                    var_Items.set_CellValue(iRow, 1, vCurNode.InnerText);
                                }
                            }
                            //可编辑
                            else
                            {
                                ///ZQ 20111019 add 设置元数据是否可以编辑
                                switch (m_IsEdite)
                                {
                                    case "0":
                                        var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.ReadOnly;
                                        break;
                                    default:
                                        var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.EditType;
                                        break;
                                }
                                ///end
                                //var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.EditType;
                                var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
                                var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);//BurlyWood
                                var_Items.set_CellValue(iRow, 1, vCurNode.InnerText);
                            }
                        }

                        var_Items.set_ExpandItem(h, true);
                    }
                }
                else
                {
                    if (vCurNode.ChildNodes.Count > 0)
                    {
                        var_Items.set_CellEditorVisible(iRow, 1, false);
                        if (getValueFromCName(strCName, 2) == "m")
                        {
                            var_Items.set_CellImages(iRow, 0, "6,5");
                            //var_Items.set_CellValue(iRow, 2, "必填");
                        }
                        else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
                        {
                            var_Items.set_CellImages(iRow, 0, "6,8");
                        }
                        var_Items.set_CellBackColor(iRow, 0, Color.BurlyWood);//Cornsilk
                        var_Items.set_CellBackColor(iRow, 1, Color.BurlyWood);//Bisque
                        XmlNodeList vCurNodes = vCurNode.ChildNodes;
                        xml2GridItems(vCurNodes, iRow, bIsNew);

                        var_Items.set_ExpandItem(h, true);
                    }
                    else
                    {
                        string strCurNodeName = getItemName(vCurNode.Name);

                        //日期
                        if (strCurNodeName == "mdDateSt" || strCurNodeName == "refDate" || strCurNodeName == "measDateTm" || strCurNodeName == "appDate" || strCurNodeName == "beginning" || strCurNodeName == "ending" || strCurNodeName == "DataNext")
                        {
                            ///ZQ 20111019 add 设置元数据是否可以编辑
                            switch (m_IsEdite)
                            {
                                case "0":
                                    var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.ReadOnly;
                                    break;
                                default:
                                    var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.DateType;
                                    break;
                            }
                            ///end
                            //var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.DateType;
                            var_Items.set_CellValue(iRow, 1, vCurNode.InnerXml);

                            if (getValueFromCName(strCName, 2) == "m")
                            {
                                var_Items.set_CellImages(iRow, 0, "7,5");
                                var_Items.set_CellValue(iRow, 2, "必填");
                            }
                            else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
                            {
                                var_Items.set_CellImages(iRow, 0, "7,8");
                            }
                            var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
                            var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);
                        }
                        else
                        {
                            if (getValueFromCName(strCName, 2) == "m")
                            {
                                var_Items.set_CellImage(iRow, 0, 5);
                                var_Items.set_CellValue(iRow, 2, "必填");
                            }
                            else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
                            {
                                var_Items.set_CellImage(iRow, 0, 8);
                            }

                            //下拉框
                            //else if (strCurNodeName == "tpCat" || strCurNodeName == "mdChar" || strCurNodeName == "role" || strCurNodeName == "refDateType" || strCurNodeName == "resType" || strCurNodeName == "idStatus" || strCurNodeName == "govName" || strCurNodeName == "GACateId" || strCurNodeName == "class" || strCurNodeName == "useLimit" || strCurNodeName == "spatRpType" || strCurNodeName == "datum" || strCurNodeName == "vertDatum" || strCurNodeName == "dataChar")
                            if (vDictItems.ContainsValue(strCurNodeName))
                            {
                                int iTextIndex = GetDictFromFile(strCurNodeName, iRow, vCurNode.InnerText);
                                var_Items.set_CellValue(iRow, 1, iTextIndex);
                                var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
                                var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);
                            }
                            //可编辑
                            else
                            {
                                ///ZQ 20111019 add 设置元数据是否可以编辑
                                switch (m_IsEdite)
                                {
                                    case "0":
                                        var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.ReadOnly;
                                        break;
                                    default:
                                        var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.EditType;
                                        break;
                                }
                                ///end
                                //var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.EditType;
                                var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
                                var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);
                                var_Items.set_CellValue(iRow, 1, vCurNode.InnerText);
                            }
                        }
                        var_Items.set_ExpandItem(h, true);
                    }
                }
            }
        }
        #endregion


    }
}
