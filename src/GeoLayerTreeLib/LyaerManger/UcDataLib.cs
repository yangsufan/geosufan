using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using DevComponents.AdvTree;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;

using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using SysCommon.Gis;


namespace GeoLayerTreeLib.LayerManager
{

    public partial class UcDataLib : UserControl
    {
        //当前Map中已经添加的图层
        private  IDictionary<string, ILayer> _DicAddLyrs = new Dictionary<string, ILayer>();
        //当前Map中添加后又删除的图层
        private  IDictionary<string, ILayer> _DicDelLyrs = new Dictionary<string, ILayer>();
        //added by chulili当前Map中已经添加的图层组
        private IDictionary<string, IGroupLayer> _DicAddGroupLyrs = new Dictionary<string, IGroupLayer>();
        //added by chulili 当前Map中添加后又删除的图层组
        private IDictionary<string, IGroupLayer> _DicDelGroupLyrs = new Dictionary<string, IGroupLayer>();
        private IDictionary<string, object > _LayerDicOfGroupLayer = new Dictionary<string,object>();
        private bool _isLayerVisible = true;//配置图层目录时，判断树图是否与浏览视图连动，不连动为false连动为true
        private DevComponents.DotNetBar.ContextMenuBar _MapContextMenu = null;
        private DevComponents.DotNetBar.ContextMenuBar _LayerContextMenu = null;
        private DevComponents.DotNetBar.ContextMenuBar _LayerTreeManageContextMenu = null;


