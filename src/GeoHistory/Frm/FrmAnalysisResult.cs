using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeoHistory
{
    public partial class FrmAnalysisResult : DevComponents.DotNetBar.Office2007Form
    {
        public FrmAnalysisResult()
        {
            InitializeComponent();
        }
        public DevComponents.DotNetBar.Controls.DataGridViewX ResultGrid
        {
            get { return this.dataGridViewX1; }
        }

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
