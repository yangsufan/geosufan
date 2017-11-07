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

namespace GeoDataCenterFunLib
{

    public partial class frmQuerytoTable: DevComponents.DotNetBar.Office2007Form
    {
        private DataTable m_dataSourceGrid;        //grid的datasource
        private IMapControlDefault m_pMapControl;                        //被查询的地图对象
        private enumQueryMode m_enumQueryMode;     //查询方式
        private bool _IsHightLight=false;
        private IFeatureCursor _FeaCursor = null;
        private int _FeaCnt = 0;
        private DataTable _DataTableAll = null;
        private DataTable _DataTablePart = null;
        private int _CurMaxID = 0;//当前页面显示的最大id,从0开始
        private int _PageID = 0;//当前页(从1开始)
        private Dictionary<int, int> _DicFeature=new Dictionary<int,int> ();
        private string _FlashTag = "frmQueryToTable";
        private IFeatureClass _QueryFeatureClass = null;
        
        private int _OIDfieldIndex=0;

        private IQueryFilter _QueryFilterAll = null;
        private IQueryFilter _QueryFilterPart1 = null;
        private IQueryFilter _QueryFilterPart2 = null;

        private Dictionary<string, DataTable> _DicDatatable = null;
        private Dictionary<string, IFeatureClass> _DicFeatureClass = null;

        public enumQueryMode QueryMode
        {
            get
            {
                return m_enumQueryMode;
            }
        }
		public IMap m_Map { get; set; }// 20110802 xisheng 
        public IGeometry m_Geometry { get; set; }//20110802 xisheng 

