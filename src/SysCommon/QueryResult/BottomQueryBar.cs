using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;
using System.Xml;
using ESRI.ArcGIS.DataSourcesGDB;

namespace Fan.Common
{
    public partial class BottomQueryBar : BaseControl
    {
        //private string LayerTreexmlpath = System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\查询图层树_QueryBar.xml"; 
        //private string m_DicPath = System.Windows.Forms.Application.StartupPath + "\\..\\Template\\Dic.gdb";
        public BottomQueryBar()
        {
            InitializeComponent();
        }

        //private List<string> _ListDataNodeKeys = null;  
        //public List<string> ListDataNodeKeys
        //{
        //    set 
        //    {
        //        _ListDataNodeKeys = value;
        //    }
        //    get
        //    {
        //        return _ListDataNodeKeys;
        //    }
        //}

        //private string _DefaultNodeKey = "";   
        //public IWorkspace m_WorkSpace = null;
        //private DataTable m_dataSourceGrid=null;
        //public IMap m_Map { get; set; }
        //public IGeometry m_Geometry { get; set; }
        //private esriSpatialRelEnum m_esriSpatialRelEnum;
        //IList<string> ListLayerName=new List<string >();
        //IList<ILayer> ListLayer;
        //IFeatureClass m_FeatureClass;
        //IGeometry m_DrawGeometry;
        //public IMapControlDefault m_pMapControl { get; set; }
        //string FeaOID = "";

        //private string _FieldOID = "";
        //#region  查询数据展现
        //internal void AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        //{
        //    if (this.IsDisposed == true) return;
        //    if (phase == esriViewDrawPhase.esriViewForeground) drawgeometryXOR(m_DrawGeometry);
        //}
        //internal enum enumQueryType
        //{
        //    enumGeometryQuery,  
        //    enumSQLQuery,       
        //    enumDomainQuery    
        //}

        //private enumQueryType _CurQueryType;   

        //private IQueryFilter _CurQueryFilter = null;
        //private List<ILayer> _ListDomainListLayers = null;
        //private List<string> _ListDomainNodeKeys = null;
        //private List<IFeatureClass> _ListDomainFeatureClasses = null;
        //private List<string> _ListDomainLayerNames = null;
        //private string _CurNodeKey = "";  

        //private void InitializeTable(IFeatureClass pFeatureClass,string tablName,ILayerFields pLayerFields)
        //{
        //    m_dataSourceGrid = new DataTable(tablName);
        //    for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
        //    {
        //        try
        //        {
        //            string fields = pFeatureClass.Fields.get_Field(i).Name .ToString ();
        //            esriFieldType pFieldType= pFeatureClass.Fields.get_Field(i).Type;
        //            if (pLayerFields != null && pFieldType!=esriFieldType.esriFieldTypeOID)
        //            {
        //                IFieldInfo pFieldInfo = pLayerFields.get_FieldInfo(i);
        //                if (pFieldInfo != null)
        //                {
        //                    if (pFieldInfo.Visible != true)
        //                    {
        //                        continue;
        //                    }
        //                }
        //            }
        //            if (fields != "Shape"&&fields!="SHAPE")
        //            {
        //                DataColumn colName = new DataColumn(fields);
        //                colName.Caption = Fan.Common.ModField.GetChineseNameOfField(fields);
        //                colName.DataType = System.Type.GetType("System.String");
        //                m_dataSourceGrid.Columns.Add(colName);
        //            }
        //        }
        //        catch
        //        {

        //        }
        //    }
        //}
        //private void InitializeTable(IFeatureClass pFeatureClass, string tablName)
        //{
        //    InitializeTable(pFeatureClass, tablName, null);
        //}
        ///// <summary>
        ///// 根据范围查找数据填充窗体
        ///// ygc 2012-8-9
        ///// </summary>
        ///// <param name="pMap"></param>
        ///// <param name="pGeometry"></param>
        ///// <param name="pesriSpatialRelEnum"></param>
        //public void EmergeQueryData(IMap pMap, IGeometry pGeometry, esriSpatialRelEnum pesriSpatialRelEnum)
        //{
        //    ListLayerName.Clear();
        //    Fan.Common.ScreenDraw.list.Add(AfterDraw);
        //   // m_dataSourceGrid.Rows.Clear();
        //    m_Map = pMap;
        //    m_Geometry = pGeometry;
        //    m_esriSpatialRelEnum = pesriSpatialRelEnum;
        //    if (pGeometry == null)
        //    {
        //        return;
        //    }
        //    UID pUID = new UIDClass();
        //    pUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";   //UID for IFeatureLayer
        //    IEnumLayer pEnumLayer = pMap.get_Layers(pUID, true);
        //    pEnumLayer.Reset();
        //    if (ListLayer != null)
        //    {
        //        ListLayer.Clear();
        //        ListLayer = null;
        //    }
        //    ILayer pLayer = pEnumLayer.Next();
        //    //获取查询的图层
        //     ListLayer = new List<ILayer>();
        //    while (pLayer != null)
        //    {
        //        ListLayer.Add(pLayer);
        //        ListLayerName.Add(pLayer.Name);
        //        pLayer = pEnumLayer.Next();
        //    }
        //    if (ListLayer.Count == 0)
        //    {
        //        return;
        //    }
        //    ILayer pDefaultLayer = null;

        //    pDefaultLayer = Fan.Common.ModuleMap.GetLayerByNodeKey(ListLayer, _DefaultNodeKey);

        //    if (pDefaultLayer == null)
        //    {
        //        pDefaultLayer = ListLayer[0];
        //        _DefaultNodeKey = Fan.Common.ModuleMap.GetNodeKeyOfLayer(pDefaultLayer);
        //    }

        //    InitializeGrid(pDefaultLayer, pGeometry, pesriSpatialRelEnum);
        //    comboLayers.Text = pDefaultLayer.Name;
        //    InitializeComBox();
        //    _CurQueryType = enumQueryType.enumGeometryQuery;
        //    InitLayersTree();
        //    this.Refresh();
        //}

        //public void EmergeQueryData(IMap pMap, IFeatureLayer pFeatLay, IQueryFilter pQueryFilter, esriSelectionResultEnum pSelectionResult)
        //{
        //    EmergeQueryData(pMap, pFeatLay, pQueryFilter, pSelectionResult, null);
        //}
        ///// <summary>
        ///// 根据条件查找数据填充窗体，用来进行SQL查询
        ///// ygc 2012-8-10
        ///// </summary>
        ///// <param name="pFeatLay"></param>
        ///// <param name="pQueryFilter"></param>
        ///// <param name="pSelectionResult"></param>
        //public void EmergeQueryData(IMap pMap, IFeatureLayer pFeatLay, IQueryFilter pQueryFilter, esriSelectionResultEnum pSelectionResult,Fan.Common.CProgress vProgress)
        //{

        //    m_dataSourceGrid = null;
        //    dataGridViewX1.DataSource = null;
        //    int count = 0;
        //    //ygc 20130416 获取字典
        //    Dictionary<string, Dictionary<string, string>> ChineseDic = ModXZQ.InitialDic(m_WorkSpace);
        //    dataGridViewX1.Rows.Clear();
        //    dataGridViewX1.Refresh();
        //    Fan.Common.ScreenDraw.list.Add(AfterDraw);
        //    m_FeatureClass = pFeatLay.FeatureClass;
        //    _DefaultNodeKey = ModuleMap.GetNodeKeyOfLayer(pFeatLay as ILayer);
        //    comboLayers.Text = pFeatLay.Name;
        //    m_Map = pMap;
        //    UID pUID = new UIDClass();
        //    pUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";   //UID for IFeatureLayer
        //    IEnumLayer pEnumLayer = pMap.get_Layers(pUID, true);
        //    pEnumLayer.Reset();
        //    ILayer pLayer = pEnumLayer.Next();
        //    //获取查询的图层
        //    if(ListLayer!=null)
        //    {
        //        ListLayer.Clear();
        //        ListLayer=null;
        //    }
        //    ListLayer = new List<ILayer>();
        //    while (pLayer != null)
        //    {
        //        if (( pLayer.Visible == true))//20110802
        //        {
        //            ListLayer.Add(pLayer);
        //            ListLayerName.Add(pLayer.Name);
        //        }
        //        pLayer = pEnumLayer.Next();
        //    }
        //    if (pFeatLay == null)
        //    {
        //        MessageBox.Show("图层数据不存在！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Error);
        //        return;
        //    }
        //    ILayerFields pLayerFields = (ILayerFields)pFeatLay;
        //    InitializeTable(pFeatLay.FeatureClass, "查询结果数据", pLayerFields);
        //    IFeatureSelection pFeatureSelection = pFeatLay as IFeatureSelection;
        //    pFeatureSelection.SelectFeatures(pQueryFilter, pSelectionResult, false);
        //    IFeature pFeature;
        //    IEnumIDs pEnumIDs = pFeatureSelection.SelectionSet.IDs;
        //    int iIDIndex = pEnumIDs.Next();

