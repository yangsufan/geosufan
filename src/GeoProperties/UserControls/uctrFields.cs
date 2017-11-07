using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

namespace GeoProperties.UserControls
{
    public partial class uctrFields : UserControl
    {
        ILayer m_pLayer; //获取的Layer
        IFeatureLayer m_pCurrentLayer; //当前图层
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pLayer"></param>
        public uctrFields(ILayer pLayer)
        {

            try
            {//实例化
                m_pLayer = pLayer;
                m_pCurrentLayer = (IFeatureLayer)m_pLayer;

                InitializeComponent();
                InitLayerFields();
            }
            catch
            {
             
            }
        }
        /// <summary>
        /// 初始化字段内容
        /// </summary>
        private void InitLayerFields()
        {

            IFields pFields;
            ListViewItem fieldItem;
            pFields = m_pCurrentLayer.FeatureClass.Fields; //获取当前图层的所有字段
            ILayerFields pLayerFields =(ILayerFields) m_pCurrentLayer;
            //遍历并获取每个字段
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                //根据字段属性设置Item
                fieldItem = new ListViewItem();
                IField pField = pFields.get_Field(i);
                IFieldInfo pFieldInfo = pLayerFields.get_FieldInfo(i);
                fieldItem.Name = pField.Name;
                fieldItem.Text = pField.Name;
                fieldItem.Tag = pField;
                fieldItem.Checked = pFieldInfo.Visible;
                //设置Item的SubItems
                string[] subItems = new string[5];
                subItems[0] = pField.AliasName;
                subItems[1] = GetFieldType(pField.Type);
                subItems[2] = pField.Length.ToString();
                subItems[3] = pField.Precision.ToString();
                subItems[4] = pField.Scale.ToString();
                fieldItem.SubItems.AddRange(subItems);
                //添加Item到ListView
                lstFieldsView.Items.Add(fieldItem);
                if (fieldItem.Checked)
                {
                    cboFields.Items.Add(fieldItem.Name);
                }
            }
            if (cboFields.Items.Count > 0)
            {
                cboFields.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            //遍历所有的Item，并设置Checked属性为true
            for (int i = 0; i < lstFieldsView.Items.Count; i++)
            {
                lstFieldsView.Items[i].Checked = true;
            }
        }
        /// <summary>
        /// 全不选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            //遍历所有的Item，并设置Checked属性为false
            for (int i = 0; i < lstFieldsView.Items.Count; i++)
            {
                lstFieldsView.Items[i].Checked = false;
            }
        }
        /// <summary>
        /// 获取Field的类型，并按规定内容显示
        /// </summary>
        /// <param name="fieldTpye"></param>
        /// <returns></returns>
        private string GetFieldType(esriFieldType fieldTpye)
        {
            string type = null;
            switch (fieldTpye)
            {
                case esriFieldType.esriFieldTypeBlob:
                    type = "Blob";
                    break;
                case esriFieldType.esriFieldTypeDate:
                    type = "Data";
                    break;
                case esriFieldType.esriFieldTypeDouble:
                    type = "Double";
                    break;
                case esriFieldType.esriFieldTypeGeometry:
                    type = GetGeometryType();
                    break;
                case esriFieldType.esriFieldTypeGlobalID:
                    type = "GlobalID";
                    break;
                case esriFieldType.esriFieldTypeGUID:
                    type = "GUID";
                    break;
                case esriFieldType.esriFieldTypeInteger:
                    type = "Integer";
                    break;
                case esriFieldType.esriFieldTypeOID:
                    type = "ObjectID";
                    break;
                case esriFieldType.esriFieldTypeRaster:
                    type = "Raster";
                    break;
                case esriFieldType.esriFieldTypeSingle:
                    type = "Single";
                    break;
                case esriFieldType.esriFieldTypeSmallInteger:
                    type = "SmallInteger";
                    break;
                case esriFieldType.esriFieldTypeString:
                    type = "Text";
                    break;
                case esriFieldType.esriFieldTypeXML:
                    type = "XML";
                    break;
            }
            return type;
        }
        /// <summary>
        /// 根据FeatureClass的ShapeType设置其显示类型
        /// </summary>
        /// <returns></returns>
        private string GetGeometryType()
        {
            string type = null;
            switch (m_pCurrentLayer.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    type = "Point";
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    type = "Polyline";
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    type = "Polygon";
                    break;
                case esriGeometryType.esriGeometryLine:
                    type = "Line";
                    break;
                case esriGeometryType.esriGeometryMultipoint:
                    type = "Multipoint";
                    break;
                default:
                    type = "Unknow";
                    break;
            }
            return type;
        }
        /// <summary>
        /// 保存字段更改
        /// </summary>
        public void SaveFieldChange()
        {
            ILayerFields pLayerFields = (ILayerFields)m_pCurrentLayer;
            if (cboFields == null) return;
            cboFields.Items.Clear();
            cboFields.Text = null;
            //遍历所有的Item，并根据其Checked属性设置字段的显示与否
            for (int i = 0; i < lstFieldsView.Items.Count; i++)
            {
                IField pField;
                pField = lstFieldsView.Items[i].Tag as IField;
                IFieldInfo pFieldInfo = pLayerFields.get_FieldInfo(i);
                ListViewItem fieldItem=lstFieldsView.Items[i];
                pFieldInfo.Visible = fieldItem.Checked;
                if (fieldItem.Checked)
                {
                    cboFields.Items.Add(fieldItem.Name);
                }
            }
            if (cboFields.Items.Count > 0)
            {
                cboFields.SelectedIndex = 0;
            }
            
        }
    }
}
