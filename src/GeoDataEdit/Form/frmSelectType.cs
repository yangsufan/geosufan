using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;


using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using GeoDataCenterFunLib;
using GeoDataEdit.Tools;




namespace GeoDataEdit
{
    public partial class frmSelectType : DevComponents.DotNetBar.Office2007Form
    {



        public frmSelectType()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            this.Close();
        }
      

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmAddPoint_Load(object sender, EventArgs e)
        {
            rbPolygon.Checked = true;
            rbPolyline.Checked = false;
        }

        private void rbPolygon_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPolygon.Checked)
            {
                ToolDrawPolygons.strType = "Polygon";
                rbPolyline.Checked = false;
            }
        }

        private void rbPolyline_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPolyline.Checked)
            {
                ToolDrawPolygons.strType = "Polyline";
                rbPolygon.Checked = false;
            }
        }

      





    }
}