        //    while (iIDIndex != -1)
        //    {
        //        if (vProgress != null)
        //        {
        //            if (vProgress.UserAskCancel)
        //            {
        //                break;
        //            }
        //        }
        //        count++;
        //        pFeature = pFeatLay.FeatureClass.GetFeature(iIDIndex);
        //        //将查询到的数据填充到表中
        //        DataRow pRow = m_dataSourceGrid.NewRow();
        //        for (int i = 0; i < pFeature.Fields.FieldCount; i++)
        //        {
        //            string Values = pFeature.get_Value(i).ToString ();
        //            string FieldName = pFeature.Fields.get_Field(i).Name;
        //            esriFieldType pFieldType=pFeature.Fields.get_Field(i).Type;
        //            IFieldInfo pFieldInfo = pLayerFields.get_FieldInfo(i);
        //            if (pFieldInfo.Visible || pFieldType==esriFieldType.esriFieldTypeOID)
        //            {
        //               // Values = Fan.Common.ModXZQ.GetChineseName(m_WorkSpace, FieldName, Values);
        //                if (ChineseDic.ContainsKey(FieldName))
        //                {
        //                    Dictionary<string, string> tempDic = ChineseDic[FieldName];
        //                    if (FieldName == "SHENG" && Values.Length > 2) Values = Values.Substring(0, 2);
        //                    else if (FieldName == "SHI" && Values.Length > 4) Values = Values.Substring(0, 4);
        //                    else if (FieldName == "XIANG" && Values.Length > 8) Values = Values.Substring(0, 8);
        //                    else if (FieldName == "CUN" && Values.Length > 10) Values = Values.Substring(0, 10);
        //                    if (tempDic.ContainsKey(Values))
        //                    {
        //                        Values = tempDic[Values];
        //                    }
        //                }
        //                try
        //                {
        //                    if (FieldName != "Shape" && FieldName != "SHAPE")
        //                    {
        //                        try
        //                        {
        //                            pRow[FieldName] = Values;
        //                        }
        //                        catch
        //                        { }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.ToString());
        //                }
        //            }
        //        }
        //        if (pRow != null)
        //        {
        //            m_dataSourceGrid.Rows.Add(pRow);
        //        }
        //        if (count > 1000) break;
        //        iIDIndex = pEnumIDs.Next();
        //    }
        //    dataGridViewX1.DataSource  = m_dataSourceGrid;
        //    for (int j = 0; j < dataGridViewX1.Columns.Count; j++)
        //    {
        //        if (dataGridViewX1.Columns[j].HeaderText.Contains("OBJECTID") || dataGridViewX1.Columns[j].HeaderText.Contains("SHAPE") || dataGridViewX1.Columns[j].HeaderText.Contains("shape"))
        //        {
        //            dataGridViewX1.Columns[j].Visible = false;
        //        }
        //        dataGridViewX1.Columns[j].HeaderText = m_dataSourceGrid.Columns[j].Caption;
        //    }
        //    this.Refresh();
        //    InitializeComBox();
        //    _CurQueryType = enumQueryType.enumSQLQuery;
        //    InitLayersTree();
        //}
        //public void EmergeQueryData( IFeatureLayer pFeatLay, IQueryFilter pQueryFilter)
        //{
        //    EmergeQueryData(pFeatLay, pQueryFilter, null);
        //    InitLayersTree();
        //}
        ///// <summary>
        ///// 根据条件查找数据填充窗体，用来进行SQL查询
        ///// ygc 2012-8-10
        ///// </summary>
        ///// <param name="pFeatLay"></param>
        ///// <param name="pQueryFilter"></param>
        ///// <param name="pSelectionResult"></param>
        //public void EmergeQueryData(IFeatureLayer pFeatLay, IQueryFilter pQueryFilter, Fan.Common.CProgress vProgress)
        //{
        //    m_dataSourceGrid = null;
        //    dataGridViewX1.DataSource = null;
        //    dataGridViewX1.Rows.Clear();
        //    dataGridViewX1.Refresh();
        //    //ygc 20130416 获取字典
        //    Dictionary<string, Dictionary<string, string>> ChineseDic = ModXZQ.InitialDic(m_WorkSpace);
        //    Fan.Common.ScreenDraw.list.Add(AfterDraw);
        //    m_FeatureClass = pFeatLay.FeatureClass;
        //    _DefaultNodeKey = ModuleMap.GetNodeKeyOfLayer(pFeatLay as ILayer);
        //    comboLayers.Text = pFeatLay.Name;
        //    //获取查询的图层
        //    if (ListLayer != null)
        //    {
        //        ListLayer.Clear();
        //        ListLayer = null;
        //    }
        //    ListLayer = new List<ILayer>();
        //    //while (pLayer != null)
        //    //{
        //    //    if ((pLayer.Visible == true))//20110802
        //    //    {
        //    //        ListLayer.Add(pLayer);
        //    //        ListLayerName.Add(pLayer.Name);
        //    //    }
        //    //    pLayer = pEnumLayer.Next();
        //    //}
        //    ListLayerName.Add(pFeatLay.Name);
        //    if (pFeatLay == null)
        //    {
        //        MessageBox.Show("图层数据不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }
        //    ILayerFields pLayerFields = (ILayerFields)pFeatLay;
        //    InitializeTable(pFeatLay.FeatureClass, "查询结果数据", pLayerFields);
        //    IFeatureCursor pCursor = pFeatLay.Search(pQueryFilter, false);
        //    //IFeatureSelection pFeatureSelection = pFeatLay as IFeatureSelection;
        //    //pFeatureSelection.SelectFeatures(pQueryFilter, pSelectionResult, false);
        //    IFeature pFeature=null;
        //    //IEnumIDs pEnumIDs = pFeatureSelection.SelectionSet.IDs;
        //    //int iIDIndex = pEnumIDs.Next();
        //    if (pCursor != null)
        //    {
        //        pFeature = pCursor.NextFeature();
        //    }
        //    //ygc 20130326 显示查询到总数
        //    int count = pFeatLay.FeatureClass.FeatureCount(pQueryFilter);
        //    lblQueryCount.Text = "查询到的要素个数:"+count.ToString ();
        //    while (pFeature!=null)
        //    {
        //        if (vProgress != null)
        //        {
        //            if (vProgress.UserAskCancel)
        //            {
        //                break;
        //            }
        //        }
        //        //pFeature = pFeatLay.FeatureClass.GetFeature(iIDIndex);
        //        //将查询到的数据填充到表中
        //        DataRow pRow = m_dataSourceGrid.NewRow();
        //        for (int i = 0; i < pFeature.Fields.FieldCount; i++)
        //        {
        //            string Values = pFeature.get_Value(i).ToString();
        //            string FieldName = pFeature.Fields.get_Field(i).Name;
        //            esriFieldType pFieldType = pFeature.Fields.get_Field(i).Type;
        //            IFieldInfo pFieldInfo = pLayerFields.get_FieldInfo(i);
        //            if (pFieldInfo.Visible || pFieldType == esriFieldType.esriFieldTypeOID)
        //            {
        //               // Values = Fan.Common.ModXZQ.GetChineseName(m_WorkSpace, FieldName, Values);
        //                if (ChineseDic.ContainsKey(FieldName))
        //                {
        //                    Dictionary<string, string> tempDic = ChineseDic[FieldName];
        //                    if (FieldName == "SHENG" && Values.Length > 2) Values = Values.Substring(0, 2);
        //                    else if (FieldName == "SHI" && Values.Length > 4) Values = Values.Substring(0, 4);
        //                    else if (FieldName == "XIANG" && Values.Length > 8) Values = Values.Substring(0, 8);
        //                    else if (FieldName == "CUN" && Values.Length > 10) Values = Values.Substring(0, 10);
        //                    if (tempDic.ContainsKey(Values))
        //                    {
        //                        Values = tempDic[Values];
        //                    }
        //                }
        //                try
        //                {
        //                    if (FieldName != "Shape" && FieldName != "SHAPE")
        //                    {
        //                        try
        //                        {
        //                            pRow[FieldName] = Values;
        //                        }
        //                        catch
        //                        { }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.ToString());
        //                }
        //            }
        //        }
        //        if (pRow != null)
        //        {
        //            m_dataSourceGrid.Rows.Add(pRow);
        //        }
        //        pFeature = pCursor.NextFeature();
        //    }
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        //    pCursor = null;
        //    dataGridViewX1.DataSource = m_dataSourceGrid;
        //    for (int j = 0; j < dataGridViewX1.Columns.Count; j++)
        //    {
        //        if (dataGridViewX1.Columns[j].HeaderText.Contains("OBJECTID") || dataGridViewX1.Columns[j].HeaderText.Contains("SHAPE") || dataGridViewX1.Columns[j].HeaderText.Contains("shape"))
        //        {
        //            dataGridViewX1.Columns[j].Visible = false;
        //        }
        //        dataGridViewX1.Columns[j].HeaderText = m_dataSourceGrid.Columns[j].Caption;
        //    }
        //    this.Refresh();
        //    InitializeComBox();
        //    _CurQueryType = enumQueryType.enumSQLQuery;
        //    InitLayersTree();
        //}
        ///// <summary>
        ///// 将数据在表格中显示
        ///// ygc 2012-8-9
        ///// </summary>
        ///// <param name="pLayer"></param>
        ///// <param name="pGeometry"></param>
        ///// <param name="pesriSpatialRelEnum"></param>
        //private void InitializeGrid(ILayer pLayer, IGeometry pGeometry, esriSpatialRelEnum pesriSpatialRelEnum)
        //{
        //    m_dataSourceGrid = null;
        //    dataGridViewX1.DataSource = null;
        //    IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
        //    if (pFeatureLayer == null) return;
        //    m_FeatureClass = pFeatureLayer.FeatureClass;
        //    ILayerFields pLayerFields = (ILayerFields)pFeatureLayer;
        //    InitializeTable(m_FeatureClass, "查询结果数据", pLayerFields);


