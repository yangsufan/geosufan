using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using GeoDataCenterFunLib;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using SysCommon;
using System.Diagnostics;
//using Microsoft.Office.Tools.Word;

using SysCommon.Error;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Display;
using GeoProperties;
//using ESRI.ArcGIS.Controls;
//using ESRI.ArcGIS.Geometry; 
//using GeoProperties;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;



namespace GeoDataManagerFrame
{
    public partial class SetControl : UserControl
    {
        // private Dictionary<string, DevComponents.DotNetBar.ContextMenuBar> _dicContextMenu;
        //  public IFeatureWorkspace m_pFeatureWorkspace;
        public static TreeNode m_tparent;       //地图树根节点
        public TreeNode m_tTextparent;   //文档树根节点
        public TreeNode m_selectnode; //选择节点调阅
        public string m_SaveXmlFileName = "";
        public string m_strLogFilePath = Application.StartupPath + "\\..\\Log\\DataCenterLog.txt";
        private static object docPath;//临时doc文件
        private Microsoft.Office.Interop.Word.Application wordApp;//word应用
        private Microsoft.Office.Interop.Word.Document doc;//word文档
        private static List<int> wordProcess = new List<int>();//word进程集
        private TreeNode m_LastDragNode = null;
        private short m_transpy = 0;//透明度 
        private double m_scale = 0;//比例尺
        private TreeNode m_CurEditLayerNode; 
        private TreeNode m_CurEditTopicNode;
        private bool XzqLoad=false;
        private bool FeatureTrue = true;//判断是不是矢量数据图层2011.0414 xs
        private　bool flag = true;
        private bool flag2 = true;
        private string m_strnew = "";//判断是否后来添加的图层

        //地图浏览工具栏容器
        private Control _MapToolControl;
        private double TempX;
        private double TempY;
        System.Drawing.Point pPoint = new System.Drawing.Point();
        private IGeometry MGeometry = null;//在mapctrl上画的范围，行政区和图幅跳转
        public SetControl(string strName, string strCation)
        {
            InitializeComponent();

            //初始化配置对应视图控件
            InitialMainViewControl();
            this.Dock = System.Windows.Forms.DockStyle.Fill;

            this.Name = strName;
            this.Tag = strCation;

            //传递参数
            ModFrameData.v_AppGisUpdate.MainUserControl = this;
            ModFrameData.v_AppGisUpdate.ArcGisMapControl = axMapControl;
         //   ModFrameData.v_AppGisUpdate.TOCControl = axTOCControl.Object as ITOCControlDefault;
            ModFrameData.v_AppGisUpdate.MapControl = axMapControl.Object as IMapControlDefault;
            ModFrameData.v_AppGisUpdate.CurrentControl = axMapControl.Object;
            ModFrameData.v_AppGisUpdate.MapDocTree = MapDocTree;
          //  ModFrameData.v_AppGisUpdate.TextDocTree = TextDoctree;
          //  ModFrameData.v_AppGisUpdate.DataIndexTree = DataIndexTree;
            ModFrameData.v_AppGisUpdate.DataUnitTree = DataUnitTree;
            // ModFrameData.v_AppGisUpdate.UserResultTree = UserResultTree;
            ModFrameData.v_AppGisUpdate.Visible = this.Visible;
            ModFrameData.v_AppGisUpdate.Enabled = this.Enabled;
            ModFrameData.v_AppGisUpdate.CurrentThread = null;
            ModFrameData.v_AppGisUpdate.tipRichBox = tipRichBox;

          //  ModFrameData.v_AppGisUpdate.DocControl = RichBoxWordDoc;
            //日志文件路径
            ModFrameData.v_AppGisUpdate.strLogFilePath = m_strLogFilePath;
            ModFrameData.v_AppGisUpdate.IndextabControl = IndextabControl;


            //根据sys配置文件添加菜单和工具栏
            InitialFrmDefControl();
            InitOutPutResultTree();
            DataUnitTree.ImageList = imageList;

            (axMapControl.ActiveView as IActiveViewEvents_Event).AfterDraw += new IActiveViewEvents_AfterDrawEventHandler(SetControl_mapctrl_afterdraw);
      
        }
        //初始化用户成果列表
        public void InitOutPutResultTree()
        {
            treeViewOutPutResults.Nodes.Clear();
            string strFilePath = Application.StartupPath + "\\..\\Res\\Xml\\OutputResultsTreeIndex.Xml";
            CreatOutPutTree(treeViewOutPutResults, strFilePath);

        }
  
