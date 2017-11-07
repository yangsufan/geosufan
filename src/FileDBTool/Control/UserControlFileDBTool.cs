using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Xml;
using Plugin;
using System.IO;


using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.TrackingAnalyst;

namespace FileDBTool
{
    public partial class UserControlFileDBTool : UserControl
    {
        private bool m_bGroupLayerVisible;   //控制解决GroupLayer显示问题(方法有待改进 )

        //右键菜单集合
        private Dictionary<string, DevComponents.DotNetBar.ContextMenuBar> _dicContextMenu;

        //地图浏览工具栏容器

        private Control _MapToolControl;
        public Control ToolControl
        {
            get { return _MapToolControl; }
        }

        private Control _SysSetToolControl;

        //private bool m_bGroupLayerVisible;   //控制解决GroupLayer显示问题(方法有待改进 )
       
        //数据浏览工具BAR
        private DevComponents.DotNetBar.Bar _MapToolBar;
        public DevComponents.DotNetBar.Bar MapToolBar
        {
            get { return _MapToolBar; }
        }

        //系统设置工具栏容器

        public Control SysToolControl
        {
            get { return _SysSetToolControl; }
        }

        //系统设置工具Bar
        private DevComponents.DotNetBar.Bar _SysToolBar;
        public DevComponents.DotNetBar.Bar SysToolBar
        {
            get { return _SysToolBar; }
        }

        //系统设置item
        private DevComponents.DotNetBar.RibbonTabItem sysRibbonItem = new DevComponents.DotNetBar.RibbonTabItem();
        
        //其他菜单项item
        private DevComponents.DotNetBar.RibbonTabItem manageRibbonItem = new DevComponents.DotNetBar.RibbonTabItem();

        public UserControlFileDBTool(string strName, string strCation)
        {
            InitializeComponent();

            InitialMainControl();

            this.Name = strName;
            this.Tag = strCation;
            this.Dock = System.Windows.Forms.DockStyle.Fill;

            axMapControl.Map.Name = "数据图层";
            axTOCControl.SetBuddyControl(axMapControl.Object);
            advTreeProject.ImageList = IconContainer;

            ModData.v_AppFileDB.MainUserControl = this;
            ModData.v_AppFileDB.TOCControl = axTOCControl.Object as ITOCControlDefault;
            ModData.v_AppFileDB.MapControl = axMapControl.Object as IMapControlDefault;
            ModData.v_AppFileDB.ArcGisMapControl = axMapControl;

            ModData.v_AppFileDB.ProjectTree = advTreeProject;     //工程树图
            ModData.v_AppFileDB.DataInfoGrid=DataInfoGridView;    //数据信息列表
            ModData.v_AppFileDB.MetaDataGrid = MetaDataGridView;  //元数据信息列表


            ModData.v_AppFileDB.SysSettingGrid=dataGridViewSysSetting;//系统设置信息列表
            ModData.v_AppFileDB.SysSettingTree = advTreeSysSetting;   //系统设置树图
            
            ModData.v_AppFileDB.TxtDisplayPage = txtDisplayPage;
            ModData.v_AppFileDB.BtnFirst = btnFirst;
            ModData.v_AppFileDB.BtnLast = btnLast;
            ModData.v_AppFileDB.BtnNext = btnNext;
            ModData.v_AppFileDB.BtnPre = btnPreview;

            ModData.v_AppFileDB.Visible = this.Visible;
            ModData.v_AppFileDB.Enabled = this.Enabled;
            ModData.v_AppFileDB.CurrentThread = null;

            //初始化框架插件界面

            InitialFrmDefControl();

            //初始化树图

            //InitialDataConnTree();

            //if (_ComboBoxScale != null)
            //{
            //    _ComboBoxScale.Items.Clear();
            //    _ComboBoxScale.Items.AddRange(new object[] { "不选择", "500", "1000", "2000", "5000", "10000", "20000" });
            //    _ComboBoxScale.SelectedIndex = 0;
            //}
           
        }

        #region 界面初始化函数

        //初始化窗体控件布局
        private void InitialMainControl()
        {
            frmBarManager newfrmBarManager = new frmBarManager();
            newfrmBarManager.TopLevel = false;    //只是该窗体是否为顶级窗口
            newfrmBarManager.Dock = DockStyle.Fill;//frmBarManager完全填充父窗体

            newfrmBarManager.Show();
            this.Controls.Add(newfrmBarManager);//将frmBarManager添加进去

            //设置树图
            DevComponents.DotNetBar.Bar barTree = newfrmBarManager.CreateBar("barTree", enumLayType.FILL);
            barTree.CanHide = false;
            barTree.CanAutoHide = true;
            DevComponents.DotNetBar.PanelDockContainer PanelTree = newfrmBarManager.CreatePanelDockContainer("PanelTree", barTree);
            DevComponents.DotNetBar.DockContainerItem TreeContainerItem = newfrmBarManager.CreateDockContainerItem("TreeContainerItem", "项目树图", PanelTree, barTree);
            //加载控制树图
            PanelTree.Controls.Add(this.tabControl);
            this.tabControl.Dock = DockStyle.Fill;
            //this.tabControl.Visible = false;
            //this.tabControl.Hide();
            //加载系统设置树图
            PanelTree.Controls.Add(this.advTreeSysSetting);
            this.advTreeSysSetting.Dock = DockStyle.Fill;
            this.advTreeSysSetting.Visible = false;
            this.advTreeSysSetting.Hide();


            //设置数据视图
            DevComponents.DotNetBar.Bar barMap = newfrmBarManager.CreateBar("barMap", enumLayType.FILL);
            barMap.CanHide = false;
            DevComponents.DotNetBar.PanelDockContainer PanelMap = newfrmBarManager.CreatePanelDockContainer("PanelMap", barMap);
            DockContainerItem MapContainerItem = newfrmBarManager.CreateDockContainerItem("TreeContainerItem", "工作空间", PanelMap, barMap);
            //加载设置数据视图
            PanelMap.Controls.Add(this.tabControlInfo);
            this.tabControlInfo.Dock = DockStyle.Fill;
            tabControlPanelMapView.Controls.Add(this.axMapControl);
            this.axMapControl.Dock=DockStyle.Fill;
            _MapToolControl = tabControlPanelMapView as Control;
            //_MapToolControl.Controls.Add(axMapControl);
            //this.tabControlInfo.Visible = false;
            //this.tabControlInfo.Hide();
            //加载系统设置列表
            _SysSetToolControl = PanelMap as Control;
            _SysSetToolControl.Controls.Add(dataGridViewSysSetting);
            dataGridViewSysSetting.Dock = DockStyle.Fill;
            dataGridViewSysSetting.Visible = false;
            dataGridViewSysSetting.Hide();

            

            //布局设置
            newfrmBarManager.MainDotNetBarManager.FillDockSite.GetDocumentUIManager().Dock(barTree, barMap,DevComponents.DotNetBar.eDockSide.Right);
            newfrmBarManager.MainDotNetBarManager.FillDockSite.GetDocumentUIManager().SetBarWidth(barTree, this.Width / 5);

            //分页显示Bar
            tabControlDataList.Controls.Add(DevBar);
            this.DevBar.Location = new System.Drawing.Point(this.DataInfoGridView.Location.X, this.DataInfoGridView.Location.Y + this.DataInfoGridView.Height);
            this.DevBar.Size = new System.Drawing.Size(this.DataInfoGridView.Width, 20);
            this.DevBar.Dock = DockStyle.Bottom;
        }

        //初始化框架插件控件界面

