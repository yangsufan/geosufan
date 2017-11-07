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

namespace GeoDBATool
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.07.29
    /// 说明：元数据地图标注设置窗体
    /// </summary>
    public partial class frmMetaMapSetLabel : DevComponents.DotNetBar.Office2007Form
    {
        public string LabelExpression = "";//设置完毕，传给调用窗体
        public frmMetaMapSetLabel(IList<string> inLst)
        {
            InitializeComponent();
            //listViewEx1.Items.Add("生产者").Tag = "producer";
            //listViewEx1.Items.Add("年度").Tag="producedate";
            //listViewEx1.Items.Add("发行者").Tag="publisher";
            //listViewEx1.Items.Add("所有者").Tag="owner";
            foreach (string fdName in inLst)
            {
                listViewEx1.Items.Add(fdName);
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
            string lbExpression="";
            foreach (ListViewItem lvi in listViewEx1.Items)
            {
                if (lvi.Checked)
                    lbExpression += "\"" + lvi.Text + "：\" & [" + lvi.Text + "] & chr(13) & ";
                    //lbExpression += "\"" + lvi.Text + "：\" & [" + lvi.Tag.ToString() + "] & chr(13) & ";
            }
            if (lbExpression.EndsWith("& "))
                LabelExpression = lbExpression.Substring(0, lbExpression.Length - 2);
        }

        private void btnCs_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
