using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoSysUpdate
{
    public partial class frmResultsTreeName : Form
    {
        public frmResultsTreeName()
        {
            InitializeComponent();
        }
        ///设置窗体名称
        public string femText
        {
            set
            {
                this.Text = value;
            }
        }
        ///设置Lab名称
        public string LabText
        {
            set
            {
                lab.Text = value;
            }
        }
        public string ResultsTreeName
        {
            get
            {
                return txtName.Text;
            }
        }

        private void bttOk_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "") return;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCanlce_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtName.Text == "") return;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
