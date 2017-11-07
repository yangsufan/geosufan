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
using GeoDataCenterFunLib;

namespace GeoDataEdit
{
    public enum enumAttributeEditMode
    {
        Top=0,
        Visiable=1,
        Selectable=2,
        CurEdit=3,
        All=4
    }

    public partial class frmAttributeEdit : DevComponents.DotNetBar.Office2007Form
    {
        private DataTable m_dataSourceGrid;        //grid的datasource
        private IMapControlDefault m_pMapControl;                        //被查询的地图对象
        private enumAttributeEditMode m_enumAttributeEditMode;     //查询方式
        public enumAttributeEditMode AttributeEditMode//从查询那得来，暂不使用
        {
            get
            {
                return m_enumAttributeEditMode;
            }
        }
        private IFeatureCursor updateFCur = null;

        /// <summary>
        /// 初始化，用来SQL查询
        /// </summary>
        /// <param name="pMapControl"></param>
        public frmAttributeEdit(IMapControlDefault pMapControl)
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
        /// <param name="penumAttributeEditMode">查询方式</param>
        public frmAttributeEdit(IMapControlDefault pMapControl, enumAttributeEditMode penumAttributeEditMode)
        {
            InitializeComponent();
            labelItem.Visible = true;
            comboBoxItem.Visible = true;

            InitializeGrid();      //初始化Grid的表现  
            m_pMapControl = pMapControl;
            m_enumAttributeEditMode = penumAttributeEditMode;
            switch(penumAttributeEditMode)
            {
                case enumAttributeEditMode.Top:
                    comboBoxItem.SelectedIndex = 0;
                    break;
                case enumAttributeEditMode.Visiable:
                    comboBoxItem.SelectedIndex = 1;
                    break;
                case enumAttributeEditMode.Selectable:
                    comboBoxItem.SelectedIndex = 2;
                    break;
                case enumAttributeEditMode.CurEdit:
                    comboBoxItem.SelectedIndex = 3;
                    break;
                case enumAttributeEditMode.All:
                    comboBoxItem.SelectedIndex = 4;
                    break;
            }
        }

        //初始化grid 显示和数据
        private void InitializeGrid()
        {
            dataGridViewX.Columns.Clear();
            //dataGridViewX.ReadOnly = true;
            dataGridViewX.AutoGenerateColumns = false;
            m_dataSourceGrid = new DataTable();
            m_dataSourceGrid.Columns.Add("Name", typeof(System.String));
            m_dataSourceGrid.Columns["Name"].ReadOnly = true;
            m_dataSourceGrid.Columns.Add("Value", typeof(System.String));
           // m_dataSourceGrid.Columns["Value"].ReadOnly = true;
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
                    m_enumAttributeEditMode = enumAttributeEditMode.Top;
                    break;
                case 1:
                    m_enumAttributeEditMode = enumAttributeEditMode.Visiable;
                    break;
                case 2:
                    m_enumAttributeEditMode = enumAttributeEditMode.Selectable;
                    break; 
                case 3:
                    m_enumAttributeEditMode = enumAttributeEditMode.CurEdit;
                    break;
                case 4:
                    m_enumAttributeEditMode = enumAttributeEditMode.All;
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
                labelItemMemo.Text = "0个要素";
                return;
            }

            UID pUID = new UIDClass();
            pUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";   //UID for IFeatureLayer
            IEnumLayer pEnumLayer=pMap.get_Layers(pUID, true);
            pEnumLayer.Reset();
            ILayer pLayer = pEnumLayer.Next();
            
