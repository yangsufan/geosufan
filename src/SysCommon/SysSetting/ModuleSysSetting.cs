using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using Fan.Common.Gis;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;


namespace Fan.Common
{
    public static class ModSysSetting
    {
        public static string _MxdListTable = "自定义显示方案列表";   //added by chulili 2012-10-10 自定义显示方案列表相关参数
        public static string _MxdListTable_UserField = "USERNAME";
        public static string _MxdListTable_NameField = "NAME";
        public static string _MxdListTable_DescripField = "DESCRIPTION";
        public static string _MxdListTable_MapField = "MAP";
        public static string _MxdListTable_ShareField = "ISSHARE";
        public static string _MxdListTable_IDField = "OBJECTID";

        //加载xml文件
        public static string m_strInitXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\DataTreeInitIndex.xml";
        //added by chulili 图层目录是否修改的标志  以前放在GeoLayerTreeLib工程，现在挪到Fan.Common工程
        private static bool _IsLayerTreeChanged = false;//该变量用于配置系统内部
        public static bool IsLayerTreeChanged
        {
            set { _IsLayerTreeChanged = value; }
            get { return _IsLayerTreeChanged; }
        }
        private static bool _IsConfigLayerTreeChanged = false;//该变量用于展示系统与配置系统之间，专门用于从配置系统切换到展示系统后，是否需刷新
        public static bool IsConfigLayerTreeChanged
        {
            set { _IsConfigLayerTreeChanged = value; }
            get { return _IsConfigLayerTreeChanged; }
        }
        private static string _LogPath = Application.StartupPath + "\\..\\Log\\";
        //added by chulili 20111117 写日志的公共函数 ，可以不传入日志文件名，直接写在公共的日志文件里
        //参数含义：写日志内容
        public static void WriteLog(string strLog)
        {
            WriteLog("", strLog);
        }
        //added by chulili 20111117 写日志的公共函数
        //参数含义：日志文件名（不带扩展名） 写日志内容
        public static void WriteLog(string strFileName,string strLog)
        {
            //判断文件是否存在  不存在就创建添加写日志的函数，为了测试加载历史数据的效率
            if (strFileName == "")
            {
                strFileName = _LogPath + "Publiclog.txt";
            }
            else
            {
                strFileName = _LogPath +strFileName+ ".txt";
            }
            if (!File.Exists(strFileName))
            {
                System.IO.FileStream pFileStream = File.Create(strFileName);
                pFileStream.Close();
            }
            //FileStream fs = File.Open(_LogFilePath,FileMode.Append);

            //StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));

            string strTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string strread = strLog + "     " + strTime + "\r\n";
            StreamWriter sw = new StreamWriter(strFileName, true, Encoding.GetEncoding("gb2312"));
            sw.Write(strread);
            sw.Close();
            //fs.Close();
            sw = null;
            //fs = null;
        }
        public static string GetLinBanLayerNodeKey(IWorkspace pTmpWorkSpace)
        {
            string strConfigPath = Application.StartupPath + "\\..\\res\\xml\\小班查询.xml";
            Fan.Common.ModSysSetting.CopyConfigXml(pTmpWorkSpace, "最大林斑号", strConfigPath);
            string LinBanLayerKey = "";
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(strConfigPath);
            string strSearch = "//QueryConfig/QueryItem[@ItemText=" + "'最大林斑号查询'" + "]";
            XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
            if (pNode == null)
            {
                return "";
            }
            XmlNodeList pNodeList = pNode.SelectNodes(".//LayerItem");
            if (pNodeList.Count > 0)
            {
                XmlNode pXZnode = pNodeList[0];
                LinBanLayerKey = pXZnode.Attributes["NodeKey"].Value;//行政地名图层名
            }
            pXmlDoc = null;
            File.Delete(strConfigPath);
            return LinBanLayerKey;
        }
        //从数据库向本地拷贝
        public static IEnvelope GetFullExtent(IWorkspace pWorkspace)
        {
                
 
            string strMinX =GetSysSettingValue(pWorkspace,"X最小值");//MinX yjl20120813 modify 
            string strMinY = GetSysSettingValue(pWorkspace, "Y最小值");//MinY
            string strMaxX = GetSysSettingValue(pWorkspace, "X最大值");//MaxX
            string strMaxY = GetSysSettingValue(pWorkspace, "Y最大值");//MaxY
            try
            {
                double dMinX = Convert.ToDouble(strMinX);
                double dMinY = Convert.ToDouble(strMinY);
                double dMaxX = Convert.ToDouble(strMaxX);
                double dMaxY = Convert.ToDouble(strMaxY);
                Envelope pEnvelope = new Envelope();
                pEnvelope.XMin = dMinX;
                pEnvelope.XMax = dMaxX;
                pEnvelope.YMin = dMinY;
                pEnvelope.YMax = dMaxY;
                return pEnvelope as IEnvelope;
            }
            catch (Exception err)
            { }
            return null;
        }
        public static void CopyConfigXml(IWorkspace pWks, string Settingname, string SettingPath)
        {
            try
            {
                SysGisTable mSystable = new Fan.Common.Gis.SysGisTable(pWks);
                Exception err = null;
                Dictionary<string, object> pDic = mSystable.GetRow("SYSSETTING", "SETTINGNAME='" + Settingname + "'", out err);
                if (pDic != null)
                {
                    if (pDic.ContainsKey("SETTINGVALUE2"))
                    {
                        if (pDic["SETTINGVALUE2"] != null)  //这里仅能成功导出当初以文件类型导入的BLOB字段 
                        {
                            object tempObj = pDic["SETTINGVALUE2"];
                            IMemoryBlobStreamVariant pMemoryBlobStreamVariant = tempObj as IMemoryBlobStreamVariant;
                            IMemoryBlobStream pMemoryBlobStream = pMemoryBlobStreamVariant as IMemoryBlobStream;
                            if (pMemoryBlobStream != null)
                            {
                                pMemoryBlobStream.SaveToFile(SettingPath);
                            }
                        }
                    }
                }
                mSystable = null;
                if (pDic != null)
                {
                    pDic.Clear();
                    pDic = null;
                }
            }
            catch (Exception err2)
            { }
        }
        public static bool CopySelectedMap(IWorkspace pWorkspace, string strCondition, out IMap pMap)
        {
            pMap = null;
            if (pWorkspace != null)
            {
                try
                {
                    //读取数据库表内容
                    Fan.Common.Gis.SysGisTable sysTable = new Fan.Common.Gis.SysGisTable(pWorkspace);
                    Exception err = null;
                    Dictionary<string, object> pDic = sysTable.GetRow(_MxdListTable, strCondition , out err);
                    if (pDic != null)
                    {
                        if (pDic.ContainsKey(_MxdListTable_MapField))
                        {
                            if (pDic[_MxdListTable_MapField] != null)  //这里仅能成功导出当初以文件类型导入的BLOB字段 
                            {
                                object tempObj = pDic[_MxdListTable_MapField];
                                IMemoryBlobStreamVariant pMemoryValue;
                                pMemoryValue = (IMemoryBlobStreamVariant)tempObj;
                                object objValue;
                                pMemoryValue.ExportToVariant(out objValue);
                                byte[] pbyte = objValue as byte[];
                                IXMLStream pXMLStream = new XMLStream();
                                pXMLStream.LoadFromBytes(ref pbyte);
                                IStream pStream = pXMLStream as IStream;
                                IPersistStream pPersistStream = new MapClass();
                                pPersistStream.Load(pStream);
                                pMap = pPersistStream as IMap;

                                sysTable = null;
                                return true;
                            }
                        }
                    }
                    sysTable = null;
                }
                catch (Exception err)
                { }
            }
            return false;
        }
        public static bool CopySysSettingtoFile(IWorkspace pWorkspace,string strSettingName, string strPath)
        {
            Exception eError = null;
            if (pWorkspace != null)
            {
                try
                {
                    //读取数据库表内容
                    Fan.Common.Gis.SysGisTable sysTable = new Fan.Common.Gis.SysGisTable(pWorkspace);
                    Exception err = null;
                    Dictionary<string, object> pDic = sysTable.GetRow("SYSSETTING", "SETTINGNAME='" + strSettingName + "'", out err);
                    if (pDic != null)
                    {
                        if (pDic.ContainsKey("SETTINGVALUE2"))
                        {
                            if (pDic["SETTINGVALUE2"] != null)  //这里仅能成功导出当初以文件类型导入的BLOB字段 
                            {
                                object tempObj = pDic["SETTINGVALUE2"];
                                IMemoryBlobStreamVariant pMemoryBlobStreamVariant = tempObj as IMemoryBlobStreamVariant;
                                IMemoryBlobStream pMemoryBlobStream = pMemoryBlobStreamVariant as IMemoryBlobStream;
                                if (pMemoryBlobStream != null)
                                {
                                    pMemoryBlobStream.SaveToFile(strPath);
                                }
                                sysTable = null;
                                return true;
                            }
                        }
                    }
                    sysTable = null;
                }
                catch(Exception err)
                {}
            }
            return false;
        }
        
