using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Collections;
using DevComponents.DotNetBar;
using GeoUtilities;
using System.Data.OracleClient;


using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using System.IO;
using GeoPageLayout;
namespace GeoSysUpdate
{
    public partial class UserControlSMPD : UserControl
    {
        //右键菜单集合
        private Dictionary<string, DevComponents.DotNetBar.ContextMenuBar> _dicContextMenu;

        //临时保存鼠标在图面上的坐标
        private double TempX;
        private double TempY;
        System.Drawing.Point pPoint = new System.Drawing.Point();

        private int m_iFileIDItem = 0;//文件标识符ID
        private int m_iRootItem = 0;//根节点ID
        //元数据库连接信息        // ZQ
        private String m_MDconnt = Application.StartupPath + "\\.. \\MDxmlData\\MDConn.dat";

        //地图浏览工具栏容器
        private System.Windows.Forms.Control _MapToolControl;
        private RichTextBox _Richtextbox;
        private DevComponents.DotNetBar.Bar _MapToolBar;
        public DevComponents.DotNetBar.Bar MapToolBar
        {
            get { return _MapToolBar; }
        }

        //控制解决GroupLayer显示问题(方法有待改进 )
        private bool m_bGroupLayerVisible;
        //图层树全路径（从数据库中拷贝下来）
        private String _layerTreePath = Application.StartupPath + "\\..\\res\\xml\\展示图层树.xml";
        private String m_MetadataItemPath = Application.StartupPath + "\\..\\MDxmlData\\Res\\MetadataItem.xml";
        private String m_ResTypePath = Application.StartupPath + "\\..\\MDxmlData\\Res\\restype.xml";
        private String m_MetadataItemPathtest = Application.StartupPath + "\\..\\MDxmlData\\M210515_9_ea382cfb-661a-4f07-84fd-db78f847caac.xml";
        //接图表图层 NodeKey信息全路径   ZQ 20110805  add
        private String m_layerNodeKey = Application.StartupPath + "\\..\\res\\xml\\接图表图层信息.xml";
        //xml文件路径
        private string _RoadandRiverXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\Road.xml";
        //end
        //xzq要素名，字段名yjl0716
        //private string xzqFeaClassName = "";
        //private string xzqCodeField = "";
        //xzq要素名，字段名yjl0716  ZQ  20110731 modify  
        private string strXZQFeaClassNameQG = "";
        private string strXZQCodeFieldQG = "";
        private string strXZQFeaClassNameGS = "";
        private string strXZQCodeFieldGS = "";
        //ZQ   20110803  add   行政区县要素名、字段名
        private string strXZQFeaClassNameGX = "";
        private string strXZQFieldGX = "";
        //ZQ  20110817  add 存储用户查询信息
        public List<string> strMeataDataSQL =new List<string>();
        //end

        private string _Decimalstr = "";

        //初始化窗体类
        public UserControlSMPD(string strName, string strCation)
        {
            InitializeComponent();
            //初始化配置对应视图控件
            InitialMainViewControl();

            this.Name = strName;
            this.Tag = strCation;
            this.Dock = System.Windows.Forms.DockStyle.Fill;

            axMapControl.Map.Name = "数据图层";
            axTOCControl.SetBuddyControl(axMapControl.Object);
            advTree.ImageList = IconContainer;
            advTreeLog.ImageList = IconContainer;
            //advTreeProject.ImageList = IconContainer;

            ModData.v_AppGisUpdate.MainUserControl = this;
            ModData.v_AppGisUpdate.TOCControl = axTOCControl.Object as ITOCControlDefault;
            ModData.v_AppGisUpdate.MapControl = axMapControl.Object as IMapControlDefault;
            ModData.v_AppGisUpdate.p1 = p1;
            ModData.v_AppGisUpdate.p2 = p2;
            ModData.v_AppGisUpdate.p3 = p3;
            ModData.v_AppGisUpdate.p4 = p4;
            ModData.v_AppGisUpdate.ArcGisMapControl = axMapControl;
            ModData.v_AppGisUpdate.CurrentControl = axMapControl.Object;
            ModData.v_AppGisUpdate.SceneControl = this.SceneControl.axSceneCtl;
            ModData.v_AppGisUpdate.SceneControlDefault = this.SceneControl.axSceneCtl.Object as ISceneControlDefault;
            ModData.v_AppGisUpdate.DataTree = advTree;
            ModData.v_AppGisUpdate.ErrTree = advTreeLog;
            ModData.v_AppGisUpdate.ResultFileTree = advTreeResultFileList;//added by chulili 20110923 修改了底层 成果列表树图

            ModData.v_AppGisUpdate.UpdateDataGrid = dgvUpdate;
            ModData.v_AppGisUpdate.DataCheckGrid = dgvDataCheck;
            ModData.v_AppGisUpdate.Visible = this.Visible;
            ModData.v_AppGisUpdate.Enabled = this.Enabled;
            ModData.v_AppGisUpdate.CurrentThread = null;
            ModData.v_AppGisUpdate.tipRichBox = _Richtextbox;
            ModData.v_AppGisUpdate.XZQTree = treeViewXZQ;//yjl20110926 add 行政区树
            ModData.v_AppGisUpdate.MetadataTree = treeViewJHTable;//yjl20110926 add 元数据树
            //初始化框架插件控件界面
            InitialFrmDefControl();

            //初始化树图
            InitDataView();

            
        }

        private void InitDataView()
        {
            //初始化树图
            this.DataViewTree.CurMap = ModData.v_AppGisUpdate.MapControl as IMapControl2;
            this.DataViewTree.CurWks = ModData.v_AppGisUpdate.CurWksInfo.Wks;
            //从数据库中拷贝图层树
            GeoLayerTreeLib.LayerManager.ModuleMap.CopyLayerTreeXmlFromDataBase(ModData.v_AppGisUpdate.CurWksInfo.Wks, _layerTreePath);
            this.DataViewTree.LayerXmlPath = _layerTreePath;
            if (_dicContextMenu.ContainsKey("TOCControlContextMenu"))
            {
                DevComponents.DotNetBar.ContextMenuBar menuBar = _dicContextMenu["TOCControlContextMenu"];
                this.DataViewTree.MapContextMenu = menuBar;     //根节点右键菜单
            }
            if (_dicContextMenu.ContainsKey("TOCControlLayerContextMenu"))
            {
                DevComponents.DotNetBar.ContextMenuBar menuBarLayer = _dicContextMenu["TOCControlLayerContextMenu"];
                this.DataViewTree.LayerContextMenu = menuBarLayer;  //图层节点右键菜单

            }
            
            
            this.DataViewTree.TocControl = ModData.v_AppGisUpdate.TOCControl;

            //ZQ  20110827   add
            this.DataViewTree.isDragDrop = false;
            //end
            //初始化行政区tab页
            treeViewXZQ.Nodes.Clear();
            //treeViewXZQ.Nodes.Add("XZQ", "行政辖区", 18, 19);
            //InitFromXZQLayer(axMapControl.Map);
            string xmlpath = Application.StartupPath + "\\..\\Res\\Xml\\XZQ.xml";
            if (!File.Exists(xmlpath))
            {
                return;
            }
            try
            {
                InitFromXZQXML(xmlpath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //初始化高速节点
            InitRoadTree();
            //初始化铁路节点
            InitRailRoadTree();
            //初始化河流节点
            InitRiverTree();
            string ResultFilePath = Application.StartupPath + "\\..\\OutputResults";
            InitResultFileList(ResultFilePath);

        }
        private void InitResultFileList(string DirPath)
        {
            advTreeResultFileList.Nodes.Clear();
            DevComponents.AdvTree.Node tparent;
            tparent = new DevComponents.AdvTree.Node();
            tparent.Text = "成果列表";
            tparent.Tag = 0;
            tparent.Name = DirPath;
            tparent.ImageIndex = 13;
            //tparent.SelectedImageIndex = 13;
            advTreeResultFileList.Nodes.Add(tparent);
            advTreeResultFileList.ExpandAll();
            AddLeafItemFromFile(tparent,DirPath);
        }
        public void AddLeafItemFromFile(DevComponents.AdvTree.Node pNode, string DirPath)
        {
            if (Directory.Exists(DirPath))
            {
                DevComponents.AdvTree.Node tChildNode, tChildChildNode;
                // string strTblName = "";
                DirectoryInfo Dinfo = new DirectoryInfo(DirPath);
                foreach (DirectoryInfo eachinfo in Dinfo.GetDirectories())
                {
                    tChildNode = new DevComponents.AdvTree.Node();
                    tChildNode.Text = eachinfo.Name;
                    tChildNode.Name = eachinfo.FullName;
                    tChildNode.ImageIndex = 11;
                    //tChildNode.SelectedImageIndex = 12;
                    pNode.Nodes.Add(tChildNode);
                    AddLeafItemFromFile(tChildNode, eachinfo.FullName);
                    foreach (FileInfo Finfo in eachinfo.GetFiles("*.xls"))
                    {
                        tChildChildNode = new DevComponents.AdvTree.Node();
                        tChildChildNode.Text = Finfo.Name.Substring(0, Finfo.Name.IndexOf("."));
                        tChildChildNode.Name = Finfo.FullName;
                        tChildChildNode.ImageIndex = 15;
                        //tChildChildNode.SelectedImageIndex = 15;
                        tChildNode.Nodes.Add(tChildChildNode);
                    }
                    foreach (FileInfo Finfo in eachinfo.GetFiles("*.mdb"))
                    {
                        tChildChildNode = new DevComponents.AdvTree.Node();
                        tChildChildNode.Text = Finfo.Name.Substring(0, Finfo.Name.IndexOf("."));
                        tChildChildNode.Name = Finfo.FullName;
                        tChildChildNode.ImageIndex = 18;
                        //tChildChildNode.SelectedImageIndex = 18;
                        tChildNode.Nodes.Add(tChildChildNode);
                    }
                    foreach (FileInfo Finfo in eachinfo.GetFiles("*.mxd"))
                    {
                        tChildChildNode = new DevComponents.AdvTree.Node();
                        tChildChildNode.Text = Finfo.Name.Substring(0, Finfo.Name.IndexOf("."));
                        tChildChildNode.Name = Finfo.FullName;
                        tChildChildNode.ImageIndex = 17;
                        //tChildChildNode.SelectedImageIndex = 17;
                        tChildNode.Nodes.Add(tChildChildNode);
                    }
                }
            }

        }
        public void LoadData()
        {
            //褚丽丽添加 初始化数据源字典（由于这个步骤有时耗时较长，所以单独提出来执行，只需初始化一次即可）
            this.DataViewTree.InitDBsource();
            this.DataViewTree.LoadData();
        }
        private void InitRiverTree()
        {
            if (!File.Exists(_RoadandRiverXmlPath))
            {
                return;
            }
            //读取xml文件
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(_RoadandRiverXmlPath);
            //读取
            string strSearch = "//RiverRoot";
            XmlNode pXmlNode = pXmlDoc.SelectSingleNode(strSearch);
            if (pXmlNode == null)
                return;
            //道路根节点
            DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
            pNode.Name = "RiverRoot";
            pNode.ImageIndex = 35;
            //pNode.SelectedImageIndex = 35;
            pNode.Text = pXmlNode.Attributes ["ItemName"].Value;
            pNode.Tag = pXmlNode.OuterXml as object ;
            treeViewXZQ.Nodes.Add(pNode);
            //遍历子节点
            for (int i = 0; i < pXmlNode.ChildNodes.Count; i++)
            {
                XmlNode pTmpXmlNode = pXmlNode.ChildNodes[i];
                if (pTmpXmlNode != null)
                {
                    DevComponents.AdvTree.Node pTmpNode = new DevComponents.AdvTree.Node();
                    pTmpNode.Name = pTmpXmlNode.Attributes ["RiverCode"].Value;
                    pTmpNode.Text = pTmpXmlNode.Attributes ["ItemName"].Value;
                    pTmpNode.ImageIndex = 34;
                    //pTmpNode.SelectedImageIndex = 34;
                    pNode.Nodes.Add(pTmpNode);
                }
            }

        }
        private void InitRailRoadTree()
        {
            if (!File.Exists(_RoadandRiverXmlPath))
            {
                return;
            }
            //读取xml文件
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(_RoadandRiverXmlPath);
            //读取
            string strSearch = "//RailRoadRoot";
            XmlNode pXmlNode = pXmlDoc.SelectSingleNode(strSearch);
            if (pXmlNode == null)
                return;
            //道路根节点
            DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
            pNode.Name = "RailRoadRoot";
            pNode.ImageIndex = 35;
            //pNode.SelectedImageIndex = 35;
            pNode.Text = pXmlNode.Attributes ["ItemName"].Value;
            pNode.Tag = pXmlNode.OuterXml as object ;
            treeViewXZQ.Nodes.Add(pNode);
            //遍历子节点
            for (int i = 0; i < pXmlNode.ChildNodes.Count; i++)
            {
                XmlNode pTmpXmlNode = pXmlNode.ChildNodes[i];
                if (pTmpXmlNode != null)
                {
                    DevComponents.AdvTree.Node pTmpNode = new DevComponents.AdvTree.Node();
                    pTmpNode.Name = pTmpXmlNode.Attributes ["RailRoadCode"].Value;
                    pTmpNode.Text = pTmpXmlNode.Attributes ["ItemName"].Value;
                    pTmpNode.ImageIndex = 34;
                    //pTmpNode.SelectedImageIndex = 34;
                    pNode.Nodes.Add(pTmpNode);
                }
            }

        }
        private void InitRoadTree()
        {
            if (!File.Exists(_RoadandRiverXmlPath))
            {
                return;
            }
            //读取xml文件
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(_RoadandRiverXmlPath);
            //读取
            string strSearch = "//RoadRoot";
            XmlNode pXmlNode = pXmlDoc.SelectSingleNode(strSearch );
            if (pXmlNode == null)
                return;
            //道路根节点
            DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
            pNode.Name = "RoadRoot";
            pNode.ImageIndex = 35;
            //pNode.SelectedImageIndex = 35;
            pNode.Text = pXmlNode.Attributes ["ItemName"].Value;
            pNode.Tag = pXmlNode.OuterXml as object ;
            treeViewXZQ.Nodes.Add(pNode );
            //遍历子节点
            for (int i = 0; i < pXmlNode.ChildNodes.Count; i++)
            {
                XmlNode pTmpXmlNode=pXmlNode.ChildNodes[i];
                if (pTmpXmlNode != null)
                {
                    DevComponents.AdvTree.Node pTmpNode = new DevComponents.AdvTree.Node();                   
                    pTmpNode.Name = pTmpXmlNode.Attributes ["RoadCode"].Value;
                    pTmpNode.ImageIndex = 34;
                    //pTmpNode.SelectedImageIndex = 34;
                    pTmpNode.Text = pTmpXmlNode.Attributes ["ItemName"].Value;
                    pNode.Nodes.Add(pTmpNode);
                }
            }

        }
        private void RefreshDataView()
        {
            //从数据库中拷贝图层树
            GeoLayerTreeLib.LayerManager.ModuleMap.CopyLayerTreeXmlFromDataBase(ModData.v_AppGisUpdate.CurWksInfo.Wks, _layerTreePath);

            this.DataViewTree.LoadData();
        }

        /// <summary>
        /// 初始化配置对应视图控件
        /// </summary>
        private void InitialMainViewControl()
        {
            frmBarManager newfrmBarManager = new frmBarManager();
            newfrmBarManager.TopLevel = false;
            newfrmBarManager.Dock = DockStyle.Fill;
            newfrmBarManager.Show();
            Plugin.LogTable._richbox = newfrmBarManager.Richtextbox;
            _Richtextbox = newfrmBarManager.Richtextbox;
            this.Controls.Add(newfrmBarManager);

            newfrmBarManager.PanelLyr.Controls.Add(this.tabControl);//直接加到界面上去
            this.tabControl.Dock = DockStyle.Fill;

            //加载设置数据视图
            DevComponents.DotNetBar.Bar barMap = newfrmBarManager.CreateBar("barMap", enumLayType.FILL);
            
            barMap.CanHide = false;

            this.tabControlInfo.Dock = DockStyle.Fill;

            barMap.Controls.Add(this.tabControlInfo);
            _MapToolControl = this.tabControlInfo as System.Windows.Forms.Control;//将视图工具放到tab里面去

            //加载设置提示窗体
            //数据处理显示
            PanelDockContainer PanelTipData = new PanelDockContainer();
            PanelTipData.Controls.Add(this.advTree);
            this.advTree.Dock = DockStyle.Fill;

            
            //DockContainerItem dockItemData = new DockContainerItem("dockItemData", "处理");
            //dockItemData.Control = PanelTipData;
            //newfrmBarManager.ButtomBar.Items.Add(dockItemData);
            //错误提示显示
         /*   PanelDockContainer PanelTipErr = new PanelDockContainer();
            PanelTipErr.Controls.Add(this.advTreeLog);
            this.advTreeLog.Dock = DockStyle.Fill;
            DockContainerItem dockItemErr = new DockContainerItem("dockItemErr", "错误");
            dockItemErr.Control = PanelTipErr;
            newfrmBarManager.ButtomBar.Items.Add(dockItemErr);
            //更新对比分析显示
            PanelDockContainer PanelTipUpdate = new PanelDockContainer();
            PanelTipUpdate.Controls.Add(this.dgvUpdate);
            this.dgvUpdate.Dock = DockStyle.Fill;
            DockContainerItem dockItemUpdate = new DockContainerItem("dockItemUpdate", "更新对比");
            dockItemUpdate.Control = PanelTipUpdate;
            newfrmBarManager.ButtomBar.Items.Add(dockItemUpdate);

            //数据检查结果
            PanelDockContainer PanelTipDataCheck = new PanelDockContainer();
            PanelTipDataCheck.Controls.Add(this.dgvDataCheck);
            this.dgvDataCheck.Dock = DockStyle.Fill;
            PanelTipDataCheck.Controls.Add(this.btnReport);
            this.btnReport.Dock = DockStyle.Bottom;
            DockContainerItem dockItemDataCheck = new DockContainerItem("dockItemDataCheck", "检查结果");
            dockItemDataCheck.Control = PanelTipDataCheck;
            newfrmBarManager.ButtomBar.Items.Add(dockItemDataCheck);*/
        }

        //初始化框架插件控件界面
        private void InitialFrmDefControl()
      {
            GetScaleDecimal();//added by chulili 20110816 获取比例尺配置（小数位数）

            //得到Xml的System节点,根据XML加载插件界面
            string xPath = ".//System[@Name='" + this.Name + "']";
            ModData.v_AppGisUpdate.ScaleBoxList = new List<ComboBoxItem>();
            Plugin.ModuleCommon.LoadButtonViewByXmlNode(ModData.v_AppGisUpdate.ControlContainer, xPath, ModData.v_AppGisUpdate);

            _dicContextMenu = ModData.v_AppGisUpdate.DicContextMenu;

            //初始化地图浏览工具栏
            Plugin.Application.IAppFormRef pAppFrm = ModData.v_AppGisUpdate as Plugin.Application.IAppFormRef;
            XmlNode barXmlNode = pAppFrm.SystemXml.SelectSingleNode(".//ToolBar[@Name='ControlMapToolBar']");
            if (barXmlNode == null || _MapToolControl == null) return;
            DevComponents.DotNetBar.Bar mapToolBar = Plugin.ModuleCommon.LoadButtonView(_MapToolControl, barXmlNode, pAppFrm, null) as Bar;
            if (mapToolBar != null)
            {
                mapToolBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
                mapToolBar.DockOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
                mapToolBar.DockSide = DevComponents.DotNetBar.eDockSide.Top;
                mapToolBar.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
                mapToolBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
                mapToolBar.RoundCorners = false;
                mapToolBar.SendToBack();
                

              /* _MapToolBar = mapToolBar;
                mapToolBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
                mapToolBar.DockOrientation = DevComponents.DotNetBar.eOrientation.Horizontal;//.Vertical;
                mapToolBar.DockSide = DevComponents.DotNetBar.eDockSide.Top;
                mapToolBar.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
                mapToolBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
                _MapToolControl.Controls.Remove(mapToolBar);
                _MapToolControl.Controls.Add(mapToolBar);*/
            }
        }

        //错误树图右键菜单处理方法
        private void advTreeLog_NodeMouseUp(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            DevComponents.AdvTree.AdvTree aTree = sender as DevComponents.AdvTree.AdvTree;
            if (aTree.SelectedNode == null) return;
            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("ContextMenuErrTree"))
            {
                if (_dicContextMenu["ContextMenuErrTree"].Items.Count > 0)
                {
                    item = _dicContextMenu["ContextMenuErrTree"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        aTree.SelectedNode.ContextMenu = item;
                    }
                }
            }
        }

        /// <summary>
        /// 数据库工程树图上右击节点时弹出菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeProject_NodeMouseUp(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            DevComponents.AdvTree.AdvTree aTree = sender as DevComponents.AdvTree.AdvTree;
            DevComponents.DotNetBar.ButtonItem item = null;

            if (aTree.SelectedNode == null) return;

            if (aTree.SelectedNode.Name == "处理范围根节点")
            {
                if (_dicContextMenu.ContainsKey("ContextMenuProTree"))
                {
                    if (_dicContextMenu["ContextMenuProTree"].Items.Count > 0)
                    {
                        item = _dicContextMenu["ContextMenuProTree"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                        if (item != null)
                        {
                            aTree.SelectedNode.ContextMenu = item;
                        }
                    }
                }
            }
        }

        //更新对比分析列表右键菜单
        private void dgvUpdate_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //列表的右键菜单
            if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            DevComponents.DotNetBar.Controls.DataGridViewX dtView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            dtView = sender as DevComponents.DotNetBar.Controls.DataGridViewX;


            if (dtView.SelectedRows.Count == 0) return;
            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("ContextMenuUpdateDataGrid"))
            {
                if (_dicContextMenu["ContextMenuUpdateDataGrid"].Items.Count > 0)
                {
                    item = _dicContextMenu["ContextMenuUpdateDataGrid"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        item.Popup(pPoint);
                    }
                }
            }

        }
        private void dgvUpdate_KeyDown(object sender, KeyEventArgs e)
        {
            DevComponents.DotNetBar.Controls.DataGridViewX dtView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            dtView = sender as DevComponents.DotNetBar.Controls.DataGridViewX;
            if (dtView.SelectedRows.Count == 0) return;
            if (e.KeyData == Keys.Space)
            {
                ZoomTo(dtView);//定位功能
            }
        }

