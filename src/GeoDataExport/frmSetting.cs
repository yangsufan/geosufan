using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDataExport
{
    public partial class frmSetting : DevComponents.DotNetBar.Office2007Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        public double g_dblXoff = 0;
        public double g_dblYoff = 0;
        public double g_dblCentX = 0;
        public double g_dblCentY = 0;
        public double g_dblRotate = 0;

        private void initCtl()
        {
            this.txtCentX.Value = g_dblCentX;
            this.txtCentY.Value = g_dblCentY;
            this.txtXoff.Value = g_dblXoff;
            this.txtYoff.Value = g_dblYoff;
            this.txtRotate.Value = g_dblRotate;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            g_dblXoff=this.txtXoff.Value;
            g_dblYoff=this.txtYoff.Value;
            g_dblCentX=this.txtCentX.Value;
            g_dblCentY=this.txtCentY.Value;
            g_dblRotate=this.txtRotate.Value;

            this.Close();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            initCtl();
        }
    }
}