        public static IFeatureClass GetFeatureClassByNodeKey(IWorkspace pTmpWorkSpace, string XmlPath, string strNodeKey)
        {
            string strDatasource = "";
            return GetFeatureClassByNodeKey(pTmpWorkSpace, XmlPath, strNodeKey, out  strDatasource);
        }
        //added by chulili 20110802褚丽丽添加函数,根据nodeKey获取地物类,直接读取数据源连接信息,读取地物类
        public static IFeatureClass GetFeatureClassByNodeKey(IWorkspace pTmpWorkSpace, string XmlPath, string strNodeKey,out string strDataSourceKey)
        {
            strDataSourceKey = "";
            if (strNodeKey.Equals(""))
                return null;
            //目录树路径变量:_layerTreePath
            XmlDocument pXmldoc = new XmlDocument();
            if (!File.Exists(XmlPath))
            {
                return null;
            }
            //打开展示图层树,获取图层节点
            pXmldoc.Load(XmlPath);
            string strSearch = "//Layer[@NodeKey=" + "'" + strNodeKey + "'" + "]";
            XmlNode pNode = pXmldoc.SelectSingleNode(strSearch);
            if (pNode == null)
            {
                return null;
            }
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
            strDataSourceKey = strDBSourceID;
            //根据数据源id,获取数据源信息
            SysGisTable sysTable = new SysGisTable(pTmpWorkSpace);
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
            catch(Exception ex)
            { }
            return pFeaClass;

        }
        //自定义汇总统计
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
        //从数据库向本地拷贝图层树
        public static bool CopyLayerTreeXmlFromDataBase(IWorkspace pWorkspace, string xmlpath)
        {
            Exception eError = null;
            SysGisTable sysTable = new SysGisTable(pWorkspace);
            //读取数据库表内容
            object objXml = sysTable.GetFieldValue("LAYERTREE_XML", "LAYERTREE", "NAME='LAYERTREE'", out eError);
            if (objXml == null)
            {
                return false;
            }
            //获取xml文档
            XmlDocument pXml = objXml as XmlDocument;
            //保存到本地
            pXml.Save(xmlpath);
            return true;
        }
        //从数据库向本地拷贝图层树
        public static bool CopyResultFileTreeXmlFromDataBase(IWorkspace pWorkspace, string xmlpath)
        {
            Exception eError = null;
            SysGisTable sysTable = new SysGisTable(pWorkspace);
            //读取数据库表内容
            object objXml = sysTable.GetFieldValue("LAYERTREE_XML", "LAYERTREE", "NAME='RESULTFILETREE'", out eError);
            if (objXml == null)
            {
                return false;
            }
            //获取xml文档
            XmlDocument pXml = objXml as XmlDocument;
            //保存到本地
            pXml.Save(xmlpath);
            return true;
        }
        /// <summary>
        /// 获取系统参数值，传入业务库工作空间和参数名称
        /// </summary>
        /// <param name="TmpWorkspace"></param>
        /// <param name="SettingName"></param>
        /// <returns>strSettingValue</returns>
        ///         
        ///
        //通用函数，根据某张系统表CODE值获取NAME值
        public static string GetNameOfCode(IWorkspace TmpWorkspace, string TableName, string strCode)
        {
            string strName = "";
            if (TmpWorkspace == null)
                return strName;
            //使用SysGisTable类获取系统表中的记录
            Gis.SysGisTable sysTable = new Gis.SysGisTable(TmpWorkspace);

            Exception eError = null;
            //根据参数名获取记录
            Dictionary<string, object> dicData = sysTable.GetRow(TableName, "CODE='" + strCode + "'", out eError);
            //获取参数值
            if (dicData != null)
            {
                if (dicData.Count > 0)
                {
                    if (dicData.ContainsKey("NAME"))
                    {
                        if (dicData["NAME"] != null)
                        {
                            strName = dicData["NAME"].ToString();
                        }
                    }
                }
            }
            dicData = null;
            sysTable = null;
            return strName;
        }
        //通用函数，根据某张系统表NAME值获取CODE值
        public static string GetCodeOfName(IWorkspace TmpWorkspace, string TableName, string strName)
        {
            string strCode = "";
            if (TmpWorkspace == null)
                return strCode;
            //使用SysGisTable类获取系统表中的记录
            Gis.SysGisTable sysTable = new Gis.SysGisTable(TmpWorkspace);

            Exception eError = null;
            //根据参数名获取记录
            Dictionary<string, object> dicData = sysTable.GetRow(TableName, "NAME='" + strName + "'", out eError);
            //获取参数值
            if (dicData != null)
            {
                if (dicData.Count > 0)
                {
                    if (dicData.ContainsKey("CODE"))
                    {
                        if (dicData["CODE"] != null)
                        {
                            strCode = dicData["CODE"].ToString();
                        }
                    }
                }
            }
            dicData = null;
            sysTable = null;
            return strCode;
        }
        public static string GetSysSettingValue(IWorkspace TmpWorkspace, string SettingName)
        {
            string strSettingValue = "";
            if (TmpWorkspace == null)
                return strSettingValue;
            //使用SysGisTable类获取系统表中的记录
            Gis.SysGisTable sysTable = new Gis.SysGisTable(TmpWorkspace);
            
            Exception eError = null;
            //根据参数名获取记录
            Dictionary<string, object> dicData = sysTable.GetRow("SysSetting", "SettingName='" + SettingName + "'", out eError);
            //获取参数值
            if(dicData!=null)
            {
                if(dicData.Count>0)
                {
                    if (dicData.ContainsKey("SETTINGVALUE"))
                    {
                        if(dicData["SETTINGVALUE"]!=null)
                        {
                            strSettingValue = dicData["SETTINGVALUE"].ToString();
                        }
                    }
                }
            }
            dicData = null;
            sysTable = null;
            return strSettingValue;

        }
       


