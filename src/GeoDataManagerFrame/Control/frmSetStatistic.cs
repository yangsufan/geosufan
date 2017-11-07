using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDataManagerFrame
{
    public partial class frmSetStatistic : DevComponents.DotNetBar.Office2007Form
    {
        public bool bIsCheck = true;
        public frmSetStatistic()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bIsCheck = chkBoX.Checked;
            DialogResult = DialogResult.OK;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

       
    }
}