        private void InitialFrmDefControl()
        {
            //得到Xml的System节点,根据XML加载插件界面
            string xPath = ".//System[@Name='" + this.Name + "']";
            Plugin.ModuleCommon.LoadButtonViewByXmlNode(ModData.v_AppFileDB.ControlContainer, xPath, ModData.v_AppFileDB);

            _dicContextMenu = ModData.v_AppFileDB.DicContextMenu;

            //初始化地图浏览工具栏
            Plugin.Application.IAppFormRef pAppFrm = ModData.v_AppFileDB as Plugin.Application.IAppFormRef;
            XmlNode barXmlNode = pAppFrm.SystemXml.SelectSingleNode(".//ToolBar[@Name='ControlMapToolBar3']");
            if (barXmlNode == null || _MapToolControl == null) return;
            DevComponents.DotNetBar.Bar mapToolBar = Plugin.ModuleCommon.LoadButtonView(_MapToolControl, barXmlNode, pAppFrm, null) as Bar;
            if (mapToolBar != null)
            {
                _MapToolBar = mapToolBar;
                mapToolBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
                mapToolBar.DockOrientation = DevComponents.DotNetBar.eOrientation.Horizontal;
                mapToolBar.DockSide = DevComponents.DotNetBar.eDockSide.Top ;
                mapToolBar.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
                mapToolBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
                _MapToolControl.Controls.Remove(mapToolBar);
                _MapToolControl.Controls.Remove(this.axMapControl);
                _MapToolControl.Controls.Add(this.axMapControl);
                this.axMapControl.Dock=DockStyle.Fill;
                _MapToolControl.Controls.Add(mapToolBar);
            }

            //初始化系统设置工具栏
            XmlNode SysSetXmlNode = pAppFrm.SystemXml.SelectSingleNode(".//ToolBar[@Name='ControlSysSetToolBar']");
            if (SysSetXmlNode == null || _SysSetToolControl == null) return;
            DevComponents.DotNetBar.Bar sysToolBar=Plugin.ModuleCommon.LoadButtonView(_SysSetToolControl,SysSetXmlNode,pAppFrm,null) as Bar;
            if(sysToolBar!=null)
            {
                _SysToolBar = sysToolBar;
                sysToolBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
                sysToolBar.DockOrientation = DevComponents.DotNetBar.eOrientation.Horizontal;
                sysToolBar.DockSide = DevComponents.DotNetBar.eDockSide.Top;
                sysToolBar.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
                sysToolBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
                _SysSetToolControl.Controls.Remove(sysToolBar);
                _SysSetToolControl.Controls.Add(sysToolBar);

                //影藏系统设置工具栏

                this._SysToolBar.Visible = false;
                this._SysToolBar.Hide();
            }

            //右键菜单加载
            foreach (KeyValuePair<string, DevComponents.DotNetBar.ContextMenuBar> keyValue in _dicContextMenu)
            {
                this.Controls.Add(keyValue.Value);
            }

            //主菜单的获取和事件的绑定
            foreach (KeyValuePair<RibbonTabItem, string> item in Plugin.ModuleCommon.DicTabs)
            {
                //RibbonTabItem的名称为与传进来的一致

                if (item.Value == this.Name)
                {
                    if (item.Key.Name == "SystemConfiguratioon")
                    {
                        sysRibbonItem = item.Key;
                        sysRibbonItem.Click += new System.EventHandler(sysRibbonItem_Click);
                    }
                    else
                    {
                        manageRibbonItem = item.Key;
                        manageRibbonItem.Click += new System.EventHandler(manageRibbonItem_Click);
                    }

                }

            }

            //比例尺下拉列表框的加载和事件的绑定

            //foreach (KeyValuePair<DevComponents.DotNetBar.BaseItem, string> kvCmd in Plugin.ModuleCommon.DicBaseItems)
            //{
            //    if (kvCmd.Value == "FileDBTool.ControlsScaleSel")
            //    {
            //        DevComponents.DotNetBar.ComboBoxItem aComboBoxItem = kvCmd.Key as DevComponents.DotNetBar.ComboBoxItem;
            //        if (aComboBoxItem != null)
            //        {
            //            aComboBoxItem.ComboWidth = 150;
            //            _ComboBoxScale = aComboBoxItem;
            //            _ComboBoxScale.SelectedIndexChanged += new EventHandler(ComboBoxScale_SelectedIndexChanged);
            //            break;
            //        }
            //    }
            //}
            //时间下拉列表框的加载和事件的绑定
            //foreach (KeyValuePair<DevComponents.DotNetBar.BaseItem, string> kvCmd in Plugin.ModuleCommon.DicBaseItems)
            //{
            //    if (kvCmd.Value == "FileDBTool.ControlsTimeShow")
            //    {
            //        DevComponents.DotNetBar.ComboBoxItem aComboBoxItem = kvCmd.Key as DevComponents.DotNetBar.ComboBoxItem;
            //        if (aComboBoxItem != null)
            //        {
            //            aComboBoxItem.ComboWidth = 200;
            //            _ComboBoxTime = aComboBoxItem;
            //            _ComboBoxTime.SelectedIndexChanged += new EventHandler(ComboBoxTime_SelectedIndexChanged);
            //            break;
            //        }
            //    }
            //}
        }


        //初始化数据连接界面

