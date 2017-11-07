using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Threading;
using System.Collections;
using System.Data.OracleClient;
//using GeoStarCore;
//using GEOFILETRANSLib;
//using GDCOREADERINTERFACELib;
//using GDCOWRITERINTERFACELib;
//using GDCOSUPPORTLib;
//using GeoSystemSupport;
//using GDCOMODELLib;
//using GDCOGMLWRITERLib;
//using AxGeoSpaceLib;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using SysCommon.Authorize;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;
using SysCommon.Error;

namespace GeoDBATool
{

    /// <summary>
    /// 作者：yjl
    /// 日期：2011.07.27
    /// 说明：元数据统计窗体
    /// </summary>
    public partial class frmMetaStat : DevComponents.DotNetBar.Office2007Form
    {
        private IWorkspace m_WS = null;        //系统维护库连接工作空间
       private string MDConnPath = Application.StartupPath + "\\..\\MDxmlData\\MDConn.dat";
       private int m_type;
       private Dictionary<string, string> fdTranslate;//字段中英文转换
       List<string> con;//0=type,1=database
        public frmMetaStat()
        {
            InitializeComponent();
            //fdTranslate = new Dictionary<string, string>();
            //fdTranslate.Add("生产者","producer");
            //fdTranslate.Add("年度","producedate");
            //fdTranslate.Add("发行者","publisher");
            //fdTranslate.Add("所有者","owner");
            //listColumns.Items.Add("生产者");
            //listColumns.Items.Add("年度");
            //listColumns.Items.Add("发行者");
            //listColumns.Items.Add("所有者");

            //暂使用元数据现有字段
            listColumns.Items.Add("图幅号");
            listColumns.Items.Add("图名");
            listColumns.Items.Add("数据类型");
            listColumns.Items.Add("数据生产单位");
            listColumns.Items.Add("数据生产时间");
            listColumns.Items.Add("入库时间");
            con = GetConnData(MDConnPath);
        }
        private Boolean bIsLink = false;
        private Boolean bIsSelected = false;
     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private List<string> GetConnData(string ConntPath)
        {
            List<string> strconn = new List<string>();
            try
            {
                System.Collections.Hashtable conset = conset = new System.Collections.Hashtable();
                List<string> strVale = new List<string>();

                SysCommon.Authorize.AuthorizeClass.Deserialize(ref conset, ConntPath);
                foreach (DictionaryEntry de in conset)
                {
                    strVale.Add(de.Value.ToString());
                }
                int index1 = strVale[0].IndexOf("|");
                int index2 = strVale[0].IndexOf("|", index1 + 1);
                int index3 = strVale[0].IndexOf("|", index2 + 1);
                int index4 = strVale[0].IndexOf("|", index3 + 1);
                int index5 = strVale[0].IndexOf("|", index4 + 1);
                int index6 = strVale[0].IndexOf("|", index5 + 1);
                strconn.Add(strVale[1].ToString());
                switch (strVale[1].ToString())
                {
                    case "1":
                        strconn.Add(strVale[0].Substring(index2 + 1, index3 - index2 - 1));
                        break;
                    case "3":
                        //Server
                        strconn.Add(strVale[0].Substring(0, index1));
                        //Service
                        strconn.Add(strVale[0].Substring(index1 + 1, index2 - index1 - 1));
                        //Database
                        strconn.Add(strVale[0].Substring(index2 + 1, index3 - index2 - 1));
                        //User
                        strconn.Add(strVale[0].Substring(index3 + 1, index4 - index3 - 1));
                        //Password
                        strconn.Add(strVale[0].Substring(index4 + 1, index5 - index4 - 1));
                        //Version
                        strconn.Add(strVale[0].Substring(index5 + 1, index6 - index5 - 1));
                        break;
                }
                return strconn;
            }
            catch
            {
                return strconn = null;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            try
            {
                beginStatics();
            }
            catch (Exception ex)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
            }
        }
        //进行分组统计
        private void beginStatics()
        {
            //if (!File.Exists(MDConnPath))
            //{
            //    MessageBox.Show("目标元数据库连接失败！", "提示！");
            //    return;
            //}
            if (listSelectColumns.Items.Count == 0)
            {
                MessageBox.Show("请选择分组字段！", "提示！");
                return;

            }

            string conn = "Provider=OraOLEDB.Oracle;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + Plugin.Mod.Server + ") (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User Id=" + Plugin.Mod.User + "; Password=" + Plugin.Mod.Password + "";
            string gpField = "", seField = "";
            foreach (object lvi in listSelectColumns.Items)
            {
                //gpField += fdTranslate[lvi.ToString()] + ",";
                //seField += fdTranslate[lvi.ToString()] + " as " + lvi.ToString() + ",";

                //暂使用元数据现有字段
                gpField += lvi.ToString() + ",";
                seField += lvi.ToString() + ",";

            }
            OleDbConnection oeCon = new OleDbConnection(conn);
            oeCon.Open();
            if (gpField.EndsWith(","))
                gpField = gpField.Substring(0, gpField.Length - 1);
            if (seField.EndsWith(","))
                seField = seField.Substring(0, seField.Length - 1);
            if (ModTableFun.isExist(oeCon, "MD_data_Statistic"))
                ModTableFun.DropTable(oeCon, "MD_data_Statistic");


            OleDbCommand oeCmd = oeCon.CreateCommand();
            //oeCmd.CommandText = "select " + seField + ",count(*) as 元数据数量 into MD_data_Statistic from METADATA_XML group by " + gpField;//access语法
            oeCmd.CommandText = "create table MD_data_Statistic as select " + seField + ",count(*) as 元数据数量 from METADATA_XML group by " + gpField;//oracle语法
            oeCmd.ExecuteNonQuery();
            //oeCmd.Transaction.Commit();
            oeCmd.CommandText = "select * from MD_data_Statistic";
            OleDbDataAdapter oeAdapter = new OleDbDataAdapter(oeCmd);
            DataTable pDataTable = new DataTable();
            oeAdapter.Fill(pDataTable);
            dataGridViewR.DataSource = pDataTable;
            dataGridViewR.Refresh();
         
        }
      