        public void CreatOutPutTree(TreeView toTreeView, string strXmlPath)
        {
            TreeNode tparent;
            tparent = new TreeNode();
            tparent.Text = "列表";
            tparent.Tag = 0;
            tparent.ImageIndex = 13;
            tparent.SelectedImageIndex = 13;
            toTreeView.Nodes.Add(tparent);
            toTreeView.ExpandAll();


            //加入子节点
            TreeNode tNewNode;
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();

            //遍历获取itemName信息 
            string strTblName = "";
            string strTblPath = "";
            XmlDocument xmldoc = new XmlDocument();
            if (xmldoc != null)
            {
                if (File.Exists(strXmlPath))
                {
                    xmldoc.Load(strXmlPath);

                    //修改根节点节点名称
                    string strRootName = "";
                    string strSearchRoot = "//Rootset";
                    string strRootPath = "";
                    XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);
                    XmlElement xmlElentRoot = (XmlElement)xmlNodeRoot;
                    strRootName = xmlElentRoot.GetAttribute("Caption");
                    strRootPath = xmlElentRoot.GetAttribute("Path");
                    tparent.Text = strRootName;
                    tparent.Name = strRootPath;
                    //首先添加第一级子节点 Childset
                    string strSearch = "//Childset";
                    XmlNode xmlNode = xmldoc.SelectSingleNode(strSearch);
                    XmlNodeList xmlNdList;
                    xmlNdList = xmlNode.ChildNodes;
                    foreach (XmlNode xmlChild in xmlNdList)
                    {
                        strTblName = "";
                        XmlElement xmlElent = (XmlElement)xmlChild;
                        strTblName = xmlElent.GetAttribute("Caption");
                        strTblPath = xmlElent.GetAttribute("Path");
                        tNewNode = new TreeNode();
                        tNewNode.Text = strTblName;
                        tNewNode.Name = strTblPath;
                        tNewNode.Tag = 1;
                        tNewNode.ImageIndex = 11;
                        tNewNode.SelectedImageIndex = 12;
                        tparent.Nodes.Add(tNewNode);
                        tparent.ExpandAll();

                        //添加最终子节点
                        AddLeafItemFromFile(tNewNode, xmlChild);
                    }
                }
            }

        }
        public void AddLeafItemFromFile(TreeNode treeNode, XmlNode xmlNode)
        {
            string path;
            path = Application.StartupPath + "\\..\\" + treeNode.Parent.Name + "\\" + treeNode.Name;
            if (Directory.Exists(path))
            {
                TreeNode tChildNode, tChildChildNode;
               // string strTblName = "";
                DirectoryInfo Dinfo = new DirectoryInfo(path);
                foreach (DirectoryInfo eachinfo in Dinfo.GetDirectories())
                {
                    tChildNode = new TreeNode();
                    tChildNode.Text = eachinfo.Name;
                    tChildNode.Name = eachinfo.FullName;
                    tChildNode.ImageIndex = 11;
                    tChildNode.SelectedImageIndex = 12;
                    treeNode.Nodes.Add(tChildNode);
                    foreach (FileInfo Finfo in eachinfo.GetFiles("*.cel"))
                    {
                        tChildChildNode = new TreeNode();
                        tChildChildNode.Text = Finfo.Name.Substring(0,Finfo.Name.IndexOf("."));
                        tChildChildNode.Name = Finfo.FullName;
                        tChildChildNode.ImageIndex = 15;
                        tChildChildNode.SelectedImageIndex =15;
                        tChildNode.Nodes.Add(tChildChildNode);
                    }
                    foreach (FileInfo Finfo in eachinfo.GetFiles("*.mdb"))
                    {
                        tChildChildNode = new TreeNode();
                        tChildChildNode.Text = Finfo.Name.Substring(0, Finfo.Name.IndexOf("."));
                        tChildChildNode.Name = Finfo.FullName;
                        tChildChildNode.ImageIndex = 18;
                        tChildChildNode.SelectedImageIndex = 18;
                        tChildNode.Nodes.Add(tChildChildNode);
                    }
                    foreach (FileInfo Finfo in eachinfo.GetFiles("*.mxd"))
                    {
                        tChildChildNode = new TreeNode();
                        tChildChildNode.Text = Finfo.Name.Substring(0, Finfo.Name.IndexOf("."));
                        tChildChildNode.Name = Finfo.FullName;
                        tChildChildNode.ImageIndex = 17;
                        tChildChildNode.SelectedImageIndex = 17;
                        tChildNode.Nodes.Add(tChildChildNode);
                    }
                }
            }

        }
        public void CreatTreeFromXmlFile(TreeView toTreeView, string strXmlPath)
        {
            TreeNode tparent;
            tparent = new TreeNode();
            tparent.Text = "列表";
            tparent.Tag = 0;
            tparent.ImageIndex = 13;
            tparent.SelectedImageIndex = 13;
            toTreeView.Nodes.Add(tparent);
            toTreeView.ExpandAll();


            //加入子节点
            TreeNode tNewNode;
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();

            //遍历获取itemName信息 
            string strTblName = "";
            XmlDocument xmldoc = new XmlDocument();
            if (xmldoc != null)
            {
                if (File.Exists(strXmlPath))
                {
                    xmldoc.Load(strXmlPath);

                    //修改根节点节点名称
                    string strRootName = "";
                    string strSearchRoot = "//Rootset";
                    XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);
                    XmlElement xmlElentRoot = (XmlElement)xmlNodeRoot;
                    strRootName = xmlElentRoot.GetAttribute("sItemName");
                    tparent.Text = strRootName;

                    //首先添加第一级子节点 Childset
                    string strSearch = "//Childset";
                    XmlNode xmlNode = xmldoc.SelectSingleNode(strSearch);
                    XmlNodeList xmlNdList;
                    xmlNdList = xmlNode.ChildNodes;
                    foreach (XmlNode xmlChild in xmlNdList)
                    {
                        strTblName = "";
                        XmlElement xmlElent = (XmlElement)xmlChild;
                        strTblName = xmlElent.GetAttribute("Caption");

                        tNewNode = new TreeNode();
                        tNewNode.Text = strTblName;
                        tNewNode.Tag = 1;
                        tNewNode.ImageIndex = 17;
                        tNewNode.SelectedImageIndex = 17;
                        tparent.Nodes.Add(tNewNode);
                        tparent.ExpandAll();

                        //添加最终子节点
                        AddLeafItem(tNewNode, xmlChild);
                    }
                }
            }
        }
        //添加叶子节点
        public void AddLeafItem(TreeNode treeNode, XmlNode xmlNode)
        {
            if (treeNode != null && xmlNode != null)
            {
                TreeNode tNewNode;
                string strTblName = "";

                XmlNodeList xmlNdList;
                xmlNdList = xmlNode.ChildNodes;
                foreach (XmlNode xmlChild in xmlNdList)
                {
                    strTblName = "";
                    XmlElement xmlElent = (XmlElement)xmlChild;
                    strTblName = xmlElent.GetAttribute("Caption");
                    tNewNode = new TreeNode();
                    tNewNode.Text = strTblName;
                    tNewNode.Tag = 2;
                    tNewNode.ImageIndex = 17;
                    tNewNode.SelectedImageIndex = 17;
                    treeNode.Nodes.Add(tNewNode);
                }
                treeNode.ExpandAll();
            }
        }



        private void InitialMainViewControl()
        {
            frmBarManager newfrmBarManager = new frmBarManager();
            newfrmBarManager.TopLevel = false;
            newfrmBarManager.Dock = DockStyle.Fill;
            newfrmBarManager.Show();
            this.Controls.Add(newfrmBarManager);


            //加载设置数据索引窗口
            DevComponents.DotNetBar.Bar barIndexView = newfrmBarManager.CreateBar("barIndexView", enumLayType.FILL);
            barIndexView.CanHide = false;
            DevComponents.DotNetBar.PanelDockContainer PanelIndexView = newfrmBarManager.CreatePanelDockContainer("PanelIndexView", barIndexView);
            PanelIndexView.Controls.Add(this.IndextabControl);
            this.IndextabControl.Dock = DockStyle.Fill;


            //加载设置视图窗口
     /*       DevComponents.DotNetBar.Bar barMapView = newfrmBarManager.CreateBar("barMapView", enumLayType.FILL);
            barMapView.CanHide = false;
            DevComponents.DotNetBar.PanelDockContainer PanelMapView = newfrmBarManager.CreatePanelDockContainer("PanelMapView", barMapView);
            PanelMapView.Controls.Add(this.panelCenterMain);
            this.panelCenterMain.Dock = DockStyle.Fill;
*/

            //加载设置视图窗口
            DevComponents.DotNetBar.Bar barMapView = newfrmBarManager.CreateBar("barMapView", enumLayType.FILL);
            DevComponents.DotNetBar.PanelDockContainer PanelMap = newfrmBarManager.CreatePanelDockContainer("PanelMapView", barMapView);
          //  DockContainerItem MapContainerItem = newfrmBarManager.CreateDockContainerItem("TreeContainerItem", "数据视图", PanelMap, barMapView);
            PanelMap.Controls.Add(this.panelCenterMain);
            this.panelCenterMain.Dock = DockStyle.Fill;
            _MapToolControl = PanelMap as Control;

            //布局设置
            newfrmBarManager.MainDotNetBarManager.FillDockSite.GetDocumentUIManager().Dock(barIndexView, barMapView, eDockSide.Right);
            newfrmBarManager.MainDotNetBarManager.FillDockSite.GetDocumentUIManager().SetBarWidth(barIndexView, this.Width / 5);

            //加载设置提示窗体
            //用户组配置
            PanelDockContainer PanelTipData = new PanelDockContainer();
            PanelTipData.Controls.Add(this.tipRichBox);
            this.tipRichBox.ContextMenuStrip = contextMenuLog;
            this.tipRichBox.Dock = DockStyle.Fill;
            DockContainerItem dockItemData = new DockContainerItem("dockItemData", "提示");
            dockItemData.Control = PanelTipData;
            newfrmBarManager.ButtomBar.Items.Add(dockItemData);
        }


        //初始化框架插件控件界面
        //根据sys配置文件添加菜单和工具栏
        private void InitialFrmDefControl()
        {
            //得到Xml的System节点,根据XML加载插件界面
            string xPath = ".//System[@Name='" + this.Name + "']";
            ModFrameData.v_AppGisUpdate.ScaleBoxList = new List<ComboBoxItem>();
            Plugin.ModuleCommon.LoadButtonViewByXmlNode(ModFrameData.v_AppGisUpdate.ControlContainer, xPath, ModFrameData.v_AppGisUpdate);

            //初始化地图浏览工具栏
            Plugin.Application.IAppFormRef pAppFrm = ModFrameData.v_AppGisUpdate as Plugin.Application.IAppFormRef;
            XmlNode barXmlNode = pAppFrm.SystemXml.SelectSingleNode(".//ToolBar[@Name='ControlMapToolBar8']");
            if (barXmlNode == null || _MapToolControl == null) return;
            //DevComponents.DotNetBar.Bar mapToolBar = Plugin.ModuleCommon.LoadButtonView(_MapToolControl, barXmlNode, pAppFrm, null, false) as Bar;
            DevComponents.DotNetBar.Bar mapToolBar = Plugin.ModuleCommon.LoadButtonView(_MapToolControl, barXmlNode, pAppFrm, null) as Bar;
            if (mapToolBar != null)
            {
                mapToolBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
                mapToolBar.DockOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
                mapToolBar.DockSide = DevComponents.DotNetBar.eDockSide.Left;
                mapToolBar.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
                mapToolBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            }
        }




        //数据单元数节点被左键点击响应
        //根据数据单元树信息更新资源目录树
        //目前只针对级别为3的节点（县级节点）有响应，后续可以考虑对市级节点响应，但是会存在资源目录列表内容太多的情况
        private void DataUnitTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (DataUnitTree.SelectedNode != e.Node)
            {
                if (DataUnitTree.SelectedNode != null)
                {
                    DataUnitTree.SelectedNode.ForeColor = Color.Black;
                }

                DataUnitTree.SelectedNode = e.Node;
                e.Node.ForeColor = Color.Red;

                string strItemName = DataUnitTree.SelectedNode.Name;
                string strItemText = DataUnitTree.SelectedNode.Text;

                //=================================
                //作者：席胜 
                //时间：2011-02-21
                //说明：判断节点级别
                //=================================
                //DataUnitTree.SelectedImageIndex = null;
                switch (DataUnitTree.SelectedNode.Level.ToString())
                {
                    case "0":
                        DataUnitTree.SelectedNode.SelectedImageIndex = 0;
                        break;
                    case "1":
                        DataUnitTree.SelectedNode.SelectedImageIndex = 17;
                        break;
                    case "2":
                        DataUnitTree.SelectedNode.SelectedImageIndex = 17;
                        break;
                    case "3":
                        DataUnitTree.SelectedNode.SelectedImageIndex = 17;
                        break;
                    default:
                        break;

                }
                DataUnitTree.Refresh();

                //根据点选节点 更新 AxMapControl 窗口
                //?     UpdateMapControl(strItemName,strItemText);

                if (DataUnitTree.SelectedNode.Level.Equals(2))//只针对县级节点
                {
                    DataUnitTree.SelectedNode.ExpandAll();//展开所有子树节点
                }
            }

            //右键点击的时候弹出右键菜单
            if (e.Button == MouseButtons.Right)
            {

                System.Drawing.Point ClickPoint = DataUnitTree.PointToScreen(new System.Drawing.Point(e.X, e.Y));
                TreeNode tSelNode;
                tSelNode = e.Node;
                if (tSelNode != null)
                {

                    if (tSelNode.Level == 2)//行政区节点
                        DataUnitcontextMenu.Show(ClickPoint);
                    else if (tSelNode.Level == 4)//叶子节点
                    {
                        DataIndexTreecontextMenu.Show(ClickPoint);
                        MenuItemExport.Visible = false;
                        MenuItemAddLoadData.Visible = true;
                        MenuItemAtt.Visible = true;
                        MenuItemLoadData.Visible = true;
                        m_selectnode = e.Node;
                    }
                    else if (tSelNode.Level == 3)//专题节点
                    {
                        DataIndexTreecontextMenu.Show(ClickPoint);
                        MenuItemExport.Visible = true;
                        MenuItemAddLoadData.Visible = false;
                        MenuItemAtt.Visible = false;
                        MenuItemLoadData.Visible = false;
                    }

                }
            }
        }

        //根据点选节点 更新 AxMapControl 窗口
        public void UpdateMapControl(string strXzqCode, string strXzqName)
        {
            if (strXzqCode.Equals(""))
                return;

            //加入没有调阅数据
            if (MapDocTree.GetNodeCount(true) > 0)
                return;

            axMapControl.ActiveView.Clear();

            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 脚本文件 from 数据单元表 where 行政代码 = '" + strXzqCode + "'";
            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
            string strIndexName = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //从workspace中获取对应的layer
            string dbpath = dIndex.GetDbValue("dbServerPath");

            IWorkspaceFactory2 rasterWorkspaceFactory = new FileGDBWorkspaceFactoryClass();

            IRasterWorkspaceEx rasterWorkspace = rasterWorkspaceFactory.OpenFromFile(dbpath, 0) as IRasterWorkspaceEx;

            //??要判断文件是否存在
            IWorkspace2 pWorkspace2 = rasterWorkspace as IWorkspace2;
            if (pWorkspace2.get_NameExists(esriDatasetType.esriDTRasterDataset, strIndexName))
            {
                IRasterDataset rasterDataset = rasterWorkspace.OpenRasterDataset(strIndexName);
                IRasterLayer pRasterLayer = new RasterLayerClass();
                pRasterLayer.CreateFromDataset(rasterDataset);
                //axMapControl.Map.AddLayer(pRasterLayer);
                IMapLayers pmaplayers = axMapControl.Map as IMapLayers;
                pmaplayers.InsertLayer(pRasterLayer, false, pmaplayers.LayerCount);
            }
            else
            {
                axMapControl.ActiveView.Clear();
                // MessageBox.Show("数据单元【" + strXzqName + "】对应的索引文件不存在", "系统提示");   
            }

            axMapControl.ActiveView.Refresh();
        }

        ////初始化数索引树 
        //public void InitDataIndexTree(string strXzqCode, string strXzqName)
        //{
        //    GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
        //    string mypath = dIndex.GetDbInfo();
        //    string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串

        //    //根据行政代码从地图入库信息表中获取对应的入库信息
        //    CreateDataIndexTree(strCon, strXzqCode, strXzqName);
        //}

  

        //调阅数据 菜单响应
        private void MenuItemLoadData_Click(object sender, EventArgs e)
        {

            // TreeNode tCurNode = DataIndexTree.SelectedNode;
            if (m_selectnode == null) return;
            TreeNode tCurNode = m_selectnode;
            //获取专题类型
            string strSubType = tCurNode.Tag.ToString();
            //初始化进度条
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();


            //获取模板路径 
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            //  string mypath = dIndex.GetDbValue("dbServerPath");
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 脚本文件 from 标准专题信息表 where 专题类型 ='" + strSubType + "'";

            //根据专题类型从标准专题信息表 中获取对应的模板文件路径
            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
            string strModpath = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //获取模板路径
            string strModFile = Application.StartupPath + "\\..\\Template\\" + strModpath;

            if (!File.Exists(strModFile))
            {
                vProgress.Close();
                MessageBox.Show("脚本文件不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //复制到Temp文件夹中
            string strWorkFile = Application.StartupPath + "\\..\\Temp\\CurPrj.xml";
            m_SaveXmlFileName = strWorkFile;
            File.Copy(strModFile, strWorkFile, true);

            //初始化地图文档树
            string strBuffer = tCurNode.Text;
            TreeNode tParent = tCurNode.Parent;
            TreeNode tRoot = tParent.Parent;
            string strCodeUnitName = tRoot.Text;
            string strSuffixx = strCodeUnitName + strBuffer; //后缀 

            //记录日志
            LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
            string strLog = "开始调阅" + tCurNode.Parent.Text + "_" + strSuffixx;
            if (log != null)
            {
                log.Writelog(strLog);
            }
            vProgress.SetProgress(strLog);

            //获取数据行政代码（数据单元代码）
            string strCodeUnitCode = tRoot.Name;

            //获取年度
            string strYear = strBuffer.Substring(0, 4);

            //获取比例尺代码
            int iStartPos = strBuffer.IndexOf("【");
            int iEndPos = strBuffer.IndexOf("】");
            int iLength = iEndPos - iStartPos - 1;
            string strScaleName = strBuffer.Substring(iStartPos + 1, iLength);
            strExp = "select 代码 from 比例尺代码表 where 描述 ='" + strScaleName + "'";
            string strScaleCode = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //清空前次加载的图层
            axMapControl.Map.ClearLayers();

            //初始化地图文档树  数据单元代码   年度  比例尺  专题类型
            InitMapDocTree(strWorkFile, strSuffixx, strCodeUnitCode, strYear, strScaleCode, strSubType, vProgress);

            vProgress.SetProgress("初始化文档树");
            //初始化文档树
            InitTextDocTree(strWorkFile, strSuffixx, strCodeUnitCode, strYear, strScaleCode, strSubType, vProgress);

            //初始化TOC控件  wgf
            //?   InitTocTree(strWorkFile, strSuffixx, strCodeUnitCode, strYear, strScaleCode, strSubType);

            vProgress.Close();
            if (!XzqLoad)//不是行政区调阅
            {
                //索引树跳转到地图文档窗口
                IndextabControl.SelectedTab = PageMapDoc;

                //视图窗口跳转到图形浏览窗口
            //    CenterTabControl.SelectedTab = MapPage;
            }
            InitXZQTree();
            InitJHTBTree();
        }
        //yjl调阅数据后根据XZQ层和数据单元初始化右边的行政区树
        private void InitXZQTree()
        {
            IMap mMap = axMapControl.Map;
            TreeNode[] mNode = MapDocTree.Nodes.Find("行政区",true);
            if (mNode.Length == 0)
                return;
            TreeNode city=DataUnitTree.Nodes[0].Nodes[0];
            treeViewXZQ.Nodes.Add(city.Name,city.Text,18,19);
            city = DataUnitTree.Nodes[0].Nodes[0].Nodes[0];
            treeViewXZQ.Nodes[0].Nodes.Add(city.Name, city.Text, 18, 19);


            TreeNode mTN = mNode[0];
            
            IFeatureLayer mFL = GetLayerofTreeNode(mTN) as IFeatureLayer;//call sub proc
            IFeatureClass mFC = mFL.FeatureClass;
            IFeatureCursor mFCs = mFC.Search(null, false);
            IFeature mF = mFCs.NextFeature();
            int fdXZQMC = mFC.Fields.FindField("XZQMC");
            int fdXZQDM = mFC.Fields.FindField("XZQDM");
            while (mF != null)
            {
                try
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = mF.get_Value(fdXZQMC).ToString();
                    tn.Name = mF.get_Value(fdXZQDM).ToString();
                    tn.ImageIndex = 18;
                    tn.SelectedImageIndex = 19;
                    treeViewXZQ.Nodes[0].Nodes[0].Nodes.Add(tn);
                }
                catch
                {
                    
                }
                mF = mFCs.NextFeature();
            }
            treeViewXZQ.ExpandAll();

            
 
        }
        //yjl调阅数据后初始化右边的接合图表树
        private void InitJHTBTree()
        {
            IMap mMap = axMapControl.Map;
            TreeNode[] mNode = MapDocTree.Nodes.Find("接合图表", true);
            if (mNode.Length == 0)
                return;
          
            treeViewJHTable.Nodes.Add("图幅号", "图幅号", 18, 19);
   
            TreeNode mTN = mNode[0];

            IFeatureLayer mFL = GetLayerofTreeNode(mTN) as IFeatureLayer;//call sub proc
            IFeatureClass mFC = mFL.FeatureClass;
            IFeatureCursor mFCs = mFC.Search(null, false);
            IFeature mF = mFCs.NextFeature();
            int fdNEWMAPNO = mFC.Fields.FindField("NEWMAPNO");
            //int fdXZQDM = mFC.Fields.FindField("XZQDM");
            while (mF != null)
            {
                try
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = mF.get_Value(fdNEWMAPNO).ToString();
                    tn.Name = tn.Text;//mF.get_Value(fdXZQDM).ToString();
                    tn.ImageIndex = 18;
                    tn.SelectedImageIndex = 19;
                    treeViewJHTable.Nodes[0].Nodes.Add(tn);
                }
                catch
                {

                }
                mF = mFCs.NextFeature();
            }
            treeViewJHTable.ExpandAll();



        }
        //调阅数据 菜单响应 根据地图文档文件打开数据
        public void LoadDatafromXml(string strXmlFile, SysCommon.CProgress vProgress)
        {

            //清空前次加载的图层
            axMapControl.Map.ClearLayers();

            //初始化地图文档树  数据单元代码   年度  比例尺  专题类型
            InitMapDocTreefromXml(strXmlFile, vProgress);


            //索引树跳转到地图文档窗口
            IndextabControl.SelectedTab = PageMapDoc;

            //视图窗口跳转到图形浏览窗口
         //   CenterTabControl.SelectedTab = MapPage;

        }



        //初始化TOC档树,并修改Layer节点的sFile名称
        public void InitTocTree(string strModPath, string strSuffixx, string strXzq, string strYear, string strScale, string strSubType)
        {


        }

        //初始化文档树
        public void InitTextDocTree(string strModPath, string strSuffixx, string strXzq, string strYear, string strScale, string strSubType, SysCommon.CProgress vProgress)
        {
            //判断文件是否存在
        /*    XmlDocument xmldoc = new XmlDocument();
            if (!File.Exists(strModPath))
                return;
            xmldoc.Load(strModPath);

            //从地图入库信息表中获取已入库数据信息（图层组成）
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串


            //获取文档库
            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
            string strExp = "select 文档库 from 地图入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" +
               strYear + "'" + "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
            string strDocLib = dDbFun.GetInfoFromMdbByExp(strCon, strExp);


            //创建树  GisMap 根节点 Group 专题节点 Layer 叶子节点
            TextDoctree.Nodes.Clear();

            //根节点
            m_tTextparent = new TreeNode();
            m_tTextparent.Text = "文档目录";
            TextDoctree.Nodes.Add(m_tTextparent);
            TextDoctree.ExpandAll();
            m_tTextparent.ImageIndex = 18;
            m_tTextparent.SelectedImageIndex = 18;

            TreeNode tGroupNode = new TreeNode();
            tGroupNode.Text = strSuffixx;
            tGroupNode.Name = strDocLib;
            tGroupNode.ExpandAll();
            m_tTextparent.Nodes.Add(tGroupNode);
            tGroupNode.ImageIndex = 18;
            tGroupNode.SelectedImageIndex = 18;

            TreeNode tLeafNode;
            string strSearchRoot = "//GisMap";
            XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);

            //若是文档库有内容就添加文档库
            if (strDocLib != null)
            {
                //操作xml文件 插入节点
                XmlElement xmlGridGroup = xmldoc.CreateElement("SubGroup");
                xmlGridGroup.SetAttribute("sItemName", "【文档数据】");
                xmlGridGroup.SetAttribute("sType", "GROUP");
                xmlGridGroup.SetAttribute("sDataSource", strDocLib);
                xmlGridGroup.SetAttribute("sGroupType", "DOC");
                xmlNodeRoot.AppendChild(xmlGridGroup);

                //添加文档叶子节点
                strExp = "select * from 文档入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" + strYear + "'" +
                     "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
                OleDbConnection mycon = new OleDbConnection(strCon);   //定义OleDbConnection对象实例并连接数据库
                OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
                try
                {
                    mycon.Open();

                    //创建datareader   对象来连接到表单     
                    OleDbDataReader aReader = aCommand.ExecuteReader();

                    //年度 专题 行政区代码
                    while (aReader.Read())
                    {
                        string strDocFilePath = strYear + strSubType + strXzq + "\\" + aReader["文档名称"].ToString() + "." + aReader["文档类型"].ToString();
                        XmlElement xmlDocLeaf = xmldoc.CreateElement("Layer");
                        xmlDocLeaf.SetAttribute("sItemName", aReader["文档名称"].ToString());
                        xmlDocLeaf.SetAttribute("sFile", strDocFilePath);
                        xmlDocLeaf.SetAttribute("sType", aReader["文档类型"].ToString());
                        xmlGridGroup.AppendChild(xmlDocLeaf);

                        tLeafNode = new TreeNode();
                        tLeafNode.Text = aReader["文档名称"].ToString();
                        tLeafNode.Name = strDocFilePath;
                        tLeafNode.ImageIndex = 19;
                        tLeafNode.SelectedImageIndex = 19;
                        tGroupNode.Nodes.Add(tLeafNode);

                    }
                    //关闭reader对象     
                    aReader.Close();

                    //关闭连接,这很重要     
                    mycon.Close();
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                //添加到树控件中

            }

            //删除属性值为空的节点
            DeleNullElementForXml(xmldoc);
            xmldoc.Save(strModPath);*/
        }

        //追加调阅更新文档树
        public void AddUpdateTextDocTree(string strModPath, string strSuffixx, string strXzq, string strYear, string strScale, string strSubType, SysCommon.CProgress vProgress)
        {
            //判断文件是否存在
            XmlDocument xmldoc = new XmlDocument();
            if (!File.Exists(strModPath))
                return;
            xmldoc.Load(strModPath);

            if (m_tTextparent == null)
                return;

            //从地图入库信息表中获取已入库数据信息（图层组成）
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串


            //获取文档库
            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
            string strExp = "select 文档库 from 地图入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" +
               strYear + "'" + "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
            string strDocLib = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //根节点
            TreeNode tGroupNode = new TreeNode();
            tGroupNode.Text = strSuffixx;
            tGroupNode.ExpandAll();
            m_tTextparent.Nodes.Add(tGroupNode);
            tGroupNode.ImageIndex = 18;
            tGroupNode.SelectedImageIndex = 18;

            TreeNode tLeafNode;
            string strSearchRoot = "//GisMap";
            XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);

            //若是文档库有内容就添加文档库
            if (strDocLib != null)
            {
                //操作xml文件 插入节点
                XmlElement xmlGridGroup = xmldoc.CreateElement("SubGroup");
                xmlGridGroup.SetAttribute("sItemName", "【文档数据】");
                xmlGridGroup.SetAttribute("sType", "GROUP");
                xmlGridGroup.SetAttribute("sDataSource", strDocLib);
                xmlGridGroup.SetAttribute("sGroupType", "DOC");
                xmlNodeRoot.AppendChild(xmlGridGroup);

                //添加文档叶子节点
                strExp = "select * from 文档入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" + strYear + "'" +
                     "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
                OleDbConnection mycon = new OleDbConnection(strCon);   //定义OleDbConnection对象实例并连接数据库
                OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
                try
                {
                    mycon.Open();

                    //创建datareader   对象来连接到表单     
                    OleDbDataReader aReader = aCommand.ExecuteReader();

                    //年度 专题 行政区代码
                    while (aReader.Read())
                    {
                        string strDocFilePath = strYear + strSubType + strXzq + "\\" + aReader["文档名称"].ToString() + "." + aReader["文档类型"].ToString();
                        XmlElement xmlDocLeaf = xmldoc.CreateElement("Layer");
                        xmlDocLeaf.SetAttribute("sItemName", aReader["文档名称"].ToString());
                        xmlDocLeaf.SetAttribute("sFile", strDocFilePath);
                        xmlDocLeaf.SetAttribute("sType", aReader["文档类型"].ToString());
                        xmlGridGroup.AppendChild(xmlDocLeaf);

                        tLeafNode = new TreeNode();
                        tLeafNode.Text = aReader["文档名称"].ToString();
                        tLeafNode.Name = strDocFilePath;
                        tLeafNode.ImageIndex = 19;
                        tLeafNode.SelectedImageIndex = 19;
                        tGroupNode.Nodes.Add(tLeafNode);

                    }
                    //关闭reader对象     
                    aReader.Close();

                    //关闭连接,这很重要     
                    mycon.Close();
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            //删除属性值为空的节点
            DeleNullElementForXml(xmldoc);
            xmldoc.Save(strModPath);

        }

        //初始化地图文档树,并修改Layer节点的sFile名称
        public void  InitMapDocTree(string strModPath, string strSuffixx, string strXzq, string strYear, string strScale, string strSubType, SysCommon.CProgress vProgress)
        {
            //判断文件是否存在
            XmlDocument xmldoc = new XmlDocument();
            if (!File.Exists(strModPath))
                return;
            xmldoc.Load(strModPath);

            //从地图入库信息表中获取已入库数据信息（图层组成）
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            //    string mypath = dIndex.GetDbValue("dbServerPath");
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 图层组成 from 地图入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" +
                strYear + "'" + "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
            string strLayerGroup = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //数据类型
            //strExp = "select 数据类型 from 地图入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" +
            //   strYear + "'" + "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
            //string strDataType = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //业务代码  地政 矿政 地环
            strExp = "select 业务代码 from 地图入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" +
               strYear + "'" + "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
            string strBusinesCode = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //图层名称前缀
        //    string strPrefix = strBusinesCode + strYear + strSubType + strXzq + strScale;//wgf 20110411


            //获取影像库
            strExp = "select 影像库 from 地图入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" +
             strYear + "'" + "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
            string strGridLib = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //创建树  GisMap 根节点 Group 专题节点 Layer 叶子节点
            MapDocTree.Nodes.Clear();

            //根节点
            m_tparent = new TreeNode();
            m_tparent.Text = "地图目录";
            m_tparent.Checked = true;
            MapDocTree.Nodes.Add(m_tparent);
            MapDocTree.ExpandAll();
            m_tparent.ImageIndex = 18;
            m_tparent.SelectedImageIndex = 18;

            TreeNode tMapNode;
            TreeNode tNewNode;
            string strTblName = "";
            string strRootName = "";
            string strSearchRoot = "//GisMap";
            XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);
            XmlElement xmlElentRoot = (XmlElement)xmlNodeRoot;
            strRootName = xmlElentRoot.GetAttribute("sItemName");
            xmlElentRoot.SetAttribute("sYear", strYear);
            xmlElentRoot.SetAttribute("sSubject", strSubType);
            xmlElentRoot.SetAttribute("sXzqCode", strXzq);
            xmlElentRoot.SetAttribute("sScale", strScale);

            tMapNode = new TreeNode();
            tMapNode.Text = strRootName + "_" + strSuffixx;
            tMapNode.Name = strSubType;
            tMapNode.Checked = true;
            m_tparent.Nodes.Add(tMapNode);
            m_tparent.ExpandAll();
            tMapNode.ImageIndex = 9;
            tMapNode.SelectedImageIndex = 9;

            //王国峰
            //创建组节点
            IGroupLayer pGroupFLayer = new GroupLayer();
            pGroupFLayer.Name = tMapNode.Text;
            IMapLayers pmaplayers = axMapControl.Map as IMapLayers;
            pmaplayers.InsertLayer(pGroupFLayer, false, pmaplayers.LayerCount);



            //若是影像库有内容就添加影像节点
            if (strGridLib != "")
            {
                //操作xml文件 插入节点

                //XmlElement xmlElentRoot = (XmlElement)xmlNodeRoot;
                XmlElement xmlElemGroup = xmldoc.CreateElement("SubGroup");
                string strGroupName = "【影像数据】";
                xmlElemGroup.SetAttribute("sItemName", strGroupName);
                xmlElemGroup.SetAttribute("sType", "GROUP");
                

                XmlElement xmlElement=xmldoc.CreateElement("Layer");
                xmlElement.SetAttribute("sDemo", "影像数据");
                string name = strGridLib.Substring(strGridLib.LastIndexOf("\\") + 1);
                xmlElement.SetAttribute("sItemName", "影像数据");
                //xmlElement.SetAttribute("sFile",name);
                xmlElement.SetAttribute("sDataSource", strGridLib);
                xmlElemGroup.AppendChild(xmlElement);

                //string strSearchBuffer = "//SubGroup";
                //XmlNode xmlNdGroupFirst = xmldoc.SelectSingleNode(strSearchBuffer);
                //if (xmlNdGroupFirst != null)
                //{
                //    xmlElentRoot.InsertBefore(xmlElemGroup, xmlNdGroupFirst);

                //}
                //else
                //{
                //    xmlElentRoot.AppendChild(xmlElemGroup);
                //}
                xmlElentRoot.AppendChild(xmlElemGroup);
                // xmldoc.Save(strModPath);
            }


            xmldoc.Save(strModPath);

            //首先添加第一级子节点 SubGroup
            string strSearch = "//SubGroup";
            XmlNodeList xmlNdList = xmldoc.SelectNodes(strSearch);
            foreach (XmlNode xmlChild in xmlNdList)
            {
                strTblName = "";
                XmlElement xmlElent = (XmlElement)xmlChild;
                strTblName = xmlElent.GetAttribute("sItemName");

                tNewNode = new TreeNode();
                tNewNode.Text = strTblName;
                tNewNode.Name = xmlElent.GetAttribute("sDataSource");

                tNewNode.Checked = true;
                tMapNode.Nodes.Add(tNewNode);
                tMapNode.ExpandAll();
                tNewNode.ImageIndex =20;
                tNewNode.SelectedImageIndex = 20;

                //添加最终子节点
                AddLeafItem(pGroupFLayer,strSubType, tNewNode, xmlChild, strLayerGroup, strYear, strXzq, strScale);

            }


            //删除属性值为空的节点
            DeleNullElementForXml(xmldoc);
            xmldoc.Save(strModPath);

            //记录日志
            LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
            string strLog = tMapNode.Text + "调阅完毕";
            if (log != null)
            {
                log.Writelog(strLog);
            }
            vProgress.SetProgress(strLog);
        }


        public void DeleNullElementForXml(XmlDocument xmldoc)
        {
            if (xmldoc != null)
            {
                string strSearch = "//Layer[@sFile='']";
                XmlNodeList xmlNdList = xmldoc.SelectNodes(strSearch);
                foreach (XmlNode xmlChild in xmlNdList)
                {
                    xmlChild.ParentNode.RemoveChild(xmlChild);
                }
            }
        }

        //直接打开保存的地图文档文件初始化地图文档树
        public void InitMapDocTreefromXml(string strModPath, SysCommon.CProgress vProgress)
        {
            //判断文件是否存在
        /*    XmlDocument xmldoc = new XmlDocument();
            if (!File.Exists(strModPath))
                return;
            xmldoc.Load(strModPath);

            //创建树  GisMap 根节点 Group 专题节点 Layer 叶子节点
            MapDocTree.Nodes.Clear();

            //根节点
            m_tparent = new TreeNode();
            m_tparent.Text = "地图目录";

            m_tparent.Checked = true;
            MapDocTree.Nodes.Add(m_tparent);
            MapDocTree.ExpandAll();
            m_tparent.ImageIndex = 18;
            m_tparent.SelectedImageIndex = 18;

          

            TreeNode tMapNode;
            TreeNode tNewNode;
            string strTblName = "";
            string strMapName = "";
            string strYear = "";
            string strSubType = "";
            string strXzq = "";
            string strScale = "";

            string strSuffixx = "";
            string strSearchRoot = "//GisDoc";
            XmlElement xmlElent;
            XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);

            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "";
            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
            string strCodeUnitName = "";
            string strScaledescribe = "";


            string strSearch = "//GisMap";
            XmlNodeList xmlNdListMap = xmlNodeRoot.SelectNodes(strSearch);
            foreach (XmlNode xmlChildMap in xmlNdListMap)
            {
                xmlElent = (XmlElement)xmlChildMap;
                strMapName = xmlElent.GetAttribute("sItemName");
                strYear = xmlElent.GetAttribute("sYear");
                strSubType = xmlElent.GetAttribute("sSubject");
                strXzq = xmlElent.GetAttribute("sXzqCode");
                strScale = xmlElent.GetAttribute("sScale");


                strExp = "select 行政名称 from 数据单元表 where 行政代码 ='" + strXzq + "'";
                strCodeUnitName = dDbFun.GetInfoFromMdbByExp(strCon, strExp);
                strExp = "select 描述 from 比例尺代码表 where 代码 ='" + strScale + "'";
                strScaledescribe = dDbFun.GetInfoFromMdbByExp(strCon, strExp);


                strSuffixx = strMapName + "_" + strCodeUnitName + strYear + "年" + "【" + strScaledescribe + "】";


                tMapNode = new TreeNode();
                tMapNode.Text = strSuffixx;
                tMapNode.Name = strSubType;
                tMapNode.Checked = true;


                //创建组节点
                IGroupLayer pGroupFLayer = new GroupLayer();
                pGroupFLayer.Name = tMapNode.Text;
                IMapLayers pmaplayers = axMapControl.Map as IMapLayers;
                pmaplayers.InsertLayer(pGroupFLayer, false, pmaplayers.LayerCount);

                m_tparent.Nodes.Add(tMapNode);
                m_tparent.ExpandAll();
                tMapNode.ImageIndex = 9;
                tMapNode.SelectedImageIndex =9;

                tTextNode = new TreeNode();
                tTextNode.Text = strSuffixx;
                m_tTextparent.Nodes.Add(tTextNode);
                m_tTextparent.ExpandAll();
                tTextNode.ImageIndex = 9;
                tTextNode.SelectedImageIndex =9;

                //首先添加第一级子节点 SubGroup
                //strSearch = "//SubGroup";  //区分文档节点  sGroupType = DOC
                XmlNodeList xmlNdList = xmlChildMap.ChildNodes;
                foreach (XmlNode xmlChild in xmlNdList)
                {
                    strTblName = "";
                    xmlElent = (XmlElement)xmlChild;
                    strTblName = xmlElent.GetAttribute("sItemName");
                    string strGroupType = xmlElent.GetAttribute("sGroupType");
                    if (strGroupType.Equals("DOC"))  //文档
                    {
                        vProgress.SetProgress("加载文档" + strTblName);
                        //添加最终子节点
                        AddLeafItemfromXml(pGroupFLayer,strSubType, tTextNode, xmlChild, strGroupType);
                    }
                    else
                    {
                        tNewNode = new TreeNode();
                        tNewNode.Text = strTblName;

                        tNewNode.Checked = true;
                        tMapNode.Nodes.Add(tNewNode);
                        tMapNode.ExpandAll();
                        tNewNode.ImageIndex = 20;
                        tNewNode.SelectedImageIndex = 20;

                        //添加最终子节点
                        AddLeafItemfromXml(pGroupFLayer,strSubType, tNewNode, xmlChild, strGroupType);

                        vProgress.SetProgress(tNewNode.Text + "加载完毕");
                    }

                }

            }
            TextDoctree.Refresh();
            xmldoc.Save(strModPath);*/
        }
        public IWorkspace GetWorkspace(string server, string service, string dataBase, string user, string password, string version, enumWSType wsType, out Exception eError)
        {
            eError = null;
            bool result = false;

            if (ModFrameData.v_AppGisUpdate.gisDataSet == null)
            {
                ModFrameData.v_AppGisUpdate.gisDataSet = new SysCommon.Gis.SysGisDataSet();
            }
            try
            {
                switch (wsType)
                {
                    case enumWSType.SDE:
                        result = ModFrameData.v_AppGisUpdate.gisDataSet.SetWorkspace(server, service, dataBase, user, password, version, out eError);
                        break;
                    case enumWSType.PDB:
                    case enumWSType.GDB:
                        result = ModFrameData.v_AppGisUpdate.gisDataSet.SetWorkspace(server, wsType, out eError);
                        break;
                    default:
                        break;
                }
                if (result)
                {
                    return ModFrameData.v_AppGisUpdate.gisDataSet.WorkSpace;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                eError = ex;
                return null;
            }
        }
        //直接读取先前保存好的xml文件
        //strInLibLayer  已经入库的图层组成
        //strPrefix      图层名称前缀  与数据入库的命名方式一致
        public void AddLeafItemfromXml(IGroupLayer pGroupFLayer,string strSubType, TreeNode treeNode, XmlNode xmlNode, string strGroupType)
        {
            if (treeNode != null && xmlNode != null)
            {
                TreeNode tNewNode;
                if (strGroupType.Equals("DOC"))
                {
                    string strLayerDescribed = ""; //图层名称 地类图斑等
                    string strNewFile = "";
                    XmlNodeList xmlNdList;
                    xmlNdList = xmlNode.ChildNodes;
                    foreach (XmlNode xmlChild in xmlNdList)
                    {
                        strLayerDescribed = "";
                        XmlElement xmlElent = (XmlElement)xmlChild;
                        strLayerDescribed = xmlElent.GetAttribute("sDemo");
                        strNewFile = xmlElent.GetAttribute("sItemName");
                        //加载图形数据
                        if (!strNewFile.Equals(""))
                        {
                            tNewNode = new TreeNode();
                            tNewNode.Text = strLayerDescribed;
                            tNewNode.Name = strNewFile;
                            treeNode.Nodes.Add(tNewNode);
                            tNewNode.ImageIndex = 19;
                            tNewNode.SelectedImageIndex =19;
                        }
                    }
                }
                else
                {
                    string strLayerDescribed = ""; //图层名称 地类图斑等
                    string strNewFile = "";
                    XmlNodeList xmlNdList;
                    xmlNdList = xmlNode.ChildNodes;
                    foreach (XmlNode xmlChild in xmlNdList)
                    {
                        strLayerDescribed = "";
                        XmlElement xmlElent = (XmlElement)xmlChild;
                        strLayerDescribed = xmlElent.GetAttribute("sDemo");
                        strNewFile = xmlElent.GetAttribute("sItemName");
                        string Datasource = xmlElent.GetAttribute("sDataSource");
                        if (xmlElent.GetAttribute("sDiaphaneity") != "")
                            m_transpy = short.Parse(xmlElent.GetAttribute("sDiaphaneity"));//透明度
                        string strcale = xmlElent.GetAttribute("sDispScale");//比例尺 1:10000
                        if (strcale != "")
                        {
                            strcale = strcale.Substring(strcale.LastIndexOf(":") + 1);//得到 10000
                            m_scale = double.Parse(strcale);//比例尺 转换为double
                        }

                        //加载图形数据
                        if (!strNewFile.Equals(""))
                        {                            
                            tNewNode = new TreeNode();
                            tNewNode.Checked = true;
                            tNewNode.Text = strLayerDescribed;
                            tNewNode.Name = strNewFile;

                            treeNode.Nodes.Add(tNewNode);
                            tNewNode.ImageIndex = 19;
                            tNewNode.SelectedImageIndex = 19;

                            //根据图层名称获取pFeatureWorkspace  wgf20110412
                            string strSubject = "";
                            if (treeNode.Parent!= null)
                            {
                                strSubject = treeNode.Parent.Name;
                            }
                            if (strNewFile.Trim() != "影像数据")
                            {
                                 IFeatureWorkspace pFeatureWorkspace ;
                                //string strLayerName = GetLayerNameFormNodeName(strNewFile, strSubject); //changed by chulili 2011-04-15
                                string strLayerName = GetLayersFileFormNode(tNewNode);
                                if (m_strnew.Trim() != "")//新增加的
                                {
                                    pFeatureWorkspace = GetWorkspaceByFileName2(strLayerName);
                                    strLayerName = strLayerName.Substring(strLayerName.LastIndexOf("\\") + 1);
                                    strLayerName = System.IO.Path.GetFileNameWithoutExtension(strLayerName);
                                }
                                else
                                {
                                    //若是m_strnew== "" 维持原来代码
                                    pFeatureWorkspace = GetWorkspaceByFileName(strLayerName);
                                }
                                if (pFeatureWorkspace != null)
                                    LoadMapData(pGroupFLayer,strSubType, pFeatureWorkspace, strLayerName, strLayerDescribed);
                                else
                                {
                                    tNewNode.ImageIndex = 14;
                                    tNewNode.SelectedImageIndex = 14;
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (Datasource.Trim() == "")
                                    {
                                        tNewNode.ImageIndex = 14;
                                        tNewNode.SelectedImageIndex = 14;                                        
                                    }
                                    LoadMapRasterData(pGroupFLayer,Datasource);//加载影像库
                                    tNewNode.ImageIndex = 19;
                                    tNewNode.SelectedImageIndex = 19;
                                }
                                catch
                                {
                                    tNewNode.ImageIndex = 14;
                                    tNewNode.SelectedImageIndex = 14;
                                }
                            }
                        }
                    }
                }
                treeNode.ExpandAll();
            }
        }


        //added  by  xs 2011.04.18
        public IFeatureWorkspace GetWorkspaceByFileName2(string filename)
        {
            if (filename.Equals(""))
                return null;
            if (filename.Length < 1)
                return null;
            IFeatureWorkspace pFeatureWorkspace;
            int Index = filename.LastIndexOf("\\");
            string filePath = filename.Substring(0, Index);
            try
            {//打开shp文件
                if (m_strnew == "shp")
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(filename);
                    //打开工作空间并添加shp文件
                    IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
                    //注意此处的路径是不能带文件名的
                    pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(filePath, 0);
                }
                else
                {
                    //打开personGeodatabase,并添加图层
                    IWorkspaceFactory pAccessWorkspaceFactory = new AccessWorkspaceFactoryClass();
                    //打开工作空间并遍历数据集
                    IWorkspace pWorkspace = pAccessWorkspaceFactory.OpenFromFile(filePath, 0);
                    pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                }
                return pFeatureWorkspace;
            }
            catch
            {
                return null;
            }
        }
        public IFeatureWorkspace GetWorkspaceByFileName(string filename)
        {
            if (filename.Equals(""))
                return null;
            if (filename.Length < 1)
                return null;
            string[] array = new string[6];//分析数据成数组形式
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 字段名称 from 图层命名规则表";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strname = db.GetInfoFromMdbByExp(strCon, strExp);
            string[] arrName = strname.Split('+');//分离字段名称
            for (int i = 0; i < arrName.Length; i++)
            {
                switch (arrName[i])
                {
                    case "业务大类代码":
                        array[0] = filename.Substring(0, 2);//业务大类代码
                        filename = filename.Remove(0, 2);
                        break;
                    case "年度":
                        array[1] = filename.Substring(0, 4);//年度
                        filename = filename.Remove(0, 4);
                        break;
                    case "业务小类代码":
                        array[2] = filename.Substring(0, 2);//业务小类代码
                        filename = filename.Remove(0, 2);
                        break;
                    case "行政代码":
                        array[3] = filename.Substring(0, 6);//行政代码
                        filename = filename.Remove(0, 6);
                        break;
                    case "比例尺":
                        array[4] = filename.Substring(0, 1);//比例尺
                        filename = filename.Remove(0, 1);
                        break;
                }
            }
            array[5] = filename;//图层组成

            IFeatureWorkspace pFeatureWorkspace = GetWorkspaceByLayerInfo(array[0], array[1], array[2], array[3], array[4], array[5]);
           return pFeatureWorkspace;
        }

        //添加叶子节点
        //strInLibLayer  已经入库的图层组成
        public void AddLeafItem(IGroupLayer pGroupFLayer,string strSubType, TreeNode treeNode, XmlNode xmlNode, string strInLibLayer, string strYear, string strXzq, string strScale)
        {
            if (treeNode != null && xmlNode != null && pGroupFLayer != null)
            {
                //wgf 20110412
              //?  IFeatureWorkspace pFeatureWorkspace = ModFrameData.v_AppGisUpdate.gisDataSet.WorkSpace as IFeatureWorkspace;
                TreeNode tNewNode;
                string strLayerDescribed = ""; //图层名称 地类图斑等
                string strFileName = "";
                string strNewFile = "";
                string strItemName="";
                string strBigClass = "";
                string strSubClass = "";
                XmlNodeList xmlNdList;
                xmlNdList = xmlNode.ChildNodes;
                foreach (XmlNode xmlChild in xmlNdList)
                {
                    strLayerDescribed = "";
                    XmlElement xmlElent = (XmlElement)xmlChild;
                    strLayerDescribed = xmlElent.GetAttribute("sDemo");//名称描述
                    strItemName = xmlElent.GetAttribute("sItemName");//名称
                    strBigClass = xmlElent.GetAttribute("sBigClass");//业务大类
                    strSubClass = xmlElent.GetAttribute("sSubClass");//业务小类
                    strFileName = xmlElent.GetAttribute("sFile");//图层代码
                    string strLayerType = xmlElent.GetAttribute("sLayerType");
                    if (xmlElent.GetAttribute("sDiaphaneity") != "")
                        m_transpy = short.Parse(xmlElent.GetAttribute("sDiaphaneity"));//透明度
                    string strcale = xmlElent.GetAttribute("sDispScale");//比例尺 1:10000
                    if (strcale != "")
                    {
                        strcale = strcale.Substring(strcale.LastIndexOf(":") + 1);//得到 10000
                        m_scale = double.Parse(strcale);//比例尺 转换为double
                    }
                    string datasource = xmlElent.GetAttribute("sDataSource");
                    if (strFileName == "")
                    {
                        if (datasource != "")
                        {
                            strNewFile = datasource.Substring(datasource.LastIndexOf("\\") + 1);
                            tNewNode = new TreeNode();
                            tNewNode.Checked = true;
                            tNewNode.Text = strLayerDescribed;
                            tNewNode.Name = strItemName;

                            treeNode.Nodes.Add(tNewNode);
                            try
                            {
                                LoadMapRasterData(pGroupFLayer,datasource);//加载影像库
                                tNewNode.ImageIndex = 19;
                                tNewNode.SelectedImageIndex = 19;
                            }
                            catch 
                            {
                                tNewNode.ImageIndex = 14;
                                tNewNode.SelectedImageIndex = 14;
                            }                           
                        }
                    }
                    else
                    {
                        if (strInLibLayer.Contains(strFileName))
                        {
                            //修改sfile名称

                            strNewFile = GetLayerName(strBigClass, strYear, strSubClass, strXzq, strScale) + strFileName;
                            xmlElent.SetAttribute("sFile", strNewFile);

                            tNewNode = new TreeNode();
                            tNewNode.Checked = true;
                            tNewNode.Text = strLayerDescribed;
                            tNewNode.Name = strItemName;

                            treeNode.Nodes.Add(tNewNode);
                            tNewNode.ImageIndex = 19;
                            tNewNode.SelectedImageIndex = 19;

                            //根据图层名称获取pFeatureWorkspace  wgf20110412
                            IFeatureWorkspace pFeatureWorkspace = GetWorkspaceByLayerInfo(strBigClass, strYear, strSubClass, strXzq, strScale, strFileName);
                            if (pFeatureWorkspace!=null)//不是空的空间
                            {//加载图形数据
                                LoadMapData(pGroupFLayer,strSubType, pFeatureWorkspace, strNewFile, strLayerDescribed);
                            }
                            else
                            {
                                tNewNode.ImageIndex = 14;//改图标
                                tNewNode.SelectedImageIndex = 14;//改图标                                
                            }
                        }
                        else
                        {
                            xmlElent.SetAttribute("sFile", "");
                        }
                    }
                }
                treeNode.ExpandAll();
            }
        }

        /// <summary>
        /// 根据每个图层对应的属性信息 获取Layer  针对专题的每个图层都保存在不同的数据库中
        /// wgf 2011-04-12
        /// strBigClass, 业务大类代码
        /// strYear,     年度
        /// strSubClass, 业务小类代码
        /// strXzq,      数据单元代码（县级代码）
        /// strScale,    比例尺代码
        /// strFileName  图层名称 DLTB
        /// </summary>
        /// <returns></returns>
        public IFeatureWorkspace GetWorkspaceByLayerInfo(string strBigClass, string strYear, string strSubClass, string strXzq, string strScale, string strFileName)
        {
            IFeatureWorkspace pFeatureWorkspace = null;

            //根据表达式从 数据编码表中获取数据源 
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = string.Format("select 数据源名称 from 数据编码表 where 业务大类代码='{0}' and 年度='{1}' and 业务小类代码='{2}'and 行政代码='{3}' and 比例尺='{4}' and 图层代码='{5}'",
              strBigClass, strYear, strSubClass, strXzq, strScale, strFileName);
     
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strDataSourceName = db.GetInfoFromMdbByExp(strCon, strExp);
            if (strDataSourceName.Trim() != "")
            {

                //changed by xisheng 2011.04.29
                //    //从物理数据源表中获取对应的数据库信息
                //    strExp = string.Format("select 数据库 from 物理数据源表 where 数据源名称='{0}'", strDataSourceName);
                //    string strDataLibName = db.GetInfoFromMdbByExp(strCon, strExp);
                //    if (strDataLibName.Trim() != "")
                //    {
                //        if (!Directory.Exists(strDataLibName) && !File.Exists(strDataLibName))//路径不存在
                //        {
                //            pFeatureWorkspace = null;
                //        }
                //        else
                //        {//创建workspace
                //            IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                //            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(strDataLibName, 0);
                //            pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                //        }
                //    }

                IWorkspace pWorkspace = GetWorkspace(strDataSourceName);//判断是GDB还是SDE的数据库，得到相应的工作空间
                pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            }
            return pFeatureWorkspace;

        }
        /// <summary>
        /// 得到数据库空间 Added by xisheng 2011.04.28
        /// </summary>
        /// <param name="str">数据源名称</param>
        /// <returns>工作空间</returns>
        private IWorkspace GetWorkspace(string str)
        {
            try
            {
                IWorkspace pws = null;
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "select * from 物理数据源表 where 数据源名称='" + str + "'";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
                string type = dt.Rows[0]["数据源类型"].ToString();
                if (type.Trim() == "GDB")
                {
                    IWorkspaceFactory pWorkspaceFactory;
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                    pws = pWorkspaceFactory.OpenFromFile(dt.Rows[0]["数据库"].ToString(), 0);
                }
                else if (type.Trim() == "SDE")
                {
                    IWorkspaceFactory pWorkspaceFactory;
                    pWorkspaceFactory = new SdeWorkspaceFactoryClass();

                    //PropertySet
                    IPropertySet pPropertySet;
                    pPropertySet = new PropertySetClass();
                    pPropertySet.SetProperty("Server", dt.Rows[0]["服务器"].ToString());
                    pPropertySet.SetProperty("Database", dt.Rows[0]["数据库"].ToString());
                    pPropertySet.SetProperty("Instance", "5151");//"port:" + txtService.Text
                    pPropertySet.SetProperty("user", dt.Rows[0]["用户"].ToString());
                    pPropertySet.SetProperty("password", dt.Rows[0]["密码"].ToString());
                    pPropertySet.SetProperty("version", "sde.DEFAULT");
                    pws = pWorkspaceFactory.Open(pPropertySet, 0);

                }
                return pws;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 组织前缀　added by xs 20110413
        /// </summary>
        /// <param name="str1">业务大类</param>
        /// <param name="str2">年度</param>
        /// <param name="str3">业务小类</param>
        /// <param name="str4">行政代码</param>
        /// <param name="str5">比例尺</param>
        /// <returns></returns>
        public string GetLayerName(string str1, string str2, string str3, string str4, string str5)
        {
            string layername="";
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 字段名称 from 图层命名规则表";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strname = db.GetInfoFromMdbByExp(strCon, strExp);
            string[] arrName = strname.Split('+');//分离字段名称
            for (int i = 0; i < arrName.Length; i++)
            {
                switch (arrName[i])
                {
                    case "业务大类代码":
                        layername += str1;
                        break;
                    case "年度":
                        layername += str2;
                        break;
                    case "业务小类代码":
                        layername += str3;
                        break;
                    case "行政代码":
                        layername += str4;
                        break;
                    case "比例尺":
                        layername += str5;
                        break;
                }
            }
            return layername;
        }
        /// <summary>
        /// 从树节点的name得到XML中sFile存的图层名称
        /// </summary>
        /// <param name="strItemName">树节点Name</param>
        ///  strSubject   根节点name 对应xml文件中的GisMap中的sSubject
        /// <returns></returns>
        public string GetLayerNameFormNodeName(string strItemName, string strSubject)
        {
            if (strItemName.Equals(""))
                return "";
            string layername = "";
            string strCurFile = Application.StartupPath + "\\..\\Temp\\CurPrj.xml";
            if (File.Exists(strCurFile))
            {
                string strSearch = "//GisMap[@sSubject='" + strSubject + "']" + "//Layer[@sItemName='" + strItemName + "']";
                XmlDocument xmlCurdoc = new XmlDocument();
                xmlCurdoc.Load(strCurFile);
                XmlNode xmlnode = xmlCurdoc.SelectSingleNode(strSearch);
                if (xmlnode != null)
                {
                    layername = ((XmlElement)xmlnode).GetAttribute("sFile");
                }

            }
            return layername;
        }
        //added by chulili 2011-4-14
        //从树节点得到XML中sFile存的图层名称
        //输入参数：树节点  输出参数：图层对应map中文件名
        public string GetLayersFileFormNode(TreeNode node)
        {
            string strsFile = "";
            m_strnew = "";
            if (node == null)
                return strsFile;
            if (node.Level < 3)
                return strsFile;
            //读取当前配置文件
            string strWorkFile = Application.StartupPath + "\\..\\Temp\\CurPrj.xml";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(strWorkFile);

            TreeNode TopicNode = node.Parent.Parent;    //获取所在的专题节点
            string strSuffixx = TopicNode.Text;
            //根据当前专题的名称构成，获得对应xml节点的各属性
            string strMapName = strSuffixx.Substring(0, strSuffixx.IndexOf("_"));
            string strYear = strSuffixx.Substring(strSuffixx.LastIndexOf("年") - 4, 4);
            string strScaledescribe = strSuffixx.Substring(strSuffixx.LastIndexOf("【") + 1, strSuffixx.LastIndexOf("】") - strSuffixx.LastIndexOf("【") - 1);
            string strCodeUnitName = strSuffixx.Substring(strSuffixx.IndexOf("_") + 1, strSuffixx.LastIndexOf("年") - 4 - strSuffixx.IndexOf("_") - 1);
            //读取当前的业务库mdb
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串

            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
            //根据行政名称获取行政代码
            string strExp = "select 行政代码 from 数据单元表 where 行政名称 ='" + strCodeUnitName + "' and 数据单元级别='3'";
            string strCodeUnit = dDbFun.GetInfoFromMdbByExp(strCon, strExp);
            //根据比例尺描述获取比例尺代码
            strExp = "select 代码 from 比例尺代码表 where 描述='" + strScaledescribe + "'";
            string strScalecode = dDbFun.GetInfoFromMdbByExp(strCon, strExp);
            //strSuffixx = strMapName + "_" + strCodeUnitName + strYear + "年" + "【" + strScaledescribe + "】";

            string strSearchExp = "//GisMap[@sItemName='" + strMapName + "' and @sYear = '" + strYear + "'"
            + "and @sXzqCode ='" + strCodeUnit + "'" + "and @sScale='" + strScalecode + "']";
            //获取变化图斑图层
            XmlNode topicnode = xmldoc.SelectSingleNode(strSearchExp);
            strSearchExp = "//Layer [@sItemName='" + node.Name + "']";
            //获取图层对应的xmlnode
            XmlNode pXmlnode = topicnode.SelectSingleNode(strSearchExp);
            if (pXmlnode == null)
                return strsFile;
            if (!pXmlnode.NodeType.ToString().Equals("Element"))
                return strsFile;
            XmlElement pEle = pXmlnode as XmlElement;
            //获取xmlnode的sFile属性
            strsFile = pEle.GetAttribute("sFile");
            m_strnew = pEle.GetAttribute("sNewLoad");
            return strsFile;
        }
        /// <summary>
        /// 从树节点的name得到XML中sDatasource存的
        /// </summary>
        /// <param name="strItemName">树节点Name</param>
        ///  strSubject   根节点name 对应xml文件中的GisMap中的sSubject
        /// <returns></returns>
        public string GetRasterLayerFormNodeName(string strItemName, string strSubject)
        {
            if (strItemName.Equals(""))
                return "";
            string layername = "";
            string strCurFile = Application.StartupPath + "\\..\\Temp\\CurPrj.xml";
            if (File.Exists(strCurFile))
            {
                if (strItemName.Trim() == "影像数据")
                {
                    string strSearch = "//GisMap[@sSubject='" + strSubject + "']" + "//Layer[@sItemName='" + strItemName + "']";
                    XmlDocument xmlCurdoc = new XmlDocument();
                    xmlCurdoc.Load(strCurFile);
                    XmlNode xmlnode = xmlCurdoc.SelectSingleNode(strSearch);
                    if (xmlnode != null)
                    {
                        layername = ((XmlElement)xmlnode).GetAttribute("sDataSource");
                        if (layername != "")
                        {
                            layername=layername.Substring(layername.LastIndexOf("\\")+1);
                        }
                    }
                }

            }
            return layername;
        }

        /// <summary>
        /// 根据当前节点递归选中父节点 added by xs 2011.04.18
        /// </summary>
        /// <param name="node">当前节点</param>
        private void ChangeParentCheck(TreeNode node)
        {
            flag2 = false;
            if (node.Parent != null)
            {
                if (node.Checked)
                {
                    node.Parent.Checked = true;
                    ChangeParentCheck(node.Parent);
                }
            }
            flag2 = true;

        }

        /// <summary>
        /// 根据当前节点递归选中父节点 added by xs 2011.04.19
        /// </summary>
        /// <param name="node">当前节点</param>
        private void ChangeChildCheck(TreeNode node)
        {
            
            foreach (TreeNode item in node.Nodes)
            {
                flag = false;
                if (item.Level.Equals(3)) flag = true;
                item.Checked = node.Checked;
                ChangeChildCheck(item); 
            }
           

        }
        //地图文档树 父节点选择状态改变后 子节点也要随之发生改变
        private void MapDocTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (flag2&&flag)
            {
                ChangeParentCheck(e.Node);
                TreeNode node = e.Node;
                //如果是叶子节点
                if (node.Level.Equals(3)&&flag)
                {
                    ModifyLayerDispStat(node, node.Checked);
                }
                else
                {
                   
                    ChangeChildCheck(node);
                    //如果是父亲节点
                    
                    foreach (TreeNode item in node.Nodes)
                    {
                       
                        if (item.Level.Equals(3))
                        {
                            //pFeatureLayer.Visible = node.Checked;
                            ModifyLayerDispStat(item, item.Checked);
                        }
                    }
   

                }
                axMapControl.ActiveView.Refresh(); //刷新视图，显示加载图层后的结果 
            }
        }


        //根据节点选择状态更新图形是否显示
        private void ModifyLayerDispStat(TreeNode node, bool bStat)
        {
            string strSubject = "";
            string strRootMapText = "";

            if (node.Parent != null)
            {
                if (node.Parent.Parent != null)
                {
                    strSubject = node.Parent.Parent.Name;
                    strRootMapText = node.Parent.Parent.Text;
                }
            }
            //string strLayerName = GetLayerNameFormNodeName(node.Name, strSubject);//changed by chulili 2011-04-15
            string strLayerName = GetLayersFileFormNode(node);
           /* IFeatureWorkspace pFeatureWorkspace = ModFrameData.v_AppGisUpdate.gisDataSet.WorkSpace as IFeatureWorkspace;
            Exception exError = null;
            IFeatureClass pFC = ModFrameData.v_AppGisUpdate.gisDataSet.GetFeatureClass(strLayerName, out exError);*/

            int iLayerCount = axMapControl.Map.LayerCount;
            IFeatureLayer pFeatureLayer;
            IRasterLayer pRasterLayer;
            IMap pMap = axMapControl.Map;
            
                if (strLayerName.Trim() != "")//矢量图层加载状态
                {
                    for (int ii = 0; ii < iLayerCount; ii++)
                    
                    {ILayer layer = pMap.get_Layer(ii);
                     if (layer is IGroupLayer && layer.Name == strRootMapText)
                     {
                          ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                          for (int c = 0; c < Comlayer.Count; c++)
                          {
                              pFeatureLayer = Comlayer.get_Layer(c) as IFeatureLayer;
                              if (pFeatureLayer == null)
                                  continue;
                              if (pFeatureLayer.FeatureClass == null)
                                  continue;
                              if (pFeatureLayer.Name == node.Name)
                              {
                                  pFeatureLayer.Visible = bStat;
                                  break;
                              }
                          }
                     }
                    }
                   /* pFeatureLayer = pMap.get_Layer(ii) as IFeatureLayer;
                    if (pFeatureLayer == null)
                        continue;
                    if (pFeatureLayer.FeatureClass.FeatureClassID == pFC.FeatureClassID)
                    {
                        pFeatureLayer.Visible = bStat;
                        break;
                    }*/

                }
                else//影像图层显示状态
                {
                    strLayerName = GetRasterLayerFormNodeName(node.Name, strSubject);
                    for (int ii = 0; ii < iLayerCount; ii++)
                    {
                        ILayer layer = pMap.get_Layer(ii);
                        if (layer is IGroupLayer && layer.Name == strRootMapText)
                        {
                            ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                            for (int c = 0; c < Comlayer.Count; c++)
                            {
                                pRasterLayer = Comlayer.get_Layer(c) as IRasterLayer;
                                if (pRasterLayer == null)
                                    continue;
                                if (pRasterLayer.Name.Substring(0, pRasterLayer.Name.LastIndexOf("\\")).ToUpper() == strLayerName.ToUpper())
                                {
                                    pRasterLayer.Visible = bStat;
                                }
                            }
                        }
                    }

            }
            axMapControl.ActiveView.Refresh(); //刷新视图，显示加载图层后的结果 
        }

        //加载栅格数据
        public void LoadMapRasterData(IGroupLayer pGroupFLayer, string strFullPath)
        {
            if (strFullPath.Trim() == "")
                return;
            IWorkspaceFactory pWorkspaceFactory;
            //IRasterWorkspace pRasterWorkspace;
            IRasterWorkspaceEx pRasterWorkspaceEx;
            IWorkspace pWorkspace;

            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            if (strFullPath == "") return;
            int Index = strFullPath.LastIndexOf("\\");
            string fileName = strFullPath.Substring(Index + 1);
            string sourcename = strFullPath.Substring(0, Index);
            //string strExp = "select 数据库 from 物理数据源表 where 数据源名称='" + sourcename + "'";
            //GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            //string filePath = db.GetInfoFromMdbByExp(strCon, strExp);
            //if (filePath.Trim() == "")
            //{
            //    filePath = sourcename;
            //}
            //if (!Directory.Exists(filePath)) return;
            //pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
            //pWorkspace = pWorkspaceFactory.OpenFromFile(filePath, 0);
            pWorkspace = GetWorkspace(sourcename);
            if (pWorkspace == null) return;
            pRasterWorkspaceEx = pWorkspace as IRasterWorkspaceEx;
            IRasterCatalog pCatalog = null;
            IEnumDatasetName pEnumDSName = pWorkspace.get_DatasetNames(esriDatasetType.esriDTRasterCatalog);
            IDatasetName pSDEDsName = pEnumDSName.Next();
            while (pSDEDsName != null)
            {
                string SDEDatasetName = pSDEDsName.Name;
                if (fileName.Trim().CompareTo(SDEDatasetName) != 0)
                {
                    pSDEDsName = pEnumDSName.Next();
                    break;
                }
                IRasterDataset2 pRasterDS = (IRasterDataset2)pRasterWorkspaceEx.OpenRasterDataset(SDEDatasetName);

                //IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTRasterCatalog);
                //IDataset pDataset = pEnumDataset.Next();
                //while (pDataset != null)
                //{
                //    if (pDataset.Name == fileName)
                //    {
                //        pCatalog = pDataset as IRasterCatalog;
                //        break;
                //    }
                //    pDataset = pEnumDataset.Next();
                //}
                ////pRasterWorkspaceEx = (IRasterWorkspaceEx)pRasterWorkspace;
                //IFeatureClass featureClass = (IFeatureClass)pCatalog;
                //IRasterCatalogItem rasterCatalogItem = (IRasterCatalogItem)featureClass.GetFeature(1);
                //IRasterDataset pRasterDataset = rasterCatalogItem.RasterDataset;
                ////try
                ////{
                ////    pRasterDataset = (IRasterDataset)pRasterWorkspace.OpenRasterDataset(fileName);
                ////}
                ////catch
                ////{
                ////     pRasterDataset = (IRasterDataset)pRasterWorkspaceEx.OpenRasterCatalog(fileName);
                ////}
                IRasterLayer pRasterLayer = new RasterLayerClass();
                pRasterLayer.CreateFromDataset(pRasterDS);
                IMapLayers pmaplayers = axMapControl.Map as IMapLayers;
                //   ModRender.SetRenderByXML(pLayer, log);//符号化   //褚丽丽编写根据sytel文件配图
                //  pmaplayers.InsertLayer(pLayer, false, pmaplayers.LayerCount);
                //王国峰
                ICompositeLayer Comlayer = pGroupFLayer as ICompositeLayer;
                pmaplayers.InsertLayerInGroup(pGroupFLayer, pRasterLayer, false, Comlayer.Count);
                //axMapControl.Extent = axMapControl.FullExtent;
                axMapControl.ActiveView.Refresh();
                pSDEDsName = pEnumDSName.Next();



            }
        }
        // 加载地图数据
        //strLayerName  数据库中文件的存储名称
        //strLayerDescribed 文件中文描述
        public void LoadMapData(IGroupLayer pGroupFLayer,string strSubType, IFeatureWorkspace pFeatureWorkspace, string strLayerName, string strLayerDescribed)
        {

            //IEnumDatasetName enumDatasetName = (pFeatureWorkspace as IWorkspace).get_DatasetNames(esriDatasetType.esriDTFeatureClass); 
            //IDatasetName datasetName = enumDatasetName.Next(); 
            //while (datasetName != null) 
            //{
            //    bool nn = (pFeatureWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, datasetName.Name);
            //    Console.WriteLine(datasetName.Name); datasetName = enumDatasetName.Next(); }



            if (pFeatureWorkspace != null && pGroupFLayer != null)
            {
                //先判断strLayerName图层是否存在 否则会死机！
                //根据每个图层记录的数据库信息 获取layer
                IWorkspace2 pWorkspace2 = pFeatureWorkspace as IWorkspace2;

                //IWorkspace workspace = pFeatureWorkspace as IWorkspace;
                //IEnumDatasetName enumDatasetName = workspace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
                //List<string> tmpL = new List<string>();
                //IDatasetName datasetName = enumDatasetName.Next(); 
                //while (datasetName != null)
                //{
                //    if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureClass, datasetName.Name))
                //    { tmpL.Add(datasetName.Name); }
                //    datasetName = enumDatasetName.Next();
                //}
                //if ((pFeatureWorkspace as IWorkspace).Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)//如果SDE加用户名前缀
                //{
                //    string username = (pFeatureWorkspace as IWorkspace).ConnectionProperties.GetProperty("USER").ToString().ToUpper();
                //    strLayerName = username + "." + strLayerName;
                //}
                if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureClass, strLayerName))
                {

                    IFeatureClass pFC = pFeatureWorkspace.OpenFeatureClass(strLayerName);

                    //定义特征图层，就是GIS里面图层的概念
                    IFeatureLayer pFLayer = new FeatureLayerClass();
                    if (m_scale != 0)
                    {
                        pFLayer.MaximumScale = m_scale;
                        pFLayer.MinimumScale = m_scale;
                    }
                    ILayerEffects pLyrEffects = pFLayer as ILayerEffects;
                    if (m_transpy != 0)
                        pLyrEffects.Transparency = m_transpy;//设置透明度

                    //设置图层名 
                    pFLayer.Name = strLayerDescribed;

                    //定义图层，并将刚才的特征图层强制转化为图层变量 
                    ILayer pLayer = pFLayer as ILayer;

                    //获取模板路径 
                    GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                    string mypath = dIndex.GetDbInfo();
                    string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                    string strExp = "select 配图方案文件 from 标准专题信息表 where 专题类型 ='" + strSubType + "'";
                    GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
                    string strModFile = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

                    MapDocumentClass pMapDocument = new MapDocumentClass();
                    if (File.Exists(strModFile))
                    {
                        //根据模版加载配图方案
                        pMapDocument.Open(strModFile, string.Empty);
                        IMap pMap = pMapDocument.get_Map(0);
                        int iLayCount = pMap.LayerCount;
                        ILayer pLayTemp;
                        for (int jj = 0; jj < iLayCount; jj++)
                        {
                            pLayTemp = pMap.get_Layer(jj);
                            if (pLayTemp == null)
                                continue;
                            if (pLayTemp.Name == pLayer.Name)
                            {
                                //设置数据源
                                pLayer = pLayTemp;
                                pLayer.Visible = true;
                                break;
                            }
                        }
                    }
              

                    //设置图层的特征集为刚才的特征集,这样就可以将特征集中的数据加载到特征图层对象中
                    (pLayer as IFeatureLayer).FeatureClass = pFC;
                    IMapLayers pmaplayers = axMapControl.Map as IMapLayers;
               //     ModRender.SetRenderByXML(pLayer);//符号化   //褚丽丽编写根据sytel文件配图
                    //  pmaplayers.InsertLayer(pLayer, false, pmaplayers.LayerCount);
                    //王国峰
                    ICompositeLayer Comlayer = pGroupFLayer as ICompositeLayer;
                    pmaplayers.InsertLayerInGroup(pGroupFLayer, pLayer, false, Comlayer.Count);
                  //?  pMapDocument.Close();

                    //记录日志
                    LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                }
                axMapControl.ActiveView.Refresh(); //刷新视图，显示加载图层后的结果   
            }
          
        }



        //追加调阅
        //在CurPrj.xml文件中添加新的专题
        private void MenuItemAddLoadData_Click(object sender, EventArgs e)
        {
            if (m_tparent == null||MapDocTree.Nodes.Count==0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (m_selectnode == null) return;
            TreeNode tCurNode = m_selectnode;
            //TreeNode tCurNode = DataIndexTree.SelectedNode;

            //获取专题类型
            string strSubType = tCurNode.Tag.ToString();
            //初始化进度条 added by chulili 
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();

            //获取模板路径 
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            //   string mypath = dIndex.GetDbValue("dbServerPath");
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 脚本文件 from 标准专题信息表 where 专题类型 ='" + strSubType + "'";

            //根据专题类型从标准专题信息表 中获取对应的模板文件路径
            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
            string strModpath = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //获取模板路径
            string strModFile = Application.StartupPath + "\\..\\Template\\" + strModpath;

            //复制到Temp文件夹中
            string strWorkFile = Application.StartupPath + "\\..\\Temp\\SupplementalPrj.xml";
            File.Copy(strModFile, strWorkFile, true);

            //初始化地图文档树
            string strBuffer = tCurNode.Text;
            TreeNode tParent = tCurNode.Parent;
            TreeNode tRoot = tParent.Parent;
            string strCodeUnitName = tRoot.Text;
            string strSuffixx = strCodeUnitName + strBuffer; //后缀 

            //记录日志
            LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
            string strLog = "开始追加调阅" + tCurNode.Text + "_" + strSuffixx;
            if (log != null)
            {
                log.Writelog(strLog);
            }
            vProgress.SetProgress(strLog);

            //获取数据行政代码（数据单元代码）
            strExp = "select 行政代码 from 数据单元表 where 行政名称 ='" + strCodeUnitName + "' and 数据单元级别='3'";
            string strCodeUnitCode = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //获取年度
            string strYear = strBuffer.Substring(0, 4);

            //获取比例尺代码
            int iStartPos = strBuffer.IndexOf("【");
            int iEndPos = strBuffer.IndexOf("】");
            int iLength = iEndPos - iStartPos - 1;
            string strScaleName = strBuffer.Substring(iStartPos + 1, iLength);
            strExp = "select 代码 from 比例尺代码表 where 描述 ='" + strScaleName + "'";
            string strScaleCode = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //追加调阅更新地图文档树  数据单元代码   年度  比例尺  专题类型
            AddUpdateMapDocTree(strWorkFile, strSuffixx, strCodeUnitCode, strYear, strScaleCode, strSubType, vProgress);

            //追加更新文档索引树
            vProgress.SetProgress("追加更新文档索引树");

            AddUpdateTextDocTree(strWorkFile, strSuffixx, strCodeUnitCode, strYear, strScaleCode, strSubType, vProgress);
            vProgress.Close();

            if (!XzqLoad)
            {
                //索引树跳转到地图文档窗口
                IndextabControl.SelectedTab = PageMapDoc;

                //视图窗口跳转到图形浏览窗口
              //  CenterTabControl.SelectedTab = MapPage;
            }

            //把SupplementalPrj中的内容添加到CurPrj.xml中
            UpdateCurPrjXml();

            //删除临时文件
            File.Delete(strWorkFile);
        }

        //更新当前的CurPrj.xml
        public void UpdateCurPrjXml()
        {
            string strCurFile = Application.StartupPath + "\\..\\Temp\\CurPrj.xml";
            string strWorkFile = Application.StartupPath + "\\..\\Temp\\SupplementalPrj.xml";
            XmlDocument xmlWorkdoc = new XmlDocument();
            xmlWorkdoc.Load(strWorkFile);
            string strSearchRoot = "//GisMap";
            XmlNode xmlNodeRoot = xmlWorkdoc.SelectSingleNode(strSearchRoot);

            XmlDocument xmlCurdoc = new XmlDocument();
            xmlCurdoc.Load(strCurFile);
            strSearchRoot = "//GisDoc";
            xmlCurdoc.Save(strCurFile);
            XmlNode xmlNodeAddRoot = xmlCurdoc.SelectSingleNode(strSearchRoot);
            XmlNode xmlNodeAdd = xmlCurdoc.ImportNode(xmlNodeRoot, true);
            xmlNodeAddRoot.AppendChild(xmlNodeAdd);

            xmlCurdoc.Save(strCurFile);
        }

        //追加调阅更新地图文档树,并修改Layer节点的sFile名称
        public void AddUpdateMapDocTree(string strModPath, string strSuffixx, string strXzq, string strYear, string strScale, string strSubType, SysCommon.CProgress vProgress)
        {

            //判断文件是否存在
            XmlDocument xmldoc = new XmlDocument();
            if (!File.Exists(strModPath))
                return;
            xmldoc.Load(strModPath);

            //如果追加调阅专题正在调阅，这不响应
            string strSearchExp = "//GisMap[@sSubject = '" + strSubType + "'" + "and @sYear = '" + strYear + "'"
            + "and @sXzqCode ='" + strXzq + "'" + "and @sScale='" + strScale + "']";
            string strCurFile = Application.StartupPath + "\\..\\Temp\\CurPrj.xml";
            XmlDocument xmlCurdoc = new XmlDocument();
            if (File.Exists(strCurFile))
            {
                xmlCurdoc.Load(strCurFile);
                XmlNode xmlFindNode = xmlCurdoc.SelectSingleNode(strSearchExp);
                if (xmlFindNode != null)
                    return;
            }


            //从地图入库信息表中获取已入库数据信息（图层组成）
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            // string mypath = dIndex.GetDbValue("dbServerPath");
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 图层组成 from 地图入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" +
                strYear + "'" + "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
            string strLayerGroup = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //数据类型
            //strExp = "select 数据类型 from 地图入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" +
            //   strYear + "'" + "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
            //string strDataType = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //业务代码  地政 矿政 地环
            strExp = "select 业务代码 from 地图入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" +
               strYear + "'" + "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
            string strBusinesCode = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //图层名称前缀
            //string strPrefix = strBusinesCode + strYear + strSubType + strXzq + strScale;

            //获取影像库
            strExp = "select 影像库 from 地图入库信息表 where 行政代码 ='" + strXzq + "'" + "And " + " 年度='" +
             strYear + "'" + "And " + " 比例尺='" + strScale + "'" + "And " + " 专题类型='" + strSubType + "'";
            string strGridLib = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //创建树  GisMap 根节点 Group 专题节点 Layer 叶子节点
            if (m_tparent == null)
            {
                //根节点
                m_tparent = new TreeNode();
                m_tparent.Text = "地图目录";

                MapDocTree.Nodes.Add(m_tparent);
                MapDocTree.ExpandAll();
                m_tparent.ImageIndex = 18;
                m_tparent.SelectedImageIndex = 18;
            }


            TreeNode tMapNode;
            TreeNode tNewNode;
            string strTblName = "";
            string strRootName = "";
            string strSearchRoot = "//GisMap";
            XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);
            XmlElement xmlElentRoot = (XmlElement)xmlNodeRoot;
            strRootName = xmlElentRoot.GetAttribute("sItemName");
            xmlElentRoot.SetAttribute("sYear", strYear);
            xmlElentRoot.SetAttribute("sSubject", strSubType);
            xmlElentRoot.SetAttribute("sXzqCode", strXzq);
            xmlElentRoot.SetAttribute("sScale", strScale);
            tMapNode = new TreeNode();
            tMapNode.Text = strRootName + "_" + strSuffixx;
            tMapNode.Name = strSubType;
            tMapNode.Checked = true;

            //创建组节点
            IGroupLayer pGroupFLayer = new GroupLayer();
            pGroupFLayer.Name = tMapNode.Text;
            IMapLayers pmaplayers = axMapControl.Map as IMapLayers;
            pmaplayers.InsertLayer(pGroupFLayer, false, pmaplayers.LayerCount);


            m_tparent.Nodes.Add(tMapNode);
            m_tparent.ExpandAll();
            tMapNode.ImageIndex = 9;
            tMapNode.SelectedImageIndex = 9;

            //若是影像库有内容就添加影像节点
            if (strGridLib != "")
            {
                //操作xml文件 插入节点
                XmlElement xmlElemGroup = xmldoc.CreateElement("SubGroup");
                string strGroupName = "【影像数据】";
                xmlElemGroup.SetAttribute("sItemName", strGroupName);
                xmlElemGroup.SetAttribute("sType", "GROUP");


                XmlElement xmlElement = xmldoc.CreateElement("Layer");
                xmlElement.SetAttribute("sDemo", "影像数据");
                string name = strGridLib.Substring(strGridLib.LastIndexOf("\\") + 1);
                xmlElement.SetAttribute("sItemName", "影像数据");
                //xmlElement.SetAttribute("sFile",name);
                xmlElement.SetAttribute("sDataSource", strGridLib);
                xmlElemGroup.AppendChild(xmlElement);

                //string strSearchBuffer = "//SubGroup";
                //XmlNode xmlNdGroupFirst = xmldoc.SelectSingleNode(strSearchBuffer);
                //if (xmlNdGroupFirst != null)
                //{
                //    xmlElentRoot.InsertBefore(xmlElemGroup, xmlNdGroupFirst);

                //}
                //else
                //{
                //    xmlElentRoot.AppendChild(xmlElemGroup);
                //}
                xmlElentRoot.AppendChild(xmlElemGroup);
            }

            xmldoc.Save(strModPath);

            //首先添加第一级子节点 SubGroup
            string strSearch = "//SubGroup";
            XmlNodeList xmlNdList = xmldoc.SelectNodes(strSearch);
            foreach (XmlNode xmlChild in xmlNdList)
            {
                strTblName = "";
                XmlElement xmlElent = (XmlElement)xmlChild;
                strTblName = xmlElent.GetAttribute("sItemName");

                tNewNode = new TreeNode();
                tNewNode.Text = strTblName;
                tNewNode.Checked = true;

                tMapNode.Nodes.Add(tNewNode);
                tMapNode.ExpandAll();
                tNewNode.SelectedImageIndex = 20;
                tNewNode.ImageIndex = 20;

                //添加最终子节点
                AddLeafItem(pGroupFLayer, strSubType, tNewNode, xmlChild, strLayerGroup, strYear, strXzq, strScale);
            }

            //删除属性值为空的节点
            DeleNullElementForXml(xmldoc);
            xmldoc.Save(strModPath);

            //记录日志
            LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
            string strLog = tMapNode.Text + "追加调阅完毕";
            if (log != null)
            {
                log.Writelog(strLog);
            }
            vProgress.SetProgress(strLog);
        }


        //右键响应  弹出图形浏览窗口的右键菜单
        private void axMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            //右键点击的时候弹出右键菜单
        /*    if (e.button == 2)
            {
                System.Drawing.Point ClickPoint = axMapControl.PointToScreen(new System.Drawing.Point(e.x, e.y));
                axMapControlcontextMenu.Show(ClickPoint);
            }*/
        }


        //地图文档树右键响应
        private void MapDocTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            System.Drawing.Point ClickPoint;
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            //树节点的单击事件，变换选中字体颜色和图标
            if (MapDocTree.SelectedNode != e.Node)
            {
                if (MapDocTree.SelectedNode != null)
                {
                    //MapDocTree.SelectedNode.ForeColor = Color.Black;
                }

                MapDocTree.SelectedNode = e.Node;
                //e.Node.ForeColor = Color.Red;
            }



            //设为当前编辑图层
            /*     IFeatureWorkspace pFeatureWorkspace = ModFrameData.v_AppGisUpdate.gisDataSet.WorkSpace as IFeatureWorkspace;
                 Exception exError = null;
                 IFeatureClass pFC = ModFrameData.v_AppGisUpdate.gisDataSet.GetFeatureClass(e.Node.Name.ToString(), out exError);
     */
            //根据树节点名称获取节点的sFile
            //根节点的名称sSubject
            TreeNode tRootNode;
            string strSubject = "";
            string strRootMapText = "";
            if (MapDocTree.SelectedNode != null)
            {
                if (MapDocTree.SelectedNode.Parent != null)
                {
                    tRootNode = MapDocTree.SelectedNode.Parent.Parent;
                    if (tRootNode != null)
                    {
                        strSubject = tRootNode.Name;
                        strRootMapText = tRootNode.Text;
                    }
                }

            }

            //string sFile = GetLayerNameFormNodeName(MapDocTree.SelectedNode.Name, strSubject);//changed by chulili 2011-04-15
            string sFile = GetLayersFileFormNode(MapDocTree.SelectedNode);
            if (sFile != "")
            {
                /*     IFeatureClass pFC = pFeatureWorkspace.OpenFeatureClass(sFile);
                     if (pFC != null)
                     {
                  
                          int iLayerCount = axMapControl.Map.LayerCount;
                         IFeatureLayer pFeatureLayer;
                         IMap pMap = axMapControl.Map;
                         for (int ii = 0; ii < iLayerCount; ii++)
                         {
                             pFeatureLayer = pMap.get_Layer(ii) as IFeatureLayer;
                             if (pFeatureLayer == null)
                                 continue;
                             if (pFeatureLayer.FeatureClass == null)
                                 continue;
                             if (pFeatureLayer.FeatureClass.FeatureClassID == pFC.FeatureClassID)
                             {
                                 axMapControl.CustomProperty = pFeatureLayer;
                                 FeatureTrue = true;
                                 //右键点击的时候弹出右键菜单
                                 if (e.Button == MouseButtons.Right)
                                 {

                                     //必须选中的是叶子节点
                                     ClickPoint = MapDocTree.PointToScreen(new System.Drawing.Point(e.X, e.Y));
                                     MapDocTreecontextMenu.Show(ClickPoint);//changed by xs 20100415 显示矢量数据菜单
                                 }
                                 break;
                             }
                         }
                     }*/

                //根据父亲节点获取组节点的名称  strRootMapText
                int iLayerCount = axMapControl.Map.LayerCount; ;
                IFeatureLayer pFeatureLayer;
                IMap pMap = axMapControl.Map;
                for (int n = 0; n < pMap.LayerCount; n++)
                {
                    ILayer layer = pMap.get_Layer(n);
                    if (layer is IGroupLayer && layer.Name == strRootMapText)
                    {
                        ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                        for (int c = 0; c < Comlayer.Count; c++)
                        {
                            pFeatureLayer = Comlayer.get_Layer(c) as IFeatureLayer;
                            if (pFeatureLayer == null)
                                continue;
                            if (pFeatureLayer.FeatureClass == null)
                                continue;
                            if (pFeatureLayer.Name == MapDocTree.SelectedNode.Name)
                            {
                                axMapControl.CustomProperty = pFeatureLayer;
                                FeatureTrue = true;
                                //右键点击的时候弹出右键菜单
                                if (e.Button == MouseButtons.Right)
                                {

                                    //必须选中的是叶子节点
                                    ClickPoint = MapDocTree.PointToScreen(new System.Drawing.Point(e.X, e.Y));
                                    MapDocTreecontextMenu.Show(ClickPoint);//changed by xs 20100415 显示矢量数据菜单
                                }
                                break;
                            }
                        }
                    }
                }

            }
            else//影像图层显示状态
            {
                sFile = GetRasterLayerFormNodeName(MapDocTree.SelectedNode.Name, strSubject);
                int iLayerCount = axMapControl.Map.LayerCount;
                IRasterLayer pRasterLayer;
                IMap pMap = axMapControl.Map;
                for (int ii = 0; ii < iLayerCount; ii++)
                {
                    ILayer layer = pMap.get_Layer(ii);
                    if (layer is IGroupLayer && layer.Name == strRootMapText)
                    {
                        ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                        for (int c = 0; c < Comlayer.Count; c++)
                        {
                            pRasterLayer = Comlayer.get_Layer(c) as IRasterLayer;
                            if (pRasterLayer == null)
                                continue;
                            if (pRasterLayer.Name.Substring(0, pRasterLayer.Name.LastIndexOf("\\")).ToUpper() == sFile.ToUpper())
                            {
                                axMapControl.CustomProperty = pRasterLayer;
                                FeatureTrue = false;
                                //右键点击的时候弹出右键菜单
                                if (e.Button == MouseButtons.Right)
                                {
                                    //必须选中的是叶子节点
                                    ClickPoint = MapDocTree.PointToScreen(new System.Drawing.Point(e.X, e.Y));
                                    MapDocTreeRastercontextMenu.Show(ClickPoint);//changed by xs 20100415 显示影像数据菜单

                                }
                                break;
                            }
                        }
                    }
                }
            }
        }


   
        //地图文档树右键菜单功能响应
        #region
        private void ClearSeletable()
        {
            int iLayerCount = axMapControl.Map.LayerCount;
            IFeatureLayer pFeatureLayer; 
            IMap pMap = axMapControl.Map;
            for (int n = 0; n < iLayerCount; n++)
            {
                ILayer layer = pMap.get_Layer(n);
                if (layer is IGroupLayer)
                {
                    ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                    for (int c = 0; c < Comlayer.Count; c++)
                    {
                        pFeatureLayer = Comlayer.get_Layer(c) as IFeatureLayer;
                        if (pFeatureLayer != null) pFeatureLayer.Selectable = false;

                    }
                }
            }
        }
        private void SetAllSeletable()
        {
            int iLayerCount = axMapControl.Map.LayerCount;
            IFeatureLayer pFeatureLayer;
            IMap pMap = axMapControl.Map;
            for (int n = 0; n < iLayerCount; n++)
            {
                ILayer layer = pMap.get_Layer(n);
                if (layer is IGroupLayer)
                {
                    ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                    for (int c = 0; c < Comlayer.Count; c++)
                    {
                        pFeatureLayer = Comlayer.get_Layer(c) as IFeatureLayer;
                        if (pFeatureLayer != null) pFeatureLayer.Selectable = true;

                    }
                }
            }
        }
        //设置图层为当前编辑项
        private void MenuItemSetEditFlag_Click(object sender, EventArgs e)
        {
            TreeNode tSelNode; IFeatureLayer pOldLayer, pNewLayer;
            tSelNode = MapDocTree.SelectedNode;
            if (m_CurEditLayerNode != null)
            {
                m_CurEditLayerNode.ForeColor = Color.Black;
                pOldLayer = GetLayerofTreeNode(m_CurEditLayerNode) as IFeatureLayer ;
                if (pOldLayer != null) pOldLayer.Selectable = false;
            }
            else
            {
                ClearSeletable();
            }
            tSelNode.ForeColor = Color.Red;
            m_CurEditLayerNode = tSelNode;
            m_CurEditTopicNode = tSelNode.Parent.Parent;
            pNewLayer = GetLayerofTreeNode(tSelNode) as IFeatureLayer;
            if (pNewLayer!=null) pNewLayer.Selectable = true;
        }

        //取消当前图层的当前编辑状态
        private void MenuItemReMoveEditFalg_Click(object sender, EventArgs e)
        {
            TreeNode tSelNode;
            tSelNode = MapDocTree.SelectedNode;
            if (tSelNode.ForeColor == Color.Red)
                tSelNode.ForeColor = Color.Black;
            if (m_CurEditLayerNode == null)
                return;
            if (m_CurEditLayerNode.Equals(tSelNode))
            {
                m_CurEditLayerNode = null;
                m_CurEditTopicNode = null;
                SetAllSeletable();
            }
            
        }


        //视图缩放到当前图层范围
        private void MenuItemZoomToLayer_Click(object sender, EventArgs e)
        {
            ILayer pLayer = null;
            pLayer = (ILayer)axMapControl.CustomProperty;
            if (pLayer == null)
                return;
            IActiveView pActiveView = axMapControl.ActiveView;
            pActiveView.Extent = pLayer.AreaOfInterest;
            pActiveView.Refresh();
        }

        //符号设置
        private void MenuItemSetSymbol_Click(object sender, EventArgs e)
        {
            IFeatureLayer pLayer = (IFeatureLayer)axMapControl.CustomProperty;
            if (pLayer == null)
                return;

            try
            {
                GeoSymbology.frmSymbology frm = new GeoSymbology.frmSymbology(pLayer);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    ESRI.ArcGIS.Carto.IGeoFeatureLayer pGeoLayer = pLayer as ESRI.ArcGIS.Carto.IGeoFeatureLayer;
                    pGeoLayer.Renderer = frm.FeatureRenderer();
                    axMapControl.ActiveView.Refresh();
                    // _AppHk.TOCControl.Update();

                    //保存当前的符号信息
                    string strLyrName = pLayer.Name;
                    if (pGeoLayer.FeatureClass != null)
                    {
                        IDataset pDataset = pGeoLayer.FeatureClass as IDataset;
                        strLyrName = pDataset.Name;
                    }
                    strLyrName = strLyrName.Substring(strLyrName.IndexOf('.') + 1);

                    /*  XmlDocument vDoc = new XmlDocument();
                      vDoc.Load(System.Windows.Forms.Application.StartupPath + "\\..\\Template\\SymbolInfo.xml");
                      UpdateSymbolInfo(strLyrName, vDoc, pGeoLayer.Renderer);
                      vDoc.Save(System.Windows.Forms.Application.StartupPath + "\\..\\Template\\SymbolInfo.xml");*/
                }
            }
            catch
            {
                return;
            }
        }

        //图层属性记录数值
        private void MenuItemOpenLayerAtt_Click(object sender, EventArgs e)
        {
            ILayer pLayer = axMapControl.CustomProperty as ILayer;
            frmAttributeTable frm = new frmAttributeTable(axMapControl);
            frm.CreateAttributeTable(pLayer);
            frm.ShowDialog();

            string strName = "查看" + pLayer.Name + "图层数据信息";
            LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
            if (log != null)
            {
                log.Writelog(strName);
            }
        }

        //图层属性信息
        private void MenuItemLayerAtt_Click(object sender, EventArgs e)
        {
            ILayer pLayer = axMapControl.CustomProperty as ILayer;
            if (pLayer != null)
            {

                string strName = "";
                if (FeatureTrue)//矢量数据
                {
                    strName = "查看" + pLayer.Name + "图层属性值信息";
                }
                else//栅格数据
                {
                    strName = "查看影像数据图层属性值信息";
                }
                LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                if (log != null)
                {
                    log.Writelog(strName);
                }
                frmLayerProperties layerProDialog = new frmLayerProperties(pLayer, axMapControl.ActiveView,FeatureTrue);
                layerProDialog.Show();
            }
        }

        //导出数据        
        #region
        //==================================================
        //作者：席胜
        //日期：2011.03.02
        //导出数据  数据另存为mdb\shp格式
        //==================================================
        private void MenuItemOutputLayer_Click(object sender, EventArgs e)
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "";
            dlg.Filter = "mdb数据|*.mdb|shp数据|*.shp";
            dlg.FilterIndex = 1;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (1 == dlg.FilterIndex)
                {
                    ExportMdb(dlg);
                }
                else if (2 == dlg.FilterIndex)
                {
                    ExportShp(dlg);
                }
            }
        }

        //导出MDB数据
        private void ExportMdb(SaveFileDialog dlg)
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            //获取模板路径
            string sourcefilename = Application.StartupPath + "\\..\\Template\\DATATEMPLATE.mdb";
            //从workspace中获取对应的layer
         /*   string dbpath = dIndex.GetDbValue("dbServerPath");
            IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(dbpath, 0);*/
            string strSubject = "";
            if (MapDocTree.SelectedNode != null)
            {
                if (MapDocTree.SelectedNode.Parent != null)
                {
                    if (MapDocTree.SelectedNode.Parent.Parent != null)
                    {
                        strSubject = MapDocTree.SelectedNode.Parent.Parent.Name;
                    }
                }

            }

            //string strLayerName = GetLayerNameFormNodeName(MapDocTree.SelectedNode.Name, strSubject);//changed by chulili 2011-04-15
            string strLayerName = GetLayersFileFormNode(MapDocTree.SelectedNode);
            IFeatureWorkspace pFeaWorkspace;
            //如果m_strnew !="" 新增加的
            if (m_strnew.Trim() != "")
            {
                pFeaWorkspace = GetWorkspaceByFileName2(strLayerName);
                strLayerName = strLayerName.Substring(strLayerName.LastIndexOf("\\") + 1);
                strLayerName = System.IO.Path.GetFileNameWithoutExtension(strLayerName);
            }
            else
            {
                //若是m_strnew== "" 维持原来代码
                pFeaWorkspace = GetWorkspaceByFileName(strLayerName);
            }
            IWorkspace pWorkspace = pFeaWorkspace as IWorkspace;
            try
            {
                if (File.Exists(sourcefilename))//原模板存在
                {
                   // DialogResult result = MessageBox.Show("导出是否去掉前缀？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    //如果存在mdb,替换文件，则复制模板到指定路径
                    //如果存在mdb，不替换，则追加到这个文件
                    File.Copy(sourcefilename, dlg.FileName, true);
                    IWorkspaceFactory Pwf = new AccessWorkspaceFactoryClass();
                    IWorkspace pws = (IWorkspace)(Pwf.OpenFromFile(dlg.FileName, 0));
                    IWorkspace2 pws2 = (IWorkspace2)pws;
                    ILayer pLayer = axMapControl.CustomProperty as ILayer;
                    if (pLayer != null)
                    {
                        IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                        if (pFeatureLayer.Visible)
                        {
                            string cellvalue;
                            if(m_strnew.Trim() != "")
                                cellvalue=strLayerName;
                            else{
                                cellvalue = pFeatureLayer.FeatureClass.AliasName;
                            }
                           // if (result == DialogResult.Yes) cellvalue = cellvalue.Substring(15);//去掉前缀
                            if (pws2.get_NameExists(esriDatasetType.esriDTFeatureClass, cellvalue))
                            {
                                IFeatureClass tmpfeatureclass;
                                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pws;
                                tmpfeatureclass = pFeatureWorkspace.OpenFeatureClass(cellvalue);
                                IDataset set = tmpfeatureclass as IDataset;
                                set.CanDelete();
                                set.Delete();
                            }
                            IFeatureDataConverter_ConvertFeatureClass(pWorkspace, pws, cellvalue, cellvalue);
                        }
                    }
                    MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                    if (log != null)
                    {
                        log.Writelog("导出图层到" + dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //导出SHP数据
        private void ExportShp(SaveFileDialog dlg)
        {
            string file = dlg.FileName.Substring(0, dlg.FileName.LastIndexOf('\\'));
            if (!System.IO.Directory.Exists(file))
            {
                System.IO.Directory.CreateDirectory(file);
            }
            try
            {
                ILayer pLayer = axMapControl.CustomProperty as ILayer;
                if (pLayer != null)
                {
                    IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                    if (pFeatureLayer.Visible)
                    {
                        ExportFeature(pFeatureLayer.FeatureClass, dlg.FileName);
                    }
                }
                MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("导出失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        //导出shp数据
        public void ExportFeature(IFeatureClass pInFeatureClass, string pPath)
        {

            // create a new Access workspace factory  
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();//需要ESRI.ArcGIS.DataSourcesFile命名空间;
            string parentPath = pPath.Substring(0, pPath.LastIndexOf('\\'));
            string fileName = pPath.Substring(pPath.LastIndexOf('\\') + 1, pPath.Length - pPath.LastIndexOf('\\') - 1);
            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(parentPath, fileName, null, 0);
            // Cast for IName        
            IName name = (IName)pWorkspaceName;

            //Open a reference to the access workspace through the name object        
            IWorkspace pOutWorkspace = (IWorkspace)name.Open();

            IDataset pInDataset = pInFeatureClass as IDataset;
            IFeatureClassName pInFCName = pInDataset.FullName as IFeatureClassName;
            IWorkspace pInWorkspace = pInDataset.Workspace;
            IDataset pOutDataset = pOutWorkspace as IDataset;
            IWorkspaceName pOutWorkspaceName = pOutDataset.FullName as IWorkspaceName;
            IFeatureClassName pOutFCName = new FeatureClassNameClass();
            IDatasetName pDatasetName = pOutFCName as IDatasetName;
            pDatasetName.WorkspaceName = pOutWorkspaceName;
            pDatasetName.Name = pInFeatureClass.AliasName;
            IFieldChecker pFieldChecker = new FieldCheckerClass();
            pFieldChecker.InputWorkspace = pInWorkspace;
            pFieldChecker.ValidateWorkspace = pOutWorkspace;
            IFields pFields = pInFeatureClass.Fields;
            IFields pOutFields;
            IEnumFieldError pEnumFieldError;
            pFieldChecker.Validate(pFields, out pEnumFieldError, out pOutFields);
            IFeatureDataConverter pFeatureDataConverter = new FeatureDataConverterClass();
            pFeatureDataConverter.ConvertFeatureClass(pInFCName, null, null, pOutFCName, null, pOutFields, "", 100, 0);
        }

        /// <summary>
        /// 导出jpg图片格式数据
        /// </summary>
        /// <param name="dlg"></param>
        #region
        private void ExportJpg(SaveFileDialog dlg)
        {
            try
            {
                IExport pExport = new ExportJPEGClass();//IExport需要using ESRI.ArcGIS.Output命名空间;
                pExport.ExportFileName = dlg.FileName;

                //设置参数 默认精度
                int reslution = 96;
                pExport.Resolution = reslution;
                //获取导出范围
                tagRECT exportRect = axMapControl.ActiveView.ExportFrame;//tagRECT需要 ESRI.ArcGIS.Display;
                ESRI.ArcGIS.Geometry.IEnvelope Env = new ESRI.ArcGIS.Geometry.EnvelopeClass();//tagRECT需要 ESRI.ArcGIS.Geometry;
                Env.PutCoords(exportRect.left, exportRect.top, exportRect.right, exportRect.bottom);
                pExport.PixelBounds = Env;
                //开始导出,get DC  
                int hDC = pExport.StartExporting();
                axMapControl.ActiveView.Output(hDC, reslution, ref exportRect, null, null);
                pExport.FinishExporting();
                pExport.Cleanup();
                MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("导出失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        #endregion
        #endregion

        //右击索引树弹出属性对话框
        private void MenuItemAtt_Click(object sender, EventArgs e)
        {
            frmMapProperty frm = new frmMapProperty();
            frm.ThisNode = DataUnitTree.SelectedNode;
            if (frm.ThisNode != null)
            {
                string strName = "查看" + frm.ThisNode.Parent.Text + frm.ThisNode.Text + "数据属性信息";
                LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                if (log != null)
                {
                    log.Writelog(strName);
                }

                frm.Show();
            }
        }

        #endregion


        //视图浏览窗口右键菜单的实现        
        #region
        ///<summary>
        ///说明：图层右键菜单的实现
        ///作者：席胜
        ///日期：2011-03-01
        ///</summary>

        //右键点击放大
        private void MapZoomInMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomInToolClass();
            pCommand.OnCreate(axMapControl.Object);
            axMapControl.CurrentTool = pCommand as ESRI.ArcGIS.SystemUI.ITool;
        }

        //右键点击缩小
        private void ZoomOutMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomOutToolClass();
            pCommand.OnCreate(axMapControl.Object);
            axMapControl.CurrentTool = pCommand as ESRI.ArcGIS.SystemUI.ITool;
        }

        //右键点击漫游
        private void MapPanMenuItem_Click(object sender, EventArgs e)
        {

            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapPanToolClass();
            pCommand.OnCreate(axMapControl.Object);
            axMapControl.CurrentTool = pCommand as ESRI.ArcGIS.SystemUI.ITool;
        }

        //右键点击中心缩小
        private void MapZoomOutFixedMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomOutFixedCommandClass();
            pCommand.OnCreate(axMapControl.Object);
            pCommand.OnClick();
        }

        //右键点击中心放大
        private void MapZoomInFixedMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomInFixedCommandClass();
            pCommand.OnCreate(axMapControl.Object);
            pCommand.OnClick();
        }

        //右键点击刷新
        private void MapRefreshMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapRefreshViewCommandClass();
            pCommand.OnCreate(axMapControl.Object);
            pCommand.OnClick();
        }

        //右键点击全屏
        private void MapFullExtentMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapFullExtentCommandClass();
            pCommand.OnCreate(axMapControl.Object);
            pCommand.OnClick();
        }

        //右键点击后景
        private void BackCommandMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomToLastExtentBackCommandClass();
            pCommand.OnCreate(axMapControl.Object);
            pCommand.OnClick();
        }

        //右键点击前景
        private void ForwardCommandMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomToLastExtentForwardCommandClass();
            pCommand.OnCreate(axMapControl.Object);
            pCommand.OnClick();
        }

        #endregion

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetailLog frm = new frmDetailLog(m_strLogFilePath);
            frm.Show();
        }

        //双击地图文档树
        private void MapDocTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode tSelNode;
            tSelNode = e.Node;
            if (tSelNode != null && tSelNode.Level == 3)
            {
                if (tSelNode.Parent.Text != "【影像数据】")
                {
                    if (m_CurEditLayerNode != null)
                    {
                        if (m_CurEditLayerNode.Equals(tSelNode))
                        {
                            m_CurEditLayerNode.ForeColor = Color.Black;
                            m_CurEditLayerNode = null;
                            m_CurEditTopicNode = null;
                            SetAllSeletable();
                        }
                        else
                        {
                            m_CurEditLayerNode.ForeColor = Color.Black;
                            IFeatureLayer pOldEditLayer = GetLayerofTreeNode(m_CurEditLayerNode) as IFeatureLayer ;
                            if (pOldEditLayer!=null) pOldEditLayer.Selectable = false;
                            tSelNode.ForeColor = Color.Red;
                            m_CurEditLayerNode = tSelNode;
                            m_CurEditTopicNode = tSelNode.Parent.Parent;
                            IFeatureLayer pNewEditLayer = GetLayerofTreeNode(tSelNode) as IFeatureLayer;
                            if (pNewEditLayer!=null) pNewEditLayer.Selectable = true;
                        }
                    }
                    else
                    {
                        ClearSeletable();
                        tSelNode.ForeColor  = Color.Red;
                        m_CurEditLayerNode = tSelNode;
                        m_CurEditTopicNode = tSelNode.Parent.Parent;
                        IFeatureLayer pNewEditLayer = GetLayerofTreeNode(tSelNode) as IFeatureLayer;
                        if (pNewEditLayer!=null) pNewEditLayer.Selectable = true;
                    }
                }
            }
        }

        private void WordToRtf(object docPath)
        {
            try
            {
                System.IO.FileStream fs = null;
                if (!File.Exists((string)docPath))
                {
                    fs = new System.IO.FileStream((string)docPath, FileMode.Create, FileAccess.Write);
                }
                // File.SetAttributes((string)docPath, FileAttributes.Hidden);
                //BinaryWriter bw = new BinaryWriter(fs);
                //bw.Write((Source);

                // 指定原文件和目标文件
                object Target = Application.StartupPath + "\\temp.rtf";
                // 缺省参数  
                object Unknown = Type.Missing;
                object saveChanges = false;
                object readOnly = true;
                wordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
                // 打开doc文件
                doc = wordApp.Documents.Open(ref docPath, ref Unknown,
                      ref readOnly, ref Unknown, ref Unknown,
                      ref Unknown, ref Unknown, ref Unknown,
                      ref Unknown, ref Unknown, ref Unknown,
                      ref Unknown, ref Unknown, ref Unknown,
                      ref Unknown, ref Unknown);
                doc.Activate();

                // 指定另存为格式(rtf)
                object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatRTF;

                // 转换格式
                doc.SaveAs(ref Target, ref format,
                           ref Unknown, ref Unknown, ref Unknown,
                           ref Unknown, ref Unknown, ref Unknown,
                           ref Unknown, ref Unknown, ref Unknown,
                           ref Unknown, ref Unknown, ref Unknown,
                           ref Unknown, ref Unknown);

                // 关闭文档和Word程序
                doc.Close(ref saveChanges, ref Unknown, ref Unknown);
                object o = Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
                wordApp = null;

                #region 结束创建的word应用进程
                List<int> arr = new List<int>();
                GetWordApp(arr);
                foreach (int i in arr)
                {
                    if (!wordProcess.Exists(delegate(int a) { return a == i; }))
                    {
                        KillOwnWordApp(i);
                    }
                }
                #endregion
            }
            catch
            {

                #region 结束创建的word应用进程
                List<int> arr = new List<int>();
                GetWordApp(arr);
                foreach (int i in arr)
                {
                    if (!wordProcess.Exists(delegate(int a) { return a == i; }))
                    {
                        KillOwnWordApp(i);
                    }
                }
                #endregion
            }
        }

        //获取word进程Pid
        private void GetWordApp(List<int> T)
        {
            Process[] ps = Process.GetProcessesByName("WINWORD");
            foreach (Process process in ps)
            {
                T.Add(process.Id);
            }
        }

        //结束指定word进程
        private void KillOwnWordApp(int Pid)
        {
            Process ps = Process.GetProcessById(Pid);
            ps.Kill();
            GC.Collect();
        }

        //外部打开文档
        private void MenuItemOutopen_Click(object sender, EventArgs e)
        {
            object oMissing = System.Reflection.Missing.Value;
            wordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
            wordApp.Visible = true;
            object fileName = @docPath;
            doc = wordApp.Documents.Open(ref fileName,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            doc.Activate();

        }


        //另存为
        private void MenuItemSaveas_Click(object sender, EventArgs e)
        {
            string Targetpath = "";
            string Sourcepath = docPath.ToString();
            string name = Sourcepath.Substring(Sourcepath.LastIndexOf("\\") + 1);
            FolderBrowserDialog dg = new FolderBrowserDialog();
            if (dg.ShowDialog() == DialogResult.OK)
            {
                Targetpath = dg.SelectedPath + "\\" + name;


                if (File.Exists(Sourcepath))
                {
                    File.Copy(Sourcepath, Targetpath, true);
                    MessageBox.Show("另存为成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string strName = "另存文档" + name + ",目标路径为:" + Targetpath;
                    LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                    if (log != null)
                    {
                        log.Writelog(strName);
                    }
                }
            }
        }
        //文档属性
        private void MenuItemDocatt_Click(object sender, EventArgs e)
        {

        }



        //移除图层
        private void MenuItemRemovelayer_Click(object sender, EventArgs e)
        {
            //删除树节点
            if (MapDocTree.SelectedNode != null)
            {
                //记录日志
                LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                string strLog = "移除图层" + MapDocTree.SelectedNode.Text;
                if (log != null)
                {
                    log.Writelog(strLog);
                }


                MapDocTree.SelectedNode.Remove();
                // MapDocTree.update();
            }
            ILayer pLayer = null;
            pLayer = axMapControl.CustomProperty as ILayer;
            if (pLayer == null)
                return;

            //删除workspace中的featureclass
            axMapControl.Map.DeleteLayer(pLayer);

            axMapControl.ActiveView.Refresh();
        }

        //设置标注
        private void MenuItemSetLabel_Click(object sender, EventArgs e)
        {
            frmSetLabel pFrmSetLabel = new frmSetLabel();
            ILayer pLayer = null;
            pLayer = axMapControl.CustomProperty as ILayer;
            if (pLayer == null)
                return;
            if (pLayer is IGeoFeatureLayer)
            {
                pFrmSetLabel.GeoFeatLayer = pLayer as IGeoFeatureLayer;
                pFrmSetLabel.MapControl = axMapControl.Object as IMapControlDefault;
                pFrmSetLabel.ShowDialog();
            }
        }

        //移除标注
        private void MenuItemRemoveLabel_Click(object sender, EventArgs e)
        {
            ILayer pLayer = null;
            pLayer = axMapControl.CustomProperty as ILayer;
            if (pLayer == null)
                return;
            IGeoFeatureLayer pGeoFeatureLayer = null;
            if (pLayer is IGeoFeatureLayer)
            {
                pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
                if (pGeoFeatureLayer.DisplayAnnotation == true)
                {
                    pGeoFeatureLayer.DisplayAnnotation = false;
                }
                axMapControl.ActiveView.Refresh();
            }
        }
        //获取某个node节点所对应的图层
        private ILayer GetLayerofTreeNode(TreeNode tnode)
        {
            if (tnode == null) return null;
            if (tnode.Level < 3) return null;

            Exception exError = null;

            IMap pMap = axMapControl.Map;

            string strRootMapText = "";
            //获取专题名称
            if (tnode.Parent != null)
            {
                if (tnode.Parent.Parent != null)
                {
                    strRootMapText = tnode.Parent.Parent.Text;
                }
            }
             
            for (int n = 0; n < pMap.LayerCount; n++)
            {
                ILayer layer = pMap.get_Layer(n);
                if (layer is IGroupLayer && layer.Name == strRootMapText)
                {
                    ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                    for (int c = 0; c < Comlayer.Count; c++)
                    {
                        IFeatureLayer  pFeatureLayer = Comlayer.get_Layer(c) as IFeatureLayer;
                        if (pFeatureLayer == null)
                            continue;
                        if (pFeatureLayer.FeatureClass == null)
                            continue;
                        if (pFeatureLayer.Name == tnode.Name)
                        {
                            return pFeatureLayer as ILayer;
                        }
                    }
                }
            }
            return null;
        }
        //获取某个node节点所对应的图层的index
        private int GetLayerIndexofTreeNode(TreeNode tnode)
        {
            if (tnode == null) return -1;
            if (tnode.Level < 3) return -1;
            Exception exError = null;

            int iLayerCount = axMapControl.Map.LayerCount;
            IFeatureLayer pFeatureLayer; ILayer pLayer;
            IMap pMap = axMapControl.Map;

            string strRootMapText = "";

            if (tnode.Parent != null)
            {
                if (tnode.Parent.Parent != null)
                {
                    strRootMapText = tnode.Parent.Parent.Text;
                }
            }
            for (int n = 0; n < pMap.LayerCount; n++)
            {
                ILayer layer = pMap.get_Layer(n);
                if (layer is IGroupLayer && layer.Name == strRootMapText)
                {
                    ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                    for (int c = 0; c < Comlayer.Count; c++)
                    {
                        pFeatureLayer = Comlayer.get_Layer(c) as IFeatureLayer;
                        if (pFeatureLayer == null)
                            continue;
                        if (pFeatureLayer.FeatureClass == null)
                            continue;
                        if (pFeatureLayer.Name == tnode.Name)
                        {
                            return c;
                        }
                    }
                }
            }

            return -1;
        }

        //节点拖拽
        private void MapDocTree_DragDrop(object sender, DragEventArgs e)
        {
            //获得拖放中的节点 
            if (m_LastDragNode != null)
            {
                m_LastDragNode.BackColor = SystemColors.Window;//取消上一个被放置的节点高亮显示   

                m_LastDragNode.ForeColor = SystemColors.WindowText;
                m_LastDragNode = null;
            }

            TreeNode moveNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                moveNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error","系统提示",MessageBoxButtons.OK ,MessageBoxIcon.Information );
            }
            if (moveNode.Level < 3)
                return;

            //根据鼠标坐标确定要移动到的目标节点  
            System.Drawing.Point pt;
            TreeNode targeNode;
            pt = ((TreeView)(sender)).PointToClient(new System.Drawing.Point(e.X, e.Y));
            targeNode = MapDocTree.GetNodeAt(pt);
            if (targeNode.Level < 2)
                return;
            if (targeNode.Level == 2)
            {
                if (!moveNode.Parent.Parent.Equals(targeNode.Parent))
                    return;
            }
            else
            {
                if (!moveNode.Parent.Parent.Equals(targeNode.Parent.Parent))
                    return;
            }
            IMap pMap = axMapControl.Map;
            string GroupLayerName = moveNode.Parent.Parent.Text;
            IGroupLayer pGroupLayer = GetGroupLayerByName(GroupLayerName) as IGroupLayer ;
            //如果目标节点无子节点则添加为同级节点,反之添加到下级节点的未端  
            TreeNode NewMoveNode = (TreeNode)moveNode.Clone();
            int newindex, oldindex; ILayer pmovelayer;
            oldindex = GetLayerIndexofTreeNode(moveNode);  //老节点在map中的索引号
            newindex = GetLayerIndexofTreeNode(targeNode); //新节点在map中的索引号
            pmovelayer = GetLayerofTreeNode(moveNode);
            int newnodeindex, oldnodeindex;
            oldnodeindex = GetAbsoluteIndexofNode(moveNode);
            newnodeindex = GetAbsoluteIndexofNode(targeNode);
            IMapLayers pmaplayers = axMapControl.Map as IMapLayers;
            TreeNode tmpnode;
            switch (targeNode.Level)
            {
                case 3:     //如果目标节点是叶子节点
                    if (targeNode.Parent.Parent != moveNode.Parent.Parent)
                        return;
                    if (oldindex < newindex)
                    {
                        targeNode.Parent.Nodes.Insert(targeNode.Index + 1, NewMoveNode);
                    }
                    else
                    {
                        targeNode.Parent.Nodes.Insert(targeNode.Index, NewMoveNode);
                    }
                    moveNode.Remove();      //删除旧节点

                    //axMapControl.MoveLayerTo(oldindex, newindex);   //修改图层顺序
                    
                    pmaplayers.MoveLayerEx(pGroupLayer, pGroupLayer, pmovelayer, newindex);
                    break;
                case 2:   //如果目标节点是2级节点
                    if (targeNode.Nodes.Count == 0)   //如果该父节点没有子节点
                    {
                        tmpnode = GetprevLeafNode(targeNode);
                        if (tmpnode != null)
                        {
                            newindex = GetLayerIndexofTreeNode(tmpnode);
                            if (newindex < oldindex)
                                newindex = newindex + 1;
                        }
                        else
                        {
                            newindex = 0;
                        }
                    }
                    else  //如果该父节点有子节点
                    {
                        newindex = GetLayerIndexofTreeNode(targeNode.LastNode);
                        if (newindex < oldindex)
                            newindex = newindex + 1;
                    }

                    targeNode.Nodes.Insert(targeNode.Nodes.Count, NewMoveNode); //添加新节点
                    moveNode.Remove();          //删除旧节点
                    //axMapControl.MoveLayerTo(oldindex, newindex);       //修改图层顺序
                    pmaplayers.MoveLayerEx(pGroupLayer, pGroupLayer, pmovelayer, newindex);
                    break;


            }
            //}

            //更新当前拖动的节点选择  
            MapDocTree.SelectedNode = NewMoveNode;

            //展开目标节点,便于显示拖放效果  
            targeNode.Expand();
            //移除拖放的节点  

            //刷新当前视图
            axMapControl.ActiveView.Refresh();

        }
        //获得与给定节点相邻的下一个叶子节点
        private TreeNode GetnextLeafNode(TreeNode pnode)
        {
            const int leafLevel = 3;
            if (pnode.NextNode == null)
                if (pnode.Level > 0)
                    return GetnextLeafNode(pnode.Parent);
                else
                    return null;
            switch (pnode.Level)
            {
                case leafLevel://3
                    if (pnode.NextNode != null)
                        return pnode.NextNode;
                    break;
                case leafLevel - 1://2
                    if (pnode.NextNode != null)
                        if (pnode.NextNode.Nodes.Count > 0)
                            return pnode.NextNode.FirstNode;
                        else
                            return GetnextLeafNode(pnode.NextNode);
                    break;
                case leafLevel - 2://1
                    if (pnode.NextNode != null)
                    {
                        if (pnode.NextNode.Nodes.Count > 0)
                            if (pnode.NextNode.FirstNode.Nodes.Count > 0)
                                return pnode.NextNode.FirstNode.FirstNode;
                            else
                                return GetnextLeafNode(pnode.NextNode.FirstNode);
                        else
                            return GetnextLeafNode(pnode.NextNode);

                    }
                    break;
                case 0:
                    return null;

            }
            return null;
        }
        //获得与给定节点相邻的上一个叶子节点
        private TreeNode GetprevLeafNode(TreeNode pnode)
        {
            const int leafLevel = 3;
            if (pnode.PrevNode == null)
                if (pnode.Level > 0)
                    return GetprevLeafNode(pnode.Parent);
                else
                    return null;
            switch (pnode.Level)
            {
                case leafLevel://3
                    if (pnode.PrevNode != null)
                        return pnode.PrevNode;
                    break;
                case leafLevel - 1://2
                    if (pnode.PrevNode != null)
                        if (pnode.PrevNode.Nodes.Count > 0)
                            return pnode.PrevNode.LastNode;
                        else
                            return GetprevLeafNode(pnode.PrevNode);
                    break;
                case leafLevel - 2://1
                    if (pnode.PrevNode != null)
                    {
                        if (pnode.PrevNode.Nodes.Count > 0)
                            if (pnode.PrevNode.LastNode.Nodes.Count > 0)
                                return pnode.PrevNode.LastNode.LastNode;
                            else
                                return GetprevLeafNode(pnode.PrevNode.LastNode);
                        else
                            return GetprevLeafNode(pnode.PrevNode);

                    }
                    break;
                case 0:
                    return null;

            }
            return null;
        }
        private void MapDocTree_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MapDocTree_ItemDrag(object sender, ItemDragEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }

        }

        private void MapDocTree_DragOver(object sender, DragEventArgs e)
        {
            //当光标悬停在 TreeView 控件上时，展开该控件中的 TreeNode   
            TreeNode moveNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                moveNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error","系统提示",MessageBoxButtons.OK ,MessageBoxIcon.Information );
            }
            if (moveNode.Level != 3)
                return;

            System.Drawing.Point p = this.MapDocTree.PointToClient(new System.Drawing.Point(e.X, e.Y));

            TreeNode tn = this.MapDocTree.GetNodeAt(p);

            //设置拖放目标TreeNode的背景色   


            if (m_LastDragNode != null && tn != m_LastDragNode)
            {
                m_LastDragNode.BackColor = SystemColors.Window;//取消上一个被放置的节点高亮显示   

                m_LastDragNode.ForeColor = SystemColors.WindowText;
            }
            else
            {
                moveNode.BackColor = SystemColors.Window;//取消上一个被放置的节点高亮显示   

                moveNode.ForeColor = SystemColors.WindowText;
            }

            if (m_LastDragNode != tn)
            {
                tn.BackColor = SystemColors.Highlight;

                tn.ForeColor = SystemColors.HighlightText;

            }
            m_LastDragNode = tn;

        }


        private void axMapControl_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            if (ModFrameData.v_AppGisUpdate.ScaleBoxList.Count > 0)
            {
                foreach (ComboBoxItem item in ModFrameData.v_AppGisUpdate.ScaleBoxList)
                {
                    if (item.Name.Equals("scale"))
                    {
                        item.ComboBoxEx.Text = "1:" + axMapControl.Map.MapScale.ToString("0.00");
                        break;
                    }
                }
            }
        }
        private int GetAbsoluteIndexofNode(TreeNode node)
        {
            if (node.Level != 3)
                return -1;
            int ind0, ind1, ind2, ind3; //各级索引
            int tmpind0, tmpind1, tmpind2, tmpind3;
            ind0 = node.Parent.Parent.Parent.Index;
            ind1 = node.Parent.Parent.Index;
            ind2 = node.Parent.Index;
            ind3 = node.Index;
            TreeNode node0;
            node0 = node.Parent.Parent.Parent;
            int AbsoluteIndex = 0;
            foreach (TreeNode node1 in node0.Nodes)
            {
                if (node1.Index >= ind1)
                    continue;
                foreach (TreeNode node2 in node1.Nodes)
                {
                    //AbsoluteIndex = AbsoluteIndex + node2.Nodes.Count;
                    foreach (TreeNode node3 in node2.Nodes)
                    {
                        AbsoluteIndex = AbsoluteIndex + 1;
                    }
                }

            }
            TreeNode node01;
            node01 = node.Parent.Parent;
            foreach (TreeNode node2 in node01.Nodes)
            {
                if (node2.Index >= ind2)
                    continue;
                //AbsoluteIndex = AbsoluteIndex + node2.Nodes.Count;
                foreach (TreeNode node3 in node2.Nodes)
                {
                    AbsoluteIndex = AbsoluteIndex + 1;
                }
            }
            TreeNode node02 = node.Parent;
            foreach (TreeNode node3 in node02.Nodes)
            {
                if (node3.Index >= ind3)
                    continue;
                AbsoluteIndex = AbsoluteIndex + 1;
            }
            //AbsoluteIndex = AbsoluteIndex + ind3;

            return AbsoluteIndex;

        }

        //==============文档目录相关===============================
        //文档目录树
        private void TextDoctree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 2)
            {
                string strsouce = e.Node.Parent.Name;
                string strdir = e.Node.Name;
                string strExp = "";
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                // string mypath = dIndex.GetDbValue("dbServerPath");
                string mypath = dIndex.GetDbInfo();
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串

                if (strsouce != "")
                {
                    strExp = "select 路径 from 文档数据源信息表 where 虚拟目录名='" + strsouce + "'";
                    strsouce = db.GetInfoFromMdbByExp(strCon, strExp);

                    docPath = strsouce + "\\" + strdir;
             //       RichBoxWordDoc.Clear();
                    this.Cursor = Cursors.WaitCursor;
                    GetWordApp(wordProcess);//获取客户端现有的word进程
                    try
                    {
                        WordToRtf(@docPath);//Source参数为从数据库获取的word的二进制数据流
                 //       RichBoxWordDoc.LoadFile(Application.StartupPath + "\\temp.rtf");//richtextbox控件加载temp.rtf

                //        this.CenterTabControl.SelectedTab = DocPage;
                        Cursor = Cursors.Default;
                    }
                    catch
                    {

                        Cursor = Cursors.Default;
                    }
                }
            }

        }

     

        //导出当前专题数据
        private void MenuItemExport_Click(object sender, EventArgs e)
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();

            TreeNode tTypeNode = DataUnitTree.SelectedNode;
            string strExp = "select 专题类型 from 标准专题信息表 where 描述= '" + tTypeNode.Text + "'";
            string strtype = db.GetInfoFromMdbByExp(strCon, strExp);//专题类型
            strExp = "select 行政代码 from 数据单元表 where 行政名称='" + tTypeNode.Parent.Text + "' and 数据单元级别='3'";
            string strarea = db.GetInfoFromMdbByExp(strCon, strExp);//行政代码
            strExp = "select * from 地图入库信息表 where 专题类型='" + strtype + "' and 行政代码='" + strarea + "'";
            DataTable dt = db.GetDataTableFromMdb(strCon, strExp);

            strExp = "select 字段名称 from 图层命名规则表";
            string strname = db.GetInfoFromMdbByExp(strCon, strExp);
            string[] arrName = strname.Split('+');//分离字段名称
            List<string> namelist = new List<string>();
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                string[] layers = dt.Rows[j]["图层组成"].ToString().Split('/');
                for (int k = 0; k < layers.Length; k++)
                {
                    string layer = layers[k];
                    if (layer != "")//图层组成不为空
                    {
                        strExp = "select 业务大类代码,业务小类代码 from 标准图层信息表 where 代码='" + layer + "'";
                        DataTable dt2 = db.GetDataTableFromMdb(strCon, strExp);
                        if (dt2.Rows.Count > 0)//该图层属于标准图层表
                        {
                            string strBig = dt2.Rows[0]["业务大类代码"].ToString();
                            string strSub = dt2.Rows[0]["业务小类代码"].ToString();
                            strname = "";
                            for (int i = 0; i < arrName.Length; i++)
                            {
                                switch (arrName[i])
                                {
                                    case "业务大类代码":
                                        strname += strBig;
                                        break;
                                    case "年度":
                                        strname += dt.Rows[j]["年度"].ToString();
                                        break;
                                    case "业务小类代码":
                                        strname += strSub;
                                        break;
                                    case "行政代码":
                                        strname += dt.Rows[j]["行政代码"].ToString();
                                        break;
                                    case "比例尺":
                                        strname += dt.Rows[j]["比例尺"].ToString();
                                        break;
                                }
                            }
                            strname += layer;
                            namelist.Add(strname);
                        }
                    }
                }
            }
            if (namelist.Count == 0)
            {
                MessageBox.Show("没有图层可以导出!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //获取模板路径
            string sourcefilename = Application.StartupPath + "\\..\\Template\\DATATEMPLATE.mdb";
            SysCommon.CProgress vProgress = new SysCommon.CProgress("正在导出数据,请稍后");
            try
            {
                if (File.Exists(sourcefilename))//原模板存在
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.Filter = "MDB数据|*.mdb";
                    dlg.OverwritePrompt = false;
                    dlg.Title = "保存到MDB";
                    DialogResult result = MessageBox.Show("导出是否去掉前缀？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        
                        vProgress.EnableCancel = false;
                        vProgress.ShowDescription = false;
                        vProgress.FakeProgress = true;
                        vProgress.TopMost = true;
                        vProgress.ShowProgress();
                       
                        //如果存在mdb,替换文件，则复制模板到指定路径
                        //如果存在mdb，不替换，则追加到这个文件
                        File.Copy(sourcefilename, dlg.FileName, true);
                        IWorkspaceFactory Pwf = new AccessWorkspaceFactoryClass();
                        IWorkspace pws = (IWorkspace)(Pwf.OpenFromFile(dlg.FileName, 0));
                        IWorkspace2 pws2 = (IWorkspace2)pws;
                        for (int i = 0; i < namelist.Count; i++)
                        {
                            //从workspace中获取对应的layer
                            IWorkspace  pWorkspace=GetWorkspaceByFileName(namelist[i]) as IWorkspace;
                            string cellvalue = namelist[i];
                            if (result == DialogResult.Yes) cellvalue = cellvalue.Substring(15);//去掉前缀
                            if (pws2.get_NameExists(esriDatasetType.esriDTFeatureClass, cellvalue))
                            {
                                IFeatureClass tmpfeatureclass;
                                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pws;
                                tmpfeatureclass = pFeatureWorkspace.OpenFeatureClass(cellvalue);
                                IDataset set = tmpfeatureclass as IDataset;
                                set.CanDelete();
                                set.Delete();
                            }
                            if(pWorkspace!=null)//空间是否存在
                            IFeatureDataConverter_ConvertFeatureClass(pWorkspace, pws, namelist[i], cellvalue);
                        }
                        vProgress.Close();
                        MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                        if (log != null)
                        {
                            log.Writelog("导出MDB数据到" + dlg.FileName);
                        }
                    }
                    
                }
            }
            catch
            {
                vProgress.Close();
                MessageBox.Show("导出发生错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        /// <summary>
        /// 将一个要素类从一个工作空间转移到另外一个工作空间
        /// 注意目标工作空间不能有该要素类，必须先清除  
        /// </summary>
        /// <param name="sourceWorkspace">源工作空间</param>
        /// <param name="targetWorkspace">目标工作空间</param>
        /// <param name="nameOfSourceFeatureClass">源要素类名</param>
        /// <param name="nameOfTargetFeatureClass">目标要素类名</param>
        public void IFeatureDataConverter_ConvertFeatureClass(IWorkspace sourceWorkspace, IWorkspace targetWorkspace, string nameOfSourceFeatureClass, string nameOfTargetFeatureClass)
        {
            //create source workspace name   
            IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
            IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;
            //create source dataset name   
            IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
            IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
            sourceDatasetName.WorkspaceName = sourceWorkspaceName;
            sourceDatasetName.Name = nameOfSourceFeatureClass;
            //create target workspace name   
            IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
            IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
            //create target dataset name   
            IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
            IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
            targetDatasetName.WorkspaceName = targetWorkspaceName;
            targetDatasetName.Name = nameOfTargetFeatureClass;
            //Open input Featureclass to get field definitions.   
            ESRI.ArcGIS.esriSystem.IName sourceName = (ESRI.ArcGIS.esriSystem.IName)sourceFeatureClassName;
            IFeatureClass sourceFeatureClass = (IFeatureClass)sourceName.Open();
            //Validate the field names because you are converting between different workspace types.   
            IFieldChecker fieldChecker = new FieldCheckerClass();
            IFields targetFeatureClassFields;
            IFields sourceFeatureClassFields = sourceFeatureClass.Fields;
            IEnumFieldError enumFieldError;
            // Most importantly set the input and validate workspaces!     
            fieldChecker.InputWorkspace = sourceWorkspace;
            fieldChecker.ValidateWorkspace = targetWorkspace;
            fieldChecker.Validate(sourceFeatureClassFields, out enumFieldError, out targetFeatureClassFields);
            // Loop through the output fields to find the geomerty field   
            IField geometryField;
            for (int i = 0; i < targetFeatureClassFields.FieldCount; i++)
            {
                if (targetFeatureClassFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    geometryField = targetFeatureClassFields.get_Field(i);
                    // Get the geometry field's geometry defenition            
                    IGeometryDef geometryDef = geometryField.GeometryDef;
                    //Give the geometry definition a spatial index grid count and grid size        
                    IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
                    targetFCGeoDefEdit.GridCount_2 = 1;
                    targetFCGeoDefEdit.set_GridSize(0, 0);
                    //Allow ArcGIS to determine a valid grid size for the data loaded      
                    targetFCGeoDefEdit.SpatialReference_2 = geometryField.GeometryDef.SpatialReference;
                    // we want to convert all of the features   
                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = "";
                    // Load the feature class     
                    IFeatureDataConverter fctofc = new FeatureDataConverterClass();
                    IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName, queryFilter, null, targetFeatureClassName, geometryDef, targetFeatureClassFields, "", 1000, 0);
                    break;
                }
            }
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            if (treeViewOutPutResults.SelectedNode == null)
                return;
            if (treeViewOutPutResults.SelectedNode.Level <2)
                return;
            string filepath = treeViewOutPutResults.SelectedNode.Name;
            switch (treeViewOutPutResults.SelectedNode.Level.ToString())
            {
                case "2":
                    if (Directory.Exists(filepath))
                    {
                        System.Diagnostics.Process.Start("explorer.exe", filepath);
                        //记录日志
                        LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                        string strLog = "打开文件夹'" + treeViewOutPutResults.SelectedNode.Text + "'";
                        if (log != null)
                        {
                            log.Writelog(strLog);
                        }
                    }
                    break;
                case "3":
                    if (File.Exists(filepath)  )
                    {
                        if (filepath.Substring(filepath.Length - 3, 3).ToLower().Equals( "cel"))
                        {
                            FormFlexcell frm = new FormFlexcell();
                            frm.OpenFlexcellFile(filepath);
                            //记录日志
                            LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                            string strLog = "打开文件'" + treeViewOutPutResults.SelectedNode.Text + "'";
                            if (log != null)
                            {
                                log.Writelog(strLog);
                            }
                            frm.Show();
                        }
                        if (filepath.Substring(filepath.Length - 3, 3).ToLower().Equals( "mxd"))
                        {
                            //记录日志
                            LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                            string strLog = "打开文件'" + treeViewOutPutResults.SelectedNode.Text + "'";
                            if (log != null)
                            {
                                log.Writelog(strLog);
                            }
                        // ?   GeoPageLayout.FrmPageLayout fmPageLayout = new GeoPageLayout.FrmPageLayout(filepath);
                         //?   fmPageLayout.ShowDialog();
                        }


                    }
                    break;
                default:
                    break;

            }

        }

        private void DeleteFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeViewOutPutResults.SelectedNode == null)
                    return;
                if (treeViewOutPutResults.SelectedNode.Level < 2)
                    return;
                string filepath = treeViewOutPutResults.SelectedNode.Name;
                switch (treeViewOutPutResults.SelectedNode.Level.ToString())
                {
                    case "2":
                        if (Directory.Exists(filepath))
                        {
                            Directory.Delete(filepath, true);
                            //记录日志
                            LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                            string strLog = "删除文件夹'" + treeViewOutPutResults.SelectedNode.Text + "'";
                            if (log != null)
                            {
                                log.Writelog(strLog);
                            }
                            treeViewOutPutResults.SelectedNode.Remove();
                        }
                        break;
                    case "3":
                        if (File.Exists(filepath))
                        {
                            File.Delete(filepath);
                            //记录日志
                            LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                            string strLog = "删除文件'" + treeViewOutPutResults.SelectedNode.Text + "'";
                            if (log != null)
                            {
                                log.Writelog(strLog);
                            }
                            treeViewOutPutResults.SelectedNode.Remove();
                        }
                        break;
                    default:
                        break;

                }
            }
            catch
            { 
            }

        }

        private void LocateFile_Click(object sender, EventArgs e)
        {
            if (treeViewOutPutResults.SelectedNode == null)
                return;
            if (treeViewOutPutResults.SelectedNode.Level <2)
                return;
            string filepath = treeViewOutPutResults.SelectedNode.Name;
            switch (treeViewOutPutResults.SelectedNode.Level.ToString())
            {
                case "2":
                    if (Directory.Exists(filepath))
                    {
                        System.Diagnostics.Process.Start("explorer.exe", filepath);
                        //记录日志
                        LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                        string strLog = "定位到文件夹'" + treeViewOutPutResults.SelectedNode.Text + "'";
                        if (log != null)
                        {
                            log.Writelog(strLog);
                        }
                    }
                    break;
                case "3":
                    string dirpath = filepath.Substring(0, filepath.LastIndexOf("\\"));
                    if (File.Exists(filepath))
                    {
                        System.Diagnostics.Process.Start("explorer.exe", dirpath);
                        //记录日志
                        LogFile log = new LogFile(tipRichBox, m_strLogFilePath);
                        string strLog = "定位到文件'" + treeViewOutPutResults.SelectedNode.Text + "'";
                        if (log != null)
                        {
                            log.Writelog(strLog);
                        }
                    }
                    break;
                default:
                    break;

            }

        }
        //按照行政区调阅数据
        private void MenuItemLoadDataByUnit_Click(object sender, EventArgs e)
        {
            XzqLoad=true;
            int count = 0;
            foreach (TreeNode node in DataUnitTree.SelectedNode.Nodes)//得到专题节点
            {
               
                foreach (TreeNode childnode in node.Nodes)//得到专题节点子节点
                {
                    count++;
                        if (m_tparent==null||MapDocTree.Nodes.Count==0)
                        {
                            m_selectnode = childnode;
                            MenuItemLoadData_Click(sender, e);//调阅

                        }
                        else
                        {
                            m_selectnode = childnode;
                            MenuItemAddLoadData_Click(sender, e);//追加调阅
                        }
                }
            }
            XzqLoad = false;
            if (count > 0)
            {
                //索引树跳转到地图文档窗口
                IndextabControl.SelectedTab = PageMapDoc;

                //视图窗口跳转到图形浏览窗口
              //  CenterTabControl.SelectedTab = MapPage;
            }
            

        }
        //函数功能：选择图斑研判   输入参数：进度条  输入参数：无
        public void SelectJudge(CProgress vProgress)
        {
            //判断当前主题是否存在
            if (m_CurEditTopicNode == null)
                return;
            vProgress.SetProgress("从当前专题获取数据");
            //获取当前专题的名称
            string strSuffixx = m_CurEditTopicNode.Text;

            ILayer bhtbLayer = GetLayerByName(strSuffixx, "变化图斑");
            //获取报批图斑图层
            ILayer pcbpLayer = GetLayerByName(strSuffixx, "批次报批红线");
            //获取用途图斑图层
            ILayer tdytLayer = GetLayerByName(strSuffixx, "森林用途");
            //获取地类图斑图层
            ILayer dltbLayer = GetLayerByName(strSuffixx, "地类图斑");
            //执行选择图斑研判功能主函数
            ChangeJudge.DoJudgeBySelect(bhtbLayer as IFeatureLayer, pcbpLayer as IFeatureLayer, tdytLayer as IFeatureLayer, dltbLayer as IFeatureLayer, vProgress);
            //treeViewOutPutResults.Nodes.Clear();
            InitOutPutResultTree();
        }
        //根据专题名、图层名获取图层
        private ILayer GetLayerByName(string GroupName, string LayerName)
        {
            if (GroupName.Equals("")) return null;
            if (LayerName.Equals("")) return null;

            Exception exError = null;

            IMap pMap = axMapControl.Map;

            for (int n = 0; n < pMap.LayerCount; n++)
            {
                ILayer layer = pMap.get_Layer(n);
                if (layer is IGroupLayer && layer.Name == GroupName)//通过比对找到专题对应图层组
                {
                    ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                    for (int c = 0; c < Comlayer.Count; c++)
                    {
                        IFeatureLayer  pFeatureLayer = Comlayer.get_Layer(c) as IFeatureLayer;
                        if (pFeatureLayer == null)
                            continue;
                        if (pFeatureLayer.FeatureClass == null)
                            continue;
                        if (pFeatureLayer.Name == LayerName)    //在图层组内，通过比对找到图层
                        {
                            return pFeatureLayer as ILayer;
                        }
                    }
                }
            }
            return null;
        }
        //根据专题名获取图层组
        private ILayer GetGroupLayerByName(string GroupName)
        {
            if (GroupName.Equals("")) return null;

            Exception exError = null;

            IMap pMap = axMapControl.Map;

            for (int n = 0; n < pMap.LayerCount; n++)
            {
                ILayer layer = pMap.get_Layer(n);
                if (layer is IGroupLayer && layer.Name == GroupName)//通过比对找到专题对应图层组
                {
                    return layer;
                }
            }
            return null;
        }
        private void tabControlRight_Click(object sender, EventArgs e)
        {

        }

        private void CenterTabControl_Click(object sender, EventArgs e)
        {

        }

        private void axMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            string xStr = e.mapX.ToString("0.00");
            string yStr = e.mapY.ToString("0.00");
            TempX = double.Parse(xStr);
            TempY = double.Parse(yStr);
            ModFrameData.v_AppGisUpdate.CoorXY = "X:" + xStr.PadRight(14, ' ') + "   Y:" + yStr.PadRight(14, ' ');
          //  ModFrameData.v_QueryResult.Hide();
        }


        //
        private void MenuItemAddlayer_Click(object sender, EventArgs e)
        {
            string strWorkFile = Application.StartupPath + "\\..\\Temp\\CurPrj.xml";
            //判断文件是否存在
            XmlDocument xmldoc = new XmlDocument();
            if (!File.Exists(strWorkFile))
                return;
            xmldoc.Load(strWorkFile);
            TreeNode node = MapDocTree.SelectedNode;
            frmAddNewFile frm = new frmAddNewFile();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string strfollow = frm.strPath.Substring(frm.strPath.LastIndexOf(".") + 1).ToLower();
                //更新XML
                string strSearchRoot = "//SubGroup[@sItemName='" + node.Parent.Text + "']";
                XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);
                string strSearchnode = "//Layer[@sItemName='" + frm.strName + "']";
                XmlNode xmlCheck = xmldoc.SelectSingleNode(strSearchnode);
                if (xmlCheck != null)
                {
                    MessageBox.Show("图层名称已存在，请重命名！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                XmlElement xmlElentRoot = (XmlElement)xmlNodeRoot;
                XmlElement xmlElemt = xmldoc.CreateElement("Layer");
                xmlElemt.SetAttribute("sDemo", frm.strDescri);
                xmlElemt.SetAttribute("sItemName", frm.strName);
                if (strfollow == "mdb")
                {
                    xmlElemt.SetAttribute("sFile", frm.strPath + "\\" + frm.strFeauture);
                    xmlElemt.SetAttribute("sNewLoad", "mdb");
                }
                else
                {
                    xmlElemt.SetAttribute("sFile", frm.strPath);
                    xmlElemt.SetAttribute("sNewLoad", "shp");
                }

                xmlElentRoot.AppendChild(xmlElemt);
                xmldoc.Save(strWorkFile);

                //更新树
                TreeNode Newnode = new TreeNode();
                Newnode.Text = frm.strDescri;
                Newnode.Name = frm.strName;
                Newnode.Checked = true;
                node.Parent.Nodes.Add(Newnode);

                //更新地图
                ILayer pLayer;
                try
                {
                    if (strfollow == "shp")
                    {
                        int Index = frm.strPath.LastIndexOf("\\");
                        string filePath = frm.strPath.Substring(0, Index);
                        string fileName = frm.strPath.Substring(Index + 1);
                        //打开工作空间并添加shp文件
                        IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
                        //注意此处的路径是不能带文件名的
                        IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(filePath, 0);
                        IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                        //注意这里的文件名是不能带路径的
                        pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(fileName);
                        pFeatureLayer.Name = frm.strName;
                        pLayer = pFeatureLayer as ILayer;
                    }
                    else
                    {
                        //打开personGeodatabase,并添加图层
                        IWorkspaceFactory pAccessWorkspaceFactory = new AccessWorkspaceFactoryClass();
                        //打开工作空间并遍历数据集
                        IWorkspace pWorkspace = pAccessWorkspaceFactory.OpenFromFile(frm.strPath, 0);
                        IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                        IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                        pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(frm.strFeauture);
                        pFeatureLayer.Name = frm.strName;
                        pLayer = pFeatureLayer as ILayer;

                    }

                    IGroupLayer pGroupFLayer = GetGroupLayerByName(node.Parent.Parent.Text) as IGroupLayer;
                    IMapLayers pmaplayers = axMapControl.Map as IMapLayers;
                    ICompositeLayer Comlayer = pGroupFLayer as ICompositeLayer;
                    pmaplayers.InsertLayerInGroup(pGroupFLayer, pLayer, false, Comlayer.Count);
                    Newnode.ImageIndex = 19;
                    Newnode.SelectedImageIndex = 19;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Newnode.ImageIndex = 14;
                    Newnode.SelectedImageIndex = 14;
                }
                
            }
        }

        private void treeViewOutPutResults_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeViewOutPutResults.SelectedNode != e.Node)
            {
                treeViewOutPutResults.SelectedNode = e.Node;
            }
        }

        private void MenuItemStatSum_Click(object sender, EventArgs e)
        {
            IFeatureLayer pFeatureLayer;
            TreeNode tSelNode = MapDocTree.SelectedNode;
            pFeatureLayer = GetLayerofTreeNode(tSelNode) as IFeatureLayer;
            if (pFeatureLayer != null)
            {
                FormStatCustomize frm = new FormStatCustomize();
                frm.InitForm(this.axMapControl.Map, pFeatureLayer);
                frm.ShowDialog();
            }
        }

        private void RefreshResult_Click(object sender, EventArgs e)
        {
            this.InitOutPutResultTree();
        }

        private void MapDocTree_ControlRemoved(object sender, ControlEventArgs e)
        {

        }

        private void treeViewXZQ_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeViewXZQ.SelectedNode != e.Node)
            {
                if (treeViewXZQ.SelectedNode != null)
                {
                    treeViewXZQ.SelectedNode.ForeColor = Color.Black;
                }

                treeViewXZQ.SelectedNode = e.Node;
                e.Node.ForeColor = Color.Red;

                string strItemName = treeViewXZQ.SelectedNode.Name;
                string strItemText = treeViewXZQ.SelectedNode.Text;

           
                treeViewXZQ.Refresh();

                //根据点选节点 更新 AxMapControl 窗口
                //?     UpdateMapControl(strItemName,strItemText);

                if (treeViewXZQ.SelectedNode.Level.Equals(2))//只针对寸级节点
                {
                    treeViewXZQ.SelectedNode.ExpandAll();//展开所有子树节点
                }
            }

            //右键点击的时候弹出右键菜单
            if (e.Button == MouseButtons.Right)
            {

                System.Drawing.Point ClickPoint = treeViewXZQ.PointToScreen(new System.Drawing.Point(e.X, e.Y));
                TreeNode tSelNode;
                tSelNode = e.Node;
                if (tSelNode != null)
                {

                    if (tSelNode.Level == 2)//行政区节点
                        XZQcontextMenu.Show(ClickPoint);
                   

                }
            }
        }

        private void jmpXZQ_Click(object sender, EventArgs e)
        {
            if (treeViewXZQ.SelectedNode == null)
                return;
            IGeometry mGeometry = null;
            TreeNode county = treeViewXZQ.SelectedNode;
            TreeNode[] layers=MapDocTree.Nodes.Find("行政区",true);
            if (layers.Length == 0)
                return;
            TreeNode layer = layers[0];

            IFeatureClass mFC = (GetLayerofTreeNode(layer) as IFeatureLayer).FeatureClass;

            IFeatureCursor mFCs = mFC.Search(null, false);
            IFeature mF = mFCs.NextFeature();
            int fdXZQMC = mFC.Fields.FindField("XZQMC");
            int fdXZQDM = mFC.Fields.FindField("XZQDM");
            while (mF != null)
            {
                try
                {
                    if (mF.get_Value(fdXZQDM).ToString() == county.Name)
                    {
                        MGeometry=mF.ShapeCopy;
                        break;
                    }
                 
                }
                catch
                {

                }
                mF = mFCs.NextFeature();
            }
            ///ZQ 20111020 定位范围扩大1.5
            IEnvelope pExtent = MGeometry.Envelope;
            SysCommon.ModPublicFun.ResizeEnvelope(pExtent, 1.5);
            axMapControl.ActiveView.Extent = pExtent;
            //drawgeometryXOR(MGeometry as IPolygon, axMapControl.ActiveView.ScreenDisplay);
            axMapControl.ActiveView.Refresh();
            Application.DoEvents();
            
        }
        //在mapcontrol上画多边形
        private void drawgeometryXOR(IPolygon pPolygon, IScreenDisplay pScreenDisplay)
        {
            if (pPolygon == null) 
                return;
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

            try
            {
                //颜色对象
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                ISymbol pSymbol = (ISymbol)pFillSymbol;
                //pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                pRGBColor.Red = 255;
                pRGBColor.Green = 170;
                pRGBColor.Blue = 0;
                pLineSymbol.Color = pRGBColor;

                pLineSymbol.Width = 0.8;
                pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = pLineSymbol;

                pFillSymbol.Color = pRGBColor;
                pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;

                pScreenDisplay.StartDrawing(pScreenDisplay.hDC, -1);  //esriScreenCache.esriNoScreenCache -1
                pScreenDisplay.SetSymbol(pSymbol);

                //不存在已画出的多边形
                if (pPolygon != null)
                {
                    pScreenDisplay.DrawPolygon(pPolygon);
                    //m_Polygon = pPolygon;
                }
                //存在已画出的多边形
                //else
                //{
                //    if (m_Polygon != null)
                //    {
                //        pScreenDisplay.DrawPolygon(m_Polygon);
                //    }
                //}

                pScreenDisplay.FinishDrawing();
            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制范围出错:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pFillSymbol = null;
            }
        }

        private void drawgeometryXOR1(IPolygon pPolygon, IScreenDisplay pScreenDisplay)
        {
            if (pPolygon == null)
                return;
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

            try
            {
                //颜色对象
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                ISymbol pSymbol = (ISymbol)pFillSymbol;
                //pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                pRGBColor.Red = 255;
                pRGBColor.Green = 170;
                pRGBColor.Blue = 0;
                pLineSymbol.Color = pRGBColor;

                pLineSymbol.Width = 0.8;
                pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = pLineSymbol;

                pFillSymbol.Color = pRGBColor;
                pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;

                //pScreenDisplay.StartDrawing(pScreenDisplay.hDC, -1);  //esriScreenCache.esriNoScreenCache -1
                pScreenDisplay.SetSymbol(pSymbol);

                //不存在已画出的多边形
                if (pPolygon != null)
                {
                    pScreenDisplay.DrawPolygon(pPolygon);
                    //m_Polygon = pPolygon;
                }
                //存在已画出的多边形
                //else
                //{
                //    if (m_Polygon != null)
                //    {
                //        pScreenDisplay.DrawPolygon(m_Polygon);
                //    }
                //}

                //pScreenDisplay.FinishDrawing();
            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制范围出错:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pFillSymbol = null;
            }
        }

        private void treeViewJHTable_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeViewJHTable.SelectedNode != e.Node)
            {
                if (treeViewJHTable.SelectedNode != null)
                {
                    treeViewJHTable.SelectedNode.ForeColor = Color.Black;
                }

                treeViewJHTable.SelectedNode = e.Node;
                e.Node.ForeColor = Color.Red;

                string strItemName = treeViewJHTable.SelectedNode.Name;
                string strItemText = treeViewJHTable.SelectedNode.Text;


                treeViewJHTable.Refresh();

                //根据点选节点 更新 AxMapControl 窗口
                //?     UpdateMapControl(strItemName,strItemText);

                //if (treeViewJHTable.SelectedNode.Level.Equals(1))//只针对寸级节点
                //{
                //    treeViewJHTable.SelectedNode.ExpandAll();//展开所有子树节点
                //}
            }

            //右键点击的时候弹出右键菜单
            if (e.Button == MouseButtons.Right)
            {

                System.Drawing.Point ClickPoint = treeViewJHTable.PointToScreen(new System.Drawing.Point(e.X, e.Y));
                TreeNode tSelNode;
                tSelNode = e.Node;
                if (tSelNode != null)
                {

                    if (tSelNode.Level == 1)//行政区节点
                        JHTBcontextMenu.Show(ClickPoint);


                }
            }
           
        }

        private void JHTBjmpStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeViewJHTable.SelectedNode == null)
                return;
            IGeometry mGeometry = null;
            TreeNode tfh = treeViewJHTable.SelectedNode;
            TreeNode[] layers = MapDocTree.Nodes.Find("接合图表", true);
            if (layers.Length == 0)
                return;
            TreeNode layer = layers[0];

            IFeatureClass mFC = (GetLayerofTreeNode(layer) as IFeatureLayer).FeatureClass;

            IFeatureCursor mFCs = mFC.Search(null, false);
            IFeature mF = mFCs.NextFeature();
            int NEWMAPNO = mFC.Fields.FindField("NEWMAPNO");
            //int fdXZQDM = mFC.Fields.FindField("XZQDM");
            while (mF != null)
            {
                try
                {
                    if (mF.get_Value(NEWMAPNO).ToString() == tfh.Name)
                    {
                        MGeometry = mF.ShapeCopy;
                        break;
                    }

                }
                catch
                {

                }
                mF = mFCs.NextFeature();
            }
            ///ZQ 20111020 定位范围扩大1.5倍
            IEnvelope pExtent = MGeometry.Envelope;
            SysCommon.ModPublicFun.ResizeEnvelope(pExtent, 1.5);
            axMapControl.ActiveView.Extent = pExtent;
            //drawgeometryXOR(MGeometry as IPolygon, axMapControl.ActiveView.ScreenDisplay);
            axMapControl.ActiveView.Refresh();
            Application.DoEvents();
            

        }
        private void SetControl_mapctrl_afterdraw(IDisplay Display, esriViewDrawPhase phase)
        {

            if (phase == esriViewDrawPhase.esriViewForeground) drawgeometryXOR1(MGeometry as IPolygon, axMapControl.ActiveView.ScreenDisplay);
        }

        private void cancelXZQMenuItem_Click(object sender, EventArgs e)
        {
            MGeometry = null;
            axMapControl.ActiveView.Refresh();
        }

        private void cancelJHTBMenuItem_Click(object sender, EventArgs e)
        {
            MGeometry = null;
            axMapControl.ActiveView.Refresh();
        }

        private void axMapControl_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {
            //drawgeometryXOR(MGeometry as IPolygon, axMapControl.ActiveView.ScreenDisplay);

        }


    }       
    
}