        private void InitialDataConnTree()
        {
            if (File.Exists(ModData.v_CoonectionInfoXML))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ModData.v_CoonectionInfoXML);
                ModData.v_AppFileDB.DBXmlDocument = xml;
                AddTreeNodeByXML(xml, advTreeProject);
            }
        }

        /// <summary>
        /// 读取xml将树图信息添加到工程树图上  陈亚飞编写

        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="MainTree"></param>
        private void AddTreeNodeByXML(XmlDocument xmlDoc, DevComponents.AdvTree.AdvTree MainTree)
        {
            if (MainTree.Nodes != null)
            {
                MainTree.Nodes.Clear();
            }
            foreach (XmlNode oneNode in xmlDoc.FirstChild.ChildNodes)
            {
                XmlElement xmlElem = oneNode as XmlElement;
                DevComponents.AdvTree.Node ConnNode = new DevComponents.AdvTree.Node();
                ConnNode.Name = xmlElem.GetAttribute("NodeName");
                ConnNode.Text = xmlElem.GetAttribute("NodeText");
                ConnNode.DataKey= xmlElem.GetAttribute("NodeType").ToString();
                if (ConnNode.DataKey.ToString() == EnumTreeNodeType.DATABASE.ToString())
                {
                    //数据库节点

                    ConnNode.ImageIndex = 1;
                  
                    //若为数据库且子节点信息不为空，则将子节点（连接信息）挂在树上
                    XmlNode subXmlNode = oneNode.FirstChild;
                    if (subXmlNode != null)
                    {
                        ConnNode.Tag = subXmlNode;//连接信息
                    }
                }
                else if (ConnNode.DataKey.ToString() == EnumTreeNodeType.DATACONNECT.ToString())
                {
                    //数据连接节点
                    ConnNode.ImageIndex = 0;
                }
                MainTree.Nodes.Add(ConnNode);
            }
        }


        #endregion

        #region 分页显示事件
        //下一页

        private void btnNext_Click(object sender, EventArgs e)
        {
            string pageInfo = txtDisplayPage.ControlText.Trim();
            if (pageInfo == "") return;
            int index2 = pageInfo.IndexOf("/");
            int len = index2;
            int currentPage = int.Parse(pageInfo.Substring(0, len).Trim());
            int totalPage = int.Parse(pageInfo.Substring(index2 + 1).Trim());
            int recNum = ModData.pageSize * currentPage;

            Plugin.Application.IAppFileRef m_Hook = ModData.v_AppFileDB;

            ModDBOperator.LoadPage(m_Hook, ModData.TotalTable, currentPage + 1, recNum);
        }
        //上一页

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string pageInfo = txtDisplayPage.ControlText.Trim();
            if (pageInfo == "") return;
            int index2 = pageInfo.IndexOf("/");
            int len = index2;
            int currentPage = int.Parse(pageInfo.Substring(0, len).Trim());
            int recNum = ModData.pageSize * (currentPage - 2);

            Plugin.Application.IAppFileRef m_Hook = ModData.v_AppFileDB;
            ModDBOperator.LoadPage(m_Hook, ModData.TotalTable, currentPage - 1, recNum);
        }
        //第一页

        private void btnFirst_Click(object sender, EventArgs e)
        {
            ModData.recNum = 0;
            ModData.CurrentPage = 1;
            int recNum = ModData.recNum;
            int currentPage = ModData.CurrentPage;

            Plugin.Application.IAppFileRef m_Hook = ModData.v_AppFileDB;
            ModDBOperator.LoadPage(m_Hook, ModData.TotalTable, currentPage, recNum);
        }
        //最后一页

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (ModData.TotalPageCount == 0) return;
            int recNum = (ModData.TotalPageCount - 1) * ModData.pageSize;
            int currentPage = ModData.TotalPageCount;

            Plugin.Application.IAppFileRef m_Hook = ModData.v_AppFileDB;
            ModDBOperator.LoadPage(m_Hook, ModData.TotalTable, currentPage, recNum);
        }

        //文本框鼠标事件

        private void txtDisplayPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtDisplayPage.ControlText == "") return;
                int totalCount = ModData.TotalPageCount;//总页数

                if (totalCount == 0) return;
                int curPage;
                try
                {
                    curPage = int.Parse(txtDisplayPage.ControlText.Trim());//当前页

                }
                catch
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("温馨提示", "请输入有效的页数！");
                    txtDisplayPage.ControlText = "";
                    btnPreview.Enabled = false;
                    btnNext.Enabled = false;
                    btnFirst.Enabled = true;
                    btnLast.Enabled = true;
                    return;
                }
                if (curPage <= 0 || curPage > totalCount)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("温馨提示", "请输入有效的页数！");
                    txtDisplayPage.ControlText = "";
                    btnPreview.Enabled = false;
                    btnNext.Enabled = false;
                    btnFirst.Enabled = true;
                    btnLast.Enabled = true;
                    return;
                }
                int recNum = (curPage - 1) * ModData.pageSize;
                Plugin.Application.IAppFileRef m_Hook = ModData.v_AppFileDB;
                ModDBOperator.LoadPage(m_Hook, ModData.TotalTable, curPage, recNum);
            }
        }
        #endregion


        private void axMapControl_OnAfterDraw(object sender, IMapControlEvents2_OnAfterDrawEvent e)
        {
            try
            {
                ModData.v_AppFileDB.RefScaleCmb.ControlText = (sender as AxMapControl).Map.ReferenceScale.ToString().Trim();
                ModData.v_AppFileDB.CurScaleCmb.ControlText = (sender as AxMapControl).Map.MapScale.ToString().Trim();
                ModData.v_AppFileDB.CurScaleCmb.Tooltip = axMapControl.Map.MapScale.ToString().Trim();
            }
            catch
            {
            }
        }

        private void axMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            //ModData.v_AppSMPD.CoorTxt.ControlText = "X:" + e.mapX + ";Y:" + e.mapY;
            ModData.v_AppFileDB.OperatorTips = "X:" + e.mapX + ";Y:" + e.mapY;
        }

        
        //任务树图上单击节点，打开相应的工程  陈亚飞增加

        private void advTreeProject_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            //陈亚飞添加

            Exception eError = null;


            if (e.Button == MouseButtons.Left)
            {
                SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();    //属性库连接类

                SysCommon.Gis.SysGisDataSet pSysGis = new SysCommon.Gis.SysGisDataSet();   //范围库连接

                DevComponents.AdvTree.AdvTree aTree = sender as DevComponents.AdvTree.AdvTree;
                DevComponents.AdvTree.Node mDBNode = null;//定义数据库节点


                //获得根节点

                mDBNode = aTree.SelectedNode;
                while (mDBNode.Parent != null)
                {
                    mDBNode = mDBNode.Parent;
                }

                //若不是数据库节点，就返回
                if (mDBNode == null) return;
                if (mDBNode.DataKey == null) return;
                if (mDBNode.DataKey.ToString() == "") return;
                if (mDBNode.DataKey.ToString() != EnumTreeNodeType.DATABASE.ToString()) return;
                if (aTree.SelectedNode.DataKey.ToString() == EnumTreeNodeType.DATABASE.ToString() && aTree.SelectedNode.ImageIndex == 1)
                {
                    //说明未连接上
                    return;
                }

                if (mDBNode.Name == "文件连接")
                {
                    DevComponents.AdvTree.Node ProjectNode = null;           //工程项目节点
                    long projectID = -1;                                     //项目ID
                    string RangeCondi = "";                                  //范围图显示条件

                    string ProRangePath = "";                                //项目图幅结合表

                    string ProIndexPath = "";                                //项目底图
                    string scpStr = "";                                      //查询控制点表格的条件   
                    long conScale = 0;                                       //控制点数据所在的产品的比例尺信息
                    //清空资源
                    if (ModData.v_AppFileDB.DataInfoGrid.DataSource != null)
                    {
                        ModData.v_AppFileDB.DataInfoGrid.DataSource = null;
                    }
                    if (ModData.v_AppFileDB.MetaDataGrid.DataSource != null)
                    {
                        ModData.v_AppFileDB.MetaDataGrid.DataSource = null;
                    }

                    DataTable dt = new DataTable();  //数据信息列表
                    DataTable metaDt = new DataTable();//数据元信息列表

                    string nodeType = aTree.SelectedNode.DataKey.ToString();

                    //连接数据库

                    XmlElement dbElem = mDBNode.Tag as XmlElement;
                    if (dbElem == null) return;
                    string ipStr = dbElem.GetAttribute("MetaDBConn");

                    //string ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ipStr + ";Persist Security Info=True";//元数据连接字符串

                    //设置数据库连接

                    //pSysDB.SetDbConnection(ConnStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                    //******************************************************************************************************
                    //guozheng 2010-10-12  改为Oracle连接方式
                    pSysDB.SetDbConnection(ipStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
                    //*******************************************************************************************************
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！连接地址为：" + ipStr);
                        return;
                    }
                    //设置范围图层工作空间
                    //pSysGis.SetWorkspace("E:\\ftppath\\MapDB.mdb", SysCommon.enumWSType.PDB, out eError);
                    //if (eError != null)
                    //{
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接范围图层失败！");
                    //    return;
                    //}
                    //标准图幅的所有范围

                    Dictionary<string, List<string>> RangeDirectory = new Dictionary<string, List<string>>();
                    //非标准图幅的所有范围

                    Dictionary<string, List<string>> NonRangeDirectory = new Dictionary<string, List<string>>();

                    if (nodeType == EnumTreeNodeType.DATABASE.ToString())
                    {
                        #region 打开项目信息
                        //获得项目信息
                        dt = GetProjectInfo(pSysDB, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        #endregion
                    }

                    else if (nodeType == EnumTreeNodeType.PROJECT.ToString())
                    {
                        #region 打开产品信息、项目元信息、加载项目底图


                        if (aTree.SelectedNode.Tag == null) return;
                        if (aTree.SelectedNode.Tag.ToString() == "") return;
                        projectID = long.Parse(aTree.SelectedNode.Tag.ToString());
                        //获得产品信息
                        dt = GetProductInfo(pSysDB, projectID, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        //获得项目元信息

                        metaDt = GetProjecMetaData(pSysDB, projectID, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //获得项目图幅结合表

                        ProRangePath = GetProjectRangePath("图幅结合表", projectID, pSysDB, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //获得项目底图
                        ProIndexPath = GetProjectRangePath("底图", projectID, pSysDB, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        string SQL = "SELECT 比例尺分母,范围号,数据类型编号 FROM ProductIndexTable WHERE 项目ID=" + projectID;
                        DataTable DataTable = pSysDB.GetSQLTable(SQL, out eError);
                        if (null != eError)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //清空底图
                        ModData.v_AppFileDB.MapControl.Map.ClearLayers();

                        //WriteConditionDictionary(DataTable, RangeDirectory);
                        RangeDirectory = GetRangeNODic(DataTable.Select("数据类型编号=0"));//标准图幅
                        NonRangeDirectory = GetRangeNODic(DataTable.Select("数据类型编号=1"));//非标准图幅


                        //无比例尺信息的所有范围号，包括标准图幅和非标准图幅

                        RangeCondi = GetRangeNOStr(DataTable);

                        #endregion
                    }
                    else if (nodeType == EnumTreeNodeType.DATAFORMAT.ToString())
                    {
                        #region 打开产品信息、项目元信息
                        if (aTree.SelectedNode.Parent.Tag == null) return;
                        if (aTree.SelectedNode.Parent.Tag.ToString() == "") return;
                        projectID = long.Parse(aTree.SelectedNode.Parent.Tag.ToString());
                        if (aTree.SelectedNode.Tag == null) return;
                        if (aTree.SelectedNode.Tag.ToString() == "") return;
                        int dataFormatID = int.Parse(aTree.SelectedNode.Tag.ToString());
                        //获得产品信息
                        dt = GetProductInfo(pSysDB, projectID, dataFormatID, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        //获得项目元信息

                        metaDt = GetProjecMetaData(pSysDB, projectID, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //获得项目图幅结合表

                        ProRangePath = GetProjectRangePath("图幅结合表", projectID, pSysDB, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        //获得项目底图
                        ProIndexPath = GetProjectRangePath("底图", projectID, pSysDB, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }


                        string SQL = "SELECT 比例尺分母,范围号,数据类型编号 FROM ProductIndexTable WHERE 项目ID=" + projectID + " AND 数据格式编号=" + dataFormatID;
                        DataTable DataTable = pSysDB.GetSQLTable(SQL, out eError);
                        if (null != eError)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        //WriteConditionDictionary(DataTable, RangeDirectory);

                        RangeDirectory = GetRangeNODic(DataTable.Select("数据类型编号=0"));//标准图幅
                        NonRangeDirectory = GetRangeNODic(DataTable.Select("数据类型编号=1"));//非标准图幅


                        RangeCondi = GetRangeNOStr(DataTable);

                        #endregion
                    }
                    else if (nodeType == EnumTreeNodeType.PRODUCT.ToString())
                    {
                        #region 打开数据信息，产品元信息,加载产品范围

                        if (aTree.SelectedNode.Parent.Parent.Tag == null) return;
                        if (aTree.SelectedNode.Parent.Parent.Tag.ToString() == "") return;
                        projectID = long.Parse(aTree.SelectedNode.Parent.Parent.Tag.ToString());
                        if (aTree.SelectedNode.Parent.Tag == null) return;
                        if (aTree.SelectedNode.Parent.Tag.ToString() == "") return;
                        int dataFormatID = int.Parse(aTree.SelectedNode.Parent.Tag.ToString());
                        if (aTree.SelectedNode.Tag == null)
                            if (aTree.SelectedNode.Tag.ToString() == "") return;
                        long productID = int.Parse(aTree.SelectedNode.Tag.ToString());
                        //数据信息
                        dt = ModDBOperator.GetDataInfo(pSysDB, productID,projectID, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        //产品元信息

                        metaDt = GetProductMetaData(pSysDB, productID, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //获得项目图幅结合表

                        ProRangePath = GetProjectRangePath("图幅结合表", projectID, pSysDB, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //获得项目底图
                        ProIndexPath = GetProjectRangePath("底图", projectID, pSysDB, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }


                        string SQL = "SELECT 比例尺分母,范围号,数据类型编号 FROM ProductIndexTable WHERE 产品ID=" + productID;
                        DataTable DataTable = pSysDB.GetSQLTable(SQL, out eError);
                        if (null != eError)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        //WriteConditionDictionary(DataTable, RangeDirectory);

                        RangeDirectory = GetRangeNODic(DataTable.Select("数据类型编号=0"));//标准图幅
                        NonRangeDirectory = GetRangeNODic(DataTable.Select("数据类型编号=1"));//非标准图幅


                        RangeCondi = GetRangeNOStr(DataTable);

                        //查询控制点数据
                        scpStr = "select * from ControlPointMDTable where 产品ID=" + productID;
                        if (DataTable.Rows.Count > 0)
                        {
                            conScale = Convert.ToInt64(DataTable.Rows[0][0].ToString().Trim());
                        }
                                    
                        #endregion
                    }
                    else if (nodeType == EnumTreeNodeType.PRODUCTPYPE.ToString())
                    {
                        #region 打开数据信息，产品元信息,加载产品范围

                        if (aTree.SelectedNode.Parent.Parent.Parent.Tag == null) return;
                        if (aTree.SelectedNode.Parent.Parent.Parent.Tag.ToString() == "") return;
                        projectID = long.Parse(aTree.SelectedNode.Parent.Parent.Parent.Tag.ToString());//项目
                        if (aTree.SelectedNode.Parent.Parent.Tag == null) return;
                        if (aTree.SelectedNode.Parent.Parent.Tag.ToString() == "") return;
                        int dataFormatID = int.Parse(aTree.SelectedNode.Parent.Parent.Tag.ToString());//产品格式
                        if (aTree.SelectedNode.Parent.Tag == null) return;
                        if (aTree.SelectedNode.Parent.Tag.ToString() == "") return;
                        long productID = int.Parse(aTree.SelectedNode.Parent.Tag.ToString());//产品
                        if (aTree.SelectedNode.Tag == null) return;
                        if (aTree.SelectedNode.Tag.ToString() == "") return;
                        int dataTypeID = int.Parse(aTree.SelectedNode.Tag.ToString());//产品类型

                        //获得数据信息
                        if (dataTypeID == 0)
                        {
                            dt = ModDBOperator.GetMapDataInfo(pSysDB, productID, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysDB.CloseDbConnection();
                                return;
                            }
                        }
                        else if (dataTypeID == 1)
                        {
                            dt = ModDBOperator.GetNonMapDataInfo(pSysDB, productID, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysDB.CloseDbConnection();
                                return;
                            }
                        }
                        else if (dataTypeID == 2)
                        {
                            dt = ModDBOperator.GetSCPDataInfo(pSysDB, productID, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysDB.CloseDbConnection();
                                return;
                            }
                            scpStr = "select * from ControlPointMDTable where 产品ID=" + productID;
                            string mmstr = "SELECT 比例尺分母 FROM ProductIndexTable WHERE 产品ID=" + productID;
                            DataTable mmDt = pSysDB.GetSQLTable(mmstr, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查找产品比例尺信息失败！");
                                pSysDB.CloseDbConnection();
                                return;
                            }
                            if (mmDt.Rows.Count > 0)
                            {
                                conScale = Convert.ToInt64(mmDt.Rows[0][0].ToString().Trim());
                            }
                            
                        }

                        //获得产品元信息

                        metaDt = GetProductMetaData(pSysDB, productID, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //获得项目图幅结合表

                        ProRangePath = GetProjectRangePath("图幅结合表", projectID, pSysDB, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //获得项目底图
                        ProIndexPath = GetProjectRangePath("底图", projectID, pSysDB, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        string SQL = "SELECT  比例尺分母,范围号,数据类型编号 FROM ProductIndexTable WHERE 项目ID=" + projectID + " AND  产品ID=" + productID + " AND 数据类型编号=" + dataTypeID;
                        DataTable DataTable = pSysDB.GetSQLTable(SQL, out eError);
                        if (null != eError)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        //WriteConditionDictionary(DataTable, RangeDirectory);

                        RangeDirectory = GetRangeNODic(DataTable.Select("数据类型编号=0"));//标准图幅
                        NonRangeDirectory = GetRangeNODic(DataTable.Select("数据类型编号=1"));//非标准图幅


                        RangeCondi = GetRangeNOStr(DataTable);

                        #endregion
                    }
                    else if (nodeType == EnumTreeNodeType.DATAITEM.ToString())
                    {
                        #region 打开数据元信息


                        if (aTree.SelectedNode.Parent.Tag == null) return;
                        if (aTree.SelectedNode.Parent.Tag.ToString() == "") return;
                        int dataTypeID = int.Parse(aTree.SelectedNode.Parent.Tag.ToString());//产品类型
                        if (aTree.SelectedNode.Tag == null) return;
                        if (aTree.SelectedNode.Tag.ToString() == "") return;
                        long dataID = long.Parse(aTree.SelectedNode.Tag.ToString());//数据
                        DevComponents.AdvTree.Node proNode = aTree.SelectedNode;
                        while(proNode.DataKey.ToString()!=EnumTreeNodeType.PROJECT.ToString())
                        {
                            proNode = proNode.Parent;
                        }
                        projectID = long.Parse(proNode.Tag.ToString());

                        metaDt = GetDataMetaInfo(pSysDB, dataID, dataTypeID, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //获得项目图幅结合表

                        ProRangePath = GetProjectRangePath("图幅结合表", projectID, pSysDB, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //获得项目底图
                        ProIndexPath = GetProjectRangePath("底图", projectID, pSysDB, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        string SQL = "SELECT 比例尺分母,范围号,数据类型编号 FROM ProductIndexTable WHERE 数据ID=" + dataID + " AND 数据类型编号=" + dataTypeID;
                        DataTable DataTable = pSysDB.GetSQLTable(SQL, out eError);
                        if (null != eError)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        //WriteConditionDictionary(DataTable, RangeDirectory);

                        RangeDirectory = GetRangeNODic(DataTable.Select("数据类型编号=0"));//标准图幅
                        NonRangeDirectory = GetRangeNODic(DataTable.Select("数据类型编号=1"));//非标准图幅


                        RangeCondi = GetRangeNOStr(DataTable);
                        //控制点数据
                        scpStr = "select * from ControlPointMDTable where ID=" + dataID;
                        if (DataTable.Rows.Count > 0)
                        {
                            conScale = Convert.ToInt64(DataTable.Rows[0][0].ToString().Trim());
                        }
                        
                        #endregion
                    }


                    //获得控制点表格
                    DataTable scpTable = null;         //控制点元信息表
                    if (scpStr != "")
                    {
                        scpTable = pSysDB.GetSQLTable(scpStr, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysDB.CloseDbConnection();
                            //return;
                        }
                    }
                    pSysDB.CloseDbConnection();


                    //界面上显示


                    #region 将表格绑定起来  数据列表信息
                    ModData.v_AppFileDB.DataInfoGrid.DataSource = dt;
                    ModData.v_AppFileDB.DataInfoGrid.ReadOnly = true;
                    ModData.v_AppFileDB.DataInfoGrid.Visible = true;
                    ModData.v_AppFileDB.DataInfoGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    for (int j = 0; j < ModData.v_AppFileDB.DataInfoGrid.Columns.Count; j++)
                    {
                        if (ModData.v_AppFileDB.DataInfoGrid.Columns[j].Name == "图形数据")
                        {
                            ModData.v_AppFileDB.DataInfoGrid.Columns[j].Visible = false;
                            continue; 
                        }
                        ModData.v_AppFileDB.DataInfoGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        //ModData.v_AppFileDB.DataInfoGrid.Columns[j].Width = (ModData.v_AppFileDB.DataInfoGrid.Width - 20) / ModData.v_AppFileDB.DataInfoGrid.Columns.Count;
                    }
                   
                    ModData.v_AppFileDB.DataInfoGrid.RowHeadersWidth = 20;
                    ModData.v_AppFileDB.DataInfoGrid.Refresh();

                    //将表格绑定起来  元数据列表信息

                    ModData.v_AppFileDB.MetaDataGrid.DataSource = metaDt;
                    #region 处理元信息显示中时间的问题
                    for (int i = 0; i < ModData.v_AppFileDB.MetaDataGrid.Rows.Count; i++)
                    {
                        string ConmName=ModData.v_AppFileDB.MetaDataGrid.Rows[i].Cells[0].Value.ToString();
                         if(ConmName=="生产日期" || ConmName=="开始时间" || ConmName=="结束时间" )
                        {
                            try
                            {
                                string Datestr = ModData.v_AppFileDB.MetaDataGrid.Rows[i].Cells[1].Value.ToString();
                                ModData.v_AppFileDB.MetaDataGrid.Rows[i].Cells[1].Value = Convert.ToDateTime(Datestr).ToShortDateString();
                            }
                            catch
                            {
                            }
                        }
                    }
                    #endregion
                    ModData.v_AppFileDB.MetaDataGrid.ReadOnly = true;
                    ModData.v_AppFileDB.MetaDataGrid.Visible = true;
                    ModData.v_AppFileDB.MetaDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    for (int j = 0; j < ModData.v_AppFileDB.MetaDataGrid.Columns.Count; j++)
                    {
                        ModData.v_AppFileDB.MetaDataGrid.Columns[j].Width = (ModData.v_AppFileDB.MetaDataGrid.Width - 20) / ModData.v_AppFileDB.MetaDataGrid.Columns.Count;
                    }
                    ModData.v_AppFileDB.MetaDataGrid.RowHeadersWidth = 20;
                    ModData.v_AppFileDB.MetaDataGrid.Refresh();

                    #endregion

                    //若存在项目索引图，先删掉
                    for (int i = 0; i < ModData.v_AppFileDB.MapControl.LayerCount; i++)
                    {
                        ILayer ppLayer = ModData.v_AppFileDB.MapControl.Map.get_Layer(i);
                        if (ppLayer is IGroupLayer && ppLayer.Name == "索引图")
                        {
                            ModData.v_AppFileDB.MapControl.Map.DeleteLayer(ppLayer);
                        }
                    }
                    //加载MXD文档，项目索引图
                    if (ProIndexPath != "")
                    {
                        try
                        {
                            IMapDocument pMapDocument = new MapDocumentClass();
                            pMapDocument.Open(ProIndexPath, "");
                            IMap pMap = pMapDocument.get_Map(0);
                            for (int i = 0; i < pMap.LayerCount; i++)
                            {
                                ILayer mLayer = pMap.get_Layer(i);
                                if (mLayer is IGroupLayer)
                                {
                                    //折叠起来
                                }
                                axMapControl.Map.AddLayer(mLayer);
                                //pGroupLayer1.Add(pMap.get_Layer(i));
                            }
                        }
                        catch
                        { 
                        }
                    }

                    //设置范围库连接

                    List<IDataset> LstDT = null;
                    SysCommon.Gis.SysGisDataSet pSysGISDT = null;
                    if (ProRangePath != "")
                    {
                        pSysGISDT = new SysCommon.Gis.SysGisDataSet();
                        pSysGISDT.SetWorkspace(ProRangePath, SysCommon.enumWSType.PDB, out eError);
                        if (eError != null)
                        {
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接范围数据库出错！");
                            return;
                        }
                        //获得范围库中所有的要素类

                        LstDT = pSysGISDT.GetAllFeatureClass();
                    }

                    //若已经存在组图，则进行清空

                    IGroupLayer pGroupLayer1 = null;
                    for (int i = 0; i < ModData.v_AppFileDB.MapControl.LayerCount; i++)
                    {
                        ILayer ppLayer = ModData.v_AppFileDB.MapControl.Map.get_Layer(i);
                        if (ppLayer is IGroupLayer && ppLayer.Name == "项目范围图")
                        {
                            pGroupLayer1 = ppLayer as IGroupLayer;
                            break;
                        }
                    }
                    if (pGroupLayer1 != null)
                    {
                        ModData.v_AppFileDB.MapControl.Map.DeleteLayer(pGroupLayer1 as ILayer);
                    }

                    //在绘制前，清除axMapControl中的任何图形元素
                    IGraphicsContainer pGra = axMapControl.Map as IGraphicsContainer;
                    pGra.DeleteAllElements();

                   
                    //加载组图
                    if (nodeType != EnumTreeNodeType.DATABASE.ToString())
                    {
                        pGroupLayer1 = new GroupLayerClass();
                        pGroupLayer1.Name = "项目范围图";
                        ModData.v_AppFileDB.MapControl.Map.AddLayer(pGroupLayer1 as ILayer);
                    }

                    //加载控制点（画点）

                    if (scpTable!=null)
                    {
                        ShowAllPoint(pGra,scpTable,conScale);
                       
                    }

                    #region 加载标准图幅和非标准图幅数据
                    if (pSysGISDT != null)
                    {
                        //加载标准图幅范围数据,带比例尺信息的，根据比例尺信息查找图层

                        AddLayerByCondi(RangeDirectory, "MapFrame_", pGroupLayer1, pSysGISDT, out eError);
                        //加载非标准图幅范围数据,带比例尺信息的，根据比例尺信息查找图层

                        AddLayerByCondi(NonRangeDirectory, "Range_", pGroupLayer1, pSysGISDT, out eError);
                    }

                    //加载自定义显示数据范围图，不带比例尺信息处理，在全库范围内查找

                    if (RangeCondi != "" && LstDT != null)
                    {
                        string wStr = "MAP_NEWNO in " + RangeCondi;
                        AddLayerByCondi(LstDT, wStr, pGroupLayer1);
                    }
                    #endregion

                    
                    //若组图下面没有任何图层，则删掉组图

                    if (pGroupLayer1 != null)
                    {
                        ICompositeLayer pComLayer = pGroupLayer1 as ICompositeLayer;
                        if (pComLayer.Count == 0)
                        {
                            ModData.v_AppFileDB.MapControl.Map.DeleteLayer(pGroupLayer1 as ILayer);
                        }
                    }

                    //加载项目底图
                    //if (pSysGISDT != null)
                    //{
                    //    GetRangeByPath(pSysGISDT, pGroupLayer1, out eError);
                    //    if (eError != null)
                    //    {
                    //        //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    //        pSysDB.CloseDbConnection();
                    //        return;
                    //    }
                    //}
                   
                    ModData.v_AppFileDB.MapControl.ActiveView.Refresh();
                    ModData.v_AppFileDB.TOCControl.Update();
                    //ModData.v_AppFileDB.MapControl.Map.ReferenceScale = 2000;
                }
            }
        }

        //菜单点击事件，系统设置点击事件

        private void sysRibbonItem_Click(object sender, EventArgs e)
        {
            //显示系统设置界面
            this.advTreeSysSetting.Visible =true;
            this.advTreeSysSetting.Show();
            dataGridViewSysSetting.Visible = true;
            dataGridViewSysSetting.Show();
            this._SysToolBar.Visible = true;
            this._SysToolBar.Show();

            //隐藏成果管理、数据浏览界面

            this.tabControl.Visible = false;
            this.tabControl.Hide();
            this.tabControlInfo.Visible = false;
            this.tabControlInfo.Hide();
        }

        //菜单点击事件，成果管理、数据浏览点击事件

        private void manageRibbonItem_Click(object sender,EventArgs e)
        {
            //显示系统设置界面
            this.advTreeSysSetting.Visible = false ;
            this.advTreeSysSetting.Hide();
            dataGridViewSysSetting.Visible = false;
            dataGridViewSysSetting.Hide();
            this._SysToolBar.Visible = false;
            this._SysToolBar.Hide();

            //隐藏成果管理、数据浏览界面

            this.tabControl.Visible = true;
            this.tabControl.Show();
            this.tabControlInfo.Visible = true;
            this.tabControlInfo.Show();
        }


        //树图的右键菜单

        private void advTreeProject_NodeMouseDown(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            DevComponents.AdvTree.AdvTree aTree = sender as DevComponents.AdvTree.AdvTree;
            if (aTree.SelectedNode == null) return;

            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("ContextMenuProTree3"))
            {
                if (_dicContextMenu["ContextMenuProTree3"].Items.Count > 0)
                {
                    item = _dicContextMenu["ContextMenuProTree3"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        aTree.SelectedNode.ContextMenu = item;
                    }
                }
            }
        }

        #region 点击树节点，在dataGrid中显示相应信息  陈亚飞编写

        
        //创建元信息表
        private DataTable CreateMetaTable()
        {
            DataTable dt = new DataTable();    //界面上显示表格

            dt.Columns.Add("字段", System.Type.GetType("System.String"));
            dt.Columns.Add("值", System.Type.GetType("System.String"));
            return dt;
        }

        /// <summary>
        /// 获得项目信息
        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private DataTable GetProjectInfo(SysCommon.DataBase.SysTable pSysDB, out Exception eError)
        {
            string str = "select * from ProjectMDTable";
            DataTable dt = pSysDB.GetSQLTable(str, out eError);
            if (eError != null)
            {
                eError = new Exception("获取项目信息出错！");
                return null;
            }
            return dt;
        }

        /// <summary>
        /// 获得项目元信息

        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="projectID"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private DataTable GetProjecMetaData(SysCommon.DataBase.SysTable pSysDB, long projectID, out Exception eError)
        {
            eError = null;
            DataTable returnDt = CreateMetaTable();
            string str = "select * from ProjectMDTable where ID=" + projectID;
            DataTable dt = pSysDB.GetSQLTable(str, out eError);
            if (eError != null)
            {
                eError = new Exception("获取项目元信息出错！");
                return null;
            }
           
            for(int i=0;i<dt.Columns.Count;i++)
            {
                DataRow aRow = returnDt.NewRow();
                aRow["字段"] = dt.Columns[i].ColumnName;
                if (dt.Rows.Count > 0)
                {

                    aRow["值"] = dt.Rows[0][i].ToString();
                }
                returnDt.Rows.Add(aRow);
            }
            return returnDt;
        }
        /// <summary>
        /// 获得产品信息
        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="projectID">项目ID</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private DataTable GetProductInfo(SysCommon.DataBase.SysTable pSysDB, long projectID, out Exception eError)
        {
            eError = null;
            //DataTable returnDt = CreateProductTable();
            string str = "SELECT * FROM ProductMDTable where 项目ID=" + projectID;
            DataTable dt = pSysDB.GetSQLTable(str, out eError);
            if (eError != null)
            {
                eError = new Exception("获取产品信息出错！");
                return null;
            }
            return dt;
        }
        /// <summary>
        /// 获得产品信息
        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="projectID"></param>
        /// <param name="dataFormatID"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private DataTable GetProductInfo(SysCommon.DataBase.SysTable pSysDB, long projectID,int dataFormatID, out Exception eError)
        {
            eError = null;
            string str = "SELECT * FROM ProductMDTable where 项目ID=" + projectID + " and 数据格式编号=" + dataFormatID;
            DataTable dt = pSysDB.GetSQLTable(str,out eError);
            if (eError != null)
            {
                eError = new Exception("获取产品信息出错！");
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
        /// 获得产品元信息

        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="productID"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private DataTable GetProductMetaData(SysCommon.DataBase.SysTable pSysDB, long productID,out Exception eError)
        {
            eError = null;
            DataTable returnDt = CreateMetaTable();

            string str = "select * from ProductMDTable where ID=" + productID;
            DataTable dt = pSysDB.GetSQLTable(str,out eError);
            if (eError != null)
            {
                eError = new Exception("获取产品元信息出错！");
                return null;
            }

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                DataRow aRow = returnDt.NewRow();
                aRow["字段"] = dt.Columns[i].ColumnName;
                if (dt.Rows.Count > 0)
                {
                    if (aRow["字段"].ToString().Trim() == "数据格式编号")
                    {
                        if (int.Parse(dt.Rows[0][i].ToString()) == 0)
                        {
                            aRow["值"] = "DLG";
                        }
                        else if (int.Parse(dt.Rows[0][i].ToString()) == 1)
                        {
                            aRow["值"] = "DEM";
                        }
                        else if (int.Parse(dt.Rows[0][i].ToString()) == 2)
                        {
                            aRow["值"] = "DOM";
                        }
                        else if (int.Parse(dt.Rows[0][i].ToString()) == 3)
                        {
                            aRow["值"] = "DRG";
                        }
                    }
                    else 
                    {
                        aRow["值"] = dt.Rows[0][i].ToString();
                    }
                }
                returnDt.Rows.Add(aRow);
            }
            return returnDt;
        }
      
       
        /// <summary>
        /// 获得数据元信息

        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="dataID"></param>
        /// <param name="dataTypeID"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private DataTable GetDataMetaInfo(SysCommon.DataBase.SysTable pSysDB, long dataID,int dataTypeID,out Exception eError)
        {
            eError = null;
             string str="";
             DataTable dt = CreateMetaTable();
            if(dataTypeID==0)
            {
                //标准图幅数据元数据

                str = "select * from StandardMapMDTable where ID=" + dataID;
               DataTable tempdt = pSysDB.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    eError = new Exception("获取标准图幅元信息出错！");
                    return null;
                }
                for (int i = 0; i < tempdt.Columns.Count; i++)
                {
                    DataRow aRow = dt.NewRow();
                    aRow["字段"] = tempdt.Columns[i].ColumnName;
                    if (tempdt.Rows.Count > 0)
                    {
                        if (aRow["字段"].ToString().Trim() == "数据格式编号")
                        {
                            if (int.Parse(tempdt.Rows[0][i].ToString()) == 0)
                            {
                                aRow["值"] = "DLG";
                            }
                            else if (int.Parse(tempdt.Rows[0][i].ToString()) == 1)
                            {
                                aRow["值"] = "DEM";
                            }
                            else if (int.Parse(tempdt.Rows[0][i].ToString()) == 2)
                            {
                                aRow["值"] = "DOM";
                            }
                            else if (int.Parse(tempdt.Rows[0][i].ToString()) == 3)
                            {
                                aRow["值"] = "DRG";
                            }
                        }
                        else
                        {
                            aRow["值"] = tempdt.Rows[0][i].ToString();
                        }
                    }
                    dt.Rows.Add(aRow);
                }
            }
            else if (dataTypeID == 1)
            {
                //非标准图幅数据元数据
                str = "select * from NonstandardMapMDTable where ID=" + dataID;
                DataTable tempdt = pSysDB.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    eError = new Exception("获取非标准图幅元信息出错！");
                    return null;
                }
                for (int i = 0; i < tempdt.Columns.Count; i++)
                {
                    DataRow aRow = dt.NewRow();
                    aRow["字段"] = tempdt.Columns[i].ColumnName;
                    if (tempdt.Rows.Count > 0)
                    {
                        if (aRow["字段"].ToString().Trim() == "数据格式编号")
                        {
                            if (int.Parse(tempdt.Rows[0][i].ToString()) == 0)
                            {
                                aRow["值"] = "DLG";
                            }
                            else if (int.Parse(tempdt.Rows[0][i].ToString()) == 1)
                            {
                                aRow["值"] = "DEM";
                            }
                            else if (int.Parse(tempdt.Rows[0][i].ToString()) == 2)
                            {
                                aRow["值"] = "DOM";
                            }
                            else if (int.Parse(tempdt.Rows[0][i].ToString()) == 3)
                            {
                                aRow["值"] = "DRG";
                            }
                        }
                        else
                        {
                            aRow["值"] = tempdt.Rows[0][i].ToString();
                        }
                    }
                    dt.Rows.Add(aRow);
                }
            }
            else if (dataTypeID == 2)
            {
                //控制点数据元数据
                str = "select * from ControlPointMDTable where ID=" + dataID;
                DataTable tempdt = pSysDB.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    eError = new Exception("获取控制点测量元信息出错！");
                    return null;
                }
                for (int i = 0; i < tempdt.Columns.Count; i++)
                {
                    if (tempdt.Columns[i].ColumnName == "图形数据") continue;
                    DataRow aRow = dt.NewRow();
                    aRow["字段"] = tempdt.Columns[i].ColumnName;
                    if (tempdt.Rows.Count > 0)
                    {
                        if (aRow["字段"].ToString().Trim() == "数据格式编号")
                        {
                            if (int.Parse(tempdt.Rows[0][i].ToString()) == 0)
                            {
                                aRow["值"] = "DLG";
                            }
                            else if (int.Parse(tempdt.Rows[0][i].ToString()) == 1)
                            {
                                aRow["值"] = "DEM";
                            }
                            else if (int.Parse(tempdt.Rows[0][i].ToString()) == 2)
                            {
                                aRow["值"] = "DOM";
                            }
                            else if (int.Parse(tempdt.Rows[0][i].ToString()) == 3)
                            {
                                aRow["值"] = "DRG";
                            }
                        }
                        else
                        {
                            aRow["值"] = tempdt.Rows[0][i].ToString();
                        }
                    }
                    dt.Rows.Add(aRow);
                }
            }
            
            return dt;
        }

        /// <summary>
        /// 根据产品ID获得范围号

        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="productID"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private string GetMapNO(SysCommon.DataBase.SysTable pSysDB, long productID, out Exception eError)
        {
            eError = null;
            string mapNO = "";
            string str = "select distinct 范围号 from ProductMDTable where ID=" + productID;
            DataTable dt = pSysDB.GetSQLTable(str, out eError);
            if (eError != null)
            {
                eError = new Exception("库中不存在产品ID位："+productID+"对应的范围号！");
                return "";
            }
            if(dt.Rows.Count==0)
            {
                eError = new Exception("该产品记录不存在！");
                return "";
            }
            mapNO = dt.Rows[0]["范围号"].ToString();
            if(mapNO=="")
            {
                eError=new Exception("该产品范围信息不存在！");
                return "";
            }
            return mapNO;
        }

        
        #endregion     
       
        private void axTOCControl_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            IBasicMap pMap = null;
            ILayer pLayer = null;
            System.Object other = null;
            System.Object LayerIndex = null;
            System.Drawing.Point pPoint = new System.Drawing.Point(e.x, e.y);

            esriTOCControlItem TOCItem = esriTOCControlItem.esriTOCControlItemNone;
            ITOCControl2 tocControl = (ITOCControl2)axTOCControl.Object;

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
                        //地图右键菜单
                    case esriTOCControlItem.esriTOCControlItemMap:
                        if (_dicContextMenu.ContainsKey("TOCControlContextMenu3"))
                        {
                            if (_dicContextMenu["TOCControlContextMenu3"] != null)
                            {
                                if (_dicContextMenu["TOCControlContextMenu3"].Items.Count > 0)
                                {
                                    item = _dicContextMenu["TOCControlContextMenu3"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                                    if (item != null)
                                    {
                                        item.Popup(axTOCControl.PointToScreen(pPoint));
                                    }
                                }
                            }
                        }
                        break;
                    case esriTOCControlItem.esriTOCControlItemLayer:
                        {
                            //图层右键菜单
                            if (!(pLayer is IGroupLayer || pLayer is IFeatureLayer || pLayer is IDataLayer || pLayer is IDynamicLayer)) return;
                            if (_dicContextMenu.ContainsKey("TOCControlLayerContextMenu3"))
                            {
                                if (_dicContextMenu["TOCControlLayerContextMenu3"] != null)
                                {
                                    if (_dicContextMenu["TOCControlLayerContextMenu3"].Items.Count > 0)
                                    {
                                        item = _dicContextMenu["TOCControlLayerContextMenu3"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                                        if (item != null)
                                        {
                                            item.Popup(axTOCControl.PointToScreen(pPoint));
                                        }
                                    }
                                }
                            }

                            if (axTOCControl.Buddy is IPageLayoutControl2)
                            {
                                IPageLayoutControl2 pPageLayoutControl = axTOCControl.Buddy as IPageLayoutControl2;
                                pPageLayoutControl.CustomProperty = pLayer;

                            }

                            else if (axTOCControl.Buddy is IMapControl3)
                            {
                                IMapControl3 pMapcontrol = axTOCControl.Buddy as IMapControl3;
                                pMapcontrol.CustomProperty = pLayer;
                            }

                        }

                        break;
                }
            }
        }
        #region 获得项目的底图存储路径

        private string GetProjectRangePath(string fieldName,long ProID, SysCommon.DataBase.SysTable pSysTable, out Exception ex)
        {
            ex = null;
            string Path = "";
            string SQL = "SELECT "+fieldName+" FROM ProjectMDTable WHERE ID=" + ProID;
            DataTable table = pSysTable.GetSQLTable(SQL,out ex);
            if (null != ex) return null;
            if (null == table) return null;
            if (table.Rows.Count > 0)
            {
                Path = table.Rows[0][0].ToString();
            }
            return Path;               
        }
        #endregion

        /// <summary>
        /// 加载底图
        /// </summary>
        /// <param name="WorkspacePath"></param>
        /// <param name="pGroupLayer"></param>
        /// <param name="ex"></param>
        private void GetRangeByPath(SysCommon.Gis.SysGisDataSet pSysGis, IGroupLayer pGroupLayer, out Exception ex)
        {
            ex=null;

            try
            {
                IFeatureLayer Fealayer = new FeatureLayerClass();
                IFeatureClass feaclass = pSysGis.GetFeatureClass("ProjectRange", out ex);
                if (null != ex)/////////////////////////////////////////////////////////////郭正 6.7增加
                {
                    ex = new Exception("获取项目范围底图失败！");
                    return;
                }
                Fealayer.FeatureClass = feaclass;
                ICompositeLayer pComLayer = pGroupLayer as ICompositeLayer;
                for (int i = 0; i < pComLayer.Count; i++)
                {
                    ILayer nLayer = pComLayer.get_Layer(i);
                    if (nLayer.Name == "项目底图")
                    {
                        pGroupLayer.Delete(nLayer);
                        break;
                    }
                }

                ILayer Layer = Fealayer as ILayer;
                Layer.Name = "项目底图";
                pGroupLayer.Add(Layer);
                ModDBOperator.SetLableToGeoFeatureLayer(Layer as IGeoFeatureLayer, "NAME", 1000, ModData.v_AppFileDB.MapControl.ReferenceScale);
            }
            catch
            {
                ex = new Exception("获取项目范围底图失败！");
                return;
            }
               
        }
     
        #region 通过产品类型（标准图幅，非标准图幅）生成自定义图层显示条件Condition
        private string GetProductTypeRange(int dataTypeID, long ProductID,SysCommon.DataBase.SysTable pSysTable, out Exception ex)
        {
            ex = null;
            string condition = "范围号=";
            if (0 != dataTypeID && 1 != dataTypeID)
            {
                ex = new Exception("产品类型不正确！");
                return string.Empty;
            }
            string SQL="";
            switch (dataTypeID)
            {
                case 0:
                    SQL = "SELECT 数据文件名 FROM StandardMapMDTable WHERE 产品ID=" + ProductID;
                    break;
                case 1:
                    SQL = "SELECT 数据文件名 FROM NonstandardMapMDTable WHERE 产品ID=" + ProductID;
                    break;
            }
            DataTable table = pSysTable.GetSQLTable(SQL,out ex);
            if (null != ex)
            {
                return string.Empty;
            }
            if (null == table)
            {
                ex = new Exception("获取图幅数据失败!");
                return string.Empty;
            }
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string range = GetDataRangeByName(table.Rows[i]["数据文件名"].ToString(), out ex);
                    if (null != ex)
                    {
                        continue;
                    }
                    condition = condition + "'" + range + "'" + " OR 范围号= ";
                }
                try
                {
                    condition = condition.Substring(0, condition.LastIndexOf('=') - 7);
                    
                }
                catch
                {
                    ex = new Exception("生成显示条件失败!");
                    return string.Empty;
                }
            }
            return condition;
        }
        #endregion
        #region 根据文件名生成范围号
        private string GetDataRangeByName(string FileName, out Exception ex)
        {
            ex = null;
            string Range;
            try
            {
                int end = FileName.LastIndexOf('.');
                Range = FileName.Substring(0, end );
                return Range;
            }
            catch
            {
                ex = new Exception("通过文件名获取范围号失败");
                return null;
            }
        }
        #endregion
        #region 将Datatable中的比例尺、范围号写入Dictionary
        private void WriteConditionDictionary(DataTable table, Dictionary<long, List<string>> RangeDirectory)
        {
            
            if (null != table)
            {
                if (table.Rows.Count > 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        try
                        {
                            if (RangeDirectory.ContainsKey(long.Parse(table.Rows[i][0].ToString())))
                            {
                                RangeDirectory[long.Parse(table.Rows[i][0].ToString())].Add(table.Rows[i][1].ToString());
                            }
                            else
                            {
                                List<string> Slist = new List<string>();
                                Slist.Add(table.Rows[i][1].ToString());
                                RangeDirectory.Add(long.Parse(table.Rows[i][0].ToString()), Slist);
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
           
        }
        #endregion


        /// <summary>
        /// 获得不带比例尺信息的所有的图幅号条件语句

        /// </summary>
        /// <param name="pDataTable"></param>
        /// <returns></returns>
        private string GetRangeNOStr(DataTable pDataTable)
        {
            string str = "";
            for (int i = 0; i < pDataTable.Rows.Count; i++)
            {
                string pRangeScale = pDataTable.Rows[i][0].ToString();
                string pRangeNO = pDataTable.Rows[i][1].ToString();
                //if (pRangeNO.Contains(".")) continue;
                if (pRangeScale == "" || pRangeScale == "0")
                {
                    //没有比例尺信息

                    str += "'" + pRangeNO + "',";
                }
            }
            if (str != "")
            {
                str ="("+ str.Substring(0, str.Length - 1) + ")";
            }

            return str;

        }

        /// <summary>
        /// 根据条件添加图层，不带比例尺信息的图层加载

        /// </summary>
        /// <param name="lstFeaCls"></param>
        /// <param name="condiStr"></param>
        private void AddLayerByCondi(List<IDataset> lstFeaCls, string condiStr, IGroupLayer pGroupLayer)
        {
            foreach (IDataset pDt in lstFeaCls)
            {
                try
                {

                    IFeatureClass pFeaCls = pDt as IFeatureClass;
                    if (pDt.Name == "ProjectRange") continue;
                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = condiStr;
                    IFeatureCursor pCursor = pFeaCls.Search(pFilter, false);
                    if (pCursor == null) continue;
                    IFeature pFeature = pCursor.NextFeature();
                    if (pFeature == null) continue;

                    //释放cursor
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

                    string pFeaClsName = pDt.Name;
                    string pScale = pFeaClsName.Substring(pFeaClsName.LastIndexOf("_") + 1);
                    if (pFeaCls == null) continue;
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    pFeatureLayer.FeatureClass = pFeaCls;

                    ILayer mLayer = pFeatureLayer as ILayer;
                    mLayer.Name = pFeaClsName;
                    ICompositeLayer pCompLayer = pGroupLayer as ICompositeLayer;
                    for (int i = 0; i < pCompLayer.Count; i++)
                    {
                        if (pCompLayer.get_Layer(i).Name == mLayer.Name)
                        {
                            pGroupLayer.Delete(pCompLayer.get_Layer(i));
                            break;
                        }
                    }
                    pGroupLayer.Add(mLayer);
                    ModData.v_AppFileDB.MapControl.ReferenceScale = Convert.ToDouble(pScale);
                    ModDBOperator.SetLableToGeoFeatureLayer(pFeatureLayer as IGeoFeatureLayer, "MAP_NEWNO", Convert.ToInt32(pScale), ModData.v_AppFileDB.MapControl.ReferenceScale);


                    //自定义显示

                    IFeatureLayerDefinition pLayerDef = pFeatureLayer as IFeatureLayerDefinition;
                    pLayerDef.DefinitionExpression = condiStr;
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 带比例尺信息的图幅号条件语句
        /// </summary>
        /// <param name="drs"></param>
        /// <returns></returns>
        private Dictionary<string,List<string>> GetRangeNODic(DataRow[] drs)
        {
            Dictionary<string, List<string>> RangeDirectory = new Dictionary<string, List<string>>();
            for (int i = 0; i < drs.Length ; i++)
            {
                if (RangeDirectory.ContainsKey(drs[i][0].ToString()))
                {
                    if (!RangeDirectory[drs[i][0].ToString()].Contains(drs[i][1].ToString()))
                    {
                        RangeDirectory[drs[i][0].ToString()].Add(drs[i][1].ToString());
                    }
                    
                }
                else
                {
                    List<string> Slist = new List<string>();
                    Slist.Add(drs[i][1].ToString());
                    RangeDirectory.Add(drs[i][0].ToString(), Slist);
                }
            }
            return RangeDirectory;
        }

        /// <summary>
        /// 加载图层，带比例尺信息的图层加载
        /// </summary>
        /// <param name="RangeDirectory"></param>
        /// <param name="eError"></param>
        private void AddLayerByCondi(Dictionary<string, List<string>> RangeDirectory,string preFeaClsName,IGroupLayer pGroupLayer1,SysCommon.Gis.SysGisDataSet pSysGISDT,out Exception eError)
        {
            eError = null;
            if (pSysGISDT == null) return;
            foreach (KeyValuePair<string, List<string>> item in RangeDirectory)
            {
                string pScale = item.Key;
                string feaClsName = preFeaClsName + pScale;
                IFeatureClass pFeaCls = pSysGISDT.GetFeatureClass(feaClsName, out eError);
                if (eError != null)
                {
                    eError=new Exception("找不到图层'" + feaClsName + "'，请检查！");
                    continue;
                }
                List<string> list = item.Value;

                string str1 = "";
                foreach (string str in list)
                {
                    str1 += "'" + str + "',";
                }

                str1 = str1.Substring(0, str1.Length - 1);
                string condition = "MAP_NEWNO in (" + str1 + ")";
                LayerDefShow(condition, pFeaCls, pGroupLayer1);
            }
        }

        /// <summary>
        /// 自定义显示加载的图层
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="pFeatureLayer"></param>
        private void LayerDefShow(string condition, IFeatureClass pFeatureCls,IGroupLayer pGroupLayer)
        {
            try
            {
                IFeatureLayer pFeaLayer = new FeatureLayerClass();
                pFeaLayer.FeatureClass = pFeatureCls;
                ILayer mLayer = pFeaLayer as ILayer;
                mLayer.Name = (pFeatureCls as IDataset).Name;

                ICompositeLayer pCompLayer = pGroupLayer as ICompositeLayer;
                for (int i = 0; i < pCompLayer.Count; i++)
                {
                    if (pCompLayer.get_Layer(i).Name == mLayer.Name)
                    {
                        pGroupLayer.Delete(pCompLayer.get_Layer(i));
                        break;
                    }
                }
                //////
                ILayerEffects EffLayer = mLayer as ILayerEffects;
                if (EffLayer.SupportsTransparency)
                    EffLayer.Transparency = 30;
                /////
                pGroupLayer.Add(mLayer);
                string pScale = (pFeatureCls as IDataset).Name.Substring((pFeatureCls as IDataset).Name.LastIndexOf("_") + 1);
                double dScale = Convert.ToDouble(pScale);
                ModData.v_AppFileDB.MapControl.ReferenceScale = dScale;
                ModDBOperator.SetLableToGeoFeatureLayer(pFeaLayer as IGeoFeatureLayer, "MAP_NEWNO", Convert.ToInt32(pScale), ModData.v_AppFileDB.MapControl.ReferenceScale );
                //自定义显示

                IFeatureLayerDefinition pLayerDef = pFeaLayer as IFeatureLayerDefinition;
                pLayerDef.DefinitionExpression = condition;

            }
            catch
            { }
        }

        /// <summary>
        /// 画一个控制点(CAD图的X坐标与ArcGIS图的坐标是反的)
        /// </summary>
        /// <param name="pGra"></param>
        /// <param name="x">cadX坐标</param>
        /// <param name="y">cady坐标</param>
        /// <param name="pointName">控制点名</param>
        private void MakePointSymbol(IGraphicsContainer pGra, double x, double y, string pointName, long conScale)
        {
            IActiveView pAcitveView = pGra as IActiveView;
            IElement pEle;
            //画点
            ESRI.ArcGIS.Geometry.IPoint pPint = new ESRI.ArcGIS.Geometry.PointClass();
            pPint.PutCoords(y, x);//坐标取反
            IRgbColor pColor = new RgbColorClass();
            pColor.Blue = 255;
            pColor.Green = 0;
            pColor.Red = 130;


            // ITopologicalOperator pTopo = pPint as ITopologicalOperator;
            //IGeometry  pGeo = pTopo.Buffer(5);
            //pTopo = pGeo as ITopologicalOperator;
            // pTopo.Simplify();
            // pGeo = pTopo as IGeometry;
            //pPint = pTopo as IPoint;

            double vMapFrameScale = double.Parse(conScale.ToString());
            double vMapRefrenceScale = double.Parse(conScale.ToString());
            ModData.v_AppFileDB.MapControl.Map.ReferenceScale = double.Parse(conScale.ToString());

            IMarkerElement pMakEle = new MarkerElementClass();
            pEle = pMakEle as IElement;
            ISimpleMarkerSymbol pMakSym = new SimpleMarkerSymbolClass();
            if (vMapRefrenceScale != 0 && vMapFrameScale != 0)
            {
                double size = (vMapFrameScale / 30) * vMapFrameScale / vMapRefrenceScale;
                pMakSym.Size = size*10;
            }
            //IDisplayName pDisName = pMakSym as IDisplayName;
            //pDisName.NameString = pointName;
            pMakSym.Color = pColor;
            pMakEle.Symbol = pMakSym;
            pMakSym.Style=esriSimpleMarkerStyle.esriSMSCircle;
            pEle.Geometry = pPint as ESRI.ArcGIS.Geometry.IGeometry;

            pGra.AddElement(pEle, 0);
            pAcitveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            ESRI.ArcGIS.Geometry.IPoint pPint2 = new ESRI.ArcGIS.Geometry.PointClass();
            pPint2.PutCoords(y - 50, x + 50);

            ITextElement pTextElement = new TextElementClass();
            ITextSymbol pTextSymbol = new TextSymbolClass();

            stdole.StdFont pStdFont = new stdole.StdFontClass();
            stdole.IFontDisp pFont = (stdole.IFontDisp)pStdFont;
            pFont.Name = "宋体";
            if (vMapRefrenceScale != 0 && vMapFrameScale != 0)
            {
                //double size = (vMapFrameScale / 30) * vMapFrameScale / vMapRefrenceScale;
                double size = (vMapFrameScale /vMapRefrenceScale)*16;
                pFont.Size = (decimal)size;
            }
            pTextSymbol.Font = pFont;
            pTextSymbol.Color = pColor;

            pTextElement.Symbol = pTextSymbol;
            pTextElement.ScaleText = true;
            pTextElement.Text = pointName;
            pEle = pTextElement as IElement;
            pEle.Geometry = pPint2 as ESRI.ArcGIS.Geometry.IGeometry;

            pGra.AddElement(pEle, 0);
            pAcitveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        /// <summary>
        /// 加载控制点图
        /// </summary>
        /// <param name="scpTable"></param>
        private void ShowAllPoint(IGraphicsContainer pGra,DataTable scpTable,long conScale)
        {
            for (int i = 0; i < scpTable.Rows.Count; i++)
            {
                double pointX = 0;               //x坐标
                double pointY = 0;               //y坐标
                string pointName = scpTable.Rows[i]["控制点名"].ToString().Trim();
                if (scpTable.Rows[i]["X坐标"].ToString().Trim() == "" || scpTable.Rows[i]["Y坐标"].ToString().Trim() == "")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "属性表中未填写坐标信息,请检查！");
                    return;
                }
                if (scpTable.Rows[i]["X坐标"].ToString().Trim() != "")
                {
                    pointX = Convert.ToDouble(scpTable.Rows[i]["X坐标"].ToString().Trim());
                }
                if (scpTable.Rows[i]["Y坐标"].ToString().Trim() != "")
                {
                    pointY = Convert.ToDouble(scpTable.Rows[i]["Y坐标"].ToString().Trim());
                }

                MakePointSymbol(pGra, pointX, pointY, pointName,conScale);
            }
        }

        /// <summary>
        /// 缩放到Feature
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="pFeature"></param>
        private void ZoomToFeature(IMapControlDefault pMapControl, IFeature pFeature)
        {
            if (pFeature == null) return;
            if (pFeature.Shape == null) return;
            IEnvelope pEnvelope = null;
            if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                ITopologicalOperator pTop = pFeature.Shape as ITopologicalOperator;
                IGeometry pGeometry = pTop.Buffer(50);
                pEnvelope = pGeometry.Envelope;
            }
            else
            {
                pEnvelope = pFeature.Extent;
            }

            if (pEnvelope == null) return;
            pEnvelope.Expand(1.5, 1.5, true);
            IActiveView pActiveView = pMapControl.Map as IActiveView;
            pActiveView.Extent = pEnvelope;
            pActiveView.Refresh();
        }

        private void axTOCControl_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        {
            IBasicMap pMap = null;
            ILayer pLayer = null;
            System.Object other = null;
            System.Object LayerIndex = null;
            System.Drawing.Point pPoint = new System.Drawing.Point(e.x, e.y);

            esriTOCControlItem TOCItem = esriTOCControlItem.esriTOCControlItemNone;
            ITOCControl2 tocControl = (ITOCControl2)axTOCControl.Object;

            tocControl.HitTest(e.x, e.y, ref TOCItem, ref pMap, ref pLayer, ref other, ref LayerIndex);
            if (e.button == 1)
            {
                if (TOCItem == esriTOCControlItem.esriTOCControlItemLayer && pLayer is IGroupLayer)
                {
                    if (m_bGroupLayerVisible != pLayer.Visible)
                    {
                        IMapControlDefault pMapcontrol = axTOCControl.Buddy as IMapControlDefault;
                        pMapcontrol.ActiveView.Refresh();
                    }
                }
            }
        }
    }
        
}