        private int GetColumnIndex(System.Data.OracleClient.OracleDataReader rstOracle, string columnName)
        {
            for (int i = 0; i < rstOracle.FieldCount; i++)
            {
                if (columnName.ToUpper() == rstOracle.GetName(i).ToUpper()) return i;
            }

            return -1;
        }
        //序列化xml
        public static byte[] xml2obj(object obj)
        {
            try
            {
                IPersistStream pPersistStream = obj as IPersistStream;
                IStream pStream = new XMLStreamClass();
                pPersistStream.Save(pStream, 0);

                IXMLStream pXMLStream = pStream as IXMLStream;
                byte[] bytes = pXMLStream.SaveToBytes();
                return bytes;
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }
        private ParameterizedThreadStart ThreadStart()
        {
            throw new NotImplementedException();
        }
        #region 原始代码
        //private void changeRow2XML(string strTableName, XmlNode vCurNode, string strSQL, string strModelXMLPath, string strWorkPath, string strFieldValue)
        //{
        //    XmlNode vTableNode = vCurNode.ParentNode.ParentNode;
        //    string strPath;
        //    if (strFieldValue != "")
        //    {
        //        strPath = strWorkPath + "\\" + strFieldValue;
        //    }
        //    else
        //    {
        //        strPath = strWorkPath;
        //    }
        //    if (!Directory.Exists(strPath)) Directory.CreateDirectory(strPath);
        //    string strCountSQL = strSQL.Replace("*","count(*)");
        //    OleDbCommand vCommand = new OleDbCommand(strCountSQL, m_vConnSor);
        //    if (m_vConnSor.State == ConnectionState.Closed  )
        //    {
        //        m_vConnSor.Open();
        //    }
        //    OleDbDataReader vDataReader = vCommand.ExecuteReader();
        //    vDataReader.Read();
        //    string lRowCount = vDataReader[0].ToString();
        //    this.lstLog.Items.Add(strFieldValue +"总数为：" +lRowCount);
        //    int iRowCount = Convert.ToInt32(lRowCount);

        //    this.progressBar1.Minimum = 0;
        //    this.progressBar1.Maximum = iRowCount;
        //    this.progressBar1.Visible = true;           

        //    OleDbCommand vCommand1 = new OleDbCommand(strSQL, m_vConnSor);
        //    if (m_vConnSor.State == ConnectionState.Closed)
        //    {
        //        m_vConnSor.Open();
        //    }
        //    OleDbDataReader vDataReader1 = vCommand1.ExecuteReader();
        //    Int32 iIndex = 0;
        //    while (vDataReader1.Read())
        //    {
        //        //读模板XML
        //        XmlDocument xmlDocModel = new XmlDocument();
        //        xmlDocModel.Load(strModelXMLPath);
        //        //清除模板中的提示信息，即将中括号[]内的信息清空
        //        clearNodeValue(xmlDocModel);

        //        XmlNamespaceManager vXMLNSM = new XmlNamespaceManager(xmlDocModel.NameTable);
        //        vXMLNSM.AddNamespace("smmd", "http://data.sbsm.gov.cn/smmd/2007");

        //        //定义图幅号变量
        //        string strIndexMapID = "";

        //        //XmlNode vGMLNode = null;
        //        string strFileNameID = "";
        //        string strGovNameID = "";
        //        string strTpCatID = "";
        //        string strScaleID = "";
        //        string strDecryption = "";

        //        XmlNode vXmlNodeComparison = vTableNode.LastChild;
        //        foreach (XmlNode vSubNode in vXmlNodeComparison.ChildNodes)
        //        {                    
        //            try
        //            {
        //                string strXMLNode = vSubNode.Attributes["name"].Value;
        //                string strFieldName = vSubNode.Attributes["fieldname"].Value;
        //                string strDefault = vSubNode.Attributes["default"].Value;
        //                string strValue = "";
        //                if (strFieldName != "")
        //                {
        //                    strValue = vDataReader1[strFieldName].ToString();
        //                    if (strFieldName == "新图号" || strFieldName == "军控点编号" || strFieldName == "编号" || strFieldName == "航片编号" || strFieldName == "景号")
        //                    {
        //                        //解决部分数据没有编号的问题
        //                        if (strValue == "")
        //                        {
        //                            strValue = Guid.NewGuid().ToString().Substring(0, 8);
        //                        }
        //                        //将新图号赋给变量
        //                        strIndexMapID = strValue;                                
                                
        //                    }
        //                }
        //                else if (strDefault != "")
        //                {
        //                    //一些默认值从对照关系表中读取，其余值从Oracle数据中读取
        //                    strValue = strDefault;
        //                }

        //                XmlNode vModelSubNode = xmlDocModel.SelectSingleNode(strXMLNode, vXMLNSM);
        //                if (vModelSubNode== null )
        //                {
        //                     this.lstLog.Items.Add("没有找到节点：" + strXMLNode);
        //                }
        //                else if (strValue == "")
        //                {
        //                     //this.lstLog.Items.Add("值为空：" + strXMLNode);
        //                }
        //                else 
        //                {
        //                    //if (vModelSubNode.Name == "smmd:mdFileID")
        //                    //strFileNameID = strValue;
        //                    if (vModelSubNode.InnerText != "")
        //                    {
        //                        if (vModelSubNode.Name == "smmd:keyword")
        //                        {
        //                            vModelSubNode.InnerXml = vModelSubNode.InnerText + "," + strValue;
        //                        }
        //                        else if (vModelSubNode.Name == "smmd:idAbs" || vModelSubNode.Name == "smmd:measResult" || vModelSubNode.Name == "smmd:linStatement")
        //                        {
        //                            vModelSubNode.InnerXml = vModelSubNode.InnerText + ";" + strFieldName + ":" + strValue;
        //                        }
        //                        else if (vModelSubNode.Name == "smmd:refDate" || vModelSubNode.Name == "smmd:mdDateSt" || vModelSubNode.Name == "smmd:measDateTm" || vModelSubNode.Name == "smmd:appDate" || vModelSubNode.Name == "smmd:beginning" || vModelSubNode.Name == "smmd:ending" || vModelSubNode.Name == "DataNext")
        //                        {
        //                            if (strDefault != "")
        //                            {
        //                                vModelSubNode.InnerXml = strValue;
        //                            }
        //                            else
        //                            {
        //                                vModelSubNode.InnerXml = getDateString(strValue);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            vModelSubNode.InnerXml = vModelSubNode.InnerText + "," + strValue;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (vModelSubNode.Name == "smmd:idAbs" || vModelSubNode.Name == "smmd:measResult" || vModelSubNode.Name == "smmd:linStatement")
        //                        {
        //                            vModelSubNode.InnerXml = strFieldName + ":" + strValue;
        //                        }
        //                        else if (vModelSubNode.Name == "smmd:refDate" || vModelSubNode.Name == "smmd:mdDateSt" || vModelSubNode.Name == "smmd:measDateTm" || vModelSubNode.Name == "smmd:appDate" || vModelSubNode.Name == "smmd:beginning" || vModelSubNode.Name == "smmd:ending" || vModelSubNode.Name == "DataNext")
        //                        {
        //                            if (strDefault != "")
        //                            {
        //                                vModelSubNode.InnerXml = strValue;
        //                            }
        //                            else
        //                            {
        //                                vModelSubNode.InnerXml = getDateString(strValue);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            vModelSubNode.InnerXml = strValue;
        //                        }
        //                        //
        //                        if (vModelSubNode.Name == "smmd:tpCat")
        //                        {
        //                            strTpCatID = getDictItemIDFromName( "tpCat",strValue);
        //                        }
        //                        else if (vModelSubNode.Name == "smmd:govName")
        //                        {
        //                            strGovNameID = "210553";
        //                        }
        //                        //比例尺或采样间隔
        //                        else if (vModelSubNode.Name == "smmd:equScale")
        //                        {
        //                            strScaleID = getScaleIDFromScale(strValue, 1);
        //                        }
        //                        else if (vModelSubNode.Name == "smmd:ScaleDist")
        //                        {
        //                            strScaleID = getScaleIDFromScale(strValue, 2);
        //                        }
        //                        else if (strXMLNode == "smmd:decryption")
        //                        {
        //                            strDecryption = getDictItemIDFromName("decryption", strValue);
        //                        }
        //                    }
        //                }
        //            }
        //            catch
        //            {
        //            }
        //        }
        //        if (strSQL.Contains("like"))
        //        {
        //            vXmlNodeComparison = vCurNode.LastChild;
        //            foreach (XmlNode vSubNode in vXmlNodeComparison.ChildNodes)
        //            {
        //                try
        //                {
        //                    string strXMLNode = vSubNode.Attributes["name"].Value;
        //                    string strFieldName = vSubNode.Attributes["fieldname"].Value;
        //                    string strDefault = vSubNode.Attributes["default"].Value;
        //                    string strValue = "";
        //                    if (strFieldName != "")
        //                    {
        //                        strValue = vDataReader1[strFieldName].ToString();
        //                        if (strFieldName == "新图号" || strFieldName == "军控点编号" || strFieldName == "编号" || strFieldName == "航片编号" || strFieldName == "景号")
        //                        {
        //                            //解决部分数据没有编号的问题
        //                            if (strValue == "")
        //                            {
        //                                strValue = Guid.NewGuid().ToString().Substring(0, 8);
        //                            }
        //                            //将新图号赋给变量
        //                            strIndexMapID = strValue;

        //                        }
        //                    }
        //                    else if (strDefault != "")
        //                    {
        //                        //一些默认值从对照关系表中读取，其余值从Oracle数据中读取
        //                        strValue = strDefault;
        //                    }

        //                    XmlNode vModelSubNode = xmlDocModel.SelectSingleNode(strXMLNode, vXMLNSM);
        //                    if (vModelSubNode == null)
        //                    {
        //                        this.lstLog.Items.Add("没有找到节点：" + strXMLNode);
        //                    }
        //                    else if (strValue == "")
        //                    {
        //                        //this.lstLog.Items.Add("值为空：" + strXMLNode);
        //                    }
        //                    else
        //                    {
        //                        //if (vModelSubNode.Name == "smmd:mdFileID")
        //                        //strFileNameID = strValue;
        //                        if (vModelSubNode.InnerText != "")
        //                        {
        //                            if (vModelSubNode.Name == "smmd:keyword")
        //                            {
        //                                vModelSubNode.InnerXml = vModelSubNode.InnerText + "," + strValue;
        //                            }
        //                            else if (vModelSubNode.Name == "smmd:idAbs" || vModelSubNode.Name == "smmd:measResult" || vModelSubNode.Name == "smmd:linStatement")
        //                            {
        //                                vModelSubNode.InnerXml = vModelSubNode.InnerText + ";" + strFieldName + ":" + strValue;
        //                            }
        //                            else if (vModelSubNode.Name == "smmd:refDate" || vModelSubNode.Name == "smmd:mdDateSt" || vModelSubNode.Name == "smmd:measDateTm" || vModelSubNode.Name == "smmd:appDate" || vModelSubNode.Name == "smmd:beginning" || vModelSubNode.Name == "smmd:ending" || vModelSubNode.Name == "DataNext")
        //                            {
        //                                if (strDefault != "")
        //                                {
        //                                    vModelSubNode.InnerXml = strValue;
        //                                }
        //                                else
        //                                {
        //                                    vModelSubNode.InnerXml = getDateString(strValue);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                vModelSubNode.InnerXml = vModelSubNode.InnerText + "," + strValue;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (vModelSubNode.Name == "smmd:idAbs" || vModelSubNode.Name == "smmd:measResult" || vModelSubNode.Name == "smmd:linStatement")
        //                            {
        //                                vModelSubNode.InnerXml = strFieldName + ":" + strValue;
        //                            }
        //                            else if (vModelSubNode.Name == "smmd:refDate" || vModelSubNode.Name == "smmd:mdDateSt" || vModelSubNode.Name == "smmd:measDateTm" || vModelSubNode.Name == "smmd:appDate" || vModelSubNode.Name == "smmd:beginning" || vModelSubNode.Name == "smmd:ending" || vModelSubNode.Name == "DataNext")
        //                            {
        //                                if (strDefault != "")
        //                                {
        //                                    vModelSubNode.InnerXml = strValue;
        //                                }
        //                                else
        //                                {
        //                                    vModelSubNode.InnerXml = getDateString(strValue);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                vModelSubNode.InnerXml = strValue;
        //                            }
        //                            //
        //                            if (vModelSubNode.Name == "smmd:tpCat")
        //                            {
        //                                strTpCatID = getDictItemIDFromName("tpCat", strValue);
        //                            }
        //                            else if (vModelSubNode.Name == "smmd:govName")
        //                            {
        //                                strGovNameID = "210553";
        //                            }
        //                            //比例尺或采样间隔
        //                            else if (vModelSubNode.Name == "smmd:equScale")
        //                            {
        //                                strScaleID = getScaleIDFromScale(strValue, 1);
        //                            }
        //                            else if (vModelSubNode.Name == "smmd:ScaleDist")
        //                            {
        //                                strScaleID = getScaleIDFromScale(strValue, 2);
        //                            }
        //                            else if (strXMLNode == "smmd:decryption")
        //                            {
        //                                strDecryption = getDictItemIDFromName("decryption", strValue);
        //                            }
        //                        }
        //                    }
        //                }
        //                catch
        //                {
        //                }
        //            }
        //        }
        //        //根据图幅号生成图形
        //        //string strGMLFile = getGMLfromIndexMapID(strIndexMapID);
        //        //if (strGMLFile != "")
        //        //{
        //        //    XmlDocument vGMLDoc = new XmlDocument();
        //        //    vGMLDoc.Load(strGMLFile);
        //        //    XmlNamespaceManager vGMLNM = new XmlNamespaceManager(vGMLDoc.NameTable);
        //        //    vGMLNM.AddNamespace("wfs", "http://www.opengis.net/wfs");
        //        //    //获得gml节点
        //        //    vGMLNode = vGMLDoc.SelectSingleNode("/wfs:FeatureCollection", vGMLNM);
        //        //    /*
        //        //     * 将GML加入到元数据XML中
        //        //     *从xmlDoc中读取dataExt节点，将GML插入为dataExt的子节点
        //        //    */
        //        //    XmlNode vCurNodeFC = xmlDocModel.SelectSingleNode("/smmd:Metadata/smmd:dataIdInfo/*/smmd:dataExt/smmd:FeatureCollection", vXMLNSM);
        //        //    if (vCurNodeFC != null & vGMLNode != null)
        //        //    {
        //        //        vCurNodeFC.InnerXml = vGMLNode.InnerXml;
        //        //    }
        //        //    //删除GML文件
        //        //    File.Delete(strGMLFile);
        //        //}
                

        //        if (strScaleID == "")
        //        {
        //            strScaleID = "000000";
        //        }
        //        if (strDecryption == "")
        //        {
        //            strDecryption = "9";
        //        }
        //        strFileNameID = strGovNameID + "_" + strTpCatID + "_" + strScaleID + "_" + "00000000" + "_" + strDecryption + "_" + Guid.NewGuid().ToString();

        //        XmlNode vFildIDNode = xmlDocModel.SelectSingleNode("/smmd:Metadata/smmd:mdFileID", vXMLNSM);
        //        vFildIDNode.InnerText = strFileNameID;

        //        xmlDocModel.Save(strPath + "\\" + strFileNameID + ".xml");
        //        iIndex++;
        //        //if (iIndex >=2) return;
                

        //        //this.progressBar1.Value = iIndex;
        //        //this.lblTile.Text = "正在转换【" + strTableName + "】" + iIndex + "/" + lRowCount;
        //        //Thread.Sleep(100);  
        //    }
        //    vDataReader1.Close();
        //}
        #endregion
        public static string getScaleIDFromScale(string strValue, int iIndex)
        {
            string strScaleID = "000000";
            if (iIndex == 1)
            {
                if (strValue == "")
                {
                    strScaleID = "S00000";
                }
                else
                {
                    int iScale = Convert.ToInt32(strValue);
                    if (iScale >= 10000)
                    {
                        strScaleID = Convert.ToString(iScale / 1000) + "K";
                    }
                    else if (iScale >= 1000 && iScale < 10000)
                    {
                        int iPro = iScale / 1000;
                        int iOUT = iScale - 1000 * iPro;
                        if (iOUT == 0)
                        {
                            strScaleID = iPro + "K";
                        }
                        else
                        {
                            string strAdminCode = Convert.ToString(iOUT);
                            int iIndex0 = 0;
                            for (int i = 0; i < strAdminCode.Length - 1; i++)
                            {
                                if (strAdminCode.Substring(strAdminCode.Length - 1 - i, 1) != "0")
                                {
                                    iIndex0 = i;
                                    break;
                                }
                            }
                            string strFCode = strAdminCode.Substring(0, strAdminCode.Length - iIndex0 - 2);
                            strScaleID = iPro + "K" + strFCode;
                        }
                    }
                    else if (iScale < 1000)
                    {
                        strScaleID = strValue;
                    }

                    if (strScaleID.Length == 1)
                    {
                        strScaleID = "S0000" + strScaleID;
                    }
                    else if (strScaleID.Length == 2)
                    {
                        strScaleID = "S000" + strScaleID;
                    }
                    else if (strScaleID.Length == 3)
                    {
                        strScaleID = "S00" + strScaleID;
                    }
                    else if (strScaleID.Length == 4)
                    {
                        strScaleID = "S0" + strScaleID;
                    }
                    else
                    {
                        strScaleID = "S" + strScaleID;
                    }
                }
            }
            else if (iIndex == 2)
            {
                if (strValue == "")
                {
                    strScaleID = "R00000";
                }
                else
                {
                    if (strValue.Contains("."))
                    {
                        string[] strTemp = strValue.Split(new char[] { '.' });
                        strScaleID = strTemp[0] + "M" + strTemp[1];
                    }
                    else
                    {
                        strScaleID = strValue + "M";
                    }
                }
                if (strScaleID.Length == 2)
                {
                    strScaleID = "R000" + strScaleID;
                }
                else if (strScaleID.Length == 3)
                {
                    strScaleID = "R00" + strScaleID;
                }
                else if (strScaleID.Length == 4)
                {
                    strScaleID = "R0" + strScaleID;
                }
                else
                {
                    strScaleID = "R" + strScaleID;
                }
            }
            return strScaleID;
        }
        //根据分类名称获得分类代码
        public  string getDictItemIDFromName(string strType, string strItemValue)
        {
            strItemValue = strItemValue.Trim();
            string strDictItemID = "";
            XmlDocument xmlDoc = new XmlDocument();
            string strXMLFile = Application.StartupPath + "\\Res\\restype.xml";
            xmlDoc.Load(strXMLFile);
            string strXPath = "/ZJMetaDataDictionery/ClassType[@xmlnode='" + strType + "']";
            XmlNode vCurNode = xmlDoc.SelectSingleNode(strXPath);

            foreach (XmlNode vSubXMLNode in vCurNode.ChildNodes)
            {
                string strName = vSubXMLNode.Attributes["value"].Value;
                string strID = vSubXMLNode.Attributes["id"].Value;
                if (strItemValue == strName)
                {
                    strDictItemID = strID;
                    break;
                }
            }
            if (strType.ToUpper() == "GOVNAME")
            {
                return strDictItemID.Substring(1, 6);
            }
            else if (strType.ToUpper() == "DECRYPTION")
            {
                return strDictItemID.Substring(5, 1);
            }
            else
            {
                return strDictItemID;
            }
        }
        //处理时间字符串，标准化为形如“1900-01-01”
        private string getDateString(string strValue)
        {
            string strNewDate = "";
            //处理时间元数据项  
            if (strValue == "0")
            {
                strNewDate = "1899-01-01";
            }
            else if ( strValue.Contains("/") || strValue.Contains("."))
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
                strNewDate = strNewDate.Substring( 0,strNewDate.Length -1);
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

            if (strNewDate.Contains("-") & strNewDate.Length !=10)
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
        
        //private string getGMLfromIndexMapID(string strIndexMapID)
        //{
        //    string strGMLFile = "";
        //    if (strIndexMapID != "")
        //    {
        //        IFeatureLayer pFeatLayer = null;
        //        string strFCName = getFCNameFromIndexMapID(strIndexMapID);
        //        if (strFCName == "") return strGMLFile;

        //        for (int i = 0; i < GeoSpaceIndexMap.LayerBox.LayerCount; i++)
        //        {
        //            GeoStarCore.ILayer pLayer = GeoSpaceIndexMap.LayerBox.get_Layers(i);
        //            if (pLayer.Name == strFCName)
        //            {
        //                pFeatLayer = pLayer as IFeatureLayer;
        //                break;
        //            }
        //        }
        //        if (pFeatLayer == null) return strGMLFile;
        //        IQueryFilter pQueryFilter = new QueryFilterClass();
        //        pQueryFilter.WhereClause = "indexrecid = '" + strIndexMapID + "'";

        //        IFeatureCursor pFeatCursor = pFeatLayer.Search(pQueryFilter);
        //        Feature pGeoFeature = pFeatCursor.NextRow();
        //        if (pGeoFeature != null)
        //        {
        //            //将Geostar的Feature转换为GML文件
        //            //strGMLFile = getGMLFileFromGeoFeature(pGeoFeature, strIndexMapID);
        //        }
        //    }
        //    return strGMLFile;
        //}
        
        //根据图幅号获得对应图形索引图层名称
        private string getFCNameFromIndexMapID(string strIndexMapID)
        {
            //根据图幅号获得比例尺,进而得到索引图层名称。**这个需要在配置中设置，后期作为扩展完善**
            //500\1000\2000的图幅号
            string strFeatClass="";
            //if (strIndexMapID.Substring(3, 1) == "K")//500
            //{
            //    strFeatClass = "idx_Polygon_500";
            //}
            //else if (strIndexMapID.Substring(3, 1) == "J")//1K
            //{
            //    strFeatClass = "idx_Polygon_1K";
            //}
            //else if (strIndexMapID.Substring(3, 1) == "I")//2K
            //{
            //    strFeatClass = "idx_Polygon_2K";
            //}
            //else 
            try
            {
                if (strIndexMapID.Substring(3, 1) == "H")//5K
                {
                    strFeatClass = "idx_Polygon_5K_ex";
                }
                else if (strIndexMapID.Substring(3, 1) == "G")//10K
                {
                    strFeatClass = "idx_Polygon_10K_ex";
                }
                else if (strIndexMapID.Substring(3, 1) == "F")//25K
                {
                    strFeatClass = "idx_Polygon_25K";
                }
                else if (strIndexMapID.Substring(3, 1) == "E")//5W
                {
                    strFeatClass = "idx_Polygon_50K";
                }
                else if (strIndexMapID.Substring(3, 1) == "D")//10W
                {
                    strFeatClass = "idx_Polygon_100K";
                }
                else if (strIndexMapID.Substring(3, 1) == "Z")//200K
                {
                    strFeatClass = "idx_Polygon_200K";
                }
                else if (strIndexMapID.Substring(3, 1) == "C")//25W
                {
                    strFeatClass = "idx_Polygon_250K";
                }
                else if (strIndexMapID.Substring(3, 1) == "B")//50W
                {
                    strFeatClass = "idx_Polygon_500K";
                }
                else if (strIndexMapID.Substring(3, 1) == "A")//100w
                {
                    strFeatClass = "idx_Polygon_1M";
                }
                else
                {
                    strFeatClass = "";
                }
            }
            catch
            {

            }
            return strFeatClass;
        }
        
        private void  clearNodeValue( XmlDocument xmlDocModel)
        {
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocModel.NameTable);
            namespaceManager.AddNamespace("smmd", "http://data.sbsm.gov.cn/smmd/2007");

            XmlNode vRootNode = xmlDocModel.SelectSingleNode("/smmd:Metadata", namespaceManager);
            ReadxmlAllNode(vRootNode.ChildNodes);
            //return xmlDocModel;
        }
        //遍历xml清除[]的内容
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
                        vCurNode.InnerText ="";
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
                if (strAllNode=="")
                {
                    strAllNode = "/smmd:" + strTemp[i];
                }
                else 
                {
                    strAllNode =strAllNode+ "/smmd:" + strTemp[i];
                }
            }
            return strAllNode;
        }
        
