using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Collections;

using SysCommon.Gis;

using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ADOX;

namespace GeoDBATool
{
    public class InitialDBTree
    {
        
     
        #region 设置选中树节点的颜色
        private void setNodeColor(DevComponents.AdvTree.AdvTree aTree)
        {
            DevComponents.DotNetBar.ElementStyle elementStyle = new DevComponents.DotNetBar.ElementStyle();
            elementStyle.BackColor = Color.FromArgb(255, 244, 213);
            elementStyle.BackColor2 = Color.FromArgb(255, 216, 105);
            elementStyle.BackColorGradientAngle = 90;
            elementStyle.Border = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderBottomWidth = 1;
            elementStyle.BorderColor = Color.DarkGray;
            elementStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderLeftWidth = 1;
            elementStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderRightWidth = 1;
            elementStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderTopWidth = 1;
            elementStyle.BorderWidth = 1;
            elementStyle.CornerDiameter = 4;
            elementStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            aTree.NodeStyleSelected = elementStyle;
            aTree.DragDropEnabled = false;
        }
        #endregion

        #region 初始化数据处理树图


        //根据数据工程XML初始化数据树图

        public void OnCreateDataTree(DevComponents.AdvTree.AdvTree aTree, XmlDocument xmlDoc)
        {
            if (aTree == null || xmlDoc == null) return;

            //初始化节点列表树图

            setNodeColor(aTree);
            InitialadvTreeNode(aTree);
            aTree.Tag = false;
            XmlElement root = xmlDoc.SelectSingleNode("//数据移植") as XmlElement;

            //获取源数据连接

            XmlNode pOrgDataNode = xmlDoc.SelectSingleNode("//源数据连接");
            if (pOrgDataNode == null) return;

            XmlDocument rulexmlDoc = new XmlDocument();
            XmlElement ruleElement = xmlDoc.SelectSingleNode("//规则") as XmlElement;
            if (ruleElement == null) return;
            try
            {
                rulexmlDoc.Load(@ruleElement.GetAttribute("路径"));
            }
            catch (Exception e)
            {
                //*******************************************************************
                //Excption Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "选择的对照关系文件无法加载,请检查!");
                return;
            }

            Exception exError = null;
            bool bRes = true;

            //环境变量设置
            Dictionary<string, object> dicValues = new Dictionary<string, object>();
            //dicValues.Add("START!DATE", DateTime.Now);

            foreach (XmlNode aDataNode in pOrgDataNode.ChildNodes)
            {
                XmlElement aDataElement = aDataNode as XmlElement;
                SysCommon.enumWSType pType = SysCommon.enumWSType.PDB;
                switch (aDataElement.GetAttribute("类型").Trim().ToUpper())
                {
                    case "PDB":
                        pType = SysCommon.enumWSType.PDB;
                        break;
                    case "GDB":
                        pType = SysCommon.enumWSType.GDB;
                        break;
                    case "SDE":
                        pType = SysCommon.enumWSType.SDE;
                        break;
                    case "SHP":
                        pType = SysCommon.enumWSType.SHP;
                        break;
                }
                SysCommon.Gis.SysGisDataSet pOrgSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
                string strDB = "";
                if (pType == SysCommon.enumWSType.SDE)
                {
                    strDB = aDataElement.GetAttribute("服务器");
                    pOrgSysGisDataSet.SetWorkspace(aDataElement.GetAttribute("服务器"), aDataElement.GetAttribute("服务名"), aDataElement.GetAttribute("数据库"), aDataElement.GetAttribute("用户"), aDataElement.GetAttribute("密码"), aDataElement.GetAttribute("版本"), out exError);
                }
                else
                {
                    strDB = aDataElement.GetAttribute("数据库");
                    pOrgSysGisDataSet.SetWorkspace(aDataElement.GetAttribute("数据库"), pType, out exError);
                }

                if (exError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "源数据" + strDB + "连接失败,请确认");
                    return;
                }

                //在树图中添加源数据节点

                DevComponents.AdvTree.Node OrgDBNode = ModDBOperator.CreateAdvTreeNode(aTree.Nodes, strDB, strDB, aTree.ImageList.Images[3], true);
                OrgDBNode.Cells.Add(new DevComponents.AdvTree.Cell());
                OrgDBNode.Cells.Add(new DevComponents.AdvTree.Cell());
                OrgDBNode.Cells.Add(new DevComponents.AdvTree.Cell());
                //获取源数据图层名
                List<string> OrgFeatureClassNames = new List<string>();
                if (pType == SysCommon.enumWSType.SDE)
                {
                    OrgFeatureClassNames = pOrgSysGisDataSet.GetAllFeatureClassNames(false, false);
                }
                else
                {
                    OrgFeatureClassNames = pOrgSysGisDataSet.GetAllFeatureClassNames(false);
                }
                //cyf 20110715 add
                if (OrgFeatureClassNames == null || OrgFeatureClassNames.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取源数据图层失败,请检查！");
                    return;
                }
                //end

                //获取目标数据连接
                XmlElement pObjDataElement = xmlDoc.SelectSingleNode("//目标数据连接") as XmlElement;
                switch (pObjDataElement.GetAttribute("类型").Trim().ToUpper())
                {
                    case "PDB":
                        pType = SysCommon.enumWSType.PDB;
                        break;
                    case "GDB":
                        pType = SysCommon.enumWSType.GDB;
                        break;
                    case "SDE":
                        pType = SysCommon.enumWSType.SDE;
                        break;
                }
                SysCommon.Gis.SysGisDataSet pObjSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
                if (pType == SysCommon.enumWSType.SDE)
                {
                    pObjSysGisDataSet.SetWorkspace(pObjDataElement.GetAttribute("服务器"), pObjDataElement.GetAttribute("服务名"), pObjDataElement.GetAttribute("数据库"), pObjDataElement.GetAttribute("用户"), pObjDataElement.GetAttribute("密码"), pObjDataElement.GetAttribute("版本"), out exError);
                }
                else
                {
                    pObjSysGisDataSet.SetWorkspace(pObjDataElement.GetAttribute("数据库"), pType, out exError);
                }

                if (exError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "目标数据连接失败,请确认");
                    return;
                }

                //完成数据入库规则初始化

                clsDBOperator pClsDBOperator = new clsDBOperator(pOrgSysGisDataSet, pObjSysGisDataSet);
                XmlDocument dataconxmldoc = new XmlDocument();  //按照此XML进行入库
                dataconxmldoc.LoadXml("<ROOT></ROOT>");
                if (!pClsDBOperator.GetConRelXml(OrgFeatureClassNames, dataconxmldoc.DocumentElement, rulexmlDoc, dicValues))
                {
                    bRes = false;
                }
                //初始化节点列表树图

                if (InitialadvTreeNode(OrgDBNode, dataconxmldoc.DocumentElement) == false)
                {
                    bRes = false;
                }
                //将转换内容XML绑定到树图上以便获取进行入库
                OrgDBNode.Tag = dataconxmldoc;
                OrgDBNode.DataKey = pClsDBOperator;
            }

