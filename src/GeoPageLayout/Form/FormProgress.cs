using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoPageLayout
{
    partial class FormProgress : DevComponents.DotNetBar.Office2007Form
    {
        public FormProgress()
        {
            InitializeComponent();
        }
        public ProgressBar PgsBar
        {
            get { return progressBar1; }
        }
        public string FormTitle
        {
            set { this.Text = value; }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    

        
    }
}
