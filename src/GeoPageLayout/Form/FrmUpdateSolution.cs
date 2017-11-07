using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoPageLayout
{
    public partial class FrmUpdateSolution : DevComponents.DotNetBar.Office2007Form
    {
        public FrmUpdateSolution()
        {
            InitializeComponent();
        }
        public string m_Name
        {
            get;
            set;
        }
        public string m_Remark
        {
            get;
            set;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_Name = txtSultionName.Text.Trim ();
            m_Remark = txtRemark.Text.Trim();
            this.Close();
            this.DialogResult = DialogResult.OK;

        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void FrmUpdateSolution_Load(object sender, EventArgs e)
        {
            txtRemark.Text  = m_Remark;
            txtSultionName.Text  = m_Name;
        }
    }
}