        /// <summary>
        /// 初始化，用来SQL查询
        /// </summary>
        /// <param name="pMapControl"></param>
        public frmQuerytoTable(IMapControlDefault pMapControl)
        {
            InitializeComponent();
            labelItem.Visible = false;
            comboBoxFeatureLayer.Visible = false;

            //InitializeGrid();      //初始化Grid的表现  
            m_pMapControl = pMapControl;
            //added by chulili 20110731 初始化字段中英文对照字典
            if (SysCommon.ModField._DicFieldName.Keys.Count ==0)
            {
                SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName,"属性对照表");
            }
            //changed by chulili 20110818 统一进行属性值映射
            if (SysCommon.ModField._DicMatchFieldValue.Keys.Count == 0)
            {
                SysCommon.ModField.InitMatchFieldValueDic(Plugin.ModuleCommon.TmpWorkSpace);
            }
            //隐藏哪些字段
            if (SysCommon.ModField._ListHideFields == null)
            {
                SysCommon.ModField.GetHideFields();
            }
            //if (ModQuery._DicGBname.Keys.Count == 0)
            //{
            //    ModQuery.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, ModQuery._DicGBname, "国标对照表");
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
        /// 适用于包含、落入和相交查询
        /// </summary>
        /// <param name="pMapControl">被查询的地图对象</param>
        /// <param name="penumQueryMode">查询方式</param>
        public frmQuerytoTable(IMapControlDefault pMapControl, enumQueryMode penumQueryMode,bool isDiff)
        {
            InitializeComponent();
            labelItem.Visible = false;
            comboBoxFeatureLayer.Visible = false;

            //InitializeGrid();      //初始化Grid的表现  
            m_pMapControl = pMapControl;
            m_enumQueryMode = penumQueryMode;
            InitiaComboBox();
            switch(penumQueryMode)
            {
                case enumQueryMode.Top:

                    comboBoxFeatureLayer.SelectedIndex = 0;
                    break;
                case enumQueryMode.Visiable:
				case enumQueryMode.Layer:
                    comboBoxFeatureLayer.SelectedIndex = 1;
                    break;
                case enumQueryMode.Selectable:
                    comboBoxFeatureLayer.SelectedIndex = 2;
                    break;
                //case enumQueryMode.CurEdit://changed by chulili 20110707现不再有当前编辑图层选项
                //    comboBoxItem.SelectedIndex = 3;
                    //break;
                case enumQueryMode.All:
                    comboBoxFeatureLayer.SelectedIndex = 3;
                    break;
            }
            //added by chulili 20110731 初始化字段中英文对照字典
            if (SysCommon.ModField._DicFieldName.Keys.Count == 0)
            {
                SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName,"属性对照表");
            }
            //changed by chulili 20110818 统一进行属性值映射
            if (SysCommon.ModField._DicMatchFieldValue.Keys.Count == 0)
            {
                SysCommon.ModField.InitMatchFieldValueDic(Plugin.ModuleCommon.TmpWorkSpace);
            }
            //隐藏哪些字段
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
            string strNodeKey = pxmlnode.Attributes["IsQuery"].Value.ToString();
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
        public frmQuerytoTable(IMapControlDefault pMapControl, enumQueryMode penumQueryMode)
        {
            InitializeComponent();
            labelItem.Visible = true;
            comboBoxFeatureLayer.Visible = true;

            //InitializeGrid();      //初始化Grid的表现  
            m_pMapControl = pMapControl;
            m_enumQueryMode = penumQueryMode;
            InitiaComboBox();
            switch (penumQueryMode)
            {
                case enumQueryMode.Top:

                    comboBoxFeatureLayer.SelectedIndex = 0;
                    break;
                case enumQueryMode.Visiable:
				case enumQueryMode.Layer:
                    comboBoxFeatureLayer.SelectedIndex = 1;
                    break;
                case enumQueryMode.Selectable:
                    comboBoxFeatureLayer.SelectedIndex = 2;
                    break;
                //case enumQueryMode.CurEdit://changed by chulili 20110707现不再有当前编辑图层选项
                //    comboBoxItem.SelectedIndex = 3;
                //    break;
                case enumQueryMode.All:
                    comboBoxFeatureLayer.SelectedIndex = 3;
                    break;
            }
            //added by chulili 20110731 初始化字段中英文对照字典
            if (SysCommon.ModField._DicFieldName.Keys.Count == 0)
            {
                SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName,"属性对照表");
            }
            //changed by chulili 20110818 统一进行属性值映射
            if (SysCommon.ModField._DicMatchFieldValue.Keys.Count == 0)
            {
                SysCommon.ModField.InitMatchFieldValueDic(Plugin.ModuleCommon.TmpWorkSpace);
            }
            //隐藏哪些字段
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
        /// <summary>
        /// 将地图中的图层加载到列表中 added by xisheng 20110731
        /// </summary>
        private void InitiaComboBox()
        {
            for (int i = 0; i < m_pMapControl.Map.LayerCount; i++)
            {
                if (m_pMapControl.Map.get_Layer(i) is IGroupLayer)
                {
                    ICompositeLayer pLayer = m_pMapControl.Map.get_Layer(i) as ICompositeLayer;
                    for (int j = 0; j < pLayer.Count; j++)
                    {
                        ILayer pFLayer = pLayer.get_Layer(j) as ILayer;
                        if (pFLayer != null)
                        {
                            if (pFLayer is IFeatureLayer)
                            {
                                IFeatureLayer pFeatureLayer = pFLayer as IFeatureLayer;
								if (GetIsQuery(pFeatureLayer))//判断是否可查询 0802
                                {
                                    comboBoxFeatureLayer.Items.Add(pFeatureLayer.Name);
                                }
                            }
                        }
                    }
                }
                else
                {
                    ILayer pFLayer = m_pMapControl.Map.get_Layer(i) as ILayer;
                    if (pFLayer != null)
                    {
                        if (pFLayer is IFeatureLayer)
                        {
                            IFeatureLayer pFeatureLayer = pFLayer as IFeatureLayer;
                            if (GetIsQuery(pFeatureLayer))//判断是否可查询 0802
                            {
                                comboBoxFeatureLayer.Items.Add(pFeatureLayer.Name);
                            }
                        }
                    }
                }
            }
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

        
        /// <summary>褚丽丽添加函数 20110802
        /// 根据条件查找数据填充窗体，用来进行SQL查询,专门用于地名查询,河流,道路查询
        /// </summary>
        /// <param name="pMap">被查询的地图对象</param>
        public void FillData(IFeatureClass pFeatureClass, IQueryFilter pQueryFilter,bool isHighLight)
        {
            if (pFeatureClass == null)
            {
                return;
            }
            _QueryFeatureClass = pFeatureClass;

            if (_FeaCursor != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_FeaCursor);
                _FeaCursor = null;
            }
            //if (_DicFeature == null)
            //{
            //    _DicFeature = new Dictionary<int, int>();
            //}
            labelItem.Visible = true;
            comboBoxFeatureLayer.Visible = true;
            if (!comboBoxFeatureLayer.Items.Contains(pFeatureClass.AliasName))
            {
                comboBoxFeatureLayer.Items.Add(pFeatureClass.AliasName);
            }
            for (int i = 0; i < comboBoxFeatureLayer.Items.Count; i++)
            {
                if (comboBoxFeatureLayer.Items[i].ToString() == pFeatureClass.AliasName)
                {
                    comboBoxFeatureLayer.SelectedIndex = i;
                    break;
                }
            }
            this.comboBoxFeatureLayer.Enabled = false;

            //m_dataSourceGrid.Clear();

            //获取查询的图层

            progressBarItem.Visible = false;
            this.Refresh();
            bool bFirst = true;
            //added by chulili 20110801 添加错误保护
            if (pFeatureClass == null)
                return;

            //循环取得选中Feature的ID来得到Feature
            //初始化表
            _DataTablePart = InitDataTable(pFeatureClass);
            _DataTableAll = InitDataTable(pFeatureClass);
            //绑定 以便动态显示
            this.gridRes.DataSource = _DataTablePart;

            //进度条设置
            progressBarItem.Visible = true;
            this.Refresh();
            int SearchCnt = pFeatureClass.FeatureCount(pQueryFilter);
            progressBarItem.Maximum = SearchCnt + 100;
            progressBarItem.Minimum = 0;
            progressBarItem.Value = 0;

            ICursor pCursor1 = pFeatureClass.Search(pQueryFilter, false) as ICursor ;
            
            IRow pRow = pCursor1.NextRow();
            int indexFeature = 0;
            IFeature pFeature = null;
            while (pRow != null)
            {
                //进度条
                progressBarItem.Value = progressBarItem.Value + 1;

                _FeaCnt = _FeaCnt + 1;
                //_DicFeature.Add(indexFeature, pRow.OID);
                InsterFeaToTable(pRow, _DataTableAll);
                indexFeature = indexFeature + 1;
                pRow = pCursor1.NextRow();
            }
            int iIndex = 0;
            for (iIndex = 0; iIndex < 100; iIndex++)
            {
                //进度条
                progressBarItem.Value = progressBarItem.Value + 1;
                if (iIndex >= _DataTableAll.Rows.Count)
                {
                    break;
                }
                DataRow pDataRow = _DataTableAll.Rows[iIndex];
                DataRow pDataRow2 = _DataTablePart.NewRow();
                pDataRow2.ItemArray = pDataRow.ItemArray;
                _DataTablePart.Rows.Add(pDataRow2);
            }
            progressBarItem.Value = progressBarItem.Maximum;
            _CurMaxID = iIndex - 1;
            _PageID = 1;
            this.textBoxPage.ControlText = _PageID.ToString();
            //下一页状态
            if (_FeaCnt > _CurMaxID + 1)
            {
                this.buttonNextPage.Enabled = true;
            }
            else
            {
                this.buttonNextPage.Enabled = false;
            }
            //上一页状态
            if (_CurMaxID > 100)
            {
                this.buttonLastPage.Enabled = true;
            }
            else
            {
                this.buttonLastPage.Enabled = false;
            }

            labelItemMemo.Text = "查找到" + _FeaCnt.ToString() + "个要素";
            progressBarItem.Value = 0;
            progressBarItem.Enabled = false;
            //处理隐藏字段
            for (int i = 0; i < gridRes.Columns.Count; i++)
            {
                DataGridViewColumn pColumn = gridRes.Columns[i];
                pColumn.HeaderText = _DataTablePart.Columns[i].ColumnName;
                pColumn.Name = _DataTablePart.Columns[i].Caption;
                string strFieldname = pColumn.Name;
                if (SysCommon.ModField._ListHideFields.Contains(pColumn.Name))
                {
                    pColumn.Visible = false;
                }
            }
            this.Refresh();
            IGraphicsContainer psGra = m_pMapControl.Map as IGraphicsContainer;
            if (isHighLight)
            {
                //drawPolyLineElement(pListGeometry, psGra);
                (m_pMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
            }
        }
        /// <summary>褚丽丽添加函数 20110807
        /// 传入多个featureclass
        /// 根据条件查找数据填充窗体，用来进行SQL查询,专门用于地名查询,河流,道路查询
        /// </summary>
        /// <param name="pMap">被查询的地图对象</param>
        public void FillData(List <string> ListLayername,List<IFeatureClass> ListFeatureClass, IQueryFilter pQueryFilter, IQueryFilter pQueryFilterOrder1, IQueryFilter pQueryFilterOrder2, bool isHighLight)
        {
            if (ListFeatureClass == null)
            {
                return;
            }
            if (ListFeatureClass.Count == 0)
                return;

            _QueryFeatureClass = ListFeatureClass[0];

            if (_FeaCursor != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_FeaCursor);
                _FeaCursor = null;
            }
            //if (_DicFeature == null)
            //{
            //    _DicFeature = new Dictionary<int, int>();
            //}
            labelItem.Visible = true;
            comboBoxFeatureLayer.Visible = true;

            if (_DicFeatureClass == null)
            {
                _DicFeatureClass = new Dictionary<string, IFeatureClass>();            
            }

            for (int i = 0; i < ListLayername.Count; i++)
            {
                comboBoxFeatureLayer.Items.Add(ListLayername[i]);
                if (!_DicFeatureClass.ContainsKey(ListLayername[i]))
                {
                    _DicFeatureClass.Add(ListLayername[i], ListFeatureClass[i]);
                }
            }
            string LayerName = ListLayername[0];
            for (int i = 0; i < comboBoxFeatureLayer.Items.Count; i++)
            {
                if (comboBoxFeatureLayer.Items[i].ToString() == ListLayername[0])
                {
                    comboBoxFeatureLayer.SelectedIndex = i;
                    break;
                }
            }
            this.comboBoxFeatureLayer.Enabled = true;

            //m_dataSourceGrid.Clear();

            //获取查询的图层

            progressBarItem.Visible = false;
            this.Refresh();
            bool bFirst = true;
            //added by chulili 20110801 添加错误保护
            if (_QueryFeatureClass  == null)
                return;
            _QueryFilterAll = pQueryFilter;
            _QueryFilterPart1 = pQueryFilterOrder1;
            _QueryFilterPart2 = pQueryFilterOrder2;
            //循环取得选中Feature的ID来得到Feature
            //初始化表
            _DataTablePart = InitDataTable(_QueryFeatureClass);
            _DataTableAll = InitDataTable(_QueryFeatureClass);
            if (_DicDatatable == null)
                _DicDatatable = new Dictionary<string, DataTable>();
            //绑定 以便动态显示
            this.gridRes.DataSource = _DataTablePart;

            ISelectionSet pSelectSet = _QueryFeatureClass.Select(pQueryFilter, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, (_QueryFeatureClass as IDataset).Workspace);
            if (pSelectSet == null)
            {
                labelItemMemo.Text = "查找到0个要素";
                progressBarItem.Visible = false;
                this.Refresh();
                return;
            }
            //进度条设置
            progressBarItem.Visible = true;
            this.Refresh();
            progressBarItem.Maximum = pSelectSet.Count + 100;
            progressBarItem.Minimum = 0;
            progressBarItem.Value = 0;

            ICursor pCursor1 = null, pCursor2 = null;
            pSelectSet.Search(pQueryFilterOrder1, false, out pCursor1);
            pSelectSet.Search(pQueryFilterOrder2, false, out pCursor2);

            IRow pRow = pCursor1.NextRow();
            int indexFeature = 0;
            IFeature pFeature = null;
            while (pRow != null)
            {
                //进度条
                progressBarItem.Value = progressBarItem.Value + 1;

                _FeaCnt = _FeaCnt + 1;
                //_DicFeature.Add(indexFeature, pRow.OID);
                InsterFeaToTable(pRow, _DataTableAll);
                indexFeature = indexFeature + 1;
                pRow = pCursor1.NextRow();
            }

            pRow = pCursor2.NextRow();
            while (pRow != null)
            {
                //进度条
                progressBarItem.Value = progressBarItem.Value + 1;

                _FeaCnt = _FeaCnt + 1;
                //_DicFeature.Add(indexFeature, pRow.OID);
                InsterFeaToTable(pRow, _DataTableAll);
                indexFeature = indexFeature + 1;
                pRow = pCursor2.NextRow();
            }
            int iIndex = 0;
            for (iIndex = 0; iIndex < 100; iIndex++)
            {
                //进度条
                progressBarItem.Value = progressBarItem.Value + 1;
                if (iIndex >= _DataTableAll.Rows.Count)
                {
                    break;
                }
                DataRow pDataRow = _DataTableAll.Rows[iIndex];
                DataRow pDataRow2 = _DataTablePart.NewRow();
                pDataRow2.ItemArray = pDataRow.ItemArray;
                _DataTablePart.Rows.Add(pDataRow2);
            }
            if (!_DicDatatable.ContainsKey(LayerName))
            {
                _DicDatatable.Add(LayerName, _DataTableAll);
            }
            progressBarItem.Value = progressBarItem.Maximum;
            _CurMaxID = iIndex - 1;
            _PageID = 1;
            this.textBoxPage.ControlText = _PageID.ToString();
            //下一页状态
            if (_FeaCnt > _CurMaxID + 1)
            {
                this.buttonNextPage.Enabled = true;
            }
            else
            {
                this.buttonNextPage.Enabled = false;
            }
            //上一页状态
            if (_CurMaxID > 100)
            {
                this.buttonLastPage.Enabled = true;
            }
            else
            {
                this.buttonLastPage.Enabled = false;
            }

            labelItemMemo.Text = "查找到" + _FeaCnt.ToString() + "个要素";
            progressBarItem.Value = 0;
            progressBarItem.Enabled = false;
            //处理隐藏字段
            for (int i = 0; i < gridRes.Columns.Count; i++)
            {
                DataGridViewColumn pColumn = gridRes.Columns[i];
                pColumn.HeaderText = _DataTablePart.Columns[i].ColumnName;
                pColumn.Name = _DataTablePart.Columns[i].Caption;
                string strFieldname = pColumn.Name;
                if (SysCommon.ModField._ListHideFields.Contains(pColumn.Name))
                {
                    pColumn.Visible = false;
                }
            }
            this.Refresh();
            IGraphicsContainer psGra = m_pMapControl.Map as IGraphicsContainer;
            if (isHighLight)
            {
                //drawPolyLineElement(pListGeometry, psGra);
                (m_pMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
            }
        }
        /// <summary>褚丽丽添加函数 20110802
        /// 根据条件查找数据填充窗体，用来进行SQL查询,专门用于地名查询,河流,道路查询
        /// </summary>
        /// <param name="pMap">被查询的地图对象</param>
        public void FillData(IFeatureClass pFeatureClass,string strLayerName, IQueryFilter pQueryFilter,IQueryFilter pQueryFilterOrder1,IQueryFilter pQueryFilterOrder2,bool isHighLight)
        {
            if (pFeatureClass == null)
            {
                return;
            }
            _QueryFeatureClass = pFeatureClass;

            if (_FeaCursor != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_FeaCursor);
                _FeaCursor = null;
            }
            //if (_DicFeature == null)
            //{
            //    _DicFeature = new Dictionary<int, int>();
            //}
            labelItem.Visible = true ;
            comboBoxFeatureLayer.Visible = true ;            
            if(!comboBoxFeatureLayer.Items.Contains(pFeatureClass.AliasName ))
            {
                comboBoxFeatureLayer.Items.Add(strLayerName);
            }
            for (int i = 0; i < comboBoxFeatureLayer.Items.Count; i++)
            {
                if (comboBoxFeatureLayer.Items[i].ToString() == strLayerName)
                {
                    comboBoxFeatureLayer.SelectedIndex = i;
                    break;
                }
            }
            this.comboBoxFeatureLayer.Enabled = false;

            //m_dataSourceGrid.Clear();

            //获取查询的图层

            progressBarItem.Visible = false;
            this.Refresh();
            bool bFirst = true;
            //added by chulili 20110801 添加错误保护
            if (pFeatureClass == null)
                return;
            _QueryFilterAll = pQueryFilter;
            _QueryFilterPart1 = pQueryFilterOrder1;
            _QueryFilterPart2 = pQueryFilterOrder2;
            //循环取得选中Feature的ID来得到Feature
            //初始化表
            _DataTablePart = InitDataTable(pFeatureClass);
            _DataTableAll = InitDataTable(pFeatureClass); 
            //绑定 以便动态显示
            this.gridRes.DataSource = _DataTablePart;

            ISelectionSet pSelectSet = null;
            try
            {
                pSelectSet = pFeatureClass.Select(pQueryFilter, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, (pFeatureClass as IDataset).Workspace);

            }
            catch(Exception errtmp)
            {}
            if (pSelectSet == null)
            {
                labelItemMemo.Text = "查找到0个要素";
                progressBarItem.Visible = false;
                this.Refresh();
                return;
            }
            //进度条设置
            progressBarItem.Visible = true;
            this.Refresh();
            progressBarItem.Maximum = pSelectSet.Count+100;
            progressBarItem.Minimum = 0;
            progressBarItem.Value = 0;

            ICursor pCursor1 = null, pCursor2 = null;

                pSelectSet.Search(pQueryFilterOrder1, false, out pCursor1);
                pSelectSet.Search(pQueryFilterOrder2, false, out pCursor2);

            IRow pRow = pCursor1.NextRow();
            int indexFeature=0;
            IFeature pFeature = null;
            while (pRow != null)
            {
                //进度条
                progressBarItem.Value = progressBarItem.Value + 1;

                _FeaCnt = _FeaCnt + 1;
                //_DicFeature.Add(indexFeature, pRow.OID );
                InsterFeaToTable(pRow, _DataTableAll);
                indexFeature = indexFeature + 1;
                pRow = pCursor1.NextRow();
            }

            pRow = pCursor2.NextRow();
            while (pRow != null)
            {
                //进度条
                progressBarItem.Value = progressBarItem.Value + 1;

                _FeaCnt = _FeaCnt + 1;
                //_DicFeature.Add(indexFeature, pRow.OID );
                InsterFeaToTable(pRow, _DataTableAll);
                indexFeature = indexFeature + 1;
                pRow = pCursor2.NextRow();
            }
            int iIndex = 0;
            for (iIndex = 0; iIndex < 100; iIndex++)
            {
                //进度条
                progressBarItem.Value = progressBarItem.Value + 1;
                if (iIndex >= _DataTableAll.Rows.Count)
                {
                    break;
                }
                DataRow pDataRow = _DataTableAll.Rows[iIndex];
                DataRow pDataRow2 = _DataTablePart.NewRow();
                pDataRow2.ItemArray = pDataRow.ItemArray;
                _DataTablePart.Rows.Add(pDataRow2);
            }
            progressBarItem.Value = progressBarItem.Maximum;
            _CurMaxID = iIndex - 1;
            _PageID = 1;
            this.textBoxPage.ControlText = _PageID.ToString();
            //下一页状态
            if (_FeaCnt > _CurMaxID + 1)
            {
                this.buttonNextPage.Enabled = true;
            }
            else
            {
                this.buttonNextPage.Enabled = false ;
            }
            //上一页状态
            if (_CurMaxID> 100)
            {
                this.buttonLastPage.Enabled = true;
            }
            else
            {
                this.buttonLastPage.Enabled = false ;
            }

            labelItemMemo.Text = "查找到" + _FeaCnt.ToString() + "个要素";
            progressBarItem.Value = 0;
            progressBarItem.Enabled  = false;
            //处理隐藏字段
            for (int i = 0; i < gridRes.Columns.Count; i++)
            {
                DataGridViewColumn pColumn = gridRes.Columns[i];
                pColumn.HeaderText = _DataTablePart.Columns[i].ColumnName;
                pColumn.Name = _DataTablePart.Columns[i].Caption;
                if (SysCommon.ModField._ListHideFields != null)
                {
                    if (SysCommon.ModField._ListHideFields.Contains(pColumn.Name))
                    {
                        pColumn.Visible = false;
                    }
                }
            }
            this.Refresh();
            IGraphicsContainer psGra = m_pMapControl.Map as IGraphicsContainer;
            if (isHighLight)
            {
                //drawPolyLineElement(pListGeometry, psGra);
                (m_pMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
            }
        }
        ////处理要素内容到表中去
        //private void InsterFeaToTable(IFeature pFea, DataTable Dt)
        //{
        //    if (Dt == null) return;
        //    DataRow vRow = Dt.NewRow();

        //    for (int i = 0; i < Dt.Columns.Count; i++)
        //    {
        //        string strColumnName = Dt.Columns[i].ColumnName;
        //        int intIndex = pFea.Fields.FindField(strColumnName);
        //        if (intIndex < 0) continue;

        //        object obj = pFea.get_Value(intIndex);
        //        if (obj == null) continue;

        //        string strValue = obj.ToString();
        //        if (pFea.Fields.get_Field(intIndex).Type == esriFieldType.esriFieldTypeDouble)
        //        {
        //            double dblTempa = 0;
        //            double.TryParse(strValue, out dblTempa);
        //            strValue = string.Format("{0:f2}", dblTempa);
        //        }

        //        vRow[strColumnName] = strValue;
        //    }

        //    Dt.Rows.Add(vRow);
        //}
        //处理要素内容到表中去
        private void InsterFeaToTable(IRow pRow, DataTable Dtall)
        {
            if (Dtall == null) return;
            DataRow vRow = Dtall.NewRow();
            
            for (int i = 0; i < Dtall.Columns.Count; i++)
            {
                string strColumnName = Dtall.Columns[i].ColumnName;//中文字段名
                string strCaption = Dtall.Columns[i].Caption;//英文字段名
                int intIndex = pRow.Fields.FindField(strCaption);
                
                if (intIndex < 0) continue;

                object obj = pRow.get_Value(intIndex);
                if (obj == null) continue;

                string strValue = obj.ToString();
                if (pRow.Fields.get_Field(intIndex).Type == esriFieldType.esriFieldTypeDouble)
                {
                    double dblTempa = 0;
                    double.TryParse(strValue, out dblTempa);
                    strValue = string.Format("{0:f2}", dblTempa);
                }
                //added by chulili 20110818 统一对字段值进行中英文映射
                if (SysCommon.ModField._DicMatchFieldValue.Keys.Contains(strCaption))
                {
                    strValue = SysCommon.ModField.GetChineseOfFieldValue(strCaption, strValue);
                }
                //if (strColumnName.Equals("国标分类码"))
                //{
                //    strValue = SysCommon.ModField.GetChineseNameOfGB(strValue);
                //}
                vRow[strColumnName] = strValue;
            }

            Dtall.Rows.Add(vRow);
        }
        private DataTable InitDataTable(IFeatureClass pFeaCls)
        {
            DataTable TempDt = new DataTable();

            for (int i = 0; i < pFeaCls.Fields.FieldCount; i++)
            {
                try
                {
                    IField pField = pFeaCls.Fields.get_Field(i);
                    if (pField.Type == esriFieldType.esriFieldTypeGeometry || pField.Type == esriFieldType.esriFieldTypeBlob) continue;
                    string strFieldName = pField.Name;
                    string strFieldChineseName = SysCommon.ModField.GetChineseNameOfField(strFieldName);
                    DataColumn pColumn = new DataColumn();
                    pColumn.ColumnName = strFieldChineseName;
                    pColumn.Caption = strFieldName;
                    pColumn.DataType = Type.GetType("System.String");
                    //都把它当作字符串类型了
                    //TempDt.Columns.Add(strFieldChineseName, Type.GetType("System.String"));
                    TempDt.Columns.Add(pColumn);
                    if (pFeaCls.OIDFieldName == strFieldName)
                    {
                        _OIDfieldIndex = TempDt.Columns.IndexOf(strFieldChineseName);
                    }
                }
                catch(Exception err)
                {
                    continue;
                }
            }            

            //TempDt.Columns.Add("个数", Type.GetType("System.String"));

            return TempDt;
        }
      

  

        //查询窗体关闭
        private void frmQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            //m_pMapControl.CurrentTool = null;
            //yjladded 为了高亮显示查询的要素
            IGraphicsContainer pGra = m_pMapControl.Map as IGraphicsContainer;
            pGra.Reset();
            IElement pEle = pGra.Next();
            
            while (pEle != null)
            {
                if ((pEle as IElementProperties).Name == "frmQueryToTable" || (pEle as IElementProperties).Name == "RoadQuery")
                {
                    pGra.DeleteElement(pEle);
                    pGra.Reset();
                }

                pEle = pGra.Next();
            }
            (m_pMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null); //changed by xisheng 20110803
            _DataTablePart.Clear();
            _DataTableAll.Clear();
            //_DicFeature.Clear();
            if (_DicDatatable != null)
            {
                _DicDatatable.Clear();
            }
            if (_DicFeatureClass != null)
            {
                _DicFeatureClass.Clear();
            }

            _DataTablePart = null;
            _DataTableAll = null;
            //_DicFeature = null;
            _DicDatatable = null;
            _DicFeatureClass = null;

            //20110731 xisheng
            //(pGra as IActiveView).Refresh();
            //yjladded 为了高亮显示查询的要素
            
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
                        //(pPolylineElement as IElementProperties).Name == "frmQueryToTable";
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

        private void buttonNextPage_Click(object sender, EventArgs e)
        {           
            _DataTablePart.Clear();
            if (_CurMaxID + 1 >= _FeaCnt)
            {
                return;
            }
            int i = 0;
            for (i = _CurMaxID + 1; i <= _CurMaxID+100; i++)
            {
                if (i >= _FeaCnt)
                {
                    break;
                }
                DataRow pDataRow = _DataTableAll.Rows[i];
                DataRow pDataRow2 = _DataTablePart.NewRow();
                pDataRow2.ItemArray = pDataRow.ItemArray;
                _DataTablePart.Rows.Add(pDataRow2);
            }
            _CurMaxID = i-1;
            _PageID = _PageID + 1;
            this.textBoxPage.ControlText = _PageID.ToString();
            //下一页状态
            if (_FeaCnt > _CurMaxID + 1)
            {
                this.buttonNextPage.Enabled = true;
            }
            else
            {
                this.buttonNextPage.Enabled = false;
            }
            //上一页状态
            if (_CurMaxID > 100)
            {
                this.buttonLastPage.Enabled = true;
            }
            else
            {
                this.buttonLastPage.Enabled = false;
            }
        }

        private void buttonLastPage_Click(object sender, EventArgs e)
        {
            _DataTablePart.Clear();
            int i = 0;
            if (_PageID <= 1)
            {
                return;
            }
            for (i = (_PageID - 2) * 100; i < (_PageID - 2) * 100 + 100; i++)
            {
                if (i >= _FeaCnt)
                {
                    break;
                }
                DataRow pDataRow = _DataTableAll.Rows[i];
                DataRow pDataRow2 = _DataTablePart.NewRow();
                pDataRow2.ItemArray = pDataRow.ItemArray;
                _DataTablePart.Rows.Add(pDataRow2);
            }
            _CurMaxID = i - 1;
            _PageID = _PageID - 1;
            this.textBoxPage.ControlText = _PageID.ToString();
            //下一页状态
            if (_FeaCnt > _CurMaxID + 1)
            {
                this.buttonNextPage.Enabled = true;
            }
            else
            {
                this.buttonNextPage.Enabled = false;
            }
            //上一页状态
            if (_CurMaxID > 100)
            {
                this.buttonLastPage.Enabled = true;
            }
            else
            {
                this.buttonLastPage.Enabled = false;
            }
        }

        private void buttonAll_Click(object sender, EventArgs e)
        {
            _DataTablePart.Clear();
            int i = 0;
            progressBarItem.Enabled = true;
            this.Refresh();
            progressBarItem.Maximum = _FeaCnt;
            progressBarItem.Value = 0;
            for (i = 0; i < _FeaCnt; i++)
            {
                progressBarItem.Value = progressBarItem.Value + 1;
                DataRow pDataRow = _DataTableAll.Rows[i];
                DataRow pDataRow2 = _DataTablePart.NewRow();
                pDataRow2.ItemArray = pDataRow.ItemArray;
                _DataTablePart.Rows.Add(pDataRow2);
            }
            _CurMaxID = i - 1;
            _PageID =  1;
            this.textBoxPage.ControlText = _PageID.ToString();
            //下一页状态 
            this.buttonNextPage.Enabled = false;

            //上一页状态
            this.buttonLastPage.Enabled = false;
            progressBarItem.Value = 0;
        }

        private void gridRes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            string strFeatureOID = gridRes.SelectedRows[0].Cells[_QueryFeatureClass.OIDFieldName].Value.ToString();
            if (strFeatureOID.Equals(""))
                return;
            
            
            try
            {
                int iFeatureOID = Convert.ToInt32(strFeatureOID);
                IFeature pFeature = _QueryFeatureClass.GetFeature(iFeatureOID);
                
                if (pFeature == null)
                {
                    return;
                }
                //yjladded 为了高亮显示查询的要素
                IGraphicsContainer pGra = m_pMapControl.Map as IGraphicsContainer;
                pGra.Reset();
                IElement pEle = pGra.Next();
                //yjl0729,add,局部刷新未起作用
                (pGra as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);//更改时需2次
                while (pEle != null)
                {
                    if ((pEle as IElementProperties).Name == _FlashTag)
                    {
                        pGra.DeleteElement(pEle);
                        pGra.Reset();
                    }

                    pEle = pGra.Next();
                }
                (pGra as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
                //yjladded 为了高亮显示查询的要素
                if (pFeature != null)
                {
                    SysCommon.Gis.ModGisPub.ZoomToFeature(m_pMapControl, pFeature);
                    Application.DoEvents();
                    ModDBOperator._FlashTagName = _FlashTag;
                    ModDBOperator.FlashFeature(pFeature, m_pMapControl);
                    //m_pMapControl.FlashShape(pfeature.Shape, 2, 500, null);
                }
            }
            catch
            {
            }
        }

        private void textBoxPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    int iPage = Convert.ToInt32(textBoxPage.ControlText.Trim());
                    GotoPage(iPage );
                }
                catch
                { }
            }
        }
        private void GotoPage(int pageID)
        {
            if ((pageID - 1) * 100 < _FeaCnt)
            {
                _DataTablePart.Clear();
                int i = 0;
                for (i = (pageID - 1) * 100; i < (pageID - 1) * 100+100; i++)
                {
                    if (i >= _FeaCnt)
                    {
                        break;
                    }
                    DataRow pDataRow = _DataTableAll.Rows[i];
                    DataRow pDataRow2 = _DataTablePart.NewRow();
                    pDataRow2.ItemArray = pDataRow.ItemArray;
                    _DataTablePart.Rows.Add(pDataRow2);
                }
                _CurMaxID = i - 1;
                _PageID = pageID;

                //下一页状态
                if (_FeaCnt > _CurMaxID + 1)
                {
                    this.buttonNextPage.Enabled = true;
                }
                else
                {
                    this.buttonNextPage.Enabled = false;
                }
                //上一页状态
                if (_CurMaxID > 100)
                {
                    this.buttonLastPage.Enabled = true;
                }
                else
                {
                    this.buttonLastPage.Enabled = false;
                }
            }
        }

        private void textBoxPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789";
            if (!char.IsControl(e.KeyChar ) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

        private void comboBoxFeatureLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strLayerName = comboBoxFeatureLayer.SelectedItem.ToString()  ;
            if (_QueryFilterAll == null)
            {
                return;
            }
            if (_DicFeatureClass.ContainsKey(strLayerName))
            {
                FillData(strLayerName);
            }

        }
        private void FillData(string strLayerName)
        {
            if (_DicDatatable == null)
            {
                _DicDatatable = new Dictionary<string, DataTable>();
            }
            if (_DicFeatureClass.ContainsKey(strLayerName))
            {
                _QueryFeatureClass = _DicFeatureClass[strLayerName];
            }
            if (_DicDatatable.ContainsKey(strLayerName))
            {
                _DataTableAll = _DicDatatable[strLayerName];
                _FeaCnt = _DataTableAll.Rows.Count;
                //进度条设置
                progressBarItem.Visible = true;
                this.Refresh();
                progressBarItem.Maximum = 100;
                progressBarItem.Minimum = 0;
                progressBarItem.Value = 0;
            }
            else
            {
                //_DataTableAll = null;
                _DataTableAll = InitDataTable(_QueryFeatureClass);
                //绑定 以便动态显示
                ISelectionSet pSelectSet = _QueryFeatureClass.Select(_QueryFilterAll, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, (_QueryFeatureClass as IDataset).Workspace);
                if (pSelectSet == null)
                {
                    labelItemMemo.Text = "查找到0个要素";
                    progressBarItem.Visible = false;
                    this.Refresh();
                    return;
                }
                //进度条设置
                progressBarItem.Visible = true;
                this.Refresh();
                progressBarItem.Maximum = pSelectSet.Count + 100;
                progressBarItem.Minimum = 0;
                progressBarItem.Value = 0;

                ICursor pCursor1 = null, pCursor2 = null;
                pSelectSet.Search(_QueryFilterPart1 , false, out pCursor1);
                pSelectSet.Search(_QueryFilterPart2 , false, out pCursor2);

                IRow pRow = pCursor1.NextRow();
                int indexFeature = 0;
                _FeaCnt = 0;
                IFeature pFeature = null;
                while (pRow != null)
                {
                    //进度条
                    progressBarItem.Value = progressBarItem.Value + 1;

                    _FeaCnt = _FeaCnt + 1;
                    //_DicFeature.Add(indexFeature, pRow.OID);
                    InsterFeaToTable(pRow, _DataTableAll);
                    indexFeature = indexFeature + 1;
                    pRow = pCursor1.NextRow();
                }

                pRow = pCursor2.NextRow();
                while (pRow != null)
                {
                    //进度条
                    progressBarItem.Value = progressBarItem.Value + 1;

                    _FeaCnt = _FeaCnt + 1;
                    //_DicFeature.Add(indexFeature, pRow.OID);
                    InsterFeaToTable(pRow, _DataTableAll);
                    indexFeature = indexFeature + 1;
                    pRow = pCursor2.NextRow();
                }
            }
            int iIndex = 0;
            _DataTablePart = null;
            _DataTablePart = InitDataTable(_QueryFeatureClass);

            _DataTablePart.Clear();
            for (iIndex = 0; iIndex < 100; iIndex++)
            {
                //进度条
                progressBarItem.Value = progressBarItem.Value + 1;
                if (iIndex >= _FeaCnt)
                {
                    break;
                }
                DataRow pDataRow = _DataTableAll.Rows[iIndex];
                DataRow pDataRow2 = _DataTablePart.NewRow();
                pDataRow2.ItemArray = pDataRow.ItemArray;
                _DataTablePart.Rows.Add(pDataRow2);
            }
            gridRes.DataSource = _DataTablePart;
            //处理隐藏字段
            for (int i = 0; i < gridRes.Columns.Count; i++)
            {
                DataGridViewColumn pColumn = gridRes.Columns[i];
                pColumn.HeaderText = _DataTablePart.Columns[i].ColumnName;
                pColumn.Name = _DataTablePart.Columns[i].Caption ;
                string strTmpName=pColumn.Name;
                if (SysCommon.ModField._ListHideFields.Contains(pColumn.Name))
                {
                    pColumn.Visible = false;
                }
            }
            progressBarItem.Value = progressBarItem.Maximum;
            _CurMaxID = iIndex - 1;
            _PageID = 1;
            this.textBoxPage.ControlText = _PageID.ToString();
            //下一页状态
            if (_FeaCnt > _CurMaxID + 1)
            {
                this.buttonNextPage.Enabled = true;
            }
            else
            {
                this.buttonNextPage.Enabled = false;
            }
            //上一页状态
            if (_CurMaxID > 100)
            {
                this.buttonLastPage.Enabled = true;
            }
            else
            {
                this.buttonLastPage.Enabled = false;
            }

            labelItemMemo.Text = "查找到" + _FeaCnt.ToString() + "个要素";
            progressBarItem.Value = 0;
            progressBarItem.Enabled = false;
            this.Refresh();
        }
    }
} 