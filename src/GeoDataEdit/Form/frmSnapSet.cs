using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

namespace GeoDataEdit
{
    public partial class frmSnapSet : DevComponents.DotNetBar.Office2007Form
    {

        private bool valitateSnapRadius;
        private bool valitateCacheRadius;

        //捕捉设置返回结果
        private bool m_OutRes;
        public bool OutRes
        {
            get
            {
                return m_OutRes;
            }
        }
        
        public frmSnapSet(IMap pMap)
        {
            InitializeComponent();

            //初始化dataGridViewX
            InitialdataGridViewX();

            //加载图层到listViewExLayers
            if (pMap.LayerCount == 0) return;
            AddLayersToList(pMap, dataGridViewX);
            if (MoData.v_SearchDist != 0)
            {
                txtSnapRadius.Text = MoData.v_SearchDist.ToString();
            }

            if (MoData.v_CacheRadius != 0)
            {
                txtCacheRadius.Text = MoData.v_CacheRadius.ToString();
            }
        }

        //初始化dataGridViewX
        private void InitialdataGridViewX()
        {
            dataGridViewX.DataSource = null;

            DataGridViewTextBoxColumn textBoxColumn = new DataGridViewTextBoxColumn();
            textBoxColumn.Name = "colLayName";
            textBoxColumn.HeaderText = "图层";
            textBoxColumn.DataPropertyName = "colLayName";
            textBoxColumn.Width = dataGridViewX.Width*3/8-18;
            textBoxColumn.ReadOnly = true;
            dataGridViewX.Columns.Add(textBoxColumn);

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "colVertexPoint";
            checkBoxColumn.HeaderText = "节点";
            checkBoxColumn.DataPropertyName = "colVertexPoint";
            checkBoxColumn.Width = dataGridViewX.Width * 1 / 8;
            dataGridViewX.Columns.Add(checkBoxColumn);

            checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "colPortPoint";
            checkBoxColumn.HeaderText = "端点";
            checkBoxColumn.DataPropertyName = "colPortPoint";
            checkBoxColumn.Width = dataGridViewX.Width * 1 / 8;
            dataGridViewX.Columns.Add(checkBoxColumn);

            checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "colIntersectPoint";
            checkBoxColumn.HeaderText = "相交点";
            checkBoxColumn.DataPropertyName = "colIntersectPoint";
            checkBoxColumn.Width = dataGridViewX.Width * 1 / 8;
            dataGridViewX.Columns.Add(checkBoxColumn);

            checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "colMidPoint";
            checkBoxColumn.HeaderText = "中点";
            checkBoxColumn.DataPropertyName = "colMidPoint";
            checkBoxColumn.Width = dataGridViewX.Width * 1 / 8;
            dataGridViewX.Columns.Add(checkBoxColumn);

            checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "colNearestPoint";
            checkBoxColumn.HeaderText = "最近点";
            checkBoxColumn.DataPropertyName = "colNearestPoint";
            checkBoxColumn.Width = dataGridViewX.Width * 1 / 8;
            dataGridViewX.Columns.Add(checkBoxColumn);

            //初始化DataGridView(dgvType)其他属性
            dataGridViewX.RowTemplate.Height = 18;
            dataGridViewX.RowTemplate.Resizable = DataGridViewTriState.False;
            dataGridViewX.RowHeadersVisible = false;
            dataGridViewX.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        //加载图层到listViewExLayers
        private void AddLayersToList(IMap pMap, DevComponents.DotNetBar.Controls.DataGridViewX pDataGridViewX)
        {
            UID pUID = new UIDClass();
            pUID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";   //UID for IGeoFeatureLayer
            IEnumLayer pEnumLayer = pMap.get_Layers(pUID, true);
            pEnumLayer.Reset();
            ILayer pLayer = pEnumLayer.Next();
            while(pLayer!=null)
            {
                AddLayerToList(pLayer, pDataGridViewX);
                pLayer = pEnumLayer.Next();
            }
        }
        private void AddLayerToList(ILayer pLayer, DevComponents.DotNetBar.Controls.DataGridViewX pDataGridViewX)
        {
            int index = pDataGridViewX.Rows.Add();
            DataGridViewRow aRow = pDataGridViewX.Rows[index];
            aRow.Tag = pLayer;
            aRow.Cells[0].Value = pLayer.Name; //图层名
            aRow.Cells[1].Value = false;        //节点捕捉
            aRow.Cells[2].Value = false;        //端点捕捉
            aRow.Cells[3].Value = false;        //相交点捕捉
            aRow.Cells[4].Value = false;        //中点捕捉
            aRow.Cells[5].Value = false;      //最近点捕捉

            if (MoData.v_dicSnapLayers != null)
            {
                if (MoData.v_dicSnapLayers.ContainsKey(pLayer))
                {
                    object[] values = MoData.v_dicSnapLayers[pLayer].ToArray();
                    aRow.Cells[1].Value = Convert.ToBoolean(values[0]);        //节点捕捉
                    aRow.Cells[2].Value = Convert.ToBoolean(values[1]);        //端点捕捉
                    aRow.Cells[3].Value = Convert.ToBoolean(values[2]);        //相交点捕捉
                    aRow.Cells[4].Value = Convert.ToBoolean(values[3]);        //中点捕捉
                    aRow.Cells[5].Value = Convert.ToBoolean(values[4]);        //最近点捕捉
                }
            }
        }

        private void buttonXOk_Click(object sender, EventArgs e)
        {
            if (txtSnapRadius.Text == "" || txtCacheRadius.Text == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "捕捉半径或缓存半径不能为空或者零!");
                return;
            }

            MoData.v_SearchDist = Convert.ToDouble(txtSnapRadius.Text);
            MoData.v_CacheRadius = Convert.ToDouble(txtCacheRadius.Text);

            if (MoData.v_dicSnapLayers == null)
            {
                MoData.v_dicSnapLayers = new Dictionary<ILayer, ArrayList>();
            }

            foreach (DataGridViewRow aRow in dataGridViewX.Rows)
            {
                bool bVertexPoint = Convert.ToBoolean(aRow.Cells[1].Value);       //节点捕捉
                bool bPortPoint = Convert.ToBoolean(aRow.Cells[2].Value);         //端点捕捉
                bool bIntersectPoint = Convert.ToBoolean(aRow.Cells[3].Value);    //相交点捕捉
                bool bMidPoint = Convert.ToBoolean(aRow.Cells[4].Value);          //中点捕捉
                bool bNearestPoint = Convert.ToBoolean(aRow.Cells[5].Value);      //最近点捕捉
                ILayer pLay = aRow.Tag as ILayer;
                if (pLay == null) continue;

                ArrayList aArrayList = new ArrayList();
                aArrayList.AddRange(new bool[] { bVertexPoint, bPortPoint, bIntersectPoint, bMidPoint, bNearestPoint });

                if (bVertexPoint || bPortPoint || bIntersectPoint || bMidPoint || bNearestPoint)
                {
                    if (!MoData.v_dicSnapLayers.ContainsKey(pLay))
                    {
                        MoData.v_dicSnapLayers.Add(pLay, aArrayList);
                    }
                }

                if (MoData.v_dicSnapLayers.ContainsKey(pLay))
                {
                    MoData.v_dicSnapLayers[pLay] = aArrayList;
                }
            }
            
            m_OutRes = true;
            this.Close();
        }

