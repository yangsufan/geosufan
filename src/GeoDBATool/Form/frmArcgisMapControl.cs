using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using ESRI.ArcGIS.Controls;

namespace GeoDBATool
{
    public partial class frmArcgisMapControl : Form
    {
        public AxMapControl ArcGisMapControl
        {
            get { return this.axMapControl; }
        }

        public frmArcgisMapControl()
        {
            InitializeComponent();
        }
    }
}