            //获取查询的图层
            List<ILayer> listLays = new List<ILayer>();
            switch (m_enumAttributeEditMode)
            {
                case enumAttributeEditMode.Top:
                case enumAttributeEditMode.Visiable:
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
                case enumAttributeEditMode.Selectable:
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
                case enumAttributeEditMode.CurEdit:
                    pEnumLayer.Reset();
                    pLayer = pEnumLayer.Next();//将当前编辑图层加进来
                    while (pLayer != null)
                    {
                        IFeatureLayer pTemp = pLayer as IFeatureLayer;
                        if (pTemp.Selectable == true)
                        {
                            listLays.Add(pLayer);
                        }

                        pLayer = pEnumLayer.Next();
                    }
                    break;
                case enumAttributeEditMode.All:
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
                if (pFeatLay.FeatureClass.FeatureCount(pSpatialFilter) == 0) continue;

                //添加图层节点
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Text = pLay.Name;
                node.Tag = pLay;
                node.Expand();
                advTree.Nodes.Add(node);

                IFeatureCursor pFeatureCursor = pFeatLay.FeatureClass.Update(pSpatialFilter, false);
                updateFCur = pFeatureCursor;
                IFeature pFeat = pFeatureCursor.NextFeature();
                while (pFeat != null)
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
                                m_dataSourceGrid.Rows.Add(new object[] { pFeat.Fields.get_Field(i).AliasName , strGeometryType });
                            }
                            else
                            {
                                m_dataSourceGrid.Rows.Add(new object[] { pFeat.Fields.get_Field(i).AliasName , pFeat.get_Value(i) });
                            }
                        }
                        bFirst = false;
                    }

                    pFeat = pFeatureCursor.NextFeature();

                    intCnt++;
                }

                if (m_enumAttributeEditMode == enumAttributeEditMode.Top)
                {
                    progressBarItem.Value = listLays.Count;
                    break;
                }
            }

            labelItemMemo.Text = intCnt.ToString() + "个要素";
            progressBarItem.Visible = false;
            this.Refresh();

            DefaultSelNde();
        }

        //默认选择第一个要素节点  并进行闪烁
        private void DefaultSelNde()
        {
            Application.DoEvents();

            for (int i = 0; i < this.advTree.Nodes.Count; i++)
            {
                if (!this.advTree.Nodes[i].HasChildNodes) continue;

                IFeature pFea = this.advTree.Nodes[i].Nodes[0].Tag as IFeature;
                if (pFea != null)
                {
                    this.advTree.Nodes[i].IsSelectionVisible = true;
                    ModDBOperator.FlashFeature(pFea.Shape, m_pMapControl.ActiveView,100);
                    break;
                }
            }
        }


        /// <summary>
        /// 根据条件查找数据填充窗体，用来进行SQL查询
        /// </summary>
        /// <param name="pMap">被查询的地图对象</param>
        public void FillData(IFeatureLayer pFeatLay, IQueryFilter pAttributeEditFilter, esriSelectionResultEnum pSelectionResult)
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
            pFeatureSelection.SelectFeatures(pAttributeEditFilter, pSelectionResult, false);
            //保存查询的结果，包括要素类和查找到的要素
            //循环取得选中Feature的ID来得到Feature

            IFeature pFeature;
            IEnumIDs pEnumIDs = pFeatureSelection.SelectionSet.IDs;
            int iIDIndex = pEnumIDs.Next();
            while (iIDIndex != -1)
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
                            m_dataSourceGrid.Rows.Add(new object[] { pFeature.Fields.get_Field(i).AliasName , strGeometryType });
                        }
                        else
                        {
                            m_dataSourceGrid.Rows.Add(new object[] { pFeature.Fields.get_Field(i).AliasName , pFeature.get_Value(i) });
                        }
                    }
                    bFirst = false;
                }
                iIDIndex = pEnumIDs.Next();
                intCnt++;
            }

            labelItemMemo.Text = intCnt.ToString() + "个要素";
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
                            m_dataSourceGrid.Rows.Add(new object[] { pfeature.Fields.get_Field(i).AliasName , strGeometryType });
                        }
                        else
                        {
                            m_dataSourceGrid.Rows.Add(new object[] { pfeature.Fields.get_Field(i).AliasName , pfeature.get_Value(i) });
                        }
                    }

                    ModDBOperator.FlashFeature(pfeature, m_pMapControl);
                }
            }
            catch
            {

            }
        }

        private void advTree_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            try
            {
                IFeature pfeature = advTree.SelectedNode.Tag as IFeature;
                if (pfeature != null)
                {
                    SysCommon.Gis.ModGisPub.ZoomToFeature(m_pMapControl, pfeature);
                    Application.DoEvents();
                    ModDBOperator.FlashFeature(pfeature, m_pMapControl);
                    //m_pMapControl.FlashShape(pfeature.Shape, 2, 500, null);
                }
            }
            catch
            {
            }
        }

        private void dataGridViewX_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                IFeature pfeature = advTree.SelectedNode.Tag as IFeature;
                if (pfeature != null)
                {
                    int fdpos = pfeature.Fields.FindFieldByAliasName(dataGridViewX[e.ColumnIndex-1,e.RowIndex].Value.ToString());
                    pfeature.set_Value(fdpos, dataGridViewX[e.ColumnIndex, e.RowIndex].Value.ToString());
                    updateFCur.UpdateFeature(pfeature);

                }
            }
            catch
            {
                MessageBox.Show("请按正确的字段格式填写属性！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}