        //    ISpatialFilter pSpatialFilter = new SpatialFilterClass();
        //    pSpatialFilter.Geometry = pGeometry;
        //    pSpatialFilter.SpatialRel = pesriSpatialRelEnum;



        //    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pSpatialFilter, false);

        //    lblQueryCount.Text = "查询总要素个数：" + pFeatureLayer.FeatureClass.FeatureCount(pSpatialFilter).ToString();
        //    IFeature pFeature = null;
        //    if (pFeatureCursor != null)
        //    {
        //        try
        //        {
        //            pFeature = pFeatureCursor.NextFeature();
        //        }
        //        catch(Exception ex)
        //        { }
        //    }

        //    Fan.Common.CProgress vProgress = new Fan.Common.CProgress();
        //    vProgress.ShowDescription = true;
        //    vProgress.ShowProgressNumber = true;
        //    vProgress.TopMost = true;
        //    vProgress.EnableCancel = true;
        //    vProgress.EnableUserCancel(true);
        //    vProgress.FakeProgress = false;


        //    //this.Refresh();
        //    vProgress.MaxValue = pFeatureLayer.FeatureClass.FeatureCount(pSpatialFilter); //这句话效率较低，注释掉，采用假进度条
        //    //ygc 20130416 获取字典
        //    Dictionary<string, Dictionary<string, string>> ChineseDic = ModXZQ.InitialDic(m_WorkSpace);
        //    vProgress.ProgresssValue = 0;
        //    vProgress.Step = 1;
        //    this.Refresh();
        //    vProgress.ShowProgress();
        //    vProgress.SetProgress("正在查询数据......");
        //    while (pFeature != null)
        //    {
        //        vProgress.SetProgress("正在查询图层：" + pLayer.Name.ToString() + "数据......");
        //        vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
        //        DataRow pRow = m_dataSourceGrid.NewRow();
        //        for (int i = 0; i < pFeature.Fields.FieldCount; i++)
        //        {
        //            string Values = pFeature.get_Value(i).ToString();
        //            string fieldName = pFeature.Fields.get_Field(i).Name;
        //            esriFieldType pFieldType = pFeature.Fields.get_Field(i).Type;
        //            IFieldInfo pFieldinfo = pLayerFields.get_FieldInfo(i);
        //            if (pFieldinfo.Visible || pFieldType==esriFieldType.esriFieldTypeOID)
        //            {
        //                //Values = Fan.Common.ModXZQ .GetChineseName (m_WorkSpace, fieldName, Values);
        //                if (ChineseDic.ContainsKey(fieldName))
        //                {
        //                    Dictionary<string, string> tempDic = ChineseDic[fieldName];
        //                    if (fieldName == "SHENG" && Values.Length > 2) Values = Values.Substring(0, 2);
        //                    else if (fieldName == "SHI" && Values.Length > 4) Values = Values.Substring(0, 4);
        //                    else if (fieldName == "XIANG" && Values.Length > 8) Values = Values.Substring(0, 8);
        //                    else if (fieldName == "CUN" && Values.Length > 10) Values = Values.Substring(0, 10);
        //                    if (tempDic.ContainsKey(Values))
        //                    {
        //                        Values = tempDic[Values];
        //                    }
        //                }
        //                try
        //                {
        //                    if (fieldName != "Shape" && fieldName != "SHAPE")
        //                    {
        //                        try
        //                        {
        //                            pRow[fieldName] = Values;
        //                        }
        //                        catch
        //                        { }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.ToString());
        //                }
        //            }

        //        }
        //        m_dataSourceGrid.Rows.Add(pRow);
        //        pFeature = pFeatureCursor.NextFeature();
        //    }
        //    vProgress.Close();
        //    dataGridViewX1.DataSource = m_dataSourceGrid;
        //    //改变datagridview列名显示
        //    for (int j = 0; j < dataGridViewX1.Columns.Count; j++)
        //    {
        //        if (dataGridViewX1.Columns[j].HeaderText.Contains("OBJECTID") || dataGridViewX1.Columns[j].HeaderText.Contains("SHAPE") || dataGridViewX1.Columns[j].HeaderText.Contains("shape"))
        //        {
        //            dataGridViewX1.Columns[j].Visible = false;
        //        }
        //        dataGridViewX1.Columns[j].HeaderText = m_dataSourceGrid.Columns[j].Caption;
        //    }
        //}
        //private void InitializeSQLQueryGrid(IMap pMap,string strNodeKey,IQueryFilter pQueryFilter)
        //{
        //    m_dataSourceGrid = null;
        //    dataGridViewX1.DataSource = null;
        //    ILayer pLayer = GetLayerByNodeKey(ListLayer , strNodeKey);

        //    if (pLayer == null)
        //    {
        //        return;
        //    }
        //    IFeatureClass pFeatureClass = null;
        //    try
        //    {
        //        pFeatureClass = (pLayer as IFeatureLayer).FeatureClass;
        //    }
        //    catch
        //    { }
        //    ILayerFields pLayerFields = pLayer as ILayerFields;
        //    InitializeTable(pFeatureClass, "查询结果数据", pLayerFields);

