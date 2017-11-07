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
//using System.Data.OleDb;
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

namespace GeoDBATool
{
    public partial class frmMetaConversion : DevComponents.DotNetBar.Office2007Form
    {

        private IWorkspace m_WS = null;        //系统维护库连接工作空间
        private IWorkspace _dataWorkspace = null;//元数据存储工作空间

       private string MDConnPath = Application.StartupPath + "\\..\\MDxmlData\\MDConn.dat";
       private int m_type;

        public frmMetaConversion()
        {
            InitializeComponent();
          
        }
        public frmMetaConversion(IWorkspace pWs)
        {
            InitializeComponent();
            m_WS = pWs;
        }
        //private OleDbConnection m_vConnSor;//原有元数据库连接
        //private string strFileUID="";
        private Boolean bIsLink = false;
        private Boolean bIsSelected = false;
        //private string Path =Application.StartupPath + "\\..\\MD_DATA";
        //private Thread vUIThread = null;
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
                if (!File.Exists(MDConnPath))
                {
                    MessageBox.Show("目标元数据库连接失败！", "提示！");
                    return;
                }

                SysGisTable sysTable = new SysGisTable(m_WS);
                Exception eError;
                System.Collections.Hashtable conset = conset = new System.Collections.Hashtable();

                List<string> strVale = new List<string>();
                SysCommon.Authorize.AuthorizeClass.Deserialize(ref conset, MDConnPath);
                foreach (DictionaryEntry de in conset)
                {
                    strVale.Add(de.Value.ToString());
                }
                string conninfostr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "ID=" + strVale[2].ToString(), out eError).ToString();

                _dataWorkspace = GetWorkSpacefromConninfo(conninfostr, int.Parse(strVale[1].ToString()));
                if (_dataWorkspace == null)
                {
                    MessageBox.Show("目标元数据库连接失败！", "提示！");
                    return;
                }
                m_type = int.Parse(strVale[1].ToString());
                int m = 0;
                for (int i = 0; i < checkedMDData.Items.Count; i++)
                {
                    if (checkedMDData.GetItemChecked(i))
                    {
                        m = m + 1;
                    }
                }
                if (m < 1)
                {
                    MessageBox.Show("请选择元数据", "提示！");
                    return;
                }
                m = 0;
                for (int j = 0; j < checkedXML.Items.Count; j++)
                {
                    if (checkedXML.GetItemChecked(j))
                    {
                        m = m + 1;
                    }
                }
                if (m < 1)
                {
                    MessageBox.Show("请选择元数据库模板", "提示！");
                    return;
                }
                //加载索引图层到地图控件，从默认路径下加载
                //try
                //{
                //    string strIndexMapPath = Application.StartupPath + "\\data\\MapIndex";
                //    if (System.IO.Directory.Exists(strIndexMapPath))
                //    {
                //        DirectoryInfo vIndexFolder = new DirectoryInfo(strIndexMapPath);
                //        int iFileCount = vIndexFolder.GetFiles().Length;

                //        this.progressBar1.Visible = true;
                //        this.progressBar1.Minimum = 0;
                //        this.progressBar1.Maximum = iFileCount;
                //        this.progressBar1.Value = 0;
                //        int iIndex = 0;
                //        this.lstLog.Items.Add("正在加载图形数据......");
                //        this.GeoSpaceIndexMap.LayerBox.ClearLayers();
                //        foreach (FileInfo vFile in vIndexFolder.GetFiles())
                //        {
                //            string strFileName = vFile.FullName;
                //            iIndex++;
                //            this.progressBar1.Value = iIndex;

                //            if (strFileName.Substring(strFileName.Length - 4, 4) == ".shp")
                //            {
                //                GeoStarCore.IFeatureLayer pFeatureLayer = CreateFeatureLayer(strFileName);
                //                if (pFeatureLayer != null)
                //                {
                //                    ILayer layer = pFeatureLayer as ILayer;
                //                    this.GeoSpaceIndexMap.LayerBox.AddLayer(layer);
                //                }
                //                //GeoSpaceIndexMap.ViewFullMap();
                //                GeoSpaceIndexMap.Update();
                //                //axGeoSpace.Refresh();

                //                GeoStarCore.IMap pMap = new GeoStarCore.Map();
                //                pMap = GeoSpaceIndexMap.LayoutBox.MapFrame.Map;
                //            }
                //        }
                //    }
                //    this.lstLog.Items.Add("加载图形数据完成！");
                //}
                //catch
                //{
                //    this.lstLog.Items.Add("加载图形数据失败！");
                //}

                //开始转换
                //this.barLog.AutoHide = true;

