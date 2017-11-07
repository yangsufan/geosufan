using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDBATool
{
    public partial class frmSetControlFields: DevComponents.DotNetBar.Office2007Form
    {

        private Dictionary<string, List<string>> m_Dic;///////记录的图层控制属性字段
        public Dictionary<string, List<string>> FieldDic
        {
            get { return this.m_Dic; }
            set { this.m_Dic = value; }
        }
        private string m_LayeName;///////////////////////////当前图层名称
        public frmSetControlFields(List<IFeatureClass> list_Fea,Dictionary<string, List<string>> in_RecDic)
        {
            InitializeComponent();
            this.m_Dic=in_RecDic;
            if (m_Dic==null)
                 m_Dic = new Dictionary<string, List<string>>();
            m_LayeName = string.Empty;
            if (list_Fea == null) return;
            this.advTree.Nodes.Clear();
            ///////////获取图层列表名称挂接在树上，FeatureClass挂接在节点tag上
            for (int i = 0; i < list_Fea.Count; i++)
            {
                DevComponents.AdvTree.Node NewNode = new DevComponents.AdvTree.Node();
                NewNode.Text = (list_Fea[i] as IDataset).Name;
                NewNode.Tag = list_Fea[i];
                this.advTree.Nodes.Add(NewNode);
            }

        }

        private void advTree_Click(object sender, EventArgs e)
        {
            DevComponents.AdvTree.Node SelectNode = this.advTree.SelectedNode;
            if (SelectNode == null) return;
            if (SelectNode.Tag == null) return;
            IFeatureClass OriFeaClass = SelectNode.Tag as IFeatureClass;
            ////////////////保存之前设置的信息////////////////////////
            if (this.dataGrid.Rows.Count>0)
            {
                string sLayerName = m_LayeName;
                List<string> list_Field = new List<string>();
                for (int i = 0; i < this.dataGrid.Rows.Count; i++)
                {
                    if (this.dataGrid.Rows[i].Cells[0].Value == null) continue;
                    if ((bool)this.dataGrid.Rows[i].Cells[0].Value)
                    {
                        list_Field.Add(this.dataGrid.Rows[i].Cells[1].Value.ToString());
                    }
                }
                if (m_Dic.ContainsKey(sLayerName))
                    m_Dic.Remove(sLayerName);
                if (list_Field.Count>0)
                    m_Dic.Add(sLayerName, list_Field);
            }           
            ///////////////////////////////////////////////////////////
            if (OriFeaClass == null) return;
            IFields getFields = OriFeaClass.Fields;
            DataTable NewDataSource = new DataTable();
            NewDataSource.Columns.Add("字段");
            for (int i = 0; i < getFields.FieldCount; i++)
            {                
                IField getField = getFields.get_Field(i);
                if (getField.Type == esriFieldType.esriFieldTypeGeometry || getField.Type == esriFieldType.esriFieldTypeGlobalID || getField.Type == esriFieldType.esriFieldTypeOID || getField.Type == esriFieldType.esriFieldTypeRaster) continue;
                DataRow NewRow = NewDataSource.NewRow();
                NewRow[0] = getField.Name;               
                NewDataSource.Rows.Add(NewRow);
            }
            this.dataGrid.DataSource = NewDataSource;
            for (int i = 0; i < this.dataGrid.Rows.Count; i++)
            {
                ////////////////////读取之前设置的信息           
                foreach (KeyValuePair<string, List<string>> item in m_Dic)
                {
                    if (item.Key == SelectNode.Text)
                    {
                        if (item.Value.Contains(this.dataGrid.Rows[i].Cells[1].Value.ToString()))
                            this.dataGrid.Rows[i].Cells[0].Value = true;
                    }
                }
                //////////////////////
            }
                m_LayeName = SelectNode.Text;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            DevComponents.AdvTree.Node SelectNode = this.advTree.SelectedNode;
            if (SelectNode == null) return;
            if (SelectNode.Tag == null) return;
            IFeatureClass OriFeaClass = SelectNode.Tag as IFeatureClass;
            ////////////////保存之前设置的信息////////////////////////
            if (this.dataGrid.Rows.Count > 0)
            {
                string sLayerName = m_LayeName;
                List<string> list_Field = new List<string>();
                for (int i = 0; i < this.dataGrid.Rows.Count; i++)
                {
                    if (this.dataGrid.Rows[i].Cells[0].Value == null) continue;
                    if ((bool)this.dataGrid.Rows[i].Cells[0].Value)
                    {
                        list_Field.Add(this.dataGrid.Rows[i].Cells[1].Value.ToString());
                    }
                }
                if (m_Dic.ContainsKey(sLayerName))
                    m_Dic.Remove(sLayerName);
                if (list_Field.Count > 0)
                    m_Dic.Add(sLayerName, list_Field);
            }    
            ///////////////////////////////////////////////////////////
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btn_cancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }


    }
}