        //    comboLayers.Text = pLayer.Name;
        //    ISelectionSet pSelectSet = pFeatureClass.Select(pQueryFilter, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, (pFeatureClass as IDataset).Workspace);
        //    if (pSelectSet == null)
        //    {
        //        this.Refresh();
        //        return;
        //    }
        //    ICursor pCursor = null;
        //    pSelectSet.Search(null, false, out pCursor);
        //    IRow pRow = pCursor.NextRow();
        //    while (pRow != null)
        //    {
        //        InsterFeaToTable(pRow, m_dataSourceGrid);
        //        pRow = pCursor.NextRow();
        //    }
        //    dataGridViewX1.DataSource = m_dataSourceGrid;
        //    //改变datagridview列名显示
        //    for (int j = 0; j < dataGridViewX1.Columns.Count; j++)
        //    {
        //        if (dataGridViewX1.Columns[j].HeaderText.Contains("OBJECTID") || dataGridViewX1.Columns[j].HeaderText.Contains("SHAPE") || dataGridViewX1.Columns[j].HeaderText.Contains("shape"))
        //        {
        //            dataGridViewX1.Columns[j].Visible = false;
        //        }
        //        dataGridViewX1.Columns[j].HeaderText = m_dataSourceGrid.Columns[j].Caption;
        //    } 
        //}
        ///// <summary>
        ///// 将数据在表格中显示
        ///// ygc 2012-8-9
        ///// </summary>
        ///// <param name="pLayer"></param>
        ///// <param name="pGeometry"></param>
        ///// <param name="pesriSpatialRelEnum"></param>
        //private void InitializeDomainQueryGrid(string strNodeKey)
        //{
        //    m_dataSourceGrid = null;
        //    dataGridViewX1.DataSource = null;
        //    ILayer pCurLayer = null; IFeatureClass pCurFeatureClass = null; string pCurLayerName = "";
        //    for (int i = 0; i < _ListDomainNodeKeys.Count; i++)
        //    {
        //        if (strNodeKey == _ListDomainNodeKeys[i])
        //        {
        //            pCurLayerName = _ListDomainLayerNames[i];
        //            pCurLayer = _ListDomainListLayers[i];
        //            pCurFeatureClass = _ListDomainFeatureClasses[i];
        //            break;
        //        }
        //    }
        //    if (pCurLayer == null)
        //    {
        //        InitializeTable(pCurFeatureClass, "查询结果数据");
        //    }
        //    else
        //    {
        //        ILayerFields pLayerFields = pCurLayer as ILayerFields;
        //        InitializeTable(pCurFeatureClass, "查询结果数据", pLayerFields);
        //    }
        //    comboLayers.Text = pCurLayerName;
        //    ISelectionSet pSelectSet = pCurFeatureClass.Select(_CurQueryFilter, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, (pCurFeatureClass as IDataset).Workspace);
        //    if (pSelectSet == null)
        //    {
        //        this.Refresh();
        //        return;
        //    }
        //    ICursor pCursor = null;
        //    pSelectSet.Search(null, false, out pCursor);
        //    IRow pRow = pCursor.NextRow();
        //    while (pRow != null)
        //    {
        //        InsterFeaToTable(pRow, m_dataSourceGrid);
        //        pRow = pCursor.NextRow();
        //    }
        //    dataGridViewX1.DataSource = m_dataSourceGrid;
        //    //改变datagridview列名显示
        //    for (int j = 0; j < dataGridViewX1.Columns.Count; j++)
        //    {
        //        if (dataGridViewX1.Columns[j].HeaderText.Contains("OBJECTID") || dataGridViewX1.Columns[j].HeaderText.Contains("SHAPE") || dataGridViewX1.Columns[j].HeaderText.Contains("shape"))
        //        {
        //            dataGridViewX1.Columns[j].Visible = false;
        //        }
        //        dataGridViewX1.Columns[j].HeaderText = m_dataSourceGrid.Columns[j].Caption;
        //    }
        //}
        ////通过图层名来获取图层
        ////ygc 2012-8-9
        //private ILayer GetLayerByName(IList<ILayer> listlayer, string LayerName)
        //{
        //    ILayer newLayer = null;
        //    if (listlayer == null) return null;
        //    if (listlayer.Count == 0) return null;
        //    for (int i = 0; i < listlayer.Count; i++)
        //    {
        //        if (listlayer[i].Name == LayerName)
        //        {
        //            newLayer = listlayer[i];
        //        }
        //    }
        //        return newLayer;
        //}
        ////通过NodeKey来获取图层
        ////added by chulili 2012-10-26
        //private ILayer GetLayerByNodeKey(IList<ILayer> listlayer, string strNodeKey)
        //{
        //    ILayer newLayer = null;
        //    if (listlayer == null) return null;
        //    if (listlayer.Count == 0) return null;
        //    for (int i = 0; i < listlayer.Count; i++)
        //    {
        //        ILayer pLayer = listlayer[i];
        //        ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
        //        //读取图层的描述
        //        string strNodeXml = pLayerGenPro.LayerDescription;
        //        XmlDocument pXmlDoc = new XmlDocument();
        //        pXmlDoc.LoadXml(strNodeXml);
        //        //构成xml节点，根据NodeKey在节点里查询
        //        string strSearch = "//Layer[@NodeKey=" + "'" + strNodeKey + "'" + "]";
        //        XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
        //        if (pNode != null)
        //        {
        //            pXmlDoc = null;
        //            return pLayer;
        //        }
        //    }
        //    return newLayer;
        //}
        ////通过OBJECTID来选择要素
        ////ygc 2012-8-9
        //private IFeature GetFeatureByOID(string OID, IFeatureClass pFeatureClass)
        //{
        //    string strFieldOID = GetOIDfield(pFeatureClass);
        //    IFeature newFeature = null;
        //    IQueryFilter pFilter=new QueryFilterClass ();
        //    if (OID == "")
        //    {
        //        return null;
        //    }
        //    pFilter .WhereClause =strFieldOID+"="+OID ;
        //    if (pFeatureClass == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        IFeatureCursor pFeatureCursor = pFeatureClass.Search(pFilter, false);
        //        newFeature = pFeatureCursor.NextFeature();
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
        //        return newFeature;
        //    }
        //}
        ////dataGridView单击事件
        ////ygc 2012-8-9
        //private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (dataGridViewX1.CurrentRow  ==null) return;
        //    string strFieldOID = GetOIDfield(m_FeatureClass);
        //    FeaOID = dataGridViewX1.CurrentRow.Cells[strFieldOID].Value.ToString().Trim();
        //    //RowIndex = dataGridViewX1.CurrentRow.Index;
        //    IFeature pCurrentFea = GetFeatureByOID(FeaOID, m_FeatureClass);
        //    if (pCurrentFea == null) return;
        //    try
        //    {

        //        m_pMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
        //        m_pMapControl.FlashShape(pCurrentFea .Shape, 3, 200, null);
        //        m_DrawGeometry = pCurrentFea.Shape;
        //        drawgeometryXOR(m_DrawGeometry);
        //    }
        //    catch
        //    {

        //    }


        //}
        //private string GetOIDfield(IFeatureClass pFeatureClass)
        //{
        //    if (pFeatureClass != null)
        //    {
        //        for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
        //        {
        //            try
        //            {
        //                esriFieldType pType = pFeatureClass.Fields.get_Field(i).Type;
        //                if (esriFieldType.esriFieldTypeOID == pType)
        //                {
        //                    return pFeatureClass.Fields.get_Field(i).Name;
        //                }
        //            }
        //            catch
        //            { }

        //        }
        //    }
        //    return "OBJECTID";
        //}
        ////datagridView双击事件，定位到选择的要素
        ////ygc 2012-8-9
        //private void dataGridViewX1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (dataGridViewX1.CurrentRow  == null) return;
        //    try
        //    {
        //        string strFieldOID = GetOIDfield(m_FeatureClass);
        //        string OID = dataGridViewX1.CurrentRow.Cells[strFieldOID].Value.ToString().Trim();
        //        IFeature pCurrentFea = GetFeatureByOID(OID, m_FeatureClass);
        //        Fan.Common.Gis.ModGisPub.ZoomToFeature(m_pMapControl, pCurrentFea);
        //        m_DrawGeometry = pCurrentFea.Shape;
        //        m_pMapControl.ActiveView.ScreenDisplay.UpdateWindow();
        //        m_pMapControl.FlashShape(m_DrawGeometry, 3, 200, null);
        //        drawgeometryXOR(m_DrawGeometry);
        //    }
        //    catch
        //    {

        //    }
        //}

        //private void drawgeometryXOR(IGeometry pGeometry)
        //{
        //    if (pGeometry == null)//如果窗体关闭或者取消 就不绘制 xisheng 2011.06.28
        //    {
        //        return;
        //    }
        //    IScreenDisplay pScreenDisplay = m_pMapControl.ActiveView.ScreenDisplay;
        //    ISymbol pSymbol = null;
        //    //颜色对象
        //    IRgbColor pRGBColor = new RgbColorClass();
        //    pRGBColor.UseWindowsDithering = false;
        //    pRGBColor = getRGB(255, 0, 0);
        //    pRGBColor.Transparency = 255;
        //    try
        //    {
        //        switch (pGeometry.GeometryType.ToString())
        //        {
        //            case "esriGeometryPoint"://点要素
        //                ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass();
        //                pMarkerSymbol.Size = 7.0;
        //                pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
        //                pMarkerSymbol.Color = pRGBColor;
        //                pSymbol = (ISymbol)pMarkerSymbol;
        //                pSymbol.ROP2 = esriRasterOpCode.esriROPCopyPen;
        //                break;
        //            case "esriGeometryPolyline"://线要素
        //                ISimpleLineSymbol pPolyLineSymbol = new SimpleLineSymbolClass();
        //                pPolyLineSymbol.Color = pRGBColor;
        //                pPolyLineSymbol.Width = 2.5;
        //                pPolyLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
        //                pSymbol = (ISymbol)pPolyLineSymbol;
        //                ///ZQ  20111117 modify
        //                pSymbol.ROP2 = esriRasterOpCode.esriROPCopyPen;
        //                break;
        //            case "esriGeometryPolygon"://面要素
        //                ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
        //                ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

        //                pSymbol = (ISymbol)pFillSymbol;
        //                pSymbol.ROP2 = esriRasterOpCode.esriROPCopyPen;
        //                /// end
        //                pLineSymbol.Color = pRGBColor;
        //                pLineSymbol.Width = 1.5;
        //                pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
        //                pFillSymbol.Outline = pLineSymbol;

        //                pFillSymbol.Color = pRGBColor;
        //                pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
        //                break;
        //        }


        //        pScreenDisplay.StartDrawing(pScreenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);  //esriScreenCache.esriNoScreenCache -1
        //        pScreenDisplay.SetSymbol(pSymbol);
        //        switch (pGeometry.GeometryType.ToString())
        //        {
        //            case "esriGeometryPoint"://点要素
        //                pScreenDisplay.DrawPoint(pGeometry);
        //                break;
        //            case "esriGeometryPolyline"://线要素
        //                pScreenDisplay.DrawPolyline(pGeometry);
        //                break;
        //            case "esriGeometryPolygon"://面要素
        //                pScreenDisplay.DrawPolygon(pGeometry);
        //                break;
        //        }
        //        pScreenDisplay.FinishDrawing();