        private void btnSetLink_Click(object sender, EventArgs e)
        {

            //frmDBSetting frm = new frmDBSetting();
            //frm.ShowDialog();
            //if (frm.bIsOK )
            //{
                //string strServer = frm.strServer;
                //string strIP = frm.strIP;
                //string strUser = frm.strUser;
                //string strPassword = frm.strPassword;
                //try
                //{
                //    //连接oracle数据源:srv7,zhfwysj,1
                //    string strPath;
                //    if (frm.strDBType == "access")
                //    {
                //        if (strIP == "")
                //        {
                //            strPath = "";
                //        }
                //        else
                //        {
                //            strPath = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + strIP+"';";
                //        }
                //    }
                //    else
                //    {
                //        if (strIP != "")
                //        {
                //            strPath = "Provider=OraOLEDB.Oracle;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + strIP + ")(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + strServer + ")));User Id= " + strUser + "; Password=" + strPassword + ";Persist Security Info=False;";
                //        }
                //        else
                //        {
                //            strPath = "Provider=OraOLEDB.Oracle;Data Source=" + strServer + ";User Id= " + strUser + "; Password=" + strPassword + ";Persist Security Info=False;";
                //        }
                //    }
                //    m_vConnSor = new OleDbConnection(strPath);
                //    //m_vConnSor.Open();
                //    bIsLink = true;
                //    //MessageBox.Show("连接数据库成功！","连接源数据库",MessageBoxButtons.OK,MessageBoxIcon.Information );
                //    this.lstLog.Items.Add( "连接数据库成功！");
                //    if (bIsSelected)
                //    {
                //        this.btnOK.Enabled = true;
                //    }
                //}
                //catch
                //{
                //    bIsLink = false;
                //    //MessageBox.Show("连接数据库失败！", "连接源数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    this.lstLog.Items.Add( "连接数据库失败！请检查网络连接或连接参数设置。");
                //    this.btnOK.Enabled = false;
                //}
            //}
        }

