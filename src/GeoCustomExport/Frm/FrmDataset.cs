using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace GeoCustomExport
{
    /// <summary>
    /// 选择数据集
    /// </summary>
    public partial class FrmDataset : DevComponents.DotNetBar.Office2007Form
    {
        public FrmDataset()
        {
            InitializeComponent();
        }


        public DevComponents.DotNetBar.Controls.ListViewEx ListDataset
        {
            get { return lstFeaDataset; }
            set { lstFeaDataset = value; }
        }

        private void FrmDataset_Load(object sender, EventArgs e)
        {
            if (lstFeaDataset.Items.Count == 0) return;
            this.lstFeaDataset.Refresh();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }
}