        //    }
        //    catch
        //    { }
        //    finally
        //    {
        //        pSymbol = null;
        //        pRGBColor = null;
        //    }
        //}
        ///// <summary>
        ///// 设置RGB函数  
        ///// </summary>
        ///// <param name="r">Red</param>
        ///// <param name="g">Green</param>
        ///// <param name="b">Blue</param>
        ///// <returns></returns>
        //public static IRgbColor getRGB(int r, int g, int b)
        //{
        //    IRgbColor pRgbColor = new RgbColorClass();
        //    pRgbColor.Red = r;
        //    pRgbColor.Green = g;
        //    pRgbColor.Blue = b;
        //    return pRgbColor;
        //}

        //private void txtLayerName_Click(object sender, EventArgs e)
        //{
        //    this.advTreeLayers.Location = new System.Drawing.Point(this.comboLayers.Location.X,this.comboLayers.Location.Y);
        //    this.advTreeLayers.Width = this.comboLayers.Width;
        //    this.advTreeLayers.Height = this.Height - this.advTreeLayers.Location.Y;
        //    this.advTreeLayers.Visible = true;
        //    this.advTreeLayers.Focus();
        //    //IWorkspace pWok = m_WorkSpace;
        //    //Fan.Common.SelectLayerByTree frm = null;
        //    //if (_CurQueryType == enumQueryType.enumDomainQuery)
        //    //{
        //    //    frm = new Fan.Common.SelectLayerByTree(pWok,null,_ListDomainNodeKeys);
        //    //}
        //    //else
        //    //{
        //    //    frm = new Fan.Common.SelectLayerByTree(pWok, _ListDataNodeKeys);
        //    //}

        //    //if (frm.ShowDialog() == DialogResult.OK)
        //    //{
        //    //    if (frm.m_NodeKey == _DefaultNodeKey)
        //    //    {
        //    //        frm = null;
        //    //        return;
        //    //    }
        //    //    comboLayers .Text  = frm.m_NodeText;
        //    //    string LayerName = frm.m_NodeText;
        //    //    string strNodekey = frm.m_NodeKey;
        //    //    _DefaultNodeKey = frm.m_NodeKey;
        //    //    if (LayerName == "")
        //    //    {
        //    //        MessageBox.Show("未选择要查看的图层!请重试!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    //        frm = null;
        //    //        return;
        //    //    }
        //    //    else
        //    //    {
        //    //        //根据选择的图层来查找符合条件的要素
        //    //        if (_CurQueryType == enumQueryType.enumGeometryQuery)
        //    //        {
        //    //            InitializeGrid(GetLayerByNodeKey(ListLayer, strNodekey), m_Geometry, m_esriSpatialRelEnum);
        //    //        }
        //    //        else if (_CurQueryType == enumQueryType.enumSQLQuery)
        //    //        {
        //    //            InitializeSQLQueryGrid(m_Map,frm.m_NodeKey,_CurQueryFilter);
        //    //        }
        //    //        else if (_CurQueryType == enumQueryType.enumDomainQuery)
        //    //        {
        //    //            InitializeDomainQueryGrid(frm.m_NodeKey);
        //    //        }
        //    //        frm = null;

        //    //    }
        //    //    InitializeComBox();
        //    //}
        //}
        ////地名查询
        //public void EmergeQueryData(List<string> ListNodeKeys, List<ILayer> ListLayers, List<string> ListLayernames, List<IFeatureClass> ListFeatureClasses, IQueryFilter pQueryFilter, IQueryFilter pQueryFilterOrder1, IQueryFilter pQueryFilterOrder2, bool isHighLight)
        //{

        //    if (ListFeatureClasses == null)
        //    {
        //        return;
        //    }
        //    if (ListFeatureClasses.Count == 0)
        //        return;

        //    _ListDomainNodeKeys = ListNodeKeys;
        //    _ListDomainFeatureClasses = ListFeatureClasses;
        //    _ListDomainLayerNames = ListLayernames;
        //    _ListDomainListLayers = ListLayers;

        //    _CurQueryFilter = pQueryFilter;
        //    m_dataSourceGrid = null;

        //    IFeatureClass pFeatureClass = ListFeatureClasses[0];
        //    _DefaultNodeKey = ListNodeKeys[0];
        //    m_FeatureClass = pFeatureClass;
        //    ILayer pCurLayer = null;
        //    try
        //    {
        //        pCurLayer = ListLayers[0];
        //    }
        //    catch
        //    { }
        //    if (pCurLayer == null)
        //    {
        //        InitializeTable(pFeatureClass, "查询结果数据");
        //    }
        //    else
        //    {
        //        ILayerFields pLayerFields = pCurLayer as ILayerFields;
        //        InitializeTable(pFeatureClass, "查询结果数据",pLayerFields);
        //    }
        //    comboLayers.Text = ListLayernames[0];
        //    ISelectionSet pSelectSet = pFeatureClass.Select(pQueryFilter, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, (pFeatureClass as IDataset).Workspace);
        //    if (pSelectSet == null)
        //    {
        //        this.Refresh();
        //        return;
        //    }
        //    ICursor pCursor = null;
        //    pSelectSet.Search(null ,false ,out pCursor );
        //    IRow pRow = pCursor.NextRow();
        //    while (pRow != null)
        //    {
        //        InsterFeaToTable(pRow, m_dataSourceGrid);
        //        pRow = pCursor.NextRow();
        //    }
        //    dataGridViewX1.DataSource = m_dataSourceGrid;
        //    //改变datagridview列名显示
        //    for (int j = 0; j < dataGridViewX1.Columns.Count; j++)
        //    {
        //        if (dataGridViewX1.Columns[j].HeaderText.Contains("OBJECTID") || dataGridViewX1.Columns[j].HeaderText.Contains("SHAPE") || dataGridViewX1.Columns[j].HeaderText.Contains("shape"))
        //        {
        //            dataGridViewX1.Columns[j].Visible = false;
        //        }
        //        dataGridViewX1.Columns[j].HeaderText = m_dataSourceGrid.Columns[j].Caption;
        //    }
        //    _CurQueryType = enumQueryType.enumDomainQuery;
        //    InitLayersTree();
        //}
        //private void InsterFeaToTable(IRow pRow, DataTable Dtall)
        //{
        //    if (Dtall == null) return;
        //    DataRow vRow = Dtall.NewRow();
        //    //初始话字典表

        //    for (int i = 0; i < Dtall.Columns.Count; i++)
        //    {
        //        string strColumnName = Dtall.Columns[i].ColumnName;//中文字段名
        //        string strCaption = Dtall.Columns[i].Caption;//英文字段名
        //        int intIndex = pRow.Fields.FindField(strColumnName);

        //        if (intIndex < 0) continue;

        //        object obj = pRow.get_Value(intIndex);
        //        if (obj == null) continue;

        //        string strValue = obj.ToString();
        //        if (pRow.Fields.get_Field(intIndex).Type == esriFieldType.esriFieldTypeDouble)
        //        {
        //            double dblTempa = 0;
        //            double.TryParse(strValue, out dblTempa);
        //            strValue = string.Format("{0:f2}", dblTempa);
        //        }
        //        //added by chulili 20110818 统一对字段值进行中英文映射
        //        if (Fan.Common.ModField._DicMatchFieldValue.Keys.Contains(strCaption))
        //        {
        //            strValue = Fan.Common.ModField.GetChineseOfFieldValue(strCaption, strValue);
        //        }
        //        vRow[strColumnName] = strValue;
        //    }

        //    Dtall.Rows.Add(vRow);
        //}
        //private void btnSecondQuery_Click(object sender, EventArgs e)
        //{
        //    if (this.advTreeLayers.Visible)
        //    {
        //        this.advTreeLayers.Visible = false;
        //    }
        //    //SecondQuery FrmSecondeQuery = new SecondQuery();
        //    if (dataGridViewX1.CurrentRow == null)
        //    {
        //        return;
        //    }
        //    string strFieldOID = GetOIDfield(m_FeatureClass);
        //    string OID = dataGridViewX1.CurrentRow.Cells[strFieldOID].ToString();
        //    IFeature pFeature = GetFeatureByOID(FeaOID, m_FeatureClass);
        //    if (pFeature == null)
        //    {

        //        MessageBox.Show("未选择要查看的要素!", "错误",MessageBoxButtons .OK,MessageBoxIcon.Error );
        //        return;
        //    }
        //    //FrmSecondeQuery.m_pFeatrueClass = m_FeatureClass;
        //    //FrmSecondeQuery.m_pFeature = pFeature;
        //    //FrmSecondeQuery.Show();
        //}
        //#endregion

        //#region 查询数据导出 ygc 2012-8-15
        //private void btnExportToExcel_Click(object sender, EventArgs e)
        //{
        //    if (dataGridViewX1.Rows.Count == 0) return;
        //    DataGridViewToExcel(dataGridViewX1); 