        //根据itemName获取tblName属性
        public static string GetTblNameByItemName(string strItemName)
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
        public static double GetExportAreaOfUser(IWorkspace pTmpWorkSpace, User pUser)
        {
            double ExportArea = -1;
            Gis.SysGisTable sysTable = new Gis.SysGisTable(pTmpWorkSpace);
            Exception eError = null;
            Dictionary<string, object> dicData = sysTable.GetRow("user_info", "NAME='" + pUser.UserName + "'", out eError);
            if (dicData != null && dicData.Count > 0)
            {
                if (dicData["EXPORTAREA"].ToString() != "")//added by chulili 20110926 提取面积限制
                {
                    ExportArea = Convert.ToDouble(dicData["EXPORTAREA"]);
                }
                return ExportArea;
            }
            return ExportArea;
 
        }
        public static string GetSystemName()
        {
            string xmlpath = Application.StartupPath + "\\..\\Res\\Xml\\SystemName.xml";
            string strSystemName = "";
            if (File.Exists(xmlpath))
            {
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.Load(xmlpath);

                XmlNode pNode= pXmldoc.SelectSingleNode("//SystemName");
                XmlElement pEle = pNode as XmlElement;

                if (pEle.HasAttribute("ItemName"))
                {
                    strSystemName = pEle.GetAttribute("ItemName");
                }
                pXmldoc = null;
            }
            return strSystemName;
        }
        public static string GetSysTables()
        {
            string strTables = "METADATA_LIB ";
            strTables = strTables + "MetaData_XML ";
            strTables = strTables + "MetaData_TEMP ";
            string xmlpath = Application.StartupPath + "\\..\\Template\\InitUserRoleConfig.Xml";
            XmlDocument doc = new XmlDocument();
            if (File.Exists(xmlpath))
            {
                doc.Load(xmlpath);
                string strSearch = "//InitSystemRoot";
                XmlNode pInitSystemnode = doc.SelectSingleNode(strSearch);
                XmlNodeList pInitSystemlist = null;
                if (pInitSystemnode != null)
                {
                    pInitSystemlist = pInitSystemnode.ChildNodes;
                    //遍历需要拷贝的表名
                    foreach (XmlNode pnode in pInitSystemlist)
                    {
                        if (pnode.NodeType == XmlNodeType.Element)
                        {
                            XmlElement pEle = pnode as XmlElement;
                            string strTableName = pEle.GetAttribute("Name");
                            if (strTableName != "")
                            {
                                strTables = strTables + strTableName + " ";
                            }
                        }
                    }
                }
            }
            doc = null;
            return strTables;
        }
        public static List<string> GetSysTableList()
        {
            List<string> strTables =new  List<string>();
            strTables.Add("METADATA_LIB");
            strTables.Add("MetaData_XML");
            strTables.Add("MetaData_TEMP");
            string xmlpath = Application.StartupPath + "\\..\\Template\\InitUserRoleConfig.Xml";
            XmlDocument doc = new XmlDocument();
            if (File.Exists(xmlpath))
            {
                doc.Load(xmlpath);
                string strSearch = "//InitSystemRoot";
                XmlNode pInitSystemnode = doc.SelectSingleNode(strSearch);
                XmlNodeList pInitSystemlist = null;
                if (pInitSystemnode != null)
                {
                    pInitSystemlist = pInitSystemnode.ChildNodes;
                    //遍历需要拷贝的表名
                    foreach (XmlNode pnode in pInitSystemlist)
                    {
                        if (pnode.NodeType == XmlNodeType.Element)
                        {
                            XmlElement pEle = pnode as XmlElement;
                            string strTableName = pEle.GetAttribute("Name");
                            if (strTableName != "")
                            {
                                strTables.Add(strTableName);
                            }
                        }
                    }
                }
            }
            doc = null;
            return strTables;
        }

    }

}