                conversionMetadata();
                comboBoxDataSource.Enabled = false;
            }
            catch
            {
            }
            //vUIThread = new Thread(System.Threading.ThreadStart(this.UIRefresh));
            //vUIThread.Start();

            //Thread vDoThread = new Thread(System.Threading.ThreadStart(this.conversionMetadata));
            //vDoThread.Start();
        }
        private void UIRefresh()
        {
            this.progressBar1.Refresh();
            Thread.Sleep(200);
        }
        private void conversionMetadata()
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress();
            try
            {
                vProgress.EnableCancel = false;//设置进度条
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("元数据正在准备入库");

                for (int i = 0; i < this.checkedMDData.Items.Count; i++)
                {
                    if (this.checkedMDData.GetItemChecked(i) == true)
                    {
                        string strPath = checkedMDData.Items[i].ToString();
                        OleDbConnection m_vConnSor = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + strPath + "'");
                        try
                        {
                         
                            m_vConnSor.Open();
                        }
                        catch
                        {
                            MessageBox.Show("数据库连接失败，请检查Access数据库是否为2007版或以上！"," 提示！");
                            return;
                        }
                        DataTable schemaTable = m_vConnSor.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                        for (int k = 0; k < schemaTable.Rows.Count; k++)
                        {
                            string strTableName = schemaTable.Rows[k][2].ToString();//获取源元数据库中的表
                            for (int j = 0; j < checkedXML.Items.Count; j++)
                            {
                                if (checkedXML.GetItemChecked(j) && strTableName == this.checkedXML.Items[j].ToString())
                                {  
                                    vProgress.SetProgress(strTableName + "元数据正在入库");
                                    this.lstLog.Items.Add("正在转换【" + strTableName + "】......");
                                  
                                    //读取对照关系
                                    XmlDocument vXmlDoc = new XmlDocument();
                                    string strXMLFile = this.txtXMLPath.Text;
                                    vXmlDoc.Load(strXMLFile);
                                    XmlNode vCurNode = vXmlDoc.SelectSingleNode("/zjMetadata/table[@name='" + strTableName + "']");
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

                                        ////创建和表同名的目标文件夹
                                        string strSQL = "";
                                        if (strClassField == "")
                                        {
                                            strSQL = "select * from " + strTableName;
                                            changeRow2XML(strTableName, vSubItemNode, strSQL, strModelXMLPath, "", m_vConnSor);
                                        }
                                        else
                                        {
                                            if (strFieldValue == "")
                                            {
                                                strSQL = "select distinct(" + strClassField + ") from " + strTableName;
                                                OleDbCommand vCommand1 = new OleDbCommand(strSQL, m_vConnSor);
                                                if (m_vConnSor.State == ConnectionState.Closed)
                                                {
                                                    m_vConnSor.Open();
                                                }
                                                OleDbDataReader vDataReader1 = vCommand1.ExecuteReader();

                                                while (vDataReader1.Read())
                                                {
                                                    strFieldValue = vDataReader1[0].ToString();
                                                    strSQL = "select * from " + strTableName + " where " + strClassField + " ='" + strFieldValue + "'";

                                                    changeRow2XML(strTableName, vSubItemNode, strSQL, strModelXMLPath, strFieldValue, m_vConnSor);
                                                }
                                            }
                                            else
                                            {
                                                if (strfuzzyQuery != "")
                                                {
                                                    if (strfuzzyQuery.ToUpper() == "TRUE")
                                                    {
                                                        strSQL = "select * from " + strTableName + " where " + strClassField + " like '%" + strFieldValue + "%'";
                                                    }
                                                    else
                                                    {
                                                        strSQL = "select * from " + strTableName + " where " + strClassField + " ='" + strFieldValue + "'";
                                                    }
                                                }
                                                else
                                                {
                                                    strSQL = "select * from " + strTableName + " where " + strClassField + " ='" + strFieldValue + "'";
                                                }
                                                changeRow2XML(strTableName, vSubItemNode, strSQL, strModelXMLPath, strFieldValue, m_vConnSor);
                                            }
                                        }

                                    }
                                }
                            }


                        }
                    }


                }
                vProgress.Close();
                this.lstLog.Items.Add("元数据转换完成！");
            }
            catch
            {
                vProgress.Close();
            }
        }
        private void changeRow2XML(string strTableName, XmlNode vCurNode, string strSQL, string strModelXMLPath, string strFieldValue, OleDbConnection m_vConnSor)
        {
            try
            {
                XmlNode vTableNode = vCurNode.ParentNode.ParentNode;
                string strCountSQL = strSQL.Replace("*","count(*)");
                OleDbCommand vCommand = new OleDbCommand(strCountSQL, m_vConnSor);
                if (m_vConnSor.State == ConnectionState.Closed  )
                {
                    m_vConnSor.Open();
                }
                OleDbDataReader vDataReader = vCommand.ExecuteReader();
                vDataReader.Read();
                string lRowCount = vDataReader[0].ToString();
                this.lstLog.Items.Add(strFieldValue +"总数为：" +lRowCount);
                int iRowCount = Convert.ToInt32(lRowCount);

                this.progressBar1.Minimum = 0;
                this.progressBar1.Maximum = iRowCount;
                this.progressBar1.Visible = true;           

                OleDbCommand vCommand1 = new OleDbCommand(strSQL, m_vConnSor);
                if (m_vConnSor.State == ConnectionState.Closed)
                {
                    m_vConnSor.Open();
                }
                OleDbDataReader vDataReader1 = vCommand1.ExecuteReader();
                Int32 iIndex = 0;
                while (vDataReader1.Read())
                {
                    //读模板XML
                    XmlDocument xmlDocModel = new XmlDocument();
                    xmlDocModel.Load(strModelXMLPath);//读取选择的模板XML（例如：基础测绘_片中DLG再匹对模板）
                    //清除模板中的提示信息，即将中括号[]内的信息清空
                    clearNodeValue(xmlDocModel);
                    XmlNamespaceManager vXMLNSM = new XmlNamespaceManager(xmlDocModel.NameTable);
                    vXMLNSM.AddNamespace("smmd", "http://data.sbsm.gov.cn/smmd/2007");

                    //定义图幅号变量
                    //string strIndexMapID = "";

                    //XmlNode vGMLNode = null;
                    string strFileNameID = "";
                    string strFileName = "";
                    string strGovNameID = "";
                    string strTpCatID = "";
                    string strScaleID = "";
                    string strDecryption = "";
                    //  ZQ 20110802   add记录  行列号
                    string[] strRowC =new string[2];

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
                                DataTable pDataTable = vDataReader1.GetSchemaTable();

                                for (int m = 0; m < pDataTable.Rows.Count; m++)
                                {


                                    if (pDataTable.Rows[m][0].ToString() == strFieldName)
                                    {
                                        string strFileValue = vDataReader1[strFieldName].ToString();
                                        strValue = strFileValue;
                                        break;
                                    }
                                    else if (strDefault != "")
                                    {
                                        strValue = strDefault;
                                    }

                                }

                            }
                            else if (strDefault != "")
                            {
                                //一些默认值从对照关系表中读取，其余值从Oracle数据中读取
                                strValue = strDefault;
                            }

                            XmlNode vModelSubNode = xmlDocModel.SelectSingleNode(strXMLNode, vXMLNSM);
                            if (vModelSubNode.Name == "smmd:resTitle")
                            {
                                strFileName = strValue;
                            }

                            if (vModelSubNode == null)
                            {
                                this.lstLog.Items.Add("没有找到节点：" + strXMLNode);
                            }
                            else if (strValue == "")
                            {
                                this.lstLog.Items.Add("值为空：" + strXMLNode);
                            }
                          
                            else
                            {
                                //if (vModelSubNode.Name == "smmd:mdFileID")
                                //strFileNameID = strValue;
                                if (vModelSubNode.InnerText != "")
                                {
                                    //ZQ  20110802  Add
                                    if (vModelSubNode.Name == "smmd:rpOrgName" || vModelSubNode.Name == "smmd:role" || vModelSubNode.Name == "smmd:refDate" || vModelSubNode.Name == "smmd:refDateType")
                                   {
                                        XmlNode pOneXmlNode =vModelSubNode.ParentNode.NextSibling;
                                        XmlNodeList pOneXmlNodeList = pOneXmlNode.ChildNodes;
                                        for (int i = 0; i < pOneXmlNodeList.Count; i++)
                                       {
                                         XmlNode pXmlNode = pOneXmlNodeList.Item(i);
                                         if (pXmlNode.Name == vModelSubNode.Name&&pXmlNode.InnerXml=="")
                                         {
                                             pXmlNode.InnerXml = strValue;
                                         }
                                         else if(pXmlNode.Name == vModelSubNode.Name&&pXmlNode.InnerXml!="")
                                         {
                                             SamePathVale(pXmlNode,strValue);
                                         }
                                     }
                                      
                                       
                                   }
                                        //end
                                    else if (vModelSubNode.Name == "smmd:keyword")
                                    {
                                        vModelSubNode.InnerXml = vModelSubNode.InnerText + "," + strValue;
                                    }
                                    else if (vModelSubNode.Name == "smmd:idAbs" || vModelSubNode.Name == "smmd:dqStatement" || vModelSubNode.Name == "smmd:mdFileID")
                                    {
                                        vModelSubNode.InnerXml = vModelSubNode.InnerText +strValue;
                                    }
                                    //ZQ   20110805 Add
                                    else if (vModelSubNode.Name == "smmd:linStatement")
                                    {
                                        switch(strValue)
                                        {
                                            case"DRG矢量化":
                                                vModelSubNode.InnerXml = "利用新版地形图资料，经数字化、数据编辑处理、拓扑关系建立、接边处理、投影转换，获取新的1:50000地形要素数据并入库。";
                                                break;
                                            case "综合判调":
                                                vModelSubNode.InnerXml = "在原有1:50000核心要素数据（DLG）基础上，利用正射影像数据、航摄和卫星影像资料、数字栅格地图（DRG），以及车载GPS道路数据、地名、勘界、水利、电力、通讯、交通、森林资源等专业资料，采用影像的室内判绘与数据整合、野外调绘与核查等技术手段，实现1:50000地形要素数据的更新并入库。";
                                                break;
                                            case "1:10000缩编":
                                                vModelSubNode.InnerXml = "利用已有的2000年以后测制生产的1:10000地形矢量数据，按照《1:50000地形要素数据规定》的要素内容和选取指标，采用地图缩编和数据整合处理等技术，获取新的1:50000地形要素数据，或对原1:50000核心地形要素数据进行补充修改，实现相应区域内1:50000地形要素数据的全面更新并入库。";
                                                break;
                                            default:
                                                vModelSubNode.InnerXml = strValue;
                                                break;

                                        }
                                    }
                                    else if (vModelSubNode.Name == "smmd:refDate" || vModelSubNode.Name == "smmd:mdDateSt" || vModelSubNode.Name == "smmd:measDateTm" )
                                    {
                                        if (strDefault != "")
                                        {
                                            vModelSubNode.InnerXml = strValue;
                                        }
                                        else
                                        {
                                            vModelSubNode.InnerXml = getDateString(strValue);
                                        }
                                    }
                                    else
                                    {
                                        vModelSubNode.InnerXml = vModelSubNode.InnerText + "," + strValue;
                                    }
                                }
                                else
                                {
                                    if (vModelSubNode.Name == "smmd:idAbs" || vModelSubNode.Name == "smmd:dqStatement" || vModelSubNode.Name == "smmd:mdFileID")
                                    {
                                        vModelSubNode.InnerXml = vModelSubNode.InnerText + strValue;
                                    }
                                        //ZQ   20110805 Add
                                    else if (vModelSubNode.Name == "smmd:linStatement")
                                    {
                                        switch (strValue)
                                        {
                                            case "DRG矢量化":
                                                vModelSubNode.InnerXml = "利用新版地形图资料，经数字化、数据编辑处理、拓扑关系建立、接边处理、投影转换，获取新的1:50000地形要素数据并入库。";
                                                break;
                                            case "综合判调":
                                                vModelSubNode.InnerXml = "在原有1:50000核心要素数据（DLG）基础上，利用正射影像数据、航摄和卫星影像资料、数字栅格地图（DRG），以及车载GPS道路数据、地名、勘界、水利、电力、通讯、交通、森林资源等专业资料，采用影像的室内判绘与数据整合、野外调绘与核查等技术手段，实现1:50000地形要素数据的更新并入库。";
                                                break;
                                            case "1:10000缩编":
                                                vModelSubNode.InnerXml = "利用已有的2000年以后测制生产的1:10000地形矢量数据，按照《1:50000地形要素数据规定》的要素内容和选取指标，采用地图缩编和数据整合处理等技术，获取新的1:50000地形要素数据，或对原1:50000核心地形要素数据进行补充修改，实现相应区域内1:50000地形要素数据的全面更新并入库。";
                                                break;
                                            default:
                                                vModelSubNode.InnerXml = strValue;
                                                break;

                                        }
                                    }
                                        //end
                                    else if (vModelSubNode.Name == "smmd:refDate" || vModelSubNode.Name == "smmd:mdDateSt" || vModelSubNode.Name == "smmd:measDateTm" || vModelSubNode.Name == "smmd:appDate" || vModelSubNode.Name == "DataNext")
                                    {
                                        if (strDefault != "")
                                        {
                                            vModelSubNode.InnerXml = strValue;
                                        }
                                        else
                                        {
                                            vModelSubNode.InnerXml = getDateString(strValue);
                                        }
                                    }
                                        //ZQ 20110802  add读取分母
                                    else if (vModelSubNode.Name == "smmd:denFlatRat")
                                    {
                                        if (strValue.Contains("/"))
                                        {
                                            string[] strfen = strValue.Split(new char[] { '/' });
                                            vModelSubNode.InnerXml = strfen[1];
                                        }

                                        }
                                        //end
                                        //ZQ 20110802  add读取分母
                                    else if (vModelSubNode.Name == "smmd:gridRows")
                                    {
                                        if(strValue.Contains("*"))
                                        {
                                        strRowC = strValue.Split(new char[]{'*'});
                                        vModelSubNode.InnerXml = strRowC[0];
                                        }
                                        else if (strValue.Contains("X"))
                                        {
                                            strRowC = strValue.Split(new char[] { 'X' });
                                            vModelSubNode.InnerXml = strRowC[0];
                                        }
                                    }
                                    else if (vModelSubNode.Name == "smmd:gridColumns")
                                    {
                                        vModelSubNode.InnerXml = strRowC[1];
                                    }
                                        //end\
                                     //ZQ  20110805  Add 处理经纬度得问题
                                    else if (vModelSubNode.Name == "smmd:westBL" || vModelSubNode.Name == "smmd:southBL")
                                    {
                                        GetLongLatMin(vModelSubNode,strValue);
                                    }
                                    else if (vModelSubNode.Name == "smmd:eastBL" || vModelSubNode.Name == "smmd:northBL")
                                    {
                                        GetLongLatMax(vModelSubNode, strValue);
                                    }
                                        //end
                                    else
                                    {
                                        vModelSubNode.InnerXml = strValue;
                                    }
                                    
                                    if (vModelSubNode.Name == "smmd:tpCat")
                                    {
                                        strTpCatID = getDictItemIDFromName("tpCat", strValue);
                                    }
                                    else if (vModelSubNode.Name == "smmd:govName")
                                    {
                                        strGovNameID = "210553";
                                    }
                                    ////比例尺或采样间隔
                                     if (vModelSubNode.Name == "smmd:equScale")
                                    {
                                        strScaleID = getScaleIDFromScale(strValue, 1);
                                    }
                                    else if (vModelSubNode.Name == "smmd:ScaleDist")
                                    {
                                        strScaleID = getScaleIDFromScale(strValue, 2);
                                    }
                                    else if (strXMLNode == "smmd:decryption")
                                    {
                                        strDecryption = getDictItemIDFromName("decryption", strValue);
                                    }
                                }
                            }
                    }
                    catch
                    {
                    }
                }

                //根据图幅号生成图形
                //string strGMLFile = getGMLfromIndexMapID(strIndexMapID);
                //if (strGMLFile != "")
                //{
                //    XmlDocument vGMLDoc = new XmlDocument();
                //    vGMLDoc.Load(strGMLFile);
                //    XmlNamespaceManager vGMLNM = new XmlNamespaceManager(vGMLDoc.NameTable);
                //    vGMLNM.AddNamespace("wfs", "http://www.opengis.net/wfs");
                //    //获得gml节点
                //    vGMLNode = vGMLDoc.SelectSingleNode("/wfs:FeatureCollection", vGMLNM);
                //    /*
                //     * 将GML加入到元数据XML中
                //     *从xmlDoc中读取dataExt节点，将GML插入为dataExt的子节点
                //    */
                //    XmlNode vCurNodeFC = xmlDocModel.SelectSingleNode("/smmd:Metadata/smmd:dataIdInfo/*/smmd:dataExt/smmd:FeatureCollection", vXMLNSM);
                //    if (vCurNodeFC != null & vGMLNode != null)
                //    {
                //        vCurNodeFC.InnerXml = vGMLNode.InnerXml;
                //    }
                //    //删除GML文件
                //    File.Delete(strGMLFile);
                //}


                if (strScaleID == "")
                {
                    strScaleID = "000000";
                }
                if (strDecryption == "")
                {
                    strDecryption = "9";
                }
                strFileNameID = Guid.NewGuid().ToString();
                //XmlNode vFildIDNode = xmlDocModel.SelectSingleNode("/smmd:Metadata/smmd:mdFileID", vXMLNSM);
                //vFildIDNode.InnerText = strFileNameID; 
                if (m_type == 1)
                {
                    string PathName = _dataWorkspace.PathName;
                    PathName = System.IO.Path.GetDirectoryName(PathName);
                    //ZQ  20110802   add
                    if (!File.Exists(PathName + "\\XMLData"))
                    {
                        Directory.CreateDirectory(PathName + "\\XMLData");// 当路径不存在时创建临时文件存储路径
                    }
                    //end
                    xmlDocModel.Save(PathName + "\\XMLData\\" + strFileNameID + ".xml");
                    IFeatureWorkspace pFeatureWorkspace = _dataWorkspace as IFeatureWorkspace;

                    ITable pTable = pFeatureWorkspace.OpenTable("MD_table");
                    IRow row = pTable.CreateRow();
                    IRowSubtypes rowSubTypes = (IRowSubtypes)row;
                    rowSubTypes.InitDefaultValues();
                    switch (strTableName)
                    {
                        case"dem":
                            row.set_Value(pTable.FindField("metadatatype"), "DEM");
                            break;
                        case"dlg":
                            row.set_Value(pTable.FindField("metadatatype"), "DLG");
                            break;
                        case "MetaData":
                            row.set_Value(pTable.FindField("metadatatype"), "DOM");
                            break;
                    }
                    row.set_Value(pTable.FindField("Data_Time"), DateTime.Now);
                    row.set_Value(pTable.FindField("XMLID"), strFileNameID);
                    row.set_Value(pTable.FindField("MapNo"), strFileName);
                    row.Store();
                    pFeatureWorkspace = null;
                    pTable = null;
                    row = null;
                }
                else if (m_type == 3)
                {
                    System.Collections.Hashtable conset = conset = new System.Collections.Hashtable();

                    List<string> strVale = new List<string>();
                    SysCommon.Authorize.AuthorizeClass.Deserialize(ref conset, MDConnPath);
                    List<string> strConndata = GetConnData(MDConnPath);      
                    string sDatabase = strConndata[3];
                    string sUser =strConndata[4];
                    string sPassword = strConndata[5];
                    //string PathName = Application.StartupPath + "\\..\\MDxmlData\\work\\";
                    //PathName = PathName + strFileNameID + ".xml";
                    //xmlDocModel.Save(PathName);
                    //xmlDocModel.Load(PathName);

                    //string connStr = "Provider=OraOLEDB.Oracle;Data Source=" + sDatabase + ";User Id=" + sUser
                    //  + ";Password=" + sPassword + ";OLEDB.NET=True;";
                    //string OracleConn = "Data Source=" + sDatabase + ";User Id=" + sUser
                    //  + ";Password=" + sPassword + ";";
                    string OracleConn = "Data Source=cxw;User ID=yddata;PASSWORD=yddata;";
                    string strInsert = "insert into ZQMDTABLE (DataTime,XMLID,MapNo) values (to_date('" + System.DateTime.Now + "','yyyy-mm-dd HH24:MI:SS'),'" + strFileNameID + "','" + strFileName + "')";
                    OracleConnection pOracleConnection = new OracleConnection(OracleConn);
                    pOracleConnection.Open();
                    OracleCommand pOracleCommand = new OracleCommand(strInsert,pOracleConnection);
                    pOracleCommand.ExecuteNonQuery();
                    //pOracleConnection.Close();
                    strInsert = "select t.XMLDOC.getclobval() from ZQMDTABLE t WHERE XMLID='" + strFileNameID.Trim() +"'For UPdate";
                    byte[] newvalue = System.Text.Encoding.Unicode.GetBytes(xmlDocModel.InnerXml);
                    //pOracleConnection.Open();
                    pOracleCommand = new System.Data.OracleClient.OracleCommand(strInsert, pOracleConnection);
                    System.Data.OracleClient.OracleDataReader rstOracle  = pOracleCommand.ExecuteReader();
                    rstOracle.Read();
                    //System.Data.OracleClient.OracleTransaction txn = pOracleConnection.BeginTransaction();
                    pOracleCommand.Transaction = pOracleConnection.BeginTransaction();
                    System.Data.OracleClient.OracleLob clob = rstOracle.GetOracleLob(0);
                    //clob.Erase();
                    if (pOracleConnection.State == ConnectionState.Open)
                    {
                      
                        clob.Write(newvalue, 0, newvalue.Length);
                    }
                    //txn.Commit();
                    pOracleCommand.Transaction.Commit();
                    pOracleConnection.Close();
                    //if (File.Exists(PathName))
                    //{
                    //    File.Delete(PathName);
                    //}
                }
                    
            }
            vDataReader1.Close();  
            }
            catch(Exception ex)
            {

            }
            
         
        }

        //ZQ 20110805 Add
        private void GetLongLatMin(XmlNode vModelSubNode, string strValue)
        {
            double DouLongLat;
            if (strValue.Contains("-"))
            {
                string[] strLongLat = strValue.Split(new char[] {'-'});
                DouLongLat = Convert.ToDouble(strLongLat[0].Substring(3,2))/60;
                DouLongLat = Convert.ToDouble(strLongLat[0].Substring(0,3))+DouLongLat  + Convert.ToDouble(strLongLat[0].Substring(5,strLongLat[0].Length-5))/360;
                vModelSubNode.InnerXml = DouLongLat.ToString();
            }
        }
        private void GetLongLatMax(XmlNode vModelSubNode, string strValue)
        {
            double DouLongLat;
            if (strValue.Contains("-"))
            {
                string[] strLongLat = strValue.Split(new char[] {'-'});
                DouLongLat = Convert.ToDouble(strLongLat[1].Substring(3, 2)) / 60;
                DouLongLat = Convert.ToDouble(strLongLat[1].Substring(0, 3)) + DouLongLat + Convert.ToDouble(strLongLat[1].Substring(5, strLongLat[1].Length - 5))/ 360;
                vModelSubNode.InnerXml = DouLongLat.ToString();
            }
        }

        //ZQ  20110802  Add
        private void SamePathVale(XmlNode vModelSubNode, string strValue)
        {
          XmlNode pOneXmlNode =vModelSubNode.ParentNode.NextSibling;
          XmlNodeList pOneXmlNodeList = pOneXmlNode.ChildNodes;
          for (int i = 0; i < pOneXmlNodeList.Count; i++)
          {
             XmlNode pXmlNode = pOneXmlNodeList.Item(i);
             if (pXmlNode.Name == vModelSubNode.Name&&pXmlNode.InnerXml=="")
             {
                 pXmlNode.InnerXml = strValue;
             }
             else if(pXmlNode.Name == vModelSubNode.Name&&pXmlNode.InnerXml!="")
             {
                 SamePathVale(pXmlNode,strValue);
             }
           }
        }
        //end
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
            
            if (File.Exists(MDConnPath))
            {
                comboBoxDataSource.Enabled = false;
                System.Collections.Hashtable conset = conset = new System.Collections.Hashtable();
                List<string> strVale = new List<string>();
                SysCommon.Authorize.AuthorizeClass.Deserialize(ref conset, MDConnPath);
                foreach (DictionaryEntry de in conset)
                {
                    strVale.Add(de.Value.ToString());
                }
                comboBoxDataSource.Text = strVale[0].ToString();
            }
            
            string strXMLFile = Application.StartupPath + "\\..\\MDxmlData\\Res\\metachange.xml";
            this.txtXMLPath.Text = strXMLFile;

            this.timer1.Interval = 1000;
            this.timer1.Start();
        }

        private void btnAllSelected_Click(object sender, EventArgs e)
        {
            if (checkedMDData.Items.Count > 0)
            {
                for (int i = 0; i < checkedMDData.Items.Count; i++)
                {
                    this.checkedMDData.SetItemChecked(i, true);
                }
            }
        }

        private void btnOtherSelected_Click(object sender, EventArgs e)
        {
            if (checkedMDData.Items.Count > 0)
            {
                for (int i = 0; i < checkedMDData.Items.Count; i++)
                {
                    if (this.checkedMDData.GetItemChecked(i))
                    {
                        this.checkedMDData.SetItemChecked(i, false);
                    }
                    else
                    {
                        this.checkedMDData.SetItemChecked(i, true);
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            this.progressBar1.Visible = false;
            this.timer1.Stop();

        }
        #region
        //将Geostar的Feature转换为GML文件，返回GML文件名称，在本地"..\\work\\gmltemp\\"路径下
        //private string getGMLFileFromGeoFeature(Feature pGeoFeature, string strIndexMapID)
        //{
        //    string strFileName = "";
        //    //将geostar的Feature转换为GDC的Feature
        //    GDCOFeature pGDCOFeature = TransGeostarFeatureToGDCOFeature(pGeoFeature);

        //    IGDCOWriter piWriter;
        //    piWriter = new GDCOGMLWriter() as IGDCOWriter;


        //    IGDCOWriter2 piWriter2;
        //    piWriter2 = piWriter as IGDCOWriter2;

        //    GDCOStringArray piWritePara = new GDCOStringArrayClass();
        //    piWritePara.Add("OVERWRITE");
        //    piWritePara.Add("YES");

        //    string strPath = Application.StartupPath + "\\gmltemp\\";
        //    if (!Directory.Exists(strPath))
        //    {
        //        Directory.CreateDirectory(strPath);
        //    }
        //    if (strFileName == "")
        //    {
        //        strFileName = strPath + Guid.NewGuid().ToString() + ".xml";
        //    }
        //    else
        //    {
        //        strFileName = strPath + strIndexMapID + ".xml";
        //    }

        //    piWriter2.WriteStart(strFileName, piWritePara);  //文件夹gmltemp

        //    piWriter2.WriteNext(pGDCOFeature);
        //    piWriter2.WriteCancel();

        //    if (File.Exists(strFileName))
        //    {
        //        return strFileName;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
        
        //public static GeoStarCore.IFeatureLayer CreateFeatureLayer(string strFileName)
        //{
        //    FileTrans vFileTrans = new FileTransClass();
        //    GDCOStringArray strArray = null;
        //    IGDCOReader2 vGDCReader = null;

        //    vFileTrans.GetFileReaderStrings(strFileName, GEOFILETRANSLib.geoDataType.SHP, ref strArray, ref vGDCReader);
            
        //    object[] objs = vFileTrans.TransToFeacls(strArray, vGDCReader) as object[];
        //    if (objs != null)
        //    {
        //        if (objs.Length > 0)
        //        {
        //            GeoStarCore.FeatureClass pFeatureClass = objs[0] as GeoStarCore.FeatureClass;
        //            if (pFeatureClass != null)
        //            {
        //                GeoStarCore.IFeatureLayer pFeatureLayer = new GeoStarCore.FeatureLayerClass();
        //                pFeatureLayer.FeatureClass = pFeatureClass;
        //                pFeatureLayer.Name = System.IO.Path.GetFileNameWithoutExtension(strFileName);
        //                return pFeatureLayer;
        //            }
        //        }
        //    }
        //    return null;
        //}
        
        //private GDCOFeature TransGeostarFeatureToGDCOFeature(Feature pGeoFeature)
        //{
        //    GDCOFeature pGDCOFeature = new GDCOFeatureClass();
        //    FeatureClass pFeatClass = null;
        //    pFeatClass = pGeoFeature.FeatureClass;

        //    object Coords;
        //    object Interpret;
        //    int iDimension = 0;

        //    IGeometry pGeometry = pGeoFeature.Geometry;
        //    IGeometrySerial pGeometrySerial;
        //    pGeometrySerial = (IGeometrySerial)pGeometry;
        //    pGeometrySerial.ExportToOracle(out Coords, out Interpret, out iDimension);
        //    //设置几何数据
        //    pGDCOFeature.put_Geometry(Coords, Interpret);

        //    IDataRoom pDataRoom = null;
        //    pDataRoom = (IDataRoom)pFeatClass;
        //    //设置地物类名称
        //    if (pDataRoom.Name != null)
        //    {
        //        pGDCOFeature.FeatureClassName = pDataRoom.Name;
        //    }
        //    else
        //    {
        //        pGDCOFeature.FeatureClassName = pFeatClass.AliasName;
        //    }

        //    //设置几何类型
        //    pGDCOFeature.GeometryType = pGeoFeature.Geometry.GeometryType;

        //    Fields pGeoFields = pGeoFeature.Fields;
        //    int iCount = 0;
        //    iCount = pGeoFields.Count;

        //    Field pGeoField = null;
        //    string strFieldName = "";

        //    Fields pGDCOFields = new FieldsClass();

        //    if (pGeoFeature.Geometry.GeometryType == geoGEOMETRYTYPE.GEO_GEOMETRY_ANNOTATION)
        //    {
        //        for (int i = 27; i < iCount; i++)
        //        {
        //            pGeoField = pGeoFields.get_Field(i);
        //            pGDCOFields.set_Field(i - 27, pGeoField);
        //            strFieldName = pGeoField.Name;
        //            string strVal = "";
        //            strVal = (string)pGeoFeature.get_Value(i);

        //            //设置属性数据
        //            pGDCOFeature.put_Value(strFieldName, strVal);
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 2; i < iCount; i++)
        //        {
        //            pGeoField = pGeoFields.get_Field(i);
        //            pGDCOFields.set_Field(i - 2, pGeoField);
        //            strFieldName = pGeoField.Name;
        //            string strVal = pGeoFeature.get_Value(i).ToString();

        //            //设置属性数据
        //            pGDCOFeature.put_Value(strFieldName, strVal);
        //        }
        //    }

        //    //设置属性结构
        //    pGDCOFeature.Fields = pGDCOFields;

        //    //坐标单位，具体是什么填写什么
        //    pGDCOFeature.Unit = geoUnits.GEO_DEGREE;
        //    //坐标维数，具体是什么填写什么
        //    pGDCOFeature.Dimension = 2;
        //    //设置数据是否有效
        //    pGDCOFeature.IsValid = 1;
        //    //设置地物类型
        //    pGDCOFeature.Type = 1;

        //    return pGDCOFeature;
        //}
        #endregion
        private void txtXMLPath_TextChanged(object sender, EventArgs e)
        {
            string strXMLFile = this.txtXMLPath.Text;
            checkedXML.Items.Clear();
            //读模板XML
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strXMLFile);
            XmlNodeList vSubNodes = xmlDoc.SelectNodes("/zjMetadata/table");
            foreach (XmlNode vCurNode in vSubNodes)
            {
                checkedXML.Items.Add( vCurNode.Attributes["name"].Value,false);
            }
        }

        private void btnOpenXml_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog sOpenFileD = new System.Windows.Forms.OpenFileDialog();
            sOpenFileD.Multiselect = false ;
            sOpenFileD.Filter = "xml文件(*.xml)|*.xml|所有文件(*.*)|*.*";
            sOpenFileD.InitialDirectory = Application.StartupPath + "\\Res";
            if (sOpenFileD.ShowDialog() == DialogResult.OK)
            {
                this.txtXMLPath.Text  = sOpenFileD.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog sOpenFileD = new System.Windows.Forms.OpenFileDialog();
            sOpenFileD.CheckFileExists = true;
            sOpenFileD.CheckPathExists = true;
            sOpenFileD.Multiselect = true;
            sOpenFileD.Title = "选择元数据";
            sOpenFileD.Filter = "元数据库2000(*.mdb)|*.mdb|元数据库2007(*accdb)|*.accdb";
            int m = 1;
            if (sOpenFileD.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < checkedMDData.Items.Count; i++)
                {
                    if (sOpenFileD.FileName.ToString()==checkedMDData.Items[i].ToString())
                    {
                        if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            checkedMDData.Items.Add(sOpenFileD.FileName, true);
                            return;
                        }
                        else
                        {
                            m = 0;
                            break;
                        }
                    }

                }
                if(m==1)
                {
                checkedMDData.Items.Add(sOpenFileD.FileName,true);
                }
                //this.clbTable.Items.Add(System.IO.Path.GetFileNameWithoutExtension(sOpenFileD.FileName));
                
            }
            if (checkedMDData.Items.Count > 0)
            {
                button3.Enabled = true;
                button4.Enabled = true;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog pFolderBrowserDialog = new FolderBrowserDialog();
            int m = 1;
            if (pFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string[] pFilePathArr = null;
                pFilePathArr = Directory.GetFiles(pFolderBrowserDialog.SelectedPath, "*.mdb", SearchOption.TopDirectoryOnly);
                for (int j = 0; j < pFilePathArr.Length; j++)
                {
                    for (int i = 0; i < checkedMDData.Items.Count; i++)
                    {
                        if (pFilePathArr[j].ToString() == checkedMDData.Items[i].ToString())
                        {
                            if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                checkedMDData.Items.Add(pFilePathArr[j].ToString(), true);
                                return;
                            }
                            else
                            {
                                m = 0;
                                break;
                                
                            }

                        }
                    }
                    if(m==1)
                    {
                    checkedMDData.Items.Add(pFilePathArr[j].ToString(), true);
                    }
                }
                pFilePathArr = Directory.GetFiles(pFolderBrowserDialog.SelectedPath, "*.accdb", SearchOption.TopDirectoryOnly);
                m = 1;
                for (int j = 0; j < pFilePathArr.Length; j++)
                {
                    for (int i = 0; i < checkedMDData.Items.Count; i++)
                    {
                        if (pFilePathArr[j].ToString() == checkedMDData.Items[i].ToString())
                        {
                            if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                checkedMDData.Items.Add(pFilePathArr[j].ToString(), true);
                                return;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    if (m == 1)
                    {
                        checkedMDData.Items.Add(pFilePathArr[j].ToString(), true);
                    }
                }

               
            } 
            if (checkedMDData.Items.Count > 0)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                }
        }
        //private void checkedXML_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int Index = checkedXML.SelectedIndex;
        //    for (int i = 0; i < checkedXML.Items.Count;i++ )
        //    {
        //        if (i == Index)
        //        {
        //            this.checkedXML.SetItemChecked(Index, true);
        //        }
        //        else
        //        {
        //            this.checkedXML.SetItemChecked(i,false);
        //        }
        //    }
       
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            int Index = this.checkedMDData.SelectedIndex;
            if(Index!=-1)
            {
                checkedMDData.Items.Remove(checkedMDData.Items[Index].ToString());
            }
            if (checkedMDData.Items.Count == 0)
            {
                button3.Enabled = false;
                button4.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkedMDData.Items.Clear();
            button3.Enabled = false;
            button4.Enabled = false;
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
                if (!File.Exists(MDConnPath))
                {
                    //进行序列化
                    System.Collections.Hashtable conset = new System.Collections.Hashtable();
                    ////工作库 
                    conset.Add("PathName", conninfostr);
                    conset.Add("dbtype", type);
                    conset.Add("ID", pID);

                    SysCommon.Authorize.AuthorizeClass.Serialize(conset, MDConnPath);
                }
                //临时解决无法创建数据源问题
                List<string> strConnData = GetConnData(MDConnPath);
                if (strConnData[0] == "1" && !File.Exists(strConnData[1]))
                {
                    string oldPath = Application.StartupPath + "\\..\\MDxmlData\\work\\MD_data.mdb";
                    File.Copy(oldPath, strConnData[1], false);
                }
                IWorkspace pWorkspace = GetWorkSpacefromConninfo(conninfostr, type);
                if (pWorkspace == null) return;
                _dataWorkspace = pWorkspace;
            }
            catch
            {
            }
          
        }

       
       

      

    

 
    }
}