        //}
        ///// <summary>
        ///// 以行的方式导出成EXCEL
        ///// ygc 2012-8-15
        ///// </summary>
        ///// <param name="dgv"></param>
        //public static void DataGridViewToExcel(DataGridView dgv)
        //{


        //    #region   验证可操作性

        //    //申明保存对话框   
        //    SaveFileDialog dlg = new SaveFileDialog();
        //    //默然文件后缀   
        //    dlg.DefaultExt = "xls ";
        //    //文件后缀列表   
        //    dlg.Filter = "EXCEL文件(*.XLS)|*.xls ";
        //    //默然路径是系统当前路径   
        //   // dlg.InitialDirectory = Directory.GetCurrentDirectory();
        //    //打开保存对话框   
        //    if (dlg.ShowDialog() == DialogResult.Cancel) return;
        //    //返回文件路径   
        //    string fileNameString = dlg.FileName;
        //    //验证strFileName是否为空或值无效   
        //    if (fileNameString.Trim() == " ")
        //    { return; }
        //    //定义表格内数据的行数和列数   
        //    int rowscount = dgv.Rows.Count;
        //    int colscount = dgv.Columns.Count;
        //    //行数必须大于0   
        //    if (rowscount <= 0)
        //    {
        //        MessageBox.Show("没有数据可供保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    //列数必须大于0   
        //    if (colscount <= 0)
        //    {
        //        MessageBox.Show("没有数据可供保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    //行数不可以大于65536   
        //    if (rowscount > 65536)
        //    {
        //        MessageBox.Show("数据记录数太多(最多不能超过65536条)，不能保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    //列数不可以大于255   
        //    if (colscount > 255)
        //    {
        //        MessageBox.Show("数据记录行数太多，不能保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    //验证以fileNameString命名的文件是否存在，如果存在删除它   
        //    FileInfo file = new FileInfo(fileNameString);
        //    if (file.Exists)
        //    {
        //        try
        //        {
        //            file.Delete();
        //        }
        //        catch (Exception error)
        //        {
        //            MessageBox.Show(error.Message, "删除失败 ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //    }
        //    #endregion
        //    Microsoft.Office.Interop.Excel.Application objExcel = null;
        //    Microsoft.Office.Interop.Excel.Workbook objWorkbook = null;
        //    Microsoft.Office.Interop.Excel.Worksheet objsheet = null;
        //    try
        //    {
        //        //申明对象   
        //        objExcel = new Microsoft.Office.Interop.Excel.Application();
        //        objWorkbook = objExcel.Workbooks.Add(System.Reflection.Missing.Value);
        //        objsheet = (Microsoft.Office.Interop.Excel.Worksheet)objWorkbook.ActiveSheet;
        //        //设置EXCEL不可见   
        //        objExcel.Visible = false;

        //        //向Excel中写入表格的表头   
        //        int displayColumnsCount = 1;
        //        for (int i = 0; i <= dgv.ColumnCount - 1; i++)
        //        {
        //            if (dgv.Columns[i].Visible == true)
        //            {
        //                objExcel.Cells[1, displayColumnsCount] = dgv.Columns[i].HeaderText.Trim();
        //                displayColumnsCount++;
        //            }
        //        }
        //        Fan.Common.CProgress vProgress = new Fan.Common.CProgress();
        //        //设置进度条   

        //        vProgress.ShowProgress();
        //        vProgress.ShowDescription = true;
        //        vProgress.ShowProgressNumber = true;
        //        vProgress.TopMost = true;
        //       // vProgress.EnableCancel = true;
        //        //vProgress.EnableUserCancel(true);
        //        vProgress.MaxValue  = dgv.RowCount;
        //        vProgress.Step = 1;
        //        vProgress.Caption = "正在导出数据......";
        //        //向Excel中逐行逐列写入表格中的数据   
        //        for (int row = 0; row <= dgv.RowCount - 1; row++)
        //        {
        //            vProgress.ProgresssValue = vProgress.ProgresssValue + 1;

        //            if (vProgress.UserAskCancel)
        //            {
        //                objExcel.Quit();
        //                GC.Collect();
        //                Kill(objExcel);
        //                vProgress.Close();
        //                return;
        //            }
        //            displayColumnsCount = 1;
        //            for (int col = 0; col < colscount; col++)
        //            {
        //                if (dgv.Columns[col].Visible == true)
        //                {
        //                    try
        //                    {

        //                        objExcel.Cells[row + 2, displayColumnsCount] = dgv.Rows[row].Cells[col].Value.ToString().Trim();
        //                        displayColumnsCount++;
        //                    }
        //                    catch (Exception)
        //                    {

        //                    }

        //                }
        //            }
        //        }
        //        //隐藏进度条   
        //        vProgress.Close(); 
        //        //保存文件   
        //        objWorkbook.SaveAs(fileNameString, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
        //                Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, Missing.Value, Missing.Value, Missing.Value,
        //                Missing.Value, Missing.Value);
        //    }
        //    catch (Exception error)
        //    {
        //        MessageBox.Show(error.Message, "警告 ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }
        //    Kill(objExcel);
        //    MessageBox.Show(fileNameString + "\n\n导出完毕! ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //}
        //public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        //{
        //    //IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口 

        //    int k = 0;
        //    GetWindowThreadProcessId(new IntPtr(excel.Hwnd), out k);   //得到本进程唯一标志k
        //    System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用
        //    p.Kill();     //关闭进程k
        //}
        //[DllImport("User32.dll", CharSet = CharSet.Auto)]
        //public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        //#endregion

        //#region 统计查询数据 ygc2012-8-16
        ////初始化下拉框
        ////ygc 2012-8-16
        //private void InitializeComBox()
        //{
        //    if (m_FeatureClass ==null)
        //    {
        //        MessageBox.Show("无查询结果数据！","提示");
        //        return;
        //    }
        //    //清空已有元素--ygc 2012-9-4
        //    cbClassifyField.Items.Clear(); ;
        //    cbStatisticsField.Items.Clear();
        //    cbClassifyField.Text = "";
        //    cbStatisticsField.Text = "";

        //    IFields pFields = m_FeatureClass.Fields;
        //    for (int i = 0; i < pFields .FieldCount ; i++)
        //    {
        //        if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeDouble || pFields.get_Field(i).Type == esriFieldType.esriFieldTypeSingle || pFields.get_Field(i).Type==esriFieldType .esriFieldTypeInteger)
        //        {
        //            cbStatisticsField.Items.Add(Fan.Common.ModField.GetChineseNameOfField(pFields.get_Field(i).Name.ToString()));
        //        }
        //        cbClassifyField.Items.Add(Fan.Common.ModField.GetChineseNameOfField(pFields.get_Field(i).Name.ToString ()));
        //    }
        //    if (cbStatisticsField.Items.Count > 0)
        //    {
        //        cbStatisticsField.SelectedIndex = 0;
        //    }
        //    if(cbClassifyField .Items .Count >0)
        //    {
        //        cbClassifyField.SelectedIndex = 0;
        //    }
        //}
        //private void btnStatistics_Click(object sender, EventArgs e)
        //{
        //    if (this.cbStatisticsField.SelectedIndex < 0 || this.cbClassifyField.SelectedIndex < 0)
        //    {
        //        MessageBox.Show("请先指定分类和统计的字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }
        //    if (dataGridViewX1.Rows.Count ==0) return;
        //    string strStatictisValue = "";
        //    string strTitle = "";
        //    string strColumnTitle = "";
        //    ArrayList XLable = new ArrayList();
        //    ArrayList ColorGuider = new ArrayList();
        //    ArrayList[] CharData = { new ArrayList() };

        //    try
        //    {
        //        strStatictisValue = cbStatisticsField.SelectedItem.ToString();
        //        strTitle = strStatictisValue + "统计图";
        //        strColumnTitle = strStatictisValue;
        //        for (int i = 0; i < dataGridViewX1.Columns.Count; i++)
        //        {
        //            if (strStatictisValue == dataGridViewX1.Columns[i].HeaderText)
        //            {

        //                strStatictisValue = dataGridViewX1.Columns[i].Name.ToString();
        //                break;
        //            }
        //        }

        //        string strWhere = cbClassifyField.SelectedItem.ToString();
        //        for (int i = 0; i < this.dataGridViewX1.Columns.Count; i++)
        //        {
        //            string strTemp = this.dataGridViewX1.Columns[i].HeaderText;
        //            if (strTemp == strWhere)
        //            {
        //                strWhere = this.dataGridViewX1.Columns[i].DataPropertyName;
        //                break;
        //            }
        //        }

