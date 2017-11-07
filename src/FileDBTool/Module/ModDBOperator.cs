using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using stdole;
using System.IO;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;

namespace FileDBTool
{
    public static class ModDBOperator
    {
        /// <summary>
        /// 将时间添加到时间列表框中  陈亚飞编写

        /// </summary>
        /// <param name="ipStr"></param>
        /// <param name="eError"></param>
        public static void LoadComboxTime(string DBPath, out Exception eError)
        {
            eError = null;
            //string DBPath = "//" + ipStr + "//MetaDataBase//MetaDataBase.mdb";//元数据库路径
            SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();
           // string ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBPath + ";Persist Security Info=True";//元数据连接字符串
           // pSysDB.SetDbConnection(ConnStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
            //******************************************************************************************************
            //guozheng 2010-10-12 改为Oracle连接方式
            pSysDB.SetDbConnection(DBPath, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
            //******************************************************************************************************
            if (eError != null)
            {
                pSysDB.CloseDbConnection();
                return;
            }
            string str = "select distinct 生产日期 from ProductIndexTable";
            DataTable dt = pSysDB.GetSQLTable(str, out eError);
            if (eError != null)
            {
                pSysDB.CloseDbConnection();
                return;
            }
            if (ModData.v_ComboxTime.Items.Count > 0)
            {
                ModData.v_ComboxTime.Items.Clear();
            }
            ModData.v_ComboxTime.Items.Add("不选择");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string timestr = dt.Rows[i]["生产日期"].ToString();
                if (timestr != "")
                {
                    string sDate = string.Empty;
                    try
                    {
                        sDate = Convert.ToDateTime(timestr).ToShortDateString();
                    }
                    catch
                    {
                        sDate = DateTime.MinValue.ToShortDateString();
                    }
                    ModData.v_ComboxTime.Items.Add(sDate);
                }
            }
            ModData.v_ComboxTime.SelectedIndex = 0;
            ModData.v_ComboxTime.Enabled = true;
            ModData.v_ComboxTime.Visible = true;
            pSysDB.CloseDbConnection();
        }

        public static void LoadComboxTime(SysCommon.DataBase.SysTable in_pSysDB, out Exception eError)
        {
            eError = null;
            if (in_pSysDB == null)
            {
                eError = new Exception("元信息库连接信息未初始化");
                return;
            }
            SysCommon.DataBase.SysTable pSysDB = in_pSysDB;
            string str = "select distinct 生产日期 from ProductIndexTable";
            DataTable dt = pSysDB.GetSQLTable(str, out eError);
            if (eError != null)
            {
                pSysDB.CloseDbConnection();
                return;
            }
            if (ModData.v_ComboxTime.Items.Count > 0)
            {
                ModData.v_ComboxTime.Items.Clear();
            }
            ModData.v_ComboxTime.Items.Add("不选择");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string timestr = dt.Rows[i]["生产日期"].ToString();
                if (timestr != "")
                {
                    string sDate = string.Empty;
                    try
                    {
                        sDate = Convert.ToDateTime(timestr).ToShortDateString();
                    }
                    catch
                    {
                        sDate = DateTime.MinValue.ToShortDateString();
                    }
                    ModData.v_ComboxTime.Items.Add(sDate);
                }
            }
            ModData.v_ComboxTime.SelectedIndex = 0;
            ModData.v_ComboxTime.Enabled = true;
            ModData.v_ComboxTime.Visible = true;
            //pSysDB.CloseDbConnection();
        }

        /// <summary>
        /// 将数据库连接信息写入到xml中    陈亚飞编写

        /// </summary>
        /// <param name="nodeName">NodeName属性</param>
        /// <param name="nodeType">NodeText属性</param>
        /// <param name="nodeText">NodeType属性</param>
        /// <param name="connNode">数据库连接信息节点</param>
        /// <param name="eError"></param>
        private static XmlNode WriteToProXml(string metaDBConn, string nodeName, string nodeType, string nodeText, string user, string password, string instance, string version, out Exception eError)
        {
            eError = null;
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ModData.v_CoonectionInfoXML);

                XmlNode pNode = xml.SelectSingleNode(".//DBCatalog");//获得根节点

