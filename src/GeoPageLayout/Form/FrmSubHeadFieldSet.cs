using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110811
    /// 说明：设置副标题字段的窗体--批量导入范围出图
    /// </summary>
    public partial class FrmSubHeadFieldSet : DevComponents.DotNetBar.Office2007Form
    {
        public FrmSubHeadFieldSet(IList<string> inLst)
        {
            InitializeComponent();
            foreach (string str in inLst)
            {
              cboFields.Items.Add(str); 
            }
            if (cboFields.Items.Count > 0)
                cboFields.SelectedIndex = 0;
        }
        public string ResField = "";
        private void btnXOK_Click(object sender, EventArgs e)
        {
            ResField = cboFields.Text;
        }

        private void FrmEditLayerSet_Load(object sender, EventArgs e)
        {
            
        }

        private void cboLayers_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnXCancel_Click(object sender, EventArgs e)
        {

        }

        private void FrmEditLayerSet_Load_1(object sender, EventArgs e)
        {

        }

    }
}
