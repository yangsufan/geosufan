using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace FileDBTool
{
    public partial class FrmQueryAll : DevComponents.DotNetBar.Office2007Form
    {
       

        public FrmQueryAll()
        {
            InitializeComponent();
        }

      
        private void cmbRange_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
           
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //双击定位
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}