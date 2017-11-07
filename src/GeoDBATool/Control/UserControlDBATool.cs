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
using System.IO;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDBATool
{
    public partial class UserControlDBATool : UserControl
    {
        public frmBarManager _frmBarManager;
        
        //右键菜单集合
        private Dictionary<string, DevComponents.DotNetBar.ContextMenuBar> _dicContextMenu;

        //地图浏览工具栏容器
        private Control _MapToolControl;
        public Control ToolControl
        {
            get { return _MapToolControl; }
        }

        private DevComponents.DotNetBar.Bar _MapToolBar;
        public DevComponents.DotNetBar.Bar MapToolBar
        {
            get { return _MapToolBar; }
        }

        private SysCommon.BottomQueryBar _BottomQueryBar; //added by chulili 2012-10-16 入库更新系统添加查询结果状态栏

        private bool m_bGroupLayerVisible;   //控制解决GroupLayer显示问题(方法有待改进 )

        System.Drawing.Point m_Point = new System.Drawing.Point();//定义点，用来捕捉鼠标移动点坐标

        //初始化窗体类
        public UserControlDBATool(string strName, string strCation)
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

            ModData.v_AppGIS.MainUserControl = this;
            ModData.v_AppGIS.TOCControl = axTOCControl.Object as ITOCControlDefault;
            ModData.v_AppGIS.MapControl = axMapControl.Object as IMapControlDefault;
            ModData.v_AppGIS.ArcGisMapControl = axMapControl;
            advTreeProject.ImageList = IconContainer;
            ModData.v_AppGIS.ProjectTree = advTreeProject;
            ModData.v_AppGIS.DataTree = advTree;
            ModData.v_AppGIS.UpdateGrid = dgvUpdate;
            ModData.v_AppGIS.PolylineSearchGrid = this.PolylineDataGrid;
            ModData.v_AppGIS.PolygonSearchGrid = this.PolygonDataGrid;
            ModData.v_AppGIS.JoinMergeResultGrid = this.JoinMergeResultDataGrid;
            ModData.v_AppGIS.TxtDisplayPage = txtDisplayPage;
            ModData.v_AppGIS.BtnFirst = btnFirst;
            ModData.v_AppGIS.BtnLast = btnLast;
            ModData.v_AppGIS.BtnNext = btnNext;
            ModData.v_AppGIS.BtnPre = btnPreview;
            ModData.v_AppGIS.DataCheckGrid = dgvDataCheck;
            ModData.v_AppGIS.Visible = this.Visible;
            ModData.v_AppGIS.Enabled = this.Enabled;
            ModData.v_AppGIS.CurrentThread = null;
            ModData.v_AppGIS.QueryBar = _BottomQueryBar;
            //ModData.v_QueryResult = new frmQueryOperationRecords(); //初始化操作查询窗体
            //初始化框架插件控件界面
            InitialFrmDefControl();

            //初始化更新对比列表分页显示按钮
            btnLast.Enabled = true;
            btnNext.Enabled = true;
            btnPreview.Enabled = true;
            btnFirst.Enabled = true;
            txtDisplayPage.Enabled = true;
            if (dgvUpdate.DataSource == null || (dgvUpdate.DataSource as DataTable).Rows.Count == 0)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnPreview.Enabled = false;
                btnFirst.Enabled = false;
                txtDisplayPage.Enabled = false;
            }


            //初始化数据库工程树图
            //InitialDBProject();
        }

        //初始化配置对应视图控件
        private void InitialMainViewControl()
        {
            _frmBarManager = new frmBarManager();
            _frmBarManager.TopLevel = false;
            _frmBarManager.Dock = DockStyle.Fill;
            _frmBarManager.Show();
            this.Controls.Add(_frmBarManager);

            //加载设置控制树图
            DevComponents.DotNetBar.Bar barTree = _frmBarManager.CreateBar("barTree", enumLayType.FILL);
            barTree.CanHide = false;
            barTree.CanAutoHide = true;
            DevComponents.DotNetBar.PanelDockContainer PanelTree = _frmBarManager.CreatePanelDockContainer("PanelTree", barTree);
            DockContainerItem TreeContainerItem = _frmBarManager.CreateDockContainerItem("TreeContainerItem", "控制树图", PanelTree, barTree);
            PanelTree.Controls.Add(this.tabControl);
            this.tabControl.Dock = DockStyle.Fill;

            //加载设置数据视图
            DevComponents.DotNetBar.Bar barMap = _frmBarManager.CreateBar("barMap", enumLayType.FILL);
            barMap.CanHide = false;
            DevComponents.DotNetBar.PanelDockContainer PanelMap = _frmBarManager.CreatePanelDockContainer("PanelMap", barMap);
            DockContainerItem MapContainerItem = _frmBarManager.CreateDockContainerItem("TreeContainerItem", "数据视图", PanelMap, barMap);
            PanelMap.Controls.Add(this.axMapControl);
            this.axMapControl.Dock = DockStyle.Fill;
            _MapToolControl = PanelMap as Control;

            //布局设置
            _frmBarManager.MainDotNetBarManager.FillDockSite.GetDocumentUIManager().Dock(barTree, barMap, eDockSide.Right);
            _frmBarManager.MainDotNetBarManager.FillDockSite.GetDocumentUIManager().SetBarWidth(barTree, this.Width / 5);

            //加载设置提示窗体
            //数据处理显示
            PanelDockContainer PanelTipData = new PanelDockContainer();
            PanelTipData.Controls.Add(this.advTree);
            this.advTree.Dock = DockStyle.Fill;
            DockContainerItem dockItemData = new DockContainerItem("dockItemData", "处理");
            dockItemData.Control = PanelTipData;
            _frmBarManager.ButtomBar.Items.Add(dockItemData);

            //更新对比分析显示
            //PanelDockContainer PanelTipUpdate = new PanelDockContainer();
            //PanelTipUpdate.Controls.Add(this.dgvUpdate);
            //this.dgvUpdate.Dock = DockStyle.Fill;
            //DockContainerItem dockItemUpdate = new DockContainerItem("dockItemUpdate", "更新对比");
            //dockItemUpdate.Control = PanelTipUpdate;
            //_frmBarManager.ButtomBar.Items.Add(dockItemUpdate);
            //this.dgvUpdate.DataSourceChanged += new EventHandler(dgvUpdate_DataSourceChanged);
           
            ///自动隐藏
            ///
            _frmBarManager.ButtomBar.AutoHide = true;

            //分页显示bar
            //PanelTipUpdate.Controls.Add(this.DevBar);
            //this.DevBar.Location = new System.Drawing.Point(this.dgvUpdate.Location.X, this.dgvUpdate.Location.Y + this.dgvUpdate.Height);
            //this.DevBar.Size = new System.Drawing.Size(this.dgvUpdate.Width, 20);
            //this.DevBar.Dock = DockStyle.Bottom;

            //cyf 20110610 modify:屏蔽掉检查加过控件
            //数据检查结果
            //PanelDockContainer PanelTipDataCheck = new PanelDockContainer();
            //PanelTipDataCheck.Controls.Add(this.dgvDataCheck);
            //this.dgvDataCheck.Dock = DockStyle.Fill;
            //DockContainerItem dockItemDataCheck = new DockContainerItem("dockItemDataCheck", "检查结果");
            //dockItemDataCheck.Control = PanelTipDataCheck;
            //_frmBarManager.ButtomBar.Items.Add(dockItemDataCheck);
            //end

            //线搜索结果列表
            PanelDockContainer PanelTipPolyline = new PanelDockContainer();
            PanelTipPolyline.Controls.Add(this.PolylineDataGrid);
            this.PolylineDataGrid.Dock = DockStyle.Fill;
            DockContainerItem dockItemPolyline = new DockContainerItem("dockItemPolyline", "线搜索结果");
            dockItemPolyline.Control = PanelTipPolyline;
            _frmBarManager.ButtomBar.Items.Add(dockItemPolyline);
            dockItemPolyline.Visible = false;

            //多边形搜索结果
            PanelDockContainer PanelTipPolygon = new PanelDockContainer();
            PanelTipPolygon.Controls.Add(this.PolygonDataGrid);
            this.PolygonDataGrid.Dock = DockStyle.Fill;
            DockContainerItem dockItemPolygon = new DockContainerItem("dockItemPolygon", "多边形搜索结果");
            dockItemPolygon.Control = PanelTipPolygon;
            _frmBarManager.ButtomBar.Items.Add(dockItemPolygon);
            dockItemPolygon.Visible = false;

            //接边融合记录结果
            PanelDockContainer PanelTipJoinMergeResult = new PanelDockContainer();
            PanelTipJoinMergeResult.Controls.Add(this.JoinMergeResultDataGrid);
            this.JoinMergeResultDataGrid.Dock = DockStyle.Fill;
            DockContainerItem dockItemJoinMergeResult = new DockContainerItem("dockItemJoinMergeResylt", "接边融合结果");
            dockItemJoinMergeResult.Control = PanelTipJoinMergeResult;
            _frmBarManager.ButtomBar.Items.Add(dockItemJoinMergeResult);
            dockItemJoinMergeResult.Visible = false;


            PanelDockContainer PanelTipDataCheck = new PanelDockContainer();
            PanelTipDataCheck.Controls.Add(this.bottomBar);
            this.dgvDataCheck.Dock = DockStyle.Fill;
            this.bottomBar.Dock = DockStyle.Fill;
            //PanelTipDataCheck.Controls.Add(this.btnReport);
            //this.btnReport.Dock = DockStyle.Bottom;
            DockContainerItem dockItemQueryRes = new DockContainerItem("dockItemDataCheck", "查询结果");
            dockItemQueryRes.Control = PanelTipDataCheck;
            _frmBarManager.ButtomBar.Items.Add(dockItemQueryRes);
           
            _BottomQueryBar = this.bottomBar;


        }

        //初始化框架插件控件界面
        private void InitialFrmDefControl()
        {
            //得到Xml的System节点,根据XML加载插件界面
            string xPath = ".//System[@Name='" + this.Name + "']";
            Plugin.ModuleCommon.LoadButtonViewByXmlNode(ModData.v_AppGIS.ControlContainer, xPath, ModData.v_AppGIS);

            _dicContextMenu = ModData.v_AppGIS.DicContextMenu;

            //初始化地图浏览工具栏
            Plugin.Application.IAppFormRef pAppFrm = ModData.v_AppGIS as Plugin.Application.IAppFormRef;
            XmlNode barXmlNode = pAppFrm.SystemXml.SelectSingleNode(".//ToolBar[@Name='ControlMapToolBar2']");
            if (barXmlNode == null || _MapToolControl == null) return;
            DevComponents.DotNetBar.Bar mapToolBar = Plugin.ModuleCommon.LoadButtonView(_MapToolControl, barXmlNode, pAppFrm, null) as Bar;
            if (mapToolBar != null)
            {
             /*   _MapToolBar = mapToolBar;
                mapToolBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
                //cyf 20110609 modify
                mapToolBar.DockOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
                mapToolBar.DockSide = DevComponents.DotNetBar.eDockSide.Left;
                //end
                mapToolBar.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
                mapToolBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
                _MapToolControl.Controls.Remove(mapToolBar);
                //_MapToolControl.Controls.Remove(this.axMapControl);
                //_MapToolControl.Controls.Add(this.axMapControl);
                _MapToolControl.Controls.Add(mapToolBar);*/
                //wgf 20110609 把top修改为left布局
                mapToolBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
                mapToolBar.DockOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
                mapToolBar.DockSide = DevComponents.DotNetBar.eDockSide.Left;
                mapToolBar.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.None;
                mapToolBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
                mapToolBar.RoundCorners = false;
                mapToolBar.SendToBack();
            }

            foreach (KeyValuePair<string, DevComponents.DotNetBar.ContextMenuBar> keyValue in _dicContextMenu)
            {
                this.Controls.Add(keyValue.Value);
            }
        }

        //初始化数据库工程树图
        private void InitialDBProject()
        {
            if(File.Exists(ModData.v_projectXML))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ModData.v_projectXML);
                ModData.v_AppGIS.DBXmlDocument = xml;

                ProjectXml.AddTreeNodeByXML(ModData.v_AppGIS.DBXmlDocument, ModData.v_AppGIS.ProjectTree);
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
                        if (_dicContextMenu.ContainsKey("TOCControlContextMenu2"))
                        {
                            if (_dicContextMenu["TOCControlContextMenu2"] != null)
                            {
                                if (_dicContextMenu["TOCControlContextMenu2"].Items.Count > 0)
                                {
                                    item = _dicContextMenu["TOCControlContextMenu2"].Items[0] as DevComponents.DotNetBar.ButtonItem;
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
                            if (!(pLayer is IGroupLayer || pLayer is IFeatureLayer || pLayer is IDataLayer || pLayer is IDynamicLayer)) return;
                            if (_dicContextMenu.ContainsKey("TOCControlLayerContextMenu2"))
                            {
                                if (_dicContextMenu["TOCControlLayerContextMenu2"] != null)
                                {
                                    if (_dicContextMenu["TOCControlLayerContextMenu2"].Items.Count > 0)
                                    {
                                        item = _dicContextMenu["TOCControlLayerContextMenu2"].Items[0] as DevComponents.DotNetBar.ButtonItem;
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

        //图层浏览右键菜单
        private void axMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            timerShow.Enabled = false;  //进行操作时，操作历史查询不可用

            if (e.button == 1 || _dicContextMenu == null)
                return;
            System.Drawing.Point pPoint = new System.Drawing.Point(e.x, e.y);
            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("MapControlContextMenu2"))
            {
                if (_dicContextMenu["MapControlContextMenu2"].Items.Count > 0)
                {
                    item = _dicContextMenu["MapControlContextMenu2"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        item.Popup(axMapControl.PointToScreen(pPoint));
                    }
                }
            }
        }

        //利用此事件结合局部变量m_bGroupLayerVisible控制解决GroupLayer显示问题
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

        #region 陈亚飞 添加
        //更新对比列表右键菜单
        private void dgvUpdate_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //列表的右键菜单
            if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            DevComponents.DotNetBar.Controls.DataGridViewX dtView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            dtView = sender as DevComponents.DotNetBar.Controls.DataGridViewX;

            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("ContextMenuUpdateDataGrid2"))
            {
                if (_dicContextMenu["ContextMenuUpdateDataGrid2"].Items.Count > 0)
                {
                    item = _dicContextMenu["ContextMenuUpdateDataGrid2"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        item.Popup(m_Point);
                    }
                }
            }
        }

        private void dgvUpdate_MouseMove(object sender, MouseEventArgs e)
        {
            //捕捉鼠标移动点坐标
            m_Point = System.Windows.Forms.Cursor.Position;
        }

        //下一页
        private void btnNext_Click(object sender, EventArgs e)
        {
            string pageInfo = txtDisplayPage.ControlText.Trim();
            if (pageInfo == "") return;
            int index2 = pageInfo.IndexOf("/");
            int len = index2;
            int currentPage = int.Parse(pageInfo.Substring(0, len).Trim());
            int totalPage = int.Parse(pageInfo.Substring(index2+1).Trim());
            int recNum = ModData.pageSize * currentPage;

            Plugin.Application.IAppGISRef m_Hook = ModData.v_AppGIS;

            ModDBOperator.LoadPage(m_Hook, ModData.TotalTable, currentPage+1, recNum);
        }
        //上一页
        private void btnPreview_Click(object sender, EventArgs e)
        {
            string pageInfo = txtDisplayPage.ControlText.Trim();
            if (pageInfo == "") return;
            int index2 = pageInfo.IndexOf("/");
            int len = index2 ;
            int currentPage = int.Parse(pageInfo.Substring(0, len).Trim());
            int recNum = ModData.pageSize * (currentPage - 2);

            Plugin.Application.IAppGISRef m_Hook = ModData.v_AppGIS;
            ModDBOperator.LoadPage(m_Hook, ModData.TotalTable, currentPage-1, recNum);
        }
        //第一页
        private void btnFirst_Click(object sender, EventArgs e)
        {
            ModData.recNum = 0;
            ModData.CurrentPage = 1;
            int recNum = ModData.recNum; 
            int currentPage = ModData.CurrentPage;

            Plugin.Application.IAppGISRef m_Hook = ModData.v_AppGIS;
            ModDBOperator.LoadPage(m_Hook, ModData.TotalTable, currentPage, recNum);
        }
        //最后一页
        private void btnLast_Click(object sender, EventArgs e)
        {
            if (ModData.TotalPageCount == 0) return;
            int recNum = (ModData.TotalPageCount - 1) * ModData.pageSize;
            int currentPage = ModData.TotalPageCount;

            Plugin.Application.IAppGISRef m_Hook = ModData.v_AppGIS;
            ModDBOperator.LoadPage(m_Hook, ModData.TotalTable, currentPage, recNum);
        }

        private void dgvUpdate_DataSourceChanged(object sender, EventArgs e)
        {
            //刷新更新对比列表分页显示按钮
            if (dgvUpdate.DataSource == null || (dgvUpdate.DataSource as DataTable).Rows.Count == 0)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnPreview.Enabled = false;
                btnFirst.Enabled = false;
                txtDisplayPage.Enabled = false;
            }
            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
                btnPreview.Enabled = true;
                btnFirst.Enabled = true;
                txtDisplayPage.Enabled = true;
            }
        }

        private void dgvDataCheck_MouseMove(object sender, MouseEventArgs e)
        {
            m_Point = System.Windows.Forms.Cursor.Position;

        }

        //加载检查的右键菜单
        private void dgvDataCheck_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //列表的右键菜单
            if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            DevComponents.DotNetBar.Controls.DataGridViewX dtView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            dtView = sender as DevComponents.DotNetBar.Controls.DataGridViewX;

            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("ContextMenuCheckGrid2"))
            {
                if (_dicContextMenu["ContextMenuCheckGrid2"].Items.Count > 0)
                {
                    item = _dicContextMenu["ContextMenuCheckGrid2"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        item.Popup(m_Point);
                    }
                }
            }

        }

        #endregion

        private void advTreeProject_NodeMouseDown(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            DevComponents.AdvTree.AdvTree aTree = sender as DevComponents.AdvTree.AdvTree;
            if (aTree.SelectedNode == null) return;
            //cyf 20110625 modify:
            if (!(aTree.SelectedNode.DataKeyString == "DB"|| aTree.SelectedNode.DataKeyString == "FD" || aTree.SelectedNode.DataKeyString == "FC" || aTree.SelectedNode.DataKeyString == "RC" || aTree.SelectedNode.DataKeyString == "RD")) return;
            //cyf 20110608  
            //if (!(aTree.SelectedNode.DataKeyString == "现势库" || aTree.SelectedNode.DataKeyString == "历史库" || aTree.SelectedNode.DataKeyString=="栅格数据库")) return;
            //end
            //end
            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("ContextMenuProjectTree"))
            {
                if (_dicContextMenu["ContextMenuProjectTree"].Items.Count > 0)
                {
                    item = _dicContextMenu["ContextMenuProjectTree"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        aTree.SelectedNode.ContextMenu = item;
                    }
                }
            }
        }

        private void axMapControl_OnAfterDraw(object sender, IMapControlEvents2_OnAfterDrawEvent e)
        {
            try
            {
                //=========================================================================================
                //陈亚飞添加  20101220  修改 ：根据主窗口mapcontrol同时放大或缩小付窗口
                if (ModData.m_Mapcontrol != null)
                {
                    //若付窗体存在
                    IEnvelope pReferFuulEx = axMapControl.FullExtent;//主窗体的FullExtent
                    IEnvelope pReferEnvelop = axMapControl.Extent;   //主窗体的Extent
                    IEnvelope pMainFullEx = ModData.m_Mapcontrol.ActiveView.FullExtent;// 付窗体的FullExtent

                    IEnvelope pMainEx = new EnvelopeClass();     //声明变量Envelop用来保存付窗体的Extent

                    //付窗体的最大最小x，最大最小y
                    pMainEx.XMin = pMainFullEx.XMin + (pReferEnvelop.XMin - pReferFuulEx.XMin) / pReferFuulEx.Width * pMainFullEx.Width;
                    pMainEx.XMax = pMainFullEx.XMax - (pReferFuulEx.XMax - pReferEnvelop.XMax) / pReferFuulEx.Width * pMainFullEx.Width;
                    pMainEx.YMin = pMainFullEx.YMin + (pReferEnvelop.YMin - pReferFuulEx.YMin) / pReferFuulEx.Height * pMainFullEx.Height;
                    pMainEx.YMax = pMainFullEx.YMax - (pReferFuulEx.YMax - pReferEnvelop.YMax) / pReferFuulEx.Height * pMainFullEx.Height;

                    ModData.m_Mapcontrol.Extent = pMainEx;  //给付窗体的范围赋值
                    ModData.m_Mapcontrol.ActiveView.Refresh(); //刷新控件
                }

                //===================================================================

                ModData.v_AppGIS.RefScaleCmb.ControlText = (sender as AxMapControl).Map.ReferenceScale.ToString().Trim();
                ModData.v_AppGIS.CurScaleCmb.ControlText = (sender as AxMapControl).Map.MapScale.ToString().Trim();
                ModData.v_AppGIS.CurScaleCmb.Tooltip = axMapControl.Map.MapScale.ToString().Trim();
            }
            catch
            {
            }
        }

        private void axMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
			double TempX = double.Parse(e.mapX.ToString("0.00"));
            double TempY = double.Parse(e.mapY.ToString("0.00"));
            //ModData.v_AppSMPD.CoorTxt.ControlText = "X:" + e.mapX + ";Y:" + e.mapY;
            if (!ModData.v_AppGIS.ProgressBar.Visible)  //cyf 20110707 modify:进度条控制死否赋值
            {
                //changed by chulili 20110716近似到小数点后两位
                ModData.v_AppGIS.OperatorTips = "X:" + TempX + ";Y:" + TempY;
            }
        }

        //处理提示右键
        private void advTree_NodeMouseDown(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            //if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            //DevComponents.AdvTree.AdvTree aTree = sender as DevComponents.AdvTree.AdvTree;
            //DevComponents.DotNetBar.ButtonItem item = null;
            //if (_dicContextMenu.ContainsKey("ContextMenuProjectTree"))
            //{
            //    if (_dicContextMenu["ContextMenuProjectTree"].Items.Count > 0)
            //    {
            //        item = _dicContextMenu["ContextMenuProjectTree"].Items[0] as DevComponents.DotNetBar.ButtonItem;
            //        if (item != null)
            //        {
            //            aTree.SelectedNode.ContextMenu = item;
            //        }
            //    }
            //}
        }
        private void txtDisplayPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtDisplayPage.ControlText.Trim() == "") return;
                int totalCount = ModData.TotalPageCount;//总页数
                if (totalCount == 0) return;
                int curPage;
                try
                {
                    curPage = int.Parse(txtDisplayPage.ControlText.Trim());//当前页
                }
                catch(Exception er)
                {
                    //*******************************************************************
                    //guozheng added
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(er, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(er, null, DateTime.Now);
                    }
                    //********************************************************************

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
                Plugin.Application.IAppGISRef m_Hook = ModData.v_AppGIS;
                ModDBOperator.LoadPage(m_Hook, ModData.TotalTable, curPage, recNum);
            }
        }

        /// <summary>
        /// 接边搜索结果表单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PolylineDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.PolylineDataGrid.DataSource == null)
                return;
            ShowState((this.PolylineDataGrid.DataSource as DataTable).TableName);
        }

        private void PolygonDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.PolygonDataGrid.DataSource == null)
                return;
            ShowState((this.PolygonDataGrid.DataSource as DataTable).TableName);
           
        }
        private void ShowState(string TableName)
        {
            IGraphicsContainer pGra = ModData.v_AppGIS.ArcGisMapControl.Map as IGraphicsContainer;
            pGra.DeleteAllElements();
            if (TableName == "PolylineSearchTable")
            {
                #region 显示线接边信息
                if (this.PolylineDataGrid.DataSource == null)
                    return;
                DataRow getrow = null;
                try
                {
                    getrow = (this.PolylineDataGrid.DataSource as DataTable).Rows[this.PolylineDataGrid.CurrentRow.Index];
                }
                catch(Exception e)
                {
                    //*******************************************************************
                    //guozheng added
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

                    return;
                }

                string DataSetname = string.Empty;
                long OriOid = -1;
                long DesOid = -1;
                string OriPtn = string.Empty;
                string DesPtn = string.Empty;
                OriPtn = getrow["OriPtn"].ToString().Trim();
                DesPtn = getrow["DesPtn"].ToString().Trim();
                DataSetname = getrow["数据集"].ToString().Trim();

                OriOid = Convert.ToInt64(getrow["源要素ID"].ToString().Trim());
                DesOid = Convert.ToInt64(getrow["目标要素ID"].ToString().Trim());

                if (DesOid == -1)
                    return;

                IFeature OriLineFea = GetFeatureFromMap(DataSetname, OriOid);
                if (null == OriLineFea)
                    return;
                ZoomToFeature(this.axMapControl, OriLineFea);

                pGra.DeleteAllElements();
                if (DesOid != -1)
                {
                    IFeature DesLineFea = GetFeatureFromMap(DataSetname, DesOid);
                    if (null == DesLineFea)
                        return;
                    IPoint OriPt = new PointClass();
                    IPolyline getline = OriLineFea.Shape as IPolyline;
                    IPolyline getline2 = DesLineFea.Shape as IPolyline;
                    if (getline==null)
                        return;
                    if (OriPtn == "ToPoint")
                    {
                        // MakePointSymbol(pGra, OriTop.X, OriTop.Y, "起点");
                        OriPt = getline.ToPoint;
                    }
                    else if (OriPtn == "FromPoint")
                    {
                        // MakePointSymbol(pGra, OriFromp.X, OriFromp.Y, "起点");
                        OriPt = getline.FromPoint;
                    }
                    MakePointSymbol(pGra, OriPt.X, OriPt.Y);
                    /////////////
                    if (getline2 == null)
                        return;
                    IPoint DesPt = new PointClass();
                    if (DesPtn == "FromPoint")
                    {
                        DesPt = getline2.FromPoint;
                    }
                    else if (DesPtn == "ToPoint")
                    {
                        DesPt = getline2.ToPoint;
                    }
                    MakePointSymbol(pGra, DesPt.X, DesPt.Y);
                }

                #endregion
            }
            else if (TableName == "PolygonSearchTable")
            {
                #region 显示多边形接边信息
                if (this.PolygonDataGrid.DataSource == null)
                    return;
                DataRow getrow = null;
                try
                {
                    getrow = (this.PolygonDataGrid.DataSource as DataTable).Rows[this.PolygonDataGrid.CurrentRow.Index];
                }
                catch(Exception e)
                {
                    //*******************************************************************
                    //guozheng added
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
                    return;
                }

                string DataSetName = string.Empty;
                long OriOID = -1;
                long OriIndex = -1;
                long DesOID = -1;
                long DesIndex = -1;
                DataSetName = getrow["数据集"].ToString().Trim();
                OriOID = Convert.ToInt64(getrow["源要素ID"].ToString().Trim());
                DesOID = Convert.ToInt64(getrow["目标要素ID"].ToString().Trim());
                OriIndex = Convert.ToInt64(getrow["OriLineIndex"].ToString().Trim());
                DesIndex = Convert.ToInt64(getrow["DesLineIndex"].ToString().Trim());
                if (OriOID == -1 || DesOID == -1)
                    return;
                IFeature OriFea = GetFeatureFromMap(DataSetName, OriOID);
                IFeature DesFea = GetFeatureFromMap(DataSetName, DesOID);
                if (null == OriFea || null == DesFea)
                    return;
                ZoomToFeature(this.axMapControl, OriFea);
                if (OriIndex != -1)
                {
                    IPointCollection PoCol = new Polyline();
                    IPolygon newpolygon = OriFea.ShapeCopy as IPolygon;
                    PoCol.AddPointCollection(newpolygon as IPointCollection);
                    ISegmentCollection newSeCol = PoCol as ISegmentCollection;
                    ISegment Seg = newSeCol.get_Segment((int)OriIndex);
                    if (Seg.GeometryType == esriGeometryType.esriGeometryLine)
                    {
                        ILine getLine = Seg as ILine;
                        MakeLineSymbol(pGra, getLine);
                    }
                }
                if (DesIndex != -1)
                {
                    IPointCollection PoCol = new Polyline();
                    IPolygon newpolygon = DesFea.ShapeCopy as IPolygon;
                    PoCol.AddPointCollection(newpolygon as IPointCollection);
                    ISegmentCollection newSeCol = PoCol as ISegmentCollection;
                    ISegment Seg = newSeCol.get_Segment((int)DesIndex);
                    if (Seg.GeometryType == esriGeometryType.esriGeometryLine)
                    {
                        ILine getLine = Seg as ILine;
                        MakeLineSymbol(pGra, getLine);
                    }
                }

                #endregion
            }
            else if (TableName == "JoinResultTable" || TableName == "MergeResultTable")
            {
                #region 显示结果信息信息
                if (this.JoinMergeResultDataGrid.DataSource == null)
                    return;
                DataRow getrow = null;
                try
                {
                    getrow = (this.JoinMergeResultDataGrid.DataSource as DataTable).Rows[this.JoinMergeResultDataGrid.CurrentRow.Index];
                }
                catch(Exception e)
                {
                    //*******************************************************************
                    //guozheng added
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

                    return;
                }

                string DataSetName = string.Empty;
                long OriOID = -1;
                long DesOID = -1;
                DataSetName = getrow["数据集"].ToString().Trim();
                OriOID = Convert.ToInt64(getrow["源要素ID"].ToString().Trim());
                DesOID = Convert.ToInt64(getrow["目标要素ID"].ToString().Trim());
                if (OriOID == -1 || DesOID == -1)
                    return;
                IFeature OriFea = GetFeatureFromMap(DataSetName, OriOID);
                if (null == OriFea)
                    return;
                ZoomToFeature(this.axMapControl, OriFea);

                #endregion
            }
        }
        private void MakePointSymbol(IGraphicsContainer pGra, double x, double y)
        {

            IActiveView pAcitveView = pGra as IActiveView;
            IElement pEle;
            //画点
            ESRI.ArcGIS.Geometry.IPoint pPint = new ESRI.ArcGIS.Geometry.PointClass();
            pPint.PutCoords(x, y);
            IRgbColor pColor = new RgbColorClass();
            pColor.Blue = 255;
            pColor.Green = 0;
            pColor.Red = 130;

            IMarkerElement pMakEle = new MarkerElementClass();
            pEle = pMakEle as IElement;
            IMarkerSymbol pMakSym = new SimpleMarkerSymbolClass();
            pMakSym.Color = pColor;
            pMakEle.Symbol = pMakSym;
            pEle.Geometry = pPint as ESRI.ArcGIS.Geometry.IGeometry;

            pGra.AddElement(pEle, 0);
            
        }
        private void MakeLineSymbol(IGraphicsContainer pGra, ILine line)
        {
            IActiveView pAcitveView = pGra as IActiveView;
            IElement pEle;
            IRgbColor pColor = new RgbColorClass();
            pColor.Blue = 0;
            pColor.Green = 0;
            pColor.Red = 255;

            ILineElement pMakEle = new LineElementClass();

            pEle = pMakEle as IElement;
            ILineSymbol pMakSym = new SimpleLineSymbolClass();
            pMakSym.Width = 2;
            pMakSym.Color = pColor;
            pMakEle.Symbol = pMakSym;
            IPolyline newline = new PolylineClass();
            newline.ToPoint = line.ToPoint;
            newline.FromPoint = line.FromPoint;
            pEle.Geometry = newline as ESRI.ArcGIS.Geometry.IGeometry;

            pGra.AddElement(pEle, 0);
            pAcitveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        private void ZoomToFeature(AxMapControl pMapControl, IFeature pFeature)
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
                pEnvelope = pFeature.Shape.Envelope;
            }

            if (pEnvelope == null) return;
            pEnvelope.Expand(1.5, 1.5, true);
            IActiveView pActiveView = pMapControl.Map as IActiveView;
            pActiveView.Extent = pEnvelope;
            pActiveView.Refresh();
        }
        private IFeature GetFeatureFromMap(string Layername, long FeatureOID)
        {
            if (string.IsNullOrEmpty(Layername))
                return null;
            if (FeatureOID < 0)
                return null;
            int Layercount = ModData.v_AppGIS.ArcGisMapControl.LayerCount;
            if (Layercount == 0)
                return null;
            for (int i = 0; i < Layercount; i++)
            {
                IFeatureLayer getLayer = ModData.v_AppGIS.ArcGisMapControl.get_Layer(i) as IFeatureLayer;
                IFeatureClass getFeacls = getLayer.FeatureClass;
                if (null == getFeacls)
                    return null;
                IFeature getFea = null;
                if (getLayer.Name == Layername)
                {
                    try
                    {
                        getFea = getFeacls.GetFeature((int)FeatureOID);
                        return getFea;
                    }
                    catch(Exception e)
                    {
                        //*******************************************************************
                        //guozheng added
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

                        return null;
                    }
                }
            }
            return null;
        }

        private void PolylineDataGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.PolylineDataGrid.Name = null;
            this.PolygonDataGrid.Name = null;
            this.JoinMergeResultDataGrid.Name = null;
            //接边信息表的右键菜单
            if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            DevComponents.DotNetBar.Controls.DataGridViewX dtView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            dtView = sender as DevComponents.DotNetBar.Controls.DataGridViewX;

            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("JoinDataGridMenu"))
            {
                if (_dicContextMenu["JoinDataGridMenu"].Items.Count > 0)
                {
                    item = _dicContextMenu["JoinDataGridMenu"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        this.PolylineDataGrid.Name = "ActiveMenu";
                        item.Popup(m_Point);
                    }
                }
            }
            
        }

        private void PolygonDataGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.PolylineDataGrid.Name = null;
            this.PolygonDataGrid.Name = null;
            this.JoinMergeResultDataGrid.Name = null;
            //接边信息表的右键菜单
            if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            DevComponents.DotNetBar.Controls.DataGridViewX dtView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            dtView = sender as DevComponents.DotNetBar.Controls.DataGridViewX;

            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("JoinDataGridMenu"))
            {
                if (_dicContextMenu["JoinDataGridMenu"].Items.Count > 0)
                {
                    item = _dicContextMenu["JoinDataGridMenu"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        this.PolygonDataGrid.Name = "ActiveMenu";
                        item.Popup(m_Point);
                    }
                }
            }
        }

        private void JoinMergeResultDataGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.PolylineDataGrid.Name = null;
            this.PolygonDataGrid.Name = null;
            this.JoinMergeResultDataGrid.Name = null;
            //接边信息表的右键菜单
            if (e.Button != MouseButtons.Right || _dicContextMenu == null) return;
            DevComponents.DotNetBar.Controls.DataGridViewX dtView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            dtView = sender as DevComponents.DotNetBar.Controls.DataGridViewX;

            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("JoinDataGridMenu"))
            {
                if (_dicContextMenu["JoinDataGridMenu"].Items.Count > 0)
                {
                    item = _dicContextMenu["JoinDataGridMenu"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        this.JoinMergeResultDataGrid.Name = "ActiveMenu";
                        item.Popup(m_Point);
                    }
                }
            }
        }

        private void JoinMergeResultDataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.JoinMergeResultDataGrid.DataSource == null)
                return;
            ShowState((this.JoinMergeResultDataGrid.DataSource as DataTable).TableName);

        }

        private void PolylineDataGrid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_Point = System.Windows.Forms.Cursor.Position;
        }

        private void PolygonDataGrid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_Point = System.Windows.Forms.Cursor.Position;
        }

        
        private void JoinMergeResultDataGrid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_Point = System.Windows.Forms.Cursor.Position;
        }
    }
}