            aTree.Refresh();
            if (!bRes)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据入库初始化失败,请确认");
                return;
            }
            aTree.Tag = true;
        }
        //初始化节点列表树图结构

        private void InitialadvTreeNode(DevComponents.AdvTree.AdvTree aTree)
        {
            aTree.Nodes.Clear();
            aTree.Columns.Clear();
            DevComponents.AdvTree.ColumnHeader aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "NodeOrg";
            aColumnHeader.Text = "源数据";
            aColumnHeader.Width.Relative = 35;
            aTree.Columns.Add(aColumnHeader);

            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "NodeObj";
            aColumnHeader.Text = "目标数据";
            aColumnHeader.Width.Relative = 35;
            aTree.Columns.Add(aColumnHeader);

            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "NodeState";
            aColumnHeader.Text = "状态";
            aColumnHeader.Width.Relative = 15;
            aTree.Columns.Add(aColumnHeader);

            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "NodeRes";
            aColumnHeader.Text = "结果";
            aColumnHeader.Width.Relative = 15;
            aTree.Columns.Add(aColumnHeader);
        }
        //在树图上表现数据节点
        private bool InitialadvTreeNode(DevComponents.AdvTree.Node treeNode, XmlElement xmlElement)
        {
            try
            {
                XmlNode aNode = xmlElement as XmlNode;

                foreach (XmlNode aChild in aNode.ChildNodes)
                {
                    XmlElement chidElement = (XmlElement)aChild;
                    DevComponents.AdvTree.Node newNode = ModDBOperator.CreateAdvTreeNode(treeNode.Nodes, chidElement.GetAttribute("Name"), chidElement.GetAttribute("Name"), treeNode.TreeControl.ImageList.Images[6], true);

                    if (aChild.HasChildNodes == true)
                    {
                        foreach (XmlNode aaChild in aChild.ChildNodes)
                        {
                            XmlElement newchidElement = (XmlElement)aaChild;
                            DevComponents.AdvTree.Node newNodeChild = ModDBOperator.CreateAdvTreeNode(newNode.Nodes, newchidElement.GetAttribute("OrgFCName"), treeNode.Name + "|" + newchidElement.GetAttribute("OrgFCName") + "|" + newchidElement.GetAttribute("ObjFCName"), treeNode.TreeControl.ImageList.Images[8], false);
                            ModDBOperator.CreateAdvTreeCell(newNodeChild, newchidElement.GetAttribute("ObjFCName"), treeNode.TreeControl.ImageList.Images[8]);
                            newNodeChild.Cells.Add(new DevComponents.AdvTree.Cell());
                            newNodeChild.Cells.Add(new DevComponents.AdvTree.Cell());
                            XmlElement errElement = aaChild.SelectSingleNode(".//Err") as XmlElement;
                            if (errElement != null)
                            {
                                foreach (XmlAttribute aAttribute in errElement.Attributes)
                                {
                                    DevComponents.AdvTree.Node ErrNodeChild = ModDBOperator.CreateAdvTreeNode(newNodeChild.Nodes, aAttribute.Value, "", treeNode.TreeControl.ImageList.Images[11], true);
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                //*******************************************************************
                //Excption Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                return false;
            }
        }

        #endregion

    }

    public class clsDataThread
    {
        private DevComponents.AdvTree.AdvTree m_AdvTreeData;          //数据处理树图
        private Plugin.Application.IAppFormRef m_AppForm;             //主窗体APP
        private Plugin.Application.IAppGISRef m_AppGIS;               //主功能应用APP
        private IGeometry m_Geometry;                                 //空间范围条件
        private string m_SpatialRel;                                  //范围条件空间关系
        private esriSpatialRelEnum m_SpatialRelEnum;                  //范围条件空间关系
        private bool m_Exit;                                          //处理过程出错时是否跳出执行


        private DataTable m_DataTable;                               //FID记录表内容

        //private string m_OutPath;                                    //输出FID记录表内容存放路径

        private EnumOperateType m_OperaType = EnumOperateType.NULL;
        private SysCommon.DataBase.SysDataBase m_SysDB;//FID记录表连接


        private SysCommon.Gis.SysGisDataSet m_HistoGisDT;//历史库连接

        private string m_dateStr;//当前时间字符串


        //当前处理数据的线程

        private Thread _CurrentThread;
        public Thread CurrentThread
        {
            get
            {
                return _CurrentThread;
            }
            set
            {
                _CurrentThread = value;
            }
        }

        private bool m_Res;
        public bool Res
        {
            get
            {
                return m_Res;
            }
        }
        public string UserName { get; set; }//历史库用户名  xisheng 2011.07.15
        public string UserNameNow { get; set; }//现势库用户名  xisheng 2011.07.15
       

        public clsDataThread(Plugin.Application.IAppGISRef pApp, IGeometry pGeometry, esriSpatialRelEnum pSpatialRelEnum, bool bExit, DataTable pDataTable, SysCommon.DataBase.SysDataBase pSysDB, EnumOperateType pOperaType, SysCommon.Gis.SysGisDataSet pHistoSysGisDT, string pDateStr)
        {
            m_AdvTreeData = pApp.DataTree;
            m_AppGIS = pApp;
            m_AppForm = pApp as Plugin.Application.IAppFormRef;
            m_Geometry = pGeometry;
            m_SpatialRelEnum = pSpatialRelEnum;
            m_Exit = bExit;
            m_DataTable = pDataTable;
            m_SysDB = pSysDB;
            //m_OutPath = strOutPath;
            m_OperaType = pOperaType;
            m_HistoGisDT = pHistoSysGisDT;
            m_dateStr = pDateStr;
        }
        public clsDataThread(Plugin.Application.IAppGISRef pApp, IGeometry pGeometry, string strSpatialRel, bool bExit, DataTable pDataTable, SysCommon.DataBase.SysDataBase pSysDB, EnumOperateType pOperaType)
        {
            m_AdvTreeData = pApp.DataTree;
            m_AppGIS = pApp;
            m_AppForm = pApp as Plugin.Application.IAppFormRef;
            m_Geometry = pGeometry;
            m_SpatialRel = strSpatialRel;
            m_Exit = bExit;
            m_DataTable = pDataTable;
            m_SysDB = pSysDB;
            //m_OutPath = strOutPath;
            m_OperaType = pOperaType;
        }

        //数据提取(入库、更新),适用于批量操作,对于发生错误的地方只需记录日志无需终止过程
        public void DoBatchWorks()
        {
            if (m_AdvTreeData.Nodes.Count == 0) return;

            m_AppForm.MainForm.Invoke(new ShowProgress(ShowProgressBar), true);

            bool bInDB = true;
            DevComponents.AdvTree.AdvTree aTree = m_AdvTreeData;

            foreach (DevComponents.AdvTree.Node aTempTreeNode in aTree.Nodes)
            {
                DevComponents.AdvTree.Node aTreeNode = aTempTreeNode;
                if (aTreeNode == null) continue;

                m_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { aTreeNode, "运行", "", false, true });
                m_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), aTreeNode.Name + "开始入库");

                //获取工程XML
                XmlDocument dataconxmldoc = aTreeNode.Tag as XmlDocument;
                //获取源、目标数据连接信息

                clsDBOperator pDBOperator = aTreeNode.DataKey as clsDBOperator;
                if (dataconxmldoc == null || pDBOperator == null)
                {
                    m_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { aTreeNode, "失败", "", false, false });
                    continue;
                }
                Exception exError = null;

                //开启编辑

                pDBOperator.ObjSysGisDataSet.StartWorkspaceEdit(false, out exError);
                if (exError != null)
                {
                    bInDB = false;
                    m_AppForm.MainForm.Invoke(new AddNode(AddTreeNode), "开启编辑失败", "原因：" + exError.Message);
                    m_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { aTreeNode, "失败", "", false, false });
                    continue;
                }

                //获取存放FID记录表内容连接信息

                if (m_DataTable != null && m_SysDB != null)
                {
                    //CreateInitBaseNField(m_OutPath, m_DataTable.TableName);
                    //pSysDataBase = new SysCommon.DataBase.SysTable();
                    //pSysDataBase.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_OutPath + ";Mode=Share Deny None;Persist Security Info=False", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out exError);
                    pDBOperator.OutDataBase = m_SysDB;
                    pDBOperator.OutDataBase.StartTransaction();
                    pDBOperator.InfoTable = m_DataTable;
                }

                #region 提交图幅数据时，首先将历史库中的数据置为历史，然后删除目标库中的数据//和FID记录表里面对应的记录
                if (m_OperaType == EnumOperateType.Submit)
                {
                    if (m_HistoGisDT != null)
                    {
                        if (m_HistoGisDT.WorkSpace != null)
                        {

                            m_HistoGisDT.StartWorkspaceEdit(false, out exError);
                            if (exError != null)
                            {
                                bInDB = false;
                                m_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "提示", "开启编辑失败！" });
                                m_HistoGisDT.EndWorkspaceEdit(false, out exError);
                                pDBOperator.ObjSysGisDataSet.EndWorkspaceEdit(false, out exError);
                            }
                            //将相应范围内的图幅数据职位历史
                            if (!UpdateHistoData(m_HistoGisDT, m_SpatialRelEnum, m_Geometry, m_dateStr))
                            {
                                bInDB = false;
                                m_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "提示", "更新历史库失败！" });
                                m_HistoGisDT.EndWorkspaceEdit(false, out exError);
                                pDBOperator.ObjSysGisDataSet.EndWorkspaceEdit(false, out exError);
                            }
                        }
                    }
                    //if (m_SysDB != null)
                    //{
                    //pDBOperator.OutDataBase = m_SysDB;
                    //pDBOperator.OutDataBase.StartTransaction();
                    //删除原始数据和FID记录表数据
                    if (!DelSubmitData(m_SysDB, pDBOperator.ObjSysGisDataSet, m_Geometry))
                    {
                        bInDB = false;
                        //结束编辑,回滚事务
                        //pDBOperator.OutDataBase.EndTransaction(false);
                        //pDBOperator.OutDataBase.CloseDbConnection();
                        m_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "提示", "删除业务维护信息表或者数据失败" });
                        pDBOperator.ObjSysGisDataSet.EndWorkspaceEdit(false, out exError);
                        return;
                    }
                    //}
                }
                #endregion

                #region 根据解析的XML内容进行数据入库
                // 新建结果集节点

                XmlElement ResultsElement = dataconxmldoc.CreateElement("Results");
                dataconxmldoc.DocumentElement.AppendChild(ResultsElement);

                //  进行转换操作,注意取得全部FC
                bool bInFc = true;
                int NodeCount = 0;
                string XPath = ".//FC[@OrgFCName != '' and @ObjFCName != '']";
                XmlNodeList FCNodeList = dataconxmldoc.SelectNodes(XPath);
                foreach (XmlNode aFCNode in FCNodeList)
                {
                    NodeCount = NodeCount + 1;
                    XmlElement aFCElement = (XmlElement)aFCNode;
                    XmlElement AttrsElement = aFCElement["Attrs"];

                    string ObjFCName = aFCElement.GetAttribute("ObjFCName").Trim();
                    string OrgFCName = aFCElement.GetAttribute("OrgFCName").Trim();
                    string OrgCondition = aFCElement.GetAttribute("OrgCondition");
                    string featureType = aFCElement.GetAttribute("FeatureType");

                    // 新建结果节点 用于用效的记录数据操作 
                    XmlElement ResElement = dataconxmldoc.CreateElement("Res");
                    ResultsElement.AppendChild(ResElement);
                    ResElement.SetAttribute("FeatureType", featureType);

                    m_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { m_AdvTreeData.FindNodeByName(aTreeNode.Name + "|" + OrgFCName + "|" + ObjFCName), "运行", "", false, false });
                    m_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), OrgFCName + "入到" + ObjFCName + "...(" + NodeCount + "/" + FCNodeList.Count + ")");

                    if (!pDBOperator.ImpFeatures(OrgFCName, ObjFCName, OrgCondition, AttrsElement, ResElement, m_AppForm, m_Geometry, m_SpatialRel, m_SpatialRelEnum, m_Exit, m_OperaType, m_HistoGisDT, m_dateStr))
                    {
                        string strErr = "";
                        if (ResElement.HasAttribute("ErrorE"))
                        {
                            strErr = ResElement.GetAttribute("ErrorE");
                            m_AppForm.MainForm.Invoke(new AddNode(AddTreeNode), strErr);
                        }
                        else if (ResElement.HasChildNodes)
                        {
                            foreach (XmlNode aErrNode in ResElement.ChildNodes)
                            {
                                XmlElement aErrElement = aErrNode as XmlElement;
                                strErr = "源数据OID:" + aErrElement.GetAttribute("ErrorOrgOID") + "\n" + aErrElement.GetAttribute("ErrorDescribe");
                                m_AppForm.MainForm.Invoke(new AddNode(AddTreeNode), strErr);
                            }
                        }

                        bInFc = false;
                        m_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { m_AdvTreeData.FindNodeByName(aTreeNode.Name + "|" + OrgFCName + "|" + ObjFCName), "失败", "", false, false });
                        if (m_Exit == true)
                        {
                            break;
                        }
                    }
                    else
                    {
                        m_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { m_AdvTreeData.FindNodeByName(aTreeNode.Name + "|" + OrgFCName + "|" + ObjFCName), "完成", ResElement.GetAttribute("count") + "个要素完成入库", false, false });
                    }
                }
                if (m_OperaType == EnumOperateType.Submit)
                {
                    //图幅更新提交的数据写入版本信息
                    Exception eError = null;
                    IWorkspace pWS = pDBOperator.ObjSysGisDataSet.WorkSpace;  //目标工作空间
                    DateTime SaveTime = DateTime.Now;                          //更新时间
                    try
                    {
                        SaveTime = Convert.ToDateTime(m_dateStr);
                    }
                    catch
                    { }
                    WriteDBVersion(pWS,ModData.DBVersion, SaveTime, out eError);
                    if (eError != null)
                    {
                        bInFc = false;
                        m_AppForm.MainForm.Invoke(new AddNode(AddTreeNode),"更新版本信息写入失败。\n原因：" + eError.Message);
                    }
                }
                #endregion

                //结束编辑,回滚事务
                if (pDBOperator.OutDataBase != null)
                {
                    pDBOperator.OutDataBase.EndTransaction(bInFc);
                    pDBOperator.OutDataBase.CloseDbConnection();
                }

                //结束编辑
                pDBOperator.ObjSysGisDataSet.EndWorkspaceEdit(bInFc, out exError);
                if (m_HistoGisDT != null)
                {
                    if (m_HistoGisDT.WorkSpace != null)
                    {
                        m_HistoGisDT.EndWorkspaceEdit(bInFc, out exError);
                    }
                }
                if (exError != null)
                {
                    bInFc = false;
                    m_AppForm.MainForm.Invoke(new AddNode(AddTreeNode), "结束编辑失败");
                }

                //关闭连接
                pDBOperator.OrgSysGisDataSet.Dispose();

                if (bInFc == false)
                {
                    bInDB = false;
                    m_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { aTreeNode, "失败", "", false, false });
                }

                m_AppForm.MainForm.Invoke(new ChangeProgressValue(ChangeProgressBarValue), NodeCount);
                m_AppForm.MainForm.Invoke(new RefreshFrm(RefreshMainFrm));
            }

            m_AppForm.MainForm.Invoke(new ChangeSelectNode(ChangeTreeSelectNode), new object[] { null, "", "", false, false });
            m_AppForm.MainForm.Invoke(new ShowProgress(ShowProgressBar), false);
            m_AppForm.MainForm.Invoke(new ShowTips(ShowStatusTips), "");

            if (bInDB == true)
            {
                m_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "提示", "操作成功!" });
            }
            else
            {
                m_AppForm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "提示", "操作完成,存在错误!" });
            }

            m_Res = bInDB;
            m_AppGIS.CurrentThread = null;
            _CurrentThread.Abort();
        }

        #region 进程与界面控件响应实现


        //弹出提示对话框

        private delegate void ShowForm(string strCaption, string strText);
        private void ShowErrForm(string strCaption, string strText)
        {
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle(strCaption, strText);
        }

        //刷新界面
        private delegate void RefreshFrm();
        private void RefreshMainFrm()
        {
            m_AppForm.MainForm.Refresh();
        }

        //改变进度条进度

        private delegate void ChangeProgressValue(int intValue);
        private void ChangeProgressBarValue(int intValue)
        {
            m_AppForm.ProgressBar.Value = intValue;
        }

        //控制进度条显示

        private delegate void ShowProgress(bool bVisible);
        private void ShowProgressBar(bool bVisible)
        {
            if (bVisible == true)
            {
                m_AppForm.ProgressBar.Visible = true;
            }
            else
            {
                m_AppForm.ProgressBar.Visible = false;
            }
        }

        //改变状态栏提示内容
        private delegate void ShowTips(string strText);
        private void ShowStatusTips(string strText)
        {
            m_AppForm.OperatorTips = strText;
        }

        //为数据处理树图节点添加处理结果状态

        private delegate void ChangeSelectNode(DevComponents.AdvTree.Node aNode, string strMemo, string strRes, bool bClear, bool bExpand);
        private void ChangeTreeSelectNode(DevComponents.AdvTree.Node aNode, string strMemo, string strRes, bool bClear, bool bExpand)
        {
            if (aNode == null)
            {
                m_AdvTreeData.SelectedNode = null;
                m_AdvTreeData.Refresh();
                return;
            }

            m_AdvTreeData.SelectedNode = aNode;
            if (bExpand)
            {
                m_AdvTreeData.SelectedNode.Expand();
            }
            else
            {
                m_AdvTreeData.SelectedNode.Collapse();
            }

            if (bClear)
            {
                m_AdvTreeData.SelectedNode.Nodes.Clear();
            }
            m_AdvTreeData.SelectedNode.Cells[2].Text = strMemo;
            m_AdvTreeData.SelectedNode.Cells[3].Text = strRes;
            m_AdvTreeData.Refresh();
        }

        //为数据处理树图添加错误节点

        private delegate void AddNode(string strText);
        private void AddTreeNode(string strText)
        {
            ModDBOperator.CreateAdvTreeNode(m_AdvTreeData.SelectedNode.Nodes, strText, "", m_AdvTreeData.ImageList.Images[11], true);
        }

        //修改进度条

        private delegate void ChangeProgress(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value);
        private void ChangeProgressBar(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value)
        {
            if (min != -1)
            {
                pProgressBar.Minimum = min;
            }
            if (max != -1)
            {
                pProgressBar.Maximum = max;
            }
            pProgressBar.Value = value;
            pProgressBar.Refresh();
        }

        #endregion

        /// <summary>
        ///  写入数据库版本表
        /// </summary>
        /// <param name="in_iVersion">要写入的版本号</param>
        /// <param name="in_DateTime">版本建立时间</param>
        /// <param name="ex"></param>
        private void WriteDBVersion(IWorkspace pWS, int in_iVersion, DateTime in_DateTime, out Exception ex)
        {
            ex = null;
            if (null == pWS) { ex = new Exception("更新环境库连接信息未初始化。"); return; }
            if (null == in_DateTime) { ex = new Exception("输入时间不能为空"); return; }
            string sql = "INSERT INTO " + ModData.m_sDBVersionTable + "(VERSION,USERNAME,VERSIONTIME,DES) values(";
            //cyf 20110621 modify
            if (pWS.WorkspaceFactory.WorkspaceType != esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                sql += in_iVersion.ToString() + "," + "null,'" + in_DateTime + "',null)";
            }
            else
            {
                sql += in_iVersion.ToString() + "," + "null," + "to_date('" + in_DateTime.ToString("G") + "','yyyy-mm-dd hh24:mi:ss')" + ",null)";
            }
            //end
            try
            {
                pWS.ExecuteSQL(sql);
            }
            catch (Exception eError)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
                ex = new Exception("写入数据库版本表失败。\n原因：" + eError.Message);
                return;
            }

        }
       

        /// <summary>
        /// 图幅批量提交，删除原始的数据和FID记录表  陈亚飞编写

        /// </summary>
        /// <param name="pSysDB">FID记录表库</param>
        /// <param name="pSysDT">目标库连接</param>
        /// <param name="pGeo">范围限定</param>
        /// <returns></returns>
        private bool DelSubmitData(SysCommon.DataBase.SysDataBase pSysDB, SysCommon.Gis.SysGisDataSet pSysDT, IGeometry pGeo)
        {
            Exception eError = null;
            Dictionary<string, StringBuilder> DicFCInfo = new Dictionary<string, StringBuilder>();//记录范围内的图层名和OID
            List<IDataset> LstDt = pSysDT.GetAllFeatureClass();//获得该数据记集下所有的图层
            foreach (IDataset pDataset in LstDt)
            {
                string FcName = pDataset.Name;
                if(FcName.ToUpper().EndsWith("_GOH")) continue;//将历史库排除掉
                string user = "";
                if (UserNameNow != "" && FcName.Contains("."))//从要素集取出用户  xisheng 2011.07.15
                {
                    string[] str = FcName.Split('.');
                    user = str[0];
                }
                if (user != "" && user != UserNameNow)//判断用户是否相同  xisheng 2011.07.15
                {
                    continue;
                }
                IFeatureClass pFeatureClass = pDataset as IFeatureClass;
                ISpatialFilter pFilter = new SpatialFilterClass();
                pFilter.Geometry = pGeo;
                pFilter.GeometryField = "SHAPE";
                pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelEnvelopeIntersects;
                IFeatureCursor pCusor = pFeatureClass.Search(pFilter as IQueryFilter, false);
                if (pCusor == null) return false;
                IFeature pFeature = pCusor.NextFeature();
                while (pFeature != null)
                {

                    if (!DicFCInfo.ContainsKey(FcName))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(pFeature.OID.ToString());
                        DicFCInfo.Add(FcName, sb);
                    }
                    else
                    {
                        if (DicFCInfo[FcName].Length != 0)
                        {
                            DicFCInfo[FcName].Append(',');
                        }
                        DicFCInfo[FcName].Append(pFeature.OID.ToString());
                    }
                    pFeature = pCusor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCusor);
            }
            foreach (KeyValuePair<string, StringBuilder> FCItem in DicFCInfo)
            {
                //删除FID记录表
                //cyf 20110621 delete
                //if (pSysDB != null)
                //{
                //    if (!pSysDB.UpdateTable("delete from FID记录表 where FCNAME='" + FCItem.Key.Trim() + "' and OID in (" + FCItem.Value.ToString().Trim() + ")", out eError))
                //    {
                //        return false;
                //    }
                //}
                //end
                //删除数据changed by xisheng 20110721
                int count =FCItem.Value.Length/10000;//按长度10000分组
                string  s;
                string strend = "";
                for (int i = 0; i <count; i++)
                {
                  s=FCItem.Value.ToString().Substring(i * 10000, 10000);
                  if (strend.Trim() != "") s = strend + s;//如果上组留下了一个数，将添加到这组中。
                  strend = s.Substring(s.LastIndexOf(",")+1);//取逗号后的数，留到下一组，以免隔断
                  s=s.Substring(0,s.LastIndexOf(","));//取逗号前的数
                  //if (s.StartsWith(",")) s = s.Substring(1, 9999);
                  if (!pSysDT.DeleteRows(FCItem.Key.ToString().Trim(), "OBJECTID in (" + s.Trim() + ")", out eError))
                  {
                      return false;
                  }
                }
                s = FCItem.Value.ToString().Substring(count * 10000);
                if (strend.Trim() != "") s = strend + s;
                if (s.StartsWith(",")) s = s.Substring(1);
                if (!pSysDT.DeleteRows(FCItem.Key.ToString().Trim(), "OBJECTID in (" + s.Trim() + ")", out eError))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 图幅批量提交，将历史库相应范围内的数据置为历史

        /// </summary>
        /// <param name="pHistoSysGisDT">历史库连接</param>
        /// <param name="RelEnum">范围过滤相交条件</param>
        /// <param name="pGeometry">范围限制</param>
        /// <param name="pDateTimeStr">失效日期</param>
        /// <returns></returns>
        private bool UpdateHistoData(SysCommon.Gis.SysGisDataSet pHistoSysGisDT, esriSpatialRelEnum RelEnum, IGeometry pGeometry, string pDateTimeStr)
        {
            try
            {
               
                List<IDataset> LstDT = pHistoSysGisDT.GetAllFeatureClass();//.WorkSpace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                foreach (IDataset pDT in LstDT)
                {
                    string pDtStr = pDT.Name;
                    string user="";
                    if (UserName != "" && pDtStr.Contains("."))//从要素集取出用户 xisheng 2011.07.15
                    {
                        string[] str = pDtStr.Split('.');
                        user = str[0];
                    }
                    if (!pDtStr.ToLower().Contains("_GOH")) continue;
                    else
                    {
                        if (user != "" && user != UserName)//判断用户是否相同 xisheng 2011.07.15
                            continue;
                    }

                    //若是历史库的数据，则更新历史库

                    IFeatureClass pHistoFeaCls = pDT as IFeatureClass;
                    ISpatialFilter pFilter = new SpatialFilterClass();
                    pFilter.WhereClause = "ToDate='" + DateTime.MaxValue.ToString("u") + "'";

                    pFilter.GeometryField = "SHAPE";
                    pFilter.Geometry = pGeometry;
                    pFilter.SpatialRel = RelEnum;
                  
                    IFeatureCursor pCursor = pHistoFeaCls.Search(pFilter as IQueryFilter, false);
                    if (pCursor == null) return false;
                    int pIndex = -1;
                    pIndex = pHistoFeaCls.Fields.FindField("ToDate");
                    if (pIndex == -1)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "历史库中不存在字段名'ToDate'，请检查！");
                        return false;
                    }
                    IFeature pFea = pCursor.NextFeature();
                    while (pFea != null)
                    {
                        pFea.set_Value(pIndex, pDateTimeStr as object);
                        pFea.Store();
                        pFea = pCursor.NextFeature();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    //IFeatureCursor pUpdateCursor = pHistoFeaCls.Update(pFilter as IQueryFilter, false);
                    //if (pUpdateCursor == null) return false;
                    //IFeature pUpdateFeature = pUpdateCursor.NextFeature();
                    //while (pUpdateFeature != null)
                    //{
                    //    try
                    //    {

                    //        pUpdateFeature.set_Value(pIndex, pDateTimeStr as object);
                    //        pUpdateCursor.UpdateFeature(pUpdateFeature);
                    //        //pUpdateCursor.Flush();
                    //        pUpdateFeature = pUpdateCursor.NextFeature();
                    //    }
                    //    catch
                    //    {
                    //    }
                    //}
                    //pUpdateCursor.Flush();
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pUpdateCursor);
                }
                return true;
            }
            catch (Exception e)
            {
                //*******************************************************************
                //Excption Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "修改历史库数据失败！");
                return false;
            }
        }
        /// <summary>
        /// 在指定位置创建数据更新记录表文件，创建默认字段

        /// </summary>
        private void CreateInitBaseNField(string strUpdateUnitTablePath, string strTableName)
        {
            //创建mdb文件
            ADOX.Catalog AdoxCatalog = new ADOX.Catalog();
            AdoxCatalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strUpdateUnitTablePath + ";");

            //在创建的mdb中生成数据更新日志记录表
            ADOX.TableClass tbl = new ADOX.TableClass();
            tbl.ParentCatalog = AdoxCatalog;
            tbl.Name = strTableName;

            ADOX.ColumnClass col = new ADOX.ColumnClass();
            col.ParentCatalog = AdoxCatalog;
            col.Type = ADOX.DataTypeEnum.adInteger; // 必须先设置字段类型

            col.Name = "GOFID";
            col.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
            tbl.Columns.Append(col, ADOX.DataTypeEnum.adInteger, 0);

            col = new ADOX.ColumnClass();
            col.ParentCatalog = AdoxCatalog;
            col.Type = ADOX.DataTypeEnum.adLongVarWChar;
            col.Name = "FCNAME";
            col.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
            tbl.Columns.Append(col, ADOX.DataTypeEnum.adInteger, 0);

            col = new ADOX.ColumnClass();
            col.ParentCatalog = AdoxCatalog;
            col.Type = ADOX.DataTypeEnum.adInteger;
            col.Name = "OID";
            col.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
            tbl.Columns.Append(col, ADOX.DataTypeEnum.adInteger, 0);

            tbl.Keys.Append("PrimaryKey", ADOX.KeyTypeEnum.adKeyPrimary, "GOFID", "", "");
            AdoxCatalog.Tables.Append(tbl);

            //销毁对象 
            System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(AdoxCatalog);
        }
    }

    public class clsDBOperator
    {
        //源数据 目标数据连接信息
        private SysCommon.Gis.SysGisDataSet m_OrgSysGisDataSet;
        private SysCommon.Gis.SysGisDataSet m_ObjSysGisDataSet;
        public SysCommon.Gis.SysGisDataSet OrgSysGisDataSet
        {
            get { return m_OrgSysGisDataSet; }
            set { m_OrgSysGisDataSet = value; }
        }
        public SysCommon.Gis.SysGisDataSet ObjSysGisDataSet
        {
            get { return m_ObjSysGisDataSet; }
            set { m_ObjSysGisDataSet = value; }
        }
        //private SysCommon.Gis.SysGisDataSet m_HistoSysGisDtset;
        //public SysCommon.Gis.SysGisDataSet HistoSysGisDtset
        //{ }

        public clsDBOperator(SysCommon.Gis.SysGisDataSet OrgSysGisDataSet, SysCommon.Gis.SysGisDataSet ObjSysGisDataSet)
        {
            m_OrgSysGisDataSet = OrgSysGisDataSet;
            m_ObjSysGisDataSet = ObjSysGisDataSet;
        }

        //输出FID记录表连接

        private SysCommon.DataBase.SysDataBase m_OutDataBase;
        public SysCommon.DataBase.SysDataBase OutDataBase
        {
            get { return m_OutDataBase; }
            set { m_OutDataBase = value; }
        }
        //库中FID记录表内容

        private DataTable m_InfoTable;
        public DataTable InfoTable
        {
            set { m_InfoTable = value; }
        }

        #region 源数据与目标数据的匹配转换关系xml解析

        /// <summary>
        /// 得到由指定关系得到的数据装换关系
        /// </summary>
        /// <param name="FCNames">源数据FC名称集</param>
        /// <param name="orgelement">实际转换关系XML</param>
        /// <param name="rulexmldoc">规则对照关系XML</param>
        /// <param name="dicValues">环境变量</param>
        /// <returns></returns>
        public bool GetConRelXml(List<string> FCNames, XmlElement orgelement, XmlDocument rulexmldoc, Dictionary<string, object> dicValues)
        {
            bool bRes = true;
            orgelement.InnerXml = String.Empty;

            string XPath = ".//类别";
            XmlNodeList styleNodeList = rulexmldoc.SelectNodes(XPath);
            foreach (XmlNode aStyleNode in styleNodeList)
            {
                XmlElement aStyleElement = (XmlElement)aStyleNode;
                string StyleName = aStyleElement["名称"].InnerText;
                XmlElement newStyleElement = orgelement.OwnerDocument.CreateElement("Style");
                newStyleElement.SetAttribute("Name", StyleName);
                orgelement.AppendChild(newStyleElement);

                XPath = ".//规则";
                XmlNodeList aNodeList = aStyleNode.SelectNodes(XPath);
                //得到全部的指定关系规则创建FC节点
                foreach (XmlNode aRuleNode in aNodeList)
                {
                    XmlElement aRuleElement = (XmlElement)aRuleNode;
                    string OrgFeatureClassName = aRuleElement["源数据"]["名称"].InnerText.Trim();
                    string ObjFeatureClassName = aRuleElement["目标数据"]["名称"].InnerText.Trim();
                    string FeatureClassType = aRuleElement["类型"].InnerText.Trim();                                //规则中数据类型


                    if (FCNames.Contains(OrgFeatureClassName))
                    {
                        XmlElement aFCElement = orgelement.OwnerDocument.CreateElement("FC");
                        newStyleElement.AppendChild(aFCElement);

                        #region 设置相关属性


                        aFCElement.SetAttribute("OrgFCName", OrgFeatureClassName);
                        aFCElement.SetAttribute("ObjFCName", ObjFeatureClassName);
                        aFCElement.SetAttribute("FeatureType", FeatureClassType);

                        //添加错误内容属性节点

                        XmlElement errElement = orgelement.OwnerDocument.CreateElement("Err");
                        aFCElement.AppendChild(errElement);
                        string strErr = string.Empty;
                        #region 进行相关检查

                        //检查数据集是否存在
                        Exception errEx = null;
                        string orgFeatureType = string.Empty;                                                         //源数据类型

                        string objFeatureType = string.Empty;                                                         //目标数据类型
                        if (!m_OrgSysGisDataSet.CheckFeatureClassExist(OrgFeatureClassName, out orgFeatureType, out errEx))                            //源数据不存在
                        {
                            strErr = "源数据中数据集" + OrgFeatureClassName + "不存在";
                            errElement.SetAttribute("源数据错误", strErr);
                            continue;
                            //bRes = false;
                        }
                        if (!m_ObjSysGisDataSet.CheckFeatureClassExist(ObjFeatureClassName, out objFeatureType, out errEx))                           //目标数据不存在
                        {
                            strErr = "目标数据中数据集" + ObjFeatureClassName + "不存在";
                            errElement.SetAttribute("目标数据错误", strErr);
                            continue;
                            // bRes = false;
                        }

                        //检查数据类型

                        //if (FeatureClassType.ToUpper() != orgFeatureType.ToUpper() || FeatureClassType.ToUpper() != objFeatureType.ToUpper())
                        //{
                        //    errElement.SetAttribute("数据类型错误", "类型不一致");
                        //    bRes = false;
                        //}
                        //wgf提交修改 20111025
                        if (FeatureClassType.ToUpper() != orgFeatureType.ToUpper() || FeatureClassType.ToUpper() != objFeatureType.ToUpper())
                        {
                            string strTishi = "类型不一致,源文件类型为" + orgFeatureType.ToUpper() + ",目标数据类型为" + FeatureClassType.ToUpper();
                            errElement.SetAttribute("数据类型错误", strTishi);
                            continue;
                            //bRes = false;
                        }
                        #endregion

                        #region 原始数据过滤条件属性获取

                        bool bSuccessed;
                        string condition = GetCondition(aRuleNode["源数据"], out bSuccessed);
                        if (bSuccessed == false)
                        {
                            strErr = "规则中数据集" + OrgFeatureClassName + "过滤条件不正确";
                            errElement.SetAttribute("过滤条件错误", strErr);
                            bRes = false;
                        }
                        #endregion
                        aFCElement.SetAttribute("OrgCondition", condition);

                        //添加源数据与目标数据字段对应关系属性节点

                        XmlElement AttrsElement = orgelement.OwnerDocument.CreateElement("Attrs");
                        aFCElement.AppendChild(AttrsElement);

                        #region 获取源数据与目标数据字段对应关系属性

                        XPath = ".//字段对应关系";
                        XmlNodeList aTempNodeList = aRuleElement["目标数据"].SelectNodes(XPath);

                        if (aTempNodeList == null)
                        {
                            errElement.SetAttribute("字段对应关系错误", "未设置源数据与目标数据字段对应关系");
                            bRes = false;
                        }
                        else
                        {
                            if (aTempNodeList.Count > 0)
                            {
                                //遍历原始数据的所有字段(OID和Shape除外)
                                //找到目标图层对应全部字段 ，记录它们的Index对

                                string OrgFCName = aFCElement.GetAttribute("OrgFCName");
                                string ObjFCName = aFCElement.GetAttribute("ObjFCName");

                                //获取源属性名称和序号的集合

                                Dictionary<string, int> dicOrgFieldIndex = m_OrgSysGisDataSet.GetFieldIndexs(OrgFCName, false);
                                if (dicOrgFieldIndex == null)
                                {
                                    errElement.SetAttribute("数据库连接错误", "无法获取图层" + OrgFCName);
                                    bRes = false;
                                    continue;
                                }
                                if (dicOrgFieldIndex.Count == 0) continue;

                                //获取目标属性名称和序号的集合

                                Dictionary<string, int> dicObjFieldIndex = m_ObjSysGisDataSet.GetFieldIndexs(ObjFCName, false);
                                if (dicObjFieldIndex == null)
                                {
                                    errElement.SetAttribute("数据库连接错误", "无法获取图层" + ObjFCName);
                                    bRes = false;
                                    continue;
                                }
                                if (dicObjFieldIndex.Count == 0) continue;

                                //为源和目标的属性名称和序号集合进行配对
                                GetFieldsIndex(OrgFeatureClassName, AttrsElement, aTempNodeList, dicOrgFieldIndex, dicObjFieldIndex, dicValues, rulexmldoc);

                                if (AttrsElement.HasAttribute("Err"))        //属性对应关系不匹配
                                {
                                    errElement.SetAttribute("字段对应关系匹配错误", "源数据与目标数据字段对应关系不匹配");
                                    bRes = false;
                                }
                            }
                        }
                        #endregion
                        #endregion
                    }
                }
            } 
            return bRes;
        }

        /// <summary>
        /// 根据 字段对集合，原始数据字段集合，目标数据字段集合

        /// 将字段索引节点添加到指定Attrs节点下并记录字段索引对

        /// </summary>
        /// <param name="attrselement"> 指定Attrs节点</param>
        /// <param name="fieldpairnodelist"> 字段对集合</param>
        /// <param name="dicOrgFieldIndex"> 原始数据字段集合</param>
        /// <param name="dicValues"> 环境变量值</param>
        /// <returns></returns>
        private void GetFieldsIndex(string OrgFeatureClassName, XmlElement attrselement, XmlNodeList fieldpairnodelist, Dictionary<string, int> dicOrgFieldIndex, Dictionary<string, int> dicObjFieldIndex, Dictionary<string, object> dicValues, XmlDocument ruleXmlDoc)
        {
            if (fieldpairnodelist == null && fieldpairnodelist.Count == 0) return;

            if (dicOrgFieldIndex.Count == 0 || dicObjFieldIndex.Count == 0) return;

            Dictionary<string, string> dicFields = new Dictionary<string, string>();
            foreach (XmlNode aFieldPairNode in fieldpairnodelist)
            {
                XmlElement aFieldPairElement = (XmlElement)aFieldPairNode;
                string ObjFieldName = aFieldPairElement["名称"].InnerText.ToUpper();
                string OrgFieldName = aFieldPairElement["值"].InnerText.ToUpper();

                dicFields.Add(ObjFieldName, OrgFieldName);
            }

            //  记录已经被赋值的目标字段索引
            List<int> ObjFieldIndexs = new List<int>();
            foreach (KeyValuePair<string, string> FieldNamePair in dicFields)
            {
                if (FieldNamePair.Value == @"*" && FieldNamePair.Key == @"*")
                {   //全部目标字段按同名进行转化


                    foreach (KeyValuePair<string, int> keyvalue in dicOrgFieldIndex)
                    {
                        // 原始字段是否存在于目标字段字典中
                        if (!dicObjFieldIndex.ContainsKey(keyvalue.Key)) continue;
                        // 目标字段是否已经被赋值

                        if (ObjFieldIndexs.Contains(dicObjFieldIndex[keyvalue.Key])) continue;

                        XmlElement AttrElement = attrselement.OwnerDocument.CreateElement("Attr");
                        attrselement.AppendChild(AttrElement);
                        AttrElement.SetAttribute("OrgField", keyvalue.Key);
                        AttrElement.SetAttribute("OrgIndex", Convert.ToString(keyvalue.Value));
                        AttrElement.SetAttribute("ObjField", keyvalue.Key);
                        AttrElement.SetAttribute("ObjIndex", Convert.ToString(dicObjFieldIndex[keyvalue.Key]));

                        ObjFieldIndexs.Add(dicObjFieldIndex[keyvalue.Key]);
                    }
                }
                else
                {   // 目标字段是否存在于数据库中

                    string ObjFieldName = FieldNamePair.Key;
                    if (!dicObjFieldIndex.ContainsKey(ObjFieldName))
                    {
                        attrselement.SetAttribute("Err", "不匹配");
                        continue;
                    }

                    // 目标字段是否已经被赋值

                    int ObjFieldIndex = dicObjFieldIndex[ObjFieldName];
                    if (ObjFieldIndexs.Contains(ObjFieldIndex)) continue;

                    if (FieldNamePair.Value.StartsWith("%"))
                    {
                        string OrgFieldName = FieldNamePair.Value.Substring(1);
                        if (!dicOrgFieldIndex.ContainsKey(OrgFieldName)) continue;

                        // 创建对应的字段配对

                        XmlElement AttrElement = attrselement.OwnerDocument.CreateElement("Attr");
                        attrselement.AppendChild(AttrElement);

                        AttrElement.SetAttribute("ObjField", ObjFieldName);
                        AttrElement.SetAttribute("ObjIndex", Convert.ToString(ObjFieldIndex));
                        AttrElement.SetAttribute("OrgField", OrgFieldName);
                        AttrElement.SetAttribute("OrgIndex", Convert.ToString(dicOrgFieldIndex[OrgFieldName]));
                    }
                    else if (FieldNamePair.Value.StartsWith(@"@"))
                    {   //环境变量
                        XmlElement AttrElement = attrselement.OwnerDocument.CreateElement("Attr");
                        attrselement.AppendChild(AttrElement);

                        string EnvConstantValue = GetEnvConstantByKey(FieldNamePair.Value, dicValues);
                        if (EnvConstantValue.Length == 0) continue;

                        AttrElement.SetAttribute("ObjField", ObjFieldName);
                        AttrElement.SetAttribute("ObjIndex", Convert.ToString(ObjFieldIndex));
                        AttrElement.SetAttribute("OrgField", "");
                        AttrElement.SetAttribute("OrgIndex", Convert.ToString(-3));
                        AttrElement.InnerText = EnvConstantValue;
                    }
                    else if (FieldNamePair.Value.ToUpper().StartsWith("KEYVALUE@"))
                    {
                        string OrgFieldName = FieldNamePair.Value.Substring(9);
                        if (!dicOrgFieldIndex.ContainsKey(OrgFieldName)) continue;

                        // 创建对应的字段配对

                        XmlElement AttrElement = attrselement.OwnerDocument.CreateElement("Attr");
                        attrselement.AppendChild(AttrElement);

                        AttrElement.SetAttribute("ObjField", ObjFieldName);
                        AttrElement.SetAttribute("ObjIndex", Convert.ToString(ObjFieldIndex));
                        AttrElement.SetAttribute("OrgField", OrgFieldName);
                        AttrElement.SetAttribute("OrgIndex", Convert.ToString(dicOrgFieldIndex[OrgFieldName]));
                        XmlNodeList keyValueNodeList = ruleXmlDoc.SelectNodes(".//keyValue");
                        if (keyValueNodeList == null) continue;
                        foreach (XmlNode keyValueNode in keyValueNodeList)
                        {
                            if (keyValueNode.SelectSingleNode(".//名称").InnerText == ObjFieldName)
                            {
                                XmlNodeList tempNodeList = keyValueNode.SelectNodes(".//图层");
                                if (tempNodeList == null) continue;
                                foreach (XmlNode tempNode in tempNodeList)
                                {
                                    if (tempNode.SelectSingleNode(".//名称").InnerText.Trim() == OrgFeatureClassName)
                                    {
                                        //XmlNode tempNode = keyValueNode.SelectSingleNode(".//图层[@名称='" + OrgFeatureClassName + "']");
                                        XmlNode newNode = AttrElement.OwnerDocument.ImportNode(tempNode, true);
                                        AttrElement.AppendChild(newNode);
                                    }
                                }
                            }
                        }


                    }
                    else
                    {   //常量值

                        XmlElement AttrElement = attrselement.OwnerDocument.CreateElement("Attr");
                        attrselement.AppendChild(AttrElement);

                        AttrElement.SetAttribute("ObjField", ObjFieldName);
                        AttrElement.SetAttribute("ObjIndex", Convert.ToString(ObjFieldIndex));
                        AttrElement.SetAttribute("OrgField", "");
                        AttrElement.SetAttribute("OrgIndex", Convert.ToString(-1));
                        AttrElement.InnerText = FieldNamePair.Value;
                    }

                    // 将此字段索引加入到已经被赋值的目标字段集合中

                    ObjFieldIndexs.Add(ObjFieldIndex);
                }
            }
        }

        /// <summary>
        /// 得到环境变量对应的常量

        /// </summary>
        /// <param name="key">环境变量</param>
        /// <param name="dicValues">环境变量值</param>
        /// <returns></returns>
        private string GetEnvConstantByKey(string key, Dictionary<string, object> dicValues)
        {
            if (key.Length == 0 || !key.StartsWith(@"@")) return string.Empty;

            // 除去@符号
            string TempKey = key.Substring(1);
            int index = TempKey.IndexOf('!');
            if (index < 0) return string.Empty;

            object value = string.Empty;
            if (dicValues.ContainsKey(TempKey))
            {
                value = dicValues[TempKey];
            }

            return Convert.ToString(value);
        }

        //  根据操作类型设置过滤条件
        private string GetSubWhereClause(XmlElement conelement)
        {
            try
            {
                // 获取操作类型
                string SubWhereClause = string.Empty;
                switch (conelement["Operator"].InnerText)
                {
                    case ("IN"):
                        SubWhereClause += conelement["AttributeName"].InnerText + " " + conelement["Operator"].InnerText + " (" + conelement["AttributeValue"].InnerText + ")";
                        break;

                    case ("EQ"):
                        SubWhereClause += conelement["AttributeName"].InnerText + " = " + conelement["AttributeValue"].InnerText;
                        break;

                    case ("NE"):
                        SubWhereClause += conelement["AttributeName"].InnerText + " <> " + conelement["AttributeValue"].InnerText;
                        break;

                    case ("LT"):
                        SubWhereClause += conelement["AttributeName"].InnerText + " < " + conelement["AttributeValue"].InnerText;
                        break;

                    case ("GT"):
                        SubWhereClause += conelement["AttributeName"].InnerText + " > " + conelement["AttributeValue"].InnerText;
                        break;

                    case ("LE"):
                        SubWhereClause += conelement["AttributeName"].InnerText + " <= " + conelement["AttributeValue"].InnerText;
                        break;

                    case ("GE"):
                        SubWhereClause += conelement["AttributeName"].InnerText + " >= " + conelement["AttributeValue"].InnerText;
                        break;

                    case ("LIKE"):
                        SubWhereClause += conelement["AttributeName"].InnerText + " like " + conelement["AttributeValue"].InnerText;
                        break;

                    case ("ULIKE"):
                        SubWhereClause += conelement["AttributeName"].InnerText + " not like " + conelement["AttributeValue"].InnerText;
                        break;

                    case ("WhereClause"):
                        SubWhereClause += conelement["WhereClause"].InnerText;
                        break;

                    default:
                        return string.Empty;
                }

                // 获取条件类型
                switch (conelement["ConditionType"].InnerText)
                {
                    case ("GDC_WHERE"):
                        break;

                    case ("GDC_AND"):
                        SubWhereClause = " and " + SubWhereClause;
                        break;

                    case ("GDC_NOT"):
                        SubWhereClause = " or " + SubWhereClause;
                        break;

                    default:
                        return string.Empty;
                }
                return SubWhereClause;
            }
            catch (Exception e)
            {
                //*******************************************************************
                //Excption Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                return string.Empty;
            }
        }

        // 数据入库中未用到条件过滤,暂时XML相关节点未做整理修改 wjj.............
        //  通过原始数据节点得到过滤条件
        private string GetCondition(XmlNode sourcenode, out bool bsuccessed)
        {
            bsuccessed = false;
            string XPath = ".//Condition[ConditionType = 'GDC_WHERE']";
            XmlElement MainElement = (XmlElement)sourcenode.SelectSingleNode(XPath);

            if (MainElement == null)
            {
                bsuccessed = true;
                return string.Empty;
            }

            string Condition = GetSubWhereClause(MainElement);
            if (Condition.Length == 0) return string.Empty;

            // 取得其他过滤条件
            XPath = ".//Condition[ConditionType = 'GDC_AND']";
            XmlNodeList ConsNodeList = sourcenode.SelectNodes(XPath);
            foreach (XmlNode aConNode in ConsNodeList)
            {
                XmlElement ConElement = (XmlElement)aConNode;
                string SubCondition = GetSubWhereClause(ConElement);

                if (SubCondition.Length == 0) return string.Empty;
                Condition = Condition + SubCondition;
            }

            //071013 zhuyi 添加 补充运算符


            XPath = ".//Condition[ConditionType = 'GDC_NOT']";
            ConsNodeList = sourcenode.SelectNodes(XPath);
            foreach (XmlNode aConNode in ConsNodeList)
            {
                XmlElement ConElement = (XmlElement)aConNode;
                string SubCondition = GetSubWhereClause(ConElement);

                if (SubCondition.Length == 0) return string.Empty;
                Condition = Condition + SubCondition;
            }
            //071013 zhuyi 添加

            bsuccessed = true;
            return Condition;
        }

        #endregion

        private delegate void ChangeProgress(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value);
        private void ChangeProgressBar(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value)
        {
            if (min != -1)
            {
                pProgressBar.Minimum = min;
            }
            if (max != -1)
            {
                pProgressBar.Maximum = max;
            }
            pProgressBar.Value = value;
            pProgressBar.Refresh();
        }

        #region 数据入库
        /// <summary>
        /// 数据入库
        /// </summary>
        /// <param name="OrgFCName">源数据FC</param>
        /// <param name="ObjFCName">目标数据FC</param>
        /// <param name="OrgCondition">源数据过滤条件</param>
        /// <param name="attrselement">源与目标数据字段对应关系</param>
        /// <param name="reselement">入库结果日志</param>
        /// <param name="pAppForm">主窗体以获取进度条</param>
        /// <param name="bUpdate">true-更新,false-入库</param>
        /// <param name="bExit">遇到错误是否跳出</param>
        /// <returns></returns>
        public bool ImpFeatures(string OrgFCName, string ObjFCName, string OrgCondition, XmlElement attrselement, XmlElement reselement, Plugin.Application.IAppFormRef pAppForm, IGeometry pGeometry, string strSpatialRel, esriSpatialRelEnum pSpatialRelEnum, bool bExit, EnumOperateType pOpeType, SysCommon.Gis.SysGisDataSet pHistoDT, string pDateStr)
        {
            int count = -1;
            int total = -1;
            Exception errEx = null;

            //  根据过滤条件获取源数据

            IFeatureCursor pFeaCursor = null;
            if (strSpatialRel != null)
            {
                pFeaCursor = m_OrgSysGisDataSet.GetFeatureCursor(OrgFCName, OrgCondition, pGeometry, strSpatialRel, out errEx, out count, out total);
            }
            else if (pSpatialRelEnum != esriSpatialRelEnum.esriSpatialRelUndefined)
            {
                pFeaCursor = m_OrgSysGisDataSet.GetFeatureCursor(OrgFCName, OrgCondition, pGeometry, pSpatialRelEnum, out errEx, out count, out total);
            }
            else
                pFeaCursor = m_OrgSysGisDataSet.GetFeatureCursor(OrgFCName, OrgCondition, null, "", out errEx, out count, out total);

            if (errEx != null)
            {
                reselement.SetAttribute("ErrorE", "获取源数据失败," + errEx.ToString());
                reselement.SetAttribute("success", "0");
                return false;
            }

            reselement.SetAttribute("orifeaclass", OrgFCName);                //源要素类
            reselement.SetAttribute("objfeaclass", ObjFCName);                //目标要素类

            reselement.SetAttribute("orgfeacondition", OrgCondition);         //条件
            reselement.SetAttribute("count", count.ToString());               //入库要素数

            reselement.SetAttribute("total", total.ToString());               //源要素类要素总数
            reselement.SetAttribute("starttime", DateTime.Now.ToString());    //开始时间


            if (count == 0)
            {
                reselement.SetAttribute("success", "1");
                reselement.SetAttribute("endtime", DateTime.Now.ToString());  //结束时间
                return true;
            }

            int index = ObjFCName.IndexOf("\\");
            if (index > 0) ObjFCName = ObjFCName.Substring(index + 1);

            if (!pAppForm.MainForm.IsDisposed && pAppForm.MainForm.InvokeRequired)
            {
                pAppForm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { pAppForm.ProgressBar, 0, count, 1 });
            }
            else
            {
                pAppForm.ProgressBar.Minimum = 0;
                pAppForm.ProgressBar.Maximum = count;
                pAppForm.ProgressBar.Value = 1;
            }
            bool Result = ImpSimpleFeatures(ObjFCName, OrgFCName, pFeaCursor, attrselement, reselement, pAppForm, pGeometry, bExit, pOpeType, pHistoDT, pDateStr);
            reselement.SetAttribute("endtime", DateTime.Now.ToString());  //结束时间

            //释放cursor
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);

            return Result;
        }

        //  简单要素类数据入库
        //  注意:注记要素类型也使用同样的方法[效率改善?]
        private bool ImpSimpleFeatures(string pfeaclsname, string orgFcName, IFeatureCursor pfeacursor, XmlElement attrselement, XmlElement reselement, Plugin.Application.IAppFormRef pAppForm, IGeometry pGeometry, bool bExit, EnumOperateType pOpeType, SysCommon.Gis.SysGisDataSet pHistoDT, string pDateStr)
        {
            Exception errEx = null;
            IFeatureClass pFeatureCls = m_ObjSysGisDataSet.GetFeatureClass(pfeaclsname, out errEx);//目标库

            if (errEx != null)
            {
                reselement.SetAttribute("ErrorE", "获取目标数据失败," + errEx.ToString());
                reselement.SetAttribute("success", "0");
                return false;
            }
            IFeatureClass pHistoFeaCls = null;//矢量历史库要素类
            if (pHistoDT != null)
            {
                if (pHistoDT.WorkSpace != null)
                {
                    pHistoFeaCls = pHistoDT.GetFeatureClass(pfeaclsname + "_GOH", out errEx);//矢量历史库


                    if (errEx != null)
                    {
                        reselement.SetAttribute("ErrorE", "获取矢量历史库数据失败," + errEx.ToString());
                        reselement.SetAttribute("success", "0");
                        return false;
                    }
                }
            }
            IWorkspaceEdit pWSEdit = m_ObjSysGisDataSet.WorkSpace as IWorkspaceEdit;//开启目标库编辑
            if (!pWSEdit.IsBeingEdited())
            {
                reselement.SetAttribute("ErrorE", "目标数据不处于编辑状态");
                reselement.SetAttribute("success", "0");
                return false;
            }

            //提数据同时提FID记录表内容时不用该注记方法

            if (pFeatureCls.FeatureType == esriFeatureType.esriFTAnnotation && m_OutDataBase == null && m_InfoTable == null)
            {
                return ImpAnnoFeatures(orgFcName, pFeatureCls, pfeacursor, attrselement, reselement, pAppForm, bExit);
            }

            int icnt = 0;
            int iCount = 0;
            int orgOID = -1;
            //创建目标库缓冲

            IFeatureBuffer pFeatureBuffer = null;
            IFeatureCursor pNewFeatureCursor = null;
            IFeature pFeature = null;
            try
            {
                pFeatureBuffer = pFeatureCls.CreateFeatureBuffer();
                pNewFeatureCursor = pFeatureCls.Insert(true);
                pFeature = pfeacursor.NextFeature();
            }
            catch (Exception ex)
            {
                reselement.SetAttribute("ErrorE", ex.ToString());
                reselement.SetAttribute("success", "0");
                return false;
            }

            bool bInFeat = true;
            while (pFeature != null)
            {
                try
                {
                    orgOID = pFeature.OID;

                    //======================================
                    //chenyafei  20110105  add   ：若源数据的几何形状为0长度线或0面积面，则排除掉
                    #region 排除掉不规则的形状
                    IGeometry pFeaGeo = pFeature.Shape;
                    if (pFeaGeo.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        IPointCollection mPointCol = new PolygonClass();
                        mPointCol.AddPointCollection(pFeaGeo as IPointCollection);

                        //闭合点集合成面状
                        IPolygon mPolygon = mPointCol as IPolygon;
                        mPolygon.Close();

                        //根据闭合的面状进行查找
                        mPointCol = mPolygon as IPointCollection;
                        if (mPointCol is IArea)
                        {
                            IArea pArea = mPointCol as IArea;
                            if (pArea != null)
                            {
                                if (pArea.Area == 0)
                                {
                                    //0面积面
                                    pFeature = pfeacursor.NextFeature();
                                    continue;
                                }
                            }
                        }
                    }
                    else if (pFeaGeo.GeometryType == esriGeometryType.esriGeometryPolyline)
                    {
                        //零长度线
                         int index = -1;
                         //获得要素长度字段索引
                         for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                         {
                             IField pField = pFeature.Fields.get_Field(i);   //字段
                             string fieldName = pField.Name;  //字段名
                             //获取线长度字段索引
                             if (fieldName.ToUpper().Contains("LENGTH") && pField.Type == esriFieldType.esriFieldTypeDouble) //xisheng 2011.07.20 线因有一个shape_leng为0导致全部不能入库
                             {
                                 //线长度字段
                                 index = i;
                                 break;
                             }
                         }
                         if (index != -1)
                         {
                             try
                             {
                                 double pLen =Convert.ToDouble(pFeature.get_Value(index).ToString());
                                 if (pLen == 0)
                                 {
                                     pFeature = pfeacursor.NextFeature();
                                     continue;
                                 }
                             }
                             catch
                             { }
                         }
                    }
                    #endregion
                    //====================================

                    //创建一个矢量历史库的ifeature
                    IFeature pHistoFeature = null;
                    int fromdateIndex = -1;//生效日期
                    int todateIndex = -1;//失效日期
                    int stateIndex = -1;//更新状态
                    int newVersion = ModData.DBVersion;  //版本

                    int sourceIDIndex = -1;//目标库数据OID
                    if (pHistoFeaCls != null)
                    {
                        pHistoFeature = pHistoFeaCls.CreateFeature();
                        fromdateIndex = pHistoFeaCls.Fields.FindField("FromDate");
                        todateIndex = pHistoFeaCls.Fields.FindField("ToDate");
                        stateIndex = pHistoFeaCls.Fields.FindField("State");
                        sourceIDIndex = pHistoFeaCls.Fields.FindField("SourceOID");
                        newVersion = pHistoFeaCls.Fields.FindField("VERSION");
                        if (fromdateIndex == -1 || todateIndex == -1 || stateIndex == -1 || sourceIDIndex == -1 || newVersion==-1)
                        {
                            reselement.SetAttribute("ErrorE", "矢量历史库中的缺少必须的字段名,请检查！");
                            bInFeat = false;
                            break;
                        }
                    }

                    //  此循环过程中因为无法有效存储固定值

                    //  固此循环没有在开始外面完成而是嵌套在大循环内部
                    foreach (XmlNode xmlnode in attrselement.ChildNodes)
                    {
                        XmlElement aElement = (XmlElement)xmlnode;
                        int SDEIndex = Convert.ToInt32(aElement.GetAttribute("ObjIndex"));//目标字段索引
                        int PDBIndex = Convert.ToInt32(aElement.GetAttribute("OrgIndex"));//源字段索引

                        int HistoIndex = -1;//矢量历史库字段索引

                        if (pHistoFeaCls != null)
                        {
                            HistoIndex = pHistoFeaCls.Fields.FindField(pHistoFeaCls.Fields.get_Field(SDEIndex).Name);

                            if (HistoIndex == -1)
                            {
                                reselement.SetAttribute("ErrorE", "矢量历史库与目标库中的字段名不匹配,请检查！");
                                bInFeat = false;
                                break;
                            }
                        }
                        switch (PDBIndex)
                        {
                            case (-1):      //  环境变量
                            case (-3):      //  固定值

                                if (aElement.InnerText != "")
                                {
                                    if (pFeatureBuffer.Fields.get_Field(SDEIndex).Type == esriFieldType.esriFieldTypeDate)
                                    {
                                        try
                                        {
                                            Convert.ToDateTime(aElement.InnerText);
                                            pFeatureBuffer.set_Value(SDEIndex, aElement.InnerText);//目标库赋值
                                        }
                                        catch
                                        {

                                        }
                                        if (!pFeatureBuffer.Fields.get_Field(SDEIndex).IsNullable)
                                        {
                                            pFeatureBuffer.set_Value(SDEIndex, DateTime.Now);
                                        }
                                    }
                                    else
                                    {
                                        pFeatureBuffer.set_Value(SDEIndex, aElement.InnerText);//目标库赋值
                                    }

                                    if (pHistoFeature != null)
                                    {
                                        if (pHistoFeature.Fields.get_Field(HistoIndex).Type == esriFieldType.esriFieldTypeDate)
                                        {
                                            try
                                            {
                                                Convert.ToDateTime(aElement.InnerText);
                                                pHistoFeature.set_Value(HistoIndex, aElement.InnerText);//矢量历史库赋值
                                            }
                                            catch
                                            {

                                            }
                                            if (!pHistoFeature.Fields.get_Field(HistoIndex).IsNullable)
                                            {
                                                pHistoFeature.set_Value(HistoIndex, DateTime.Now);
                                            }
                                        }
                                        else
                                        {
                                            pHistoFeature.set_Value(HistoIndex, aElement.InnerText);//矢量历史库赋值
                                        }

                                    }

                                }
                                break;
                            default:        //　>=0的情形

                                if ((xmlnode.ChildNodes != null) && (xmlnode.ChildNodes.Count > 0))
                                {//对应字段值,映射
                                    XmlNodeList relationNodeList = xmlnode.SelectNodes(".//对应关系");
                                    bool isReflection = false;
                                    foreach (XmlNode relationNode in relationNodeList)
                                    {
                                        string oriFieldValue = relationNode["Key"].InnerText;
                                        string pValue = pFeature.get_Value(PDBIndex).ToString();
                                        if (pValue == "")
                                        {
                                            //pFeatureBuffer.set_Value(SDEIndex, "");//目标库

                                            //if (pHistoFeature != null)
                                            //{
                                            //    pHistoFeature.set_Value(HistoIndex, "");//矢量历史库

                                            //}
                                            isReflection = true;
                                            break;
                                        }
                                        else if (pValue == oriFieldValue)
                                        {
                                            object objFieldValue = relationNode["Value"].InnerText as object;
                                            if (objFieldValue.ToString() != "")
                                            {
                                                if (pFeatureBuffer.Fields.get_Field(SDEIndex).Type == esriFieldType.esriFieldTypeDate)
                                                {
                                                    try
                                                    {
                                                        Convert.ToDateTime(objFieldValue);
                                                        pFeatureBuffer.set_Value(SDEIndex, objFieldValue);//目标库赋值
                                                    }
                                                    catch
                                                    {

                                                    }
                                                    if (!pFeatureBuffer.Fields.get_Field(SDEIndex).IsNullable)
                                                    {
                                                        pFeatureBuffer.set_Value(SDEIndex, DateTime.Now);
                                                    }
                                                }
                                                else
                                                {
                                                    pFeatureBuffer.set_Value(SDEIndex, objFieldValue);//目标库
                                                }

                                                if (pHistoFeature != null)
                                                {
                                                    if (pHistoFeature.Fields.get_Field(HistoIndex).Type == esriFieldType.esriFieldTypeDate)
                                                    {
                                                        try
                                                        {
                                                            Convert.ToDateTime(objFieldValue);
                                                            pHistoFeature.set_Value(HistoIndex, objFieldValue);//矢量历史库赋值
                                                        }
                                                        catch
                                                        {

                                                        }
                                                        if (!pHistoFeature.Fields.get_Field(HistoIndex).IsNullable)
                                                        {
                                                            pHistoFeature.set_Value(HistoIndex, DateTime.Now);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        pHistoFeature.set_Value(HistoIndex, objFieldValue);//矢量历史库
                                                    }

                                                }
                                            }
                                            isReflection = true;
                                            break;
                                        }
                                        else if (pValue != oriFieldValue)
                                        {
                                            continue;
                                        }
                                    }
                                    if (isReflection == false)
                                    {
                                        if (pFeature.get_Value(PDBIndex).ToString() != "")
                                        {
                                            if (pFeatureBuffer.Fields.get_Field(SDEIndex).Type == esriFieldType.esriFieldTypeDate)
                                            {
                                                try
                                                {
                                                    Convert.ToDateTime(pFeature.get_Value(PDBIndex));
                                                    pFeatureBuffer.set_Value(SDEIndex, pFeature.get_Value(PDBIndex));//目标库赋值
                                                }
                                                catch
                                                {

                                                }
                                                if (!pFeatureBuffer.Fields.get_Field(SDEIndex).IsNullable)
                                                {
                                                    pFeatureBuffer.set_Value(SDEIndex, DateTime.Now);
                                                }
                                            }
                                            else
                                            {
                                                pFeatureBuffer.set_Value(SDEIndex, pFeature.get_Value(PDBIndex));//目标库
                                            }

                                            if (pHistoFeature != null)
                                            {
                                                if (pHistoFeature.Fields.get_Field(HistoIndex).Type == esriFieldType.esriFieldTypeDate)
                                                {
                                                    try
                                                    {
                                                        Convert.ToDateTime(pFeature.get_Value(PDBIndex));
                                                        pHistoFeature.set_Value(HistoIndex, pFeature.get_Value(PDBIndex));//矢量历史库赋值
                                                    }
                                                    catch
                                                    {

                                                    }
                                                    if (!pHistoFeature.Fields.get_Field(HistoIndex).IsNullable)
                                                    {
                                                        pHistoFeature.set_Value(HistoIndex, DateTime.Now);
                                                    }
                                                }
                                                else
                                                {

                                                    pHistoFeature.set_Value(HistoIndex, pFeature.get_Value(PDBIndex));//矢量历史库
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //普通

                                    if (pFeature.get_Value(PDBIndex).ToString() != "")
                                    {
                                        if (pFeatureBuffer.Fields.get_Field(SDEIndex).Type == esriFieldType.esriFieldTypeDate)
                                        {
                                            try
                                            {
                                                Convert.ToDateTime(pFeature.get_Value(PDBIndex));
                                                pFeatureBuffer.set_Value(SDEIndex, pFeature.get_Value(PDBIndex));//目标库赋值
                                            }
                                            catch
                                            {

                                            }
                                            if (!pFeatureBuffer.Fields.get_Field(SDEIndex).IsNullable)
                                            {
                                                pFeatureBuffer.set_Value(SDEIndex, DateTime.Now);
                                            }
                                        }
                                        else
                                        {
                                            pFeatureBuffer.set_Value(SDEIndex, pFeature.get_Value(PDBIndex));//目标库
                                        }

                                        if (pHistoFeature != null)
                                        {
                                            if (pHistoFeature.Fields.get_Field(HistoIndex).Type == esriFieldType.esriFieldTypeDate)
                                            {
                                                try
                                                {
                                                    Convert.ToDateTime(pFeature.get_Value(PDBIndex));
                                                    pHistoFeature.set_Value(HistoIndex, pFeature.get_Value(PDBIndex));//矢量历史库赋值
                                                }
                                                catch
                                                {

                                                }
                                                if (!pHistoFeature.Fields.get_Field(HistoIndex).IsNullable)
                                                {
                                                    pHistoFeature.set_Value(HistoIndex, DateTime.Now);
                                                }
                                            }
                                            else
                                            {
                                                pHistoFeature.set_Value(HistoIndex, pFeature.get_Value(PDBIndex));//矢量历史库
                                            }

                                        }
                                    }
                                }
                                break;
                        }
                    }

                    //  一定要将原始数据的Shape赋给新建数据
                    IGeometry pNewGeometry = pFeature.Shape;
                    if ((pFeatureCls.ShapeType == esriGeometryType.esriGeometryPolygon || pFeatureCls.ShapeType == esriGeometryType.esriGeometryPolyline) && pFeatureCls.FeatureType != esriFeatureType.esriFTAnnotation)
                    {
                        //自动做拓扑简单化
                        ITopologicalOperator2 pTopologicalOperator2 = (ITopologicalOperator2)pNewGeometry;
                        pTopologicalOperator2.IsKnownSimple_2 = false;
                        pTopologicalOperator2.Simplify();
                        pNewGeometry = (IGeometry)pTopologicalOperator2;
                    }

                    IGeometry OutputGeometry = pNewGeometry;

                    //提取时将相交要素的处于外面的图形提取进来
                    if (pOpeType == EnumOperateType.Stract)
                    {
                        if (pGeometry != null)
                        {
                            //如果范围条件不为空则进行图形求交
                            //cyf  20110621 add:添加对几何范围的空间参考的控制
                            if (pGeometry.SpatialReference != null)
                            {
                                if (pGeometry.SpatialReference.FactoryCode != pNewGeometry.SpatialReference.FactoryCode)
                                {
                                    reselement.SetAttribute("ErrorE", "范围与源数据空间参考不一致!");
                                    bInFeat = false;
                                    break;
                                }
                            }
                            //end
                            ITopologicalOperator pTopo = pNewGeometry as ITopologicalOperator;
                            //若为提取，则求差
                            OutputGeometry = pTopo.Difference(pGeometry);
                            pTopo = OutputGeometry as ITopologicalOperator;
                            pTopo.Simplify();
                            OutputGeometry = pTopo as IGeometry;

                            //OutputGeometry = pNewGeometry;
                            if (OutputGeometry.IsEmpty)
                            {
                                //若几何为空

                                pFeature = pfeacursor.NextFeature();
                                continue;
                            }
                            //else
                            //{
                            //if (pNewGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
                            //{
                            //    OutputGeometry = pTopo.Intersect(pGeometry, esriGeometryDimension.esriGeometry2Dimension);
                            //}
                            //else if (pNewGeometry.GeometryType == esriGeometryType.esriGeometryPolyline)
                            //{
                            //    OutputGeometry = pTopo.Intersect(pGeometry, esriGeometryDimension.esriGeometry1Dimension);
                            //}
                            //}
                        }
                    }
                    //判断目标数据空间参考与源数据空间参考是否一致

                    IGeoDataset pDataset = pFeatureCls as IGeoDataset;
                    //cyf  20110621 add:添加对几何范围的空间参考的控制
                    if (OutputGeometry.SpatialReference != null)
                    {
                        if (OutputGeometry.SpatialReference.FactoryCode != pDataset.SpatialReference.FactoryCode)
                        {
                            reselement.SetAttribute("ErrorE", "目标数据与源数据空间参考不一致!");
                            bInFeat = false;
                            break;
                        }
                    }
                    //判断源数据与矢量历史库空间参考是否一致

                    IGeoDataset pHistoDataset = pHistoFeaCls as IGeoDataset;
                    if (pHistoDataset != null)
                    {
                        //cyf  20110621 add:添加对几何范围的空间参考的控制
                        if (OutputGeometry.SpatialReference != null)
                        {
                            if (OutputGeometry.SpatialReference.FactoryCode != pHistoDataset.SpatialReference.FactoryCode)
                            {
                                reselement.SetAttribute("ErrorE", "矢量历史库数据与源数据空间参考不一致，请检查!");
                                bInFeat = false;
                                break;
                            }
                        }
                        //end
                    }
                    pFeatureBuffer.Shape = OutputGeometry;
                    int newOid = (int)pNewFeatureCursor.InsertFeature(pFeatureBuffer);
                    if (pHistoFeature != null)
                    {
                        //给矢量历史库的SHAPE字段赋值

                        pHistoFeature.Shape = OutputGeometry;
                        //给矢量历史库中多余的字段赋值
                        Exception eError=null;
                        ModData.DBVersion = GetVersion(m_ObjSysGisDataSet.WorkSpace, out eError); //版本
                        if (eError != null)
                        {
                        }
                        pHistoFeature.set_Value(fromdateIndex, pDateStr);//生效日期
                        pHistoFeature.set_Value(todateIndex, DateTime.MaxValue.ToString("u"));//失效日期
                        pHistoFeature.set_Value(stateIndex, 1);//状态：新建
                        pHistoFeature.set_Value(sourceIDIndex, newOid);//目标中对应要素的OID
                        pHistoFeature.set_Value(newVersion, ModData.DBVersion);  //历史库版本
                        pHistoFeature.Store();
                    }

                    //图幅数据提交时，根据新入库的数据生成FID，写到FID记录表

                    if (pOpeType == EnumOperateType.Submit)
                    {
                        //cyf 20110621 delete
                        //if (m_OutDataBase != null)
                        //{
                        //    string str = "insert into FID记录表(FCNAME,OID) values('" + pfeaclsname + "'," + newOid + ")";
                        //    m_OutDataBase.UpdateTable(str, out errEx);
                        //    if (errEx != null)
                        //    {
                        //        reselement.SetAttribute("ErrorE", "更新业务维护信息失败,错误原因" + errEx.ToString() + ",位置源数据OID:" + orgOID.ToString());
                        //        bInFeat = false;
                        //        if (bExit == true)
                        //        {
                        //            break;
                        //        }
                        //    }
                        //}
                        //end

                        //图幅数据提交后，更新日志记录表
                        Exception eError = null;
                        IWorkspace pWS = m_ObjSysGisDataSet.WorkSpace;  //目标工作空间
                        IRow pRow =pFeatureBuffer as IRow; //更新行
                        int iState = 1;    //更新状态  新增
                        DateTime SaveTime = DateTime.Now;
                        try
                        {
                            SaveTime = Convert.ToDateTime(pDateStr);  //保存时间
                        }
                        catch
                        { }
                       
                        string sLayerName = pfeaclsname;   //图层名
                        //写入更新日志表
                        RecordLOG(pWS, pRow, iState, SaveTime, ModData.DBVersion, sLayerName,newOid, out eError);
                        if (eError != null)
                        {
                            reselement.SetAttribute("ErrorE", "更新编辑记录失败。\n原因：" + eError.Message);
                            bInFeat = false;
                            break;
                        }
                    }
                    //提取FID记录表对应信息

                    if (m_InfoTable != null && m_OutDataBase != null)
                    {
                        DataRow[] pDataRow = m_InfoTable.Select("OID=" + orgOID + " and FCNAME='" + orgFcName + "'");
                        if (pDataRow.Length == 0)
                        {
                            reselement.SetAttribute("ErrorE", "获取业务维护信息失败,位置源数据OID:" + orgOID.ToString());
                            bInFeat = false;
                            if (bExit == true)
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (FIDLogFeatureOID(Convert.ToInt32(pDataRow[0]["GOFID"]), pfeaclsname, newOid, out errEx) == false)
                            {
                                if (errEx == null)
                                {
                                    reselement.SetAttribute("ErrorE", "提取写日志失败,位置源数据OID:" + orgOID.ToString());
                                }
                                else
                                {
                                    reselement.SetAttribute("ErrorE", "提取写日志失败,错误原因" + errEx.ToString() + ",位置源数据OID:" + orgOID.ToString());
                                }

                                bInFeat = false;
                                if (bExit == true)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    iCount++;
                    icnt++;
                    if (iCount == 1000)
                    {
                        pNewFeatureCursor.Flush();
                        iCount = 0;
                    }

                    if (!pAppForm.MainForm.IsDisposed && pAppForm.MainForm.InvokeRequired)
                    {
                        pAppForm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { pAppForm.ProgressBar, -1, -1, icnt });
                    }
                    else
                    {
                        pAppForm.ProgressBar.Value = icnt;
                        pAppForm.ProgressBar.Refresh();
                    }
                }
                catch (Exception e)
                {
                    reselement.SetAttribute("success", "0");
                    //创建错误节点
                    XmlElement ErrElement = reselement.OwnerDocument.CreateElement("Error");
                    reselement.AppendChild(ErrElement);
                    ErrElement.SetAttribute("ErrorDescribe", e.ToString());
                    ErrElement.SetAttribute("ErrorOrgOID", orgOID.ToString());
                    bInFeat = false;
                    if (bExit == true)
                    {
                        break;
                    }
                }

                pFeature = pfeacursor.NextFeature();
            }

            pNewFeatureCursor.Flush();

            //  完成此步骤后释放对应的Cursor
            Marshal.ReleaseComObject(pfeacursor);
            Marshal.ReleaseComObject(pNewFeatureCursor);
            if (bInFeat == true)
            {
                reselement.SetAttribute("success", "1");
            }
            else
            {
                reselement.SetAttribute("success", "0");
            }
            return bInFeat;
        }

        /// <summary>
        /// 获取当前版本信息（获取环境中数据库版本表中VERSION的最大值加1做为当前编辑生成版本）
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public int GetVersion(IWorkspace pWS,out Exception ex)
        {
            ex = null;
            int iVersion = -1;
            if (pWS == null) { ex = new Exception("更新环境库尚未初始化"); return -1; }
            try
            {
                ITable getTable = (pWS as IFeatureWorkspace).OpenTable(ModData.m_sDBVersionTable);
                if (getTable.RowCount(null) == 0) return 1;
                else
                {
                    int index = getTable.FindField("VERSION");
                    if (index < 0) { ex = new Exception("数据库版本表中未能找到VERSION字段"); return -1; }
                    ICursor TableCursor = getTable.Search(null, false);
                    //IRow getRow = TableCursor.NextRow();
                    //while (getRow != null)
                    //{
                    //    int getValue =Convert.ToInt32(getRow.get_Value(index));
                    //    if (getValue > iVersion) iVersion = getValue;
                    //    getRow = TableCursor.NextRow();
                    //}
                    IDataStatistics dataStatistics = new DataStatisticsClass();
                    dataStatistics.Field = "VERSION";
                    dataStatistics.Cursor = TableCursor;
                    ESRI.ArcGIS.esriSystem.IStatisticsResults statisticsResults = dataStatistics.Statistics;
                    double getMaxVersion = statisticsResults.Maximum;
                    iVersion = Convert.ToInt32(getMaxVersion);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(TableCursor);
                }
                return iVersion + 1;
            }
            catch (Exception eError)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
                ex = new Exception("获取新数据库版本信息失败。\n原因：" + eError.Message);
                return -1;
            }
        }

        /// <summary>
        /// 记录一个要素的编辑信息
        /// </summary>
        /// <param name="in_Row">编辑要素的IRow</param>
        /// <param name="in_iState">编辑状态：1、新增，2、修改，3、删除</param>
        /// <param name="in_DateTime">编辑的时间</param>
        /// <param name="in_iVersion">版本</param>
        /// <param name="in_sLayerName">要素所在的图层名</param>
        /// <param name="ex"></param>
        private void RecordLOG(IWorkspace pWS,IRow in_Row, int in_iState, DateTime in_DateTime, int in_iVersion, string in_sLayerName,int newOID,out Exception ex)
        {
            ex = null;
            if (in_Row == null) { ex = new Exception("输入要素为空"); return; }
            if (in_DateTime == null) { ex = new Exception("输入的编辑时间为空"); return; }
            IFeature getFea = in_Row as IFeature;
            if (getFea == null) { ex = new Exception("获取要素失败"); return; }
            ///////////////获取必要信息/////////////////
            //////去掉sde图层带的用户名
            if (in_sLayerName.Contains("."))
            {
                in_sLayerName = in_sLayerName.Substring(in_sLayerName.LastIndexOf('.') + 1);
            }
            int iOID = newOID;/////////////////////////////要素OID
            string sLayerName = string.Empty;//////////要素图层名
            int iVersion = -1;/////////////////////////版本信息 
            sLayerName = in_sLayerName;
            iVersion = in_iVersion;/////////////获取版本
            if (ex != null) return;
            //////////////////写入日志////////////////////////////
            try
            {
                ITransactions LOGTran = pWS as ITransactions;
                LOGTran.StartTransaction();
                ////////写入更新日志表///////
                WriteLog(pWS,iOID, sLayerName, iVersion, in_DateTime, in_iState, getFea.Shape.Envelope, out ex);
                if (ex != null) { LOGTran.AbortTransaction(); return; }
                ////////写入数据库版本表/////
                LOGTran.CommitTransaction();
            }
            catch (Exception eError)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
                ex = eError;
                return;
            }
        }

       
        /// <summary>
        /// 写入远程更新日志
        /// </summary>
        /// <param name="in_iOID">更新要素OID</param>
        /// <param name="in_sLayerName">更新要素所在图层名</param>
        /// <param name="in_iVersion">版本</param>
        /// <param name="in_DateTime">更新时间</param>
        /// <param name="in_iState">更新状态：1、新增，2、修改，3、删除</param>
        /// <param name="in_Envelope">更新要素的最小外包矩形</param>
        /// <param name="ex"></param>
        private void WriteLog(IWorkspace pWS,int in_iOID, string in_sLayerName, int in_iVersion, DateTime in_DateTime, int in_iState, IEnvelope in_Envelope, out Exception ex)
        {
            ex = null;
            //////去掉sde图层带的用户名
            if (in_sLayerName.Contains("."))
            {
                in_sLayerName = in_sLayerName.Substring(in_sLayerName.LastIndexOf('.') + 1);
            }
            if (pWS == null) { ex = new Exception("更新环境库连接信息未初始化"); return; };
            string sql = "INSERT INTO " +ModData.m_sUpDataLOGTable + "(OID,STATE,LAYERNAME,LASTUPDATE,VERSION,XMIN,XMAX,YMIN,YMAX) values(";
            //cyf 20110621 modify:
            if (pWS.WorkspaceFactory.WorkspaceType != esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                sql += in_iOID.ToString() + "," + in_iState.ToString() + ",'" + in_sLayerName + "','" + in_DateTime + "'," + in_iVersion.ToString() + ",";
            }
            else
            {
                sql += in_iOID.ToString() + "," + in_iState.ToString() + ",'" + in_sLayerName + "'," + "to_date('" + in_DateTime.ToString("G") + "','yyyy-mm-dd hh24:mi:ss')" + "," + in_iVersion.ToString() + ",";
            }
                //end
            if (in_Envelope != null)
                sql += in_Envelope.XMin.ToString() + "," + in_Envelope.XMax.ToString() + "," + in_Envelope.YMin.ToString() + "," + in_Envelope.YMax.ToString() + ")";
            else
                sql += "NULL,NULL,NULL,NULL)";
            try
            {
                pWS.ExecuteSQL(sql);
            }
            catch (Exception eError)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
                ex = new Exception("写入日志表失败。\n原因：" + eError.Message);
                return;
            }
        }
        

        //  未使用函数[注记转换,扩展属性赋值需进行写代码set_Value]
        private bool ImpAnnoFeatures(string orgFcName, IFeatureClass pfeatureCls, IFeatureCursor PDBfeacursor, XmlElement attrselement, XmlElement reselement, Plugin.Application.IAppFormRef pAppForm, bool bExit)
        {
            IDataset pDs = (IDataset)pfeatureCls;
            IFDOGraphicsLayerFactory pGLF = new FDOGraphicsLayerFactoryClass();
            IFeatureWorkspace pFeatureWs = (IFeatureWorkspace)pDs.Workspace;

            IFDOGraphicsLayer pFDOGLayer = (IFDOGraphicsLayer)pGLF.OpenGraphicsLayer(pFeatureWs, pfeatureCls.FeatureDataset, pDs.Name);
            IFDOAttributeConversion pFDPAtrrCon = (IFDOAttributeConversion)pFDOGLayer;

            if (pFDOGLayer == null || PDBfeacursor == null) return true;
            if (attrselement == null || attrselement.ChildNodes.Count == 0) return true;

            int intELEMENT = pfeatureCls.Fields.FindField("ELEMENT");
            Dictionary<int, int> dicFieldIndexPair = new Dictionary<int, int>();
            Dictionary<int, int> dicConFieldIndexPair = new Dictionary<int, int>();
            foreach (XmlNode xmlnode in attrselement.ChildNodes)
            {
                XmlElement aElement = (XmlElement)xmlnode;

                int PDBIndex = Convert.ToInt32(aElement.GetAttribute("OrgIndex"));
                int SDEIndex = Convert.ToInt32(aElement.GetAttribute("ObjIndex"));

                //将ELEMENT、ANNOTATIONCLASSID、ZORDER除外不然显示会有问题
                if (SDEIndex == intELEMENT) continue;
                //注记特殊属性除外的属性

                IObjectClassDescription pOCDesc = new AnnotationFeatureClassDescription();
                IFields fields = pOCDesc.RequiredFields;
                IFeatureClassDescription pFDesc = pOCDesc as IFeatureClassDescription;
                if (pOCDesc.RequiredFields.FindField(pfeatureCls.Fields.get_Field(SDEIndex).Name) == -1)
                {
                    dicConFieldIndexPair.Add(SDEIndex, PDBIndex);
                    continue;
                }

                dicFieldIndexPair.Add(SDEIndex, PDBIndex);
            }

            //注记特殊属性对应配对

            int index = 0;
            int[] PDBIndexs = new int[dicFieldIndexPair.Count + dicConFieldIndexPair.Count];
            int[] SDEIndexs = new int[dicFieldIndexPair.Count + dicConFieldIndexPair.Count];
            foreach (KeyValuePair<int, int> keyvalue in dicFieldIndexPair)
            {
                PDBIndexs[index] = keyvalue.Value;
                SDEIndexs[index] = keyvalue.Key;

                index = index + 1;
            }

            //注记特殊属性以外的其它用户自定义属性对应配对

            foreach (KeyValuePair<int, int> keyvalue in dicConFieldIndexPair)
            {
                PDBIndexs[index] = keyvalue.Key;
                SDEIndexs[index] = keyvalue.Key;

                index = index + 1;
            }

            pFDOGLayer.BeginAddElements();
            //注记特殊属性对应赋值

            pFDPAtrrCon.SetupAttributeConversion2(dicFieldIndexPair.Count + dicConFieldIndexPair.Count, PDBIndexs, SDEIndexs);

            IFeature pPDBFeature = PDBfeacursor.NextFeature();
            int icnt = 0;
            int orgOID = -1;
            bool bInFeat = true;
            int indexFid = pfeatureCls.Fields.FindField("GOFID");
            while (pPDBFeature != null)
            {
                try
                {
                    orgOID = pPDBFeature.OID;

                    IAnnotationFeature2 pAnnoFeature = (IAnnotationFeature2)pPDBFeature;
                    IClone pAClone = (IClone)pAnnoFeature.Annotation;
                    IGroupSymbolElement pGSElement = (IGroupSymbolElement)pAClone.Clone();

                    IFeatureBuffer pFeatBuffer = pfeatureCls.CreateFeatureBuffer();
                    //注记特殊属性以外的其它用户自定义字段赋值

                    foreach (KeyValuePair<int, int> keyvalue in dicConFieldIndexPair)
                    {
                        if (keyvalue.Value > -1)
                        {
                            pFeatBuffer.set_Value(keyvalue.Key, pPDBFeature.get_Value(keyvalue.Value));
                        }
                    }

                    IFeature pObjFeat = pFeatBuffer as IFeature;
                    pFDOGLayer.DoAddFeature(pObjFeat, pGSElement as IElement, 0);

                    icnt++;
                    if (!pAppForm.MainForm.IsDisposed && pAppForm.MainForm.InvokeRequired)
                    {
                        pAppForm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { pAppForm.ProgressBar, -1, -1, icnt });
                    }
                    else
                    {
                        pAppForm.ProgressBar.Value = icnt;
                        pAppForm.ProgressBar.Refresh();
                    }
                }
                catch (Exception e)
                {
                    //*******************************************************************
                    //Excption Log
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(e, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(e, null, DateTime.Now);
                    }
                    //********************************************************************
                    reselement.SetAttribute("success", "0");
                    //创建错误节点
                    XmlElement ErrElement = reselement.OwnerDocument.CreateElement("Error");
                    reselement.AppendChild(ErrElement);
                    ErrElement.SetAttribute("ErrorDescribe", e.ToString());
                    ErrElement.SetAttribute("ErrorOrgOID", orgOID.ToString());
                    bInFeat = false;
                    if (bExit == true)
                    {
                        break;
                    }
                }
                pPDBFeature = PDBfeacursor.NextFeature();
            }
            pFDOGLayer.EndAddElements();

            //  完成此步骤后释放对应的Cursor
            Marshal.ReleaseComObject(PDBfeacursor);
            if (bInFeat == true)
            {
                reselement.SetAttribute("success", "1");
            }
            else
            {
                reselement.SetAttribute("success", "0");
            }
            return bInFeat;
        }
        #endregion


        private bool FIDLogFeatureOID(int FID, string orgFcName, int OID, out Exception errEx)
        {
            errEx = null;
            if (m_OutDataBase == null) return false;
            string strSQL = "insert into FID记录表(GOFID,FCNAME,OID) values(" + FID.ToString() + ",'" + orgFcName + "'," + OID.ToString() + ")";
            return m_OutDataBase.UpdateTable(strSQL, out errEx);
        }
    }
}
