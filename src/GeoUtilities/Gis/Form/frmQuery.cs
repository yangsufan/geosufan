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

namespace GeoUtilities
{
    public enum enumQueryMode
    {
        Top=0,
        Visiable=1,
        Selectable=2,
        All=3
    }

    public partial class frmQuery : DevComponents.DotNetBar.Office2007Form
    {
        private DataTable m_dataSourceGrid;        //grid的datasource
        private IMapControlDefault m_pMapControl;                        //被查询的地图对象
        private enumQueryMode m_enumQueryMode;     //查询方式
        public enumQueryMode QueryMode
        {
            get
            {
                return m_enumQueryMode;
            }
        }

        /// <summary>
        /// 初始化，用来SQL查询
        /// </summary>
        /// <param name="pMapControl"></param>
        public frmQuery(IMapControlDefault pMapControl)
        {
            InitializeComponent();
            labelItem.Visible = false;
            comboBoxItem.Visible = false;

            InitializeGrid();      //初始化Grid的表现  
            m_pMapControl = pMapControl;
        }
        /// <summary>
        /// 适用于一般查询,缓冲查询,选择查询等一般查询
        /// </summary>
        /// <param name="pMapControl">被查询的地图对象</param>
        /// <param name="penumQueryMode">查询方式</param>
        public frmQuery(IMapControlDefault pMapControl, enumQueryMode penumQueryMode)
        {
            InitializeComponent();
            labelItem.Visible = true;
            comboBoxItem.Visible = true;

            InitializeGrid();      //初始化Grid的表现  
            m_pMapControl = pMapControl;
            m_enumQueryMode = penumQueryMode;
            switch(penumQueryMode)
            {
                case enumQueryMode.Top:
                    comboBoxItem.SelectedIndex = 0;
                    break;
                case enumQueryMode.Visiable:
                    comboBoxItem.SelectedIndex = 1;
                    break;
                case enumQueryMode.Selectable:
                    comboBoxItem.SelectedIndex = 2;
                    break;
                case enumQueryMode.All:
                    comboBoxItem.SelectedIndex = 3;
                    break;
            }
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
                case 3:
                    m_enumQueryMode = enumQueryMode.All;
                    break;
            }
        }