        //added by chulili 20111119 为复制黏贴图层添加的变量
        private DevComponents.AdvTree.Node _CopyLayerNode = null;
        //end added by chulili 20111119
        //added by chulili 20111119 是否可以粘贴图层
        public bool CanPasteLayer()
        {
            if (_CopyLayerNode == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public AdvTree DataTree//yjl20110915 add for pagelayout
        {
            get { return TreeDataLib; }
        }
        public bool LayerVisible
        {
            set { _isLayerVisible = value; }
        }
        public DevComponents.DotNetBar.ContextMenuBar MapContextMenu
        {
            set { _MapContextMenu = value; }
        }
        public DevComponents.DotNetBar.ContextMenuBar LayerContextMenu
        {
            set { _LayerContextMenu = value; }
        }
        public DevComponents.DotNetBar.ContextMenuBar LayerTreeManageContextMenu
        {
            set { _LayerTreeManageContextMenu = value; }
        }
        public IDictionary<string, ILayer> DicAddLyrs
        {
            set { _DicAddLyrs = value; }
        }
        public IDictionary<string, ILayer> DicDelLyrs
        {
            set { _DicDelLyrs = value; }
        }
        //ZQ  20110827   add   判断图层目录树是否可拖拽
        public bool isDragDrop
        {
            set { this.TreeDataLib.DragDropEnabled = value; }
        }
        //
        public UcDataLib()
        {
            InitializeComponent();
            InitUC();
        }
        private bool _isLayerConfig=false;//是否在配置图层目录，默认非配置，即正在展示子系统
        public bool isLayerConfig
        {
            set { _isLayerConfig = value; }
        }

        //单前Map
        private IMapControl2 m_pMapCtl;
        public IMapControl2 CurMap
        {
            set { m_pMapCtl = value; }
        }
        private ITOCControlDefault _TocControl;
        public ITOCControlDefault TocControl
        {
            set { _TocControl = value; }
        }
        //当前Toc
        //private ITocControl m_pToc;
        //public ITocControl CurToc
        //{
        //    set { m_pToc = value; }
        //}

        private IWorkspace m_pWks;
        public IWorkspace CurWks
        {
            set { m_pWks = value; }
        }
        private String _LayerXmlPath;
        public String LayerXmlPath
        {
            set { _LayerXmlPath = value; }
        }
        //changed by chulili 移除变量m_vDataViewXml，仅使用xml全路径表示即可
        //private XmlDocument m_vDataViewXml;
        //public XmlDocument DataViewXml
        //{
        //    set { m_vDataViewXml = value; }
        //    get { return m_vDataViewXml; }
        //}

        public Dictionary<string, DevComponents.DotNetBar.ContextMenuBar> DicMenu
        {
            get{ return m_vDicMenu;}
            set { m_vDicMenu = value; }
        }

        //右键菜单集合
        private Dictionary<string, DevComponents.DotNetBar.ContextMenuBar> m_vDicMenu = null;
        private void BoundMenu()
        {

        }
        
        private void InitUC()
        {
            

            this.TreeDataLib.Nodes.Clear();
            this.TreeDataLib.ImageList = this.ImageList;
            

            //
        }

        public void InitDataView()
        {
            //if (m_vDataViewXml == null) return;
            //changed by chulili 修改判断方式，移除变量m_vDataViewXml
            if (!System.IO.File.Exists(_LayerXmlPath)) return;
        }
        //初始化图层名字典（英文名到中文名的映射）
        public void InitLayerDic()
        {
           SysCommon.ModField.InitLayerNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicLayerName);
            
         }
        #region 初始化树图代码
        public void InitDBsource()
        {
            ModuleMap._DicDataLibWks.Clear();
            //获得xml
            ModuleMap.InitDBSourceDic(Plugin.ModuleCommon.TmpWorkSpace, ModuleMap._DicDataLibWks);
        }
        public void LoadDataByMap(XmlDocument pDataXmlDoc, IMap pMXDMap)
        {
            if (pMXDMap == null) { return; }
            //初始化目录节点
            LoadTreeByXml(pDataXmlDoc, TreeDataLib, true, false);
            IObjectCopy pOC = null;
            if (pMXDMap.LayerCount > 0)
            {
                for (int x = pMXDMap.LayerCount - 1; x >= 0; x--)
                {
                    try
                    {
                        pOC = new ObjectCopyClass();
                        ILayer pLayer = pOC.Copy(pMXDMap.get_Layer(x)) as ILayer;
                        ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
                        //读取图层的描述
                        string strNodeXml = pLayerGenPro.LayerDescription;
                        XmlDocument pXmlDoc = new XmlDocument();
                        pXmlDoc.LoadXml(strNodeXml);
                        //获取节点的NodeKey信息
                        XmlNode pxmlnode = pXmlDoc.SelectSingleNode("//Layer");
                        if (pxmlnode == null)
                        {
                            continue;
                        }
                        string strNodeKey = pxmlnode.Attributes["NodeKey"].Value.ToString();
                        //构成xml节点，根据NodeKey在节点里查询
                        string strSearch = "//Layer[@NodeKey=" + "'" + strNodeKey + "'" + "]";
                        XmlNode pNode = pDataXmlDoc.SelectSingleNode(strSearch);
                        if (pNode != null && pLayer.Valid && pLayer.Visible)
                        {
                            m_pMapCtl.Map.AddLayer(pLayer);
                        }
                    }
                    catch { }
                }
                //循环处理map中的图层
                for (int i = m_pMapCtl.Map.LayerCount - 1; i >= 0; i--)
                {
                    ILayer pLayer = m_pMapCtl.Map.get_Layer(i);
                    OpenMxdDocDealLayer(pLayer);
                }
            }
        }
        public void LoadDataByMxd(XmlDocument pXmldoc, string strLayerTreeMxdPath)
        {
            
            if (System.IO.File.Exists(strLayerTreeMxdPath)) //判断文件是否存在
            {
                //初始化目录节点
                LoadTreeByXml(pXmldoc, TreeDataLib, true, false);
                if (m_pMapCtl.CheckMxFile(strLayerTreeMxdPath)) //文件是否能正确打开
                {
                    //m_pMapCtl.LoadMxFile(strLayerTreeMxdPath, "", "");  //加载文件
                    IMapDocument pMapdoc = new MapDocumentClass();
                    pMapdoc.Open(strLayerTreeMxdPath, "");
                    IObjectCopy pOC = new ObjectCopyClass();
                    if (pMapdoc.MapCount > 0)
                    {
                        IMap pMap = pMapdoc.get_Map(0);
                        m_pMapCtl.Map = pOC.Copy(pMap) as IMap;
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pMapdoc);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pOC);
                    pMapdoc = null;
                    pOC = null;
                    //对目录节点状态进行同步
                    //清空节点打勾状态
                    //for (int i = 0; i < this.TreeDataLib.Nodes.Count; i++)
                    //{
                    //    DevComponents.AdvTree.Node pNode = TreeDataLib.Nodes[i];
                    //    UnSelectAllAdvNode(pNode);
                    //}

                    //循环处理map中的图层
                    for (int i = m_pMapCtl.Map.LayerCount-1; i>=0; i--)
                    {
                        ILayer pLayer = m_pMapCtl.Map.get_Layer(i);
                        OpenMxdDocDealLayer(pLayer);
                    }
                }
            }           
        }
        public void RefreshDataByMap(XmlDocument pDoc,IMap pMap)
        {
            _DicAddLyrs.Clear();
            _DicDelLyrs.Clear();
            _DicAddGroupLyrs.Clear();
            _DicDelGroupLyrs.Clear();
            _LayerDicOfGroupLayer.Clear();
            m_pMapCtl.ClearLayers();
            LoadDataByMap(pDoc, pMap);
        }
        public void LoadData()
        {
            _DicAddLyrs.Clear();            
            _DicDelLyrs.Clear();
            _DicAddGroupLyrs.Clear();
            _DicDelGroupLyrs.Clear();
            _LayerDicOfGroupLayer.Clear();
            m_pMapCtl.ClearLayers();
            //m_vDicMenu = ModData.v_AppGisUpdate.DicContextMenu;
            
            XmlDocument xmldoc = new XmlDocument();
            if (!System.IO.File.Exists(_LayerXmlPath)) return;
            xmldoc.Load(_LayerXmlPath);

            bool bReadMxd = false;
            string LayerTreeMxdPath = Application.StartupPath + "\\..\\Template\\展示图层树.mxd";
            if (!_isLayerConfig)    //展示系统采用读取MXD的模式，配置系统采用读取XML的模式 chulili 20111105
            {
                bReadMxd = SysCommon.ModSysSetting.CopySysSettingtoFile(Plugin.ModuleCommon.TmpWorkSpace, "初始加载地图文档", LayerTreeMxdPath);
            }
            if (bReadMxd)   //读取图层目录的MXD文件成功
            {
                LoadDataByMxd(xmldoc, LayerTreeMxdPath);
            }
            else
            {
                ModuleMap.ChangeLableEngine2MapLex(m_pMapCtl as IMapControlDefault);
                if (_TocControl != null)
                {
                    _TocControl.SetBuddyControl(null);//yjl20111102 add 加载数据前 解绑TOC
                }
                m_pMapCtl.ActiveView.Deactivate();  //取消刷新 added by chulili 20111102
                LoadTreeByXml(xmldoc, this.TreeDataLib, true,true);
                ModuleMap.LayersComposeEx(m_pMapCtl as IMapControlDefault);

                m_pMapCtl.ActiveView.Activate(m_pMapCtl.hWnd);  //启动刷新 added by chulili 20111102
                if (_TocControl != null)
                {
                    _TocControl.SetBuddyControl(m_pMapCtl);//yjl20111102 add 加载数据后 绑定TOC（200图层效率快1倍）
                }
                //added by chulili 20110628图层排序后，刷新一下toc控件 
                if (_TocControl != null)
                {
                    _TocControl.Update();
                }
                //ICommand _cmd = new ESRI.ArcGIS.Controls.ControlsMapFullExtentCommand();
                //_cmd.OnCreate(m_pMapCtl);
                //_cmd.OnClick();
                IEnvelope pEnv = SysCommon.ModSysSetting.GetFullExtent(m_pWks); //changed by chulili 20111107使用系统参数进行全屏
                if (pEnv != null)
                {
                    m_pMapCtl.Extent = pEnv;
                }
                else
                {
                    ICommand _cmd = new ESRI.ArcGIS.Controls.ControlsMapFullExtentCommand();
                    _cmd.OnCreate(m_pMapCtl);
                    _cmd.OnClick();
                    _cmd = null;
                }
                pEnv = null;
            }
            
            //菜单绑定
            BoundMenu();
            //编写加载外部数据触发的视图事件
            if (!_isLayerConfig)
            {
                IMap pMap = m_pMapCtl.Map;//主视图
                IActiveViewEvents_Event pAE = pMap as IActiveViewEvents_Event;
                pAE.ItemAdded += new IActiveViewEvents_ItemAddedEventHandler(this.OnLayerAdded);
            }
        }
        private void OnLayerAdded(object item)
        {
            ILayer pLayer;
            pLayer = item as ILayer;
            ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
            //读取该图层的描述信息，转成xml节点
            string strNodeXml = pLayerGenPro.LayerDescription;
            if (!strNodeXml.Contains("NodeKey"))
            {//this.TreeDataLib
                //创建并设定图层节点
                DevComponents.AdvTree.Node treenodeOutLayer = new DevComponents.AdvTree.Node();
                treenodeOutLayer.Name = "OutLayer";
                treenodeOutLayer.Text = pLayer.Name;
                treenodeOutLayer.Tag = "OutLayer";
                treenodeOutLayer.DataKey = pLayer as object;
                treenodeOutLayer.Expanded = true;
                if (pLayer is IFeatureLayer)
                {
                    IFeatureLayer pFeaLayer = pLayer as IFeatureLayer;
                    if (pFeaLayer != null)
                    {
                        if (pFeaLayer.FeatureClass != null)
                        {
                            if (pFeaLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                                treenodeOutLayer.Image = this.ImageList.Images["_annotation"];
                            else if(pFeaLayer.FeatureClass.FeatureType==esriFeatureType.esriFTDimension)
                                treenodeOutLayer.Image = this.ImageList.Images["_Dimension"];
                            else
                            {
                                switch (pFeaLayer.FeatureClass.ShapeType)
                                {
                                    case esriGeometryType.esriGeometryPoint:
                                        treenodeOutLayer.Image = this.ImageList.Images["_point"];
                                        break;
                                    case esriGeometryType.esriGeometryPolyline:
                                        treenodeOutLayer.Image = this.ImageList.Images["_line"];
                                        break;
                                    case esriGeometryType.esriGeometryPolygon:
                                        treenodeOutLayer.Image = this.ImageList.Images["_polygon"];
                                        break;
                                    case esriGeometryType.esriGeometryMultiPatch:
                                        treenodeOutLayer.Image = this.ImageList.Images["_MultiPatch"];
                                        break;
                                    default:
                                        treenodeOutLayer.Image = this.ImageList.Images["Layer"];
                                        break;
                                }
                            }
                        }
                    }
                }
                else 
                {
                    treenodeOutLayer.Image = this.ImageList.Images["Layer"];
                }
                this.TreeDataLib.Nodes[0].Nodes.Add(treenodeOutLayer);
            }

        }
        private XmlDocument GetXmlDoc()
        {
            if (m_pWks == null) return null;

            SysCommon.Gis.SysGisTable systable = new SysCommon.Gis.SysGisTable(m_pWks);
            Exception ex = null;

            object objtemp = systable.GetFieldValue("SysSetting", "SettingValue2", "SettingName='DataViewXml'", out ex);
            systable = null;
            if (objtemp == null) return null ;

            return objtemp as XmlDocument;
        }
        //折叠
        public void CollapseAllNode()
        {
            this.TreeDataLib.CollapseAll();
        }
        //展开
        public void ExpendAllNode()
        {
            this.TreeDataLib.ExpandAll();
        }
        //移过来的代码 需要验证
        public void LoadTreeByXml(XmlDocument xmldoc,AdvTree Tree, bool IsInConfig,bool LoadData)
        {
            if (xmldoc == null) return;
            Tree.Nodes.Clear();

            //获取Xml的根节点并作为根节点加到UltraTree上
            XmlNode xmlnodeRoot = xmldoc.DocumentElement;
            XmlElement xmlelementRoot = xmlnodeRoot as XmlElement;

            xmlelementRoot.SetAttribute("NodeKey", "Root");
            string sNodeText = xmlelementRoot.GetAttribute("NodeText");

            //创建并设定树的根节点
            DevComponents.AdvTree.Node treenodeRoot = new DevComponents.AdvTree.Node();
            treenodeRoot.Name="Root";
            treenodeRoot.Text=sNodeText;

            if (IsInConfig)
            {
                //treenodeRoot.Override.NodeStyle = NodeStyle.CheckBox;

                if (xmlelementRoot.Attributes["IsCollection"] != null &&
                    xmlelementRoot.Attributes["IsCollection"].Value.ToLower() == "true" && LoadData)
                    treenodeRoot.CheckState = CheckState.Checked;
                else
                    treenodeRoot.CheckState = CheckState.Unchecked;
            }
            treenodeRoot.Tag = "Root";
            treenodeRoot.DataKey = xmlelementRoot;
            treenodeRoot.Expanded = true;
            Tree.Nodes.Add(treenodeRoot);

            //treenodeRoot.LeftImages.Add("Root");
            treenodeRoot.Image = treenodeRoot.TreeControl.ImageList.Images["Root"];
            LoadTreeNodeByXmlNode(treenodeRoot, xmlnodeRoot, IsInConfig, LoadData);
            //if (IsInConfig)
            //    LoadTreeNodeByXmlNode(treenodeRoot, xmlnodeRoot, IsInConfig);
            //else
            //    LoadTreeNodeByXmlNode(treenodeRoot, xmlnodeRoot, Tree, clsMain);
        }

        public void LoadTreeNodeByXmlNode(DevComponents.AdvTree.Node treenode, XmlNode xmlnode, bool IsInConfig,bool LoadData)
        {

            for (int iChildIndex = 0; iChildIndex < xmlnode.ChildNodes.Count; iChildIndex++)
            {
                XmlElement xmlElementChild = xmlnode.ChildNodes[iChildIndex] as XmlElement;
                if (xmlElementChild == null)
                {
                    continue;
                }
                if (IsInConfig)
                {
                    if (xmlElementChild.Attributes["Enabled"] == null ||
                        xmlElementChild.Attributes["Enabled"].Value.ToLower() == "false")
                        continue;
                }
                else
                {
                    if (xmlElementChild.Attributes["IsCollection"] == null ||
                        xmlElementChild.Attributes["IsCollection"].Value.ToLower() == "false")
                        continue;
                }

                //用Xml子节点的"NodeKey"和"NodeText"属性来构造树子节点
                string sNodeKey = xmlElementChild.GetAttribute("NodeKey");
                if (!_isLayerConfig)
                {
                    if (!Plugin.ModuleCommon.AppUser.Name.Equals("admin"))
                    {
                        if (Plugin.ModuleCommon.ListUserdataPriID == null)//changed by xisheng 2011.06.29
                        {
                            continue;
                        }
                        if (!Plugin.ModuleCommon.ListUserdataPriID.Contains(sNodeKey))
                        {
                            continue;
                        }
                    }
                }
                string sNodeText = xmlElementChild.GetAttribute("NodeText");

                DevComponents.AdvTree.Node treenodeChild = new DevComponents.AdvTree.Node();
                treenodeChild.Name = sNodeKey;
                treenodeChild.Text = sNodeText;

                if (IsInConfig || xmlElementChild.Name == "DataDIR")
                {
                    //treenodeChild.Override.NodeStyle = NodeStyle.CheckBox;
                    treenodeChild.CheckBoxVisible = true;

                    if (xmlElementChild.Attributes["IsCollection"] != null &&
                    xmlElementChild.Attributes["IsCollection"].Value.ToLower() == "true" && LoadData)
                        treenodeChild.CheckState = CheckState.Checked;
                    else
                        treenodeChild.CheckState = CheckState.Unchecked;
                }
                
                treenodeChild.DataKey = xmlElementChild;
                treenodeChild.Tag = xmlElementChild.Name;
                if (xmlElementChild.Attributes["Expand"] != null &&
                    xmlElementChild.Attributes["Expand"].Value.ToLower() == "true")
                {
                    treenodeChild.Expanded = true;
                }

                treenode.Nodes.Add(treenodeChild);
                //changed by chulili 20110708删除加载数据集的代码，一开始不加载，如果子节点加载则加载
                //if (xmlElementChild.Name == "DataDIR")
                //{
                //    //创建组节点
                //    IGroupLayer pGroupFLayer = new GroupLayer();                    
                //    XmlElement pele = xmlnode.ChildNodes[iChildIndex] as XmlElement ;

                //    pGroupFLayer.Name = sNodeText;
                //    _DicAddGroupLyrs.Add(sNodeKey, pGroupFLayer);
                //    pGroupFLayer.Visible = _isLayerVisible;
                //    m_pMapCtl.AddLayer(pGroupFLayer, m_pMapCtl.LayerCount);
                //}
                //加载数据 下面这段代码需要单独提出来
                bool NodeVisible = false;
                if (LoadData)
                {
                    #region 加载图层
                    if (xmlElementChild.Name == "Layer")
                    {

                        ILayer pLyr = null;
                        bool blnLoad = XmlOperation.GetBoolean(xmlnode.ChildNodes[iChildIndex], false, "Load");
                        if (blnLoad)
                        {
                            IGroupLayer pParentGroupLayer = null;
                            string sParentNodeKey = "";
                            XmlNode pParePareXmlnode = xmlnode.ParentNode;
                            XmlNode pDirNode = null;
                            XmlNode pGroupLayerXmlNode = null;
                            if (xmlnode.Name == "DataDIR" && pParePareXmlnode.Name == "DIR")
                            {
                                pDirNode = pParePareXmlnode;
                                pGroupLayerXmlNode = xmlnode;
                            }
                            else
                            {
                                pDirNode = pParePareXmlnode.ParentNode;
                                pGroupLayerXmlNode = pParePareXmlnode;
                            }
                            #region 图层组处理，先注释掉
                            //if (pGroupLayerXmlNode.Name == "DataDIR")
                            //{
                            //    sParentNodeKey = pGroupLayerXmlNode.Attributes["NodeKey"].Value.ToString();
                            //    if (_DicAddGroupLyrs.Keys.Contains(sParentNodeKey))
                            //    {
                            //        pParentGroupLayer = _DicAddGroupLyrs[sParentNodeKey];
                            //    }
                            //    else
                            //    {
                            //       //创建组节点
                            //        IGroupLayer pGroupFLayer = new GroupLayerClass ();
                            //        string sPareNodeText = pGroupLayerXmlNode.Attributes["NodeText"].Value.ToString();
                            //        //changed by chulili 20110728 添加上数据库名称
                            //        if (pDirNode != null)
                            //        {
                            //            if (pDirNode.Name.Equals("DIR"))
                            //            {
                            //                sPareNodeText = sPareNodeText + "_" + pDirNode.Attributes["NodeText"].Value;
                            //            }
                            //        }
                            //        //end changed by chulili 20110728 添加上数据库名称
                            //        pGroupFLayer.Name = sPareNodeText;
                            //        _DicAddGroupLyrs.Add(sParentNodeKey, pGroupFLayer);
                            //        pGroupFLayer.Visible = _isLayerVisible;
                            //        //added by chulili 20110728 添加图层描述
                            //        ILayerGeneralProperties layerProerties = (ILayerGeneralProperties)pGroupFLayer;
                            //        layerProerties.LayerDescription = pGroupLayerXmlNode.OuterXml;
                            //        //end added by chulili
                            //        m_pMapCtl.AddLayer(pGroupFLayer, m_pMapCtl.LayerCount);
                            //        pParentGroupLayer = _DicAddGroupLyrs[sParentNodeKey];
                            //    }
                            //}
                            # endregion
                            Exception errLayer = null;
                            bool boolLoad = XmlOperation.GetBoolean(xmlnode.ChildNodes[iChildIndex], false, "Load");
                            if (boolLoad)
                            {
                                pLyr = ModuleMap.AddLayerFromXml(ModuleMap._DicDataLibWks, xmlnode.ChildNodes[iChildIndex], m_pWks, "", null, out errLayer);
                            } 
                                //加载到Map中去
                            
                            if (pLyr != null && boolLoad)
                            {
                                //将数据加到图层集合中
                                if (!_DicAddLyrs.Keys.Contains(sNodeKey))
                                {
                                    _DicAddLyrs.Add(sNodeKey, pLyr);
                                }

                                //added by chulili 2011-06-11设置图层是否可见(默认可见，配置图层目录时可以设置成不可见)
                                if (_isLayerConfig)
                                {
                                    pLyr.Visible = _isLayerVisible;
                                }
                                #region 图层组处理，先注释掉
                                //if (pParentGroupLayer != null)
                                //{
                                //    IMapLayers pmaplayers = m_pMapCtl.Map as IMapLayers;
                                //    ICompositeLayer Comlayer = pParentGroupLayer as ICompositeLayer;
                                //    pmaplayers.InsertLayerInGroup(pParentGroupLayer, pLyr, false, Comlayer.Count);
                                //    IDictionary<string, ILayer> pDic = null;
                                //    if (_LayerDicOfGroupLayer.Keys.Contains(sParentNodeKey))
                                //    {
                                //        pDic = _LayerDicOfGroupLayer[sParentNodeKey] as IDictionary<string,ILayer >;
                                //        pDic.Add(sNodeKey, pLyr);
                                //    }
                                //    else
                                //    {
                                //        pDic = new Dictionary<string, ILayer >();
                                //        pDic.Add(sNodeKey, pLyr);
                                //        _LayerDicOfGroupLayer.Add(sParentNodeKey, pDic as object );
                                //    }
                                //}
                                //else
                                //{
                                #endregion
                                m_pMapCtl.AddLayer(pLyr, m_pMapCtl.LayerCount);
                                //}
                                m_pMapCtl.ActiveView.Refresh();
                                //改变树图状态
                                treenodeChild.CheckState = CheckState.Checked;
                                NodeVisible =ModuleMap.GetVisibleOfLayer(m_pMapCtl.Map.MapScale, pLyr);
                            }
                        }
                    }
                    #endregion
                }

                if (LoadData)
                {
                    //向上设置复选框
                    SetParentCheckState(treenodeChild);
                }

                //递归
                LoadTreeNodeByXmlNode(treenodeChild, xmlElementChild as XmlNode, IsInConfig, LoadData);
                if (LoadData)
                {
                    if (xmlElementChild.Name.CompareTo("DataDIR") == 0)
                    {
                        SetUltraTreeNodeImage(treenodeChild);
                    }
                }
                InitializeNodeImage(treenodeChild, NodeVisible);
            }

        }
        public void UpdateLayerNodeImage()
        {
            if (m_pMapCtl == null)
            {
                return;
            }
            for (int i = 0; i < m_pMapCtl.LayerCount; i++)
            {
                ILayer pLayer = m_pMapCtl.get_Layer(i);
                if (pLayer is IFeatureLayer || pLayer is IRasterLayer || pLayer is IRasterCatalog)
                {
                    ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
                    //读取图层的描述，转成xml节点
                    string strNodeXml = pLayerGenPro.LayerDescription;

                    if (strNodeXml.Equals(""))
                    {
                        continue ;
                    }
                    XmlDocument pXmldoc = new XmlDocument();
                    pXmldoc.LoadXml(strNodeXml);
                    //获取节点的NodeKey信息
                    XmlNode pxmlnode = pXmldoc.SelectSingleNode("//Layer");
                    if (pxmlnode == null)
                    { 
                        continue;
                    }
                    string strNodeKey = pxmlnode.Attributes["NodeKey"].Value.ToString();
                    pXmldoc = null;
                    //获取图层对应的节点
                    DevComponents.AdvTree.Node pnode = SearchLayerNodebyName(strNodeKey);
                    //获取图层是否真正可见
                    bool nodeVisible = ModuleMap.GetVisibleOfLayer(m_pMapCtl.Map.MapScale, pLayer);
                    //修改图层对应节点的图标
                    InitializeNodeImage(pnode, nodeVisible);
                }
            }
        }
    
        /// <summary>  
        /// 设定DataDIR节点的图标，注意从跟DataDIR节点起，不用考虑其子DataDIR节点
        /// 算法：将对应DataDIR下的Layer或者DataDIR节点的数值与子节点数目比较
        /// 相等则为全开，互补则为关闭，否则为半开
        /// 数值的算法为：开为加一，关为减一，半开（子DtatDIR）为零
        /// </summary>
        /// <param name="nodeDataDIR"></param>
        private static void SetUltraTreeNodeImage(DevComponents.AdvTree.Node nodeDataDIR)
        {
            int iNumber = 0;
            foreach (DevComponents.AdvTree.Node nodeChildTree in nodeDataDIR.Nodes)
            {

                if (nodeChildTree.HasChildNodes == false)
                {
                    if (nodeChildTree.CheckBoxVisible ==true)
                    {
                        if (nodeChildTree.CheckState == CheckState.Checked)
                            iNumber++;  //子节点选中，加一
                        else if (nodeChildTree.CheckState == CheckState.Unchecked)
                            iNumber--;  //子节点未选中，减一
                    }
                    else
                        iNumber++;  //特殊情况,DataDIR节点无子节点
                }
                else
                {
                    SetUltraTreeNodeImage(nodeChildTree);
                    if (("DataDIR&AllOpened").CompareTo(nodeChildTree.Tag) == 0)
                        iNumber++;          //子dataDIR全开，加一
                    else if (("DataDIR&Closed").CompareTo(nodeChildTree.Tag) == 0)
                        iNumber--;          //子dataDIR关，减一
                    else
                        iNumber = iNumber + 0;  //子dataDIR半开，不变
                }
            }
            if (iNumber == nodeDataDIR.Nodes.Count)
            {
                nodeDataDIR.Tag = "DataDIR&AllOpened";
            }
            else if ((iNumber + nodeDataDIR.Nodes.Count) == 0)
            {
                nodeDataDIR.Tag = "DataDIR&Closed";
            }
            else
            {
                nodeDataDIR.Tag = "DataDIR&HalfOpened";
            }
        }
        //added by chulili 20111119 函数重载
        public static void InitializeNodeImage(DevComponents.AdvTree.Node treenode)
        {
            InitializeNodeImage(treenode, false);
        }
        /// <summary>
        /// 通过传入节点的tag，选择对应的图标        
        /// </summary>
        /// <param name="treenode"></param>
        /// changed by chulili 20111119 添加一个参数，该节点是否可见（仅用于图层节点）
        public static void InitializeNodeImage(DevComponents.AdvTree.Node treenode,bool NodeVisible)
        {
            switch (treenode.Tag.ToString())
            {
                case "Root":
                    treenode.Image = treenode.TreeControl.ImageList.Images["Root"];
                    treenode.CheckBoxVisible = false;
                    break;
                case "SDE":
                    treenode.Image = treenode.TreeControl.ImageList.Images["SDE"];
                    break;
                case "PDB":
                    treenode.Image = treenode.TreeControl.ImageList.Images["PDB"];
                    break;
                case "FD":
                    treenode.Image = treenode.TreeControl.ImageList.Images["FD"];
                    break;
                case "FC":
                    treenode.Image = treenode.TreeControl.ImageList.Images["FC"];
                    break;
                case "TA":
                    treenode.Image = treenode.TreeControl.ImageList.Images["TA"];
                    break;
                case "DIR":
                    treenode.Image = treenode.TreeControl.ImageList.Images["DIR"];
                    //treenode.CheckBoxVisible = false;
                    break;
                case "DataDIR":
                    treenode.Image = treenode.TreeControl.ImageList.Images["DataDIRClosed"];
                    break;
                case "DataDIR&AllOpened":
                    treenode.Image = treenode.TreeControl.ImageList.Images["DataDIROpen"];
                    break;
                case "DataDIR&Closed":
                    treenode.Image = treenode.TreeControl.ImageList.Images["DataDIRClosed"];
                    break;
                case "DataDIR&HalfOpened":
                    treenode.Image = treenode.TreeControl.ImageList.Images["DataDIRHalfOpen"];
                    break;
                case "Layer":
                    XmlNode xmlnodeChild = (XmlNode)treenode.DataKey;
                    if (xmlnodeChild != null && xmlnodeChild.Attributes["FeatureType"] != null)
                    {
                        string strFeatureType = xmlnodeChild.Attributes["FeatureType"].Value;
                        if (NodeVisible)
                        {
                            switch (strFeatureType)
                            {
                                case "esriGeometryPoint":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["point_v"];
                                    break;
                                case "esriGeometryPolyline":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["line_v"];
                                    break;
                                case "esriGeometryPolygon":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["polygon_v"];
                                    break;
                                case "esriFTAnnotation":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["annotation_v"];
                                    break;
                                case "esriFTDimension":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["dimension_v"];
                                    break;
                                case "esriGeometryMultiPatch":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["multipatch_v"];
                                    break;
                                default:
                                    treenode.Image = treenode.TreeControl.ImageList.Images["layer_v"];
                                    break;
                            }
                        }
                        else
                        {
                            switch (strFeatureType)
                            {
                                case "esriGeometryPoint":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["point"];
                                    break;
                                case "esriGeometryPolyline":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["line"];
                                    break;
                                case "esriGeometryPolygon":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["polygon"];
                                    break;
                                case "esriFTAnnotation":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["annotation"];
                                    break;
                                case "esriFTDimension":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["dimension"];
                                    break;
                                case "esriGeometryMultiPatch":
                                    treenode.Image = treenode.TreeControl.ImageList.Images["multipatch"];
                                    break;
                                default:
                                    treenode.Image = treenode.TreeControl.ImageList.Images["layer"];
                                    break;
                            }
                        }
                    }
                    else
                    {
                        treenode.Image = treenode.TreeControl.ImageList.Images["Layer"];
                    }
                    break;
                case "RC":
                    if (NodeVisible)
                    {
                        treenode.Image = treenode.TreeControl.ImageList.Images["layer_v"];//RC->Layer  没有名称为RC的图片
                    }
                    else
                    {
                        treenode.Image = treenode.TreeControl.ImageList.Images["layer"];//RC->Layer  没有名称为RC的图片
                    }
                    break;
                case "RD":
                    if (NodeVisible)
                    {
                        treenode.Image = treenode.TreeControl.ImageList.Images["layer_v"];//RD->Layer  没有名称为RD的图片
                    }
                    else
                    {
                        treenode.Image = treenode.TreeControl.ImageList.Images["layer"];//RD->Layer  没有名称为RD的图片
                    }
                    break;
                case "SubType":
                    treenode.Image = treenode.TreeControl.ImageList.Images["SubType"];
                    break;
                default:
                    break;
            }//end switch
        }
        #endregion

        private void TreeDataLib_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            DevComponents.AdvTree.Node vNode = e.Node as DevComponents.AdvTree.Node;
            TreeDataLib.SelectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                if (_isLayerConfig)//如果是在配置目录，弹出配置右键菜单
                {
                    return;
                }
                PopMenu(e.Node);
            }
            else if (e.Button == MouseButtons.Left)
            {
                
            }
        }

