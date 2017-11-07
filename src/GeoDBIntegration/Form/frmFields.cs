using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDBIntegration
{
    public partial class frmFields : Form
    {
        public frmFields()
        {
            InitializeComponent();
        }

        private List<string> m_lstSourcefields;
        public List<string> lstSourceFields
        {
            set { m_lstSourcefields = value; }
        }

        private List<string> m_lstSourceNames;
        public List<string> lstSourceNames
        {
            set { m_lstSourceNames = value; }
        }

        private List<string> m_lstTagFields;
        public List<string> lstTagFields
        {
            get { return m_lstTagFields; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.lstFields.Items.Count; i++)
            {
                if (this.lstFields.Items[i].Checked == true)
                {
                    m_lstTagFields.Add(m_lstSourceNames[i]);
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void frmFields_Load(object sender, EventArgs e)
        {
            m_lstTagFields = new List<string>();
            this.lstFields.Items.Clear();
            if (m_lstSourcefields == null) return;

            for (int i = 0; i < m_lstSourcefields.Count; i++)
            {
                this.lstFields.Items.Add(m_lstSourcefields[i]);
            }

            for (int i = 0; i < this.lstFields.Items.Count; i++)
            {
                this.lstFields.Items[i].Checked = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnAllselection_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.lstFields.Items.Count; i++)
            {
                this.lstFields.Items[i].Checked = true;
            }
        }

        private void btnReturnselection_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.lstFields.Items.Count; i++)
            {
                if (this.lstFields.Items[i].Checked == true)
                {
                    this.lstFields.Items[i].Checked = false;
                }
                else
                {
                    this.lstFields.Items[i].Checked = true;
                }
            }
        }
    }
}