        /// <summary>
        /// 根据范围查找数据填充窗体
        /// </summary>
        /// <param name="pMap">被查询的地图对象</param>
        /// <param name="pGeometry">查询范围</param>
        public void FillData(IMap pMap,IGeometry pGeometry)
        {
            advTree.Nodes.Clear();
            m_dataSourceGrid.Clear();

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
                case enumQueryMode.Top:
                case enumQueryMode.Visiable:
                    pEnumLayer.Reset();
                    pLayer = pEnumLayer.Next();
                    while(pLayer!=null)
                    {
                        if (pLayer.Visible == true)
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
                        if (pTemp.Selectable == true)
                        {
                            listLays.Add(pLayer);
                        }

                        pLayer = pEnumLayer.Next();
                    }
                    break;
                case enumQueryMode.All:
                    pEnumLayer.Reset();
                    pLayer = pEnumLayer.Next();
                    while(pLayer!=null)
                    {
                       listLays.Add(pLayer);
                       pLayer = pEnumLayer.Next();
                    }
                    break;
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
                ISpatialFilter pSpatialFilter=new SpatialFilterClass();
                pSpatialFilter.Geometry=pGeometry;
                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel=esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureLayerDefinition pLayerDef = pFeatLay as IFeatureLayerDefinition;
                if (pLayerDef.DefinitionExpression != "")
                {
                    pSpatialFilter.WhereClause = pLayerDef.DefinitionExpression;
                }
                if (pFeatLay.FeatureClass.FeatureCount(pSpatialFilter) == 0) continue;

                //添加图层节点
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Text = pLay.Name;
                node.Tag = pLay;
                node.Expand();
                advTree.Nodes.Add(node);

                IFeatureCursor pFeatureCursor = pFeatLay.FeatureClass.Search(pSpatialFilter, false);
                IFeature pFeat = pFeatureCursor.NextFeature();
                while (pFeat != null&&intCnt<=100)//ZQ  201129  modify
                {
                    //添加要素节点
                    DevComponents.AdvTree.Node featnode = new DevComponents.AdvTree.Node();
                    featnode.Text = pFeat.OID.ToString();
                    featnode.Tag = pFeat;
                    node.Nodes.Add(featnode);

                    if (bFirst == true)
                    {
                        advTree.SelectedNode = featnode;
                        m_dataSourceGrid.Clear();

                        for (int i = 0; i < pFeat.Fields.FieldCount; i++)
                        {
                            if (pFeat.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                            {
                                m_dataSourceGrid.Rows.Add(new object[] { pFeat.Fields.get_Field(i).Name, pFeat.Shape.GeometryType.ToString() });
                            }
                            else
                            {
                                m_dataSourceGrid.Rows.Add(new object[] { pFeat.Fields.get_Field(i).Name, pFeat.get_Value(i) });
                            }
                        }
                        bFirst = false;
                    }

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

        /// <summary>
        /// 根据条件查找数据填充窗体，用来进行SQL查询
        /// </summary>
        /// <param name="pMap">被查询的地图对象</param>
        public void FillData(IFeatureLayer pFeatLay, IQueryFilter pQueryFilter, esriSelectionResultEnum pSelectionResult)
        {

            advTree.Nodes.Clear();
            m_dataSourceGrid.Clear();

            //获取查询的图层
            progressBarItem.Visible = false ;
            this.Refresh();
            int intCnt = 0;//记录要素的个数
            bool bFirst = true;
            
            //添加图层节点
            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
            node.Text = pFeatLay.Name;
            node.Tag = pFeatLay;
            node.Expand();
            advTree.Nodes.Add(node);
            //进行查询操作
            IFeatureSelection pFeatureSelection = pFeatLay as IFeatureSelection;
            pFeatureSelection.SelectFeatures(pQueryFilter, pSelectionResult, false);
            //保存查询的结果，包括要素类和查找到的要素
            //循环取得选中Feature的ID来得到Feature
            IFeature pFeature;
            IEnumIDs pEnumIDs = pFeatureSelection.SelectionSet.IDs;
            int iIDIndex = pEnumIDs.Next();
            while (iIDIndex != -1&&intCnt<=100)//ZQ  20110729 modify
            {
                pFeature = pFeatLay.FeatureClass.GetFeature(iIDIndex);
                //添加要素节点
                DevComponents.AdvTree.Node featnode = new DevComponents.AdvTree.Node();
                featnode.Text = pFeature.OID.ToString();
                featnode.Tag = pFeature;
                node.Nodes.Add(featnode);

                if (bFirst == true)
                {
                    advTree.SelectedNode = featnode;
                    m_dataSourceGrid.Clear();

                    for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                    {
                        if (pFeature.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            m_dataSourceGrid.Rows.Add(new object[] { pFeature.Fields.get_Field(i).Name, pFeature.Shape.GeometryType.ToString() });
                        }
                        else
                        {
                            m_dataSourceGrid.Rows.Add(new object[] { pFeature.Fields.get_Field(i).Name, pFeature.get_Value(i) });
                        }
                    }
                    bFirst = false;
                }
                iIDIndex = pEnumIDs.Next();
                intCnt++;
            }

            labelItemMemo.Text = "查找到" + intCnt.ToString() + "个要素";
            pFeatureSelection.Clear();
            this.Refresh();
            DefaultSelNde();
        }
       
        /// <summary>
        /// 多个要素同时闪烁 add by xisheng 2011.07.01
        /// </summary>
        private void DefaultSelNde()
        {
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
        private void advTree_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            IFeature pfeature = advTree.SelectedNode.Tag as IFeature;
            if (pfeature != null)
            {
                m_dataSourceGrid.Clear();

                for (int i = 0; i < pfeature.Fields.FieldCount; i++)
                {
                    if (pfeature.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        m_dataSourceGrid.Rows.Add(new object[] { pfeature.Fields.get_Field(i).Name, pfeature.Shape.GeometryType.ToString() });
                    }
                    else
                    {
                        m_dataSourceGrid.Rows.Add(new object[] { pfeature.Fields.get_Field(i).Name, pfeature.get_Value(i) });
                    }
                }

                FlashFeature(pfeature, m_pMapControl);
            }
        }

        private void advTree_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            IFeature pfeature = advTree.SelectedNode.Tag as IFeature;
            if (pfeature != null)
            {
                SysCommon.Gis.ModGisPub.ZoomToFeature(m_pMapControl, pfeature);
                Application.DoEvents();
                m_pMapControl.FlashShape(pfeature.Shape, 2, 500, null);
            }
        }

        #region  实现闪烁效果的代码
        //闪烁要素
        public void FlashFeature(IFeature pFeature, IActiveView pActiveView)
        {
            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing(pScreenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            int interval = 150;
            if (pFeature.Shape == null) return;
            switch (pFeature.Shape.GeometryType)
            {
                case esriGeometryType.esriGeometryPolyline:
                    FlashLine(pActiveView, pFeature.Shape, interval);
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    FlashPolygon(pActiveView, pFeature.Shape, interval);
                    break;
                case esriGeometryType.esriGeometryPoint:
                    FlashPoint(pActiveView, pFeature.Shape, interval);
                    break;
                default:
                    break;
            }
            pScreenDisplay.FinishDrawing();
        }

        //闪烁要素
        public void FlashFeature(IGeometry pGeometry, IActiveView pActiveView, int interval)
        {
            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing(pScreenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            if (pGeometry == null) return;
            switch (pGeometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPolyline:
                case esriGeometryType.esriGeometryLine:
                    FlashLine(pActiveView, pGeometry, interval);
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    FlashPolygon(pActiveView, pGeometry, interval);
                    break;
                case esriGeometryType.esriGeometryPoint:
                    FlashPoint(pActiveView, pGeometry, interval);
                    break;
                default:
                    break;
            }
            pScreenDisplay.FinishDrawing();
        }

        public void FlashFeature(IFeature pFeature, IMapControlDefault pMapControl)
        {
            IActiveView pActiveView = pMapControl.ActiveView;

            IEnvelope pEnvSmall = pFeature.Shape.Envelope;
            IEnvelope pEnvScreen = pActiveView.Extent;

            IPoint pPointCenter = new PointClass();
            pPointCenter.PutCoords((pEnvSmall.XMax + pEnvSmall.XMin) / 2, (pEnvSmall.YMax + pEnvSmall.YMin) / 2);

            IPoint pPointXMax = new PointClass();
            pPointXMax.PutCoords(pEnvScreen.XMax, pPointCenter.Y);


            IPoint pPointXMaxSmall = new PointClass();
            pPointXMaxSmall.PutCoords(pEnvSmall.XMax, pPointCenter.Y);


            IPoint pPointXMin = new PointClass();
            pPointXMin.PutCoords(pEnvScreen.XMin, pPointCenter.Y);
            IPoint pPointXMinSmall = new PointClass();
            pPointXMinSmall.PutCoords(pEnvSmall.XMin, pPointCenter.Y);

            IPoint pPointYMax = new PointClass();
            pPointYMax.PutCoords(pPointCenter.X, pEnvScreen.YMax);
            IPoint pPointYMaxSmall = new PointClass();
            pPointYMaxSmall.PutCoords(pPointCenter.Y, pEnvSmall.YMax);

            IPoint pPointYMin = new PointClass();
            pPointYMin.PutCoords(pPointCenter.X, pEnvScreen.YMin);
            IPoint pPointYMinSmall = new PointClass();
            pPointYMinSmall.PutCoords(pPointCenter.Y, pEnvSmall.YMin);

            for (int i = 0; i < 13; i++)
            {
                IPolyline pLineXMax = new PolylineClass();
                pLineXMax.FromPoint = pPointXMax;
                IPoint pPointTo = new PointClass();
                double xMax = pPointXMax.X + Convert.ToInt32((pPointXMaxSmall.X - pPointXMax.X) * i / 13);

                pPointTo.PutCoords(xMax, pPointCenter.Y);
                pLineXMax.ToPoint = pPointTo;

                IPolyline pLineXMin = new PolylineClass();
                pLineXMin.FromPoint = pPointXMin;
                double xMin = pPointXMin.X + (pPointXMinSmall.X - pPointXMin.X) * i / 13;

                pPointTo.PutCoords(xMin, pPointCenter.Y);
                pLineXMin.ToPoint = pPointTo;

                IPolyline pLineYMin = new PolylineClass();
                pLineYMin.FromPoint = pPointYMin;
                double yMin = pPointYMin.Y + (pPointYMinSmall.Y - pPointYMin.Y) * i / 13;
                pPointTo.PutCoords(pPointYMin.X, yMin);
                pLineYMin.ToPoint = pPointTo;

                IPolyline pLineYMax = new PolylineClass();
                pLineYMax.FromPoint = pPointYMax;
                double yMax = pPointYMax.Y + (pPointYMaxSmall.Y - pPointYMax.Y) * i / 13;
                pPointTo.PutCoords(pPointYMax.X, yMax);
                pLineYMax.ToPoint = pPointTo;

                IGeometryCollection pGeoColl = new PolylineClass();
                object obj = Type.Missing;

                IPath pPath = new PathClass();
                pPath.FromPoint = pLineXMax.FromPoint;
                pPath.ToPoint = pLineXMax.ToPoint;
                pGeoColl.AddGeometry(pPath, ref obj, ref obj);

                pPath = new PathClass();
                pPath.FromPoint = pLineXMin.FromPoint;
                pPath.ToPoint = pLineXMin.ToPoint;
                pGeoColl.AddGeometry(pPath, ref obj, ref obj);

                pPath = new PathClass();
                pPath.FromPoint = pLineYMax.FromPoint;
                pPath.ToPoint = pLineYMax.ToPoint;
                pGeoColl.AddGeometry(pPath, ref obj, ref obj);

                pPath = new PathClass();
                pPath.FromPoint = pLineYMin.FromPoint;
                pPath.ToPoint = pLineYMin.ToPoint;
                pGeoColl.AddGeometry(pPath, ref obj, ref obj);
                
                FlashFeature(pGeoColl as IGeometry, pActiveView, 5);
            }

            pMapControl.FlashShape(pFeature.Shape, 2, 500, null);
        }

        //闪烁线
        private void FlashLine(IActiveView pActiveView, IGeometry pGeometry, int interval)
        {
            IScreenDisplay pDisplay = pActiveView.ScreenDisplay;
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            ISymbol pSymbol;
            IRgbColor pRGBColor;

            tagPOINT tagPOINT = new tagPOINT();
            WKSPoint WKSPoint = new WKSPoint();

            tagPOINT.x = (int)1;
            tagPOINT.y = (int)1;
            pDisplay.DisplayTransformation.TransformCoords(ref WKSPoint, ref tagPOINT, 1, 6);
            
            pLineSymbol = new SimpleLineSymbolClass();
            if(pActiveView.FocusMap.ReferenceScale!=0)
            {
                pLineSymbol.Width = WKSPoint.X * 10000 / pActiveView.FocusMap.ReferenceScale;
            }
            else
            {
                pLineSymbol.Width = WKSPoint.X;
            }

            pRGBColor = new RgbColorClass();
            pRGBColor.Green = 148;
            pRGBColor.Red = 32;
            pRGBColor.Blue = 0;

            pSymbol = pLineSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            pDisplay.SetSymbol(pSymbol);
            pDisplay.DrawPolyline(pGeometry);
            System.Threading.Thread.Sleep(interval);
            pDisplay.DrawPolyline(pGeometry);
        }

        //闪烁多边形
        private void FlashPolygon(IActiveView pActiveView, IGeometry pGeometry, int interval)
        {
            IScreenDisplay pDisplay = pActiveView.ScreenDisplay;
            ISimpleFillSymbol pFillSymbol;
            ISymbol pSymbol;
            IRgbColor pRGBColor;

            pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Outline = null;

            pRGBColor = new RgbColorClass();
            pRGBColor.Green = 148;
            pRGBColor.Red = 32;
            pRGBColor.Blue = 0;

            pSymbol = pFillSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            pDisplay.SetSymbol(pSymbol);
            pDisplay.DrawPolygon(pGeometry);
            System.Threading.Thread.Sleep(interval);
            pDisplay.DrawPolygon(pGeometry);
        }

        //闪烁点
        private void FlashPoint(IActiveView pActiveView, IGeometry pGeometry, int interval)
        {
            IScreenDisplay pDisplay = pActiveView.ScreenDisplay;
            ISimpleMarkerSymbol pMarkerSymbol;
            ISymbol pSymbol;
            IRgbColor pRGBColor;

            pMarkerSymbol = new SimpleMarkerSymbolClass();
            pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;

            pRGBColor = new RgbColorClass();
            pRGBColor.Green = 148;
            pRGBColor.Red = 32;
            pRGBColor.Blue = 0;

            pSymbol = pMarkerSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
            pDisplay.SetSymbol(pSymbol);
            pDisplay.DrawPoint(pGeometry);
            System.Threading.Thread.Sleep(interval);
            pDisplay.DrawPoint(pGeometry);
        }
        #endregion
    }
}