        //处理右键菜单
        private void PopMenu(DevComponents.AdvTree.Node vNode)
        {
            if (m_vDicMenu == null) return;
            DevComponents.DotNetBar.ContextMenuBar tempBar = null;
            switch (vNode.Tag.ToString())
            {
                case "Root":
                    m_vDicMenu.TryGetValue("ContextMenuDataViewTreeRoot", out tempBar);
                    break;
                case "Layer":
                    break;
            }

            if (tempBar == null) return;
            if (tempBar.Items.Count < 1) return;

            //this.Controls.Add(tempBar);
            //vNode.ContextMenu = this.contextMenuBar1;
            //this.contextMenuBar1.Show();
        }

        //移除某个图层（通常用户修改图层信息时候，先将旧图层移除，再将修改后的新图层加入）
        public void RemoveLayer(DevComponents.AdvTree.Node pNode)
        {
            if (pNode == null)
            {
                return;
            }
            if (!pNode.Tag.ToString().Equals("Layer"))
            {
                return;
            }
            XmlNode layerNode = pNode.DataKey as XmlNode;
            if (layerNode == null)
            {
                return;
            }
            string nodeKey = layerNode.Attributes["NodeKey"].Value;

            if (_DicDelLyrs.ContainsKey(nodeKey))
                _DicDelLyrs.Remove(nodeKey);

            if (_DicAddLyrs.ContainsKey(nodeKey))
            {
                
                ILayer removeLayer = _DicAddLyrs[nodeKey];
                m_pMapCtl.Map.DeleteLayer(removeLayer);
                _DicAddLyrs.Remove(nodeKey);
            }


        }
        //移除某个图层组
        private void RemoveDataDIRfromMap(DevComponents.AdvTree.Node pNode)
        {
            if (pNode == null)
            {
                return;
            }
            if (!pNode.Tag.ToString().Contains("DataDIR"))
            {
                return;
            }
            XmlNode layerNode = pNode.DataKey as XmlNode;
            if (layerNode == null)
            {
                return;
            }
            string nodeKey = layerNode.Attributes["NodeKey"].Value;

            if (_DicDelGroupLyrs.ContainsKey(nodeKey))
                _DicDelGroupLyrs.Remove(nodeKey);

            if (_DicAddGroupLyrs.ContainsKey(nodeKey))
            {

                ILayer removeLayer = _DicAddGroupLyrs[nodeKey];
                m_pMapCtl.Map.DeleteLayer(removeLayer);
                _DicAddGroupLyrs.Remove(nodeKey);
            }


        }
        //在视图浏览中添加或卸载图层
        private void AddOrDelLyr(DevComponents.AdvTree.Node checkNode, bool blnChecked)
        {
            //added by chulili 20110722 添加进度条
            SysCommon.CProgress vProgress = null;

            XmlNode layerNode = checkNode.DataKey as XmlNode;
            XmlElement Layerele = layerNode as XmlElement;
            XmlDocument pXmldoc = null;
            //added by chulili 20110708 读取最新xml配置文件
            pXmldoc = new XmlDocument();
            pXmldoc.Load(_LayerXmlPath);
            //add end
            string nodeKey = layerNode.Attributes["NodeKey"].Value;
            //added by chulili 20110708 从最新xml文件中找到当前图层节点
            string strSearch = "//" + layerNode.Name + "[@NodeKey='" + nodeKey + "']";
            XmlNode pdocLayerNode = pXmldoc.SelectSingleNode(strSearch);

            IGroupLayer pParentGroupLayer = null;
            ICompositeLayer Comlayer = null;
            XmlNode GroupNode = layerNode.ParentNode;
            IDictionary<string, ILayer> pDic = null;
            string sParentNodeKey = "";
            //if (GroupNode.Name == "DataDIR")
            //{
            //    XmlElement pParentele = GroupNode as XmlElement;
            //    sParentNodeKey = pParentele.GetAttribute("NodeKey");
            //    if (_DicAddGroupLyrs.Keys.Contains(sParentNodeKey))
            //    {
            //        pParentGroupLayer = _DicAddGroupLyrs[sParentNodeKey];
            //    }
            //    else
            //    {
            //        if (_DicDelGroupLyrs.Keys.Contains(sParentNodeKey))
            //        {
            //            pParentGroupLayer = _DicDelGroupLyrs[sParentNodeKey];
            //        }
            //        else
            //        {
            //            if (blnChecked)
            //            {
            //                //创建组节点
            //                IGroupLayer pGroupFLayer = new GroupLayerClass ();
            //                string sPareNodeText = pParentele.GetAttribute("NodeText");
            //                //changed by chulili 20110728 添加上数据库名称
            //                XmlNode pDIRnode = pParentele.ParentNode;
            //                if (pDIRnode != null)
            //                {
            //                    if (pDIRnode.Name.Equals("DIR"))
            //                    {
            //                        sPareNodeText = sPareNodeText + "_" + pDIRnode.Attributes["NodeText"].Value;
            //                    }
            //                }
            //                //end changed by chulili 20110728 添加上数据库名称
            //                pGroupFLayer.Name = sPareNodeText;
            //                _DicAddGroupLyrs.Add(sParentNodeKey, pGroupFLayer);
            //                pGroupFLayer.Visible = _isLayerVisible;
            //                //added by chulili 20110728 添加图层描述
            //                ILayerGeneralProperties layerProerties = (ILayerGeneralProperties)pGroupFLayer;
            //                layerProerties.LayerDescription = pParentele.OuterXml;
            //                //end added by chulili
            //                m_pMapCtl.AddLayer(pGroupFLayer, m_pMapCtl.LayerCount);
            //                pParentGroupLayer = _DicAddGroupLyrs[sParentNodeKey];
            //            }
            //        }
            //    }
            //    if (pParentGroupLayer != null)
            //    {
            //        Comlayer = pParentGroupLayer as ICompositeLayer;                 
            //    }

            //    if (_LayerDicOfGroupLayer.Keys.Contains(sParentNodeKey))
            //    {
            //        pDic = _LayerDicOfGroupLayer[sParentNodeKey] as IDictionary<string, ILayer>;
            //    }
            //    else
            //    {
            //        pDic = new Dictionary<string, ILayer>();
            //        _LayerDicOfGroupLayer.Add(sParentNodeKey, pDic as object);
            //    }
            //}
            if (blnChecked)
            {
                vProgress = new SysCommon.CProgress("加载'" + checkNode.Text + "'");
                vProgress.EnableCancel = false;
                vProgress.ShowDescription = true;
                vProgress.ShowProgressNumber = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
            }
            //end added by chulili 
            if (blnChecked)
            {
                //if (pParentGroupLayer != null)//added by chulili 改bug
                //{
                //    if (!_DicAddGroupLyrs.Keys.Contains(sParentNodeKey))
                //    {

                //        m_pMapCtl.AddLayer(pParentGroupLayer, 0);
                //        IMapLayers pmaplayers = m_pMapCtl.Map as IMapLayers;
                //        foreach (string strkey in pDic.Keys)
                //        {
                //            ILayer player = pDic[strkey];
                //            pmaplayers.DeleteLayer(player);
                //        }
                //        pDic.Clear();
                //        _DicAddGroupLyrs.Add(sParentNodeKey, pParentGroupLayer);
                //    }
                //}
                //if (_DicDelGroupLyrs.Keys.Contains(sParentNodeKey))
                //{
                //    _DicDelGroupLyrs.Remove(sParentNodeKey);
                //}
                XmlElement pLayerEle = layerNode as XmlElement;
                if (pLayerEle != null)
                {
                    pLayerEle.SetAttribute("Load", "1");
                }
                //layerNode.Attributes["Load"].Value = "1";


                //layerNode.Attributes["View"].Value = "1";
                //added by chulili 20110708 修改最新xml文件
                XmlElement pDocLayerEle = pdocLayerNode as XmlElement;
                if (pDocLayerEle != null)
                {
                    pDocLayerEle.SetAttribute("Load", "1");
                }
                //pdocLayerNode.Attributes["Load"].Value = "1";

                //pdocLayerNode.Attributes["View"].Value = "1";
                vProgress.SetProgress(50, "正在加载...");//added by chulili 20110722
                //add end 
                ILayer addLayer = null;
                Exception errLayer = null;
                if (_DicDelLyrs.ContainsKey(nodeKey))
                    addLayer = _DicDelLyrs[nodeKey];
                else
                    addLayer = ModuleMap.AddLayerFromXml(ModuleMap._DicDataLibWks, layerNode, m_pWks, "", null,out errLayer );
                //shduan 20110801 退出时关闭进度条
                if (addLayer == null)
                {
                    vProgress.Close();
                    return;
                }
                int insertIndex = 0;// ModuleMap.GetControlLayerIndex(layerNode, m_SystemData);
                //设置图层是否可见（默认可见，在配置图层目录时可以设置成不可见，避免刷新）
                if (_isLayerConfig)
                {
                    addLayer.Visible = _isLayerVisible;
                }
                //if (pParentGroupLayer != null)
                //{
                //    IMapLayers pmaplayers = m_pMapCtl.Map as IMapLayers;
                //    pmaplayers.InsertLayerInGroup(pParentGroupLayer, addLayer, false, Comlayer.Count );
                //    //关于该图层组的字典中加入该图层
                //    if (!pDic.Keys.Contains(nodeKey))
                //    {
                //        pDic.Add(nodeKey, addLayer);
                //    }
                //}
                //else
                //{
                    m_pMapCtl.AddLayer(addLayer, insertIndex);
                //}
                ModuleMap.DealOrderOfNewLayer(m_pMapCtl as IMapControlDefault,addLayer);

                //added by chulili 20110628图层排序后，刷新一下toc控件 
                if (_TocControl != null)
                {
                    _TocControl.Update();
                }
                if (_DicDelLyrs.ContainsKey(nodeKey))
                    _DicDelLyrs.Remove(nodeKey);
                if (_DicAddLyrs.ContainsKey(nodeKey) == false)
                    _DicAddLyrs.Add(nodeKey, addLayer);

            }
            else
            {
                //layerNode.Attributes["Load"].Value = "0";

                XmlElement tmpLayerEle = layerNode as XmlElement;   //替换赋值方法 chulili 20111103
                tmpLayerEle.SetAttribute("Load", "0");

                //layerNode.Attributes["View"].Value = "0";
                //added by chulili 20110708 修改最新xml文件

                XmlElement tmpDocLayerEle = pdocLayerNode as XmlElement;    //替换赋值方法 chulili 20111103
                tmpDocLayerEle.SetAttribute("Load", "0");

                //pdocLayerNode.Attributes["Load"].Value = "0";
                //pdocLayerNode.Attributes["View"].Value = "0";
                //add end 
                if (_DicAddLyrs.ContainsKey(nodeKey))
                {
                    ILayer removeLayer = _DicAddLyrs[nodeKey];
                    m_pMapCtl.Map.DeleteLayer(removeLayer);

                    _DicAddLyrs.Remove(nodeKey);

                    //(removeLayer as ILayerGeneralProperties).LayerDescription = layerNode.OuterXml;
                    if (_DicDelLyrs.ContainsKey(nodeKey) == false)
                        _DicDelLyrs.Add(nodeKey, removeLayer);
                    //if (pDic != null)
                    //{
                    //    if (pDic.Keys.Contains(nodeKey))
                    //    {
                    //        pDic.Remove(nodeKey);
                    //    }
                    //}
                }
            }
            InitializeNodeImage(checkNode, blnChecked);
            pXmldoc.Save(_LayerXmlPath);
			if (_isLayerConfig)
            {
                SysCommon.ModSysSetting.IsLayerTreeChanged = true;
            }
            
            SetParentCheckState(checkNode.Parent);
            if (blnChecked)
            {   //added by chulili 20110722关闭进度条
                vProgress.SetProgress(100, "加载完毕");
                vProgress.Close();
            }
            //added by chulili 20110714 判断父节点是否需从toc中删除
            if (!blnChecked && !checkNode.Parent.Checked)
            {
                if (checkNode.Parent.Tag != "Root")
                {
                    if (checkNode.Parent.DataKey != null)
                    {
                        AddOrDelDataDir(checkNode.Parent, checkNode.Parent.CheckState);
                    }
                }
            }
            //end add
        }
        public void AddDataDir(string strNodeKey)
        {
            if(strNodeKey.Equals(""))
            {
                return;
            }
            //根据NodeKey查找节点
            DevComponents.AdvTree.Node pDataDirNode= SearchLayerNodebyName(strNodeKey);
            if (pDataDirNode == null)
                return;
            //如果无法从节点获取xml信息
            if (pDataDirNode.DataKey == null)
            {
                return;
            }
            if (pDataDirNode.Tag.ToString().Contains("DataDIR") == false) 
                return;
            //调用函数,添加改数据集节点
            if (!pDataDirNode.Checked)
            {
                pDataDirNode.Checked = true;
                AddOrDelDataDir(pDataDirNode, pDataDirNode.CheckState );
            }
        }
        //处理DataDir
        private void AddOrDelDataDir(DevComponents.AdvTree.Node checkNode, CheckState checkState)
        {
            //if (checkNode.Tag.ToString().Contains("DataDIR") == false) return;
            //if (checkNode.Tag.ToString().Contains("DIR") == false) return;

            //deleted by chulili checkState由外部传入，不必进行修改
            //if (IsChildAllUnCheck(checkNode))
            //    checkState = CheckState.Checked;
            //else
            //    checkState = CheckState.Unchecked;
            //end delete
            //向上、向下设置节点状态
            //deleted by chulili 20111117  用户中途可以取消加载或卸载过程，所以一开始不要全部修改节点的Check状态
            //if (checkState==CheckState.Unchecked)
            //{
            //    WhileNodeStateChange(checkNode, checkState.ToString());
            //    SetChildCheckState(checkNode);
            //    SetParentCheckState(checkNode.Parent);
            //}
            //end deleted by chulili 
            XmlNode xmlNode = checkNode.DataKey as XmlNode;
            if (xmlNode != null)
            {
                xmlNode.OwnerDocument.Load(_LayerXmlPath);
            }
            XmlElement xmlele = xmlNode as XmlElement;
            string GroupNodeKey = xmlele.Attributes["NodeKey"].Value;
            //added by chulili 20110708 读取最新xml配置文件
            XmlDocument  pXmldoc = new XmlDocument();
            pXmldoc.Load(_LayerXmlPath);
            XmlNode pDocLayerNode = null;
            //add end
             //添加或删除图层组节点
            //IGroupLayer DealGroupLayer = null;
            //if (checkState == CheckState.Checked)
            //{
                //if (_DicDelGroupLyrs.ContainsKey(GroupNodeKey))
                //    DealGroupLayer = _DicDelGroupLyrs[GroupNodeKey];
                //else
                //{
                //    DealGroupLayer = new GroupLayerClass();
                //    //changed by chulili 20110728 添加上数据库名称
                //    XmlNode pDIRnode = xmlele.ParentNode;

                //    string GroupLayertxt = xmlele.Attributes["NodeText"].Value;
                //    if (pDIRnode != null)
                //    {
                //        if (pDIRnode.Name.Equals("DIR"))
                //        {
                //            GroupLayertxt = GroupLayertxt + "_" + pDIRnode.Attributes["NodeText"].Value ;
                //        }
                //    }
                //    //end changed by chulili 20110728 添加上数据库名称
                //    DealGroupLayer.Name = GroupLayertxt;
                //}
                //int insertIndex = 0;
                //if (DealGroupLayer == null) return;                
                //added by chulili 20110728 添加图层描述
                //ILayerGeneralProperties layerProerties = (ILayerGeneralProperties)DealGroupLayer;
                //layerProerties.LayerDescription = xmlele.OuterXml;
                //end added by chulili
                //设置图层是否可见（默认可见，配置图层目录时可以设置成不可见，避免刷新）
                //DealGroupLayer.Visible = _isLayerVisible;
                //m_pMapCtl.AddLayer(DealGroupLayer, insertIndex);

                //if (_DicDelGroupLyrs.ContainsKey(GroupNodeKey))
                //    _DicDelGroupLyrs.Remove(GroupNodeKey);
                //if (_DicAddGroupLyrs.ContainsKey(GroupNodeKey) == false)
                //    _DicAddGroupLyrs.Add(GroupNodeKey, DealGroupLayer);
            //}
            //else
            //{
                //if (_DicAddGroupLyrs.ContainsKey(GroupNodeKey))
                //{
                //    DealGroupLayer = _DicAddGroupLyrs[GroupNodeKey];
                //    m_pMapCtl.Map.DeleteLayer(DealGroupLayer);

                //    _DicAddGroupLyrs.Remove(GroupNodeKey);

                //    (DealGroupLayer as ILayerGeneralProperties).LayerDescription = xmlele.OuterXml;
                //    if (_DicDelGroupLyrs.ContainsKey(GroupNodeKey) == false)
                //        _DicDelGroupLyrs.Add(GroupNodeKey, DealGroupLayer);
                //}
            //}
            //IDictionary<string, ILayer> pDic = null;
            //if (_LayerDicOfGroupLayer.Keys.Contains(GroupNodeKey))
            //{
            //    pDic = _LayerDicOfGroupLayer[GroupNodeKey] as IDictionary<string, ILayer>;
            //}
            //else
            //{
            //    pDic = new Dictionary<string, ILayer>();
            //    _LayerDicOfGroupLayer.Add(GroupNodeKey, pDic as object);
            //}
            //ICompositeLayer Comlayer = DealGroupLayer as ICompositeLayer;
            XmlNodeList layerList = xmlNode.SelectNodes(".//Layer");
            IMapLayers pmaplayers = m_pMapCtl.Map as IMapLayers;
            //added by chulili 20110722 添加进度条
            SysCommon.CProgress vProgress = null;
            string strLoadInfo = "";
            if (checkState == CheckState.Checked)
            {
                if (_TocControl != null)
                {
                    _TocControl.SetBuddyControl(null);//yjl20111102 add 加载数据前 解绑TOC
                }
                SysCommon.ModSysSetting.WriteLog("Deactivate start");
                m_pMapCtl.ActiveView.Deactivate();
                SysCommon.ModSysSetting.WriteLog("Deactivate end");
                strLoadInfo = "加载";
            }
            else
            {
                strLoadInfo = "卸载";
            }
            //changed by chulili 20111117 加载或卸载都要进度条
            vProgress = new SysCommon.CProgress(strLoadInfo+"'" + checkNode.Text + "'");
            //vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.ShowProgressNumber = true;
            vProgress.TopMost = true;
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);
            vProgress.ShowProgress();
            List<string> ListLayerError = new List<string>();
            //end added by chulili 
            //图层操作
            for (int i = 0; i < layerList.Count; i++)
            {
                //联动
                //LinkNodeToControlTree(layerList[i], checkState);
                //LinkNodeToFavoriteTree(layerList[i]);

                string nodeKey = layerList[i].Attributes["NodeKey"].Value;                
                string strSearch = "//" + layerList[i].Name + "[@NodeKey='" + nodeKey + "']";
                pDocLayerNode = pXmldoc.SelectSingleNode(strSearch );
                if (!_isLayerConfig)
                {
                    if (!Plugin.ModuleCommon.AppUser.Name.Equals("admin"))
                    {
                        if (Plugin.ModuleCommon.ListUserdataPriID == null)//changed by xisheng 2011.06.29
                        {
                            continue;
                        }
                        if (!Plugin.ModuleCommon.ListUserdataPriID.Contains(nodeKey))
                        {
                            continue;
                        }
                    }
                }
                if (vProgress.UserAskCancel)
                {
                    break;
                }
                string layertxt = "";
                if (pDocLayerNode != null)
                {
                    layertxt = pDocLayerNode.Attributes["NodeText"].Value.ToString();
                }
                //added by chulili 20110722
                double di = double.Parse((i + 1).ToString());

                vProgress.SetProgress(Convert.ToInt32(di / (layerList.Count) * 100), strLoadInfo+ "'" + layertxt + "'图层");
                DevComponents.AdvTree.Node pCurrentNode = GetLayerNodeByNodeKey(checkNode, nodeKey);
                if (checkState == CheckState.Checked)               
                {
                    SysCommon.ModSysSetting.WriteLog("开始加载" + layertxt);                  
                    
                    XmlElement tmpLayeritemEle = layerList[i] as XmlElement;    //替换赋值方法 chulili 20111103
                    if (tmpLayeritemEle != null)
                    {
                        tmpLayeritemEle.SetAttribute("Load", "1");
                    }

                    //layerList[i].Attributes["Load"].Value = "1";
                    //layerList[i].Attributes["View"].Value = "1";
                    //added by chulili 20110708 修改最新xml文件

                    XmlElement tmpDocLayerEle = pDocLayerNode as XmlElement;    //替换赋值方法,chulili 20111103
                    if (tmpDocLayerEle != null)
                    {
                        tmpDocLayerEle.SetAttribute("Load", "1");
                    }
                    //if (pDocLayerNode != null)
                    //{
                    //    pDocLayerNode.Attributes["Load"].Value = "1";
                    //}
                    //pDocLayerNode.Attributes["View"].Value = "1";
                    //add end 
                    ILayer addLayer = null;
                    
                    //fmPgs.lblOut.Text = "加载'" + layertxt + "'图层";
                    Exception errLayer=null;
                    if (_DicDelLyrs.ContainsKey(nodeKey))
                    {
                        addLayer = _DicDelLyrs[nodeKey];
                    }
                    else
                    {   
                        addLayer = ModuleMap.AddLayerFromXml(ModuleMap._DicDataLibWks, layerList[i], m_pWks, "", null,out errLayer);
                    }
                    if (addLayer == null)
                    {
                        if(errLayer!=null)
                        {
                            ListLayerError.Add(layertxt+"加载失败，失败原因："+errLayer.Message);
                        }
                        if (pCurrentNode != null)
                        {
                            pCurrentNode.Checked = false;
                            SetParentCheckState(pCurrentNode);
                        }
                        continue;
                    }
                    //moved by chulili 20111118 代码挪动位置，确定已经获取到图层后，再在节点上打勾
                    if (pCurrentNode != null)
                    {
                        pCurrentNode.Checked = true;
                        SetParentCheckState(pCurrentNode);
                    }
                    //end moved by chulili 20111118
                    if (_isLayerConfig)
                    {
                        addLayer.Visible = _isLayerVisible;
                    }
                    SysCommon.ModSysSetting.WriteLog("GetIndexOfNewLayer start");
                    //changed by chulili 20111117 把加载和调整顺序整合成一个步骤，减少刷新
                    int NewLayerIndex = ModuleMap.GetIndexOfNewLayer(m_pMapCtl as IMapControlDefault, addLayer);
                    SysCommon.ModSysSetting.WriteLog("AddLayer start");
                    m_pMapCtl.AddLayer(addLayer, NewLayerIndex);
                    //end changed by chulili 20111117
                    //ModuleMap.DealOrderOfNewLayer(m_pMapCtl as IMapControlDefault, addLayer);

                    //m_SystemData.frmProgress.ShowProgress("正在加载" + addLayer.Name + "数据...", (int)((i + 1) * 100 / layerList.Count));

                    if (_DicDelLyrs.ContainsKey(nodeKey))
                        _DicDelLyrs.Remove(nodeKey);
                    if (_DicAddLyrs.ContainsKey(nodeKey) == false)
                        _DicAddLyrs.Add(nodeKey, addLayer);
                    
                    SysCommon.ModSysSetting.WriteLog("加载完毕" + layertxt);
                    InitializeNodeImage(pCurrentNode, true);
                }
                else
                {
                    if (pCurrentNode != null)
                    {
                        pCurrentNode.Checked = false;
                        SetParentCheckState(pCurrentNode);
                    }
                    if (vProgress.UserAskCancel)
                    {
                        break;
                    }
                    XmlElement tmpLayeritemEle = layerList[i] as XmlElement;    //替换赋值方法 chulili 20111103
                    if (tmpLayeritemEle != null)
                    {
                        tmpLayeritemEle.SetAttribute("Load", "0");
                    }

                    //layerList[i].Attributes["Load"].Value = "0";
                    //layerList[i].Attributes["View"].Value = "0";
                    //added by chulili 20110708 修改最新xml文件

                    XmlElement tmpDocLayerEle = pDocLayerNode as XmlElement;    //替换赋值方法,chulili 20111103
                    if (tmpDocLayerEle != null)
                    {
                        tmpDocLayerEle.SetAttribute("Load", "0");
                    }

                    //if (pDocLayerNode != null)
                    //{
                    //    pDocLayerNode.Attributes["Load"].Value = "0";
                    //}
                    //pDocLayerNode.Attributes["View"].Value = "0";
                    //add end
                    if (_DicAddLyrs.ContainsKey(nodeKey))
                    {
                        ILayer removeLayer = _DicAddLyrs[nodeKey];
                        m_pMapCtl.Map.DeleteLayer(removeLayer);

                        //m_SystemData.frmProgress.ShowProgress("正在移除" + removeLayer.Name + "数据...", (int)((i + 1) * 100 / layerList.Count));
                        _DicAddLyrs.Remove(nodeKey);

                        //(removeLayer as ILayerGeneralProperties).LayerDescription = layerList[i].OuterXml;
                        if (_DicDelLyrs.ContainsKey(nodeKey) == false)
                            _DicDelLyrs.Add(nodeKey, removeLayer);
                    }
                    InitializeNodeImage(pCurrentNode, false);
                }
                
            }
            //if (checkState == CheckState.Checked)
            //{
            //    SetParentCheckState(checkNode);     //added by chulili 20111028
            //}
			pXmldoc.Save(_LayerXmlPath);//added by chulili 20110708
            if (_isLayerConfig)
            {
                SysCommon.ModSysSetting.IsLayerTreeChanged = true;
            }
            if (checkState == CheckState.Checked)
            {//添加图层后排序
                //ModuleMap.LayersCompose(m_pMapCtl as IMapControlDefault);
                //added by chulili 20110628图层排序后，刷新一下toc控件 
                m_pMapCtl.ActiveView.Activate(m_pMapCtl.hWnd);
                
                if (_TocControl != null)
                {
                    _TocControl.SetBuddyControl(m_pMapCtl);//yjl20111102 add 加载数据后 绑定TOC（200图层效率快1倍）
                    _TocControl.Update();
                }
                
                //if (fmPgs != null && !fmPgs.IsDisposed)
                //    fmPgs.Dispose();
            }
            vProgress.Close();  //changed by chulili 20111117 加载和卸载都要进度条
            if (ListLayerError.Count > 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandleEx("错误提示", "加载图层遇到以下错误", ListLayerError);
            }
            ListLayerError = null;
            //m_SystemData.TocTreeESRI.SetBuddyControl(m_SystemData.MainMap);
            //GeoFrameBase.ModuleControl.TreeRedraw(ptr, true);
            //GeoFrameBase.ModuleControl.CollapseTocLayer(m_SystemData.MainMap.Map, m_SystemData.TocTreeESRI, false);
            //m_SystemData.frmProgress.Visible = false;
            //changed by chulili 20110718 用户修改节点check状态时，并无意修改展开状态
            //控制节点是否展开  
            //if (XmlOperation.GetBoolean(xmlNode, true, "Expand") == false)
            //    checkNode.Expanded = false;
            //end changed by chulili 
        }
        //added by chulili 20111118 根据NodeKey查找节点
        public DevComponents.AdvTree.Node GetNodeByNodeKey( string strNodeKey)
        {
            if (TreeDataLib.Nodes.Count == 0)
            {
                return null;
            }
            DevComponents.AdvTree.Node pNode = TreeDataLib.Nodes[0];
            return GetLayerNodeByNodeKey(pNode, strNodeKey);
        }
        //根据NodeKey查找节点，递归调用函数
        //参数含义：查找节点范围，NodeKey值
        public DevComponents.AdvTree.Node GetLayerNodeByNodeKey(DevComponents.AdvTree.Node pNode,string strNodeKey)
        {
            DevComponents.AdvTree.Node pTmpnode = null;
            if (pNode.Name.Equals(strNodeKey))
                return pNode;
            if (pNode.Nodes.Count > 0)
            {
                for (int i = 0; i < pNode.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node pChildNode = pNode.Nodes[i];
                    pTmpnode = GetLayerNodeByNodeKey(pChildNode,strNodeKey);
                    if (pTmpnode != null)
                    {
                        return pTmpnode;
                    }
                }
            }
            return null;
        }
        //添加或卸载图层
        private void TreeDataLib_AfterCheck(object sender, AdvTreeCellEventArgs e)
        {
            if (e.Action == eTreeAction.Mouse)
            {
                DevComponents.AdvTree.Node vNode = e.Cell.Parent;
                if (vNode.DataKey == null) return;
                XmlNode layerNode = vNode.DataKey as XmlNode;
                if (layerNode.Name == "DataDIR" || layerNode.Name == "Root")
                {
                    AddOrDelDataDir(vNode, vNode.CheckState);
                }
                else if (layerNode.Name == "DIR")
                {
                    AddOrDelDataDir(vNode, vNode.CheckState);
                    //return;
                }
                else
                {
                    AddOrDelLyr(vNode, vNode.Checked);
                }
            }
            
        }

        //判断是否所有子节点的CheckedState都为Checked
        protected bool IsChildAllCheck(DevComponents.AdvTree.Node treenode)
        {
            if (treenode.Nodes.Count == 0)
                return false;
            for (int i = 0; i < treenode.Nodes.Count; i++)
            {
                if (treenode.Nodes[i].CheckState == CheckState.Unchecked)
                    return false;
            }
            return true;
        }
        //判断是否所有子节点的CheckedState都为UnChecked
        protected bool IsChildAllUnCheck(DevComponents.AdvTree.Node treenode)
        {
            if (treenode.Nodes.Count == 0)
                return false;
            for (int i = 0; i < treenode.Nodes.Count; i++)
            {
                if (treenode.Nodes[i].CheckState == CheckState.Checked)
                    return false;
            }
            return true;
        }

        //向下设置复选框
        protected void SetChildCheckState(DevComponents.AdvTree.Node nodeCurrent)
        {
            for (int i = 0; i < nodeCurrent.Nodes.Count; i++)
            {
                nodeCurrent.Nodes[i].CheckState = nodeCurrent.CheckState;
                WhileNodeStateChange(nodeCurrent.Nodes[i], nodeCurrent.CheckState.ToString());
                SetChildCheckState(nodeCurrent.Nodes[i]);
            }
        }

        //向上设置复选框
        protected void SetParentCheckState(DevComponents.AdvTree.Node nodeParent)
        {
            if (nodeParent == null) return;

            if (nodeParent.Tag.ToString() == "Layer")
            {//deleted by chulili 20110708图层节点没有子节点，所以不必进行判断，Checked状态仅根据xml中Load属性设置
                //if (IsChildAllCheck(nodeParent))
                //{
                //    nodeParent.CheckState = CheckState.Checked;
                //    WhileNodeStateChange(nodeParent, CheckState.Checked.ToString());
                //}
                //else if (IsChildAllUnCheck(nodeParent))
                //{
                //    nodeParent.CheckState = CheckState.Unchecked;
                //    WhileNodeStateChange(nodeParent, CheckState.Unchecked.ToString());
                //}
                //else
                //{
                //    nodeParent.CheckState = CheckState.Checked;
                //    WhileNodeStateChange(nodeParent, "DataDIR&HalfOpened");
                //}
                SetParentCheckState(nodeParent.Parent);
            }
            else if (nodeParent.Tag.ToString().Contains("DataDIR"))
            {
                if (IsChildAllCheck(nodeParent))
                {
                    nodeParent.CheckState = CheckState.Checked;
                    WhileNodeStateChange(nodeParent, CheckState.Checked.ToString());
                }
                else if (IsChildAllUnCheck(nodeParent))
                {
                    nodeParent.CheckState = CheckState.Unchecked;
                    WhileNodeStateChange(nodeParent, CheckState.Unchecked.ToString());
                }
                else
                {
                    nodeParent.CheckState = CheckState.Checked;
                    WhileNodeStateChange(nodeParent, "DataDIR&HalfOpened");
                }

                SetParentCheckState(nodeParent.Parent);
            }
            else if (nodeParent.Tag.ToString()=="DIR")  //added by chulili 20111009支持专题节点复选框
            {
                if (IsChildAllCheck(nodeParent))
                {
                    nodeParent.CheckState = CheckState.Checked;
                }
                else if (IsChildAllUnCheck(nodeParent))
                {
                    nodeParent.CheckState = CheckState.Unchecked;
                }
                else
                {
                    nodeParent.CheckState = CheckState.Checked;
                }
                SetParentCheckState(nodeParent.Parent);
            }
        }

        //设置树节点的tag值及图片
        private void SetTreeNodeImage(DevComponents.AdvTree.Node treenode, string tagStr)
        {
            if (treenode.Tag == null) return;
            if (treenode.Tag.ToString().Contains("DataDIR") == false) return;
            treenode.Image = null;
            string tagString = tagStr;
            switch (tagStr)
            {
                case "DataDIR&AllOpened":
                    tagString = "DataDIROpen";
                    break;
                case "DataDIR&Closed":
                    tagString = "DataDIRClosed";
                    break;
                case "DataDIR&HalfOpened":
                    tagString = "DataDIRHalfOpen";
                    break;
                case "Checked":
                    tagString = "DataDIROpen";
                    break;
                case "Unchecked":
                    tagString = "DataDIRClosed";
                    break;
            }
            //treenode.Tag = tagString;//shduan20110612
            //treenode.LeftImages.Add(tagString);
            treenode.Image = treenode.TreeControl.ImageList.Images[tagString];
        }

        //复选框的选择状态改变时程序要做的事情
        protected void WhileNodeStateChange(DevComponents.AdvTree.Node treenode, string checkState)
        {
            SetTreeNodeImage(treenode, checkState);
        }

        private void UcDataLib_Load(object sender, EventArgs e)
        {
            //如果在配置图层目录时使用，则设置相应的右键菜单
            if (_isLayerConfig == true)
            {
                //this.barMenu.Visible = true;                    
                TreeDataLib.ContextMenuStrip = this.contextMenuLayerTree;
                this.contextMenuLayerTree.Visible = true;
                //this.checkBoxShow.Checked = false;
                _isLayerVisible = false;
                ModuleMap.SetLayersVisibleAttri(m_pMapCtl as IMapControlDefault, _isLayerVisible);
            }
        }
        public void AddFolder()
        {
            DevComponents.AdvTree.Node pNode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;
            if (pNode == null)
                return;
            //对代码进行修改，严格限制允许添加文件夹的节点名称
            if (!pNode.Tag.ToString().Equals("DIR") && !pNode.Tag.ToString().Contains("Root"))
                return;  
            try
            {
                //弹出对话框
                FormAddFolder pFrm = new FormAddFolder(Plugin.ModuleCommon.TmpWorkSpace );
                if (pFrm.ShowDialog() != DialogResult.OK)
                    return;
                //根据从对话框返回的属性值，设置节点
                DevComponents.AdvTree.Node addNode = new DevComponents.AdvTree.Node();
                addNode.Text = pFrm._Foldername;
                addNode.Tag = "DIR";
                addNode.Image = this.ImageList.Images["DIR"];
                addNode.CheckBoxVisible = true;
                addNode.CheckState = CheckState.Checked;
                string nodekey = Guid.NewGuid().ToString();
                addNode.Name = nodekey;
                pNode.Nodes.Add(addNode);
                //添加相应的xml节点
                XmlDocument XMLDoc = new XmlDocument();
                XMLDoc.Load(_LayerXmlPath);
                //shduan 20110612 修改*******************************************************************************************
                //string strSearch = "//" + pNode.Tag + "[@NodeKey='" + pNode.Name + "']";
                string strSearch = "";
                if (pNode.Tag.ToString().Contains("DataDIR"))
                {
                    strSearch = "//DataDIR" + "[@NodeKey='" + pNode.Name + "']";
                }
                else
                {
                    strSearch = "//" + pNode.Tag + "[@NodeKey='" + pNode.Name + "']";
                }
                //****************************************************************************************************************
                XmlNode pxmlnode = XMLDoc.SelectSingleNode(strSearch);
                XmlElement childele = XMLDoc.CreateElement("DIR");

                childele.SetAttribute("NodeKey", nodekey);
                childele.SetAttribute("NodeText", pFrm._Foldername);
                childele.SetAttribute("Description", pFrm._FolderScrip);
                childele.SetAttribute("DataScale", pFrm._Scale);
                childele.SetAttribute("DataType", pFrm._DataType);
                childele.SetAttribute("Year", pFrm._Year);
                childele.SetAttribute("DIRType", pFrm._DIRType);
                //deleted by chulili 20110921 第一级专题节点没有行政区
                //childele.SetAttribute("XZQCode", pFrm._XZQCode);
                childele.SetAttribute("Enabled", "true");
                childele.SetAttribute("Expand", "100");
                pxmlnode.AppendChild(childele as XmlNode);
                XMLDoc.Save(_LayerXmlPath);
                //addNode.DataKey = childele as object;
                ModuleMap.SetDataKey(addNode, childele as XmlNode);
                SysCommon.ModSysSetting.IsLayerTreeChanged = true;
            }
            catch
            { }
        }
        //添加文件夹
        private void MenuItemAddFolder_Click(object sender, EventArgs e)
        {
            AddFolder();
        }