                XmlElement subElem = pNode.OwnerDocument.CreateElement("DataBase");
                subElem.SetAttribute("NodeName", nodeName);
                subElem.SetAttribute("NodeText", nodeText);
                subElem.SetAttribute("NodeType", nodeType);
                XmlElement aElem = null;
                if (nodeType == EnumTreeNodeType.DATABASE.ToString())
                {
                    aElem = subElem.OwnerDocument.CreateElement("连接信息");
                    aElem.SetAttribute("服务器", nodeText);
                    aElem.SetAttribute("用户", user);
                    aElem.SetAttribute("密码", password);
                    aElem.SetAttribute("实例名", instance);
                    aElem.SetAttribute("版本", version);
                    aElem.SetAttribute("MetaDBConn", metaDBConn);
                    subElem.AppendChild(aElem as XmlNode);
                }
                pNode.AppendChild(subElem as XmlNode);
                xml.Save(ModData.v_CoonectionInfoXML);
                return (aElem as XmlNode);
            }
            catch
            {
                eError = new Exception("数据库连接信息写入xml失败！");
                return null;
            }
        }


        /// <summary>
        /// 修改数据库连接信息xml    陈亚飞编写

        /// </summary>
        /// <param name="mNode">"DataBase"</param>
        /// <param name="nodeName">NodeName属性</param>
        /// <param name="nodeType">NodeText属性</param>
        /// <param name="nodeText">NodeType属性</param>
        /// <param name="connNode">数据库连接信息节点</param>
        /// <param name="eError"></param>
        public static XmlNode AlterProXml(DevComponents.AdvTree.Node orgNode, string metaDBConn, string ipStr, string user, string password, string instance, string version, out Exception eError)
        {
            eError = null;
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ModData.v_CoonectionInfoXML);
                //查找要修改的原始的xmlnode节点
                string nodeText = orgNode.Text;
                XmlElement orgSubNode = orgNode.Tag as XmlElement;

                XmlElement aElem = null;
                XmlNodeList nodeLst = xml.SelectNodes(".//DBCatalog//DataBase");
                if (orgSubNode != null)
                {
                    #region 存在子节点

                    string orgIP = orgSubNode.GetAttribute("服务器");
                    string orgUser = orgSubNode.GetAttribute("用户");
                    string orgPassword = orgSubNode.GetAttribute("密码");
                    string orgInstance = orgSubNode.GetAttribute("实例名");
                    string orgVersion = orgSubNode.GetAttribute("版本");
                    string orgMeta = orgSubNode.GetAttribute("MetaDBConn");

                    //string condition = "[@NodeText='" + nodeText + "']";
                    foreach (XmlNode pNode in nodeLst)
                    {
                        XmlNode mNode = null;
                        if ((pNode as XmlElement).GetAttribute("NodeText") == orgIP)
                        {
                            aElem = pNode.SelectSingleNode(".//连接信息") as XmlElement;//连接信息节点
                            if (aElem.GetAttribute("服务器") == orgIP && aElem.GetAttribute("用户") == orgUser && aElem.GetAttribute("密码") == orgPassword && aElem.GetAttribute("实例名") == orgInstance && aElem.GetAttribute("版本") == orgVersion && aElem.GetAttribute("MetaDBConn") == orgMeta)
                            {
                                //修改节点信息
                                aElem.SetAttribute("服务器", ipStr);
                                aElem.SetAttribute("用户", user);
                                aElem.SetAttribute("密码", password);
                                aElem.SetAttribute("实例名", instance);
                                aElem.SetAttribute("版本", version);
                                aElem.SetAttribute("MetaDBConn", metaDBConn);
                                mNode = pNode;
                            }
                            if (mNode != null)
                            {
                                XmlElement pElem = mNode as XmlElement;
                                pElem.SetAttribute("NodeText", ipStr);
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    //不存在子节点
                    aElem = xml.CreateElement("连接信息");
                    aElem.SetAttribute("服务器", ipStr);
                    aElem.SetAttribute("用户", user);
                    aElem.SetAttribute("密码", password);
                    aElem.SetAttribute("实例名", instance);
                    aElem.SetAttribute("版本", version);
                    aElem.SetAttribute("MetaDBConn", metaDBConn);

                    foreach (XmlNode pNode in nodeLst)
                    {
                        XmlNode temNode = pNode.SelectSingleNode(".//连接信息");

                        if ((pNode as XmlElement).GetAttribute("NodeText") == nodeText && temNode == null)
                        {
                            pNode.AppendChild(aElem as XmlNode);
                            XmlElement pElem = pNode as XmlElement;
                            pElem.SetAttribute("NodeText", ipStr);
                            break;
                        }
                    }
                }
                xml.Save(ModData.v_CoonectionInfoXML);
                return (aElem as XmlNode);
            }
            catch
            {
                eError = new Exception("修改数据库连接信息xml失败！");
                return null;
            }
        }

        // 查询  陈亚飞编写

        //=====================================================================================================================================
        //根据属性条件进行查询


        /// <summary>
        /// 创建数据信息数据源表格   陈亚飞编写

        /// </summary>
        public static DataTable CreateDataInfoTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", System.Type.GetType("System.String"));
            dt.Columns.Add("项目名称", System.Type.GetType("System.String"));
            dt.Columns.Add("产品名称", System.Type.GetType("System.String"));
            dt.Columns.Add("数据文件名", System.Type.GetType("System.String"));
            dt.Columns.Add("范围号", System.Type.GetType("System.String"));
            dt.Columns.Add("比例尺", System.Type.GetType("System.String"));
            dt.Columns.Add("存储位置", System.Type.GetType("System.String"));
            dt.Columns.Add("生产日期", System.Type.GetType("System.String"));
            dt.Columns.Add("数据类型", System.Type.GetType("System.String"));
            dt.Columns.Add("项目ID", System.Type.GetType("System.String"));
            dt.Columns.Add("产品ID", System.Type.GetType("System.String"));
            //dt.Columns.Add("存储时间", System.Type.GetType("System.String"));
            return dt;
        }

        private static DataTable CreateDataTable2()
        {
            DataTable dt = new DataTable();    //界面上显示表格

            dt.Columns.Add("ID", System.Type.GetType("System.String"));
            dt.Columns.Add("数据文件名", System.Type.GetType("System.String"));
            dt.Columns.Add("范围号", System.Type.GetType("System.String"));
            dt.Columns.Add("比例尺", System.Type.GetType("System.String"));
            dt.Columns.Add("存储位置", System.Type.GetType("System.String"));
            dt.Columns.Add("生产日期", System.Type.GetType("System.String"));
            //dt.Columns.Add("存储时间", System.Type.GetType("System.String"));
            dt.Columns.Add("数据类型", System.Type.GetType("System.String"));
            return dt;
        }

        /// <summary>
        /// 获取数据信息
        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="projectID">项目ID</param>
        /// <param name="productID">产品ID</param>
        /// <param name="dataFormatID">数据格式编码</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static DataTable GetDataInfo(SysCommon.DataBase.SysTable pSysDB, long productID, long projectID, out Exception eError)
        {
            DataTable dt1 = GetMapDataInfo(pSysDB, productID, out eError);
            if (eError != null) return null;
            DataTable dt2 = GetNonMapDataInfo(pSysDB, productID, out eError);
            if (eError != null) return null;
            DataTable dt3 = GetSCPDataInfo(pSysDB, productID, out eError);
            if (eError != null) return null;

            string projectName = GetProjectName(projectID, pSysDB, out eError);
            if (eError != null)
            {
                eError = new Exception("获取项目名称失败！");
                //return;
            }
            string productName = GetProductName(productID, pSysDB, out eError);
            if (eError != null)
            {
                eError = new Exception("获取产品名称失败！");
                //return;
            }

            DataTable dt = CreateDataInfoTable();// CreateDataTable();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                DataRow newRow = dt.NewRow();
                newRow["ID"] = dt1.Rows[i]["ID"].ToString();
                newRow["数据文件名"] = dt1.Rows[i]["数据文件名"].ToString();
                newRow["数据类型"] = "标准图幅数据";
                newRow["存储位置"] = dt1.Rows[i]["存储位置"].ToString();
                newRow["范围号"] = dt1.Rows[i]["图幅号"].ToString();
                newRow["比例尺"] = dt1.Rows[i]["图幅比例尺"].ToString();
                //newRow["存储时间"] = dt1.Rows[i]["存储时间"].ToString();
                newRow["生产日期"] = dt1.Rows[i]["生产日期"].ToString();
                newRow["项目ID"] = projectID.ToString();
                newRow["项目名称"] = projectName;
                newRow["产品ID"] = productID.ToString();
                newRow["产品名称"] = productName;
                dt.Rows.Add(newRow);
            }
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                DataRow newRow = dt.NewRow();
                newRow["ID"] = dt2.Rows[i]["ID"].ToString();
                newRow["数据文件名"] = dt2.Rows[i]["数据文件名"].ToString();
                newRow["数据类型"] = "非标准图幅数据";
                newRow["范围号"] = dt2.Rows[i]["块图号"].ToString();
                newRow["比例尺"] = dt2.Rows[i]["块图比例尺"].ToString();
                newRow["存储位置"] = dt2.Rows[i]["存储位置"].ToString();
                //newRow["存储时间"] = dt2.Rows[i]["存储时间"].ToString();
                newRow["生产日期"] = dt2.Rows[i]["生产日期"].ToString();
                newRow["项目ID"] = projectID.ToString();
                newRow["项目名称"] = projectName;
                newRow["产品ID"] = productID.ToString();
                newRow["产品名称"] = productName;
                dt.Rows.Add(newRow);
            }
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                DataRow newRow = dt.NewRow();
                newRow["ID"] = dt3.Rows[i]["ID"].ToString();
                newRow["数据文件名"] = dt3.Rows[i]["数据文件名"].ToString();
                newRow["数据类型"] = "控制点数据";
                newRow["存储位置"] = dt3.Rows[i]["存储位置"].ToString();
                //newRow["存储时间"] = dt3.Rows[i]["存储时间"].ToString();
                newRow["生产日期"] = dt3.Rows[i]["生产日期"].ToString();
                newRow["项目ID"] = projectID.ToString();
                newRow["项目名称"] = projectName;
                newRow["产品ID"] = productID.ToString();
                newRow["产品名称"] = productName;
                dt.Rows.Add(newRow);
            }
            return dt;
        }

        /// <summary>
        /// 获取标准图幅数据
        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="projectID"></param>
        /// <param name="productID"></param>
        /// <param name="dataFormatID"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static DataTable GetMapDataInfo(SysCommon.DataBase.SysTable pSysDB, long productID, out Exception eError)
        {
            eError = null;
            string str = "SELECT * FROM StandardMapMDTable  where 产品ID=" + productID;
            DataTable dt = pSysDB.GetSQLTable(str, out  eError);
            if (eError != null)
            {
                eError = new Exception("获取标准图幅数据出错！");
                return null;
            }
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dataFormatID == 0)
            //    {
            //        dt.Rows[i]["数据格式编号"] = "DLG";
            //    }
            //    else if (dataFormatID == 1)
            //    {
            //        dt.Rows[i]["数据格式编号"] = "DEM";
            //    }
            //    else if (dataFormatID == 2)
            //    {
            //        dt.Rows[i]["数据格式编号"] = "DOM";
            //    }
            //    else if (dataFormatID == 3)
            //    {
            //        dt.Rows[i]["数据格式编号"] = "DRG";
            //    }
            //}
            return dt;
        }

        /// <summary>
        /// 获取非标准图幅数据

        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="projectID"></param>
        /// <param name="productID"></param>
        /// <param name="dataFormatID"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static DataTable GetNonMapDataInfo(SysCommon.DataBase.SysTable pSysDB, long productID, out Exception eError)
        {
            eError = null;
            string str = "SELECT * FROM NonstandardMapMDTable where 产品ID=" + productID;
            DataTable dt = pSysDB.GetSQLTable(str, out eError);
            if (eError != null)
            {
                eError = new Exception("获取非标准图幅数据出错！");
                return null;
            }
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dataFormatID == 0)
            //    {
            //        dt.Rows[i]["数据格式编号"] = "DLG";
            //    }
            //    else if (dataFormatID == 1)
            //    {
            //        dt.Rows[i]["数据格式编号"] = "DEM";
            //    }
            //    else if (dataFormatID == 2)
            //    {
            //        dt.Rows[i]["数据格式编号"] = "DOM";
            //    }
            //    else if (dataFormatID == 3)
            //    {
            //        dt.Rows[i]["数据格式编号"] = "DRG";
            //    }
            //}
            return dt;
        }

        /// <summary>
        /// 获取控制点测量数据

        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="projectID"></param>
        /// <param name="productID"></param>
        /// <param name="dataFormatID"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static DataTable GetSCPDataInfo(SysCommon.DataBase.SysTable pSysDB, long productID, out Exception eError)
        {
            eError = null;
            DataTable rdt = new DataTable();
            string str = "SELECT * FROM ControlPointMDTable where 产品ID=" + productID;
            DataTable dt = pSysDB.GetSQLTable(str, out eError);
            if (eError != null)
            {
                eError = new Exception("获取控制点测量数据出错！");
                return null;
            }

            return dt;
        }


        /// <summary>
        /// 根据产品ID获得产品名称
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="sysTable"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static string GetProductName(long productID, SysCommon.DataBase.SysTable sysTable, out Exception eError)
        {
            eError = null;
            string productName = "";
            try
            {
                string str = "select * from ProductMDTable where ID=" + productID;
                DataTable dt = sysTable.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    return productName;
                }
                if (dt.Rows.Count == 0) return productName;
                productName = dt.Rows[0]["产品名称"].ToString();
                return productName;
            }
            catch (Exception ex)
            {
                eError = ex;
                return "";
            }
        }
        /// <summary>
        /// 根据项目ID获得项目名称
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="sysTable"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static string GetProjectName(long projectID, SysCommon.DataBase.SysTable sysTable, out Exception eError)
        {
            eError = null;
            try
            {
                string str = "select * from ProjectMDTable where ID=" + projectID;
                DataTable dt = sysTable.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    return "";
                }
                string projectName = dt.Rows[0]["项目名称"].ToString();
                return projectName;
            }
            catch (System.Exception ex)
            {
                eError = ex;
                return "";
            }
        }


        //================================================================================================================================
        //根据范围进行查询


        /// <summary>
        /// 根据范围获得范围号列表

        /// </summary>
        /// <param name="pGeo"></param>
        /// <param name="pFeatureClass"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public static string GetRangNoLst(IGeometry pGeo, List<IDataset> lstDT, out Exception eError)
        {
            eError = null;
            try
            {
                string RangNOstr = "";
                StringBuilder RangeNOLst = new StringBuilder();
                foreach (IDataset pDt in lstDT)
                {
                    try
                    {
                        if (pDt.Name == "ProjectRange") continue;
                        IFeatureClass pFeatureClass = pDt as IFeatureClass;
                        int index = -1;
                        index = pFeatureClass.Fields.FindField("MAP_NEWNO");
                        if (index == -1)
                        {
                            eError = new Exception("在范围图层" + (pFeatureClass as IDataset).Name + "找不到字段'范围号'");
                            continue;
                        }
                        ISpatialFilter pFilter = new SpatialFilterClass();
                        pFilter.Geometry = pGeo;
                        pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        IFeatureCursor pCursor = pFeatureClass.Search(pFilter as IQueryFilter, false);
                        if (pCursor == null) return null;
                        IFeature pFeature = pCursor.NextFeature();
                        while (pFeature != null)
                        {
                            string pRangeNO = pFeature.get_Value(index).ToString();
                            if (pRangeNO != "")
                            {
                                RangeNOLst.Append("'");
                                RangeNOLst.Append(pRangeNO);
                                RangeNOLst.Append("',");
                            }
                            pFeature = pCursor.NextFeature();
                        }

                        //释放cursor
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    }
                    catch
                    {
                        continue;
                    }
                }
                RangNOstr = RangeNOLst.ToString().Substring(0, RangeNOLst.ToString().Length - 1);
                return RangNOstr;
            }
            catch (Exception ex)
            {
                eError = ex;
                return "";
            }
        }

        /// <summary>
        /// 根据范围条件查询数据信息
        /// </summary>
        /// <param name="pGeometry"></param>
        /// <param name="m_MapControl"></param>
        /// <param name="m_hook"></param>
        public static void QueryDataByGeometry(IGeometry pGeometry, Plugin.Application.IAppFileRef m_hook)
        {
            Exception eError = null;

            DevComponents.AdvTree.Node mDBNode = null;//定义数据库根节点
            DevComponents.AdvTree.Node mProNode = null;//项目根节点
            mDBNode = m_hook.ProjectTree.SelectedNode;
            mProNode = m_hook.ProjectTree.SelectedNode;
            while (mDBNode.Parent != null)
            {
                mDBNode = mDBNode.Parent;
            }
            while (mProNode.DataKey.ToString() != EnumTreeNodeType.PROJECT.ToString())
            {
                mProNode = mProNode.Parent;
            }
            if (mDBNode == null) return;
            if (mDBNode.DataKey.ToString() != EnumTreeNodeType.DATABASE.ToString() || mDBNode.Name != "文件连接") return;
            XmlElement dbElem = mDBNode.Tag as XmlElement;
            if (dbElem == null) return;
            string ipStr = dbElem.GetAttribute("MetaDBConn");

            //属性库连接类
            SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();
            //string ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ipStr + ";Persist Security Info=True";//元数据连接字符串

            //设置数据库连接
            pSysDB.SetDbConnection(ipStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！");
                pSysDB.CloseDbConnection();
                return;
            }
            //根据项目ID查找图幅结合表
            string stra = "select * from ProjectMDTable where ID=" + long.Parse(mProNode.Tag.ToString().Trim());
            DataTable proTable = pSysDB.GetSQLTable(stra, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接范围数据库失败！");
                pSysDB.CloseDbConnection();
                return;
            }
            if (proTable.Rows.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "找不到ID为：" + mProNode.Tag.ToString() + "的项目");
                pSysDB.CloseDbConnection();
                return;
            }
            string proPath = proTable.Rows[0]["图幅结合表"].ToString().Trim();


            //范围库连接信息
            SysCommon.Gis.SysGisDataSet pSysGis = new SysCommon.Gis.SysGisDataSet();
            pSysGis.SetWorkspace(proPath, SysCommon.enumWSType.PDB, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "设置图幅结合表工作空间失败！");
                return;
            }
            //获得范围要素类
            List<IDataset> lstDT = pSysGis.GetAllFeatureClass();

            //获得范围号列表
            string RangeNoList = ModDBOperator.GetRangNoLst(pGeometry, lstDT, out eError);
            if (eError != null)
            {
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                //return;
            }

            #region 根据范围号列表查找数据
            try
            {
                //申明表格保存查询的所有结果

                //DataTable allDT = new DataTable();
                #region 执行查找
                //for (int j = 0; j < RangeNoList.Count; j++)
                //{
                string strSel = "select * from ProductIndexTable where 范围号 in (" + RangeNoList + ")";
                DataTable dt = pSysDB.GetSQLTable(strSel, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查询成果索引表失败！");
                    pSysDB.CloseDbConnection();
                    return;
                }
                //if (dt.Rows.Count == 0)
                //{
                //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "在范围" + RangeNoList[j] + "找不到任何产品！");
                //    pSysDB.CloseDbConnection();
                //    return;
                //}


                //声明保存查询结果的表格

                DataTable dataDT = CreateDataInfoTable();
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    long productID = long.Parse(dt.Rows[k]["产品ID"].ToString());//产品ID
                    long dataID = long.Parse(dt.Rows[k]["数据ID"].ToString());   //数据ID
                    int dataTypeID = int.Parse(dt.Rows[k]["数据类型编号"].ToString());
                    long projectID = long.Parse(dt.Rows[k]["项目ID"].ToString());//项目ID
                    string projectName = GetProjectName(projectID, pSysDB, out eError);
                    if (eError != null)
                    {
                        eError = new Exception("获取项目名称失败！");
                        return;
                    }
                    string productName = GetProductName(productID, pSysDB, out eError);
                    if (eError != null)
                    {
                        eError = new Exception("获取产品名称失败！");
                        return;
                    }

                    string str = "";
                    if (dataTypeID == 0)
                    {
                        //标准图幅
                        str = "SELECT * FROM StandardMapMDTable  where  ID=" + dataID;
                    }
                    else if (dataTypeID == 1)
                    {
                        //非标准图幅 
                        str = "SELECT * FROM NonstandardMapMDTable  where  ID=" + dataID;
                    }
                    //查询产品范围内的数据
                    DataTable tempDT = pSysDB.GetSQLTable(str, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    for (int n = 0; n < tempDT.Rows.Count; n++)
                    {
                        DataRow aRow = dataDT.NewRow();
                        aRow["ID"] = tempDT.Rows[n]["ID"].ToString();
                        aRow["数据文件名"] = tempDT.Rows[n]["数据文件名"].ToString();
                        aRow["存储位置"] = tempDT.Rows[n]["存储位置"].ToString();
                        aRow["生产日期"] = tempDT.Rows[n]["生产日期"].ToString();
                        aRow["项目ID"] = projectID.ToString();
                        aRow["项目名称"] = projectName;
                        aRow["产品ID"] = productID.ToString();
                        aRow["产品名称"] = productName;
                        if (dataTypeID == 0)
                        {
                            //标准图幅
                            aRow["数据类型"] = "标准图幅数据";
                            aRow["范围号"] = tempDT.Rows[n]["图幅号"].ToString();
                            aRow["比例尺"] = tempDT.Rows[n]["图幅比例尺"].ToString();

                        }
                        else if (dataTypeID == 1)
                        {
                            //非标准图幅 
                            aRow["数据类型"] = "非标准图幅数据";
                            aRow["范围号"] = tempDT.Rows[n]["块图号"].ToString();
                            aRow["比例尺"] = tempDT.Rows[n]["块图比例尺"].ToString();

                        }
                        dataDT.Rows.Add(aRow);
                    }

                    //if (k == 0)
                    //{
                    //    allDT = dataDT.Copy();
                    //}
                    //else
                    //{
                    //    for (int m = 0; m < dataDT.Rows.Count; m++)
                    //    {
                    //        DataRow newRow = allDT.NewRow();
                    //        newRow.ItemArray = dataDT.Rows[m].ItemArray;
                    //        allDT.Rows.Add(newRow);
                    //    }

                    //}
                }
                //}
                #endregion

                //绑定表格
                m_hook.DataInfoGrid.Parent.BringToFront();
                m_hook.DataInfoGrid.BringToFront();
                if (m_hook.DataInfoGrid.DataSource != null)
                {
                    m_hook.DataInfoGrid.DataSource = null;
                }
                m_hook.DataInfoGrid.DataSource = dataDT;
                m_hook.DataInfoGrid.ReadOnly = true;
                m_hook.DataInfoGrid.Visible = true;
                m_hook.DataInfoGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
              
                for (int j = 0; j < m_hook.DataInfoGrid.Columns.Count; j++)
                {
                    m_hook.DataInfoGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    //m_hook.DataInfoGrid.Columns[j].Width = (m_hook.DataInfoGrid.Width - 20) / m_hook.DataInfoGrid.Columns.Count;
                }
                m_hook.DataInfoGrid.RowHeadersWidth = 20;
                m_hook.DataInfoGrid.Update();
                m_hook.DataInfoGrid.Refresh();

                pSysDB.CloseDbConnection();
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", ex.Message);
                pSysDB.CloseDbConnection();
                return;
            }
            #endregion
        }

        /// <summary>
        /// 数据信息列表分页显示，加载页面记录　　陈亚飞编写

        /// </summary>
        /// <param name="DT">数据源</param>
        /// <param name="currenPage">当前页</param>
        /// <param name="recNum">上一个页面的最后一条记录</param>
        /// <returns></returns>
        public static void LoadPage(Plugin.Application.IAppFileRef m_Hook, DataTable DT, int currenPage, int recNum)
        {
            if (DT == null) return;
            int totalRec = DT.Rows.Count;//总记录数
            int pageSize = ModData.pageSize;//每页包含的记录数
            int pageCount = totalRec / pageSize;//总页数

            if (totalRec % pageSize > 0)
            {
                pageCount += 1;
            }

            if (pageCount == 0)
            {
                m_Hook.DataInfoGrid.DataSource = null;
                m_Hook.TxtDisplayPage.Enabled = false;
                m_Hook.BtnLast.Enabled = false;
                m_Hook.BtnNext.Enabled = false;
                m_Hook.BtnFirst.Enabled = false;
                m_Hook.BtnPre.Enabled = false;
                return;
            }
            ModData.TotalPageCount = pageCount;

            int starRec;
            int endRec;
            DataTable tempDT = DT.Clone();
            if (currenPage == pageCount)
            {
                endRec = totalRec;
            }
            else
            {
                endRec = pageSize * currenPage;
            }
            starRec = recNum;
            for (int i = starRec; i < endRec; i++)
            {
                tempDT.ImportRow(DT.Rows[i]);
                recNum += 1;
            }

            //清空更新对比列表
            if (m_Hook.DataInfoGrid.DataSource != null)
            {
                m_Hook.DataInfoGrid.DataSource = null;
            }
            //将表格绑定到DataGrid上

            m_Hook.DataInfoGrid.DataSource = tempDT;
            m_Hook.DataInfoGrid.Visible = true;
            m_Hook.DataInfoGrid.ReadOnly = true;
            m_Hook.DataInfoGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            for (int j = 0; j < m_Hook.DataInfoGrid.Columns.Count; j++)
            {
                m_Hook.DataInfoGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                //m_Hook.DataInfoGrid.Columns[j].Width = (m_Hook.DataInfoGrid.Width - 20) / m_Hook.DataInfoGrid.Columns.Count;
            }
            m_Hook.DataInfoGrid.RowHeadersWidth = 20;
            m_Hook.DataInfoGrid.Refresh();
            //分页文本框信息显示

            m_Hook.TxtDisplayPage.ControlText = currenPage.ToString() + "/" + pageCount.ToString();

            //设置按是否可用

            if (currenPage == pageCount)
            {
                //若为最后一页，则"最后一页"按钮和“下一页”按钮都不可用

                m_Hook.BtnLast.Enabled = false;
                m_Hook.BtnNext.Enabled = false;
            }
            else
            {
                m_Hook.BtnLast.Enabled = true;
                m_Hook.BtnNext.Enabled = true;
            }
            if (currenPage == 1)
            {
                //若为第一页，则该按钮不可用

                m_Hook.BtnFirst.Enabled = false;
                m_Hook.BtnPre.Enabled = false;
            }
            else
            {
                m_Hook.BtnFirst.Enabled = true;
                m_Hook.BtnPre.Enabled = true;
            }
        }

        /// <summary>
        /// 增加树子节点 陈亚飞

        /// </summary>
        /// <param name="pNodes">树节点集合</param>
        /// <param name="nodeType">DataKey</param>
        /// <param name="nodeText">Text</param>
        /// <param name="nodeTag">Tag</param>
        /// <param name="nodeName">Name</param>
        public static DevComponents.AdvTree.Node AppendNode(DevComponents.AdvTree.NodeCollection pNodes, string nodeType, string nodeText, object nodeTag, string nodeName, int imageIndex)
        {
            DevComponents.AdvTree.Node aNode = new DevComponents.AdvTree.Node();
            aNode.DataKey = nodeType;
            aNode.Text = nodeText;
            aNode.Tag = nodeTag;
            aNode.Name = nodeName;
            aNode.ImageIndex = imageIndex;
            aNode.Expanded = false;
            pNodes.Add(aNode);
            return aNode;
        }


        /// <summary>
        /// 连接数据库后，从数据库读取文件夹，并将相应的节点附加在树图上   陈亚飞

        /// </summary>
        /// <param name="aNodes">数据库节点</param>
        /// <param name="metaDBConn">数据库连接字符串</param>
        /// <param name="eError"></param>
        private static void AppendSubFolderNode(DevComponents.AdvTree.NodeCollection aNodes, string metaDBConn, out Exception eError)
        {
            eError = null;
            SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();    //属性库连接类

          //  string ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + metaDBConn + ";Persist Security Info=True";//元数据连接字符串
          //  pSysDB.SetDbConnection(ConnStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
            //**************************************************************************************************************
            //2010-10-11 guozheng
            //修改为Oracle连接方式
            pSysDB.SetDbConnection(metaDBConn, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
            //**************************************************************************************************************
            if (eError != null)
            {
                eError = new Exception("连接元数据库失败！连接地址为：" + metaDBConn);
                pSysDB.CloseDbConnection();
                return;
            }
            string selStr = "";

            //查询项目元信息表
            selStr = "select * from ProjectMDTable";
            DataTable dt = pSysDB.GetSQLTable(selStr, out eError);
            if (eError != null)
            {
                eError = new Exception("查询项目元信息表失败！");
                pSysDB.CloseDbConnection();
                return;
            }
            #region 遍历项目元信息表，在数据库树节点后附加项目节点

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                long projectID = long.Parse(dt.Rows[i]["ID"].ToString());    //项目ID
                string projectName = dt.Rows[i]["项目名称"].ToString();      //项目名称
                string projectPath = dt.Rows[i]["存储位置"].ToString();      //存储位置
                //执行附加节点操作
                DevComponents.AdvTree.Node ProNode = null;    //项目树节点

                ProNode = AppendNode(aNodes, EnumTreeNodeType.PROJECT.ToString(), projectName, projectID, projectPath, 3);


                XmlDocument dataFormatInfoXml = new XmlDocument();
                dataFormatInfoXml.Load(ModData.v_ProjectInfoXML);
                XmlNodeList Nodelist = dataFormatInfoXml.SelectSingleNode(".//项目目录").ChildNodes;
                #region 遍历项目xml文件，在项目节点后附加数据格式节点（DLG、DEM、DOM、DRG）

                foreach (XmlNode formatPathNode in Nodelist)
                {
                    string dataFormatName = formatPathNode.InnerText.ToString();   //数据格式名称
                    int dataFormatID = -1;                                                      //数据格式ID
                    if (dataFormatName == EnumDataFormat.DLG.ToString())
                    {
                        dataFormatID = 0;
                    }
                    else if (dataFormatName == EnumDataFormat.DEM.ToString())
                    {
                        dataFormatID = 1;
                    }
                    else if (dataFormatName == EnumDataFormat.DOM.ToString())
                    {
                        dataFormatID = 2;
                    }
                    else if (dataFormatName == EnumDataFormat.DRG.ToString())
                    {
                        dataFormatID = 3;
                    }
                    if (dataFormatID == -1) continue;
                    string dataFormatPath = projectPath + "/" + dataFormatName;              //数据格式路径
                    //执行附加节点操作
                    DevComponents.AdvTree.Node dataFormatNode = null;//数据格式树节点

                    dataFormatNode = AppendNode(ProNode.Nodes, EnumTreeNodeType.DATAFORMAT.ToString(), dataFormatName, dataFormatID, dataFormatPath, 3);


                    //查询产品元信息表
                    selStr = "select * from ProductMDTable where 项目ID=" + projectID + " and 数据格式编号=" + dataFormatID;
                    DataTable productDT = pSysDB.GetSQLTable(selStr, out eError);
                    if (eError != null)
                    {
                        eError = new Exception("查询产品元数据库信息失败！");
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    #region 遍历产品元信息表，在数据格式节点下附加产品节点

                    for (int j = 0; j < productDT.Rows.Count; j++)
                    {
                        string productID = productDT.Rows[j]["ID"].ToString();           //产品ID
                        string productName = productDT.Rows[j]["产品名称"].ToString();   //产品名称
                        string productPath = productDT.Rows[j]["存储位置"].ToString();   //产品存储位置
                        //执行附加节点操作
                        DevComponents.AdvTree.Node productNode = null;//产品节点
                        productNode = AppendNode(dataFormatNode.Nodes, EnumTreeNodeType.PRODUCT.ToString(), productName, productID, productPath, 3);


                        XmlDocument dataTypeInfoXml = new XmlDocument();
                        dataTypeInfoXml.Load(ModData.v_ProductInfoXML);
                        XmlNodeList dataTypeNodelist = dataTypeInfoXml.SelectSingleNode(".//产品目录").ChildNodes;
                        #region 遍历产品xml文件，在产品节点后面附加数据类型子节点

                        foreach (XmlNode dataPathNode in dataTypeNodelist)
                        {
                            string dataTypeName = dataPathNode.InnerText;               //数据类型名称
                            int dataTypeID = -1;                                        //数据类型ID
                            string dataTypePath = "";                                   //数据类型路径

                            DevComponents.AdvTree.Node dataTypeNode = null;           //数据类型树节点

                            if (dataTypeName == EnumDataType.标准图幅.ToString())
                            {
                                dataTypeID = 0;
                                dataTypePath = productPath + "/" + dataTypeName;
                                //执行附加节点操作
                                dataTypeNode = AppendNode(productNode.Nodes, EnumTreeNodeType.PRODUCTPYPE.ToString(), dataTypeName, dataTypeID, dataTypePath, 3);


                                selStr = "select * from StandardMapMDTable where 产品ID=" + productID;
                                DataTable standardDataDT = pSysDB.GetSQLTable(selStr, out eError);
                                if (eError != null)
                                {
                                    eError = new Exception("查询标准图幅数据元信息失败！");
                                    pSysDB.CloseDbConnection();
                                    return;
                                }
                                #region 遍历标准图幅数据元信息，在数据类型节点下增加标准图幅数据节点
                                for (int k = 0; k < standardDataDT.Rows.Count; k++)
                                {
                                    long standarDataID = long.Parse(standardDataDT.Rows[k]["ID"].ToString());  //标准图幅数据ID
                                    string standarDataName = standardDataDT.Rows[k]["数据文件名"].ToString();  //标准图幅数据名称
                                    string standarDataPath = standardDataDT.Rows[k]["存储位置"].ToString();    //标准图幅数据路径
                                    //执行附加节点操作
                                    DevComponents.AdvTree.Node standardDataNode = null;                                     //标准图幅数据树节点

                                    standardDataNode = AppendNode(dataTypeNode.Nodes, EnumTreeNodeType.DATAITEM.ToString(), standarDataName, standarDataID, standarDataPath, 4);
                                }
                                #endregion
                            }
                            else if (dataTypeName == EnumDataType.非标准图幅.ToString())
                            {
                                dataTypeID = 1;
                                dataTypePath = productPath + "/" + dataTypeName;
                                //执行附加节点操作
                                dataTypeNode = AppendNode(productNode.Nodes, EnumTreeNodeType.PRODUCTPYPE.ToString(), dataTypeName, dataTypeID, dataTypePath, 3);


                                selStr = "select * from NonstandardMapMDTable where 产品ID=" + productID;
                                DataTable nonStandardDataDT = pSysDB.GetSQLTable(selStr, out eError);
                                if (eError != null)
                                {
                                    eError = new Exception("查询标准图幅数据元信息失败！");
                                    pSysDB.CloseDbConnection();
                                    return;
                                }
                                #region 遍历非标准图幅数据元信息，在数据类型节点下增加非标准图幅数据节点
                                for (int k = 0; k < nonStandardDataDT.Rows.Count; k++)
                                {
                                    long nonStandarDataID = long.Parse(nonStandardDataDT.Rows[k]["ID"].ToString());  //非标准图幅数据ID
                                    string nonStandarDataName = nonStandardDataDT.Rows[k]["数据文件名"].ToString();  //非标准图幅数据名称

                                    string nonStandarDataPath = nonStandardDataDT.Rows[k]["存储位置"].ToString();    //非标准图幅数据路径

                                    //执行附加节点操作
                                    DevComponents.AdvTree.Node nonStandardDataNode = null;                           //非标准图幅数据树节点
                                    nonStandardDataNode = AppendNode(dataTypeNode.Nodes, EnumTreeNodeType.DATAITEM.ToString(), nonStandarDataName, nonStandarDataID, nonStandarDataPath, 4);
                                }
                                #endregion
                            }
                            else if (dataTypeName == EnumDataType.控制点数据.ToString())
                            {
                                dataTypeID = 2;
                                dataTypePath = productPath + "/" + dataTypeName;
                                //执行附加节点操作
                                dataTypeNode = AppendNode(productNode.Nodes, EnumTreeNodeType.PRODUCTPYPE.ToString(), dataTypeName, dataTypeID, dataTypePath, 3);


                                selStr = "select * from ControlPointMDTable where 产品ID=" + productID;
                                DataTable SCPDataDT = pSysDB.GetSQLTable(selStr, out eError);
                                if (eError != null)
                                {
                                    eError = new Exception("查询标准图幅数据元信息失败！");
                                    pSysDB.CloseDbConnection();
                                    return;
                                }
                                #region 遍历标准图幅数据元信息，在数据类型节点下增加标准图幅数据节点
                                for (int k = 0; k < SCPDataDT.Rows.Count; k++)
                                {
                                    long SCPDataID = long.Parse(SCPDataDT.Rows[k]["ID"].ToString());   //标准图幅数据ID
                                    string SCPDataName = SCPDataDT.Rows[k]["数据文件名"].ToString();  //标准图幅数据名称
                                    string SCPDataPath = SCPDataDT.Rows[k]["存储位置"].ToString();    //标准图幅数据路径
                                    //执行附加节点操作
                                    DevComponents.AdvTree.Node SCPDataNode = null;                        //标准图幅数据树节点

                                    SCPDataNode = AppendNode(dataTypeNode.Nodes, EnumTreeNodeType.DATAITEM.ToString(), SCPDataName, SCPDataID, SCPDataPath, 4);
                                }
                                #endregion
                            }

                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            #endregion

            pSysDB.CloseDbConnection();
        }


        /// <summary>
        /// 连接数据库，并初始化界面  陈亚飞

        /// </summary>
        /// <param name="metaDBConn"></param>
        /// <param name="IP"></param>
        /// <param name="ID"></param>
        /// <param name="Password"></param>
        /// <param name="eError"></param>
        public static void ConnectDB(string metaDBConn, string IP, string ID, string Password, out Exception eError)
        {
            eError = null;
            FTP_Class Ftp = new FTP_Class(IP, ID, Password);
            string error = "";
            Ftp.Connecttest(IP, ID, Password, out error);
            if (error == "Succeed")
            {
                //连接成功
                DevComponents.AdvTree.Node DBNode = null;//定义数据库根节点
                if (ModData.v_AppFileDB.ProjectTree.SelectedNode.DataKey.ToString() == EnumTreeNodeType.DATACONNECT.ToString())
                {
                    #region 新增的连接

                    //将数据库连接信息写入xml中

                    XmlNode pNode = WriteToProXml(metaDBConn, "文件连接", EnumTreeNodeType.DATABASE.ToString(), IP, ID, Password, "", "", out eError);
                    if (eError != null) return;

                    //将数据库节点挂在树上
                    DBNode = AppendNode(ModData.v_AppFileDB.ProjectTree.Nodes, EnumTreeNodeType.DATABASE.ToString(), IP, pNode, "文件连接", 2);
                    //DBNode = new DevComponents.AdvTree.Node();
                    //DBNode.Name = "文件连接";
                    //DBNode.Text = IP;  //ftp地址
                    //DBNode.DataKey = EnumTreeNodeType.DATABASE.ToString();//树节点类型

                    //DBNode.Tag = pNode;                //登陆FTP连接信息
                    //DBNode.ImageIndex = 2;
                    //DBNode.Expanded = false;
                    //ModData.v_AppFileDB.ProjectTree.Nodes.Add(DBNode);                 
                    #endregion
                }
                else if (ModData.v_AppFileDB.ProjectTree.SelectedNode.DataKey.ToString() == EnumTreeNodeType.DATABASE.ToString())
                {
                    DBNode = ModData.v_AppFileDB.ProjectTree.SelectedNode;
                    ModData.v_AppFileDB.ProjectTree.SelectedNode.Nodes.Clear();
                    #region 已有的连接

                    //修改已有的xml中数据库连接信息
                    XmlNode pNode = AlterProXml(ModData.v_AppFileDB.ProjectTree.SelectedNode, metaDBConn, IP, ID, Password, "", "", out eError);
                    if (eError != null) return;

                    //修改已有的数据库树节点信息

                    ModData.v_AppFileDB.ProjectTree.SelectedNode.Text = IP;
                    ModData.v_AppFileDB.ProjectTree.SelectedNode.Tag = pNode;
                    ModData.v_AppFileDB.ProjectTree.SelectedNode.ImageIndex = 2;
                    ModData.v_AppFileDB.ProjectTree.SelectedNode.Expanded = false;
                    #endregion
                }

                //获取数据库中的文件夹并将它们附加到树节点上

                AppendSubFolderNode(DBNode.Nodes, metaDBConn, out eError);
                if (eError != null)
                {
                    //删除节点
                    //DBNode.Remove();
                    return;
                }

                //初始化界面（时间列表框）
                LoadComboxTime(metaDBConn, out eError);
                if (eError != null)
                {
                    eError = new Exception("初始化时间列表框失败！" + eError.Message);
                    return;
                }
                //添加底图
                //AddRangeLayer(metaDBConn,out eError);
                //if(eError!=null)
                //{
                //    return;
                //}
            }
            else
            {
                eError = new Exception("连接失败！");
                return;
            }
        }

        public static void AddRangeLayer(string metaDBConn, out Exception eError)
        {
            //添加底图
            ModData.v_AppFileDB.MapControl.ClearLayers();

            SysCommon.Gis.SysGisDataSet pSysGis = new SysCommon.Gis.SysGisDataSet();
            pSysGis.SetWorkspace(metaDBConn, SysCommon.enumWSType.PDB, out eError);
            if (eError != null)
            {
                eError = new Exception("添加底图失败，连接不到数据库！");
                return;
            }
            IFeatureLayer featurelayer = new FeatureLayerClass();
            IFeatureClass featureclass = pSysGis.GetFeatureClass("ProjectRange", out eError);
            if (eError != null)
            {
                return;
            }
            featurelayer.FeatureClass = featureclass;
            ILayer layer = featurelayer as ILayer;
            layer.Name = "项目范围图";

            ModData.v_AppFileDB.MapControl.Map.AddLayer(layer);
            //图层显示标注
            SetLableToGeoFeatureLayer(featurelayer as IGeoFeatureLayer, "NAME", 0, ModData.v_AppFileDB.MapControl.ReferenceScale );
        }

        /// <summary>
        /// 设置属性标注

        /// </summary>
        /// <param name="pGeoFeatureLayer">图层</param>
        /// <param name="vLabelField">属性字段</param>
        /// <param name="vMapFrameScale">图层比例尺</param>
        /// <param name="vMapRefrenceScale">参考比例尺</param>
        public static void SetLableToGeoFeatureLayer(IGeoFeatureLayer pGeoFeatureLayer, string vLabelField, int vMapFrameScale, double vMapRefrenceScale)
        {
            IAnnotateLayerPropertiesCollection pAnnoLayerProperCol = pGeoFeatureLayer.AnnotationProperties;
            IAnnotateLayerProperties pAnnoLayerProper;
            IElementCollection placedElements;
            IElementCollection unplacedElements;


            //得到当前层的当前标注属性

            pAnnoLayerProperCol.QueryItem(0, out pAnnoLayerProper, out placedElements, out unplacedElements);
            ILabelEngineLayerProperties pLabelEngineLayerProper = (ILabelEngineLayerProperties)pAnnoLayerProper;
            IBasicOverposterLayerProperties4 pBasicOverposterLayerProper = (IBasicOverposterLayerProperties4)pLabelEngineLayerProper.BasicOverposterLayerProperties;
            //标注的字体

            ITextSymbol pTextSymbol = pLabelEngineLayerProper.Symbol;
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.Red = 0;
            pRGBColor.Blue = 255;
            pRGBColor.Green = 0;
            pTextSymbol.Color = pRGBColor;
            stdole.StdFont pStdFont = new stdole.StdFontClass();
            IFontDisp pFont = (IFontDisp)pStdFont;
            pFont.Name = "宋体";
            if (vMapRefrenceScale != 0 && vMapFrameScale != 0)
            {
                double size = (vMapFrameScale / 30) * vMapFrameScale / vMapRefrenceScale;
                //size = (ModData.v_AppFileDB.MapControl.Map.MapScale / ModData.v_AppFileDB.MapControl.Map.ReferenceScale) * 16;
                pFont.Size = (decimal)size;
            }

            pTextSymbol.Font = pFont;
            //标注内容
            pLabelEngineLayerProper.Expression = "[" + vLabelField + "]";
            pBasicOverposterLayerProper.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerPart;
            //标注的方向信息

            pBasicOverposterLayerProper.PolygonPlacementMethod = esriOverposterPolygonPlacementMethod.esriAlwaysHorizontal;
            //标注的与几何图形的大小关系

            //pBasicOverposterLayerProper.PlaceOnlyInsidePolygon = true;//仅在地物内部显示标注  deleted by chulili s20111018 界面上并没有这项设置，这句话应注释掉，否则像是错误
            //开启标注

            pGeoFeatureLayer.DisplayAnnotation = true;
        }


        /// <summary>
        /// 组合字段值(Orcle)

        /// </summary>
        /// <param name="columnType"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public static string GetSQl(Type columnType, string columnValue)
        {
            string str = "";

            if (columnType.FullName == "System.String")
            {
                str = "'" + columnValue + "'";
            }
            else if (columnType.FullName == "System.Int32")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                str = columnValue;
            }
            else if (columnType.FullName == "System.Int16")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                str = columnValue;

            }
            else if (columnType.FullName == "System.Int64")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                str = columnValue;
            }
            else if (columnType.FullName == "System.Double")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                str = columnValue;
            }
            else if (columnType.FullName == "System.Single")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                str = columnValue;
            }
            else if (columnType.FullName == "System.DateTime")
            {
                string sValue = string.Empty;
                if (columnValue == "")
                {
                    sValue = DateTime.MinValue.ToShortDateString();
                }
                else
                {
                    try
                    {
                        sValue = Convert.ToDateTime(columnValue).ToShortDateString();
                    }
                    catch
                    {
                        sValue = DateTime.MinValue.ToShortDateString();
                    }
                }
               // str = "#" + columnValue + "#";
                //guozheng 2010-10-12 Oracle 日期入库方式
                str = "to_date('" + sValue + "','ýyyy-mm-dd')";
            }
            else if (columnType.FullName == "System.Decimal")
            {
                try
                {                   
                    str=System.Convert.ToDouble(columnValue).ToString();
                }
                catch
                {
                    str = string.Empty;
                }
            }
            return str;
        }
        /// <summary>
        /// 组合字段值(OleDb)

        /// </summary>
        /// <param name="columnType"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public static string GetSQlEX(Type columnType, string columnValue)
        {
            string str = "";

            if (columnType.FullName == "System.String")
            {
                str = "'" + columnValue + "'";
            }
            else if (columnType.FullName == "System.Int32")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                str = columnValue;
            }
            else if (columnType.FullName == "System.Int16")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                str = columnValue;

            }
            else if (columnType.FullName == "System.Int64")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                str = columnValue;
            }
            else if (columnType.FullName == "System.Double")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                str = columnValue;
            }
            else if (columnType.FullName == "System.Single")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                str = columnValue;
            }
            else if (columnType.FullName == "System.DateTime")
            {
                string sValue = string.Empty;
                if (columnValue == "")
                {
                    sValue = DateTime.MinValue.ToShortDateString();
                }
                else
                {
                    try
                    {
                        sValue = Convert.ToDateTime(columnValue).ToShortDateString();
                    }
                    catch
                    {
                        sValue = DateTime.MinValue.ToShortDateString();
                    }
                }
                str = "#" + columnValue + "#";
                //guozheng 2010-10-12 Oracle 日期入库方式
                //str = "to_date('" + sValue + "','ýyyy-mm-dd')";
            }
            else if (columnType.FullName == "System.Decimal")
            {
                try
                {
                    str = System.Convert.ToDouble(columnValue).ToString();
                }
                catch
                {
                    str = string.Empty;
                }
            }
            return str;
        }

        /// <summary>
        /// 删除一条范围记录（非标准图幅）
        /// </summary>
        /// <param name="pDataID"></param>
        /// <param name="pSysTable"></param>
        /// <param name="pSysDT"></param>
        /// <param name="eError"></param>
        public static void DeleteOneNonStandardRange(long pDataID, SysCommon.DataBase.SysTable pSysTable, SysCommon.Gis.SysGisDataSet pSysDT, out Exception eError)
        {
            eError = null;
            string pScale = "";          //比例尺
            string pRangNO = "";         //块图号 
            string pfeaClsName = "";     //要删除的要素类名称
            string condiStr = "";        //删除的条件
            string str = "select * from NonstandardMapMDTable where ID=" + pDataID;
            DataTable tempDT = pSysTable.GetSQLTable(str, out eError);
            if (eError != null)
            {
                eError = new Exception("查询非标准数据元信息失败!");
                return;
            }
            if (tempDT.Rows.Count == 0)
            {
                eError = new Exception("非标准数据元信息表为空!");
                return;
            }
            pScale = tempDT.Rows[0]["块图比例尺"].ToString().Trim();
            pRangNO = tempDT.Rows[0]["块图号"].ToString().Trim();
            //////////////////判断块图号是否有重复的引用////////
            str = "SELECT COUNT(*) FROM NonstandardMapMDTable WHERE  块图号='" + pRangNO + "'";
            tempDT = pSysTable.GetSQLTable(str, out eError);
            if (eError != null)
            {
                eError = new Exception("删除数据范围失败！数据ID为：" + pDataID);
                return;
            }
            if (null != tempDT)
            {
                int count =int.Parse(tempDT.Rows[0][0].ToString());
                if (count > 1)
                {
                    return;
                }
            }
            ////////////////////////////////////////////////////
            if (pScale != "" && pRangNO != "")
            {
                pfeaClsName = "Range_" + pScale;
                condiStr = "MAP_NEWNO='" + pRangNO + "'";
                pSysDT.DeleteRows(pfeaClsName, condiStr, out eError);
                if (eError != null)
                {
                    eError = new Exception("删除数据范围失败！数据ID为：" + pDataID);
                    return;
                }
            }

        }


        #region 下载文件
        /// <summary>
        /// 下载ftp服务器上文件到本地制定路径

        /// </summary>
        /// <param name="ConIP">连接ip</param>
        /// <param name="ConID">连接用户ID</param>
        /// <param name="ConPassword">连接用户密码</param>
        /// <param name="ftpFilePath">ftp上文件相对于根目录的路径</param>
        /// <param name="ftpFileName">ftp服务器上的文件名</param>
        /// <param name="SaveName">文件保存名</param>
        /// <param name="SavePath">文件保存路径</param>
        /// <param name="error">out错误信息</param>
        /// <returns></returns>
        public static bool DownloadFile(string ConIP, string ConID, string ConPassword, string ftpFilePath, string ftpFileName, string SaveName, string SavePath, out string error)
        {
            error = "";
            FTP_Class ftp = new FTP_Class(ConIP, ConID, ConPassword);
            bool state = ftp.Download(ftpFilePath + "/" + ftpFileName, SavePath + "/" + SaveName, SaveName, out error);
            ftp.Close();
            if (state)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion
        #region 上传文件
        /// <summary>
        /// 上传文件至ftp服务器制定目录下并制定文件名
        /// </summary>
        /// <param name="ConIP">连接ip</param>
        /// <param name="ConID">连接用户id</param>
        /// <param name="ConPassword">连接用户密码</param>
        /// <param name="FilePath">本地文件路径</param>
        /// <param name="FileName">本地文件名</param>
        /// <param name="ftpFilePath">ftp服务器文件保存路径</param>
        /// <param name="ftpSaveFileName">ftp服务器文件保存名</param>
        /// <param name="error">out错误信息</param>
        /// <returns></returns>
        public static bool UpLoadFile(string ConIP, string ConID, string ConPassword, string FilePath, string FileName, string ftpFilePath, string ftpSaveFileName, out  string error)
        {
            error = "";
            FTP_Class ftp = new FTP_Class(ConIP, ConID, ConPassword);
            bool state = ftp.Upload(FilePath + "/" + FileName, ftpFilePath, ftpSaveFileName, out error);
            ftp.Close();
            if (state)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion
        #region 文件库新建一个产品

        /// <summary>
        /// 文件库新建一个产品(先在工程树上选中一个数据类型节点DEM,Dom,DLG,DRG)
        /// </summary>
        /// <param name="productName">产品名称</param>
        /// <param name="ProjectTree">工程目树</param>
        /// <returns></returns>
        public static bool CreatProduct(string productName, long productID, DevComponents.AdvTree.Node SelNode, out Exception ex)
        {
            ex = null;
            if (null == SelNode.DataKey)
            {
                ex = new Exception("节点类型不正确");
                return false;
            }//
            if (EnumTreeNodeType.DATAFORMAT.ToString() != SelNode.DataKey.ToString())
            {
                ex = new Exception("节点类型不正确");
                return false;
            }//选中的不是产品类型节点（dlg,dom,dem,drg）返回

            ///////////////获取产品目录xml文件下的目录信息///////////////////
            XmlDocument Doc = new XmlDocument();
            Doc.Load(ModData.v_ProductInfoXML);
            XmlNodeList Nodelist = Doc.SelectSingleNode("产品目录").ChildNodes;
            if (null == Nodelist)
            {
                ex = new Exception("未能获取产品子目录信息!");
                return false;
            }
            StringBuilder result = new StringBuilder();
            String[] Floders;
            foreach (XmlNode Node1 in Nodelist)
            {
                result.Append(Node1.InnerText);
                result.Append("\n");
            }
            result.Remove(result.ToString().LastIndexOf('\n'), 1);
            Floders = result.ToString().Split('\n');
            if (null == Floders)
            {
                ex = new Exception("未能获取产品子目录信息!");
                return false;
            }
            ///////////////获取ftp连接信息，工程信息///////////////////
            long ProjectId;
            string IP;
            string Id;
            string Password;
            string Path = SelNode.Name;

            DevComponents.AdvTree.Node mDBNode = SelNode;
            while (mDBNode.Parent != null)
            {
                mDBNode = mDBNode.Parent;
            }
            if (mDBNode.Name == "文件连接")
            {
                System.Xml.XmlElement dbElem = mDBNode.Tag as System.Xml.XmlElement;
                if (dbElem == null)
                {
                    ex = new Exception("获取连接信息失败！");
                    return false;
                }
                string ipStr = dbElem.GetAttribute("MetaDBConn");
                IP = dbElem.GetAttribute("服务器");
                Id = dbElem.GetAttribute("用户");
                Password = dbElem.GetAttribute("密码");
            }
            else
            {
                ex = new Exception("目录节点不是文件库连接节点!");
                return false;
            }
            DevComponents.AdvTree.Node Projectnode = SelNode;
            while (Projectnode.DataKey.ToString() != EnumTreeNodeType.PROJECT.ToString())
            {
                if (null == Projectnode.Parent)
                {
                    ex = new Exception("获取项目ID失败!");
                    return false;
                }
                Projectnode = Projectnode.Parent;
            }
            ProjectId = long.Parse(Projectnode.Tag.ToString());
            ///////////////在工程树上添加产品节点///////////////////
            FTP_Class ftp = new FTP_Class(IP, Id, Password);
            string err = "";
            ftp.MakeDir(Path + "/" + productName, out err);////ftp上创建目录

            DevComponents.AdvTree.Node pnode = new DevComponents.AdvTree.Node();
            if ("Succeed" != err)
            {
                ex = new Exception(err);
                ftp.Close();
                return false;
            }
            else
            {

                pnode.Text = productName;
                pnode.DataKey = EnumTreeNodeType.PRODUCT.ToString();
                pnode.Name = Path + "/" + productName;
                pnode.ImageIndex = 3;
                pnode.Tag = productID;// "1";/////////////////////////////////////////////////////////产品的ID@@@@@@@@@@@@@@（）

            }
            foreach (string ProductSub in Floders)
            {
                err = "";
                ftp.MakeDir(Path + "/" + productName + "/" + ProductSub, out err);////ftp上创建目录

                if ("Succeed" != err)
                {
                    ex = new Exception(err);
                    ftp.Close();
                    return false;
                }
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Text = ProductSub;
                node.DataKey = EnumTreeNodeType.PRODUCTPYPE.ToString();
                node.Name = Path + "/" + pnode.Text + "/" + ProductSub;
                node.ImageIndex = 3;
                //给tag属性赋值

                if (ProductSub == EnumDataType.标准图幅.ToString())
                {
                    node.Tag = 0;
                }
                else if (ProductSub == EnumDataType.非标准图幅.ToString())
                {
                    node.Tag = 1;
                }
                else if (ProductSub == EnumDataType.控制点数据.ToString())
                {
                    node.Tag = 2;
                }
                pnode.Nodes.Add(node);

            }
            SelNode.Nodes.Add(pnode);
            ftp.Close();
            return true;

        }
        #endregion
        #region 删除一个产品（包括文件库中目录和数据库中元信息）

        /// <summary>
        /// 删除一个产品

        /// </summary>
        /// <param name="ProductNode">产品树节点</param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool DelProduct(DevComponents.AdvTree.Node ProductNode, out Exception ex)
        {
            ex = null;
            if (null == ProductNode.DataKey)
            {
                ex = new Exception("选中节点没有指定节点类型！");
                return false;
            }
            if (EnumTreeNodeType.PRODUCT.ToString() != ProductNode.DataKey.ToString())
            {
                ex = new Exception("选中节点不是产品节点！");
                return false;
            }

            if (ProductNode.Tag == null)
            {
                ex = new Exception("获取ID失败！");
                return false;
            }
            if (ProductNode.Tag.ToString() == "")
            {
                ex = new Exception("获取ID失败！");
                return false; ;
            }
            long ProductID = int.Parse(ProductNode.Tag.ToString());//产品ID
            DevComponents.AdvTree.Node mDBNode = ProductNode;
            while (mDBNode.Parent != null)
            {
                mDBNode = mDBNode.Parent;
            }
            if (mDBNode.Name == "文件连接")
            {
                System.Xml.XmlElement dbElem = mDBNode.Tag as System.Xml.XmlElement;
                if (dbElem == null)
                {
                    ex = new Exception("获取连接信息失败！");
                    return false;
                }
                string ipStr = dbElem.GetAttribute("MetaDBConn");
                string ip = dbElem.GetAttribute("服务器");
                string id = dbElem.GetAttribute("用户");
                string password = dbElem.GetAttribute("密码");
               // string ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ipStr + ";Persist Security Info=True";//元数据连接字符串
                
                /////////删除文件库中产品目录以及目录下的数据文件/////////////
                if (!ModDBOperator.DelDirtory(ip, id, password, ProductNode.Name, out ex))
                {
                    return false;
                }
                /////////删除数据库中的记录///////////////////////////////////
                SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();    //属性库连接类

               // pSysDB.SetDbConnection(ConnStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out ex);
                pSysDB.SetDbConnection(ipStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out ex);
                if (ex != null)
                {
                    ex = new Exception("连接元数据库失败！连接地址为：" + ipStr);
                    pSysDB.CloseDbConnection();
                    return false;
                }

                pSysDB.StartTransaction();

                string selStr = "";
                ////在成果索引表中删除/////////////////
                selStr = "DELETE FROM ProductIndexTable WHERE 产品ID=" + ProductID;
                pSysDB.UpdateTable(selStr, out ex);
                if (null != ex)
                {
                    pSysDB.EndTransaction(false);
                    pSysDB.CloseDbConnection();
                    return false;
                }
                ////在标准图幅表中删除/////////////////
                selStr = "DELETE FROM StandardMapMDTable WHERE 产品ID=" + ProductID;
                pSysDB.UpdateTable(selStr, out ex);
                if (null != ex)
                {
                    pSysDB.EndTransaction(false);
                    pSysDB.CloseDbConnection();
                    return false;
                }
                ////在非标准图幅表中删除/////////////////
                
                #region 删除非标准图幅的范围
                selStr = "SELECT ID FROM NonstandardMapMDTable WHERE 产品ID=" + ProductID;
                DataTable GetIdTable = pSysDB.GetSQLTable(selStr,out ex);
                if (null != ex)
                {
                    pSysDB.EndTransaction(false);
                    pSysDB.CloseDbConnection();
                    return false;
                }
                if (null != GetIdTable)
                {
                    for (int i = 0; i < GetIdTable.Rows.Count; i++)
                    {
                        long DataID = -1;
                        try
                        {
                            DataID = Convert.ToInt64(GetIdTable.Rows[i][0].ToString());
                        }
                        catch
                        {
                            DataID = -1;
                        }
                        if (DataID != -1)
                        {
                            DelDataItem(DataID, EnumDataType.非标准图幅.GetHashCode(), pSysDB, out ex);
                            if (null != ex)
                            {
                                pSysDB.EndTransaction(false);
                                pSysDB.CloseDbConnection();
                                return false;
                            }
                        }
                    }
                }


                #endregion
                
                selStr = "DELETE FROM NonstandardMapMDTable WHERE 产品ID=" + ProductID;
                pSysDB.UpdateTable(selStr, out ex);
                if (null != ex)
                {
                    pSysDB.EndTransaction(false);
                    pSysDB.CloseDbConnection();
                    return false;
                }
                ////在控制点数据的元信息表中删除/////////////////
                selStr = "DELETE FROM ControlPointMDTable WHERE 产品ID=" + ProductID;
                pSysDB.UpdateTable(selStr, out ex);
                if (null != ex)
                {
                    pSysDB.EndTransaction(false);
                    pSysDB.CloseDbConnection();
                    return false;
                }
                ////在产品元信息表中删除/////////////////
                selStr = "DELETE FROM ProductMDTable WHERE ID=" + ProductID;
                pSysDB.UpdateTable(selStr, out ex);
                if (null != ex)
                {
                    pSysDB.EndTransaction(false);
                    pSysDB.CloseDbConnection();
                    return false;
                }

                pSysDB.EndTransaction(true);

                #region 图层处理
                if (ModData.v_AppFileDB.MapControl.LayerCount > 0 )
                {
                    IGroupLayer Glayer = null;
                    for (int i = 0; i < ModData.v_AppFileDB.MapControl.LayerCount; i++)
                    {
                        ILayer getlayer = ModData.v_AppFileDB.MapControl.get_Layer(i);
                        if (getlayer.Name == "项目范围图")
                        {
                            Glayer = getlayer as IGroupLayer;
                        }
                    }
                    if (null != Glayer)
                    {
                        ICompositeLayer comlayer = Glayer as ICompositeLayer;
                        if (comlayer != null)
                        {
                            for (int i = 0; i < comlayer.Count; i++)
                            {
                                ILayer orglayer = comlayer.get_Layer(i);
                                string lname = orglayer.Name;
                                if (lname.Contains("MapFrame_") || lname.Contains("Range_"))
                                {
                                    Glayer.Delete(orglayer);
                                    ModData.v_AppFileDB.TOCControl.Update();
                                    ModData.v_AppFileDB.MapControl.ActiveView.Refresh();
                                    i = i - 1;
                                }
                            }
                        }
                    }
                    IGraphicsContainer pGra = ModData.v_AppFileDB.MapControl.Map as IGraphicsContainer;
                    pGra.DeleteAllElements();
                    ModData.v_AppFileDB.MapControl.ActiveView.Refresh();
                }
                #endregion         
                #region 元信息处理
                ModData.v_AppFileDB.MetaDataGrid.DataSource = null;
                ModData.v_AppFileDB.DataInfoGrid.DataSource = null;
                #endregion

                //刷新时间列表框

                ModDBOperator.LoadComboxTime(ipStr, out ex);
                if (ex != null)
                {
                   ex=new Exception("加载时间列表框失败，" + ex.Message);
                }
            }


            return true;
        }
        #endregion
        #region 删除一个数据项（从元数据表中删除）若是非标准同时删除范围信息
        /// <summary>
        /// 删除一个数据项（从元数据表中删除）
        /// </summary>
        /// <param name="DataId">数据ID</param>
        /// <param name="DataTypeID">数据类型ID(标准，非标准，属性)</param>
        /// <param name="metaDBConn">连接字符串</param>
        /// <returns></returns>
        public static bool DelDataItem(long DataId, int DataTypeID, SysCommon.DataBase.SysTable pSysDB, out Exception ex)
        {
            ex = null;           
            ////在成果索引表中删除/////////////////
            string selStr = string.Empty;
            #region 若是非标准图幅，删除其范围信息
            ///获取范围图库所在位置
            if (DataTypeID == EnumDataType.非标准图幅.GetHashCode())
            {
                long ProjectID = -1;
                string MapRange = null;
                long MapScal = -1;
                long ProductID = -1;
                string MapRangePath = null;
                #region  获取必要信息
                selStr = "SELECT 产品ID,块图号,块图比例尺 FROM NonstandardMapMDTable WHERE ID=" + DataId;
                DataTable table = pSysDB.GetSQLTable(selStr, out ex);
                if (ex == null)
                {
                    if (null != table)
                    {
                        if (table.Rows.Count > 0)
                        {
                            try
                            {
                                ProductID = Convert.ToInt64(table.Rows[0][0].ToString());
                                MapRange = table.Rows[0][1].ToString();
                                MapScal = Convert.ToInt64(table.Rows[0][2].ToString());
                            }
                            catch
                            { }
                        }
                    }
                    if (ProductID != -1)
                    {
                        selStr = "SELECT 项目ID FROM  ProductMDTable WHERE  ID="+ProductID;
                        table = pSysDB.GetSQLTable(selStr, out ex);
                    }
                    if (null == ex && table != null)
                    {
                        if (table.Rows.Count > 0)
                        {
                            try
                            {
                                ProjectID = Convert.ToInt64(table.Rows[0][0].ToString());
                            }
                            catch
                            {
                            }
                        }
                    }
                    if (ProjectID != -1)
                    {
                        selStr = "SELECT  图幅结合表 FROM  ProjectMDTable WHERE  ID=" + ProjectID;
                        table = pSysDB.GetSQLTable(selStr, out ex);
                    }
                    if (null == ex && table != null)
                    {
                        if (table.Rows.Count > 0)
                        {
                            MapRangePath = table.Rows[0][0].ToString();
                        }
                    }
                }
                #endregion
                #region 执行范围信息的删除
                if (!string.IsNullOrEmpty(MapRangePath) && !string.IsNullOrEmpty(MapRange))
                {
                    SysCommon.Gis.SysGisDataSet pSysGISDT = new SysCommon.Gis.SysGisDataSet();
                    pSysGISDT.SetWorkspace(MapRangePath, SysCommon.enumWSType.PDB, out ex);
                    if (ex != null)
                    {
                        ex = new Exception("连接范围数据库出错");
                    }
                    else
                    {
                        ModDBOperator.DeleteOneNonStandardRange(DataId, pSysDB, pSysGISDT, out ex);
                    }
                }
                #endregion
            }
            #endregion
            selStr = "DELETE FROM ProductIndexTable WHERE 数据ID=" + DataId + " AND  数据类型编号=" + DataTypeID;
            pSysDB.UpdateTable(selStr, out ex);
            if (null != ex)
            {
                return false;
            }
            ////在标准图幅表或非标准图幅表或控制点表中删除///////
            string Tablename = "";
            switch (DataTypeID)
            {
                case 0://标准图幅
                    Tablename = "StandardMapMDTable";
                    break;
                case 1://非标准图幅

                    Tablename = "NonstandardMapMDTable";
                    break;
                case 2://属性信息

                    Tablename = "ControlPointMDTable";
                    break;
                default:
                    break;
            }
            ////若是非标准图幅，删除其范围信息
           
            if ("" == Tablename)
            {
                return false;
            }
            selStr = "DELETE FROM" + "  " + Tablename + "  " + "WHERE  ID=" + DataId;
            pSysDB.UpdateTable(selStr, out ex);
            if (null != ex)
            {
                return false;
            }
            //刷新时间列表框

           // ModDBOperator.LoadComboxTime(pSysDB.DbConn.ConnectionString, out ex);
            ModDBOperator.LoadComboxTime(pSysDB, out ex);
            if (ex != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "加载时间列表框失败，" + ex.Message);
                return false;
            }
            ModData.v_AppFileDB.MetaDataGrid.DataSource = null;
            return true;
        }
        #endregion
        #region 删除一个数据项(从文件库中删除)
        /// <summary>
        /// 删除一个数据项(从文件库中删除)
        /// </summary>
        /// <param name="DataItemNode">数据项节点</param>
        /// <param name="IP">连接IP</param>
        /// <param name="ID">连接ID</param>
        /// <param name="Password">连接密码</param>
        /// <returns></returns>
        public static bool DelDataItem(string FilePath, string FileName, string IP, string ID, string Password, out Exception ex)
        {
            ex = null;
            FTP_Class ftp = new FTP_Class(IP, ID, Password);
            string error = "";
            ftp.DeleteFileName(FilePath + "/" + FileName, out error);
            //try////试图删除文件库中文件的mdb元数据库
            //{
            //    string eer = "";
            //    string mdbFileName = FileName.Substring(0, FileName.LastIndexOf(".")) + ".mdb";
            //    ftp.DeleteFileName(FilePath + "/" + mdbFileName, out eer);
            //}
            //catch { }
            ///////////////////删除文件夹下同图符号的文件/////////////////////////
            try
            {
                string eer = "";
                string[] subFileName = ftp.GetFileList(FilePath, out eer);
                string rangeno = FileName.Substring(0, FileName.LastIndexOf("."));
                if ("Succeed" == eer)
                {
                    if (null != subFileName)
                    {
                        foreach (string File in subFileName)
                        {
                            if (File.Contains(rangeno))
                            {
                                ftp.DeleteFileName(FilePath + "/" + File, out eer);
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            if ("Succeed" != error)
            {
                ex = new Exception(error);
                ftp.Close();
                return false;
            }
            else
            {
                ftp.Close();
                return true;
            }
        }
        #endregion
        #region 删除ftp服务器上制定文件目录下的所有文件和文件夹

        /// <summary>
        /// 删除ftp服务器上制定目录下的所有文件和文件夹

        /// </summary>
        /// <param name="IP">连接IP</param>
        /// <param name="ID">连接ID</param>
        /// <param name="Password">连接密码</param>
        /// <param name="Path">文件目录</param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool DelDirtory(string IP, string ID, string Password, string Path, out Exception ex)
        {
            ex = null;
            string error = "";
            string Operatepath = Path;
            string FirstPath = Path;
            FTP_Class ftp = new FTP_Class(IP, ID, Password);
            string[] subFiles;
            string[] subFloders;

        loop:
            /////////////////获取操作目录下的文件和文件夹/////////////////////
            subFiles = ftp.GetFileList(Operatepath, out error);
            if ("Succeed" != error)
            {
                ex = new Exception("获取文件列表失败！");
                ftp.Close();
                return false;
            }
            subFloders = ftp.GetFloder(Operatepath, out error);
            if ("Succeed" != error)
            {
                ex = new Exception("获取文件目录列表失败！");
                ftp.Close();
                return false;
            }
            /////////////////////如果顶级目录下没有文件和文件夹就删除此目录函数返回//
            if (Path == Operatepath && null == subFiles && null == subFloders)
            {
                ftp.delDir(Operatepath, out error);
                if ("Succeed" != error)
                {
                    ex = new Exception("删除文件夹：" + Operatepath + "失败！");
                    ftp.Close();
                    return false;
                }
                else
                {
                    ftp.Close();
                    return true;
                }
            }
            /////////////////首先删除文件目录下的文件删除///////////////////////
            if (null != subFiles)
            {
                foreach (string file in subFiles)
                {
                    ftp.DeleteFileName(Operatepath + "/" + file, out error);
                    if ("Succeed" != error)
                    {
                        ex = new Exception("删除文件：" + Operatepath + "/" + file + "失败！");
                        ftp.Close();
                        return false;
                    }
                }
            }
            ///////////////获取操作目录下的文件夹取出第一个作为操作目录//////////////
            ///////////////没有子文件夹则删除此文件夹返回顶级目录//////////////
            if (null == subFloders)
            {
                ftp.delDir(Operatepath, out error);
                if ("Succeed" != error)
                {
                    ex = new Exception("删除文件夹：" + Operatepath + "失败！");
                    ftp.Close();
                    return false;
                }
                Operatepath = Path;

            }
            else
            {
                Operatepath = Operatepath + "/" + subFloders[0];
            }
            /////////////////////////////////////////////////////
            goto loop;
        }
        #endregion
        #region 在数据库中删除一个工程的所有信息

        /// <summary>
        /// 在数据库中删除一个工程的所有信息

        /// </summary>
        /// <param name="ProjectID">工程ID</param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool DelProject(long ProjectID, string metaDBConn, out Exception ex)
        {
            ex = null;
            SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();    //属性库连接类

            string ConnStr = metaDBConn;
            pSysDB.SetDbConnection(ConnStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out ex);
            if (ex != null)
            {
                ex = new Exception("连接元数据库失败！连接地址为：" + metaDBConn);
                pSysDB.CloseDbConnection();
                return false;
            }
            string selStr = "SELECT ID FROM ProductMDTable WHERE 项目ID=" + ProjectID;
            DataTable dt = pSysDB.GetSQLTable(selStr, out ex);
            if (null != ex) return false;
            #region 删除产品元信息表中的标准图幅数据、非标准图幅数据，控制点数据
            pSysDB.StartTransaction();
            if (null != dt)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    long ProductID = long.Parse(dt.Rows[i]["ID"].ToString()); ////产品ID 
                    selStr = "DELETE FROM StandardMapMDTable WHERE 产品ID=" + ProductID;/////标准图幅数据
                    pSysDB.UpdateTable(selStr, out ex);
                    if (null != ex)
                    {
                        pSysDB.EndTransaction(false);
                        pSysDB.CloseDbConnection();
                        return false;
                    }
                    selStr = "DELETE FROM NonstandardMapMDTable WHERE 产品ID=" + ProductID;/////非标准图幅数据

                    pSysDB.UpdateTable(selStr, out ex);
                    if (null != ex)
                    {
                        pSysDB.EndTransaction(false);
                        pSysDB.CloseDbConnection();
                        return false;
                    }
                    selStr = "DELETE FROM ControlPointMDTable WHERE 产品ID=" + ProductID;/////控制点数据的元信息

                    pSysDB.UpdateTable(selStr, out ex);
                    if (null != ex)
                    {
                        pSysDB.EndTransaction(false);
                        pSysDB.CloseDbConnection();
                        return false;
                    }
                }

            }
            #endregion
            ////删除成果索引表中的数据/////////////////
            selStr = "DELETE FROM ProductIndexTable WHERE 项目ID=" + ProjectID;/////成果索引表的元信息

            pSysDB.UpdateTable(selStr, out ex);
            if (null != ex)
            {
                pSysDB.EndTransaction(false);
                pSysDB.CloseDbConnection();
                return false;
            }
            ////删除产品表中的数据/////////////////
            selStr = "DELETE FROM ProductMDTable WHERE 项目ID=" + ProjectID;/////产品元信息表的元信息
            pSysDB.UpdateTable(selStr, out ex);
            if (null != ex)
            {
                pSysDB.EndTransaction(false);
                pSysDB.CloseDbConnection();
                return false;
            }
            ////删除产品表中的数据/////////////////
            selStr = "DELETE FROM ProjectMDTable WHERE ID=" + ProjectID;/////项目元信息表的元信息
            pSysDB.UpdateTable(selStr, out ex);
            if (null != ex)
            {
                pSysDB.EndTransaction(false);
                pSysDB.CloseDbConnection();
                return false;
            }
            ////删除地图控件中的图层//
            ModData.v_AppFileDB.MapControl.ClearLayers();
            ModData.v_AppFileDB.MapControl.ActiveView.Refresh();
            ModData.v_AppFileDB.TOCControl.Update();

            //if (ModData.v_AppFileDB.MapControl.LayerCount>0)
            //{
            //    int count = ModData.v_AppFileDB.MapControl.LayerCount;
            //    for (int i = 0; i < count; i++)
            //    {
            //        ILayer layer = ModData.v_AppFileDB.MapControl.get_Layer(i);
            //        if (layer.Name == "项目范围图")
            //        {
            //            ModData.v_AppFileDB.MapControl.DeleteLayer(i);
            //            ModData.v_AppFileDB.TOCControl.Update();
            //            break;
            //        }
            //    }
            //}
           
            pSysDB.EndTransaction(true);
            return true;

        }
        #endregion
        #region 判断表中是否有已存在的元组，存在返回 true，不存在false
        /// <summary>
        /// 判断表中是否有已存在满足条件的元组，存在返回 true，不存在false
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="ConStr">连接字符串</param>
        /// <param name="TableFild">字段名</param>
        /// <param name="value">值(重载string ,int,long )</param>
        ///  <param name="condition">where语句附加查询条件</param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool IsTableFildExist(string TableName, SysCommon.DataBase.SysTable pSysTable, string TableFild, string value, string condition, out Exception ex)
        {

            ////////
            string SQl = "SELECT * FROM" + "  " + TableName + "  " + "WHERE" + "  " + TableFild + "='" + value + "'";
            if (null != condition)
                SQl = SQl + " " + "AND" + " " + condition;
            DataTable table = pSysTable.GetSQLTable(SQl, out ex);
            if (null != ex)
            {
                //pSysTable.CloseDbConnection();
                return false;
            }

            if (table.Rows.Count > 0)
            {
                //pSysTable.CloseDbConnection();
                return true;
            }
            else return false;
            ////////
        }
        public static bool IsTableFildExist(string TableName, SysCommon.DataBase.SysTable pSysTable, string TableFild, int value, string condition, out Exception ex)
        {

            ////////
            string SQl = "SELECT * FROM" + "  " + TableName + "  " + "WHERE" + "  " + TableFild + "=" + value;
            if (null != condition)
                SQl = SQl + " " + "AND" + " " + condition;
            DataTable table = pSysTable.GetSQLTable(SQl, out ex);
            if (null != ex)
            {
                //pSysTable.CloseDbConnection();
                return false;
            }

            if (table.Rows.Count > 0)
            {
                //pSysTable.CloseDbConnection();
                return true;
            }
            else return false;
            ////////
        }
        public static bool IsTableFildExist(string TableName, SysCommon.DataBase.SysTable pSysTable, string TableFild, long value, string condition, out Exception ex)
        {

            string SQl = "SELECT * FROM" + "  " + TableName + "  " + "WHERE" + "  " + TableFild + "=" + value;
            if (null != condition)
                SQl = SQl + "  " + "AND" + "  " + condition;
            DataTable table = pSysTable.GetSQLTable(SQl, out ex);
            if (null != ex)
            {
                // pSysTable.CloseDbConnection();
                return false;
            }

            if (table.Rows.Count > 0)
            {
                //pSysTable.CloseDbConnection();
                return true;
            }
            else return false;
            ////////
        }
        #endregion
        #region 根据树节点的name属性和text属性来删除选中节点下的子节点

        /// <summary>
        /// 删除选中节点下的节点（依据node.name和node.text）

        /// </summary>
        /// <param name="SelNode"></param>
        /// <param name="NodeName"></param>
        /// <param name="NodeText"></param>
        /// <param name="ex"></param>
        public static void DelNodeByNameAndText(DevComponents.AdvTree.Node SelNode, string NodeName, string NodeText, out Exception ex)
        {
            ex = null;
            DevComponents.AdvTree.Node node = SelNode;
            try
            {
                if (node.Nodes.Count > 0)
                {
                    for (int i = 0; i < node.Nodes.Count; i++)
                    {
                        if (node.Nodes[i].Name == NodeName && node.Nodes[i].Text == NodeText)
                        {
                            node.Nodes[i].Remove();
                            i = i - 1;
                            continue;
                        }
                        if (node.Nodes[i].Nodes.Count > 0)
                        {
                            DelNodeByNameAndText(node.Nodes[i], NodeName, NodeText, out ex);
                        }

                    }
                }
            }
            catch (Exception Error)
            {
                ex = new Exception("删除节点NAME=" + NodeName + "," + "TEXT=" + NodeText + ",失败原因：" + Error.Message);
                return;
            }
        }
        #endregion
        #region ftp上传文件夹

        /// <summary>
        /// ftp上传文件夹

        /// </summary>
        /// <param name="UPloadPath">需要上传的文件夹路径</param>
        /// <param name="SavePath">ftp上保存路径,为null则为根目录</param>
        /// <param name="IP"></param>
        /// <param name="ID"></param>
        /// <param name="PassWord"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool UpLoadDirectory(string UPloadPath, string SavePath, string IP, string ID, string PassWord, out Exception ex)
        {
            ex = null;
            string error = "";
            string[] UPFiles = Directory.GetFiles(UPloadPath);
            if (null != UPFiles)
            {
                FileInfo filepathinfo = new FileInfo(UPFiles[0]);
                string Floder = filepathinfo.Directory.Name;
                //////////在ftp服务器上建立文件夹////////////////
                FTP_Class ftp = new FTP_Class(IP, ID, PassWord);
                if (null != SavePath)
                    ftp.MakeDir(SavePath + "/" + Floder, out error);
                else
                    ftp.MakeDir(Floder, out error);
                if ("Succeed" != error)
                {
                    ex = new Exception("FTP服务器上创建文件夹:" + Floder + "失败！");
                    ftp.Close();
                    return false;
                }
                /////////向创建的文件夹中上传数据//////////////////
                foreach (string fileinfon in UPFiles)
                {
                    FileInfo info = new FileInfo(fileinfon);
                    string file = info.Name;
                    if (null == SavePath)
                        ftp.Upload(fileinfon, Floder, file, out error);
                    else
                        ftp.Upload(fileinfon, SavePath + "/" + Floder, file, out error);
                    if ("Succeed" != error)
                    {
                        ex = new Exception("文件：" + file + ",上传至:ftp://" + SavePath + "/" + UPloadPath + "失败");
                        ftp.Close();
                        return false;
                    }
                }
                ftp.Close();
                return true;
            }
            else
            {
                ex = new Exception("文件夹中没有需要上传的文件!");
                return false;
            }
        }
        #endregion
        #region ftp下载整个文件
        /// <summary>
        /// 从ftp下载整个文件夹

        /// </summary>
        /// <param name="FtpPath">ftp上的文件夹路径</param>
        /// <param name="SavePath">本地保存路径</param>
        /// <param name="IP"></param>
        /// <param name="ID"></param>
        /// <param name="PassWord"></param>
        /// <param name="ex"></param>
        /// <returns></returns>

        public static bool DownloadDirectory(string FtpPath, string SavePath, string IP, string ID, string PassWord, out Exception ex)
        {
            ex = null;
            string Floder;
            string newSavePath;
            try
            {
                int j = FtpPath.LastIndexOf('/') + 1;
                int end = FtpPath.Length - j;
                int start = j;
                Floder = FtpPath.Substring(start, end);
                newSavePath = SavePath + "\\" + Floder;
            }
            catch
            {
                ex = new Exception("获取文件夹名称失败!");
                return false;
            }
            ////////////本地建立目录////////////////////
            if (Directory.Exists(newSavePath))
            {
                ex = new Exception("本地目录已存在！");
                return false;
            }
            else
            {
                Directory.CreateDirectory(newSavePath);
            }
            /////////////下载文件至本地目录/////////////
            string[] Files;
            string error = "";
            FTP_Class ftp = new FTP_Class(IP, ID, PassWord);
            Files = ftp.GetFileList(FtpPath, out error);
            if ("Succeed" != error)
            {
                ex = new Exception("获取文件夹：" + FtpPath + "，下的文件列表失败！");
                ftp.Close();
                return false;
            }
            if (null != Files)
            {
                foreach (string file in Files)
                {
                    ModDBOperator.DownloadFile(IP, ID, PassWord, FtpPath, file, file, newSavePath, out error);
                    if ("Succeed" != error)
                    {
                        ex = new Exception("文件：" + file + ",下载失败！");
                        ftp.Close();
                        return false;
                    }
                }
            }
            ftp.Close();
            return true;
        }
        #endregion
        #region 判断ftp上文件是否存在同名文件

        /// <summary>
        /// 判断ftp上文件是否存在同名文件

        /// </summary>
        /// <param name="IP"></param>
        /// <param name="ID"></param>
        /// <param name="PassWord"></param>
        /// <param name="FilePath"></param>
        /// <param name="FileName"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool IsFTPExistFile(string IP, string ID, string PassWord, string FilePath, string FileName, out Exception ex)
        {
            ex = null;
            FTP_Class ftp = new FTP_Class(IP, ID, PassWord);
            string error = "";
            string[] Files = ftp.GetFileList(FilePath, out error);
            if (error != "Succeed")
            {
                ex = new Exception("获取ftp服务器文件列表失败！");
                ftp.Close();
                return true;
            }
            if (null == Files)
            {
                ftp.Close();
                return false;
            }
            else
            {
                foreach (string file in Files)
                {
                    if (file == FileName)
                    {
                        ftp.Close();
                        return true;
                    }
                }
            }
            ftp.Close();
            return false;
        }
        #endregion
        #region 通过dwg文件获取空间参考
        /// <summary>
        /// 通过dwg文件获取空间参考
        /// </summary>
        /// <param name="in_DWGFileName"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ESRI.ArcGIS.Geometry.ISpatialReference GetSpatialReferencfromDWG(string in_DWGFileName, out Exception ex)
        {
            ex = null;
            ESRI.ArcGIS.Geometry.ISpatialReference out_SpatialReference = null;
            FileInfo info = new FileInfo(in_DWGFileName);
            string filename = info.Name;
            string Path = info.DirectoryName;
            //CAD文件的空间数据集的遍历
            IWorkspaceFactory pCADWorkSpaceFactory = new CadWorkspaceFactoryClass();
            IFeatureDataset pCADFeatureDataset = null;
            try
            {
                IFeatureWorkspace pCADFeatureWorkSpace = (IFeatureWorkspace)pCADWorkSpaceFactory.OpenFromFile(Path, 0);
                pCADFeatureDataset = pCADFeatureWorkSpace.OpenFeatureDataset(filename);
            }
            catch
            {
                ex = new Exception("读取DWG文件失败!");
                return null;
            }
            IFeatureClassContainer pFeatureClassContainer = pCADFeatureDataset as IFeatureClassContainer;
            IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
            IFeatureClass pFeatureClass = pEnumFeatureClass.Next();
            while (pFeatureClass != null)
            {
                try
                {
                    out_SpatialReference = pFeatureClass.GetFeature(1).Shape.SpatialReference;

                }
                catch
                {
                    out_SpatialReference = null;
                    pFeatureClass = pEnumFeatureClass.Next();
                    continue;
                }
                pFeatureClass = pEnumFeatureClass.Next();
            }
            return out_SpatialReference;
        }
        #endregion
        #region 通过dwg文件获取Feature保存到in_mdbFileName当中
        /// <summary>
        /// 通过dwg文件获取Feature保存到in_mdbFileName当中
        /// </summary>
        /// <param name="in_DWGFileName">输入的dwg文件路径</param>
        /// <param name="in_mdbFileName">输入的mdb文件路径</param>
        /// <param name="in_FeatureClassName"></param>
        /// <param name="ex"></param>
        public static void CreatFeatureByDWG(string in_DWGFileName, string in_mdbFileName, string in_FeatureClassName, IList<string> Field, IList<string> Value, out Exception ex)
        {
            ex = null;
            FileInfo info = new FileInfo(in_DWGFileName);
            string filename = info.Name;
            string Path = info.DirectoryName;
            IWorkspace outWorkspace = null;
            IWorkspaceFactory pWorkSpaceFactory = new ShapefileWorkspaceFactoryClass();
            outWorkspace = pWorkSpaceFactory.OpenFromFile(Path, 0);
            IDataset shpDataset = (IDataset)outWorkspace;
            IWorkspaceName pShpWorkspaceName = (IWorkspaceName)shpDataset.FullName;
            //CAD文件的空间数据集的遍历
            IWorkspaceFactory pCADWorkSpaceFactory = new CadWorkspaceFactoryClass();
            IFeatureWorkspace pCADFeatureWorkSpace = (IFeatureWorkspace)pCADWorkSpaceFactory.OpenFromFile(Path, 0);
            IFeatureDataset pCADFeatureDataset = pCADFeatureWorkSpace.OpenFeatureDataset(filename);
            IFeatureClassContainer pFeatureClassContainer = pCADFeatureDataset as IFeatureClassContainer;
            IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
            IFeatureClass pFeatureClass = pEnumFeatureClass.Next();
            IWorkspace ws = null;
            IWorkspaceFactory wsf = new AccessWorkspaceFactoryClass();
            IFeatureClass fc;
            string geodbname = in_mdbFileName;
            ws = wsf.OpenFromFile(geodbname, 0);
            IFeatureWorkspace fws = (IFeatureWorkspace)ws;
            fc = fws.OpenFeatureClass(in_FeatureClassName);
            ///////////////////////////////////////////////
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)ws;
            workspaceEdit.StartEditing(true);

            while (pFeatureClass != null)
            {
                IDataset dataset = (IDataset)fc;
                if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    try
                    {
                        IFeature newfeature = fc.CreateFeature();
                        if (Field.Count == Value.Count)
                        {
                            for (int i = 0; i < Field.Count; i++)
                            {
                                int index = newfeature.Fields.FindField(Field[i]);
                                newfeature.set_Value(index, Value[i]);
                            }
                        }
                        newfeature.Shape = pFeatureClass.GetFeature(1).Shape;///shape中可以读出空间参考
                        newfeature.Store();
                    }
                    catch { }
                }
                pFeatureClass = pEnumFeatureClass.Next();
            }
            workspaceEdit.StopEditing(true);

        }
        #endregion
        #region  在制定的pdb中新建FeatureClasss
        /// <summary>
        /// 在制定的pdb中新建FeatureClasss
        /// </summary>
        /// <param name="in_FileName"></param>
        /// <param name="in_FilePath"></param>
        /// <param name="featureclassname"></param>
        /// <param name="in_SpatialReference"></param>
        /// <param name="FieldsName"></param>
        /// <param name="FieldType"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static IFeatureClass CreatFeatureClass(string in_FileName, string in_FilePath, string featureclassname, ESRI.ArcGIS.Geometry.ISpatialReference in_SpatialReference, List<string> FieldsName, List<esriFieldType> FieldType, out Exception ex)
        {
            ex = null;
            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();
            //IWorkspaceName workspaceName = workspaceFactory.Create(in_FilePath, in_FileName, null, 0);
            //ESRI.ArcGIS.esriSystem.IName name = (ESRI.ArcGIS.esriSystem.IName)workspaceName;
            //IWorkspace pGDB_workspace = (IWorkspace)name.Open();
            IFeatureWorkspace pFWS = workspaceFactory.OpenFromFile(in_FilePath + "\\" + in_FileName, 0) as IFeatureWorkspace;



            IFields pFields = new FieldsClass();
            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
            IGeometryDef pGeomDef = new GeometryDef();
            IGeometryDefEdit pGeomDefEdit = pGeomDef as IGeometryDefEdit;
            ///////////////////////////////////////////////////////
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = "Shape";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            pGeomDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon;
            pFieldsEdit.AddField(pField);
            //////////////////////空间参考处理///////////////////////
            ISpatialReference pSpatialReference = null;
            if (null == in_SpatialReference)
            {
                ISpatialReferenceFactory pSpaReferenceFac = new SpatialReferenceEnvironmentClass();
                pSpatialReference = new UnknownCoordinateSystemClass();
                //设置默认的Resolution
                ISpatialReferenceResolution pSpatiaprefRes = pSpatialReference as ISpatialReferenceResolution;
                pSpatiaprefRes.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon 
                pSpatiaprefRes.SetDefaultXYResolution();
                pSpatiaprefRes.SetDefaultZResolution();
                pSpatiaprefRes.SetDefaultMResolution();
                //设置默认的Tolerence
                ISpatialReferenceTolerance pSpatialrefTole = pSpatiaprefRes as ISpatialReferenceTolerance;
                pSpatialrefTole.SetDefaultXYTolerance();
                pSpatialrefTole.SetDefaultZTolerance();
                pSpatialrefTole.SetDefaultMTolerance();
            }
            else
            {
                pSpatialReference = in_SpatialReference;
            }
            pFieldEdit.GeometryDef_2 = pGeomDef;
            pGeomDefEdit.SpatialReference_2 = pSpatialReference;
            /////////////建立字段////////////////////////////
            //pField = new Field();
            //pFieldEdit = pField as IFieldEdit;
            //pFieldEdit.Length_2 = 30;
            //pFieldEdit.Name_2 = "OBJECTID";
            //pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            //pFieldsEdit.AddField(pField);
            ////////////////////////////////////
            //pField = new Field();
            //pFieldEdit = pField as IFieldEdit;
            //pFieldEdit.Length_2 = 30;
            //pFieldEdit.Name_2 = "MAP_NEWNO";
            //pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            //pFieldsEdit.AddField(pField);
            //////////////////////////////////
            //pField = new Field();
            //pFieldEdit = pField as IFieldEdit;
            //pFieldEdit.Length_2 = 30;
            //pFieldEdit.Name_2 = "MAP_NAME";
            //pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            //pFieldsEdit.AddField(pField);
            //////////////////////////////////   
            if (FieldsName.Count == FieldType.Count)
            {
                for (int i = 0; i < FieldsName.Count; i++)
                {
                    pField = new Field();
                    pFieldEdit = pField as IFieldEdit;
                    pFieldEdit.Length_2 = 30;
                    pFieldEdit.Name_2 = FieldsName[i];
                    pFieldEdit.Type_2 = FieldType[i];
                    pFieldsEdit.AddField(pField);
                }
            }
            try
            {
                IFeatureClass pFeatClass = pFWS.CreateFeatureClass(featureclassname, pFields, null, null, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, "Shape", "");
                return pFeatClass;
            }
            catch (Exception Eerror)
            {
                ex = Eerror;
                return null;
            }
        }
        #endregion
        #region  通过dwg文件入库图幅结合表
        /// <summary>
        /// 通过dwg文件入库图幅结合表
        /// </summary>
        /// <param name="in_dwgFile"></param>
        /// <param name="in_mdbFile"></param>
        /// <param name="pMetaTable"></param>
        /// <param name="ex"></param>
        public static void CreateMapRangeByDwg(string in_dwgFile, string in_mdbFile, SysCommon.DataBase.SysTable pMetaTable, int datatype, out Exception ex)
        {
            ex = null;
            int mapScal = -1;
            string MapRangeNo = "";
            string MapRangeName = "";
            string SQL = "";
            string FeatureClassName = "";
            #region 根据图幅类型获取比例尺，图符号，图幅名信息
            if (datatype == EnumDataType.标准图幅.GetHashCode())
            {
                SQL = "SELECT MAP_SCALE,MAP_NEWNO,MAP_NAME  FROM metadata";
                FeatureClassName = "MapFrame_";
            }
            else if (datatype == EnumDataType.非标准图幅.GetHashCode())
            {
                SQL = "SELECT 块图比例尺,块图号,名称  FROM metadata";
                FeatureClassName = "Range_";
            }
            DataTable table = pMetaTable.GetSQLTable(SQL, out ex);
            if (null != ex)
            {
                return;
            }
            if (table != null)
            {
                if (table.Rows.Count > 0)
                {
                    try
                    {
                        mapScal = Convert.ToInt32(table.Rows[0][0].ToString());
                        MapRangeNo = table.Rows[0][1].ToString();
                        MapRangeName = table.Rows[0][2].ToString();
                    }
                    catch (Exception exx)
                    {
                        ex = exx;
                        return;
                    }
                }
            }
            #endregion
            FeatureClassName += mapScal;
            #region 在in_mdbFile(MapDb.mdb)中试图读出FeatureClassName，没有就建立这个FeatureClassName
            SysCommon.Gis.SysGisDataSet pSysGISDT = new SysCommon.Gis.SysGisDataSet();
            pSysGISDT.SetWorkspace(in_mdbFile, SysCommon.enumWSType.PDB, out ex);
            if (ex != null)
            {
                ex = new Exception("连接范围数据库出错");
                return;
            }
            IFeatureClass Fclass = pSysGISDT.GetFeatureClass(FeatureClassName, out ex);

            if (null != ex) return;
            if (null == Fclass)////不存在此feartureclasss则建立
            {
                FileInfo Finfo = new FileInfo(in_mdbFile);
                string mdbName = Finfo.Name;
                string mdbPath = Finfo.DirectoryName;
                ISpatialReference SpatialReference = GetSpatialReferencfromDWG(in_dwgFile, out ex);
                List<string> LFields = new List<string>();
                List<esriFieldType> FieldTypes = new List<esriFieldType>();
                LFields.Add("OBJECTID");
                FieldTypes.Add(esriFieldType.esriFieldTypeOID);
                LFields.Add("MAP_NEWNO");
                FieldTypes.Add(esriFieldType.esriFieldTypeString);
                LFields.Add("MAP_NAME");
                FieldTypes.Add(esriFieldType.esriFieldTypeString);
                IFeatureClass pFeatClass = CreatFeatureClass(mdbName, mdbPath, FeatureClassName, SpatialReference, LFields, FieldTypes, out ex);
                if (null != ex)
                    return;
            }
            else////存在此feartureclasss则试图删除已存在的范围号信息
            {
                ITable ClassTable = Fclass as ITable;
                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.WhereClause = "MAP_NEWNO = '" + MapRangeNo + "'";
                ClassTable.DeleteSearchedRows(queryFilter);
            }
            pSysGISDT.Dispose();
            #endregion
            #region 向in_mdbFile的FeatureClassName写入信息
            List<string> Fields = new List<string>();
            List<string> Values = new List<string>();
            Fields.Add("MAP_NEWNO");
            Fields.Add("MAP_NAME");
            Values.Add(MapRangeNo);
            Values.Add(MapRangeName);
            CreatFeatureByDWG(in_dwgFile, in_mdbFile, FeatureClassName, Fields, Values, out ex);
            #endregion

        }
        #endregion
        #region 通过mdb入库图幅结合表
        public static void CreateMapRangeByMDB(string in_Rangemdb, string in_MapRangeDB, string FeatureClassName, out Exception ex)
        {
            ex = null;
            //////从文件的in_Rangemdb（2008008_Range.mdb）获取featureClass
            SysCommon.Gis.SysGisDataSet pSysGISDT = new SysCommon.Gis.SysGisDataSet();
            pSysGISDT.SetWorkspace(in_Rangemdb, SysCommon.enumWSType.PDB, out ex);
            if (ex != null)
            {
                ex = new Exception("连接范围数据库出错");
                return;
            }

            SysCommon.Gis.SysGisDataSet MapRangeDT = new SysCommon.Gis.SysGisDataSet();
            MapRangeDT.SetWorkspace(in_MapRangeDB, SysCommon.enumWSType.PDB, out ex);
            if (ex != null)
            {
                ex = new Exception("连接范围数据库出错");
                return;
            }
            IFeatureClass NewFeatureClass = MapRangeDT.GetFeatureClass(FeatureClassName, out ex);
            IFeatureClass OrgFeatureClass = pSysGISDT.GetFeatureClass(FeatureClassName, out ex);
            if (null == OrgFeatureClass)
            {
                ex = new Exception("入库的图幅结合表中没有记录！");
                return;
            }
            if (OrgFeatureClass.FeatureCount(null) == 0)
            {
                ex = new Exception("入库的图幅结合表中没有记录！");
                return;
            }
            if (null == NewFeatureClass)///MApDb中不存在FeatureClass
            {
                #region 建立FeatureClass
                List<string> LFields = new List<string>();
                List<esriFieldType> FieldTypes = new List<esriFieldType>();
                LFields.Add("OBJECTID");
                FieldTypes.Add(esriFieldType.esriFieldTypeOID);
                LFields.Add("MAP_NEWNO");
                FieldTypes.Add(esriFieldType.esriFieldTypeString);
                LFields.Add("MAP_NAME");
                FieldTypes.Add(esriFieldType.esriFieldTypeString);

                int fieldIndexValue = OrgFeatureClass.FindField("OBJECTID");
                IFeatureCursor FeatureCursor = OrgFeatureClass.Search(null, false);
                IFeature fea = FeatureCursor.NextFeature();

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(FeatureCursor);

                ISpatialReference SpatialReference = fea.Shape.SpatialReference;

                FileInfo info = new FileInfo(in_MapRangeDB);
                string FileName = info.Name;
                string FilePath = info.DirectoryName;
                CreatFeatureClass(FileName, FilePath, FeatureClassName, SpatialReference, LFields, FieldTypes, out ex);
                if (null != ex)
                    return;
                #endregion
            }
            else///MApDb中已存在FeatureClass
            {
                #region 删除重复的记录
                if (null != OrgFeatureClass && null == ex)
                {
                    string GetRangeNO = null;
                    if (OrgFeatureClass.FeatureCount(null) > 0)
                    {
                        IFeatureCursor FeatureCursor = OrgFeatureClass.Search(null, false);
                        IFeature fea = FeatureCursor.NextFeature();
                        while (fea != null)
                        {
                            int index = fea.Fields.FindField("MAP_NEWNO");
                            if (-1 != index)
                            {
                                GetRangeNO = fea.get_Value(index).ToString();
                            }
                            if (!String.IsNullOrEmpty(GetRangeNO))
                            {
                                MapRangeDT.DeleteRows(FeatureClassName, "MAP_NEWNO = '" + GetRangeNO + "'", out ex);
                                if (null != ex)
                                {
                                    ex = new Exception("范围记录:范围号" + GetRangeNO + "删除失败!");
                                }
                            }
                            fea = FeatureCursor.NextFeature();
                        }

                        //释放CURSOR
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(FeatureCursor);
                    }

                }
                #endregion
            }
            try
            {
                IWorkspace Workspace = null;
                IWorkspaceFactory WorkspaceFactory = new AccessWorkspaceFactoryClass();
                IFeatureClass FeatureClass;
                string geodbname = in_MapRangeDB;
                Workspace = WorkspaceFactory.OpenFromFile(geodbname, 0);
                IFeatureWorkspace fws = (IFeatureWorkspace)Workspace;
                FeatureClass = fws.OpenFeatureClass(FeatureClassName);
                IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)Workspace;
                workspaceEdit.StartEditing(true);
                IFeature newfeature = FeatureClass.CreateFeature();
                IFeatureCursor FeatureCursor = OrgFeatureClass.Search(null, false);
                IFeature fea = FeatureCursor.NextFeature();

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(FeatureCursor);

                newfeature.Shape = fea.Shape;
                int index = 1;
                index = newfeature.Fields.FindField("MAP_NEWNO");
                newfeature.set_Value(index, fea.get_Value(fea.Fields.FindField("MAP_NEWNO")));
                index = newfeature.Fields.FindField("MAP_NAME");
                newfeature.set_Value(index, fea.get_Value(fea.Fields.FindField("MAP_NAME")));
                newfeature.Store();
                workspaceEdit.StopEditing(true);
            }
            catch
            {
                ex = new Exception("图幅结合表入库失败！");
            }






        }
        #endregion
        /// <summary>
        /// 获取项目树上的任意节点
        /// </summary>
        /// <param name="SelNode"></param>
        /// <param name="NodeName"></param>
        /// <param name="NodeText"></param>
        /// <param name="NodeType"></param>
        /// <param name="NodeID"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static DevComponents.AdvTree.Node GetTreeNode(DevComponents.AdvTree.Node SelNode, string NodeName, string NodeText, string NodeType, long NodeID, out Exception ex)
        {
            ex = null;
            if (SelNode == null)
                return null;
            DevComponents.AdvTree.Node node = SelNode;
            try
            {
                if (node.Nodes.Count > 0)
                {
                    for (int i = 0; i < node.Nodes.Count; i++)
                    {
                        if (node.Nodes[i].Name == NodeName && node.Nodes[i].Text == NodeText)
                        {
                            try
                            {
                                string GetNodetype = node.Nodes[i].DataKey.ToString().Trim();
                                long GetID = long.Parse(node.Nodes[i].Tag.ToString());
                                if (GetNodetype == NodeType && GetID == NodeID)
                                    return node.Nodes[i];
                            }
                            catch
                            {
                                continue;
                            }                                                    
                        }
                        if (node.Nodes[i].Nodes.Count > 0)
                        {
                            DevComponents.AdvTree.Node newnode = GetTreeNode(node.Nodes[i], NodeName, NodeText, NodeType, NodeID, out ex);
                            if (null == ex && null != newnode)
                                return newnode;
                        }

                    }
                }
            }
            catch 
            {
                ex = new Exception("获取树节点失败!");
                return null;
            }
            return null;
        }
    }
}