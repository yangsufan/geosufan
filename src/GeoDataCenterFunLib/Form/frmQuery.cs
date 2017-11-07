using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.Xml;
using System.IO;

namespace GeoDataCenterFunLib
{
    public enum enumQueryMode
    {
        Top=0,
        Visiable=1,
        Selectable=2,
        //CurEdit=3,//changed by chulili 20110707现不再有当前编辑图层选项
        All=3,
        Layer=4
    }

    public partial class frmQuery : DevComponents.DotNetBar.Office2007Form
    {
        private DataTable m_dataSourceGrid;        //grid的datasource
        private IMapControlDefault m_pMapControl;                        //被查询的地图对象
        private enumQueryMode m_enumQueryMode;     //查询方式
        private bool _IsHightLight=false;
        private string _QueryTag = "frmQuery";
        private IGeometry m_DrawGeometry = null;
        ///ZQ 201118 modify
        private  IActiveViewEvents_Event m_pActiveViewEvents;
        public enumQueryMode QueryMode
        {
            get
            {
                return m_enumQueryMode;
            }
        }
		public IMap m_Map { get; set; }// 20110802 xisheng 
        public IGeometry m_Geometry { get; set; }//20110802 xisheng 
        IActiveView m_ActiveView = null;
		private IFeatureClass m_pFeatureClass; //20111119 xisheng 记住选择图层
        private string LayerName = "";

        private DevComponents.AdvTree.Node curNode = null;//yjl20110818 add for alert if more than 100
        private int queryThreshold=100;//yjl20110819 add for change from xml
        private IQueryFilter m_pQueryFilter = null;//yjl20110819 add for 属性查询

