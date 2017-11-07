using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using SysCommon.Gis;
namespace GeoSysSetting.SubControl
{
    public partial class UCDataSourceManger : UserControl
    {
        public IWorkspace m_TmpWorkSpace = null;
        //右键菜单集合
        
        //private String _layerTreePath = Application.StartupPath + "\\..\\res\\xml\\配置图层树.xml";
        
        //changed by chulili 20110722启动于数据源界面 IAppPrivilegesRef->IAppFormRef
        public Plugin.Application.IAppFormRef m_AppFrmRef = null;
        private ILayer _SelSymLayer = null;

        private List<string> _DeleteNodeKeys = new List<string >();
        private string _Decimalstr = "";//added by chulili 20111008 状态栏当前比例尺小数位数格式化参数
        public UCDataSourceManger(Plugin.Application.IAppFormRef hook)
        {
            InitializeComponent();
            //added by chulili 20111010 
            m_AppFrmRef = hook;
            Plugin.Application.IAppDBIntegraRef pDBIntegra = hook as Plugin.Application.IAppDBIntegraRef;
            Plugin.Application.IAppFormRef pAppFrm = hook as Plugin.Application.IAppFormRef;
            pAppFrm.LayerTree = this.layerTree as object;
            
            pDBIntegra.CurScaleVisible = true;
            pDBIntegra.MapControl = this.MapControlLayer.Object  as IMapControlDefault ;
            pDBIntegra.TOCControl = this.axTOCControl.Object as ITOCControlDefault ;
            //end added
            InitLayerTree();
            axTOCControl.SetBuddyControl(MapControlLayer.Object );
            
        }
        //初始化图层树 added by chulili 20110601
        public void InitLayerTree()
        {
            this.MapControlLayer.Map.Name = "数据图层";
            //ZQ  20110827  add
            this.layerTree.isDragDrop = false;  //changed by chulili 20111107 不允许拖拽
            //end
            this.layerTree.isLayerConfig = true;
            this.layerTree.LayerVisible = false;
            this.layerTree.CurMap = this.MapControlLayer.Object  as ESRI.ArcGIS.Controls.IMapControl2 ;
            this.layerTree.CurWks = Plugin.ModuleCommon.TmpWorkSpace ;
            //string strpath = Application.StartupPath+"\\..\\res\\xml\\图层树.xml";
            if (GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu.ContainsKey("ContextLayerTreeManage"))
            {
                this.layerTree.LayerTreeManageContextMenu = GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu["ContextLayerTreeManage"];
            }
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, ModPublicFun._layerTreePath);
            this.layerTree.TocControl = this.axTOCControl.Object as ITOCControlDefault ;
            this.layerTree.DicMenu = null;
            this.layerTree.LayerXmlPath = ModPublicFun._layerTreePath;
            this.layerTree.InitLayerDic();
            this.layerTree.LoadData();

            GetScaleDecimal();//added by chulili 20110816 获取比例尺配置（小数位数）
            
        }

        //added by chulili 20110722添加函数，不判断状态，直接刷新
        public void RefreshLayerTreeEx()
        {
               //初始化图层视图
                SysCommon.CProgress vProgress = new SysCommon.CProgress("目录管理");
                vProgress.EnableCancel = false;
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("刷新图层列表");
                SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, ModPublicFun._layerTreePath);

