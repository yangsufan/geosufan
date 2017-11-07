using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDBATool
{
    public partial class RasterErrInfoFrm : DevComponents.DotNetBar.Office2007Form
    {
        //table
        DataTable m_Table = null;
        public RasterErrInfoFrm(DataTable pTable)
        {
            InitializeComponent();
            m_Table = pTable;
            dataGridViewX1.DataSource = null;
            dataGridViewX1.DataSource = pTable;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