        //        //
        //        SortedDictionary<string, double> values = new SortedDictionary<string, double>();
        //        string strValue = strStatictisValue;
        //        DataTable dt = this.dataGridViewX1.DataSource as DataTable;
        //        int count = 0;
        //        if (dt.Columns.Contains("统计长度") || dt.Columns.Contains("统计面积"))//如果是线和面则不取最后一行
        //        {
        //            count = dt.Rows.Count - 1;
        //        }
        //        else//如果是点，没有增加最后一行则取最后一行
        //        {
        //            count = dt.Rows.Count;
        //        }
        //        for (int i = 0; i < count; i++)
        //        {
        //            string strKey = "";
        //            object obj = dt.Rows[i][strWhere];
        //            if (obj == null)
        //            {
        //                strKey = "其他";
        //            }
        //            else
        //            {
        //                strKey = obj.ToString();
        //            }
        //            if (strKey == "") strKey = "其他";

        //            //值
        //            string strTempValue = "";
        //            object objvalue = dt.Rows[i][strValue];
        //            if (objvalue == null)
        //            {
        //                strTempValue = "0";
        //            }
        //            else
        //            {
        //                strTempValue = objvalue.ToString();
        //            }


        //            double dblSubArea = 0;
        //            if (!double.TryParse(strTempValue, out dblSubArea)) dblSubArea = 0;

        //            if (values.ContainsKey(strKey))
        //            {
        //                double dblArea = 0;
        //                values.TryGetValue(strKey, out dblArea);
        //                dblArea = dblArea + dblSubArea;
        //                values.Remove(strKey);
        //                values.Add(strKey, dblArea);
        //            }
        //            else
        //            {
        //                values.Add(strKey, dblSubArea);
        //            }

        //        }
        //        ////获取统计数据 ZQ  20111014 add 
        //        foreach (string Keys in values.Keys)
        //        {
        //            XLable.Add(Keys);
        //            CharData[0].Add(values[Keys]);
        //        }
        //        //Fan.Plugin.LogTable.Writelog("出统计图,分类字段为" + this.comboType.Text + ",统计的是" + strColumnTitle);
        //        ////更换统计图显示的控件    ZQ 20111014  Modify
        //        FrmResult pfrm = new FrmResult();
        //        pfrm.strTitle = strTitle;
        //        pfrm.strXLabels = cbClassifyField.SelectedItem.ToString();
        //        pfrm.strYLabels = strColumnTitle;
        //        pfrm.ArrXLable = XLable;
        //        pfrm.ArrCharData = CharData;
        //        pfrm.ArrColorGuider = ColorGuider;
        //        pfrm.ShowDialog();
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString ());
        //    }
        //}
        //#endregion

        //private void btnQuerSecond_Click(object sender, EventArgs e)
        //{
        //    if (this.advTreeLayers.Visible)
        //    {
        //        this.advTreeLayers.Visible = false;
        //    }
        //    if ( m_FeatureClass == null)
        //    {
        //        return;
        //    }
        //    SQLSecondfrm newfrm = new SQLSecondfrm(m_FeatureClass);
        //    newfrm.m_pWorkspace = m_WorkSpace;
        //    string Where="";
        //    if (newfrm.ShowDialog() == DialogResult.OK)
        //    {
        //        Where = newfrm.SQL;
        //    }
        //    else
        //    {
        //        return;
        //    }
        //    IQueryFilter pFilter=new QueryFilterClass ();
        //    pFilter .WhereClause =Where ;
        //    IFeatureCursor pFeatureCursor = m_FeatureClass.Search(pFilter, false);
        //    //获取当前datagrideview中数据的OID
        //    IList<string> listOID = new List<string>();
        //    string strFieldOID = GetOIDfield(m_FeatureClass);
        //    for (int i = 0; i < dataGridViewX1.Rows.Count; i++)
        //    {
        //        listOID.Add(dataGridViewX1.Rows[i].Cells[strFieldOID].Value.ToString());
        //    }
        //    //
        //  //  DataTable tempDt = (DataTable)dataGridViewX1.DataSource;
        //    //
        //    IFeature pFeature = pFeatureCursor.NextFeature();
        //    dataGridViewX1.DataSource =null ;
        //    m_dataSourceGrid = null;
        //    InitializeTable(m_FeatureClass,"二次查询数据");
        //    //ygc 20130416 获取字典
        //    Dictionary<string, Dictionary<string, string>> ChineseDic = ModXZQ.InitialDic(m_WorkSpace);

        //    DataRow pRow = null; 
        //    while (pFeature != null)
        //    {
        //        int fieldIndex=pFeature .Fields .FindField (strFieldOID);
        //        string FOID = pFeature.get_Value(fieldIndex).ToString();
        //        if (listOID.Contains(FOID))
        //        {
        //            pRow = m_dataSourceGrid.NewRow();
        //            for (int i = 0; i < pFeature.Fields.FieldCount; i++)
        //            {
        //                string Values = pFeature.get_Value(i).ToString();
        //                string fieldName = pFeature.Fields.get_Field(i).Name;

        //                //Values = Fan.Common.ModField.GetDomainValueOfFieldValue(m_FeatureClass, fieldName, Values);
        //                if (ChineseDic.ContainsKey(fieldName))
        //                {
        //                    Dictionary<string, string> tempDic = ChineseDic[fieldName];
        //                    if (fieldName == "SHENG" && Values.Length > 2) Values = Values.Substring(0, 2);
        //                    else if (fieldName == "SHI" && Values.Length > 4) Values = Values.Substring(0, 4);
        //                    else if (fieldName == "XIANG" && Values.Length > 8) Values = Values.Substring(0, 8);
        //                    else if (fieldName == "CUN" && Values.Length > 10) Values = Values.Substring(0, 10);
        //                    if (tempDic.ContainsKey(Values))
        //                    {
        //                        Values = tempDic[Values];
        //                    }
        //                }
        //                try
        //                {
        //                    if (fieldName != "Shape" && fieldName != "SHAPE")
        //                    {
        //                        try
        //                        {
        //                            pRow[fieldName] = Values;
        //                        }
        //                        catch
        //                        { }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.ToString());
        //                }
        //            }
        //            m_dataSourceGrid.Rows.Add(pRow);
        //        }
        //        pFeature = pFeatureCursor.NextFeature();
        //    }

        //    dataGridViewX1.DataSource = m_dataSourceGrid;
        //    //改变datagridview列名显示
        //    for (int j = 0; j < dataGridViewX1.Columns.Count; j++)
        //    {
        //        if (dataGridViewX1.Columns[j].HeaderText.Contains("OBJECTID") || dataGridViewX1.Columns[j].HeaderText.Contains("SHAPE") || dataGridViewX1.Columns[j].HeaderText.Contains("shape"))
        //        {
        //            dataGridViewX1.Columns[j].Visible = false;
        //        }
        //        dataGridViewX1.Columns[j].HeaderText = m_dataSourceGrid.Columns[j].Caption;
        //    }
        //}

        //private void BottomQueryBar_Load(object sender, EventArgs e)
        //{
        //    comboLayers.ItemHeight = 21;
        //    comboLayers.Height = 21;
        //    //初始化图层树列表
        //}
        //private void InitLayersTree()
        //{

        //    ModSysSetting.CopyLayerTreeXmlFromDataBase(m_WorkSpace, LayerTreexmlpath);
        //    if (File.Exists(LayerTreexmlpath))
        //    {

        //        XmlDocument LayerTreeXmldoc = new XmlDocument();

        //        LayerTreeXmldoc.Load(LayerTreexmlpath);
        //        advTreeLayers.Nodes.Clear();

        //        //获取Xml的根节点并作为根节点加到UltraTree上
        //        XmlNode xmlnodeRoot = LayerTreeXmldoc.DocumentElement;
        //        XmlElement xmlelementRoot = xmlnodeRoot as XmlElement;

        //        xmlelementRoot.SetAttribute("NodeKey", "Root");
        //        string sNodeText = xmlelementRoot.GetAttribute("NodeText");

        //        //创建并设定树的根节点
        //        DevComponents.AdvTree.Node treenodeRoot = new DevComponents.AdvTree.Node();
        //        treenodeRoot.Name = "Root";
        //        treenodeRoot.Text = sNodeText;

        //        treenodeRoot.Tag = "Root";
        //        treenodeRoot.DataKey = xmlelementRoot;
        //        treenodeRoot.Expanded = true;
        //        this.advTreeLayers.Nodes.Add(treenodeRoot);

        //        treenodeRoot.Image = this.ImageList.Images["Root"];
        //        InitLayerTreeByXmlNode(treenodeRoot, xmlnodeRoot);
        //        LayerTreeXmldoc = null;
        //    }
        //}
        ////根据配置文件显示图层树
        //private void InitLayerTreeByXmlNode(DevComponents.AdvTree.Node treenode, XmlNode xmlnode)
        //{