                this.layerTree.LoadData();
                vProgress.Close();
                ModPublicFun._SaveLayerTree = true;
        }
        
        //导入图层目录后，目录配置界面
        public void RefreshLayerTree()
        {
            if ( ModPublicFun._SaveLayerTree == false)
            {                //初始化图层视图
                SysCommon.CProgress vProgress = new SysCommon.CProgress("目录管理");
                vProgress.EnableCancel = false;
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("刷新图层列表");
                SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase (Plugin.ModuleCommon.TmpWorkSpace, ModPublicFun._layerTreePath);

                this.layerTree.LoadData();
                vProgress.Close();
                ModPublicFun._SaveLayerTree = true;
            }
        }
        //修改图层的可见状态（点击视图浏览菜单）传入参数为修改后的状态
        public void ChangeLayerVisible(bool newvisible)
        {
            this.layerTree.LayerVisible = newvisible;
            GeoLayerTreeLib.LayerManager.ModuleMap.SetLayersVisibleAttri(MapControlLayer.Object as IMapControlDefault, newvisible);
            
            MapControlLayer.ActiveView.Refresh();

        }
        #region 右键菜单
        private void MapControlLayer_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            if(e.button == 2)
            {
                System.Drawing.Point ClickPoint = MapControlLayer.PointToScreen(new System.Drawing.Point(e.x, e.y));
                axMapControlcontextMenu.Show(ClickPoint);
            }
        }

        //右键点击放大
        private void MapZoomInMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomInToolClass();
            pCommand.OnCreate(MapControlLayer.Object);
            MapControlLayer.CurrentTool = pCommand as ESRI.ArcGIS.SystemUI.ITool;
        }

        //右键点击缩小
        private void ZoomOutMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomOutToolClass();
            pCommand.OnCreate(MapControlLayer.Object);
            MapControlLayer.CurrentTool = pCommand as ESRI.ArcGIS.SystemUI.ITool;
        }

        //右键点击漫游
        private void MapPanMenuItem_Click(object sender, EventArgs e)
        {

            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapPanToolClass();
            pCommand.OnCreate(MapControlLayer.Object);
            MapControlLayer.CurrentTool = pCommand as ESRI.ArcGIS.SystemUI.ITool;
        }

        //右键点击中心缩小
        private void MapZoomOutFixedMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomOutFixedCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }

        //右键点击中心放大
        private void MapZoomInFixedMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomInFixedCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }

        //右键点击刷新
        private void MapRefreshMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapRefreshViewCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }

        //右键点击全屏
        private void MapFullExtentMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapFullExtentCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }

        //右键点击后景
        private void BackCommandMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomToLastExtentBackCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }

        //右键点击前景
        private void ForwardCommandMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomToLastExtentForwardCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }

        #endregion

        #region 工具条菜单
        //工具条漫游
        private void buttonItemPan_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapPanToolClass();
            pCommand.OnCreate(MapControlLayer.Object);
            MapControlLayer.CurrentTool = pCommand as ESRI.ArcGIS.SystemUI.ITool;
        }
        //工具条放大
        private void buttonItemZoomIn_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomInToolClass();
            pCommand.OnCreate(MapControlLayer.Object);
            MapControlLayer.CurrentTool = pCommand as ESRI.ArcGIS.SystemUI.ITool;

        }
        //工具条缩小
        private void buttonItemZoonOut_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomOutToolClass();
            pCommand.OnCreate(MapControlLayer.Object);
            MapControlLayer.CurrentTool = pCommand as ESRI.ArcGIS.SystemUI.ITool;

        }
        //工具条全屏
        private void buttonItemFullExtent_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapFullExtentCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }
        //工具条中心放大
        private void buttonItemZoonInfixed_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomInFixedCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }
        //工具条中心缩小
        private void buttonItemZoomOutfixed_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomOutFixedCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }
        //工具条刷新
        private void buttonItemRefresh_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapRefreshViewCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }
        //工具条后景
        private void buttonItemBack_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomToLastExtentBackCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }
        //工具条前景
        private void buttonItemForward_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomToLastExtentForwardCommandClass();
            pCommand.OnCreate(MapControlLayer.Object);
            pCommand.OnClick();
        }
        #endregion

        private void UCDataSourceManger_VisibleChanged(object sender, EventArgs e)
        {
            //ModPublicFun.DealChangeSave(); //changed by chulili 20110722启动于数据源界面 IAppPrivilegesRef->IAppDBIntegraRef
            Plugin.Application.IAppDBIntegraRef pRef = m_AppFrmRef as Plugin.Application.IAppDBIntegraRef;
            if (pRef == null) return; //shduan 20110725

            if (this.Visible == true)
            {
                pRef.CurScaleVisible = true;
            }
            else
            {
                m_AppFrmRef.CoorXY = "";    //取自不同的接口
                pRef.CurScaleVisible = false;

            }
            //end changed by chulili
            //如果用户未保存目录，则初始化目录管理列表(将此步骤移动到点击目录管理菜单时比较合适)
            //if (!issave)
            //{
            //    this.RefreshLayerTree();
            //}
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
        private void MapControlLayer_OnAfterDraw(object sender, IMapControlEvents2_OnAfterDrawEvent e)
        {
            if (_Decimalstr == "")
            {
                _Decimalstr = "0.00";
            }
            double CurScale = double.Parse((sender as AxMapControl).Map.MapScale.ToString(_Decimalstr));
            if (m_AppFrmRef != null)
            {
                m_AppFrmRef.CurScaleCmb.ControlText = CurScale.ToString();
                m_AppFrmRef.CurScaleCmb.Tooltip = CurScale.ToString();

            }
            this.layerTree.UpdateLayerNodeImage();
            //this.comboBoxCurScale.ControlText = (sender as AxMapControl).Map.MapScale.ToString().Trim();
            //this.comboBoxCurScale.Tooltip = MapControlLayer.Map.MapScale.ToString().Trim();

        }

        private void UCDataSourceManger_EnabledChanged(object sender, EventArgs e)
        {
            ModPublicFun.DealChangeSave();
            //changed by chulili 20110722启动于数据源界面 IAppPrivilegesRef->IAppDBIntegraRef
            Plugin.Application.IAppDBIntegraRef pRef = m_AppFrmRef as Plugin.Application.IAppDBIntegraRef;
            if (pRef == null) return; // shduan 20110725

            if (this.Enabled == true)
            {
                pRef.CurScaleVisible = true;    //状态栏显示当前比例尺
            }
            else
            {
                m_AppFrmRef.CoorXY = "";    //取自不同的接口 状态栏不显示X\Y坐标
                pRef.CurScaleVisible = false;//状态栏不显示当前比例尺
            }
            //end changed by chulili
        }

        private void UCDataSourceManger_Load(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = this.splitContainer1.Width / 5;
        }

        private void axTOCControl_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            //if (e.button == 1)
            //{
            //    esriTOCControlItem pItem = new esriTOCControlItem();
            //    IBasicMap pMap = new MapClass();
            //    ILayer pLayer = new FeatureLayerClass();
            //    object other = new object();
            //    object index = new object();
            //    ILegendGroup pLegendGroup;
            //    _SelSymLayer = null;

            //    axTOCControl.HitTest(e.x, e.y, ref pItem, ref pMap, ref pLayer, ref other, ref index);
            //    if (pLayer == null)
            //        return;

            //    if (pItem == esriTOCControlItem.esriTOCControlItemLayer)
            //    {
            //        if (pLayer.GetType() is IAnnotationSublayer) return;
            //        else
            //        {
            //            _SelSymLayer = pLayer;
            //        }
            //    }
            //}
            IBasicMap pMap = null;
            ILayer pLayer = null;
            System.Object other = null;
            System.Object LayerIndex = null;
            System.Drawing.Point pPoint = new System.Drawing.Point(e.x, e.y);

            esriTOCControlItem TOCItem = esriTOCControlItem.esriTOCControlItemNone;
            ITOCControl2 tocControl = (ITOCControl2)axTOCControl.Object;

            tocControl.HitTest(e.x, e.y, ref TOCItem, ref pMap, ref pLayer, ref other, ref LayerIndex);
            if (e.button == 2 && GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu != null)
            {
                DevComponents.DotNetBar.ButtonItem item = null;
                DevComponents.DotNetBar.ContextMenuBar menuBar = GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu["TOCContextMenu2"];
                DevComponents.DotNetBar.ContextMenuBar menuBarLayer = GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu["TOCLayerContextMenu2"];
                this.Controls.Add(menuBar);
                this.Controls.Add(menuBarLayer);
                switch (TOCItem)
                {
                    case esriTOCControlItem.esriTOCControlItemMap:
                        if (GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu.ContainsKey("TOCContextMenu2"))
                        {
                            if (GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu["TOCContextMenu2"] != null)
                            {
                                if (GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu["TOCContextMenu2"].Items.Count > 0)
                                {
                                    item = GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu["TOCContextMenu2"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                                    if (item != null)
                                    {
                                        item.Popup(axTOCControl.PointToScreen(pPoint));
                                    }
                                }
                            }
                        }
                        break;
                    case esriTOCControlItem.esriTOCControlItemLayer:

                        if (!(pLayer is IGroupLayer || pLayer is IFeatureLayer || pLayer is IDataLayer)) return;
                        if (pLayer is IFeatureLayer || pLayer is IDataLayer)
                        {
                            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                            if (GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu.ContainsKey("TOCLayerContextMenu2"))
                            {
                                if (GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu["TOCLayerContextMenu2"] != null)
                                {
                                    if (GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu["TOCLayerContextMenu2"].Items.Count > 0)
                                    {
                                        item = GeoDBIntegration.ModuleData.v_AppDBIntegra.DicContextMenu["TOCLayerContextMenu2"].Items[0] as DevComponents.DotNetBar.ButtonItem;
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

        private void axTOCControl_OnMouseMove(object sender, ITOCControlEvents_OnMouseMoveEvent e)
        {
            //IBasicMap pMap = new MapClass();
            //ILayer pLayer = new FeatureLayerClass();
            //object pOther = new object();
            //esriTOCControlItem pItem = new esriTOCControlItem();
            //object pIndex = new object();   
            ////实现调整图层顺序功能
            //if(e.button == 1)
            //{
            //    axTOCControl.HitTest(e.x, e.y, ref pItem, ref pMap, ref pLayer, ref pOther, ref pIndex);    
            //}
            //if(pItem != esriTOCControlItem.esriTOCControlItemNone)
            //{
            //    Icon icon = new Icon( "icon.ico");
            //    axTOCControl.MouseIcon = icon;
            //    axTOCControl.MousePointer = esriControlsMousePointer.esriPointerCustom;
            //}

        }

        private void axTOCControl_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        {
            //esriTOCControlItem pItem = new esriTOCControlItem();
            //IBasicMap pMap = new MapClass(); 
            //ILayer pLayer = new FeatureLayerClass(); 
            //object pOther = new object();
            //object pIndex = new object();
            //int i,j;
            //bool bUpdataToc;
            //axTOCControl.MousePointer = esriControlsMousePointer.esriPointerArrow;
            ////实现调整图层顺序功能
            //if(e.button == 1)
            //{
            //    axTOCControl.HitTest(e.x, e.y, ref pItem, ref pMap, ref pLayer, ref pOther, ref pIndex);
            //}

            //if (pItem == esriTOCControlItem.esriTOCControlItemLayer || pItem == esriTOCControlItem.esriTOCControlItemLegendClass)
            //{
            //    if (pLayer == null || _SelSymLayer == null || _SelSymLayer == pLayer)
            //        return;
            //    if (e.button == 1)
            //    {
            //        int Toindex = -1;
            //        for (i = 0; i < pMap.LayerCount; i++)
            //        {
            //            ILayer pLayTmp;
            //            pLayTmp = pMap.get_Layer(i);
            //            //得到点击当前的索引值
            //            if (pLayer == pLayTmp)
            //            {
            //                Toindex = i;
            //                break ;
            //            }
            //        }
            //        if (Toindex > 0)
            //        {
            //            ((IMap)pMap).MoveLayer(_SelSymLayer, i);
            //            (MapControlLayer.Map as IActiveView).Refresh();
            //        }
            //    }
            //}
            
        }


        #region 目录管理右键菜单
        public string  GetTagOfSeletedNode()
        {
            return this.layerTree.GetTagOfSeletedNode();
        }
        //设置数据源
        public void SetDbSource()
        {
            this.layerTree.SetDbSource();
        }
        //修改节点
        public void ModifyNode()
        {
            this.layerTree.ModifyNode();
        }
        //删除节点
        public void DeleteNode()
        {
            this.layerTree.DeleteTreeNode();
            this.layerTree.GetDeleteNodeKeys(ref _DeleteNodeKeys);
        }
        public void SaveDataRight()
        {
            SysGisTable pTable = new SysGisTable(m_TmpWorkSpace);
            Exception err = null;
            for (int i = 0; i < _DeleteNodeKeys.Count; i++)
            {
                string strNodeKey = _DeleteNodeKeys[i];
                pTable.DeleteRows("role_Data", "DATAPRI_ID='" + strNodeKey + "'", out err);
            }
            pTable = null;
                
        }
        //自动匹配图层设置
        public void AutoMathLayerConfig()
        {
            this.layerTree.AutoMathLayerConfig();
        }
        //添加图层
        public void AddLayer()
        {
            this.layerTree.AddLayer();
        }
        public void AddWMSserviceLayer()
        {
            this.layerTree.AddWMSserviceLayer();
        }
        //添加图层组
        public void AddGroup()
        {
            this.layerTree.AddGroup();
        }
        //添加专题
        public void AddFolder()
        {
            this.layerTree.AddFolder();
        }
        //添加数据集
        public void AddDataSet()
        {
            this.layerTree.AddDataSet();
        }
        //缩放到图层
        public void ZoomToLayer()
        {
            this.layerTree.ZoomToLayer();
        }
        //批量设置属性
        public void SetLayerAttributes()
        {
            this.layerTree.SetLayerAttributes();
        }
        //是否可以粘贴图层
        public bool CanPasteLayer()
        {
            return this.layerTree.CanPasteLayer();
        }
        //复制图层
        public void CopyLayer()
        {
            this.layerTree.CopyLayer();
        }
        //粘贴图层
        public void PasteLayer()
        {
            this.layerTree.PasteLayer();
        }
        #endregion

        private void buttonItemMapScan_Click(object sender, EventArgs e)
        {
            if (buttonItemMapScan.Checked == false)
            {
                buttonItemMapScan.Checked = true;
                ChangeLayerVisible(true);
                //Plugin.LogTable.Writelog("图层浏览");//xisheng 2011.07.09 增加日志   deleted by chulili 2012-09-07 山西只记录重要操作的日志
            }
            else
            {
                buttonItemMapScan.Checked = false;
                ChangeLayerVisible(false);
            }
        }
        //保存目录之前，处理每个图层的OrderID
        //如果map中后一个图层的顺序号小于前一个图层，则后一个顺序号修改为前一个顺序号+0.01（为避免重复）
        //DealLayerOrderID函数与SetOrderIDofAllLayer函数先后执行，配合使用，完成保存目录之前对图层的顺序号进行处理

        //DealLayerOrderID函数完成对map中当前加载的图层进行顺序号的重新设置，依据是图层见的上下顺序
        //SetOrderIDofAllLayer函数完成对xml中的图层节点的顺序号进行重新赋值，由小到大都赋以整数值
        //RefreshOrderIDofLayer函数刷新map中图层的顺序号
        public void DealLayerOrderID()
        {
            IMap pMap=this.MapControlLayer.Map;
            if (pMap.LayerCount == 0) return;
            //先处理第一个图层
            ILayer pLayer = pMap.get_Layer(0);
            string strOrderid = GetOrderIDofLayer(pLayer);
            int intOrderid = -1;
            if (!strOrderid.Equals(""))
            {
                intOrderid = int.Parse(strOrderid);
            }
            if (intOrderid < 0)
            {
                SetOrderIDofLayer(pLayer,0.01);//第一个层如果没有有效的顺序号，则赋值0.01
            }
            for (int i = 0; i < pMap.LayerCount - 1; i++)
            {
                ILayer pTmplayer = pMap.get_Layer(i);
                if (pTmplayer is IGroupLayer)
                {
                    DealLayerOrderIDofGroupLayer(pTmplayer as IGroupLayer);//如果是图层组类型，则先对图层组内部进行处理 
                }
            }
            for (int i = 0; i < pMap.LayerCount-1; i++)
            {
                ILayer pLayeri = pMap.get_Layer(i);
                ILayer pLayerj = pMap.get_Layer(i + 1);
                string strOrderID_i = GetOrderIDofLayer(pLayeri );
                string strOrderID_j = GetOrderIDofLayer(pLayerj );
                double dOrderID_i = -1;
                double dOrderID_j = -1;
                if (!strOrderID_i.Equals(""))
                    dOrderID_i = double.Parse(strOrderID_i);
                if (!strOrderID_j.Equals(""))
                    dOrderID_j = double.Parse(strOrderID_j );
                //如果后一个图层顺序号小于前一个图层，则重新设置后一个顺序号
                if (dOrderID_j < dOrderID_i)
                {
                    dOrderID_j = dOrderID_i + 0.01;//+0.01是为了防止顺序号重复，原本顺序号都是整型
                    SetOrderIDofLayer(pLayerj,dOrderID_j );
                }

            }
        }
        //设置图层组内部图层间的顺序
        private void SetOrderIDofLayerInGroup(XmlDocument pXmldoc, XmlNode pGroupNode)
        {
            if (pGroupNode == null) return;
            if (pXmldoc == null) return;
            XmlNodeList pXmlnodelist = null;
            pXmlnodelist = pGroupNode.SelectNodes("//Layer");

            SetOrderIDofList(pXmlnodelist, "Layer", pXmldoc);
        }
        //供调用的函数
        //对集合中的图层进行顺序号的整理和统一赋值，按先后顺序赋整数值
        private void SetOrderIDofList(XmlNodeList pXmlnodelist, string strLayerType, XmlDocument pXmldoc)
        {
            if (pXmlnodelist == null) return;
            if (pXmldoc == null) return;
            if (pXmlnodelist.Count == 0) return;
            IDictionary<double, string> pDicofLayer = new Dictionary<double, string>();
            List<double> ListOrderID = new List<double>();
            for (int i = 0; i < pXmlnodelist.Count; i++)
            {
                XmlNode pNode = pXmlnodelist[i];
                XmlElement pEle = pNode as XmlElement;
                //获取图层唯一标识
                string strNodeKey = "";
                if (pEle.HasAttribute("NodeKey"))
                {
                    strNodeKey = pEle.GetAttribute("NodeKey");
                }
                //获取图层顺序号
                string strOrderID = "";
                if (pEle.HasAttribute("OrderID"))
                {
                    strOrderID = pEle.GetAttribute("OrderID");
                }
                //将图层唯一标识按照顺序号存入字典
                if (!strOrderID.Equals(""))
                {
                    double dOrderID = double.Parse(strOrderID);
                    while (pDicofLayer.Keys.Contains(dOrderID))
                    {
                        dOrderID = dOrderID + 0.0001;
                    }
                    pDicofLayer.Add(dOrderID, strNodeKey);
                    ListOrderID.Add(dOrderID);
                    if (strLayerType.ToUpper().Equals("GROUPLAYER"))//如果是图层组，则对内部图层再次执行设置顺序号函数
                    {
                        SetOrderIDofLayerInGroup(pXmldoc, pNode);
                    }
                }
            }
            int intSetCurOrderID = 1;
            //对图层的顺序号进行排序
            for (int i = 0; i < ListOrderID.Count - 1; i++)
            {
                double dCurOrderID = ListOrderID[i];
                int ItemOfMin = i;
                for (int j = i + 1; j < ListOrderID.Count; j++)
                {
                    if (ListOrderID[j] < dCurOrderID)
                    {
                        dCurOrderID = ListOrderID[j];
                        ItemOfMin = j;
                    }
                }
                double tmpOrderID = ListOrderID[i];
                ListOrderID[i] = dCurOrderID;
                ListOrderID[ItemOfMin] = tmpOrderID;
            }
            //按照顺序号由小到大取出图层，对图层重新赋予顺序号（赋整型顺序号）
            for (int i = 0; i < ListOrderID.Count; i++)
            {
                double dTmpOrderID = ListOrderID[i];
                string strNodeKey = pDicofLayer[dTmpOrderID];

                SetOrderIDofLayer(strNodeKey, intSetCurOrderID, strLayerType);

                intSetCurOrderID = intSetCurOrderID + 1;
            }
        }
        //为xml中所有图层节点设置OrderID，仅针对已经有OrderID的节点
        public void SetOrderIDofAllLayer(string strLayerType)
        {
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(ModPublicFun._layerTreePath);
            
            //获取所有Layer节点
            XmlNodeList pXmlnodelist = null;
            if (strLayerType.ToUpper().Equals("LAYER"))
            {
                pXmlnodelist = pXmldoc.SelectNodes("//Layer");
            }
            else
            {
                pXmlnodelist = pXmldoc.SelectNodes("//DataDIR");
            }
            SetOrderIDofList(pXmlnodelist, strLayerType, pXmldoc);

        }
        //刷新单个层的图层顺序号，支持图层组类型以及图层类型
        public void RefreshOrderIDofLayer(ILayer pLayer,XmlDocument pXmldoc)
        {
            if (pLayer == null) return;
            ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
            //读取图层的描述
            string strNodeXml = pLayerGenPro.LayerDescription;
            if (strNodeXml == "")
            {
                return;
            }
            XmlDocument pTmpXmlDoc = new XmlDocument();
            pTmpXmlDoc.LoadXml(strNodeXml);
            //构成xml节点，根据NodeKey在节点里查询
            XmlNode pNode = pTmpXmlDoc.ChildNodes[0];
            string strNodeKey = pNode.Attributes["NodeKey"].Value.ToString();
            string strSearch = "";
            if (pLayer is IGroupLayer)
            {
                strSearch = "//DataDIR [@NodeKey='" + strNodeKey + "']";
            }
            else
            {
                strSearch = "//Layer [@NodeKey='" + strNodeKey + "']";
            }
            pTmpXmlDoc = null;
            XmlNode pNewLayerNode = pXmldoc.SelectSingleNode(strSearch);
            if (pNewLayerNode != null)
            {
                pLayerGenPro.LayerDescription = pNewLayerNode.OuterXml;
                this.layerTree.RefreshOrderIDofLayer(strNodeKey, pNewLayerNode.OuterXml);
            }
            
        }
        //刷新所有map中图层的顺序号，刷新成与xml中图层的顺序号一致
        public void RefreshOrderIDofAllLayer()
        {
            XmlDocument pNewXmldoc = new XmlDocument();
            pNewXmldoc.Load(ModPublicFun._layerTreePath);
            for (int i = 0; i < MapControlLayer.Map.LayerCount; i++)
            {
                ILayer pLayer = MapControlLayer.Map.get_Layer(i);
                //刷新当前图层的顺序号
                RefreshOrderIDofLayer(pLayer, pNewXmldoc);
                //针对图层组做特殊处理
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                    if (pComLayer != null)
                    {
                        for (int j = 0; j < pComLayer.Count; j++)
                        {
                            ILayer pTmpLayer = pComLayer.get_Layer(j);
                            RefreshOrderIDofLayer(pTmpLayer, pNewXmldoc);
                        }
                    }
                }
            }
        }
        //处理图层组内部图层间的顺序号
        //如果map中后一个图层的顺序号小于前一个图层，则后一个顺序号修改为前一个顺序号+0.01（为避免重复）
        private void DealLayerOrderIDofGroupLayer(IGroupLayer pGroupLayer)
        {
            if (pGroupLayer == null)
                return;
            ICompositeLayer pComLayer = pGroupLayer as ICompositeLayer;
            if (pComLayer != null)
            {
                if (pComLayer.Count == 0) return;
                //先处理第一个图层
                ILayer pLayer = pComLayer.get_Layer(0);
                string strOrderid = GetOrderIDofLayer(pLayer);
                double dOrderid = -1;
                if (!strOrderid.Equals(""))
                {
                    dOrderid = double.Parse(strOrderid);
                }
                if (dOrderid < 0)
                {
                    SetOrderIDofLayer(pLayer, 0.01);
                }
                for (int i = 0; i < pComLayer.Count - 1; i++)
                {
                    ILayer pLayeri = pComLayer.get_Layer(i);
                    ILayer pLayerj = pComLayer.get_Layer(i + 1);
                    string strOrderID_i = GetOrderIDofLayer(pLayeri);
                    string strOrderID_j = GetOrderIDofLayer(pLayerj);
                    double dOrderID_i = -1;
                    double dOrderID_j = -1;
                    if (!strOrderID_i.Equals(""))
                        dOrderID_i = double.Parse(strOrderID_i);
                    if (!strOrderID_j.Equals(""))
                        dOrderID_j = double.Parse(strOrderID_j);
                    //如果后一个图层顺序号小于前一个，则重新设置后一个图层顺序号
                    if (dOrderID_j < dOrderID_i)
                    {
                        dOrderID_j = dOrderID_i + 0.01;//+0.01是为了防止顺序号重复，原本顺序号都是整型
                        SetOrderIDofLayer(pLayerj, dOrderID_j);
                    }

                }
            }
        }
        //为图层赋顺序号，此处是双精度型
        private void SetOrderIDofLayer(ILayer pLayer,double dOrderID)
        {
            try
            {
                ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
                //读取图层的描述
                string strNodeXml = pLayerGenPro.LayerDescription;
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.LoadXml(strNodeXml);
                //构成xml节点，根据NodeKey在节点里查询
                XmlNode pNode = pXmlDoc.ChildNodes[0];
                string strNodeKey = pNode.Attributes["NodeKey"].Value.ToString();
                pXmlDoc = null;
                //为图层设置顺序号，双精度型
                XmlDocument pnewXmldoc = new XmlDocument();
                pnewXmldoc.Load(ModPublicFun._layerTreePath);
                string strSearch = "";
                if (pLayer is IGroupLayer)
                {   //若是图层组，则取数据集节点
                    strSearch = "//DataDIR [@NodeKey='" + strNodeKey + "']";
                }
                else
                {   //若是单图层，则取图层节点
                    strSearch = "//Layer [@NodeKey='" + strNodeKey + "']";
                }
                XmlNode pLayerNode = pnewXmldoc.SelectSingleNode(strSearch);
                XmlElement pLayerEle = pLayerNode as XmlElement;
                if (pLayerEle!= null)
                {
                    pLayerEle.SetAttribute("OrderID",dOrderID.ToString());
                }
                pLayerGenPro.LayerDescription = pLayerNode.OuterXml;//这一句很重要，将修改的顺序号保存回去
                pnewXmldoc.Save(ModPublicFun._layerTreePath);
            }
            catch
            {
            }
        }
        //为图层赋顺序号，此处是整型，这个顺序号是最终保存到xml中的顺序号
        public void SetOrderIDofLayer(string strNodeKey, int intOrderID,string strLayerType)
        {
            //为图层设置顺序号，双精度型
            XmlDocument pnewXmldoc = new XmlDocument();
            pnewXmldoc.Load(ModPublicFun._layerTreePath);
            string strSearch = "";
            //查找NodeKey对应的图层节点
            strSearch = "";
            if (strLayerType.ToUpper().Equals("LAYER"))
            {
                strSearch = "//Layer [@NodeKey='" + strNodeKey + "']";
            }
            else
            {
                strSearch = "//DataDIR [@NodeKey='" + strNodeKey + "']";
            }

            XmlNode pLayerNode = pnewXmldoc.SelectSingleNode(strSearch);
            XmlElement pLayerEle = pLayerNode as XmlElement;
            if (pLayerEle != null)
            {   //为图层赋顺序号
                pLayerEle.SetAttribute("OrderID", intOrderID.ToString());
            }
            pnewXmldoc.Save(ModPublicFun._layerTreePath);
        }
        //added by chulili 20110731 获取图层顺序号，返回字符型，为了同时适应整形和双精度型
        private string GetOrderIDofLayer(ILayer pLayer)
        {
            try
            {
                ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
                //读取图层的描述
                string strNodeXml = pLayerGenPro.LayerDescription;
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.LoadXml(strNodeXml);
                //构成xml节点，根据NodeKey在节点里查询
                XmlNode pNode = pXmlDoc.ChildNodes[0];
                string strOrder = "";
                try
                {
                    strOrder = pNode.Attributes["OrderID"].Value.ToString();
                }
                catch
                { }
                return strOrder;
            }
            catch
            {
            }
            return "";

        }

        private void MapControlLayer_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            
            if (m_AppFrmRef != null)
            {
                double TempX = double.Parse(e.mapX.ToString("0.00"));
                double TempY = double.Parse(e.mapY.ToString("0.00"));
                m_AppFrmRef.CoorXY = "X:" + TempX + "   Y:" + TempY;
                
            }
        }
    }
}
