using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Authorize;
using SysCommon.Gis;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using System.Collections;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.07.29
    /// 说明：面层符号显示设置窗体
    /// </summary>
    public partial class frmSetPolygonSymbol : DevComponents.DotNetBar.Office2007Form
    {
        //public string LabelExpression = "";//设置完毕，传给调用窗体
        public IList<ILayer> ResLst = null;
        public frmSetPolygonSymbol(IList<ILayer> inLst)
        {
            InitializeComponent();
            //listViewEx1.Items.Add("生产者").Tag = "producer";
            //listViewEx1.Items.Add("年度").Tag="producedate";
            //listViewEx1.Items.Add("发行者").Tag="publisher";
            //listViewEx1.Items.Add("所有者").Tag="owner";
            foreach (ILayer fdName in inLst)
            {
                ListViewItem lvi = listViewEx1.Items.Add(fdName.Name);
                lvi.Tag = fdName;
                lvi.Checked = true;
            }
            listViewEx1.Refresh();
        }
       


    
       

        private void frmMetaMap_Load(object sender, EventArgs e)
        {
           
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                if (lvi.Index > 0)
                {
                    int lviindex = lvi.Index;
                    listViewEx1.Items.Remove(lvi);
                    listViewEx1.Items.Insert(lviindex - 1, lvi);
                }
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnDn_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                if (lvi.Index < listViewEx1.Items.Count-1)
                {
                    int lviindex = lvi.Index;
                    listViewEx1.Items.Remove(lvi);
                    listViewEx1.Items.Insert(lviindex + 1, lvi);
                }
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnTp_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            int i = 0;
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                int lviindex = lvi.Index;
                listViewEx1.Items.Remove(lvi);
                listViewEx1.Items.Insert(i, lvi);
                i++;
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnBt_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            int i = listViewEx1.Items.Count - listViewEx1.SelectedItems.Count;
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                int lviindex = lvi.Index;
                listViewEx1.Items.Remove(lvi);
                listViewEx1.Items.Insert(i, lvi);
                i++;
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnSA_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.Items)
            {
                lvi.Checked = true;
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnSR_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.Items)
            {
                if(lvi.Checked)
                    lvi.Checked = false;
                else
                    lvi.Checked = true;
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ResLst = new List<ILayer>();
            foreach (ListViewItem lvi in listViewEx1.Items)
            {
                if(!lvi.Checked)
                  ResLst.Add(lvi.Tag as ILayer);
            }


        }

        private void btnCs_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