        //    for (int iChildIndex = 0; iChildIndex < xmlnode.ChildNodes.Count; iChildIndex++)
        //    {
        //        XmlElement xmlElementChild = xmlnode.ChildNodes[iChildIndex] as XmlElement;
        //        if (xmlElementChild == null)
        //        {
        //            continue;
        //        }
        //        else if (xmlElementChild.Name == "ConfigInfo")
        //        {
        //            continue;
        //        }
        //        //用Xml子节点的"NodeKey"和"NodeText"属性来构造树子节点
        //        string sNodeKey = xmlElementChild.GetAttribute("NodeKey");
        //        if (_ListDataNodeKeys != null)
        //        {
        //            if (!_ListDataNodeKeys.Contains(sNodeKey))
        //            {
        //                continue;
        //            }
        //        }
        //        string sNodeText = xmlElementChild.GetAttribute("NodeText");

        //        DevComponents.AdvTree.Node treenodeChild = new DevComponents.AdvTree.Node();
        //        treenodeChild.Name = sNodeKey;
        //        treenodeChild.Text = sNodeText;

        //        treenodeChild.DataKey = xmlElementChild;
        //        treenodeChild.Tag = xmlElementChild.Name;


        //        treenode.Nodes.Add(treenodeChild);

        //        //递归
        //        if (xmlElementChild.Name != "Layer")
        //        {
        //            InitLayerTreeByXmlNode(treenodeChild, xmlElementChild as XmlNode);
        //        }

        //        InitializeNodeImage(treenodeChild);
        //    }

        //}
        ///// <summary>
        ///// 通过传入节点的tag，选择对应的图标        
        ///// </summary>
        ///// <param name="treenode"></param>
        //private void InitializeNodeImage(DevComponents.AdvTree.Node treenode)
        //{
        //    switch (treenode.Tag.ToString())
        //    {
        //        case "Root":
        //            treenode.Image = this.ImageList.Images["Root"];
        //            treenode.CheckBoxVisible = false;
        //            break;
        //        case "SDE":
        //            treenode.Image = this.ImageList.Images["SDE"];
        //            break;
        //        case "PDB":
        //            treenode.Image = this.ImageList.Images["PDB"];
        //            break;
        //        case "FD":
        //            treenode.Image = this.ImageList.Images["FD"];
        //            break;
        //        case "FC":
        //            treenode.Image = this.ImageList.Images["FC"];
        //            break;
        //        case "TA":
        //            treenode.Image = this.ImageList.Images["TA"];
        //            break;
        //        case "DIR":
        //            treenode.Image = this.ImageList.Images["DIR"];
        //            //treenode.CheckBoxVisible = false;
        //            break;
        //        case "DataDIR":
        //            treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
        //            break;
        //        case "DataDIR&AllOpened":
        //            treenode.Image = this.ImageList.Images["DataDIROpen"];
        //            break;
        //        case "DataDIR&Closed":
        //            treenode.Image = this.ImageList.Images["DataDIRClosed"];
        //            break;
        //        case "DataDIR&HalfOpened":
        //            treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
        //            break;
        //        case "Layer":
        //            XmlNode xmlnodeChild = (XmlNode)treenode.DataKey;
        //            if (xmlnodeChild != null && xmlnodeChild.Attributes["FeatureType"] != null)
        //            {
        //                string strFeatureType = xmlnodeChild.Attributes["FeatureType"].Value;

        //                switch (strFeatureType)
        //                {
        //                    case "esriGeometryPoint":
        //                        treenode.Image = this.ImageList.Images["_point"];
        //                        break;
        //                    case "esriGeometryPolyline":
        //                        treenode.Image = this.ImageList.Images["_line"];
        //                        break;
        //                    case "esriGeometryPolygon":
        //                        treenode.Image = this.ImageList.Images["_polygon"];
        //                        break;
        //                    case "esriFTAnnotation":
        //                        treenode.Image = this.ImageList.Images["_annotation"];
        //                        break;
        //                    case "esriFTDimension":
        //                        treenode.Image = this.ImageList.Images["_Dimension"];
        //                        break;
        //                    case "esriGeometryMultiPatch":
        //                        treenode.Image = this.ImageList.Images["_MultiPatch"];
        //                        break;
        //                    default:
        //                        treenode.Image = this.ImageList.Images["Layer"];
        //                        break;
        //                }
        //            }
        //            else
        //            {
        //                treenode.Image = this.ImageList.Images["Layer"];
        //            }
        //            break;
        //        case "RC":
        //            treenode.Image = this.ImageList.Images["RC"];
        //            break;
        //        case "RD":
        //            treenode.Image = this.ImageList.Images["RD"];
        //            break;
        //        case "SubType":
        //            treenode.Image = this.ImageList.Images["SubType"];
        //            break;
        //        default:
        //            break;
        //    }//end switch
        //}
        //private void advTreeLayers_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        //{
        //    DealSelectNodeEX();
        //}
        //private void DealSelectNodeEX()
        //{
        //    if (this.advTreeLayers.SelectedNode == null)
        //        return;
        //    if (advTreeLayers.SelectedNode.Tag.ToString() != "Layer")//不是叶子节点 返回
        //    {
        //        return;
        //    }

        //    string LayerID = GetNodeKey(advTreeLayers.SelectedNode);
        //    comboLayers.Text = advTreeLayers.SelectedNode.Text;
        //    string LayerName = advTreeLayers.SelectedNode.Text;
        //    string strNodekey = LayerID;
        //    _DefaultNodeKey = LayerID;
        //    if (LayerName == "")
        //    {
        //        MessageBox.Show("未选择要查看的图层!请重试!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }
        //    else
        //    {
        //        this.advTreeLayers.Visible = false;
        //        //根据选择的图层来查找符合条件的要素
        //        if (_CurQueryType == enumQueryType.enumGeometryQuery)
        //        {
        //            InitializeGrid(GetLayerByNodeKey(ListLayer, strNodekey), m_Geometry, m_esriSpatialRelEnum);
        //        }
        //        else if (_CurQueryType == enumQueryType.enumSQLQuery)
        //        {
        //            InitializeSQLQueryGrid(m_Map, LayerID, _CurQueryFilter);
        //        }
        //        else if (_CurQueryType == enumQueryType.enumDomainQuery)
        //        {
        //            InitializeDomainQueryGrid(LayerID);
        //        }

        //    }
        //    InitializeComBox();
        //}
        ////通过NODE 得到NODYKEY
        //private string GetNodeKey(DevComponents.AdvTree.Node Node)
        //{
        //    // labelErr.Text = "";
        //    XmlNode xmlnode = (XmlNode)Node.DataKey;
        //    XmlElement xmlelement = xmlnode as XmlElement;
        //    string strDataType = "";
        //    if (xmlelement.HasAttribute("DataType"))
        //    {
        //        strDataType = xmlnode.Attributes["DataType"].Value;
        //    }
        //    if (strDataType == "RD" || strDataType == "RC")//是影像数据 返回
        //    {
        //        // labelErr.Text = "请选择矢量数据进行操作!";
        //        return "";
        //    }
        //    if (xmlelement.HasAttribute("IsQuery"))
        //    {
        //        if (xmlelement["IsQuery"].Value == "False")
        //        {
        //            // labelErr.Text = "该图层不可查询!";
        //            return "";
        //        }
        //    }
        //    if (xmlelement.HasAttribute("NodeKey"))
        //    {
        //        return xmlelement.GetAttribute("NodeKey");

        //    }
        //    return "";

        //}

        //private void advTreeLayers_Leave(object sender, EventArgs e)
        //{
        //    this.advTreeLayers.Visible = false;
        //}

        //private void BottomQueryBar_Click(object sender, EventArgs e)
        //{
        //    if (this.advTreeLayers.Visible)
        //    {
        //        this.advTreeLayers.Visible = false;
        //    }
        //}

        //private void labelItem1_Click(object sender, EventArgs e)
        //{
        //    if (this.advTreeLayers.Visible)
        //    {
        //        this.advTreeLayers.Visible = false;
        //    }
        //}

        //private void labelItem2_Click(object sender, EventArgs e)
        //{
        //    if (this.advTreeLayers.Visible)
        //    {
        //        this.advTreeLayers.Visible = false;
        //    }
        //}

        //private void barBottom_Click(object sender, EventArgs e)
        //{
        //    if (this.advTreeLayers.Visible)
        //    {
        //        this.advTreeLayers.Visible = false;
        //    }
        //}
        ////ygc 20130416 打开字典表
        //private IWorkspace GetDicWorkspace(string gdbPath)
        //{
        //    IWorkspace pWorkspace = null;
        //    IWorkspaceFactory pGDBWorkSpace = new FileGDBWorkspaceFactoryClass();
        //    try
        //    {
        //        pWorkspace = pGDBWorkSpace.OpenFromFile(gdbPath, 0);
        //    }
        //    catch
        //    {

        //    }
        //    return pWorkspace;
        //}
    }
}