        private esriSpatialRelEnum m_esriSpatialRelEnum;
        /// <summary>
        /// 初始化，用来SQL查询
        /// </summary>
        /// <param name="pMapControl"></param>
        public frmQuery(IMapControlDefault pMapControl)
        {
            InitializeComponent();
            labelItem.Visible = false;
            comboBoxItem.Visible = false;
            queryThreshold = getQueryThreshold();//yjl20110819
            InitializeGrid();      //初始化Grid的表现  
            m_pMapControl = pMapControl;
            //added by chulili 20110731 初始化字段中英文对照字典
            if (SysCommon.ModField._DicFieldName.Keys.Count ==0)
            {
                SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            if (SysCommon.ModField._DicMatchFieldValue.Keys.Count == 0)
            {
                SysCommon.ModField.InitMatchFieldValueDic(Plugin.ModuleCommon.TmpWorkSpace);
            }
            if (SysCommon.ModField._ListHideFields == null)
            {
                SysCommon.ModField.GetHideFields();
            }
            //end added by chulili
        }
        /// <summary>
        /// 适用于包含、落入和相交查询
        /// </summary>
        /// <param name="pMapControl">被查询的地图对象</param>
        /// <param name="penumQueryMode">查询方式</param>
        public frmQuery(IMapControlDefault pMapControl, enumQueryMode penumQueryMode,bool isDiff)
        {
            InitializeComponent();
            labelItem.Visible = false;
            comboBoxItem.Visible = false;
            queryThreshold = getQueryThreshold();//yjl20110819
            InitializeGrid();      //初始化Grid的表现  
            m_pMapControl = pMapControl;
            m_enumQueryMode = penumQueryMode;
            InitiaComboBox();
            switch(penumQueryMode)
            {
                case enumQueryMode.Top:

                    comboBoxItem.SelectedIndex = 0;
                    break;
                case enumQueryMode.Visiable:
				case enumQueryMode.Layer:
                    comboBoxItem.SelectedIndex = 1;
                    break;
                case enumQueryMode.Selectable:
                    comboBoxItem.SelectedIndex = 2;
                    break;
                //case enumQueryMode.CurEdit://changed by chulili 20110707现不再有当前编辑图层选项
                //    comboBoxItem.SelectedIndex = 3;
                    //break;
                case enumQueryMode.All:
                    comboBoxItem.SelectedIndex = 3;
                    break;
            }
            //added by chulili 20110731 初始化字段中英文对照字典
            if (SysCommon.ModField._DicFieldName.Keys.Count == 0)
            {
                SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            if (SysCommon.ModField._DicMatchFieldValue.Keys.Count == 0)
            {
                SysCommon.ModField.InitMatchFieldValueDic(Plugin.ModuleCommon.TmpWorkSpace);
            }
            if (SysCommon.ModField._ListHideFields == null)
            {
                SysCommon.ModField.GetHideFields();
            }
            //if (ModQuery._DicGBname.Keys.Count == 0)
            //{
            //    ModQuery.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, ModQuery._DicGBname,"国标对照表");
            //}
            //if (ModQuery._DicClassname.Keys.Count == 0)
            //{
            //    ModQuery.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, ModQuery._DicClassname, "地名分类对照表");
            //}
            //if (ModQuery._DicXZQname.Keys.Count == 0)
            //{
            //    ModQuery.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, ModQuery._DicXZQname, "行政区编码表");
            //}
            //end added by chulili
        }
        ///ZQ 201118 modify
        //被擦去时画出
        internal void AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            if (this.IsDisposed == true) return;
            if (phase == esriViewDrawPhase.esriViewForeground) drawgeometryXOR(m_DrawGeometry);
        }
        private void frmQuery_Load(object sender, EventArgs e)
        {
            m_ActiveView = m_pMapControl.Map as IActiveView;
            m_pActiveViewEvents = m_ActiveView as IActiveViewEvents_Event;
            SysCommon.ScreenDraw.list.Add(AfterDraw);
        }
        ///end
        /// <summary>
        /// 查询该图层是否可查询 added by xisheng 20110802
        /// </summary>
        /// <param name="layer">图层</param>
        /// <returns></returns>
        public bool GetIsQuery(IFeatureLayer layer)
        {
            ILayerGeneralProperties pLayerGenPro = layer as ILayerGeneralProperties;
            //读取该图层的描述信息，转成xml节点
            string strNodeXml = pLayerGenPro.LayerDescription;

            if (strNodeXml.Equals(""))
            {
                return true;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.LoadXml(strNodeXml);
            //获取节点的NodeKey信息
            XmlNode pxmlnode = pXmldoc.SelectSingleNode("//AboutShow");
            if (pxmlnode == null)
            {
                pXmldoc = null;
                return true;
            }
            string strNodeKey =null;
            try
            {
                strNodeKey = pxmlnode.Attributes["IsQuery"].Value.ToString();
            }
            catch { strNodeKey = "FALSE"; }
            
            if (strNodeKey.Trim().ToUpper() == "FALSE")
            {
                pXmldoc = null;
                return false;
            }
            else
            {
                pXmldoc = null;
                return true;
            }

        }
        /// 适用于一般查询,缓冲查询,选择查询等一般查询
        /// </summary>
        /// <param name="pMapControl">被查询的地图对象</param>
        /// <param name="penumQueryMode">查询方式</param>
        public frmQuery(IMapControlDefault pMapControl, enumQueryMode penumQueryMode)
        {
            InitializeComponent();
            labelItem.Visible = true;
            comboBoxItem.Visible = true;
            queryThreshold = getQueryThreshold();//yjl20110819
            InitializeGrid();      //初始化Grid的表现  
            m_pMapControl = pMapControl;
            m_enumQueryMode = penumQueryMode;
            InitiaComboBox();
            switch (penumQueryMode)
            {
                case enumQueryMode.Top:

                    comboBoxItem.SelectedIndex = 0;
                    break;
                case enumQueryMode.Visiable:
				case enumQueryMode.Layer:
                    comboBoxItem.SelectedIndex = 1;
                    break;
                case enumQueryMode.Selectable:
                    comboBoxItem.SelectedIndex = 2;
                    break;
                //case enumQueryMode.CurEdit://changed by chulili 20110707现不再有当前编辑图层选项
                //    comboBoxItem.SelectedIndex = 3;
                //    break;
                case enumQueryMode.All:
                    comboBoxItem.SelectedIndex = 3;
                    break;
            }
            //added by chulili 20110731 初始化字段中英文对照字典
            if (SysCommon.ModField._DicFieldName.Keys.Count == 0)
            {
                SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            if (SysCommon.ModField._DicMatchFieldValue.Keys.Count == 0)
            {
                SysCommon.ModField.InitMatchFieldValueDic(Plugin.ModuleCommon.TmpWorkSpace);
            }
            if (SysCommon.ModField._ListHideFields == null)
            {
                SysCommon.ModField.GetHideFields();
            }
            //if (ModQuery._DicGBname.Keys.Count == 0)
            //{
            //    ModQuery.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, ModQuery._DicGBname,"国标对照表");
            //}
            //if (ModQuery._DicClassname.Keys.Count == 0)
            //{
            //    ModQuery.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, ModQuery._DicClassname,"地名分类对照表");
            //}
            //if (ModQuery._DicXZQname.Keys.Count == 0)
            //{
            //    ModQuery.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, ModQuery._DicXZQname, "行政区编码表");
            //}
            //end added by chulili
        }
        /// <summary>
        /// 将地图中的图层加载到列表中 added by xisheng 20110731
        /// </summary>
        private void InitiaComboBox()
        {

            comboBoxItem.Items.Add("选择查询图层");
            //改变选择单个图层的选择方式 屏蔽之前代码 xisheng 20111119
            //for (int i = 0; i < m_pMapControl.Map.LayerCount; i++)
            //{
            //    if (m_pMapControl.Map.get_Layer(i) is IGroupLayer)
            //    {
            //        ICompositeLayer pLayer = m_pMapControl.Map.get_Layer(i) as ICompositeLayer;
            //        for (int j = 0; j < pLayer.Count; j++)
            //        {
            //            ILayer pFLayer = pLayer.get_Layer(j) as ILayer;
            //            if (pFLayer != null)
            //            {
            //                if (pFLayer is IFeatureLayer)
            //                {
            //                    IFeatureLayer pFeatureLayer = pFLayer as IFeatureLayer;
            //                    if (GetIsQuery(pFeatureLayer))//判断是否可查询 0802
            //                    {
            //                        comboBoxItem.Items.Add(pFeatureLayer.Name);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        ILayer pFLayer = m_pMapControl.Map.get_Layer(i) as ILayer;
            //        if (pFLayer != null)
            //        {
            //            if (pFLayer is IFeatureLayer)
            //            {
            //                IFeatureLayer pFeatureLayer = pFLayer as IFeatureLayer;
            //                if (GetIsQuery(pFeatureLayer))//判断是否可查询 0802
            //                {
            //                    comboBoxItem.Items.Add(pFeatureLayer.Name);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        //初始化grid 显示和数据
        private void InitializeGrid()
        {
            dataGridViewX.Columns.Clear();
            dataGridViewX.ReadOnly = true;
            dataGridViewX.AutoGenerateColumns = false;
            m_dataSourceGrid = new DataTable();
            m_dataSourceGrid.Columns.Add("Name", typeof(System.String));
            m_dataSourceGrid.Columns["Name"].ReadOnly = true;
            m_dataSourceGrid.Columns.Add("Value", typeof(System.String));
            m_dataSourceGrid.Columns["Value"].ReadOnly = true;
            AddColToGrid(dataGridViewX, "字段名称", "Name", true, 132, "TextBox");
            AddColToGrid(dataGridViewX, "字段值", "Value", true, 132, "TextBox");
            dataGridViewX.Columns[dataGridViewX.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewX.DataSource = m_dataSourceGrid;
        }
        private void AddColToGrid(DataGridView axiGrid, string HeaderText, string Key, bool Visible, int Width, string CellType)
        {
            DataGridViewColumn vColumn;
            switch (CellType)
            {
                case "TextBox":
                    vColumn = new DataGridViewTextBoxColumn();
                    break;
                case "Button":
                    vColumn = new DataGridViewButtonColumn();
                    break;
                case "CheckBox":
                    vColumn = new DataGridViewCheckBoxColumn();
                    break;
                case "ComboBox":
                    vColumn = new DataGridViewComboBoxColumn();
                    break;
                case "Image":
                    vColumn = new DataGridViewImageColumn();
                    break;
                case "Link":
                    vColumn = new DataGridViewLinkColumn();
                    break;
                default:
                    return;
            }

            vColumn.Visible = Visible;
            vColumn.Width = Width;
            vColumn.HeaderText = HeaderText;
            vColumn.Name = Key;
            vColumn.DataPropertyName = Key; //"C" + (axiGrid.Columns.Count + 1).ToString();
            axiGrid.Columns.Add(vColumn);
        }

        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isCancel = false;// xisheng 20111121 判断选择图层是否点击取消
            switch (comboBoxItem.SelectedIndex)
            {
                case 0:
                    m_enumQueryMode = enumQueryMode.Top;
                    break;
                case 1:
                    m_enumQueryMode = enumQueryMode.Visiable;
                    break;
                case 2:
                    m_enumQueryMode = enumQueryMode.Selectable;
                    break; 
                //case 3:
                //    m_enumQueryMode = enumQueryMode.CurEdit;//changed by chulili 20110707现不再有当前编辑图层选项
                //    break;
                case 3:
                    m_enumQueryMode = enumQueryMode.All;
                    break;
                default://选择单个图层的时候 xisheng 20110731; 改变选择单个图层选择方式 xisheng 20111119
                    m_enumQueryMode = enumQueryMode.Layer;
                    Plugin.SelectLayerByTree frm = new Plugin.SelectLayerByTree(1);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        
                        if (frm.m_NodeKey.Trim() != "")
                        {
                            m_pFeatureClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, frm._LayerTreePath, frm.m_NodeKey);
                        }

                        if (m_pFeatureClass != null)
                        {

                            LayerName = frm.m_NodeText;
                            //comboBoxItem.Text = LayerName;
                        }
                    }
                    else
                    {
                        isCancel = true;
                    }

                    break;
            }
            if (!isCancel)
            {
                FillData(m_Map, m_Geometry, m_esriSpatialRelEnum);//20110802 xisheng
            }
        }

        /// <summary>
        /// 根据范围查找数据填充窗体
        /// </summary>
        /// <param name="pMap">被查询的地图对象</param>
        /// <param name="pGeometry">查询范围</param>
        public void FillData(IMap pMap, IGeometry pGeometry, esriSpatialRelEnum pesriSpatialRelEnum)
        {
            advTree.Nodes.Clear();
            m_dataSourceGrid.Clear();
			m_Map = pMap;//xisheng 20110802
            m_Geometry = pGeometry;//xisheng 20110802
            m_esriSpatialRelEnum = pesriSpatialRelEnum;
            if (pGeometry == null)
            {
                labelItemMemo.Text = "查找到0个要素";
                return;
            }

            UID pUID = new UIDClass();
            pUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";   //UID for IFeatureLayer
            IEnumLayer pEnumLayer=pMap.get_Layers(pUID, true);
            pEnumLayer.Reset();
            ILayer pLayer = pEnumLayer.Next();
            
            //获取查询的图层
            List<ILayer> listLays = new List<ILayer>();
            switch (m_enumQueryMode)
            {
                //case enumQueryMode.Top://原有可见图层
                //case enumQueryMode.Visiable:
                //    pEnumLayer.Reset();
                //    pLayer = pEnumLayer.Next();
                //    while(pLayer!=null)
                //    {
                //        if (pLayer.Visible == true&&GetIsQuery(pLayer as IFeatureLayer))//20110802
                //        {
                //            listLays.Add(pLayer);
                //        }

                //        pLayer = pEnumLayer.Next();
                //    }
                //    break;
                case enumQueryMode.Top:
                case enumQueryMode.Visiable://可见图层，暂时用比例尺大小来控制 20111031 xisheng 可见修改
                    pEnumLayer.Reset();
                    pLayer = pEnumLayer.Next();
                    while (pLayer != null)
                    {
                        if ((pLayer.MaximumScale <= pMap.MapScale || pLayer.MaximumScale == 0) && (pLayer.MinimumScale == 0 || pMap.MapScale <= pLayer.MinimumScale) && GetIsQuery(pLayer as IFeatureLayer) && pLayer.Visible == true)//20110802
                            {
                                listLays.Add(pLayer);
                            }
                        pLayer = pEnumLayer.Next();
                    }
                    break;
                case enumQueryMode.Selectable:
                    pEnumLayer.Reset();
                    pLayer = pEnumLayer.Next();
                    while(pLayer!=null)
                    {
                        IFeatureLayer pTemp = pLayer as IFeatureLayer;
                        if (pTemp.Selectable == true && GetIsQuery(pLayer as IFeatureLayer))//20110802
                        {
                            listLays.Add(pLayer);
                        }

                        pLayer = pEnumLayer.Next();
                    }
                    break;
                //case enumQueryMode.CurEdit://changed by chulili 20110707现不再有当前编辑图层选项
                //    pEnumLayer.Reset();
                //    pLayer = pEnumLayer.Next();//将当前编辑图层加进来
                //    while (pLayer != null)
                //    {
                //        IFeatureLayer pTemp = pLayer as IFeatureLayer;
                //        if (pTemp.Selectable == true)
                //        {
                //            listLays.Add(pLayer);
                //        }

                //        pLayer = pEnumLayer.Next();
                //    }
                //    break;
                case enumQueryMode.All:
                    pEnumLayer.Reset();
                    pLayer = pEnumLayer.Next();
                    while(pLayer!=null)
                    {
						//20110802
                       if( GetIsQuery(pLayer as IFeatureLayer))
                       { 
                           listLays.Add(pLayer);
                       }
                       pLayer = pEnumLayer.Next();
                    }
                    break;
                //选择单个图层 xisheng 20110731 修改选择图层方式20111119
                case enumQueryMode.Layer:
                    //pEnumLayer.Reset();
                    //pLayer = pEnumLayer.Next();
                    //while (pLayer != null)
                    //{
                    ////     if(!pLayer.Name.Contains(m_pFeatureClass.AliasName))
                    ////    {
                    ////        pLayer = pEnumLayer.Next();
                    ////        continue;

                    ////    }
                    //    listLays.Add(pLayer);
                    //    pLayer = pEnumLayer.Next();
                    //}                    
                    if (m_pFeatureClass != null)
                    {
                        IFeatureLayer pfLayer = new FeatureLayerClass();
                        pfLayer.Name = LayerName;
                        pfLayer.FeatureClass = m_pFeatureClass;
                        listLays.Add(pfLayer);
                    }
                    
                    break;
                    //xisheng 20111119 end***************************
            }

            if (listLays.Count == 0)
            {
                labelItemMemo.Text = "查找到0个要素";
                return;
            }

            SysCommon.CProgress vProgress = new SysCommon.CProgress();
            vProgress.ShowDescription = true;
            vProgress.ShowProgressNumber = true;
            vProgress.TopMost = true;
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);


            //this.Refresh();
            vProgress.MaxValue = listLays.Count;
            vProgress.ProgresssValue = 0;
            vProgress.Step = 1;
            this.Refresh();
            vProgress.ShowProgress();
            vProgress.SetProgress("正在查询数据......");
            //progressBarItem.Visible = true;
            //progressBarItem.Maximum = listLays.Count;
            //progressBarItem.Minimum = 0;
            //progressBarItem.Value = 0;
            int intCnt = 0;
            bool bFirst = true;
            //循环图层查找要素
            foreach (ILayer pLay in listLays)
            {
                vProgress.SetProgress("正在查询图层："+pLay.Name.ToString()+"数据......");
                vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
                if(vProgress.UserAskCancel)
                {
                    vProgress.Close();
                    break;
                }
               
                //progressBarItem.Value = progressBarItem.Value + 1;

                IFeatureLayer pFeatLay = pLay as IFeatureLayer;
                 switch(pesriSpatialRelEnum)
                {
                    case esriSpatialRelEnum.esriSpatialRelCrosses:
                        if (pGeometry.GeometryType == pFeatLay.FeatureClass.ShapeType && pGeometry.GeometryType.ToString() == "esriGeometryPolygon")
                        {
                            continue;
                        }
                        if (pGeometry.GeometryType == pFeatLay.FeatureClass.ShapeType && pGeometry.GeometryType.ToString() == "esriGeometryPoint")
                        {
                            continue;
                        }
                        break;
                
                }
                int intKeyFieldIndex = GetKeyFieldIndexOfLayer(pFeatLay);
                if (intKeyFieldIndex == -1)
                    intKeyFieldIndex = 0;
                ISpatialFilter pSpatialFilter=new SpatialFilterClass();
                pSpatialFilter.Geometry=pGeometry;
                //pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel= pesriSpatialRelEnum;
                
                //int iFeatureCount =pFeatLay.FeatureClass.FeatureCount(pSpatialFilter);
                //Application.DoEvents();
                //if (vProgress.UserAskCancel)
                //{
                //    vProgress.Close();
                //    break;
                //}
                //if (iFeatureCount == 0) continue;
               
                //添加图层节点
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Text = pLay.Name;
                node.Tag = pLay;
                //node.DataKey = iFeatureCount;//yjl20110818 add for save number
                node.Expand();
                int nodei=advTree.Nodes.Add(node);
                int fcount = 0;
                IFeatureCursor pFeatureCursor = pFeatLay.Search(pSpatialFilter, false);
                Application.DoEvents();
                //int nameIdx = pFeatLay.FeatureClass.FindField("name");//yjl20110819 add
                //int clasIdx = pFeatLay.FeatureClass.FindField("class");
                IFeature pFeat = pFeatureCursor.NextFeature();
                while (pFeat != null)
                {
                    if (vProgress.UserAskCancel)
                    {
                        vProgress.Close();
                        break;
                    }
                    //yjl0817数量达到100标记
                    if (fcount >= queryThreshold)
                    {
                        advTree.Nodes[nodei].DataKeyString = "T";
                        break;
                    }
                    //添加要素节点
                    DevComponents.AdvTree.Node featnode = new DevComponents.AdvTree.Node();
                    //featnode.Text = pFeat.OID.ToString();
                    featnode.Text = pFeat.get_Value(intKeyFieldIndex).ToString();
                    //if (nameIdx > -1)
                    //    featnode.Text = pFeat.get_Value(nameIdx).ToString();
                    //else if (nameIdx == -1 && clasIdx > -1)
                    //    featnode.Text = pFeat.get_Value(clasIdx).ToString();//yjl20110819 add for list not OID
                    featnode.Tag = pFeat;
                    node.Nodes.Add(featnode);
                    
                    if (bFirst == true)
                    {
                        advTree.SelectedNode = featnode;
                        m_dataSourceGrid.Clear();

                        for (int i = 0; i < pFeat.Fields.FieldCount; i++)
                        {
                            if (vProgress.UserAskCancel)
                            {
                                vProgress.Close();
                                break;
                            }
                            //added by chulili 20110731 取字段的中文名
                            string strFieldname = pFeat.Fields.get_Field(i).Name;
                            IField tmpField = pFeat.Fields.get_Field(i);//yjl20110817过滤OID
                            //added by chulili 20110801 过滤Shape相关字段
                            if (strFieldname.ToUpper().Contains("SHAPE") || strFieldname.ToUpper().Equals("T") || tmpField.Type == esriFieldType.esriFieldTypeOID || tmpField.Type == esriFieldType.esriFieldTypeGeometry)
                            {
                                continue;
                            }
                            //排除隐藏字段（与上面过滤字段的代码有重复，暂时两者都用）
                            if (SysCommon.ModField._ListHideFields != null)
                            {
                                if (SysCommon.ModField._ListHideFields.Contains(strFieldname))
                                {
                                    continue;
                                }
                            }
                            //end added by chulili
                            string strChineseName = SysCommon.ModField.GetChineseNameOfField(strFieldname);
                            //end added by chulili 
                            if (pFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                            {
                                string strGeometryType = "";
                                if (pFeat.FeatureType == esriFeatureType.esriFTSimple)
                                {
                                    switch (pFeat.Shape.GeometryType)
                                    {
                                        case esriGeometryType.esriGeometryPoint:
                                            strGeometryType = "点";
                                            break;
                                        case esriGeometryType.esriGeometryPolyline:
                                            strGeometryType = "线";
                                            break;
                                        case esriGeometryType.esriGeometryPolygon:
                                            strGeometryType = "多边形";
                                            break;
                                    }
                                }
                                else if (pFeat.FeatureType == esriFeatureType.esriFTAnnotation)
                                {
                                    strGeometryType = "注记";
                                }
                                if (string.IsNullOrEmpty(strGeometryType))
                                {
                                    strGeometryType = pFeat.Shape.GeometryType.ToString();
                                }
                                m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGeometryType });//changed by chulili 20110731
                            }
                            else
                            {
                                if (vProgress.UserAskCancel)
                                {
                                    vProgress.Close();
                                    break;
                                }
                                string strFieldValue = pFeat.get_Value(i).ToString();
                                string strFieldValueChinese = strFieldValue;
                                //if (SysCommon.ModField._DicMatchFieldValue.Keys.Contains(strFieldname))
                                //{
                                //    strFieldValueChinese = SysCommon.ModField.GetChineseOfFieldValue(strFieldname, strFieldValue);
                                //}
                                try
                                {
                                    //strFieldValueChinese = SysCommon.ModField.GetDomainValueOfFieldValue(pFeatLay.FeatureClass, strFieldname, strFieldValue);
                                    strFieldValueChinese = SysCommon.ModXZQ.GetChineseName(Plugin.ModuleCommon.TmpWorkSpace, strFieldname, strFieldValue);
                                }
                                catch
                                { }
                                m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strFieldValueChinese });

                                //switch (strFieldname)//changed by chulili 20110810
                                //{
                                //    case "GB":
                                //        string strFieldValue = pFeat.get_Value(i).ToString();
                                //        string strGBValue = ModQuery.GetChineseNameOfGB(strFieldValue);
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGBValue });
                                //        break;
                                //    case "CLASS"://地名分类中英文映射
                                //        strFieldValue = pFeat.get_Value(i).ToString();
                                //        string strClassValue = ModQuery.GetChineseNameOfClass(strFieldValue);
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strClassValue });
                                //        break;
                                //    case "GNID":
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeat.get_Value(i) });//changed by chulili 20110731
                                //        break;
                                //    default:
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeat.get_Value(i) });//changed by chulili 20110731
                                //        break;
                                //}
                            }
                        }
                        bFirst = false;
                    }

                    pFeat = pFeatureCursor.NextFeature();
                    fcount++;
                    intCnt++;
                }
                if (node.Nodes.Count == 0) { advTree.Nodes.Remove(node); }

                if (m_enumQueryMode == enumQueryMode.Top)
                {
                    if (node.Nodes.Count > 0)
                    {
                        vProgress.Close();
                        //progressBarItem.Value = listLays.Count;
                        break;
                    }
                    
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pSpatialFilter);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                pSpatialFilter = null;
                pFeatureCursor = null;
            }

            labelItemMemo.Text = "查找到" + intCnt.ToString() + "个要素";
            //progressBarItem.Visible = false;
            vProgress.Close();
            //Application.DoEvents();
            this.Refresh();

            DefaultSelNde();
        }
        ///end
        public int GetKeyFieldIndexOfLayer(IFeatureLayer pLayer)
        {
            ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
            //读取该图层的描述信息，转成xml节点
            string strNodeXml = pLayerGenPro.LayerDescription;
            if (strNodeXml.Equals(""))
            {
                return -1;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.LoadXml(strNodeXml);
            //获取节点的NodeKey信息
            XmlNode pxmlnode = pXmldoc.SelectSingleNode("//AboutShow");
            if (pxmlnode == null)
            {
                pXmldoc = null;
                return -1;
            }
            string strKeyField = "";
            XmlElement pNodeEle = pxmlnode as XmlElement;
            if (pNodeEle != null)
            {
                if (pNodeEle.HasAttribute("KeyField"))
                {
                    strKeyField = pNodeEle.GetAttribute("KeyField");
                    if (pLayer.FeatureClass != null)
                    {
                        int intIndex = pLayer.FeatureClass.Fields.FindField(strKeyField);
                        return intIndex;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// 响应【查询全部】按钮的填充当前图层节点 yjl20110818 add 空间查询
        /// </summary>
        /// <param name="pLayerNd">当前图层节点</param>
        /// <param name="pGeometry">查询范围</param>
        public void FillData(DevComponents.AdvTree.Node pLayerNd, IGeometry pGeometry)
        {

                IFeatureLayer pFeatLay = pLayerNd.Tag as IFeatureLayer;
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pGeometry;
                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                int intKeyFieldIndex = GetKeyFieldIndexOfLayer(pFeatLay );
                if (intKeyFieldIndex == -1)
                { intKeyFieldIndex = 0; }
                ///ZQ 201118 modify
                int iFeatureCount = pFeatLay.FeatureClass.FeatureCount(pSpatialFilter);
                if (iFeatureCount != 0)
                {
                    progressBarItem.Visible = true;
                    this.Refresh();

                    progressBarItem.Minimum = 0;
                    progressBarItem.Value = 0;
                    progressBarItem.Maximum = iFeatureCount;
                }
                pLayerNd.Nodes.Clear();
                //添加图层节点
                bool bFirst = true;
                int fcount = 0;
                //int nameIdx = pFeatLay.FeatureClass.FindField("name");//yjl20110819 add
                //int clasIdx = pFeatLay.FeatureClass.FindField("class");
                IFeatureCursor pFeatureCursor = pFeatLay.Search(pSpatialFilter, false);
                IFeature pFeat = pFeatureCursor.NextFeature();
                while (pFeat != null)
                {
                    progressBarItem.Value = progressBarItem.Value + 1;
                    //添加要素节点
                    DevComponents.AdvTree.Node featnode = new DevComponents.AdvTree.Node();
                    //featnode.Text = pFeat.OID.ToString();
                    featnode.Text = pFeat.get_Value(intKeyFieldIndex ).ToString();
                    //if (nameIdx > -1)
                    //    featnode.Text = pFeat.get_Value(nameIdx).ToString();
                    //else if (nameIdx == -1 && clasIdx > -1)
                    //    featnode.Text = pFeat.get_Value(clasIdx).ToString();//yjl20110819 add for list not OID
                    featnode.Tag = pFeat;
                    pLayerNd.Nodes.Add(featnode);

                    if (bFirst == true)
                    {
                        advTree.SelectedNode = featnode;
                        m_dataSourceGrid.Clear();

                        for (int i = 0; i < pFeat.Fields.FieldCount; i++)
                        {
                            //added by chulili 20110731 取字段的中文名
                            string strFieldname = pFeat.Fields.get_Field(i).Name;
                            IField tmpField = pFeat.Fields.get_Field(i);//yjl20110817过滤OID
                            //added by chulili 20110801 过滤Shape相关字段
                            if (strFieldname.ToUpper().Contains("SHAPE") || strFieldname.ToUpper().Equals("T") || tmpField.Type == esriFieldType.esriFieldTypeOID || tmpField.Type == esriFieldType.esriFieldTypeGeometry)
                            {
                                continue;
                            }
                            //end added by chulili
                            string strChineseName = SysCommon.ModField.GetChineseNameOfField(strFieldname);
                            //end added by chulili 
                            if (pFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                            {
                                string strGeometryType = "";
                                if (pFeat.FeatureType == esriFeatureType.esriFTSimple)
                                {
                                    switch (pFeat.Shape.GeometryType)
                                    {
                                        case esriGeometryType.esriGeometryPoint:
                                            strGeometryType = "点";
                                            break;
                                        case esriGeometryType.esriGeometryPolyline:
                                            strGeometryType = "线";
                                            break;
                                        case esriGeometryType.esriGeometryPolygon:
                                            strGeometryType = "多边形";
                                            break;
                                    }
                                }
                                else if (pFeat.FeatureType == esriFeatureType.esriFTAnnotation)
                                {
                                    strGeometryType = "注记";
                                }
                                if (string.IsNullOrEmpty(strGeometryType))
                                {
                                    strGeometryType = pFeat.Shape.GeometryType.ToString();
                                }
                                m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGeometryType });//changed by chulili 20110731
                            }
                            else
                            {
                                string strFieldValue = pFeat.get_Value(i).ToString();
                                string strFieldValueChinese = strFieldValue;
                                //if (SysCommon.ModField._DicMatchFieldValue.Keys.Contains(strFieldname))
                                //{
                                //    strFieldValueChinese = SysCommon.ModField.GetChineseOfFieldValue(strFieldname, strFieldValue);
                                //}
                                try
                                {
                                    strFieldValueChinese = SysCommon.ModField.GetDomainValueOfFieldValue(pFeatLay.FeatureClass, strFieldname, strFieldValue);
                                }
                                catch
                                { }
                                m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strFieldValueChinese });

                                //switch (strFieldname)//changed by chulili 20110810
                                //{
                                //    case "GB":
                                //        string strFieldValue = pFeat.get_Value(i).ToString();
                                //        string strGBValue = ModQuery.GetChineseNameOfGB(strFieldValue);
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGBValue });
                                //        break;
                                //    case "CLASS"://地名分类中英文映射
                                //        strFieldValue = pFeat.get_Value(i).ToString();
                                //        string strClassValue = ModQuery.GetChineseNameOfClass(strFieldValue);
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strClassValue });
                                //        break;
                                //    case "GNID":
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeat.get_Value(i) });//changed by chulili 20110731
                                //        break;
                                //    default:
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeat.get_Value(i) });//changed by chulili 20110731
                                //        break;
                                //}
                            }
                        }
                        bFirst = false;
                    }

                    pFeat = pFeatureCursor.NextFeature();
                    fcount++;
                    
                }


                pLayerNd.DataKey = fcount;

           labelItemMemo.Text = "查找到" + fcount.ToString() + "个要素";
            progressBarItem.Visible = false;
            this.Refresh();

            DefaultSelNde();
        }
        /// <summary>
        /// 响应【查询全部】按钮的填充当前图层节点 yjl20110818 add 属性查询
        /// </summary>
        /// <param name="pLayerNd">当前图层节点</param>
        /// <param name="pGeometry">查询范围</param>
        public void FillData(DevComponents.AdvTree.Node pLayerNd, IQueryFilter pQueryFilter)
        {

            IFeatureLayer pFeatLay = pLayerNd.Tag as IFeatureLayer;
            int intKeyFieldIndex = GetKeyFieldIndexOfLayer(pFeatLay);
            if (intKeyFieldIndex == -1)
            {
                intKeyFieldIndex = 0;
            }
            ///ZQ 201118 modify
            int iFeatureCount = pFeatLay.FeatureClass.FeatureCount(pQueryFilter);
            if (iFeatureCount != 0)
            {
                progressBarItem.Visible = true;
                this.Refresh();

                progressBarItem.Minimum = 0;
                progressBarItem.Value = 0;
                progressBarItem.Maximum = iFeatureCount;
            }
            ///end
            pLayerNd.Nodes.Clear();
            //添加图层节点
            bool bFirst = true;
            int fcount = 0;
            //int nameIdx = pFeatLay.FeatureClass.FindField("name");//yjl20110819 add
            //int clasIdx = pFeatLay.FeatureClass.FindField("class");
            IFeatureCursor pFeatureCursor = pFeatLay.Search(pQueryFilter, false);
            IFeature pFeat = pFeatureCursor.NextFeature();
            while (pFeat != null)
            {
                progressBarItem.Value = progressBarItem.Value + 1;
                //添加要素节点
                DevComponents.AdvTree.Node featnode = new DevComponents.AdvTree.Node();
                //featnode.Text = pFeat.OID.ToString();
                featnode.Text = pFeat.get_Value(intKeyFieldIndex).ToString();
                //if (nameIdx > -1)
                //    featnode.Text = pFeat.get_Value(nameIdx).ToString();
                //else if (nameIdx == -1 && clasIdx > -1)
                //    featnode.Text = pFeat.get_Value(clasIdx).ToString();//yjl20110819 add for list not OID
                featnode.Tag = pFeat;
                pLayerNd.Nodes.Add(featnode);

                if (bFirst == true)
                {
                    advTree.SelectedNode = featnode;
                    m_dataSourceGrid.Clear();

                    for (int i = 0; i < pFeat.Fields.FieldCount; i++)
                    {
                        //added by chulili 20110731 取字段的中文名
                        string strFieldname = pFeat.Fields.get_Field(i).Name;
                        IField tmpField = pFeat.Fields.get_Field(i);//yjl20110817过滤OID
                        //added by chulili 20110801 过滤Shape相关字段
                        if (strFieldname.ToUpper().Contains("SHAPE") || strFieldname.ToUpper().Equals("T") || tmpField.Type == esriFieldType.esriFieldTypeOID || tmpField.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            continue;
                        }
                        //end added by chulili
                        string strChineseName = SysCommon.ModField.GetChineseNameOfField(strFieldname);
                        //end added by chulili 
                        if (pFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            string strGeometryType = "";
                            if (pFeat.FeatureType == esriFeatureType.esriFTSimple)
                            {
                                switch (pFeat.Shape.GeometryType)
                                {
                                    case esriGeometryType.esriGeometryPoint:
                                        strGeometryType = "点";
                                        break;
                                    case esriGeometryType.esriGeometryPolyline:
                                        strGeometryType = "线";
                                        break;
                                    case esriGeometryType.esriGeometryPolygon:
                                        strGeometryType = "多边形";
                                        break;
                                }
                            }
                            else if (pFeat.FeatureType == esriFeatureType.esriFTAnnotation)
                            {
                                strGeometryType = "注记";
                            }
                            if (string.IsNullOrEmpty(strGeometryType))
                            {
                                strGeometryType = pFeat.Shape.GeometryType.ToString();
                            }
                            m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGeometryType });//changed by chulili 20110731
                        }
                        else
                        {
                            string strFieldValue = pFeat.get_Value(i).ToString();
                            string strFieldValueChinese = strFieldValue;
                            //if (SysCommon.ModField._DicMatchFieldValue.Keys.Contains(strFieldname))
                            //{
                            //    strFieldValueChinese = SysCommon.ModField.GetChineseOfFieldValue(strFieldname, strFieldValue);
                            //}
                            try
                            {
                                strFieldValueChinese = SysCommon.ModField.GetDomainValueOfFieldValue(pFeatLay.FeatureClass, strFieldname, strFieldValue);
                            }
                            catch
                            { }
                            m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strFieldValueChinese });

                            //switch (strFieldname)//changed by chulili 20110810
                            //{
                            //    case "GB":
                            //        string strFieldValue = pFeat.get_Value(i).ToString();
                            //        string strGBValue = ModQuery.GetChineseNameOfGB(strFieldValue);
                            //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGBValue });
                            //        break;
                            //    case "CLASS"://地名分类中英文映射
                            //        strFieldValue = pFeat.get_Value(i).ToString();
                            //        string strClassValue = ModQuery.GetChineseNameOfClass(strFieldValue);
                            //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strClassValue });
                            //        break;
                            //    case "GNID":
                            //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeat.get_Value(i) });//changed by chulili 20110731
                            //        break;
                            //    default:
                            //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeat.get_Value(i) });//changed by chulili 20110731
                            //        break;
                            //}
                        }
                    }
                    bFirst = false;
                }

                pFeat = pFeatureCursor.NextFeature();
                fcount++;

            }


            pLayerNd.DataKey = fcount;

            labelItemMemo.Text = "查找到" + fcount.ToString() + "个要素";
            progressBarItem.Visible = false;
            this.Refresh();

            DefaultSelNde();
        }
        /// <summary>
        /// 根据范围查找数据填充窗体
        /// </summary>
        /// <param name="pMap">被查询的地图对象</param>
        /// <param name="pGeometry">查询范围</param>
        /// <param name="inQuryType">查询类型1相交2落入3包含</param>
        /// <remarks author="yjl" date="2011-6-23" />
        public void FillData(List<ILayer> listLays, IGeometry pGeometry, queryType inQueryType)
        {
            advTree.Nodes.Clear();
            m_dataSourceGrid.Clear();

            if (pGeometry == null)
            {
                labelItemMemo.Text = "查找到0个要素";
                return;
            }
            if (listLays.Count == 0)
            {
                labelItemMemo.Text = "查找到0个要素";
                return;
            }

            progressBarItem.Visible = true;
            this.Refresh();
            progressBarItem.Maximum = listLays.Count;
            progressBarItem.Minimum = 0;
            progressBarItem.Value = 0;

            int intCnt = 0;
            bool bFirst = true;
            //循环图层查找要素
            foreach (ILayer pLay in listLays)
            {
                progressBarItem.Value = progressBarItem.Value + 1;

                IFeatureLayer pFeatLay = pLay as IFeatureLayer;
                int intKeyFieldIndex = GetKeyFieldIndexOfLayer(pFeatLay);
                if (intKeyFieldIndex == -1)
                    intKeyFieldIndex = 0;
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pGeometry;
                pSpatialFilter.GeometryField = "SHAPE";
                switch (inQueryType)
                {
                    case queryType.Cross:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                        break;
                    case queryType.Within:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;
                        break;
                    case queryType.Contain:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                        break;
                    case queryType.Intersect :
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;
                    default:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                        break;
                }
                
                if (pFeatLay.FeatureClass.FeatureCount(pSpatialFilter) == 0) continue;

                //添加图层节点
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Text = pLay.Name;
                node.Tag = pLay;
                node.DataKey = pFeatLay.FeatureClass.FeatureCount(pSpatialFilter);//yjl20110818 add for save number
                node.Expand();
                int nodei = advTree.Nodes.Add(node);
                int fcount = 0;
                bool isGoOn = false;//yjl20110817
                //int nameIdx = pFeatLay.FeatureClass.FindField("name");//yjl20110819 add
                //int clasIdx = pFeatLay.FeatureClass.FindField("class");
                IFeatureCursor pFeatureCursor = pFeatLay.Search(pSpatialFilter, false);
                IFeature pFeat = pFeatureCursor.NextFeature();
                while (pFeat != null)
                {
                    //yjl0817数量达到100标记
                    if (fcount >= queryThreshold)
                    {
                        advTree.Nodes[nodei].DataKeyString = "T";
                        break;
                    }
                    //添加要素节点
                    DevComponents.AdvTree.Node featnode = new DevComponents.AdvTree.Node();
                    //featnode.Text = pFeat.OID.ToString();
                    featnode.Text = pFeat.get_Value(intKeyFieldIndex).ToString();
                    //if (nameIdx > -1)
                    //    featnode.Text = pFeat.get_Value(nameIdx).ToString();
                    //else if (nameIdx == -1 && clasIdx > -1)
                    //    featnode.Text = pFeat.get_Value(clasIdx).ToString();//yjl20110819 add for list not OID
                    featnode.Tag = pFeat;
                    node.Nodes.Add(featnode);

                    if (bFirst == true)
                    {
                        advTree.SelectedNode = featnode;
                        m_dataSourceGrid.Clear();

                        for (int i = 0; i < pFeat.Fields.FieldCount; i++)
                        {
                            //added by chulili 20110731 取字段的中文名
                            string strFieldname = pFeat.Fields.get_Field(i).Name;
                            IField tmpField = pFeat.Fields.get_Field(i);//yjl20110817过滤OID
                            //added by chulili 20110801 过滤Shape相关字段
                            if (strFieldname.ToUpper().Contains("SHAPE") || strFieldname.ToUpper().Equals("T") || tmpField.Type == esriFieldType.esriFieldTypeOID || tmpField.Type == esriFieldType.esriFieldTypeGeometry)
                            {
                                continue;
                            }
                            //排除隐藏字段（与上面过滤字段的代码有重复，暂时两者都用）
                            if (SysCommon.ModField._ListHideFields.Contains(strFieldname))
                            {
                                continue;
                            }

                            //end added by chulili
                            string strChineseName = SysCommon.ModField.GetChineseNameOfField(strFieldname);
                            //end added by chulili 
                            if (pFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                            {
                                string strGeometryType = "";
                                if (pFeat.FeatureType == esriFeatureType.esriFTSimple)
                                {
                                    switch (pFeat.Shape.GeometryType)
                                    {
                                        case esriGeometryType.esriGeometryPoint:
                                            strGeometryType = "点";
                                            break;
                                        case esriGeometryType.esriGeometryPolyline:
                                            strGeometryType = "线";
                                            break;
                                        case esriGeometryType.esriGeometryPolygon:
                                            strGeometryType = "多边形";
                                            break;
                                    }
                                }
                                else if (pFeat.FeatureType == esriFeatureType.esriFTAnnotation)
                                {
                                    strGeometryType = "注记";
                                }
                                if (string.IsNullOrEmpty(strGeometryType))
                                {
                                    strGeometryType = pFeat.Shape.GeometryType.ToString();
                                }
                                m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGeometryType });//added by chulili 20110731
                            }
                            else
                            {
                                string strFieldValue = pFeat.get_Value(i).ToString();
                                string strFieldValueChinese = strFieldValue;
                                //if (SysCommon.ModField._DicMatchFieldValue.Keys.Contains(strFieldname))
                                //{
                                //    strFieldValueChinese = SysCommon.ModField.GetChineseOfFieldValue(strFieldname, strFieldValue);
                                //}
                                try
                                {
                                    strFieldValueChinese = SysCommon.ModField.GetDomainValueOfFieldValue(pFeatLay.FeatureClass, strFieldname, strFieldValue);
                                }
                                catch
                                { }
                                m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strFieldValueChinese });
                                //switch (strFieldname)//changed by chulili 20110810
                                //{
                                //    case "GB":
                                //        string strFieldValue = pFeat.get_Value(i).ToString();
                                //        string strGBValue = ModQuery.GetChineseNameOfGB(strFieldValue);
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGBValue });
                                //        break;
                                //    case "CLASS"://地名分类中英文映射
                                //        strFieldValue = pFeat.get_Value(i).ToString();
                                //        string strClassValue = ModQuery.GetChineseNameOfClass(strFieldValue);
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strClassValue });
                                //        break;
                                //    case "GNID":
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeat.get_Value(i) });//changed by chulili 20110731
                                //        break;
                                //    default:
                                //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeat.get_Value(i) });//changed by chulili 20110731
                                //        break;
                                //}
                            }
                        }
                        bFirst = false;
                    }
                    fcount++;
                    pFeat = pFeatureCursor.NextFeature();

                    intCnt++;
                }

                if (m_enumQueryMode == enumQueryMode.Top)
                {
                    progressBarItem.Value = listLays.Count;
                    break;
                }
            }

            labelItemMemo.Text = "查找到" + intCnt.ToString() + "个要素";
            progressBarItem.Visible = false;
            this.Refresh();

            DefaultSelNde();
        }

        
        private void DefaultSelNde()
        {
            //默认选择第一个要素节点  并进行闪烁
            //Application.DoEvents();

            //for (int i = 0; i < this.advTree.Nodes.Count; i++)
            //{
            //    if (!this.advTree.Nodes[i].HasChildNodes) continue;

            //    IFeature pFea = this.advTree.Nodes[i].Nodes[0].Tag as IFeature;
            //    if (pFea != null)
            //    {
            //        this.advTree.Nodes[i].IsSelectionVisible = true;
            //        ModDBOperator.FlashFeature(pFea.Shape, m_pMapControl.ActiveView,100);
            //        break;
            //    }
            //}

            //所有的要素一起闪烁，包括点线面 changed by xisheng 2011.07.01
            Application.DoEvents();
            IArray geoArray = new ArrayClass();
            for (int i = 0; i < this.advTree.Nodes.Count; i++)
            {
                if (!this.advTree.Nodes[i].HasChildNodes) continue;
                for (int j = 0; j < this.advTree.Nodes[i].Nodes.Count; j++)
                {
                    IFeature pFeature = this.advTree.Nodes[i].Nodes[j].Tag as IFeature;
                    geoArray.Add(pFeature);
                }
            }
            if (geoArray == null)
                return;
            HookHelperClass hookHelper = new HookHelperClass();
            hookHelper.Hook = m_pMapControl.Object;
            IHookActions hookAction = (IHookActions)hookHelper;
            hookAction.DoActionOnMultiple(geoArray, esriHookActions.esriHookActionsFlash);
            
        }
        ///// <summary>褚丽丽添加函数 20110802
        ///// 根据条件查找数据填充窗体，用来进行SQL查询,专门用于地名查询,河流,道路查询
        ///// </summary>
        ///// <param name="pMap">被查询的地图对象</param>
        //public void FillData(IFeatureClass pFeatureClass, IQueryFilter pQueryFilter,bool isHighLight)
        //{
        //    advTree.Nodes.Clear();
        //    m_dataSourceGrid.Clear();

        //    //获取查询的图层
        //    m_pQueryFilter = pQueryFilter;
        //    progressBarItem.Visible = false;
        //    this.Refresh();
        //    int intCnt = 0;//记录要素的个数
        //    bool bFirst = true;
        //    IEnvelope pEnvelop = null;//xisheng 20110804 添加所查询要素的最小矩形
        //    //added by chulili 20110801 添加错误保护
        //    if (pFeatureClass == null)
        //        return;

        //    //添加图层节点
        //    DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
        //    node.Text = pFeatureClass.AliasName;
        //    node.Tag = pFeatureClass;
        //    node.DataKey = pFeatureClass.FeatureCount(pQueryFilter);//yjl20110818 add for save number
        //    node.Expand();
        //    int nodei = advTree.Nodes.Add(node);
        //    IFeatureCursor pFeaCursor = pFeatureClass.Search(pQueryFilter, false);
        //    if (pFeaCursor == null)
        //    {
        //        labelItemMemo.Text = "查找到0个要素";
        //        progressBarItem.Visible = false;
        //        this.Refresh();
        //        return;
        //    }
        //    ////查询结果分组字段
        //    //string strGroupbyField = "";
        //    //IDataStatistics pdata = null;
        //    //IEnumerator pEnumVar = null;
        //    //if (pFeature.Fields.FindField("GB") > 0)
        //    //{
        //    //    strGroupbyField = "GB";
        //    //}
        //    //else if (pFeature.Fields.FindField("CLASS") > 0)
        //    //{
        //    //    strGroupbyField = "CLASS";
        //    //}
        //    //if (!strGroupbyField.Equals(""))
        //    //{
        //    //    //coClass对象实例生成
        //    //    pdata = new DataStatisticsClass();
        //    //    pdata.Field = strGroupbyField;
        //    //    pdata.Cursor = pFeaCursor as ICursor;
        //    //    //枚举唯一值
        //    //    pEnumVar = pdata.UniqueValues;
        //    //    pEnumVar.Reset();
        //    //    while (pEnumVar.MoveNext())
        //    //    {
        //    //        //添加分组节点
        //    //        DevComponents.AdvTree.Node Gruopnode = new DevComponents.AdvTree.Node();
        //    //        Gruopnode.Text = pEnumVar.ToString();
        //    //        Gruopnode.Tag = pEnumVar.ToString();
        //    //        node.Nodes.Add(Gruopnode);
        //    //    } 
        //    //}
        //    int nameIdx = pFeatureClass.FindField("name");//yjl20110819 add
        //    int clasIdx = pFeatureClass.FindField("class");
        //    //循环取得选中Feature的ID来得到Feature
        //    IFeature pFeature;
        //    pFeature = pFeaCursor.NextFeature();
        //    List<IGeometry> pListGeometry = new List<IGeometry>();
        //    pEnvelop = pFeature.Shape.Envelope;//xisheng 20110804
        //    while (pFeature!=null)
        //    {
        //        //yjl0817数量达到100标记
        //        if (intCnt >= queryThreshold)
        //        {
        //            advTree.Nodes[nodei].DataKeyString = "T";
        //            break;
        //        }

        //        //添加要素节点
        //        pListGeometry.Add(pFeature.Shape);
        //        pEnvelop.Union(pFeature.Shape.Envelope);//xisheng 20110804
        //        DevComponents.AdvTree.Node featnode = new DevComponents.AdvTree.Node();
        //        featnode.Text = pFeature.OID.ToString();
        //        if (nameIdx > -1)
        //            featnode.Text = pFeature.get_Value(nameIdx).ToString();
        //        else if (nameIdx == -1 && clasIdx > -1)
        //            featnode.Text = pFeature.get_Value(clasIdx).ToString();//yjl20110819 add for list not OID
        //        featnode.Tag = pFeature;
        //        node.Nodes.Add(featnode);

        //        if (bFirst == true)
        //        {
        //            advTree.SelectedNode = featnode;
        //            m_dataSourceGrid.Clear();

        //            for (int i = 0; i < pFeature.Fields.FieldCount; i++)
        //            {
        //                //added by chulili 20110731 取字段的中文名
        //                string strFieldname = pFeature.Fields.get_Field(i).Name;
        //                IField tmpField = pFeature.Fields.get_Field(i);//yjl20110817过滤OID
        //                //added by chulili 20110801 过滤Shape相关字段
        //                if (strFieldname.ToUpper().Contains("SHAPE") || strFieldname.ToUpper().Equals("T")||tmpField.Type==esriFieldType.esriFieldTypeOID||tmpField.Type==esriFieldType.esriFieldTypeGeometry)
        //                {
        //                    continue;
        //                }
        //                //排除隐藏字段（与上面过滤字段的代码有重复，暂时两者都用）
        //                if (ModQuery._ListHideFields.Contains(strFieldname))
        //                {
        //                    continue;
        //                }
        //                //end added by chulili
        //                string strChineseName = ModQuery.GetChineseNameOfField(strFieldname);
        //                //end added by chulili 
        //                if (pFeature.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
        //                {
        //                    string strGeometryType = "";
        //                    if (pFeature.FeatureType == esriFeatureType.esriFTSimple)
        //                    {
        //                        switch (pFeature.Shape.GeometryType)
        //                        {
        //                            case esriGeometryType.esriGeometryPoint:
        //                                strGeometryType = "点";
        //                                break;
        //                            case esriGeometryType.esriGeometryPolyline:
        //                                strGeometryType = "线";
        //                                break;
        //                            case esriGeometryType.esriGeometryPolygon:
        //                                strGeometryType = "多边形";
        //                                break;
        //                        }
        //                    }
        //                    else if (pFeature.FeatureType == esriFeatureType.esriFTAnnotation)
        //                    {
        //                        strGeometryType = "注记";
        //                    }
        //                    if (string.IsNullOrEmpty(strGeometryType))
        //                    {
        //                        strGeometryType = pFeature.Shape.GeometryType.ToString();
        //                    }
        //                    m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGeometryType });//added by chulili 20110731
        //                }
        //                else
        //                {
        //                    string strFieldValue = pFeature.get_Value(i).ToString();
        //                    string strFieldValueChinese = strFieldValue;
        //                    if (ModQuery._DicMatchFieldValue.Keys.Contains(strFieldname))
        //                    {
        //                        strFieldValueChinese = ModQuery.GetChineseOfFieldValue(strFieldname, strFieldValue);
        //                    }
        //                    m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strFieldValueChinese });
        //                    //switch (strFieldname)//changed by chulili 20110810
        //                    //{

        //                    //    case "GB":
        //                    //        string strFieldValue = pFeature.get_Value(i).ToString();
        //                    //        string strGBValue = ModQuery.GetChineseNameOfGB(strFieldValue);
        //                    //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGBValue });
        //                    //        break;
        //                    //    case "CLASS"://地名分类中英文映射
        //                    //        strFieldValue = pFeature.get_Value(i).ToString();
        //                    //        string strClassValue = ModQuery.GetChineseNameOfClass(strFieldValue);
        //                    //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strClassValue });
        //                    //        break;
        //                    //    case "GNID":
        //                    //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeature.get_Value(i) });//changed by chulili 20110731
        //                    //        break;
        //                    //    default:
        //                    //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeature.get_Value(i) });//changed by chulili 20110731
        //                    //        break;
        //                    //}
        //                }
        //            }
        //            bFirst = false;
        //        }
        //        pFeature = pFeaCursor.NextFeature();
        //        intCnt++;
        //    }
        //   // pEnvelop = (pFeatureClass as IGeoDataset).Extent;//xisheng 20110803
        //    labelItemMemo.Text = "查找到" + intCnt.ToString() + "个要素";
        //    progressBarItem.Visible = false;
        //    this.Refresh();
        //    IGraphicsContainer psGra = m_pMapControl.Map as IGraphicsContainer;
        //    if (isHighLight)
        //    {
        //        drawPolyLineElement(pListGeometry, psGra);
        //        pEnvelop.Expand(2, 2, true);//xisheng 20110804
        //        m_pMapControl.Extent = pEnvelop;//xisheng 20110804
        //        (m_pMapControl.Map as IActiveView).Refresh();
        //    }
        //    else
        //    {
        //        DefaultSelNde();
        //    }
        //}

        /// <summary>
        /// 根据条件查找数据填充窗体，用来进行SQL查询
        /// </summary>
        /// <param name="pMap">被查询的地图对象</param>
        public void FillData(IFeatureLayer pFeatLay, IQueryFilter pQueryFilter, esriSelectionResultEnum pSelectionResult)
        {
            advTree.Nodes.Clear();
            m_dataSourceGrid.Clear();

            //获取查询的图层
            m_pQueryFilter = pQueryFilter;
            progressBarItem.Visible = false ;
            this.Refresh();
            int intCnt = 0;//记录要素的个数
            bool bFirst = true;
            //added by chulili 20110801 添加错误保护
            if (pFeatLay == null)
                return;

            //添加图层节点
            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
            node.Text = pFeatLay.Name;
            node.Tag = pFeatLay;
            node.DataKey = pFeatLay.FeatureClass.FeatureCount(pQueryFilter);//yjl20110818 add for save number
            node.Expand();
            int intKeyFieldIndex = GetKeyFieldIndexOfLayer(pFeatLay);
            if (intKeyFieldIndex == -1)
                intKeyFieldIndex = 0;
            int nodei = advTree.Nodes.Add(node);
            //进行查询操作
            
            IFeatureSelection pFeatureSelection = pFeatLay as IFeatureSelection;
            pFeatureSelection.SelectFeatures(pQueryFilter, pSelectionResult, false);
            //保存查询的结果，包括要素类和查找到的要素
            //循环取得选中Feature的ID来得到Feature
            //int nameIdx = pFeatLay.FeatureClass.FindField("name");//yjl20110819 add
            //int clasIdx = pFeatLay.FeatureClass.FindField("class");
            IFeature pFeature;
            IEnumIDs pEnumIDs = pFeatureSelection.SelectionSet.IDs;
            int iIDIndex = pEnumIDs.Next();
            while (iIDIndex != -1)
            {
                //yjl0817数量达到100标记
                if (intCnt >= queryThreshold)
                {
                    advTree.Nodes[nodei].DataKeyString = "T";
                    break;
                }
                pFeature = pFeatLay.FeatureClass.GetFeature(iIDIndex);
                //添加要素节点
                DevComponents.AdvTree.Node featnode = new DevComponents.AdvTree.Node();
                //featnode.Text = pFeature.OID.ToString();
                featnode.Text = pFeature.get_Value(intKeyFieldIndex).ToString();
                //if (nameIdx > -1)
                //    featnode.Text = pFeature.get_Value(nameIdx).ToString();
                //else if (nameIdx == -1 && clasIdx > -1)
                //    featnode.Text = pFeature.get_Value(clasIdx).ToString();//yjl20110819 add for list not OID
                featnode.Tag = pFeature;
                node.Nodes.Add(featnode);

                if (bFirst == true)
                {
                    advTree.SelectedNode = featnode;
                    m_dataSourceGrid.Clear();

                    for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                    {
                        //added by chulili 20110731 取字段的中文名
                        string strFieldname = pFeature.Fields.get_Field(i).Name;
                        IField tmpField = pFeature.Fields.get_Field(i);//yjl20110817过滤OID
                        //added by chulili 20110801 过滤Shape相关字段
                        if (strFieldname.ToUpper().Contains("SHAPE") || strFieldname.ToUpper().Equals("T") || tmpField.Type == esriFieldType.esriFieldTypeOID || tmpField.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            continue;
                        }
                        //排除隐藏字段（与上面过滤字段的代码有重复，暂时两者都用）
                        if (SysCommon.ModField._ListHideFields.Contains(strFieldname))
                        {
                            continue;
                        }
                        //end added by chulili
                        string strChineseName = SysCommon.ModField.GetChineseNameOfField(strFieldname);
                        //end added by chulili 
                        if (pFeature.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            string strGeometryType = "";
                            if (pFeature.FeatureType == esriFeatureType.esriFTSimple)
                            {
                                switch (pFeature.Shape.GeometryType)
                                {
                                    case esriGeometryType.esriGeometryPoint:
                                        strGeometryType = "点";
                                        break;
                                    case esriGeometryType.esriGeometryPolyline:
                                        strGeometryType = "线";
                                        break;
                                    case esriGeometryType.esriGeometryPolygon:
                                        strGeometryType = "多边形";
                                        break;
                                }
                            }
                            else if (pFeature.FeatureType == esriFeatureType.esriFTAnnotation)
                            {
                                strGeometryType = "注记";
                            }
                            if (string.IsNullOrEmpty(strGeometryType))
                            {
                                strGeometryType = pFeature.Shape.GeometryType.ToString();
                            }
                            m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGeometryType });//added by chulili 20110731
                        }
                        else
                        {
                            string strFieldValue = pFeature.get_Value(i).ToString();
                            string strFieldValueChinese = strFieldValue;
                            //if (SysCommon.ModField._DicMatchFieldValue.Keys.Contains(strFieldname))
                            //{
                            //    strFieldValueChinese = SysCommon.ModField.GetChineseOfFieldValue(strFieldname, strFieldValue);
                            //}
                            try
                            {
                                strFieldValueChinese = SysCommon.ModField.GetDomainValueOfFieldValue(pFeatLay.FeatureClass, strFieldname, strFieldValue);
                            }
                            catch
                            { }
                            m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strFieldValueChinese });

                            //switch (strFieldname)//changed by chulili 20110810
                            //{
                            //    case "GB":
                            //        string strFieldValue = pFeature.get_Value(i).ToString();
                            //        string strGBValue = ModQuery.GetChineseNameOfGB(strFieldValue);
                            //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGBValue });
                            //        break;
                            //    case "CLASS"://地名分类中英文映射
                            //        strFieldValue = pFeature.get_Value(i).ToString();
                            //        string strClassValue = ModQuery.GetChineseNameOfClass(strFieldValue);
                            //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strClassValue });
                            //        break;
                            //    case "GNID":
                            //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeature.get_Value(i) });//changed by chulili 20110731
                            //        break;
                            //    default:
                            //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pFeature.get_Value(i) });//changed by chulili 20110731
                            //        break;
                            //}
                            
                        }
                    }
                    bFirst = false;
                }
                iIDIndex = pEnumIDs.Next();
                intCnt++;
            }

            labelItemMemo.Text = "查找到" + intCnt.ToString() + "个要素";
            progressBarItem.Visible = false;
            this.Refresh();

            DefaultSelNde();
        }

        private void advTree_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            try
            {
               
                IFeature pfeature = advTree.SelectedNode.Tag as IFeature;
                if (pfeature != null)
                {
                    m_dataSourceGrid.Clear();

                    for (int i = 0; i < pfeature.Fields.FieldCount; i++)
                    {
                        //added by chulili 20110731 取字段的中文名
                        string strFieldname = pfeature.Fields.get_Field(i).Name;
                        IField tmpField = pfeature.Fields.get_Field(i);//yjl20110817过滤OID
                        //added by chulili 20110801 过滤Shape相关字段
                        if (strFieldname.ToUpper().Contains("SHAPE") || strFieldname.ToUpper().Equals("T") || tmpField.Type == esriFieldType.esriFieldTypeOID || tmpField.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            continue;
                        }
                        //排除隐藏字段（与上面过滤字段的代码有重复，暂时两者都用）
                        if (SysCommon.ModField._ListHideFields != null)
                        {
                            if (SysCommon.ModField._ListHideFields.Contains(strFieldname))
                            {
                                continue;
                            }
                        }
                        //end added by chulili 
                        string strChineseName = SysCommon.ModField.GetChineseNameOfField(strFieldname);
                        //end added by chulili 
                        if (pfeature.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            string strGeometryType = "";
                            if (pfeature.FeatureType == esriFeatureType.esriFTSimple)
                            {
                                switch (pfeature.Shape.GeometryType)
                                {
                                    case esriGeometryType.esriGeometryPoint:
                                        strGeometryType = "点";
                                        break;
                                    case esriGeometryType.esriGeometryPolyline:
                                        strGeometryType = "线";
                                        break;
                                    case esriGeometryType.esriGeometryPolygon:
                                        strGeometryType = "多边形";
                                        break;
                                }
                            }
                            else if (pfeature.FeatureType == esriFeatureType.esriFTAnnotation)
                            {
                                strGeometryType = "注记";
                            }
                            if (string.IsNullOrEmpty(strGeometryType))
                            {
                                strGeometryType = pfeature.Shape.GeometryType.ToString();
                            }
                            m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGeometryType });//changed by chulili 20110731
                        }
                        else
                        {
                            string strFieldValue = pfeature.get_Value(i).ToString();
                            string strFieldValueChinese = strFieldValue;
                            //if (SysCommon.ModField._DicMatchFieldValue.Keys.Contains(strFieldname))
                            //{
                            //    strFieldValueChinese = SysCommon.ModField.GetChineseOfFieldValue(strFieldname, strFieldValue);
                            //}
                            try
                            {
                                IObjectClass pObjectClass =pfeature.Class;
                                IFeatureClass pFeatureClass = pObjectClass as IFeatureClass;
                                //strFieldValueChinese = SysCommon.ModField.GetDomainValueOfFieldValue(pFeatureClass, strFieldname, strFieldValue);
                                strFieldValueChinese = SysCommon.ModXZQ.GetChineseName(Plugin.ModuleCommon.TmpWorkSpace, strFieldname, strFieldValue);
                            }
                            catch
                            { }
                            m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strFieldValueChinese });

                            //switch (strFieldname)//changed by chulili 20110810
                            //{
                                //case "GB"://国标中英文映射
                                //    string strFieldValue = pfeature.get_Value(i).ToString();
                                //    string strGBValue = ModQuery.GetChineseNameOfGB(strFieldValue);
                                //    m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strGBValue });
                                //    break;
                                //case "CLASS"://地名分类中英文映射
                                //    strFieldValue = pfeature.get_Value(i).ToString();
                                //    string strClassValue = ModQuery.GetChineseNameOfClass(strFieldValue);
                                //    m_dataSourceGrid.Rows.Add(new object[] { strChineseName, strClassValue });
                                //    break;
                                //case "GNID"://行政区中英文映射
                                //    m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pfeature.get_Value(i) });
                                //    break;
                            //    default:
                            //        m_dataSourceGrid.Rows.Add(new object[] { strChineseName, pfeature.get_Value(i) });
                            //        break;
                            //}
                        }
                    }
					//yjl20110729,add,单击高亮显示要素但不缩放范围

                    ///ZQ 201118 modify
                    try
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            m_pMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
                            m_DrawGeometry = pfeature.Shape;
                            ////yjladded 为了高亮显示查询的要素
                            //IGraphicsContainer pGra = m_pMapControl.Map as IGraphicsContainer;
                            //pGra.Reset();
                            //IElement pEle = pGra.Next();
                            //(pGra as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);//更改时需2次
                            //while (pEle != null)
                            //{
                            //    if ((pEle as IElementProperties).Name == _QueryTag)
                            //    {
                            //        pGra.DeleteElement(pEle);
                            //        pGra.Reset();
                            //    }

                            //    pEle = pGra.Next();
                            //}
                            //Application.DoEvents();
                            //(pGra as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                            ////yjladded 为了高亮显示查询的要素
                            //if (pfeature != null)
                            //{
                            //    Application.DoEvents();
                            //    ModDBOperator._FlashTagName = _QueryTag;
                            //ModDBOperator.FlashFeature(pfeature, m_pMapControl);
                            //    //m_pMapControl.FlashShape(pfeature.Shape, 2, 500, null);
                            //}

                            m_pMapControl.FlashShape(m_DrawGeometry, 3, 200, null);
                            drawgeometryXOR(m_DrawGeometry);
                          
                        }
                        //else if (e.Button == MouseButtons.Right)//yjl20110817 modify 从双击变成右击
                        //{
                        //    //yjladded 为了高亮显示查询的要素且缩放范围
                        //    IGraphicsContainer pGra = m_pMapControl.Map as IGraphicsContainer;
                        //    pGra.Reset();
                        //    IElement pEle = pGra.Next();
                        //    //yjl0729,add,局部刷新未起作用
                        //    //(pGra as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);//更改时需2次
                        //    while (pEle != null)
                        //    {
                        //        if ((pEle as IElementProperties).Name == _QueryTag)
                        //        {
                        //            pGra.DeleteElement(pEle);
                        //            pGra.Reset();
                        //        }

                        //        pEle = pGra.Next();
                        //    }
                        //    (pGra as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                        //    //yjladded 为了高亮显示查询的要素
                        //    //IFeature pfeature = advTree.SelectedNode.Tag as IFeature;
                        //    if (pfeature != null)
                        //    {
                        //        SysCommon.Gis.ModGisPub.ZoomToFeature(m_pMapControl, pfeature);
                        //        Application.DoEvents();
                        //        ModDBOperator._FlashTagName = _QueryTag;
                        //        ModDBOperator.FlashFeature(pfeature, m_pMapControl);
                        //        //m_pMapControl.FlashShape(pfeature.Shape, 2, 500, null);
                        //    }
 
                        //}
                    }
                    catch
                    {
                    }
					//yjl20110729,add,单击高亮显示要素但不缩放范围    end
                }
            }
            catch
            {

            }
        }
        private void advTree_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            //yjl20110729,add,单击高亮显示要素但不缩放范围
            try
            {
                IFeature pfeature = advTree.SelectedNode.Tag as IFeature;
                if (e.Button == MouseButtons.Left)
                {
                    SysCommon.Gis.ModGisPub.ZoomToFeature(m_pMapControl, pfeature);
                    m_DrawGeometry = pfeature.Shape;
                    m_pMapControl.ActiveView.ScreenDisplay.UpdateWindow();
                    m_pMapControl.FlashShape(m_DrawGeometry, 3, 200, null);
                    drawgeometryXOR(m_DrawGeometry);

                }
            }
            catch
            {

            }
        }





        /// <summary>
        /// 设置RGB函数  
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <returns></returns>
        public static IRgbColor getRGB(int r, int g, int b)
        {
            IRgbColor pRgbColor = new RgbColorClass();
            pRgbColor.Red = r;
            pRgbColor.Green = g;
            pRgbColor.Blue = b;
            return pRgbColor;
        }
        /// <summary>
        /// 绘制pGeometry的图形
        /// </summary>

        private void drawgeometryXOR(IGeometry pGeometry)
        {
            if (pGeometry == null)//如果窗体关闭或者取消 就不绘制 xisheng 2011.06.28
            {
                return;
            }
            IScreenDisplay pScreenDisplay = m_pMapControl.ActiveView.ScreenDisplay;
            ISymbol pSymbol = null;
            //颜色对象
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.UseWindowsDithering = false;
            pRGBColor = getRGB(255, 0, 0);
            pRGBColor.Transparency = 255;
            try
            {
                switch (pGeometry.GeometryType.ToString())
                {
                    case "esriGeometryPoint"://点要素
                        ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass();
                        pMarkerSymbol.Size = 7.0;
                        pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                        pMarkerSymbol.Color = pRGBColor;
                        pSymbol = (ISymbol)pMarkerSymbol;
                        pSymbol.ROP2 = esriRasterOpCode.esriROPCopyPen;
                        break;
                    case "esriGeometryPolyline"://线要素
                        ISimpleLineSymbol pPolyLineSymbol = new SimpleLineSymbolClass();
                        pPolyLineSymbol.Color = pRGBColor;
                        pPolyLineSymbol.Width = 2.5;
                        pPolyLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                        pSymbol = (ISymbol)pPolyLineSymbol;
                        ///ZQ  20111117 modify
                        pSymbol.ROP2 = esriRasterOpCode.esriROPCopyPen;
                        break;
                    case "esriGeometryPolygon"://面要素
                        ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
                        ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

                        pSymbol = (ISymbol)pFillSymbol;
                        pSymbol.ROP2 = esriRasterOpCode.esriROPCopyPen;
                        /// end
                        pLineSymbol.Color = pRGBColor;
                        pLineSymbol.Width = 1.5;
                        pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                        pFillSymbol.Outline = pLineSymbol;

                        pFillSymbol.Color = pRGBColor;
                        pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                        break;
                }


                pScreenDisplay.StartDrawing(pScreenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);  //esriScreenCache.esriNoScreenCache -1
                pScreenDisplay.SetSymbol(pSymbol);
                switch (pGeometry.GeometryType.ToString())
                {
                    case "esriGeometryPoint"://点要素
                        pScreenDisplay.DrawPoint(pGeometry);
                        break;
                    case "esriGeometryPolyline"://线要素
                        pScreenDisplay.DrawPolyline(pGeometry);
                        break;
                    case "esriGeometryPolygon"://面要素
                        pScreenDisplay.DrawPolygon(pGeometry);
                        break;
                }
                pScreenDisplay.FinishDrawing();

            }
            catch
            { }
            finally
            {
                pSymbol = null;
                pRGBColor = null;
            }
        }

        //查询窗体关闭
        private void frmQuery_FormClosed(object sender, FormClosedEventArgs e)
        {

            m_DrawGeometry = null;
            if (m_ActiveView != null)
            {
                m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null); //changed by xisheng 20110803
            }
            //if (m_pActiveViewEvents != null)
            //{
            //    m_pActiveViewEvents.AfterDraw += new IActiveViewEvents_AfterDrawEventHandler(m_pActiveViewEvents_AfterDraw);
            //}
            this.Dispose(true);
            SysCommon.ScreenDraw.list.Remove(AfterDraw);
            this.Dispose(true);
 
            //m_pMapControl.CurrentTool = null; 
            //yjladded 为了高亮显示查询的要素
            //IGraphicsContainer pGra = m_pMapControl.Map as IGraphicsContainer;
            //pGra.Reset();
            //IElement pEle = pGra.Next();
            
            //while (pEle != null)
            //{
            //    if ((pEle as IElementProperties).Name == _QueryTag || (pEle as IElementProperties).Name == "RoadQuery")
            //    {
            //        pGra.DeleteElement(pEle);
            //        pGra.Reset();
            //    }

            //    pEle = pGra.Next();
            //}
         
            //20110731 xisheng
            //(pGra as IActiveView).Refresh();
            //yjladded 为了高亮显示查询的要素
            ///end
            
        }
        //导出查询结果 褚丽丽添加 20110706
        #region 导出查询结果
        private void buttonItemExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog pDlg = new SaveFileDialog();
            pDlg.Filter = "Excel WorkBook (*.xls)|*.xls";
            if (pDlg.ShowDialog() == DialogResult.Cancel)
                return;
            SysCommon.CProgress vProgress = new SysCommon.CProgress("导出查询结果");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            string strFileName = pDlg.FileName;
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = null;
                Microsoft.Office.Interop.Excel.Workbook wb = null;
                //建立Excel对象
                excel = new Microsoft.Office.Interop.Excel.Application();
                wb = excel.Application.Workbooks.Add(true);
                excel.Visible = false;
                wb.Application.ActiveWindow.Caption = strFileName;
                int iRow = 1;
                int layerCount = advTree.Nodes.Count;
                for (int i = 0; i < layerCount; i++)
                {
                    DevComponents.AdvTree.Node pNode = advTree.Nodes[i];
                    IFeatureLayer pFeaLayer = pNode.Tag as IFeatureLayer;
                    if (pFeaLayer == null) continue;
                    vProgress.SetProgress("导出图层:" + pFeaLayer.Name);
                    IFeatureClass pFeaCls = pFeaLayer.FeatureClass;
                    #region 获取图层几何类型
                    string strGeometryType = "";
                    if (pFeaCls.FeatureType == esriFeatureType.esriFTSimple)
                    {
                        switch (pFeaCls.ShapeType)
                        {
                            case esriGeometryType.esriGeometryPoint:
                                strGeometryType = "点";
                                break;
                            case esriGeometryType.esriGeometryPolyline:
                                strGeometryType = "线";
                                break;
                            case esriGeometryType.esriGeometryPolygon:
                                strGeometryType = "多边形";
                                break;
                        }
                    }
                    else if (pFeaCls.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        strGeometryType = "注记";
                    }
                    if (string.IsNullOrEmpty(strGeometryType))
                    {
                        strGeometryType = pFeaCls.ShapeType.ToString();
                    }
                    #endregion
                    //WriteLayerStruToFlexcell(pGrid, pFeaCls, pFeaLayer as ILayer);
                    WriteLayerStruToExcel(excel, pFeaCls, pFeaLayer as ILayer,iRow);
                    iRow = iRow + 2;
                    int feacount = pNode.Nodes.Count;
                    for (int j = 0; j < feacount; j++)
                    {
                        DevComponents.AdvTree.Node pFeaNode = pNode.Nodes[j];
                        IFeature pFea = pFeaNode.Tag as IFeature;
                        if (pFea == null) continue;
                        WriteFeaToExcel(excel, pFea, iRow, strGeometryType);
                        iRow = iRow + 1;
                    }
                    iRow = iRow + 1;
                    //pGrid.Rows = pGrid.Rows + 1;
                }
                //pGrid.AutoRedraw = true;
                //pGrid.ExportToExcel(strFileName);
                //pFrm = null;
                ///导出查询结果暂不关闭查询结果  ZQ 20111116 modify
                ///
                try
                {
                    wb.SaveAs(strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                catch (System.Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    //SysCommon.ModExcel.Kill(excel);
                    GC.Collect();
                    this.Close();
                    vProgress.Close();
                    return;
                }
                excel.Workbooks.Close();
                excel.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                //SysCommon.ModExcel.Kill(excel);
                GC.Collect();

                this.Close();

                vProgress.SetProgress("打开导出的Excel文件");
                try
                {
                    System.Diagnostics.Process.Start(strFileName);
                }
                catch (System.Exception ex)
                {

                }
                //OpenExcelFile(strFileName);
                vProgress.Close();
            }
            catch
            {
                vProgress.Close();
            }

        }
        //将图层信息写到导出的flexcell中
        private void WriteLayerStruToFlexcell(AxFlexCell.AxGrid pGrid, IFeatureClass pFeaCls, ILayer pLayer, int iRow)
        {
            if (pGrid == null)
                return;
            if (pFeaCls == null)
                return;
            if (iRow == 0)
                iRow = pGrid.Rows;
            if (pGrid.Rows <= iRow)
                pGrid.Rows = iRow + 2;
            if (pGrid.Cols <= pFeaCls.Fields.FieldCount)
                pGrid.Cols = pFeaCls.Fields.FieldCount + 1;
            if (pGrid.Cols < 4)
                pGrid.Cols = 4;
            //写图层名
            pGrid.Range(iRow, 1, iRow, 3).Merge();
            pGrid.Cell(iRow, 1).Text = "图层：" + pLayer.Name;
            //写图层属性列
            for (int i = 0; i < pFeaCls.Fields.FieldCount; i++)
            {
                pGrid.Cell(iRow + 1, i + 1).Text = pFeaCls.Fields.get_Field(i).AliasName;
            }
        }
        private void WriteLayerStruToExcel(Microsoft.Office.Interop.Excel.Application pExcel, IFeatureClass pFeaCls, ILayer pLayer)
        {
            WriteLayerStruToExcel(pExcel, pFeaCls, pLayer, 0);
        }
        //将图层信息写到导出的flexcell中
        private void WriteLayerStruToExcel(Microsoft.Office.Interop.Excel.Application pExcel, IFeatureClass pFeaCls, ILayer pLayer, int iRow)
        {
            if (pExcel == null)
                return;
            if (pFeaCls == null)
                return;
            //写图层名
            //pGrid.Range(iRow, 1, iRow, 3).Merge();
            //pGrid.Cell(iRow, 1).Text = "图层：" + pLayer.Name;
            pExcel.get_Range(pExcel.Cells[iRow,1],pExcel.Cells[iRow,3]).Merge(false);
            pExcel.Cells[iRow, 1] = "图层：" + pLayer.Name;

            //写图层属性列
            for (int i = 0; i < pFeaCls.Fields.FieldCount; i++)
            {
                //pGrid.Cell(iRow + 1, i + 1).Text = pFeaCls.Fields.get_Field(i).AliasName;
                pExcel.Cells[iRow + 1, i + 1] = pFeaCls.Fields.get_Field(i).AliasName;
            }
        }
        //将图层信息写到导出的flexcell中
        private void WriteLayerStruToFlexcell(AxFlexCell.AxGrid pGrid, IFeatureClass pFeaCls, ILayer pLayer)
        {
            WriteLayerStruToFlexcell(pGrid, pFeaCls, pLayer, 0);
        }
        //将地物信息写到导出的flexcell中
        private void WriteFeaToFlexcell(AxFlexCell.AxGrid pGrid, IFeature pFea, int iRow, string strGeometryType)
        {
            if (pGrid == null)
                return;
            if (pFea == null)
                return;
            if (iRow == 0)
                iRow = pGrid.Rows;
            if (pGrid.Rows <= iRow)
                pGrid.Rows = iRow + 1;
            if (pGrid.Cols <= pFea.Fields.FieldCount)
                pGrid.Cols = pFea.Fields.FieldCount + 1;
            //写属性值
            for (int i = 0; i < pFea.Fields.FieldCount; i++)
            {   //Geometry字段单独处理
                if (pFea.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    pGrid.Cell(iRow, i + 1).Text = strGeometryType;
                }
                else
                {
                    pGrid.Cell(iRow, i + 1).Text = pFea.get_Value(i).ToString();
                }
            }
        }

        private void WriteFeaToExcel(Microsoft.Office.Interop.Excel.Application pExcel, IFeature pFea, string strGeometryType)
        {
            WriteFeaToExcel( pExcel,  pFea, 0,  strGeometryType);
        }
        //将地物信息写到导出的excel中
        private void WriteFeaToExcel(Microsoft.Office.Interop.Excel.Application pExcel, IFeature pFea, int iRow, string strGeometryType)
        {
            if (pExcel == null)
                return;
            if (pFea == null)
                return;
            //写属性值
            for (int i = 0; i < pFea.Fields.FieldCount; i++)
            {   //Geometry字段单独处理
                if (pFea.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    //pGrid.Cell(iRow, i + 1).Text = strGeometryType;
                    pExcel.Cells[iRow, i + 1] = strGeometryType;
                }
                else if (pFea.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeString)
                {
                    pExcel.Cells[iRow, i + 1] = "'"+pFea.get_Value(i).ToString();
                }
                else
                {
                    //pGrid.Cell(iRow, i + 1).Text = pFea.get_Value(i).ToString();
                    pExcel.Cells[iRow, i + 1] = pFea.get_Value(i).ToString();
                }
            }
        }
        //将地物信息写到导出的flexcell中
        private void WriteFeaToFlexcell(AxFlexCell.AxGrid pGrid, IFeature pFea, string strGeometryType)
        {
            WriteFeaToFlexcell(pGrid, pFea, 0, strGeometryType);
        }
        //打开excel文档
        private void OpenExcelFile(string filepath)
        {
            
            Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application();
            object MissingValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Workbook xBook = xApp.Workbooks._Open(filepath, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue);
            Microsoft.Office.Interop.Excel.Worksheet xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];
            xApp.Visible = true;

        }
        #endregion

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
                        pProperty.Name = "RoadQuery";
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
        //yjl20110818 add for more than 100
        private void advTree_NodeMouseDown(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            curNode = e.Node;
            if (curNode == null)
                return;
            if (curNode.Parent != null)
                curNode = curNode.Parent as DevComponents.AdvTree.Node;
            if (curNode.DataKeyString == "T")
            {
                labelItemMemo.Text = "要素数量超过"+queryThreshold.ToString()+"。";
                btnItem100.Visible = true;
                this.Refresh();
               
            }
            else
            {
                ///ZQ 20111118 modify
                labelItemMemo.Text = "当前层："+(Convert.ToInt32(curNode.Nodes.Count)).ToString()+"个要素。";
                ///end
                btnItem100.Visible = false;
 
            }
        }
        //yjl20110818 add for more than 100
        private void btnItem100_Click(object sender, EventArgs e)
        {
            btnItem100.Visible = false;
            if (curNode == null)
                return;
            curNode.DataKeyString = "";//取消标记
            if (m_Geometry != null)
            {
                if (curNode.Parent == null)
                    FillData(curNode, m_Geometry);
                else
                {
                    FillData(curNode, m_Geometry);
                }
            }
            else if (m_pQueryFilter != null)
            {
                if (curNode.Parent == null)
                    FillData(curNode, m_pQueryFilter);
                else
                {
                    FillData(curNode, m_pQueryFilter);
                }
 
            }

        }
        //从xml文件读取查询数量的阈值 yjl20110819 add 
        private int getQueryThreshold()
        {
            string cPath = Application.StartupPath + "\\..\\Res\\Xml\\QuerySet.xml";
            if (!File.Exists(cPath))
            {
                return 100;
            }
            XmlDocument cXmlDoc = new XmlDocument();
            string strCnt = "";
            if (cXmlDoc != null)
            {
                cXmlDoc.Load(cPath);

                XmlNodeList xnl = cXmlDoc.GetElementsByTagName("Query");
                strCnt = xnl.Item(0).Attributes["Threshold"].Value;
            }
            if (strCnt != "")
                return Convert.ToInt32(strCnt);
            else
                return 100;
        }

      

    }
}