        private void frmMetaDataChange_Load(object sender, EventArgs e)
        {
            
            //if (File.Exists(MDConnPath))
            //{
            //    comboBoxDataSource.Enabled = false;
            //    System.Collections.Hashtable conset = conset = new System.Collections.Hashtable();
            //    List<string> strVale = new List<string>();
            //    SysCommon.Authorize.AuthorizeClass.Deserialize(ref conset, MDConnPath);
            //    foreach (DictionaryEntry de in conset)
            //    {
            //        strVale.Add(de.Value.ToString());
            //    }
            //    comboBoxDataSource.Text = strVale[0].ToString();
            //}
            
            
        }

        private void btnAllSelected_Click(object sender, EventArgs e)
        {
            
        }

        private void btnOtherSelected_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

         
            
        }
        
       

        private void txtXMLPath_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnOpenXml_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
           
        }
      
        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void comboBoxDataSource_DropDown(object sender, EventArgs e)
        {
            SysGisTable sysTable = new SysGisTable(m_WS);
            //Dictionary<string, object> dicData = new Dictionary<string, object>();
            Exception eError;
            //初始化数据源
            List<object> ListDatasource = sysTable.GetFieldValues("DATABASEMD", "DATABASENAME", "", out eError);
            this.comboBoxDataSource.Items.Clear();
            foreach (object datasource in ListDatasource)
            {
                this.comboBoxDataSource.Items.Add(datasource.ToString());
            }
        }
        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        private IWorkspace GetWorkSpacefromConninfo(string conninfostr, int type)
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
        private void comboBoxDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(MDConnPath))
                {
                    File.Delete(MDConnPath);
                }
                SysGisTable sysTable = new SysGisTable(m_WS);
                Exception eError;
                string DataSourceName = this.comboBoxDataSource.Text;
                string conninfostr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "DATABASENAME='" + DataSourceName + "'", out eError).ToString();
                int type = int.Parse(sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "DATABASENAME='" + DataSourceName + "'", out eError).ToString());
                int iDataType = int.Parse(sysTable.GetFieldValue("DATABASEMD", "DATABASETYPEID", "DATABASENAME='" + DataSourceName + "'", out eError).ToString());
                string strDBPara = sysTable.GetFieldValue("DATABASEMD", "DBPARA", "DATABASENAME='" + DataSourceName + "'", out eError).ToString();
                string pID = sysTable.GetFieldValue("DATABASEMD", "ID", "DATABASENAME='" + DataSourceName + "'", out eError).ToString();
                int index6 = conninfostr.LastIndexOf("|");
                
            }
            catch
            {
            }
          
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (listColumns.SelectedItems.Count == 0)
            {
                MessageBox.Show("没有选中的可用字段!", "提示");
                return;
            }
            for (int i = 0; i < listColumns.SelectedItems.Count; i++)
            {
                string strFieldname = listColumns.SelectedItems[i] as string;
                if (!listSelectColumns.Items.Contains(strFieldname))
                {
                    listSelectColumns.Items.Add(strFieldname);
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (listSelectColumns.SelectedItems.Count == 0)
            {
                //MessageBox.Show("没有选中的分组字段!", "提示");
                return;
            }
            //倒序删除，以防出错
            int cnt = listSelectColumns.SelectedItems.Count;
            for (int i = cnt; i > 0; i--)
            {
                int Fieldindex = listSelectColumns.SelectedIndices[i - 1];
                listSelectColumns.Items.RemoveAt(Fieldindex);
            }
        }

        //利用exel应用程序
        private void exportExcelApp(string fileName, DataGridView myDGV)
        {
            string saveFileName = "";
            bool fileSaved = false;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel文件(*.xls)|*.xls";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            System.Windows.Forms.Application.DoEvents();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消 
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("无法创建Excel对象，可能您的电脑未安装Excel");
                return;
            }

            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1

            //写入标题
            for (int i = 0; i < myDGV.ColumnCount; i++)
            {
                worksheet.Cells[1, i + 1] = myDGV.Columns[i].HeaderText;
            }
            //写入数值
            for (int r = 0; r < myDGV.Rows.Count; r++)
            {
                for (int i = 0; i < myDGV.ColumnCount; i++)
                {
                    try
                    {
                        if (myDGV.Rows[r].Cells[i].Value != null)
                            worksheet.Cells[r + 2, i + 1] = myDGV.Rows[r].Cells[i].Value.ToString();
                        else
                            worksheet.Cells[r + 2, i + 1] = null;

                    }
                    catch (Exception ex)
                    {
                        worksheet.Cells[r + 2, i + 1] = ex.Message;

                    }
                }

            }
            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            //if (Microsoft.Office.Interop.cmbxType.Text != "Notification")
            //{
            //    Excel.Range rg = worksheet.get_Range(worksheet.Cells[2, 2], worksheet.Cells[ds.Tables[0].Rows.Count + 1, 2]);
            //    rg.NumberFormat = "00000000";
            //}

            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                    fileSaved = true;
                }
                catch (Exception ex)
                {
                    fileSaved = false;
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                }

            }
            else
            {
                fileSaved = false;
            }
            xlApp.Visible = true;
            //xlApp = null;
            //GC.Collect();//强行销毁 
            //if (fileSaved && System.IO.File.Exists(saveFileName)) System.Diagnostics.Process.Start(saveFileName); //打开EXCEL
            //MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewR.RowCount == 0)
                return;
            exportExcelApp("元数据统计", dataGridViewR);
        }



       
       

      

    

 
    }
}
