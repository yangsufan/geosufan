using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data.OleDb;
using System.Collections;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

using System.IO;
using SysCommon.Gis;
namespace GeoDBIntegration
{
    /// <summary>
    /// 集成管理子系统界面   陈亚飞 20100927
    /// </summary>
    public partial class UserControlDBIntegra : UserControl
    {

        //右键菜单集合
        private Dictionary<string, DevComponents.DotNetBar.ContextMenuBar> _dicContextMenu;

        //地图浏览工具栏容器
        private Control _MapToolControl;
        private Control _DBControl;

        private bool m_bGroupLayerVisible;   //控制解决GroupLayer显示问题(方法有待改进 )
        //初始化窗体类
        public UserControlDBIntegra(string strName, string strCation)
        {

            InitializeComponent();
            //********************************************************************************//
            //初始化数据库面板类
            if (null == ModuleData.v_DataBaseProPanel) ModuleData.v_DataBaseProPanel = new clsDatabasePanel(this.groupPanel1);
            //********************************************************************************//
            //**************************************
            //2010-11-22 guozheng added  系统运行日志
            ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
            if (SysCommon.Log.Module.SysLog == null)
            {
                SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Initial();
            }
            SysCommon.Log.Module.SysLog.Write("进入数据库管理子系统", null, DateTime.Now);
            //*************************************
            //初始化配置对应视图控件
            InitialMainViewControl();

            this.Name = strName;
            this.Tag = strCation;
            this.Dock = System.Windows.Forms.DockStyle.Fill;

            axMapControl1.Map.Name = "数据视图";
            axTOCControl1.SetBuddyControl(axMapControl1.Object);
            advTreeProject.ImageList = IconContainer;


            ModuleData.v_AppDBIntegra.MainUserControl = this;
            ModuleData.v_AppDBIntegra.TOCControl = axTOCControl1.Object as ITOCControlDefault;
            ModuleData.v_AppDBIntegra.MapControl = axMapControl1.Object as IMapControlDefault;
            ModuleData.v_AppDBIntegra.ArcGisMapControl = axMapControl1;
            ModuleData.v_AppDBIntegra.ProjectTree = advTreeProject;
            /////*********************************************************
            ////guozheng  2010-10-8
            ModuleData.v_AppDBIntegra.cmbImageDB = this.cmbImageDB;
            ModuleData.v_AppDBIntegra.cmbFileDB = this.cmbFileDB;
            ModuleData.v_AppDBIntegra.cmbFeatureDB = this.cmbFeatureDB;
            ModuleData.v_AppDBIntegra.cmbEntiDB = this.cmbEntiDB;
            ModuleData.v_AppDBIntegra.cmbDemDB = this.cmbDemDB;
            ModuleData.v_AppDBIntegra.cmbAdressDB = this.cmbAdressDB;
            //**********************************************************


            //界面信息
            btnDemDB.Enabled = false;
            btnEntiDB.Enabled = false;
            btnFeaDB.Enabled = false;
            btnFileDB.Enabled = false;
            btnImageDB.Enabled = false;
            btnMapDB.Enabled = false;
            btnSubjectDB.Enabled = false;
            bnAddressDB.Enabled = false;


            //初始化框架插件控件界面
            InitialFrmDefControl();

            //清空本地xml文件，方便后面读取数据库刷新界面xml文件
            if (File.Exists(ModuleData.v_feaProjectXML))
            {
                //框架要素库子系统
                File.Delete(ModuleData.v_feaProjectXML);
            }
            if (File.Exists(ModuleData.v_ImageProjectXml))
            {
                //高程数据库子系统
                File.Delete(ModuleData.v_ImageProjectXml);
            }
            if (File.Exists(ModuleData.v_DEMProjectXml))
            {
                //影像数据库子系统
                File.Delete(ModuleData.v_DEMProjectXml);
            }


            //读取系统维护库中的所有数据库信息挂接到界面上
            //cyf 20110602 modify:修改系统维护库连接信息
            Exception ex = null;
            if (File.Exists(ModuleData.v_ConfigPath))
            {
                SysCommon.Gis.SysGisDB vgisDb = new SysCommon.Gis.SysGisDB();
                //获得系统维护库连接信息
                SysCommon.Authorize.AuthorizeClass.GetConnectInfo(ModuleData.v_ConfigPath, out ModuleData.Server, out ModuleData.Instance, out ModuleData.Database, out ModuleData.User, out ModuleData.Password, out ModuleData.Version, out ModuleData.dbType);
                //进行系统维护库连接
                bool blnCanConnect = CanOpenConnect(vgisDb, ModuleData.dbType, ModuleData.Server, ModuleData.Instance, ModuleData.Database, ModuleData.User, ModuleData.Password, ModuleData.Version);
                if (!blnCanConnect)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统维护库连接失败，请重新配置！");
                    return;
                }
                ModuleData.TempWks = vgisDb.WorkSpace;

                clsAddAppDBConnection addAppDB = new clsAddAppDBConnection();
                //判断系统维护库的连接信息是否正确
                addAppDB.JudgeAppDbConfiguration(out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "界面初始化化失败，\n原因:" + ex.Message);
                    return;
                }
                #region cyf 20110627 add:初始化工程树图
                IFeatureWorkspace pFeaWS = ModuleData.TempWks as IFeatureWorkspace;
                if (pFeaWS == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库失败！");
                    return;
                }
                string pTableName = "DATABASETYPEMD";
                string pFieldNames = "DATABASETYPE";
                ICursor pCursor = ModDBOperate.GetCursor(pFeaWS, pTableName, pFieldNames, "", out ex);
                if (ex != null || pCursor == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询数据库系统维护库中的数据库类型表失败！");
                    return;
                }
                IRow pRow = pCursor.NextRow();
                //遍历行，将树节点加载在树图上
                while (pRow != null)
                {
                    string pDBType = pRow.get_Value(0).ToString();  //数据库类型
                    DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                    pNode.Expanded = true;
                    pNode.Name = "node2";
                    pNode.TagString = "Database";
                    pNode.Text = pDBType;
                    pNode.Image = IconContainer.Images[1];  //cyf 20110711 添加图标
                    this.node1.Nodes.Add(pNode);
                    pRow = pCursor.NextRow();
                }
                //释放游标
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);   //cyf 20110713 add
                this.advTreeProject.Refresh();
                #endregion

