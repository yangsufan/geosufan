using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using System.Data.SqlClient;
using System.Data.OracleClient;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;

namespace GeoDBIntegration
{
    /// <summary>
    /// 公共静态函数类
    /// </summary>
    public static class ModDBOperate
    {

        private static string _LogFilePath = Application.StartupPath + "\\..\\Log\\GeoDBIntegration.txt";
        
        //added by chulili 20111118 判断当前图层目录中是否引用了某数据源
        //参数含义：数据源编号
        public static bool IsDbSourceInLayerTree(long DBid)
        {
            bool bRes = false;
            string strTempPath = Application.StartupPath + "\\..\\Res\\Xml\\TmpLayerTree.xml";
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, strTempPath);
            if (File.Exists(strTempPath))
            {
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.Load(strTempPath);
                string strSearch = "//Layer[@ConnectKey='"+DBid +"']";
                XmlNode pLayerNode = pXmldoc.SelectSingleNode(strSearch);
                if (pLayerNode != null)
                {
                    bRes = true;
                }
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pXmldoc);
                pXmldoc = null;
                pLayerNode = null;
                File.Delete(strTempPath);
            }
            return bRes;
        }
        
        //added by chulili 20111108 添加写日志的函数
        public static void WriteLog(string strLog)
        {
            //判断文件是否存在  不存在就创建添加写日志的函数，为了测试加载历史数据的效率
            if (!File.Exists(_LogFilePath))
            {
                System.IO.FileStream pFileStream = File.Create(_LogFilePath);
                pFileStream.Close();
            }
            //FileStream fs = File.Open(_LogFilePath,FileMode.Append);

            //StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));

            string strTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string strread = strLog + "     " + strTime + "\r\n";
            StreamWriter sw = new StreamWriter(_LogFilePath, true, Encoding.GetEncoding("gb2312"));
            sw.Write(strread);
            sw.Close();
            //fs.Close();
            sw = null;
            //fs = null;
        }
        /// <summary>
        /// 初始化系统界面   陈亚飞添加20100928
        /// </summary>
        /// <param name="xmlPath">子系统界面xml</param>
        public static void IntialSysFrm(string xmlPath, Form pForm)
        {
            Plugin.ModuleCommon.DicTabs.Clear();
            //读取子系统权限XML
            XmlDocument docXml = new XmlDocument();
            if (!File.Exists(xmlPath)) return;
            docXml.Load(xmlPath);

            try
            {
                //从插件文件夹中获取插件接口对象
                Plugin.Parse.PluginCollection pluginCol = ModuleData.m_PluginCol;
                if (pluginCol == null)
                {
                    Plugin.Parse.PluginHandle pluginHandle = new Plugin.Parse.PluginHandle();
                    pluginHandle.PluginFolderPath = ModuleData.m_PluginFolderPath;
                    pluginCol = pluginHandle.GetPluginFromDLL();
                    ModuleData.m_PluginCol = pluginCol;
                }

                //初始化主框架对象
                //if (!ModuleData.v_AppFormDic.ContainsKey(xmlPath))
                //{
                ModuleData.v_AppForm = new Plugin.Application.AppForm(pForm, null, docXml, null, null, pluginCol, ModuleData.m_PicPath);

                //分类解析、获取插件
                Plugin.ModuleCommon.IntialModuleCommon(docXml, ModuleData.m_PicPath, pluginCol, ModuleData.m_SysLogPath);

                //根据XML加载插件界面
                Plugin.ModuleCommon.LoadFormByXmlNode(ModuleData.v_AppForm as Plugin.Application.IApplicationRef);

                ModuleData.v_AppFormDic.Add(xmlPath, ModuleData.v_AppForm);
                //}
                //else
                //{
                //    ModuleData.v_AppForm = ModuleData.v_AppFormDic[xmlPath];
                //}
            }
            catch (System.Exception ex)
            {

            }
        }

        /// <summary>
        ///  刷新xml文件   陈亚飞添加   20101009　　　陈亚飞修改
        /// </summary>
        /// <param name="lDBTypeID">数据库类型ID：框架要素库、文件库、地名库、高程库、影像库、地理编码库</param>
        /// <param name="proID">数据库工程ID</param>
        /// <param name="proName">数据库工程名称</param>
        /// <param name="pScale">数据库比例尺</param>
        /// <param name="strConType">数据库连接类型</param>
        /// <param name="strConn">数据库连接信息</param>
        /// <param name="pParaDB">数据库参数信息</param>
        /// <param name="eError"></param>
        public static void RefeshXml(long lDBTypeID, string proID, string proName, string pScale, string strConType, string strConn, string pParaDB, out Exception eError)
        {
            eError = null;

            //若为框架要素库，将连接信息写入到xml中
            if (lDBTypeID.ToString() == enumInterDBType.框架要素数据库.GetHashCode().ToString())//cyf modify 20110614 :通过数据库类型ID来判断
            {
                //刷新框架要素库xml
                RefeshFeaXml(ModuleData.v_feaProjectXML,ModuleData.v_feaProjectXMLTemp, proID, proName, pScale, strConType, strConn,pParaDB,out eError);
                if (eError != null)
                {
                    return;
                }

            }
            else if (lDBTypeID.ToString() == enumInterDBType.高程数据库.GetHashCode().ToString())//cyf modify 20110614 :通过数据库类型ID来判断
            {
                //刷新高程要素库xml
                //cyf 20110608 modify
                //RefeshRasterXml(ModuleData.v_DEMProjectXml, ModuleData.v_DEMProjectXmlTemp, proID, proName, pScale, strConType, strConn, pParaDB, out eError);
                RefeshRasterXml(ModuleData.v_feaProjectXML, ModuleData.v_feaProjectXMLTemp, proID, proName, pScale, strConType, strConn, pParaDB, out eError);
                if (eError != null)
                {
                    return;
                }
                //end
            }
            else if (lDBTypeID.ToString() == enumInterDBType.影像数据库.GetHashCode().ToString())//cyf modify 20110614 :通过数据库类型ID来判断
            {
                //刷新影像要素库xml
               //cyf 20110608 modify
                //RefeshRasterXml(ModuleData.v_ImageProjectXml, ModuleData.v_ImageProjectXmlTemp, proID, proName, pScale, strConType, strConn,pParaDB, out eError);
                RefeshRasterXml(ModuleData.v_feaProjectXML, ModuleData.v_feaProjectXMLTemp, proID, proName, pScale, strConType, strConn, pParaDB, out eError);
                if (eError != null)
                {
                    return;
                }
                //end
            }
            else if (lDBTypeID.ToString() == enumInterDBType.成果文件数据库.GetHashCode().ToString())//cyf modify 20110614 :通过数据库类型ID来判断
            {

            }
            else if (lDBTypeID.ToString() == enumInterDBType.地名数据库.GetHashCode().ToString())//cyf modify 20110614 :通过数据库类型ID来判断
            {

            }
            else if (lDBTypeID.ToString() == enumInterDBType.地理编码数据库.GetHashCode().ToString())//cyf modify 20110614 :通过数据库类型ID来判断
            {

            }
        }
        
        /// <summary>
        /// 刷新 框架要素库子系统 xml文件   陈亚飞添加 20101009  陈亚飞修改20101011
        /// <param name="curXml">数据库子系统工程xml</param>
        /// <param name="xmlTemp">数据库子系统工程xml模板</param>
        /// <param name="proID">数据库工程ID</param>
        /// <param name="proName">数据库工程名称</param>
        /// <param name="pScale">数据库工程比例尺</param>
        /// <param name="strConType">数据库连接类型:PDB、GDB、SDE,针对ArcGIS平台</param>
        /// <param name="strConn">数据库连接信息</param>
        /// <param name="eError"></param>
        private static void RefeshFeaXml(string curXml, string xmlTemp, string proID, string proName, string pScale, string strConType, string strConn, string strParaDB, out Exception eError)
        {
            eError = null;
            string pServer = "";       //服务器
            string pInstance = "";     //服务名
            string pDB = "";           //数据库
            string pUser = "";         //用户
            string pPassword = "";     //密码
            string pVersion = "";      //版本
            string pDtName = "";       //数据集名称

            //获得数据库连接参数
            string[] strArr = strConn.Split(new char[] { '|' });
            if (strArr.Length != 7)
            {
                eError = new Exception("连接字符串设置有误！");
                return;
            }
            pServer = strArr[0];
            pInstance = strArr[1];
            pDB = strArr[2];
            pUser = strArr[3];
            pPassword = strArr[4];
            pVersion = strArr[5];
            pDtName = strArr[6];

            //加载xml文件
            if (!File.Exists(xmlTemp))
            {
                eError = new Exception("缺失模板文件,请检查!");
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(curXml))
            {
                //若工程xml文件不存在，则创建xml文件
                xmlDoc.LoadXml("<工程管理></工程管理>");
                xmlDoc.Save(curXml);
            }
            xmlDoc.Load(curXml);

            //cyf 20110623 add:若存在当前节点，则移除当前节点
            XmlNode ProNode = null;//当前工程节点
            try { ProNode = xmlDoc.DocumentElement.SelectSingleNode(".//工程[@编号='" + proID + "']"); }
            catch { }
            if (ProNode != null)
            {
                //若该工程节点已经存在，则将该节点删除
                xmlDoc.DocumentElement.RemoveChild(ProNode);
                xmlDoc.Save(curXml);
            }
            //end

            //创建工程管理节点下面的子节点 “工程”节点
            XmlElement proElement = xmlDoc.CreateElement("工程");//SelectNode(xmlDoc.DocumentElement, "工程") as XmlElement;//
            xmlDoc.DocumentElement.AppendChild(proElement as XmlNode);

            //加载工程模板xml文件
            XmlDocument xmlDocTemple = new XmlDocument();
            xmlDocTemple.Load(xmlTemp);
            //获得xml模板文件中的“工程”节点
            XmlNode nodeTemple = xmlDocTemple.SelectSingleNode(".//工程管理//工程");
            //将模板文件中的“工程”节点引入到新创建的xml文件中
            XmlNode DBXmlNodeNew = xmlDoc.ImportNode(nodeTemple, true);
            //设置工程节点的属性信息
            (DBXmlNodeNew as XmlElement).SetAttribute("编号", proID);
            (DBXmlNodeNew as XmlElement).SetAttribute("名称", proName);
            (DBXmlNodeNew as XmlElement).SetAttribute("比例尺", pScale);
            //用设置好的节点替换原有的节点
            xmlDoc.DocumentElement.ReplaceChild(DBXmlNodeNew, proElement as XmlNode);

            //工作库设置为false
            XmlNode tempNode = DBXmlNodeNew.SelectSingleNode(".//内容//工作库");
            if (tempNode != null)
            {
                XmlElement tempElem = tempNode as XmlElement;
                if (tempElem != null)
                {
                    tempElem.SetAttribute("是否显示", "false");
                }
            }
            //end

            //cyf 20110608 modify :将栅格数据库的属性设置为FALSE
            XmlNode RasterNode = DBXmlNodeNew.SelectSingleNode(".//内容//栅格数据库");
            if (RasterNode != null)
            {
                XmlElement RasterElem = RasterNode as XmlElement;
                if (RasterElem != null)
                {
                    RasterElem.SetAttribute("是否显示", "false");
                }
            }
            //end



            //内容节点
            XmlNode contextNode = DBXmlNodeNew.FirstChild;
            #region 遍历数据库子节点集合，设置数据库的连接属性
            foreach (XmlNode subNode in contextNode.ChildNodes)
            {
                string sVisible = (subNode as XmlElement).GetAttribute("是否显示");
                if (sVisible == bool.FalseString.ToLower()) continue;

                //连接信息节点
                XmlElement subElement = subNode.FirstChild as XmlElement;

                //设置连接信息属性
                if (strConType == enumInterDBFormat.ARCGISPDB.ToString())
                {
                    subElement.SetAttribute("类型", "PDB");
                    subElement.SetAttribute("数据库", pDB);

                }
                else if (strConType == enumInterDBFormat.ARCGISGDB.ToString())
                {
                    subElement.SetAttribute("类型", "GDB");
                    subElement.SetAttribute("数据库", pDB);
                }
                else if (strConType == enumInterDBFormat.ARCGISSDE.ToString())
                {
                    subElement.SetAttribute("类型", "SDE");
                    subElement.SetAttribute("服务器", pServer);
                    subElement.SetAttribute("服务名", pInstance);
                    subElement.SetAttribute("数据库", pDB);
                    subElement.SetAttribute("用户", pUser);
                    subElement.SetAttribute("密码", pPassword);
                    subElement.SetAttribute("版本", pVersion);
                }
                else if (strConType == enumInterDBFormat.GEOSTARACCESS.ToString())
                {
                    subElement.SetAttribute("类型", "Access");
                    subElement.SetAttribute("数据库", pDB);
                }
                else if (strConType == enumInterDBFormat.GEOSTARORACLE.ToString())
                {
                    subElement.SetAttribute("类型", "Oracle");
                    //subElement.SetAttribute("服务器", pPassword);
                    subElement.SetAttribute("数据库", pDB);
                    subElement.SetAttribute("用户", pUser);
                    subElement.SetAttribute("密码", pPassword);
                }
                else if (strConType == enumInterDBFormat.GEOSTARORSQLSERVER.ToString())
                {
                    subElement.SetAttribute("类型", "SQL Server");
                    subElement.SetAttribute("服务器", pServer);
                    subElement.SetAttribute("数据库", pDB);
                    subElement.SetAttribute("用户", pUser);
                    subElement.SetAttribute("密码", pPassword);
                }
                else if (strConType == enumInterDBFormat.ORACLESPATIAL.ToString())
                {
                    subElement.SetAttribute("服务器", pServer);
                    subElement.SetAttribute("用户", pUser);
                    subElement.SetAttribute("密码", pPassword);
                }


                //库体子节点
                XmlElement dbElem = subElement.FirstChild as XmlElement;
                //设置数据集名称
                dbElem.SetAttribute("名称", pDtName);

            }

            #endregion

            //设置图幅工作库  范围信息节点
            XmlElement rangeNode = contextNode.SelectSingleNode(".//图幅工作库//范围信息") as XmlElement;
            rangeNode.SetAttribute("范围", strParaDB);

            //保存设置
            xmlDoc.Save(curXml);
        }

        /// <summary>
        /// 刷新 高程数据库子系统和影像数据库子系统 xml文件   陈亚飞添加 20101012
        /// <param name="curXml">数据库子系统工程xml</param>
        /// <param name="xmlTemp">数据库子系统工程xml模板</param>
        /// <param name="proID">数据库工程ID</param>
        /// <param name="proName">数据库工程名称</param>
        /// <param name="pScale">数据库工程比例尺</param>
        /// <param name="strConType">数据库连接类型:PDB、GDB、SDE,针对ArcGIS平台</param>
        /// <param name="strConn">数据库连接信息</param>
        /// <param name="strParaDB">数据库参数信息</param>
        /// <param name="eError"></param>
        private static void RefeshRasterXml(string curXml, string xmlTemp, string proID, string proName, string pScale, string strConType, string strConn,string strParaDB,out Exception eError)
        {
            eError = null;
            string pServer = "";       //服务器
            string pInstance = "";     //服务名
            string pDB = "";           //数据库
            string pUser = "";         //用户
            string pPassword = "";     //密码
            string pVersion = "";      //版本
            string pDtName = "";       //数据集名称

            string dbtypeStr = "";        //存储类型
            string pResampleStr="";       //重采样方法
            string pCompressionStr = "";  //压缩类型
            string pPyramidStr = "";      //金字塔
            string pTileHStr = "";        //瓦片高度
            string pTileWStr = "";        //瓦片宽度
            string pBandStr = "";         //波段


            #region 获得数据库连接参数

            string[] strArr = strConn.Split(new char[] { '|' });
            if (strArr.Length != 7)
            {
                eError = new Exception("连接字符串设置有误！");
                return;
            }
            pServer = strArr[0];
            pInstance = strArr[1];
            pDB = strArr[2];
            pUser = strArr[3];
            pPassword = strArr[4];
            pVersion = strArr[5];
            pDtName = strArr[6];

            #endregion

            #region 获得栅格数据库参数
            if (strParaDB != "")
            {
                string[] paraArr = strParaDB.Split(new char[] { '|' });
                if (paraArr.Length != 7)
                {
                    eError = new Exception("栅格数据库参数设置有误！");
                    return;
                }
                dbtypeStr = paraArr[0];
                pResampleStr = paraArr[1];
                pCompressionStr = paraArr[2];
                pPyramidStr = paraArr[3];
                pTileHStr = paraArr[4];
                pTileWStr = paraArr[5];
                pBandStr = paraArr[6];
            }
            #endregion

            //加载xml文件
            if (!File.Exists(xmlTemp))
            {
                eError = new Exception("缺失模板文件,请检查!");
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(curXml))
            {
                //若工程xml文件不存在，则创建xml文件
                xmlDoc.LoadXml("<工程管理></工程管理>");
                xmlDoc.Save(curXml);
            }
            xmlDoc.Load(curXml);

            //创建工程管理节点下面的子节点 “工程”节点
            XmlElement proElement = xmlDoc.CreateElement("工程");//SelectNode(xmlDoc.DocumentElement, "工程") as XmlElement;//
            xmlDoc.DocumentElement.AppendChild(proElement as XmlNode);

            //加载工程模板xml文件
            XmlDocument xmlDocTemple = new XmlDocument();
            xmlDocTemple.Load(xmlTemp);
            //获得xml模板文件中的“工程”节点
            XmlNode nodeTemple = xmlDocTemple.SelectSingleNode(".//工程管理//工程");
            //将模板文件中的“工程”节点引入到新创建的xml文件中
            XmlNode DBXmlNodeNew = xmlDoc.ImportNode(nodeTemple, true);
            //设置工程节点的属性信息
            (DBXmlNodeNew as XmlElement).SetAttribute("编号", proID);
            (DBXmlNodeNew as XmlElement).SetAttribute("名称", proName);
            (DBXmlNodeNew as XmlElement).SetAttribute("比例尺", pScale);
            //用设置好的节点替换原有的节点
            xmlDoc.DocumentElement.ReplaceChild(DBXmlNodeNew, proElement as XmlNode);
            //cyf 20110608 modify :将现势库、历史库、工作库的属性设置为FALSE
            XmlNode tempNode = DBXmlNodeNew.SelectSingleNode(".//内容//现势库");
            XmlElement tempElem = null;
            if (tempNode != null)
            {
                tempElem = tempNode as XmlElement;
                if (tempElem != null)
                {
                    tempElem.SetAttribute("是否显示", "false");
                }
            }
            //历史库设置为FALSE
            tempNode = DBXmlNodeNew.SelectSingleNode(".//内容//历史库");
            if (tempNode != null)
            {
                tempElem = tempNode as XmlElement;
                if (tempElem != null)
                {
                    tempElem.SetAttribute("是否显示", "false");
                }
            }
            //工作库设置为false
            tempNode = DBXmlNodeNew.SelectSingleNode(".//内容//工作库");
            if (tempNode != null)
            {
                tempElem = tempNode as XmlElement;
                if (tempElem != null)
                {
                    tempElem.SetAttribute("是否显示", "false");
                }
            }
            //end

            XmlNode contextNode = DBXmlNodeNew.FirstChild;
            //遍历数据库子节点集合，设置数据库的连接属性
            foreach (XmlNode subNode in contextNode.ChildNodes)
            {
                string sVisible = (subNode as XmlElement).GetAttribute("是否显示");
                if (sVisible == bool.FalseString.ToLower()) continue;

                //设置数据库节点的“存储类型”属性
                (subNode as XmlElement).SetAttribute("存储类型", dbtypeStr);

                //连接信息节点
                XmlElement subElement = subNode.FirstChild as XmlElement;
                //设置连接信息属性
                if (strConType == "ARCGISPDB")
                {
                    subElement.SetAttribute("类型", "PDB");
                    subElement.SetAttribute("数据库", pDB);

                }
                else if (strConType == "ARCGISGDB")
                {
                    subElement.SetAttribute("类型", "GDB");
                    subElement.SetAttribute("数据库", pDB);
                }
                else if (strConType == "ARCGISSDE")
                {
                    subElement.SetAttribute("类型", "SDE");
                    subElement.SetAttribute("服务器", pServer);
                    subElement.SetAttribute("服务名", pInstance);
                    subElement.SetAttribute("数据库", pDB);
                    subElement.SetAttribute("用户", pUser);
                    subElement.SetAttribute("密码", pPassword);
                    subElement.SetAttribute("版本", pVersion);
                }

                //库体子节点
                XmlElement dbElem = subElement.FirstChild as XmlElement;
                //设置数据集名称
                dbElem.SetAttribute("名称", pDtName);

                //栅格数据参数节点
                XmlElement rasterParaEle = subNode.SelectSingleNode(".//栅格参数设置") as XmlElement;
                //设置栅格数据参数
                rasterParaEle.SetAttribute("重采样类型", pResampleStr);
                rasterParaEle.SetAttribute("压缩类型", pCompressionStr);
                rasterParaEle.SetAttribute("金字塔", pPyramidStr);
                rasterParaEle.SetAttribute("瓦片高度", pTileHStr);
                rasterParaEle.SetAttribute("瓦片宽度", pTileWStr);
                rasterParaEle.SetAttribute("波段", pBandStr);

                break;
            }

            //保存设置
            xmlDoc.Save(curXml);
        }


        /// <summary>
        /// 选中XML某节点若不存在则创建
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="strName">子节点名称</param>
        /// <returns></returns>
        private static XmlNode SelectNode(XmlElement parentNode, string strName)
        {
            XmlNode aNode = parentNode.SelectSingleNode(".//" + strName);
            if (aNode == null)
            {
                aNode = parentNode.OwnerDocument.CreateElement(strName) as XmlNode;
            }

            parentNode.AppendChild(aNode);
            return aNode;
        }


        /// <summary>
        /// 初始化子系统界面的选中状态   chenyafei  add 20110215  页面跳转
        /// </summary>
        /// <param name="pSysName">子系统name</param>
        /// <param name="pSysCaption">子系统caption</param>
        public static void InitialForm(string pSysName, string pSysCaption)
        {
            if (Plugin.ModuleCommon.DicTabs == null || Plugin.ModuleCommon.AppFrm == null) return;
            //初始化当前应用成素的名称和标题
            Plugin.ModuleCommon.AppFrm.CurrentSysName = pSysName;
            Plugin.ModuleCommon.AppFrm.Caption = pSysCaption;

            //显示选定的子系统界面
            bool bEnable = false;
            bool bVisible = false;
            if (Plugin.ModuleCommon.DicControls != null)
            {
                foreach (KeyValuePair<string, Plugin.Interface.IControlRef> keyValue in Plugin.ModuleCommon.DicControls)
                {
                    bEnable = keyValue.Value.Enabled;
                    bVisible = keyValue.Value.Visible;

                    Plugin.Interface.ICommandRef pCmd = keyValue.Value as Plugin.Interface.ICommandRef;
                    if (pCmd != null)
                    {
                        if (keyValue.Key == pSysName)
                        {
                            pCmd.OnClick();
                        }
                    }
                }
            }
            //默认显示子系统界面的第一项
            int i = 0;
            foreach (KeyValuePair<DevComponents.DotNetBar.RibbonTabItem, string> keyValue in Plugin.ModuleCommon.DicTabs)
            {
                if (keyValue.Value == pSysName)
                {
                    i = i + 1;
                    keyValue.Key.Visible = true;
                    keyValue.Key.Enabled = true;
                    if (i == 1)
                    {
                        //默认选中第一项
                        keyValue.Key.Checked = true;
                    }
                }
                else
                {
                    keyValue.Key.Visible = false;
                    keyValue.Key.Enabled = false;
                }
            }
        }

        /// <summary>
        /// chenyafei  20110314 add  content ：连接系统维护库  
        /// </summary>
        public static void ConnectDB(out SysCommon.DataBase.SysTable pSysDB, out Exception outError)
        {
            outError = null;
            pSysDB = new SysCommon.DataBase.SysTable();
            string pConnStr = ModuleData.v_AppConnStr;  //连接字符串
            if (pConnStr.Trim() == "")
            {
                outError = new Exception("连接系统维护库失败，系统维护库连接字符串信息为空！");
                return;
            }

            //连接数据库
            pSysDB.SetDbConnection(pConnStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out outError);
            if (outError != null)
            {
                outError = new Exception("连接系统维护库失败,原因：" + outError.Message);
                return;
            }
        }

        //根据XML节点获取连接信息
        public static object GetDBInfoByXMLNode(XmlElement dbElement, string strPath)
        {
            try
            {
                string strType = dbElement.GetAttribute("类型");
                string strServer = dbElement.GetAttribute("服务器");
                string strInstance = dbElement.GetAttribute("服务名");
                string strDB = dbElement.GetAttribute("数据库");
                if (strPath != "")
                {
                    strDB = strPath + strDB;
                }
                string strUser = dbElement.GetAttribute("用户");
                string strPassword = dbElement.GetAttribute("密码");
                string strVersion = dbElement.GetAttribute("版本");

                IPropertySet pPropSet = null;
                switch (strType.Trim().ToLower())
                {
                    case "pdb":
                        pPropSet = new PropertySetClass();
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        if (!File.Exists(strDB))
                        {
                            FileInfo filePDB = new FileInfo(strDB);
                            pAccessFact.Create(filePDB.DirectoryName, filePDB.Name, null, 0);
                        }
                        pPropSet.SetProperty("DATABASE", strDB);
                        IWorkspace pdbWorkspace = pAccessFact.Open(pPropSet, 0);
                        pAccessFact = null;
                        return pdbWorkspace;

                    case "gdb":
                        pPropSet = new PropertySetClass();
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        if (!Directory.Exists(strDB))
                        {
                            DirectoryInfo dirGDB = new DirectoryInfo(strDB);
                            pFileGDBFact.Create(dirGDB.Parent.FullName, dirGDB.Name, null, 0);
                        }
                        pPropSet.SetProperty("DATABASE", strDB);
                        IWorkspace gdbWorkspace = pFileGDBFact.Open(pPropSet, 0);
                        pFileGDBFact = null;
                        return gdbWorkspace;

                    case "sde":
                        pPropSet = new PropertySetClass();
                        IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
                        pPropSet.SetProperty("SERVER", strServer);
                        pPropSet.SetProperty("INSTANCE", strInstance);
                        pPropSet.SetProperty("DATABASE", strDB);
                        pPropSet.SetProperty("USER", strUser);
                        pPropSet.SetProperty("PASSWORD", strPassword);
                        pPropSet.SetProperty("VERSION", strVersion);
                        IWorkspace sdeWorkspace = pSdeFact.Open(pPropSet, 0);
                        pSdeFact = null;
                        return sdeWorkspace;

                    case "access":
                        System.Data.Common.DbConnection dbCon = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strDB);
                        dbCon.Open();
                        return dbCon;

                    case "oracle":
                        string strOracle = "Data Source=" + strDB + ";Persist Security Info=True;User ID=" + strUser + ";Password=" + strPassword + ";Unicode=True";
                        System.Data.Common.DbConnection dbConoracle = new OracleConnection(strOracle);
                        dbConoracle.Open();
                        return dbConoracle;

                    case "sql":
                        string strSql = "Data Source=" + strDB + ";Initial Catalog=" + strInstance + ";User ID=" + strUser + ";Password=" + strPassword;
                        System.Data.Common.DbConnection dbConsql = new SqlConnection(strSql);
                        dbConsql.Open();
                        return dbConsql;

                    default:
                        break;
                }

                return null;
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModuleData.v_SysLog != null)
                {
                    ModuleData.v_SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModuleData.v_SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                return null;
            }
        }

        /// <summary>
        /// 根据图层组名获取组图层

        /// </summary>
        /// <param name="pMapcontrol"></param>
        /// <param name="strName">图层组名称</param>
        /// <returns></returns>
        public static IGroupLayer GetGroupLayer(IMapControlDefault pMapcontrol, string strName)
        {
            IGroupLayer pGroupLayer = null;
            for (int i = 0; i < pMapcontrol.Map.LayerCount; i++)
            {
                ILayer pLayer = pMapcontrol.Map.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    if (pLayer.Name == strName)
                    {
                        pGroupLayer = pLayer as IGroupLayer;
                        break;
                    }
                }
            }
            return pGroupLayer;
        }

        // *---------------------------------------------------------------------------------------
        // *开 发 者：chenyafei
        // *开发时间：20110622
        // *功能函数：根据执行的条件查询数据库中的信息
        // *参    数：表名称（支持多表，以逗号隔开）、字段名称(支持多只段，以逗号隔开)、where字句，异常(输出)
        // *返 回 值：返回查询表格的游标
        public static ICursor GetCursor(IFeatureWorkspace pFeaWS, string tableName, string fieldName, string whereStr, out Exception outError)
        {
            outError = null;
            ICursor pCursor = null;
            IQueryDef pQueryDef = pFeaWS.CreateQueryDef();
            pQueryDef.Tables = tableName;
            pQueryDef.SubFields = fieldName;
            pQueryDef.WhereClause = whereStr;
            try
            {
                pCursor = pQueryDef.Evaluate();
                if (pCursor == null)
                {
                    outError = new Exception("查询数据库失败！");
                    return null; ;
                }
            }
            catch
            {
                outError = new Exception("查询数据库失败！");
                return null;
            }
            return pCursor;
        }

        // *-------------------------------------------------------------------------------------------------------
        // *功能函数：获取自动编号的下一个值
        // *开 发 者：陈亚飞
        // *开发日期：20110617
        // *参    数：表格所在工作空间，表格名称，自动编号的字段名，异常
        // *返    回：返回该字段的下一个值
        public static long GetMaxID(IFeatureWorkspace pFeaWs, string pTableName, string pFiledName, out Exception outError)
        {
            outError = null;
            long ReturnMaxID = -1;
            //获取表格
            ITable pTable = pFeaWs.OpenTable(pTableName);
            try
            {
                //表格行数
                long count = Convert.ToInt64(pTable.RowCount(null).ToString());
                if (count == 0)
                {
                    ReturnMaxID = 1;
                }
                else
                {
                    //若表格行数部位0行，则统计表格中该字段的最大值
                    IDataStatistics pDataSta = new DataStatisticsClass();
                    pDataSta.Field = pFiledName;
                    pDataSta.Cursor = pTable.Search(null, false);
                    IStatisticsResults pStaRes = null;
                    pStaRes = pDataSta.Statistics;
                    count = (long)pStaRes.Maximum;
                    //下一个值为最大值+1
                    ReturnMaxID = count + 1;
                }
                return ReturnMaxID;
            }
            catch (Exception eError)
            {
                outError = new Exception("获取自动编号的下一个值失败！");
                return -1;
            }
        }
    
    }
}