//打开地图文档的后续处理 added by chulili 20110728
        public void OpenMxdDocDeal()
        {
            //清空节点打勾状态
            for (int i = 0; i < this.TreeDataLib.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node pNode = TreeDataLib.Nodes[i];
                UnSelectAllAdvNode(pNode);
            }
            //清空各类字典
            _DicAddLyrs.Clear();
            _DicDelLyrs.Clear();
            _DicAddGroupLyrs.Clear();
            _DicDelGroupLyrs.Clear();
            _LayerDicOfGroupLayer.Clear();

            //循环处理map中的图层
            for ( int i = m_pMapCtl.Map.LayerCount-1;i>=0; i--)
            {
                ILayer pLayer = m_pMapCtl.Map.get_Layer(i);
                OpenMxdDocDealLayer(pLayer);
            }
        }
        //递归函数，将所有的节点不选中 added by chulili 20110728
        private void UnSelectAllAdvNode(DevComponents.AdvTree.Node pNode)
        {
            pNode.Checked = false;
            if (pNode.Nodes.Count > 0)
            {
                for (int i = 0; i < pNode.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node pTmpNode = pNode.Nodes[i];
                    UnSelectAllAdvNode(pTmpNode );
                }
            }
        }
        //打开地图文档后对图层后续处理  递归调用added by chulili 20110728
        private void OpenMxdDocDealLayer(ILayer pLayer)
        {
            if (pLayer is IGroupLayer)
            {
                #region 对GroupLayer的支持，先注释掉，目前没有GroupLayer chulili 20111103
                //ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
                ////读取该图层的描述信息，转成xml节点
                //string strNodeXml = pLayerGenPro.LayerDescription;

                //if (strNodeXml.Equals(""))
                //{
                //    return;
                //}
                //XmlDocument pXmldoc = new XmlDocument();
                //pXmldoc.LoadXml(strNodeXml);
                ////获取节点的NodeKey信息
                //XmlNode pxmlnode = pXmldoc.SelectSingleNode("//DataDIR");
                //if (pxmlnode == null)
                //{
                //    pXmldoc = null;
                //    return;
                //}
                //string strNodeKey = pxmlnode.Attributes["NodeKey"].Value.ToString();
                //DevComponents.AdvTree.Node pnode = SearchLayerNodebyName(strNodeKey);
                //if (pnode == null)
                //{
                //    pXmldoc = null;
                //    return;
                //}
                ////存储到图层组字典里
                //if (!_DicAddGroupLyrs.Keys.Contains(strNodeKey)) 
                //{
                //    _DicAddGroupLyrs.Add(strNodeKey, pLayer as IGroupLayer );
                //}
                ////相应节点打勾
                //pnode.Checked = true;
                ////依次处理子节点
                //ICompositeLayer pCompoLayer = pLayer as ICompositeLayer;
                //for (int i = 0; i < pCompoLayer.Count; i++)
                //{
                //    ILayer pTmpLayer = pCompoLayer.get_Layer(i);
                //    OpenMxdDocDealLayer(pTmpLayer);
                //}
                #endregion
            }
            else if (pLayer is IFeatureLayer || pLayer is IRasterLayer || pLayer is IRasterCatalog)
            {
                IFeatureLayer pFeaLayer = pLayer as IFeatureLayer;
                ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
                //读取图层的描述，转成xml节点
                string strNodeXml = pLayerGenPro.LayerDescription;
                
                if (strNodeXml.Equals(""))
                {
                    return;
                }
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.LoadXml(strNodeXml );
                //获取节点的NodeKey信息
                XmlNode pxmlnode = pXmldoc.SelectSingleNode("//Layer");
                if (pxmlnode == null)
                { return; }
                string strNodeKey = pxmlnode.Attributes["NodeKey"].Value.ToString();
                pXmldoc = null;
                if (!Plugin.ModuleCommon.AppUser.Name.Equals("admin"))
                {
                    if (Plugin.ModuleCommon.ListUserdataPriID == null)//changed by xisheng 2011.06.29
                    {
                        m_pMapCtl.Map.DeleteLayer(pLayer);
                        return;
                    }
                    if (!Plugin.ModuleCommon.ListUserdataPriID.Contains(strNodeKey))
                    {
                        m_pMapCtl.Map.DeleteLayer(pLayer);
                        return;
                    }
                }
                ModuleMap.SetFieldVisibleOfLayer(pLayer,pxmlnode);
                //将图层保存到图层字典里
                DevComponents.AdvTree.Node pnode = SearchLayerNodebyName(strNodeKey);
                if (pnode == null)
                {
                    return; 
                }
                if (!_DicAddLyrs.Keys.Contains(strNodeKey))
                {
                    _DicAddLyrs.Add(strNodeKey, pLayer);
                }
                //数据目录中相应节点打勾                
                pnode.Checked = true;
                SetParentCheckState(pnode);
                #region 对GroupLayer的支持，先注释掉，目前没有GroupLayer chulili 20111103
                //如果父节点是数据集节点
                //DevComponents.AdvTree.Node pParentNode = pnode.Parent;
                //if (!pParentNode.Tag.ToString().Contains("DataDIR"))
                //{
                //    return;
                //}
                
                //string sParentNodeKey = pParentNode.Name;
                ////父节点对子节点的字典里保存该图层
                //IDictionary<string, ILayer> pDic = null;
                //if (_LayerDicOfGroupLayer.Keys.Contains(sParentNodeKey))
                //{
                //    pDic = _LayerDicOfGroupLayer[sParentNodeKey] as IDictionary<string, ILayer>;
                //}
                //else
                //{
                //    pDic = new Dictionary<string, ILayer>();
                //    _LayerDicOfGroupLayer.Add(sParentNodeKey, pDic as object);
                //}
                ////关于该图层组的字典中加入该图层
                //if (!pDic.Keys.Contains(strNodeKey))
                //{
                //    pDic.Add(strNodeKey, pLayer );
                //} 
                #endregion
            }
        }

        //移除某个图层，可能是外部加载，可能是内部加载
        public void Removelayer(ILayer player)
        {
            string nodekey = "";
            //内部加载
            if (_DicAddLyrs.Values.Contains(player))
            {
                foreach (string strkey in _DicAddLyrs.Keys)
                {
                    ILayer ptmplayer = _DicAddLyrs[strkey];
                    if (ptmplayer.Equals(player))
                    {   //先找到树节点名称(树节点就是xml中节点的nodekey)
                        nodekey = strkey;
                        break;
                    }
                }
                //根据树节点名称，查找对应树节点
                DevComponents.AdvTree.Node pnode = SearchLayerNodebyName(nodekey);
                //如果是内部加载的图层，找到对应的树节点，走树节点AfterCheck流程
                if (pnode != null)
                {
                    pnode.Checked = false;
                    AddOrDelLyr(pnode, false);
                }
            }
            //外部加载
            else
            {
                m_pMapCtl.Map.DeleteLayer(player );
            }
        }
        //移除某个图层，可能是外部加载，可能是内部加载
        public void Removelayer()
        {
            DevComponents.AdvTree.Node pNode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;
            if (pNode == null)
                return;
            //对代码进行修改，严格限制允许添加文件夹的节点名称
            if (!pNode.Tag.ToString().Contains ("Layer"))
                return;
            string strTag = pNode.Tag.ToString();
            switch (strTag)
            {
                case "Layer":
                    pNode.Checked = false;
                    AddOrDelLyr(pNode, false);
                    break;
                case "OutLayer":
                    ILayer pLayer = pNode.DataKey as ILayer;
                    m_pMapCtl.Map.DeleteLayer(pLayer);
                    pNode.Remove();
                    break;

            }
        }
        //added by chulili 20110714 已知节点名称，查找节点（名称唯一）
        private DevComponents.AdvTree.Node  SearchLayerNodebyName(string nodename)
        {
            if (TreeDataLib.Nodes.Count == 0)
                return null;
            DevComponents.AdvTree.Node pSearchNode = null;
            for (int i = 0; i < TreeDataLib.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node pNode = TreeDataLib.Nodes[i];
                if (pNode.Name == nodename)
                    return pNode;
                //调用递归函数，查找节点
                pSearchNode = SearchLayerNodebyName(pNode, nodename);
                if (pSearchNode != null)
                    return pSearchNode;

            }
            return null;
        }//added by chulili 20110714 已知节点名称，查找节点（名称唯一），递归调用
        private DevComponents.AdvTree.Node SearchLayerNodebyName(DevComponents.AdvTree.Node pNode, string nodename)
        {
            if (pNode.Name == nodename)
                return pNode;
            if(pNode.Nodes.Count == 0)
                return null;
            DevComponents.AdvTree.Node pSearchNode = null;
            //遍历子节点
            for (int i = 0; i < pNode.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node ptmpNode = pNode.Nodes[i];
                if (ptmpNode.Name == nodename)
                    return ptmpNode;
                //递归调用查找
                pSearchNode = SearchLayerNodebyName(ptmpNode,nodename);
                //如果已找到，则返回
                if (pSearchNode != null)
                    return pSearchNode;

            }
            return null;
        }
        public void AddDataSet()
        {
            DevComponents.AdvTree.Node pNode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;

            if (pNode == null)
                return;
            //修改代码，对允许添加数据集的节点类型进行严格限定
            if (!pNode.Tag.ToString().Equals("DIR") && !pNode.Tag.ToString().Contains("Root") && !pNode.Tag.ToString().Contains("DataDIR"))
                return;
            try
            {
                FormAddLayer pFrm = new FormAddLayer(null,_LayerXmlPath, m_pWks, pNode, true, this.ImageList, 2);
                pFrm.ShowDialog();
                //added by chulili 20110712 根据节点的选定状态判断是否加载
                DevComponents.AdvTree.Node pAddnode = pFrm.CurNode;
                if (pAddnode.Checked)
                {
                    m_pMapCtl.ActiveView.Deactivate();  //不刷新
                    AddNodeToMap(pAddnode);
                    m_pMapCtl.ActiveView.Activate(m_pMapCtl.hWnd);  //刷新
                    //m_pMapCtl.ActiveView.Refresh();
                }
                //end add
            }
            catch
            { }
        }
        //添加数据集
        private void MenuItemAddDataset_Click(object sender, EventArgs e)
        {

            AddDataSet();
               
            //shduan 20110612屏蔽
            //if (pNode.Tag.ToString().Equals("Layer") || pNode.Tag.ToString().Contains ("DataDIR"))
            //    return;
            //******************************************************************************************
            //FormAddDataSet pFrm = new FormAddDataSet();
            //if (pFrm.ShowDialog() != DialogResult.OK)
            //    return;
            //DevComponents.AdvTree.Node addNode = new DevComponents.AdvTree.Node();
            //addNode.Text = pFrm._DataSetname;
            //addNode.Tag = "DataDIR";
            //addNode.Image = this.ImageList.Images["DataDIROpen"];
            //addNode.CheckBoxVisible = true;
            //addNode.CheckState = CheckState.Checked;
            //string nodekey = Guid.NewGuid().ToString();
            //addNode.Name = nodekey;
            //pNode.Nodes.Add(addNode);
            //string strTag = pNode.Tag.ToString();
            //string NodeName = strTag;
            //if (strTag.Contains("DataDIR"))
            //{
            //    NodeName = "DataDIR";
            //}
            //XmlDocument XMLDoc = new XmlDocument();
            //XMLDoc.Load(_LayerXmlPath);
            //string strSearch = "//" + NodeName + "[@NodeKey='" + pNode.Name + "']";
            //XmlNode pxmlnode = XMLDoc.SelectSingleNode(strSearch);
            //XmlElement childele = XMLDoc.CreateElement("DataDIR");

            //childele.SetAttribute("NodeKey", nodekey);
            //childele.SetAttribute("NodeText", pFrm._DataSetname);
            //childele.SetAttribute("Description", pFrm._DataSetScrip);
            //childele.SetAttribute("Enabled", "true");
            //childele.SetAttribute("Expand", "100");
            //pxmlnode.AppendChild(childele as XmlNode);
            //XMLDoc.Save(_LayerXmlPath);

        }
        public void AddWMSserviceLayer()
        {
            DevComponents.AdvTree.Node pNode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;
            if (pNode == null)
                return;

            //修改代码，对允许添加图层的节点进行严格限定
            if (!pNode.Tag.ToString().Equals("DIR") && !pNode.Tag.ToString().Contains("Root") && !pNode.Tag.ToString().Contains("DataDIR"))
                return;
            try
            {
                FormAddServiceLayer2 pFrm = new FormAddServiceLayer2(null,_LayerXmlPath, m_pWks, pNode, true, this.ImageList,"WMS");
                pFrm.ShowDialog();//FormAddServiceLayer yjl20120814 modify
                //added by chulili 20110712 根据节点的选定状态判断是否加载
                DevComponents.AdvTree.Node pAddnode = pFrm.CurNode;
                if (pAddnode.Checked)
                {
                    //m_pMapCtl.ActiveView.Deactivate();  //不刷新

                    AddNodeToMap(pAddnode);

                    //m_pMapCtl.ActiveView.Activate(m_pMapCtl.hWnd);  //刷新

                    m_pMapCtl.ActiveView.Refresh();
                }
                //end add
            }
            catch
            { }
        }
        public void AddLayer()
        {
            DevComponents.AdvTree.Node pNode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;
            if (pNode == null)
                return;

            //修改代码，对允许添加图层的节点进行严格限定
            if (!pNode.Tag.ToString().Equals("DIR") && !pNode.Tag.ToString().Contains("Root") && !pNode.Tag.ToString().Contains("DataDIR"))
                return;
            try
            {
                FormAddLayer pFrm = new FormAddLayer(null,_LayerXmlPath, m_pWks, pNode, true, this.ImageList, 1);
                pFrm.ShowDialog();
                //added by chulili 20110712 根据节点的选定状态判断是否加载
                DevComponents.AdvTree.Node pAddnode = pFrm.CurNode;
                if (pAddnode.Checked)
                {
                    //m_pMapCtl.ActiveView.Deactivate();  //不刷新

                    AddNodeToMap(pAddnode);

                    //m_pMapCtl.ActiveView.Activate(m_pMapCtl.hWnd);  //刷新

                    m_pMapCtl.ActiveView.Refresh();
                }
                //end add
            }
            catch
            { }
        }
        //添加图层
        private void MenuItemAddLayer_Click(object sender, EventArgs e)
        {
            AddLayer();
        }
        public string GetTagOfSeletedNode()
        {
            //获取用户选中的节点
            DevComponents.AdvTree.Node pnode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;
            if (pnode == null)
                return "";
            string strTag = pnode.Tag.ToString();
            string NodeName = strTag;
            //若是数据集节点，则将节点名称转换一下
            if (strTag.Contains("DataDIR"))
            {
                NodeName = "DataDIR";
            }
            return NodeName;
        }
        public void ModifyNode()
        {
            //获取用户选中的节点
            DevComponents.AdvTree.Node pnode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;
            if (pnode == null)
                return;
            string strTag = pnode.Tag.ToString();
            string NodeName = strTag;
            //若是数据集节点，则将节点名称转换一下
            if (strTag.Contains("DataDIR"))
            {
                NodeName = "DataDIR";
            }
            //调用函数对节点进行修改
            switch (NodeName)
            {
                case "Root":
                    ModifyRoot(pnode);
                    break;
                case "DIR":
                    ModifyFolder(pnode);
                    break;
                case "DataDIR":
                    ModifyDataset(pnode);
                    break;
                case "Layer":
                    ModifyLayer(pnode);
                    break;
                default:
                    break;

            }
        }
        //修改节点
        private void MenuItemModify_Click(object sender, EventArgs e)
        {
            ModifyNode();
        }
        //删除节点
        private void MenuDelete_Click(object sender, EventArgs e)
        {
            DeleteTreeNode();
        }
        //added by chulili 2012-11-09 获取删除的数据节点列表
        private void GetDeleteNodeKeys(DevComponents.AdvTree.Node pNode, ref List<string> ListDeleteNodeKeys)
        {
            string strtag = pNode.Tag.ToString();
            if (pNode.Nodes.Count > 0)
            {
                for (int i = 0; i < pNode.Nodes.Count; i++)
                {
                    GetDeleteNodeKeys(pNode.Nodes[i],ref ListDeleteNodeKeys);
                }
            }
            string strName = pNode.Name;
            ListDeleteNodeKeys.Add(strName);
            if (pNode.Tag.ToString() == "Layer")
            {
                //查找在xml中对应节点
                XmlDocument XMLDoc = new XmlDocument();
                XMLDoc.Load(_LayerXmlPath);
                string strSearch = "//Layer[@NodeKey='" + strName + "']";
                XmlNode pxmlnode = XMLDoc.SelectSingleNode(strSearch);
                if (pxmlnode.ChildNodes.Count > 0)
                {
                    foreach (XmlNode pChildnode in pxmlnode.ChildNodes)
                    {
                        if (pChildnode.Name == "Field")
                        {
                            try
                            {
                                string strFieldNodeKey = (pChildnode as XmlElement).GetAttribute("NodeKey");
                                ListDeleteNodeKeys.Add(strFieldNodeKey);
                            }
                            catch
                            { }
                        }
                    }
                }
                XMLDoc = null;
            }

        }
        public void GetDeleteNodeKeys(ref List<string> ListDeleteNodeKeys)
        {
            DevComponents.AdvTree.Node pnode = TreeDataLib.SelectedNode;
            GetDeleteNodeKeys(pnode, ref ListDeleteNodeKeys);

        }
        //changed by chulili 2012-11-09  删除节点的同时，删除数据权限列表中相对应的记录
        public void DeleteTreeNode()
        {
            DevComponents.AdvTree.Node pnode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;
            if (pnode == null)
            {
                return;
            }
            if (MessageBox.Show("确定要删除节点'" + pnode.Text + "'吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            string strTag = pnode.Tag.ToString();
            string NodeName = strTag;
            //若是数据集节点，则将节点名称转换一下
            if (strTag.Contains("DataDIR"))
            {
                NodeName = "DataDIR";
            }
            switch (NodeName)
            {
                case "Root":
                    //根节点不允许删除
                    break;
                case "DIR":
                    Deletenode(pnode);
                    break;
                case "DataDIR":
                    Deletenode(pnode);
                    break;
                case "Layer":
                    Deletenode(pnode);
                    break;
                default:
                    break;
            }
        }
        //修改文件夹
        private void ModifyFolder(DevComponents.AdvTree.Node pNode)
        {
            //查找在xml中对应节点
            XmlDocument XMLDoc = new XmlDocument();
            XMLDoc.Load(_LayerXmlPath);
            string strSearch = "//" + pNode.Tag + "[@NodeKey='" + pNode.Name + "']";
            XmlNode pxmlnode = XMLDoc.SelectSingleNode(strSearch);
            if (pxmlnode == null)
                return;
            XmlElement pELe = pxmlnode as XmlElement;
            //获取xml中节点属性
            string Foldername = pELe.GetAttribute("NodeText");
            string FolderScrip = pELe.GetAttribute("Description");
            string strScale = "";
            if (pELe.HasAttribute("DataScale"))
            {
                strScale = pELe.GetAttribute("DataScale");
            }
            string strDataType = "";
            if (pELe.HasAttribute("DataType"))
            {
                strDataType = pELe.GetAttribute("DataType");
            }
            string strDIRType = "";
            if (pELe.HasAttribute("DIRType"))
            {
                strDIRType = pELe.GetAttribute("DIRType");
            }
            string strYear = "";
            if (pELe.HasAttribute("Year"))
            {
                strYear = pELe.GetAttribute("Year");
            }
            //deleted by chulili 20110921 第一级专题节点没有行政区
            //string strXZQ = "";
            //if (pELe.HasAttribute("XZQCode"))
            //{
            //    strXZQ = pELe.GetAttribute("XZQCode");
            //}
            //弹出对话框供用户修改
            FormAddFolder pFrm = new FormAddFolder(Plugin.ModuleCommon.TmpWorkSpace, Foldername, FolderScrip,strScale,strDataType,strYear,strDIRType,"修改专题");
            if (pFrm.ShowDialog() != DialogResult.OK)
                return;
            pNode.Text = pFrm._Foldername;
            //修改属性
            pELe.SetAttribute("NodeText", pFrm._Foldername);
            pELe.SetAttribute("Description", pFrm._FolderScrip);
            pELe.SetAttribute("DataScale", pFrm._Scale);
            pELe.SetAttribute("DataType", pFrm._DataType);
            pELe.SetAttribute("Year", pFrm._Year);
            pELe.SetAttribute("DIRType", pFrm._DIRType);
            //deleted by chulili 20110921 第一级专题节点没有行政区
            //pELe.SetAttribute("XZQCode", pFrm._XZQCode);
            XMLDoc.Save(_LayerXmlPath);
            //pNode.DataKey = pELe as object;
            ModuleMap.SetDataKey(pNode, pELe as XmlNode);
            SysCommon.ModSysSetting.IsLayerTreeChanged = true;

        }
        //修改图层目录根节点
        private void ModifyRoot(DevComponents.AdvTree.Node pNode)
        {
            //查找在xml中对应节点
            XmlDocument XMLDoc = new XmlDocument();
            XMLDoc.Load(_LayerXmlPath);
            string strSearch = "//" + pNode.Tag + "[@NodeKey='" + pNode.Name + "']";
            XmlNode pxmlnode = XMLDoc.SelectSingleNode(strSearch);
            if (pxmlnode == null)
                return;
            XmlElement pELe = pxmlnode as XmlElement;
            //获取xml中节点属性
            string Foldername = pELe.GetAttribute("NodeText");
            string FolderScrip = pELe.GetAttribute("Description");
            //弹出对话框供用户修改
            FormAddDataSet pFrm = new FormAddDataSet(Foldername, FolderScrip, "修改根节点");
            if (pFrm.ShowDialog() != DialogResult.OK)
                return;
            pNode.Text = pFrm._DataSetname;
            //修改属性
            pELe.SetAttribute("NodeText", pFrm._DataSetname);
            pELe.SetAttribute("Description", pFrm._DataSetScrip);
            XMLDoc.Save(_LayerXmlPath);
            SysCommon.ModSysSetting.IsLayerTreeChanged = true;
        }

        //删除节点
        private void Deletenode(DevComponents.AdvTree.Node pNode)
        {
            if (pNode == null)
            {
                return;
            }
            string strTag = pNode.Tag.ToString();
            string NodeName = strTag;
            //判断是否是数据集节点
            if (strTag.Contains("DataDIR"))
            {
                NodeName = "DataDIR";
            }
            //查找在xml中对应节点
            XmlDocument XMLDoc = new XmlDocument();
            XMLDoc.Load(_LayerXmlPath);
            string strSearch = "//" + NodeName + "[@NodeKey='" + pNode.Name + "']";
            XmlNode pxmlnode = XMLDoc.SelectSingleNode(strSearch);

            //分别在xml中和控件中删除节点
            //RemoveLayer(pNode);
            //added by chulili 20111011
            DevComponents.AdvTree.Node pParentNode = pNode.Parent;
            XmlNode pXmlParent = null;
            if (pxmlnode != null)
            {
                pXmlParent = pxmlnode.ParentNode;
            }

            //end added by chulili
            RemoveNodeFromMap(pNode );//changed by chulili 适应文件夹及数据集节点,从视图浏览中删除图层
            pNode.Remove( );
            ModuleMap.SetDataKey(pParentNode, pXmlParent);

            //changed by chulili 挪动位置 即使pxmlnode为空，也应该先删除树图中的节点
            if (pxmlnode == null)
            {
                SysCommon.ModSysSetting.IsLayerTreeChanged = true;
                return;
            }
            XmlNode pxmlParent = pxmlnode.ParentNode;
            if (pxmlParent != null)
            {
                pxmlParent.RemoveChild(pxmlnode);
            }
            //保存本地xml文件
            XMLDoc.Save(_LayerXmlPath);
            SysCommon.ModSysSetting.IsLayerTreeChanged = true;
        }
        //修改数据集
        private void ModifyDataset(DevComponents.AdvTree.Node pNode)
        {
            //查找在xml中对应节点
            XmlDocument XMLDoc = new XmlDocument();
            XMLDoc.Load(_LayerXmlPath);
            string strTag = pNode.Tag.ToString();
            string NodeName = strTag;
            //如果是数据集节点，则需要对节点名称做处理
            if (strTag.Contains("DataDIR"))
            {
                NodeName = "DataDIR";
            }
            string strSearch = "//" + NodeName + "[@NodeKey='" + pNode.Name + "']";
            XmlNode pxmlnode = XMLDoc.SelectSingleNode(strSearch);
            if (pxmlnode == null)
                return;
            XmlElement pELe = pxmlnode as XmlElement;
            string Datasetname = pELe.GetAttribute("NodeText");
            string DatasetScrip = pELe.GetAttribute("Description");
            //RemoveNodeFromMap(pNode);//added by chulili 20110630先从视图中删除数据集中的图层
            //弹出对话框供用户修改

            FormAddDataSet pFrm = new FormAddDataSet(Datasetname, DatasetScrip,"修改图层组");
            DialogResult pResult = pFrm.ShowDialog();
            if (pResult != DialogResult.OK)

                return;
            RemoveNodeFromMap(pNode);
            pNode.Text = pFrm._DataSetname;
            pELe.SetAttribute("NodeText", pFrm._DataSetname);
            pELe.SetAttribute("Description", pFrm._DataSetScrip);
            //保存本地xml文件
            XMLDoc.Save(_LayerXmlPath);
            //pNode.DataKey = pELe as object;
            ModuleMap.SetDataKey(pNode, pELe as XmlNode);
            SysCommon.ModSysSetting.IsLayerTreeChanged = true;
            if (pNode.Checked)
            {
                //m_pMapCtl.ActiveView.Deactivate();  //不刷新
                AddNodeToMap(pNode);
                //m_pMapCtl.ActiveView.Refresh();
                //m_pMapCtl.ActiveView.Activate(m_pMapCtl.hWnd);  //刷新
            }
        }
        //修改图层
        private void ModifyLayer(DevComponents.AdvTree.Node pnode)
        {
            if (pnode == null) return;
            //在视图中删除原来的图层视图
            XmlNode pLayerNode = pnode.DataKey as XmlNode;
            if(pLayerNode==null) return;
            XmlElement pLayerEle=pLayerNode as XmlElement;
            string strDataType="";
            if(pLayerEle.HasAttribute("DataType"))
            {
                strDataType=pLayerEle.GetAttribute("DataType");
            }
            if (!strDataType.Contains("SERVICE"))
            {
                FormAddLayer pFrm = new FormAddLayer(null,_LayerXmlPath, m_pWks, pnode, false, this.ImageList,1);
                DialogResult pResult= pFrm.ShowDialog();

                //shduan 20110625 add判断

                if (pFrm.m_bIsModify && pResult==DialogResult.OK )
                {
                    RemoveLayer(pnode);
                    DevComponents.AdvTree.Node pNewNode = pFrm.CurNode;
                    if (pNewNode == null) return;
                    if (pNewNode.Checked)
                    {
                        //显示新的图层到视图浏览
                        AddOrDelLyr(pNewNode, pNewNode.Checked);
                    }
                }
            }
            else
            {
                FormModifyServiceLayer pFrmModify = new FormModifyServiceLayer(_LayerXmlPath, m_pWks,pnode);
                DialogResult pRes = pFrmModify.ShowDialog();
                if (pRes == DialogResult.OK && pFrmModify._Changed ) 
                {
                    RemoveLayer(pnode);

                    if (pnode.Checked)
                    {
                        //显示新的图层到视图浏览
                        AddOrDelLyr(pnode, pnode.Checked);
                    }
                }
            }
        }
        public void ZoomToLayer()
        {
            if (this.TreeDataLib.SelectedNode == null)
            {
                return;
            }
            //获取所选图层节点
            DevComponents.AdvTree.Node vNode = TreeDataLib.SelectedNode;
            //若不是图层节点，则跳出
            if (!vNode.Tag.ToString().Equals("Layer"))
                return;

            if (vNode.DataKey == null) return;
            try
            {
                //获取xml节点
                XmlNode layerNode = vNode.DataKey as XmlNode;
                string nodeKey = layerNode.Attributes["NodeKey"].Value;

                ILayer addLayer = null;
                //获取图层
                if (_DicAddLyrs.ContainsKey(nodeKey))
                    addLayer = _DicAddLyrs[nodeKey];    //若已加载到当前视图
                else if (_DicDelLyrs.ContainsKey(nodeKey))
                {
                    addLayer = _DicDelLyrs[nodeKey];    //若从当前卸载
                }
                else
                {   //若是新添加的图层
                    Exception errLayer = null;
                    addLayer = ModuleMap.AddLayerFromXml(ModuleMap._DicDataLibWks, layerNode, m_pWks, "", null, out errLayer);
                }
                if (addLayer == null) return;
                //缩放到该图层
                IActiveView pActiveView = m_pMapCtl.Map as IActiveView;
                pActiveView.Extent = addLayer.AreaOfInterest;
               
                //更改缩放到图层 不调整比例尺 xisheng 20111117********************************************
                //if (addLayer.MinimumScale > 0)
                  //  m_pMapCtl.Map.MapScale = addLayer.MinimumScale;//yjl20111031 add zoomtovisible 
                //更改缩放到图层 不调整比例尺 xisheng 20111117*****************************************end

                pActiveView.Refresh();
            }
            catch (Exception eError)
            {
                if (SysCommon.Log.Module.SysLog == null) SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
            }
        }
        //缩放到图层 added by chulili 2011-06-17
        private void MenuItemZoomToLayer_Click(object sender, EventArgs e)
        {
            ZoomToLayer();
        }
        //shduan 20110625
        private void TreeDataLib_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if(e.KeyChar  ==4)
            //DeleteTreeNode();
        }

        private void TreeDataLib_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {

                DeleteTreeNode();
            }
        }
        public void AutoMathLayerConfig()
        {
            DevComponents.AdvTree.Node pNode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;
            if (pNode == null)
                return;
            //RemoveNodeFromMap(pNode);//先从视图中删除节点
            FormAutoMatch pFrm = new FormAutoMatch(this,_LayerXmlPath, m_pWks, pNode);
            DialogResult pResult= pFrm.ShowDialog();

            //XmlDocument pXmldoc = new XmlDocument();
            //pXmldoc.Load(_LayerXmlPath);
            ////符号化自动匹配
            //AutoMatchRender(pNode, pXmldoc);
            //pXmldoc.Save(_LayerXmlPath);
            //ModuleMap.IsLayerTreeChanged = true;
            //再向视图中添加节点
            if (pResult == DialogResult.OK)
            {
                if (pNode.Checked)
                {
                    //AddNodeToMap(pNode);
                    m_pMapCtl.ActiveView.Refresh();
                }
            }
            if (pResult == DialogResult.OK)
            {
                MessageBox.Show("自动匹配符号成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //自动匹配符号
        private void MenuItemSetRender_Click(object sender, EventArgs e)
        {
            AutoMathLayerConfig();
        }
        //右键菜单，设置数据源
        public void SetDbSource()
        {
            DevComponents.AdvTree.Node pNode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;
            if (pNode == null)
                return;
            try
            {
                //RemoveNodeFromMap(pNode);//先从视图中删除节点
                FormSetDBsource pFrm = new FormSetDBsource(this,_LayerXmlPath, m_pWks, pNode);
                if (pFrm.ShowDialog() == DialogResult.OK)
                {
                    if (pNode.Checked)
                    {
                        //AddNodeToMap(pNode);
                        m_pMapCtl.ActiveView.Refresh();
                    }
                }
                pFrm = null;
            }
            catch
            { }
        }
        private void MenuItemSetDbsource_Click(object sender, EventArgs e)
        {
            SetDbSource();
        }
        //向视图中添加节点（与RemoveNodeFromMap函数对应）
        public void AddNodeToMap(DevComponents.AdvTree.Node pNode)
        {
            string strtag = pNode.Tag.ToString();
            if (strtag.Contains("DataDIR"))
            {
                strtag = "DataDIR";
            }
            switch (strtag)
            {
                case "Root":
                case "DIR":
                case "DataDIR":
                    //for (int i = 0; i < pNode.Nodes.Count; i++)
                    //{
                    //    DevComponents.AdvTree.Node tmpnode = pNode.Nodes[i];
                    //    AddNodeToMap(tmpnode);
                    //}
                    AddOrDelDataDir(pNode, pNode.CheckState);
                    break;
                case "Layer":
                    if (pNode.Checked)
                    {
                        AddOrDelLyr(pNode, pNode.Checked);
                    }

                    break;
            }
        }
        //从视图中删除节点
        public void RemoveNodeFromMap(DevComponents.AdvTree.Node pNode)
        {
            string strtag = pNode.Tag.ToString();
            if (strtag.Contains("DataDIR"))
            {
                strtag = "DataDIR";
            }
            switch (strtag)
            {
                case "Root":
                case "DIR":
                case "DataDIR":
                    for (int i = 0; i < pNode.Nodes.Count; i++)
                    {
                        DevComponents.AdvTree.Node tmpnode = pNode.Nodes[i];
                        RemoveNodeFromMap(tmpnode);
                        
                    }
                    RemoveDataDIRfromMap(pNode);
                    break;
                case "Layer":
                    RemoveLayer(pNode);
                    break;
            }
        }
        public void AddGroup()
        {
            DevComponents.AdvTree.Node pNode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;
            if (pNode == null)
                return;
            //对代码进行修改，严格限制允许添加文件夹的节点名称
            if (!pNode.Tag.ToString().Equals("DIR") && !pNode.Tag.ToString().Contains("Root") && !pNode.Tag.ToString().Contains("DataDIR"))
                return;
            try
            {
                //弹出对话框
                FormAddDataSet pFrm = new FormAddDataSet();
                if (pFrm.ShowDialog() != DialogResult.OK)
                    return;
                //根据从对话框返回的属性值，设置节点
                DevComponents.AdvTree.Node addNode = new DevComponents.AdvTree.Node();
                addNode.Text = pFrm._DataSetname;
                addNode.Tag = "DataDIR";
                addNode.Image = this.ImageList.Images["DataDIROpen"];
                addNode.CheckBoxVisible = true;
                addNode.CheckState = CheckState.Checked;
                string nodekey = Guid.NewGuid().ToString();
                addNode.Name = nodekey;
                pNode.Nodes.Add(addNode);
                //添加相应的xml节点
                XmlDocument XMLDoc = new XmlDocument();
                XMLDoc.Load(_LayerXmlPath);
                //shduan 20110612 修改*******************************************************************************************
                //string strSearch = "//" + pNode.Tag + "[@NodeKey='" + pNode.Name + "']";
                //chulili对pNode的级别已经严格限定，因此不必判断pNode.Tag 
                string strSearch = "//" + pNode.Tag + "[@NodeKey='" + pNode.Name + "']";

                //****************************************************************************************************************
                XmlNode pxmlnode = XMLDoc.SelectSingleNode(strSearch);
                XmlElement childele = XMLDoc.CreateElement("DataDIR");

                childele.SetAttribute("NodeKey", nodekey);
                childele.SetAttribute("NodeText", pFrm._DataSetname);
                childele.SetAttribute("Description", pFrm._DataSetScrip);
                childele.SetAttribute("Enabled", "true");
                childele.SetAttribute("Expand", "100");
                pxmlnode.AppendChild(childele as XmlNode);
                XMLDoc.Save(_LayerXmlPath);
                //addNode.DataKey = childele as object ;
                ModuleMap.SetDataKey(addNode, childele as XmlNode);
                SysCommon.ModSysSetting.IsLayerTreeChanged = true;
            }
            catch
            { }
        }
        private void MenuItemAddGroup_Click(object sender, EventArgs e)
        {
            AddGroup();
        }
        //added by chulili 20110709 判断右键菜单可用状态
        //added by chulili 20110709 判断右键菜单可用状态
        private void TreeDataLib_NodeMouseDown(object sender, TreeNodeMouseEventArgs e)
        {
            System.Drawing.Point pPoint = new System.Drawing.Point(e.X, e.Y);

            if (e.Node == null)
            {
                return;
            }
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            string nodetag = e.Node.Tag.ToString();
            TreeDataLib.SelectedNode = e.Node;
            if (nodetag.Contains("DataDIR"))
            {
                nodetag = "DataDIR";
            }
            DevComponents.DotNetBar.ButtonItem item = null;
            if (!_isLayerConfig)//展示系统右键菜单
            {

                if (_MapContextMenu != null) this.Controls.Add(_MapContextMenu);
                if (_LayerContextMenu != null) this.Controls.Add(_LayerContextMenu);
                switch (nodetag)
                {

                    case "Root":
                        //    if (_MapContextMenu != null)
                        //    {
                        //        item = _MapContextMenu.Items[0] as DevComponents.DotNetBar.ButtonItem;
                        //        if (item != null)
                        //        {
                        //            item.Popup(TreeDataLib.PointToScreen(pPoint));
                        //        }
                        //    }
                        break;
                    case "Layer":

                        //获取xml节点
                        if (e.Node.DataKey != null)
                        {
                            XmlNode layerNode = e.Node.DataKey as XmlNode;
                            string nodeKey = "";
                            if ((layerNode as XmlElement).HasAttribute("NodeKey"))
                            {
                                nodeKey = layerNode.Attributes["NodeKey"].Value;
                            }
                            ILayer addLayer = null;
                            //获取图层
                            Exception errLayer = null;
                            if (_DicAddLyrs.ContainsKey(nodeKey))
                                addLayer = _DicAddLyrs[nodeKey];    //若已加载到当前视图
                            else if (_DicDelLyrs.ContainsKey(nodeKey))
                            {
                                addLayer = _DicDelLyrs[nodeKey];    //若从当前卸载
                            }
                            else
                            {   //若是新添加的图层
                                addLayer = ModuleMap.AddLayerFromXml(SysCommon.ModuleMap._DicDataLibWks, layerNode, m_pWks, "", null, out errLayer);
                            }

                            if (_TocControl.Buddy is IPageLayoutControl2)
                            {
                                IPageLayoutControl2 pPageLayoutControl = _TocControl.Buddy as IPageLayoutControl2;
                                pPageLayoutControl.CustomProperty = addLayer;

                            }
                            else if (_TocControl.Buddy is IMapControl3)
                            {
                                IMapControl3 pMapcontrol = _TocControl.Buddy as IMapControl3;
                                pMapcontrol.CustomProperty = addLayer;
                            }
                        }

                        break;
                    case "OutLayer"://外部加载的图层 added by chulili 20110902
                        //获取xml节点
                        if (e.Node.DataKey != null)
                        {
                            ILayer pLayer = e.Node.DataKey as ILayer;
                            //if (_LayerContextMenu != null)
                            //{
                            //    item = _LayerContextMenu.Items[0] as DevComponents.DotNetBar.ButtonItem;
                            //    /*xisheng 20110909 修改右键菜单位置 */
                            //    int y = TreeDataLib.PointToScreen(pPoint).Y;
                            //    int  x = TreeDataLib.PointToScreen(pPoint).X;
                            //    if ((y + 170) > SystemInformation.WorkingArea.Height)
                            //        y = SystemInformation.WorkingArea.Height - 170;
                            //    if (item != null)
                            //    {
                            //        //item.Popup(TreeDataLib.PointToScreen(pPoint));
                            //        item.Popup(x, y);
                            //    }
                            //}

                            if (_TocControl.Buddy is IPageLayoutControl2)
                            {
                                IPageLayoutControl2 pPageLayoutControl = _TocControl.Buddy as IPageLayoutControl2;
                                pPageLayoutControl.CustomProperty = pLayer;

                            }
                            else if (_TocControl.Buddy is IMapControl3)
                            {
                                IMapControl3 pMapcontrol = _TocControl.Buddy as IMapControl3;
                                pMapcontrol.CustomProperty = pLayer;
                            }
                        }
                        break;
                }
                if (_LayerContextMenu != null)
                {
                    item = _LayerContextMenu.Items[0] as DevComponents.DotNetBar.ButtonItem;
                    string strTagLower = nodetag.ToLower();
                    # region 菜单弹出之前，判断子项的可见状态，这段代码与配置文件有关联，是在已知子菜单项的名字前提下写出 chulili 20111216
                    try//added by chulili 20111216 错误保护，用户修改了菜单配置文件后，可能有的子项找不到
                    {
                        if (strTagLower == "root")
                        {
                            item.SubItems["GeoSysUpdate.ControlsAddDataCommand"].Visible = true;    //加载外部数据
                            item.SubItems["GeoUtilities.ControlsSetCoordinateSys1"].Visible = true; //设置空间参考
                            item.SubItems["GeoSysUpdate.ControlsAddjustLayerOrder"].Visible = true; //调整图层顺序
                            item.SubItems["GeoSysUpdate.ControlsExpandAllNode"].Visible = true;     //展开节点
                            item.SubItems["GeoSysUpdate.ControlsCollapseAllNode"].Visible = true;   //折叠节点
                            item.SubItems["GeoUtilities.ControlsSetLimitScale1"].Visible = true;    //设置限制比例尺
                            item.SubItems["GeoUtilities.ControlsZoomToLayer"].Visible = false;      //缩放到图层
                            item.SubItems["GeoUtilities.ControlsZoomToVisibleScale1"].Visible = false;   //缩放到可见比例尺
                            item.SubItems["GeoSysUpdate.ControlsRemoveLayer"].Visible = false;       //移除图层
                            item.SubItems["GeoUtilities.ControlsSetLabel"].Visible = false;      //设置标注
                            item.SubItems["GeoUtilities.ControlsDisplayLabel"].Visible = false;  //移除标注
                            item.SubItems["GeoUserManager.CommandSymbol"].Visible = false;       //符号设置
                            item.SubItems["GeoUtilities.ControlsLayerAttribute"].Visible = false;    //打开属性表
                            item.SubItems["GeoUtilities.ControlsLayerAtt"].Visible = false;      //图层属性
                        }
                        else if (strTagLower.Contains("layer"))
                        {
                            item.SubItems["GeoSysUpdate.ControlsAddDataCommand"].Visible = false;    //加载外部数据
                            item.SubItems["GeoUtilities.ControlsSetCoordinateSys1"].Visible = false; //设置空间参考
                            item.SubItems["GeoSysUpdate.ControlsAddjustLayerOrder"].Visible = true; //调整图层顺序
                            item.SubItems["GeoSysUpdate.ControlsExpandAllNode"].Visible = false;     //展开节点
                            item.SubItems["GeoSysUpdate.ControlsCollapseAllNode"].Visible = false;   //折叠节点
                            item.SubItems["GeoUtilities.ControlsSetLimitScale1"].Visible = true;    //设置限制比例尺
                            item.SubItems["GeoUtilities.ControlsZoomToLayer"].Visible = true;      //缩放到图层
                            item.SubItems["GeoUtilities.ControlsZoomToVisibleScale1"].Visible = true;   //缩放到可见比例尺
                            item.SubItems["GeoSysUpdate.ControlsRemoveLayer"].Visible = true;       //移除图层
                            item.SubItems["GeoUtilities.ControlsSetLabel"].Visible = true;      //设置标注
                            item.SubItems["GeoUtilities.ControlsDisplayLabel"].Visible = true;  //移除标注
                            item.SubItems["GeoUserManager.CommandSymbol"].Visible = true;       //符号设置
                            item.SubItems["GeoUtilities.ControlsLayerAttribute"].Visible = true;    //打开属性表
                            item.SubItems["GeoUtilities.ControlsLayerAtt"].Visible = true;      //图层属性
                        }
                        else
                        {
                            item.SubItems["GeoSysUpdate.ControlsAddDataCommand"].Visible = false;    //加载外部数据
                            item.SubItems["GeoUtilities.ControlsSetCoordinateSys1"].Visible = false; //设置空间参考
                            item.SubItems["GeoSysUpdate.ControlsAddjustLayerOrder"].Visible = true; //调整图层顺序
                            item.SubItems["GeoSysUpdate.ControlsExpandAllNode"].Visible = true;     //展开节点
                            item.SubItems["GeoSysUpdate.ControlsCollapseAllNode"].Visible = true;   //折叠节点
                            item.SubItems["GeoUtilities.ControlsSetLimitScale1"].Visible = true;    //设置限制比例尺
                            item.SubItems["GeoUtilities.ControlsZoomToLayer"].Visible = false;      //缩放到图层
                            item.SubItems["GeoUtilities.ControlsZoomToVisibleScale1"].Visible = false;   //缩放到可见比例尺
                            item.SubItems["GeoSysUpdate.ControlsRemoveLayer"].Visible = false;       //移除图层
                            item.SubItems["GeoUtilities.ControlsSetLabel"].Visible = false;      //设置标注
                            item.SubItems["GeoUtilities.ControlsDisplayLabel"].Visible = false;  //移除标注
                            item.SubItems["GeoUserManager.CommandSymbol"].Visible = false;       //符号设置
                            item.SubItems["GeoUtilities.ControlsLayerAttribute"].Visible = false;    //打开属性表
                            item.SubItems["GeoUtilities.ControlsLayerAtt"].Visible = false;      //图层属性
                        }
                    }
                    catch
                    { }
                    # endregion
                    /*xisheng 20110909 修改右键菜单位置 */
                    int y = TreeDataLib.PointToScreen(pPoint).Y, x = TreeDataLib.PointToScreen(pPoint).X;
                    if ((y + 170) > SystemInformation.WorkingArea.Height)
                        y = SystemInformation.WorkingArea.Height - 170;

                    if (item != null)
                    {
                        //item.Popup(TreeDataLib.PointToScreen(pPoint));
                        item.Popup(x, y);
                    }
                    //end
                }


            }


            else//配置子系统右键菜单
            {

                if (_LayerTreeManageContextMenu != null)
                {
                    this.Controls.Add(_LayerTreeManageContextMenu);
                    item = _LayerTreeManageContextMenu.Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        item.Popup(TreeDataLib.PointToScreen(pPoint));
                    }
                }
                if (e.Node.DataKey != null)
                {
                    XmlNode layerNode = e.Node.DataKey as XmlNode;
                    string nodeKey = "";
                    if ((layerNode as XmlElement).HasAttribute("NodeKey"))
                    {
                        nodeKey = layerNode.Attributes["NodeKey"].Value;
                    }
                    ILayer addLayer = null;
                    //获取图层
                    Exception errLayer = null;
                    if (_DicAddLyrs.ContainsKey(nodeKey))
                        addLayer = _DicAddLyrs[nodeKey];    //若已加载到当前视图
                    if (_TocControl != null)
                    {
                        if (_TocControl.Buddy is IPageLayoutControl2)
                        {
                            IPageLayoutControl2 pPageLayoutControl = _TocControl.Buddy as IPageLayoutControl2;
                            pPageLayoutControl.CustomProperty = addLayer;

                        }
                        else if (_TocControl.Buddy is IMapControl3)
                        {
                            IMapControl3 pMapcontrol = _TocControl.Buddy as IMapControl3;
                            pMapcontrol.CustomProperty = addLayer;
                        }
                    }
                }

                //this.contextMenuLayerTree.Visible = true;
                //switch (nodetag)
                //{
                //    case "Root"://根节点
                //        //有三个通用菜单，所有级别的节点都可用
                //        //this.contextMenuLayerTree.Items["MenuItemSetDbsource"].Enabled = true;
                //        //this.contextMenuLayerTree.Items["MenuItemSetRender"].Enabled = true;

                //        this.contextMenuLayerTree.Items["MenuItemAddFolder"].Enabled = true;
                //        this.contextMenuLayerTree.Items["MenuItemAddGroup"].Enabled = true;
                //        this.contextMenuLayerTree.Items["MenuItemAddLayer"].Enabled = true;

                //        this.contextMenuLayerTree.Items["MenuItemAddDataset"].Enabled = true;

                //        //this.contextMenuLayerTree.Items["MenuItemModify"].Enabled = true;
                //        this.contextMenuLayerTree.Items["MenuDelete"].Enabled = false;

                //        this.contextMenuLayerTree.Items["MenuItemZoomToLayer"].Enabled = false;
                //        break;
                //    case "DIR"://文件夹节点（专题节点）
                //        //this.contextMenuLayerTree.Items["MenuItemSetDbsource"].Enabled = true;
                //        //this.contextMenuLayerTree.Items["MenuItemSetRender"].Enabled = true;

                //        this.contextMenuLayerTree.Items["MenuItemAddFolder"].Enabled = true;
                //        this.contextMenuLayerTree.Items["MenuItemAddGroup"].Enabled = true;
                //        this.contextMenuLayerTree.Items["MenuItemAddLayer"].Enabled = true;

                //        this.contextMenuLayerTree.Items["MenuItemAddDataset"].Enabled = true;

                //        //this.contextMenuLayerTree.Items["MenuItemModify"].Enabled = true;
                //        this.contextMenuLayerTree.Items["MenuDelete"].Enabled = true;

                //        this.contextMenuLayerTree.Items["MenuItemZoomToLayer"].Enabled = false;
                //        break;
                //    case "DataDIR"://数据集节点（图层组节点）
                //        //this.contextMenuLayerTree.Items["MenuItemSetDbsource"].Enabled = true;
                //        //this.contextMenuLayerTree.Items["MenuItemSetRender"].Enabled = true;

                //        this.contextMenuLayerTree.Items["MenuItemAddFolder"].Enabled = false;
                //        this.contextMenuLayerTree.Items["MenuItemAddGroup"].Enabled = false;
                //        this.contextMenuLayerTree.Items["MenuItemAddLayer"].Enabled = true;

                //        this.contextMenuLayerTree.Items["MenuItemAddDataset"].Enabled = true;//changed by chulili 20110808允许在分组节点下添加数据集

                //        //this.contextMenuLayerTree.Items["MenuItemModify"].Enabled = true;
                //        this.contextMenuLayerTree.Items["MenuDelete"].Enabled = true;

                //        this.contextMenuLayerTree.Items["MenuItemZoomToLayer"].Enabled = false;
                //        break;
                //    case "Layer"://图层节点
                //        //this.contextMenuLayerTree.Items["MenuItemSetDbsource"].Enabled = true ;
                //        //this.contextMenuLayerTree.Items["MenuItemSetRender"].Enabled = true;

                //        this.contextMenuLayerTree.Items["MenuItemAddFolder"].Enabled = false;
                //        this.contextMenuLayerTree.Items["MenuItemAddGroup"].Enabled = false;
                //        this.contextMenuLayerTree.Items["MenuItemAddLayer"].Enabled = false;

                //        this.contextMenuLayerTree.Items["MenuItemAddDataset"].Enabled = false;

                //        //this.contextMenuLayerTree.Items["MenuItemModify"].Enabled = true;
                //        this.contextMenuLayerTree.Items["MenuDelete"].Enabled = true;

                //        this.contextMenuLayerTree.Items["MenuItemZoomToLayer"].Enabled = true;
                //        break;
                //}

            }
        }
        //批量设置图层属性  加载 显示 可编辑 可查询  可选择  以及最大 最小比例尺
        public void SetLayerAttributes()
        {
            DevComponents.AdvTree.Node pNode = TreeDataLib.SelectedNode;// layerTree.SelectedNode;
            if (pNode == null)
                return;
            try
            {
                //RemoveNodeFromMap(pNode);//先从视图中删除节点
                FormSetLayerAtt pFrm = new FormSetLayerAtt(this,_LayerXmlPath, pNode);
                DialogResult pResult= pFrm.ShowDialog();
                if (pResult == DialogResult.OK)
                {
                    if (pFrm.GetLoad())
                    {
                        SysCommon.CProgress vProgress = null;
                        vProgress = new SysCommon.CProgress("加载'" + pNode.Text + "'");
                        //vProgress.EnableCancel = false;
                        vProgress.ShowDescription = false ;
                        vProgress.FakeProgress = true;
                        vProgress.TopMost = true;
                        vProgress.EnableCancel = true;
                        vProgress.EnableUserCancel(true);
                        vProgress.ShowProgress();
                        SetCheckState(pNode, CheckState.Checked, vProgress);
                        vProgress.Close();
                        vProgress = null;
                    }
                    else
                    {
                        SetCheckState(pNode, CheckState.Unchecked,null);
                    }
                }
                //if (pNode.Checked)
                //{
                //    AddNodeToMap(pNode);
                //    m_pMapCtl.ActiveView.Refresh();
                //}
                pFrm = null;
            }
            catch
            { }
        }
        private void SetCheckState(DevComponents.AdvTree.Node pNode,CheckState pCheckstate,SysCommon.CProgress vprog)
        {
            if (pNode == null)
                return;
            string strtag = pNode.Tag.ToString();
            if (strtag.Contains("DataDIR"))
            {
                strtag = "DataDIR";
            }
            switch (strtag)
            {
                case "Root":
                case"DIR":         
                    
                case "DataDIR":
                case "Layer":
                    pNode.SetChecked(pCheckstate,DevComponents.AdvTree.eTreeAction.Mouse);
                    break;
            }
            
        }
        public void RefreshOrderIDofLayer(string strNodeKey,string strXML)
        {
            ILayer pLayer = null;
            if (_DicAddLyrs != null)
            {
                if (_DicAddLyrs.Keys.Contains(strNodeKey))
                {
                    pLayer = _DicAddLyrs[strNodeKey];

                }
                else if (_DicDelLyrs != null)
                {
                    if (_DicDelLyrs.Keys.Contains(strNodeKey))
                    {
                        pLayer = _DicDelLyrs[strNodeKey];
                    }
                }
            }
            if (pLayer != null)
            {
                ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
                //读取图层的描述
                pLayerGenPro.LayerDescription = strXML;
            }
        }
        public void CopyLayer()
        {
            _CopyLayerNode = null;
            if (this.TreeDataLib.SelectedNode == null)
            {
                return;
            }
            //获取所选图层节点
            DevComponents.AdvTree.Node vNode = TreeDataLib.SelectedNode;
            //若不是图层节点，则跳出
            if (!vNode.Tag.ToString().Equals("Layer"))
                return;

            if (vNode.DataKey == null) return;
            try
            {
                //获取xml节点
                XmlNode layerNode = vNode.DataKey as XmlNode;
                string nodeKey = layerNode.Attributes["NodeKey"].Value;

                ILayer addLayer = null;
                //获取图层
                if (_DicAddLyrs.ContainsKey(nodeKey))
                    addLayer = _DicAddLyrs[nodeKey];    //若已加载到当前视图
                else if (_DicDelLyrs.ContainsKey(nodeKey))
                {
                    addLayer = _DicDelLyrs[nodeKey];    //若从当前卸载
                }
                else
                {   //若是新添加的图层
                    Exception errLayer = null;
                    addLayer = ModuleMap.AddLayerFromXml(ModuleMap._DicDataLibWks, layerNode, m_pWks, "", null, out errLayer);
                }
                if (addLayer == null) return;

                _CopyLayerNode = vNode.Copy();
            }
            catch (Exception eError)
            {
                if (SysCommon.Log.Module.SysLog == null) SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
            }
        }
        private void MenuItemPasteLayer_Click(object sender, EventArgs e)
        {
            PasteLayer();
        }
        public void PasteLayer()
        {
            if (_CopyLayerNode == null)
            {
                return;
            }
            if (this.TreeDataLib.SelectedNode == null)
            {
                return;
            }
            //获取所选图层节点
            bool bCheckState = _CopyLayerNode.Checked;

            _CopyLayerNode.CheckState = CheckState.Unchecked;
            DevComponents.AdvTree.Node vNode = TreeDataLib.SelectedNode;
            //若不是图层节点，则跳出
            if (!vNode.Tag.ToString().Equals("Layer"))
                return;
            if (vNode.Parent != null)
            {
                DevComponents.AdvTree.Node pParent = vNode.Parent;
                
                XmlDocument XMLDoc = new XmlDocument();
                XMLDoc.Load(_LayerXmlPath);
                //shduan 20110612 修改*******************************************************************************************
                //string strSearch = "//" + pNode.Tag + "[@NodeKey='" + pNode.Name + "']";
                string strSearch = "";
                if (vNode.Tag.ToString().Contains("DataDIR"))
                {
                    strSearch = "//DataDIR" + "[@NodeKey='" + vNode.Name + "']";
                }
                else
                {
                    strSearch = "//" + vNode.Tag + "[@NodeKey='" + vNode.Name + "']";
                }
                string strCopy = "";
                strCopy = "//" + _CopyLayerNode.Tag + "[@NodeKey='" + _CopyLayerNode.Name + "']";

                //****************************************************************************************************************
                XmlNode pxmlnode = XMLDoc.SelectSingleNode(strSearch);
                XmlNode pxmlnodeCopy = XMLDoc.SelectSingleNode(strCopy);
                if (pxmlnode != null)
                {
                    XmlNode pParentXmlnode = pxmlnode.ParentNode;
                    if (pParent != null && pParentXmlnode!=null)
                    {
                        
                        XmlNode pnewNode = pxmlnodeCopy.Clone();
                        XmlElement pNewEle=pnewNode as XmlElement;
                        string strNewNodeKey = System.Guid.NewGuid().ToString();
                        pNewEle.SetAttribute("NodeKey", strNewNodeKey);
                        pNewEle.SetAttribute("NodeText", _CopyLayerNode.Text+"复件");
                        _CopyLayerNode.Text = _CopyLayerNode.Text + "复件";
                        _CopyLayerNode.Name = strNewNodeKey;
                        pParentXmlnode.InsertBefore(pnewNode, pxmlnode);
                        _CopyLayerNode.DataKey = pnewNode as object;
                        pParent.Nodes.Insert(vNode.Index, _CopyLayerNode);

                        
                        XMLDoc.Save(_LayerXmlPath);
                        if (bCheckState)
                        {
                            _CopyLayerNode.SetChecked(true, eTreeAction.Mouse);
                        }
                        _CopyLayerNode = _CopyLayerNode.Copy();
                    }
                    
                }
                XMLDoc = null;
            }
        }
    }
}