                //刷新界面
                while (!addAppDB.refurbish(out ex))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "界面初始化化失败，\n原因:" + ex.Message);
                    return; 
                }
            }
            else
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统维护库连接配置文件不存在，请检查！");
                return;
            }
            #region 原有代码
            //*******************************************************//
            //guozheng 2010-09-28 获取系统维护库连接信息，
            //读取系统维护库中的所有数据库信息挂接到界面上
            //Exception ex = null;
            //clsAddAppDBConnection addAppDB = new clsAddAppDBConnection();
            //string sConnect = addAppDB.GetAppDBConInfo(out ex);
            //if (string.IsNullOrEmpty(sConnect))
            //{
            //    sConnect = addAppDB.SetAppDBConInfo(out ex);
            //}
            //if (!string.IsNullOrEmpty(sConnect))
            //{
            //    //判断系统维护库的连接信息是否正确
            //addAppDB.JudgeAppDbConfiguration(sConnect, out ex);
            //    if (ex != null)
            //    {
            //        if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "系统维护库库体结构错误：" + ex.Message + ",\n是否重新配置系统维护库连接信息？"))
            //        {
            //            sConnect = addAppDB.SetAppDBConInfo(out ex);
            //        }
            //        else
            //            return;
            //    }

            //    while (!addAppDB.refurbish(sConnect, out ex))
            //    {

            //        if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "界面初始化化失败，\n原因:" + ex.Message + ",\n是否重新配置系统维护库连接信息？"))
            //        {
            //            sConnect = addAppDB.SetAppDBConInfo(out ex);
            //            /////将连接字符串记录下来
            //            ModuleData.v_AppConnStr = sConnect;
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //    /////将连接字符串记录下来
            //    ModuleData.v_AppConnStr = sConnect;
            //}

            //******************************************************//
            #endregion
        }
        //yjl20120808 add 开放初始化工程视图，当新增数据库类型后 需要初始化工程树图
        public void InitProjectTree()
        {
            Exception ex;
            clsAddAppDBConnection addAppDB = new clsAddAppDBConnection();
            //判断系统维护库的连接信息是否正确
            addAppDB.JudgeAppDbConfiguration(out ex);
            if (ex != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "界面初始化化失败，\n原因:" + ex.Message);
                return;
            }
            #region cyf 20110627 add:初始化工程树图
            
            IFeatureWorkspace pFeaWS = ModuleData.TempWks as IFeatureWorkspace;
            if (pFeaWS == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库失败！");
                return;
            }
            string pTableName = "DATABASETYPEMD";
            string pFieldNames = "DATABASETYPE";
            ICursor pCursor = ModDBOperate.GetCursor(pFeaWS, pTableName, pFieldNames, "", out ex);
            if (ex != null || pCursor == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询数据库系统维护库中的数据库类型表失败！");
                return;
            }
            this.node1.Nodes.Clear();
            IRow pRow = pCursor.NextRow();
            //遍历行，将树节点加载在树图上
            while (pRow != null)
            {
                string pDBType = pRow.get_Value(0).ToString();  //数据库类型
                DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                pNode.Expanded = true;
                pNode.Name = "node2";
                pNode.TagString = "Database";
                pNode.Text = pDBType;
                pNode.Image = IconContainer.Images[1];  //cyf 20110711 添加图标
                this.node1.Nodes.Add(pNode);
                pRow = pCursor.NextRow();
            }
            //释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);   //cyf 20110713 add
            this.advTreeProject.Refresh();
            #endregion
            //刷新界面
            while (!addAppDB.refurbish(out ex))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "界面初始化化失败，\n原因:" + ex.Message);
                return;
            }

        }
        //测试链接信息是否可用
        public static bool CanOpenConnect(SysCommon.Gis.SysGisDB vgisDb, string strType, string strServer, string strService, string strDatabase, string strUser, string strPassword, string strVersion)
        {
            bool blnOpen = false;

            Exception Err;

            if (strType.ToUpper() == "ORACLE" || strType.ToUpper() == "SQLSERVER")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, strService, strDatabase, strUser, strPassword, strVersion, out Err);
            }
            else if (strType.ToUpper() == "ACCESS")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.PDB, out Err);
            }
            else if (strType.ToUpper() == "FILE")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.GDB, out Err);
            }

            return blnOpen;

        }
        //初始化配置对应视图控件
        private void InitialMainViewControl()
        {
            frmBarManager newfrmBarManager = new frmBarManager();
            newfrmBarManager.TopLevel = false;
            newfrmBarManager.Dock = DockStyle.Fill;
            newfrmBarManager.Show();
            this.Controls.Add(newfrmBarManager);

            //加载设置控制树图
            DevComponents.DotNetBar.Bar barTree = newfrmBarManager.CreateBar("barTree", enumLayType.FILL);
            barTree.CanHide = false;
            barTree.CanAutoHide = true;
            DevComponents.DotNetBar.PanelDockContainer PanelTree = newfrmBarManager.CreatePanelDockContainer("PanelTree", barTree);
            DockContainerItem TreeContainerItem = newfrmBarManager.CreateDockContainerItem("TreeContainerItem", "数据管理", PanelTree, barTree);
            PanelTree.Controls.Add(this.advTreeProject);//.tabTreeControl
            this.advTreeProject.Dock = DockStyle.Fill;

            //加载图层显示  cyf 20110711 modify 
            //DevComponents.DotNetBar.PanelDockContainer PanelTree1 = newfrmBarManager.CreatePanelDockContainer("PanelTree1", barTree);
            //DockContainerItem TreeContainerItem1 = newfrmBarManager.CreateDockContainerItem("TreeContainerItem1", "图层显示", PanelTree1, barTree);
            //PanelTree1.Controls.Add(this.axTOCControl1);//.tabTreeControl
            //this.axTOCControl1.Dock = DockStyle.Fill;
            //end

            //加载设置数据库空间面板
            DevComponents.DotNetBar.Bar barMap = newfrmBarManager.CreateBar("barMap", enumLayType.FILL);
            barMap.CanHide = false;
            DevComponents.DotNetBar.PanelDockContainer PanelMap = newfrmBarManager.CreatePanelDockContainer("PanelMap", barMap);
            DockContainerItem MapContainerItem = newfrmBarManager.CreateDockContainerItem("MapContainerItem", "数据库面板", PanelMap, barMap);
            PanelMap.Controls.Add(this.groupPanel1);//.tabMapControl
            this.groupPanel1.Dock = DockStyle.Fill;
            _DBControl = PanelMap as Control;
            //加载数据视图  cyf 20110711 modify
            //DevComponents.DotNetBar.PanelDockContainer PanelMap1 = newfrmBarManager.CreatePanelDockContainer("PanelMap1", barMap);
            //DockContainerItem MapContainerItem1 = newfrmBarManager.CreateDockContainerItem("MapContainerItem", "图层面板", PanelMap1, barMap);
            //PanelMap1.Controls.Add(this.axMapControl1);//.tabMapControl
            //this.axMapControl1.Dock = DockStyle.Fill;
            //_MapToolControl = PanelMap1 as Control;
            //end

            //布局设置
            newfrmBarManager.MainDotNetBarManager.FillDockSite.GetDocumentUIManager().Dock(barTree, barMap, eDockSide.Right);
            newfrmBarManager.MainDotNetBarManager.FillDockSite.GetDocumentUIManager().SetBarWidth(barTree, this.Width / 5);

            //cyf 20110610 modify:屏蔽掉相关的窗体控件
            //加载设置提示窗体
            //连接信息
            PanelDockContainer PanelTipConn = new PanelDockContainer();
            PanelTipConn.Controls.Add(this.dgConnecInfo);
            this.dgConnecInfo.Dock = DockStyle.Fill;
            DockContainerItem dockItemConn = new DockContainerItem("dockItemConn", "数据库运行状态");
            dockItemConn.Control = PanelTipConn;
            //dockItemConn.Visible = false;
            newfrmBarManager.ButtomBar.Items.Add(dockItemConn);

            //参数信息显示
            PanelDockContainer PanelTipPara = new PanelDockContainer();
            PanelTipPara.Controls.Add(this.dgParaInfo);
            this.dgParaInfo.Dock = DockStyle.Fill;
            DockContainerItem dockItemPara = new DockContainerItem("dockItemPara", "参数信息");
            dockItemPara.Control = PanelTipPara;
            newfrmBarManager.ButtomBar.Items.Add(dockItemPara);

            ////查询结果显示
            //PanelDockContainer PanelTipQuery = new PanelDockContainer();
            //PanelTipQuery.Controls.Add(this.dgQueryRes);
            //this.dgQueryRes.Dock = DockStyle.Fill;
            //DockContainerItem dockItemQuery = new DockContainerItem("dockItemQuery", "查询结果");
            //dockItemQuery.Control = PanelTipQuery;
            //newfrmBarManager.ButtomBar.Items.Add(dockItemQuery);
            //end
        }

        //初始化框架插件控件界面
        private void InitialFrmDefControl()
        {
            //得到Xml的System节点,根据XML加载插件界面
            string xPath = ".//System[@Name='" + this.Name + "']";
            Plugin.ModuleCommon.LoadButtonViewByXmlNode(ModuleData.v_AppDBIntegra.ControlContainer, xPath, ModuleData.v_AppDBIntegra);

            _dicContextMenu = ModuleData.v_AppDBIntegra.DicContextMenu;

            ////初始化地图浏览工具栏
            Plugin.Application.IAppFormRef pAppFrm = ModuleData.v_AppDBIntegra as Plugin.Application.IAppFormRef;
            XmlNode barXmlNode = pAppFrm.SystemXml.SelectSingleNode(".//ToolBar[@Name='ControlMapToolBar4']");
            if (barXmlNode == null || _MapToolControl == null) return;
            DevComponents.DotNetBar.Bar mapToolBar = Plugin.ModuleCommon.LoadButtonView(_MapToolControl, barXmlNode, pAppFrm, null) as Bar;
            if (mapToolBar != null)
            {
                mapToolBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
                mapToolBar.DockOrientation = DevComponents.DotNetBar.eOrientation.Horizontal;//.Vertical;
                mapToolBar.DockSide = DevComponents.DotNetBar.eDockSide.Top;//.Left;
                mapToolBar.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
                mapToolBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
                _MapToolControl.Controls.Remove(mapToolBar);
                _MapToolControl.Controls.Add(mapToolBar);
            }

            foreach (KeyValuePair<string, DevComponents.DotNetBar.ContextMenuBar> keyValue in _dicContextMenu)
            {
                this.Controls.Add(keyValue.Value);
            }
        }


        //图层控制右键菜单
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            IBasicMap pMap = null;
            ILayer pLayer = null;
            System.Object other = null;
            System.Object LayerIndex = null;
            System.Drawing.Point pPoint = new System.Drawing.Point(e.x, e.y);

            esriTOCControlItem TOCItem = esriTOCControlItem.esriTOCControlItemNone;
            ITOCControl2 tocControl = (ITOCControl2)axTOCControl1.Object;

            tocControl.HitTest(e.x, e.y, ref TOCItem, ref pMap, ref pLayer, ref other, ref LayerIndex);

            if (e.button == 1)
            {
                //控制解决GroupLayer显示问题  
                if (TOCItem == esriTOCControlItem.esriTOCControlItemLayer && pLayer is IGroupLayer)
                {
                    if (pLayer.Visible)
                    {
                        m_bGroupLayerVisible = false;
                    }
                    else
                    {
                        m_bGroupLayerVisible = true;
                    }
                }
                return;
            }

            if (e.button == 2 && _dicContextMenu != null)
            {
                DevComponents.DotNetBar.ButtonItem item = null;
                switch (TOCItem)
                {
                    case esriTOCControlItem.esriTOCControlItemMap:
                        if (_dicContextMenu.ContainsKey("TOCControlContextMenu4"))
                        {
                            if (_dicContextMenu["TOCControlContextMenu4"] != null)
                            {
                                if (_dicContextMenu["TOCControlContextMenu4"].Items.Count > 0)
                                {
                                    item = _dicContextMenu["TOCControlContextMenu4"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                                    if (item != null)
                                    {
                                        item.Popup(axTOCControl1.PointToScreen(pPoint));
                                    }
                                }
                            }
                        }
                        break;
                    case esriTOCControlItem.esriTOCControlItemLayer:
                        {
                            if (!(pLayer is IGroupLayer || pLayer is IFeatureLayer || pLayer is IDataLayer || pLayer is IDynamicLayer)) return;
                            if (_dicContextMenu.ContainsKey("TOCControlLayerContextMenu4"))
                            {
                                if (_dicContextMenu["TOCControlLayerContextMenu4"] != null)
                                {
                                    if (_dicContextMenu["TOCControlLayerContextMenu4"].Items.Count > 0)
                                    {
                                        item = _dicContextMenu["TOCControlLayerContextMenu4"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                                        if (item != null)
                                        {
                                            item.Popup(axTOCControl1.PointToScreen(pPoint));
                                        }
                                    }
                                }
                            }

                            if (axTOCControl1.Buddy is IPageLayoutControl2)
                            {
                                IPageLayoutControl2 pPageLayoutControl = axTOCControl1.Buddy as IPageLayoutControl2;
                                pPageLayoutControl.CustomProperty = pLayer;

                            }

                            else if (axTOCControl1.Buddy is IMapControl3)
                            {
                                IMapControl3 pMapcontrol = axTOCControl1.Buddy as IMapControl3;
                                pMapcontrol.CustomProperty = pLayer;
                            }

                        }

                        break;
                }
            }
        }

        //图层浏览右键菜单
        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            //timerShow.Enabled = false;  //进行操作时，操作历史查询不可用

            if (e.button == 1 || _dicContextMenu == null)
                return;
            System.Drawing.Point pPoint = new System.Drawing.Point(e.x, e.y);
            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("MapControlContextMenu4"))
            {
                if (_dicContextMenu["MapControlContextMenu4"].Items.Count > 0)
                {
                    item = _dicContextMenu["MapControlContextMenu4"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        item.Popup(axMapControl1.PointToScreen(pPoint));
                    }
                }
            }
        }

        //利用此事件结合局部变量m_bGroupLayerVisible控制解决GroupLayer显示问题
        private void axTOCControl1_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        {
            IBasicMap pMap = null;
            ILayer pLayer = null;
            System.Object other = null;
            System.Object LayerIndex = null;
            System.Drawing.Point pPoint = new System.Drawing.Point(e.x, e.y);

            esriTOCControlItem TOCItem = esriTOCControlItem.esriTOCControlItemNone;
            ITOCControl2 tocControl = (ITOCControl2)axTOCControl1.Object;

            tocControl.HitTest(e.x, e.y, ref TOCItem, ref pMap, ref pLayer, ref other, ref LayerIndex);
            if (e.button == 1)
            {
                if (TOCItem == esriTOCControlItem.esriTOCControlItemLayer && pLayer is IGroupLayer)
                {
                    if (m_bGroupLayerVisible != pLayer.Visible)
                    {
                        IMapControlDefault pMapcontrol = axTOCControl1.Buddy as IMapControlDefault;
                        pMapcontrol.ActiveView.Refresh();
                    }
                }
            }
        }

        private void axMapControl1_OnAfterDraw(object sender, IMapControlEvents2_OnAfterDrawEvent e)
        {
            try
            {
                ModuleData.v_AppDBIntegra.RefScaleCmb.ControlText = (sender as AxMapControl).Map.ReferenceScale.ToString().Trim();
                ModuleData.v_AppDBIntegra.CurScaleCmb.ControlText = (sender as AxMapControl).Map.MapScale.ToString().Trim();
                ModuleData.v_AppDBIntegra.CurScaleCmb.Tooltip = axMapControl1.Map.MapScale.ToString().Trim();
            }
            catch
            {
            }
        }

        private void axMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            //ModData.v_AppSMPD.CoorTxt.ControlText = "X:" + e.mapX + ";Y:" + e.mapY;
            ModuleData.v_AppDBIntegra.OperatorTips = "X:" + e.mapX + ";Y:" + e.mapY;
        }

        /// <summary>
        /// 将数据库ID写入xml中  陈亚飞添加20100930
        /// </summary>
        /// <param name="pDBID">数据库ID</param>
        private void SaveIDToXml(string pDBID, string xmlCur, string xmlTemp)
        {
            try
            {
                Convert.ToInt32(pDBID);
                if (!File.Exists(xmlCur))
                {
                    File.Copy(xmlTemp, xmlCur);
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlCur);
                XmlElement pElem = xmlDoc.SelectSingleNode(".//工程管理") as XmlElement;
                pElem.SetAttribute("当前库体编号", pDBID);
                xmlDoc.Save(xmlCur);
            }
            catch (Exception eError)
            {
                //****************************************************
                if (ModuleData.v_SysLog != null)
                    ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                //****************************************************
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请先创建数据库连接工程");
                return;
            }

        }

        private void cmbFileDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //文件库界面
            //将当前数据库ID写入xml中
            if (cmbFileDB.SelectedValue != null)
            {
                Exception ex = null;
                string pDBID = cmbFileDB.SelectedValue.ToString();
                string sDBName = cmbFileDB.Text.Trim();
                if (pDBID == "System.Data.DataRowView" || sDBName == "System.Data.DataRowView") return;
                clsFTPOper FTPOper = new clsFTPOper();
                FTPOper.SaveProjectXML(pDBID, sDBName, out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                    return;
                }
            }

        }

        private void cmbFeatureDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //框架要素界面
            //将当前数据库ID写入xml中
            if (cmbFeatureDB.SelectedValue != null)
            {
                string pDBID = cmbFeatureDB.SelectedValue.ToString();//当前要启动的工程ID
                //将当前数据库ID写入xml中
                SaveIDToXml(pDBID, ModuleData.v_feaProjectXML, ModuleData.v_feaProjectXMLTemp);

            }
        }

        private void cmbImageDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //影像数据库界面
            //将当前数据库ID写入xml中
            if (cmbImageDB.SelectedValue != null)
            {
                string pDBID = cmbImageDB.SelectedValue.ToString();
                SaveIDToXml(pDBID, ModuleData.v_ImageProjectXml, ModuleData.v_ImageProjectXmlTemp);
            }
        }

        private void cmbDemDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //高程数据库界面
            //将当前数据库ID写入xml中
            if (cmbDemDB.SelectedValue != null)
            {
                string pDBID = cmbDemDB.SelectedValue.ToString();
                SaveIDToXml(pDBID, ModuleData.v_DEMProjectXml, ModuleData.v_DEMProjectXmlTemp);
            }
        }

        private void cmbSubjectDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //专题数据库界面
            //将当前数据库ID写入xml中
            if (cmbSubjectDB.SelectedValue != null)
            {
                string pDBID = cmbSubjectDB.SelectedValue.ToString();
                //SaveIDToXml(pDBID);
            }
        }

        private void cmbAdressDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //地名数据库界面
            //将当前数据库ID写入xml中
            if (cmbAdressDB.SelectedValue != null)
            {
                string pDBID = cmbAdressDB.SelectedValue.ToString();
                //SaveIDToXml(pDBID);
            }
        }

        private void cmbEntiDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //地理实体数据库界面
            //将当前数据库ID写入xml中
            if (cmbEntiDB.SelectedValue != null)
            {
                string pDBID = cmbEntiDB.SelectedValue.ToString();
                //SaveIDToXml(pDBID);
            }
        }

        private void cmbMapDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //电子地图数据库界面
            //将当前数据库ID写入xml中
            if (cmbMapDB.SelectedValue != null)
            {
                string pDBID = cmbMapDB.SelectedValue.ToString();
                //SaveIDToXml(pDBID);
            }
        }

        private void btnFeaDB_Click(object sender, EventArgs e)
        {

            if (cmbFeatureDB.SelectedValue == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请先添加数据库连接工程，并创建数据库！");
                return;
            }

            string pDBID = "";  //当前要启动的工程ID
            pDBID = cmbFeatureDB.SelectedValue.ToString();

            //将当前数据库ID写入xml中
            SaveIDToXml(pDBID, ModuleData.v_feaProjectXML, ModuleData.v_feaProjectXMLTemp);

            for (int i = 0; i < advTreeProject.SelectedNode.Nodes.Count; i++)
            {
                if (advTreeProject.SelectedNode.Nodes[i].DataKey.ToString() != pDBID) continue;

                //==============================================================================================================================================
                //chenyafei  modify 20100215 解决系统插件加载的问题
                //
                //
                string pSysName = "";   //子系统名称
                string pSysCaption = ""; //子系统标题

                XmlElement pElem = advTreeProject.SelectedNode.Nodes[i].Tag as XmlElement;  //数据库平台节点
                string ptStr = pElem.GetAttribute("数据库平台");   //数据库平台信息
                if (ptStr == enumInterDBFormat.ARCGISGDB.ToString() || ptStr == enumInterDBFormat.ARCGISPDB.ToString() || ptStr == enumInterDBFormat.ARCGISSDE.ToString())
                {
                    //启动ArcGIs平台
                    pSysName = "GeoDBATool.ControlDBATool";    //Name
                }
                else if (ptStr == enumInterDBFormat.GEOSTARACCESS.ToString() || ptStr == enumInterDBFormat.GEOSTARORACLE.ToString() || ptStr == enumInterDBFormat.GEOSTARORSQLSERVER.ToString())
                {
                    //启动Geostar平台
                    pSysName = "GeoStarTest.ControlTest";       //Name
                }
                else if (ptStr == enumInterDBFormat.ORACLESPATIAL.ToString())
                {
                    //启动oraclespatial平台
                    pSysName = "OracleSpatialDBTool.ControlOracleSpatialDBTool";    //Name
                }

                //根据Name获得子系统的caption
                XmlDocument sysXml = new XmlDocument();
                sysXml.Load(ModuleData.m_SysXmlPath);
                XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
                if (sysNode == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                    return;
                }
                pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

                //进入子系统界面
                ModDBOperate.InitialForm(pSysName, pSysCaption);

                //===================================================================================================================================================


                //*********************************************************************
                //guozheng added enter feature Db Log
                if (ModuleData.v_SysLog != null)
                {
                    List<string> Pra = new List<string>();
                    Pra.Add(pElem.GetAttribute("数据库工程名"));
                    Pra.Add(pElem.GetAttribute("数据库平台"));
                    Pra.Add(pElem.GetAttribute("数据库类型"));
                    Pra.Add(pElem.GetAttribute("数据库连接信息"));
                    ModuleData.v_SysLog.Write("进入框架要素库", Pra, DateTime.Now);
                }
                //*********************************************************************
                break;
            }
        }

        private void btnFileDB_Click(object sender, EventArgs e)
        {
            //将当前数据库ID写入xml中
            if (cmbFileDB.SelectedValue != null)
            {
                Exception ex = null;
                string pDBID = cmbFileDB.SelectedValue.ToString();
                string sDBName = cmbFileDB.Text.Trim();
                if (pDBID == "System.Data.DataRowView" || sDBName == "System.Data.DataRowView") return;
                clsFTPOper FTPOper = new clsFTPOper();
                FTPOper.SaveProjectXML(pDBID, sDBName, out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                    return;
                }
            }

            //进入文件库界面
            //==================================================================================
            //  chenayfei  modify 20110215  进入子系统界面修改
            //
            //
            string pSysName = "";   //子系统名称
            string pSysCaption = ""; //子系统标题
            //根据Name获得子系统的caption

            pSysName = "FileDBTool.ControlFileDBTool";    //Name

            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModuleData.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

            //进入子系统界面
            ModDBOperate.InitialForm(pSysName, pSysCaption);

            //========================================================================

        }

        private void btnSubjectDB_Click(object sender, EventArgs e)
        {
            //将当前数据库ID写入xml中
            if (cmbSubjectDB.SelectedValue != null)
            {
                string pDBID = cmbSubjectDB.SelectedValue.ToString();
                //SaveIDToXml(pDBID);
            }

            //进入专题数据库界面

            //Form pForm = (ModuleData.v_AppDBIntegra as Plugin.Application.IAppFormRef).MainForm;
            //pForm.Controls.Clear();
            //ModDBOperate.IntialSysFrm(ModuleData.m_SubjectSysXmlPath, pForm);
        }

        private void bnAddressDB_Click(object sender, EventArgs e)
        {
            //将当前数据库ID写入xml中
            if (cmbAdressDB.SelectedValue != null)
            {
                string pDBID = cmbAdressDB.SelectedValue.ToString();
                //SaveIDToXml(pDBID);
            }

            //地名数据库界面
            //==================================================================================
            //  chenayfei  modify 20110215  进入子系统界面修改
            //
            //
            string pSysName = "";   //子系统名称
            string pSysCaption = ""; //子系统标题
            //根据Name获得子系统的caption

            pSysName = "GeoDBAddress.ControlDBAddressTool";    //Name

            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModuleData.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

            //进入子系统界面
            //ModDBOperate.InitialForm(pSysName, pSysCaption);

            //========================================================================
        }

        private void btnImageDB_Click(object sender, EventArgs e)
        {
            //将当前数据库ID写入xml中
            if (cmbImageDB.SelectedValue != null)
            {
                string pDBID = cmbImageDB.SelectedValue.ToString();
                SaveIDToXml(pDBID, ModuleData.v_ImageProjectXml, ModuleData.v_ImageProjectXmlTemp);
            }

            //影像数据库界面
            //==================================================================================
            //  chenayfei  modify 20110215  进入子系统界面修改
            //
            //
            string pSysName = "";   //子系统名称
            string pSysCaption = ""; //子系统标题
            //根据Name获得子系统的caption

            pSysName = "GeoDBATool.ControlDBATool";    //Name

            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModuleData.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

            //进入子系统界面
            ModDBOperate.InitialForm(pSysName, pSysCaption);
            //==========================================================================================
        }

        private void btnEntiDB_Click(object sender, EventArgs e)
        {
            //将当前数据库ID写入xml中
            if (cmbEntiDB.SelectedValue != null)
            {
                string pDBID = cmbEntiDB.SelectedValue.ToString();
                //SaveIDToXml(pDBID);
            }

            //进入地理实体数据库界面
            //==================================================================================
            //  chenayfei  modify 20110215  进入子系统界面修改
            //
            //
            string pSysName = "";   //子系统名称
            string pSysCaption = ""; //子系统标题
            //根据Name获得子系统的caption

            pSysName = "GeoDBEntity.ControlDBEntityTool";    //Name

            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModuleData.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

            //进入子系统界面
            //ModDBOperate.InitialForm(pSysName, pSysCaption);

            //========================================================================
        }

        private void btnDemDB_Click(object sender, EventArgs e)
        {
            //将当前数据库ID写入xml中
            if (cmbDemDB.SelectedValue != null)
            {
                string pDBID = cmbDemDB.SelectedValue.ToString();
                SaveIDToXml(pDBID, ModuleData.v_DEMProjectXml, ModuleData.v_DEMProjectXmlTemp);
            }

            //高程数据库界面
            //==================================================================================
            //  chenayfei  modify 20110215  进入子系统界面修改
            //
            //
            string pSysName = "";   //子系统名称
            string pSysCaption = ""; //子系统标题
            //根据Name获得子系统的caption

            pSysName = "GeoDBATool.ControlDBATool";    //Name

            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModuleData.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

            //进入子系统界面
            ModDBOperate.InitialForm(pSysName, pSysCaption);
            //==========================================================================================

        }

        private void btnMapDB_Click(object sender, EventArgs e)
        {
            //进入电子地图数据库界面
            //Form pForm = (ModuleData.v_AppDBIntegra as Plugin.Application.IAppFormRef).MainForm;
            //pForm.Controls.Clear();
            //ModDBOperate.IntialSysFrm(ModuleData.m_MapSysXmlPath, pForm);
        }

        private void advTreeProject_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            DevComponents.AdvTree.AdvTree aTree = sender as DevComponents.AdvTree.AdvTree;
            if (aTree.SelectedNode == null) return;
            if (aTree.SelectedNode.Tag == null) return;
            try
            {
                XmlElement DbInfoEle = aTree.SelectedNode.Tag as XmlElement;
                //string sDataBaseType = DbInfoEle.GetAttribute("数据库类型"); if (string.IsNullOrEmpty(sDataBaseType)) return;
                
                /********增加参数信息 xisheng 20111103***************************/
                dgParaInfo.DataSource = null;
                if (DbInfoEle != null)
                {
                    DataTable dt = new DataTable();
                    for (int i = 0; i < DbInfoEle.Attributes.Count; i++)
                    {
                        if (DbInfoEle.Attributes[i].Name.EndsWith("ID")) continue;
                        if (DbInfoEle.Attributes[i].Name == "数据库连接信息") continue;
                        if (DbInfoEle.Attributes[i].Name == "数据库参数") continue;
                        dt.Columns.Add(DbInfoEle.Attributes[i].Name);
                    }
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        row[i] = DbInfoEle.GetAttribute(dt.Columns[i].ColumnName);
                    }

                    string[] arrConect = DbInfoEle.GetAttribute("数据库连接信息").Split('|');
                    if (arrConect[0] == "")
                    {
                        dt.Columns.Add("数据库");
                        row["数据库"] = arrConect[2];
                        dt.Columns.Add("数据集");
                        row["数据集"] = arrConect[6];
                    }
                    else
                    {
                        dt.Columns.Add("服务器");
                        row["服务器"] = arrConect[0];
                        dt.Columns.Add("服务端口");
                        row["服务端口"] = arrConect[1];
                        dt.Columns.Add("数据库");
                        row["数据库"] = arrConect[2];
                        dt.Columns.Add("用户名");
                        row["用户名"] = arrConect[3];
                        //*******************参数信息不显示密码 xisheng 20111117***********//
                        //dt.Columns.Add("密码");
                        //row["密码"] = arrConect[4];
                        //end*******************参数信息不显示密码 xisheng 20111117***********//
                        dt.Columns.Add("版本");
                        row["版本"] = arrConect[5];
                        dt.Columns.Add("数据集");
                        row["数据集"] = arrConect[6];
                    }

                    dt.Rows.Add(row);
                    dgParaInfo.DataSource = dt;
                    dgParaInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dgParaInfo.AllowUserToAddRows = false;
                    dgParaInfo.ReadOnly = true;
                    /******** end 增加参数信息 xisheng 20111103***************************/
                }
                //long lDBID = -1; lDBID = (long)aTree.SelectedNode.DataKey; if (lDBID < 0) return;
                ModuleData.v_DataBaseProPanel.SelectButton(aTree.SelectedNode);
            }
            catch
            {
            }
            //if (aTree.SelectedNode.Tag.ToString() == "Database")
            //{
            //    if (aTree.SelectedNode.Text == enumInterDBType.框架要素数据库.ToString())
            //    {
            //        btnFeaDB.Enabled = true;
            //        btnDemDB.Enabled = false;
            //        btnEntiDB.Enabled = false;
            //        btnFileDB.Enabled = false;
            //        btnImageDB.Enabled = false;
            //        btnMapDB.Enabled = false;
            //        btnSubjectDB.Enabled = false;
            //        bnAddressDB.Enabled = false;
            //    }
            //    if (aTree.SelectedNode.Text == enumInterDBType.高程数据库.ToString())
            //    {
            //        btnDemDB.Enabled = true;
            //        btnEntiDB.Enabled = false;
            //        btnFeaDB.Enabled = false;
            //        btnFileDB.Enabled = false;
            //        btnImageDB.Enabled = false;
            //        btnMapDB.Enabled = false;
            //        btnSubjectDB.Enabled = false;
            //        bnAddressDB.Enabled = false;
            //    }
            //    if (aTree.SelectedNode.Text == enumInterDBType.影像数据库.ToString())
            //    {
            //        btnImageDB.Enabled = true;
            //        btnDemDB.Enabled = false;
            //        btnEntiDB.Enabled = false;
            //        btnFeaDB.Enabled = false;
            //        btnFileDB.Enabled = false;
            //        btnMapDB.Enabled = false;
            //        btnSubjectDB.Enabled = false;
            //        bnAddressDB.Enabled = false;
            //    }
            //    if (aTree.SelectedNode.Text == enumInterDBType.成果文件数据库.ToString())
            //    {
            //        btnFileDB.Enabled = true;
            //        btnDemDB.Enabled = false;
            //        btnEntiDB.Enabled = false;
            //        btnFeaDB.Enabled = false;
            //        btnImageDB.Enabled = false;
            //        btnMapDB.Enabled = false;
            //        btnSubjectDB.Enabled = false;
            //        bnAddressDB.Enabled = false;
            //    }
            //    if (aTree.SelectedNode.Text == enumInterDBType.地名数据库.ToString())
            //    {
            //        bnAddressDB.Enabled = true;
            //        btnDemDB.Enabled = false;
            //        btnEntiDB.Enabled = false;
            //        btnFeaDB.Enabled = false;
            //        btnFileDB.Enabled = false;
            //        btnImageDB.Enabled = false;
            //        btnMapDB.Enabled = false;
            //        btnSubjectDB.Enabled = false;
            //    }
            //    if (aTree.SelectedNode.Text == enumInterDBType.地理编码数据库.ToString())
            //    {
            //        btnEntiDB.Enabled = true;
            //        btnDemDB.Enabled = false;
            //        btnFeaDB.Enabled = false;
            //        btnFileDB.Enabled = false;
            //        btnImageDB.Enabled = false;
            //        btnMapDB.Enabled = false;
            //        btnSubjectDB.Enabled = false;
            //        bnAddressDB.Enabled = false;
            //    }
            //    if (aTree.SelectedNode.Text == enumInterDBType.专题要素数据库.ToString())
            //    {
            //        btnSubjectDB.Enabled = true;
            //        btnDemDB.Enabled = false;
            //        btnEntiDB.Enabled = false;
            //        btnFeaDB.Enabled = false;
            //        btnFileDB.Enabled = false;
            //        btnImageDB.Enabled = false;
            //        btnMapDB.Enabled = false;
            //        bnAddressDB.Enabled = false;
            //    }
            //    if (aTree.SelectedNode.Text == enumInterDBType.电子地图数据库.ToString())
            //    {
            //        btnMapDB.Enabled = true;
            //        btnDemDB.Enabled = false;
            //        btnEntiDB.Enabled = false;
            //        btnFeaDB.Enabled = false;
            //        btnFileDB.Enabled = false;
            //        btnImageDB.Enabled = false;
            //        btnSubjectDB.Enabled = false;
            //        bnAddressDB.Enabled = false;
            //    }
            //}
            //else
            //{
            //    //按钮不可用
            //    btnDemDB.Enabled = false;
            //    btnEntiDB.Enabled = false;
            //    btnFeaDB.Enabled = false;
            //    btnFileDB.Enabled = false;
            //    btnImageDB.Enabled = false;
            //    btnMapDB.Enabled = false;
            //    btnSubjectDB.Enabled = false;
            //    bnAddressDB.Enabled = false;
            //}
        }

        private void advTreeProject_NodeMouseDown(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            ////////////////////////////////////////数据库工程树图的右键菜单//////////////////////////
            if (e.Button == MouseButtons.Right && advTreeProject.SelectedNode != null && advTreeProject.SelectedNode.Level==2)
            {
                advTreeProject.SelectedNode.ContextMenu = contextDataSource;
            }
            if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            DevComponents.AdvTree.AdvTree aTree = sender as DevComponents.AdvTree.AdvTree;
            if (aTree.SelectedNode == null) return;

            if (aTree.SelectedNode.Text!="数据库管理工具") return;  //cyf 20110713 add

            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("ContextMenuProTree4"))
            {
                if (_dicContextMenu["ContextMenuProTree4"].Items.Count > 0)
                {
                    item = _dicContextMenu["ContextMenuProTree4"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        aTree.SelectedNode.ContextMenu = item;
                    }
                }
            }
        }

        private void MenuItemAttr_Click(object sender, EventArgs e)
        {
            if(advTreeProject.SelectedNode!=null)
                browseDSAttr(advTreeProject.SelectedNode);
        }
        //yjl20111016 add 数据源属性
        private void browseDSAttr(DevComponents.AdvTree.Node inNode)
        {
            string DSname = inNode.Text;
            IWorkspace pWorkspace = Plugin.ModuleCommon.TmpWorkSpace;
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            ITable pTable = pFeatureWorkspace.OpenTable("DATABASEMD");
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "DATABASENAME='" + DSname + "'";
            ICursor pCursor = pTable.Search(pQF, false);
            if (pCursor == null)
                return;
            IRow pRow = pCursor.NextRow();
            int fdx = pTable.FindField("CONNECTIONINFO");
            if (pRow != null && fdx != -1)
            {
                string value = pRow.get_Value(fdx).ToString();
                frmDSAttribute fmDSAttr = new frmDSAttribute(value,pWorkspace);
                //   fmDSAttr.m_strValue = value;
                if (fmDSAttr.ShowDialog() == DialogResult.OK)
                {
                    //修改记录  m_strValue 修改数据库中对应的表记录
                    Exception eError;
                    SysGisTable sysTable = new SysGisTable(pWorkspace);
                    Dictionary<string, object> dicData = new Dictionary<string, object>();
                    dicData.Add("CONNECTIONINFO", fmDSAttr.m_strValue);
                    if (!sysTable.UpdateRow("DATABASEMD", pQF.WhereClause, dicData, out eError))
                    {
                        MessageBox.Show("更新失败: " + eError.Message);
                        return;
                    }


                }

            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            pCursor = null;
            pFeatureWorkspace = null;
            pWorkspace = null;
        }
        /// <summary>
        /// ZQ 20111119 add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupPanel1_SizeChanged(object sender, EventArgs e)
        {

        }

    }
}