        private void dgvUpdate_MouseMove(object sender, MouseEventArgs e)
        {
            pPoint = System.Windows.Forms.Cursor.Position;
        }

        private void dgvUpdate_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DevComponents.DotNetBar.Controls.DataGridViewX dtView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            dtView = sender as DevComponents.DotNetBar.Controls.DataGridViewX;
            ZoomTo(dtView);//定位功能
        }


        /// <summary>
        /// dtView选中一行根据条件执行自动定位操作，若选中多行则不执行任何操作
        /// </summary>
        /// <param name="dtView"></param>
        private void ZoomTo(DevComponents.DotNetBar.Controls.DataGridViewX dtView)
        {
            ILayer m_CurLayer = null; ;
            long oid = -1;
            if (dtView.SelectedRows.Count == 1)
            {
                string layerName = dtView.SelectedRows[0].Cells[0].FormattedValue.ToString();//图层名
                oid = long.Parse(dtView.SelectedRows[0].Cells[1].FormattedValue.ToString());//OID
                string feaid = dtView.SelectedRows[0].Cells[2].FormattedValue.ToString();//FEAID
                string operationType = dtView.SelectedRows[0].Cells[3].FormattedValue.ToString();//状态：新建1、修改3、删除2
                Dictionary<string, ILayer> DicLayer = new Dictionary<string, ILayer>();
                for (int i = 0; i < axMapControl.Map.LayerCount; i++)
                {
                    ILayer pLayer = axMapControl.Map.get_Layer(i);

                    if (pLayer is IGroupLayer)
                    {
                        IGroupLayer tLayer = pLayer as IGroupLayer;
                        if (tLayer.Name == "示意图")
                        {
                            continue;
                        }
                        else
                        {
                            DicLayer.Add(tLayer.Name, tLayer);
                        }
                    }
                }
                if (DicLayer == null) return;
                if (!DicLayer.ContainsKey("本地备份库体数据") && !DicLayer.ContainsKey("更新库体数据") && !DicLayer.ContainsKey("工作库数据")) return;
                ICompositeLayer comLayer = null;
                ILayer tt = null;
                if (DicLayer.ContainsKey("更新库体数据") && (operationType == "新建" || operationType == "修改"))
                {
                    //新建或修改的记录
                    comLayer = DicLayer["更新库体数据"] as ICompositeLayer;
                    for (int j = 0; j < comLayer.Count; j++)
                    {
                        tt = comLayer.get_Layer(j);
                        if (tt.Name == layerName)
                        {
                            m_CurLayer = tt;
                            break;
                        }
                    }
                }
                else if (DicLayer.ContainsKey("本地备份库体数据") && (operationType == "删除" || operationType == "修改"))
                {
                    //修改或删除的记录
                    comLayer = DicLayer["本地备份库体数据"] as ICompositeLayer;
                    for (int j = 0; j < comLayer.Count; j++)
                    {
                        tt = comLayer.get_Layer(j);
                        if (tt.Name == layerName)
                        {
                            m_CurLayer = tt;
                            break;
                        }
                    }
                }
                else if (DicLayer.ContainsKey("工作库数据") && (operationType == "新建" || operationType == "修改"))
                {
                    //修改或删除的记录
                    comLayer = DicLayer["工作库数据"] as ICompositeLayer;
                    for (int j = 0; j < comLayer.Count; j++)
                    {
                        tt = comLayer.get_Layer(j);
                        if (tt.Name == layerName)
                        {
                            m_CurLayer = tt;
                            break;
                        }
                    }
                }
            }
            if (m_CurLayer != null && oid != -1)
            {
                //若选中的图层不为空，则根据oid进行定位
                try
                {
                    IFeatureLayer pFeatLay = m_CurLayer as IFeatureLayer;
                    IFeatureClass pFeatCls = pFeatLay.FeatureClass;
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "OBJECTID=" + oid;
                    IFeatureCursor pFeatCursor = pFeatCls.Search(pQueryFilter, false);
                    IFeature pFeature = pFeatCursor.NextFeature();
                    if (pFeature == null) return;
                    axMapControl.Map.ClearSelection();
                    axMapControl.Map.SelectFeature(m_CurLayer, pFeature);
                    IMapControlDefault mapControl = axMapControl.Object as IMapControlDefault;
                    SysCommon.Gis.ModGisPub.ZoomToFeature(mapControl, pFeature);
                }
                catch
                { }
            }
        }

        //图层控制右键菜单
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

            //if (e.button == 1)
            //{
            //    //控制解决GroupLayer显示问题  
            //    if (TOCItem == esriTOCControlItem.esriTOCControlItemLayer)
            //    {
            //        if (pLayer is IGroupLayer)
            //        {
            //            if (pLayer.Visible)
            //            {
            //                m_bGroupLayerVisible = false;
            //            }
            //            else
            //            {
            //                m_bGroupLayerVisible = true;
            //            }
            //        }
            //    }
            //    return;
            //}

            //if (e.button == 2 && _dicContextMenu != null)
            //{
            //    DevComponents.DotNetBar.ButtonItem item = null;
            //    DevComponents.DotNetBar.ContextMenuBar menuBar = _dicContextMenu["TOCControlContextMenu"];
            //    DevComponents.DotNetBar.ContextMenuBar menuBarLayer = _dicContextMenu["TOCControlLayerContextMenu"];
            //    this.Controls.Add(menuBar);
            //    this.Controls.Add(menuBarLayer);
            //    switch (TOCItem)
            //    {
                    //        case esriTOCControlItem.esriTOCControlItemMap:
                    //            if (_dicContextMenu.ContainsKey("TOCControlContextMenu"))
                    //            {
                    //                if (_dicContextMenu["TOCControlContextMenu"] != null)
                    //                {
                    //                    if (_dicContextMenu["TOCControlContextMenu"].Items.Count > 0)
                    //                    {
                    //                        item = _dicContextMenu["TOCControlContextMenu"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    //                        if (item != null)
                    //                        {
                    //                            item.Popup(axTOCControl.PointToScreen(pPoint));
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //            break;
                    //case esriTOCControlItem.esriTOCControlItemLayer:
                    //    {
                    //        if (!(pLayer is IGroupLayer || pLayer is IFeatureLayer || pLayer is IDataLayer)) return;
                    //        if (pLayer is IFeatureLayer || pLayer is IDataLayer)
                    //        {
                    //            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                    //            if (_dicContextMenu.ContainsKey("TOCControlLayerContextMenu2"))
                    //            {
                    //                if (_dicContextMenu["TOCControlLayerContextMenu2"] != null)
                    //                {
                    //                    if (_dicContextMenu["TOCControlLayerContextMenu2"].Items.Count > 0)
                    //                    {
                    //                        item = _dicContextMenu["TOCControlLayerContextMenu2"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    //                        if (item != null)
                    //                        {
                    //                            item.Popup(axTOCControl.PointToScreen(pPoint));
                    //                        }
                    //                    }
                    //                }
                    //            }

                    //            if (axTOCControl.Buddy is IPageLayoutControl2)
                    //            {
                    //                IPageLayoutControl2 pPageLayoutControl = axTOCControl.Buddy as IPageLayoutControl2;
                    //                pPageLayoutControl.CustomProperty = pLayer;

                    //            }
                    //            else if (axTOCControl.Buddy is IMapControl3)
                    //            {
                    //                IMapControl3 pMapcontrol = axTOCControl.Buddy as IMapControl3;
                    //                pMapcontrol.CustomProperty = pLayer;
                    //            }
                    //        }
                            //                else
                            //                {
                            //                    IGroupLayer pGroupLayer = pLayer as IGroupLayer;
                            //                    if (_dicContextMenu.ContainsKey("TOCControlGroupContextMenu"))
                            //                    {
                            //                        if (_dicContextMenu["TOCControlGroupContextMenu"] != null)
                            //                        {
                            //                            if (_dicContextMenu["TOCControlGroupContextMenu"].Items.Count > 0)
                            //                            {
                            //                                item = _dicContextMenu["TOCControlGroupContextMenu"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                            //                                if (item != null)
                            //                                {
                            //                                    item.Popup(axTOCControl.PointToScreen(pPoint));
                            //                                }
                            //                            }
                            //                        }
                            //                    }
                            //                    if (axTOCControl.Buddy is IPageLayoutControl2)
                            //                    {
                            //                        IPageLayoutControl2 pPageLayoutControl = axTOCControl.Buddy as IPageLayoutControl2;
                            //                        pPageLayoutControl.CustomProperty = pGroupLayer;

                            //                    }
                            //                    else if (axTOCControl.Buddy is IMapControl3)
                            //                    {
                            //                        IMapControl3 pMapcontrol = axTOCControl.Buddy as IMapControl3;
                            //                        pMapcontrol.CustomProperty = pGroupLayer;
                            //                    }
                            //                }

                            //            }

            //                break;

            //            }
            //    }
            //}
        }

        //图层浏览右键菜单
        private void axMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 4)
            {
                this.axMapControl.Pan();
            }

            timerShow.Enabled = false;  //进行操作时，操作历史查询不可用

            if (e.button == 1 || _dicContextMenu == null || e.button == 4 || axMapControl.CurrentTool.ToString().Equals("GeoUtilities.ControlsMapMeasureToolDefClass"))
                return;
            //设置当前点坐标
            ModData.v_CurPoint = new PointClass();
            ModData.v_CurPoint.PutCoords(e.mapX, e.mapY);
            System.Drawing.Point pPoint = new System.Drawing.Point(e.x, e.y);
            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("MapControlContextMenu"))
            {
                if (_dicContextMenu["MapControlContextMenu"].Items.Count > 0)
                {
                    DevComponents.DotNetBar.ContextMenuBar menuBar = _dicContextMenu["MapControlContextMenu"];
                    this.Controls.Add(menuBar);
                    item = menuBar.Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        item.Popup(axMapControl.PointToScreen(pPoint));
                    }
                }
            }
        }

        /// <summary>
        /// 显示鼠标所落在的mapsheet的更新状态列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMapControl_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            if (ModData.v_AppGisUpdate.CurrentTool == "")
            {
                timerShow.Enabled = true;
            }
            else
            {
                timerShow.Enabled = false;
            }
            TempX = e.mapX;
            TempY = e.mapY;
            //ModData.v_QueryResult.Hide();
        }

        //把当前图幅号的操作日志内容显示出来  famgmiao  20090708
        public bool DisplayLogs(string StrTableName, string StrMapNo, string DataSource, DevComponents.DotNetBar.Controls.ListViewEx pListView)
        {
            //--------初始化ListView控件的属性----------------------------------------------------
            pListView.GridLines = false;
            pListView.FullRowSelect = true;
            pListView.View = View.Details;
            pListView.MultiSelect = true;
            pListView.Scrollable = false;
            pListView.HeaderStyle = ColumnHeaderStyle.Clickable;
            pListView.Columns.Add("图幅号", 100, HorizontalAlignment.Center);
            pListView.Columns.Add("操作类型", 100, HorizontalAlignment.Center);
            pListView.Columns.Add("时间", pListView.Width - 200, HorizontalAlignment.Center);

            pListView.Items.Clear();
            //------------------------------------------------------------------------------------

            OleDbConnection vOleDbConnection = new OleDbConnection();
            vOleDbConnection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DataSource;
            vOleDbConnection.Open();
            try
            {
                if (vOleDbConnection.State == ConnectionState.Open)
                {
                    string StrSQL = "";
                    StrSQL = "Select * from " + StrTableName + " where MAP_NEWNO =" + "'" + StrMapNo + "'" + " order by OperateTime desc";

                    OleDbDataAdapter pDataAdapter = new OleDbDataAdapter(StrSQL, vOleDbConnection);
                    DataTable pDataTable = new DataTable();
                    pDataAdapter.Fill(pDataTable);
                    //pDataTable.DefaultView.Sort = "Time  desc";
                    //pDataTable.DefaultView.Sort = "Time  asc";
                    DataRow pDataRow;
                    if (pDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < pDataTable.Rows.Count; i++)
                        {
                            pDataRow = pDataTable.Rows[i];
                            ListViewItem Item = new ListViewItem();
                            Item.SubItems.Clear();
                            Item.SubItems[0].Text = pDataRow[0].ToString();
                            for (int j = 1; j < pDataTable.Columns.Count; j++)
                            {
                                //读取数据库中字段 
                                Item.SubItems.Add(pDataRow[j].ToString());
                            }
                            pListView.Items.Add(Item);
                            if (Item.SubItems[1].Text == "入库")
                            {
                                pListView.Items[i].ImageIndex = 1;
                            }
                            else if (Item.SubItems[1].Text == "更新")
                            {
                                pListView.Items[i].ImageIndex = 2;
                            }
                            else
                            {
                                pListView.Items[i].ImageIndex = 3;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception Err)
            {
                MessageBox.Show("出现错误" + Err, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false; ;
            }
            finally
            {
                vOleDbConnection.Close();
            }
        }



        /// <summary>
        /// 当地图上有选择的要素时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMapControl_OnSelectionChanged(object sender, EventArgs e)
        {
            //改变数据更新对比列表内容
            IFeatureLayer pFeatureLay = ModDBOperator.GetMapFrameLayer(ModData.Scale, ModData.v_AppGisUpdate.MapControl, "示意图") as IFeatureLayer;
            if (pFeatureLay != null)
            {
                IFeatureSelection pFeatSel = pFeatureLay as IFeatureSelection;
                if (pFeatureLay.Visible == true && pFeatSel.SelectionSet.Count != 0)
                {
                    ModData.v_RefreshUpdateDataInfo = true;
                }
            }
        }

        /// <summary>
        /// 单焦点在MAP上，按下某个键时轻开时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMapControl_OnKeyUp(object sender, IMapControlEvents2_OnKeyUpEvent e)
        {
        }

        //利用此事件结合局部变量m_bGroupLayerVisible控制解决GroupLayer显示问题
        private void axTOCControl_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        {
            //IBasicMap pMap = null;
            //ILayer pLayer = null;
            //System.Object other = null;
            //System.Object LayerIndex = null;
            //System.Drawing.Point pPoint = new System.Drawing.Point(e.x, e.y);

            //esriTOCControlItem TOCItem = esriTOCControlItem.esriTOCControlItemNone;
            //ITOCControl2 tocControl = (ITOCControl2)axTOCControl.Object;

            //tocControl.HitTest(e.x, e.y, ref TOCItem, ref pMap, ref pLayer, ref other, ref LayerIndex);
            //if (e.button == 1)
            //{
            //    //if (TOCItem == esriTOCControlItem.esriTOCControlItemLayer && pLayer is IGroupLayer)
            //    //{
            //    //    if (m_bGroupLayerVisible == pLayer.Visible)
            //    //    {
            //    //        IMapControlDefault pMapcontrol = axTOCControl.Buddy as IMapControlDefault;
            //    //        pMapcontrol.ActiveView.Refresh();
            //    //    }
            //    //}
            //}
        }
        //added by chulili 20110802 切换到元数据tab页
        public void TurnToSheetTab()
        {
            tabControl.SelectedTab = tabItemJHTable;
        }
        //added by chulili 20110802 切换到行政区tab页
        public void TurnToXZQTab()
        {
            tabControl.SelectedTab = tabItemXZQ;
        }
        public void AdjustLayerOrder()
        {
            tabControl.SelectedTab = tabItemDBLayer;
        }
        /// <summary>
        /// 控件切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlInfo_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
        {
            if (!tabControlInfo.SelectedTab.Name.Equals("tabSceneView"))//控制三维TOC 的显隐   张琪    20110706
            {
                tabItem3DLayer.Visible = false;
            }
            if (tabControlInfo.SelectedTab.Name.Equals("tabItemMapView"))
            {
                //m_controlsSynchronizer.ActivateMap();
                ModData.v_AppGisUpdate.CurrentControl = axMapControl.Object;
                //  张琪添加    20110709
                tabControl.SelectedTab = tabItemDBSource; //changed by chulili 20110830 tabItemDBLayer->tabItemDBSource 图层目录已隐藏
            }
            else if (tabControlInfo.SelectedTab.Name.Equals("tabSceneView"))
            {
                ModData.v_AppGisUpdate.CurrentControl = this.SceneControl.axSceneCtl.Object; 
                //选中三维视图同时选中三维图层Tab并可见       张琪    20110706
                tabControl.SelectedTab = tabItem3DLayer;
                axTOCControl1.SetBuddyControl(this.SceneControl.axSceneCtl.Object);
                axTOCControl.Update();
                tabItem3DLayer.Visible = true;
             

                //
                if (this.SceneControl.axSceneCtl.Scene.LayerCount > 0) return;
                for (int i = 0; i < 1; i++)
                {
                    //this.SceneControl.axSceneCtl.Scene.AddLayer(this.axMapControl.get_Layer(i), true);
                }
                this.SceneControl.axSceneCtl.SceneGraph.RefreshViewers();
            }
            else if (tabControlInfo.SelectedTab.Name.Equals("tabMetadataView"))//tabMetadataView 元数据视图 //shduan 20110619*****************
            {
                //控制菜单栏和工具栏的按钮的可用性 ,需要增加
                //  张琪添加
 				if (tabControl.SelectedTab != tabItemJHTable)//added by chulili 20110727  添加判断，如果本来就是元数据页面，就不用再初始化
                {
                    tabControl.SelectedTab = tabItemJHTable;
                    treeViewJHTable.Nodes.Clear();
                    InitJHTBTree();//由yjl add in 2011-5-27
                    treeViewJHTable.ExpandAll();
                }                
            }
            //*********************************************************************************************************************************
            else
            {
                //做工具条的关联
                if (ModData.v_AppGisUpdate.DicTabs != null)
                {
                    foreach (DevComponents.DotNetBar.RibbonTabItem item in ModData.v_AppGisUpdate.DicTabs.Keys)
                    {
                        if (item.Name.Equals("DataCarto"))
                        {
                            item.Select();
                        }
                    }
                }

            }
        }


        private void axMapControl_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            if (ModData.v_AppGisUpdate.ScaleBoxList.Count > 0)
            {
                foreach (ComboBoxItem item in ModData.v_AppGisUpdate.ScaleBoxList)
                {
                    if (item.Name.Equals("scale"))
                    {
                        item.ComboBoxEx.Text = "1:" + axMapControl.Map.MapScale.ToString("0.00");
                        break;
                    }
                }
            }
        }
        //yjl add 2011-5-27 for 初始化 行政区树 and 接合图表 树
        #region
        ////yjl调阅数据后根据XZQ层和数据单元初始化右边的行政区树
        //private void InitXZQTree()
        //{
        //    IMap mMap = axMapControl.Map;
        //    //TreeNode[] mNode = MapDocTree.Nodes.Find("行政区", true);
        //    if (mNode.Length == 0)
        //        return;
        //    TreeNode city = DataUnitTree.Nodes[0].Nodes[0];
        //    treeViewXZQ.Nodes.Add(city.Name, city.Text, 18, 19);
        //    city = DataUnitTree.Nodes[0].Nodes[0].Nodes[0];
        //    treeViewXZQ.Nodes[0].Nodes.Add(city.Name, city.Text, 18, 19);


        //    TreeNode mTN = mNode[0];

        //    IFeatureLayer mFL = GetLayerofTreeNode(mTN) as IFeatureLayer;//call sub proc
        //    IFeatureClass mFC = mFL.FeatureClass;
        //    IFeatureCursor mFCs = mFC.Search(null, false);
        //    IFeature mF = mFCs.NextFeature();
        //    int fdXZQMC = mFC.Fields.FindField("XZQMC");
        //    int fdXZQDM = mFC.Fields.FindField("XZQDM");
        //    while (mF != null)
        //    {
        //        try
        //        {
        //            TreeNode tn = new TreeNode();
        //            tn.Text = mF.get_Value(fdXZQMC).ToString();
        //            tn.Name = mF.get_Value(fdXZQDM).ToString();
        //            tn.ImageIndex = 18;
        //            tn.SelectedImageIndex = 19;
        //            treeViewXZQ.Nodes[0].Nodes[0].Nodes.Add(tn);
        //        }
        //        catch
        //        {

        //        }
        //        mF = mFCs.NextFeature();
        //    }
        //    treeViewXZQ.ExpandAll();



        //}
        //yjl初始化接合图表树2011-5-26
        private void InitJHTBTree()
        {
            IMap pMap = axMapControl.Map;
            //TreeNode[] mNode = MapDocTree.Nodes.Find("接合图表", true);
            //if (mNode.Length == 0)
            //    return;
       
            string schemaPath = Application.StartupPath + "\\..\\Res\\Xml\\JHTBschema.xml";
            if (!File.Exists(schemaPath))
            {
                return;
            }
            InitFromJHTBschemaXML(schemaPath);

            string JHTBPath=Application.StartupPath + "\\..\\Res\\Xml\\JHTB.xml";
            if (File.Exists(JHTBPath))
            {
                InitFromJHTBXML(JHTBPath);
            }
            else
            {
            
                try
                {
                   InitFromJHTBLayer(pMap);
                }
                catch
                {

                }
                
            }
            
           



        }
        //从xml文件初始化接合图表树for schema
        private void InitFromJHTBschemaXML(string cPath)
        {
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(cPath);

                XmlNodeList xnl = cXmlDoc.GetElementsByTagName("JHTBTree");
                string xnlattr=xnl.Item(0).Attributes["ItemName"].Value;
                //changed by chulili 更换节点图片
                treeViewJHTable.Nodes.Add(xnlattr, xnlattr, 16, 16);
                //treeViewJHTable.Nodes.Add(xnlattr, xnlattr, 22, 23);
                //TreeNode tNode1 = treeViewJHTable.Nodes[0];
                
                //foreach (XmlNode xn in xnl.Item(0).ChildNodes)
                //{
                //    string xnattr = xn.Attributes["ItemName"].Value;
                //    //changed by chulili 更换节点图片
                //    tNode1.Nodes.Add(xnattr, xnattr, 16, 16);
                //    //tNode1.Nodes.Add(xnattr, xnattr, 22, 23);
                  //}
                cXmlDoc = null;
           }

       }
        //从xml文件填充接合图表树
        private void InitFromJHTBXML(string cPath)
        {
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(cPath);

                XmlNodeList xnl = cXmlDoc.GetElementsByTagName("BZTFScale");
                TreeNode tNode1 = treeViewJHTable.Nodes[0];
                for(int scaleNode=0;scaleNode<tNode1.Nodes.Count;scaleNode++)
                {
                   
                    foreach (XmlNode xn in xnl)
                    {
                        if(tNode1.Nodes[scaleNode].Text!=xn.Attributes["ItemName"].Value)
                            continue;
                        tNode1.Nodes[scaleNode].Tag = xn.Attributes["LayerName"].Value;
                        TreeNode tNode2 = treeViewJHTable.Nodes[0].Nodes[scaleNode];
                        foreach (XmlNode xn2 in xn.ChildNodes)
                        {
                            //changed by chulili 更换节点图片
                            TreeNode tNode3 = tNode2.Nodes.Add(xn2.Attributes["ItemName"].Value, xn2.Attributes["ItemName"].Value, 16, 16);
                            //TreeNode tNode3 = tNode2.Nodes.Add(xn2.Attributes["ItemName"].Value, xn2.Attributes["ItemName"].Value, 22, 23);
                            if (xn2.HasChildNodes)
                            {
                                foreach (XmlNode xn3 in xn2.ChildNodes)
                                {
                                    //changed by chulili 更换节点图片
                                    tNode3.Nodes.Add(xn3.Attributes["ItemName"].Value, xn3.Attributes["ItemName"].Value, 16, 16);
                                    //tNode3.Nodes.Add(xn3.Attributes["ItemName"].Value, xn3.Attributes["ItemName"].Value, 22, 23);
                                }
                            }
                           
                        }
                       
                    }
                }
                cXmlDoc = null;
           }

       }
        //从图层填充接合图表树
        private void InitFromJHTBLayer(IMap inMap)
        {
            for(int i=0;i<inMap.LayerCount;i++)
            {
                ILayer pLayer=inMap.get_Layer(i);
                if(pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer=pLayer as ICompositeLayer;
                    for(int j=0;j<pCLayer.Count;j++)
                    {
                        if (pCLayer.get_Layer(j).Name != "接合图表")
                            continue;
                        IFeatureLayer pFLayer=pCLayer.get_Layer(j) as IFeatureLayer;
                        fillJHTBTreeFromFL(pFLayer);

                    }
                }
                else//不是grouplayer
                {
                    if (pLayer.Name != "接合图表")
                        continue;
                    IFeatureLayer pFLayer=pLayer as IFeatureLayer;
                    fillJHTBTreeFromFL(pFLayer);
                }
            }
        }
        //根据featureclass字段填充treeJHTB,the caller is InitFromJHTBLayer(IMap inMap)
        private void fillJHTBTreeFromFL(IFeatureLayer pFLayer)
        {
            IFeatureClass pFC=pFLayer.FeatureClass;
            int dexScale=pFC.FindField("比例尺分母");
            int dexTFH=pFC.FindField("图幅号");
            TreeNode tNode1 = treeViewJHTable.Nodes[0];
            for(int scaleNode=0;scaleNode<tNode1.Nodes.Count;scaleNode++)
            {
                if(pFC.GetFeature(0).get_Value(dexScale).ToString()==tNode1.Nodes[scaleNode].Text.Substring(2))
                {
                    TreeNode tNode2=tNode1.Nodes[scaleNode];
                    IFeatureCursor pFCursor=pFC.Search(null,false);
                    IFeature pFeature=pFCursor.NextFeature();
                    while(pFeature!=null)
                    {
                        string pFValue=pFeature.get_Value(dexTFH).ToString();
                        //changed by chulili 更换节点图片
                        tNode2.Nodes.Add(pFValue, pFValue, 16, 16);
                        //tNode2.Nodes.Add(pFValue,pFValue,22,23);
                        pFeature = pFCursor.NextFeature();
                    }
                }
            }
        }
        ////从图层填充行政辖区树
        //private void InitFromXZQLayer(IMap inMap)
        //{
            
        //    for (int i = 0; i < inMap.LayerCount; i++)
        //    {
        //        ILayer pLayer = inMap.get_Layer(i);
        //        if (pLayer is IGroupLayer)
        //        {
        //            ICompositeLayer pCLayer = pLayer as ICompositeLayer;
        //            for (int j = 0; j < pCLayer.Count; j++)
        //            {
        //                if (pCLayer.get_Layer(j).Name != "行政辖区")
        //                    continue;
        //                IFeatureLayer pFLayer = pCLayer.get_Layer(j) as IFeatureLayer;
        //                fillXZQTreeFromFL(pFLayer);

        //            }
        //        }
        //        else//不是grouplayer
        //        {
        //            if (pLayer.Name != "行政辖区")
        //                continue;
        //            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
        //            fillXZQTreeFromFL(pFLayer);
        //        }
        //    }
        //}
        ////从xml文件初始化行政区树
        //private void InitFromXZQXML000(string cPath)
        //{
        //    XmlDocument cXmlDoc = new XmlDocument();

        //    if (cXmlDoc != null)
        //    {
        //        cXmlDoc.Load(cPath);

        //        XmlNodeList xnl = cXmlDoc.GetElementsByTagName("XZQTree");
        //        string xnlattr = xnl.Item(0).Attributes["ItemName"].Value;
        //        string feaName = xnl.Item(0).Attributes["FeatureClassName"].Value;
        //        treeViewXZQ.Nodes.Add(feaName, xnlattr, 21, 21);//行政区节点
        //        DevComponents.AdvTree.Node tNode1 = treeViewXZQ.Nodes[0];
        //        if (!xnl.Item(0).HasChildNodes)
        //            return;
        //        tNode1.Expand();
        //        foreach (XmlNode xn in xnl.Item(0).ChildNodes)//全国0
        //        {
        //            string xnatt = xn.Attributes["ItemName"].Value;
        //            string xzqCode = xn.Attributes["XzqCode"].Value;
        //            tNode1.Nodes.Add(xzqCode, xnatt, 0, 0);
        //            if (!xn.HasChildNodes)
        //                continue;
        //            int i1 = 0;

        //            DevComponents.AdvTree.Node tNode2 = tNode1.Nodes[0];
        //            tNode2.Expand();
        //            foreach (XmlNode xn1 in xn.ChildNodes)//省1
        //            { 
        //                string xnatt1 = xn1.Attributes["ItemName"].Value;
        //                string xzqCode1 = xn1.Attributes["XzqCode"].Value;
        //                tNode2.Nodes.Add(xzqCode1, xnatt1, 17, 17);
        //                if (!xn1.HasChildNodes)
        //                    continue;

        //                DevComponents.AdvTree.Node tNode3 = tNode2.Nodes[i1];
        //                int i2 = 0;
        //                foreach (XmlNode xn2 in xn1.ChildNodes)//市2
        //                {
        //                    string xnatt2 = xn2.Attributes["ItemName"].Value;
        //                    string xzqCode2 = xn2.Attributes["XzqCode"].Value;
        //                    tNode3.Nodes.Add(xzqCode2, xnatt2, 17, 17);
        //                    if (!xn2.HasChildNodes)
        //                        continue;

        //                    DevComponents.AdvTree.Node tNode4 = tNode3.Nodes[i2];
        //                    int i3 = 0;
        //                    foreach (XmlNode xn3 in xn2.ChildNodes)//县3
        //                    {
        //                        string xnatt3 = xn3.Attributes["ItemName"].Value;
        //                        string xzqCode3 = xn3.Attributes["XzqCode"].Value;
        //                        tNode4.Nodes.Add(xzqCode3, xnatt3, 17, 17);
        //                        if (!xn3.HasChildNodes)
        //                            continue;

        //                        TreeNode tNode5 = tNode4.Nodes[i3];
                                
        //                        foreach (XmlNode xn4 in xn3.ChildNodes)//村4
        //                        {
        //                            string xnatt4 = xn4.Attributes["ItemName"].Value;
        //                            string xzqCode4 = xn4.Attributes["XzqCode"].Value;
        //                            tNode5.Nodes.Add(xzqCode4, xnatt4, 17, 17);
                                    
        //                        }
        //                        i3++;
        //                    }
        //                    if (xn2.Attributes["Expand"].Value == "1")
        //                        tNode4.Expand();
        //                    i2++;
        //                }
        //                //if (xn1.Attributes["Expand"].Value == "1")
        //                    tNode3.Expand();
        //                i1++;
        //            }
        //            tNode2.Expand();
        //            tNode1.Expand();
        //        }
        //        cXmlDoc = null;
        //    }

        //}
        //从xml文件初始化行政区树
        private void InitFromXZQXML(string cPath)
        {
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(cPath);
                //ZQ   20110729 modify
                //XmlNode xnl = cXmlDoc.FirstChild;
                //XmlNode vChinaNode = xnl.NextSibling.FirstChild;//XZQTree节点
                //changed by chulili 准确找到行政区节点
                string strSearch = "//XZQTree";
                XmlNode vChinaNode = cXmlDoc.SelectSingleNode(strSearch);

                //XmlNode vChinaNode = xnl.NextSibling;
                string xnlattr = vChinaNode.Attributes["ItemName"].Value;
                //xzqFeaClassName = vChinaNode.Attributes["FeatureClassName"].Value;

                //ZQ  20110731   Add  25万省界图层及字段    Zq 20110801 modify
                strXZQFeaClassNameQG = vChinaNode.Attributes["FeatureClassNameQG"].Value;
                strXZQCodeFieldQG = vChinaNode.Attributes["FieldQG"].Value;
                //ZQ  20110731   Add  25万市界图层及字段
                strXZQFeaClassNameGS = vChinaNode.Attributes["FeatureClassNameGS"].Value;
                strXZQCodeFieldGS = vChinaNode.Attributes["FieldGS"].Value;
                //end    20110801d
                //ZQ  20110804 add 5万县界图层及字段
                strXZQFeaClassNameGX = vChinaNode.Attributes["FeatureClassNameGX"].Value;
                strXZQFieldGX = vChinaNode.Attributes["FieldGX"].Value;
                //end
                //xzqCodeField = vChinaNode.Attributes["Field"].Value;
                DevComponents.AdvTree.Node vRootNode = new DevComponents.AdvTree.Node();
                vRootNode.Text = xnlattr;
                vRootNode.Name = xnlattr;
                vRootNode.Tag = vChinaNode.Name;
                vRootNode.ImageIndex = 38;
                vRootNode.DataKey = vChinaNode as object;
                treeViewXZQ.Nodes.Add(vRootNode);//行政区节点
                DevComponents.AdvTree.Node tNode1 = treeViewXZQ.Nodes[0];
                if (!vChinaNode.HasChildNodes)
                    return;

                foreach (XmlNode vSubXmlNode in vChinaNode.ChildNodes)//全国0//改为省节点
                {
                    //shduan 20110731
                    //int iTreeNodeClass = 0;
                    string strXZQName = vSubXmlNode.Attributes["ItemName"].Value;
                    string strXZQCode = vSubXmlNode.Attributes["XzqCode"].Value;
                    DevComponents.AdvTree.Node vNode = new DevComponents.AdvTree.Node();
                    vNode.Text = strXZQName;
                    vNode.Name = strXZQCode;
                    vNode.Tag = vSubXmlNode.Name;
                    vNode.DataKey = vSubXmlNode as object;
                    //added by chulili 20110818 设置节点图标 
                    switch (vSubXmlNode.Name)
                    {
                        case "Province":
                            vNode.ImageIndex = 35;
                            //vNode.SelectedImageIndex = 35;
                            break;
                        case "City":
                            vNode.ImageIndex = 37;
                            //vNode.SelectedImageIndex = 37;
                            break;
                        case "County":
                            vNode.ImageIndex = 36;
                            //vNode.SelectedImageIndex = 36;
                            break;
                      
                    }
                    //end added by chulili 20110818
                    tNode1.Nodes.Add(vNode);
                    getTreeNodeFromXMLNode(vSubXmlNode, vNode);
                }
                //tNode1.Expand();
            }
        }
        //递归遍历xmlnode初始化treenode
        //shduan 20110731
        private void getTreeNodeFromXMLNode(XmlNode vXmlNode, DevComponents.AdvTree.Node vNode)
        {
            //iTreeNodeClass = iTreeNodeClass + 1;
            foreach (XmlNode vSubXmlNode in vXmlNode.ChildNodes)//全国0
            {
                string strXZQName = vSubXmlNode.Attributes["ItemName"].Value;
                string strXZQCode = vSubXmlNode.Attributes["XzqCode"].Value;
                DevComponents.AdvTree.Node vSubNode = new DevComponents.AdvTree.Node();
                vSubNode.Text = strXZQName;
                vSubNode.Name = strXZQCode;
                vSubNode.Tag = vSubXmlNode.Name;
                vSubNode.DataKey = vSubXmlNode as object;
                //added by chulili 20110818 设置节点图标 
                switch (vSubXmlNode.Name)
                {
                    case "Province":
                        vSubNode.ImageIndex = 35;
                        //vSubNode.SelectedImageIndex = 35;
                        break;
                    case "City":
                        vSubNode.ImageIndex = 37;
                        //vSubNode.SelectedImageIndex = 37;
                        break;
                    case "County":
                        vSubNode.ImageIndex = 36;
                        //vSubNode.SelectedImageIndex = 36;
                        break;
                }
                //end added by chulili 20110818
                vNode.Nodes.Add(vSubNode);
                getTreeNodeFromXMLNode(vSubXmlNode, vSubNode);
            }
            if (vNode.Tag.ToString() == "XZQTree") //如果行政区代码为0就就展开，若不是则不展开，防止递归展开所以子节点 xisheng 20110803
            vNode.Expand();
        }
        //递归遍历xmlnode初始化treenode
        /*private TreeNode tNodeFromXMLNode(XmlNode inXmlNode)
        {
            string xnatt = inXmlNode.Attributes["ItemName"].Value;
            string xzqCode = inXmlNode.Attributes["XzqCode"].Value;
            TreeNode tNode = new TreeNode(xnatt, 24, 24);
            tNode.Name = xzqCode;
            if (inXmlNode.HasChildNodes)
            {
                foreach (XmlNode xn in inXmlNode.ChildNodes)
                {
                    tNode.Nodes.Add(tNodeFromXMLNode(xn));
                }
            }
            try
            {
                if (inXmlNode.Attributes["Expand"].Value == "1")
                tNode.Expand();
            }
            catch
            {
            }
            return tNode;
        }*/
        
        //根据featureclass字段填充treeXZQ,the caller is InitFromJHTBLayer(IMap inMap)
        //private void fillXZQTreeFromFL(IFeatureLayer pFLayer)
        //{
        //    IFeatureClass pFC = pFLayer.FeatureClass;
            
        //    int dexTFH = pFC.FindField("行政区名称");
        //    DevComponents.AdvTree.Node tNode1 = treeViewXZQ.Nodes[0];
        //    IFeatureCursor pFCursor = pFC.Search(null, false);
        //    IFeature pFeature = pFCursor.NextFeature();
        //    while (pFeature != null)
        //    {
        //        string pFValue = pFeature.get_Value(dexTFH).ToString();

        //        tNode1.Nodes.Add(pFValue, pFValue, 24, 24);
        //        pFeature = pFCursor.NextFeature();
        //    }  
        //}
        //获取行政区范围 韶关特定功能代码
        //从父节点中获取父节点所在图层的NodeKey，FieldXZBM（行政区编码的属性名），然后获取到本节点对应的地物范围
        private IGeometry getExtentByXZQ(DevComponents.AdvTree.Node vSelectNode)
        {
            if (vSelectNode.Parent != null)
            {
                DevComponents.AdvTree.Node vParentNode = vSelectNode.Parent;
                if (vParentNode.DataKey != null)
                {
                    XmlNode pNode = vParentNode.DataKey as XmlNode;
                    if ((pNode as XmlElement)!=null)
                    {
                        XmlElement pNodeEle = pNode as XmlElement;
                        if (pNodeEle.HasAttribute("NodeKey") && pNodeEle.HasAttribute("FieldXZBM"))
                        {
                            string strNodeKey = pNodeEle.GetAttribute("NodeKey");
                            string strField = pNodeEle.GetAttribute("FieldXZBM");
                            IFeatureClass pFeatureClass = ModDBOperate.GetFeatureClassByNodeKey(strNodeKey);
                            return getXZQExtentFromFL(pFeatureClass, vSelectNode.Name , strField);
                        }
                    }
                }
            }
            return null;
        }
        //根据 行政区 搜索 行政区图层 得到相应的 范围
		//参数说明xzqFCname,xzqfield从xml读取
        private IGeometry getExtentByXZQ(TreeNode vSelectNode, IMap inMap,string xzqFCname,string xzqfield)
        {
            //shduan 20110731
            string inXZQMC = vSelectNode.Name;
            IGeometry psGeometry = null;
			ILayer pXzqLayer = null;//added by chulili 20110729 增加对是否加载行政区图层的判断
            for (int i = 0; i < inMap.LayerCount; i++)
            {

                ILayer pLayer = inMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        if (pCLayer.get_Layer(j) is IFeatureLayer)
                        {
                            IFeatureLayer pFLayer = pCLayer.get_Layer(j) as IFeatureLayer;
 							//added by chulili 20110729 错误保护
                            if(pFLayer!=null)
                            {
                                string name = (pFLayer.FeatureClass as IDataset).Name;
                                if (name.Contains("."))//20110818 xisheng added 支持sde用户名的问题
                                {
                                    name=name.Substring(name.LastIndexOf(".") + 1);
                                }
                                if (name== xzqFCname)
                                {
                                    pXzqLayer = pFLayer;//added by chulili 20110729 增加对是否加载行政区图层判断
                                    psGeometry = getXZQExtentFromFL(pFLayer, inXZQMC, xzqfield);
                                    if (psGeometry != null)
                                        break;
                                }
                            }
                        }

                    }
                }
                else//不是grouplayer
                {
                    //if (pLayer.Name != "行政辖区")
                    //    continue;
                    IFeatureLayer pFLayer = pLayer as IFeatureLayer;
					//added by chulili 20110729 错误保护
                    if (pFLayer != null)
                    {
                        string name = (pFLayer.FeatureClass as IDataset).Name;
                        if (name.Contains("."))//20110818 xisheng added 支持sde用户名的问题
                        {
                            name = name.Substring(name.LastIndexOf(".") + 1);
                        }
                        if (xzqFCname == name)
                        {
                            pXzqLayer = pFLayer;//added by chulili 20110729 增加对是否加载行政区图层判断
                            psGeometry = getXZQExtentFromFL(pFLayer, inXZQMC, xzqfield);
                            if (psGeometry != null)
                                break;
                        }
                    }
                }
                //shduan 20110731
                if (psGeometry != null)
                {
                    break;
                }
            }			
            //added by chulili 20110729 增加对是否加载行政区图层的判断
            if (pXzqLayer == null)
            {
                //MessageBox.Show("未加载行政区图层!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //end added by chulili
            return psGeometry;
        }
        //根据 行政区 搜索 行政区图层 得到相应的 行政区要素
        //参数说明xzqFCname,xzqfield从xml读取yjl20110920 add
        private IFeatureClass getXZQ_FC(IMap inMap, string xzqFCname)
        {
            IFeatureClass pFeatureClass = null;
            ILayer pXzqLayer = null;//added by chulili 20110729 增加对是否加载行政区图层的判断
            for (int i = 0; i < inMap.LayerCount; i++)
            {

                ILayer pLayer = inMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        if (pCLayer.get_Layer(j).Valid && pCLayer.get_Layer(j) is IFeatureLayer)
                        {
                            IFeatureLayer pFLayer = pCLayer.get_Layer(j) as IFeatureLayer;
                            //added by chulili 20110729 错误保护
                            if (pFLayer != null)
                            {
                                string name = (pFLayer.FeatureClass as IDataset).Name;
                                if (name.Contains("."))//20110818 xisheng added 支持sde用户名的问题
                                {
                                    name = name.Substring(name.LastIndexOf(".") + 1);
                                }
                                if (name == xzqFCname)
                                {
                                    pFeatureClass= pFLayer.FeatureClass;
                                }
                            }
                        }

                    }
                }
                else//不是grouplayer
                {
                    //if (pLayer.Name != "行政辖区")
                    //    continue;
                    IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                    //added by chulili 20110729 错误保护
                    if (pFLayer != null)
                    {
                        string name = (pFLayer.FeatureClass as IDataset).Name;
                        if (name.Contains("."))//20110818 xisheng added 支持sde用户名的问题
                        {
                            name = name.Substring(name.LastIndexOf(".") + 1);
                        }
                        if (xzqFCname == name)
                        {
                            pFeatureClass= pFLayer.FeatureClass;
                        }
                    }
                }
   
            }

            return pFeatureClass;
        }
        //根据行政区图层，行政编码字段名称和字段值，获取对应的地物几何数据
        private IGeometry getXZQExtentFromFL(IFeatureClass pXZQFeaCls, string strXZQBM, string strFieldXZQBM)
        {

            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                if (pXZQFeaCls != null)
                {//查找行政编码属性列
                    int iIndex = pXZQFeaCls.Fields.FindField(strFieldXZQBM);
                    IField pField = pXZQFeaCls.Fields.get_Field(iIndex);
                    //构造过滤条件
                    if (pField.Type.ToString() == "esriFieldTypeString")
                    {

                        pQueryFilter.WhereClause = strFieldXZQBM + "='" + strXZQBM + "'";
                    }
                    else if (pField.Type.ToString() == "esriFieldTypeDouble")
                    {
                        pQueryFilter.WhereClause = strFieldXZQBM + "=" + strXZQBM;
                    }
                    //end
                    //查找
                    IFeatureCursor pFCursor = pXZQFeaCls.Search(pQueryFilter, false);
                    IFeature pFeature = pFCursor.NextFeature();
                    //只获取找到的第一个地物
                    if (pFeature != null)
                    {
                        pFCursor = null;
                        pQueryFilter = null;
                        return pFeature.ShapeCopy;
                    }
                    pFCursor = null;
                    pQueryFilter = null;
                }

            }
            catch
            {

            }

            return null;
        }
        //搜索FC,定位行政区得范围，the caller is getExtentByXZQ(string inXZQMC, IMap inMap)
        private IGeometry getXZQExtentFromFL(IFeatureLayer pFLayer, string inXZQMC, string xzqfield)
        {
            
                try
                {
                    //IFeatureClass pFC = pFLayer.FeatureClass;
                    ////int dexScale = pFC.FindField("比例尺分母");
                    //int dexTFH = pFC.FindField(xzqfield);
     
                    //IFeatureCursor pFCursor = pFC.Search(null, false);
                    //IFeature pFeature = pFCursor.NextFeature();
                    //while (pFeature != null)
                    //{
                    //    if (pFeature.get_Value(dexTFH).ToString() == inXZQMC)
                    //        return pFeature.ShapeCopy;
                    //    pFeature = pFCursor.NextFeature();
                    //}

                  IFeatureClass pFC = pFLayer.FeatureClass;
                  IQueryFilter pQueryFilter = new QueryFilterClass();
                 //ZQ   20110807  Add
                  ITable pTable = pFC as ITable;
                  IFields pFields = pTable.Fields;
                  IField pField = pFields.get_Field( pFields.FindField(xzqfield));
                   if( pField.Type.ToString()=="esriFieldTypeString")
                   {
                    
                    pQueryFilter.WhereClause = xzqfield + "='" + inXZQMC +"'";
                   }
                   else if (pField.Type.ToString() == "esriFieldTypeDouble")
                   {
                       pQueryFilter.WhereClause = xzqfield + "=" + inXZQMC;
                   }
                    //end
                    IFeatureCursor pFCursor = pFC.Search(pQueryFilter, false);
                    IFeature pFeature = pFCursor.NextFeature();
                    while (pFeature != null)
                    {
                        pFCursor = null;
                        return pFeature.ShapeCopy;
                    }
                    pFCursor = null;
                
                }
                catch
                {

                }
            
            return null;
        }
        
        //根据 图幅号 搜索 接合表图层 得到相应的 范围
        private IGeometry getExtentByTFNO(string inTFNO, IMap inMap, bool isLScale,string strLayerName)
        {
            IGeometry psGeometry=null;
            for (int i = 0; i < inMap.LayerCount; i++)
            {
                
                ILayer pLayer = inMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        if (pCLayer.get_Layer(j).Name != strLayerName)
                            continue;
                        IFeatureLayer pFLayer = pCLayer.get_Layer(j) as IFeatureLayer;
                        psGeometry=getExtentFromFL(pFLayer, inTFNO, isLScale);
                        if (psGeometry != null)
                            break;

                    }
                }
                else//不是grouplayer
                {
                    if (pLayer.Name != strLayerName)
                        continue;
                    IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                    psGeometry = getExtentFromFL(pFLayer, inTFNO, isLScale);
                    if (psGeometry != null)
                        break;
                }
            }
            return  psGeometry;
        }
        //搜索FC,定位图幅号得范围，the caller is getExtentByTFNO(string inTFNO, IMap inMap,bool isLScale)
        private IGeometry getExtentFromFL(IFeatureLayer pFLayer, string inTFNO, bool isLScale)
        {
            if (isLScale)
            {
                try
                {
                    IFeatureClass pFC = pFLayer.FeatureClass;
                    //int dexScale = pFC.FindField("比例尺分母");
                    int dexTFH = pFC.FindField("图幅号");
                    if (pFC.Search(null, false).NextFeature().get_Value(dexTFH).ToString().Substring(5, 1) == inTFNO.Substring(5, 1))
                    {

                        IFeatureCursor pFCursor = pFC.Search(null, false);
                        IFeature pFeature = pFCursor.NextFeature();
                        while (pFeature != null)
                        {
                            if (pFeature.get_Value(dexTFH).ToString() == inTFNO)
                                return pFeature.ShapeCopy;
                            pFeature = pFCursor.NextFeature();
                        }
                    }
                }
                catch
                {
 
                }
             }
            return null;
        }
        

        #endregion//yjl add end

        private void treeViewJHTable_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            cntxtJMP.Hide();
            //if (e.Button == MouseButtons.Left)
            //{
                treeViewJHTable.SelectedNode = e.Node;
                //e.Node.Expand();
            //}

            //右键点击的时候弹出右键菜单
            if (e.Button == MouseButtons.Right)
            {
                treeViewJHTable.SelectedNode = e.Node;
                System.Drawing.Point ClickPoint = treeViewJHTable.PointToScreen(new System.Drawing.Point(e.X, e.Y));
                TreeNode tSelNode;
                tSelNode = e.Node;
                if (tSelNode != null&&tSelNode.Level==2&&tSelNode.Nodes.Count==0)
                {
                    //cntxtJMP.TopLevel = false;
                    //cntxtJMP.Parent = treeViewJHTable as Control;
                    //cntxtJMP.Show(e.Location);
                    System.Drawing.Point pPoint = new System.Drawing.Point(e.Location.X, e.Location.Y);
                    DevComponents.DotNetBar.ButtonItem item = null;
                    DevComponents.DotNetBar.ContextMenuBar menuBar = _dicContextMenu["ContextMenuMapSheet"];
                    this.Controls.Add(menuBar);
                    if (_dicContextMenu.ContainsKey("ContextMenuMapSheet"))
                    {
                        if (_dicContextMenu["ContextMenuMapSheet"] != null)
                        {
                            if (_dicContextMenu["ContextMenuMapSheet"].Items.Count > 0)
                            {
                                item = _dicContextMenu["ContextMenuMapSheet"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                                if (item != null)
                                {
                                    item.Popup(treeViewJHTable.PointToScreen(pPoint));
                                }
                            }
                        }
                    }
                }
                if (tSelNode != null && tSelNode.Level == 3)
                {
                    //cntxtJMP.TopLevel = true;
                    //cntxtJMP.Parent = treeViewJHTable as Control;
                    //cntxtJMP.Show(e.Location);
                    System.Drawing.Point pPoint = new System.Drawing.Point(e.Location.X, e.Location.Y);
                    DevComponents.DotNetBar.ButtonItem item = null;
                    DevComponents.DotNetBar.ContextMenuBar menuBar = _dicContextMenu["ContextMenuMapSheet"];
                    this.Controls.Add(menuBar);
                    if (_dicContextMenu.ContainsKey("ContextMenuMapSheet"))
                    {
                        if (_dicContextMenu["ContextMenuMapSheet"] != null)
                        {
                            if (_dicContextMenu["ContextMenuMapSheet"].Items.Count > 0)
                            {
                                item = _dicContextMenu["ContextMenuMapSheet"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                                if (item != null)
                                {
                                    item.Popup(treeViewJHTable.PointToScreen(pPoint));
                                }
                            }
                        }
                    }
                }
            }
        }
		        #region   ZQ    20110731    Add  行政区右键菜单 
        
        private IGeometry GetXZQGeometry()
        {
            IGeometry pGeometry = null;
            DevComponents.AdvTree.Node vSelectNode = treeViewXZQ.SelectedNode;
            pGeometry = getExtentByXZQ(vSelectNode);
            //vSelectNode.Tag = treeViewXZQ.SelectedNode.Tag;
            //if (vSelectNode.Tag != null)
            //{
            //    if (vSelectNode.Tag.ToString() == "Province")//省
            //    {
            //        pGeometry = getExtentByXZQ(vSelectNode, axMapControl.Map, strXZQFeaClassNameQG, strXZQCodeFieldQG);
            //    }
            //    else if (vSelectNode.Tag.ToString() == "City")//市
            //    {
            //        pGeometry = getExtentByXZQ(vSelectNode, axMapControl.Map, strXZQFeaClassNameGS, strXZQCodeFieldGS);
            //    }
            //    else if (vSelectNode.Tag.ToString() == "County")//县
            //    {
            //        pGeometry = getExtentByXZQ(vSelectNode, axMapControl.Map, strXZQFeaClassNameGX, strXZQFieldGX);
            //    }
            //}
            return pGeometry;
        }		
        //private void treeViewXZQ_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
        //    //ZQ    20110731   modify
        //    cntxtXZQ.Hide();
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        treeViewXZQ.SelectedNode = e.Node;
               
        //    }

        //    //右键点击的时候弹出右键菜单
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        treeViewXZQ.SelectedNode = e.Node;
        //        //System.Drawing.Point ClickPoint = treeViewXZQ.PointToScreen(new System.Drawing.Point(e.X, e.Y));
        //        TreeNode tSelNode;
        //        tSelNode = e.Node;
        //        //if (tSelNode != null&&tSelNode.Level==1)
        //        //{
        //        //    cntxtJMP.TopLevel = false;
        //        //    cntxtJMP.Parent = treeViewXZQ as Control;
        //        //    cntxtJMP.Show(e.Location);
        //        //}//ZQ    20110731   modify

        //        if (tSelNode != null && tSelNode.Level >= 1 && tSelNode.ImageIndex<37)
        //        {

        //            //cntxtXZQ.TopLevel = false;
        //            //cntxtXZQ.Parent = treeViewXZQ as Control;
        //            //cntxtXZQ.Show(e.Location);
        //            System.Drawing.Point pPoint = new System.Drawing.Point(e.Location.X,e.Location.Y );
        //            DevComponents.DotNetBar.ButtonItem item = null;
        //            DevComponents.DotNetBar.ContextMenuBar menuBar = _dicContextMenu["ContextMenuXZQ"];
        //            this.Controls.Add(menuBar);
        //            if (_dicContextMenu.ContainsKey("ContextMenuXZQ"))
        //            {
        //                if (_dicContextMenu["ContextMenuXZQ"] != null)
        //                {
        //                    if (_dicContextMenu["ContextMenuXZQ"].Items.Count > 0)
        //                    {
        //                        item = _dicContextMenu["ContextMenuXZQ"].Items[0] as DevComponents.DotNetBar.ButtonItem;
        //                        if (item != null)
        //                        {
        //                            item.Popup(treeViewXZQ.PointToScreen(pPoint));
        //                        }
        //                    }
        //                }
        //            }

        //        }

        //    }
        //}
        private void DealRoadorRiverLocation(DevComponents.AdvTree.Node pNode)
        {
            //判断前提条件(高速定位 铁路定位  或者河流定位)
            if (pNode == null) return;
            if (pNode.Parent == null) return;
            DevComponents.AdvTree.Node pParentNode = pNode.Parent;
            if (pParentNode.Name != "RoadRoot" && pParentNode.Name != "RailRoadRoot" && pParentNode.Name != "RiverRoot")
                return;
            //配置文件是否存在
            if (!File.Exists(_RoadandRiverXmlPath))
            {
                return;
            }
            //读取配置文件
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_RoadandRiverXmlPath);
            string strSearch = "//" + pParentNode.Name;
            XmlNode pXmlnode = pXmldoc.SelectSingleNode(strSearch);
            if (pXmlnode == null)
                return;
            //获取图层 编码字段
            string strNodeKey = pXmlnode.Attributes["NodeKey"].Value;
            string strField = pXmlnode.Attributes["FieldName"].Value;
            //直接从数据源中获取地物类
            IFeatureClass pFeatureClass = ModDBOperate.GetFeatureClassByNodeKey(strNodeKey);
            if (pFeatureClass == null)
            {
                return;
            }
            string strFieldValue = pNode.Name;
            //构造查询条件
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            strFieldValue = strFieldValue.Replace(",","','");
            if (strFieldValue.Contains(","))
            {
                pSpatialFilter.WhereClause = strField + " in ('" + strFieldValue + "')";
            }
            else
            {
                pSpatialFilter.WhereClause = strField + "='" + strFieldValue + "'";
            }
            IFeatureCursor pFeaCursor = pFeatureClass.Search(pSpatialFilter,false);
            IFeature pFea = pFeaCursor.NextFeature();
            if (pFea == null) return;
            List<IGeometry> pListGeometry = new List<IGeometry>();
            IEnvelope pEnvelope = pFea.Shape.Envelope;
            while (pFea != null)
            {
                pListGeometry.Add(pFea.Shape);
                pEnvelope.Union(pFea.Shape.Envelope);
                pFea = pFeaCursor.NextFeature();
            }
            //定位并高亮
            IGraphicsContainer psGra = axMapControl.Map  as IGraphicsContainer;
            ResizeEnvelope(pEnvelope,1.2);
            axMapControl.Extent = pEnvelope;
            drawPolyLineElement(pListGeometry, psGra);
            axMapControl.ActiveView.Refresh();
            pListGeometry.Clear();


        }
        //调整矩形框的大小，中心点不变，矩形框放大或缩小，仅作二维矩形
        private void ResizeEnvelope(IEnvelope pEnve,double dSize)
        {
            //排错
            if (pEnve == null)
                return;
            if (dSize == 1)
                return;
            if (dSize <= 0)
                return;
            //取矩形框的高度、宽度
            double dHight = pEnve.Height;
            double dWidth = pEnve.Width;
            //取矩形框的最小最大X Y
            double dxmin = pEnve.XMin;
            double dxmax = pEnve.XMax;
            double dymin = pEnve.YMin;
            double dymax = pEnve.YMax;
            //对矩形框进行缩放
            pEnve.XMin = dxmin - ((dSize - 1) / 2) * dWidth;
            pEnve.XMax = dxmax + ((dSize - 1) / 2) * dWidth;
            pEnve.YMin = dymin - ((dSize - 1) / 2) * dHight ;
            pEnve.YMax = dymax + ((dSize - 1) / 2) * dHight;

        }
        //added by chulili 20110802 从别处拷贝代码
        //在mapcontrol上画多边形
        private void drawPolyLineElement(List<IGeometry> ListGeometry, IGraphicsContainer pGra)
        {
            if (ListGeometry == null)
                return;
            pGra.DeleteAllElements();
            //ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

            try
            {
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                pRGBColor.Red = 0;
                pRGBColor.Green = 0;
                pRGBColor.Blue = 255;
                pLineSymbol.Color = pRGBColor;
                pLineSymbol.Width = 2;

                pRGBColor.Transparency = 0;
                //pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                for (int i = 0; i < ListGeometry.Count; i++)
                {
                    IPolyline pLine = ListGeometry[i] as IPolyline;
                    if (pLine != null)
                    {

                        ILineElement pPolylineElement = new LineElementClass();
                        (pPolylineElement as IElement).Geometry = pLine as IGeometry;
                        pPolylineElement.Symbol = pLineSymbol;
                        //(pPolylineElement as IElementProperties).Name == "query123";
                        IElementProperties pProperty = pPolylineElement as IElementProperties;
                        pProperty.Name = "RoadLocation";
                        pGra.AddElement(pPolylineElement as IElement, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制范围出错:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pLineSymbol = null;
            }
        }
		//  ZQ    20110803  add
        //private void treeViewXZQ_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
        //    DevComponents.AdvTree.Node pNode = e.Node;
        //    if (pNode.Parent != null)
        //    {
        //        TreeNode pParentNode = pNode.Parent;
        //        switch (pParentNode.Name)
        //        {
        //            case "RoadRoot":
        //            case "RailRoadRoot":
        //            case "RiverRoot":
        //                DealRoadorRiverLocation(pNode);
        //                return;
        //                break;
        //        }
        //    }
        //    try
        //    {
        //        IGraphicsContainer psGra = axMapControl.Map as IGraphicsContainer;
        //        //ZQ   20110803  modify
        //        IGeometry pGeometry = GetXZQGeometry();
        //        //end
        //        if (pGeometry == null)
        //        {
        //            MessageBox.Show("未找到相应行政区", "提示", MessageBoxButtons.OK,
        //                MessageBoxIcon.Information);
        //            return;
        //        }

        //        IEnvelope pExtent = pGeometry.Envelope;
        //        (axMapControl.Map as IActiveView).Extent = pExtent;

        //        //ZQ    20110914    modify   改变显示方式
        //        //drawPolygonElement(pGeometry as IPolygon, psGra);
        //        axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
        //        //end
        //        //ZQ   20110809   modify   先刷新后闪烁问题
        //        axMapControl.ActiveView.ScreenDisplay.UpdateWindow();
        //        //end
        //        axMapControl.FlashShape(pGeometry, 3, 200, null);

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK,
        //                 MessageBoxIcon.Information);
        //    }

        //}
        public void LocationByXZQNode()
        {
            try
            {
                //ZQ  20110804 add
                tabControlInfo.SelectedTab = tabItemMapView;
                tabControl.SelectedTab = tabItemXZQ;
                //end
                IGraphicsContainer psGra = axMapControl.Map as IGraphicsContainer;
                //shduan 20110731 修改
                //ZQ   20110803  modify
                IGeometry pGeometry = GetXZQGeometry();
                //end
                //added by chulili 20110719 错误保护 changed by xisheng 提示未找到 20110813
                if (pGeometry == null)
                {
                    MessageBox.Show("未找到相应行政区", "提示", MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
                    return;
                }

                IEnvelope pExtent = pGeometry.Envelope;
                (axMapControl.Map as IActiveView).Extent = pExtent;
                 //ZQ    20110914    modify   改变显示方式
                //drawPolygonElement(pGeometry as IPolygon, psGra);
                axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
                //end
                //ZQ   20110809   modify   先刷新后闪烁问题
                axMapControl.ActiveView.ScreenDisplay.UpdateWindow();
                //end
                axMapControl.FlashShape(pGeometry, 3, 200, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
            }
        }
        private void toolMenuDw_Click(object sender, EventArgs e)
        {
            LocationByXZQNode();
            //try
            //{
            //    IGraphicsContainer psGra = axMapControl.Map as IGraphicsContainer;
            //    //shduan 20110731 修改
            //    //ZQ   20110803  modify
            //    IGeometry pGeometry = GetXZQGeometry();
            //    //end
            //    //added by chulili 20110719 错误保护
            //    if (pGeometry == null)
            //    {
            //        return;
            //    }

            //    IEnvelope pExtent = pGeometry.Envelope;
            //    (axMapControl.Map as IActiveView).Extent = pExtent;
            //    drawPolygonElement(pGeometry as IPolygon, psGra);
            //    (axMapControl.Map as IActiveView).Refresh();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK,
            //             MessageBoxIcon.Information);
            //}
        }


        public void ExportByXZQNode()
        {
            try
            {
                //ZQ  20110804 add
                tabControlInfo.SelectedTab = tabItemMapView;
                tabControl.SelectedTab = tabItemXZQ;
                //end
                IGraphicsContainer psGra = axMapControl.Map as IGraphicsContainer;
                //shduan 20110731 修改
                //ZQ   20110803  modify
                IGeometry pGeometry = GetXZQGeometry();
                //end
                //added by chulili 20110719 错误保护
                if (pGeometry == null)
                {
                    MessageBox.Show("未找到相应行政区", "提示", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                IList<string> listMapNo = new List<string>();
                IEnvelope pExtent = pGeometry.Envelope;
                (axMapControl.Map as IActiveView).Extent = pExtent;
                //ZQ    20110914    modify   改变显示方式
                //drawPolygonElement(pGeometry as IPolygon, psGra);
                axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
                //end
                //ZQ   20110809   modify   先刷新后闪烁问题
                axMapControl.ActiveView.ScreenDisplay.UpdateWindow();
                //end
                axMapControl.FlashShape(pGeometry, 3, 200, null);
                //这里需要修改NodeKey 的值      ZQ  20110801     Add
                string NodeKey = GetNodeKey("1:50000").ToString();//不同比例尺的接图表的ID号
                IFeatureClass pFeatureClass = ModDBOperate.GetFeatureClassByNodeKey(NodeKey);
                if (pFeatureClass == null) { return; }
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pGeometry;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pFeaCursor = pFeatureClass.Search(pSpatialFilter, false);
                IFeature pFeature = pFeaCursor.NextFeature();
                while (pFeature != null)
                {
                    //ZQ   20110807  modify
                    listMapNo.Add(pFeature.get_Value(pFeature.Table.FindField(GetMapNoField(NodeKey).ToString())).ToString());
                    //end
                    pFeature = pFeaCursor.NextFeature();
                }

                pFeaCursor = null;
                GeoUtilities.Gis.Form.frmExportDataByMapNO pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(listMapNo, true, false, false, false, axMapControl as IMapControlDefault, ModData.v_AppGisUpdate.CurWksInfo.Wks);
                pfrmExportDataByMapNO.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
            }
        }
        private void toolMenuSc_Click(object sender, EventArgs e)
        {
            ExportByXZQNode();
            //try
            //{
            //    IGraphicsContainer psGra = axMapControl.Map as IGraphicsContainer;
            //    //shduan 20110731 修改
            //    //ZQ   20110803  modify
            //    IGeometry pGeometry = GetXZQGeometry();  
            //    //end
            //    //added by chulili 20110719 错误保护
            //    if (pGeometry == null)
            //    {
            //        return;
            //    }
            //    IList<string> listMapNo = new List<string>();
            //    IEnvelope pExtent = pGeometry.Envelope;
            //    (axMapControl.Map as IActiveView).Extent = pExtent;
            //    drawPolygonElement(pGeometry as IPolygon, psGra);
            //    (axMapControl.Map as IActiveView).Refresh();
            //    Exception eError;
            //    //这里需要修改NodeKey 的值      ZQ  20110801     Add
            //    string NodeKey = "c113ac32-14ce-44f6-83b2-c5e0322ef8f9";//不同比例尺的接图表的ID号
            //    IFeatureClass pFeatureClass = GetFeatureClass(NodeKey, _layerTreePath);
            //    if (pFeatureClass == null) { return; }
            //    ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            //    pSpatialFilter.Geometry = pGeometry;
            //    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //    IFeatureCursor pFeaCursor = pFeatureClass.Search(pSpatialFilter, false);
            //    IFeature pFeature = pFeaCursor.NextFeature();
            //    while (pFeature != null)
            //    {
            //        listMapNo.Add(pFeature.get_Value(pFeature.Table.FindField("MAP")).ToString());
            //        pFeature = pFeaCursor.NextFeature();
            //    }

            //    pFeaCursor = null;
            //    GeoUtilities.Gis.Form.frmExportDataByMapNO pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(listMapNo, true, false, false, false, axMapControl as IMapControlDefault, ModData.v_AppGisUpdate.CurWksInfo.Wks);
            //    pfrmExportDataByMapNO.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK,
            //             MessageBoxIcon.Information);
            //}
        }
       public void OutMapByXZQNode()
       {
           //ZQ   20110803  modify
           IGeometry pGeometry = GetXZQGeometry();
           //end
           if (pGeometry == null)
           {
               MessageBox.Show("未找到相应行政区", "提示", MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
               return;
           }
           GeoPageLayout.FrmPageLayout pFrmPageLayout = new GeoPageLayout.FrmPageLayout(axMapControl.Map, pGeometry);
           pFrmPageLayout.ShowDialog();
       }
        private void toolMenuZt_Click(object sender, EventArgs e)
        {
			//ZQ   20110803  modify
            //IGeometry pGeometry = GetXZQGeometry();
            ////end
            //if (pGeometry == null)
            //{
            //    return;
            //}
            //GeoPageLayout.FrmPageLayout pFrmPageLayout = new GeoPageLayout.FrmPageLayout(axMapControl.Map,pGeometry);
            //pFrmPageLayout.ShowDialog();
            OutMapByXZQNode();

        }
		#endregion
        //private void treeViewXZQ_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    treeViewXZQ.SelectedNode.ForeColor = Color.Red;
        //    treeViewXZQ.Refresh();
        //}

        private void treeViewJHTable_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeViewJHTable.SelectedNode.ForeColor = Color.Red;
            treeViewJHTable.Refresh();
        }

        private void TlStripJMP_Click(object sender, EventArgs e)
        {
           //if ((cntxtJMP.Parent as Control).Name == "treeViewJHTable")
            //{
            //    string strScale = "";
            //    string strLayerNm = "";
            //    if (treeViewJHTable.SelectedNode.Level == 2)
            //    {
            //         strScale = treeViewJHTable.SelectedNode.Parent.Text.Substring(2);
            //         strLayerNm = treeViewJHTable.SelectedNode.Parent.Tag.ToString();
            //    }
            //    if (treeViewJHTable.SelectedNode.Level == 3)
            //    {
            //        strScale = treeViewJHTable.SelectedNode.Parent.Parent.Text.Substring(2);
            //        strLayerNm = treeViewJHTable.SelectedNode.Parent.Parent.Tag.ToString();
            //    }

            //    IGraphicsContainer psGra = axMapControl.Map as IGraphicsContainer;
            //    IGeometry pGeometry = getExtentByTFNO(treeViewJHTable.SelectedNode.Text.Trim(), axMapControl.Map,
            //                      Convert.ToInt32(strScale) > 2000,strLayerNm);
            //    if (pGeometry != null)
            //    {
            //        axMapControl.Extent = pGeometry.Envelope;
            //        drawPolygonElement(pGeometry as IPolygon, psGra);
            //    }

              
            //    axMapControl.Refresh();
            //}
            //else
            //{
            //    IGraphicsContainer psGra = axMapControl.Map as IGraphicsContainer;
            //    IGeometry pGeometry = getExtentByXZQ(treeViewXZQ.SelectedNode.Text, axMapControl.Map);
            //    if (pGeometry != null)
            //    {
            //        axMapControl.Extent = pGeometry.Envelope;
            //        drawPolygonElement(pGeometry as IPolygon, psGra);
            //    }            
            //    axMapControl.Refresh(); 
            //}
        }
        //在mapcontrol上画多边形
        private void drawPolygonElement(IPolygon pPolygon, IGraphicsContainer pGra)
        {
            if (pPolygon == null)
                return;

            pGra.DeleteAllElements();
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            IFillShapeElement pPolygonElement = new PolygonElementClass();
            try
            {
                //颜色对象
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                ISymbol pSymbol = (ISymbol)pFillSymbol;
                //pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                pRGBColor.Red = 0;
                pRGBColor.Green = 0;
                pRGBColor.Blue = 255;
                pLineSymbol.Color = pRGBColor;

                pLineSymbol.Width = 2;
                //pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = pLineSymbol;
                pRGBColor.RGB =Color.Transparent.ToArgb();
                pFillSymbol.Color = pRGBColor;
                //pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                (pPolygonElement as IElement).Geometry = pPolygon;
                pPolygonElement.Symbol = pFillSymbol;
                pGra.AddElement(pPolygonElement as IElement, 0);         

            

            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制范围出错:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pFillSymbol = null;
            }
        }

        private void tabItemJHTable_Click(object sender, EventArgs e)
        {
            //treeViewJHTable.Nodes.Clear();
            ////if (treeViewJHTable.Nodes.Count > 0)
            ////    return;
            //InitJHTBTree();//由yjl add in 2011-5-27
            //treeViewJHTable.ExpandAll();         
        }

        private void treeViewJHTable_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeViewJHTable.SelectedNode == null)
                return;
            treeViewJHTable.SelectedNode.ForeColor = Color.Black;
            treeViewJHTable.Refresh();
        }

        //private void treeViewXZQ_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        //{
        //    if (treeViewXZQ.SelectedNode == null)
        //        return;
        //    treeViewXZQ.SelectedNode.ForeColor = Color.Black;
        //    treeViewXZQ.Refresh();
        //}

        private void TlStripCancel_Click(object sender, EventArgs e)
        {
            (axMapControl.Map as IGraphicsContainer).DeleteAllElements();
            axMapControl.Refresh();
        }

        private void treeViewJHTable_MouseDown(object sender, MouseEventArgs e)
        {
            cntxtJMP.Hide();
        }

        //private void treeViewXZQ_MouseDown(object sender, MouseEventArgs e)
        //{
        //    cntxtJMP.Hide();
        //}

        private void tabItemXZQ_Click(object sender, EventArgs e)
        {

        }

        //private void treeViewXZQ_Leave(object sender, EventArgs e)
        //{
        //    cntxtXZQ.Hide();
        //}

        private void treeViewJHTable_Leave(object sender, EventArgs e)
        {
            cntxtJMP.Hide();
        }

        private void TlStripZHT_Click(object sender, EventArgs e)
        {
            //if ((cntxtJMP.Parent as Control).Name == "treeViewJHTable")
            //{
            //    string strScale = "";
            //    string strLayerNm = "";
            //    if (treeViewJHTable.SelectedNode.Level == 2)
            //    {
            //        strScale = treeViewJHTable.SelectedNode.Parent.Text.Substring(2);
            //        strLayerNm = treeViewJHTable.SelectedNode.Parent.Tag.ToString();
            //    }
            //    if (treeViewJHTable.SelectedNode.Level == 3)
            //    {
            //        strScale = treeViewJHTable.SelectedNode.Parent.Parent.Text.Substring(2);
            //        strLayerNm = treeViewJHTable.SelectedNode.Parent.Parent.Tag.ToString();
            //    }

            //    IGraphicsContainer psGra = axMapControl.Map as IGraphicsContainer;
            //    IGeometry pGeometry = getExtentByTFNO(treeViewJHTable.SelectedNode.Text, axMapControl.Map,
            //                      Convert.ToInt32(strScale) > 2000, strLayerNm);
            //    if (pGeometry != null)
            //    {
            //        axMapControl.Extent = pGeometry.Envelope;
            //        drawPolygonElement(pGeometry as IPolygon, psGra);
            //        axMapControl.Refresh();
            //        GeoPageLayout.GeoPageLayout gpl = new GeoPageLayout.GeoPageLayout(axMapControl.Map, treeViewJHTable.SelectedNode.Text, Convert.ToInt32(strScale),null);
            //        gpl.typePageLayout = 3;
            //        gpl.MapOut();
            //        gpl = null;
            //    }
              
                
            //}
            //else
            //{
            //    IGraphicsContainer psGra = axMapControl.Map as IGraphicsContainer;
            //    IGeometry pGeometry = getExtentByXZQ(treeViewXZQ.SelectedNode.Text, axMapControl.Map);
            //    if (pGeometry != null)
            //    {
            //        axMapControl.Extent = pGeometry.Envelope;
            //        drawPolygonElement(pGeometry as IPolygon, psGra);
            //        axMapControl.Refresh();
            //    }
         

            //}
        }

        ////shduan 20110618 add
        //private void treeViewJHTable_DoubleClick(object sender, EventArgs e)
        //{
        //    string strXMLFile = "D:\\01项目\\GuoJiaJu_PSC\\MDxmlData\\M210515_9_ea382cfb-661a-4f07-84fd-db78f847caac.xml";
        //    //加载元数据文件
        //    addXMLFile2GRID(strXMLFile, false);

        //    xmlGridView.DataFilePath = strXMLFile;
        //    xmlGridView.ViewMode = XmlGridView.VIEW_MODE.XML;
        //}


        //private void addXMLFile2GRID(string strXMLFile, Boolean bIsNew)
        //{
        //    this.exgridXML.BeginUpdate();
        //    exontrol.EXGRIDLib.Appearance vAppearance = this.exgridXML.VisualAppearance;
        //    //创建列标题
        //    exontrol.EXGRIDLib.Columns vColumns = this.exgridXML.Columns;
        //    vColumns.Clear();
        //    exontrol.EXGRIDLib.Column vColumn1 = vColumns.Add("元数据项");
        //    vColumn1.Width = 45;
        //    vColumn1.Editor.EditType = exontrol.EXGRIDLib.EditTypeEnum.ReadOnly;
        //    vColumn1.DisplayFilterButton = true;
        //    vColumn1.Alignment = exontrol.EXGRIDLib.AlignmentEnum.LeftAlignment;
        //    vColumn1.FireFormatColumn = true;
        //    vColumn1.LevelKey = "123";

        //    vColumns.Add("元数据值").Editor.EditType = exontrol.EXGRIDLib.EditTypeEnum.SpinType;
        //    exontrol.EXGRIDLib.Column vColumn2 = vColumns.Add("备注");
        //    vColumn2.Editor.EditType = exontrol.EXGRIDLib.EditTypeEnum.ReadOnly;
        //    vColumn2.Width = 5;
        //    exgridXML.LoadXML(strXMLFile);
        //    try
        //    {
        //        //读模板XML
        //        XmlDocument vXMLDoc = new XmlDocument();
        //        vXMLDoc.Load(strXMLFile);

        //        XmlNamespaceManager vXMLNM = new XmlNamespaceManager(vXMLDoc.NameTable);
        //        vXMLNM.AddNamespace("smmd", "http://data.sbsm.gov.cn/smmd/2007");

        //        XmlNode rootNode = vXMLDoc.SelectSingleNode("/smmd:Metadata", vXMLNM);
        //        //if (bIsNew)
        //        //{
        //        //    XmlNode vNodeFileID = vXMLDoc.SelectSingleNode("/smmd:Metadata/smmd:mdFileID", vXMLNM);
        //        //    if (vNodeFileID != null)
        //        //    {
        //        //        vNodeFileID.InnerText   = "";
        //        //    }
        //        //}

        //        exgridXML.Images(imageList1);

        //        //增加Grid行中的项
        //        exontrol.EXGRIDLib.Items var_Items = exgridXML.Items;
        //        string strRootName = getItemName(rootNode.Name);
        //        int iRootIndex = var_Items.InsertItem(0, 0, getCNameFromText(strRootName));
        //        m_iRootItem = iRootIndex;
        //        var_Items.set_CellEditorVisible(iRootIndex, 1, false);
        //        var_Items.set_CellBackColor(iRootIndex, 0, Color.BurlyWood);
        //        var_Items.set_CellBackColor(iRootIndex, 1, Color.BurlyWood);

        //        XmlNodeList vChildNodes = rootNode.ChildNodes;

        //        xml2GridItems(vChildNodes, iRootIndex, bIsNew);


        //        //展开节点
        //        var_Items.set_ExpandItem(iRootIndex, true);
        //        this.exgridXML.EndUpdate();
        //    }
        //    catch
        //    {
        //        MessageBox.Show("元数据文件不符合规范！", "打开元数据文件", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

        //private string getItemName(string strEName)
        //{
        //    string[] strTemp = strEName.Split(new char[1] { ':' });
        //    return strTemp[1];
        //}

        ////从元数据项的英文名称得到中文名称 
        //private  string getCNameFromText(string strEName)
        //{
        //    string strCName = "";
        //    XmlDocument xmlDoc = new XmlDocument();
        //    string strXMLFile = m_MetadataItemPath;
        //    if (File.Exists(strXMLFile))
        //    {
        //        xmlDoc.Load(strXMLFile);
        //        XmlNode vCurNode = xmlDoc.SelectSingleNode("/ZJMetaDataItems/item[@name='" + strEName + "']");
        //        if (vCurNode != null)
        //        {
        //            strCName = vCurNode.Attributes["value"].Value;
        //        }
        //    }
        //    return strCName;
        //}

        ////从元数据项的中文名称,iPramIndex=1得到英文名称;iPramIndex=2得到选择条件;iPramIndex=3得到提示信息;
        //private string getValueFromCName(string strCName, int iPramIndex)
        //{
        //    string strEName = "";
        //    XmlDocument xmlDoc = new XmlDocument();
        //    string strXMLFile = m_MetadataItemPath;
        //    if (File.Exists(strXMLFile))
        //    {
        //        xmlDoc.Load(strXMLFile);
        //        XmlNode vCurNode = xmlDoc.SelectSingleNode("/ZJMetaDataItems/item[@value='" + strCName + "']");
        //        if (vCurNode != null)
        //        {
        //            if (iPramIndex == 1)
        //            {
        //                strEName = vCurNode.Attributes["name"].Value;
        //            }
        //            else if (iPramIndex == 2)
        //            {
        //                strEName = vCurNode.Attributes["optional"].Value;
        //            }
        //            else if (iPramIndex == 3)
        //            {
        //                strEName = vCurNode.Attributes["remark"].Value;
        //            }
        //        }
        //    }
        //    return strEName;
        //}
        ////从字典文件中读取字典项，放到下来框中
        //private int GetDictFromFile(string strItem, int iRow, string strCodeText)
        //{
        //    int iOutIndex = 0;
        //    exontrol.EXGRIDLib.Items var_Items = exgridXML.Items;
        //    exontrol.EXGRIDLib.Editor editor = var_Items.get_CellEditor(iRow, 1);
        //    editor.EditType = exontrol.EXGRIDLib.EditTypeEnum.DropDownListType;
        //    editor.DropDownAutoWidth = exontrol.EXGRIDLib.DropDownWidthType.exDropDownEditorWidth;
        //    XmlDocument xmlDoc = new XmlDocument();
        //    string strXMLFile = m_ResTypePath;
        //    xmlDoc.Load(strXMLFile);
        //    XmlNode vCurNode = xmlDoc.SelectSingleNode("/ZJMetaDataDictionery/ClassType[@xmlnode='" + strItem + "']");
        //    int iIndex = 0;
        //    editor.AddItem(iIndex, " ");
        //    foreach (XmlNode vSubXMLNode in vCurNode.ChildNodes)
        //    {
        //        iIndex++;
        //        string strName = vSubXMLNode.Attributes["value"].Value;
        //        string strID = vSubXMLNode.Attributes["id"].Value;
        //        if (strItem == "resType")
        //        {
        //            editor.AddItem(iIndex, strName);
        //        }
        //        else if (strItem == "tpCat")
        //        {
        //            if (strID.Substring(2, 2) == "00")
        //            {
        //                editor.AddItem(iIndex, strName);
        //            }
        //            else if (strID.Substring(3, 1) == "0")
        //            {
        //                editor.AddItem(iIndex, "     " + strName);
        //            }
        //            else
        //            {
        //                editor.AddItem(iIndex, "          " + strName);
        //            }

        //        }
        //        else if (strItem == "GACateId")
        //        {
        //            if (strID.Length == 2)
        //            {
        //                editor.AddItem(iIndex, strName);
        //            }
        //            else if (strID.Length == 5)
        //            {
        //                editor.AddItem(iIndex, "     " + strName);
        //            }
        //        }
        //        else
        //        {
        //            editor.AddItem(iIndex, strName);
        //        }
        //        if (strName == strCodeText)
        //        {
        //            iOutIndex = iIndex;
        //        }
        //    }
        //    return iOutIndex;
        //}
        ////遍历xml,写到GRID中
        //private void xml2GridItems(XmlNodeList vChildNodes, int h, Boolean bIsNew)
        //{
        //    Dictionary<int, string> vDictItems = new Dictionary<int, string>();
        //    int iIndex = 0;
        //    string strXMLFile = m_ResTypePath;
        //    XmlDocument xmlDoc = new XmlDocument();
        //    if (File.Exists(strXMLFile))
        //    {
        //        xmlDoc.Load(strXMLFile);
        //        XmlNode vRootNode = xmlDoc.SelectSingleNode("/ZJMetaDataDictionery");
        //        if (vRootNode != null)
        //        {
        //            foreach (XmlNode vSubNode in vRootNode.ChildNodes)
        //            {
        //                if (vSubNode.Attributes["xmlnode"].Value != "")
        //                {
        //                    vDictItems.Add(iIndex, vSubNode.Attributes["xmlnode"].Value);
        //                    iIndex++;
        //                }
        //            }
        //        }
        //    }

        //    exontrol.EXGRIDLib.Items var_Items = exgridXML.Items;
        //    foreach (XmlNode vCurNode in vChildNodes)
        //    {
        //        string strCurName = getItemName(vCurNode.Name);
        //        //测试代码
        //        if (strCurName == "resType")
        //        {

        //        }
        //        string strCName = getCNameFromText(strCurName);
        //        if (strCurName == "FeatureCollection")
        //        {
        //            continue;
        //        }


        //        if (strCName == "")
        //        {
        //            strCName = vCurNode.Name;
        //        }

        //        int iRow = var_Items.InsertItem(h, 0, strCName);

        //        //shduan 20101101 将节点对应的路径记录在提示中
        //        string strNodePath = "";
        //        strNodePath = "/" + vCurNode.ParentNode.Name + "/" + vCurNode.Name;
        //        var_Items.set_CellToolTip(iRow, 0, strNodePath);
        //        if (strCurName == "mdFileID")
        //        {
        //            m_iFileIDItem = iRow;
        //            var_Items.set_CellEditorVisible(iRow, 1, false);
        //        }

        //        if (vCurNode.InnerText != "")
        //        {
        //            if (vCurNode.ChildNodes.Count > 1 || vCurNode.ChildNodes[0].HasChildNodes)
        //            {
        //                var_Items.set_CellEditorVisible(iRow, 1, false);
        //                if (getValueFromCName(strCName, 2) == "m")
        //                {
        //                    var_Items.set_CellImages(iRow, 0, "6,5");
        //                    //var_Items.set_CellValue(iRow, 2, "必填");
        //                }
        //                else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
        //                {
        //                    var_Items.set_CellImages(iRow, 0, "6,8");
        //                }
        //                var_Items.set_CellBackColor(iRow, 0, Color.BurlyWood);//Cornsilk
        //                var_Items.set_CellBackColor(iRow, 1, Color.BurlyWood);//Bisque

        //                XmlNodeList vCurNodes = vCurNode.ChildNodes;
        //                xml2GridItems(vCurNodes, iRow, bIsNew);

        //                var_Items.set_ExpandItem(h, true);
        //            }
        //            else
        //            {
        //                string strCurNodeName = getItemName(vCurNode.Name);
        //                //日期
        //                if (strCurNodeName == "mdDateSt" || strCurNodeName == "refDate" || strCurNodeName == "measDateTm" || strCurNodeName == "appDate" || strCurNodeName == "beginning" || strCurNodeName == "ending" || strCurNodeName == "DataNext")
        //                {
        //                    exontrol.EXGRIDLib.Editor vEditor = var_Items.get_CellEditor(iRow, 1);
        //                    vEditor.EditType = exontrol.EXGRIDLib.EditTypeEnum.DateType;
        //                    //vEditor.set_ItemToolTip(0, "aaaaaaaaaaaaaaaa");
        //                    if (bIsNew)
        //                    {
        //                        var_Items.set_CellValue(iRow, 1, "");
        //                    }
        //                    else
        //                    {
        //                        var_Items.set_CellValue(iRow, 1, vCurNode.InnerText);
        //                    }
        //                    if (getValueFromCName(strCName, 2) == "m")
        //                    {
        //                        var_Items.set_CellImages(iRow, 0, "7,5");
        //                        var_Items.set_CellValue(iRow, 2, "必填");
        //                    }
        //                    else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
        //                    {
        //                        var_Items.set_CellImages(iRow, 0, "7,8");
        //                    }
        //                    var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
        //                    var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);
        //                }
        //                else
        //                {
        //                    if (getValueFromCName(strCName, 2) == "m")
        //                    {
        //                        var_Items.set_CellImage(iRow, 0, 5);
        //                        var_Items.set_CellValue(iRow, 2, "必填");
        //                    }
        //                    else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
        //                    {
        //                        var_Items.set_CellImage(iRow, 0, 8);
        //                    }

        //                    //下拉框
        //                    //else if (strCurNodeName == "tpCat" || strCurNodeName == "mdChar" || strCurNodeName == "role" || strCurNodeName == "refDateType" || strCurNodeName == "resType" || strCurNodeName == "idStatus" || strCurNodeName == "govName" || strCurNodeName == "GACateId" || strCurNodeName == "class" || strCurNodeName == "useLimit" || strCurNodeName == "spatRpType" || strCurNodeName == "datum" || strCurNodeName == "vertDatum" || strCurNodeName == "dataChar")
        //                    if (vDictItems.ContainsValue(strCurNodeName))
        //                    {
        //                        int iTextIndex = GetDictFromFile(strCurNodeName, iRow, vCurNode.InnerText);
        //                        var_Items.set_CellValue(iRow, 1, iTextIndex);
        //                        var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
        //                        var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);
        //                    }
        //                    else if (strCurNodeName == "adminCode")
        //                    {
        //                        //string strAdminName = GetAdminFromXMLFile(vCurNode.InnerText, 2);

        //                        var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.EditType;
        //                        var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
        //                        var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);//BurlyWood
        //                        var_Items.set_CellValue(iRow, 1, vCurNode.InnerText);
        //                    }
        //                    else if (strCurNodeName == "mdFileID")
        //                    {
        //                        var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
        //                        var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);//BurlyWood
        //                        if (bIsNew)
        //                        {
        //                            var_Items.set_CellValue(iRow, 1, "");
        //                        }
        //                        else
        //                        {
        //                            var_Items.set_CellValue(iRow, 1, vCurNode.InnerText);
        //                        }
        //                    }
        //                    //可编辑
        //                    else
        //                    {
        //                        var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.EditType;
        //                        var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
        //                        var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);//BurlyWood
        //                        var_Items.set_CellValue(iRow, 1, vCurNode.InnerText);
        //                    }
        //                }

        //                var_Items.set_ExpandItem(h, true);
        //            }
        //        }
        //        else
        //        {
        //            if (vCurNode.ChildNodes.Count > 0)
        //            {
        //                var_Items.set_CellEditorVisible(iRow, 1, false);
        //                if (getValueFromCName(strCName, 2) == "m")
        //                {
        //                    var_Items.set_CellImages(iRow, 0, "6,5");
        //                    //var_Items.set_CellValue(iRow, 2, "必填");
        //                }
        //                else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
        //                {
        //                    var_Items.set_CellImages(iRow, 0, "6,8");
        //                }
        //                var_Items.set_CellBackColor(iRow, 0, Color.BurlyWood);//Cornsilk
        //                var_Items.set_CellBackColor(iRow, 1, Color.BurlyWood);//Bisque
        //                XmlNodeList vCurNodes = vCurNode.ChildNodes;
        //                xml2GridItems(vCurNodes, iRow, bIsNew);

        //                var_Items.set_ExpandItem(h, true);
        //            }
        //            else
        //            {
        //                string strCurNodeName = getItemName(vCurNode.Name);

        //                //日期
        //                if (strCurNodeName == "mdDateSt" || strCurNodeName == "refDate" || strCurNodeName == "measDateTm" || strCurNodeName == "appDate" || strCurNodeName == "beginning" || strCurNodeName == "ending" || strCurNodeName == "DataNext")
        //                {
        //                    var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.DateType;
        //                    var_Items.set_CellValue(iRow, 1, vCurNode.InnerXml);

        //                    if (getValueFromCName(strCName, 2) == "m")
        //                    {
        //                        var_Items.set_CellImages(iRow, 0, "7,5");
        //                        var_Items.set_CellValue(iRow, 2, "必填");
        //                    }
        //                    else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
        //                    {
        //                        var_Items.set_CellImages(iRow, 0, "7,8");
        //                    }
        //                    var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
        //                    var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);
        //                }
        //                else
        //                {
        //                    if (getValueFromCName(strCName, 2) == "m")
        //                    {
        //                        var_Items.set_CellImage(iRow, 0, 5);
        //                        var_Items.set_CellValue(iRow, 2, "必填");
        //                    }
        //                    else if (getValueFromCName(strCName, 2) == "c" || getValueFromCName(strCName, 2) == "o")
        //                    {
        //                        var_Items.set_CellImage(iRow, 0, 8);
        //                    }

        //                    //下拉框
        //                    //else if (strCurNodeName == "tpCat" || strCurNodeName == "mdChar" || strCurNodeName == "role" || strCurNodeName == "refDateType" || strCurNodeName == "resType" || strCurNodeName == "idStatus" || strCurNodeName == "govName" || strCurNodeName == "GACateId" || strCurNodeName == "class" || strCurNodeName == "useLimit" || strCurNodeName == "spatRpType" || strCurNodeName == "datum" || strCurNodeName == "vertDatum" || strCurNodeName == "dataChar")
        //                    if (vDictItems.ContainsValue(strCurNodeName))
        //                    {
        //                        int iTextIndex = GetDictFromFile(strCurNodeName, iRow, vCurNode.InnerText);
        //                        var_Items.set_CellValue(iRow, 1, iTextIndex);
        //                        var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
        //                        var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);
        //                    }
        //                    //可编辑
        //                    else
        //                    {
        //                        var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.EditType;
        //                        var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
        //                        var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);
        //                        var_Items.set_CellValue(iRow, 1, vCurNode.InnerText);
        //                    }
        //                }
        //                var_Items.set_ExpandItem(h, true);
        //            }
        //        }
        //    }
        //}

        private void chkBiXuan_CheckedChanged(object sender, EventArgs e)
        {
            exgridXML.BeginUpdate();
            if (chkBiXuan.Checked )
            {
                exontrol.EXGRIDLib.Columns vColumns = this.exgridXML.Columns;
                if (vColumns.Count == 0) return;
                exontrol.EXGRIDLib.Column vColumn = vColumns[2];
                if (vColumn != null)
                {
                    vColumn.DisplayFilterButton = false;
                    vColumn.FilterOnType = true;
                    vColumn.Filter = "必填";
                    vColumn.FilterType = exontrol.EXGRIDLib.FilterTypeEnum.exPattern;

                    exgridXML.ApplyFilter();
                }
            }
            else
            {
                exgridXML.ClearFilter();
            }
            exgridXML.EndUpdate();
        }
        //折叠节点
        public void CollapseAllNode()
        {
            this.DataViewTree.CollapseAllNode();
        }
        //展开节点
        public void ExpendAllNode()
        {
            this.DataViewTree.ExpendAllNode();
        }
        private void exgridXML_KeyDown(object sender, ref short KeyCode, short Shift)
        {
            if (Shift == 2)
            {
                if (KeyCode == 70)
                {
                    if (!exgridXML.FilterBarPromptVisible)
                    {
                        exgridXML.FilterBarPromptVisible = true;
                        exgridXML.FilterBarCaption = "";
                    }
                }
                else if (KeyCode == 83)
                {
                    //SaveXmlFile("");
                }
                //shduan 20101130
                else if (KeyCode == 78)
                {
                    //frmTypeList frm = new frmTypeList();
                    //frm.ShowDialog();
                    //if (frm.strModelFile != "")
                    //{
                    //    m_strModelFile = frm.strModelFile;
                    //    addXMLFile2GRID(m_strModelFile, true);
                    //    xmlGridView.DataFilePath = m_strModelFile;
                    //    xmlGridView.ViewMode = XmlGridView.VIEW_MODE.XML;
                    //    m_bNewFile = true;
                    //}
                }
            }
        }
        #region   元数据信息查看菜单   ZQ  20110728   add
        /// <summary>
        /// 通过图名获取图层
        /// </summary>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        private ILayer GetLayerByName(string LayerName)
        {
            ILayer pLayer = null;
            if (axMapControl.LayerCount > 0)
            {
                for (int i = 0; i < axMapControl.LayerCount; i++)
                {
                    pLayer = axMapControl.get_Layer(i);
                    if (pLayer.Valid && pLayer.Name == LayerName)
                    {
                        return pLayer;
                    }
                }
            }

            return pLayer = null;
        }
        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        private IWorkspace GetWorkSpacefromConninfo(string conninfostr, int type)
        {
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
        //ZQ  20110820   modify   改成从Oracle数据库中获取元数据信息 
        public void ShowMapSheetInfo()
        {
            try
            {

                TreeNode pSelNode = treeViewJHTable.SelectedNode;//changed by chulili 20110727先获取选中的节点，第二句话会导致选中节点为null
                if (tabControlInfo.SelectedTab != tabMetadataView)
                {
                    tabControlInfo.SelectedTab = tabMetadataView;//  张琪   20110709 
                }
                string strXMLFile = pSelNode.Text;
                string sqlStr = "select t.metadataxml.getclobval() metadataxml from metadata_xml t WHERE 图幅号='" + strXMLFile + "'AND 数据类型='" + pSelNode.Parent.Parent.Text + "'";
                string pOracleConn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + Plugin.Mod.Server + ") (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME="+Plugin.Mod.Database+")));Persist Security Info=True;User Id=" + Plugin.Mod.User + "; Password=" + Plugin.Mod.Password + "";
                OracleConnection con = new OracleConnection();
                DataTable dt = new DataTable();
                con.ConnectionString = pOracleConn;
                if (con.State == ConnectionState.Closed)
                {
                    try
                    {
                        con.Open();
                    }
                    catch
                    {
                        MessageBox.Show("数据库连接失败！", "提示！");
                    }
                }
                OracleDataAdapter da = new OracleDataAdapter(sqlStr, con);
                da.Fill(dt);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                XmlDocument pXmlDocument = new XmlDocument();
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    pXmlDocument.InnerXml = dt.Rows[i]["metadataxml"].ToString();
                }
                DeleteFolder(Application.StartupPath + "\\..\\MDxmlData\\XMLData\\");
                string strXMLPath = Application.StartupPath + "\\..\\MDxmlData\\XMLData\\" + pSelNode.Parent.Parent.Text + "_"+ strXMLFile + ".xml";
                 pXmlDocument.Save(strXMLPath );
                //m_MetadataItemPathtest;
                //加载元数据文件
                 addXMLFile2GRID(strXMLPath, false);

                 xmlGridView.DataFilePath = strXMLPath;
                xmlGridView.ViewMode = XmlGridView.VIEW_MODE.XML;
            }
            catch
            {
            }
        }
        private void TlStripViewMD_Click(object sender, EventArgs e)
        {
            ShowMapSheetInfo();
            //try
            //{

            //    TreeNode pSelNode = treeViewJHTable.SelectedNode;//changed by chulili 20110727先获取选中的节点，第二句话会导致选中节点为null
            //    if (tabControlInfo.SelectedTab != tabMetadataView)
            //    {
            //        tabControlInfo.SelectedTab = tabMetadataView;//  张琪   20110709 
            //    }
            //    string strXMLFile = pSelNode.Text;

            //    List<string> strConn = GetConnData(m_MDconnt);
            //    string SQL = "MapNo='" + strXMLFile + "'" + " and " + "metadatatype='" + pSelNode.Parent.Parent.Text + "'";
            //    List<string> XMLID = GetMapNo(strConn, "MD_table", SQL);
            //    if (strConn[0].ToString() == "1")
            //    {
            //        if (XMLID.Count == 0)
            //        {
            //            return;
            //        }
            //        strXMLFile = System.IO.Path.GetDirectoryName(strConn[1].ToString()) + "\\XMLData\\" + XMLID[2] + ".xml";
            //        //m_MetadataItemPathtest;
            //        //加载元数据文件
            //        addXMLFile2GRID(strXMLFile, false);

            //        xmlGridView.DataFilePath = strXMLFile;
            //        xmlGridView.ViewMode = XmlGridView.VIEW_MODE.XML;
            //    }
            //}
            //catch
            //{
            //}
        }
        /// <summary>
        /// 读取接图表图层.xml来获取图层NodeKey     ZQ   20110805  add
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        private string GetNodeKey(string Number)
        {
            string strNodeKey = "";
            XmlDocument pXmlDocument = new XmlDocument();
            if (!File.Exists(m_layerNodeKey))
            {
                return strNodeKey = "";
            }
            pXmlDocument.Load(m_layerNodeKey);
            XmlNode pxmlnode = null;
            switch (Number)
            {
                case "1:1000000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:1000000']");
                    break;
                case "1:250000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:250000']");
                    break;
                case "1:200000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:200000']");
                    break;
                case "1:100000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:100000']");
                    break;
                case "1:50000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:50000']");
                    break;
                case "1:25000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:25000']");
                    break;
                case "1:10000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:10000']");
                    break;
                case "1:5000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:5000']");
                    break;
                case "1:2000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:2000']");
                    break;
                case "1:1000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:1000']");
                    break;
                case "1:500":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:500']");
                    break;
            }
            if (pxmlnode==null)
            {
                return strNodeKey = "";
            }
            strNodeKey = pxmlnode.Attributes["NodeKey"].Value.ToString();
            return strNodeKey;

        }
        /// <summary>
        /// 根据Nodekey 获取接图表图层图幅号字段名  ZQ  20110807   Add
        /// </summary>
        /// <param name="strNodeKey"></param>
        /// <returns></returns>
        private string GetMapNoField(string strNodeKey)
        {
            string strMapNoField = "";
            XmlDocument pXmlDocument = new XmlDocument();
            if (!File.Exists(m_layerNodeKey))
            {
                return strMapNoField = "";
            }
            pXmlDocument.Load(m_layerNodeKey);
            XmlNode pxmlnode = null;
            pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@NodeKey='" + strNodeKey + "']");
            if (pxmlnode == null)
            {
                return strMapNoField = "";
            }
            strMapNoField = pxmlnode.Attributes["MapNo"].Value.ToString();
            return strMapNoField;
        }
		/// <summary>
        /// ZQ   20110801   通过读xml 文件中属性到系统库中获得要素类
        /// </summary>
        /// <param name="NodeKey"></param>
        /// <param name="_layerTreePath"></param>
        /// <returns></returns>
        private IFeatureClass GetFeatureClass(string NodeKey, string _layerTreePath)
        {
            IFeatureClass pFeatureClass;
            try
            {
                Exception eError;
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(_layerTreePath);
                SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(ModData.v_AppGisUpdate.CurWksInfo.Wks);
                string strSearch = "//Layer[@NodeKey='" + NodeKey.ToString() + "']";
                XmlNode pxmlnode = xmldoc.SelectSingleNode(strSearch);
                XmlElement pxmlele = pxmlnode as XmlElement;
                if (pxmlnode == null)
                {
                    return pFeatureClass = null;
                }
                string strDataSourceID = pxmlele.GetAttribute("ConnectKey");
                string feaclscode = pxmlele.GetAttribute("Code");
                object objDataSource = sysTable.GetFieldValue("DATABASEMD", "DATABASENAME", "ID=" + strDataSourceID, out eError);
                string DataSourcename = "";
                if (objDataSource != null) DataSourcename = objDataSource.ToString();
                string conninfostr = "";
                int type = -1;
                object objconnstr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "ID=" + strDataSourceID, out eError);
                object objType = sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "DATABASENAME='" + DataSourcename + "'", out eError);
                if (objconnstr != null)
                    conninfostr = objconnstr.ToString();
                if (objType != null)
                    type = int.Parse(objType.ToString());
                //连接并获得目录数据库
                IWorkspace pWorkspace = GetWorkSpacefromConninfo(conninfostr, type);
                if (pWorkspace == null)
                {
                    return pFeatureClass = null;
                }
                IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                pFeatureClass = pFeatureWorkspace.OpenFeatureClass(feaclscode);
                return pFeatureClass;
            }
            catch
            {
                return pFeatureClass = null;
            }
        }

        public void ShowMapByMapSheetNode(string  strMapNo)
        {
            try
            {
                string NodeKey = "";
                if (strMapNo.Length == 3)//1:100万 接图表NodeKey
                {
                    NodeKey = GetNodeKey("1:1000000").ToString();
                }
                else
                {
                    switch (strMapNo.ToString().Substring(3, 1).ToUpper())
                    {
                        case "C"://1:25万 接图表NodeKey
                            NodeKey = GetNodeKey("1:250000").ToString();
                            break;
                        case "B"://1:50万 接图表NodeKey
                            NodeKey = GetNodeKey("1:500000").ToString();//不同比例尺的接图表的ID号
                            break;
                        case "D"://1:10万 接图表NodeKey
                            NodeKey = GetNodeKey("1:100000").ToString();
                            break;
                        case "E"://1:5万 接图表NodeKey
                            NodeKey = GetNodeKey("1:50000").ToString();//不同比例尺的接图表的ID号
                            break;
                        case "F"://1:2.5万 接图表NodeKey
                            NodeKey = GetNodeKey("1:25000").ToString();
                            break;
                        case "G"://1:1万 接图表NodeKey
                            NodeKey = GetNodeKey("1:10000").ToString();//不同比例尺的接图表的ID号
                            break;
                        case "H"://1:5千 接图表NodeKey
                            NodeKey = GetNodeKey("1:5000").ToString();
                            break;
                        case "I"://1:2千 接图表NodeKey
                            NodeKey = GetNodeKey("1:2000").ToString();//不同比例尺的接图表的ID号
                            break;
                        case "M"://1:1千 接图表NodeKey
                            NodeKey = GetNodeKey("1:1000").ToString();
                            break;
                        case "N"://1:5百 接图表NodeKey
                            NodeKey = GetNodeKey("1:500").ToString();//不同比例尺的接图表的ID号
                            break;
                    }
                }
                if (NodeKey=="")
                {
                    return;
                }
                IFeatureClass pFeatureClass = ModDBOperate.GetFeatureClassByNodeKey(NodeKey);

                //ILayer pLayer = GetLayerByName("全国50K范围图");
                if (pFeatureClass == null) { return; }

                tabControlInfo.SelectedTab = tabItemMapView;
                tabControl.SelectedTab = tabItemJHTable;

                //IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                //IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
                IQueryFilter pQueryFilter = new QueryFilterClass();
                IFeature pFeature = null;
                IFeatureCursor pFeatureCursor = null;
                //ZQ 20110807   add
                ITable pTable = pFeatureClass as ITable;
                IFields pFields = pTable.Fields;
                IField pField =pFields.get_Field( pFields.FindField(GetMapNoField(NodeKey)));
                if (pField.Type.ToString() == "esriFieldTypeString")
                {

                    pQueryFilter.WhereClause = GetMapNoField(NodeKey).ToString() + "='" + strMapNo.ToString().ToUpper()+ "'";
                }
                else if (pField.Type.ToString() == "esriFieldTypeDouble")
                {
                    pQueryFilter.WhereClause = GetMapNoField(NodeKey).ToString() + "=" + strMapNo.ToString().ToUpper();
                }
                //end
                pFeatureCursor = pFeatureClass.Search(pQueryFilter, false);
                pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    //先刷新，后闪烁问题
                    axMapControl.Extent = pFeature.Extent;
                    //ZQ 20110809  modify 
                    axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
                    //end
                    //ZQ   20110809   modify   先刷新后闪烁问题
                    axMapControl.ActiveView.ScreenDisplay.UpdateWindow();
                    //end
                    axMapControl.FlashShape(pFeature.ShapeCopy, 3, 200, null);
                    pFeature = pFeatureCursor.NextFeature();
                }
                pFeatureCursor = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);

            }
            catch { }
        }
        public string GettreView()
        {
            string strMapNo = treeViewJHTable.SelectedNode.Text;
            return strMapNo;
        }
       //根据图幅号定位到图形信息    ZQ  20110728  add      20110730  modify
        private void TlStripViewMap_Click(object sender, EventArgs e)
        {
            string strMapNo = treeViewJHTable.SelectedNode.Text;
            ShowMapByMapSheetNode(strMapNo);
            //try
            //{
            //    string strMapNo = treeViewJHTable.SelectedNode.Text;
              
            //    string NodeKey ="";
            //     if (strMapNo.Length == 3)//1:100万 接图表NodeKey
            //    {
            //        NodeKey = "61073218-927f-4eba-a245-e6e59a121a5e";
            //    }
            //    else
            //    {
            //        switch (strMapNo.ToString().Substring(3, 1).ToUpper())
            //        {
            //            case "C"://1:25万 接图表NodeKey
            //                NodeKey = "610we218-927f-4eba-a245-e6e59a121a5e";
            //                break;
            //            case "E"://1:5万 接图表NodeKey
            //                NodeKey = "c113ac32-14ce-44f6-83b2-c5e0322ef8f9";//不同比例尺的接图表的ID号
            //                break;
            //        }
            //    }

            //    IFeatureClass pFeatureClass = GetFeatureClass(NodeKey, _layerTreePath);
               
            //    //ILayer pLayer = GetLayerByName("全国50K范围图");
            //    if (pFeatureClass == null){ return; }
              
            //    tabControlInfo.SelectedTab = tabItemMapView;
            //    tabControl.SelectedTab = tabItemJHTable;
             
            //    //IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
            //    //IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            //    IQueryFilter pQueryFilter = new QueryFilterClass();
            //    IFeature pFeature = null;
            //    IFeatureCursor pFeatureCursor = null;
            //    pQueryFilter.WhereClause = "MAP='" + strMapNo.ToString() + "'";//需要根据具体的图层字段名进行更改
            //    pFeatureCursor = pFeatureClass.Search(pQueryFilter, false);
            //    pFeature = pFeatureCursor.NextFeature();
            //    while (pFeature != null)
            //    {

            //        axMapControl.Extent = pFeature.Extent;
            //        axMapControl.ActiveView.Refresh();
            //        axMapControl.FlashShape(pFeature.ShapeCopy, 3, 200, null);
            //        pFeature = pFeatureCursor.NextFeature();
            //    }
            //    pFeatureCursor = null;
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
         
            //}
            //catch { }
        }
        public void ExportByMapSheetNode()
        {
            try
            {
                IList<string> strMapNo = new List<string>();
                strMapNo.Add(treeViewJHTable.SelectedNode.Text);
                //GeoUtilities.Gis.Form.frmExportDataByMapNO pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(strMapNo);
                string strDataType = treeViewJHTable.SelectedNode.Parent.Parent.Text;
                GeoUtilities.Gis.Form.frmExportDataByMapNO pfrmExportDataByMapNO = null;
                //ZQ   20110801   modify   
                switch (strDataType.ToUpper())
                {
                    case "DLG":
                        pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(strMapNo, true, true, false, false, axMapControl as IMapControlDefault, ModData.v_AppGisUpdate.CurWksInfo.Wks);
                        break;
                    case "DEM":
                        pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(strMapNo, true, false, true, false, axMapControl as IMapControlDefault, ModData.v_AppGisUpdate.CurWksInfo.Wks);
                        break;
                    case "DOM":
                        pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(strMapNo, true, false, false, true, axMapControl as IMapControlDefault, ModData.v_AppGisUpdate.CurWksInfo.Wks);
                        break;
                }
                //end
                pfrmExportDataByMapNO.ShowDialog();
            }
            catch { }
        }
        //根据图幅号输出图形  ZQ 20110728  add
        private void TlStripExportMap_Click(object sender, EventArgs e)
        {
            ExportByMapSheetNode();
            //try
            //{
            //    IList<string> strMapNo = new List<string>();
            //    strMapNo.Add(treeViewJHTable.SelectedNode.Text);
            //    //GeoUtilities.Gis.Form.frmExportDataByMapNO pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(strMapNo);
            //    string strDataType = treeViewJHTable.SelectedNode.Parent.Parent.Text;
            //    GeoUtilities.Gis.Form.frmExportDataByMapNO pfrmExportDataByMapNO = null;
            //    //ZQ   20110801   modify   
            //    switch (strDataType.ToUpper())
            //    {
            //        case "DLG":
            //            pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(strMapNo, true, true, false, false, axMapControl as IMapControlDefault, ModData.v_AppGisUpdate.CurWksInfo.Wks);
            //            break;
            //        case "DEM":
            //            pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(strMapNo, true, false, true, false, axMapControl as IMapControlDefault, ModData.v_AppGisUpdate.CurWksInfo.Wks);
            //            break;
            //        case "DOM":
            //            pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(strMapNo, true, false, false, true, axMapControl as IMapControlDefault, ModData.v_AppGisUpdate.CurWksInfo.Wks);
            //            break;
            //    }
            //    //end
            //    pfrmExportDataByMapNO.ShowDialog();
            //}
            //catch { }
        }
        #endregion
        //shduan 20110618 add
        private void btnMetadataQuery_Click(object sender, EventArgs e)
        {
           
            queryMDFromFile();
        }
        // ZQ 20110817  add  
        //private void bttSQL_Click(object sender, EventArgs e)
        //{
        //    frmMetaDataSQL pfrmMetaDataSQL = new frmMetaDataSQL(strMeataDataSQL,this);
        //    pfrmMetaDataSQL.ShowDialog();
        //}
       
        
        // ZQ  20110729   modify    ZQ 20110820   modify   全文检索
        private void queryMDFromFile()
        {
            try
            {   
            
                treeViewJHTable.Nodes.Clear();
                treeViewJHTable.Nodes.Add("元数据查询结果", "元数据查询结果", 16, 16);
                TreeNode tNode1 = treeViewJHTable.Nodes[0];
                //ZQ  20110805
                //ShowMapByMapSheetNode(txtMetadataQuery.Text);
                //end
                string pOracleConn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + Plugin.Mod.Server + ") (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=" + Plugin.Mod.Database+ ")));Persist Security Info=True;User Id=" + Plugin.Mod.User + "; Password=" + Plugin.Mod.Password + "";
                OracleConnection con = new OracleConnection();
                con.ConnectionString = pOracleConn;
                if (con.State == ConnectionState.Closed)
                {
                    try
                    {
                        con.Open();
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show("数据库连接失败！","提示！");
                    }
                }
                 string sqlStr="";
                  if (ModStringpro.IsChina(txtMetadataQuery.Text))//判断是否包含中文字符  ZQ 20110820    add
                {
                    sqlStr = "SELECT 图幅号,数据类型 FROM MetaData_XML  WHERE CONTAINS(MetaDataXML,'" + ModStringpro.GetSQLChina(txtMetadataQuery.Text.Trim()) + "')>0";
                }
               //else  if(ModStringpro.IsMathEng(txtMetadataQuery.Text))
                //{
                //    sqlStr = "SELECT 图幅号,数据类型 FROM MetaData_XML  WHERE CONTAINS(MetaDataXML,'" + ModStringpro.GetSQLMathEng(txtMetadataQuery.Text.Trim()) + "')>0";
                //}
              
                else
                {
                    sqlStr = "SELECT 图幅号,数据类型 FROM MetaData_XML  WHERE CONTAINS(MetaDataXML,'" + ModStringpro.GetSQLMathEng(txtMetadataQuery.Text.Trim()) + "')>0";
                }
                OracleDataAdapter da = new OracleDataAdapter(sqlStr, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                //ZQ 20110822    对查询的结果进行排序
                DataRow[] pDataRow = dt.Select("", "图幅号  ASC");//对图幅号进行升序排列
                DataTable tmptable = dt.Clone();
                tmptable.Clear();
                foreach(DataRow row in pDataRow)
                {
                    tmptable.ImportRow(row);
                }
                //  end
                for (int i = 0; i < tmptable.Rows.Count; i++)
                {
                    try
                    {
                        List<string> strMapNo = new List<string>();
                        strMapNo.Add(tmptable.Rows[i]["数据类型"].ToString());
                        strMapNo.Add(tmptable.Rows[i]["图幅号"].ToString());
                        if (tNode1.Nodes.Count == 0)
                        {
                            TreeNode tNode2 = tNode1.Nodes.Add(strMapNo[0], strMapNo[0], 16, 16);
                            TreeNode tNode3 = tNode2.Nodes.Add(strMapNo[1].ToString().Substring(0, 3), strMapNo[1].ToString().Substring(0, 3), 16, 16);
                            tNode1.Expand();
                            tNode2.Expand();
                            tNode3.Nodes.Add(strMapNo[1].ToString());
                            //break;
                        }
                        else
                        {
                            int flog1 = 0;
                            for (int j = 0; j < tNode1.Nodes.Count; j++)
                            {

                                TreeNode tNode2 = tNode1.Nodes[j];
                                if (tNode2.Name == strMapNo[0].ToString())
                                {
                                    int flog2 = 0;
                                    for (int m = 0; m < tNode2.Nodes.Count; m++)
                                    {
                                        TreeNode tNode3 = tNode2.Nodes[m];

                                        if (tNode3.Name == strMapNo[1].ToString().Substring(0, 3))
                                        {
                                            int flog3 = 0;
                                            for (int n = 0; n < tNode3.Nodes.Count; n++)
                                            {
                                                TreeNode tNode4 = tNode3.Nodes[n];
                                                if (tNode4.Text == strMapNo[1].ToString())
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    flog3 = flog3 + 1;
                                                    continue;
                                                }

                                            }
                                            if (flog3 == tNode3.Nodes.Count)
                                            {
                                                tNode3.Nodes.Add(strMapNo[1].ToString());
                                            }
                                        }
                                        else
                                        {
                                            flog2 = flog2 + 1;
                                            continue;
                                        }
                                    }
                                    if (flog2 == tNode2.Nodes.Count)
                                    {
                                        TreeNode tNode13 = tNode2.Nodes.Add(strMapNo[1].ToString().Substring(0, 3), strMapNo[1].ToString().Substring(0, 3), 16, 16);
                                        tNode2.Expand();
                                        tNode13.Nodes.Add(strMapNo[1].ToString());
                                        //break;
                                    }
                                }
                                else
                                {
                                    flog1 = flog1 + 1;
                                    continue;
                                }
                            }
                            if (flog1 == tNode1.Nodes.Count)
                            {
                                TreeNode tNode12 = tNode1.Nodes.Add(strMapNo[0], strMapNo[0], 16, 16);
                                TreeNode tNode3 = tNode12.Nodes.Add(strMapNo[1].ToString().Substring(0, 3), strMapNo[1].ToString().Substring(0, 3), 16, 16);
                                tNode1.Expand();
                                tNode12.Expand();
                                tNode3.Nodes.Add(strMapNo[1].ToString());
                                //break;
                            }
                        }
                    }

                    catch
                    { }
                }
            }
            catch
            {
            }
        }
        private void txtMetadataQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                queryMDFromFile();
            }
        }

        //added by chulili 20110728 打开地图文档的后续处理
        public void OpenMxdDocDeal()
        {
            this.DataViewTree.OpenMxdDocDeal();
        }
        //shduan 20110618 add11
        private void treeViewJHTable_DoubleClick(object sender, EventArgs e)
        {
            ShowMapSheetInfo();
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
                    MessageBox.Show("没有查找到元数据详细信息,请确认数据.","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Information );
                    return;
                }
                //end
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

        private string getItemName(string strEName)
        {
            string[] strTemp = strEName.Split(new char[1] { ':' });
            return strTemp[1];
        }

        //从元数据项的英文名称得到中文名称 
        private  string getCNameFromText(string strEName)
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
                            vEditor.EditType = exontrol.EXGRIDLib.EditTypeEnum.DateType;
                            //vEditor.set_ItemToolTip(0, "aaaaaaaaaaaaaaaa");
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

                            //下拉框
                            //else if (strCurNodeName == "tpCat" || strCurNodeName == "mdChar" || strCurNodeName == "role" || strCurNodeName == "refDateType" || strCurNodeName == "resType" || strCurNodeName == "idStatus" || strCurNodeName == "govName" || strCurNodeName == "GACateId" || strCurNodeName == "class" || strCurNodeName == "useLimit" || strCurNodeName == "spatRpType" || strCurNodeName == "datum" || strCurNodeName == "vertDatum" || strCurNodeName == "dataChar")
                            if (vDictItems.ContainsValue(strCurNodeName))
                            {
                                int iTextIndex = GetDictFromFile(strCurNodeName, iRow, vCurNode.InnerText);
                                var_Items.set_CellValue(iRow, 1, iTextIndex);
                                var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
                                var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);
                            }
                            //else if (strCurNodeName == "adminCode")
                            //{
                            //    //string strAdminName = GetAdminFromXMLFile(vCurNode.InnerText, 2);

                            //    var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.EditType;
                            //    var_Items.set_CellBackColor(iRow, 0, Color.LightGray);
                            //    var_Items.set_CellBackColor(iRow, 1, Color.AliceBlue);//BurlyWood
                            //    var_Items.set_CellValue(iRow, 1, vCurNode.InnerText);
                            //}
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
                                var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.EditType;
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
                            var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.DateType;
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
                                var_Items.get_CellEditor(iRow, 1).EditType = exontrol.EXGRIDLib.EditTypeEnum.EditType;
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

        //private void chkBiXuan_CheckedChanged(object sender, EventArgs e)
        //{
        //    exgridXML.BeginUpdate();
        //    if (chkBiXuan.Checked )
        //    {
        //        exontrol.EXGRIDLib.Columns vColumns = this.exgridXML.Columns;

        //        exontrol.EXGRIDLib.Column vColumn = vColumns[2];
        //        if (vColumn != null)
        //        {
        //            vColumn.DisplayFilterButton = false;
        //            vColumn.FilterOnType = true;
        //            vColumn.Filter = "必填";
        //            vColumn.FilterType = exontrol.EXGRIDLib.FilterTypeEnum.exPattern;

        //            exgridXML.ApplyFilter();
        //        }
        //    }
        //    else
        //    {
        //        exgridXML.ClearFilter();
        //    }
        //    exgridXML.EndUpdate();
        //}

        //private void exgridXML_KeyDown(object sender, ref short KeyCode, short Shift)
        //{
        //    if (Shift == 2)
        //    {
        //        if (KeyCode == 70)
        //        {
        //            if (!exgridXML.FilterBarPromptVisible)
        //            {
        //                exgridXML.FilterBarPromptVisible = true;
        //                exgridXML.FilterBarCaption = "";
        //            }
        //        }
        //        else if (KeyCode == 83)
        //        {
        //            //SaveXmlFile("");
        //        }
        //        //shduan 20101130
        //        else if (KeyCode == 78)
        //        {
        //            //frmTypeList frm = new frmTypeList();
        //            //frm.ShowDialog();
        //            //if (frm.strModelFile != "")
        //            //{
        //            //    m_strModelFile = frm.strModelFile;
        //            //    addXMLFile2GRID(m_strModelFile, true);
        //            //    xmlGridView.DataFilePath = m_strModelFile;
        //            //    xmlGridView.ViewMode = XmlGridView.VIEW_MODE.XML;
        //            //    m_bNewFile = true;
        //            //}
        //        }
        //    }
        //}

        //private void TlStripViewMD_Click(object sender, EventArgs e)
        //{
        //    string strXMLFile = "D:\\01项目\\GuoJiaJu_PSC\\MDxmlData\\M210515_9_ea382cfb-661a-4f07-84fd-db78f847caac.xml";
        //    //加载元数据文件
        //    addXMLFile2GRID(strXMLFile, false);

        //    xmlGridView.DataFilePath = strXMLFile;
        //    xmlGridView.ViewMode = XmlGridView.VIEW_MODE.XML;
        //}
        public void RemoveLayer()
        {
            this.DataViewTree.Removelayer();
        }
        ////added by chulili 20110728 打开地图文档的后续处理
        //public void OpenMxdDocDeal()
        //{
        //    this.DataViewTree.OpenMxdDocDeal();
        //}
        private void UserControlSMPD_EnabledChanged(object sender, EventArgs e)
        {
            if (this.Enabled == false)
            {
                FormCollection pFC = Application.OpenForms;

                foreach (Form pFm in pFC)
                {
                    if (pFm.Name.Equals("FrmEagleEye") && pFm.Text.Equals("鹰眼"))//判断鹰眼是否打开
                    {
                        if (pFm.Visible)
                        {
                            pFm.Visible = false ;
                        }

                    }
                }
                // axMapControl.Map.ClearLayers();xisheng delete 20110803
            }
            else
            {
                //初始化图层视图
                Plugin.CProgress vProgress = new Plugin.CProgress("数据展示管理");
                vProgress.EnableCancel = false;
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("刷新图层列表");

                if (SysCommon.ModSysSetting.IsConfigLayerTreeChanged)
                {
                    axMapControl.Map.ClearLayers();
                    try
                    {
                        RefreshDataView();
                    }
                    catch
                    { }
                    SysCommon.ModSysSetting.IsConfigLayerTreeChanged = false;
                }
                //try xisheng delete 20110803
                //{
                //    RefreshDataView();
                //}
                //catch
                //{ }
                vProgress.Close();
                //处理鹰眼
                FormCollection pFC = Application.OpenForms;

                foreach (Form pFm in pFC)
                {
                    if (pFm.Name.Equals("FrmEagleEye") && pFm.Text.Equals("鹰眼"))//判断鹰眼是否打开
                    {
                        if (pFm.Visible==false  )
                        {
                            pFm.Visible = true;
                        }

                    }
                }

                
            }
        }

        //当鼠标移动，则隐藏当前内容  
        private void axMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            TempX = double.Parse(e.mapX.ToString("0.00"));
            TempY = double.Parse(e.mapY.ToString("0.00"));
            //ModData.v_AppGisUpdate.CoorXY = "X:" + TempX + "   Y:" + TempY;
            //cyf 20110615 modify:更改坐标信息
            ModData.v_AppGisUpdate.OperatorTips = "X:" + TempX + "   Y:" + TempY;
            //end
        }
        private void GetScaleDecimal()
        {
            int intDecimal;
            Plugin.ModScale.GetScaleConfig(out intDecimal);
            _Decimalstr = "0.";
            for (int i = 0; i < intDecimal; i++)
            {
                _Decimalstr = _Decimalstr + "0";
            }
            if (_Decimalstr.EndsWith("."))
            {
                _Decimalstr = _Decimalstr.Substring(0, _Decimalstr.Length - 1);
            }
        }
        private void axMapControl_OnAfterDraw(object sender, IMapControlEvents2_OnAfterDrawEvent e)
        {
            ModData.v_AppGisUpdate.RefScaleCmb.ControlText = (sender as AxMapControl).Map.ReferenceScale.ToString().Trim();
            if (_Decimalstr == "")
            {
                _Decimalstr = "0.00";
            }
            double CurScale = double.Parse((sender as AxMapControl).Map.MapScale.ToString(_Decimalstr));
            ModData.v_AppGisUpdate.CurScaleCmb.ControlText = CurScale.ToString();
            ModData.v_AppGisUpdate.CurScaleCmb.Tooltip = CurScale.ToString(); 

        }
        public void AddDataDir(string strNodeKey)
        {
            this.DataViewTree.AddDataDir(strNodeKey);
        }
        //yjl20110915 add
        public string  MapSheet
        {
            get
            {
                TreeNode pSelNode = treeViewJHTable.SelectedNode;
                if (pSelNode == null)
                    return null;
                return pSelNode.Text;
            }
          

            //string xmlPath = Application.StartupPath + "\\..\\res\\xml\\展示图层树.xml";
            //if (!File.Exists(xmlPath))
            //{
            //    return;
            //}
            //XmlDocument dataTree = new XmlDocument();
            //dataTree.Load(xmlPath);
            //string xPath = "//DIR[contains(@NodeText,'dlg')]";
            //XmlNodeList tdlyXLst = dataTree.SelectNodes(xPath);
            //if (tdlyXLst == null)
            //    return;
           
        }
        //yjl20110915 add
        public DevComponents.AdvTree.AdvTree DataTree
        {
            get { return DataViewTree.DataTree; }
        }
        public DevComponents.AdvTree.AdvTree XZQTree
        {
            get { return treeViewXZQ; }
        }
        public IGeometry GetXZQ
        {
            get { return GetXZQGeometry(); }
        }
        public string GetXZQMC
        {
            get { return treeViewXZQ.SelectedNode.Text; }
        }
        //yjl20110920 add
        public IFeatureClass XZQ_XIAN
        {
            get 
            {
                   return getXZQFC(treeViewXZQ.SelectedNode); 
            }
        }
        //yjl20110924 add
        private IFeatureClass getXZQFC(DevComponents.AdvTree.Node vSelectNode)
        {
            if (vSelectNode.Parent != null)
            {
                DevComponents.AdvTree.Node vParentNode = vSelectNode.Parent;
                if (vParentNode.DataKey != null)
                {
                    XmlNode pNode = vParentNode.DataKey as XmlNode;
                    if ((pNode as XmlElement) != null)
                    {
                        XmlElement pNodeEle = pNode as XmlElement;
                        if (pNodeEle.HasAttribute("NodeKey") && pNodeEle.HasAttribute("FieldXZBM"))
                        {
                            string strNodeKey = pNodeEle.GetAttribute("NodeKey");
                            string strField = pNodeEle.GetAttribute("FieldXZBM");
                            IFeatureClass pFeatureClass = ModDBOperate.GetFeatureClassByNodeKey(strNodeKey);
                            return pFeatureClass;
                        }
                    }
                }
            }
            return null;
        }
        public string GetXZQ_XIAN_Field
        {
            get { return strXZQFieldGX; }
        }

        private void treeViewXZQ_AfterNodeSelect(object sender, DevComponents.AdvTree.AdvTreeNodeEventArgs e)
        {
            //treeViewXZQ.SelectedNode = Color.Red;
            //treeViewXZQ.Refresh();
        }

        private void treeViewXZQ_BeforeNodeSelect(object sender, DevComponents.AdvTree.AdvTreeNodeCancelEventArgs e)
        {
            //if (treeViewXZQ.SelectedNode == null)
            //    return;
            //treeViewXZQ.SelectedNode.ForeColor = Color.Black;
            //treeViewXZQ.Refresh();
        }

        private void treeViewXZQ_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            //ZQ    20110731   modify
            cntxtXZQ.Hide();
            if (e.Button == MouseButtons.Left)
            {
                treeViewXZQ.SelectedNode = e.Node;

            }

            //右键点击的时候弹出右键菜单
            if (e.Button == MouseButtons.Right)
            {
                treeViewXZQ.SelectedNode = e.Node;
                //System.Drawing.Point ClickPoint = treeViewXZQ.PointToScreen(new System.Drawing.Point(e.X, e.Y));
                DevComponents.AdvTree.Node tSelNode;
                tSelNode = e.Node;
                //if (tSelNode != null&&tSelNode.Level==1)
                //{
                //    cntxtJMP.TopLevel = false;
                //    cntxtJMP.Parent = treeViewXZQ as Control;
                //    cntxtJMP.Show(e.Location);
                //}//ZQ    20110731   modify
                //yjl20110927 modify 右键菜单条件层次从0开始
                if (tSelNode != null && tSelNode.Level >= 0 && tSelNode.Tag != null)
                {
                    if (tSelNode.Tag.ToString() == "Province" || tSelNode.Tag.ToString() == "City" || tSelNode.Tag.ToString() == "County" || tSelNode.Tag.ToString() == "XZQTree"
                    {
                        //cntxtXZQ.TopLevel = false;
                        //cntxtXZQ.Parent = treeViewXZQ as Control;
                        //cntxtXZQ.Show(e.Location);
                        System.Drawing.Point pPoint = new System.Drawing.Point(e.X, e.Y);
                        DevComponents.DotNetBar.ButtonItem item = null;
                        DevComponents.DotNetBar.ContextMenuBar menuBar = _dicContextMenu["ContextMenuXZQ"];
                        this.Controls.Add(menuBar);
                        if (_dicContextMenu.ContainsKey("ContextMenuXZQ"))
                        {
                            if (_dicContextMenu["ContextMenuXZQ"] != null)
                            {
                                if (_dicContextMenu["ContextMenuXZQ"].Items.Count > 0)
                                {
                                    item = _dicContextMenu["ContextMenuXZQ"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                                    if (item != null)
                                    {
                                        item.Popup(treeViewXZQ.PointToScreen(pPoint));
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        private void treeViewXZQ_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            DevComponents.AdvTree.Node pNode = e.Node;
            if (pNode.Parent != null)
            {
                DevComponents.AdvTree.Node pParentNode = pNode.Parent;
                switch (pParentNode.Name)
                {
                    case "RoadRoot":
                    case "RailRoadRoot":
                    case "RiverRoot":
                        DealRoadorRiverLocation(pNode);
                        return;
                        break;
                }
            }
            try
            {
                IGraphicsContainer psGra = axMapControl.Map as IGraphicsContainer;
                //ZQ   20110803  modify
                IGeometry pGeometry = GetXZQGeometry();
                //end
                if (pGeometry == null)
                {
                    MessageBox.Show("未找到相应行政区", "提示", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                IEnvelope pExtent = pGeometry.Envelope;
                (axMapControl.Map as IActiveView).Extent = pExtent;

                //ZQ    20110914    modify   改变显示方式
                //drawPolygonElement(pGeometry as IPolygon, psGra);
                axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
                //end
                //ZQ   20110809   modify   先刷新后闪烁问题
                axMapControl.ActiveView.ScreenDisplay.UpdateWindow();
                //end
                axMapControl.FlashShape(pGeometry, 3, 200, null);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
            }
        }

        private void advTreeResultFileList_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.advTreeResultFileList.SelectedNode = e.Node;
                DevComponents.AdvTree.Node tSelNode;
                tSelNode = e.Node;

                if (tSelNode != null)
                {
                    System.Drawing.Point pPoint = new System.Drawing.Point(e.X, e.Y);
                    DevComponents.DotNetBar.ButtonItem item = null;
                    DevComponents.DotNetBar.ContextMenuBar menuBar = _dicContextMenu["ContextMenuResultFile"];
                    if (menuBar!=null) this.Controls.Add(menuBar);
                    if (_dicContextMenu.ContainsKey("ContextMenuResultFile"))
                    {
                        if (_dicContextMenu["ContextMenuResultFile"] != null)
                        {
                            if (_dicContextMenu["ContextMenuResultFile"].Items.Count > 0)
                            {
                                item = _dicContextMenu["ContextMenuResultFile"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                                if (item != null)
                                {
                                    item.Popup(advTreeResultFileList.PointToScreen(pPoint));
                                }
                            }
                        }
                    }
                }

            }
        }

        private void tabControl_Click(object sender, EventArgs e)
        {

        }

    }

}
