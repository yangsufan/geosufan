using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataManagerFrame
{
    /// <summary>
    /// 作者：席胜
    /// 日期：2011.03.02
    /// 说明：图层属性表实现窗体
    /// </summary>
    public partial class frmAttributeTable : DevComponents.DotNetBar.Office2007Form
    {
        public frmAttributeTable(ESRI.ArcGIS.Controls.AxMapControl axMapControl)
        {
            InitializeComponent();
            m_axMapControl=axMapControl;
        }
        public IMap m_map;
        private ESRI.ArcGIS.Controls.AxMapControl m_axMapControl;
        public DataTable attributeTable;
        static IFeatureLayer m_pFeatLyr;
        static ILayer m_player;
        int n;//初始加载的数量
        DataTable pDataTable;
        ITable pTable;
        string shapeType;//图层类型
        
        /// <summary>
        /// 根据图层字段创建一个只含字段的空DataTable
        /// </summary>
        /// <param name="pLayer"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static DataTable CreateDataTableByLayer(ILayer pLayer, string tableName)
        {
            //创建一个DataTable表
            DataTable pDataTable = new DataTable(tableName);
            //取得ITable接口
            ITable pTable = pLayer as ITable;
            IField pField = null;
            DataColumn pDataColumn;
            //根据每个字段的属性建立DataColumn对象
            for (int i = 0; i < pTable.Fields.FieldCount; i++)
            {
                pField = pTable.Fields.get_Field(i);
                //新建一个DataColumn并设置其属性
                pDataColumn = new DataColumn(pField.Name);
                if (pField.Name == pTable.OIDFieldName)
                {
                    pDataColumn.Unique = true;//字段值是否唯一
                }
                //字段值是否允许为空
                pDataColumn.AllowDBNull = pField.IsNullable;
                //字段别名
                pDataColumn.Caption = pField.AliasName;
                //字段数据类型
                pDataColumn.DataType = System.Type.GetType(ParseFieldType(pField.Type));
                //字段默认值
                pDataColumn.DefaultValue = pField.DefaultValue;
                //当字段为String类型是设置字段长度
                if (pField.VarType == 8)
                {
                    pDataColumn.MaxLength = pField.Length;
                }
                //字段添加到表中
                pDataTable.Columns.Add(pDataColumn);
                pField = null;
                pDataColumn = null;
            }
            return pDataTable;
        }


        /// <summary>
        /// 将GeoDatabase字段类型转换成.Net相应的数据类型
        /// </summary>
        /// <param name="fieldType">字段类型</param>
        /// <returns></returns>
        public static string ParseFieldType(esriFieldType fieldType)
        {
            switch (fieldType)
            {
                case esriFieldType.esriFieldTypeBlob:
                    return "System.String";
                case esriFieldType.esriFieldTypeDate:
                    return "System.DateTime";
                case esriFieldType.esriFieldTypeDouble:
                    return "System.Double";
                case esriFieldType.esriFieldTypeGeometry:
                    return "System.String";
                case esriFieldType.esriFieldTypeGlobalID:
                    return "System.String";
                case esriFieldType.esriFieldTypeGUID:
                    return "System.String";
                case esriFieldType.esriFieldTypeInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeOID:
                    return "System.String";
                case esriFieldType.esriFieldTypeRaster:
                    return "System.String";
                case esriFieldType.esriFieldTypeSingle:
                    return "System.Single";
                case esriFieldType.esriFieldTypeSmallInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeString:
                    return "System.String";
                default:
                    return "System.String";
            }
        }
        /// <summary> 
        /// 填充DataTable中的数据
        /// </summary>
        /// <param name="pLayer"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public  DataTable CreateDataTable(ILayer pLayer, string tableName)
        {
            //创建空DataTable
            pDataTable = CreateDataTableByLayer(pLayer, tableName);
            //取得图层类型
           shapeType = getShapeType(pLayer);
            //创建DataTable的行对象
            DataRow pDataRow = null;
            //从ILayer查询到ITable
            pTable = pLayer as ITable;
            //取得ITable中的行信息
            ICursor pCursor = pTable.Search(null, false);
            IRow pRow = pCursor.NextRow();
            n = 0;
            while (pRow != null)
            {
                //新建DataTable的行对象
                pDataRow = pDataTable.NewRow();
                for (int i = 0; i < pRow.Fields.FieldCount; i++)
                {
                    //如果字段类型为esriFieldTypeGeometry，则根据图层类型设置字段值
                    if (pRow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        pDataRow[i] = shapeType;
                    }
                    //当图层类型为Anotation时，要素类中会有esriFieldTypeBlob类型的数据，
                    //其存储的是标注内容，如此情况需将对应的字段值设置为Element
                    else if (pRow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeBlob)
                    {
                        pDataRow[i] = "Element";
                    }
                    else
                    {
                        pDataRow[i] = pRow.get_Value(i);
                    }
                }
                //添加DataRow到DataTable
                pDataTable.Rows.Add(pDataRow);
                pDataRow = null;
                n++;
                //为保证效率，一次只装载最多条记录
                if (n ==100)
                {
                    pRow = null;
                }
                else
                {
                    pRow = pCursor.NextRow();
                }
            }
            return pDataTable;
        }

        /// <summary>
        /// 获得图层的Shape类型
        /// </summary>
        /// <param name="pLayer">图层</param>
        /// <returns></returns>
        public static string getShapeType(ILayer pLayer)
        {
            IFeatureLayer pFeatLyr = (IFeatureLayer)pLayer;
            m_pFeatLyr = pFeatLyr;//全局变量
            m_player = pLayer;
            switch (pFeatLyr.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    return "Point";
                case esriGeometryType.esriGeometryPolyline:
                    return "Polyline";
                case esriGeometryType.esriGeometryPolygon:
                    return "Polygon";
                default:
                    return "";
            }
        }

        /// <summary> 
        /// 绑定DataTable到DataGridView
        /// </summary>
        /// <param name="player"></param>
        public void CreateAttributeTable(ILayer player)
        {
            string tableName;
            if (player == null) return;
            tableName = getValidFeatureClassName(player.Name);
            this.Text = "属性表[" + tableName + "] ";
            attributeTable = CreateDataTable(player, tableName);
            this.dataGridView.DataSource = attributeTable;
            
        }

        /// <summary>
        /// 替换数据表名中的点
        /// </summary>
        /// <param name="FCname"></param>
        /// <returns></returns>
        public static string getValidFeatureClassName(string FCname)
        {
            int dot = FCname.IndexOf(".");
            if (dot != -1)
            {
                return FCname.Replace(".", "_");
            }
            return FCname;
        }

        private void frmAttributeTable_Load(object sender, EventArgs e)
        {
            textBoxIndex.Text ="1";
            dataGridView.CurrentCell = dataGridView.Rows[0].Cells[0];
            panel1.Height = 30;
            dataGridView.Height = this.Height - 65;
            timer.Enabled = true;


        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxIndex.Text = e.RowIndex + 1 + "";
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            textBoxIndex.Text = "1";
            dataGridView.CurrentCell = dataGridView.Rows[0].Cells[0];
        }

        private void btnprevious_Click(object sender, EventArgs e)
        {   
            int index=dataGridView.CurrentRow.Index;
            if (index > 0)
            {
                textBoxIndex.Text = index.ToString();
                dataGridView.CurrentCell = dataGridView.Rows[index - 1].Cells[0];
            }
        }

        private void btnnext_Click(object sender, EventArgs e)
        {
            int index = dataGridView.CurrentRow.Index;
            if (index < dataGridView.Rows.Count - 2)
            {
                textBoxIndex.Text = index + 2 + "";
                dataGridView.CurrentCell = dataGridView.Rows[index + 1].Cells[0];
            }
        }

        private void btnlast_Click(object sender, EventArgs e)
        {
            textBoxIndex.Text = dataGridView.Rows.Count - 1 + "";
            dataGridView.CurrentCell = dataGridView.Rows[dataGridView.Rows.Count-2].Cells[0];
        }

        //调整控件长宽
        private void frmAttributeTable_ResizeEnd(object sender, EventArgs e)
        {
            panel1.Height = 30;
            dataGridView.Height = this.Height - 65;
        }

        private void frmAttributeTable_SizeChanged(object sender, EventArgs e)
        {
            panel1.Height = 30;
            dataGridView.Height = this.Height - 65;
        }

        //实现要素与图层联动。双击行实现
        private void dataGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        { 
           IFeature feature = m_pFeatLyr.FeatureClass.GetFeature(e.RowIndex+1);//得到当前选中要素
           IMap map = m_axMapControl.Map;//得到当前地图
           //map.ClearLayers();
           map.ClearSelection();//清空上次高亮显示的要素
           map.SelectFeature(m_player, feature);//选择当前选中的要素并高亮显示
           m_axMapControl.Extent = feature.Shape.Envelope;//放大要素并居中
           //IFeatureSelection pFeatureSel =new FeatureLayerClass();
           //pFeatureSel = (IFeatureSelection)m_pFeatLyr;
            //pFeatureSel.SelectFeatures
           m_axMapControl.ActiveView.Refresh();//刷新地图
        }

        /// <summary>
        /// 打开窗体后继续加载GridView
        /// </summary>
        private void LoadDataTable()
        {
            try
            {
                IQueryFilter pqueryFilter = new QueryFilterClass();
                pqueryFilter.SubFields = "*";
                pqueryFilter.WhereClause = "OBJECTID>" + n;
                ICursor pCursor = pTable.Search(pqueryFilter, false);
                //创建DataTable的行对象
                DataRow pDataRow = null;
                //取得ITable中的行信息
                IRow pRow = pCursor.NextRow();
                if (pRow == null)
                {
                    this.timer.Enabled = false;
                    this.Text += "记录数：" + attributeTable.Rows.Count;
                }
                int m = 0;
                while (pRow != null)
                {
                    //新建DataTable的行对象
                    pDataRow = pDataTable.NewRow();
                    for (int i = 0; i < pRow.Fields.FieldCount; i++)
                    {
                        //如果字段类型为esriFieldTypeGeometry，则根据图层类型设置字段值
                        if (pRow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            pDataRow[i] = shapeType;
                        }
                        //当图层类型为Anotation时，要素类中会有esriFieldTypeBlob类型的数据，
                        //其存储的是标注内容，如此情况需将对应的字段值设置为Element
                        else if (pRow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeBlob)
                        {
                            pDataRow[i] = "Element";
                        }
                        else
                        {
                            pDataRow[i] = pRow.get_Value(i);
                        }
                    }
                    //添加DataRow到DataTable
                    pDataTable.Rows.Add(pDataRow);
                    pDataRow = null;
                    m++;
                    n++;
                    //为保证效率，一次只装载最多条记录
                    if (m == 100)
                    {
                        pRow = null;
                    }
                    else
                    {
                        pRow = pCursor.NextRow();
                    }
                }
            }
            catch
            { }



        }
        private void timer_Tick(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.WaitCursor;
            LoadDataTable();
            //this.Cursor = Cursors.Default;
           
        } 


    }
}