        private void buttonXCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSnapRadius_KeyDown(object sender, KeyEventArgs e)
        {
            valitateSnapRadius = false;

            //小数点键".",检查小数点是否已经存在
            if (e.KeyValue == 46 && txtSnapRadius.Text.Contains("."))
            {
                valitateSnapRadius = true;
                return;
            }

            //数字键(48~57),退格键8
            if ((e.KeyValue > 57 || e.KeyValue < 48) && e.KeyValue != 8)
            {
                valitateSnapRadius = true;
                return;
            }
        }

        private void txtSnapRadius_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (valitateSnapRadius == true)
            {
                e.Handled = true;
            }
        }

        private void txtCacheRadius_KeyDown(object sender, KeyEventArgs e)
        {
            valitateSnapRadius = false;

            //小数点键".",检查小数点是否已经存在
            if (e.KeyValue == 46 && txtSnapRadius.Text.Contains("."))
            {
                valitateCacheRadius = true;
                return;
            }

            //数字键(48~57),退格键8
            if ((e.KeyValue > 57 || e.KeyValue < 48) && e.KeyValue != 8)
            {
                valitateCacheRadius = true;
                return;
            }
        }

        private void txtCacheRadius_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (valitateCacheRadius == true)
            {
                e.Handled = true;
            }
        }

        private void txtSnapRadius_TextChanged(object sender, EventArgs e)
        {
            if (txtSnapRadius.Text == "") return;
            if (Convert.ToDouble(txtSnapRadius.Text) > 1300)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "设置的值过大!该值最好不要超过屏幕的宽度(像素单位)!请重新设置!");
            }
        }

        private void txtCacheRadius_TextChanged(object sender, EventArgs e)
        {
            if (txtCacheRadius.Text == "") return;
            if (Convert.ToDouble(txtCacheRadius.Text) > 1300)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "设置的值过大!该值最好不要超过屏幕的宽度(像素单位)!请重新设置!");
            }
        }
    }
}